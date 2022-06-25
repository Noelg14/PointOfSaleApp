using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.IO;
using QRCoder;
//using C1.C1Excel;
using ClosedXML.Excel;

namespace Demo
{
    internal class Utils
    {
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
            log("Searching for PLU "+PLU);
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
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            //Console.WriteLine(date);
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            try
            {
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = cnn;
                cmd.CommandText = "INSERT INTO sales VALUES(" + c.id + "," + total + ",'"+ date+ "')";
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
        private static void recLine(Cart c,MySqlConnection cnn) //does connection close here? 
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
        public static List<string[]> getSales()
        {
            List<string[]> sales = new List<string[]>();
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;
            cnn.Open();
            try
            {
               
                cmd.CommandText = "Select * from sales";
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string[] s= new string[100];
                    for (int i= 0; i <= dr.FieldCount -1 ;i++)
                    {
                        s[i] = dr.GetValue(i).ToString();
                    }
                    sales.Add(s);
                }
            }
            finally
            {
                cnn.Close();
            }
            return sales;
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

                cmd.CommandText = "Select Date from sales group by Date order by Date desc limit 5";
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

        public static List<Product> getProductData()
        {
            List<Product> dates = new List<Product>();
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnn;

            try
            {
                cnn.Open();
                cmd.CommandText = "Select Date from sales group by Date order by Date desc limit 5";
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                }
            }
            finally
            {
                cnn.Close();
            }
            return dates;
        }

        //Settings
        public static Dictionary<string,string> getSettings()
        {
            Dictionary<string, string> kv = new Dictionary<string, string>();
            MySqlConnection cnn = initConn();
            MySqlCommand cmd = initCmd();
            cmd.Connection = cnn;
            cmd.CommandText = "Select * from settings";
            cnn.Open();
            try
            {
             

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    kv.Add(dr.GetString("setting"), dr.GetString("data"));
                }
                return kv;
            }
            catch(Exception ex)
            {
                log(ex.Message);
            }
            finally
            {
                cnn.Close();
            }
            return null;
        }
        public static void updateSettings(string name,string data)
        {
         
            MySqlConnection cnn = initConn();
            MySqlCommand cmd = initCmd();
            cmd.Connection= cnn;
            try
            {
                cnn.Open();
                cmd.Prepare();
                cmd.CommandText = "update settings set data=@data where setting=@name";
                cmd.Parameters.AddWithValue("@data", data.ToString());
                cmd.Parameters.AddWithValue("@name", name.ToString());

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
            finally
            {
                cnn.Close();
            }
        }

        public static string getIndiviudalSetting(string key)
        {

            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = cmd.Connection;
            string data;
            try
            {
                cnn.Open();
                cmd.Prepare();
                cmd.CommandText = "select * from settings where setting = @key";
                cmd.Parameters.AddWithValue("@key", key.ToString());

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    data = dr.GetString("data");
                    return data;
                }
                return null;

            }
            finally
            {
                cnn.Close();
            }

        }
        
        // QR Code Stuff
        public static Bitmap genQR(string data)
        {
            string qrData = getIndiviudalSetting("url") +"?data="+ data;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.H );
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeAsBitmap = qrCode.GetGraphic(5);
            return qrCodeAsBitmap;
        }

        //public static void ExcelTest()
        //{
        //    C1XLBook book = new C1XLBook();
        //    book.Author = "Noel Griffin";
        //    XLSheet sheet = book.Sheets[0];
        //    int i;
        //    for (i = 0; i <= 9; i++)
        //    {
        //        sheet[i, 0].Value = (i + 1) * 10;
        //        sheet[i, 1].Value = (i + 1) * 100;
        //        sheet[i, 2].Value = (i + 1) * 1000;
        //    }
        //    book.Save("MyBook.xlsx");
        //}
        
        //public static void ClosedXMLTest()
        //{
        //    string fileName = "Report1";
        //    if (File.Exists(fileName+".xlsx"))
        //    {
        //        File.Delete(fileName + ".xlsx");
        //    }

        //    XLWorkbook book = new XLWorkbook();
        //    var ws = book.Worksheets.Add("Sheet1");
        //    ws.Cell("A1").Value = "TEST";
        //    ws.Cell("A2").Value = "Value";
        //    book.SaveAs(fileName + ".xlsx");

        //}
        public static void ExcelExport<T>(List<T> sales)
        {
            string fileName = "Report1";
            if (File.Exists(fileName + ".xlsx"))
            {
                File.Delete(fileName + ".xlsx");
            }


            XLWorkbook book = new XLWorkbook();
            var ws = book.Worksheets.Add("Sheet1");
            ws.Cell("A1").Value = "SALES";
            int i = 1;
            foreach (T value in sales)
            {
                ws.Cell(i, 1).Value=value;
                i++;
            }

            // Header
            book.SaveAs(fileName + ".xlsx");

        }

        /*
         * SQL stuff
         */
        private static MySqlConnection initConn()
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = getConfig();
            return cnn;
        }
        private static MySqlCommand initCmd()
        {

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection=initConn();
            return cmd;
        }         
    }
}
