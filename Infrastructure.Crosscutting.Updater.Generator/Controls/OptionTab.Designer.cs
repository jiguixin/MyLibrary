namespace Infrastructure.Crosscutting.Updater.Generator.Controls
{
	partial class OptionTab
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
			FSLib.Windows.Components.CueInfo cueInfo1 = new FSLib.Windows.Components.CueInfo(true);
			FSLib.Windows.Components.CueInfo cueInfo2 = new FSLib.Windows.Components.CueInfo(true);
			this.label1 = new System.Windows.Forms.Label();
			this.deletePreviousFileMode = new System.Windows.Forms.ComboBox();
			this.deleteRules = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.gpSetDeleteSyntax = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.requiredMinVersion = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtPackagePassword = new System.Windows.Forms.TextBox();
			this.cueProvider1 = new FSLib.Windows.Components.CueProvider();
			this.gpSetDeleteSyntax.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(113, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "更新时删除原始文件";
			// 
			// deletePreviousFileMode
			// 
			this.deletePreviousFileMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.deletePreviousFileMode.FormattingEnabled = true;
			this.deletePreviousFileMode.Items.AddRange(new object[] {
            "仅覆盖, 不主动删除",
            "清空原程序目录",
            "仅删除指定文件和目录"});
			this.deletePreviousFileMode.Location = new System.Drawing.Point(122, 6);
			this.deletePreviousFileMode.Name = "deletePreviousFileMode";
			this.deletePreviousFileMode.Size = new System.Drawing.Size(246, 20);
			this.deletePreviousFileMode.TabIndex = 1;
			// 
			// deleteRules
			// 
			this.deleteRules.Location = new System.Drawing.Point(5, 20);
			this.deleteRules.Multiline = true;
			this.deleteRules.Name = "deleteRules";
			this.deleteRules.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.deleteRules.Size = new System.Drawing.Size(521, 74);
			this.deleteRules.TabIndex = 2;
			this.deleteRules.WordWrap = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 98);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(512, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "使用相对路径判断，不包括程序所在目录；使用正则表达式语法，一行一条记录。";
			// 
			// gpSetDeleteSyntax
			// 
			this.gpSetDeleteSyntax.Controls.Add(this.deleteRules);
			this.gpSetDeleteSyntax.Controls.Add(this.label2);
			this.gpSetDeleteSyntax.Location = new System.Drawing.Point(5, 31);
			this.gpSetDeleteSyntax.Name = "gpSetDeleteSyntax";
			this.gpSetDeleteSyntax.Size = new System.Drawing.Size(532, 120);
			this.gpSetDeleteSyntax.TabIndex = 4;
			this.gpSetDeleteSyntax.TabStop = false;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label8.Location = new System.Drawing.Point(5, 160);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(113, 12);
			this.label8.TabIndex = 5;
			this.label8.Text = "支持更新的最低版本";
			// 
			// requiredMinVersion
			// 
			cueInfo1.Text = "低于此版本的软件将会要求用户进行手动更新";
			this.cueProvider1.SetCue(this.requiredMinVersion, cueInfo1);
			this.requiredMinVersion.Location = new System.Drawing.Point(122, 157);
			this.requiredMinVersion.Name = "requiredMinVersion";
			this.requiredMinVersion.Size = new System.Drawing.Size(328, 21);
			this.requiredMinVersion.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(5, 187);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(89, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "升级文件包密码";
			// 
			// txtPackagePassword
			// 
			cueInfo2.Text = "用于加密生成的压缩文件包";
			this.cueProvider1.SetCue(this.txtPackagePassword, cueInfo2);
			this.txtPackagePassword.Location = new System.Drawing.Point(122, 184);
			this.txtPackagePassword.Name = "txtPackagePassword";
			this.txtPackagePassword.PasswordChar = '*';
			this.txtPackagePassword.Size = new System.Drawing.Size(328, 21);
			this.txtPackagePassword.TabIndex = 8;
			// 
			// OptionTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtPackagePassword);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.requiredMinVersion);
			this.Controls.Add(this.gpSetDeleteSyntax);
			this.Controls.Add(this.deletePreviousFileMode);
			this.Controls.Add(this.label1);
			this.Name = "OptionTab";
			this.Size = new System.Drawing.Size(540, 280);
			this.gpSetDeleteSyntax.ResumeLayout(false);
			this.gpSetDeleteSyntax.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox deletePreviousFileMode;
		private System.Windows.Forms.TextBox deleteRules;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox gpSetDeleteSyntax;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox requiredMinVersion;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPackagePassword;
		private FSLib.Windows.Components.CueProvider cueProvider1;
	}
}
