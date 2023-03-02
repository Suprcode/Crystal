using System.Collections.Generic;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class SepWizard : MonsterObject
    {
        public long FearTime, DecreaseMPTime, RepulsionTime;
        public byte AttackRange = 5;
        public bool Summoned;

        protected internal SepWizard(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            DelayedAction action;

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

            List<MapObject> targets = FindAllTargets(1, CurrentLocation, false);
            if (targets.Count > 0 && Envir.Time > RepulsionTime)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].IsAttackTarget(this) && targets[i].Level < Level)
                        targets[i].Pushed(this, Functions.DirectionFromPoint(targets[i].CurrentLocation, targets[i].Back), 4);
                }
                RepulsionTime = Envir.Time + Settings.Second * Envir.Random.Next(10, 30);
                Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.Repulsion, Cast = true, Level = 3 });
                return;
            }


            if (Envir.Random.Next(3) == 0)
            {

                Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.FireBang, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });
                ProjectileAttack(damage);
                return;
            };


            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.GreatFireBall, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });
            action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MACAgility);
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

            //DeadTime = Envir.Time + DeadDelay;
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
                Class = MirClass.Wizard,
                Gender = master != null ? master.Gender : Envir.Random.Next(2) == 0 ? MirGender.Male : MirGender.Female,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = master != null ? master.Hair : (byte)Envir.Random.Next(0, 5),
                Weapon = 22,
                Armour = 7,
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
