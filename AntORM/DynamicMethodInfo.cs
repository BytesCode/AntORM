using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace AntORM
{
    public class DynamicMethodInfo
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        public Type SourceType;
        public MethodInfo CanSettedMethod;
        public MethodInfo GetValueMethod;


        public DynamicMethodInfo(Type type)
        {
            SourceType = type;
            CanSettedMethod = this.GetType().GetMethod("CanSetted", new Type[] { type, typeof(string) });
            GetValueMethod = type.GetMethod("get_Item", new Type[] { typeof(string) });
        }


        /// <summary>
        /// 判断datareader是否存在某字段并且值不为空
        /// 已经改为一次验证
        /// </summary>
        /// <param name="dr">当前的datareader</param>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public static bool CanSetted(IDataRecord dr, string name)
        {
            return !dr[name].Equals(DBNull.Value);
        }
    }
}
