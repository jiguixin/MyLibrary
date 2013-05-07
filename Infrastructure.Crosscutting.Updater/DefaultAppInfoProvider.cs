using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Crosscutting.Updater
{
	/// <summary>
	/// 默认的应用程序提供类
	/// </summary>
	public class DefaultAppInfoProvider : IAppInfoProvider
	{
		#region IAppInfoProvider 成员

		/// <summary>
		/// 获得当前应用软件的版本
		/// </summary>
		public virtual Version CurrentVersion
		{
			get { return new Version(System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).ProductVersion); }
		}

		/// <summary>
		/// 获得当前应用程序的根目录
		/// </summary>
		public virtual string CurrentApplicationDirectory
		{
			get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location); }
		}

		/// <summary>
		/// 获得更新包的路径
		/// </summary>
		/// <param name="xmlUrl">更新的XML文件路径</param>
		/// <param name="updateInfo">更新信息</param>
		/// <returns>返回的 <see cref="T:System.String"></see> 表示目标更新包的路径</returns>
		public virtual string GetUpdatePackageUrl(string xmlUrl, UpdateInfo updateInfo)
		{
			return xmlUrl.Substring(0, xmlUrl.LastIndexOf("/") + 1) + updateInfo.Package;
		}
		/// <summary>
		/// 获得应用程序更新的信息地址
		/// </summary>
		public virtual string UpdateUrl { get; protected set; }

		#endregion

		/// <summary>
		/// 创建 <see cref="DefaultAppInfoProvider" /> 的新实例
		/// </summary>
		public DefaultAppInfoProvider(string updateUrl)
		{
			UpdateUrl = updateUrl;
		}

		/// <summary>
		/// 创建 <see cref="DefaultAppInfoProvider" /> 的新实例
		/// </summary>
		public DefaultAppInfoProvider()
		{

		}
	}
}
