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
    class AncientBringer : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 12;
            }
        }

        protected internal AncientBringer(MonsterInfo info)
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 2);

            if (!ranged)
            {
                if (Envir.Random.Next(5) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                    LineAttack1(2); //Normal Double Punch Attack   
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    LineAttack2(3);//Screaming Attack
                }
            }
            else
            {
                if (Envir.Random.Next(10) > 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                    RangeAttack1(3);//Normal Ranged Attack                 
                }
                else
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
                    RangeAttack2(3);//Range Attack Which Spawns Bats
                }

            }
            if (Target.Dead)
                FindTarget();
        }

        private void LineAttack1(int distance)
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.ACAgility);
        }

        Cell cell = null;

        private void LineAttack2(int distance)
        {
            MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);
            dir = Functions.NextDir(dir);
            Point target = Functions.PointMove(CurrentLocation, dir, 1);

            int damage = GetAttackPower(MinDC, MaxDC * 2);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.ACAgility);

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

                    ob.Attacked(this, damage, DefenceType.ACAgility);

                    if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                    {
                        if (Envir.Random.Next(5) == 0)
                        {
                            Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Paralysis, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
                        }
                    }
                    break;
                }
            }
        }

        private void RangeAttack1(int distance)
        {
            int damage = GetAttackPower(MinMC, MaxMC);
            if (damage == 0) return;
            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.MAC);
            Target.Attacked(this, damage, DefenceType.MACAgility);
        }

        private void RangeAttack2(int distance)
        {
            int damage = GetAttackPower(MinMC, MaxMC * 2);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.MACAgility);

            SpawnSlaves();
        }

        private void SpawnSlaves()
        {
            int count = Math.Min(6, 40 - SlaveList.Count);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            for (int i = 0; i < count; i++)
            {
                MonsterObject mob = null;
                mob = GetMonster(Envir.GetMonsterInfo(Settings.AncientBatName));                
                if (mob == null) continue;

                if (!mob.Spawn(CurrentMap, Target.CurrentLocation))
                    mob.Spawn(CurrentMap, Target.CurrentLocation);

                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
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