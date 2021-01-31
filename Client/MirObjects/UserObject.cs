using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client.MirScenes;
using S = ServerPackets;

namespace Client.MirObjects
{
    public class UserObject : PlayerObject
    {
        public uint Id;

        public int HP, MP;

        public int AttackSpeed;

        public Stats Stats;

        public int CurrentHandWeight,
                      CurrentWearWeight,
                      CurrentBagWeight;

        public long Experience, MaxExperience;
        public byte LifeOnHit;

        public bool TradeLocked;
        public uint TradeGoldAmount;
        public bool AllowTrade;

        public bool RentalGoldLocked;
        public bool RentalItemLocked;
        public uint RentalGoldAmount;

        public bool HasTeleportRing, HasProtectionRing, HasRevivalRing, HasClearRing,
            HasMuscleRing, HasParalysisRing, HasFireRing, HasHealRing, HasProbeNecklace, HasSkillNecklace, NoDuraLoss,
            HasBlinkSkill;

        public byte MagicResist, PoisonResist, HealthRecovery, SpellRecovery, PoisonRecovery, CriticalRate, CriticalDamage, Holy, Freezing, PoisonAttack, HpDrainRate;
        public BaseStats CoreStats = new BaseStats(0);


        public UserItem[] Inventory = new UserItem[46], Equipment = new UserItem[14], Trade = new UserItem[10], QuestInventory = new UserItem[40];
        public int BeltIdx = 6;
        public bool HasExpandedStorage = false;
        public DateTime ExpandedStorageExpiryTime;

        public List<ClientMagic> Magics = new List<ClientMagic>();
        public List<ItemSets> ItemSets = new List<ItemSets>();
        public List<EquipmentSlot> MirSet = new List<EquipmentSlot>();

        public List<ClientIntelligentCreature> IntelligentCreatures = new List<ClientIntelligentCreature>();//IntelligentCreature
        public IntelligentCreatureType SummonedCreatureType = IntelligentCreatureType.None;//IntelligentCreature
        public bool CreatureSummoned;//IntelligentCreature
        public int PearlCount = 0;

        public List<ClientQuestProgress> CurrentQuests = new List<ClientQuestProgress>();
        public List<int> CompletedQuests = new List<int>();
        public List<ClientMail> Mail = new List<ClientMail>();

        public ClientMagic NextMagic;
        public Point NextMagicLocation;
        public MapObject NextMagicObject;
        public MirDirection NextMagicDirection;
        public QueuedAction QueuedAction;

        public UserObject(uint objectID) : base(objectID)
        {
            Stats = new Stats();
        }

        public void Load(S.UserInformation info)
        {
            Id = info.RealId;
            Name = info.Name;
            Settings.LoadTrackedQuests(info.Name);
            NameColour = info.NameColour;
            GuildName = info.GuildName;
            GuildRankName = info.GuildRank;
            Class = info.Class;
            Gender = info.Gender;
            Level = info.Level;

            CurrentLocation = info.Location;
            MapLocation = info.Location;
            GameScene.Scene.MapControl.AddObject(this);

            Direction = info.Direction;
            Hair = info.Hair;

            HP = info.HP;
            MP = info.MP;

            Experience = info.Experience;
            MaxExperience = info.MaxExperience;

            LevelEffects = info.LevelEffects;

            Inventory = info.Inventory;
            Equipment = info.Equipment;
            QuestInventory = info.QuestInventory;

            HasExpandedStorage = info.HasExpandedStorage;
            ExpandedStorageExpiryTime = info.ExpandedStorageExpiryTime;

            Magics = info.Magics;
            for (int i = 0; i < Magics.Count; i++ )
            {
                if (Magics[i].CastTime > 0)
                    Magics[i].CastTime = CMain.Time - Magics[i].CastTime;
            }

            IntelligentCreatures = info.IntelligentCreatures;//IntelligentCreature
            SummonedCreatureType = info.SummonedCreatureType;//IntelligentCreature
            CreatureSummoned = info.CreatureSummoned;//IntelligentCreature

            BindAllItems();

            RefreshStats();

            SetAction();
        }

        public override void SetLibraries()
        {
            /*
            Weapon = -1;
            Armour = 0;
            WingEffect = 0; 
            if (Equipment != null)
            {
                if (Equipment[(int) EquipmentSlot.Weapon] != null)
                    Weapon = Equipment[(int) EquipmentSlot.Weapon].Info.Shape;
                if (Equipment[(int)EquipmentSlot.Armour] != null)
                {
                    Armour = Equipment[(int)EquipmentSlot.Armour].Info.Shape;
                    WingEffect = Equipment[(int)EquipmentSlot.Armour].Info.Effect;
                }
            }
            */
            base.SetLibraries();
        }

