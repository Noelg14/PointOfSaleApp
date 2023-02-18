using HOApi.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace HOApi.Repository
{
    public class ProductRepo
    {
        static MySqlConnection _cnn = dbWork.initConn();
        static MySqlClientFactory _clientFactory = MySqlClientFactory.Instance;

        public static Product getProductByPLU(string id)
        {
            IDbCommand command = _clientFactory.CreateCommand();
            command.Connection = _cnn;
            Product resultProduct = new();

            if (_cnn.State == ConnectionState.Open)
            {
                _cnn.Close();
            }
            try
            {
                _cnn.Open();
                command.CommandText = $"SELECT p.plu,p.desc,p.price,p.allfra,p.type FROM product p  where p.plu='{id}'";

                IDataReader dataTable = command.ExecuteReader();
                while (dataTable.Read())
                {
                    resultProduct = new Product(dataTable.GetString(0), dataTable.GetString(1), dataTable.GetFloat(2), dataTable.GetBoolean(3), dataTable.GetChar(4));

                }

                return resultProduct;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                _cnn.Close();
            }
        }
        public static IEnumerable<Product> getProducts() {
            IDbCommand command = _clientFactory.CreateCommand();
            command.Connection= _cnn;
            List<Product> products = new List<Product>();

            if(_cnn.State == ConnectionState.Open)
            {
                _cnn.Close();
            }
            try
            {
                _cnn.Open();
                command.CommandText = "SELECT p.plu,p.desc,p.price,p.allfra,p.type FROM product p";

                IDataReader dataTable = command.ExecuteReader();
                while (dataTable.Read())
                {
                    products.Add(new Product(dataTable.GetString(0), dataTable.GetString(1), dataTable.GetFloat(2), (dataTable.GetChar(3) == 'N' ? true : false), dataTable.GetChar(4))) ;
                }

                return products;
            }catch(Exception ex)
            {
                return products;
            }
            finally
            {
                _cnn.Close();
            }
        }

        public static bool createProduct(Product p)
        {
            IDbCommand command = _clientFactory.CreateCommand();
            command.Connection = _cnn;
            Product resultProduct = new();

            if (_cnn.State == ConnectionState.Open)
            {
                _cnn.Close();
            }
            try
            {
                _cnn.Open();
                command.CommandText = $"INSERT INTO PRODUCT VALUES('{p.PLU}','{p.desc}',{p.price},{p.allowFra},'{p.type}')";

                int res = command.ExecuteNonQuery();


                return res == 1 ? true:false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _cnn.Close();
            }
        }
    }
}
