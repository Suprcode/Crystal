using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Server.MirEnvir;
using Server.MirNetwork;
using Server.MirObjects;

namespace Server.MirDatabase
{
    public class HeroInfo
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public int Index;
        public string Name;

        public ushort Level;
        public MirClass Class;
        public MirGender Gender;
        public byte Hair;

        public DateTime CreationDate;

        public bool Deleted;
        public DateTime DeleteDate;

        public int HP, MP;
        public long Experience;
        public long MaxExperience;

        public UserItem[] Inventory = new UserItem[8], Equipment = new UserItem[14];       
        public List<UserMagic> Magics = new List<UserMagic>();            

        public PlayerObject Player;

        public HeroInfo(ClientPackets.NewHero p)
        {
            Name = p.Name;
            Class = p.Class;
            Gender = p.Gender;
            CreationDate = Envir.Now;

            Level = 1;
            RefreshMaxExperience();
        }

        public HeroInfo(BinaryReader reader, int version, int customVersion)
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
        }

        public void Save(BinaryWriter writer)
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
        }

        public int ResizeInventory()
        {
            if (Inventory.Length >= 40) return Inventory.Length;
            Array.Resize(ref Inventory, Inventory.Length + 8);
            return Inventory.Length;
        }

        public void RefreshMaxExperience()
        {
            MaxExperience = Level < Settings.ExperienceList.Count ? Settings.ExperienceList[Level - 1] : 0;
        }
    }
}
