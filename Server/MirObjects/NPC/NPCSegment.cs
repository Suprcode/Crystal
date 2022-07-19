using Server.MirDatabase;
using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Server.MirObjects.Actions;
using Server.MirObjects.Checks;
using S = ServerPackets;

namespace Server.MirObjects
{
    public class NPCSegment
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        protected static MessageQueue MessageQueue
        {
            get { return MessageQueue.Instance; }
        }

        public NPCPage Page;

        public readonly string Key;
        public List<NPCChecks> CheckList = new List<NPCChecks>();
        public List<NPCActions> ActList = new List<NPCActions>(), ElseActList = new List<NPCActions>();
        public List<string> Say, ElseSay, Buttons, ElseButtons, GotoButtons;

        public string Param1;
        public int Param1Instance, Param2, Param3;

        public List<string> Args = new List<string>();
        protected List<NPCCheck> Checks = new List<NPCCheck>();
        public List<NPCAction> Actions = new List<NPCAction>(), ElseActions = new List<NPCAction>();
        protected Dictionary<string, Type> CheckTypeCommands, ActionTypeCommands;
        public NPCSegment(NPCPage page, List<string> say, List<string> buttons, List<string> elseSay, List<string> elseButtons, List<string> gotoButtons)
        {
            Page = page;

            Say = say;
            Buttons = buttons;

            ElseSay = elseSay;
            ElseButtons = elseButtons;

            GotoButtons = gotoButtons;
            InitializeCheckTypes();
            InitializeActionTypes();
        }
        private void InitializeCheckTypes()
        {
            CheckTypeCommands = new Dictionary<string, Type>();
            var types = typeof(NPCCheck).Assembly.GetTypes().Where(type => type.BaseType != null && !type.IsAbstract && type.BaseType == typeof(NPCCheck)).ToList();
            CheckTypeCommands = types.ToDictionary(NPCCheck.GetCommand);
        }

        private void InitializeActionTypes()
        {
            ActionTypeCommands = new Dictionary<string, Type>();
            var types = typeof(NPCCheck).Assembly.GetTypes().Where(type => type.BaseType != null && !type.IsAbstract && type.BaseType == typeof(NPCAction)).ToList();
            foreach (var type in types)
            {
                if (ActionTypeCommands.TryGetValue(NPCAction.GetCommand(type), out var val))
                {
                    MessageQueue.Enqueue("Duplicate Action Command: " + NPCAction.GetCommand(type));
                }
                else
                    ActionTypeCommands.Add(NPCAction.GetCommand(type), type);
            }
        }

        public string[] ParseArguments(string[] words)
        {
            Regex r = new Regex(@"\%ARG\((\d+)\)");

            for (int i = 0; i < words.Length; i++)
            {
                foreach (Match m in r.Matches(words[i].ToUpper()))
                {
                    if (!m.Success) continue;

                    int sequence = Convert.ToInt32(m.Groups[1].Value);

                    if (Page.Args.Count >= (sequence + 1)) words[i] = words[i].Replace(m.Groups[0].Value, Page.Args[sequence]);
                }
            }

            return words;
        }

        public void AddVariable(MapObject player, string key, string value)
        {
            Regex regex = new Regex(@"[A-Za-z][0-9]");

            if (!regex.Match(key).Success) return;

            for (int i = 0; i < player.NPCVar.Count; i++)
            {
                if (!String.Equals(player.NPCVar[i].Key, key, StringComparison.CurrentCultureIgnoreCase)) continue;
                player.NPCVar[i] = new KeyValuePair<string, string>(player.NPCVar[i].Key, value);
                return;
            }

            player.NPCVar.Add(new KeyValuePair<string, string>(key, value));
        }

        public string FindVariable(MapObject player, string key)
        {
            Regex regex = new Regex(@"\%[A-Za-z][0-9]");

            if (!regex.Match(key).Success) return key;

            string tempKey = key.Substring(1);

            foreach (KeyValuePair<string, string> t in player.NPCVar)
            {
                if (String.Equals(t.Key, tempKey, StringComparison.CurrentCultureIgnoreCase)) return t.Value;
            }

            return key;
        }
        protected static readonly Dictionary<Type, Func<string, string[], NPCCheck>> _CheckCreators = new Dictionary<Type, Func<string, string[], NPCCheck>>();
        protected static readonly Dictionary<Type, Func<NPCSegment, string, string[], NPCAction>> _ActionCreators = new Dictionary<Type, Func<NPCSegment, string, string[], NPCAction>>(); 

