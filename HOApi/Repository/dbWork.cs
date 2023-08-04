using MySql.Data.Types;
using MySql.Data.MySqlClient;
using HOApi.Models;

namespace HOApi.Repository
{
    public class dbWork
    {
        public static void addSales(List<Models.sales> sales)
        {
            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = cmd.Connection;

            try
            {
                cnn.Open();
                cmd.Prepare();
                cmd.CommandText = "INSERT INTO Sales values(@Cart,@value,@date)";

                foreach(Models.sales item in sales)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Cart", item.cartID);
                    cmd.Parameters.AddWithValue("@value", double.Parse(item.value));
                    cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(item.date).ToString("yyyy-MM-dd"));

                    cmd.ExecuteNonQuery();
                    //Console.WriteLine("Added line"); 
                }
                Console.WriteLine("Complete");
            }
            
            catch(MySqlException mse)
            {
                Console.WriteLine(mse.Message);
            }

        }
        public static void addSaleLines(List<Models.salesline> salelines)
        {
            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = cmd.Connection;

            try
            {
                cnn.Open();
                cmd.Prepare();
                cmd.CommandText = "INSERT INTO Salelines values(@PLU,@value,@Cart)";

                foreach (Models.salesline item in salelines)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@PLU", item.PLU);
                    cmd.Parameters.AddWithValue("@value", double.Parse(item.value));
                    cmd.Parameters.AddWithValue("@Cart", item.cartID);

                    cmd.ExecuteNonQuery();
                    //Console.WriteLine("Added sale");
                }
                Console.WriteLine("Complete");
            }

            catch (MySqlException mse)
            {
                Console.WriteLine(mse.Message);
            }

        }

        public static void addStock(List<Models.stock> stock)
        {
            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = cmd.Connection;

            try
            {
                truncTable("stockbck"); // clear table
                cnn.Open();
                cmd.CommandText = "INSERT INTO stockbck\r\nSELECT * FROM stocklvl"; // backup existing
                cmd.ExecuteNonQuery();

               
                truncTable("stocklvl"); // clear table


                cmd.Prepare();
                cmd.CommandText = "INSERT INTO stocklvl values(@PLU,@value,curdate())";

                foreach (Models.stock item in stock)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@PLU", item.PLU);
                    cmd.Parameters.AddWithValue("@value", double.Parse(item.qty));
                    //cmd.Parameters.AddWithValue("@Cart", item.cartID);

                    cmd.ExecuteNonQuery();
                    //Console.WriteLine("Added sale");
                }
                Console.WriteLine("Complete");
            }

            catch (MySqlException mse)
            {
                Console.WriteLine(mse.Message);
            }

        }

        private static bool truncTable(string tableName)
        {

            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = cmd.Connection;

            try
            {
                cnn.Open();
                // cmd.Prepare();
                cmd.CommandText = $"TRUNCATE TABLE {tableName}";

                //cmd.Parameters.AddWithValue("@table", tableName);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException msqle)
            {
                return false;
            }
            finally
            {
                cnn.Close();
            }

        }

        public static IReadOnlyList<sales> GetSales()
        {
            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = cmd.Connection;
            List<sales> salesList = new List<sales>();

            try
            {
                cnn.Open();
                cmd.CommandText = "Select * from sales";
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while(dataReader.Read()) 
                {
                    salesList.Add(new sales(
                        dataReader.GetString(0),
                         dataReader.GetDouble(1),
                         dataReader.GetString(2)
                        ));
                }
                return salesList;
            }
            catch (MySqlException mse)
            {
                Console.WriteLine(mse.Message);
                throw mse;
            }
            finally
            {
                cnn.Close();
            }
        }


        #region SQL stuff 
        //Making these public as it will be easier
        public static MySqlConnection initConn()
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString =   "server=127.0.0.1;database=masterdb;uid=noel;pwd=noel;";
            return cnn;
        }
        public static MySqlCommand initCmd()
        {

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = initConn();
            return cmd;
        }

        #endregion

    }
}
