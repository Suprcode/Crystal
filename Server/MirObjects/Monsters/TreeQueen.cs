using System.Collections.Generic;
using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Linq;
using System.Text;


namespace Server.MirObjects.Monsters
{
    public class TreeQueen : MonsterObject
    {
        protected override bool CanMove { get { return false; } }
        protected override bool CanRegen { get { return false; } }

        private bool _raged = false;
        private long _rageDelay = Settings.Minute * 2;
        private long _rageTime;

        private int _rootSpreadMin = 5;
        private int _rootSpreadMax = 15;
        private int _rootCount = 2;

        private int _groundrootSpreadMin = 5;
        private int _groundrootSpreadMax = 25;
        private int _groundrootCount = 1;
        private long _groundrootSpawnTime;

        public long SlaveSpawnTime;
        private int slaveSpawnSpreadMin = 10;
        private int slaveSpawnSpreadMax = 20;

        protected virtual byte AttackRange
        {
            get
            {
                return 10;
            }
        }

        protected internal TreeQueen(MonsterInfo info)
            : base(info)
        {
            Direction = MirDirection.Up;
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return true;
        }

        public override void Turn(MirDirection dir)
        {
        }
        public override bool Walk(MirDirection dir) { return false; }


        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            return base.Attacked(attacker, damage, type);
        }

        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
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
                if (Envir.Random.Next(10) > 0)
                {
                    {
                        // Fire Bombardment Spell
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                        int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                        if (damage == 0) return;

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, true, false);
                        ActionList.Add(action);
                    }
                }
                else
                {
                    // Mass Roots Spell
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false, true);
                    ActionList.Add(action);

                }
            }
            else
            {
                if (CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange))
                {
                    // Mass Roots Spell
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false, true);
                    ActionList.Add(action);
                }
            }

            if (Target.Dead)
                FindTarget();
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            List<MapObject> targets = FindAllTargets(2, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                target = targets[i];
                if (target.IsAttackTarget(this))
                    target.Attacked(this, damage, defence);
            }

            var finalDamage = target.Attacked(this, damage, defence);

            if (finalDamage > 0)
            {
                PoisonTarget(target, 15, 10, PoisonType.Red, 1000);
            }
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool fireBombardment = (bool)data[3];
            bool massRoots = (bool)data[4];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (fireBombardment)
            {
                List<MapObject> targets = FindAllTargets(2, CurrentLocation);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    target = targets[i];
                    if (target.IsAttackTarget(this))
                        target.Attacked(this, damage, defence);
                }
            }

            if (massRoots)
            {
                List<MapObject> targets = FindAllTargets(3, target.CurrentLocation);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    target = targets[i];
                    if (target.IsAttackTarget(this))
                        target.Attacked(this, damage, defence);
                }
            }
        }

        private void SpawnRoots()
        {
            int count = Envir.Random.Next(1, _raged ? _rootCount * 2 : _rootCount);
            int distance = Envir.Random.Next(_rootSpreadMin, _rootSpreadMax);

            for (int j = 0; j < CurrentMap.Players.Count; j++)
            {
                Point playerLocation = CurrentMap.Players[j].CurrentLocation;

                for (int i = 0; i < count; i++)
                {
                    Point location = new Point(playerLocation.X + Envir.Random.Next(-distance, distance + 1),
                                             playerLocation.Y + Envir.Random.Next(-distance, distance + 1));

                    if (Envir.Random.Next(10) == 0)
                    {
                        location = playerLocation;
                    }

                    if (!CurrentMap.ValidPoint(location)) continue;

                    SpellObject spellObj = new SpellObject
                    {
                        Spell = Spell.TreeQueenRoot,
                        Value = Envir.Random.Next(Envir.Random.Next(Stats[Stat.MinDC], Stats[Stat.MaxDC])),
                        ExpireTime = Envir.Time + (2000),
                        TickSpeed = 20000,
                        Caster = null,
                        CurrentLocation = location,
                        CurrentMap = CurrentMap,
                        Direction = MirDirection.Up
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + Envir.Random.Next(5000), spellObj);
                    CurrentMap.ActionList.Add(action);
                }
            }
        }

        private void SpawnGroundRoots()
        {
            int count = Envir.Random.Next(1, _raged ? _groundrootCount * 1 : _groundrootCount);
            int distance = Envir.Random.Next(_groundrootSpreadMin, _groundrootSpreadMax);

            for (int j = 0; j < CurrentMap.Players.Count; j++)
            {
                Point playerLocation = CurrentMap.Players[j].CurrentLocation;

                for (int i = 0; i < count; i++)
                {
                    Point location = new Point(playerLocation.X + Envir.Random.Next(-distance, distance + 1),
                                             playerLocation.Y + Envir.Random.Next(-distance, distance + 1));

                    if (Envir.Random.Next(10) == 0)
                    {
                        location = playerLocation;
                    }

                    if (!CurrentMap.ValidPoint(location)) continue;

                    SpellObject spellObj = null;

                    spellObj = new SpellObject
                    {
                        Spell = Spell.TreeQueenGroundRoots,
                        Value = Envir.Random.Next(Envir.Random.Next(Stats[Stat.MinDC], Stats[Stat.MaxDC])),
                        ExpireTime = Envir.Time + (2000),
                        TickSpeed = 40000,
                        Caster = null,
                        CurrentLocation = location,
                        CurrentMap = CurrentMap,
                        Direction = MirDirection.Up
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + Envir.Random.Next(60000), spellObj);
                    CurrentMap.ActionList.Add(action);
                }
            }

        }

        private void SpawnSlaves()
        {
            int count = Math.Min(2, 6 - SlaveList.Count);
            int distance = Envir.Random.Next(slaveSpawnSpreadMin, slaveSpawnSpreadMax);

            for (int j = 0; j < CurrentMap.Players.Count; j++)
            {
                Point playerLocation = CurrentMap.Players[j].CurrentLocation;

                Point location = new Point(playerLocation.X + Envir.Random.Next(-distance, distance + 1),
                                             playerLocation.Y + Envir.Random.Next(-distance, distance + 1));

                for (int i = 0; i < count; i++)
                {
                    MonsterObject mob = null;
                    switch (Envir.Random.Next(4))
                    {
                        case 0:
                            mob = GetMonster(Envir.GetMonsterInfo(Settings.TreeQueenMob1)); // RhinoWarrior
                            break;
                        case 1:
                            mob = GetMonster(Envir.GetMonsterInfo(Settings.TreeQueenMob2)); // RhinoPriest
                            break;
                        case 2:
                            mob = GetMonster(Envir.GetMonsterInfo(Settings.TreeQueenMob3)); // RockGuard
                            break;
                    }

                    if (mob == null) return;

                    mob.Spawn(CurrentMap, location);

                    mob.ActionTime = Envir.Time + 2000;
                    SlaveList.Add(mob);
                }
            }
        }

        public void Rage()
        {
            if (Dead) return;

            if (Stats[Stat.HP] >= 7)
            {
                _rageTime = Envir.Time + _rageDelay;
                _raged = true;

                Broadcast(GetInfo());
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

        protected override void ProcessTarget()
        {

            if (CurrentMap.Players.Count == 0) return;

            SpawnRoots();

            if (Envir.Time > _groundrootSpawnTime)
            {
                SpawnGroundRoots();
                _groundrootSpawnTime = Envir.Time + 5000;
            }


            if (!CanAttack) return;

            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }
        }

        public override void Spawned()
        {
            // Begin countdown timer
            SlaveSpawnTime = Envir.Time + (Settings.Second * 45);

            base.Spawned();
        }

        protected override void ProcessAI()
        {
            if (Dead) return;

            // After first 60 seconds: spawn mobs, then every 60 seconds after, spawn more mobs.
            if (Target != null && Envir.Time > SlaveSpawnTime)
            {
                SpawnSlaves();
                SlaveSpawnTime = Envir.Time + (Settings.Second * 45);
            }

            base.ProcessAI();
        }
    }
}
