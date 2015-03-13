using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class HedgeKekTal : RightGuard
    {
        protected internal HedgeKekTal(MonsterInfo info)
            : base(info)
        {
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (!ranged)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                if (damage == 0) return;

                Target.Attacked(this, damage, DefenceType.ACAgility);
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                AttackTime = Envir.Time + AttackSpeed + 500;
                if (damage == 0) return;

                int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.AC);
                ActionList.Add(action);
            }


            if (Target.Dead)
                FindTarget();

        }
    }
}
