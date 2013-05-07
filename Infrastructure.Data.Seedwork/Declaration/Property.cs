using System;
using System.Data;
using Infrastructure.Crosscutting.Utility.CustomAttribute;
using Infrastructure.Data.Seedwork.Resources;

namespace Infrastructure.Data.Seedwork.Declaration
{
    #region CRTableKind
    /// <summary>
    /// 这个enum内部顺序是不能变的
    /// </summary>
    public enum CRTableKind : byte
    {
        [LocalizableDescription("CRTableKind_Region", typeof(Messages))]
        Region,

        [LocalizableDescription("CRTableKind_Store", typeof(Messages))]
        Store,

        [LocalizableDescription("CRTableKind_Department", typeof(Messages))]
        Department,

        [LocalizableDescription("CRTableKind_Employee", typeof(Messages))]
        Employee,

        [LocalizableDescription("CRTableKind_MenuCategory", typeof(Messages))]
        MenuCategory,

        [LocalizableDescription("CRTableKind_MenuSet", typeof(Messages))]
        MenuSet,

        [LocalizableDescription("CRTableKind_Combo", typeof(Messages))]
        Combo,

        [LocalizableDescription("CRTableKind_Cater", typeof(Messages))]
        Cater,

        [LocalizableDescription("CRTableKind_Organization", typeof(Messages))]
        Organization,

        [LocalizableDescription("CRTableKind_Customer", typeof(Messages))]
        Customer,
        
        [LocalizableDescription("CRTableKind_Supplier", typeof(Messages))]
        Supplier,

        [LocalizableDescription("CRTableKind_Warehouse", typeof(Messages))]
        Warehouse,

        [LocalizableDescription("CRTableKind_MaterialCategory", typeof(Messages))]
        MaterialCategory,

        [LocalizableDescription("CRTableKind_Material", typeof(Messages))]
        Material,

        [LocalizableDescription("CRTableKind_Section", typeof(Messages))]
        Section,

        [LocalizableDescription("CRTableKind_Station", typeof(Messages))]
        Station,

        [LocalizableDescription("CRTableKind_Ticket", typeof(Messages))]
        Ticket,

        [LocalizableDescription("CRTableKind_Resv", typeof(Messages))]
        Resv,

        [LocalizableDescription("CRTableKind_Card", typeof(Messages))]
        Card
    }
    #endregion

    #region CRPropertyTargetTable
    public static class CRTableKindDataProvider
    {
        public static CRTableKind[] GetOptions()
        {
            CRTableKind[] sta = new CRTableKind[(int)CRTableKind.Card + 1];
            for (byte i = (byte)CRTableKind.Region; i <= (byte)CRTableKind.Card; i++)
                sta[i] = (CRTableKind)i;
            return sta;
        }
    }
    #endregion

    [Serializable]
    public class CRPropertyValue
    {
        #region Data Member
        public int CRPropertyId { get; private set; }
        public int? CRPropertyOptionId { get; set; }
        #endregion

        #region ctor
        public CRPropertyValue(DataRow dr)
        {
            CRPropertyId = (int)dr["CRPropertyId"];
            CRPropertyOptionId = (int)dr["CRPropertyOptionId"];
        }

        public CRPropertyValue(int crPropertyId, int? crPropertyOptionId)
        {
            CRPropertyId = crPropertyId;
            CRPropertyOptionId = crPropertyOptionId;
        }

        public CRPropertyValue(CRPropertyValue src)
        {
            CRPropertyId = src.CRPropertyId;
            CRPropertyOptionId = src.CRPropertyOptionId;
        }
        #endregion
    }
}
