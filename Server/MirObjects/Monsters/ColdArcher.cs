using Server.MirDatabase;
using Server.MirEnvir;
using System;
using System.Collections.Generic;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ColdArcher : MonsterObject
    {
        public long FearTime;
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
                                    Target = ob;
                                    return;
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
            if (Target == null) return;

            List<MapObject> targets = FindAllNearby(12, Target.CurrentLocation);

            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                if (target == null || !target.IsFriendlyTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) continue;

                if (target.IsFriendlyTarget(this))
                {
                    BuffType type = BuffType.HealthAid;

                    var stats = new Stats
                    {
                        [Stat.HP] = 1000
                    };

                    target.AddBuff(type, this, Settings.Second * 10, stats);
                    target.OperateTime = 0;
                }
            }
        }

        protected override void Attack()
        {
            if (Target == null)
            {
                return;
            }

            if (!Target.IsFriendlyTarget(this))
            {
                Target = null;
                return;
            }

            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500;

            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
            ActionList.Add(new DelayedAction(DelayedType.RangeDamage, Envir.Time + delay)); //Time for projectiles should always be calculated through their distance
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
        }
    }
}