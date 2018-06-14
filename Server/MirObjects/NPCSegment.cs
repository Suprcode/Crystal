using Server.MirDatabase;
using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using S = ServerPackets;

namespace Server.MirObjects
{
    public class NPCSegment
    {
        public NPCPage Page;

        public readonly string Key;
        public List<NPCChecks> CheckList = new List<NPCChecks>();
        public List<NPCActions> ActList = new List<NPCActions>(), ElseActList = new List<NPCActions>();
        public List<string> Say, ElseSay, Buttons, ElseButtons, GotoButtons;

        public string Param1;
        public int Param1Instance, Param2, Param3;

        public List<string> Args = new List<string>();

        public NPCSegment(NPCPage page, List<string> say, List<string> buttons, List<string> elseSay, List<string> elseButtons, List<string> gotoButtons)
        {
            Page = page;

            Say = say;
            Buttons = buttons;

            ElseSay = elseSay;
            ElseButtons = elseButtons;

            GotoButtons = gotoButtons;
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
        public string FindVariable(string key)
        {
            Regex regex = new Regex(@"\%[A-Za-z][0-9]");

            if (!regex.Match(key).Success) return key;

            string tempKey = key.Substring(1);
            /* //hoping i truly dont need that :p
            foreach (KeyValuePair<string, string> t in player.NPCVar)
            {
                if (String.Equals(t.Key, tempKey, StringComparison.CurrentCultureIgnoreCase)) return t.Value;
            }
            */
            return key;
        }


        public void ParseCheck(string line)
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            parts = ParseArguments(parts);

            if (parts.Length == 0) return;

            string tempString, tempString2;

            var regexFlag = new Regex(@"\[(.*?)\]");
            var regexQuote = new Regex("\"([^\"]*)\"");

            Match quoteMatch = null;

            switch (parts[0].ToUpper())
            {
                case "LEVEL":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.Level, parts[1], parts[2]));
                    break;

