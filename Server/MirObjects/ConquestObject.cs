using Server.MirEnvir;
using Server.MirDatabase;

namespace Server.MirObjects
{
    public class ConquestObject
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public ConquestGuildInfo GuildInfo;

        public ConquestInfo Info
        {
            get { return GuildInfo.Info; }
            set { GuildInfo.Info = value; }
        }

        public List<ConquestGuildArcherInfo> ArcherList
        {
            get { return GuildInfo.ArcherList; }
            set { GuildInfo.ArcherList = value; }
        }

        public List<ConquestGuildGateInfo> GateList
        {
            get { return GuildInfo.GateList; }
            set { GuildInfo.GateList = value; }
        }

        public List<ConquestGuildWallInfo> WallList
        {
            get { return GuildInfo.WallList; }
            set { GuildInfo.WallList = value; }
        }

        public List<ConquestGuildSiegeInfo> SiegeList
        {
            get { return GuildInfo.SiegeList; }
            set { GuildInfo.SiegeList = value; }
        }

        public List<ConquestGuildFlagInfo> FlagList
        {
            get { return GuildInfo.FlagList; }
            set { GuildInfo.FlagList = value; }
        }

        public Dictionary<ConquestGuildFlagInfo, Dictionary<GuildObject, int>> ControlPoints
        {
            get { return GuildInfo.ControlPoints; }
            set { GuildInfo.ControlPoints = value; }
        }

        public GuildObject Guild;

        public Map ConquestMap;
        public Map PalaceMap;

        private bool AtWar = false;

        public DateTime WarStartTime;
        public DateTime WarEndTime;

        public ConquestGame GameType;
        public ConquestType StartType;

        public long ScheduleTimer;
        public long ScoreTimer;

        public const int MAX_KING_POINTS = 18; //3 minutes
        public const int MAX_CONTROL_POINTS = 6; //1 minute

        public Dictionary<GuildObject, int> KingPoints = new Dictionary<GuildObject, int>();

        public List<SpellObject> WarEffects = new List<SpellObject>();
        public List<NPCObject> ConquestNPCs = new List<NPCObject>();

        public ConquestObject(ConquestGuildInfo guildInfo)
        {
            GuildInfo = guildInfo;
        }

