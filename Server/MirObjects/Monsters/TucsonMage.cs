using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class TucsonMage : MonsterObject
    {
        protected internal TucsonMage(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 3 || y > 3) return false;

            return (x <= 3 && y <= 3) || (x == y || x % 3 == y % 3);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            bool range = !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!range && Envir.Random.Next(3) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;
 
                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                WideLineAttack();
            }

            ShockTime = 0;
        }

        private void WideLineAttack()
        {
            int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            if (damage == 0) return;

            var forward = Functions.PointMove(CurrentLocation, Direction, 1);

            Cell cell = CurrentMap.GetCell(forward);
            if (cell.Objects != null)
            {
                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject ob = cell.Objects[o];
                    if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                    {
                        if (!ob.IsAttackTarget(this)) continue;

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, ob, damage, DefenceType.ACAgility);
                        ActionList.Add(action);
                    }
                    else continue;

                    break;
                }
            }

            var direction = Functions.PreviousDir(Direction);

            for (int d = 0; d < 3; d++)
            {
                for (int i = 1; i <= 2; i++)
                {
                    Point target = Functions.PointMove(forward, direction, i);

                    if (!CurrentMap.ValidPoint(target)) continue;

                    cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                        {
                            if (!ob.IsAttackTarget(this)) continue;

                            int delay = Functions.MaxDistance(CurrentLocation, ob.CurrentLocation) * 50 + 500; //50 MS per Step
                            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, ob, damage, DefenceType.ACAgility);
                            ActionList.Add(action);
                        }
                        else continue;

                        break;
                    }
                }

                direction = Functions.NextDir(direction);
            }
        }
    }
}
