using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Infrastructure.Crosscutting.Utility.CommomHelper;

namespace Infrastructure.Crosscutting.Declaration
{
    /// <summary>
    ///     封装一些对象的扩展方法
    /// </summary>
    public static class ObjectExtendMethod
    {
        #region 对象扩展方法

        /// <summary>
        ///     将一个对象转化为Int型数据，为空时时返回0
        /// </summary>
        /// <param name="obj">要转化为Int型数据的对象</param>
        /// <returns>Int型数据，若转化失败返回0</returns>
        public static int ToInt32(object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return default(int);
            }
        }

        /// <summary>
        ///     将一个对象转化为Int型数据，为空时时返回0
        /// </summary>
        /// <param name="obj">要转化为Int型数据的对象</param>
        /// <returns>Int型数据，若转化失败返回0</returns>
        public static long ToLong(object obj)
        {
            try
            {
                return Convert.ToInt64(obj);
            }
            catch
            {
                return default(long);
            }
        }

        /// <summary>
        ///     将一个对象转化为日期型数据
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回时间型数据,若转化失败,则返回DateTime的默认值</returns>
        public static DateTime ToDateTime(object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return ToType<DateTime>("1900-1-1");
                //return default(DateTime);
            }
        }

        /// <summary>
        ///     将一个对象转化为逻辑性数据
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回布尔值,若转化失败,返回布尔型的默认值</returns>
        public static bool ToBoolean(object obj)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return default(bool);
            }
        }

        /// <summary>
        ///     将一个对象转化为实数类型
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回实数类型,若转化失败,返回实数的默认值</returns>
        public static decimal ToDecimal(object obj)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return default(decimal);
            }
        }

        /// <summary>
        ///     转化为实数类型，发生异常时返回默认，而不报错
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回实数类型,若转化失败,返回实数的默认值</returns>
        public static double ToDouble(object obj)
        {
            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return default(double);
            }
        }

        /// <summary>
        ///     将一个对象转换为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="source">对象源</param>
        /// <returns>具体类型</returns>
        public static T ToType<T>(this object source)
        {
            try
            {
                return (T) Convert.ChangeType(source, typeof (T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
        }
         
        /// <summary>
        ///     反序列化
        ///     先将数据库中取出的对象反序强制转化为byte数组，再反序列化为对象
        /// </summary>
        /// <param name="obj">要进行反序列化的对象</param>
        /// <returns>反序列化后生成的对象</returns>
        public static object Deserialize(this object obj)
        {
            try
            {
                return SerializeHelper.Deserialize((byte[]) obj);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     序列话，将一个对象序列化为byte数组
        /// </summary>
        /// <param name="obj">要进行序列化的对象</param>
        /// <returns>返回二进制数据</returns>
        public static byte[] Serialize(this object obj)
        {
            return SerializeHelper.Serialize(obj);
        }

        #region IsNull

        /// <summary>
        ///     Determines if the object is null
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>True if it is null, false otherwise</returns>
        public static bool IsNull(this object Object)
        {
            return Object == null;
        }

        #endregion

        #region IsNotNull

        /// <summary>
        ///     Determines if the object is not null
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>False if it is null, true otherwise</returns>
        public static bool IsNotNull(this object Object)
        {
            return Object != null;
        }

        #endregion

        #region IsNotNullOrDBNull

        /// <summary>
        ///     Determines if the object is not null or DBNull
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>False if it is null/DBNull, true otherwise</returns>
        public static bool IsNotNullOrDbNull(this object Object)
        {
            return Object != null && !Convert.IsDBNull(Object);
        }

        #endregion

        #region IsNullOrDBNull

        /// <summary>
        ///     Determines if the object is null or DBNull
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>True if it is null/DBNull, false otherwise</returns>
        public static bool IsNullOrDbNull(this object Object)
        {
            return Object == null || Convert.IsDBNull(Object);
        }

        #endregion

        #region ThrowIfNullOrDBNull

        /// <summary>
        ///     Determines if the object is null or DbNull and throws an ArgumentNullException if it is
        /// </summary>
        /// <param name="item">The object to check</param>
        /// <param name="name">Name of the argument</param>
        public static void ThrowIfNullOrDbNull(this object item, string name)
        {
            if (item.IsNullOrDbNull())
                throw new ArgumentNullException(name);
        }

        #endregion

        #region ThrowIfNull

        /// <summary>
        ///     Determines if the object is null and throws an ArgumentNullException if it is
        /// </summary>
        /// <param name="item">The object to check</param>
        /// <param name="name">Name of the argument</param>
        public static void ThrowIfNull(this object item, string name)
        {
            if (item.IsNull())
                throw new ArgumentNullException(name);
        }

        #endregion

        #region FormatToString

        /// <summary>
        ///     Calls the object's ToString function passing in the formatting
        /// </summary>
        /// <param name="input">Input object</param>
        /// <param name="format">Format of the output string</param>
        /// <returns>The formatted string</returns>
        public static string FormatToString(this object input, string format)
        {
            if (input.IsNull())
                return "";
            return !string.IsNullOrEmpty(format) ? (string) CallMethod("ToString", input, format) : input.ToString();
        }

        #endregion

        #endregion

        #region 字符串扩展方法

        private const string RegNumberStr = @"[+-]?\d+[\.]?\d*";

       
        /// <summary>
        ///     判断字符串是否为空
        ///     为空时返回true、否则返回false
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>为空时返回true、否则返回false</returns>
        public static bool IsEmptyString(this string str)
        {
            return string.IsNullOrEmpty(str) || str.Trim().Length == 0;
        }

        /// <summary>
        ///     判断字符串是否为int
        ///     为int 时返回true、否则返回false
        /// </summary>
        /// <param name="s"></param>
        /// <returns>为int 时返回true、否则返回false</returns>
        public static bool IsInt(this string s)
        {
            int i;
            bool b = int.TryParse(s, out i);
            return b;
        } 

        /// <summary>
        ///     扩展方法用来判断字符串是不是Email形式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(this string s)
        {   
            var r = new Regex(@"^[\w-]+(\.[\w-]+)*\.*@[\w-]+(\.[\w-]+)+$");
            return r.IsMatch(s);
        }

        public static bool IsIp(this string str)
        {
            const string pattern =
                @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

            if (IsEmptyString(str))
                return true;

            str = str.Trim();

            var rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rgx.IsMatch(str);
        }

        public static bool IsUrl(this string str)
        {
            const string pattern =
                @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)";

            if (IsEmptyString(str))
                return true;

            str = str.Trim();

            var rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rgx.IsMatch(str);
        }

        public static bool IsPhone(this string str)
        { 
            const string pattern = @"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";

            if (IsEmptyString(str))
                return true;

            str = str.Trim();

            var rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rgx.IsMatch(str); 
        }

        /// <summary>
        ///     日期格式字符串判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str)
        {
            bool result;
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
                    DateTime.Parse(str);
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        ///     验证身份证号码
        /// </summary>
        /// <param name="id">身份证号码</param>
        /// <returns>验证成功为True，否则为False</returns>
        public static bool IsIdCard(this string id)
        {
            if (id.Length == 18)
            {
                return CheckIdCard18(id);
            }
            return id.Length == 15 && CheckIdCard15(id);
        }

        /// <summary>
        ///     转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string HtmlEncode(this string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }

        /// <summary>
        ///     解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string HtmlDecode(this string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        /// <summary>
        ///     SQL注入字符清理
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        public static string SqlTextClear(this string sqlText)
        {
            if (sqlText == null)
            {
                return null;
            }
            if (sqlText == "")
            {
                return "";
            }
            sqlText = sqlText.Replace(",", "");
            sqlText = sqlText.Replace("<", "");
            sqlText = sqlText.Replace(">", "");
            sqlText = sqlText.Replace("--", "");
            sqlText = sqlText.Replace("'", "");
            sqlText = sqlText.Replace("\"", "");
            sqlText = sqlText.Replace("=", "");
            sqlText = sqlText.Replace("%", "");
            sqlText = sqlText.Replace(" ", "");
            return sqlText;
        }

        //提取数字的正则表达式

        /// <summary>
        ///     在字符串中提取最后一个数值
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static decimal GetNumberDecimal(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                MatchCollection mc = Regex.Matches(str, RegNumberStr);

                int i = mc.Count;

                if (i > 0)
                {
                    return mc[i - 1].Groups[0].Value.ToType<decimal>();
                }
                return 0;
            }
            return 0;
        }

        /// <summary>
        ///     在字符串中提取最后一个数值
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static double GetNumberDouble(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                MatchCollection mc = Regex.Matches(str, RegNumberStr);

                int i = mc.Count;

                if (i > 0)
                {
                    return mc[i - 1].Groups[0].Value.ToType<double>();
                }
                return 0;
            }
            return 0;
        }

        /// <summary>
        ///     提取字符串中的数值，如果不为数值者替换为 空字符 得到后在分割得到想到的数据
        ///     用str.Split(' ')分割，去掉不想要的空字符
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static string GetNumberStr(this string str)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                result = Regex.Replace(str, @"[^\d.\d]", " ").Trim();
            }
            return result;
        }

        /// <summary>
        ///     在字符串中提取最后一个整型
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static int GetNumberInt(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                MatchCollection mc = Regex.Matches(str, RegNumberStr, RegexOptions.Compiled);

                int i = mc.Count;

                if (i > 0)
                {
                    return mc[i - 1].Groups[0].Value.ToType<int>();
                }
                return 0;
            }
            return 0;
        }

        /// <summary>
        ///     避免了用StringBuilder的性能问题
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string StringFormat(this string format, params object[] args)
        {
            if (format == null || args == null)
                throw new ArgumentNullException((format == null) ? "format" : "args");

            int capacity = format.Length + args.Where(p => p != null).Select(p => p.ToString()).Sum(p => p.Length);
            var stringBuilder = new StringBuilder(capacity);
            stringBuilder.AppendFormat(format, args);
            return stringBuilder.ToString();
        }

        #endregion

        #region IEnumerable扩展方法

        /// <summary>
        ///     将指定类型列表转换为T-Sql的In语句的类型
        /// </summary>
        /// <returns></returns>
        public static string ToSqlInContent<TSource>(this IEnumerable<TSource> source)
        {
            try
            {
                var sb = new StringBuilder();
                foreach (TSource content in source)
                {
                    sb.Append("'");
                    sb.Append(content);
                    sb.Append("'");
                    sb.Append(",");
                }

                if (sb.Length > 2)
                    sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        ///     将指定类型的列表转换为以逗号分开的字符串，默认是“，”号分割, 结果,如 "1,2,3"
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <param name="separator">指定分割符</param>
        /// <returns></returns>
        public static string ToColumnString<TSource>(this IEnumerable<TSource> sources, string separator = ",")
        {
            try
            {
                var sb = new StringBuilder();

                foreach (TSource content in sources)
                {
                    if (sb.Length > 0)
                        sb.Append(separator);

                    sb.Append(content);
                }

                return sb.Length > 0 ? sb.ToString() : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     转换为一个DataTable
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TResult>(this IEnumerable<TResult> value) where TResult : class
        {
            //创建属性的集合
            var pList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof (TResult);
            var dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach(type.GetProperties(), p =>
                {
                    pList.Add(p);
                    dt.Columns.Add(p.Name);
                });
            foreach (TResult item in value)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();
                //给row 赋值
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }

        #region ThrowIfNullOrEmpty

        /// <summary>
        ///     Determines if the IEnumerable is null or empty and throws an ArgumentNullException if it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="name">Name of the argument</param>
        public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> item, string name)
        {
            if (item.IsNullOrEmpty())
                throw new ArgumentNullException(name);
        }

        #endregion

        #region Functions

        #region Exists

        /// <summary>
        ///     Used to determine if an item in the IEnumerable matches a predicate
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">List to search</param>
        /// <param name="match">The predicate used to check if something exists</param>
        /// <returns>True if at least one item matches the predicate, false otherwise</returns>
        public static bool Exists<T>(this IEnumerable<T> list, Predicate<T> match)
        {
            match.ThrowIfNull("Match");
            if (list.IsNull())
                return false;
            return list.Any(item => match(item));
        }

        #endregion

        #region For

        /// <summary>
        ///     Does an action for each item in the IEnumerable between the start and end indexes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> For<T>(this IEnumerable<T> list, int start, int end, Action<T> action)
        {
            list.ThrowIfNull("List");
            action.ThrowIfNull("Action");
            int x = 0;
            var enumerable = list as T[] ?? list.ToArray();
            foreach (T item in enumerable)
            {
                if (x.Between(start, end))
                    action(item);
                ++x;
                if (x > end)
                    break;
            }
            return enumerable;
        }

        /// <summary>
        ///     Does a function for each item in the IEnumerable between the start and end indexes and returns an IEnumerable of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> For<T, R>(this IEnumerable<T> list, int start, int end, Func<T, R> function)
        {
            list.ThrowIfNull("List");
            function.ThrowIfNull("Function");
            int x = 0;
            var returnValues = new List<R>();
            foreach (T item in list)
            {
                if (x.Between(start, end))
                    returnValues.Add(function(item));
                ++x;
                if (x > end)
                    break;
            }
            return returnValues;
        }

        #endregion

        #region ForEach

        /// <summary>
        ///     Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            list.ThrowIfNull("List");
            action.ThrowIfNull("Action");
            foreach (T item in list)
                action(item);
            return list;
        }

        /// <summary>
        ///     Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> list, Func<T, R> function)
        {
            list.ThrowIfNull("List");
            function.ThrowIfNull("Function");
            return list.Select(item => function(item)).ToList();
        }

        #endregion

        #region ForParallel

        /// <summary>
        ///     Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForParallel<T>(this IEnumerable<T> list, int start, int end, Action<T> action)
        {
            list.ThrowIfNull("List");
            action.ThrowIfNull("Action");
            Parallel.For(start, end + 1, x => action(list.ElementAt(x)));
            return list;
        }

        /// <summary>
        ///     Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForParallel<T, R>(this IEnumerable<T> list, int start, int end, Func<T, R> function)
        {
            list.ThrowIfNull("List");
            function.ThrowIfNull("Function");
            var results = new R[(end + 1) - start];
            Parallel.For(start, end + 1, x => results[x - start] = function(list.ElementAt(x)));
            return results;
        }

        #endregion

        #region ForEachParallel

        /// <summary>
        ///     Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> list, Action<T> action)
        {
            list.ThrowIfNull("List");
            action.ThrowIfNull("Action");
            Parallel.ForEach(list, action);
            return list;
        }

        /// <summary>
        ///     Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <returns>The results in an IEnumerable list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> list, Func<T, R> function)
        {
            list.ThrowIfNull("List");
            function.ThrowIfNull("Function");
            return list.ForParallel(0, list.Count() - 1, function);
        }

        #endregion

        #region IsNullOrEmpty

        /// <summary>
        ///     Determines if a list is null or empty
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="value">List to check</param>
        /// <returns>True if it is null or empty, false otherwise</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
        {
            return value.IsNull() || !value.Any();
        }

        #endregion

        #region RemoveDefaults

        /// <summary>
        ///     Removes default values from a list
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">List to cull items from</param>
        /// <param name="equalityComparer">Equality comparer used (defaults to GenericEqualityComparer)</param>
        /// <returns>An IEnumerable with the default values removed</returns>
        public static IEnumerable<T> RemoveDefaults<T>(this IEnumerable<T> value,
                                                       IEqualityComparer<T> equalityComparer = null)
        {
            if (value.IsNull())
                yield break;
            equalityComparer = equalityComparer.NullCheck(new GenericEqualityComparer<T>());
            foreach (T item in value.Where(x => !equalityComparer.Equals(x, default(T))))
                yield return item;
        }

        #endregion

        #region ToArray

        /// <summary>
        ///     Converts a list to an array
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TArget">Target type</typeparam>
        /// <param name="list">List to convert</param>
        /// <param name="convertingFunction">Function used to convert each item</param>
        /// <returns>The array containing the items from the list</returns>
        public static TArget[] ToArray<TSource, TArget>(this IEnumerable<TSource> list,
                                                       Func<TSource, TArget> convertingFunction)
        {
            list.ThrowIfNull("List");
            convertingFunction.ThrowIfNull("ConvertingFunction");
            return list.ForEach(convertingFunction).ToArray();
        }

        #endregion

        #region ToString

        /// <summary>
        ///     Converts the list to a string where each item is seperated by the Seperator
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">List to convert</param>
        /// <param name="itemOutput">Used to convert the item to a string (defaults to calling ToString)</param>
        /// <param name="seperator">Seperator to use between items (defaults to ,)</param>
        /// <returns>The string version of the list</returns>
        public static string ToString<T>(this IEnumerable<T> list, Func<T, string> itemOutput = null,
                                         string seperator = ",")
        {
            list.ThrowIfNull("List");
            seperator = seperator.NullCheck("");
            itemOutput = itemOutput.NullCheck(x => x.ToString());
            var builder = new StringBuilder();
            string tempSeperator = "";
            list.ForEach(x =>
                {
                    builder.Append(tempSeperator).Append(itemOutput(x));
                    tempSeperator = seperator;
                });
            return builder.ToString();
        }

        #endregion

        #region TrueForAll

        /// <summary>
        ///     Determines if a predicate is true for each item in a list
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="list">IEnumerable to look through</param>
        /// <param name="predicate">Predicate to use to check the IEnumerable</param>
        /// <returns>True if they all pass the predicate, false otherwise</returns>
        public static bool TrueForAll<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            list.ThrowIfNull("List");
            predicate.ThrowIfNull("Predicate");
            return list.All(x => predicate(x));
        }

        #endregion

        #region TryAll

        /// <summary>
        ///     Tries to do the action on each item in the list. If an exception is thrown,
        ///     it does the catch action on the item (if it is not null).
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="list">IEnumerable to look through</param>
        /// <param name="action">Action to run on each item</param>
        /// <param name="catchAction">Catch action (defaults to null)</param>
        /// <returns>The list after the action is run on everything</returns>
        public static IEnumerable<T> TryAll<T>(this IEnumerable<T> list, Action<T> action, Action<T> catchAction = null)
        {
            list.ThrowIfNull("List");
            action.ThrowIfNull("Action");
            foreach (T item in list)
            {
                try
                {
                    action(item);
                }
                catch
                {
                    if (catchAction != null) catchAction(item);
                }
            }
            return list;
        }

        #endregion

        #region TryAllParallel

        /// <summary>
        ///     Tries to do the action on each item in the list. If an exception is thrown,
        ///     it does the catch action on the item (if it is not null). This is done in
        ///     parallel.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="list">IEnumerable to look through</param>
        /// <param name="action">Action to run on each item</param>
        /// <param name="catchAction">Catch action (defaults to null)</param>
        /// <returns>The list after the action is run on everything</returns>
        public static IEnumerable<T> TryAllParallel<T>(this IEnumerable<T> list, Action<T> action,
                                                       Action<T> catchAction = null)
        {
            list.ThrowIfNull("List");
            action.ThrowIfNull("Action");
            Parallel.ForEach(list, delegate(T Item)
                {
                    try
                    {
                        action(Item);
                    }
                    catch
                    {
                        if (catchAction != null) catchAction(Item);
                    }
                });
            return list;
        }

        #endregion

        #endregion

        #endregion

        #region DateTime 扩展方法

        /// <summary>
        ///     返回指定日期的是星期几，会根据区域信息来返回，如：中文环境为 “星期一”
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToCultureDayOfWeek(this DateTime dateTime)
        {
            try
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dateTime.DayOfWeek);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     返回指定日期的农历日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToChineseDateTime(this DateTime dateTime)
        {
            try
            {
                return ChineseLunisolar.GetChineseDateTime(dateTime);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region Guid 扩展方法

        public static bool HasValue(this Guid source)
        {
            return source != Guid.Empty;
        }

        #endregion

        #region Helper

        #region String Extension Helper

        /// <summary>
        ///     验证15位身份证号
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIdCard18(string id)
        {
            long num;
            if (!long.TryParse(id.Remove(17), out num) || num < Math.Pow(10.0, 16.0) ||
                !long.TryParse(id.Replace('x', '0').Replace('X', '0'), out num))
            {
                return false;
            }
            const string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (text.IndexOf(id.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;
            }
            string s = id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime dateTime;
            if (!DateTime.TryParse(s, out dateTime))
            {
                return false;
            }
            string[] array = "1,0,x,9,8,7,6,5,4,3,2".Split(new[]
                {
                    ','
                });
            string[] array2 = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(new[]
                {
                    ','
                });
            char[] array3 = id.Remove(17).ToCharArray();
            int num2 = 0;
            for (int i = 0; i < 17; i++)
            {
                num2 += int.Parse(array2[i])*int.Parse(array3[i].ToString(CultureInfo.InvariantCulture));
            }
            int num3;
            Math.DivRem(num2, 11, out num3);
            return array[num3] == id.Substring(17, 1).ToLower();
        }

        /// <summary>
        ///     验证18位身份证号
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIdCard15(string id)
        {
            long num;
            if (!long.TryParse(id, out num) || num < Math.Pow(10.0, 14.0))
            {
                return false;
            }
            const string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (text.IndexOf(id.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;
            }
            string s = id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime dateTime;
            return DateTime.TryParse(s, out dateTime);
        }

        #endregion

        #region Private Static Functions

        /// <summary>
        ///     Calls a method on an object
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        private static object CallMethod(string methodName, object Object, params object[] inputVariables)
        {
            if (string.IsNullOrEmpty(methodName) || Object.IsNull())
                return null;
            Type objectType = Object.GetType();
            MethodInfo method;
            if (inputVariables.IsNotNull())
            {
                var methodInputTypes = new Type[inputVariables.Length];
                for (int x = 0; x < inputVariables.Length; ++x)
                    methodInputTypes[x] = inputVariables[x].GetType();
                method = objectType.GetMethod(methodName, methodInputTypes);
                if (method != null)
                    return method.Invoke(Object, inputVariables);
            }
            method = objectType.GetMethod(methodName);
            return method.IsNull() ? null : method.Invoke(Object, null);
        }

        #endregion

        #endregion

        #region ReflectionExtensions

        #region Functions

        #region CallMethod

        /// <summary>
        ///     Calls a method on an object
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="TReturnType">Return type expected</typeparam>
        /// <returns>The returned value of the method</returns>
        public static TReturnType CallMethod<TReturnType>(this object Object, string methodName,
                                                        params object[] inputVariables)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("methodName");
            if (inputVariables == null)
                inputVariables = new object[0];
            Type objectType = Object.GetType();
            var methodInputTypes = new Type[inputVariables.Length];
            for (int x = 0; x < inputVariables.Length; ++x)
                methodInputTypes[x] = inputVariables[x].GetType();
            MethodInfo method = objectType.GetMethod(methodName, methodInputTypes);
            if (method == null)
                throw new NullReferenceException("Could not find method " + methodName +
                                                 " with the appropriate input variables.");
            return (TReturnType) method.Invoke(Object, inputVariables);
        }

        #endregion

        #region CreateInstance

        /// <summary>
        ///     Creates an instance of the type and casts it to the specified type
        /// </summary>
        /// <typeparam name="TClassType">Class type to return</typeparam>
        /// <param name="type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static TClassType CreateInstance<TClassType>(this Type type, params object[] args)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return (TClassType) type.CreateInstance(args);
        }

        /// <summary>
        ///     Creates an instance of the type
        /// </summary>
        /// <param name="type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static object CreateInstance(this Type type, params object[] args)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return Activator.CreateInstance(type, args);
        }

        #endregion

        #region DumpProperties

        /// <summary>
        ///     Dumps the property names and current values from an object
        /// </summary>
        /// <param name="Object">Object to dunp</param>
        /// <param name="htmlOutput">Determines if the output should be HTML or not</param>
        /// <returns>An HTML formatted table containing the information about the object</returns>
        public static string DumpProperties(this object Object, bool htmlOutput = true)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            var tempValue = new StringBuilder();
            tempValue.Append(htmlOutput
                                 ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>"
                                 : "Property Name\t\t\t\tProperty Value");
            Type objectType = Object.GetType();
            foreach (PropertyInfo property in objectType.GetProperties())
            {
                tempValue.Append(htmlOutput ? "<tr><td>" : "")
                         .Append(property.Name)
                         .Append(htmlOutput ? "</td><td>" : "\t\t\t\t");
                ParameterInfo[] parameters = property.GetIndexParameters();
                if (property.CanRead && parameters.Length == 0)
                {
                    try
                    {
                        object value = property.GetValue(Object, null);
                        tempValue.Append(value == null ? "null" : value.ToString());
                    }
                    catch
                    {
                    }
                }
                tempValue.Append(htmlOutput ? "</td></tr>" : "");
            }
            tempValue.Append(htmlOutput ? "</tbody></table>" : "");
            return tempValue.ToString();
        }

        /// <summary>
        ///     Dumps the properties names and current values
        ///     from an object type (used for static classes)
        /// </summary>
        /// <param name="objectType">Object type to dunp</param>
        /// <param name="htmlOutput"></param>
        /// <returns>An HTML formatted table containing the information about the object type</returns>
        public static string DumpProperties(this Type objectType, bool htmlOutput = true)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");
            var tempValue = new StringBuilder();
            tempValue.Append(htmlOutput
                                 ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>"
                                 : "Property Name\t\t\t\tProperty Value");
            PropertyInfo[] properties = objectType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                tempValue.Append(htmlOutput ? "<tr><td>" : "")
                         .Append(property.Name)
                         .Append(htmlOutput ? "</td><td>" : "\t\t\t\t");
                if (property.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        tempValue.Append(property.GetValue(null, null) == null
                                             ? "null"
                                             : property.GetValue(null, null).ToString());
                    }
                    catch
                    {
                    }
                }
                tempValue.Append(htmlOutput ? "</td></tr>" : "");
            }
            tempValue.Append(htmlOutput ? "</tbody></table>" : "");
            return tempValue.ToString();
        }

        #endregion

        #region GetAttribute

        /// <summary>
        ///     Gets the attribute from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="provider">Attribute provider</param>
        /// <param name="inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>Attribute specified if it exists</returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider provider, bool inherit = true) where T : Attribute
        {
            return provider.IsDefined(typeof (T), inherit) ? provider.GetAttributes<T>(inherit)[0] : default(T);
        }

        #endregion

        #region GetAttributes

        /// <summary>
        ///     Gets the attributes from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="provider">Attribute provider</param>
        /// <param name="inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>Array of attributes</returns>
        public static T[] GetAttributes<T>(this ICustomAttributeProvider provider, bool inherit = true)
            where T : Attribute
        {
            return provider.IsDefined(typeof (T), inherit)
                       ? provider.GetCustomAttributes(typeof (T), inherit).ToArray(x => (T) x)
                       : new T[0];
        }

        #endregion

        #region GetName

        /// <summary>
        ///     Returns the type's name (Actual C# name, not the funky version from
        ///     the Name property)
        /// </summary>
        /// <param name="objectType">Type to get the name of</param>
        /// <returns>string name of the type</returns>
        public static string GetName(this Type objectType)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");
            var output = new StringBuilder();
            if (objectType.Name == "Void")
            {
                output.Append("void");
            }
            else
            {
                if (objectType.Name.Contains("`"))
                {
                    Type[] genericTypes = objectType.GetGenericArguments();
                    output.Append(objectType.Name.Remove(objectType.Name.IndexOf("`", StringComparison.Ordinal)))
                          .Append("<");
                    string seperator = "";
                    foreach (Type genericType in genericTypes)
                    {
                        output.Append(seperator).Append(genericType.GetName());
                        seperator = ",";
                    }
                    output.Append(">");
                }
                else
                {
                    output.Append(objectType.Name);
                }
            }
            return output.ToString();
        }

        #endregion

        #region GetObjects

        /// <summary>
        ///     Returns an instance of all classes that it finds within an assembly
        ///     that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="TClassType">Base type/interface searching for</typeparam>
        /// <param name="assembly">Assembly to search within</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<TClassType> GetObjects<TClassType>(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            return assembly.GetTypes<TClassType>().Where(x => !x.ContainsGenericParameters).Select(type => type.CreateInstance<TClassType>()).ToList();
        }

        /// <summary>
        ///     Returns an instance of all classes that it finds within a group of assemblies
        ///     that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="TClassType">Base type/interface searching for</typeparam>
        /// <param name="assemblies">Assemblies to search within</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<TClassType> GetObjects<TClassType>(this IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");
            var returnValues = new List<TClassType>();
            foreach (Assembly assembly in assemblies)
                returnValues.AddRange(assembly.GetObjects<TClassType>());
            return returnValues;
        }

        /// <summary>
        ///     Returns an instance of all classes that it finds within a directory
        ///     that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="TClassType">Base type/interface searching for</typeparam>
        /// <param name="directory">Directory to search within</param>
        /// <param name="Recursive"></param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<TClassType> GetObjects<TClassType>(this DirectoryInfo directory, bool Recursive = false)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");
            return directory.LoadAssemblies(Recursive).GetObjects<TClassType>();
        }

        #endregion

        #region GetProperty

        /// <summary>
        ///     Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object GetProperty(object Object, PropertyInfo property)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (property == null)
                throw new ArgumentNullException("property");
            return property.GetValue(Object, null);
        }

        /// <summary>
        ///     Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object GetProperty(object Object, string property)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(property))
                throw new ArgumentNullException("property");
            string[] properties = property.Split(new[] {"."}, StringSplitOptions.None);
            object tempObject = Object;
            Type tempObjectType = tempObject.GetType();
            PropertyInfo destinationProperty;
            for (int x = 0; x < properties.Length - 1; ++x)
            {
                destinationProperty = tempObjectType.GetProperty(properties[x]);
                tempObjectType = destinationProperty.PropertyType;
                tempObject = destinationProperty.GetValue(tempObject, null);
                if (tempObject == null)
                    return null;
            }
            destinationProperty = tempObjectType.GetProperty(properties[properties.Length - 1]);
            return GetProperty(tempObject,destinationProperty);
        }

        #endregion

        #region GetPropertyGetter

        /// <summary>
        ///     Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="TClassType">Class type</typeparam>
        /// <typeparam name="TDataType">Data type expecting</typeparam>
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<TClassType, TDataType>> GetPropertyGetter<TClassType, TDataType>(
            this PropertyInfo property)
        {
            if (!IsOfType(property.PropertyType, typeof (TDataType)))
                throw new ArgumentException("Property is not of the type specified");
            if (!IsOfType(property.DeclaringType, typeof (TClassType)))
                throw new ArgumentException("Property is not from the declaring class type specified");
            ParameterExpression objectInstance = Expression.Parameter(type: property.DeclaringType, name: "x");
            MemberExpression propertyGet = Expression.Property(objectInstance, property);
            if (property.PropertyType != typeof (TDataType))
            {
                UnaryExpression convert = Expression.Convert(propertyGet, typeof (TDataType));
                return Expression.Lambda<Func<TClassType, TDataType>>(convert, objectInstance);
            }
            return Expression.Lambda<Func<TClassType, TDataType>>(propertyGet, objectInstance);
        }

        /// <summary>
        ///     Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="TClassType">Class type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<TClassType, object>> GetPropertyGetter<TClassType>(this PropertyInfo property)
        {
            return property.GetPropertyGetter<TClassType, object>();
        }

        #endregion

        #region GetPropertyName

        /// <summary>
        ///     Gets a property name
        /// </summary>
        /// <typeparam name="TClassType">Class type</typeparam>
        /// <typeparam name="TDataType">Data type of the property</typeparam>
        /// <param name="expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string GetPropertyName<TClassType, TDataType>(this Expression<Func<TClassType, TDataType>> expression)
        {
            if (expression.Body is UnaryExpression && expression.Body.NodeType == ExpressionType.Convert)
            {
                var temp = (MemberExpression) ((UnaryExpression) expression.Body).Operand;
                return GetPropertyName(temp.Expression) + temp.Member.Name;
            }
            if (!(expression.Body is MemberExpression))
                throw new ArgumentException("Expression.Body is not a MemberExpression");
            return ((MemberExpression) expression.Body).Expression.GetPropertyName() +
                   ((MemberExpression) expression.Body).Member.Name;
        }

        /// <summary>
        ///     Gets a property name
        /// </summary>
        /// <param name="expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string GetPropertyName(this Expression expression)
        {
            if (!(expression is MemberExpression))
                return "";
            return ((MemberExpression) expression).Expression.GetPropertyName() +
                   ((MemberExpression) expression).Member.Name + ".";
        }

        #endregion

        #region GetPropertyType

        /// <summary>
        ///     Gets a property's type
        /// </summary>
        /// <param name="Object">object who contains the property</param>
        /// <param name="propertyPath">
        ///     Path of the property (ex: Prop1.Prop2.Prop3 would be
        ///     the Prop1 of the source object, which then has a Prop2 on it, which in turn
        ///     has a Prop3 on it.)
        /// </param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type GetPropertyType(object Object, string propertyPath)
        {
            if (Object == null || string.IsNullOrEmpty(propertyPath))
                return null;
            return GetPropertyType(Object.GetType(),propertyPath);
        }

        /// <summary>
        ///     Gets a property's type
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="propertyPath">
        ///     Path of the property (ex: Prop1.Prop2.Prop3 would be
        ///     the Prop1 of the source object, which then has a Prop2 on it, which in turn
        ///     has a Prop3 on it.)
        /// </param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type GetPropertyType(Type objectType, string propertyPath)
        {
            if (objectType == null || string.IsNullOrEmpty(propertyPath))
                return null;
            string[] sourceProperties = propertyPath.Split(new[] {"."}, StringSplitOptions.None);
            for (int x = 0; x < sourceProperties.Length; ++x)
            {
                PropertyInfo PropertyInfo = objectType.GetProperty(sourceProperties[x]);
                objectType = PropertyInfo.PropertyType;
            }
            return objectType;
        }

        #endregion

        #region GetPropertySetter

        /// <summary>
        ///     Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="TClassType">Class type</typeparam>
        /// <typeparam name="TDataType">Data type expecting</typeparam>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<TClassType, TDataType>> GetPropertySetter<TClassType, TDataType>(
            this Expression<Func<TClassType, TDataType>> property)
        {
            if (property == null)
                throw new ArgumentNullException("property");
            string propertyName = property.GetPropertyName();
            string[] splitName = propertyName.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries);
            PropertyInfo propertyInfo = typeof (TClassType).GetProperty(splitName[0]);
            ParameterExpression objectInstance = Expression.Parameter(propertyInfo.DeclaringType, "x");
            ParameterExpression PropertySet = Expression.Parameter(typeof (TDataType), "y");
            MethodCallExpression SetterCall = null;
            MemberExpression PropertyGet = null;
            if (splitName.Length > 1)
            {
                PropertyGet = Expression.Property(objectInstance, propertyInfo);
                for (int x = 1; x < splitName.Length - 1; ++x)
                {
                    propertyInfo = propertyInfo.PropertyType.GetProperty(splitName[x]);
                    PropertyGet = Expression.Property(PropertyGet, propertyInfo);
                }
                propertyInfo = propertyInfo.PropertyType.GetProperty(splitName[splitName.Length - 1]);
            }
            if (propertyInfo.PropertyType != typeof (TDataType))
            {
                UnaryExpression Convert = Expression.Convert(PropertySet, propertyInfo.PropertyType);
                if (PropertyGet == null)
                    SetterCall = Expression.Call(objectInstance, propertyInfo.GetSetMethod(), Convert);
                else
                    SetterCall = Expression.Call(PropertyGet, propertyInfo.GetSetMethod(), Convert);
                return Expression.Lambda<Action<TClassType, TDataType>>(SetterCall, objectInstance, PropertySet);
            }
            if (PropertyGet == null)
                SetterCall = Expression.Call(objectInstance, propertyInfo.GetSetMethod(), PropertySet);
            else
                SetterCall = Expression.Call(PropertyGet, propertyInfo.GetSetMethod(), PropertySet);
            return Expression.Lambda<Action<TClassType, TDataType>>(SetterCall, objectInstance, PropertySet);
        }

        /// <summary>
        ///     Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<ClassType, object>> GetPropertySetter<ClassType>(
            this Expression<Func<ClassType, object>> Property)
        {
            return Property.GetPropertySetter<ClassType, object>();
        }

        #endregion

        #region GetTypes

        /// <summary>
        ///     Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes<BaseType>(this Assembly Assembly)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            return Assembly.GetTypes(typeof (BaseType));
        }

        /// <summary>
        ///     Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <param name="BaseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes(this Assembly Assembly, Type BaseType)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            if (BaseType == null)
                throw new ArgumentNullException("BaseType");
            return Assembly.GetTypes().Where(x => x.IsOfType(BaseType) && x.IsClass && !x.IsAbstract);
        }

        /// <summary>
        ///     Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes<BaseType>(this IEnumerable<Assembly> Assemblies)
        {
            if (Assemblies == null)
                throw new ArgumentNullException("Assemblies");
            return Assemblies.GetTypes(typeof (BaseType));
        }

        /// <summary>
        ///     Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <param name="BaseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes(this IEnumerable<Assembly> Assemblies, Type BaseType)
        {
            if (Assemblies == null)
                throw new ArgumentNullException("Assemblies");
            if (BaseType == null)
                throw new ArgumentNullException("BaseType");
            var ReturnValues = new List<Type>();
            Assemblies.ForEach(y => ReturnValues.AddRange(y.GetTypes(BaseType)));
            return ReturnValues;
        }

        #endregion

        #region IsIEnumerable

        /// <summary>
        ///     Simple function to determine if an item is an IEnumerable
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsIEnumerable(this Type ObjectType)
        {
            return ObjectType.IsOfType(typeof (IEnumerable));
        }

        #endregion

        #region IsOfType

        /// <summary>
        ///     Determines if an object is of a specific type
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsOfType(this object Object, Type type)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (type == null)
                throw new ArgumentNullException("type");
            return Object.GetType().IsOfType(type);
        }

        /// <summary>
        ///     Determines if an object is of a specific type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsOfType(this Type ObjectType, Type type)
        {
            if (ObjectType == null)
                return false;
            if (type == null)
                throw new ArgumentNullException("type");
            if (type == ObjectType || ObjectType.GetInterface(type.Name, true) != null)
                return true;
            if (ObjectType.BaseType == null)
                return false;
            return ObjectType.BaseType.IsOfType(type);
        }

        #endregion

        #region Load

        /// <summary>
        ///     Loads an assembly by its name
        /// </summary>
        /// <param name="Name">Name of the assembly to return</param>
        /// <returns>The assembly specified if it exists</returns>
        public static Assembly Load(this AssemblyName Name)
        {
            Name.ThrowIfNull("Name");
            return AppDomain.CurrentDomain.Load(Name);
        }

        #endregion

        #region LoadAssemblies

        /// <summary>
        ///     Loads assemblies within a directory and returns them in an array.
        /// </summary>
        /// <param name="Directory">The directory to search in</param>
        /// <param name="Recursive">Determines whether to search recursively or not</param>
        /// <returns>Array of assemblies in the directory</returns>
        public static IEnumerable<Assembly> LoadAssemblies(this DirectoryInfo Directory, bool Recursive = false)
        {
            foreach (
                FileInfo File in
                    Directory.GetFiles("*.dll", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                )
                yield return AssemblyName.GetAssemblyName(File.FullName).Load();
        }

        #endregion

        #region MarkedWith

        /// <summary>
        ///     Goes through a list of types and determines if they're marked with a specific attribute
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Types">Types to check</param>
        /// <param name="Inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>The list of types that are marked with an attribute</returns>
        public static IEnumerable<Type> MarkedWith<T>(this IEnumerable<Type> Types, bool Inherit = true)
            where T : Attribute
        {
            if (Types == null)
                return null;
            return Types.Where(x => x.IsDefined(typeof (T), Inherit) && !x.IsAbstract);
        }

        #endregion

        #region MakeShallowCopy

        /// <summary>
        ///     Makes a shallow copy of the object
        /// </summary>
        /// <param name="Object">Object to copy</param>
        /// <param name="SimpleTypesOnly">If true, it only copies simple types (no classes, only items like int, string, etc.), false copies everything.</param>
        /// <returns>A copy of the object</returns>
        public static T MakeShallowCopy<T>(this T Object, bool SimpleTypesOnly = false)
        {
            if (Object == null)
                return default(T);
            Type ObjectType = Object.GetType();
            var ClassInstance = ObjectType.CreateInstance<T>();
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                try
                {
                    if (Property.CanRead
                        && Property.CanWrite
                        && SimpleTypesOnly
                        && Property.PropertyType.IsValueType)
                        Property.SetValue(ClassInstance, Property.GetValue(Object, null), null);
                    else if (!SimpleTypesOnly
                             && Property.CanRead
                             && Property.CanWrite)
                        Property.SetValue(ClassInstance, Property.GetValue(Object, null), null);
                }
                catch
                {
                }
            }

            foreach (FieldInfo Field in ObjectType.GetFields())
            {
                try
                {
                    if (SimpleTypesOnly && Field.IsPublic)
                        Field.SetValue(ClassInstance, Field.GetValue(Object));
                    else if (!SimpleTypesOnly && Field.IsPublic)
                        Field.SetValue(ClassInstance, Field.GetValue(Object));
                }
                catch
                {
                }
            }

            return ClassInstance;
        }

        #endregion

        #region SetProperty

        /// <summary>
        ///     Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static object SetProperty(object Object, PropertyInfo Property, object Value, string Format = "")
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (Property == null)
                throw new ArgumentNullException("Property");
            if (Value == null)
                throw new ArgumentNullException("Value");
            if (Property.PropertyType == typeof (string))
                Value = FormatToString(Value,Format);
            if (!Value.GetType().IsOfType(Property.PropertyType))
                Value = Convert.ChangeType(Value, Property.PropertyType);
            Property.SetValue(Object, Value, null);
            return Object;
        }

        /// <summary>
        ///     Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static object SetProperty(object Object, string Property, object Value, string Format = "")
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(Property))
                throw new ArgumentNullException("Property");
            if (Value == null)
                throw new ArgumentNullException("Value");
            string[] Properties = Property.Split(new[] {"."}, StringSplitOptions.None);
            object TempObject = Object;
            Type TempObjectType = TempObject.GetType();
            PropertyInfo DestinationProperty = null;
            for (int x = 0; x < Properties.Length - 1; ++x)
            {
                DestinationProperty = TempObjectType.GetProperty(Properties[x]);
                TempObjectType = DestinationProperty.PropertyType;
                TempObject = DestinationProperty.GetValue(TempObject, null);
                if (TempObject == null)
                    return Object;
            }
            DestinationProperty = TempObjectType.GetProperty(Properties[Properties.Length - 1]);
            SetProperty(TempObject,DestinationProperty, Value, Format);
            return Object;
        }

        #endregion

        #region ToLongVersionString

        /// <summary>
        ///     Gets the long version of the version information
        /// </summary>
        /// <param name="Assembly">Assembly to get version information from</param>
        /// <returns>The long version of the version information</returns>
        public static string ToLongVersionString(this Assembly Assembly)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            return Assembly.GetName().Version.ToString();
        }

        #endregion

        #region ToShortVersionString

        /// <summary>
        ///     Gets the short version of the version information
        /// </summary>
        /// <param name="Assembly">Assembly to get version information from</param>
        /// <returns>The short version of the version information</returns>
        public static string ToShortVersionString(this Assembly Assembly)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            Version VersionInfo = Assembly.GetName().Version;
            return VersionInfo.Major + "." + VersionInfo.Minor;
        }

        #endregion

        #endregion

        #endregion

        #region IComparableExtensions

        #region Functions

        #region Between

        /// <summary>
        ///     Checks if an item is between two values
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="Value">Value to check</param>
        /// <param name="Min">Minimum value</param>
        /// <param name="Max">Maximum value</param>
        /// <param name="Comparer">Comparer used to compare the values (defaults to GenericComparer)"</param>
        /// <returns>True if it is between the values, false otherwise</returns>
        public static bool Between<T>(this T Value, T Min, T Max, IComparer<T> Comparer = null) where T : IComparable
        {
            Comparer = Comparer.NullCheck(new GenericComparer<T>());
            return Comparer.Compare(Max, Value) >= 0 && Comparer.Compare(Value, Min) >= 0;
        }

        #endregion

        #endregion

        #endregion

        #region T Extensions

        #region NullCheck

        /// <summary>
        ///     Does a null check and either returns the default value (if it is null) or the object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="DefaultValue">Default value to return in case it is null</param>
        /// <returns>The default value if it is null, the object otherwise</returns>
        public static T NullCheck<T>(this T Object, T DefaultValue = default(T))
        {
            return Object == null ? DefaultValue : Object;
        }

        #endregion

        #region IsNotDefault

        /// <summary>
        ///     Determines if the object is not null
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">The object to check</param>
        /// <param name="EqualityComparer">Equality comparer used to determine if the object is equal to default</param>
        /// <returns>False if it is null, true otherwise</returns>
        public static bool IsNotDefault<T>(this T Object, IEqualityComparer<T> EqualityComparer = null)
        {
            return !EqualityComparer.NullCheck(new GenericEqualityComparer<T>()).Equals(Object, default(T));
        }

        #endregion

        #region IsDefault

        /// <summary>
        ///     Determines if the object is null
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">The object to check</param>
        /// <param name="EqualityComparer">Equality comparer used to determine if the object is equal to default</param>
        /// <returns>True if it is null, false otherwise</returns>
        public static bool IsDefault<T>(this T Object, IEqualityComparer<T> EqualityComparer = null)
        {
            return EqualityComparer.NullCheck(new GenericEqualityComparer<T>()).Equals(Object, default(T));
        }

        #endregion

        #region ThrowIfDefault

        /// <summary>
        ///     Determines if the object is equal to default value and throws an ArgumentNullException if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="EqualityComparer">Equality comparer used to determine if the object is equal to default</param>
        /// <param name="Name">Name of the argument</param>
        public static void ThrowIfDefault<T>(this T Item, string Name, IEqualityComparer<T> EqualityComparer = null)
        {
            if (Item.IsDefault(EqualityComparer))
                throw new ArgumentNullException(Name);
        }

        #endregion

        #endregion

        #region  Type & DbType & SqlDbType Extensions

        #region ToSQLDbType

        /// <summary>
        ///     Converts a .Net type to SQLDbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding SQLDbType</returns>
        public static SqlDbType ToSQLDbType(this Type Type)
        {
            return Type.ToDbType().ToSqlDbType();
        }

        /// <summary>
        ///     Converts a DbType to a SqlDbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding SqlDbType (if it exists)</returns>
        public static SqlDbType ToSqlDbType(this DbType Type)
        {
            var Parameter = new SqlParameter();
            Parameter.DbType = Type;
            return Parameter.SqlDbType;
        }

        #endregion

        #region ToDbType

        /// <summary>
        ///     Converts a .Net type to DbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding DbType</returns>
        public static DbType ToDbType(this Type Type)
        {
            if (Type == typeof (byte)) return DbType.Byte;
            else if (Type == typeof (sbyte)) return DbType.SByte;
            else if (Type == typeof (short)) return DbType.Int16;
            else if (Type == typeof (ushort)) return DbType.UInt16;
            else if (Type == typeof (int)) return DbType.Int32;
            else if (Type == typeof (uint)) return DbType.UInt32;
            else if (Type == typeof (long)) return DbType.Int64;
            else if (Type == typeof (ulong)) return DbType.UInt64;
            else if (Type == typeof (float)) return DbType.Single;
            else if (Type == typeof (double)) return DbType.Double;
            else if (Type == typeof (decimal)) return DbType.Decimal;
            else if (Type == typeof (bool)) return DbType.Boolean;
            else if (Type == typeof (string)) return DbType.String;
            else if (Type == typeof (char)) return DbType.StringFixedLength;
            else if (Type == typeof (Guid)) return DbType.Guid;
            else if (Type == typeof (DateTime)) return DbType.DateTime2;
            else if (Type == typeof (DateTimeOffset)) return DbType.DateTimeOffset;
            else if (Type == typeof (byte[])) return DbType.Binary;
            else if (Type == typeof (byte?)) return DbType.Byte;
            else if (Type == typeof (sbyte?)) return DbType.SByte;
            else if (Type == typeof (short?)) return DbType.Int16;
            else if (Type == typeof (ushort?)) return DbType.UInt16;
            else if (Type == typeof (int?)) return DbType.Int32;
            else if (Type == typeof (uint?)) return DbType.UInt32;
            else if (Type == typeof (long?)) return DbType.Int64;
            else if (Type == typeof (ulong?)) return DbType.UInt64;
            else if (Type == typeof (float?)) return DbType.Single;
            else if (Type == typeof (double?)) return DbType.Double;
            else if (Type == typeof (decimal?)) return DbType.Decimal;
            else if (Type == typeof (bool?)) return DbType.Boolean;
            else if (Type == typeof (char?)) return DbType.StringFixedLength;
            else if (Type == typeof (Guid?)) return DbType.Guid;
            else if (Type == typeof (DateTime?)) return DbType.DateTime2;
            else if (Type == typeof (DateTimeOffset?)) return DbType.DateTimeOffset;
            return DbType.Int32;
        }

        /// <summary>
        ///     Converts SqlDbType to DbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding DbType (if it exists)</returns>
        public static DbType ToDbType(this SqlDbType Type)
        {
            var Parameter = new SqlParameter();
            Parameter.SqlDbType = Type;
            return Parameter.DbType;
        }

        #endregion

        #region ToType

        /// <summary>
        ///     Converts a SQLDbType value to .Net type
        /// </summary>
        /// <param name="Type">SqlDbType Type</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ToType(this SqlDbType Type)
        {
            return Type.ToDbType().ToType();
        }

        /// <summary>
        ///     Converts a DbType value to .Net type
        /// </summary>
        /// <param name="Type">DbType</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ToType(this DbType Type)
        {
            if (Type == DbType.Byte) return typeof (byte);
            else if (Type == DbType.SByte) return typeof (sbyte);
            else if (Type == DbType.Int16) return typeof (short);
            else if (Type == DbType.UInt16) return typeof (ushort);
            else if (Type == DbType.Int32) return typeof (int);
            else if (Type == DbType.UInt32) return typeof (uint);
            else if (Type == DbType.Int64) return typeof (long);
            else if (Type == DbType.UInt64) return typeof (ulong);
            else if (Type == DbType.Single) return typeof (float);
            else if (Type == DbType.Double) return typeof (double);
            else if (Type == DbType.Decimal) return typeof (decimal);
            else if (Type == DbType.Boolean) return typeof (bool);
            else if (Type == DbType.String) return typeof (string);
            else if (Type == DbType.StringFixedLength) return typeof (char);
            else if (Type == DbType.Guid) return typeof (Guid);
            else if (Type == DbType.DateTime2) return typeof (DateTime);
            else if (Type == DbType.DateTime) return typeof (DateTime);
            else if (Type == DbType.DateTimeOffset) return typeof (DateTimeOffset);
            else if (Type == DbType.Binary) return typeof (byte[]);
            return typeof (int);
        }

        #endregion

        #endregion

        //public static class IEnumerableExtensions
        //{
        //    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        //    {
        //        foreach (T item in items)
        //        {
        //            action(item);
        //        }
        //    }
        //}

        //public static class MiscExtensions
        //{
        //    #region Business Error Mapping

        //    public static List<ErrorItem> ToErrorItemList(this IEnumerable<ValidationErrorItem> validationErrorItems)
        //    {
        //        List<ErrorItem> items = new List<ErrorItem>();
        //        validationErrorItems.ForEach(validationErrorItem => items.Add(new ErrorItem { Key = validationErrorItem.ErrorKey, Parameters = validationErrorItem.Parameters }));
        //        return items;
        //    }

        //    #endregion
        //}
    }


    public static class BasicTypeExtensions
    {
        //public static bool HasValue(this UniqueId source)
        //{
        //    return source.Value != Guid.Empty;
        //}
    }
}