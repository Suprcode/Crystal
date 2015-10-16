//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Drawing;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using Server.MirDatabase;
//using Server.MirEnvir;
//using S = ServerPackets;

//namespace Server.MirObjects
//{
//    public sealed class NPCObject : MapObject
//    {
//        public override ObjectType Race
//        {
//            get { return ObjectType.Merchant; }
//        }

//        public const string
//            MainKey = "[@MAIN]",
//            BuyKey = "[@BUY]",
//            SellKey = "[@SELL]",
//            BuySellKey = "[@BUYSELL]",
//            RepairKey = "[@REPAIR]",
//            SRepairKey = "[@SREPAIR]",
//            RefineKey = "[@REFINE]",
//            RefineCheckKey = "[@REFINECHECK]",
//            RefineCollectKey = "[@REFINECOLLECT]",
//            ReplaceWedRingKey = "[@REPLACEWEDDINGRING]",
//            BuyBackKey = "[@BUYBACK]",
//            StorageKey = "[@STORAGE]",
//            ConsignKey = "[@CONSIGN]",
//            MarketKey = "[@MARKET]",
//            ConsignmentsKey = "[@CONSIGNMENT]",

//            TradeKey = "[TRADE]",
//            TypeKey = "[TYPES]",
//            QuestKey = "[QUESTS]",

//            GuildCreateKey = "[@CREATEGUILD]",
//            RequestWarKey = "[@REQUESTWAR]",
//            SendParcelKey = "[@SENDPARCEL]",
//            CollectParcelKey = "[@COLLECTPARCEL]",
//            AwakeningKey = "[@AWAKENING]",
//            DisassembleKey = "[@DISASSEMBLE]",
//            DowngradeKey = "[@DOWNGRADE]",
//            ResetKey = "[@RESET]",
//            PearlBuyKey = "[@PEARLBUY]",
//            BuyUsedKey = "[@BUYUSED]";


//        public static Regex Regex = new Regex(@"[^\{\}]<.*?/(.*?)>");
//        public static Regex Regex = new Regex(@"<.*?/(\@.*?)>");
//        public NPCInfo Info;
//        private const long TurnDelay = 10000;
//        public long TurnTime, UsedGoodsTime, VisTime;
//        public bool NeedSave;
//        public bool Visible = true;

//        public List<UserItem> Goods = new List<UserItem>();
//        public List<UserItem> UsedGoods = new List<UserItem>();
//        public Dictionary<string, List<UserItem>> BuyBack = new Dictionary<string, List<UserItem>>();

//        public List<ItemType> Types = new List<ItemType>();
//        public List<NPCPage> NPCSections = new List<NPCPage>();
//        public List<QuestInfo> Quests = new List<QuestInfo>();

//        public Dictionary<int, bool> VisibleLog = new Dictionary<int, bool>();

//        public static List<string> Args = new List<string>();

//        #region Overrides
//        public override string Name
//        {
//            get { return Info.Name; }
//            set { throw new NotSupportedException(); }
//        }

//        public override bool Blocking
//        {
//            get { return Visible; }
//        }


//        public void CheckVisible(PlayerObject Player, bool Force = false)
//        {
//            if (Info.Sabuk != false && WARISON) NEEDS ADDING WHEN SABUK IS ADDED

//            bool CanSee;

//            VisibleLog.TryGetValue(Player.Info.Index, out CanSee);

//            if (Info.FlagNeeded != 0 && !Player.Info.Flags[Info.FlagNeeded])
//            {
//                if (CanSee) CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation, Player);
//                VisibleLog[Player.Info.Index] = false;
//                return;
//            }

//            if (Info.MinLev != 0 && Player.Level < Info.MinLev || Info.MaxLev != 0 && Player.Level > Info.MaxLev)
//            {
//                if (CanSee) CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation, Player);
//                VisibleLog[Player.Info.Index] = false;
//                return;
//            }

//            if (Info.ClassRequired != "" && Player.Class.ToString() != Info.ClassRequired)
//            {
//                if (CanSee) CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation, Player);
//                VisibleLog[Player.Info.Index] = false;
//                return;
//            }

//            if (Visible && !CanSee) CurrentMap.Broadcast(new S.ObjectNPC { Name = Name, Direction = Direction, Image = Info.Image, Location = CurrentLocation, NameColour = NameColour, ObjectID = ObjectID, QuestIDs = Info.CollectQuestIndexes }, CurrentLocation, Player);
//            else if (Force && Visible) CurrentMap.Broadcast(new S.ObjectNPC { Name = Name, Direction = Direction, Image = Info.Image, Location = CurrentLocation, NameColour = NameColour, ObjectID = ObjectID, QuestIDs = Info.CollectQuestIndexes }, CurrentLocation, Player);

//            VisibleLog[Player.Info.Index] = true;

//        }

//        public override int CurrentMapIndex { get; set; }

//        public override Point CurrentLocation
//        {
//            get { return Info.Location; }
//            set { throw new NotSupportedException(); }
//        }

//        public override MirDirection Direction { get; set; }

//        public override ushort Level
//        {
//            get { throw new NotSupportedException(); }
//            set { throw new NotSupportedException(); }
//        }

//        public override uint Health
//        {
//            get { throw new NotSupportedException(); }
//        }

//        public override uint MaxHealth
//        {
//            get { throw new NotSupportedException(); }
//        }
//        #endregion

//        public NPCObject(NPCInfo info)
//        {
//            Info = info;
//            NameColour = Color.Lime;

//            if (!Info.IsDefault)
//            {
//                Direction = (MirDirection)Envir.Random.Next(3);
//                TurnTime = Envir.Time + Envir.Random.Next(100);

//                Spawned();
//            }

//            LoadInfo();
//            LoadGoods();
//        }

//        public void LoadInfo(bool clear = false)
//        {
//            if (clear) ClearInfo();

//            if (!Directory.Exists(Settings.NPCPath)) return;

//            string fileName = Path.Combine(Settings.NPCPath, Info.FileName + ".txt");

//            if (File.Exists(fileName))
//            {
//                List<string> lines = File.ReadAllLines(fileName).ToList();

//                lines = ParseInsert(lines);

//                if (Info.IsDefault)
//                    ParseDefault(lines);
//                else
//                    ParseScript(lines);
//            }
//            else
//                SMain.Enqueue(string.Format("File Not Found: {0}, NPC: {1}", Info.FileName, Info.Name));
//        }

//        public void LoadGoods()
//        {
//            string path = Settings.GoodsPath + Info.Index.ToString() + ".msd";

//            if (!File.Exists(path)) return;

//            using (FileStream stream = File.OpenRead(path))
//            {
//                using (BinaryReader reader = new BinaryReader(stream))
//                {
//                    int version = reader.ReadInt32();
//                    int count = version;
//                    int customversion = Envir.LoadCustomVersion;
//                    if (version == 9999)//the only real way to tell if the file was made before or after version code got added: assuming nobody had a config option to save more then 10000 sold items :p
//                    {
//                        version = reader.ReadInt32();
//                        customversion = reader.ReadInt32();
//                        count = reader.ReadInt32();
//                    }
//                    else
//                        version = Envir.LoadVersion;
                    

//                    for (int k = 0; k < count; k++)
//                    {
//                        UserItem item = new UserItem(reader, version, customversion);
//                        if (SMain.Envir.BindItem(item))
//                            UsedGoods.Add(item);
//                    }
//                }
//            }
//        }

//        public void ClearInfo()
//        {
//            Goods = new List<UserItem>();
//            Types = new List<ItemType>();
//            NPCSections = new List<NPCPage>();

//            if (Info.IsDefault)
//            {
//                SMain.Envir.CustomCommands.Clear();
//            }
//        }

//        private void ParseDefault(List<string> lines)
//        {
//            for (int i = 0; i < lines.Count; i++)
//            {
//                if (!lines[i].ToUpper().StartsWith("[@_")) continue;

//                if (lines[i].ToUpper().Contains("MAPCOORD"))
//                {
//                    Regex regex = new Regex(@"\((.*?),([0-9]{1,3}),([0-9]{1,3})\)");
//                    Match match = regex.Match(lines[i]);

//                    if (!match.Success) continue;

//                    Map map = Envir.MapList.FirstOrDefault(m => m.Info.FileName == match.Groups[1].Value);

//                    if (map == null) continue;

//                    Point point = new Point(Convert.ToInt16(match.Groups[2].Value), Convert.ToInt16(match.Groups[3].Value));

//                    if (!map.Info.ActiveCoords.Contains(point))
//                    {
//                        map.Info.ActiveCoords.Add(point);
//                    }

//                }
//                if (lines[i].ToUpper().Contains("CUSTOMCOMMAND"))
//                {
//                    Regex regex = new Regex(@"\((.*?)\)");
//                    Match match = regex.Match(lines[i]);

//                    if (!match.Success) continue;

//                    SMain.Envir.CustomCommands.Add(match.Groups[1].Value);
//                }

//                ParseScript(lines, lines[i]);
//            }
//        }

//        private void ParseScript(List<string> lines, string key = MainKey)
//        {
//            List<string> buttons = ParseSection(lines, key);

//            for (int i = 0; i < buttons.Count; i++)
//            {
//                string section = buttons[i];

//                bool match = NPCSections.Any(t => t.Key.ToUpper() == section.ToUpper());

//                if (match) continue;

//                buttons.AddRange(ParseSection(lines, section));
//            }

//            ParseGoods(lines);
//            ParseTypes(lines);
//            ParseQuests(lines);
//        }

//        private List<string> ParseSection(List<string> lines, string sectionName)
//        {
//            List<string>
//                checks = new List<string>(),
//                acts = new List<string>(),
//                say = new List<string>(),
//                buttons = new List<string>(),
//                elseSay = new List<string>(),
//                elseActs = new List<string>(),
//                elseButtons = new List<string>(),
//                gotoButtons = new List<string>();

//            List<string> currentSay = say, currentButtons = buttons;

//            Cleans arguments out of search page name
//            string tempSectionName = ArgumentParse(sectionName);

//            for (int i = 0; i < lines.Count; i++)
//            {
//                if (!lines[i].ToUpper().StartsWith(tempSectionName.ToUpper())) continue;

//                for (int x = i + 1; x < lines.Count; x++)
//                {
//                    if (string.IsNullOrEmpty(lines[x]) || lines[x].StartsWith(";")) continue;

//                    if (lines[x].StartsWith("#"))
//                    {
//                        string[] action = lines[x].Remove(0, 1).ToUpper().Trim().Split(' ');
//                        switch (action[0])
//                        {
//                            case "IF":
//                                currentSay = checks;
//                                currentButtons = null;
//                                continue;
//                            case "SAY":
//                                currentSay = say;
//                                currentButtons = buttons;
//                                continue;
//                            case "ACT":
//                                currentSay = acts;
//                                currentButtons = gotoButtons;
//                                continue;
//                            case "ELSESAY":
//                                currentSay = elseSay;
//                                currentButtons = elseButtons;
//                                continue;
//                            case "ELSEACT":
//                                currentSay = elseActs;
//                                currentButtons = gotoButtons;
//                                continue;
//                            case "INCLUDE":
//                                lines.InsertRange(x + 1, ParseInclude(lines[x]));
//                                continue;
//                            default:
//                                throw new NotImplementedException();
//                        }
//                    }

//                    if (lines[x].StartsWith("[") && lines[x].EndsWith("]")) break;

//                    if (currentButtons != null)
//                    {
//                        Match match = Regex.Match(lines[x]);
//                        while (match.Success)
//                        {
//                            currentButtons.Add(string.Format("[{0}]", match.Groups[1].Captures[0].Value));//ToUpper()
//                            match = match.NextMatch();
//                        }

//                        Check if line has a goto command
//                        var parts = lines[x].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

//                        if (parts.Count() > 1)
//                            switch (parts[0].ToUpper())
//                            {
//                                case "GOTO":
//                                case "GROUPGOTO":
//                                    gotoButtons.Add(string.Format("[{0}]", parts[1].ToUpper()));
//                                    break;
//                                case "TIMERECALL":
//                                    if (parts.Length > 2)
//                                        gotoButtons.Add(string.Format("[{0}]", parts[2].ToUpper()));
//                                    break;
//                                case "TIMERECALLGROUP":
//                                    if (parts.Length > 2)
//                                        gotoButtons.Add(string.Format("[{0}]", parts[2].ToUpper()));
//                                    break;
//                                case "DELAYGOTO":
//                                    gotoButtons.Add(string.Format("[{0}]", parts[2].ToUpper()));
//                                    break;
//                            }
//                    }

//                    currentSay.Add(lines[x].TrimEnd());
//                }

