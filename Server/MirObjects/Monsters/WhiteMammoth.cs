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
                    Target.Attacked(this, damage);
                }
                
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                Attack3();
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

        }
        private void Attack3()
        {
            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;
            Target.Attacked(this, damage);

            if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
            {
                Target.ApplyPoison(new Poison { Owner = this, Duration = 3, PType = PoisonType.Stun, TickSpeed = 2000 }, this);
            }
        }
    }
}
