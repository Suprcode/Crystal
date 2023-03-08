using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class RhinoPriest : AxeSkeleton
    {
        protected internal RhinoPriest(MonsterInfo info)
            : base(info)
        {
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
            bool range = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (!range)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 600, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action);
            }
            else
            {
                AttackTime = Envir.Time + AttackSpeed + 500;

                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;

                if (Envir.Random.Next(3) > 0)
                {
                    //Rhino Ranged Attack
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 600, Target, damage, DefenceType.MACAgility, false);
                    ActionList.Add(action);
                }
                else
                {
                    //Blue Circle Attack
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.MAC, true);
                    ActionList.Add(action);
                }
            }
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool poison = (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (target.Attacked(this, damage, defence) <= 0) return;

            if (poison)
            {
                if (Envir.Random.Next(4) > 0)
                {
                    PoisonTarget(target, 2, 5, PoisonType.Slow, 1000);
                }
                else
                {
                    PoisonTarget(target, 4, 5, PoisonType.Frozen, 1000);
                }
            }
            else
            {
                var stats = new Stats
                {
                    [Stat.MaxDC] = damage * -1,
                    [Stat.MaxMC] = damage * -1,
                    [Stat.MaxSC] = damage * -1
                };

                target.AddBuff(BuffType.RhinoPriestDebuff, this, Settings.Second * (5 + damage), stats);
            }
        }
    }
}