//                break;
//            }


//            NPCPage page = new NPCPage(sectionName.ToUpper(), Info.Name, say, buttons, elseSay, elseButtons, gotoButtons);

//            for (int i = 0; i < checks.Count; i++)
//                page.ParseCheck(checks[i]);

//            for (int i = 0; i < acts.Count; i++)
//                page.ParseAct(page.ActList, acts[i]);

//            for (int i = 0; i < elseActs.Count; i++)
//                page.ParseAct(page.ElseActList, elseActs[i]);

//            NPCSections.Add(page);
//            currentButtons = new List<string>();
//            currentButtons.AddRange(buttons);
//            currentButtons.AddRange(elseButtons);
//            currentButtons.AddRange(gotoButtons);

//            return currentButtons;
//        }

//        private List<string> ParseInsert(List<string> lines)
//        {
//            List<string> newLines = new List<string>();

//            for (int i = 0; i < lines.Count; i++)
//            {
//                if (!lines[i].ToUpper().StartsWith("#INSERT")) continue;

//                string[] split = lines[i].Split(' ');

//                if (split.Length < 2) continue;

//                string path = Path.Combine(Settings.EnvirPath, split[1].Substring(1, split[1].Length - 2));

//                if (!File.Exists(path))
//                    SMain.Enqueue(string.Format("File Not Found: {0}, NPC: {1}", path, Info.Name));
//                else
//                    newLines = File.ReadAllLines(path).ToList();

//                lines.AddRange(newLines);
//            }

//            lines.RemoveAll(str => str.ToUpper().StartsWith("#INSERT"));

//            return lines;
//        }

//        private IEnumerable<string> ParseInclude(string line)
//        {
//            string[] split = line.Split(' ');

//            string path = Path.Combine(Settings.EnvirPath, split[1].Substring(1, split[1].Length - 2));
//            string page = ("[" + split[2] + "]").ToUpper();

//            bool start = false, finish = false;

//            var parsedLines = new List<string>();

//            if (!File.Exists(path)) return parsedLines;
//            IList<string> lines = File.ReadAllLines(path);

//            for (int i = 0; i < lines.Count; i++)
//            {
//                if (!lines[i].ToUpper().StartsWith(page)) continue;

//                for (int x = i + 1; x < lines.Count; x++)
//                {
//                    if (lines[x].Trim() == ("{"))
//                    {
//                        start = true;
//                        continue;
//                    }

//                    if (lines[x].Trim() == ("}"))
//                    {
//                        finish = true;
//                        break;
//                    }

//                    parsedLines.Add(lines[x]);
//                }
//            }

//            if (start && finish)
//                return parsedLines;

//            return new List<string>();
//        }


//        public string ArgumentParse(string key)
//        {
//            if (key.StartsWith("[@_")) return key; //Default NPC page so doesn't use arguments in this way

//            Regex r = new Regex(@"\((.*)\)");

//            Match match = r.Match(key);
//            if (!match.Success) return key;

//            key = Regex.Replace(key, r.ToString(), "()");

//            string strValues = match.Groups[1].Value;
//            string[] arrValues = strValues.Split(',');

//            Args = new List<string>();

//            foreach (var t in arrValues)
//                Args.Add(t);

//            return key;
//        }


//        private void ParseTypes(IList<string> lines)
//        {
//            for (int i = 0; i < lines.Count; i++)
//            {
//                if (!lines[i].ToUpper().StartsWith(TypeKey)) continue;

//                while (++i < lines.Count)
//                {
//                    if (String.IsNullOrEmpty(lines[i])) continue;

//                    int index;
//                    if (!int.TryParse(lines[i], out index)) return;
//                    Types.Add((ItemType)index);
//                }
//            }
//        }
//        private void ParseGoods(IList<string> lines)
//        {
//            for (int i = 0; i < lines.Count; i++)
//            {
//                if (!lines[i].ToUpper().StartsWith(TradeKey)) continue;

//                while (++i < lines.Count)
//                {
//                    if (lines[i].StartsWith("[")) return;
//                    if (String.IsNullOrEmpty(lines[i])) continue;

//                    var data = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

//                    ItemInfo info = SMain.Envir.GetItemInfo(data[0]);
//                    if (info == null)
//                        continue;
//                    UserItem goods = new UserItem(info) { CurrentDura = info.Durability, MaxDura = info.Durability };
//                    if (goods == null || Goods.Contains(goods))
//                    {
//                        SMain.Enqueue(string.Format("Could not find Item: {0}, File: {1}", lines[i], Info.FileName));
//                        continue;
//                    }
//                    uint count = 1;
//                    if (data.Length == 2)
//                        uint.TryParse(data[1], out count);
//                    goods.Count = count;
//                    goods.UniqueID = (ulong)i;

//                    Goods.Add(goods);
//                }
//            }
//        }
//        private void ParseQuests(IList<string> lines)
//        {
//            for (int i = 0; i < lines.Count; i++)
//            {
//                if (!lines[i].ToUpper().StartsWith(QuestKey)) continue;

//                while (++i < lines.Count)
//                {
//                    if (lines[i].StartsWith("[")) return;
//                    if (String.IsNullOrEmpty(lines[i])) continue;

//                    int index;

//                    int.TryParse(lines[i], out index);

//                    if (index == 0) continue;

//                    QuestInfo info = SMain.Envir.GetQuestInfo(Math.Abs(index));

//                    if (info == null) return;

//                    if (index > 0)
//                        info.NpcIndex = ObjectID;
//                    else
//                        info.FinishNpcIndex = ObjectID;

//                    if (Quests.All(x => x != info))
//                        Quests.Add(info);
//                }
//            }
//        }

//        public override void Process(DelayedAction action)
//        {
//            throw new NotSupportedException();
//        }

//        public override bool IsAttackTarget(PlayerObject attacker)
//        {
//            throw new NotSupportedException();
//        }
//        public override bool IsFriendlyTarget(PlayerObject ally)
//        {
//            throw new NotSupportedException();
//        }
//        public override bool IsFriendlyTarget(MonsterObject ally)
//        {
//            throw new NotSupportedException();
//        }
//        public override bool IsAttackTarget(MonsterObject attacker)
//        {
//            throw new NotSupportedException();
//        }

//        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
//        {
//            throw new NotSupportedException();
//        }

//        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
//        {
//            throw new NotSupportedException();
//        }

//        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
//        {
//            throw new NotSupportedException();
//        }

//        public override void SendHealth(PlayerObject player)
//        {
//            throw new NotSupportedException();
//        }

//        public override void Die()
//        {
//            throw new NotSupportedException();
//        }

//        public override int Pushed(MapObject pusher, MirDirection dir, int distance)
//        {
//            throw new NotSupportedException();
//        }

//        public override void ReceiveChat(string text, ChatType type)
//        {
//            throw new NotSupportedException();
//        }


//        public override void Process()
//        {
//            base.Process();

//            if (Envir.Time > TurnTime)
//            {
//                TurnTime = Envir.Time + TurnDelay;
//                Turn((MirDirection)Envir.Random.Next(3));
//            }

//            if (Envir.Time > UsedGoodsTime)
//            {
//                UsedGoodsTime = SMain.Envir.Time + (Settings.Minute * Settings.GoodsBuyBackTime);
//                ProcessGoods();
//            }

//            if (Envir.Time > VisTime)
//            {
//                VisTime = Envir.Time + (Settings.Minute);

//                if (Info.DayofWeek != "" && Info.DayofWeek != DateTime.Now.DayOfWeek.ToString())
//                {
//                    if (Visible) Hide();
//                }
//                else
//                {

//                    int StartTime = ((Info.HourStart * 60) + Info.MinuteStart);
//                    int FinishTime = ((Info.HourEnd * 60) + Info.MinuteEnd);
//                    int CurrentTime = ((DateTime.Now.Hour * 60) + DateTime.Now.Minute);

//                    if (Info.TimeVisible)
//                        if (StartTime > CurrentTime || FinishTime <= CurrentTime)
//                        {
//                            if (Visible) Hide();
//                        }
//                        else if (StartTime <= CurrentTime && FinishTime > CurrentTime)
//                        {
//                            if (!Visible) Show();
//                        }

//                }
//             }
//        }

//        public void ProcessGoods(bool clear = false)
//        {
//            if (!Settings.GoodsOn) return;

//            List<UserItem> deleteList = new List<UserItem>();

//            foreach (var playerGoods in BuyBack)
//            {
//                List<UserItem> items = playerGoods.Value;

//                for (int i = 0; i < items.Count; i++)
//                {
//                    UserItem item = items[i];

//                    if (DateTime.Compare(item.BuybackExpiryDate.AddMinutes(Settings.GoodsBuyBackTime), Envir.Now) <= 0 || clear)
//                    {
//                        deleteList.Add(BuyBack[playerGoods.Key][i]);

//                        if (UsedGoods.Count >= Settings.GoodsMaxStored)
//                        {
//                            UserItem nonAddedItem = UsedGoods.FirstOrDefault(e => e.IsAdded == false);

//                            if (nonAddedItem != null)
//                            {
//                                UsedGoods.Remove(nonAddedItem);
//                            }
//                            else
//                            {
//                                UsedGoods.RemoveAt(0);
//                            }
//                        }

//                        UsedGoods.Add(item);
//                        NeedSave = true;
//                    }
//                }

//                for (int i = 0; i < deleteList.Count; i++)
//                {
//                    BuyBack[playerGoods.Key].Remove(deleteList[i]);
//                }
//            }
//        }

//        public override void SetOperateTime()
//        {
//            long time = Envir.Time + 2000;

//            if (TurnTime < time && TurnTime > Envir.Time)
//                time = TurnTime;

//            if (OwnerTime < time && OwnerTime > Envir.Time)
//                time = OwnerTime;

//            if (ExpireTime < time && ExpireTime > Envir.Time)
//                time = ExpireTime;

//            if (PKPointTime < time && PKPointTime > Envir.Time)
//                time = PKPointTime;

//            if (LastHitTime < time && LastHitTime > Envir.Time)
//                time = LastHitTime;

//            if (EXPOwnerTime < time && EXPOwnerTime > Envir.Time)
//                time = EXPOwnerTime;

//            if (BrownTime < time && BrownTime > Envir.Time)
//                time = BrownTime;

//            for (int i = 0; i < ActionList.Count; i++)
//            {
//                if (ActionList[i].Time >= time && ActionList[i].Time > Envir.Time) continue;
//                time = ActionList[i].Time;
//            }

//            for (int i = 0; i < PoisonList.Count; i++)
//            {
//                if (PoisonList[i].TickTime >= time && PoisonList[i].TickTime > Envir.Time) continue;
//                time = PoisonList[i].TickTime;
//            }

//            for (int i = 0; i < Buffs.Count; i++)
//            {
//                if (Buffs[i].ExpireTime >= time && Buffs[i].ExpireTime > Envir.Time) continue;
//                time = Buffs[i].ExpireTime;
//            }


//            if (OperateTime <= Envir.Time || time < OperateTime)
//                OperateTime = time;
//        }

//        public void Turn(MirDirection dir)
//        {
//            Direction = dir;

//            Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
//        }

//        public void Hide()
//        {
//            CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation);
//            Visible = false;
//        }

//        public void Show()
//        {
//            Visible = true;
//            for (int i = CurrentMap.Players.Count - 1; i >= 0; i--)
//            {
//                PlayerObject player = CurrentMap.Players[i];

//                if (Functions.InRange(CurrentLocation, player.CurrentLocation, Globals.DataRange))
//                    CheckVisible(player, true);
//            }
//        }

//        public override Packet GetInfo()
//        {
//            return new S.ObjectNPC
//                {
//                    ObjectID = ObjectID,
//                    Name = Name,
//                    NameColour = NameColour,
//                    Image = Info.Image,
//                    Location = CurrentLocation,
//                    Direction = Direction,
//                    QuestIDs = (from q in Quests
//                                select q.Index).ToList()
//                };
//        }

//        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false)
//        {
//            throw new NotSupportedException();
//        }

//        public override void AddBuff(Buff b)
//        {
//            throw new NotSupportedException();
//        }

//        public void Call(PlayerObject player, string key)
//        {
//            key = key.ToUpper();

//            if (!player.NPCDelayed)
//            {
//                if (key != MainKey) // && ObjectID != player.DefaultNPC.ObjectID
//                {
//                    if (player.NPCID != ObjectID) return;

//                    if (player.NPCSuccess)
//                    {
//                        if (!player.NPCPage.Buttons.Any(c => c.ToUpper().Contains(key))) return;
//                    }
//                    else
//                    {
//                        if (!player.NPCPage.ElseButtons.Any(c => c.ToUpper().Contains(key))) return;
//                    }
//                }
//            }
//            else
//            {
//                player.NPCDelayed = false;
//            }

