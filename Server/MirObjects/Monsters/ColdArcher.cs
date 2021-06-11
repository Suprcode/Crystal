using Server.MirDatabase;
using Server.MirEnvir;
using System;
using System.Collections.Generic;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ColdArcher : MonsterObject
    {
        public long BuffTime;

        protected internal ColdArcher(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }

        protected override void ProcessTarget()
        {
            if (Envir.Time > BuffTime)
            {
                var friends = FindAllFriends(Info.ViewRange, CurrentLocation);

                if (friends.Count > 0)
                {
                    var friend = friends[Envir.Random.Next(friends.Count)];

                    int delay = Functions.MaxDistance(CurrentLocation, friend.CurrentLocation) * 50 + 500;

                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = friend.ObjectID, Type = 1 });

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, friend, 0, DefenceType.MACAgility);

                    ActionList.Add(action);

                    BuffTime = Envir.Time + 20000;
                    ActionTime = Envir.Time + 300;
                    AttackTime = Envir.Time + AttackSpeed;
                    ShockTime = 0;
                    return;
                }

                BuffTime = Envir.Time + 10000;
            }

            if (Target == null || !CanAttack) return;

            if (InAttackRange())
            {
                Attack();

                if (Target != null && Target.Dead)
                {
                    FindTarget();
                }

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

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.ACAgility);
            ActionList.Add(action);
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (target.IsFriendlyTarget(this))
            {
                var friends = FindAllFriends(4, target.CurrentLocation);

                for (int i = 0; i < friends.Count; i++)
                {
                    friends[i].AddBuff(BuffType.ColdArcherBuff, this, Settings.Second * 10, new Stats { [Stat.MaxMAC] = 100 }, visible: true);
                    friends[i].OperateTime = 0;
                }
            }
            else if (target.IsAttackTarget(this))
            {
                target.Attacked(this, damage, defence);
            }
        }
    }
}