using System;
using System.Collections.Generic;
using System.Text;

namespace Server.MirDatabase
{
    public class BuffInfo
    {
        public BuffType Type { get; set; }
        public bool CanStack { get; set; }
        public int Icon { get; set; }
        public bool Visible { get; set; }
        public bool Infinite { get; set; }
        public string Text { get; set; }
        public Dictionary<string, object> Values { get; set; }  //Positive, Negative, Percentage Yes/No
    }
}
