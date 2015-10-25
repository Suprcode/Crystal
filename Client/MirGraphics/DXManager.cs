using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Client.MirControls;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Blend = Microsoft.DirectX.Direct3D.Blend;

namespace Client.MirGraphics
{


    class DXManager
    {
        public static List<MImage> TextureList = new List<MImage>();
        public static List<MirControl> ControlList = new List<MirControl>();


        public static Device Device;
        public static Sprite Sprite, TextSprite;
        public static Line Line;

        public static Surface CurrentSurface;
        public static Surface MainSurface;
        public static PresentParameters Parameters;
        public static bool DeviceLost;
        public static float Opacity = 1F;
        public static bool Blending;


        public static Texture RadarTexture;
        public static List<Texture> Lights = new List<Texture>();
        public static Texture PoisonDotBackground;

        public static Point[] LightSizes =
        {
            new Point(125,95),
            new Point(205,156),
            new Point(285,217),
            new Point(365,277),
            new Point(445,338),
            new Point(525,399),
            new Point(605,460),
            new Point(685,521),
            new Point(765,581),
            new Point(845,642),
            new Point(925,703)
        };

        public static void Create()
        {
            Parameters = new PresentParameters
            {
                BackBufferFormat = Format.X8R8G8B8,
                PresentFlag = PresentFlag.LockableBackBuffer,
                BackBufferWidth = Settings.ScreenWidth,
                BackBufferHeight = Settings.ScreenHeight,
                SwapEffect = SwapEffect.Discard,
                PresentationInterval = Settings.FPSCap ? PresentInterval.One : PresentInterval.Immediate,
                Windowed = !Settings.FullScreen,
            };


            Caps devCaps = Manager.GetDeviceCaps(0, DeviceType.Hardware);
            DeviceType devType = DeviceType.Reference;
            CreateFlags devFlags = CreateFlags.HardwareVertexProcessing;

            if (devCaps.VertexShaderVersion.Major >= 2 && devCaps.PixelShaderVersion.Major >= 2)
                devType = DeviceType.Hardware;

            if (devCaps.DeviceCaps.SupportsHardwareTransformAndLight)
                devFlags = CreateFlags.HardwareVertexProcessing;


            if (devCaps.DeviceCaps.SupportsPureDevice)
                devFlags |= CreateFlags.PureDevice;


            Device = new Device(Manager.Adapters.Default.Adapter, devType, Program.Form, devFlags, Parameters);

            Device.DeviceLost += (o, e) => DeviceLost = true;
            Device.DeviceResizing += (o, e) => e.Cancel = true;
            Device.DeviceReset += (o, e) => LoadTextures();
            Device.Disposing += (o, e) => Clean();

            Device.SetDialogBoxesEnabled(true);

            LoadTextures();
        }

        private static unsafe void LoadTextures()
        {
            Sprite = new Sprite(Device);
            TextSprite = new Sprite(Device);
            Line = new Line(Device) { Width = 1F };

            MainSurface = Device.GetBackBuffer(0, 0, BackBufferType.Mono);
            CurrentSurface = MainSurface;
            Device.SetRenderTarget(0, MainSurface);


            if (RadarTexture == null || RadarTexture.Disposed)
            {
                RadarTexture = new Texture(Device, 2, 2, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);

                using (GraphicsStream stream = RadarTexture.LockRectangle(0, LockFlags.Discard))
                using (Bitmap image = new Bitmap(2, 2, 8, PixelFormat.Format32bppArgb, (IntPtr)stream.InternalDataPointer))
                using (Graphics graphics = Graphics.FromImage(image))
                    graphics.Clear(Color.White);
            }
            if (PoisonDotBackground == null || PoisonDotBackground.Disposed)
            {
                PoisonDotBackground = new Texture(Device, 5, 5, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);

                using (GraphicsStream stream = PoisonDotBackground.LockRectangle(0, LockFlags.Discard))
                using (Bitmap image = new Bitmap(5, 5, 20, PixelFormat.Format32bppArgb, (IntPtr)stream.InternalDataPointer))
                using (Graphics graphics = Graphics.FromImage(image))
                    graphics.Clear(Color.White);
            }
            CreateLights();
        }

