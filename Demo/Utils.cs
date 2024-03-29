﻿using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data;
using MySql.Data.Types;
using System.IO;
using QRCoder;
using ClosedXML.Excel;
using System.Data.SqlClient;
using Demo.Services;

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
                throw new Exception("An error ocurred");
            }
            if (!File.Exists(file))
            {
                log("File does not exist, please ensure file config exists");
                return "N";
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
        #region Settings
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
        public static List<string> getTypes()
        {
            List< string> types = new List<string>();
            MySqlConnection cnn = initConn();
            MySqlCommand cmd = initCmd();
            cmd.Connection = cnn;
            cmd.CommandText = "Select * from types";
            cnn.Open();
            try
            {


                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    types.Add(dr.GetString("Desc"));
                }
                return types;
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
            finally
            {
                cnn.Close();
            }
            return null;
        }
        public static Dictionary<string,char> getTypeDict()
        {
            Dictionary<string, char> dict = new Dictionary<string, char>();
            MySqlConnection cnn = initConn();
            MySqlCommand cmd = initCmd();
            cmd.Connection = cnn;
            cmd.CommandText = "Select * from types";
            cnn.Open();
            try
            {


                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dict.Add(dr.GetString("Desc"), dr.GetString("Key").ToCharArray()[0]);
                }
                return dict;
            }
            catch (Exception ex)
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
                return "0";

            }
            finally
            {
                cnn.Close();
            }

        }
        public static List<string> getExports()
        {

            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = cmd.Connection;
            //string data;
            List<string> names = new List<string>();

            try
            {
                cnn.Open();
                //cmd.Prepare();
                cmd.CommandText = "select * from settings where Type = 'export'";
                //cmd.Parameters.AddWithValue("@key", key.ToString());

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    names.Add(dr.GetString("setting"));
                    
                }
                return names;

            }
            finally
            {
                cnn.Close();
            }

        }
        #endregion
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
        #region Excel
        public static void ExcelExport<T>(List<T> data,string optionalType="Sales")
        {
            string fileName = "Report1";
            if (File.Exists(fileName + ".xlsx"))
            {
                try
                {
                    File.Delete(fileName + ".xlsx");
                }
                catch (Exception e)
                {
                    log("Error ocurred when deleting file");
                    MessageBox.Show(e.Message, "File in use, Please close and Try Again");
                    return;
                }

            }


            XLWorkbook book = new XLWorkbook();
            var ws = book.Worksheets.Add("Sheet1");
            ws.Row(1).Style.Fill.SetBackgroundColor(XLColor.SteelBlue);
            ws.Cell("A1").RichText.SetFontColor(XLColor.White);

            ws.Cell("A1").Value =optionalType;
            int i = 2;
            foreach (T value in data)
            {
                ws.Cell(i, 1).Value=value;
                i++;
            }

            SaveFileDialog s = new SaveFileDialog();
            s.FileName = fileName + ".xlsx";
            s.InitialDirectory = Directory.GetCurrentDirectory();
            s.Filter = "Excel Files (*.xlsx,*.xls)|*.xls;*.xlsx";

            if (s.ShowDialog() == DialogResult.OK)
            {
                string file = s.FileName;
                if(!file.Contains(".xls"))
                {
                    file += ".xlsx";
                }

                book.SaveAs(file);
                MessageBox.Show("Exported as " + file, "Export Sucessful");
            }

           // book.SaveAs(fileName + ".xlsx");

        }
        //generic as can be used for double,double / string/string  
        //wouldnt be ideal for like Cart.Products,double but can look at
        public static void ExcelExport<T,E>(List<T> maindata,List<E> otherData,string optionalMain ="Date",string optionalOther ="Sale Value")
        {
            string fileName = "Report1";

            if (File.Exists(fileName + ".xlsx"))
            {
                try
                {
                    File.Delete(fileName + ".xlsx");
                }catch(Exception e)
                {
                    log("Error ocurred when deleting file");
                    MessageBox.Show(e.Message, "File in use, Please close and Try Again");

                    return ;
                }

            }


            XLWorkbook book = new XLWorkbook();
            var ws = book.Worksheets.Add("Sheet1");

            var header = ws.Range("A1:B1");


            header.Style.Fill.SetBackgroundColor(XLColor.SteelBlue);

            ws.Cell("A1").Value = optionalMain;
            ws.Cell("B1").Value = optionalOther;

            ws.Cell("A1").RichText.SetFontColor(XLColor.White);
            ws.Cell("B1").RichText.SetFontColor(XLColor.White);

            ws.Column(1).Width = 10;
            ws.Column(2).Style.NumberFormat.Format= "€ #,##0.00";
            ws.Column(2).Width=10;

            int row = 2; 
            //start at second row, Might be cleaner to do a .Skip(), will look at.
            foreach (T value in maindata)
            {
                ws.Cell(row, 1).Value = value;
                ws.Cell(row, 2).Value = otherData[maindata.IndexOf(value)];
                row++;
            }

            SaveFileDialog s = new SaveFileDialog();
            s.FileName = fileName + ".xlsx";
            s.InitialDirectory = Directory.GetCurrentDirectory();
            s.DefaultExt = ".xlsx";
            s.Filter = "Excel Files (*.xlsx,*.xls)|*.xls;*.xlsx";

            if (s.ShowDialog() == DialogResult.OK)
            {
                fileName = s.FileName;
                if (!fileName.Contains(".xls"))
                {
                    fileName += ".xlsx";
                }
                book.SaveAs(fileName);
                MessageBox.Show("Exported as " + fileName, "Export Sucessful");
                string fileToOpen = fileName.Split(Directory.GetCurrentDirectory())[1].Substring(1);
                //MessageBox.Show(fileName.Split(Directory.GetCurrentDirectory())[1].Substring(1));
                //Process.Start(@fileName);
                //Process.Start(fileToOpen);

                //Process p = new Process();
                //p.StartInfo.FileName = "excel";
                //p.StartInfo.Arguments = fileToOpen;

            }
            else
            {
               
            }

            // book.SaveAs(fileName + ".xlsx");

        }
        #endregion
        // export based on DB saved query. This is a catch all for any excel export.
        public static void GeneralExport(string type)
        {

            MySqlCommand cmd = initCmd();
            MySqlConnection cnn = cmd.Connection;
            string data;
            try
            {
                cnn.Open();
                cmd.Prepare();
                cmd.CommandText = "select * from settings where setting = @key and Type='export'";
                cmd.Parameters.AddWithValue("@key", type.ToString());

                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (!dr.HasRows)
                {
                    MessageBox.Show("No Such export exists, please try again");
                    return;
                }
                data = dr.GetString("data"); // get query

                dr.Dispose();
                cmd.CommandText = data;


                dr = cmd.ExecuteReader();
                excelFunctions.createExcel(dr);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                cnn.Close();
            }
        }

        public static DataTable GetExportData(string type)
            {

                MySqlCommand cmd = initCmd();
                MySqlConnection cnn = cmd.Connection;
                string data;
                try
                {
                    cnn.Open();
                    cmd.Prepare();
                    cmd.CommandText = "select * from settings where setting = @key and Type='export'";
                    cmd.Parameters.AddWithValue("@key", type.ToString());

                    MySqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    if (!dr.HasRows)
                    {
                        MessageBox.Show("No Such export exists, please try again");
                        return new DataTable();
                    }
                    data = dr.GetString("data"); // get query

                    dr.Dispose();
                    cmd.CommandText = data;
                    DataTable dt = new DataTable();

                    dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    return dt;
                // excelFunctions.createExcel(dr);

            }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return new DataTable();
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
