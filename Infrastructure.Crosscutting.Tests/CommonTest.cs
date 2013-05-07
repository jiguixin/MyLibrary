using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using Infrastructure.Crosscutting.Declaration;
using NUnit.Framework;

namespace Infrastructure.Crosscutting.Tests
{
    [TestFixture]
    public class CommonTest
    {
        [Test]
        public void Text_String_Format()
        {
            Console.WriteLine(MyEnum.Get.ToString("g"));
        }
        
        public enum MyEnum
        {
            Get,
            Post
        }

        [Test]
        public void Convert()
        {
            Console.WriteLine(PinyinHelper.GetPinyin("姐姐"));
        }

        [Test]
        public void Test_1()
        {
            string str = GetNumberStr("adfasdf12.23asdfasad123121dfsdf");
            Console.WriteLine(str);
            List<string> strs = str.Split(' ').ToList();
            string result = str.Replace(" ", "");
            Console.WriteLine(result);
           



            //Console.WriteLine("adfasdf1223.32asdfasdfasdf".GetNumberInt());

            //Console.Write(IsNum("aaa13950.4928bbb11ccc,888"));//你们看下效果就知道了.
        }

        public static string GetNumberStr(string str)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                result = Regex.Replace(str, @"[^\d.\d]", " ");
            }
            return result;
        }

        public string IsNum(String str)
        {
            string ss = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsNumber(str, i) == true)
                {
                    ss += str.Substring(i, 1);
                }
                else
                {
                    if (str.Substring(i, 1) == ",")
                    {
                        ss += str.Substring(i, 1);
                    }
                }

            }
            return ss;
        }
    }

   
} 
