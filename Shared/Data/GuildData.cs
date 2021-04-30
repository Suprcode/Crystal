using System;
using System.Collections.Generic;
using System.IO;

public class GuildRank
{
    public List<GuildMember> Members = new List<GuildMember>();
    public string Name = "";
    public int Index = 0;
    public GuildRankOptions Options = (GuildRankOptions)0;
    public GuildRank() { }

    public GuildRank(BinaryReader reader, bool Offline = false)
    {
        Name = reader.ReadString();
        Options = (GuildRankOptions)reader.ReadByte();
        if (!Offline)
            Index = reader.ReadInt32();
        int Membercount = reader.ReadInt32();
        for (int j = 0; j < Membercount; j++)
            Members.Add(new GuildMember(reader, Offline));
    }
    public void Save(BinaryWriter writer, bool Save = false)
    {
        writer.Write(Name);
        writer.Write((byte)Options);
        if (!Save)
            writer.Write(Index);
        writer.Write(Members.Count);
        for (int j = 0; j < Members.Count; j++)
            Members[j].Save(writer);
    }
}

public class GuildStorageItem
{
    public UserItem Item;
    public long UserId = 0;
    public GuildStorageItem() { }

    public GuildStorageItem(BinaryReader reader)
    {
        Item = new UserItem(reader);
        UserId = reader.ReadInt64();
    }
    public void Save(BinaryWriter writer)
    {
        Item.Save(writer);
        writer.Write(UserId);
    }
}

public class GuildMember
{
    public string name = "";
    public int Id;
    public object Player;
    public DateTime LastLogin;
    public bool hasvoted;
    public bool Online;

    public GuildMember() { }

    public GuildMember(BinaryReader reader, bool offline = false)
    {
        name = reader.ReadString();
        Id = reader.ReadInt32();
        LastLogin = DateTime.FromBinary(reader.ReadInt64());
        hasvoted = reader.ReadBoolean();
        Online = reader.ReadBoolean();
        Online = offline ? false : Online;
    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(name);
        writer.Write(Id);
        writer.Write(LastLogin.ToBinary());
        writer.Write(hasvoted);
        writer.Write(Online);
    }
}

public class GuildBuffInfo
{
    public int Id;
    public int Icon = 0;
    public string name = "";
    public byte LevelRequirement;
    public byte PointsRequirement = 1;
    public int TimeLimit;
    public int ActivationCost;
    public byte BuffAc;
    public byte BuffMac;
    public byte BuffDc;
    public byte BuffMc;
    public byte BuffSc;
    public byte BuffAttack;
    public int BuffMaxHp;
    public int BuffMaxMp;
    public byte BuffMineRate;
    public byte BuffGemRate;
    public byte BuffFishRate;
    public byte BuffExpRate;
    public byte BuffCraftRate;
    public byte BuffSkillRate;
    public byte BuffHpRegen;
    public byte BuffMPRegen;

    public byte BuffDropRate;
    public byte BuffGoldRate;

    public GuildBuffInfo() { }

    public GuildBuffInfo(BinaryReader reader)
    {
        Id = reader.ReadInt32();
        Icon = reader.ReadInt32();
        name = reader.ReadString();
        LevelRequirement = reader.ReadByte();
        PointsRequirement = reader.ReadByte();
        TimeLimit = reader.ReadInt32();
        ActivationCost = reader.ReadInt32();
        BuffAc = reader.ReadByte();
        BuffMac = reader.ReadByte();
        BuffDc = reader.ReadByte();
        BuffMc = reader.ReadByte();
        BuffSc = reader.ReadByte();
        BuffMaxHp = reader.ReadInt32();
        BuffMaxMp = reader.ReadInt32();
        BuffMineRate = reader.ReadByte();
        BuffGemRate = reader.ReadByte();
        BuffFishRate = reader.ReadByte();
        BuffExpRate = reader.ReadByte();
        BuffCraftRate = reader.ReadByte();
        BuffSkillRate = reader.ReadByte();
        BuffHpRegen = reader.ReadByte();
        BuffMPRegen = reader.ReadByte();
        BuffAttack = reader.ReadByte();
        BuffDropRate = reader.ReadByte();
        BuffGoldRate = reader.ReadByte();
    }

