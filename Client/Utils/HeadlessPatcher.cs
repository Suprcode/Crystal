using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Client;
using Client.Utils;

namespace Launcher
{
    public class PatcherProgressEventArgs : EventArgs
    {
        public string CurrentFileName { get; set; }
        public int FilesDownloaded { get; set; }
        public int TotalFiles { get; set; }
        public long BytesDownloaded { get; set; }
        public long TotalBytes { get; set; }
        public double SpeedBytesPerSecond { get; set; }
        public string StatusMessage { get; set; }
    }

    public class HeadlessPatcher
    {
        public event EventHandler<PatcherProgressEventArgs> ProgressChanged;

        private static readonly HttpClient _httpClient;
        
        private long _completedBytes = 0;
        private long _totalBytesToDownload = 0;
        private int _totalFilesToDownload = 0;
        private int _filesDownloadedCount = 0;
        private Stopwatch _downloadStopwatch;
        private long _lastConsoleReportTime = 0;
        private readonly object _consoleLock = new object();

        static HeadlessPatcher()
        {
            // Upgrade HTTP stack: Use SocketsHttpHandler to support connection pooling and compression.
            var handler = new SocketsHttpHandler
            {
                AutomaticDecompression = DecompressionMethods.All,
                PooledConnectionLifetime = TimeSpan.FromMinutes(5)
            };

            _httpClient = new HttpClient(handler)
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        /// <summary>
        /// Run the full headless patching flow asynchronously.
        /// </summary>
        public async Task<bool> RunAsync(bool cleanFiles = false, CancellationToken cancellationToken = default)
        {
            // Step 1: Self-Update Check
            if (CheckSelfUpdate())
            {
                // Self-update triggered process restart; abort this execution
                return false;
            }

            // Step 2: Initialization
            if (!Settings.P_Patcher)
            {
                LogProgress(null, 0, 0, 0, 0, 0, "Patcher is disabled in settings. Skipping update flow.");
                return true;
            }

            PurgeOldBackups();

            string tempDirPath = Path.Combine(Settings.P_Client, ".patch_temp");

            try
            {
                // Step 3: Fetch the Manifest
                List<FileInformation> manifestList = await FetchManifestAsync(cancellationToken).ConfigureAwait(false);
                if (manifestList == null || manifestList.Count == 0)
                {
                    LogProgress(null, 0, 0, 0, 0, 0, "No files found in patch manifest.");
                    return true;
                }

                // Step 4: Local Validation
                List<FileInformation> downloadQueue = ValidateLocalFiles(manifestList, out _totalBytesToDownload);
                _totalFilesToDownload = downloadQueue.Count;

                if (_totalFilesToDownload == 0)
                {
                    LogProgress(null, 0, 0, 0, 0, 0, "Game client is up-to-date!");
                    
                    // Clean obsolete files even if no downloads are needed
                    if (cleanFiles)
                    {
                        CleanUpObsoleteFiles(manifestList);
                    }
                    return true;
                }

                LogProgress(null, 0, _totalFilesToDownload, 0, _totalBytesToDownload, 0, 
                    $"Starting update: {_totalFilesToDownload} files to download ({_totalBytesToDownload / 1024 / 1024} MB)...");

                // Prepare temp directory: do not delete existing files to support resuming
                if (!Directory.Exists(tempDirPath))
                {
                    Directory.CreateDirectory(tempDirPath);
                }

                // Step 5: Concurrent Downloading & Decompression
                _downloadStopwatch = Stopwatch.StartNew();

                var options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Settings.P_Concurrency > 0 ? Settings.P_Concurrency : 1,
                    CancellationToken = cancellationToken
                };

                await Parallel.ForEachAsync(downloadQueue, options, async (fileInfo, ct) =>
                {
                    await DownloadFileAsync(fileInfo, tempDirPath, ct).ConfigureAwait(false);
                }).ConfigureAwait(false);

                _downloadStopwatch.Stop();

                // Step 6: Atomic Move Strategy (Move temp files to destination)
                LogProgress(null, _filesDownloadedCount, _totalFilesToDownload, _completedBytes, _totalBytesToDownload, 0, 
                    "All downloads successfully completed. Applying file updates...");

                foreach (var fileInfo in downloadQueue)
                {
                    string localRelative = NormalizeLocalPath(fileInfo.FileName);
                    string sourcePath = Path.Combine(tempDirPath, localRelative);
                    string destPath = Path.Combine(Settings.P_Client, localRelative);

                    // Perform strict integrity check on temp files before swap
                    if (!File.Exists(sourcePath))
                    {
                        throw new FileNotFoundException($"Downloaded temp file was not found: {sourcePath}");
                    }
                    long actualSize = new FileInfo(sourcePath).Length;
                    if (actualSize != fileInfo.Length)
                    {
                        throw new InvalidDataException($"Integrity check failed: Temp file size ({actualSize}) does not match manifest size ({fileInfo.Length}) for {fileInfo.FileName}.");
                    }

                    // Apply Atomic Swap Strategy (Handles running / locked process files safely)
                    PerformAtomicSwap(sourcePath, destPath);

                    // Post-Processing: Set creation & write time to match the server timestamp
                    File.SetLastWriteTime(destPath, fileInfo.Creation);
                }

                // Clean up temp directory
                if (Directory.Exists(tempDirPath))
                {
                    Directory.Delete(tempDirPath, true);
                }

                // Step 7: Clean Up Obsolete Files
                if (cleanFiles)
                {
                    CleanUpObsoleteFiles(manifestList);
                }

                LogProgress(null, _totalFilesToDownload, _totalFilesToDownload, _totalBytesToDownload, _totalBytesToDownload, 0, 
                    "Update completed successfully! Client is up to date.");

                return true;
            }
            catch (Exception ex)
            {
                LogProgress(null, 0, 0, 0, 0, 0, $"Update failed: {ex.Message}");

                // Clean rollback: Delete any temporary downloads to prevent corrupted state
                try
                {
                    if (Directory.Exists(tempDirPath))
                    {
                        Directory.Delete(tempDirPath, true);
                    }
                }
                catch { }

                return false;
            }
        }

