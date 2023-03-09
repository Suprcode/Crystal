using Server.MirDatabase;
using Server.MirEnvir;

namespace Server.MirForms
{
    public static class ConvertMapInfo
    {
        public static Envir EditEnvir = null;

        public static string Path = string.Empty;

        private static List<String> errors = new List<String>();

        public static void Start(Envir envirToUpdate)
        {
            if (Path == string.Empty) return;
            EditEnvir = envirToUpdate;

            if (EditEnvir == null) return;

            var lines = File.ReadAllLines(Path);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("[")) // Read map info
                {
                    lines[i] = System.Text.RegularExpressions.Regex.Replace(lines[i], @"\s+", " "); // Clear white-space
                    lines[i] = lines[i].Replace(" ;", ";"); // Remove space before semi-colon

                    // Trim comment at the end of the line
                    if (lines[i].Contains(';'))
                        lines[i] = lines[i].Substring(0, lines[i].IndexOf(";", System.StringComparison.Ordinal));

                    MirDatabase.MapInfo newMapInfo = new MirDatabase.MapInfo { Index = ++EditEnvir.MapIndex };

                    var a = lines[i].Split(']'); // Split map info into [0] = MapFile MapName 0 || [1] = Attributes
                    string[] b = a[0].Split(' ');

                    newMapInfo.FileName = b[0].TrimStart('['); // Assign MapFile from variable and trim leading '[' char
                    newMapInfo.Title = b[1]; // Assign MapName from variable

                    List<string> mapAttributes = new List<string>(); // Group of all attributes associated with that map
                    mapAttributes.AddRange(a[1].Split(' '));

                    newMapInfo.NoTeleport = mapAttributes.Any(s => s.Contains("NOTELEPORT".ToUpper()));
                    newMapInfo.NoRandom = mapAttributes.Any(s => s.Contains("NORANDOMMOVE".ToUpper()));
                    newMapInfo.NoEscape = mapAttributes.Any(s => s.Contains("NOESCAPE".ToUpper()));
                    newMapInfo.NoRecall = mapAttributes.Any(s => s.Contains("NORECALL".ToUpper()));
                    newMapInfo.NoDrug = mapAttributes.Any(s => s.Contains("NODRUG".ToUpper()));
                    newMapInfo.NoPosition = mapAttributes.Any(s => s.Contains("NOPOSITIONMOVE".ToUpper()));
                    newMapInfo.NoThrowItem = mapAttributes.Any(s => s.Contains("NOTHROWITEM".ToUpper()));
                    newMapInfo.NoDropPlayer = mapAttributes.Any(s => s.Contains("NOPLAYERDROP".ToUpper()));
                    newMapInfo.NoDropMonster = mapAttributes.Any(s => s.Contains("NOMONSTERDROP".ToUpper()));
                    newMapInfo.NoNames = mapAttributes.Any(s => s.Contains("NONAMES".ToUpper()));
                    newMapInfo.NoFight = mapAttributes.Any(s => s.Contains("NOFIGHT".ToUpper()));
                    newMapInfo.NoMount = mapAttributes.Any(s => s.Contains("NOMOUNT".ToUpper()));
                    newMapInfo.NeedBridle = mapAttributes.Any(s => s.Contains("NEEDBRIDLE".ToUpper()));
                    newMapInfo.Fight = mapAttributes.Any(s => s.Contains("FIGHT".ToUpper()));
                    newMapInfo.NoTownTeleport = mapAttributes.Any(s => s.Contains("NOTOWNTELEPORT".ToUpper()));

                    newMapInfo.Fire = mapAttributes.Any(x => x.StartsWith("FIRE(".ToUpper()));
                    if (newMapInfo.Fire)
                    {
                        int index = mapAttributes.FindIndex(x => x.StartsWith("FIRE(".ToUpper()));
                        newMapInfo.FireDamage = Convert.ToInt16(mapAttributes[index].TrimStart("FIRE(".ToCharArray()).TrimEnd(')'));
                    }
                    newMapInfo.Lightning = mapAttributes.Any(x => x.StartsWith("LIGHTNING(".ToUpper()));
                    if (newMapInfo.Lightning)
                    {
                        int index = mapAttributes.FindIndex(x => x.StartsWith("LIGHTNING(".ToUpper()));
                        newMapInfo.LightningDamage = Convert.ToInt16(mapAttributes[index].TrimStart("LIGHTNING(".ToCharArray()).TrimEnd(')'));
                    }
                    newMapInfo.NoReconnect = mapAttributes.Any(x => x.StartsWith("NORECONNECT(".ToUpper()));
                    if (newMapInfo.NoReconnect)
                    {
                        int index = mapAttributes.FindIndex(x => x.StartsWith("NORECONNECT(".ToUpper()));
                        newMapInfo.NoReconnectMap = newMapInfo.NoReconnectMap == string.Empty ? "0" : mapAttributes[index].TrimStart("NORECONNECT(".ToCharArray()).TrimEnd(')');
                    }

                    if (mapAttributes.Any(x => x.StartsWith("MINIMAP(".ToUpper())))
                    {
                        int index = mapAttributes.FindIndex(x => x.StartsWith("MINIMAP(".ToUpper()));
                        newMapInfo.MiniMap = Convert.ToUInt16(mapAttributes[index].TrimStart("MINIMAP(".ToCharArray()).TrimEnd(')'));
                    }
                    if (mapAttributes.Any(x => x.StartsWith("BIGMAP(".ToUpper())))
                    {
                        int index = mapAttributes.FindIndex(x => x.StartsWith("BIGMAP(".ToUpper()));
                        newMapInfo.BigMap = Convert.ToUInt16(mapAttributes[index].TrimStart("BIGMAP(".ToCharArray()).TrimEnd(')'));
                    }
                    if (mapAttributes.Any(s => s.Contains("MINE(".ToUpper())))
                    {
                        int index = mapAttributes.FindIndex(x => x.StartsWith("MINE(".ToUpper()));
                        newMapInfo.MineIndex = Convert.ToByte(mapAttributes[index].TrimStart("MINE(".ToCharArray()).TrimEnd(')'));
                    }
                    if (mapAttributes.Any(s => s.Contains("MAPLIGHT(".ToUpper())))
                    {
                        int index = mapAttributes.FindIndex(x => x.StartsWith("MAPLIGHT(".ToUpper()));
                        newMapInfo.MapDarkLight = Convert.ToByte(mapAttributes[index].TrimStart("MAPLIGHT(".ToCharArray()).TrimEnd(')'));
                    }
                    if (mapAttributes.Any(s => s.Contains("MUSIC(".ToUpper())))
                    {
                        int index = mapAttributes.FindIndex(x => x.StartsWith("MUSIC(".ToUpper()));
                        newMapInfo.Music = Convert.ToUInt16(mapAttributes[index].TrimStart("MUSIC(".ToCharArray()).TrimEnd(')'));
                    }
                    if (mapAttributes.Any(x => x.StartsWith("LIGHT(".ToUpper()))) // Check if there is a LIGHT attribute and get its value
                    {
                        int index = mapAttributes.FindIndex(x => x.StartsWith("LIGHT(".ToUpper()));
                        switch (mapAttributes[index].TrimStart("LIGHT(".ToCharArray()).TrimEnd(')'))
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
                    else newMapInfo.Light = LightSetting.Normal;

                    // Check for light type
                    if (mapAttributes.Any(s => s.Contains("DAY".ToUpper()))) // DAY = Day
                        newMapInfo.Light = LightSetting.Day;
                    else if (mapAttributes.Any(s => s.Contains("DARK".ToUpper()))) // DARK = Night
                        newMapInfo.Light = LightSetting.Night;

                    EditEnvir.MapInfoList.Add(newMapInfo); // Add map to list
                }
                else if (lines[i].StartsWith(";")) continue;
                else
                    continue;
            }

