using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;


namespace AutoPatcherAdmin
{

    public partial class AMain : Form
    {
        // 补丁列表文件名
        public const string PatchFileName = @"PList.gz";

        // 排除列表
        public string[] ExcludeList = new string[] { "Thumbs.db" };

        public List<FileInformation> OldList, NewList;
        public Queue<FileInformation> UploadList;
        private Stopwatch _stopwatch = Stopwatch.StartNew();

        // 更新总字节、完成字节
        long _totalBytes, _completedBytes;

        public AMain()
        {
            InitializeComponent();

            ClientTextBox.Text = Settings.Client;
            HostTextBox.Text = Settings.Host;
            LoginTextBox.Text = Settings.Login;
            PasswordTextBox.Text = Settings.Password;
            AllowCleanCheckBox.Checked = Settings.AllowCleanUp;

            // 初始化悬停信息提示
            ToolTip ttpInfo = new ToolTip();
            ttpInfo.InitialDelay = 200;
            ttpInfo.AutoPopDelay = 10 * 1000;
            ttpInfo.ReshowDelay = 200;
            ttpInfo.ShowAlways = true;
            ttpInfo.IsBalloon = false;

            // 设置悬停提示信息
            string tipOverClientBox = "Mir2 客户端目录";
            string tipOverHostBox = "FTP 服务器的地址";
            string tipOverLoginBox = "用于登录服务器的用户名";
            string tipOverPasswordtBox = "用于登录服务器的密码";
            string tipOverCheckBox = "允许清理服务器上无用的文件";
            string tipOverProcessButton = "生成及上传客户端补丁到服务器";
            string tipOverListButton = "仅生成上传补丁列表 PList.gz";
            ttpInfo.SetToolTip(ClientTextBox, tipOverClientBox);
            ttpInfo.SetToolTip(HostTextBox, tipOverHostBox);
            ttpInfo.SetToolTip(LoginTextBox, tipOverLoginBox);
            ttpInfo.SetToolTip(PasswordTextBox, tipOverPasswordtBox);
            ttpInfo.SetToolTip(AllowCleanCheckBox, tipOverCheckBox);
            ttpInfo.SetToolTip(ProcessButton, tipOverProcessButton);
            ttpInfo.SetToolTip(ListButton, tipOverListButton);
        }


        private void ProcessButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 配置文件参数保存
                ProcessButton.Enabled = false;
                Settings.Client = ClientTextBox.Text;
                Settings.Host = HostTextBox.Text;
                Settings.Login = LoginTextBox.Text;
                Settings.Password = PasswordTextBox.Text;
                Settings.AllowCleanUp = AllowCleanCheckBox.Checked;

                // 旧列表、新列表、更新列表
                OldList = new List<FileInformation>();
                NewList = new List<FileInformation>();
                UploadList = new Queue<FileInformation>();

                // 从服务器上下载补丁列表文件
                byte[] data = Download(PatchFileName);

                // 根据文件生成 旧列表
                if (data != null)
                {
                    using (MemoryStream stream = new MemoryStream(data))
                    using (BinaryReader reader = new BinaryReader(stream))
                        ParseOld(reader);
                }

                ActionLabel.Text = "检查文件...";
                Refresh();
                
                // 根据本地文件生成 新列表
                CheckFiles();

                // 遍历[新列表]且与[旧列表]对比，生成[更新列表]
                for (int i = 0; i < NewList.Count; i++)
                {
                    // 遍历[新列表]内文件信息
                    FileInformation info = NewList[i];

                    // 文件存在[排除列表]中，跳过本次循环
                    if (InExcludeList(info.FileName)) continue;

                    // 文件是否需要更新
                    if (NeedUpdate(info))
                    {
                        // 加入到[更新列表]中
                        UploadList.Enqueue(info);
                        _totalBytes += info.Length;
                    }
                    else
                    {
                        for (int o = 0; o < OldList.Count; o++)
                        {
                            if (OldList[o].FileName != info.FileName) continue;
                            NewList[i] = OldList[o];
                            break;
                        }
                    }
                }


