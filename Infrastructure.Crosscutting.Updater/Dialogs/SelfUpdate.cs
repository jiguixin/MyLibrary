using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Infrastructure.Crosscutting.Updater.Dialogs
{
	public partial class SelfUpdate : Form
	{
		public SelfUpdate()
		{
			InitializeComponent();
		}

		private void SelfUpdate_Load(object sender, EventArgs e)
		{
			var updater = Updater.Instance;
			updater.NoUpdatesFound += new EventHandler(updater_NoUpdatesFound);
			updater.Error += new EventHandler(updater_Error);
			updater.ApplicationDirectory = Environment.CurrentDirectory;

#if DEBUG
			Updater.CheckUpdateSimple("http://localhost:2099/update.xml");
#else
			Updater.CheckUpdateSimple("http://www.u-tide.com/fish/Service.asmx/update/33/update.xml");
#endif
		}

		void updater_Error(object sender, EventArgs e)
		{
			if (MessageBox.Show("检查更新失败：" + Updater.Instance.Exception.Message + "，您希望前往主页查看更新吗？", "更新失败", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.Yes)
			{
				System.Diagnostics.Process.Start("http://www.u-tide.com/fish/soft/simple_autoupdater/index.html");
			}
			Close();
		}

		void updater_NoUpdatesFound(object sender, EventArgs e)
		{
			MessageBox.Show("您所使用的类库是最新的，没有更新的版本。", "没有更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
			Close();
		}
	}
}