        /// <summary>
        /// Cache Constructors for NPCCheck subclasses.
        /// <para>Without caching the constructors it'll likely have an impact on Startup/Reload time.</para>
        /// </summary>
        /// <param name="type">The Type of the NPCCheck</param>
        /// <param name="line">The Line provided</param>
        /// <param name="parts">The Line split into Parts</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">If the constructor for the type cannot be found, this exception will be thrown.</exception>
        protected virtual NPCCheck CreateCheck(Type type, string line, string[] parts)
        {
            if (_CheckCreators.TryGetValue(type, out var constructor))
                return constructor?.Invoke(line, parts);
            var typeConstructor = type.GetConstructor(new[] { typeof(string), typeof(string[]) });
            if (typeConstructor is null)
                throw new NullReferenceException(nameof(typeConstructor));
            var parameter1 = Expression.Parameter(typeof(string), "line");
            var parameter2 = Expression.Parameter(typeof(string[]), "parts");
            var constructorExpression = Expression.New(typeConstructor, parameter1, parameter2);
            var lambda = Expression.Lambda<Func<string, string[], NPCCheck>>(constructorExpression, parameter1, parameter2);
            var result = lambda.Compile();
            _CheckCreators.Add(type, result);
            return result.Invoke(line, parts);
        }

        protected virtual NPCAction CreateAction(Type type, string line, string[] parts)
        {
            if (_ActionCreators.TryGetValue(type, out var constructor))
                return constructor?.Invoke(this, line, parts);
            var typeConstructor = type.GetConstructor(new[] { typeof(NPCSegment), typeof(string), typeof(string[]) });
            if (typeConstructor is null)
                throw new NullReferenceException(nameof(typeConstructor));
            var parameter1 = Expression.Parameter(typeof(NPCSegment), "segment");
            var parameter2 = Expression.Parameter(typeof(string), "line");
            var parameter3 = Expression.Parameter(typeof(string[]), "parts");
            var constructorExpression = Expression.New(typeConstructor, parameter1, parameter2, parameter3);
            var lambda = Expression.Lambda<Func<NPCSegment, string, string[], NPCAction>>(constructorExpression, parameter1, parameter2, parameter3);
            var result = lambda.Compile();
            _ActionCreators.Add(type, result);
            return result.Invoke(this, line, parts);
        }

        public void ParseCheck(string line)
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            parts = ParseArguments(parts);

            if (parts.Length == 0) return;

