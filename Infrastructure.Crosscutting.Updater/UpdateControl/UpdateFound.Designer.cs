namespace Infrastructure.Crosscutting.Updater.UpdateControl
{
	partial class UpdateFound
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.lblDesc = new System.Windows.Forms.Label();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.lnkUpdateDesc = new System.Windows.Forms.LinkLabel();
			this.lnkOpenSite = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// lblDesc
			// 
			this.lblDesc.Location = new System.Drawing.Point(3, 4);
			this.lblDesc.Name = "lblDesc";
			this.lblDesc.Size = new System.Drawing.Size(194, 50);
			this.lblDesc.TabIndex = 0;
			this.lblDesc.Text = "已发现新版本 10.10.10.2323，你想要立刻更新吗？";
			// 
			// btnUpdate
			// 
			this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnUpdate.ForeColor = System.Drawing.SystemColors.WindowText;
			this.btnUpdate.Location = new System.Drawing.Point(29, 74);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(68, 24);
			this.btnUpdate.TabIndex = 1;
			this.btnUpdate.Text = "是(&Y)";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button2.ForeColor = System.Drawing.SystemColors.WindowText;
			this.button2.Location = new System.Drawing.Point(103, 74);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(68, 24);
			this.button2.TabIndex = 1;
			this.button2.Text = "否(&N)";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// lnkUpdateDesc
			// 
			this.lnkUpdateDesc.AutoSize = true;
			this.lnkUpdateDesc.BackColor = System.Drawing.Color.Transparent;
			this.lnkUpdateDesc.Location = new System.Drawing.Point(7, 59);
			this.lnkUpdateDesc.Name = "lnkUpdateDesc";
			this.lnkUpdateDesc.Size = new System.Drawing.Size(53, 12);
			this.lnkUpdateDesc.TabIndex = 2;
			this.lnkUpdateDesc.TabStop = true;
			this.lnkUpdateDesc.Text = "更新内容";
			this.lnkUpdateDesc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUpdateDesc_LinkClicked);
			// 
			// lnkOpenSite
			// 
			this.lnkOpenSite.AutoSize = true;
			this.lnkOpenSite.BackColor = System.Drawing.Color.Transparent;
			this.lnkOpenSite.Location = new System.Drawing.Point(78, 59);
			this.lnkOpenSite.Name = "lnkOpenSite";
			this.lnkOpenSite.Size = new System.Drawing.Size(77, 12);
			this.lnkOpenSite.TabIndex = 2;
			this.lnkOpenSite.TabStop = true;
			this.lnkOpenSite.Text = "打开发布主页";
			this.lnkOpenSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenSite_LinkClicked);
			// 
			// UpdateFound
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lnkOpenSite);
			this.Controls.Add(this.lnkUpdateDesc);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.lblDesc);
			this.Name = "UpdateFound";
			this.Size = new System.Drawing.Size(200, 98);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.LinkLabel lnkUpdateDesc;
		private System.Windows.Forms.LinkLabel lnkOpenSite;
	}
}
