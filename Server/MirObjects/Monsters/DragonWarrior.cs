using System.Collections.Generic;
using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Linq;
using System.Text;

namespace Server.MirObjects.Monsters
{
    class DragonWarrior : MonsterObject
    {
        protected internal DragonWarrior(MonsterInfo info)
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

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Random.Next(5) > 0)
            {
                if (Envir.Random.Next(3) > 0)
                {
                    base.Attack();        
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    HalfmoonAttack();
                }
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                //Shield Bash Attack
                SinglePushAttack(Stats[Stat.MinDC],Stats[Stat.MaxDC]);
                PoisonTarget(Target, 3, 5, PoisonType.Dazed);
            }

            if (Target.Dead)
                FindTarget();
        }
    }
}

