using SlimDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MirGraphics.Particles
{
    public class FogParticle : Particle
    {
        private static int xwidth = (int)(512 * (Math.Ceiling(Settings.ScreenWidth / 512M) + 2));
        private static int ywidth = (int)(512 * (Math.Ceiling(Settings.ScreenHeight / 512M) + 2));
        private Vector2 xreset = new Vector2(xwidth, 0);
        private Vector2 yreset = new Vector2(0, ywidth);

        public FogParticle(ParticleEngine engine, ParticleImageInfo image)
        {
            Engine = engine;
            ImageInfo = image;
        }

        public override void Update()
        {
            if (CMain.Now < NextUpdateTime) return;

            NextUpdateTime = CMain.Now.AddMilliseconds(50);
            Position += Velocity;
        }
    }
    public class SandParticle : Particle
    {
        private static int xwidth = (int)(800 * (Math.Ceiling(Settings.ScreenWidth / 800M) + 2));
        private static int ywidth = (int)(600 * (Math.Ceiling(Settings.ScreenHeight / 600M) + 2));
        private Vector2 xreset = new Vector2(xwidth, 0);
        private Vector2 yreset = new Vector2(0, ywidth);

        public SandParticle(ParticleEngine engine, ParticleImageInfo image)
        {
            Engine = engine;
            ImageInfo = image;
        }

        public override void Update()
        {
            if (CMain.Now < NextUpdateTime) return;

            NextUpdateTime = CMain.Now.AddMilliseconds(50);
            Position += Velocity;
        }

        protected override void OnPositionChanged()
        {
            if (Position.Y < -ImageInfo.Size.Height * 2)
                Position += yreset;
            else if (Position.Y > Settings.ScreenHeight + ImageInfo.Size.Height)
                Position -= yreset;
            else if (Position.X < -ImageInfo.Size.Width * 2)
                Position += xreset;
            else if (Position.X > Settings.ScreenWidth + ImageInfo.Size.Width)
                Position -= xreset;
        }
    }
    public class SnowParticle : Particle
    {
        private static int xwidth = (int)(400 * (Math.Ceiling(Settings.ScreenWidth / 400M) + 2));
        private static int ywidth = (int)(400 * (Math.Ceiling(Settings.ScreenHeight / 400M) + 2));
        private Vector2 xreset = new Vector2(xwidth, 0);
        private Vector2 yreset = new Vector2(0, ywidth);

        public SnowParticle(ParticleEngine engine, ParticleImageInfo image)
        {
            Engine = engine;
            ImageInfo = image;
        }

        public override void Update()
        {
            if (CMain.Now < NextUpdateTime) return;

            NextUpdateTime = CMain.Now.AddMilliseconds(50);
            Position += Velocity;
        }

        protected override void OnPositionChanged()
        {
            if (Position.Y < -ImageInfo.Size.Height * 2)
                Position += yreset;
            else if (Position.Y > Settings.ScreenHeight + ImageInfo.Size.Height)
                Position -= yreset;
            else if (Position.X < -ImageInfo.Size.Width * 2)
                Position += xreset;
            else if (Position.X > Settings.ScreenWidth + ImageInfo.Size.Width)
                Position -= xreset;
        }
    }
    public class FlowerParticle : Particle
    {
        private static int xwidth = (int)(400 * (Math.Ceiling(Settings.ScreenWidth / 400M) + 2));
        private static int ywidth = (int)(400 * (Math.Ceiling(Settings.ScreenHeight / 400M) + 2));
        private Vector2 xreset = new Vector2(xwidth, 0);
        private Vector2 yreset = new Vector2(0, ywidth);

        public FlowerParticle(ParticleEngine engine, ParticleImageInfo image)
        {
            Engine = engine;
            ImageInfo = image;
        }

        public override void Update()
        {
           if (CMain.Now < NextUpdateTime) return;

            NextUpdateTime = CMain.Now.AddMilliseconds(20);
            Position += Velocity;
        }

        protected override void OnPositionChanged()
        {
            if (Position.Y < -ImageInfo.Size.Height * 2)
                Position += yreset;
            else if (Position.Y > Settings.ScreenHeight + ImageInfo.Size.Height)
                Position -= yreset;
            else if (Position.X < -ImageInfo.Size.Width * 2)
                Position += xreset;
            else if (Position.X > Settings.ScreenWidth + ImageInfo.Size.Width)
                Position -= xreset;
        }
    }
}
