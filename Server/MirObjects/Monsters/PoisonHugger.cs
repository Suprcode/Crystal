using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class PoisonHugger : MonsterObject
    {
        public byte AttackRange = 5;
        public long ExplosionTime;

        protected internal PoisonHugger(MonsterInfo info)
            : base(info)
        {
            ExplosionTime = Envir.Time + 1000 * 60 * 5;
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
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

            if (!Target.IsAttackTarget(this))
            {
                Die(); return;
            }

            bool ranged = !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if(InAttackRange())
            {
                if(ranged)
                {
                    if(Envir.Random.Next(5) == 0)
                    {
                        Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                        Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                        ActionTime = Envir.Time + 300;
                        AttackTime = Envir.Time + AttackSpeed;

                        int damage = GetAttackPower(MinDC, MaxDC);
                        if (damage == 0) return;

                        int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.ACAgility, true);
                        ActionList.Add(action);

                        return;
                    }
                    else
                    {
                        MoveTo(Target.CurrentLocation);
                    }
                }
                else
                {
                    Die();
                }
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }

        public override void Die()
        {
            ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + 500, null, 0, 0, false));
            base.Die();
        }

        protected override void CompleteAttack(IList<object> data)
        {
            if((bool)data[3] == true)
            {
                MapObject target = (MapObject)data[0];
                if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

                if (target.Attacked(this, (int)data[1], (DefenceType)data[2]) <= 0) return;

                if (Envir.Random.Next(5) == 0)
                    target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
            }
            else
            {
                List<MapObject> targets = FindAllTargets(1, CurrentLocation);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    int damage = GetAttackPower(MinDC, MaxDC);
                    if (damage == 0) return;

                    if (targets[i] == null || !targets[i].IsAttackTarget(this) || targets[i].CurrentMap != CurrentMap || targets[i].Node == null) continue;

                    if (targets[i].Attacked(this, damage, DefenceType.MAC) <= 0) return;

                    if (Envir.Random.Next(5) == 0)
                        targets[i].ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
                }
            }
        }
    }
}
