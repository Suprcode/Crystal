using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class SepHighArcher : MonsterObject
    {
        public long FearTime, DecreaseMPTime;
        public byte AttackRange = 6;
        public bool Summoned;

        protected internal SepHighArcher(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
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

            int distance = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);
            int delay = distance * 50 + 500; //50 MS per Step


            if (Envir.Random.Next(3) == 0 && Functions.InRange(CurrentLocation, Target.CurrentLocation, 2))
            {
                Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.BackStep, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });
                int travel = 0;
                bool blocked = false;
                int jumpDistance = 3;
                MirDirection jumpDir = Functions.ReverseDirection(Direction);
                Point location = CurrentLocation;
                for (int i = 0; i < jumpDistance; i++)
                {
                    location = Functions.PointMove(location, jumpDir, 1);
                    if (!CurrentMap.ValidPoint(location)) break;

                    Cell cInfo = CurrentMap.GetCell(location);
                    if (cInfo.Objects != null)
                        for (int c = 0; c < cInfo.Objects.Count; c++)
                        {
                            MapObject ob = cInfo.Objects[c];
                            if (!ob.Blocking) continue;
                            blocked = true;
                            if ((cInfo.Objects == null) || blocked) break;
                        }
                    if (blocked) break;
                    travel++;
                }

                jumpDistance = travel;
                if (jumpDistance > 0)
                {
                    for (int i = 0; i < jumpDistance; i++)
                    {
                        location = Functions.PointMove(CurrentLocation, jumpDir, 1);
                        CurrentMap.GetCell(CurrentLocation).Remove(this);
                        RemoveObjects(jumpDir, 1);
                        CurrentLocation = location;
                        CurrentMap.GetCell(CurrentLocation).Add(this);
                        AddObjects(jumpDir, 1);
                    }
                   
                    Broadcast(new S.ObjectBackStep { ObjectID = ObjectID, Direction = Direction, Location = location, Distance = jumpDistance });
                }
                return;
            }

            bool hasPoisonBuff = (Buffs.Where(x => x.Type == BuffType.PoisonShot).ToList().Count() > 0);


            if (hasPoisonBuff && Envir.Random.Next(2) == 0)
            {
                Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.CrippleShot, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });

                PoisonTarget(Target, 5, 8, PoisonType.Green, 2000);

                if (hasPoisonBuff)
                {
                    Point place = Target.CurrentLocation;
                    for (int y = place.Y - 1; y <= place.Y + 1; y++)
                    {
                        if (y < 0) continue;
                        if (y >= CurrentMap.Height) break;
                        for (int x = place.X - 1; x <= place.X + 1; x++)
                        {
                            if (x < 0) continue;
                            if (x >= CurrentMap.Width) break;
                            Cell cell = CurrentMap.GetCell(x, y);
                            if (!cell.Valid || cell.Objects == null) continue;
                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject targetob = cell.Objects[i];
                                if (targetob.Race != ObjectType.Monster && targetob.Race != ObjectType.Player && targetob.Race != ObjectType.Hero) continue;
                                if (targetob == null || !targetob.IsAttackTarget(this) || targetob.Node == null) continue;
                                if (targetob.Dead) continue;

                                targetob.Attacked(this, damage, DefenceType.MAC);
                                targetob.ApplyPoison(new Poison
                                {
                                    Duration = (Envir.Random.Next(1, 3) + 1) * 7,
                                    Owner = this,
                                    PType = PoisonType.Green,
                                    TickSpeed = 2000,
                                    Value = damage / 25 + 4,
                                }, this);
                                targetob.OperateTime = 0;
                                RemoveBuff(BuffType.PoisonShot);
                            }
                        }
                    }
                }
                return;
            }


            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.PoisonShot, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });
            if (Envir.Random.Next(10) <= 4)
            {
                if (!hasPoisonBuff)
                {
                    Target.AddBuff(BuffType.PoisonShot, Target, Settings.Second * 10, new Stats());
                }
            }

            action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.ACAgility);
            ActionList.Add(action);

            if (Target.Dead)
                FindTarget();
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (!InAttackRange())
            {
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

            if (InAttackRange() && Envir.Time < FearTime)
            {
                Attack();
                return;
            }

            FearTime = Envir.Time + 5000;

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            int dist = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);

            if (dist < AttackRange)
            {
                MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

                if (Walk(dir)) return;

                switch (Envir.Random.Next(2)) //No favour
                {
                    case 0:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.NextDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                    default:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.PreviousDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                }
            }
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
                Class = MirClass.Archer,
                Gender = master != null ? master.Gender : Envir.Random.Next(2) == 0 ? MirGender.Male : MirGender.Female,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = master != null ? master.Hair : (byte)Envir.Random.Next(0, 5),
                Weapon = 218,
                Armour = 41,
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