        public void Bind()
        {
            //Bind Info to Saved Archer objects or create new objects
            for (var j = 0; j < Info.ConquestGuards.Count; j++)
            {
                var tempArcher = GuildInfo.ArcherList.FirstOrDefault(x => x.Index == Info.ConquestGuards[j].Index);

                if (tempArcher != null)
                {
                    tempArcher.Info = Info.ConquestGuards[j];
                    tempArcher.Conquest = this;
                }
                else
                {
                    GuildInfo.ArcherList.Add(new ConquestGuildArcherInfo { Info = Info.ConquestGuards[j], Alive = true, Index = Info.ConquestGuards[j].Index, Conquest = this });
                }
            }

            //Remove archers that have been removed from DB
            for (var j = 0; j < GuildInfo.ArcherList.Count; j++)
            {
                if (GuildInfo.ArcherList[j].Info == null)
                {
                    GuildInfo.ArcherList.Remove(GuildInfo.ArcherList[j]);
                }
            }

            //Bind Info to Saved Gate objects or create new objects
            for (var j = 0; j < Info.ConquestGates.Count; j++)
            {
                var tempGate = GuildInfo.GateList.FirstOrDefault(x => x.Index == Info.ConquestGates[j].Index);

                if (tempGate != null)
                {
                    tempGate.Info = Info.ConquestGates[j];
                    tempGate.Conquest = this;
                }
                else
                {
                    GuildInfo.GateList.Add(new ConquestGuildGateInfo { Info = Info.ConquestGates[j], Health = int.MaxValue, Index = Info.ConquestGates[j].Index, Conquest = this });
                }
            }

            //Bind Info to Saved Flag objects or create new objects
            for (var j = 0; j < Info.ConquestFlags.Count; j++)
            {
                GuildInfo.FlagList.Add(new ConquestGuildFlagInfo { Info = Info.ConquestFlags[j], Index = Info.ConquestFlags[j].Index, Conquest = this });
            }

            //Remove Gates that have been removed from DB
            for (var j = 0; j < GuildInfo.GateList.Count; j++)
            {
                if (GuildInfo.GateList[j].Info == null)
                {
                    GuildInfo.GateList.Remove(GuildInfo.GateList[j]);
                }
            }

            //Bind Info to Saved Wall objects or create new objects
            for (var j = 0; j < Info.ConquestWalls.Count; j++)
            {
                var tempWall = GuildInfo.WallList.FirstOrDefault(x => x.Index == Info.ConquestWalls[j].Index);

                if (tempWall != null)
                {
                    tempWall.Info = Info.ConquestWalls[j];
                    tempWall.Conquest = this;
                }
                else
                {
                    GuildInfo.WallList.Add(new ConquestGuildWallInfo { Info = Info.ConquestWalls[j], Index = Info.ConquestWalls[j].Index, Health = int.MaxValue, Conquest = this });
                }
            }

            //Remove Walls that have been removed from DB
            for (var j = 0; j < GuildInfo.WallList.Count; j++)
            {
                if (GuildInfo.WallList[j].Info == null)
                {
                    GuildInfo.WallList.Remove(GuildInfo.WallList[j]);
                }
            }


            //Bind Info to Saved Siege objects or create new objects
            for (var j = 0; j < Info.ConquestSieges.Count; j++)
            {
                var tempSiege = GuildInfo.SiegeList.FirstOrDefault(x => x.Index == Info.ConquestSieges[j].Index);

                if (tempSiege != null)
                {
                    tempSiege.Info = Info.ConquestSieges[j];
                    tempSiege.Conquest = this;
                }
                else
                {
                    GuildInfo.SiegeList.Add(new ConquestGuildSiegeInfo { Info = Info.ConquestSieges[j], Index = Info.ConquestSieges[j].Index, Health = int.MaxValue, Conquest = this });
                }
            }

            //Remove Siege that have been removed from DB
            for (var j = 0; j < GuildInfo.SiegeList.Count; j++)
            {
                if (GuildInfo.SiegeList[j].Info == null)
                {
                    GuildInfo.SiegeList.Remove(GuildInfo.SiegeList[j]);
                }
            }

            //Bind Info to Saved Flag objects or create new objects
            for (var j = 0; j < Info.ControlPoints.Count; j++)
            {
                ConquestGuildFlagInfo cp;
                GuildInfo.ControlPoints.Add(cp = new ConquestGuildFlagInfo { Info = Info.ControlPoints[j], Index = Info.ControlPoints[j].Index, Conquest = this }, new Dictionary<GuildObject, int>());
            }

            LoadArchers();
            LoadGates();
            LoadWalls();
            LoadSieges();
            LoadFlags();
            LoadNPCs();
            LoadControlPoints();
        }

        public bool WarIsOn
        {
            get
            {
                return AtWar;
            }
            set
            {
                AtWar = value;
                AtWarChanged();
            }
        }

        public void LoadArchers()
        {
            for (int i = 0; i < ArcherList.Count; i++)
            {
                ArcherList[i].Spawn();
            }
        }

        public void LoadGates()
        {
            for (int i = 0; i < GateList.Count; i++)
            {
                GateList[i].Spawn(false);
            }
        }

        public void LoadWalls()
        {
            for (int i = 0; i < WallList.Count; i++)
            {
                WallList[i].Spawn(false);
            }
        }

        public void LoadSieges()
        {
            for (int i = 0; i < SiegeList.Count; i++)
            {
                SiegeList[i].Spawn();
            }
        }

        public void LoadFlags()
        {
            for (int i = 0; i < FlagList.Count; i++)
            {
                FlagList[i].Spawn();
            }
        }

