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


        private static List<BuffInfo> _buffs = null;
        public static List<BuffInfo> Buffs
        {
            get
            {
                if (_buffs == null)
                {
                    _buffs = new List<BuffInfo>();

                    _buffs.Add(new BuffInfo { Type = BuffType.BagWeight, CanStack = true, Icon = 1, Infinite = false, Text = "", Visible = false, Values = new Dictionary<string, object> { { "MaxBagWeight", 5 } } });
                }

                return _buffs;
            }
        }
    }
}
