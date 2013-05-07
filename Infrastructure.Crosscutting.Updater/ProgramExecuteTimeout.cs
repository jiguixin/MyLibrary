﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Infrastructure.Crosscutting.Updater
{
	public partial class ProgramExecuteTimeout : Form
	{
		public ProgramExecuteTimeout()
		{
			InitializeComponent();
		}

		private void btnWait_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnKill_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
