using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

/* 
    AI Information:
    Attack 1 = Standard attack 
    Attack 2 = Double damage attack (DC)
    300ms attack delay (damage is registered when mob frames hit you, not at beginning of animation)
*/

namespace Server.MirObjects.Monsters
{
    public class BloodBaboon : MonsterObject
    {
        protected internal BloodBaboon(MonsterInfo info)
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

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool range = !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (Envir.Random.Next(5) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;                

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                LineAttack(2);
            }
        }

        private void LineAttack(int distance)
        {
            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC] * 2);
            if (damage == 0) return;            

            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //delay for animations         
            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.ACAgility);
            ActionList.Add(action);
        }

    }
}
