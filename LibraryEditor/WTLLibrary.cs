using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace LibraryEditor
{
    public class WTLLibrary
    {
        private readonly string _fileName;

        public WTLImage[] Images;

        private BinaryReader _bReader;
        private int _count;
        private FileStream _fStream;
        private int[] _indexList;
        private bool _initialized;

        public WTLLibrary(string filename)
        {
            _fileName = Path.ChangeExtension(filename, null);
            Initialize();
        }

        public void Initialize()
        {
            _initialized = true;
            if (!File.Exists(_fileName + ".wtl")) return;
            _fStream = new FileStream(_fileName + ".wtl", FileMode.Open, FileAccess.ReadWrite);
            _bReader = new BinaryReader(_fStream);
            LoadImageInfo();

            for (int i = 0; i < _count; i++)
            {
                CheckMImage(i);
            }
        }

        private void LoadImageInfo()
        {
            _fStream.Seek(28, SeekOrigin.Begin);

            _count = _bReader.ReadInt32();
            Images = new WTLImage[_count];
            _indexList = new int[_count];

            for (int i = 0; i < _count; i++)
                _indexList[i] = _bReader.ReadInt32();
        }

        public void Close()
        {
            if (_fStream != null)
                _fStream.Dispose();
            if (_bReader != null)
                _bReader.Dispose();
        }

        public void CheckMImage(int index)
        {
            if (!_initialized)
                Initialize();

            if (index < 0 || index >= Images.Length) return;
            if (_indexList[index] == 0)
            {
                Images[index] = new WTLImage();
                return;
            }
            WTLImage image = Images[index];

            if (image == null)
            {
                _fStream.Seek(_indexList[index], SeekOrigin.Begin);
                image = new WTLImage(_bReader);
                Images[index] = image;
                image.CreateTexture(_bReader);
            }
        }

        public void ToMLibrary()
        {
            string fileName = Path.ChangeExtension(_fileName, ".Lib");

            if (File.Exists(fileName))
                File.Delete(fileName);

            MLibraryV2 library = new MLibraryV2(fileName) { Images = new List<MLibraryV2.MImage>(), IndexList = new List<int>(), Count = Images.Length };
            //library.Save();

            for (int i = 0; i < library.Count; i++)
                library.Images.Add(null);

            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 8 };
            try
            {
                Parallel.For(0, Images.Length, options, i =>
                {
                    WTLImage image = Images[i];
                    if (image.HasMask)
                        library.Images[i] = new MLibraryV2.MImage(image.Image, image.MaskImage) { X = image.X, Y = image.Y, ShadowX = image.ShadowX, ShadowY = image.ShadowY, Shadow = image.Shadow, MaskX = image.MaskX, MaskY = image.MaskY };
                    else
                        library.Images[i] = new MLibraryV2.MImage(image.Image) { X = image.X, Y = image.Y, ShadowX = image.ShadowX, ShadowY = image.ShadowY, Shadow = image.Shadow };
                });
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                library.Save();
            }

            // Operation finished.
            // System.Windows.Forms.MessageBox.Show("Converted " + fileName + " successfully.",
            //    "Shanda Information",
            //        System.Windows.Forms.MessageBoxButtons.OK,
            //            System.Windows.Forms.MessageBoxIcon.Information,
            //                System.Windows.Forms.MessageBoxDefaultButton.Button1);
        }
    }

    public class WTLImage
    {
        public readonly short Width, Height, X, Y, ShadowX, ShadowY;
        public readonly int Length;
        public readonly byte Shadow;
        private byte[] _fBytes;
        public Bitmap Image;

        public readonly Boolean HasMask;
        public short MaskWidth, MaskHeight, MaskX, MaskY;
        public int MaskLength;
        private byte[] _MaskfBytes;
        public Bitmap MaskImage;

        public WTLImage()
        {
            _fBytes = new byte[0];
            _MaskfBytes = new byte[0];
        }

        public WTLImage(BinaryReader bReader)
        {
            Width = bReader.ReadInt16();
            Height = bReader.ReadInt16();
            X = bReader.ReadInt16();
            Y = bReader.ReadInt16();
            ShadowX = bReader.ReadInt16();
            ShadowY = bReader.ReadInt16();
            Length = bReader.ReadByte() | bReader.ReadByte() << 8 | bReader.ReadByte() << 16;
            Shadow = bReader.ReadByte();
            HasMask = ((Shadow >> 7) == 1) ? true : false;
        }

        public unsafe void CreateTexture(BinaryReader bReader)
        {
            Image = ReadImage(bReader, Length, Width, Height);
            if (HasMask)
            {
                if (HasMask)
                {
                    MaskWidth = bReader.ReadInt16();
                    MaskHeight = bReader.ReadInt16();
                    MaskX = bReader.ReadInt16();
                    MaskY = bReader.ReadInt16();
                    bReader.ReadInt16();//mask shadow x
                    bReader.ReadInt16();//mask shadow y
                    MaskLength = bReader.ReadByte() | bReader.ReadByte() << 8 | bReader.ReadByte() << 16;
                    bReader.ReadByte(); //mask shadow
                }
                MaskImage = ReadImage(bReader, MaskLength, MaskWidth, MaskHeight);
            }
        }

        public unsafe Bitmap ReadImage(BinaryReader bReader, int imageLength, short outputWidth, short outputHeight)
        {
            const int size = 8;
            int offset = 0, blockOffSet = 0;
            List<byte> countList = new List<byte>();
            int tWidth = 2;

            while (tWidth < Width)
                tWidth *= 2;

            _fBytes = bReader.ReadBytes(imageLength);

            Bitmap output = new Bitmap(outputWidth, outputHeight);
            if (_fBytes.Length != imageLength) return null;
            BitmapData data = output.LockBits(new Rectangle(0, 0, outputWidth, outputHeight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* pixels = (byte*)data.Scan0;
            int cap = outputWidth * outputHeight * 4;
            int currentx = 0;

            while (blockOffSet < imageLength)
            {
                countList.Clear();
                for (int i = 0; i < 8; i++)
                    countList.Add(_fBytes[blockOffSet++]);

                for (int i = 0; i < countList.Count; i++)
                {
                    int count = countList[i];

                    if (i % 2 == 0)
                    {
                        if (currentx >= tWidth)
                            currentx -= tWidth;

                        for (int off = 0; off < count; off++)
                        {
                            if (currentx < outputWidth)
                                offset++;

                            currentx += 4;

                            if (currentx >= tWidth)
                                currentx -= tWidth;
                        }
                        continue;
                    }

                    for (int c = 0; c < count; c++)
                    {
                        if (blockOffSet >= _fBytes.Length)
                            break;

                        byte[] newPixels = new byte[64];
                        byte[] block = new byte[size];

                        Array.Copy(_fBytes, blockOffSet, block, 0, size);
                        blockOffSet += size;
                        DecompressBlock(newPixels, block);

                        int pixelOffSet = 0;
                        byte[] sourcePixel = new byte[4];

                        for (int py = 0; py < 4; py++)
                        {
                            for (int px = 0; px < 4; px++)
                            {
                                int blockx = offset % (outputWidth / 4);
                                int blocky = offset / (outputWidth / 4);

                                int x = blockx * 4;
                                int y = blocky * 4;

                                int destPixel = ((y + py) * outputWidth) * 4 + (x + px) * 4;

                                Array.Copy(newPixels, pixelOffSet, sourcePixel, 0, 4);
                                pixelOffSet += 4;

                                if (destPixel + 4 > cap)
                                    break;
                                for (int pc = 0; pc < 4; pc++)
                                    pixels[destPixel + pc] = sourcePixel[pc];
                            }
                        }
                        offset++;
                        if (currentx >= outputWidth)
                            currentx -= outputWidth;
                        currentx += 4;
                    }
                }
            }

            output.UnlockBits(data);
            return output;
        }

        private static void DecompressBlock(IList<byte> newPixels, byte[] block)
        {
            byte[] colours = new byte[8];
            Array.Copy(block, 0, colours, 0, 8);

            byte[] codes = new byte[16];

            int a = Unpack(block, 0, codes, 0);
            int b = Unpack(block, 2, codes, 4);

            for (int i = 0; i < 3; i++)
            {
                int c = codes[i];
                int d = codes[4 + i];

                if (a <= b)
                {
                    codes[8 + i] = (byte)((c + d) / 2);
                    codes[12 + i] = 0;
                }
                else
                {
                    codes[8 + i] = (byte)((2 * c + d) / 3);
                    codes[12 + i] = (byte)((c + 2 * d) / 3);
                }
            }

            codes[8 + 3] = 255;
            codes[12 + 3] = (a <= b) ? (byte)0 : (byte)255;
            for (int i = 0; i < 4; i++)
            {
                if ((codes[i * 4] == 0) && (codes[(i * 4) + 1] == 0) && (codes[(i * 4) + 2] == 0) && (codes[(i * 4) + 3] == 255))
                { //dont ever use pure black cause that gives transparency issues
                    codes[i * 4] = 1;
                    codes[(i * 4) + 1] = 1;
                    codes[(i * 4) + 2] = 1;
                }
            }

            byte[] indices = new byte[16];
            for (int i = 0; i < 4; i++)
            {
                byte packed = block[4 + i];

                indices[0 + i * 4] = (byte)(packed & 0x3);
                indices[1 + i * 4] = (byte)((packed >> 2) & 0x3);
                indices[2 + i * 4] = (byte)((packed >> 4) & 0x3);
                indices[3 + i * 4] = (byte)((packed >> 6) & 0x3);
            }

            for (int i = 0; i < 16; i++)
            {
                byte offset = (byte)(4 * indices[i]);
                for (int j = 0; j < 4; j++)
                    newPixels[4 * i + j] = codes[offset + j];
            }
        }

        private static int Unpack(IList<byte> packed, int srcOffset, IList<byte> colour, int dstOffSet)
        {
            int value = packed[0 + srcOffset] | (packed[1 + srcOffset] << 8);
            // get components in the stored range
            byte red = (byte)((value >> 11) & 0x1F);
            byte green = (byte)((value >> 5) & 0x3F);
            byte blue = (byte)(value & 0x1F);

            // Scale up to 24 Bit
            colour[2 + dstOffSet] = (byte)((red << 3) | (red >> 2));
            colour[1 + dstOffSet] = (byte)((green << 2) | (green >> 4));
            colour[0 + dstOffSet] = (byte)((blue << 3) | (blue >> 2));
            colour[3 + dstOffSet] = 255;
            //*/
            return value;
        }
    }
}