using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data;
using MySql.Data.Types;
using System.IO;
using QRCoder;
using ClosedXML.Excel;
using System.Data.SqlClient;
using Demo.Services;
using System.Text.Encodings;
using System.Text;


namespace Demo
{
    public class Utils
    {
        //Get DB Configs
        public static string getConfig()
        {

            if (Debugger.IsAttached)
            {
                //log("Debugger Attached, using debug credentials.");
                return "server=127.0.0.1;database=demo;uid=noel;pwd=noel;";
            }
            string file = //"C:\\test\\config.dat";
            Directory.GetCurrentDirectory() + "/Localdata/config.dat";
            Console.WriteLine(file);
            string conString, server = null, catalog = null, pw = null, user = null;

            if (file == null)
            {
                throw new Exception("An error ocurred");
            }
            if (!File.Exists(file))
            {
                throw new Exception("File does not exist, please ensure file config exists");
            }
            string[] conf = File.ReadAllLines(file);
            for (int i = 0; i < conf.Length; i++) // could be a switch, will look at
            {
                if (conf[i].StartsWith("SERVER"))
                {
                    server = conf[i].Split('=')[1].Trim();
                }
                if (conf[i].StartsWith("DB"))
                {
                    catalog = conf[i].Split('=')[1].Trim();
                }
                if (conf[i].StartsWith("PW"))
                {
                    pw = conf[i].Split('=')[1];
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
        //Get a specific Setting from config file
        public static string getConfig(string config)
        {
            string file;
            //if (Debugger.IsAttached)
            //{
            //     file = "C:\\test\\config.dat";
            //}
            //else
            //{
               file= Directory.GetCurrentDirectory() + "/Localdata/config.dat";

           // }

            if (file == null)
            {
                return "";
                //throw new Exception("An error ocurred");
            }
            if (!File.Exists(file))
            {
                log("File does not exist, please ensure file config exists");
                return "";
            }
            string[] conf = File.ReadAllLines(file);
            for (int i = 0; i < conf.Length; i++) // could be a switch, will look at
            {
                if (conf[i].StartsWith(config))
                {
                   return conf[i].Split('=')[1].Trim();
                }
            }
            return "";
        }
        //Search for product
        public static Product search(string PLU)
        {
            Product product = null;
            string connString = getConfig();
            MySqlConnection cnn = new MySqlConnection(connString);
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            cmd.CommandText = "SELECT * FROM PRODUCT WHERE PLU = '" + PLU + "' or `desc` = '" + PLU + "' LIMIT 1";
            log("Searching for PLU "+PLU);
            try
            {
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Product p = new Product(dr.GetString(0), dr.GetString(1), dr.GetFloat(2), dr.GetBoolean(3),dr.GetChar("Type"));
                    p.qty = 1;
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
        public static string getTypeDesc(char type)
        {
            return "";
        }
        //Get product qty in stock
        public static double getProductQty(string PLU)
        {
            MySqlConnection cnn = initConn();
            MySqlCommand cmd = initCmd();

            cmd.Connection = cnn;
            try
            {
                cnn.Open();
                cmd.Prepare();
                cmd.CommandText = "select * from stocklvl where PLU = @plu";
                cmd.Parameters.AddWithValue("@plu", PLU);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    return dr.GetDouble("QTY");
                }
                else
                {
                    dr.DisposeAsync();
                    cmd.CommandText = $"insert into stocklvl(PLU,QTY) values('{PLU}',0);";
                    cmd.ExecuteNonQuery();
                   // MessageBox.Show("Added Product with QTY 0");
                    return 0;
                }
            }
            catch(MySqlException SQLe)
            {
                MessageBox.Show(SQLe.Message);
            }
            return 0;
        }
        public static void updateProductQty(string PLU, double newQTY)
        {
            MySqlConnection cnn = initConn();
            MySqlCommand cmd = initCmd();

            cmd.Connection = cnn;
            try
            {
                cnn.Open();
                cmd.Prepare();
                cmd.CommandText = "select * from stocklvl where PLU = @plu";
                cmd.Parameters.AddWithValue("@plu", PLU);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Dispose();
                    cmd.Prepare();
                    cmd.CommandText = "update stocklvl set QTY = @qty where PLU = @plu";
                    cmd.Parameters.AddWithValue("@QTY", newQTY);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    dr.DisposeAsync();
                    cmd.CommandText = $"insert into stocklvl(PLU,QTY) values('{PLU}', {newQTY} );";
                    cmd.ExecuteNonQuery();
                    //MessageBox.Show($"Added Product with QTY {newQTY}");
                    //return 0;
                }
            }
            catch (MySqlException SQLe)
            {
                MessageBox.Show(SQLe.Message);
            }
            //return 0;
        }
        public static List<Product> getButtons()
        {
            List<Product> list = new List<Product>();
          
            string connString = getConfig();
            MySqlConnection cnn = new MySqlConnection(connString);
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            cmd.CommandText = "SELECT * FROM PRODUCT WHERE AllFRA = '1' group by PLU";
            log("Searching for Buttons");
            try
            {
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Product p = new Product(dr.GetString(0), dr.GetString(1), dr.GetFloat(2), dr.GetBoolean(3),dr.GetChar("Type"));
                        list.Add(p);
                    }

                }
                else
                {
                    return list;
                }
                return list;
            }
            finally
            {
                cnn.Close();
               
            }


        }
        public static void recSale(Cart c, double total,out Cart cart)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            //Console.WriteLine(date);
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            try
            {
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = cnn;
                cmd.CommandText = "INSERT INTO sales VALUES(" + c.id + "," + total + ",'"+ date+ "',null)";
                Console.WriteLine(cmd.CommandText);
                log("Writing to sales");
                cmd.ExecuteNonQuery();
                recLine(c, cnn,out cart);

            }
            finally
            {
                cnn.Close();
            }
        }
        private static void recLine(Cart c,MySqlConnection cnn,out Cart cart) //does connection close here? 
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            foreach (Product p in c.products)
            {

                // adding * of qty to ensure refunds are recorded right
                cmd.CommandText = "INSERT INTO salelines VALUES('" + p.PLU+ "'," + p.price + ","+c.id+",null)";
                cmd.ExecuteNonQuery();
                log("Writing to salelines");
                // if voucher do this :
                if (p.type == 'G')
                {
                    string vouchRef = VoucherService.getNewVoucherRef().ToString();
                    VoucherService.CreateNewVoucher(vouchRef, p.price);
                    p.sID = vouchRef;
                    log($"adding Voucher");
                }
                //if non stock, do not calculate 
                if (p.type != 'D')
                {
                    updateProductQty(p.PLU, getProductQty(p.PLU) - p.qty);
                    log($"updated stock level of item {p.PLU}");
                }
            }
            cart = c;
        }
        private static string decodeBase64(string inputString)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(inputString));
        }
        private static string encodeBase64(string inputString)
        {
            byte[] stringByteArray = Encoding.ASCII.GetBytes(inputString);
            return Convert.ToBase64String(stringByteArray);
        }
        public static void recPayment(string type,double amount,Cart c)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            cnn.Open();
            try
            {
                cmd.CommandText = "INSERT INTO tender VALUES('"+type+"'," + amount + ","+c.id+ ",'" +date+ "')";
                cmd.ExecuteNonQuery();
                log("Writing to tender");
            }
            finally
            {
                cnn.Close();
            }

        }

        ///// <summary>
        ///// Deprecated - use VoucherService
        ///// </summary>
        ///// <param name="number"></param>
        ///// <returns></returns>
        //public static Voucher GetVoucher(string number)
        //{
        //    MySqlCommand cmd = initCmd();
        //    MySqlConnection conn = initConn();
        //    cmd.Connection = conn;
        //    try{
        //        conn.Open();
        //        cmd.CommandText = $"SELECT * FROM Voucher where Number ='{number}'";
        //        MySqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.HasRows)
        //        {
        //            dr.Read();
        //            return new Voucher(dr.GetString(0), dr.GetDouble(1));
        //        }
        //    }catch(Exception e)
        //    {
                
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //    return new Voucher();
        //}
        ///// <summary>
        ///// Deprecated - use VoucherService
        ///// </summary>
        ///// <param name="number"></param>
        ///// <param name="balance"></param>
        //public static void UpdateVoucher(string number,double balance)
        //{
        //    MySqlCommand cmd = initCmd();
        //    MySqlConnection conn = initConn();
        //    cmd.Connection = conn;
        //    try
        //    {
        //        conn.Open();
        //        cmd.CommandText = $"Update Voucher set balance = {balance} where Number ='{number}'";
        //        int rows = cmd.ExecuteNonQuery();
                
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //}


        public static void log(string msg)
        {


            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = initConn();
            cmd.Connection = cnn;
            cnn.Open();
            msg.Replace("'", "");
            try
            {
                cmd.CommandText = $"INSERT INTO log VALUES(NULL,'{msg}','"+DateTime.UtcNow.ToString()+"');";
                cmd.ExecuteNonQuery();
            }
            finally
            {
                cnn.Close();
            }
        }
        public static List<double> getSalesDouble()
        {
            List<double> sales = new List<double>();
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            cnn.Open();
            try
            {

                cmd.CommandText = "SELECT SUM(VALUE) AS 'Sales' FROM sales GROUP BY DATE ORDER BY DATE desc";
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    double s;
                    s = (double)dr.GetFloat("Sales");
                    sales.Add(s);
                }
            }
            finally
            {
                cnn.Close();
            }
            return sales;
        }
        public static List<string> getSalesDates()
        {
            List<string> dates = new List<string>();
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            cnn.Open();
            try
            {

                cmd.CommandText = "Select Date from sales group by Date order by Date desc";
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string s;
                    s = dr.GetString("Date");
                    dates.Add(s);
                }
            }
            finally
            {
                cnn.Close();
            }
            return dates;
        }
        public static List<Product> getProductData() // doesnt get product data?
        {
            List<Product> productsToSend = new List<Product>();
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;

            try
            {
                cnn.Open();
                cmd.CommandText = "Select * from products ";
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    productsToSend.Add(
                        new Product(
                            dr.GetString(0),  //PLU
                            dr.GetString(1), //Desc
                            dr.GetFloat(2), //Price
                            dr.GetBoolean(3), //Button?
                            dr.GetChar(4)) // Type
                    );
                }
            }
            finally
            {
                cnn.Close();
            }
            return productsToSend;
        }

        #region SQL stuff 
        //Making these public as it will be easier
        public static MySqlConnection initConn()
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            return cnn;
        }
        public static MySqlCommand initCmd()
        {

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection=initConn();
            return cmd;
        }
        #endregion
    }
}
