using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Server.MirEnvir;
using System.Drawing;
using Server.MirDatabase;
using Server.MirObjects.Monsters;

namespace Server.MirObjects
{
    public class ConquestObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public ConquestInfo Info;

        public int Owner = 0;
        public GuildObject Guild;

        public int AttackerID;

        public Map ConquestMap;
        public Map PalaceMap;
        public List<Map> ExtraMaps = new List<Map>();

        public List<ConquestArcherObject> ArcherList = new List<ConquestArcherObject>();
        public List<ConquestGateObject> GateList = new List<ConquestGateObject>();
        public List<ConquestWallObject> WallList = new List<ConquestWallObject>();
        public List<ConquestSiegeObject> SiegeList = new List<ConquestSiegeObject>();
        public List<ConquestFlagObject> FlagList = new List<ConquestFlagObject>();

        public int ArcherCount;
        public int GateCount;
        public int WallCount;
        public int SiegeCount;

        public uint GoldStorage;
        public byte npcRate = 0;

        public bool NeedSave = false;
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
        public Dictionary<ConquestFlagObject, Dictionary<GuildObject, int>> ControlPoints = new Dictionary<ConquestFlagObject, Dictionary<GuildObject, int>>();

        public List<SpellObject> WarEffects = new List<SpellObject>();
        public List<NPCObject> ConquestNPCs = new List<NPCObject>();


        public ConquestObject()
        {

        }
        public ConquestObject(PlayerObject owner, string name)
        {

        }
        public ConquestObject(BinaryReader reader)
        {
            Owner = reader.ReadInt32();
            ArcherCount = reader.ReadInt32();
            for (int i = 0; i < ArcherCount; i++)
            {
                ArcherList.Add(new ConquestArcherObject(reader));
            }
            GateCount = reader.ReadInt32();
            for (int i = 0; i < GateCount; i++)
            {
                GateList.Add(new ConquestGateObject(reader));
            }
            WallCount = reader.ReadInt32();
            for (int i = 0; i < WallCount; i++)
            {
                WallList.Add(new ConquestWallObject(reader));
            }
            SiegeCount = reader.ReadInt32();
            for (int i = 0; i < SiegeCount; i++)
            {
                SiegeList.Add(new ConquestSiegeObject(reader));
            }
            GoldStorage = reader.ReadUInt32();
            npcRate = reader.ReadByte();
            AttackerID = reader.ReadInt32();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(Owner);
            writer.Write(ArcherList.Count);
            for (int i = 0; i < ArcherList.Count; i++)
            {
                ArcherList[i].Save(writer);
            }
            writer.Write(GateList.Count);
            for (int i = 0; i < GateList.Count; i++)
            {
                GateList[i].Save(writer);
            }
            writer.Write(WallList.Count);
            for (int i = 0; i < WallList.Count; i++)
            {
                WallList[i].Save(writer);
            }
            writer.Write(SiegeList.Count);
            for (int i = 0; i < SiegeList.Count; i++)
            {
                SiegeList[i].Save(writer);
            }
            writer.Write(GoldStorage);
            writer.Write(npcRate);
            writer.Write(AttackerID);
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
                if (ConquestMap.NPCs[i].Info.Conquest == Info.Index)
                {
                    ConquestMap.NPCs[i].Conq = this;
                    ConquestNPCs.Add(ConquestMap.NPCs[i]);
                }
                    

            PalaceMap = Envir.GetMap(Info.PalaceIndex);

            if (PalaceMap != null)
            {
                for (int i = 0; i < PalaceMap.NPCs.Count; i++)
                    if (PalaceMap.NPCs[i].Info.Conquest == Info.Index)
                    {
                        PalaceMap.NPCs[i].Conq = this;
                        ConquestNPCs.Add(ConquestMap.NPCs[i]);
                    }
                        
            }

            Map temp;
            for (int i = 0; i < Info.ExtraMaps.Count; i++)
            {
                temp = Envir.GetMap(Info.ExtraMaps[i]);
                if (temp == null) continue;
                for (int j = 0; j < temp.NPCs.Count; j++)
                    if (temp.NPCs[j].Info.Conquest == Info.Index)
                    {
                        temp.NPCs[j].Conq = this;
                        ConquestNPCs.Add(temp.NPCs[j]);
                    }
                        
            }
        }

