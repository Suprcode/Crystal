using System;
using System.Collections.Generic;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    /// <summary>    
    ///  Attack1 - Standard Melee Attack.
    ///  Attack2 - CrossHalfmoon-type attack (hits everyone within 1-2 cells around it).
    ///  AttackRange1 - Randomly hits various cells with spiderweb - anyone caught in spiderweb gets paralysed and spiderweb animation on them.
    ///  AttackRange2 - Dragon headed circle appears on ground, bursts into fire >>>>> (CANNOT FIND THIS SPELL ANIMATION) <<<<<.
    /// </summary>

    public class FlamingMutant : MonsterObject
    {
        private const byte AttackRange = 8;


        private readonly int _webSpreadMin = 6;
        private readonly int _webSpreadMax = 12;
        private readonly int _webCount = 3;

        protected internal FlamingMutant(MonsterInfo info)
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
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!ranged)
            {
                if (Envir.Random.Next(2) == 0)
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
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, true);
                    ActionList.Add(action);
                }
            }
            else
            {
                if (Envir.Random.Next(1) == 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                    SpawnWebs();
                }
                else
                {
                    MoveTo(Target.CurrentLocation);
                }
            }

            if (Target.Dead)
                FindTarget();
        }

        private void SpawnWebs()
        {
            if (Dead) return;

            int count = Envir.Random.Next(1, _webCount);

            for (int i = 0; i < CurrentMap.Players.Count; i++)
            {
                Point playerLocation = CurrentMap.Players[i].CurrentLocation;

                for (int j = 0; j < count; j++)
                {
                    Point location = new Point(CurrentLocation.X + Envir.Random.Next(-_webSpreadMin, _webSpreadMax + 1),
                                             CurrentLocation.Y + Envir.Random.Next(-_webSpreadMin, _webSpreadMax + 1));

                    if (Envir.Random.Next(3) == 0)
                    {
                        location = playerLocation;                        
                    }

                    if (!CurrentMap.ValidPoint(location)) continue;

                    for (int y = location.Y - 2; y <= location.Y + 2; y++)
                    {
                        if (y < 0) continue;
                        if (y >= CurrentMap.Height) break;

                        for (int x = location.X - 2; x <= location.X + 2; x++)
                        {
                            if (x < 0) continue;
                            if (x >= CurrentMap.Width) break;

                            if (x == CurrentLocation.X && y == CurrentLocation.Y) continue;

                            var cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid) continue;

                            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MinDC]);

                            var start = 500;

                            if (location == CurrentLocation)
                            { 
                                return; 
                            }

                            SpellObject ob = new SpellObject
                            {
                                Spell = Spell.FlamingMutantMassWeb,
                                Value = damage,
                                ExpireTime = Envir.Time + 1200 + start,
                                TickSpeed = 3000,
                                CurrentLocation = new Point(x, y),
                                CastLocation = location,
                                Show = location.X == x && location.Y == y,
                                CurrentMap = CurrentMap,
                                Caster = null,
                                Owner = this
                            };

                            DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + start, ob);
                            CurrentMap.ActionList.Add(action);
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
            bool aoeAttack = (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (aoeAttack)
            {
                List<MapObject> targets = FindAllTargets(1, CurrentLocation);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].Attacked(this, damage, defence);
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
