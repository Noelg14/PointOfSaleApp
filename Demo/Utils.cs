using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Demo
{
    internal class Utils
    {
        public static string getConfig()
        {
            string file = "C:\\test\\poscfg.dat";
            //Directory.GetCurrentDirectory() + "/Localdata/poscfg.dat";
            Console.WriteLine(file);
            string conString, server = null, catalog = null, pw = null, user = null;

            if (file == null)
            {
                throw new Exception("An error ocurred");
            }
            if (!File.Exists(file))
            {
                throw new Exception("File does not exist, please ensure file poscfg exists");
            }
            string[] conf = File.ReadAllLines(file);
            for (int i = 0; i < conf.Length; i++)
            {
                if (conf[i].StartsWith("SERVER"))
                {
                    server = conf[i].Split('=')[1].Trim();
                }
                if (conf[i].StartsWith("CATALOG"))
                {
                    catalog = conf[i].Split('=')[1].Trim();
                }
                if (conf[i].StartsWith("PW"))
                {
                    pw = conf[i].Split('=')[1].Trim();
                }
                if (conf[i].StartsWith("USER"))
                {
                    user = conf[i].Split('=')[1].Trim();
                }
            }
            if (server is null || catalog is null || user is null)
            {
                throw new Exception("Error getting configs");
            }
            return conString = "server=" + server + ";database=" + catalog + ";uid=" + user + ";pwd=" + pw + ";";
        }
        public static Product search(string PLU)
        {
            Product product = null;
            string connString = getConfig();
            MySqlConnection cnn = new MySqlConnection(connString);
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            cmd.CommandText = "SELECT * FROM PRODUCT WHERE PLU = '" + PLU + "' LIMIT 1";
            log("Searching for PLU"+PLU);
            try
            {
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Product p = new Product(dr.GetString(0), dr.GetString(1), dr.GetFloat(2), dr.GetBoolean(3));
                    return p;
                }
                else
                {
                    return product;
                }
            }
            finally
            {
                cnn.Close();
            }


        }
        public static void recSale(Cart c, double total)
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            try
            {
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = cnn;
                cmd.CommandText = "INSERT INTO sales VALUES(" + c.id + "," + total + ")";
                Console.WriteLine(cmd.CommandText);
                log("Writing to sales");
                cmd.ExecuteNonQuery();
                recLine(c, cnn);

            }
            finally
            {
                cnn.Close();
            }
        }
        private static void recLine(Cart c,MySqlConnection cnn)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            foreach (Product p in c.products)
            {
                cmd.CommandText = "INSERT INTO salelines VALUES('" + p.PLU+ "'," + p.price + ","+c.id+")";

                cmd.ExecuteNonQuery();
                log("Writing to salelines");
            }
            return;
        }
        public static void recPayment(string type,double amount,Cart c)
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            cnn.Open();
            try
            {
                cmd.CommandText = "INSERT INTO tender VALUES('"+type+"'," + amount + ","+c.id+")";
                cmd.ExecuteNonQuery();
                log("Writing to tender");
            }
            finally
            {
                cnn.Close();
            }

        }
        public static void log(string msg)
        {
            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = initConn();
            cmd.Connection = cnn;
            cnn.Open();
            try
            {
                cmd.CommandText = "INSERT INTO log VALUES(NULL,'"+msg+"','"+DateTime.UtcNow.ToString()+"');";
                cmd.ExecuteNonQuery();
            }
            finally
            {
                cnn.Close();
            }
        }

        private static MySqlConnection initConn()
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            return cnn;
        }
        private static MySqlCommand initCmd()
        {

            MySqlCommand cmd = new MySqlCommand();
            return cmd;
        }
    }
}