    public GuildBuffInfo(InIReader reader, int i)
    {
        Id = reader.ReadInt32("Buff-" + i.ToString(), "Id", 0);
        Icon = reader.ReadInt32("Buff-" + i.ToString(), "Icon", 0);
        name = reader.ReadString("Buff-" + i.ToString(), "Name", "");
        LevelRequirement = reader.ReadByte("Buff-" + i.ToString(), "LevelReq", 0);
        PointsRequirement = reader.ReadByte("Buff-" + i.ToString(), "PointsReq", 1);
        TimeLimit = reader.ReadInt32("Buff-" + i.ToString(), "TimeLimit", 0); ;
        ActivationCost = reader.ReadInt32("Buff-" + i.ToString(), "ActivationCost", 0);
        BuffAc = reader.ReadByte("Buff-" + i.ToString(), "BuffAc", 0);
        BuffMac = reader.ReadByte("Buff-" + i.ToString(), "BuffMAC", 0);
        BuffDc = reader.ReadByte("Buff-" + i.ToString(), "BuffDc", 0);
        BuffMc = reader.ReadByte("Buff-" + i.ToString(), "BuffMc", 0);
        BuffSc = reader.ReadByte("Buff-" + i.ToString(), "BuffSc", 0);
        BuffMaxHp = reader.ReadInt32("Buff-" + i.ToString(), "BuffMaxHp", 0);
        BuffMaxMp = reader.ReadInt32("Buff-" + i.ToString(), "BuffMaxMp", 0);
        BuffMineRate = reader.ReadByte("Buff-" + i.ToString(), "BuffMineRate", 0);
        BuffGemRate = reader.ReadByte("Buff-" + i.ToString(), "BuffGemRate", 0);
        BuffFishRate = reader.ReadByte("Buff-" + i.ToString(), "BuffFishRate", 0);
        BuffExpRate = reader.ReadByte("Buff-" + i.ToString(), "BuffExpRate", 0);
        BuffCraftRate = reader.ReadByte("Buff-" + i.ToString(), "BuffCraftRate", 0);
        BuffSkillRate = reader.ReadByte("Buff-" + i.ToString(), "BuffSkillRate", 0);
        BuffHpRegen = reader.ReadByte("Buff-" + i.ToString(), "BuffHpRegen", 0);
        BuffMPRegen = reader.ReadByte("Buff-" + i.ToString(), "BuffMpRegen", 0);
        BuffAttack = reader.ReadByte("Buff-" + i.ToString(), "BuffAttack", 0);
        BuffDropRate = reader.ReadByte("Buff-" + i.ToString(), "BuffDropRate", 0);
        BuffGoldRate = reader.ReadByte("Buff-" + i.ToString(), "BuffGoldRate", 0);
    }

    public void Save(InIReader reader, int i)
    {
        reader.Write("Buff-" + i.ToString(), "Id", Id);
        reader.Write("Buff-" + i.ToString(), "Icon", Icon);
        reader.Write("Buff-" + i.ToString(), "Name", name);
        reader.Write("Buff-" + i.ToString(), "LevelReq", LevelRequirement);
        reader.Write("Buff-" + i.ToString(), "PointsReq", PointsRequirement);
        reader.Write("Buff-" + i.ToString(), "TimeLimit", TimeLimit); ;
        reader.Write("Buff-" + i.ToString(), "ActivationCost", ActivationCost); ;
        reader.Write("Buff-" + i.ToString(), "BuffAc", BuffAc); ;
        reader.Write("Buff-" + i.ToString(), "BuffMAC", BuffMac); ;
        reader.Write("Buff-" + i.ToString(), "BuffDc", BuffDc); ;
        reader.Write("Buff-" + i.ToString(), "BuffMc", BuffMc); ;
        reader.Write("Buff-" + i.ToString(), "BuffSc", BuffSc); ;
        reader.Write("Buff-" + i.ToString(), "BuffMaxHp", BuffMaxHp); ;
        reader.Write("Buff-" + i.ToString(), "BuffMaxMp", BuffMaxMp); ;
        reader.Write("Buff-" + i.ToString(), "BuffMineRate", BuffMineRate); ;
        reader.Write("Buff-" + i.ToString(), "BuffGemRate", BuffGemRate); ;
        reader.Write("Buff-" + i.ToString(), "BuffFishRate", BuffFishRate); ;
        reader.Write("Buff-" + i.ToString(), "BuffExpRate", BuffExpRate); ;
        reader.Write("Buff-" + i.ToString(), "BuffCraftRate", BuffCraftRate); ;
        reader.Write("Buff-" + i.ToString(), "BuffSkillRate", BuffSkillRate); ;
        reader.Write("Buff-" + i.ToString(), "BuffHpRegen", BuffHpRegen); ;
        reader.Write("Buff-" + i.ToString(), "BuffMpRegen", BuffMPRegen); ;
        reader.Write("Buff-" + i.ToString(), "BuffAttack", BuffAttack); ;
        reader.Write("Buff-" + i.ToString(), "BuffDropRate", BuffDropRate); ;
        reader.Write("Buff-" + i.ToString(), "BuffGoldRate", BuffGoldRate); ;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(Icon);
        writer.Write(name);
        writer.Write(LevelRequirement);
        writer.Write(PointsRequirement);
        writer.Write(TimeLimit);
        writer.Write(ActivationCost);
        writer.Write(BuffAc);
        writer.Write(BuffMac);
        writer.Write(BuffDc);
        writer.Write(BuffMc);
        writer.Write(BuffSc);
        writer.Write(BuffMaxHp);
        writer.Write(BuffMaxMp);
        writer.Write(BuffMineRate);
        writer.Write(BuffGemRate);
        writer.Write(BuffFishRate);
        writer.Write(BuffExpRate);
        writer.Write(BuffCraftRate);
        writer.Write(BuffSkillRate);
        writer.Write(BuffHpRegen);
        writer.Write(BuffMPRegen);
        writer.Write(BuffAttack);
        writer.Write(BuffDropRate);
        writer.Write(BuffGoldRate);
    }

