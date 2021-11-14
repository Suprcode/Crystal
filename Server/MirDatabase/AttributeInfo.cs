using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MirDatabase
{

    //ATTRIBUTES SYSTEM
    public class AttributeInfo
    {
        private static InIReader Reader { get; set; }

        public Attributes Limits = new Attributes();
        public Dictionary<string, int> Costs = new Dictionary<string, int>();

        public int HPMPMultiplier;

        public AttributeInfo()
        {
            if (!File.Exists(Settings.ConfigPath + @".\Attributes.ini"))
            {
                File.Create(Settings.ConfigPath + @".\Attributes.ini");
            }

            Reader = new InIReader(Settings.ConfigPath + @".\Attributes.ini");

            HPMPMultiplier = Reader.ReadInt32("Settings", "HPMPMultiplier", 1);

            List<string> keys = new List<string>(Limits.List.Keys);

            foreach (var key in keys)
            {
                Limits.List[key] = Reader.ReadInt32("Limit", key, 0);
            }

            foreach (var key in keys)
            {
                Costs[key] = Reader.ReadInt32("Cost", key, 1);
            }
        }

        public int GetCost(string stat)
        {
            try
            {
                return Costs[stat];
            }
            catch
            {
                return 1;
            }
        }
    }

}
