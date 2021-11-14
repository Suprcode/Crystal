using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class ItemInfo
{
    public int Index;
    public string Name = string.Empty;
    public ItemType Type;
    public ItemGrade Grade;
    public RequiredType RequiredType = RequiredType.Level;
    public RequiredClass RequiredClass = RequiredClass.None;
    public RequiredGender RequiredGender = RequiredGender.None;
    public ItemSet Set;

    public short Shape;
    public byte Weight, Light;

    public ushort Image, Durability, RequiredAmount;

    public uint Price; 
    public ushort StackSize = 1;

    public bool StartItem;
    public byte Effect;

    public byte ItemGlow;

    public bool NeedIdentify, ShowGroupPickup, GlobalDropNotify;
    public bool ClassBased;
    public bool LevelBased;
    public bool CanMine;
    public bool CanFastRun;
    public bool CanAwakening;

    public BindMode Bind = BindMode.None;

    public SpecialItemMode Unique = SpecialItemMode.None;
    public byte RandomStatsId;
    public RandomItemStat RandomStats;
    public string ToolTip = string.Empty;

    public byte Slots;

    public Stats Stats;

    public bool AllowLvlSys = false;
    public bool AllowRandomStats = false;
    public int[] LvlSysExp = new int[10];
    public int[] LvlSysMaxAC = new int[10];
    public int[] LvlSysMaxMAC = new int[10];
    public int[] LvlSysMaxSC = new int[10];
    public int[] LvlSysMaxMC = new int[10];
    public int[] LvlSysMaxDC = new int[10];
    /*
    public int[] LvlSysAccuracy = new int[10];
    public int[] LvlSysAgility = new int[10];
    public int[] LvlSysHP = new int[10];
    public int[] LvlSysMP = new int[10];
    public int[] LvlSysPoisonRecovery = new int[10];
    public int[] LvlSysPoisonAttack = new int[10];
    public int[] LvlSysPoisonResist = new int[10];
    public int[] LvlSysMagicResist = new int[10];
    public int[] LvlSysFreezing = new int[10];
    public int[] LvlSysLuck = new int[10];
    public int[] LvlSysHealthRecovery = new int[10];
    public int[] LvlSysSpellRecovery = new int[10];
    public int[] LvlSysReflect = new int[10];
    public int[] LvlSysAttackSpeed = new int[10];
    public int[] LvlSysAttackSpeedRatePercent = new int[10];
    public int[] LvlSysHPRatePercent = new int[10];
    public int[] LvlSysMPRatePercent = new int[10];
    public int[] LvlSysMaxACRatePercent = new int[10];
    public int[] LvlSysMaxMACRatePercent = new int[10];
    public int[] LvlSysMaxDCRatePercent = new int[10];
    public int[] LvlSysMaxMCRatePercent = new int[10];
    public int[] LvlSysMaxSCRatePercent = new int[10];
    public int[] LvlSysGoldDropRatePercent = new int[10];
    public int[] LvlSysExpRatePercent = new int[10];
    public int[] LvlSysItemDropRatePercent = new int[10];
    public int[] LvlSysPVEDamage = new int[10];
    public int[] LvlSysPVPDamage = new int[10];
    public int[] LvlSysCriticalDamage = new int[10];
    public int[] LvlSysCriticalRate = new int[10];
    public int[] LvlSysDamageReductionPercent = new int[10];
    public int[] LvlSysSkillGainMultiplier = new int[10];
    */
    public byte BaseRate, BaseRateDrop, MaxStats, MaxGemStat;

    public bool IsConsumable
    {
        get { return Type == ItemType.Potion || Type == ItemType.Scroll || Type == ItemType.Food || Type == ItemType.Transform || Type == ItemType.Script; }
    }
    public bool IsFishingRod
    {
        get { return Globals.FishingRodShapes.Contains(Shape); }
    }

    public string FriendlyName
    {
        get
        {
            string temp = Name;
            temp = Regex.Replace(temp, @"\d+$", string.Empty); //hides end numbers
            temp = Regex.Replace(temp, @"\[[^]]*\]", string.Empty); //hides square brackets

            return temp;
        }
    }

    public ItemInfo() 
    {
        Stats = new Stats();
    }

    public ItemInfo(BinaryReader reader, int version = int.MaxValue, int customVersion = int.MaxValue)
    {
        Index = reader.ReadInt32();
        Name = reader.ReadString();
        Type = (ItemType)reader.ReadByte();
        Grade = (ItemGrade)reader.ReadByte();
        RequiredType = (RequiredType)reader.ReadByte();
        RequiredClass = (RequiredClass)reader.ReadByte();
        RequiredGender = (RequiredGender)reader.ReadByte();
        Set = (ItemSet)reader.ReadByte();

        Shape = reader.ReadInt16();
        Weight = reader.ReadByte();
        Light = reader.ReadByte();

        RequiredAmount = reader.ReadUInt16();
        Image = reader.ReadUInt16();
        Durability = reader.ReadUInt16();

        if (version <= 84)
        {
            StackSize = (ushort)reader.ReadUInt32();
        }
        else
        {
            StackSize = reader.ReadUInt16();
        }

        Price = reader.ReadUInt32();

        if (version <= 84)
        {
            Stats = new Stats();
            Stats[Stat.MinAC] = reader.ReadByte();
            Stats[Stat.MaxAC] = reader.ReadByte();
            Stats[Stat.MinMAC] = reader.ReadByte();
            Stats[Stat.MaxMAC] = reader.ReadByte();
            Stats[Stat.MinDC] = reader.ReadByte();
            Stats[Stat.MaxDC] = reader.ReadByte();
            Stats[Stat.MinMC] = reader.ReadByte();
            Stats[Stat.MaxMC] = reader.ReadByte();
            Stats[Stat.MinSC] = reader.ReadByte();
            Stats[Stat.MaxSC] = reader.ReadByte();
            Stats[Stat.HP] = reader.ReadUInt16();
            Stats[Stat.MP] = reader.ReadUInt16();
            Stats[Stat.Accuracy] = reader.ReadByte();
            Stats[Stat.Agility] = reader.ReadByte();

            Stats[Stat.Luck] = reader.ReadSByte();
            Stats[Stat.AttackSpeed] = reader.ReadSByte();
        }

        StartItem = reader.ReadBoolean();

        if (version <= 84)
        {
            Stats[Stat.BagWeight] = reader.ReadByte();
            Stats[Stat.HandWeight] = reader.ReadByte();
            Stats[Stat.WearWeight] = reader.ReadByte();
        }

        Effect = reader.ReadByte();

        if (version <= 84)
        {
            Stats[Stat.Strong] = reader.ReadByte();
            Stats[Stat.MagicResist] = reader.ReadByte();
            Stats[Stat.PoisonResist] = reader.ReadByte();
            Stats[Stat.HealthRecovery] = reader.ReadByte();
            Stats[Stat.SpellRecovery] = reader.ReadByte();
            Stats[Stat.PoisonRecovery] = reader.ReadByte();
            Stats[Stat.HPRatePercent] = reader.ReadByte();
            Stats[Stat.MPRatePercent] = reader.ReadByte();
            Stats[Stat.CriticalRate] = reader.ReadByte();
            Stats[Stat.CriticalDamage] = reader.ReadByte();
        }


        byte bools = reader.ReadByte();
        NeedIdentify = (bools & 0x01) == 0x01;
        ShowGroupPickup = (bools & 0x02) == 0x02;
        ClassBased = (bools & 0x04) == 0x04;
        LevelBased = (bools & 0x08) == 0x08;
        CanMine = (bools & 0x10) == 0x10;

        if (version >= 77)
        {
            GlobalDropNotify = (bools & 0x20) == 0x20;
        }

        if (version <= 84)
        {
            Stats[Stat.MaxACRatePercent] = reader.ReadByte();
            Stats[Stat.MaxMACRatePercent] = reader.ReadByte();
            Stats[Stat.Holy] = reader.ReadByte();
            Stats[Stat.Freezing] = reader.ReadByte();
            Stats[Stat.PoisonAttack] = reader.ReadByte();
        }

        Bind = (BindMode)reader.ReadInt16();

        if (version <= 84)
        {
            Stats[Stat.Reflect] = reader.ReadByte();
            Stats[Stat.HPDrainRatePercent] = reader.ReadByte();
        }

        Unique = (SpecialItemMode)reader.ReadInt16();
        RandomStatsId = reader.ReadByte();

        CanFastRun = reader.ReadBoolean();

        CanAwakening = reader.ReadBoolean();

        if (version > 83)
        {
            Slots = reader.ReadByte();
        }

        if (version > 84)
        {
            Stats = new Stats(reader);
        }

        bool isTooltip = reader.ReadBoolean();
        if (isTooltip)
            ToolTip = reader.ReadString();

        if (version < 70) //before db version 70 all specialitems had wedding rings disabled, after that it became a server option
        {
            if ((Type == ItemType.Ring) && (Unique != SpecialItemMode.None))
                Bind |= BindMode.NoWeddingRing;
        }

        if (version > 11)
        {
            AllowLvlSys = reader.ReadBoolean();
            AllowRandomStats = reader.ReadBoolean();

            for (int i = 0; i < LvlSysExp.Length; i++)
                LvlSysExp[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxAC.Length; i++)
                LvlSysMaxAC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxMAC.Length; i++)
                LvlSysMaxMAC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxDC.Length; i++)
                LvlSysMaxDC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxMC.Length; i++)
                LvlSysMaxMC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxSC.Length; i++)
                LvlSysMaxSC[i] = reader.ReadInt32();

            #region Disable ItemLvl
            /*
            for (int i = 0; i < LvlSysAccuracy.Length; i++)
                LvlSysAccuracy[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysAgility.Length; i++)
                LvlSysAgility[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysHP.Length; i++)
                LvlSysHP[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMP.Length; i++)
                LvlSysMP[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysPoisonRecovery.Length; i++)
                LvlSysPoisonRecovery[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysPoisonAttack.Length; i++)
                LvlSysPoisonAttack[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysPoisonResist.Length; i++)
                LvlSysPoisonResist[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMagicResist.Length; i++)
                LvlSysMagicResist[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysFreezing.Length; i++)
                LvlSysFreezing[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysLuck.Length; i++)
                LvlSysLuck[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysHealthRecovery.Length; i++)
                LvlSysHealthRecovery[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysSpellRecovery.Length; i++)
                LvlSysSpellRecovery[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysReflect.Length; i++)
                LvlSysReflect[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysAttackSpeed.Length; i++)
                LvlSysAttackSpeed[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysAttackSpeedRatePercent.Length; i++)
                LvlSysAttackSpeedRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysHPRatePercent.Length; i++)
                LvlSysHPRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMPRatePercent.Length; i++)
                LvlSysMPRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxACRatePercent.Length; i++)
                LvlSysMaxACRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxMACRatePercent.Length; i++)
                LvlSysMaxMACRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxDCRatePercent.Length; i++)
                LvlSysMaxDCRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxMCRatePercent.Length; i++)
                LvlSysMaxMCRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxSCRatePercent.Length; i++)
                LvlSysMaxSCRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysGoldDropRatePercent.Length; i++)
                LvlSysGoldDropRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysExpRatePercent.Length; i++)
                LvlSysExpRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysItemDropRatePercent.Length; i++)
                LvlSysItemDropRatePercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysPVEDamage.Length; i++)
                LvlSysPVEDamage[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysPVPDamage.Length; i++)
                LvlSysPVPDamage[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysCriticalDamage.Length; i++)
                LvlSysCriticalDamage[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysCriticalRate.Length; i++)
                LvlSysCriticalRate[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysDamageReductionPercent.Length; i++)
                LvlSysDamageReductionPercent[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysSkillGainMultiplier.Length; i++)
                LvlSysSkillGainMultiplier[i] = reader.ReadInt32();
            */
            #endregion
        }

        if (version > 130)
        {
            BaseRate = reader.ReadByte();
            BaseRateDrop = reader.ReadByte();
            MaxStats = reader.ReadByte();
            MaxGemStat = reader.ReadByte();
        }

        ItemGlow = reader.ReadByte();
    }

    public static ItemInfo CloneItem(ItemInfo item)
    {
        ItemInfo Clone = new ItemInfo
        {
            Index = item.Index,
            Name = item.Name,
            Type = item.Type,
            Grade = item.Grade,
            RequiredType = item.RequiredType,
            RequiredClass = item.RequiredClass,
            RequiredGender = item.RequiredGender,
            Set = item.Set,

            Shape = item.Shape,
            Weight = item.Weight,
            Light = item.Light,

            RequiredAmount = item.RequiredAmount,
            Image = item.Image,
            Durability = item.Durability,

            StackSize = item.StackSize,
            Price = item.Price,

            StartItem = item.StartItem,
            Effect = item.Effect,

            NeedIdentify = item.NeedIdentify,
            ShowGroupPickup = item.ShowGroupPickup,
            ClassBased = item.ClassBased,
            LevelBased = item.LevelBased,
            CanMine = item.CanMine,

            GlobalDropNotify = item.GlobalDropNotify,
            Bind = item.Bind,
            Unique = item.Unique,
            RandomStatsId = item.RandomStatsId,
            CanFastRun = item.CanFastRun,
            CanAwakening = item.CanAwakening,
            Slots = item.Slots,
            Stats = item.Stats,
            ToolTip = item.ToolTip,

            RandomStats = item.RandomStats,
            ItemGlow = item.ItemGlow,

            AllowLvlSys = item.AllowLvlSys,
            AllowRandomStats = item.AllowRandomStats,
            LvlSysExp = item.LvlSysExp,
            LvlSysMaxAC = item.LvlSysMaxAC,
            LvlSysMaxMAC = item.LvlSysMaxMAC,
            LvlSysMaxSC = item.LvlSysMaxSC,
            LvlSysMaxDC = item.LvlSysMaxDC,
            LvlSysMaxMC = item.LvlSysMaxMC,

            BaseRate = item.BaseRate,
            BaseRateDrop = item.BaseRateDrop,
            MaxStats = item.MaxStats,
            MaxGemStat = item.MaxGemStat,
        };

        return Clone;
    }

    public void UpdateItem(ItemInfo item)
    {
        Index = item.Index;
        Name = item.Name;
        Type = item.Type;
        Grade = item.Grade;
        RequiredType = item.RequiredType;
        RequiredClass = item.RequiredClass;
        RequiredGender = item.RequiredGender;
        Set = item.Set;

        Shape = item.Shape;
        Weight = item.Weight;
        Light = item.Light;

        RequiredAmount = item.RequiredAmount;
        Image = item.Image;
        Durability = item.Durability;

        StackSize = item.StackSize;
        Price = item.Price;

        StartItem = item.StartItem;
        Effect = item.Effect;

        NeedIdentify = item.NeedIdentify;
        ShowGroupPickup = item.ShowGroupPickup;
        ClassBased = item.ClassBased;
        LevelBased = item.LevelBased;
        CanMine = item.CanMine;

        GlobalDropNotify = item.GlobalDropNotify;
        Bind = item.Bind;
        Unique = item.Unique;
        RandomStatsId = item.RandomStatsId;
        CanFastRun = item.CanFastRun;
        CanAwakening = item.CanAwakening;
        Slots = item.Slots;
        Stats = item.Stats;
        ToolTip = item.ToolTip;

        RandomStats = item.RandomStats;
        ItemGlow = item.ItemGlow;

        AllowLvlSys = item.AllowLvlSys;
        AllowRandomStats = item.AllowRandomStats;
        LvlSysExp = item.LvlSysExp;
        LvlSysMaxAC = item.LvlSysMaxAC;
        LvlSysMaxMAC = item.LvlSysMaxMAC;
        LvlSysMaxSC = item.LvlSysMaxSC;
        LvlSysMaxDC = item.LvlSysMaxDC;
        LvlSysMaxMC = item.LvlSysMaxMC;

        BaseRate = item.BaseRate;
        BaseRateDrop = item.BaseRateDrop;
        MaxStats = item.MaxStats;
        MaxGemStat = item.MaxGemStat;
    }



    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        writer.Write(Name);
        writer.Write((byte)Type);
        writer.Write((byte)Grade);
        writer.Write((byte)RequiredType);
        writer.Write((byte)RequiredClass);
        writer.Write((byte)RequiredGender);
        writer.Write((byte)Set);

        writer.Write(Shape);
        writer.Write(Weight);
        writer.Write(Light);
        writer.Write(RequiredAmount);

        writer.Write(Image);
        writer.Write(Durability);

        writer.Write(StackSize);
        writer.Write(Price);

        writer.Write(StartItem);

        writer.Write(Effect);

        byte bools = 0;
        if (NeedIdentify) bools |= 0x01;
        if (ShowGroupPickup) bools |= 0x02;
        if (ClassBased) bools |= 0x04;
        if (LevelBased) bools |= 0x08;
        if (CanMine) bools |= 0x10;
        if (GlobalDropNotify) bools |= 0x20;
        writer.Write(bools);
        
        writer.Write((short)Bind);        
        writer.Write((short)Unique);

        writer.Write(RandomStatsId);

        writer.Write(CanFastRun);
        writer.Write(CanAwakening);
        writer.Write(Slots);

        Stats.Save(writer);

        writer.Write(ToolTip != null);
        if (ToolTip != null)
            writer.Write(ToolTip);

        writer.Write(AllowLvlSys);
        writer.Write(AllowRandomStats);

        for (int i = 0; i < LvlSysExp.Length; i++)
        {
            writer.Write(LvlSysExp[i]);
        }
        for (int i = 0; i < LvlSysMaxAC.Length; i++)
        {
            writer.Write(LvlSysMaxAC[i]);
        }
        for (int i = 0; i < LvlSysMaxMAC.Length; i++)
        {
            writer.Write(LvlSysMaxMAC[i]);
        }
        for (int i = 0; i < LvlSysMaxDC.Length; i++)
        {
            writer.Write(LvlSysMaxDC[i]);
        }
        for (int i = 0; i < LvlSysMaxMC.Length; i++)
        {
            writer.Write(LvlSysMaxMC[i]);
        }
        for (int i = 0; i < LvlSysMaxSC.Length; i++)
        {
            writer.Write(LvlSysMaxSC[i]);
        }
        #region Disable ItemLvl
        /*
        for (int i = 0; i < LvlSysAccuracy.Length; i++)
        {
            writer.Write(LvlSysAccuracy[i]);
        }
        for (int i = 0; i < LvlSysAgility.Length; i++)
        {
            writer.Write(LvlSysAgility[i]);
        }
        for (int i = 0; i < LvlSysHP.Length; i++)
        {
            writer.Write(LvlSysHP[i]);
        }
        for (int i = 0; i < LvlSysMP.Length; i++)
        {
            writer.Write(LvlSysMP[i]);
        }
        for (int i = 0; i < LvlSysPoisonRecovery.Length; i++)
        {
            writer.Write(LvlSysPoisonRecovery[i]);
        }
        for (int i = 0; i < LvlSysPoisonAttack.Length; i++)
        {
            writer.Write(LvlSysPoisonAttack[i]);
        }
        for (int i = 0; i < LvlSysPoisonResist.Length; i++)
        {
            writer.Write(LvlSysPoisonResist[i]);
        }
        for (int i = 0; i < LvlSysMagicResist.Length; i++)
        {
            writer.Write(LvlSysMagicResist[i]);
        }
        for (int i = 0; i < LvlSysFreezing.Length; i++)
        {
            writer.Write(LvlSysFreezing[i]);
        }
        for (int i = 0; i < LvlSysLuck.Length; i++)
        {
            writer.Write(LvlSysLuck[i]);
        }
        for (int i = 0; i < LvlSysHealthRecovery.Length; i++)
        {
            writer.Write(LvlSysHealthRecovery[i]);
        }
        for (int i = 0; i < LvlSysSpellRecovery.Length; i++)
        {
            writer.Write(LvlSysSpellRecovery[i]);
        }
        for (int i = 0; i < LvlSysReflect.Length; i++)
        {
            writer.Write(LvlSysReflect[i]);
        }
        for (int i = 0; i < LvlSysAttackSpeed.Length; i++)
        {
            writer.Write(LvlSysAttackSpeed[i]);
        }
        for (int i = 0; i < LvlSysAttackSpeedRatePercent.Length; i++)
        {
            writer.Write(LvlSysAttackSpeedRatePercent[i]);
        }
        for (int i = 0; i < LvlSysHPRatePercent.Length; i++)
        {
            writer.Write(LvlSysHPRatePercent[i]);
        }
        for (int i = 0; i < LvlSysMPRatePercent.Length; i++)
        {
            writer.Write(LvlSysMPRatePercent[i]);
        }
        for (int i = 0; i < LvlSysMaxACRatePercent.Length; i++)
        {
            writer.Write(LvlSysMaxACRatePercent[i]);
        }
        for (int i = 0; i < LvlSysMaxMACRatePercent.Length; i++)
        {
            writer.Write(LvlSysMaxMACRatePercent[i]);
        }
        for (int i = 0; i < LvlSysMaxDCRatePercent.Length; i++)
        {
            writer.Write(LvlSysMaxDCRatePercent[i]);
        }
        for (int i = 0; i < LvlSysMaxMCRatePercent.Length; i++)
        {
            writer.Write(LvlSysMaxMCRatePercent[i]);
        }
        for (int i = 0; i < LvlSysMaxSCRatePercent.Length; i++)
        {
            writer.Write(LvlSysMaxSCRatePercent[i]);
        }
        for (int i = 0; i < LvlSysGoldDropRatePercent.Length; i++)
        {
            writer.Write(LvlSysGoldDropRatePercent[i]);
        }
        for (int i = 0; i < LvlSysExpRatePercent.Length; i++)
        {
            writer.Write(LvlSysExpRatePercent[i]);
        }
        for (int i = 0; i < LvlSysItemDropRatePercent.Length; i++)
        {
            writer.Write(LvlSysItemDropRatePercent[i]);
        }
        for (int i = 0; i < LvlSysPVEDamage.Length; i++)
        {
            writer.Write(LvlSysPVEDamage[i]);
        }
        for (int i = 0; i < LvlSysPVPDamage.Length; i++)
        {
            writer.Write(LvlSysPVPDamage[i]);
        }
        for (int i = 0; i < LvlSysCriticalDamage.Length; i++)
        {
            writer.Write(LvlSysCriticalDamage[i]);
        }
        for (int i = 0; i < LvlSysCriticalRate.Length; i++)
        {
            writer.Write(LvlSysCriticalRate[i]);
        }
        for (int i = 0; i < LvlSysDamageReductionPercent.Length; i++)
        {
            writer.Write(LvlSysDamageReductionPercent[i]);
        }
        for (int i = 0; i < LvlSysSkillGainMultiplier.Length; i++)
        {
            writer.Write(LvlSysSkillGainMultiplier[i]);
        }
        */
        #endregion

        writer.Write(BaseRate);
        writer.Write(BaseRateDrop);
        writer.Write(MaxStats);
        writer.Write(MaxGemStat);
        writer.Write(ItemGlow);
    }

    public static ItemInfo FromText(string text)
    {
        return null;
    }

    public string ToText()
    {
        return null;
    }

    public override string ToString()
    {
        return string.Format("{0}: {1}", Index, Name);
    }
}

