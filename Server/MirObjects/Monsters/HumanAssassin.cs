using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects.Monsters;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class HumanAssassin : MonsterObject
    {
        public byte AttackRange = 1;
        public int AttackDamage = 0;
        public bool Summoned;
        public long ExplosionTime;

        protected internal HumanAssassin(MonsterInfo info)
            : base(info)
        {
            ExplosionTime = Envir.Time + 1000 * 10;
            Summoned = true;
        }

        protected override void RefreshBase()
        {
            MaxHP = 1500;
            MinAC = Master.MinAC;
            MaxAC = Master.MaxAC;
            MinMAC = Master.MinMAC;
            MaxMAC = Master.MaxMAC;
            MinDC = Master.MinDC;
            MaxDC = Master.MaxDC;
            MinMC = Master.MinMC;
            MaxMC = Master.MaxMC;
            MinSC = Master.MinSC;
            MaxSC = Master.MaxSC;
            Accuracy = Master.Accuracy;
            Agility = Master.Agility;

            MoveSpeed = 100;
            AttackSpeed = Master.ASpeed;
        }

        public override void RefreshAll()
        {
            RefreshBase();

            MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + PetLevel * 20);
            MinAC = (ushort)Math.Min(ushort.MaxValue, MinAC + PetLevel * 2);
            MaxAC = (ushort)Math.Min(ushort.MaxValue, MaxAC + PetLevel * 2);
            MinMAC = (ushort)Math.Min(ushort.MaxValue, MinMAC + PetLevel * 2);
            MaxMAC = (ushort)Math.Min(ushort.MaxValue, MaxMAC + PetLevel * 2);
            MinDC = (ushort)Math.Min(ushort.MaxValue, MinDC + PetLevel);
            MaxDC = (ushort)Math.Min(ushort.MaxValue, MaxDC + PetLevel);

            if (MoveSpeed < 100) MoveSpeed = 100;
            if (AttackSpeed < 100) AttackSpeed = 100;

            RefreshBuffs();
        }

        public override bool Walk(MirDirection dir)
        {
            if (!CanMove) return false;

            Point location = Functions.PointMove(CurrentLocation, dir, 2);

            if (!CurrentMap.ValidPoint(location)) return false;

            Cell cell = CurrentMap.GetCell(location);

            bool isBreak = false;

            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (!ob.Blocking) continue;
                    isBreak = true;
                    break;
                }

            if (isBreak)
            {
                location = Functions.PointMove(CurrentLocation, dir, 1);

                if (!CurrentMap.ValidPoint(location)) return false;

                cell = CurrentMap.GetCell(location);

                if (cell.Objects != null)
                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if (!ob.Blocking) continue;
                        return false;
                    }
            }

            CurrentMap.GetCell(CurrentLocation).Remove(this);

            Direction = dir;
            RemoveObjects(dir, 1);
            CurrentLocation = location;
            CurrentMap.GetCell(CurrentLocation).Add(this);
            AddObjects(dir, 1);

            if (Hidden)
            {
                Hidden = false;

                for (int i = 0; i < Buffs.Count; i++)
                {
                    if (Buffs[i].Type != BuffType.Hiding) continue;

                    Buffs[i].ExpireTime = 0;
                    break;
                }
            }


            CellTime = Envir.Time + 500;
            ActionTime = Envir.Time + 300;
            MoveTime = Envir.Time + MoveSpeed;
            if (MoveTime > AttackTime)
                AttackTime = MoveTime;

            InSafeZone = CurrentMap.GetSafeZone(CurrentLocation) != null;

            if (isBreak)
                Broadcast(new S.ObjectWalk { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            else
                Broadcast(new S.ObjectRun { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            cell = CurrentMap.GetCell(CurrentLocation);

            for (int i = 0; i < cell.Objects.Count; i++)
            {
                if (cell.Objects[i].Race != ObjectType.Spell) continue;
                SpellObject ob = (SpellObject)cell.Objects[i];

                ob.ProcessSpell(this);
                //break;
            }

            return true;
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void ProcessAI()
        {
            if (Dead) return;

            ProcessSearch();
            ProcessTarget();

            if (Master != null && Master is PlayerObject)
            {
                if (Envir.Time > ExplosionTime) Die();
            }
        }

        protected override void ProcessSearch()
        {
            if (Envir.Time < SearchTime) return;

            SearchTime = Envir.Time + SearchDelay;

            //Stacking or Infront of master - Move
            bool stacking = false;

            Cell cell = CurrentMap.GetCell(CurrentLocation);

            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (ob == this || !ob.Blocking) continue;
                    stacking = true;
                    break;
                }

            if (CanMove && stacking)
            {
                //Walk Randomly
                if (!Walk(Direction))
                {
                    MirDirection dir = Direction;

                    switch (Envir.Random.Next(3)) // favour Clockwise
                    {
                        case 0:
                            for (int i = 0; i < 7; i++)
                            {
                                dir = Functions.NextDir(dir);

                                if (Walk(dir))
                                    break;
                            }
                            break;
                        default:
                            for (int i = 0; i < 7; i++)
                            {
                                dir = Functions.PreviousDir(dir);

                                if (Walk(dir))
                                    break;
                            }
                            break;
                    }
                }
            }

            if (Target == null || Envir.Random.Next(3) == 0)
                FindTarget();
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (InAttackRange())
            {
                Attack();
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            int dist = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);

            if (dist >= AttackRange)
                MoveTo(Target.CurrentLocation);
        }

        protected override void Attack()
        {
            if (AttackDamage >= 500) Die();

            ShockTime = 0;

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }


            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            AttackDamage += damage;

            if (damage == 0) return;

            Target.Attacked(this, damage);
        }

        public override void Spawned()
        {
            base.Spawned();

            Summoned = false;
        }

        public override void Die()
        {
            if (Dead) return;

            explosionDie();

            HP = 0;
            Dead = true;

            //DeadTime = Envir.Time + DeadDelay;
            DeadTime = 0;

            Broadcast(new S.ObjectDied { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = (byte)2 });

            if (EXPOwner != null && Master == null && EXPOwner.Race == ObjectType.Player) EXPOwner.WinExp(Experience);

            if (Respawn != null)
                Respawn.Count--;

            PoisonList.Clear();
            Envir.MonsterCount--;
            CurrentMap.MonsterCount--;
        }

        private void explosionDie()
        {
            int criticalDamage = Envir.Random.Next(0, 100) <= Accuracy ? MaxDC * 2 : MinDC * 2;
            int damage = (MinDC / 5 + 4 * (Level / 20)) * criticalDamage / 20 + MaxDC;

            for (int i = 0; i < 16; i++)
            {
                MirDirection dir = (MirDirection)(i % 8);
                Point hitPoint = Functions.PointMove(CurrentLocation, dir, (i / 8 + 1));

                if (!CurrentMap.ValidPoint(hitPoint)) continue;

                Cell cell = CurrentMap.GetCell(hitPoint);

                if (cell.Objects == null) continue;


                for (int j = 0; j < cell.Objects.Count; j++)
                {
                    MapObject target = cell.Objects[j];
                    switch (target.Race)
                    {
                        case ObjectType.Monster:
                        case ObjectType.Player:
                            //Only targets
                            if (target.IsAttackTarget((PlayerObject)Master))
                            {
                                target.Attacked((PlayerObject)Master, damage, DefenceType.AC, false);
                            }
                            break;
                    }
                }
            }
        }

        public override Packet GetInfo()
        {
            PlayerObject master = null;
            short weapon = -1;
            short armour = 0;
            byte wing = 0;
            if (Master != null && Master is PlayerObject) master = (PlayerObject)Master;
            if (master != null)
            {
                weapon = master.Looks_Weapon;
                armour = master.Looks_Armour;
                wing = master.Looks_Wings;
            }
            return new S.ObjectPlayer
            {
                ObjectID = ObjectID,
                Name = master != null ? master.Name : Name,
                NameColour = NameColour,
                Class = master != null ? master.Class : MirClass.Assassin,
                Gender = master != null ? master.Gender : MirGender.Male,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = master != null ? master.Hair : (byte)0,
                Weapon = weapon,
                Armour = armour,
                Light = master != null ? master.Light : Light,
                Poison = CurrentPoison,
                Dead = Dead,
                Hidden = Hidden,
                Effect = SpellEffect.None,
                WingEffect = wing,
                Extra = false,
                TransformType = -1
            };
        }
    }
}