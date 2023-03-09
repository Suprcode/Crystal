using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class IceCrystalSoldier : MonsterObject
    {
        private long _areaTime;
        protected internal IceCrystalSoldier(MonsterInfo info)
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

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Time < _areaTime)
            {
                if (Envir.Random.Next(4) == 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage * 3 / 2, DefenceType.ACAgility);
                    ActionList.Add(action);
                }
                else
                {
                    base.Attack();
                }
                return;
            }

            _areaTime = Envir.Time + 2000 + Envir.Random.Next(5) * 1000;

            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            ActionList.Add(new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500));

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]) * 2;
            if (damage == 0) return;

            List<MapObject> targets = FindAllTargets(1, Functions.PointMove(CurrentLocation, Direction, 1));
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
                targets[i].Attacked(this, damage, DefenceType.MAC);
        }

    }
}