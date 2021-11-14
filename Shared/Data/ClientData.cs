using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class ClientMagic
{
    public string Name;
    public Spell Spell;
    public byte BaseCost, LevelCost, Icon;
    public ushort Level1, Level2, Level3, Level4, Level5, Level6, Level7, Level8, Level9, Level10;
    public ushort Need1, Need2, Need3, Need4, Need5, Need6, Need7, Need8, Need9, Need10;

    public byte Level, Key, Range;
    public ushort Experience;

    public bool IsTempSpell;
    public long CastTime, Delay;

    public ushort PowerBase, PowerBonus;
    public ushort MPowerBase, MPowerBonus;
    public float MultiplierBase = 1.0f, MultiplierBonus;

    public ushort PvPPowerBase, PvPPowerBonus;
    public ushort PvPMPowerBase, PvPMPowerBonus;
    public float PvPMultiplierBase = 1.0f, PvPMultiplierBonus;

    public ClientMagic()
    {
    }

    public ClientMagic(BinaryReader reader)
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

        Level = reader.ReadByte();
        Key = reader.ReadByte();
        Experience = reader.ReadUInt16();

        Delay = reader.ReadInt64();

        Range = reader.ReadByte();
        CastTime = reader.ReadInt64();

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

        writer.Write(Level);
        writer.Write(Key);
        writer.Write(Experience);

        writer.Write(Delay);

        writer.Write(Range);
        writer.Write(CastTime);

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

public class ClientRecipeInfo
{
    public uint Gold;
    public byte Chance;
    public UserItem Item;
    public List<UserItem> Tools = new List<UserItem>();
    public List<UserItem> Ingredients = new List<UserItem>();

    public ClientRecipeInfo() { }


    public ClientRecipeInfo(BinaryReader reader)
    {
        Gold = reader.ReadUInt32();
        Chance = reader.ReadByte();

        Item = new UserItem(reader);

        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            Tools.Add(new UserItem(reader));
        }

        count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            Ingredients.Add(new UserItem(reader));
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Gold);
        writer.Write(Chance);
        Item.Save(writer);

        writer.Write(Tools.Count);
        foreach (var tool in Tools)
        {
            tool.Save(writer);
        }

        writer.Write(Ingredients.Count);
        foreach (var ingredient in Ingredients)
        {
            ingredient.Save(writer);
        }
    }
}

public class ClientFriend
{
    public int Index;
    public string Name;
    public string Memo = "";
    public bool Blocked;

    public bool Online;

    public ClientFriend() { }

    public ClientFriend(BinaryReader reader)
    {
        Index = reader.ReadInt32();
        Name = reader.ReadString();
        Memo = reader.ReadString();
        Blocked = reader.ReadBoolean();

        Online = reader.ReadBoolean();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        writer.Write(Name);
        writer.Write(Memo);
        writer.Write(Blocked);

        writer.Write(Online);
    }
}

public class ClientMail
{
    public ulong MailID;
    public string SenderName;
    public string Message;
    public bool Opened, Locked, CanReply, Collected;

    public DateTime DateSent;

    public uint Gold;
    public uint Credit;
    public List<UserItem> Items = new List<UserItem>();

    public ClientMail() { }

    public ClientMail(BinaryReader reader)
    {
        MailID = reader.ReadUInt64();
        SenderName = reader.ReadString();
        Message = reader.ReadString();
        Opened = reader.ReadBoolean();
        Locked = reader.ReadBoolean();
        CanReply = reader.ReadBoolean();
        Collected = reader.ReadBoolean();

        DateSent = DateTime.FromBinary(reader.ReadInt64());

        Credit = reader.ReadUInt32();
        Gold = reader.ReadUInt32();
        int count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            Items.Add(new UserItem(reader));
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(MailID);
        writer.Write(SenderName);
        writer.Write(Message);
        writer.Write(Opened);
        writer.Write(Locked);
        writer.Write(CanReply);
        writer.Write(Collected);

        writer.Write(DateSent.ToBinary());

        writer.Write(Credit);
        writer.Write(Gold);
        writer.Write(Items.Count);

        for (int i = 0; i < Items.Count; i++)
            Items[i].Save(writer);
    }
}

