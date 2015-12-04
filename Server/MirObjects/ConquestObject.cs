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

        public Dictionary<GuildObject, int> Points = new Dictionary<GuildObject, int>();
        public int points;

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
            for (int i = 0; i < WallCount; i++)
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
                GateList[i].Spawn();
            }
        }

        public void LoadWalls()
        {
            for (int i = 0; i < WallList.Count; i++)
            {
                WallList[i].Spawn();
            }
        }

        public void LoadSieges()
        {
            for (int i = 0; i < SiegeList.Count; i++)
            {
                SiegeList[i].Spawn();
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
                    GameType = ConquestGame.KingOfHill;
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
                WarIsOn = false;
            }
            
            if (StartType != ConquestType.Forced)
            {
            if (WarIsOn && start > now || finish <= now)
            {
                WarIsOn = false;
            }
            else if (start <= now && finish > now && CheckDay())
            {
                if (!WarIsOn)
                {
                    if (Info.Type == ConquestType.Request)
                    {
                        if (AttackerID != -1)
                        {
                            WarIsOn = true;
                            GameType = Info.Game;
                            StartType = Info.Type;
                        }
                    }
                    else
                    {
                        WarIsOn = true;
                        GameType = Info.Game;
                        StartType = Info.Type;
                    }

                }
            }
            }
            ScheduleTimer = Envir.Time + Settings.Minute;
        }


        public void Process()
        {
            if (ScheduleTimer < Envir.Time) AutoSchedule();
            if (WarIsOn && GameType == ConquestGame.KingOfHill) ScorePoints();
        }

        public void TakeConquest(PlayerObject player = null, GuildObject winningGuild = null)
        {
            if (player == null && winningGuild == null) return;
            if (player == null || player.MyGuild == null || player.MyGuild.Conquest != null) return;

            GuildObject tmpPrevious = null;

            switch (GameType)
            {
                case ConquestGame.CapturePalace:
                    if (player == null) return;
                    if (Info.Type == ConquestType.Request)
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
                    WarIsOn = false;
                    break;
                case ConquestGame.KingOfHill:
                    if (Info.Type == ConquestType.Request)
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
            }

            UpdatePlayers(Guild);
            if (tmpPrevious != null) UpdatePlayers(tmpPrevious);
            NeedSave = true;
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
            for (int y = Info.ObjectLoc.Y - Info.ObjectSize; y <= Info.ObjectLoc.Y + Info.ObjectSize; y++)
            {
                if (y < 0) continue;
                if (y >= ConquestMap.Height) break;
                for (int x = Info.ObjectLoc.X - Info.ObjectSize; x <= Info.ObjectLoc.X + Info.ObjectSize; x += Math.Abs(y - Info.ObjectLoc.Y) == Info.ObjectSize ? 1 : Info.ObjectSize * 2)
                {
                    if (x < 0) continue;
                    if (x >= ConquestMap.Width) break;
                    if (!ConquestMap.Cells[x, y].Valid) continue;

                    SpellObject spell = new SpellObject
                    {
                        ExpireTime = long.MaxValue,
                        Spell = Spell.Trap,
                        TickSpeed = int.MaxValue,
                        CurrentLocation = new Point(x, y),
                        CurrentMap = ConquestMap,
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
                    if (WarEffects[i].CurrentLocation != null)
                    {
                        WarEffects[i].Despawn();
                        ConquestMap.RemoveObject(WarEffects[i]);
                    }                
                        
                }
            }

        }

        public void ScorePoints()
        {
            bool PointsChanged = false;

            switch (GameType)
            {
                case ConquestGame.KingOfHill:
                    for (int i = 0; i < ConquestMap.Players.Count; i++)
                    {
                        if (ConquestMap.Players[i].WarZone && ConquestMap.GetInnerConquest(ConquestMap.Players[i].CurrentLocation) != null)
                            if (ConquestMap.Players[i].MyGuild != null)
                            {
                                if (StartType == ConquestType.Request && ConquestMap.Players[i].MyGuild.Guildindex != AttackerID) continue;

                                if (ConquestMap.Players[i].MyGuild.Conquest != null && ConquestMap.Players[i].MyGuild.Conquest != this) continue;

                                Points.TryGetValue(ConquestMap.Players[i].MyGuild, out points);

                                if (points == 0)
                                    Points[ConquestMap.Players[i].MyGuild] = 1;
                                else if (points < 15)
                                    Points[ConquestMap.Players[i].MyGuild] += 1;

                                foreach (var item in Points.Keys)
                                {
                                    if (ConquestMap.Players[i].MyGuild == item) continue;
                                    Points.TryGetValue(ConquestMap.Players[i].MyGuild, out points);
                                    if (points > 0)
                                        Points[item] -= 1;
                                }

                                PointsChanged = true;
                            }
                    }
                    break;

                default:
                    return;
            }

            if (PointsChanged)
            {
                GuildObject tempWinning = Guild;
                int tempInt;

                //Check Scores
                for (int i = 0; i < Envir.GuildList.Count; i++)
                {
                    Points.TryGetValue(Envir.GuildList[i], out points);
                    if (tempWinning != null)
                        Points.TryGetValue(tempWinning, out tempInt);
                    else tempInt = 0;

                    if (points > tempInt)
                    {
                        tempWinning = Envir.GuildList[i];
                    }
                }

                if (tempWinning != Guild)
                    TakeConquest(null, tempWinning);
            }

        }
    }
}