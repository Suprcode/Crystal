using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MS.Internal.Xml.XPath;
using Server.MirEnvir;
using Server.MirNetwork;
using Server.MirObjects;
using System.Windows.Forms;

namespace Server.MirDatabase
{
    public class CharacterInfo
    {
        public int Index;
        public string Name;
        public byte Level;
        public MirClass Class;
        public MirGender Gender;
        public byte Hair;
        public int GuildIndex = -1;

        public string CreationIP;
        public DateTime CreationDate;

        public bool Banned;
        public string BanReason = string.Empty;
        public DateTime ExpiryDate;

        public bool ChatBanned;
        public DateTime ChatBanExpiryDate;

        public string LastIP = string.Empty;
        public DateTime LastDate;

        public bool Deleted;
        public DateTime DeleteDate;

        public ListViewItem ListItem;

        //Location
        public int CurrentMapIndex;
        public Point CurrentLocation;
        public MirDirection Direction;
        public int BindMapIndex;
        public Point BindLocation;

        public ushort HP, MP;
        public long Experience;

        public AttackMode AMode;
        public PetMode PMode;
        public bool AllowGroup;
        public bool AllowTrade;

        public int PKPoints;

        public bool NewDay;

        public bool Thrusting, HalfMoon, CrossHalfMoon;
        public bool DoubleSlash;
        public byte MentalState;
        public byte MentalStateLvl;

        public UserItem[] Inventory = new UserItem[46], Equipment = new UserItem[14], Trade = new UserItem[10], QuestInventory = new UserItem[40];
        public List<UserMagic> Magics = new List<UserMagic>();
        public List<PetInfo> Pets = new List<PetInfo>();
        public List<Buff> Buffs = new List<Buff>();
        public List<MailInfo> Mail = new List<MailInfo>();

        //IntelligentCreature
        public List<UserIntelligentCreature> IntelligentCreatures = new List<UserIntelligentCreature>();
        public IntelligentCreatureType SummonedCreatureType = IntelligentCreatureType.None;
        public bool CreatureSummoned;
        public int PearlCount;

        public List<QuestProgressInfo> CurrentQuests = new List<QuestProgressInfo>();

        public bool[] Flags = new bool[Globals.FlagIndexCount];

        public AccountInfo AccountInfo;
        public PlayerObject Player;
        public MountInfo Mount;

        public CharacterInfo()
        {

        }

        public CharacterInfo(ClientPackets.NewCharacter p, MirConnection c)
        {
            Name = p.Name;
            Class = p.Class;
            Gender = p.Gender;

            CreationIP = c.IPAddress;
            CreationDate = SMain.Envir.Now;
        }

        public CharacterInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Name = reader.ReadString();

            Level = reader.ReadByte();
            Class = (MirClass) reader.ReadByte();
            Gender = (MirGender) reader.ReadByte();
            Hair = reader.ReadByte();

            CreationIP = reader.ReadString();
            CreationDate = DateTime.FromBinary(reader.ReadInt64());

            Banned = reader.ReadBoolean();
            BanReason = reader.ReadString();
            ExpiryDate = DateTime.FromBinary(reader.ReadInt64());

            LastIP = reader.ReadString();
            LastDate = DateTime.FromBinary(reader.ReadInt64());

            Deleted = reader.ReadBoolean();
            DeleteDate = DateTime.FromBinary(reader.ReadInt64());

            CurrentMapIndex = reader.ReadInt32();
            CurrentLocation = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            BindMapIndex = reader.ReadInt32();
            BindLocation = new Point(reader.ReadInt32(), reader.ReadInt32());

            HP = reader.ReadUInt16();
            MP = reader.ReadUInt16();
            Experience = reader.ReadInt64();
            
            AMode = (AttackMode) reader.ReadByte();
            PMode = (PetMode) reader.ReadByte();

