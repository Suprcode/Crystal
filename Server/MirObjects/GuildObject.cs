using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Server.MirEnvir;
using System.Drawing;

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

        public ushort FlagImage = 1000;
        public Color FlagColour = Color.White;

        public ConquestObject Conquest;

        public List<GuildObject> AllyGuilds = new List<GuildObject>();
        public int AllyCount;


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

            FlagColour = Color.FromArgb(255, Envir.Random.Next(255), Envir.Random.Next(255), Envir.Random.Next(255));
        }

        public GuildObject(BinaryReader reader) 
        {
            int customversion = Envir.LoadCustomVersion;
            int version = reader.ReadInt32();
            Guildindex = version;
            if (version == int.MaxValue)
            {
                version = reader.ReadInt32();
                customversion = reader.ReadInt32();
                Guildindex = reader.ReadInt32();
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
                    Item = new UserItem(reader, version, customversion),
                    UserId = reader.ReadInt64()
                };
                
                if (SMain.Envir.BindItem(Guilditem.Item) && j < StoredItems.Length)
                    StoredItems[j] = Guilditem;
            }
            int BuffCount = reader.ReadInt32();
            if (version < 61)
            {
                for (int j = 0; j < BuffCount; j++)
                    new GuildBuffOld(reader);
            }
            else
            {
                for (int j = 0; j < BuffCount; j++)
                {
                    //new GuildBuff(reader);
                    BuffList.Add(new GuildBuff(reader));
                }
            }
            for (int j = 0; j < BuffList.Count; j++)
                BuffList[j].Info = Envir.FindGuildBuffInfo(BuffList[j].Id);
            int  NoticeCount = reader.ReadInt32();
            for (int j = 0; j < NoticeCount; j++)
                Notice.Add(reader.ReadString());
            if (Level < Settings.Guild_ExperienceList.Count)
                MaxExperience = Settings.Guild_ExperienceList[Level];
            if (Level < Settings.Guild_MembercapList.Count)
                MemberCap = Settings.Guild_MembercapList[Level];

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
            writer.Write(Envir.LoadVersion);

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

            writer.Write(FlagImage);
            writer.Write(FlagColour.ToArgb());   
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

        public void SendOutputMessage(string message, OutputMessageType Type = OutputMessageType.Guild)
        {
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                        player.ReceiveOutputMessage(message, Type);
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
            string gName = Name;
            string conquest = "";

                if (Conquest != null)
                {
                    conquest = "[" + Conquest.Info.Name + "]";
                    gName = gName + conquest;
                }

            member.Enqueue(new ServerPackets.GuildStatus()
                {
                    GuildName = gName,
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
                    BuffCount = (byte)0,//(byte)BuffList.Count,
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
            if (Ranks.Count >= byte.MaxValue)
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
            if (MemberRank.Index == 0)
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
            if (Member.Player != null)
            {
                PlayerObject LeavingMember = (PlayerObject)Member.Player;
                LeavingMember.RefreshStats();
            }
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
                formermember.RefreshStats();
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
                SparePoints = (byte)Math.Min(byte.MaxValue, SparePoints + Settings.Guild_PointPerLevel);
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

            if (Envir.GuildsAtWar.Where(e => e.GuildA == this && e.GuildB == enemyGuild).Any() || Envir.GuildsAtWar.Where(e => e.GuildA == enemyGuild && e.GuildB == this).Any())
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

        public string GetName()
        {
            if (Conquest != null)
                return Name + "[" + Conquest.Info.Name + "]";
            else
                return Name;
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

        public void RefreshAllStats()
        {
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                        player.RefreshStats();
                }
        }


        public void Process()
        {
            //guild buffs
            bool NeedUpdate = false;
            List<GuildBuff> UpdatedBuffs = new List<GuildBuff>();
            for (int k = 0; k < BuffList.Count; k++)
            {
                if ((BuffList[k].Info == null) || (BuffList[k].Info.TimeLimit == 0)) continue; //dont bother if it's infinite buffs
                if (BuffList[k].Active == false) continue;//dont bother if the buff isnt active
                BuffList[k].ActiveTimeRemaining -= 1;
                if (BuffList[k].ActiveTimeRemaining < 0)
                {
                    NeedUpdate = true;
                    BuffList[k].Active = false;
                    UpdatedBuffs.Add(BuffList[k]);
                    //SendServerPacket(new ServerPackets.RemoveGuildBuff {ObjectID = (uint)BuffList[k].Id});
                }
            }
            if (NeedUpdate)
            {
                if (UpdatedBuffs.Count > 0)
                    SendServerPacket(new ServerPackets.GuildBuffList { ActiveBuffs = UpdatedBuffs });
                RefreshAllStats();
            }
        }

        public GuildBuff GetBuff(int Id)
        {
            for (int i = 0; i < BuffList.Count; i++ )
            {
                if (BuffList[i].Id == Id)
                    return BuffList[i];
            }
            return null;
        }

        public void NewBuff(int Id, bool charge = true)
        {
            GuildBuffInfo Info = Envir.FindGuildBuffInfo(Id);
            if (Info == null) return;
            GuildBuff Buff = new GuildBuff()
            {
                Id = Id,
                Info = Info,
                Active = true,
            };
            Buff.ActiveTimeRemaining = Buff.Info.TimeLimit;

            if (charge)
            {
                ChargeForBuff(Buff);
            }

            BuffList.Add(Buff);
            List<GuildBuff> NewBuff = new List<GuildBuff>();
            NewBuff.Add(Buff);
            SendServerPacket(new ServerPackets.GuildBuffList { ActiveBuffs = NewBuff });
            //now tell everyone our new sparepoints
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                    if (Ranks[i].Members[j].Player != null)
                        SendGuildStatus((PlayerObject)Ranks[i].Members[j].Player);
            NeedSave = true;
            RefreshAllStats();
        }

        private void ChargeForBuff(GuildBuff buff)
        {
            if (buff == null) return;

            SparePoints -= buff.Info.PointsRequirement;
        }

        public void ActivateBuff(int Id)
        {
            GuildBuff Buff = GetBuff(Id);
            if (Buff == null) return;
            if (Buff.Active) return;//no point activating buffs if they have no time limit anyway
            if (Gold < Buff.Info.ActivationCost) return;
            Buff.Active = true;
            Buff.ActiveTimeRemaining = Buff.Info.TimeLimit;
            Gold -= (uint)Buff.Info.ActivationCost;
            List<GuildBuff> NewBuff = new List<GuildBuff>();
            NewBuff.Add(Buff);
            SendServerPacket(new ServerPackets.GuildBuffList { ActiveBuffs = NewBuff });
            SendServerPacket(new ServerPackets.GuildStorageGoldChange() { Type = 2, Name = "", Amount = (uint)Buff.Info.ActivationCost });
            NeedSave = true;
            RefreshAllStats();
        }
        public void RemoveAllBuffs()
        {
            //note this removes them all but doesnt reset the sparepoints!(should make some sort of 'refreshpoints' procedure for that
            SendServerPacket(new ServerPackets.GuildBuffList {Remove = 1, ActiveBuffs = BuffList});
            BuffList.Clear();
            RefreshAllStats();
            NeedSave = true;
        }
        
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

            TimeRemaining = Settings.Minute * Settings.Guild_WarTime;
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
