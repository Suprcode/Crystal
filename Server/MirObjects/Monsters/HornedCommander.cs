using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    /// <summary>
    /// Attack1 - Basic melee attack
    /// Attack2 - Basic melee attack alt
    /// Attack3 - Charge up then Spin attack (between 5 and 10 damage multiplier) twice
    /// Attack4 - Charge up then AOE rock fall (between 5 and 10 damage multiplier)
    /// Attack5 - Immunity shield, Summons slave (Under 10% HP)
    /// AttackRange1 - Teleport
    /// AttackRange2 - Hammer smash AOE in front
    /// AttackRange3 - Summon rock spikes (Between 10% - 30% HP)
    /// Summons Rock Slaves
    /// </summary>

    public class HornedCommander : MonsterObject
    {
        private bool _StartAdvanced;
        private bool _Immune;

        private Point _MapCentre
        {
            get
            {
                if (CurrentMap == null) return Point.Empty;

                //Should be spawned on "HY01_morae_chon". Change me to correct centre if its spawned on another map.
                return new Point(26, 32);
            }
        }

        //Phase 0
        private readonly byte _BoulderHealthPercent = 80;
        private bool _CalledBoulders, _StartedBoulderWalk;

        //Phase 1
        private readonly byte _RockHealthPercent = 50;
        private bool _CalledRockSpikes;
        private long _RockSpikeTime;
        private readonly Point[,] _RockSpikeArea = new Point[7, 7];
        private readonly List<SpellObject> _RockSpikeEffects = new List<SpellObject>();

        //Phase 2
        private readonly byte _ShieldHealthPercent = 10;
        private bool _CalledShield;
        private readonly byte _ShieldSeconds = 20;

        protected internal HornedCommander(MonsterInfo info)
            : base(info)
        {
        }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return !_Immune && base.IsAttackTarget(attacker);
        }

        public override bool IsAttackTarget(HumanObject attacker)
        {
            return !_Immune && base.IsAttackTarget(attacker);
        }

        protected override void ProcessAI()
        {
            if ((HealthPercent < _BoulderHealthPercent || _StartedBoulderWalk) && !_CalledBoulders)
            {
                _StartedBoulderWalk = true;
                SpawnBoulder();
            }

            if (_Immune) return;

            if (HealthPercent < _ShieldHealthPercent && !_CalledShield)
            {
                KillRockSpikes();

                _CalledShield = true;
                _Immune = true;

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 4, Level = _ShieldSeconds });

                AddBuff(BuffType.HornedCommanderShield, this, Settings.Second * _ShieldSeconds, new Stats());

                SpawnSlave();
                return;
            }

            if (HealthPercent < _RockHealthPercent && HealthPercent >= _ShieldHealthPercent)
            {
                if (!_CalledRockSpikes)
                {
                    SetupRockSpike();

                    _CalledRockSpikes = true;
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                }
            }

            if (_CalledRockSpikes && Envir.Time > _RockSpikeTime)
            {
                var spawned = SpawnRockSpikes();
                _RockSpikeTime = Envir.Time + 5000;

                if (!spawned)
                {
                    _RockSpikeTime = long.MaxValue;
                }
            }

            if (HealthPercent < 100 && !_StartAdvanced)
            {
                _StartAdvanced = true;
            }
            else if (HealthPercent == 100 && _StartAdvanced)
            {
                Reset();
            }

            base.ProcessAI();
        }

        private void Reset()
        {
            _StartAdvanced = false;
            _StartedBoulderWalk = false;
            _CalledBoulders = false;
            _CalledRockSpikes = false;
            _CalledShield = false;
            _RockSpikeTime = 0;

            KillRockSpikes();
            KillSlaves();
        }

        protected override void ProcessBuffEnd(Buff buff)
        {
            if (buff.Type == BuffType.HornedCommanderShield)
            {
                _Immune = false;
                AttackTime = Envir.Time + AttackSpeed;
                ActionTime = Envir.Time + 300;
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (InAttackRange())
            {
                Attack();

                if (Target != null && Target.Dead)
                {
                    FindTarget();
                }

                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            //Charge-Up AOE Rock Fall
            if (_StartAdvanced && Envir.Random.Next(20) == 0)
            {
                byte rockFallLoops = (byte)Envir.Random.Next(5, 10);
                int rockFallDuration = rockFallLoops * 500;

                _Immune = true;
                ActionTime = Envir.Time + (rockFallDuration) + 500;
                AttackTime = Envir.Time + (rockFallDuration) + 500 + AttackSpeed;

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 3, Level = rockFallLoops });

                var front = Functions.PointMove(CurrentLocation, Direction, 2);

                MassSpawnRockFall(front, rockFallDuration);

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]) * rockFallLoops;

                DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + (rockFallDuration) + 500, Target, damage, DefenceType.AC, 5);
                ActionList.Add(action);

                return;
            }

            //Charge-Up Spin Hit
            if (_StartAdvanced && Envir.Random.Next(15) == 0)
            {
                byte spinLoops = (byte)Envir.Random.Next(5, 10);
                int spinDuration = spinLoops * 700;

                _Immune = true;
                ActionTime = Envir.Time + spinDuration + 1500;
                AttackTime = Envir.Time + spinDuration + 1500 + AttackSpeed;

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2, Level = spinLoops });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]) * spinLoops;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + spinDuration + 500, Target, damage, DefenceType.AC, true);
                ActionList.Add(action);

                action = new DelayedAction(DelayedType.Damage, Envir.Time + spinDuration + 1000, Target, damage, DefenceType.AC, true);
                ActionList.Add(action);

                return;
            }

            //Hammer Smash
            if (_StartAdvanced && Envir.Random.Next(10) == 0)
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 300, Target, damage, DefenceType.AC, 3);
                ActionList.Add(action);
                return;
            }
            
            //Teleport
            if (_StartAdvanced && Envir.Random.Next(10) == 0)
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 300);
                ActionList.Add(action);

                return;
            }

            //Normal Attacks
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

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false);
                ActionList.Add(action);
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            _Immune = false;

            if (data.Count > 0)
            {
                MapObject target = (MapObject)data[0];
                int damage = (int)data[1];
                DefenceType defence = (DefenceType)data[2];
                bool aoe = (bool)data[3];

                if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

                if (aoe)
                {
                    var targets = FindAllTargets(3, CurrentLocation, false);

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
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            _Immune = false;

            if (data.Count > 0)
            {
                MapObject target = (MapObject)data[0];
                int damage = (int)data[1];
                DefenceType defence = (DefenceType)data[2];
                int aoeSize = (int)data[3];

                if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

                var front = Functions.PointMove(CurrentLocation, Direction, 2);
                var targets = FindAllTargets(aoeSize, front, false);

                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].Attacked(this, damage, defence);
                }
            }
            else
            {
                TeleportRandom(10, 10);
                Target = null;
            }
        }

        public override bool TeleportRandom(int attempts, int distance, Map temp = null)
        {
            for (int i = 0; i < attempts; i++)
            {
                Point location;

                if (distance <= 0)
                    location = new Point(Envir.Random.Next(CurrentMap.Width), Envir.Random.Next(CurrentMap.Height));
                else
                    location = new Point(CurrentLocation.X + Envir.Random.Next(-distance, distance + 1),
                                         CurrentLocation.Y + Envir.Random.Next(-distance, distance + 1));

                if (Teleport(CurrentMap, location, true, 10)) return true;
            }

            return false;
        }

        private void MassSpawnRockFall(Point centerPoint, int duration)
        {
            int range = 15;

            for (int x = -range; x < range; x+=10)
            {
                for (int y = -range; y < range; y += 10)
                {
                    var location = new Point(centerPoint.X + x, centerPoint.Y + y);

                    SpawnRockFall(location, Envir.Random.Next(0, 200), duration);
                }
            }
        }

        private void SpawnRockFall(Point location, int offset, int duration)
        {
            if (Dead) return;

            for (int y = location.Y - 10; y <= location.Y + 10; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = location.X - 10; x <= location.X + 10; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;

                    if (x == CurrentLocation.X && y == CurrentLocation.Y) continue;

                    var cell = CurrentMap.GetCell(x, y);

                    if (!cell.Valid) continue;

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MinMC]);

                    var start = 500 + offset;

                    SpellObject ob = new SpellObject
                    {
                        Spell = Spell.HornedCommanderRockFall,
                        Value = damage,
                        ExpireTime = Envir.Time + duration + start,
                        TickSpeed = 2000,
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

        private void SetupRockSpike()
        {
            var xLength = _RockSpikeArea.GetLength(0);
            var yLength = _RockSpikeArea.GetLength(1);

            var midX = ((int)Math.Ceiling((decimal)xLength / 2) - 1) * -1;
            var midY = ((int)Math.Ceiling((decimal)yLength / 2) - 1) * -1;

            var actualX = midX;
            var actualY = midY;

            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    var point = new Point(_MapCentre.X + (actualX * 5), _MapCentre.Y + (actualY * 5));

                    _RockSpikeArea[x, y] = point;

                    actualY++;
                }

                actualY = midY;
                actualX++;
            }
        }

        private bool SpawnRockSpikes()
        {
            var spawned = false;

            var xLength = _RockSpikeArea.GetLength(0);
            var yLength = _RockSpikeArea.GetLength(1);

            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    var point = _RockSpikeArea[x, y];

                    var existing = _RockSpikeEffects.Any(x => x.CastLocation == point);

                    if (existing) continue;

                    spawned = SpawnRockSpike(point);

                    if (spawned)
                    {
                        return true;
                    }
                }

                if (spawned)
                {
                    return true;
                }
            }

            return false;
        }

        private bool SpawnRockSpike(Point location)
        {
            var spawned = false;

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

                    if (location.X == x && location.Y == y)
                    {
                        spawned = true;
                    }

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MinMC]);

                    var start = 500;

                    SpellObject ob = new SpellObject
                    {
                        Spell = Spell.HornedCommanderRockSpike,
                        Value = damage,
                        ExpireTime = Envir.Time + start + (Settings.Minute * 10),
                        TickSpeed = 1000,
                        CurrentLocation = new Point(x, y),
                        CastLocation = location,
                        Show = location.X == x && location.Y == y,
                        CurrentMap = CurrentMap,
                        Caster = this
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + start, ob);
                    CurrentMap.ActionList.Add(action);

                    _RockSpikeEffects.Add(ob);
                }
            }

            return spawned;
        }

        private void SpawnBoulder()
        {
            if (Functions.InRange(CurrentLocation, _MapCentre, 20))
            {
                Teleport(CurrentMap, _MapCentre, true, 10);
            }

            _CalledBoulders = true;

            for (int i = 0; i < 8; i++)
            {
                var mob = GetMonster(Envir.GetMonsterInfo(Settings.HornedCommanderBombMob));

                var odd = i % 2 != 0;

                var point = Functions.PointMove(CurrentLocation, (MirDirection)i, odd ? 7 : 9);

                if (mob == null) continue;

                mob.Direction = Functions.DirectionFromPoint(point, CurrentLocation);

                if (mob.Spawn(CurrentMap, point))
                {
                    mob.Target = Target;
                    mob.ActionTime = Envir.Time;
                    SlaveList.Add(mob);
                }
            }
        }

        private void SpawnSlave()
        {
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            var mob = GetMonster(Envir.GetMonsterInfo(Settings.HornedCommanderMob));

            if (mob == null) return;

            if (!mob.Spawn(CurrentMap, Front))
                mob.Spawn(CurrentMap, CurrentLocation);

            mob.Target = Target;
            mob.ActionTime = Envir.Time;
            SlaveList.Add(mob);
        }

        private void KillRockSpikes()
        {
            _RockSpikeTime = long.MaxValue;

            foreach (var effect in _RockSpikeEffects)
            {
                effect.ExpireTime = Envir.Time;
            }

            _RockSpikeEffects.Clear();
        }

        private void KillSlaves()
        {
            //Kill Minions
            for (int i = SlaveList.Count - 1; i >= 0; i--)
            {
                if (!SlaveList[i].Dead && SlaveList[i].Node != null)
                {
                    SlaveList[i].Die();
                }
            }
        }

        public override void Die()
        {
            base.Die();

            KillRockSpikes();
            KillSlaves();
        }
    }
}
