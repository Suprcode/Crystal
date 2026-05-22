using System;

namespace Client.Platform
{
    [Flags]
    public enum MirMouseButtons
    {
        None = 0,
        Left = 1048576,
        Right = 2097152,
        Middle = 4194304
    }

    public enum MirKeys
    {
        None = 0,
        LButton = 1,
        RButton = 2,
        Cancel = 3,
        MButton = 4,
        Back = 8,
        Tab = 9,
        Clear = 12,
        Enter = 13,
        Return = 13,
        ShiftKey = 16,
        ControlKey = 17,
        Menu = 18, // Alt
        Pause = 19,
        Capital = 20, // Caps Lock
        CapsLock = 20,
        Escape = 27,
        Space = 32,
        Prior = 33, // Page Up
        PageUp = 33,
        Next = 34,  // Page Down
        PageDown = 34,
        End = 35,
        Home = 36,
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        Select = 41,
        Print = 42,
        Execute = 43,
        Snapshot = 44, // Print Screen
        PrintScreen = 44,
        Insert = 45,
        Delete = 46,
        Help = 47,
        D0 = 48,
        D1 = 49,
        D2 = 50,
        D3 = 51,
        D4 = 52,
        D5 = 53,
        D6 = 54,
        D7 = 55,
        D8 = 56,
        D9 = 57,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LWin = 91,
        RWin = 92,
        Apps = 93,
        NumPad0 = 96,
        NumPad1 = 97,
        NumPad2 = 98,
        NumPad3 = 99,
        NumPad4 = 100,
        NumPad5 = 101,
        NumPad6 = 102,
        NumPad7 = 103,
        NumPad8 = 104,
        NumPad9 = 105,
        Multiply = 106,
        Add = 107,
        Separator = 108,
        Subtract = 109,
        Decimal = 110,
        Divide = 111,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        NumLock = 144,
        Scroll = 145,
        LShiftKey = 160,
        RShiftKey = 161,
        LControlKey = 162,
        RControlKey = 163,
        LMenu = 164,
        RMenu = 165,
        OemSemicolon = 186,
        Oem1 = 186,
        OemPlus = 187,
        OemComma = 188,
        OemMinus = 189,
        OemPeriod = 190,
        OemQuestion = 191,
        Oem2 = 191,
        Oemtilde = 192,
        Oem3 = 192,
        OemOpenBrackets = 219,
        Oem4 = 219,
        OemPipe = 220,
        Oem5 = 220,
        OemCloseBrackets = 221,
        Oem6 = 221,
        OemQuotes = 222,
        Oem7 = 222,
        Oem8 = 223,
        // Modifier flags
        Shift = 65536,
        Control = 131072,
        Alt = 262144
    }

    public class MirMouseEventArgs : EventArgs
    {
        public MirMouseButtons Button { get; }
        public int Clicks { get; }
        public int X { get; }
        public int Y { get; }
        public int Delta { get; }
        public System.Drawing.Point Location => new System.Drawing.Point(X, Y);

        public MirMouseEventArgs(MirMouseButtons button, int clicks, int x, int y, int delta)
        {
            Button = button;
            Clicks = clicks;
            X = x;
            Y = y;
            Delta = delta;
        }
    }

    public class MirKeyEventArgs : EventArgs
    {
        public MirKeys KeyCode { get; }
        public bool Handled { get; set; }
        public MirKeys Modifiers { get; }
        
        public bool Shift => (Modifiers & MirKeys.Shift) == MirKeys.Shift;
        public bool Control => (Modifiers & MirKeys.Control) == MirKeys.Control;
        public bool Alt => (Modifiers & MirKeys.Alt) == MirKeys.Alt;

        public MirKeyEventArgs(MirKeys keyCode, MirKeys modifiers = MirKeys.None)
        {
            KeyCode = keyCode;
            Modifiers = modifiers;
        }
    }

    public class MirKeyPressEventArgs : EventArgs
    {
        public char KeyChar { get; set; }
        public bool Handled { get; set; }

        public MirKeyPressEventArgs(char keyChar)
        {
            KeyChar = keyChar;
        }
    }