        public void LoadNPCs()
        {
            for (int i = 0; i < ConquestMap.NPCs.Count; i++)
            {
                if (ConquestMap.NPCs[i].Info.Conquest == Info.Index)
                {
                    ConquestMap.NPCs[i].Conq = this;
                    ConquestNPCs.Add(ConquestMap.NPCs[i]);
                }
            }

            PalaceMap = Envir.GetMap(Info.PalaceIndex);

            if (PalaceMap != null)
            {
                for (int i = 0; i < PalaceMap.NPCs.Count; i++)
                {
                    if (PalaceMap.NPCs[i].Info.Conquest == Info.Index)
                    {
                        PalaceMap.NPCs[i].Conq = this;
                        ConquestNPCs.Add(ConquestMap.NPCs[i]);
                    }
                }                    
            }

            Map temp;
            for (int i = 0; i < Info.ExtraMaps.Count; i++)
            {
                temp = Envir.GetMap(Info.ExtraMaps[i]);
                if (temp == null) continue;

                for (int j = 0; j < temp.NPCs.Count; j++)
                {
                    if (temp.NPCs[j].Info.Conquest == Info.Index)
                    {
                        temp.NPCs[j].Conq = this;
                        ConquestNPCs.Add(temp.NPCs[j]);
                    }
                }
                        
            }
        }

        public void LoadControlPoints()
        {
            foreach (var cp in ControlPoints.Keys)
            {
                cp.Spawn();
            }
        }

        private void AtWarChanged()
        {
            if (WarIsOn)
            {
                if (StartType == ConquestType.Forced)
                {
                    WarStartTime = Envir.Now;
                    WarEndTime = WarStartTime.AddMinutes(Info.WarLength);
                    GameType = Info.Game;
                }
                
                NPCVisibility(true);

                switch (GameType)
                {
                    case ConquestGame.CapturePalace:
                        break;
                    case ConquestGame.KingOfHill:
                        CreateZone(true);
                        break;
                    case ConquestGame.Random:
                        break;
                    case ConquestGame.Classic:
                        break;
                }

            }
            else
            {
                NPCVisibility(false);

                for (int i = 0; i < ArcherList.Count; i++)
                {
                    if (ArcherList[i].ArcherMonster != null)
                        ArcherList[i].ArcherMonster.Target = null;
                }

                switch (StartType)
                {
                    case ConquestType.Request:
                        GuildInfo.AttackerID = -1;
                        break;
                }

                switch (GameType)
                {
                    case ConquestGame.CapturePalace:
                        break;
                    case ConquestGame.KingOfHill:
                        CreateZone(false);
                        break;
                    case ConquestGame.Random:
                        break;
                    case ConquestGame.Classic:
                        break;
                }
            }
        }

        private void NPCVisibility(bool show = true)
        {
            //Check if Players in Conquest Zone;
            for (int j = 0; j < ConquestMap.Players.Count; j++)
            {
                ConquestMap.Players[j].CheckConquest();
            }

            PalaceMap = Envir.GetMap(Info.PalaceIndex);
            if (PalaceMap != null)
            {
                if (show)
                {
                    PalaceMap.tempConquest = this;
                }
                else
                {
                    PalaceMap.tempConquest = null;
                }

                for (int j = 0; j < PalaceMap.Players.Count; j++)
                {
                    PalaceMap.Players[j].CheckConquest();
                }
            }

            Map temp;
            for (int i = 0; i < Info.ExtraMaps.Count; i++)
            {
                temp = Envir.GetMap(Info.ExtraMaps[i]);

                if (temp == null) continue;

                if (show)
                {
                    temp.tempConquest = this;
                }
                else
                {
                    temp.tempConquest = null;
                }

                for (int k = 0; k < temp.Players.Count; k++)
                {
                    temp.Players[k].CheckConquest();
                }
            }


            //Set NPCs to invisible
            Map npcMap;
            NPCObject npcTemp;

            for (int i = 0; i < ConquestNPCs.Count; i++)
            {
                npcMap = ConquestNPCs[i].CurrentMap;
                npcTemp = ConquestNPCs[i];
                for (int j = 0; j < npcMap.Players.Count; j++)
                {
                    if (Functions.InRange(npcTemp.CurrentLocation, npcMap.Players[j].CurrentLocation, Globals.DataRange))
                        npcTemp.CheckVisible(npcMap.Players[j]);
                }

            }

        }

        public bool CheckDay()
        {
            switch (Envir.Now.DayOfWeek.ToString())
            {
                case "Monday":
                    return Info.Monday;
                case "Tuesday":
                    return Info.Tuesday;
                case "Wednesday":
                    return Info.Wednesday;
                case "Thursday":
                    return Info.Thursday;
                case "Friday":
                    return Info.Friday;
                case "Saturday":
                    return Info.Saturday;
                case "Sunday":
                    return Info.Sunday;
            }

            return true;
        }

