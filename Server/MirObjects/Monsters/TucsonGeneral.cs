using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class TucsonGeneral : MonsterObject
    {

        private long _RageTime;

        private const int _RockCount = 15;

        protected internal TucsonGeneral(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 2);

            if (Envir.Time > _RageTime)
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                _RageTime = Envir.Time + 20000;

                AttackTime = Envir.Time + 8000;

                var targets = FindAllTargets(10, CurrentLocation, false);

                for (int i = 0; i < _RockCount; i++)
                {
                    Point location = new Point(CurrentLocation.X + Envir.Random.Next(-Info.ViewRange, Info.ViewRange + 1),
                                                                 CurrentLocation.Y + Envir.Random.Next(-Info.ViewRange, Info.ViewRange + 1));

                    if (Envir.Random.Next(3) == 0 && targets.Count > 0)
                    {
                        location = targets[Envir.Random.Next(targets.Count)].CurrentLocation;
                    }

                    if (location.X == CurrentLocation.X || location.Y == CurrentLocation.Y) continue;

                    var start = Envir.Random.Next(0, 5000);
                    var value = Envir.Random.Next(Stats[Stat.MinDC], Stats[Stat.MaxDC]);

                    var spellObj = new SpellObject
                    {
                        Spell = Spell.TucsonGeneralRock,
                        Value = value,
                        ExpireTime = Envir.Time + 2000 + start,
                        TickSpeed = 1000,
                        Caster = this,
                        CurrentLocation = location,
                        CurrentMap = CurrentMap,
                        Direction = MirDirection.Up,
                        StartTime = Envir.Time + 1000 + start
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + start, spellObj);
                    CurrentMap.ActionList.Add(action);
                }

                return;
            }

            if (!ranged && Envir.Random.Next(4) > 0)
            {
                if (Envir.Random.Next(3) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, true);
                    ActionList.Add(action);
                }
            }
            else
            {
                if (Envir.Random.Next(4) > 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
                    int damage = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]);
                    if (damage == 0) return;

                    ProjectileAttack(damage, DefenceType.MACAgility);
                }
                else
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 2 });
                    int damage = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC] * 2);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, true);
                    ActionList.Add(action);
                }
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool stomp = (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (stomp)
            {
                var targets = FindAllTargets(3, CurrentLocation, false);

                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].Attacked(this, damage, defence) <= 0) continue;

                    PoisonTarget(targets[i], 3, 5, PoisonType.Paralysis, 1000);
                }
            }
            else
            {
                target.Attacked(this, damage, defence);
            }
        }

        protected override void ProcessTarget()
        {
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

            MoveTo(Target.CurrentLocation);
        }
    }
}