//            if (key.StartsWith("[@@") && player.NPCInputStr == string.Empty)
//            {
//                send off packet to request input
//                player.Enqueue(new S.NPCRequestInput { NPCID = ObjectID, PageName = key });
//                return;
//            }

//            for (int i = 0; i < NPCSections.Count; i++)
//            {
//                NPCPage page = NPCSections[i];

//                if (!String.Equals(page.Key, key, StringComparison.CurrentCultureIgnoreCase)) continue;
//                ProcessPage(player, page);
//            }


//            player.NPCInputStr = string.Empty;
//        }

//        public void Buy(PlayerObject player, ulong index, uint count)
//        {
//            UserItem goods = null;

//            for (int i = 0; i < Goods.Count; i++)
//            {
//                if (Goods[i].UniqueID != index) continue;
//                goods = Goods[i];
//                break;
//            }

//            bool isUsed = false;
//            if (goods == null)
//            {
//                for (int i = 0; i < UsedGoods.Count; i++)
//                {
//                    if (UsedGoods[i].UniqueID != index) continue;
//                    goods = UsedGoods[i];
//                    isUsed = true;
//                    break;
//                }
//            }

//            bool isBuyBack = false;
//            if (goods == null)
//            {
//                if (!BuyBack.ContainsKey(player.Name)) BuyBack[player.Name] = new List<UserItem>();
//                for (int i = 0; i < BuyBack[player.Name].Count; i++)
//                {
//                    if (BuyBack[player.Name][i].UniqueID != index) continue;
//                    goods = BuyBack[player.Name][i];
//                    isBuyBack = true;
//                    break;
//                }
//            }

//            if (goods == null || goods.Count == 0 || goods.Count > goods.Info.StackSize) return;

//            uint cost = goods.Price();
//            cost = (uint)(cost * Info.PriceRate);

//            if (player.NPCPage.Key.ToUpper() == PearlBuyKey)//pearl currency
//            {
//                if (cost > player.Info.PearlCount) return;
//            }
//            else if (cost > player.Account.Gold) return;

//            UserItem item = (isBuyBack || isUsed ? goods : Envir.CreateFreshItem(goods.Info));
//            item.Count = goods.Count;

//            if (!player.CanGainItem(item)) return;

//            if (player.NPCPage.Key.ToUpper() == PearlBuyKey)//pearl currency
//            {
//                player.Info.PearlCount -= (int)cost;
//            }
//            else
//            {
//                player.Account.Gold -= cost;
//                player.Enqueue(new S.LoseGold { Gold = cost });
//            }
//            player.GainItem(item);

//            player.Report.ItemChanged("BuyItem", item, item.Count, 2);

//            if (isUsed)
//            {
//                UsedGoods.Remove(goods);

//                NeedSave = true;

//                player.Enqueue(new S.NPCGoods { List = UsedGoods, Rate = Info.PriceRate });
//            }

//            if (isBuyBack)
//            {
//                BuyBack[player.Name].Remove(goods);
//                player.Enqueue(new S.NPCGoods { List = BuyBack[player.Name], Rate = Info.PriceRate });
//            }
//        }
//        public void Sell(PlayerObject player, UserItem item)
//        {
//            /* Handle Item Sale */
//            if (!BuyBack.ContainsKey(player.Name)) BuyBack[player.Name] = new List<UserItem>();

//            if (BuyBack[player.Name].Count >= Settings.GoodsBuyBackMaxStored)
//                BuyBack[player.Name].RemoveAt(0);

//            player.Report.ItemChanged("SellItem", item, item.Count, 1);

//            item.BuybackExpiryDate = Envir.Now;
//            BuyBack[player.Name].Add(item);
//        }

//        private void ProcessPage(PlayerObject player, NPCPage page)
//        {
//            player.NPCID = ObjectID;
//            player.NPCSuccess = page.Check(player);
//            player.NPCPage = page;

//            switch (page.Key.ToUpper())
//            {
//                case BuyKey:
//                    for (int i = 0; i < Goods.Count; i++)
//                        player.CheckItem(Goods[i]);

//                    player.Enqueue(new S.NPCGoods { List = Goods, Rate = Info.PriceRate });
//                    break;
//                case SellKey:
//                    player.Enqueue(new S.NPCSell());
//                    break;
//                case BuySellKey:
//                    for (int i = 0; i < Goods.Count; i++)
//                        player.CheckItem(Goods[i]);

//                    player.Enqueue(new S.NPCGoods { List = Goods, Rate = Info.PriceRate });
//                    player.Enqueue(new S.NPCSell());
//                    break;
//                case RepairKey:
//                    player.Enqueue(new S.NPCRepair { Rate = Info.PriceRate });
//                    break;
//                case SRepairKey:
//                    player.Enqueue(new S.NPCSRepair { Rate = Info.PriceRate });
//                    break;
//                case RefineKey:
//                    if (player.Info.CurrentRefine != null)
//                    {
//                        player.ReceiveChat("You're already refining an item.", ChatType.System);
//                        player.Enqueue(new S.NPCRefine { Rate = (Settings.RefineCost), Refining = true });
//                        break;
//                    }
//                    else
//                        player.Enqueue(new S.NPCRefine { Rate = (Settings.RefineCost), Refining = false });
//                    break;
//                case RefineCheckKey:
//                    player.Enqueue(new S.NPCCheckRefine());
//                    break;
//                case RefineCollectKey:
//                    player.CollectRefine();
//                    break;
//                case ReplaceWedRingKey:
//                    player.Enqueue(new S.NPCReplaceWedRing { Rate = Settings.ReplaceWedRingCost });
//                    break;
//                case StorageKey:
//                    player.SendStorage();
//                    player.Enqueue(new S.NPCStorage());
//                    break;
//                case BuyBackKey:
//                    if (!BuyBack.ContainsKey(player.Name)) BuyBack[player.Name] = new List<UserItem>();

//                    for (int i = 0; i < BuyBack[player.Name].Count; i++)
//                    {
//                        player.CheckItem(BuyBack[player.Name][i]);
//                    }

//                    player.Enqueue(new S.NPCGoods { List = BuyBack[player.Name], Rate = Info.PriceRate });
//                    break;
//                case BuyUsedKey:
//                    for (int i = 0; i < UsedGoods.Count; i++)
//                        player.CheckItem(UsedGoods[i]);

//                    player.Enqueue(new S.NPCGoods { List = UsedGoods, Rate = Info.PriceRate });
//                    break;
//                case ConsignKey:
//                    player.Enqueue(new S.NPCConsign());
//                    break;
//                case MarketKey:
//                    player.UserMatch = false;
//                    player.GetMarket(string.Empty, ItemType.Nothing);
//                    break;
//                case ConsignmentsKey:
//                    player.UserMatch = true;
//                    player.GetMarket(string.Empty, ItemType.Nothing);
//                    break;
//                case GuildCreateKey:
//                    if (player.Info.Level < Settings.Guild_RequiredLevel)
//                    {
//                        player.ReceiveChat(String.Format("You have to be at least level {0} to create a guild.", Settings.Guild_RequiredLevel), ChatType.System);
//                    }
//                    if (player.MyGuild == null)
//                    {
//                        player.CanCreateGuild = true;
//                        player.Enqueue(new S.GuildNameRequest());
//                    }
//                    else
//                        player.ReceiveChat("You are already part of a guild.", ChatType.System);
//                    break;
//                case RequestWarKey:
//                    if (player.MyGuild != null)
//                    {
//                        if (player.MyGuildRank != player.MyGuild.Ranks[0])
//                        {
//                            player.ReceiveChat("You must be the leader to request a war.", ChatType.System);
//                            return;
//                        }
//                        player.Enqueue(new S.GuildRequestWar());
//                    }
//                    else
//                    {
//                        player.ReceiveChat("You are not in a guild.", ChatType.System);
//                    }
//                    break;
//                case SendParcelKey:
//                    player.Enqueue(new S.MailSendRequest());
//                    break;
//                case CollectParcelKey:

//                    sbyte result = 0;

//                    if (player.GetMailAwaitingCollectionAmount() < 1)
//                    {
//                        result = -1;
//                    }
//                    else
//                    {
//                        foreach (var mail in player.Info.Mail)
//                        {
//                            if (mail.Parcel) mail.Collected = true;
//                        }
//                    }
//                    player.Enqueue(new S.ParcelCollected { Result = result });
//                    player.GetMail();
//                    break;
//                case AwakeningKey:
//                    player.Enqueue(new S.NPCAwakening());
//                    break;
//                case DisassembleKey:
//                    player.Enqueue(new S.NPCDisassemble());
//                    break;
//                case DowngradeKey:
//                    player.Enqueue(new S.NPCDowngrade());
//                    break;
//                case ResetKey:
//                    player.Enqueue(new S.NPCReset());
//                    break;
//                case PearlBuyKey://pearl currency
//                    for (int i = 0; i < Goods.Count; i++)
//                        player.CheckItem(Goods[i]);

//                    player.Enqueue(new S.NPCPearlGoods { List = Goods, Rate = Info.PriceRate });
//                    break;
//            }

//        }
//    }

//    public class NPCPage
//    {
//        public readonly string Key;
//        public List<NPCChecks> CheckList = new List<NPCChecks>();

//        public List<NPCActions> ActList = new List<NPCActions>(), ElseActList = new List<NPCActions>();
//        public List<string> Say, ElseSay, Buttons, ElseButtons, GotoButtons;

//        public string Param1;
//        public int Param1Instance, Param2, Param3;

//        public string NPCName;

//        public NPCPage(string key, string npcName, List<string> say, List<string> buttons, List<string> elseSay, List<string> elseButtons, List<string> gotoButtons)
//        {
//            Key = key;
//            NPCName = npcName;

//            Say = say;
//            Buttons = buttons;

//            ElseSay = elseSay;
//            ElseButtons = elseButtons;

//            GotoButtons = gotoButtons;
//        }


//        On server load
//        public void ParseCheck(string line)
//        {
//            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

//            parts = ParseArguments(parts);

//            if (parts.Length == 0) return;

//            string tempString, tempString2;

//            var regexFlag = new Regex(@"\[(.*?)\]");
//            var regexQuote = new Regex("\"([^\"]*)\"");

//            Match quoteMatch = null;

//            switch (parts[0].ToUpper())
//            {
//                case "LEVEL":
//                    if (parts.Length < 3) return;

//                    CheckList.Add(new NPCChecks(CheckType.Level, parts[1], parts[2]));
//                    break;

//                case "CHECKGOLD":
//                    if (parts.Length < 3) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckGold, parts[1], parts[2]));
//                    break;
//                case "CHECKCREDIT":
//                    if (parts.Length < 3) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckCredit, parts[1], parts[2]));
//                    break;
//                case "CHECKITEM":
//                    if (parts.Length < 2) return;

//                    tempString = parts.Length < 3 ? "1" : parts[2];
//                    tempString2 = parts.Length > 3 ? parts[3] : "";

//                    CheckList.Add(new NPCChecks(CheckType.CheckItem, parts[1], tempString, tempString2));
//                    break;

//                case "CHECKGENDER":
//                    if (parts.Length < 2) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckGender, parts[1]));
//                    break;

//                case "CHECKCLASS":
//                    if (parts.Length < 2) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckClass, parts[1]));
//                    break;

//                case "DAYOFWEEK":
//                    if (parts.Length < 2) return;
//                    CheckList.Add(new NPCChecks(CheckType.CheckDay, parts[1]));
//                    break;

//                case "HOUR":
//                    if (parts.Length < 2) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckHour, parts[1]));
//                    break;

//                case "MIN":
//                    if (parts.Length < 2) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckMinute, parts[1]));
//                    break;

//                cant use stored var
//                case "CHECKNAMELIST":
//                    if (parts.Length < 2) return;

//                    quoteMatch = regexQuote.Match(line);

//                    string listPath = parts[1];

//                    if (quoteMatch.Success)
//                        listPath = quoteMatch.Groups[1].Captures[0].Value;

//                    var fileName = Settings.NameListPath + listPath;

//                    string sDirectory = Path.GetDirectoryName(fileName);
//                    Directory.CreateDirectory(sDirectory);

//                    if (File.Exists(fileName))
//                        CheckList.Add(new NPCChecks(CheckType.CheckNameList, fileName));
//                    break;

//                case "ISADMIN":
//                    CheckList.Add(new NPCChecks(CheckType.IsAdmin));
//                    break;

