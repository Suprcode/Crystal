using System;
using System.Drawing;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class Khazard : MonsterObject
    {
        public long PullTime;
        public bool Range;

        protected internal Khazard(MonsterInfo info)
            : base(info)
        {
            PullTime = Envir.Time;
        }

        protected bool CanPull
        {
            get
            {
                return Range && Envir.Time >= PullTime;
            }
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 4 || y > 4) return false;

            bool targetinrange = (x == 0) || (y == 0) || (x == y);
            if (!targetinrange) return false;

            Range = (x > 1 || y > 1) ? true : false;

            return (!Range || CanPull) ? true : false;
        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Range && CanPull)
            {
                PullAttack();
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;
                PullTime = Envir.Time + 5000;
            }
            else
            {
                if (!Range) base.Attack();
            }
        }

        private void PullAttack()
        {
            for (int i = 1; i <= 4; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (target == Target.CurrentLocation)
                {
                    if (Envir.Random.Next(Settings.MagicResistWeight) < Target.MagicResist) continue;
                    MirDirection pushdir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);
                    Target.Pushed(this, pushdir, i);
                }
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                if (Target.Dead)
                    FindTarget();

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
