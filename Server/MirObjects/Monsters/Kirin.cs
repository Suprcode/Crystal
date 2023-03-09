using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class Kirin : MonsterObject
    {

        private byte AttackRange = 3;
        protected internal Kirin(MonsterInfo info) : base(info)
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


        protected bool InRangeAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void ProcessTarget()
        {
            if (Target == null || Dead) return;

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

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            //MoveTo(Target.CurrentLocation);
        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            ShockTime = 0;
            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            if (Envir.Random.Next(5) == 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.AC);
                ActionList.Add(action);
            }
            else
            {
                base.Attack();
            }

            if (Target.Dead)
                FindTarget();
        }

        public void RangeAttack()
        {

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            ShockTime = 0;
            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            if (damage == 0) return;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });

            IceThrust();
        }

        private void IceThrust()
        {
            Point location = CurrentLocation;
            MirDirection direction = Direction;
            Cell cell;

            int Damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);

            int col = 3;
            int row = 3;

            Point[] loc = new Point[col];
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
                                    if (target.Attacked(this, Damage, DefenceType.MAC) > 0)
                                    {
                                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= target.Stats[Stat.PoisonResist])
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