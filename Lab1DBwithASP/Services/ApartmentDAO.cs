using Lab1DBwithASP.Models;
using MySql.Data.MySqlClient;

namespace Lab1DBwithASP.Services
{
    public class ApartmentDAO : IApartmentDataService
    {
        public string connectionString = "server=localhost;uid=root;pwd=qwerty123;database=balancedb";

        public int Delete(int id)
        {
            string sqlStatement = "DELETE FROM apartvalues WHERE fk_id_apartment = @Id;\r\nDELETE FROM turnoversheet WHERE id = @Id;";

            using (MySqlConnection connection = new(connectionString))
            {
                MySqlCommand sqlCommand = new(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                try
                {
                    connection.Open();

                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    return id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return 0;
        }

        public int DeleteEntry(int id)
        {
            string sqlStatement = "DELETE FROM apartvalues WHERE fk_id_apartment = @Id AND year = @yearNum AND fk_id_month = @monthNum";
            string yearStatement = "SELECT year FROM apartvalues t1 WHERE t1.fk_id_apartment = @id order by year desc";
            string monthStatement = "SELECT fk_id_month FROM apartvalues t1 WHERE t1.fk_id_apartment = @id AND t1.year = @yearNum order by fk_id_month desc";

            using (MySqlConnection connection = new(connectionString))
            {
                try
                {
                    MySqlCommand sqlCommand = new(yearStatement, connection);
                    sqlCommand.Parameters.AddWithValue("@Id", (UInt32)id);
                    

                    connection.Open();
                    MySqlDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();
                    UInt32 year = (UInt32)reader[0];
                    reader.Close();

                    sqlCommand = new(monthStatement, connection);
                    sqlCommand.Parameters.AddWithValue("@Id", (UInt32)id);
                    sqlCommand.Parameters.AddWithValue("@yearNum", year);

                    reader = sqlCommand.ExecuteReader();
                    reader.Read();
                    int month = (int)reader[0];
                    reader.Close();

                    sqlCommand = new(sqlStatement, connection);
                    sqlCommand.Parameters.AddWithValue("@Id", (UInt32)id);
                    sqlCommand.Parameters.AddWithValue("@yearNum", year);
                    sqlCommand.Parameters.AddWithValue("@monthNum", month);

                    sqlCommand.ExecuteNonQuery();

                    connection.Close();

                    return 1;

                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            return -1;
        }

        public int Edit(ApartmentModel apartment)
        {
            //string sqlStatement_1 = "SELECT * FROM apartvalues WHERE fk_id_apartment = @Id AND year = @yearNum AND fk_id_month = @monthNum;";
            string sqlStatement_2 = "UPDATE apartvalues SET remaining = remaining + @newValue WHERE fk_id_apartment = @Id AND year = @yearNum AND fk_id_month >= @monthNum;";
            string sqlStatement_3 = "UPDATE apartvalues SET remaining = remaining + @newValue WHERE fk_id_apartment = @Id AND year > @yearNum;";
            string sqlStatement_4 = "UPDATE apartvalues SET additional = @add, paid = @paid WHERE fk_id_apartment = @Id AND year = @yearNum AND fk_id_month = @monthNum;";

            using (MySqlConnection connection = new(connectionString))
            {
                try
                {
                    
                    /*mySqlCommand.Parameters.AddWithValue("@Id", apartment.Id);
                    mySqlCommand.Parameters.AddWithValue("@yearNum", apartment.Year);
                    mySqlCommand.Parameters.AddWithValue("@monthNum", apartment.MonthId);

                    connection.Open();
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();

                    reader.Read();

                    ApartmentModel OldApartment = new ApartmentModel
                    {
                        Id = (UInt32)reader[0],
                        First = (double)reader[1],
                        MonthId = (int)reader[2],
                        Additional = (double)reader[3],
                        Paid = (double)reader[4],
                        Left = (double)reader[5],
                        Year = (UInt32)reader[7],
                        Month = (string)reader[10]
                    };
                    reader.Close();*/

                    ApartmentModel OldApartment = GetApartmentById((int)apartment.Id, (int)apartment.Year, apartment.MonthId);

                    decimal oldVal = OldApartment.Additional - OldApartment.Paid;
                    decimal newVal = apartment.Additional - apartment.Paid;
                    newVal -= oldVal;

                    //mySqlCommand = new(sqlStatement_1, connection);
                    MySqlCommand mySqlCommand = new(sqlStatement_2, connection);
                    mySqlCommand.Parameters.AddWithValue("@Id", apartment.Id);
                    mySqlCommand.Parameters.AddWithValue("@yearNum", apartment.Year);
                    mySqlCommand.Parameters.AddWithValue("@monthNum", apartment.MonthId);
                    mySqlCommand.Parameters.AddWithValue("@newValue", newVal);

                    connection.Open();
                    mySqlCommand.ExecuteNonQuery();

                    mySqlCommand = new(sqlStatement_3, connection);
                    mySqlCommand.Parameters.AddWithValue("@Id", apartment.Id);
                    mySqlCommand.Parameters.AddWithValue("@yearNum", apartment.Year);
                    mySqlCommand.Parameters.AddWithValue("@newValue", newVal);

                    mySqlCommand.ExecuteNonQuery();

                    mySqlCommand = new(sqlStatement_4, connection);
                    mySqlCommand.Parameters.AddWithValue("@Id", apartment.Id);
                    mySqlCommand.Parameters.AddWithValue("@yearNum", apartment.Year);
                    mySqlCommand.Parameters.AddWithValue("@monthNum", apartment.MonthId);
                    mySqlCommand.Parameters.AddWithValue("@add", apartment.Additional);
                    mySqlCommand.Parameters.AddWithValue("@paid", apartment.Paid);

                    mySqlCommand.ExecuteNonQuery();

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }

                return 1;
            }
        }

        public List<ApartmentModel> GetApartmentYear(int year, int id)
        {
            string sqlStatement = "SELECT * FROM apartvalues t2 INNER JOIN months t3 ON t2.fk_id_month = t3.id_month INNER JOIN turnoversheet t1 on t2.fk_id_apartment = t1.id WHERE t2.year = @yearNum AND t2.fk_id_apartment = @Id";
            List<ApartmentModel>? apartments = null;

            using (MySqlConnection connection = new(connectionString))
            {
                MySqlCommand sqlCommand = new(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("@yearNum", (UInt32)year);
                sqlCommand.Parameters.AddWithValue("@Id", (UInt32)id);

                connection.Open();
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                apartments = new List<ApartmentModel>();
                while (reader.Read())
                {
                    apartments.Add(new ApartmentModel()
                    {
                        MonthId = (int)reader[0],
                        Additional = (decimal)reader[1],
                        Paid = (decimal)reader[2],
                        Left = (decimal)reader[3],
                        Id = (UInt32)reader[4],
                        Year = (UInt32)reader[5],
                        Month = (string)reader[8],
                        First = (decimal)reader[10]
                    });
                }

            }

            if (apartments != null)
                return apartments;
            return new();
        }


        public List<ApartmentModel> GetApartmentsMonth(int year, int month, int apartStart, int apartEnd)
        {
            string sqlStatement = "SELECT * FROM apartvalues t2 INNER JOIN months t3 ON t2.fk_id_month = t3.id_month INNER JOIN turnoversheet t1 on t2.fk_id_apartment = t1.id WHERE t2.year = @yearNum AND t2.fk_id_month = @monthNum AND t2.fk_id_apartment >= @apartStart AND t2.fk_id_apartment <= @apartEnd";
            List<ApartmentModel>? apartments = null;

            using (MySqlConnection connection = new(connectionString))
            {
                MySqlCommand sqlCommand= new(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("@yearNum", (UInt32)year);
                sqlCommand.Parameters.AddWithValue("@monthNum", month);
                sqlCommand.Parameters.AddWithValue("@apartStart", (UInt32)apartStart);
                sqlCommand.Parameters.AddWithValue("@apartEnd", (UInt32)apartEnd);

                connection.Open();
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                apartments = new List<ApartmentModel>();
                while (reader.Read())
                {
                    apartments.Add(new ApartmentModel()
                    {
                        MonthId = (int)reader[0],
                        Additional = (decimal)reader[1],
                        Paid = (decimal)reader[2],
                        Left = (decimal)reader[3],
                        Id = (UInt32)reader[4],
                        Year = (UInt32)reader[5],
                        Month = (string)reader[8],
                        First = (decimal)reader[10]
                    });
                }

            }

            if (apartments != null)
                return apartments;
            return new();
        }

        public ApartmentModel GetApartmentById(int id, int year, int month)
        {
            ApartmentModel? apartments = null;
            string sqlStatement = "SELECT * FROM turnoversheet t1 INNER JOIN apartvalues t2 ON t1.id = t2.fk_id_apartment INNER JOIN months t3 ON t2.fk_id_month = t3.id_month WHERE t1.id = @Id AND t2.year = @yearNum AND t2.fk_id_month = @monthNum;";

            using (MySqlConnection connection = new(connectionString))
            {
                MySqlCommand sqlCommand = new(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.Parameters.AddWithValue("@yearNum", (UInt32)year);
                sqlCommand.Parameters.AddWithValue("@monthNum", month);
                try
                {
                    connection.Open();

                    MySqlDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    apartments = new ApartmentModel
                    {
                        Id = (UInt32)reader[0],
                        First = (decimal)reader[1],
                        MonthId = (int)reader[2],
                        Additional = (decimal)reader[3],
                        Paid = (decimal)reader[4],
                        Left = (decimal)reader[5],
                        Year = (UInt32)reader[7],
                        Month = (string)reader[10]
                    };

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            if (apartments != null)
                return apartments;
            return new();
            //throw new ArgumentNullException(nameof(apartments), "No elements.\n");
        }

        public List<ApartmentModel> GetApartments(int year)
        {
            List<ApartmentModel> apartments = new();
            string sqlStatement = "SELECT * FROM turnoversheet t1 INNER JOIN apartvalues t2 ON t1.id = t2.fk_id_apartment INNER JOIN months t3 ON t2.fk_id_month = t3.id_month WHERE year = @yearNum";

            using (MySqlConnection connection = new(connectionString))
            {
                MySqlCommand sqlCommand = new(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("@yearNum", (UInt32)year);
                try
                {
                    connection.Open();
                    bool check = false;
                    MySqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        apartments.Add(new ApartmentModel
                        {
                            Id = (UInt32)reader[0],
                            First = (decimal)reader[1],
                            MonthId = (int)reader[2],
                            Additional = (decimal)reader[3],
                            Paid = (decimal)reader[4],
                            Left = (decimal)reader[5],
                            Year = (UInt32)reader[7],
                            Month = (string)reader[10]
                        });
                        check = true;
                    }
                    reader.Close();
                    if(!check)
                        apartments.Add(new ApartmentModel { Year = (UInt32)year });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            var sortedList = apartments.OrderBy(x => x.Id).ToList();
            return sortedList;
        }

        public bool Insert(ApartmentModel model)
        {
            string sqlStatement = "INSERT INTO turnoversheet (id, inSaldo) VALUES (@id, @inSaldo);\r\nINSERT INTO apartvalues (fk_id_month, additional, paid, apartvalues.remaining, fk_id_apartment, apartvalues.year) VALUES (@id_month, @additional, @paid, @remain, @id, @yearnum);";


            using MySqlConnection connection = new(connectionString);
            MySqlCommand sqlCommand = new(sqlStatement, connection);

            if (GetApartmentById((int)model.Id, (int)model.Year, model.MonthId).Id == 0)
            {
                sqlCommand.Parameters.Add("@id", MySqlDbType.UInt32).Value = model.Id;
                sqlCommand.Parameters.Add("@inSaldo", MySqlDbType.Decimal).Value = model.First;
                sqlCommand.Parameters.Add("@id_month", MySqlDbType.Int32).Value = model.MonthId;
                sqlCommand.Parameters.Add("@additional", MySqlDbType.Decimal).Value = model.Additional;
                sqlCommand.Parameters.Add("@paid", MySqlDbType.Decimal).Value = model.Paid;
                sqlCommand.Parameters.Add("@remain", MySqlDbType.Decimal).Value = model.Additional + model.First - model.Paid;
                sqlCommand.Parameters.Add("@yearnum", MySqlDbType.UInt32).Value = model.Year;

                try
                {
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    return false;
                }
            }
            return false;
        }

        public List<ApartmentModel> SearchApartments(string query)
        {
            throw new NotImplementedException();
        }

        public int Update(ApartmentModel model)
        {
            int returnNumber = -1;

            string sqlStatement = "INSERT INTO apartvalues (fk_id_month, additional, paid, apartvalues.remaining, fk_id_apartment, apartvalues.year) VALUES (@id_month, @additional, @paid, @remain, @id, @year);";
            string yearStatement = "SELECT year FROM apartvalues t1 WHERE t1.fk_id_apartment = @id order by year desc";
            string monthStatement = "SELECT fk_id_month FROM apartvalues t1 WHERE t1.fk_id_apartment = @id AND t1.year = @year order by fk_id_month desc";


            using MySqlConnection connection = new(connectionString);
            MySqlCommand sqlCommand = new(yearStatement, connection);

            sqlCommand.Parameters.Add("@id", MySqlDbType.UInt32).Value = model.Id;
            try
            {
                UInt32 year = 0;
                int month = 0;
                decimal Debt = 0;

                connection.Open();
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                reader.Read();
                year = (UInt32)reader[0];
                reader.Close();

                sqlCommand = new(monthStatement, connection);
                sqlCommand.Parameters.Add("@id", MySqlDbType.UInt32).Value = model.Id;
                sqlCommand.Parameters.Add("@year", MySqlDbType.UInt32).Value = year;
                reader = sqlCommand.ExecuteReader();
                reader.Read();
                month = (int)reader[0];
                reader.Close();

                string lastDebt = "SELECT remaining FROM apartvalues t1 WHERE t1.fk_id_apartment = @id AND t1.year = @year AND t1.fk_id_month = @id_month";
                sqlCommand = new(lastDebt, connection);
                sqlCommand.Parameters.Add("@id", MySqlDbType.UInt32).Value = model.Id;
                sqlCommand.Parameters.Add("@id_month", MySqlDbType.Int32).Value = month;
                sqlCommand.Parameters.Add("@year", MySqlDbType.UInt32).Value = year;
                reader = sqlCommand.ExecuteReader();
                reader.Read();
                Debt = (decimal)reader[0];
                reader.Close();

                if (month == 12)
                {
                    month = 1;
                    year++;
                }
                else
                    month++;

                sqlCommand = new(sqlStatement, connection);
                sqlCommand.Parameters.Add("@id", MySqlDbType.UInt32).Value = model.Id;
                sqlCommand.Parameters.Add("@id_month", MySqlDbType.Int32).Value = month;
                sqlCommand.Parameters.Add("@additional", MySqlDbType.Decimal).Value = model.Additional;
                sqlCommand.Parameters.Add("@paid", MySqlDbType.Decimal).Value = model.Paid;
                sqlCommand.Parameters.Add("@remain", MySqlDbType.Decimal).Value = model.Additional + Debt - model.Paid;
                sqlCommand.Parameters.Add("@year", MySqlDbType.UInt32).Value = year;
                sqlCommand.ExecuteNonQuery();

                returnNumber = (int)year;
                return returnNumber;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                return returnNumber;
            }
        }
    }
}
