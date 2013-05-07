using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using FSLib.IO;
using FSLib.Threading;
using FSLib.Windows.Forms;
using ICCEmbedded.SharpZipLib.Core;
using ICCEmbedded.SharpZipLib.Zip;
using IScanFilter = ICCEmbedded.SharpZipLib.Core.IScanFilter;

namespace Infrastructure.Crosscutting.Updater.Generator
{
    public partial class Main : FunctionalForm
    {
        public Main()
        {
            InitializeComponent();
            InitWorker();
            InitDropSupport();

            btnOpen.Click += btnOpen_Click;
            CheckForIllegalCrossThreadCalls = false;
        }

        #region 拖放支持

        private void InitDropSupport()
        {
            AllowDrop = true;
            txtNewSoftDir.AllowDrop = true;

            //自身
            DragEnter += (s, e) =>
                             {
                                 StringCollection files;
                                 var doe = e.Data as DataObject;
                                 if (
                                     !doe.ContainsFileDropList()
                                     ||
                                     (files = doe.GetFileDropList()).Count == 0
                                     ||
                                     !File.Exists(files[0])
                                     ||
                                     !files[0].EndsWith(".xml", StringComparison.OrdinalIgnoreCase)
                                     ) return;

                                 e.Effect = DragDropEffects.Link;
                             };
            DragDrop += (s, e) =>
                            {
                                string file = (e.Data as DataObject).GetFileDropList()[0];
                                OpenXML(file);
                            };
            //升级包
            txtNewSoftDir.DragEnter += (s, e) =>
                                           {
                                               StringCollection files;
                                               var doe = e.Data as DataObject;
                                               if (
                                                   !doe.ContainsFileDropList()
                                                   ||
                                                   (files = doe.GetFileDropList()).Count == 0
                                                   ||
                                                   !Directory.Exists(files[0])
                                                   ) return;

                                               e.Effect = DragDropEffects.Link;
                                           };
            txtNewSoftDir.DragDrop +=
                (s, e) => { SelectedNewSoftDirPath = (e.Data as DataObject).GetFileDropList()[0]; };
        }

        #endregion

        #region 界面响应函数

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog
                           {
                               Title = "打开升级信息文件",
                               Filter = "XML信息文件(*.xml)|*.xml"
                           };
            if (open.ShowDialog() != DialogResult.OK) return;
            OpenXML(open.FileName);
        }

        /// <summary>
        /// 打开配置文件
        /// </summary>
        /// <param name="path"></param>
        private void OpenXML(string path)
        {
            var ui = typeof (UpdateInfo).XmlDeserializeFile(path) as UpdateInfo;
            if (ui == null) Information("无法加载信息文件，请确认选择正确的文件");
            else
            {
                txtAfterExecuteArgs.Text = ui.ExecuteArgumentAfter;
                txtAppName.Text = ui.AppName;
                txtAppVersion.Text = ui.AppVersion;
                txtDesc.Text = ui.Desc;
                txtPreExecuteArgs.Text = ui.ExecuteArgumentBefore;
                txtPublishUrl.Text = ui.PublishUrl;
                txtTimeout.Text = ui.ExecuteTimeout.ToString();
                SelectedPackagePath = Path.Combine(Path.GetDirectoryName(path), ui.Package);

                options.UpdateInterface(ui);
            }
        }


        private void btnCreate_Click(object sender, EventArgs e)
        {
            epp.Clear();

            if (string.IsNullOrEmpty(txtAppName.Text))
            {
                epp.SetError(txtAppName, "请输入应用程序名");
                return;
            }
            try
            {
                new Version(txtAppVersion.Text);
            }
            catch (Exception)
            {
                epp.SetError(txtAppVersion, "请输入版本号");
                return;
            }
            if (!Directory.Exists(SelectedNewSoftDirPath))
            {
                epp.SetError(txtNewSoftDir, "请选择新程序的目录");
                return;
            }
            if (string.IsNullOrEmpty(SelectedPackagePath))
            {
                epp.SetError(txtPackagePath, "请选择打包后的组件和升级信息文件所在路径");
                return;
            }
            if (!Directory.Exists(Path.GetDirectoryName(SelectedPackagePath)))
            {
                epp.SetError(txtPackagePath, "文件包所在目录不存在");
                return;
            }
            if (File.Exists(SelectedPackagePath))
            {
                File.Delete(SelectedPackagePath);
            }
            if (File.Exists(GetXmlPath(SelectedPackagePath)))
            {
                File.Delete(GetXmlPath(SelectedPackagePath));
            }

            Create();
        }

        #endregion

        /// <summary>
        /// 获得对应升级包的升级信息文件路径
        /// </summary>
        /// <returns></returns>
        private string GetXmlPath(string pkgPath)
        {
            return Path.ChangeExtension(pkgPath, ".xml"); 
        }
          
        #region 主要创建流程

