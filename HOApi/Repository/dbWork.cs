using MySql.Data.Types;
using MySql.Data.MySqlClient;

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