public class UserItem
{
    public ulong UniqueID;
    public int ItemIndex;

    public ItemInfo Info;
    public ushort CurrentDura, MaxDura;
    public ushort Count = 1,
                GemCount = 0;

    public RefinedValue RefinedValue = RefinedValue.None;
    public byte RefineAdded = 0;
    public int RefineSuccessChance = 0;

    public bool DuraChanged;
    public int SoulBoundId = -1;
    public bool Identified = false;
    public bool Cursed = false;

    public int WeddingRing = -1;

    public UserItem[] Slots = new UserItem[0];

    public DateTime BuybackExpiryDate;

    public ExpireInfo ExpireInfo;
    public RentalInformation RentalInformation;

    public bool IsShopItem;

    public Awake Awake = new Awake();

    public Stats AddedStats;

    public int LvlSysExpGained;
    public int LvlSystem;
    public string GTInvite { get; set; } = string.Empty;

    public bool IsAdded
    {
        get { return AddedStats.Count > 0 || Slots.Length > Info.Slots; }
    }

    public int Weight
    {
        get { return Info.Type == ItemType.Amulet ? Info.Weight : Info.Type == ItemType.TaoPoison ? Info.Weight : Info.Weight * Count; }
    }

    public string Name
    {
        get { return Count > 1 ? string.Format("{0} ({1})", Info.Name, Count) : Info.Name; }
    }