        public void AutoSchedule()
        {
            int start = ((Info.StartHour * 60));
            int finish = ((Info.StartHour * 60) + Info.WarLength);
            int now = ((Envir.Now.Hour * 60) + Envir.Now.Minute);

            if (WarIsOn && StartType == ConquestType.Forced && WarEndTime <= Envir.Now)
            {
                EndWar(Info.Game);
            }
            
            if (StartType != ConquestType.Forced)
            {
                if (WarIsOn && (now > start && finish <= now))
                {
                    EndWar(Info.Game);
                }
                else if (start <= now && finish > now && CheckDay())
                {
                    if (!WarIsOn)
                    {
                        if (Info.Type == ConquestType.Request)
                        {
                            if (GuildInfo.AttackerID != -1)
                            {
                                GameType = Info.Game;
                                StartType = Info.Type;
                                StartWar(Info.Game);
                            }
                        }
                        else
                        {
                            GameType = Info.Game;
                            StartType = Info.Type;
                            StartWar(Info.Game);
                        }
                    }
                }
            }

            ScheduleTimer = Envir.Time + Settings.Minute;
        }

        public void Process()
        {
            if (ScheduleTimer < Envir.Time) AutoSchedule();
            if (WarIsOn && (GameType == ConquestGame.KingOfHill || GameType == ConquestGame.Classic || GameType == ConquestGame.ControlPoints)) ScorePoints();
        }

        public void Reset()
        {
            GuildInfo.Owner = -1;
            GuildInfo.AttackerID = -1;
            GuildInfo.GoldStorage = 0;
            GuildInfo.NPCRate = 0;

            if (Guild != null)
            {
                Guild.Conquest = null;
                UpdatePlayers(Guild);
                Guild = null;
            }

            for (int i = 0; i < ArcherList.Count; i++)
            {
                ArcherList[i].Spawn();
            }

            for (int i = 0; i < GateList.Count; i++)
            {
                GateList[i].Repair();
            }

            for (int i = 0; i < WallList.Count; i++)
            {
                WallList[i].Repair();
            }

            for (int i = 0; i < SiegeList.Count; i++)
            {
                //SiegeList[i].Repair();
            }

            GuildInfo.NeedSave = true;
        }

        public void TakeConquest(PlayerObject player = null, GuildObject winningGuild = null)
        {
            if (winningGuild == null && (player == null || player.MyGuild == null || player.MyGuild.Conquest != null || player.Dead)) return;
            if (winningGuild != null && winningGuild.Conquest != null) return;
            if (player != null && player.MyGuild != null && player.MyGuild.Conquest != null) return;

            GuildObject tmpPrevious = null;

            switch (GameType)
            {
                case ConquestGame.CapturePalace:
                    if (StartType == ConquestType.Request)
                        if (player.MyGuild.Guildindex != GuildInfo.AttackerID) break;

                    if (Guild != null)
                    {
                        tmpPrevious = Guild;
                        Guild.Conquest = null;
                        GuildInfo.AttackerID = tmpPrevious.Guildindex;
                    }

                    GuildInfo.Owner = player.MyGuild.Guildindex;
                    Guild = player.MyGuild;
                    player.MyGuild.Conquest = this;
                    EndWar(GameType);
                    break;
                case ConquestGame.KingOfHill:
                case ConquestGame.Classic:
                    if (StartType == ConquestType.Request)
                        if (winningGuild.Guildindex != GuildInfo.AttackerID) break;

                    if (Guild != null)
                    {
                        tmpPrevious = Guild;
                        Guild.Conquest = null;
                        GuildInfo.AttackerID = tmpPrevious.Guildindex;
                    }

                    GuildInfo.Owner = winningGuild.Guildindex;
                    Guild = winningGuild;
                    Guild.Conquest = this;
                    break;
                case ConquestGame.ControlPoints:

                    if (Guild != null)
                    {
                        tmpPrevious = Guild;
                    }

                    GuildInfo.Owner = winningGuild.Guildindex;
                    Guild = winningGuild;
                    Guild.Conquest = this;

                    List<ConquestGuildFlagInfo> keys = new List<ConquestGuildFlagInfo>(ControlPoints.Keys);
                    foreach (ConquestGuildFlagInfo key in keys)
                    {
                        key.ChangeOwner(Guild);
                        ControlPoints[key] = new Dictionary<GuildObject, int>();
                    }
                    
                    break;
            }

            for (int i = 0; i < FlagList.Count; i++)
            {
                FlagList[i].Guild = Guild;
                FlagList[i].UpdateImage();
                FlagList[i].UpdateColour();
            }

            if (Guild != null &&
                (tmpPrevious == null || Guild != tmpPrevious))
            {
                UpdatePlayers(Guild);
                if (tmpPrevious != null)
                {
                    tmpPrevious.Conquest = null;
                    UpdatePlayers(tmpPrevious);
                }
                GuildInfo.NeedSave = true;
            }
        }

