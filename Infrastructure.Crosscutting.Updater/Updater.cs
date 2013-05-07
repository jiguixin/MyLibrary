using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using Infrastructure.Crosscutting.Updater.Wrapper;
using Infrastructure.Crosscutting.Updater.Lib;

namespace Infrastructure.Crosscutting.Updater
{
	/// <summary>
	/// 自动更新操作类
	/// </summary>
	public class Updater
	{
		private static IAppInfoProvider _appInfoProvider;
		/// <summary>
		/// 获得或设置当前的应用程序信息提供对象
		/// </summary>
		public static IAppInfoProvider AppInfoProvider
		{
			get { return _appInfoProvider ?? (_appInfoProvider = new DefaultAppInfoProvider()); }
			set { _appInfoProvider = value; }
		}

		#region 静态对象

		static Updater _instance;
		static UpdateFound _foundUpdateDialog;

		/// <summary>
		/// 当前的更新实例
		/// </summary>
		public static Updater Instance
		{
			get
			{
				if (_instance == null) new Updater();

				return _instance;
			}
		}

		static bool _processed;

		/// <summary>
		/// 提供一个最简单的自动更新入口
		/// </summary>
		public static void CheckUpdateSimple()
		{
			CheckUpdateSimple(null);
		}

		/// <summary>
		/// 提供一个最简单的自动更新入口
		/// </summary>
		/// <param name="updateUrl">更新URL. 如果不传递或传递空的地址, 请使用 <see cref="T:Infrastructure.Crosscutting.Updater.UpdateableAttribute"/> 属性来标记更新地址</param>
		public static void CheckUpdateSimple(string updateUrl)
		{
			_foundUpdateDialog = new UpdateFound();
			if (!string.IsNullOrEmpty(updateUrl)) Instance.UpdateUrl = updateUrl;
			if (!_processed)
			{
				_processed = true;
				Instance.UpdatesFound += Instance_UpdatesFound;
				Instance.MinmumVersionRequired += Instance_MinmumVersionRequired;
			}
			Instance.BeginCheckUpdateInProcess();
		}

		//要求最低版本
		static void Instance_MinmumVersionRequired(object sender, EventArgs e)
		{
			new Dialogs.MinmumVersionRequired().ShowDialog();
		}

		//找到更新，启动更新
		static void Instance_UpdatesFound(object sender, EventArgs e)
		{
			if (_foundUpdateDialog.ShowDialog() != DialogResult.OK) return;

			Instance.StartExternalUpdater();
		}

		#endregion

		#region 构造函数

		/// <summary>
		/// 手动创建更新类，并指定当前版本和应用程序目录
		/// </summary>
		/// <param name="appVersion">指定的应用程序版本</param>
		/// <param name="appDirectory">指定的应用程序路径</param>
		public Updater(Version appVersion, string appDirectory)
			: this()
		{
			CurrentVersion = appVersion;
			ApplicationDirectory = appDirectory;
		}

		/// <summary>
		/// 使用指定的信息类来提供应用程序需要的信息
		/// </summary>
		/// <param name="provider">提供应用程序信息的 <see cref="IAppInfoProvider"/></param>
		public Updater()
		{
			_instance = this;

			InitializeWorkingPath();

			AutoEndProcessesWithinAppDir = true;
			this.ExternalProcessID = new List<int>();
			this.ExternalProcessName = new List<string>();

			//注册关闭事件, 清理临时目录
			Application.ApplicationExit += (s, e) =>
			{
				CleanTemp();
			};
		}

		#endregion

		#region 事件区域

		/// <summary>
		/// 开始下载更新信息文件
		/// </summary>
		public event EventHandler DownloadUpdateInfo;

		/// <summary>
		/// 引发 <see cref="DownloadUpdateInfo"/> 事件
		/// </summary>
		protected virtual void OnDownloadUpdateInfo()
		{
			if (DownloadUpdateInfo == null)
				return;

			DownloadUpdateInfo(this, EventArgs.Empty);
		}

		/// <summary>
		/// 结束下载更新信息文件
		/// </summary>
		public event EventHandler DownloadUpdateInfoFinished;

