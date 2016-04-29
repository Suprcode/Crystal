using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class Behemoth : MonsterObject
    {
        public byte AttackRange = 10;

        protected internal Behemoth(MonsterInfo info)
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

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!ranged)
            {
                switch (Envir.Random.Next(5))
                {
                    case 0:
                    case 1:
                    case 2:
                        base.Attack(); //swipe
                        break;
                    case 3:
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                        //fire circle
                        Attack1();
                        break;
                    case 4:
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                        //push back
                        Attack2();
                        break;
                }
                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    if (Envir.Random.Next(15) == 0)
                    {
                        Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Bleeding, Value = GetAttackPower(MinDC, MinDC), TickSpeed = 1000 }, this);
                    }
                }
            }
            else
            {
                if (Envir.Random.Next(2) == 0)
                {
                    MoveTo(Target.CurrentLocation);
                }
                else
                {
                    switch (Envir.Random.Next(2))
                    {
                        case 0:
                            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                            SpawnSlaves(); //spawn huggers
                            break;
                        case 1:
                            {
                                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                                List<MapObject> targets = FindAllTargets(AttackRange, CurrentLocation);
                                if (targets.Count == 0) return;

                                int damage = GetAttackPower(MinDC, MaxDC) * 3;
                                if (damage == 0) return;

                                for (int i = 0; i < targets.Count; i++)
                                {
                                    Broadcast(new S.ObjectEffect { ObjectID = targets[i].ObjectID, Effect = SpellEffect.Behemoth });

                                    if (targets[i].Attacked(this, damage, DefenceType.ACAgility) > 0)
                                    {
                                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                                        {
                                            if (Envir.Random.Next(15) == 0)
                                            {
                                                targets[i].ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, this);
                                            }
                                        }
                                    }
                                }

                            }
                            break;
                    }
                }

                ShockTime = 0;
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;
            }
        }

        private void Attack1()
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation);

            if (targets.Count == 0) return;

            int damage = GetAttackPower(MinDC, MaxDC);

            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].Attacked(this, damage, DefenceType.AC);
            }
        }

        private void Attack2()
        {
            Point target = Functions.PointMove(CurrentLocation, Direction, 1);

            Cell cell = CurrentMap.GetCell(target);

            if (cell.Objects != null)
            {
                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject t = cell.Objects[o];
                    if (t == null || t.Race != ObjectType.Player) continue;

                    if (t.IsAttackTarget(this))
                    {
                        t.Pushed(this, Direction, 4);

                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= t.PoisonResist)
                        {
                            if (Envir.Random.Next(3) == 0)
                            {
                                t.ApplyPoison(new Poison { Owner = this, Duration = 15, PType = PoisonType.Stun, TickSpeed = 1000 }, this);
                            }
                        }
                    }
                    break;
                }
            }
        }

        private void SpawnSlaves()
        {
            List<MapObject> targets = FindAllTargets(10, CurrentLocation);

            int count = Math.Min(8, (targets.Count * 5) - SlaveList.Count);
            
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            for (int i = 0; i < count; i++)
            {
                MonsterObject mob = null;
                switch (Envir.Random.Next(4))
                {
                    case 0:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.BehemothMonster1));
                        break;
                    case 1:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.BehemothMonster2));
                        break;
                    case 2:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.BehemothMonster3));
                        break;
                }

                if (mob == null) continue;

                if (!mob.Spawn(CurrentMap, Front))
                    mob.Spawn(CurrentMap, CurrentLocation);
                
                mob.Target = targets[Envir.Random.Next(targets.Count)];
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
            }
        }

        public override void Die()
        {
            foreach (var slave in SlaveList)
            {
                slave.Die();
            }

            base.Die();
        }
    }
}