using System;
using System.Collections.Generic;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Linq;

namespace Server.MirObjects.Monsters
{
    public class HumanTaoist : MonsterObject
    {
        public long PoisonCloudTime, PoisoningTime, CurseTime, SoulFireBallTime, BlessedArmourTime, SoulShieldTime, UltimateEnhancerTime;
        public long CastTime;
        public bool Casting = false;

        public MirClass mobsClass;
        public MirGender mobsGender;
        public short weapon, armour;
        public byte wing, hair, light;

        public long FearTime;
        public byte AttackRange = 9;

        protected internal HumanTaoist(MonsterInfo info)
            : base(info)
        {
            GetHumanInfo();
            Direction = MirDirection.Down;
        }
        protected virtual bool InHelpRange(MapObject target)
        {
            if (target.CurrentMap != CurrentMap) return false;

            return target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, target.CurrentLocation, 15);
        }
        public PlayerObject PlayerMaster
        {
            get
            {
                return Master != null ? ((PlayerObject)Master) : null;
            }
        }
        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }
        /*
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
        */
        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (Master != null)
                MoveTo(Master.CurrentLocation);

            if (InAttackRange() && (Master != null || Envir.Time < FearTime))
            {
                if (!Casting)
                {
                    if (IsFriendlyTarget(PlayerMaster) && Envir.Time > SoulShieldTime)
                    {
                        Casting = true;
                        SoulShieldTime = Envir.Time + Settings.Second * 70;
                        PerformSoulShield();
                    }
                    else if (IsFriendlyTarget(PlayerMaster) && Envir.Time > BlessedArmourTime)
                    {
                        Casting = true;
                        BlessedArmourTime = Envir.Time + Settings.Second * 65;
                        PerformBlessedArmour();
                    }
                    else if (IsFriendlyTarget(PlayerMaster) && Envir.Time > UltimateEnhancerTime)
                    {
                        Casting = true;
                        UltimateEnhancerTime = Envir.Time + Settings.Second * 60;
                        PerformUltimateEnhancer();
                    }
                    else if (Envir.Time > PoisonCloudTime)
                    {
                        Casting = true;
                        PoisonCloudTime = Envir.Time + Settings.Second * 15;
                        PerformPoisonCloud();
                    }
                    else if (Envir.Time > PoisoningTime)
                    {
                        PoisoningTime = Envir.Time + Settings.Second * 10;
                        PerformPoisoning();
                    }
                    else if (Envir.Time > SoulFireBallTime)
                    {
                        SoulFireBallTime = Envir.Time + Settings.Second * 2;
                        PerformSoulFireBall();
                    }
                }
                else if (Envir.Time > CastTime)
                    Casting = false;
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

                if (Walk(dir))
                    return;

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

        #region Taoist
        public void PerformSoulShield()
        {
            MapObject target = null;
            if (!PlayerMaster.Buffs.Any(x => x.Type == BuffType.SoulShield) && (InHelpRange(PlayerMaster)))
            {
                target = PlayerMaster;
            }
            else if (!Buffs.Any(x => x.Type == BuffType.SoulShield))
            {
                target = this;
            }
            else if (PlayerMaster.GroupMembers != null)
            {
                var p = PlayerMaster.GroupMembers.FirstOrDefault(x => !x.Buffs.Any(y => y.Type == BuffType.SoulShield) && (InHelpRange(x)));
                target = p;
            }
            int value = (int)Math.Round(Stats[Stat.MaxMAC] * (0.04 + (0.02 * 3)));
            if (target != null)
            {
                List<MapObject> targets = FindAllNearby(4, Master.CurrentLocation, false);
                for (int i = 0; i < targets.Count; i++)
                   // if (targets[i].Race.In(ObjectType.Player) && targets[i].IsFriendlyTarget(PlayerMaster))
                    //    targets[i].AddBuff(new Buff { Type = BuffType.SoulShield, Caster = this, ExpireTime = Envir.Time + 60 * 1000, ObjectID = targets[i].ObjectID, Values = new int[] { value } });
                Direction = Functions.DirectionFromPoint(CurrentLocation, Master.CurrentLocation);
                Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.SoulShield/*, TargetID = trg.ObjectID*/, Target = Master.CurrentLocation, Cast = true, Level = 3 });

                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                return;
            }
        }
        public void PerformBlessedArmour()
        {
            MapObject target = null;
            if (!PlayerMaster.Buffs.Any(x => x.Type == BuffType.BlessedArmour) && (InHelpRange(PlayerMaster)))
            {
                target = PlayerMaster;
            }
            else if (!Buffs.Any(x => x.Type == BuffType.BlessedArmour))
            {
                target = this;
            }
            else if (PlayerMaster.GroupMembers != null)
            {
                var p = PlayerMaster.GroupMembers.FirstOrDefault(x => !x.Buffs.Any(y => y.Type == BuffType.BlessedArmour) && (InHelpRange(x)));
                target = p;
            }
            int value = (int)Math.Round(Stats[Stat.MaxAC] * (0.04 + (0.02 * 3)));
            if (target != null)
            {
                List<MapObject> targets = FindAllNearby(4, Master.CurrentLocation, false);
                for (int i = 0; i < targets.Count; i++)
                    //if (targets[i].Race.In(ObjectType.Player) && targets[i].IsFriendlyTarget(PlayerMaster))
                        //targets[i].AddBuff(new Buff { Type = BuffType.BlessedArmour, Caster = this, ExpireTime = Envir.Time + 60 * 1000, ObjectID = targets[i].ObjectID, Values = new int[] { value } });
                Direction = Functions.DirectionFromPoint(CurrentLocation, Master.CurrentLocation);
                Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.BlessedArmour/*, TargetID = trg.ObjectID*/, Target = Master.CurrentLocation, Cast = true, Level = 3 });

                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                return;
            }
        }
        public void PerformUltimateEnhancer()
        {
            MapObject target = null;
            if (!PlayerMaster.Buffs.Any(x => x.Type == BuffType.UltimateEnhancer) && (InHelpRange(PlayerMaster)))
            {
                target = PlayerMaster;
            }
            else if (!Buffs.Any(x => x.Type == BuffType.UltimateEnhancer))
            {
                target = this;
            }
            else if (PlayerMaster.GroupMembers != null)
            {
                var p = PlayerMaster.GroupMembers.FirstOrDefault(x => !x.Buffs.Any(y => y.Type == BuffType.UltimateEnhancer) && (InHelpRange(x)));
                target = p;
            }
            int value = (int)Math.Round(Stats[Stat.MaxSC] * (0.04 + (0.02 * 3)));
            if (target != null)
            {
                //target.AddBuff(new Buff { Type = BuffType.UltimateEnhancer, Caster = this, ExpireTime = Envir.Time + 60 * 1000, ObjectID = target.ObjectID, Values = new int[] { value } });
                Direction = Functions.DirectionFromPoint(CurrentLocation, target.CurrentLocation);
                Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.UltimateEnhancer, TargetID = target.ObjectID, Target = target.CurrentLocation, Cast = true, Level = 3 });
                //Broadcast(new S.ObjectEffect { Effect = SpellEffect.UEnhance, ObjectID = target.ObjectID });

                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                return;
            }
        }
        public void PerformPoisonCloud()
        {
            //Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.PoisonCloud, Cast = true, Level = 3 });
            int damage = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]);
            //Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.HumanCastPoisonCloud });
            //DelayedAction action = new DelayedAction(DelayedType.MonsterMagic, Envir.Time + 500, this, Spell.MobPoisonCloud, damage, Target.CurrentLocation);
            //  Add the action to the current map
            //CurrentMap.ActionList.Add(action);
        }
        public void PerformSoulFireBall()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            int damage = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]);
            if (damage == 0)
                return;

            if (Envir.Random.Next(Settings.MagicResistWeight) >= Target.Stats[Stat.MagicResist])
            {
                int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MACAgility);
                ActionList.Add(action);
                Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.SoulFireBall, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });
            }
            if (Target.Dead)
                FindTarget();
        }
        public void PerformPoisoning()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.Poisoning, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });
            if (Envir.Random.Next(0, 100) > 50)
                Target.ApplyPoison(new Poison { PType = PoisonType.Green, Duration = 30, TickSpeed = 1000, Value = GetAttackPower(Stats[Stat.MinSC] / 10, Stats[Stat.MaxSC] / 10) }, this);
            else
                Target.ApplyPoison(new Poison { PType = PoisonType.Red, Duration = 30, TickSpeed = 1000, Value = GetAttackPower(Stats[Stat.MinSC] / 10, Stats[Stat.MaxSC] / 10) }, this);
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

            if (EXPOwner != null && Master == null && EXPOwner.Race == ObjectType.Player)
                EXPOwner.WinExp(Experience);

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
