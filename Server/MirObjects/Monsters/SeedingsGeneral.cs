using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class SeedingsGeneral : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 2;
            }
        }

        protected internal SeedingsGeneral(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
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

            if (!ranged && Envir.Random.Next(5) > 0)
            {
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                if (Envir.Random.Next(4) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                    //Blood Attack
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                    //Green Splash Attack
                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.MACAgility);
                    ActionList.Add(action);
                }
            }
            else
            {
                ActionTime = Envir.Time + 600;
                AttackTime = Envir.Time + AttackSpeed;

                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                
                if (Envir.Random.Next(5) > 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });
                    if (damage == 0) return;

                    //Echo Shout
                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 300, Target, damage, DefenceType.MACAgility, false);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
                    if (damage == 0) return;

                    //Stomp
                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 300, Target, damage, DefenceType.MACAgility, true);
                    ActionList.Add(action);
                }
            }
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool aoe = (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (aoe)
            {
                var targets = FindAllTargets(2, CurrentLocation, false);

                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].Attacked(this, damage, defence) <= 0) continue;

                    PoisonTarget(targets[i], 5, 5, PoisonType.Frozen, 1000);
                }
            }
            else
            {
                if (target.Attacked(this, damage, defence) <= 0) return;

                PoisonTarget(target, 5, 5, PoisonType.Slow, 1000);
            }
        }
    }
}

