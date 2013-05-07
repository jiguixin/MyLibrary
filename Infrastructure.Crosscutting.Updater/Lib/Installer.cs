using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Infrastructure.Crosscutting.Updater.Wrapper;
using System.Diagnostics;

namespace Infrastructure.Crosscutting.Updater.Lib
{
	public class Installer : Base
	{
		/// <summary>
		/// 文件操作事件类
		/// </summary>
		public class InstallFileEventArgs : EventArgs
		{
			/// <summary>
			/// 获得来源文件
			/// </summary>
			public string Source { get; private set; }

			/// <summary>
			/// 获得目标文件
			/// </summary>
			public string Destination { get; private set; }

			/// <summary>
			/// 创建 <see cref="InstallFileEventArgs" /> 的新实例
			/// </summary>
			public InstallFileEventArgs(string source, string destination)
			{
				Source = source;
				Destination = destination;
			}
		}

		#region 属性

		/// <summary>
		/// 获得或设置更新的信息
		/// </summary>
		public UpdateInfo UpdateInfo { get; set; }

		public string WorkingRoot { get; set; }

		private string _applicationRoot;
		/// <summary>
		/// 获得或设置应用程序目录
		/// </summary>
		public string ApplicationRoot
		{
			get
			{
				return _applicationRoot;
			}
			set
			{
				_applicationRoot = value;
				if (!_applicationRoot.EndsWith(@"\")) _applicationRoot += @"\";
			}
		}

		private string _sourceFolder;
		/// <summary>
		/// 安装的源文件夹
		/// </summary>
		public string SourceFolder
		{
			get
			{
				return _sourceFolder;
			}
			set
			{
				_sourceFolder = value;
				if (!_sourceFolder.EndsWith(@"\")) _sourceFolder += @"\";
			}
		}

		/// <summary>
		/// 获得备份路径
		/// </summary>
		string BackupPath
		{
			get
			{
				return System.IO.Path.Combine(WorkingRoot, "backup");
			}
		}

		/// <summary>
		/// 获得还原路径
		/// </summary>
		string RollbackPath
		{
			get
			{
				return System.IO.Path.Combine(WorkingRoot, "rollback");
			}
		}

		#endregion

		#region 私有变量

		/// <summary>
		/// 备份文件
		/// </summary>
		readonly List<string> bakList = new List<string>();
		readonly List<string> installedFile = new List<string>();


		#endregion

		#region 事件

		/// <summary>
		/// 开始删除文件
		/// </summary>
		public event EventHandler DeleteFileStart;

		/// <summary>
		/// 引发 <see cref="DeleteFileStart" /> 事件
		/// </summary>
		protected virtual void OnDeleteFileStart()
		{
			if (DeleteFileStart != null)
			{
				DeleteFileStart(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// 删除文件完成事件
		/// </summary>
		public event EventHandler DeleteFileFinished;

		/// <summary>
		/// 引发 <see cref="DeleteFileFinished" /> 事件
		/// </summary>
		protected virtual void OnDeleteFileFinished()
		{
			if (DeleteFileFinished != null)
				DeleteFileFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 删除文件事件
		/// </summary>
		public event EventHandler<InstallFileEventArgs> DeleteFile;

		/// <summary>
		/// 引发 <see cref="DeleteFile" /> 事件
		/// </summary>
		protected virtual void OnDeleteFile(InstallFileEventArgs ea)
		{
			if (DeleteFile == null)
				return;

			DeleteFile(this, ea);
		}



		/// <summary>
		/// 开始安装文件事件
		/// </summary>
		public event EventHandler InstallFileStart;

		/// <summary>
		/// 引发 <see cref="InstallFileStart" /> 事件
		/// </summary>
		protected virtual void OnInstallFileStart()
		{
			if (InstallFileStart == null)
				return;

			InstallFileStart(this, EventArgs.Empty);
		}

		/// <summary>
		/// 完成安装文件事件
		/// </summary>
		public event EventHandler InstallFileFinished;

		/// <summary>
		/// 引发 <see cref="InstallFileFinished" /> 事件
		/// </summary>
		protected virtual void OnInstallFileFinished()
		{
			if (InstallFileFinished == null)
				return;

			InstallFileFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 安装文件事件
		/// </summary>
		public event EventHandler<InstallFileEventArgs> InstallFile;

		/// <summary>
		/// 引发 <see cref="InstallFile" /> 事件
		/// </summary>
		protected virtual void OnInstallFile(InstallFileEventArgs ea)
		{
			if (InstallFile == null)
				return;

			InstallFile(this, ea);
		}

		/// <summary>
		/// 回滚文件开始事件
		/// </summary>
		public event EventHandler RollbackStart;

		/// <summary>
		/// 引发 <see cref="RollbackStart" /> 事件
		/// </summary>
		protected virtual void OnRollbackStart()
		{
			if (RollbackStart == null)
				return;

			RollbackStart(this, EventArgs.Empty);
		}

		/// <summary>
		/// 回滚文件结束事件
		/// </summary>
		public event EventHandler RollbackFinished;

		/// <summary>
		/// 引发 <see cref="RollbackFinished" /> 事件
		/// </summary>
		protected virtual void OnRollbackFinished()
		{
			if (RollbackFinished == null)
				return;

			RollbackFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 回滚文件事件
		/// </summary>
		public event EventHandler<InstallFileEventArgs> RollbackFile;

		/// <summary>
		/// 引发 <see cref="RollbackFile" /> 事件
		/// </summary>
		protected virtual void OnRollbackFile(InstallFileEventArgs ea)
		{
			if (RollbackFile == null)
				return;

			RollbackFile(this, ea);
		}


		#endregion

		/// <summary>
		/// 安装文件
		/// </summary>
		public bool Install(RunworkEventArgs e)
		{
			if (!DeletePreviousFile(e))
			{
				RollbackFiles(e);
				return false;
			}

			if (!InstallFiles(e))
			{
				DeleteInstalledFiles(e);
				RollbackFiles(e);

				return false;
			}

			return true;
		}

		#region 工作函数

		/// <summary>
		/// 删除原始安装文件
		/// </summary>
		bool DeletePreviousFile(RunworkEventArgs e)
		{
			if (this.UpdateInfo.DeleteMethod == DeletePreviousProgramMethod.None) return true;

			e.PostEvent(OnDeleteFileStart);

			var bakPath = RollbackPath;
			var rules = UpdateInfo.GetDeleteFileLimitRuleSet();

			//找到所有文件
			var allOldFiles = System.IO.Directory.GetFiles(ApplicationRoot, "*.*", System.IO.SearchOption.AllDirectories);

			//备份
			foreach (var file in allOldFiles)
			{
				var rPath = file.Remove(0, ApplicationRoot.Length).TrimEnd('\\');
				var dPath = System.IO.Path.Combine(bakPath, rPath);
				if ((UpdateInfo.DeleteMethod == DeletePreviousProgramMethod.AllExceptSpecified && rules.FindIndex(s => s.IsMatch(rPath)) == -1)
						||
					(UpdateInfo.DeleteMethod == DeletePreviousProgramMethod.NoneButSpecified && rules.FindIndex(s => s.IsMatch(rPath)) != -1)
					)
				{
					e.PostEvent(new SendOrPostCallback(_arg => OnDeleteFile(new InstallFileEventArgs(file, dPath))));
					try
					{
						System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(dPath));
						Debug.WriteLine("Moving File: " + file + "  ->  " + dPath);
						System.IO.File.Move(file, dPath);
						installedFile.Add(rPath);
					}
					catch (Exception ex)
					{
						this.Exception = ex;
						return false;
					}
				}
			}
			e.PostEvent(OnDeleteFileFinished);

			return true;
		}



		/// <summary>
		/// 安装文件
		/// </summary>
		bool InstallFiles(RunworkEventArgs e)
		{
			e.PostEvent(OnInstallFileStart);

			string[] filelist = CreateNewFileList();
			string OriginalPath, newVersionFile, backupPath;
			OriginalPath = newVersionFile = backupPath = "";

			try
			{
				foreach (var file in filelist)
				{
					OriginalPath = System.IO.Path.Combine(ApplicationRoot, file);
					newVersionFile = System.IO.Path.Combine(SourceFolder, file);
					backupPath = System.IO.Path.Combine(BackupPath, file);

					e.PostEvent(new SendOrPostCallback(_ => OnInstallFile(new InstallFileEventArgs(newVersionFile, OriginalPath))));

					if (System.IO.File.Exists(OriginalPath))
					{
						System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(backupPath));
						System.IO.File.Move(OriginalPath, backupPath);
						Debug.WriteLine("Backup File: " + OriginalPath + "  ->  " + backupPath);
						installedFile.Add(file);
					}
					System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(OriginalPath));
					System.IO.File.Move(newVersionFile, OriginalPath);
					Debug.WriteLine("Install File: " + newVersionFile + "  ->  " + OriginalPath);
				}
			}
			catch (Exception ex)
			{
				this.Exception = new Exception(string.Format(Infrastructure.Crosscutting.Updater.SR.Updater_InstallFileError, OriginalPath, newVersionFile, ex.Message));
				return false;
			}

			e.PostEvent(OnInstallFileFinished);

			return true;
		}

		/// <summary>
		/// 删除已安装的文件, 并还原原始文件
		/// </summary>
		void DeleteInstalledFiles(RunworkEventArgs e)
		{
			foreach (var filepath in installedFile)
			{
				var originalFile = System.IO.Path.Combine(ApplicationRoot, filepath);
				var backupFile = System.IO.Path.Combine(BackupPath, filepath);

				if (System.IO.File.Exists(originalFile)) System.IO.File.Delete(originalFile);
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(originalFile));
				System.IO.File.Move(backupFile, originalFile);
				Debug.WriteLine("Restore File: " + backupFile + "  ->  " + originalFile);
			}
		}

		/// <summary>
		/// 回滚备份的文件
		/// </summary>
		void RollbackFiles(RunworkEventArgs e)
		{
			e.PostEvent(OnRollbackStart);
			string rootPath = RollbackPath;

			foreach (string file in installedFile)
			{
				string newPath = System.IO.Path.Combine(ApplicationRoot, file);
				string oldPath = System.IO.Path.Combine(rootPath, file);

				OnRollbackFile(new InstallFileEventArgs(oldPath, newPath));

				Debug.WriteLine("Rollback File: " + oldPath + "  ->  " + newPath);
				System.IO.File.Move(oldPath, newPath);
			}

			e.PostEvent(OnRollbackFinished);
		}

		/// <summary>
		/// 创建要安装的新文件列表
		/// </summary>
		/// <returns></returns>
		string[] CreateNewFileList()
		{
			string source = SourceFolder;

			string[] files = System.IO.Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i++)
			{
				files[i] = files[i].Remove(0, source.Length).Trim(new char[] { '\\', '/' });
			}

			return files;
		}


		#endregion
	}
}