    public string FriendlyName
    {
        get { return Count > 1 ? string.Format("{0} ({1})", Info.FriendlyName, Count) : Info.FriendlyName; }
    }

    public UserItem(ItemInfo info)
    {
        SoulBoundId = -1;
        ItemIndex = info.Index;
        Info = info;
        AddedStats = new Stats();

        SetSlotSize();
    }
    public UserItem(BinaryReader reader, int version = int.MaxValue, int customVersion = int.MaxValue)
    {
        UniqueID = reader.ReadUInt64();
        ItemIndex = reader.ReadInt32();

        CurrentDura = reader.ReadUInt16();
        MaxDura = reader.ReadUInt16();

        if (version <= 84)
        {
            Count = (ushort)reader.ReadUInt32();
        }
        else
        {
            Count = reader.ReadUInt16();
        }

        if (version <= 84)
        {
            AddedStats = new Stats();

            AddedStats[Stat.MaxAC] = reader.ReadByte();
            AddedStats[Stat.MaxMAC] = reader.ReadByte();
            AddedStats[Stat.MaxDC] = reader.ReadByte();
            AddedStats[Stat.MaxMC] = reader.ReadByte();
            AddedStats[Stat.MaxSC] = reader.ReadByte();

            AddedStats[Stat.Accuracy] = reader.ReadByte();
            AddedStats[Stat.Agility] = reader.ReadByte();
            AddedStats[Stat.HP] = reader.ReadByte();
            AddedStats[Stat.MP] = reader.ReadByte();

            AddedStats[Stat.AttackSpeed] = reader.ReadSByte();
            AddedStats[Stat.Luck] = reader.ReadSByte();
        }

        SoulBoundId = reader.ReadInt32();
        byte Bools = reader.ReadByte();
        Identified = (Bools & 0x01) == 0x01;
        Cursed = (Bools & 0x02) == 0x02;

        if (version <= 84)
        {
            AddedStats[Stat.Strong] = reader.ReadByte();
            AddedStats[Stat.MagicResist] = reader.ReadByte();
            AddedStats[Stat.PoisonResist] = reader.ReadByte();
            AddedStats[Stat.HealthRecovery] = reader.ReadByte();
            AddedStats[Stat.SpellRecovery] = reader.ReadByte();
            AddedStats[Stat.PoisonRecovery] = reader.ReadByte();
            AddedStats[Stat.CriticalRate] = reader.ReadByte();
            AddedStats[Stat.CriticalDamage] = reader.ReadByte();
            AddedStats[Stat.Freezing] = reader.ReadByte();
            AddedStats[Stat.PoisonAttack] = reader.ReadByte();
            AddedStats[Stat.AttackSpeedRatePercent] = reader.ReadByte();
            AddedStats[Stat.HPRatePercent] = reader.ReadByte();
            AddedStats[Stat.MPRatePercent] = reader.ReadByte();
            AddedStats[Stat.MaxACRatePercent] = reader.ReadByte();
            AddedStats[Stat.MaxMACRatePercent] = reader.ReadByte();
            AddedStats[Stat.MaxDCRatePercent] = reader.ReadByte();
            AddedStats[Stat.MaxMCRatePercent] = reader.ReadByte();
            AddedStats[Stat.MaxSCRatePercent] = reader.ReadByte();
        }

        int count = reader.ReadInt32();

        SetSlotSize(count);

        for (int i = 0; i < count; i++)
        {
            if (reader.ReadBoolean()) continue;
            UserItem item = new UserItem(reader, version, customVersion);
            Slots[i] = item;
        }

        if (version <= 84)
        {
            GemCount = (ushort)reader.ReadUInt32();
        }
        else
        {
            GemCount = reader.ReadUInt16();
        }

        if (version > 84)
        {
            AddedStats = new Stats(reader);
        }

        Awake = new Awake(reader);

        RefinedValue = (RefinedValue)reader.ReadByte();
        RefineAdded = reader.ReadByte();

        if (version > 85)
        {
            RefineSuccessChance = reader.ReadInt32();
        }

        WeddingRing = reader.ReadInt32();

        if (version < 65) return;

        if (reader.ReadBoolean())
            ExpireInfo = new ExpireInfo(reader, version, customVersion);

        if (version < 76)
            return;

        if (reader.ReadBoolean())
            RentalInformation = new RentalInformation(reader, version, customVersion);

        if (version < 83) return;

        IsShopItem = reader.ReadBoolean();
<<<<<<< HEAD

        if (version > 12)
        {
            LvlSysExpGained = reader.ReadInt32();
            LvlSystem = reader.ReadInt32();
        }

        if (version > 112)
            GTInvite = reader.ReadString();
=======
>>>>>>> parent of 3b68b8d (Item Seals. Type = Gem, Shape = 8, Durability = Minutes)
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(UniqueID);
        writer.Write(ItemIndex);

        writer.Write(CurrentDura);
        writer.Write(MaxDura);

        writer.Write(Count);
       
        writer.Write(SoulBoundId);
        byte Bools = 0;
        if (Identified) Bools |= 0x01;
        if (Cursed) Bools |= 0x02;
        writer.Write(Bools);

        writer.Write(Slots.Length);
        for (int i = 0; i < Slots.Length; i++)
        {
            writer.Write(Slots[i] == null);
            if (Slots[i] == null) continue;

            Slots[i].Save(writer);
        }

        writer.Write(GemCount);


        AddedStats.Save(writer);
        Awake.Save(writer);

        writer.Write((byte)RefinedValue);
        writer.Write(RefineAdded);
        writer.Write(RefineSuccessChance);

        writer.Write(WeddingRing);

        writer.Write(ExpireInfo != null);
        ExpireInfo?.Save(writer);

        writer.Write(RentalInformation != null);
        RentalInformation?.Save(writer);

        writer.Write(IsShopItem);
<<<<<<< HEAD

        writer.Write(LvlSysExpGained);
        writer.Write(LvlSystem);

        writer.Write(GTInvite);
    }

