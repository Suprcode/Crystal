using Server.MirDatabase;

namespace Server.MirObjects.Monsters
{
    public class RevivingZombie : MonsterObject
    {
        public byte RevivalCount;
        public int LifeCount;
        public long RevivalTime, DieTime;

        public override uint Experience
        {
            get { return (uint)(Info.Experience * (100 - (25 * RevivalCount)) / 100); }
        }

        protected internal RevivingZombie(MonsterInfo info)
            : base(info)
        {
            RevivalCount = 0;
            LifeCount = Envir.Random.Next(3);
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

                int newhp = Stats[Stat.HP] * (100 - (25 * RevivalCount)) / 100;
                Revive(newhp, false);
            }

            base.ProcessAI();
        }
    }
}