        /// <summary>
        /// Perform atomic swap using rename-first strategy, allowing replacement of loaded/locked assemblies (like Client.dll).
        /// </summary>
        private void PerformAtomicSwap(string sourcePath, string destPath)
        {
            string destDir = Path.GetDirectoryName(destPath);
            if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            if (File.Exists(destPath))
            {
                // CRITICAL FOR SELF-UPDATE & PROCESS LOCKING:
                // Renaming a locked/running file (e.g. Client.dll or Client.exe) is allowed under both Windows and Linux.
                // We rename it first to ".patch_old", then atomically swap the new file in place.
                string backupPath = destPath + ".patch_old";
                if (File.Exists(backupPath))
                {
                    try { File.Delete(backupPath); } catch { }
                }

                try
                {
                    File.Move(destPath, backupPath);
                    File.Move(sourcePath, destPath, overwrite: true);

                    // Under Linux, inode unlinking allows us to immediately delete the old running binary safely!
                    // Under Windows, we catch and ignore locking exceptions, leaving the backup to be deleted on next start.
                    try
                    {
                        File.Delete(backupPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Patcher] Running backup file {Path.GetFileName(backupPath)} kept until next application launch: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Patcher] Safe rename-swap failed for {destPath}: {ex.Message}. Attempting direct overwrite.");
                    File.Move(sourcePath, destPath, overwrite: true);
                }
            }
            else
            {
                File.Move(sourcePath, destPath, overwrite: true);
            }
        }

        /// <summary>
        /// Fetch binary patch list (manifest) file.
        /// </summary>
        private async Task<List<FileInformation>> FetchManifestAsync(CancellationToken cancellationToken)
        {
            string url = $"{Settings.P_Host}{Settings.P_PatchFileName}";
            
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new InvalidOperationException($"Invalid patch host URL: {url}");
            }

            using var request = CreateRequest(url);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            byte[] data = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            if (data == null || data.Length == 0)
            {
                throw new InvalidDataException("Empty response received from manifest download.");
            }

            if (data[0] == 60) // '<' character - got an HTML error page instead of binary list
            {
                throw new InvalidDataException("Received invalid HTML manifest. Please verify your host patch settings.");
            }

