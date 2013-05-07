/*
 * 名称：用户信息
 * 功能：用于将用户信息增加到HTTP请求中。
 * 创建者：吉桂昕
 * 创建时间：2012-06-21 14:01:44
 * 修改时间：
 * 备注： 
*/
using System;
using System.Security.Principal;
using System.Web.Script.Serialization;

namespace Infrastructure.CrossCutting.Web.Core.Security
{ 
    public class UserInfo : IPrincipal
    {
        public int UserId;
        public int GroupId;
        public string UserName;

        // 如果还有其它的用户信息，可以继续添加。 
        public override string ToString()
        {
            return string.Format("UserId: {0}, GroupId: {1}, UserName: {2}, IsAdmin: {3}",
                UserId, GroupId, UserName, IsInRole("Admin"));
        }

        #region IPrincipal Members

        [ScriptIgnore]
        public IIdentity Identity
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInRole(string role)
        {
            if (string.Compare(role, "Admin", true) == 0)
                return GroupId == 1;
            else
                return GroupId > 0;
        }

        #endregion
    }


}
