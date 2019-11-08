using System;
using System.Drawing;
using System.IO;

namespace Server.MirForms.VisualMapInfo.Class
{
    public class ReadMap
    {
        public int Width, Height, MonsterCount;
        public Cell[,] Cells;
        public long LightningTime, FireTime;
        public Bitmap clippingZone;
        public string mapFormat, mapFile;

        private byte FindType(byte[] input)
        {
            if (input[0] == 0) // Mir 3 WeMade
                return 5;
            if ((input[0] == 0x0F) && (input[5] == 0x53) && (input[14] == 0x33)) // Mir 3 Shanda
                return 6;
            if ((input[0] == 0x15) && (input[4] == 0x32) && (input[6] == 0x41) && (input[19] == 0x31)) // Mir WeMade AntiHack
                return 4;
            if ((input[0] == 0x10) && (input[2] == 0x61) && (input[7] == 0x31) && (input[14] == 0x31)) // Mir 2010 WeMade
                return 1;
            if ((input[4] == 0x0F) && (input[18] == 0x0D) && (input[19] == 0x0A)) // Mir 2012 Shanda
            {
                int W = input[0] + (input[1] << 8),
                    H = input[2] + (input[3] << 8);

                if (input.Length > (52 + (W * H * 14)))
                    return 3;
                else
                    return 2;
            }
            if ((input[0] == 0x0D) && (input[1] == 0x4C) && (input[7] == 0x20) && (input[11] == 0x6D)) // Mir 3/4 Heroes
                return 7;
            if ((input[0] == 0xC8) && (input[2] == 0xC8) && (input[4] == 0x0D)) // Shortys Map Save
                return 8;
            if ((input[2] == 0x43) && (input[3] == 0x23)) //C#
                return 100;

            return 0;
        }

        private void LoadMapCellsv0(byte[] fileBytes)
        {
            int offSet = 0;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            Cells = new Cell[Width, Height];

            offSet = 52;

            clippingZone = new Bitmap(Width, Height);

            LockBitmap BitLock = new LockBitmap(clippingZone);
            BitLock.LockBits();

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (Cells[x, y] == null)
                        BitLock.SetPixel(x, y, Color.WhiteSmoke);

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        BitLock.SetPixel(x, y, Color.Black);

                    offSet += 2;

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        BitLock.SetPixel(x, y, Color.Black);

                    offSet += 10;
                }
            BitLock.UnlockBits();
            clippingZone.Dispose();
        }

        private void LoadMapCellsv1(byte[] fileBytes)
        {
            int offSet = 21;

            int w = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            int xor = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            int h = BitConverter.ToInt16(fileBytes, offSet);
            Width = w ^ xor;
            Height = h ^ xor;
            Cells = new Cell[Width, Height];

            offSet = 54;

            clippingZone = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (Cells[x, y] == null)
                        clippingZone.SetPixel(x, y, Color.WhiteSmoke);

                    if (((BitConverter.ToInt32(fileBytes, offSet) ^ 0xAA38AA38) & 0x20000000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 6;

                    if (((BitConverter.ToInt16(fileBytes, offSet) ^ xor) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 9;
                }
        }

        private void LoadMapCellsv2(byte[] fileBytes)
        {
            int offSet = 0;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            Cells = new Cell[Width, Height];

            offSet = 52;

            clippingZone = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (Cells[x, y] == null)
                        clippingZone.SetPixel(x, y, Color.WhiteSmoke);

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 2;

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 12;
                }
        }

        private void LoadMapCellsv3(byte[] fileBytes)
        {
            int offSet = 0;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            Cells = new Cell[Width, Height];

            offSet = 52;

            clippingZone = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (Cells[x, y] == null)
                        clippingZone.SetPixel(x, y, Color.WhiteSmoke);

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 2;

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 34;
                }
        }

        private void LoadMapCellsv4(byte[] fileBytes)
        {
            int offSet = 31;
            int w = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            int xor = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            int h = BitConverter.ToInt16(fileBytes, offSet);
            Width = w ^ xor;
            Height = h ^ xor;
            Cells = new Cell[Width, Height];

            offSet = 64;

            clippingZone = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (Cells[x, y] == null)
                        clippingZone.SetPixel(x, y, Color.WhiteSmoke);

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 2;

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 10;
                }
        }

