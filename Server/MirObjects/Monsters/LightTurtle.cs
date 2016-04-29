using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class LightTurtle : MonsterObject
    {
        protected internal LightTurtle(MonsterInfo info)
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

            ShockTime = 0;

            if (Envir.Random.Next(3) > 0)
            {
                base.Attack();
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;

                Target.Attacked(this, damage, DefenceType.ACAgility);

                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    if (Envir.Random.Next(4) == 0)
                        Target.ApplyPoison(new Poison { Owner = this, PType = PoisonType.Green, Duration = GetAttackPower(MinSC,MaxSC), TickSpeed = 1000 }, this);
                }
            }
        }
    }
}
