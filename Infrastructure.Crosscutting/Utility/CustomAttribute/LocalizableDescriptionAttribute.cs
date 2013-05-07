

namespace Infrastructure.Crosscutting.Utility.CustomAttribute
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Resources; 


    /// <summary>
    /// Use this attribute descriptor in front of an enum value so that it can be linked to a localized string in Properties.Strings
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class LocalizableDescriptionAttribute : DescriptionAttribute
    {
        #region Member
        private readonly Type _resourcesType;
        private bool _isLocalized;
        #endregion

        #region ctor
        public LocalizableDescriptionAttribute(string description, Type resourcesType)
            : base(description)
        {
            _resourcesType = resourcesType;
        }
        #endregion

        #region Method
        public override string Description
        {
            get
            {
                if (!_isLocalized)
                {
                    ResourceManager resMgr = _resourcesType.InvokeMember(@"ResourceManager", BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, null, new object[] { }) as ResourceManager;

                    CultureInfo culture = _resourcesType.InvokeMember(@"Culture", BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, null, new object[] { }) as CultureInfo;

                    _isLocalized = true;

                    if (resMgr != null)
                    {
                        DescriptionValue = resMgr.GetString(DescriptionValue, culture);
                    }
                }

                return DescriptionValue;
            }
        }
        #endregion
    }
}
