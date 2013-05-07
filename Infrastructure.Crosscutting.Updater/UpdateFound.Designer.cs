namespace Infrastructure.Crosscutting.Updater
{
	partial class UpdateFound
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateFound));
			this.lblFound = new System.Windows.Forms.Label();
			this.txtReadMe = new System.Windows.Forms.TextBox();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lnkSoft = new System.Windows.Forms.LinkLabel();
			this.lblSize = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblFound
			// 
			resources.ApplyResources(this.lblFound, "lblFound");
			this.lblFound.Name = "lblFound";
			// 
			// txtReadMe
			// 
			resources.ApplyResources(this.txtReadMe, "txtReadMe");
			this.txtReadMe.Name = "txtReadMe";
			this.txtReadMe.ReadOnly = true;
			// 
			// btnUpdate
			// 
			this.btnUpdate.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btnUpdate, "btnUpdate");
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lnkSoft
			// 
			resources.ApplyResources(this.lnkSoft, "lnkSoft");
			this.lnkSoft.Name = "lnkSoft";
			this.lnkSoft.TabStop = true;
			// 
			// lblSize
			// 
			resources.ApplyResources(this.lblSize, "lblSize");
			this.lblSize.Name = "lblSize";
			// 
			// UpdateFound
			// 
			this.AcceptButton = this.btnUpdate;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.lblSize);
			this.Controls.Add(this.lnkSoft);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.txtReadMe);
			this.Controls.Add(this.lblFound);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "UpdateFound";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.UpdateFound_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFound;
		private System.Windows.Forms.TextBox txtReadMe;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.LinkLabel lnkSoft;
		private System.Windows.Forms.Label lblSize;
	}
}