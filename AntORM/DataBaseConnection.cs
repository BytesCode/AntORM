using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace AntORM
{
    public class DataBaseConnection: IDisposable
    {
        public static string ConnectionString = "";
        

        protected SqlConnection _connection;
        protected SqlConnection connection => _connection ?? (_connection = GetOpenConnection());

        public static SqlConnection GetOpenConnection()
        {
            var connect = new SqlConnection(ConnectionString);
            connect.Open();
            return connect;
        }



        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
