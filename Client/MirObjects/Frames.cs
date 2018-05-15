using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.MirObjects
{
    public class FrameSet
    {
        public static FrameSet Players;
        public static List<FrameSet> NPCs; //Make Array
        public static List<FrameSet> Monsters;
        public static List<FrameSet> HelperPets; //IntelligentCreature
        public static List<FrameSet> Gates;
        public static List<FrameSet> Walls;

        public Dictionary<MirAction, Frame> Frames = new Dictionary<MirAction, Frame>();

        static FrameSet()
        {
            FrameSet frame;
            NPCs = new List<FrameSet>();

            Monsters = new List<FrameSet>();

            Players = new FrameSet();

            HelperPets = new List<FrameSet>(); //IntelligentCreature

            Gates = new List<FrameSet>();

            Walls = new List<FrameSet>();

            /*
             * PLAYERS
             */

            #region Player Frames
            //Common
            Players.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500, 0, 8, 0, 250));
            Players.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100, 64, 6, 0, 100));
            Players.Frames.Add(MirAction.Running, new Frame(80, 6, 0, 100, 112, 6, 0, 100));
            Players.Frames.Add(MirAction.Stance, new Frame(128, 1, 0, 1000, 160, 1, 0, 1000));
            Players.Frames.Add(MirAction.Stance2, new Frame(300, 1, 5, 1000, 332, 1, 5, 1000));
            Players.Frames.Add(MirAction.Attack1, new Frame(136, 6, 0, 100, 168, 6, 0, 100));
            Players.Frames.Add(MirAction.Attack2, new Frame(184, 6, 0, 100, 216, 6, 0, 100));
            Players.Frames.Add(MirAction.Attack3, new Frame(232, 8, 0, 100, 264, 8, 0, 100));
            Players.Frames.Add(MirAction.Attack4, new Frame(416, 6, 0, 100, 448, 6, 0, 100));
            Players.Frames.Add(MirAction.Spell, new Frame(296, 6, 0, 100, 328, 6, 0, 100));
            Players.Frames.Add(MirAction.Harvest, new Frame(344, 2, 0, 300, 376, 2, 0, 300));
            Players.Frames.Add(MirAction.Struck, new Frame(360, 3, 0, 100, 392, 3, 0, 100));
            Players.Frames.Add(MirAction.Die, new Frame(384, 4, 0, 100, 416, 4, 0, 100));
            Players.Frames.Add(MirAction.Dead, new Frame(387, 1, 3, 1000, 419, 1, 3, 1000));
            Players.Frames.Add(MirAction.Revive, new Frame(384, 4, 0, 100, 416, 4, 0, 100) { Reverse = true });
            Players.Frames.Add(MirAction.Mine, new Frame(184, 6, 0, 100, 216, 6, 0, 100));
            Players.Frames.Add(MirAction.Lunge, new Frame(139, 1, 5, 1000, 300, 1, 5, 1000)); //slashingburst test

            //Assassin
            Players.Frames.Add(MirAction.Sneek, new Frame(464, 6, 0, 100, 496, 6, 0, 100));
            Players.Frames.Add(MirAction.DashAttack, new Frame(80, 3, 3, 100, 112, 3, 3, 100));

            //Archer
            Players.Frames.Add(MirAction.WalkingBow, new Frame(0, 6, 0, 100, 0, 6, 0, 100));
            Players.Frames.Add(MirAction.RunningBow, new Frame(48, 6, 0, 100, 48, 6, 0, 100));
            Players.Frames.Add(MirAction.AttackRange1, new Frame(96, 8, 0, 100, 96, 8, 0, 100));
            Players.Frames.Add(MirAction.AttackRange2, new Frame(160, 8, 0, 100, 160, 8, 0, 100));
            Players.Frames.Add(MirAction.AttackRange3, new Frame(224, 8, 0, 100, 224, 8, 0, 100));
            Players.Frames.Add(MirAction.Jump, new Frame(288, 8, 0, 100, 288, 8, 0, 100));

            //Mounts
            Players.Frames.Add(MirAction.MountStanding, new Frame(416, 4, 0, 500, 448, 4, 0, 500));
            Players.Frames.Add(MirAction.MountWalking, new Frame(448, 8, 0, 100, 480, 8, 0, 500));
            Players.Frames.Add(MirAction.MountRunning, new Frame(512, 6, 0, 100, 544, 6, 0, 100));
            Players.Frames.Add(MirAction.MountStruck, new Frame(560, 3, 0, 100, 592, 3, 0, 100));
            Players.Frames.Add(MirAction.MountAttack, new Frame(584, 6, 0, 100, 616, 6, 0, 100));

            //Fishing
            Players.Frames.Add(MirAction.FishingCast, new Frame(632, 8, 0, 100));
            Players.Frames.Add(MirAction.FishingWait, new Frame(696, 6, 0, 120));
            Players.Frames.Add(MirAction.FishingReel, new Frame(744, 8, 0, 100));

            #endregion

            /*
             * NPCS
             */

            #region NPC Frames
            //0 4 frames + direction + harvest(10 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 450));
            frame.Frames.Add(MirAction.Harvest, new Frame(12, 10, 0, 200));

            //1 4 frames + direction + harvest(20 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(4, 4, 0, 450));
            frame.Frames.Add(MirAction.Harvest, new Frame(12, 20, 0, 200));

            //2 4 frames, 4 frames + direction
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 450));

            //3 12 frames + animation(10 frames) (large tele)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 12, 0, 250, 12, 10, 2, 250));

            //4 2 frames + animation(9 frames) (small tele)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, 0, 1250, 2, 9, 1, 250));

            //5 2 frame + animation(6 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, 0, 300, 2, 6, 0, 100));

            //6 1 frame
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, 0, 1500));

            //7 10 frames
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 250));

            //8 12 frames
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 12, 0, 250));

            //9 8 frames
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 8, 0, 250));

            //10 6 frames + direction
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 250));

            //11 1 frame + animation(8 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, 0, 400, 2, 8, 0, 100));

            //12 11 frames
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 11, 0, 250));

            //13 20 frames + animation(20 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 20, 0, 450, 20, 20, 0, 450));

            //14 4 frames + direction + animation(4 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 250, 12, 4, 0, 250));

            //15 4 frames + harvest(6 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 250));
            frame.Frames.Add(MirAction.Harvest, new Frame(12, 6, 0, 200));

            //16 6 frames + animation(12 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 400, 6, 12, 0, 200));

            //17 9 frames + direction
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 9, 0, 650));

            //18 5 frames + direction
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 5, 0, 400));

            //19 7 frames + direction + harvest(10 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 7, 0, 550));
            frame.Frames.Add(MirAction.Harvest, new Frame(21, 10, 0, 200));

            //20 1 frame + animation(9 frames)
            NPCs.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, 0, 900, 1, 9, 0, 100));
            #endregion

            /*
             * MONSTERS             
             */

            #region Monster Frames
            //0 - Guard, Guard2
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));

            //1 - Hen, Deer, Sheep, Wolf, Pig, Bull, DarkBrownWolf
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Skeleton, new Frame(224, 1, 0, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //2 - Regular
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //3 - CannibalPlant
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Show, new Frame(4, 8, -8, 200));
            frame.Frames.Add(MirAction.Hide, new Frame(12, 8, -8, 200) { Reverse = true });
            frame.Frames.Add(MirAction.Attack1, new Frame(20, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(68, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(84, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(93, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(84, 10, 0, 100) { Reverse = true });

            //4 - ForestYeti, CaveMaggot, FrostYeti
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 4, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(147, 1, 3, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 4, 0, 100) { Reverse = true });

            //5 - Scorpion
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(128, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(176, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(192, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(201, 1, 9, 1000));

            //6 - ChestnutTree, EbonyTree, LargeMushroom, CherryTree, ChristmasTree, SnowTree
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, -10, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, -10, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, -10, 100) { Reverse = true });

            //7 - EvilCentipede
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 1000));
            frame.Frames.Add(MirAction.Show, new Frame(22, 10, -10, 150));
            frame.Frames.Add(MirAction.Hide, new Frame(31, 10, -10, 150) { Reverse = true });
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, -10, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, -10, 150) { Reverse = true });

            //8 - BugBatMaggot
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, -10, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, -10, 150) { Reverse = true });

            //9 - CrystalSpider, WhiteFoxman, LightTurtle, CrystalWeaver
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //10 - RedMoonEvil
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 20, -20, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(31, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 20, -20, 100) { Reverse = true });

            //11 - ZumaStatue, ZumaGuardian, FrozenZumaStatue, FrozenZumaGuardian
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Stoned, new Frame(0, 1, 5, 100));
            frame.Frames.Add(MirAction.Show, new Frame(0, 6, 0, 100));
            frame.Frames.Add(MirAction.Hide, new Frame(5, 6, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Standing, new Frame(48, 4, 0, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(176, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(192, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(201, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(192, 10, 0, 100) { Reverse = true });

            //12 - ZumaTaurus
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Stoned, new Frame(0, 1, -1, 100));
            frame.Frames.Add(MirAction.Show, new Frame(0, 20, -20, 100));
            frame.Frames.Add(MirAction.Standing, new Frame(20, 4, 0, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(52, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(100, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(148, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(164, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(173, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(164, 10, 0, 100) { Reverse = true });

            //13 - RedThunderZuma, FrozenRedZuma
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Stoned, new Frame(272, 1, 5, 100));
            frame.Frames.Add(MirAction.Show, new Frame(272, 6, 0, 100));
            frame.Frames.Add(MirAction.Hide, new Frame(277, 6, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //14 - KingScorpion, DarkDevil, RightGuard, LeftGuard, MinotaurKing
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //15 - BoneFamilar
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Appear, new Frame(224, 10, -10, 100) { Blend = true });
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //16 - Shinsu
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Appear, new Frame(0, 10, 0, 100));
            frame.Frames.Add(MirAction.Standing, new Frame(80, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(112, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Show, new Frame(256, 10, 0, 100));
            frame.Frames.Add(MirAction.Hide, new Frame(265, 10, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Revive, new Frame(176, 10, 0, 100) { Reverse = true });

            //17 - DigOutZombie
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Show, new Frame(224, 10, 0, 200));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //18 - ClZombie, NdZombie, CrawlerZombie
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(224, 10, 0, 100));

            //19 - ShamanZombie
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //20 - Khazard, FinialTurtle
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //21 - BoneLord
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(128, 6, 0, 200));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(176, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(224, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(240, 20, 0, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(259, 1, 19, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(240, 20, 0, 150) { Reverse = true });

            //22 - FrostTiger, FlameTiger
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.SitDown, new Frame(272, 4, 0, 500));

            //23 Yimoogi, RedYimoogi, Snake10-17
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //24 - HolyDeva
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Appear, new Frame(216, 10, -10, 100) { Blend = true });
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500) { Blend = true });
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.AttackRange1, new Frame(80, 6, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200) { Blend = true });
            frame.Frames.Add(MirAction.Die, new Frame(144, 9, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.Revive, new Frame(144, 9, 0, 100) { Blend = true, Reverse = true });

            //25 - GreaterWeaver, RootSpider
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 18, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, 16, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, 20, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, 12, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, 21, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, 12, 150) { Reverse = true });

            //26 - BombSpider, MutatedHugger
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, 5, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(0, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 1, 5, 200));
            frame.Frames.Add(MirAction.Die, new Frame(48, 10, 0, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(57, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(48, 10, 0, 150) { Reverse = true });

            //27 - CrossbowOma, DarkCrossbowOma
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 1, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(88, 6, 1, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(144, 1, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(160, 10, 0, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(169, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(160, 10, 0, 150) { Reverse = true });

            //28 - YinDevilNode, YangDevilNode
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 180));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, -10, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, -10, 150) { Reverse = true });

            //29 - OmaKing
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(464, 20, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 20, 0, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(163, 1, 19, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 20, 0, 150) { Reverse = true });

            //30 - BlackFoxman, RedFoxman
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //31 - TrapRock
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Show, new Frame(4, 5, -5, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(9, 5, -5, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(14, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(16, 10, -10, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(25, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(16, 10, -10, 100) { Reverse = true });

            //32 - GuardianRock
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 4, -4, 200));

            //33 - ThunderElement, CloudElement
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, -10, 800));
            frame.Frames.Add(MirAction.Walking, new Frame(10, 10, -10, 80));
            frame.Frames.Add(MirAction.Attack1, new Frame(20, 10, -10, 80));
            frame.Frames.Add(MirAction.Struck, new Frame(30, 4, -4, 200));
            frame.Frames.Add(MirAction.Die, new Frame(34, 10, -10, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(43, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(34, 10, -10, 150) { Reverse = true });

            //34 - GreatFoxSpirit level 0
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 20, -20, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(22, 8, -8, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(20, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });

            //35 - GreatFoxSpirit level 1
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(60, 20, -20, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(82, 8, -8, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(80, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });

            //36 - GreatFoxSpirit level 2
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(120, 20, -20, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(142, 8, -8, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(140, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });

            //37 - GreatFoxSpirit level 3
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(180, 20, -20, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(202, 8, -8, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(200, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });

            //38 - GreatFoxSpirit level 4
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(240, 20, -20, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(262, 8, -8, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(260, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(300, 18, -18, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(317, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(300, 18, -18, 150) { Reverse = true });

            //39 - HedgeKekTal, BigHedgeKekTal
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 4, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 6, 4, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(192, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(208, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(217, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(208, 10, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.AttackRange1, new Frame(288, 6, 0, 100));

            //40 - EvilMir
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, -10, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(42, 8, -8, 120));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(10, 6, 4, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(40, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(42, 7, -7, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(48, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(42, 7, -7, 120) { Reverse = true });

            //41 - DragonStatue 1
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(300, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(300, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(300, 1, -1, 200));

            //42 - DragonStatue 2
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(301, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(301, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(301, 1, -1, 200));

            //43 - DragonStatue 3
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(302, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(302, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(302, 1, -1, 200));

            //44 - DragonStatue 4
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(320, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(320, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(320, 1, -1, 200));

            //45 - DragonStatue 5
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(321, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(321, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(321, 1, -1, 200));

            //46 - DragonStatue 6
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(322, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(322, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(322, 1, -1, 200));

            //47 - ArcherGuard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 3, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(104, 6, 3, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(176, 2, 0, 100));
            frame.Frames.Add(MirAction.Die, new Frame(192, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(201, 1, 9, 1000));

            //48 - TaoistGuard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(32, 6, 0, 100));

            //49 - VampireSpider (Archer SummonVampire)
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Show, new Frame(24, 6, 0, 150));
            frame.Frames.Add(MirAction.Hide, new Frame(29, 6, 0, 150) { Reverse = true });
            frame.Frames.Add(MirAction.Standing, new Frame(72, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(104, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(152, 5, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(192, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(216, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(225, 1, 9, 1000));

            //50 - SpittingToad (Archer SummonToad)
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(32, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(104, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(128, 10, 0, 100));
            frame.Frames.Add(MirAction.Hide, new Frame(208, 4, 0, 100));
            frame.Frames.Add(MirAction.Show, new Frame(211, 4, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Dead, new Frame(137, 1, 9, 1000));

            //51 - SnakeTotem (Archer SummonSnakes Totem)
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, -2, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(0, 2, -2, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 1, -1, 100));
            frame.Frames.Add(MirAction.Die, new Frame(0, 1, -1, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(0, 1, -1, 100));

            //52 - CharmedSnake (Archer SummonSnakes Snake)
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 5, 0, 200));
            frame.Frames.Add(MirAction.Walking, new Frame(0, 5, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(0, 5, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 5, 0, 100));
            frame.Frames.Add(MirAction.Die, new Frame(52, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(59, 1, 7, 1000));

            //53 - HighAssassin
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(152, 4, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(155, 1, 3, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(152, 4, 0, 100) { Reverse = true });

            //54 - DarkDustPile, MudPile, SnowPile, Treasurebox
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 3, -3, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(3, 3, -3, 200));
            frame.Frames.Add(MirAction.Die, new Frame(3, 7, -7, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(9, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(3, 7, -7, 150) { Reverse = true });

            //55 - Football
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, 0, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(8, 6, 0, 100));

            //56 - GingerBreadman
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(152, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(157, 1, 5, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(152, 6, 0, 100) { Reverse = true });

            //57 - DreamDevourer
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(208, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //58 - TailedLion
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(96, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(168, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(173, 1, 5, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(168, 6, 0, 100) { Reverse = true });

            //59 - Behemoth
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(352, 7, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(272, 10, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(408, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //60 - Hugger, ManectricSlave
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(256, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //61 - DarkDevourer
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(208, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //62 - Snowman
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, 0, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(16, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(32, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(39, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(32, 8, 0, 100) { Reverse = true });

            //63 - GiantEgg, IcePillar
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, -1, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(1, 4, -4, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(0, 1, -1, 1000));
            frame.Frames.Add(MirAction.Die, new Frame(5, 7, -7, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(11, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(5, 7, -7, 150) { Reverse = true });

            //64 - BlueSanta
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(144, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(168, 5, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(172, 1, 4, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(168, 5, 0, 100) { Reverse = true });

            //65 - BattleStandard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 8, -8, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(8, 3, -3, 200));
            frame.Frames.Add(MirAction.Die, new Frame(11, 8, -8, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(17, 1, -1, 1000));

            //66 - WingedTigerLord
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(328, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(288, 5, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(392, 5, 0, 150));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(432, 8, 0, 150));
            frame.Frames.Add(MirAction.Struck, new Frame(192, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(216, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(224, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(216, 9, 0, 100) { Reverse = true });

            //67 - TurtleKing
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 150));
            frame.Frames.Add(MirAction.Attack2, new Frame(248, 6, 0, 150));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(1000, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(176, 9, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.AttackRange1, new Frame(296, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(344, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange3, new Frame(392, 8, 0, 100));

            //68 - Bush
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(4, 4, -4, 200));
            frame.Frames.Add(MirAction.Die, new Frame(8, 4, -4, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(15, 1, -1, 1000));

            //-----------------------
            //--ABOVE FRAMES LOCKED, NO NEED TO TEST ABOVE--
            //-----------------------

            //69 - HellSlasher, HellCannibal, ManectricClub
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //70 - HellPirate
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //71 - HellBolt, WitchDoctor
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(185, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(256, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(176, 10, 0, 100) { Reverse = true });

            //72 - Hellkeeper         
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 8, -8, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(22, 10, -10, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(12, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(14, 8, -8, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(21, 1, 0, 100) { Reverse = true });

            //73 - ManectricHammer
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 7, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(150, 1, 6, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 7, 0, 100) { Reverse = true });

            //74 - ManectricStaff
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(248, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(184, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(176, 9, 0, 100) { Reverse = true });

            //75 - ManectricBlest, NamelessGhost, DarkGhost, ChaosGhost, TrollHammer, TrollBomber, TrollStoner, MutatedManworm, CrazyManworm
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(272, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //76 - ManectricKing, TrollKing
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 120));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(288, 9, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(224, 8, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //77 - FlameSpear, FlameMage, FlameScythe, FlameAssassin
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //78 - FlameQueen
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(296, 8, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //79 - HellKnight1~4
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Appear, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(176, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 4, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(147, 1, 3, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(176, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 4, 0, 100) { Reverse = true });

            //80 - HellLord
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(10, 5, -5, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(14, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(10, 5, -5, 100) { Reverse = true });

            //81 - WaterGuard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //82 - IceGuard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //83 - DemonGuard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 4, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(112, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(128, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(135, 1, 7, 1000));
            frame.Frames.Add(MirAction.Attack2, new Frame(192, 6, 0, 100));
            frame.Frames.Add(MirAction.Show, new Frame(240, 6, 0, 200));

            //84 - KingGuard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 6, 0, 100));//bugy
            frame.Frames.Add(MirAction.AttackRange1, new Frame(272, 8, 0, 100));//ragneg ?
            frame.Frames.Add(MirAction.AttackRange2, new Frame(336, 7, 0, 100));//ragneg ?
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //--------------------
            //--CHECK NO FURTHER UNTIL ABOVE HAS BEEN LOCKED--
            //--------------------

            //85 - Bunny, Bunny2
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 5, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(72, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(149, 1, 4, 1000));

            //86 - DarkBeast, LightBeast 
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(152, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(184, 1, 0, 1000) { Reverse = true });
            frame.Frames.Add(MirAction.Attack2, new Frame(248, 6, 0, 100));

            //87 - HardenRhino
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 10, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(184, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(208, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(217, 1, 0, 1000) { Reverse = true });
            frame.Frames.Add(MirAction.Attack2, new Frame(288, 7, 0, 100));

            //88 - AncientBringer
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(304, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(192, 4, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(224, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(233, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(384, 8, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(448, 8, 0, 100));

            //89 - Jar1
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(18, 4, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(50, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(59, 1, 0, 1000));

            //90 - SeedingsGeneral
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.SitDown, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Standing, new Frame(32, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(64, 7, 0, 200));
            //frame.Frames.Add(MirAction.Runing, new Frame(120, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(168, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(240, 9, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(312, 8, 0, 100)); //stupple 08/04
            frame.Frames.Add(MirAction.AttackRange2, new Frame(376, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(448, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(472, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(479, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(472, 10, 0, 100) { Reverse = true });

            //91 - Tucson, TucsonFighter
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(168, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(192, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(200, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(192, 10, 0, 100) { Reverse = true });

            //92 - FrozenDoor
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, -1, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(1, 3, -3, 200));
            frame.Frames.Add(MirAction.Die, new Frame(4, 7, -7, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(10, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(4, 7, -7, 150) { Reverse = true });

            //93 - TucsonMage
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 7, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(56, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(104, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(216, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(240, 7, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(246, 1, 6, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(240, 7, 0, 100) { Reverse = true });

            //94 - TucsonWarrior
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 5, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(40, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(88, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(216, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(240, 7, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(246, 1, 6, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(240, 7, 0, 100) { Reverse = true });

            //95 - Armadillo
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(240, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(312, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(336, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(345, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(336, 10, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Show, new Frame(416, 7, 0, 200));

            //96 - ArmadilloElder
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 5, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(40, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(88, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(168, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(248, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(320, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(344, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(353, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(336, 10, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Show, new Frame(424, 7, 0, 200));

            //97 - TucsonEgg
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, -1, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(1, 1, -1, 200));
            frame.Frames.Add(MirAction.Die, new Frame(10, 10, -10, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(11, 1, -1, 1000));

            //98 - PlaguedTucson 
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(184, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(191, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(184, 8, 0, 100) { Reverse = true });

            //99 - SandSnail 
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 9, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(72, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(184, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(264, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(344, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(368, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(377, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(368, 10, 0, 100) { Reverse = true });

            //100 - CannibalTentacles
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 9, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(72, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(184, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(344, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(368, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(377, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(368, 10, 0, 100) { Reverse = true });

            //101 - TucsonGeneral  
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 8, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(64, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(168, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 7, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(280, 8, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(344, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(408, 4, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(440, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(448, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(440, 8, 0, 100) { Reverse = true });

            //102 - GasToad 
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 8, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(64, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(192, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(256, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(336, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(360, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(369, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(360, 10, 0, 100) { Reverse = true });

            //103 - Mantis
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 7, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(56, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(104, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(168, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(224, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(248, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(288, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(248, 8, 0, 100) { Reverse = true });

            //104 - SwampWarrior 
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(288, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(312, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(321, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(312, 10, 0, 100) { Reverse = true });

            //105 - AssassinBird
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 7, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(56, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(104, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(232, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(304, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(328, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(335, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(328, 8, 0, 100) { Reverse = true });

            //106 - RhinoWarrior 
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(184, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(240, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(264, 7, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(270, 1, 6, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(264, 7, 0, 100) { Reverse = true });

            //107 - RhinoPriest 
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(208, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(280, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(304, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(312, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(304, 9, 0, 100) { Reverse = true });

            //108 - SwampSlime
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(288, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(312, 7, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(218, 1, 6, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(312, 7, 0, 100) { Reverse = true });

            //109 - RockGuard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 9, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(72, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(200, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(280, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(304, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(311, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(304, 8, 0, 100) { Reverse = true });

            //110 - MudWarrior
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 8, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(64, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(192, 10, 0, 100));
            //frame.Frames.Add(MirAction.Attack3, new Frame(272, 10, 0, 100));
            //frame.Frames.Add(MirAction.AttackRange1, new Frame(848, 8, 0, 100)); 
            //frame.Frames.Add(MirAction.AttackRange2, new Frame(912, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(272, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(296, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(303, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(396, 8, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Show, new Frame(360, 9, 0, 200));

            //111 - SmallPot
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(12, 9, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(84, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(132, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(212, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(292, 10, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(372, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(452, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(476, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(303, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(476, 1, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Show, new Frame(360, 9, 0, 200));

            //112 - TreeQueen
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, -10, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(11, 10, -10, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(21, 3, -3, 200));
            frame.Frames.Add(MirAction.Die, new Frame(24, 11, -11, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(34, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(24, 11, -11, 100) { Reverse = true });

            //113 - ShellFighter
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 9, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(72, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(192, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(272, 9, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(344, 9, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(416, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(496, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(520, 9, 1, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(528, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(520, 9, 1, 100) { Reverse = true });

            //114 - DarkBaboon
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(232, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(296, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(152, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 7, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(182, 1, 6, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(176, 7, 0, 100) { Reverse = true });

            //115 - TwinHeadBeast
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 9, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(72, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(296, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(192, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(216, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(225, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(216, 10, 0, 100) { Reverse = true });

            //116 - OmaCannibal
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 9, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(72, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(184, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(256, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(280, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(289, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(280, 10, 0, 100) { Reverse = true });

            //117 - OmaSlasher
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(208, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(232, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(240, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(232, 9, 0, 100) { Reverse = true });

            //118 - OmaAssassin
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(208, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(232, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(241, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(232, 10, 0, 100) { Reverse = true });

            //119 - OmaMage //DUPE of 104
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(288, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(312, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(311, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(312, 10, 0, 100) { Reverse = true });

            //120 - OmaWitchDoctor
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 9, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(72, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(176, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(232, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(304, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(328, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(336, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(280, 9, 0, 100) { Reverse = true });

            //121 - OmaBlest //DUPE of 104
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(288, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(312, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(321, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(312, 10, 0, 100) { Reverse = true });

            //122 - LightningBead, HealingBead, PowerUpBead
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 7, -7, 800));
            frame.Frames.Add(MirAction.Walking, new Frame(7, 7, -7, 80));
            frame.Frames.Add(MirAction.Attack1, new Frame(8, 5, -5, 80));
            frame.Frames.Add(MirAction.Struck, new Frame(14, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(16, 8, -8, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(23, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(34, 10, -10, 150) { Reverse = true });

            //123 - DarkOmaKing
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(200, 34, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(472, 8, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(536, 9, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(608, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(680, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(704, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(703, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(704, 10, 0, 100) { Reverse = true });

            //124 - CaveMage
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, -2, 800));
            frame.Frames.Add(MirAction.Attack1, new Frame(0, 2, -2, 80));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(2, 8, -8, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(9, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(2, 8, -8, 150) { Reverse = true });

            //125 - Mandrill
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(136, 3, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(184, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(193, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(184, 10, 0, 100) { Reverse = true });

            //126 - PlagueCrab
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(144, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(168, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(177, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(168, 10, 0, 100) { Reverse = true });

            //127 - CreeperPlant
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(32, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(88, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(136, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(160, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(168, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(160, 9, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Show, new Frame(232, 9, -9, 150));
            frame.Frames.Add(MirAction.Hide, new Frame(241, 9, -9, 150) { Reverse = true });

            //128 - SackWarrior
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(144, 9, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(216, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(240, 13, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(252, 1, 12, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(240, 13, 0, 100) { Reverse = true });

            //129 - WereTiger
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(240, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(264, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(263, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(264, 10, 0, 100) { Reverse = true });

            //130 - KingHydrax
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(144, 7, 0, 200));
            frame.Frames.Add(MirAction.Attack3, new Frame(200, 8, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(264, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(288, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(287, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(288, 10, 0, 100) { Reverse = true });

            //131 - FloatingWraith
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(143, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(167, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(176, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(167, 10, 0, 100) { Reverse = true });

            //132 - ArmedPlant
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(144, 6, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(192, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(198, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(192, 8, 0, 100) { Reverse = true });

            //133 - AvengerPlant
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(136, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(160, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(167, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(160, 8, 0, 100) { Reverse = true });

            //134 - Nadz, AvengingSpirit
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(140, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //135 - AvengingWarrior
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(128, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(178, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(192, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(191, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(192, 10, 0, 100) { Reverse = true });

            //136 - AxePlant, ClawBeast
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(185, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(193, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(185, 8, 0, 100) { Reverse = true });

            //137 - WoodBox
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(1, 1, 0, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(1, 1, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(9, 11, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(19, 1, 0, 1000));

            //138 - KillerPlant
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(80, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(144, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(200, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(256, 7, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(368, 7, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(424, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(480, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(504, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(503, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(504, 10, 0, 100) { Reverse = true });

            //139 - Hydrax
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(144, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(168, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(167, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(168, 9, 0, 100) { Reverse = true });

            //140 - Basiloid
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, -1, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(2, 7, -7, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(8, 1, -1, 1000));

            //141 - HornedMage
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(144, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(216, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(280, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(304, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(313, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(304, 10, 0, 100) { Reverse = true });

            //142 - HornedArcher, ColdArcher
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(232, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(256, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(265, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(256, 10, 0, 100) { Reverse = true });

            //143 - HornedWarrior
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(144, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(216, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(280, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(304, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(312, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(304, 9, 0, 100) { Reverse = true });

            //144 - FloatingRock
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(80, 8, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 7, -7, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(1502, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 7, -7, 100) { Reverse = true });

            //145 - ScalyBeast
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 10, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(256, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(272, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(270, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(272, 9, 0, 100) { Reverse = true });

            //146 - HornedSorceror
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 9, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(295, 8, 0, 200));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(359, 9, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(432, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(456, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(455, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(456, 10, 0, 100) { Reverse = true });

            //147 - BoulderSpirit
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 8, -8, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(8, 3, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(32, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(56, 8, -8, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(63, 1, -1, 1000));

            //148 - HornedCommander
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(176, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(240, 10, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(320, 8, 0, 200));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(384, 8, 0, 200));
            frame.Frames.Add(MirAction.AttackRange3, new Frame(448, 8, 0, 200));
            //frame.Frames.Add(MirAction.AttackRange4, new Frame(512, 10, 0, 200));
            //frame.Frames.Add(MirAction.AttackRange5, new Frame(592, 8, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(656, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(680, 13, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(692, 1, 12, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(680, 13, 0, 100) { Reverse = true });

            //149 - MoonStone, SunStone, LightningStone
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, -2, 500));

            //150 - Turtlegrass //  use black fox ai -- 1turtlegrass
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Hide, new Frame(0, 1, 0, 500));
            frame.Frames.Add(MirAction.Show, new Frame(8, 4, 0, 200) { Reverse = true });
            frame.Frames.Add(MirAction.Standing, new Frame(40, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(72, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(256, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(280, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(289, 1, 0, 1000));
            frame.Frames.Add(MirAction.Attack2, new Frame(184, 9, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //151 - Mantree
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(64, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(96, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(288, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(368, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(392, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(401, 1, 0, 1000));
            //frame.Frames.Add(MirAction.Standing, new Frame(1, 1, 0, 500)); neeed codeing
            //frame.Frames.Add(MirAction.Walking, new Frame(8, 7, 0, 100));neeed codeing

            //152 - Bear //AI BLACKFOX
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(216, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(240, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(248, 1, 0, 1000));
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 8, 0, 100));

            //153 - Leopard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(184, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(191, 1, 0, 1000));

            //154 - ChieftainArcher
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(216, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(240, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(248, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(240, 9, 0, 100) { Reverse = true });

            //155 - ChieftainSword
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(232, 10, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(312, 10, 0, 200));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(384, 9, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(658, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(672, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(681, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(672, 10, 0, 100) { Reverse = true });

            //156 - StoningSpider //Archer Summon
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, -0, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(16, 4, 0, 100));
            frame.Frames.Add(MirAction.Die, new Frame(48, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(57, 1, 9, 1000));

            //157 - FrozenSoldier
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(152, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(175, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(176, 10, 0, 100) { Reverse = true });

            //158 - FrozenFighter //DUPE of 142
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(232, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(256, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(265, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(256, 10, 0, 100) { Reverse = true });

            //159 - FrozenArcher
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(184, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(193, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(184, 10, 0, 100) { Reverse = true });

            //160 - FrozenKnight
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(256, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(280, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(289, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(280, 10, 0, 100) { Reverse = true });

            //161 - FrozenGolem
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(144, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(168, 12, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(179, 1, 11, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(168, 11, 0, 100) { Reverse = true });

            //162 - IcePhantom
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(216, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(240, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(249, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(240, 10, 0, 100) { Reverse = true });

            //163 - SnowWolf
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(232, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(256, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(254, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(256, 9, 0, 100) { Reverse = true });

            //164 - SnowWolfKing
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(232, 10, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(312, 5, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(352, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(376, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(385, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(376, 10, 0, 100) { Reverse = true });

            //165 - WaterDragon //ec ai
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Hide, new Frame(0, 8, 0, 500) { Reverse = true });
            frame.Frames.Add(MirAction.Standing, new Frame(64, 6, 0, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(256, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(280, 15, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(294, 1, 0, 1000));
            //frame.Frames.Add(MirAction.Walking, new Frame(72, 6, 0, 100));

            //166 - BlackTortoise
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            //frame.Frames.Add(MirAction.StandingAlt, new Frame(32, 10, 0, 200));
            frame.Frames.Add(MirAction.Walking, new Frame(112, 4, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(144, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(192, 7, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(248, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(296, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(312, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(326, 1, 5, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(312, 6, 0, 100) { Reverse = true });

            //167 - Manticore //mage ai needed black fox will do 4 now
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(112, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(176, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(240, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(296, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(352, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(376, 15, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(390, 1, 0, 1000));
            //frame.Frames.Add(MirAction.Flying, new Frame(32, 9, 0, 100));

            //168 - DragonWarrior:
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            //frame.Frames.Add(MirAction.Standing2, new Frame(32, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(112, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(176, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(240, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(320, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(376, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(400, 13, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(412, 1, 12, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(400, 13, 0, 100) { Reverse = true });

            //169 - DragonArcher
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            //frame.Frames.Add(MirAction.Standing2, new Frame(32, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(112, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(176, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(240, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(288, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(312, 13, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(324, 1, 12, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(312, 13, 0, 100) { Reverse = true });

            //170 - Kirin
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            //frame.Frames.Add(MirAction.Standing2, new Frame(32, 9, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(104, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(152, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 12, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(304, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(352, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(376, 2, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(377, 1, 0, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(376, 2, 0, 100) { Reverse = true });

            //171 - Guard3
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 7, 0, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(56, 7, 0, 100));

            //172 - ArcherGuard3
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 4, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 7, 3, 100));

            //173 - BLANK
            Monsters.Add(frame = new FrameSet());

            //174 - FrozenMiner
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 10, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(240, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(320, 4, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(352, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(361, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(352, 10, 0, 100) { Reverse = true });

            //175 - FrozenAxeman
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            //frame.Frames.Add(MirAction.Standing2, new Frame(32, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(112, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(256, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(336, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(416, 4, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(448, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(457, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(448, 10, 0, 100) { Reverse = true });

            //176 - FrozenMagician
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            //frame.Frames.Add(MirAction.Standing2, new Frame(32, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(112, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(160, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(240, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(320, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(400, 4, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(432, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(441, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(432, 10, 0, 100) { Reverse = true });

            //177 - SnowYeti
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 7, 0, 500));
            //frame.Frames.Add(MirAction.Standing2, new Frame(56, 10, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(136, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(200, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(272, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(344, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(408, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(432, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(440, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(432, 9, 0, 100) { Reverse = true });

            //178 - IceCrystalSoldier
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(192, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(272, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(352, 4, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(384, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(393, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(384, 10, 0, 100) { Reverse = true });

            //179 - DarkWraith
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 4, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(256, 4, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(280, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(289, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(280, 10, 0, 100) { Reverse = true });

            //180 - CrystalBeast
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(176, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(240, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(288, 5, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(328, 2, 0, 100));
            frame.Frames.Add(MirAction.AttackRange3, new Frame(344, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(408, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(432, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(440, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(432, 8, 0, 100) { Reverse = true });

            //181 - RedOrb, BlueOrb, YellowOrb, GreenOrb, WhiteOrb
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 5, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(120, 5, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 5, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(180, 1, 4, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(176, 5, 0, 100) { Reverse = true });

            //182 - FatalLotus
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(224, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(240, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(245, 1, 5, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(240, 6, 0, 100) { Reverse = true });

            //183 - AntCommander
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(176, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(240, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(304, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(320, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(393, 1, 5, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(320, 6, 0, 100) { Reverse = true });

            //184 - CargoBoxwithlogo
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, 0, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(8, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(24, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(29, 1, 0, 1000));

            //185 - Doe
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(144, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(160, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(165, 1, 5, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(160, 6, 0, 100) { Reverse = true });

            //186 - AngryReindeer
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 7, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(56, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(184, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(256, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(312, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(328, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(335, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(328, 8, 0, 100) { Reverse = true });

            //187 - DeathCrawler
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(152, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(184, 1, 8, 1000));

            //188 - UndeadWolf //Dupe of 126
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(144, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(168, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(177, 1, 9, 1000));

            //189 - BurningZombie //FrozenZombie
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(152, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(185, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(256, 7, 0, 100));//fozzesrange

            //190 - MudZombie 
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 10, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(176, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(200, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(205, 1, 5, 1000));
            frame.Frames.Add(MirAction.Attack2, new Frame(248, 1, 7, 1000));

            //191 - BloodBaboon 
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(152, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(183, 1, 7, 1000));

            //192 - FightingCat, PoisonHugger
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //193 - FireCat
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 5, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(120, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(168, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(184, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(191, 1, 7, 1000));

            //194 - CatWidow
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(128, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(176, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(192, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(199, 1, 7, 1000));

            //195 - StainHammerCat
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(136, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(160, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(169, 1, 9, 1000));

            //196 - BlackHammerCat
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(136, 12, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(232, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(256, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(265, 1, 9, 1000));

            //197 - StrayCat
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            //frame.Frames.Add(MirAction.Walking, new Frame(32, 10, 0, 200));
            frame.Frames.Add(MirAction.Walking, new Frame(112, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(160, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(240, 10, 0, 200));
            //frame.Frames.Add(MirAction.Attack2, new Frame(320, 13, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(424, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(448, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(457, 1, 9, 1000));

            //198 - CatShaman
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 10, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 7, 0, 200));
            //frame.Frames.Add(MirAction.Attack2, new Frame(216, 7, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(272, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(288, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(296, 1, 8, 1000));

            //199 - Jar2
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, 0, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(128, 10, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(208, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(232, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(241, 1, 9, 1000));

            //200 - RestlessJar
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(48, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(120, 10, 0, 200));
            frame.Frames.Add(MirAction.Attack3, new Frame(200, 10, 0, 200));
            frame.Frames.Add(MirAction.Struck, new Frame(280, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(304, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(303, 1, 9, 1000));

            //201 - FlamingMutant, FlyingStatue, ManectricClaw
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 10, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //202 - StoningStatue
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(304, 20, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 20, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(163, 1, 19, 1000));

            //203 - ArcherGuard2
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 3, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(104, 10, 3, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(208, 3, 0, 100));
            frame.Frames.Add(MirAction.Die, new Frame(232, 4, 0, 200));
            frame.Frames.Add(MirAction.Dead, new Frame(235, 1, 3, 1000));

            //204 - Tornado
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500) { Blend = true });
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.AttackRange1, new Frame(116, 8, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200) { Blend = true });
            frame.Frames.Add(MirAction.Die, new Frame(200, 9, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.Dead, new Frame(271, 1, 9, 1000) { Blend = true });
            frame.Frames.Add(MirAction.Revive, new Frame(208, 9, 0, 100) { Blend = true, Reverse = true });

            //205 - ElementBomb1
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(52, 9, -9, 100) { Blend = true });
            frame.Frames.Add(MirAction.Attack1, new Frame(999, 1, -1, 120) { Blend = true });
            frame.Frames.Add(MirAction.Struck, new Frame(52, 9, -9, 100) { Blend = true });

            //206 - ElementBomb2
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(70, 9, -9, 100) { Blend = true });
            frame.Frames.Add(MirAction.Attack1, new Frame(999, 1, -1, 120) { Blend = true });
            frame.Frames.Add(MirAction.Struck, new Frame(70, 9, -9, 100) { Blend = true });

            //207 - ElementBomb3
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(88, 9, -9, 100) { Blend = true });
            frame.Frames.Add(MirAction.Attack1, new Frame(999, 1, -1, 120) { Blend = true });
            frame.Frames.Add(MirAction.Struck, new Frame(88, 9, -9, 100) { Blend = true });

            #endregion

            /*
             * INTELLIGENTCREATURES             
             */
            #region IntelligentCreature Frames

            //0 - BabyPig
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 9, 0, 100));      //normal pickup
            frame.Frames.Add(MirAction.Attack2, new Frame(168, 5, 0, 100));     //oink - sound
            frame.Frames.Add(MirAction.Attack3, new Frame(208, 10, 0, 200));    //walk in circle
            frame.Frames.Add(MirAction.Attack4, new Frame(288, 10, 0, 200));    //lay on back
            frame.Frames.Add(MirAction.Die, new Frame(288, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(297, 1, 9, 1000));

            //1 - Chick
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));         // normal pickup
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 8, 0, 100));         // cleaning himself ?
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 17, 0, 100));        // flying?
            frame.Frames.Add(MirAction.Attack4, new Frame(360, 9, 0, 100));         // chirping or gulping worm ?
            frame.Frames.Add(MirAction.Die, new Frame(360, 1, 9, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(360, 1, 9, 1000));

            //2 - Kitten - BlackKitten
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 6, 0, 100));          // slipping
            frame.Frames.Add(MirAction.Attack2, new Frame(144, 10, 0, 100));        // pickup
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 10, 0, 100));        // rolling
            frame.Frames.Add(MirAction.Attack4, new Frame(304, 7, 0, 100));         // boxing
            frame.Frames.Add(MirAction.Die, new Frame(224, 3, 7, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(226, 1, 9, 1000));

            //3 - BabySkeleton
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 7, 0, 100));          // headbutt
            frame.Frames.Add(MirAction.Attack2, new Frame(136, 10, 0, 100));        // head rotate
            frame.Frames.Add(MirAction.Attack3, new Frame(216, 8, 0, 100));         // poking
            frame.Frames.Add(MirAction.Attack4, new Frame(280, 20, 0, 100));        // die and revive
            frame.Frames.Add(MirAction.Die, new Frame(280, 10, 10, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(290, 1, 19, 1000));

            //4 - Baekdon
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 6, 0, 100));          // mace swing
            frame.Frames.Add(MirAction.Attack2, new Frame(144, 10, 0, 100));        // mace tap
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 7, 0, 100));         // dance
            frame.Frames.Add(MirAction.Attack4, new Frame(280, 5, 0, 100));         // ears dance
            frame.Frames.Add(MirAction.Die, new Frame(280, 5, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(284, 1, 4, 1000));

            //5 - Wimaen
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 7, 0, 100));          // handstand
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 10, 0, 100));        // punch
            frame.Frames.Add(MirAction.Attack3, new Frame(232, 6, 0, 100));         // rolling
            frame.Frames.Add(MirAction.Attack4, new Frame(280, 4, 0, 100));          // wave
            frame.Frames.Add(MirAction.Die, new Frame(280, 4, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(283, 1, 3, 1000));

            //6 - BlackKitten
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 6, 0, 100));          // paw stretch
            frame.Frames.Add(MirAction.Attack2, new Frame(144, 10, 0, 100));        // tail up
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 10, 0, 100));        // rolling
            frame.Frames.Add(MirAction.Attack4, new Frame(304, 7, 0, 100));         // boxing
            frame.Frames.Add(MirAction.Die, new Frame(224, 3, 7, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(226, 1, 9, 1000));

            //7 - BabyDragon
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 7, 0, 100));          // bow down
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 6, 0, 100));         // ball play
            frame.Frames.Add(MirAction.Attack3, new Frame(200, 10, 0, 100));        // tail swing
            frame.Frames.Add(MirAction.Die, new Frame(200, 5, 5, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(204, 1, 9, 1000));

            //8 - OlympicFlame
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 3, 0, 100));          // bow down
            frame.Frames.Add(MirAction.Attack2, new Frame(112, 8, 0, 100));         // jump
            frame.Frames.Add(MirAction.Attack3, new Frame(176, 10, 0, 100));        // lay down
            frame.Frames.Add(MirAction.Die, new Frame(176, 8, 2, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(183, 1, 9, 1000));
            //Effect1 Smoke : 256,3,0,100
            //Effect2 Fire : 280,4,0,100

            //9 - BabySnowMan
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 7, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(104, 6, 0, 100));         // belly dance
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 7, 0, 100));         // melting
            frame.Frames.Add(MirAction.Die, new Frame(152, 7, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(158, 1, 6, 1000));
            //Effect1 Snow : 208,11,0,100

            //10 - Frog
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 6, 0, 100));         // tongue
            frame.Frames.Add(MirAction.Attack2, new Frame(144, 8, 0, 100));         // beltch - sound
            frame.Frames.Add(MirAction.Attack3, new Frame(208, 9, 0, 100));         //rock
            frame.Frames.Add(MirAction.Attack4, new Frame(280, 7, 0, 100));         //backflip
            frame.Frames.Add(MirAction.Die, new Frame(208, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(286, 1, 6, 1000));

            //11 - Monkey
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 8, 0, 100));         // pickup??
            frame.Frames.Add(MirAction.Attack2, new Frame(160, 8, 0, 100));         // sleep
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 9, 0, 100));         //balance
            frame.Frames.Add(MirAction.Attack4, new Frame(296, 9, 0, 100));         //backflip
            frame.Frames.Add(MirAction.Die, new Frame(224, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(304, 1, 8, 1000));

            //12 - AngryBird
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 7, 0, 100));         // pickup??
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 8, 0, 100));         //bow
            frame.Frames.Add(MirAction.Attack3, new Frame(216, 9, 0, 100));         //spin?
            frame.Frames.Add(MirAction.Attack4, new Frame(296, 9, 0, 100));         //hover?
            frame.Frames.Add(MirAction.Die, new Frame(224, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(304, 1, 8, 1000));

            //13 - Foxey
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 8, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(64, 6, 0, 100));          //
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 8, 0, 100));         // pickup??
            frame.Frames.Add(MirAction.Attack2, new Frame(176, 10, 0, 100));         //bow
            frame.Frames.Add(MirAction.Attack3, new Frame(256, 10, 0, 100));         //spin?
            frame.Frames.Add(MirAction.Attack4, new Frame(336, 10, 0, 100));         //hover?
            frame.Frames.Add(MirAction.Die, new Frame(224, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(258, 1, 8, 1000));

            #endregion

            /*
             * Castle Gates             
             */
            #region Gate Frames

            //Sabuk Gate
            Gates.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(80, 1, 7, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(80, 4, 4, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 6, -6, 200));//open
            frame.Frames.Add(MirAction.Attack2, new Frame(128, 6, -6, 200));//close
            frame.Frames.Add(MirAction.Die, new Frame(104, 10, -10, 200));
            frame.Frames.Add(MirAction.Dead, new Frame(113, 1, -1, 1000));


            //Castle Gi Gate South
            Gates.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, 1, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 2, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(22, 7, -7, 200));//open
            frame.Frames.Add(MirAction.Attack2, new Frame(28, 7, -7, 200) { Reverse = true });//close
            frame.Frames.Add(MirAction.Die, new Frame(14, 8, -8, 200));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));

            //Castle Gi Gate East
            Gates.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(29, 1, 1, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(29, 2, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(51, 7, -7, 200));//open
            frame.Frames.Add(MirAction.Attack2, new Frame(57, 7, -7, 200) { Reverse = true });//close
            frame.Frames.Add(MirAction.Die, new Frame(43, 8, -8, 200));
            frame.Frames.Add(MirAction.Dead, new Frame(50, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(50, 8, -8, 200) { Reverse = true });

            //Castle Gi Gate West
            Gates.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(58, 1, 1, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(58, 2, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 7, -7, 200));//open
            frame.Frames.Add(MirAction.Attack2, new Frame(86, 7, -7, 200) { Reverse = true });//close
            frame.Frames.Add(MirAction.Die, new Frame(72, 8, -8, 200));
            frame.Frames.Add(MirAction.Dead, new Frame(79, 1, -1, 1000));

            #endregion

            #region Wall Frames

            //Left of Sabuk Palace
            Walls.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(168, 1, 0, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(168, 1, 0, 1000));
            frame.Frames.Add(MirAction.Die, new Frame(172, 1, -1, 400));
            frame.Frames.Add(MirAction.Dead, new Frame(172, 1, -1, 1000));

            //Left of Sabuk Palace Door
            Walls.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(184, 1, 0, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(184, 1, 0, 1000));
            frame.Frames.Add(MirAction.Die, new Frame(188, 1, -1, 400, 196));
            frame.Frames.Add(MirAction.Dead, new Frame(188, 1, -1, 1000, 196));

            //Right of Sabuk Palace Door
            Walls.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(200, 1, 0, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(200, 1, 0, 1000));
            frame.Frames.Add(MirAction.Die, new Frame(204, 1, -1, 400));
            frame.Frames.Add(MirAction.Dead, new Frame(204, 1, -1, 1000));

            //Left of Shanda Sabuk Palace
            Walls.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, 0, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 1, 0, 1000));
            frame.Frames.Add(MirAction.Die, new Frame(4, 1, -1, 400));
            frame.Frames.Add(MirAction.Dead, new Frame(4, 1, -1, 1000));

            //Left of Shanda Sabuk Palace Door
            Walls.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(5, 1, 0, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(5, 1, 0, 1000));
            frame.Frames.Add(MirAction.Die, new Frame(9, 1, -1, 400));
            frame.Frames.Add(MirAction.Dead, new Frame(9, 1, -1, 1000));

            //Right of Shanda Sabuk Palace Door
            Walls.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(10, 1, 0, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 1, 0, 1000));
            frame.Frames.Add(MirAction.Die, new Frame(14, 1, -1, 400));
            frame.Frames.Add(MirAction.Dead, new Frame(14, 1, -1, 1000));

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
    }

}
