using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ToxicGhoul : HarvestMonster
    {
        protected internal ToxicGhoul(MonsterInfo info)
            : base(info)
        {
        }

        protected override void Attack()
        {
            ShockTime = 0;

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }


            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            if (Target.Attacked(this, damage, DefenceType.MACAgility) > 0 && Envir.Random.Next(8) == 0)
            {
                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    int poison = GetAttackPower(MinSC, MaxSC);

                    Target.ApplyPoison(new Poison
                    {
                        Owner = this,
                        Duration = 5,
                        PType = PoisonType.Green,
                        Value = poison,
                        TickSpeed = 2000
                    }, this);
                }
            }
        }
    }
}