            if (!CheckTypeCommands.TryGetValue(parts[0].ToUpper(), out var type))
                return;
            Checks.Add(CreateCheck(type, line, parts));
        }
        public void ParseAct(List<NPCAction> acts, string line)
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            parts = ParseArguments(parts);

            if (parts.Length == 0) return;

            if (!ActionTypeCommands.TryGetValue(parts[0].ToUpper(), out var type))
                return;
            acts.Add(CreateAction(type, line, parts));
        }

        public List<string> ParseSay(PlayerObject player, List<string> speech)
        {
            for (var i = 0; i < speech.Count; i++)
            {
                var parts = speech[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0) continue;

                foreach (var part in parts)
                {
                    speech[i] = speech[i].Replace(part, ReplaceValue(player, part));
                }
            }
            return speech;
        }

        public string ReplaceValue(PlayerObject player, string param)
        {
            var regex = new Regex(@"\<\$(.*)\>");
            var varRegex = new Regex(@"(.*?)\(([A-Z][0-9])\)");
            var oneValRegex = new Regex(@"(.*?)\(((.*?))\)");
            var twoValRegex = new Regex(@"(.*?)\(((.*?),(.*?))\)");
            ConquestObject Conquest;
            ConquestGuildArcherInfo Archer;
            ConquestGuildGateInfo Gate;
            ConquestGuildWallInfo Wall;
            ConquestGuildSiegeInfo Siege;

            var match = regex.Match(param);

            if (!match.Success) return param;

            string innerMatch = match.Groups[1].Captures[0].Value.ToUpper();

            Match varMatch = varRegex.Match(innerMatch);
            Match oneValMatch = oneValRegex.Match(innerMatch);
            Match twoValMatch = twoValRegex.Match(innerMatch);

            if (varRegex.Match(innerMatch).Success)
                innerMatch = innerMatch.Replace(varMatch.Groups[2].Captures[0].Value.ToUpper(), "");
            else if (twoValRegex.Match(innerMatch).Success)
                innerMatch = innerMatch.Replace(twoValMatch.Groups[2].Captures[0].Value.ToUpper(), "");
            else if (oneValRegex.Match(innerMatch).Success)
                innerMatch = innerMatch.Replace(oneValMatch.Groups[2].Captures[0].Value.ToUpper(), "");

            string newValue = string.Empty;

            switch (innerMatch)
            {
                case "MONSTERCOUNT()":
                    Map map = Envir.GetMapByNameAndInstance(oneValMatch.Groups[2].Captures[0].Value.ToUpper());
                    newValue = map == null ? "N/A" : map.MonsterCount.ToString();
                    break;
                case "CONQUESTGUARD()":
                    var val1 = FindVariable(player, "%" + twoValMatch.Groups[3].Captures[0].Value.ToUpper());
                    var val2 = FindVariable(player, "%" + twoValMatch.Groups[4].Captures[0].Value.ToUpper());

                    int intVal1, intVal2;

                    if (int.TryParse(val1.Replace("%", ""), out intVal1) && int.TryParse(val2.Replace("%", ""), out intVal2))
                    {

                        Conquest = Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return "Not Found";

                        Archer = Conquest.ArcherList.FirstOrDefault(x => x.Index == intVal2);
                        if (Archer == null) return "Not Found";

                        if (Archer.Info.Name == "" || Archer.Info.Name == null)
                            newValue = "Conquest Guard";
                        else
                            newValue = Archer.Info.Name;

                        if (Archer.GetRepairCost() == 0)
                            newValue += " - [ Still Alive ]";
                        else
                            newValue += " - [ " + Archer.GetRepairCost().ToString("#,##0") + " gold ]";
                    }
                    break;
                case "CONQUESTGATE()":
                    val1 = FindVariable(player, "%" + twoValMatch.Groups[3].Captures[0].Value.ToUpper());
                    val2 = FindVariable(player, "%" + twoValMatch.Groups[4].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1) && int.TryParse(val2.Replace("%", ""), out intVal2))
                    {
                        Conquest = Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return "Not Found";

                        Gate = Conquest.GateList.FirstOrDefault(x => x.Index == intVal2);
                        if (Gate == null) return "Not Found";

                        if (Gate.Info.Name == "" || Gate.Info.Name == null)
                            newValue = "Conquest Gate";
                        else
                            newValue = Gate.Info.Name;

                        if (Gate.GetRepairCost() == 0)
                            newValue += " - [ No Repair Required ]";
                        else
                            newValue += " - [ " + Gate.GetRepairCost().ToString("#,##0") + " gold ]";
                    }
                    break;
                case "CONQUESTWALL()":
                    val1 = FindVariable(player, "%" + twoValMatch.Groups[3].Captures[0].Value.ToUpper());
                    val2 = FindVariable(player, "%" + twoValMatch.Groups[4].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1) && int.TryParse(val2.Replace("%", ""), out intVal2))
                    {
                        Conquest = Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return "Not Found";

                        Wall = Conquest.WallList.FirstOrDefault(x => x.Index == intVal2);
                        if (Wall == null) return "Not Found";

                        if (Wall.Info.Name == "" || Wall.Info.Name == null)
                            newValue = "Conquest Wall";
                        else
                            newValue = Wall.Info.Name;

                        if (Wall.GetRepairCost() == 0)
                            newValue += " - [ No Repair Required ]";
                        else
                            newValue += " - [ " + Wall.GetRepairCost().ToString("#,##0") + " gold ]";
                    }
                    break;
                case "CONQUESTSIEGE()":
                    val1 = FindVariable(player, "%" + twoValMatch.Groups[3].Captures[0].Value.ToUpper());
                    val2 = FindVariable(player, "%" + twoValMatch.Groups[4].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1) && int.TryParse(val2.Replace("%", ""), out intVal2))
                    {
                        Conquest = Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return "Not Found";

                        Siege = Conquest.SiegeList.FirstOrDefault(x => x.Index == intVal2);
                        if (Siege == null) return "Not Found";

                        if (Siege.Info.Name == "" || Siege.Info.Name == null)
                            newValue = "Conquest Siege";
                        else
                            newValue = Siege.Info.Name;

                        if (Siege.GetRepairCost() == 0)
                            newValue += " - [ Still Alive ]";
                        else
                            newValue += " - [ " + Siege.GetRepairCost().ToString("#,##0") + " gold ]";
                    }
                    break;
                case "CONQUESTOWNER()":
                    val1 = FindVariable(player, "%" + oneValMatch.Groups[2].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1))
                    {
                        Conquest = Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return string.Empty;
                        if (Conquest.Guild == null) return "No Owner";

                        newValue = Conquest.Guild.Name;
                    }
                    break;
                case "CONQUESTGOLD()":
                    val1 = FindVariable(player, "%" + oneValMatch.Groups[2].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1))
                    {
                        Conquest = Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return string.Empty;

                        newValue = Conquest.GuildInfo.GoldStorage.ToString();
                    }
                    break;
                case "CONQUESTRATE()":
                    val1 = FindVariable(player, "%" + oneValMatch.Groups[2].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1))
                    {
                        Conquest = Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return string.Empty;

                        newValue = Conquest.GuildInfo.NPCRate.ToString() + "%";
                    }
                    break;
                case "CONQUESTSCHEDULE()":
                    val1 = FindVariable(player, "%" + oneValMatch.Groups[2].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1))
                    {
                        Conquest = Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return "Conquest Not Found";
                        if (Conquest.GuildInfo.AttackerID == -1) return "No War Scheduled";

                        if (Envir.Guilds.FirstOrDefault(x => x.Guildindex == Conquest.GuildInfo.AttackerID) == null)
                        {
                            newValue = "No War Scheduled";
                        }
                        else
                        {
                            newValue = (Envir.Guilds.FirstOrDefault(x => x.Guildindex == Conquest.GuildInfo.AttackerID).Name);
                        }
                    }
                    break;
                case "OUTPUT()":
                    newValue = FindVariable(player, "%" + varMatch.Groups[2].Captures[0].Value.ToUpper());
                    break;
                case "NPCNAME":
                    for (int i = 0; i < player.CurrentMap.NPCs.Count; i++)
                    {
                        NPCObject ob = player.CurrentMap.NPCs[i];
                        if (ob.ObjectID != player.NPCObjectID) continue;
                        newValue = ob.Name.Replace("_", " ");
                    }
                    break;
                case "USERNAME":
                    newValue = player.Name;
                    break;
                case "LEVEL":
                    newValue = player.Level.ToString(CultureInfo.InvariantCulture);
                    break;
                case "CLASS":
                    newValue = player.Class.ToString();
                    break;
                case "MAP":
                    newValue = player.CurrentMap.Info.FileName;
                    break;
                case "X_COORD":
                    newValue = player.CurrentLocation.X.ToString();
                    break;
                case "Y_COORD":
                    newValue = player.CurrentLocation.Y.ToString();
                    break;
                case "HP":
                    newValue = player.HP.ToString(CultureInfo.InvariantCulture);
                    break;
                case "MAXHP":
                    newValue = player.Stats[Stat.HP].ToString(CultureInfo.InvariantCulture);
                    break;
                case "MP":
                    newValue = player.MP.ToString(CultureInfo.InvariantCulture);
                    break;
                case "MAXMP":
                    newValue = player.Stats[Stat.MP].ToString(CultureInfo.InvariantCulture);
                    break;
                case "GAMEGOLD":
                    newValue = player.Account.Gold.ToString(CultureInfo.InvariantCulture);
                    break;
                case "CREDIT":
                    newValue = player.Account.Credit.ToString(CultureInfo.InvariantCulture);
                    break;
                case "ARMOUR":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Armour] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Armour].FriendlyName : "No Armour";
                    break;
                case "WEAPON":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Weapon] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Weapon].FriendlyName : "No Weapon";
                    break;
                case "RING_L":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.RingL] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.RingL].FriendlyName : "No Ring";
                    break;
                case "RING_R":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.RingR] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.RingR].FriendlyName : "No Ring";
                    break;
                case "BRACELET_L":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.BraceletL] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.BraceletL].FriendlyName : "No Bracelet";
                    break;
                case "BRACELET_R":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.BraceletR] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.BraceletR].FriendlyName : "No Bracelet";
                    break;
                case "NECKLACE":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Necklace] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Necklace].FriendlyName : "No Necklace";
                    break;
                case "BELT":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Belt] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Belt].FriendlyName : "No Belt";
                    break;
                case "BOOTS":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Boots] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Boots].FriendlyName : "No Boots";
                    break;
                case "HELMET":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Helmet] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Helmet].FriendlyName : "No Helmet";
                    break;
                case "AMULET":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Amulet] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Amulet].FriendlyName : "No Amulet";
                    break;
                case "STONE":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Stone] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Stone].FriendlyName : "No Stone";
                    break;
                case "TORCH":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Torch] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Torch].FriendlyName : "No Torch";
                    break;

                case "DATE":
                    newValue = Envir.Now.ToShortDateString();
                    break;
                case "USERCOUNT":
                    newValue = Envir.PlayerCount.ToString(CultureInfo.InvariantCulture);
                    break;
                case "PKPOINT":
                    newValue = player.PKPoints.ToString();
                    break;
                case "GUILDWARTIME":
                    newValue = Settings.Guild_WarTime.ToString();
                    break;
                case "GUILDWARFEE":
                    newValue = Settings.Guild_WarCost.ToString();
                    break;

                case "PARCELAMOUNT":
                    newValue = player.GetMailAwaitingCollectionAmount().ToString();
                    break;
                case "GUILDNAME":
                    if (player.MyGuild == null) return "No Guild";
                    else
                        newValue = player.MyGuild.Name + " Guild";
                    break;
                case "ROLLRESULT":
                    newValue = player.NPCData.TryGetValue("NPCRollResult", out object _rollResult) ? _rollResult.ToString() : "Not Rolled";
                    break;

                default:
                    newValue = string.Empty;
                    break;
            }

            if (string.IsNullOrEmpty(newValue)) return param;

            return param.Replace(match.Value, newValue);
        }
        public string ReplaceValue(MonsterObject Monster, string param)
        {
            var regex = new Regex(@"\<\$(.*)\>");
            var varRegex = new Regex(@"(.*?)\(([A-Z][0-9])\)");

            var match = regex.Match(param);

            if (!match.Success) return param;

            string innerMatch = match.Groups[1].Captures[0].Value.ToUpper();

            Match varMatch = varRegex.Match(innerMatch);

            if (varRegex.Match(innerMatch).Success)
                innerMatch = innerMatch.Replace(varMatch.Groups[2].Captures[0].Value.ToUpper(), "");

            string newValue = string.Empty;

            switch (innerMatch)
            {
                case "OUTPUT()":
                    newValue = FindVariable(Monster, "%" + varMatch.Groups[2].Captures[0].Value.ToUpper());
                    break;
                case "USERNAME":
                    newValue = Monster.Name;
                    break;
                case "LEVEL":
                    newValue = Monster.Level.ToString(CultureInfo.InvariantCulture);
                    break;
                case "MAP":
                    newValue = Monster.CurrentMap.Info.FileName;
                    break;
                case "X_COORD":
                    newValue = Monster.CurrentLocation.X.ToString();
                    break;
                case "Y_COORD":
                    newValue = Monster.CurrentLocation.Y.ToString();
                    break;
                case "HP":
                    newValue = Monster.HP.ToString(CultureInfo.InvariantCulture);
                    break;
                case "MAXHP":
                    newValue = Monster.Stats[Stat.HP].ToString(CultureInfo.InvariantCulture);
                    break;
                case "DATE":
                    newValue = Envir.Now.ToShortDateString();
                    break;
                case "USERCOUNT":
                    newValue = Envir.PlayerCount.ToString(CultureInfo.InvariantCulture);
                    break;
                case "GUILDWARTIME":
                    newValue = Settings.Guild_WarTime.ToString();
                    break;
                case "GUILDWARFEE":
                    newValue = Settings.Guild_WarCost.ToString();
                    break;
                default:
                    newValue = string.Empty;
                    break;
            }

            if (string.IsNullOrEmpty(newValue)) return param;

            return param.Replace(match.Value, newValue);
        }

        public bool Check()
        {
            var failed = false;

            foreach (var npcCheck in Checks)
            {
                if (!npcCheck.Check(null))
                    failed = true;
                if (!failed) continue;
                Failed();
                return false;
            }
            Success();
            return true;
        }
        public bool Check(MonsterObject monster)
        {
            var failed = false;
            foreach (var npcCheck in Checks)
            {
                if (!npcCheck.Check(monster))
                    failed = true;
                if (!failed) continue;
                Failed(monster);
                return false;
            }
            Success(monster);
            return true;
        }
        public bool Check(PlayerObject player)
        {
            var failed = false;
            foreach (var npcCheck in Checks)
            {
                List<string> param = npcCheck.Parts.Select(t => FindVariable(player, t)).ToList();

                for (int j = 0; j < param.Count; j++)
                {
                    var parts = param[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 0) continue;

                    foreach (var part in parts)
                    {
                        param[j] = param[j].Replace(part, ReplaceValue(player, part));
                    }
                }
                if (!npcCheck.Check(player))
                    failed = true;
                if (!failed) continue;
                Failed(player);
                return false;
            }
            Success(player);
            return true;
        }

        private void Act(IList<NPCAction> acts)
        {
            foreach (var npcAction in acts)
            {
                npcAction.Execute(null);
            }
        }
        private void Act(IList<NPCAction> acts, PlayerObject player)
        {
            MailInfo mailInfo = null;
            foreach (var npcAction in acts)
            {
                var param = npcAction.Parts.Skip(1).Select(t => FindVariable(player, t)).ToList();

                for (int j = 0; j < param.Count; j++)
                {
                    var parts = param[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 0) continue;

                    foreach (var part in parts)
                    {
                        param[j] = param[j].Replace(part, ReplaceValue(player, part));
                    }

                    if (player.NPCData.TryGetValue("NPCInputStr", out object _npcInputStr))
                    {
                        param[j] = param[j].Replace("%INPUTSTR", (string)_npcInputStr);
                    }
                }
                switch (npcAction)
                {
                    case ActionAddMailGold mailGold:
                        if (mailInfo is null) continue;
                        mailGold.SetMailInfo(mailInfo);
                        break;
                    case ActionAddMailItem mailItem:
                        if (mailInfo is null) continue;
                        mailItem.SetMailInfo(mailInfo);
                        break;
                    case ActionSendMail send:
                        if (mailInfo is null) continue;
                        send.SetMailInfo(mailInfo);
                        mailInfo = null;
                        break;
                }
                npcAction.Execute(player);
                
                switch (npcAction)
                {
                    case ActionComposeMail compose:
                        mailInfo = compose.MailInfo;
                        break;
                }
            }
        }
        private void Act(IList<NPCAction> acts, MonsterObject monster)
        {
            for (var i = 0; i < acts.Count; i++)
            {
                var act = acts[i];
                List<string> param = act.Parts.Skip(1).Select(t => FindVariable(monster, t)).ToList();

                for (int j = 0; j < param.Count; j++)
                {
                    var parts = param[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 0) continue;

                    foreach (var part in parts)
                    {
                        param[j] = param[j].Replace(part, ReplaceValue(monster, part));
                    }
                }
                act.Execute(monster);
            }
        }

        private void Success(PlayerObject player)
        {
            Act(Actions, player);

            var parseSay = new List<String>(Say);
            parseSay = ParseSay(player, parseSay);

            player.NPCSpeech.AddRange(parseSay);
        }

        private void Failed(PlayerObject player)
        {
            Act(ElseActions, player);

            var parseElseSay = new List<String>(ElseSay);
            parseElseSay = ParseSay(player, parseElseSay);

            player.NPCSpeech.AddRange(parseElseSay);
        }

        private void Success(MonsterObject Monster)
        {
            Act(Actions, Monster);
        }

        private void Failed(MonsterObject Monster)
        {
            Act(ElseActions, Monster);
        }

        private void Success()
        {
            Act(Actions);
        }

        private void Failed()
        {
            Act(ElseActions);
        }

        public static int Calculate(string op, int left, int right)
        {
            switch (op)
            {
                case "+": return left + right;
                case "-": return left - right;
                case "*": return left * right;
                case "/": return left / right;
                default: throw new ArgumentException("Invalid sum operator: {0}", op);
            }
        }
    }
}