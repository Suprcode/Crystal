using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class SepHighAssassin : MonsterObject
    {
        public long FearTime, DecreaseMPTime;
        public byte AttackRange = 3;
        public bool Summoned;

        protected internal SepHighAssassin(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (!InAttackRange())
            {
                if (CanAttack)
                {
                    if (Envir.Random.Next(5) == 0)
                        RangeAttack();
                }

                if (CurrentLocation == Target.CurrentLocation)
                {
                    MirDirection direction = (MirDirection)Envir.Random.Next(8);
                    int rotation = Envir.Random.Next(2) == 0 ? 1 : -1;

                    for (int d = 0; d < 8; d++)
                    {
                        if (Walk(direction)) break;

                        direction = Functions.ShiftDirection(direction, rotation);
                    }
                }
                else
                    MoveTo(Target.CurrentLocation);
            }

            if (!CanAttack) return;

            if (Envir.Random.Next(5) > 0)
            {
                if (InAttackRange())
                    Attack();
            }
            else RangeAttack();
        }


        public bool Walk(MirDirection dir, bool br = false)
        {
            if (!CanMove) return false;

            var temploc = Functions.PointMove(CurrentLocation, dir, 1);

            if (!CurrentMap.ValidPoint(temploc)) return false;

            var cell = CurrentMap.GetCell(temploc);

            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (!ob.Blocking) continue;
                    return false;
                }



            Point location = Functions.PointMove(CurrentLocation, dir, 2);

            if (!CurrentMap.ValidPoint(location)) return false;

            cell = CurrentMap.GetCell(location);

            bool isBreak = br;



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
        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            DelayedAction action;
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Random.Next(5) == 0)
            {
                Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.CrescentSlash, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });
                MirDirection backDir = Functions.ReverseDirection(Direction);
                MirDirection preBackDir = Functions.PreviousDir(backDir);
                MirDirection nextBackDir = Functions.NextDir(backDir);

                for (int i = 0; i < 8; i++)
                {
                    MirDirection dir = (MirDirection)i;

                    Point hitPoint = Functions.PointMove(CurrentLocation, dir, 1);

                    if (dir != backDir && dir != preBackDir && dir != nextBackDir)
                    {

                        for (int k = 0; k < 2; k++)
                        {
                            hitPoint = Functions.PointMove(hitPoint, dir, k);

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
                                    case ObjectType.Hero:

                                        if (target.IsAttackTarget(this))
                                        {
                                            WideLineAttack(damage, 4, 500, DefenceType.ACAgility, false, 4);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                return;
            }

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.DoubleSlash });
            Target.Attacked(this, damage);
            action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, (int)(damage * 0.8f), DefenceType.ACAgility, false);
            ActionList.Add(action);
        }

        public void RangeAttack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.HeavenlySword, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });
            LineAttack(damage, AttackRange);

        }
        public override void Die()
        {
            if (Dead) return;

            HP = 0;
            Dead = true;

            DeadTime = 0;

            Broadcast(new S.ObjectDied { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = (byte)(Master != null ? 1 : 0) });

            if (EXPOwner != null && Master == null && EXPOwner.Race == ObjectType.Player) EXPOwner.WinExp(Experience, Level);

            if (Respawn != null)
                Respawn.Count--;

            if (Master == null)
                Drop();

            Master = null;

            PoisonList.Clear();
            Envir.MonsterCount--;

            if (CurrentMap != null)
                CurrentMap.MonsterCount--;
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
                Class = MirClass.Assassin,
                Gender = master != null ? master.Gender : Envir.Random.Next(2) == 0 ? MirGender.Male : MirGender.Female,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = master != null ? master.Hair : (byte)Envir.Random.Next(0, 5),
                Weapon = 113,
                Armour = 34,
                Light = master != null ? master.Light : Light,
                Poison = CurrentPoison,
                Dead = Dead,
                Hidden = Hidden,
                Effect = SpellEffect.None,
                WingEffect = wing,
                Extra = Summoned,
                TransformType = -1,

            };
        }
    }
}
