using System;
using System.Collections.Generic;
using System.Drawing;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class ThunderElement : MonsterObject
    {
        private MapObject OriginalTarget;

        protected internal ThunderElement(MonsterInfo info)
            : base(info)
        {
        }

        protected override void CompleteAttack(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(2, CurrentLocation);
            if (targets.Count == 0) return;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            OriginalTarget = Target;
            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];
                Attack();
            }
            Target = OriginalTarget;
        }
        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + 500));
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.MAC);
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            if (type != DefenceType.Repulsion) return 0;

            return base.Attacked(attacker, damage, type);
        }
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            if (type != DefenceType.Repulsion) return 0;

            return base.Attacked(attacker, damage, type, damageWeapon);
        }
        public override int Pushed(MapObject pusher, MirDirection dir, int distance)
        {
            int result = base.Pushed(pusher, dir, distance);

            if (result > 0)
            {
                if (pusher is PlayerObject) Attacked((PlayerObject)pusher, Math.Max(50, Envir.Random.Next((int)MaxHP)), DefenceType.Repulsion);
                else if (pusher is MonsterObject) Attacked((MonsterObject)pusher, Math.Max(50, Envir.Random.Next((int)MaxHP)), DefenceType.Repulsion);
            }
            return result;
        }

        public override void PoisonDamage(int amount, MapObject Attacker)
        {
            return;
        }
    }
}
