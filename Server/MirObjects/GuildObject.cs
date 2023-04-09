using Server.MirEnvir;
using Server.MirDatabase;

namespace Server.MirObjects
{
    public sealed class GuildObject
    {
        private static Envir Envir
        {
            get { return Envir.Main; }
        }

        public GuildInfo Info;

        public int Guildindex
        {
            get { return Info.GuildIndex; }
            set { Info.GuildIndex = value; }
        }

        public string Name
        {
            get { return Info.Name; }
            set { Info.Name = value; }
        }

        public bool NeedSave
        {
            get { return Info.NeedSave; }
            set { Info.NeedSave = value; }
        }

        public List<GuildRank> Ranks
        {
            get { return Info.Ranks; }
            set { Info.Ranks = value; }
        }

        public List<GuildBuff> BuffList
        {
            get { return Info.BuffList; }
            set { Info.BuffList = value; }
        }

        public GuildStorageItem[] StoredItems
        {
            get { return Info.StoredItems; }
            set { Info.StoredItems = value; }
        }

        public uint Gold
        {
            get { return Info.Gold; }
            set { Info.Gold = value; }
        }

        public long NextExpUpdate = 0;
        public List<GuildObject> WarringGuilds = new List<GuildObject>();

        public ConquestObject Conquest;

        public List<GuildObject> AllyGuilds = new List<GuildObject>();
        public int AllyCount;

        public GuildObject(GuildInfo info)
        {
            Info = info;

            Envir.Guilds.Add(this);
        }

