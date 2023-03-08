using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class CrystalSpider : MonsterObject
    {
        protected internal CrystalSpider(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 3 || y > 3) return false;

            return (x == 0) || (y == 0) || (x == y);
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

            if (!ranged) base.Attack();
            else
            {
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;
                ShockTime = 0;

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                LineAttack(damage, 3, 300, DefenceType.MACAgility);
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool attack = data.Count < 4 || (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (!attack || target.Attacked(this, damage, defence) <= 0) return;

            PoisonTarget(target, 8, 5, PoisonType.Green, 2000);
        }

        protected override void LineAttack(int damage, int distance, int additionalDelay = 500, DefenceType defenceType = DefenceType.ACAgility, bool push = false)
        {
            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (!CurrentMap.ValidPoint(target)) continue;

                Cell cell = CurrentMap.GetCell(target);
                if (cell.Objects == null) continue;

                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject ob = cell.Objects[o];
                    if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                    {
                        if (!ob.IsAttackTarget(this)) continue;

                        if (Envir.Random.Next(Settings.MagicResistWeight) >= ob.Stats[Stat.MagicResist])
                        {
                            int delay = Functions.MaxDistance(CurrentLocation, ob.CurrentLocation) * 50 + additionalDelay; //50 MS per Step
                            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, ob, damage, defenceType, true);
                            ActionList.Add(action);
                        }
                    }
                    else continue;

                    break;
                }
            }
        }
    }
}