//                case "CHECKPKPOINT":
//                    if (parts.Length < 3) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckPkPoint, parts[1], parts[2]));
//                    break;

//                case "CHECKRANGE":
//                    if (parts.Length < 4) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckRange, parts[1], parts[2], parts[3]));
//                    break;

//                case "CHECKMAP":
//                    if (parts.Length < 2) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckMap, parts[1]));
//                    break;

//                cant use stored var
//                case "CHECK":
//                    if (parts.Length < 3) return;
//                    var match = regexFlag.Match(parts[1]);
//                    if (match.Success)
//                    {
//                        string flagIndex = match.Groups[1].Captures[0].Value;
//                        CheckList.Add(new NPCChecks(CheckType.Check, flagIndex, parts[2]));
//                    }
//                    break;

//                case "CHECKHUM":
//                    if (parts.Length < 4) return;

//                    tempString = parts.Length < 5 ? "1" : parts[4];
//                    CheckList.Add(new NPCChecks(CheckType.CheckHum, parts[1], parts[2], parts[3], tempString));
//                    break;

//                case "CHECKMON":
//                    if (parts.Length < 4) return;

//                    tempString = parts.Length < 5 ? "1" : parts[4];
//                    CheckList.Add(new NPCChecks(CheckType.CheckMon, parts[1], parts[2], parts[3], tempString));
//                    break;

//                case "CHECKEXACTMON":
//                    if (parts.Length < 5) return;

//                    tempString = parts.Length < 6 ? "1" : parts[5];
//                    CheckList.Add(new NPCChecks(CheckType.CheckExactMon, parts[1], parts[2], parts[3], parts[4], tempString));
//                    break;

//                case "RANDOM":
//                    if (parts.Length < 2) return;

//                    CheckList.Add(new NPCChecks(CheckType.Random, parts[1]));
//                    break;

//                case "GROUPLEADER":
//                    CheckList.Add(new NPCChecks(CheckType.Groupleader));
//                    break;

//                case "GROUPCOUNT":
//                    if (parts.Length < 3) return;

//                    CheckList.Add(new NPCChecks(CheckType.GroupCount, parts[1], parts[2]));
//                    break;

//                case "PETCOUNT":
//                    if (parts.Length < 3) return;

//                    CheckList.Add(new NPCChecks(CheckType.PetCount, parts[1], parts[2]));
//                    break;

//                case "PETLEVEL":
//                    if (parts.Length < 3) return;

//                    CheckList.Add(new NPCChecks(CheckType.PetLevel, parts[1], parts[2]));
//                    break;

//                case "CHECKCALC":
//                    if (parts.Length < 4) return;
//                    CheckList.Add(new NPCChecks(CheckType.CheckCalc, parts[1], parts[2], parts[3]));
//                    break;

//                case "INGUILD":
//                    string guildName = string.Empty;

//                    if (parts.Length > 1) guildName = parts[1];

//                    CheckList.Add(new NPCChecks(CheckType.InGuild, guildName));
//                    break;

//                case "CHECKQUEST":
//                    if (parts.Length < 3) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckQuest, parts[1], parts[2]));
//                    break;
//                case "CHECKRELATIONSHIP":
//                    CheckList.Add(new NPCChecks(CheckType.CheckRelationship));
//                    break;
//                case "CHECKWEDDINGRING":
//                    CheckList.Add(new NPCChecks(CheckType.CheckWeddingRing));
//                    break;

//                case "CHECKPET":
//                    if (parts.Length < 2) return;

//                    CheckList.Add(new NPCChecks(CheckType.CheckPet, parts[1]));
//                    break;
//            }

//        }
//        public void ParseAct(List<NPCActions> acts, string line)
//        {
//            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

//            parts = ParseArguments(parts);

//            if (parts.Length == 0) return;

//            string fileName;
//            var regexQuote = new Regex("\"([^\"]*)\"");
//            var regexFlag = new Regex(@"\[(.*?)\]");

//            Match quoteMatch = null;

//            switch (parts[0].ToUpper())
//            {
//                case "MOVE":
//                    if (parts.Length < 2) return;

//                    string tempx = parts.Length > 3 ? parts[2] : "0";
//                    string tempy = parts.Length > 3 ? parts[3] : "0";

//                    acts.Add(new NPCActions(ActionType.Move, parts[1], tempx, tempy));
//                    break;

//                case "INSTANCEMOVE":
//                    if (parts.Length < 5) return;

//                    acts.Add(new NPCActions(ActionType.InstanceMove, parts[1], parts[2], parts[3], parts[4]));
//                    break;

//                case "GIVEGOLD":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.GiveGold, parts[1]));
//                    break;

//                case "TAKEGOLD":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.TakeGold, parts[1]));
//                    break;
//                case "GIVECREDIT":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.GiveCredit, parts[1]));
//                    break;
//                case "TAKECREDIT":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.TakeCredit, parts[1]));
//                    break;

//                case "GIVEPEARLS":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.GivePearls, parts[1]));
//                    break;

//                case "TAKEPEARLS":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.TakePearls, parts[1]));
//                    break;

//                case "GIVEITEM":
//                    if (parts.Length < 2) return;

//                    string count = parts.Length < 3 ? string.Empty : parts[2];
//                    acts.Add(new NPCActions(ActionType.GiveItem, parts[1], count));
//                    break;

//                case "TAKEITEM":
//                    if (parts.Length < 3) return;

//                    count = parts.Length < 3 ? string.Empty : parts[2];
//                    string dura = parts.Length > 3 ? parts[3] : "";

//                    acts.Add(new NPCActions(ActionType.TakeItem, parts[1], count, dura));
//                    break;

//                case "GIVEEXP":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.GiveExp, parts[1]));
//                    break;

//                case "GIVEPET":
//                    if (parts.Length < 2) return;

//                    string petcount = parts.Length > 2 ? parts[2] : "1";
//                    string petlevel = parts.Length > 3 ? parts[3] : "0";

//                    acts.Add(new NPCActions(ActionType.GivePet, parts[1], petcount, petlevel));
//                    break;
//                case "REMOVEPET":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.RemovePet, parts[1]));
//                    break;
//                case "CLEARPETS":
//                    acts.Add(new NPCActions(ActionType.ClearPets));
//                    break;

//                case "GOTO":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.Goto, parts[1]));
//                    break;

//                cant use stored var
//                case "ADDNAMELIST":
//                    if (parts.Length < 2) return;

//                    quoteMatch = regexQuote.Match(line);

//                    string listPath = parts[1];

//                    if (quoteMatch.Success)
//                        listPath = quoteMatch.Groups[1].Captures[0].Value;

//                    fileName = Settings.NameListPath + listPath;

//                    string sDirectory = Path.GetDirectoryName(fileName);
//                    Directory.CreateDirectory(sDirectory);

//                    if (!File.Exists(fileName))
//                        File.Create(fileName).Close();

//                    acts.Add(new NPCActions(ActionType.AddNameList, fileName));
//                    break;

//                cant use stored var
//                case "DELNAMELIST":
//                    if (parts.Length < 2) return;

//                    quoteMatch = regexQuote.Match(line);

//                    listPath = parts[1];

//                    if (quoteMatch.Success)
//                        listPath = quoteMatch.Groups[1].Captures[0].Value;

//                    fileName = Settings.NameListPath + listPath;

//                    sDirectory = Path.GetDirectoryName(fileName);
//                    Directory.CreateDirectory(sDirectory);

//                    if (File.Exists(fileName))
//                        acts.Add(new NPCActions(ActionType.DelNameList, fileName));
//                    break;

//                cant use stored var
//                case "CLEARNAMELIST":
//                    if (parts.Length < 2) return;
                    
//                    quoteMatch = regexQuote.Match(line);

//                    listPath = parts[1];

//                    if (quoteMatch.Success)
//                        listPath = quoteMatch.Groups[1].Captures[0].Value;

//                    fileName = Settings.NameListPath + listPath;

//                    sDirectory = Path.GetDirectoryName(fileName);
//                    Directory.CreateDirectory(sDirectory);

//                    if (File.Exists(fileName))
//                        acts.Add(new NPCActions(ActionType.ClearNameList, fileName));
//                    break;

//                case "GIVEHP":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.GiveHP, parts[1]));
//                    break;

//                case "GIVEMP":
//                    if (parts.Length < 2) return;
//                    acts.Add(new NPCActions(ActionType.GiveMP, parts[1]));
//                    break;

//                case "CHANGELEVEL":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.ChangeLevel, parts[1]));
//                    break;

//                case "SETPKPOINT":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.SetPkPoint, parts[1]));
//                    break;

//                case "REDUCEPKPOINT":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.ReducePkPoint, parts[1]));
//                    break;

//                case "INCREASEPKPOINT":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.IncreasePkPoint, parts[1]));
//                    break;

//                case "CHANGEGENDER":
//                    acts.Add(new NPCActions(ActionType.ChangeGender));
//                    break;

//                case "CHANGECLASS":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.ChangeClass, parts[1]));
//                    break;

//                case "CHANGEHAIR":
//                    if (parts.Length < 2)
//                    {
//                        acts.Add(new NPCActions(ActionType.ChangeHair));
//                    }
//                    else
//                    {
//                        acts.Add(new NPCActions(ActionType.ChangeHair, parts[1]));
//                    }
//                    break;

//                case "LOCALMESSAGE":
//                    var match = regexQuote.Match(line);
//                    if (match.Success)
//                    {
//                        var message = match.Groups[1].Captures[0].Value;

//                        var last = parts.Count() - 1;
//                        acts.Add(new NPCActions(ActionType.LocalMessage, message, parts[last]));
//                    }
//                    break;

//                case "GLOBALMESSAGE":
//                    match = regexQuote.Match(line);
//                    if (match.Success)
//                    {
//                        var message = match.Groups[1].Captures[0].Value;

//                        var last = parts.Count() - 1;
//                        acts.Add(new NPCActions(ActionType.GlobalMessage, message, parts[last]));
//                    }
//                    break;

//                case "GIVESKILL":
//                    if (parts.Length < 3) return;

//                    string spelllevel = parts.Length > 2 ? parts[2] : "0";
//                    acts.Add(new NPCActions(ActionType.GiveSkill, parts[1], spelllevel));
//                    break;

//                cant use stored var
//                case "SET":
//                    if (parts.Length < 3) return;
//                    match = regexFlag.Match(parts[1]);
//                    if (match.Success)
//                    {
//                        string flagIndex = match.Groups[1].Captures[0].Value;
//                        acts.Add(new NPCActions(ActionType.Set, flagIndex, parts[2]));
//                    }
//                    break;

//                case "PARAM1":
//                    if (parts.Length < 2) return;

//                    string instanceId = parts.Length < 3 ? "1" : parts[2];
//                    acts.Add(new NPCActions(ActionType.Param1, parts[1], instanceId));
//                    break;

//                case "PARAM2":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.Param2, parts[1]));
//                    break;

//                case "PARAM3":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.Param3, parts[1]));
//                    break;

//                case "MONGEN":
//                    if (parts.Length < 2) return;

//                    count = parts.Length < 3 ? "1" : parts[2];
//                    acts.Add(new NPCActions(ActionType.Mongen, parts[1], count));
//                    break;

//                case "TIMERECALL":
//                    if (parts.Length < 2) return;

//                    string page = parts.Length > 2 ? parts[2] : "";

//                    acts.Add(new NPCActions(ActionType.TimeRecall, parts[1], page));
//                    break;

//                case "TIMERECALLGROUP":
//                    if (parts.Length < 2) return;

//                    page = parts.Length > 2 ? parts[2] : "";

//                    acts.Add(new NPCActions(ActionType.TimeRecallGroup, parts[1], page));
//                    break;

//                case "BREAKTIMERECALL":
//                    acts.Add(new NPCActions(ActionType.BreakTimeRecall));
//                    break;

//                case "DELAYGOTO":
//                    if (parts.Length < 3) return;

//                    acts.Add(new NPCActions(ActionType.DelayGoto, parts[1], parts[2]));
//                    break;

//                case "MONCLEAR":
//                    if (parts.Length < 2) return;

//                    instanceId = parts.Length < 3 ? "1" : parts[2];
//                    acts.Add(new NPCActions(ActionType.MonClear, parts[1], instanceId));
//                    break;

//                case "GROUPRECALL":
//                    acts.Add(new NPCActions(ActionType.GroupRecall));
//                    break;

//                case "GROUPTELEPORT":
//                    if (parts.Length < 2) return;
//                    string x;
//                    string y;