		/// <summary>
		/// 引发 <see cref="DownloadUpdateInfoFinished"/> 事件
		/// </summary>
		public virtual void OnDownloadUpdateInfoFinished()
		{
			if (DownloadUpdateInfoFinished == null)
				return;

			DownloadUpdateInfoFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 没有发现更新
		/// </summary>
		public event EventHandler NoUpdatesFound;

		/// <summary>
		/// 引发 <see cref="NoUpdatesFound"/> 事件
		/// </summary>
		protected virtual void OnNoUpdatesFound()
		{
			if (NoUpdatesFound == null)
				return;

			NoUpdatesFound(this, EventArgs.Empty);
		}

		/// <summary>
		/// 发现了更新
		/// </summary>
		public event EventHandler UpdatesFound;

		/// <summary>
		/// 引发 <see cref="UpdatesFound"/> 事件
		/// </summary>
		protected virtual void OnUpdatesFound()
		{
			if (UpdatesFound == null)
				return;

			UpdatesFound(this, EventArgs.Empty);
		}

		/// <summary>
		/// 开始下载升级包
		/// </summary>
		public event EventHandler DownloadPackage;

		/// <summary>
		/// 引发 <see cref="DownloadPackage"/> 事件
		/// </summary>
		protected virtual void OnDownloadPackage()
		{
			if (DownloadPackage == null)
				return;

			DownloadPackage(this, EventArgs.Empty);
		}

		/// <summary>
		/// 完成下载升级包
		/// </summary>
		public event EventHandler DownloadPackageFinished;

		/// <summary>
		/// 引发 <see cref="DownloadPackageFinished"/> 事件
		/// </summary>
		protected virtual void OnDownloadPackageFinished()
		{
			if (DownloadPackageFinished == null)
				return;

			DownloadPackageFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 下载状态发生变化
		/// </summary>
		public event EventHandler<Infrastructure.Crosscutting.Updater.Wrapper.RunworkEventArgs> DownloadProgressChanged;

		/// <summary>
		/// 引发 <see cref="DownloadProgressChanged"/> 事件
		/// </summary>
		/// <param name="e">类型为 <see cref="T:FSLib.Threading.RunworkEventArgs"/> 的参数</param>
		protected virtual void OnDownloadProgressChanged(Infrastructure.Crosscutting.Updater.Wrapper.RunworkEventArgs e)
		{
			if (DownloadProgressChanged == null)
				return;

			DownloadProgressChanged(this, e);
		}

		/// <summary>
		/// 正在关闭主程序
		/// </summary>
		public event EventHandler<QueryCloseApplicationEventArgs> QueryCloseApplication;

		/// <summary>
		/// 引发 <see cref="QueryCloseApplication"/> 事件
		/// </summary>
		protected virtual void OnQueryCloseApplication(QueryCloseApplicationEventArgs e)
		{
			if (QueryCloseApplication == null)
				return;

			QueryCloseApplication(this, e);
		}

		/// <summary>
		/// 正在安装更新
		/// </summary>
		public event EventHandler InstallUpdates;

		/// <summary>
		/// 引发 <see cref="InstallUpdates"/> 事件
		/// </summary>
		protected virtual void OnInstallUpdates()
		{
			if (InstallUpdates == null)
				return;

			InstallUpdates(this, EventArgs.Empty);
		}

		/// <summary>
		/// 已经完成更新
		/// </summary>
		public event EventHandler UpdateFinsihed;

		/// <summary>
		/// 引发 <see cref="UpdateFinsihed"/> 事件
		/// </summary>
		protected virtual void OnUpdateFinsihed()
		{
			if (UpdateFinsihed == null)
				return;

			UpdateFinsihed(this, EventArgs.Empty);
		}


		/// <summary>
		/// 更新中发生错误
		/// </summary>
		public event EventHandler Error;

		protected virtual void OnError()
		{
			if (Error == null) return;

			Error(this, EventArgs.Empty);
		}

		/// <summary>
		/// 正在验证安装包
		/// </summary>
		public event EventHandler VerifyPackage;

		/// <summary>
		/// 引发 <see cref="VerifyPackage"/> 事件
		/// </summary>
		protected virtual void OnVerifyPackage()
		{
			if (VerifyPackage == null) return;

			VerifyPackage(this, EventArgs.Empty);
		}

		/// <summary>
		/// 完整验证安装包
		/// </summary>
		public event EventHandler VerifyPackageFinished;

		/// <summary>
		/// 引发 <see cref="VerifyPackageFinished"/> 事件
		/// </summary>
		protected virtual void OnVerifyPackageFinished()
		{
			if (VerifyPackageFinished == null) return;

			VerifyPackageFinished(this, EventArgs.Empty);
		}

		/// <summary>
		/// 不满足最低版本要求
		/// </summary>
		public event EventHandler MinmumVersionRequired;

		/// <summary>
		/// 引发 <see cref="MinmumVersionRequired" /> 事件
		/// </summary>
		protected virtual void OnMinmumVersionRequired()
		{
			if (MinmumVersionRequired == null)
				return;

			MinmumVersionRequired(this, EventArgs.Empty);
		}

		#endregion

		#region 属性

		/// <summary>
		/// 获得一个值，表示当前的自动升级信息是否已经下载完全
		/// </summary>
		public bool IsUpdateInfoDownloaded
		{
			get
			{
				return !string.IsNullOrEmpty(UpdateContent) || System.IO.File.Exists(UpdateInfoFilePath);
			}
		}

		/// <summary>
		/// 获得一个值，表示当前是否工作在独立更新状态
		/// </summary>
		public bool IsWorkInUpdateMode { get; private set; }

		/// <summary>
		/// 获得更新中发生的错误
		/// </summary>
		public Exception Exception { get; private set; }

		/// <summary>
		/// 获得 update.xml 的文件路径
		/// </summary>
		/// <returns></returns>
		public string UpdateInfoFilePath
		{
			get
			{
				return System.IO.Path.Combine(WorkingRoot, "update.xml");
			}
		}

		/// <summary>
		/// 获得升级包路径
		/// </summary>
		/// <returns></returns>
		string UpdatePackageFilePath
		{
			get
			{
				if (UpdateInfo == null) throw new InvalidOperationException();
				return System.IO.Path.Combine(WorkingRoot, UpdateInfo.Package);
			}
		}

		/// <summary>
		/// 获得或设置一个值，指示着当自动更新的时候是否将应用程序目录中的所有进程都作为主进程请求结束
		/// </summary>
		public bool AutoEndProcessesWithinAppDir { get; set; }

		/// <summary>
		/// 外部要结束的进程ID列表
		/// </summary>
		public IList<int> ExternalProcessID { get; private set; }

		/// <summary>
		/// 外部要结束的进程名称
		/// </summary>
		public IList<string> ExternalProcessName { get; private set; }

		/// <summary>
		/// 获得安装包解压后文件路径
		/// </summary>
		/// <returns></returns>
		string UpdateSourceDirectory
		{
			get
			{
				return System.IO.Path.Combine(WorkingRoot, "package");
			}
		}

		private Lib.Installer _fileInstaller;
		/// <summary>
		/// 获得文件安装对象
		/// </summary>
		public Lib.Installer FileInstaller
		{
			get
			{
				return _fileInstaller ?? (_fileInstaller = new Lib.Installer()
				{
					SourceFolder = UpdateSourceDirectory,
					ApplicationRoot = ApplicationDirectory,
					WorkingRoot = WorkingRoot
				});
			}
		}

		#endregion

		#region 初始化函数

		/// <summary>
		/// 确定工作路径
		/// </summary>
		void InitializeWorkingPath()
		{
			Assembly currentAssembly = Assembly.GetExecutingAssembly();
			Assembly callingAssembly = Assembly.GetEntryAssembly();
			var cmdArgs = Environment.GetCommandLineArgs();

			if (currentAssembly == callingAssembly && cmdArgs.Length >= 3)
			{
				IsWorkInUpdateMode = true;
				WorkingRoot = Application.StartupPath;
				UpdateUrl = Environment.GetCommandLineArgs()[3];
				CurrentVersion = new Version(Environment.GetCommandLineArgs()[1]);
				ApplicationDirectory = Environment.GetCommandLineArgs()[2];
			}
			else
			{
				IsWorkInUpdateMode = false;
				WorkingRoot = System.IO.Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString());

				//读取升级的URL
				if (string.IsNullOrEmpty(UpdateUrl))
				{
					object[] attrlist = callingAssembly.GetCustomAttributes(typeof(UpdateableAttribute), true);
					if (attrlist.Length > 0)
					{
						UpdateUrl = (attrlist[0] as UpdateableAttribute).UpdateUrl;
					}
				}

				CurrentVersion = AppInfoProvider.CurrentVersion;
				ApplicationDirectory = AppInfoProvider.CurrentApplicationDirectory;
				if (string.IsNullOrEmpty(UpdateUrl)) UpdateUrl = AppInfoProvider.UpdateUrl;
			}
		}

		#endregion

		#region 私有变量

		/// <summary>
		/// 是否正在操作中
		/// </summary>
		bool _inRun;

		/// <summary>
		/// 工作根路径
		/// </summary>
		public string WorkingRoot { get; private set; }

		/// <summary>
		/// 升级路径
		/// </summary>
		public string UpdateUrl { get; set; }

		/// <summary>
		/// 当前版本
		/// </summary>
		public Version CurrentVersion { get; set; }

		/// <summary>
		/// 应用程序路径
		/// </summary>
		public string ApplicationDirectory { get; set; }

		/// <summary>
		/// 自动更新信息
		/// </summary>
		public UpdateInfo UpdateInfo { get; private set; }

		/// <summary>
		/// 更新信息内容
		/// </summary>
		public string UpdateContent { get; private set; }

		#endregion

		#region 检查更新部分

		/// <summary>
		/// 开始检测更新
		/// </summary>
		public void BeginCheckUpdateInProcess()
		{
			if (string.IsNullOrEmpty(this.UpdateUrl)) throw new InvalidOperationException(Infrastructure.Crosscutting.Updater.SR.Updater_AssemblyNotMarkedAsUpdateable);
			if (_inRun) return;

			var bgw = new BackgroundWorker()
			{
				WorkerSupportReportProgress = true
			};
			bgw.DoWork += DownloadUpdateInfoInternal;
			bgw.WorkFailed += (s, e) =>
			{
				_inRun = false;
				this.Exception = e.Exception;
				OnError();
			};
			bgw.WorkCompleted += (s, e) =>
			{
				if (this.UpdateInfo == null) return;

				if (!string.IsNullOrEmpty(UpdateInfo.RequiredMinVersion))
				{
					if (new Version(UpdateInfo.RequiredMinVersion) > CurrentVersion)
					{
						OnMinmumVersionRequired();
						return;
					}
				}

				var newVer = new Version(UpdateInfo.AppVersion);

				if (newVer <= CurrentVersion)
				{
					OnNoUpdatesFound();
				}
				else OnUpdatesFound();

				_inRun = false;
			};
			bgw.WorkerProgressChanged += (s, e) =>
			{
				OnDownloadProgressChanged(e);
			};
			_inRun = true;
			bgw.RunWorkASync();
		}

		/// <summary>
		/// 下载更新信息
		/// </summary>
		void DownloadUpdateInfoInternal(object sender, RunworkEventArgs e)
		{
			if (!IsUpdateInfoDownloaded)
			{
				var client = new System.Net.WebClient();
				client.DownloadProgressChanged += (x, y) =>
				{
					e.ReportProgress((int)y.TotalBytesToReceive, (int)y.BytesReceived);
				};

				//下载更新信息
				e.PostEvent(OnDownloadUpdateInfo);

				//下载信息时不直接下载到文件中.这样不会导致始终创建文件夹
				var finished = false;
				Exception ex = null;
				client.DownloadDataCompleted += (x, y) =>
				{
					ex = y.Error;
					if (ex == null) UpdateContent = System.Text.Encoding.UTF8.GetString(y.Result);
					finished = true;
				};
				client.DownloadDataAsync(new Uri(UpdateUrl));
				while (!finished)
				{
					System.Threading.Thread.Sleep(50);
				}
				if (this.Exception != null) throw ex;
				e.PostEvent(OnDownloadUpdateInfoFinished);

				//是否返回了正确的结果?
				if (string.IsNullOrEmpty(UpdateContent))
				{
					throw new ApplicationException("服务器返回了不正确的更新结果");
				}
			}
			if (UpdateInfo == null)
			{
				if (string.IsNullOrEmpty(UpdateContent))
				{
					UpdateContent = System.IO.File.ReadAllText(UpdateInfoFilePath, System.Text.Encoding.UTF8);
				}

				UpdateInfo = XMLSerializeHelper.XmlDeserializeFromString<UpdateInfo>(UpdateContent);
			}
		}

		#endregion

		#region 更新主要步骤

		#region 关闭外部程序

		/// <summary>
		/// 关闭主程序进程
		/// </summary>
		bool CloseApplication(RunworkEventArgs e)
		{
			string[] argus = Environment.GetCommandLineArgs();
			List<Process> closeApplication = new List<Process>();

			for (int i = 4; i < argus.Length; i++)
			{
				string tid = argus[i];
				if (tid.StartsWith("*"))
				{
					//TID模式
					int t = int.Parse(tid.Trim(new char[] { '*' }));

					try
					{
						Process p = Process.GetProcessById(t);
						if (p != null) closeApplication.Add(p);
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message);
					}
				}
				else
				{
					Process[] plist = Process.GetProcessesByName(tid);
					if (plist.Length > 0) closeApplication.AddRange(plist);
				}
			}

			if (closeApplication.Count > 0)
			{
				var evt = new QueryCloseApplicationEventArgs(closeApplication, NotifyUserToCloseApp);
				e.PostEvent(new SendOrPostCallback(_ => OnQueryCloseApplication(evt)));
				while (!evt.IsCancelled.HasValue)
				{
					System.Threading.Thread.Sleep(100);
				}
				return !evt.IsCancelled.Value;
			}

			return true;
		}
		/// <summary>
		/// 提示用户关闭程序
		/// </summary>
		/// <param name="plist"></param>
		/// <returns></returns>
		static void NotifyUserToCloseApp(QueryCloseApplicationEventArgs e)
		{
			using (var ca = new CloseApp())
			{
				ca.AttachProcessList(e.Processes);
				e.IsCancelled = ca.ShowDialog() != DialogResult.OK;
			}
		}
		#endregion

