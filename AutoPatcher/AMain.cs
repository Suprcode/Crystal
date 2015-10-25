using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace AutoPatcher
{
    public partial class AMain : Form
    {
        long _totalBytes, _completedBytes, _currentBytes;
        private int _fileCount, _currentCount;

        private FileInformation _currentFile;
        public bool Completed, Checked, ErrorFound;
        
        public List<FileInformation> OldList;
        public Queue<FileInformation> DownloadList;

        private Stopwatch _stopwatch = Stopwatch.StartNew();

        private Thread _workThread;

        public AMain()
        {
            InitializeComponent();
        }



        public void Start()
        {
            OldList = new List<FileInformation>();
            DownloadList = new Queue<FileInformation>();

            byte[] data = Download(Settings.PatchFileName);

            if (data != null)
            {
                using (MemoryStream stream = new MemoryStream(data))
                using (BinaryReader reader = new BinaryReader(stream))
                    ParseOld(reader);
            }
            else
            {
                MessageBox.Show("Could not get Patch Information.");
                Completed = true;
                return;
            }

            _fileCount = OldList.Count;
            for (int i = 0; i < OldList.Count; i++)
                CheckFile(OldList[i]);

            Checked = true;
            _fileCount = 0;
            _currentCount = 0;


            _fileCount = DownloadList.Count;
            BeginDownload();
        }

        private void BeginDownload()
        {
            if (DownloadList == null) return;

            if (DownloadList.Count == 0)
            {
                DownloadList = null;
                _currentFile = null;
                Completed = true;

                CleanUp();
                return;
            }

            _currentFile = DownloadList.Dequeue();

            Download(_currentFile);
        }
        private void CleanUp()
        {
            if (!Settings.AllowCleanUp) return;

            string[] fileNames;

            if (Directory.Exists(@".\Data\"))
            {
                fileNames = Directory.GetFiles(@".\Data\", @"*.lib");

                for (int i = 0; i < fileNames.Length; i++)
                {
                    if (!NeedFile(fileNames[i]))
                        File.Delete(fileNames[i]);
                }
            }


            if (Directory.Exists(@".\Sound\"))
            {
                fileNames = Directory.GetFiles(@".\Sound\", @"*.wav");

                for (int i = 0; i < fileNames.Length; i++)
                {
                    if (!NeedFile(fileNames[i]))
                        File.Delete(fileNames[i]);
                }

                fileNames = Directory.GetFiles(@".\Sound\", @"*.mp3");

                for (int i = 0; i < fileNames.Length; i++)
                {
                    if (!NeedFile(fileNames[i]))
                        File.Delete(fileNames[i]);
                }
            }

            if (Directory.Exists(@".\Map\"))
            {
                fileNames = Directory.GetFiles(@".\Map\", @"*.map");

                for (int i = 0; i < fileNames.Length; i++)
                {
                    if (!NeedFile(fileNames[i]))
                        File.Delete(fileNames[i]);
                }
            }

        }
        public bool NeedFile(string fileName)
        {
            for (int i = 0; i < OldList.Count; i++)
            {
                if (fileName.EndsWith(OldList[i].FileName))
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

        public void CheckFile(FileInformation old)
        {
            FileInformation info = GetFileInformation(Settings.Client + (old.FileName == "AutoPatcher.gz" ? "AutoPatcher.exe" : old.FileName));
            _currentCount++;

            if (info == null || old.Length != info.Length || old.Creation != info.Creation)
            {
                DownloadList.Enqueue(old);
                _totalBytes += old.Compressed;
            }
        }

        public void Download(FileInformation info)
        {
            string fileName = info.FileName.Replace(@"\", "/");

            if (fileName != "PList.gz")
                fileName += Path.GetExtension(fileName);

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += (o, e) =>
                    {
                        _currentBytes = e.BytesReceived;
                    };
                    client.DownloadDataCompleted += (o, e) =>
                    {
                        if (e.Error != null)
                        {
                            File.AppendAllText(@".\Error.txt",
                                   string.Format("[{0}] {1}{2}", DateTime.Now, info.FileName + " could not be downloaded. (" + e.Error.Message + ")", Environment.NewLine));
                            ErrorFound = true;
                        }
                        else
                        {
                            _currentCount++;
                            _completedBytes += _currentBytes;
                            _currentBytes = 0;
                            _stopwatch.Stop();

                            if (!Directory.Exists(Settings.Client + Path.GetDirectoryName(info.FileName)))
                                Directory.CreateDirectory(Settings.Client + Path.GetDirectoryName(info.FileName));

                            File.WriteAllBytes(Settings.Client + info.FileName, Decompress(e.Result));
                            File.SetLastWriteTime(Settings.Client + info.FileName, info.Creation);
                        }
                        BeginDownload();
                    };

                    if (Settings.NeedLogin) client.Credentials = new NetworkCredential(Settings.Login, Settings.Password);


                    _stopwatch = Stopwatch.StartNew();
                    client.DownloadDataAsync(new Uri(Settings.Host + Path.ChangeExtension("/" + fileName, ".gz")));
                }
            }
            catch
            {
                MessageBox.Show(string.Format("Failed to download file: {0}", fileName));
            }
        }

        public byte[] Download(string fileName)
        {
            fileName = fileName.Replace(@"\", "/");

            if (fileName != "AutoPatcher.gz" && fileName != "PList.gz")
                fileName += Path.GetExtension(fileName);

            try
            {
                using (WebClient client = new WebClient())
                {
                    if (Settings.NeedLogin)
                        client.Credentials = new NetworkCredential(Settings.Login, Settings.Password);
                    else
                        client.Credentials = new NetworkCredential("", "");

                    return Decompress(client.DownloadData(Settings.Host + Path.ChangeExtension(fileName, ".gz")));
                }
            }
            catch
            {
                return null;
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

        public FileInformation GetFileInformation(string fileName)
        {
            if (!File.Exists(fileName)) return null;

            FileInfo info = new FileInfo(fileName);
            return new FileInformation
            {
                FileName = fileName.Remove(0, Settings.Client.Length),
                Length = (int)info.Length,
                Creation = info.LastWriteTime
            };
        }

        private void ImageLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ImageLinkLabel.LinkVisited = true;
            Process.Start("http://www.lomcn.org/forum/member.php?5330-DevilsKnight");
        }

        private void AMain_Load(object sender, EventArgs e)
        {
            PlayButton.Enabled = false;
            _workThread = new Thread(Start) { IsBackground = true };
            _workThread.Start();
        }

        private void InterfaceTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Completed)
                {
                    if (Settings.AutoStart)
                    {
                        Play();
                        return;
                    }
                    ActionLabel.Text = "Completed.";
                    SizeLabel.Text = "Completed.";
                    FileLabel.Text = "Completed.";
                    SpeedLabel.Text = "Completed.";
                    progressBar1.Value = 100;
                    progressBar2.Value = 100;
                    PlayButton.Enabled = true;
                    InterfaceTimer.Enabled = false;

                    if (ErrorFound) MessageBox.Show("One or more files failed to download, check Error.txt for details.", "Failed to Download.");
                    ErrorFound = false;
                    return;
                }

                ActionLabel.Text = !Checked ? string.Format("Checking Files... {0}/{1}", _currentCount, _fileCount) : string.Format("Downloading... {0}/{1}", _currentCount, _fileCount);
                SizeLabel.Text = string.Format("{0:#,##0} MB / {1:#,##0} MB", (_completedBytes + _currentBytes) / 1024 / 1024, _totalBytes / 1024 / 1024);

                if (_currentFile != null)
                {
                    FileLabel.Text = string.Format("{0}, ({1:#,##0} MB) / ({2:#,##0} MB)", _currentFile.FileName, _currentBytes / 1024 / 1024, _currentFile.Compressed / 1024 / 1024);
                    SpeedLabel.Text = (_currentBytes / 1024F / _stopwatch.Elapsed.TotalSeconds).ToString("#,##0.##") + "KB/s";
                    progressBar2.Value = (int)(100 * _currentBytes / _currentFile.Compressed);
                }
                progressBar1.Value = (int)(100 * (_completedBytes + _currentBytes) / _totalBytes);
            }
            catch

            {
                
            }

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Play();
        }

        public void Play()
        {
            if (File.Exists(Settings.Client + "Client.exe"))
            {
                ProcessStartInfo info = new ProcessStartInfo(Settings.Client + "Client.exe") { WorkingDirectory = Settings.Client };
                Process.Start(info);
            }
            else
                MessageBox.Show(Settings.Client + "Client.exe not found.", "Client not found.");
            Close();

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
