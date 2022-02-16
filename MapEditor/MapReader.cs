using System;
using System.IO;

namespace Map_Editor
{
  public class MapReader
  {
    public int Width;
    public int Height;
    public CellInfo[,] MapCells;
    private string FileName;
    private byte[] Bytes;

    public MapReader(string FileName)
    {
      this.FileName = FileName;
      this.initiate();
    }

    private void initiate()
    {
      if (File.Exists(this.FileName))
      {
        this.Bytes = File.ReadAllBytes(this.FileName);
        if (this.Bytes[2] == (byte) 67 && this.Bytes[3] == (byte) 35)
          this.LoadMapType100();
        else if (this.Bytes[0] == (byte) 0)
          this.LoadMapType5();
        else if (this.Bytes[0] == (byte) 15 && this.Bytes[5] == (byte) 83 && this.Bytes[14] == (byte) 51)
          this.LoadMapType6();
        else if (this.Bytes[0] == (byte) 21 && this.Bytes[4] == (byte) 50 && this.Bytes[6] == (byte) 65 && this.Bytes[19] == (byte) 49)
          this.LoadMapType4();
        else if (this.Bytes[0] == (byte) 16 && this.Bytes[2] == (byte) 97 && this.Bytes[7] == (byte) 49 && this.Bytes[14] == (byte) 49)
          this.LoadMapType1();
        else if (this.Bytes[4] == (byte) 15 && this.Bytes[18] == (byte) 13 && this.Bytes[19] == (byte) 10)
        {
          if (this.Bytes.Length > 52 + ((int) this.Bytes[0] + ((int) this.Bytes[1] << 8)) * ((int) this.Bytes[2] + ((int) this.Bytes[3] << 8)) * 14)
            this.LoadMapType3();
          else
            this.LoadMapType2();
        }
        else if (this.Bytes[0] == (byte) 13 && this.Bytes[1] == (byte) 76 && this.Bytes[7] == (byte) 32 && this.Bytes[11] == (byte) 109)
          this.LoadMapType7();
        else
          this.LoadMapType0();
      }
      else
      {
        this.Width = 1000;
        this.Height = 1000;
        this.MapCells = new CellInfo[this.Width, this.Height];
        for (int index1 = 0; index1 < this.Width; ++index1)
        {
          for (int index2 = 0; index2 < this.Height; ++index2)
            this.MapCells[index1, index2] = new CellInfo();
        }
      }
    }

