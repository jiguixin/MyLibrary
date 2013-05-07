namespace Infrastructure.Crosscutting.Updater.Dialogs
{
	partial class MinmumVersionRequired
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
			this.label1 = new System.Windows.Forms.Label();
			this.lblDesc = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(281, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "很抱歉，您的软件因为版本过低而无法自动升级";
			// 
			// lblDesc
			// 
			this.lblDesc.AutoSize = true;
			this.lblDesc.Location = new System.Drawing.Point(13, 44);
			this.lblDesc.Name = "lblDesc";
			this.lblDesc.Size = new System.Drawing.Size(197, 12);
			this.lblDesc.TabIndex = 1;
			this.lblDesc.Text = "请前往软件的主页手动下载新版本。";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(238, 74);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(69, 27);
			this.btnGo.TabIndex = 0;
			this.btnGo.Text = "转到主页";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(312, 74);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(69, 27);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "关闭";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// MinmumVersionRequired
			// 
			this.AcceptButton = this.btnGo;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(393, 113);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.lblDesc);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MinmumVersionRequired";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "当前版本过低无法升级";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.MinmumVersionRequired_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button btnClose;
	}
}