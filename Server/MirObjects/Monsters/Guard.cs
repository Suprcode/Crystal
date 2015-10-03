using System;
using System.Drawing;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Guard : MonsterObject
    {
        public override bool Blocking
        {
            get { return true; }
        }

        protected override bool CanMove
        {
            get { return Route.Count > 0 && !Dead && Envir.Time > MoveTime && Envir.Time > ActionTime && Envir.Time > ShockTime; }
        }

        protected override bool CanRegen
        {
            get { return false; }
        }

        protected internal Guard(MonsterInfo info)
            : base(info)
        {
            NameColour = Color.SkyBlue;
        }
        public override void Spawned()
        {
            if (Respawn != null && Respawn.Info.Direction < 8)
                Direction = (MirDirection)Respawn.Info.Direction;
            
            base.Spawned();
        }



        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }

        protected override void ProcessRegen() { }
        protected override void ProcessRoam()
        {
            if (CanMove)
                base.ProcessRoam();
        }

        public override bool IsAttackTarget(PlayerObject attacker) { return false; }
        public override bool IsAttackTarget(MonsterObject attacker) { return false; }
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true) { throw new NotSupportedException(); }
        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility) { throw new NotSupportedException(); }
        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            return 0;
        }
        protected override bool DropItem(UserItem item) { throw new NotSupportedException(); }
        protected override bool DropGold(uint gold) { throw new NotSupportedException(); }
        public override void Die() { }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this)) return;

            Point target = Target.Back;
            MirDirection dir = Functions.DirectionFromPoint(target, Target.CurrentLocation);

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = dir, Location = target });
            Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);

            if (Target.Race != ObjectType.Player) damage = int.MaxValue;

            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.AC);
        }
    }
}
