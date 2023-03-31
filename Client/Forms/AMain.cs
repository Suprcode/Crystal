using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using Client;
using Microsoft.Web.WebView2.Core;
using System.Net.Http.Headers;
using System.Net.Http.Handlers;
using Client.Utils;

namespace Launcher
{
    public partial class AMain : Form
    {
        long _totalBytes, _completedBytes;
        private int _fileCount, _currentCount;

        public bool Completed, Checked, CleanFiles, LabelSwitch, ErrorFound;

        public List<FileInformation> OldList;
        public Queue<FileInformation> DownloadList = new Queue<FileInformation>();
        public List<Download> ActiveDownloads = new List<Download>();

        private Stopwatch _stopwatch = Stopwatch.StartNew();

        public Thread _workThread;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private Config ConfigForm = new Config();

        private bool Restart = false;

        public AMain()
        {
            InitializeComponent();

            BackColor = Color.FromArgb(1, 0, 0);
            TransparencyKey = Color.FromArgb(1, 0, 0);
        }

        public static void SaveError(string ex)
        {
            try
            {
                if (Settings.RemainingErrorLogs-- > 0)
                {
                    File.AppendAllText(@".\Error.txt",
                                       string.Format("[{0}] {1}{2}", DateTime.Now, ex, Environment.NewLine));
                }
            }
            catch
            {
            }
        }

        public void Start()
        {
            try
            {
                GetOldFileList();

                if (OldList.Count == 0)
                {
                    MessageBox.Show(GameLanguage.PatchErr);
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

                ServicePointManager.DefaultConnectionLimit = Settings.P_Concurrency;

                _stopwatch = Stopwatch.StartNew();
                for (var i = 0; i < Settings.P_Concurrency; i++)
                    BeginDownload();


            }
            catch (EndOfStreamException ex)
            {
                MessageBox.Show("End of stream found. Host is likely using a pre version 1.1.0.0 patch system");
                Completed = true;
                SaveError(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                Completed = true;
                SaveError(ex.ToString());
            }

            _stopwatch.Stop();
        }

        private void BeginDownload()
        {
            if (DownloadList.Count == 0)
            {
                Completed = true;

                CleanUp();
                return;
            }

            var download = new Download();
            download.Info = DownloadList.Dequeue();

            DownloadFile(download);
        }

        private void CleanUp()
        {
            if (!CleanFiles) return;

            string[] fileNames = Directory.GetFiles(@".\", "*.*", SearchOption.AllDirectories);
            string fileName;
            for (int i = 0; i < fileNames.Length; i++)
            {
                if (fileNames[i].StartsWith(".\\Screenshots\\")) continue;

                fileName = Path.GetFileName(fileNames[i]);

                if (fileName == "Mir2Config.ini" || fileName == System.AppDomain.CurrentDomain.FriendlyName) continue;

                try
                {
                    if (!NeedFile(fileNames[i]))
                        File.Delete(fileNames[i]);
                }
                catch { }
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

        private void GetOldFileList()
        {
            OldList = new List<FileInformation>();

            byte[] data = Download(Settings.P_PatchFileName);
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


        public void ParseOld(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                OldList.Add(new FileInformation(reader));
        }

        public void CheckFile(FileInformation old)
        {
            FileInformation info = GetFileInformation(Settings.P_Client + old.FileName);
            _currentCount++;

            if (info == null || old.Length != info.Length || old.Creation != info.Creation)
            {
                if (info != null && (Path.GetExtension(old.FileName).ToLower() == ".dll" || Path.GetExtension(old.FileName).ToLower() == ".exe"))
                {
                    string oldFilename = Path.Combine(Path.GetDirectoryName(old.FileName), ("Old__" + Path.GetFileName(old.FileName)));

                    try
                    {
                        File.Move(Settings.P_Client + old.FileName, oldFilename);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        SaveError(ex.ToString());
                    }
                    finally
                    {
                        //Might cause an infinite loop if it can never gain access
                        Restart = true;
                    }
                }

                DownloadList.Enqueue(old);
                _totalBytes += old.Length;
            }
        }

        public void DownloadFile(Download dl)
        {
            var info = dl.Info;
            string fileName = info.FileName.Replace(@"\", "/");

            if (fileName != "PList.gz" && (info.Compressed != info.Length || info.Compressed == 0))
            {
                fileName += ".gz";
            }

            try
            {
                HttpClientHandler httpClientHandler = new() { AllowAutoRedirect = true };
                ProgressMessageHandler progressMessageHandler = new(httpClientHandler);

                progressMessageHandler.HttpReceiveProgress += (_, args) =>
                {

                    dl.CurrentBytes = args.BytesTransferred;

                };

                using (HttpClient client = new(progressMessageHandler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.AcceptCharset.Clear();
                    client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));

                    if (Settings.P_NeedLogin)
                    {
                        string authInfo = Settings.P_Login + ":" + Settings.P_Password;
                        authInfo = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(authInfo));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
                    }

                    ActiveDownloads.Add(dl);

                    var task = Task.Run(() => client
                                    .GetAsync(new Uri($"{Settings.P_Host}{fileName}"), HttpCompletionOption.ResponseHeadersRead));

                    var response = task.Result;

                    using Stream sm = response.Content.ReadAsStream();
                    using MemoryStream ms = new();
                    sm.CopyTo(ms);
                    byte[] data = ms.ToArray();

                    _currentCount++;
                    _completedBytes += dl.CurrentBytes;
                    dl.CurrentBytes = 0;
                    dl.Completed = true;

                    if (info.Compressed > 0 && info.Compressed != info.Length)
                    {
                        data = Decompress(data);
                    }

                    var fileNameOut = Settings.P_Client + info.FileName;
                    var dirName = Path.GetDirectoryName(fileNameOut);
                    if (!Directory.Exists(dirName))
                        Directory.CreateDirectory(dirName);

                    File.WriteAllBytes(fileNameOut, data);
                    File.SetLastWriteTime(fileNameOut, info.Creation);
                }
            }
            catch (HttpRequestException e)
            {
                File.AppendAllText(@".\Error.txt",
                                       $"[{DateTime.Now}] {info.FileName} could not be downloaded. ({e.Message}) {Environment.NewLine}");
                ErrorFound = true;
            }
            finally
            {
                if (ErrorFound)
                {
                    MessageBox.Show(string.Format("Failed to download file: {0}", fileName));
                }
            }

            BeginDownload();
        }

        public byte[] Download(string fileName)
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptCharset.Clear();
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));

                if (Settings.P_NeedLogin)
                {
                    string authInfo = Settings.P_Login + ":" + Settings.P_Password;
                    authInfo = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(authInfo));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
                }

                var task = Task.Run(() => client
                    .GetAsync(new Uri(Settings.P_Host + Path.ChangeExtension(fileName, ".gz")), HttpCompletionOption.ResponseHeadersRead));

                var response = task.Result;

                using Stream sm = response.Content.ReadAsStream();
                using MemoryStream ms = new();
                sm.CopyTo(ms);
                byte[] data = ms.ToArray();
                return data;
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
                FileName = fileName.Remove(0, Settings.P_Client.Length),
                Length = (int)info.Length,
                Creation = info.LastWriteTime
            };
        }

