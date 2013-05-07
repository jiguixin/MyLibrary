using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Seedwork.Declaration
{
    public class ApplicationSetting
    {
        public static bool APPNAMESPACE_ACTIVE = true;

        // 从ApplicationSetting中读的
        public static bool IS_SIMPLE_UI = false;

        public static int PHOTO_MAXSIZE = 0;
        public static int BUTTON_MAXSIZE = 0;

        public static string ENTERPRISE_CONTROLLER_IP = null;
        public static string ENTERPRISE_CONTROLLER_NAME = null;
        public static int ENTERPRISE_CONTROLLER_PORT = 0;

        public static int SWEEP_TIMER_INTERVAL = 0;
        public static int SERVER_SHUTDOWN_DELAY = 0;

        public static string LOG_SOURCE = null;
        public static bool LOG_LEVEL = true;
        public static int LOG_MAXLENGTH = 0;
         
        public static bool AUTO_GENERATE_LOOKUP = false;

        /// <summary>
        /// 这个要特殊一些，虽然总部系统没有加密文件，有些model类仍然需要使用store code做前缀，所以默认是5个0。在门店系统中，Store.Controller会根据加密文件覆盖这个值
        /// </summary>
        public static string STORE_CODE = "00000";
    }
}
