using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class HardenRhino : MonsterObject
    {
        protected internal HardenRhino(MonsterInfo info)
            : base(info)
        {

        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }

        protected override void ProcessSearch()
        {
            if (Envir.Time < SearchTime) return;
            if (Master != null && (Master.PMode == PetMode.MoveOnly || Master.PMode == PetMode.None || Master.PMode == PetMode.FocusMasterTarget)) return;

            SearchTime = Envir.Time + SearchDelay;

            if (Target == null || Envir.Random.Next(3) == 0)
                FindTarget();

            if (Target != null && !Functions.InRange(CurrentLocation, Target.CurrentLocation, 3) && Envir.Random.Next(3) == 0)
            {
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 1500, Target);
                ActionList.Add(action);

                ActionTime = Envir.Time + 1500;
                MoveTime = Envir.Time + 1500;
            }
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            ShockTime = 0;

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility);
            ActionList.Add(action);
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            Dash(target);

            MoveTo(target.CurrentLocation);
            ActionTime = Envir.Time + 300;
        }

        private void Dash(MapObject target)
        {
            var dist = Functions.MaxDistance(CurrentLocation, target.CurrentLocation);

            var travelled = 0;

            if (dist > 2)
            {
                var location = CurrentLocation;

                for (int i = 0; i < dist; i += 2)
                {
                    if (Functions.MaxDistance(CurrentLocation, target.CurrentLocation) <= 2) break;

                    Direction = Functions.DirectionFromPoint(CurrentLocation, target.CurrentLocation);

                    location = Functions.PointMove(location, Direction, 2);

                    if (!CurrentMap.ValidPoint(location)) break;

                    CurrentMap.GetCell(CurrentLocation).Remove(this);
                    RemoveObjects(Direction, 1);

                    CurrentLocation = location;
                    travelled++;

                    Broadcast(new S.ObjectRun { ObjectID = ObjectID, Direction = Direction, Location = location });

                    CurrentMap.GetCell(CurrentLocation).Add(this);
                    AddObjects(Direction, 1);
                }
            }
        }
    }
}
