using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class DragonInfo
    {
        public bool Enabled;
        public string MapFileName, MonsterName, BodyName;
        public Point Location, DropAreaTop, DropAreaBottom;
        public List<DropInfo>[] Drops = new List<DropInfo>[Globals.MaxDragonLevel];
        public long[] Exps = new long[Globals.MaxDragonLevel - 1];

        public byte Level;
        public long Experience;

        public DragonInfo()
        {
            //Default values
            Enabled = false;
            MapFileName = "D2083";
            MonsterName = "Evil Mir";
            BodyName = "00";
            Location = new Point(82, 44);
            DropAreaTop = new Point(75, 45);
            DropAreaBottom = new Point(86, 57);

            Level = 1;

            for (int i = 0; i < Exps.Length; i++)
            {
                Exps[i] = (i + 1) * 10000;
            }
            for (int i = 0; i < Drops.Length; i++)
            {
                Drops[i] = new List<DropInfo>();
            }
        }
        public DragonInfo(BinaryReader reader)
        {
            Enabled = reader.ReadBoolean();
            MapFileName = reader.ReadString();
            MonsterName = reader.ReadString();
            BodyName = reader.ReadString();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            DropAreaTop = new Point(reader.ReadInt32(), reader.ReadInt32());
            DropAreaBottom = new Point(reader.ReadInt32(), reader.ReadInt32());

            Level = 1;

            for (int i = 0; i < Exps.Length; i++)
            {
                Exps[i] = reader.ReadInt64();
            }
            for (int i = 0; i < Drops.Length; i++)
            {
                Drops[i] = new List<DropInfo>();
            }
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(Enabled);
            writer.Write(MapFileName);
            writer.Write(MonsterName);
            writer.Write(BodyName);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(DropAreaTop.X);
            writer.Write(DropAreaTop.Y);
            writer.Write(DropAreaBottom.X);
            writer.Write(DropAreaBottom.Y);
            for (int i = 0; i < Exps.Length; i++)
            {
                writer.Write(Exps[i]);
            }
        }

        public void LoadDrops()
        {
            for (int i = 0; i < Globals.MaxDragonLevel; i++) Drops[i].Clear();
            string path = Path.Combine(Settings.DropPath, "DragonItem.txt");
            if (!File.Exists(path))
            {
                string[] contents = new[]
                    {
                        ";Pots + Other", string.Empty, string.Empty,
                        ";Weapons", string.Empty, string.Empty,
                        ";Armour", string.Empty, string.Empty,
                        ";Helmets", string.Empty, string.Empty,
                        ";Necklace", string.Empty, string.Empty,
                        ";Bracelets", string.Empty, string.Empty,
                        ";Rings", string.Empty, string.Empty,
                        ";Shoes", string.Empty, string.Empty,
                        ";Belts", string.Empty, string.Empty,
                        ";Stone",
                    };

                File.WriteAllLines(path, contents);


                return;
            }

            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(";") || string.IsNullOrWhiteSpace(lines[i])) continue;

                DropInfo drop = DropInfo.FromLine(lines[i]);
                if (drop == null)
                {
                    SMain.Enqueue(string.Format("Could not load Drop: DragonItem, Line {0}", lines[i]));
                    continue;
                }

                if (drop.level <= 0 || drop.level > Globals.MaxDragonLevel) return;
                Drops[drop.level - 1].Add(drop);
            }

            for (int i = 0; i < Globals.MaxDragonLevel; i++)
            {
                Drops[i].Sort((drop1, drop2) =>
                {
                    if (drop1.Gold > 0 && drop2.Gold == 0)
                        return 1;
                    if (drop1.Gold == 0 && drop2.Gold > 0)
                        return -1;

                    return drop1.Item.Type.CompareTo(drop2.Item.Type);
                });
            }
        }

        public class DropInfo
        {
            public int Chance;
            public ItemInfo Item;
            public uint Gold;
            public byte level;

            public static DropInfo FromLine(string s)
            {
                string[] parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                DropInfo info = new DropInfo();

                if (!int.TryParse(parts[0].Substring(2), out info.Chance)) return null;
                if (string.Compare(parts[1], "Gold", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (parts.Length < 4) return null;
                    if (!uint.TryParse(parts[2], out info.Gold) || info.Gold == 0) return null;
                    if (!byte.TryParse(parts[3], out info.level)) return null;
                }
                else
                {
                    if (parts.Length < 3) return null;
                    info.Item = SMain.Envir.GetItemInfo(parts[1]);
                    if (info.Item == null) return null;
                    if (!byte.TryParse(parts[2], out info.level)) return null;
                }
                return info;
            }
        }
    }
}