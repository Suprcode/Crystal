using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using S = ServerPackets;
using System.Drawing;
using Server.MirEnvir;

namespace Server.MirObjects.Monsters
{
    public class TurtleKing : MonsterObject
    {
        public byte AttackRange = 7;
        public byte CloseRange = 2;

        private byte _stage = 5;

        protected internal TurtleKing(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }
        
        protected override void ProcessAI()
        {
            if (Dead) return;

            if (MaxHP >= 5)
            {
                byte stage = (byte)(HP / (MaxHP / 5));

                if (stage < _stage)
                {
                    SpawnSlaves();
                    _stage = stage;
                }
            }

            base.ProcessAI();
        }
        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, CloseRange);

            if (!ranged)
            {
                switch (Envir.Random.Next(5))
                {
                    case 0:
                    case 1:
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                        LineAttack(2);
                        break;
                    case 2:
                    case 3:
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                        LineAttack(3);
                        break;
                    case 4:
                        WaterBlast();
                        break;
                }
            }
            else
            {
                if (!Functions.InRange(CurrentLocation, Target.CurrentLocation, CloseRange) && Envir.Random.Next(4) == 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                    Point target = Functions.PointMove(CurrentLocation, Direction, 1);
                    Target.Teleport(CurrentMap, target, true, 6);
                }
                else if (!Functions.InRange(CurrentLocation, Target.CurrentLocation, CloseRange) && Envir.Random.Next(4) == 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });

                    Point target = Functions.PointMove(Target.CurrentLocation, Target.Direction, 1);

                    Teleport(CurrentMap, target, true, 6);
                }
                else
                {
                    WaterBlast();
                }
            }
            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
        }
        
        private void LineAttack(int distance)
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (target == Target.CurrentLocation)
                {
                    Target.Attacked(this, damage, DefenceType.ACAgility);
                }
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

                            ob.Attacked(this, damage, DefenceType.ACAgility);
                        }
                        else continue;

                        break;
                    }
                }

                if (Envir.Random.Next(8) == 0)
                {
                    Target.ApplyPoison(new Poison { Owner = this, Duration = 3, PType = PoisonType.Stun, TickSpeed = 1000, }, this);
                }
            }
        }

        private void SpawnSlaves()
        {
            int count = Math.Min(8, 30 - SlaveList.Count);

            for (int i = 0; i < count; i++)
            {
                MonsterObject mob = null;
                switch (Envir.Random.Next(7))
                {
                    case 0:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle1));
                        break;
                    case 1:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle2));
                        break;
                    case 2:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle3));
                        break;
                    case 3:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle4));
                        break;
                    case 4:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle5));
                        break;
                }

                if (mob == null) continue;

                if (!mob.Spawn(CurrentMap, Front))
                    mob.Spawn(CurrentMap, CurrentLocation);

                //mob.Master = this;
                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
            }
        }

        private void WaterBlast()
        {
            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

            List<MapObject> targets = FindAllTargets(AttackRange, CurrentLocation);
            if (targets.Count == 0) return;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];
                Broadcast(new S.ObjectEffect { ObjectID = Target.ObjectID, Effect = SpellEffect.TurtleKing });
                if (Target.Attacked(this, damage, DefenceType.MAC) <= 0) return;

                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    if (Envir.Random.Next(5) == 0)
                        Target.ApplyPoison(new Poison { Owner = this, Duration = 15, PType = PoisonType.Slow, TickSpeed = 1000 }, this);
                    if (Envir.Random.Next(15) == 0)
                        Target.ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, this);
                }
            }

            return;
        }
    }
}
