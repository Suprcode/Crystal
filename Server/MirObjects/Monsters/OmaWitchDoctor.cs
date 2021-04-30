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
    public class OmaWitchDoctor : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 8;
            }
        }

        protected internal OmaWitchDoctor(MonsterInfo info)
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
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.MACAgility);
            }
            else
            {
                if (Envir.Random.Next(3) > 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });
                    AttackTime = Envir.Time + AttackSpeed + 500;
                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) return;
                    
                    LineAttack(6);

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]); 
                    AttackTime = Envir.Time + AttackSpeed + 500;
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                    ActionList.Add(action);
                }
            }

            if (Target.Dead)
                FindTarget();
        }


        private void LineAttack(int distance)
        {
            int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            if (damage == 0) return;

            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (target == Target.CurrentLocation)
                    Target.Attacked(this, damage, DefenceType.MACAgility);
                else
                {
                    if (!CurrentMap.ValidPoint(target)) continue;

                    Cell cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                        {
                            if (!ob.IsAttackTarget(this)) continue;
                            ob.Attacked(this, damage, DefenceType.MACAgility);
                        }
                        else continue;

                        break;
                    }
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