		#region 外部进程调度

		/// <summary>
		/// 替换环境变量
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		string ReplaceEnvVar(string v)
		{
			v = v.Replace("$appdir$", "\"" + ApplicationDirectory + "\"");

			return v;
		}

		/// <summary>
		/// 执行外部进程
		/// </summary>
		/// <param name="program"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		bool RunExternalProgram(string program, string arguments, bool waitingForExit)
		{
			if (string.IsNullOrEmpty(program)) return true;

			ProcessStartInfo psi = new ProcessStartInfo(program, ReplaceEnvVar(arguments));
			Process p = Process.Start(psi);

			if (waitingForExit)
			{
				if (this.UpdateInfo.ExecuteTimeout > 0)
				{
					p.WaitForExit(1000 * this.UpdateInfo.ExecuteTimeout);
				}
				else p.WaitForExit();

				Action<Process> actor = m =>
				{
					ProgramExecuteTimeout pet = new ProgramExecuteTimeout();
					if (pet.ShowDialog() == DialogResult.OK)
					{
						if (!m.HasExited) m.Kill();
					}
				};
				while (!p.HasExited)
				{
					Application.OpenForms[0].Invoke(actor, p);
				}
			}

			return true;
		}
		/// <summary>
		/// 执行外部进程-安装后
		/// </summary>
		/// <returns></returns>
		bool RunExternalProgramAfter()
		{
			return RunExternalProgram(UpdateInfo.FileExecuteAfter, UpdateInfo.ExecuteArgumentAfter, false);
		}

