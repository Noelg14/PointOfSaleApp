using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Services
{
    public class databaseService
    {
        public MySqlConnection _connection { get; set; }
        public MySqlCommand _command { get; set; }
        public databaseService() { 
            _connection = initConn();

            _command = initCmd();
            _command.Connection = _connection;
        }

        public async Task<DataSet> runQuery(string queryToRun)
        {   
            DataSet dataSet = new DataSet();
            if(queryToRun == null || string.IsNullOrEmpty(queryToRun))
            {
                return null;
            }
            if(_connection.State == System.Data.ConnectionState.Open)
            {
                await _connection.CloseAsync();
            }
            _command.CommandText = queryToRun;
            try
            {
                _connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = _command;
                await adapter.FillAsync(dataSet);
                return dataSet;

            }
            catch (Exception ex)
            {
                Demo.Utils.log(ex.Message);
                return null;
            }finally { _connection.Close(); }
        }

        private static MySqlConnection initConn()
        {
            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString = Demo.Utils.getConfig();
            return cnn;
        }
        private static MySqlCommand initCmd()
        {

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = initConn();
            return cmd;
        }

    }
}
