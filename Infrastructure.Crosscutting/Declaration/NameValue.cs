using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Crosscutting.Declaration
{
    /// <summary>
    /// 名称-值类
    /// </summary>
    [Serializable]
    public class NameValue
    {

        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Value;
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        /// <summary>
        /// 构造一个NameValue的对象,根据指定的参数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public NameValue(string name, string value)
        {
            _Name = name;
            _Value = value;
        }


        /// <summary>
        /// 将对象序列化,这里返回其名称Name
        /// </summary>
        /// <returns>返回Name</returns>
        public override string ToString()
        {
            return _Name;
        }


        /// <summary>
        /// 获取对象本身
        /// </summary>
        public NameValue Self
        {
            get { return this; }
        }

        /// <summary>
        /// 判断是否相等,只要Value相等,就是为两个对象相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //if (obj.GetType() != typeof(NameValue)) return false;
            try
            {
                if (((NameValue)obj).Value == this.Value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
