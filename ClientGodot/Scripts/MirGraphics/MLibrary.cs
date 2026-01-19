using System;
using System.IO;
using System.IO.Compression;
using Godot;

namespace ClientGodot.Scripts.MirGraphics
{
    public class MLibrary
    {
        public const string Extension = ".Lib";
        public const int LibVersion = 3;

        private readonly string _fileName;
        private MImage[] _images;
        private int[] _indexList;
        private int _count;
        private bool _initialized;

        private BinaryReader _reader;
        private FileStream _fStream;

        public MLibrary(string filename)
        {
            _fileName = Path.ChangeExtension(filename, Extension);
        }

        public void Initialize()
        {
            _initialized = true;

            // In Godot, file paths might be global or res://.
            // The original code used global paths constructed from Settings.DataPath.
            // We assume _fileName is a valid global path or relative to execution.

            // Convert Godot "user://" or "res://" to global path if necessary,
            // but System.IO needs real OS paths.
            string loadPath = ProjectSettings.GlobalizePath(_fileName);

            if (!File.Exists(loadPath))
            {
                // GD.PrintErr($"Library file not found: {loadPath}");
                return;
            }

            try
            {
                _fStream = new FileStream(loadPath, FileMode.Open, System.IO.FileAccess.Read);
                _reader = new BinaryReader(_fStream);

                int currentVersion = _reader.ReadInt32();
                if (currentVersion < 2)
                {
                    GD.PrintErr($"Wrong library version for {_fileName}.");
                    return;
                }

                _count = _reader.ReadInt32();
                int frameSeek = 0;
                if (currentVersion >= 3)
                {
                    frameSeek = _reader.ReadInt32();
                }

                _images = new MImage[_count];
                _indexList = new int[_count];

                for (int i = 0; i < _count; i++)
                    _indexList[i] = _reader.ReadInt32();
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to initialize library {_fileName}: {ex}");
                _initialized = false;
            }
        }

        public MImage GetImage(int index)
        {
            if (!_initialized) Initialize();

            if (_images == null || index < 0 || index >= _images.Length)
                return null;

            if (_images[index] == null)
            {
                try
                {
                    _fStream.Seek(_indexList[index], SeekOrigin.Begin);
                    _images[index] = new MImage(_reader);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"Error loading image {index} from {_fileName}: {ex}");
                    return null;
                }
            }

            return _images[index];
        }

        // Helper to get Texture directly
        public Texture2D GetTexture(int index)
        {
            MImage img = GetImage(index);
            if (img == null) return null;
            return img.CreateTexture();
        }
    }
}