                BeginUpload();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                ActionLabel.Text = "Error...";
            }

        }

        /// <summary>
        /// 开始上传
        /// </summary>
        private void BeginUpload()
        {
            // 检查[更新列表]是否为空
            if (UploadList == null ) return;

            // 检查[更新列表]更新文件数量
            if (UploadList.Count == 0)
            {
                // 清理服务器中文件
                CleanUp();

                // 生成列表文件并上传
                Upload(new FileInformation {FileName = PatchFileName}, CreateNew());
                UploadList = null;
                ActionLabel.Text = string.Format("完成...");
                ProcessButton.Enabled = true;
                return;
            }

            // 
            ActionLabel.Text = string.Format("上传... 文件: {0}, 总大小: {1:#,##0}MB (未压缩)", UploadList.Count, (_totalBytes - _completedBytes)/1048576);

            progressBar1.Value = (int) (_completedBytes*100/_totalBytes) > 100 ? 100 : (int) (_completedBytes*100/_totalBytes);

            FileInformation info = UploadList.Dequeue();

            Upload(info, File.ReadAllBytes(Settings.Client + (info.FileName == "AutoPatcher.gz" ? "AutoPatcher.exe" : info.FileName)));
        }

        /// <summary>
        /// 清理FTP服务器中无用文件 (新列表中无此文件 || 在排除列表中)
        /// </summary>
        private void CleanUp()
        {
            if (!Settings.AllowCleanUp) return;

            for (int i = 0; i < OldList.Count; i++)
            {
                // 在排除列表中 && 新列表中无此文件，则继续
                if (NeedFile(OldList[i].FileName)) continue;

                try
                {
                    FtpWebRequest request = (FtpWebRequest) WebRequest.Create(new Uri(Settings.Host + OldList[i].FileName + ".gz"));
                    request.Credentials = new NetworkCredential(Settings.Login, Settings.Password);
                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    FtpWebResponse response = (FtpWebResponse) request.GetResponse();
                    response.Close();
                }
                catch 
                {

                }
            }

        }

        /// <summary>
        /// 对比[新旧列表]文件名相同且不在[排除列表]中，返回 true
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool NeedFile(string fileName)
        {
            for (int i = 0; i < NewList.Count; i++)
            {
                if (fileName.EndsWith(NewList[i].FileName) && !InExcludeList(NewList[i].FileName))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取文件加入[旧列表]
        /// </summary>
        /// <param name="reader">文件</param>
        public void ParseOld(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                OldList.Add(new FileInformation(reader));
        }

        /// <summary>
        /// 根据[新列表]，生成新列表文件
        /// </summary>
        /// <returns></returns>
        public byte[] CreateNew()
        {
            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(NewList.Count);
                for (int i = 0; i < NewList.Count; i++)
                    NewList[i].Save(writer);

                return stream.ToArray();
            }

        }

        /// <summary>
        /// 检查client下所有文件信息，并加入到 新列表 内
        /// </summary>
        public void CheckFiles()
        {
            // 获取客户端目录下所有文件
            string[] files = Directory.GetFiles(Settings.Client, "*.*" ,SearchOption.AllDirectories);

            // 将文件信息组合并加入到 新列表 内
            for (int i = 0; i < files.Length; i++)
                NewList.Add(GetFileInformation(files[i]));
        }

        /// <summary>
        /// 检查文件是否在[排除列表]内, 存在返回 true
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool InExcludeList(string fileName)
        {
            foreach (var item in ExcludeList)
            {
                if (fileName.EndsWith(item)) return true;
            }

            return false;
        }

        /// <summary>
        /// 检查文件是否需要更新, 需要则返回 true
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool NeedUpdate(FileInformation info)
        {
            for (int i = 0; i < OldList.Count; i++)
            {
                FileInformation old = OldList[i];
                // 对比文件信息，若相同则跳过本次循环
                if (old.FileName != info.FileName) continue;
                if (old.Length != info.Length) return true;
                if (old.Creation != info.Creation) return true;

                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileInformation GetFileInformation(string fileName)
        {
            FileInfo info = new FileInfo(fileName);

            // 将文件信息组合（文件名、文件大小、修改时间）
            FileInformation file =  new FileInformation
                {
                    FileName = fileName.Remove(0, Settings.Client.Length),
                    Length = (int) info.Length,
                    Creation = info.LastWriteTime
                };

            if (file.FileName == "AutoPatcher.exe")
                file.FileName = "AutoPatcher.gz";

            return file;
        }


        /// <summary>
        /// 从FTP服务器上下载指定文件
        /// </summary>
        /// <param name="fileName">需要下载的文件名</param>
        /// <returns>成功返回下载的文件，失败返回 null</returns>
        public byte[] Download(string fileName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(Settings.Login, Settings.Password);
                    return client.DownloadData(Settings.Host + "/" + fileName);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 连接服务器，并上传文件
        /// </summary>
        /// <param name="info">文件信息</param>
        /// <param name="raw"></param>
        /// <param name="retry"></param>
        public void Upload(FileInformation info, byte[] raw, bool retry = true)
        {
            string fileName = info.FileName.Replace(@"\", "/");

            // 为文件名添加 .gz 后缀
            if (fileName != "AutoPatcher.gz" && fileName != "PList.gz")
                fileName += ".gz";

            using (WebClient client = new WebClient())
            {
                // 初始化连接并登录
                client.Credentials = new NetworkCredential(Settings.Login, Settings.Password);

                byte[] data = !retry ? raw : raw;
                info.Compressed = data.Length;

                // 数据上传进度
                client.UploadProgressChanged += (o, e) =>
                    {
                        int value = (int)(100 * e.BytesSent / e.TotalBytesToSend);
                        progressBar2.Value = value > progressBar2.Maximum ? progressBar2.Maximum : value;

                        FileLabel.Text = fileName;
                        SizeLabel.Text = string.Format("{0} KB / {1} KB", e.BytesSent / 1024, e.TotalBytesToSend  / 1024);
                        SpeedLabel.Text = ((double) e.BytesSent/1024/_stopwatch.Elapsed.TotalSeconds).ToString("0.##") + " KB/s";
                    };

                // 数据上传完成
                client.UploadDataCompleted += (o, e) =>
                    {
                        _completedBytes += info.Length;

                        if (e.Error != null && retry)
                        {
                            CheckDirectory(Path.GetDirectoryName(fileName));
                            Upload(info, data, false);
                            return;
                        }

                        if (info.FileName == PatchFileName)
                        {
                            FileLabel.Text = "完成...";
                            SizeLabel.Text = "完成...";
                            SpeedLabel.Text = "完成...";
                            return;
                        }

                        progressBar1.Value = (int)(_completedBytes * 100 / _totalBytes) > 100 ? 100 : (int)(_completedBytes * 100 / _totalBytes);
                        BeginUpload();
                    };

                _stopwatch = Stopwatch.StartNew();

                client.UploadDataAsync(new Uri(Settings.Host + fileName), data);
            }
        }

        /// <summary>
        /// 检查 FTP服务器 是否存在该目录，不存在则创建
        /// </summary>
        /// <param name="directory"></param>
        public void CheckDirectory(string directory)
        {
            string Directory = "";
            char[] splitChar = { '\\' };
            string[] DirectoryList = directory.Split(splitChar);

            foreach (string directoryCheck in DirectoryList)
            {
                Directory += "\\" + directoryCheck;
            try
              {
                        if (string.IsNullOrEmpty(Directory)) return;

                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Settings.Host + Directory + "/");
                        request.Credentials = new NetworkCredential(Settings.Login, Settings.Password);
                        request.Method = WebRequestMethods.Ftp.MakeDirectory;

                        request.UsePassive = true;
                        request.UseBinary = true;
                        request.KeepAlive = false;
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        Stream ftpStream = response.GetResponseStream();

                        if (ftpStream != null) ftpStream.Close();
                        response.Close();

              }
                    catch (WebException ex)
                    {
                        FtpWebResponse response = (FtpWebResponse)ex.Response;
                        response.Close();
                    }
            }
        }

        /// <summary>
        /// 解压缩数据
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] raw)
        {
            using (GZipStream gStream = new GZipStream(new MemoryStream(raw), CompressionMode.Decompress))
            {
                const int size = 4096; //4kb
                byte[] buffer = new byte[size];
                using (MemoryStream mStream = new MemoryStream())
                {
                    int count;
                    do
                    {
                        count = gStream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            mStream.Write(buffer, 0, count);
                        }
                    } while (count > 0);
                    return mStream.ToArray();
                }
            }
        }

        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                using (GZipStream gStream = new GZipStream(mStream, CompressionMode.Compress, true))
                    gStream.Write(raw, 0, raw.Length);
                return mStream.ToArray();
            }
        }


        /// <summary>
        /// 根据[新列表]与[旧列表]，生成列表文件并上传服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListButton_Click(object sender, EventArgs e)
        {
            // 获取配置信息
            Settings.Client = ClientTextBox.Text;
            Settings.Host = HostTextBox.Text;
            Settings.Login = LoginTextBox.Text;
            Settings.Password = PasswordTextBox.Text;

            OldList = new List<FileInformation>();
            NewList = new List<FileInformation>();
            byte[] data = Download(PatchFileName);

            if (data != null)
            {
                using (MemoryStream stream = new MemoryStream(data))
                using (BinaryReader reader = new BinaryReader(stream))
                    ParseOld(reader);
            }

            CheckFiles();

            for (int i = 0; i < NewList.Count; i++)
            {
                FileInformation info = NewList[i];
                for (int o = 0; o < OldList.Count; o++)
                {
                    if (OldList[o].FileName != info.FileName) continue;
                    NewList[i].Compressed = OldList[o].Compressed;
                    break;
                }
            }

            Upload(new FileInformation { FileName = PatchFileName }, CreateNew());
        }

        private void SourceLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SourceLinkLabel.LinkVisited = true;
            Process.Start("http://www.lomcn.org/forum/member.php?141-Jamie-Hello");
        }

        private void AMain_Load(object sender, EventArgs e)
        {

        }

    }

    /// <summary>
    /// 文件信息类
    /// </summary>
    public class FileInformation
    {
        // 文件名、长度、压缩、创建时间
        public string FileName; //相对路径.
        public int Length, Compressed;
        public DateTime Creation;

        public FileInformation()
        {

        }
        public FileInformation(BinaryReader reader)
        {
            FileName = reader.ReadString();
            Length = reader.ReadInt32();
            Compressed = reader.ReadInt32();
            Creation = DateTime.FromBinary(reader.ReadInt64());
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(FileName);
            writer.Write(Length);
            writer.Write(Compressed);
            writer.Write(Creation.ToBinary());
        }
    }
}
