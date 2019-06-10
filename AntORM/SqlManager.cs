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
        public static int ExecuteNonQuery(string sqlTxt, params SqlParameter[] param)
        {
            return GetCommand(sqlTxt, param).ExecuteNonQuery();
        }

        /// <summary>
        /// 异步执行SQL， 返回受影响行数
        /// </summary>
        /// <param name="sql">Sql文本</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync(string sqlTxt, params SqlParameter[] param)
        {
            return await GetCommand(sqlTxt, param).ExecuteNonQueryAsync();
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
            return GetCommand(sqlTxt, param).ExecuteReader().ToEntityList<T>();
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
            var dataReader = await GetCommand(sqlTxt, param).ExecuteReaderAsync();
            if (dataReader == null || !dataReader.HasRows) return new List<T>();
            return dataReader.ToEntityList<T>();
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
            return GetCommand(sqlTxt, param).ExecuteReader().ToEntity<T>();
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
            var dataReader = await GetCommand(sqlTxt, param).ExecuteReaderAsync();
            if (dataReader == null || !dataReader.HasRows) return new T();
            return dataReader.ToEntity<T>();
        }
        #endregion

        #region 执行SQL,获取结果集中第一行的第一列
        /// <summary>
        /// 执行SQL,获取结果集中第一行的第一列
        /// </summary>
        /// <param name="sqlTxt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sqlTxt, params SqlParameter[] param)
        {
            return GetCommand(sqlTxt, param).ExecuteScalar();
        }

        /// <summary>
        /// 异步执行SQL,获取结果集中第一行的第一列
        /// </summary>
        /// <param name="sqlTxt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarAsync(string sqlTxt, params SqlParameter[] param)
        {
            return await GetCommand(sqlTxt, param).ExecuteScalarAsync();
        }
        #endregion
    }
}
