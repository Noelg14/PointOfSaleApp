using HOApi.Models;
using MySql.Data.MySqlClient;

namespace HOApi.Repository
{
    public class VoucherRepo
    {
        static MySqlConnection _cnn = dbWork.initConn();
        static MySqlClientFactory _clientFactory = MySqlClientFactory.Instance;

        public static Voucher getVoucher(string id)
        {
            //init cmd and connection
            MySqlCommand cmd = (MySqlCommand)_clientFactory.CreateCommand();
            cmd.Connection= _cnn;

            try
            {
                _cnn.Open();
                cmd.CommandText = $"SELECT * FROM voucher where Number ='{id}' LIMIT 1";
                MySqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    dr.Read();
                    Voucher v = new Voucher(dr.GetString(0), dr.GetDouble(1));
                    dr.Close();
                    return v;
                }
                else
                {
                    return Voucher.GetNewVoucher();
                }

            }
            catch(Exception e)
            {
                return Voucher.GetNewVoucher();
            }
            finally
            {
                _cnn.Close();
            }


        }
        public static Voucher updateBalance(string id, double usage)
        {
            //init cmd and connection
            MySqlCommand cmd = (MySqlCommand)_clientFactory.CreateCommand();
            cmd.Connection = _cnn;

            try
            {
                Voucher v = getVoucher(id);
                _cnn.Open();
                    if (v.Balance < usage)
                    {
                        v.Message = "Cannot update as balance is less than usage";
                        return v;
                    }
                    cmd.CommandText = $"UPDATE Voucher set Balance ={v.Balance - usage} where Number ='{v.Id}'";
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        v.Message = "An issue ocurred when updating";
                        return v;
                    }

                    v.updateBal(usage);
                    v.Message = $"Sucessfully used €{usage.ToString("0.00")}";
                    return v;
            }
            catch (Exception e)
            {
                return Voucher.GetNewVoucher();
            }
            finally
            {
                _cnn.Close();
            }

        }
    }
}
