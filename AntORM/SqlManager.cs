using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AntORM
{
    public class SqlManage : DataBaseConnection
    {
        private static readonly string conStr = "Data Source=.;Initial Catalog=studyCode;Integrated Security=True";
        

        #region 执行SQL， 返回受影响行数
        /// <summary>
        /// 执行SQL， 返回受影响行数
        /// </summary>
        /// <param name="sql">Sql文本</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] param)
        {
            using (var connection=GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    if (param != null) cmd.Parameters.AddRange(param);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 异步执行SQL， 返回受影响行数
        /// </summary>
        /// <param name="sql">Sql文本</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] param)
        {
            using (var connection = GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    if (param != null) cmd.Parameters.AddRange(param);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region 执行SQL，获取List对象
        /// <summary>
        /// 执行SQL，获取List对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlTxt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<T> ExecuteListEntity<T>(string sqlTxt, params SqlParameter[] param) where T : new()
        {
            using (var conn = GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sqlTxt, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (param != null) cmd.Parameters.AddRange(param);
                    return cmd.ExecuteReader().ToEntityList<T>();
                }
            }
        }

        /// <summary>
        /// 异步执行SQL，获取List对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlTxt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<List<T>> ExecuteListEntityAsync<T>(string sqlTxt, params SqlParameter[] param) where T : new()
        {
            using (var conn = GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sqlTxt, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (param != null) cmd.Parameters.AddRange(param);
                    var dataReader = await cmd.ExecuteReaderAsync();
                    return dataReader.ToEntityList<T>();
                }
            }
        }
        #endregion

        #region 执行SQL，获取单个对象
        /// <summary>
        /// 执行SQL，获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlTxt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ExecuteEntity<T>(string sqlTxt, params SqlParameter[] param) where T : new()
        {
            using (var conn = GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sqlTxt, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (param != null) cmd.Parameters.AddRange(param);
                    return cmd.ExecuteReader().ToEntity<T>();
                }
            }
        }

        /// <summary>
        /// 异步执行SQL，获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlTxt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteEntityAsync<T>(string sqlTxt, params SqlParameter[] param) where T : new()
        {
            using (var conn = GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sqlTxt, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (param != null) cmd.Parameters.AddRange(param);
                    var dataReader = await cmd.ExecuteReaderAsync();
                    return dataReader.ToEntity<T>();
                }
            }
        }
        #endregion
    }
}
