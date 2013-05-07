using System.Data;

namespace Infrastructure.Data.Seedwork.Declaration
{
    /// <summary>
    /// 在服务器端，对应一个登录某个应用模块的员工Employee或门店员工StoreEmployee
    /// </summary>
    public abstract class LoginClientInfo
    {
        #region Member
        public int SysId { get; private set; }
        public string Code { get; private set; }
        public string EmplNum { get; private set; }
        public string Pwd { get; private set; }
        public string Title { get; private set; }
        public int? RoleId { get; private set; }
        
        public ClientInfo ClientData { get; set; }

        /// <summary>
        /// 当前login client是否将服务器锁定
        /// </summary>
        public bool IsExclusiveLocked { get; private set; }
        #endregion

        #region ctor
        public LoginClientInfo(DataRow drEmpl, ClientInfo clientData, bool isExclusive)
        {
            SysId = (int)drEmpl["EmplId"];
            Code = (string)drEmpl["Code"];
            EmplNum = (string)drEmpl["EmplNum"];
            Pwd = (string)drEmpl["Password"];
            Title = (string)drEmpl["Title"];
            RoleId = ModelBase.Get<int?>(drEmpl, "RoleId");

            ClientData = clientData;

            IsExclusiveLocked = isExclusive;
        }
        #endregion
    }

    public class EnterpriseLoginClientInfo : LoginClientInfo
    {
        public string StoreCode { get; private set; }

        public EnterpriseLoginClientInfo(DataRow drEmpl, ClientInfo clientData, bool isExclusive, string storeCode)
            : base(drEmpl, clientData, isExclusive)
        {
            StoreCode = storeCode;
        }
    }

    public class StoreLoginClientInfo : LoginClientInfo
    {
        // 此员工可见的根级就餐区
        public int? RootSectionId { get; private set; }

        public StoreLoginClientInfo(DataRow drEmpl, ClientInfo clientData, bool isExclusive)
            : base(drEmpl, clientData, isExclusive)
        {
            RootSectionId = ModelBase.Get<int?>(drEmpl, "RootSectionId");
        }
    }
}
