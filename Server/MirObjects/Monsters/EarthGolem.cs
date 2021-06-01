using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class EarthGolem : ZumaMonster
    {
        public long FearTime;

        protected virtual byte AttackRange
        {
            get
            {
                return 8;
            }
        }

        protected internal EarthGolem(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void ProcessAI()
        {
            if (!Dead && Envir.Time > ActionTime)
            {
                bool stoned = !FindNearby(4);

                if (Stoned && !stoned)
                {
                    Wake();

                    AttackTime = Envir.Time + 1500;
                    ActionTime = Envir.Time + 1500;
                    MoveTime = Envir.Time + 1500;
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

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (!ranged)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                ActionList.Add(action);
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                var location = Target.CurrentLocation;
                var show = true;

                for (int y = location.Y - 1; y <= location.Y + 1; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = location.X - 1; x <= location.X + 1; x++)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        var cell = CurrentMap.GetCell(x, y);

                        if (!cell.Valid) continue;

                        bool cast = true;
                        if (cell.Objects != null)
                            for (int o = 0; o < cell.Objects.Count; o++)
                            {
                                MapObject target = cell.Objects[o];
                                if (target.Race != ObjectType.Spell || ((SpellObject)target).Spell != Spell.EarthGolemPile) continue;

                                cast = false;
                                break;
                            }

                        if (!cast) continue;

                        int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MinDC]);

                        SpellObject ob = new SpellObject
                        {
                            Spell = Spell.EarthGolemPile,
                            Value = damage,
                            ExpireTime = Envir.Time + 1700,
                            TickSpeed = 1000,
                            CurrentLocation = new Point(x, y),
                            CastLocation = location,
                            Show = show,
                            CurrentMap = CurrentMap
                        };

                        show = false;

                        DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + 500, ob);
                        CurrentMap.ActionList.Add(action);
                    }
                }
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (InAttackRange() && Envir.Time < FearTime)
            {
                Attack();
                return;
            }

            FearTime = Envir.Time + 2000;

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            int dist = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);

            if (dist >= AttackRange)
                MoveTo(Target.CurrentLocation);
            else
            {
                MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

                if (Walk(dir)) return;

                switch (Envir.Random.Next(2)) //No favour
                {
                    case 0:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.NextDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                    default:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.PreviousDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                }

            }
        }
    }
}
