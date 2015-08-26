using Server.MirDatabase;
using System;
using System.Drawing;

namespace Server.MirObjects.Monsters
{
    public class Football : MonsterObject
    {
        protected override bool CanAttack { get { return false; }}

        private int _ballMoveDistance = 4;

        protected internal Football(MonsterInfo info)
            : base(info)
        {

        }

        protected override void FindTarget() { }

        protected override void ProcessTarget() { }

        public override bool IsAttackTarget(MonsterObject attacker) { return false; }

        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            int currentMoveDistance = 0;

            Point target = Functions.PointMove(CurrentLocation, attacker.Direction, _ballMoveDistance);

            MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, target);

            while (currentMoveDistance < _ballMoveDistance)
            {
                Point location = Functions.PointMove(CurrentLocation, dir, 1);

                if (location.X < 0 || location.Y < 0 || location.X >= CurrentMap.Width || location.Y >= CurrentMap.Height) break;

                currentMoveDistance++;

                if (!CurrentMap.GetCell(location).Valid)
                {
                    dir = Functions.ReverseDirection(dir);
                    continue;
                }

                Walk(dir);
                MoveTime = 0;
                ActionTime = 0;
            }

            return 0;
        }
        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility) { throw new NotSupportedException(); }

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            return 0;
        }

        public override void Die() { }
    }
}
