using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class AssassinBird : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 6;
            }
        }

        protected internal AssassinBird(MonsterInfo info)
            : base(info)
        {
        }

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

            if (!ranged)
            {
                if (Envir.Random.Next(8) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                    CompleteAttack();
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });                    
                    PushAttack();
                    CompleteAttack();
                }
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });                
                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MACAgility);
                ActionList.Add(action);
            }

            if (Target.Dead)
                FindTarget();
        }

        private void PushAttack()
        {   
            int levelgap = 5;
            int moblevel = this.Level;
            int targetlevel = Target.Level;

            if ((targetlevel <= moblevel + levelgap))
            {
                Target.Pushed(this, Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation), 3);
            }
        }

        private void CompleteAttack()
        {
            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
            ActionList.Add(action);
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
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
    }
}
