using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class IncarnatedZT : ZumaMonster
    {
        protected internal IncarnatedZT(MonsterInfo info) : base(info)
        {
            AvoidFireWall = false;
            Stoned = false;
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
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            if (Target.Attacked(this, damage, DefenceType.MACAgility) <= 0) return;

            if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
            {
                if (Envir.Random.Next(12) == 0)
                    Target.ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, this);
            }
        }
    }

}
