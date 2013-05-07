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

    }
}
