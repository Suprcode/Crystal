using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class SnakeTotem : MonsterObject
    {
        public bool Summoned;
        public long AliveTime;
        public long DieTime;
        public int MaxMinions;

        protected internal SnakeTotem(MonsterInfo info) : base(info)
        {
            ActionTime = Envir.Time + 1000;
            Direction = MirDirection.Up;
        }

        public override string Name
        {
            get { return Master == null ? Info.GameName : (Dead ? Info.GameName : string.Format("{0}({1})", Info.GameName, Master.Name)); }
            set { throw new NotSupportedException(); }
        }

        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        protected override void ProcessRoam() { }

        public override void Process()
        {
            MaxMinions = PetLevel + 1;
            if (!Dead && Summoned)
            {
                bool selfDestruct = false;
                if (Master != null)
                {
                    if (Master.CurrentMap != CurrentMap || !Functions.InRange(Master.CurrentLocation, CurrentLocation, 15)) selfDestruct = true;
                    if (Summoned && Envir.Time > AliveTime) selfDestruct = true;
                    if (selfDestruct)
                    {
                        Die();
                        DieTime = Envir.Time + 3000;
                    }
                }
                base.Process();
            }
            else if (Envir.Time >= DieTime) Despawn();
        }

        public override void Process(DelayedAction action)
        {
            switch (action.Type)
            {
                case DelayedType.Damage:
                    CompleteAttack(action.Params);
                    break;
            }
        }

        protected override void ProcessAI()
        {
            if (Dead) return;

            //Search for target
            if (Envir.Time < SearchTime) return;
            SearchTime = Envir.Time + SearchDelay;

            //Cant agro when shocked
            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }
            //update targets all the time ?
            AgroAllMobsInRange();

            //Refresh Minions
            for (int i = SlaveList.Count - 1; i >= 0; i--)
                if (SlaveList[i].Dead || SlaveList[i].Node == null)
                {
                    SlaveList[i].DeadTime = 0;
                }

            //Keep Minions Updated
            if (SlaveList.Count < MaxMinions) SpawnMinion();
        }

        public void AgroAllMobsInRange()
        {
            for (int d = 0; d <= Info.ViewRange; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (!cell.Valid || cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                    if (!ob.IsAttackTarget(this)) continue;
                                    if (ob.Hidden && (!CoolEye || Level < ob.Level)) continue;
                                    if (((MonsterObject)ob).Info.CoolEye == 100) continue;
                                    ob.Target = this;//Agro the mobs in range - Very simple agro system overwriting mobs target
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
        }

        public bool SpawnMinion()
        {
            if (Pets.Count >= MaxMinions) return false;

            MonsterInfo info = Envir.GetMonsterInfo(Settings.SnakesName);
            if (info == null) return false;

            MonsterObject monster;
            monster = MonsterObject.GetMonster(info);
            monster.PetLevel = PetLevel;
            monster.Master = this;
            monster.MaxPetLevel = (byte)(1 + PetLevel * 2);
            monster.Direction = Direction;
            monster.ActionTime = Envir.Time + 1000;

            ((Monsters.CharmedSnake)monster).AliveTime = Envir.Time + ((PetLevel * 2000) + 10000);
            ((Monsters.CharmedSnake)monster).MasterTotem = this;

            SlaveList.Add(monster);
            monster.Spawn(CurrentMap, CurrentLocation);

            return true;
        }

        public override void Die()
        {
            base.Die();

            DeadTime = 0;
            
            //Kill Minions
            for (int i = SlaveList.Count - 1; i >= 0; i--)
                if (!SlaveList[i].Dead && SlaveList[i].Node != null)
                {
                    SlaveList[i].Die();
                    SlaveList[i].DeadTime = 0;
                }
        }

        public override void Spawned()
        {
            base.Spawned();
            Summoned = true;
        }

        public override Packet GetInfo()
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Location = CurrentLocation,
                Image = Monster.SnakeTotem,
                Direction = Direction,
                Effect = Info.Effect,
                AI = Info.AI,
                Light = Info.Light,
                Dead = Dead,
                Skeleton = Harvested,
                Poison = CurrentPoison,
                Hidden = Hidden,
                Extra = Summoned,
            };
        }
    }
}