        public override void SetEffects()
        {
            base.SetEffects();
        }

        public void RefreshStats()
        {
            Stats.Clear();

            RefreshLevelStats();
            RefreshBagWeight();
            RefreshEquipmentStats();
            RefreshItemSetStats();
            RefreshMirSetStats();
            RefreshSkills();
            RefreshBuffs();
            RefreshGuildBuffs();

            SetLibraries();
            SetEffects();
            
            if (this == User && Light < 3) Light = 3;
            AttackSpeed = 1400 - ((Stats[Stat.AttackSpeed] * 60) + Math.Min(370, (Level * 14)));
            if (AttackSpeed < 550) AttackSpeed = 550;

            PercentHealth = (byte)(HP / (float)Stats[Stat.HP] * 100);

            GameScene.Scene.Redraw();
        }

        private void RefreshLevelStats()
        {
            Light = 0;
            LifeOnHit = 0;

            Stats[Stat.Accuracy] = CoreStats.StartAccuracy;
            Stats[Stat.Agility] = CoreStats.StartAgility;
            Stats[Stat.CriticalRate] = CoreStats.StartCriticalRate;
            Stats[Stat.CriticalDamage] = CoreStats.StartCriticalDamage;

            Stats[Stat.HP] = (ushort)Math.Min(ushort.MaxValue, 14 + (Level / CoreStats.HpGain + CoreStats.HpGainRate) * Level);

            Stats[Stat.MinAC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MinAc > 0 ? Level / CoreStats.MinAc : 0);
            Stats[Stat.MaxAC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MaxAc > 0 ? Level / CoreStats.MaxAc : 0);
            Stats[Stat.MinMAC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MinMac > 0 ? Level / CoreStats.MinMac : 0);
            Stats[Stat.MaxMAC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MaxMac > 0 ? Level / CoreStats.MaxMac : 0);
            Stats[Stat.MinDC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MinDc > 0 ? Level / CoreStats.MinDc : 0);
            Stats[Stat.MaxDC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MaxDc > 0 ? Level / CoreStats.MaxDc : 0);
            Stats[Stat.MinMC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MinMc > 0 ? Level / CoreStats.MinMc : 0);
            Stats[Stat.MaxMC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MaxMc > 0 ? Level / CoreStats.MaxMc : 0);
            Stats[Stat.MinSC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MinSc > 0 ? Level / CoreStats.MinSc : 0);
            Stats[Stat.MaxSC] = (ushort)Math.Min(ushort.MaxValue, CoreStats.MaxSc > 0 ? Level / CoreStats.MaxSc : 0);
            Stats[Stat.CriticalRate] = (byte)Math.Min(byte.MaxValue, CoreStats.CritialRateGain > 0 ? CriticalRate + (Level / CoreStats.CritialRateGain) : CriticalRate);
            Stats[Stat.CriticalDamage] = (byte)Math.Min(byte.MaxValue, CoreStats.CriticalDamageGain > 0 ? CriticalDamage + (Level / CoreStats.CriticalDamageGain) : CriticalDamage);
            Stats[Stat.BagWeight] = (ushort)Math.Min(ushort.MaxValue, 50 + Level / CoreStats.BagWeightGain * Level);
            Stats[Stat.WearWeight] = (ushort)Math.Min(ushort.MaxValue, 15 + Level / CoreStats.WearWeightGain * Level);
            Stats[Stat.HandWeight] = (ushort)Math.Min(ushort.MaxValue, 12 + Level / CoreStats.HandWeightGain * Level);

            switch (Class)
            {
                case MirClass.Warrior:
                    Stats[Stat.HP] = (ushort)Math.Min(ushort.MaxValue, 14 + (Level / CoreStats.HpGain + CoreStats.HpGainRate + Level / 20F) * Level);
                    Stats[Stat.MP] = (ushort)Math.Min(ushort.MaxValue, 11 + (Level * 3.5F) + (Level * CoreStats.MpGainRate));
                    break;
                case MirClass.Wizard:
                    Stats[Stat.MP] = (ushort)Math.Min(ushort.MaxValue, 13 + ((Level / 5F + 2F) * 2.2F * Level) + (Level * CoreStats.MpGainRate));
                    break;
                case MirClass.Taoist:
                    Stats[Stat.MP] = (ushort)Math.Min(ushort.MaxValue, (13 + Level / 8F * 2.2F * Level) + (Level * CoreStats.MpGainRate));
                    break;
                case MirClass.Assassin:
                    Stats[Stat.MP] = (ushort)Math.Min(ushort.MaxValue, (11 + Level * 5F) + (Level * CoreStats.MpGainRate));
                    break;
                case MirClass.Archer:
                    Stats[Stat.MP] = (ushort)Math.Min(ushort.MaxValue, (11 + Level * 4F) + (Level * CoreStats.MpGainRate));
                    break;
            }
        }

