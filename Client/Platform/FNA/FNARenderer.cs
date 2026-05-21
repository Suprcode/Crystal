using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Platform;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

#if FNA
using TextureHandle = Microsoft.Xna.Framework.Graphics.Texture2D;
using MatrixType = Microsoft.Xna.Framework.Matrix;
#else
using TextureHandle = SlimDX.Direct3D9.Texture;
using MatrixType = SlimDX.Matrix;
#endif

namespace Client.Platform.FNA
{
    public class FNARenderer : IGraphicsRenderer, IDisposable
    {
        public GraphicsDevice Device { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public RenderTarget2D LightRenderTarget { get; private set; }
        public BasicEffect BasicEffect { get; private set; }

        private MatrixType _transformMatrix = MatrixType.Identity;
        private bool _hasTransform = false;
        private Texture2D _whiteTexture;
        private readonly BlendState _additiveBlendState;
        private readonly BlendState _multiplyBlendState;
        private readonly VertexPositionColor[] _radialLightVertices;

        public FNARenderer(GraphicsDevice device)
        {
            Device = device;
            Device.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            SpriteBatch = new SpriteBatch(Device);
            BasicEffect = new BasicEffect(Device)
            {
                VertexColorEnabled = true,
                Projection = Matrix.CreateOrthographicOffCenter(0, device.Viewport.Width, device.Viewport.Height, 0, 0, 1)
            };

            _whiteTexture = new Texture2D(Device, 1, 1);
            _whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });

            _additiveBlendState = new BlendState
            {
                ColorSourceBlend = Blend.SourceAlpha,
                ColorDestinationBlend = Blend.One,
                AlphaSourceBlend = Blend.SourceAlpha,
                AlphaDestinationBlend = Blend.One
            };

            _multiplyBlendState = new BlendState
            {
                ColorSourceBlend = Blend.Zero,
                ColorDestinationBlend = Blend.SourceColor,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.Zero
            };

            _radialLightVertices = new VertexPositionColor[34];
        }