    public delegate void MirMouseEventHandler(object sender, MirMouseEventArgs e);
    public delegate void MirKeyEventHandler(object sender, MirKeyEventArgs e);
    public delegate void MirKeyPressEventHandler(object sender, MirKeyPressEventArgs e);
}

#if WINDOWS
namespace Client.Platform
{
    public static class InputTranslation
    {
        public static MirKeys ToNeutral(this System.Windows.Forms.Keys keys)
        {
            return (MirKeys)(int)keys;
        }

        public static MirMouseButtons ToNeutral(this System.Windows.Forms.MouseButtons button)
        {
            return (MirMouseButtons)(int)button;
        }

        public static MirKeyEventArgs ToNeutral(this System.Windows.Forms.KeyEventArgs e)
        {
            var modifiers = MirKeys.None;
            if (e.Shift) modifiers |= MirKeys.Shift;
            if (e.Control) modifiers |= MirKeys.Control;
            if (e.Alt) modifiers |= MirKeys.Alt;
            return new MirKeyEventArgs(e.KeyCode.ToNeutral(), modifiers) { Handled = e.Handled };
        }

        public static MirMouseEventArgs ToNeutral(this System.Windows.Forms.MouseEventArgs e)
        {
            return new MirMouseEventArgs(e.Button.ToNeutral(), e.Clicks, e.X, e.Y, e.Delta);
        }

        public static MirKeyPressEventArgs ToNeutral(this System.Windows.Forms.KeyPressEventArgs e)
        {
            return new MirKeyPressEventArgs(e.KeyChar) { Handled = e.Handled };
        }
    }
}
#else
namespace System.Drawing
{
    public enum FontStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Strikeout = 8,
    }

    public enum GraphicsUnit
    {
        World = 0,
        Display = 1,
        Pixel = 2,
        Point = 3,
        Inch = 4,
        Document = 5,
        Millimeter = 6,
    }

    public class Font : IDisposable
    {
        public string Name { get; }
        public float Size { get; }
        public FontStyle Style { get; }
        public GraphicsUnit Unit { get; }

        public Font(string name, float size)
        {
            Name = name;
            Size = size;
            Unit = GraphicsUnit.Point;
        }

        public Font(string name, float size, FontStyle style)
        {
            Name = name;
            Size = size;
            Style = style;
            Unit = GraphicsUnit.Point;
        }

        public Font(string name, float size, FontStyle style, GraphicsUnit unit)
        {
            Name = name;
            Size = size;
            Style = style;
            Unit = unit;
        }

        public int Height => (int)Math.Ceiling(GetHeight(96f));

        public float GetHeight(float dpi)
        {
            return Size * (dpi / 72.0f);
        }

        public void Dispose() { }
    }

    public class Bitmap : IDisposable
    {
        public int Width { get; }
        public int Height { get; }

        public Bitmap(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Bitmap(int width, int height, Imaging.PixelFormat format)
        {
            Width = width;
            Height = height;
        }

        public Bitmap(string path)
        {
            Width = 1;
            Height = 1;
        }

        public Bitmap(System.IO.Stream stream)
        {
            Width = 1;
            Height = 1;
        }

        public Imaging.BitmapData LockBits(Rectangle rect, Imaging.ImageLockMode flags, Imaging.PixelFormat format)
        {
            return new Imaging.BitmapData();
        }

        public void UnlockBits(Imaging.BitmapData data) { }

        public void Dispose() { }
    }

    public class Graphics : IDisposable
    {
        public static Graphics FromImage(Bitmap bitmap) => new Graphics();

        public float DpiX => 96f;
        public float DpiY => 96f;

        public SizeF MeasureString(string text, Font font)
        {
            if (string.IsNullOrEmpty(text)) return SizeF.Empty;
            var vec = Client.Platform.FNA.FNAFontManager.GetFont(font.Size).MeasureString(text);
            return new SizeF(vec.X, vec.Y);
        }

        public SizeF MeasureString(string text, Font font, int width)
        {
            if (string.IsNullOrEmpty(text)) return SizeF.Empty;
            var vec = Client.Platform.FNA.FNAFontManager.GetFont(font.Size).MeasureString(text);
            if (vec.X <= width)
            {
                return new SizeF(vec.X, vec.Y);
            }
            int lines = (int)Math.Ceiling(vec.X / width);
            if (lines < 1) lines = 1;
            return new SizeF(width, vec.Y * lines);
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea)
        {
            return MeasureString(text, font, (int)layoutArea.Width);
        }

        public void DrawString(string text, Font font, Brush brush, RectangleF layoutRectangle) { }

        public void Dispose() { }
    }

    public abstract class Brush : IDisposable
    {
        public void Dispose() { }
    }

    public class SolidBrush : Brush
    {
        public Color Color { get; }
        public SolidBrush(Color color) => Color = color;
    }
}

