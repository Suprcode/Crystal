using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;
namespace Server.MirObjects.Monsters
{
    class FloatingRock : MonsterObject
    {
        protected override bool CanMove { get { return false; } }

        protected virtual byte AttackRange
        {
            get
            {
                return 7;
            }
        }

        protected internal FloatingRock(MonsterInfo info)
            : base(info)
        {
        }

        //public override void Turn(MirDirection dir)
        //{
        //}
        public override bool Walk(MirDirection dir) { return false; }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

            int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            if (damage == 0) return;
            DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 1000, Target, damage, DefenceType.MAC);
            ActionList.Add(action);

            if (Target.Dead)
                FindTarget();
        }

        protected override void ProcessRoam() { }
    }
}
