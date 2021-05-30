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
        private int _rootCount = 1;
        private long _rootSpawnTime;

        private int _groundrootSpreadMin = 5;
        private int _groundrootSpreadMax = 25;
        private int _groundrootCount = 1;
        private long _groundRootSpawnTime;


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
                if (Envir.Random.Next(2) > 0)
                {
                        // Fire Bombardment Spell
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                        int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                        if (damage == 0) return;

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, true, false);
                        ActionList.Add(action);                    
                }
                else
                {
                    // Push Attack
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false, true);
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

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, true);
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
            bool fireBombardment = (bool)data[3];
            bool pushAttack = (bool)data[4];

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

            if (pushAttack)
            {
                List<MapObject> targets = FindAllTargets(1, CurrentLocation);
                if (targets.Count == 0) return;
                for (int i = 0; i < targets.Count; i++)
                {
                    Target = targets[i];
                    Target.Pushed(this, Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation), 5);
                }
            }
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool massRoots = (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (massRoots)
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

                    if (Envir.Random.Next(3) == 0)
                    {
                        location = playerLocation;
                    }

                    if (!CurrentMap.ValidPoint(location)) continue;

                    SpellObject spellObj = new SpellObject
                    {
                        Spell = Spell.TreeQueenRoot,
                        Value = Envir.Random.Next(Envir.Random.Next(Stats[Stat.MinDC], Stats[Stat.MaxDC])),
                        ExpireTime = Envir.Time + (1000),
                        TickSpeed = 2000,
                        Caster = null,
                        CurrentLocation = location,
                        CurrentMap = CurrentMap,
                        Direction = MirDirection.Up
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + Envir.Random.Next(2000), spellObj);
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

                    if (Envir.Random.Next(3) == 0)
                    {
                        location = playerLocation;
                    }

                    if (!CurrentMap.ValidPoint(location)) continue;

                    SpellObject spellObj = null;

                    spellObj = new SpellObject
                    {
                        Spell = Spell.TreeQueenGroundRoots,
                        Value = Envir.Random.Next(Envir.Random.Next(Stats[Stat.MinDC], Stats[Stat.MaxDC])),
                        ExpireTime = Envir.Time + (3000),
                        TickSpeed = 4000,
                        Caster = null,
                        CurrentLocation = location,
                        CurrentMap = CurrentMap,
                        Direction = MirDirection.Up
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + Envir.Random.Next(4000), spellObj);
                    CurrentMap.ActionList.Add(action);
                }
            }
        }

        public void Rage()
        {
            if (Dead) return;

            if (Stats[Stat.HP] >= 4)
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


        public override void Spawned()
        {
            // Begin timers (stops players from being bombarded with attacks when they enter the room / map).
            _rootSpawnTime = Envir.Time + (Settings.Second * 8);
            _groundRootSpawnTime = Envir.Time + (Settings.Second * 19);

            base.Spawned();
        }

        protected override void ProcessTarget()
        {

            if (CurrentMap.Players.Count == 0) return;

            if(Envir.Time > _rootSpawnTime)
            {
                SpawnRoots();
                _rootSpawnTime = Envir.Time + 10000;
            }            

            if (Envir.Time > _groundRootSpawnTime)
            {
                SpawnGroundRoots();
                _groundRootSpawnTime = Envir.Time + 15000;
            }

            if (!CanAttack) return;

            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }
        }

    }
}
