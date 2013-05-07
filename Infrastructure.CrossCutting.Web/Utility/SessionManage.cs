using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Infrastructure.CrossCutting.Web.Utility
{
    public class SessionManage
    {
        //存储判断是否成功登录
        public static bool LoginOk
        {
            get
            {
                return (HttpContext.Current.Session["LoginOk"] != null && (bool)HttpContext.Current.Session["LoginOk"]);
            }
            set
            {
                HttpContext.Current.Session["LoginOk"] = value;
            }
        }

        //存储登录验证成功之后的Account
        public static string Account
        {
            get
            {
                return (HttpContext.Current.Session["Account"] != null ? HttpContext.Current.Session["Account"].ToString() : string.Empty);
            }
            set
            {
                HttpContext.Current.Session["Account"] = value;
            }
        }

        //存储用户登录成功之后目标去向。
        public static string TargetPath
        {
           
            get
            {
                return (HttpContext.Current.Session["TargetPath"] != null ? HttpContext.Current.Session["TargetPath"].ToString() : VirtualPathUtility.ToAbsolute("~/Default.aspx"));
            }
            set
            { 
                HttpContext.Current.Session["TargetPath"] = value;
            }
        }
    }
}
