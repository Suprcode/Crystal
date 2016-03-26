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
                        Attack1(); //push back
                        break;
                    case 4:
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                        Attack2(); //fire circle
                        break;
                }

                if (Envir.Random.Next(15) == 0)
                {
                    Target.ApplyPoison(new Poison { Owner = this, Duration = 15, PType = PoisonType.Bleeding, TickSpeed = 1000 }, this);
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
                            SummonSlaves(); //spawn huggers
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
                                    Target = targets[i];
                                    Broadcast(new S.ObjectEffect { ObjectID = Target.ObjectID, Effect = SpellEffect.Behemoth });

                                    if (Target.Attacked(this, damage, DefenceType.ACAgility) > 0)
                                    {
                                        if (Envir.Random.Next(15) == 0)
                                            Target.ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, this);
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
            List<MapObject> targets = FindAllTargets(2, CurrentLocation);

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

                        if (Envir.Random.Next(3) == 0)
                        {
                            t.ApplyPoison(new Poison { Owner = this, Duration = 15, PType = PoisonType.Stun, TickSpeed = 1000 }, this);
                        }
                    }
                    break;
                }

                SummonSlaves();
            }
        }

        private void SummonSlaves()
        {
            List<MapObject> targets = FindAllTargets(10, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                MonsterObject spawn = null;

                switch(Envir.Random.Next(3))
                {
                    case 0:
                        spawn = GetMonster(Envir.GetMonsterInfo("Hugger"));
                        break;
                    case 1:
                        spawn = GetMonster(Envir.GetMonsterInfo("PoisonHugger"));
                        break;
                    case 2:
                        spawn = GetMonster(Envir.GetMonsterInfo("MutatedHugger"));
                        break;
                }

                if (spawn == null) return;

                spawn.Target = targets[i];
                spawn.ActionTime = Envir.Time + 1000;

                spawn.Spawn(CurrentMap, CurrentLocation);
               
            }
        }
    }
}