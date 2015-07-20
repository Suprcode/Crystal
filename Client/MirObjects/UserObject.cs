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

        public ushort HP, MaxHP, MP, MaxMP;

        public byte MinAC, MaxAC,
                   MinMAC, MaxMAC,
                   MinDC, MaxDC,
                   MinMC, MaxMC,
                   MinSC, MaxSC;

        public byte Accuracy, Agility;
        public sbyte ASpeed, Luck;
        public int AttackSpeed;

        public byte CurrentHandWeight, MaxHandWeight,
                      CurrentWearWeight, MaxWearWeight;
        public ushort CurrentBagWeight, MaxBagWeight;
        public long Experience, MaxExperience;
        public byte LifeOnHit;

        public bool TradeLocked;
        public bool AllowTrade;

        public bool HasTeleportRing, HasProtectionRing, HasRevivalRing, HasClearRing,
            HasMuscleRing, HasParalysisRing, HasFireRing, HasHealRing, HasProbeNecklace, HasSkillNecklace, NoDuraLoss;

        public byte MagicResist, PoisonResist, HealthRecovery, SpellRecovery, PoisonRecovery, CriticalRate, CriticalDamage, Holy, Freezing, PoisonAttack, HpDrainRate;
        public BaseStats CoreStats = new BaseStats(0);


        public UserItem[] Inventory = new UserItem[46], Equipment = new UserItem[14], Trade = new UserItem[10], QuestInventory = new UserItem[40];
        public int BeltIdx = 6;
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
        }

        public void Load(S.UserInformation info)
        {
            Id = info.RealId;
            Name = info.Name;
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

            Magics = info.Magics;

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
            RefreshLevelStats();
            RefreshBagWeight();
            RefreshEquipmentStats();
            RefreshItemSetStats();
            RefreshMirSetStats();
            RefreshSkills();
            RefreshBuffs();
            RefreshMountStats();
            //RefreshFishingStats();

            SetLibraries();
            SetEffects();
            
            if (this == User && Light < 3) Light = 3;
            AttackSpeed = 1400 - ((ASpeed * 60) + Math.Min(370, (Level * 14)));
            if (AttackSpeed < 550) AttackSpeed = 550;

            PercentHealth = (byte)(HP / (float)MaxHP * 100);

            GameScene.Scene.Redraw();
        }
        private void RefreshLevelStats()
        {
            MaxHP = 0; MaxMP = 0;
            MinAC = 0; MaxAC = 0;
            MinMAC = 0; MaxMAC = 0;
            MinDC = 0; MaxDC = 0;
            MinMC = 0; MaxMC = 0;
            MinSC = 0; MaxSC = 0;


            //Other Stats;
            MaxBagWeight = 0;
            MaxWearWeight = 0;
            MaxHandWeight = 0;
            ASpeed = 0;
            Luck = 0;
            Light = 0;
            LifeOnHit = 0;
            HpDrainRate = 0;
            MagicResist = 0;
            PoisonResist = 0;
            HealthRecovery = 0;
            SpellRecovery = 0;
            PoisonRecovery = 0;
            Holy = 0;
            Freezing = 0;
            PoisonAttack = 0;

            Accuracy = CoreStats.StartAccuracy;
            Agility = CoreStats.StartAgility;
            CriticalRate = CoreStats.StartCriticalRate;
            CriticalDamage = CoreStats.StartCriticalDamage;

            MaxHP = (ushort)Math.Min(ushort.MaxValue, 14 + (Level / CoreStats.HpGain + CoreStats.HpGainRate) * Level);

            MinAC = (byte)Math.Min(byte.MaxValue, CoreStats.MinAc > 0 ? Level / CoreStats.MinAc : 0);
            MaxAC = (byte)Math.Min(byte.MaxValue, CoreStats.MaxAc > 0 ? Level / CoreStats.MaxAc : 0);
            MinMAC = (byte)Math.Min(byte.MaxValue, CoreStats.MinMac > 0 ? Level / CoreStats.MinMac : 0);
            MaxMAC = (byte)Math.Min(byte.MaxValue, CoreStats.MaxMac > 0 ? Level / CoreStats.MaxMac : 0);
            MinDC = (byte)Math.Min(byte.MaxValue, CoreStats.MinDc > 0 ? Level / CoreStats.MinDc : 0);
            MaxDC = (byte)Math.Min(byte.MaxValue, CoreStats.MaxDc > 0 ? Level / CoreStats.MaxDc : 0);
            MinMC = (byte)Math.Min(byte.MaxValue, CoreStats.MinMc > 0 ? Level / CoreStats.MinMc : 0);
            MaxMC = (byte)Math.Min(byte.MaxValue, CoreStats.MaxMc > 0 ? Level / CoreStats.MaxMc : 0);
            MinSC = (byte)Math.Min(byte.MaxValue, CoreStats.MinSc > 0 ? Level / CoreStats.MinSc : 0);
            MaxSC = (byte)Math.Min(byte.MaxValue, CoreStats.MaxSc > 0 ? Level / CoreStats.MaxSc : 0);
            CriticalRate = (byte)Math.Min(byte.MaxValue, CoreStats.CritialRateGain > 0 ? CriticalRate + (Level / CoreStats.CritialRateGain) : CriticalRate);
            CriticalDamage = (byte)Math.Min(byte.MaxValue, CoreStats.CriticalDamageGain > 0 ? CriticalDamage + (Level / CoreStats.CriticalDamageGain) : CriticalDamage);
            MaxBagWeight = (ushort)(50 + Level / CoreStats.BagWeightGain * Level);
            MaxWearWeight = (byte)Math.Min(byte.MaxValue, 15 + Level / CoreStats.WearWeightGain * Level);
            MaxHandWeight = (byte)Math.Min(byte.MaxValue, 12 + Level / CoreStats.HandWeightGain * Level);


            switch (Class)
            {
                case MirClass.Warrior:
                    MaxHP = (ushort)Math.Min(ushort.MaxValue, 14 + (Level / CoreStats.HpGain + CoreStats.HpGainRate + Level / 20F) * Level);
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, 11 + (Level * 3.5F) + (Level * CoreStats.MpGainRate));
                    break;
                case MirClass.Wizard:
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, 13 + ((Level / 5F + 2F) * 2.2F * Level) + (Level * CoreStats.MpGainRate));
                    break;
                case MirClass.Taoist:
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, (13 + Level / 8F * 2.2F * Level) + (Level * CoreStats.MpGainRate));
                    break;
                case MirClass.Assassin:
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, (11 + Level * 5F) + (Level * CoreStats.MpGainRate));
                    break;
                case MirClass.Archer:
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, (11 + Level * 4F) + (Level * CoreStats.MpGainRate));
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
                    CurrentBagWeight = (ushort)Math.Min(ushort.MaxValue, CurrentBagWeight + item.Weight);
            }
        }
        private void RefreshEquipmentStats()
        {
            Weapon = -1;
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
            short Macrate = 0, Acrate = 0, HPrate = 0, MPrate = 0;

            ItemSets.Clear();
            MirSet.Clear();

            for (int i = 0; i < Equipment.Length; i++)
            {
                UserItem temp = Equipment[i];
                if (temp == null) continue;

                ItemInfo RealItem = Functions.GetRealItem(temp.Info, Level, Class, GameScene.ItemInfoList);

                if (RealItem.Type == ItemType.Weapon || RealItem.Type == ItemType.Torch)
                    CurrentHandWeight = (byte)Math.Min(byte.MaxValue, CurrentHandWeight + temp.Weight);
                else
                    CurrentWearWeight = (byte)Math.Min(byte.MaxValue, CurrentWearWeight + temp.Weight);

                if (temp.CurrentDura == 0 && RealItem.Durability > 0) continue;


                MinAC = (byte)Math.Min(byte.MaxValue, MinAC + RealItem.MinAC + temp.Awake.getAC());
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + RealItem.MaxAC + temp.AC + temp.Awake.getAC());
                MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + RealItem.MinMAC + temp.Awake.getMAC());
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + RealItem.MaxMAC + temp.MAC + temp.Awake.getMAC());

                MinDC = (byte)Math.Min(byte.MaxValue, MinDC + RealItem.MinDC + temp.Awake.getDC());
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + RealItem.MaxDC + temp.DC + temp.Awake.getDC());
                MinMC = (byte)Math.Min(byte.MaxValue, MinMC + RealItem.MinMC + temp.Awake.getMC());
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + RealItem.MaxMC + temp.MC + temp.Awake.getMC());
                MinSC = (byte)Math.Min(byte.MaxValue, MinSC + RealItem.MinSC + temp.Awake.getSC());
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + RealItem.MaxSC + temp.SC + temp.Awake.getSC());

                Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + RealItem.Accuracy + temp.Accuracy);
                Agility = (byte)Math.Min(byte.MaxValue, Agility + RealItem.Agility + temp.Agility);

                MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + RealItem.HP + temp.HP + temp.Awake.getHPMP());
                MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + RealItem.MP + temp.MP + temp.Awake.getHPMP());

                ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, ASpeed + temp.AttackSpeed + RealItem.AttackSpeed)));
                Luck = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, Luck + temp.Luck + RealItem.Luck)));

                MaxBagWeight = (ushort)Math.Max(ushort.MinValue, (Math.Min(ushort.MaxValue, MaxBagWeight + RealItem.BagWeight)));
                MaxWearWeight = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, MaxWearWeight + RealItem.WearWeight)));
                MaxHandWeight = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, MaxHandWeight + RealItem.HandWeight)));
                HPrate = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, HPrate + RealItem.HPrate));
                MPrate = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, MPrate + RealItem.MPrate));
                Acrate = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, Acrate + RealItem.MaxAcRate));
                Macrate = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, Macrate + RealItem.MaxMacRate));
                MagicResist = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, MagicResist + temp.MagicResist + RealItem.MagicResist)));
                PoisonResist = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, PoisonResist + temp.PoisonResist + RealItem.PoisonResist)));
                HealthRecovery = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, HealthRecovery + temp.HealthRecovery + RealItem.HealthRecovery)));
                SpellRecovery = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, SpellRecovery + temp.ManaRecovery + RealItem.SpellRecovery)));
                PoisonRecovery = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, PoisonRecovery + temp.PoisonRecovery + RealItem.PoisonRecovery)));
                CriticalRate = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, CriticalRate + temp.CriticalRate + RealItem.CriticalRate)));
                CriticalDamage = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, CriticalDamage + temp.CriticalDamage + RealItem.CriticalDamage)));
                Holy = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, Holy + RealItem.Holy)));
                Freezing = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, Freezing + temp.Freezing + RealItem.Freezing)));
                PoisonAttack = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, PoisonAttack + temp.PoisonAttack + RealItem.PoisonAttack)));
                HpDrainRate = (byte)Math.Max(100, Math.Min(byte.MaxValue, HpDrainRate + RealItem.HpDrainRate));


                
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

                if (RealItem.Type == ItemType.Armour)
                {
                    Armour = RealItem.Shape;
                    WingEffect = RealItem.Effect;
                }
                if (RealItem.Type == ItemType.Weapon)
                    Weapon = RealItem.Shape;

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

                //bool sameSetFound = false;
                //foreach (var set in ItemSets.Where(set => set.Set == RealItem.Set && !set.Type.Contains(RealItem.Type)).TakeWhile(set => !set.SetComplete))
                //{
                //    set.Type.Add(RealItem.Type);
                //    set.Count++;
                //    sameSetFound = true;
                //}

                //if (!ItemSets.Any() || !sameSetFound)
                //    ItemSets.Add(new ItemSets { Count = 1, Set = RealItem.Set, Type = new List<ItemType> { RealItem.Type } });

                //Mir Set
                if (RealItem.Set == ItemSet.Mir)
                {
                    if (!MirSet.Contains((EquipmentSlot)i))
                        MirSet.Add((EquipmentSlot)i);
                }
            }

            MaxHP = (ushort)Math.Min(ushort.MaxValue, (((double)HPrate / 100) + 1) * MaxHP);
            MaxMP = (ushort)Math.Min(ushort.MaxValue, (((double)MPrate / 100) + 1) * MaxMP);
            MaxAC = (byte)Math.Min(byte.MaxValue, (((double)Acrate / 100) + 1) * MaxAC);
            MaxMAC = (byte)Math.Min(byte.MaxValue, (((double)Macrate / 100) + 1) * MaxMAC);

            if (HasMuscleRing)
            {
                MaxBagWeight = (ushort)(MaxBagWeight * 2);
                MaxWearWeight = Math.Min(byte.MaxValue, (byte)(MaxWearWeight * 2));
                MaxHandWeight = Math.Min(byte.MaxValue, (byte)(MaxHandWeight * 2));
            }

        }

        private void RefreshItemSetStats()
        {
            foreach (var s in ItemSets)
            {
                if ((s.Set == ItemSet.Smash) && (s.Type.Contains(ItemType.Ring)) && (s.Type.Contains(ItemType.Bracelet)))
                    ASpeed = (sbyte)Math.Min(sbyte.MaxValue, ASpeed + 2);
                if ((s.Set == ItemSet.Purity) && (s.Type.Contains(ItemType.Ring)) && (s.Type.Contains(ItemType.Bracelet)))
                    Holy = Math.Min(byte.MaxValue, (byte)(Holy + 3));
                if ((s.Set == ItemSet.HwanDevil) && (s.Type.Contains(ItemType.Ring)) && (s.Type.Contains(ItemType.Bracelet)))
                {
                    MaxWearWeight = (byte)Math.Min(byte.MaxValue, MaxWearWeight + 5);
                    MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + 20);
                }

                if (!s.SetComplete) continue;
                switch (s.Set)
                {
                    case ItemSet.Mundane:
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 50);
                        break;
                    case ItemSet.NokChi:
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 50);
                        break;
                    case ItemSet.TaoProtect:
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 30);
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 30);
                        break;
                    case ItemSet.RedOrchid:
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + 2);
                        HpDrainRate = (byte)Math.Min(byte.MaxValue, HpDrainRate + 10);
                        break;
                    case ItemSet.RedFlower:
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 50);
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP - 50);
                        break;
                    case ItemSet.Smash:
                        MinDC = (byte)Math.Min(byte.MaxValue, MinDC + 1);
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 3);
                        break;
                    case ItemSet.HwanDevil:
                        MinMC = (byte)Math.Min(byte.MaxValue, MinMC + 1);
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 2);
                        break;
                    case ItemSet.Purity:
                        MinSC = (byte)Math.Min(byte.MaxValue, MinSC + 1);
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 2);
                        //holy +2;
                        break;
                    case ItemSet.FiveString:
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + ((MaxHP / 100) * 30));
                        MinAC = (byte)Math.Min(byte.MaxValue, MinAC + 2);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 2);
                        break;
                    case ItemSet.Spirit:
                        MinDC = (byte)Math.Min(byte.MaxValue, MinDC + 2);
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 5);
                        ASpeed = (sbyte)Math.Min(sbyte.MaxValue, ASpeed + 2);
                        break;
                    case ItemSet.Bone:
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 2);
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                        break;
                    case ItemSet.Bug:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 1);
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                        PoisonResist = (byte)Math.Min(byte.MaxValue, PoisonResist + 1);
                        break;
                    case ItemSet.WhiteGold:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 2);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 2);
                        break;
                    case ItemSet.WhiteGoldH:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 3);
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 30);
                        ASpeed = (sbyte)Math.Min(sbyte.MaxValue, ASpeed + 2);
                        break;
                    case ItemSet.RedJade:
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 2);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 2);
                        break;
                    case ItemSet.RedJadeH:
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 2);
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 40);
                        Agility = (byte)Math.Min(byte.MaxValue, Agility + 2);
                        break;
                    case ItemSet.Nephrite:
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 2);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                        break;
                    case ItemSet.NephriteH:
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 2);
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 15);
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 20);
                        Holy = (byte)Math.Min(byte.MaxValue, Holy + 1);
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + 1);
                        break;
                    case ItemSet.Whisker1:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 1);
                        MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + 25);
                        break;
                    case ItemSet.Whisker2:
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                        MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + 17);
                        break;
                    case ItemSet.Whisker3:
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                        MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + 17);
                        break;
                    case ItemSet.Whisker4:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 1);
                        MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + 20);
                        break;
                    case ItemSet.Whisker5:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 1);
                        MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + 17);
                        break;
                    case ItemSet.Hyeolryong:
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 2);
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 15);
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 20);
                        Holy = (byte)Math.Min(byte.MaxValue, Holy + 1);
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + 1);
                        break;
                    case ItemSet.Monitor:
                        MagicResist = (byte)Math.Min(byte.MaxValue, MagicResist + 1);
                        PoisonResist = (byte)Math.Min(byte.MaxValue, PoisonResist + 1);
                        break;
                    case ItemSet.Oppressive:
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
                        Agility = (byte)Math.Min(byte.MaxValue, Agility + 1);
                        break;
                }
            }
        }

        private void RefreshMirSetStats()
        {
            if (MirSet.Count() == 10)
            {
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + 70);
                Luck = (sbyte)Math.Min(sbyte.MaxValue, Luck + 2);
                ASpeed = (sbyte)Math.Min(sbyte.MaxValue, ASpeed + 2);
                MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 70);
                MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 80);
                MagicResist = (byte)Math.Min(byte.MaxValue, MagicResist + 6);
                PoisonResist = (byte)Math.Min(byte.MaxValue, PoisonResist + 6);
            }

            if (MirSet.Contains(EquipmentSlot.RingL) && MirSet.Contains(EquipmentSlot.RingR))
            {
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
            }
            if (MirSet.Contains(EquipmentSlot.BraceletL) && MirSet.Contains(EquipmentSlot.BraceletR))
            {
                MinAC = (byte)Math.Min(byte.MaxValue, MinAC + 1);
                MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + 1);
            }
            if ((MirSet.Contains(EquipmentSlot.RingL) | MirSet.Contains(EquipmentSlot.RingR)) && (MirSet.Contains(EquipmentSlot.BraceletL) | MirSet.Contains(EquipmentSlot.BraceletR)) && MirSet.Contains(EquipmentSlot.Necklace))
            {
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
                MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + 30);
                MaxWearWeight = (byte)Math.Min(byte.MaxValue, MaxWearWeight + 17);
            }
            if (MirSet.Contains(EquipmentSlot.RingL) && MirSet.Contains(EquipmentSlot.RingR) && MirSet.Contains(EquipmentSlot.BraceletL) && MirSet.Contains(EquipmentSlot.BraceletR) && MirSet.Contains(EquipmentSlot.Necklace))
            {
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
                MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + 20);
                MaxWearWeight = (byte)Math.Min(byte.MaxValue, MaxWearWeight + 10);
            }
            if (MirSet.Contains(EquipmentSlot.Armour) && MirSet.Contains(EquipmentSlot.Helmet) && MirSet.Contains(EquipmentSlot.Weapon))
            {
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 2);
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                Agility = (byte)Math.Min(byte.MaxValue, Agility + 1);
            }
            if (MirSet.Contains(EquipmentSlot.Armour) && MirSet.Contains(EquipmentSlot.Boots) && MirSet.Contains(EquipmentSlot.Belt))
            {
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 1);
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                MaxHandWeight = (byte)Math.Min(byte.MaxValue, MaxHandWeight + 17);
            }
            if (MirSet.Contains(EquipmentSlot.Armour) && MirSet.Contains(EquipmentSlot.Boots) && MirSet.Contains(EquipmentSlot.Belt) && MirSet.Contains(EquipmentSlot.Helmet) && MirSet.Contains(EquipmentSlot.Weapon))
            {
                MinDC = (byte)Math.Min(byte.MaxValue, MinDC + 1);
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 1);
                MinMC = (byte)Math.Min(byte.MaxValue, MinMC + 1);
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                MinSC = (byte)Math.Min(byte.MaxValue, MinSC + 1);
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                MaxHandWeight = (byte)Math.Min(byte.MaxValue, MaxHandWeight + 17);
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
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + magic.Level * 3);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + (magic.Level + 1) * 3);
                        break;
                    case Spell.FatalSword:
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + magic.Level);
                        break;
                    case Spell.SpiritSword:
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + magic.Level);
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + MaxSC * (magic.Level + 1) * 0.1F);
                        break;
                }
            }
        }
        private void RefreshBuffs()
        {
            for (int i = 0; i < GameScene.Scene.Buffs.Count; i++)
            {
                Buff buff = GameScene.Scene.Buffs[i];

                switch (buff.Type)
                {
                    case BuffType.Haste:
                    case BuffType.Fury:
                        ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, ASpeed + buff.Value)));
                        break;
                    case BuffType.SwiftFeet:
                        Sprint = true;
                        break;
                    case BuffType.LightBody:
                        Agility = (byte)Math.Min(byte.MaxValue, Agility + buff.Value);
                        break;
                    case BuffType.SoulShield:
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + buff.Value);
                        break;
                    case BuffType.BlessedArmour:
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + buff.Value);
                        break;
                    case BuffType.UltimateEnhancer:
                        if (Class == MirClass.Wizard || Class == MirClass.Archer)
                        {
                            MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + buff.Value);
                        }
                        else if (Class == MirClass.Taoist)
                        {
                            MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + buff.Value);
                        }
                        else
                        {
                            MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + buff.Value);
                        }
                        break;
                    case BuffType.ProtectionField:
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + buff.Value);
                        break;
                    case BuffType.Rage:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + buff.Value);
                        break;
                    case BuffType.CounterAttack:
                        MinAC = (byte)Math.Min(byte.MaxValue, MinAC + buff.Value);
                        MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + buff.Value);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + buff.Value);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + buff.Value);
                        break;
                    case BuffType.Curse:
                        byte rMaxDC = (byte)(((int)MaxDC / 100) * buff.Value);
                        byte rMaxMC = (byte)(((int)MaxMC / 100) * buff.Value);
                        byte rMaxSC = (byte)(((int)MaxSC / 100) * buff.Value);
                        byte rASpeed = (byte)(((int)ASpeed / 100) * buff.Value);

                        MaxDC = (byte)Math.Max(byte.MinValue, MaxDC - rMaxDC);
                        MaxMC = (byte)Math.Max(byte.MinValue, MaxMC - rMaxMC);
                        MaxSC = (byte)Math.Max(byte.MinValue, MaxSC - rMaxSC);
                        ASpeed = (sbyte)Math.Min(sbyte.MaxValue, (Math.Max(sbyte.MinValue, ASpeed - rASpeed)));
                        break;

                    case BuffType.Impact:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + buff.Value);
                        break;
                    case BuffType.Magic:
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + buff.Value);
                        break;
                    case BuffType.Taoist:
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + buff.Value);
                        break;
                    case BuffType.Storm:
                        ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, ASpeed + buff.Value)));
                        break;
                    case BuffType.HealthAid:
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + buff.Value);
                        break;
                    case BuffType.ManaAid:
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + buff.Value);
                        break;
                    case BuffType.WonderShield:
                        MinAC = (byte)Math.Min(byte.MaxValue, MinAC + buff.Value);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + buff.Value);
                        break;
                    case BuffType.MagicWonderShield:
                        MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + buff.Value);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + buff.Value);
                        break;
                    case BuffType.BagWeight:
                        MaxBagWeight = (ushort)Math.Min(ushort.MaxValue, MaxBagWeight + buff.Value);
                        break;
                }

            }
        }

        public void RefreshMountStats()
        {
            UserItem MountItem = Equipment[(int)EquipmentSlot.Mount];

            if (!RidingMount || MountItem == null) return;

            UserItem[] Slots = MountItem.Slots;

            for (int i = 0; i < Slots.Length; i++)
            {
                UserItem temp = Slots[i];
                if (temp == null) continue;

                ItemInfo RealItem = Functions.GetRealItem(temp.Info, Level, Class, GameScene.ItemInfoList);

                CurrentWearWeight = (byte)Math.Min(byte.MaxValue, CurrentWearWeight + temp.Weight);

                if (temp.CurrentDura == 0 && temp.Info.Durability > 0) continue;

                MinAC = (byte)Math.Min(byte.MaxValue, MinAC + RealItem.MinAC);
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + RealItem.MaxAC + temp.AC);
                MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + RealItem.MinMAC);
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + RealItem.MaxMAC + temp.MAC);

                MinDC = (byte)Math.Min(byte.MaxValue, MinDC + RealItem.MinDC);
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + RealItem.MaxDC + temp.DC);
                MinMC = (byte)Math.Min(byte.MaxValue, MinMC + RealItem.MinMC);
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + RealItem.MaxMC + temp.MC);
                MinSC = (byte)Math.Min(byte.MaxValue, MinSC + RealItem.MinSC);
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + RealItem.MaxSC + temp.SC);

                Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + RealItem.Accuracy + temp.Accuracy);
                Agility = (byte)Math.Min(byte.MaxValue, Agility + RealItem.Agility + temp.Agility);

                MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + RealItem.HP + temp.HP);
                MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + RealItem.MP + temp.MP);

                ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, ASpeed + temp.AttackSpeed + RealItem.AttackSpeed)));
                Luck = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, Luck + temp.Luck + RealItem.Luck)));
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
            if (CurrentBagWeight + item.Weight <= MaxBagWeight && FreeSpace(Inventory) > 0) return;

            uint min = 0;
            uint max = item.Count;

            if (CurrentBagWeight >= MaxBagWeight)
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
                    item.Count = bagItem.Info.StackSize - bagItem.Count;
                    min += item.Count;
                    if (min >= max)
                    {
                        item.Count = max;
                        return;
                    }
                }

                if (min == 0)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat(FreeSpace(Inventory) == 0 ? "You do not have enough space." : "You do not have enough weight.", ChatType.System);

                    item.Count = 0;
                    return;
                }

                item.Count = min;
                return;
            }

            if (CurrentBagWeight + item.Weight > MaxBagWeight)
            {
                item.Count = (uint)(Math.Max((MaxBagWeight - CurrentBagWeight), uint.MinValue) / item.Info.Weight);
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

                    item.Count = bagItem.Info.StackSize - bagItem.Count;
                    min += item.Count;
                    if (min >= max)
                    {
                        item.Count = max;
                        return;
                    }
                }

                if (min == 0)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough space.", ChatType.System);
                    item.Count = 0;
                }
            }
            else
            {
                GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough space.", ChatType.System);
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

