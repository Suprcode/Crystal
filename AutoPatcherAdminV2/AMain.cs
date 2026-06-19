using System.IO.Compression;
using System.Net;
using System.Net.Sockets;

namespace AutoPatcherAdmin
{
    public partial class AMain : Form
    {
        public const string PatchFileName = @"PList.gz";
        public const string StagingDirectoryName = "_staging";
        public const string TempUploadDirectory = "Out";
        public const string TempDownloadDirectory = "In";
        private const int MaxRetries = 3;
        private const int RetryDelaySeconds = 30;

        public string[] ExcludeList = new string[] { "Thumbs.db" };

        public List<FileInformation> OldList, NewList;
        public Queue<FileInformation> UploadList;
        private PatchCompareResult _compareResult;
        private bool _publishPreviewReady;
        private byte[]? _remotePListRawBytes;
        private readonly List<PreviewRowInfo> _previewRows = new();
        private readonly HashSet<string> _activeRowKeys = new(StringComparer.OrdinalIgnoreCase);
        private DateTime _lastPublishStart;
        private long _lastPublishOriginalBytes;
        private long _lastPublishCompressedBytes;
        private int _lastPublishFileCount;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private DgvProgressBarColumn? _progressColumn;
        private int _totalPublishFiles;
        private int _overallSteps;
        private int _liveConnections;
        private int _connectionCeiling;
        private ToolStripStatusLabel? _statusConnectionLabel;
        private readonly object _connectionCreateLock = new();

#if DEBUG
        private static readonly string LogFile = Path.Combine(AppContext.BaseDirectory, "patcher.log");
#endif

        private sealed class PreviewRowInfo
        {
            public string Action { get; init; } = string.Empty;
            public string Path { get; init; } = string.Empty;
            public long Size { get; init; }
        }

        public AMain()
        {
            InitializeComponent();

            ClientTextBox.Text = Settings.Client;
            HostTextBox.Text = Settings.Host;
            LoginTextBox.Text = Settings.Login;
            PasswordTextBox.Text = Settings.Password;
            AllowCleanCheckBox.Checked = Settings.AllowCleanUp;
            CompressFilesCheckBox.Checked = Settings.CompressFiles;
            ProtocolDropDown.SelectedIndex = ProtocolDropDown.FindString(Settings.Protocol);
            PortNumericUpDown.Value = Math.Clamp(Settings.Port, 0, 65535);
            PreviewActionFilterDropDown.SelectedIndex = 0;

            DeleteDirectory(TempDownloadDirectory);
            DeleteDirectory(TempUploadDirectory);
            UpdateSummaryLabels(null);
            ClearPreviewGrid();
            ProcessButton.Enabled = false;

            _progressColumn = new DgvProgressBarColumn();
            PreviewGrid.Columns.Add(_progressColumn);
            PreviewGrid.Sort(_progressColumn, System.ComponentModel.ListSortDirection.Descending);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cts.Cancel();
            DeleteDirectory(TempDownloadDirectory);
            DeleteDirectory(TempUploadDirectory);
            base.OnFormClosing(e);
        }

        private void SaveConnectionSettings()
        {
            Settings.Client = ClientTextBox.Text;
            Settings.Protocol = ProtocolDropDown.SelectedItem as string ?? "Ftp";
            Settings.Host = NormalizeHost(HostTextBox.Text.Trim(), Settings.Protocol);
            HostTextBox.Text = Settings.Host;
            Settings.Login = LoginTextBox.Text;
            Settings.Password = PasswordTextBox.Text;
            Settings.AllowCleanUp = AllowCleanCheckBox.Checked;
            Settings.CompressFiles = CompressFilesCheckBox.Checked;
            Settings.Port = (int)PortNumericUpDown.Value;
            Settings.Save();
        }

        private static string NormalizeHost(string host, string protocol)
        {
            if (string.IsNullOrWhiteSpace(host) || host.Contains("://")) return host;
            string scheme = (protocol ?? "Ftp").ToLowerInvariant() switch
            {
                "sftp"  => "sftp://",
                "http"  => "http://",
                "https" => "https://",
                _       => "ftp://"
            };
            return scheme + host;
        }

        private void BrowseClientButton_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Select client directory";
            string current = ClientTextBox.Text.TrimEnd('\\', '/');
            if (Directory.Exists(current))
                dialog.SelectedPath = current;
            if (dialog.ShowDialog() == DialogResult.OK)
                ClientTextBox.Text = dialog.SelectedPath + @"\";
        }