        private void AtWarChanged()
        {
            if (WarIsOn)
            {
                if (StartType == ConquestType.Forced)
                {
                    WarStartTime = DateTime.Now;
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
                        AttackerID = -1;
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

        private void NPCVisibility(bool Show = true)
        {
            //Check if Players in Conquest Zone;
            for (int j = 0; j < ConquestMap.Players.Count; j++)
                ConquestMap.Players[j].CheckConquest();

            PalaceMap = Envir.GetMap(Info.PalaceIndex);
            if (PalaceMap != null)
            {
                if (Show)
                    PalaceMap.tempConquest = this;
                else
                    PalaceMap.tempConquest = null;
            
                for (int j = 0; j < PalaceMap.Players.Count; j++)
                    PalaceMap.Players[j].CheckConquest();
            }

            Map temp;
            for (int i = 0; i < Info.ExtraMaps.Count; i++)
            {
                temp = Envir.GetMap(Info.ExtraMaps[i]);
                if (temp == null) continue;

                if (Show)
                    temp.tempConquest = this;
                else
                    temp.tempConquest = null;

                for (int k = 0; k < temp.Players.Count; k++)
                    temp.Players[k].CheckConquest();
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
            switch (DateTime.Now.DayOfWeek.ToString())
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
            int now = ((DateTime.Now.Hour * 60) + DateTime.Now.Minute);

            if (WarIsOn && StartType == ConquestType.Forced && WarEndTime <= DateTime.Now)
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
                            if (AttackerID != -1)
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
            Owner = -1;
            AttackerID = -1;
            GoldStorage = 0;
            npcRate = 0;

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

            NeedSave = true;
        }

        public void TakeConquest(PlayerObject player = null, GuildObject winningGuild = null)
        {
            if (winningGuild == null && (player == null || player.MyGuild == null || player.MyGuild.Conquest != null)) return;
            if (winningGuild != null && winningGuild.Conquest != null) return;
            if (player != null && player.MyGuild != null && player.MyGuild.Conquest != null) return;

            GuildObject tmpPrevious = null;

            switch (GameType)
            {
                case ConquestGame.CapturePalace:
                    if (player == null) return;
                    if (StartType == ConquestType.Request)
                        if (player.MyGuild.Guildindex != AttackerID) break;

                    if (Guild != null)
                    {
                        tmpPrevious = Guild;
                        Guild.Conquest = null;
                        AttackerID = tmpPrevious.Guildindex;
                    }

                    Owner = player.MyGuild.Guildindex;
                    Guild = player.MyGuild;
                    player.MyGuild.Conquest = this;
                    EndWar(GameType);
                    break;
                case ConquestGame.KingOfHill:
                case ConquestGame.Classic:
                    if (StartType == ConquestType.Request)
                        if (winningGuild.Guildindex != AttackerID) break;

                    if (Guild != null)
                    {
                        tmpPrevious = Guild;
                        Guild.Conquest = null;
                        AttackerID = tmpPrevious.Guildindex;
                    }

                    Owner = winningGuild.Guildindex;
                    Guild = winningGuild;
                    Guild.Conquest = this;
                    break;
                case ConquestGame.ControlPoints:
                    Owner = winningGuild.Guildindex;
                    Guild = winningGuild;
                    Guild.Conquest = this;

                    List<ConquestFlagObject> keys = new List<ConquestFlagObject>(ControlPoints.Keys);
                    foreach (ConquestFlagObject key in keys)
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

            if (Guild != null)
            {
                UpdatePlayers(Guild);
                if (tmpPrevious != null) UpdatePlayers(tmpPrevious);
                NeedSave = true;
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

        private void CreateZone(bool Create = true)
        {
            if (Create)
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
            bool PointsChanged = false;

            switch (GameType)
            {
                case ConquestGame.ControlPoints:
                    {
                        int points;

                        foreach (KeyValuePair<ConquestFlagObject, Dictionary<GuildObject, int>> item in ControlPoints)
                        {
                            PointsChanged = false;

                            ConquestFlagObject controlFlag = null;
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

                                PointsChanged = true;
                            }

                            if (PointsChanged)
                            {
                                GuildObject tempWinning = Guild;
                                int tempInt;

                                //Check Scores
                                for (int i = 0; i < Envir.GuildList.Count; i++)
                                {
                                    controlFlagPoints.TryGetValue(Envir.GuildList[i], out points);
                                    if (tempWinning != null)
                                        controlFlagPoints.TryGetValue(tempWinning, out tempInt);
                                    else tempInt = 0;

                                    if (points > tempInt)
                                    {
                                        tempWinning = Envir.GuildList[i];
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
                                if (StartType == ConquestType.Request && ConquestMap.Players[i].MyGuild.Guildindex != AttackerID) continue;

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

                                PointsChanged = true;
                            }
                        }

                        if (PointsChanged)
                        {
                            GuildObject tempWinning = Guild;
                            int tempInt;

                            //Check Scores
                            for (int i = 0; i < Envir.GuildList.Count; i++)
                            {
                                KingPoints.TryGetValue(Envir.GuildList[i], out points);
                                if (tempWinning != null)
                                    KingPoints.TryGetValue(tempWinning, out tempInt);
                                else tempInt = 0;

                                if (points > tempInt)
                                {
                                    tempWinning = Envir.GuildList[i];
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
                    int GuildCounter = 0;
                    GuildObject TakingGuild = null;
                    for (int i = 0; i < PalaceMap.Players.Count; i++)
                    {
                        if (PalaceMap.Players[i].Dead) continue;

                        if (PalaceMap.Players[i].MyGuild != null)
                        {
                            if (TakingGuild == null || TakingGuild != PalaceMap.Players[i].MyGuild)
                                GuildCounter++;

                            TakingGuild = PalaceMap.Players[i].MyGuild;
                        }
                        else
                        {
                            GuildCounter++;
                        }
                    }

                    if (GuildCounter == 1 && TakingGuild != Guild)
                    {
                        if (StartType == ConquestType.Request && TakingGuild.Guildindex != AttackerID) return;

                        TakeConquest(null, TakingGuild);
                    }

                    break;
                

                default:
                    return;
            }

            

        }

        public void StartWar(ConquestGame type)
        {
            WarIsOn = true;
        }

        public void EndWar(ConquestGame type)
        {
            WarIsOn = false;

            switch(type)
            {
                case ConquestGame.ControlPoints:
                    Dictionary<GuildObject, int> controlledPoints = new Dictionary<GuildObject, int>();
                    int count = 0;

                    foreach (KeyValuePair<ConquestFlagObject, Dictionary<GuildObject, int>> item in ControlPoints)
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
                            controlledPoints.TryGetValue(tempWinning, out tempInt);
                        else tempInt = 0;

                        if (count > tempInt)
                        {
                            tempWinning = Envir.GuildList[i];
                        }
                    }

                    TakeConquest(null, tempWinning);

                    break;
            }
        }
    }

    public class ConquestSiegeObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public int Index;
        public uint Health;

        public ConquestSiegeInfo Info;

        public ConquestObject Conquest;

        public Gate Gate;


        public ConquestSiegeObject()
        {

        }
        public ConquestSiegeObject(PlayerObject owner, string name)
        {

        }
        public ConquestSiegeObject(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Health = reader.ReadUInt32();
        }
        public void Save(BinaryWriter writer)
        {
            //if (Gate != null) Health = Gate.HP; - needs adding
            writer.Write(Index);
            writer.Write(Health);
        }


        public void Spawn()
        {
            if (Gate != null) Gate.Despawn();

            MonsterInfo monsterInfo = Envir.GetMonsterInfo(Info.MobIndex);

            if (monsterInfo == null) return;
            if (monsterInfo.AI != 72) return;

            if (monsterInfo.AI == 72)
                Gate = (Gate)MonsterObject.GetMonster(monsterInfo);
            else if (monsterInfo.AI == 73)
                //Gate = (GateWest)MonsterObject.GetMonster(monsterInfo);


                if (Gate == null) return;

            Gate.Conquest = Conquest;
            Gate.GateIndex = Index;

            Gate.Spawn(Conquest.ConquestMap, Info.Location);

            if (Health == 0)
                Gate.Die();
            else
                Gate.SetHP(Health);

            Gate.CheckDirection();
        }

        public uint GetRepairCost()
        {
            uint cost = 0;

            if (Gate.MaxHP == Gate.HP) return cost;

            if (Gate != null)
            {
                if (Info.RepairCost != 0)
                    cost = Info.RepairCost / (Gate.MaxHP / (Gate.MaxHP - Gate.HP));
            }
            return cost;
        }

        public void Repair()
        {
            if (Gate == null)
            {
                Spawn();
                return;
            }

            if (Gate.Dead)
                Spawn();
            else
                Gate.HP = Gate.MaxHP;

            Gate.CheckDirection();


        }
    }

    public class ConquestFlagObject
    {
        public int Index;

        public ConquestFlagInfo Info;

        public ConquestObject Conquest;
        public GuildObject Guild;

        public NPCObject Flag;

        public ConquestFlagObject()
        {

        }

        public void Spawn()
        {
            NPCInfo npcInfo = new NPCInfo
            {
                Name = Info.Name,
                FileName = Info.FileName,
                Location = Info.Location,
                Image = 1000
            };

            if(Conquest.Guild != null)
            {
                Guild = Conquest.Guild;
                npcInfo.Image = Guild.FlagImage;
                npcInfo.Colour = Guild.FlagColour;
            }

            Flag = new NPCObject(npcInfo);
            Flag.CurrentMap = Conquest.ConquestMap;

            Flag.CurrentMap.AddObject(Flag);

            Flag.Spawned();
        }

        public void ChangeOwner(GuildObject guild)
        {
            Guild = guild;

            UpdateImage();
            UpdateColour();
        }

        public void UpdateImage()
        {
            if(Guild != null)
            {
                Flag.Info.Image = Guild.FlagImage;

                Flag.Broadcast(Flag.GetUpdateInfo());
            }
        }

        public void UpdateColour()
        {
            if (Guild != null)
            {
                Flag.Info.Colour = Guild.FlagColour;

                Flag.Broadcast(Flag.GetUpdateInfo());
            }
        }
    }

    public class ConquestWallObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public int Index;
        public uint Health;

        public ConquestWallInfo Info;

        public ConquestObject Conquest;

        public Wall Wall;


        public ConquestWallObject()
        {

        }
        public ConquestWallObject(PlayerObject owner, string name)
        {

        }
        public ConquestWallObject(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Health = reader.ReadUInt32();
        }
        public void Save(BinaryWriter writer)
        {
            if (Wall != null) Health = Wall.HP;
            writer.Write(Index);
            writer.Write(Wall.Health);
        }


        public void Spawn(bool repair)
        {
            if (Wall != null) Wall.Despawn();

            MonsterInfo monsterInfo = Envir.GetMonsterInfo(Info.MobIndex);

            if (monsterInfo == null) return;

            if (monsterInfo.AI != 82) return;

            Wall = (Wall)MonsterObject.GetMonster(monsterInfo);

            if (Wall == null) return;

            Wall.Conquest = Conquest;
            Wall.WallIndex = Index;

            Wall.Spawn(Conquest.ConquestMap, Info.Location);

            if (repair) Health = Wall.MaxHP;

            if (Health == 0)
                Wall.Die();
            else
                Wall.SetHP(Health);

            Wall.CheckDirection();
        }

        public uint GetRepairCost()
        {
            uint cost = 0;

            if (Wall.MaxHP == Wall.HP) return cost;
            if (Wall != null)
            {
                if (Info.RepairCost != 0)
                    cost = Info.RepairCost / (Wall.MaxHP / (Wall.MaxHP - Wall.HP));
            }
            return cost;
        }

        public void Repair()
        {
            if (Wall == null)
            {
                Spawn(true);
                return;
            }

            if (Wall.Dead)
                Spawn(true);
            else
                Wall.HP = Wall.MaxHP;

            Wall.CheckDirection();
        }
    }

    public class ConquestGateObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public int Index;
        public uint Health;

        public ConquestGateInfo Info;

        public ConquestObject Conquest;

        public Gate Gate;


        public ConquestGateObject()
        {

        }
        public ConquestGateObject(PlayerObject owner, string name)
        {

        }
        public ConquestGateObject(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Health = reader.ReadUInt32();
        }
        public void Save(BinaryWriter writer)
        {
            if (Gate != null) Health = Gate.HP;
            writer.Write(Index);
            writer.Write(Health);
        }


        public void Spawn(bool repair)
        {
            if (Gate != null) Gate.Despawn();

            MonsterInfo monsterInfo = Envir.GetMonsterInfo(Info.MobIndex);

            if (monsterInfo == null) return;
            if (monsterInfo.AI != 81) return;

            Gate = (Gate)MonsterObject.GetMonster(monsterInfo);

            if (Gate == null) return;

            Gate.Conquest = Conquest;
            Gate.GateIndex = Index;

            Gate.Spawn(Conquest.ConquestMap, Info.Location);

            if (repair) Health = Gate.MaxHP;

            if (Health == 0)
                Gate.Die();
            else
                Gate.SetHP(Health);

            Gate.CheckDirection();
        }

        public uint GetRepairCost()
        {
            uint cost = 0;

            if (Gate.MaxHP == Gate.HP) return cost;

            if (Gate != null)
            {
                if (Info.RepairCost != 0)
                    cost = Info.RepairCost / (Gate.MaxHP / (Gate.MaxHP - Gate.HP));
            }
            return cost;
        }

        public void Repair()
        {
            if (Gate == null)
            {
                Spawn(true);
                return;
            }

            if (Gate.Dead)
                Spawn(true);
            else
                Gate.HP = Gate.MaxHP;

            Gate.CheckDirection();


        }
    }

    public class ConquestArcherObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public int Index;
        public bool Alive;

        public ConquestArcherInfo Info;

        public ConquestObject Conquest;

        public ConquestArcher ArcherMonster;


        public ConquestArcherObject()
        {

        }
        public ConquestArcherObject(PlayerObject owner, string name)
        {

        }
        public ConquestArcherObject(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Alive = reader.ReadBoolean();
        }
        public void Save(BinaryWriter writer)
        {
            if (ArcherMonster == null || ArcherMonster.Dead) Alive = false;
            else Alive = true;
            writer.Write(Index);
            writer.Write(Alive);
        }


        public void Spawn(bool Revive = false)
        {
            if (Revive) Alive = true;

            MonsterInfo monsterInfo = Envir.GetMonsterInfo(Info.MobIndex);

            if (monsterInfo == null) return;
            if (monsterInfo.AI != 80) return;

            ArcherMonster = (ConquestArcher)MonsterObject.GetMonster(monsterInfo);

            if (ArcherMonster == null) return;

            ArcherMonster.Conquest = Conquest;
            ArcherMonster.ArcherIndex = Index;

            if (Alive)
                ArcherMonster.Spawn(Conquest.ConquestMap, Info.Location);
            else
            {
                ArcherMonster.Spawn(Conquest.ConquestMap, Info.Location);
                ArcherMonster.Die();
                ArcherMonster.DeadTime = Envir.Time;
            }
        }

        public uint GetRepairCost()
        {
            uint cost = 0;

            if (ArcherMonster == null || ArcherMonster.Dead)
                cost = Info.RepairCost;

            return cost;
        }
    }
}