    public void ItemLevelUp(ushort level, MirClass job, List<ItemInfo> list)
    {
        ItemInfo realItem = Functions.GetRealItem(Info, level, job, list);
        if (Info.AllowRandomStats)
        {

            Random rand = new Random(DateTime.Now.Millisecond);

            AddedStats[Stat.MaxAC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxAC] + rand.Next(0, realItem.LvlSysMaxAC[LvlSystem - 1]));
            AddedStats[Stat.MaxMAC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxMAC] + rand.Next(0, realItem.LvlSysMaxMAC[LvlSystem - 1]));
            AddedStats[Stat.MaxDC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxDC] + rand.Next(0, realItem.LvlSysMaxDC[LvlSystem - 1]));
            AddedStats[Stat.MaxMC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxMC] + rand.Next(0, realItem.LvlSysMaxMC[LvlSystem - 1]));
            AddedStats[Stat.MaxSC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxSC] + rand.Next(0, realItem.LvlSysMaxSC[LvlSystem - 1]));

            #region Disable ItemLvl
            /*
            AddedStats[Stat.Accuracy] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Accuracy] + rand.Next(0, realItem.LvlSysAccuracy[LvlSystem - 1]));
            AddedStats[Stat.Agility] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Agility] + rand.Next(0, realItem.LvlSysAgility[LvlSystem - 1]));
            AddedStats[Stat.HP] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.HP] + rand.Next(0, realItem.LvlSysHP[LvlSystem - 1]));
            AddedStats[Stat.MP] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MP] + rand.Next(0, realItem.LvlSysMP[LvlSystem - 1]));
            AddedStats[Stat.PoisonRecovery] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PoisonRecovery] + rand.Next(0, realItem.LvlSysPoisonRecovery[LvlSystem - 1]));
            AddedStats[Stat.PoisonAttack] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PoisonAttack] + rand.Next(0, realItem.LvlSysPoisonAttack[LvlSystem - 1]));
            AddedStats[Stat.PoisonResist] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PoisonResist] + rand.Next(0, realItem.LvlSysPoisonResist[LvlSystem - 1]));
            AddedStats[Stat.MagicResist] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MagicResist] + rand.Next(0, realItem.LvlSysMagicResist[LvlSystem - 1]));
            AddedStats[Stat.Freezing] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Freezing] + rand.Next(0, realItem.LvlSysFreezing[LvlSystem - 1]));
            AddedStats[Stat.Luck] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Luck] + rand.Next(0, realItem.LvlSysLuck[LvlSystem - 1]));
            AddedStats[Stat.HealthRecovery] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.HealthRecovery] + rand.Next(0, realItem.LvlSysHealthRecovery[LvlSystem - 1]));
            AddedStats[Stat.SpellRecovery] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.SpellRecovery] + rand.Next(0, realItem.LvlSysSpellRecovery[LvlSystem - 1]));
            AddedStats[Stat.Reflect] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Reflect] + rand.Next(0, realItem.LvlSysReflect[LvlSystem - 1]));
            AddedStats[Stat.AttackSpeed] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.AttackSpeed] + rand.Next(0, realItem.LvlSysAttackSpeed[LvlSystem - 1]));
            AddedStats[Stat.AttackSpeedRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.AttackSpeedRatePercent] + rand.Next(0, realItem.LvlSysAttackSpeedRatePercent[LvlSystem - 1]));
            AddedStats[Stat.HPRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.HPRatePercent] + rand.Next(0, realItem.LvlSysHPRatePercent[LvlSystem - 1]));
            AddedStats[Stat.MPRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MPRatePercent] + rand.Next(0, realItem.LvlSysMPRatePercent[LvlSystem - 1]));
            AddedStats[Stat.MaxACRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxACRatePercent] + rand.Next(0, realItem.LvlSysMaxACRatePercent[LvlSystem - 1]));
            AddedStats[Stat.MaxMACRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxMACRatePercent] + rand.Next(0, realItem.LvlSysMaxMACRatePercent[LvlSystem - 1]));
            AddedStats[Stat.MaxDCRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxDCRatePercent] + rand.Next(0, realItem.LvlSysMaxDCRatePercent[LvlSystem - 1]));
            AddedStats[Stat.MaxMCRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxMCRatePercent] + rand.Next(0, realItem.LvlSysMaxMCRatePercent[LvlSystem - 1]));
            AddedStats[Stat.MaxSCRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxSCRatePercent] + rand.Next(0, realItem.LvlSysMaxSCRatePercent[LvlSystem - 1]));
            AddedStats[Stat.GoldDropRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.GoldDropRatePercent] + rand.Next(0, realItem.LvlSysGoldDropRatePercent[LvlSystem - 1]));
            AddedStats[Stat.ExpRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.ExpRatePercent] + rand.Next(0, realItem.LvlSysExpRatePercent[LvlSystem - 1]));
            AddedStats[Stat.ItemDropRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.ItemDropRatePercent] + rand.Next(0, realItem.LvlSysItemDropRatePercent[LvlSystem - 1]));
            AddedStats[Stat.PVEDamage] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PVEDamage] + rand.Next(0, realItem.LvlSysPVEDamage[LvlSystem - 1]));
            AddedStats[Stat.PVPDamage] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PVPDamage] + rand.Next(0, realItem.LvlSysPVPDamage[LvlSystem - 1]));
            AddedStats[Stat.CriticalDamage] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.CriticalDamage] + rand.Next(0, realItem.LvlSysCriticalDamage[LvlSystem - 1]));
            AddedStats[Stat.CriticalRate] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.CriticalRate] + rand.Next(0, realItem.LvlSysCriticalRate[LvlSystem - 1]));
            AddedStats[Stat.DamageReductionPercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.DamageReductionPercent] + rand.Next(0, realItem.LvlSysDamageReductionPercent[LvlSystem - 1]));
            AddedStats[Stat.SkillGainMultiplier] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.SkillGainMultiplier] + rand.Next(0, realItem.LvlSysSkillGainMultiplier[LvlSystem - 1]));
            */
            #endregion
        }
        else
        {
            AddedStats[Stat.MaxAC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxAC] + realItem.LvlSysMaxAC[LvlSystem - 1]);
            AddedStats[Stat.MaxMAC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxMAC] + realItem.LvlSysMaxMAC[LvlSystem - 1]);
            AddedStats[Stat.MaxDC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxDC] + realItem.LvlSysMaxDC[LvlSystem - 1]);
            AddedStats[Stat.MaxMC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxMC] + realItem.LvlSysMaxMC[LvlSystem - 1]);
            AddedStats[Stat.MaxSC] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxSC] + realItem.LvlSysMaxSC[LvlSystem - 1]);

            #region Disable ItemLvl
            /*
            AddedStats[Stat.Accuracy] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Accuracy] + realItem.LvlSysAccuracy[LvlSystem - 1]);
            AddedStats[Stat.Agility] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Agility] + realItem.LvlSysAgility[LvlSystem - 1]);
            AddedStats[Stat.HP] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.HP] + realItem.LvlSysHP[LvlSystem - 1]);
            AddedStats[Stat.MP] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MP] + realItem.LvlSysMP[LvlSystem - 1]);
            AddedStats[Stat.PoisonRecovery] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PoisonRecovery] + realItem.LvlSysPoisonRecovery[LvlSystem - 1]);
            AddedStats[Stat.PoisonAttack] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PoisonAttack] + realItem.LvlSysPoisonAttack[LvlSystem - 1]);
            AddedStats[Stat.PoisonResist] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PoisonResist] + realItem.LvlSysPoisonResist[LvlSystem - 1]);
            AddedStats[Stat.MagicResist] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MagicResist] + realItem.LvlSysMagicResist[LvlSystem - 1]);
            AddedStats[Stat.Freezing] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Freezing] + realItem.LvlSysFreezing[LvlSystem - 1]);
            AddedStats[Stat.Luck] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Luck] + realItem.LvlSysLuck[LvlSystem - 1]);
            AddedStats[Stat.HealthRecovery] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.HealthRecovery] + realItem.LvlSysHealthRecovery[LvlSystem - 1]);
            AddedStats[Stat.SpellRecovery] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.SpellRecovery] + realItem.LvlSysSpellRecovery[LvlSystem - 1]);
            AddedStats[Stat.Reflect] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.Reflect] + realItem.LvlSysReflect[LvlSystem - 1]);
            AddedStats[Stat.AttackSpeed] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.AttackSpeed] + realItem.LvlSysAttackSpeed[LvlSystem - 1]);
            AddedStats[Stat.AttackSpeedRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.AttackSpeedRatePercent] + realItem.LvlSysAttackSpeedRatePercent[LvlSystem - 1]);
            AddedStats[Stat.HPRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.HPRatePercent] + realItem.LvlSysHPRatePercent[LvlSystem - 1]);
            AddedStats[Stat.MPRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MPRatePercent] + realItem.LvlSysMPRatePercent[LvlSystem - 1]);
            AddedStats[Stat.MaxACRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxACRatePercent] + realItem.LvlSysMaxACRatePercent[LvlSystem - 1]);
            AddedStats[Stat.MaxMACRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxMACRatePercent] + realItem.LvlSysMaxMACRatePercent[LvlSystem - 1]);
            AddedStats[Stat.MaxDCRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxDCRatePercent] + realItem.LvlSysMaxDCRatePercent[LvlSystem - 1]);
            AddedStats[Stat.MaxMCRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxMCRatePercent] + realItem.LvlSysMaxMCRatePercent[LvlSystem - 1]);
            AddedStats[Stat.MaxSCRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.MaxSCRatePercent] + realItem.LvlSysMaxSCRatePercent[LvlSystem - 1]);
            AddedStats[Stat.GoldDropRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.GoldDropRatePercent] + realItem.LvlSysGoldDropRatePercent[LvlSystem - 1]);
            AddedStats[Stat.ExpRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.ExpRatePercent] + realItem.LvlSysExpRatePercent[LvlSystem - 1]);
            AddedStats[Stat.ItemDropRatePercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.ItemDropRatePercent] + realItem.LvlSysItemDropRatePercent[LvlSystem - 1]);
            AddedStats[Stat.PVEDamage] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PVEDamage] + realItem.LvlSysPVEDamage[LvlSystem - 1]);
            AddedStats[Stat.PVPDamage] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.PVPDamage] + realItem.LvlSysPVPDamage[LvlSystem - 1]);
            AddedStats[Stat.CriticalDamage] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.CriticalDamage] + realItem.LvlSysCriticalDamage[LvlSystem - 1]);
            AddedStats[Stat.CriticalRate] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.CriticalRate] + realItem.LvlSysCriticalRate[LvlSystem - 1]);
            AddedStats[Stat.DamageReductionPercent] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.DamageReductionPercent] + realItem.LvlSysDamageReductionPercent[LvlSystem - 1]);
            AddedStats[Stat.SkillGainMultiplier] = (byte)Math.Min(byte.MaxValue, AddedStats[Stat.SkillGainMultiplier] + realItem.LvlSysSkillGainMultiplier[LvlSystem - 1]);
            */
            #endregion
        }
    }
    public bool ItemGainExp(int exp)
    {
        if (!Info.AllowLvlSys) return false;
        if (LvlSystem == Info.LvlSysExp.Length - 1) return false;
        if (Info.LvlSysExp[LvlSystem] == 0) return false;

        LvlSysExpGained += exp;

        var loop = true;
        var lvlUP = false;
        while (loop)
        {
            if (LvlSystem < Info.LvlSysExp.Length - 1 && Info.LvlSysExp[LvlSystem] != 0 && LvlSysExpGained >= Info.LvlSysExp[LvlSystem])
            {
                LvlSysExpGained -= Info.LvlSysExp[LvlSystem];
                LvlSystem++;
                lvlUP = true;
            }
            else
                loop = false;
        }

        return lvlUP;
=======
>>>>>>> parent of 3b68b8d (Item Seals. Type = Gem, Shape = 8, Durability = Minutes)
    }

    public int GetTotal(Stat type)
    {
        return AddedStats[type] + Info.Stats[type];
    }

    public uint Price()
    {
        if (Info == null) return 0;

        uint p = Info.Price;


        if (Info.Durability > 0)
        {
            float r = ((Info.Price / 2F) / Info.Durability);

            p = (uint)(MaxDura * r);

            if (MaxDura > 0)
                r = CurrentDura / (float)MaxDura;
            else
                r = 0;

            p = (uint)Math.Floor(p / 2F + ((p / 2F) * r) + Info.Price / 2F);
        }


        p = (uint)(p * (AddedStats.Count * 0.1F + 1F));


        return p * Count;
    }
    public uint RepairPrice()
    {
        if (Info == null || Info.Durability == 0)
            return 0;

        var p = Info.Price;

        if (Info.Durability > 0)
        {
            p = (uint)Math.Floor(MaxDura * ((Info.Price / 2F) / Info.Durability) + Info.Price / 2F);
            p = (uint)(p * (AddedStats.Count * 0.1F + 1F));

        }

        var cost = p * Count - Price();

        if (RentalInformation == null)
            return cost;

        return cost * 2;
    }

    public uint Quality()
    {
        uint q = (uint)(AddedStats.Count + Awake.GetAwakeLevel() + 1);

        return q;
    }

    public uint AwakeningPrice()
    {
        if (Info == null) return 0;

        uint p = 1500;

        p = (uint)((p * (1 + Awake.GetAwakeLevel() * 2)) * (uint)Info.Grade);

        return p;
    }

    public uint DisassemblePrice()
    {
        if (Info == null) return 0;

        uint p = 1500 * (uint)Info.Grade;

        p = (uint)(p * ((AddedStats.Count + Awake.GetAwakeLevel()) * 0.1F + 1F));

        return p;
    }

    public uint DowngradePrice()
    {
        if (Info == null) return 0;

        uint p = 3000;

        p = (uint)((p * (1 + (Awake.GetAwakeLevel() + 1) * 2)) * (uint)Info.Grade);

        return p;
    }

    public uint ResetPrice()
    {
        if (Info == null) return 0;

        uint p = 3000 * (uint)Info.Grade;

        p = (uint)(p * (AddedStats.Count * 0.2F + 1F));

        return p;
    }
    public void SetSlotSize(int? size = null)
    {
        if (size == null)
        {
            switch (Info.Type)
            {
                case ItemType.Mount:
                    if (Info.Shape < 7)
                        size = 4;
                    else if (Info.Shape < 12)
                        size = 5;
                    break;
                case ItemType.Weapon:
                    if (Info.Shape == 49 || Info.Shape == 50)
                        size = 5;
                    break;
            }
        }

        if (size == null && Info == null) return;
        if (size != null && size == Slots.Length) return;
        if (size == null && Info != null && Info.Slots == Slots.Length) return;

        Array.Resize(ref Slots, size ?? Info.Slots);
    }

    public ushort Image
    {
        get
        {
            switch (Info.Type)
            {
                #region Amulet Stack Image changes
                case ItemType.Amulet:
                    if (Info.StackSize > 0)
                    {
                        switch (Info.Shape)
                        {
                            case 0: //Amulet
                                if (Count >= 300) return 3662;
                                if (Count >= 200) return 3661;
                                if (Count >= 100) return 3660;
                                return 3660;
                        }
                    }
                    break;
                #endregion
                #region Poison Stack Image changes
                case ItemType.TaoPoison:
                    if (Info.StackSize > 0)
                    {
                        switch (Info.Shape)
                        {
                            case 0: //Grey Poison
                                if (Count >= 150) return 3675;
                                if (Count >= 100) return 2960;
                                if (Count >= 50) return 3674;
                                return 3673;
                            case 1: //Yellow Poison
                                if (Count >= 150) return 3672;
                                if (Count >= 100) return 2961;
                                if (Count >= 50) return 3671;
                                return 3670;
                        }
                    }
                    break;
                    #endregion
            }
            return Info.Image;
        }
    }

    public UserItem Clone()
    {
        UserItem item = new UserItem(Info)
        {
            UniqueID = UniqueID,
            CurrentDura = CurrentDura,
            MaxDura = MaxDura,
            Count = Count,
            GemCount = GemCount,
            DuraChanged = DuraChanged,
            SoulBoundId = SoulBoundId,
            Identified = Identified,
            Cursed = Cursed,
            Slots = Slots,
            AddedStats = new Stats(AddedStats),
            Awake = Awake,

            RefineAdded = RefineAdded,

            ExpireInfo = ExpireInfo,
            RentalInformation = RentalInformation,
<<<<<<< HEAD

            LvlSystem = LvlSystem,
            LvlSysExpGained = LvlSysExpGained,
=======
>>>>>>> parent of 3b68b8d (Item Seals. Type = Gem, Shape = 8, Durability = Minutes)

            IsShopItem = IsShopItem
        };

        return item;
    }

}