        private unsafe static void CreateLights()
        {

            for (int i = Lights.Count - 1; i >= 0; i--)
                Lights[i].Dispose();

            Lights.Clear();

            for (int i = 1; i < LightSizes.Length; i++)
            {
                // int width = 125 + (57 *i);
                //int height = 110 + (57 * i);
                int width = LightSizes[i].X;
                int height = LightSizes[i].Y;
                Texture light = new Texture(Device, width, height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);

                using (GraphicsStream stream = light.LockRectangle(0, LockFlags.Discard))
                using (Bitmap image = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, (IntPtr)stream.InternalDataPointer))
                {
                    using (Graphics graphics = Graphics.FromImage(image))
                    {
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            //path.AddEllipse(new Rectangle(0, 0, width, height));
                            //using (PathGradientBrush brush = new PathGradientBrush(path))
                            //{
                            //    graphics.Clear(Color.FromArgb(0, 0, 0, 0));
                            //    brush.SurroundColors = new[] { Color.FromArgb(0, 255, 255, 255) };
                            //    brush.CenterColor = Color.FromArgb(255, 255, 255, 255);
                            //    graphics.FillPath(brush, path);
                            //    graphics.Save();
                            //}

                            path.AddEllipse(new Rectangle(0, 0, width, height));
                            using (PathGradientBrush brush = new PathGradientBrush(path))
                            {
                                Color[] blendColours = { Color.White,
                                                     Color.FromArgb(255,150,150,145),
                                                     Color.FromArgb(255,80,80,80),
                                                     Color.FromArgb(255,55,50,45),
                                                     Color.FromArgb(255,20,20,25),
                                                     Color.FromArgb(0,0,0,0)};

                                float[] radiusPositions = { 0f, .20f, .40f, .60f, .80f, 1.0f };

                                ColorBlend colourBlend = new ColorBlend();
                                colourBlend.Colors = blendColours;
                                colourBlend.Positions = radiusPositions;

                                graphics.Clear(Color.FromArgb(0, 0, 0, 0));
                                brush.InterpolationColors = colourBlend;
                                brush.SurroundColors = blendColours;
                                brush.CenterColor = Color.White;
                                graphics.FillPath(brush, path);
                                graphics.Save();
                            }
                        }
                    }
                }
                light.Disposing += (o, e) => Lights.Remove(light);
                Lights.Add(light);
            }
        }

        public static void SetSurface(Surface surface)
        {
            if (CurrentSurface == surface)
                return;

            Sprite.Flush();
            CurrentSurface = surface;
            Device.SetRenderTarget(0, surface);
        }
        public static void AttemptReset()
        {
            try
            {
                int result;
                Device.CheckCooperativeLevel(out result);
                switch ((ResultCode)result)
                {
                    case ResultCode.DeviceNotReset:
                        Device.Reset(Parameters);
                        break;
                    case ResultCode.DeviceLost:
                        break;
                    case ResultCode.Success:
                        DeviceLost = false;
                        CurrentSurface = Device.GetBackBuffer(0, 0, BackBufferType.Mono);
                        Device.SetRenderTarget(0, CurrentSurface);
                        break;
                }
            }
            catch
            {
            }
        }
        public static void AttemptRecovery()
        {
            try
            {
                Sprite.End();
            }
            catch
            {
            }

            try
            {
                Device.EndScene();
            }
            catch
            {
            }
            try
            {
                MainSurface = Device.GetBackBuffer(0, 0, BackBufferType.Mono);
                CurrentSurface = MainSurface;
                Device.SetRenderTarget(0, MainSurface);
            }
            catch
            {
            }
        }
        public static void SetOpacity(float opacity)
        {
            if (Opacity == opacity)
                return;

            Sprite.Flush();
            Device.RenderState.AlphaBlendEnable = true;
            if (opacity >= 1 || opacity < 0)
            {
                Device.RenderState.SourceBlend = Blend.SourceAlpha;
                Device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
                Device.RenderState.AlphaSourceBlend = Blend.One;
                Device.RenderState.BlendFactor = Color.FromArgb(255, 255, 255, 255);
            }
            else
            {
                Device.RenderState.SourceBlend = Blend.BlendFactor;
                Device.RenderState.DestinationBlend = Blend.InvBlendFactor;
                Device.RenderState.AlphaSourceBlend = Blend.SourceAlpha;
                Device.RenderState.BlendFactor = Color.FromArgb((byte)(255 * opacity), (byte)(255 * opacity),
                                                                (byte)(255 * opacity), (byte)(255 * opacity));
            }
            Opacity = opacity;
            Sprite.Flush();
        }
        public static void SetBlend(bool value, float rate = 1F)
        {
            if (value == Blending) return;
            Blending = value;
            Sprite.Flush();

            Sprite.End();
            if (Blending)
            {
                Sprite.Begin(SpriteFlags.DoNotSaveState);
                Device.RenderState.AlphaBlendEnable = true;
                Device.RenderState.SourceBlend = Blend.BlendFactor;
                Device.RenderState.DestinationBlend = Blend.One;
                Device.RenderState.BlendFactor = Color.FromArgb((byte)(255 * rate), (byte)(255 * rate),
                                                                (byte)(255 * rate), (byte)(255 * rate));
            }
            else
                Sprite.Begin(SpriteFlags.AlphaBlend);

            Device.SetRenderTarget(0, CurrentSurface);
        }

        public static void Clean()
        {
            for (int i = TextureList.Count - 1; i >= 0; i--)
            {
                MImage m = TextureList[i];

                if (m == null)
                {
                    TextureList.RemoveAt(i);
                    continue;
                }

                if (CMain.Time <= m.CleanTime) continue;


                TextureList.RemoveAt(i);
                if (m.Image != null && !m.Image.Disposed)
                    m.Image.Dispose();
            }

            for (int i = ControlList.Count - 1; i >= 0; i--)
            {
                MirControl c = ControlList[i];

                if (c == null)
                {
                    ControlList.RemoveAt(i);
                    continue;
                }

                if (CMain.Time <= c.CleanTime) continue;

                c.DisposeTexture();
            }
        }

        private static void CleanUp()
        {
            for (int i = TextureList.Count - 1; i >= 0; i--)
            {
                MImage m = TextureList[i];

                if (m == null) continue;

                if (m.Image != null && !m.Image.Disposed)
                    m.Image.Dispose();
            }
            TextureList.Clear();


            for (int i = ControlList.Count - 1; i >= 0; i--)
            {
                MirControl c = ControlList[i];

                if (c == null) continue;

                c.DisposeTexture();
            }
            ControlList.Clear();
        }
    }
}