        private void LoadMapCellsv5(byte[] fileBytes)
        {
            int offSet = 22;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            Cells = new Cell[Width, Height];

            offSet = 28 + (3 * ((Width / 2) + (Width % 2)) * (Height / 2));

            clippingZone = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if ((fileBytes[offSet] & 0x01) != 1)
                        clippingZone.SetPixel(x, y, Color.Black);

                    else if ((fileBytes[offSet] & 0x02) != 2)
                        clippingZone.SetPixel(x, y, Color.Black);

                    else
                        clippingZone.SetPixel(x, y, Color.WhiteSmoke);

                    offSet += 14;
                }
        }

        private void LoadMapCellsv6(byte[] fileBytes)
        {
            int offSet = 16;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            Cells = new Cell[Width, Height];

            offSet = 40;

            clippingZone = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if ((fileBytes[offSet] & 0x01) != 1)
                        clippingZone.SetPixel(x, y, Color.Black);

                    else if ((fileBytes[offSet] & 0x02) != 2)
                        clippingZone.SetPixel(x, y, Color.Black);

                    else
                        clippingZone.SetPixel(x, y, Color.WhiteSmoke);

                    offSet += 20;
                }
        }

        private void LoadMapCellsv7(byte[] fileBytes)
        {
            int offSet = 21;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 4;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            Cells = new Cell[Width, Height];

            offSet = 54;

            clippingZone = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 2;

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    if (Cells[x, y] == null)
                        clippingZone.SetPixel(x, y, Color.WhiteSmoke);

                    offSet += 13;
                }
        }

        private void LoadMapCellsv8(byte[] fileBytes)
        {
            int offSet = 0;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            Cells = new Cell[Width, Height];

            offSet = 52;

            clippingZone = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (Cells[x, y] == null)
                        clippingZone.SetPixel(x, y, Color.WhiteSmoke);

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 2;

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 10;
                }
        }

        private void LoadMapCellsv100(byte[] fileBytes)
        {
            int offSet = 4;

            if ((fileBytes[0] != 1) || (fileBytes[1] != 0)) return;//only support version 1 atm;

            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            Cells = new Cell[Width, Height];

            offSet = 8;

            clippingZone = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    offSet += 2;

                    if (Cells[x, y] == null)
                        clippingZone.SetPixel(x, y, Color.WhiteSmoke);

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 10;

                    if ((BitConverter.ToInt16(fileBytes, offSet) & 0x8000) != 0)
                        clippingZone.SetPixel(x, y, Color.Black);

                    offSet += 14;
                }
        }

        public void Load()
        {
            try
            {
                if (File.Exists(@".\Maps\" + mapFile + ".map"))
                {
                    byte[] fileBytes = File.ReadAllBytes(@".\Maps\" + mapFile + ".map");

                    switch (FindType(fileBytes))
                    {
                        case 0:
                            LoadMapCellsv0(fileBytes);
                            mapFormat = "?";
                            break;

                        case 1:
                            LoadMapCellsv1(fileBytes);
                            mapFormat = "WEMADE 2010";
                            break;

                        case 2:
                            LoadMapCellsv2(fileBytes);
                            mapFormat = "Old SHANDA";
                            break;

                        case 3:
                            LoadMapCellsv3(fileBytes);
                            mapFormat = "SHANDA 2012";
                            break;

                        case 4:
                            LoadMapCellsv4(fileBytes);
                            mapFormat = "WEMADE Mir 2";
                            break;

                        case 5:
                            LoadMapCellsv5(fileBytes);
                            mapFormat = "WEMADE Mir 3";
                            break;

                        case 6:
                            LoadMapCellsv6(fileBytes);
                            mapFormat = "SHANDA Mir 3";
                            break;

                        case 7:
                            LoadMapCellsv7(fileBytes);
                            mapFormat = "Heroes";
                            break;

                        case 8:
                            LoadMapCellsv8(fileBytes);
                            mapFormat = "Shortys";
                            break;

                        case 100:
                            LoadMapCellsv100(fileBytes);
                            mapFormat = "C#";
                            break;
                    }
                }
            }

            catch (Exception) { }

            VisualizerGlobal.ClippingMap = clippingZone;
        }

        public Cell GetCell(Point location)
        {
            return Cells[location.X, location.Y];
        }

        public Cell GetCell(int x, int y)
        {
            return Cells[x, y];
        }
    }

    public class Cell
    {
        public static readonly Cell HighWall;
        public static readonly Cell LowWall;
    }
}