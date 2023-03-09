using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class AntCommander : MonsterObject
    {
        protected internal AntCommander(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && CanFly(Target.CurrentLocation) && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (!ranged)
            {
                switch (Envir.Random.Next(6))
                {
                    case 0:
                    case 3:
                    case 4:                    
                        {
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                            if (damage == 0) return;

                            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false);
                            ActionList.Add(action);
                        }
                        break;
                    case 1:
                    case 5:
                        {
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]) * 2;
                            if (damage == 0) return;

                            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 600, Target, damage, DefenceType.ACAgility, true);
                            ActionList.Add(action);
                        }
                        break;
                    case 2:
                        {
                            AttackTime = Envir.Time + AttackSpeed + 500;

                            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                            int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                            if (damage == 0) return;

                            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                            DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + delay, Target, damage, DefenceType.MACAgility, true);
                            ActionList.Add(action);
                        }
                        break;
                }
            }
            else
            {
                AttackTime = Envir.Time + AttackSpeed + 500;

                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;

                int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + delay, Target, damage, DefenceType.MACAgility, true);
                ActionList.Add(action);
            }
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool poison = (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            var finalDamage = target.Attacked(this, damage, defence);

            if (finalDamage > 0 && poison)
            {
                PoisonTarget(target, 5, 7, PoisonType.Green, 1000);
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool poison = (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (target.Attacked(this, damage, defence) <= 0) return;

            var finalDamage = target.Attacked(this, damage, defence);

            if (finalDamage > 0 && poison)
            {
                PoisonTarget(target, 4, 5, PoisonType.Slow, 1000);
                PoisonTarget(target, 2, 5, PoisonType.Dazed, 1000);
            }
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
