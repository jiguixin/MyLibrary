using System.Net;
using System.Web;

namespace Infrastructure.CrossCutting.Web.Utility.CommomHelper
{
    /// <summary>
    /// 网络帮助文件
    /// </summary>
    public class NetHelper
    {
        public static string GetLocalIP()
        {
#pragma warning disable 618,612
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
#pragma warning restore 618,612
        }

        #region 穿过代理服务器获得Ip地址,如果有多个IP，则第一个是用户的真实IP，其余全是代理的IP，用逗号隔开
        public static string GetRealIp()
        {
            string UserIP;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null) //得到穿过代理服务器的ip地址
            {
                UserIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString(); 
            }
            else
            {
                UserIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); 
            }
            return UserIP;
        }

        public static string GetRealIp(HttpRequest request)
        {
            string ip = request.Headers.Get("x-forwarded-for");
            if (ip == null || ip.Length == 0 || "unknown".Equals(ip))
            {
                ip = request.Headers.Get("Proxy-Client-IP");
            }
            if (ip == null || ip.Length == 0 || "unknown".Equals(ip))
            {
                ip = request.Headers.Get("WL-Proxy-Client-IP");
            }
            if (ip == null || ip.Length == 0 || "unknown".Equals(ip))
            {
                ip = request.ServerVariables["REMOTE_Addr"].ToString();
            }
            return ip;
        }
        #endregion
    }
}
