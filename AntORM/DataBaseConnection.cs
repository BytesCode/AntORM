using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AntORM
{
    public class DataBaseConnection: IDisposable
    {
        public static string ConnectionString = string.Empty;
        

        protected SqlConnection _connection;
        protected SqlConnection connection => _connection ?? (_connection = GetOpenConnection());

        public static SqlConnection GetOpenConnection()
        {
            var connect = new SqlConnection(ConnectionString);
            connect.Open();
            return connect;
        }


        public static SqlCommand GetCommand(string sqlTxt, params SqlParameter[] param)
        {
            var cmd = new SqlCommand(sqlTxt, GetOpenConnection());
            cmd.CommandType = CommandType.Text;
            if (param != null) cmd.Parameters.AddRange(param);
            return cmd;
        }

        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
