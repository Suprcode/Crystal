using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Wall : MonsterObject
    {
        public ConquestObject Conquest;
        public int WallIndex;

        protected internal Wall(MonsterInfo info) : base(info)
        {
            Direction = MirDirection.Up;
        }
        public override void Despawn()
        {
            base.Despawn();
        }
        protected override bool CanAttack
        {
            get
            {
                return false;
            }
        }
        protected override bool CanMove { get { return false; } }

        public override void Turn(MirDirection dir) { }

        public override bool Walk(MirDirection dir) { return false; }

        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            CheckDirection();

            if (!Conquest.WarIsOn || attacker.MyGuild != null && Conquest.GuildInfo.Owner == attacker.MyGuild.Guildindex) damage = 0;

            return base.Attacked(attacker, damage, type, damageWeapon);
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            CheckDirection();

            if (!Conquest.WarIsOn) damage = 0;

            return base.Attacked(attacker, damage, type);
        }

        public void RepairWall()
        {
            if (HP == 0)
                Revive(Stats[Stat.HP], false);
            else
                SetHP(Stats[Stat.HP]);

            CheckDirection();
        }

        public void CheckDirection()
        {
            MirDirection newDirection = (MirDirection)(4 - GetDamageLevel());

            if (newDirection != Direction)
            {
                Direction = newDirection;
                Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            }
        }

        public override void Die()
        {
            base.Die();
        }

        protected int GetDamageLevel()
        {
            int level = (int)Math.Round((double)(4 * HP) / Stats[Stat.HP]);

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

