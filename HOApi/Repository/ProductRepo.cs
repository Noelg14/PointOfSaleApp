using HOApi.Models;
using MySql.Data.MySqlClient;


namespace HOApi.Repository
{
    public class ProductRepo
    {
        static MySqlConnection _cnn = dbWork.initConn();
        static MySqlClientFactory _clientFactory = MySqlClientFactory.Instance;

        public static Product getProductByPLU(string id)
        { 
            return new Product();
        }


    }
}
