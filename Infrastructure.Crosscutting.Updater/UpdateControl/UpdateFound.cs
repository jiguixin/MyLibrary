using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Infrastructure.Crosscutting.Updater.UpdateControl
{
	public partial class UpdateFound : ControlBase
	{
		public UpdateFound()
		{
			InitializeComponent();

			if (Program.IsRunning)
			{
				Updater.Instance.UpdatesFound += Instance_UpdatesFound;
			}
		}

		//找到更新
		void Instance_UpdatesFound(object sender, EventArgs e)
		{
			HideControls();

			lnkOpenSite.Visible = !string.IsNullOrEmpty(Updater.Instance.UpdateInfo.PublishUrl);
			lnkUpdateDesc.Visible = !string.IsNullOrEmpty(Updater.Instance.UpdateInfo.Desc);

			lblDesc.Text = string.Format("已经发现『{1}』的新版本：{0}，您希望自动更新到最新版本吗？", Updater.Instance.UpdateInfo.AppVersion, Updater.Instance.UpdateInfo.AppName);

			this.Visible = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Updater.Instance.BeginUpdate();
		}

		private void lnkUpdateDesc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Infrastructure.Crosscutting.Updater.Wrapper.FunctionalForm.Information(Updater.Instance.UpdateInfo.Desc);
		}

		private void lnkOpenSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(Updater.Instance.UpdateInfo.PublishUrl);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
