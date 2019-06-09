using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace AntORM
{
    public static class DataTableSerialize
    {
        public delegate T LoadDataReader<T>(IDataRecord dr);
        private static readonly Dictionary<Type, MethodInfo> TypeMap;

        //emit里面用到的针对datareader的元数据信息
        private static readonly DynamicMethodInfo dataReadAssembly = new DynamicMethodInfo(typeof(IDataRecord));


        static DataTableSerialize()
        {
            TypeMap = new Dictionary<Type, MethodInfo>()
            {
                {typeof(int),typeof(Convert).GetMethod("ToInt32",new Type[]{typeof(object)})},
                {typeof(Int16),typeof(Convert).GetMethod("ToInt16",new Type[]{typeof(object)})},
                {typeof(Int64),typeof(Convert).GetMethod("ToInt64",new Type[]{typeof(object)})},
                {typeof(DateTime),typeof(Convert).GetMethod("ToDateTime",new Type[]{typeof(object)})},
                {typeof(decimal),typeof(Convert).GetMethod("ToDecimal",new Type[]{typeof(object)})},
                {typeof(Double),typeof(Convert).GetMethod("ToDouble",new Type[]{typeof(object)})},
                {typeof(Boolean),typeof(Convert).GetMethod("ToBoolean",new Type[]{typeof(object)})},
                {typeof(string),typeof(Convert).GetMethod("ToString",new Type[]{typeof(object)})}
            };
        }

        /// <summary>
        /// 将 DataReader转换成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this IDataReader reader) where T : new()
        {
            T t = new T();
            LoadDataReader<T> Load = null;
            //用于 暂时存放 字段
            Dictionary<string, PropertyInfo> FieldNames = new Dictionary<string, PropertyInfo>();

            var properties = typeof(T).GetProperties();
            FieldNames = GetPropertyInfo(reader, properties);
            Load = (LoadDataReader<T>)DataReaderCreateEntity<T>(dataReadAssembly, FieldNames).CreateDelegate(typeof(LoadDataReader<T>));

            while (reader.Read())
            {
                t = Load(reader);
                break;
            }
            return t;
        }


        /// <summary>
        /// 将 DataReader转换成指定 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToEntityList<T>(this IDataReader reader)
        {
            List<T> list = new List<T>();
            LoadDataReader<T> Load = null;
            //用于 暂时存放 字段
            Dictionary<string, PropertyInfo> FieldNames = new Dictionary<string, PropertyInfo>();

            var properties = typeof(T).GetProperties();
            FieldNames = GetPropertyInfo(reader, properties);

            Load = (LoadDataReader<T>)DataReaderCreateEntity<T>(dataReadAssembly, FieldNames).CreateDelegate(typeof(LoadDataReader<T>));

            while (reader.Read())
            {
                list.Add(Load(reader));
            }

            return list;
        }

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DicProperties"></param>
        /// <returns></returns>
        private static DynamicMethod DataReaderCreateEntity<T>(DynamicMethodInfo Assembly, Dictionary<string, PropertyInfo> DicProperties)
        {
            Type type = typeof(T);
            var dm = new DynamicMethod("ParamInfo" + Guid.NewGuid().ToString(), type, new Type[] { Assembly.SourceType }, true);
            var il = dm.GetILGenerator();
            LocalBuilder result = il.DeclareLocal(type);

            il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, result);

            foreach (var item in DicProperties)
            {
                PropertyInfo property = item.Value;
                var endIfLabel = il.DefineLabel();
                il.Emit(OpCodes.Ldarg_0);
                //第一组，调用AssembleInfo的CanSetted方法，判断是否可以转换
                il.Emit(OpCodes.Ldstr, item.Key);
                il.Emit(OpCodes.Call, Assembly.CanSettedMethod);
                il.Emit(OpCodes.Brfalse, endIfLabel);

                il.Emit(OpCodes.Ldloc, result);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldstr, item.Key);
                il.Emit(OpCodes.Call, Assembly.GetValueMethod);//获取数据库值

                if (item.Value.PropertyType.IsValueType || item.Value.PropertyType == typeof(string))
                {
                    var cur = Nullable.GetUnderlyingType(item.Value.PropertyType);
                    ////调用强转方法赋值
                    il.Emit(OpCodes.Call, TypeMap[cur ?? item.Value.PropertyType]);

                    if (cur != null)
                        il.Emit(OpCodes.Newobj, item.Value.PropertyType.GetConstructor(new Type[] { cur }));
                }
                else
                {
                    il.Emit(OpCodes.Castclass, item.Value.PropertyType);
                }
                il.Emit(OpCodes.Call, property.GetSetMethod());//直接赋值给 属性
                il.MarkLabel(endIfLabel);
            }
            il.Emit(OpCodes.Ldloc, result);
            il.Emit(OpCodes.Ret);

            return dm;
        }


        /// <summary>
        /// 获取可 生成是 列名，以及类型
        /// </summary>
        /// <param name="Reader">DataReader</param>
        /// <param name="PropertyInfos">PropertyInfo</param>
        /// <returns></returns>
        private static Dictionary<string, PropertyInfo> GetPropertyInfo(IDataReader Reader, PropertyInfo[] PropertyInfos)
        {
            var dic = new Dictionary<string, PropertyInfo>();
            //实体的字段列表
            List<string> colnum = new List<string>();

            for (int i = 0; i < Reader.FieldCount; i++)
                colnum.Add(Reader.GetName(i));

            //查找 可匹配的列
            foreach (PropertyInfo item in PropertyInfos)
            {
                if (colnum.Contains(item.Name))
                {
                    dic.Add(item.Name, item);
                }
            }
            return dic;
        }

    }
}
