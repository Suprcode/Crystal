using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class ToxicGhoul : HarvestMonster
    {
        protected internal ToxicGhoul(MonsterInfo info)
            : base(info)
        {
        }

        protected override void Attack()
        {
            ShockTime = 0;

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }


            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            if (Target.Attacked(this, damage, DefenceType.MACAgility) > 0 && Envir.Random.Next(8) == 0)
            {
                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    int poison = GetAttackPower(MinSC, MaxSC);

                    Target.ApplyPoison(new Poison
                    {
                        Owner = this,
                        Duration = 5,
                        PType = PoisonType.Green,
                        Value = poison,
                        TickSpeed = 2000
                    }, this);
                }
            }
        }

        public override void Die()
        {
            if (Info.Effect == 1)
            {
                ActionList.Add(new DelayedAction(DelayedType.Die, Envir.Time + 1000));
            }

            base.Die();
        }

        protected override void CompleteDeath(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation, false);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;

                if (targets[i].Attacked(this, damage, DefenceType.ACAgility) <= 0) return;

                if (Envir.Random.Next(Settings.PoisonResistWeight) >= targets[i].PoisonResist)
                {
                    if (Envir.Random.Next(5) == 0)
                        targets[i].ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
                }
            }
        }

    }
}
