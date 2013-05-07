using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Infrastructure.CrossCutting.Web.Utility;

namespace Infrastructure.CrossCutting.Web.Declaration
{
    public class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// 此方法判断是否登录
        /// </summary>
        public void LoginAuthorizationed()
        {
            if (!SessionManage.LoginOk) //成功能登录
            {
                HttpContext.Current.Response.Redirect(SessionManage.TargetPath);  //导向目录页面。
            }
        }
    }
}
