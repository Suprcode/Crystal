using System;
using System.Drawing;
using System.Collections.Generic;

#if FNA
using TextureHandle = Microsoft.Xna.Framework.Graphics.Texture2D;
using MatrixType = Microsoft.Xna.Framework.Matrix;
#else
using TextureHandle = SlimDX.Direct3D9.Texture;
using MatrixType = SlimDX.Matrix;
#endif

namespace Client.Platform
{
    public interface IGraphicsRenderer
    {
        void Initialize(int width, int height, bool fullScreen);
        void BeginDraw();
        void EndDraw();
        void Clear(Color color);
        void SetViewport(int x, int y, int width, int height);

        void Draw(TextureHandle texture, Rectangle sourceRect, System.Drawing.Point position, Color color);
        void DrawOpaque(TextureHandle texture, Rectangle sourceRect, System.Drawing.Point position, Color color, float opacity);
        void DrawBlend(TextureHandle texture, Rectangle sourceRect, System.Drawing.Point position, Color color, float rate);
        void DrawTinted(TextureHandle texture, TextureHandle maskTexture, Rectangle sourceRect, System.Drawing.Point position, Color color, Color tint);
        void DrawRectangle(Rectangle rect, Color color, float opacity);

        // Dynamic GPU-driven radial lights rendering
        void RenderGPULights(List<MirLightSource> lights, Color darkness);

        TextureHandle CreateTexture(int width, int height);
        TextureHandle LoadTexture(string path);
        
        void SetTransform(MatrixType matrix);
        void ResetTransform();

        void SetSurface(object surface);
        void SetGrayscale(bool value);
        void SetOpacity(float opacity);
#if FNA
        void SetBlend(bool blend, float rate = 1f, Client.MirGraphics.BlendMode mode = Client.MirGraphics.BlendMode.Normal);
#else
        void SetBlend(bool blend, float rate = 1f, global::BlendMode mode = global::BlendMode.NORMAL);
#endif
    }

    public class MirLightSource
    {
        public System.Drawing.Point Center { get; set; }
        public int Radius { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color Color { get; set; }
    }
}
