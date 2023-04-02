using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirNetwork;
using S = ServerPackets;
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

        public HeroInfo HInfo;
        public new PlayerObject Owner;
        public override int PotionBeltMinimum => 0;
        public override int PotionBeltMaximum => 2;
        public override int AmuletBeltMinimum => 1;
        public override int AmuletBeltMaximum => 2;
        public override int BeltSize => 2;

        public override bool CanMove
        {
            get
            {
                return base.CanMove && !ActiveBlizzard && !ActiveReincarnation;
            }
        }
        public override bool CanWalk
        {
            get
            {
                return base.CanWalk && !ActiveBlizzard && !ActiveReincarnation;
            }
        }
        public override bool CanRun
        {
            get
            {
                return base.CanRun && !ActiveBlizzard && !ActiveReincarnation;
            }
        }
        public override bool CanAttack
        {
            get
            {
                return base.CanAttack && !ActiveBlizzard && !ActiveReincarnation;
            }
        }
        protected override bool CanCast
        {
            get
            {
                return base.CanCast && !ActiveBlizzard && !ActiveReincarnation;
            }
        }

        public const int SearchDelay = 3000, ViewRange = 8, RoamDelay = 1000, RevivalDelay = 2000, AutoPotDelay = 1000;
        public long RoamTime, AutoPotTime;

        public override long BrownTime
        {
            get { return Owner.BrownTime; }
            set { brownTime = value; }
        }
        public byte Grade
        {
            get { return HInfo.Grade; }
            set { HInfo.Grade = value; }
        }
        public bool AutoPot
        {
            get { return HInfo.AutoPot; }
            set { HInfo.AutoPot = value; }
        }

        public byte AutoHPPercent
        {
            get { return HInfo.AutoHPPercent; }
            set { HInfo.AutoHPPercent = value; }
        }

        public byte AutoMPPercent
        {
            get { return HInfo.AutoMPPercent; }
            set { HInfo.AutoMPPercent = value; }
        }

        public int HPItemIndex
        {
            get { return HInfo.HPItemIndex; }
            set { HInfo.HPItemIndex = value; }
        }

        public int MPItemIndex
        {
            get { return HInfo.MPItemIndex; }
            set { HInfo.MPItemIndex = value; }
        }

        protected Spell NextMagicSpell;
        protected MirDirection NextMagicDirection;
        protected uint NextMagicTargetID;
        protected Point NextMagicLocation;

        protected int TargetDistance;
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
            CheckCellTime = false;

            Owner = owner;

            base.Report = owner.Report;

            Load(info, null);           
        }

        protected override void Load(CharacterInfo info, MirConnection connection)
        {
            info.Mount = new MountInfo(this);

            Info = info;
            HInfo = (HeroInfo)info;

            Stats = new Stats();            

            if (Level == 0) NewCharacter();

            RefreshStats();
            SendInfo();

            switch (HP)
            {
                case 0:
                    Dead = true;
                    break;
                case -1:
                    SetHP(Stats[Stat.HP]);
                    SetMP(Stats[Stat.MP]);
                    break;
            }
        }
        protected override void NewCharacter()
        {
            base.NewCharacter();
            Grade = (byte)Envir.Random.Next(4);
        }

        public override void Enqueue(Packet p) 
        {
            if (p == null) return;

            switch ((ServerPacketIds)p.Index)
            {
                case ServerPacketIds.AddBuff:
                case ServerPacketIds.RemoveBuff:
                case ServerPacketIds.PauseBuff:
                case ServerPacketIds.MagicDelay:
                case ServerPacketIds.MagicLeveled:
                case ServerPacketIds.DeleteItem:
                case ServerPacketIds.UseItem:
                    Owner.Enqueue(p);
                    break;
            }
        }

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

        public void Despawn(bool cleanup)
        {
            Despawn();

            if (cleanup)
                CleanUp();
        }

        public override void Despawn()
        {
            if (Node != null)
            {
                Envir.Heroes.Remove(this);
                CurrentMap.RemoveObject(this);

                for (int i = Buffs.Count - 1; i >= 0; i--)
                {
                    var buff = Buffs[i];
                    buff.Caster = null;
                    buff.ObjectID = 0;

                    if (buff.Properties.HasFlag(BuffProperty.RemoveOnExit))
                    {
                        Buffs.RemoveAt(i);
                    }
                }

                base.Despawn();
            }            
        }

        protected override void CleanUp()
        {
            Owner = null;
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

            if (HPItemIndex > 0)
                Owner.CheckItemInfo(Envir.GetItemInfo(HPItemIndex));
            if (MPItemIndex > 0)
                Owner.CheckItemInfo(Envir.GetItemInfo(MPItemIndex));
        }
        public override void SendMagicInfo(UserMagic magic)
        {
            Owner.Enqueue(magic.GetInfo(true));
        }
        protected bool CanUseMagic(UserMagic magic)
        {
            if (magic == null) return false;
            if (MagicCost(magic) > MP) return false;
            if (magic.Key <= 0) return false;

            return true;
        }
        protected bool HasMagic(Spell spell) => Info.Magics.Any(x => x.Spell == spell);
        public override bool TryMagic()
        {
            return true;
        }
        public override void BeginMagic(Spell spell, MirDirection dir, uint targetID, Point location, bool spellTargetLock = false)
        {
            NextMagicSpell = spell;
            NextMagicDirection = dir;
            NextMagicTargetID = targetID;
            NextMagicLocation = location;
        }
        public override MapObject DefaultMagicTarget => Owner;
        public override void UseItem(ulong id)
        {
            S.UseItem p = new S.UseItem { UniqueID = id, Grid = MirGridType.HeroInventory, Success = false };

            UserItem item = null;
            int index = -1;

            if (Owner.Hero != null && Owner.Hero.Dead) return;

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                item = Info.Inventory[i];
                if (item == null || item.UniqueID != id) continue;
                index = i;
                break;
            }

            if (item == null || index == -1 || !CanUseItem(item))
            {
                Owner.Enqueue(p);
                return;
            }

            if (Dead && !(item.Info.Type == ItemType.Scroll && item.Info.Shape == 6))
            {
                Owner.Enqueue(p);
                return;
            }

            switch (item.Info.Type)
            {
                case ItemType.Potion:
                    switch (item.Info.Shape)
                    {
                        case 0: //NormalPotion
                            PotHealthAmount = (ushort)Math.Min(ushort.MaxValue, PotHealthAmount + item.Info.Stats[Stat.HP]);
                            PotManaAmount = (ushort)Math.Min(ushort.MaxValue, PotManaAmount + item.Info.Stats[Stat.MP]);
                            break;
                        case 1: //SunPotion
                            ChangeHP(item.Info.Stats[Stat.HP]);
                            ChangeMP(item.Info.Stats[Stat.MP]);
                            break;
                        case 2: //MysteryWater
                            if (UnlockCurse)
                            {
                                ReceiveChat("You can already unequip a cursed item.", ChatType.Hint);
                                Owner.Enqueue(p);
                                return;
                            }
                            ReceiveChat("You can now unequip a cursed item.", ChatType.Hint);
                            UnlockCurse = true;
                            break;
                        case 3: //Buff
                            {
                                int time = item.Info.Durability;

                                if (item.GetTotal(Stat.MaxDC) > 0)
                                    AddBuff(BuffType.Impact, this, time * Settings.Minute, new Stats { [Stat.MaxDC] = item.GetTotal(Stat.MaxDC) });

                                if (item.GetTotal(Stat.MaxMC) > 0)
                                    AddBuff(BuffType.Magic, this, time * Settings.Minute, new Stats { [Stat.MaxMC] = item.GetTotal(Stat.MaxMC) });

                                if (item.GetTotal(Stat.MaxSC) > 0)
                                    AddBuff(BuffType.Taoist, this, time * Settings.Minute, new Stats { [Stat.MaxSC] = item.GetTotal(Stat.MaxSC) });

                                if (item.GetTotal(Stat.AttackSpeed) > 0)
                                    AddBuff(BuffType.Storm, this, time * Settings.Minute, new Stats { [Stat.AttackSpeed] = item.GetTotal(Stat.AttackSpeed) });

                                if (item.GetTotal(Stat.HP) > 0)
                                    AddBuff(BuffType.HealthAid, this, time * Settings.Minute, new Stats { [Stat.HP] = item.GetTotal(Stat.HP) });

                                if (item.GetTotal(Stat.MP) > 0)
                                    AddBuff(BuffType.ManaAid, this, time * Settings.Minute, new Stats { [Stat.MP] = item.GetTotal(Stat.MP) });

                                if (item.GetTotal(Stat.MaxAC) > 0)
                                    AddBuff(BuffType.Defence, this, time * Settings.Minute, new Stats { [Stat.MaxAC] = item.GetTotal(Stat.MaxAC) });

                                if (item.GetTotal(Stat.MaxMAC) > 0)
                                    AddBuff(BuffType.MagicDefence, this, time * Settings.Minute, new Stats { [Stat.MaxMAC] = item.GetTotal(Stat.MaxMAC) });

                                if (item.GetTotal(Stat.BagWeight) > 0)
                                    AddBuff(BuffType.BagWeight, this, time * Settings.Minute, new Stats { [Stat.BagWeight] = item.GetTotal(Stat.BagWeight) });
                            }
                            break;
                        case 4: //Exp
                            {
                                int time = item.Info.Durability;
                                AddBuff(BuffType.Exp, this, Settings.Minute * time, new Stats { [Stat.ExpRatePercent] = item.GetTotal(Stat.Luck) });
                            }
                            break;
                        case 5: //Drop
                            {
                                int time = item.Info.Durability;
                                AddBuff(BuffType.Drop, this, Settings.Minute * time, new Stats { [Stat.ItemDropRatePercent] = item.GetTotal(Stat.Luck) });
                            }
                            break;
                    }
                    break;
                case ItemType.Scroll:
                    UserItem temp;
                    switch (item.Info.Shape)
                    {
                        case 3: //BenedictionOil
                            if (!TryLuckWeapon())
                            {
                                Owner.Enqueue(p);
                                return;
                            }
                            break;
                        case 4: //RepairOil
                            temp = Info.Equipment[(int)EquipmentSlot.Weapon];
                            if (temp == null || temp.MaxDura == temp.CurrentDura)
                            {
                                Owner.Enqueue(p);
                                return;
                            }
                            if (temp.Info.Bind.HasFlag(BindMode.DontRepair))
                            {
                                Owner.Enqueue(p);
                                return;
                            }
                            temp.MaxDura = (ushort)Math.Max(0, temp.MaxDura - Math.Min(5000, temp.MaxDura - temp.CurrentDura) / 30);

                            temp.CurrentDura = (ushort)Math.Min(temp.MaxDura, temp.CurrentDura + 5000);
                            temp.DuraChanged = false;

                            ReceiveChat("Your weapon has been partially repaired", ChatType.Hint);
                            Owner.Enqueue(new S.ItemRepaired { UniqueID = temp.UniqueID, MaxDura = temp.MaxDura, CurrentDura = temp.CurrentDura });
                            break;
                        case 5: //WarGodOil
                            temp = Info.Equipment[(int)EquipmentSlot.Weapon];
                            if (temp == null || temp.MaxDura == temp.CurrentDura)
                            {
                                Owner.Enqueue(p);
                                return;
                            }
                            if (temp.Info.Bind.HasFlag(BindMode.DontRepair) || (temp.Info.Bind.HasFlag(BindMode.NoSRepair)))
                            {
                                Owner.Enqueue(p);
                                return;
                            }
                            temp.CurrentDura = temp.MaxDura;
                            temp.DuraChanged = false;

                            ReceiveChat("Your weapon has been completely repaired", ChatType.Hint);
                            Owner.Enqueue(new S.ItemRepaired { UniqueID = temp.UniqueID, MaxDura = temp.MaxDura, CurrentDura = temp.CurrentDura });
                            break;
                        case 6: //ResurrectionScroll
                            if (CurrentMap.Info.NoReincarnation)
                            {
                                ReceiveChat(string.Format("Cannot use on this map"), ChatType.System);
                                Owner.Enqueue(p);
                                return;
                            }
                            if (Dead)
                            {
                                MP = Stats[Stat.MP];
                                Revive(MaxHealth, true);
                            }
                            break;
                    }
                    break;
                case ItemType.Book:
                    UserMagic magic = new UserMagic((Spell)item.Info.Shape);

                    if (magic.Info == null)
                    {
                        Owner.Enqueue(p);
                        return;
                    }

                    Info.Magics.Add(magic);
                    SendMagicInfo(magic);
                    RefreshStats();
                    break;
                case ItemType.Food:
                    temp = Info.Equipment[(int)EquipmentSlot.Mount];
                    if (temp == null || temp.MaxDura == temp.CurrentDura)
                    {
                        Owner.Enqueue(p);
                        return;
                    }

                    switch (item.Info.Shape)
                    {
                        case 0:
                            temp.MaxDura = (ushort)Math.Max(0, temp.MaxDura - Math.Min(1000, temp.MaxDura - (temp.CurrentDura / 30)));
                            break;
                        case 1:
                            break;
                    }

                    temp.CurrentDura = (ushort)Math.Min(temp.MaxDura, temp.CurrentDura + item.CurrentDura);
                    temp.DuraChanged = false;

                    ReceiveChat("Your mount has been fed.", ChatType.Hint);
                    Owner.Enqueue(new S.ItemRepaired { UniqueID = temp.UniqueID, MaxDura = temp.MaxDura, CurrentDura = temp.CurrentDura });

                    RefreshStats();
                    break;                
                case ItemType.Transform: //Transforms
                    {
                        AddBuff(BuffType.Transform, this, (Settings.Second * item.Info.Durability), new Stats(), values: item.Info.Shape);
                    }
                    break;
                case ItemType.Deco:

                    DecoObject decoOb = new DecoObject
                    {
                        Image = item.Info.Shape,
                        CurrentMap = CurrentMap,
                        CurrentLocation = CurrentLocation,
                    };

                    CurrentMap.AddObject(decoOb);
                    decoOb.Spawned();

                    Owner.Enqueue(decoOb.GetInfo());

                    break;
                case ItemType.MonsterSpawn:

                    var monsterID = item.Info.Stats[Stat.HP];
                    var spawnAsPet = item.Info.Shape == 1;
                    var conquestOnly = item.Info.Shape == 2;

                    var monsterInfo = Envir.GetMonsterInfo(monsterID);
                    if (monsterInfo == null) break;

                    MonsterObject monster = MonsterObject.GetMonster(monsterInfo);
                    if (monster == null) break;

                    if (spawnAsPet)
                    {
                        if (Pets.Count(t => !t.Dead && t.Race != ObjectType.Creature) >= Globals.MaxPets)
                        {
                            ReceiveChat("Maximum number of pets already reached.", ChatType.Hint);
                            Owner.Enqueue(p);
                            return;
                        }

                        monster.Master = this;
                        monster.PetLevel = 0;
                        monster.MaxPetLevel = 7;

                        Pets.Add(monster);
                    }

                    if (conquestOnly)
                    {
                        var con = CurrentMap.GetConquest(CurrentLocation);
                        if (con == null)
                        {
                            ReceiveChat(string.Format("{0} can only be spawned during a conquest.", monsterInfo.GameName), ChatType.Hint);
                            Owner.Enqueue(p);
                            return;
                        }
                    }

                    monster.Direction = Direction;
                    monster.ActionTime = Envir.Time + 5000;

                    if (!monster.Spawn(CurrentMap, Front))
                        monster.Spawn(CurrentMap, CurrentLocation);
                    break;
                case ItemType.SiegeAmmo:
                    //TODO;
                    break;
                default:
                    return;
            }

            if (item.Count > 1) item.Count--;
            else Info.Inventory[index] = null;
            RefreshBagWeight();

            Report?.ItemChanged(item, 1, 1);

            p.Success = true;
            Owner.Enqueue(p);
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
            Owner.Enqueue(new S.UpdateHeroSpawnState { State = HeroSpawnState.Dead });

            for (int i = 0; i < Buffs.Count; i++)
            {
                Buff buff = Buffs[i];

                if (!buff.Properties.HasFlag(BuffProperty.RemoveOnDeath)) continue;

                RemoveBuff(buff.Type);
            }

            PoisonList.Clear();
            InTrapRock = false;
        }

        public override void Revive(int hp, bool effect)
        {
            if (!Dead) return;

            SetHP(hp);
            SetMP(Stats[Stat.MP]);

            CurrentMap.RemoveObject(this);
            Broadcast(new S.ObjectRemove { ObjectID = ObjectID });

            Dead = false;
            ActionTime = Envir.Time + RevivalDelay;

            CurrentMap.AddObject(this);
            BroadcastInfo();
            Broadcast(new S.ObjectRevived { ObjectID = ObjectID, Effect = effect });
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Owner.Enqueue(new S.HeroLevelChanged { Level = Level, Experience = Experience, MaxExperience = MaxExperience });
        }
        protected override void SendHealthChanged()
        {
            Owner.Enqueue(new S.HeroHealthChanged { HP = HP, MP = MP });
            base.SendHealthChanged();
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

            if (Node == null || Info == null) return;

            if (Target != null && (Target.CurrentMap != CurrentMap || !Target.IsAttackTarget(this) || !Functions.InRange(CurrentLocation, Target.CurrentLocation, Globals.DataRange)))
                Target = null;

            if (!Functions.InRange(CurrentLocation, Owner.CurrentLocation, Globals.DataRange) || CurrentMap != Owner.CurrentMap)
                OwnerRecall();

            if (Dead) return;            

            if (Owner.PMode == PetMode.MoveOnly || Owner.PMode == PetMode.None)
                Target = null;

            ProcessAutoPot();
            ProcessStacking();
            ProcessSearch();
            ProcessAI();            
            ProcessTarget();
            ProcessRoam();
        }

        protected void ProcessAutoPot()
        {
            if (Envir.Time < AutoPotTime) return;

            AutoPotTime = Envir.Time + AutoPotDelay;

            if (PercentHealth < AutoHPPercent && HPItemIndex > 0 && PotHealthAmount <= 0)
                TryAutoPot(HPItemIndex);

            if (PercentMana < AutoMPPercent && MPItemIndex > 0 && PotManaAmount <= 0)
                TryAutoPot(MPItemIndex);
        }

        protected void TryAutoPot(int ItemIndex)
        {
            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                UserItem item = Info.Inventory[i];
                if (item == null) continue;
                if (item.Info.Index != ItemIndex) continue;

                UseItem(item.UniqueID);
                return;
            }
        }

        protected void ProcessAI() 
        {           
            if (NextMagicSpell != Spell.None) return;            
            ProcessFriend();

            if (NextMagicSpell != Spell.None) return;
            ProcessAttack();
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
            if (Owner.Info.HeroBehaviour == HeroBehaviour.Follow || !Mount.CanAttack) return;

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
        protected virtual void ProcessFriend() { }
        protected virtual void ProcessAttack() { }
        protected virtual void ProcessTarget()
        {
            if (CanCast && NextMagicSpell != Spell.None)
            {
                Magic(NextMagicSpell, NextMagicDirection, NextMagicTargetID, NextMagicLocation);
                NextMagicSpell = Spell.None;
            }

            if (Target == null || !CanAttack) return;

            TargetDistance = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);

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
            Attack(Direction, Spell.None);
        }

        protected virtual bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }

        protected List<MapObject> FindAllTargets(int dist, Point location, bool needSight = true)
        {
            List<MapObject> targets = new List<MapObject>();
            for (int d = 0; d <= dist; d++)
            {
                for (int y = location.Y - d; y <= location.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = location.X - d; x <= location.X + d; x += Math.Abs(y - location.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (!cell.Valid || cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                case ObjectType.Player:
                                case ObjectType.Hero:
                                    if (!ob.IsAttackTarget(this)) continue;
                                    if (ob.Hidden && (!CoolEye || Level < ob.Level) && needSight) continue;
                                    if (ob.Race == ObjectType.Player)
                                    {
                                        PlayerObject player = ((PlayerObject)ob);
                                        if (player.GMGameMaster) continue;
                                    }
                                    targets.Add(ob);
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
            return targets;
        }

        protected virtual void MoveTo(Point location)
        {
            if (CurrentLocation == location) return;

            bool inRange = Functions.InRange(location, CurrentLocation, 1);
            bool inRunRange = RidingMount ? Functions.InRange(location, CurrentLocation, 2) : inRange;

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

            if (!inRunRange && _stepCounter > 0 && Run(dir))
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

            if (Dead)
            {
                Despawn(false);
                return;
            }

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
                                    if (ob.Master != null && Target != ob) continue;
                                    if (Owner.Info.HeroBehaviour == HeroBehaviour.CounterAttack && ob.Target != this && ob.Target != Owner) continue;

                                    Target = ob;
                                    return;
                                case ObjectType.Player:
                                    PlayerObject playerob = (PlayerObject)ob;
                                    if (!ob.IsAttackTarget(Owner)) continue;
                                    if (playerob.GMGameMaster || ob.Hidden && (!CoolEye || Level < ob.Level)) continue;
                                    if (Target != ob && Owner.LastHitter != ob && ob.LastHitter != Owner) continue;

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
            if (Owner == null) return false;
            if (Dead) return false;

            return Owner.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(MonsterObject attacker)
        {
            if (Owner == null) return false;
            if (Dead) return false;

            return Owner.IsAttackTarget(attacker);
        }
        public override bool IsFriendlyTarget(HumanObject ally)
        {
            if (Owner == null) return false;
            return Owner.IsFriendlyTarget(ally);
        }
        public override bool IsFriendlyTarget(MonsterObject ally)
        {
            if (Owner == null) return false;
            return Owner.IsFriendlyTarget(ally);
        }

        public override void WinExp(uint amount, uint targetLevel = 0)
        {
            Owner.WinExp(amount, targetLevel);
        }

        public override void GainExp(uint amount)
        {
            if (amount == 0) return;

            for (int i = 0; i < Pets.Count; i++)
            {
                MonsterObject monster = Pets[i];
                if (monster.CurrentMap == CurrentMap && Functions.InRange(monster.CurrentLocation, CurrentLocation, Globals.DataRange) && !monster.Dead)
                    monster.PetExp(amount);
            }

            if (!CanGainExp) return;

            if (Stats[Stat.ExpRatePercent] > 0)
            {
                amount += (uint)Math.Max(0, (amount * Stats[Stat.ExpRatePercent]) / 100);
            }

            Experience += amount;

            Owner.Enqueue(new S.GainHeroExperience { Amount = amount });

            if (Experience < MaxExperience) return;
            if (Level >= ushort.MaxValue) return;

            //Calculate increased levels
            var experience = Experience;

            while (experience >= MaxExperience)
            {
                Level++;
                experience -= MaxExperience;

                RefreshLevelStats();

                if (Level >= ushort.MaxValue) break;
            }

            Experience = experience;

            LevelUp();
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

                HP = HP,
                MP = MP,

                Experience = Experience,
                MaxExperience = MaxExperience,

                Inventory = new UserItem[Info.Inventory.Length],
                Equipment = new UserItem[Info.Equipment.Length],

                AutoPot = AutoPot,
                AutoHPPercent = AutoHPPercent,
                AutoMPPercent = AutoMPPercent,
                HPItemIndex = HPItemIndex,
                MPItemIndex = MPItemIndex
            };

            for (int i = 0; i < Info.Magics.Count; i++)
                packet.Magics.Add(Info.Magics[i].CreateClientMagic());

            Info.Inventory.CopyTo(packet.Inventory, 0);
            Info.Equipment.CopyTo(packet.Equipment, 0);

            Owner.Enqueue(packet);

            SendBaseStats();
        }

        protected override void SendBaseStats()
        {
            Owner.Enqueue(new S.HeroBaseStatsInfo { Stats = Settings.ClassBaseStats[(byte)Class] });
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
