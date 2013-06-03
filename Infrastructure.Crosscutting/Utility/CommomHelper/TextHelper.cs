using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Declaration;

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

        public static T[] StringToArray<T>(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            string[] arr = str.Split(CommomConst.COMMA);
            T[] result = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                result[i] = (T)Convert.ChangeType(arr[i], typeof(T));

            return result;
        }

        //把字母,数字由半角转化为全角
        /// <summary>
        /// 把字母,数字由半角转化为全角
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>全角字符串</returns>
        public static string ChangeStrToSBC(string str)
        {
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 0)
                    {
                        b[0] = (byte)(b[0] - 32);
                        b[1] = 255;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            //半角  
            string strNew = new string(c);
            return strNew;
        }

        //将字母，数字由全角转化为半角
        /// <summary>
        /// 将字母，数字由全角转化为半角
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>半角字符串</returns>
        public static string ChangeStrToDBC(string str)
        {
            string s = str;
            char[] c = s.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            //半角  
            string news = new string(c);
            return news;
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
    }
}