    private void LoadMapType0()
    {
      try
      {
        int startIndex1 = 0;
        this.Width = (int) BitConverter.ToInt16(this.Bytes, startIndex1);
        this.Height = (int) BitConverter.ToInt16(this.Bytes, startIndex1 + 2);
        this.MapCells = new CellInfo[this.Width, this.Height];
        int startIndex2 = 52;
        for (int index1 = 0; index1 < this.Width; ++index1)
        {
          for (int index2 = 0; index2 < this.Height; ++index2)
          {
            this.MapCells[index1, index2] = new CellInfo();
            this.MapCells[index1, index2].BackIndex = (short) 0;
            this.MapCells[index1, index2].MiddleIndex = (short) 1;
            this.MapCells[index1, index2].BackImage = (int) BitConverter.ToInt16(this.Bytes, startIndex2);
            int startIndex3 = startIndex2 + 2;
            this.MapCells[index1, index2].MiddleImage = BitConverter.ToInt16(this.Bytes, startIndex3);
            int startIndex4 = startIndex3 + 2;
            this.MapCells[index1, index2].FrontImage = BitConverter.ToInt16(this.Bytes, startIndex4);
            int num1 = startIndex4 + 2;
            CellInfo mapCell1 = this.MapCells[index1, index2];
            byte[] bytes1 = this.Bytes;
            int index3 = num1;
            int num2 = index3 + 1;
            int num3 = (int) bytes1[index3];
            mapCell1.DoorIndex = (byte) num3;
            CellInfo mapCell2 = this.MapCells[index1, index2];
            byte[] bytes2 = this.Bytes;
            int index4 = num2;
            int num4 = index4 + 1;
            int num5 = (int) bytes2[index4];
            mapCell2.DoorOffset = (byte) num5;
            CellInfo mapCell3 = this.MapCells[index1, index2];
            byte[] bytes3 = this.Bytes;
            int index5 = num4;
            int num6 = index5 + 1;
            int num7 = (int) bytes3[index5];
            mapCell3.FrontAnimationFrame = (byte) num7;
            CellInfo mapCell4 = this.MapCells[index1, index2];
            byte[] bytes4 = this.Bytes;
            int index6 = num6;
            int num8 = index6 + 1;
            int num9 = (int) bytes4[index6];
            mapCell4.FrontAnimationTick = (byte) num9;
            CellInfo mapCell5 = this.MapCells[index1, index2];
            byte[] bytes5 = this.Bytes;
            int index7 = num8;
            int num10 = index7 + 1;
            int num11 = (int) (short) ((int) bytes5[index7] + 2);
            mapCell5.FrontIndex = (short) num11;
            CellInfo mapCell6 = this.MapCells[index1, index2];
            byte[] bytes6 = this.Bytes;
            int index8 = num10;
            startIndex2 = index8 + 1;
            int num12 = (int) bytes6[index8];
            mapCell6.Light = (byte) num12;
            if ((uint) (this.MapCells[index1, index2].BackImage & 32768) > 0U)
              this.MapCells[index1, index2].BackImage = this.MapCells[index1, index2].BackImage & (int) short.MaxValue | 536870912;
            if (this.MapCells[index1, index2].Light == (byte) 100 || this.MapCells[index1, index2].Light == (byte) 101)
              this.MapCells[index1, index2].FishingCell = true;
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void LoadMapType1()
    {
      try
      {
        int startIndex1 = 21;
        int int16_1 = (int) BitConverter.ToInt16(this.Bytes, startIndex1);
        int startIndex2 = startIndex1 + 2;
        int int16_2 = (int) BitConverter.ToInt16(this.Bytes, startIndex2);
        int int16_3 = (int) BitConverter.ToInt16(this.Bytes, startIndex2 + 2);
        this.Width = int16_1 ^ int16_2;
        this.Height = int16_3 ^ int16_2;
        this.MapCells = new CellInfo[this.Width, this.Height];
        int startIndex3 = 54;
        for (int index1 = 0; index1 < this.Width; ++index1)
        {
          for (int index2 = 0; index2 < this.Height; ++index2)
          {
            int num1;
            int num2;
            int num3;
            int num4;
            int num5;
            int num6;
            int num7;
            int num8;
            int num9;
            this.MapCells[index1, index2] = new CellInfo()
            {
              BackIndex = (short) 0,
              BackImage = (int) ((long) BitConverter.ToInt32(this.Bytes, startIndex3) ^ 2855840312L),
              MiddleIndex = (short) 1,
              MiddleImage = (short) ((int) BitConverter.ToInt16(this.Bytes, num1 = startIndex3 + 4) ^ int16_2),
              FrontImage = (short) ((int) BitConverter.ToInt16(this.Bytes, num2 = num1 + 2) ^ int16_2),
              DoorIndex = this.Bytes[num3 = num2 + 2],
              DoorOffset = this.Bytes[num4 = num3 + 1],
              FrontAnimationFrame = this.Bytes[num5 = num4 + 1],
              FrontAnimationTick = this.Bytes[num6 = num5 + 1],
              FrontIndex = (short) ((int) this.Bytes[num7 = num6 + 1] + 2),
              Light = this.Bytes[num8 = num7 + 1],
              Unknown = this.Bytes[num9 = num8 + 1]
            };
            startIndex3 = num9 + 1;
            if (this.MapCells[index1, index2].Light == (byte) 100 || this.MapCells[index1, index2].Light == (byte) 101)
              this.MapCells[index1, index2].FishingCell = true;
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void LoadMapType2()
    {
      try
      {
        int startIndex1 = 0;
        this.Width = (int) BitConverter.ToInt16(this.Bytes, startIndex1);
        this.Height = (int) BitConverter.ToInt16(this.Bytes, startIndex1 + 2);
        this.MapCells = new CellInfo[this.Width, this.Height];
        int startIndex2 = 52;
        for (int index1 = 0; index1 < this.Width; ++index1)
        {
          for (int index2 = 0; index2 < this.Height; ++index2)
          {
            this.MapCells[index1, index2] = new CellInfo();
            this.MapCells[index1, index2].BackImage = (int) BitConverter.ToInt16(this.Bytes, startIndex2);
            int startIndex3 = startIndex2 + 2;
            this.MapCells[index1, index2].MiddleImage = BitConverter.ToInt16(this.Bytes, startIndex3);
            int startIndex4 = startIndex3 + 2;
            this.MapCells[index1, index2].FrontImage = BitConverter.ToInt16(this.Bytes, startIndex4);
            int num1 = startIndex4 + 2;
            CellInfo mapCell1 = this.MapCells[index1, index2];
            byte[] bytes1 = this.Bytes;
            int index3 = num1;
            int num2 = index3 + 1;
            int num3 = (int) bytes1[index3];
            mapCell1.DoorIndex = (byte) num3;
            CellInfo mapCell2 = this.MapCells[index1, index2];
            byte[] bytes2 = this.Bytes;
            int index4 = num2;
            int num4 = index4 + 1;
            int num5 = (int) bytes2[index4];
            mapCell2.DoorOffset = (byte) num5;
            CellInfo mapCell3 = this.MapCells[index1, index2];
            byte[] bytes3 = this.Bytes;
            int index5 = num4;
            int num6 = index5 + 1;
            int num7 = (int) bytes3[index5];
            mapCell3.FrontAnimationFrame = (byte) num7;
            CellInfo mapCell4 = this.MapCells[index1, index2];
            byte[] bytes4 = this.Bytes;
            int index6 = num6;
            int num8 = index6 + 1;
            int num9 = (int) bytes4[index6];
            mapCell4.FrontAnimationTick = (byte) num9;
            CellInfo mapCell5 = this.MapCells[index1, index2];
            byte[] bytes5 = this.Bytes;
            int index7 = num8;
            int num10 = index7 + 1;
            int num11 = (int) (short) ((int) bytes5[index7] + 120);
            mapCell5.FrontIndex = (short) num11;
            CellInfo mapCell6 = this.MapCells[index1, index2];
            byte[] bytes6 = this.Bytes;
            int index8 = num10;
            int num12 = index8 + 1;
            int num13 = (int) bytes6[index8];
            mapCell6.Light = (byte) num13;
            CellInfo mapCell7 = this.MapCells[index1, index2];
            byte[] bytes7 = this.Bytes;
            int index9 = num12;
            int num14 = index9 + 1;
            int num15 = (int) (short) ((int) bytes7[index9] + 100);
            mapCell7.BackIndex = (short) num15;
            CellInfo mapCell8 = this.MapCells[index1, index2];
            byte[] bytes8 = this.Bytes;
            int index10 = num14;
            startIndex2 = index10 + 1;
            int num16 = (int) (short) ((int) bytes8[index10] + 110);
            mapCell8.MiddleIndex = (short) num16;
            if ((uint) (this.MapCells[index1, index2].BackImage & 32768) > 0U)
              this.MapCells[index1, index2].BackImage = this.MapCells[index1, index2].BackImage & (int) short.MaxValue | 536870912;
            if (this.MapCells[index1, index2].Light == (byte) 100 || this.MapCells[index1, index2].Light == (byte) 101)
              this.MapCells[index1, index2].FishingCell = true;
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void LoadMapType3()
    {
      try
      {
        int startIndex1 = 0;
        this.Width = (int) BitConverter.ToInt16(this.Bytes, startIndex1);
        this.Height = (int) BitConverter.ToInt16(this.Bytes, startIndex1 + 2);
        this.MapCells = new CellInfo[this.Width, this.Height];
        int startIndex2 = 52;
        for (int index1 = 0; index1 < this.Width; ++index1)
        {
          for (int index2 = 0; index2 < this.Height; ++index2)
          {
            this.MapCells[index1, index2] = new CellInfo();
            this.MapCells[index1, index2].BackImage = (int) BitConverter.ToInt16(this.Bytes, startIndex2);
            int startIndex3 = startIndex2 + 2;
            this.MapCells[index1, index2].MiddleImage = BitConverter.ToInt16(this.Bytes, startIndex3);
            int startIndex4 = startIndex3 + 2;
            this.MapCells[index1, index2].FrontImage = BitConverter.ToInt16(this.Bytes, startIndex4);
            int num1 = startIndex4 + 2;
            CellInfo mapCell1 = this.MapCells[index1, index2];
            byte[] bytes1 = this.Bytes;
            int index3 = num1;
            int num2 = index3 + 1;
            int num3 = (int) bytes1[index3];
            mapCell1.DoorIndex = (byte) num3;
            CellInfo mapCell2 = this.MapCells[index1, index2];
            byte[] bytes2 = this.Bytes;
            int index4 = num2;
            int num4 = index4 + 1;
            int num5 = (int) bytes2[index4];
            mapCell2.DoorOffset = (byte) num5;
            CellInfo mapCell3 = this.MapCells[index1, index2];
            byte[] bytes3 = this.Bytes;
            int index5 = num4;
            int num6 = index5 + 1;
            int num7 = (int) bytes3[index5];
            mapCell3.FrontAnimationFrame = (byte) num7;
            CellInfo mapCell4 = this.MapCells[index1, index2];
            byte[] bytes4 = this.Bytes;
            int index6 = num6;
            int num8 = index6 + 1;
            int num9 = (int) bytes4[index6];
            mapCell4.FrontAnimationTick = (byte) num9;
            CellInfo mapCell5 = this.MapCells[index1, index2];
            byte[] bytes5 = this.Bytes;
            int index7 = num8;
            int num10 = index7 + 1;
            int num11 = (int) (short) ((int) bytes5[index7] + 120);
            mapCell5.FrontIndex = (short) num11;
            CellInfo mapCell6 = this.MapCells[index1, index2];
            byte[] bytes6 = this.Bytes;
            int index8 = num10;
            int num12 = index8 + 1;
            int num13 = (int) bytes6[index8];
            mapCell6.Light = (byte) num13;
            CellInfo mapCell7 = this.MapCells[index1, index2];
            byte[] bytes7 = this.Bytes;
            int index9 = num12;
            int num14 = index9 + 1;
            int num15 = (int) (short) ((int) bytes7[index9] + 100);
            mapCell7.BackIndex = (short) num15;
            CellInfo mapCell8 = this.MapCells[index1, index2];
            byte[] bytes8 = this.Bytes;
            int index10 = num14;
            int startIndex5 = index10 + 1;
            int num16 = (int) (short) ((int) bytes8[index10] + 110);
            mapCell8.MiddleIndex = (short) num16;
            this.MapCells[index1, index2].TileAnimationImage = BitConverter.ToInt16(this.Bytes, startIndex5);
            int num17 = startIndex5 + 7;
            CellInfo mapCell9 = this.MapCells[index1, index2];
            byte[] bytes9 = this.Bytes;
            int index11 = num17;
            int startIndex6 = index11 + 1;
            int num18 = (int) bytes9[index11];
            mapCell9.TileAnimationFrames = (byte) num18;
            this.MapCells[index1, index2].TileAnimationOffset = BitConverter.ToInt16(this.Bytes, startIndex6);
            startIndex2 = startIndex6 + 14;
            if ((uint) (this.MapCells[index1, index2].BackImage & 32768) > 0U)
              this.MapCells[index1, index2].BackImage = this.MapCells[index1, index2].BackImage & (int) short.MaxValue | 536870912;
            if (this.MapCells[index1, index2].Light == (byte) 100 || this.MapCells[index1, index2].Light == (byte) 101)
              this.MapCells[index1, index2].FishingCell = true;
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void LoadMapType4()
    {
      try
      {
        int startIndex1 = 31;
        int int16_1 = (int) BitConverter.ToInt16(this.Bytes, startIndex1);
        int startIndex2 = startIndex1 + 2;
        int int16_2 = (int) BitConverter.ToInt16(this.Bytes, startIndex2);
        int int16_3 = (int) BitConverter.ToInt16(this.Bytes, startIndex2 + 2);
        this.Width = int16_1 ^ int16_2;
        this.Height = int16_3 ^ int16_2;
        this.MapCells = new CellInfo[this.Width, this.Height];
        int startIndex3 = 64;
        for (int index1 = 0; index1 < this.Width; ++index1)
        {
          for (int index2 = 0; index2 < this.Height; ++index2)
          {
            this.MapCells[index1, index2] = new CellInfo();
            this.MapCells[index1, index2].BackIndex = (short) 0;
            this.MapCells[index1, index2].MiddleIndex = (short) 1;
            this.MapCells[index1, index2].BackImage = (int) (short) ((int) BitConverter.ToInt16(this.Bytes, startIndex3) ^ int16_2);
            int startIndex4 = startIndex3 + 2;
            this.MapCells[index1, index2].MiddleImage = (short) ((int) BitConverter.ToInt16(this.Bytes, startIndex4) ^ int16_2);
            int startIndex5 = startIndex4 + 2;
            this.MapCells[index1, index2].FrontImage = (short) ((int) BitConverter.ToInt16(this.Bytes, startIndex5) ^ int16_2);
            int num1 = startIndex5 + 2;
            CellInfo mapCell1 = this.MapCells[index1, index2];
            byte[] bytes1 = this.Bytes;
            int index3 = num1;
            int num2 = index3 + 1;
            int num3 = (int) bytes1[index3];
            mapCell1.DoorIndex = (byte) num3;
            CellInfo mapCell2 = this.MapCells[index1, index2];
            byte[] bytes2 = this.Bytes;
            int index4 = num2;
            int num4 = index4 + 1;
            int num5 = (int) bytes2[index4];
            mapCell2.DoorOffset = (byte) num5;
            CellInfo mapCell3 = this.MapCells[index1, index2];
            byte[] bytes3 = this.Bytes;
            int index5 = num4;
            int num6 = index5 + 1;
            int num7 = (int) bytes3[index5];
            mapCell3.FrontAnimationFrame = (byte) num7;
            CellInfo mapCell4 = this.MapCells[index1, index2];
            byte[] bytes4 = this.Bytes;
            int index6 = num6;
            int num8 = index6 + 1;
            int num9 = (int) bytes4[index6];
            mapCell4.FrontAnimationTick = (byte) num9;
            CellInfo mapCell5 = this.MapCells[index1, index2];
            byte[] bytes5 = this.Bytes;
            int index7 = num8;
            int num10 = index7 + 1;
            int num11 = (int) (short) ((int) bytes5[index7] + 2);
            mapCell5.FrontIndex = (short) num11;
            CellInfo mapCell6 = this.MapCells[index1, index2];
            byte[] bytes6 = this.Bytes;
            int index8 = num10;
            startIndex3 = index8 + 1;
            int num12 = (int) bytes6[index8];
            mapCell6.Light = (byte) num12;
            if ((uint) (this.MapCells[index1, index2].BackImage & 32768) > 0U)
              this.MapCells[index1, index2].BackImage = this.MapCells[index1, index2].BackImage & (int) short.MaxValue | 536870912;
            if (this.MapCells[index1, index2].Light == (byte) 100 || this.MapCells[index1, index2].Light == (byte) 101)
              this.MapCells[index1, index2].FishingCell = true;
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void LoadMapType5()
    {
      try
      {
        int startIndex1 = 20;
        BitConverter.ToInt16(this.Bytes, startIndex1);
        int num1;
        this.Width = (int) BitConverter.ToInt16(this.Bytes, num1 = startIndex1 + 2);
        int num2;
        this.Height = (int) BitConverter.ToInt16(this.Bytes, num2 = num1 + 2);
        int index1 = 28;
        this.MapCells = new CellInfo[this.Width, this.Height];
        for (int index2 = 0; index2 < this.Width; ++index2)
        {
          for (int index3 = 0; index3 < this.Height; ++index3)
            this.MapCells[index2, index3] = new CellInfo();
        }
        for (int index4 = 0; index4 < this.Width / 2; ++index4)
        {
          for (int index5 = 0; index5 < this.Height / 2; ++index5)
          {
            for (int index6 = 0; index6 < 4; ++index6)
            {
              this.MapCells[index4 * 2 + index6 % 2, index5 * 2 + index6 / 2].BackIndex = this.Bytes[index1] != byte.MaxValue ? (short) ((int) this.Bytes[index1] + 200) : (short) -1;
              this.MapCells[index4 * 2 + index6 % 2, index5 * 2 + index6 / 2].BackImage = (int) BitConverter.ToInt16(this.Bytes, index1 + 1) + 1;
            }
            index1 += 3;
          }
        }
        int num3 = 28 + 3 * (this.Width / 2 + this.Width % 2) * (this.Height / 2);
        for (int index7 = 0; index7 < this.Width; ++index7)
        {
          for (int index8 = 0; index8 < this.Height; ++index8)
          {
            byte[] bytes1 = this.Bytes;
            int index9 = num3;
            int num4 = index9 + 1;
            byte num5 = bytes1[index9];
            CellInfo mapCell = this.MapCells[index7, index8];
            byte[] bytes2 = this.Bytes;
            int index10 = num4;
            int index11 = index10 + 1;
            int num6 = (int) bytes2[index10];
            mapCell.MiddleAnimationFrame = (byte) num6;
            this.MapCells[index7, index8].FrontAnimationFrame = this.Bytes[index11] == byte.MaxValue ? (byte) 0 : this.Bytes[index11];
            this.MapCells[index7, index8].FrontAnimationFrame &= (byte) 143;
            int index12 = index11 + 1;
            this.MapCells[index7, index8].MiddleAnimationTick = (byte) 0;
            this.MapCells[index7, index8].FrontAnimationTick = (byte) 0;
            this.MapCells[index7, index8].FrontIndex = this.Bytes[index12] != byte.MaxValue ? (short) ((int) this.Bytes[index12] + 200) : (short) -1;
            int index13 = index12 + 1;
            this.MapCells[index7, index8].MiddleIndex = this.Bytes[index13] != byte.MaxValue ? (short) ((int) this.Bytes[index13] + 200) : (short) -1;
            int startIndex2 = index13 + 1;
            this.MapCells[index7, index8].MiddleImage = (short) ((int) BitConverter.ToInt16(this.Bytes, startIndex2) + 1);
            int startIndex3 = startIndex2 + 2;
            this.MapCells[index7, index8].FrontImage = (short) ((int) BitConverter.ToInt16(this.Bytes, startIndex3) + 1);
            if (this.MapCells[index7, index8].FrontImage == (short) 1 && this.MapCells[index7, index8].FrontIndex == (short) 200)
              this.MapCells[index7, index8].FrontIndex = (short) -1;
            int index14 = startIndex3 + 2 + 3;
            this.MapCells[index7, index8].Light = (byte) ((uint) this.Bytes[index14] & 15U);
            this.MapCells[index7, index8].Light *= (byte) 4;
            num3 = index14 + 2;
            if (((int) num5 & 1) != 1)
              this.MapCells[index7, index8].BackImage |= 536870912;
            if (((int) num5 & 2) != 2)
              this.MapCells[index7, index8].FrontImage = (short) ((int) (ushort) this.MapCells[index7, index8].FrontImage | 32768);
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void LoadMapType6()
    {
      try
      {
        int startIndex1 = 16;
        this.Width = (int) BitConverter.ToInt16(this.Bytes, startIndex1);
        this.Height = (int) BitConverter.ToInt16(this.Bytes, startIndex1 + 2);
        this.MapCells = new CellInfo[this.Width, this.Height];
        int num1 = 40;
        for (int index1 = 0; index1 < this.Width; ++index1)
        {
          for (int index2 = 0; index2 < this.Height; ++index2)
          {
            this.MapCells[index1, index2] = new CellInfo();
            byte[] bytes1 = this.Bytes;
            int index3 = num1;
            int index4 = index3 + 1;
            byte num2 = bytes1[index3];
            this.MapCells[index1, index2].BackIndex = this.Bytes[index4] != byte.MaxValue ? (short) ((int) this.Bytes[index4] + 300) : (short) -1;
            int index5 = index4 + 1;
            this.MapCells[index1, index2].MiddleIndex = this.Bytes[index5] != byte.MaxValue ? (short) ((int) this.Bytes[index5] + 300) : (short) -1;
            int index6 = index5 + 1;
            this.MapCells[index1, index2].FrontIndex = this.Bytes[index6] != byte.MaxValue ? (short) ((int) this.Bytes[index6] + 300) : (short) -1;
            int startIndex2 = index6 + 1;
            this.MapCells[index1, index2].BackImage = (int) (short) ((int) BitConverter.ToInt16(this.Bytes, startIndex2) + 1);
            int startIndex3 = startIndex2 + 2;
            this.MapCells[index1, index2].MiddleImage = (short) ((int) BitConverter.ToInt16(this.Bytes, startIndex3) + 1);
            int startIndex4 = startIndex3 + 2;
            this.MapCells[index1, index2].FrontImage = (short) ((int) BitConverter.ToInt16(this.Bytes, startIndex4) + 1);
            int num3 = startIndex4 + 2;
            if (this.MapCells[index1, index2].FrontImage == (short) 1 && this.MapCells[index1, index2].FrontIndex == (short) 200)
              this.MapCells[index1, index2].FrontIndex = (short) -1;
            CellInfo mapCell = this.MapCells[index1, index2];
            byte[] bytes2 = this.Bytes;
            int index7 = num3;
            int index8 = index7 + 1;
            int num4 = (int) bytes2[index7];
            mapCell.MiddleAnimationFrame = (byte) num4;
            this.MapCells[index1, index2].FrontAnimationFrame = this.Bytes[index8] == byte.MaxValue ? (byte) 0 : this.Bytes[index8];
            if (this.MapCells[index1, index2].FrontAnimationFrame > (byte) 15)
              this.MapCells[index1, index2].FrontAnimationFrame &= (byte) 15;
            int index9 = index8 + 1;
            this.MapCells[index1, index2].MiddleAnimationTick = (byte) 1;
            this.MapCells[index1, index2].FrontAnimationTick = (byte) 1;
            this.MapCells[index1, index2].Light = (byte) ((uint) this.Bytes[index9] & 15U);
            this.MapCells[index1, index2].Light *= (byte) 4;
            num1 = index9 + 8;
            if (((int) num2 & 1) != 1)
              this.MapCells[index1, index2].BackImage |= 536870912;
            if (((int) num2 & 2) != 2)
              this.MapCells[index1, index2].FrontImage = (short) ((int) (ushort) this.MapCells[index1, index2].FrontImage | 32768);
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void LoadMapType7()
    {
      try
      {
        int startIndex1 = 21;
        this.Width = (int) BitConverter.ToInt16(this.Bytes, startIndex1);
        this.Height = (int) BitConverter.ToInt16(this.Bytes, startIndex1 + 4);
        this.MapCells = new CellInfo[this.Width, this.Height];
        int startIndex2 = 54;
        for (int index1 = 0; index1 < this.Width; ++index1)
        {
          for (int index2 = 0; index2 < this.Height; ++index2)
          {
            int num1;
            int num2;
            int num3;
            int num4;
            int num5;
            int num6;
            int num7;
            int num8;
            int num9;
            this.MapCells[index1, index2] = new CellInfo()
            {
              BackIndex = (short) 0,
              BackImage = BitConverter.ToInt32(this.Bytes, startIndex2),
              MiddleIndex = (short) 1,
              MiddleImage = BitConverter.ToInt16(this.Bytes, num1 = startIndex2 + 4),
              FrontImage = BitConverter.ToInt16(this.Bytes, num2 = num1 + 2),
              DoorIndex = this.Bytes[num3 = num2 + 2],
              DoorOffset = this.Bytes[num4 = num3 + 1],
              FrontAnimationFrame = this.Bytes[num5 = num4 + 1],
              FrontAnimationTick = this.Bytes[num6 = num5 + 1],
              FrontIndex = (short) ((int) this.Bytes[num7 = num6 + 1] + 2),
              Light = this.Bytes[num8 = num7 + 1],
              Unknown = this.Bytes[num9 = num8 + 1]
            };
            if ((uint) (this.MapCells[index1, index2].BackImage & 32768) > 0U)
              this.MapCells[index1, index2].BackImage = this.MapCells[index1, index2].BackImage & (int) short.MaxValue | 536870912;
            startIndex2 = num9 + 1;
            if (this.MapCells[index1, index2].Light == (byte) 100 || this.MapCells[index1, index2].Light == (byte) 101)
              this.MapCells[index1, index2].FishingCell = true;
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void LoadMapType100()
    {
      try
      {
        int startIndex1 = 4;
        if (this.Bytes[0] != (byte) 1 || this.Bytes[1] > (byte) 0)
          return;
        this.Width = (int) BitConverter.ToInt16(this.Bytes, startIndex1);
        this.Height = (int) BitConverter.ToInt16(this.Bytes, startIndex1 + 2);
        this.MapCells = new CellInfo[this.Width, this.Height];
        int startIndex2 = 8;
        for (int index1 = 0; index1 < this.Width; ++index1)
        {
          for (int index2 = 0; index2 < this.Height; ++index2)
          {
            this.MapCells[index1, index2] = new CellInfo();
            this.MapCells[index1, index2].BackIndex = BitConverter.ToInt16(this.Bytes, startIndex2);
            int startIndex3 = startIndex2 + 2;
            this.MapCells[index1, index2].BackImage = BitConverter.ToInt32(this.Bytes, startIndex3);
            int startIndex4 = startIndex3 + 4;
            this.MapCells[index1, index2].MiddleIndex = BitConverter.ToInt16(this.Bytes, startIndex4);
            int startIndex5 = startIndex4 + 2;
            this.MapCells[index1, index2].MiddleImage = BitConverter.ToInt16(this.Bytes, startIndex5);
            int startIndex6 = startIndex5 + 2;
            this.MapCells[index1, index2].FrontIndex = BitConverter.ToInt16(this.Bytes, startIndex6);
            int startIndex7 = startIndex6 + 2;
            this.MapCells[index1, index2].FrontImage = BitConverter.ToInt16(this.Bytes, startIndex7);
            int num1 = startIndex7 + 2;
            CellInfo mapCell1 = this.MapCells[index1, index2];
            byte[] bytes1 = this.Bytes;
            int index3 = num1;
            int num2 = index3 + 1;
            int num3 = (int) bytes1[index3];
            mapCell1.DoorIndex = (byte) num3;
            CellInfo mapCell2 = this.MapCells[index1, index2];
            byte[] bytes2 = this.Bytes;
            int index4 = num2;
            int num4 = index4 + 1;
            int num5 = (int) bytes2[index4];
            mapCell2.DoorOffset = (byte) num5;
            CellInfo mapCell3 = this.MapCells[index1, index2];
            byte[] bytes3 = this.Bytes;
            int index5 = num4;
            int num6 = index5 + 1;
            int num7 = (int) bytes3[index5];
            mapCell3.FrontAnimationFrame = (byte) num7;
            CellInfo mapCell4 = this.MapCells[index1, index2];
            byte[] bytes4 = this.Bytes;
            int index6 = num6;
            int num8 = index6 + 1;
            int num9 = (int) bytes4[index6];
            mapCell4.FrontAnimationTick = (byte) num9;
            CellInfo mapCell5 = this.MapCells[index1, index2];
            byte[] bytes5 = this.Bytes;
            int index7 = num8;
            int num10 = index7 + 1;
            int num11 = (int) bytes5[index7];
            mapCell5.MiddleAnimationFrame = (byte) num11;
            CellInfo mapCell6 = this.MapCells[index1, index2];
            byte[] bytes6 = this.Bytes;
            int index8 = num10;
            int startIndex8 = index8 + 1;
            int num12 = (int) bytes6[index8];
            mapCell6.MiddleAnimationTick = (byte) num12;
            this.MapCells[index1, index2].TileAnimationImage = BitConverter.ToInt16(this.Bytes, startIndex8);
            int startIndex9 = startIndex8 + 2;
            this.MapCells[index1, index2].TileAnimationOffset = BitConverter.ToInt16(this.Bytes, startIndex9);
            int num13 = startIndex9 + 2;
            CellInfo mapCell7 = this.MapCells[index1, index2];
            byte[] bytes7 = this.Bytes;
            int index9 = num13;
            int num14 = index9 + 1;
            int num15 = (int) bytes7[index9];
            mapCell7.TileAnimationFrames = (byte) num15;
            CellInfo mapCell8 = this.MapCells[index1, index2];
            byte[] bytes8 = this.Bytes;
            int index10 = num14;
            startIndex2 = index10 + 1;
            int num16 = (int) bytes8[index10];
            mapCell8.Light = (byte) num16;
            if (this.MapCells[index1, index2].Light == (byte) 100 || this.MapCells[index1, index2].Light == (byte) 101)
              this.MapCells[index1, index2].FishingCell = true;
          }
        }
      }
      catch (Exception ex)
      {
      }
    }
  }
}
