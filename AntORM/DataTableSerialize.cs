using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace AntORM
{
    public class DataTableSerialize
    {
        /// <summary>
        /// DataTable 转换List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DatatableList<T>(DataTable table)
        {
            if (table == null || table.Columns.Count == 0)
                return new List<T>();

            List<T> list = new List<T>();
            Type type = typeof(T);

            foreach (DataRow item in table.Rows)
            {
                object obj = Activator.CreateInstance(type);
                foreach (var prop in type.GetProperties())
                    prop.SetValue(obj, item[prop.Name], null);

                list.Add((T)obj);
            }
            return list;
        }


        /// <summary>
        /// 将IDataReader 转换成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> DataReaderToListEntity<T>(IDataReader reader) where T : new()
        {
            List<T> list = new List<T>();
            if (reader != null)
            {
                while (reader.Read())
                    list.Add(DataReaderToEntity<T>(reader));
            }
            return list;
        }


        /// <summary>
        /// 将 DataReader转换成指定 实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T DataReaderToEntity<T>(IDataReader reader) where T : new()
        {
            T t = new T();
            //获取需要映射实体的 所有属性
            PropertyInfo[] propertys = t.GetType().GetProperties();

            //存储IDataReader 读取出来的所有列名
            List<string> FieldNameList = new List<string>();

            //循环reader 将 列名加入list
            for (int i = 0; i < reader.FieldCount; i++)
                FieldNameList.Add(reader.GetName(i));

            //循环需要映射的属性
            foreach (PropertyInfo property in propertys)
            {
                //判断 该值 是否可写入
                if (!property.CanWrite)
                    continue;

                //获取字段名
                string FieldName = property.Name;
                if (FieldNameList.Contains(FieldName))
                {
                    object value = reader[FieldName];
                    if (value is DBNull)
                        continue;

                    try
                    {
                        property.SetValue(t, value, null);
                    }
                    catch { continue; }
                }
            }
            return t;
        }
    }
}