        private void AMain_Load(object sender, EventArgs e)
        {
            var envir = CoreWebView2Environment.CreateAsync(null, Settings.ResourcePath).Result;
            Main_browser.EnsureCoreWebView2Async(envir);

            if (Settings.P_BrowserAddress != "")
            {
                Main_browser.NavigationCompleted += Main_browser_NavigationCompleted;
                Main_browser.Source = new Uri(Settings.P_BrowserAddress);
            }

            RepairOldFiles();

            Launch_pb.Enabled = false;
            ProgressCurrent_pb.Width = 5;
            TotalProg_pb.Width = 5;
            Version_label.Text = string.Format("Build: {0}.{1}.{2}", Globals.ProductCodename, Settings.UseTestConfig ? "Debug" : "Release", Application.ProductVersion);

            if (Settings.P_ServerName != String.Empty)
            {
                Name_label.Visible = true;
                Name_label.Text = Settings.P_ServerName;
            }

            _workThread = new Thread(Start) { IsBackground = true };
            _workThread.Start();
        }

        private void Main_browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (Main_browser.Source.AbsolutePath != "blank") Main_browser.Visible = true;
        }

        private void Launch_pb_Click(object sender, EventArgs e)
        {
            Launch();
        }

        private void Launch()
        {
            if (ConfigForm.Visible) ConfigForm.Visible = false;
            Program.Form = new CMain();
            Program.Form.Closed += (s, args) => this.Close();
            Program.Form.Show();
            Program.PForm.Hide();
        }

        private void Close_pb_Click(object sender, EventArgs e)
        {
            if (ConfigForm.Visible) ConfigForm.Visible = false;
            Close();
        }