public class ExpireInfo
{
    public DateTime ExpiryDate;

    public ExpireInfo() { }

    public ExpireInfo(BinaryReader reader, int version = int.MaxValue, int Customversion = int.MaxValue)
    {
        ExpiryDate = DateTime.FromBinary(reader.ReadInt64());
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(ExpiryDate.ToBinary());
    }
}

public class RentalInformation
{
    public string OwnerName;
    public BindMode BindingFlags = BindMode.None;
    public DateTime ExpiryDate;
    public bool RentalLocked;

    public RentalInformation() { }

    public RentalInformation(BinaryReader reader, int version = int.MaxValue, int CustomVersion = int.MaxValue)
    {
        OwnerName = reader.ReadString();
        BindingFlags = (BindMode)reader.ReadInt16();
        ExpiryDate = DateTime.FromBinary(reader.ReadInt64());
        RentalLocked = reader.ReadBoolean();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(OwnerName);
        writer.Write((short)BindingFlags);
        writer.Write(ExpiryDate.ToBinary());
        writer.Write(RentalLocked);
    }
}

public class GameShopItem
{
    public int ItemIndex;
    public int GIndex;
    public ItemInfo Info;
    public ushort Count = 1;
    public string Class = "";
    public string Category = "";
    public int Stock = 0;
    public bool iStock = false;
    public bool Deal = false;
    public bool TopItem = false;
    public DateTime Date;