            var manifestList = new List<FileInformation>();
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    manifestList.Add(new FileInformation(reader));
                }
            }

            return manifestList;
        }

        /// <summary>
        /// Validate local files against the manifest by checking size and last modification dates.
        /// </summary>
        private List<FileInformation> ValidateLocalFiles(List<FileInformation> manifestList, out long totalBytesToDownload)
        {
            var downloadQueue = new List<FileInformation>();
            totalBytesToDownload = 0;

            foreach (var manifestInfo in manifestList)
            {
                string localRelative = NormalizeLocalPath(manifestInfo.FileName);
                string localFull = Path.Combine(Settings.P_Client, localRelative);

                var localInfo = GetLocalFileInformation(localFull, manifestInfo.FileName);

                if (localInfo == null || manifestInfo.Length != localInfo.Length || manifestInfo.Creation != localInfo.Creation)
                {
                    downloadQueue.Add(manifestInfo);
                    totalBytesToDownload += manifestInfo.Length;
                }
            }

            return downloadQueue;
        }

        private FileInformation GetLocalFileInformation(string fullPath, string relativeName)
        {
            if (!File.Exists(fullPath)) return null;

            var info = new FileInfo(fullPath);
            return new FileInformation
            {
                FileName = relativeName,
                Length = (int)info.Length,
                Creation = info.LastWriteTime
            };
        }

        /// <summary>
        /// Download a single file and stream-decompress it on-the-fly directly to disk.
        /// </summary>
        private async Task DownloadFileAsync(FileInformation fileInfo, string tempDirPath, CancellationToken cancellationToken)
        {
            // STRICT CASE-SENSITIVITY MANDATE: 
            // Preserve manifest path casing perfectly in both the request URL and local file hierarchy.
            string serverPath = NormalizeUrlPath(fileInfo.FileName);
            string localRelative = NormalizeLocalPath(fileInfo.FileName);
            
            bool isCompressed = (serverPath != "PList.gz" && (fileInfo.Compressed != fileInfo.Length || fileInfo.Compressed == 0));
            if (isCompressed)
            {
                serverPath += ".gz";
            }

            string url = $"{Settings.P_Host}{serverPath}";
            string tempFilePath = Path.Combine(tempDirPath, localRelative);

            string tempFileDir = Path.GetDirectoryName(tempFilePath);
            if (!string.IsNullOrEmpty(tempFileDir) && !Directory.Exists(tempFileDir))
            {
                Directory.CreateDirectory(tempFileDir);
            }

            // Resume Function: Check if the file is already fully downloaded in temp directory
            if (File.Exists(tempFilePath))
            {
                var fi = new FileInfo(tempFilePath);
                if (fi.Length == fileInfo.Length)
                {
                    Interlocked.Add(ref _completedBytes, fileInfo.Length);
                    Interlocked.Increment(ref _filesDownloadedCount);
                    TriggerProgressUpdate(fileInfo.FileName, $"Resumed {fileInfo.FileName}");
                    return;
                }
            }

            using var request = CreateRequest(url);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
            {
                Stream sourceStream = responseStream;
                if (isCompressed)
                {
                    // Stream-decompress directly in memory to target stream
                    sourceStream = new GZipStream(responseStream, CompressionMode.Decompress);
                }

                using (sourceStream)
                using (var fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                {
                    // MODERN DECOMPRESSION & NETWORKING MANDATES:
                    // Decompress on-the-fly directly to target file stream using memory buffer from ArrayPool.
                    // Absolutely avoids allocating large intermediate byte arrays!
                    byte[] buffer = ArrayPool<byte>.Shared.Rent(8192);
                    try
                    {
                        int bytesRead;
                        while ((bytesRead = await sourceStream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken).ConfigureAwait(false)) > 0)
                        {
                            await fs.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);
                            Interlocked.Add(ref _completedBytes, bytesRead);
                            TriggerProgressUpdate(fileInfo.FileName, $"Downloading {fileInfo.FileName}...");
                        }
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
                }
            }

            Interlocked.Increment(ref _filesDownloadedCount);
            TriggerProgressUpdate(fileInfo.FileName, $"Finished downloading {fileInfo.FileName}");
        }

        /// <summary>
        /// Self-update check logic that identifies and handles updates to the executable itself.
        /// </summary>
        public static bool CheckSelfUpdate()
        {
            string exeDir = AppContext.BaseDirectory;
            string fromName = Path.Combine(exeDir, "AutoPatcher.gz");
            string toName = Environment.ProcessPath ?? Path.Combine(exeDir, "AutoPatcher.exe");

            if (!File.Exists(fromName)) return false;

            Console.WriteLine($"[Self-Update] Found self-update payload: {fromName}");

            string processName = Path.GetFileNameWithoutExtension(toName);
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                foreach (var p in processes)
                {
                    if (p.Id != Environment.ProcessId)
                    {
                        try { p.Kill(); } catch { }
                    }
                }
            }

            try
            {
                byte[] rawBytes = File.ReadAllBytes(fromName);
                byte[] decompressedBytes = DecompressBytes(rawBytes);

                // Handle process locking by using the safe renaming strategy
                if (File.Exists(toName))
                {
                    string oldBackup = toName + ".patch_old";
                    if (File.Exists(oldBackup))
                    {
                        try { File.Delete(oldBackup); } catch { }
                    }
                    try
                    {
                        File.Move(toName, oldBackup);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Self-Update] Backup rename failed for {toName}: {ex.Message}. Trying direct delete.");
                        File.Delete(toName);
                    }
                }

                File.WriteAllBytes(toName, decompressedBytes);
                File.Delete(fromName);

                Console.WriteLine($"[Self-Update] Successfully patched and restarting: {toName}");

                Process.Start(new ProcessStartInfo
                {
                    FileName = toName,
                    Arguments = "Auto",
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Self-Update] Error during self-update operation: {ex}");
                return false;
            }
        }

        public static byte[] DecompressBytes(byte[] gzipData)
        {
            if (gzipData == null || gzipData.Length < 18)
                throw new InvalidDataException("Invalid GZip data.");

            int uncompressedSize = BitConverter.ToInt32(gzipData, gzipData.Length - 4);
            if (uncompressedSize < 0)
                throw new InvalidDataException("Invalid GZip size.");

            byte[] decompressed = new byte[uncompressedSize];
            using var ms = new MemoryStream(gzipData, writable: false);
            using var gzip = new GZipStream(ms, CompressionMode.Decompress);

            int bytesRead;
            int offset = 0;
            while ((bytesRead = gzip.Read(decompressed, offset, decompressed.Length - offset)) > 0)
            {
                offset += bytesRead;
            }

            return decompressed;
        }

        /// <summary>
        /// Safely clean up obsolete files that are no longer part of the server manifest.
        /// </summary>
        public void CleanUpObsoleteFiles(List<FileInformation> manifestList)
        {
            string clientDir = Settings.P_Client;
            if (!Directory.Exists(clientDir)) return;

            string[] filePaths = Directory.GetFiles(clientDir, "*", SearchOption.AllDirectories);

            foreach (var filePath in filePaths)
            {
                string relativePath = Path.GetRelativePath(clientDir, filePath);
                string normalizedPath = relativePath.Replace('\\', '/');
                
                // Keep Screenshots
                if (normalizedPath.StartsWith("Screenshots", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Keep Temp Patch Folder
                if (normalizedPath.StartsWith(".patch_temp", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Keep User Data
                if (normalizedPath.StartsWith("Data/UserData", StringComparison.OrdinalIgnoreCase))
                    continue;

                string fileName = Path.GetFileName(filePath);
                
                // Keep system critical client configuration files, C# assemblies, native libs, configs, and executables
                string extension = Path.GetExtension(filePath).ToLower();
                if (fileName.Equals("Mir2Config.ini", StringComparison.OrdinalIgnoreCase) ||
                    fileName.Equals("Mir2Test.ini", StringComparison.OrdinalIgnoreCase) ||
                    fileName.Equals("Mir2Config.ini.patch_old", StringComparison.OrdinalIgnoreCase) ||
                    fileName.Equals("Mir2Test.ini.patch_old", StringComparison.OrdinalIgnoreCase) ||
                    fileName.Equals("KeyBinds.ini", StringComparison.OrdinalIgnoreCase) ||
                    fileName.Equals("Error.txt", StringComparison.OrdinalIgnoreCase) ||
                    fileName.Equals(Path.GetFileName(Environment.ProcessPath), StringComparison.OrdinalIgnoreCase) ||
                    filePath.EndsWith(".patch_old", StringComparison.OrdinalIgnoreCase) ||
                    extension == ".dll" || extension == ".so" || extension == ".pdb" ||
                    extension == ".json" || extension == ".config" || extension == ".ico" ||
                    filePath.Contains(".so.") ||
                    fileName.Equals("Client", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Match casing-insensitively for the manifest cleanup validation
                bool existsInManifest = false;
                foreach (var info in manifestList)
                {
                    string localRelative = NormalizeLocalPath(info.FileName);
                    if (relativePath.Equals(localRelative, StringComparison.OrdinalIgnoreCase))
                    {
                        existsInManifest = true;
                        break;
                    }
                }

                if (!existsInManifest)
                {
                    try
                    {
                        File.Delete(filePath);
                        Console.WriteLine($"[Patcher] Cleaned obsolete file: {relativePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Patcher] Failed to clean obsolete file {relativePath}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Purge any old backups leftover from previous running assembly replacements.
        /// </summary>
        public static void PurgeOldBackups()
        {
            try
            {
                string clientDir = Settings.P_Client;
                if (!Directory.Exists(clientDir)) return;

                var files = Directory.GetFiles(clientDir, "*.patch_old", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                        Console.WriteLine($"[Patcher] Purged leftover locked file backup: {Path.GetFileName(file)}");
                    }
                    catch
                    {
                        // File might still be locked by running processes
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Patcher] Failed to purge leftover backups: {ex.Message}");
            }
        }

        private HttpRequestMessage CreateRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Version = HttpVersion.Version30;
            request.VersionPolicy = HttpVersionPolicy.RequestVersionOrLower;

            // Add standard browser User-Agent header to bypass Cloudflare/CDN blocks
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

            if (Settings.P_NeedLogin)
            {
                string authInfo = $"{Settings.P_Login}:{Settings.P_Password}";
                string base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authInfo));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64);
            }

            return request;
        }

        private void TriggerProgressUpdate(string currentFile, string status)
        {
            double seconds = _downloadStopwatch?.Elapsed.TotalSeconds ?? 0;
            double speed = seconds > 0 ? _completedBytes / seconds : 0;

            var args = new PatcherProgressEventArgs
            {
                CurrentFileName = currentFile,
                FilesDownloaded = _filesDownloadedCount,
                TotalFiles = _totalFilesToDownload,
                BytesDownloaded = _completedBytes,
                TotalBytes = _totalBytesToDownload,
                SpeedBytesPerSecond = speed,
                StatusMessage = status
            };

            ProgressChanged?.Invoke(this, args);

            // Output to console with 500ms throttling to avoid console spam while displaying live progress
            lock (_consoleLock)
            {
                long now = Environment.TickCount64;
                if (now - _lastConsoleReportTime >= 500 || _filesDownloadedCount == _totalFilesToDownload)
                {
                    _lastConsoleReportTime = now;
                    Console.WriteLine($"[Patcher] Status: {status} | Progress: {_filesDownloadedCount}/{_totalFilesToDownload} files ({ConvertBytes(_completedBytes)}/{ConvertBytes(_totalBytesToDownload)}) | Speed: {ConvertBytes((long)speed)}/s");
                }
            }
        }

        private void LogProgress(string currentFile, int filesDownloaded, int totalFiles, long bytesDownloaded, long totalBytes, double speed, string status)
        {
            var args = new PatcherProgressEventArgs
            {
                CurrentFileName = currentFile,
                FilesDownloaded = filesDownloaded,
                TotalFiles = totalFiles,
                BytesDownloaded = bytesDownloaded,
                TotalBytes = totalBytes,
                SpeedBytesPerSecond = speed,
                StatusMessage = status
            };

            ProgressChanged?.Invoke(this, args);

            // Output to console for headless reporting
            Console.WriteLine($"[Patcher] Status: {status} | Progress: {filesDownloaded}/{totalFiles} files ({ConvertBytes(bytesDownloaded)}/{ConvertBytes(totalBytes)})");
        }

        private static string ConvertBytes(long byteCount)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            double count = byteCount;
            int index = 0;
            while (count >= 1024 && index < suffixes.Length - 1)
            {
                count /= 1024;
                index++;
            }
            return $"{count:0.##} {suffixes[index]}";
        }

        public static string NormalizeUrlPath(string filePath)
        {
            return filePath.Replace('\\', '/');
        }

        public static string NormalizeLocalPath(string filePath)
        {
            return filePath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
