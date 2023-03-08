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

            if (Target == null || Envir.Time > ExplosionTime || !Target.IsAttackTarget(this))
            {
                Die(); return;
            }

            bool ranged = !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if(InAttackRange())
            {
                if (ranged)
                {
                    if(Envir.Random.Next(5) == 0)
                    {
                        Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                        Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                        ActionTime = Envir.Time + 300;
                        AttackTime = Envir.Time + AttackSpeed;

                        int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                        if (damage == 0) return;

                        int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                        DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + delay, Target, damage, DefenceType.ACAgility);
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
            List<MapObject> targets = FindAllTargets(1, CurrentLocation);

            for (int i = 0; i < targets.Count; i++)
            {
                ActionList.Add(new DelayedAction(DelayedType.Die, Envir.Time + 500, targets[i], GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]), DefenceType.ACAgility));
            }

            base.Die();
        }

        protected override void CompleteDeath(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            
            if (target.Attacked(this, damage, defence) <= 0) return;

            PoisonTarget(target, 5, 5, PoisonType.Green, 2000);
        }
    }
}