		/// <summary>
		/// 执行外部进程-安装前
		/// </summary>
		/// <returns></returns>
		bool RunExternalProgramBefore()
		{
			return RunExternalProgram(UpdateInfo.FileExecuteBefore, UpdateInfo.ExecuteArgumentBefore, true);
		}
		#endregion

		/// <summary>
		/// 开始进行更新
		/// </summary>
		internal void BeginUpdate()
		{
			var bgw = new BackgroundWorker() { WorkerSupportReportProgress = true };
			bgw.DoWork += UpdateInternal;
			bgw.WorkerProgressChanged += (s, e) => OnDownloadProgressChanged(e);
			bgw.WorkFailed += (s, e) =>
			{
				this.Exception = e.Exception;
				OnError();
			};
			bgw.RunWorkASync();
		}


		//BMK 更新主函数 (正式更新)
		/// <summary>
		/// 运行更新进程(主更新进程)
		/// </summary>
		void UpdateInternal(object sender, RunworkEventArgs e)
		{
			Action<int, int> reportProgress = (x, y) => e.ReportProgress(x, y);

			DownloadUpdateInfoInternal(sender, e);

			//下载更新包
			e.PostEvent(OnDownloadPackage);
			var packageDownloader = new Lib.PackageDownloader(AppInfoProvider.GetUpdatePackageUrl(UpdateUrl, UpdateInfo), UpdatePackageFilePath) { ProgressReportor = reportProgress };
			if (!packageDownloader.Download()) { this.Exception = packageDownloader.Exception; return; }
			e.PostEvent(OnDownloadPackageFinished);

			//验证包
			e.PostEvent(OnVerifyPackage);
			var packageValidator = new Lib.PackageValidator(UpdateInfo.MD5, UpdatePackageFilePath);
			if (!packageValidator.Validate((x, y) => e.ReportProgress(x, y))) throw new InvalidProgramException(Infrastructure.Crosscutting.Updater.SR.Updater_MD5VerifyFailed);
			e.PostEvent(OnVerifyPackageFinished);

			//解压缩并安装包
			ExtractPackage(e);

			//关闭主程序
			if (!CloseApplication(e)) throw new Exception(Infrastructure.Crosscutting.Updater.SR.Updater_UpdateCanceledByCloseApp);

			//运行安装前进程
			RunExternalProgramBefore();

			//安装文件
			var installer = this.FileInstaller;
			installer.ProgressReportor = reportProgress;
			installer.UpdateInfo = this.UpdateInfo;
			if (!installer.Install(e))
			{
				throw installer.Exception;
			}

			//运行安装后进程
			RunExternalProgramAfter();

			//完成更新
			e.PostEvent(OnUpdateFinsihed);
		}