        private void RefreshBagWeight()
        {
            CurrentBagWeight = 0;

            for (int i = 0; i < Inventory.Length; i++)
            {
                UserItem item = Inventory[i];
                if (item != null)
                {
                    CurrentBagWeight += item.Weight;
                }
            }
        }

        private void RefreshEquipmentStats()
        {
            Weapon = -1;
            WeaponEffect = 0;
            Armour = 0;
            WingEffect = 0;
            MountType = -1;

            CurrentWearWeight = 0;
            CurrentHandWeight = 0;

            HasTeleportRing = false;
            HasProtectionRing = false;
            HasMuscleRing = false;
            HasParalysisRing = false;
            HasProbeNecklace = false;
            HasSkillNecklace = false;
            NoDuraLoss = false;
            FastRun = false;

            ItemSets.Clear();
            MirSet.Clear();

            for (int i = 0; i < Equipment.Length; i++)
            {
                UserItem temp = Equipment[i];
                if (temp == null) continue;

                ItemInfo RealItem = Functions.GetRealItem(temp.Info, Level, Class, GameScene.ItemInfoList);

                if (RealItem.Type == ItemType.Weapon || RealItem.Type == ItemType.Torch)
                    CurrentHandWeight = (ushort)Math.Min(ushort.MaxValue, CurrentHandWeight + temp.Weight);
                else
                    CurrentWearWeight = (ushort)Math.Min(ushort.MaxValue, CurrentWearWeight + temp.Weight);

                if (temp.CurrentDura == 0 && RealItem.Durability > 0) continue;

                Stats.Add(RealItem.Stats);
                Stats.Add(temp.AddedStats);

                Stats[Stat.MinAC] += temp.Awake.GetAC();
                Stats[Stat.MaxAC] += temp.Awake.GetAC();
                Stats[Stat.MinMAC] += temp.Awake.GetMAC();
                Stats[Stat.MaxMAC] += temp.Awake.GetMAC();

                Stats[Stat.MinDC] += temp.Awake.GetDC();
                Stats[Stat.MaxDC] += temp.Awake.GetDC();
                Stats[Stat.MinMC] += temp.Awake.GetMC();
                Stats[Stat.MaxMC] += temp.Awake.GetMC();
                Stats[Stat.MinSC] += temp.Awake.GetSC();
                Stats[Stat.MaxSC] += temp.Awake.GetSC();

                Stats[Stat.HP] += temp.Awake.GetHPMP();
                Stats[Stat.MP] += temp.Awake.GetHPMP();

                if (RealItem.Light > Light) Light = RealItem.Light;
                if (RealItem.Unique != SpecialItemMode.None)
                {
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Paralize)) HasParalysisRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Teleport)) HasTeleportRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Clearring)) HasClearRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Protection)) HasProtectionRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Revival)) HasRevivalRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Muscle)) HasMuscleRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Probe)) HasProbeNecklace = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Skill)) HasSkillNecklace = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.NoDuraLoss)) NoDuraLoss = true;
                }

                if (RealItem.CanFastRun)
                {
                    FastRun = true;
                }

                RefreshSocketStats(temp);

                if (RealItem.Type == ItemType.Armour)
                {
                    Armour = RealItem.Shape;
                    WingEffect = RealItem.Effect;
                }
                if (RealItem.Type == ItemType.Weapon)
                {
                    Weapon = RealItem.Shape;
                    WeaponEffect = RealItem.Effect;
                }

                if (RealItem.Type == ItemType.Mount)
                    MountType = RealItem.Shape;

                if (RealItem.Set == ItemSet.None) continue;

                ItemSets itemSet = ItemSets.Where(set => set.Set == RealItem.Set && !set.Type.Contains(RealItem.Type) && !set.SetComplete).FirstOrDefault();

                if (itemSet != null)
                {
                    itemSet.Type.Add(RealItem.Type);
                    itemSet.Count++;
                }
                else
                {
                    ItemSets.Add(new ItemSets { Count = 1, Set = RealItem.Set, Type = new List<ItemType> { RealItem.Type } });
                }

                //Mir Set
                if (RealItem.Set == ItemSet.Mir)
                {
                    if (!MirSet.Contains((EquipmentSlot)i))
                        MirSet.Add((EquipmentSlot)i);
                }
            }

            Stats[Stat.HP] = ((Stats[Stat.HPrate] / 100) + 1) * Stats[Stat.HP];
            Stats[Stat.MP] = ((Stats[Stat.MPrate] / 100) + 1) * Stats[Stat.MP];
            Stats[Stat.MaxAC] = ((Stats[Stat.MaxACRate] / 100) + 1) * Stats[Stat.MaxAC];
            Stats[Stat.MaxMAC] = ((Stats[Stat.MaxMACRate] / 100) + 1) * Stats[Stat.MaxMAC];

            if (HasMuscleRing)
            {
                Stats[Stat.BagWeight] = Stats[Stat.BagWeight] * 2;
                Stats[Stat.WearWeight] = Stats[Stat.WearWeight] * 2;
                Stats[Stat.HandWeight] = Stats[Stat.HandWeight] * 2;
            }
        }


        private void RefreshSocketStats(UserItem equipItem)
        {
            if (equipItem == null) return;

            if (equipItem.Info.Type == ItemType.Weapon && equipItem.Info.IsFishingRod)
            {
                return;
            }

            if (equipItem.Info.Type == ItemType.Mount && !RidingMount)
            {
                return;
            }

            for (int i = 0; i < equipItem.Slots.Length; i++)
            {
                UserItem temp = equipItem.Slots[i];

                if (temp == null) continue;
                ItemInfo RealItem = Functions.GetRealItem(temp.Info, Level, Class, GameScene.ItemInfoList);

                if (RealItem.Type == ItemType.Weapon || RealItem.Type == ItemType.Torch)
                    CurrentHandWeight = (byte)Math.Min(byte.MaxValue, CurrentHandWeight + temp.Weight);
                else
                    CurrentWearWeight = (byte)Math.Min(byte.MaxValue, CurrentWearWeight + temp.Weight);

                if (temp.CurrentDura == 0 && RealItem.Durability > 0) continue;

                Stats.Add(RealItem.Stats);
                Stats.Add(temp.AddedStats);
        
                if (RealItem.Light > Light) Light = RealItem.Light;
                if (RealItem.Unique != SpecialItemMode.None)
                {
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Paralize)) HasParalysisRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Teleport)) HasTeleportRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Clearring)) HasClearRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Protection)) HasProtectionRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Revival)) HasRevivalRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Muscle)) HasMuscleRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Probe)) HasProbeNecklace = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Skill)) HasSkillNecklace = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.NoDuraLoss)) NoDuraLoss = true;
                }
            }
        }

        private void RefreshItemSetStats()
        {
            foreach (var s in ItemSets)
            {
                if ((s.Set == ItemSet.Smash) &&
                    ((s.Type.Contains(ItemType.Ring) && s.Type.Contains(ItemType.Bracelet)) || (s.Type.Contains(ItemType.Ring) && s.Type.Contains(ItemType.Necklace)) || (s.Type.Contains(ItemType.Bracelet) && s.Type.Contains(ItemType.Necklace))))
                {
                    Stats[Stat.AttackSpeed] += 2;
                }

                if ((s.Set == ItemSet.Purity) && (s.Type.Contains(ItemType.Ring)) && (s.Type.Contains(ItemType.Bracelet)))
                {
                    Stats[Stat.Holy] += 3;
                }

                if ((s.Set == ItemSet.HwanDevil) && (s.Type.Contains(ItemType.Ring)) && (s.Type.Contains(ItemType.Bracelet)))
                {
                    Stats[Stat.WearWeight] += 5;
                    Stats[Stat.BagWeight] += 20;
                }

                if ((s.Set == ItemSet.DarkGhost) && (s.Type.Contains(ItemType.Necklace)) && (s.Type.Contains(ItemType.Bracelet)))
                {
                    Stats[Stat.HP] += 25;
                }

                if (!s.SetComplete) continue;

                switch (s.Set)
                {
                    case ItemSet.Mundane:
                        Stats[Stat.HP] += 50;
                        break;
                    case ItemSet.NokChi:
                        Stats[Stat.MP] += 50;
                        break;
                    case ItemSet.TaoProtect:
                        Stats[Stat.HP] += 30;
                        Stats[Stat.MP] += 30;
                        break;
                    case ItemSet.RedOrchid:
                        Stats[Stat.Accuracy] += 2;
                        Stats[Stat.HpDrainRate] += 10;
                        break;
                    case ItemSet.RedFlower:
                        Stats[Stat.HP] += 50;
                        Stats[Stat.MP] -= 25;
                        break;
                    case ItemSet.Smash:
                        Stats[Stat.MinDC] += 1;
                        Stats[Stat.MaxDC] += 3;
                        break;
                    case ItemSet.HwanDevil:
                        Stats[Stat.MinMC] += 1;
                        Stats[Stat.MaxMC] += 2;
                        break;
                    case ItemSet.Purity:
                        Stats[Stat.MinSC] += 1;
                        Stats[Stat.MaxSC] += 2;
                        break;
                    case ItemSet.FiveString:
                        Stats[Stat.HP] += (int)(((double)Stats[Stat.HP] / 100) * 30);
                        Stats[Stat.MinAC] += 2;
                        Stats[Stat.MaxAC] += 2;
                        break;
                    case ItemSet.Spirit:
                        Stats[Stat.MinDC] += 2;
                        Stats[Stat.MaxDC] += 5;
                        Stats[Stat.AttackSpeed] += 2;
                        break;
                    case ItemSet.Bone:
                        Stats[Stat.MaxAC] += 2;
                        Stats[Stat.MaxMC] += 1;
                        Stats[Stat.MaxSC] += 1;
                        break;
                    case ItemSet.Bug:
                        Stats[Stat.MaxDC] += 1;
                        Stats[Stat.MaxMC] += 1;
                        Stats[Stat.MaxSC] += 1;
                        Stats[Stat.MaxMAC] += 1;
                        Stats[Stat.PoisonResist] += 1;
                        break;
                    case ItemSet.WhiteGold:
                        Stats[Stat.MaxDC] += 2;
                        Stats[Stat.MaxAC] += 2;
                        break;
                    case ItemSet.WhiteGoldH:
                        Stats[Stat.MaxDC] += 3;
                        Stats[Stat.HP] += 30;
                        Stats[Stat.AttackSpeed] += 2;
                        break;
                    case ItemSet.RedJade:
                        Stats[Stat.MaxMC] += 2;
                        Stats[Stat.MaxMAC] += 2;
                        break;
                    case ItemSet.RedJadeH:
                        Stats[Stat.MaxMC] += 2;
                        Stats[Stat.MP] += 40;
                        Stats[Stat.Agility] += 2;
                        break;
                    case ItemSet.Nephrite:
                        Stats[Stat.MaxSC] += 2;
                        Stats[Stat.MaxAC] += 1;
                        Stats[Stat.MaxMAC] += 1;
                        break;
                    case ItemSet.NephriteH:
                        Stats[Stat.MaxSC] += 2;
                        Stats[Stat.HP] += 15;
                        Stats[Stat.MP] += 20;
                        Stats[Stat.Holy] += 1;
                        Stats[Stat.Accuracy] += 1;
                        break;
                    case ItemSet.Whisker1:
                        Stats[Stat.MaxDC] += 1;
                        Stats[Stat.BagWeight] += 25;
                        break;
                    case ItemSet.Whisker2:
                        Stats[Stat.MaxMC] += 1;
                        Stats[Stat.BagWeight] += 17;
                        break;
                    case ItemSet.Whisker3:
                        Stats[Stat.MaxSC] += 1;
                        Stats[Stat.BagWeight] += 17;
                        break;
                    case ItemSet.Whisker4:
                        Stats[Stat.MaxDC] += 1;
                        Stats[Stat.BagWeight] += 20;
                        break;
                    case ItemSet.Whisker5:
                        Stats[Stat.MaxDC] += 1;
                        Stats[Stat.BagWeight] += 17;
                        break;
                    case ItemSet.Hyeolryong:
                        Stats[Stat.MaxSC] += 2;
                        Stats[Stat.HP] += 15;
                        Stats[Stat.MP] += 20;
                        Stats[Stat.Holy] += 1;
                        Stats[Stat.Accuracy] += 1;
                        break;
                    case ItemSet.Monitor:
                        Stats[Stat.MagicResist] += 1;
                        Stats[Stat.PoisonResist] += 1;
                        break;
                    case ItemSet.Oppressive:
                        Stats[Stat.MaxAC] += 1;
                        Stats[Stat.Agility] += 1;
                        break;
                    case ItemSet.BlueFrost:
                        Stats[Stat.MinDC] += 1;
                        Stats[Stat.MaxDC] += 1;
                        Stats[Stat.MinMC] += 1;
                        Stats[Stat.MaxMC] += 1;
                        Stats[Stat.HandWeight] += 1;
                        Stats[Stat.WearWeight] += 2;
                        break;
                    case ItemSet.BlueFrostH:
                        Stats[Stat.MinDC] += 1;
                        Stats[Stat.MaxDC] += 2;
                        Stats[Stat.MaxMC] += 2;
                        Stats[Stat.Accuracy] += 1;
                        Stats[Stat.HP] += 50;
                        break;
                    case ItemSet.DarkGhost:
                        Stats[Stat.MP] += 25;
                        Stats[Stat.AttackSpeed] += 2;
                        break;
                }
            }
        }


        private void RefreshMirSetStats()
        {
            if (MirSet.Count() == 10)
            {
                Stats[Stat.MaxAC] += 1;
                Stats[Stat.MaxMAC] += 1;
                Stats[Stat.BagWeight] += 70;
                Stats[Stat.Luck] += 2;
                Stats[Stat.AttackSpeed] += 2;
                Stats[Stat.HP] += 70;
                Stats[Stat.MP] += 80;
                Stats[Stat.MagicResist] += 6;
                Stats[Stat.PoisonResist] += 6;
            }

            if (MirSet.Contains(EquipmentSlot.RingL) && MirSet.Contains(EquipmentSlot.RingR))
            {
                Stats[Stat.MaxMAC] += 1;
                Stats[Stat.MaxAC] += 1;
            }
            if (MirSet.Contains(EquipmentSlot.BraceletL) && MirSet.Contains(EquipmentSlot.BraceletR))
            {
                Stats[Stat.MinAC] += 1;
                Stats[Stat.MinMAC] += 1;
            }
            if ((MirSet.Contains(EquipmentSlot.RingL) | MirSet.Contains(EquipmentSlot.RingR)) && (MirSet.Contains(EquipmentSlot.BraceletL) | MirSet.Contains(EquipmentSlot.BraceletR)) && MirSet.Contains(EquipmentSlot.Necklace))
            {
                Stats[Stat.MaxMAC] += 1;
                Stats[Stat.MaxAC] += 1;
                Stats[Stat.BagWeight] += 30;
                Stats[Stat.WearWeight] += 17;
            }
            if (MirSet.Contains(EquipmentSlot.RingL) && MirSet.Contains(EquipmentSlot.RingR) && MirSet.Contains(EquipmentSlot.BraceletL) && MirSet.Contains(EquipmentSlot.BraceletR) && MirSet.Contains(EquipmentSlot.Necklace))
            {
                Stats[Stat.MaxMAC] += 1;
                Stats[Stat.MaxAC] += 1;
                Stats[Stat.BagWeight] += 20;
                Stats[Stat.WearWeight] += 10;
            }
            if (MirSet.Contains(EquipmentSlot.Armour) && MirSet.Contains(EquipmentSlot.Helmet) && MirSet.Contains(EquipmentSlot.Weapon))
            {
                Stats[Stat.MaxDC] += 2;
                Stats[Stat.MaxMC] += 1;
                Stats[Stat.MaxSC] += 1;
                Stats[Stat.Agility] += 1;
            }
            if (MirSet.Contains(EquipmentSlot.Armour) && MirSet.Contains(EquipmentSlot.Boots) && MirSet.Contains(EquipmentSlot.Belt))
            {
                Stats[Stat.MaxDC] += 1;
                Stats[Stat.MaxMC] += 1;
                Stats[Stat.MaxSC] += 1;
                Stats[Stat.HandWeight] += 17;
            }
            if (MirSet.Contains(EquipmentSlot.Armour) && MirSet.Contains(EquipmentSlot.Boots) && MirSet.Contains(EquipmentSlot.Belt) && MirSet.Contains(EquipmentSlot.Helmet) && MirSet.Contains(EquipmentSlot.Weapon))
            {
                Stats[Stat.MinDC] += 1;
                Stats[Stat.MaxDC] += 1;
                Stats[Stat.MinMC] += 1;
                Stats[Stat.MaxMC] += 1;
                Stats[Stat.MinSC] += 1;
                Stats[Stat.MaxSC] += 1;
                Stats[Stat.HandWeight] += 17;
            }
        }

        private void RefreshSkills()
        {
            for (int i = 0; i < Magics.Count; i++)
            {
                ClientMagic magic = Magics[i];
                switch (magic.Spell)
                {
                    case Spell.Fencing:
                        Stats[Stat.Accuracy] += magic.Level * 3;
                        Stats[Stat.MaxAC] += (magic.Level + 1) * 3;
                        break;
                    case Spell.FatalSword:
                        Stats[Stat.Accuracy] += magic.Level;
                        break;
                    case Spell.SpiritSword:
                        Stats[Stat.Accuracy] += magic.Level;
                        Stats[Stat.MaxDC] += (int)(Stats[Stat.MaxSC] * (magic.Level + 1) * 0.1F);
                        break;
                }
            }
        }

        private void RefreshBuffs()
        {
            TransformType = -1;

            for (int i = 0; i < GameScene.Scene.Buffs.Count; i++)
            {
                Buff buff = GameScene.Scene.Buffs[i];

                switch (buff.Type)
                {
                    case BuffType.Haste:
                    case BuffType.Fury:
                        Stats[Stat.AttackSpeed] += buff.Values[0];
                        break;
                    case BuffType.ImmortalSkin:
                        Stats[Stat.MaxAC] += buff.Values[0];
                        Stats[Stat.MaxDC] -= buff.Values[1];
                        break;
                    case BuffType.SwiftFeet:
                        Sprint = true;
                        break;
                    case BuffType.LightBody:
                        Stats[Stat.Agility] += buff.Values[0];
                        break;
                    case BuffType.SoulShield:
                        Stats[Stat.MaxMAC] += buff.Values[0];
                        break;
                    case BuffType.BlessedArmour:
                        Stats[Stat.MaxAC] += buff.Values[0];
                        break;
                    case BuffType.UltimateEnhancer:
                        switch (Class)
                        {
                            case MirClass.Wizard:
                            case MirClass.Archer:
                                Stats[Stat.MaxMC] += buff.Values[0];
                                break;
                            case MirClass.Taoist:
                                Stats[Stat.MaxSC] += buff.Values[0];
                                break;
                            case MirClass.Warrior:
                                Stats[Stat.MaxDC] += buff.Values[0];
                                break;
                        }
                        break;
                    case BuffType.ProtectionField:
                        Stats[Stat.MaxAC] += buff.Values[0];
                        break;
                    case BuffType.Rage:
                        Stats[Stat.MaxDC] += buff.Values[0];
                        break;
                    case BuffType.CounterAttack:
                        Stats[Stat.MinAC] += buff.Values[0];
                        Stats[Stat.MinMAC] += buff.Values[0];
                        Stats[Stat.MaxAC] += buff.Values[0];
                        Stats[Stat.MaxMAC] += buff.Values[0];
                        break;
                    case BuffType.Curse:

                        ushort rMaxDC = (ushort)(((int)Stats[Stat.MaxDC] / 100) * buff.Values[0]);
                        ushort rMaxMC = (ushort)(((int)Stats[Stat.MaxMC] / 100) * buff.Values[0]);
                        ushort rMaxSC = (ushort)(((int)Stats[Stat.MaxSC] / 100) * buff.Values[0]);
                        byte rASpeed = (byte)(((int)Stats[Stat.AttackSpeed] / 100) * buff.Values[0]);

                        Stats[Stat.MaxDC] -= rMaxDC;
                        Stats[Stat.MaxMC] -= rMaxMC;
                        Stats[Stat.MaxSC] -= rMaxSC;
                        Stats[Stat.AttackSpeed] -= rASpeed;
                        break;
                    case BuffType.MagicBooster:
                        Stats[Stat.MinMC] += buff.Values[0];
                        Stats[Stat.MaxMC] += buff.Values[0];
                        break;
                    case BuffType.Knapsack:
                    case BuffType.BagWeight:
                        Stats[Stat.BagWeight] += buff.Values[0];
                        break;
                    case BuffType.Transform:
                        TransformType = (short)buff.Values[0];
                        break;
                    case BuffType.Impact:
                        Stats[Stat.MaxDC] += buff.Values[0];
                        break;
                    case BuffType.Magic:
                        Stats[Stat.MaxMC] += buff.Values[0];
                        break;
                    case BuffType.Taoist:
                        Stats[Stat.MaxSC] += buff.Values[0];
                        break;
                    case BuffType.Storm:
                        Stats[Stat.AttackSpeed] += buff.Values[0];
                        break;
                    case BuffType.HealthAid:
                        Stats[Stat.HP] += buff.Values[0];
                        break;
                    case BuffType.ManaAid:
                        Stats[Stat.MP] += buff.Values[0];
                        break;
                    case BuffType.Defence:
                        Stats[Stat.MaxAC] += buff.Values[0];
                        break;
                    case BuffType.MagicDefence:
                        Stats[Stat.MaxMAC] += buff.Values[0];
                        break;
                    case BuffType.WonderDrug:
                        switch (buff.Values[0])
                        {
                            case 2:
                                Stats[Stat.HP] += buff.Values[0];
                                break;
                            case 3:
                                Stats[Stat.MP] += buff.Values[0];
                                break;
                            case 4:
                                Stats[Stat.MinAC] += buff.Values[0];
                                Stats[Stat.MaxAC] += buff.Values[0];
                                break;
                            case 5:
                                Stats[Stat.MinMAC] += buff.Values[0];
                                Stats[Stat.MaxMAC] += buff.Values[0];
                                break;
                            case 6:
                                Stats[Stat.AttackSpeed] += buff.Values[0];
                                break;
                        }
                        break;
                }

            }
        }

        public void RefreshGuildBuffs()
        {
            if (User != this) return;
            if (GameScene.Scene.GuildDialog == null) return;
            for (int i = 0; i < GameScene.Scene.GuildDialog.EnabledBuffs.Count; i++)
            {
                GuildBuff buff = GameScene.Scene.GuildDialog.EnabledBuffs[i];
                if (buff == null) continue;
                if (!buff.Active) continue;

                if (buff.Info == null)
                {
                    buff.Info = GameScene.Scene.GuildDialog.FindGuildBuffInfo(buff.Id);
                }

                if (buff.Info == null) continue;

                Stats[Stat.MaxAC] += buff.Info.BuffAc;
                Stats[Stat.MaxMAC] += buff.Info.BuffMac;
                Stats[Stat.MaxDC] += buff.Info.BuffDc;
                Stats[Stat.MaxMC] += buff.Info.BuffMc;
                Stats[Stat.MaxSC] += buff.Info.BuffSc;
                Stats[Stat.HP] += buff.Info.BuffMaxHp;
                Stats[Stat.MP] += buff.Info.BuffMaxMp;
                Stats[Stat.HealthRecovery] += buff.Info.BuffHpRegen;
                Stats[Stat.SpellRecovery] += buff.Info.BuffMPRegen;
            }
        }


        public void BindAllItems()
        {
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] == null) continue;
                GameScene.Bind(Inventory[i]);
            }

            for (int i = 0; i < Equipment.Length; i++)
            {
                if (Equipment[i] == null) continue;
                GameScene.Bind(Equipment[i]);
            }

            for (int i = 0; i < QuestInventory.Length; i++)
            {
                if (QuestInventory[i] == null) continue;
                GameScene.Bind(QuestInventory[i]);
            }
        }


        public ClientMagic GetMagic(Spell spell)
        {
            for (int i = 0; i < Magics.Count; i++)
            {
                ClientMagic magic = Magics[i];
                if (magic.Spell != spell) continue;
                return magic;
            }

            return null;
        }


        public void GetMaxGain(UserItem item)
        {
            if (CurrentBagWeight + item.Weight <= Stats[Stat.BagWeight] && FreeSpace(Inventory) > 0) return;

            ushort min = 0;
            ushort max = item.Count;

            if (CurrentBagWeight >= Stats[Stat.BagWeight])
            {

            }

            if (item.Info.Type == ItemType.Amulet)
            {
                for (int i = 0; i < Inventory.Length; i++)
                {
                    UserItem bagItem = Inventory[i];

                    if (bagItem == null || bagItem.Info != item.Info) continue;

                    if (bagItem.Count + item.Count <= bagItem.Info.StackSize)
                    {
                        item.Count = max;
                        return;
                    }
                    item.Count = (ushort)(bagItem.Info.StackSize - bagItem.Count);
                    min += item.Count;
                    if (min >= max)
                    {
                        item.Count = max;
                        return;
                    }
                }

                if (min == 0)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat(FreeSpace(Inventory) == 0 ? GameLanguage.NoBagSpace : "You do not have enough weight.", ChatType.System);

                    item.Count = 0;
                    return;
                }

                item.Count = min;
                return;
            }

            if (CurrentBagWeight + item.Weight > Stats[Stat.BagWeight])
            {
                item.Count = (ushort)(Math.Max((Stats[Stat.BagWeight] - CurrentBagWeight), ushort.MinValue) / item.Info.Weight);
                max = item.Count;
                if (item.Count == 0)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough weight.", ChatType.System);
                    return;
                }
            }

            if (item.Info.StackSize > 1)
            {
                for (int i = 0; i < Inventory.Length; i++)
                {
                    UserItem bagItem = Inventory[i];

                    if (bagItem == null) return;
                    if (bagItem.Info != item.Info) continue;

                    if (bagItem.Count + item.Count <= bagItem.Info.StackSize)
                    {
                        item.Count = max;
                        return;
                    }

                    item.Count = (ushort)(bagItem.Info.StackSize - bagItem.Count);
                    min += item.Count;
                    if (min >= max)
                    {
                        item.Count = max;
                        return;
                    }
                }

                if (min == 0)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.NoBagSpace, ChatType.System);
                    item.Count = 0;
                }
            }
            else
            {
                GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.NoBagSpace, ChatType.System);
                item.Count = 0;
            }

        }
        private int FreeSpace(UserItem[] array)
        {
            int count = 0;
            for (int i = 0; i < array.Length; i++)
                count++;
            return count;
        }

        public override void SetAction()
        {
            if (QueuedAction != null )
            {
                if ((ActionFeed.Count == 0) || (ActionFeed.Count == 1 && NextAction.Action == MirAction.Stance))
                {
                    ActionFeed.Clear();
                    ActionFeed.Add(QueuedAction);
                    QueuedAction = null;
                }
            }

            base.SetAction();
        }
        public override void ProcessFrames()
        {
            bool clear = CMain.Time >= NextMotion;

            base.ProcessFrames();

            if (clear) QueuedAction = null;
            if ((CurrentAction == MirAction.Standing || CurrentAction == MirAction.MountStanding || CurrentAction == MirAction.Stance || CurrentAction == MirAction.Stance2 || CurrentAction == MirAction.DashFail) && (QueuedAction != null || NextAction != null))
                SetAction();
        }

        public void ClearMagic()
        {
            NextMagic = null;
            NextMagicDirection = 0;
            NextMagicLocation = Point.Empty;
            NextMagicObject = null;
        } 
    }
}

