using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class WhiteMammoth : MonsterObject
    {
        protected internal WhiteMammoth(MonsterInfo info)
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

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Random.Next(8) > 0)
            {
                if (Envir.Random.Next(6) > 0)
                {
                    base.Attack();
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC] * 2);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility, false);
                    ActionList.Add(action);
                }       
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });

                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility, true);
                ActionList.Add(action);
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

        }

        protected override void CompleteAttack(IList<object> data)
        {
            int damage = (int)data[1];

            if (data.Count >= 4)
            {
                bool stomp = (bool)data[3];

                if (stomp)
                {
                    List<MapObject> targets = FindAllTargets(1, CurrentLocation, false);

                    for (int i = 0; i < targets.Count; i++)
                    {
                        var target = targets[i];

                        if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) continue;

                        if (target.Attacked(this, damage) <= 0) continue;

                        PoisonTarget(target, 0, 5, PoisonType.Dazed, 2000);
                    }

                    return;
                }
            }

            base.CompleteAttack(data);
        }
    }
}