    public uint GoldPrice = 0;
    public uint CreditPrice = 0;
    public uint HuntPointsPrice = 0;
    public bool CanBuyGold = false;
    public bool CanBuyCredit = false;
    public bool CanBuyHuntPoints = false;

    public GameShopItem()
    {
    }

    public GameShopItem(BinaryReader reader, int version = int.MaxValue, int Customversion = int.MaxValue)
    {
        ItemIndex = reader.ReadInt32();
        GIndex = reader.ReadInt32();
        GoldPrice = reader.ReadUInt32();
        CreditPrice = reader.ReadUInt32();
        HuntPointsPrice = reader.ReadUInt32();
        if (version <= 84)
        {
            Count = (ushort)reader.ReadUInt32();
        }
        else
        {
            Count = reader.ReadUInt16();
        }
        Class = reader.ReadString();
        Category = reader.ReadString();
        Stock = reader.ReadInt32();
        iStock = reader.ReadBoolean();
        Deal = reader.ReadBoolean();
        TopItem = reader.ReadBoolean();
        Date = DateTime.FromBinary(reader.ReadInt64());
        if (version > 84)
        {
            CanBuyGold = reader.ReadBoolean();
            CanBuyCredit = reader.ReadBoolean();
            CanBuyHuntPoints = reader.ReadBoolean();
        }
    }
    public void UpdateItem(GameShopItem g)
    {
        ItemIndex = g.ItemIndex;
        GIndex = g.GIndex;
        Info = g.Info;
        GoldPrice = g.GoldPrice;
        CreditPrice = g.CreditPrice;
        HuntPointsPrice = g.HuntPointsPrice;
        Count = g.Count;
        Class = g.Class;
        Category = g.Category;
        Stock = g.Stock;
        iStock = g.iStock;
        Deal = g.Deal;
        TopItem = g.TopItem;
        Date = g.Date;
        CanBuyGold = g.CanBuyGold;
        CanBuyCredit = g.CanBuyCredit;
        CanBuyHuntPoints = g.CanBuyHuntPoints;
    }
    public static GameShopItem CloneItem(GameShopItem g)
    {
        var n = new GameShopItem
        {
            ItemIndex = g.ItemIndex,
            GIndex = g.GIndex,
            Info = g.Info,
            GoldPrice = g.GoldPrice,
            CreditPrice = g.CreditPrice,
            HuntPointsPrice = g.HuntPointsPrice,
            Count = g.Count,
            Class = g.Class,
            Category = g.Category,
            Stock = g.Stock,
            iStock = g.iStock,
            Deal = g.Deal,
            TopItem = g.TopItem,
            Date = g.Date,
            CanBuyGold = g.CanBuyGold,
            CanBuyCredit = g.CanBuyCredit,
            CanBuyHuntPoints = g.CanBuyHuntPoints
        };

        return n;
    }

    public GameShopItem(BinaryReader reader, bool packet = false)
    {
        ItemIndex = reader.ReadInt32();
        GIndex = reader.ReadInt32();
        Info = new ItemInfo(reader);
        GoldPrice = reader.ReadUInt32();
        CreditPrice = reader.ReadUInt32();
        HuntPointsPrice = reader.ReadUInt32();
        Count = reader.ReadUInt16();
        Class = reader.ReadString();
        Category = reader.ReadString();
        Stock = reader.ReadInt32();
        iStock = reader.ReadBoolean();
        Deal = reader.ReadBoolean();
        TopItem = reader.ReadBoolean();
        Date = DateTime.FromBinary(reader.ReadInt64());
        CanBuyGold = reader.ReadBoolean();
        CanBuyCredit = reader.ReadBoolean();
        CanBuyHuntPoints = reader.ReadBoolean();
    }

    public void Save(BinaryWriter writer, bool packet = false)
    {
        writer.Write(ItemIndex);
        writer.Write(GIndex);
        if (packet) Info.Save(writer);
        writer.Write(GoldPrice);
        writer.Write(CreditPrice);
        writer.Write(HuntPointsPrice);
        writer.Write(Count);
        writer.Write(Class);
        writer.Write(Category);
        writer.Write(Stock);
        writer.Write(iStock);
        writer.Write(Deal);
        writer.Write(TopItem);
        writer.Write(Date.ToBinary());
        writer.Write(CanBuyGold);
        writer.Write(CanBuyCredit);
        writer.Write(CanBuyHuntPoints);
    }

    public override string ToString()
    {
        return string.Format("{0}: {1}", GIndex, Info.Name);
    }

}

