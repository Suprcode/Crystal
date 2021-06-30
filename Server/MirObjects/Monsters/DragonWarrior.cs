using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class DragonWarrior : MonsterObject
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

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    HalfmoonAttack(damage);
                }
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                //Shield Bash Attack

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);

                SinglePushAttack(damage);

                //TODO - Delay
                PoisonTarget(Target, 3, 5, PoisonType.Dazed);
            }
        }
    }
}

