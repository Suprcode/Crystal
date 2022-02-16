using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Map_Editor
{
  public sealed class MLibrary
  {
    public const int LibVersion = 2;
    public static bool Load = true;
    public string FileName;
    public List<MLibrary.MImage> Images = new List<MLibrary.MImage>();
    public List<int> IndexList = new List<int>();
    public int Count;
    private bool _initialized;
    private BinaryReader _reader;
    private FileStream _stream;

    public MLibrary(string filename)
    {
      this.FileName = filename + ".lib";
      this.Initialize();
    }

    public void Initialize()
    {
      this._initialized = true;
      if (!File.Exists(this.FileName))
        return;
      this._stream = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
      this._reader = new BinaryReader((Stream) this._stream);
      int num1 = this._reader.ReadInt32();
      if (num1 != 2)
      {
        int num2 = (int) MessageBox.Show("Wrong version, expecting lib version: " + 2.ToString() + " found version: " + num1.ToString() + ".", "Failed to open", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
      else
      {
        this.Count = this._reader.ReadInt32();
        this.Images = new List<MLibrary.MImage>();
        this.IndexList = new List<int>();
        for (int index = 0; index < this.Count; ++index)
          this.IndexList.Add(this._reader.ReadInt32());
        for (int index = 0; index < this.Count; ++index)
          this.Images.Add((MLibrary.MImage) null);
      }
    }

    public void Close()
    {
      if (this._stream != null)
        this._stream.Dispose();
      if (this._reader == null)
        return;
      this._reader.Dispose();
    }

    public void Save()
    {
      this.Close();
      MemoryStream output = new MemoryStream();
      BinaryWriter writer = new BinaryWriter((Stream) output);
      this.Count = this.Images.Count;
      this.IndexList.Clear();
      int num = 8 + this.Count * 4;
      for (int index = 0; index < this.Count; ++index)
      {
        this.IndexList.Add((int) output.Length + num);
        this.Images[index].Save(writer);
      }
      writer.Flush();
      byte[] array = output.ToArray();
      this._stream = File.Create(this.FileName);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) this._stream);
      binaryWriter.Write(2);
      binaryWriter.Write(this.Count);
      for (int index = 0; index < this.Count; ++index)
        binaryWriter.Write(this.IndexList[index]);
      binaryWriter.Write(array);
      binaryWriter.Flush();
      binaryWriter.Close();
      binaryWriter.Dispose();
      this.Close();
    }

    public void CheckImage(int index)
    {
      if (!this._initialized)
        this.Initialize();
      if (this.Images == null || index < 0 || index >= this.Images.Count || this._stream == null)
        return;
      if (this.Images[index] == null)
      {
        this._stream.Position = (long) this.IndexList[index];
        this.Images[index] = new MLibrary.MImage(this._reader);
      }
      if (!MLibrary.Load)
        return;
      if (!this.Images[index].TextureValid)
      {
        this._stream.Seek((long) (this.IndexList[index] + 12), SeekOrigin.Begin);
        this.Images[index].CreateBmpTexture(this._reader);
      }
      if (this.Images[index].TextureValid)
        return;
      this._stream.Seek((long) (this.IndexList[index] + 17), SeekOrigin.Begin);
      this.Images[index].CreateTexture(this._reader);
    }

    public Point GetOffSet(int index)
    {
      if (!this._initialized)
        this.Initialize();
      if (this.Images == null || index < 0 || index >= this.Images.Count)
        return Point.Empty;
      if (this.Images[index] == null)
      {
        this._stream.Seek((long) this.IndexList[index], SeekOrigin.Begin);
        this.Images[index] = new MLibrary.MImage(this._reader);
      }
      return new Point((int) this.Images[index].X, (int) this.Images[index].Y);
    }

    public Size GetSize(int index)
    {
      if (!this._initialized)
        this.Initialize();
      if (this.Images == null || index < 0 || index >= this.Images.Count)
        return Size.Empty;
      if (this.Images[index] == null)
      {
        this._stream.Seek((long) this.IndexList[index], SeekOrigin.Begin);
        this.Images[index] = new MLibrary.MImage(this._reader);
      }
      return new Size((int) this.Images[index].Width, (int) this.Images[index].Height);
    }

    public MLibrary.MImage GetMImage(int index)
    {
      if (index < 0 || index >= this.Images.Count)
        return (MLibrary.MImage) null;
      this.CheckImage(index);
      return this.Images[index];
    }

    public Bitmap GetPreview(int index)
    {
      if (index < 0 || index >= this.Images.Count)
        return new Bitmap(1, 1);
      MLibrary.MImage image = this.Images[index];
      if (image == null || image.Image == null)
        return new Bitmap(1, 1);
      if (image.Preview == null)
        image.CreatePreview();
      return image.Preview;
    }

    public void AddImage(Bitmap image, short x, short y)
    {
      MLibrary.MImage mimage = new MLibrary.MImage(image)
      {
        X = x,
        Y = y
      };
      ++this.Count;
      this.Images.Add(mimage);
    }

    public void ReplaceImage(int Index, Bitmap image, short x, short y)
    {
      MLibrary.MImage mimage = new MLibrary.MImage(image)
      {
        X = x,
        Y = y
      };
      this.Images[Index] = mimage;
    }

    public void InsertImage(int index, Bitmap image, short x, short y)
    {
      MLibrary.MImage mimage = new MLibrary.MImage(image)
      {
        X = x,
        Y = y
      };
      ++this.Count;
      this.Images.Insert(index, mimage);
    }

    public void RemoveImage(int index)
    {
      if (this.Images == null || this.Count <= 1)
      {
        this.Count = 0;
        this.Images = new List<MLibrary.MImage>();
      }
      else
      {
        --this.Count;
        this.Images.RemoveAt(index);
      }
    }

    public static bool CompareBytes(byte[] a, byte[] b)
    {
      if (a == b)
        return true;
      if (a == null || b == null || a.Length != b.Length)
        return false;
      for (int index = 0; index < a.Length; ++index)
      {
        if ((int) a[index] != (int) b[index])
          return false;
      }
      return true;
    }

    public void RemoveBlanks(bool safe = false)
    {
      for (int index = this.Count - 1; index >= 0; --index)
      {
        if (this.Images[index].FBytes == null || this.Images[index].FBytes.Length <= 24)
        {
          if (!safe)
            this.RemoveImage(index);
          else if (this.Images[index].X == (short) 0 && this.Images[index].Y == (short) 0)
            this.RemoveImage(index);
        }
      }
    }

    public sealed class MImage
    {
      public short Width;
      public short Height;
      public short X;
      public short Y;
      public short ShadowX;
      public short ShadowY;
      public byte Shadow;
      public int Length;
      public byte[] FBytes;
      public bool TextureValid;
      public Bitmap Image;
      public Bitmap Preview;
      public Texture ImageTexture;
      public short MaskWidth;
      public short MaskHeight;
      public short MaskX;
      public short MaskY;
      public int MaskLength;
      public byte[] MaskFBytes;
      public Bitmap MaskImage;
      public Texture MaskImageTexture;
      public bool HasMask;
      public unsafe byte* Data;

      public MImage(BinaryReader reader)
      {
        this.Width = reader.ReadInt16();
        this.Height = reader.ReadInt16();
        this.X = reader.ReadInt16();
        this.Y = reader.ReadInt16();
        this.ShadowX = reader.ReadInt16();
        this.ShadowY = reader.ReadInt16();
        this.Shadow = reader.ReadByte();
        this.Length = reader.ReadInt32();
        this.FBytes = reader.ReadBytes(this.Length);
        this.HasMask = (int) this.Shadow >> 7 == 1;
        if (!this.HasMask)
          return;
        this.MaskWidth = reader.ReadInt16();
        this.MaskHeight = reader.ReadInt16();
        this.MaskX = reader.ReadInt16();
        this.MaskY = reader.ReadInt16();
        this.MaskLength = reader.ReadInt32();
        this.MaskFBytes = reader.ReadBytes(this.MaskLength);
      }

      public MImage(byte[] image, short Width, short Height)
      {
        this.FBytes = image;
        this.Width = Width;
        this.Height = Height;
      }

      public MImage(Bitmap image)
      {
        if (image == null)
        {
          this.FBytes = new byte[0];
        }
        else
        {
          this.Width = (short) image.Width;
          this.Height = (short) image.Height;
          this.Image = image;
          this.FBytes = this.ConvertBitmapToArray(this.Image);
        }
      }

      public MImage(Bitmap image, Bitmap Maskimage)
      {
        if (image == null)
        {
          this.FBytes = new byte[0];
        }
        else
        {
          this.Width = (short) image.Width;
          this.Height = (short) image.Height;
          this.Image = image;
          this.FBytes = this.ConvertBitmapToArray(this.Image);
          if (Maskimage == null)
          {
            this.MaskFBytes = new byte[0];
          }
          else
          {
            this.HasMask = true;
            this.MaskWidth = (short) Maskimage.Width;
            this.MaskHeight = (short) Maskimage.Height;
            this.MaskImage = Maskimage;
            this.MaskFBytes = this.ConvertBitmapToArray(this.MaskImage);
          }
        }
      }

      private Bitmap FixImageSize(Bitmap input)
      {
        int width = input.Width + (4 - input.Width % 4) % 4;
        int height = input.Height + (4 - input.Height % 4) % 4;
        if (input.Width != width || input.Height != height)
        {
          Bitmap bitmap = new Bitmap(width, height);
          using (Graphics graphics = Graphics.FromImage((Image) bitmap))
          {
            graphics.Clear(Color.Transparent);
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.DrawImage((Image) input, 0, 0);
            graphics.Save();
          }
          input.Dispose();
          input = bitmap;
        }
        return input;
      }

      private byte[] ConvertBitmapToArray(Bitmap input)
      {
        BitmapData bitmapdata = input.LockBits(new Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        byte[] numArray = new byte[input.Width * input.Height * 4];
        Marshal.Copy(bitmapdata.Scan0, numArray, 0, numArray.Length);
        input.UnlockBits(bitmapdata);
        for (int index = 0; index < numArray.Length; index += 4)
        {
          if (numArray[index] == (byte) 0 && numArray[index + 1] == (byte) 0 && numArray[index + 2] == (byte) 0)
            numArray[index + 3] = (byte) 0;
        }
        return MLibrary.MImage.Compress(numArray);
      }

      public void CreateBmpTexture(BinaryReader reader)
      {
        int width = (int) this.Width;
        int height = (int) this.Height;
        if (width == 0 || height == 0 || width < 2 || height < 2)
          return;
        this.Image = new Bitmap(width, height);
        BitmapData bitmapdata1 = this.Image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        byte[] source1 = MLibrary.MImage.Decompress(this.FBytes);
        Marshal.Copy(source1, 0, bitmapdata1.Scan0, source1.Length);
        this.Image.UnlockBits(bitmapdata1);
        if (!this.HasMask)
          return;
        int maskWidth = (int) this.MaskWidth;
        int maskHeight = (int) this.MaskHeight;
        if (maskWidth == 0 || maskHeight == 0)
          return;
        try
        {
          this.MaskImage = new Bitmap(maskWidth, maskHeight);
          BitmapData bitmapdata2 = this.MaskImage.LockBits(new Rectangle(0, 0, maskWidth, maskHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
          byte[] source2 = MLibrary.MImage.Decompress(this.MaskFBytes);
          Marshal.Copy(source2, 0, bitmapdata2.Scan0, source2.Length);
          this.MaskImage.UnlockBits(bitmapdata2);
        }
        catch (Exception ex)
        {
          File.AppendAllText(".\\Error.txt", string.Format("[{0}] {1}{2}", (object) DateTime.Now, (object) ex, (object) Environment.NewLine));
        }
      }

      public unsafe void CreateTexture(BinaryReader reader)
      {
        int width1 = (int) this.Width;
        int height1 = (int) this.Height;
        if (width1 == 0 || height1 == 0 || width1 < 2 || height1 < 2)
          return;
        this.ImageTexture = new Texture(DXManager.Device, width1, height1, 1, Usage.None, Microsoft.DirectX.Direct3D.Format.A8R8G8B8, Pool.Managed);
        GraphicsStream graphicsStream1 = this.ImageTexture.LockRectangle(0, LockFlags.Discard);
        this.Data = (byte*) graphicsStream1.InternalDataPointer;
        byte[] buffer1 = MLibrary.MImage.DecompressImage(reader.ReadBytes(this.Length));
        graphicsStream1.Write(buffer1, 0, buffer1.Length);
        graphicsStream1.Dispose();
        this.ImageTexture.UnlockRectangle(0);
        if (this.HasMask)
        {
          reader.ReadBytes(12);
          int width2 = (int) this.Width;
          int height2 = (int) this.Height;
          this.MaskImageTexture = new Texture(DXManager.Device, width2, height2, 1, Usage.None, Microsoft.DirectX.Direct3D.Format.A8R8G8B8, Pool.Managed);
          GraphicsStream graphicsStream2 = this.MaskImageTexture.LockRectangle(0, LockFlags.Discard);
          byte[] buffer2 = MLibrary.MImage.DecompressImage(reader.ReadBytes(this.Length));
          graphicsStream2.Write(buffer2, 0, buffer2.Length);
          graphicsStream2.Dispose();
          this.MaskImageTexture.UnlockRectangle(0);
        }
        this.TextureValid = true;
      }

      public void Save(BinaryWriter writer)
      {
        writer.Write(this.Width);
        writer.Write(this.Height);
        writer.Write(this.X);
        writer.Write(this.Y);
        writer.Write(this.ShadowX);
        writer.Write(this.ShadowY);
        writer.Write(this.HasMask ? (byte) ((uint) this.Shadow | 128U) : this.Shadow);
        writer.Write(this.FBytes.Length);
        writer.Write(this.FBytes);
        if (!this.HasMask)
          return;
        writer.Write(this.MaskWidth);
        writer.Write(this.MaskHeight);
        writer.Write(this.MaskX);
        writer.Write(this.MaskY);
        writer.Write(this.MaskFBytes.Length);
        writer.Write(this.MaskFBytes);
      }

      public static byte[] Compress(byte[] raw)
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Compress, true))
            gzipStream.Write(raw, 0, raw.Length);
          return memoryStream.ToArray();
        }
      }

      private static byte[] Decompress(byte[] gzip)
      {
        using (GZipStream gzipStream = new GZipStream((Stream) new MemoryStream(gzip), CompressionMode.Decompress))
        {
          byte[] buffer = new byte[4096];
          using (MemoryStream memoryStream = new MemoryStream())
          {
            int count;
            do
            {
              count = gzipStream.Read(buffer, 0, 4096);
              if (count > 0)
                memoryStream.Write(buffer, 0, count);
            }
            while (count > 0);
            return memoryStream.ToArray();
          }
        }
      }

      private static byte[] DecompressImage(byte[] image)
      {
        using (GZipStream gzipStream = new GZipStream((Stream) new MemoryStream(image), CompressionMode.Decompress))
        {
          byte[] buffer = new byte[4096];
          using (MemoryStream memoryStream = new MemoryStream())
          {
            int count;
            do
            {
              count = gzipStream.Read(buffer, 0, 4096);
              if (count > 0)
                memoryStream.Write(buffer, 0, count);
            }
            while (count > 0);
            return memoryStream.ToArray();
          }
        }
      }

      public void CreatePreview()
      {
        if (this.Image == null)
        {
          this.Preview = new Bitmap(1, 1);
        }
        else
        {
          this.Preview = new Bitmap(64, 64);
          using (Graphics graphics = Graphics.FromImage((Image) this.Preview))
          {
            graphics.InterpolationMode = InterpolationMode.Low;
            graphics.Clear(Color.Transparent);
            int width = Math.Min((int) this.Width, 64);
            int height = Math.Min((int) this.Height, 64);
            graphics.DrawImage((Image) this.Image, new Rectangle((64 - width) / 2, (64 - height) / 2, width, height), new Rectangle(0, 0, (int) this.Width, (int) this.Height), GraphicsUnit.Pixel);
            graphics.Save();
          }
        }
      }
    }
  }
}
