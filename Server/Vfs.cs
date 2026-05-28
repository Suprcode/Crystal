using System;
using System.IO;
using System.Collections.Generic;
using Shared.Vfs;

namespace Server
{
    public static class File
    {
        public static string Resolve(string path) => VfsManager.Resolve(path);
        public static string ResolveDir(string path) => VfsManager.ResolveDir(path);
        public static bool Exists(string path) => VfsManager.FileExists(path);

        public static System.IO.FileStream Create(string path)
        {
            var resolved = Resolve(path);
            var fs = System.IO.File.Create(resolved);
            VfsManager.RegisterFile(path, resolved);
            return fs;
        }

        public static void Delete(string path)
        {
            var resolved = Resolve(path);
            System.IO.File.Delete(resolved);
            VfsManager.UnregisterFile(path, resolved);
        }

        public static void Copy(string sourceFileName, string destFileName)
        {
            var resolvedSource = Resolve(sourceFileName);
            var resolvedDest = Resolve(destFileName);
            System.IO.File.Copy(resolvedSource, resolvedDest);
            VfsManager.RegisterFile(destFileName, resolvedDest);
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            var resolvedSource = Resolve(sourceFileName);
            var resolvedDest = Resolve(destFileName);
            System.IO.File.Copy(resolvedSource, resolvedDest, overwrite);
            VfsManager.RegisterFile(destFileName, resolvedDest);
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            var resolvedSource = Resolve(sourceFileName);
            var resolvedDest = Resolve(destFileName);
            System.IO.File.Move(resolvedSource, resolvedDest);
            VfsManager.UnregisterFile(sourceFileName, resolvedSource);
            VfsManager.RegisterFile(destFileName, resolvedDest);
        }

        public static void Move(string sourceFileName, string destFileName, bool overwrite)
        {
            var resolvedSource = Resolve(sourceFileName);
            var resolvedDest = Resolve(destFileName);
            System.IO.File.Move(resolvedSource, resolvedDest, overwrite);
            VfsManager.UnregisterFile(sourceFileName, resolvedSource);
            VfsManager.RegisterFile(destFileName, resolvedDest);
        }

        public static System.IO.FileStream OpenRead(string path)
        {
            return VfsManager.OpenRead(path);
        }

        public static byte[] ReadAllBytes(string path)
        {
            return VfsManager.ReadAllBytes(path);
        }

        public static string[] ReadAllLines(string path)
        {
            return VfsManager.ReadAllLines(path);
        }

        public static string ReadAllText(string path)
        {
            return VfsManager.ReadAllText(path);
        }

        public static void WriteAllText(string path, string contents)
        {
            VfsManager.WriteAllText(path, contents);
        }

        public static void WriteAllText(string path, string contents, System.Text.Encoding encoding)
        {
            VfsManager.WriteAllText(path, contents, encoding);
        }

        public static void WriteAllLines(string path, IEnumerable<string> contents)
        {
            VfsManager.WriteAllLines(path, contents);
        }

        public static void WriteAllLines(string path, IEnumerable<string> contents, System.Text.Encoding encoding)
        {
            VfsManager.WriteAllLines(path, contents, encoding);
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            VfsManager.WriteAllBytes(path, bytes);
        }

        public static DateTime GetLastWriteTime(string path)
        {
            return VfsManager.GetLastWriteTime(path);
        }

        public static System.IO.StreamWriter AppendText(string path)
        {
            return VfsManager.AppendText(path);
        }

        public static IEnumerable<string> ReadLines(string path)
        {
            return VfsManager.ReadLines(path);
        }
    }

    public static class Directory
    {
        public static bool Exists(string path)
        {
            return VfsManager.DirectoryExists(path);
        }

        public static System.IO.DirectoryInfo CreateDirectory(string path)
        {
            var resolved = File.ResolveDir(path);
            var di = System.IO.Directory.CreateDirectory(resolved);
            VfsManager.RegisterDir(path, resolved);
            return di;
        }

        public static string[] GetFiles(string path)
        {
            return VfsManager.GetFilesMatching(path, "*", SearchOption.TopDirectoryOnly);
        }

        public static string[] GetFiles(string path, string searchPattern)
        {
            return VfsManager.GetFilesMatching(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetFiles(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            return VfsManager.GetFilesMatching(path, searchPattern, searchOption);
        }

        public static string[] GetDirectories(string path)
        {
            return VfsManager.GetDirectoriesMatching(path, "*", SearchOption.TopDirectoryOnly);
        }

        public static string[] GetDirectories(string path, string searchPattern)
        {
            return VfsManager.GetDirectoriesMatching(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetDirectories(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            return VfsManager.GetDirectoriesMatching(path, searchPattern, searchOption);
        }

        public static void Delete(string path)
        {
            var resolved = File.ResolveDir(path);
            System.IO.Directory.Delete(resolved);
            VfsManager.UnregisterDir(path, resolved);
        }

        public static void Delete(string path, bool recursive)
        {
            var resolved = File.ResolveDir(path);
            System.IO.Directory.Delete(resolved, recursive);
            VfsManager.UnregisterDir(path, resolved);
        }
    }
}
