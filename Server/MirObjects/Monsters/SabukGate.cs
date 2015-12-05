using System;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;
using System.Drawing;

namespace Server.MirObjects.Monsters
{
    public class SabukGate : CastleGate
    {
        protected internal SabukGate(MonsterInfo info) : base(info)
        {
            BlockArray = new Point[] 
            {
                new Point(0, -1),
                new Point(0, -2),
                new Point(1, -1),
                new Point(1, -2),
                new Point(-1, 0),
                new Point(-2, 0),
                new Point(-1, -1),
                new Point(-1, 1)
            };

            Direction = MirDirection.Up;
        }
        
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            MirDirection newDirection = (MirDirection)(3 - GetDamageLevel());

            if (newDirection != Direction)
            {
                Direction = newDirection;
                Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            }

            return base.Attacked(attacker, damage, type, damageWeapon);
        }

        public override void OpenDoor()
        {
            Direction = (MirDirection)6;
            Closed = false;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

            ActiveDoorWall(false);
        }

        public override void CloseDoor()
        {
            Direction = (MirDirection)(3 - GetDamageLevel());

            Closed = true;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

            ActiveDoorWall(true);
        }

        public override void RepairWall(int level)
        {

        }

        protected override int GetDamageLevel()
        {
            int level = (int)Math.Round((double)(3 * HP) / MaxHP);

            if (level < 1) level = 1;

            return level;
        }
    }
}
