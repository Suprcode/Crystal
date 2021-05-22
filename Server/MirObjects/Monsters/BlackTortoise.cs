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
    class BlackTortoise : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 7;
            }
        }

        protected internal BlackTortoise(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && CanFly(Target.CurrentLocation) && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;

            if (!ranged)
            {
                if (Envir.Random.Next(5) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 400, Target, damage, DefenceType.ACAgility);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    HalfmoonAttack(); //Halfmoon Attack  
                }
            }
            else
            {
                //TODO - ADD A SOURCE TO EFFECT FOR CLIENT SO ANIMATION CAN START AT CORRECT PLACE.
                RangeAttack(Stats[Stat.MinMC], Stats[Stat.MaxMC], 0);  
                PoisonTarget(7, 5, PoisonType.Green, 1000);
            }

            if (Target.Dead)
                FindTarget();
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);

        }
    }

}

