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
	/// XML���л�֧����
	/// </summary>
	static class XMLSerializeHelper
	{
		/// <summary>
		/// �����з����л���ָ���������͵Ķ���
		/// </summary>
		/// <param name="objType">��������</param>
		/// <param name="stream">������</param>
		/// <returns>�����н��</returns>
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
		/// �����з����л�����
		/// </summary>
		/// <typeparam name="T">��������</typeparam>
		/// <param name="stream">������</param>
		/// <returns>�����л����</returns>
		public static T XmlDeserializeFromStream<T>(Stream stream) where T : class
		{
			T res = XmlDeserializeFromStream(stream, typeof(T)) as T;
			return res;
		}

		/// <summary>
		/// ���л�����Ϊ�ı�
		/// </summary>
		/// <param name="objectToSerialize">Ҫ���л��Ķ���</param>
		/// <returns>������Ϣ�� <see cref="T:System.String"/></returns>
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
