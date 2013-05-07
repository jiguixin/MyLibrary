using Microsoft.Win32;

namespace Infrastructure.Crosscutting.Utility.CommomHelper
{ 
    /// <summary>
    /// 注册表访问类
    /// </summary>
    public class RegHelper
    {
        /// <summary>
        /// 创建与设置注册表节点
        /// </summary>
        /// <param name="keyName">KEY</param>
        /// <param name="valueName">VALUE</param>
        /// <param name="value">注册表类型</param>
        public static void SetValue(string keyName,string valueName,object value)
        {
            Registry.SetValue(keyName, valueName, value,RegistryValueKind.String);
        }

        /// <summary>
        /// 得到注册表中的值
        /// </summary>
        /// <param name="keyName">KEY</param>
        /// <param name="valueName">VALUE</param>
        /// <param name="defaultValue">没有找到节点，返回默认值</param>
        /// <returns></returns>
        public static object GetValue(string keyName,string valueName,object defaultValue)
        {
            object obj1 = Registry.GetValue(keyName, valueName, defaultValue);
            if (obj1 == null)
            {
                return defaultValue;
            }
            else
            {
                return obj1;
            }
        }
    } 

}
