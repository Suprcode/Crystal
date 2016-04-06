using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class CaveMaggot : HarvestMonster
    {
        protected internal CaveMaggot(MonsterInfo info)
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
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            if (Target.Attacked(this, damage, DefenceType.MACAgility) <= 0) return;

            if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
            {
                if (Envir.Random.Next(20) == 0)
                {
                    Target.ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, this);
                }
            }
        }
    }
}