public class ClientAuction
{
    public ulong AuctionID;
    public UserItem Item;
    public string Seller = string.Empty;
    public uint Price;
    public DateTime ConsignmentDate = DateTime.MinValue;
    public MarketItemType ItemType;

    public ClientAuction() { }

    public ClientAuction(BinaryReader reader)
    {
        AuctionID = reader.ReadUInt64();
        Item = new UserItem(reader);
        Seller = reader.ReadString();
        Price = reader.ReadUInt32();
        ConsignmentDate = DateTime.FromBinary(reader.ReadInt64());
        ItemType = (MarketItemType)reader.ReadByte();
    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(AuctionID);
        Item.Save(writer);
        writer.Write(Seller);
        writer.Write(Price);
        writer.Write(ConsignmentDate.ToBinary());
        writer.Write((byte)ItemType);
    }
}

public class ClientQuestInfo
{
    public int Index;

    public uint NPCIndex;

    public string Name, Group;
    public List<string> Description = new List<string>();
    public List<string> TaskDescription = new List<string>();
    public List<string> CompletionDescription = new List<string>();

    public int MinLevelNeeded, MaxLevelNeeded;
    public int QuestNeeded;
    public RequiredClass ClassNeeded;

    public QuestType Type;

    public int TimeLimitInSeconds = 0;

    public bool percentageExp;
    public bool autoComplete;
    public uint RewardGold;
    public uint RewardExp;
    public uint RewardCredit;
    public uint RewardHuntPoints;
    public List<QuestItemReward> RewardsFixedItem = new List<QuestItemReward>();
    public List<QuestItemReward> RewardsSelectItem = new List<QuestItemReward>();

    public uint FinishNPCIndex;

    public bool SameFinishNPC
    {
        get { return NPCIndex == FinishNPCIndex; }
    }

    public ClientQuestInfo() { }

    public ClientQuestInfo(BinaryReader reader)
    {
        Index = reader.ReadInt32();
        NPCIndex = reader.ReadUInt32();
        Name = reader.ReadString();
        Group = reader.ReadString();

        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
            Description.Add(reader.ReadString());

        count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
            TaskDescription.Add(reader.ReadString());

        count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
            CompletionDescription.Add(reader.ReadString());

        MinLevelNeeded = reader.ReadInt32();
        MaxLevelNeeded = reader.ReadInt32();
        QuestNeeded = reader.ReadInt32();
        ClassNeeded = (RequiredClass)reader.ReadByte();
        Type = (QuestType)reader.ReadByte();
        TimeLimitInSeconds = reader.ReadInt32();
        percentageExp = reader.ReadBoolean();
        autoComplete = reader.ReadBoolean();
        RewardGold = reader.ReadUInt32();
        RewardExp = reader.ReadUInt32();
        RewardCredit = reader.ReadUInt32();
        RewardHuntPoints = reader.ReadUInt32();

        count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            RewardsFixedItem.Add(new QuestItemReward(reader));

        count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            RewardsSelectItem.Add(new QuestItemReward(reader));

        FinishNPCIndex = reader.ReadUInt32();
    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        writer.Write(NPCIndex);
        writer.Write(Name);
        writer.Write(Group);

        writer.Write(Description.Count);
        for (int i = 0; i < Description.Count; i++)
            writer.Write(Description[i]);

        writer.Write(TaskDescription.Count);
        for (int i = 0; i < TaskDescription.Count; i++)
            writer.Write(TaskDescription[i]);

        writer.Write(CompletionDescription.Count);
        for (int i = 0; i < CompletionDescription.Count; i++)
            writer.Write(CompletionDescription[i]);

        writer.Write(MinLevelNeeded);
        writer.Write(MaxLevelNeeded);
        writer.Write(QuestNeeded);
        writer.Write((byte)ClassNeeded);
        writer.Write((byte)Type);
        writer.Write(TimeLimitInSeconds);

        writer.Write(percentageExp);
        writer.Write(autoComplete);
        writer.Write(RewardGold);
        writer.Write(RewardExp);
        writer.Write(RewardCredit);
        writer.Write(RewardHuntPoints);

        writer.Write(RewardsFixedItem.Count);

        for (int i = 0; i < RewardsFixedItem.Count; i++)
            RewardsFixedItem[i].Save(writer);

        writer.Write(RewardsSelectItem.Count);

        for (int i = 0; i < RewardsSelectItem.Count; i++)
            RewardsSelectItem[i].Save(writer);

        writer.Write(FinishNPCIndex);
    }

