using Server.MirDatabase;

namespace Server.MirObjects.Monsters
{
    public class Runaway : MonsterObject
    {
        private bool _nearEnemy;

        protected override bool CanAttack
        {
            get
            {
                return _nearEnemy;
            }
        }

        protected internal Runaway(MonsterInfo info) : base(info)
        {
        }

        protected override void FindTarget()
        {
            if (!_nearEnemy)
                base.FindTarget();
        }

        protected override void ProcessTarget()
        {
            if (!CanMove || Target == null) return;

            bool nearEnemy = Functions.InRange(Target.CurrentLocation, CurrentLocation, 1);

            if (nearEnemy)
            {
                _nearEnemy = true;
                base.ProcessTarget();
                return;
            }

            _nearEnemy = false;

            MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

            if (Walk(dir)) return;

            switch (Envir.Random.Next(2)) //No favour
            {
                case 0:
                    for (int i = 0; i < 7; i++)
                    {
                        dir = Functions.NextDir(dir);

                        if (Walk(dir))
                            return;
                    }
                    break;
                default:
                    for (int i = 0; i < 7; i++)
                    {
                        dir = Functions.PreviousDir(dir);

                        if (Walk(dir))
                            return;
                    }
                    break;
            }
        }
    }
}
