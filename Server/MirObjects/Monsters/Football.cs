using Server.MirDatabase;
using System.Drawing;

namespace Server.MirObjects.Monsters
{
    public class Football : MonsterObject
    {
        protected override bool CanAttack { get { return false; }}

        private int _ballMoveDistance = 3;

        protected internal Football(MonsterInfo info)
            : base(info)
        {

        }

        protected override void FindTarget()
        {
            
        }

        protected override void ProcessTarget()
        {
            
        }

        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            Point target = Functions.PointMove(CurrentLocation, attacker.Direction, _ballMoveDistance);

            while (CurrentLocation != target)
            {
                MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, target);

                Point location = Functions.PointMove(CurrentLocation, dir, 1);

                if (location.X < 0 || location.Y < 0 || location.X >= CurrentMap.Width || location.Y >= CurrentMap.Height) break;

                if (!CurrentMap.GetCell(location).Valid) break;

                Walk(dir);
                MoveTime = 0;
                ActionTime = 0;
            }

            return 0;
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            return 0;
        }
    }
}
