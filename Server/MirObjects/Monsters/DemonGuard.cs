using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    class DemonGuard : ZumaMonster
    {
        public uint RevivalCount;
        public int LifeCount;
        public long RevivalTime, DieTime;

        public override uint Experience
        {
            get { return Info.Experience * (100 - (25 * RevivalCount)) / 100; }
        }

        protected internal DemonGuard(MonsterInfo info)
            : base(info)
        {
            RevivalCount = 0;
            LifeCount = Envir.Random.Next(3);
        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Random.Next(3) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.ACAgility);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                int damage = GetAttackPower(MinDC, MaxDC * 2);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.ACAgility);

            }

        }

        public override void Die()
        {
            DieTime = Envir.Time;
            RevivalTime = (4 + Envir.Random.Next(20)) * 1000;
            base.Die();
        }


        protected override void ProcessAI()
        {
            if (Dead && Envir.Time > DieTime + RevivalTime && RevivalCount < LifeCount)
            {
                RevivalCount++;

                uint newhp = MaxHP * (100 - (25 * RevivalCount)) / 100;
                Revive(newhp, false);
            }

            base.ProcessAI();
        }

    }
}