		/// <summary>
		/// 解开安装包
		/// </summary>
		void ExtractPackage(RunworkEventArgs e)
		{
			e.ReportProgress(0, 0, Infrastructure.Crosscutting.Updater.SR.Updater_ExtractPackage);
			ICCEmbedded.SharpZipLib.Zip.FastZipEvents evt = new ICCEmbedded.SharpZipLib.Zip.FastZipEvents();
			evt.ProcessFile += (s, f) =>
			{
				e.ReportProgress(0, 0, "正在解压缩 " + System.IO.Path.GetFileName(f.Name));
			};
			ICCEmbedded.SharpZipLib.Zip.FastZip fz = new ICCEmbedded.SharpZipLib.Zip.FastZip(evt);
			if (!string.IsNullOrEmpty(UpdateInfo.PackagePassword)) fz.Password = UpdateInfo.PackagePassword;
			fz.ExtractZip(UpdatePackageFilePath, UpdateSourceDirectory, "");
		}

		#endregion

		#region 启动外部更新进程

		/// <summary>
		/// 复制更新程序到临时目录并启动
		/// </summary>
		void CopyAndStartUpdater(string[] ownerProcessList)
		{
			//写入更新文件
			var updateinfoFile = UpdateInfoFilePath;
			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(updateinfoFile));
			System.IO.File.WriteAllText(updateinfoFile, UpdateContent, System.Text.Encoding.UTF8);

