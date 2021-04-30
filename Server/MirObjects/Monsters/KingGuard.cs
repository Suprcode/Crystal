using System.Collections.Generic;
using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Linq;
using System.Text;

namespace Server.MirObjects.Monsters
{
    class KingGuard : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 10;
            }
        }

        protected internal KingGuard(MonsterInfo info)
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

            if (!ranged && Envir.Random.Next(3) > 0)
            {
                if (Envir.Random.Next(5) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;
                    Target.Attacked(this, damage, DefenceType.ACAgility);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    AOEAttack1(1);
                }

            }
            else
            {
                if (Envir.Random.Next(3) > 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                    AttackTime = Envir.Time + AttackSpeed + 500;
                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) return;

                    if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
                    {
                        if (Envir.Random.Next(10) == 0)
                            Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, Value = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]), TickSpeed = 1000 }, this);
                    }

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                    ActionList.Add(action);

                }
                else
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
                    MassRangedAttack(1);
                }
            }


            if (Target.Dead)
                FindTarget();

        }

        private void AOEAttack1(int distance)//AOE Attack 1
        {
            List<MapObject> targets = FindAllTargets(3, CurrentLocation);
            if (targets.Count == 0) return;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC] * 2);
            if (damage == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];
                if (Target.IsAttackTarget(this))
                    targets[i].Attacked(this, damage, DefenceType.AC);
            }
        }

        private void MassRangedAttack(int distance)
        {
            List<MapObject> targets = FindAllTargets(10, CurrentLocation);
            if (targets.Count == 0) return;

            ShockTime = 0;

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];

                Broadcast(new S.ObjectEffect { ObjectID = Target.ObjectID, Effect = SpellEffect.KingGuard });

                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC] * 2);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.MAC);

                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
                {
                    if (Envir.Random.Next(5) == 0)
                        Target.ApplyPoison(new Poison { Owner = this, Duration = 10, PType = PoisonType.Slow, Value = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]), TickSpeed = 1000 }, this);
                    if (Envir.Random.Next(5) == 0)
                        Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Paralysis, Value = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]), TickSpeed = 1000 }, this);
                }
            }
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
