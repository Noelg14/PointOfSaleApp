using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo;
using DocumentFormat.OpenXml.Drawing.Charts;
using MySql.Data.MySqlClient;
using sendToMaster.Models;
using Product = Demo.Product;

namespace sendToMaster
{
    internal class dbFunctions
    {
        public static List<sales> GetSales()
        {

            MySqlCommand cmd = Utils.initCmd();
            MySqlConnection cnn = cmd.Connection;
            string data;

            List<Models.sales> list = new List<Models.sales>();

            try
            {
                cnn.Open();
                //cmd.Prepare();
                cmd.CommandText = "select * from settings where setting = 'sendSales' and Type='HO'";

                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (!dr.HasRows)
                {
                    MessageBox.Show("No Such export exists, please try again");
                    return new List<Models.sales>();
                }
                data = dr.GetString("data"); // get query

                dr.Dispose();
                cmd.CommandText = data;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Models.sales(dr.GetString("CartID"),dr.GetDouble("value"),dr.GetString("Date")));
                }
                return list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                cnn.Close();
            }

        }

        public static List<salesline> GetSaleslines()
        {

            MySqlCommand cmd = Utils.initCmd();
            MySqlConnection cnn = cmd.Connection;
            string data;

            List<Models.salesline> list = new List<Models.salesline>();

            try
            {
                cnn.Open();
                //cmd.Prepare();
                cmd.CommandText = "select * from settings where setting = 'sendSaleline' and Type='HO'";

                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (!dr.HasRows)
                {
                    MessageBox.Show("No Such export exists, please try again");
                    return new List<Models.salesline>();
                }
                data = dr.GetString("data"); // get query

                dr.Dispose();
                cmd.CommandText = data;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Models.salesline(dr.GetString(0), dr.GetDouble(1), dr.GetString(2)));
                }
                return list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                cnn.Close();
            }

        }

        public static List<stock> GetStock()
        {

            MySqlCommand cmd = Utils.initCmd();
            MySqlConnection cnn = cmd.Connection;
            string data;

            List<Models.stock> list = new List<Models.stock>();

            try
            {
                cnn.Open();
                //cmd.Prepare();
                cmd.CommandText = "select * from settings where setting = 'sendStock' and Type='HO'";

                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (!dr.HasRows)
                {
                    MessageBox.Show("No Such export exists, please try again");
                    return new List<Models.stock>();
                }
                data = dr.GetString("data"); // get query

                dr.Dispose();
                cmd.CommandText = data;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Models.stock(dr.GetString(0), dr.GetString(1)));
                }
                return list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                cnn.Close();
            }

        }

        public static List<Product> GetProducts()
        {
            MySqlCommand cmd = Utils.initCmd();
            MySqlConnection cnn = cmd.Connection;
            string data;

            List<Product> list = new List<Product>();

            try
            {
                cnn.Open();
                //cmd.Prepare();
                cmd.CommandText = "select * from settings where setting = 'sendProd' and Type='HO'";

                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (!dr.HasRows)
                {
                    MessageBox.Show("No Such export exists, please try again");
                    return new List<Product>();
                }
                data = dr.GetString("data"); // get query

                dr.Dispose();
                cmd.CommandText = data;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Product prodToSend = new Product(dr.GetString("PLU"), dr.GetString("Desc"), dr.GetFloat("price"), false, dr.GetChar("ALLFRA"));
                    list.Add(prodToSend);
                }
                return list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                cnn.Close();
            }
        }

        public static int updateSales()
        {
            MySqlCommand cmd = Utils.initCmd();
            MySqlConnection cnn = cmd.Connection;
            cmd.CommandText = "update sales set Sent=1 where Sent is NULL";

            try
            {
                cnn.Open();
                return cmd.ExecuteNonQuery();

            }
            catch (Exception e) { return 0; }
        }
        
        public static int updateSaleLine()
        {
            MySqlCommand cmd = Utils.initCmd();
            MySqlConnection cnn = cmd.Connection;
            cmd.CommandText = "update salelines set Sent=1 where Sent is NULL";

            try
            {
                cnn.Open();
                return cmd.ExecuteNonQuery();

            }
            catch (Exception e) { return 0; }
        }

    }
}
