using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class TreeQueen : MonsterObject
    {
        protected override bool CanMove { get { return false; } }
        protected override bool CanRegen { get { return false; } }

        private readonly int _rootSpreadMin = 5;
        private readonly int _rootSpreadMax = 15;
        private readonly int _rootCount = 5;
        private long _rootSpawnTime;

        private readonly int _groundrootSpread = 5;
        private long _groundRootSpawnTime;

        private readonly int _nearMultiplier = 4;
        private bool _notNear = true;

        protected internal TreeQueen(MonsterInfo info)
            : base(info)
        {
            Direction = MirDirection.Up;
        }

        protected override bool InAttackRange()
        {
            return Target.CurrentMap == CurrentMap;
        }

        public override void Turn(MirDirection dir) { }

        public override bool Walk(MirDirection dir) { return false; }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            return base.Attacked(attacker, damage, type);
        }

        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            return base.Attacked(attacker, damage, type, damageWeapon);
        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true)
        {

        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;

            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + AttackSpeed;

            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 2);

            if (!ranged)
            {
                _notNear = false;

                if (Envir.Random.Next(2) > 0)
                {
                    // Fire Bombardment Spell
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MACAgility, true, false);
                    ActionList.Add(action);                    
                }
                else
                {
                    // Push Attack
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MACAgility, false, true);
                    ActionList.Add(action);
                }
            }
            else
            {
                _notNear = true;
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool fireBombardment = (bool)data[3];
            bool pushAttack = (bool)data[4];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (fireBombardment)
            {
                List<MapObject> targets = FindAllTargets(3, CurrentLocation);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].Attacked(this, damage, defence);
                }
            }

            if (pushAttack)
            {
                List<MapObject> targets = FindAllTargets(1, CurrentLocation);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].Pushed(this, Functions.DirectionFromPoint(CurrentLocation, targets[i].CurrentLocation), 5);
                }
            }
        }

        private void SpawnRoots()
        {
            if (Dead) return;

            int count = Envir.Random.Next(1, _rootCount);
            int distance = Envir.Random.Next(_rootSpreadMin, _rootSpreadMax);

            for (int j = 0; j < CurrentMap.Players.Count; j++)
            {
                Point playerLocation = CurrentMap.Players[j].CurrentLocation;

                bool hit = false;

                for (int i = 0; i < count; i++)
                {
                    Point location = new Point(playerLocation.X + Envir.Random.Next(-distance, distance + 1),
                                             playerLocation.Y + Envir.Random.Next(-distance, distance + 1));

                    if (Envir.Random.Next(3) == 0)
                    {
                        location = playerLocation;
                        hit = true;
                    }

                    if (!CurrentMap.ValidPoint(location)) continue;

                    var start = Envir.Random.Next(2000);

                    SpellObject spellObj = new SpellObject
                    {
                        Spell = Spell.TreeQueenRoot,
                        Value = Envir.Random.Next(Envir.Random.Next(Stats[Stat.MinMC], Stats[Stat.MaxMC])),
                        ExpireTime = Envir.Time + 1500 + start,
                        TickSpeed = 2000,
                        Caster = this,
                        CurrentLocation = location,
                        CurrentMap = CurrentMap,
                        Direction = MirDirection.Up
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + start, spellObj);
                    CurrentMap.ActionList.Add(action);

                    if (hit)
                    {
                        break;
                    }
                }
            }
        }

        private void SpawnMassRoots()
        {
            if (Dead) return;

            var count = CurrentMap.Players.Count;

            if (count == 0) return;

            var target = CurrentMap.Players[Envir.Random.Next(count)];

            var location = target.CurrentLocation;

            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = target.ObjectID, Type = 1 });

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

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MinMC]);

                    var start = 500;

                    SpellObject ob = new SpellObject
                    {
                        Spell = Spell.TreeQueenMassRoots,
                        Value = damage,
                        ExpireTime = Envir.Time + 1500 + start,
                        TickSpeed = 1000,
                        CurrentLocation = new Point(x, y),
                        CastLocation = location,
                        Show = location.X == x && location.Y == y,
                        CurrentMap = CurrentMap,
                        Caster = this
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + start, ob);
                    CurrentMap.ActionList.Add(action);
                }
            }
        }

        private void SpawnGroundRoots()
        {
            if (Dead) return;

            for (int i = 0; i < CurrentMap.Players.Count; i++)
            {
                Point location = new Point(CurrentLocation.X + Envir.Random.Next(-_groundrootSpread, _groundrootSpread + 1),
                                         CurrentLocation.Y + Envir.Random.Next(-_groundrootSpread, _groundrootSpread + 1));

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

                        var start = Envir.Random.Next(4000);

                        SpellObject ob = new SpellObject
                        {
                            Spell = Spell.TreeQueenGroundRoots,
                            Value = damage,
                            ExpireTime = Envir.Time + 900 + start,
                            TickSpeed = 1000,
                            CurrentLocation = new Point(x, y),
                            CastLocation = location,
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

        public override Packet GetInfo()
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Location = CurrentLocation,
                Image = Info.Image,
                Direction = Direction,
                Effect = Info.Effect,
                AI = Info.AI,
                Light = Info.Light,
                Dead = Dead,
                Skeleton = Harvested,
                Poison = CurrentPoison,
                Hidden = Hidden,
            };
        }

        public override void Spawned()
        {
            // Begin timers (stops players from being bombarded with attacks when they enter the room / map).
            _rootSpawnTime = Envir.Time + (Settings.Second * 5);
            _groundRootSpawnTime = Envir.Time + (Settings.Second * 15);

            base.Spawned();
        }

        protected override void ProcessTarget()
        {
            if (CurrentMap.Players.Count == 0) return;

            if (Envir.Time > _rootSpawnTime)
            {
                if (Envir.Random.Next(4) > 0)
                {
                    SpawnRoots();
                }
                else
                {
                    SpawnMassRoots();
                }

                var next = Envir.Random.Next(1, 4);

                _rootSpawnTime = Envir.Time + (Settings.Second * (_notNear ? next : next * _nearMultiplier));
            }

            if (Envir.Time > _groundRootSpawnTime)
            {
                SpawnGroundRoots();

                var next = Envir.Random.Next(2, 3);

                _groundRootSpawnTime = Envir.Time + (Settings.Second * (_notNear ? next : next * _nearMultiplier));
            }

            if (Target == null || !CanAttack) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }
        }
    }
}
