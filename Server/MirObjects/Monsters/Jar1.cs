using Server.MirDatabase;
using Server.MirEnvir;

namespace Server.MirObjects.Monsters
{
    public class Jar1 : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 1;
            }
        }

        protected override bool CanMove { get { return false; } }
        protected override bool CanRegen { get { return false; } }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected internal Jar1(MonsterInfo info)
            : base(info)
        {

        }

        public override void Die()
        {
            ActionList.Add(new DelayedAction(DelayedType.Die, Envir.Time + 1000));

            base.Die();
        }

        protected override void CompleteDeath(IList<object> data)
        {
            SpawnSlave();
        }

        private void SpawnSlave()
        {
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            var monsters = Envir.MonsterInfoList.Where(x => x.Level <= Level && x.Level >= (Level - 10));

            if (monsters.Count() > 0)
            {
                var idx = Envir.Random.Next(monsters.Count());

                var monster = monsters.ElementAt(idx);

                var mob = GetMonster(monster);

                if (mob == null) return;

                mob.Spawn(CurrentMap, CurrentLocation);

                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
            }
        }
    }
}
