
using Infrastructure.Crosscutting.Utility.CustomAttribute;
using Infrastructure.Data.Seedwork.Resources;

namespace Infrastructure.Data.Seedwork.Declaration
{
    #region AppModule
    /// <summary>
    /// 这里的顺序千万不能动，不然要影响UserSetting表
    /// </summary>
    public enum AppModule : byte
    {
        [LocalizableDescription("AppModule_EnterpriceDictionary", typeof(Messages))]
        EnterpriceDictionary,

        [LocalizableDescription("AppModule_EnterpriceCard", typeof(Messages))]
        EnterpriceCard,

        [LocalizableDescription("AppModule_EnterpriceIWL", typeof(Messages))]
        EnterpriceIWL,

        [LocalizableDescription("AppModule_BackOffice", typeof(Messages))]
        EnterpriceBackOffice,

        [LocalizableDescription("AppModule_StoreBackOffice", typeof(Messages))]
        StoreBackOffice,

        [LocalizableDescription("AppModule_StorePOS", typeof(Messages))]
        StorePOS,

        [LocalizableDescription("AppModule_StoreResv", typeof(Messages))]
        StoreResv,

        [LocalizableDescription("AppModule_StoreCRM", typeof(Messages))]
        StoreCRM,

        [LocalizableDescription("AppModule_StoreProduction", typeof(Messages))]
        StoreProduction,

        [LocalizableDescription("AppModule_StoreInventory", typeof(Messages))]
        StoreInventory,

        [LocalizableDescription("AppModule_StoreVSync", typeof(Messages))]
        StoreSync,             /*数据同步*/

        [LocalizableDescription("AppModule_StorePrint", typeof(Messages))]
        StorePrint
    }

    public static class EnterpriseAppModuleDataProvider
    {
        public static AppModule[] GetOptions()
        {
            return new AppModule[] 
            { 
                AppModule.EnterpriceDictionary,
                AppModule.EnterpriceCard,
                AppModule.EnterpriceIWL,
                AppModule.EnterpriceBackOffice
            };
        }
    }

    public static class StoreAppModuleDataProvider
    {
        public static AppModule[] GetOptions()
        {
            return new AppModule[]
            {    
                AppModule.StoreBackOffice,
                AppModule.StorePOS,
                AppModule.StoreResv,
                AppModule.StoreCRM,
                AppModule.StoreProduction,
                AppModule.StoreInventory,
                AppModule.StoreSync,
                AppModule.StorePrint
            };
        }
    }
    #endregion

    #region ApplicationMessageKind
    public enum ApplicationMessageKind : byte
    {
        NotDefined,

        Critial,
        Attension,
        Info,
        Warning,
        Error,
        Question
    }
    #endregion

    public enum CalendarKind : byte
    {
        [LocalizableDescription("CalendarKind_Solar", typeof(Messages))]
        Solar,
        [LocalizableDescription("CalendarKind_Lunar", typeof(Messages))]
        Lunar
    }
}
