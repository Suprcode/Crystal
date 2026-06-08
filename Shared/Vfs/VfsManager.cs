using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Shared.Vfs
{
    public static class VfsManager
    {
        private static readonly Dictionary<string, string> _vfsIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, string> _vfsDirIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _lock = new object();
        private static bool _initialized = false;

        static VfsManager()
        {
            Initialize(Directory.GetCurrentDirectory());
        }

        public static void Initialize(string rootDir = null)
        {
            lock (_lock)
            {
                if (_initialized) return;
                if (string.IsNullOrEmpty(rootDir))
                {
                    rootDir = Directory.GetCurrentDirectory();
                }

                try
                {
                    if (!Directory.Exists(rootDir)) return;

                    // Index directories
                    var dirs = Directory.GetDirectories(rootDir, "*", SearchOption.AllDirectories);
                    foreach (var dir in dirs)
                    {
                        var normalized = Path.GetFullPath(dir).Replace('\\', '/');
                        _vfsDirIndex[normalized] = dir;
                    }

                    // Index files
                    var files = Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        var normalized = Path.GetFullPath(file).Replace('\\', '/');
                        _vfsIndex[normalized] = file;
                    }

                    _initialized = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[VFS] Indexing error: {ex}");
                }
            }
        }

        public static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return path.Replace('\\', '/');
        }

        public static string Resolve(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            var normalizedInput = NormalizePath(path);
            var fullPath = Path.GetFullPath(normalizedInput).Replace('\\', '/');

            lock (_lock)
            {
                if (_vfsIndex.TryGetValue(fullPath, out var resolvedPath))
                {
                    return resolvedPath;
                }
            }

            if (File.Exists(path))
            {
                lock (_lock)
                {
                    _vfsIndex[fullPath] = path;
                }
                return path;
            }

            return path;
        }

        public static string ResolveDir(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            var normalizedInput = NormalizePath(path);
            var fullPath = Path.GetFullPath(normalizedInput).Replace('\\', '/');

            lock (_lock)
            {
                if (_vfsDirIndex.TryGetValue(fullPath, out var resolvedPath))
                {
                    return resolvedPath;
                }
            }

            if (Directory.Exists(path))
            {
                lock (_lock)
                {
                    _vfsDirIndex[fullPath] = path;
                }
                return path;
            }

            return path;
        }

        public static bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            var resolved = Resolve(path);
            return File.Exists(resolved);
        }

        public static bool DirectoryExists(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            var resolved = ResolveDir(path);
            return Directory.Exists(resolved);
        }

        public static FileStream OpenRead(string path)
        {
            var resolved = Resolve(path);
            return File.OpenRead(resolved);
        }

        public static byte[] ReadAllBytes(string path)
        {
            var resolved = Resolve(path);
            return File.ReadAllBytes(resolved);
        }

        public static string[] ReadAllLines(string path)
        {
            var resolved = Resolve(path);
            return File.ReadAllLines(resolved);
        }

        public static string ReadAllText(string path)
        {
            var resolved = Resolve(path);
            return File.ReadAllText(resolved);
        }

        public static void WriteAllText(string path, string contents)
        {
            var resolved = Resolve(path);
            File.WriteAllText(resolved, contents);
            RegisterFile(path, resolved);
        }

        public static void WriteAllText(string path, string contents, System.Text.Encoding encoding)
        {
            var resolved = Resolve(path);
            File.WriteAllText(resolved, contents, encoding);
            RegisterFile(path, resolved);
        }

        public static void WriteAllLines(string path, IEnumerable<string> contents)
        {
            var resolved = Resolve(path);
            File.WriteAllLines(resolved, contents);
            RegisterFile(path, resolved);
        }

        public static void WriteAllLines(string path, IEnumerable<string> contents, System.Text.Encoding encoding)
        {
            var resolved = Resolve(path);
            File.WriteAllLines(resolved, contents, encoding);
            RegisterFile(path, resolved);
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            var resolved = Resolve(path);
            File.WriteAllBytes(resolved, bytes);
            RegisterFile(path, resolved);
        }

        public static DateTime GetLastWriteTime(string path)
        {
            var resolved = Resolve(path);
            return File.GetLastWriteTime(resolved);
        }

        public static StreamWriter AppendText(string path)
        {
            var resolved = Resolve(path);
            return File.AppendText(resolved);
        }

        public static IEnumerable<string> ReadLines(string path)
        {
            var resolved = Resolve(path);
            return File.ReadLines(resolved);
        }

        public static void RegisterFile(string path, string resolvedPath)
        {
            if (string.IsNullOrEmpty(path)) return;
            var normalized = Path.GetFullPath(NormalizePath(path)).Replace('\\', '/');
            lock (_lock)
            {
                _vfsIndex[normalized] = resolvedPath;
            }
        }

        public static void UnregisterFile(string path, string resolvedPath = null)
        {
            if (string.IsNullOrEmpty(path)) return;
            var normalized = Path.GetFullPath(NormalizePath(path)).Replace('\\', '/');
            lock (_lock)
            {
                _vfsIndex.Remove(normalized);
                if (resolvedPath != null)
                {
                    var resolvedNormalized = Path.GetFullPath(NormalizePath(resolvedPath)).Replace('\\', '/');
                    _vfsIndex.Remove(resolvedNormalized);
                }
            }
        }

        public static void RegisterDir(string path, string resolvedPath)
        {
            if (string.IsNullOrEmpty(path)) return;
            var normalized = Path.GetFullPath(NormalizePath(path)).Replace('\\', '/');
            lock (_lock)
            {
                _vfsDirIndex[normalized] = resolvedPath;
            }
        }

        public static void UnregisterDir(string path, string resolvedPath = null)
        {
            if (string.IsNullOrEmpty(path)) return;
            var normalized = Path.GetFullPath(NormalizePath(path)).Replace('\\', '/');
            lock (_lock)
            {
                _vfsDirIndex.Remove(normalized);
                if (resolvedPath != null)
                {
                    var resolvedNormalized = Path.GetFullPath(NormalizePath(resolvedPath)).Replace('\\', '/');
                    _vfsDirIndex.Remove(resolvedNormalized);
                }
            }
        }

        public static Regex ConvertGlobToRegex(string wildcard, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(wildcard))
            {
                return new Regex("^.*$");
            }
            string regexPattern = "^" + Regex.Escape(wildcard).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            return new Regex(regexPattern, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        public static string[] GetFilesMatching(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var resolvedDir = ResolveDir(path);
            if (!Directory.Exists(resolvedDir))
            {
                return Array.Empty<string>();
            }

            var regex = ConvertGlobToRegex(searchPattern);
            var results = new List<string>();
            var normalizedDirPrefix = Path.GetFullPath(NormalizePath(resolvedDir)).Replace('\\', '/');
            if (!normalizedDirPrefix.EndsWith("/"))
            {
                normalizedDirPrefix += "/";
            }

            lock (_lock)
            {
                foreach (var kvp in _vfsIndex)
                {
                    var fileFullPath = kvp.Key;
                    var physicalPath = kvp.Value;

                    if (fileFullPath.StartsWith(normalizedDirPrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        var relativePart = fileFullPath.Substring(normalizedDirPrefix.Length);
                        
                        if (searchOption == SearchOption.TopDirectoryOnly && relativePart.Contains('/'))
                        {
                            continue;
                        }

                        var fileName = Path.GetFileName(physicalPath);
                        if (regex.IsMatch(fileName))
                        {
                            results.Add(physicalPath);
                        }
                    }
                }
            }

            return results.ToArray();
        }

        public static string[] GetDirectoriesMatching(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var resolvedDir = ResolveDir(path);
            if (!Directory.Exists(resolvedDir))
            {
                return Array.Empty<string>();
            }

            var regex = ConvertGlobToRegex(searchPattern);
            var results = new List<string>();
            var normalizedDirPrefix = Path.GetFullPath(NormalizePath(resolvedDir)).Replace('\\', '/');
            if (!normalizedDirPrefix.EndsWith("/"))
            {
                normalizedDirPrefix += "/";
            }

            lock (_lock)
            {
                foreach (var kvp in _vfsDirIndex)
                {
                    var dirFullPath = kvp.Key;
                    var physicalPath = kvp.Value;

                    if (dirFullPath.StartsWith(normalizedDirPrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        var relativePart = dirFullPath.Substring(normalizedDirPrefix.Length);
                        
                        if (searchOption == SearchOption.TopDirectoryOnly && relativePart.Contains('/'))
                        {
                            continue;
                        }

                        var dirName = Path.GetFileName(physicalPath);
                        if (regex.IsMatch(dirName))
                        {
                            results.Add(physicalPath);
                        }
                    }
                }
            }

            return results.ToArray();
        }
    }
}
