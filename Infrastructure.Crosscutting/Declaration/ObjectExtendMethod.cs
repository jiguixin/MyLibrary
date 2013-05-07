using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// 封装一些对象的扩展方法
    /// </summary>
    public static class ObjectExtendMethod
    {
        #region 对象扩展方法
        /// <summary>
        /// 将一个对象转化为Int型数据，为空时时返回0
        /// </summary>
        /// <param name="obj">要转化为Int型数据的对象</param>
        /// <returns>Int型数据，若转化失败返回0</returns>
        public static int ToInt32(this object obj)
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
        /// 将一个对象转化为日期型数据
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回时间型数据,若转化失败,则返回DateTime的默认值</returns>
        public static DateTime ToDateTime(this object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return "1900-1-1".ToDateTime();
                //return default(DateTime);
            }
        }
         
        /// <summary>
        /// 将一个对象转化为逻辑性数据
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回布尔值,若转化失败,返回布尔型的默认值</returns>
        public static bool ToBoolean(this object obj)
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
        /// 将一个对象转化为实数类型
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回实数类型,若转化失败,返回实数的默认值</returns>
        public static decimal ToDecimal(this object obj)
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
        /// 转化为实数类型，发生异常时返回默认，而不报错
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回实数类型,若转化失败,返回实数的默认值</returns>
        public static double ToDouble(this object obj)
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
        /// 反序列化
        ///  先将数据库中取出的对象反序强制转化为byte数组，再反序列化为对象
        /// </summary>
        /// <param name="obj">要进行反序列化的对象</param>
        /// <returns>反序列化后生成的对象</returns>
        public static object Deserialize(this object obj)
        {
            try
            {
                return SerializeHelper.Deserialize((byte[])obj);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 序列话，将一个对象序列化为byte数组
        /// </summary>
        /// <param name="obj">要进行序列化的对象</param>
        /// <returns>返回二进制数据</returns>
        public static byte[] Serialize(this object obj)
        {
            return SerializeHelper.Serialize(obj);
        }

        #region IsNull

        /// <summary>
        /// Determines if the object is null
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
        /// Determines if the object is not null
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
        /// Determines if the object is not null or DBNull
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>False if it is null/DBNull, true otherwise</returns>
        public static bool IsNotNullOrDBNull(this object Object)
        {
            return Object != null && !Convert.IsDBNull(Object);
        }

        #endregion

        #region IsNullOrDBNull

        /// <summary>
        /// Determines if the object is null or DBNull
        /// </summary>
        /// <param name="Object">The object to check</param>
        /// <returns>True if it is null/DBNull, false otherwise</returns>
        public static bool IsNullOrDBNull(this object Object)
        {
            return Object == null || Convert.IsDBNull(Object);
        }

        #endregion

        #region ThrowIfNullOrDBNull

        /// <summary>
        /// Determines if the object is null or DbNull and throws an ArgumentNullException if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        public static void ThrowIfNullOrDBNull(this object Item, string Name)
        {
            if (Item.IsNullOrDBNull())
                throw new ArgumentNullException(Name);
        }

        #endregion

        #region ThrowIfNull

        /// <summary>
        /// Determines if the object is null and throws an ArgumentNullException if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        public static void ThrowIfNull(this object Item, string Name)
        {
            if (Item.IsNull())
                throw new ArgumentNullException(Name);
        }

        #endregion

        #region FormatToString

        /// <summary>
        /// Calls the object's ToString function passing in the formatting
        /// </summary>
        /// <param name="Input">Input object</param>
        /// <param name="Format">Format of the output string</param>
        /// <returns>The formatted string</returns>
        public static string FormatToString(this object Input, string Format)
        {
            if (Input.IsNull())
                return "";
            return !string.IsNullOrEmpty(Format) ? (string)CallMethod("ToString", Input, Format) : Input.ToString();
        }

        #endregion

        #endregion

        #region 字符串扩展方法
        /// <summary>
        /// 判断字符串是否为空
        /// 为空时返回true、否则返回false
        /// </summary>
        /// <param name="s"></param>
        /// <returns>为空时返回true、否则返回false</returns>
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// 判断字符串是否为int
        /// 为int 时返回true、否则返回false
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
        /// 扩展方法用来判断字符串是不是Email形式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(this string s)
        {
            //Regex r = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");     
            Regex r = new Regex(@"^[\w-]+(\.[\w-]+)*\.*@[\w-]+(\.[\w-]+)+$");
            return r.IsMatch(s);
        }

        public static bool IsEmptyString(this string str)
        {
            return string.IsNullOrEmpty(str) || str.Trim().Length == 0;
        }

        public static bool IsNumberString(this string str)
        {
            int result = 0;
            return int.TryParse(str, out result);
        }

        public static bool IsValidateIP(this string str)
        {
            string pattern = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

            if (IsEmptyString(str))
                return true;

            str = str.Trim();

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rgx.IsMatch(str);
        }

        public static bool IsValidateEmail(this string str)
        {
            string pattern = @"\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b";

            if (IsEmptyString(str))
                return true;

            str = str.Trim();

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rgx.IsMatch(str);
        }

        public static bool IsValidateUrl(this string str)
        {
            string pattern = @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)";

            if (IsEmptyString(str))
                return true;

            str = str.Trim();

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rgx.IsMatch(str);
        }

        public static bool IsValidatePhone(this string str)
        {
            //string pattern = @"\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b";

            if (IsEmptyString(str))
                return true;

            //str = str.Trim();

            //Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            //return rgx.IsMatch(str);
            return true;
        }
         
        /// <summary>
        /// 日期格式字符串判断
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
                    DateTime.Parse(str);
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
        /// 验证身份证号码
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns>验证成功为True，否则为False</returns>
        public static bool CheckIDCard(this string Id)
        {
            if (Id.Length == 18)
            {
                return ObjectExtendMethod.CheckIDCard18(Id);
            }
            return Id.Length == 15 && ObjectExtendMethod.CheckIDCard15(Id);
        }

        /// <summary>
        /// 转换成 HTML code
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
        /// 解析html成 普通文本
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
        /// SQL注入字符清理
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

        /// <summary>
        /// 在字符串中提取数值
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static decimal GetNumber(this string str)
        {
            decimal result = 0;
            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                str = Regex.Replace(str, @"[^\d.\d]", " ");
                // 如果是数字，则转换为decimal类型 
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = decimal.Parse(str);
                }
            }
            return result; 
        }

        /// <summary>
        /// 提取字符串中的数值，如果不为数值者替换为 空字符 得到后在分割得到想到的数据
        /// 用str.Split(' ')分割，去掉不想要的空字符
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static string GetNumberStr(this string str)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                result = Regex.Replace(str, @"[^\d.\d]", " ");                  
            }
            return result;
        }

        /// <summary>
        /// 在字符串中提取整型
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static int GetNumberInt(this string str)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                str = Regex.Replace(str, @"[^\d\d]", " ");
                // 如果是数字，则转换为decimal类型 
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = int.Parse(str);
                }
            }
            return result; 
        }

        /// <summary>
        /// 避免了用StringBuilder的性能问题
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static String Format(this String format, params object[] args)
        {
            if (format == null || args == null)
                throw new ArgumentNullException((format == null) ? "format" : "args");

            var capacity = format.Length + args.Where(p => p != null).Select(p => p.ToString()).Sum(p => p.Length);
            var stringBuilder = new StringBuilder(capacity);
            stringBuilder.AppendFormat(format, args);
            return stringBuilder.ToString();
        }

        #endregion

        #region IEnumerable扩展方法

        /// <summary>
        /// 将指定类型列表转换为T-Sql的In语句的类型
        /// </summary>        
        /// <returns></returns>
        public static string ToSqlInContent<TSource>(this IEnumerable<TSource> source)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var content in source)
                {
                    sb.Append("'");
                    sb.Append(content.ToString());
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
        /// 将指定类型的列表转换为以逗号分开的字符串，如（"1,2,3"）
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static string ToColumnString<TSource>(this IEnumerable<TSource> sources)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                foreach (var content in sources)
                {
                    if (sb.Length > 0)
                        sb.Append(CommomConst.COMMA);

                    sb.Append(content);
                }

                if (sb.Length > 0)
                    return sb.ToString();
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 转换为一个DataTable
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TResult>(this IEnumerable<TResult> value) where TResult : class
        {
            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof(TResult);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach<PropertyInfo>(type.GetProperties(), p =>
            {
                pList.Add(p);
                dt.Columns.Add(p.Name);
            });
            foreach (var item in value)
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
        /// Determines if the IEnumerable is null or empty and throws an ArgumentNullException if it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> Item, string Name)
        {
            if (Item.IsNullOrEmpty())
                throw new ArgumentNullException(Name);
        }

        #endregion

        #region Functions

        #region Exists

        /// <summary>
        /// Used to determine if an item in the IEnumerable matches a predicate
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">List to search</param>
        /// <param name="Match">The predicate used to check if something exists</param>
        /// <returns>True if at least one item matches the predicate, false otherwise</returns>
        public static bool Exists<T>(this IEnumerable<T> List, Predicate<T> Match)
        {
            Match.ThrowIfNull("Match");
            if (List.IsNull())
                return false;
            foreach (T Item in List)
            {
                if (Match(Item))
                    return true;
            }
            return false;
        }

        #endregion

        #region For

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> For<T>(this IEnumerable<T> List, int Start, int End, Action<T> Action)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            int x = 0;
            foreach (T Item in List)
            {
                if (x.Between(Start, End))
                    Action(Item);
                ++x;
                if (x > End)
                    break;
            }
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable between the start and end indexes and returns an IEnumerable of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> For<T, R>(this IEnumerable<T> List, int Start, int End, Func<T, R> Function)
        {
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            int x = 0;
            List<R> ReturnValues = new List<R>();
            foreach (T Item in List)
            {
                if (x.Between(Start, End))
                    ReturnValues.Add(Function(Item));
                ++x;
                if (x > End)
                    break;
            }
            return ReturnValues;
        }

        #endregion

        #region ForEach

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> List, Action<T> Action)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            foreach (T Item in List)
                Action(Item);
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> List, Func<T, R> Function)
        {
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            List<R> ReturnValues = new List<R>();
            foreach (T Item in List)
                ReturnValues.Add(Function(Item));
            return ReturnValues;
        }

        #endregion

        #region ForParallel

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForParallel<T>(this IEnumerable<T> List, int Start, int End, Action<T> Action)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            Parallel.For(Start, End + 1, new Action<int>(x => Action(List.ElementAt(x))));
            return List;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForParallel<T, R>(this IEnumerable<T> List, int Start, int End, Func<T, R> Function)
        {
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            R[] Results = new R[(End + 1) - Start];
            Parallel.For(Start, End + 1, new Action<int>(x => Results[x - Start] = Function(List.ElementAt(x))));
            return Results;
        }

        #endregion

        #region ForEachParallel

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> List, Action<T> Action)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            Parallel.ForEach(List, Action);
            return List;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The results in an IEnumerable list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> List, Func<T, R> Function)
        {
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            return List.ForParallel(0, List.Count() - 1, Function);
        }

        #endregion

        #region IsNullOrEmpty

        /// <summary>
        /// Determines if a list is null or empty
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Value">List to check</param>
        /// <returns>True if it is null or empty, false otherwise</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> Value)
        {
            return Value.IsNull() || Value.Count() == 0;
        }

        #endregion

        #region RemoveDefaults

        /// <summary>
        /// Removes default values from a list
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="Value">List to cull items from</param>
        /// <param name="EqualityComparer">Equality comparer used (defaults to GenericEqualityComparer)</param>
        /// <returns>An IEnumerable with the default values removed</returns>
        public static IEnumerable<T> RemoveDefaults<T>(this IEnumerable<T> Value, IEqualityComparer<T> EqualityComparer = null)
        {
            if (Value.IsNull())
                yield break;
            EqualityComparer = EqualityComparer.NullCheck(new GenericEqualityComparer<T>());
            foreach (T Item in Value.Where(x => !EqualityComparer.Equals(x, default(T))))
                yield return Item;
        }

        #endregion

        #region ToArray

        /// <summary>
        /// Converts a list to an array
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="ConvertingFunction">Function used to convert each item</param>
        /// <returns>The array containing the items from the list</returns>
        public static Target[] ToArray<Source, Target>(this IEnumerable<Source> List, Func<Source, Target> ConvertingFunction)
        {
            List.ThrowIfNull("List");
            ConvertingFunction.ThrowIfNull("ConvertingFunction");
            return List.ForEach(ConvertingFunction).ToArray();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Converts the list to a string where each item is seperated by the Seperator
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="ItemOutput">Used to convert the item to a string (defaults to calling ToString)</param>
        /// <param name="Seperator">Seperator to use between items (defaults to ,)</param>
        /// <returns>The string version of the list</returns>
        public static string ToString<T>(this IEnumerable<T> List, Func<T, string> ItemOutput = null, string Seperator = ",")
        {
            List.ThrowIfNull("List");
            Seperator = Seperator.NullCheck("");
            ItemOutput = ItemOutput.NullCheck(x => x.ToString());
            StringBuilder Builder = new StringBuilder();
            string TempSeperator = "";
            List.ForEach(x =>
            {
                Builder.Append(TempSeperator).Append(ItemOutput(x));
                TempSeperator = Seperator;
            });
            return Builder.ToString();
        }

        #endregion

        #region TrueForAll

        /// <summary>
        /// Determines if a predicate is true for each item in a list
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Predicate">Predicate to use to check the IEnumerable</param>
        /// <returns>True if they all pass the predicate, false otherwise</returns>
        public static bool TrueForAll<T>(this IEnumerable<T> List, Predicate<T> Predicate)
        {
            List.ThrowIfNull("List");
            Predicate.ThrowIfNull("Predicate");
            return !List.Any(x => !Predicate(x));
        }

        #endregion

        #region TryAll

        /// <summary>
        /// Tries to do the action on each item in the list. If an exception is thrown,
        /// it does the catch action on the item (if it is not null).
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Action">Action to run on each item</param>
        /// <param name="CatchAction">Catch action (defaults to null)</param>
        /// <returns>The list after the action is run on everything</returns>
        public static IEnumerable<T> TryAll<T>(this IEnumerable<T> List, Action<T> Action, Action<T> CatchAction = null)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            foreach (T Item in List)
            {
                try
                {
                    Action(Item);
                }
                catch { if (CatchAction != null) CatchAction(Item); }
            }
            return List;
        }

        #endregion

        #region TryAllParallel

        /// <summary>
        /// Tries to do the action on each item in the list. If an exception is thrown,
        /// it does the catch action on the item (if it is not null). This is done in
        /// parallel.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Action">Action to run on each item</param>
        /// <param name="CatchAction">Catch action (defaults to null)</param>
        /// <returns>The list after the action is run on everything</returns>
        public static IEnumerable<T> TryAllParallel<T>(this IEnumerable<T> List, Action<T> Action, Action<T> CatchAction = null)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            Parallel.ForEach<T>(List, delegate(T Item)
            {
                try
                {
                    Action(Item);
                }
                catch { if (CatchAction != null) CatchAction(Item); }
            });
            return List;
        }

        #endregion

        #endregion

        #endregion

        #region DateTime 扩展方法

        /// <summary>
        /// 返回指定日期的是星期几，会根据区域信息来返回，如：中文环境为 “星期一”
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToCultureDayOfWeek(this DateTime dateTime)
        {
            try
            {
                return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dateTime.DayOfWeek);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回指定日期的农历日期
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
        /// 验证15位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard18(string Id)
        {
            long num = 0L;
            if (!long.TryParse(Id.Remove(17), out num) || (double)num < Math.Pow(10.0, 16.0) || !long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out num))
            {
                return false;
            }
            string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (text.IndexOf(Id.Remove(2)) == -1)
            {
                return false;
            }
            string s = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime dateTime = default(DateTime);
            if (!DateTime.TryParse(s, out dateTime))
            {
                return false;
            }
            string[] array = "1,0,x,9,8,7,6,5,4,3,2".Split(new char[]
			{
				','
			});
            string[] array2 = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(new char[]
			{
				','
			});
            char[] array3 = Id.Remove(17).ToCharArray();
            int num2 = 0;
            for (int i = 0; i < 17; i++)
            {
                num2 += int.Parse(array2[i]) * int.Parse(array3[i].ToString());
            }
            int num3 = -1;
            Math.DivRem(num2, 11, out num3);
            return !(array[num3] != Id.Substring(17, 1).ToLower());
        }
        /// <summary>
        /// 验证18位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard15(string Id)
        {
            long num = 0L;
            if (!long.TryParse(Id, out num) || (double)num < Math.Pow(10.0, 14.0))
            {
                return false;
            }
            string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (text.IndexOf(Id.Remove(2)) == -1)
            {
                return false;
            }
            string s = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime dateTime = default(DateTime);
            return DateTime.TryParse(s, out dateTime);
        }

        #endregion

        #region Private Static Functions

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        private static object CallMethod(string MethodName, object Object, params object[] InputVariables)
        {
            if (string.IsNullOrEmpty(MethodName) || Object.IsNull())
                return null;
            Type ObjectType = Object.GetType();
            MethodInfo Method = null;
            if (InputVariables.IsNotNull())
            {
                Type[] MethodInputTypes = new Type[InputVariables.Length];
                for (int x = 0; x < InputVariables.Length; ++x)
                    MethodInputTypes[x] = InputVariables[x].GetType();
                Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
                if (Method != null)
                    return Method.Invoke(Object, InputVariables);
            }
            Method = ObjectType.GetMethod(MethodName);
            return Method.IsNull() ? null : Method.Invoke(Object, null);
        }

        #endregion

        #endregion

        #region ReflectionExtensions

        #region Functions

        #region CallMethod

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <returns>The returned value of the method</returns>
        public static ReturnType CallMethod<ReturnType>(this object Object, string MethodName, params object[] InputVariables)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(MethodName))
                throw new ArgumentNullException("MethodName");
            if (InputVariables == null)
                InputVariables = new object[0];
            Type ObjectType = Object.GetType();
            Type[] MethodInputTypes = new Type[InputVariables.Length];
            for (int x = 0; x < InputVariables.Length; ++x)
                MethodInputTypes[x] = InputVariables[x].GetType();
            MethodInfo Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
            if (Method == null)
                throw new NullReferenceException("Could not find method " + MethodName + " with the appropriate input variables.");
            return (ReturnType)Method.Invoke(Object, InputVariables);
        }

        #endregion

        #region CreateInstance

        /// <summary>
        /// Creates an instance of the type and casts it to the specified type
        /// </summary>
        /// <typeparam name="ClassType">Class type to return</typeparam>
        /// <param name="Type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static ClassType CreateInstance<ClassType>(this Type Type, params object[] args)
        {
            if (Type == null)
                throw new ArgumentNullException("Type");
            return (ClassType)Type.CreateInstance(args);
        }

        /// <summary>
        /// Creates an instance of the type
        /// </summary>
        /// <param name="Type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static object CreateInstance(this Type Type, params object[] args)
        {
            if (Type == null)
                throw new ArgumentNullException("Type");
            return Activator.CreateInstance(Type, args);
        }

        #endregion

        #region DumpProperties

        /// <summary>
        /// Dumps the property names and current values from an object
        /// </summary>
        /// <param name="Object">Object to dunp</param>
        /// <param name="HTMLOutput">Determines if the output should be HTML or not</param>
        /// <returns>An HTML formatted table containing the information about the object</returns>
        public static string DumpProperties(this object Object, bool HTMLOutput = true)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            StringBuilder TempValue = new StringBuilder();
            TempValue.Append(HTMLOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            Type ObjectType = Object.GetType();
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                TempValue.Append(HTMLOutput ? "<tr><td>" : "").Append(Property.Name).Append(HTMLOutput ? "</td><td>" : "\t\t\t\t");
                ParameterInfo[] Parameters = Property.GetIndexParameters();
                if (Property.CanRead && Parameters.Length == 0)
                {
                    try
                    {
                        object Value = Property.GetValue(Object, null);
                        TempValue.Append(Value == null ? "null" : Value.ToString());
                    }
                    catch { }
                }
                TempValue.Append(HTMLOutput ? "</td></tr>" : "");
            }
            TempValue.Append(HTMLOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        /// <summary>
        /// Dumps the properties names and current values
        /// from an object type (used for static classes)
        /// </summary>
        /// <param name="ObjectType">Object type to dunp</param>
        /// <returns>An HTML formatted table containing the information about the object type</returns>
        public static string DumpProperties(this Type ObjectType, bool HTMLOutput = true)
        {
            if (ObjectType == null)
                throw new ArgumentNullException("ObjectType");
            StringBuilder TempValue = new StringBuilder();
            TempValue.Append(HTMLOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            PropertyInfo[] Properties = ObjectType.GetProperties();
            foreach (PropertyInfo Property in Properties)
            {
                TempValue.Append(HTMLOutput ? "<tr><td>" : "").Append(Property.Name).Append(HTMLOutput ? "</td><td>" : "\t\t\t\t");
                if (Property.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        TempValue.Append(Property.GetValue(null, null) == null ? "null" : Property.GetValue(null, null).ToString());
                    }
                    catch { }
                }
                TempValue.Append(HTMLOutput ? "</td></tr>" : "");
            }
            TempValue.Append(HTMLOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        #endregion

        #region GetAttribute

        /// <summary>
        /// Gets the attribute from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Provider">Attribute provider</param>
        /// <param name="Inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>Attribute specified if it exists</returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider Provider, bool Inherit = true) where T : Attribute
        {
            return Provider.IsDefined(typeof(T), Inherit) ? Provider.GetAttributes<T>(Inherit)[0] : default(T);
        }

        #endregion

        #region GetAttributes

        /// <summary>
        /// Gets the attributes from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Provider">Attribute provider</param>
        /// <param name="Inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>Array of attributes</returns>
        public static T[] GetAttributes<T>(this ICustomAttributeProvider Provider, bool Inherit = true) where T : Attribute
        {
            return Provider.IsDefined(typeof(T), Inherit) ? Provider.GetCustomAttributes(typeof(T), Inherit).ToArray(x => (T)x) : new T[0];
        }

        #endregion

        #region GetName

        /// <summary>
        /// Returns the type's name (Actual C# name, not the funky version from
        /// the Name property)
        /// </summary>
        /// <param name="ObjectType">Type to get the name of</param>
        /// <returns>string name of the type</returns>
        public static string GetName(this Type ObjectType)
        {
            if (ObjectType == null)
                throw new ArgumentNullException("ObjectType");
            StringBuilder Output = new StringBuilder();
            if (ObjectType.Name == "Void")
            {
                Output.Append("void");
            }
            else
            {
                if (ObjectType.Name.Contains("`"))
                {
                    Type[] GenericTypes = ObjectType.GetGenericArguments();
                    Output.Append(ObjectType.Name.Remove(ObjectType.Name.IndexOf("`")))
                        .Append("<");
                    string Seperator = "";
                    foreach (Type GenericType in GenericTypes)
                    {
                        Output.Append(Seperator).Append(GenericType.GetName());
                        Seperator = ",";
                    }
                    Output.Append(">");
                }
                else
                {
                    Output.Append(ObjectType.Name);
                }
            }
            return Output.ToString();
        }

        #endregion

        #region GetObjects

        /// <summary>
        /// Returns an instance of all classes that it finds within an assembly
        /// that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Assembly">Assembly to search within</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> GetObjects<ClassType>(this Assembly Assembly)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            System.Collections.Generic.List<ClassType> ReturnValues = new System.Collections.Generic.List<ClassType>();
            foreach (Type Type in Assembly.GetTypes<ClassType>().Where(x => !x.ContainsGenericParameters))
                ReturnValues.Add(Type.CreateInstance<ClassType>());
            return ReturnValues;
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within a group of assemblies
        /// that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Assemblies">Assemblies to search within</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> GetObjects<ClassType>(this IEnumerable<Assembly> Assemblies)
        {
            if (Assemblies == null)
                throw new ArgumentNullException("Assemblies");
            List<ClassType> ReturnValues = new List<ClassType>();
            foreach (Assembly Assembly in Assemblies)
                ReturnValues.AddRange(Assembly.GetObjects<ClassType>());
            return ReturnValues;
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within a directory
        /// that are of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Directory">Directory to search within</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<ClassType> GetObjects<ClassType>(this DirectoryInfo Directory, bool Recursive = false)
        {
            if (Directory == null)
                throw new ArgumentNullException("Directory");
            return Directory.LoadAssemblies(Recursive).GetObjects<ClassType>();
        }

        #endregion

        #region GetProperty

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="Property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object GetProperty(this object Object, PropertyInfo Property)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (Property == null)
                throw new ArgumentNullException("Property");
            return Property.GetValue(Object, null);
        }

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="Property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object GetProperty(this object Object, string Property)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(Property))
                throw new ArgumentNullException("Property");
            string[] Properties = Property.Split(new string[] { "." }, StringSplitOptions.None);
            object TempObject = Object;
            Type TempObjectType = TempObject.GetType();
            PropertyInfo DestinationProperty = null;
            for (int x = 0; x < Properties.Length - 1; ++x)
            {
                DestinationProperty = TempObjectType.GetProperty(Properties[x]);
                TempObjectType = DestinationProperty.PropertyType;
                TempObject = DestinationProperty.GetValue(TempObject, null);
                if (TempObject == null)
                    return null;
            }
            DestinationProperty = TempObjectType.GetProperty(Properties[Properties.Length - 1]);
            return TempObject.GetProperty(DestinationProperty);
        }

        #endregion

        #region GetPropertyGetter

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<ClassType, DataType>> GetPropertyGetter<ClassType, DataType>(this PropertyInfo Property)
        {
            if (!IsOfType(Property.PropertyType, typeof(DataType)))
                throw new ArgumentException("Property is not of the type specified");
            if (!IsOfType(Property.DeclaringType, typeof(ClassType)))
                throw new ArgumentException("Property is not from the declaring class type specified");
            ParameterExpression ObjectInstance = Expression.Parameter(Property.DeclaringType, "x");
            MemberExpression PropertyGet = Expression.Property(ObjectInstance, Property);
            if (Property.PropertyType != typeof(DataType))
            {
                UnaryExpression Convert = Expression.Convert(PropertyGet, typeof(DataType));
                return Expression.Lambda<Func<ClassType, DataType>>(Convert, ObjectInstance);
            }
            return Expression.Lambda<Func<ClassType, DataType>>(PropertyGet, ObjectInstance);
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<ClassType, object>> GetPropertyGetter<ClassType>(this PropertyInfo Property)
        {
            return Property.GetPropertyGetter<ClassType, object>();
        }

        #endregion

        #region GetPropertyName

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type of the property</typeparam>
        /// <param name="Expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string GetPropertyName<ClassType, DataType>(this Expression<Func<ClassType, DataType>> Expression)
        {
            if (Expression.Body is UnaryExpression && Expression.Body.NodeType == ExpressionType.Convert)
            {
                MemberExpression Temp = (MemberExpression)((UnaryExpression)Expression.Body).Operand;
                return GetPropertyName(Temp.Expression) + Temp.Member.Name;
            }
            if (!(Expression.Body is MemberExpression))
                throw new ArgumentException("Expression.Body is not a MemberExpression");
            return ((MemberExpression)Expression.Body).Expression.GetPropertyName() + ((MemberExpression)Expression.Body).Member.Name;
        }

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="Expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string GetPropertyName(this Expression Expression)
        {
            if (!(Expression is MemberExpression))
                return "";
            return ((MemberExpression)Expression).Expression.GetPropertyName() + ((MemberExpression)Expression).Member.Name + ".";
        }

        #endregion

        #region GetPropertyType

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="Object">object who contains the property</param>
        /// <param name="PropertyPath">Path of the property (ex: Prop1.Prop2.Prop3 would be
        /// the Prop1 of the source object, which then has a Prop2 on it, which in turn
        /// has a Prop3 on it.)</param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type GetPropertyType(this object Object, string PropertyPath)
        {
            if (Object == null || string.IsNullOrEmpty(PropertyPath))
                return null;
            return Object.GetType().GetPropertyType(PropertyPath);
        }

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="PropertyPath">Path of the property (ex: Prop1.Prop2.Prop3 would be
        /// the Prop1 of the source object, which then has a Prop2 on it, which in turn
        /// has a Prop3 on it.)</param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type GetPropertyType(this Type ObjectType, string PropertyPath)
        {
            if (ObjectType == null || string.IsNullOrEmpty(PropertyPath))
                return null;
            string[] SourceProperties = PropertyPath.Split(new string[] { "." }, StringSplitOptions.None);
            PropertyInfo PropertyInfo = null;
            for (int x = 0; x < SourceProperties.Length; ++x)
            {
                PropertyInfo = ObjectType.GetProperty(SourceProperties[x]);
                ObjectType = PropertyInfo.PropertyType;
            }
            return ObjectType;
        }

        #endregion

        #region GetPropertySetter

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="PropertyName">Property name</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<ClassType, DataType>> GetPropertySetter<ClassType, DataType>(this Expression<Func<ClassType, DataType>> Property)
        {
            if (Property == null)
                throw new ArgumentNullException("Property");
            string PropertyName = Property.GetPropertyName();
            string[] SplitName = PropertyName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            PropertyInfo PropertyInfo = typeof(ClassType).GetProperty(SplitName[0]);
            ParameterExpression ObjectInstance = Expression.Parameter(PropertyInfo.DeclaringType, "x");
            ParameterExpression PropertySet = Expression.Parameter(typeof(DataType), "y");
            MethodCallExpression SetterCall = null;
            MemberExpression PropertyGet = null;
            if (SplitName.Length > 1)
            {
                PropertyGet = Expression.Property(ObjectInstance, PropertyInfo);
                for (int x = 1; x < SplitName.Length - 1; ++x)
                {
                    PropertyInfo = PropertyInfo.PropertyType.GetProperty(SplitName[x]);
                    PropertyGet = Expression.Property(PropertyGet, PropertyInfo);
                }
                PropertyInfo = PropertyInfo.PropertyType.GetProperty(SplitName[SplitName.Length - 1]);
            }
            if (PropertyInfo.PropertyType != typeof(DataType))
            {
                UnaryExpression Convert = Expression.Convert(PropertySet, PropertyInfo.PropertyType);
                if (PropertyGet == null)
                    SetterCall = Expression.Call(ObjectInstance, PropertyInfo.GetSetMethod(), Convert);
                else
                    SetterCall = Expression.Call(PropertyGet, PropertyInfo.GetSetMethod(), Convert);
                return Expression.Lambda<Action<ClassType, DataType>>(SetterCall, ObjectInstance, PropertySet);
            }
            if (PropertyGet == null)
                SetterCall = Expression.Call(ObjectInstance, PropertyInfo.GetSetMethod(), PropertySet);
            else
                SetterCall = Expression.Call(PropertyGet, PropertyInfo.GetSetMethod(), PropertySet);
            return Expression.Lambda<Action<ClassType, DataType>>(SetterCall, ObjectInstance, PropertySet);
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<ClassType, object>> GetPropertySetter<ClassType>(this Expression<Func<ClassType, object>> Property)
        {
            return Property.GetPropertySetter<ClassType, object>();
        }

        #endregion

        #region GetTypes

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes<BaseType>(this Assembly Assembly)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            return Assembly.GetTypes(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
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
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> GetTypes<BaseType>(this IEnumerable<Assembly> Assemblies)
        {
            if (Assemblies == null)
                throw new ArgumentNullException("Assemblies");
            return Assemblies.GetTypes(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
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
            List<Type> ReturnValues = new List<Type>();
            Assemblies.ForEach(y => ReturnValues.AddRange(y.GetTypes(BaseType)));
            return ReturnValues;
        }

        #endregion

        #region IsIEnumerable

        /// <summary>
        /// Simple function to determine if an item is an IEnumerable
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsIEnumerable(this Type ObjectType)
        {
            return ObjectType.IsOfType(typeof(IEnumerable));
        }

        #endregion

        #region IsOfType

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsOfType(this object Object, Type Type)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (Type == null)
                throw new ArgumentNullException("Type");
            return Object.GetType().IsOfType(Type);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsOfType(this Type ObjectType, Type Type)
        {
            if (ObjectType == null)
                return false;
            if (Type == null)
                throw new ArgumentNullException("Type");
            if (Type == ObjectType || ObjectType.GetInterface(Type.Name, true) != null)
                return true;
            if (ObjectType.BaseType == null)
                return false;
            return ObjectType.BaseType.IsOfType(Type);
        }

        #endregion

        #region Load

        /// <summary>
        /// Loads an assembly by its name
        /// </summary>
        /// <param name="Name">Name of the assembly to return</param>
        /// <returns>The assembly specified if it exists</returns>
        public static System.Reflection.Assembly Load(this AssemblyName Name)
        {
            Name.ThrowIfNull("Name");
            return AppDomain.CurrentDomain.Load(Name);
        }

        #endregion

        #region LoadAssemblies

        /// <summary>
        /// Loads assemblies within a directory and returns them in an array.
        /// </summary>
        /// <param name="Directory">The directory to search in</param>
        /// <param name="Recursive">Determines whether to search recursively or not</param>
        /// <returns>Array of assemblies in the directory</returns>
        public static IEnumerable<Assembly> LoadAssemblies(this DirectoryInfo Directory, bool Recursive = false)
        {
            foreach (FileInfo File in Directory.GetFiles("*.dll", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                yield return AssemblyName.GetAssemblyName(File.FullName).Load();
        }

        #endregion

        #region MarkedWith

        /// <summary>
        /// Goes through a list of types and determines if they're marked with a specific attribute
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Types">Types to check</param>
        /// <param name="Inherit">When true, it looks up the heirarchy chain for the inherited custom attributes</param>
        /// <returns>The list of types that are marked with an attribute</returns>
        public static IEnumerable<Type> MarkedWith<T>(this IEnumerable<Type> Types, bool Inherit = true) where T : Attribute
        {
            if (Types == null)
                return null;
            return Types.Where(x => x.IsDefined(typeof(T), Inherit) && !x.IsAbstract);
        }

        #endregion

        #region MakeShallowCopy

        /// <summary>
        /// Makes a shallow copy of the object
        /// </summary>
        /// <param name="Object">Object to copy</param>
        /// <param name="SimpleTypesOnly">If true, it only copies simple types (no classes, only items like int, string, etc.), false copies everything.</param>
        /// <returns>A copy of the object</returns>
        public static T MakeShallowCopy<T>(this T Object, bool SimpleTypesOnly = false)
        {
            if (Object == null)
                return default(T);
            Type ObjectType = Object.GetType();
            T ClassInstance = ObjectType.CreateInstance<T>();
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
                catch { }
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
                catch { }
            }

            return ClassInstance;
        }

        #endregion

        #region SetProperty

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static object SetProperty(this object Object, PropertyInfo Property, object Value, string Format = "")
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (Property == null)
                throw new ArgumentNullException("Property");
            if (Value == null)
                throw new ArgumentNullException("Value");
            if (Property.PropertyType == typeof(string))
                Value = Value.FormatToString(Format);
            if (!Value.GetType().IsOfType(Property.PropertyType))
                Value = Convert.ChangeType(Value, Property.PropertyType);
            Property.SetValue(Object, Value, null);
            return Object;
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        public static object SetProperty(this object Object, string Property, object Value, string Format = "")
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (string.IsNullOrEmpty(Property))
                throw new ArgumentNullException("Property");
            if (Value == null)
                throw new ArgumentNullException("Value");
            string[] Properties = Property.Split(new string[] { "." }, StringSplitOptions.None);
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
            TempObject.SetProperty(DestinationProperty, Value, Format);
            return Object;
        }

        #endregion

        #region ToLongVersionString

        /// <summary>
        /// Gets the long version of the version information
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
        /// Gets the short version of the version information
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
        /// Checks if an item is between two values
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
        /// Does a null check and either returns the default value (if it is null) or the object
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
        /// Determines if the object is not null
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
        /// Determines if the object is null
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
        /// Determines if the object is equal to default value and throws an ArgumentNullException if it is
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
        /// Converts a .Net type to SQLDbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding SQLDbType</returns>
        public static SqlDbType ToSQLDbType(this Type Type)
        {
            return Type.ToDbType().ToSqlDbType();
        }

        /// <summary>
        /// Converts a DbType to a SqlDbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding SqlDbType (if it exists)</returns>
        public static SqlDbType ToSqlDbType(this DbType Type)
        {
            SqlParameter Parameter = new SqlParameter();
            Parameter.DbType = Type;
            return Parameter.SqlDbType;
        }

        #endregion

        #region ToDbType

        /// <summary>
        /// Converts a .Net type to DbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding DbType</returns>
        public static DbType ToDbType(this Type Type)
        {
            if (Type == typeof(byte)) return DbType.Byte;
            else if (Type == typeof(sbyte)) return DbType.SByte;
            else if (Type == typeof(short)) return DbType.Int16;
            else if (Type == typeof(ushort)) return DbType.UInt16;
            else if (Type == typeof(int)) return DbType.Int32;
            else if (Type == typeof(uint)) return DbType.UInt32;
            else if (Type == typeof(long)) return DbType.Int64;
            else if (Type == typeof(ulong)) return DbType.UInt64;
            else if (Type == typeof(float)) return DbType.Single;
            else if (Type == typeof(double)) return DbType.Double;
            else if (Type == typeof(decimal)) return DbType.Decimal;
            else if (Type == typeof(bool)) return DbType.Boolean;
            else if (Type == typeof(string)) return DbType.String;
            else if (Type == typeof(char)) return DbType.StringFixedLength;
            else if (Type == typeof(Guid)) return DbType.Guid;
            else if (Type == typeof(DateTime)) return DbType.DateTime2;
            else if (Type == typeof(DateTimeOffset)) return DbType.DateTimeOffset;
            else if (Type == typeof(byte[])) return DbType.Binary;
            else if (Type == typeof(byte?)) return DbType.Byte;
            else if (Type == typeof(sbyte?)) return DbType.SByte;
            else if (Type == typeof(short?)) return DbType.Int16;
            else if (Type == typeof(ushort?)) return DbType.UInt16;
            else if (Type == typeof(int?)) return DbType.Int32;
            else if (Type == typeof(uint?)) return DbType.UInt32;
            else if (Type == typeof(long?)) return DbType.Int64;
            else if (Type == typeof(ulong?)) return DbType.UInt64;
            else if (Type == typeof(float?)) return DbType.Single;
            else if (Type == typeof(double?)) return DbType.Double;
            else if (Type == typeof(decimal?)) return DbType.Decimal;
            else if (Type == typeof(bool?)) return DbType.Boolean;
            else if (Type == typeof(char?)) return DbType.StringFixedLength;
            else if (Type == typeof(Guid?)) return DbType.Guid;
            else if (Type == typeof(DateTime?)) return DbType.DateTime2;
            else if (Type == typeof(DateTimeOffset?)) return DbType.DateTimeOffset;
            return DbType.Int32;
        }

        /// <summary>
        /// Converts SqlDbType to DbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding DbType (if it exists)</returns>
        public static DbType ToDbType(this SqlDbType Type)
        {
            SqlParameter Parameter = new SqlParameter();
            Parameter.SqlDbType = Type;
            return Parameter.DbType;
        }

        #endregion

        #region ToType

        /// <summary>
        /// Converts a SQLDbType value to .Net type
        /// </summary>
        /// <param name="Type">SqlDbType Type</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ToType(this SqlDbType Type)
        {
            return Type.ToDbType().ToType();
        }

        /// <summary>
        /// Converts a DbType value to .Net type
        /// </summary>
        /// <param name="Type">DbType</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ToType(this DbType Type)
        {
            if (Type == DbType.Byte) return typeof(byte);
            else if (Type == DbType.SByte) return typeof(sbyte);
            else if (Type == DbType.Int16) return typeof(short);
            else if (Type == DbType.UInt16) return typeof(ushort);
            else if (Type == DbType.Int32) return typeof(int);
            else if (Type == DbType.UInt32) return typeof(uint);
            else if (Type == DbType.Int64) return typeof(long);
            else if (Type == DbType.UInt64) return typeof(ulong);
            else if (Type == DbType.Single) return typeof(float);
            else if (Type == DbType.Double) return typeof(double);
            else if (Type == DbType.Decimal) return typeof(decimal);
            else if (Type == DbType.Boolean) return typeof(bool);
            else if (Type == DbType.String) return typeof(string);
            else if (Type == DbType.StringFixedLength) return typeof(char);
            else if (Type == DbType.Guid) return typeof(Guid);
            else if (Type == DbType.DateTime2) return typeof(DateTime);
            else if (Type == DbType.DateTime) return typeof(DateTime);
            else if (Type == DbType.DateTimeOffset) return typeof(DateTimeOffset);
            else if (Type == DbType.Binary) return typeof(byte[]);
            return typeof(int);
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
