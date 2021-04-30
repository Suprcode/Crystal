using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;


namespace Server.MirObjects.Monsters
{
    public class OmaBlest : MonsterObject
    {
        protected internal OmaBlest(MonsterInfo info)
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
            ShockTime = 0;

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
                AOEAttack(1);
            }
        }

        private void AOEAttack(int distance)
        {
            List<MapObject> targets = FindAllTargets(1, Target.CurrentLocation);
            if (targets.Count == 0) return;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];
                if (Target.IsAttackTarget(this))
                    targets[i].Attacked(this, damage, DefenceType.AC);
            }
        }

    }
}
