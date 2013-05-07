using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Crosscutting.Declaration
{
    [Serializable]
    /// <summary>
    /// Name-ID类, 封装有Name和Id构造成的一个对象
    /// </summary>    
    public class NameId
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

        private int _Id;
        /// <summary>
        /// Id - 对应数据库中的主键
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        /// <summary>
        /// 返回对象本身
        /// </summary>
        public NameId Self
        {
            get { return this; }
        }



        public NameId()
        {

        }

        /// <summary>
        /// 构造NameId类
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="id">Id号</param>
        public NameId(string name, int id)
        {
            _Name = name;
            _Id = id;
            if (id <= 0)
            {
                _Name = "";
            }
        }


        /// <summary>
        /// 将对象序列化为字符串,返回的格式为 Id:Name
        /// </summary>
        /// <returns>返回一个包含有Id和Name的字符串</returns>
        public override string ToString()
        {
            if (_Id > 0)
            {
                return _Id.ToString() + ":" + _Name;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 从格式为 Id:Name的字符串构造对象
        /// </summary>
        /// <param name="s">要分解的字符串</param>
        /// <returns>返回一个NameId的对象,该对象又给定的字符串分解构造而成</returns>
        public static NameId From(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;

            string[] sp = s.Split(new char[] { ':' });
            NameId ni = new NameId();

            try
            {
                ni.Id = sp[0].ToInt32();

                //循环数组，分别把后面的内容加到Name里面，防止Name中有冒号而造成的数组损坏
                for (int i = 1; i < sp.Length; i++)
                {
                    ni.Name += sp[i];
                }

                //保证ni.Name不为空
                if (ni.Name == null) ni.Name = "";
            }
            catch
            {
                ni.Id = 0;
                ni.Name = "-";
            }
            return ni;
        }


        /// <summary>
        /// 从object构造对象,自动将object转化为格式为 Id:Name的string
        /// </summary>
        /// <param name="obj">要分解的对象</param>
        /// <returns>返回一个NameId的对象,该对象又给定的对象转化为的字符串分解构造而成</returns>
        public static NameId From(object obj)
        {
            string s = obj.ToString();
            string[] sp = s.Split(new char[] { ':' });
            NameId ni = new NameId();

            try
            {
                ni.Id = sp[0].ToInt32();
                //循环数组，分别把后面的内容加到Name里面，防止Name中有冒号而造成的数组损坏
                for (int i = 1; i < sp.Length; i++)
                {
                    ni.Name += sp[i];
                }

                //保证ni.Name不为空
                if (ni.Name == null) ni.Name = "";
            }
            catch
            {
                ni.Id = 0;
                ni.Name = "-";
            }
            return ni;
        }


        /// <summary>
        /// 判断两个对象是否相等。这里只要属性 Id相同。则视为两个对象相等
        /// </summary>
        /// <param name="obj">要进行比较的对象</param>
        /// <returns>若两个NameId对象的Id相等,返回true,否则返回false</returns>
        public override bool Equals(object obj)
        {
            try
            {
                if (((NameId)obj).Id == this.Id)
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


        /// <summary>
        /// 判定对象是否为空值
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return this._Id == 0;
        }


        /// <summary>
        /// 获取空值的NameId
        /// </summary>
        public static NameId Null
        {
            get { return new NameId("", 0); }
        }
    }
}
