using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class FeederAnt : MonsterObject
    {
        public long FearTime;
        public byte HealRange = 6;

        protected internal FeederAnt(MonsterInfo info) : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, HealRange);
        }

        protected virtual void HealTarget()
        {
            if (!Target.IsAttackTarget(this) || Target.Race == ObjectType.Player)
            {
                Target = null;
                return;
            }
            ShockTime = 0;

            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (ranged)
            {
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed + 300;


                Target.HealAmount += MaxSC;
                Broadcast(new S.ObjectEffect { ObjectID = Target.ObjectID, Effect = SpellEffect.Healing });
                SearchTarget();
            }

            if (Target.Dead || Target.PercentHealth == 100 || (Target.Race == ObjectType.Player && Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) > 3))
                SearchTarget();
        }

        protected virtual void HealMyself()
        {
            HealAmount += MaxSC;
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Location = CurrentLocation });
            Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.Healing });
            SearchTarget();
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack || Target.PercentHealth == 100) return;

            if (PercentHealth != 100 && Envir.Time < FearTime)
            {
                HealMyself();
                return;
            }
            if (InAttackRange() && Envir.Time < FearTime && Target.Race != ObjectType.Player)
            {
                HealTarget();
                return;
            }

            FearTime = Envir.Time + 5000;

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            if (Target.Race == ObjectType.Player)
            {
                int dist = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);

                if (dist < 3)
                {
                    MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

                    if (Walk(dir)) return;

                    switch (Envir.Random.Next(2)) //No favour
                    {
                        case 0:
                            for (int i = 0; i < 4; i++)
                            {
                                dir = Functions.NextDir(dir);

                                if (Walk(dir))
                                    return;
                            }
                            break;
                        default:
                            for (int i = 0; i < 4; i++)
                            {
                                dir = Functions.PreviousDir(dir);

                                if (Walk(dir))
                                    return;
                            }
                            break;
                    }
                }
                SearchTarget();
            }
        }

        protected virtual void SearchTarget()
        {
            var near = FindAllNearby(HealRange, CurrentLocation);
            for (int i = 0; i < near.Count; i++)
            {
                if (near[i].Race == ObjectType.Player)
                    Target = (PlayerObject)near[i];
                else
                    Target = (MonsterObject)near[i];
            }
        }
    }
}