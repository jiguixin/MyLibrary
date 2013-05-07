using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infrastructure.Crosscutting.Updater.Wrapper;

namespace Infrastructure.Crosscutting.Updater
{
	public partial class UpdateFound : Form
	{
		public UpdateFound()
		{
			InitializeComponent();
			this.TopMost = true;

			this.lnkSoft.Click += (s, e) => System.Diagnostics.Process.Start(Updater.Instance.UpdateInfo.PublishUrl);
		}

		private void UpdateFound_Load(object sender, EventArgs e)
		{
			if (Updater.Instance == null) return;

			lblFound.Text = string.Format(Infrastructure.Crosscutting.Updater.SR.UpdateFound_Desc, Updater.Instance.UpdateInfo.AppName, Updater.Instance.UpdateInfo.AppVersion);
			txtReadMe.Text = Updater.Instance.UpdateInfo.Desc;
			lblSize.Text += Updater.Instance.UpdateInfo.PackageSize == 0 ? "<未知>" : ExtensionMethod.ToSizeDescription(Updater.Instance.UpdateInfo.PackageSize);

			this.lnkSoft.Visible = !string.IsNullOrEmpty(Updater.Instance.UpdateInfo.PublishUrl);
		}
	}
}
