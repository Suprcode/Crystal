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
        public const string PatchFileName = @"PList.gz";

        public string[] ExcludeList = new string[] { "Thumbs.db" };

        public List<FileInformation> OldList, NewList;
        public Queue<FileInformation> UploadList;
        public Queue<FileInformation> DownloadList;

        private Stopwatch _stopwatch = Stopwatch.StartNew();

        public bool Completed;
        long _currentBytes;
        private int _fileCount, _currentCount;

        private FileInformation _currentFile;

        long _totalBytes, _completedBytes;

        public AMain()
        {
            InitializeComponent();

            ClientTextBox.Text = Settings.Client;
            HostTextBox.Text = Settings.Host;
            LoginTextBox.Text = Settings.Login;
            PasswordTextBox.Text = Settings.Password;
            AllowCleanCheckBox.Checked = Settings.AllowCleanUp;
        }

        private void CompleteDownload()
        {
            FileLabel.Text = "Complete...";
            SizeLabel.Text = "Complete...";
            SpeedLabel.Text = "Complete...";
            ActionLabel.Text = "Complete...";

            progressBar1.Value = 100;
            progressBar2.Value = 100;

            DownloadExistingButton.Enabled = true;

            Completed = true;
        }

        private void CleanUp()
        {
            if (!Settings.AllowCleanUp) return;

            for (int i = 0; i < OldList.Count; i++)
            {
                if (NeedFile(OldList[i].FileName)) continue;

                var compressed = OldList[i].Length != OldList[i].Compressed;
                try
                {
                    FtpWebRequest request = (FtpWebRequest) WebRequest.Create(new Uri(Settings.Host + OldList[i].FileName + (compressed ? ".gz" : "")));
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
        public bool NeedFile(string fileName)
        {
            for (int i = 0; i < NewList.Count; i++)
            {
                if (fileName.EndsWith(NewList[i].FileName) && !InExcludeList(NewList[i].FileName))
                    return true;
            }

            return false;
        }

        public void GetOldFileList()
        {
            OldList = new List<FileInformation>();

            byte[] data = Download(PatchFileName);

            if (data != null)
            {
                using MemoryStream stream = new MemoryStream(data);
                using BinaryReader reader = new BinaryReader(stream);

                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                    OldList.Add(new FileInformation(reader));
            }
        }

        public byte[] CreateNew()
        {
            using MemoryStream stream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(NewList.Count);
            for (int i = 0; i < NewList.Count; i++)
                NewList[i].Save(writer);

            return stream.ToArray();
        }

        public void GetNewFileList()
        {
            NewList = new List<FileInformation>();

            string[] files = Directory.GetFiles(Settings.Client, "*.*" ,SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
                NewList.Add(GetFileInformation(files[i]));
        }

        public bool InExcludeList(string fileName)
        {
            foreach (var item in ExcludeList)
            {
                if (fileName.EndsWith(item)) return true;
            }

            return false;
        }

        public void FixFilenameExtensions()
        {
            for (int i = 0; i < OldList.Count; i++)
            {
                FileInformation old = OldList[i];

                try
                {
                    ActionLabel.Text = old.FileName;
                    Refresh();

                    if (old.Compressed == old.Length)
                    {
                        //exists, but not compressed, lets rename it

                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(Settings.Host + "/" + old.FileName + ".gz"));
                        request.Credentials = new NetworkCredential(Settings.Login, Settings.Password);
                        request.Method = WebRequestMethods.Ftp.Rename;
                        request.RenameTo = Path.GetFileName(old.FileName);

                        FtpWebResponse  ftpResponse = (FtpWebResponse)request.GetResponse();
                        ftpResponse.Close();
                    }
                    else
                    {
                        //exists, but its compressed and ends with .gz, so its correct
                    }
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    response.Close();
                }
            }
        }

        public bool NeedUpdate(FileInformation info)
        {
            for (int i = 0; i < OldList.Count; i++)
            {
                FileInformation old = OldList[i];
                if (old.FileName != info.FileName) continue;

                if (old.Length != info.Length) return true;
                if (old.Creation != info.Creation) return true;

                return false;
            }
            return true;
        }

        public DateTime TrimMilliseconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
        }

        public FileInformation GetFileInformation(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            FileInfo info = new FileInfo(fileName);

            FileInformation file =  new FileInformation
            {
                FileName = fileName.Remove(0, Settings.Client.Length),
                Length = (int)info.Length,
                Creation = info.LastWriteTime
            };

            if (file.FileName == "AutoPatcher.exe")
                file.FileName = "AutoPatcher.gz";

            return file;
        }

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

        public void Download(FileInformation info)
        {
            string fileName = info.FileName.Replace(@"\", "/");

            if (fileName != "PList.gz" && (info.Compressed != info.Length || info.Compressed == 0))
            {
                fileName += ".gz";
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += (o, e) =>
                    {
                        _currentBytes = e.BytesReceived;

                        int value = (int)(100 * _currentBytes / _currentFile.Length);
                        progressBar2.Value = value > progressBar2.Maximum ? progressBar2.Maximum : value;

                        FileLabel.Text = fileName;
                        SizeLabel.Text = string.Format("{0} KB / {1} KB", _currentBytes / 1024, _currentFile.Length / 1024);
                        SpeedLabel.Text = (_currentBytes / 1024F / _stopwatch.Elapsed.TotalSeconds).ToString("#,##0.##") + "KB/s";
                    };
                    client.DownloadDataCompleted += (o, e) =>
                    {
                        if (e.Error != null)
                        {
                            File.AppendAllText(@".\Error.txt",
                                   string.Format("[{0}] {1}{2}", DateTime.Now, info.FileName + " could not be downloaded. (" + e.Error.Message + ")", Environment.NewLine));
                        }
                        else
                        {
                            _currentCount++;
                            _completedBytes += _currentBytes;
                            _currentBytes = 0;
                            _stopwatch.Stop();

                            byte[] raw = e.Result;

                            if (info.Compressed > 0 && info.Compressed != info.Length)
                            {
                                raw = Decompress(e.Result);
                            }

                            if (!Directory.Exists(Settings.Client + Path.GetDirectoryName(info.FileName)))
                            {
                                Directory.CreateDirectory(Settings.Client + Path.GetDirectoryName(info.FileName));
                            }

                            File.WriteAllBytes(Settings.Client + info.FileName, raw);
                            File.SetLastWriteTime(Settings.Client + info.FileName, info.Creation);
                        }
                        BeginDownload();
                    };

                    client.Credentials = new NetworkCredential(Settings.Login, Settings.Password);

                    progressBar1.Value = (int)(_completedBytes * 100 / _totalBytes) > 100 ? 100 : (int)(_completedBytes * 100 / _totalBytes);

                    _stopwatch = Stopwatch.StartNew();
                    client.DownloadDataAsync(new Uri(Settings.Host + fileName));
                }
            }
            catch
            {
                MessageBox.Show(string.Format("Failed to download file: {0}", fileName));
            }
        }
        private void BeginDownload()
        {
            if (DownloadList == null) return;

            if (DownloadList.Count == 0)
            {
                DownloadList = null;
                _currentFile = null;
                CompleteDownload();
                DownloadExistingButton.Enabled = true;

                CleanUp();
                return;
            }

            ActionLabel.Text = string.Format("Downloading... Files: {0}, Total Size: {1:#,##0}MB (Uncompressed)", DownloadList.Count, (_totalBytes - _completedBytes) / 1048576);

            progressBar1.Value = (int)(_completedBytes * 100 / _totalBytes) > 100 ? 100 : (int)(_completedBytes * 100 / _totalBytes);

            _currentFile = DownloadList.Dequeue();

            Download(_currentFile);
        }

        public void Upload(FileInformation info, byte[] raw, bool retry = true)
        {
            string fileName = info.FileName.Replace(@"\", "/");

            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(Settings.Login, Settings.Password);

                byte[] data = (!retry || !Settings.CompressFiles || fileName == "PList.gz" || fileName == "AutoPatcher.gz") ? raw : Compress(raw);
                info.Compressed = data.Length;

                if (fileName != "AutoPatcher.gz" && fileName != "PList.gz" && Settings.CompressFiles)
                {
                    fileName += ".gz";
                }

                client.UploadProgressChanged += (o, e) =>
                    {
                        int value = (int)(100 * e.BytesSent / e.TotalBytesToSend);
                        progressBar2.Value = value > progressBar2.Maximum ? progressBar2.Maximum : value;

                        FileLabel.Text = fileName;
                        SizeLabel.Text = string.Format("{0} KB / {1} KB", e.BytesSent / 1024, e.TotalBytesToSend  / 1024);
                        SpeedLabel.Text = ((double) e.BytesSent/1024/_stopwatch.Elapsed.TotalSeconds).ToString("0.##") + " KB/s";
                    };

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
                            FileLabel.Text = "Complete...";
                            SizeLabel.Text = "Complete...";
                            SpeedLabel.Text = "Complete...";
                            return;
                        }

                        progressBar1.Value = (int)(_completedBytes * 100 / _totalBytes) > 100 ? 100 : (int)(_completedBytes * 100 / _totalBytes);
                        BeginUpload();
                    };

                _stopwatch = Stopwatch.StartNew();

                client.UploadDataAsync(new Uri(Settings.Host + fileName), data);
            }
        }

        private void BeginUpload()
        {
            if (UploadList == null) return;

            if (UploadList.Count == 0)
            {
                CleanUp();

                Upload(new FileInformation { FileName = PatchFileName }, CreateNew());
                UploadList = null;
                ActionLabel.Text = string.Format("Complete...");
                ProcessButton.Enabled = true;
                return;
            }

            ActionLabel.Text = string.Format("Uploading... Files: {0}, Total Size: {1:#,##0}MB (Uncompressed)", UploadList.Count, (_totalBytes - _completedBytes) / 1048576);

            progressBar1.Value = (int)(_completedBytes * 100 / _totalBytes) > 100 ? 100 : (int)(_completedBytes * 100 / _totalBytes);

            FileInformation info = UploadList.Dequeue();
            Upload(info, File.ReadAllBytes(Settings.Client + (info.FileName == "AutoPatcher.gz" ? "AutoPatcher.exe" : info.FileName)));
        }

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
        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                using (GZipStream gStream = new GZipStream(mStream, CompressionMode.Compress, true))
                    gStream.Write(raw, 0, raw.Length);
                return mStream.ToArray();
            }
        }

        private void ListButton_Click(object sender, EventArgs e)
        {
            Settings.Client = ClientTextBox.Text;
            Settings.Host = HostTextBox.Text;
            Settings.Login = LoginTextBox.Text;
            Settings.Password = PasswordTextBox.Text;

            GetOldFileList();
            GetNewFileList();

            for (int i = 0; i < NewList.Count; i++)
            {
                FileInformation info = NewList[i];
                for (int o = 0; o < OldList.Count; o++)
                {
                    if (OldList[o].FileName != info.FileName) continue;

                    NewList[i].Compressed = OldList[o].Compressed;
                    break;
                }

                if (info.Compressed == 0)
                {
                    //We've uploaded a new file which is unknown to the existing PList (or no PList available). Assume this file was uploaded uncompressed and set to file length.
                    info.Compressed = info.Length;
                }
            }

            Upload(new FileInformation { FileName = PatchFileName }, CreateNew());
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessButton.Enabled = false;
                Settings.Client = ClientTextBox.Text;
                Settings.Host = HostTextBox.Text;
                Settings.Login = LoginTextBox.Text;
                Settings.Password = PasswordTextBox.Text;
                Settings.AllowCleanUp = AllowCleanCheckBox.Checked;

                UploadList = new Queue<FileInformation>();

                GetOldFileList();

                ActionLabel.Text = "Checking Files...";
                Refresh();

                GetNewFileList();

                for (int i = 0; i < NewList.Count; i++)
                {
                    FileInformation info = NewList[i];

                    if (InExcludeList(info.FileName)) continue;

                    if (NeedUpdate(info))
                    {
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

        private void btnFixGZ_Click(object sender, EventArgs e)
        {
            btnFixGZ.Enabled = false;

            Settings.Client = ClientTextBox.Text;
            Settings.Host = HostTextBox.Text;
            Settings.Login = LoginTextBox.Text;
            Settings.Password = PasswordTextBox.Text;

            GetOldFileList();
            GetNewFileList();

            for (int i = 0; i < NewList.Count; i++)
            {
                FileInformation info = NewList[i];
                for (int o = 0; o < OldList.Count; o++)
                {
                    if (OldList[o].FileName != info.FileName) continue;
                    NewList[i].Compressed = OldList[o].Length;
                    break;
                }
            }

            Upload(new FileInformation { FileName = PatchFileName }, CreateNew());

            FixFilenameExtensions();
        }

        private void DownloadExistingButton_Click(object sender, EventArgs e)
        {
            DownloadExistingButton.Enabled = false;
            Settings.Client = ClientTextBox.Text;
            Settings.Host = HostTextBox.Text;
            Settings.Login = LoginTextBox.Text;
            Settings.Password = PasswordTextBox.Text;
            Settings.AllowCleanUp = AllowCleanCheckBox.Checked;

            DownloadList = new Queue<FileInformation>();

            _totalBytes = 0;
            _currentCount = 0;
            _fileCount = 0;

            try
            {
                GetOldFileList();

                if (OldList.Count > 0)
                {
                    for (int i = 0; i < OldList.Count; i++)
                    {
                        var old = OldList[i];
                        _currentCount++;

                        FileInformation info = GetFileInformation(Settings.Client + old.FileName);

                        if (info == null || old.Length != info.Length || old.Creation != info.Creation)
                        {
                            DownloadList.Enqueue(old);
                            _totalBytes += old.Length;
                        }
                    }

                    _fileCount = DownloadList.Count;
                    BeginDownload();
                }
                else
                {
                    MessageBox.Show("No existing patch list found");
                    CompleteDownload();
                    DownloadExistingButton.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                CompleteDownload();
                DownloadExistingButton.Enabled = true;
            }
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

    public class FileInformation
    {
        public string FileName; //Relative.
        public int Length, Compressed;
        public DateTime Creation;

        public FileInformation()
        {
            Creation = DateTime.Now;
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
