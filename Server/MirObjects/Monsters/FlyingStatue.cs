using Server.MirDatabase;
using System;
using System.Collections.Generic;
using System.Drawing;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    /// <summary>
    /// Attack1 - Green Poison attack
    /// Attack2 - Purple Poison attack
    /// AttackRange1 - Ice Tornado (big range)
    /// AttackRange2 - Blizzard (big range)
    /// </summary>

    public class FlyingStatue : MonsterObject
    {
        public long blizzardCooldown;

        protected virtual byte AttackRange
        {
            get
            {
                return 12;
            }
        }

        protected internal FlyingStatue(MonsterInfo info)
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

            ActionTime = Envir.Time + 600;
            AttackTime = Envir.Time + AttackSpeed;

            if (!ranged)
            {
                if (Envir.Random.Next(6) != 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 1100, Target, damage, DefenceType.MACAgility);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 1100, Target, damage, DefenceType.MACAgility);
                    ActionList.Add(action);
                }
            }
            else
            {
                if (Envir.Random.Next(6) != 0 && Envir.Time > blizzardCooldown)
                {

                    SpawnIceTornado();
                }
                else
                {
                    SpawnBlizzard();
                    blizzardCooldown = Envir.Time + (Settings.Second * Envir.Random.Next(7, 15));
                }
            }
        }

        private void SpawnBlizzard()
        {
            if (Dead) return;

            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });

            List<MapObject> getTargetInAttackRange = FindAllTargets(AttackRange, CurrentLocation);

            var count = getTargetInAttackRange.Count;

            if (count == 0) return;

            var target = getTargetInAttackRange[Envir.Random.Next(count)];

            var location = target.CurrentLocation;

            for (int y = location.Y - 3; y <= location.Y + 3; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = location.X - 3; x <= location.X + 3; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;

                    if (x == CurrentLocation.X && y == CurrentLocation.Y) continue;

                    var cell = CurrentMap.GetCell(x, y);

                    if (!cell.Valid) continue;

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MinMC]) * 2;

                    var start = 500;

                    SpellObject ob = new SpellObject
                    {
                        Spell = Spell.MonsterBlizzard,
                        Value = damage,
                        ExpireTime = Envir.Time + 2500 + start,
                        TickSpeed = 1000,
                        CurrentLocation = new Point(x, y),
                        CastLocation = location,
                        Show = location.X == x && location.Y == y,
                        CurrentMap = CurrentMap,
                        Owner = this
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + start, ob);
                    CurrentMap.ActionList.Add(action);
                }
            }
        }

        private void SpawnIceTornado()
        {
            if (Dead) return;

            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });

            List<MapObject> getAllTargetsInAttackRange = FindAllTargets(AttackRange, CurrentLocation);

            var count = getAllTargetsInAttackRange.Count;

            if (count == 0) return;

            var target = getAllTargetsInAttackRange[Envir.Random.Next(count)];

            var location = target.CurrentLocation;

            for (int y = location.Y - 1; y <= location.Y + 1; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = location.X - 1; x <= location.X + 1; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;

                    if (x == CurrentLocation.X && y == CurrentLocation.Y) continue;

                    var cell = CurrentMap.GetCell(x, y);

                    if (!cell.Valid) continue;

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MinMC]);

                    var start = 500;

                    SpellObject ob = new SpellObject
                    {
                        Spell = Spell.FlyingStatueIceTornado,
                        Value = damage,
                        ExpireTime = Envir.Time + 1500 + start,
                        TickSpeed = 3000,
                        CurrentLocation = new Point(x, y),
                        CastLocation = location,
                        Show = location.X == x && location.Y == y,
                        CurrentMap = CurrentMap,
                        Owner = this
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + start, ob);
                    CurrentMap.ActionList.Add(action);
                }
            }
        }

        public override void Spawned()
        {
            // Begin timers (stops players from being bombarded with attacks when they enter the room / map).
            blizzardCooldown = Envir.Time + (Settings.Second * 10);

            base.Spawned();
        }

        protected override void ProcessTarget()
        {
            int dist = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);
            var hpPercent = (HP * 100) / Stats[Stat.HP];
            bool halfHealth = hpPercent <= 50;

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

            if (dist >= AttackRange)
                MoveTo(Target.CurrentLocation);
            else
            {
                MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

                if (Walk(dir)) return;

                switch (Envir.Random.Next(6)) //No favour
                {
                    case 0:
                    case 1:
                    case 2:
                        if (halfHealth == true)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                dir = Functions.NextDir(dir);

                                if (Walk(dir))
                                    return;
                            }
                        }
                        else
                        {
                            MoveTo(Target.CurrentLocation);
                        }
                        break;
                    case 3:
                    case 4:
                    case 5:
                    default:
                        MoveTo(Target.CurrentLocation);
                        break;
                }

            }
        }
    }
}
