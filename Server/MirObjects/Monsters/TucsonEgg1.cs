using System;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class TucsonEgg1 : MonsterObject
    {
        protected override bool CanMove { get { return false; } }


        protected internal TucsonEgg1(MonsterInfo info)
            : base(info)
        {
        }

        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return base.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            return base.IsAttackTarget(attacker);
        }

        protected override void ProcessRoam() { }

        protected override void ProcessSearch()
        {
            base.ProcessSearch();
        }

        public override Packet GetInfo()
        {
            return base.GetInfo();
        }

        protected override void ProcessTarget()
        {
            if (Target == null)
                return;

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

        }

        private void SpawnSlave()
        {
            int count = Math.Min(1, 1 - SlaveList.Count);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            MonsterObject mob = null;
            mob = GetMonster(Envir.GetMonsterInfo(Settings.TucsonGeneralEgg));

            if (mob == null) return;
            if (!mob.Spawn(CurrentMap, Front))
                mob.Spawn(CurrentMap, CurrentLocation);

            mob.Target = Target;
            mob.ActionTime = Envir.Time + 2000;
            SlaveList.Add(mob);
        }

        protected override void CompleteAttack(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;

                if (targets[i].Attacked(this, damage, DefenceType.MAC) <= 0) return;

                if (Envir.Random.Next(3) == 0)
                    targets[i].ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
            }

        }

        public override void Die()

        {
            ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + 300));
            SpawnSlave();
            base.Die();
        }

    }
}