			//启动外部程序
			Assembly runningAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			string file = runningAssembly.Location;

			System.IO.File.Copy(file, System.IO.Path.Combine(WorkingRoot, System.IO.Path.GetFileName(file)), true);
			//复制引用
			var assemblies = runningAssembly.GetReferencedAssemblies();
			foreach (var ass in assemblies)
			{
				var location = Assembly.Load(ass).Location;
				if (!location.StartsWith(ApplicationDirectory, StringComparison.OrdinalIgnoreCase)) continue;
				System.IO.File.Copy(location, System.IO.Path.Combine(WorkingRoot, System.IO.Path.GetFileName(location)), true);
			}

			//启动
			System.Text.StringBuilder sb = new StringBuilder(0x400);
			sb.AppendFormat("\"{0}\" ", CurrentVersion.ToString());
			sb.AppendFormat("\"{0}\" ", ApplicationDirectory);
			sb.AppendFormat("\"{0}\" ", UpdateUrl);

			FetchProcessList(ownerProcessList).ForEach(s => sb.AppendFormat("\"{0}\" ", s));

			ProcessStartInfo psi = new ProcessStartInfo(System.IO.Path.Combine(WorkingRoot, System.IO.Path.GetFileName(file)), sb.ToString())
			{
				UseShellExecute = true
			};
			if (Environment.OSVersion.Version.Major > 5) psi.Verb = "runas";
			Process.Start(psi);
		}

