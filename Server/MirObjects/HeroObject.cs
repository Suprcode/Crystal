using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using C = ClientPackets;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirNetwork;
using S = ServerPackets;
using System.Text.RegularExpressions;
using Server.MirObjects.Monsters;

namespace Server.MirObjects
{
    public class HeroObject : HumanObject
    {
        public override ObjectType Race
        {
            get { return ObjectType.Hero; }
        }

        public override MirConnection Connection
        {
            get { return connection; }
            set { throw new NotSupportedException(); }
        }

        new PlayerObject Owner;
        public override int PotionBeltMinimum => 0;
        public override int PotionBeltMaximum => 2;
        public override int AmuletBeltMinimum => 1;
        public override int AmuletBeltMaximum => 2;
        public override int BeltSize => 2;

        public const int SearchDelay = 3000, ViewRange = 8, RoamDelay = 1000;
        public long RoamTime;
        public override GuildObject MyGuild
        {
            get { return Owner.MyGuild; }
            set { throw new NotSupportedException(); }
        }

        public override MapObject Master
        {
            get { return Owner; }
            set { Owner = (PlayerObject)value; }
        }

        public HeroObject(CharacterInfo info, PlayerObject owner)
        {
            Owner = owner;            
            Load(info, null);           
        }

        protected override void Load(CharacterInfo info, MirConnection connection)
        {
            info.Mount = new MountInfo(this);

            Info = info;

            Stats = new Stats();            

            if (Level == 0) NewCharacter();

            RefreshStats();
            SendInfo();
            if (HP == 0)
            {
                SetHP(Stats[Stat.HP]);
                SetMP(Stats[Stat.MP]);
            }
        }

        public override void Enqueue(Packet p) { }

        public override void RefreshNameColour()
        {
            Color colour = Color.MediumOrchid;

            if (colour == NameColour) return;

            NameColour = colour;
            if ((Owner.MyGuild == null) || (!Owner.MyGuild.IsAtWar()))
                Enqueue(new S.ColourChanged { NameColour = NameColour });

            BroadcastColourChange();
        }

        public void Spawn(Map map, Point p)
        {
            CurrentLocation = p;
            map.AddObject(this);
            CurrentMap = map;            
            Envir.Heroes.Add(this);
            Spawned();
        }

        public override void Despawn()
        {
            Envir.Heroes.Remove(this);
            CurrentMap.RemoveObject(this);
            Master = null;

            base.Despawn();

            Info.Player = null;
            Info = null;                              
        }

        public override void Spawned()
        {
            base.Spawned();

            BroadcastHealthChange();
            BroadcastManaChange();
        }

        protected virtual void GetItemInfo()
        {
            UserItem item;
            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                item = Info.Inventory[i];
                if (item == null) continue;

                Owner.CheckItem(item);
            }

