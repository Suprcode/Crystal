using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;

namespace Server.MirObjects.Monsters
{
    public class WoomaTaurus : FlamingWooma
    {
        private long _teleTime, _madTime;
        private const long TeleDelay = 10000;
        private byte _stage = 7;

        protected internal WoomaTaurus(MonsterInfo info)
            : base(info)
        {
        }

        protected override void ProcessAI()
        {
            if (Dead) return;

            if (_madTime > 0 && Envir.Time > _madTime)
            {
                _madTime = 0;
                RefreshAll();
            }

            if (Envir.Time > _teleTime)
            {
                _teleTime = Envir.Time + TeleDelay;

                int count = 0;
                MirDirection dir = Direction;

                for (int i = 0; i < 8; i++)
                {
                    Point location = Functions.PointMove(CurrentLocation, dir, 1);

                    if (CurrentMap.ValidPoint(location))
                    {
                        Cell cell = CurrentMap.GetCell(location);

                        if (cell.Objects == null) continue;

                        for (int o = 0; o < cell.Objects.Count; o++)
                        {
                            if (!cell.Objects[o].Blocking) continue;
                            count++;
                            break;
                        }
                    }
                    else count++;

                    dir = Functions.NextDir(dir);
                }

                if (count >= 5)
                {
                    Target = null;
                    TeleportRandom(4, 0);
                }
            }


            if (MaxHP >= 7)
            {
                byte stage = (byte)(HP / (MaxHP / 7));

                if (stage < _stage)
                {
                    _madTime = Envir.Time + 8000;
                    MoveSpeed = 400;
                    AttackSpeed = 500;
                }
                _stage = stage;
            }


            base.ProcessAI();
        }
    }
}