		/// <summary>
		/// 获得自动更新所需要结束的进程列表
		/// </summary>
		/// <param name="ownerProcess"></param>
		/// <returns></returns>
		List<string> FetchProcessList(string[] ownerProcess)
		{
			var list = new List<string>(this.ExternalProcessName);
			var mainProcessID = System.Diagnostics.Process.GetCurrentProcess().Id;
			list.Add("*" + mainProcessID);
			list.AddRange(ExtensionMethod.Select(this.ExternalProcessID, s => "*" + s));
			if (this.AutoEndProcessesWithinAppDir)
			{
				Func<Process, string> pathLookup = _ =>
				{
					try
					{
						return _.MainModule.FileName;
					}
					catch (Exception)
					{
						return string.Empty;
					}
				};
				//获得所有进程
				var processes = System.Diagnostics.Process.GetProcesses();
				//查找当前目录下的进程, 并加入到列表
				foreach (var s in processes)
				{
					if (!this.ExternalProcessID.Contains(s.Id) && s.Id != mainProcessID && pathLookup(s).StartsWith(ApplicationDirectory, StringComparison.OrdinalIgnoreCase))
						list.Add("*" + s.Id);
				}
			}

			if (ownerProcess != null) list.AddRange(ownerProcess);

			return list;
		}

		/// <summary>
		/// 启动进程外更新程序
		/// </summary>
		/// <param name="ownerProcess">主进程列表</param>
		public void StartExternalUpdater(string[] ownerProcess)
		{
			CopyAndStartUpdater(ownerProcess);
		}

		/// <summary>
		/// 启动进程外更新程序
		/// </summary>
		public void StartExternalUpdater()
		{
			StartExternalUpdater(null);
		}
		#endregion

		#region 临时目录清理

		/// <summary>
		/// 清理临时目录
		/// </summary>
		public void CleanTemp()
		{
			if (!IsWorkInUpdateMode || !System.IO.Directory.Exists(WorkingRoot)) return;

			var localpath = Environment.ExpandEnvironmentVariables(@"%TEMP%\FSLib.DeleteTmp.exe");
			System.IO.File.WriteAllBytes(localpath, Properties.Resources.FSLib_App_Utilities);
			var arg = "deletetmp \"" + Process.GetCurrentProcess().Id + "\" \"" + this.WorkingRoot + "\"";
			System.Diagnostics.Process.Start(localpath, arg);
		}



		#endregion
	}
}
