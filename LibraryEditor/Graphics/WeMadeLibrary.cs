using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEditor
{
    public class WeMadeLibrary
    {
        private const string WixExtention = ".Wix",
                             WilExtention = ".Wil",
                             WzlExtention = ".Wzl",
                             WzxExtention = ".Wzx",
                             MizExtention = ".Miz",
                             MixExtention = ".Mix",
                             LibExtention = ".Lib";

        public WeMadeImage[] Images;

        private readonly string _fileName;

        private int[] _palette;
        private List<int> _indexList;
        private int _version = 0;

        private BinaryReader _bReader;
        private FileStream _fStream;
        private string _MainExtention = WilExtention;
        private string _IndexExtention = WixExtention;

        private bool _initialized;

        public byte _nType = 0; //0 = .wil //1 = .wzl //2 = .wil new wemade design //3 = .wil mir3 //4 = .miz shanda mir3 // 5 = 32bit wil
        private byte[] ImageStructureSize = { 8, 16, 16, 17, 16, 16 };//base size of an image structure

        public WeMadeLibrary(string name)
        {
            switch (Path.GetExtension(name).ToLower())
            {
                case ".wzl":
                    _nType = 1;
                    _MainExtention = WzlExtention;
                    _IndexExtention = WzxExtention;
                    break;

                case ".miz":
                    _nType = 4;
                    _MainExtention = MizExtention;
                    _IndexExtention = MixExtention;
                    break;
            }
            _fileName = Path.ChangeExtension(name, null);
            Initialize();
        }

        public void Initialize()
        {
            _initialized = true;

            if (!File.Exists(_fileName + _IndexExtention)) return;
            if (!File.Exists(_fileName + _MainExtention)) return;

            _fStream = new FileStream(_fileName + _MainExtention, FileMode.Open, FileAccess.Read);
            _bReader = new BinaryReader(_fStream);
            LoadImageInfo();

            for (int i = 0; i < Images.Length; i++)
                CheckImage(i);
        }

        private void LoadImageInfo()
        {
            byte[] buffer;
            _palette = new int[256] { -16777216, -8388608, -16744448, -8355840, -16777088, -8388480, -16744320, -4144960, -11173737, -6440504, -8686733, -13817559, -10857902, -10266022, -12437191, -14870504, -15200240, -14084072, -15726584, -886415, -2005153, -42406, -52943, -2729390, -7073792, -7067368, -13039616, -9236480, -4909056, -4365486, -12445680, -21863, -10874880, -9225943, -5944783, -7046285, -4369871, -11394800, -8703720, -13821936, -7583183, -7067392, -4378368, -3771566, -9752296, -3773630, -3257856, -5938375, -10866408, -14020608, -15398912, -12969984, -16252928, -14090240, -11927552, -6488064, -2359296, -2228224, -327680, -6524078, -7050422, -9221591, -11390696, -7583208, -7846895, -11919104, -14608368, -2714534, -3773663, -1086720, -35072, -5925756, -12439263, -15200248, -14084088, -14610432, -13031144, -7576775, -12441328, -9747944, -8697320, -7058944, -7568261, -9739430, -11910599, -14081768, -12175063, -4872812, -8688806, -3231340, -5927821, -7572646, -4877197, -2710157, -1071798, -1063284, -8690878, -9742791, -4352934, -10274560, -2701651, -11386327, -7052520, -1059155, -5927837, -10266038, -4348549, -10862056, -4355023, -13291223, -7043997, -8688822, -5927846, -10859991, -6522055, -12439280, -1069791, -15200256, -14081792, -6526208, -7044006, -11386344, -9741783, -8690911, -6522079, -2185984, -10857927, -13555440, -3228293, -10266055, -7044022, -3758807, -15688680, -12415926, -13530046, -15690711, -16246768, -16246760, -16242416, -15187415, -5917267, -9735309, -15193815, -15187382, -13548982, -10238242, -12263937, -7547153, -9213127, -532935, -528500, -530688, -9737382, -10842971, -12995089, -11887410, -13531979, -13544853, -2171178, -4342347, -7566204, -526370, -16775144, -16246727, -16248791, -16246784, -16242432, -16756059, -16745506, -15718070, -15713941, -15707508, -14591323, -15716006, -15711612, -13544828, -15195855, -11904389, -11375707, -14075549, -15709474, -14079711, -11908551, -14079720, -11908567, -8684734, -6513590, -10855895, -12434924, -13027072, -10921728, -3525332, -9735391, -14077696, -13551344, -13551336, -12432896, -11377896, -10849495, -13546984, -15195904, -15191808, -15189744, -10255286, -9716406, -10242742, -10240694, -10838966, -11891655, -10238390, -10234294, -11369398, -13536471, -10238374, -11354806, -15663360, -15193832, -11892662, -11868342, -16754176, -16742400, -16739328, -16720384, -16716288, -16712960, -11904364, -10259531, -8680234, -9733162, -8943361, -3750194, -7039844, -6515514, -13553351, -14083964, -15204220, -11910574, -11386245, -10265997, -3230217, -7570532, -8969524, -2249985, -1002454, -2162529, -1894477, -1040, -6250332, -8355712, -65536, -16711936, -256, -16776961, -65281, -16711681, -1, };
            if (_nType == 0) //at least we know it's a .wil file up to now
            {
                _fStream.Seek(0, SeekOrigin.Begin);
                buffer = _bReader.ReadBytes(49);

                if (buffer[40] == 1 || buffer[40] == 6)
                    _nType = 2;
                else if (buffer[2] == 73 || buffer[2] == 72)
                    _nType = 3;
                else if (buffer[48] == 32)
                {
                    _nType = 5;
                }

                if ((_nType == 2 || _nType == 0) && _bReader.ReadInt16() == 32)
                {
                    _nType = 5;
                }

                if (_nType == 0)
                {
                    _palette = new int[_bReader.ReadInt32()];
                    _fStream.Seek(4, SeekOrigin.Current);
                    _version = _bReader.ReadInt32();
                    _fStream.Seek(_version == 0 ? 0 : 4, SeekOrigin.Current);
                    for (int i = 1; i < _palette.Length; i++)
                        _palette[i] = _bReader.ReadInt32() + (255 << 24);
                }
            }
            LoadIndexFile();
            Images = new WeMadeImage[_indexList.Count];
        }

        private void LoadIndexFile()
        {
            _indexList = new List<int>();
            FileStream stream = null;

            try
            {
                stream = new FileStream(_fileName + _IndexExtention, FileMode.Open, FileAccess.Read);
                stream.Seek(0, SeekOrigin.Begin);
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    switch (_nType)
                    {
                        case 4:
                            stream.Seek(24, SeekOrigin.Begin);
                            break;

                        case 3:
                            reader.ReadBytes(26);
                            if (reader.ReadUInt16() != 0xB13A)
                                stream.Seek(24, SeekOrigin.Begin);
                            break;

                        case 2:
                        case 5:
                            reader.ReadBytes(52);
                            break;

                        default:
                            reader.ReadBytes(_version == 0 ? 48 : 52);
                            break;
                    }

                    stream = null;
                    while (reader.BaseStream.Position <= reader.BaseStream.Length - 4)
                        _indexList.Add(reader.ReadInt32());
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        private void CheckImage(int index)
        {
            if (!_initialized) Initialize();
            if (Images == null || index < 0 || index >= Images.Length) return;
            if (Images[index] == null)
            {
                _fStream.Position = _indexList[index];
                Images[index] = new WeMadeImage(_bReader, _nType);
            }

            if (Images[index].Image == null)
            {
                _fStream.Seek(_indexList[index] + (_nType > 0 ? ImageStructureSize[_nType] : _version == 0 ? 8 : 12), SeekOrigin.Begin);
                Images[index].CreateTexture(_bReader, _palette);
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
                        WeMadeImage image = Images[i];
                        if (image.HasMask)
                            library.Images[i] = new MLibraryV2.MImage(image.Image, image.MaskImage) { X = image.X, Y = image.Y, ShadowX = image.ShadowX, ShadowY = image.ShadowY, Shadow = image.boHasShadow ? (byte)1 : (byte)0, MaskX = image.X, MaskY = image.Y };
                        else
                            library.Images[i] = new MLibraryV2.MImage(image.Image) { X = image.X, Y = image.Y, ShadowX = image.ShadowX, ShadowY = image.ShadowY, Shadow = image.boHasShadow ? (byte)1 : (byte)0 };
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
        }

        public class WeMadeImage
        {
            public readonly short Width, Height, X, Y, ShadowX, ShadowY;
            public Rectangle TrueSize;
            public Bitmap Image;
            public long CleanTime;
            public byte nType = 0;
            public bool bo16bit = false;
            public int nSize;
            public bool boHasShadow;
            public bool HasMask;
            public Bitmap MaskImage;

            private int convert16bitTo32bit(int color)
            {
                byte red = (byte)((color & 0xf800) >> 8);
                byte green = (byte)((color & 0x07e0) >> 3);
                byte blue = (byte)((color & 0x001f) << 3);
                return ((red << 0x10) | (green << 0x8) | blue) | (255 << 24);//the final or is setting alpha to max so it'll display (since mir2 images have no alpha layer)
            }

            private int WidthBytes(int nBit, int nWidth)
            {
                return (((nWidth * nBit) + 31) >> 5) * 4;
            }

            private byte[][] DecompressWemadeMir3(BinaryReader BReader, short OutputWidth, short OutputHeight, int InputLength)
            {
                byte[][] Pixels = new byte[2][];
                Pixels[0] = new byte[OutputWidth * OutputHeight * 2];
                Pixels[1] = new byte[OutputWidth * OutputHeight * 2];
                int n = BReader.ReadInt32();
                if (n != 0) BReader.BaseStream.Seek(-4, SeekOrigin.Current);
                byte[] FileBytes = BReader.ReadBytes(InputLength * 2);

                int End = 0, OffSet = 0, Start = 0, Count;

                int nX, x = 0;
                //for (int Y = 0; Y < OutputHeight; Y++)
                for (int Y = OutputHeight - 1; Y >= 0; Y--)
                {
                    OffSet = Start * 2;
                    End += FileBytes[OffSet+1] << 8 | FileBytes[OffSet];
                    Start++;
                    nX = Start;
                    OffSet += 2;

                    while (nX < End && End > 0)
                    {
                        switch (FileBytes[OffSet])
                        {
                            default: //Unknown
                                OffSet += 1;
                                break;
                            case 192: //No Colour
                                nX += 2;
                                x += FileBytes[OffSet + 3] << 8 | FileBytes[OffSet + 2];
                                OffSet += 4;
                                break;

                            case 193:  //Solid Colour
                                nX += 2;
                                Count = FileBytes[OffSet + 3] << 8 | FileBytes[OffSet + 2];
                                OffSet += 4;
                                for (int i = 0; i < Count; i++)
                                {
                                    Pixels[0][(Y * OutputWidth + x) * 2] = FileBytes[OffSet];
                                    Pixels[0][(Y * OutputWidth + x) * 2 + 1] = FileBytes[OffSet + 1];
                                    OffSet += 2;
                                    if (x >= OutputWidth) continue;
                                    x++;
                                }
                                nX += Count;
                                break;

                            case 194:  //Overlay Colour
                            case 195:
                                HasMask = true;
                                nX += 2;
                                Count = FileBytes[OffSet + 3] << 8 | FileBytes[OffSet + 2];
                                OffSet += 4;
                                for (int i = 0; i < Count; i++)
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        Pixels[j][(Y * OutputWidth + x) * 2] = FileBytes[OffSet];
                                        Pixels[j][(Y * OutputWidth + x) * 2 + 1] = FileBytes[OffSet + 1];
                                    }
                                    OffSet += 2;
                                    if (x >= OutputWidth) continue;
                                    x++;
                                }
                                nX += Count;
                                break;
                        }
                    }
                    End++;
                    Start = End;
                    x = 0;
                }
                return Pixels;
            }

            public WeMadeImage(BinaryReader reader, byte nType)
            {
                if (reader.BaseStream.Position == 0) return;
                this.nType = nType;
                if (nType == 1)
                {
                    bo16bit = (reader.ReadByte() == 5 ? true : false);
                    reader.ReadBytes(3);
                }
                Width = reader.ReadInt16();
                Height = reader.ReadInt16();
                X = reader.ReadInt16();
                Y = reader.ReadInt16();
                nSize = Width * Height;

                switch (nType)
                {
                    case 1:
                        nSize = reader.ReadInt32();
                        break;

                    case 4:
                        bo16bit = true;
                        nSize = reader.ReadInt32();
                        break;

                    case 2:
                        bo16bit = true;
                        reader.ReadInt16();
                        reader.ReadInt16();
                        nSize = reader.ReadInt32();
                        Width = (nSize < 6) ? (short)0 : Width;
                        break;

                    case 3:
                        bo16bit = true;
                        boHasShadow = reader.ReadByte() == 1 ? true : false;
                        ShadowX = reader.ReadInt16();
                        ShadowY = reader.ReadInt16();
                        nSize = reader.ReadInt32() * 2;
                        break;

                    case 5:
                        reader.ReadInt16();
                        reader.ReadInt16();
                        nSize = reader.ReadInt32();
                        Width = (nSize < 6) ? (short)0 : Width;
                        break;
                }
                Width = (nSize == 0) ? (short)0 : Width; //this makes sure blank images aren't being processed
            }

            public unsafe void CreateTexture(BinaryReader reader, int[] palette)
            {
                if (Width == 0 || Height == 0) return;
                Image = new Bitmap(Width, Height);
                MaskImage = new Bitmap(1, 1);

                BitmapData data = Image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                byte[] bytes = new byte[0];
                byte[] maskbytes = new byte[0];
                MemoryStream output;
                switch (nType)
                {
                    case 0://wemade wil file uncompressed
                        if (palette.Length > 256)
                        {
                            bo16bit = true;
                            nSize = nSize * 2;
                        }
                        bytes = reader.ReadBytes(nSize);
                        break;

                    case 1://shanda wzl file compressed
                    case 4://shanda miz file compressed
                        output = new MemoryStream();
                        Ionic.Zlib.ZlibStream deflateStream = new Ionic.Zlib.ZlibStream(output, Ionic.Zlib.CompressionMode.Decompress);
                        deflateStream.Write(reader.ReadBytes(nSize), 0, nSize);
                        bytes = output.ToArray();
                        deflateStream.Close();
                        output.Close();
                        break;

                    case 2:
                    case 5:
                        byte Compressed = reader.ReadByte();
                        reader.ReadBytes(5);
                        if (Compressed != 8)
                        {
                            bytes = reader.ReadBytes(nSize - 6);
                            break;
                        }
                        MemoryStream input = new MemoryStream(reader.ReadBytes(nSize - 6));
                        output = new MemoryStream();
                        byte[] buffer = new byte[10];
                        System.IO.Compression.DeflateStream decompress = new System.IO.Compression.DeflateStream(input, System.IO.Compression.CompressionMode.Decompress);
                        int len;
                        while ((len = decompress.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, len);
                        }
                        bytes = output.ToArray();
                        decompress.Close();
                        output.Close();
                        input.Close();
                        break;

                    case 3:
                        MaskImage = new Bitmap(Width, Height);
                        byte[][] DecodedPixels = DecompressWemadeMir3(reader, Width, Height, nSize);
                        if (DecodedPixels != null)
                        {
                            bytes = DecodedPixels[0];
                            if (HasMask)
                                maskbytes = DecodedPixels[1];
                        }
                        else
                        {
                            HasMask = false;
                            bytes = new byte[Width * Height * 2];
                        }
                        break;
                }

                int index = 0;

                if (nType == 5 && bytes.Length == Width * Height * 2)
                    bo16bit = true;

                if (bytes.Length <= 1)
                {
                    Image.UnlockBits(data);
                    Image.Dispose();
                    Image = null;
                    MaskImage.Dispose();
                    return;
                }

                int* scan0 = (int*)data.Scan0;
                {
                    for (int y = Height - 1; y >= 0; y--)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            if (nType == 5 && !bo16bit)
                            {
                                scan0[y * Width + x] = BitConverter.ToInt32(bytes, index);
                                index += 4;
                                continue;
                            }

                            if (bo16bit)
                                scan0[y * Width + x] = convert16bitTo32bit(bytes[index++] + (bytes[index++] << 8));
                            else
                                scan0[y * Width + x] = palette[bytes[index++]];
                        }
                        if (((nType == 1) || (nType == 4)) & (Width % 4 > 0))
                            index += WidthBytes(bo16bit ? 16 : 8, Width) - (Width * (bo16bit ? 2 : 1));
                    }
                }
                Image.UnlockBits(data);
                index = 0;
                if (HasMask)
                {
                    BitmapData Maskdata = MaskImage.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    int* maskscan0 = (int*)Maskdata.Scan0;
                    {
                        for (int y = Height - 1; y >= 0; y--)
                        {
                            for (int x = 0; x < Width; x++)
                                maskscan0[y * Width + x] = convert16bitTo32bit(maskbytes[index++] + (maskbytes[index++] << 8));
                        }
                    }
                    MaskImage.UnlockBits(Maskdata);
                }
            }
        }
    }
}
