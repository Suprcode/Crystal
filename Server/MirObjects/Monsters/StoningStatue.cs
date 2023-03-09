using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class StoningStatue : MonsterObject
    {
        private long _areaTime = long.MaxValue;

        private const byte AttackRange = 2;

        protected internal StoningStatue(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 2 || y > 2) return false;

            return (x <= 1 && y <= 1) || (x == y || x % 2 == y % 2);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            if (_areaTime == long.MaxValue)
            {
                _areaTime = Envir.Time + 10000;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Time < _areaTime)
            {
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                LineAttack(damage, AttackRange);
            }
            else
            {
                _areaTime = Envir.Time + 5000 + Envir.Random.Next(10) * 1000;

                ActionTime = Envir.Time + 500;
                AttackTime = Envir.Time + (AttackSpeed * 2);

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                var damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;

                ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + 1600, Target, 0, DefenceType.MACAgility, true));
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool area = data.Count >= 4 && (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (!area)
            {
                target.Attacked(this, damage, defence);
            }
            else
            {
                List<MapObject> targets = FindAllTargets(2, CurrentLocation, false);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].Attacked(this, Stats[Stat.MaxMC], defence);

                    PoisonTarget(targets[i], 2, Envir.Random.Next(5, 10), PoisonType.Dazed, 1000);
                }
            }
        }
    }
}