    public QuestIcon GetQuestIcon(bool taken = false, bool completed = false)
    {
        QuestIcon icon = QuestIcon.None;

        switch (Type)
        {
            case QuestType.General:
            case QuestType.Repeatable:
                if (completed)
                    icon = QuestIcon.QuestionYellow;
                else if (taken)
                    icon = QuestIcon.QuestionWhite;
                else
                    icon = QuestIcon.ExclamationYellow;
                break;
            case QuestType.Daily:
                if (completed)
                    icon = QuestIcon.QuestionBlue;
                else if (taken)
                    icon = QuestIcon.QuestionWhite;
                else
                    icon = QuestIcon.ExclamationBlue;
                break;
            case QuestType.Weekly:
                if (completed)
                    icon = QuestIcon.QuestionBlue;
                else if (taken)
                    icon = QuestIcon.QuestionWhite;
                else
                    icon = QuestIcon.ExclamationBlue;
                break;
            case QuestType.Story:
                if (completed)
                    icon = QuestIcon.QuestionGreen;
                else if (taken)
                    icon = QuestIcon.QuestionWhite;
                else
                    icon = QuestIcon.ExclamationGreen;
                break;
        }

        return icon;
    }
}

public class ClientQuestProgress
{
    public int Id;

    public ClientQuestInfo QuestInfo;

    public List<string> TaskList = new List<string>();

    public bool Taken;
    public bool Completed;
    public bool New;

    public QuestIcon Icon
    {
        get
        {
            return QuestInfo.GetQuestIcon(Taken, Completed);
        }
    }

    public ClientQuestProgress() { }

    public ClientQuestProgress(BinaryReader reader)
    {
        Id = reader.ReadInt32();

        int count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            TaskList.Add(reader.ReadString());

        Taken = reader.ReadBoolean();
        Completed = reader.ReadBoolean();
        New = reader.ReadBoolean();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Id);

        writer.Write(TaskList.Count);

        for (int i = 0; i < TaskList.Count; i++)
            writer.Write(TaskList[i]);

        writer.Write(Taken);
        writer.Write(Completed);
        writer.Write(New);
    }
}

public class ClientBuff
{
    public BuffType Type;
    public string Caster;
    public bool Visible;
    public uint ObjectID;
    public long ExpireTime;
    public bool Infinite;
    public Stats Stats;
    public bool Paused;

    public int[] Values;

    public ClientBuff()
    {
        Stats = new Stats();
    }

    public ClientBuff(BinaryReader reader)
    {
        Caster = null;

        Type = (BuffType)reader.ReadUInt16();
        Visible = reader.ReadBoolean();
        ObjectID = reader.ReadUInt32();
        ExpireTime = reader.ReadInt64();
        Infinite = reader.ReadBoolean();
        Paused = reader.ReadBoolean();

        Stats = new Stats(reader);

        int count = reader.ReadInt32();

        Values = new int[count];

        for (int i = 0; i < count; i++)
        {
            Values[i] = reader.ReadInt32();
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((ushort)Type);
        writer.Write(Visible);
        writer.Write(ObjectID);
        writer.Write(ExpireTime);
        writer.Write(Infinite);
        writer.Write(Paused);

        Stats.Save(writer);

        writer.Write(Values.Length);
        for (int i = 0; i < Values.Length; i++)
        {
            writer.Write(Values[i]);
        }
    }
}