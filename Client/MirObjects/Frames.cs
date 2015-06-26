﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.MirObjects
{
    public class FrameSet
    {
        public static FrameSet Players;
        public static List<FrameSet> HighPlayers; //stupple
        public static List<FrameSet> NPCs; //Make Array
        public static List<FrameSet> Monsters;
        public static List<FrameSet> HelperPets; //IntelligentCreature

        public Dictionary<MirAction, Frame> Frames = new Dictionary<MirAction, Frame>();


        static FrameSet()
        {
            FrameSet frame;
            NPCs = new List<FrameSet>();
            
            HighPlayers = new List<FrameSet>();//stupple

            Monsters = new List<FrameSet>();

            Players = new FrameSet();

            HelperPets = new List<FrameSet>(); //IntelligentCreature

            /*
             * PLAYERS
             */

            #region Player Frames
            //Common
            Players.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500, 0, 8, 0, 250));
            Players.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100, 64, 6, 0, 100));
            Players.Frames.Add(MirAction.Running, new Frame(80, 6, 0, 100, 112, 6, 0, 100));
            Players.Frames.Add(MirAction.Stance, new Frame(128, 1, 0, 1000, 160, 1, 0, 1000));
            Players.Frames.Add(MirAction.Stance2, new Frame(300, 1, 5, 1000, 300, 1, 5, 1000));
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
            //0
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));

            //1
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

            //3
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Show, new Frame(4, 8, -8, 200));
            frame.Frames.Add(MirAction.Hide, new Frame(11, 8, -8, 200) { Reverse = true });
            frame.Frames.Add(MirAction.Attack1, new Frame(12, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(60, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(76, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(85, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(76, 10, 0, 100) { Reverse = true });

            //4
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 4, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(147, 1, 3, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 4, 0, 100) { Reverse = true });
            
            //5
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(128, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(176, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(192, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(201, 1, 9, 1000));

            //6
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, -10, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, -10, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, -10, 100) { Reverse = true });

            //7
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 1000));
            frame.Frames.Add(MirAction.Show, new Frame(22, 10, -10, 150));
            frame.Frames.Add(MirAction.Hide, new Frame(31, 10, -10, 150) { Reverse = true });
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, -10, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, -10, 150) { Reverse = true });

            //8
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, -10, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, -10, 150) { Reverse = true });

            //9
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //10
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 20, -20, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(31, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 20, -20, 100) { Reverse = true });

            //11
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

            //12
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

            //13
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
            
            //14
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //15
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Appear, new Frame(224, 10, -10, 100) { Blend = true });
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //16
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Appear, new Frame(0, 10, 0, 100));
            frame.Frames.Add(MirAction.Standing, new Frame(80, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(112, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Show, new Frame(256, 10, 0, 100));
            frame.Frames.Add(MirAction.Hide, new Frame(265, 10, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Revive, new Frame(176, 10, 0, 100) { Reverse = true });

            //17
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Show, new Frame(224, 10, 0, 200));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //18
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(224, 10, 0, 100));

            //19
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //20
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //21
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

            //22
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

            //23
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

            //24
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Appear, new Frame(216, 10, -10, 100) { Blend = true });
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500) { Blend = true });
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.AttackRange1, new Frame(80, 6, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200) { Blend = true });
            frame.Frames.Add(MirAction.Die, new Frame(144, 9, 0, 100) { Blend = true });
            frame.Frames.Add(MirAction.Revive, new Frame(144, 9, 0, 100) { Blend = true, Reverse = true });

            //25
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 18, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, 16, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, 20, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, 12, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, 21, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, 12, 150) { Reverse = true });

            //26
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, 5, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(0, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 1, 5, 200));
            frame.Frames.Add(MirAction.Die, new Frame(48, 10, 0, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(57, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(48, 10, 0, 150) { Reverse = true });

            //27
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 1, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(88, 6, 1, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(144, 1, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(160, 10, 0, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(169, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(160, 10, 0, 150) { Reverse = true });

            //28
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 180));
            frame.Frames.Add(MirAction.Struck, new Frame(10, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(12, 10, -10, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(12, 10, -10, 150) { Reverse = true });

            //29
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(464, 20, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 20, 0, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(163, 1, 19, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 20, 0, 150) { Reverse = true });

            //30
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //31
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Show, new Frame(4, 5, -5, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(9, 5, -5, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(14, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(16, 10, -10, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(25, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(16, 10, -10, 100) { Reverse = true });

            //32
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 4, -4, 200));

            //33
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

            //39
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 4, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(112, 6, 4, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(192, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(208, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(217, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(208, 10, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.AttackRange1, new Frame(288, 6, 0, 100));

            //40
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 10, -10, 1000));
            frame.Frames.Add(MirAction.Attack1, new Frame(42, 8, -8, 120));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(10, 6, 4, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(40, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(42, 7, -7, 120));
            frame.Frames.Add(MirAction.Dead, new Frame(48, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(42, 7, -7, 120) { Reverse = true });

            //41 - Dragon Statue 1
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(300, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(300, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(300, 1, -1, 200));

            //42 - Dragon Statue 2
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(301, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(301, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(301, 1, -1, 200));

            //43 - Dragon Statue 3
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(302, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(302, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(302, 1, -1, 200));

            //44 - Dragon Statue 4
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(320, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(320, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(320, 1, -1, 200));

            //45 - Dragon Statue 5
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(321, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(321, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(321, 1, -1, 200));

            //46 - Dragon Statue 6
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(322, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(322, 1, -1, 120));
            frame.Frames.Add(MirAction.Struck, new Frame(322, 1, -1, 200));

            //47 - Archer Guard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 3, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(104, 6, 3, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(176, 2, 0 , 100));
            frame.Frames.Add(MirAction.Die, new Frame(192, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(201, 1, 9, 1000));

            //48 - Taoist Guard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(32, 6, 0, 100));

            //49 - Archer SummonVampire
            Monsters.Add(frame = new FrameSet());
            //frame.Frames.Add(MirAction.???, new Frame(0, 3, 0, 500));// dunno what miraction to assign to this ?
            frame.Frames.Add(MirAction.Show, new Frame(24, 6, 0, 150));
            frame.Frames.Add(MirAction.Hide, new Frame(29, 6, 0, 150) { Reverse = true });
            frame.Frames.Add(MirAction.Standing, new Frame(72, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(104, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(152, 5, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(192, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(216, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(225, 1, 9, 1000));

            //50 - Archer SummonToad
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(32, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(104, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(128, 10, 0, 100));
            frame.Frames.Add(MirAction.Hide, new Frame(208, 4, 0, 100));
            frame.Frames.Add(MirAction.Show, new Frame(211, 4, 0, 100) { Reverse = true });
            frame.Frames.Add(MirAction.Dead, new Frame(137, 1, 9, 1000));

            //51 - Archer SummonSnakes Totem
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(0, 2, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 1, 0, 100));
            frame.Frames.Add(MirAction.Die, new Frame(0, 1, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(0, 1, 0, 100));

            //52 - Archer SummonSnakes Snake
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 5, 0, 200));
            frame.Frames.Add(MirAction.Walking, new Frame(0, 5, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(0, 5, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 5, 0, 100));
            frame.Frames.Add(MirAction.Die, new Frame(52, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(59, 1, 7, 1000));

            //53
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(152, 4, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(155, 1, 3, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(152, 4, 0, 100) { Reverse = true });

            //54 DirtPiles & TreasureChest (147, 156, 164, 165)
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 3, -3, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(3, 3, -3, 200));
            frame.Frames.Add(MirAction.Die, new Frame(3, 7, -7, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(9, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(3, 7, -7, 150) { Reverse = true });

            //55 Football (149)
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, 0, 1000));
            frame.Frames.Add(MirAction.Walking, new Frame(8, 6, 0, 100));

            //56
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(152, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(157, 1, 5, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(152, 6, 0, 100) { Reverse = true });

            //57
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //58
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(96, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(168, 6, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(173, 1, 5, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(168, 6, 0, 100) { Reverse = true });

            //59
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(272, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //60
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

            //61
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //62
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 2, 0, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(16, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(32, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(39, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(32, 8, 0, 100) { Reverse = true });

            //63
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 1, -1, 1000));
            frame.Frames.Add(MirAction.Struck, new Frame(1, 4, -4, 200));
            frame.Frames.Add(MirAction.Die, new Frame(5, 7, -7, 150));
            frame.Frames.Add(MirAction.Dead, new Frame(11, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(5, 7, -7, 150) { Reverse = true });

            //64
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(120, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(96, 3, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(168, 5, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(173, 1, 4, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(168, 5, 0, 100) { Reverse = true });

            //65
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 8, -8, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(8, 3, -3, 200));
            frame.Frames.Add(MirAction.Die, new Frame(11, 8, -8, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(17, 1, -1, 1000));

            //66 (Mob 184)
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(128, 11, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(216, 9, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(288, 5, 0, 100));
            //frame.Frames.Add(MirAction.Attack4, new Frame(328, 8, 0, 100));
            //frame.Frames.Add(MirAction.Attack5, new Frame(392, 5, 0, 200));
            //frame.Frames.Add(MirAction.Attack6, new Frame(432, 8, 0, 100));
            //frame.Frames.Add(MirAction.Attack7, new Frame(496, 8, 0, 100));
            //This mob has a lot actions , I just listed the frame numbers ready for adding when coding the AI

            //67 (Mob187)
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(248, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(184, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(176, 9, 0, 100) { Reverse = true });
            //frame.Frames.Add(MirAction.Attack3, new Frame(296, 6, 0, 100));
            //frame.Frames.Add(MirAction.Attack4, new Frame(344, 6, 0, 100));
            //frame.Frames.Add(MirAction.Attack5, new Frame(392, 8, 0, 100));

            //68 (Bush)
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Struck, new Frame(4, 4, -4, 200));
            frame.Frames.Add(MirAction.Die, new Frame(8, 4, -4, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(15, 1, -1, 1000));

            //69 HellSlasher, HellCannibal, ManectricClub, ManectricClaw
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //70 HellPirate, FlameSpear
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 7, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(224, 7, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //71 HellBolt, WitchDoctor
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(185, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(256, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(256, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(176, 10, 0, 100) { Reverse = true });

            //72 Hellkeeper         
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 8, -8, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(22, 10, -10, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(12, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(14, 8, -8, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(21, 1, -1, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(21, 1, 0, 100) { Reverse = true });

            //73 ManectricHammer
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 7, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(150, 1, 6, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 7, 0, 100) { Reverse = true });

            //74 ManectricStaff
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 10, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(248, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(160, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(176, 9, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(184, 1, 8, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(176, 9, 0, 100) { Reverse = true });

            //75 ManectricBlest, NamelessGhost, DarkGhost, ChaosGhost, TrollHammer, TrollBommer, TrollStoner
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //76 ManectricKing, TrollKing
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 8, 0, 100));
            frame.Frames.Add(MirAction.Attack3, new Frame(224, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 8, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //77 FlameMage, FlameScythe, FlameAssassin
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(224, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //78 FlameQueen
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(224, 9, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 10, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(153, 1, 9, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(224, 9, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(224, 9, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 10, 0, 100) { Reverse = true });

            //79 HellKnight1~4
            Monsters.Add(frame = new FrameSet());
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

            //80 HellLord
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, -4, 500));
            frame.Frames.Add(MirAction.Attack1, new Frame(4, 6, -6, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(10, 5, -5, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(0, 2, -2, 200));
            frame.Frames.Add(MirAction.Die, new Frame(0, 4, -4, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(0, 1, -1, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(4, 6, -6, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(10, 5, -5, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(0, 4, -4, 100) { Reverse = true });

            //81 WaterGuard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 7, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(208, 7, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //82 IceGuard
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 200));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack2, new Frame(208, 6, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange2, new Frame(208, 6, 0, 100));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            //83 DarkDevourer
            Monsters.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 4, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(32, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(80, 6, 0, 100));
            frame.Frames.Add(MirAction.AttackRange1, new Frame(208, 8, 0, 100));
            frame.Frames.Add(MirAction.Struck, new Frame(128, 2, 0, 200));
            frame.Frames.Add(MirAction.Die, new Frame(144, 8, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(151, 1, 7, 1000));
            frame.Frames.Add(MirAction.Revive, new Frame(144, 8, 0, 100) { Reverse = true });

            #endregion

            /*
             * INTELLIGENTCREATURES             
             */
            #region IntelligentCreature Frames

            //0 - BabyPig
            HelperPets.Add(frame = new FrameSet());
            frame.Frames.Add(MirAction.Standing, new Frame(0, 6, 0, 500));
            frame.Frames.Add(MirAction.Walking, new Frame(48, 6, 0, 100));
            frame.Frames.Add(MirAction.Attack1, new Frame(96, 9, 0, 100));      //Standing Variant_0
            frame.Frames.Add(MirAction.Attack2, new Frame(168, 5, 0, 100));     //Standing Variant_1
            frame.Frames.Add(MirAction.Attack3, new Frame(208, 10, 0, 100));    //Standing Variant_2
            frame.Frames.Add(MirAction.Attack4, new Frame(288, 10, 0, 100));    //Standing Variant_3
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
            frame.Frames.Add(MirAction.Attack1, new Frame(104, 6, 0, 100));         // belly dance :P
            frame.Frames.Add(MirAction.Attack2, new Frame(152, 7, 0, 100));         // melting
            frame.Frames.Add(MirAction.Die, new Frame(152, 7, 0, 100));
            frame.Frames.Add(MirAction.Dead, new Frame(158, 1, 6, 1000));
            //Effect1 Snow : 208,11,0,100

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
        
        public Frame(int start, int count, int skip, int interval, bool isEqual)//stupple 
        {
            Start = start;
            Count = count;
            Skip = skip;
            Interval = interval;
            EffectStart = isEqual ? start : 0;
            EffectCount = isEqual ? count : 0;
            EffectSkip = isEqual ? skip : 0;
            EffectInterval = isEqual ? interval : 0;
        }
    }

}
