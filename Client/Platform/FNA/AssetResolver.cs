using System;
using System.IO;
using System.Diagnostics;
using Client.Platform;
using Shared.Vfs;

namespace Client.Platform.FNA
{
    public class AssetResolver : IAssetResolver
    {
        private static readonly string _transcodeCacheDir;

        static AssetResolver()
        {
            _transcodeCacheDir = Path.Combine(Directory.GetCurrentDirectory(), "TranscodeCache");
            if (!Directory.Exists(_transcodeCacheDir))
            {
                Directory.CreateDirectory(_transcodeCacheDir);
            }
        }

        public string Resolve(string path)
        {
            return VfsManager.Resolve(path);
        }

        public bool Exists(string path)
        {
            return VfsManager.FileExists(path);
        }

        public byte[] ReadAllBytes(string path)
        {
            return VfsManager.ReadAllBytes(path);
        }

        public Stream OpenRead(string path)
        {
            return VfsManager.OpenRead(path);
        }

        public string ResolveSound(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            var resolvedPath = Resolve(path);
            var extension = Path.GetExtension(resolvedPath).ToLower();

            if (extension == ".wma")
            {
                return GetTranscodedOggPath(resolvedPath);
            }

            return resolvedPath;
        }

        private string GetTranscodedOggPath(string wmaPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(wmaPath);
            var oggPath = Path.Combine(_transcodeCacheDir, fileName + ".ogg");

            // Bypasses transcoding if already cached
            if (File.Exists(oggPath))
            {
                return oggPath;
            }

            // Transcode dynamically using ffmpeg if present
            if (TryTranscode(wmaPath, oggPath))
            {
                // Register the newly created file in the VFS index
                VfsManager.RegisterFile(oggPath, oggPath);
                return oggPath;
            }

            // Fallback gracefully to WAV if transcoding failed
            var wavFallback = Path.ChangeExtension(wmaPath, ".wav");
            if (File.Exists(Resolve(wavFallback)))
            {
                return Resolve(wavFallback);
            }

            return wmaPath;
        }

        private bool TryTranscode(string inputPath, string outputPath)
        {
            try
            {
                var processStart = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-y -i \"{inputPath}\" -codec:a libvorbis -qscale:a 4 \"{outputPath}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = Process.Start(processStart))
                {
                    process.WaitForExit(8000); // Max 8 seconds per track
                    if (process.ExitCode == 0 && File.Exists(outputPath))
                    {
                        Console.WriteLine($"Transcode success: {Path.GetFileName(inputPath)} -> {Path.GetFileName(outputPath)}");
                        return true;
                    }
                }
            }
            catch
            {
                // Silence exception if ffmpeg is missing in user path
            }
            return false;
        }
    }
}
