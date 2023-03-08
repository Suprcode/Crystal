using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class BlockingObject : MonsterObject
    {
        public MonsterObject Parent;
        public bool Visible;

        protected internal BlockingObject(MonsterObject parent, MonsterInfo info) : base(info)
        {
            Parent = parent;
            Visible = true;
        }

        public override bool Blocking
        {
            get
            {
                return Parent.Blocking;
            }
        }

        public override bool Walk(MirDirection dir) { return false; }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return Parent.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(HumanObject attacker)
        {
            return Parent.IsAttackTarget(attacker);
        }

        protected override void ProcessRoam() { }

        protected override void ProcessSearch() { }

        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            return Parent.Attacked(attacker, damage, type, damageWeapon);
        }

        public override void Spawned()
        {
            base.Spawned();
        }

        public override Packet GetInfo()
        {
            if (!Visible) return null;

            return base.GetInfo();
        }

        public void Hide()
        {
            Visible = false;

            if (CurrentMap == null) return;

            CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation);
        }

        public void Show()
        {
            Visible = true;

            if (CurrentMap == null) return;

            Broadcast(GetInfo());
        }
    }
}
