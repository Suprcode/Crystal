using System;
using System.IO;
using System.IO.Compression;
using Godot;

namespace ClientGodot.Scripts.MirGraphics
{
    public class MImage
    {
        public short Width, Height, X, Y, ShadowX, ShadowY;
        public byte Shadow;
        public int Length;

        public bool HasMask;
        public short MaskWidth, MaskHeight, MaskX, MaskY;
        public int MaskLength;

        // Raw compressed data for later loading
        private byte[] _compressedData;
        private byte[] _compressedMaskData;

        // Godot Resources
        public ImageTexture Texture;

        public MImage(BinaryReader reader)
        {
            Width = reader.ReadInt16();
            Height = reader.ReadInt16();
            X = reader.ReadInt16();
            Y = reader.ReadInt16();
            ShadowX = reader.ReadInt16();
            ShadowY = reader.ReadInt16();
            Shadow = reader.ReadByte();
            Length = reader.ReadInt32();

            HasMask = ((Shadow >> 7) == 1);

            // Read compressed data into memory
            if (Length > 0)
                _compressedData = reader.ReadBytes(Length);
            else
                _compressedData = new byte[0];

            if (HasMask)
            {
                MaskWidth = reader.ReadInt16();
                MaskHeight = reader.ReadInt16();
                MaskX = reader.ReadInt16();
                MaskY = reader.ReadInt16();
                MaskLength = reader.ReadInt32();

                if (MaskLength > 0)
                    _compressedMaskData = reader.ReadBytes(MaskLength);
            }
        }

        public Texture2D CreateTexture()
        {
            if (Texture != null) return Texture;
            if (Width == 0 || Height == 0) return null;

            byte[] rawData = DecompressImage(_compressedData);

            // Original data is BGRA (DirectX A8R8G8B8 usually stores as B G R A in memory)
            // or sometimes it's just raw ARGB.
            // In C# SlimDX/DirectX, Color.FromArgb stores as A R G B.
            // But texture memory layout is typically BGRA on Windows.
            // Let's assume BGRA for now, and we need RGBA for Godot.

            // However, looking at DecompressImage in original code, it copies bytes directly.
            // If the source was made for DX9, it's likely BGRA.

            if (rawData.Length == Width * Height * 4)
            {
                 // Swizzle BGRA -> RGBA
                 for (int i = 0; i < rawData.Length; i += 4)
                 {
                     byte b = rawData[i];
                     byte g = rawData[i+1];
                     byte r = rawData[i+2];
                     byte a = rawData[i+3];

                     rawData[i] = r;
                     rawData[i+1] = g;
                     rawData[i+2] = b;
                     rawData[i+3] = a;
                 }
            }

            Godot.Image image = Godot.Image.CreateFromData(Width, Height, false, Godot.Image.Format.Rgba8, rawData);
            Texture = ImageTexture.CreateFromImage(image);

            // Clear compressed data to save RAM?
            // Maybe not if we need to reload context-loss (Godot handles this though).
            _compressedData = null;

            return Texture;
        }

        private static byte[] DecompressImage(byte[] image)
        {
            if (image == null || image.Length == 0) return new byte[0];

            using (GZipStream stream = new GZipStream(new MemoryStream(image), CompressionMode.Decompress))
            using (MemoryStream memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                return memory.ToArray();
            }
        }
    }
}
