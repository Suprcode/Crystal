using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class DarkDevil : MonsterObject
    {
        private long _areaTime;

        protected internal DarkDevil(MonsterInfo info) : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            return Functions.InRange(CurrentLocation, Target.CurrentLocation, Envir.Time > _areaTime ? 3 : 1);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            if (Envir.Time < _areaTime)
            {
                base.Attack();
                return;
            }

            _areaTime = Envir.Time + 2000 + Envir.Random.Next(3)*1000;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            ActionList.Add(new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500));

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            int damage = GetAttackPower(MinDC, MaxDC) * 3;
            if (damage == 0) return;

            List<MapObject> targets = FindAllTargets(1, Functions.PointMove(CurrentLocation, Direction, 2));
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
                targets[i].Attacked(this, damage, DefenceType.MACAgility);
        }
    }
}
