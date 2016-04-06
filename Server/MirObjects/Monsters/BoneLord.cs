using System;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class BoneLord : MonsterObject
    {
        public byte AttackRange = 7;
        public byte _stage = 3;

        protected internal BoneLord(MonsterInfo info)
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
            bool range = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (range)
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                AttackTime = Envir.Time + AttackSpeed;
                ActionTime = Envir.Time + 300;

                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;

                if (Envir.Random.Next(Settings.MagicResistWeight) >= Target.MagicResist)
                {
                    int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MACAgility);
                    ActionList.Add(action);
                }
            }
            else
            {
                base.Attack();
            }

            if (Target.Dead)
                FindTarget();

        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (MaxHP >= 3)
            {
                byte stage = (byte)(HP / (MaxHP / 3));

                if (stage < _stage)
                {
                    SpawnSlaves();
                    _stage = stage;
                    return;
                    }
            }

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

        private void SpawnSlaves()
        {
            int count = Math.Min(8, 40 - SlaveList.Count);

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            for (int i = 0; i < count; i++)
            {
                MonsterObject mob = null;
                switch (Envir.Random.Next(4))
                {
                    case 0:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.BoneMonster1));
                        break;
                    case 1:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.BoneMonster2));
                        break;
                    case 2:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.BoneMonster3));
                        break;
                    case 3:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.BoneMonster4));
                        break;
                }

                if (mob == null) continue;

                if (!mob.Spawn(CurrentMap, Front))
                    mob.Spawn(CurrentMap, CurrentLocation);

                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
            }

        }
    }
}
