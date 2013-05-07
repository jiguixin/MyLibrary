using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Crosscutting.Updater
{
	/// <summary>
	/// 用于向自动更新库提供当前应用程序的信息
	/// </summary>
	public interface IAppInfoProvider
	{
		/// <summary>
		/// 获得当前应用软件的版本
		/// </summary>
		Version CurrentVersion { get; }

		/// <summary>
		/// 获得当前应用程序的根目录
		/// </summary>
		string CurrentApplicationDirectory { get; }

		/// <summary>
		/// 获得更新包的路径
		/// </summary>
		/// <param name="xmlUrl">更新的XML文件路径</param>
		/// <param name="updateInfo">更新信息</param>
		/// <returns>返回的 <see cref="T:System.String"/> 表示目标更新包的路径</returns>
		string GetUpdatePackageUrl(string xmlUrl, UpdateInfo updateInfo);

		/// <summary>
		/// 获得应用程序更新的信息地址
		/// </summary>
		string UpdateUrl { get; }
	}
}
