using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Demo.Models;
using MySql.Data.MySqlClient;


namespace Demo.Services
{
    internal class VoucherService
    {
        public static VoucherService Instance = new VoucherService();

        private readonly IDbConnection cnn = Utils.initConn();
        private readonly IDbCommand cmd = Utils.initCmd();

        private VoucherService()
        {
            Instance = this;
        }

        public static Models.Voucher getVoucherDetails(string id)
        {
            VoucherService v = VoucherService.Instance;
            Demo.Models.Voucher vouch = null;
            v.cnn.Open();
            v.cmd.Connection = v.cnn;
            
            v.cmd.CommandText = $"Select * from voucher where Number = '{id}'";
            MySqlDataReader dr = (MySqlDataReader) v.cmd.ExecuteReader(); 
            while(dr.Read())
            {

                vouch = new Models.Voucher(dr.GetString("Number"), dr.GetDouble("Balance"));
            }
            v.cnn.Close();
            dr.CloseAsync();

            return vouch;
        }

        public static string CreateNewVoucher(string id,double bal)
        {
            
            VoucherService v = VoucherService.Instance;
            //Demo.Models.Voucher vouch = null;
            v.cnn.Open();
            v.cmd.Connection = v.cnn;
            string res = "New Voucher Created";
            try
            {
                v.cmd.CommandText = $"Select * from voucher where Number = '{id}'";
                MySqlDataReader dr = (MySqlDataReader)v.cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    res = "Voucher already exists";
                    dr.Dispose();
                }
                else
                {
                    dr.Dispose();
                    try
                    {
                        v.cmd.CommandText = $"INSERT INTO voucher(Number,Balance) VALUES('{id}',{bal})";
                        v.cmd.ExecuteNonQuery();
                    }
                    catch (InvalidOperationException ioe)
                    {
                        res = "An Error Occurred " + ioe.Message;
                    }

                }
            }catch(Exception e)
            {
                Utils.log(e.Message);
            }

            //while (dr.Read())
            //{

            //    vouch = new Models.Voucher(dr.GetString("Number"), dr.GetDouble("Balance"));
            //}
            finally
            {
                v.cnn.Close();
            }
            return res;
        }

        public static double UpdateVoucher(string id,double usageAmt)
        {
            VoucherService v = VoucherService.Instance;
            try
            {
                double oldBalance = checkBalance(id);
                if(oldBalance == -1)
                {
                    return oldBalance;
                }

                v.cnn.Open();
                v.cmd.Connection = v.cnn;

                double curBal = oldBalance - usageAmt;
                if(curBal < 0)
                {
                    return -99;
                }
                v.cmd.CommandText = $"UPDATE voucher set balance={curBal} where Number ='{id}'";
                v.cmd.ExecuteNonQuery();

                v.cmd.CommandText = $"INSERT INTO voucherusage(Number,OldBalance,NewBalance) VALUES ({id},{oldBalance},{curBal});";
                v.cmd.ExecuteNonQuery();


                return curBal;


            }
            catch(Exception e)
            {
                Utils.log(e.Message);
                return -1;
            }
            finally
            {
                v.cnn.Close();
            }

            

        }
        public static double UpdateVoucher(string id, double usageAmt,Cart c)
        {
            VoucherService v = VoucherService.Instance;
            try
            {
                double oldBalance = checkBalance(id);
                if (oldBalance == -1)
                {
                    return oldBalance;
                }

                v.cnn.Open();
                v.cmd.Connection = v.cnn;

                double curBal = oldBalance - usageAmt;
                if (curBal < 0)
                {
                    return -99;
                }
                v.cmd.CommandText = $"UPDATE voucher set balance={curBal} where Number ='{id}'";
                v.cmd.ExecuteNonQuery();

                v.cmd.CommandText = $"INSERT INTO voucherusage(Number,OldBalance,NewBalance,CartID) VALUES ({id},{oldBalance},{curBal},{c.id});";
                v.cmd.ExecuteNonQuery();


                return curBal;


            }
            catch (Exception e)
            {
                Utils.log(e.Message);
                return -1;
            }
            finally
            {
                v.cnn.Close();
            }



        }


        private static double checkBalance(string id)
        {
            VoucherService v = VoucherService.Instance;
            double vouch = -1; 
            v.cnn.Open();
            v.cmd.Connection = v.cnn;

            v.cmd.CommandText = $"Select * from voucher where Number = '{id}'";
            MySqlDataReader dr = (MySqlDataReader)v.cmd.ExecuteReader();
            while (dr.Read())
            {

                vouch = dr.GetDouble("Balance");
            }
            v.cnn.Close();
            dr.CloseAsync();

            return vouch;
        }


    }
}
