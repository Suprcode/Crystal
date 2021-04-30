using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class CatWidow : MonsterObject
    {
        protected internal CatWidow(MonsterInfo info)
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

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (Envir.Random.Next(5) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.ACAgility);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC] * 2);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.ACAgility);
            }

            if (Target.Dead)
                FindTarget();
        }
    }
}
