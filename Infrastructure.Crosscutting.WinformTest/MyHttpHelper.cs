﻿///<summary>
/// 类说明：HttpHelps类，用来实现Http访问，Post或者Get方式的，直接访问，带Cookie的，带证书的等方式
///</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace Infrastructure.Crosscutting.WinformTest
{
    public class HttpHelps
    {
        #region 预定义方法或者变更

        //默认的编码
        public Encoding encoding = Encoding.Default;
        //HttpWebRequest对象用来发起请求
        private HttpWebRequest request = null;
        //获取影响流的数据对象
        private HttpWebResponse response = null;
        public Boolean isToLower = true;
        //读取流的对象
        private StreamReader reader = null;
        //需要返回的数据对象
        private string returnData = "String Error";

        ///<summary>
        /// 根据相传入的数据，得到相应页面数据
        ///</summary>
        ///<param name="strPostdata">传入的数据Post方式,get方式传NUll或者空字符串都可以</param>
        ///<returns>string类型的响应数据</returns>
        private string GetHttpRequestData(string strPostdata)
        {
            try
            {
                //支持跳转页面，查询结果将是跳转后的页面
                request.AllowAutoRedirect = true;

                //验证在得到结果时是否有传入数据
                if (!string.IsNullOrEmpty(strPostdata) && request.Method.Trim().ToLower().Contains("post"))
                {
                    byte[] buffer = encoding.GetBytes(strPostdata);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                //最大连接数
                request.ServicePoint.ConnectionLimit = 1024;

                #region 得到请求的response

                using (response = (HttpWebResponse)request.GetResponse())
                {
                    //从这里开始我们要无视编码了
                    if (encoding == null)
                    {
                        MemoryStream _stream = new MemoryStream();

                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //开始读取流并设置编码方式
                            new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                            ////.net4.0以下写法
                            //GetMemoryStream(_stream);
                        }
                        else
                        {
                            response.GetResponseStream().CopyTo(_stream, 10240);
                            ////.net4.0以下写法
                            //GetMemoryStream(_stream);
                        }
                        byte[] RawResponse = _stream.ToArray();
                        string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                        //<meta(.*?)charset([\s]?)=[^>](.*?)>
                        Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                        returnData = encoding.GetString(RawResponse);
                    }
                    else
                    {
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //开始读取流并设置编码方式
                            using (reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), encoding))
                            {
                                returnData = reader.ReadToEnd();
                            }
                        }
                        else
                        {
                            //开始读取流并设置编码方式
                            using (reader = new StreamReader(response.GetResponseStream(), encoding))
                            {
                                returnData = reader.ReadToEnd();
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception)
            {
                //这里是在发生异常时返回的错误信息
                returnData = "String Error";
            }
            if (isToLower)
            {
                returnData = returnData.ToLower();
            }
            return returnData;
        }

        ///<summary>
        /// 4.0以下.net版本取水运
        ///</summary>
        ///<param name="_stream"></param>
        private static void GetMemoryStream(MemoryStream _stream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = _stream.Read(buffer, 0, Length);
            // write the required bytes  
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = _stream.Read(buffer, 0, Length);
            }
        }

        ///<summary>
        /// 为请求准备参数
        ///</summary>
        ///<param name="_URL">请求的URL地址</param>
        ///<param name="_Method">请求方式Get或者Post</param>
        ///<param name="_Accept">Accept</param>
        ///<param name="_ContentType">ContentType返回类型</param>
        ///<param name="_UserAgent">UserAgent客户端的访问类型，包括浏览器版本和操作系统信息</param>
        ///<param name="_Encoding">读取数据时的编码方式</param>
        private void SetRequest(string _URL, string _Method, string _Accept, string _ContentType, string _UserAgent, Encoding _Encoding)
        {
            //初始化对像，并设置请求的URL地址
            request = (HttpWebRequest)WebRequest.Create(GetUrl(_URL));
            //请求方式Get或者Post
            request.Method = _Method;
            //Accept
            request.Accept = _Accept;
            //ContentType返回类型
            request.ContentType = _ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            request.UserAgent = _UserAgent;
            //读取数据时的编码方式
            encoding = _Encoding;
        }

        //取当前webBrowser登录后的Cookie值
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref int pcchCookieData, int dwFlags, object lpReserved);

        //取出Cookie，当登录后才能取
        public static string GetCookieString(string url)
        {
            // Determine the size of the cookie     
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, null))
            {
                if (datasize < 0)
                    return null;
                // Allocate stringbuilder large enough to hold the cookie     
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, null))
                    return null;
            }
            return cookieData.ToString();
        }

        #endregion

        #region 普通类型

        ///<summary>
        /// 采用https协议GET|POST方式访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="_URL"></param>
        ///<param name="_Method">请求方式Get或者Post</param>
        ///<param name="_Accept">Accept</param>
        ///<param name="_ContentType">ContentType返回类型</param>
        ///<param name="_UserAgent">UserAgent客户端的访问类型，包括浏览器版本和操作系统信息</param>
        ///<param name="_Encoding">读取数据时的编码方式</param>
        ///<param name="_Postdata">只有_Method为Post方式时才需要传入值</param>
        ///<returns>返回Html源代码</returns>
        public string GetHttpRequestString(string _URL, string _Method, string _Accept, string _ContentType, string _UserAgent, Encoding _Encoding, string _Postdata)
        {
            //准备参数
            SetRequest(_URL, _Method, _Accept, _ContentType, _UserAgent, _Encoding);
            //调用专门读取数据的类
            return GetHttpRequestData(_Postdata);
        }

        ///<summary>
        ///采用https协议GET方式访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="objencoding">编码方式例如：System.Text.Encoding.UTF8;</param>
        ///<returns>String类型的数据</returns>
        public string GetHttpRequestStringByNUll_Get(string URL, Encoding objencoding)
        {
            //准备参数
            SetRequest(URL, "GET", "text/html, application/xhtml+xml, */*", "text/html", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", objencoding);
            //调用专门读取数据的类
            return GetHttpRequestData("");
        }

        ///<summary>
        ///采用https协议GET方式访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="objencoding">编码方式例如：System.Text.Encoding.UTF8;</param>
        ///<param name="stgrcookie">Cookie字符串</param>
        ///<returns>String类型的数据</returns>
        public string GetHttpRequestStringByNUll_GetBycookie(string URL, Encoding objencoding, string stgrcookie)
        {
            //准备参数
            SetRequest(URL, "GET", "text/html, application/xhtml+xml, */*", "text/html", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", objencoding);
            request.Headers[HttpRequestHeader.Cookie] = stgrcookie;
            //调用专门读取数据的类
            return GetHttpRequestData("");
        }

        ///<summary>
        ///采用https协议GET方式访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="objencoding">编码方式例如：System.Text.Encoding.UTF8;</param>
        ///<returns>String类型的数据</returns>
        public string GetHttpRequestStringByNUll_Get(string URL, Encoding objencoding, string _Accept, string useragent)
        {
            //准备参数
            SetRequest(URL, "GET", _Accept, "text/html", useragent, objencoding);
            //调用专门读取数据的类
            return GetHttpRequestData("");
        }

        ///<summary>
        ///采用https协议Post方式访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="strPostdata">Post发送的数据</param>
        ///<param name="objencoding">编码方式例如：System.Text.Encoding.UTF8;</param>
        ///<returns>String类型的数据</returns>
        public string GetHttpRequestStringByNUll_Post(string URL, string strPostdata, Encoding objencoding)
        {
            //准备参数
            SetRequest(URL, "post", "text/html, application/xhtml+xml, */*", "text/html", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", objencoding);
            //调用专门读取数据的类
            return GetHttpRequestData(strPostdata);
        }

        /// <summary>
        /// 传入一个正确或不正确的URl，返回正确的URL
        /// </summary>
        /// <param name="URL">url</param>
        /// <returns></returns>
        public static string GetUrl(string URL)
        {
            if (!(URL.Contains("http://") || URL.Contains("https://")))
            {
                URL = "http://" + URL;
            }
            return URL;
        }

        #endregion
    }
}