using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public static class File
    {
        internal static readonly Dictionary<string, string> _vfsIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        internal static readonly Dictionary<string, string> _vfsDirIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        static File()
        {
            BuildVfsIndex(System.IO.Directory.GetCurrentDirectory());
        }

        private static void BuildVfsIndex(string rootDir)
        {
            try
            {
                if (!System.IO.Directory.Exists(rootDir)) return;

                // Index directories
                var dirs = System.IO.Directory.GetDirectories(rootDir, "*", System.IO.SearchOption.AllDirectories);
                foreach (var dir in dirs)
                {
                    var normalized = System.IO.Path.GetFullPath(dir).Replace('\\', '/');
                    _vfsDirIndex[normalized] = dir;
                }

                // Index files
                var files = System.IO.Directory.GetFiles(rootDir, "*", System.IO.SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var normalized = System.IO.Path.GetFullPath(file).Replace('\\', '/');
                    _vfsIndex[normalized] = file;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VFS] Indexing error: {ex}");
            }
        }

        public static string Resolve(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');

            lock (_vfsIndex)
            {
                if (_vfsIndex.TryGetValue(fullPath, out var resolvedPath))
                {
                    return resolvedPath;
                }
            }

            if (System.IO.File.Exists(path))
            {
                lock (_vfsIndex)
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

            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');

            lock (_vfsDirIndex)
            {
                if (_vfsDirIndex.TryGetValue(fullPath, out var resolvedPath))
                {
                    return resolvedPath;
                }
            }

            if (System.IO.Directory.Exists(path))
            {
                lock (_vfsDirIndex)
                {
                    _vfsDirIndex[fullPath] = path;
                }
                return path;
            }

            return path;
        }

        public static bool Exists(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            var resolved = Resolve(path);
            return System.IO.File.Exists(resolved);
        }

        public static System.IO.FileStream Create(string path)
        {
            var resolved = Resolve(path);
            var fs = System.IO.File.Create(resolved);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (_vfsIndex)
            {
                _vfsIndex[fullPath] = resolved;
            }
            return fs;
        }

        public static void Delete(string path)
        {
            var resolved = Resolve(path);
            System.IO.File.Delete(resolved);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (_vfsIndex)
            {
                _vfsIndex.Remove(fullPath);
                var resolvedFullPath = System.IO.Path.GetFullPath(resolved).Replace('\\', '/');
                _vfsIndex.Remove(resolvedFullPath);
            }
        }

        public static void Copy(string sourceFileName, string destFileName)
        {
            var resolvedSource = Resolve(sourceFileName);
            var resolvedDest = Resolve(destFileName);
            System.IO.File.Copy(resolvedSource, resolvedDest);
            lock (_vfsIndex)
            {
                var normalizedDest = System.IO.Path.GetFullPath(destFileName.Replace('\\', '/')).Replace('\\', '/');
                _vfsIndex[normalizedDest] = resolvedDest;
            }
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            var resolvedSource = Resolve(sourceFileName);
            var resolvedDest = Resolve(destFileName);
            System.IO.File.Copy(resolvedSource, resolvedDest, overwrite);
            lock (_vfsIndex)
            {
                var normalizedDest = System.IO.Path.GetFullPath(destFileName.Replace('\\', '/')).Replace('\\', '/');
                _vfsIndex[normalizedDest] = resolvedDest;
            }
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            var resolvedSource = Resolve(sourceFileName);
            var resolvedDest = Resolve(destFileName);
            System.IO.File.Move(resolvedSource, resolvedDest);
            lock (_vfsIndex)
            {
                var normalizedSource = System.IO.Path.GetFullPath(sourceFileName.Replace('\\', '/')).Replace('\\', '/');
                _vfsIndex.Remove(normalizedSource);
                _vfsIndex.Remove(System.IO.Path.GetFullPath(resolvedSource).Replace('\\', '/'));

                var normalizedDest = System.IO.Path.GetFullPath(destFileName.Replace('\\', '/')).Replace('\\', '/');
                _vfsIndex[normalizedDest] = resolvedDest;
            }
        }

        public static void Move(string sourceFileName, string destFileName, bool overwrite)
        {
            var resolvedSource = Resolve(sourceFileName);
            var resolvedDest = Resolve(destFileName);
            System.IO.File.Move(resolvedSource, resolvedDest, overwrite);
            lock (_vfsIndex)
            {
                var normalizedSource = System.IO.Path.GetFullPath(sourceFileName.Replace('\\', '/')).Replace('\\', '/');
                _vfsIndex.Remove(normalizedSource);
                _vfsIndex.Remove(System.IO.Path.GetFullPath(resolvedSource).Replace('\\', '/'));

                var normalizedDest = System.IO.Path.GetFullPath(destFileName.Replace('\\', '/')).Replace('\\', '/');
                _vfsIndex[normalizedDest] = resolvedDest;
            }
        }

        public static System.IO.FileStream OpenRead(string path)
        {
            var resolved = Resolve(path);
            return System.IO.File.OpenRead(resolved);
        }

        public static byte[] ReadAllBytes(string path)
        {
            var resolved = Resolve(path);
            return System.IO.File.ReadAllBytes(resolved);
        }

        public static string[] ReadAllLines(string path)
        {
            var resolved = Resolve(path);
            return System.IO.File.ReadAllLines(resolved);
        }

        public static string ReadAllText(string path)
        {
            var resolved = Resolve(path);
            return System.IO.File.ReadAllText(resolved);
        }

        public static void WriteAllText(string path, string contents)
        {
            var resolved = Resolve(path);
            System.IO.File.WriteAllText(resolved, contents);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (_vfsIndex)
            {
                _vfsIndex[fullPath] = resolved;
            }
        }

        public static void WriteAllText(string path, string contents, System.Text.Encoding encoding)
        {
            var resolved = Resolve(path);
            System.IO.File.WriteAllText(resolved, contents, encoding);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (_vfsIndex)
            {
                _vfsIndex[fullPath] = resolved;
            }
        }

        public static void WriteAllLines(string path, IEnumerable<string> contents)
        {
            var resolved = Resolve(path);
            System.IO.File.WriteAllLines(resolved, contents);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (_vfsIndex)
            {
                _vfsIndex[fullPath] = resolved;
            }
        }

        public static void WriteAllLines(string path, IEnumerable<string> contents, System.Text.Encoding encoding)
        {
            var resolved = Resolve(path);
            System.IO.File.WriteAllLines(resolved, contents, encoding);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (_vfsIndex)
            {
                _vfsIndex[fullPath] = resolved;
            }
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            var resolved = Resolve(path);
            System.IO.File.WriteAllBytes(resolved, bytes);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (_vfsIndex)
            {
                _vfsIndex[fullPath] = resolved;
            }
        }

        public static DateTime GetLastWriteTime(string path)
        {
            var resolved = Resolve(path);
            return System.IO.File.GetLastWriteTime(resolved);
        }

        public static System.IO.StreamWriter AppendText(string path)
        {
            var resolved = Resolve(path);
            return System.IO.File.AppendText(resolved);
        }

        public static IEnumerable<string> ReadLines(string path)
        {
            var resolved = Resolve(path);
            return System.IO.File.ReadLines(resolved);
        }
    }

    public static class Directory
    {
        public static bool Exists(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            var resolved = File.ResolveDir(path);
            return System.IO.Directory.Exists(resolved);
        }

        public static System.IO.DirectoryInfo CreateDirectory(string path)
        {
            var resolved = File.ResolveDir(path);
            var di = System.IO.Directory.CreateDirectory(resolved);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (File._vfsDirIndex)
            {
                File._vfsDirIndex[fullPath] = resolved;
            }
            return di;
        }

        public static string[] GetFiles(string path)
        {
            var resolved = File.ResolveDir(path);
            return System.IO.Directory.GetFiles(resolved);
        }

        public static string[] GetFiles(string path, string searchPattern)
        {
            var resolved = File.ResolveDir(path);
            return System.IO.Directory.GetFiles(resolved, searchPattern);
        }

        public static string[] GetFiles(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            var resolved = File.ResolveDir(path);
            return System.IO.Directory.GetFiles(resolved, searchPattern, searchOption);
        }

        public static string[] GetDirectories(string path)
        {
            var resolved = File.ResolveDir(path);
            return System.IO.Directory.GetDirectories(resolved);
        }

        public static string[] GetDirectories(string path, string searchPattern)
        {
            var resolved = File.ResolveDir(path);
            return System.IO.Directory.GetDirectories(resolved, searchPattern);
        }

        public static string[] GetDirectories(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            var resolved = File.ResolveDir(path);
            return System.IO.Directory.GetDirectories(resolved, searchPattern, searchOption);
        }

        public static void Delete(string path)
        {
            var resolved = File.ResolveDir(path);
            System.IO.Directory.Delete(resolved);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (File._vfsDirIndex)
            {
                File._vfsDirIndex.Remove(fullPath);
                var resolvedFullPath = System.IO.Path.GetFullPath(resolved).Replace('\\', '/');
                File._vfsDirIndex.Remove(resolvedFullPath);
            }
        }

        public static void Delete(string path, bool recursive)
        {
            var resolved = File.ResolveDir(path);
            System.IO.Directory.Delete(resolved, recursive);
            var normalizedInput = path.Replace('\\', '/');
            var fullPath = System.IO.Path.GetFullPath(normalizedInput).Replace('\\', '/');
            lock (File._vfsDirIndex)
            {
                File._vfsDirIndex.Remove(fullPath);
                var resolvedFullPath = System.IO.Path.GetFullPath(resolved).Replace('\\', '/');
                File._vfsDirIndex.Remove(resolvedFullPath);
            }
        }
    }
}
