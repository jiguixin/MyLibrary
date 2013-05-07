using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infrastructure.Crosscutting.Updater.Wrapper;

namespace Infrastructure.Crosscutting.Updater.UpdateControl
{
	public partial class RunUpdate : Infrastructure.Crosscutting.Updater.UpdateControl.ControlBase
	{
		public RunUpdate()
		{
			InitializeComponent();

			if (Program.IsRunning)
			{

				Updater.Instance.DownloadPackage += Instance_DownloadPackage;
				Updater.Instance.DownloadPackageFinished += Instance_DownloadPackageFinished;
				Updater.Instance.DownloadProgressChanged += Instance_DownloadProgressChanged;
				Updater.Instance.InstallUpdates += Instance_InstallUpdates;
				Updater.Instance.QueryCloseApplication += Instance_QueryCloseApplication;
				Updater.Instance.VerifyPackage += Instance_VerifyPackage;
				Updater.Instance.VerifyPackageFinished += Instance_VerifyPackageFinished;
			}
		}

		void Instance_VerifyPackageFinished(object sender, EventArgs e)
		{
			pbProgress.Style = ProgressBarStyle.Marquee;
			lblDesc.Text = "已完成验证升级包";
			lblProgressDesc.Text = string.Empty;
		}

		void Instance_VerifyPackage(object sender, EventArgs e)
		{
			pbProgress.Style = ProgressBarStyle.Marquee;
			lblDesc.Text = "正在验证升级包...";
			lblProgressDesc.Text = string.Empty;
		}

		void Instance_QueryCloseApplication(object sender, EventArgs e)
		{
			pbProgress.Style = ProgressBarStyle.Marquee;
			lblDesc.Text = "正在请求关闭应用程序...";
			lblProgressDesc.Text = string.Empty;
		}

		void Instance_InstallUpdates(object sender, EventArgs e)
		{
			pbProgress.Style = ProgressBarStyle.Marquee;
			lblDesc.Text = "正在安装升级包...";
			lblProgressDesc.Text = string.Empty;
		}



		void Instance_DownloadProgressChanged(object sender, Infrastructure.Crosscutting.Updater.Wrapper.RunworkEventArgs e)
		{
			pbProgress.Style = ProgressBarStyle.Blocks;
			pbProgress.Value = e.Progress.TaskPercentage;

			if (!string.IsNullOrEmpty(e.Progress.StateMessage)) lblProgressDesc.Text = e.Progress.StateMessage;
			else lblProgressDesc.Text = string.Format("{0}/{1}", ExtensionMethod.ToSizeDescription(e.Progress.TaskProgress), ExtensionMethod.ToSizeDescription(e.Progress.TaskCount));
		}

		void Instance_DownloadPackageFinished(object sender, EventArgs e)
		{
			pbProgress.Style = ProgressBarStyle.Marquee;
			lblDesc.Text = "已完成下载升级包";
			lblProgressDesc.Text = string.Empty;
		}

		void Instance_DownloadPackage(object sender, EventArgs e)
		{
			this.Show();
			pbProgress.Style = ProgressBarStyle.Marquee;
			lblDesc.Text = "正在下载升级包...";
			lblProgressDesc.Text = string.Empty;
		}
	}
}
