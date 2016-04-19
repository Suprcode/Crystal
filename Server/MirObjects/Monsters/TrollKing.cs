using Server.MirDatabase;
using System.Collections.Generic;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class TrollKing : MonsterObject
    {
        public long FearTime;
        public byte AttackRange = 7;

        protected internal TrollKing(MonsterInfo info)
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

            if (Functions.InRange(CurrentLocation, Target.CurrentLocation, 3))
            {
                if (Envir.Random.Next(2) == 0 || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 2))
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                    int damage = GetAttackPower(MinMC, MaxMC);
                    if (damage == 0) return;

                    List<MapObject> targets = FindAllTargets(3, CurrentLocation, false);

                    if (targets.Count == 0) return;

                    for (int i = 0; i < targets.Count; i++)
                    {
                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, targets[i], damage, DefenceType.MACAgility);
                        ActionList.Add(action);
                    }
                }
                else
                {
                    WalkAway();
                }
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });

                List<MapObject> targets = FindAllTargets(3, Target.CurrentLocation, false);

                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    int damage = GetAttackPower(MinDC, MaxDC);
                    if (damage == 0) continue;

                    int delay = Functions.MaxDistance(CurrentLocation, targets[i].CurrentLocation) * 50 + 500; //50 MS per Step

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + delay, targets[i], damage, DefenceType.ACAgility);
                    ActionList.Add(action);
                }
            }


            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + AttackSpeed;

            if (Target.Dead)
                FindTarget();
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            target.Attacked(this, damage, defence);
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            target.Attacked(this, damage, defence);

            if (target.Attacked(this, damage, DefenceType.MACAgility) > 0)
            {
                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    Target.ApplyPoison(new Poison
                    {
                        Owner = this,
                        Duration = Envir.Random.Next(MaxMC),
                        PType = PoisonType.Stun,
                        Value = damage,
                        TickSpeed = 1000
                    }, this);
                }
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (InAttackRange() && Envir.Time < FearTime)
            {
                Attack();
                return;
            }

            FearTime = Envir.Time + 5000;

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            int dist = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);

            if (dist >= AttackRange)
                MoveTo(Target.CurrentLocation);
            else
            {
                WalkAway();
            }
        }

        private void WalkAway()
        {
            if (Target == null || !CanAttack) return;

            MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

            if (Walk(dir)) return;

            switch (Envir.Random.Next(2)) //No favour
            {
                case 0:
                    for (int i = 0; i < 7; i++)
                    {
                        dir = Functions.NextDir(dir);

                        if (Walk(dir))
                            return;
                    }
                    break;
                default:
                    for (int i = 0; i < 7; i++)
                    {
                        dir = Functions.PreviousDir(dir);

                        if (Walk(dir))
                            return;
                    }
                    break;
            }
        }
    }
}
