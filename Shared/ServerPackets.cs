using System.Drawing;

namespace ServerPackets
{
    public sealed class KeepAlive : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.KeepAlive; }
        }
        public long Time;

        protected override void ReadPacket(BinaryReader reader)
        {
            Time = reader.ReadInt64();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Time);
        }
    }
    public sealed class Connected : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.Connected; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class ClientVersion : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ClientVersion; }
        }

        public byte Result;
        /*
         * 0: Wrong Version
         * 1: Correct Version
         */

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
        }
    }
    public sealed class Disconnect : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.Disconnect; }
        }

        public byte Reason;

        /*
         * 0: Server Closing.
         * 1: Another User.
         * 2: Packet Error.
         * 3: Server Crashed.
         */

        protected override void ReadPacket(BinaryReader reader)
        {
            Reason = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Reason);
        }
    }
    public sealed class NewAccount : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewAccount; }
        }

        public byte Result;
        /*
         * 0: Disabled
         * 1: Bad AccountID
         * 2: Bad Password
         * 3: Bad Email
         * 4: Bad Name
         * 5: Bad Question
         * 6: Bad Answer
         * 7: Account Exists.
         * 8: Success
         */

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
        }

    }
    public sealed class ChangePassword : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ChangePassword; }
        }

        public byte Result;
        /*
         * 0: Disabled
         * 1: Bad AccountID
         * 2: Bad Current Password
         * 3: Bad New Password
         * 4: Account Not Exist
         * 5: Wrong Password
         * 6: Success
         */

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
        }
    }
    public sealed class ChangePasswordBanned : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ChangePasswordBanned; }
        }

        public string Reason = string.Empty;
        public DateTime ExpiryDate;

        protected override void ReadPacket(BinaryReader reader)
        {
            Reason = reader.ReadString();
            ExpiryDate = DateTime.FromBinary(reader.ReadInt64());
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Reason);
            writer.Write(ExpiryDate.ToBinary());
        }
    }
    public sealed class Login : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.Login; }
        }

        public byte Result;
        /*
         * 0: Disabled
         * 1: Bad AccountID
         * 2: Bad Password
         * 3: Account Not Exist
         * 4: Wrong Password
         */

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
        }
    }
    public sealed class LoginBanned : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.LoginBanned; }
        }

        public string Reason = string.Empty;
        public DateTime ExpiryDate;

        protected override void ReadPacket(BinaryReader reader)
        {
            Reason = reader.ReadString();
            ExpiryDate = DateTime.FromBinary(reader.ReadInt64());
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Reason);
            writer.Write(ExpiryDate.ToBinary());
        }
    }
    public sealed class LoginSuccess : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.LoginSuccess; }
        }

        public List<SelectInfo> Characters = new List<SelectInfo>();

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                Characters.Add(new SelectInfo(reader));
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Characters.Count);

            for (int i = 0; i < Characters.Count; i++)
                Characters[i].Save(writer);
        }
    }
    public sealed class NewCharacter : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewCharacter; }
        }

        /*
         * 0: Disabled.
         * 1: Bad Character Name
         * 2: Bad Gender
         * 3: Bad Class
         * 4: Max Characters
         * 5: Character Exists.
         * 
         * 10: Success
         * */
        public byte Result;

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
        }
    }
    public sealed class NewCharacterSuccess : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewCharacterSuccess; }
        }

        public SelectInfo CharInfo;

        protected override void ReadPacket(BinaryReader reader)
        {
            CharInfo = new SelectInfo(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            CharInfo.Save(writer);
        }
    }
    public sealed class DeleteCharacter : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.DeleteCharacter; }
        }

        public byte Result;

        /*
         * 0: Disabled.
         * 1: Character Not Found
         * */

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
        }
    }
    public sealed class DeleteCharacterSuccess : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.DeleteCharacterSuccess; }
        }

        public int CharacterIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            CharacterIndex = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CharacterIndex);
        }
    }
    public sealed class StartGame : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.StartGame; }
        }

        public byte Result;
        public int Resolution;

        /*
         * 0: Disabled.
         * 1: Not logged in
         * 2: Character not found.
         * 3: Start Game Error
         * */

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadByte();
            Resolution = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
            writer.Write(Resolution);
        }
    }
    public sealed class StartGameBanned : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.StartGameBanned; }
        }

        public string Reason = string.Empty;
        public DateTime ExpiryDate;

        protected override void ReadPacket(BinaryReader reader)
        {
            Reason = reader.ReadString();
            ExpiryDate = DateTime.FromBinary(reader.ReadInt64());
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Reason);
            writer.Write(ExpiryDate.ToBinary());
        }
    }
    public sealed class StartGameDelay : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.StartGameDelay; }
        }

        public long Milliseconds;

        protected override void ReadPacket(BinaryReader reader)
        {
            Milliseconds = reader.ReadInt64();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Milliseconds);
        }

    }
    public sealed class MapInformation : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.MapInformation; }
        }

        public int MapIndex;
        public string FileName = string.Empty;
        public string Title = string.Empty;
        public ushort MiniMap, BigMap, Music;
        public LightSetting Lights;
        public bool Lightning, Fire;
        public byte MapDarkLight;

        protected override void ReadPacket(BinaryReader reader)
        {
            MapIndex = reader.ReadInt32();
            FileName = reader.ReadString();
            Title = reader.ReadString();
            MiniMap = reader.ReadUInt16();
            BigMap = reader.ReadUInt16();
            Lights = (LightSetting)reader.ReadByte();
            byte bools = reader.ReadByte();
            if ((bools & 0x01) == 0x01) Lightning = true;
            if ((bools & 0x02) == 0x02) Fire = true;
            MapDarkLight = reader.ReadByte();
            Music = reader.ReadUInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MapIndex);
            writer.Write(FileName);
            writer.Write(Title);
            writer.Write(MiniMap);
            writer.Write(BigMap);
            writer.Write((byte)Lights);
            byte bools = 0;
            bools |= (byte)(Lightning ? 0x01 : 0);
            bools |= (byte)(Fire ? 0x02 : 0);
            writer.Write(bools);
            writer.Write(MapDarkLight);
            writer.Write(Music);
        }
    }

    public sealed class NewMapInfo : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewMapInfo; }
        }

        public int MapIndex;
        public ClientMapInfo Info;

        protected override void ReadPacket(BinaryReader reader)
        {
            MapIndex = reader.ReadInt32();
            Info = new ClientMapInfo(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MapIndex);
            Info.Save(writer);
        }
    }

    public sealed class WorldMapSetupInfo : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.WorldMapSetup; }
        }

        public WorldMapSetup Setup;
        public int TeleportToNPCCost;

        protected override void ReadPacket(BinaryReader reader)
        {
            Setup = new WorldMapSetup(reader);
            TeleportToNPCCost = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Setup.Save(writer);
            writer.Write(TeleportToNPCCost);
        }
    }

    public sealed class SearchMapResult : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.SearchMapResult; }
        }

        public int MapIndex = -1;
        public uint NPCIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            MapIndex = reader.ReadInt32();
            NPCIndex = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MapIndex);
            writer.Write(NPCIndex);
        }
    }
    public class UserInformation : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UserInformation; }
        }

        public uint ObjectID;
        public uint RealId;
        public string Name = string.Empty;
        public string GuildName = string.Empty;
        public string GuildRank = string.Empty;
        public Color NameColour;
        public MirClass Class;
        public MirGender Gender;
        public ushort Level;
        public Point Location;
        public MirDirection Direction;
        public byte Hair;
        public int HP, MP;
        public long Experience, MaxExperience;

        public LevelEffects LevelEffects;

        public bool HasHero;
        public HeroBehaviour HeroBehaviour;
        public UserItem[] Inventory, Equipment, QuestInventory;
        public uint Gold, Credit;

        public bool HasExpandedStorage;
        public DateTime ExpandedStorageExpiryTime;

        public List<ClientMagic> Magics = new List<ClientMagic>();

        public List<ClientIntelligentCreature> IntelligentCreatures = new List<ClientIntelligentCreature>();
        public IntelligentCreatureType SummonedCreatureType = IntelligentCreatureType.None;
        public bool CreatureSummoned;
        public bool AllowObserve;
        public bool Observer;



        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            RealId = reader.ReadUInt32();
            Name = reader.ReadString();
            GuildName = reader.ReadString();
            GuildRank = reader.ReadString();
            NameColour = Color.FromArgb(reader.ReadInt32());
            Class = (MirClass)reader.ReadByte();
            Gender = (MirGender)reader.ReadByte();
            Level = reader.ReadUInt16();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            Hair = reader.ReadByte();
            HP = reader.ReadInt32();
            MP = reader.ReadInt32();

            Experience = reader.ReadInt64();
            MaxExperience = reader.ReadInt64();

            LevelEffects = (LevelEffects)reader.ReadUInt16();
            HasHero = reader.ReadBoolean();
            HeroBehaviour = (HeroBehaviour)reader.ReadByte();

            if (reader.ReadBoolean())
            {
                Inventory = new UserItem[reader.ReadInt32()];
                for (int i = 0; i < Inventory.Length; i++)
                {
                    if (!reader.ReadBoolean()) continue;
                    Inventory[i] = new UserItem(reader);
                }
            }

            if (reader.ReadBoolean())
            {
                Equipment = new UserItem[reader.ReadInt32()];
                for (int i = 0; i < Equipment.Length; i++)
                {
                    if (!reader.ReadBoolean()) continue;
                    Equipment[i] = new UserItem(reader);
                }
            }

            if (reader.ReadBoolean())
            {
                QuestInventory = new UserItem[reader.ReadInt32()];
                for (int i = 0; i < QuestInventory.Length; i++)
                {
                    if (!reader.ReadBoolean()) continue;
                    QuestInventory[i] = new UserItem(reader);
                }
            }

            Gold = reader.ReadUInt32();
            Credit = reader.ReadUInt32();

            HasExpandedStorage = reader.ReadBoolean();
            ExpandedStorageExpiryTime = DateTime.FromBinary(reader.ReadInt64());

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                Magics.Add(new ClientMagic(reader));
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                IntelligentCreatures.Add(new ClientIntelligentCreature(reader));
            }
            SummonedCreatureType = (IntelligentCreatureType)reader.ReadByte();
            CreatureSummoned = reader.ReadBoolean();
            AllowObserve = reader.ReadBoolean();
            Observer = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(RealId);
            writer.Write(Name);
            writer.Write(GuildName);
            writer.Write(GuildRank);
            writer.Write(NameColour.ToArgb());
            writer.Write((byte)Class);
            writer.Write((byte)Gender);
            writer.Write(Level);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
            writer.Write(Hair);
            writer.Write(HP);
            writer.Write(MP);

            writer.Write(Experience);
            writer.Write(MaxExperience);

            writer.Write((ushort)LevelEffects);
            writer.Write(HasHero);
            writer.Write((byte)HeroBehaviour);

            writer.Write(Inventory != null);
            if (Inventory != null)
            {
                writer.Write(Inventory.Length);
                for (int i = 0; i < Inventory.Length; i++)
                {
                    writer.Write(Inventory[i] != null);
                    if (Inventory[i] == null) continue;

                    Inventory[i].Save(writer);
                }

            }

            writer.Write(Equipment != null);
            if (Equipment != null)
            {
                writer.Write(Equipment.Length);
                for (int i = 0; i < Equipment.Length; i++)
                {
                    writer.Write(Equipment[i] != null);
                    if (Equipment[i] == null) continue;

                    Equipment[i].Save(writer);
                }
            }

            writer.Write(QuestInventory != null);
            if (QuestInventory != null)
            {
                writer.Write(QuestInventory.Length);
                for (int i = 0; i < QuestInventory.Length; i++)
                {
                    writer.Write(QuestInventory[i] != null);
                    if (QuestInventory[i] == null) continue;

                    QuestInventory[i].Save(writer);
                }
            }

            writer.Write(Gold);
            writer.Write(Credit);

            writer.Write(HasExpandedStorage);
            writer.Write(ExpandedStorageExpiryTime.ToBinary());

            writer.Write(Magics.Count);
            for (int i = 0; i < Magics.Count; i++)
            {
                Magics[i].Save(writer);
            }

            writer.Write(IntelligentCreatures.Count);
            for (int i = 0; i < IntelligentCreatures.Count; i++)
            {
                IntelligentCreatures[i].Save(writer);
            }

            writer.Write((byte)SummonedCreatureType);
            writer.Write(CreatureSummoned);
            writer.Write(AllowObserve);
            writer.Write(Observer);
        }
    }

    public sealed class UserSlotsRefresh : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UserSlotsRefresh; }
        }
  
        public UserItem[] Inventory, Equipment;

        protected override void ReadPacket(BinaryReader reader)
        {
            
            if (reader.ReadBoolean())
            {
                Inventory = new UserItem[reader.ReadInt32()];
                for (int i = 0; i < Inventory.Length; i++)
                {
                    if (!reader.ReadBoolean()) continue;
                    Inventory[i] = new UserItem(reader);
                }
            }

            if (reader.ReadBoolean())
            {
                Equipment = new UserItem[reader.ReadInt32()];
                for (int i = 0; i < Equipment.Length; i++)
                {
                    if (!reader.ReadBoolean()) continue;
                    Equipment[i] = new UserItem(reader);
                }
            }
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Inventory != null);
            if (Inventory != null)
            {
                writer.Write(Inventory.Length);
                for (int i = 0; i < Inventory.Length; i++)
                {
                    writer.Write(Inventory[i] != null);
                    if (Inventory[i] == null) continue;

                    Inventory[i].Save(writer);
                }

            }

            writer.Write(Equipment != null);
            if (Equipment != null)
            {
                writer.Write(Equipment.Length);
                for (int i = 0; i < Equipment.Length; i++)
                {
                    writer.Write(Equipment[i] != null);
                    if (Equipment[i] == null) continue;

                    Equipment[i].Save(writer);
                }
            }
        }
    }

    public sealed class UserLocation : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UserLocation; }
        }

        public override bool Observable
        {
            get { return false; }
        }

        public Point Location;
        public MirDirection Direction;


        protected override void ReadPacket(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public class ObjectPlayer : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectPlayer; }
        }

        public uint ObjectID;
        public string Name = string.Empty;
        public string GuildName = string.Empty;
        public string GuildRankName = string.Empty;
        public Color NameColour;
        public MirClass Class;
        public MirGender Gender;
        public ushort Level;
        public Point Location;
        public MirDirection Direction;
        public byte Hair;
        public byte Light;
		public short Weapon, WeaponEffect, Armour;
		public PoisonType Poison;
        public bool Dead, Hidden;
        public SpellEffect Effect;
        public byte WingEffect;
        public bool Extra;

        public short MountType;
        public bool RidingMount;
        public bool Fishing;

        public short TransformType;

        public uint ElementOrbEffect;
        public uint ElementOrbLvl;
        public uint ElementOrbMax;

        public LevelEffects LevelEffects;

        public List<BuffType> Buffs = new List<BuffType>();

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Name = reader.ReadString();
            GuildName = reader.ReadString();
            GuildRankName = reader.ReadString();
            NameColour = Color.FromArgb(reader.ReadInt32());
            Class = (MirClass)reader.ReadByte();
            Gender = (MirGender)reader.ReadByte();
            Level = reader.ReadUInt16();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            Hair = reader.ReadByte();
            Light = reader.ReadByte();
            Weapon = reader.ReadInt16();
			WeaponEffect = reader.ReadInt16();
			Armour = reader.ReadInt16();
            Poison = (PoisonType)reader.ReadUInt16();
            Dead = reader.ReadBoolean();
            Hidden = reader.ReadBoolean();
            Effect = (SpellEffect)reader.ReadByte();
            WingEffect = reader.ReadByte();
            Extra = reader.ReadBoolean();
            MountType = reader.ReadInt16();
            RidingMount = reader.ReadBoolean();
            Fishing = reader.ReadBoolean();

            TransformType = reader.ReadInt16();

            ElementOrbEffect = reader.ReadUInt32();
            ElementOrbLvl = reader.ReadUInt32();
            ElementOrbMax = reader.ReadUInt32();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Buffs.Add((BuffType)reader.ReadByte());
            }

            LevelEffects = (LevelEffects)reader.ReadUInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Name);
            writer.Write(GuildName);
            writer.Write(GuildRankName);
            writer.Write(NameColour.ToArgb());
            writer.Write((byte)Class);
            writer.Write((byte)Gender);
            writer.Write(Level);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
            writer.Write(Hair);
            writer.Write(Light);
            writer.Write(Weapon);
			writer.Write(WeaponEffect);
			writer.Write(Armour);
            writer.Write((ushort)Poison);
            writer.Write(Dead);
            writer.Write(Hidden);
            writer.Write((byte)Effect);
            writer.Write(WingEffect);
            writer.Write(Extra);
            writer.Write(MountType);
            writer.Write(RidingMount);
            writer.Write(Fishing);

            writer.Write(TransformType);

            writer.Write(ElementOrbEffect);
            writer.Write(ElementOrbLvl);
            writer.Write(ElementOrbMax);

            writer.Write(Buffs.Count);
            for (int i = 0; i < Buffs.Count; i++)
            {
                writer.Write((byte)Buffs[i]);
            }

            writer.Write((ushort)LevelEffects);
        }
    }

    public sealed class ObjectHero : ObjectPlayer
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectHero; }
        }

        public string OwnerName;

        protected override void ReadPacket(BinaryReader reader)
        {
            base.ReadPacket(reader);

            OwnerName = reader.ReadString();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            base.WritePacket(writer);

            writer.Write(OwnerName);
        }
    }
    public sealed class ObjectRemove : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectRemove; }
        }

        public uint ObjectID;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
        }

    }
    public sealed class ObjectTurn : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectTurn; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class ObjectWalk : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectWalk; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class ObjectRun : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectRun; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class Chat : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.Chat; }
        }

        public override bool Observable
        {
            get { return Type != ChatType.WhisperIn && Type != ChatType.WhisperOut; }
        }

        public string Message = string.Empty;
        public ChatType Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            Message = reader.ReadString();
            Type = (ChatType)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Message);
            writer.Write((byte)Type);
        }
    }
    public sealed class ObjectChat : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectChat; }
        }

        public uint ObjectID;
        public string Text = string.Empty;
        public ChatType Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Text = reader.ReadString();
            Type = (ChatType)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Text);
            writer.Write((byte)Type);
        }
    }
    public sealed class NewItemInfo : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewItemInfo; }
        }

        public ItemInfo Info;

        protected override void ReadPacket(BinaryReader reader)
        {
            Info = new ItemInfo(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Info.Save(writer);
        }
    }
    public sealed class NewHeroInfo : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewHeroInfo; }
        }

        public ClientHeroInformation Info;
        public int StorageIndex = -1;

        protected override void ReadPacket(BinaryReader reader)
        {
            Info = new ClientHeroInformation(reader);
            StorageIndex = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Info.Save(writer);
            writer.Write(StorageIndex);
        }
    }
    public sealed class NewChatItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewChatItem; }
        }

        public UserItem Item;

        protected override void ReadPacket(BinaryReader reader)
        {
            Item = new UserItem(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Item.Save(writer);
        }
    }
    public sealed class MoveItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.MoveItem; }
        }

        public MirGridType Grid;
        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }
    public sealed class EquipItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.EquipItem; }
        }

        public MirGridType Grid;
        public ulong UniqueID;
        public int To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(UniqueID);
            writer.Write(To);
            writer.Write(Success);
        }
    }
    public sealed class MergeItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.MergeItem; }
        }

        public MirGridType GridFrom, GridTo;
        public ulong IDFrom, IDTo;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            GridFrom = (MirGridType)reader.ReadByte();
            GridTo = (MirGridType)reader.ReadByte();
            IDFrom = reader.ReadUInt64();
            IDTo = reader.ReadUInt64();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)GridFrom);
            writer.Write((byte)GridTo);
            writer.Write(IDFrom);
            writer.Write(IDTo);
            writer.Write(Success);
        }
    }
    public sealed class RemoveItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.RemoveItem; }
        }

        public MirGridType Grid;
        public ulong UniqueID;
        public int To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(UniqueID);
            writer.Write(To);
            writer.Write(Success);
        }
    }
    public sealed class RemoveSlotItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.RemoveSlotItem; }
        }

        public MirGridType Grid;
        public MirGridType GridTo;
        public ulong UniqueID;
        public int To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            GridTo = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write((byte)GridTo);
            writer.Write(UniqueID);
            writer.Write(To);
            writer.Write(Success);
        }
    }
    public sealed class TakeBackItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.TakeBackItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }
    public sealed class StoreItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.StoreItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }

    public sealed class DepositRefineItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.DepositRefineItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }

    public sealed class RetrieveRefineItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.RetrieveRefineItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }

    public sealed class RefineCancel : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.RefineCancel; }
        }

        public bool Unlock;
        protected override void ReadPacket(BinaryReader reader)
        {
            Unlock = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Unlock);
        }
    }

    public sealed class RefineItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.RefineItem; }
        }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }

    public sealed class DepositTradeItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.DepositTradeItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }
    public sealed class RetrieveTradeItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.RetrieveTradeItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }
    public sealed class SplitItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.SplitItem; }
        }

        public UserItem Item;
        public MirGridType Grid;

        protected override void ReadPacket(BinaryReader reader)
        {
            if (reader.ReadBoolean())
                Item = new UserItem(reader);

            Grid = (MirGridType)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Item != null);
            if (Item != null) Item.Save(writer);
            writer.Write((byte)Grid);
        }
    }
    public sealed class SplitItem1 : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.SplitItem1; }
        }

        public MirGridType Grid;
        public ulong UniqueID;
        public ushort Count;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt16();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(UniqueID);
            writer.Write(Count);
            writer.Write(Success);
        }
    }
    public sealed class UseItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UseItem; }
        }

        public ulong UniqueID;
        public bool Success;
        public MirGridType Grid;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Success = reader.ReadBoolean();
            Grid = (MirGridType)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Success);
            writer.Write((byte)Grid);
        }
    }
    public sealed class DropItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.DropItem; }
        }

        public ulong UniqueID;
        public ushort Count;
        public bool HeroItem = false;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt16();
            HeroItem = reader.ReadBoolean();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Count);
            writer.Write(HeroItem);
            writer.Write(Success);
        }
    }

    public sealed class TakeBackHeroItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.TakeBackHeroItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }

    public sealed class TransferHeroItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.TransferHeroItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }
    public sealed class PlayerUpdate : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.PlayerUpdate; }
        }

        public uint ObjectID;
        public byte Light;
		public short Weapon, WeaponEffect, Armour;
		public byte WingEffect;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();

            Light = reader.ReadByte();
            Weapon = reader.ReadInt16();
			WeaponEffect = reader.ReadInt16();
			Armour = reader.ReadInt16();
            WingEffect = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);

            writer.Write(Light);
            writer.Write(Weapon);
			writer.Write(WeaponEffect);
			writer.Write(Armour);
            writer.Write(WingEffect);
        }
    }
    public sealed class PlayerInspect : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.PlayerInspect; }
        }

        public override bool Observable
        {
            get { return false; }
        }

        public string Name = string.Empty;
        public string GuildName = string.Empty;
        public string GuildRank = string.Empty;
        public UserItem[] Equipment;
        public MirClass Class;
        public MirGender Gender;
        public byte Hair;
        public ushort Level;
        public string LoverName;
        public bool AllowObserve;
        public bool IsHero = false;

        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
            GuildName = reader.ReadString();
            GuildRank = reader.ReadString();
            Equipment = new UserItem[reader.ReadInt32()];
            for (int i = 0; i < Equipment.Length; i++)
            {
                if (reader.ReadBoolean())
                    Equipment[i] = new UserItem(reader);
            }

            Class = (MirClass)reader.ReadByte();
            Gender = (MirGender)reader.ReadByte();
            Hair = reader.ReadByte();
            Level = reader.ReadUInt16();
            LoverName = reader.ReadString();
            AllowObserve = reader.ReadBoolean();
            IsHero = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(GuildName);
            writer.Write(GuildRank);
            writer.Write(Equipment.Length);
            for (int i = 0; i < Equipment.Length; i++)
            {
                UserItem T = Equipment[i];
                writer.Write(T != null);
                if (T != null) T.Save(writer);
            }

            writer.Write((byte)Class);
            writer.Write((byte)Gender);
            writer.Write(Hair);
            writer.Write(Level);
            writer.Write(LoverName);
            writer.Write(AllowObserve);
            writer.Write(IsHero);
        }
    }

    public sealed class MarriageRequest : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.MarriageRequest; } }

        public string Name;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }

    public sealed class DivorceRequest : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.DivorceRequest; } }

        public string Name;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }

    public sealed class MentorRequest : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.MentorRequest; } }

        public string Name;
        public ushort Level;

        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
            Level = reader.ReadUInt16();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Level);
        }
    }

    public sealed class TradeRequest : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.TradeRequest; } }

        public string Name;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class TradeAccept : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.TradeAccept; } }

        public string Name;

        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class TradeGold : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.TradeGold; }
        }

        public uint Amount;

        protected override void ReadPacket(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Amount);
        }
    }
    public sealed class TradeItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.TradeItem; }
        }

        public UserItem[] TradeItems;

        protected override void ReadPacket(BinaryReader reader)
        {
            TradeItems = new UserItem[reader.ReadInt32()];
            for (int i = 0; i < TradeItems.Length; i++)
            {
                if (reader.ReadBoolean())
                    TradeItems[i] = new UserItem(reader);
            }
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(TradeItems.Length);
            for (int i = 0; i < TradeItems.Length; i++)
            {
                UserItem T = TradeItems[i];
                writer.Write(T != null);
                if (T != null) T.Save(writer);
            }
        }
    }
    public sealed class TradeConfirm : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.TradeConfirm; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class TradeCancel : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.TradeCancel; }
        }

        public bool Unlock;
        protected override void ReadPacket(BinaryReader reader)
        {
            Unlock = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Unlock);
        }
    }

    public sealed class LogOutSuccess : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.LogOutSuccess; }
        }

        public override bool Observable => false;

        public List<SelectInfo> Characters = new List<SelectInfo>();

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                Characters.Add(new SelectInfo(reader));
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Characters.Count);

            for (int i = 0; i < Characters.Count; i++)
                Characters[i].Save(writer);
        }
    }
    public sealed class LogOutFailed : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.LogOutFailed; }
        }

        public override bool Observable => false;

        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class ReturnToLogin : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ReturnToLogin; }
        }

        public override bool Observable => false;

        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class TimeOfDay : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.TimeOfDay; }
        }

        public LightSetting Lights;

        protected override void ReadPacket(BinaryReader reader)
        {
            Lights = (LightSetting)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Lights);
        }
    }
    public sealed class ChangeAMode : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ChangeAMode; }
        }

        public AttackMode Mode;

        protected override void ReadPacket(BinaryReader reader)
        {
            Mode = (AttackMode)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Mode);
        }
    }
    public sealed class ChangePMode : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ChangePMode; }
        }

        public PetMode Mode;

        protected override void ReadPacket(BinaryReader reader)
        {
            Mode = (PetMode)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Mode);
        }
    }
    public sealed class ObjectItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectItem; }
        }

        public uint ObjectID;
        public string Name = string.Empty;
        public Color NameColour;
        public Point Location;
        public ushort Image;
        public ItemGrade grade;


        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Name = reader.ReadString();
            NameColour = Color.FromArgb(reader.ReadInt32());
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Image = reader.ReadUInt16();
            grade = (ItemGrade)reader.ReadByte();
		}

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Name);
            writer.Write(NameColour.ToArgb());
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Image);
            writer.Write((byte)grade);
		}
    }
    public sealed class ObjectGold : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectGold; }
        }

        public uint ObjectID;
        public uint Gold;
        public Point Location;


        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Gold = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Gold);
            writer.Write(Location.X);
            writer.Write(Location.Y);
        }
    }
    public sealed class GainedItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GainedItem; }
        }

        public UserItem Item;

        protected override void ReadPacket(BinaryReader reader)
        {
            Item = new UserItem(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Item.Save(writer);
        }
    }
    public sealed class GainedGold : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GainedGold; }
        }

        public uint Gold;

        protected override void ReadPacket(BinaryReader reader)
        {
            Gold = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Gold);
        }
    }
    public sealed class LoseGold : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.LoseGold; }
        }

        public uint Gold;

        protected override void ReadPacket(BinaryReader reader)
        {
            Gold = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Gold);
        }
    }
    public sealed class GainedCredit : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GainedCredit; }
        }

        public uint Credit;

        protected override void ReadPacket(BinaryReader reader)
        {
            Credit = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Credit);
        }
    }
    public sealed class LoseCredit : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.LoseCredit; }
        }

        public uint Credit;

        protected override void ReadPacket(BinaryReader reader)
        {
            Credit = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Credit);
        }
    }

    public sealed class ObjectMonster : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectMonster; }
        }

        public uint ObjectID;
        public string Name = string.Empty;
        public Color NameColour;
        public Point Location;
        public Monster Image;
        public MirDirection Direction;
        public byte Effect, AI, Light;
        public bool Dead, Skeleton;
        public PoisonType Poison;
        public bool Hidden, Extra;
        public byte ExtraByte;
        public long ShockTime;
        public bool BindingShotCenter;

        public List<BuffType> Buffs = new List<BuffType>();

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Name = reader.ReadString();
            NameColour = Color.FromArgb(reader.ReadInt32());
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Image = (Monster)reader.ReadUInt16();
            Direction = (MirDirection)reader.ReadByte();
            Effect = reader.ReadByte();
            AI = reader.ReadByte();
            Light = reader.ReadByte();
            Dead = reader.ReadBoolean();
            Skeleton = reader.ReadBoolean();
            Poison = (PoisonType)reader.ReadUInt16();
            Hidden = reader.ReadBoolean();
            ShockTime = reader.ReadInt64();
            BindingShotCenter = reader.ReadBoolean();
            Extra = reader.ReadBoolean();
            ExtraByte = reader.ReadByte();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Buffs.Add((BuffType)reader.ReadByte());
            }
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Name);
            writer.Write(NameColour.ToArgb());
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((ushort)Image);
            writer.Write((byte)Direction);
            writer.Write(Effect);
            writer.Write(AI);
            writer.Write(Light);
            writer.Write(Dead);
            writer.Write(Skeleton);
            writer.Write((ushort)Poison);
            writer.Write(Hidden);
            writer.Write(ShockTime);
            writer.Write(BindingShotCenter);
            writer.Write(Extra);
            writer.Write((byte)ExtraByte);

            writer.Write(Buffs.Count);
            for (int i = 0; i < Buffs.Count; i++)
            {
                writer.Write((byte)Buffs[i]);
            }
        }

    }
    public sealed class ObjectAttack : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectAttack; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;
        public Spell Spell;
        public byte Level;
        public byte Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            Spell = (Spell)reader.ReadByte();
            Level = reader.ReadByte();
            Type = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
            writer.Write((byte)Spell);
            writer.Write(Level);
            writer.Write(Type);
        }
    }
    public sealed class Struck : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.Struck; }
        }

        public uint AttackerID;

        protected override void ReadPacket(BinaryReader reader)
        {
            AttackerID = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AttackerID);
        }
    }
    public sealed class ObjectStruck : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectStruck; }
        }

        public uint ObjectID;
        public uint AttackerID;
        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            AttackerID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(AttackerID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }

    public sealed class DamageIndicator : Packet
    {
        public int Damage;
        public DamageType Type;
        public uint ObjectID;

        public override short Index
        {
            get { return (short)ServerPacketIds.DamageIndicator; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            Damage = reader.ReadInt32();
            Type = (DamageType)reader.ReadByte();
            ObjectID = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Damage);
            writer.Write((byte)Type);
            writer.Write(ObjectID);
        }
    }

    public sealed class DuraChanged : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.DuraChanged; }
        }

        public ulong UniqueID;
        public ushort CurrentDura;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            CurrentDura = reader.ReadUInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(CurrentDura);
        }
    }
    public sealed class HealthChanged : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.HealthChanged; }
        }

        public int HP, MP;

        protected override void ReadPacket(BinaryReader reader)
        {
            HP = reader.ReadInt32();
            MP = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(HP);
            writer.Write(MP);
        }
    }

    public sealed class HeroHealthChanged : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.HeroHealthChanged; }
        }

        public int HP, MP;

        protected override void ReadPacket(BinaryReader reader)
        {
            HP = reader.ReadInt32();
            MP = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(HP);
            writer.Write(MP);
        }
    }
    public sealed class DeleteItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.DeleteItem; }
        }

        public ulong UniqueID;
        public ushort Count;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Count);
        }
    }
    public sealed class Death : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.Death; }
        }

        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class ObjectDied : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectDied; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;
        public byte Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            Type = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
            writer.Write(Type);
        }
    }
    public sealed class ColourChanged : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ColourChanged; }
        }

        public Color NameColour;

        protected override void ReadPacket(BinaryReader reader)
        {
            NameColour = Color.FromArgb(reader.ReadInt32());
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(NameColour.ToArgb());
        }
    }
    public sealed class ObjectColourChanged : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectColourChanged; }
        }

        public uint ObjectID;
        public Color NameColour;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            NameColour = Color.FromArgb(reader.ReadInt32());
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(NameColour.ToArgb());
        }
    }
    public sealed class ObjectGuildNameChanged : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectGuildNameChanged; }
        }

        public uint ObjectID;
        public string GuildName;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            GuildName = reader.ReadString();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(GuildName);
        }
    }
    public sealed class GainExperience : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GainExperience; }
        }

        public uint Amount;

        protected override void ReadPacket(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Amount);
        }
    }

    public sealed class GainHeroExperience : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GainHeroExperience; }
        }

        public uint Amount;

        protected override void ReadPacket(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Amount);
        }
    }
    public sealed class LevelChanged : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.LevelChanged; }
        }

        public ushort Level;
        public long Experience, MaxExperience;

        protected override void ReadPacket(BinaryReader reader)
        {
            Level = reader.ReadUInt16();
            Experience = reader.ReadInt64();
            MaxExperience = reader.ReadInt64();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Level);
            writer.Write(Experience);
            writer.Write(MaxExperience);
        }
    }

    public sealed class HeroLevelChanged : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.HeroLevelChanged; }
        }

        public ushort Level;
        public long Experience, MaxExperience;

        protected override void ReadPacket(BinaryReader reader)
        {
            Level = reader.ReadUInt16();
            Experience = reader.ReadInt64();
            MaxExperience = reader.ReadInt64();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Level);
            writer.Write(Experience);
            writer.Write(MaxExperience);
        }
    }
    public sealed class ObjectLeveled : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectLeveled; }
        }

        public uint ObjectID;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
        }
    }
    public sealed class ObjectHarvest : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectHarvest; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class ObjectHarvested : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectHarvested; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }

    }
    public sealed class ObjectNPC : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectNpc; }
        }

        public uint ObjectID;
        public string Name = string.Empty;

        public Color NameColour;
        public ushort Image;
        public Color Colour;
        public Point Location;
        public MirDirection Direction;
        public List<int> QuestIDs = new List<int>();

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Name = reader.ReadString();
            NameColour = Color.FromArgb(reader.ReadInt32());
            Image = reader.ReadUInt16();
            Colour = Color.FromArgb(reader.ReadInt32());
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();

            int count = reader.ReadInt32();

            for (var i = 0; i < count; i++)
                QuestIDs.Add(reader.ReadInt32());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Name);
            writer.Write(NameColour.ToArgb());
            writer.Write(Image);
            writer.Write(Colour.ToArgb());
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);

            writer.Write(QuestIDs.Count);

            for (int i = 0; i < QuestIDs.Count; i++)
                writer.Write(QuestIDs[i]);
        }
    }
    public sealed class NPCResponse : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCResponse; } }

        public List<string> Page;

        protected override void ReadPacket(BinaryReader reader)
        {
            Page = new List<string>();

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                Page.Add(reader.ReadString());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Page.Count);

            for (int i = 0; i < Page.Count; i++)
                writer.Write(Page[i]);
        }
    }
    public sealed class ObjectHide : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectHide; } }

        public uint ObjectID;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
        }
    }
    public sealed class ObjectShow : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectShow; } }

        public uint ObjectID;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
        }
    }
    public sealed class Poisoned : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.Poisoned; } }

        public PoisonType Poison;

        protected override void ReadPacket(BinaryReader reader)
        {
            Poison = (PoisonType)reader.ReadUInt16();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((ushort)Poison);
        }
    }
    public sealed class ObjectPoisoned : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectPoisoned; } }

        public uint ObjectID;
        public PoisonType Poison;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Poison = (PoisonType)reader.ReadUInt16();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write((ushort)Poison);
        }
    }
    public sealed class MapChanged : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.MapChanged; }
        }

        public int MapIndex;
        public string FileName = string.Empty;
        public string Title = string.Empty;
        public ushort MiniMap, BigMap, Music;
        public LightSetting Lights;
        public Point Location;
        public MirDirection Direction;
        public byte MapDarkLight;


        protected override void ReadPacket(BinaryReader reader)
        {
            MapIndex = reader.ReadInt32();
            FileName = reader.ReadString();
            Title = reader.ReadString();
            MiniMap = reader.ReadUInt16();
            BigMap = reader.ReadUInt16();
            Lights = (LightSetting)reader.ReadByte();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            MapDarkLight = reader.ReadByte();
            Music = reader.ReadUInt16();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MapIndex);
            writer.Write(FileName);
            writer.Write(Title);
            writer.Write(MiniMap);
            writer.Write(BigMap);
            writer.Write((byte)Lights);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
            writer.Write(MapDarkLight);
            writer.Write(Music);
        }
    }
    public sealed class ObjectTeleportOut : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectTeleportOut; } }

        public uint ObjectID;
        public byte Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Type = reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Type);
        }
    }
    public sealed class ObjectTeleportIn : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectTeleportIn; } }

        public uint ObjectID;
        public byte Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Type = reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Type);
        }
    }
    public sealed class TeleportIn : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.TeleportIn; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class NPCGoods : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCGoods; } }

        public List<UserItem> List = new List<UserItem>();
        public float Rate;
        public PanelType Type;
        public bool HideAddedStats;

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                List.Add(new UserItem(reader));

            Rate = reader.ReadSingle();
            Type = (PanelType)reader.ReadByte();

            HideAddedStats = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(List.Count);

            for (int i = 0; i < List.Count; i++)
                List[i].Save(writer);

            writer.Write(Rate);
            writer.Write((byte)Type);

            writer.Write(HideAddedStats);
        }
    }
    public sealed class NPCSell : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCSell; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class NPCRepair : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCRepair; } }
        public float Rate;

        protected override void ReadPacket(BinaryReader reader)
        {
            Rate = reader.ReadSingle();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Rate);
        }
    }
    public sealed class NPCSRepair : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCSRepair; } }

        public float Rate;

        protected override void ReadPacket(BinaryReader reader)
        {
            Rate = reader.ReadSingle();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Rate);
        }
    }

    public sealed class NPCRefine : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCRefine; } }

        public float Rate;
        public bool Refining;

        protected override void ReadPacket(BinaryReader reader)
        {
            Rate = reader.ReadSingle();
            Refining = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Rate);
            writer.Write(Refining);
        }
    }

    public sealed class NPCCheckRefine : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCCheckRefine; } }


        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class NPCCollectRefine : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCCollectRefine; } }

        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            Success = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Success);
        }
    }

    public sealed class NPCReplaceWedRing : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCReplaceWedRing; } }

        public float Rate;

        protected override void ReadPacket(BinaryReader reader)
        {
            Rate = reader.ReadSingle();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Rate);
        }
    }

    public sealed class NPCStorage : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCStorage; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class SellItem : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SellItem; } }

        public ulong UniqueID;
        public ushort Count;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt16();
            Success = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Count);
            writer.Write(Success);
        }
    }
    public sealed class RepairItem : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.RepairItem; } }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }
    public sealed class ItemRepaired : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ItemRepaired; } }

        public ulong UniqueID;
        public ushort MaxDura, CurrentDura;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            MaxDura = reader.ReadUInt16();
            CurrentDura = reader.ReadUInt16();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(MaxDura);
            writer.Write(CurrentDura);
        }
    }

    public sealed class ItemSlotSizeChanged : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ItemSlotSizeChanged; } }

        public ulong UniqueID;
        public int SlotSize;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            SlotSize = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(SlotSize);
        }
    }

    public sealed class ItemSealChanged : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ItemSealChanged; } }

        public ulong UniqueID;
        public DateTime ExpiryDate;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            ExpiryDate = DateTime.FromBinary(reader.ReadInt64());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(ExpiryDate.ToBinary());
        }
    }


    public sealed class NewMagic : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewMagic; }
        }

        public ClientMagic Magic;
        public bool Hero;
        protected override void ReadPacket(BinaryReader reader)
        {
            Magic = new ClientMagic(reader);
            Hero = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Magic.Save(writer);
            writer.Write(Hero);
        }
    }
    public sealed class RemoveMagic : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.RemoveMagic; }
        }

        public int PlaceId;
        protected override void ReadPacket(BinaryReader reader)
        {
            PlaceId = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(PlaceId);
        }

    }
    public sealed class MagicLeveled : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.MagicLeveled; }
        }

        public uint ObjectID;
        public Spell Spell;
        public byte Level;
        public ushort Experience;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Spell = (Spell)reader.ReadByte();
            Level = reader.ReadByte();
            Experience = reader.ReadUInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write((byte)Spell);
            writer.Write(Level);
            writer.Write(Experience);
        }
    }

    public sealed class Magic : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.Magic; } }

        public Spell Spell;
        public uint TargetID;
        public Point Target;
        public bool Cast;
        public byte Level;

        public List<uint> SecondaryTargetIDs = new List<uint>();

        protected override void ReadPacket(BinaryReader reader)
        {
            Spell = (Spell)reader.ReadByte();
            TargetID = reader.ReadUInt32();
            Target = new Point(reader.ReadInt32(), reader.ReadInt32());
            Cast = reader.ReadBoolean();
            Level = reader.ReadByte();

            var count = reader.ReadInt32();
            SecondaryTargetIDs = new List<uint>();
            for (int i = 0; i < count; i++)
            {
                SecondaryTargetIDs.Add(reader.ReadUInt32());
            }
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Spell);
            writer.Write(TargetID);
            writer.Write(Target.X);
            writer.Write(Target.Y);
            writer.Write(Cast);
            writer.Write(Level);

            writer.Write(SecondaryTargetIDs.Count);
            foreach (var targetID in SecondaryTargetIDs)
            {
                writer.Write(targetID);
            }

        }
    }
    public sealed class MagicDelay : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.MagicDelay; } }

        public uint ObjectID;
        public Spell Spell;
        public long Delay;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Spell = (Spell)reader.ReadByte();
            Delay = reader.ReadInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write((byte)Spell);
            writer.Write(Delay);
        }
    }
    public sealed class MagicCast : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.MagicCast; } }

        public Spell Spell;

        protected override void ReadPacket(BinaryReader reader)
        {
            Spell = (Spell)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Spell);
        }
    }

    public sealed class ObjectMagic : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectMagic; } }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;

        public Spell Spell;
        public uint TargetID;
        public Point Target;
        public bool Cast;
        public byte Level;
        public bool SelfBroadcast = false;
        public List<uint> SecondaryTargetIDs = new List<uint>();

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();

            Spell = (Spell)reader.ReadByte();
            TargetID = reader.ReadUInt32();

            Target = new Point(reader.ReadInt32(), reader.ReadInt32());
            Cast = reader.ReadBoolean();
            Level = reader.ReadByte();
            SelfBroadcast = reader.ReadBoolean();

            var count = reader.ReadInt32();
            SecondaryTargetIDs = new List<uint>();
            for (int i = 0; i < count; i++)
            {
                SecondaryTargetIDs.Add(reader.ReadUInt32());
            }
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);

            writer.Write((byte)Spell);
            writer.Write(TargetID);

            writer.Write(Target.X);
            writer.Write(Target.Y);
            writer.Write(Cast);
            writer.Write(Level);
            writer.Write(SelfBroadcast);

            writer.Write(SecondaryTargetIDs.Count);
            foreach (var targetID in SecondaryTargetIDs)
            {
                writer.Write(targetID);
            }
        }
    }



    public sealed class ObjectEffect : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectEffect; } }

        public uint ObjectID;
        public SpellEffect Effect;
        public uint EffectType;
        public uint DelayTime = 0;
        public uint Time = 0;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Effect = (SpellEffect)reader.ReadByte();
            EffectType = reader.ReadUInt32();
            DelayTime = reader.ReadUInt32();
            Time = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write((byte)Effect);
            writer.Write(EffectType);
            writer.Write(DelayTime);
            writer.Write(Time);
        }
    }

    public sealed class ObjectProjectile : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectProjectile; } }

        public Spell Spell;
        public uint Source;
        public uint Destination;

        protected override void ReadPacket(BinaryReader reader)
        {
            Spell = (Spell)reader.ReadByte();
            Source = reader.ReadUInt32();
            Destination = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Spell);
            writer.Write(Source);
            writer.Write(Destination);
        }
    }

    public sealed class RangeAttack : Packet //ArcherTest
    {
        public override short Index { get { return (short)ServerPacketIds.RangeAttack; } }

        public uint TargetID;
        public Point Target;
        public Spell Spell;

        protected override void ReadPacket(BinaryReader reader)
        {
            TargetID = reader.ReadUInt32();
            Target = new Point(reader.ReadInt32(), reader.ReadInt32());
            Spell = (Spell)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(TargetID);
            writer.Write(Target.X);
            writer.Write(Target.Y);
            writer.Write((byte)Spell);
        }
    }

    public sealed class Pushed : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.Pushed; }
        }

        public Point Location;
        public MirDirection Direction;


        protected override void ReadPacket(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class ObjectPushed : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectPushed; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class ObjectName : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectName; } }

        public uint ObjectID;
        public string Name = string.Empty;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Name);
        }
    }
    public sealed class UserStorage : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.UserStorage; } }

        public UserItem[] Storage;

        protected override void ReadPacket(BinaryReader reader)
        {
            if (!reader.ReadBoolean()) return;

            Storage = new UserItem[reader.ReadInt32()];
            for (int i = 0; i < Storage.Length; i++)
            {
                if (!reader.ReadBoolean()) continue;
                Storage[i] = new UserItem(reader);
            }
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Storage != null);
            if (Storage == null) return;

            writer.Write(Storage.Length);
            for (int i = 0; i < Storage.Length; i++)
            {
                writer.Write(Storage[i] != null);
                if (Storage[i] == null) continue;

                Storage[i].Save(writer);
            }
        }
    }
    public sealed class SwitchGroup : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SwitchGroup; } }

        public bool AllowGroup;
        protected override void ReadPacket(BinaryReader reader)
        {
            AllowGroup = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AllowGroup);
        }
    }
    public sealed class DeleteGroup : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.DeleteGroup; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class DeleteMember : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.DeleteMember; } }

        public string Name = string.Empty;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class GroupInvite : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.GroupInvite; } }

        public string Name = string.Empty;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class AddMember : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.AddMember; } }

        public string Name = string.Empty;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class GroupMembersMap : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.GroupMembersMap; } }

        public string PlayerName = string.Empty;
        public string PlayerMap = string.Empty;

        protected override void ReadPacket(BinaryReader reader)
        {
            PlayerName = reader.ReadString();
            PlayerMap = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(PlayerName);
            writer.Write(PlayerMap);
        }
    }
    public sealed class SendMemberLocation : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SendMemberLocation; } }

        public string MemberName;
        public Point MemberLocation;

        protected override void ReadPacket(BinaryReader reader)
        {
            MemberName = reader.ReadString();
            MemberLocation = new Point(reader.ReadInt32(), reader.ReadInt32());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MemberName);
            writer.Write(MemberLocation.X);
            writer.Write(MemberLocation.Y);
        }
    }
    public sealed class Revived : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.Revived; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class ObjectRevived : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectRevived; } }
        public uint ObjectID;
        public bool Effect;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Effect = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Effect);
        }
    }
    public sealed class SpellToggle : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SpellToggle; } }
        public uint ObjectID;
        public Spell Spell;
        public bool CanUse;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Spell = (Spell)reader.ReadByte();
            CanUse = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write((byte)Spell);
            writer.Write(CanUse);
        }
    }
    public sealed class ObjectHealth : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectHealth; } }
        public uint ObjectID;
        public byte Percent, Expire;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Percent = reader.ReadByte();
            Expire = reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Percent);
            writer.Write(Expire);
        }
    }

    public sealed class ObjectMana : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectMana; } }
        public uint ObjectID;
        public byte Percent;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Percent = reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Percent);
        }
    }
    public sealed class MapEffect : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.MapEffect; } }

        public Point Location;
        public SpellEffect Effect;
        public byte Value;

        protected override void ReadPacket(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Effect = (SpellEffect)reader.ReadByte();
            Value = reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Effect);
            writer.Write(Value);
        }
    }
    public sealed class AllowObserve : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.AllowObserve; }
        }

        public bool Allow;

        protected override void ReadPacket(BinaryReader reader)
        {
            Allow = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Allow);
        }
    }
    public sealed class ObjectRangeAttack : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectRangeAttack; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;
        public uint TargetID;
        public Point Target;
        public byte Type;
        public Spell Spell;
        public byte Level;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            TargetID = reader.ReadUInt32();
            Target = new Point(reader.ReadInt32(), reader.ReadInt32());
            Type = reader.ReadByte();
            Spell = (Spell)reader.ReadByte();
            Level = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
            writer.Write(TargetID);
            writer.Write(Target.X);
            writer.Write(Target.Y);
            writer.Write(Type);
            writer.Write((byte)Spell);
            writer.Write(Level);
        }
    }
    public sealed class AddBuff : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.AddBuff; } }

        public ClientBuff Buff;

        protected override void ReadPacket(BinaryReader reader)
        {
            Buff = new ClientBuff(reader);
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            Buff.Save(writer);
        }
    }
    public sealed class RemoveBuff : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.RemoveBuff; } }

        public BuffType Type;
        public uint ObjectID;

        protected override void ReadPacket(BinaryReader reader)
        {
            Type = (BuffType)reader.ReadByte();
            ObjectID = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write(ObjectID);
        }
    }
    public sealed class PauseBuff : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.PauseBuff; } }

        public BuffType Type;
        public uint ObjectID;
        public bool Paused;

        protected override void ReadPacket(BinaryReader reader)
        {
            Type = (BuffType)reader.ReadByte();
            ObjectID = reader.ReadUInt32();
            Paused = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write(ObjectID);
            writer.Write(Paused);
        }
    }

    public sealed class ObjectHidden : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectHidden; } }
        public uint ObjectID;
        public bool Hidden;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Hidden = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Hidden);
        }
    }
    public sealed class RefreshItem : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.RefreshItem; } }
        public UserItem Item;
        protected override void ReadPacket(BinaryReader reader)
        {
            Item = new UserItem(reader);
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            Item.Save(writer);
        }
    }
    public sealed class ObjectSpell : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectSpell; }
        }

        public uint ObjectID;
        public Point Location;
        public Spell Spell;
        public MirDirection Direction;
        public bool Param;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Spell = (Spell)reader.ReadByte();
            Direction = (MirDirection)reader.ReadByte();
            Param = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Spell);
            writer.Write((byte)Direction);
            writer.Write(Param);
        }
    }
    public sealed class UserDash : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UserDash; }
        }

        public Point Location;
        public MirDirection Direction;


        protected override void ReadPacket(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class ObjectDash : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectDash; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;


        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class UserDashFail : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UserDashFail; }
        }

        public Point Location;
        public MirDirection Direction;


        protected override void ReadPacket(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class ObjectDashFail : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectDashFail; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;


        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }
    public sealed class RemoveDelayedExplosion : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.RemoveDelayedExplosion; } }

        public uint ObjectID;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
        }
    }

    public sealed class NPCConsign : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCConsign; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class NPCMarket : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCMarket; } }

        public List<ClientAuction> Listings = new List<ClientAuction>();
        public int Pages;
        public bool UserMode;

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                Listings.Add(new ClientAuction(reader));

            Pages = reader.ReadInt32();
            UserMode = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Listings.Count);

            for (int i = 0; i < Listings.Count; i++)
                Listings[i].Save(writer);

            writer.Write(Pages);
            writer.Write(UserMode);
        }
    }
    public sealed class NPCMarketPage : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCMarketPage; } }

        public List<ClientAuction> Listings = new List<ClientAuction>();

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                Listings.Add(new ClientAuction(reader));
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Listings.Count);

            for (int i = 0; i < Listings.Count; i++)
                Listings[i].Save(writer);
        }
    }
    public sealed class ConsignItem : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ConsignItem; } }

        public ulong UniqueID;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Success = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Success);
        }
    }
    public sealed class MarketFail : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.MarketFail; } }

        public byte Reason;

        /*
         * 0: Dead.
         * 1: Not talking to TrustMerchant.
         * 2: Already Sold.
         * 3: Expired.
         * 4: Not enough Gold.
         * 5: Not enough bag space.
         * 6: You cannot buy your own items.
         * 7: Trust Merchant is too far.
         * 8: Too much Gold.
         */

        protected override void ReadPacket(BinaryReader reader)
        {
            Reason = reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Reason);
        }
    }
    public sealed class MarketSuccess : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.MarketSuccess; } }

        public string Message = string.Empty;

        protected override void ReadPacket(BinaryReader reader)
        {
            Message = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Message);
        }
    }
    public sealed class ObjectSitDown : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectSitDown; } }
        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;
        public bool Sitting;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            Sitting = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
            writer.Write(Sitting);
        }
    }
    public sealed class InTrapRock : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.InTrapRock; } }
        public bool Trapped;

        protected override void ReadPacket(BinaryReader reader)
        {
            Trapped = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Trapped);
        }
    }
    public sealed class BaseStatsInfo : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.BaseStatsInfo; }
        }

        public BaseStats Stats;

        protected override void ReadPacket(BinaryReader reader)
        {
            Stats = new BaseStats(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Stats.Save(writer);
        }
    }

    public sealed class HeroBaseStatsInfo : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.HeroBaseStatsInfo; }
        }

        public BaseStats Stats;

        protected override void ReadPacket(BinaryReader reader)
        {
            Stats = new BaseStats(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Stats.Save(writer);
        }
    }

    public sealed class UserName : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.UserName; } }
        public uint Id;
        public string Name;
        protected override void ReadPacket(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Name);
        }
    }
    public sealed class ChatItemStats : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ChatItemStats; } }
        public ulong ChatItemId;
        public UserItem Stats;
        protected override void ReadPacket(BinaryReader reader)
        {
            ChatItemId = reader.ReadUInt64();
            Stats = new UserItem(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ChatItemId);
            if (Stats != null) Stats.Save(writer);
        }
    }

    public sealed class GuildNoticeChange : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GuildNoticeChange; }
        }
        public int update = 0;
        public List<string> notice = new List<string>();
        protected override void ReadPacket(BinaryReader reader)
        {
            update = reader.ReadInt32();
            for (int i = 0; i < update; i++)
                notice.Add(reader.ReadString());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            if (update < 0)
            {
                writer.Write(update);
                return;
            }
            writer.Write(notice.Count);
            for (int i = 0; i < notice.Count; i++)
                writer.Write(notice[i]);
        }
    }

    public sealed class GuildMemberChange : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GuildMemberChange; }
        }
        public string Name = string.Empty;
        public byte Status = 0;
        public byte RankIndex = 0;
        public List<GuildRank> Ranks = new List<GuildRank>();
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
            RankIndex = reader.ReadByte();
            Status = reader.ReadByte();
            if (Status > 5)
            {
                int rankcount = reader.ReadInt32();
                for (int i = 0; i < rankcount; i++)
                    Ranks.Add(new GuildRank(reader));
            }
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(RankIndex);
            writer.Write(Status);
            if (Status > 5)
            {
                writer.Write(Ranks.Count);
                for (int i = 0; i < Ranks.Count; i++)
                    Ranks[i].Save(writer);
            }
        }
    }

    public sealed class GuildStatus : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GuildStatus; }
        }
        public string GuildName = string.Empty;
        public string GuildRankName = string.Empty;
        public byte Level;
        public long Experience;
        public long MaxExperience;
        public uint Gold;
        public byte SparePoints;
        public int MemberCount;
        public int MaxMembers;
        public bool Voting;
        public byte ItemCount;
        public byte BuffCount;
        public GuildRankOptions MyOptions;
        public int MyRankId;

        protected override void ReadPacket(BinaryReader reader)
        {
            GuildName = reader.ReadString();
            GuildRankName = reader.ReadString();
            Level = reader.ReadByte();
            Experience = reader.ReadInt64();
            MaxExperience = reader.ReadInt64();
            Gold = reader.ReadUInt32();
            SparePoints = reader.ReadByte();
            MemberCount = reader.ReadInt32();
            MaxMembers = reader.ReadInt32();
            Voting = reader.ReadBoolean();
            ItemCount = reader.ReadByte();
            BuffCount = reader.ReadByte();
            MyOptions = (GuildRankOptions)reader.ReadByte();
            MyRankId = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(GuildName);
            writer.Write(GuildRankName);
            writer.Write(Level);
            writer.Write(Experience);
            writer.Write(MaxExperience);
            writer.Write(Gold);
            writer.Write(SparePoints);
            writer.Write(MemberCount);
            writer.Write(MaxMembers);
            writer.Write(Voting);
            writer.Write(ItemCount);
            writer.Write(BuffCount);
            writer.Write((byte)MyOptions);
            writer.Write(MyRankId);
        }
    }
    public sealed class GuildInvite : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.GuildInvite; } }

        public string Name = string.Empty;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class GuildExpGain : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.GuildExpGain; } }

        public uint Amount = 0;
        protected override void ReadPacket(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Amount);
        }
    }
    public sealed class GuildNameRequest : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.GuildNameRequest; } }
        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class GuildStorageGoldChange : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.GuildStorageGoldChange; } }
        public uint Amount = 0;
        public byte Type = 0;
        public string Name = string.Empty;

        protected override void ReadPacket(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
            Type = reader.ReadByte();
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Amount);
            writer.Write(Type);
            writer.Write(Name);
        }
    }
    public sealed class GuildStorageItemChange : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.GuildStorageItemChange; } }
        public int User = 0;
        public byte Type = 0;
        public int To = 0;
        public int From = 0;
        public GuildStorageItem Item = null;
        protected override void ReadPacket(BinaryReader reader)
        {
            Type = reader.ReadByte();
            To = reader.ReadInt32();
            From = reader.ReadInt32();
            User = reader.ReadInt32();
            if (!reader.ReadBoolean()) return;
            Item = new GuildStorageItem
            {
                UserId = reader.ReadInt64(),
                Item = new UserItem(reader)
            };
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Type);
            writer.Write(To);
            writer.Write(From);
            writer.Write(User);
            writer.Write(Item != null);
            if (Item == null) return;
            writer.Write(Item.UserId);
            Item.Item.Save(writer);
        }
    }
    public sealed class GuildStorageList : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GuildStorageList; }
        }
        public GuildStorageItem[] Items;
        protected override void ReadPacket(BinaryReader reader)
        {
            Items = new GuildStorageItem[reader.ReadInt32()];
            for (int i = 0; i < Items.Length; i++)
            {
                if (reader.ReadBoolean() == true)
                    Items[i] = new GuildStorageItem(reader);
            }
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Items.Length);
            for (int i = 0; i < Items.Length; i++)
            {
                writer.Write(Items[i] != null);
                if (Items[i] != null)
                    Items[i].Save(writer);
            }
        }

    }
    public sealed class GuildRequestWar : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.GuildRequestWar; } }
        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class HeroCreateRequest : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.HeroCreateRequest; } }

        public bool[] CanCreateClass;
        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            CanCreateClass = new bool[count];
            for (int i = 0; i < count; i++)
                CanCreateClass[i] = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CanCreateClass.Length);
            for (int i = 0; i < CanCreateClass.Length; i++)
                writer.Write(CanCreateClass[i]);
        }
    }

    public sealed class NewHero : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewHero; }
        }

        /*
         * 0: Disabled.
         * 1: Bad Character Name
         * 2: Bad Gender
         * 3: Bad Class
         * 4: Max Heroes
         * 5: Name Exists.
         * */
        public byte Result;

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
        }
    }

    public sealed class HeroInformation : UserInformation
    {
        public bool AutoPot;
        public byte AutoHPPercent;
        public byte AutoMPPercent;
        public int HPItemIndex;
        public int MPItemIndex;
        public override short Index
        {
            get { return (short)ServerPacketIds.HeroInformation; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Name = reader.ReadString();
            Class = (MirClass)reader.ReadByte();
            Gender = (MirGender)reader.ReadByte();
            Level = reader.ReadUInt16();
            Hair = reader.ReadByte();

            HP = reader.ReadInt32();
            MP = reader.ReadInt32();

            Experience = reader.ReadInt64();
            MaxExperience = reader.ReadInt64();

            if (reader.ReadBoolean())
            {
                Inventory = new UserItem[reader.ReadInt32()];
                for (int i = 0; i < Inventory.Length; i++)
                {
                    if (!reader.ReadBoolean()) continue;
                    Inventory[i] = new UserItem(reader);
                }
            }

            if (reader.ReadBoolean())
            {
                Equipment = new UserItem[reader.ReadInt32()];
                for (int i = 0; i < Equipment.Length; i++)
                {
                    if (!reader.ReadBoolean()) continue;
                    Equipment[i] = new UserItem(reader);
                }
            }

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                Magics.Add(new ClientMagic(reader));
            }

            AutoPot = reader.ReadBoolean();
            AutoHPPercent = reader.ReadByte();
            AutoMPPercent = reader.ReadByte();
            HPItemIndex = reader.ReadInt32();
            MPItemIndex = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Name);
            writer.Write((byte)Class);
            writer.Write((byte)Gender);
            writer.Write(Level);
            writer.Write(Hair);

            writer.Write(HP);
            writer.Write(MP);

            writer.Write(Experience);
            writer.Write(MaxExperience);

            writer.Write(Inventory != null);
            if (Inventory != null)
            {
                writer.Write(Inventory.Length);
                for (int i = 0; i < Inventory.Length; i++)
                {
                    writer.Write(Inventory[i] != null);
                    if (Inventory[i] == null) continue;

                    Inventory[i].Save(writer);
                }

            }

            writer.Write(Equipment != null);
            if (Equipment != null)
            {
                writer.Write(Equipment.Length);
                for (int i = 0; i < Equipment.Length; i++)
                {
                    writer.Write(Equipment[i] != null);
                    if (Equipment[i] == null) continue;

                    Equipment[i].Save(writer);
                }
            }

            writer.Write(Magics.Count);
            for (int i = 0; i < Magics.Count; i++)
            {
                Magics[i].Save(writer);
            }

            writer.Write(AutoPot);
            writer.Write(AutoHPPercent);
            writer.Write(AutoMPPercent);
            writer.Write(HPItemIndex);
            writer.Write(MPItemIndex);
        }
    }

    public sealed class UpdateHeroSpawnState : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UpdateHeroSpawnState; }
        }

        public HeroSpawnState State;

        protected override void ReadPacket(BinaryReader reader)
        {
            State = (HeroSpawnState)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)State);
        }
    }
    public sealed class UnlockHeroAutoPot : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.UnlockHeroAutoPot; } }
        protected override void ReadPacket(BinaryReader reader) { }
        protected override void WritePacket(BinaryWriter writer) { }
    }

    public sealed class SetAutoPotValue : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SetAutoPotValue; } }

        public Stat Stat;
        public uint Value;
        protected override void ReadPacket(BinaryReader reader) 
        {
            Stat = (Stat)reader.ReadByte();
            Value = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer) 
        {
            writer.Write((byte)Stat);
            writer.Write(Value);
        }
    }

    public sealed class SetAutoPotItem : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SetAutoPotItem; } }

        public MirGridType Grid;
        public int ItemIndex;
        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            ItemIndex = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(ItemIndex);
        }
    }

    public sealed class SetHeroBehaviour : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SetHeroBehaviour; } }

        public HeroBehaviour Behaviour;
        protected override void ReadPacket(BinaryReader reader)
        {
            Behaviour = (HeroBehaviour)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Behaviour);
        }
    }

    public sealed class ManageHeroes : Packet
    {
        public int MaximumCount;
        public ClientHeroInformation CurrentHero;
        public ClientHeroInformation[] Heroes;
        public override short Index { get { return (short)ServerPacketIds.ManageHeroes; } }
        protected override void ReadPacket(BinaryReader reader)
        {
            MaximumCount = reader.ReadInt32();

            if (reader.ReadBoolean())
                CurrentHero = new ClientHeroInformation(reader);

            if (reader.ReadBoolean())
            {
                int count = reader.ReadInt32();
                Heroes = new ClientHeroInformation[count];
                for (int i = 0; i < count; i++)
                {
                    if (reader.ReadBoolean())
                        Heroes[i] = new ClientHeroInformation(reader);
                }
            }
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MaximumCount);

            writer.Write(CurrentHero != null);
            if (CurrentHero != null)
                CurrentHero.Save(writer);

            writer.Write(Heroes != null);
            if (Heroes != null)
            {
                writer.Write(Heroes.Length);
                for (int i = 0; i < Heroes.Length; i++)
                {
                    writer.Write(Heroes[i] != null);
                    if (Heroes[i] != null)
                        Heroes[i].Save(writer);
                }
            }
        }
    }

    public sealed class ChangeHero : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ChangeHero; } }

        public int FromIndex;
        protected override void ReadPacket(BinaryReader reader)
        {
            FromIndex = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(FromIndex);
        }
    }

    public sealed class DefaultNPC : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.DefaultNPC; } }

        public uint ObjectID;
        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
        }
    }

    public sealed class NPCUpdate : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCUpdate; } }

        public uint NPCID;

        protected override void ReadPacket(BinaryReader reader)
        {
            NPCID = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(NPCID);
        }
    }


    public sealed class NPCImageUpdate : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCImageUpdate; } }

        public long ObjectID;
        public ushort Image;
        public Color Colour;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadInt64();
            Image = reader.ReadUInt16();
            Colour = Color.FromArgb(reader.ReadInt32());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Image);
            writer.Write(Colour.ToArgb());
        }
    }
    public sealed class MountUpdate : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.MountUpdate; } }

        public long ObjectID;
        public short MountType;
        public bool RidingMount;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadInt64();
            MountType = reader.ReadInt16();
            RidingMount = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(MountType);
            writer.Write(RidingMount);
        }
    }

    public sealed class TransformUpdate : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.TransformUpdate; } }

        public long ObjectID;
        public short TransformType;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadInt64();
            TransformType = reader.ReadInt16();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(TransformType);
        }
    }

    public sealed class EquipSlotItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.EquipSlotItem; }
        }

        public MirGridType Grid;
        public ulong UniqueID;
        public int To;
        public bool Success;
        public MirGridType GridTo;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            To = reader.ReadInt32();
            GridTo = (MirGridType)reader.ReadByte();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(UniqueID);
            writer.Write(To);
            writer.Write((byte)GridTo);
            writer.Write(Success);
        }
    }

    public sealed class FishingUpdate : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.FishingUpdate; } }

        public long ObjectID;
        public bool Fishing;
        public int ProgressPercent;
        public int ChancePercent;
        public Point FishingPoint;
        public bool FoundFish;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadInt64();
            Fishing = reader.ReadBoolean();
            ProgressPercent = reader.ReadInt32();
            ChancePercent = reader.ReadInt32();
            FishingPoint = new Point(reader.ReadInt32(), reader.ReadInt32());
            FoundFish = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Fishing);
            writer.Write(ProgressPercent);
            writer.Write(ChancePercent);
            writer.Write(FishingPoint.X);
            writer.Write(FishingPoint.Y);
            writer.Write(FoundFish);
        }
    }

    //public sealed class UpdateQuests : Packet
    //{
    //    public override short Index
    //    {
    //        get { return (short)ServerPacketIds.UpdateQuests; }
    //    }

    //    public List<ClientQuestProgress> CurrentQuests = new List<ClientQuestProgress>();
    //    public List<int> CompletedQuests = new List<int>();

    //    protected override void ReadPacket(BinaryReader reader)
    //    {
    //        int count = reader.ReadInt32();
    //        for (var i = 0; i < count; i++)
    //            CurrentQuests.Add(new ClientQuestProgress(reader));

    //        count = reader.ReadInt32();
    //        for (var i = 0; i < count; i++)
    //            CompletedQuests.Add(reader.ReadInt32());
    //    }
    //    protected override void WritePacket(BinaryWriter writer)
    //    {
    //        writer.Write(CurrentQuests.Count);
    //        foreach (var q in CurrentQuests)
    //            q.Save(writer);

    //        writer.Write(CompletedQuests.Count);
    //        foreach (int q in CompletedQuests)
    //            writer.Write(q);
    //    }
    //}


    public sealed class ChangeQuest : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ChangeQuest; }
        }

        public ClientQuestProgress Quest = new ClientQuestProgress();
        public QuestState QuestState;
        public bool TrackQuest;

        protected override void ReadPacket(BinaryReader reader)
        {
            Quest = new ClientQuestProgress(reader);
            QuestState = (QuestState)reader.ReadByte();
            TrackQuest = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            Quest.Save(writer);
            writer.Write((byte)QuestState);
            writer.Write(TrackQuest);
        }
    }

    public sealed class CompleteQuest : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.CompleteQuest; }
        }

        public List<int> CompletedQuests = new List<int>();

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
                CompletedQuests.Add(reader.ReadInt32());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CompletedQuests.Count);
            foreach (int q in CompletedQuests)
                writer.Write(q);
        }
    }

    public sealed class ShareQuest : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ShareQuest; }
        }

        public int QuestIndex;
        public string SharerName;

        protected override void ReadPacket(BinaryReader reader)
        {
            QuestIndex = reader.ReadInt32();
            SharerName = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(QuestIndex);
            writer.Write(SharerName);
        }
    }


    public sealed class NewQuestInfo : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewQuestInfo; }
        }

        public ClientQuestInfo Info;

        protected override void ReadPacket(BinaryReader reader)
        {
            Info = new ClientQuestInfo(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Info.Save(writer);
        }
    }

    public sealed class GainedQuestItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GainedQuestItem; }
        }

        public UserItem Item;

        protected override void ReadPacket(BinaryReader reader)
        {
            Item = new UserItem(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Item.Save(writer);
        }
    }

    public sealed class DeleteQuestItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.DeleteQuestItem; }
        }

        public ulong UniqueID;
        public ushort Count;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Count);
        }
    }

    public sealed class GameShopInfo : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GameShopInfo; }
        }

        public GameShopItem Item;
        public int StockLevel;

        protected override void ReadPacket(BinaryReader reader)
        {
            Item = new GameShopItem(reader, true);
            StockLevel = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Item.Save(writer, true);
            writer.Write(StockLevel);
        }
    }

    public sealed class GameShopStock : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GameShopStock; }
        }

        public int GIndex;
        public int StockLevel;

        protected override void ReadPacket(BinaryReader reader)
        {
            GIndex = reader.ReadInt32();
            StockLevel = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(GIndex);
            writer.Write(StockLevel);
        }
    }

    public sealed class CancelReincarnation : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.CancelReincarnation; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class RequestReincarnation : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.RequestReincarnation; } }


        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }

    }

    public sealed class UserBackStep : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UserBackStep; }
        }

        public Point Location;
        public MirDirection Direction;


        protected override void ReadPacket(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }

    public sealed class ObjectBackStep : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectBackStep; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;
        public int Distance;


        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            Distance = reader.ReadInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
            writer.Write(Distance);
        }
    }

    public sealed class UserDashAttack : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UserDashAttack; }
        }

        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }

    public sealed class ObjectDashAttack : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectDashAttack; }
        }

        public uint ObjectID;
        public Point Location;
        public MirDirection Direction;
        public int Distance;


        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            Distance = reader.ReadInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
            writer.Write(Distance);
        }
    }

    public sealed class UserAttackMove : Packet//warrior skill - SlashingBurst move packet 
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UserAttackMove; }
        }


        public Point Location;
        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
        }


        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write((byte)Direction);
        }
    }

    public sealed class CombineItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.CombineItem; }
        }

        public MirGridType Grid;
        public ulong IDFrom, IDTo;
        public bool Success;
        public bool Destroy;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            IDFrom = reader.ReadUInt64();
            IDTo = reader.ReadUInt64();
            Success = reader.ReadBoolean();
            Destroy = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(IDFrom);
            writer.Write(IDTo);
            writer.Write(Success);
            writer.Write(Destroy);
        }
    }

    public sealed class ItemUpgraded : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ItemUpgraded; }
        }

        public UserItem Item;

        protected override void ReadPacket(BinaryReader reader)
        {
            Item = new UserItem(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Item.Save(writer);
        }
    }

    public sealed class SetConcentration : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SetConcentration; } }

        public uint ObjectID;
        public bool Enabled;
        public bool Interrupted;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Enabled = reader.ReadBoolean();
            Interrupted = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Enabled);
            writer.Write(Interrupted);
        }
    }
    
    public sealed class SetElemental : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SetElemental; } }

        public uint ObjectID;
        public bool Enabled;
        public bool Casted;
        public uint Value;
        public uint ElementType;
        public uint ExpLast;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Enabled = reader.ReadBoolean();
            Casted = reader.ReadBoolean();
            Value = reader.ReadUInt32();
            ElementType = reader.ReadUInt32();
            ExpLast = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Enabled);
            writer.Write(Casted);
            writer.Write(Value);
            writer.Write(ElementType);
            writer.Write(ExpLast);
        }
    }

    public sealed class ObjectDeco : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ObjectDeco; }
        }

        public uint ObjectID;
        public Point Location;
        public int Image;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Image = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Image);
        }
    }
    public sealed class ObjectSneaking : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectSneaking; } }
        public uint ObjectID;
        public bool SneakingActive;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            SneakingActive = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(SneakingActive);
        }
    }

    public sealed class ObjectLevelEffects : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ObjectLevelEffects; } }

        public uint ObjectID;
        public LevelEffects LevelEffects;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            LevelEffects = (LevelEffects)reader.ReadUInt16();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write((ushort)LevelEffects);
        }
    }

    public sealed class SetBindingShot : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SetBindingShot; } }

        public uint ObjectID;
        public bool Enabled;
        public long Value;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Enabled = reader.ReadBoolean();
            Value = reader.ReadInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Enabled);
            writer.Write(Value);
        }
    }

    public sealed class SendOutputMessage : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SendOutputMessage; } }

        public string Message;
        public OutputMessageType Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            Message = reader.ReadString();
            Type = (OutputMessageType)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Message);
            writer.Write((byte)Type);
        }
    }
    public sealed class NPCAwakening : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCAwakening; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class NPCDisassemble : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCDisassemble; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class NPCDowngrade : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCDowngrade; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class NPCReset : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCReset; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class AwakeningNeedMaterials : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.AwakeningNeedMaterials; } }

        public ItemInfo[] Materials;
        public byte[] MaterialsCount;

        protected override void ReadPacket(BinaryReader reader)
        {
            if (!reader.ReadBoolean()) return;

            int count = reader.ReadInt32();
            Materials = new ItemInfo[count];
            MaterialsCount = new byte[count];

            for (int i = 0; i < Materials.Length; i++)
            {
                if (!reader.ReadBoolean()) continue;
                Materials[i] = new ItemInfo(reader);
                MaterialsCount[i] = reader.ReadByte();
            }
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Materials != null);
            if (Materials == null) return;

            writer.Write(Materials.Length);
            for (int i = 0; i < Materials.Length; i++)
            {
                writer.Write(Materials[i] != null);
                if (Materials[i] == null) continue;

                Materials[i].Save(writer);
                writer.Write(MaterialsCount[i]);
            }
        }
    }

    public sealed class AwakeningLockedItem : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.AwakeningLockedItem; } }

        public ulong UniqueID;
        public bool Locked;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Locked = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Locked);
        }
    }

    public sealed class Awakening : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.Awakening; } }

        public int result;
        public long removeID = -1;

        protected override void ReadPacket(BinaryReader reader)
        {
            result = reader.ReadInt32();
            removeID = reader.ReadInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(result);
            writer.Write(removeID);
        }
    }

    public sealed class ReceiveMail : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ReceiveMail; }
        }

        public List<ClientMail> Mail = new List<ClientMail>();

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                Mail.Add(new ClientMail(reader));
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Mail.Count);

            for (int i = 0; i < Mail.Count; i++)
                Mail[i].Save(writer);
        }
    }
    public sealed class MailLockedItem : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.MailLockedItem; } }

        public ulong UniqueID;
        public bool Locked;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Locked = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Locked);
        }
    }

    public sealed class MailSent : Packet
    {
        public sbyte Result;

        public override short Index
        {
            get { return (short)ServerPacketIds.MailSent; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadSByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
        }
    }

    public sealed class MailSendRequest : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.MailSendRequest; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class ParcelCollected : Packet
    {
        public sbyte Result;

        public override short Index
        {
            get { return (short)ServerPacketIds.ParcelCollected; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            Result = reader.ReadSByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Result);
        }
    }

    public sealed class MailCost : Packet
    {
        public uint Cost;

        public override short Index
        {
            get { return (short)ServerPacketIds.MailCost; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            Cost = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Cost);
        }
    }

    public sealed class ResizeInventory : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ResizeInventory; } }

        public int Size;

        protected override void ReadPacket(BinaryReader reader)
        {
            Size = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Size);
        }
    }

    public sealed class ResizeStorage : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ResizeStorage; } }

        public int Size;
        public bool HasExpandedStorage;
        public DateTime ExpiryTime;

        protected override void ReadPacket(BinaryReader reader)
        {
            Size = reader.ReadInt32();
            HasExpandedStorage = reader.ReadBoolean();
            ExpiryTime = DateTime.FromBinary(reader.ReadInt64());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Size);
            writer.Write(HasExpandedStorage);
            writer.Write(ExpiryTime.ToBinary());
        }
    }

    public sealed class NewIntelligentCreature : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewIntelligentCreature; }
        }

        public ClientIntelligentCreature Creature;
        protected override void ReadPacket(BinaryReader reader)
        {
            Creature = new ClientIntelligentCreature(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Creature.Save(writer);
        }
    }
    public sealed class UpdateIntelligentCreatureList : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UpdateIntelligentCreatureList; }
        }

        public List<ClientIntelligentCreature> CreatureList = new List<ClientIntelligentCreature>();
        public bool CreatureSummoned = false;
        public IntelligentCreatureType SummonedCreatureType = IntelligentCreatureType.None;
        public int PearlCount = 0;

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                CreatureList.Add(new ClientIntelligentCreature(reader));
            CreatureSummoned = reader.ReadBoolean();
            SummonedCreatureType = (IntelligentCreatureType)reader.ReadByte();
            PearlCount = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CreatureList.Count);
            for (int i = 0; i < CreatureList.Count; i++)
                CreatureList[i].Save(writer);
            writer.Write(CreatureSummoned);
            writer.Write((byte)SummonedCreatureType);
            writer.Write(PearlCount);
        }
    }

    public sealed class IntelligentCreatureEnableRename : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.IntelligentCreatureEnableRename; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class IntelligentCreaturePickup : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.IntelligentCreaturePickup; }
        }

        public uint ObjectID;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
        }
    }

    public sealed class NPCPearlGoods : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCPearlGoods; } }

        public List<UserItem> List = new List<UserItem>();
        public float Rate;
        public PanelType Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                List.Add(new UserItem(reader));

            Rate = reader.ReadSingle();

            Type = (PanelType)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(List.Count);

            for (int i = 0; i < List.Count; i++)
                List[i].Save(writer);

            writer.Write(Rate);
            writer.Write((byte)Type);
        }
    }

    public sealed class FriendUpdate : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.FriendUpdate; }
        }

        public List<ClientFriend> Friends = new List<ClientFriend>();

        protected override void ReadPacket(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
                Friends.Add(new ClientFriend(reader));
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Friends.Count);

            for (int i = 0; i < Friends.Count; i++)
                Friends[i].Save(writer);
        }
    }

    public sealed class GuildBuffList : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.GuildBuffList; } }

        public byte Remove = 0;
        public List<GuildBuff> ActiveBuffs = new List<GuildBuff>();
        public List<GuildBuffInfo> GuildBuffs = new List<GuildBuffInfo>();

        protected override void ReadPacket(BinaryReader reader)
        {
            Remove = reader.ReadByte();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                ActiveBuffs.Add(new GuildBuff(reader));
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                GuildBuffs.Add(new GuildBuffInfo(reader));
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Remove);
            writer.Write(ActiveBuffs.Count);
            for (int i = 0; i < ActiveBuffs.Count; i++)
                ActiveBuffs[i].Save(writer);
            writer.Write(GuildBuffs.Count);
            for (int i = 0; i < GuildBuffs.Count; i++)
                GuildBuffs[i].Save(writer);
        }
    }
    public sealed class LoverUpdate : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.LoverUpdate; }
        }

        public string Name;
        public DateTime Date;
        public string MapName;
        public short MarriedDays;

        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
            Date = DateTime.FromBinary(reader.ReadInt64());
            MapName = reader.ReadString();
            MarriedDays = reader.ReadInt16();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Date.ToBinary());
            writer.Write(MapName);
            writer.Write(MarriedDays);
        }
    }

    public sealed class MentorUpdate : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.MentorUpdate; }
        }

        public string Name;
        public ushort Level;
        public bool Online;
        public long MenteeEXP;

        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
            Level = reader.ReadUInt16();
            Online = reader.ReadBoolean();
            MenteeEXP = reader.ReadInt64();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Level);
            writer.Write(Online);
            writer.Write(MenteeEXP);
        }
    }

    public sealed class NPCRequestInput : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.NPCRequestInput; } }

        public uint NPCID;
        public string PageName;

        protected override void ReadPacket(BinaryReader reader)
        {
            NPCID = reader.ReadUInt32();
            PageName = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(NPCID);
            writer.Write(PageName);
        }
    }

    public sealed class Rankings : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.Rankings; } }
        public override bool Observable
        {
            get { return false; }
        }

        public byte RankType = 0;
        public int MyRank = 0;
        public List<RankCharacterInfo> ListingDetails = new List<RankCharacterInfo>();
        public List<long> Listings = new List<long>();
        public int Count;

        protected override void ReadPacket(BinaryReader reader)
        {
            RankType = reader.ReadByte();
            MyRank = reader.ReadInt32();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                ListingDetails.Add(new RankCharacterInfo(reader));
            }
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Listings.Add(reader.ReadInt64());
            }
            Count = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(RankType);
            writer.Write(MyRank);
            writer.Write(ListingDetails.Count);
            for (int i = 0; i < ListingDetails.Count; i++)
                ListingDetails[i].Save(writer);
            writer.Write(Listings.Count);
            for (int i = 0; i < Listings.Count; i++)
                writer.Write(Listings[i]);
            writer.Write(Count);
        }
    }

    public sealed class Opendoor : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.Opendoor; } }

        public bool Close = false;
        public byte DoorIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            DoorIndex = reader.ReadByte();
            Close = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(DoorIndex);
            writer.Write(Close);
        }
    }

    public sealed class GetRentedItems : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.GetRentedItems; }
        }

        public List<ItemRentalInformation> RentedItems = new List<ItemRentalInformation>();

        protected override void ReadPacket(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (var i = 0; i < count; i++)
                RentedItems.Add(new ItemRentalInformation(reader));
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(RentedItems.Count);

            foreach (var rentedItemInformation in RentedItems)
                rentedItemInformation.Save(writer);
        }
    }

    public sealed class ItemRentalRequest : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ItemRentalRequest; } }

        public string Name;
        public bool Renting;

        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
            Renting = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Renting);
        }
    }

    public sealed class ItemRentalFee : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ItemRentalFee; }
        }

        public uint Amount;

        protected override void ReadPacket(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Amount);
        }
    }

    public sealed class ItemRentalPeriod : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ItemRentalPeriod; }
        }

        public uint Days;

        protected override void ReadPacket(BinaryReader reader)
        {
            Days = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Days);
        }
    }

    public sealed class DepositRentalItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.DepositRentalItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }

    public sealed class RetrieveRentalItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.RetrieveRentalItem; }
        }

        public int From, To;
        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
            Success = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            writer.Write(Success);
        }
    }

    public sealed class UpdateRentalItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UpdateRentalItem; }
        }

        public bool HasData;
        public UserItem LoanItem;

        protected override void ReadPacket(BinaryReader reader)
        {
            HasData = reader.ReadBoolean();

            if (HasData)
                LoanItem = new UserItem(reader); 
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(LoanItem != null);

            if (LoanItem != null)
                LoanItem.Save(writer);
        }
    }

    public sealed class CancelItemRental : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.CancelItemRental; }
        }

        protected override void ReadPacket(BinaryReader reader)
        { }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }

    public sealed class ItemRentalLock : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ItemRentalLock; }
        }

        public bool Success;
        public bool GoldLocked;
        public bool ItemLocked;

        protected override void ReadPacket(BinaryReader reader)
        {
            Success = reader.ReadBoolean();
            GoldLocked = reader.ReadBoolean();
            ItemLocked = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Success);
            writer.Write(GoldLocked);
            writer.Write(ItemLocked);
        }
    }

    public sealed class ItemRentalPartnerLock : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ItemRentalPartnerLock; }
        }

        public bool GoldLocked;
        public bool ItemLocked;

        protected override void ReadPacket(BinaryReader reader)
        {
            GoldLocked = reader.ReadBoolean();
            ItemLocked = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(GoldLocked);
            writer.Write(ItemLocked);
        }
    }

    public sealed class CanConfirmItemRental : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.CanConfirmItemRental; }
        }

        protected override void ReadPacket(BinaryReader reader)
        { }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }

    public sealed class ConfirmItemRental : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.ConfirmItemRental; }
        }

        protected override void ReadPacket(BinaryReader reader)
        { }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }


    public sealed class NewRecipeInfo : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.NewRecipeInfo; }
        }

        public ClientRecipeInfo Info;

        protected override void ReadPacket(BinaryReader reader)
        {
            Info = new ClientRecipeInfo(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Info.Save(writer);
        }
    }
    public sealed class CraftItem : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.CraftItem; } }

        public bool Success;

        protected override void ReadPacket(BinaryReader reader)
        {
            Success = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Success);
        }
    }


    public sealed class OpenBrowser : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.OpenBrowser; }
        }

        public string Url;

        protected override void ReadPacket(BinaryReader reader)
        {
            Url = reader.ReadString();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Url);
        }
    }
    public sealed class PlaySound : Packet
    {
        public int Sound;

        public override short Index
        {
            get { return (short)ServerPacketIds.PlaySound; }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            Sound = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Sound);
        }
    }

    public sealed class SetTimer : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SetTimer; } }

        public string Key;
        public byte Type;
        public int Seconds;

        protected override void ReadPacket(BinaryReader reader)
        {
            Key = reader.ReadString();
            Type = reader.ReadByte();
            Seconds = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Key);
            writer.Write(Type);
            writer.Write(Seconds);
        }
    }

    public sealed class ExpireTimer : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.ExpireTimer; } }

        public string Key;

        protected override void ReadPacket(BinaryReader reader)
        {
            Key = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Key);
        }
    }

    public sealed class UpdateNotice : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.UpdateNotice; }
        }

        public Notice Notice = new Notice();

        protected override void ReadPacket(BinaryReader reader)
        {
            Notice = new Notice(reader);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Notice.Save(writer);
        }
    }

    public sealed class Roll : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.Roll; } }

        public int Type;
        public string Page;
        public int Result;
        public bool AutoRoll;

        protected override void ReadPacket(BinaryReader reader)
        {
            Type = reader.ReadInt32();
            Page = reader.ReadString();
            Result = reader.ReadInt32();
            AutoRoll = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Type);
            writer.Write(Page);
            writer.Write(Result);
            writer.Write(AutoRoll);
        }
    }


    public sealed class SetCompass : Packet
    {
        public override short Index { get { return (short)ServerPacketIds.SetCompass; } }

        public Point Location;

        protected override void ReadPacket(BinaryReader reader)
        {
            var x = reader.ReadInt32();
            var y = reader.ReadInt32();

            Location = new Point(x, y);
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
        }
    }
}