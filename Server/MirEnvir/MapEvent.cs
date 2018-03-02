using Server.MirDatabase;
using Server.MirObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using S = ServerPackets;

namespace Server.MirEnvir
{
    public class PublicEvent
    {
        private static Envir Envir
        {
            get { return SMain.Envir; }
        }
        public PublicEventInfo Info;
        public Map Map;
        public List<Point> Locations = new List<Point>();
        public Point CurrentLocation;
        public List<MapRespawn> MapRespawns = new List<MapRespawn>();
        public DateTime LastRunTime = DateTime.MinValue;
        public NPCObject DefaultNPC;
        public List<SpellObject> Zone = new List<SpellObject>();
        public List<PlayerObject> Contributers = new List<PlayerObject>();
        public int Stage = 0;

        public List<PlayerObject> Players
        {
            get
            {
                List<PlayerObject> players = new List<PlayerObject>();
                for (int d = 0; d <= Info.EventSize; d++)
                {
                    for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                    {
                        if (y < 0) continue;
                        if (y >= Map.Height) break;
                        for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                        {
                            if (x < 0) continue;
                            if (x >= Map.Width) break;

                            Cell cell = Map.GetCell(x, y);
                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                switch (ob.Race)
                                {
                                    case ObjectType.Player:
                                        PlayerObject p = (PlayerObject)ob;
                                        players.Add(p);
                                        continue;
                                    default:
                                        continue;
                                }
                            }
                        }
                    }
                }

                return players;
            }

        }
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                //If event alrdy active and we're disabling it
                if (_isActive && !value)
                    DeactivateEvent();

                if (value)
                    ActivateEvent();