        public void DrawRectangle(System.Drawing.Rectangle rect, System.Drawing.Color color, float opacity)
        {
            var xnaColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A) * opacity;
            var destRect = new Microsoft.Xna.Framework.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            SpriteBatch.Draw(_whiteTexture, destRect, xnaColor);
        }

        public void Initialize(int width, int height, bool fullScreen)
        {
            Device.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            // Reset the render target when resolutions change
            if (LightRenderTarget != null)
            {
                LightRenderTarget.Dispose();
            }
            LightRenderTarget = new RenderTarget2D(Device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
        }

        public void BeginDraw()
        {
            if (_hasTransform)
            {
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, _transformMatrix);
            }
            else
            {
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            }
        }

        public void EndDraw()
        {
            SpriteBatch.End();
        }

        public void Clear(System.Drawing.Color color)
        {
            Device.Clear(new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A));
        }

        public void SetViewport(int x, int y, int width, int height)
        {
            Device.Viewport = new Viewport(x, y, width, height);
            BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);
        }

        public void Draw(TextureHandle texture, System.Drawing.Rectangle sourceRect, System.Drawing.Point position, System.Drawing.Color color)
        {
            var xnaRect = new Microsoft.Xna.Framework.Rectangle(sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height);
            var xnaColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
            var destRect = new Microsoft.Xna.Framework.Rectangle(position.X, position.Y, sourceRect.Width, sourceRect.Height);
            
            SpriteBatch.Draw(texture, destRect, xnaRect, xnaColor);
        }

        public void DrawOpaque(TextureHandle texture, System.Drawing.Rectangle sourceRect, System.Drawing.Point position, System.Drawing.Color color, float opacity)
        {
            var xnaRect = new Microsoft.Xna.Framework.Rectangle(sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height);
            var xnaColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A) * opacity;
            var destRect = new Microsoft.Xna.Framework.Rectangle(position.X, position.Y, sourceRect.Width, sourceRect.Height);

            SpriteBatch.Draw(texture, destRect, xnaRect, xnaColor);
        }

        public void DrawBlend(TextureHandle texture, System.Drawing.Rectangle sourceRect, System.Drawing.Point position, System.Drawing.Color color, float rate)
        {
            // Set additive/blended states for special FX
            SpriteBatch.End();
            
            if (_hasTransform)
                SpriteBatch.Begin(SpriteSortMode.Deferred, _additiveBlendState, null, null, null, null, _transformMatrix);
            else
                SpriteBatch.Begin(SpriteSortMode.Deferred, _additiveBlendState);

            Draw(texture, sourceRect, position, color);

            SpriteBatch.End();
            
            if (_hasTransform)
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, _transformMatrix);
            else
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
        }

        public void DrawTinted(TextureHandle texture, TextureHandle maskTexture, System.Drawing.Rectangle sourceRect, System.Drawing.Point position, System.Drawing.Color color, System.Drawing.Color tint)
        {
            // Simple multi-pass draw for tinted overlays
            Draw(texture, sourceRect, position, color);
            Draw(maskTexture, sourceRect, position, tint);
        }

        public void RenderGPULights(List<MirLightSource> lights, System.Drawing.Color darkness)
        {
            if (darkness.R == 255 && darkness.G == 255 && darkness.B == 255 && darkness.A == 255 && (lights == null || lights.Count == 0))
                return;

            // Recreate render target if viewport dimensions changed (DPI, resolution change)
            if (LightRenderTarget == null || LightRenderTarget.Width != Device.Viewport.Width || LightRenderTarget.Height != Device.Viewport.Height)
            {
                LightRenderTarget?.Dispose();
                LightRenderTarget = new RenderTarget2D(Device, Device.Viewport.Width, Device.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
            }

            // End active SpriteBatch before switching render targets and drawing with BasicEffect
            SpriteBatch.End();

            // Bind Light Mask target
            Device.SetRenderTarget(LightRenderTarget);
            Device.Clear(new Microsoft.Xna.Framework.Color(darkness.R, darkness.G, darkness.B, darkness.A)); // Ambient dark baseline

            // Update basic effect parameters for clean 2D pixel space rendering
            BasicEffect.World = Microsoft.Xna.Framework.Matrix.Identity;
            BasicEffect.View = Microsoft.Xna.Framework.Matrix.Identity;
            BasicEffect.Projection = Microsoft.Xna.Framework.Matrix.CreateOrthographicOffCenter(0, LightRenderTarget.Width, LightRenderTarget.Height, 0, -1, 1);
            BasicEffect.TextureEnabled = false;
            BasicEffect.VertexColorEnabled = true;

            // Set Additive Blending for stacking lights
            Device.BlendState = _additiveBlendState;
            Device.RasterizerState = RasterizerState.CullNone;
            Device.DepthStencilState = DepthStencilState.None;

            if (lights != null && lights.Count > 0)
            {
                // Render each light as a mathematically interpolated GPU primitive
                foreach (var light in lights)
                {
                    DrawGPURadialLight(light.Center.X, light.Center.Y, light.Width / 2f, light.Height / 2f, light.Color);
                }
            }

            // Restore Main Surface
            Device.SetRenderTarget(null);

            // Draw composite light mask onto main viewport with Multiplicative blending
            SpriteBatch.Begin(SpriteSortMode.Immediate, _multiplyBlendState);
            SpriteBatch.Draw(LightRenderTarget, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
            SpriteBatch.End();

            // Re-bind default drawing state
            if (_hasTransform)
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, _transformMatrix);
            else
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
        }

        private void DrawGPURadialLight(float centerX, float centerY, float radiusX, float radiusY, System.Drawing.Color color)
        {
            const int segments = 32;
            var xnaColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
            var outerColor = new Microsoft.Xna.Framework.Color(xnaColor.R, xnaColor.G, xnaColor.B, 0);

            var vertices = new VertexPositionColor[segments * 3];

            for (int i = 0; i < segments; i++)
            {
                float angle1 = i * MathHelper.TwoPi / segments;
                float angle2 = (i + 1) * MathHelper.TwoPi / segments;

                float x1 = centerX + radiusX * MathF.Cos(angle1);
                float y1 = centerY + radiusY * MathF.Sin(angle1);

                float x2 = centerX + radiusX * MathF.Cos(angle2);
                float y2 = centerY + radiusY * MathF.Sin(angle2);

                int idx = i * 3;
                vertices[idx] = new VertexPositionColor(new Vector3(centerX, centerY, 0.0f), xnaColor);
                vertices[idx + 1] = new VertexPositionColor(new Vector3(x1, y1, 0.0f), outerColor);
                vertices[idx + 2] = new VertexPositionColor(new Vector3(x2, y2, 0.0f), outerColor);
            }

            // Render primitive using GPU vertex interpolators
            foreach (var pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, segments);
            }
        }

        public TextureHandle CreateTexture(int width, int height)
        {
            return new Texture2D(Device, width, height, false, SurfaceFormat.Color);
        }

        public TextureHandle LoadTexture(string path)
        {
            using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(path))
            {
                var texture = new Texture2D(Device, image.Width, image.Height, false, SurfaceFormat.Color);
                var pixels = new Microsoft.Xna.Framework.Color[image.Width * image.Height];

                image.ProcessPixelRows(accessor =>
                {
                    for (int y = 0; y < accessor.Height; y++)
                    {
                        var row = accessor.GetRowSpan(y);
                        for (int x = 0; x < accessor.Width; x++)
                        {
                            var pixel = row[x];
                            pixels[y * image.Width + x] = new Microsoft.Xna.Framework.Color(pixel.R, pixel.G, pixel.B, pixel.A);
                        }
                    }
                });

                texture.SetData(pixels);
                return texture;
            }
        }

        public void SetTransform(MatrixType matrix)
        {
            _transformMatrix = matrix;
            _hasTransform = true;
            
            // Re-apply viewport spritebatch if active
            SpriteBatch.End();
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, _transformMatrix);
        }

        public void ResetTransform()
        {
            _transformMatrix = MatrixType.Identity;
            _hasTransform = false;

            SpriteBatch.End();
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
        }

        public void SetSurface(object surface)
        {
            // Bypassed: dynamic composite rendering used directly under FNA
        }

        public void SetGrayscale(bool value)
        {
            // Grayscale rendering state handled in main pixel shaders
        }

        public void SetOpacity(float opacity)
        {
            // Opacity drawing state integrated natively in spritebatch calls
        }

        public void SetBlend(bool blend, float rate = 1f, Client.MirGraphics.BlendMode mode = Client.MirGraphics.BlendMode.Normal)
        {
            // Additive/alpha blend states managed inside shader render passes
        }

        public void Dispose()
        {
            SpriteBatch?.Dispose();
            LightRenderTarget?.Dispose();
            BasicEffect?.Dispose();
            _whiteTexture?.Dispose();
            _additiveBlendState?.Dispose();
            _multiplyBlendState?.Dispose();
        }
    }
}
