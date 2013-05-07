using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Infrastructure.Crosscutting.Updater.UpdateControl
{
	public class ControlBase : UserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.Size = new Size(200, 98);
			if (Program.IsRunning)
			{
				this.Location = new Point(0, 22);
				this.BackColor = Color.Transparent;
				this.ForeColor = Color.White;
				this.Visible = false;

				foreach (Control ctl in this.Controls)
				{
					if (ctl is Button) (ctl as Button).ForeColor = SystemColors.ControlText;
				}
			}
		}

		/// <summary>
		/// 请求主窗体隐藏所有控件
		/// </summary>
		protected void HideControls()
		{
			MainWindow mw = this.FindForm() as MainWindow;
			mw.HideAllControls();
		}
	}
}
