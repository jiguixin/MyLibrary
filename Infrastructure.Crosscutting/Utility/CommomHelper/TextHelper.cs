using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Declaration;
using System.Text.RegularExpressions;

namespace Infrastructure.Crosscutting.Utility.CommomHelper
{
    public class TextHelper
    { 
        public static bool IsValidPrecesion<T>(T val, uint precesion)
        {
            if (val == null)
                return false;

            string sTest = val.ToString();
            int idx = sTest.LastIndexOf('.');
            if (idx >= 0 && idx < sTest.Length && sTest.Length - (idx + 1) > precesion)
                return false;
            return true;
        }

        /// <summary>
        /// Add leading or tailing "0" so that LENGTH(str) equals to len
        /// </summary>
        public static string SupZero(string str, int len, bool leading)
        {
            while (str.Length < len)
            {
                str = string.Format(leading ? "0{0}" : "{0}0", str);
            }

            return str;
        }

        public static string TodayToString()
        {
            return string.Format("{0}{1}{2}", DateTime.Today.Year, SupZero(DateTime.Today.Month.ToString(), 2, true), SupZero(DateTime.Today.Day.ToString(), 2, true));
        }

        /// <summary>
        /// 生成流水号，不传code 者返回YYMMDDHHmmss格式
        /// </summary>
        /// <param name="code">5位的CODE，不足5位在前台补0</param>
        /// <returns>时间(YYMMDDHHmmss)+code</returns>
        public static string NextDocNum(string code)
        {
            string strDate = DateTime.Now.ToString("yyMMddHHmmss");
            string result = null;

            if (!code.IsEmpty())
            {
                code = code.Trim();

                if (code.Length < 5)
                {
                    string strStuff = string.Empty;
                    for (int i = 0; i < 5 - code.Length; i++)
                        strStuff += "0";
                    code = strStuff + code;
                }

                result = strDate + code;
            }
            else
                result = strDate;
            return result;
        }

        public static string ArrayToString<T>(T[] items)
        {
            if (items == null)
                return null;

            if (typeof(T) == typeof(Guid))
                return "'" + string.Join("','", items) + "'";
            else
                return string.Join(",", items);
        }

        public static string ListToString<T>(IEnumerable<T> items)
        {
            if (items == null || items.Count() == 0)
                return null;

            if (typeof(T) == typeof(Guid))
                return "'" + string.Join("','", items) + "'";
            else
                return string.Join(",", items);
        }

        public static T[] StringToArray<T>(string str, char separator = ',')
        {
            if (string.IsNullOrEmpty(str))
                return null;

            string[] arr = str.Split(separator);
            T[] result = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                result[i] = (T)Convert.ChangeType(arr[i], typeof(T));

            return result;
        }

        /// <summary>
        /// 将英文标点转为中文标点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToCnInterpunction(string input)
        {
            return input.Replace('\'', '’')
                        .Replace('"', '“')
                        .Replace(',', '，')
                        .Replace('.', '。')
                        .Replace('!', '！')
                        .Replace('(', '（')
                        .Replace(')', '）')
                        .Replace(';', '；');
        }

        /// <summary>
        /// 将中文标点转为英文标点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToEngInterpunction(string input)
        {
            return input.Replace('’', '\'')
                        .Replace('“', '"')
                        .Replace('”', '"')
                        .Replace('，', ',')
                        .Replace('。', '.')
                        .Replace('！', '!')
                        .Replace('（', '(')
                        .Replace('）', ')')
                        .Replace('；', ';');
        }

        /// 转全角的函数(SBC case)
        ///
        ///任意字符串
        ///全角字符串
        ///
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///
        public static String ToSBC(String input)
        {
            // 半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new String(c);
        }

        /**/
        // /
        // / 转半角的函数(DBC case)
        // /
        // /任意字符串
        // /半角字符串
        // /
        // /全角空格为12288，半角空格为32
        // /其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        // /
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }
         
         /// <summary>
        /// 清除该字符最后的换行符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimEndLf(string str)
        {
            return str.TrimEnd('\n', '\r');
        }

        /// <summary>
        /// 通过正则表达试得到字符串的中CSS样式
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<string> GetCssContent(string source)
        {
            //匹配CSS文件中的CSS样式
            var reg = @"[\.\#]?\w+[^{]+\{[^}]*\}";
            var lstResult = new List<string>();
            if (!string.IsNullOrEmpty(source))
            {
                var r = new Regex(reg); //定义一个Regex对象实例
                var mc = r.Matches(source);
                for (var i = 0; i < mc.Count; i++) //在输入字符串中找到所有匹配
                {
                    string val = mc[i].Value;

                    if (val.IsNullOrEmpty())
                        continue;

                    lstResult.Add(val.Trim());
                }
            }
            return lstResult;
        }
    }
}
