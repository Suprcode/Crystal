using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Mantis : MonsterObject
    {
        protected internal Mantis(MonsterInfo info)
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

            if (Envir.Random.Next(5) > 0)
            {
                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.ACAgility);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.ACAgility);

                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    if (Envir.Random.Next(8) == 0)
                        Target.ApplyPoison(new Poison { Owner = this, Duration = 6, PType = PoisonType.Stun, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
                }
            }


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;


        }

        
    }
}