            for (int j = 0; j < EditEnvir.MapInfoList.Count; j++)
            {
                for (int k = 0; k < lines.Length; k++)
                {
                    try
                    {
                        if (lines[k].StartsWith(EditEnvir.MapInfoList[j].FileName + " "))
                        {
                            MirDatabase.MovementInfo newMovement = new MirDatabase.MovementInfo();

                            if (lines[k].Contains("NEEDHOLE"))
                            {
                                newMovement.NeedHole = true;
                                lines[k] = lines[k].Replace("NEEDHOLE", "");
                            }
                            if (lines[k].Contains("NEEDMOVE"))
                            {
                                newMovement.NeedMove = true;
                                lines[k] = lines[k].Replace("NEEDMOVE", "");
                            }
                            if (lines[k].Contains("NEEDCONQUEST"))
                            {
                                int conqLocation = lines[k].IndexOf(" NEEDCONQUEST");
                                string conq = lines[k].Substring(conqLocation);
                                int conqIndex = int.Parse(conq.Replace("NEEDCONQUEST(", "").Replace(")", "")); //get value
                                newMovement.ConquestIndex = conqIndex;
                                lines[k] = lines[k].Remove(conqLocation);
                            }
                            if (lines[k].Contains("SHOWONBIGMAP"))
                            {
                                newMovement.ShowOnBigMap = true;
                                lines[k] = lines[k].Replace("SHOWONBIGMAP", "");
                            }
                            if (lines[k].Contains("BIGMAPICON"))
                            {
                                int iconLocation = lines[k].IndexOf(" BIGMAPICON");
                                string icon = lines[k].Substring(iconLocation);
                                int iconIndex = int.Parse(icon.Replace("BIGMAPICON(", "").Replace(")", "")); //get value
                                newMovement.Icon = iconIndex;
                                lines[k] = lines[k].Remove(iconLocation);
                            }

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
                            if (toMap < 0)
                            {
                                toMapIndex = EditEnvir.MapInfoList.FindIndex(a => a.FileName.ToString() == c[3]);

                                if (toMapIndex >= 0)
                                {
                                    toMap = EditEnvir.MapInfoList[toMapIndex].Index;
                                }
                            }

                            if (toMap < 0)
                            {
                                errors.Add("Destination Map Failed on line: " + lines[k] + "");
                                continue;
                            }

                            newMovement.MapIndex = toMap;
                            newMovement.Source = new Point(int.Parse(d[0]), int.Parse(d[1]));
                            newMovement.Destination = new Point(int.Parse(e[0]), int.Parse(e[1]));
                            //NeedHole
                            //NeedMove
                            //ConquestIndex
                            EditEnvir.MapInfoList[j].Movements.Add(newMovement);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            for (int j = 0; j < EditEnvir.MapInfoList.Count; j++)
            {
                for (int k = 0; k < lines.Length; k++)
                {
                    if (!lines[k].StartsWith("MINEZONE")) continue;
                    var line = lines[k].Split(' ');

                    try
                    {
                        if (line[1] == EditEnvir.MapInfoList[j].FileName)
                        {
                            MineZone newMineInfo = new MineZone
                            {
                                Mine = Convert.ToByte(line[3]),
                                Location = new Point(Convert.ToInt16(line[4]), Convert.ToInt16(line[5])),
                                Size = Convert.ToUInt16(line[6])
                            };
                            EditEnvir.MapInfoList[j].MineZones.Add(newMineInfo);
                        }
                    }
                    catch (Exception) { continue; }
                }
            }
            for (int j = 0; j < EditEnvir.MapInfoList.Count; j++)
            {
                for (int k = 0; k < lines.Length; k++)
                {
                    //STARTZONE(0,150,150,50) || SAFEZONE(0,150,150,50)
                    if (!lines[k].StartsWith("SAFEZONE") && !lines[k].StartsWith("STARTZONE")) continue;
                    var line = lines[k].Replace(")", "").Split(','); // STARTZONE(0,150,150,50) -> STARTZONE(0 || 150 || 150 || 50
                    var head = line[0].Split('('); // STARTZONE(0 -> STARTZONE || 0
                    try
                    {
                        if (head[1] == EditEnvir.MapInfoList[j].FileName)
                        {
                            MirDatabase.SafeZoneInfo newSafeZone = new MirDatabase.SafeZoneInfo
                            {
                                Info = EditEnvir.MapInfoList[j],
                                StartPoint = head[0].Equals("STARTZONE"),
                                Location = new Point(Convert.ToInt16(line[2]), Convert.ToInt16(line[3])),
                                Size = Convert.ToUInt16(line[1])
                            };
                            if (!EditEnvir.MapInfoList[j].SafeZones.Exists(sz => sz.StartPoint == newSafeZone.StartPoint && sz.Location.X == newSafeZone.Location.X && sz.Location.Y == newSafeZone.Location.Y))
                                EditEnvir.MapInfoList[j].SafeZones.Add(newSafeZone);
                        }
                    }
                    catch (Exception) { continue; }
                }
            }
        }
        public static void End()
        {
            SMain.Enqueue(String.Join("MapInfo Import Report:", errors.Count > 0 ? "" : "No Errors"));
            foreach (String error in errors)
                SMain.Enqueue(error);
        }
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

                    var Line = System.Text.RegularExpressions.Regex.Replace(MonGen[j], @"\s+", " ").Split(',');

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
                            Direction = (Line.Length >= 8) ? Convert.ToInt16(Line[7]) : 0,
                            RoutePath = (Line.Length >= 9) ? Line[8] : string.Empty
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
            Name = string.Empty,
            RoutePath = string.Empty;

        public int
            X = 0,
            Y = 0,
            Range = 0,
            Count = 0,
            Delay = 0,
            Direction = 0;
    }
}