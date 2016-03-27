using Server.MirDatabase;
using System.Collections.Generic;
using System.Drawing;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Tornado : MonsterObject
    {
        private const byte AttackRange = 5;

        protected internal Tornado(MonsterInfo info)
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

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if(!ranged)
            {
                base.Attack();
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                List<MapObject> targets = FindAllTargets(AttackRange, CurrentLocation, false);

                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    MirDirection dir = Functions.DirectionFromPoint(targets[i].CurrentLocation, CurrentLocation);
                    int dist = Functions.MaxDistance(targets[i].CurrentLocation, CurrentLocation);

                    targets[i].Pushed(this, dir, dist - 1);
                }
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
        }
    }
}
