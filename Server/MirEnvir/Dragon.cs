using System;
using System.Collections.Generic;
using System.Drawing;
using Server.MirDatabase;
using Server.MirObjects;
using Server.MirObjects.Monsters;

namespace Server.MirEnvir
{
    public class Dragon
    {
        private int ProcessDelay = 2000;
        public int DeLevelDelay = 60 * (60 * 1000);
        private long ProcessTime;
        public byte MaxLevel = Globals.MaxDragonLevel;
        private Rectangle DropArea;
        public long DeLevelTime;
        public bool Loaded;

        private static Envir Envir
        {
            get { return SMain.Envir; }
        }

        private Point[] BodyLocations = new[]
        {
            new Point(-3, -1),
            new Point(-3, -0),
            new Point(-2, -3),
            new Point(-2, -2),
            new Point(-2, -1),
            new Point(-2, 0),
            new Point(-2, 1),
            new Point(-1, -2),
            new Point(-1, -1),
            new Point(-1, 0),
            new Point(-1, 1),
            new Point(-1, 2),
            new Point(0, -2),
            new Point(0, -1),
            new Point(0, 1),
            new Point(0, 2),
            new Point(0, 3),
            new Point(1, -2),
            new Point(1, 0),
            new Point(1, 1),
            new Point(1, 2),
            new Point(1, 3),
            new Point(2, 1),
            new Point(2, 2),
        };


        public DragonInfo Info;
        public MonsterObject LinkedMonster;

        public Dragon(DragonInfo info)
        {
            Info = info;
        }
        public bool Load()
        {
            try
            {
                MonsterInfo info = Envir.GetMonsterInfo(Info.MonsterName);
                if (info == null)
                {
                    SMain.Enqueue("Failed to load Dragon (bad monster name): " + Info.MonsterName);
                    return false;
                }
                LinkedMonster = MonsterObject.GetMonster(info);

                Map map = SMain.Envir.GetMapByNameAndInstance(Info.MapFileName);
                if (map == null)
                {
                    SMain.Enqueue("Failed to load Dragon (bad map name): " + Info.MapFileName);
                    return false;
                }

                if (Info.Location.X > map.Width || Info.Location.Y > map.Height)
                {
                    SMain.Enqueue("Failed to load Dragon (bad map XY): " + Info.MapFileName);
                    return false;
                }

                if (LinkedMonster.Spawn(map, Info.Location))
                {
                    if (LinkedMonster is EvilMir)
                    {
                        EvilMir mob = (EvilMir)LinkedMonster;
                        if (mob != null)
                        {
                            mob.DragonLink = true;
                        }
                    }
                    MonsterInfo bodyinfo = Envir.GetMonsterInfo(Info.BodyName);
                    if (bodyinfo != null)
                    {
                        MonsterObject bodymob;
                        Point spawnlocation = Point.Empty;
                        for (int i = 0; i <= BodyLocations.Length - 1; i++)
                        {
                            bodymob = MonsterObject.GetMonster(bodyinfo);
                            spawnlocation = new Point(LinkedMonster.CurrentLocation.X + BodyLocations[i].X, LinkedMonster.CurrentLocation.Y + BodyLocations[i].Y);
                            if (bodymob != null) bodymob.Spawn(LinkedMonster.CurrentMap, spawnlocation);
                        }
                    }

                    DropArea = new Rectangle(Info.DropAreaTop.X, Info.DropAreaTop.Y, Info.DropAreaBottom.X - Info.DropAreaTop.X, Info.DropAreaBottom.Y - Info.DropAreaTop.Y);
                    Loaded = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                SMain.Enqueue(ex);
            }

            SMain.Enqueue("Failed to load Dragon");
            return false;
        }
        public void GainExp(int ammount)
        {
            if (ammount <= 0) return;

            Info.Experience += ammount;
            if (Info.Experience >= Info.Exps[Math.Min(11, Info.Level - 1)])
            {
                Info.Experience -= Info.Exps[Math.Min(11, Info.Level - 1)];
                LevelUp();
            }
        }
        public void LevelUp()
        {
            Drop(Info.Level);//i would suggest having the max level drop be empty or 'trash' > that way you stop ppl from exploiting it
            if (Info.Level < Globals.MaxDragonLevel) Info.Level = (byte)(Math.Max(1, (Info.Level + 1)));
            //if it reaches max level > make it stay that level for 6*deleveldelay and then reset to 0, rather then letting ppl farm it by making it drop every hour
            if (Info.Level == Globals.MaxDragonLevel)
                DeLevelTime = Envir.Time + (6 * DeLevelDelay);
        }
        public void LevelDown()
        {
            if (Info.Level > 1)
            {
                Info.Level = (byte)(Math.Max(1, (Info.Level - 1)));
                Info.Experience = 0;
            }
        }
        public void Drop(byte level)
        {
            if (level > Info.Drops.Length) return;
            if (Info.Drops[level - 1] == null) return;
            if (LinkedMonster == null) return;
            List<DragonInfo.DropInfo> droplist = new List<DragonInfo.DropInfo>(Info.Drops[level - 1]);

            for (int i = 0; i < droplist.Count; i++)
            {
                DragonInfo.DropInfo drop = droplist[i];

                int rate = (int)(drop.Chance / Settings.DropRate); if (rate < 1) rate = 1;
                if (Envir.Random.Next(rate) != 0) continue;

                if (drop.Gold > 0)
                {
                    int gold = Envir.Random.Next((int)(drop.Gold / 2), (int)(drop.Gold + drop.Gold / 2)); //Messy

                    if (gold <= 0) continue;

                    if (!DropGold((uint)gold)) return;
                }
                else
                {
                    UserItem item = Envir.CreateDropItem(drop.Item);
                    if (item == null) continue;
                    if (!DropItem(item)) return;
                }
            }
        }
        protected bool DropItem(UserItem item)
        {
            Point droplocation = new Point(DropArea.Left + (DropArea.Width / 2), DropArea.Top);
            ItemObject ob = new ItemObject(this.LinkedMonster, item, droplocation)
            {
                Owner = this.LinkedMonster.EXPOwner,
                OwnerTime = Envir.Time + Settings.Minute,
            };

            return ob.DragonDrop(DropArea.Width / 2);
        }

        protected bool DropGold(uint gold)
        {
            if (this.LinkedMonster.EXPOwner != null && this.LinkedMonster.EXPOwner.CanGainGold(gold))
            {
                this.LinkedMonster.EXPOwner.WinGold(gold);
                return true;
            }

            Point droplocation = new Point(DropArea.Left + (DropArea.Width / 2), DropArea.Top);
            ItemObject ob = new ItemObject(this.LinkedMonster, gold, droplocation)
            {
                Owner = this.LinkedMonster.EXPOwner,
                OwnerTime = Envir.Time + Settings.Minute,
            };

            return ob.DragonDrop(DropArea.Width / 2);
        }

        public void Process()
        {
            if (!Loaded) return;
            if (Envir.Time < ProcessTime) return;

            ProcessTime = Envir.Time + ProcessDelay;

            if ((Info.Level >= Globals.MaxDragonLevel) && (Envir.Time > DeLevelTime))
            {
                Info.Level = (byte)1;
                Info.Experience = 0;
                DeLevelTime = Envir.Time + DeLevelDelay;
            }

            if (Info.Level > 1 && Envir.Time > DeLevelTime)
            {
                LevelDown();
                DeLevelTime = Envir.Time + DeLevelDelay;
            }
        }
    }
}
