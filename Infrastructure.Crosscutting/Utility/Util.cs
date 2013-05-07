using System.Collections.Generic;
using System.Data;
using System.Configuration;
using FileIO = System.IO;
using System.Reflection;
using System.Diagnostics;
using System;
using System.ComponentModel;

namespace Infrastructure.Crosscutting.Utility
{
    public static class Util
    {
        public static T[] TableToArray<T>(DataTable dt, string colName)
        {
            T[] result = new T[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                result[i] = (T)dt.Rows[i][colName];

            return result;
        }

        public static bool HasEventHandler(object instance, string eventName)
        {
            Type clsType = instance.GetType();

            FieldInfo evtField = clsType.GetField(eventName, BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);

            return evtField.GetValue(instance) != null;
        }

        public static bool IsNullable(Type t)
        {
            return t == typeof(string) || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static Type Nullable2ValueType(Type nullableType)
        {
            return new NullableConverter(nullableType).UnderlyingType;
        }

        public static object ConvertByType(PropertyInfo pi, string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;

            Type targetType = pi.PropertyType;
            if (Util.IsNullable(targetType))
                targetType = Util.Nullable2ValueType(targetType);

            // enum要做特殊处理
            if (targetType.IsEnum)
                return Enum.ToObject(targetType, byte.Parse(val));
            else if (targetType == typeof(bool))
                return val == "1";
            else
                return Convert.ChangeType(val, targetType);
        }

        #region 对DataRow的数据处理

        /// <summary>
        /// 从DataRow中读取/写入值，值可能是空的情况。
        /// </summary>
        public static T Get<T>(DataRow dr, string colName)
        {
            // 只有表中包含这个列，我们才取值，不然就返回类型的默认值
            if (dr.Table.Columns.Contains(colName) && dr[colName] != DBNull.Value)
                return (T)dr[colName];

            return default(T);
        }

        /// <summary>
        /// 把百分比的值转成格式化的字符串，如0.9=>90%
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static string GetPercent(DataRow dr, string colName)
        { 
            if (dr.Table.Columns.Contains(colName) && dr[colName] != DBNull.Value)
                return string.Format("{0:P0}", dr[colName]);

            return null;
        }

        /// <summary>
        /// 把Money转成格式化的字符串，如12.36=>￥12.36
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static string GetPrice(DataRow dr, string colName)
        {
            if (dr.Table.Columns.Contains(colName) && dr[colName] != DBNull.Value)
                return string.Format("{0:C}", dr[colName]);

            return null;
        }


        /// <summary>
        /// 向DataRow中写入值，值可能是空的情况。
        /// </summary>
        public static void To<T>(T value, DataRow dr, string colName)
        {
            // 因为是保存，必须验证表中是否有这个列
            ConfirmColumn(dr, colName);

            if (value != null)
            {
                if (value.GetType() == typeof(string) && string.IsNullOrWhiteSpace(value.ToString()))
                    dr[colName] = DBNull.Value;
                else
                    dr[colName] = value;
            }
            else
                dr[colName] = DBNull.Value;
        }

        #endregion

        #region Internal Helper

        private static void ConfirmColumn(DataRow dr, string colName)
        {
#if DEBUG
            if (!dr.Table.Columns.Contains(colName))
            {
                StackTrace st = new StackTrace();
                if (st.FrameCount > 2)
                {
                    // get calling frame, not myself
                    StackFrame sf = st.GetFrame(2);
                    MethodBase mb = sf.GetMethod();

                    Debug.Fail(string.Format("{0}.{1}产生错误：表中不包含{2}列.", mb.DeclaringType.Name, mb.Name, colName));
                }
            }
#endif
        }
        #endregion

        #region 实现对集合与对象的复制

        public static void CopyCollection<T>(IEnumerable<T> from, ICollection<T> to)
        {
            if (from == null || to == null || to.IsReadOnly)
            {
                return;
            }

            to.Clear();
            foreach (T element in from)
            {
                to.Add(element);
            }
        }

        public static void CopyModel(object from, object to)
        {
            if (from == null || to == null)
            {
                return;
            }

            PropertyDescriptorCollection fromProperties = TypeDescriptor.GetProperties(from);
            PropertyDescriptorCollection toProperties = TypeDescriptor.GetProperties(to);

            foreach (PropertyDescriptor fromProperty in fromProperties)
            {
                PropertyDescriptor toProperty = toProperties.Find(fromProperty.Name, true /* ignoreCase */);
                if (toProperty != null && !toProperty.IsReadOnly)
                {
                    // Can from.Property reference just be assigned directly to to.Property reference?
                    bool isDirectlyAssignable = toProperty.PropertyType.IsAssignableFrom(fromProperty.PropertyType);
                    // Is from.Property just the nullable form of to.Property?
                    bool liftedValueType = (isDirectlyAssignable) ? false : (Nullable.GetUnderlyingType(fromProperty.PropertyType) == toProperty.PropertyType);

                    if (isDirectlyAssignable || liftedValueType)
                    {
                        object fromValue = fromProperty.GetValue(from);
                        if (isDirectlyAssignable || (fromValue != null && liftedValueType))
                        {
                            toProperty.SetValue(to, fromValue);
                        }
                    }
                }
            }
        } 

        #endregion

    }
}
