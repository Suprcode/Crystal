using Client.MirGraphics;
using Client.MirScenes;
using Client.MirSounds;
using S = ServerPackets;

namespace Client.MirObjects
{
    class SpellObject : MapObject
    {
        public override ObjectType Race
        {
            get { return ObjectType.Spell; }
        }

        public override bool Blocking
        {
            get { return false; }
        }

        public Spell Spell;
        public Point AnimationOffset = new(0, 0);
        public int FrameCount, FrameInterval, FrameIndex;
        public bool Repeat, Ended;
        

        public SpellObject(uint objectID) : base(objectID)
        {
        }

        public void Load(S.ObjectSpell info)
        {
            CurrentLocation = info.Location;
            MapLocation = info.Location;
            GameScene.Scene.MapControl.AddObject(this);
            Spell = info.Spell;
            Direction = info.Direction;
            Repeat = true;
            Ended = false;

            switch (Spell)
            {
                case Spell.TrapHexagon:
                    BodyLibrary = Libraries.Magic;
                    DrawFrame = 1390;
                    FrameInterval = 100;
                    FrameCount = 10;
                    Blend = true;
                    break;
                case Spell.FireWall:
                    BodyLibrary = Libraries.Magic;
                    DrawFrame = 1630;
                    FrameInterval = 120;
                    FrameCount = 6;
                    Light = 3;
                    Blend = true;
                    break;
                case Spell.PoisonCloud:
                    BodyLibrary = Libraries.Magic2;
                    DrawFrame = 1650;
                    FrameInterval = 120;
                    FrameCount = 20;
                    Light = 3;
                    Blend = true;
                    break;
                case Spell.DigOutZombie:
                    BodyLibrary = (ushort)Monster.DigOutZombie < Libraries.Monsters.Count() ? Libraries.Monsters[(ushort)Monster.DigOutZombie] : Libraries.Magic;
                    DrawFrame = 304 + (byte) Direction;
                    FrameCount = 0;
                    Blend = false;
                    break;
                case Spell.Blizzard:
                    AnimationOffset = new Point(0, -20);
                    BodyLibrary = Libraries.Magic2;
                    DrawFrame = 1550;
                    FrameInterval = 100;
                    FrameCount = 30;
                    Light = 3;
                    Blend = true;
                    Repeat = false;
                    break;
                case Spell.MeteorStrike:
                    AnimationOffset = new Point(0, -20);
                    MapControl.Effects.Add(new Effect(Libraries.Magic2, 1600, 10, 800, CurrentLocation) { Repeat = true, RepeatUntil = CMain.Time + 3000 });
                    BodyLibrary = Libraries.Magic2;
                    DrawFrame = 1610;
                    FrameInterval = 100;
                    FrameCount = 30;
                    Light = 3;
                    Blend = true;
                    Repeat = false;
                    break;
                case Spell.Rubble:
                    if (Direction == 0)
                        BodyLibrary = null;
                    else
                    {
                        BodyLibrary = Libraries.Effect;
                        DrawFrame = 64 + Math.Min(4, (int)(Direction - 1));
                        FrameCount = 1;
                        FrameInterval = 10000;
                    }
                    break;
                case Spell.Reincarnation:
                    BodyLibrary = Libraries.Magic2;
                    DrawFrame = 1680;
                    FrameInterval = 100;
                    FrameCount = 10;
                    Light = 1;
                    Blend = true;
                    Repeat = true;
                    break;
                case Spell.ExplosiveTrap:
                    BodyLibrary = Libraries.Magic3;
                    if (info.Param)
                    {
                        DrawFrame = 1570;
                        FrameInterval = 100;
                        FrameCount = 9;
                        Repeat = false;
                        SoundManager.PlaySound(20000 + 124 * 10 + 5);//Boom for all players in range
                    }
                    else
                    {
                        DrawFrame = 1560;
                        FrameInterval = 100;
                        FrameCount = 10;
                        Repeat = true;
                    }
                    //Light = 1;
                    Blend = true;
                    break;
                case Spell.Trap:
                    BodyLibrary = Libraries.Magic2;
                    DrawFrame = 2360;
                    FrameInterval = 100;
                    FrameCount = 8;
                    Blend = true;
                    break;
                case Spell.MapLightning:
                    MapControl.Effects.Add(new Effect(Libraries.Dragon, 400 + (CMain.Random.Next(3) * 10), 5, 600, CurrentLocation));
                    SoundManager.PlaySound(8301);
                    break;
                case Spell.MapLava:
                    MapControl.Effects.Add(new Effect(Libraries.Dragon, 440, 20, 1600, CurrentLocation) { Blend = false });
                    MapControl.Effects.Add(new Effect(Libraries.Dragon, 470, 10, 800, CurrentLocation));
                    SoundManager.PlaySound(8302);
                    break;
                case Spell.MapQuake1:
                    MapControl.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.HellLord], 27, 12, 1200, CurrentLocation) { Blend = false });
                    SoundManager.PlaySound(8304);
                    break;
                case Spell.MapQuake2:
                    MapControl.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.HellLord], 39, 13, 1300, CurrentLocation) { Blend = false });
                    SoundManager.PlaySound(8304);
                    break;
                case Spell.DigOutArmadillo:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.Armadillo];
                    DrawFrame = 472 + (byte)Direction;
                    FrameCount = 0;
                    Blend = false;
                    break;
                case Spell.GeneralMeowMeowThunder:                
                    MapControl.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.GeneralMeowMeow], 522, 7, 700, CurrentLocation) { Blend = true });
                    SoundManager.PlaySound(8321);
                    break;
                case Spell.StoneGolemQuake:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.StoneGolem];
                    DrawFrame = 368 + (int)Direction * 8;
                    FrameInterval = 100;
                    FrameCount = 8;
                    Light = 0;
                    Blend = false;
                    Repeat = false;
                    SoundManager.PlaySound(8304);
                    break;
                case Spell.EarthGolemPile:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.EarthGolem];
                    DrawFrame = 441;
                    FrameInterval = 100;
                    FrameCount = 8;
                    Light = 0;
                    Blend = false;
                    Repeat = false;
                    SoundManager.PlaySound(8331);
                    break;
                case Spell.TreeQueenMassRoots:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.TreeQueen];
                    DrawFrame = 82;
                    FrameInterval = 100;
                    FrameCount = 15;
                    Blend = false;
                    Repeat = false;
                    SoundManager.PlaySound(8341);
                    MapControl.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.TreeQueen], 97, 14, 1400, CurrentLocation) { Blend = true });
                    break;
                case Spell.TreeQueenGroundRoots:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.TreeQueen];
                    DrawFrame = 48;
                    FrameInterval = 100;
                    FrameCount = 9;
                    Blend = false;
                    Repeat = false;
                    SoundManager.PlaySound(8342);
                    MapControl.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.TreeQueen], 57, 9, 900, CurrentLocation) { Blend = true });
                    break;
                case Spell.TreeQueenRoot:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.TreeQueen];
                    DrawFrame = 111;
                    FrameInterval = 100;
                    FrameCount = 15;
                    Blend = false;
                    Repeat = false;
                    SoundManager.PlaySound(8343);
                    break;
                case Spell.TucsonGeneralRock:
                    MapControl.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.TucsonGeneral], 552, 20, 2000, CurrentLocation) { Repeat = false, Blend = false });
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.TucsonGeneral];
                    DrawFrame = 572;
                    FrameInterval = 100;
                    FrameCount = 20;
                    Light = 1;
                    Blend = true;
                    Repeat = false;
                    break;
                case Spell.Portal:
                    BodyLibrary = Libraries.Magic2;
                    DrawFrame = 2360;
                    FrameInterval = 100;
                    FrameCount = 8;
                    Blend = true;
                    break;
                case Spell.HealingCircle:
                    BodyLibrary = Libraries.Magic3;
                    DrawFrame = 630;
                    FrameInterval = 80;
                    FrameCount = 11;
                    Light = 3;
                    Blend = true;
                    break;
                case Spell.FlyingStatueIceTornado:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.FlyingStatue];
                    DrawFrame = 314;
                    FrameInterval = 100;
                    FrameCount = 20;
                    Blend = true;
                    Repeat = false;
                    SoundManager.PlaySound(8303);
                    break;
                case Spell.DarkOmaKingNuke:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.DarkOmaKing];
                    DrawFrame = 1630 + (int)Direction * 9;
                    FrameInterval = 100;
                    FrameCount = 9;
                    Blend = true;
                    Repeat = false;
                    SoundManager.PlaySound(((ushort)Monster.DarkOmaKing * 10) + 9);
                    break;
                case Spell.HornedSorcererDustTornado:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.HornedSorceror];
                    DrawFrame = 634;
                    FrameInterval = 100;
                    FrameCount = 10;
                    Blend = true;
                    Repeat = true;
                    SoundManager.PlaySound(8306);
                    break;
                case Spell.HornedCommanderRockFall:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.HornedCommander];
                    DrawFrame = 1066;
                    FrameInterval = 100;
                    FrameCount = 12;
                    Blend = true;
                    Repeat = true;
                    SoundManager.PlaySound(8456);
                    break;
                case Spell.HornedCommanderRockSpike:
                    BodyLibrary = Libraries.Monsters[(ushort)Monster.HornedCommander];
                    DrawFrame = 1190;
                    FrameInterval = 100;
                    FrameCount = 9;
                    Blend = false;
                    Repeat = true;
                    SoundManager.PlaySound(8457);
                    MapControl.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.HornedCommander], 1199, 9, 900, CurrentLocation) { Blend = true });
                    break;
            }

            NextMotion = CMain.Time + FrameInterval;
            NextMotion -= NextMotion % 100;
        }

        public override void Process()
        {
            if (CMain.Time >= NextMotion)
            {
                if (++FrameIndex >= FrameCount && Repeat)
                {
                    FrameIndex = 0;
                    Ended = true;
                }

                NextMotion = CMain.Time + FrameInterval;

                switch (Spell)
                {
                    case Spell.TucsonGeneralRock:
                        if (FrameIndex == 10) SoundManager.PlaySound(8305);
                        break;
                    case Spell.HornedSorcererDustTornado:
                        if (FrameIndex == 0 && CMain.Random.Next(3) == 0) SoundManager.PlaySound(8306);
                        break;
                    case Spell.HornedCommanderRockSpike:
                        if (Ended)
                        {
                            DrawFrame = 1198;
                            FrameCount = 1;
                            FrameIndex = 0;
                        }
                        break;
                }
            }

            DrawLocation = new Point((CurrentLocation.X - User.Movement.X + MapControl.OffSetX) * MapControl.CellWidth, (CurrentLocation.Y - User.Movement.Y + MapControl.OffSetY) * MapControl.CellHeight);
            DrawLocation.Offset(GlobalDisplayLocationOffset);
            DrawLocation.Offset(User.OffSetMove);
        }

        public override void Draw()
        {
            if (FrameIndex >= FrameCount && !Repeat) return;
            if (BodyLibrary == null) return;

            if (Blend)
            {
                BodyLibrary.DrawBlend(
                    DrawFrame + FrameIndex,
                    AnimationOffset == default ? DrawLocation : GetDrawWithOffset(),
                    DrawColour, true,
                    0.8F);
            }
            else
            {
                BodyLibrary.Draw(DrawFrame + FrameIndex,
                    AnimationOffset == default ? DrawLocation : GetDrawWithOffset(),
                    DrawColour,
                    true);
            }
        }

        public override bool MouseOver(Point p)
        {
            return false;
        }

        public override void DrawBehindEffects(bool effectsEnabled)
        {
        }

        public override void DrawEffects(bool effectsEnabled)
        { 
        }

        private Point GetDrawWithOffset()
        {
            Point newDrawLocation = new (
                (CurrentLocation.X + AnimationOffset.X - User.Movement.X + MapControl.OffSetX) * MapControl.CellWidth,
                (CurrentLocation.Y + AnimationOffset.Y - User.Movement.Y + MapControl.OffSetY) * MapControl.CellHeight);

            newDrawLocation.Offset(GlobalDisplayLocationOffset);
            newDrawLocation.Offset(User.OffSetMove);

            return newDrawLocation;
        }
    }
}
