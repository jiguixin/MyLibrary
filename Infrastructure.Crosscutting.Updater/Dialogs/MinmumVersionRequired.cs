using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Infrastructure.Crosscutting.Updater.Dialogs
{
	public partial class MinmumVersionRequired : Form
	{
		public MinmumVersionRequired()
		{
			InitializeComponent();
		}

		private void MinmumVersionRequired_Load(object sender, EventArgs e)
		{
			var updater = Updater.Instance;
			var info = updater.UpdateInfo;

			lblDesc.Text += "\r\n\r\n" + string.Format("升级要求最低版本：{0}\r\n您当前已安装版本：{1}", info.RequiredMinVersion, updater.CurrentVersion);
		}

		private void btnGo_Click(object sender, EventArgs e)
		{
			var updater = Updater.Instance;

			try
			{
				System.Diagnostics.Process.Start(updater.UpdateInfo.PublishUrl);
			}
			catch (Exception)
			{

			}
			Close();
		}
	}
}
