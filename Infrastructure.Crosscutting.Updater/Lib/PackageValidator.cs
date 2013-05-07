using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Crosscutting.Updater.Lib
{
	public class PackageValidator
	{
		public string MD5 { get; set; }

		public string FilePath { get; set; }

		/// <summary>
		/// 创建 <see cref="PackageValidator" /> 的新实例
		/// </summary>
		public PackageValidator(string mD5, string filePath)
		{
			MD5 = mD5;
			FilePath = filePath;
		}

		/// <summary>
		/// 验证下载包的MD5是否正确
		/// </summary>
		/// <returns></returns>
		public bool Validate(Action<int,int> progressReportor)
		{
			using (Infrastructure.Crosscutting.Updater.Wrapper.ExtendFileStream fs = new Infrastructure.Crosscutting.Updater.Wrapper.ExtendFileStream(FilePath, System.IO.FileMode.Open))
			{
				if (progressReportor != null)
				{
					fs.ProgressChanged += (x, y) => progressReportor((int)fs.Length, (int)fs.Position);
				}

				System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
				byte[] hash = md5.ComputeHash(fs);
				string hashCode = BitConverter.ToString(hash).Replace("-", "");

				if (string.Compare(hashCode, MD5, true) != 0) return false;
			}
			return true;
		}
	}
}
