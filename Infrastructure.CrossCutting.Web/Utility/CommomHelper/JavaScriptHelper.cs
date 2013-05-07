using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Infrastructure.CrossCutting.Web.Utility.CommomHelper
{
    /// <summary>
    /// JavaScript 操作类
    /// </summary>
    public class JavaScriptHelper
    {
        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseScript(Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>" + script + "</script>");
        }
        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="value">-1/1</param>
        public static void GoHistory(int value)
        {
            string format = "<Script language='JavaScript'>\r\n                    history.go({0});  \r\n                  </Script>";
            HttpContext.Current.Response.Write(string.Format(format, value));
        }
        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        public static void CloseWindow()
        {
            string s = "<Script language='JavaScript'>\r\n                    parent.opener=null;window.close();  \r\n                  </Script>";
            HttpContext.Current.Response.Write(s);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 刷新父窗口
        /// </summary>
        public static void RefreshParent(string url)
        {
            string s = string.Concat(new string[]
			{
				"<script>try{top.location=\"", 
				url, 
				"\"}catch(e){location=\"", 
				url, 
				"\"}</script>"
			});
            HttpContext.Current.Response.Write(s);
        }
        /// <summary>
        /// 刷新打开窗口
        /// </summary>
        public static void RefreshOpener()
        {
            string s = "<Script language='JavaScript'>\r\n                    opener.location.reload();\r\n                  </Script>";
            HttpContext.Current.Response.Write(s);
        }
        /// <summary>
        /// 转向Url指定的页面
        /// </summary>
        /// <param name="url">连接地址</param>
        public static void JavaScriptLocationHref(string url)
        {
            string text = "<Script language='JavaScript'>\r\n                    window.location.replace('{0}');\r\n                  </Script>";
            text = string.Format(text, url);
            HttpContext.Current.Response.Write(text);
        }
        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">连接地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param>
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            string features = string.Concat(new string[]
			{
				"dialogWidth:", 
				width.ToString(), 
				"px;dialogHeight:", 
				height.ToString(), 
				"px;dialogLeft:", 
				left.ToString(), 
				"px;dialogTop:", 
				top.ToString(), 
				"px;center:yes;help=no;resizable:no;status:no;scroll=yes"
			});
            JavaScriptHelper.ShowModalDialogWindow(webFormUrl, features);
        }
        /// <summary>
        /// 打开模式对话框
        /// </summary>
        /// <param name="webFormUrl">链接地址</param>
        /// <param name="features"></param>
        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string s = JavaScriptHelper.ShowModalDialogJavascript(webFormUrl, features);
            HttpContext.Current.Response.Write(s);
        }
        /// <summary>
        /// 打开模式对话框
        /// </summary>
        /// <param name="webFormUrl"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            return string.Concat(new string[]
			{
				"<script language=javascript>\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\tshowModalDialog('", 
				webFormUrl, 
				"','','", 
				features, 
				"');</script>"
			});
        }
        /// <summary>
        /// 打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        /// <param name="top">头位置</param>
        /// <param name="left">左位置</param>
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {
            string s = string.Concat(new object[]
			{
				"<Script language='JavaScript'>window.open('", 
				url, 
				"','','height=", 
				heigth, 
				",width=", 
				width, 
				",top=", 
				top, 
				",left=", 
				left, 
				",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>"
			});
            HttpContext.Current.Response.Write(s);
        }
        /// <summary>
        /// 页面跳转（跳出框架）
        /// </summary>
        /// <param name="url"></param>
        public static void JavaScriptExitIfream(string url)
        {
            string text = "<Script language='JavaScript'>\r\n                    parent.window.location.replace('{0}');\r\n                  </Script>";
            text = string.Format(text, url);
            HttpContext.Current.Response.Write(text);
        }
    }
}
