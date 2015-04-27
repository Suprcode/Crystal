using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Server.MirEnvir;

namespace Server.MirObjects
{

    public class GuildObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public int Guildindex = 0;
        public string Name = "";
        public byte Level = 0;
        public byte SparePoints = 0;
        public long Experience = 0;
        public uint Gold = 0;
        public List<Rank> Ranks = new List<Rank>();
        public GuildStorageItem[] StoredItems = new GuildStorageItem[112];
        public List<GuildBuff> BuffList = new List<GuildBuff>();
        public Int32 Votes = 0;
        public DateTime LastVoteAttempt;
        public bool Voting = false;
        public bool NeedSave = false;
        public int Membercount = 0;
        public long MaxExperience = 0;
        public long NextExpUpdate = 0;
        public int MemberCap = 0;
        public List<string> Notice = new List<string>();
        public List<GuildObject> WarringGuilds = new List<GuildObject>();

        public GuildObject()
        {
        }
        public GuildObject(PlayerObject owner, string name)
        {
            Name = name;
            Rank Owner = new Rank() { Name = "Leader", Options = (RankOptions)255 , Index = 0};
            GuildMember Leader = new GuildMember() { name = owner.Info.Name, Player = owner, Id = owner.Info.Index, LastLogin = Envir.Now, Online = true};
            Owner.Members.Add(Leader);
            Ranks.Add(Owner);
            Membercount++;
            NeedSave = true;
            if (Level < Settings.Guild_ExperienceList.Count)
                MaxExperience = Settings.Guild_ExperienceList[Level];
            if (Level < Settings.Guild_MembercapList.Count)
                MemberCap = Settings.Guild_MembercapList[Level];
        }
        public GuildObject(BinaryReader reader) 
        {
            Guildindex = reader.ReadInt32();
            Name = reader.ReadString();
            Level = reader.ReadByte();
            SparePoints = reader.ReadByte();
            Experience = reader.ReadInt64();
            Gold = reader.ReadUInt32();
            Votes = reader.ReadInt32();
            LastVoteAttempt = DateTime.FromBinary(reader.ReadInt64());
            Voting = reader.ReadBoolean();
            int RankCount = reader.ReadInt32();
            Membercount = 0;
            for (int i = 0; i < RankCount; i++)
            {
                int index = i;
                Ranks.Add(new Rank(reader, true) { Index = index });
                Membercount += Ranks[i].Members.Count;
            }
            int ItemCount = reader.ReadInt32();
            for (int j = 0; j < ItemCount; j++)
            {
                if (Envir.Version > 28)
                    if (!reader.ReadBoolean()) continue;
                GuildStorageItem Guilditem = new GuildStorageItem()
                {
                    Item = new UserItem(reader, Envir.LoadVersion),
                    UserId = reader.ReadInt64()
                };
                
                if (SMain.Envir.BindItem(Guilditem.Item) && j < StoredItems.Length)
                    StoredItems[j] = Guilditem;
            }
            int BuffCount = reader.ReadInt32();
            for (int j = 0; j < BuffCount; j++)
                BuffList.Add(new GuildBuff(reader));

            int  NoticeCount = reader.ReadInt32();
            for (int j = 0; j < NoticeCount; j++)
                Notice.Add(reader.ReadString());
            if (Level < Settings.Guild_ExperienceList.Count)
                MaxExperience = Settings.Guild_ExperienceList[Level];
            if (Level < Settings.Guild_MembercapList.Count)
                MemberCap = Settings.Guild_MembercapList[Level];
        }
        public void Save(BinaryWriter writer)
        {
            int RankCount = 0;
            for (int i = Ranks.Count - 1; i >= 0; i--)
                if (Ranks[i].Members.Count > 0)
                    RankCount++;

            writer.Write(Guildindex);
            writer.Write(Name);
            writer.Write(Level);
            writer.Write(SparePoints);
            writer.Write(Experience);
            writer.Write(Gold);
            writer.Write(Votes);
            writer.Write(LastVoteAttempt.ToBinary());
            writer.Write(Voting);
            writer.Write(RankCount);
            for (int i = 0; i < Ranks.Count; i++)
                if (Ranks[i].Members.Count > 0)
                    Ranks[i].Save(writer,true);
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
                BuffList[i].Save(writer);
            writer.Write(Notice.Count);
            for (int i = 0; i < Notice.Count; i++)
                writer.Write(Notice[i]);
        }

