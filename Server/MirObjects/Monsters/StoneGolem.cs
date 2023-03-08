using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class StoneGolem : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 4;
            }
        }

        protected internal StoneGolem(MonsterInfo info)
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (!ranged)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.AC);
                ActionList.Add(action);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                var location = Functions.PointMove(CurrentLocation, Direction, 3);

                for (int y = location.Y - 2; y <= location.Y + 2; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = location.X - 2; x <= location.X + 2; x++)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        var cell = CurrentMap.GetCell(x, y);

                        if (!cell.Valid) continue;

                        int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MinMC]);

                        var start = 500;

                        SpellObject ob = new SpellObject
                        {
                            Spell = Spell.StoneGolemQuake,
                            Value = damage,
                            ExpireTime = Envir.Time + 800 + start,
                            TickSpeed = 1000,
                            Direction = Direction,
                            CurrentLocation = new Point(x, y),
                            CastLocation = CurrentLocation,
                            Show = location.X == x && location.Y == y,
                            CurrentMap = CurrentMap,
                            Caster = this
                        };

                        DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + start, ob);
                        CurrentMap.ActionList.Add(action);
                    }
                }
            }
        }
    }
}
