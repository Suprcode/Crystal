using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class GlacierSnail : MonsterObject
    {
        protected internal GlacierSnail(MonsterInfo info)
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            if (Envir.Random.Next(5) != 0)
            {
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action);
            }
            else
            {
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                DelayedAction action1 = new DelayedAction(DelayedType.Damage, Envir.Time + 350, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action1);
                DelayedAction action2 = new DelayedAction(DelayedType.Damage, Envir.Time + 550, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action2);
                DelayedAction action3 = new DelayedAction(DelayedType.Damage, Envir.Time + 750, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action3);
            }
        }        
    }
}
