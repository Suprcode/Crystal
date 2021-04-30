﻿using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{   
    public class SandSnail : MonsterObject
    {
        
        protected internal SandSnail(MonsterInfo info)
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

            if (Envir.Random.Next(7) > 0)
            {
                if (Envir.Random.Next(2) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                    int damage = GetAttackPower(MinDC, MaxDC);
                    if (damage == 0) return;
                    Target.Attacked(this, damage);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                    Attack3(); //Poison Shake
                }
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                Attack2(); //Halfmoon Tongue Attack
            }

        }

        private void Attack2() //Halfmoon Attack
        {
            MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

            dir = Functions.NextDir(dir);

            Point target = Functions.PointMove(CurrentLocation, dir, 1);

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.AC);

            Cell cell = null;

            for (int i = 0; i < 6; i++)
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

                    ob.Attacked(this, damage, DefenceType.AC);
                    break;
                }
            }
        }

        private void Attack3()
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                int damage = GetAttackPower(MinSC, MaxSC);
                if (damage == 0) return;

                if (targets[i].Attacked(this, damage, DefenceType.MAC) <= 0) return;
                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    targets[i].ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
                }
            }

        }
    }
}