        public void UpdatePlayers(GuildObject tempGuild)
        {
            PlayerObject player = null;
            Packet p;

            for (int i = 0; i < tempGuild.Ranks.Count; i++)
                for (int j = 0; j < tempGuild.Ranks[i].Members.Count; j++)
                {
                    player = (PlayerObject)tempGuild.Ranks[i].Members[j].Player;
                    if (player != null)
                    {
                        tempGuild.SendGuildStatus(player);
                        p = new ServerPackets.ObjectGuildNameChanged { ObjectID = player.ObjectID, GuildName = player.MyGuild.GetName()};
                        BroadcastGuildName(player, p);
                    }
                }
        }

        public void BroadcastGuildName(PlayerObject player, Packet p)
        {
            if (player.CurrentMap == null) return;

            for (int i = player.CurrentMap.Players.Count - 1; i >= 0; i--)
            {
                PlayerObject tempPlayer = player.CurrentMap.Players[i];
                if (tempPlayer == player) continue;

                if (Functions.InRange(player.CurrentLocation, tempPlayer.CurrentLocation, Globals.DataRange))
                {
                    if (p != null)
                        tempPlayer.Enqueue(p);
                }
            }
        }

        private void CreateZone(bool create = true)
        {
            if (create)
            { 
                WarEffects.Clear();
                for (int y = Info.KingLocation.Y - Info.KingSize; y <= Info.KingLocation.Y + Info.KingSize; y++)
                {
                    if (y < 0) continue;
                    if (y >= ConquestMap.Height) break;
                    for (int x = Info.KingLocation.X - Info.KingSize; x <= Info.KingLocation.X + Info.KingSize; x += Math.Abs(y - Info.KingLocation.Y) == Info.KingSize ? 1 : Info.KingSize * 2)
                    {
                        if (x < 0) continue;
                        if (x >= ConquestMap.Width) break;
                        if (!ConquestMap.Cells[x, y].Valid) continue;

                        SpellObject spell = new SpellObject
                        {
                            ExpireTime = long.MaxValue,
                            Spell = Spell.TrapHexagon,
                            TickSpeed = int.MaxValue,
                            CurrentLocation = new Point(x, y),
                            CurrentMap = ConquestMap,
                            Decoration = true
                        };

                        ConquestMap.Cells[x, y].Add(spell);
                        WarEffects.Add(spell);
                        spell.Spawned();
                    }
                }
            }
            else
            {
                for (int i = 0; i < WarEffects.Count; i++)
                {
                    //if (WarEffects[i].CurrentLocation != null)
                    //{
                        WarEffects[i].Despawn();
                        ConquestMap.RemoveObject(WarEffects[i]);
                    //}                
                        
                }

                WarEffects.Clear();
            }
        }

