using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class PeacockSpider : MonsterObject
    {
        private long _PoisonTime;
        private long _PoisonRainTime;

        protected internal PeacockSpider(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 3);

            if (Envir.Time > _PoisonRainTime)
            {
                //TODO - Animation broken as too large

                //Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
                //int damage = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC] * 2);
                //if (damage == 0) return;

                //DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, true);
                //ActionList.Add(action);

                _PoisonRainTime = Envir.Time + 30000;
                //return;
            }

            if (!ranged && Envir.Random.Next(4) > 0)
            {
                if (Envir.Time > _PoisonTime) //Poison
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MACAgility, true, false);
                    ActionList.Add(action);

                    _PoisonTime = Envir.Time + 20000;

                    return;
                }

                if (Envir.Random.Next(3) > 0) //Normal
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false, false);
                    ActionList.Add(action);
                }
                else //Front Stomp
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                    var damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    TriangleAttack(damage, 3, 2, 500, DefenceType.ACAgility, false);
                }
            }
            else
            {
                if (Envir.Random.Next(5) == 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) return;

                    ProjectileAttack(damage, DefenceType.MACAgility);
                }
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool poisonCloud = data.Count >= 4 && (bool)data[3];
            bool daze = data.Count < 5 || (bool)data[4];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (poisonCloud)
            {
                var targets = FindAllTargets(3, CurrentLocation, false);

                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].Attacked(this, damage, defence) <= 0) continue;
                    PoisonTarget(targets[i], 2, Envir.Random.Next(2, 6), PoisonType.Green, 1000);
                }

                return;
            }
            if (daze)
            {
                if (target.Attacked(this, damage, defence) <= 0) return;
                PoisonTarget(target, 2, Envir.Random.Next(2, 6), PoisonType.Dazed, 1000);
                return;
            }

            target.Attacked(this, damage, defence);
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (target.Attacked(this, damage, defence) <= 0) return;

            PoisonTarget(target, 4, 5, PoisonType.Paralysis, 1000);
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
