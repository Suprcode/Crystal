using System;
using System.Collections.Generic;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ManectricClaw : MonsterObject
    {
        private const byte AttackRange = 3;
        private long _thrustTime;

        protected internal ManectricClaw(MonsterInfo info)
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

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if(ranged || Envir.Time > _thrustTime)
            {
                if (ranged && Envir.Random.Next(2) == 0)
                {
                    MoveTo(Target.CurrentLocation);
                    ActionTime = Envir.Time + 300;
                    return;
                }

                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                AttackTime = Envir.Time + AttackSpeed;

                DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500);
                ActionList.Add(action);

                _thrustTime = Envir.Time + 5000;

                return;
            }

            base.Attack();

            if (Target.Dead)
                FindTarget();
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            IceThrust();
        }

        private void IceThrust()
        {
            Point location = CurrentLocation;
            MirDirection direction = Direction;
            Cell cell;

            int nearDamage = GetAttackPower(MinDC, MaxDC);
            int farDamage = GetAttackPower(MinMC, MaxMC);

            int col = 3;
            int row = 3;

            Point[] loc = new Point[col]; //0 = left 1 = center 2 = right
            loc[0] = Functions.PointMove(location, Functions.PreviousDir(direction), 1);
            loc[1] = Functions.PointMove(location, direction, 1);
            loc[2] = Functions.PointMove(location, Functions.NextDir(direction), 1);

            for (int i = 0; i < col; i++)
            {
                Point startPoint = loc[i];
                for (int j = 0; j < row; j++)
                {
                    Point hitPoint = Functions.PointMove(startPoint, direction, j);

                    if (!CurrentMap.ValidPoint(hitPoint)) continue;

                    cell = CurrentMap.GetCell(hitPoint);

                    if (cell.Objects == null) continue;

                    for (int k = 0; k < cell.Objects.Count; k++)
                    {
                        MapObject target = cell.Objects[k];
                        switch (target.Race)
                        {
                            case ObjectType.Monster:
                            case ObjectType.Player:
                                if (target.IsAttackTarget(this))
                                {
                                    if (target.Attacked(this, j <= 1 ? nearDamage : farDamage, DefenceType.MAC) > 0)
                                    {
                                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= target.PoisonResist)
                                        {
                                            if (Envir.Random.Next(5) == 0)
                                            {
                                                target.ApplyPoison(new Poison
                                                {
                                                    Owner = this,
                                                    Duration = target.Race == ObjectType.Player ? 4 : 5 + Envir.Random.Next(5),
                                                    PType = PoisonType.Slow,
                                                    TickSpeed = 1000,
                                                }, this);
                                                target.OperateTime = 0;
                                            }

                                            if (Envir.Random.Next(5) == 0)
                                            {
                                                target.ApplyPoison(new Poison
                                                {
                                                    Owner = this,
                                                    Duration = target.Race == ObjectType.Player ? 2 : 5 + Envir.Random.Next(this.Freezing),
                                                    PType = PoisonType.Frozen,
                                                    TickSpeed = 1000,
                                                }, this);
                                                target.OperateTime = 0;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}