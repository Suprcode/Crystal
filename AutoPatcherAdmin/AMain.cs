using System.IO.Compression;
using WinSCP;

namespace AutoPatcherAdmin
{
    public partial class AMain : Form
    {
        public const string PatchFileName = @"PList.gz";
        public const string TempUploadDirectory = "Out";
        public const string TempDownloadDirectory = "In";

        public string[] ExcludeList = new string[] { "Thumbs.db" };

        public List<FileInformation> OldList, NewList;
        public Queue<FileInformation> UploadList;

        public bool Completed;

        public AMain()
        {
            InitializeComponent();

            ClientTextBox.Text = Settings.Client;
            HostTextBox.Text = Settings.Host;
            LoginTextBox.Text = Settings.Login;
            PasswordTextBox.Text = Settings.Password;
            AllowCleanCheckBox.Checked = Settings.AllowCleanUp;
            ProtocolDropDown.SelectedIndex = ProtocolDropDown.FindString(Settings.Protocol);

            DeleteDirectory(TempDownloadDirectory);
            DeleteDirectory(TempUploadDirectory);
        }

        private void CompleteDownload()
        {
            FileLabel.Text = "Complete...";
            SpeedLabel.Text = "Complete...";
            ActionLabel.Text = "Complete...";

            progressBar1.Value = 100;
            progressBar2.Value = 100;

            DownloadExistingButton.Enabled = true;

            Completed = true;
        }

        private void CompleteUpload()
        {
            FileLabel.Text = "Complete...";
            SpeedLabel.Text = "Complete...";
            ActionLabel.Text = "Complete...";

            progressBar1.Value = 100;
            progressBar2.Value = 100;

            ProcessButton.Enabled = true;
            ListButton.Enabled = true;
            btnFixGZ.Enabled = true;

            Completed = true;
        }

        private void CleanUp()
        {
            if (!Settings.AllowCleanUp) return;

            var rootPath = (new Uri(Settings.Host)).AbsolutePath;

            using Session session = new();
            OpenSession(session);

            for (int i = 0; i < OldList.Count; i++)
            {
                if (NeedFile(OldList[i].FileName)) continue;

                var compressed = OldList[i].Length != OldList[i].Compressed;

                try
                {
                    var filename = OldList[i].FileName + (compressed ? ".gz" : "");

                    var filePath = Path.Combine(rootPath, filename).Replace(@"\", "/");

                    if (session.FileExists(filePath))
                    {
                        session.RemoveFile(filePath);
                    }
                }
                catch
                {

                }
            }
        }

        private bool NeedFile(string fileName)
        {
            for (int i = 0; i < NewList.Count; i++)
            {
                if (fileName.EndsWith(NewList[i].FileName) && !InExcludeList(NewList[i].FileName))
                    return true;
            }

            return false;
        }

        private void GetOldFileList()
        {
            OldList = new List<FileInformation>();

            byte[] data = DownloadFile(PatchFileName);

            if (data != null)
            {
                using MemoryStream stream = new MemoryStream(data);
                using BinaryReader reader = new BinaryReader(stream);

                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    OldList.Add(new FileInformation(reader));
                }
            }
        }

        private byte[] CreateNewList()
        {
            using MemoryStream stream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(NewList.Count);
            for (int i = 0; i < NewList.Count; i++)
            {
                NewList[i].Save(writer);
            }

            return stream.ToArray();
        }

        private void GetNewFileList()
        {
            NewList = new List<FileInformation>();

            string[] files = Directory.GetFiles(Settings.Client, "*.*" ,SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                NewList.Add(GetFileInformation(files[i]));
            }
        }

        private bool InExcludeList(string fileName)
        {
            foreach (var item in ExcludeList)
            {
                if (fileName.EndsWith(item)) return true;
            }

            return false;
        }

        private void FixFilenameExtensions()
        {
            var rootPath = (new Uri(Settings.Host)).AbsolutePath;

            using Session session = new Session();
            OpenSession(session);

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

                        var compressedFilename = OldList[i].FileName + ".gz";
                        var compressedFilePath = Path.Combine(rootPath, compressedFilename).Replace(@"\", "/");

                        var uncompressedFilename = OldList[i].FileName;
                        var uncompressedFilePath = Path.Combine(rootPath, compressedFilename).Replace(@"\", "/");

                        if (session.FileExists(compressedFilePath))
                        {
                            session.MoveFile(compressedFilePath, uncompressedFilename);
                        }
                    }
                    else
                    {
                        //exists, but its compressed and ends with .gz, so its correct
                    }
                }
                catch
                {

                }
            }
        }

        private bool NeedUpdate(FileInformation info)
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

        private FileInformation? GetFileInformation(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            FileInfo info = new(fileName);

            FileInformation file =  new()
            {
                FileName = fileName.Remove(0, Settings.Client.Length).TrimStart('\\'),
                Length = (int)info.Length,
                Creation = info.LastWriteTime
            };

            return file;
        }