public class Awake
{
    //Awake Option
    public static byte AwakeSuccessRate = 70;
    public static byte AwakeHitRate = 70;
    public static int MaxAwakeLevel = 5;
    public static byte Awake_WeaponRate = 1;
    public static byte Awake_HelmetRate = 1;
    public static byte Awake_ArmorRate = 5;
    public static byte AwakeChanceMin = 1;
    public static float[] AwakeMaterialRate = new float[4] { 1.0F, 1.0F, 1.0F, 1.0F };
    public static byte[] AwakeChanceMax = new byte[4] { 1, 2, 3, 4 };
    public static List<List<byte>[]> AwakeMaterials = new List<List<byte>[]>();

    public AwakeType Type = AwakeType.None;
    readonly List<byte> listAwake = new List<byte>();

    public Awake() { }

    public Awake(BinaryReader reader)
    {
        Type = (AwakeType)reader.ReadByte();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            listAwake.Add(reader.ReadByte());
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)Type);
        writer.Write(listAwake.Count);
        foreach (byte value in listAwake)
        {
            writer.Write(value);
        }
    }
    public bool IsMaxLevel() { return listAwake.Count == Awake.MaxAwakeLevel; }

    public int GetAwakeLevel() { return listAwake.Count; }

    public byte GetAwakeValue()
    {
        byte total = 0;

        foreach (byte value in listAwake)
        {
            total += value;
        }

        return total;
    }

    public bool CheckAwakening(UserItem item, AwakeType type)
    {
        if (item.Info.Bind.HasFlag(BindMode.DontUpgrade))
            return false;

        if (item.Info.CanAwakening != true)
            return false;

        if (item.Info.Grade == ItemGrade.None)
            return false;

        if (IsMaxLevel()) return false;

        if (this.Type == AwakeType.None)
        {
            if (item.Info.Type == ItemType.Weapon)
            {
                if (type == AwakeType.DC ||
                    type == AwakeType.MC ||
                    type == AwakeType.SC)
                {
                    this.Type = type;
                    return true;
                }
                else
                    return false;
            }
            else if (item.Info.Type == ItemType.Helmet)
            {
                if (type == AwakeType.AC ||
                    type == AwakeType.MAC)
                {
                    this.Type = type;
                    return true;
                }
                else
                    return false;
            }
            else if (item.Info.Type == ItemType.Armour)
            {
                if (type == AwakeType.HPMP)
                {
                    this.Type = type;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        else
        {
            if (this.Type == type)
                return true;
            else
                return false;
        }
    }

    public int UpgradeAwake(UserItem item, AwakeType type, out bool[] isHit)
    {
        //return -1 condition error, -1 = dont upgrade, 0 = failed, 1 = Succeed,  
        isHit = null;
        if (CheckAwakening(item, type) != true)
            return -1;

        Random rand = new Random(DateTime.Now.Millisecond);

        if (rand.Next(0, 100) <= AwakeSuccessRate)
        {
            isHit = Awakening(item);
            return 1;
        }
        else
        {
            isHit = MakeHit(1, out _);
            return 0;
        }
    }

    public int RemoveAwake()
    {
        if (listAwake.Count > 0)
        {
            listAwake.Remove(listAwake[listAwake.Count - 1]);

            if (listAwake.Count == 0)
                Type = AwakeType.None;

            return 1;
        }
        else
        {
            Type = AwakeType.None;
            return 0;
        }
    }

    public int GetAwakeLevelValue(int i) { return listAwake[i]; }

    public byte GetDC() { return (Type == AwakeType.DC ? GetAwakeValue() : (byte)0); }
    public byte GetMC() { return (Type == AwakeType.MC ? GetAwakeValue() : (byte)0); }
    public byte GetSC() { return (Type == AwakeType.SC ? GetAwakeValue() : (byte)0); }
    public byte GetAC() { return (Type == AwakeType.AC ? GetAwakeValue() : (byte)0); }
    public byte GetMAC() { return (Type == AwakeType.MAC ? GetAwakeValue() : (byte)0); }
    public byte GetHPMP() { return (Type == AwakeType.HPMP ? GetAwakeValue() : (byte)0); }

    private bool[] MakeHit(int maxValue, out int makeValue)
    {
        float stepValue = (float)maxValue / 5.0f;
        float totalValue = 0.0f;
        bool[] isHit = new bool[5];
        Random rand = new Random(DateTime.Now.Millisecond);

        for (int i = 0; i < 5; i++)
        {
            if (rand.Next(0, 100) < AwakeHitRate)
            {
                totalValue += stepValue;
                isHit[i] = true;
            }
            else
            {
                isHit[i] = false;
            }
        }

        makeValue = totalValue <= 1.0f ? 1 : (int)totalValue;
        return isHit;
    }

    private bool[] Awakening(UserItem item)
    {
        int minValue = AwakeChanceMin;
        int maxValue = (AwakeChanceMax[(int)item.Info.Grade - 1] < minValue) ? minValue : AwakeChanceMax[(int)item.Info.Grade - 1];

        bool[] returnValue = MakeHit(maxValue, out int result);

        switch (item.Info.Type)
        {
            case ItemType.Weapon:
                result *= (int)Awake_WeaponRate;
                break;
            case ItemType.Armour:
                result *= (int)Awake_ArmorRate;
                break;
            case ItemType.Helmet:
                result *= (int)Awake_HelmetRate;
                break;
            default:
                result = 0;
                break;
        }

        listAwake.Add((byte)result);

        return returnValue;
    }
}


public class ItemRentalInformation
{
    public ulong ItemId;
    public string ItemName;
    public string RentingPlayerName;
    public DateTime ItemReturnDate;

    public ItemRentalInformation()
    { }

    public ItemRentalInformation(BinaryReader reader)
    {
        ItemId = reader.ReadUInt64();
        ItemName = reader.ReadString();
        RentingPlayerName = reader.ReadString();
        ItemReturnDate = DateTime.FromBinary(reader.ReadInt64());
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(ItemId);
        writer.Write(ItemName);
        writer.Write(RentingPlayerName);
        writer.Write(ItemReturnDate.ToBinary());
    }
}


public class ItemSets
{
    public ItemSet Set;
    public List<ItemType> Type;
    private byte Amount
    {
        get
        {
            switch (Set)
            {
                case ItemSet.Mundane:
                case ItemSet.NokChi:
                case ItemSet.TaoProtect:
                case ItemSet.Whisker1:
                case ItemSet.Whisker2:
                case ItemSet.Whisker3:
                case ItemSet.Whisker4:
                case ItemSet.Whisker5:
                    return 2;
                case ItemSet.RedOrchid:
                case ItemSet.RedFlower:
                case ItemSet.Smash:
                case ItemSet.HwanDevil:
                case ItemSet.Purity:
                case ItemSet.FiveString:
                case ItemSet.Bone:
                case ItemSet.Bug:
                case ItemSet.DarkGhost:
                    return 3;
                case ItemSet.Recall:
                    return 4;
                case ItemSet.Spirit:
                case ItemSet.WhiteGold:
                case ItemSet.WhiteGoldH:
                case ItemSet.RedJade:
                case ItemSet.RedJadeH:
                case ItemSet.Nephrite:
                case ItemSet.NephriteH:
                case ItemSet.Hyeolryong:
                case ItemSet.Monitor:
                case ItemSet.Oppressive:
                case ItemSet.Paeok:
                case ItemSet.Sulgwan:
                case ItemSet.BlueFrostH:
                case ItemSet.BlueFrost:
                    return 5;
                default:
                    return 0;
            }
        }
    }
    public byte Count;
    public bool SetComplete
    {
        get
        {
            return Count >= Amount;
        }
    }
}

//ATTRIBUTES SYSTEM
public class Attributes
{
    public Dictionary<string, int> List = new Dictionary<string, int>();

    public Attributes()
    {
        LoadAttributeList();
    }

    public Attributes(BinaryReader reader)
    {
        LoadAttributeList();

        List<string> keys = new List<string>(List.Keys);

        foreach (var key in keys)
        {
            List[key] = reader.ReadInt32();
        }
    }

    private void LoadAttributeList()
    {
        List["HP"] = 0;
        List["MP"] = 0;
        List["MinAC"] = 0;
        List["MaxAC"] = 0;
        List["MinMAC"] = 0;
        List["MaxMAC"] = 0;
        List["MinDC"] = 0;
        List["MaxDC"] = 0;
        List["MinMC"] = 0;
        List["MaxMC"] = 0;
        List["MinSC"] = 0;
        List["MaxSC"] = 0;
        //List["Fire"] = 0;
        //List["Earth"] = 0;
        //List["Thunder"] = 0;
        //List["Water"] = 0;
        //List["Light"] = 0;
        //List["Dark"] = 0;
    }

    public void Save(BinaryWriter writer)
    {
        foreach (var key in List.Keys)
        {
            writer.Write(List[key]);
        }
    }

    public bool AttributeExists(string attribute)
    {
        return List.ContainsKey(attribute);
    }

    public void AddValue(string attribute, int add)
    {
        List[attribute] += add;
    }

    public int GetValue(string attribute)
    {
        try
        {
            return List[attribute];
        }
        catch
        {
            return 0;
        }
    }
}


public class RandomItemStat
{
    public byte MaxDuraChance, MaxDuraStatChance, MaxDuraMaxStat;
    public byte MaxAcChance, MaxAcStatChance, MaxAcMaxStat, MaxMacChance, MaxMacStatChance, MaxMacMaxStat, MaxDcChance, MaxDcStatChance, MaxDcMaxStat, MaxMcChance, MaxMcStatChance, MaxMcMaxStat, MaxScChance, MaxScStatChance, MaxScMaxStat;
    public byte AccuracyChance, AccuracyStatChance, AccuracyMaxStat, AgilityChance, AgilityStatChance, AgilityMaxStat, HpChance, HpStatChance, HpMaxStat, MpChance, MpStatChance, MpMaxStat, StrongChance, StrongStatChance, StrongMaxStat;
    public byte MagicResistChance, MagicResistStatChance, MagicResistMaxStat, PoisonResistChance, PoisonResistStatChance, PoisonResistMaxStat;
    public byte HpRecovChance, HpRecovStatChance, HpRecovMaxStat, MpRecovChance, MpRecovStatChance, MpRecovMaxStat, PoisonRecovChance, PoisonRecovStatChance, PoisonRecovMaxStat;
    public byte CriticalRateChance, CriticalRateStatChance, CriticalRateMaxStat, CriticalDamageChance, CriticalDamageStatChance, CriticalDamageMaxStat;
    public byte FreezeChance, FreezeStatChance, FreezeMaxStat, PoisonAttackChance, PoisonAttackStatChance, PoisonAttackMaxStat;
    public byte AttackSpeedChance, AttackSpeedStatChance, AttackSpeedMaxStat, LuckChance, LuckStatChance, LuckMaxStat;
    public byte CurseChance;
    public byte SlotChance, SlotStatChance, SlotMaxStat;

    public RandomItemStat(ItemType Type = ItemType.Book)
    {
        switch (Type)
        {
            case ItemType.Weapon:
                SetWeapon();
                break;
            case ItemType.Armour:
                SetArmour();
                break;
            case ItemType.Helmet:
                SetHelmet();
                break;
            case ItemType.Belt:
            case ItemType.Boots:
                SetBeltBoots();
                break;
            case ItemType.Necklace:
                SetNecklace();
                break;
            case ItemType.Bracelet:
                SetBracelet();
                break;
            case ItemType.Ring:
                SetRing();
                break;
            case ItemType.Mount:
                SetMount();
                break;
        }
    }

    public void SetWeapon()
    {
        MaxDuraChance = 2;
        MaxDuraStatChance = 13;
        MaxDuraMaxStat = 13;

        MaxDcChance = 15;
        MaxDcStatChance = 15;
        MaxDcMaxStat = 13;

        MaxMcChance = 20;
        MaxMcStatChance = 15;
        MaxMcMaxStat = 13;

        MaxScChance = 20;
        MaxScStatChance = 15;
        MaxScMaxStat = 13;

        AttackSpeedChance = 60;
        AttackSpeedStatChance = 30;
        AttackSpeedMaxStat = 3;

        StrongChance = 24;
        StrongStatChance = 20;
        StrongMaxStat = 2;

        AccuracyChance = 30;
        AccuracyStatChance = 20;
        AccuracyMaxStat = 2;
    }
    public void SetArmour()
    {
        MaxDuraChance = 2;
        MaxDuraStatChance = 10;
        MaxDuraMaxStat = 3;

        MaxAcChance = 30;
        MaxAcStatChance = 15;
        MaxAcMaxStat = 7;

        MaxMacChance = 30;
        MaxMacStatChance = 15;
        MaxMacMaxStat = 7;

        MaxDcChance = 40;
        MaxDcStatChance = 20;
        MaxDcMaxStat = 7;

        MaxMcChance = 40;
        MaxMcStatChance = 20;
        MaxMcMaxStat = 7;

        MaxScChance = 40;
        MaxScStatChance = 20;
        MaxScMaxStat = 7;

    }
    public void SetHelmet()
    {
        MaxDuraChance = 2;
        MaxDuraStatChance = 10;
        MaxDuraMaxStat = 3;

        MaxAcChance = 30;
        MaxAcStatChance = 15;
        MaxAcMaxStat = 7;

        MaxMacChance = 30;
        MaxMacStatChance = 15;
        MaxMacMaxStat = 7;

        MaxDcChance = 40;
        MaxDcStatChance = 20;
        MaxDcMaxStat = 7;

        MaxMcChance = 40;
        MaxMcStatChance = 20;
        MaxMcMaxStat = 7;

        MaxScChance = 40;
        MaxScStatChance = 20;
        MaxScMaxStat = 7;
    }
    public void SetBeltBoots()
    {
        MaxDuraChance = 2;
        MaxDuraStatChance = 10;
        MaxDuraMaxStat = 3;

        MaxAcChance = 30;
        MaxAcStatChance = 30;
        MaxAcMaxStat = 3;

        MaxMacChance = 30;
        MaxMacStatChance = 30;
        MaxMacMaxStat = 3;

        MaxDcChance = 30;
        MaxDcStatChance = 30;
        MaxDcMaxStat = 3;

        MaxMcChance = 30;
        MaxMcStatChance = 30;
        MaxMcMaxStat = 3;

        MaxScChance = 30;
        MaxScStatChance = 30;
        MaxScMaxStat = 3;

        AgilityChance = 60;
        AgilityStatChance = 30;
        AgilityMaxStat = 3;
    }
    public void SetNecklace()
    {
        MaxDuraChance = 2;
        MaxDuraStatChance = 10;
        MaxDuraMaxStat = 3;

        MaxDcChance = 15;
        MaxDcStatChance = 30;
        MaxDcMaxStat = 7;

        MaxMcChance = 15;
        MaxMcStatChance = 30;
        MaxMcMaxStat = 7;

        MaxScChance = 15;
        MaxScStatChance = 30;
        MaxScMaxStat = 7;

        AccuracyChance = 60;
        AccuracyStatChance = 30;
        AccuracyMaxStat = 7;

        AgilityChance = 60;
        AgilityStatChance = 30;
        AgilityMaxStat = 7;
    }
    public void SetBracelet()
    {
        MaxDuraChance = 2;
        MaxDuraStatChance = 10;
        MaxDuraMaxStat = 3;

        MaxAcChance = 20;
        MaxAcStatChance = 30;
        MaxAcMaxStat = 6;

        MaxMacChance = 20;
        MaxMacStatChance = 30;
        MaxMacMaxStat = 6;

        MaxDcChance = 30;
        MaxDcStatChance = 30;
        MaxDcMaxStat = 6;

        MaxMcChance = 30;
        MaxMcStatChance = 30;
        MaxMcMaxStat = 6;

        MaxScChance = 30;
        MaxScStatChance = 30;
        MaxScMaxStat = 6;
    }
    public void SetRing()
    {
        MaxDuraChance = 2;
        MaxDuraStatChance = 10;
        MaxDuraMaxStat = 3;

        MaxAcChance = 25;
        MaxAcStatChance = 20;
        MaxAcMaxStat = 6;

        MaxMacChance = 25;
        MaxMacStatChance = 20;
        MaxMacMaxStat = 6;

        MaxDcChance = 15;
        MaxDcStatChance = 30;
        MaxDcMaxStat = 6;

        MaxMcChance = 15;
        MaxMcStatChance = 30;
        MaxMcMaxStat = 6;

        MaxScChance = 15;
        MaxScStatChance = 30;
        MaxScMaxStat = 6;
    }

    public void SetMount()
    {
        SetRing();
    }
}

public class GTMap
{
    public int index;
    public string Name;
    public string Owner;
    public string Leader;
    public int price;
    public int days;

    public GTMap()
    {

    }

    public GTMap(BinaryReader reader)
    {
        index = reader.ReadInt32();
        Name = reader.ReadString();
        Owner = reader.ReadString();
        Leader = reader.ReadString();
        price = reader.ReadInt32();
        days = reader.ReadInt32();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(index);
        writer.Write(Name);
        writer.Write(Owner);
        writer.Write(Leader);
        writer.Write(price);
        writer.Write(days);
    }

}

public class ChatItem
{
    public ulong UniqueID;
    public string Title;
    public MirGridType Grid;

    public string RegexInternalName
    {
        get { return $"<{Title.Replace("(", "\\(").Replace(")", "\\)")}>"; }
    }

    public string InternalName
    {
        get { return $"<{Title}/{UniqueID}>"; }
    }

    public ChatItem() { }

    public ChatItem(BinaryReader reader)
    {
        UniqueID = reader.ReadUInt64();
        Title = reader.ReadString();
        Grid = (MirGridType)reader.ReadByte();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(UniqueID);
        writer.Write(Title);
        writer.Write((byte)Grid);
    }
}