//                    if (parts.Length == 4)
//                    {
//                        instanceId = "1";
//                        x = parts[2];
//                        y = parts[3];
//                    }
//                    else
//                    {
//                        instanceId = parts.Length < 3 ? "1" : parts[2];
//                        x = parts.Length < 4 ? "0" : parts[3];
//                        y = parts.Length < 5 ? "0" : parts[4];
//                    }

//                    acts.Add(new NPCActions(ActionType.GroupTeleport, parts[1], instanceId, x, y));
//                    break;

//                case "MOV":
//                    if (parts.Length < 3) return;
//                    match = Regex.Match(parts[1], @"[A-Z][0-9]", RegexOptions.IgnoreCase);

//                    quoteMatch = regexQuote.Match(line);

//                    string valueToStore = parts[2];

//                    if (quoteMatch.Success)
//                        valueToStore = quoteMatch.Groups[1].Captures[0].Value;

//                    if (match.Success)
//                        acts.Add(new NPCActions(ActionType.Mov, parts[1], valueToStore));

//                    break;
//                case "CALC":
//                    if (parts.Length < 4) return;

//                    match = Regex.Match(parts[1], @"[A-Z][0-9]", RegexOptions.IgnoreCase);

//                    quoteMatch = regexQuote.Match(line);

//                    valueToStore = parts[3];

//                    if (quoteMatch.Success)
//                        valueToStore = quoteMatch.Groups[1].Captures[0].Value;

//                    if (match.Success)
//                        acts.Add(new NPCActions(ActionType.Calc, "%" + parts[1], parts[2], valueToStore, parts[1].Insert(1, "-")));

//                    break;
//                case "GIVEBUFF":
//                    if (parts.Length < 4) return;

//                    string visible = "";
//                    string infinite = "";

//                    if (parts.Length > 4) visible = parts[4];
//                    if (parts.Length > 5) infinite = parts[5];

//                    acts.Add(new NPCActions(ActionType.GiveBuff, parts[1], parts[2], parts[3], visible, infinite));
//                    break;

//                case "REMOVEBUFF":
//                    if (parts.Length < 2) return;

//                    acts.Add(new NPCActions(ActionType.RemoveBuff, parts[1]));
//                    break;

//                case "ADDTOGUILD":
//                    if (parts.Length < 2) return;
//                    acts.Add(new NPCActions(ActionType.AddToGuild, parts[1]));
//                    break;

//                case "REMOVEFROMGUILD":
//                    if (parts.Length < 2) return;
//                    acts.Add(new NPCActions(ActionType.RemoveFromGuild, parts[1]));
//                    break;

//                case "REFRESHEFFECTS":
//                    acts.Add(new NPCActions(ActionType.RefreshEffects));
//                    break;

//                case "CANGAINEXP":
//                    if (parts.Length < 2) return;
//                    acts.Add(new NPCActions(ActionType.CanGainExp, parts[1]));
//                    break;

//                case "COMPOSEMAIL":
//                    match = regexQuote.Match(line);
//                    if (match.Success)
//                    {
//                        var message = match.Groups[1].Captures[0].Value;

//                        var last = parts.Count() - 1;
//                        acts.Add(new NPCActions(ActionType.ComposeMail, message, parts[last]));
//                    }
//                    break;

//                case "ADDMAILGOLD":
//                    if (parts.Length < 2) return;
//                    acts.Add(new NPCActions(ActionType.AddMailGold, parts[1]));
//                    break;

//                case "ADDMAILITEM":
//                    if (parts.Length < 3) return;
//                    acts.Add(new NPCActions(ActionType.AddMailItem, parts[1], parts[2]));
//                    break;

//                case "SENDMAIL":
//                    acts.Add(new NPCActions(ActionType.SendMail));
//                    break;

//                case "GROUPGOTO":
//                    if (parts.Length < 2) return;
//                    acts.Add(new NPCActions(ActionType.GroupGoto, parts[1]));
//                    break;

//                case "ENTERMAP":
//                    acts.Add(new NPCActions(ActionType.EnterMap));
//                    break;
//                case "MAKEWEDDINGRING":
//                    acts.Add(new NPCActions(ActionType.MakeWeddingRing));
//                    break;
//                case "FORCEDIVORCE":
//                    acts.Add(new NPCActions(ActionType.ForceDivorce));
//                    break;

//                case "LOADVALUE":
//                    if (parts.Length < 5) return;

//                    quoteMatch = regexQuote.Match(line);

//                    if (quoteMatch.Success)
//                    {
//                        fileName = Settings.ValuePath + quoteMatch.Groups[1].Captures[0].Value;

//                        string group = parts[parts.Length - 2];
//                        string key = parts[parts.Length - 1];

//                        sDirectory = Path.GetDirectoryName(fileName);
//                        Directory.CreateDirectory(sDirectory);

//                        if (!File.Exists(fileName))
//                            File.Create(fileName).Close();

//                        acts.Add(new NPCActions(ActionType.LoadValue, parts[1], fileName, group, key));
//                    }
//                    break;

//                case "SAVEVALUE":
//                    if (parts.Length < 5) return;

//                    MatchCollection matchCol = regexQuote.Matches(line);

//                    if (matchCol.Count > 0 && matchCol[0].Success)
//                    {
//                        fileName = Settings.ValuePath + matchCol[0].Groups[1].Captures[0].Value;

//                        string value = parts[parts.Length - 1];

//                        if (matchCol.Count > 1 && matchCol[1].Success)
//                            value = matchCol[1].Groups[1].Captures[0].Value;

//                        string[] newParts = line.Replace(value, string.Empty).Replace("\"", "").Trim().Split(' ');

//                        string group = newParts[newParts.Length - 2];
//                        string key = newParts[newParts.Length - 1];

//                        sDirectory = Path.GetDirectoryName(fileName);
//                        Directory.CreateDirectory(sDirectory);

//                        if (!File.Exists(fileName))
//                            File.Create(fileName).Close();

//                        acts.Add(new NPCActions(ActionType.SaveValue, fileName, group, key, value));
//                    }
//                    break;
//            }

//        }

//        public string[] ParseArguments(string[] words)
//        {
//            Regex r = new Regex(@"\%ARG\((\d+)\)$");

//            for (int i = 0; i < words.Length; i++)
//            {
//                Match match = r.Match(words[i].ToUpper());

//                if (!match.Success) continue;

//                int sequence = Convert.ToInt32(match.Groups[1].Value);

//                if (NPCObject.Args.Count >= (sequence + 1)) words[i] = NPCObject.Args[sequence];
//            }

//            return words;
//        }


//        On Demand
//        public bool Check(PlayerObject player)
//        {
//            var failed = false;

//            for (int i = 0; i < CheckList.Count; i++)
//            {
//                NPCChecks check = CheckList[i];
//                List<string> param = check.Params.Select(t => FindVariable(player, t)).ToList();

//                for (int j = 0; j < param.Count; j++)
//                {
//                    var parts = param[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

//                    if (parts.Length == 0) continue;

//                    foreach (var part in parts)
//                    {
//                        param[j] = param[j].Replace(part, ReplaceValue(player, part));
//                    }
//                }

//                byte tempByte;
//                uint tempUint;
//                int tempInt;
//                int tempInt2;

//                switch (check.Type)
//                {
//                    case CheckType.Level:
//                        if (!byte.TryParse(param[1], out tempByte))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        try
//                        {
//                            failed = !Compare(param[0], player.Level, tempByte);
//                        }
//                        catch (ArgumentException)
//                        {
//                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
//                            return true;
//                        }
//                        break;

//                    case CheckType.CheckGold:
//                        if (!uint.TryParse(param[1], out tempUint))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        try
//                        {
//                            failed = !Compare(param[0], player.Account.Gold, tempUint);
//                        }
//                        catch (ArgumentException)
//                        {
//                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
//                            return true;
//                        }
//                        break;
//                    case CheckType.CheckCredit:
//                        if (!uint.TryParse(param[1], out tempUint))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        try
//                        {
//                            failed = !Compare(param[0], player.Account.Credit, tempUint);
//                        }
//                        catch (ArgumentException)
//                        {
//                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
//                            return true;
//                        }
//                        break;

//                    case CheckType.CheckItem:
//                        uint count;
//                        ushort dura;

//                        if (!uint.TryParse(param[1], out count))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        bool checkDura = ushort.TryParse(param[2], out dura);

//                        var info = SMain.Envir.GetItemInfo(param[0]);

//                        foreach (var item in player.Info.Inventory.Where(item => item != null && item.Info == info))
//                        {
//                            if (checkDura)
//                                if (item.CurrentDura < (dura * 1000)) continue;

//                            if (count > item.Count)
//                            {
//                                count -= item.Count;
//                                continue;
//                            }

//                            if (count > item.Count) continue;
//                            count = 0;
//                            break;
//                        }
//                        if (count > 0)
//                            failed = true;
//                        break;

//                    case CheckType.CheckGender:
//                        MirGender gender;

//                        if (!MirGender.TryParse(param[0], false, out gender))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        failed = player.Gender != gender;
//                        break;

//                    case CheckType.CheckClass:
//                        MirClass mirClass;

//                        if (!MirClass.TryParse(param[0], true, out mirClass))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        failed = player.Class != mirClass;
//                        break;

//                    case CheckType.CheckDay:
//                        var day = DateTime.Now.DayOfWeek.ToString().ToUpper();
//                        var dayToCheck = param[0].ToUpper();

//                        failed = day != dayToCheck;
//                        break;

//                    case CheckType.CheckHour:
//                        if (!uint.TryParse(param[0], out tempUint))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        var hour = DateTime.Now.Hour;
//                        var hourToCheck = tempUint;

//                        failed = hour != hourToCheck;
//                        break;

//                    case CheckType.CheckMinute:
//                        if (!uint.TryParse(param[0], out tempUint))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        var minute = DateTime.Now.Minute;
//                        var minuteToCheck = tempUint;

//                        failed = minute != minuteToCheck;
//                        break;

//                    case CheckType.CheckNameList:
//                        if (!File.Exists(param[0])) return true;

//                        var read = File.ReadAllLines(param[0]);
//                        failed = !read.Contains(player.Name);
//                        break;

//                    case CheckType.IsAdmin:
//                        failed = !player.IsGM;
//                        break;

//                    case CheckType.CheckPkPoint:
//                        if (!int.TryParse(param[1], out tempInt))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        try
//                        {
//                            failed = !Compare(param[0], player.PKPoints, tempInt);
//                        }
//                        catch (ArgumentException)
//                        {
//                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
//                            return true;
//                        }
//                        break;

//                    case CheckType.CheckRange:
//                        int x, y, range;
//                        if (!int.TryParse(param[0], out x) || !int.TryParse(param[1], out y) || !int.TryParse(param[2], out range))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        var target = new Point { X = x, Y = y };

//                        failed = !Functions.InRange(player.CurrentLocation, target, range);
//                        break;

//                    case CheckType.CheckMap:
//                        Map map = SMain.Envir.GetMapByNameAndInstance(param[0]);

//                        failed = player.CurrentMap != map;
//                        break;

//                    case CheckType.Check:
//                        uint onCheck;

//                        if (!uint.TryParse(param[0], out tempUint) || !uint.TryParse(param[1], out onCheck) || tempUint > Globals.FlagIndexCount)
//                        {
//                            failed = true;
//                            break;
//                        }

//                        bool tempBool = Convert.ToBoolean(onCheck);

//                        bool flag = player.Info.Flags[tempUint];

//                        failed = flag != tempBool;
//                        break;

//                    case CheckType.CheckHum:
//                        if (!int.TryParse(param[1], out tempInt) || !int.TryParse(param[3], out tempInt2))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        map = SMain.Envir.GetMapByNameAndInstance(param[2], tempInt2);
//                        if (map == null)
//                        {
//                            failed = true;
//                            break;
//                        }

//                        failed = !Compare(param[0], map.Players.Count(), tempInt);

//                        break;

//                    case CheckType.CheckMon:
//                        if (!int.TryParse(param[1], out tempInt) || !int.TryParse(param[3], out tempInt2))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        map = SMain.Envir.GetMapByNameAndInstance(param[2], tempInt2);
//                        if (map == null)
//                        {
//                            failed = true;
//                            break;
//                        }

//                        failed = !Compare(param[0], map.MonsterCount, tempInt);

//                        break;

//                    case CheckType.CheckExactMon:
//                        if (SMain.Envir.GetMonsterInfo(param[0]) == null)
//                        {
//                            failed = true;
//                            break;
//                        }

//                        if (!int.TryParse(param[2], out tempInt) || !int.TryParse(param[4], out tempInt2))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        map = SMain.Envir.GetMapByNameAndInstance(param[3], tempInt2);
//                        if (map == null)
//                        {
//                            failed = true;
//                            break;
//                        }

