using Server.MirDatabase;

namespace Server.MirObjects.Monsters
{
    public class GlacierBeast : CrazyManworm
    {
        protected internal GlacierBeast(MonsterInfo info)
            : base(info)
        {
        }
        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            int attackType = (int)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (target.Attacked(this, damage, defence) <= 0) return;

            if (attackType == 1)
                PoisonTarget(target, 3, 5, PoisonType.Slow, 2000);
        }  
    }
}
