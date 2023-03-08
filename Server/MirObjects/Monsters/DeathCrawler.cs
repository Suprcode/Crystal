using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class DeathCrawler : MonsterObject
    {
        protected internal DeathCrawler(MonsterInfo info)
            : base(info)
        {
        }

        public override void Die()
        {
            base.Die();

            ActionList.Add(new DelayedAction(DelayedType.Die, Envir.Time + 500));
        }

        public override void ApplyNegativeEffects(HumanObject attacker, DefenceType type, ushort levelOffset)
        {
            base.ApplyNegativeEffects(attacker, type, levelOffset);

            if (Envir.Random.Next(3) == 0)
            {
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DeathCrawlerBreath });

                PoisonTarget(attacker, 5, 5, PoisonType.Green, 2000);
            }
        }

        protected override void CompleteDeath(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation, false);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                PoisonTarget(targets[i], 5, 5, PoisonType.Green, 2000);
            }
        }
    }
}