//                        failed = (!Compare(param[1], SMain.Envir.Objects.Count((
//                            d => d.CurrentMap == map &&
//                                d.Race == ObjectType.Monster &&
//                                string.Equals(d.Name.Replace(" ", ""), param[0], StringComparison.OrdinalIgnoreCase) &&
//                                !d.Dead)), tempInt));

//                        break;

//                    case CheckType.Random:
//                        if (!int.TryParse(param[0], out tempInt))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        failed = 0 != SMain.Envir.Random.Next(0, tempInt);
//                        break;

//                    case CheckType.Groupleader:
//                        failed = (player.GroupMembers == null || player.GroupMembers[0] != player);
//                        break;

//                    case CheckType.GroupCount:
//                        if (!int.TryParse(param[1], out tempInt))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        failed = (player.GroupMembers == null || !Compare(param[0], player.GroupMembers.Count, tempInt));
//                        break;

//                    case CheckType.PetCount:
//                        if (!int.TryParse(param[1], out tempInt))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        failed = !Compare(param[0], player.Pets.Count(), tempInt);
//                        break;

//                    case CheckType.PetLevel:
//                        if (!int.TryParse(param[1], out tempInt))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        for (int p = 0; p < player.Pets.Count(); p++)
//                        {
//                            failed = !Compare(param[0], player.Pets[p].PetLevel, tempInt);
//                        }
//                        break;

//                    case CheckType.CheckCalc:
//                        int left;
//                        int right;

//                        if (!int.TryParse(param[0], out left) || !int.TryParse(param[2], out right))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        try
//                        {
//                            failed = !Compare(param[1], left, right);
//                        }
//                        catch (ArgumentException)
//                        {
//                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[1], Key));
//                            return true;
//                        }
//                        break;
//                    case CheckType.InGuild:
//                        if (param[0].Length > 0)
//                        {
//                            failed = player.MyGuild == null || player.MyGuild.Name != param[0];
//                            break;
//                        }

//                        failed = player.MyGuild == null;
//                        break;

//                    case CheckType.CheckQuest:
//                        if (!int.TryParse(param[0], out tempInt))
//                        {
//                            failed = true;
//                            break;
//                        }

//                        string tempString = param[1].ToUpper();

//                        if (tempString == "ACTIVE")
//                        {
//                            failed = !player.CurrentQuests.Any(e => e.Index == tempInt);
//                        }
//                        else //COMPLETE
//                        {
//                            failed = !player.CompletedQuests.Contains(tempInt);
//                        }
//                        break;
//                    case CheckType.CheckRelationship:
//                        if (player.Info.Married == 0)
//                        {
//                            failed = true;
//                        }
//                        break;
//                    case CheckType.CheckWeddingRing:
//                        if ((player.Info.Equipment[(int)EquipmentSlot.RingL] == null) || (player.Info.Equipment[(int)EquipmentSlot.RingL].WeddingRing != -1))
//                        {
//                            failed = true;
//                        }
//                        break;
//                    case CheckType.CheckPet:

//                        bool petMatch = false;
//                        for (int c = player.Pets.Count - 1; c >= 0; c--)
//                        {
//                            if (string.Compare(player.Pets[c].Name, param[0], true) != 0) continue;

//                            petMatch = true;
//                        }

//                        failed = !petMatch;
//                        break;
//                }

//                if (!failed) continue;

//                Failed(player);
//                return false;
//            }

//            Success(player);
//            return true;

//        }
//        private void Act(IList<NPCActions> acts, PlayerObject player)
//        {
//            MailInfo mailInfo = null;

//            for (var i = 0; i < acts.Count; i++)
//            {
//                uint gold;
//                uint credit;
//                uint Pearls;
//                uint count;
//                ushort tempuShort;
//                string tempString = string.Empty;
//                int x, y;
//                int tempInt;
//                byte tempByte;
//                long tempLong;
//                bool tempBool;
//                Packet p;

//                ItemInfo info;
//                MonsterInfo monInfo;

//                NPCActions act = acts[i];
//                List<string> param = act.Params.Select(t => FindVariable(player, t)).ToList();

//                for (int j = 0; j < param.Count; j++)
//                {
//                    var parts = param[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

//                    if (parts.Length == 0) continue;

//                    foreach (var part in parts)
//                    {
//                        param[j] = param[j].Replace(part, ReplaceValue(player, part));
//                    }

//                    param[j] = param[j].Replace("%INPUTSTR", player.NPCInputStr);
//                }

//                switch (act.Type)
//                {
//                    case ActionType.Move:
//                        Map map = SMain.Envir.GetMapByNameAndInstance(param[0]);
//                        if (map == null) return;

//                        if (!int.TryParse(param[1], out x)) return;
//                        if (!int.TryParse(param[2], out y)) return;

//                        var coords = new Point(x, y);

//                        if (coords.X > 0 && coords.Y > 0) player.Teleport(map, coords);
//                        else player.TeleportRandom(200, 0, map);
//                        break;

//                    case ActionType.InstanceMove:
//                        int instanceId;
//                        if (!int.TryParse(param[1], out instanceId)) return;
//                        if (!int.TryParse(param[2], out x)) return;
//                        if (!int.TryParse(param[3], out y)) return;

//                        map = SMain.Envir.GetMapByNameAndInstance(param[0], instanceId);
//                        if (map == null) return;
//                        player.Teleport(map, new Point(x, y));
//                        break;

//                    case ActionType.GiveGold:
//                        if (!uint.TryParse(param[0], out gold)) return;

//                        if (gold + player.Account.Gold >= uint.MaxValue)
//                            gold = uint.MaxValue - player.Account.Gold;

//                        player.GainGold(gold);
//                        break;

//                    case ActionType.TakeGold:
//                        if (!uint.TryParse(param[0], out gold)) return;

//                        if (gold >= player.Account.Gold) gold = player.Account.Gold;

//                        player.Account.Gold -= gold;
//                        player.Enqueue(new S.LoseGold { Gold = gold });
//                        break;
//                    case ActionType.GiveCredit:
//                        if (!uint.TryParse(param[0], out credit)) return;

//                        if (credit + player.Account.Credit >= uint.MaxValue)
//                            credit = uint.MaxValue - player.Account.Credit;

//                        player.GainCredit(credit);
//                        break;

//                    case ActionType.TakeCredit:
//                        if (!uint.TryParse(param[0], out credit)) return;

//                        if (credit >= player.Account.Credit) credit = player.Account.Credit;

//                        player.Account.Credit -= credit;
//                        player.Enqueue(new S.LoseCredit { Credit = credit });
//                        break;

//                    case ActionType.GivePearls:
//                        if (!uint.TryParse(param[0], out Pearls)) return;

//                        if (Pearls + player.Info.PearlCount >= int.MaxValue)
//                            Pearls = (uint)(int.MaxValue - player.Info.PearlCount);

//                        player.IntelligentCreatureGainPearls((int)Pearls);
//                        break;

//                    case ActionType.TakePearls:
//                        if (!uint.TryParse(param[0], out Pearls)) return;

//                        if (Pearls >= player.Info.PearlCount) Pearls = (uint)player.Info.PearlCount;

//                        player.IntelligentCreatureLosePearls((int)Pearls);
//                        break;

//                    case ActionType.GiveItem:
//                        if (param.Count < 2 || !uint.TryParse(param[1], out count)) count = 1;

//                        info = SMain.Envir.GetItemInfo(param[0]);

//                        if (info == null)
//                        {
//                            SMain.Enqueue(string.Format("Failed to get ItemInfo: {0}, Page: {1}", param[0], Key));
//                            break;
//                        }

//                        while (count > 0)
//                        {
//                            UserItem item = SMain.Envir.CreateFreshItem(info);

//                            if (item == null)
//                            {
//                                SMain.Enqueue(string.Format("Failed to create UserItem: {0}, Page: {1}", param[0], Key));
//                                return;
//                            }

//                            if (item.Info.StackSize > count)
//                            {
//                                item.Count = count;
//                                count = 0;
//                            }
//                            else
//                            {
//                                count -= item.Info.StackSize;
//                                item.Count = item.Info.StackSize;
//                            }

//                            if (player.CanGainItem(item, false))
//                                player.GainItem(item);
//                        }
//                        break;

//                    case ActionType.TakeItem:
//                        if (param.Count < 2 || !uint.TryParse(param[1], out count)) count = 1;
//                        info = SMain.Envir.GetItemInfo(param[0]);

//                        ushort dura;
//                        bool checkDura = ushort.TryParse(param[2], out dura);

//                        if (info == null)
//                        {
//                            SMain.Enqueue(string.Format("Failed to get ItemInfo: {0}, Page: {1}", param[0], Key));
//                            break;
//                        }

//                        for (int j = 0; j < player.Info.Inventory.Length; j++)
//                        {
//                            UserItem item = player.Info.Inventory[j];
//                            if (item == null) continue;
//                            if (item.Info != info) continue;

//                            if (checkDura)
//                                if (item.CurrentDura < dura) continue;

//                            if (count > item.Count)
//                            {
//                                player.Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = item.Count });
//                                player.Info.Inventory[j] = null;

//                                count -= item.Count;
//                                continue;
//                            }

//                            player.Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = count });
//                            if (count == item.Count)
//                                player.Info.Inventory[j] = null;
//                            else
//                                item.Count -= count;
//                            break;
//                        }
//                        player.RefreshStats();
//                        break;

//                    case ActionType.GiveExp:
//                        uint tempUint;
//                        if (!uint.TryParse(param[0], out tempUint)) return;
//                        player.GainExp(tempUint);
//                        break;

//                    case ActionType.GivePet:
//                        byte petcount = 0;
//                        byte petlevel = 0;

//                        monInfo = SMain.Envir.GetMonsterInfo(param[0]);
//                        if (monInfo == null) return;

//                        if (param.Count > 1)
//                            petcount = byte.TryParse(param[1], out petcount) ? Math.Min((byte)5, petcount) : (byte)1;

//                        if (param.Count > 2)
//                            petlevel = byte.TryParse(param[2], out petlevel) ? Math.Min((byte)7, petlevel) : (byte)0;

//                        for (int j = 0; j < petcount; j++)
//                        {
//                            MonsterObject monster = MonsterObject.GetMonster(monInfo);
//                            if (monster == null) return;
//                            monster.PetLevel = petlevel;
//                            monster.Master = player;
//                            monster.MaxPetLevel = 7;
//                            monster.Direction = player.Direction;
//                            monster.ActionTime = SMain.Envir.Time + 1000;
//                            monster.Spawn(player.CurrentMap, player.CurrentLocation);
//                            player.Pets.Add(monster);
//                        }
//                        break;

//                    case ActionType.RemovePet:
//                        for (int c = player.Pets.Count - 1; c >= 0; c--)
//                        {
//                            if (string.Compare(player.Pets[c].Name, param[0], true) == 0) continue;

//                            player.Pets[c].Die();
//                        }
//                        break;

//                    case ActionType.ClearPets:
//                        for (int c = player.Pets.Count - 1; c >= 0; c--)
//                        {
//                            player.Pets[c].Die();
//                        }
//                        break;

//                    case ActionType.AddNameList:
//                        tempString = param[0];
//                        if (File.ReadAllLines(tempString).All(t => player.Name != t))
//                        {
//                            using (var line = File.AppendText(tempString))
//                            {
//                                line.WriteLine(player.Name);
//                            }
//                        }
//                        break;

//                    case ActionType.DelNameList:
//                        tempString = param[0];
//                        File.WriteAllLines(tempString, File.ReadLines(tempString).Where(l => l != player.Name).ToList());
//                        break;

//                    case ActionType.ClearNameList:
//                        tempString = param[0];
//                        File.WriteAllLines(tempString, new string[] { });
//                        break;

//                    case ActionType.GiveHP:
//                        if (!int.TryParse(param[0], out tempInt)) return;
//                        player.ChangeHP(tempInt);
//                        break;

//                    case ActionType.GiveMP:
//                        if (!int.TryParse(param[0], out tempInt)) return;
//                        player.ChangeMP(tempInt);
//                        break;

//                    case ActionType.ChangeLevel:
//                        if (!ushort.TryParse(param[0], out tempuShort)) return;
//                        tempuShort = Math.Min(ushort.MaxValue, tempuShort);

//                        player.Level = tempuShort;
//                        player.LevelUp();
//                        break;

//                    case ActionType.SetPkPoint:
//                        if (!int.TryParse(param[0], out tempInt)) return;
//                        player.PKPoints = tempInt;
//                        break;

//                    case ActionType.ReducePkPoint:

