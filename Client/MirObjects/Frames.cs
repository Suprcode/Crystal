namespace Client.MirObjects
{
    public class FrameSet : Dictionary<MirAction, Frame>
    {
        public static FrameSet Player;
        public static FrameSet DefaultNPC, DefaultMonster;
        public static List<FrameSet> DragonStatue, GreatFoxSpirit, HellBomb, CaveStatue;

        static FrameSet()
        {
            FrameSet frame;

            Player = new FrameSet();

            //Default NPC
            DefaultNPC = new FrameSet
            {
                { MirAction.Standing, new Frame(0, 4, 0, 450) },
                { MirAction.Harvest, new Frame(12, 10, 0, 200) }
            };

            //Default Monster
            DefaultMonster = new FrameSet
            {
                { MirAction.Standing, new Frame(0, 4, 0, 500) },
                { MirAction.Walking, new Frame(32, 6, 0, 100) },
                { MirAction.Attack1, new Frame(80, 6, 0, 100) },
                { MirAction.Struck, new Frame(128, 2, 0, 200) },
                { MirAction.Die, new Frame(144, 10, 0, 100) },
                { MirAction.Dead, new Frame(153, 1, 9, 1000) },
                { MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true } }
            };

            #region DragonStatue
            //DragonStatue 1
            DragonStatue = new List<FrameSet> { (frame = new FrameSet()) };
            frame.Add(MirAction.Standing, new Frame(300, 1, -1, 1000));
            frame.Add(MirAction.AttackRange1, new Frame(300, 1, -1, 120));
            frame.Add(MirAction.Struck, new Frame(300, 1, -1, 200));

            //DragonStatue 2
            DragonStatue.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(301, 1, -1, 1000));
            frame.Add(MirAction.AttackRange1, new Frame(301, 1, -1, 120));
            frame.Add(MirAction.Struck, new Frame(301, 1, -1, 200));

            //DragonStatue 3
            DragonStatue.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(302, 1, -1, 1000));
            frame.Add(MirAction.AttackRange1, new Frame(302, 1, -1, 120));
            frame.Add(MirAction.Struck, new Frame(302, 1, -1, 200));

            //DragonStatue 4
            DragonStatue.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(320, 1, -1, 1000));
            frame.Add(MirAction.AttackRange1, new Frame(320, 1, -1, 120));
            frame.Add(MirAction.Struck, new Frame(320, 1, -1, 200));

            //DragonStatue 5
            DragonStatue.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(321, 1, -1, 1000));
            frame.Add(MirAction.AttackRange1, new Frame(321, 1, -1, 120));
            frame.Add(MirAction.Struck, new Frame(321, 1, -1, 200));

            //DragonStatue 6
            DragonStatue.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(322, 1, -1, 1000));
            frame.Add(MirAction.AttackRange1, new Frame(322, 1, -1, 120));
            frame.Add(MirAction.Struck, new Frame(322, 1, -1, 200));
            #endregion

            #region GreatFoxSpirit
            //GreatFoxSpirit level 0
            GreatFoxSpirit = new List<FrameSet> { (frame = new FrameSet()) };
            frame.Add(MirAction.Standing, new Frame(0, 20, -20, 100));
            frame.Add(MirAction.Attack1, new Frame(22, 8, -8, 120));
            frame.Add(MirAction.Struck, new Frame(20, 2, -2, 200));
            frame.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });

            //GreatFoxSpirit level 1
            GreatFoxSpirit.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(60, 20, -20, 100));
            frame.Add(MirAction.Attack1, new Frame(82, 8, -8, 120));
            frame.Add(MirAction.Struck, new Frame(80, 2, -2, 200));
            frame.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });

            //GreatFoxSpirit level 2
            GreatFoxSpirit.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(120, 20, -20, 100));
            frame.Add(MirAction.Attack1, new Frame(142, 8, -8, 120));
            frame.Add(MirAction.Struck, new Frame(140, 2, -2, 200));
            frame.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });

            //GreatFoxSpirit level 3
            GreatFoxSpirit.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(180, 20, -20, 100));
            frame.Add(MirAction.Attack1, new Frame(202, 8, -8, 120));
            frame.Add(MirAction.Struck, new Frame(200, 2, -2, 200));
            frame.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });

            //GreatFoxSpirit level 4
            GreatFoxSpirit.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(240, 20, -20, 100));
            frame.Add(MirAction.Attack1, new Frame(262, 8, -8, 120));
            frame.Add(MirAction.Struck, new Frame(260, 2, -2, 200));
            frame.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });
            #endregion

            #region HellBombs
            //HellBomb1
            HellBomb = new List<FrameSet> { (frame = new FrameSet()) };
            frame.Add(MirAction.Standing, new Frame(52, 9, -9, 100) { Blend = true });
            frame.Add(MirAction.Attack1, new Frame(999, 1, -1, 120) { Blend = true });
            frame.Add(MirAction.Struck, new Frame(52, 9, -9, 100) { Blend = true });

            //HellBomb2
            HellBomb.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(70, 9, -9, 100) { Blend = true });
            frame.Add(MirAction.Attack1, new Frame(999, 1, -1, 120) { Blend = true });
            frame.Add(MirAction.Struck, new Frame(70, 9, -9, 100) { Blend = true });

            //HellBomb3
            HellBomb.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(88, 9, -9, 100) { Blend = true });
            frame.Add(MirAction.Attack1, new Frame(999, 1, -1, 120) { Blend = true });
            frame.Add(MirAction.Struck, new Frame(88, 9, -9, 100) { Blend = true });
            #endregion

            #region CaveStatues
            //CaveStatue1
            CaveStatue = new List<FrameSet> { (frame = new FrameSet()) };
            frame.Add(MirAction.Standing, new Frame(0, 1, -1, 100) { Blend = false });
            frame.Add(MirAction.Struck, new Frame(0, 1, -1, 100) { Blend = false });
            frame.Add(MirAction.Die, new Frame(2, 8, -8, 100) { Blend = false });
            frame.Add(MirAction.Dead, new Frame(9, 1, -1, 100) { Blend = false });

            //CaveStatue2
            CaveStatue.Add(frame = new FrameSet());
            frame.Add(MirAction.Standing, new Frame(18, 1, -1, 100) { Blend = false });
            frame.Add(MirAction.Struck, new Frame(18, 1, -1, 100) { Blend = false });
            frame.Add(MirAction.Die, new Frame(20, 8, -8, 100) { Blend = false });
            frame.Add(MirAction.Dead, new Frame(27, 1, -1, 100) { Blend = false });
            #endregion

            #region Player
            //Common
            Player.Add(MirAction.Standing, new Frame(0, 4, 0, 500, 0, 8, 0, 250));
            Player.Add(MirAction.Walking, new Frame(32, 6, 0, 100, 64, 6, 0, 100));
            Player.Add(MirAction.Running, new Frame(80, 6, 0, 100, 112, 6, 0, 100));
            Player.Add(MirAction.Stance, new Frame(128, 1, 0, 1000, 160, 1, 0, 1000));
            Player.Add(MirAction.Stance2, new Frame(300, 1, 5, 1000, 332, 1, 5, 1000));
            Player.Add(MirAction.Attack1, new Frame(136, 6, 0, 100, 168, 6, 0, 100));
            Player.Add(MirAction.Attack2, new Frame(184, 6, 0, 100, 216, 6, 0, 100));
            Player.Add(MirAction.Attack3, new Frame(232, 8, 0, 100, 264, 8, 0, 100));
            Player.Add(MirAction.Attack4, new Frame(416, 6, 0, 100, 448, 6, 0, 100));
            Player.Add(MirAction.Spell, new Frame(296, 6, 0, 100, 328, 6, 0, 100));
            Player.Add(MirAction.Harvest, new Frame(344, 2, 0, 300, 376, 2, 0, 300));
            Player.Add(MirAction.Struck, new Frame(360, 3, 0, 100, 392, 3, 0, 100));
            Player.Add(MirAction.Die, new Frame(384, 4, 0, 100, 416, 4, 0, 100));
            Player.Add(MirAction.Dead, new Frame(387, 1, 3, 1000, 419, 1, 3, 1000));
            Player.Add(MirAction.Revive, new Frame(384, 4, 0, 100, 416, 4, 0, 100) { Reverse = true });
            Player.Add(MirAction.Mine, new Frame(184, 6, 0, 100, 216, 6, 0, 100));
            Player.Add(MirAction.Lunge, new Frame(139, 1, 5, 1000, 300, 1, 5, 1000));

            //Assassin
            Player.Add(MirAction.Sneek, new Frame(464, 6, 0, 100, 496, 6, 0, 100));
            Player.Add(MirAction.DashAttack, new Frame(80, 3, 3, 100, 112, 3, 3, 100));

            //Archer
            Player.Add(MirAction.WalkingBow, new Frame(0, 6, 0, 100, 0, 6, 0, 100));
            Player.Add(MirAction.RunningBow, new Frame(48, 6, 0, 100, 48, 6, 0, 100));
            Player.Add(MirAction.AttackRange1, new Frame(96, 8, 0, 100, 96, 8, 0, 100));
            Player.Add(MirAction.AttackRange2, new Frame(160, 8, 0, 100, 160, 8, 0, 100));
            Player.Add(MirAction.AttackRange3, new Frame(224, 8, 0, 100, 224, 8, 0, 100));
            Player.Add(MirAction.Jump, new Frame(288, 8, 0, 100, 288, 8, 0, 100));

            //Mounts
            Player.Add(MirAction.MountStanding, new Frame(416, 4, 0, 500, 448, 4, 0, 500));
            Player.Add(MirAction.MountWalking, new Frame(448, 8, 0, 100, 480, 8, 0, 500));
            Player.Add(MirAction.MountRunning, new Frame(512, 6, 0, 100, 544, 6, 0, 100));
            Player.Add(MirAction.MountStruck, new Frame(560, 3, 0, 100, 592, 3, 0, 100));
            Player.Add(MirAction.MountAttack, new Frame(584, 6, 0, 100, 616, 6, 0, 100));

            //Fishing
            Player.Add(MirAction.FishingCast, new Frame(632, 8, 0, 100));
            Player.Add(MirAction.FishingWait, new Frame(696, 6, 0, 120));
            Player.Add(MirAction.FishingReel, new Frame(744, 8, 0, 100));

            #endregion
        }
    }

    public class Frame
    {
        public int Start, Count, Skip, EffectStart, EffectCount, EffectSkip;
        public int Interval, EffectInterval;
        public bool Reverse, Blend;

        public int OffSet
        {
            get { return Count + Skip; }
        }

        public int EffectOffSet
        {
            get { return EffectCount + EffectSkip; }
        }

        public Frame(int start, int count, int skip, int interval, int effectstart = 0, int effectcount = 0, int effectskip = 0, int effectinterval = 0)
        {
            Start = start;
            Count = count;
            Skip = skip;
            Interval = interval;
            EffectStart = effectstart;
            EffectCount = effectcount;
            EffectSkip = effectskip;
            EffectInterval = effectinterval;
        }

        public Frame(BinaryReader reader)
        {
            Start = reader.ReadInt32();
            Count = reader.ReadInt32();
            Skip = reader.ReadInt32();
            Interval = reader.ReadInt32();
            EffectStart = reader.ReadInt32();
            EffectCount = reader.ReadInt32();
            EffectSkip = reader.ReadInt32();
            EffectInterval = reader.ReadInt32();
            Reverse = reader.ReadBoolean();
            Blend = reader.ReadBoolean();
        }
    }

}
