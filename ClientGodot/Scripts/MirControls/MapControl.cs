using System;
using System.IO;
using System.Drawing; // For Point
using Godot;
using ClientGodot.Scripts.MirGraphics;
using ClientGodot.Scripts.MirScenes; // Add this

using System.Collections.Generic; // Add
using ClientGodot.Scripts.MirObjects; // Add

namespace ClientGodot.Scripts.MirControls
{
    public partial class MapControl : Node2D
    {
        public int Width, Height;
        public Cell[,] Cells;

        // Entity Manager
        public List<MapObject> MapObjects = new List<MapObject>();

        private string _mapFileName;
        private Point _userLocation;

        // Tile constants
        public const int CellWidth = 48;
        public const int CellHeight = 32;
        
        // Map format detection constants
        private const int MinHeaderSize = 20; // Minimum bytes needed for format detection

        private byte FindMapType(byte[] input)
        {
            // Check minimum length for safe header detection
            if (input.Length < MinHeaderSize) return 0;
            
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

                // Validate dimensions to prevent integer overflow
                if (W <= 0 || H <= 0 || W > 2000 || H > 2000)
                    return 0;

                // Use long to prevent overflow in calculation
                long expectedSize = 52 + ((long)W * H * 14);
                if (input.Length > expectedSize)
                    return 3;
                else
                    return 2;
            }
            if (input.Length >= 12 && (input[0] == 0x0D) && (input[1] == 0x4C) && (input[7] == 0x20) && (input[11] == 0x6D)) // Mir 3/4 Heroes
                return 7;
            if (input.Length >= 5 && (input[0] == 0xC8) && (input[2] == 0xC8) && (input[4] == 0x0D)) // Shortys Map Save
                return 8;
            if (input.Length >= 4 && (input[2] == 0x43) && (input[3] == 0x23)) //C#
                return 100;

            return 0;
        }