            if (Envir.LoadVersion > 34)
            {
                PKPoints = reader.ReadInt32();
            }

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, Envir.LoadVersion);
                if (SMain.Envir.BindItem(item) && i < Inventory.Length)
                    Inventory[i] = item;
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, Envir.LoadVersion);
                if (SMain.Envir.BindItem(item) && i < Equipment.Length)
                    Equipment[i] = item;
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, Envir.LoadVersion);
                if (SMain.Envir.BindItem(item) && i < QuestInventory.Length)
                    QuestInventory[i] = item;
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                UserMagic magic = new UserMagic(reader);
                if (magic.Info == null) continue;
                Magics.Add(magic);
            }


            if (Envir.LoadVersion < 2) return;

            Thrusting = reader.ReadBoolean();
            HalfMoon = reader.ReadBoolean();
            CrossHalfMoon = reader.ReadBoolean();
            DoubleSlash = reader.ReadBoolean();

            if (Envir.LoadVersion < 4) return;

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                Pets.Add(new PetInfo(reader));


            if (Envir.LoadVersion < 5) return;

            AllowGroup = reader.ReadBoolean();

            if (Envir.LoadVersion < 12) return;

            if (Envir.LoadVersion == 12) count = reader.ReadInt32();

            for (int i = 0; i < Globals.FlagIndexCount; i++)
                Flags[i] = reader.ReadBoolean();

            if (Envir.LoadVersion > 27)
                GuildIndex = reader.ReadInt32();

            if (Envir.LoadVersion > 30)
                AllowTrade = reader.ReadBoolean();

            if (Envir.LoadVersion > 33)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    CurrentQuests.Add(new QuestProgressInfo(reader));
            }

            if(Envir.LoadVersion > 42)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    Buffs.Add(new Buff(reader));
            }

            if(Envir.LoadVersion > 43)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    Mail.Add(new MailInfo(reader));
            }

            //IntelligentCreature
            if (Envir.LoadVersion > 44)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    UserIntelligentCreature creature = new UserIntelligentCreature(reader);
                    if (creature.Info == null) continue;
                    IntelligentCreatures.Add(creature);
                }
                SummonedCreatureType = (IntelligentCreatureType)reader.ReadByte();
                CreatureSummoned = reader.ReadBoolean();
                PearlCount = reader.ReadInt32();
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Name);
            writer.Write(Level);
            writer.Write((byte) Class);
            writer.Write((byte) Gender);
            writer.Write(Hair);

            writer.Write(CreationIP);
            writer.Write(CreationDate.ToBinary());

            writer.Write(Banned);
            writer.Write(BanReason);
            writer.Write(ExpiryDate.ToBinary());

            writer.Write(LastIP);
            writer.Write(LastDate.ToBinary());

            writer.Write(Deleted);
            writer.Write(DeleteDate.ToBinary());

            writer.Write(CurrentMapIndex);
            writer.Write(CurrentLocation.X);
            writer.Write(CurrentLocation.Y);
            writer.Write((byte)Direction);
            writer.Write(BindMapIndex);
            writer.Write(BindLocation.X);
            writer.Write(BindLocation.Y);

            writer.Write(HP);
            writer.Write(MP);
            writer.Write(Experience);

            writer.Write((byte) AMode);
            writer.Write((byte) PMode);

            writer.Write(PKPoints);

            writer.Write(Inventory.Length);
            for (int i = 0; i < Inventory.Length; i++)
            {
                writer.Write(Inventory[i] != null);
                if (Inventory[i] == null) continue;

                Inventory[i].Save(writer);
            }

            writer.Write(Equipment.Length);
            for (int i = 0; i < Equipment.Length; i++)
            {
                writer.Write(Equipment[i] != null);
                if (Equipment[i] == null) continue;

                Equipment[i].Save(writer);
            }

            writer.Write(QuestInventory.Length);
            for (int i = 0; i < QuestInventory.Length; i++)
            {
                writer.Write(QuestInventory[i] != null);
                if (QuestInventory[i] == null) continue;

                QuestInventory[i].Save(writer);
            }

            writer.Write(Magics.Count);
            for (int i = 0; i < Magics.Count; i++)
                Magics[i].Save(writer);

            writer.Write(Thrusting);
            writer.Write(HalfMoon);
            writer.Write(CrossHalfMoon);
            writer.Write(DoubleSlash);


            writer.Write(Pets.Count);
            for (int i = 0; i < Pets.Count; i++)
                Pets[i].Save(writer);

            writer.Write(AllowGroup);

            for (int i = 0; i < Flags.Length; i++)
                writer.Write(Flags[i]);
            writer.Write(GuildIndex);

            writer.Write(AllowTrade);

            writer.Write(CurrentQuests.Count);
            for (int i = 0; i < CurrentQuests.Count; i++)
                CurrentQuests[i].Save(writer);

            writer.Write(Buffs.Count);
            for (int i = 0; i < Buffs.Count; i++)
                Buffs[i].Save(writer);

            writer.Write(Mail.Count);
            for (int i = 0; i < Mail.Count; i++)
                Mail[i].Save(writer);

            //IntelligentCreature
            writer.Write(IntelligentCreatures.Count);
            for (int i = 0; i < IntelligentCreatures.Count; i++)
                IntelligentCreatures[i].Save(writer);
            writer.Write((byte)SummonedCreatureType);
            writer.Write(CreatureSummoned);
            writer.Write(PearlCount);

        }

        public ListViewItem CreateListView()
        {
            if (ListItem != null)
                ListItem.Remove();

            ListItem = new ListViewItem(Index.ToString()) { Tag = this };

            ListItem.SubItems.Add(Name);
            ListItem.SubItems.Add(Level.ToString());
            ListItem.SubItems.Add(Class.ToString());
            ListItem.SubItems.Add(Gender.ToString());

            return ListItem;
        }

        public SelectInfo ToSelectInfo()
        {
            return new SelectInfo
                {
                    Index = Index,
                    Name = Name,
                    Level = Level,
                    Class = Class,
                    Gender = Gender,
                    LastAccess = LastDate
                };
        }

        //IntelligentCreature
        public bool CheckHasIntelligentCreature(IntelligentCreatureType petType)
        {
            for (int i = 0; i < IntelligentCreatures.Count; i++)
                if (IntelligentCreatures[i].PetType == petType) return true;
            return false;
        }
    }

    public class PetInfo
    {
        public int MonsterIndex;
        public uint HP, Experience;
        public byte Level, MaxPetLevel;

        public PetInfo(MonsterObject ob)
        {
            MonsterIndex = ob.Info.Index;
            HP = ob.HP;
            Experience = ob.PetExperience;
            Level = ob.PetLevel;
            MaxPetLevel = ob.MaxPetLevel;
        }

        public PetInfo(BinaryReader reader)
        {
            MonsterIndex = reader.ReadInt32();
            if (MonsterIndex == 271) MonsterIndex = 275;
            HP = reader.ReadUInt32();
            Experience = reader.ReadUInt32();
            Level = reader.ReadByte();
            MaxPetLevel = reader.ReadByte();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(MonsterIndex);
            writer.Write(HP);
            writer.Write(Experience);
            writer.Write(Level);
            writer.Write(MaxPetLevel);
        }
    }

    public class MountInfo
    {
        public PlayerObject Player;
        public short MountType = -1;

        public bool CanRide
        {
            get { return HasMount && Slots[(int)MountSlot.Saddle] != null; }
        }
        public bool CanMapRide
        {
            get { return HasMount && !Player.CurrentMap.Info.NoMount; }
        }
        public bool CanDungeonRide
        {
            get { return HasMount && CanMapRide && (!Player.CurrentMap.Info.NeedBridle || Slots[(int)MountSlot.Reins] != null); }
        }
        public bool CanAttack
        {
            get { return HasMount && Slots[(int)MountSlot.Bells] != null || !RidingMount; }
        }
        public bool SlowLoyalty
        {
            get { return HasMount && Slots[(int)MountSlot.Ribbon] != null; }
        }

        public bool HasMount
        {
            get { return Player.Info.Equipment[(int)EquipmentSlot.Mount] != null; }
        }

        private bool RidingMount
        {
            get { return Player.RidingMount; }
            set { Player.RidingMount = value; }
        }

        public UserItem[] Slots
        {
            get { return Player.Info.Equipment[(int)EquipmentSlot.Mount].Slots; }
        }


        public MountInfo(PlayerObject ob)
        {
            Player = ob;
        }
    }

    //IntelligentCreature
    public class IntelligentCreatureInfo
    {
        public static List<IntelligentCreatureInfo> Creatures = new List<IntelligentCreatureInfo>();

        public static IntelligentCreatureInfo BabyPig,
                                                Chick,
                                                Kitten,
                                                BabySkeleton,
                                                Baekdon,
                                                Wimaen,
                                                BlackKitten,
                                                BabyDragon,
                                                OlympicFlame,
                                                BabySnowMan;

        public IntelligentCreatureType PetType;

        public int Icon;
        public int MinimalFullness = 1;

        public bool MousePickupEnabled = false;
        public int MousePickupRange = 0;
        public bool AutoPickupEnabled = false;
        public int AutoPickupRange = 0;
        public bool SemiAutoPickupEnabled = false;
        public int SemiAutoPickupRange = 0;

        public bool CanProduceBlackStone = false;

        public string Info = "";
        public string Info1 = "";
        public string Info2 = "";

        static IntelligentCreatureInfo()
        {
            BabyPig = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BabyPig, Icon = 500, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 3, MinimalFullness = 7100, Info = "Can pickup items (3x3 semi-auto).", Info1 = "Unable to produce BlackStones.", Info2 = "Can produce Pearls, used to buy Creature items." };
            Chick = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.Chick, Icon = 501, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 7, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 7, CanProduceBlackStone = true, Info = "Can pickup items (7x7 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones.", Info2 = "Can produce Pearls, used to buy Creature items." };
            Kitten = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.Kitten, Icon = 502, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 3, MinimalFullness = 7100, Info = "Can pickup items (5x5 semi-auto).", Info1 = "Unable to produce BlackStones.", Info2 = "Can produce Pearls, used to buy Creature items." };
            BabySkeleton = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BabySkeleton, Icon = 503, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 7, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 7, CanProduceBlackStone = true, Info = "Can pickup items (7x7 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones.", Info2 = "Can produce Pearls, used to buy Creature items." };
            Baekdon = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.Baekdon, Icon = 504, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 7, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 7, CanProduceBlackStone = true, Info = "Can pickup items (7x7 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones.", Info2 = "Can produce Pearls, used to buy Creature items." };
            Wimaen = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.Wimaen, Icon = 505, MousePickupEnabled = true, MousePickupRange = 7, AutoPickupEnabled = true, AutoPickupRange = 5, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 5, MinimalFullness = 7100, Info = "Can pickup items (5x5 auto/semi-auto, 7x7 mouse).", Info1 = "Unable to produce BlackStones.", Info2 = "Can produce Pearls, used to buy Creature items." };
            BlackKitten = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BlackKitten, Icon = 506 };
            BabyDragon = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BabyDragon, Icon = 507 };
            OlympicFlame = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.OlympicFlame, Icon = 508 };
            BabySnowMan = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BabySnowMan, Icon = 509 };
        }

        public IntelligentCreatureInfo()
        {
            Creatures.Add(this);
        }

        public static IntelligentCreatureInfo GetCreatureInfo(IntelligentCreatureType petType)
        {
            for (int i = 0; i < Creatures.Count; i++)
            {
                IntelligentCreatureInfo info = Creatures[i];
                if (info.PetType != petType) continue;
                return info;
            }
            return null;
        }
    }
    public class UserIntelligentCreature
    {
        public IntelligentCreatureType PetType;
        public IntelligentCreatureInfo Info;
        public IntelligentCreatureItemFilter Filter;

        public IntelligentCreaturePickupMode petMode = IntelligentCreaturePickupMode.SemiAutomatic;

        public string CustomName;
        public int Fullness;
        public int SlotIndex;
        public long ExpireTime = -9999;//
        public long BlackstoneTime = 0;

        public UserIntelligentCreature(IntelligentCreatureType creatureType, int slot, byte effect = 0)
        {
            PetType = creatureType;
            Info = IntelligentCreatureInfo.GetCreatureInfo(PetType);
            CustomName = Settings.IntelligentCreatureNameList[(byte)PetType];
            Fullness = 6000;//starts at 51% feed
            SlotIndex = slot;

            if (effect > 0) ExpireTime = effect * 86400;//effect holds the amount in days
            else ExpireTime = -9999;

            BlackstoneTime = 0;

            Filter = new IntelligentCreatureItemFilter();
        }

        public UserIntelligentCreature(BinaryReader reader)
        {
            PetType = (IntelligentCreatureType)reader.ReadByte();
            Info = IntelligentCreatureInfo.GetCreatureInfo(PetType);

            CustomName = reader.ReadString();
            Fullness = reader.ReadInt32();
            SlotIndex = reader.ReadInt32();
            ExpireTime = reader.ReadInt64();
            BlackstoneTime = reader.ReadInt64();

            petMode = (IntelligentCreaturePickupMode)reader.ReadByte();

            Filter = new IntelligentCreatureItemFilter(reader);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write((byte)PetType);

            writer.Write(CustomName);
            writer.Write(Fullness);
            writer.Write(SlotIndex);
            writer.Write(ExpireTime);
            writer.Write(BlackstoneTime);

            writer.Write((byte)petMode);

            Filter.Save(writer);
        }

        public Packet GetInfo()
        {
            return new ServerPackets.NewIntelligentCreature
            {
                Creature = CreateClientIntelligentCreature()
            };
        }

        public ClientIntelligentCreature CreateClientIntelligentCreature()
        {
            return new ClientIntelligentCreature
            {
                PetType = PetType,
                Icon = Info.Icon,
                CustomName = CustomName,
                Fullness = Fullness,
                SlotIndex = SlotIndex,
                ExpireTime = ExpireTime,
                BlackstoneTime = BlackstoneTime,

                petMode = petMode,

                CreatureRules = new IntelligentCreatureRules
                {
                    MinimalFullness = Info.MinimalFullness,
                    MousePickupEnabled = Info.MousePickupEnabled,
                    MousePickupRange = Info.MousePickupRange,
                    AutoPickupEnabled = Info.AutoPickupEnabled,
                    AutoPickupRange = Info.AutoPickupRange,
                    SemiAutoPickupEnabled = Info.SemiAutoPickupEnabled,
                    SemiAutoPickupRange = Info.SemiAutoPickupRange,
                    CanProduceBlackStone = Info.CanProduceBlackStone,
                    Info = Info.Info,
                    Info1 = Info.Info1,
                    Info2 = Info.Info2
                },

                Filter = Filter
            };
        }
    }
}