        public void ScorePoints()
        {
            bool pointsChanged = false;

            switch (GameType)
            {
                case ConquestGame.ControlPoints:
                    {
                        int points;

                        foreach (KeyValuePair<ConquestGuildFlagInfo, Dictionary<GuildObject, int>> item in ControlPoints)
                        {
                            pointsChanged = false;

                            ConquestGuildFlagInfo controlFlag = null;
                            Dictionary<GuildObject, int> controlFlagPoints = null;

                            for (int i = 0; i < ConquestMap.Players.Count; i++)
                            {
                                if (!ConquestMap.Players[i].WarZone || ConquestMap.Players[i].MyGuild == null || ConquestMap.Players[i].Dead) continue;

                                controlFlag = item.Key;
                                controlFlagPoints = item.Value;

                                if (!Functions.InRange(controlFlag.Info.Location, ConquestMap.Players[i].CurrentLocation, 3)) continue;

                                controlFlagPoints.TryGetValue(ConquestMap.Players[i].MyGuild, out points);

                                if (points == 0)
                                {
                                    controlFlagPoints[ConquestMap.Players[i].MyGuild] = 1;
                                    ConquestMap.Players[i].MyGuild.SendOutputMessage(string.Format("Gaining control of {1} {0:P0}", ((double)controlFlagPoints[ConquestMap.Players[i].MyGuild] / MAX_CONTROL_POINTS), controlFlag.Info.Name));
                                }
                                else if (points < MAX_CONTROL_POINTS)
                                {
                                    controlFlagPoints[ConquestMap.Players[i].MyGuild] += 1;
                                    ConquestMap.Players[i].MyGuild.SendOutputMessage(string.Format("Gaining control of {1} {0:P0}", ((double)controlFlagPoints[ConquestMap.Players[i].MyGuild] / MAX_CONTROL_POINTS), controlFlag.Info.Name));
                                }

                                List<GuildObject> guilds = controlFlagPoints.Keys.ToList();
                                foreach (var guild in guilds)
                                {
                                    if (ConquestMap.Players[i].MyGuild == guild) continue;
                                    controlFlagPoints.TryGetValue(ConquestMap.Players[i].MyGuild, out points);
                                    if (controlFlagPoints[guild] > 0)
                                    {
                                        controlFlagPoints[guild] -= 1;
                                    }
                                }

                                pointsChanged = true;
                            }

                            if (pointsChanged)
                            {
                                GuildObject tempWinning = Guild;
                                int tempInt;

                                //Check Scores
                                for (int i = 0; i < Envir.Guilds.Count; i++)
                                {
                                    controlFlagPoints.TryGetValue(Envir.Guilds[i], out points);
                                    if (tempWinning != null)
                                        controlFlagPoints.TryGetValue(tempWinning, out tempInt);
                                    else tempInt = 0;

                                    if (points > tempInt)
                                    {
                                        tempWinning = Envir.Guilds[i];
                                    }
                                }

                                if (tempWinning != controlFlag.Guild)
                                {
                                    controlFlag.ChangeOwner(tempWinning);

                                    for (int j = 0; j < ConquestMap.Players.Count; j++)
                                    {
                                        ConquestMap.Players[j].ReceiveChat(string.Format("{0} has captured {1} at {2}", tempWinning.Name, controlFlag.Info.Name, Info.Name), ChatType.System);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case ConquestGame.KingOfHill:
                    {
                        int points;

                        for (int i = 0; i < ConquestMap.Players.Count; i++)
                        {
                            if (ConquestMap.Players[i].WarZone && ConquestMap.Players[i].MyGuild != null && !ConquestMap.Players[i].Dead && Functions.InRange(Info.KingLocation, ConquestMap.Players[i].CurrentLocation, Info.KingSize))
                            {
                                if (StartType == ConquestType.Request && ConquestMap.Players[i].MyGuild.Guildindex != GuildInfo.AttackerID) continue;

                                if (ConquestMap.Players[i].MyGuild.Conquest != null && ConquestMap.Players[i].MyGuild.Conquest != this) continue;

                                KingPoints.TryGetValue(ConquestMap.Players[i].MyGuild, out points);

                                if (points == 0)
                                {
                                    KingPoints[ConquestMap.Players[i].MyGuild] = 1;
                                    ConquestMap.Players[i].MyGuild.SendOutputMessage(string.Format("Gaining control of {1} {0:P0}", ((double)KingPoints[ConquestMap.Players[i].MyGuild] / MAX_KING_POINTS), Info.Name));
                                }
                                else if (points < MAX_KING_POINTS)
                                {
                                    KingPoints[ConquestMap.Players[i].MyGuild] += 1;
                                    ConquestMap.Players[i].MyGuild.SendOutputMessage(string.Format("Gaining control of {1} {0:P0}", ((double)KingPoints[ConquestMap.Players[i].MyGuild] / MAX_KING_POINTS), Info.Name));
                                }

                                List<GuildObject> guilds = KingPoints.Keys.ToList();
                                foreach (var guild in guilds)
                                {
                                    if (ConquestMap.Players[i].MyGuild == guild) continue;
                                    KingPoints.TryGetValue(ConquestMap.Players[i].MyGuild, out points);
                                    if (KingPoints[guild] > 0)
                                    {
                                        KingPoints[guild] -= 1;
                                        guild.SendOutputMessage(string.Format("Losing control of {1} {0:P0}", ((double)KingPoints[guild] / MAX_KING_POINTS), Info.Name));
                                    }
                                }

                                pointsChanged = true;
                            }
                        }

                        if (pointsChanged)
                        {
                            GuildObject tempWinning = Guild;
                            int tempInt;

                            //Check Scores
                            for (int i = 0; i < Envir.Guilds.Count; i++)
                            {
                                KingPoints.TryGetValue(Envir.Guilds[i], out points);
                                if (tempWinning != null)
                                    KingPoints.TryGetValue(tempWinning, out tempInt);
                                else tempInt = 0;

                                if (points > tempInt)
                                {
                                    tempWinning = Envir.Guilds[i];
                                }
                            }

                            if (tempWinning != Guild)
                            {
                                TakeConquest(null, tempWinning);

                                for (int j = 0; j < ConquestMap.Players.Count; j++)
                                {
                                    ConquestMap.Players[j].ReceiveChat(string.Format("{0} has captured the hill", tempWinning.Name), ChatType.System);
                                }
                            }
                        }
                    }
                    break;
                case ConquestGame.Classic:
                    int guildCounter = 0;
                    GuildObject takingGuild = null;
                    for (int i = 0; i < PalaceMap.Players.Count; i++)
                    {
                        if (PalaceMap.Players[i].Dead) continue;

                        if (PalaceMap.Players[i].MyGuild != null)
                        {
                            if (takingGuild == null || takingGuild != PalaceMap.Players[i].MyGuild)
                                guildCounter++;

                            takingGuild = PalaceMap.Players[i].MyGuild;
                        }
                        else
                        {
                            guildCounter++;
                        }
                    }

                    if (guildCounter == 1 && takingGuild != Guild)
                    {
                        if (StartType == ConquestType.Request && takingGuild.Guildindex != GuildInfo.AttackerID) return;

                        TakeConquest(null, takingGuild);
                    }

                    break;
                
                default:
                    return;
            }
        }

        public void StartWar(ConquestGame type)
        {
            WarIsOn = true;

            foreach (var pl in Envir.Players)
                pl.BroadcastInfo();
        }

        public void EndWar(ConquestGame type)
        {
            WarIsOn = false;

            switch(type)
            {
                case ConquestGame.ControlPoints:
                    Dictionary<GuildObject, int> controlledPoints = new Dictionary<GuildObject, int>();
                    int count = 0;

                    foreach (KeyValuePair<ConquestGuildFlagInfo, Dictionary<GuildObject, int>> item in ControlPoints)
                    {
                        controlledPoints.TryGetValue(item.Key.Guild, out count);

                        if(count == 0)
                            controlledPoints[item.Key.Guild] = 1;
                        else
                            controlledPoints[item.Key.Guild] += 1;
                    }

                    GuildObject tempWinning = Guild;
                    int tempInt;

                    List<GuildObject> guilds = controlledPoints.Keys.ToList();

                    //Check Scores
                    for (int i = 0; i < guilds.Count; i++)
                    {
                        controlledPoints.TryGetValue(guilds[i], out count);

                        if (tempWinning != null)
                        {
                            controlledPoints.TryGetValue(tempWinning, out tempInt);
                        }
                        else
                        {
                            tempInt = 0;
                        }

                        if (count > tempInt)
                        {
                            tempWinning = Envir.Guilds[i];
                        }
                    }

                    TakeConquest(null, tempWinning);

                    break;
            }

            foreach (var pl in Envir.Players)
                pl.BroadcastInfo();
        }
    }
}
