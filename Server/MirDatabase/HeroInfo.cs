using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class HeroInfo : CharacterInfo
    {
        public bool AutoPot;
        public byte Grade;
        public int HPItemIndex;
        public int MPItemIndex;
        public byte AutoHPPercent;
        public byte AutoMPPercent;
        public ushort SealCount;
        public HeroInfo(ClientPackets.NewHero p)
        {
            Name = p.Name;
            Class = p.Class;
            Gender = p.Gender;

            HP = -1;

            Inventory = new UserItem[10];

            CreationDate = Envir.Now;
        }

        public HeroInfo(BinaryReader reader, int version, int customVersion) : base(reader, version, customVersion) { }

        public override void Load(BinaryReader reader, int version, int customVersion)
        {
            Index = reader.ReadInt32();
            Name = reader.ReadString();
            Level = reader.ReadUInt16(); 
            Class = (MirClass) reader.ReadByte();
            Gender = (MirGender) reader.ReadByte();
            Hair = reader.ReadByte();

            CreationDate = DateTime.FromBinary(reader.ReadInt64());

            Deleted = reader.ReadBoolean();
            DeleteDate = DateTime.FromBinary(reader.ReadInt64());

            HP = reader.ReadInt32();
            MP = reader.ReadInt32();            

            Experience = reader.ReadInt64();

            int count = reader.ReadInt32();

            Array.Resize(ref Inventory, count);

            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, version, customVersion);
                if (Envir.BindItem(item) && i < Inventory.Length)
                {
                    Inventory[i] = item;
                }
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, version, customVersion);
                if (Envir.BindItem(item) && i < Equipment.Length)
                {
                    Equipment[i] = item;
                }
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                UserMagic magic = new UserMagic(reader, version, customVersion);
                if (magic.Info == null) continue;

                magic.CastTime = int.MinValue;
                Magics.Add(magic);
            }  
            
            if (version > 99)
            {
                AutoPot = reader.ReadBoolean();
                Grade = reader.ReadByte();
                HPItemIndex = reader.ReadInt32();
                MPItemIndex = reader.ReadInt32();
                AutoHPPercent = reader.ReadByte();
                AutoMPPercent = reader.ReadByte();
            }

            if (version > 101)
                SealCount = reader.ReadUInt16();

        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Name);
            writer.Write(Level);
            writer.Write((byte) Class);
            writer.Write((byte) Gender);
            writer.Write(Hair);

            writer.Write(CreationDate.ToBinary());

            writer.Write(Deleted);
            writer.Write(DeleteDate.ToBinary());

            writer.Write(HP);
            writer.Write(MP);
            writer.Write(Experience);

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

            writer.Write(Magics.Count);
            for (int i = 0; i < Magics.Count; i++)
            {
                Magics[i].Save(writer);
            }

            writer.Write(AutoPot);
            writer.Write(Grade);
            writer.Write(HPItemIndex);
            writer.Write(MPItemIndex);
            writer.Write(AutoHPPercent);
            writer.Write(AutoMPPercent);
            writer.Write(SealCount);
        }

        public override int ResizeInventory()
        {
            if (Inventory.Length >= 42) return Inventory.Length;
            Array.Resize(ref Inventory, Inventory.Length + 8);
            return Inventory.Length;
        }

        public ClientHeroInformation ClientInformation
        {
            get
            {
                return new ClientHeroInformation()
                {
                    Index = Index,
                    Name = Name,
                    Level = Math.Max((ushort)1, Level),
                    Class = Class,
                    Gender = Gender
                };
            }
        }
    }
}
