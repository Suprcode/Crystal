using System;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;
using System.Drawing;

namespace Server.MirObjects.Monsters
{
    public class Gate : CastleGate
    {
        protected internal Gate(MonsterInfo info) : base(info)
        {
            switch (info.Effect)
            {
                case 1: //Sabuk Door
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
                    break;
                case 2: //West Pointing Castle Gi
                    BlockArray = new Point[]
                    {
                        new Point(0, -1),
                        new Point(-1, -1),
                        new Point(-1, 0),
                        new Point(0, 1),
                        new Point(1, 1),
                        new Point(1, 0),
                        new Point(1, -1),
                        new Point(0, -2),
                    };
                    break;
                case 3: // South Pointing Castle Gi
                    {
                        BlockArray = new Point[]
                        {
                        new Point(1, -1),
                        new Point(0, -1),
                        new Point(0, -2),
                        new Point(-1, -1),
                        new Point(-2, -1),
                        new Point(-1, 0),
                        new Point(-1, 1),
                        new Point(-2, 0),
                        };
                    }
                    break;
                case 4: // East Pointing Castle Gi
                    {
                        BlockArray = new Point[]
                        {
                        new Point(-2, 0),
                        new Point(-1, 1),
                        new Point(0, 2),
                        new Point(0, 1),
                        new Point(-1, 0),
                        new Point(-1, -1),
                        new Point(1, 1),
                        };

                    }
                    break;
            }
           
            Direction = MirDirection.Up;
        }
        public override void Despawn()
        {
            base.Despawn();
        }
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            CheckDirection();

            if (!Conquest.WarIsOn || attacker.MyGuild != null && Conquest.Owner == attacker.MyGuild.Guildindex) damage = 0;
             
            return base.Attacked(attacker, damage, type, damageWeapon);
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            CheckDirection();

            if (!Conquest.WarIsOn) damage = 0;

            return base.Attacked(attacker, damage, type);
        }

        public override void OpenDoor()
        {
            if (!Closed) return;

            Direction = (MirDirection)6;
            Closed = false;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

            ActiveDoorWall(false);
        }

        public void CheckDirection()
        {
            MirDirection newDirection = (MirDirection)(3 - GetDamageLevel());

            if (newDirection != Direction)
            {
                Direction = newDirection;
                Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            }
        }

        public override void CloseDoor()
        {

            if (Closed) return;

            Direction = (MirDirection)(3 - GetDamageLevel());

            Closed = true;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

            ActiveDoorWall(true);
        }

        public override void RepairGate()
        {
            if (HP == 0)
                Revive(MaxHP, false);
            else
                SetHP(MaxHP);

            CheckDirection();
        }

        protected override int GetDamageLevel()
        {
            int level = (int)Math.Round((double)(3 * HP) / MaxHP);

            if (level < 1) level = 1;

            return level;
        }

        protected override bool CanRegen
        {
            get
            {
                return false;
            }
        }
    }
}
