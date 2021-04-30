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

        public bool TradeLocked;
        public uint TradeGoldAmount;
        public bool AllowTrade;

        public bool RentalGoldLocked;
        public bool RentalItemLocked;
        public uint RentalGoldAmount;

        public SpecialItemMode ItemMode;

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

            Stats[Stat.HP] += (Stats[Stat.HP] * Stats[Stat.HPRatePercent]) / 100;
            Stats[Stat.MP] += (Stats[Stat.MP] * Stats[Stat.MPRatePercent]) / 100;
            Stats[Stat.MaxAC] += (Stats[Stat.MaxAC] * Stats[Stat.MaxACRatePercent]) / 100;
            Stats[Stat.MaxMAC] += (Stats[Stat.MaxMAC] * Stats[Stat.MaxMACRatePercent]) / 100;

            Stats[Stat.MaxDC] += (Stats[Stat.MaxDC] * Stats[Stat.MaxDCRatePercent]) / 100;
            Stats[Stat.MaxMC] += (Stats[Stat.MaxMC] * Stats[Stat.MaxMCRatePercent]) / 100;
            Stats[Stat.MaxSC] += (Stats[Stat.MaxSC] * Stats[Stat.MaxSCRatePercent]) / 100;
            Stats[Stat.AttackSpeed] += (Stats[Stat.AttackSpeed] * Stats[Stat.AttackSpeedRatePercent]) / 100;

            RefreshStatCaps();

            if (this == User && Light < 3) Light = 3;
            AttackSpeed = 1400 - ((Stats[Stat.AttackSpeed] * 60) + Math.Min(370, (Level * 14)));
            if (AttackSpeed < 550) AttackSpeed = 550;

            PercentHealth = (byte)(HP / (float)Stats[Stat.HP] * 100);

            GameScene.Scene.Redraw();
        }

        private void RefreshLevelStats()
        {
            Light = 0;

            foreach (var stat in CoreStats.Stats)
            {
                Stats[stat.Type] = stat.Calculate(Class, Level);
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

            ItemMode = SpecialItemMode.None;
            FastRun = false;

            ItemSets.Clear();
            MirSet.Clear();

            for (int i = 0; i < Equipment.Length; i++)
            {
                UserItem temp = Equipment[i];
                if (temp == null) continue;

                ItemInfo RealItem = Functions.GetRealItem(temp.Info, Level, Class, GameScene.ItemInfoList);

                if (RealItem.Type == ItemType.Weapon || RealItem.Type == ItemType.Torch)
                    CurrentHandWeight = (int)Math.Min(int.MaxValue, CurrentHandWeight + temp.Weight);
                else
                    CurrentWearWeight = (int)Math.Min(int.MaxValue, CurrentWearWeight + temp.Weight);

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
                    ItemMode |= RealItem.Unique;
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

            if (ItemMode.HasFlag(SpecialItemMode.Muscle))
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
                    CurrentHandWeight = (int)Math.Min(int.MaxValue, CurrentHandWeight + temp.Weight);
                else
                    CurrentWearWeight = (int)Math.Min(int.MaxValue, CurrentWearWeight + temp.Weight);

                if (temp.CurrentDura == 0 && RealItem.Durability > 0) continue;

                Stats.Add(RealItem.Stats);
                Stats.Add(temp.AddedStats);
        
                if (RealItem.Light > Light) Light = RealItem.Light;
                if (RealItem.Unique != SpecialItemMode.None)
                {
                    ItemMode |= RealItem.Unique;
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
                        Stats[Stat.HPDrainRatePercent] += 10;
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
                ClientBuff buff = GameScene.Scene.Buffs[i];

                Stats.Add(buff.Stats);

                switch (buff.Type)
                {
                    case BuffType.SwiftFeet:
                        Sprint = true;
                        break;
                    case BuffType.Transform:
                        TransformType = (short)buff.Values[0];
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

                Stats.Add(buff.Info.Stats);
            }
        }

        public void RefreshStatCaps()
        {
            foreach (var cap in CoreStats.Caps.Values)
            {
                Stats[cap.Key] = Math.Min(cap.Value, Stats[cap.Key]);
            }

            Stats[Stat.HP] = Math.Max(0, Stats[Stat.HP]);
            Stats[Stat.MP] = Math.Max(0, Stats[Stat.MP]);

            Stats[Stat.MinAC] = Math.Max(0, Stats[Stat.MinAC]);
            Stats[Stat.MaxAC] = Math.Max(0, Stats[Stat.MaxAC]);
            Stats[Stat.MinMAC] = Math.Max(0, Stats[Stat.MinMAC]);
            Stats[Stat.MaxMAC] = Math.Max(0, Stats[Stat.MaxMAC]);
            Stats[Stat.MinDC] = Math.Max(0, Stats[Stat.MinDC]);
            Stats[Stat.MaxDC] = Math.Max(0, Stats[Stat.MaxDC]);
            Stats[Stat.MinMC] = Math.Max(0, Stats[Stat.MinMC]);
            Stats[Stat.MaxMC] = Math.Max(0, Stats[Stat.MaxMC]);
            Stats[Stat.MinSC] = Math.Max(0, Stats[Stat.MinSC]);
            Stats[Stat.MaxSC] = Math.Max(0, Stats[Stat.MaxSC]);

            Stats[Stat.MinDC] = Math.Min(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            Stats[Stat.MinMC] = Math.Min(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            Stats[Stat.MinSC] = Math.Min(Stats[Stat.MinSC], Stats[Stat.MaxSC]);
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