namespace System.Drawing.Imaging
{
    public enum PixelFormat
    {
        Format32bppArgb,
    }

    public enum ImageLockMode
    {
        ReadOnly,
        WriteOnly,
        ReadWrite,
    }

    public class BitmapData
    {
        public IntPtr Scan0 => IntPtr.Zero;
    }
}

namespace System.Windows.Forms
{
    [Flags]
    public enum TextFormatFlags
    {
        Default = 0,
        WordBreak = 16,
        TextBoxControl = 512,
        HorizontalCenter = 1,
        VerticalCenter = 4,
        SingleLine = 32,
        NoPadding = 268435456,
        NoPrefix = 2048,
        ExpandTabs = 64,
        Left = 0,
        Right = 2,
        Top = 0,
        Bottom = 8,
        RightToLeft = 131072,
    }

    public static class SystemInformation
    {
        public static int MouseWheelScrollDelta => 120;
    }

    public static class TextRenderer
    {
        public static System.Drawing.Size MeasureText(System.Drawing.Graphics graphics, string text, System.Drawing.Font font)
        {
            var size = graphics.MeasureString(text, font);
            return new System.Drawing.Size((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
        }

        public static System.Drawing.Size MeasureText(System.Drawing.Graphics graphics, string text, System.Drawing.Font font, System.Drawing.Size proposedSize, TextFormatFlags flags)
        {
            var size = graphics.MeasureString(text, font, proposedSize.Width);
            return new System.Drawing.Size((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
        }

        public static void DrawText(System.Drawing.Graphics graphics, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color color, TextFormatFlags flags)
        {
            using (var brush = new System.Drawing.SolidBrush(color))
            {
                graphics.DrawString(text, font, brush, bounds);
            }
        }
    }
}

namespace SlimDX
{
    public class SlimDXStub { }
}
namespace SlimDX.Direct3D9
{
    public class D3D9Stub { }
    public enum RenderState
    {
        SourceBlend,
        DestinationBlend
    }
    public enum Blend
    {
        SourceAlpha,
        InverseSourceAlpha
    }
    [Flags]
    public enum SpriteFlags
    {
        None = 0,
        AlphaBlend = 1
    }
}

namespace Client
{
    using System.Drawing;
    using System.Windows.Forms;
    using Client.MirControls;
    using Client.MirGraphics;
    using Client.MirObjects;
    using Client.MirScenes;
    using Client.Platform.FNA;
    using Shared;
    using System.Linq;
    using System.IO;

    public static class CMain
    {
        public static long Time;
        public static bool Shift;
        public static bool Ctrl;
        public static bool Alt;
        public static System.Drawing.Point MPoint;
        public static bool Tilde;
        public static MouseCursor CurrentCursor = MouseCursor.None;
        public static void SetMouseCursor(MouseCursor cursor) { CurrentCursor = cursor; }

        public static bool IsKeyLocked(Client.Platform.MirKeys key)
        {
            if (key == Client.Platform.MirKeys.Capital)
            {
                try
                {
                    return Console.CapsLock;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public static void CreateScreenShot()
        {
            if (FNAEntry.Instance == null || FNAEntry.Instance.GraphicsDevice == null) return;

            try
            {
                var device = FNAEntry.Instance.GraphicsDevice;
                int width = device.PresentationParameters.BackBufferWidth;
                int height = device.PresentationParameters.BackBufferHeight;

                Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[width * height];
                device.GetBackBufferData(colors);

                using (var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(width, height))
                {
                    image.ProcessPixelRows(accessor =>
                    {
                        for (int y = 0; y < height; y++)
                        {
                            var row = accessor.GetRowSpan(y);
                            for (int x = 0; x < width; x++)
                            {
                                var c = colors[y * width + x];
                                row[x] = new SixLabors.ImageSharp.PixelFormats.Rgba32(c.R, c.G, c.B, c.A);
                            }
                        }
                    });

                    string path = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    int count = Directory.GetFiles(path, "*.png").Length;
                    string fileName = Path.Combine(path, $"Image {count}.png");
                    SixLabors.ImageSharp.ImageExtensions.SaveAsPng(image, fileName);
                }
            }
            catch (Exception ex)
            {
                SaveError($"Screenshot Error: {ex.Message}");
            }
        }

        public static DateTime Now => DateTime.Now;
        public static Random Random = new Random();
        public static bool SpellTargetLock;
        public static long PingTime;
        public static string DebugText = string.Empty;
        
        public static Client.MirControls.MirControl DebugBaseLabel, HintBaseLabel;
        public static Client.MirControls.MirLabel DebugTextLabel, HintTextLabel;
        
        public static int FPS;
        public static int DPS;
        public static int DPSCounter;
        private static long _fpsTime;
        private static int _fps;

        public static uint BytesReceived;
        public static uint BytesSent;
        public static long NextPing;

        public static KeyBindSettings InputKeys = new KeyBindSettings();

        public static void CMain_KeyUp(object sender, Client.Platform.MirKeyEventArgs e) { }
        public static void CMain_KeyDown(object sender, Client.Platform.MirKeyEventArgs e) { }

        public static void SaveError(string ex)
        {
            try
            {
                System.IO.File.AppendAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "Error.txt"), $"{DateTime.Now}: {ex}{Environment.NewLine}");
            }
            catch { }
        }

        public static void SetResolution(int width, int height)
        {
            if (Settings.ScreenWidth == width && Settings.ScreenHeight == height) return;

            Settings.ScreenWidth = width;
            Settings.ScreenHeight = height;

            if (FNAEntry.Instance != null)
            {
                FNAEntry.Instance.Graphics.PreferredBackBufferWidth = (int)(width * Settings.WindowScale);
                FNAEntry.Instance.Graphics.PreferredBackBufferHeight = (int)(height * Settings.WindowScale);
                FNAEntry.Instance.Graphics.ApplyChanges();

                if (FNAEntry.Instance.Renderer != null)
                {
                    FNAEntry.Instance.Renderer.Initialize(width, height, Settings.FullScreen);
                    FNAEntry.Instance.Renderer.SetViewport(0, 0, width, height);
                }
            }
        }

        private static System.Drawing.Bitmap _dummyBmp = new System.Drawing.Bitmap(1, 1);
        public static System.Drawing.Graphics Graphics = System.Drawing.Graphics.FromImage(_dummyBmp);

        public static void UpdateFrameTime()
        {
            if (Time >= _fpsTime)
            {
                _fpsTime = Time + 1000;
                FPS = _fps;
                _fps = 0;

                DPS = DPSCounter;
                DPSCounter = 0;
            }
            else
                _fps++;
        }

        public static void CreateHintLabel()
        {
            if (HintBaseLabel == null || HintBaseLabel.IsDisposed)
            {
                HintBaseLabel = new MirControl
                {
                    BackColour = Color.FromArgb(255, 0, 0, 0),
                    Border = true,
                    DrawControlTexture = true,
                    BorderColour = Color.FromArgb(255, 144, 144, 0),
                    ForeColour = Color.Yellow,
                    Parent = MirScene.ActiveScene,
                    NotControl = true,
                    Opacity = 0.5F
                };
            }

            if (HintTextLabel == null || HintTextLabel.IsDisposed)
            {
                HintTextLabel = new MirLabel
                {
                    AutoSize = true,
                    BackColour = Color.Transparent,
                    ForeColour = Color.Yellow,
                    Parent = HintBaseLabel,
                    NotControl = true,
                };

                HintTextLabel.SizeChanged += (o, e) => HintBaseLabel.Size = HintTextLabel.Size;
            }

            if (MirControl.MouseControl == null || string.IsNullOrEmpty(MirControl.MouseControl.Hint))
            {
                HintBaseLabel.Visible = false;
                return;
            }

            HintBaseLabel.Visible = true;

            HintTextLabel.Text = MirControl.MouseControl.Hint;

            Point point = MPoint.Add(-HintTextLabel.Size.Width, 20);

            if (point.X + HintBaseLabel.Size.Width >= Settings.ScreenWidth)
                point.X = Settings.ScreenWidth - HintBaseLabel.Size.Width - 1;
            if (point.Y + HintBaseLabel.Size.Height >= Settings.ScreenHeight)
                point.Y = Settings.ScreenHeight - HintBaseLabel.Size.Height - 1;

            if (point.X < 0)
                point.X = 0;
            if (point.Y < 0)
                point.Y = 0;

            HintBaseLabel.Location = point;
        }

        public static void CreateDebugLabel()
        {
            string text;

            if (MirControl.MouseControl != null)
            {
                text = string.Format("FPS: {0}", FPS);

                text += string.Format(", DPS: {0}", DPS);

                text += string.Format(", Time: {0:HH:mm:ss UTC}", Now);

                if (MirControl.MouseControl is MapControl)
                    text += string.Format(", Co Ords: {0}", MapControl.MapLocation);

                if (!(MirControl.MouseControl is MapControl))
                    text += string.Format(", Control: {0}", MirControl.MouseControl.GetType().Name);

                if (MirScene.ActiveScene is GameScene)
                    text += string.Format(", Objects: {0}", MapControl.Objects.Count);

                if (MirScene.ActiveScene is GameScene && !string.IsNullOrEmpty(DebugText))
                    text += string.Format(", Debug: {0}", DebugText);

                if (MirObjects.MapObject.MouseObject != null)
                {
                    text += string.Format(", Target: {0}", MirObjects.MapObject.MouseObject.Name);
                }
                else
                {
                    text += string.Format(", Target: none");
                }
            }
            else
            {
                text = string.Format("FPS: {0}", FPS);
            }

            text += string.Format(", Ping: {0}", PingTime);

            text += string.Format(", Sent: {0}, Received: {1}", Functions.ConvertByteSize(BytesSent), Functions.ConvertByteSize(BytesReceived));

            text += string.Format(", TLC: {0}", DXManager.TextureList.Count(x => x.TextureValid));
            text += string.Format(", CLC: {0}", DXManager.ControlList.Count(x => x.IsDisposed == false));

            if (Settings.FullScreen)
            {
                if (DebugBaseLabel == null || DebugBaseLabel.IsDisposed)
                {
                    DebugBaseLabel = new MirControl
                    {
                        BackColour = Color.FromArgb(50, 50, 50),
                        Border = true,
                        BorderColour = Color.Black,
                        DrawControlTexture = true,
                        Location = new Point(5, 5),
                        NotControl = true,
                        Opacity = 0.5F,
                        Parent = MirScene.ActiveScene,
                    };
                }

                if (DebugTextLabel == null || DebugTextLabel.IsDisposed)
                {
                    DebugTextLabel = new MirLabel
                    {
                        AutoSize = true,
                        BackColour = Color.Transparent,
                        ForeColour = Color.White,
                        Parent = DebugBaseLabel,
                    };

                    DebugTextLabel.SizeChanged += (o, e) => DebugBaseLabel.Size = DebugTextLabel.Size;
                }

                DebugTextLabel.Text = text;
            }
            else
            {
                if (DebugBaseLabel != null && DebugBaseLabel.IsDisposed == false)
                {
                    DebugBaseLabel.Dispose();
                    DebugBaseLabel = null;
                }
                if (DebugTextLabel != null && DebugTextLabel.IsDisposed == false)
                {
                    DebugTextLabel.Dispose();
                    DebugTextLabel = null;
                }

                if (FNAEntry.Instance != null)
                {
                    FNAEntry.Instance.Window.Title = $"{GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GameName)} - {text}";
                }
            }
        }
    }
}
#endif

