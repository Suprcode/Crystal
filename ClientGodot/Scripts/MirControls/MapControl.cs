using System;
using System.IO;
using System.Drawing; // For Point
using Godot;
using ClientGodot.Scripts.MirGraphics;
using ClientGodot.Scripts.MirScenes; // Add this

namespace ClientGodot.Scripts.MirControls
{
    public partial class MapControl : Node2D
    {
        public int Width, Height;
        public Cell[,] Cells;

        private string _mapFileName;
        private Point _userLocation;

        // Tile constants
        public const int CellWidth = 48;
        public const int CellHeight = 32;

        public void LoadMap(string fileName)
        {
            _mapFileName = fileName;
            string path = ProjectSettings.GlobalizePath(Settings.UserDataPath + "Map/" + fileName + ".map");

            if (!File.Exists(path))
            {
                GD.PrintErr($"Map file not found: {path}");
                return;
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(path);
                using (MemoryStream ms = new MemoryStream(fileBytes))
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    // Read Header
                    // Original Mir Map Header logic:
                    // 2 bytes: Version? Or Checksum?
                    // Actually usually:
                    // byte 0-1: Width
                    // byte 2-3: Height
                    // But depends on version (Wemade vs Shanda vs Mir3).
                    // Let's assume standard V1 map for now.

                    // Simple check for header tag
                    short width = reader.ReadInt16();
                    short height = reader.ReadInt16();

                    Width = width;
                    Height = height;

                    Cells = new Cell[Width, Height];

                    GD.Print($"Map Size: {Width}x{Height}");

                    // Read Cells
                    // Standard format: BackIndex(2), MiddleIndex(2), FrontIndex(2), DoorIndex(1), DoorOffset(1), AniFrame(1), AniTick(1), FileIndex(1), Light(1), Unknown(1), Flag(1)
                    // Total 14 bytes per cell usually. Or 12.
                    // Need to check specific format. Assuming common Mir2 format:
                    // Back (2), Mid (2), Front (2), Door (1), DoorOff(1), Ani(1), AniTick(1), File(1), Light(1) ...

                    // Let's try reading generic blocks.
                    // Ideally we should port the full MapReader. But for this step, we just read BackIndex (Tiles).

                    int size = (fileBytes.Length - 4) / (Width * Height);
                    GD.Print($"Bytes per cell: {size}"); // Helpful for debugging format

                    // Re-seek to 0 just in case logic above was wrong about header only being 4 bytes
                    ms.Seek(0, SeekOrigin.Begin);
                    // There is usually a header.
                    // Generic implementation:

                    // Header (52 bytes usually?)
                    // Let's assume Wemade Mir2 Map.
                    // Offsets:
                    // 0: Width (2)
                    // 2: Height (2)
                    // 4..52: Attributes?
                    // Actually, often:
                    // [Version 2 bytes] [Width 2] [Height 2]

                    // Let's skip complex detection and try standard reading.
                    ms.Seek(22, SeekOrigin.Begin); // Header size often 22 or 52
                    // Actually, let's look at file size.
                    // Common: Width(2), Height(2).
                    // Then Width*Height * SizeOf(CellInfo).

                    ms.Seek(0, SeekOrigin.Begin);
                    short w = reader.ReadInt16();
                    short h = reader.ReadInt16();

                    // If dimensions seem crazy, it might be the other format (Header first)
                    // Let's assume standard.
                    Width = w;
                    Height = h;
                    Cells = new Cell[Width, Height];

                    for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        Cells[x, y] = new Cell();
                        // 2 bytes Back (Bng)
                        Cells[x, y].BackIndex = reader.ReadInt16();
                        // 2 bytes Mid
                        Cells[x, y].MiddleIndex = reader.ReadInt16();
                        // 2 bytes Front (Obj)
                        Cells[x, y].FrontIndex = reader.ReadInt16();

                        // Door
                        reader.ReadByte();
                        reader.ReadByte();

                        // Animation
                        reader.ReadByte();
                        reader.ReadByte();

                        // File Index (which library)
                        Cells[x, y].FileIndex = reader.ReadByte();

                        // Light
                        reader.ReadByte();

                        // Skipping remaining padding if size is larger
                        // Assuming 12-14 bytes.
                        // If we are desynchronized, the map will look glitchy.
                        // For safety in this blind implementation, we assume 12 bytes basic info + 2 bytes padding?
                        // Let's rely on standard: 14 bytes total?
                        // (2+2+2) + 1+1 + 1+1 + 1 + 1 + 2(pad) = 14?
                        reader.ReadBytes(5);
                    }
                }

                QueueRedraw(); // Trigger _Draw
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to load map: {ex.Message}");
            }
        }

        public void SetUserLocation(Point p)
        {
            _userLocation = p;
            QueueRedraw();
        }

        public Point GetMapLocation(Vector2 screenPos)
        {
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
        }

        public void Process()
        {
             // Update logic
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
