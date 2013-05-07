using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSLib;

namespace Infrastructure.Crosscutting.Updater.Generator.Controls
{
	public partial class OptionTab : UserControl
	{
		/// <summary>
		/// 根据配置更新界面
		/// </summary>
		/// <param name="info"></param>
		public void UpdateInterface(Infrastructure.Crosscutting.Updater.UpdateInfo info)
		{
			this.deletePreviousFileMode.SelectedIndex = (int)info.DeleteMethod;
			this.deleteRules.Text = info.DeleteFileLimits.IsEmpty() ? "" : string.Join(Environment.NewLine, info.DeleteFileLimits);
			this.requiredMinVersion.Text = info.RequiredMinVersion;
			this.txtPackagePassword.Text = info.PackagePassword ?? "";
		}

		/// <summary>
		/// 保存设置到配置中
		/// </summary>
		/// <param name="info"></param>
		public void SaveSetting(Infrastructure.Crosscutting.Updater.UpdateInfo info)
		{
			info.DeleteFileLimits = this.deleteRules.Lines;
			info.DeleteMethod = (Infrastructure.Crosscutting.Updater.DeletePreviousProgramMethod)this.deletePreviousFileMode.SelectedIndex;
			info.RequiredMinVersion = this.requiredMinVersion.Text;
			info.PackagePassword = this.txtPackagePassword.Text;
		}

		public OptionTab()
		{
			InitializeComponent();

			this.deletePreviousFileMode.SelectedIndexChanged += (_, __) =>
			{
				this.gpSetDeleteSyntax.Visible = this.deletePreviousFileMode.SelectedIndex > 0;
				this.gpSetDeleteSyntax.Text = this.deletePreviousFileMode.SelectedIndex == 1 ? "要保留的文件或路径" : "要删除的文件或文件夹";
			};
			this.deletePreviousFileMode.SelectedIndex = 0;
			this.txtPackagePassword.Text = "";
		}
	}
}
