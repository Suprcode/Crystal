using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;


namespace Server.MirObjects.Monsters
{
    public class TucsonWarrior : MonsterObject
    {
        protected internal TucsonWarrior(MonsterInfo info)
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

            if (!range && Envir.Random.Next(5) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                HalfmoonAttack(damage, 300);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                SmashAttack(1);
            }

            ShockTime = 0;
        }

        private void SmashAttack(int radius)
        {
            int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            if (damage == 0) return;

            var targets = FindAllTargets(radius, Target.CurrentLocation, false);

            for (int i = 0; i < targets.Count; i++)
            {
                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, targets[i], damage, DefenceType.ACAgility);
                ActionList.Add(action);
            }
        }
    }
}