        public void SendMessage(string message, ChatType Type = ChatType.Guild)
        {
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                        player.ReceiveChat(message, Type);
                }
        }

        public void PlayerLogged(PlayerObject member, bool online, bool New = false)
        {
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if (Ranks[i].Members[j].Id == member.Info.Index)
                    {
                        if (online)
                        {
                            Ranks[i].Members[j].Player = member;
                            Ranks[i].Members[j].Online = true;
                        }
                        else
                        {
                            Ranks[i].Members[j].LastLogin = Envir.Now;
                            Ranks[i].Members[j].Player = null;
                            Ranks[i].Members[j].Online = false;
                            NeedSave = true;
                        }
                    }
                }
            SendServerPacket(new ServerPackets.GuildMemberChange() {Name = member.Name, Status = (byte)(New? 2: online? 1: 0)});
            if (online && !New)
                SendGuildStatus(member);
        }

        public void SendGuildStatus(PlayerObject member)
        {
                member.Enqueue(new ServerPackets.GuildStatus()
                {
                    GuildName = Name,
                    GuildRankName = member.MyGuildRank != null? member.MyGuildRank.Name: "",
                    Experience = Experience,
                    MaxExperience = MaxExperience,
                    MemberCount = Membercount,
                    MaxMembers = MemberCap,
                    Gold = Gold,
                    Level = Level,
                    Voting = Voting,
                    SparePoints = SparePoints,
                    ItemCount = (byte)StoredItems.Length,
                    BuffCount = (byte)BuffList.Count,
                    MyOptions = member.MyGuildRank != null? member.MyGuildRank.Options: (RankOptions)0,
                    MyRankId = member.MyGuildRank != null? member.MyGuildRank.Index: 256
                });
        }

        public void NewMember(PlayerObject newmember)
        {
            if (Ranks.Count < 2)
                Ranks.Add(new Rank() { Name = "Members", Index = 1});
            Rank currentrank = Ranks[Ranks.Count - 1];
            GuildMember Member = new GuildMember() { name = newmember.Info.Name, Player = newmember, Id = newmember.Info.Index, LastLogin = Envir.Now, Online = true };
            currentrank.Members.Add(Member);
            PlayerLogged(newmember, true, true);
            
            Membercount++;
            NeedSave = true;
        }

        public bool ChangeRank(PlayerObject Self, string membername, byte RankIndex, string RankName = "Members")
        {
            if ((Self.MyGuild != this) || (Self.MyGuildRank == null)) return false;
            if (RankIndex >= Ranks.Count) return false;
            GuildMember Member = null;
            Rank MemberRank = null;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                    if (Ranks[i].Members[j].name == membername)
                    {
                        Member = Ranks[i].Members[j];
                        MemberRank = Ranks[i];
                        goto Found;
                    }

            Found:
            if (Member == null) return false;

            MirDatabase.CharacterInfo Character = Envir.GetCharacterInfo(membername);
            if (Character == null) return false;
            if ((RankIndex == 0) && (Character.Level < Settings.Guild_RequiredLevel))
            {
                Self.ReceiveChat(String.Format("A guild leader needs to be at least level {0}", Settings.Guild_RequiredLevel), ChatType.System);
                return false;
            }

            if ((MemberRank.Index >= Self.MyGuildRank.Index) && (Self.MyGuildRank.Index != 0))return false;
            if (MemberRank.Index == 0)
            {
                if (MemberRank.Members.Count <= 2)
                {
                    Self.ReceiveChat("A guild needs at least 2 leaders.", ChatType.System);
                    return false;
                }
                for (int i = 0; i < MemberRank.Members.Count; i++)
                {
                    if ((MemberRank.Members[i].Player != null) && (MemberRank.Members[i] != Member))
                        goto AllOk;
                }
                Self.ReceiveChat("You need at least 1 leader online.", ChatType.System);
                return false;
            }

            AllOk:
            Ranks[RankIndex].Members.Add(Member);
            MemberRank.Members.Remove(Member);

            MemberRank = Ranks[RankIndex];

            List<Rank> NewRankList = new List<Rank>();
            NewRankList.Add(Ranks[RankIndex]);
            NeedSave = true;
            PlayerObject player = (PlayerObject)Member.Player;
            if (player != null)
            {
                player.MyGuildRank = Ranks[RankIndex];
                player.Enqueue(new ServerPackets.GuildMemberChange() { Name = Self.Info.Name, Status = (byte)8, Ranks = NewRankList });
                player.BroadcastInfo();
            }

            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                    if ((Ranks[i].Members[j].Player != null) && (Ranks[i].Members[j].Player != Member.Player))
                    {
                        player = (PlayerObject)Ranks[i].Members[j].Player;
                        player.Enqueue(new ServerPackets.GuildMemberChange() { Name = Member.name, Status = (byte)5, RankIndex = (byte)MemberRank.Index });
                        player.GuildMembersChanged = true;
                    }
            return true;
        }

        public bool NewRank(PlayerObject Self)
        {
            if (Ranks.Count > 254)
            {
                Self.ReceiveChat("You cannot have anymore ranks.", ChatType.System);
                return false;
            }
            int NewIndex = Ranks.Count > 1? Ranks.Count -1: 1;
            Rank NewRank = new Rank(){Index = NewIndex, Name = String.Format("Rank-{0}",NewIndex), Options = (RankOptions)0};
            Ranks.Insert(NewIndex, NewRank);
            Ranks[Ranks.Count - 1].Index = Ranks.Count - 1;
            List<Rank> NewRankList = new List<Rank>();
            NewRankList.Add(NewRank);
            SendServerPacket(new ServerPackets.GuildMemberChange() { Name = Self.Name, Status = (byte)6, Ranks = NewRankList});
            NeedSave = true;
            return true;
        }

        public bool ChangeRankOption(PlayerObject Self, byte RankIndex, int Option, string Enabled)
        {
            if ((RankIndex >= Ranks.Count) || (Option > 7))
            {
                Self.ReceiveChat("Rank not found!", ChatType.System);
                return false;
            }
            if (Self.MyGuildRank.Index >= RankIndex)
            {
                Self.ReceiveChat("You cannot change the options of your own rank!", ChatType.System);
                return false;
            }
            if ((Enabled != "true") && (Enabled != "false"))
            {
                return false;
            }
            Ranks[RankIndex].Options = Enabled == "true" ? Ranks[RankIndex].Options |= (RankOptions)(1 << Option) : Ranks[RankIndex].Options ^= (RankOptions)(1 << Option);

            List<Rank> NewRankList = new List<Rank>();
            NewRankList.Add(Ranks[RankIndex]);
            SendServerPacket(new ServerPackets.GuildMemberChange() { Name = Self.Name, Status = (byte)7, Ranks = NewRankList });
            NeedSave = true;
            return true;
        }
        public bool ChangeRankName(PlayerObject Self, string RankName, byte RankIndex)
        {
            int SelfRankIndex = -1;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if (Ranks[i].Members[j].Player == Self)
                    {
                        SelfRankIndex = i;
                        break;
                    }
                }

            if (SelfRankIndex > RankIndex)
            {
                Self.ReceiveChat("Your rank is not adequate.", ChatType.System);
                return false;
            }
            if (RankIndex >= Ranks.Count)
                return false;
            Ranks[RankIndex].Name = RankName;
            PlayerObject player = null;
            List<Rank> NewRankList = new List<Rank>();
            NewRankList.Add(Ranks[RankIndex]);
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        player.Enqueue(new ServerPackets.GuildMemberChange() { Name = Self.Info.Name, Status = (byte)7, Ranks = NewRankList });
                        player.GuildMembersChanged = true;
                        if (i == RankIndex)
                            player.BroadcastInfo();
                    }
                }
            NeedSave = true;
            return true;
        }

        public bool DeleteMember(PlayerObject Kicker, string membername)
        {//carefull this can lead to guild with no ranks or members(or no leader)

            GuildMember Member = null;
            Rank MemberRank = null;
            if ((Kicker.MyGuild != this) || (Kicker.MyGuildRank == null)) return false;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                    if (Ranks[i].Members[j].name == membername)
                    {
                        Member = Ranks[i].Members[j];
                        MemberRank = Ranks[i];
                        goto Found;
                    }

            Found:
            if (Member == null) return false;
            if (((Kicker.MyGuildRank.Index >= MemberRank.Index) && (Kicker.MyGuildRank.Index != 0)) && (Kicker.Info.Name != membername))
            {
                Kicker.ReceiveChat("Your rank is not adequate.", ChatType.System);
                return false;
            }
            if (MemberRank.Index == 0 && Name != Settings.Guild_NewbieName)
            {
                if (MemberRank.Members.Count < 2)
                {
                    Kicker.ReceiveChat("You cannot leave the guild when you're leader.", ChatType.System);
                    return false;
                }
                for (int i = 0; i < MemberRank.Members.Count; i++)
                    if ((MemberRank.Members[i].Online) && (MemberRank.Members[i] != Member))
                        goto AllOk;
                Kicker.ReceiveChat("You need at least 1 leader online.", ChatType.System);
                return false;
            }
            AllOk:
            MemberDeleted(membername, (PlayerObject)Member.Player, Member.name == Kicker.Info.Name);
            MemberRank.Members.Remove(Member);
            NeedSave = true;
            Membercount--;
            return true;
        }

        public void MemberDeleted(string name, PlayerObject formermember, bool kickself)
        {
            PlayerObject player = null;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if ((Ranks[i].Members[j].Player != null) && (Ranks[i].Members[j].Player != formermember))
                    {
                        player = (PlayerObject)Ranks[i].Members[j].Player;
                        player.Enqueue(new ServerPackets.GuildMemberChange() { Name = name, Status = (byte)(kickself ? 4:3) });
                        player.GuildMembersChanged = true;
                    }
                }
            if (formermember != null)
            {
                formermember.Info.GuildIndex = -1;
                formermember.MyGuild = null;
                formermember.MyGuildRank = null;
                formermember.ReceiveChat(kickself ? "You have left your guild." : "You have been removed from your guild.", ChatType.Guild);
                formermember.Enqueue(new ServerPackets.GuildStatus() { GuildName = "", GuildRankName = "", MyOptions = (RankOptions)0 });
                formermember.BroadcastInfo();
            }
        }

        public Rank FindRank(string name)
        {
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                    if (Ranks[i].Members[j].name == name)
                        return Ranks[i];
            return null;
        }
        public void NewNotice(List<string> notice)
        {
            Notice = notice;
            NeedSave = true;
            PlayerObject player = null;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                    if (Ranks[i].Members[j].Player != null)
                    {
                        player = (PlayerObject)Ranks[i].Members[j].Player;
                        player.GuildNoticeChanged = true;
                    }
            SendServerPacket(new ServerPackets.GuildNoticeChange() { update = -1 });
        }

        public void SendServerPacket(Packet p)
        {
            PlayerObject player = null;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                        player.Enqueue(p);
                }
        }

        public void SendItemInfo(UserItem Item)
        {
            PlayerObject player = null;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        player.CheckItem(Item);
                    }
                }
        }

        public bool HasRoom()
        {
            if (Level < Settings.Guild_MembercapList.Count)
                if ((Settings.Guild_MembercapList[Level] != 0) && (Membercount >= Settings.Guild_MembercapList[Level]))
                    return false;

            return true;
        }
        public void GainExp(uint amount)
        {
            bool Leveled = false;
            if (MaxExperience == 0) return;
            uint ExpAmount = (uint)(amount * Settings.Guild_ExpRate);
            if (ExpAmount == 0) return;
            Experience += ExpAmount;
            
            var experience = Experience;

            while (experience > MaxExperience)
            {
                Leveled = true;
                Level++;
                experience -= MaxExperience;
                if (Level < Settings.Guild_ExperienceList.Count)
                    MaxExperience = Settings.Guild_ExperienceList[Level];
                else
                    MaxExperience = 0;
                if (MaxExperience == 0) break;
                if (Level == byte.MaxValue) break;
            }

            if (Leveled)
            {
                if (Level < Settings.Guild_MembercapList.Count)
                    MemberCap = Settings.Guild_MembercapList[Level];
                NextExpUpdate = Envir.Time + 10000;
                for (int i = 0; i < Ranks.Count; i++)
                    for (int j = 0; j < Ranks[i].Members.Count; j++)
                        if (Ranks[i].Members[j].Player != null)
                            SendGuildStatus((PlayerObject)Ranks[i].Members[j].Player);
            }
            else
            {
                if (NextExpUpdate < Envir.Time)
                {
                    NextExpUpdate = Envir.Time + 10000;
                    SendServerPacket(new ServerPackets.GuildExpGain() { Amount = ExpAmount });
                }
            }

        }


        #region Guild Wars

        public bool GoToWar(GuildObject enemyGuild)
        {
            if(enemyGuild == null)
            {
                return false;
            }

            if (Envir.GuildsAtWar.Where(e => e.GuildA == this && e.GuildB == enemyGuild).Any() || Envir.GuildsAtWar.Where(e => e.GuildA == enemyGuild || e.GuildB == this).Any())
            {
                return false;
            }

            Envir.GuildsAtWar.Add(new GuildAtWar(this, enemyGuild));
            UpdatePlayersColours();
            enemyGuild.UpdatePlayersColours();
            return true;
        }

        public void UpdatePlayersColours()
        {
            //in a way this is a horrible spam situation, it should only broadcast to your  own guild or enemy or allies guild but not sure i wanna code yet another broadcast for that
            PlayerObject player = null;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        //player.Enqueue(player.GetInfoEx(player));
                        player.Enqueue(new ServerPackets.ColourChanged { NameColour = player.GetNameColour(player) });
                        player.BroadcastInfo();
                    }
                }
        }

        public bool IsAtWar()
        {
            if (WarringGuilds.Count == 0) return false;
            return true;
        }

        public bool IsEnemy(GuildObject enemyGuild)
        {
            if (enemyGuild == null) return false;
            if (enemyGuild.IsAtWar() != true) return false;
            for (int i = 0; i < WarringGuilds.Count; i++)
            {
                if (WarringGuilds[i] == enemyGuild)
                    return true;
            }
            return false;
        }
        #endregion
    }

    public class GuildAtWar
    {
        public GuildObject GuildA;
        public GuildObject GuildB;
        public long TimeRemaining;

        public GuildAtWar(GuildObject a, GuildObject b)
        {
            GuildA = a;
            GuildB = b;

            GuildA.WarringGuilds.Add(GuildB);
            GuildB.WarringGuilds.Add(GuildA);

            TimeRemaining = Settings.Minute * Settings.Guild_WarTime; //make this changable in server form
        }

        public void EndWar()
        {
            GuildA.WarringGuilds.Remove(GuildB);
            GuildB.WarringGuilds.Remove(GuildA);

            GuildA.SendMessage(string.Format("War ended with {0}.", GuildB.Name, ChatType.Guild));
            GuildB.SendMessage(string.Format("War ended with {0}.", GuildA.Name, ChatType.Guild));
            GuildA.UpdatePlayersColours();
            GuildB.UpdatePlayersColours();
        }
    }
}