//                        if (!int.TryParse(param[0], out tempInt)) return;

//                        player.PKPoints -= tempInt;
//                        if (player.PKPoints < 0) player.PKPoints = 0;

//                        break;

//                    case ActionType.IncreasePkPoint:
//                        if (!int.TryParse(param[0], out tempInt)) return;
//                        player.PKPoints += tempInt;
//                        break;

//                    case ActionType.ChangeGender:

//                        switch (player.Info.Gender)
//                        {
//                            case MirGender.Male:
//                                player.Info.Gender = MirGender.Female;
//                                break;
//                            case MirGender.Female:
//                                player.Info.Gender = MirGender.Male;
//                                break;
//                        }
//                        break;

//                    case ActionType.ChangeHair:

//                        if (param.Count < 1)
//                        {
//                            player.Info.Hair = (byte)SMain.Envir.Random.Next(0, 9);
//                        }
//                        else
//                        {
//                            byte.TryParse(param[0], out tempByte);

//                            if (tempByte >= 0 && tempByte <= 9)
//                            {
//                                player.Info.Hair = tempByte;
//                            }
//                        }
//                        break;

//                    case ActionType.ChangeClass:

//                        MirClass mirClass;
//                        if (!Enum.TryParse(param[0], true, out mirClass)) return;

//                        switch (mirClass)
//                        {
//                            case MirClass.Warrior:
//                                player.Info.Class = MirClass.Warrior;
//                                break;
//                            case MirClass.Taoist:
//                                player.Info.Class = MirClass.Taoist;
//                                break;
//                            case MirClass.Wizard:
//                                player.Info.Class = MirClass.Wizard;
//                                break;
//                            case MirClass.Assassin:
//                                player.Info.Class = MirClass.Assassin;
//                                break;
//                            case MirClass.Archer:
//                                player.Info.Class = MirClass.Archer;
//                                break;
//                        }
//                        break;

//                    case ActionType.LocalMessage:
//                        ChatType chatType;
//                        if (!Enum.TryParse(param[1], true, out chatType)) return;
//                        player.ReceiveChat(param[0], chatType);
//                        break;

//                    case ActionType.GlobalMessage:
//                        if (!Enum.TryParse(param[1], true, out chatType)) return;

//                        p = new S.Chat { Message = param[0], Type = chatType };
//                        SMain.Envir.Broadcast(p);
//                        break;

//                    case ActionType.GiveSkill:
//                        byte spellLevel = 0;

//                        Spell skill;
//                        if (!Enum.TryParse(param[0], true, out skill)) return;

//                        if (player.Info.Magics.Any(e => e.Spell == skill)) break;

//                        if (param.Count > 1)
//                            spellLevel = byte.TryParse(param[1], out spellLevel) ? Math.Min((byte)3, spellLevel) : (byte)0;

//                        var magic = new UserMagic(skill) { Level = spellLevel };

//                        if (magic.Info == null) return;

//                        player.Info.Magics.Add(magic);
//                        player.Enqueue(magic.GetInfo());
//                        break;

//                    case ActionType.Goto:
//                        DelayedAction action = new DelayedAction(DelayedType.NPC, -1, player.NPCID, "[" + param[0] + "]");
//                        player.ActionList.Add(action);
//                        break;

//                    case ActionType.Set:
//                        int flagIndex;
//                        uint onCheck;
//                        if (!int.TryParse(param[0], out flagIndex)) return;
//                        if (!uint.TryParse(param[1], out onCheck)) return;

//                        if (flagIndex < 0 || flagIndex >= Globals.FlagIndexCount) return;
//                        var flagIsOn = Convert.ToBoolean(onCheck);

//                        player.Info.Flags[flagIndex] = flagIsOn;

//                        for (int f = player.CurrentMap.NPCs.Count - 1; f >= 0; f--)
//                        {
//                            if (Functions.InRange(player.CurrentMap.NPCs[f].CurrentLocation, player.CurrentLocation, Globals.DataRange))
//                                player.CurrentMap.NPCs[f].CheckVisible(player);
//                        }

//                        if (flagIsOn) player.CheckNeedQuestFlag(flagIndex);

//                        break;

//                    case ActionType.Param1:
//                        if (!int.TryParse(param[1], out tempInt)) return;

//                        Param1 = param[0];
//                        Param1Instance = tempInt;
//                        break;

//                    case ActionType.Param2:
//                        if (!int.TryParse(param[0], out tempInt)) return;

//                        Param2 = tempInt;
//                        break;

//                    case ActionType.Param3:
//                        if (!int.TryParse(param[0], out tempInt)) return;

//                        Param3 = tempInt;
//                        break;

//                    case ActionType.Mongen:
//                        if (Param1 == null || Param2 == 0 || Param3 == 0) return;
//                        if (!byte.TryParse(param[1], out tempByte)) return;

//                        map = SMain.Envir.GetMapByNameAndInstance(Param1, Param1Instance);
//                        if (map == null) return;

//                        monInfo = SMain.Envir.GetMonsterInfo(param[0]);
//                        if (monInfo == null) return;

//                        for (int j = 0; j < tempByte; j++)
//                        {
//                            MonsterObject monster = MonsterObject.GetMonster(monInfo);
//                            if (monster == null) return;
//                            monster.Direction = 0;
//                            monster.ActionTime = SMain.Envir.Time + 1000;
//                            monster.Spawn(map, new Point(Param2, Param3));
//                        }
//                        break;

//                    case ActionType.TimeRecall:
//                        if (!long.TryParse(param[0], out tempLong)) return;

//                        if (param[1].Length > 0) tempString = "[" + param[1] + "]";

//                        Map tempMap = player.CurrentMap;
//                        Point tempPoint = player.CurrentLocation;

//                        action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time + (tempLong * 1000), player.NPCID, tempString, tempMap, tempPoint);
//                        player.ActionList.Add(action);

//                        break;

//                    case ActionType.TimeRecallGroup:
//                        if (player.GroupMembers == null) return;
//                        if (!long.TryParse(param[0], out tempLong)) return;
//                        if (param[1].Length > 0) tempString = "[" + param[1] + "]";

//                        tempMap = player.CurrentMap;
//                        tempPoint = player.CurrentLocation;

//                        for (int j = 0; j < player.GroupMembers.Count(); j++)
//                        {
//                            var groupMember = player.GroupMembers[j];

//                            action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time + (tempLong * 1000), player.NPCID, tempString, tempMap, tempPoint);
//                            groupMember.ActionList.Add(action);
//                        }
//                        break;

//                    case ActionType.BreakTimeRecall:
//                        player.ActionList.RemoveAll(d => d.Type == DelayedType.NPC);
//                        break;

//                    case ActionType.DelayGoto:
//                        if (!long.TryParse(param[0], out tempLong)) return;

//                        action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time + (tempLong * 1000), player.NPCID, "[" + param[1] + "]");
//                        player.ActionList.Add(action);
//                        break;

//                    case ActionType.MonClear:
//                        if (!int.TryParse(param[1], out tempInt)) return;

//                        map = SMain.Envir.GetMapByNameAndInstance(param[0], tempInt);
//                        if (map == null) return;

//                        foreach (var cell in map.Cells)
//                        {
//                            if (cell == null || cell.Objects == null) continue;

//                            for (int j = 0; j < cell.Objects.Count(); j++)
//                            {
//                                MapObject ob = cell.Objects[j];

//                                if (ob.Race != ObjectType.Monster) continue;
//                                if (ob.Dead) continue;
//                                ob.Die();
//                            }
//                        }
//                        break;
//                    case ActionType.GroupRecall:
//                        if (player.GroupMembers == null) return;

//                        for (int j = 0; j < player.GroupMembers.Count(); j++)
//                        {
//                            player.GroupMembers[j].Teleport(player.CurrentMap, player.CurrentLocation);
//                        }
//                        break;

//                    case ActionType.GroupTeleport:
//                        if (player.GroupMembers == null) return;
//                        if (!int.TryParse(param[1], out tempInt)) return;
//                        if (!int.TryParse(param[2], out x)) return;
//                        if (!int.TryParse(param[3], out y)) return;

//                        map = SMain.Envir.GetMapByNameAndInstance(param[0], tempInt);
//                        if (map == null) return;

//                        for (int j = 0; j < player.GroupMembers.Count(); j++)
//                        {
//                            if (x == 0 || y == 0)
//                            {
//                                player.GroupMembers[j].TeleportRandom(200, 0, map);
//                            }
//                            else
//                            {
//                                player.GroupMembers[j].Teleport(map, new Point(x, y));
//                            }
//                        }
//                        break;

//                    case ActionType.Mov:
//                        string value = param[0];
//                        AddVariable(player, value, param[1]);
//                        break;

//                    case ActionType.Calc:
//                        int left;
//                        int right;

//                        bool resultLeft = int.TryParse(param[0], out left);
//                        bool resultRight = int.TryParse(param[2], out right);

//                        if (resultLeft && resultRight)
//                        {
//                            try
//                            {
//                                int result = Calculate(param[1], left, right);
//                                AddVariable(player, param[3].Replace("-", ""), result.ToString());
//                            }
//                            catch (ArgumentException)
//                            {
//                                SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[1], Key));
//                            }
//                        }
//                        else
//                        {
//                            AddVariable(player, param[3].Replace("-", ""), param[0] + param[2]);
//                        }
//                        break;

//                    case ActionType.GiveBuff:

//                        if (!Enum.IsDefined(typeof(BuffType), param[0])) return;

//                        tempBool = false;
//                        bool tempBool2 = false;

//                        long.TryParse(param[1], out tempLong);

//                        string[] stringValues = param[2].Split(',');

//                        if (stringValues.Length < 1) return;

//                        int[] intValues = new int[stringValues.Length];

//                        for (int j = 0; j < intValues.Length; j++)
//                        {
//                            int.TryParse(stringValues[j], out intValues[j]);
//                        }

//                        if (intValues.Length < 1) return;

//                        if (param[3].Length > 0)
//                            bool.TryParse(param[3], out tempBool);

//                        if (param[4].Length > 0)
//                            bool.TryParse(param[4], out tempBool2);

//                        Buff buff = new Buff
//                        {
//                            Type = (BuffType)(byte)Enum.Parse(typeof(BuffType), param[0], true),
//                            Caster = player,
//                            ExpireTime = SMain.Envir.Time + tempLong * 1000,
//                            Values = intValues,
//                            Infinite = tempBool,
//                            Visible = tempBool2
//                        };

//                        player.AddBuff(buff);
//                        break;

//                    case ActionType.RemoveBuff:
//                        if (!Enum.IsDefined(typeof(BuffType), param[0])) return;

//                        BuffType bType = (BuffType)(byte)Enum.Parse(typeof(BuffType), param[0]);

//                        for (int j = 0; j < player.Buffs.Count; j++)
//                        {
//                            if (player.Buffs[j].Type != bType) continue;

//                            player.Buffs[j].Infinite = false;
//                            player.Buffs[j].ExpireTime = SMain.Envir.Time;
//                        }
//                        break;

//                    case ActionType.AddToGuild:
//                        {
//                            if (player.MyGuild != null) return;

//                            GuildObject guild = SMain.Envir.GetGuild(param[0]);

//                            if (guild == null) return;

//                            player.PendingGuildInvite = guild;
//                            player.GuildInvite(true);
//                        }
//                        break;

//                    case ActionType.RemoveFromGuild:
//                        if (player.MyGuild == null) return;

//                        if (player.MyGuildRank == null) return;

//                        player.MyGuild.DeleteMember(player, player.Name);

//                        break;

//                    case ActionType.RefreshEffects:
//                        player.SetLevelEffects();
//                        p = new S.ObjectLevelEffects { ObjectID = player.ObjectID, LevelEffects = player.LevelEffects };
//                        player.Enqueue(p);
//                        player.Broadcast(p);
//                        break;

//                    case ActionType.CanGainExp:
//                        bool.TryParse(param[0], out tempBool);
//                        player.CanGainExp = tempBool;
//                        break;

//                    case ActionType.ComposeMail:

//                        mailInfo = new MailInfo(player.Info.Index, false)
//                        {
//                            Sender = param[1],
//                            Message = param[0]
//                        };
//                        break;
//                    case ActionType.AddMailGold:
//                        if (mailInfo == null) return;

//                        uint.TryParse(param[0], out tempUint);

//                        mailInfo.Gold += tempUint;
//                        break;

//                    case ActionType.AddMailItem:
//                        if (mailInfo == null) return;
//                        if (mailInfo.Items.Count > 5) return;

//                        if (param.Count < 2 || !uint.TryParse(param[1], out count)) count = 1;

//                        info = SMain.Envir.GetItemInfo(param[0]);

