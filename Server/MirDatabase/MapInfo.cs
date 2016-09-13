using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class MapInfo
    {
        [Key]
        public int Index { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public ushort MiniMap, BigMap, Music;
        public int DBMiniMap { get { return MiniMap;} set { MiniMap = (ushort) value; } }
        public int DBBigMap { get { return BigMap; } set { BigMap = (ushort) value; } }
        public int DBMusic { get { return Music; } set { Music = (ushort) value; } }

        public LightSetting Light { get; set; }
        public byte MapDarkLight { get; set; } = 0;
        public byte MineIndex { get; set; } = 0;

        public bool NoTeleport { get; set; }
        public bool NoReconnect { get; set; }
        public bool NoRandom { get; set; }
        public bool NoEscape { get; set; }
        public bool NoRecall { get; set; }
        public bool NoDrug { get; set; }
        public bool NoPosition { get; set; }
        public bool NoFight { get; set; }

        public bool NoThrowItem { get; set; }

        public bool NoDropPlayer { get; set; }
        public bool NoDropMonster { get; set; }
        public bool NoNames { get; set; }
        public bool NoMount { get; set; }
        public bool NeedBridle { get; set; }
        public bool Fight { get; set; }
        public bool NeedHole { get; set; }
        public bool Fire { get; set; }
        public bool Lightning { get; set; }

        public string NoReconnectMap { get; set; } = string.Empty;
        public int FireDamage { get; set; }
        public int LightningDamage { get; set; }
        [NotMapped]
        public List<SafeZoneInfo> SafeZones = new List<SafeZoneInfo>();
        [NotMapped]
        public List<MovementInfo> Movements = new List<MovementInfo>();
        [NotMapped]
        public List<RespawnInfo> Respawns = new List<RespawnInfo>();
        [NotMapped]
        public List<NPCInfo> NPCs = new List<NPCInfo>();
        [NotMapped]
        public List<MineZone> MineZones = new List<MineZone>();
        [NotMapped]
        public List<Point> ActiveCoords = new List<Point>();

        public string DBActiveCoords
        {
            get
            {
                var stringList = from c in ActiveCoords select c.X + "," + c.Y;
                return string.Join("_", stringList);
            }
            set
            {
                if(string.IsNullOrEmpty(value)) return;
                var stringList = value.Split('_');
                foreach (var s in stringList)
                {
                    var tempArray = s.Split(',');
                    ActiveCoords.Add(new Point()
                    {
                        X = int.Parse(tempArray[0]),
                        Y = int.Parse(tempArray[1]?? "0")
                    });
                }
            }
        }

        [NotMapped]
        public InstanceInfo Instance;

        public MapInfo()
        {

        }

        public MapInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            FileName = reader.ReadString();
            Title = reader.ReadString();
            MiniMap = reader.ReadUInt16();
            Light = (LightSetting) reader.ReadByte();

            if (Envir.LoadVersion >= 3) BigMap = reader.ReadUInt16();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                SafeZones.Add(new SafeZoneInfo(reader) { Info = this });

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                Respawns.Add(new RespawnInfo(reader, Envir.LoadVersion, Envir.LoadCustomVersion));

            if (Envir.LoadVersion <= 33)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    NPCs.Add(new NPCInfo(reader));
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                Movements.Add(new MovementInfo(reader));

            if (Envir.LoadVersion < 14) return;

            NoTeleport = reader.ReadBoolean();
            NoReconnect = reader.ReadBoolean();
            NoReconnectMap = reader.ReadString();
            NoRandom = reader.ReadBoolean();
            NoEscape = reader.ReadBoolean();
            NoRecall = reader.ReadBoolean();
            NoDrug = reader.ReadBoolean();
            NoPosition = reader.ReadBoolean();
            NoThrowItem = reader.ReadBoolean();
            NoDropPlayer = reader.ReadBoolean();
            NoDropMonster = reader.ReadBoolean();
            NoNames = reader.ReadBoolean();
            Fight = reader.ReadBoolean();
            if (Envir.LoadVersion == 14) NeedHole = reader.ReadBoolean();
            Fire = reader.ReadBoolean();
            FireDamage = reader.ReadInt32();
            Lightning = reader.ReadBoolean();
            LightningDamage = reader.ReadInt32();
            if (Envir.LoadVersion < 23) return;
            MapDarkLight = reader.ReadByte();
            if (Envir.LoadVersion < 26) return;
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                MineZones.Add(new MineZone(reader));
            if (Envir.LoadVersion < 27) return;
            MineIndex = reader.ReadByte();

            if (Envir.LoadVersion < 33) return;
            NoMount = reader.ReadBoolean();
            NeedBridle = reader.ReadBoolean();

            if (Envir.LoadVersion < 42) return;
            NoFight = reader.ReadBoolean();

            if (Envir.LoadVersion < 53) return;
                Music = reader.ReadUInt16(); 

        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(FileName);
            writer.Write(Title);
            writer.Write(MiniMap);
            writer.Write((byte)Light);
            writer.Write(BigMap);
            writer.Write(SafeZones.Count);

            for (int i = 0; i < SafeZones.Count; i++)
                SafeZones[i].Save(writer);

            writer.Write(Respawns.Count);
            for (int i = 0; i < Respawns.Count; i++)
                Respawns[i].Save(writer);

            writer.Write(Movements.Count);
            for (int i = 0; i < Movements.Count; i++)
                Movements[i].Save(writer);

            writer.Write(NoTeleport);
            writer.Write(NoReconnect);
            writer.Write(NoReconnectMap);
            writer.Write(NoRandom);
            writer.Write(NoEscape);
            writer.Write(NoRecall);
            writer.Write(NoDrug);
            writer.Write(NoPosition);
            writer.Write(NoThrowItem);
            writer.Write(NoDropPlayer);
            writer.Write(NoDropMonster);
            writer.Write(NoNames);
            writer.Write(Fight);
            writer.Write(Fire);
            writer.Write(FireDamage);
            writer.Write(Lightning);
            writer.Write(LightningDamage);
            writer.Write(MapDarkLight);
            writer.Write(MineZones.Count);
            for (int i = 0; i < MineZones.Count; i++)
                MineZones[i].Save(writer);
            writer.Write(MineIndex);

            writer.Write(NoMount);
            writer.Write(NeedBridle);

            writer.Write(NoFight);

            writer.Write(Music);

            
        }


        public void CreateMap()
        {
            for (int j = 0; j < SMain.Envir.NPCInfoList.Count; j++)
            {
                if (SMain.Envir.NPCInfoList[j].MapIndex != Index) continue;

                NPCs.Add(SMain.Envir.NPCInfoList[j]);
            }

            Map map = new Map(this);

            if(map.Info.FileName == "orc25")
            {

            }

            if (!map.Load()) return;

            SMain.Envir.MapList.Add(map);

            if (Instance == null)
            {
                Instance = new InstanceInfo(this, map);
            }

            for (int i = 0; i < SafeZones.Count; i++)
                if (SafeZones[i].StartPoint)
                    SMain.Envir.StartPoints.Add(SafeZones[i]);
        }

        public void CreateInstance()
        {
            if (Instance.MapList.Count == 0) return;

            Map map = new Map(this);
            if (!map.Load()) return;

            SMain.Envir.MapList.Add(map);

            Instance.AddMap(map);
        }

        public void CreateSafeZone()
        {
            SafeZones.Add(new SafeZoneInfo { Info = this });
        }

        public void CreateRespawnInfo()
        {
            Respawns.Add(new RespawnInfo { RespawnIndex = ++SMain.EditEnvir.RespawnIndex });
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Index, Title);
        }

        public void CreateNPCInfo()
        {
            NPCs.Add(new NPCInfo());
        }

        public void CreateMovementInfo()
        {
            Movements.Add(new MovementInfo());
        }

        public static void FromText(string text)
        {
            string[] data = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 8) return;

            MapInfo info = new MapInfo {FileName = data[0], Title = data[1]};


            if (!ushort.TryParse(data[2], out info.MiniMap)) return;

            LightSetting lightSetting;
            if (!Enum.TryParse(data[3], out lightSetting)) return;
            info.Light = lightSetting;
            int sziCount, miCount, riCount, npcCount;

            if (!int.TryParse(data[4], out sziCount)) return;
            if (!int.TryParse(data[5], out miCount)) return;
            if (!int.TryParse(data[6], out riCount)) return;
            if (!int.TryParse(data[7], out npcCount)) return;


            int start = 8;

            for (int i = 0; i < sziCount; i++)
            {
                SafeZoneInfo temp = new SafeZoneInfo { Info = info };
                int x, y;

                if (!int.TryParse(data[start + (i * 4)], out x)) return;
                if (!int.TryParse(data[start + 1 + (i * 4)], out y)) return;
                if (!ushort.TryParse(data[start + 2 + (i * 4)], out temp.Size)) return;
                bool startPoint;
                if (!bool.TryParse(data[start + 3 + (i * 4)], out startPoint)) return;
                temp.StartPoint = startPoint;

                temp.Location = new Point(x, y);
                info.SafeZones.Add(temp);
            }
            start += sziCount * 4;



            for (int i = 0; i < miCount; i++)
            {
                MovementInfo temp = new MovementInfo();
                int x, y;

                if (!int.TryParse(data[start + (i * 5)], out x)) return;
                if (!int.TryParse(data[start + 1 + (i * 5)], out y)) return;
                temp.Source = new Point(x, y);

                int mapIndex;
                if (!int.TryParse(data[start + 2 + (i * 5)], out mapIndex)) return;
                temp.MapIndex = mapIndex;

                if (!int.TryParse(data[start + 3 + (i * 5)], out x)) return;
                if (!int.TryParse(data[start + 4 + (i * 5)], out y)) return;
                temp.Destination = new Point(x, y);

                info.Movements.Add(temp);
            }
            start += miCount * 5;


            for (int i = 0; i < riCount; i++)
            {
                RespawnInfo temp = new RespawnInfo();
                int x, y;

                int monsterIndex;
                if (!int.TryParse(data[start + (i * 7)], out monsterIndex)) return;
                temp.MonsterIndex = monsterIndex;
                if (!int.TryParse(data[start + 1 + (i * 7)], out x)) return;
                if (!int.TryParse(data[start + 2 + (i * 7)], out y)) return;

                temp.Location = new Point(x, y);

                if (!ushort.TryParse(data[start + 3 + (i * 7)], out temp.Count)) return;
                if (!ushort.TryParse(data[start + 4 + (i * 7)], out temp.Spread)) return;
                if (!ushort.TryParse(data[start + 5 + (i * 7)], out temp.Delay)) return;
                byte direction;
                if (!byte.TryParse(data[start + 6 + (i * 7)], out direction)) return;
                temp.Direction = direction;
                int respawnIndex;
                if (!int.TryParse(data[start + 7 + (i * 7)], out respawnIndex)) return;
                temp.RespawnIndex = respawnIndex;
                bool saveRespawnTime;
                if (!bool.TryParse(data[start + 8 + (i * 7)], out saveRespawnTime)) return;
                temp.SaveRespawnTime = saveRespawnTime;
                if (!ushort.TryParse(data[start + 9 + (i * 7)], out temp.RespawnTicks)) return;

                info.Respawns.Add(temp);
            }
            start += riCount * 7;


            for (int i = 0; i < npcCount; i++)
            {
                NPCInfo temp = new NPCInfo { FileName = data[start + (i * 6)], Name = data[start + 1 + (i * 6)] };
                int x, y;

                if (!int.TryParse(data[start + 2 + (i * 6)], out x)) return;
                if (!int.TryParse(data[start + 3 + (i * 6)], out y)) return;

                temp.Location = new Point(x, y);

                if (!ushort.TryParse(data[start + 4 + (i * 6)], out temp.Rate)) return;
                byte result;
                if (!byte.TryParse(data[start + 5 + (i * 6)], out result)) return;
                temp.Image = result;

                info.NPCs.Add(temp);
            }



            info.Index = ++SMain.EditEnvir.MapIndex;
            SMain.EditEnvir.MapInfoList.Add(info);
        }
    }

    public class InstanceInfo
    {
        //Constants
        public int PlayerCap = 2;
        public int MaxInstanceCount = 10;

        //
        public MapInfo MapInfo;
        public List<Map> MapList = new List<Map>();

        /*
         Notes
         Create new instance from here if all current maps are full
         Destroy maps when instance is empty - process loop in map or here?
         Change NPC INSTANCEMOVE to move and create next available instance

        */

        public InstanceInfo(MapInfo mapInfo, Map map)
        {
            MapInfo = mapInfo;
            AddMap(map);
        }

        public void AddMap(Map map)
        {
            MapList.Add(map);
        }

        public void RemoveMap(Map map)
        {
            MapList.Remove(map);
        }

        public Map GetFirstAvailableInstance()
        {
            for (int i = 0; i < MapList.Count; i++)
            {
                Map m = MapList[i];

                if (m.Players.Count < PlayerCap) return m;
            }

            return null;
        }

        public void CreateNewInstance()
        {
            MapInfo.CreateInstance();
        }
    }
}