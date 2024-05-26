using System.Text.RegularExpressions;

namespace Server.Library.MirObjects.NPC {
    public class NpcPage {
        public string Key;
        public List<NpcSegment> SegmentList = new();
        public List<string> Args = new();
        public List<string> Buttons = new();

        public List<int> ScriptCalls = new();

        public bool BreakFromSegments = false;

        public NpcPage(string key) {
            Key = key;
        }

        public string ArgumentParse(string key) {
            if(key.StartsWith("[@_")) {
                return key; //Default Npc page so doesn't use arguments in this way
            }

            Regex r = new(@"\((.*)\)");

            Match match = r.Match(key);
            if(!match.Success) {
                return key;
            }

            key = Regex.Replace(key, r.ToString(), "()");

            string strValues = match.Groups[1].Value;
            string[] arrValues = strValues.Split(',');

            Args = new List<string>();

            foreach(string t in arrValues) {
                Args.Add(t);
            }

            return key;
        }
    }
}