        private void OnUi(Action action)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private void PostUi(Action action)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(action);
                }
                catch (InvalidOperationException)
                {
                }
            }
            else
            {
                action();
            }
        }

        private T OnUi<T>(Func<T> action)
        {
            if (InvokeRequired)
                return (T)Invoke(action);

            return action();
        }

        private void SetActionText(string text)
        {
            PostUi(() => ActionLabel.Text = text);
        }

        private void SetFileText(string text)
        {
            PostUi(() => FileLabel.Text = text);
        }

        private void ShowError(Exception ex)
        {
            OnUi(() =>
            {
                MessageBox.Show(this, GetFriendlyError(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ActionLabel.Text = "Error.";
            });
        }

        private static string GetFriendlyError(Exception ex)
        {
            if (ex is SocketException se)
                return $"Network error: {se.Message}\nCheck host address and that the server is accessible.";
            if (ex is IOException && ex.InnerException is SocketException se2)
                return $"Network error: {se2.Message}\nCheck host address and that the server is accessible.";
            if (ex is WebException we)
            {
                return we.Status switch
                {
                    WebExceptionStatus.ConnectFailure        => "Could not connect to the server. Check the host address and that the server is running.",
                    WebExceptionStatus.Timeout               => "Connection timed out. The server may be busy or unreachable.",
                    WebExceptionStatus.NameResolutionFailure => "Host name could not be resolved. Check the host address.",
                    WebExceptionStatus.ConnectionClosed      => "The server closed the connection unexpectedly.",
                    WebExceptionStatus.ProtocolError         => $"Server returned an error: {we.Message}",
                    _                                        => $"Network error: {we.Message}"
                };
            }
            if (ex is DirectoryNotFoundException)
                return $"Directory not found:\n{ex.Message}";
            if (ex is FileNotFoundException fnf)
                return $"File not found:\n{fnf.FileName ?? ex.Message}";
            if (ex is UnauthorizedAccessException)
                return $"Access denied. Check server permissions.\n{ex.Message}";
            string typeName = ex.GetType().FullName ?? string.Empty;
            if (typeName.Contains("SshAuthenticationException"))
                return "Authentication failed. Check the username and password.";
            if (typeName.Contains("SshConnectionException"))
                return "SSH connection failed. Check the host address and that the SFTP server is running.";
            if (ex.InnerException != null)
                return GetFriendlyError(ex.InnerException);
            return ex.Message;
        }

        private static bool IsDisconnectException(Exception ex)
        {
            if (ex is SocketException) return true;
            if (ex is IOException && ex.InnerException is SocketException) return true;
            if (ex is TimeoutException) return true;
            if (ex is WebException we && we.Status is
                WebExceptionStatus.ConnectionClosed or
                WebExceptionStatus.ConnectFailure or
                WebExceptionStatus.ReceiveFailure or
                WebExceptionStatus.SendFailure or
                WebExceptionStatus.Timeout) return true;
            string typeName = ex.GetType().FullName ?? string.Empty;
            if (typeName.Contains("SshConnectionException") ||
                typeName.Contains("SshOperationTimeoutException")) return true;
            return ex.InnerException != null && IsDisconnectException(ex.InnerException);
        }

        private static bool IsConnectionLimitException(Exception ex)
        {
            string msg = ex.Message;
            if (msg.Contains("421") ||
                msg.Contains("too many", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("connection limit", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("max sessions", StringComparison.OrdinalIgnoreCase))
                return true;
            string typeName = ex.GetType().FullName ?? string.Empty;
            if (typeName.Contains("ChannelOpen") || typeName.Contains("TooMany"))
                return true;
            return ex.InnerException != null && IsConnectionLimitException(ex.InnerException);
        }

        private void RetryOnDisconnect(Action operation, string phase)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    operation();
                    return;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex) when (IsDisconnectException(ex) && attempt < MaxRetries)
                {
                    attempt++;
                    Log($"Disconnect during {phase} (attempt {attempt}/{MaxRetries}): {ex.Message}");
                    for (int remaining = RetryDelaySeconds; remaining > 0; remaining--)
                    {
                        CheckCancelled();
                        SetActionText($"{phase} — connection lost, retrying in {remaining}s ({attempt}/{MaxRetries})...");
                        SetFileText(string.Empty);
                        Thread.Sleep(1000);
                    }
                    SetActionText($"Reconnecting ({attempt}/{MaxRetries})...");
                }
            }
        }

        private bool ValidateSettings()
        {
            if (!Directory.Exists(Settings.Client))
            {
                string msg = $"Client directory not found:\n{Settings.Client}\n\nUpdate the Client Directory setting and try again.";
                OnUi(() => MessageBox.Show(this, msg, "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error));
                SetActionText("Error: client directory not found.");
                return false;
            }
            if (!Uri.TryCreate(Settings.Host, UriKind.Absolute, out _))
            {
                string msg = $"Host address is not a valid URL:\n{Settings.Host}\n\nCheck the Host Address setting.";
                OnUi(() => MessageBox.Show(this, msg, "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error));
                SetActionText("Error: invalid host address.");
                return false;
            }
            return true;
        }

        private void CheckCancelled()
        {
            if (_cts.IsCancellationRequested)
                throw new OperationCanceledException("Operation cancelled.");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        private static void Log(string message)
        {
#if DEBUG
            try
            {
                var logFile1 = LogFile + ".1";
                if (File.Exists(LogFile) && new FileInfo(LogFile).Length > 1_048_576)
                {
                    if (File.Exists(logFile1)) File.Delete(logFile1);
                    File.Move(LogFile, logFile1);
                }
                File.AppendAllText(LogFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\r\n");
            }
            catch { }
#endif
        }

        private void CompletePListOperation(string status)
        {
            SetActionText(status);
            SetFileText(string.Empty);
        }

        private void CompleteDownload()
        {
            OnUi(() =>
            {
                FileLabel.Text = "Complete...";
                SpeedLabel.Text = "Complete...";
                ActionLabel.Text = "Complete...";
            });
        }

        private void CompleteUpload()
        {
            _publishPreviewReady = false;
            ShowPublishCompleteAndReset();
        }

        private void ShowPublishCompleteAndReset()
        {
            var elapsed = DateTime.Now - _lastPublishStart;
            string timeStr = elapsed.TotalMinutes >= 1
                ? $"{(int)elapsed.TotalMinutes}m {elapsed.Seconds}s"
                : $"{elapsed.Seconds}s";

            var result = _compareResult;
            int added    = result?.Added.Count    ?? 0;
            int changed  = result?.Changed.Count  ?? 0;
            int deleted  = result?.Deleted.Count  ?? 0;
            long origBytes = _lastPublishOriginalBytes;
            long compBytes = _lastPublishCompressedBytes;
            int fileCount  = _lastPublishFileCount;

            Log($"Publish complete — {fileCount} files uploaded ({added} added, {changed} changed, {deleted} removed), time: {timeStr}");

            OnUi(() =>
            {
                using var dlg = new PublishCompleteDialog(
                    filesUploaded:   fileCount,
                    added:           added,
                    changed:         changed,
                    removed:         Settings.AllowCleanUp ? deleted : 0,
                    origBytes:       origBytes,
                    compBytes:       compBytes,
                    showCompression: Settings.CompressFiles && origBytes > 0,
                    timeElapsed:     timeStr);
                dlg.ShowDialog(this);

                ActionLabel.Text = "Publish complete.";
                FileLabel.Text = string.Empty;
                SpeedLabel.Text = string.Empty;
                _compareResult = null;
                UpdateSummaryLabels(null);
                ClearPreviewGrid();
            });
        }

        private void CleanUp()
        {
            if (!Settings.AllowCleanUp) return;

            var rootPath = (new Uri(Settings.Host)).AbsolutePath;
            using IPatchTransport transport = PatchTransportFactory.Create();

            var stale = OldList.Where(f => !NeedFile(f.FileName)).ToList();
            int total = stale.Count;
            int done = 0;
            foreach (var entry in stale)
            {
                CheckCancelled();
                done++;
                var isCompressed = entry.Length != entry.Compressed;
                var filename = entry.FileName + (isCompressed ? ".gz" : "");
                SetActionText($"Removing old file {done} of {total}...");
                SetFileText(filename);
                try
                {
                    var filePath = Path.Combine(rootPath, filename).Replace(@"\", "/");
                    if (transport.FileExists(filePath))
                        transport.DeleteFile(filePath);
                }
                catch { }
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

            byte[]? data = DownloadFile(PatchFileName);
            _remotePListRawBytes = data;

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

        private static string CombineRemotePath(string rootPath, string fileName)
        {
            rootPath = (rootPath ?? string.Empty).Replace('\\', '/').TrimEnd('/');
            fileName = (fileName ?? string.Empty).Replace('\\', '/').TrimStart('/');

            return string.IsNullOrEmpty(rootPath) ? "/" + fileName : rootPath + "/" + fileName;
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
            fileName = (fileName ?? string.Empty).Replace('\\', '/');

            foreach (var item in ExcludeList)
            {
                if (fileName.EndsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        }

        private FileInformation FindOldFile(string fileName)
        {
            return OldList?.FirstOrDefault(x => string.Equals(x.FileName.Replace('\\', '/'), fileName.Replace('\\', '/'), StringComparison.OrdinalIgnoreCase));
        }

        private PatchCompareResult BuildPListCompareResult()
        {
            var result = new PatchCompareResult();
            var oldByName = (OldList ?? Enumerable.Empty<FileInformation>())
                .ToDictionary(x => x.FileName.Replace('\\', '/'), StringComparer.OrdinalIgnoreCase);

            foreach (var info in NewList ?? Enumerable.Empty<FileInformation>())
            {
                if (InExcludeList(info.FileName)) continue;
                string path = info.FileName.Replace('\\', '/');
                var patchFile = new PListFile { Path = path, Length = info.Length };

                if (!oldByName.TryGetValue(path, out var old))
                {
                    result.Added.Add(patchFile);
                }
                else if (old.Length != info.Length || old.Creation != info.Creation)
                {
                    result.Changed.Add(patchFile);
                }
                else
                {
                    result.Unchanged.Add(patchFile);
                }
            }

            var localPaths = new HashSet<string>(
                (NewList ?? Enumerable.Empty<FileInformation>())
                    .Where(f => !InExcludeList(f.FileName))
                    .Select(f => f.FileName.Replace('\\', '/')),
                StringComparer.OrdinalIgnoreCase);
            foreach (var old in OldList ?? Enumerable.Empty<FileInformation>())
            {
                if (!localPaths.Contains(old.FileName.Replace('\\', '/')))
                    result.Deleted.Add(new PListFile { Path = old.FileName.Replace('\\', '/'), Length = old.Length });
            }

            return result;
        }

        private void UpdateSummaryLabels(PatchCompareResult? result)
        {
            int added = result?.Added.Count ?? 0;
            int changed = result?.Changed.Count ?? 0;
            int unchanged = result?.Unchanged.Count ?? 0;
            int deleted = result?.Deleted.Count ?? 0;
            int uploadCount = result?.UploadCount ?? 0;
            long uploadBytes = result?.UploadBytes ?? 0;

            OnUi(() =>
            {
                SummaryAddedLabel.Text = $"Added: {added}";
                SummaryChangedLabel.Text = $"Changed: {changed}";
                SummaryUnchangedLabel.Text = $"Unchanged: {unchanged}";
                SummaryDeletedLabel.Text = $"Deleted: {deleted}";
                string sizeNote = Settings.CompressFiles && uploadCount > 0 ? " (uncompressed)" : string.Empty;
                SummaryUploadSizeLabel.Text = $"Upload: {uploadCount} files / {FormatBytes(uploadBytes)}{sizeNote}";
            });
        }

        private void ClearPreviewGrid()
        {
            _totalPublishFiles = 0;
            _overallSteps = 0;
            OnUi(() =>
            {
                _previewRows.Clear();
                _activeRowKeys.Clear();
                PreviewGrid.Rows.Clear();
                ProcessButton.Enabled = false;
                OverallProgressBar.Value = 0;
            });
            _publishPreviewReady = false;
        }

        private void PopulateUploadQueue(IReadOnlyList<FileInformation> files)
        {
            OnUi(() =>
            {
                _previewRows.Clear();
                PreviewGrid.Rows.Clear();
                foreach (var file in files)
                {
                    string normalizedPath = file.FileName.Replace('\\', '/');
                    int rowIndex = PreviewGrid.Rows.Add("Pending", normalizedPath, FormatBytes(file.Length), 0);
                    var row = PreviewGrid.Rows[rowIndex];
                    row.Tag = normalizedPath;
                    row.DefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(21, 67, 120);
                }
            });
        }

        private void UpdateRowStatus(string key, string statusText, Color backColor, Color foreColor)
        {
            PostUi(() =>
            {
                _activeRowKeys.Remove(key);
                int activeCount = _activeRowKeys.Count;
                int colIndex = _progressColumn?.Index ?? -1;

                for (int i = 0; i < PreviewGrid.Rows.Count; i++)
                {
                    var row = PreviewGrid.Rows[i];
                    if (!string.Equals(row.Tag as string, key, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (i < activeCount)
                    {
                        // Row is inside the active zone — relocate it to just below active rows
                        string path = row.Cells[1].Value as string ?? key;
                        string size = row.Cells[2].Value as string ?? string.Empty;
                        int progress = colIndex >= 0 ? Convert.ToInt32(row.Cells[colIndex].Value ?? 0) : 0;
                        PreviewGrid.Rows.RemoveAt(i);
                        int insertAt = Math.Min(activeCount, PreviewGrid.Rows.Count);
                        PreviewGrid.Rows.Insert(insertAt, statusText, path, size, progress);
                        var newRow = PreviewGrid.Rows[insertAt];
                        newRow.Tag = key;
                        newRow.DefaultCellStyle.BackColor = backColor;
                        newRow.DefaultCellStyle.ForeColor = foreColor;
                    }
                    else
                    {
                        // Already below the active zone — update in place
                        row.Cells[0].Value = statusText;
                        row.DefaultCellStyle.BackColor = backColor;
                        row.DefaultCellStyle.ForeColor = foreColor;
                    }
                    break;
                }
            });
        }

        private void MoveRowToTop(string key, string statusText, Color backColor, Color foreColor)
        {
            _activeRowKeys.Add(key);
            int colIndex = _progressColumn?.Index ?? -1;
            for (int i = 0; i < PreviewGrid.Rows.Count; i++)
            {
                var row = PreviewGrid.Rows[i];
                if (!string.Equals(row.Tag as string, key, StringComparison.OrdinalIgnoreCase))
                    continue;
                string path = row.Cells[1].Value as string ?? key;
                string size = row.Cells[2].Value as string ?? string.Empty;
                int progress = colIndex >= 0 ? Convert.ToInt32(row.Cells[colIndex].Value ?? 0) : 0;
                PreviewGrid.Rows.RemoveAt(i);
                PreviewGrid.Rows.Insert(0, statusText, path, size, progress);
                var newRow = PreviewGrid.Rows[0];
                newRow.Tag = key;
                newRow.DefaultCellStyle.BackColor = backColor;
                newRow.DefaultCellStyle.ForeColor = foreColor;
                PreviewGrid.FirstDisplayedScrollingRowIndex = 0;
                break;
            }
        }

        private void SetUploadRowPreparing(string rawFileName, string verb)
        {
            string key = rawFileName.Replace('\\', '/');
            PostUi(() => MoveRowToTop(key, verb, Color.FromArgb(243, 232, 255), Color.FromArgb(80, 20, 130)));
        }

        private void SetUploadRowUploading(string contentFileName)
        {
            string key = NormalizeUploadKey(contentFileName);
            PostUi(() => MoveRowToTop(key, "Uploading", Color.FromArgb(255, 246, 214), Color.FromArgb(120, 78, 0)));
        }

        private void RemoveUploadRow(string contentFileName)
        {
            string key = NormalizeUploadKey(contentFileName);
            PostUi(() =>
            {
                _activeRowKeys.Remove(key);
                foreach (DataGridViewRow row in PreviewGrid.Rows)
                {
                    if (string.Equals(row.Tag as string, key, StringComparison.OrdinalIgnoreCase))
                    {
                        PreviewGrid.Rows.Remove(row);
                        break;
                    }
                }
            });
        }

        private void SetUploadRowStaging(string contentFileName)
        {
            string key = NormalizeUploadKey(contentFileName);
            PostUi(() => MoveRowToTop(key, "Staging", Color.FromArgb(200, 245, 245), Color.FromArgb(0, 100, 110)));
        }

        private void SetUploadRowVerifying(string contentFileName)
        {
            string key = NormalizeUploadKey(contentFileName);
            PostUi(() => MoveRowToTop(key, "Verifying", Color.FromArgb(255, 250, 200), Color.FromArgb(100, 85, 0)));
        }

        private static string NormalizeUploadKey(string contentFileName)
        {
            string key = contentFileName.Replace('\\', '/');
            if (key.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
                key = key[..^3];
            return key;
        }

        private void SetUploadRowProgress(string contentFileName, int pct)
        {
            if (_progressColumn == null) return;
            string key = NormalizeUploadKey(contentFileName);
            int colIndex = _progressColumn.Index;
            PostUi(() =>
            {
                foreach (DataGridViewRow row in PreviewGrid.Rows)
                {
                    if (string.Equals(row.Tag as string, key, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Cells[colIndex].Value = pct;
                        break;
                    }
                }
            });
        }

        private void AdvanceOverallProgress()
        {
            if (_totalPublishFiles <= 0) return;
            int steps = Interlocked.Increment(ref _overallSteps);
            int pct = Math.Min(100, steps * 100 / (_totalPublishFiles * 4));
            PostUi(() => OverallProgressBar.Value = pct);
        }

        private void PopulatePreviewGrid(PatchCompareResult result)
        {
            OnUi(() =>
            {
                _previewRows.Clear();
                AddPreviewRows("Added", result.Added);
                AddPreviewRows("Changed", result.Changed);
                AddPreviewRows("Deleted", result.Deleted);
                AddPreviewRows("Unchanged", result.Unchanged);
                RenderPreviewRows();
            });
        }

        private void AddPreviewRows(string action, IEnumerable<PListFile> files)
        {
            foreach (var file in files.OrderBy(x => x.Path, StringComparer.OrdinalIgnoreCase))
            {
                _previewRows.Add(new PreviewRowInfo
                {
                    Action = action,
                    Path = file.Path,
                    Size = file.Length
                });
            }
        }

        private void RenderPreviewRows()
        {
            string filter = PreviewActionFilterDropDown.SelectedItem as string ?? "All";
            PreviewGrid.Rows.Clear();

            foreach (var row in _previewRows.Where(x => filter == "All" || x.Action == filter))
            {
                int rowIndex = PreviewGrid.Rows.Add(row.Action, row.Path, FormatBytes(row.Size));
                ApplyPreviewRowStyle(PreviewGrid.Rows[rowIndex], row.Action);
            }
        }

        private static void ApplyPreviewRowStyle(DataGridViewRow row, string action)
        {
            switch (action)
            {
                case "Added":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(225, 248, 225);
                    row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                    break;
                case "Changed":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 246, 214);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 78, 0);
                    break;
                case "Deleted":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 225, 225);
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    break;
                case "Unchanged":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(238, 238, 238);
                    row.DefaultCellStyle.ForeColor = Color.DimGray;
                    break;
            }
        }

        private void PreviewActionFilterDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderPreviewRows();
        }

        private bool PreparePublishPreview()
        {
            if (!ValidateSettings()) return false;

            SetActionText("Verifying connection...");
            try
            {
                using var probe = PatchTransportFactory.Create();
                probe.DirectoryExists(new Uri(Settings.Host).AbsolutePath);
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                ShowError(ex);
                SetActionText("Connection failed — check host and credentials.");
                return false;
            }

            Log($"Compare started — host: {Settings.Host}");
            UploadList = new Queue<FileInformation>();
            ClearPreviewGrid();
            UpdateSummaryLabels(null);

            SetActionText("Connecting...");
            RetryOnDisconnect(() =>
            {
                GetOldFileList();
            }, "Connecting");
            bool forceReuploadAll = false;

            if (OldList.Count == 0)
            {
                var result = OnUi(() => MessageBox.Show(this,
                        "No existing PList was found on the host. Publishing now will upload every file. Continue with compare?",
                        "PList not found",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning));

                if (result != DialogResult.Yes)
                {
                    SetActionText("Cancelled.");
                    return false;
                }
            }

            CheckCancelled();
            SetActionText("Comparing files...");

            GetNewFileList();
            CheckCancelled();

            _compareResult = BuildPListCompareResult();

            UpdateSummaryLabels(_compareResult);
            PopulatePreviewGrid(_compareResult);
            Log($"Compare done — added:{_compareResult.Added.Count} changed:{_compareResult.Changed.Count} deleted:{_compareResult.Deleted.Count} unchanged:{_compareResult.Unchanged.Count}");

            var changedPaths = new HashSet<string>(
                _compareResult.Added.Concat(_compareResult.Changed).Select(x => x.Path),
                StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < NewList.Count; i++)
            {
                FileInformation info = NewList[i];
                string normalizedPath = info.FileName.Replace('\\', '/');

                if (InExcludeList(info.FileName)) continue;

                if (forceReuploadAll || changedPaths.Contains(normalizedPath))
                {
                    UploadList.Enqueue(info);
                }
                else
                {
                    var old = FindOldFile(info.FileName);
                    if (old != null)
                        NewList[i] = old;
                }
            }

            bool hasPListChanges = _compareResult.UploadCount > 0 || _compareResult.Deleted.Count > 0;
            _publishPreviewReady = hasPListChanges;
            OnUi(() =>
            {
                ProcessButton.Enabled = _publishPreviewReady;
                ActionLabel.Text = _publishPreviewReady
                    ? $"Ready: {_compareResult.UploadCount} files to upload, {_compareResult.Deleted.Count} files to remove."
                    : "Up to date — all files match remote.";
            });
            return true;
        }

        private void DisableAllButtons()
        {
            OnUi(() =>
            {
                CompareButton.Enabled = false;
                ProcessButton.Enabled = false;
                ListButton.Enabled = false;
                DownloadExistingButton.Enabled = false;
                ClearRepositoryButton.Enabled = false;
                TestConnectionButton.Enabled = false;
                PreviewActionFilterDropDown.Enabled = false;
                AbortButton.Enabled = true;
            });
        }

        private void RestoreButtons()
        {
            OnUi(() =>
            {
                CompareButton.Enabled = true;
                ProcessButton.Enabled = _publishPreviewReady;
                ListButton.Enabled = true;
                DownloadExistingButton.Enabled = true;
                ClearRepositoryButton.Enabled = true;
                TestConnectionButton.Enabled = true;
                PreviewActionFilterDropDown.Enabled = true;
                AbortButton.Enabled = false;
                AbortButton.Text = "Cancel";
                PreviewGrid.Enabled = true;
                if (_statusConnectionLabel != null) _statusConnectionLabel.Text = string.Empty;
                Cursor = Cursors.Default;
            });
        }

        private static string FormatSpeed(long bytesPerSecond)
        {
            if (bytesPerSecond >= 1L << 30) return $"{bytesPerSecond / (double)(1L << 30):0.##} GB/s";
            if (bytesPerSecond >= 1L << 20) return $"{bytesPerSecond / (double)(1L << 20):0.##} MB/s";
            if (bytesPerSecond >= 1L << 10) return $"{bytesPerSecond / (double)(1L << 10):0.##} KB/s";
            return $"{bytesPerSecond} B/s";
        }

        private static string FormatBytes(long bytes)
        {
            if (bytes >= 1L << 30) return $"{bytes / (double)(1L << 30):0.##} GB";
            if (bytes >= 1L << 20) return $"{bytes / (double)(1L << 20):0.##} MB";
            if (bytes >= 1L << 10) return $"{bytes / (double)(1L << 10):0.##} KB";
            return $"{bytes} B";
        }

        private bool NeedUpdate(FileInformation info)
        {
            for (int i = 0; i < OldList.Count; i++)
            {
                FileInformation old = OldList[i];
                if (old.FileName != info.FileName) continue;

                if (old.Length != info.Length) return true;
                if (old.Creation != info.Creation)
                    return !RemoteContentMatches(old, info);

                return false;
            }
            return true;
        }

        private bool RemoteContentMatches(FileInformation old, FileInformation current)
        {
            try
            {
                string remoteFileName = old.FileName;
                if (old.Compressed > 0 && old.Compressed != old.Length)
                    remoteFileName += ".gz";

                byte[]? remoteBytes = DownloadFile(remoteFileName);
                if (remoteBytes == null) return false;

                if (old.Compressed > 0 && old.Compressed != old.Length)
                    remoteBytes = Decompress(remoteBytes);

                byte[] localBytes = File.ReadAllBytes(Path.Combine(Settings.Client, current.FileName));
                return remoteBytes.SequenceEqual(localBytes);
            }
            catch
            {
                return false;
            }
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

        private void BeginUpload()
        {
            if (UploadList == null) return;
            if (!ValidateSettings()) throw new OperationCanceledException("Validation failed.");

            // Guard against stale compare: re-download the PList and warn if it changed.
            SetActionText("Checking remote state...");
            byte[]? currentRemote = DownloadFile(PatchFileName);
            if (currentRemote != null && _remotePListRawBytes != null &&
                !currentRemote.SequenceEqual(_remotePListRawBytes))
            {
                var answer = OnUi(() => MessageBox.Show(this,
                    "The remote PList has changed since you ran Compare.\n" +
                    "Another publish may have happened in the meantime.\n\n" +
                    "Continue with the current publish plan?",
                    "Remote Changed",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2));
                if (answer != DialogResult.Yes)
                    throw new OperationCanceledException("Publish cancelled: remote PList changed.");
            }

            Log($"Publish started — {UploadList.Count} files");
            _lastPublishStart = DateTime.Now;

            DeleteDirectory(TempUploadDirectory);

            var uploadSnapshot = UploadList.ToList();
            _totalPublishFiles = uploadSnapshot.Count;
            _overallSteps = 0;
            OnUi(() => OverallProgressBar.Value = 0);

            PopulateUploadQueue(uploadSnapshot);

            var pListFiles = new List<string>();

            int totalToCompress = uploadSnapshot.Count;
            int compressed = 0;
            long totalOriginalBytes = 0L;
            long totalCompressedBytes = 0L;
            var contentFilePaths = new string[totalToCompress];
            var uploadedInfos = new FileInformation[totalToCompress];
            string prepVerb = Settings.CompressFiles ? "Compressing" : "Preparing";

            PostUi(() => ActionLabel.Text = $"{prepVerb} {totalToCompress} files...");

            // activeKeys  — files being compressed right now (at most ProcessorCount)
            // completedKeys — files that just finished; drained by the timer to turn green
            var activeKeys  = new System.Collections.Concurrent.ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            var completedKeys = new System.Collections.Concurrent.ConcurrentQueue<string>();
            int compressDone = 0;
            string completedStatus = Settings.CompressFiles ? "Compressed" : "Prepared";

            // WinForms Timer fires directly on the UI thread (no BeginInvoke queue).
            // Each tick: bubble active rows to top, drain up to 20 completed rows to green.
            System.Windows.Forms.Timer? uiTimer = null;
            OnUi(() =>
            {
                uiTimer = new System.Windows.Forms.Timer { Interval = 80 };
                uiTimer.Tick += (_, _) =>
                {
                    if (Volatile.Read(ref compressDone) != 0) return;

                    ActionLabel.Text = $"{prepVerb} {Volatile.Read(ref compressed)} of {totalToCompress}...";

                    // Turn recently-completed rows green (cap at 20 per tick to stay fast)
                    int drain = 20;
                    while (drain-- > 0 && completedKeys.TryDequeue(out string? doneKey))
                    {
                        _activeRowKeys.Remove(doneKey);
                        foreach (DataGridViewRow r in PreviewGrid.Rows)
                        {
                            if (string.Equals(r.Tag as string, doneKey, StringComparison.OrdinalIgnoreCase))
                            {
                                r.Cells[0].Value = completedStatus;
                                r.DefaultCellStyle.BackColor = Color.FromArgb(210, 240, 210);
                                r.DefaultCellStyle.ForeColor = Color.FromArgb(20, 100, 20);
                                break;
                            }
                        }
                    }

                    // Bubble active rows to top
                    foreach (string key in activeKeys.Keys)
                        MoveRowToTop(key, prepVerb, Color.FromArgb(243, 232, 255), Color.FromArgb(80, 20, 130));
                };
                uiTimer.Start();
            });

            try
            {
                Parallel.For(0, totalToCompress, new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    CancellationToken = _cts.Token
                }, i =>
                {
                    FileInformation info = uploadSnapshot[i];
                    string key = info.FileName.Replace('\\', '/');
                    activeKeys[key] = true;

                    string localPath = Path.Combine(Settings.Client, info.FileName);
                    byte[] raw = File.ReadAllBytes(localPath);
                    Interlocked.Add(ref totalOriginalBytes, raw.Length);

                    contentFilePaths[i] = CreateTempUploadFiles(info, raw);
                    Interlocked.Add(ref totalCompressedBytes, info.Compressed);
                    uploadedInfos[i] = info;
                    AdvanceOverallProgress();

                    activeKeys.TryRemove(key, out _);
                    completedKeys.Enqueue(key);
                    Interlocked.Increment(ref compressed);
                });
            }
            finally
            {
                Volatile.Write(ref compressDone, 1);
                OnUi(() => { uiTimer?.Stop(); uiTimer?.Dispose(); });
            }

            // Final sweep — mark everything remaining as Compressed.
            OnUi(() =>
            {
                _activeRowKeys.Clear();
                foreach (DataGridViewRow row in PreviewGrid.Rows)
                {
                    row.Cells[0].Value = completedStatus;
                    row.DefaultCellStyle.BackColor = Color.FromArgb(210, 240, 210);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 100, 20);
                }
                ActionLabel.Text = $"{prepVerb} complete — {totalToCompress} files.";
            });

            var contentFiles = contentFilePaths.ToList();

            _lastPublishFileCount = totalToCompress;
            _lastPublishOriginalBytes = totalOriginalBytes;
            _lastPublishCompressedBytes = totalCompressedBytes;

            if (Settings.CompressFiles && totalToCompress > 0 && totalOriginalBytes > 0)
            {
                long saved = totalOriginalBytes - totalCompressedBytes;
                int savePct = (int)((double)saved / totalOriginalBytes * 100);
                OnUi(() => SummaryUploadSizeLabel.Text += $" → {FormatBytes(totalCompressedBytes)} compressed ({savePct}% saved)");
            }

            pListFiles.Add(CreateTempUploadFiles(new FileInformation { FileName = PatchFileName }, CreateNewList()));

            CheckCancelled();
            try
            {
                RetryOnDisconnect(() => UploadFilesStaged(contentFiles, pListFiles), "Upload");
            }
            catch
            {
                DeleteDirectory(TempUploadDirectory);
                throw;
            }

            SetActionText("Cleaning old files...");
            try
            {
                RetryOnDisconnect(CleanUp, "Cleanup");
            }
            catch (Exception ex)
            {
                OnUi(() => MessageBox.Show(this, "Publish completed, but old-file cleanup failed:\r\n\r\n" + GetFriendlyError(ex), "Cleanup failed", MessageBoxButtons.OK, MessageBoxIcon.Warning));
            }

            UploadList = null;
            CompleteUpload();
        }

        private string CreateTempUploadFiles(FileInformation info, byte[] raw)
        {
            string fileName = info.FileName.Replace(@"\", "/");

            byte[] data = (!Settings.CompressFiles || fileName == PatchFileName) ? raw : Compress(raw);
            info.Compressed = data.Length;

            if (fileName != PatchFileName && Settings.CompressFiles)
            {
                fileName += ".gz";
            }

            var sourceDir = Path.GetDirectoryName(fileName);
            var tempSourceDir = string.IsNullOrEmpty(sourceDir)
                ? TempUploadDirectory
                : Path.Combine(TempUploadDirectory, sourceDir);

            var tempFilePath = Path.Combine(TempUploadDirectory, fileName).Replace(@"\", "/");

            if (!Directory.Exists(tempSourceDir))
            {
                Directory.CreateDirectory(tempSourceDir);
            }

            File.WriteAllBytes(tempFilePath, data);
            File.SetLastWriteTime(tempFilePath, info.Creation);

            return fileName;
        }

        private void UploadFilesStaged(IReadOnlyList<string> contentFiles, IReadOnlyList<string> pListFiles)
        {
            var rootPath = (new Uri(Settings.Host)).AbsolutePath;
            var publishId = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var stagingRoot = CombineRemotePath(rootPath, StagingDirectoryName);
            var stagingPath = CombineRemotePath(stagingRoot, publishId);
            var allFiles = contentFiles.Concat(pListFiles).ToList();
            int netDegree = Math.Max(2, Math.Min(Environment.ProcessorCount / 2, 8));

            try
            {
                // Create staging tree with a single connection before parallel uploads begin.
                SetActionText("Preparing staging area...");
                using (var setup = PatchTransportFactory.Create())
                {
                    setup.EnsureDirectory(rootPath);
                    EnsureRemoteDirectories(setup, allFiles, stagingPath);
                }

                // Stage content files in parallel.
                int staged = 0;
                int stageActive = 0;
                int totalStage = contentFiles.Count;
                SetActionText($"Staging {totalStage} files...");
                RunParallelWithTransport(contentFiles, netDegree, (t, file) =>
                {
                    CheckCancelled();
                    SetUploadRowStaging(file);
                    Interlocked.Increment(ref stageActive);

                    EventHandler<PatchTransferProgress> onProgress =
                        (_, e) => SetUploadRowProgress(file, (int)(e.FileProgress * 100));
                    t.ProgressChanged += onProgress;
                    int snapActive;
                    try
                    {
                        t.UploadDirectory(TempUploadDirectory, stagingPath, new[] { file }, "Staging", _cts.Token);
                        SetUploadRowProgress(file, 100);
                        AdvanceOverallProgress();
                        snapActive = Volatile.Read(ref stageActive);
                    }
                    finally
                    {
                        t.ProgressChanged -= onProgress;
                        Interlocked.Decrement(ref stageActive);
                    }

                    UpdateRowStatus(NormalizeUploadKey(file), "Staged", Color.FromArgb(190, 225, 240), Color.FromArgb(0, 70, 110));
                    int done = Interlocked.Increment(ref staged);
                    PostUi(() =>
                    {
                        ActionLabel.Text = $"Staging — {done} / {totalStage} done  ({snapActive} active)";
                        FileLabel.Text = file;
                    });
                });

                // Stage PList files sequentially (1-2 files; not worth a parallel pool).
                using (var mTransport = PatchTransportFactory.Create())
                    mTransport.UploadDirectory(TempUploadDirectory, stagingPath, pListFiles, "Staging PList", _cts.Token);

                // Verify staged files in parallel.
                VerifyStagedFiles(stagingPath, allFiles, netDegree);

                // Create live destination directories before promoting in parallel.
                using (var setup = PatchTransportFactory.Create())
                    EnsureRemoteDirectories(setup, contentFiles, rootPath);

                // Promote content files in parallel.
                PromoteStagedFiles(stagingPath, rootPath, contentFiles, "Uploading", netDegree,
                    onStart: SetUploadRowUploading,
                    onComplete: RemoveUploadRow);

                // Promote PList atomically on a single connection.
                using (var mTransport = PatchTransportFactory.Create())
                {
                    foreach (var file in pListFiles)
                    {
                        CheckCancelled();
                        SetActionText("Publishing PList...");
                        string src = CombineRemotePath(stagingPath, file);
                        string dst = CombineRemotePath(rootPath, file);
                        mTransport.EnsureDirectory(GetRemoteDirectory(dst));
                        mTransport.MoveFile(src, dst);
                    }
                }

                using (var cleanup = PatchTransportFactory.Create())
                    TryRemoveRemoteStagingDirectories(cleanup, stagingRoot, stagingPath);
            }
            catch
            {
                SetActionText("Cleaning up staging...");
                try
                {
                    using var cleanup = PatchTransportFactory.Create();
                    TryRemoveRemoteStagingDirectories(cleanup, stagingRoot, stagingPath);
                }
                catch { }
                throw;
            }

            DeleteDirectory(TempUploadDirectory);
        }

        private void VerifyStagedFiles(string stagingPath, IReadOnlyList<string> files, int degree)
        {
            int total = files.Count;
            int verified = 0;
            int verifyActive = 0;
            SetActionText($"Verifying {total} files...");
            RunParallelWithTransport(files, degree, (transport, file) =>
            {
                CheckCancelled();
                SetUploadRowVerifying(file);
                Interlocked.Increment(ref verifyActive);

                string localPath = Path.Combine(TempUploadDirectory, file);
                string remotePath = CombineRemotePath(stagingPath, file);

                if (!File.Exists(localPath))
                    throw new FileNotFoundException("Local staged file is missing.", localPath);

                long localLength = new FileInfo(localPath).Length;
                long remoteLength = transport.GetFileLength(remotePath);
                int snapActive = Volatile.Read(ref verifyActive);
                Interlocked.Decrement(ref verifyActive);

                if (localLength != remoteLength)
                    throw new InvalidOperationException($"Staged upload verification failed for {file}: local {localLength} bytes, remote {remoteLength} bytes.");

                AdvanceOverallProgress();
                int done = Interlocked.Increment(ref verified);
                PostUi(() => ActionLabel.Text = $"Verifying — {done} / {total} done  ({snapActive} active)");
            });
        }

        private void PromoteStagedFiles(string stagingPath, string rootPath, IReadOnlyList<string> files,
            string label, int degree,
            Action<string>? onStart = null, Action<string>? onComplete = null)
        {
            int total = files.Count;
            int promoted = 0;
            int promoteActive = 0;
            SetActionText($"{label} {total} files...");
            RunParallelWithTransport(files, degree, (transport, file) =>
            {
                CheckCancelled();
                onStart?.Invoke(file);
                Interlocked.Increment(ref promoteActive);
                try
                {
                    transport.MoveFile(CombineRemotePath(stagingPath, file), CombineRemotePath(rootPath, file));
                }
                finally
                {
                    Interlocked.Decrement(ref promoteActive);
                }
                onComplete?.Invoke(file);
                AdvanceOverallProgress();
                int done = Interlocked.Increment(ref promoted);
                int snapActive = Volatile.Read(ref promoteActive);
                PostUi(() =>
                {
                    ActionLabel.Text = $"{label} — {done} / {total} done  ({snapActive} active)";
                    FileLabel.Text = file;
                });
            });
        }

        private void RunParallelWithTransport(IReadOnlyList<string> files, int degree, Action<IPatchTransport, string> body)
        {
            if (files.Count == 0) return;

            int requestedWorkerCount = Math.Min(degree, files.Count);
            int discoveredCeiling = DiscoverConnectionCeiling(requestedWorkerCount);
            int activeTarget = GetSafeConnectionTarget(discoveredCeiling);
            int workerCount = Math.Min(requestedWorkerCount, discoveredCeiling);
            var fileQueue = new System.Collections.Concurrent.ConcurrentQueue<string>(files);

            // Keep one safety margin below the discovered/current ceiling. For example, an
            // 8-connection ceiling starts 6 workers and displays as "Connections: N / 8".
            Volatile.Write(ref _connectionCeiling, discoveredCeiling);
            Volatile.Write(ref _liveConnections, 0);
            PostUi(RefreshConnectionLabel);

            var workers = new Task[workerCount];
            for (int w = 0; w < workerCount; w++)
            {
                int workerId = w;
                workers[w] = Task.Run(() =>
                {
                    IPatchTransport? conn = null;
                    int reconnects = 0;
                    const int maxReconnects = 3;

                    try
                    {
                        // Stop this worker if the dynamic target has scaled below our ID.
                        while (Volatile.Read(ref activeTarget) > workerId && fileQueue.TryDequeue(out string? file))
                        {
                            _cts.Token.ThrowIfCancellationRequested();

                            if (conn == null)
                            {
                                // Stagger connection creation so a burst of simultaneous handshakes
                                // does not look like a connection-flood attack to the SSH daemon.
                                lock (_connectionCreateLock)
                                    Thread.Sleep(100);

                                try
                                {
                                    conn = PatchTransportFactory.Create();
                                    Interlocked.Increment(ref _liveConnections);
                                    PostUi(RefreshConnectionLabel);
                                    reconnects = 0;
                                }
                                catch (OperationCanceledException) { throw; }
                                catch (Exception ex)
                                {
                                    // File not yet processed — put it back for surviving workers.
                                    fileQueue.Enqueue(file);

                                    if (IsConnectionLimitException(ex))
                                    {
                                        // Treat the last attempted target as the host/session
                                        // ceiling, then keep active workers at 80% of that.
                                        int cur = Volatile.Read(ref activeTarget);
                                        int reduced = GetSafeConnectionTarget(cur);
                                        Interlocked.CompareExchange(ref activeTarget, reduced, cur);
                                        Volatile.Write(ref _connectionCeiling, cur);
                                        PostUi(RefreshConnectionLabel);
                                        Log($"Server connection limit reached — using {reduced}/{cur} worker(s): {ex.Message}");
                                    }
                                    else
                                    {
                                        Log($"Connection failed, worker {workerId} exiting: {ex.Message}");
                                    }
                                    return;
                                }
                            }

                            try
                            {
                                body(conn, file);
                            }
                            catch (OperationCanceledException)
                            {
                                fileQueue.Enqueue(file);
                                throw;
                            }
                            catch (Exception ex) when (IsDisconnectException(ex))
                            {
                                // Transport dropped mid-transfer — re-queue and reconnect next iteration.
                                fileQueue.Enqueue(file);
                                Interlocked.Decrement(ref _liveConnections);
                                PostUi(RefreshConnectionLabel);
                                try { conn.Dispose(); } catch { }
                                conn = null;

                                if (++reconnects >= maxReconnects)
                                {
                                    Log($"Worker {workerId} exiting after {maxReconnects} consecutive transport failures: {ex.Message}");
                                    return;
                                }
                                Log($"Transport dropped, re-queuing '{file}' (attempt {reconnects}/{maxReconnects}).");
                            }
                        }
                    }
                    finally
                    {
                        if (conn != null)
                        {
                            Interlocked.Decrement(ref _liveConnections);
                            PostUi(RefreshConnectionLabel);
                        }
                        conn?.Dispose();
                    }
                });
            }

            try
            {
                Task.WaitAll(workers);
            }
            catch (AggregateException aex)
            {
                var inner = aex.Flatten().InnerExceptions.FirstOrDefault(e => e is not OperationCanceledException)
                    ?? aex.InnerException;
                if (inner != null)
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(inner).Throw();
                throw;
            }

            _cts.Token.ThrowIfCancellationRequested();

            if (!fileQueue.IsEmpty)
                throw new InvalidOperationException(
                    $"Upload incomplete — {fileQueue.Count} file(s) could not be processed. " +
                    "The server rejected all connections or all workers exceeded the reconnect limit.");
        }

        private int DiscoverConnectionCeiling(int requestedCeiling)
        {
            if (requestedCeiling <= 1)
                return 1;

            var rootPath = new Uri(Settings.Host).AbsolutePath;
            SetActionText($"Checking connection ceiling up to {requestedCeiling}...");

            var discovered = 1;
            for (var candidate = 1; candidate <= requestedCeiling; candidate++)
            {
                _cts.Token.ThrowIfCancellationRequested();

                if (CanOpenConcurrentProbeConnections(candidate, rootPath))
                {
                    discovered = candidate;
                    continue;
                }

                break;
            }

            Log($"Connection ceiling discovered: {discovered}; active target: {GetSafeConnectionTarget(discovered)}.");
            return discovered;
        }

        private bool CanOpenConcurrentProbeConnections(int count, string rootPath)
        {
            using var startGate = new ManualResetEventSlim(false);
            var transports = new IPatchTransport?[count];
            Exception? failure = null;
            var tasks = new Task[count];

            for (var i = 0; i < count; i++)
            {
                var index = i;
                tasks[index] = Task.Run(() =>
                {
                    try
                    {
                        startGate.Wait(_cts.Token);
                        _cts.Token.ThrowIfCancellationRequested();

                        var transport = PatchTransportFactory.Create();
                        transports[index] = transport;

                        // Force a lightweight request so FTP/HTTP restrictions are probed too.
                        transport.DirectoryExists(rootPath);

                        // Keep persistent transports open briefly so later probes overlap.
                        _cts.Token.WaitHandle.WaitOne(250);
                    }
                    catch (Exception ex)
                    {
                        Interlocked.CompareExchange(ref failure, ex, null);
                    }
                }, _cts.Token);
            }

            startGate.Set();

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ex)
            {
                failure ??= ex.Flatten().InnerExceptions.FirstOrDefault();
            }
            finally
            {
                foreach (var transport in transports)
                {
                    try { transport?.Dispose(); } catch { }
                }
            }

            if (failure == null)
                return true;

            if (failure is OperationCanceledException)
                throw failure;

            if (IsConnectionLimitException(failure) || IsDisconnectException(failure))
            {
                Log($"Connection ceiling probe failed at {count}: {failure.Message}");
                return false;
            }

            Log($"Connection ceiling probe stopped at {count}: {failure.GetType().Name}: {failure.Message}");
            return false;
        }

        private static int GetSafeConnectionTarget(int connectionCeiling)
        {
            return Math.Max(1, (int)Math.Floor(connectionCeiling * 0.8));
        }

        private static void EnsureRemoteDirectories(IPatchTransport transport, IReadOnlyList<string> files, string remoteRoot)
        {
            var dirs = files
                .Select(f => GetRemoteDirectory(CombineRemotePath(remoteRoot, f)))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(d => d.Length) // parents before children
                .ToList();
            foreach (var dir in dirs)
                transport.EnsureDirectory(dir);
        }

        private static string GetRemoteDirectory(string remotePath)
        {
            remotePath = (remotePath ?? string.Empty).Replace('\\', '/');
            int index = remotePath.LastIndexOf('/');
            return index <= 0 ? "/" : remotePath.Substring(0, index);
        }

        private void TryRemoveRemoteDirectory(IPatchTransport transport, string remoteDirectory)
        {
            try
            {
                if (transport.DirectoryExists(remoteDirectory))
                    transport.DeleteDirectory(remoteDirectory);
            }
            catch
            {
                // Staging cleanup is best-effort; files have already been promoted.
            }
        }

        private void TryRemoveRemoteStagingDirectories(IPatchTransport transport, string stagingRoot, string stagingPath)
        {
            TryRemoveRemoteDirectory(transport, stagingPath);
            TryRemoveEmptyRemoteDirectory(transport, stagingRoot);
        }

        private void TryRemoveEmptyRemoteDirectory(IPatchTransport transport, string remoteDirectory)
        {
            try
            {
                if (transport.DirectoryExists(remoteDirectory))
                    transport.DeleteEmptyDirectory(remoteDirectory);
            }
            catch
            {
                // Another publish may still be using the staging root, or the host may reject empty-dir removal.
            }
        }

        private byte[]? DownloadFile(string fileName)
        {
            try
            {
                using IPatchTransport transport = PatchTransportFactory.Create();
                var rootPath = (new Uri(Settings.Host)).AbsolutePath;
                string remotePath = CombineRemotePath(rootPath, fileName);
                if (!transport.FileExists(remotePath))
                    return null;

                return transport.DownloadFile(remotePath);
            }
            catch (Exception ex) when (IsRemoteFileNotFound(ex))
            {
                return null;
            }
        }

        private static bool IsRemoteFileNotFound(Exception ex)
        {
            if (ex is WebException webException)
            {
                if ((webException.Response as FtpWebResponse)?.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return true;
                if ((webException.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                    return true;
            }

            if (ex is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
                return true;

            string typeName = ex.GetType().FullName ?? string.Empty;
            if (typeName.Contains("SftpPathNotFoundException", StringComparison.Ordinal))
                return true;

            return ex.InnerException != null && IsRemoteFileNotFound(ex.InnerException);
        }

        private void DownloadFiles()
        {
            var rootPath = (new Uri(Settings.Host)).AbsolutePath;

            using IPatchTransport transport = PatchTransportFactory.Create();
            transport.ProgressChanged += (_, e) => PostUi(() =>
            {
                FileLabel.Text = e.FileName;
                SpeedLabel.Text = FormatSpeed(e.BytesPerSecond);
                ActionLabel.Text = $"Downloading... {e.RemainingFiles} remaining";
            });

            if (!Directory.Exists(TempDownloadDirectory))
            {
                Directory.CreateDirectory(TempDownloadDirectory);
            }

            var remotePaths = OldList.Select(x =>
            {
                bool compressed = x.Length != x.Compressed;
                return CombineRemotePath(rootPath, x.FileName + (compressed ? ".gz" : ""));
            }).ToList();

            transport.DownloadFiles(remotePaths, TempDownloadDirectory, _cts.Token);
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
                var destDir = string.IsNullOrEmpty(relativeDestDir)
                    ? Settings.Client
                    : Path.Combine(Settings.Client, relativeDestDir);
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




        private async void ListButton_Click(object sender, EventArgs e)
        {
            SaveConnectionSettings();
            if (!ValidateSettings()) return;

            _cts = new CancellationTokenSource();
            DisableAllButtons();
            Log("PList upload started.");
            try
            {
                await Task.Run(() =>
                {
                    GetOldFileList();
                    CheckCancelled();
                    GetNewFileList();
                    CheckCancelled();

                    for (int i = 0; i < NewList.Count; i++)
                    {
                        CheckCancelled();
                        FileInformation info = NewList[i];
                        for (int o = 0; o < OldList.Count; o++)
                        {
                            if (OldList[o].FileName != info.FileName) continue;
                            NewList[i].Compressed = OldList[o].Compressed;
                            break;
                        }
                        if (info.Compressed == 0)
                            info.Compressed = info.Length;
                    }

                    var pListFiles = new List<string>
                    {
                        CreateTempUploadFiles(new FileInformation { FileName = PatchFileName }, CreateNewList())
                    };
                    RetryOnDisconnect(() => UploadFilesStaged(new List<string>(), pListFiles), "Upload");

                    CompletePListOperation("PList updated.");
                    Log("PList upload complete.");
                });
            }
            catch (OperationCanceledException)
            {
                ActionLabel.Text = "Cancelled.";
                DeleteDirectory(TempUploadDirectory);
                Log("PList upload cancelled.");
                ClearPreviewGrid();
                UpdateSummaryLabels(null);
                MessageBox.Show(this, "Cancel complete. Any partial work has been cleaned up.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ActionLabel.Text = "Error.";
                DeleteDirectory(TempUploadDirectory);
                Log($"PList upload error: {ex.Message}");
                MessageBox.Show(this, GetFriendlyError(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                RestoreButtons();
            }
        }

        private async void ProcessButton_Click(object sender, EventArgs e)
        {
            if (!_publishPreviewReady || UploadList == null)
            {
                MessageBox.Show(this, "Run Compare first so you can review the publish plan.", "Compare required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _cts = new CancellationTokenSource();
            DisableAllButtons();
            ActionLabel.Text = $"Publishing {_compareResult?.UploadCount ?? 0} changed files...";
            try
            {
                await Task.Run(BeginUpload);
            }
            catch (OperationCanceledException)
            {
                _publishPreviewReady = false;
                ActionLabel.Text = "Cancelled.";
                Log("Publish cancelled.");
                ClearPreviewGrid();
                UpdateSummaryLabels(null);
                MessageBox.Show(this, "Cancel complete. Any partial work has been cleaned up.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _publishPreviewReady = false;
                ActionLabel.Text = "Error.";
                Log($"Publish error: {ex.Message}");
                MessageBox.Show(this, GetFriendlyError(ex), "Publish Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                RestoreButtons();
            }
        }

        private async void CompareButton_Click(object sender, EventArgs e)
        {
            SaveConnectionSettings();
            _cts = new CancellationTokenSource();
            DisableAllButtons();
            try
            {
                await Task.Run(PreparePublishPreview);
            }
            catch (OperationCanceledException)
            {
                ActionLabel.Text = "Cancelled.";
                Log("Compare cancelled.");
                ClearPreviewGrid();
                UpdateSummaryLabels(null);
                MessageBox.Show(this, "Cancel complete. Any partial work has been cleaned up.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ActionLabel.Text = "Error.";
                Log($"Compare error: {ex.Message}");
                MessageBox.Show(this, GetFriendlyError(ex), "Compare Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                RestoreButtons();
            }
        }

        private async void DownloadExistingButton_Click(object sender, EventArgs e)
        {
            SaveConnectionSettings();
            if (!ValidateSettings()) return;

            _cts = new CancellationTokenSource();
            DisableAllButtons();
            Log("Download started.");
            try
            {
                await Task.Run(() =>
                {
                    RetryOnDisconnect(() =>
                    {
                        GetOldFileList();
                        DownloadFiles();
                    }, "Download");
                    MoveTempDownloadedFiles();
                    Log("Download complete.");
                });
                CompleteDownload();
            }
            catch (OperationCanceledException)
            {
                ActionLabel.Text = "Cancelled.";
                Log("Download cancelled.");
                await Task.Run(() => DeleteDirectory(TempDownloadDirectory));
                ClearPreviewGrid();
                UpdateSummaryLabels(null);
                MessageBox.Show(this, "Cancel complete. Any partial work has been cleaned up.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ActionLabel.Text = "Error.";
                Log($"Download error: {ex.Message}");
                await Task.Run(() => DeleteDirectory(TempDownloadDirectory));
                MessageBox.Show(this, GetFriendlyError(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                RestoreButtons();
            }
        }

        private async void ClearRepositoryButton_Click(object sender, EventArgs e)
        {
            SaveConnectionSettings();
            if (!ValidateSettings()) return;

            var confirm = MessageBox.Show(this,
                "WARNING: This will permanently delete ALL files from the remote repository, including the PList and all client files.\n\nThis cannot be undone. Are you absolutely sure?",
                "Clear Remote Repository",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
            {
                ActionLabel.Text = "Clear cancelled.";
                return;
            }

            _cts = new CancellationTokenSource();
            DisableAllButtons();

            try
            {
                await Task.Run(ClearRemoteRepository);
            }
            catch (OperationCanceledException)
            {
                ActionLabel.Text = "Cancelled.";
                MessageBox.Show(this, "Cancel complete. Any partial work has been cleaned up.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, GetFriendlyError(ex), "Clear Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ActionLabel.Text = "Error during clear.";
            }
            finally
            {
                ClearPreviewGrid();
                UpdateSummaryLabels(null);
                RestoreButtons();
            }
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
            SetActionText("Cancelling...");
            OnUi(() =>
            {
                AbortButton.Enabled = false;
                AbortButton.Text = "Cancelling...";
                PreviewGrid.Enabled = false;
                Cursor = Cursors.WaitCursor;
            });
        }

        private async void TestConnectionButton_Click(object sender, EventArgs e)
        {
            SaveConnectionSettings();
            if (!ValidateSettings()) return;

            _cts = new CancellationTokenSource();
            DisableAllButtons();
            SetActionText("Testing connection...");
            try
            {
                bool hasPList = false;
                await Task.Run(() =>
                {
                    using var transport = PatchTransportFactory.Create();
                    var rootPath = new Uri(Settings.Host).AbsolutePath;
                    hasPList = transport.FileExists(CombineRemotePath(rootPath, PatchFileName));
                });
                ActionLabel.Text = hasPList
                    ? "Connection OK — PList found."
                    : "Connection OK — no PList yet (fresh repository).";
            }
            catch (OperationCanceledException)
            {
                ActionLabel.Text = "Cancelled.";
            }
            catch (Exception ex)
            {
                ActionLabel.Text = "Connection failed.";
                MessageBox.Show(this, GetFriendlyError(ex), "Connection Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                RestoreButtons();
            }
        }

        private void ClearRemoteRepository()
        {
            var rootPath = (new Uri(Settings.Host)).AbsolutePath;
            var failures = new List<string>();

            using IPatchTransport transport = PatchTransportFactory.Create();

            // --- Delete files ---
            SetActionText("Fetching remote file list...");
            IReadOnlyList<string> remoteFiles = transport.EnumerateFiles(rootPath, _cts.Token);

            var filesToDelete = remoteFiles
                .Where(x => !string.Equals(x.TrimEnd('/'), NormalizeRemotePath(rootPath), StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => IsPListPath(x) ? 1 : 0)
                .ThenBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToList();

            int fileTotal = filesToDelete.Count;
            int fileDone = 0;

            foreach (string remotePath in filesToDelete)
            {
                CheckCancelled();
                fileDone++;
                string displayPath = MakeRelativeRemotePath(rootPath, remotePath);
                SetActionText($"Deleting file {fileDone} of {fileTotal}...");
                SetFileText(displayPath);

                try
                {
                    transport.DeleteFile(remotePath);
                    if (transport.FileExists(remotePath))
                    {
                        failures.Add($"{displayPath}: file still exists after delete");
                        Log($"Clear Remote verify failed: {displayPath}");
                    }
                    else
                    {
                        Log($"Clear Remote deleted: {displayPath}");
                    }
                }
                catch (Exception ex)
                {
                    failures.Add($"{displayPath}: {ex.Message}");
                    Log($"Clear Remote failed: {displayPath}: {ex.Message}");
                }
            }

            // --- Delete directories (deepest first so parents are empty when removed) ---
            SetActionText("Fetching remote directory list...");
            SetFileText(string.Empty);
            IReadOnlyList<string> remoteDirs = transport.EnumerateDirectories(rootPath, _cts.Token);

            int dirTotal = remoteDirs.Count;
            int dirDone = 0;

            foreach (string dirPath in remoteDirs)
            {
                CheckCancelled();
                dirDone++;
                string displayPath = MakeRelativeRemotePath(rootPath, dirPath);
                SetActionText($"Removing directory {dirDone} of {dirTotal}...");
                SetFileText(displayPath);

                try
                {
                    if (transport.DirectoryExists(dirPath))
                        transport.DeleteDirectory(dirPath);
                    Log($"Clear Remote removed dir: {displayPath}");
                }
                catch (Exception ex)
                {
                    failures.Add($"{displayPath}/: {ex.Message}");
                    Log($"Clear Remote dir failed: {displayPath}: {ex.Message}");
                }
            }

            if (fileTotal == 0 && dirTotal == 0)
            {
                SetActionText("Remote repository is already empty.");
                return;
            }

            if (failures.Count > 0)
                throw new InvalidOperationException($"Remote clear completed with {failures.Count} failure(s):\r\n\r\n" + string.Join("\r\n", failures.Take(20)));

            OldList = new List<FileInformation>();
            SetFileText(string.Empty);
            SetActionText($"Remote repository cleared — {fileTotal} file(s), {dirTotal} director(ies) removed.");
        }

        private static string NormalizeRemotePath(string path)
        {
            path = (path ?? string.Empty).Replace('\\', '/');
            return path.StartsWith("/") ? path : "/" + path;
        }

        private static bool IsPListPath(string remotePath)
        {
            string fileName = Path.GetFileName(remotePath.Replace('\\', '/'));
            return string.Equals(fileName, PatchFileName, StringComparison.OrdinalIgnoreCase);
        }

        private static string MakeRelativeRemotePath(string rootPath, string remotePath)
        {
            rootPath = NormalizeRemotePath(rootPath).TrimEnd('/');
            remotePath = NormalizeRemotePath(remotePath);

            if (!string.IsNullOrEmpty(rootPath) &&
                remotePath.StartsWith(rootPath + "/", StringComparison.OrdinalIgnoreCase))
                return remotePath.Substring(rootPath.Length + 1);

            return remotePath.TrimStart('/');
        }

        private void AMain_Load(object sender, EventArgs e)
        {
            ActionLabel.TextChanged += (_, _) => StatusActionLabel.Text = ActionLabel.Text;
            FileLabel.TextChanged   += (_, _) => StatusFileLabel.Text   = FileLabel.Text;

            _statusConnectionLabel = new ToolStripStatusLabel
            {
                AutoSize = false,
                Width = 160,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = string.Empty
            };
            MainStatusStrip.Items.Insert(1, _statusConnectionLabel);
        }

        private void RefreshConnectionLabel()
        {
            if (_statusConnectionLabel == null) return;
            int live = Volatile.Read(ref _liveConnections);
            int ceil = Volatile.Read(ref _connectionCeiling);
            _statusConnectionLabel.Text = live > 0 ? $"Connections: {live} / {ceil}" : string.Empty;
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
