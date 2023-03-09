using Server.MirDatabase;
using Server.MirEnvir;

namespace Server.MirObjects.Monsters
{
    public class BoulderSpirit : MonsterObject
    {
        protected override bool CanMove { get { return false; } }
        protected override bool CanAttack { get { return false; } }
        protected override bool CanRegen { get { return false; } }

        protected internal BoulderSpirit(MonsterInfo info)
            : base(info)
        {
        }
        public override bool Walk(MirDirection dir)
        {
            return false;
        }

        protected override void ProcessAI()
        {
            if (Dead) return;

            var targets = FindAllTargets(Info.ViewRange, CurrentLocation, true);

            if (targets.Count > 0)
            {
                Die();
            }
        }

        public override void Die()
        {
            ActionList.Add(new DelayedAction(DelayedType.Die, Envir.Time + 300));
            base.Die();
        }

        protected override void CompleteDeath(IList<object> data)
        {
            var targets = FindAllTargets(Info.ViewRange, CurrentLocation, false);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                if (targets[i].Attacked(this, damage, DefenceType.ACAgility) <= 0) continue;
            }
        }
    }
}
