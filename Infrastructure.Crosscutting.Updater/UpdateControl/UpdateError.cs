using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Infrastructure.Crosscutting.Updater.UpdateControl
{
	public partial class UpdateError : Infrastructure.Crosscutting.Updater.UpdateControl.ControlBase
	{
		public UpdateError()
		{
			InitializeComponent();

			if (Program.IsRunning)
			{
				Updater.Instance.Error += Instance_Error;
			}
		}

		void Instance_Error(object sender, EventArgs e)
		{
			HideControls();
			this.Visible = true;
			lblDesc.Text = Updater.Instance.Exception.Message;

#if DEBUG
			System.Windows.Forms.MessageBox.Show(Updater.Instance.Exception.ToString());
#endif
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.FindForm().Close();
		}
	}
}
