using Server.MirDatabase;
using System.Drawing;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ThunderElement : MonsterObject
    {
        protected internal ThunderElement(MonsterInfo info)
            : base(info)
        {
        }

        protected override void CompleteAttack(IList<object> data)
        {
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            List<MapObject> targets = FindAllTargets(2, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                MapObject target = targets[i];

                if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

                target.Attacked(this, damage, defence);
            }

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                if (Envir.Random.Next(3) == 1)
                {
                    int targetXOffset, targetYOffset;
                    targetXOffset = Envir.Random.Next(-1, 2);
                    targetYOffset = Envir.Random.Next(-1, 2);
                    Point point = new Point(Target.CurrentLocation.X + targetXOffset, Target.CurrentLocation.Y + targetYOffset);
                    MoveTo(point);
                }
                Attack();
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.MAC);
            ActionList.Add(action);
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            if (type != DefenceType.Repulsion) return 0;

            return base.Attacked(attacker, damage, type);
        }
        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            if (type != DefenceType.Repulsion) return 0;

            return base.Attacked(attacker, damage, type, damageWeapon);
        }
        public override int Pushed(MapObject pusher, MirDirection dir, int distance)
        {
            int result = base.Pushed(pusher, dir, distance);

            if (result > 0)
            {
                int damage;
                damage = distance * (Math.Max(50, (Envir.Random.Next(Stats[Stat.HP]) / 5)));
                if (pusher is PlayerObject)
                {
                    //int damage = Math.Max(50, Envir.Random.Next(Stats[Stat.HP]));
                    Attacked((PlayerObject)pusher, damage, DefenceType.Repulsion);
                }
                else if (pusher is MonsterObject) Attacked((MonsterObject)pusher, damage, DefenceType.Repulsion);
            }
            return result;
        }

        public override void PoisonDamage(int amount, MapObject Attacker)
        {
            return;
        }
    }
}
