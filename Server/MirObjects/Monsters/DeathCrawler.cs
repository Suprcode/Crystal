using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            ActionList.Add(new DelayedAction(DelayedType.Die, Envir.Time + 500));
            base.Die();
        }

        protected override void CompleteDeath(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation, false);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                if (Envir.Random.Next(Settings.PoisonResistWeight) >= targets[i].PoisonResist)
                {
                    if (Envir.Random.Next(5) == 0)
                    {
                        targets[i].ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
                    }
                }
            }

        }
    }
}