                case "CHECKGOLD":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckGold, parts[1], parts[2]));
                    break;
                case "CHECKGUILDGOLD":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckGuildGold, parts[1], parts[2]));
                    break;
                case "CHECKCREDIT":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckCredit, parts[1], parts[2]));
                    break;
                case "CHECKITEM":
                    if (parts.Length < 2) return;

                    tempString = parts.Length < 3 ? "1" : parts[2];
                    tempString2 = parts.Length > 3 ? parts[3] : "";

                    CheckList.Add(new NPCChecks(CheckType.CheckItem, parts[1], tempString, tempString2));
                    break;

                case "CHECKGENDER":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckGender, parts[1]));
                    break;

                case "CHECKCLASS":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckClass, parts[1]));
                    break;

                case "DAYOFWEEK":
                    if (parts.Length < 2) return;
                    CheckList.Add(new NPCChecks(CheckType.CheckDay, parts[1]));
                    break;

                case "HOUR":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckHour, parts[1]));
                    break;

                case "MIN":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckMinute, parts[1]));
                    break;

                //cant use stored var
                case "CHECKNAMELIST":
                    if (parts.Length < 2) return;

                    quoteMatch = regexQuote.Match(line);

                    string listPath = parts[1];

                    if (quoteMatch.Success)
                        listPath = quoteMatch.Groups[1].Captures[0].Value;

                    var fileName = Settings.NameListPath + listPath;

                    string sDirectory = Path.GetDirectoryName(fileName);
                    Directory.CreateDirectory(sDirectory);

                    if (File.Exists(fileName))
                        CheckList.Add(new NPCChecks(CheckType.CheckNameList, fileName));
                    break;

                //cant use stored var
                case "CHECKGUILDNAMELIST":
                    if (parts.Length < 2) return;

                    quoteMatch = regexQuote.Match(line);

                    listPath = parts[1];

                    if (quoteMatch.Success)
                        listPath = quoteMatch.Groups[1].Captures[0].Value;

                    fileName = Settings.NameListPath + listPath;

                    sDirectory = Path.GetDirectoryName(fileName);
                    Directory.CreateDirectory(sDirectory);

                    if (File.Exists(fileName))
                        CheckList.Add(new NPCChecks(CheckType.CheckGuildNameList, fileName));
                    break;
                case "ISADMIN":
                    CheckList.Add(new NPCChecks(CheckType.IsAdmin));
                    break;

                case "CHECKPKPOINT":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckPkPoint, parts[1], parts[2]));
                    break;

                case "CHECKRANGE":
                    if (parts.Length < 4) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckRange, parts[1], parts[2], parts[3]));
                    break;

                case "CHECKMAP":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckMap, parts[1]));
                    break;

                //cant use stored var
                case "CHECK":
                    if (parts.Length < 3) return;
                    var match = regexFlag.Match(parts[1]);
                    if (match.Success)
                    {
                        string flagIndex = match.Groups[1].Captures[0].Value;
                        CheckList.Add(new NPCChecks(CheckType.Check, flagIndex, parts[2]));
                    }
                    break;

                case "CHECKHUM":
                    if (parts.Length < 4) return;

                    tempString = parts.Length < 5 ? "1" : parts[4];
                    CheckList.Add(new NPCChecks(CheckType.CheckHum, parts[1], parts[2], parts[3], tempString));
                    break;

                case "CHECKMON":
                    if (parts.Length < 4) return;

                    tempString = parts.Length < 5 ? "1" : parts[4];
                    CheckList.Add(new NPCChecks(CheckType.CheckMon, parts[1], parts[2], parts[3], tempString));
                    break;

                case "CHECKEXACTMON":
                    if (parts.Length < 5) return;

                    tempString = parts.Length < 6 ? "1" : parts[5];
                    CheckList.Add(new NPCChecks(CheckType.CheckExactMon, parts[1], parts[2], parts[3], parts[4], tempString));
                    break;

                case "RANDOM":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.Random, parts[1]));
                    break;

                case "GROUPLEADER":
                    CheckList.Add(new NPCChecks(CheckType.Groupleader));
                    break;

                case "GROUPCOUNT":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.GroupCount, parts[1], parts[2]));
                    break;

                case "GROUPCHECKNEARBY":
                    CheckList.Add(new NPCChecks(CheckType.GroupCheckNearby));
                    break;

                case "PETCOUNT":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.PetCount, parts[1], parts[2]));
                    break;

                case "PETLEVEL":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.PetLevel, parts[1], parts[2]));
                    break;

                case "CHECKCALC":
                    if (parts.Length < 4) return;
                    CheckList.Add(new NPCChecks(CheckType.CheckCalc, parts[1], parts[2], parts[3]));
                    break;

                case "INGUILD":
                    string guildName = string.Empty;

                    if (parts.Length > 1) guildName = parts[1];

                    CheckList.Add(new NPCChecks(CheckType.InGuild, guildName));
                    break;

                case "CHECKQUEST":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckQuest, parts[1], parts[2]));
                    break;
                case "CHECKRELATIONSHIP":
                    CheckList.Add(new NPCChecks(CheckType.CheckRelationship));
                    break;
                case "CHECKWEDDINGRING":
                    CheckList.Add(new NPCChecks(CheckType.CheckWeddingRing));
                    break;

                case "CHECKPET":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckPet, parts[1]));
                    break;

                case "HASBAGSPACE":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.HasBagSpace, parts[1], parts[2]));
                    break;
                case "ISNEWHUMAN":
                    CheckList.Add(new NPCChecks(CheckType.IsNewHuman));
                    break;
                case "CHECKCONQUEST":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckConquest, parts[1]));
                    break;
                case "AFFORDGUARD":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.AffordGuard, parts[1], parts[2]));
                    break;
                case "AFFORDGATE":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.AffordGate, parts[1], parts[2]));
                    break;
                case "AFFORDWALL":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.AffordWall, parts[1], parts[2]));
                    break;
                case "AFFORDSIEGE":
                    if (parts.Length < 3) return;

                    CheckList.Add(new NPCChecks(CheckType.AffordSiege, parts[1], parts[2]));
                    break;
                case "CHECKPERMISSION":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.CheckPermission, parts[1]));
                    break;
                case "CONQUESTAVAILABLE":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.ConquestAvailable, parts[1]));
                    break;
                case "CONQUESTOWNER":
                    if (parts.Length < 2) return;

                    CheckList.Add(new NPCChecks(CheckType.ConquestOwner, parts[1]));
                    break;
            }

        }
        public void ParseAct(List<NPCActions> acts, string line)
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            parts = ParseArguments(parts);

            if (parts.Length == 0) return;

            string fileName;
            var regexQuote = new Regex("\"([^\"]*)\"");
            var regexFlag = new Regex(@"\[(.*?)\]");

            Match quoteMatch = null;

            switch (parts[0].ToUpper())
            {
                case "MOVE":
                    if (parts.Length < 2) return;

                    string tempx = parts.Length > 3 ? parts[2] : "0";
                    string tempy = parts.Length > 3 ? parts[3] : "0";

                    acts.Add(new NPCActions(ActionType.Move, parts[1], tempx, tempy));
                    break;

                case "INSTANCEMOVE":
                    if (parts.Length < 5) return;

                    acts.Add(new NPCActions(ActionType.InstanceMove, parts[1], parts[2], parts[3], parts[4]));
                    break;

                case "GIVEGOLD":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.GiveGold, parts[1]));
                    break;

                case "TAKEGOLD":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.TakeGold, parts[1]));
                    break;
                case "GIVEGUILDGOLD":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.GiveGuildGold, parts[1]));
                    break;
                case "TAKEGUILDGOLD":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.TakeGuildGold, parts[1]));
                    break;
                case "GIVECREDIT":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.GiveCredit, parts[1]));
                    break;
                case "TAKECREDIT":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.TakeCredit, parts[1]));
                    break;

                case "GIVEPEARLS":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.GivePearls, parts[1]));
                    break;

                case "TAKEPEARLS":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.TakePearls, parts[1]));
                    break;

                case "GIVEITEM":
                    if (parts.Length < 2) return;

                    string count = parts.Length < 3 ? string.Empty : parts[2];
                    acts.Add(new NPCActions(ActionType.GiveItem, parts[1], count));
                    break;

                case "TAKEITEM":
                    if (parts.Length < 3) return;

                    count = parts.Length < 3 ? string.Empty : parts[2];
                    string dura = parts.Length > 3 ? parts[3] : "";

                    acts.Add(new NPCActions(ActionType.TakeItem, parts[1], count, dura));
                    break;

                case "GIVEEXP":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.GiveExp, parts[1]));
                    break;

                case "GIVEPET":
                    if (parts.Length < 2) return;

                    string petcount = parts.Length > 2 ? parts[2] : "1";
                    string petlevel = parts.Length > 3 ? parts[3] : "0";

                    acts.Add(new NPCActions(ActionType.GivePet, parts[1], petcount, petlevel));
                    break;
                case "REMOVEPET":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.RemovePet, parts[1]));
                    break;
                case "CLEARPETS":
                    acts.Add(new NPCActions(ActionType.ClearPets));
                    break;

                case "GOTO":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.Goto, parts[1]));
                    break;

                case "BREAK":
                    acts.Add(new NPCActions(ActionType.Break));
                    break;

                //cant use stored var
                case "ADDNAMELIST":
                    if (parts.Length < 2) return;

                    quoteMatch = regexQuote.Match(line);

                    string listPath = parts[1];

                    if (quoteMatch.Success)
                        listPath = quoteMatch.Groups[1].Captures[0].Value;

                    fileName = Settings.NameListPath + listPath;

                    string sDirectory = Path.GetDirectoryName(fileName);
                    Directory.CreateDirectory(sDirectory);

                    if (!File.Exists(fileName))
                        File.Create(fileName).Close();

                    acts.Add(new NPCActions(ActionType.AddNameList, fileName));
                    break;

                //cant use stored var
                case "ADDGUILDNAMELIST":
                    if (parts.Length < 2) return;

                    quoteMatch = regexQuote.Match(line);

                    listPath = parts[1];

                    if (quoteMatch.Success)
                        listPath = quoteMatch.Groups[1].Captures[0].Value;

                    fileName = Settings.NameListPath + listPath;

                    sDirectory = Path.GetDirectoryName(fileName);
                    Directory.CreateDirectory(sDirectory);

                    if (!File.Exists(fileName))
                        File.Create(fileName).Close();

                    acts.Add(new NPCActions(ActionType.AddGuildNameList, fileName));
                    break;
                //cant use stored var
                case "DELNAMELIST":
                    if (parts.Length < 2) return;

                    quoteMatch = regexQuote.Match(line);

                    listPath = parts[1];

                    if (quoteMatch.Success)
                        listPath = quoteMatch.Groups[1].Captures[0].Value;

                    fileName = Settings.NameListPath + listPath;

                    sDirectory = Path.GetDirectoryName(fileName);
                    Directory.CreateDirectory(sDirectory);

                    if (File.Exists(fileName))
                        acts.Add(new NPCActions(ActionType.DelNameList, fileName));
                    break;

                //cant use stored var
                case "DELGUILDNAMELIST":
                    if (parts.Length < 2) return;

                    quoteMatch = regexQuote.Match(line);

                    listPath = parts[1];

                    if (quoteMatch.Success)
                        listPath = quoteMatch.Groups[1].Captures[0].Value;

                    fileName = Settings.NameListPath + listPath;

                    sDirectory = Path.GetDirectoryName(fileName);
                    Directory.CreateDirectory(sDirectory);

                    if (File.Exists(fileName))
                        acts.Add(new NPCActions(ActionType.DelGuildNameList, fileName));
                    break;
                //cant use stored var
                case "CLEARNAMELIST":
                    if (parts.Length < 2) return;

                    quoteMatch = regexQuote.Match(line);

                    listPath = parts[1];

                    if (quoteMatch.Success)
                        listPath = quoteMatch.Groups[1].Captures[0].Value;

                    fileName = Settings.NameListPath + listPath;

                    sDirectory = Path.GetDirectoryName(fileName);
                    Directory.CreateDirectory(sDirectory);

                    if (File.Exists(fileName))
                        acts.Add(new NPCActions(ActionType.ClearNameList, fileName));
                    break;
                //cant use stored var
                case "CLEARGUILDNAMELIST":
                    if (parts.Length < 2) return;

                    quoteMatch = regexQuote.Match(line);

                    listPath = parts[1];

                    if (quoteMatch.Success)
                        listPath = quoteMatch.Groups[1].Captures[0].Value;

                    fileName = Settings.NameListPath + listPath;

                    sDirectory = Path.GetDirectoryName(fileName);
                    Directory.CreateDirectory(sDirectory);

                    if (File.Exists(fileName))
                        acts.Add(new NPCActions(ActionType.ClearGuildNameList, fileName));
                    break;

                case "GIVEHP":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.GiveHP, parts[1]));
                    break;

                case "GIVEMP":
                    if (parts.Length < 2) return;
                    acts.Add(new NPCActions(ActionType.GiveMP, parts[1]));
                    break;

                case "CHANGELEVEL":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.ChangeLevel, parts[1]));
                    break;

                case "SETPKPOINT":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.SetPkPoint, parts[1]));
                    break;

                case "REDUCEPKPOINT":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.ReducePkPoint, parts[1]));
                    break;

                case "INCREASEPKPOINT":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.IncreasePkPoint, parts[1]));
                    break;

                case "CHANGEGENDER":
                    acts.Add(new NPCActions(ActionType.ChangeGender));
                    break;

                case "CHANGECLASS":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.ChangeClass, parts[1]));
                    break;

                case "CHANGEHAIR":
                    if (parts.Length < 2)
                    {
                        acts.Add(new NPCActions(ActionType.ChangeHair));
                    }
                    else
                    {
                        acts.Add(new NPCActions(ActionType.ChangeHair, parts[1]));
                    }
                    break;

                case "LOCALMESSAGE":
                    var match = regexQuote.Match(line);
                    if (match.Success)
                    {
                        var message = match.Groups[1].Captures[0].Value;

                        var last = parts.Count() - 1;
                        acts.Add(new NPCActions(ActionType.LocalMessage, message, parts[last]));
                    }
                    break;

                case "GLOBALMESSAGE":
                    match = regexQuote.Match(line);
                    if (match.Success)
                    {
                        var message = match.Groups[1].Captures[0].Value;

                        var last = parts.Count() - 1;
                        acts.Add(new NPCActions(ActionType.GlobalMessage, message, parts[last]));
                    }
                    break;

                case "GIVESKILL":
                    if (parts.Length < 3) return;

                    string spelllevel = parts.Length > 2 ? parts[2] : "0";
                    acts.Add(new NPCActions(ActionType.GiveSkill, parts[1], spelllevel));
                    break;

                case "REMOVESKILL":
                    if (parts.Length < 2) return;
                    
                    acts.Add(new NPCActions(ActionType.RemoveSkill, parts[1]));
                    break;

                //cant use stored var
                case "SET":
                    if (parts.Length < 3) return;
                    match = regexFlag.Match(parts[1]);
                    if (match.Success)
                    {
                        string flagIndex = match.Groups[1].Captures[0].Value;
                        acts.Add(new NPCActions(ActionType.Set, flagIndex, parts[2]));
                    }
                    break;

                case "PARAM1":
                    if (parts.Length < 2) return;

                    string instanceId = parts.Length < 3 ? "1" : parts[2];
                    acts.Add(new NPCActions(ActionType.Param1, parts[1], instanceId));
                    break;

                case "PARAM2":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.Param2, parts[1]));
                    break;

                case "PARAM3":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.Param3, parts[1]));
                    break;

                case "MONGEN":
                    if (parts.Length < 2) return;

                    count = parts.Length < 3 ? "1" : parts[2];
                    acts.Add(new NPCActions(ActionType.Mongen, parts[1], count));
                    break;

                case "TIMERECALL":
                    if (parts.Length < 2) return;

                    string page = parts.Length > 2 ? parts[2] : "";

                    acts.Add(new NPCActions(ActionType.TimeRecall, parts[1], page));
                    break;

                case "TIMERECALLGROUP":
                    if (parts.Length < 2) return;

                    page = parts.Length > 2 ? parts[2] : "";

                    acts.Add(new NPCActions(ActionType.TimeRecallGroup, parts[1], page));
                    break;

                case "BREAKTIMERECALL":
                    acts.Add(new NPCActions(ActionType.BreakTimeRecall));
                    break;

                case "DELAYGOTO":
                    if (parts.Length < 3) return;

                    acts.Add(new NPCActions(ActionType.DelayGoto, parts[1], parts[2]));
                    break;

                case "MONCLEAR":
                    if (parts.Length < 2) return;

                    instanceId = parts.Length < 3 ? "1" : parts[2];

                    string mobName = parts.Length < 4 ? "" : parts[3];

                    acts.Add(new NPCActions(ActionType.MonClear, parts[1], instanceId, mobName));
                    break;

                case "GROUPRECALL":
                    acts.Add(new NPCActions(ActionType.GroupRecall));
                    break;

                case "GROUPTELEPORT":
                    if (parts.Length < 2) return;
                    string x;
                    string y;

                    if (parts.Length == 4)
                    {
                        instanceId = "1";
                        x = parts[2];
                        y = parts[3];
                    }
                    else
                    {
                        instanceId = parts.Length < 3 ? "1" : parts[2];
                        x = parts.Length < 4 ? "0" : parts[3];
                        y = parts.Length < 5 ? "0" : parts[4];
                    }

                    acts.Add(new NPCActions(ActionType.GroupTeleport, parts[1], instanceId, x, y));
                    break;

                case "MOV":
                    if (parts.Length < 3) return;
                    match = Regex.Match(parts[1], @"[A-Z][0-9]", RegexOptions.IgnoreCase);

                    quoteMatch = regexQuote.Match(line);

                    string valueToStore = parts[2];

                    if (quoteMatch.Success)
                        valueToStore = quoteMatch.Groups[1].Captures[0].Value;

                    if (match.Success)
                        acts.Add(new NPCActions(ActionType.Mov, parts[1], valueToStore));

                    break;
                case "CALC":
                    if (parts.Length < 4) return;

                    match = Regex.Match(parts[1], @"[A-Z][0-9]", RegexOptions.IgnoreCase);

                    quoteMatch = regexQuote.Match(line);

                    valueToStore = parts[3];

                    if (quoteMatch.Success)
                        valueToStore = quoteMatch.Groups[1].Captures[0].Value;

                    if (match.Success)
                        acts.Add(new NPCActions(ActionType.Calc, "%" + parts[1], parts[2], valueToStore, parts[1].Insert(1, "-")));

                    break;
                case "GIVEBUFF":
                    if (parts.Length < 4) return;

                    string visible = "";
                    string infinite = "";

                    if (parts.Length > 4) visible = parts[4];
                    if (parts.Length > 5) infinite = parts[5];

                    acts.Add(new NPCActions(ActionType.GiveBuff, parts[1], parts[2], parts[3], visible, infinite));
                    break;

                case "REMOVEBUFF":
                    if (parts.Length < 2) return;

                    acts.Add(new NPCActions(ActionType.RemoveBuff, parts[1]));
                    break;

                case "ADDTOGUILD":
                    if (parts.Length < 2) return;
                    acts.Add(new NPCActions(ActionType.AddToGuild, parts[1]));
                    break;

                case "REMOVEFROMGUILD":
                    if (parts.Length < 2) return;
                    acts.Add(new NPCActions(ActionType.RemoveFromGuild, parts[1]));
                    break;

                case "REFRESHEFFECTS":
                    acts.Add(new NPCActions(ActionType.RefreshEffects));
                    break;

                case "CANGAINEXP":
                    if (parts.Length < 2) return;
                    acts.Add(new NPCActions(ActionType.CanGainExp, parts[1]));
                    break;

                case "COMPOSEMAIL":
                    match = regexQuote.Match(line);
                    if (match.Success)
                    {
                        var message = match.Groups[1].Captures[0].Value;

                        var last = parts.Count() - 1;
                        acts.Add(new NPCActions(ActionType.ComposeMail, message, parts[last]));
                    }
                    break;

                case "ADDMAILGOLD":
                    if (parts.Length < 2) return;
                    acts.Add(new NPCActions(ActionType.AddMailGold, parts[1]));
                    break;

                case "ADDMAILITEM":
                    if (parts.Length < 3) return;
                    acts.Add(new NPCActions(ActionType.AddMailItem, parts[1], parts[2]));
                    break;

                case "SENDMAIL":
                    acts.Add(new NPCActions(ActionType.SendMail));
                    break;

                case "GROUPGOTO":
                    if (parts.Length < 2) return;
                    acts.Add(new NPCActions(ActionType.GroupGoto, parts[1]));
                    break;

                case "ENTERMAP":
                    acts.Add(new NPCActions(ActionType.EnterMap));
                    break;
                case "MAKEWEDDINGRING":
                    acts.Add(new NPCActions(ActionType.MakeWeddingRing));
                    break;
                case "FORCEDIVORCE":
                    acts.Add(new NPCActions(ActionType.ForceDivorce));
                    break;

                case "LOADVALUE":
                    if (parts.Length < 5) return;

                    quoteMatch = regexQuote.Match(line);

                    if (quoteMatch.Success)
                    {
                        fileName = Settings.ValuePath + quoteMatch.Groups[1].Captures[0].Value;

                        string group = parts[parts.Length - 2];
                        string key = parts[parts.Length - 1];

                        sDirectory = Path.GetDirectoryName(fileName);
                        Directory.CreateDirectory(sDirectory);

                        if (!File.Exists(fileName))
                            File.Create(fileName).Close();

                        acts.Add(new NPCActions(ActionType.LoadValue, parts[1], fileName, group, key));
                    }
                    break;

                case "SAVEVALUE":
                    if (parts.Length < 5) return;

                    MatchCollection matchCol = regexQuote.Matches(line);

                    if (matchCol.Count > 0 && matchCol[0].Success)
                    {
                        fileName = Settings.ValuePath + matchCol[0].Groups[1].Captures[0].Value;

                        string value = parts[parts.Length - 1];

                        if (matchCol.Count > 1 && matchCol[1].Success)
                            value = matchCol[1].Groups[1].Captures[0].Value;

                        string[] newParts = line.Replace(value, string.Empty).Replace("\"", "").Trim().Split(' ');

                        string group = newParts[newParts.Length - 2];
                        string key = newParts[newParts.Length - 1];

                        sDirectory = Path.GetDirectoryName(fileName);
                        Directory.CreateDirectory(sDirectory);

                        if (!File.Exists(fileName))
                            File.Create(fileName).Close();

                        acts.Add(new NPCActions(ActionType.SaveValue, fileName, group, key, value));
                    }
                    break;
                case "CONQUESTGUARD":
                    if (parts.Length < 3) return;
                    acts.Add(new NPCActions(ActionType.ConquestGuard, parts[1], parts[2]));
                    break;
                case "CONQUESTGATE":
                    if (parts.Length < 3) return;
                    acts.Add(new NPCActions(ActionType.ConquestGate, parts[1], parts[2]));
                    break;
                case "CONQUESTWALL":
                    if (parts.Length < 3) return;
                    acts.Add(new NPCActions(ActionType.ConquestWall, parts[1], parts[2]));
                    break;
                case "TAKECONQUESTGOLD":
                    if (parts.Length < 2) return;
                    acts.Add(new NPCActions(ActionType.TakeConquestGold, parts[1]));
                    break;
                case "SETCONQUESTRATE":
                    if (parts.Length < 3) return;
                    acts.Add(new NPCActions(ActionType.SetConquestRate, parts[1], parts[2]));
                    break;
                case "STARTCONQUEST":
                    if (parts.Length < 3) return;
                    acts.Add(new NPCActions(ActionType.StartConquest, parts[1], parts[2]));
                    break;
                case "SCHEDULECONQUEST":
                    if (parts.Length < 2) return;
                    acts.Add(new NPCActions(ActionType.ScheduleConquest, parts[1]));
                    break;
                case "OPENGATE":
                    if (parts.Length < 3) return;
                    acts.Add(new NPCActions(ActionType.OpenGate, parts[1], parts[2]));
                    break;
                case "CLOSEGATE":
                    if (parts.Length < 3) return;
                    acts.Add(new NPCActions(ActionType.CloseGate, parts[1], parts[2]));
                    break;
            }

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
            ConquestArcherObject Archer;
            ConquestGateObject Gate;
            ConquestWallObject Wall;
            ConquestSiegeObject Siege;

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
                    Map map = SMain.Envir.GetMapByNameAndInstance(oneValMatch.Groups[2].Captures[0].Value.ToUpper());
                    newValue = map == null ? "N/A" : map.MonsterCount.ToString();
                    break;
                case "CONQUESTGUARD()":
                    var val1 = FindVariable(player, "%" + twoValMatch.Groups[3].Captures[0].Value.ToUpper());
                    var val2 = FindVariable(player, "%" + twoValMatch.Groups[4].Captures[0].Value.ToUpper());

                    int intVal1, intVal2;

                    if (int.TryParse(val1.Replace("%", ""), out intVal1) && int.TryParse(val2.Replace("%", ""), out intVal2))
                    {

                        Conquest = SMain.Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
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
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
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
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
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
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
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
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return string.Empty;
                        if (Conquest.Guild == null) return "No Owner";

                        newValue = Conquest.Guild.Name;
                    }
                    break;
                case "CONQUESTGOLD()":
                    val1 = FindVariable(player, "%" + oneValMatch.Groups[2].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1))
                    {
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return string.Empty;

                        newValue = Conquest.GoldStorage.ToString();
                    }
                    break;
                case "CONQUESTRATE()":
                    val1 = FindVariable(player, "%" + oneValMatch.Groups[2].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1))
                    {
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return string.Empty;

                        newValue = Conquest.npcRate.ToString() + "%";
                    }
                    break;
                case "CONQUESTSCHEDULE()":
                    val1 = FindVariable(player, "%" + oneValMatch.Groups[2].Captures[0].Value.ToUpper());

                    if (int.TryParse(val1.Replace("%", ""), out intVal1))
                    {
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(x => x.Info.Index == intVal1);
                        if (Conquest == null) return "Conquest Not Found";
                        if (Conquest.AttackerID == -1) return "No War Scheduled";

                        if (SMain.Envir.GuildList.FirstOrDefault(x => x.Guildindex == Conquest.AttackerID) == null)
                            newValue = "No War Scheduled";
                        else
                            newValue = (SMain.Envir.GuildList.FirstOrDefault(x => x.Guildindex == Conquest.AttackerID).Name);
                    }
                    break;
                case "OUTPUT()":
                    newValue = FindVariable(player, "%" + varMatch.Groups[2].Captures[0].Value.ToUpper());
                    break;
                case "NPCNAME":
                    for (int i = 0; i < player.CurrentMap.NPCs.Count; i++)
                    {
                        NPCObject ob = player.CurrentMap.NPCs[i];
                        if (ob.ObjectID != player.NPCID) continue;
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
                    newValue = player.MaxHP.ToString(CultureInfo.InvariantCulture);
                    break;
                case "MP":
                    newValue = player.MP.ToString(CultureInfo.InvariantCulture);
                    break;
                case "MAXMP":
                    newValue = player.MaxMP.ToString(CultureInfo.InvariantCulture);
                    break;
                case "GAMEGOLD":
                    newValue = player.Account.Gold.ToString(CultureInfo.InvariantCulture);
                    break;
                case "CREDIT":
                    newValue = player.Account.Credit.ToString(CultureInfo.InvariantCulture);
                    break;
                case "ARMOUR":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Armour] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Armour].Info.Name : "No Armour";
                    break;
                case "WEAPON":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Weapon] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Weapon].Info.Name : "No Weapon";
                    break;
                case "RING_L":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.RingL] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.RingL].Info.Name : "No Ring";
                    break;
                case "RING_R":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.RingR] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.RingR].Info.Name : "No Ring";
                    break;
                case "BRACELET_L":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.BraceletL] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.BraceletL].Info.Name : "No Bracelet";
                    break;
                case "BRACELET_R":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.BraceletR] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.BraceletR].Info.Name : "No Bracelet";
                    break;
                case "NECKLACE":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Necklace] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Necklace].Info.Name : "No Necklace";
                    break;
                case "BELT":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Belt] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Belt].Info.Name : "No Belt";
                    break;
                case "BOOTS":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Boots] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Boots].Info.Name : "No Boots";
                    break;
                case "HELMET":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Helmet] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Helmet].Info.Name : "No Helmet";
                    break;
                case "AMULET":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Amulet] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Amulet].Info.Name : "No Amulet";
                    break;
                case "STONE":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Stone] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Stone].Info.Name : "No Stone";
                    break;
                case "TORCH":
                    newValue = player.Info.Equipment[(int)EquipmentSlot.Torch] != null ?
                        player.Info.Equipment[(int)EquipmentSlot.Torch].Info.Name : "No Torch";
                    break;

                case "DATE":
                    newValue = DateTime.Now.ToShortDateString();
                    break;
                case "USERCOUNT":
                    newValue = SMain.Envir.PlayerCount.ToString(CultureInfo.InvariantCulture);
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
                    newValue = Monster.MaxHP.ToString(CultureInfo.InvariantCulture);
                    break;
                case "DATE":
                    newValue = DateTime.Now.ToShortDateString();
                    break;
                case "USERCOUNT":
                    newValue = SMain.Envir.PlayerCount.ToString(CultureInfo.InvariantCulture);
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

            for (int i = 0; i < CheckList.Count; i++)
            {
                NPCChecks check = CheckList[i];
                List<string> param = check.Params.Select(t => FindVariable(t)).ToList();

                uint tempUint;
                int tempInt;
                int tempInt2;
                Map map;
                switch (check.Type)
                {
                    case CheckType.CheckDay:
                        var day = DateTime.Now.DayOfWeek.ToString().ToUpper();
                        var dayToCheck = param[0].ToUpper();

                        failed = day != dayToCheck;
                        break;

                    case CheckType.CheckHour:
                        if (!uint.TryParse(param[0], out tempUint))
                        {
                            failed = true;
                            break;
                        }

                        var hour = DateTime.Now.Hour;
                        var hourToCheck = tempUint;

                        failed = hour != hourToCheck;
                        break;

                    case CheckType.CheckMinute:
                        if (!uint.TryParse(param[0], out tempUint))
                        {
                            failed = true;
                            break;
                        }

                        var minute = DateTime.Now.Minute;
                        var minuteToCheck = tempUint;

                        failed = minute != minuteToCheck;
                        break;

                    case CheckType.CheckHum:
                        if (!int.TryParse(param[1], out tempInt) || !int.TryParse(param[3], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        map = SMain.Envir.GetMapByNameAndInstance(param[2], tempInt2);
                        if (map == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = !Compare(param[0], map.Players.Count(), tempInt);

                        break;

                    case CheckType.CheckMon:
                        if (!int.TryParse(param[1], out tempInt) || !int.TryParse(param[3], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        map = SMain.Envir.GetMapByNameAndInstance(param[2], tempInt2);
                        if (map == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = !Compare(param[0], map.MonsterCount, tempInt);

                        break;

                    case CheckType.CheckExactMon:
                        if (SMain.Envir.GetMonsterInfo(param[0]) == null)
                        {
                            failed = true;
                            break;
                        }

                        if (!int.TryParse(param[2], out tempInt) || !int.TryParse(param[4], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        map = SMain.Envir.GetMapByNameAndInstance(param[3], tempInt2);
                        if (map == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = (!Compare(param[1], SMain.Envir.Objects.Count((
                            d => d.CurrentMap == map &&
                                d.Race == ObjectType.Monster &&
                                string.Equals(d.Name.Replace(" ", ""), param[0], StringComparison.OrdinalIgnoreCase) &&
                                !d.Dead)), tempInt));

                        break;

                    case CheckType.Random:
                        if (!int.TryParse(param[0], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        failed = 0 != SMain.Envir.Random.Next(0, tempInt);
                        break;
                    case CheckType.CheckCalc:
                        int left;
                        int right;

                        if (!int.TryParse(param[0], out left) || !int.TryParse(param[2], out right))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            failed = !Compare(param[1], left, right);
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[1], Key));
                            return true;
                        }
                        break;
                }

                if (!failed) continue;

                Failed();
                return false;
            }

            Success();
            return true;

        }

        public bool Check(MonsterObject Monster)
        {
            var failed = false;

            for (int i = 0; i < CheckList.Count; i++)
            {
                NPCChecks check = CheckList[i];
                List<string> param = check.Params.Select(t => FindVariable(Monster, t)).ToList();

                for (int j = 0; j < param.Count; j++)
                {
                    var parts = param[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 0) continue;

                    foreach (var part in parts)
                    {
                        param[j] = param[j].Replace(part, ReplaceValue(Monster, part));
                    }
                }

                byte tempByte;
                uint tempUint;
                int tempInt;
                int tempInt2;
                Map map;

                switch (check.Type)
                {
                    case CheckType.Level:
                        if (!byte.TryParse(param[1], out tempByte))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            failed = !Compare(param[0], Monster.Level, tempByte);
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                    case CheckType.CheckDay:
                        var day = DateTime.Now.DayOfWeek.ToString().ToUpper();
                        var dayToCheck = param[0].ToUpper();

                        failed = day != dayToCheck;
                        break;

                    case CheckType.CheckHour:
                        if (!uint.TryParse(param[0], out tempUint))
                        {
                            failed = true;
                            break;
                        }

                        var hour = DateTime.Now.Hour;
                        var hourToCheck = tempUint;

                        failed = hour != hourToCheck;
                        break;

                    case CheckType.CheckMinute:
                        if (!uint.TryParse(param[0], out tempUint))
                        {
                            failed = true;
                            break;
                        }

                        var minute = DateTime.Now.Minute;
                        var minuteToCheck = tempUint;

                        failed = minute != minuteToCheck;
                        break;

                    case CheckType.CheckRange:
                        int x, y, range;
                        if (!int.TryParse(param[0], out x) || !int.TryParse(param[1], out y) || !int.TryParse(param[2], out range))
                        {
                            failed = true;
                            break;
                        }

                        var target = new Point { X = x, Y = y };

                        failed = !Functions.InRange(Monster.CurrentLocation, target, range);
                        break;

                    case CheckType.CheckMap:
                        map = SMain.Envir.GetMapByNameAndInstance(param[0]);

                        failed = Monster.CurrentMap != map;
                        break;
                    case CheckType.CheckHum:
                        if (!int.TryParse(param[1], out tempInt) || !int.TryParse(param[3], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        map = SMain.Envir.GetMapByNameAndInstance(param[2], tempInt2);
                        if (map == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = !Compare(param[0], map.Players.Count(), tempInt);

                        break;

                    case CheckType.CheckMon:
                        if (!int.TryParse(param[1], out tempInt) || !int.TryParse(param[3], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        map = SMain.Envir.GetMapByNameAndInstance(param[2], tempInt2);
                        if (map == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = !Compare(param[0], map.MonsterCount, tempInt);

                        break;

                    case CheckType.CheckExactMon:
                        if (SMain.Envir.GetMonsterInfo(param[0]) == null)
                        {
                            failed = true;
                            break;
                        }

                        if (!int.TryParse(param[2], out tempInt) || !int.TryParse(param[4], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        map = SMain.Envir.GetMapByNameAndInstance(param[3], tempInt2);
                        if (map == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = (!Compare(param[1], SMain.Envir.Objects.Count((
                            d => d.CurrentMap == map &&
                                d.Race == ObjectType.Monster &&
                                string.Equals(d.Name.Replace(" ", ""), param[0], StringComparison.OrdinalIgnoreCase) &&
                                !d.Dead)), tempInt));

                        break;

                    case CheckType.Random:
                        if (!int.TryParse(param[0], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        failed = 0 != SMain.Envir.Random.Next(0, tempInt);
                        break;
                    case CheckType.CheckCalc:
                        int left;
                        int right;

                        if (!int.TryParse(param[0], out left) || !int.TryParse(param[2], out right))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            failed = !Compare(param[1], left, right);
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[1], Key));
                            return true;
                        }
                        break;
                }

                if (!failed) continue;

                Failed(Monster);
                return false;
            }

            Success(Monster);
            return true;

        }

        public bool Check(PlayerObject player)
        {
            var failed = false;

            for (int i = 0; i < CheckList.Count; i++)
            {
                NPCChecks check = CheckList[i];
                List<string> param = check.Params.Select(t => FindVariable(player, t)).ToList();

                for (int j = 0; j < param.Count; j++)
                {
                    var parts = param[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 0) continue;

                    foreach (var part in parts)
                    {
                        param[j] = param[j].Replace(part, ReplaceValue(player, part));
                    }
                }

                byte tempByte;
                uint tempUint;
                int tempInt;
                int tempInt2;

                switch (check.Type)
                {
                    case CheckType.Level:
                        if (!byte.TryParse(param[1], out tempByte))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            failed = !Compare(param[0], player.Level, tempByte);
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;

                    case CheckType.CheckGold:
                        if (!uint.TryParse(param[1], out tempUint))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            failed = !Compare(param[0], player.Account.Gold, tempUint);
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                    case CheckType.CheckGuildGold:
                        if (!uint.TryParse(param[1], out tempUint))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            failed = !Compare(param[0], player.MyGuild.Gold, tempUint);
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                    case CheckType.CheckCredit:
                        if (!uint.TryParse(param[1], out tempUint))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            failed = !Compare(param[0], player.Account.Credit, tempUint);
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;

                    case CheckType.CheckItem:
                        uint count;
                        ushort dura;

                        if (!uint.TryParse(param[1], out count))
                        {
                            failed = true;
                            break;
                        }

                        bool checkDura = ushort.TryParse(param[2], out dura);

                        var info = SMain.Envir.GetItemInfo(param[0]);

                        foreach (var item in player.Info.Inventory.Where(item => item != null && item.Info == info))
                        {
                            if (checkDura)
                                if (item.CurrentDura < (dura * 1000)) continue;

                            if (count > item.Count)
                            {
                                count -= item.Count;
                                continue;
                            }

                            if (count > item.Count) continue;
                            count = 0;
                            break;
                        }
                        if (count > 0)
                            failed = true;
                        break;

                    case CheckType.CheckGender:
                        MirGender gender;

                        if (!MirGender.TryParse(param[0], false, out gender))
                        {
                            failed = true;
                            break;
                        }

                        failed = player.Gender != gender;
                        break;

                    case CheckType.CheckClass:
                        MirClass mirClass;

                        if (!MirClass.TryParse(param[0], true, out mirClass))
                        {
                            failed = true;
                            break;
                        }

                        failed = player.Class != mirClass;
                        break;

                    case CheckType.CheckDay:
                        var day = DateTime.Now.DayOfWeek.ToString().ToUpper();
                        var dayToCheck = param[0].ToUpper();

                        failed = day != dayToCheck;
                        break;

                    case CheckType.CheckHour:
                        if (!uint.TryParse(param[0], out tempUint))
                        {
                            failed = true;
                            break;
                        }

                        var hour = DateTime.Now.Hour;
                        var hourToCheck = tempUint;

                        failed = hour != hourToCheck;
                        break;

                    case CheckType.CheckMinute:
                        if (!uint.TryParse(param[0], out tempUint))
                        {
                            failed = true;
                            break;
                        }

                        var minute = DateTime.Now.Minute;
                        var minuteToCheck = tempUint;

                        failed = minute != minuteToCheck;
                        break;

                    case CheckType.CheckNameList:
                        if (!File.Exists(param[0]))
                        {
                            failed = true;
                            break;
                        }

                        var read = File.ReadAllLines(param[0]);
                        failed = !read.Contains(player.Name);
                        break;

                    case CheckType.CheckGuildNameList:
                        if (!File.Exists(param[0]))
                        {
                            failed = true;
                            break;
                        }
                        
                        read = File.ReadAllLines(param[0]);
                        failed = player.MyGuild == null || !read.Contains(player.MyGuild.Name);
                        break;

                    case CheckType.IsAdmin:
                        failed = !player.IsGM;
                        break;

                    case CheckType.CheckPkPoint:
                        if (!int.TryParse(param[1], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            failed = !Compare(param[0], player.PKPoints, tempInt);
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;

                    case CheckType.CheckRange:
                        int x, y, range;
                        if (!int.TryParse(param[0], out x) || !int.TryParse(param[1], out y) || !int.TryParse(param[2], out range))
                        {
                            failed = true;
                            break;
                        }

                        var target = new Point { X = x, Y = y };

                        failed = !Functions.InRange(player.CurrentLocation, target, range);
                        break;

                    case CheckType.CheckMap:
                        Map map = SMain.Envir.GetMapByNameAndInstance(param[0]);

                        failed = player.CurrentMap != map;
                        break;

                    case CheckType.Check:
                        uint onCheck;

                        if (!uint.TryParse(param[0], out tempUint) || !uint.TryParse(param[1], out onCheck) || tempUint > Globals.FlagIndexCount)
                        {
                            failed = true;
                            break;
                        }

                        bool tempBool = Convert.ToBoolean(onCheck);

                        bool flag = player.Info.Flags[tempUint];

                        failed = flag != tempBool;
                        break;

                    case CheckType.CheckHum:
                        if (!int.TryParse(param[1], out tempInt) || !int.TryParse(param[3], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        map = SMain.Envir.GetMapByNameAndInstance(param[2], tempInt2);
                        if (map == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = !Compare(param[0], map.Players.Count(), tempInt);

                        break;

                    case CheckType.CheckMon:
                        if (!int.TryParse(param[1], out tempInt) || !int.TryParse(param[3], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        map = SMain.Envir.GetMapByNameAndInstance(param[2], tempInt2);
                        if (map == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = !Compare(param[0], map.MonsterCount, tempInt);

                        break;

                    case CheckType.CheckExactMon:
                        if (SMain.Envir.GetMonsterInfo(param[0]) == null)
                        {
                            failed = true;
                            break;
                        }

                        if (!int.TryParse(param[2], out tempInt) || !int.TryParse(param[4], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        map = SMain.Envir.GetMapByNameAndInstance(param[3], tempInt2);
                        if (map == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = (!Compare(param[1], SMain.Envir.Objects.Count((
                            d => d.CurrentMap == map &&
                                d.Race == ObjectType.Monster &&
                                string.Equals(d.Name.Replace(" ", ""), param[0], StringComparison.OrdinalIgnoreCase) &&
                                !d.Dead)), tempInt));

                        break;

                    case CheckType.Random:
                        if (!int.TryParse(param[0], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        failed = 0 != SMain.Envir.Random.Next(0, tempInt);
                        break;

                    case CheckType.Groupleader:
                        failed = (player.GroupMembers == null || player.GroupMembers[0] != player);
                        break;

                    case CheckType.GroupCount:
                        if (!int.TryParse(param[1], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        failed = (player.GroupMembers == null || !Compare(param[0], player.GroupMembers.Count, tempInt));
                        break;
                    case CheckType.GroupCheckNearby:
                        target = new Point(-1,-1);
                        for (int j = 0; j < player.CurrentMap.NPCs.Count; j++)
                        {
                            NPCObject ob = player.CurrentMap.NPCs[j];
                            if (ob.ObjectID != player.NPCID) continue;
                            target = ob.CurrentLocation;
                            break;
                        }
                        if (target.X == -1)
                        {
                            failed = true;
                            break;
                        }
                        if (player.GroupMembers == null)
                            failed = true;
                        else
                        {
                            for (int j = 0; j < player.GroupMembers.Count; j++)
                            {
                                if (player.GroupMembers[j] == null) continue;
                                failed |= !Functions.InRange(player.GroupMembers[j].CurrentLocation, target, 9);
                                if (failed) break;
                            }
                        }
                        break;

                    case CheckType.PetCount:
                        if (!int.TryParse(param[1], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        failed = !Compare(param[0], player.Pets.Count(), tempInt);
                        break;

                    case CheckType.PetLevel:
                        if (!int.TryParse(param[1], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        for (int p = 0; p < player.Pets.Count(); p++)
                        {
                            failed = !Compare(param[0], player.Pets[p].PetLevel, tempInt);
                        }
                        break;

                    case CheckType.CheckCalc:
                        int left;
                        int right;

                        try
                        {
                            if (!int.TryParse(param[0], out left) || !int.TryParse(param[2], out right))
                            {
                                failed = !Compare(param[1], param[0], param[2]);
                            }
                            else
                            {
                                failed = !Compare(param[1], left, right);
                            }
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[1], Key));
                            return true;
                        }
                        break;
                    case CheckType.InGuild:
                        if (param[0].Length > 0)
                        {
                            failed = player.MyGuild == null || player.MyGuild.Name != param[0];
                            break;
                        }

                        failed = player.MyGuild == null;
                        break;

                    case CheckType.CheckQuest:
                        if (!int.TryParse(param[0], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        string tempString = param[1].ToUpper();

                        if (tempString == "ACTIVE")
                        {
                            failed = !player.CurrentQuests.Any(e => e.Index == tempInt);
                        }
                        else //COMPLETE
                        {
                            failed = !player.CompletedQuests.Contains(tempInt);
                        }
                        break;
                    case CheckType.CheckRelationship:
                        if (player.Info.Married == 0)
                        {
                            failed = true;
                        }
                        break;
                    case CheckType.CheckWeddingRing:
                        failed = !player.CheckMakeWeddingRing();
                        break;
                    case CheckType.CheckPet:

                        bool petMatch = false;
                        for (int c = player.Pets.Count - 1; c >= 0; c--)
                        {
                            if (string.Compare(player.Pets[c].Info.Name, param[0], true) != 0) continue;

                            petMatch = true;
                        }

                        failed = !petMatch;
                        break;

                    case CheckType.HasBagSpace:
                        if (!int.TryParse(param[1], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        int slotCount = 0;

                        for (int k = 0; k < player.Info.Inventory.Length; k++)
                            if (player.Info.Inventory[k] == null) slotCount++;

                        failed = !Compare(param[0], slotCount, tempInt);
                        break;
                    case CheckType.IsNewHuman:
                        failed = player.Info.AccountInfo.Characters.Count > 1;
                        break;

                    case CheckType.CheckConquest:
                        if (!int.TryParse(param[0], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            ConquestObject Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                            if (Conquest == null)
                            {
                                failed = true;
                                break;
                            }
                            failed = Conquest.WarIsOn;
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                    case CheckType.AffordGuard:
                        if (!int.TryParse(param[0], out tempInt) || !int.TryParse(param[1], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            ConquestObject Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                            if (Conquest == null)
                            {
                                failed = true;
                                break;
                            }

                            ConquestArcherObject Archer = Conquest.ArcherList.FirstOrDefault(g => g.Info.Index == tempInt2);
                            if (Archer == null || Archer.GetRepairCost() == 0)
                            {
                                failed = true;
                                break;
                            }
                            if (player.MyGuild != null)
                                failed = (player.MyGuild.Gold < Archer.GetRepairCost());
                            else
                                failed = true;
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                    case CheckType.AffordGate:
                        if (!int.TryParse(param[0], out tempInt) || !int.TryParse(param[1], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            ConquestObject Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                            if (Conquest == null)
                            {
                                failed = true;
                                break;
                            }

                            ConquestGateObject Gate = Conquest.GateList.FirstOrDefault(f => f.Info.Index == tempInt2);
                            if (Gate == null || Gate.GetRepairCost() == 0)
                            {
                                failed = true;
                                break;
                            }
                            if (player.MyGuild != null)
                                failed = (player.MyGuild.Gold < Gate.GetRepairCost());
                            else
                                failed = true;
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                    case CheckType.AffordWall:
                        if (!int.TryParse(param[0], out tempInt) || !int.TryParse(param[1], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            ConquestObject Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                            if (Conquest == null)
                            {
                                failed = true;
                                break;
                            }

                            ConquestWallObject Wall = Conquest.WallList.FirstOrDefault(h => h.Info.Index == tempInt2);
                            if (Wall == null || Wall.GetRepairCost() == 0)
                            {
                                failed = true;
                                break;
                            }
                            if (player.MyGuild != null)
                                failed = (player.MyGuild.Gold < Wall.GetRepairCost());
                            else
                                failed = true;
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                    case CheckType.AffordSiege:
                        if (!int.TryParse(param[0], out tempInt) || !int.TryParse(param[1], out tempInt2))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            ConquestObject Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                            if (Conquest == null)
                            {
                                failed = true;
                                break;
                            }

                            ConquestGateObject Gate = Conquest.GateList.FirstOrDefault(f => f.Info.Index == tempInt2);
                            if (Gate == null || Gate.GetRepairCost() == 0)
                            {
                                failed = true;
                                break;
                            }
                            if (player.MyGuild != null)
                                failed = (player.MyGuild.Gold < Gate.GetRepairCost());
                            else
                                failed = true;
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                    case CheckType.CheckPermission:
                        RankOptions guildPermissions;
                        if (!Enum.TryParse(param[0], true, out guildPermissions))
                        {
                            failed = true;
                            break;
                        }
                        
                        if (player.MyGuild == null)
                        {
                            failed = true;
                            break;
                        }

                        failed = !(player.MyGuildRank.Options.HasFlag(guildPermissions));

                        break;
                    case CheckType.ConquestAvailable:
                        if (!int.TryParse(param[0], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            ConquestObject Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                            if (Conquest == null)
                            {
                                failed = true;
                                break;
                            }

                            if (player.MyGuild != null)
                                failed = (Conquest.AttackerID != -1);
                            else
                                failed = true;
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                    case CheckType.ConquestOwner:
                        if (!int.TryParse(param[0], out tempInt))
                        {
                            failed = true;
                            break;
                        }

                        try
                        {
                            ConquestObject Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                            if (Conquest == null)
                            {
                                failed = true;
                                break;
                            }

                            if (player.MyGuild != null && player.MyGuild.Guildindex == Conquest.Owner)
                                failed = false;
                            else
                                failed = true;
                        }
                        catch (ArgumentException)
                        {
                            SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[0], Key));
                            return true;
                        }
                        break;
                }

                if (!failed) continue;

                Failed(player);
                return false;
            }

            Success(player);
            return true;

        }
        private void Act(IList<NPCActions> acts, PlayerObject player)
        {
            MailInfo mailInfo = null;

            for (var i = 0; i < acts.Count; i++)
            {
                uint gold;
                uint credit;
                uint Pearls;
                uint count;
                ushort tempuShort;
                string tempString = string.Empty;
                int x, y;
                int tempInt;
                byte tempByte;
                long tempLong;
                bool tempBool;
                Packet p;

                ItemInfo info;
                MonsterInfo monInfo;

                ConquestObject Conquest;

                NPCActions act = acts[i];
                List<string> param = act.Params.Select(t => FindVariable(player, t)).ToList();

                for (int j = 0; j < param.Count; j++)
                {
                    var parts = param[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 0) continue;

                    foreach (var part in parts)
                    {
                        param[j] = param[j].Replace(part, ReplaceValue(player, part));
                    }

                    param[j] = param[j].Replace("%INPUTSTR", player.NPCInputStr);
                }

                switch (act.Type)
                {
                    case ActionType.Move:
                        Map map = SMain.Envir.GetMapByNameAndInstance(param[0]);
                        if (map == null) return;

                        if (!int.TryParse(param[1], out x)) return;
                        if (!int.TryParse(param[2], out y)) return;

                        var coords = new Point(x, y);

                        if (coords.X > 0 && coords.Y > 0) player.Teleport(map, coords);
                        else player.TeleportRandom(200, 0, map);
                        break;

                    case ActionType.InstanceMove:
                        int instanceId;
                        if (!int.TryParse(param[1], out instanceId)) return;
                        if (!int.TryParse(param[2], out x)) return;
                        if (!int.TryParse(param[3], out y)) return;

                        map = SMain.Envir.GetMapByNameAndInstance(param[0], instanceId);
                        if (map == null) return;
                        player.Teleport(map, new Point(x, y));
                        break;

                    case ActionType.GiveGold:
                        if (!uint.TryParse(param[0], out gold)) return;

                        if (gold + player.Account.Gold >= uint.MaxValue)
                            gold = uint.MaxValue - player.Account.Gold;

                        player.GainGold(gold);
                        break;

                    case ActionType.TakeGold:
                        if (!uint.TryParse(param[0], out gold)) return;

                        if (gold >= player.Account.Gold) gold = player.Account.Gold;

                        player.Account.Gold -= gold;
                        player.Enqueue(new S.LoseGold { Gold = gold });
                        break;
                    case ActionType.GiveGuildGold:
                        if (!uint.TryParse(param[0], out gold)) return;

                        if (gold + player.MyGuild.Gold >= uint.MaxValue)
                            gold = uint.MaxValue - player.MyGuild.Gold;

                        player.MyGuild.Gold += gold;
                        player.MyGuild.SendServerPacket(new S.GuildStorageGoldChange() { Type = 3, Amount = gold });
                        break;
                    case ActionType.TakeGuildGold:
                        if (!uint.TryParse(param[0], out gold)) return;

                        if (gold >= player.MyGuild.Gold) gold = player.MyGuild.Gold;

                        player.MyGuild.Gold -= gold;
                        player.MyGuild.SendServerPacket(new S.GuildStorageGoldChange() { Type = 2, Amount = gold });
                        break;
                    case ActionType.GiveCredit:
                        if (!uint.TryParse(param[0], out credit)) return;

                        if (credit + player.Account.Credit >= uint.MaxValue)
                            credit = uint.MaxValue - player.Account.Credit;

                        player.GainCredit(credit);
                        break;

                    case ActionType.TakeCredit:
                        if (!uint.TryParse(param[0], out credit)) return;

                        if (credit >= player.Account.Credit) credit = player.Account.Credit;

                        player.Account.Credit -= credit;
                        player.Enqueue(new S.LoseCredit { Credit = credit });
                        break;

                    case ActionType.GivePearls:
                        if (!uint.TryParse(param[0], out Pearls)) return;

                        if (Pearls + player.Info.PearlCount >= int.MaxValue)
                            Pearls = (uint)(int.MaxValue - player.Info.PearlCount);

                        player.IntelligentCreatureGainPearls((int)Pearls);
                        break;

                    case ActionType.TakePearls:
                        if (!uint.TryParse(param[0], out Pearls)) return;

                        if (Pearls >= player.Info.PearlCount) Pearls = (uint)player.Info.PearlCount;

                        player.IntelligentCreatureLosePearls((int)Pearls);
                        break;

                    case ActionType.GiveItem:
                        if (param.Count < 2 || !uint.TryParse(param[1], out count)) count = 1;

                        info = SMain.Envir.GetItemInfo(param[0]);

                        if (info == null)
                        {
                            SMain.Enqueue(string.Format("Failed to get ItemInfo: {0}, Page: {1}", param[0], Key));
                            break;
                        }

                        while (count > 0)
                        {
                            UserItem item = SMain.Envir.CreateFreshItem(info);

                            if (item == null)
                            {
                                SMain.Enqueue(string.Format("Failed to create UserItem: {0}, Page: {1}", param[0], Key));
                                return;
                            }

                            if (item.Info.StackSize > count)
                            {
                                item.Count = count;
                                count = 0;
                            }
                            else
                            {
                                count -= item.Info.StackSize;
                                item.Count = item.Info.StackSize;
                            }

                            if (player.CanGainItem(item, false))
                                player.GainItem(item);
                        }
                        break;

                    case ActionType.TakeItem:
                        if (param.Count < 2 || !uint.TryParse(param[1], out count)) count = 1;
                        info = SMain.Envir.GetItemInfo(param[0]);

                        ushort dura;
                        bool checkDura = ushort.TryParse(param[2], out dura);

                        if (info == null)
                        {
                            SMain.Enqueue(string.Format("Failed to get ItemInfo: {0}, Page: {1}", param[0], Key));
                            break;
                        }

                        for (int j = 0; j < player.Info.Inventory.Length; j++)
                        {
                            UserItem item = player.Info.Inventory[j];
                            if (item == null) continue;
                            if (item.Info != info) continue;

                            if (checkDura)
                                if (item.CurrentDura < dura) continue;

                            if (count > item.Count)
                            {
                                player.Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = item.Count });
                                player.Info.Inventory[j] = null;

                                count -= item.Count;
                                continue;
                            }

                            player.Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = count });
                            if (count == item.Count)
                                player.Info.Inventory[j] = null;
                            else
                                item.Count -= count;
                            break;
                        }
                        player.RefreshStats();
                        break;

                    case ActionType.GiveExp:
                        uint tempUint;
                        if (!uint.TryParse(param[0], out tempUint)) return;
                        player.GainExp(tempUint);
                        break;

                    case ActionType.GivePet:
                        byte petcount = 0;
                        byte petlevel = 0;

                        monInfo = SMain.Envir.GetMonsterInfo(param[0]);
                        if (monInfo == null) return;

                        if (param.Count > 1)
                            petcount = byte.TryParse(param[1], out petcount) ? Math.Min((byte)5, petcount) : (byte)1;

                        if (param.Count > 2)
                            petlevel = byte.TryParse(param[2], out petlevel) ? Math.Min((byte)7, petlevel) : (byte)0;

                        for (int j = 0; j < petcount; j++)
                        {
                            MonsterObject monster = MonsterObject.GetMonster(monInfo);
                            if (monster == null) return;
                            monster.PetLevel = petlevel;
                            monster.Master = player;
                            monster.MaxPetLevel = 7;
                            monster.Direction = player.Direction;
                            monster.ActionTime = SMain.Envir.Time + 1000;
                            monster.Spawn(player.CurrentMap, player.CurrentLocation);
                            player.Pets.Add(monster);
                        }
                        break;

                    case ActionType.RemovePet:
                        for (int c = player.Pets.Count - 1; c >= 0; c--)
                        {
                            if (string.Compare(player.Pets[c].Info.Name, param[0], true) != 0) continue;

                            player.Pets[c].Die();
                        }
                        break;

                    case ActionType.ClearPets:
                        for (int c = player.Pets.Count - 1; c >= 0; c--)
                        {
                            player.Pets[c].Die();
                        }
                        break;

                    case ActionType.AddNameList:
                        tempString = param[0];
                        if (File.ReadAllLines(tempString).All(t => player.Name != t))
                        {
                            using (var line = File.AppendText(tempString))
                            {
                                line.WriteLine(player.Name);
                            }
                        }
                        break;


                    case ActionType.AddGuildNameList:
                        tempString = param[0];
                        if (player.MyGuild == null) break;
                        if (File.ReadAllLines(tempString).All(t => player.MyGuild.Name != t))
                        {
                            using (var line = File.AppendText(tempString))
                            {
                                line.WriteLine(player.MyGuild.Name);
                            }
                        }
                        break;
                    case ActionType.DelNameList:
                        tempString = param[0];
                        File.WriteAllLines(tempString, File.ReadLines(tempString).Where(l => l != player.Name).ToList());
                        break;

                    case ActionType.DelGuildNameList:
                        if (player.MyGuild == null) break;
                        tempString = param[0];
                        File.WriteAllLines(tempString, File.ReadLines(tempString).Where(l => l != player.MyGuild.Name).ToList());
                        break;
                    case ActionType.ClearNameList:
                        tempString = param[0];
                        File.WriteAllLines(tempString, new string[] { });
                        break;
                    case ActionType.ClearGuildNameList:
                        if (player.MyGuild == null) break;
                        tempString = param[0];
                        File.WriteAllLines(tempString, new string[] { });
                        break;

                    case ActionType.GiveHP:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        player.ChangeHP(tempInt);
                        break;

                    case ActionType.GiveMP:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        player.ChangeMP(tempInt);
                        break;

                    case ActionType.ChangeLevel:
                        if (!ushort.TryParse(param[0], out tempuShort)) return;
                        tempuShort = Math.Min(ushort.MaxValue, tempuShort);

                        player.Level = tempuShort;
                        player.LevelUp();
                        break;

                    case ActionType.SetPkPoint:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        player.PKPoints = tempInt;
                        break;

                    case ActionType.ReducePkPoint:

                        if (!int.TryParse(param[0], out tempInt)) return;

                        player.PKPoints -= tempInt;
                        if (player.PKPoints < 0) player.PKPoints = 0;

                        break;

                    case ActionType.IncreasePkPoint:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        player.PKPoints += tempInt;
                        break;

                    case ActionType.ChangeGender:

                        switch (player.Info.Gender)
                        {
                            case MirGender.Male:
                                player.Info.Gender = MirGender.Female;
                                break;
                            case MirGender.Female:
                                player.Info.Gender = MirGender.Male;
                                break;
                        }
                        break;

                    case ActionType.ChangeHair:

                        if (param.Count < 1)
                        {
                            player.Info.Hair = (byte)SMain.Envir.Random.Next(0, 9);
                        }
                        else
                        {
                            byte.TryParse(param[0], out tempByte);

                            if (tempByte >= 0 && tempByte <= 9)
                            {
                                player.Info.Hair = tempByte;
                            }
                        }
                        break;

                    case ActionType.ChangeClass:

                        MirClass mirClass;
                        if (!Enum.TryParse(param[0], true, out mirClass)) return;

                        switch (mirClass)
                        {
                            case MirClass.Warrior:
                                player.Info.Class = MirClass.Warrior;
                                break;
                            case MirClass.Taoist:
                                player.Info.Class = MirClass.Taoist;
                                break;
                            case MirClass.Wizard:
                                player.Info.Class = MirClass.Wizard;
                                break;
                            case MirClass.Assassin:
                                player.Info.Class = MirClass.Assassin;
                                break;
                            case MirClass.Archer:
                                player.Info.Class = MirClass.Archer;
                                break;
                        }
                        break;

                    case ActionType.LocalMessage:
                        ChatType chatType;
                        if (!Enum.TryParse(param[1], true, out chatType)) return;
                        player.ReceiveChat(param[0], chatType);
                        break;

                    case ActionType.GlobalMessage:
                        if (!Enum.TryParse(param[1], true, out chatType)) return;

                        p = new S.Chat { Message = param[0], Type = chatType };
                        SMain.Envir.Broadcast(p);
                        break;

                    case ActionType.GiveSkill:
                        byte spellLevel = 0;

                        Spell skill;
                        if (!Enum.TryParse(param[0], true, out skill)) return;

                        if (player.Info.Magics.Any(e => e.Spell == skill)) break;

                        if (param.Count > 1)
                            spellLevel = byte.TryParse(param[1], out spellLevel) ? Math.Min((byte)3, spellLevel) : (byte)0;

                        var magic = new UserMagic(skill) { Level = spellLevel };

                        if (magic.Info == null) return;

                        player.Info.Magics.Add(magic);
                        player.Enqueue(magic.GetInfo());
                        break;

                    case ActionType.RemoveSkill:

                        if (!Enum.TryParse(param[0], true, out skill)) return;

                        if (!player.Info.Magics.Any(e => e.Spell == skill)) break;

                        for (var j = player.Info.Magics.Count - 1; j >= 0; j--)
                        {
                            if (player.Info.Magics[j].Spell != skill) continue;

                            player.Info.Magics.RemoveAt(j);
                            player.Enqueue(new S.RemoveMagic { PlaceId = j });
                        }

                        break;

                    case ActionType.Goto:
                        DelayedAction action = new DelayedAction(DelayedType.NPC, -1, player.NPCID, "[" + param[0] + "]");
                        player.ActionList.Add(action);
                        break;

                    case ActionType.Break:
                        Page.BreakFromSegments = true;
                        break;

                    case ActionType.Set:
                        int flagIndex;
                        uint onCheck;
                        if (!int.TryParse(param[0], out flagIndex)) return;
                        if (!uint.TryParse(param[1], out onCheck)) return;

                        if (flagIndex < 0 || flagIndex >= Globals.FlagIndexCount) return;
                        var flagIsOn = Convert.ToBoolean(onCheck);

                        player.Info.Flags[flagIndex] = flagIsOn;

                        for (int f = player.CurrentMap.NPCs.Count - 1; f >= 0; f--)
                        {
                            if (Functions.InRange(player.CurrentMap.NPCs[f].CurrentLocation, player.CurrentLocation, Globals.DataRange))
                                player.CurrentMap.NPCs[f].CheckVisible(player);
                        }

                        if (flagIsOn) player.CheckNeedQuestFlag(flagIndex);

                        break;

                    case ActionType.Param1:
                        if (!int.TryParse(param[1], out tempInt)) return;

                        Param1 = param[0];
                        Param1Instance = tempInt;
                        break;

                    case ActionType.Param2:
                        if (!int.TryParse(param[0], out tempInt)) return;

                        Param2 = tempInt;
                        break;

                    case ActionType.Param3:
                        if (!int.TryParse(param[0], out tempInt)) return;

                        Param3 = tempInt;
                        break;

                    case ActionType.Mongen:
                        if (Param1 == null || Param2 == 0 || Param3 == 0) return;
                        if (!byte.TryParse(param[1], out tempByte)) return;

                        map = SMain.Envir.GetMapByNameAndInstance(Param1, Param1Instance);
                        if (map == null) return;

                        monInfo = SMain.Envir.GetMonsterInfo(param[0]);
                        if (monInfo == null) return;

                        for (int j = 0; j < tempByte; j++)
                        {
                            MonsterObject monster = MonsterObject.GetMonster(monInfo);
                            if (monster == null) return;
                            monster.Direction = 0;
                            monster.ActionTime = SMain.Envir.Time + 1000;
                            monster.Spawn(map, new Point(Param2, Param3));
                        }
                        break;

                    case ActionType.TimeRecall:
                        if (!long.TryParse(param[0], out tempLong)) return;

                        if (param[1].Length > 0) tempString = "[" + param[1] + "]";

                        Map tempMap = player.CurrentMap;
                        Point tempPoint = player.CurrentLocation;

                        action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time + (tempLong * 1000), player.NPCID, tempString, tempMap, tempPoint);
                        player.ActionList.Add(action);

                        break;

                    case ActionType.TimeRecallGroup:
                        if (player.GroupMembers == null) return;
                        if (!long.TryParse(param[0], out tempLong)) return;
                        if (param[1].Length > 0) tempString = "[" + param[1] + "]";

                        tempMap = player.CurrentMap;
                        tempPoint = player.CurrentLocation;

                        for (int j = 0; j < player.GroupMembers.Count(); j++)
                        {
                            var groupMember = player.GroupMembers[j];

                            action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time + (tempLong * 1000), player.NPCID, tempString, tempMap, tempPoint);
                            groupMember.ActionList.Add(action);
                        }
                        break;

                    case ActionType.BreakTimeRecall:
                        foreach (DelayedAction ac in player.ActionList.Where(u => u.Type == DelayedType.NPC))
                        {
                            ac.FlaggedToRemove = true;
                        }
                        break;

                    case ActionType.DelayGoto:
                        if (!long.TryParse(param[0], out tempLong)) return;

                        action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time + (tempLong * 1000), player.NPCID, "[" + param[1] + "]");
                        player.ActionList.Add(action);
                        break;

                    case ActionType.MonClear:
                        if (!int.TryParse(param[1], out tempInt)) return;

                        map = SMain.Envir.GetMapByNameAndInstance(param[0], tempInt);
                        if (map == null) return;
                        
                        foreach (var cell in map.Cells)
                        {
                            if (cell == null || cell.Objects == null) continue;

                            for (int j = 0; j < cell.Objects.Count(); j++)
                            {
                                MapObject ob = cell.Objects[j];

                                if (ob.Race != ObjectType.Monster) continue;
                                if (ob.Dead) continue;
                                
                                if (!string.IsNullOrEmpty(param[2]) && string.Compare(param[2], ((MonsterObject)ob).Info.Name, true) != 0)
                                    continue;

                                ob.Die();
                            }
                        }
                        break;
                    case ActionType.GroupRecall:
                        if (player.GroupMembers == null) return;

                        for (int j = 0; j < player.GroupMembers.Count(); j++)
                        {
                            player.GroupMembers[j].Teleport(player.CurrentMap, player.CurrentLocation);
                        }
                        break;

                    case ActionType.GroupTeleport:
                        if (player.GroupMembers == null) return;
                        if (!int.TryParse(param[1], out tempInt)) return;
                        if (!int.TryParse(param[2], out x)) return;
                        if (!int.TryParse(param[3], out y)) return;

                        map = SMain.Envir.GetMapByNameAndInstance(param[0], tempInt);
                        if (map == null) return;

                        for (int j = 0; j < player.GroupMembers.Count(); j++)
                        {
                            if (x == 0 || y == 0)
                            {
                                player.GroupMembers[j].TeleportRandom(200, 0, map);
                            }
                            else
                            {
                                player.GroupMembers[j].Teleport(map, new Point(x, y));
                            }
                        }
                        break;

                    case ActionType.Mov:
                        string value = param[0];
                        AddVariable(player, value, param[1]);
                        break;

                    case ActionType.Calc:
                        int left;
                        int right;

                        bool resultLeft = int.TryParse(param[0], out left);
                        bool resultRight = int.TryParse(param[2], out right);

                        if (resultLeft && resultRight)
                        {
                            try
                            {
                                int result = Calculate(param[1], left, right);
                                AddVariable(player, param[3].Replace("-", ""), result.ToString());
                            }
                            catch (ArgumentException)
                            {
                                SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[1], Key));
                            }
                        }
                        else
                        {
                            AddVariable(player, param[3].Replace("-", ""), param[0] + param[2]);
                        }
                        break;

                    case ActionType.GiveBuff:

                        if (!Enum.IsDefined(typeof(BuffType), param[0])) return;

                        tempBool = false;
                        bool tempBool2 = false;

                        long.TryParse(param[1], out tempLong);

                        string[] stringValues = param[2].Split(',');

                        if (stringValues.Length < 1) return;

                        int[] intValues = new int[stringValues.Length];

                        for (int j = 0; j < intValues.Length; j++)
                        {
                            int.TryParse(stringValues[j], out intValues[j]);
                        }

                        if (intValues.Length < 1) return;

                        if (param[3].Length > 0)
                            bool.TryParse(param[3], out tempBool);

                        if (param[4].Length > 0)
                            bool.TryParse(param[4], out tempBool2);

                        Buff buff = new Buff
                        {
                            Type = (BuffType)(byte)Enum.Parse(typeof(BuffType), param[0], true),
                            Caster = player,
                            ExpireTime = SMain.Envir.Time + tempLong * 1000,
                            Values = intValues,
                            Infinite = tempBool,
                            Visible = tempBool2
                        };

                        player.AddBuff(buff);
                        break;

                    case ActionType.RemoveBuff:
                        if (!Enum.IsDefined(typeof(BuffType), param[0])) return;

                        BuffType bType = (BuffType)(byte)Enum.Parse(typeof(BuffType), param[0]);

                        for (int j = 0; j < player.Buffs.Count; j++)
                        {
                            if (player.Buffs[j].Type != bType) continue;

                            player.Buffs[j].Infinite = false;
                            player.Buffs[j].ExpireTime = SMain.Envir.Time;
                        }
                        break;

                    case ActionType.AddToGuild:
                        {
                            if (player.MyGuild != null) return;

                            GuildObject guild = SMain.Envir.GetGuild(param[0]);

                            if (guild == null) return;

                            player.PendingGuildInvite = guild;
                            player.GuildInvite(true);
                        }
                        break;

                    case ActionType.RemoveFromGuild:
                        if (player.MyGuild == null) return;

                        if (player.MyGuildRank == null) return;

                        player.MyGuild.DeleteMember(player, player.Name);

                        break;

                    case ActionType.RefreshEffects:
                        player.SetLevelEffects();
                        p = new S.ObjectLevelEffects { ObjectID = player.ObjectID, LevelEffects = player.LevelEffects };
                        player.Enqueue(p);
                        player.Broadcast(p);
                        break;

                    case ActionType.CanGainExp:
                        bool.TryParse(param[0], out tempBool);
                        player.CanGainExp = tempBool;
                        break;

                    case ActionType.ComposeMail:

                        mailInfo = new MailInfo(player.Info.Index, false)
                        {
                            Sender = param[1],
                            Message = param[0]
                        };
                        break;
                    case ActionType.AddMailGold:
                        if (mailInfo == null) return;

                        uint.TryParse(param[0], out tempUint);

                        mailInfo.Gold += tempUint;
                        break;

                    case ActionType.AddMailItem:
                        if (mailInfo == null) return;
                        if (mailInfo.Items.Count > 5) return;

                        if (param.Count < 2 || !uint.TryParse(param[1], out count)) count = 1;

                        info = SMain.Envir.GetItemInfo(param[0]);

                        if (info == null)
                        {
                            SMain.Enqueue(string.Format("Failed to get ItemInfo: {0}, Page: {1}", param[0], Key));
                            break;
                        }

                        while (count > 0 && mailInfo.Items.Count < 5)
                        {
                            UserItem item = SMain.Envir.CreateFreshItem(info);

                            if (item == null)
                            {
                                SMain.Enqueue(string.Format("Failed to create UserItem: {0}, Page: {1}", param[0], Key));
                                return;
                            }

                            if (item.Info.StackSize > count)
                            {
                                item.Count = count;
                                count = 0;
                            }
                            else
                            {
                                count -= item.Info.StackSize;
                                item.Count = item.Info.StackSize;
                            }

                            mailInfo.Items.Add(item);
                        }


                        break;

                    case ActionType.SendMail:
                        if (mailInfo == null) return;

                        mailInfo.Send();

                        break;

                    case ActionType.GroupGoto:
                        if (player.GroupMembers == null) return;

                        for (int j = 0; j < player.GroupMembers.Count(); j++)
                        {
                            action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time, player.NPCID, "[" + param[0] + "]");
                            player.GroupMembers[j].ActionList.Add(action);
                        }
                        break;

                    case ActionType.EnterMap:
                        if (player.NPCMoveMap == null || player.NPCMoveCoord.IsEmpty) return;
                        player.Teleport(player.NPCMoveMap, player.NPCMoveCoord, false);
                        player.NPCMoveMap = null;
                        player.NPCMoveCoord = Point.Empty;
                        break;

                    case ActionType.MakeWeddingRing:
                        player.MakeWeddingRing();
                        break;

                    case ActionType.ForceDivorce:
                        player.NPCDivorce();
                        break;

                    case ActionType.LoadValue:
                        string val = param[0];
                        string filePath = param[1];
                        string header = param[2];
                        string key = param[3];

                        InIReader reader = new InIReader(filePath);
                        string loadedString = reader.ReadString(header, key, "");

                        if (loadedString == "") break;
                        AddVariable(player, val, loadedString);
                        break;

                    case ActionType.SaveValue:
                        filePath = param[0];
                        header = param[1];
                        key = param[2];
                        val = param[3];

                        reader = new InIReader(filePath);
                        reader.Write(header, key, val);
                        break;
                    case ActionType.ConquestGuard:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;

                        if (!int.TryParse(param[1], out tempInt)) return;
                        ConquestArcherObject ConquestArcher = Conquest.ArcherList.FirstOrDefault(z => z.Index == tempInt);
                        if (ConquestArcher == null) return;

                        if (ConquestArcher.ArcherMonster != null)
                            if (!ConquestArcher.ArcherMonster.Dead) return;

                        if (player.MyGuild == null || player.MyGuild.Gold < ConquestArcher.GetRepairCost()) return;

                        player.MyGuild.Gold -= ConquestArcher.GetRepairCost();
                        player.MyGuild.SendServerPacket(new S.GuildStorageGoldChange() { Type = 2, Amount = ConquestArcher.GetRepairCost() });

                        ConquestArcher.Spawn(true);
                        break;
                    case ActionType.ConquestGate:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;

                        if (!int.TryParse(param[1], out tempInt)) return;
                        ConquestGateObject ConquestGate = Conquest.GateList.FirstOrDefault(z => z.Index == tempInt);
                        if (ConquestGate == null) return;

                        if (player.MyGuild == null || player.MyGuild.Gold < ConquestGate.GetRepairCost()) return;

                        player.MyGuild.Gold -= ConquestGate.GetRepairCost();
                        player.MyGuild.SendServerPacket(new S.GuildStorageGoldChange() { Type = 2, Amount = ConquestGate.GetRepairCost() });

                        ConquestGate.Repair();
                        break;
                    case ActionType.ConquestWall:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;

                        if (!int.TryParse(param[1], out tempInt)) return;
                        ConquestWallObject ConquestWall = Conquest.WallList.FirstOrDefault(z => z.Index == tempInt);
                        if (ConquestWall == null) return;

                        //if (ConquestWall.Wall != null)
                        //    if (!ConquestWall.Wall.Dead) return;

                        uint repairCost = ConquestWall.GetRepairCost();

                        if (player.MyGuild == null || player.MyGuild.Gold < repairCost) return;

                        player.MyGuild.Gold -= repairCost;
                        player.MyGuild.SendServerPacket(new S.GuildStorageGoldChange() { Type = 2, Amount = repairCost });

                        ConquestWall.Repair();
                        break;
                    case ActionType.ConquestSiege:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;

                        if (!int.TryParse(param[1], out tempInt)) return;
                        ConquestSiegeObject ConquestSiege = Conquest.SiegeList.FirstOrDefault(z => z.Index == tempInt);
                        if (ConquestSiege == null) return;

                        if (ConquestSiege.Gate != null)
                            if (!ConquestSiege.Gate.Dead) return;

                        if (player.MyGuild == null || player.MyGuild.Gold < ConquestSiege.GetRepairCost()) return;

                        player.MyGuild.Gold -= ConquestSiege.GetRepairCost();
                        player.MyGuild.SendServerPacket(new S.GuildStorageGoldChange() { Type = 2, Amount = ConquestSiege.GetRepairCost() });

                        ConquestSiege.Repair();
                        break;
                    case ActionType.TakeConquestGold:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;

                        if (player.MyGuild != null && player.MyGuild.Guildindex == Conquest.Owner)
                        {
                            player.MyGuild.Gold += Conquest.GoldStorage;
                            player.MyGuild.SendServerPacket(new S.GuildStorageGoldChange() { Type = 3, Amount = Conquest.GoldStorage });
                            Conquest.GoldStorage = 0;
                        }
                        break;
                    case ActionType.SetConquestRate:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;

                        if (!byte.TryParse(param[1], out tempByte)) return;
                        if (player.MyGuild != null && player.MyGuild.Guildindex == Conquest.Owner)
                        {
                            Conquest.npcRate = tempByte;
                        }
                        break;
                    case ActionType.StartConquest:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;
                        ConquestGame tempGame;

                        if (!ConquestGame.TryParse(param[1], out tempGame)) return;

                        if (!Conquest.WarIsOn)
                        {
                            Conquest.StartType = ConquestType.Forced;
                            Conquest.GameType = tempGame;
                            Conquest.StartWar(tempGame);
                        }
                        break;
                    case ActionType.ScheduleConquest:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;

                        if (player.MyGuild != null && player.MyGuild.Guildindex != Conquest.Owner && !Conquest.WarIsOn)
                        {
                            Conquest.AttackerID = player.MyGuild.Guildindex;
                        }
                        break;
                    case ActionType.OpenGate:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;

                        if (!int.TryParse(param[1], out tempInt)) return;
                        ConquestGateObject OpenGate = Conquest.GateList.FirstOrDefault(z => z.Index == tempInt);
                        if (OpenGate == null) return;
                        if (OpenGate.Gate == null) return;
                        OpenGate.Gate.OpenDoor();
                        break;
                    case ActionType.CloseGate:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Conquest = SMain.Envir.Conquests.FirstOrDefault(z => z.Info.Index == tempInt);
                        if (Conquest == null) return;

                        if (!int.TryParse(param[1], out tempInt)) return;
                        ConquestGateObject CloseGate = Conquest.GateList.FirstOrDefault(z => z.Index == tempInt);
                        if (CloseGate == null) return;
                        if (CloseGate.Gate == null) return;
                        CloseGate.Gate.CloseDoor();
                        break;
                }
            }
        }
        private void Act(IList<NPCActions> acts, MonsterObject Monster)
        {

            for (var i = 0; i < acts.Count; i++)
            {
                string tempString = string.Empty;
                int tempInt;
                byte tempByte;
                long tempLong;
                bool tempBool;
                Packet p;

                MonsterInfo monInfo;

                NPCActions act = acts[i];
                List<string> param = act.Params.Select(t => FindVariable(Monster, t)).ToList();

                for (int j = 0; j < param.Count; j++)
                {
                    var parts = param[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 0) continue;

                    foreach (var part in parts)
                    {
                        param[j] = param[j].Replace(part, ReplaceValue(Monster, part));
                    }
                }
                Map map;
                ChatType chatType;
                switch (act.Type)
                {
                    case ActionType.GiveHP:
                        if (!int.TryParse(param[0], out tempInt)) return;
                        Monster.ChangeHP(tempInt);
                        break;
                    case ActionType.GlobalMessage:
                        if (!Enum.TryParse(param[1], true, out chatType)) return;

                        p = new S.Chat { Message = param[0], Type = chatType };
                        SMain.Envir.Broadcast(p);
                        break;

                    /* //mobs have no real "delayed" npc code so not added this yet
                                        case ActionType.Goto:
                                            DelayedAction action = new DelayedAction(DelayedType.NPC, -1, player.NPCID, "[" + param[0] + "]");
                                            player.ActionList.Add(action);
                                            break;
                    */
                    case ActionType.Break:
                        Page.BreakFromSegments = true;
                        break;

                    case ActionType.Param1:
                        if (!int.TryParse(param[1], out tempInt)) return;

                        Param1 = param[0];
                        Param1Instance = tempInt;
                        break;

                    case ActionType.Param2:
                        if (!int.TryParse(param[0], out tempInt)) return;

                        Param2 = tempInt;
                        break;

                    case ActionType.Param3:
                        if (!int.TryParse(param[0], out tempInt)) return;

                        Param3 = tempInt;
                        break;

                    case ActionType.Mongen:
                        if (Param1 == null || Param2 == 0 || Param3 == 0) return;
                        if (!byte.TryParse(param[1], out tempByte)) return;

                        map = SMain.Envir.GetMapByNameAndInstance(Param1, Param1Instance);
                        if (map == null) return;

                        monInfo = SMain.Envir.GetMonsterInfo(param[0]);
                        if (monInfo == null) return;

                        for (int j = 0; j < tempByte; j++)
                        {
                            MonsterObject monster = MonsterObject.GetMonster(monInfo);
                            if (monster == null) return;
                            monster.Direction = 0;
                            monster.ActionTime = SMain.Envir.Time + 1000;
                            monster.Spawn(map, new Point(Param2, Param3));
                        }
                        break;
                    case ActionType.MonClear:
                        if (!int.TryParse(param[1], out tempInt)) return;

                        map = SMain.Envir.GetMapByNameAndInstance(param[0], tempInt);
                        if (map == null) return;

                        foreach (var cell in map.Cells)
                        {
                            if (cell == null || cell.Objects == null) continue;

                            for (int j = 0; j < cell.Objects.Count(); j++)
                            {
                                MapObject ob = cell.Objects[j];

                                if (ob.Race != ObjectType.Monster) continue;
                                if (ob.Dead) continue;
                                ob.Die();
                            }
                        }
                        break;

                    case ActionType.Mov:
                        string value = param[0];
                        AddVariable(Monster, value, param[1]);
                        break;

                    case ActionType.Calc:
                        int left;
                        int right;

                        bool resultLeft = int.TryParse(param[0], out left);
                        bool resultRight = int.TryParse(param[2], out right);

                        if (resultLeft && resultRight)
                        {
                            try
                            {
                                int result = Calculate(param[1], left, right);
                                AddVariable(Monster, param[3].Replace("-", ""), result.ToString());
                            }
                            catch (ArgumentException)
                            {
                                SMain.Enqueue(string.Format("Incorrect operator: {0}, Page: {1}", param[1], Key));
                            }
                        }
                        else
                        {
                            AddVariable(Monster, param[3].Replace("-", ""), param[0] + param[2]);
                        }
                        break;

                    case ActionType.GiveBuff:

                        if (!Enum.IsDefined(typeof(BuffType), param[0])) return;

                        tempBool = false;
                        bool tempBool2 = false;

                        long.TryParse(param[1], out tempLong);

                        string[] stringValues = param[2].Split(',');

                        if (stringValues.Length < 1) return;

                        int[] intValues = new int[stringValues.Length];

                        for (int j = 0; j < intValues.Length; j++)
                        {
                            int.TryParse(stringValues[j], out intValues[j]);
                        }

                        if (intValues.Length < 1) return;

                        if (param[3].Length > 0)
                            bool.TryParse(param[3], out tempBool);

                        if (param[4].Length > 0)
                            bool.TryParse(param[4], out tempBool2);

                        Buff buff = new Buff
                        {
                            Type = (BuffType)(byte)Enum.Parse(typeof(BuffType), param[0], true),
                            Caster = Monster,
                            ExpireTime = SMain.Envir.Time + tempLong * 1000,
                            Values = intValues,
                            Infinite = tempBool,
                            Visible = tempBool2
                        };

                        Monster.AddBuff(buff);
                        break;

                    case ActionType.RemoveBuff:
                        if (!Enum.IsDefined(typeof(BuffType), param[0])) return;

                        BuffType bType = (BuffType)(byte)Enum.Parse(typeof(BuffType), param[0]);

                        for (int j = 0; j < Monster.Buffs.Count; j++)
                        {
                            if (Monster.Buffs[j].Type != bType) continue;

                            Monster.Buffs[j].Infinite = false;
                            Monster.Buffs[j].ExpireTime = SMain.Envir.Time;
                        }
                        break;

                    case ActionType.LoadValue:
                        string val = param[0];
                        string filePath = param[1];
                        string header = param[2];
                        string key = param[3];

                        InIReader reader = new InIReader(filePath);
                        string loadedString = reader.ReadString(header, key, "");

                        if (loadedString == "") break;
                        AddVariable(Monster, val, loadedString);
                        break;

                    case ActionType.SaveValue:
                        filePath = param[0];
                        header = param[1];
                        key = param[2];
                        val = param[3];

                        reader = new InIReader(filePath);
                        reader.Write(header, key, val);
                        break;
                }
            }
        }
        private void Act(IList<NPCActions> acts)
        {

            for (var i = 0; i < acts.Count; i++)
            {
                string tempString = string.Empty;
                int tempInt;
                byte tempByte;
                Packet p;

                MonsterInfo monInfo;

                NPCActions act = acts[i];
                List<string> param = act.Params.Select(t => FindVariable(t)).ToList();
                Map map;
                ChatType chatType;
                switch (act.Type)
                {
                    case ActionType.ClearNameList:
                        tempString = param[0];
                        File.WriteAllLines(tempString, new string[] { });
                        break;

                    case ActionType.GlobalMessage:
                        if (!Enum.TryParse(param[1], true, out chatType)) return;

                        p = new S.Chat { Message = param[0], Type = chatType };
                        SMain.Envir.Broadcast(p);
                        break;

                    case ActionType.Break:
                        Page.BreakFromSegments = true;
                        break;

                    case ActionType.Param1:
                        if (!int.TryParse(param[1], out tempInt)) return;

                        Param1 = param[0];
                        Param1Instance = tempInt;
                        break;

                    case ActionType.Param2:
                        if (!int.TryParse(param[0], out tempInt)) return;

                        Param2 = tempInt;
                        break;

                    case ActionType.Param3:
                        if (!int.TryParse(param[0], out tempInt)) return;

                        Param3 = tempInt;
                        break;

                    case ActionType.Mongen:
                        if (Param1 == null || Param2 == 0 || Param3 == 0) return;
                        if (!byte.TryParse(param[1], out tempByte)) return;

                        map = SMain.Envir.GetMapByNameAndInstance(Param1, Param1Instance);
                        if (map == null) return;

                        monInfo = SMain.Envir.GetMonsterInfo(param[0]);
                        if (monInfo == null) return;

                        for (int j = 0; j < tempByte; j++)
                        {
                            MonsterObject monster = MonsterObject.GetMonster(monInfo);
                            if (monster == null) return;
                            monster.Direction = 0;
                            monster.ActionTime = SMain.Envir.Time + 1000;
                            monster.Spawn(map, new Point(Param2, Param3));
                        }
                        break;

                    case ActionType.MonClear:
                        if (!int.TryParse(param[1], out tempInt)) return;

                        map = SMain.Envir.GetMapByNameAndInstance(param[0], tempInt);
                        if (map == null) return;

                        foreach (var cell in map.Cells)
                        {
                            if (cell == null || cell.Objects == null) continue;

                            for (int j = 0; j < cell.Objects.Count(); j++)
                            {
                                MapObject ob = cell.Objects[j];

                                if (ob.Race != ObjectType.Monster) continue;
                                if (ob.Dead) continue;
                                ob.Die();
                            }
                        }
                        break;
                }
            }
        }
        private void Success(PlayerObject player)
        {
            Act(ActList, player);

            var parseSay = new List<String>(Say);
            parseSay = ParseSay(player, parseSay);

            player.NPCSpeech.AddRange(parseSay);
            //player.Enqueue(new S.NPCResponse { Page = parseSay });
        }

        private void Failed(PlayerObject player)
        {
            Act(ElseActList, player);

            var parseElseSay = new List<String>(ElseSay);
            parseElseSay = ParseSay(player, parseElseSay);

            player.NPCSpeech.AddRange(parseElseSay);
            //player.Enqueue(new S.NPCResponse { Page = parseElseSay });
        }

        private void Success(MonsterObject Monster)
        {
            Act(ActList, Monster);
        }

        private void Failed(MonsterObject Monster)
        {
            Act(ElseActList, Monster);
        }

        private void Success()
        {
            Act(ActList);
        }

        private void Failed()
        {
            Act(ElseActList);
        }



        public static bool Compare<T>(string op, T left, T right) where T : IComparable<T>
        {
            switch (op)
            {
                case "<": return left.CompareTo(right) < 0;
                case ">": return left.CompareTo(right) > 0;
                case "<=": return left.CompareTo(right) <= 0;
                case ">=": return left.CompareTo(right) >= 0;
                case "==": return left.Equals(right);
                case "!=": return !left.Equals(right);
                default: throw new ArgumentException("Invalid comparison operator: {0}", op);
            }
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