            for (int i = 0; i < Info.Equipment.Length; i++)
            {
                item = Info.Equipment[i];

                if (item == null) continue;

                Owner.CheckItem(item);
            }
        }
        public override void Die()
        {
            if (SpecialMode.HasFlag(SpecialItemMode.Revival) && Envir.Time > LastRevivalTime)
            {
                LastRevivalTime = Envir.Time + 300000;

                for (var i = (int)EquipmentSlot.RingL; i <= (int)EquipmentSlot.RingR; i++)
                {
                    var item = Info.Equipment[i];

                    if (item == null) continue;
                    if (!(item.Info.Unique.HasFlag(SpecialItemMode.Revival)) || item.CurrentDura < 1000) continue;
                    SetHP(Stats[Stat.HP]);
                    item.CurrentDura = (ushort)(item.CurrentDura - 1000);
                    Enqueue(new S.DuraChanged { UniqueID = item.UniqueID, CurrentDura = item.CurrentDura });
                    RefreshStats();
                    ReceiveChat("You have been given a second chance at life", ChatType.System);
                    return;
                }
            }

            for (int i = Pets.Count - 1; i >= 0; i--)
            {
                if (Pets[i].Dead) continue;
                Pets[i].Die();
            }

            RemoveBuff(BuffType.MagicShield);
            RemoveBuff(BuffType.ElementalBarrier);

            if (!InSafeZone)
                DeathDrop(LastHitter);

            HP = 0;
            Dead = true;

            LogTime = Envir.Time;
            BrownTime = Envir.Time;

            Broadcast(new S.ObjectDied { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            for (int i = 0; i < Buffs.Count; i++)
            {
                Buff buff = Buffs[i];

                if (!buff.Properties.HasFlag(BuffProperty.RemoveOnDeath)) continue;

                RemoveBuff(buff.Type);
            }

            PoisonList.Clear();
            InTrapRock = false;
        }

        public override void BroadcastHealthChange()
        {
            byte time = Math.Min(byte.MaxValue, (byte)Math.Max(5, (RevTime - Envir.Time) / 1000));
            Packet p = new S.ObjectHealth { ObjectID = ObjectID, Percent = PercentHealth, Expire = time };

            if (Envir.Time < RevTime)
            {
                CurrentMap.Broadcast(p, CurrentLocation);
                return;
            }

            Owner.Enqueue(p);

            if (Owner.GroupMembers != null)
            {
                for (int i = 0; i < Owner.GroupMembers.Count; i++)
                {
                    PlayerObject member = Owner.GroupMembers[i];

                    if (Master == member) continue;

                    if (member.CurrentMap != CurrentMap || !Functions.InRange(member.CurrentLocation, CurrentLocation, Globals.DataRange)) continue;
                    member.Enqueue(p);
                }
            }
        }

        public byte PercentMana
        {
            get { return (byte)(MP / (float)Stats[Stat.MP] * 100); }
        }

        public void BroadcastManaChange()
        {
            Packet p = new S.ObjectMana { ObjectID = ObjectID, Percent = PercentMana };
            Owner.Enqueue(p);
        }

        public override void Process(DelayedAction action)
        {
            if (action.FlaggedToRemove)
                return;

            switch (action.Type)
            {
                case DelayedType.Magic:
                    CompleteMagic(action.Params);
                    break;
                case DelayedType.Damage:
                    CompleteAttack(action.Params);
                    break;
                case DelayedType.Mine:
                    CompleteMine(action.Params);
                    break;
                case DelayedType.Poison:
                    CompletePoison(action.Params);
                    break;
                case DelayedType.DamageIndicator:
                    CompleteDamageIndicator(action.Params);
                    break;
                case DelayedType.SpellEffect:
                    CompleteSpellEffect(action.Params);
                    break;
            }
        }

        public override void Process()
        {
            base.Process();

            if (Target != null && (Target.CurrentMap != CurrentMap || !Target.IsAttackTarget(this) || !Functions.InRange(CurrentLocation, Target.CurrentLocation, Globals.DataRange)))
                Target = null;

            ProcessAI();
        }

        protected void ProcessAI()
        {           
            if (Dead) return;

            if (Master != null)
            {
                if ((Master.PMode == PetMode.Both || Master.PMode == PetMode.MoveOnly))
                {
                    if (!Functions.InRange(CurrentLocation, Master.CurrentLocation, Globals.DataRange) || CurrentMap != Master.CurrentMap)
                        OwnerRecall();
                }

                if (Master.PMode == PetMode.MoveOnly || Master.PMode == PetMode.None)
                    Target = null;
            }

            ProcessStacking();
            ProcessSearch();
            ProcessRoam();
            ProcessTarget();
        }

        protected virtual void ProcessStacking()
        {
            Stacking = CheckStacked();

            if (CanMove && ((Owner != null && Owner.Front == CurrentLocation) || Stacking))
            {
                if (!Walk(Direction))
                {
                    MirDirection dir = Direction;

                    switch (Envir.Random.Next(3)) 
                    {
                        case 0:
                            for (int i = 0; i < 7; i++)
                            {
                                dir = Functions.NextDir(dir);

                                if (Walk(dir))
                                    break;
                            }
                            break;
                        default:
                            for (int i = 0; i < 7; i++)
                            {
                                dir = Functions.PreviousDir(dir);

                                if (Walk(dir))
                                    break;
                            }
                            break;
                    }
                }

                return;
            }
        }

        protected virtual void ProcessSearch()
        {
            if (Envir.Time < SearchTime) return;
            if (Owner != null && (Owner.PMode == PetMode.MoveOnly || Owner.PMode == PetMode.None)) return;

            SearchTime = Envir.Time + SearchDelay;

            if (Target == null || Envir.Random.Next(3) == 0)
                FindTarget();
        }

        protected virtual void ProcessRoam()
        {
            if (Target != null || Envir.Time < RoamTime) return;

            if (Owner != null)
            {
                MoveTo(Owner.Back);
                return;
            }

            RoamTime = Envir.Time + RoamDelay;
        }

        protected virtual void ProcessTarget()
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

            MoveTo(Target.CurrentLocation);
        }

        protected virtual void Attack()
        {
            if (!Target.IsAttackTarget(Owner))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility, true);
            ActionList.Add(action);
        }

