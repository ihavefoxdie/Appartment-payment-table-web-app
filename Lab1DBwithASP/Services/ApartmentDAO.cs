using Lab1DBwithASP.Models;
using MySql.Data.MySqlClient;

namespace Lab1DBwithASP.Services
{
    public class ApartmentDAO : IApartmentDataService
    {
        public string connectionString = "server=localhost;uid=root;pwd=qwerty123;database=balancedb";

        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ApartmentModel GetApartmentById(int id)
        {
            ApartmentModel? apartments = null;
            string sqlStatement = "SELECT * FROM turnoversheet t1 INNER JOIN apartvalues t2 ON t1.id = t2.fk_id_apartment INNER JOIN months t3 ON t2.fk_id_month = t3.id_month WHERE t1.id = @Id";

            using (MySqlConnection connection = new(connectionString))
            {
                MySqlCommand sqlCommand = new(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                try
                {
                    connection.Open();

                    MySqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
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

            if (GetApartmentById((int)model.Id).Id == 0)
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

            string sqlStatement = "INSERT INTO apartvalues (fk_id_month, additional, paid, apartvalues.remaining, fk_id_apartment, apartvalues.year) VALUES (@id_month, @additional, @paid, @remain, @id, @yearnum);";
            string yearStatement = "SELECT year FROM apartvalues t1 WHERE t1.fk_id_apartment = @id order by year desc";
            string monthStatement = "SELECT fk_id_month FROM apartvalues t1 WHERE t1.fk_id_apartment = @id AND t1.year = @currentYear order by fk_id_month desc";


            using MySqlConnection connection = new(connectionString);
            MySqlCommand sqlCommand = new(yearStatement, connection);

            sqlCommand.Parameters.Add("@id", MySqlDbType.UInt32).Value = model.Id;
            try
            {
                int year = 0, month = 0;

                connection.Open();
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                year = (int)reader[0];

                sqlCommand = new(monthStatement, connection);
                reader.Close();
                reader = sqlCommand.ExecuteReader();
                month = (int)reader[0];
                reader.Close();

                string lastMonth = "SELECT remaining FROM apartvalues t1 WHERE t1.fk_id_apartment = @id AND t1.fk_id_month = @lastMonth AND t1.year = @year";

                if (month == 1)
                {

                }
                

                if (month == 12)
                {
                    month = 1;
                    year++;
                }
                sqlCommand.Parameters.Add("@id_month", MySqlDbType.Int32).Value = month;
                sqlCommand.Parameters.Add("@additional", MySqlDbType.Double).Value = model.Additional;
                sqlCommand.Parameters.Add("@paid", MySqlDbType.Double).Value = model.Paid;
                sqlCommand.Parameters.Add("@remain", MySqlDbType.Double).Value = model.Additional + model.First - model.Paid;
                sqlCommand.Parameters.Add("@yearnum", MySqlDbType.Double).Value = (UInt32)year;
                sqlCommand = new(sqlStatement, connection);
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
