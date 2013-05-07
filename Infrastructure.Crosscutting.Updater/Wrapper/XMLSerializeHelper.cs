using System.Diagnostics;
using System.Data;
using System.Collections;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;

namespace Infrastructure.Crosscutting.Updater.Wrapper
{
	/// <summary>
	/// XML序列化支持类
	/// </summary>
	static class XMLSerializeHelper
	{
		/// <summary>
		/// 从流中反序列化出指定对象类型的对象
		/// </summary>
		/// <param name="objType">对象类型</param>
		/// <param name="stream">流对象</param>
		/// <returns>反序列结果</returns>
		public static object XmlDeserializeFromStream(Stream stream, System.Type objType)
		{
			try
			{
				XmlSerializer xso = new XmlSerializer(objType);
				object res = xso.Deserialize(stream);
				return res;
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// 从流中反序列化对象
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="stream">流对象</param>
		/// <returns>反序列化结果</returns>
		public static T XmlDeserializeFromStream<T>(Stream stream) where T : class
		{
			T res = XmlDeserializeFromStream(stream, typeof(T)) as T;
			return res;
		}

		/// <summary>
		/// 序列化对象为文本
		/// </summary>
		/// <param name="objectToSerialize">要序列化的对象</param>
		/// <returns>保存信息的 <see cref="T:System.String"/></returns>
		public static T XmlDeserializeFromString<T>(string content) where T : class
		{
			if (String.IsNullOrEmpty(content))
				return null;

			using (var ms = new MemoryStream())
			{
				byte[] buffer = System.Text.Encoding.Unicode.GetBytes(content);
				ms.Write(buffer, 0, buffer.Length);
				ms.Seek(0, SeekOrigin.Begin);

				return XmlDeserializeFromStream<T>(ms);
			}
		}

	}
}