        private void OpenSession(Session session)
        {
            if (session.Opened) return;

            Uri uri = null;

            if (!string.IsNullOrEmpty(Settings.Host))
            {
                uri = new Uri(Settings.Host);
            }

            if (!Protocol.TryParse(Settings.Protocol, true, out Protocol protocol))
            {
                protocol = Protocol.Ftp;
            }

            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = protocol,
                HostName = uri.Host,
                UserName = Settings.Login,
                Password = Settings.Password
            };

            if (sessionOptions.Protocol == Protocol.Sftp)
            {
                var fingerprint = session.ScanFingerprint(sessionOptions, "SHA-256");

                sessionOptions.SshHostKeyFingerprint = fingerprint;
            }

            session.Open(sessionOptions);
        }


        private void BeginUpload()
        {
            if (UploadList == null) return;

            progressBar1.Value = 0;
            progressBar2.Value = 0;

            int uploadCount = UploadList.Count;

            while (UploadList.Count > 0)
            {
                FileInformation info = UploadList.Dequeue();

                CreateTempUploadFiles(info, File.ReadAllBytes(Settings.Client + (info.FileName)));
            }

            CleanUp();

            CreateTempUploadFiles(new FileInformation { FileName = PatchFileName }, CreateNewList());
            UploadFiles(++uploadCount);

            UploadList = null;
        }
        private void CreateTempUploadFiles(FileInformation info, byte[] raw)
        {
            string fileName = info.FileName.Replace(@"\", "/");

            byte[] data = (!Settings.CompressFiles || fileName == "PList.gz") ? raw : Compress(raw);
            info.Compressed = data.Length;

            if (fileName != "PList.gz" && Settings.CompressFiles)
            {
                fileName += ".gz";
            }

            var sourceDir = Path.GetDirectoryName(fileName);
            var tempSourceDir = Path.Combine(TempUploadDirectory, sourceDir);

            var tempFilePath = Path.Combine(TempUploadDirectory, fileName).Replace(@"\", "/");

            if (!Directory.Exists(tempSourceDir))
            {
                Directory.CreateDirectory(tempSourceDir);
            }

            File.WriteAllBytes(tempFilePath, data);
            File.SetLastWriteTime(tempFilePath, info.Creation);
        }

        private void UploadFiles(int uploadCount = 0)
        {
            var rootPath = (new Uri(Settings.Host)).AbsolutePath;

            using Session session = new Session();

            session.FileTransferProgress += (o, e) =>
            {
                int value = (int)(e.OverallProgress * 100);
                progressBar1.Value = value > progressBar1.Maximum ? progressBar1.Maximum : value;

                int value2 = (int)(e.FileProgress * 100);
                progressBar2.Value = value2 > progressBar2.Maximum ? progressBar2.Maximum : value2;

                FileLabel.Text = e.FileName.TrimStart(TempUploadDirectory.ToCharArray()).TrimStart('\\');
                SpeedLabel.Text = ((double)e.CPS / 1024).ToString("0.##") + " KB/s";

                ActionLabel.Text = string.Format("Uploading... Files: {0}", uploadCount);
            };

            session.FileTransferred += (o, e) =>
            {
                uploadCount--;

                if (uploadCount <= 0)
                {
                    uploadCount = 0;
                    CompleteUpload();
                }
            };

            OpenSession(session);

            TransferOptions transferOptions = new TransferOptions
            {
                TransferMode = TransferMode.Binary,
                OverwriteMode = OverwriteMode.Overwrite
            };

            if (!session.FileExists(rootPath))
            {
                session.CreateDirectory(rootPath);
            }

            var result = session.PutFilesToDirectory(TempUploadDirectory, rootPath, options: transferOptions);
            result.Check();

            DeleteDirectory(TempUploadDirectory);
        }




        private byte[] DownloadFile(string fileName)
        {
            using Session session = new Session();
            OpenSession(session);

            try
            {
                if (!Directory.Exists(TempDownloadDirectory))
                {
                    Directory.CreateDirectory(TempDownloadDirectory);
                }

                TransferOptions transferOptions = new TransferOptions
                {
                    TransferMode = TransferMode.Binary,
                    OverwriteMode = OverwriteMode.Overwrite
                };

                var rootPath = (new Uri(Settings.Host)).AbsolutePath;

                var result = session.GetFiles(Path.Combine(rootPath, fileName), Path.Combine(TempDownloadDirectory, fileName), options: transferOptions);
                result.Check();

                MemoryStream ms = new MemoryStream();
                using (FileStream fs = new FileStream(Path.Combine(TempDownloadDirectory, fileName), FileMode.Open, FileAccess.Read))
                    fs.CopyTo(ms);

                DeleteDirectory(TempDownloadDirectory);

                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private void DownloadFiles()
        {
            var rootPath = (new Uri(Settings.Host)).AbsolutePath;

            using Session session = new Session();

            session.FileTransferProgress += (o, e) =>
            {
                int value = (int)(e.OverallProgress * 100);
                progressBar1.Value = value > progressBar1.Maximum ? progressBar1.Maximum : value;

                int value2 = (int)(e.FileProgress * 100);
                progressBar2.Value = value2 > progressBar2.Maximum ? progressBar2.Maximum : value2;

                FileLabel.Text = e.FileName.Replace(rootPath, "");
                SpeedLabel.Text = ((double)e.CPS / 1024).ToString("0.##") + " KB/s";

                ActionLabel.Text = "Downloading... Files";
            };

            session.FileTransferred += (o, e) =>
            {

            };

            OpenSession(session);

            TransferOptions transferOptions = new TransferOptions
            {
                TransferMode = TransferMode.Binary,
                OverwriteMode = OverwriteMode.Overwrite
            };

            if (!Directory.Exists(TempDownloadDirectory))
            {
                Directory.CreateDirectory(TempDownloadDirectory);
            }

            var result = session.GetFilesToDirectory(rootPath, TempDownloadDirectory, options: transferOptions);
            result.Check();

            CompleteDownload();
        }

        private void MoveTempDownloadedFiles()
        {
            for (int i = 0; i < OldList.Count; i++)
            {
                var info = OldList[i];
                var compressed = OldList[i].Length != OldList[i].Compressed;
                var filename = OldList[i].FileName + (compressed ? ".gz" : "");

                var currentPath = Path.Combine(TempDownloadDirectory, filename);

                var relativeDestDir = Path.GetDirectoryName(info.FileName);
                var destDir = Path.Combine(Settings.Client, relativeDestDir);
                var destFilename = Path.Combine(Settings.Client, info.FileName);

                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                if (File.Exists(destFilename))
                {
                    File.Delete(destFilename);
                }

                if (compressed)
                {
                    byte[] raw = File.ReadAllBytes(currentPath);

                    File.WriteAllBytes(destFilename, Decompress(raw));
                }
                else
                {
                    File.Move(currentPath, destFilename);
                }

                File.SetLastWriteTime(destFilename, info.Creation);
            }

            DeleteDirectory(TempDownloadDirectory);
        }




        private void ListButton_Click(object sender, EventArgs e)
        {
            try
            {
                ListButton.Enabled = false;
                Settings.Client = ClientTextBox.Text;
                Settings.Host = HostTextBox.Text;
                Settings.Login = LoginTextBox.Text;
                Settings.Password = PasswordTextBox.Text;
                Settings.Protocol = (string)ProtocolDropDown.SelectedItem;

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

                CreateTempUploadFiles(new FileInformation { FileName = PatchFileName }, CreateNewList());
                UploadFiles(1);
            }
            catch (Exception ex)
            {
                ListButton.Enabled = true;
                MessageBox.Show(ex.ToString());
                ActionLabel.Text = "Error...";
            }
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
                Settings.Protocol = (string)ProtocolDropDown.SelectedItem;

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
                ProcessButton.Enabled = true;
                MessageBox.Show(ex.ToString());
                ActionLabel.Text = "Error...";
            }
        }

        private void btnFixGZ_Click(object sender, EventArgs e)
        {
            try
            {
                btnFixGZ.Enabled = false;

                Settings.Client = ClientTextBox.Text;
                Settings.Host = HostTextBox.Text;
                Settings.Login = LoginTextBox.Text;
                Settings.Password = PasswordTextBox.Text;
                Settings.Protocol = (string)ProtocolDropDown.SelectedItem;

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

                CreateTempUploadFiles(new FileInformation { FileName = PatchFileName }, CreateNewList());
                UploadFiles(1);

                FixFilenameExtensions();
            }
            catch(Exception ex)
            {
                btnFixGZ.Enabled = true;
                MessageBox.Show(ex.ToString(), "Error");
                ActionLabel.Text = "Error...";
            }
        }

        private void DownloadExistingButton_Click(object sender, EventArgs e)
        {
            try
            {
                DownloadExistingButton.Enabled = false;
                Settings.Client = ClientTextBox.Text;
                Settings.Host = HostTextBox.Text;
                Settings.Login = LoginTextBox.Text;
                Settings.Password = PasswordTextBox.Text;
                Settings.AllowCleanUp = AllowCleanCheckBox.Checked;
                Settings.Protocol = (string)ProtocolDropDown.SelectedItem;

                GetOldFileList();
                DownloadFiles();
                MoveTempDownloadedFiles();
            }
            catch (Exception ex)
            {
                DownloadExistingButton.Enabled = true;
                MessageBox.Show(ex.ToString(), "Error");
                ActionLabel.Text = "Error...";
            }
        }

        private void AMain_Load(object sender, EventArgs e)
        {

        }


        private void DeleteDirectory(string target_dir)
        {
            if (!Directory.Exists(target_dir)) return;

            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        private byte[] Decompress(byte[] raw)
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

        private byte[] Compress(byte[] raw)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                using (GZipStream gStream = new GZipStream(mStream, CompressionMode.Compress, true))
                    gStream.Write(raw, 0, raw.Length);
                return mStream.ToArray();
            }
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