        private void LoadMapCellsv0(byte[] fileBytes)
        {
            int offSet = 0;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            // Validate file size: header (52) + (Width * Height * bytes_per_cell)
            long expectedSize = 52 + ((long)Width * Height * 14);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            offSet = 52;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 14 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    Cells[x, y].BackIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].MiddleIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 6; // Skip door, animation, light bytes
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 1; // Skip additional byte
                }
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
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            // Validate file size
            long expectedSize = 54 + ((long)Width * Height * 15);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            offSet = 54;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 15 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    // WeMade 2010 format uses XOR encryption for obfuscation
                    // BackIndex is encrypted as a 32-bit int with key 0xAA38AA38
                    int backData = (int)(BitConverter.ToInt32(fileBytes, offSet) ^ 0xAA38AA38);
                    Cells[x, y].BackIndex = (short)(backData & 0xFFFF);
                    offSet += 6;
                    // MiddleIndex is XORed with the XOR key from the header
                    Cells[x, y].MiddleIndex = (short)(BitConverter.ToInt16(fileBytes, offSet) ^ xor);
                    offSet += 2;
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 4; // Skip remaining bytes
                }
        }

        private void LoadMapCellsv2(byte[] fileBytes)
        {
            int offSet = 0;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            // Validate file size
            long expectedSize = 52 + ((long)Width * Height * 14);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            offSet = 52;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 14 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    Cells[x, y].BackIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].MiddleIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 6; // Skip door, animation bytes
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 1; // Skip light
                }
        }

        private void LoadMapCellsv3(byte[] fileBytes)
        {
            int offSet = 0;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            // Validate file size
            long expectedSize = 52 + ((long)Width * Height * 36);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            offSet = 52;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 36 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    Cells[x, y].BackIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].MiddleIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 28; // Skip additional bytes (total 36 bytes per cell)
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 5; // Skip remaining bytes
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
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            // Validate file size
            long expectedSize = 64 + ((long)Width * Height * 12);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            offSet = 64;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 12 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    Cells[x, y].BackIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].MiddleIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 4; // Skip door, animation bytes
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 1; // Skip light
                }
        }

        private void LoadMapCellsv5(byte[] fileBytes)
        {
            int offSet = 22;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            // WeMade Mir 3 offset calculation: 
            // 28 byte header + light map data (3 bytes per 2x2 block)
            // Light map size = 3 * ceil(Width/2) * floor(Height/2)
            int startOffset = 28 + (3 * ((Width / 2) + (Width % 2)) * (Height / 2));
            offSet = startOffset;
            
            // Validate file size: startOffset + (Width * Height * 14)
            long expectedSize = startOffset + ((long)Width * Height * 14);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 14 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    offSet += 1; // Skip flags byte
                    Cells[x, y].BackIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].MiddleIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 5; // Skip door, animation bytes
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 1; // Skip light
                }
        }

        private void LoadMapCellsv6(byte[] fileBytes)
        {
            int offSet = 16;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            // Validate file size
            long expectedSize = 40 + ((long)Width * Height * 20);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            offSet = 40;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 20 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    offSet += 1; // Skip flags byte
                    Cells[x, y].BackIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].MiddleIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 11; // Skip door, animation, light bytes (total 20 bytes per cell)
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 1; // Skip remaining byte
                }
        }

        private void LoadMapCellsv7(byte[] fileBytes)
        {
            int offSet = 21;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 4;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            // Validate file size
            long expectedSize = 54 + ((long)Width * Height * 15);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            offSet = 54;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 15 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    Cells[x, y].BackIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].MiddleIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 5; // Skip door, animation bytes
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 2; // Skip remaining bytes
                }
        }

        private void LoadMapCellsv8(byte[] fileBytes)
        {
            int offSet = 0;
            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            // Validate file size
            long expectedSize = 52 + ((long)Width * Height * 12);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            offSet = 52;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 12 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    Cells[x, y].BackIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].MiddleIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 4; // Skip door, animation bytes
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 1; // Skip light
                }
        }

        private void LoadMapCellsv100(byte[] fileBytes)
        {
            int offSet = 4;

            if ((fileBytes[0] != 1) || (fileBytes[1] != 0))
            {
                GD.PrintErr("Unsupported C# map version");
                Cells = null;
                return;
            }

            Width = BitConverter.ToInt16(fileBytes, offSet);
            offSet += 2;
            Height = BitConverter.ToInt16(fileBytes, offSet);
            
            if (Width <= 0 || Height <= 0 || Width > 2000 || Height > 2000)
            {
                GD.PrintErr($"Invalid map dimensions: {Width}x{Height}");
                Cells = null;
                return;
            }
            
            // Validate file size: 8 byte header + (Width * Height * 28 bytes per cell)
            // v100 C# format cell structure: 
            // 2 bytes (padding) + 2 (back) + 8 (skip) + 2 (middle) + 12 (skip) + 2 (front) = 28 bytes
            long expectedSize = 8 + ((long)Width * Height * 28);
            if (fileBytes.Length < expectedSize)
            {
                GD.PrintErr($"File too small: {fileBytes.Length} bytes, expected at least {expectedSize}");
                Cells = null;
                return;
            }
            
            Cells = new Cell[Width, Height];
            offSet = 8;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (offSet + 28 > fileBytes.Length)
                    {
                        GD.PrintErr($"Unexpected end of file at offset {offSet}");
                        Cells = null;
                        return;
                    }
                    
                    Cells[x, y] = new Cell();
                    offSet += 2; // Skip initial bytes
                    Cells[x, y].BackIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 8; // Skip middle section
                    Cells[x, y].MiddleIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    offSet += 12; // Skip to front section
                    Cells[x, y].FrontIndex = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    Cells[x, y].FileIndex = fileBytes[offSet];
                    offSet += 1;
                    offSet += 1; // Skip remaining byte
                }
        }

        public void LoadMap(string fileName)
        {
            _mapFileName = fileName;
            string path = System.IO.Path.Combine(Settings.UserDataPath, "Map", fileName + ".map");
            path = path.Replace("\\", "/"); // Normalize

            if (!File.Exists(path))
            {
                GD.PrintErr($"Map file not found: {path}. Checked Root: {Settings.UserDataPath}");
                Cells = null;
                return;
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(path);
                
                // Check minimum file size for header detection
                if (fileBytes.Length < MinHeaderSize)
                {
                    GD.PrintErr($"Map file too small: {fileBytes.Length} bytes, need at least {MinHeaderSize}");
                    Cells = null;
                    return;
                }

                byte mapType = FindMapType(fileBytes);
                string formatName = "Unknown";

                switch (mapType)
                {
                    case 0:
                        LoadMapCellsv0(fileBytes);
                        formatName = "Default/Unknown";
                        break;
                    case 1:
                        LoadMapCellsv1(fileBytes);
                        formatName = "WEMADE 2010";
                        break;
                    case 2:
                        LoadMapCellsv2(fileBytes);
                        formatName = "Old SHANDA";
                        break;
                    case 3:
                        LoadMapCellsv3(fileBytes);
                        formatName = "SHANDA 2012";
                        break;
                    case 4:
                        LoadMapCellsv4(fileBytes);
                        formatName = "WEMADE Mir 2";
                        break;
                    case 5:
                        LoadMapCellsv5(fileBytes);
                        formatName = "WEMADE Mir 3";
                        break;
                    case 6:
                        LoadMapCellsv6(fileBytes);
                        formatName = "SHANDA Mir 3";
                        break;
                    case 7:
                        LoadMapCellsv7(fileBytes);
                        formatName = "Heroes";
                        break;
                    case 8:
                        LoadMapCellsv8(fileBytes);
                        formatName = "Shortys";
                        break;
                    case 100:
                        LoadMapCellsv100(fileBytes);
                        formatName = "C#";
                        break;
                }

                if (Cells != null)
                {
                    GD.Print($"Map loaded successfully: {fileName}");
                    GD.Print($"Format: {formatName}");
                    GD.Print($"Size: {Width}x{Height}");
                    QueueRedraw();
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to load map: {ex.Message}");
                GD.PrintErr($"Stack trace: {ex.StackTrace}");
                Cells = null;
            }
        }

        public void SetUserLocation(Point p)
        {
            _userLocation = p;
            QueueRedraw();
        }

        public void AddObject(MapObject obj)
        {
            if (obj == null) return;
            MapObjects.Add(obj);
        }

        public void RemoveObject(uint objectID)
        {
            for(int i = MapObjects.Count - 1; i >= 0; i--)
            {
                if (MapObjects[i].ObjectID == objectID)
                {
                    MapObjects.RemoveAt(i);
                    break;
                }
            }
        }

        public MapObject GetObjectAt(Point location)
        {
            // Simple linear search. Optimise with spatial hash later if needed.
            foreach (var obj in MapObjects)
            {
                if (obj.CurrentLocation == location && !obj.Dead)
                    return obj;
            }
            return null;
        }

        public Point GetMapLocation(Vector2 screenPos)
        {
            if (Cells == null) return Point.Empty;

            // Convert Screen -> World -> Grid
            // Screen Center = User Location

            var viewport = GetViewportRect();
            float centerX = viewport.Size.X / 2.0f;
            float centerY = viewport.Size.Y / 2.0f;

            float diffX = screenPos.X - centerX;
            float diffY = screenPos.Y - centerY;

            int cellX = _userLocation.X + (int)(diffX / CellWidth);
            int cellY = _userLocation.Y + (int)(diffY / CellHeight);

            return new Point(cellX, cellY);
        }

        public override void _Draw()
        {
            if (Cells == null) return;

            // Simple Culling
            // Viewport size
            var viewport = GetViewportRect();
            int viewW = (int)viewport.Size.X;
            int viewH = (int)viewport.Size.Y;

            // Center camera on user
            // User X, Y is in "Cells".
            // Screen coordinates:
            // Center of screen should correspond to User Location.

            // Calculate pixel offset
            float centerX = viewW / 2.0f;
            float centerY = viewH / 2.0f;

            // User pixel position in world
            float userWorldX = _userLocation.X * CellWidth;
            float userWorldY = _userLocation.Y * CellHeight;

            // Draw range
            int rangeX = (viewW / CellWidth) / 2 + 2;
            int rangeY = (viewH / CellHeight) / 2 + 2;

            int minX = Math.Max(0, _userLocation.X - rangeX);
            int maxX = Math.Min(Width, _userLocation.X + rangeX);
            int minY = Math.Max(0, _userLocation.Y - rangeY);
            int maxY = Math.Min(Height, _userLocation.Y + rangeY);

            for (int x = minX; x < maxX; x++)
            for (int y = minY; y < maxY; y++)
            {
                Cell cell = Cells[x, y];

                // Screen Position
                float drawX = centerX + ((x - _userLocation.X) * CellWidth);
                float drawY = centerY + ((y - _userLocation.Y) * CellHeight);

                // Draw Floor (BackIndex)
                if (cell.BackIndex != -1 && (cell.BackIndex & 0x7FFF) > 0)
                {
                    int index = cell.BackIndex & 0x7FFF;
                    // Which library?
                    // Usually MapLibs[0] or dependent on FileIndex?
                    // In standard Mir2: MapLibs[0] is Tiles, MapLibs[1] is SmTiles.
                    // Logic is complex.
                    // For prototype: Assume Tiles (Lib 0) if index > 0.

                    // Libraries.MapLibs might be null if not loaded.
                    if (Libraries.MapLibs != null && Libraries.MapLibs[0] != null)
                    {
                        var lib = Libraries.MapLibs[0];
                        // Get texture and draw
                        // Note: MImage logic handles offsets.
                        var img = lib.GetImage(index - 1); // Index often 1-based?
                        if (img != null)
                        {
                            var tex = img.CreateTexture();
                            if (tex != null)
                            {
                                DrawTexture(tex, new Vector2(drawX, drawY));
                            }
                        }
                    }
                }
            }

            // Draw User (Simple Circle for now)
            // DrawCircle(new Vector2(centerX, centerY), 10, Godot.Colors.Red);

            // Draw User Sprite
            if (GameScene.Scene.User != null)
            {
                // User is always at center of screen in this implementation
                GameScene.Scene.User.DrawOnCanvas(this, new Vector2(centerX, centerY));
            }

            // Draw Entities
            // We should sort them by Y first to handle occlusion properly
            // Simple depth sort:
            // MapObjects.Sort((a, b) => a.CurrentLocation.Y.CompareTo(b.CurrentLocation.Y));
            // But we need screen Y.

            foreach (var obj in MapObjects)
            {
                if (obj == GameScene.Scene.User) continue; // Skip user if already drawn (or draw here instead)

                // Calculate Screen Pos
                float objX = centerX + ((obj.CurrentLocation.X - _userLocation.X) * CellWidth);
                float objY = centerY + ((obj.CurrentLocation.Y - _userLocation.Y) * CellHeight);

                // Culling
                if (objX < -100 || objY < -100 || objX > viewW + 100 || objY > viewH + 100) continue;

                obj.DrawOnCanvas(this, new Vector2(objX, objY));
            }
        }

        public void Process()
        {
             foreach(var obj in MapObjects)
                 obj.Process();
        }

        public void CreateDamageIndicator(int damage, Point location)
        {
            var scene = GD.Load<PackedScene>("res://Scenes/Effects/DamageLabel.tscn");
            if (scene != null)
            {
                var lbl = scene.Instantiate<ClientGodot.Scripts.MirScenes.DamageLabel>();

                // Calculate position relative to MapControl
                // Location is Grid.
                // Screen Pos = Center + (Grid - User) * Cell
                // But MapControl is just a Node2D. The visual offset is handled in _Draw relative to Viewport.
                // Wait, if we add Label as Child of MapControl, we need to set its position in local space?
                // Or if MapControl _Draw logic uses pure immediate mode, then adding Children works differently.
                // Since MapControl is a Node2D, if we add a child at (100, 100), it stays at (100, 100).
                // But our "Camera" logic is inside _Draw (calculating offsets).
                // We don't actually move the MapControl node.
                // So adding a child Label at a static position will make it "stick" to the screen if we don't move it manually?
                // Actually, since we don't move MapControl.Position, the local space (0,0) is always top-left of viewport?
                // No, Node2D (0,0) is where we placed it.
                // We should probably convert Grid -> Screen coordinates and place the label there.

                var viewport = GetViewportRect();
                float centerX = viewport.Size.X / 2.0f;
                float centerY = viewport.Size.Y / 2.0f;
                float drawX = centerX + ((location.X - _userLocation.X) * CellWidth);
                float drawY = centerY + ((location.Y - _userLocation.Y) * CellHeight);

                lbl.Position = new Vector2(drawX, drawY - 20); // Above head
                lbl.Text = damage.ToString();
                AddChild(lbl);
            }
        }
    }

    public class Cell
    {
        public short BackIndex;
        public short MiddleIndex;
        public short FrontIndex;
        public byte FileIndex;
    }
}
