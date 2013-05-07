using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Infrastructure.Crosscutting.Updater
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (Program.IsRunning)
			{
				HideAllControls();
				Updater upd = Updater.Instance;
				upd.QueryCloseApplication += (_s, _e) => _e.CallDefaultBeihavior();

				if (upd.IsUpdateInfoDownloaded) { upd.BeginUpdate(); }
				else upd.BeginCheckUpdateInProcess();
			}
		}

		/// <summary>
		/// 隐藏所有控件
		/// </summary>
		internal void HideAllControls()
		{
			foreach (Control item in this.Controls)
			{
				if (item is UpdateControl.ControlBase) item.Visible = false;
			}
		}
	}
}
