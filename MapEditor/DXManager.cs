using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Map_Editor
{
  internal class DXManager
  {
    public static List<MLibrary.MImage> TextureList = new List<MLibrary.MImage>();
    public static Device Device;
    public static Sprite Sprite;
    public static Sprite TextSprite;
    public static Line Line;
    public static Surface CurrentSurface;
    public static Surface MainSurface;
    public static PresentParameters Parameters;
    public static bool DeviceLost;
    public static float Opacity = 1f;
    public static bool Blending;
    public static List<Texture> Lights = new List<Texture>();
    public static Point[] LightSizes = new Point[11]
    {
      new Point(125, 95),
      new Point(205, 156),
      new Point(285, 217),
      new Point(365, 277),
      new Point(445, 338),
      new Point(525, 399),
      new Point(605, 460),
      new Point(685, 521),
      new Point(765, 581),
      new Point(845, 642),
      new Point(925, 703)
    };
    private static Control _control;

    public static void Create(Control control)
    {
      DXManager._control = control;
      DXManager.Parameters = new PresentParameters()
      {
        BackBufferFormat = Microsoft.DirectX.Direct3D.Format.X8R8G8B8,
        PresentFlag = PresentFlag.LockableBackBuffer,
        BackBufferWidth = control.Width,
        BackBufferHeight = control.Height,
        SwapEffect = SwapEffect.Discard,
        PresentationInterval = PresentInterval.One,
        Windowed = true
      };
      Caps deviceCaps1 = Manager.GetDeviceCaps(0, DeviceType.Hardware);
      DeviceType deviceType = DeviceType.Reference;
      CreateFlags behaviorFlags = CreateFlags.HardwareVertexProcessing;
      if (deviceCaps1.VertexShaderVersion.Major >= 2 && deviceCaps1.PixelShaderVersion.Major >= 2)
        deviceType = DeviceType.Hardware;
      DeviceCaps deviceCaps2 = deviceCaps1.DeviceCaps;
      if (deviceCaps2.SupportsHardwareTransformAndLight)
        behaviorFlags = CreateFlags.HardwareVertexProcessing;
      deviceCaps2 = deviceCaps1.DeviceCaps;
      if (deviceCaps2.SupportsPureDevice)
        behaviorFlags |= CreateFlags.PureDevice;
      DXManager.Device = new Device(Manager.Adapters.Default.Adapter, deviceType, control, behaviorFlags, new PresentParameters[1]
      {
        DXManager.Parameters
      });
      DXManager.Device.DeviceLost += new EventHandler(DXManager.Device_DeviceLost);
      DXManager.Device.DeviceResizing += new CancelEventHandler(DXManager.Device_DeviceResizing);
      DXManager.Device.DeviceReset += new EventHandler(DXManager.Device_DeviceReset);
      DXManager.Device.Disposing += new EventHandler(DXManager.Device_Disposing);
      DXManager.Device.SetDialogBoxesEnabled(true);
      DXManager.LoadTextures();
    }

    private static void Device_Disposing(object sender, EventArgs e) => DXManager.Clean();

    private static void Device_DeviceReset(object sender, EventArgs e) => DXManager.LoadTextures();

    private static void Device_DeviceResizing(object sender, CancelEventArgs e)
    {
      if (DXManager._control.Size == new Size(0, 0))
        e.Cancel = true;
      else
        e.Cancel = false;
    }

    private static void Device_DeviceLost(object sender, EventArgs e) => DXManager.DeviceLost = true;

    private static void LoadTextures()
    {
      DXManager.Sprite = new Sprite(DXManager.Device);
      DXManager.TextSprite = new Sprite(DXManager.Device);
      DXManager.Line = new Line(DXManager.Device)
      {
        Width = 1f
      };
      DXManager.MainSurface = DXManager.Device.GetBackBuffer(0, 0, BackBufferType.Mono);
      DXManager.CurrentSurface = DXManager.MainSurface;
      DXManager.Device.SetRenderTarget(0, DXManager.MainSurface);
    }

    private static unsafe void CreateLights()
    {
      for (int index = DXManager.Lights.Count - 1; index >= 0; --index)
        DXManager.Lights[index].Dispose();
      DXManager.Lights.Clear();
      for (int index = 1; index < DXManager.LightSizes.Length; ++index)
      {
        int x = DXManager.LightSizes[index].X;
        int y = DXManager.LightSizes[index].Y;
        Texture light = new Texture(DXManager.Device, x, y, 1, Usage.None, Microsoft.DirectX.Direct3D.Format.A8R8G8B8, Pool.Managed);
        using (GraphicsStream graphicsStream = light.LockRectangle(0, LockFlags.Discard))
        {
          using (Bitmap bitmap = new Bitmap(x, y, x * 4, PixelFormat.Format32bppArgb, (IntPtr) graphicsStream.InternalDataPointer))
          {
            using (Graphics graphics = Graphics.FromImage((Image) bitmap))
            {
              using (GraphicsPath path = new GraphicsPath())
              {
                path.AddEllipse(new Rectangle(0, 0, x, y));
                using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path))
                {
                  graphics.Clear(Color.FromArgb(0, 0, 0, 0));
                  pathGradientBrush.SurroundColors = new Color[1]
                  {
                    Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)
                  };
                  pathGradientBrush.CenterColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
                  graphics.FillPath((Brush) pathGradientBrush, path);
                  graphics.Save();
                }
              }
            }
          }
        }
        DXManager.Lights.Add(light);
        light.Disposing += (EventHandler) ((o, e) => DXManager.Lights.Remove(light));
      }
    }

    public static void SetSurface(Surface surface)
    {
      if ((Resource) DXManager.CurrentSurface == (Resource) surface)
        return;
      DXManager.Sprite.Flush();
      DXManager.CurrentSurface = surface;
      DXManager.Device.SetRenderTarget(0, surface);
    }

    public static void AttemptReset()
    {
      try
      {
        int result;
        DXManager.Device.CheckCooperativeLevel(out result);
        switch ((ResultCode) result)
        {
          case ResultCode.DeviceNotReset:
            DXManager.Device.Reset(DXManager.Parameters);
            break;
          case ResultCode.Success:
            DXManager.DeviceLost = false;
            DXManager.MainSurface = DXManager.Device.GetBackBuffer(0, 0, BackBufferType.Mono);
            DXManager.CurrentSurface = DXManager.Device.GetBackBuffer(0, 0, BackBufferType.Mono);
            DXManager.Device.SetRenderTarget(0, DXManager.CurrentSurface);
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
        DXManager.Sprite.End();
        DXManager.TextSprite.End();
      }
      catch
      {
      }
      try
      {
        DXManager.Device.EndScene();
      }
      catch
      {
      }
      try
      {
        DXManager.MainSurface = DXManager.Device.GetBackBuffer(0, 0, BackBufferType.Mono);
        DXManager.CurrentSurface = DXManager.MainSurface;
        DXManager.Device.SetRenderTarget(0, DXManager.MainSurface);
      }
      catch
      {
      }
    }

    public static void SetOpacity(float opacity)
    {
      if ((double) DXManager.Opacity == (double) opacity)
        return;
      DXManager.Sprite.Flush();
      DXManager.Device.RenderState.AlphaBlendEnable = true;
      if ((double) opacity >= 1.0 || (double) opacity < 0.0)
      {
        DXManager.Device.RenderState.SourceBlend = Microsoft.DirectX.Direct3D.Blend.SourceAlpha;
        DXManager.Device.RenderState.DestinationBlend = Microsoft.DirectX.Direct3D.Blend.InvSourceAlpha;
        DXManager.Device.RenderState.AlphaSourceBlend = Microsoft.DirectX.Direct3D.Blend.One;
        DXManager.Device.RenderState.BlendFactor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      }
      else
      {
        DXManager.Device.RenderState.SourceBlend = Microsoft.DirectX.Direct3D.Blend.BlendFactor;
        DXManager.Device.RenderState.DestinationBlend = Microsoft.DirectX.Direct3D.Blend.InvBlendFactor;
        DXManager.Device.RenderState.AlphaSourceBlend = Microsoft.DirectX.Direct3D.Blend.SourceAlpha;
        DXManager.Device.RenderState.BlendFactor = Color.FromArgb((int) (byte) ((double) byte.MaxValue * (double) opacity), (int) (byte) ((double) byte.MaxValue * (double) opacity), (int) (byte) ((double) byte.MaxValue * (double) opacity), (int) (byte) ((double) byte.MaxValue * (double) opacity));
      }
      DXManager.Opacity = opacity;
      DXManager.Sprite.Flush();
    }

    public static void SetBlend(bool value, float rate = 1f)
    {
      if (value == DXManager.Blending)
        return;
      DXManager.Blending = value;
      DXManager.Sprite.Flush();
      DXManager.Sprite.End();
      if (DXManager.Blending)
      {
        DXManager.Sprite.Begin(SpriteFlags.DoNotSaveState);
        DXManager.Device.RenderState.AlphaBlendEnable = true;
        DXManager.Device.RenderState.SourceBlend = Microsoft.DirectX.Direct3D.Blend.BlendFactor;
        DXManager.Device.RenderState.DestinationBlend = Microsoft.DirectX.Direct3D.Blend.One;
        DXManager.Device.RenderState.BlendFactor = Color.FromArgb((int) (byte) ((double) byte.MaxValue * (double) rate), (int) (byte) ((double) byte.MaxValue * (double) rate), (int) (byte) ((double) byte.MaxValue * (double) rate), (int) (byte) ((double) byte.MaxValue * (double) rate));
      }
      else
        DXManager.Sprite.Begin(SpriteFlags.AlphaBlend);
      DXManager.Device.SetRenderTarget(0, DXManager.CurrentSurface);
    }

    public static void Clean()
    {
      for (int index = DXManager.TextureList.Count - 1; index >= 0; --index)
      {
        if (DXManager.TextureList[index] == null)
          DXManager.TextureList.RemoveAt(index);
        else
          DXManager.TextureList.RemoveAt(index);
      }
    }
  }
}