    public override string ToString()
    {
        return string.Format("{0}: {1}", Id, name);
    }

    public string ShowStats()
    {
        string text = string.Empty;

        //text = name + "\n";
        if (BuffAc > 0)
        {
            text += string.Format("Increases AC by: 0-{0}.", BuffAc);
            if (text != "") text += "\n";
        }
        if (BuffMac > 0)
        {
            text += string.Format("Increases MAC by: 0-{0}.", BuffMac);
            if (text != "") text += "\n";
        }
        if (BuffDc > 0)
        {
            text += string.Format("Increases DC by: 0-{0}.", BuffDc);
            if (text != "") text += "\n";
        }
        if (BuffMc > 0)
        {
            text += string.Format("Increases MC by: 0-{0}.", BuffMc);
            if (text != "") text += "\n";
        }
        if (BuffSc > 0)
        {
            text += string.Format("Increases SC by: 0-{0}.", BuffSc);
            if (text != "") text += "\n";
        }
        if (BuffMaxHp > 0)
        {
            text += string.Format("Increases Hp by: {0}.", BuffMaxHp);
            if (text != "") text += "\n";
        }
        if (BuffMaxMp > 0)
        {
            text += string.Format("Increases MP by: {0}.", BuffMaxMp);
            if (text != "") text += "\n";
        }
        if (BuffHpRegen > 0)
        {
            text += string.Format("Increases Health regen by: {0}.", BuffHpRegen);
            if (text != "") text += "\n";
        }
        if (BuffMPRegen > 0)
        {
            text += string.Format("Increases Mana regen by: {0}.", BuffMPRegen);
            if (text != "") text += "\n";
        }
        if (BuffMineRate > 0)
        {
            text += string.Format("Increases Mining success by: {0}%.", BuffMineRate * 5);
            if (text != "") text += "\n";
        }
        if (BuffGemRate > 0)
        {
            text += string.Format("Increases Gem success by: {0}%.", BuffGemRate * 5);
            if (text != "") text += "\n";
        }
        if (BuffFishRate > 0)
        {
            text += string.Format("Increases Fishing success by: {0}%.", BuffFishRate * 5);
            if (text != "") text += "\n";
        }
        if (BuffExpRate > 0)
        {
            text += string.Format("Increases Experience by: {0}%.", BuffExpRate);
            if (text != "") text += "\n";
        }
        if (BuffCraftRate > 0)
        {
            text += string.Format("Increases Crafting success by: {0}%.", BuffCraftRate * 5);
            if (text != "") text += "\n";
        }
        if (BuffSkillRate > 0)
        {
            text += string.Format("Increases Skill training by: {0}.", BuffSkillRate);
            if (text != "") text += "\n";
        }
        if (BuffAttack > 0)
        {
            text += string.Format("Increases Damage by: {0}.", BuffAttack);
            if (text != "") text += "\n";
        }
        if (BuffDropRate > 0)
        {
            text += string.Format("Droprate increased by: {0}%.", BuffDropRate);
            if (text != "") text += "\n";
        }
        if (BuffGoldRate > 0)
        {
            text += string.Format("Goldrate increased by: 0-{0}.", BuffGoldRate);
            if (text != "") text += "\n";
        }


        return text;
    }
}

public class GuildBuff
{
    public int Id;
    public GuildBuffInfo Info;
    public bool Active = false;
    public int ActiveTimeRemaining;

    public bool UsingGuildSkillIcon
    {
        get { return Info != null && Info.Icon < 1000; }
    }

    public GuildBuff() { }

    public GuildBuff(BinaryReader reader)
    {
        Id = reader.ReadInt32();
        Active = reader.ReadBoolean();
        ActiveTimeRemaining = reader.ReadInt32();
    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(Active);
        writer.Write(ActiveTimeRemaining);
    }

    public string PrintTimeSpan(double secs)
    {
        TimeSpan t = TimeSpan.FromMinutes(secs);
        string answer;
        if (t.TotalMinutes < 1.0)
        {
            answer = string.Format("{0}s", t.Seconds);
        }
        else if (t.TotalHours < 1.0)
        {
            answer = string.Format("{0}ms", t.Minutes);
        }
        else // more than 1 hour
        {
            answer = string.Format("{0}h {1:D2}m ", (int)t.TotalHours, t.Minutes);
        }

        return answer;
    }

    public string ShowStats()
    {
        if (Info == null) return "";
        return Info.ShowStats();
    }

}

//outdated but cant delete it or old db's wont load
public class GuildBuffOld
{
    public GuildBuffOld() { }

    public GuildBuffOld(BinaryReader reader)
    {
        reader.ReadByte();
        reader.ReadInt64();
    }
}