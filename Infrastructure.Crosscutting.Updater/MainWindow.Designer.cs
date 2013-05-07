namespace Infrastructure.Crosscutting.Updater
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.slideComponent1 = new Infrastructure.Crosscutting.Updater.Wrapper.SlideComponent();
			this.runUpdate1 = new Infrastructure.Crosscutting.Updater.UpdateControl.RunUpdate();
			this.updateFinished1 = new Infrastructure.Crosscutting.Updater.UpdateControl.UpdateFinished();
			this.updateError1 = new Infrastructure.Crosscutting.Updater.UpdateControl.UpdateError();
			this.noUpdateFound1 = new Infrastructure.Crosscutting.Updater.UpdateControl.NoUpdateFound();
			this.updateFound1 = new Infrastructure.Crosscutting.Updater.UpdateControl.UpdateFound();
			this.downloadingInfo1 = new Infrastructure.Crosscutting.Updater.UpdateControl.DownloadingInfo();
			this.SuspendLayout();
			// 
			// slideComponent1
			// 
			this.slideComponent1.AlwaysSetLocation = false;
			this.slideComponent1.AttachedForm = this;
			this.slideComponent1.DirectX = Infrastructure.Crosscutting.Updater.Wrapper.SlideComponent.FlyXDirection.None;
			this.slideComponent1.MoveSpeedX = 0;
			this.slideComponent1.MoveSpeedY = 8;
			// 
			// runUpdate1
			// 
			resources.ApplyResources(this.runUpdate1, "runUpdate1");
			this.runUpdate1.Name = "runUpdate1";
			// 
			// updateFinished1
			// 
			resources.ApplyResources(this.updateFinished1, "updateFinished1");
			this.updateFinished1.Name = "updateFinished1";
			// 
			// updateError1
			// 
			resources.ApplyResources(this.updateError1, "updateError1");
			this.updateError1.Name = "updateError1";
			// 
			// noUpdateFound1
			// 
			resources.ApplyResources(this.noUpdateFound1, "noUpdateFound1");
			this.noUpdateFound1.Name = "noUpdateFound1";
			// 
			// updateFound1
			// 
			this.updateFound1.BackColor = System.Drawing.Color.Transparent;
			this.updateFound1.ForeColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.updateFound1, "updateFound1");
			this.updateFound1.Name = "updateFound1";
			// 
			// downloadingInfo1
			// 
			resources.ApplyResources(this.downloadingInfo1, "downloadingInfo1");
			this.downloadingInfo1.Name = "downloadingInfo1";
			// 
			// MainWindow
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.downloadingInfo1);
			this.Controls.Add(this.runUpdate1);
			this.Controls.Add(this.updateFinished1);
			this.Controls.Add(this.updateError1);
			this.Controls.Add(this.noUpdateFound1);
			this.Controls.Add(this.updateFound1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "MainWindow";
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private Infrastructure.Crosscutting.Updater.UpdateControl.UpdateFound updateFound1;
		private Infrastructure.Crosscutting.Updater.Wrapper.SlideComponent slideComponent1;
		private Infrastructure.Crosscutting.Updater.UpdateControl.UpdateFinished updateFinished1;
		private Infrastructure.Crosscutting.Updater.UpdateControl.UpdateError updateError1;
		private Infrastructure.Crosscutting.Updater.UpdateControl.NoUpdateFound noUpdateFound1;
		private Infrastructure.Crosscutting.Updater.UpdateControl.RunUpdate runUpdate1;
		private Infrastructure.Crosscutting.Updater.UpdateControl.DownloadingInfo downloadingInfo1;
	}
}