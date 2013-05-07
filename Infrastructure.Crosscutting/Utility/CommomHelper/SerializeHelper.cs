using System; 
using System.IO; 
using System.Runtime.Serialization.Formatters.Binary;

namespace Infrastructure.Crosscutting.Utility.CommomHelper
{
    public class SerializeHelper
    {
        ///*****************************************
        /// <summary>
        /// 序列化一个对象
        /// </summary>
        /// <param name="o">将要序列化的对象</param>
        /// <returns>返回byte[]</returns>
        ///*****************************************
        public static byte[] Serialize(object o)
        {
            if (o == null) return null;
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, o);
            ms.Position = 0;
            byte[] b = new byte[ms.Length];
            ms.Read(b, 0, b.Length);
            ms.Close();
            return b;
        }


        ///*****************************************
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="b">返回一个对象</param>
        ///*****************************************
        public static object Deserialize(byte[] b)
        {
            try
            {
                if (b.Length == 0) return null;

                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                ms.Write(b, 0, b.Length);
                ms.Position = 0;
                object n = (object)bf.Deserialize(ms);
                ms.Close();
                return n;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
