using System.Text.RegularExpressions;

namespace Server.MirObjects
{
    public class NpcPage
    {
        public string Key;
        public List<NpcSegment> SegmentList = new List<NpcSegment>();
        public List<string> Args = new List<string>();
        public List<string> Buttons = new List<string>();

        public List<int> ScriptCalls = new List<int>();

        public bool BreakFromSegments = false;

        public NpcPage(string key)
        {
            Key = key;
        }

        public string ArgumentParse(string key)
        {
            if (key.StartsWith("[@_")) return key; //Default Npc page so doesn't use arguments in this way

            Regex r = new Regex(@"\((.*)\)");

            Match match = r.Match(key);
            if (!match.Success) return key;

            key = Regex.Replace(key, r.ToString(), "()");

            string strValues = match.Groups[1].Value;
            string[] arrValues = strValues.Split(',');

            Args = new List<string>();

            foreach (var t in arrValues)
                Args.Add(t);

            return key;
        }
    }
}