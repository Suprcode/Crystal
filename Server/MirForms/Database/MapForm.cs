using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Server.MirForms
{
    public static class ConvertMapInfo
    {
        public static Envir EditEnvir = null;

        public static List<MapInfo> MapInfo = new List<MapInfo>();
        public static List<MapMovements> MapMovements = new List<MapMovements>();
        public static List<MineInfo> MineInfo = new List<MineInfo>();

        private static int _endIndex = 0;
        public static string Path = string.Empty;

        public static void Start(Envir envir)
        {
            if (Path == string.Empty) return;

            EditEnvir = envir;

            if (EditEnvir == null) return;

            var lines = File.ReadAllLines(Path);

            _endIndex = EditEnvir.MapIndex; // Last map index number

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("[")) // Read map info
                {
                    lines[i] = System.Text.RegularExpressions.Regex.Replace(lines[i], @"\s+", " "); // Clear white-space

                    lines[i] = lines[i].Replace(" ;", ";"); // Remove space before semi-colon

                    // Trim comment at the end of the line
                    if (lines[i].Contains(';'))
                        lines[i] = lines[i].Substring(0, lines[i].IndexOf(";", System.StringComparison.Ordinal));

                    MapInfo newMapInfo = new MapInfo { Index = ++_endIndex };

                    var a = lines[i].Split(']'); // Split map info into [0] = MapFile MapName 0 || [1] = Attributes

                    string[] b = a[0].Split(' ');

                    newMapInfo.MapFile = b[0].TrimStart('['); // Assign MapFile from variable and trim leading '[' char
                    newMapInfo.MapName = b[1]; // Assign MapName from variable

                    List<string> mapAttributes = new List<string>(); // Group of all attributes associated with that map
                    mapAttributes.AddRange(a[1].Split(' '));

                    int nri = mapAttributes.FindIndex(x => x.StartsWith("NORECONNECT(".ToUpper())); // NORECONNECT() placement in list of parameters
                    int fi = mapAttributes.FindIndex(x => x.StartsWith("FIRE(".ToUpper())); // FIRE() placement in list of parameters
                    int li = mapAttributes.FindIndex(x => x.StartsWith("LIGHTNING(".ToUpper())); // LIGHTNING() placement in list of parameters
                    int lighti = mapAttributes.FindIndex(x => x.StartsWith("LIGHT(".ToUpper())); // LIGHT() placement in list of parameters
                    int mmi = mapAttributes.FindIndex(x => x.StartsWith("MINIMAP(".ToUpper())); // MINIMAP() placement in list of parameters
                    int bmi = mapAttributes.FindIndex(x => x.StartsWith("BIGMAP(".ToUpper())); // BIGMAP() placement in list of parameters
                    int mli = mapAttributes.FindIndex(x => x.StartsWith("MAPLIGHT(".ToUpper())); // MAPLIGHT() placement in list of parameters
                    int minei = mapAttributes.FindIndex(x => x.StartsWith("MINE(".ToUpper())); // MINE() placement in list of parameters
                    int musici = mapAttributes.FindIndex(x => x.StartsWith("MUSIC(".ToUpper()));
                    newMapInfo.NoTeleport = mapAttributes.Any(s => s.Contains("NOTELEPORT".ToUpper()));
                    newMapInfo.NoReconnect = mapAttributes.Any(x => x.StartsWith("NORECONNECT(".ToUpper()));
                    newMapInfo.NoRandom = mapAttributes.Any(s => s.Contains("NORANDOMMOVE".ToUpper()));
                    newMapInfo.NoEscape = mapAttributes.Any(s => s.Contains("NOESCAPE".ToUpper()));
                    newMapInfo.NoRecall = mapAttributes.Any(s => s.Contains("NORECALL".ToUpper()));
                    newMapInfo.NoDrug = mapAttributes.Any(s => s.Contains("NODRUG".ToUpper()));
                    newMapInfo.NoPositionMove = mapAttributes.Any(s => s.Contains("NOPOSITIONMOVE".ToUpper()));
                    newMapInfo.NoThrowItem = mapAttributes.Any(s => s.Contains("NOTHROWITEM".ToUpper()));
                    newMapInfo.NoPlayerDrop = mapAttributes.Any(s => s.Contains("NOPLAYERDROP".ToUpper()));
                    newMapInfo.NoMonsterDrop = mapAttributes.Any(s => s.Contains("NOMONSTERDROP".ToUpper()));
                    newMapInfo.NoNames = mapAttributes.Any(s => s.Contains("NONAMES".ToUpper()));
                    newMapInfo.NoFight = mapAttributes.Any(s => s.Contains("NOFIGHT".ToUpper()));
                    newMapInfo.Fight = mapAttributes.Any(s => s.Contains("FIGHT".ToUpper()));
                    newMapInfo.Fire = mapAttributes.Any(x => x.StartsWith("FIRE(".ToUpper()));
                    newMapInfo.Lightning = mapAttributes.Any(x => x.StartsWith("LIGHTNING(".ToUpper()));
                    newMapInfo.MiniMap = mapAttributes.Any(x => x.StartsWith("MINIMAP(".ToUpper()));
                    newMapInfo.BigMap = mapAttributes.Any(x => x.StartsWith("BIGMAP(".ToUpper()));
                    newMapInfo.Mine = mapAttributes.Any(s => s.Contains("MINE(".ToUpper()));
                    newMapInfo.MapLight = mapAttributes.Any(s => s.Contains("MAPLIGHT(".ToUpper()));
                    newMapInfo.Music = mapAttributes.Any(s => s.Contains("MUSIC(".ToUpper()));
                    newMapInfo.Light = LightSetting.Normal;


                    if (newMapInfo.NoReconnect == true) // If there is a NORECONNECT attribute get its MapFile
                        newMapInfo.ReconnectMap = (newMapInfo.ReconnectMap == string.Empty) ? "0" : mapAttributes[nri].TrimStart("NORECONNECT(".ToCharArray()).TrimEnd(')');
                    if (mli != -1) // If there is a MAPLIGHT attribute get its value
                        newMapInfo.MapLightValue = Convert.ToByte(mapAttributes[mli].TrimStart("MAPLIGHT(".ToCharArray()).TrimEnd(')'));

                    if (mapAttributes.Any(s => s.Contains("MINE1".ToUpper()))) // Mir Mine 1
                        newMapInfo.MineIndex = 1;
                    if (mapAttributes.Any(s => s.Contains("MINE2".ToUpper()))) // Mir Mine 2
                        newMapInfo.MineIndex = 2;
                    if (minei != -1) // If there is a MINE attribute get its value
                        newMapInfo.MineIndex = Convert.ToByte(mapAttributes[minei].TrimStart("MINE(".ToCharArray()).TrimEnd(')'));

                    if (newMapInfo.Fire == true) // If there is a FIRE attribute get its value
                        newMapInfo.FireDamage = Convert.ToInt16(mapAttributes[fi].TrimStart("FIRE(".ToCharArray()).TrimEnd(')'));
                    if (newMapInfo.Lightning == true) // If there is a LIGHTNING attribute get its value
                        newMapInfo.LightningDamage = Convert.ToInt16(mapAttributes[li].TrimStart("LIGHTNING(".ToCharArray()).TrimEnd(')'));

                    if (newMapInfo.MiniMap == true) // If there is a MINIMAP attribute get its value
                        newMapInfo.MiniMapNumber = Convert.ToUInt16(mapAttributes[mmi].TrimStart("MINIMAP(".ToCharArray()).TrimEnd(')'));
                    if (newMapInfo.BigMap == true) // If there is a BIGMAP attribute get its value
                        newMapInfo.BigMapNumber = Convert.ToUInt16(mapAttributes[bmi].TrimStart("BIGMAP(".ToCharArray()).TrimEnd(')'));

                    if (newMapInfo.Music == true)
                        newMapInfo.MusicNumber = Convert.ToUInt16(mapAttributes[musici].TrimStart("MUSIC(".ToCharArray()).TrimEnd(')'));

                    if (lighti != -1) // Check if there is a LIGHT attribute and get its value
                    {
                        switch (mapAttributes[lighti].TrimStart("LIGHT(".ToCharArray()).TrimEnd(')'))
                        {
                            case "Dawn":
                                newMapInfo.Light = LightSetting.Dawn;
                                break;
                            case "Day":
                                newMapInfo.Light = LightSetting.Day;
                                break;
                            case "Evening":
                                newMapInfo.Light = LightSetting.Evening;
                                break;
                            case "Night":
                                newMapInfo.Light = LightSetting.Night;
                                break;
                            case "Normal":
                                newMapInfo.Light = LightSetting.Normal;
                                break;
                            default:
                                newMapInfo.Light = LightSetting.Normal;
                                break;
                        }
                    }

                    // Check for light type
                    if (mapAttributes.Any(s => s.Contains("DAY".ToUpper()))) // DAY = Day
                        newMapInfo.Light = LightSetting.Day;
                    else if (mapAttributes.Any(s => s.Contains("DARK".ToUpper()))) // DARK = Night
                        newMapInfo.Light = LightSetting.Night;

                    MapInfo.Add(newMapInfo); // Add map to list
                }
            }

            for (int j = 0; j < MapInfo.Count; j++)
            {
                for (int k = 0; k < lines.Length; k++)
                {
                    try
                    {
                        if (lines[k].StartsWith(MapInfo[j].MapFile + " "))
                        {
                            MapMovements newmapMovements = new MapMovements();

                            lines[k] = lines[k].Replace('.', ','); // Replace point with comma
                            lines[k] = lines[k].Replace(":", ","); // Replace colon with comma
                            lines[k] = lines[k].Replace(", ", ","); // Remove space after comma
                            lines[k] = lines[k].Replace(" ,", ","); // Remove space before comma
                            lines[k] = System.Text.RegularExpressions.Regex.Replace(lines[k], @"\s+", " "); // Clear whitespace
                            lines[k] = lines[k].Replace(" ;", ";"); // Remove space before semi-colon

                            // Trim comment at the end of the line
                            if (lines[k].Contains(';'))
                                lines[k] = lines[k].Substring(0, lines[k].IndexOf(";", System.StringComparison.Ordinal));

                            var c = lines[k].Split(' ');

                            // START - Get values from line
                            if (c.Length == 7) // Every value has a space
                            {
                                c[1] = c[1] + "," + c[2];
                                c[2] = c[5] + "," + c[6];
                                c[3] = c[4];
                            }
                            else if (c.Length == 6) // One value has a space
                            {
                                if (c[2] == "->") // Space in to XY
                                {
                                    c[2] = c[4] + "," + c[5];
                                }
                                else if (c[3] == "->") // Space in from XY
                                {
                                    c[1] = c[1] + "," + c[2];
                                    c[2] = c[5];
                                    c[3] = c[4];
                                }
                            }
                            else if (c.Length == 5) // Proper format
                            {
                                c[2] = c[4];
                            }
                            else // Unreadable value count
                            {
                                continue;
                            }
                            // END - Get values from line

                            string[] d = c[1].Split(',');

                            string[] e = c[2].Split(',');


                            var toMapIndex = EditEnvir.MapInfoList.FindIndex(a => a.FileName == c[3]); //check existing maps for the connection info
                            var toMap = -1;

                            if (toMapIndex >= 0)
                            {
                                toMap = EditEnvir.MapInfoList[toMapIndex].Index; //get real index
                            }

                            if(toMap < 0)
                            {
                                toMapIndex = MapInfo.FindIndex(a => a.MapFile.ToString() == c[3]);

                                if(toMapIndex >= 0)
                                {
                                    toMap = MapInfo[toMapIndex].Index;
                                }
                            }

                            if (toMap < 0) continue;

                            newmapMovements.fromIndex = MapInfo[MapInfo.FindIndex(a => a.MapFile.ToString() == MapInfo[j].MapFile)].Index; // Check MapInfo for MapFile (mapInfo[j].mapFile) and get it's index number
                            newmapMovements.fromX = d[0];
                            newmapMovements.fromY = d[1];
                            newmapMovements.toMap = toMap;
                            newmapMovements.toX = e[0];
                            newmapMovements.toY = e[1];

                            MapMovements.Add(newmapMovements); // Add movements
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            for (int j = 0; j < MapInfo.Count; j++)
            {
                for (int k = 0; k < lines.Length; k++)
                {
                    if (!lines[k].StartsWith("MINEZONE")) continue;
                    var line = lines[k].Split(' ');

                    try
                    {
                        if (line[1] == MapInfo[j].MapFile)
                        {
                          MineInfo newMineInfo = new MineInfo
                          {
                             MapIndex = MapInfo[j].Index,
                             MineIndex = Convert.ToInt16(line[3]),
                             Location = new Point(Convert.ToInt16(line[4]), Convert.ToInt16(line[5])),
                             Range = Convert.ToInt16(line[6])
                          };

                          MineInfo.Add(newMineInfo);
                        }
                    }
                    catch (Exception) { continue; }
                }
            }
        }

        public static void End()
        {
            MapInfo.Clear();
            MapMovements.Clear();
        }
    }

    public class MapInfo
    {
        public LightSetting
            Light = LightSetting.Normal;

        public int
            Index = 0,
            FireDamage = 0,
            LightningDamage = 0,
            MineIndex = 0;

        public ushort
            MiniMapNumber = 0,
            BigMapNumber = 0,
            MusicNumber = 0;

        public string
            MapFile = string.Empty,
            MapName = string.Empty,
            ReconnectMap = string.Empty;

        public byte
            MapLightValue = 0;

        public bool
            NoTeleport = false,
            NoReconnect = false,
            NoRandom = false,
            NoEscape = false,
            NoRecall = false,
            NoDrug = false,
            NoPositionMove = false,
            NoThrowItem = false,
            NoPlayerDrop = false,
            NoMonsterDrop = false,
            NoNames = false,
            Fight = false,
            NoFight = false,
            Fire = false,
            Lightning = false,
            MiniMap = false,
            BigMap = false,
            MapLight = false,
            Music = false,
            Mine = false;

        public List<MapMovements>
            MapMovements = new List<MapMovements>();
    }

    public class MapMovements
    {
        public int
            fromIndex = 0,
            toMap = 0;

        public string
             fromX = string.Empty,
             fromY = string.Empty,

             toX = string.Empty,
             toY = string.Empty;
    }

    public class MineInfo
    {
        public int
            MapIndex,
            MineIndex,
            Range;

        public Point
            Location;
    }


    public static class ConvertNPCInfo
    {
        public static List<NPCInfo> NPCInfoList = new List<NPCInfo>();

        public static void Start()
        {
            string Path = string.Empty;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File|*.txt";
            ofd.ShowDialog();

            if (ofd.FileName == string.Empty) return;

            Path = ofd.FileName;

            var NPCList = File.ReadAllLines(Path);

            for (int i = 0; i < NPCList.Length; i++)
            {
                if (NPCList[i].Contains(';'))
                    NPCList[i] = NPCList[i].Substring(0, NPCList[i].IndexOf(";", System.StringComparison.Ordinal));

                var Line = System.Text.RegularExpressions.Regex.Replace(NPCList[i], @"\s+", " ").Split(' ');

                if (Line.Length < 6) continue;

                try
                {
                    NPCInfo NPC = new NPCInfo
                    {
                        FileName = Line[0],
                        Map = Line[1],
                        X = Convert.ToInt16(Line[2]),
                        Y = Convert.ToInt16(Line[3]),
                        Title = Line[4],
                        Image = (Line.Length >= 8) ? Convert.ToInt16(Line[6]) : Convert.ToInt16(Line[5])
                    };

                    NPCInfoList.Add(NPC);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public static void Stop()
        {
            NPCInfoList.Clear();
        }
    }

    public class NPCInfo
    {
        public string
            FileName = string.Empty,
            Map = "0",
            Title = string.Empty;

        public int
            Index = 0,
            X = 0,
            Y = 0,
            Image = 0,
            Rate = 100;
    }


    public static class ConvertMonGenInfo
    {
        public static List<MonGenInfo> monGenList = new List<MonGenInfo>();

        public static void Start()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File|*.txt|Gen File|*.gen|All|*.*";
            ofd.Multiselect = true;
            ofd.ShowDialog();

            if (ofd.FileNames.Length == 0) return;

            for (int i = 0; i < ofd.FileNames.Length; i++)
            {
                var MonGen = File.ReadAllLines(ofd.FileNames[i]);

                for (int j = 0; j < MonGen.Length; j++)
                {
                    if (MonGen[j].Contains(';'))
                        MonGen[j] = MonGen[j].Substring(0, MonGen[j].IndexOf(";", System.StringComparison.Ordinal));

                    var Line = System.Text.RegularExpressions.Regex.Replace(MonGen[j], @"\s+", " ").Split(' ');

                    if (Line.Length < 7) continue;

                    try
                    {
                        MonGenInfo MonGenItem = new MonGenInfo
                        {
                            Map = Line[0],
                            X = Convert.ToInt16(Line[1]),
                            Y = Convert.ToInt16(Line[2]),
                            Name = Line[3],
                            Range = Convert.ToInt16(Line[4]),
                            Count = Convert.ToInt16(Line[5]),
                            Delay = Convert.ToInt16(Line[6]),
                            Direction = (Line.Length == 8) ? Convert.ToInt16(Line[7]) : 0                    
                        };

                        monGenList.Add(MonGenItem);
                    }
                    catch (Exception) { continue; }
                }
            }
        }

        public static void Stop()
        {
            monGenList.Clear();
        }
    }

    public class MonGenInfo
    {
        public string
            Map = string.Empty,
            Name = string.Empty;

        public int
            X = 0,
            Y = 0,
            Range = 0,
            Count = 0,
            Delay = 0,
            Direction = 0;
    }
}