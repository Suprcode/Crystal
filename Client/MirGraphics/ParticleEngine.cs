using Client.MirGraphics.Particles;
using Client.MirScenes;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MirGraphics
{
    public enum ParticleType
    {
        None = 0,
        Fog,
        RedFog,
        RedFogEmber,
        BlueFog,
        YellowFog,
        WhiteEmber,
        YellowEmber,
        Test,
        Blizzard,
        BlizzardFrost,
        Bird,
        FogCloud,
        FloatingFlower,
        Sand,
        Snow,
        FlowersRain,
        Rain,
        Leaves,
        FireyLeaves,
        PurpleLeaves


    }

    public class ParticleEngine
    {
        public Vector2 EmitterLocation { get; set; }
        protected List<Particle> particles;
        protected List<ParticleImageInfo> Textures;
        public Vector2 ForceVelocity = Vector2.Zero;
        public bool GenerateParticles;
        public DateTime NextParticleTime;
        public DateTime NextVelocityTime;

        public TimeSpan NextVelocityUpdate = TimeSpan.FromMilliseconds(500);

        public TimeSpan UpdateDelay = TimeSpan.FromMilliseconds(50);

        ParticleType type;
        public ParticleEngine(List<ParticleImageInfo> textures, Vector2 location, ParticleType type)
        {
            EmitterLocation = location;
            Textures = textures;
            particles = new List<Particle>();
            GenerateParticles = true;
            this.type = type;
        }

        public virtual Particle GenerateNewParticle(ParticleType type)
        {
            Particle particle = null;
            switch (type)
            {
                case ParticleType.Fog:
                    particle = new FogParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.White,
                        Size = 1F,
                        BlendRate = 0.4F,
                        AliveTime = DateTime.MaxValue,
                        Blend = false,
                        //BlendRate = (rate / (float)100),
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.Sand:
                    particle = new SandParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.Yellow,
                        Size = 1F,
                        BlendRate = 0.2F,
                        AliveTime = DateTime.MaxValue,
                        Blend = false,
                        //BlendRate = (rate / (float)100),
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.Snow:
                    particle = new SnowParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.White,
                        Size = 1F,
                        BlendRate = 1F,
                        AliveTime = DateTime.MaxValue,
                        Blend = true,
                        //BlendRate = (rate / (float)100),
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.FireyLeaves:
                    particle = new FogParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.Firebrick,
                        BlendRate = 1F,
                        AliveTime = DateTime.MaxValue,
                        Blend = true,
                        //BlendRate = (rate / (float)100),
                    };
                    particles.Add(particle);

                    break;
                    

                case ParticleType.Leaves:
                    particle = new FogParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.Goldenrod,
                        BlendRate = 0.1F,
                        AliveTime = DateTime.MaxValue,
                        Blend = true,
                        BlendMode = BlendMode.NORMAL

                        //BlendRate = (rate / (float)100),
                    };
                    particles.Add(particle);

                    break;
                case ParticleType.PurpleLeaves:
                    particle = new FogParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.Purple,
                        BlendRate = 0.1F,
                        AliveTime = DateTime.MaxValue,
                        Blend = true,
                        BlendMode = BlendMode.NORMAL

                        //BlendRate = (rate / (float)100),
                    };
                    particles.Add(particle);

                    break;
                case ParticleType.Rain:
                    particle = new Particle()
                    {
                        Engine = this,
                        ImageInfo = Textures[CMain.Random.Next(Textures.Count)],
                        Color = System.Drawing.ColorTranslator.FromHtml("#ffffff85"),
                       // Color = Color.White,
                        Size = 1F,
                        BlendRate = 1F,
                        AliveTime = DateTime.MaxValue,
                        Blend = true,
                        //BlendRate = (rate / (float)100),
                    };

                    particles.Add(particle);
                    break;

                case ParticleType.FlowersRain:
                    particle = new FlowerParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.White,
                        Size = 1F,
                        BlendRate = 0.5F,
                        AliveTime = DateTime.MaxValue,
                        Blend = false,
                        //BlendRate = (rate / (float)100),
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.YellowFog:
                    particle = new FogParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.Yellow,
                        Size = 1F,
                        BlendRate = 0.25F,
                        AliveTime = DateTime.MaxValue,
                        Blend = false,
                        //BlendRate = (rate / (float)100),
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.FogCloud:
                    particle = new FogParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.Transparent,
                        Size = 1F,
                        BlendRate = 0.2F,
                        AliveTime = DateTime.MaxValue,
                        Blend = false,
                        //BlendRate = (rate / (float)100),
                    };

                    particles.Add(particle);
                    break;

                case ParticleType.RedFog:

                    particle = new FogParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.DarkRed,
                        Size = 1F,
                        AliveTime = DateTime.MaxValue,
                        BlendRate = 0.2F,
                        Blend = false,
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.FloatingFlower:

                    particle = new Particle()
                    {
                        Engine = this,
                        ImageInfo = Textures[CMain.Random.Next(Textures.Count)],
                        Color = Color.White,
                        Size = (float)CMain.Random.NextDouble(),
                        AliveTime = CMain.Now.AddSeconds(5 + CMain.Random.Next(4)),
                        Blend = true,
                        BlendRate = 1F,
                        Position = new Vector2(CMain.Random.Next(Settings.ScreenWidth), CMain.Random.Next(Settings.ScreenHeight / 4, Settings.ScreenHeight * 2)),
                        Velocity = new Vector2(-2F * CMain.Random.Next(4), -2F * CMain.Random.Next(3)),
                    };

                    particles.Add(particle);
                    break;

                case ParticleType.BlueFog:

                    particle = new FogParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.DeepSkyBlue,
                        Size = 1F,
                        AliveTime = DateTime.MaxValue,
                        BlendRate = 0.2F,
                        Blend = false,

                    };

                    particles.Add(particle);
                    break;
                case ParticleType.Blizzard:

                    particle = new FogParticle(this, Textures[CMain.Random.Next(Textures.Count)])
                    {
                        Color = Color.FromArgb(255, 172, 229, 238),
                        Size = 1F,
                        AliveTime = DateTime.MaxValue,
                        BlendRate = 0.2F,
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.BlizzardFrost:

                    particle = new Particle()
                    {
                        Engine = this,
                        ImageInfo = Textures[CMain.Random.Next(Textures.Count)],
                        Color = Color.White,
                        Size = (float)CMain.Random.NextDouble(),
                        AliveTime = CMain.Now.AddSeconds(1 + CMain.Random.Next(2)),
                        Blend = false,
                        BlendRate = 0.35F,
                        Position = new Vector2(CMain.Random.Next(Settings.ScreenWidth), CMain.Random.Next(Settings.ScreenHeight / 2, Settings.ScreenHeight)),
                        Velocity = new Vector2(0, 3F * CMain.Random.Next(3)),
                    };

                    particles.Add(particle);
                    break;

             
                case ParticleType.RedFogEmber:
                    particle = new Particle()
                    {
                        Engine = this,
                        ImageInfo = Textures[CMain.Random.Next(Textures.Count)],
                        Color = Color.DarkRed,
                        Size = (float)CMain.Random.NextDouble(),
                        AliveTime = CMain.Now.AddSeconds(1 + CMain.Random.Next(2)),
                        Blend = true,
                        BlendRate = 0.35F,
                        Position = new Vector2(CMain.Random.Next(Settings.ScreenWidth), CMain.Random.Next(Settings.ScreenHeight / 2, Settings.ScreenHeight)),
                        Velocity = new Vector2(0, -2F * CMain.Random.Next(3)),
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.WhiteEmber:
                    particle = new Particle()
                    {
                        Engine = this,
                        ImageInfo = Textures[CMain.Random.Next(Textures.Count)],
                        Color = Color.White,
                        Size = (float)CMain.Random.NextDouble(),
                        AliveTime = CMain.Now.AddSeconds(1 + CMain.Random.Next(2)),
                        Blend = true,
                        BlendRate = 0.35F,
                        Position = new Vector2(CMain.Random.Next(Settings.ScreenWidth), CMain.Random.Next(Settings.ScreenHeight / 2, Settings.ScreenHeight)),
                        Velocity = new Vector2(0, -2F * CMain.Random.Next(3)),
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.YellowEmber:
                    particle = new Particle()
                    {
                        Engine = this,
                        ImageInfo = Textures[CMain.Random.Next(Textures.Count)],
                        Color = Color.Yellow,
                        Size = (float)CMain.Random.NextDouble(),
                        AliveTime = CMain.Now.AddSeconds(1 + CMain.Random.Next(2)),
                        Blend = true,
                        BlendRate = 0.35F,
                        Position = new Vector2(CMain.Random.Next(Settings.ScreenWidth), CMain.Random.Next(Settings.ScreenHeight / 2, Settings.ScreenHeight)),
                        Velocity = new Vector2(0, -2F * CMain.Random.Next(3)),
                    };

                    particles.Add(particle);
                    break;
                case ParticleType.Bird:
                    particle = new Particle()
                    {
                        Engine = this,
                        ImageInfo = Textures[CMain.Random.Next(Textures.Count)],
                        Color = Color.White,
                        Size = (float)CMain.Random.NextDouble(),
                        AliveTime = CMain.Now.AddSeconds(1 + CMain.Random.Next(2)),
                        Blend = true,
                        BlendRate = 0.35F,
                        Position = new Vector2(CMain.Random.Next(Settings.ScreenWidth), CMain.Random.Next(Settings.ScreenHeight / 4, Settings.ScreenHeight * 2)),
                        Velocity = new Vector2(-2, -2F * CMain.Random.Next(3)),
                    };

                    particles.Add(particle);
                    break;
            }
            return particle;
        }

        protected Particle FindParticleFromLocation(Vector2 positon)
        {
            foreach (Particle particle in particles)
            {
                if (particle.Position == positon)
                    return particle;
            }
            return null;
        }

        public virtual void Update()
        {
           
        }
        public void Process()
        {
            foreach (var particle in particles)
            {
                particle.ProcessImage();
            }

            if (GenerateParticles && CMain.Now > NextParticleTime)
            {
                NextParticleTime = CMain.Now + UpdateDelay;
                GenerateNewParticle(type);
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                //particles[particle].ProcessImage();

                if (CMain.Now > particles[particle].AliveTime)
                {
                    particles[particle].OnParticleEnd();
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public virtual void Draw()
        {
            for (int index = 0; index < particles.Count; index++)
                particles[index].Draw();
        }

        public void ParticlesOffSet(Point offset)
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                    var particleObj = particles[particle];
                    if (particleObj.GetType() == typeof(FogParticle))
                        continue;
                    
                    particles[particle].Position += new Vector2(offset.X, offset.Y);

                }    

        }
        public void ParticlesOffSet(int x, int y)
        {
            for (int particle = 0; particle < particles.Count; particle++)
                particles[particle].Position += new Vector2(x, y);
        }
      

        public void Dispose()
        {
            for (int i = particles.Count - 1; i > 0; i--)
            {
                particles[i].ImageInfo.Library = null;
                particles[i].ImageInfo = null;
                particles[i].Engine = null;
                particles.RemoveAt(i);
            }
            particles = null;

            for (int i = Textures.Count - 1; i > 0; i--)
            {
                Textures[i].Library = null;
                Textures.RemoveAt(i);
            }
            Textures = null;

            EmitterLocation = Vector2.Zero;
            ForceVelocity = Vector2.Zero;
        }
    }
}
