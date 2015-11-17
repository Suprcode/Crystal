using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Server.MirObjects
{
    public class NPCPage
    {
        public string Key;
        public List<NPCSegment> SegmentList = new List<NPCSegment>();
        public List<string> Args = new List<string>();
        public List<string> Buttons = new List<string>();

        public bool BreakFromSegments = false;

        public NPCPage(string key)
        {
            Key = key;
        }

        public string ArgumentParse(string key)
        {
            if (key.StartsWith("[@_")) return key; //Default NPC page so doesn't use arguments in this way

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