//                        if (info == null)
//                        {
//                            SMain.Enqueue(string.Format("Failed to get ItemInfo: {0}, Page: {1}", param[0], Key));
//                            break;
//                        }

//                        while (count > 0 && mailInfo.Items.Count < 5)
//                        {
//                            UserItem item = SMain.Envir.CreateFreshItem(info);

//                            if (item == null)
//                            {
//                                SMain.Enqueue(string.Format("Failed to create UserItem: {0}, Page: {1}", param[0], Key));
//                                return;
//                            }

//                            if (item.Info.StackSize > count)
//                            {
//                                item.Count = count;
//                                count = 0;
//                            }
//                            else
//                            {
//                                count -= item.Info.StackSize;
//                                item.Count = item.Info.StackSize;
//                            }

//                            mailInfo.Items.Add(item);
//                        }


//                        break;

//                    case ActionType.SendMail:
//                        if (mailInfo == null) return;

//                        mailInfo.Send();

//                        break;

//                    case ActionType.GroupGoto:
//                        if (player.GroupMembers == null) return;

//                        for (int j = 0; j < player.GroupMembers.Count(); j++)
//                        {
//                            action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time, player.NPCID, "[" + param[0] + "]");
//                            player.GroupMembers[j].ActionList.Add(action);
//                        }
//                        break;

//                    case ActionType.EnterMap:
//                        if (player.NPCMoveMap == null || player.NPCMoveCoord.IsEmpty) return;
//                        player.Teleport(player.NPCMoveMap, player.NPCMoveCoord, false);
//                        player.NPCMoveMap = null;
//                        player.NPCMoveCoord = Point.Empty;
//                        break;

//                    case ActionType.MakeWeddingRing:
//                        player.MakeWeddingRing();
//                        break;

//                    case ActionType.ForceDivorce:
//                        player.NPCDivorce();
//                        break;

//                    case ActionType.LoadValue:
//                        string val = param[0];
//                        string filePath = param[1];
//                        string header = param[2];
//                        string key = param[3];

//                        InIReader reader = new InIReader(filePath);
//                        string loadedString = reader.ReadString(header, key, "");

//                        if (loadedString == "") break;
//                        AddVariable(player, val, loadedString);
//                        break;

//                    case ActionType.SaveValue:
//                        filePath = param[0];
//                        header = param[1];
//                        key = param[2];
//                        val = param[3];

//                        reader = new InIReader(filePath);
//                        reader.Write(header, key, val);
//                        break;
//                }
//            }
//        }
//        public List<string> ParseSay(PlayerObject player, List<string> speech)
//        {
//            for (var i = 0; i < speech.Count; i++)
//            {
//                var parts = speech[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

//                if (parts.Length == 0) continue;

//                foreach (var part in parts)
//                {
//                    speech[i] = speech[i].Replace(part, ReplaceValue(player, part));
//                }
//            }
//            return speech;
//        }

//        private void Success(PlayerObject player)
//        {
//            Act(ActList, player);

//            var parseSay = new List<String>(Say);
//            parseSay = ParseSay(player, parseSay);

//            player.Enqueue(new S.NPCResponse { Page = parseSay });
//        }
//        private void Failed(PlayerObject player)
//        {
//            Act(ElseActList, player);

//            var parseElseSay = new List<String>(ElseSay);
//            parseElseSay = ParseSay(player, parseElseSay);

//            player.Enqueue(new S.NPCResponse { Page = parseElseSay });
//        }

//        public void AddVariable(PlayerObject player, string key, string value)
//        {
//            Regex regex = new Regex(@"[A-Za-z][0-9]");

//            if (!regex.Match(key).Success) return;

//            for (int i = 0; i < player.NPCVar.Count; i++)
//            {
//                if (!String.Equals(player.NPCVar[i].Key, key, StringComparison.CurrentCultureIgnoreCase)) continue;
//                player.NPCVar[i] = new KeyValuePair<string, string>(player.NPCVar[i].Key, value);
//                return;
//            }

//            player.NPCVar.Add(new KeyValuePair<string, string>(key, value));
//        }
//        public string FindVariable(PlayerObject player, string key)
//        {
//            Regex regex = new Regex(@"\%[A-Za-z][0-9]");

//            if (!regex.Match(key).Success) return key;

//            string tempKey = key.Substring(1);

//            foreach (KeyValuePair<string, string> t in player.NPCVar)
//            {
//                if (String.Equals(t.Key, tempKey, StringComparison.CurrentCultureIgnoreCase)) return t.Value;
//            }

//            return key;
//        }

//        public string ReplaceValue(PlayerObject player, string param)
//        {
//            var regex = new Regex(@"\<\$(.*?)\>");
//            var varRegex = new Regex(@"(.*?)\(([A-Z][0-9])\)");

//            var match = regex.Match(param);

//            if (!match.Success) return param;

//            string innerMatch = match.Groups[1].Captures[0].Value.ToUpper();

//            Match varMatch = varRegex.Match(innerMatch);

//            if (varRegex.Match(innerMatch).Success)
//                innerMatch = innerMatch.Replace(varMatch.Groups[2].Captures[0].Value.ToUpper(), "");

//            string newValue = string.Empty;

//            switch (innerMatch)
//            {
//                case "OUTPUT()":
//                    newValue = FindVariable(player, "%" + varMatch.Groups[2].Captures[0].Value.ToUpper());
//                    break;
//                case "NPCNAME":
//                    newValue = NPCName.Replace("_", " ");
//                    break;
//                case "USERNAME":
//                    newValue = player.Name;
//                    break;
//                case "LEVEL":
//                    newValue = player.Level.ToString(CultureInfo.InvariantCulture);
//                    break;
//                case "MAP":
//                    newValue = player.CurrentMap.Info.FileName;
//                    break;
//                case "X_COORD":
//                    newValue = player.CurrentLocation.X.ToString();
//                    break;
//                case "Y_COORD":
//                    newValue = player.CurrentLocation.Y.ToString();
//                    break;
//                case "HP":
//                    newValue = player.HP.ToString(CultureInfo.InvariantCulture);
//                    break;
//                case "MAXHP":
//                    newValue = player.MaxHP.ToString(CultureInfo.InvariantCulture);
//                    break;
//                case "MP":
//                    newValue = player.MP.ToString(CultureInfo.InvariantCulture);
//                    break;
//                case "MAXMP":
//                    newValue = player.MaxMP.ToString(CultureInfo.InvariantCulture);
//                    break;
//                case "GAMEGOLD":
//                    newValue = player.Account.Gold.ToString(CultureInfo.InvariantCulture);
//                    break;
//                case "CREDIT":
//                    newValue = player.Account.Credit.ToString(CultureInfo.InvariantCulture);
//                    break;
//                case "ARMOUR":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.Armour] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.Armour].Info.Name : "No Armour";
//                    break;
//                case "WEAPON":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.Weapon] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.Weapon].Info.Name : "No Weapon";
//                    break;
//                case "RING_L":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.RingL] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.RingL].Info.Name : "No Ring";
//                    break;
//                case "RING_R":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.RingR] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.RingR].Info.Name : "No Ring";
//                    break;
//                case "BRACELET_L":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.BraceletL] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.BraceletL].Info.Name : "No Bracelet";
//                    break;
//                case "BRACELET_R":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.BraceletR] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.BraceletR].Info.Name : "No Bracelet";
//                    break;
//                case "NECKLACE":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.Necklace] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.Necklace].Info.Name : "No Necklace";
//                    break;
//                case "BELT":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.Belt] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.Belt].Info.Name : "No Belt";
//                    break;
//                case "BOOTS":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.Boots] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.Boots].Info.Name : "No Boots";
//                    break;
//                case "HELMET":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.Helmet] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.Helmet].Info.Name : "No Helmet";
//                    break;
//                case "AMULET":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.Amulet] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.Amulet].Info.Name : "No Amulet";
//                    break;
//                case "STONE":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.Stone] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.Stone].Info.Name : "No Stone";
//                    break;
//                case "TORCH":
//                    newValue = player.Info.Equipment[(int)EquipmentSlot.Torch] != null ?
//                        player.Info.Equipment[(int)EquipmentSlot.Torch].Info.Name : "No Torch";
//                    break;

//                case "DATE":
//                    newValue = DateTime.Now.ToShortDateString();
//                    break;
//                case "USERCOUNT":
//                    newValue = SMain.Envir.PlayerCount.ToString(CultureInfo.InvariantCulture);
//                    break;
//                case "PKPOINT":
//                    newValue = player.PKPoints.ToString();
//                    break;
//                case "GUILDWARTIME":
//                    newValue = Settings.Guild_WarTime.ToString();
//                    break;
//                case "GUILDWARFEE":
//                    newValue = Settings.Guild_WarCost.ToString();
//                    break;

//                case "PARCELAMOUNT":
//                    newValue = player.GetMailAwaitingCollectionAmount().ToString();
//                    break;

//                default:
//                    newValue = string.Empty;
//                    break;
//            }

//            if (string.IsNullOrEmpty(newValue)) return param;

//            return param.Replace(match.Value, newValue);
//        }


//        Functions
//        public static bool Compare<T>(string op, T left, T right) where T : IComparable<T>
//        {
//            switch (op)
//            {
//                case "<": return left.CompareTo(right) < 0;
//                case ">": return left.CompareTo(right) > 0;
//                case "<=": return left.CompareTo(right) <= 0;
//                case ">=": return left.CompareTo(right) >= 0;
//                case "==": return left.Equals(right);
//                case "!=": return !left.Equals(right);
//                default: throw new ArgumentException("Invalid comparison operator: {0}", op);
//            }
//        }
//        public static int Calculate(string op, int left, int right)
//        {
//            switch (op)
//            {
//                case "+": return left + right;
//                case "-": return left - right;
//                case "*": return left * right;
//                case "/": return left / right;
//                default: throw new ArgumentException("Invalid sum operator: {0}", op);
//            }
//        }
//    }

//    public class NPCChecks
//    {
//        public CheckType Type;
//        public List<string> Params = new List<string>();

//        public NPCChecks(CheckType check, params string[] p)
//        {
//            Type = check;

//            for (int i = 0; i < p.Length; i++)
//                Params.Add(p[i]);
//        }
//    }
//    public class NPCActions
//    {
//        public ActionType Type;
//        public List<string> Params = new List<string>();

//        public NPCActions(ActionType action, params string[] p)
//        {
//            Type = action;

//            Params.AddRange(p);
//        }
//    }

//    public enum ActionType
//    {
//        Move,
//        InstanceMove,
//        GiveGold,
//        TakeGold,
//        GiveCredit,
//        TakeCredit,
//        GiveItem,
//        TakeItem,
//        GiveExp,
//        GivePet,
//        ClearPets,
//        AddNameList,
//        DelNameList,
//        ClearNameList,
//        GiveHP,
//        GiveMP,
//        ChangeLevel,
//        SetPkPoint,
//        ReducePkPoint,
//        IncreasePkPoint,
//        ChangeGender,
//        ChangeClass,
//        LocalMessage,
//        Goto,
//        GiveSkill,
//        Set,
//        Param1,
//        Param2,
//        Param3,
//        Mongen,
//        TimeRecall,
//        TimeRecallGroup,
//        BreakTimeRecall,
//        MonClear,
//        GroupRecall,
//        GroupTeleport,
//        DelayGoto,
//        Mov,
//        Calc,
//        GiveBuff,
//        RemoveBuff,
//        AddToGuild,
//        RemoveFromGuild,
//        RefreshEffects,
//        ChangeHair,
//        CanGainExp,
//        ComposeMail,
//        AddMailItem,
//        AddMailGold,
//        SendMail,
//        GroupGoto,
//        EnterMap,
//        GivePearls,
//        TakePearls,
//        MakeWeddingRing,
//        ForceDivorce,
//        GlobalMessage,
//        LoadValue,
//        SaveValue,
//        RemovePet
//    }
//    public enum CheckType
//    {
//        IsAdmin,
//        Level,
//        CheckItem,
//        CheckGold,
//        CheckCredit,
//        CheckGender,
//        CheckClass,
//        CheckDay,
//        CheckHour,
//        CheckMinute,
//        CheckNameList,
//        CheckPkPoint,
//        CheckRange,
//        Check,
//        CheckHum,
//        CheckMon,
//        CheckExactMon,
//        Random,
//        Groupleader,
//        GroupCount,
//        PetLevel,
//        PetCount,
//        CheckCalc,
//        InGuild,
//        CheckMap,
//        CheckQuest,
//        CheckRelationship,
//        CheckWeddingRing,
//        CheckPet
//    }
//}
