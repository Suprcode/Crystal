using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects.Monsters;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class HumanWarrior : MonsterObject
    {
        public long NextTDBTime, NextFlamingSwordTime, _RageTime;
        public long CastTime, SpellCastTime;
        public bool Casting = false;
        public int OriginalAttackSpeed;
        public MirClass mobsClass;
        public MirGender mobsGender;
        public short weapon, armour;
        public byte wing, hair, light;
        public bool MagicShieldUp = false;

        public HumanWarrior(MonsterInfo info)
            : base(info)
        {
            GetHumanInfo();
            Direction = MirDirection.Down;
            OriginalAttackSpeed = Info.AttackSpeed;
        }
        
        public override bool Walk(MirDirection dir)
        {
            if (!CanMove) return false;

            Point location = Functions.PointMove(CurrentLocation, dir, 2);

            if (!CurrentMap.ValidPoint(location)) return false;

            Cell cell = CurrentMap.GetCell(location);

            bool isBreak = false;

            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (!ob.Blocking) continue;
                    isBreak = true;
                    break;
                }

            if (isBreak)
            {
                location = Functions.PointMove(CurrentLocation, dir, 1);

                if (!CurrentMap.ValidPoint(location)) return false;

                cell = CurrentMap.GetCell(location);

                if (cell.Objects != null)
                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if (!ob.Blocking) continue;
                        return false;
                    }
            }

            CurrentMap.GetCell(CurrentLocation).Remove(this);

            Direction = dir;
            RemoveObjects(dir, 1);
            CurrentLocation = location;
            CurrentMap.GetCell(CurrentLocation).Add(this);
            AddObjects(dir, 1);

            CellTime = Envir.Time + 500;
            ActionTime = Envir.Time + 300;
            MoveTime = Envir.Time + MoveSpeed;
            if (MoveTime > AttackTime)
                AttackTime = MoveTime;

            InSafeZone = CurrentMap.GetSafeZone(CurrentLocation) != null;

            if (isBreak)
                Broadcast(new S.ObjectWalk { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            else
                Broadcast(new S.ObjectRun { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            cell = CurrentMap.GetCell(CurrentLocation);

            for (int i = 0; i < cell.Objects.Count; i++)
            {
                if (cell.Objects[i].Race != ObjectType.Spell) continue;
                SpellObject ob = (SpellObject)cell.Objects[i];

                ob.ProcessSpell(this);
                //break;
            }

            return true;
        }
        
        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            return base.Attacked(attacker, damage, type);
        }

        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            return base.Attacked(attacker, damage, type, damageWeapon);
        }

        /// <summary>
        /// Override the ProcessTarget in order to setup the AI.
        /// </summary>
        protected override void ProcessTarget()
        {
            //  Ensure we're not trying to attack and invalid Target (Dead or non existent)
            if (Target == null || Target.Dead)
            {
                FindTarget();
                return;
            }

            //  Get the Direction to face and use on the ObjectAttack Packet
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            List<MapObject> targets = FindAllTargets(1, CurrentLocation, false);
            int closeTargets = targets.Count;
            if (Envir.Time > AttackTime)
            {
                AttackTime = Envir.Time + AttackSpeed;
                ActionTime = Envir.Time + 300;

                if (Envir.Time > _RageTime)
                    AttackSpeed = OriginalAttackSpeed;
                if (InAttackRange())
                {
                    if (Envir.Time > NextFlamingSwordTime)
                    {
                        NextFlamingSwordTime = Envir.Time + Settings.Second * Envir.Random.Next(15, 25);
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                        PerformFlamingSword();
                        return;
                    }
                    else if (closeTargets <= 1)
                    {
                        NextTDBTime = Envir.Time + Settings.Second * Envir.Random.Next(10, 20);
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                        PerformTwinDrakeBlade();
                        return;
                    }
                    else if (closeTargets >= 2)
                    {
                        if (Envir.Random.Next(0, 10) >= 5)
                        {
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                            PerformCrossHalfMoon();
                            return;
                        }
                        else
                        {
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                            PerformHalfmoon();
                            return;
                        }
                    }
                    else if (GetHitCount() == 2)
                    {
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                        PerformThrusting();
                        return;
                    }
                    else if (Functions.InRange(CurrentLocation, Target.CurrentLocation, 1))
                    {
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                        Attack();
                        return;
                    }
                }
            }
            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            //  If we're not in Range, move to the target
            if (!InAttackRange())
                MoveTo(Target.CurrentLocation);
            //  If the targets invalid or dead, find a new one.
            if (Target == null || Target.Dead)
            {
                FindTarget();
                return;
            }
            MoveTo(Target.CurrentLocation);
        }

        #region Warrior
        public int GetHitCount()
        {
            int count = 0;
            if (Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) <= 2)
                if (Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) == 1)
                {
                    Point tmmp = Functions.PointMove(CurrentLocation, Direction, 2);
                    if (CurrentMap.ValidPoint(tmmp))
                    {
                        Cell cell = CurrentMap.GetCell(tmmp);
                        if (cell.Objects != null)
                            for (int i = 0; i < cell.Objects.Count; i++)
                                if ((cell.Objects[i].Race == ObjectType.Player ||
                                    cell.Objects[i].Race == ObjectType.Monster) &&
                                    cell.Objects[i].IsAttackTarget(this))
                                    count = 2;
                    }
                }
                else
                    count = 1;
            return count;
        }
        public void PerformFlamingSword()
        {
            int damage = GetAttackPower(Stats[Stat.MinDC] * 2, Stats[Stat.MaxDC] * 2);
            Target.Attacked(this, damage, DefenceType.AC);
            //Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.HumanFlamingSword });
        }

        public void PerformTwinDrakeBlade()
        {
            int damage = GetReducedAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            Target.Attacked(this, damage, DefenceType.ACAgility);
            damage = GetReducedAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 800, Target, damage, DefenceType.ACAgility);
            ActionList.Add(action);
            //Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.HumanTwinDrakeBlade });
        }

        public int GetReducedAttackPower(int min, int max)
        {
            int damage = GetAttackPower(min, max);
            float tmp = damage / 0.75f;
            damage = (int)tmp;
            return damage;
        }

        public void PerformHalfmoon()
        {
            MirDirection dir = Functions.PreviousDir(Direction);
            for (int i = 0; i < 3; i++)
            {
                Point location = Functions.PointMove(CurrentLocation, dir, 1);
                if (!CurrentMap.ValidPoint(location))
                    continue;
                Cell cell = CurrentMap.GetCell(location);
                if (cell != null &&
                    cell.Objects != null)
                {
                    for (int x = 0; x < cell.Objects.Count; x++)
                    {
                        if (cell.Objects[x].Race == ObjectType.Player ||
                            cell.Objects[x].Race == ObjectType.Monster)
                        {
                            if (cell.Objects[x].IsAttackTarget(this))
                            {
                                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                                cell.Objects[x].Attacked(this, damage, DefenceType.ACAgility);
                            }
                        }
                    }
                }
                dir = Functions.NextDir(dir);
            }
            //Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.HumanHalfMoon });
        }

        public void PerformCrossHalfMoon()
        {
            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            List<MapObject> targets = FindAllTargets(1, CurrentLocation, false);
            for (int i = 0; i < targets.Count; i++)
                if (targets[i].IsAttackTarget(this))
                    targets[i].Attacked(this, damage, DefenceType.ACAgility);
            //Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.HumanCrossHalfMoon });
        }

        public void PerformThrusting()
        {
            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0)
                return;

            for (int i = 1; i <= 2; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (target == Target.CurrentLocation)
                {
                    if (Target.Attacked(this, damage, DefenceType.MACAgility) > 0 && Envir.Random.Next(8) == 0)
                    {
                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
                        {
                            int poison = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]);

                            Target.ApplyPoison(new Poison
                            {
                                Owner = this,
                                Duration = 5,
                                PType = PoisonType.Green,
                                Value = poison,
                                TickSpeed = 2000
                            }, this);
                        }
                    }
                }
                else
                {
                    if (!CurrentMap.ValidPoint(target))
                        continue;

                    Cell cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null)
                        continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                        {
                            if (!ob.IsAttackTarget(this))
                                continue;

                            if (ob.Attacked(this, damage, DefenceType.MACAgility) > 0 && Envir.Random.Next(8) == 0)
                            {
                                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
                                {
                                    int poison = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]);

                                    ob.ApplyPoison(new Poison
                                    {
                                        Owner = this,
                                        Duration = 5,
                                        PType = PoisonType.Green,
                                        Value = poison,
                                        TickSpeed = 2000
                                    }, this);
                                }
                            }
                        }
                        else
                            continue;

                        break;
                    }
                }
            }
            //Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.HumanThrusting });
        }
        #endregion

        public override void Die()
        {
            if (Dead) return;

            HP = 0;
            Dead = true;

            //DeadTime = Envir.Time + DeadDelay;
            DeadTime = 0;

            Broadcast(new S.ObjectDied { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = (byte)(Master != null ? 1 : 0) });

            if (EXPOwner != null && Master == null && EXPOwner.Race == ObjectType.Player) EXPOwner.WinExp(Experience);

            if (Respawn != null)
                Respawn.Count--;

            if (Master == null)
                Drop();

            PoisonList.Clear();
            Envir.MonsterCount--;
            CurrentMap.MonsterCount--;
        }
        public void GetHumanInfo()
        {
            if (Settings.HumMobs != null && Settings.HumMobs.Count > 0)
            {
                for (int i = 0; i < Settings.HumMobs.Count; i++)
                {
                    if (Settings.HumMobs[i].HumansName.ToLower() == Info.Name.ToLower())
                    {
                        mobsClass = Settings.HumMobs[i].MobsClass;
                        mobsGender = Settings.HumMobs[i].MobsGender;
                        weapon = Settings.HumMobs[i].Weapon;
                        armour = Settings.HumMobs[i].Armour;
                        wing = Settings.HumMobs[i].Wing;
                        hair = Settings.HumMobs[i].Hair;
                        light = Settings.HumMobs[i].Light;
                    }
                }
            }
        }
        public override Packet GetInfo()
        {
            GetHumanInfo();
            if (weapon < 0)
                weapon = 0;
            if (armour < 0)
                armour = 0;
            if (wing < 0)
                wing = 0;
            if (hair < 0)
                hair = 0;
            if (light < 0)
                light = 0;
            return new S.ObjectPlayer
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Class = mobsClass,
                Gender = mobsGender,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = hair,
                Weapon = weapon,
                Armour = armour,
                Light = light,
                Poison = CurrentPoison,
                Dead = Dead,
                Hidden = Hidden,
                Effect = SpellEffect.None,
                WingEffect = wing,
                TransformType = -1
            };
        }
    }
}