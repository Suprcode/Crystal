using Client.MirObjects;
using Client.MirScenes;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MirGraphics.Particles
{
    public class ParticleImageInfo
    {
        public MLibrary Library;
        public Size Size = Size.Empty;
        public int Count = 0;
        public TimeSpan DrawFrameMS = TimeSpan.FromMilliseconds(50);

        public int BaseIndex,  Duration;
        public long Start;

        public int CurrentFrame;
        public long NextFrame;

        public long CurrentDelay;
        public long Delay;

        public ParticleImageInfo(MLibrary file, int index, int count = 1, int drawMS = 50)
        {
            BaseIndex = index;
            Library = file;
            Size = Library.GetSize(index);
            Count = count;
            DrawFrameMS = TimeSpan.FromMilliseconds(50);
          //  Delay = drawMS;

            Start = CMain.Time;
            NextFrame = Start + (Duration / Count) * (CurrentFrame + 1);
            Duration =  drawMS * count;
        }
    }

    public class Particle
    {
        public ParticleImageInfo ImageInfo { get; set; }
        public ParticleEngine Engine { get; set; }
        public BlendMode BlendMode = BlendMode.NORMAL;
        public Vector2 OldPosition = Vector2.Zero;
        public Vector2 Position
        {
            get
            { return _position; }
            set
            {
                if (_position == value) return;

                OldPosition = _position;
                _position = value;
                OnPositionChanged();
            }
        }
        private Vector2 _position { get; set; }
        public int DrawFrame;
        public Vector2 OldVelocity = Vector2.Zero;
        public Vector2 _velocity { get; set; }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set
            {
                if (OldVelocity == Vector2.Zero)
                {
                    OldVelocity = value;
                }
                _velocity = value;
            }
        }
        public Color Color { get; set; }
        public float Size { get; set; }
        public DateTime AliveTime { get; set; }
        public bool Blend { get; set; }
        public float BlendRate { get; set; }
        public TimeSpan UpdateDelay = TimeSpan.FromMilliseconds(50);
        protected DateTime NextUpdateTime { get; set; }
        protected DateTime NextDraw { get; set; }

        public virtual void Update()
        {
            if (CMain.Now < NextUpdateTime) return;
            NextUpdateTime = CMain.Now + UpdateDelay;
            Position += Velocity;
        }


        public void Draw()
        {
            if (ImageInfo == null)
            {
                return;
            }

            int drawx = (int)Position.X;
            int drawy = (int)Position.Y;

            if (Blend)
                ImageInfo.Library.DrawBlend(ImageInfo.BaseIndex + ImageInfo.CurrentFrame, new Point(drawx, drawy), Color, true, BlendRate);
            else

            ImageInfo.Library.Draw(ImageInfo.BaseIndex + ImageInfo.CurrentFrame, new Point(drawx, drawy), Color, true, BlendRate);
        }
        public void ProcessImage()
        {
            if (CMain.Time <= ImageInfo.NextFrame) return;

            if (++ImageInfo.CurrentFrame >= ImageInfo.Count)
            {
               
                ImageInfo.CurrentFrame = 0;

                ImageInfo.Start = CMain.Time + ImageInfo.Delay;
                ImageInfo.NextFrame = ImageInfo.Start + (ImageInfo.Duration / ImageInfo.Count) * (ImageInfo.CurrentFrame + 1);
            }
            else ImageInfo.NextFrame = ImageInfo.Start + (ImageInfo.Duration / ImageInfo.Count) * (ImageInfo.CurrentFrame + 1);

            GameScene.Scene.MapControl.TextureValid = false;
        }
        protected virtual void OnPositionChanged()
        {
            try
            {
                if (ImageInfo.Size.Height == 0 || ImageInfo.Size.Width == 0)
                    return;
                
                int xwidth = (int)(ImageInfo.Size.Width * (Math.Ceiling(Settings.ScreenWidth / (decimal)ImageInfo.Size.Width) + 2));
                int ywidth = (int)(ImageInfo.Size.Height * (Math.Ceiling(Settings.ScreenHeight / (decimal)ImageInfo.Size.Height) + 2));
                Vector2 xreset = new Vector2(xwidth, 0);
                Vector2 yreset = new Vector2(0, ywidth);


                if (Position.Y < -ImageInfo.Size.Height * 2)
                    Position += yreset;
                else if (Position.Y > Settings.ScreenHeight + ImageInfo.Size.Height)
                    Position -= yreset;
                else if (Position.X < -ImageInfo.Size.Width * 2)
                    Position += xreset;
                else if (Position.X > Settings.ScreenWidth + ImageInfo.Size.Width)
                    Position -= xreset;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual void OnParticleEnd()
        {
        }
    }
}
