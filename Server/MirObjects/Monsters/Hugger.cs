using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Hugger : MonsterObject
    {
        public long ExplosionTime;

        protected internal Hugger(MonsterInfo info)
            : base(info)
        {
            ExplosionTime = Envir.Time + 1000 * 60 * 5;
        }

        protected override void ProcessTarget()
        {
            if (!CanAttack) return;

            if (Target == null)
            {
                Die(); return;
            }

            if (Envir.Time > ExplosionTime)
            {
                Die(); return;
            }

            if (InAttackRange())
            {
                Attack();

                if (Target.Dead)
                    Die();

                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }

        protected override void CompleteAttack(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation, false);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;

                if (targets[i].Attacked(this, damage, DefenceType.MAC) <= 0) return;

                if (Envir.Random.Next(5) == 0)
                    targets[i].ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
            }
        }

        public override void Die()
        {
            ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + 500));
            base.Die();
        }
    }
}
