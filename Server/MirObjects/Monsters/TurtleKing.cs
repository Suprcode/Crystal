using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class TurtleKing : MonsterObject
    {
        public byte AttackRange = 3;
        private byte _stage = 7;

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
            
            if (MaxHP >= 7)
            {
                byte stage = (byte)(HP / (MaxHP / 2));

                if (stage < _stage) SpawnSlaves();
                _stage = stage;
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

            ShockTime = 0;


            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!ranged)
            {
                if (Envir.Random.Next(2) > 0)
                {
                    base.Attack();
                }
                else
                {
                    if (Envir.Random.Next(2) > 0)
                    {
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                        Attack1();
                    }
                    else
                    {
                        if (Envir.Random.Next(2) > 0)
                        {
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                            Attack2();
                        }
                        else
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 3 });
                            Attack3(); 
                        }
                }

                int damage = GetAttackPower(MinDC, MaxDC);
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation,});
                if (damage == 0) return;

                Target.Attacked(this, damage, DefenceType.ACAgility);

                ShockTime = 0;
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;
               
            }
            else
            {
                if (Envir.Random.Next(2) == 0)
                {
                    MoveTo(Target.CurrentLocation);
                }
                else
                {                    
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                    int damage = GetAttackPower(MinMC, MaxMC);
                    if (damage == 0) return;
                    
                    ActionTime = Envir.Time + 300;
                    AttackTime = Envir.Time + AttackSpeed;
                    
                    int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MACAgility);
                    ActionList.Add(action);
                }
            }
        }
        private void Attack1()
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;
            
            Target.Attacked(this, damage, DefenceType.ACAgility);
        }
        private void Attack2()
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.ACAgility);
        }
        private void Attack3()
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.ACAgility);

            if (Envir.Random.Next(8) == 0)
            {
                Target.ApplyPoison(new Poison { Owner = this, Duration = 3, PType = PoisonType.Stun, TickSpeed = 1000, }, this);
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

                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
            }
        }
    }
}