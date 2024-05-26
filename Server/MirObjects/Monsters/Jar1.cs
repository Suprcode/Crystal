using Server.Library.MirDatabase;
using Shared.Functions;

namespace Server.Library.MirObjects.Monsters {
    public class Jar1 : MonsterObject {
        protected virtual byte AttackRange => 1;

        protected override bool CanMove => false;
        protected override bool CanRegen => false;

        protected override bool InAttackRange() {
            return CurrentMap == Target.CurrentMap &&
                   Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected internal Jar1(MonsterInfo info)
            : base(info) { }

        public override void Die() {
            ActionList.Add(new DelayedAction(DelayedType.Die, Envir.Time + 1000));

            base.Die();
        }

        protected override void CompleteDeath(IList<object> data) {
            SpawnSlave();
        }

        private void SpawnSlave() {
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            IEnumerable<MonsterInfo> monsters =
                Envir.MonsterInfoList.Where(x => x.Level <= Level && x.Level >= Level - 10);

            if(monsters.Count() > 0) {
                int idx = Envir.Random.Next(monsters.Count());

                MonsterInfo monster = monsters.ElementAt(idx);

                MonsterObject mob = GetMonster(monster);

                if(mob == null) {
                    return;
                }

                mob.Spawn(CurrentMap, CurrentLocation);

                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
            }
        }
    }
}
