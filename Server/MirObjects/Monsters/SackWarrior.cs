using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class SackWarrior : MonsterObject
    {
        protected internal SackWarrior(MonsterInfo info)
            : base(info)
        {
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

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Random.Next(3) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                HalfMoonAttack();
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                CrossHalfMoonAttack();
            }

            if (Target.Dead)
                FindTarget();
        }

        private void HalfMoonAttack()
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

                    ob.Attacked(this, damage, DefenceType.ACAgility);
                    break;
                }
            }

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 400, Target, damage, DefenceType.ACAgility);
            ActionList.Add(action);
        }

        private void CrossHalfMoonAttack()
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                if (targets[i].Attacked(this, damage, DefenceType.AC) <= 0) return;
            }

        }
    }
}
