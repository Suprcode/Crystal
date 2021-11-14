using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirDatabase
{
    public class MagicInfo
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public string Name;
        public Spell Spell;
        public byte BaseCost, LevelCost, Icon;
        public ushort Level1, Level2, Level3, Level4, Level5, Level6, Level7, Level8, Level9, Level10;
        public ushort Need1, Need2, Need3, Need4, Need5, Need6, Need7, Need8, Need9, Need10;
        public uint DelayBase = 1800, DelayReduction;
        public byte Range = 9;

        public ushort PowerBase, PowerBonus;
        public ushort MPowerBase, MPowerBonus;
        public float MultiplierBase = 1.0f, MultiplierBonus;

        public ushort PvPPowerBase, PvPPowerBonus;
        public ushort PvPMPowerBase, PvPMPowerBonus;
        public float PvPMultiplierBase = 1.0f, PvPMultiplierBonus;

        public override string ToString()
        {
            return Name;
        }

        public MagicInfo()
        {

        }
        public void Copy(MagicInfo info)
        {
            Name = info.Name;
            Spell = info.Spell;
            BaseCost = info.BaseCost;
            LevelCost = info.LevelCost;
            Icon = info.Icon;

            Level1 = info.Level1;
            Level2 = info.Level2;
            Level3 = info.Level3;
            Level4 = info.Level4;
            Level5 = info.Level5;
            Level6 = info.Level6;
            Level7 = info.Level7;
            Level8 = info.Level8;
            Level9 = info.Level9;
            Level10 = info.Level10;

            Need1 = info.Need1;
            Need2 = info.Need2;
            Need3 = info.Need3;
            Need4 = info.Need4;
            Need5 = info.Need5;
            Need6 = info.Need6;
            Need7 = info.Need7;
            Need8 = info.Need8;
            Need9 = info.Need9;
            Need10 = info.Need10;

            DelayBase = info.DelayBase;
            DelayReduction = info.DelayReduction;

            PowerBase = info.PowerBase;
            PowerBonus = info.PowerBonus;

            MPowerBase = info.MPowerBase;
            MPowerBonus = info.MPowerBonus;

            MultiplierBase = info.MultiplierBase;
            MultiplierBonus = info.MultiplierBonus;

            Range = info.Range;

            PvPPowerBase = info.PvPPowerBase;
            PvPPowerBonus = info.PvPPowerBonus;
            PvPMPowerBase = info.PvPMPowerBase;
            PvPMPowerBonus = info.PvPMPowerBonus;
            PvPMultiplierBase = info.PvPMultiplierBase;
            PvPMultiplierBonus = info.PvPMultiplierBonus;
        }

        public MagicInfo (BinaryReader reader, int version = int.MaxValue, int Customversion = int.MaxValue)
        {
            Name = reader.ReadString();
            Spell = (Spell)reader.ReadByte();
            BaseCost = reader.ReadByte();
            LevelCost = reader.ReadByte();
            Icon = reader.ReadByte();
            Level1 = reader.ReadUInt16();
            Level2 = reader.ReadUInt16();
            Level3 = reader.ReadUInt16();
            Level4 = reader.ReadUInt16();
            Level5 = reader.ReadUInt16();
            Level6 = reader.ReadUInt16();
            Level7 = reader.ReadUInt16();
            Level8 = reader.ReadUInt16();
            Level9 = reader.ReadUInt16();
            Level10 = reader.ReadUInt16();
            Need1 = reader.ReadUInt16();
            Need2 = reader.ReadUInt16();
            Need3 = reader.ReadUInt16();
            Need4 = reader.ReadUInt16();
            Need5 = reader.ReadUInt16();
            Need6 = reader.ReadUInt16();
            Need7 = reader.ReadUInt16();
            Need8 = reader.ReadUInt16();
            Need9 = reader.ReadUInt16();
            Need10 = reader.ReadUInt16();
            DelayBase = reader.ReadUInt32();
            DelayReduction = reader.ReadUInt32();

            if (version > 66)
                Range = reader.ReadByte();

            PowerBase = reader.ReadUInt16();
            PowerBonus = reader.ReadUInt16();
            MPowerBase = reader.ReadUInt16();
            MPowerBonus = reader.ReadUInt16();
            MultiplierBase = reader.ReadSingle();
            MultiplierBonus = reader.ReadSingle();

            PvPPowerBase = reader.ReadUInt16();
            PvPPowerBonus = reader.ReadUInt16();
            PvPMPowerBase = reader.ReadUInt16();
            PvPMPowerBonus = reader.ReadUInt16();
            PvPMultiplierBase = reader.ReadSingle();
            PvPMultiplierBonus = reader.ReadSingle();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write((byte)Spell);
            writer.Write(BaseCost);
            writer.Write(LevelCost);
            writer.Write(Icon);
            writer.Write(Level1);
            writer.Write(Level2);
            writer.Write(Level3);
            writer.Write(Level4);
            writer.Write(Level5);
            writer.Write(Level6);
            writer.Write(Level7);
            writer.Write(Level8);
            writer.Write(Level9);
            writer.Write(Level10);
            writer.Write(Need1);
            writer.Write(Need2);
            writer.Write(Need3);
            writer.Write(Need4);
            writer.Write(Need5);
            writer.Write(Need6);
            writer.Write(Need7);
            writer.Write(Need8);
            writer.Write(Need9);
            writer.Write(Need10);
            writer.Write(DelayBase);
            writer.Write(DelayReduction);
            writer.Write(Range);
            writer.Write(PowerBase);
            writer.Write(PowerBonus);
            writer.Write(MPowerBase);
            writer.Write(MPowerBonus);
            writer.Write(MultiplierBase);
            writer.Write(MultiplierBonus);
            writer.Write(PvPPowerBase);
            writer.Write(PvPPowerBonus);
            writer.Write(PvPMPowerBase);
            writer.Write(PvPMPowerBonus);
            writer.Write(PvPMultiplierBase);
            writer.Write(PvPMultiplierBonus);
        }
    }

    public class UserMagic
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public Spell Spell;
        public MagicInfo Info;

        public byte Level, Key;
        public ushort Experience;
        public bool IsTempSpell;
        public long CastTime;

        private MagicInfo GetMagicInfo(Spell spell)
        {
            for (int i = 0; i < Envir.MagicInfoList.Count; i++)
            {
                MagicInfo info = Envir.MagicInfoList[i];
                if (info.Spell != spell) continue;
                return info;
            }
            return null;
        }

        public UserMagic(Spell spell)
        {
            Spell = spell;
            
            Info = GetMagicInfo(Spell);
        }
        public UserMagic(BinaryReader reader)
        {
            Spell = (Spell) reader.ReadByte();
            Info = GetMagicInfo(Spell);

            Level = reader.ReadByte();
            Key = reader.ReadByte();
            Experience = reader.ReadUInt16();

            if (Envir.LoadVersion < 15) return;
            IsTempSpell = reader.ReadBoolean();

            if (Envir.LoadVersion < 65) return;
            CastTime = reader.ReadInt64();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write((byte) Spell);

            writer.Write(Level);
            writer.Write(Key);
            writer.Write(Experience);
            writer.Write(IsTempSpell);
            writer.Write(CastTime);
        }

        public Packet GetInfo()
        {
            return new S.NewMagic
                {
                    Magic = CreateClientMagic()
                };
        }

        public ClientMagic CreateClientMagic()
        {
            return new ClientMagic
            {
                Name = Info.Name,
                Spell = Spell,
                BaseCost = Info.BaseCost,
                LevelCost = Info.LevelCost,
                Icon = Info.Icon,
                Level1 = Info.Level1,
                Level2 = Info.Level2,
                Level3 = Info.Level3,
                Level4 = Info.Level4,
                Level5 = Info.Level5,
                Level6 = Info.Level6,
                Level7 = Info.Level7,
                Level8 = Info.Level8,
                Level9 = Info.Level9,
                Level10 = Info.Level10,
                Need1 = Info.Need1,
                Need2 = Info.Need2,
                Need3 = Info.Need3,
                Need4 = Info.Need4,
                Need5 = Info.Need5,
                Need6 = Info.Need6,
                Need7 = Info.Need7,
                Need8 = Info.Need8,
                Need9 = Info.Need9,
                Need10 = Info.Need10,
                Level = Level,
                Key = Key,
                Experience = Experience,
                IsTempSpell = IsTempSpell,
                Delay = GetDelay(),
                Range = Info.Range,
                CastTime = CastTime - Envir.Time,
                PowerBase = Info.PowerBase,
                PowerBonus = Info.PowerBonus,
                MPowerBase = Info.MPowerBase,
                MPowerBonus = Info.MPowerBonus,
                MultiplierBase = Info.MultiplierBase,
                MultiplierBonus = Info.MultiplierBonus,
                PvPPowerBase = Info.PvPPowerBase,
                PvPPowerBonus = Info.PvPPowerBonus,
                PvPMPowerBase = Info.PvPMPowerBase,
                PvPMPowerBonus = Info.PvPMPowerBonus,
                PvPMultiplierBase = Info.PvPMultiplierBase,
                PvPMultiplierBonus = Info.PvPMultiplierBonus,
            };
        }

        public int GetDamage(int DamageBase)
        {
            return (int)((DamageBase + GetPower()) * GetMultiplier());
        }

        public float GetMultiplier()
        {
            return (Info.MultiplierBase + (Level * Info.MultiplierBonus));
        }

        public int GetPower()
        {
            return (int)Math.Round((MPower() / 4F) * (Level + 1) + DefPower());
        }

        public int MPower()
        {
            if (Info.MPowerBonus > 0)
            {
                return Envir.Random.Next(Info.MPowerBase, Info.MPowerBonus + Info.MPowerBase);
            }
            else
                return Info.MPowerBase;
        }
        public int DefPower()
        {
            if (Info.PowerBonus > 0)
            {
                return Envir.Random.Next(Info.PowerBase, Info.PowerBonus + Info.PowerBase);
            }
            else
                return Info.PowerBase;
        }

        public int GetPower(int power)
        {
            return (int)Math.Round(power / 4F * (Level + 1) + DefPower());
        }

        public int PvPGetDamage(int PvPDamageBase)
        {
            return (int)((PvPDamageBase + PvPGetPower()) * PvPGetMultiplier());
        }

        public float PvPGetMultiplier()
        {
            return (Info.PvPMultiplierBase + (Level * Info.PvPMultiplierBonus));
        }

        public int PvPGetPower()
        {
            return (int)Math.Round((PvPMPower() / 4F) * (Level + 1) + PvPDefPower());
        }

        public int PvPMPower()
        {
            if (Info.PvPMPowerBonus > 0)
            {
                return Envir.Random.Next(Info.PvPMPowerBase, Info.PvPMPowerBonus + Info.PvPMPowerBase);
            }
            else
                return Info.PvPMPowerBase;
        }
        public int PvPDefPower()
        {
            if (Info.PvPPowerBonus > 0)
            {
                return Envir.Random.Next(Info.PvPPowerBase, Info.PvPPowerBonus + Info.PvPPowerBase);
            }
            else
                return Info.PvPPowerBase;
        }

        public int PvPGetPower(int pvppower)
        {
            return (int)Math.Round(pvppower / 4F * (Level + 1) + PvPDefPower());
        }

        public long GetDelay()
        {
            return Info.DelayBase - (Level * Info.DelayReduction);
        }
    }
}
