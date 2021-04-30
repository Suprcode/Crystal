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
    class CannibalTentacles : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 8;
            }
        }

        protected internal CannibalTentacles(MonsterInfo info)
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
                if (Envir.Random.Next(5) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;
                    Target.Attacked(this, damage);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    Attack2();//Halfmoon Attack
                }

            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                RangeAttack1();//Projectile Attack
            }


            if (Target.Dead)
                FindTarget();

        }

        private void Attack2() //Halfmoon Attack
        {
            MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

            dir = Functions.NextDir(dir);

            Point target = Functions.PointMove(CurrentLocation, dir, 1);

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;            

            if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
            {
                Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, TickSpeed = 1000, }, this);
            }

            Cell cell = null;

            for (int i = 0; i < 4; i++)
            {
                target = Functions.PointMove(CurrentLocation, dir, 1);
                dir = Functions.NextDir(dir);

                if (!CurrentMap.ValidPoint(target)) continue;

                cell = CurrentMap.GetCell(target);

                if (cell.Objects == null) continue;

                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject ob = cell.Objects[o];
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                    if (!ob.IsAttackTarget(this)) continue;

                    ob.Attacked(this, damage, DefenceType.MAC);
                    break;
                }
            }

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 400, Target, damage, DefenceType.MAC);
            ActionList.Add(action);
        }

        private void RangeAttack1()
        {
            int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MAC);
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

