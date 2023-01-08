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

        public int Edit(ApartmentModel apartment)
        {
            string sqlStatement_1 = "SELECT * FROM apartvalues WHERE fk_id_apartment = @Id AND year = @yearNum AND fk_id_month = @monthNum;";
            string sqlStatement_2 = "UPDATE apartvalues SET remaining = remaining + @newValue WHERE fk_id_apartment = @Id AND year = @yearNum AND fk_id_month >= @monthNum;";
            string sqlStatement_3 = "UPDATE apartvalues SET remaining = remaining + @newValue WHERE fk_id_apartment = @Id AND year > @yearNum;";

            using (MySqlConnection connection = new(connectionString))
            {
                try
                {
                    MySqlCommand mySqlCommand = new(sqlStatement_1, connection);
                    mySqlCommand.Parameters.AddWithValue("@Id", apartment.Id);
                    mySqlCommand.Parameters.AddWithValue("@yearNum", apartment.Year);
                    mySqlCommand.Parameters.AddWithValue("@monthNum", apartment.Month);

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
                    reader.Close();

                    double oldVal = OldApartment.Additional - OldApartment.Paid;
                    double newVal = apartment.Additional - apartment.Paid;
                    newVal -= oldVal;

                    mySqlCommand = new(sqlStatement_2, connection);
                    mySqlCommand.Parameters.AddWithValue("@Id", apartment.Id);
                    mySqlCommand.Parameters.AddWithValue("@yearNum", apartment.Year);
                    mySqlCommand.Parameters.AddWithValue("@monthNum", apartment.Month);
                    mySqlCommand.Parameters.AddWithValue("@newValue", apartment.Month);

                    mySqlCommand.ExecuteNonQuery();

                    mySqlCommand = new(sqlStatement_3, connection);
                    mySqlCommand.Parameters.AddWithValue("@Id", apartment.Id);
                    mySqlCommand.Parameters.AddWithValue("@yearNum", apartment.Year);
                    mySqlCommand.Parameters.AddWithValue("@newValue", apartment.Month);

                    mySqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }

                return 1;
            }
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
                        First = (double)reader[1],
                        MonthId = (int)reader[2],
                        Additional = (double)reader[3],
                        Paid = (double)reader[4],
                        Left = (double)reader[5],
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

        public List<ApartmentModel> GetApartments()
        {
            List<ApartmentModel> apartments = new();
            string sqlStatement = "SELECT * FROM turnoversheet t1 INNER JOIN apartvalues t2 ON t1.id = t2.fk_id_apartment INNER JOIN months t3 ON t2.fk_id_month = t3.id_month";

            using (MySqlConnection connection = new(connectionString))
            {
                MySqlCommand sqlCommand = new(sqlStatement, connection);
                try
                {
                    connection.Open();

                    MySqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        apartments.Add(new ApartmentModel
                        {
                            Id = (UInt32)reader[0],
                            First = (double)reader[1],
                            MonthId = (int)reader[2],
                            Additional = (double)reader[3],
                            Paid = (double)reader[4],
                            Left = (double)reader[5],
                            Year = (UInt32)reader[7],
                            Month = (string)reader[10]
                        });
                    }
                    reader.Close();
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
                sqlCommand.Parameters.Add("@inSaldo", MySqlDbType.Double).Value = model.First;
                sqlCommand.Parameters.Add("@id_month", MySqlDbType.Int32).Value = 1;
                sqlCommand.Parameters.Add("@additional", MySqlDbType.Double).Value = model.Additional;
                sqlCommand.Parameters.Add("@paid", MySqlDbType.Double).Value = model.Paid;
                sqlCommand.Parameters.Add("@remain", MySqlDbType.Double).Value = model.Additional + model.First - model.Paid;
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
            int idNumber = -1;

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
                double Debt = 0;

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
                Debt = (double)reader[0];
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
                sqlCommand.Parameters.Add("@additional", MySqlDbType.Double).Value = model.Additional;
                sqlCommand.Parameters.Add("@paid", MySqlDbType.Double).Value = model.Paid;
                sqlCommand.Parameters.Add("@remain", MySqlDbType.Double).Value = model.Additional + Debt - model.Paid;
                sqlCommand.Parameters.Add("@year", MySqlDbType.Double).Value = year;
                sqlCommand.ExecuteNonQuery();
                return idNumber = (int)model.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                return idNumber;
            }
        }
    }
}
