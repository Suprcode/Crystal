using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class SpittingToad : MonsterObject
    {
        public bool Summoned;
        public long AliveTime;
        public long deadTime;

        protected internal SpittingToad(MonsterInfo info) : base(info)
        {
            ActionTime = Envir.Time + 1000;
        }
        public override string Name
        {
            get { return Master == null ? Info.GameName : (Dead ? Info.GameName : string.Format("{0}({1})", Info.GameName, Master.Name)); }
            set { throw new NotSupportedException(); }
        }

        public override void Process()
        {
            if (!Dead && Summoned)
            {
                if (Master != null)
                {
                    bool selfDestruct = false;
                    if (FindObject(Master.ObjectID, 15) == null) selfDestruct = true;
                    if (Summoned && Envir.Time > AliveTime) selfDestruct = true;
                    if (selfDestruct && Master != null) Die();
                }
            }
            base.Process();
        }

        public override void Process(DelayedAction action)
        {
            switch (action.Type)
            {
                case DelayedType.Damage:
                    CompleteAttack(action.Params);
                    break;
                case DelayedType.RangeDamage:
                    CompleteRangeAttack(action.Params);
                    break;
                case DelayedType.Recall:
                    PetRecall((MapObject)action.Params[0]);
                    break;
            }
        }

        public void PetRecall(MapObject target)
        {
            if (target == null) return;
            if (Master == null) return;
            Teleport(Master.CurrentMap, target.CurrentLocation);
        }

        protected override void ProcessAI()
        {
            //ProcessSearch
            if (Envir.Time < SearchTime) return;
            SearchTime = Envir.Time + SearchDelay;

            if (Target == null) FindTarget();

            //ProcessTarget           
            if (Target == null || !CanAttack) return;

            if (InAttackRange())
            {
                Attack();
                if (Target.Dead) FindTarget();
                return;
            }
            else FindTarget();//Target out of range find new

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 12 || y > 12) return false;


            return (x <= 12 && y <= 12) || (x == y || x % 2 == y % 2);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Turn(Direction);
            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
            AttackTime = Envir.Time + AttackSpeed + 500;
            if (damage == 0) return;

            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + delay, Target, damage, DefenceType.MAC);
            ActionList.Add(action);

            if (Target.Dead)
                FindTarget();
        }

        public override void Spawned()
        {
            base.Spawned();
            Summoned = true;
        }

        public override Packet GetInfo()
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Location = CurrentLocation,
                Image = Monster.SpittingToad,
                Direction = Direction,
                Effect = Info.Effect,
                AI = Info.AI,
                Light = Info.Light,
                Dead = Dead,
                Skeleton = Harvested,
                Poison = CurrentPoison,
                Hidden = Hidden,
                Extra = Summoned,
            };
        }
    }
}
