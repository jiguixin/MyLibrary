using System;
using System.Collections.Generic;
using System.Threading;

namespace Infrastructure.Crosscutting.Updater.Lib
{
	public class PackageDownloader : Base
	{
		/// <summary>
		/// 下载的路径
		/// </summary>
		public string DownloadUrl { get; set; }

		/// <summary>
		/// 本地保存地址.如果留空,则下载为字符串并保存在此变量中.
		/// </summary>
		public string LocalPath { get; set; }

		/// <summary>
		/// 创建 <see cref="PackageDownloader" /> 的新实例
		/// </summary>
		public PackageDownloader(string downloadUrl, string localPath)
		{
			DownloadUrl = downloadUrl;
			LocalPath = localPath;
		}

		/// <summary>
		/// 执行文件或字符串下载
		/// </summary>
		/// <returns></returns>
		public bool Download()
		{
			if (System.IO.File.Exists(LocalPath)) System.IO.File.Delete(LocalPath);

			bool finished = false;
			bool error = false;

			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(LocalPath));

			try
			{
				var client = new System.Net.WebClient();
				client.DownloadProgressChanged += (s, e) =>
				{
					ReportProgress((int)e.TotalBytesToReceive, (int)e.BytesReceived);
				};

				if (!string.IsNullOrEmpty(LocalPath))
				{
					client.DownloadFileCompleted += (s, e) =>
					{
						if (e.Error != null)
						{
							this.Exception = e.Error;
							error = true;
						}
						finished = true;
					};
					client.DownloadFileAsync(new System.Uri(DownloadUrl), LocalPath);
				}
				else
				{
					client.DownloadStringCompleted += (s, e) =>
					{
						if (e.Error != null)
						{
							this.Exception = e.Error;
							error = true;
						}
						finished = true;
						this.LocalPath = e.Result;
					};
					client.DownloadStringAsync(new System.Uri(DownloadUrl));
				}

				//没下载完之前持续等待
				while (!finished)
				{
					Thread.Sleep(50);
				}
			}
			catch (Exception ex)
			{
				this.Exception = ex;
				return false;
			}

			if (error) return false;

			return true;
		}
	}
}
