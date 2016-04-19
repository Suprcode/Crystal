using System.Collections.Generic;
using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class OmaKing : MonsterObject
    {
        public byte AttackRange = 7;

        protected internal OmaKing(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 2 || y > 2) return false;


            return (x <= 1 && y <= 1) || (x == y || x % 2 == y % 2);
        }

        protected bool InRangedAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void CompleteAttack(IList<object> data)
        {
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            List<MapObject> targets = FindAllTargets(AttackRange, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];

                if (Target == null || !Target.IsAttackTarget(this) || Target.CurrentMap != CurrentMap || Target.Node == null) continue;

                Target.Attacked(this, damage, defence);
            }

        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !InAttackRange();

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage;

            if (!ranged && Envir.Random.Next(3) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;

                List<MapObject> targets = FindAllTargets(1, CurrentLocation);
                if (targets.Count == 0) return;

                int levelgap;

                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].IsAttackTarget(this))
                    {
                        levelgap = 60 - targets[i].Level;
                        if (Envir.Random.Next(20) < 4 + levelgap)
                        {
                            if (Envir.Random.Next(Settings.MagicResistWeight) < targets[i].MagicResist) continue;
                            if (targets[i].Pushed(this, Functions.DirectionFromPoint(CurrentLocation, targets[i].CurrentLocation), 3 + Envir.Random.Next(3)) > 0
                            && Envir.Random.Next(8) == 0)
                            {
                                if (Envir.Random.Next(Settings.PoisonResistWeight) >= targets[i].PoisonResist)
                                {
                                    targets[i].ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, this, true);
                                }
                            }
                        }
                    }
                }

                LineAttack(2);
            }
            else
            {
                AttackTime = Envir.Time + AttackSpeed + 500;
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                damage = GetAttackPower(MinMC, MaxMC);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                ActionList.Add(action);
            }

            if (Target.Dead)
                FindTarget();

        }

        private void LineAttack(int distance)
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (Target != null && target == Target.CurrentLocation)
                {
                    Target.Attacked(this, damage, DefenceType.ACAgility);
                }
                else
                {
                    if (!CurrentMap.ValidPoint(target)) continue;

                    Cell cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                        {
                            if (!ob.IsAttackTarget(this)) continue;

                            ob.Attacked(this, damage, DefenceType.ACAgility);
                        }
                        else continue;

                        break;
                    }

                }
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InRangedAttackRange() && CanAttack)
            {
                Attack();
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }
    }
}
