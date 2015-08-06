using Server.MirDatabase;

namespace Server.MirObjects.Monsters
{
    public class Football : MonsterObject
    {
        protected override bool CanAttack { get { return false; }}

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
            //move opposite direction 2 squares
            return base.Attacked(attacker, damage, type, damageWeapon);
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            return base.Attacked(attacker, damage, type);
        }
    }
}
