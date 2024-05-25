using System.Drawing;
﻿using Server.MirDatabase;
using Server.MirEnvir;
using System.Text.RegularExpressions;
using ServerPackets;

namespace Server.MirObjects
{
    public class NpcScript
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        protected static MessageQueue MessageQueue
        {
            get { return MessageQueue.Instance; }
        }

        public static NpcScript Get(int index)
        {
            return Envir.Scripts[index];
        }

        public static NpcScript GetOrAdd(uint loadedObjectID, string fileName, NpcScriptType type)
        {
            var script = Envir.Scripts.SingleOrDefault(x => x.Value.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase) && x.Value.LoadedObjectID == loadedObjectID).Value;

            if (script != null)
            {
                return script;
            }

            return new NpcScript(loadedObjectID, fileName, type);
        }

        public readonly int ScriptID;
        public readonly uint LoadedObjectID;
        public readonly NpcScriptType Type;
        public readonly string FileName;

        public const string
            MainKey = "[@MAIN]",
            BuyKey = "[@BUY]",
            SellKey = "[@SELL]",
            BuySellKey = "[@BUYSELL]",
            RepairKey = "[@REPAIR]",
            SRepairKey = "[@SREPAIR]",
            RefineKey = "[@REFINE]",
            RefineCheckKey = "[@REFINECHECK]",
            RefineCollectKey = "[@REFINECOLLECT]",
            ReplaceWedRingKey = "[@REPLACEWEDDINGRING]",
            BuyBackKey = "[@BUYBACK]",
            StorageKey = "[@STORAGE]",
            ConsignKey = "[@CONSIGN]",
            MarketKey = "[@MARKET]",
            CraftKey = "[@CRAFT]",

            GuildCreateKey = "[@CREATEGUILD]",
            RequestWarKey = "[@REQUESTWAR]",
            SendParcelKey = "[@SENDPARCEL]",
            CollectParcelKey = "[@COLLECTPARCEL]",
            AwakeningKey = "[@AWAKENING]",
            DisassembleKey = "[@DISASSEMBLE]",
            DowngradeKey = "[@DOWNGRADE]",
            ResetKey = "[@RESET]",
            PearlBuyKey = "[@PEARLBUY]",
            BuyUsedKey = "[@BUYUSED]",
            BuyNewKey = "[@BUYNEW]",
            BuySellNewKey = "[@BUYSELLNEW]",
            HeroCreateKey = "[@CREATEHERO]",
            HeroManageKey = "[@MANAGEHERO]",

            TradeKey = "[TRADE]",
            RecipeKey = "[RECIPE]",
            TypeKey = "[TYPES]",
            UsedTypeKey = "[USEDTYPES]",
            QuestKey = "[QUESTS]",
            SpeechKey = "[SPEECH]";


        public List<ItemType> Types = new List<ItemType>();
        public List<ItemType> UsedTypes = new List<ItemType>();
        public List<UserItem> Goods = new List<UserItem>();
        public List<RecipeInfo> CraftGoods = new List<RecipeInfo>();

        public List<NpcPage> NpcSections = new List<NpcPage>();
        public List<NpcPage> NpcPages = new List<NpcPage>();

        private NpcScript(uint loadedObjectID, string fileName, NpcScriptType type)
        {
            ScriptID = ++Envir.ScriptIndex;

            LoadedObjectID = loadedObjectID;
            FileName = fileName;
            Type = type;

            Load();

            Envir.Scripts.Add(ScriptID, this);
        }

        public void Load()
        {
            LoadInfo();
            LoadGoods();
        }

        public float PriceRate(PlayerObject player, bool baseRate = false)
        {
            var callingNpc = Envir.Npcs.SingleOrDefault(x => x.ObjectID == player.NpcObjectID);

            if (callingNpc == null)
            {
                return 1F;
            }

            if (callingNpc.Conq == null || baseRate)
            {
                return callingNpc.Info.Rate / 100F;
            }

            if (player.MyGuild != null && player.MyGuild.Guildindex == callingNpc.Conq.GuildInfo.Owner)
            {
                return callingNpc.Info.Rate / 100F;
            }
            else
            {
                return (((callingNpc.Info.Rate / 100F) * callingNpc.Conq.GuildInfo.NpcRate) + callingNpc.Info.Rate) / 100F;
            }
        }


        public void LoadInfo()
        {
            ClearInfo();

            if (!Directory.Exists(Settings.NpcPath)) return;

            string fileName = Path.Combine(Settings.NpcPath, FileName + ".txt");

            if (File.Exists(fileName))
            {
                List<string> lines = File.ReadAllLines(fileName).ToList();

                lines = ParseInsert(lines);
                lines = ParseInclude(lines);

                switch (Type)
                {
                    case NpcScriptType.Normal:
                    default:
                        ParseScript(lines);
                        break;
                    case NpcScriptType.AutoPlayer:
                    case NpcScriptType.AutoMonster:
                    case NpcScriptType.Robot:
                        ParseDefault(lines);
                        break;
                }
            }
            else
                MessageQueue.Enqueue(string.Format("Script Not Found: {0}", FileName));
        }
        public void ClearInfo()
        {
            Goods = new List<UserItem>();
            Types = new List<ItemType>();
            UsedTypes = new List<ItemType>();
            NpcPages = new List<NpcPage>();
            CraftGoods = new List<RecipeInfo>();

            if (Type == NpcScriptType.AutoPlayer)
            {
                Envir.CustomCommands.Clear();
            }
        }
        public void LoadGoods()
        {
            var loadedNpc = NpcObject.Get(LoadedObjectID);

            if (loadedNpc != null)
            {
                loadedNpc.UsedGoods.Clear();

                string path = Path.Combine(Settings.GoodsPath, loadedNpc.Info.Index.ToString() + ".msd");

                if (!File.Exists(path)) return;

                using (FileStream stream = File.OpenRead(path))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int version = reader.ReadInt32();
                        int count = version;
                        int customversion = Envir.LoadCustomVersion;
                        if (version == 9999)//the only real way to tell if the file was made before or after version code got added: assuming nobody had a config option to save more then 10000 sold items
                        {
                            version = reader.ReadInt32();
                            customversion = reader.ReadInt32();
                            count = reader.ReadInt32();
                        }
                        else
                            version = Envir.LoadVersion;

                        for (int k = 0; k < count; k++)
                        {
                            UserItem item = new UserItem(reader, version, customversion);
                            if (Envir.BindItem(item))
                                loadedNpc.UsedGoods.Add(item);
                        }
                    }
                }
            }
        }

        private void ParseDefault(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].ToUpper().StartsWith("[@_")) continue;

                if (Type == NpcScriptType.AutoPlayer)
                {
                    if (lines[i].ToUpper().Contains("MAPCOORD"))
                    {
                        Regex regex = new Regex(@"\((.*?),([0-9]{1,3}),([0-9]{1,3})\)");
                        Match match = regex.Match(lines[i]);

                        if (!match.Success) continue;

                        Map map = Envir.MapList.FirstOrDefault(m => m.Info.FileName == match.Groups[1].Value);

                        if (map == null) continue;

                        Point point = new Point(Convert.ToInt16(match.Groups[2].Value), Convert.ToInt16(match.Groups[3].Value));

                        if (!map.Info.ActiveCoords.Contains(point))
                        {
                            map.Info.ActiveCoords.Add(point);
                        }
                    }

                    if (lines[i].ToUpper().Contains("CUSTOMCOMMAND"))
                    {
                        Regex regex = new Regex(@"\((.*?)\)");
                        Match match = regex.Match(lines[i]);

                        if (!match.Success) continue;

                        Envir.CustomCommands.Add(match.Groups[1].Value);
                    }
                }
                else if (Type == NpcScriptType.AutoMonster)
                {
                    MonsterInfo MobInfo;
                    if (lines[i].ToUpper().Contains("SPAWN"))
                    {
                        Regex regex = new Regex(@"\((.*?)\)");
                        Match match = regex.Match(lines[i]);

                        if (!match.Success) continue;
                        MobInfo = Envir.GetMonsterInfo(Convert.ToInt16(match.Groups[1].Value));
                        if (MobInfo == null) continue;
                        MobInfo.HasSpawnScript = true;
                    }
                    if (lines[i].ToUpper().Contains("DIE"))
                    {
                        Regex regex = new Regex(@"\((.*?)\)");
                        Match match = regex.Match(lines[i]);

                        if (!match.Success) continue;
                        MobInfo = Envir.GetMonsterInfo(Convert.ToInt16(match.Groups[1].Value));
                        if (MobInfo == null) continue;
                        MobInfo.HasDieScript = true;
                    }
                }
                else if (Type == NpcScriptType.Robot)
                {
                    if (lines[i].ToUpper().Contains("TIME"))
                    {
                        Robot.AddRobot(lines[i].ToUpper());
                    }
                }

                NpcPages.AddRange(ParsePages(lines, lines[i]));
            }
        }

        private void ParseScript(IList<string> lines)
        {
            NpcPages.AddRange(ParsePages(lines));

            ParseGoods(lines);
            ParseTypes(lines);
            ParseQuests(lines);
            ParseCrafting(lines);
            ParseSpeech(lines);
        }

        private List<string> ParseInsert(List<string> lines)
        {
            List<string> newLines = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].ToUpper().StartsWith("#INSERT")) continue;

                string[] split = lines[i].Split(' ');

                if (split.Length < 2) continue;

                string path = Path.Combine(Settings.EnvirPath, split[1].Substring(1, split[1].Length - 2));

                if (!File.Exists(path))
                    MessageQueue.Enqueue(string.Format("INSERT Script Not Found: {0}", path));
                else
                    newLines = File.ReadAllLines(path).ToList();

                lines.AddRange(newLines);
            }

            lines.RemoveAll(str => str.ToUpper().StartsWith("#INSERT"));

            return lines;
        }

        private List<string> ParseInclude(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].ToUpper().StartsWith("#INCLUDE")) continue;

                string[] split = lines[i].Split(' ');

                string path = Path.Combine(Settings.EnvirPath, split[1].Substring(1, split[1].Length - 2));
                string page = ("[" + split[2] + "]").ToUpper();

                bool start = false, finish = false;

                var parsedLines = new List<string>();

                if (!File.Exists(path))
                {
                    MessageQueue.Enqueue(string.Format("INCLUDE Script Not Found: {0}", path));
                    return parsedLines;
                }

                IList<string> extLines = File.ReadAllLines(path);

                for (int j = 0; j < extLines.Count; j++)
                {
                    if (!extLines[j].ToUpper().StartsWith(page)) continue;

                    for (int x = j + 1; x < extLines.Count; x++)
                    {
                        if (extLines[x].Trim() == ("{"))
                        {
                            start = true;
                            continue;
                        }

                        if (extLines[x].Trim() == ("}"))
                        {
                            finish = true;
                            break;
                        }

                        parsedLines.Add(extLines[x]);
                    }
                }

                if (start && finish)
                {
                    lines.InsertRange(i + 1, parsedLines);
                    parsedLines.Clear();
                }
            }

            lines.RemoveAll(str => str.ToUpper().StartsWith("#INCLUDE"));

            return lines;
        }


        private List<NpcPage> ParsePages(IList<string> lines, string key = MainKey)
        {
            List<NpcPage> pages = new List<NpcPage>();
            List<string> buttons = new List<string>();

            NpcPage page = ParsePage(lines, key);
            pages.Add(page);

            buttons.AddRange(page.Buttons);

            for (int i = 0; i < buttons.Count; i++)
            {
                string section = buttons[i];

                bool match = pages.Any(t => t.Key.ToUpper() == section.ToUpper());

                if (match) continue;

                page = ParsePage(lines, section);
                buttons.AddRange(page.Buttons);

                pages.Add(page);
            }

            return pages;
        }

        private NpcPage ParsePage(IList<string> scriptLines, string sectionName)
        {
            bool nextPage = false, nextSection = false;

            List<string> lines = scriptLines.Where(x => !string.IsNullOrEmpty(x)).ToList();

            NpcPage Page = new NpcPage(sectionName);

            //Cleans arguments out of search page name
            string tempSectionName = Page.ArgumentParse(sectionName);

            //parse all individual pages in a script, defined by sectionName
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                if (line.StartsWith(";")) continue;

                if (!lines[i].ToUpper().StartsWith(tempSectionName.ToUpper())) continue;

                List<string> segmentLines = new List<string>();

                nextPage = false;

                //Found a page, now process that page and split it into segments
                for (int j = i + 1; j < lines.Count; j++)
                {
                    string nextLine = lines[j];

                    if (j < lines.Count - 1)
                        nextLine = lines[j + 1];
                    else
                        nextLine = "";

                    if (nextLine.StartsWith("[") && nextLine.EndsWith("]"))
                    {
                        nextPage = true;
                    }

                    else if (nextLine.StartsWith("#IF"))
                    {
                        nextSection = true;
                    }

                    if (nextSection || nextPage)
                    {
                        segmentLines.Add(lines[j]);

                        //end of segment, so need to parse it and put into the segment list within the page
                        if (segmentLines.Count > 0)
                        {
                            NpcSegment segment = ParseSegment(Page, segmentLines);

                            List<string> currentButtons = new List<string>();
                            currentButtons.AddRange(segment.Buttons);
                            currentButtons.AddRange(segment.ElseButtons);
                            currentButtons.AddRange(segment.GotoButtons);

                            Page.Buttons.AddRange(currentButtons);
                            Page.SegmentList.Add(segment);
                            segmentLines.Clear();

                            nextSection = false;
                        }

                        if (nextPage) break;

                        continue;
                    }

                    segmentLines.Add(lines[j]);
                }

                //bottom of script reached, add all lines found to new segment
                if (segmentLines.Count > 0)
                {
                    NpcSegment segment = ParseSegment(Page, segmentLines);

                    List<string> currentButtons = new List<string>();
                    currentButtons.AddRange(segment.Buttons);
                    currentButtons.AddRange(segment.ElseButtons);
                    currentButtons.AddRange(segment.GotoButtons);

                    Page.Buttons.AddRange(currentButtons);
                    Page.SegmentList.Add(segment);
                    segmentLines.Clear();
                }

                return Page;
            }

            return Page;
        }

        private NpcSegment ParseSegment(NpcPage page, IEnumerable<string> scriptLines)
        {
            List<string>
                checks = new List<string>(),
                acts = new List<string>(),
                say = new List<string>(),
                buttons = new List<string>(),
                elseSay = new List<string>(),
                elseActs = new List<string>(),
                elseButtons = new List<string>(),
                gotoButtons = new List<string>();

            List<string> lines = scriptLines.ToList();
            List<string> currentSay = say, currentButtons = buttons;

            Regex regex = new Regex(@"<.*?/(\@.*?)>");

            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                if (lines[i].StartsWith(";")) continue;

                if (lines[i].StartsWith("#"))
                {
                    string[] action = lines[i].Remove(0, 1).ToUpper().Trim().Split(' ');
                    switch (action[0])
                    {
                        case "IF":
                            currentSay = checks;
                            currentButtons = null;
                            continue;
                        case "SAY":
                            currentSay = say;
                            currentButtons = buttons;
                            continue;
                        case "ACT":
                            currentSay = acts;
                            currentButtons = gotoButtons;
                            continue;
                        case "ELSESAY":
                            currentSay = elseSay;
                            currentButtons = elseButtons;
                            continue;
                        case "ELSEACT":
                            currentSay = elseActs;
                            currentButtons = gotoButtons;
                            continue;
                        default:
                            throw new NotImplementedException();
                    }
                }

                if (lines[i].StartsWith("[") && lines[i].EndsWith("]")) break;

                if (currentButtons != null)
                {
                    Match match = regex.Match(lines[i]);

                    while (match.Success)
                    {
                        string argu = match.Groups[1].Captures[0].Value;
                        argu = argu.Split('/')[0];

                        currentButtons.Add(string.Format("[{0}]", argu));
                        match = match.NextMatch();
                    }

                    //Check if line has a goto command
                    var parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Count() > 1)
                        switch (parts[0].ToUpper())
                        {
                            case "GOTO":
                            case "GROUPGOTO":
                                gotoButtons.Add(string.Format("[{0}]", parts[1].ToUpper()));
                                break;
                            case "TIMERECALL":
                            case "DELAYGOTO":
                            case "TIMERECALLGROUP":
                                if (parts.Length > 2)
                                    gotoButtons.Add(string.Format("[{0}]", parts[2].ToUpper()));
                                break;
                            case "ROLLDIE":
                            case "ROLLYUT":
                                buttons.Add(string.Format("[{0}]", parts[1].ToUpper()));
                                break;
                        }
                }

                currentSay.Add(lines[i].TrimEnd());
            }

            NpcSegment segment = new NpcSegment(page, say, buttons, elseSay, elseButtons, gotoButtons);

            for (int i = 0; i < checks.Count; i++)
                segment.ParseCheck(checks[i]);

            for (int i = 0; i < acts.Count; i++)
                segment.ParseAct(segment.ActList, acts[i]);

            for (int i = 0; i < elseActs.Count; i++)
                segment.ParseAct(segment.ElseActList, elseActs[i]);

            currentButtons = new List<string>();
            currentButtons.AddRange(buttons);
            currentButtons.AddRange(elseButtons);
            currentButtons.AddRange(gotoButtons);

            return segment;
        }

        private void ParseTypes(IList<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].ToUpper().StartsWith(TypeKey)) continue;

                while (++i < lines.Count)
                {
                    if (String.IsNullOrEmpty(lines[i])) continue;

                    if (!int.TryParse(lines[i], out int index)) break;
                    Types.Add((ItemType)index);
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].ToUpper().StartsWith(UsedTypeKey)) continue;

                while (++i < lines.Count)
                {
                    if (String.IsNullOrEmpty(lines[i])) continue;

                    if (!int.TryParse(lines[i], out int index)) break;
                    UsedTypes.Add((ItemType)index);
                }
            }
        }
        private void ParseGoods(IList<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].ToUpper().StartsWith(TradeKey)) continue;

                while (++i < lines.Count)
                {
                    if (lines[i].StartsWith("[")) return;
                    if (String.IsNullOrEmpty(lines[i])) continue;

                    var data = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    ItemInfo info = Envir.GetItemInfo(data[0]);
                    if (info == null)
                        continue;

                    UserItem goods = Envir.CreateShopItem(info, (uint)i);

                    if (goods == null || Goods.Contains(goods))
                    {
                        MessageQueue.Enqueue(string.Format("Could not find Item: {0}, File: {1}", lines[i], FileName));
                        continue;
                    }

                    ushort count = 1;
                    if (data.Length == 2)
                        ushort.TryParse(data[1], out count);

                    goods.Count = count;

                    Goods.Add(goods);
                }
            }
        }
        private void ParseQuests(IList<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].ToUpper().StartsWith(QuestKey)) continue;

                var loadedNpc = NpcObject.Get(LoadedObjectID);

                if (loadedNpc == null)
                {
                    return;
                }

                while (++i < lines.Count)
                {
                    if (lines[i].StartsWith("[")) return;
                    if (String.IsNullOrEmpty(lines[i])) continue;

                    int.TryParse(lines[i], out int index);

                    if (index == 0) continue;

                    QuestInfo info = Envir.GetQuestInfo(Math.Abs(index));

                    if (info == null) return;

                    if (index > 0)
                        info.NpcIndex = LoadedObjectID;
                    else
                        info.FinishNpcIndex = LoadedObjectID;

                    if (loadedNpc.Quests.All(x => x != info))
                        loadedNpc.Quests.Add(info);

                }
            }
        }


        private void ParseSpeech(IList<string> lines)
        {
            var loadedNpc = NpcObject.Get(LoadedObjectID);

            if (loadedNpc == null)
            {
                return;
            }

            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].ToUpper().StartsWith(SpeechKey)) continue;

                while (++i < lines.Count)
                {
                    if (String.IsNullOrEmpty(lines[i])) continue;

                    var parts = lines[i].Split(' ');

                    if (parts.Length < 2 || !int.TryParse(parts[0], out int weight)) return;

                    loadedNpc.Speech.Add(new NpcSpeech { Weight = weight, Message = lines[i].Substring(parts[0].Length + 1) });
                }
            }
        }
        private void ParseCrafting(IList<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].ToUpper().StartsWith(RecipeKey)) continue;

                while (++i < lines.Count)
                {
                    if (lines[i].StartsWith("[")) return;
                    if (String.IsNullOrEmpty(lines[i])) continue;

                    var data = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    ItemInfo info = Envir.GetItemInfo(data[0]);
                    if (info == null)
                        continue;

                    RecipeInfo recipe = Envir.RecipeInfoList.SingleOrDefault(x => x.MatchItem(info.Index));

                    if (recipe == null)
                    {
                        MessageQueue.Enqueue(string.Format("Could not find recipe: {0}, File: {1}", lines[i], FileName));
                        continue;
                    }

                    if (recipe.Ingredients.Count == 0)
                    {
                        MessageQueue.Enqueue(string.Format("Could not find ingredients: {0}, File: {1}", lines[i], FileName));
                        continue;
                    }

                    CraftGoods.Add(recipe);
                }
            }
        }

        public void Call(MonsterObject monster, string key)
        {
            key = key.ToUpper();

            for (int i = 0; i < NpcPages.Count; i++)
            {
                NpcPage page = NpcPages[i];
                if (!String.Equals(page.Key, key, StringComparison.CurrentCultureIgnoreCase)) continue;

                foreach (NpcSegment segment in page.SegmentList)
                {
                    if (page.BreakFromSegments)
                    {
                        page.BreakFromSegments = false;
                        break;
                    }

                    ProcessSegment(monster, page, segment);
                }
            }
        }
        public void Call(string key)
        {
            key = key.ToUpper();

            for (int i = 0; i < NpcPages.Count; i++)
            {
                NpcPage page = NpcPages[i];
                if (!String.Equals(page.Key, key, StringComparison.CurrentCultureIgnoreCase)) continue;

                foreach (NpcSegment segment in page.SegmentList)
                {
                    if (page.BreakFromSegments)
                    {
                        page.BreakFromSegments = false;
                        break;
                    }

                    ProcessSegment(page, segment);
                }
            }
        }
        public void Call(PlayerObject player, uint objectID, string key)
        {
            key = key.ToUpper();

            if (!player.NpcDelayed)
            {
                if (key != MainKey)
                {
                    if (player.NpcObjectID != objectID) return;

                    bool found = false;

                    foreach (NpcSegment segment in player.NpcPage.SegmentList)
                    {
                        if (!player.NpcSuccess.TryGetValue(segment, out bool result)) break; //no result for segment ?

                        if ((result ? segment.Buttons : segment.ElseButtons).Any(s => s.ToUpper() == key))
                        {
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        MessageQueue.Enqueue(string.Format("Player: {0} was prevented access to Npc key: '{1}' ", player.Name, key));
                        return;
                    }
                }
            }
            else
            {
                player.NpcDelayed = false;
            }

            if (key.StartsWith("[@@") && !player.NpcData.TryGetValue("NpcInputStr", out object _npcInputStr))
            {
                //send off packet to request input
                player.Enqueue(new ServerPacket.NpcRequestInput { NpcID = player.NpcObjectID, PageName = key });
                return;
            }

            for (int i = 0; i < NpcPages.Count; i++)
            {
                NpcPage page = NpcPages[i];
                if (!String.Equals(page.Key, key, StringComparison.CurrentCultureIgnoreCase)) continue;

                player.NpcSpeech = new List<string>();
                player.NpcSuccess.Clear();

                foreach (NpcSegment segment in page.SegmentList)
                {
                    if (page.BreakFromSegments)
                    {
                        page.BreakFromSegments = false;
                        break;
                    }

                    ProcessSegment(player, page, segment, objectID);
                }

                Response(player, page);
            }

            player.NpcData.Remove("NpcInputStr");
        }

        private void Response(PlayerObject player, NpcPage page)
        {
            player.Enqueue(new ServerPacket.NpcResponse { Page = player.NpcSpeech });

            ProcessSpecial(player, page);
        }

        private void ProcessSegment(PlayerObject player, NpcPage page, NpcSegment segment, uint objectID)
        {
            player.NpcObjectID = objectID;
            player.NpcScriptID = ScriptID;
            player.NpcSuccess.Add(segment, segment.Check(player));
            player.NpcPage = page;
        }

        private void ProcessSegment(MonsterObject monster, NpcPage page, NpcSegment segment)
        {
            segment.Check(monster);
        }
        private void ProcessSegment(NpcPage page, NpcSegment segment)
        {
            segment.Check();
        }

        private void ProcessSpecial(PlayerObject player, NpcPage page)
        {
            List<UserItem> allGoods = new List<UserItem>();

            var key = page.Key.ToUpper();

            switch (key)
            {
                case BuyKey:
                case BuySellKey:
                    var sentGoods = new List<UserItem>(Goods);

                    for (int i = 0; i < Goods.Count; i++)
                        player.CheckItem(Goods[i]);

                    if (Settings.GoodsOn)
                    {
                        var callingNpc = NpcObject.Get(player.NpcObjectID);

                        if (callingNpc != null)
                        {
                            for (int i = 0; i < callingNpc.UsedGoods.Count; i++)
                                player.CheckItem(callingNpc.UsedGoods[i]);
                        }

                        sentGoods.AddRange(callingNpc.UsedGoods);
                    }

                    player.SendNpcGoods(new ServerPacket.NpcGoods { List = sentGoods, Rate = PriceRate(player), Type = PanelType.Buy, HideAddedStats = Settings.GoodsHideAddedStats });

                    if (key == BuySellKey)
                    {
                        player.Enqueue(new ServerPacket.NpcSell());
                    }
                    break;
                case BuyNewKey:
                case BuySellNewKey:
                    sentGoods = new List<UserItem>(Goods);

                    for (int i = 0; i < Goods.Count; i++)
                        player.CheckItem(Goods[i]);

                    player.SendNpcGoods(new ServerPacket.NpcGoods { List = sentGoods, Rate = PriceRate(player), Type = PanelType.Buy, HideAddedStats = Settings.GoodsHideAddedStats });

                    if (key == BuySellNewKey)
                    {
                        player.Enqueue(new ServerPacket.NpcSell());
                    }
                    break;
                case SellKey:
                    player.Enqueue(new ServerPacket.NpcSell());
                    break;
                case RepairKey:
                    player.Enqueue(new ServerPacket.NpcRepair { Rate = PriceRate(player) });
                    break;
                case SRepairKey:
                    player.Enqueue(new ServerPacket.NpcSRepair { Rate = PriceRate(player) });
                    break;
                case CraftKey:
                    for (int i = 0; i < CraftGoods.Count; i++)
                        player.CheckItemInfo(CraftGoods[i].Item.Info);

                    player.SendNpcGoods(new ServerPacket.NpcGoods { List = (from x in CraftGoods where x.CanCraft(player) select x.Item).ToList(), Rate = PriceRate(player), Type = PanelType.Craft });
                    break;
                case RefineKey:
                    if (player.Info.CurrentRefine != null)
                    {
                        player.ReceiveChat("You're already refining an item.", ChatType.System);
                        player.Enqueue(new ServerPacket.NpcRefine { Rate = (Settings.RefineCost), Refining = true });
                        break;
                    }
                    else
                        player.Enqueue(new ServerPacket.NpcRefine { Rate = (Settings.RefineCost), Refining = false });
                    break;
                case RefineCheckKey:
                    player.Enqueue(new ServerPacket.NpcCheckRefine());
                    break;
                case RefineCollectKey:
                    player.CollectRefine();
                    break;
                case ReplaceWedRingKey:
                    player.Enqueue(new ServerPacket.NpcReplaceWedRing { Rate = Settings.ReplaceWedRingCost });
                    break;
                case StorageKey:
                    player.SendStorage();
                    player.Enqueue(new ServerPacket.NpcStorage());
                    break;
                case BuyBackKey:
                    {
                        if (Settings.GoodsOn)
                        {
                            var callingNpc = NpcObject.Get(player.NpcObjectID);

                            if (callingNpc != null)
                            {
                                if (!callingNpc.BuyBack.ContainsKey(player.Name)) callingNpc.BuyBack[player.Name] = new List<UserItem>();

                                for (int i = 0; i < callingNpc.BuyBack[player.Name].Count; i++)
                                {
                                    player.CheckItem(callingNpc.BuyBack[player.Name][i]);
                                }

                                player.SendNpcGoods(new ServerPacket.NpcGoods { List = callingNpc.BuyBack[player.Name], Rate = PriceRate(player), Type = PanelType.Buy });
                            }
                        }
                    }
                    break;
                case BuyUsedKey:
                    {
                        if (Settings.GoodsOn)
                        {
                            var callingNpc = NpcObject.Get(player.NpcObjectID);

                            if (callingNpc != null)
                            {
                                for (int i = 0; i < callingNpc.UsedGoods.Count; i++)
                                    player.CheckItem(callingNpc.UsedGoods[i]);

                                player.SendNpcGoods(new ServerPacket.NpcGoods { List = callingNpc.UsedGoods, Rate = PriceRate(player), Type = PanelType.BuySub, HideAddedStats = Settings.GoodsHideAddedStats });
                            }
                        }
                    }
                    break;
                case ConsignKey:
                    player.Enqueue(new ServerPacket.NpcConsign());
                    break;
                case MarketKey:
                    player.UserMatch = false;
                    player.GetMarket(string.Empty, ItemType.Nothing);
                    break;
                case GuildCreateKey:
                    if (player.Info.Level < Settings.Guild_RequiredLevel)
                    {
                        player.ReceiveChat(String.Format("You have to be at least level {0} to create a guild.", Settings.Guild_RequiredLevel), ChatType.System);
                    }
                    else if (player.MyGuild == null)
                    {
                        player.CanCreateGuild = true;
                        player.Enqueue(new ServerPacket.GuildNameRequest());
                    }
                    else
                        player.ReceiveChat("You are already part of a guild.", ChatType.System);
                    break;
                case RequestWarKey:
                    if (player.MyGuild != null)
                    {
                        if (player.MyGuildRank != player.MyGuild.Ranks[0])
                        {
                            player.ReceiveChat("You must be the leader to request a war.", ChatType.System);
                            return;
                        }
                        player.Enqueue(new ServerPacket.GuildRequestWar());
                    }
                    else
                    {
                        player.ReceiveChat(GameLanguage.NotInGuild, ChatType.System);
                    }
                    break;
                case SendParcelKey:
                    player.Enqueue(new ServerPacket.MailSendRequest());
                    break;
                case CollectParcelKey:

                    sbyte result = 0;

                    if (player.GetMailAwaitingCollectionAmount() < 1)
                    {
                        result = -1;
                    }
                    else
                    {
                        foreach (var mail in player.Info.Mail)
                        {
                            if (mail.Parcel) mail.Collected = true;
                        }
                    }
                    player.Enqueue(new ServerPacket.ParcelCollected { Result = result });
                    player.GetMail();
                    break;
                case AwakeningKey:
                    player.Enqueue(new ServerPacket.NpcAwakening());
                    break;
                case DisassembleKey:
                    player.Enqueue(new ServerPacket.NpcDisassemble());
                    break;
                case DowngradeKey:
                    player.Enqueue(new ServerPacket.NpcDowngrade());
                    break;
                case ResetKey:
                    player.Enqueue(new ServerPacket.NpcReset());
                    break;
                case PearlBuyKey:
                    for (int i = 0; i < Goods.Count; i++)
                        player.CheckItem(Goods[i]);

                    player.Enqueue(new ServerPacket.NpcPearlGoods { List = Goods, Rate = PriceRate(player), Type = PanelType.Buy });
                    break;
                case HeroCreateKey:
                    if (player.Info.Level < Settings.Hero_RequiredLevel)
                    {
                        player.ReceiveChat(String.Format("You have to be at least level {0} to create a hero.", Settings.Hero_RequiredLevel), ChatType.System);
                        break;
                    }
                    player.CanCreateHero = true;
                    player.Enqueue(new ServerPacket.HeroCreateRequest()
                    {
                        CanCreateClass = Settings.Hero_CanCreateClass
                    });
                    break;
                case HeroManageKey:
                    player.ManageHeroes();
                    break;
            }
        }

        public void Buy(PlayerObject player, ulong index, ushort count)
        {
            UserItem goods = null;

            for (int i = 0; i < Goods.Count; i++)
            {
                if (Goods[i].UniqueID != index) continue;
                goods = Goods[i];
                break;
            }

            bool isUsed = false;
            bool isBuyBack = false;

            var callingNpc = NpcObject.Get(player.NpcObjectID);

            if (callingNpc != null)
            {
                if (goods == null)
                {
                    for (int i = 0; i < callingNpc.UsedGoods.Count; i++)
                    {
                        if (callingNpc.UsedGoods[i].UniqueID != index) continue;
                        goods = callingNpc.UsedGoods[i];
                        isUsed = true;
                        break;
                    }
                }

                if (goods == null)
                {
                    if (!callingNpc.BuyBack.ContainsKey(player.Name)) callingNpc.BuyBack[player.Name] = new List<UserItem>();
                    for (int i = 0; i < callingNpc.BuyBack[player.Name].Count; i++)
                    {
                        if (callingNpc.BuyBack[player.Name][i].UniqueID != index) continue;
                        goods = callingNpc.BuyBack[player.Name][i];
                        isBuyBack = true;
                        break;
                    }
                }
            }

            if (goods == null || count == 0 || count > goods.Info.StackSize) return;

            if ((isBuyBack || isUsed) && count > goods.Count)
                count = goods.Count;
            else
                goods.Count = count;

            uint cost = goods.Price();
            cost = (uint)(cost * PriceRate(player));
            uint baseCost = (uint)(goods.Price() * PriceRate(player, true));

            if (player.NpcPage.Key.ToUpper() == PearlBuyKey)//pearl currency
            {
                if (cost > player.Info.PearlCount) return;
            }
            else if (cost > player.Account.Gold) return;

            UserItem item = (isBuyBack || isUsed) ? goods : Envir.CreateFreshItem(goods.Info);
            item.Count = goods.Count;

            if (!player.CanGainItem(item)) return;

            if (player.NpcPage.Key.ToUpper() == PearlBuyKey)
            {
                player.IntelligentCreatureLosePearls((int)cost);
            }
            else
            {
                player.Account.Gold -= cost;
                player.Enqueue(new ServerPacket.LoseGold { Gold = cost });

                if (callingNpc != null && callingNpc.Conq != null)
                {
                    callingNpc.Conq.GuildInfo.GoldStorage += (cost - baseCost);
                }
            }

            player.GainItem(item);

            if (isUsed)
            {
                callingNpc.UsedGoods.Remove(goods); //If used or buyback will destroy whole stack instead of reducing to remaining quantity

                List<UserItem> newGoodsList = new List<UserItem>();
                newGoodsList.AddRange(Goods);
                newGoodsList.AddRange(callingNpc.UsedGoods);

                callingNpc.NeedSave = true;

                player.SendNpcGoods(new ServerPacket.NpcGoods
                {
                    List = newGoodsList,
                    Rate = PriceRate(player),
                    HideAddedStats = Settings.GoodsHideAddedStats,
                    Type = player.NpcPage.Key.ToUpper() == BuyUsedKey ? PanelType.BuySub : PanelType.Buy
                });
            }

            if (isBuyBack)
            {
                callingNpc.BuyBack[player.Name].Remove(goods); //If used or buyback will destroy whole stack instead of reducing to remaining quantity
                player.SendNpcGoods(new ServerPacket.NpcGoods { List = callingNpc.BuyBack[player.Name], Rate = PriceRate(player), HideAddedStats = false });
            }
        }
        public void Sell(PlayerObject player, UserItem item)
        {
            /* Handle Item Sale */
        }
        public void Craft(PlayerObject player, ulong index, ushort count, int[] slots)
        {
            var p = new ServerPacket.CraftItem();

            RecipeInfo recipe = null;

            for (int i = 0; i < CraftGoods.Count; i++)
            {
                if (CraftGoods[i].Item.UniqueID != index) continue;
                recipe = CraftGoods[i];
                break;
            }

            UserItem goods = recipe.Item;

            if (goods == null || count == 0 || count > goods.Info.StackSize)
            {
                player.Enqueue(p);
                return;
            }

            if (player.Account.Gold < (recipe.Gold * count))
            {
                player.Enqueue(p);
                return;
            }

            bool hasItems = true;

            List<int> usedSlots = new List<int>();

            //Check Tools
            foreach (var tool in recipe.Tools)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    int slot = slots[i];

                    if (usedSlots.Contains(slot)) continue;

                    if (slot < 0 || slot > player.Info.Inventory.Length) continue;

                    UserItem item = player.Info.Inventory[slot];

                    if (item == null || item.Info != tool.Info) continue;

                    usedSlots.Add(slot);

                    if ((uint)Math.Floor(item.CurrentDura / 1000M) < count)
                    {
                        hasItems = false;
                        break;
                    }
                }

                if (!hasItems)
                {
                    break;
                }
            }

            //Check Ingredients
            foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredient.Count * count > ingredient.Info.StackSize)
                {
                    player.Enqueue(p);
                    return;
                }

                ushort amount = (ushort)(ingredient.Count * count);

                for (int i = 0; i < slots.Length; i++)
                {
                    int slot = slots[i];

                    if (usedSlots.Contains(slot)) continue;

                    if (slot < 0 || slot > player.Info.Inventory.Length) continue;

                    UserItem item = player.Info.Inventory[slot];

                    if (item == null || item.Info != ingredient.Info) continue;

                    usedSlots.Add(slot);

                    if (ingredient.CurrentDura < ingredient.MaxDura && ingredient.CurrentDura > item.CurrentDura)
                    {
                        hasItems = false;
                        break;
                    }

                    if (amount > item.Count)
                    {
                        hasItems = false;
                        break;
                    }

                    amount = 0;
                    break;
                }

                if (amount > 0)
                {
                    hasItems = false;
                    break;
                }
            }

            if (!hasItems || usedSlots.Count != (recipe.Tools.Count + recipe.Ingredients.Count))
            {
                player.Enqueue(p);
                return;
            }

            if (count > (goods.Info.StackSize / goods.Count) || count < 1)
            {
                player.Enqueue(p);
                return;
            }

            UserItem craftedItem = Envir.CreateFreshItem(goods.Info);
            craftedItem.Count = (ushort)(goods.Count * count);

            if (!player.CanGainItem(craftedItem))
            {
                player.Enqueue(p);
                return;
            }

            List<int> usedSlots2 = new List<int>();

            //Use Tool Durability
            foreach (var tool in recipe.Tools)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    int slot = slots[i];

                    if (usedSlots2.Contains(slot)) continue;

                    if (slot < 0 || slot > player.Info.Inventory.Length) continue;

                    UserItem item = player.Info.Inventory[slot];

                    if (item == null || item.Info != tool.Info) continue;

                    usedSlots2.Add(slot);

                    player.DamageItem(item, (int)(count * 1000), true);

                    break;
                }
            }

            //Take Ingredients
            foreach (var ingredient in recipe.Ingredients)
            {
                ushort amount = (ushort)(ingredient.Count * count);

                for (int i = 0; i < slots.Length; i++)
                {
                    int slot = slots[i];

                    if (usedSlots2.Contains(slot)) continue;

                    if (slot < 0 || slot > player.Info.Inventory.Length) continue;

                    UserItem item = player.Info.Inventory[slot];

                    if (item == null || item.Info != ingredient.Info) continue;

                    usedSlots2.Add(slot);

                    if (item.Count > amount)
                    {
                        player.Enqueue(new ServerPacket.DeleteItem { UniqueID = item.UniqueID, Count = amount });
                        player.Info.Inventory[slot].Count -= amount;
                        break;
                    }
                    else
                    {
                        player.Enqueue(new ServerPacket.DeleteItem { UniqueID = item.UniqueID, Count = item.Count });
                        amount -= item.Count;
                        player.Info.Inventory[slot] = null;
                    }

                    break;
                }
            }

            //Take Gold
            player.Account.Gold -= (recipe.Gold * count);
            player.Enqueue(new ServerPacket.LoseGold { Gold = (recipe.Gold * count) });

            if (Envir.Random.Next(100) >= recipe.Chance + player.Stats[Stat.CraftRatePercent])
            {
                player.ReceiveChat("Crafting attempt failed.", ChatType.System);
            }
            else
            {
                //Give Item
                player.GainItem(craftedItem);
            }

            p.Success = true;
            player.Enqueue(p);
        }
    }

    public enum NpcScriptType
    {
        Normal,
        Called,
        AutoPlayer,
        AutoMonster,
        Robot
    }
}