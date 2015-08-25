using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AutoPatcher
{
    public partial class AMain : Form
    {
        long _totalBytes, _completedBytes, _currentBytes;
        private int _fileCount, _currentCount;

        private FileInformation _currentFile;
        public bool Completed, Checked;
        
        public List<FileInformation> OldList;
        public Queue<FileInformation> DownloadList;

        private Stopwatch _stopwatch = Stopwatch.StartNew();

        private Thread _workThread;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        Config ConfigForm = new Config();
        bool configVisible = false;
        public bool Restart = false;

        public AMain()
        {
            InitializeComponent();
        }



        private void Start()
        {
            OldList = new List<FileInformation>();
            DownloadList= new Queue<FileInformation>();

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
            FileInformation info = GetFileInformation(Settings.Client + old.FileName);
            _currentCount++;

            if (info == null || old.Length != info.Length || old.Creation != info.Creation)
            {
                if (old.FileName == System.AppDomain.CurrentDomain.FriendlyName)
                {
                    File.Move(Settings.Client + System.AppDomain.CurrentDomain.FriendlyName, Settings.Client + "OldPatcher.exe");
                    Restart = true;
                }

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
                                throw e.Error;

                            _currentCount++;
                            _completedBytes += _currentBytes;
                            _currentBytes = 0;
                            _stopwatch.Stop();

                            if (!Directory.Exists(Settings.Client + Path.GetDirectoryName(info.FileName)))
                                Directory.CreateDirectory(Settings.Client + Path.GetDirectoryName(info.FileName));

                            File.WriteAllBytes(Settings.Client + info.FileName, Decompress(e.Result));
                            File.SetLastWriteTime(Settings.Client + info.FileName, info.Creation);

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

            if (fileName != "PList.gz")
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

        private void AMain_Load(object sender, EventArgs e)
        {
            ConfigForm.Show(this);
            ConfigForm.Visible = false;
            if (Settings.BrowserAddress != string.Empty) Main_browser.Url = new Uri(Settings.BrowserAddress);

            if (File.Exists(Settings.Client + "OldPatcher.exe")) File.Delete(Settings.Client + "OldPatcher.exe");
            Launch_pb.Enabled = false;
            Version_label.Text = Application.ProductVersion;
            Name_label.Text = Settings.ServerName;
            _workThread = new Thread(Start) { IsBackground = true };
            _workThread.Start();
        }

        private void Play_picturebox_Click(object sender, EventArgs e)
        {

        }

        private void pBar1_Load(object sender, EventArgs e)
        {

        }

        private void Check_button_Click(object sender, EventArgs e)
        {
            //PlayButton.Enabled = false;
            //Check_button.Enabled = false;
            _workThread = new Thread(Start) { IsBackground = true };
            _workThread.Start();
        }

        private void Launch_pb_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void Close_pb_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Movement_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Movement_panel_MouseClick(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Movement_panel_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Movement_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
               
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
                if (configVisible) ConfigForm.Location = new Point(Location.X + Config_pb.Location.X - 73, Location.Y + 35);
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Launch_pb_MouseEnter(object sender, EventArgs e)
        {
            Launch_pb.Image = Properties.Resources.Launch_Hovernew;
        }

        private void Launch_pb_MouseLeave(object sender, EventArgs e)
        {
            Launch_pb.Image = Properties.Resources.Launch_Base;
        }

        private void Close_pb_MouseEnter(object sender, EventArgs e)
        {
            Close_pb.Image = Properties.Resources.Cross_Hover1;
        }

        private void Close_pb_MouseLeave(object sender, EventArgs e)
        {
            Close_pb.Image = Properties.Resources.Cross_Base;
        }

        private void Launch_pb_MouseDown(object sender, MouseEventArgs e)
        {
            Launch_pb.Image = Properties.Resources.Launch_Pressednew;
        }

        private void Launch_pb_MouseUp(object sender, MouseEventArgs e)
        {
            Launch_pb.Image = Properties.Resources.Launch_Base;
        }

        private void Close_pb_MouseDown(object sender, MouseEventArgs e)
        {
            Close_pb.Image = Properties.Resources.Cross_Pressed1;
        }

        private void Close_pb_MouseUp(object sender, MouseEventArgs e)
        {
            Close_pb.Image = Properties.Resources.Cross_Base;
        }

        private void ProgressCurrent_pb_SizeChanged(object sender, EventArgs e)
        {
            ProgEnd_pb.Location = new Point((ProgressCurrent_pb.Location.X + ProgressCurrent_pb.Width), 490);
            if (ProgressCurrent_pb.Width == 0) ProgEnd_pb.Visible = false;
            else ProgEnd_pb.Visible = true;
        }

        private void Config_pb_MouseDown(object sender, MouseEventArgs e)
        {
            Config_pb.Image = Properties.Resources.Config_Pressed;
        }

        private void Config_pb_MouseEnter(object sender, EventArgs e)
        {
            Config_pb.Image = Properties.Resources.Config_Hover;
        }

        private void Config_pb_MouseLeave(object sender, EventArgs e)
        {
            Config_pb.Image = Properties.Resources.Config_Base;
        }

        private void Config_pb_MouseUp(object sender, MouseEventArgs e)
        {
            Config_pb.Image = Properties.Resources.Config_Base;
        }

        private void Config_pb_Click(object sender, EventArgs e)
        {
            if (configVisible == true) configVisible = false; 
            else configVisible = true;

            ConfigForm.Visible = configVisible;
            ConfigForm.Location = new Point(Location.X + Config_pb.Location.X - 181, Location.Y + 29);

        }

        private void TotalProg_pb_SizeChanged(object sender, EventArgs e)
        {
            ProgTotalEnd_pb.Location = new Point((TotalProg_pb.Location.X + TotalProg_pb.Width), 508);
            if (TotalProg_pb.Width == 0) ProgTotalEnd_pb.Visible = false;
            else ProgTotalEnd_pb.Visible = true;
        }

        private void Main_browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Main_browser.Visible = true;
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
                    ActionLabel.Text = "";
                    CurrentFile_label.Text = "Up to date."; //testing
                    SpeedLabel.Text = "";
                    ProgressCurrent_pb.Width = 550;
                    TotalProg_pb.Width = 550;
                    CurrentPercent_label.Text = "100%";
                    TotalPercent_label.Text = "100%";
                    InterfaceTimer.Enabled = false;
                    Launch_pb.Enabled = true;
                    if (Restart)
                    {
                        if (!File.Exists(Settings.Client + System.AppDomain.CurrentDomain.FriendlyName)) File.Move(Settings.Client + "OldPatcher.exe", Settings.Client + System.AppDomain.CurrentDomain.FriendlyName);
                        Application.Restart();
                    }
                    return;
                }

                //ActionLabel.Text = !Checked ? string.Format("Checking Files... {0}/{1}", _currentCount, _fileCount) : string.Format(" {0}/{1}", _currentCount, _fileCount);
                ActionLabel.Text = string.Format("{0:#,##0} MB / {1:#,##0} MB", (_completedBytes + _currentBytes) / 1024 / 1024, _totalBytes / 1024 / 1024);
                //pBar1.LabelText = string.Format("{0:#,##0} MB / {1:#,##0} MB", (_completedBytes + _currentBytes) / 1024 / 1024, _totalBytes / 1024 / 1024);

                if (_currentFile != null)
                {
                    //FileLabel.Text = string.Format("{0}, ({1:#,##0} MB) / ({2:#,##0} MB)", _currentFile.FileName, _currentBytes / 1024 / 1024, _currentFile.Compressed / 1024 / 1024);
                    //CurrentFile_label.Text = string.Format("{0}, ({1:#,##0} MB) / ({2:#,##0} MB)", _currentFile.FileName, _currentBytes / 1024 / 1024, _currentFile.Compressed / 1024 / 1024);
                    CurrentFile_label.Text = string.Format("{0}", _currentFile.FileName);
                    SpeedLabel.Text = (_currentBytes / 1024F / _stopwatch.Elapsed.TotalSeconds).ToString("#,##0.##") + "KB/s";
                    CurrentPercent_label.Text = ((int)(100 * _currentBytes / _currentFile.Compressed)).ToString() + "%";
                    ProgressCurrent_pb.Width = (int)( 5.5 * (100 * _currentBytes / _currentFile.Compressed));
                }
                TotalPercent_label.Text = ((int)(100 * (_completedBytes + _currentBytes) / _totalBytes)).ToString() + "%";
                TotalProg_pb.Width = (int)(5.5 * (100 * (_completedBytes + _currentBytes) / _totalBytes));
            }
            catch

            {
                
            }

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
