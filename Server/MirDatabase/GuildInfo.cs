using Server.MirEnvir;
using Server.MirObjects;

namespace Server.MirDatabase
{
    public class GuildInfo
    {
        public int GuildIndex = 0;
        public string Name = "";
        public byte Level = 0;
        public byte SparePoints = 0;
        public long Experience = 0;
        public uint Gold = 0;

        public Int32 Votes = 0;
        public DateTime LastVoteAttempt;
        public bool Voting = false;

        public int Membercount = 0;
        public List<GuildRank> Ranks = new List<GuildRank>();
        public GuildStorageItem[] StoredItems = new GuildStorageItem[112];
        public List<GuildBuff> BuffList = new List<GuildBuff>();
        public List<string> Notice = new List<string>();

        public long MaxExperience = 0;
        public int MemberCap = 0;

        public ushort FlagImage = 1000;
        public Color FlagColour = Color.White;

        public bool NeedSave = false;

        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public GuildInfo(PlayerObject owner, string name)
        {
            Name = name;

            var ownerRank = new GuildRank { Name = "Leader", Options = (GuildRankOptions)255, Index = 0 };
            var leader = new GuildMember { Name = owner.Info.Name, Player = owner, Id = owner.Info.Index, LastLogin = Envir.Now, Online = true };

            ownerRank.Members.Add(leader);
            Ranks.Add(ownerRank);

            Membercount++;
            NeedSave = true;

            if (Level < Settings.Guild_ExperienceList.Count)
            {
                MaxExperience = Settings.Guild_ExperienceList[Level];
            }

            if (Name == Settings.NewbieGuild)
            {
                MemberCap = Settings.NewbieGuildMaxSize;
                Level = 1;
            }
            else if(Level < Settings.Guild_MembercapList.Count)
            {
                MemberCap = Settings.Guild_MembercapList[Level];
            }

            FlagColour = Color.FromArgb(255, Envir.Random.Next(255), Envir.Random.Next(255), Envir.Random.Next(255));
        }

        public GuildInfo(BinaryReader reader)
        {
            int customversion = Envir.LoadCustomVersion;
            int version = reader.ReadInt32();
            GuildIndex = version;

            if (version == int.MaxValue)
            {
                version = reader.ReadInt32();
                customversion = reader.ReadInt32();
                GuildIndex = reader.ReadInt32();
            }
            else
            {
                version = Envir.LoadVersion;
                NeedSave = true;
            }

            Name = reader.ReadString();
            Level = reader.ReadByte();
            SparePoints = reader.ReadByte();
            Experience = reader.ReadInt64();
            Gold = reader.ReadUInt32();
            Votes = reader.ReadInt32();
            LastVoteAttempt = DateTime.FromBinary(reader.ReadInt64());
            Voting = reader.ReadBoolean();

            int rankCount = reader.ReadInt32();
            Membercount = 0;

            for (int i = 0; i < rankCount; i++)
            {
                int index = i;
                Ranks.Add(new GuildRank(reader, true) { Index = index });
                Membercount += Ranks[i].Members.Count;
            }

            int itemCount = reader.ReadInt32();
            for (int j = 0; j < itemCount; j++)
            {
                if (!reader.ReadBoolean()) continue;

                GuildStorageItem Guilditem = new GuildStorageItem()
                {
                    Item = new UserItem(reader, version, customversion),
                    UserId = reader.ReadInt64()
                };

                if (Envir.BindItem(Guilditem.Item) && j < StoredItems.Length)
                    StoredItems[j] = Guilditem;
            }

            int buffCount = reader.ReadInt32();
            if (version < 61)
            {
                for (int j = 0; j < buffCount; j++)
                    new GuildBuffOld(reader);
            }
            else
            {
                for (int j = 0; j < buffCount; j++)
                {
                    //new GuildBuff(reader);
                    BuffList.Add(new GuildBuff(reader));
                }
            }

            for (int j = 0; j < BuffList.Count; j++)
            {
                BuffList[j].Info = Envir.FindGuildBuffInfo(BuffList[j].Id);
            }

            int noticeCount = reader.ReadInt32();
            for (int j = 0; j < noticeCount; j++)
            {
                Notice.Add(reader.ReadString());
            }

            if (Level < Settings.Guild_ExperienceList.Count)
            {
                MaxExperience = Settings.Guild_ExperienceList[Level];
            }

            if (Name == Settings.NewbieGuild)
            {
                MemberCap = Settings.NewbieGuildMaxSize;
            }
            else if (Level < Settings.Guild_MembercapList.Count)
            {
                MemberCap = Settings.Guild_MembercapList[Level];
            }

            if (version > 72)
            {
                FlagImage = reader.ReadUInt16();
                FlagColour = Color.FromArgb(reader.ReadInt32());
            }
        }

        public void Save(BinaryWriter writer)
        {
            int temp = int.MaxValue;
            writer.Write(temp);
            writer.Write(Envir.Version);
            writer.Write(Envir.CustomVersion);

            int rankCount = 0;
            for (int i = Ranks.Count - 1; i >= 0; i--)
            {
                if (Ranks[i].Members.Count > 0)
                {
                    rankCount++;
                }
            }

            writer.Write(GuildIndex);
            writer.Write(Name);
            writer.Write(Level);
            writer.Write(SparePoints);
            writer.Write(Experience);
            writer.Write(Gold);
            writer.Write(Votes);
            writer.Write(LastVoteAttempt.ToBinary());
            writer.Write(Voting);

            writer.Write(rankCount);
            for (int i = 0; i < Ranks.Count; i++)
            {
                if (Ranks[i].Members.Count > 0)
                {
                    Ranks[i].Save(writer, true);
                }
            }

            writer.Write(StoredItems.Length);
            for (int i = 0; i < StoredItems.Length; i++)
            {
                writer.Write(StoredItems[i] != null);
                if (StoredItems[i] != null)
                {
                    StoredItems[i].Item.Save(writer);
                    writer.Write(StoredItems[i].UserId);
                }
            }

            writer.Write(BuffList.Count);
            for (int i = 0; i < BuffList.Count; i++)
            {
                BuffList[i].Save(writer);
            }

            writer.Write(Notice.Count);
            for (int i = 0; i < Notice.Count; i++)
            {
                writer.Write(Notice[i]);
            }

            writer.Write(FlagImage);
            writer.Write(FlagColour.ToArgb());
        }

    }
}