        private readonly BackgroundWorker bgw = new BackgroundWorker
                                                    {
                                                        WorkerSupportReportProgress = true
                                                    };

        /// <summary>
        /// 初始化线程类
        /// </summary>
        private void InitWorker()
        {
            bgw.WorkerProgressChanged += (s, e) =>
                                             {
                                                 lblStatus.Text = e.Progress.StateMessage;
                                                 pbProgress.Value = e.Progress.TaskPercentage;
                                             };
            bgw.WorkCompleted += (s, e) =>
                                     {
                                         btnCreate.Enabled = true;
                                         pbProgress.Visible = false;
                                         Information("已经成功创建");
                                     };
            bgw.WorkFailed += (s, e) =>
                                  {
                                      btnCreate.Enabled = true;
                                      pbProgress.Visible = false;
                                      Information("出现错误：" + e.Exception.Message);
                                  };
            bgw.DoWork += CreatePackage;
            FormClosing += (s, e) => { e.Cancel = !btnCreate.Enabled; };
        }

        //创建信息的具体操作函数
        private void CreatePackage(object sender, RunworkEventArgs e)
        {
            var info = new UpdateInfo
                           {
                               AppName = txtAppName.Text,
                               AppVersion = txtAppVersion.Text,
                               Desc = txtDesc.Text,
                               ExecuteArgumentAfter = txtAfterExecuteArgs.Text,
                               ExecuteArgumentBefore = txtPreExecuteArgs.Text,
                               PublishUrl = txtPublishUrl.Text,
                               FileExecuteAfter = fileAfterExecute.SelectedFileName,
                               FileExecuteBefore = filePreExecute.SelectedFileName,
                               MD5 = "",
                               Package = Path.GetFileName(txtPackagePath.Text),
                               ExecuteTimeout = txtTimeout.Text.ToInt32(),
                               PackageSize = 0,
                               RequiredMinVersion = ""
                           };
            options.SaveSetting(info);

            var evt = new FastZipEvents();
            evt.ProcessFile += (s, f) => e.ReportProgress(0, 0, "正在压缩文件 " + Path.GetFileName(f.Name));
            var zip = new FastZip(evt);
            if (!info.PackagePassword.IsNullOrEmpty()) zip.Password = info.PackagePassword;
            //zip.CreateZip(this.txtPackagePath.Text, this.txtNewSoftDir.Text, true, "");

            if (ckbModifyTime.Checked)
            {
                IScanFilter df = new DateTimeFilter(DateTime.Parse(dateTimePicker1.Text));
                zip.CreateZip(File.Create(txtPackagePath.Text), txtNewSoftDir.Text, true, df, null);
            }
            else
            {
                zip.CreateZip(txtPackagePath.Text, txtNewSoftDir.Text, true, "", null);
            }

            //校验MD5
            byte[] hash = null;
            int size = 0;
            using (var fs = new ExtendFileStream(SelectedPackagePath, FileMode.Open))
            {
                e.ReportProgress((int) fs.Length, 0, "");
                fs.ProgressChanged += (s, f) => { e.ReportProgress((int) fs.Position); };
                MD5 md5 = MD5.Create();

                hash = md5.ComputeHash(fs);
                size = (int) fs.Length;
            }
            info.MD5 = BitConverter.ToString(hash).Replace("-", "").ToUpper();
            info.PackageSize = size;
            info.XmlSerilizeToFile(GetXmlPath(SelectedPackagePath));

            e.ReportProgress(0, 0, "生成成功，MD5校验：" + info.MD5);
        }

        private void Create()
        {
            btnCreate.Enabled = false;
            pbProgress.Visible = true;
            bgw.RunWorkASync();
        }

        #endregion

        #region 界面属性

        /// <summary>
        /// 获得或设置文件包路径
        /// </summary>
        public string SelectedPackagePath
        {
            get { return txtPackagePath.Text; }
            set { txtPackagePath.Text = value; }
        }

        /// <summary>
        /// 获得或设置选定的新软件目录
        /// </summary>
        public string SelectedNewSoftDirPath
        {
            get { return txtNewSoftDir.Text; }
            set
            {
                txtNewSoftDir.Text = value;
                Environment.CurrentDirectory = value;
                filePreExecute.RootPath = fileAfterExecute.RootPath = value;
            }
        }

        #endregion

        #region 界面响应函数

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() != DialogResult.OK) return;

            SelectedNewSoftDirPath = fbd.SelectedPath;
        }

        private void browseFile_Click(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() != DialogResult.OK) return;

            SelectedPackagePath = sfd.FileName;
        }

        private void txtNewSoftDir_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txtNewSoftDir.Text))
            {
                filePreExecute.RootPath = fileAfterExecute.RootPath = SelectedNewSoftDirPath;
            }
        }

        private void txtPackagePath_TextChanged(object sender, EventArgs e)
        {
        }

        #endregion
    }
}