                _isActive = value;
            }
        }
        public PublicEvent(PublicEventInfo info, Map map)
        {
            Info = info;
            Map = map;

            var splittedCoords = info.MultipleCoords.Split(',');
            foreach (var coord in splittedCoords)
            {
                var secondSplit = coord.Split(':');

                int x = 0; int y = 0;
                if (!int.TryParse(secondSplit[0], out x))
                    return;
                if (!int.TryParse(secondSplit[1], out y))
                    return;

                Locations.Add(new Point(x, y));


                string fileName = string.Format("{0}-Event-{1}.txt", map.Info.FileName, info.Index);
                string directoryFileName = Path.Combine(Settings.EventsPath, fileName);

                if (!Directory.Exists(Settings.EventsPath))
                    Directory.CreateDirectory(Settings.EventsPath);

                if (!File.Exists(directoryFileName))
                {
                    FileStream NewFile = File.Create(directoryFileName);
                    NewFile.Close();
                }

                DefaultNPC = new NPCObject(new NPCInfo() { Name = "DefaultNPC", FileName = fileName, IsDefault = true });

            }
        }
        public void ActivateEvent()
        {
            Point randomLocation = Locations[Envir.Random.Next(Locations.Count)];
            CurrentLocation = randomLocation;
            SMain.EnqueueDebugging(string.Format("{0} Activating Event:{1}", Map.Info.FileName, Info.EventName));
            switch (Info.EventType)
            {
                case EventType.Invasion:
                    Stage = 1;
                    SpawnInvasionMonsters(1);
                    break;
                default:
                    SpawnMonsters();
                    break;
            }

            MapEventClientSide eventNotification = new MapEventClientSide()
            {
                EventName = Info.EventName,
                EventType = Info.EventType,
                Index = Info.Index,
                IsActive = true,
                Size = Info.EventSize,
                Location = CurrentLocation
            };

            foreach (var player in Map.Players)
                player.Enqueue(new S.ActivateEvent() { Event = eventNotification });

            #region Boundaries
            for (int y = CurrentLocation.Y - Info.EventSize; y <= CurrentLocation.Y + Info.EventSize; y++)
            {
                if (y < 0) continue;
                if (y >= Map.Height) break;
                for (int x = CurrentLocation.X - Info.EventSize; x <= CurrentLocation.X + Info.EventSize; x += Math.Abs(y - CurrentLocation.Y) == Info.EventSize ? 1 : Info.EventSize * 2)
                {
                    if (x < 0) continue;
                    if (x >= Map.Width) break;
                    if (!Map.Cells[x, y].Valid) continue;

                    SpellObject spell = new SpellObject
                    {
                        ExpireTime = long.MaxValue,
                        Spell = this.Info.IsSafezone ? Spell.TrapHexagon : Spell.FireWall,
                        TickSpeed = int.MaxValue,
                        CurrentLocation = new Point(x, y),
                        CurrentMap = Map,
                        Decoration = true
                    };

                    Map.Cells[x, y].Add(spell);
                    Zone.Add(spell);
                    spell.Spawned();
                }
            }
            #endregion
        }
        private void SpawnInvasionMonsters(int order)
        {
            var respawns = Info.Respawns.Where(o => o.Order == order).ToList();
            if (respawns == null || respawns.Count == 0)
            {
                SMain.EnqueueDebugging(string.Format("De Activating Invasion Event:{0} Stage:{1}", Info.EventName, Stage));
                IsActive = false;
            }

            SpawnMonstersForInvasion(respawns);


            var p = new S.EnterOrUpdatePublicEvent { };
            int total = respawns.Where(r => r.IsObjective).Select(o => o.MonsterCount).Sum(o => o);
            int alive = total;

            var monsterNames = respawns.Select(mr => mr.MonsterName).Distinct().ToList();
            var obj = (string.Format("{0}", string.Join(",", monsterNames)));

            p.RemainingCount = string.Format("{0}/{1}", alive, total);
            p.Stage = Stage;
            p.Objective = obj;

            foreach (var player in Players)
                player.Enqueue(p);
        }
        private void SpawnMonstersForInvasion(List<EventRespawn> respawns)
        {
            var topLeftLocation = new Point(CurrentLocation.X - Info.EventSize, CurrentLocation.Y - Info.EventSize);

            foreach (var eventRespawn in respawns)
            {
                var monster = Envir.GetMonsterInfo(eventRespawn.MonsterName);
                if (monster == null)
                {
                    SMain.EnqueueDebugging(string.Format("{0} Event:{1} FAILED COULDN'T FIND MONSTER {2}", Map.Info.FileName, Info.EventName, eventRespawn.MonsterName));
                    continue;
                }

                var respawnInfo = new RespawnInfo()
                {
                    Spread = eventRespawn.MonsterCount,
                    Count = eventRespawn.MonsterCount,
                    MonsterIndex = monster.Index,
                    Delay = ushort.MaxValue
                };

                if (eventRespawn.SpreadY == 0 && eventRespawn.SpreadX == 0)
                    respawnInfo.Location = CurrentLocation;
                else
                    respawnInfo.Location = new Point(topLeftLocation.X + eventRespawn.SpreadX * 2, topLeftLocation.Y + eventRespawn.SpreadY * 2);

                MapRespawn mapRespawn = new MapRespawn(respawnInfo);

                RouteInfo route = new RouteInfo()
                {
                    Location = respawnInfo.Location,
                    Delay = 1000,
                };
                mapRespawn.Route.Add(route);

                mapRespawn.Map = Map;
                mapRespawn.IsEventObjective = eventRespawn.IsObjective;
                mapRespawn.Event = this;
                MapRespawns.Add(mapRespawn);
            }

            Map.Respawns.AddRange(MapRespawns);
        }

        private void SpawnMonsters()
        {
            var topLeftLocation = new Point(CurrentLocation.X - Info.EventSize, CurrentLocation.Y - Info.EventSize);

            foreach (var eventRespawn in Info.Respawns)
            {

                var respawnInfo = new RespawnInfo();

                var monster = Envir.GetMonsterInfo(eventRespawn.MonsterName);
                if (monster == null)
                {
                    SMain.EnqueueDebugging(string.Format("{0} Activating Event:{1} FAILED COULDN'T FIND MONSTER {2}", Map.Info.FileName, Info.EventName, eventRespawn.MonsterName));
                    return;
                }
                respawnInfo.Spread = eventRespawn.MonsterCount;
                respawnInfo.Count = eventRespawn.MonsterCount;
                respawnInfo.MonsterIndex = monster.Index;
                respawnInfo.Delay = 10000;
                respawnInfo.SaveRespawnTime = false;
                respawnInfo.RespawnIndex = 0;
                respawnInfo.RespawnTicks = 0;


                if (eventRespawn.SpreadY == 0 && eventRespawn.SpreadX == 0 && eventRespawn.MonsterCount == 1)
                    respawnInfo.Location = CurrentLocation;
                else
                    respawnInfo.Location = new Point(topLeftLocation.X + eventRespawn.SpreadX * 2, topLeftLocation.Y + eventRespawn.SpreadY * 2);

                MapRespawn mapRespawn = new MapRespawn(respawnInfo);

                RouteInfo route = new RouteInfo();
                route.Location = respawnInfo.Location;
                route.Delay = 1000;
                mapRespawn.Route.Add(route);

                mapRespawn.Map = Map;
                mapRespawn.IsEventObjective = eventRespawn.IsObjective;
                mapRespawn.Event = this;
                MapRespawns.Add(mapRespawn);
            }

            Map.Respawns.AddRange(MapRespawns);
        }

        public void DeactivateEvent()
        {
            LastRunTime = DateTime.Now;

            foreach (var mapRespawn in MapRespawns)
                Map.Respawns.Remove(mapRespawn);

            MapRespawns.Clear();

            Stage = 0;
            AwardPlayers();

            MapEventClientSide eventNotification = new MapEventClientSide()
            {
                EventName = Info.EventName,
                EventType = Info.EventType,
                Index = Info.Index,
                IsActive = false,
                Size = Info.EventSize,
                Location = CurrentLocation
            };

            foreach (var player in Map.Players)
                player.Enqueue(new S.DeactivateEvent() { Event = eventNotification });

            #region RemoveBoundaries
            foreach (var spellObj in Zone)
            {
                spellObj.Despawn();
                Map.RemoveObject(spellObj);
            }
            #endregion
        }
        public void AwardPlayers()
        {
            List<PlayerObject> insidePlayers = new List<PlayerObject>();
            insidePlayers.AddRange(Players);

            foreach (var contributer in Contributers)
            {
                if (!insidePlayers.Contains(contributer) || contributer.Info == null)
                    continue;

                switch (Info.EventType)
                {
                    case EventType.DailyBoss:
                        if (contributer.CanGainDailyAward(Info.Index))
                            contributer.Info.DailyEventsCompleted.Add(Info.Index);
                        else
                            continue;
                        break;
                    case EventType.WeeklyBoss:
                        if (contributer.CanGainWeeklyAward(Info.Index))
                            contributer.Info.WeeklyEventsCompleted.Add(Info.Index);
                        else
                            continue;
                        break;
                }
                CallDefaultNPC(DefaultNPCType.EventReward, contributer);
            }
            Contributers.Clear();
        }

        public void EventMonsterDied(List<PlayerObject> monsterContributers)
        {
            if (!IsActive)
                return;

            for (int i = 0; i < monsterContributers.Count; i++)
            {
                var monContributor = monsterContributers[i];
                if (!Contributers.Contains(monContributor))
                    Contributers.Add(monContributor);
            }

            switch (Info.EventType)
            {
                case EventType.Invasion:
                    var invasionRespawns = MapRespawns.Where(o => o.IsEventObjective);

                    if (invasionRespawns.All(o => o.Count == 0))
                    {
                        Stage++;
                        SpawnInvasionMonsters(Stage);
                    }
                    else
                    {
                        var monsterNames = MapRespawns.Select(mr => mr.Monster.Name).Distinct().ToList();
                        var obj = (string.Format("{0}", string.Join(",", monsterNames)));

                        int total = invasionRespawns.Select(o => o.Info.Count).Sum(o => o);
                        int alive = invasionRespawns.Select(o => o.Count).Sum(o => o);
                        var dead = total - alive;

                        var remainingCount = string.Format("{0}/{1}", alive, total);
                        var completedPerc = (int)(((decimal)dead / total) * 100);
                        var p = new S.EnterOrUpdatePublicEvent(Info.EventName, obj, remainingCount, completedPerc, Stage);

                        foreach (var player in Players)
                            player.Enqueue(p);
                    }
                    break;
                default:
                    var objectiveRespawns = MapRespawns.Where(o => o.IsEventObjective);

                    int totalMonstersAsObj = objectiveRespawns.Select(o => o.Info.Count).Sum(o => o);
                    int aliveMonstersCount = objectiveRespawns.Select(o => o.Count).Sum(o => o);
                    var deadMonsters = totalMonstersAsObj - aliveMonstersCount;

                    var remainCount = string.Format("{0}/{1}", aliveMonstersCount, totalMonstersAsObj);
                    var percent = (int)(((decimal)deadMonsters / totalMonstersAsObj) * 100);

                    var packet = new S.EnterOrUpdatePublicEvent(Info.EventName, string.Empty, remainCount, percent, Stage);

                    foreach (var player in Players)
                        player.Enqueue(packet);


                    if (objectiveRespawns.All(o => o.Count == 0))
                    {
                        SMain.EnqueueDebugging(string.Format("De Activating Event:{0}", Info.EventName));
                        IsActive = false;
                    }
                    break;
            }
        }
        public void Process()
        {
            //First Run
            if (LastRunTime == DateTime.MinValue)
            {
                IsActive = true;
                LastRunTime = DateTime.Now;
                return;
            }

            //Process Logic
            if (!IsActive)
            { 
                if ((DateTime.Now - LastRunTime).TotalMinutes > Info.CooldownInMinutes)
                    IsActive = true;
            }

        }
        public void CallDefaultNPC(DefaultNPCType type, PlayerObject player, params object[] value)
        {
            string key = string.Empty;

            switch (type)
            {
                case DefaultNPCType.Login:
                    key = "Login";
                    break;
                case DefaultNPCType.UseItem:
                    if (value.Length < 1) return;
                    key = string.Format("UseItem({0})", value[0]);
                    break;
                case DefaultNPCType.Trigger:
                    if (value.Length < 1) return;
                    key = string.Format("Trigger({0})", value[0]);
                    break;
                case DefaultNPCType.MapCoord:
                    if (value.Length < 3) return;
                    key = string.Format("MapCoord({0},{1},{2})", value[0], value[1], value[2]);
                    break;
                case DefaultNPCType.MapEnter:
                    if (value.Length < 1) return;
                    key = string.Format("MapEnter({0})", value[0]);
                    break;
                case DefaultNPCType.Die:
                    key = "Die";
                    break;
                case DefaultNPCType.LevelUp:
                    key = "LevelUp";
                    break;
                case DefaultNPCType.CustomCommand:
                    if (value.Length < 1) return;
                    key = string.Format("CustomCommand({0})", value[0]);
                    break;
                case DefaultNPCType.OnAcceptQuest:
                    if (value.Length < 1) return;
                    key = string.Format("OnAcceptQuest({0})", value[0]);
                    break;
                case DefaultNPCType.OnFinishQuest:
                    if (value.Length < 1) return;
                    key = string.Format("OnFinishQuest({0})", value[0]);
                    break;
                case DefaultNPCType.Daily:
                    key = "Daily";
                    player.Info.NewDay = false;
                    break;
                case DefaultNPCType.TalkMonster:
                    if (value.Length < 1) return;
                    key = string.Format("TalkMonster({0})", value[0]);
                    break;
                case DefaultNPCType.EventReward:
                    key = "EventReward";
                    break;
            }
            key = string.Format("[@_{0}]", key);

            if (player == null)
                return;

            DelayedAction action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time + 1, DefaultNPC.ObjectID, key);
            player.ActionList.Add(action);

            player.Enqueue(new S.NPCUpdate { NPCID = DefaultNPC.ObjectID });
        }
    }

}
