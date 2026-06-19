using System.Net;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace AutoPatcherAdmin
{
    public sealed class PatchTransferProgress
    {
        public string FileName { get; init; } = string.Empty;
        public double OverallProgress { get; init; }
        public double FileProgress { get; init; }
        public long BytesPerSecond { get; init; }
        public int RemainingFiles { get; init; }
        public string Action { get; init; } = string.Empty;
    }

    public interface IPatchTransport : IDisposable
    {
        event EventHandler<PatchTransferProgress>? ProgressChanged;

        bool FileExists(string remotePath);
        bool DirectoryExists(string remotePath);
        IReadOnlyList<string> EnumerateFiles(string remoteDirectory, CancellationToken cancellationToken = default);
        IReadOnlyList<string> EnumerateDirectories(string remoteDirectory, CancellationToken cancellationToken = default);
        long GetFileLength(string remotePath);
        byte[] DownloadFile(string remotePath);
        void DownloadFiles(IEnumerable<string> remotePaths, string localRoot, CancellationToken cancellationToken = default);
        void UploadDirectory(string localRoot, string remoteRoot, IReadOnlyList<string> relativePaths, string action, CancellationToken cancellationToken = default);
        void EnsureDirectory(string remoteDirectory);
        void MoveFile(string sourcePath, string destinationPath);
        void DeleteFile(string remotePath);
        void DeleteDirectory(string remoteDirectory);
        void DeleteEmptyDirectory(string remoteDirectory);
    }

    public static class PatchTransportFactory
    {
        public static IPatchTransport Create()
        {
            var protocol = (Settings.Protocol ?? string.Empty).Trim();
            var uri = new Uri(Settings.Host);

            if (protocol.Equals("SFtp", StringComparison.OrdinalIgnoreCase) ||
                protocol.Equals("Sftp", StringComparison.OrdinalIgnoreCase) ||
                uri.Scheme.Equals("sftp", StringComparison.OrdinalIgnoreCase))
            {
                return new SftpPatchTransport(uri, Settings.Login, Settings.Password, Settings.Port);
            }

            if (protocol.Equals("Http", StringComparison.OrdinalIgnoreCase) ||
                protocol.Equals("Https", StringComparison.OrdinalIgnoreCase) ||
                uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) ||
                uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                return new HttpPatchTransport(uri, Settings.Login, Settings.Password, Settings.Port);
            }

            return new FtpPatchTransport(uri, Settings.Login, Settings.Password, Settings.Port);
        }
    }

    internal abstract class PatchTransportBase : IPatchTransport
    {
        public event EventHandler<PatchTransferProgress>? ProgressChanged;

        public abstract bool FileExists(string remotePath);
        public abstract bool DirectoryExists(string remotePath);
        public abstract IReadOnlyList<string> EnumerateFiles(string remoteDirectory, CancellationToken cancellationToken = default);
        public virtual IReadOnlyList<string> EnumerateDirectories(string remoteDirectory, CancellationToken cancellationToken = default) => Array.Empty<string>();
        public abstract long GetFileLength(string remotePath);
        public abstract byte[] DownloadFile(string remotePath);
        public abstract void EnsureDirectory(string remoteDirectory);
        public abstract void MoveFile(string sourcePath, string destinationPath);
        public abstract void DeleteFile(string remotePath);
        public abstract void DeleteDirectory(string remoteDirectory);
        public abstract void DeleteEmptyDirectory(string remoteDirectory);
        public abstract void Dispose();

        public virtual void DownloadFiles(IEnumerable<string> remotePaths, string localRoot, CancellationToken cancellationToken = default)
        {
            var paths = remotePaths.ToList();
            int remaining = paths.Count;
            int completed = 0;
            string hostRoot = new Uri(Settings.Host).AbsolutePath.Trim('/');

            foreach (string remotePath in paths)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string relativePath = NormalizeRemotePath(remotePath).TrimStart('/');
                if (!string.IsNullOrEmpty(hostRoot) &&
                    relativePath.StartsWith(hostRoot + "/", StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = relativePath.Substring(hostRoot.Length + 1);
                }

                string localPath = Path.Combine(localRoot, relativePath);
                string? localDirectory = Path.GetDirectoryName(localPath);
                if (!string.IsNullOrEmpty(localDirectory) && !Directory.Exists(localDirectory))
                    Directory.CreateDirectory(localDirectory);

                byte[] data = DownloadFile(remotePath);
                File.WriteAllBytes(localPath, data);

                completed++;
                remaining--;
                Report("Downloading", remotePath, completed / (double)Math.Max(paths.Count, 1), 1, 0, remaining);
            }
        }

        public virtual void UploadDirectory(string localRoot, string remoteRoot, IReadOnlyList<string> relativePaths, string action, CancellationToken cancellationToken = default)
        {
            int remaining = relativePaths.Count;
            int completed = 0;

            foreach (string relativePath in relativePaths)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string localPath = Path.Combine(localRoot, relativePath);
                string remotePath = CombineRemotePath(remoteRoot, relativePath);
                string remoteDirectory = GetRemoteDirectory(remotePath);

                EnsureDirectory(remoteDirectory);
                UploadFile(localPath, remotePath, action, completed, relativePaths.Count, remaining);

                completed++;
                remaining--;
                Report(action, relativePath, completed / (double)Math.Max(relativePaths.Count, 1), 1, 0, remaining);
            }
        }

        protected abstract void UploadFile(string localPath, string remotePath, string action, int completedFiles, int totalFiles, int remainingFiles);

        protected void Report(string action, string fileName, double overall, double fileProgress, long bytesPerSecond, int remainingFiles)
        {
            ProgressChanged?.Invoke(this, new PatchTransferProgress
            {
                Action = action,
                FileName = fileName,
                OverallProgress = overall,
                FileProgress = fileProgress,
                BytesPerSecond = bytesPerSecond,
                RemainingFiles = remainingFiles
            });
        }

        protected static string CombineRemotePath(string rootPath, string fileName)
        {
            rootPath = (rootPath ?? string.Empty).Replace('\\', '/').TrimEnd('/');
            fileName = (fileName ?? string.Empty).Replace('\\', '/').TrimStart('/');
            return string.IsNullOrEmpty(rootPath) ? "/" + fileName : rootPath + "/" + fileName;
        }

        protected static string GetRemoteDirectory(string remotePath)
        {
            remotePath = NormalizeRemotePath(remotePath);
            int index = remotePath.LastIndexOf('/');
            return index <= 0 ? "/" : remotePath.Substring(0, index);
        }

        protected static string NormalizeRemotePath(string path)
        {
            path = (path ?? string.Empty).Replace('\\', '/');
            return path.StartsWith("/") ? path : "/" + path;
        }

        protected static string SafeNormalizePath(string path)
        {
            string p = NormalizeRemotePath(path).TrimEnd('/');
            return string.IsNullOrEmpty(p) ? "/" : p;
        }
    }

    internal sealed class SftpPatchTransport : PatchTransportBase
    {
        private readonly SftpClient _client;

        public SftpPatchTransport(Uri host, string userName, string password, int portOverride)
        {
            int port = portOverride > 0 ? portOverride : host.IsDefaultPort ? 22 : host.Port;
            _client = new SftpClient(host.Host, port, userName, password);
            _client.Connect();
        }

        public override bool FileExists(string remotePath)
        {
            return _client.Exists(NormalizeRemotePath(remotePath));
        }

        public override bool DirectoryExists(string remotePath)
        {
            remotePath = SafeNormalizePath(remotePath);
            return _client.Exists(remotePath) && _client.GetAttributes(remotePath).IsDirectory;
        }

        public override IReadOnlyList<string> EnumerateFiles(string remoteDirectory, CancellationToken cancellationToken = default)
        {
            var files = new List<string>();
            EnumerateFilesRecursive(SafeNormalizePath(remoteDirectory), files, cancellationToken);
            return files;
        }

        public override long GetFileLength(string remotePath)
        {
            return _client.GetAttributes(NormalizeRemotePath(remotePath)).Size;
        }

        public override byte[] DownloadFile(string remotePath)
        {
            using var stream = new MemoryStream();
            _client.DownloadFile(NormalizeRemotePath(remotePath), stream);
            return stream.ToArray();
        }

        public override void EnsureDirectory(string remoteDirectory)
        {
            remoteDirectory = NormalizeRemotePath(remoteDirectory).TrimEnd('/');
            if (remoteDirectory == string.Empty || remoteDirectory == "/") return;

            string current = "/";
            foreach (string part in remoteDirectory.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                current = current == "/" ? current + part : current + "/" + part;
                if (!_client.Exists(current))
                    _client.CreateDirectory(current);
            }
        }

        public override void MoveFile(string sourcePath, string destinationPath)
        {
            sourcePath = NormalizeRemotePath(sourcePath);
            destinationPath = NormalizeRemotePath(destinationPath);
            if (_client.Exists(destinationPath))
                _client.DeleteFile(destinationPath);
            _client.RenameFile(sourcePath, destinationPath);
        }

        public override void DeleteFile(string remotePath)
        {
            remotePath = NormalizeRemotePath(remotePath);
            if (_client.Exists(remotePath))
                _client.DeleteFile(remotePath);
        }

        public override void DeleteDirectory(string remoteDirectory)
        {
            remoteDirectory = SafeNormalizePath(remoteDirectory);
            if (!_client.Exists(remoteDirectory)) return;
            DeleteDirectoryRecursive(remoteDirectory);
        }

        public override void DeleteEmptyDirectory(string remoteDirectory)
        {
            remoteDirectory = SafeNormalizePath(remoteDirectory);
            if (_client.Exists(remoteDirectory))
                _client.DeleteDirectory(remoteDirectory);
        }

        protected override void UploadFile(string localPath, string remotePath, string action, int completedFiles, int totalFiles, int remainingFiles)
        {
            using var stream = File.OpenRead(localPath);
            long started = Environment.TickCount64;
            _client.UploadFile(stream, NormalizeRemotePath(remotePath), true, uploaded =>
            {
                double fileProgress = stream.Length == 0 ? 1 : uploaded / (double)stream.Length;
                double overall = (completedFiles + fileProgress) / Math.Max(totalFiles, 1);
                long elapsed = Math.Max(Environment.TickCount64 - started, 1);
                long cps = (long)(uploaded / (elapsed / 1000.0));
                Report(action, localPath, overall, fileProgress, cps, remainingFiles);
            });
        }

        public override void Dispose()
        {
            _client.Dispose();
        }

        private void DeleteDirectoryRecursive(string remoteDirectory)
        {
            foreach (SftpFile file in _client.ListDirectory(remoteDirectory))
            {
                if (file.Name == "." || file.Name == "..") continue;

                if (file.IsDirectory)
                    DeleteDirectoryRecursive(file.FullName);
                else
                    _client.DeleteFile(file.FullName);
            }

            _client.DeleteDirectory(remoteDirectory);
        }

        public override IReadOnlyList<string> EnumerateDirectories(string remoteDirectory, CancellationToken cancellationToken = default)
        {
            var dirs = new List<string>();
            EnumerateDirectoriesRecursive(SafeNormalizePath(remoteDirectory), dirs, cancellationToken);
            return dirs;
        }

        private void EnumerateFilesRecursive(string remoteDirectory, List<string> files, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (SftpFile file in _client.ListDirectory(remoteDirectory))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (file.Name == "." || file.Name == "..") continue;

                if (file.IsDirectory)
                    EnumerateFilesRecursive(file.FullName, files, cancellationToken);
                else
                    files.Add(NormalizeRemotePath(file.FullName));
            }
        }

        private void EnumerateDirectoriesRecursive(string remoteDirectory, List<string> dirs, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (SftpFile file in _client.ListDirectory(remoteDirectory))
            {
                if (file.Name == "." || file.Name == "..") continue;
                if (!file.IsDirectory) continue;
                EnumerateDirectoriesRecursive(file.FullName, dirs, cancellationToken);
                dirs.Add(NormalizeRemotePath(file.FullName)); // deepest first
            }
        }
    }

    internal sealed class FtpPatchTransport : PatchTransportBase
    {
        private readonly Uri _host;
        private readonly NetworkCredential _credentials;
        private readonly int _portOverride;

        public FtpPatchTransport(Uri host, string userName, string password, int portOverride)
        {
            _host = host;
            _credentials = new NetworkCredential(userName, password);
            _portOverride = portOverride;
        }

        public override bool FileExists(string remotePath)
        {
            try
            {
                GetFileLength(remotePath);
                return true;
            }
            catch (WebException ex) when ((ex.Response as FtpWebResponse)?.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
            {
                return false;
            }
        }

        public override bool DirectoryExists(string remotePath)
        {
            try
            {
                using var response = (FtpWebResponse)CreateRequest(remotePath, WebRequestMethods.Ftp.ListDirectory).GetResponse();
                return true;
            }
            catch (WebException ex) when ((ex.Response as FtpWebResponse)?.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
            {
                return false;
            }
        }

        public override IReadOnlyList<string> EnumerateFiles(string remoteDirectory, CancellationToken cancellationToken = default)
        {
            var files = new List<string>();
            EnumerateFilesRecursive(NormalizeRemotePath(remoteDirectory).TrimEnd('/'), files, cancellationToken);
            return files;
        }

        public override long GetFileLength(string remotePath)
        {
            var request = CreateRequest(remotePath, WebRequestMethods.Ftp.GetFileSize);
            using var response = (FtpWebResponse)request.GetResponse();
            return response.ContentLength;
        }

        public override byte[] DownloadFile(string remotePath)
        {
            var request = CreateRequest(remotePath, WebRequestMethods.Ftp.DownloadFile);
            using var response = (FtpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();
            using var output = new MemoryStream();
            stream?.CopyTo(output);
            return output.ToArray();
        }

        public override void EnsureDirectory(string remoteDirectory)
        {
            remoteDirectory = NormalizeRemotePath(remoteDirectory).TrimEnd('/');
            if (remoteDirectory == string.Empty || remoteDirectory == "/") return;

            string current = "/";
            foreach (string part in remoteDirectory.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                current = current == "/" ? current + part : current + "/" + part;
                try
                {
                    using var response = (FtpWebResponse)CreateRequest(current, WebRequestMethods.Ftp.MakeDirectory).GetResponse();
                }
                catch (WebException ex) when ((ex.Response as FtpWebResponse)?.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                }
            }
        }

        public override void MoveFile(string sourcePath, string destinationPath)
        {
            DeleteFile(destinationPath);
            var request = CreateRequest(sourcePath, WebRequestMethods.Ftp.Rename);
            request.RenameTo = NormalizeRemotePath(destinationPath);
            using var response = (FtpWebResponse)request.GetResponse();
        }

        public override void DeleteFile(string remotePath)
        {
            if (!FileExists(remotePath)) return;
            using var response = (FtpWebResponse)CreateRequest(remotePath, WebRequestMethods.Ftp.DeleteFile).GetResponse();
        }

        public override void DeleteDirectory(string remoteDirectory)
        {
            DeleteDirectoryRecursive(SafeNormalizePath(remoteDirectory));
        }

        public override void DeleteEmptyDirectory(string remoteDirectory)
        {
            remoteDirectory = SafeNormalizePath(remoteDirectory);
            if (!DirectoryExists(remoteDirectory)) return;
            using var response = (FtpWebResponse)CreateRequest(remoteDirectory, WebRequestMethods.Ftp.RemoveDirectory).GetResponse();
        }

        private void DeleteDirectoryRecursive(string remoteDirectory)
        {
            foreach (string name in ListDirectoryNames(remoteDirectory))
            {
                if (string.IsNullOrWhiteSpace(name) || name == "." || name == "..") continue;
                string child = name.StartsWith("/", StringComparison.Ordinal)
                    ? NormalizeRemotePath(name)
                    : CombineRemotePath(remoteDirectory, name);
                if (DirectoryExists(child))
                    DeleteDirectoryRecursive(child);
                else
                    DeleteFile(child);
            }
            using var response = (FtpWebResponse)CreateRequest(remoteDirectory, WebRequestMethods.Ftp.RemoveDirectory).GetResponse();
        }

        protected override void UploadFile(string localPath, string remotePath, string action, int completedFiles, int totalFiles, int remainingFiles)
        {
            byte[] data = File.ReadAllBytes(localPath);
            var request = CreateRequest(remotePath, WebRequestMethods.Ftp.UploadFile);
            request.ContentLength = data.Length;

            long started = Environment.TickCount64;
            using (var stream = request.GetRequestStream())
            {
                int offset = 0;
                while (offset < data.Length)
                {
                    int count = Math.Min(64 * 1024, data.Length - offset);
                    stream.Write(data, offset, count);
                    offset += count;

                    double fileProgress = data.Length == 0 ? 1 : offset / (double)data.Length;
                    double overall = (completedFiles + fileProgress) / Math.Max(totalFiles, 1);
                    long elapsed = Math.Max(Environment.TickCount64 - started, 1);
                    long cps = (long)(offset / (elapsed / 1000.0));
                    Report(action, localPath, overall, fileProgress, cps, remainingFiles);
                }
            }

            using var response = (FtpWebResponse)request.GetResponse();
        }

        public override void Dispose()
        {
        }

        private FtpWebRequest CreateRequest(string remotePath, string method)
        {
            var builder = new UriBuilder(_host)
            {
                Path = NormalizeRemotePath(remotePath)
            };
            if (_portOverride > 0)
                builder.Port = _portOverride;
            var uri = builder.Uri;
#pragma warning disable SYSLIB0014 // FtpWebRequest is the built-in .NET FTP client API.
            var request = (FtpWebRequest)WebRequest.Create(uri);
#pragma warning restore SYSLIB0014
            request.Method = method;
            request.Credentials = _credentials;
            request.UseBinary = true;
            request.KeepAlive = false;
            return request;
        }

        public override IReadOnlyList<string> EnumerateDirectories(string remoteDirectory, CancellationToken cancellationToken = default)
        {
            var dirs = new List<string>();
            EnumerateDirectoriesRecursive(SafeNormalizePath(remoteDirectory), dirs, cancellationToken);
            return dirs;
        }

        private void EnumerateFilesRecursive(string remoteDirectory, List<string> files, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (string name in ListDirectoryNames(remoteDirectory))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (string.IsNullOrWhiteSpace(name) || name == "." || name == "..") continue;

                string child = name.StartsWith("/", StringComparison.Ordinal)
                    ? NormalizeRemotePath(name)
                    : CombineRemotePath(remoteDirectory, name);
                if (FileExists(child))
                {
                    files.Add(NormalizeRemotePath(child));
                    continue;
                }

                if (DirectoryExists(child))
                    EnumerateFilesRecursive(child, files, cancellationToken);
            }
        }

        private void EnumerateDirectoriesRecursive(string remoteDirectory, List<string> dirs, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (string name in ListDirectoryNames(remoteDirectory))
            {
                if (string.IsNullOrWhiteSpace(name) || name == "." || name == "..") continue;
                string child = name.StartsWith("/", StringComparison.Ordinal)
                    ? NormalizeRemotePath(name)
                    : CombineRemotePath(remoteDirectory, name);
                if (!DirectoryExists(child)) continue;
                EnumerateDirectoriesRecursive(child, dirs, cancellationToken);
                dirs.Add(NormalizeRemotePath(child)); // deepest first
            }
        }

        private IEnumerable<string> ListDirectoryNames(string remoteDirectory)
        {
            var request = CreateRequest(remoteDirectory, WebRequestMethods.Ftp.ListDirectory);
            using var response = (FtpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream ?? Stream.Null);

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line != null)
                    yield return line.Trim();
            }
        }
    }

    internal sealed class HttpPatchTransport : PatchTransportBase
    {
        private readonly Uri _host;
        private readonly HttpClient _client;
        private readonly int _portOverride;

        public HttpPatchTransport(Uri host, string userName, string password, int portOverride)
        {
            _host = host;
            _portOverride = portOverride;
            var handler = new HttpClientHandler();
            if (!string.IsNullOrEmpty(userName))
                handler.Credentials = new NetworkCredential(userName, password);

            _client = new HttpClient(handler);

            if (!string.IsNullOrEmpty(userName))
            {
                string token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{userName}:{password}"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
            }
        }

        public override bool FileExists(string remotePath)
        {
            using var response = _client.Send(new HttpRequestMessage(HttpMethod.Head, BuildUri(remotePath)));
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        public override bool DirectoryExists(string remotePath)
        {
            using var request = new HttpRequestMessage(new HttpMethod("PROPFIND"), BuildUri(remotePath));
            request.Headers.TryAddWithoutValidation("Depth", "0");
            using var response = _client.Send(request);
            return response.IsSuccessStatusCode;
        }

        public override IReadOnlyList<string> EnumerateFiles(string remoteDirectory, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(new HttpMethod("PROPFIND"), BuildUri(remoteDirectory));
            request.Headers.TryAddWithoutValidation("Depth", "infinity");

            using var response = _client.Send(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            string rootPath = BuildUri(remoteDirectory).AbsolutePath.TrimEnd('/');
            string xml = response.Content.ReadAsStringAsync(cancellationToken).GetAwaiter().GetResult();
            XDocument document = XDocument.Parse(xml);
            var files = new List<string>();

            foreach (var href in document.Descendants().Where(x => x.Name.LocalName.Equals("href", StringComparison.OrdinalIgnoreCase)))
            {
                cancellationToken.ThrowIfCancellationRequested();

                string value = Uri.UnescapeDataString((href.Value ?? string.Empty).Trim());
                if (string.IsNullOrWhiteSpace(value) || value.EndsWith("/", StringComparison.Ordinal))
                    continue;

                if (Uri.TryCreate(value, UriKind.Absolute, out var absolute))
                    value = absolute.AbsolutePath;

                if (!value.StartsWith(rootPath + "/", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(value, rootPath, StringComparison.OrdinalIgnoreCase))
                    continue;

                files.Add(NormalizeRemotePath(value));
            }

            return files.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }

        public override long GetFileLength(string remotePath)
        {
            using var response = _client.Send(new HttpRequestMessage(HttpMethod.Head, BuildUri(remotePath)));
            response.EnsureSuccessStatusCode();
            return response.Content.Headers.ContentLength ?? 0;
        }

        public override byte[] DownloadFile(string remotePath)
        {
            return _client.GetByteArrayAsync(BuildUri(remotePath)).GetAwaiter().GetResult();
        }

        public override void EnsureDirectory(string remoteDirectory)
        {
            // HTTP(S) publishing expects a PUT/WebDAV-capable endpoint that creates parent paths,
            // or an endpoint that treats path creation as implicit.
        }

        public override void MoveFile(string sourcePath, string destinationPath)
        {
            var request = new HttpRequestMessage(new HttpMethod("MOVE"), BuildUri(sourcePath));
            request.Headers.TryAddWithoutValidation("Destination", BuildUri(destinationPath).ToString());
            using var response = _client.Send(request);
            response.EnsureSuccessStatusCode();
        }

        public override void DeleteFile(string remotePath)
        {
            using var response = _client.DeleteAsync(BuildUri(remotePath)).GetAwaiter().GetResult();
            if (response.StatusCode != HttpStatusCode.NotFound)
                response.EnsureSuccessStatusCode();
        }

        public override void DeleteDirectory(string remoteDirectory)
        {
            using var response = _client.DeleteAsync(BuildUri(remoteDirectory)).GetAwaiter().GetResult();
            if (response.StatusCode != HttpStatusCode.NotFound)
                response.EnsureSuccessStatusCode();
        }

        public override void DeleteEmptyDirectory(string remoteDirectory)
        {
            if (!IsDirectoryEmpty(remoteDirectory)) return;
            DeleteDirectory(remoteDirectory);
        }

        private bool IsDirectoryEmpty(string remoteDirectory)
        {
            using var request = new HttpRequestMessage(new HttpMethod("PROPFIND"), BuildUri(remoteDirectory));
            request.Headers.TryAddWithoutValidation("Depth", "1");

            using var response = _client.Send(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();

            string rootPath = BuildUri(remoteDirectory).AbsolutePath.TrimEnd('/');
            string xml = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            XDocument document = XDocument.Parse(xml);

            foreach (var href in document.Descendants().Where(x => x.Name.LocalName.Equals("href", StringComparison.OrdinalIgnoreCase)))
            {
                string value = Uri.UnescapeDataString((href.Value ?? string.Empty).Trim()).TrimEnd('/');
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                if (Uri.TryCreate(value, UriKind.Absolute, out var absolute))
                    value = absolute.AbsolutePath.TrimEnd('/');

                if (!string.Equals(value, rootPath, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        protected override void UploadFile(string localPath, string remotePath, string action, int completedFiles, int totalFiles, int remainingFiles)
        {
            byte[] data = File.ReadAllBytes(localPath);
            long started = Environment.TickCount64;
            using var content = new ByteArrayContent(data);
            using var response = _client.PutAsync(BuildUri(remotePath), content).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            long elapsed = Math.Max(Environment.TickCount64 - started, 1);
            long cps = (long)(data.Length / (elapsed / 1000.0));
            double overall = (completedFiles + 1) / (double)Math.Max(totalFiles, 1);
            Report(action, localPath, overall, 1, cps, remainingFiles);
        }

        public override void Dispose()
        {
            _client.Dispose();
        }

        private Uri BuildUri(string remotePath)
        {
            var builder = new UriBuilder(_host)
            {
                Path = NormalizeRemotePath(remotePath)
            };
            if (_portOverride > 0)
                builder.Port = _portOverride;

            return builder.Uri;
        }
    }
}
