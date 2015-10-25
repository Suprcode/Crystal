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
        
        public List<FileInformation> OldList, NewList;
        public Queue<FileInformation> UploadList;
        private Stopwatch _stopwatch = Stopwatch.StartNew();

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

                OldList = new List<FileInformation>();
                NewList = new List<FileInformation>();
                UploadList = new Queue<FileInformation>();

                byte[] data = Download(PatchFileName);

                if (data != null)
                {
                    using (MemoryStream stream = new MemoryStream(data))
                    using (BinaryReader reader = new BinaryReader(stream))
                        ParseOld(reader);
                }

                ActionLabel.Text = "Checking Files...";
                Refresh();
                
                CheckFiles();

                for (int i = 0; i < NewList.Count; i++)
                {
                    FileInformation info = NewList[i];
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

        private void BeginUpload()
        {
            if (UploadList == null ) return;

            if (UploadList.Count == 0)
            {
                CleanUp();

                Upload(new FileInformation {FileName = PatchFileName}, CreateNew());
                UploadList = null;
                ActionLabel.Text = string.Format("Complete...");
                ProcessButton.Enabled = true;
                return;
            }

            ActionLabel.Text = string.Format("Uploading... Files: {0}, Total Size: {1:#,##0}MB (Uncompressed)", UploadList.Count, (_totalBytes - _completedBytes)/1048576);

            progressBar1.Value = (int) (_completedBytes*100/_totalBytes) > 100 ? 100 : (int) (_completedBytes*100/_totalBytes);

            FileInformation info = UploadList.Dequeue();
            Upload(info, File.ReadAllBytes(Settings.Client + (info.FileName == "AutoPatcher.gz" ? "AutoPatcher.exe" : info.FileName)));
        }

        private void CleanUp()
        {
            if (!Settings.AllowCleanUp) return;

            for (int i = 0; i < OldList.Count; i++)
            {
                if (NeedFile(OldList[i].FileName)) continue;

                try
                {
                    FtpWebRequest request = (FtpWebRequest) WebRequest.Create(new Uri(Settings.Host + Path.ChangeExtension(OldList[i].FileName, ".gz")));
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
                if (fileName.EndsWith(NewList[i].FileName))
                    return true;
            }

            return false;
        }

        public void ParseOld(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                OldList.Add(new FileInformation(reader));
        }
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
        public void CheckFiles()
        {
            string[] files = Directory.GetFiles(Settings.Client, "*.*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
                NewList.Add(GetFileInformation(files[i]));
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

        public FileInformation GetFileInformation(string fileName)
        {
            FileInfo info = new FileInfo(fileName);

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


        public byte[] Download(string fileName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(Settings.Login, Settings.Password);
                    return Decompress(client.DownloadData(Settings.Host + Path.ChangeExtension(fileName, ".gz")));
                }
            }
            catch
            {
                return null;
            }
        }
        public void Upload(FileInformation info, byte[] raw, bool retry = true)
        {
            string fileName = info.FileName.Replace(@"\", "/");

            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(Settings.Login, Settings.Password);

                byte[] data = !retry ? raw : Compress(raw);
                info.Compressed = data.Length;

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


                client.UploadDataAsync(new Uri(Settings.Host  + Path.ChangeExtension(fileName, ".gz")), data);
            }
        }

        public void CheckDirectory(string directory)
        {
            try
            {

                if (string.IsNullOrEmpty(directory)) return;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Settings.Host + directory + "/");
                request.Credentials = new NetworkCredential(Settings.Login, Settings.Password);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;

                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse) request.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                if (ftpStream != null) ftpStream.Close();
                response.Close();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse) ex.Response;
                response.Close();

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

    }

    public class FileInformation
    {
        public string FileName; //Relative.
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
