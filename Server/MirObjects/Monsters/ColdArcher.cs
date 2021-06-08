using Server.MirDatabase;
using Server.MirEnvir;
using System;
using System.Collections.Generic;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ColdArcher : MonsterObject
    {
        public byte AttackRange = 8;
        public long BuffTime;

        protected internal ColdArcher(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void FindTarget()
        {
            Map Current = CurrentMap;

            for (int d = 0; d <= Info.ViewRange; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= Current.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= Current.Width) break;
                        Cell cell = Current.Cells[x, y];
                        if (cell.Objects == null || !cell.Valid) continue;
                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                    if (ob == this) continue;
                                    if (!ob.IsFriendlyTarget(this)) continue;
                                    if (ob.Hidden) continue;
                                    if (ob.Dead) continue;
                                    Target = ob;
                                    break;
                                case ObjectType.Player:
                                    if (ob.Dead) continue;
                                    Target = ob;
                                    break;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (Target == null) return;

            if (target.IsFriendlyTarget(this))
            {
                List<MapObject> targets = FindAllNearby(12, Target.CurrentLocation);

                for (int i = 0; i < targets.Count; i++)
                {
                    target = targets[i];
                    if (target == null || target.CurrentMap != CurrentMap || target.Node == null) continue;


                    BuffType type = BuffType.MonsterMACBuff;

                    var stats = new Stats
                    {
                        [Stat.MaxMAC] = this.Stats[Stat.MaxMAC] + 100
                    };

                    target.AddBuff(type, this, Settings.Second * 10, stats, visible: true);
                    target.OperateTime = 0;
                }
            }

                if (!target.IsFriendlyTarget(this))
                {
                    target.Attacked(this, damage, defence);
                }
            
        }

        protected override void Attack()
        {
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500;

            if (Target == null)
            {
                return;
            }

            if (Envir.Random.Next(3) != 0)
            {
                AttackPlayer();
            }
            else
            {
                if (Target.IsFriendlyTarget(this) && Envir.Time > BuffTime) // BUFF
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, DefenceType.MACAgility);
                    BuffTime = Envir.Time + 20000;
                }
                else
                {
                    AttackPlayer(); // Added so that if BuffTime is not up then the mob doesn't just stand there.
                }
            }

            if (Target.Dead)
            {
                FindTarget();
            }
        }

        private void AttackPlayer()
        {
            if (!Target.IsFriendlyTarget(this))
            {
                int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500;

                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action2 = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action2);
            }
        }

    }
}