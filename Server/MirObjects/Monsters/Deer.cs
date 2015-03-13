using Server.MirDatabase;

namespace Server.MirObjects.Monsters
{
    public class Deer : HarvestMonster
    {
        private bool _runAway;

        protected override bool CanAttack
        {
            get
            {
                return base.CanAttack && !_runAway;
            }
        }

        protected internal Deer(MonsterInfo info)
            : base(info)
        {
            if (Info.AI != 2) return;

            RemainingSkinCount = 5;

            if (Envir.Random.Next(7) == 0)
            {
                _runAway = true;
                Quality = (short)(Envir.Random.Next(8) * 2000);
                MoveSpeed -= 300;
            }
            else
                Quality = (short)(Envir.Random.Next(4) * 1000);
        }

        public override void RefreshAll()
        {
            base.RefreshAll();

            if (_runAway && MoveSpeed >= 600) MoveSpeed -= 300;
        }
        protected override void FindTarget()
        {
            if (_runAway)
                base.FindTarget();
        }

        protected override void ProcessTarget()
        {
            if (_runAway)
            {
                if (!CanMove || Target == null) return;

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
            else base.ProcessTarget();
        }
    }
}