        private void Movement_panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (ConfigForm.Visible) ConfigForm.Visible = false;
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
            }
        }

        private void Launch_pb_MouseEnter(object sender, EventArgs e)
        {
            Launch_pb.Image = Client.Resources.Images.Launch_Hover;
        }

        private void Launch_pb_MouseLeave(object sender, EventArgs e)
        {
            Launch_pb.Image = Client.Resources.Images.Launch_Base1;
        }

        private void Close_pb_MouseEnter(object sender, EventArgs e)
        {
            Close_pb.Image = Client.Resources.Images.Cross_Hover;
        }

        private void Close_pb_MouseLeave(object sender, EventArgs e)
        {
            Close_pb.Image = Client.Resources.Images.Cross_Base;
        }

        private void Launch_pb_MouseDown(object sender, MouseEventArgs e)
        {
            Launch_pb.Image = Client.Resources.Images.Launch_Pressed;
        }

        private void Launch_pb_MouseUp(object sender, MouseEventArgs e)
        {
            Launch_pb.Image = Client.Resources.Images.Launch_Base1;
        }

        private void Close_pb_MouseDown(object sender, MouseEventArgs e)
        {
            Close_pb.Image = Client.Resources.Images.Cross_Pressed;
        }

        private void Close_pb_MouseUp(object sender, MouseEventArgs e)
        {
            Close_pb.Image = Client.Resources.Images.Cross_Base;
        }

        private void ProgressCurrent_pb_SizeChanged(object sender, EventArgs e)
        {
            ProgEnd_pb.Location = new Point((ProgressCurrent_pb.Location.X + ProgressCurrent_pb.Width), ProgressCurrent_pb.Location.Y);
            if (ProgressCurrent_pb.Width == 0) ProgEnd_pb.Visible = false;
            else ProgEnd_pb.Visible = true;
        }

        private void Config_pb_MouseDown(object sender, MouseEventArgs e)
        {
            Config_pb.Image = Client.Resources.Images.Config_Pressed;
        }

        private void Config_pb_MouseEnter(object sender, EventArgs e)
        {
            Config_pb.Image = Client.Resources.Images.Config_Hover;
        }

        private void Config_pb_MouseLeave(object sender, EventArgs e)
        {
            Config_pb.Image = Client.Resources.Images.Config_Base;
        }

        private void Config_pb_MouseUp(object sender, MouseEventArgs e)
        {
            Config_pb.Image = Client.Resources.Images.Config_Base;
        }

        private void Config_pb_Click(object sender, EventArgs e)
        {
            if (ConfigForm.Visible) ConfigForm.Hide();
            else ConfigForm.Show(Program.PForm);
            ConfigForm.Location = new Point(Location.X + Config_pb.Location.X - 183, Location.Y + 36);
        }

        private void TotalProg_pb_SizeChanged(object sender, EventArgs e)
        {
            ProgTotalEnd_pb.Location = new Point((TotalProg_pb.Location.X + TotalProg_pb.Width), TotalProg_pb.Location.Y);
            if (TotalProg_pb.Width == 0) ProgTotalEnd_pb.Visible = false;
            else ProgTotalEnd_pb.Visible = true;
        }

        private void InterfaceTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Completed)
                {

                    ActionLabel.Text = "";
                    CurrentFile_label.Text = "Up to date.";
                    SpeedLabel.Text = "";
                    ProgressCurrent_pb.Width = 550;
                    TotalProg_pb.Width = 550;
                    CurrentFile_label.Visible = true;
                    CurrentPercent_label.Visible = true;
                    TotalPercent_label.Visible = true;
                    CurrentPercent_label.Text = "100%";
                    TotalPercent_label.Text = "100%";
                    InterfaceTimer.Enabled = false;
                    Launch_pb.Enabled = true;
                    if (ErrorFound) MessageBox.Show("One or more files failed to download, check Error.txt for details.", "Failed to Download.");
                    ErrorFound = false;

                    if (CleanFiles)
                    {
                        CleanFiles = false;
                        MessageBox.Show("Your files have been cleaned up.", "Clean Files");
                    }

                    if (Restart)
                    {
                        Program.Restart = true;

                        MoveOldFilesToCurrent();

                        Close();
                    }

                    if (Settings.P_AutoStart)
                    {
                        Launch();
                    }
                    return;
                }

                var currentBytes = 0L;
                FileInformation currentFile = null;

                // Remove completed downloads..
                for (var i = ActiveDownloads.Count - 1; i >= 0; i--)
                {
                    var dl = ActiveDownloads[i];

                    if (dl.Completed)
                    {
                        ActiveDownloads.RemoveAt(i);
                        continue;
                    }
                }

                for (var i = ActiveDownloads.Count - 1; i >= 0; i--)
                {
                    var dl = ActiveDownloads[i];
                    if (!dl.Completed)
                        currentBytes += dl.CurrentBytes;
                }

                if (Settings.P_Concurrency == 1)
                {
                    // Note: Just mimic old behaviour for now until a better UI is done.
                    if (ActiveDownloads.Count > 0)
                        currentFile = ActiveDownloads[0].Info;
                }

                ActionLabel.Visible = true;
                SpeedLabel.Visible = true;
                CurrentFile_label.Visible = true;
                CurrentPercent_label.Visible = true;
                TotalPercent_label.Visible = true;

                if (LabelSwitch) ActionLabel.Text = string.Format("{0} Files Remaining", _fileCount - _currentCount);
                else ActionLabel.Text = string.Format("{0:#,##0}MB Remaining", ((_totalBytes) - (_completedBytes + currentBytes)) / 1024 / 1024);

                if (Settings.P_Concurrency > 1)
                {
                    CurrentFile_label.Text = string.Format("<Concurrent> {0}", ActiveDownloads.Count);
                    SpeedLabel.Text = ToSize(currentBytes / _stopwatch.Elapsed.TotalSeconds);
                }
                else
                {
                    if (currentFile != null)
                    {
                        CurrentFile_label.Text = string.Format("{0}", currentFile.FileName);
                        SpeedLabel.Text = ToSize(currentBytes / _stopwatch.Elapsed.TotalSeconds);
                        CurrentPercent_label.Text = ((int)(100 * currentBytes / currentFile.Length)).ToString() + "%";
                        ProgressCurrent_pb.Width = (int)(5.5 * (100 * currentBytes / currentFile.Length));
                    }
                }

                if (!(_completedBytes is 0 && currentBytes is 0 && _totalBytes is 0))
                {
                    TotalProg_pb.Width = (int)(5.5 * (100 * (_completedBytes + currentBytes) / _totalBytes));
                    TotalPercent_label.Text = ((int)(100 * (_completedBytes + currentBytes) / _totalBytes)).ToString() + "%";
                }

            }
            catch
            {
                //to-do 
            }

        }

        private void AMain_Click(object sender, EventArgs e)
        {
            if (ConfigForm.Visible) ConfigForm.Visible = false;
        }

        private void ActionLabel_Click(object sender, EventArgs e)
        {
            LabelSwitch = !LabelSwitch;
        }

        private void Credit_label_Click(object sender, EventArgs e)
        {
            if (Credit_label.Text == "Powered by Crystal M2") Credit_label.Text = "Designed by Breezer";
            else Credit_label.Text = "Powered by Crystal M2";
        }

        private void AMain_FormClosed(object sender, FormClosedEventArgs e)
        {
                MoveOldFilesToCurrent();

                Launch_pb?.Dispose();
                Close_pb?.Dispose();
                Environment.Exit(0);
        }

        private static string[] suffixes = new[] { " B", " KB", " MB", " GB", " TB", " PB" };

        private string ToSize(double number, int precision = 2)
        {
            // unit's number of bytes
            const double unit = 1024;
            // suffix counter
            int i = 0;
            // as long as we're bigger than a unit, keep going
            while (number > unit)
            {
                number /= unit;
                i++;
            }
            // apply precision and current suffix
            return Math.Round(number, precision) + suffixes[i];
        }

        private void RepairOldFiles()
        {
            var files = Directory.GetFiles(Settings.P_Client, "*", SearchOption.AllDirectories).Where(x => Path.GetFileName(x).StartsWith("Old__"));

            foreach (var oldFilename in files)
            {
                if (!File.Exists(oldFilename.Replace("Old__", "")))
                {
                    File.Move(oldFilename, oldFilename.Replace("Old__", ""));
                }
                else
                {
                    File.Delete(oldFilename);
                }
            }
        }

        private void MoveOldFilesToCurrent()
        {
            var files = Directory.GetFiles(Settings.P_Client, "*", SearchOption.AllDirectories).Where(x => Path.GetFileName(x).StartsWith("Old__"));

            foreach (var oldFilename in files)
            {
                string originalFilename = Path.Combine(Path.GetDirectoryName(oldFilename), (Path.GetFileName(oldFilename).Replace("Old__", "")));

                if (!File.Exists(originalFilename) && File.Exists(oldFilename))
                    File.Move(oldFilename, originalFilename);
            }
        }
    } 
}
