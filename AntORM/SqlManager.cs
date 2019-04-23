using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AntORM
{
    public class SqlManage
    {
        private static readonly string conStr = "Data Source=.;Initial Catalog=studyCode;Integrated Security=True";

        /// <summary>
        /// 执行SQL， 返回受影响行数
        /// </summary>
        /// <param name="sql">Sql文本</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] param)
        {
            using (SqlConnection conn=new SqlConnection(conStr))
            {
                using (SqlCommand cmd=new SqlCommand(sql,conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// 执行SQL，获取List对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlTxt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<T> ExecuteListEntity<T>(string sqlTxt, params SqlParameter[] param) where T : new()
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlTxt, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    conn.Open();
                    return cmd.ExecuteReader().ToEntityList<T>();
                }
            }
        }

        /// <summary>
        /// 执行SQL，获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlTxt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ExecuteEntity<T>(string sqlTxt, params SqlParameter[] param) where T : new()
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlTxt, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    conn.Open();
                    return cmd.ExecuteReader().ToEntity<T>();
                }
            }
        }
    }
}
