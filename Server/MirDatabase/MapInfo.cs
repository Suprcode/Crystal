using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class MapInfo
    {
        [Key]
        public int Index { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public ushort MiniMap { get; set; }
        public ushort BigMap { get; set; }
        public ushort Music { get; set; }
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

        public byte[] SafeZoneBytes { get; set; }
        [NotMapped]
        public List<SafeZoneInfo> SafeZones = new List<SafeZoneInfo>();
        [NotMapped]
        public List<MovementInfo> Movements = new List<MovementInfo>();
        [NotMapped]
        public List<RespawnInfo> Respawns = new List<RespawnInfo>();
        [NotMapped]
        public List<NPCInfo> NPCs = new List<NPCInfo>();
        public byte[] MineZoneBytes { get; set; }
        [NotMapped]
        public List<MineZone> MineZones = new List<MineZone>();
        [NotMapped]
        public List<Point> ActiveCoords = new List<Point>();
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

        public void Save()
        {
            using (Envir.ServerDb = new ServerDbContext())
            {
                if (this.Index == 0) Envir.ServerDb.Maps.Add(this);
                if (Envir.ServerDb.Entry(this).State == EntityState.Detached)
                {
                    Envir.ServerDb.Maps.Attach(this);
                    Envir.ServerDb.Entry(this).State = EntityState.Modified;
                }
                using (var s = new MemoryStream())
                using (var bw = new BinaryWriter(s))
                {
                    foreach (var z in SafeZones)
                        z.Save(bw);

                    SafeZoneBytes = s.GetBuffer();
                }
                using (var s = new MemoryStream())
                using (var bw = new BinaryWriter(s))
                {
                    foreach (var z in MineZones)
                        z.Save(bw);
                    MineZoneBytes = s.GetBuffer();
                }

                Envir.ServerDb.SaveChanges();
                Envir.ServerDb.Respawns.RemoveRange(Envir.ServerDb.Respawns.Where(r => r.MapIndex == Index));
                Respawns.ForEach(r => r.MapIndex = Index);
                Envir.ServerDb.Respawns.AddRange(Respawns);
                Envir.ServerDb.Movements.RemoveRange(Envir.ServerDb.Movements.Where(m => m.MapIndex == Index));
                Movements.ForEach(m => m.MapIndex = Index);
                Envir.ServerDb.Movements.AddRange(Movements);
                Envir.ServerDb.SaveChanges();
            }
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


            if (!ushort.TryParse(data[2], out var infoMiniMap)) return;
            info.MiniMap = infoMiniMap;
            if (!Enum.TryParse(data[3], out LightSetting lightSetting)) return;
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
                if (!bool.TryParse(data[start + 3 + (i * 4)], out temp.StartPoint)) return;

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

                if (!int.TryParse(data[start + 2 + (i * 5)], out var outInt)) return;
                temp.MapIndex = outInt;

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

                if (!int.TryParse(data[start + (i * 7)], out var outInt)) return;
                temp.MonsterIndex = outInt;

                if (!int.TryParse(data[start + 1 + (i * 7)], out x)) return;
                if (!int.TryParse(data[start + 2 + (i * 7)], out y)) return;

                temp.Location = new Point(x, y);

                if (!ushort.TryParse(data[start + 3 + (i * 7)], out var outUShort)) return;
                temp.Count = outUShort;
                if (!ushort.TryParse(data[start + 4 + (i * 7)], out outUShort)) return;
                temp.Spread = outUShort;
                if (!ushort.TryParse(data[start + 5 + (i * 7)], out outUShort)) return;
                temp.Delay = outUShort;
                if (!byte.TryParse(data[start + 6 + (i * 7)], out var outByte)) return;
                temp.Direction = outByte;
                if (!int.TryParse(data[start + 7 + (i * 7)], out outInt)) return;
                temp.RespawnIndex = outInt;
                if (!bool.TryParse(data[start + 8 + (i * 7)], out var outBool)) return;
                temp.SaveRespawnTime = outBool;
                if (!ushort.TryParse(data[start + 9 + (i * 7)], out outUShort)) return;
                temp.RespawnTicks = outUShort;

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

                if (!ushort.TryParse(data[start + 4 + (i * 6)], out var outUShort)) return;
                temp.Rate = outUShort;
                if (!ushort.TryParse(data[start + 5 + (i * 6)], out outUShort)) return;
                temp.Image = outUShort;

                info.NPCs.Add(temp);
            }



            if (Settings.UseSqlDb) info.Index = 0;
            else info.Index = ++SMain.EditEnvir.MapIndex;
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