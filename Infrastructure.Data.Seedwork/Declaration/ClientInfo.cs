using System;
using System.Collections.Generic;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using System.Net;
using System.Net.Sockets;

namespace Infrastructure.Data.Seedwork.Declaration
{
    /// <summary>
    /// 代表从客户端传到服务器端的一个客户端情况
    /// </summary>
    [Serializable]
    public class ClientInfo
    {
        public AppModule AppModuleData { get; private set; }

        public string HostName { get; private set; }
        public string HostIP { get; private set; }

        public ClientInfo(AppModule appModuleData, string dbConnString)
        {
            string[] tmp = dbConnString.Split(CommomConst.SEMI);
#if DEBUG
            // 给本地测试用的，以后要去掉
            if (tmp[0].Contains("localhost"))
            {
                Dictionary<string, string> setting = FileHelper.ReadConfig();
                tmp[0] = tmp[0].Replace("localhost", setting["LocalHostIP"]);
            }
#endif
            string subnet = tmp[0].Substring("Data Source=".Length);
            subnet = subnet.Substring(0, subnet.LastIndexOf(CommomConst.DOT));

            string single = null;
            HostName = Environment.MachineName;
            IPAddress[] arr = Dns.GetHostAddresses(HostName);
            foreach (IPAddress ip in arr)
            {
                single = ip.ToString();

                // 要根据链接到默认数据库的地址设置使用的是哪个IP网段（如果此工作站支持VPN，可能有多网卡）
                if (ip.AddressFamily == AddressFamily.InterNetwork && single.Substring(0, single.LastIndexOf(CommomConst.DOT)) == subnet)
                {
                    HostIP = single;
                    break;
                }
            }

            AppModuleData = appModuleData;

            if (string.IsNullOrEmpty(HostIP))
                throw new ApplicationException("Cannot find network card, unable to talk to network.");
        }
    }
}
