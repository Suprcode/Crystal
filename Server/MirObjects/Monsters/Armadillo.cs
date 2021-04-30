using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    class Armadillo : DigOutZombie
    {
        //TODO: Code Attack3 - rolling attack(disengage?)

        protected internal Armadillo(MonsterInfo info)
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (Envir.Random.Next(6) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                LineAttack(1);
            }

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.ACAgility);

            if (Target.Dead)
                FindTarget();

        }

        private void LineAttack(int distance)
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < 4; i++)
            {
                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;
            }

        }
    }
}
