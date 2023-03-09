using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using Shared.Extensions;

namespace Server.MirObjects.Monsters
{
    public class FloatingRock : MonsterObject
    {
        protected override bool CanMove { get { return false; } }

        protected internal FloatingRock(MonsterInfo info)
            : base(info)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        protected override void ProcessRoam() { }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }

        protected override void ProcessTarget()
        {
            if (!CanAttack) return;

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            var targets = FindAllNearby(Info.ViewRange, CurrentLocation, true);
            targets.Shuffle();

            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].Race != ObjectType.Monster) continue;

                var target = (MonsterObject)targets[i];

                if (target.Info.AI == Info.AI) continue;

                var mob = GetMonster(Envir.GetMonsterInfo(target.Info.Name));

                mob.PetLevel = target.PetLevel;
                mob.ActionTime = Envir.Time + 1000;

                if (Envir.Random.Next(3) == 0)
                {
                    var attempts = 4;
                    var distance = 5;

                    for (int j = 0; j < attempts; j++)
                    {
                        var location = new Point(CurrentLocation.X + Envir.Random.Next(-distance, distance + 1),
                                             CurrentLocation.Y + Envir.Random.Next(-distance, distance + 1));

                        if (location == CurrentLocation) continue;

                        if (mob.Spawn(CurrentMap, location))
                        {
                            Direction = Functions.DirectionFromPoint(CurrentLocation, target.CurrentLocation);

                            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = mob.ObjectID });

                            SlaveList.Add(mob);

                            break;
                        }
                    }
                }

                break;
            }
        }

        public override void Die()
        {
            //Kill Minions
            for (int i = SlaveList.Count - 1; i >= 0; i--)
            {
                if (!SlaveList[i].Dead && SlaveList[i].Node != null)
                {
                    SlaveList[i].Die();
                }
            }

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);

            List<MapObject> targets = FindAllTargets(3, CurrentLocation, false);

            for (int i = 0; i < targets.Count; i++)
            {
                DelayedAction action = new DelayedAction(DelayedType.Die, Envir.Time + 300, targets[i], damage, DefenceType.AC);
                ActionList.Add(action);
            }

            base.Die();
        }

        protected override void CompleteDeath(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            target.Attacked(this, damage, defence);
        }
    }
}