        protected virtual bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }

        protected virtual void MoveTo(Point location)
        {
            if (CurrentLocation == location) return;

            bool inRange = Functions.InRange(location, CurrentLocation, 1);

            if (inRange)
            {
                if (!CurrentMap.ValidPoint(location)) return;
                Cell cell = CurrentMap.GetCell(location);
                if (cell.Objects != null)
                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if (!ob.Blocking) continue;
                        return;
                    }
            }

            MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, location);

            if (!inRange && _stepCounter > 0 && Run(dir))
                return;

            if (Walk(dir)) return;

            switch (Envir.Random.Next(2))
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

        public void OwnerRecall()
        {
            if (Owner == null) return;
            if (!Teleport(Owner.CurrentMap, Owner.Back))
                Teleport(Owner.CurrentMap, Owner.CurrentLocation);
        }        

        protected virtual void FindTarget()
        {
            Map Current = CurrentMap;

            for (int d = 0; d <= ViewRange; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= Current.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= Current.Width) break;
                        Cell cell = Current.Cells[x, y];
                        if (cell.Objects == null || !cell.Valid) continue;
                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                case ObjectType.Hero:
                                    if (ob is TownArcher) continue;
                                    if (!ob.IsAttackTarget(Owner)) continue;
                                    if (ob.Hidden && (!CoolEye || Level < ob.Level)) continue;
                                    Target = ob;
                                    return;
                                case ObjectType.Player:
                                    PlayerObject playerob = (PlayerObject)ob;
                                    if (!ob.IsAttackTarget(Owner)) continue;
                                    if (playerob.GMGameMaster || ob.Hidden && (!CoolEye || Level < ob.Level)) continue;

                                    Target = ob;

                                    if (Owner != null)
                                    {
                                        for (int j = 0; j < playerob.Pets.Count; j++)
                                        {
                                            MonsterObject pet = playerob.Pets[j];

                                            if (!pet.IsAttackTarget(this)) continue;
                                            Target = pet;
                                            break;
                                        }
                                    }
                                    return;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
        }
        public override bool IsAttackTarget(HumanObject attacker)
        {
            return Owner.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(MonsterObject attacker)
        {            
            return Owner.IsAttackTarget(attacker);
        }
        public override bool IsFriendlyTarget(HumanObject ally)
        {
            return Owner.IsFriendlyTarget(ally);
        }
        public override bool IsFriendlyTarget(MonsterObject ally)
        {
            return Owner.IsFriendlyTarget(ally);
        }

        private void SendInfo()
        {
            GetItemInfo();
            S.HeroInformation packet = new S.HeroInformation
            {
                ObjectID = ObjectID,
                Name = Name,
                Class = Class,
                Gender = Gender,
                Level = Level,
                Hair = Hair,

                Experience = Experience,
                MaxExperience = MaxExperience,

                Inventory = new UserItem[Info.Inventory.Length],
                Equipment = new UserItem[Info.Equipment.Length],
            };

            for (int i = 0; i < Info.Magics.Count; i++)
                packet.Magics.Add(Info.Magics[i].CreateClientMagic());

            Info.Inventory.CopyTo(packet.Inventory, 0);
            Info.Equipment.CopyTo(packet.Equipment, 0);

            Owner.Enqueue(packet);
        }

        public override Packet GetInfo()
        {
            return new S.ObjectHero
            {
                ObjectID = ObjectID,
                Name = CurrentMap.Info.NoNames ? "?????" : Name,
                NameColour = NameColour,
                Class = Class,
                Gender = Gender,
                Level = Level,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = Hair,
                Weapon = Looks_Weapon,
                WeaponEffect = Looks_WeaponEffect,
                Armour = Looks_Armour,
                Light = Light,
                Poison = CurrentPoison,
                Dead = Dead,
                Hidden = Hidden,
                Effect = HasBuff(BuffType.MagicShield, out _) ? SpellEffect.MagicShieldUp : HasBuff(BuffType.ElementalBarrier, out _) ? SpellEffect.ElementalBarrierUp : SpellEffect.None,
                WingEffect = Looks_Wings,
                MountType = Mount.MountType,
                RidingMount = RidingMount,

                TransformType = TransformType,

                ElementOrbEffect = (uint)GetElementalOrbCount(),
                ElementOrbLvl = (uint)ElementsLevel,
                ElementOrbMax = (uint)Settings.OrbsExpList[Settings.OrbsExpList.Count - 1],

                Buffs = Buffs.Where(d => d.Info.Visible).Select(e => e.Type).ToList(),

                LevelEffects = LevelEffects,
                OwnerName = Owner.Name
            };
        }
    }
}
