using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Infrastructure.Crosscutting.Utility.CommomHelper
{
    public class HttpHelper
    {
        #region 私有成员

        const string sUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; Trident/4.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; InfoPath.2; .NET4.0C; .NET4.0E; 360SE)";
        const string sContentType = "image/gif, image/jpeg,image/pjpeg, image/pjpeg, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
        const string sRequestEncoding = "ascii";
        const string sPostContentType = "application/x-www-form-urlencoded";
        private static Encoding responseEncoding;

        #endregion

        #region 公开的方法

        /// <summary>
        /// 用post方式请求见面
        /// </summary>
        /// <param name="request">构造的请求对象</param>
        /// <param name="data">POST 和参数</param>
        /// <param name="resEncoding">返回字符串的编码方式，NULL为默认</param>
        /// <returns></returns>
        public static string PostData(HttpWebRequest request, string data, Encoding resEncoding)
        { 
            if (request == null)
                return "";
            responseEncoding = resEncoding;

            request.Method = "POST";

            #region 填充要post的内容

            Encoding encoding = Encoding.GetEncoding(sRequestEncoding);
            byte[] bytesToPost = encoding.GetBytes(data);
            
            StuffPostData(request,bytesToPost);

            #endregion
             
            return GetResponseContent(request);
        }
         
        ///<summary>
        /// Post data到url
        ///</summary>
        ///<param name="data">要post的数据</param>
        /// <param name="resEncoding">返回字符串的编码方式，NULL为默认</param>
        ///<param name="url">目标url</param>
        ///<returns>服务器响应</returns>
        public static string PostData(string data, string url, Encoding resEncoding)
        {
            responseEncoding = resEncoding;
            Encoding encoding = Encoding.GetEncoding(sRequestEncoding);
            byte[] bytesToPost = encoding.GetBytes(data);
            return PostDataToUrl(bytesToPost, url,null,null);
        }

        /// <summary>
        ///使用代理实现Post请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="proxy"></param>
        /// <param name="resEncoding">返回字符串的编码方式，NULL为默认</param>
        /// <returns></returns>
        public static string PostData(string data, string url, IWebProxy proxy, Encoding resEncoding)
        {
            responseEncoding = resEncoding;
            Encoding encoding = Encoding.GetEncoding(sRequestEncoding);
            byte[] bytesToPost = encoding.GetBytes(data);
            return PostDataToUrl(bytesToPost, url,null, proxy);
        } 
     
        ///<summary>
        /// Post data到url
        ///</summary>
        ///<param name="data">要post的数据</param>
        ///<param name="header">HTTP消息头 不能增加如下头:Accept,Connection.Content-Length,Content-Type,Expect,Date,Host,If-Modified-Since,Range,Referer,Transfer-Encoding,User-Agent</param>        
        ///<param name="url">目标url</param>
        /// <param name="resEncoding">返回字符串的编码方式，NULL为默认</param>
        ///<returns>服务器响应</returns>
        public static string PostData(string data, string url, System.Collections.Specialized.NameValueCollection header, Encoding resEncoding)
        {
            responseEncoding = resEncoding;
            Encoding encoding = Encoding.GetEncoding(sRequestEncoding);
            byte[] bytesToPost = encoding.GetBytes(data);
            //return PostDataToHeader(bytesToPost,header, url);
            return PostDataToUrl(bytesToPost, url,header,null);
        }

        ///<summary>
        /// Post data到url
        ///</summary>
        ///<param name="data">要post的数据</param>
        ///<param name="header">HTTP消息头 不能增加如下头:Accept,Connection.Content-Length,Content-Type,Expect,Date,Host,If-Modified-Since,Range,Referer,Transfer-Encoding,User-Agent</param>
        ///<param name="url">目标url</param>
        /// <param name="resEncoding">返回字符串的编码方式，NULL为默认</param>
        ///<returns>服务器响应</returns>
        public static string PostData(string data, string url, System.Collections.Specialized.NameValueCollection header, IWebProxy proxy, Encoding resEncoding)
        {
            responseEncoding = resEncoding;
            Encoding encoding = Encoding.GetEncoding(sRequestEncoding);
            byte[] bytesToPost = encoding.GetBytes(data);
            //return PostDataToHeader(bytesToPost,header, url);
            return PostDataToUrl(bytesToPost, url, header, proxy);
        } 

        /// <summary>
        /// GET 方式请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding">返回字符串的编码方式，NULL为默认</param>
        /// <returns></returns>
        public static string GETDataToUrl(string url,Encoding encoding)
        { 
            return GETDataToUrl(url, null, null, encoding);
        }

        /// <summary>
        /// 用GET方法请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="header">HTTP消息头 不能增加如下头:Accept,Connection.Content-Length,Content-Type,Expect,Date,Host,If-Modified-Since,Range,Referer,Transfer-Encoding,User-Agent</param>
        /// <param name="resEncoding">返回字符串的编码方式，NULL为默认</param>
        /// <returns></returns>
        public static string GETDataToUrl(string url, System.Collections.Specialized.NameValueCollection header, Encoding resEncoding)
        { 
            return GETDataToUrl(url, header, null, resEncoding);
        }

        /// <summary>
        /// 用GET方法请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="header">HTTP消息头 不能增加如下头:Accept,Connection.Content-Length,Content-Type,Expect,Date,Host,If-Modified-Since,Range,Referer,Transfer-Encoding,User-Agent</param>
        /// <param name="proxy">使用的代理</param>
        /// <param name="encoding">返回字符串的编码方式，NULL为默认</param>
        /// <returns></returns>
        public static string GETDataToUrl(string url, System.Collections.Specialized.NameValueCollection header, IWebProxy proxy, Encoding encoding)
        {
            string stringResponse = string.Empty;
            responseEncoding = encoding;
            try
            { 
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.Accept = sContentType;                      
                req.UserAgent = sUserAgent;

                if (header != null)
                    req.Headers.Add(header);
                if (proxy != null)
                    req.Proxy = proxy;
                
                return GetResponseContent(req);
            }
            catch (WebException ex)
            {
            }

            return stringResponse;
        }

        #endregion

        #region 私有方法

        //得到响应的内容
        private static string GetResponseContent(HttpWebRequest request)
        {
            #region 发送post请求到服务器并读取服务器返回信息

            Stream responseStream;

            HttpWebResponse httpWebResponse;

            try
            {
                httpWebResponse = (HttpWebResponse)request.GetResponse();
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            { 
                return string.Format("得到响应的内容发生异常：{0}", e.Message); 
            }

            #endregion

            #region 读取服务器返回信息

            string stringResponse = string.Empty;

            if (responseStream != null)
            {
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                {
                    responseStream = new GZipStream(responseStream, 0);
                }
                if (httpWebResponse.ContentEncoding.ToLower().Contains("deflate"))
                {
                    responseStream = new DeflateStream(responseStream, 0);
                }

                if (responseEncoding != null)
                { 
                    using (StreamReader responseReader =
                    new StreamReader(responseStream,responseEncoding))
                    { 
                        stringResponse = responseReader.ReadToEnd();
                    }
                }
                else
                {
                    using (StreamReader responseReader =
                    new StreamReader(responseStream, Encoding.Default))
                    {
                        stringResponse = responseReader.ReadToEnd();
                    }
                }
                

                responseStream.Close();
            }

            #endregion

            return stringResponse;
        }
          
        ///<summary>
        /// Post data到url
        ///</summary>
        ///<param name="data">要post的数据</param>
        ///<param name="url">目标url</param>
        /// <param name="proxy">要配置的代理</param>
        /// <param name="header">HTTP消息头 不能增加如下头:Accept,Connection.Content-Length,Content-Type,Expect,Date,Host,If-Modified-Since,Range,Referer,Transfer-Encoding,User-Agent</param>
        ///<returns>服务器响应</returns>
        private static string PostDataToUrl(byte[] data,string url,System.Collections.Specialized.NameValueCollection header ,IWebProxy proxy)
        {
            #region 创建httpWebRequest对象

            HttpWebRequest httpRequest = CreateHttpWebRequest(url);

            if (httpRequest == null)
                return string.Empty;

            if (proxy != null)
            {
                httpRequest.Proxy = proxy;
            }

            #endregion

            #region 填充httpWebRequest的基本信息 

            if (header != null)
            {
                StuffHttpWebRequest(httpRequest);
                httpRequest.Headers.Add(header);
            }
            else
            {
                StuffHttpWebRequest(httpRequest);
            }
             
            #endregion

            StuffPostData(httpRequest, data);

            return GetResponseContent(httpRequest);
        }

        //创建httpWebRequest对象
        private static HttpWebRequest CreateHttpWebRequest(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);

            HttpWebRequest httpRequest = webRequest as HttpWebRequest;

            if (httpRequest == null)
            {
                throw new ApplicationException(
                    string.Format("Invalid url string: {0}", url)
                    );
            }
            return httpRequest;
        }

        //填充httpWebRequest的基本信息
        private static void StuffHttpWebRequest(HttpWebRequest httpRequest)
        {
            httpRequest.UserAgent = sUserAgent;
            
            //些处必须加该类型，就算是用到sContentType里面也不行。
            httpRequest.ContentType = sPostContentType;
            httpRequest.CookieContainer = new CookieContainer();
            httpRequest.ServicePoint.Expect100Continue = false;
            httpRequest.Method = "POST";
            httpRequest.KeepAlive = false;


            #region 填充要post的内容

            //  StuffPostData(httpRequest, data);

            #endregion
        } 

        private static void StuffPostData(HttpWebRequest httpRequest, byte[] data)
        {
            httpRequest.ContentLength = data.Length;

            Stream requestStream = httpRequest.GetRequestStream();

            requestStream.Write(data, 0, data.Length);

            requestStream.Close();
        }

        #endregion

        /////<summary>
        ///// Post data到url
        /////</summary>
        /////<param name="data">要post的数据</param>
        /////<param name="header">HTTP消息头</param>
        /////<param name="url">目标url</param>
        /////<returns>服务器响应</returns>
        //private static string PostDataToHeader(byte[] data, System.Collections.Specialized.NameValueCollection header, string url)
        //{

        //    #region 创建httpWebRequest对象

        //    WebRequest webRequest = WebRequest.Create(url);

        //    HttpWebRequest httpRequest = webRequest as HttpWebRequest;

        //    if (httpRequest == null)
        //    {
        //        return string.Empty;
        //    }

        //    #endregion

        //    #region 填充httpWebRequest的基本信息

        //    httpRequest.UserAgent = sUserAgent; 
        //    httpRequest.ContentType = sContentType;
        //    httpRequest.Accept =
        //        "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
        //    httpRequest.Headers.Add(header);
        //    httpRequest.CookieContainer = new CookieContainer();
        //    //httpRequest.ServicePoint.Expect100Continue = false;
        //    httpRequest.Method = "POST";

        //    #endregion

        //    #region 填充要post的内容

        //    httpRequest.ContentLength = data.Length;

        //    Stream requestStream = httpRequest.GetRequestStream();

        //    requestStream.Write(data, 0, data.Length);

        //    requestStream.Close();

        //    #endregion

        //    return GetResponseContent(httpRequest);

        //}

        //得到HTTP响应的字符串

        //private static string PostDataToUrl(byte[] data, string url, IWebProxy proxy)
        //{
        //    #region 创建httpWebRequest对象

        //    HttpWebRequest httpRequest = CreateHttpWebRequest(url);

        //    if (httpRequest == null)
        //        return string.Empty;

        //    if (proxy != null)
        //    {
        //        httpRequest.Proxy = proxy;
        //    }

        //    #endregion

        //    #region 填充httpWebRequest的基本信息


        //    StuffHttpWebRequest(httpRequest, data);

        //    #endregion

        //    return GetResponseContent(httpRequest);
        //}
    }
     
}

