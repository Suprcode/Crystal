using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class FurbolgGuard : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 8;
            }
        }

        protected internal FurbolgGuard(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }
            //else
            //{

            //    int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            //    int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            //    if (x > 2 || y > 2) return false;

            //    return (x <= 1 && y <= 1) || (x == y || x % 2 == y % 2);
            //}

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);


            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!ranged)
            {
                int dist = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);
                Point location = Functions.PointMove(CurrentLocation, Functions.ReverseDirection(Direction), 3);
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                if (dist <= 2 && CurrentMap.ValidPoint(location) && Envir.Random.Next(3) == 0)
                {
                    LineAttack(damage, 3, 500, DefenceType.ACAgility, true);
                    JumpBack(3);
                    return;
                }
                LineAttack(damage,3, 500);
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                ProjectileAttack(damage);
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
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