        public void SendMessage(string message, ChatType Type = ChatType.Guild)
        {
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        player.ReceiveChat(message, Type);
                    }
                }
            }
        }

        public void SendOutputMessage(string message, OutputMessageType Type = OutputMessageType.Guild)
        {
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        player.ReceiveOutputMessage(message, Type);
                    }
                }
            }
        }

        public List<PlayerObject> GetOnlinePlayers()
        {
            List<PlayerObject> players = new List<PlayerObject>();

            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        players.Add(player);
                    }
                }
            }

            return players;
        }

        public void PlayerLogged(PlayerObject member, bool online, bool New = false)
        {
            for (int i = 0; i < Ranks.Count; i++)
            {
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
            }

            SendServerPacket(new ServerPackets.GuildMemberChange() {Name = member.Name, Status = (byte)(New? 2: online? 1: 0)});

            if (online && !New)
            {
                SendGuildStatus(member);
            }
        }

        public void SendGuildStatus(PlayerObject member)
        {
            string gName = Name;

            if (Conquest != null)
            {
                gName += "[" + Conquest.Info.Name + "]";
            }

            member.Enqueue(new ServerPackets.GuildStatus()
                {
                    GuildName = gName,
                    GuildRankName = member.MyGuildRank != null? member.MyGuildRank.Name: "",
                    Experience = Info.Experience,
                    MaxExperience = Info.MaxExperience,
                    MemberCount = Info.Membercount,
                    MaxMembers = Info.MemberCap,
                    Gold = Info.Gold,
                    Level = Info.Level,
                    Voting = Info.Voting,
                    SparePoints = Info.SparePoints,
                    ItemCount = (byte)Info.StoredItems.Length,
                    BuffCount = (byte)0,//(byte)BuffList.Count,
                    MyOptions = member.MyGuildRank != null? member.MyGuildRank.Options: (GuildRankOptions)0,
                    MyRankId = member.MyGuildRank != null? member.MyGuildRank.Index: 256
                });
        }

        public void NewMember(PlayerObject newMember)
        {
            if (Ranks.Count < 2)
            {
                Ranks.Add(new GuildRank() { Name = "Members", Index = 1 });
            }

            GuildRank lowestRank = Ranks[Ranks.Count - 1];
            GuildMember member = new GuildMember() { Name = newMember.Info.Name, Player = newMember, Id = newMember.Info.Index, LastLogin = Envir.Now, Online = true };
            lowestRank.Members.Add(member);

            PlayerLogged(newMember, true, true);
            Info.Membercount++;
            NeedSave = true;
        }

        public bool ChangeRank(PlayerObject self, string memberName, byte rankIndex, string rankName = "Members")
        {
            if ((self.MyGuild != this) || (self.MyGuildRank == null)) return false;
            if (rankIndex >= Ranks.Count) return false;
            GuildMember Member = null;
            GuildRank MemberRank = null;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                    if (Ranks[i].Members[j].Name == memberName)
                    {
                        Member = Ranks[i].Members[j];
                        MemberRank = Ranks[i];
                        goto Found;
                    }

            Found:
            if (Member == null) return false;

            MirDatabase.CharacterInfo Character = Envir.GetCharacterInfo(memberName);
            if (Character == null) return false;
            if ((rankIndex == 0) && (Character.Level < Settings.Guild_RequiredLevel))
            {
                self.ReceiveChat(String.Format("A guild leader needs to be at least level {0}", Settings.Guild_RequiredLevel), ChatType.System);
                return false;
            }

            if ((MemberRank.Index >= self.MyGuildRank.Index) && (self.MyGuildRank.Index != 0))return false;
            if (MemberRank.Index == 0)
            {
                if (MemberRank.Members.Count <= 2)
                {
                    self.ReceiveChat("A guild needs at least 2 leaders.", ChatType.System);
                    return false;
                }
                for (int i = 0; i < MemberRank.Members.Count; i++)
                {
                    if ((MemberRank.Members[i].Player != null) && (MemberRank.Members[i] != Member))
                        goto AllOk;
                }
                self.ReceiveChat("You need at least 1 leader online.", ChatType.System);
                return false;
            }

            AllOk:
            Ranks[rankIndex].Members.Add(Member);
            MemberRank.Members.Remove(Member);

            MemberRank = Ranks[rankIndex];

            List<GuildRank> NewRankList = new List<GuildRank>
            {
                Ranks[rankIndex]
            };
            NeedSave = true;
            PlayerObject player = (PlayerObject)Member.Player;
            if (player != null)
            {
                player.MyGuildRank = Ranks[rankIndex];
                player.Enqueue(new ServerPackets.GuildMemberChange() { Name = self.Info.Name, Status = (byte)8, Ranks = NewRankList });
                player.BroadcastInfo();
            }

            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                    if ((Ranks[i].Members[j].Player != null) && (Ranks[i].Members[j].Player != Member.Player))
                    {
                        player = (PlayerObject)Ranks[i].Members[j].Player;
                        player.Enqueue(new ServerPackets.GuildMemberChange() { Name = Member.Name, Status = (byte)5, RankIndex = (byte)MemberRank.Index });
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
            GuildRank NewRank = new GuildRank(){Index = NewIndex, Name = String.Format("Rank-{0}",NewIndex), Options = (GuildRankOptions)0};
            Ranks.Insert(NewIndex, NewRank);
            Ranks[Ranks.Count - 1].Index = Ranks.Count - 1;
            List<GuildRank> NewRankList = new List<GuildRank>
            {
                NewRank
            };
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
            Ranks[RankIndex].Options = Enabled == "true" ? Ranks[RankIndex].Options |= (GuildRankOptions)(1 << Option) : Ranks[RankIndex].Options ^= (GuildRankOptions)(1 << Option);

            List<GuildRank> NewRankList = new List<GuildRank>
            {
                Ranks[RankIndex]
            };
            SendServerPacket(new ServerPackets.GuildMemberChange() { Name = Self.Name, Status = (byte)7, Ranks = NewRankList });
            NeedSave = true;
            return true;
        }
        public bool ChangeRankName(PlayerObject Self, string RankName, byte RankIndex)
        {
            int SelfRankIndex = -1;
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if (Ranks[i].Members[j].Player == Self)
                    {
                        SelfRankIndex = i;
                        break;
                    }
                }
            }

            if (SelfRankIndex > RankIndex)
            {
                Self.ReceiveChat("Your rank is not adequate.", ChatType.System);
                return false;
            }

            if (RankIndex >= Ranks.Count)
            {
                return false;
            }

            Ranks[RankIndex].Name = RankName;
            List<GuildRank> NewRankList = new List<GuildRank>
            {
                Ranks[RankIndex]
            };

            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        player.Enqueue(new ServerPackets.GuildMemberChange() { Name = Self.Info.Name, Status = (byte)7, Ranks = NewRankList });
                        player.GuildMembersChanged = true;
                        if (i == RankIndex)
                        {
                            player.BroadcastInfo();
                        }
                    }
                }
            }

            NeedSave = true;
            return true;
        }

        public bool DeleteMember(PlayerObject Kicker, string membername)
        {
            //careful this can lead to guild with no ranks or members(or no leader)

            GuildMember Member = null;
            GuildRank MemberRank = null;

            if ((Kicker.MyGuild != this) || (Kicker.MyGuildRank == null)) return false;

            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if (Ranks[i].Members[j].Name == membername)
                    {
                        Member = Ranks[i].Members[j];
                        MemberRank = Ranks[i];
                        goto Found;
                    }
                }
            }

        Found:
            if (Member == null) return false;
            if ((Kicker.MyGuildRank.Index >= MemberRank.Index) && (Kicker.MyGuildRank.Index != 0) && (Kicker.Info.Name != membername))
            {
                Kicker.ReceiveChat("Your rank is not adequate.", ChatType.System);
                return false;
            }

            if (MemberRank.Index == 0)
            {
                if (MemberRank.Members.Count < 2 && Info.Membercount < 2) //Checks if last remaining member (and leader)
                {
                    goto LeaderOk;
                }
                else
                {
                    if (MemberRank.Members.Count > 1) //Allows other leaders to leave without another leader online.
                        goto AllOk;
                }
                Kicker.ReceiveChat("You need to be the last leading member of the guild to disband the guild.", ChatType.System);
                return false;
            }

        AllOk:
            MemberDeleted(membername, (PlayerObject)Member.Player, Member.Name == Kicker.Info.Name);

            if (Member.Player != null)
            {
                PlayerObject LeavingMember = (PlayerObject)Member.Player;
                LeavingMember.RefreshStats();
            }

            MemberRank.Members.Remove(Member);

            NeedSave = true;
            Info.Membercount--;

            return true;

        LeaderOk:
            MemberDeleted(membername, (PlayerObject)Member.Player, Member.Name == Kicker.Info.Name);

            if (Member.Player != null)
            {
                PlayerObject LeavingMember = (PlayerObject)Member.Player;
                LeavingMember.RefreshStats();
            }

            MemberRank.Members.Remove(Member);

            Envir.DeleteGuild(this);
            Kicker.ReceiveChat("You have disbanded the guild", ChatType.System);

            return true;
        }

        public void MemberDeleted(string name, PlayerObject formerMember, bool kickSelf)
        {
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if ((Ranks[i].Members[j].Player != null) && (Ranks[i].Members[j].Player != formerMember))
                    {
                        PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                        player.Enqueue(new ServerPackets.GuildMemberChange() { Name = name, Status = (byte)(kickSelf ? 4 : 3) });
                        player.GuildMembersChanged = true;
                    }
                }
            }

            if (formerMember != null)
            {
                formerMember.Info.GuildIndex = -1;
                formerMember.MyGuild = null;
                formerMember.MyGuildRank = null;
                formerMember.ReceiveChat(kickSelf ? "You have left your guild." : "You have been removed from your guild.", ChatType.Guild);
                formerMember.RefreshStats();
                formerMember.Enqueue(new ServerPackets.GuildStatus() { GuildName = "", GuildRankName = "", MyOptions = (GuildRankOptions)0 });
                formerMember.BroadcastInfo();
            }
        }

        public GuildRank FindRank(string name)
        {
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if (Ranks[i].Members[j].Name == name)
                    {
                        return Ranks[i];
                    }
                }
            }

            return null;
        }

        public void NewNotice(List<string> notice)
        {
            Info.Notice = notice;
            NeedSave = true;

            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if (Ranks[i].Members[j].Player != null)
                    {
                        PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                        player.GuildNoticeChanged = true;
                    }
                }
            }

            SendServerPacket(new ServerPackets.GuildNoticeChange() { update = -1 });
        }

        public void SendServerPacket(Packet p)
        {
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        player.Enqueue(p);
                    }
                }
            }
        }

        public void SendItemInfo(UserItem Item)
        {
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        player.CheckItem(Item);
                    }
                }
            }
        }

        public bool HasRoom()
        {
            if (Info.Level < Settings.Guild_MembercapList.Count)
            {
                if ((Settings.Guild_MembercapList[Info.Level] != 0) && (Info.Membercount >= Settings.Guild_MembercapList[Info.Level]))
                {
                    return false;
                }
            }

            return true;
        }
        public void GainExp(uint amount)
        {
            bool Leveled = false;
            if (Info.MaxExperience <= 0) return;

            uint expAmount = (uint)(amount * Settings.Guild_ExpRate);
            if (expAmount == 0) return;

            Info.Experience += expAmount;
            
            var experience = Info.Experience;

            while (experience > Info.MaxExperience)
            {
                Leveled = true;
                Info.Level++;
                Info.SparePoints = (byte)Math.Min(byte.MaxValue, Info.SparePoints + Settings.Guild_PointPerLevel);
                experience -= Info.MaxExperience;

                if (Info.Level < Settings.Guild_ExperienceList.Count)
                {
                    Info.MaxExperience = Settings.Guild_ExperienceList[Info.Level];
                }
                else
                {
                    Info.MaxExperience = 0;
                }

                if (Info.MaxExperience == 0) break;
                if (Info.Level == byte.MaxValue) break;
            }

            if (Leveled)
            {
                if (Info.Level < Settings.Guild_MembercapList.Count)
                {
                    Info.MemberCap = Settings.Guild_MembercapList[Info.Level];
                }

                NextExpUpdate = Envir.Time + 10000;

                for (int i = 0; i < Ranks.Count; i++)
                {
                    for (int j = 0; j < Ranks[i].Members.Count; j++)
                    {
                        if (Ranks[i].Members[j].Player != null)
                        {
                            SendGuildStatus((PlayerObject)Ranks[i].Members[j].Player);
                        }
                    }
                }
            }
            else
            {
                if (NextExpUpdate < Envir.Time)
                {
                    NextExpUpdate = Envir.Time + 10000;
                    SendServerPacket(new ServerPackets.GuildExpGain() { Amount = expAmount });
                }
            }
        }


        #region Guild Wars

        public bool GoToWar(GuildObject enemyGuild)
        {
            if (enemyGuild == null)
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
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    //in a way this is a horrible spam situation, it should only broadcast to your  own guild or enemy or allies guild but not sure i wanna code yet another broadcast for that
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        //player.Enqueue(player.GetInfoEx(player));
                        player.Enqueue(new ServerPackets.ColourChanged { NameColour = player.GetNameColour(player) });
                        player.BroadcastInfo();
                    }
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
            {
                return Name + "[" + Conquest.Info.Name + "]";
            }
            else
            {
                return Name;
            }
        }

        public bool IsEnemy(GuildObject enemyGuild)
        {
            if (enemyGuild == null) return false;
            if (enemyGuild.IsAtWar() != true) return false;

            for (int i = 0; i < WarringGuilds.Count; i++)
            {
                if (WarringGuilds[i] == enemyGuild)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        public void RefreshAllStats()
        {
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    PlayerObject player = (PlayerObject)Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        player.RefreshStats();
                    }
                }
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
                {
                    SendServerPacket(new ServerPackets.GuildBuffList { ActiveBuffs = UpdatedBuffs });
                }

                RefreshAllStats();
            }
        }

        public GuildBuff GetBuff(int Id)
        {
            for (int i = 0; i < BuffList.Count; i++ )
            {
                if (BuffList[i].Id == Id)
                {
                    return BuffList[i];
                }
            }
            return null;
        }

        public void NewBuff(int Id, bool charge = true)
        {
            GuildBuffInfo info = Envir.FindGuildBuffInfo(Id);

            if (info == null)
            {
                return;
            }

            GuildBuff buff = new GuildBuff()
            {
                Id = Id,
                Info = info,
                Active = true,
            };

            buff.ActiveTimeRemaining = buff.Info.TimeLimit;

            if (charge)
            {
                ChargeForBuff(buff);
            }

            BuffList.Add(buff);
            List<GuildBuff> NewBuff = new List<GuildBuff>
            {
                buff
            };

            SendServerPacket(new ServerPackets.GuildBuffList { ActiveBuffs = NewBuff });

            //now tell everyone our new sparepoints
            for (int i = 0; i < Ranks.Count; i++)
            {
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if (Ranks[i].Members[j].Player != null)
                    {
                        SendGuildStatus((PlayerObject)Ranks[i].Members[j].Player);
                    }
                }
            }

            NeedSave = true;
            RefreshAllStats();
        }

        private void ChargeForBuff(GuildBuff buff)
        {
            if (buff == null) return;

            Info.SparePoints -= buff.Info.PointsRequirement;
        }

        public void ActivateBuff(int Id)
        {
            GuildBuff buff = GetBuff(Id);

            if (buff == null) return;
            if (buff.Active) return;//no point activating buffs if they have no time limit anyway

            if (Info.Gold < buff.Info.ActivationCost) return;

            buff.Active = true;
            buff.ActiveTimeRemaining = buff.Info.TimeLimit;

            Info.Gold -= (uint)buff.Info.ActivationCost;

            List<GuildBuff> NewBuff = new List<GuildBuff>
            {
                buff
            };

            SendServerPacket(new ServerPackets.GuildBuffList { ActiveBuffs = NewBuff });
            SendServerPacket(new ServerPackets.GuildStorageGoldChange() { Type = 2, Name = "", Amount = (uint)buff.Info.ActivationCost });

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

            GuildA.SendMessage(string.Format("War ended with {0}.", GuildB.Name), ChatType.Guild);
            GuildB.SendMessage(string.Format("War ended with {0}.", GuildA.Name), ChatType.Guild);
            GuildA.UpdatePlayersColours();
            GuildB.UpdatePlayersColours();
        }
    }

    public class GuildItemVolume
    {
        public ItemInfo Item;
        public string ItemName;
        public uint Amount;
    }
}
