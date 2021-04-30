using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class OmaMage : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 8;
            }
        }

        protected internal OmaMage(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && CanFly(Target.CurrentLocation) && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!ranged)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.ACAgility);
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed + 500;
                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.MACAgility);

                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
                {
                    if (Envir.Random.Next(4) == 0)
                        Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Slow, Value = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]), TickSpeed = 1000 }, this);
                }
                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
                {
                    if (Envir.Random.Next(6) == 0)
                        Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Frozen, Value = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]), TickSpeed = 1000 }, this);
                }

                DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                ActionList.Add(action);
            }

            if (Target.Dead)
                FindTarget();
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }
    }
}
