using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

public sealed class Stats
{
    private SortedDictionary<Stat, int> Values { get; set; } = new SortedDictionary<Stat, int>();
    public int Count => Values.Sum(pair => Math.Abs(pair.Value));

    public int this[Stat stat]
    {
        get
        {
            return !Values.TryGetValue(stat, out int result) ? 0 : result;
        }
        set
        {
            if (value == 0)
            {
                if (Values.ContainsKey(stat))
                {
                    Values.Remove(stat);
                }

                return;
            }

            Values[stat] = value;
        }
    }

    public Stats() { }

    public Stats(Stats stats)
    {
        foreach (KeyValuePair<Stat, int> pair in stats.Values)
            this[pair.Key] += pair.Value;
    }

    public Stats(BinaryReader reader)
    {
        int count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            Values[(Stat)reader.ReadByte()] = reader.ReadInt32();
    }

    public void Add(Stats stats)
    {
        foreach (KeyValuePair<Stat, int> pair in stats.Values)
            this[pair.Key] += pair.Value;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Values.Count);

        foreach (KeyValuePair<Stat, int> pair in Values)
        {
            writer.Write((byte)pair.Key);
            writer.Write(pair.Value);
        }
    }

    public StatType GetType(Stat stat)
    {
        Type type = stat.GetType();

        MemberInfo[] infos = type.GetMember(stat.ToString());

        StatDescription description = infos[0].GetCustomAttribute<StatDescription>();

        if (description == null) return StatType.None;

        return description.Type;
    }

    public void Clear()
    {
        Values.Clear();
    }
}


public enum Stat : byte
{
    MinAC = 0,
    MaxAC = 1,
    MinMAC = 2,
    MaxMAC = 3,
    MinDC = 4,
    MaxDC = 5,
    MinMC = 6,
    MaxMC = 7,
    MinSC = 8,
    MaxSC = 9,

    Accuracy = 20,
    Agility = 21,

    HP = 30,
    MP = 31,

    AttackSpeed = 40,
    Luck = 41,

    BagWeight = 50,
    HandWeight = 51,
    WearWeight = 52,

    Reflect = 60,
    Strong = 61,

    MagicResist = 70,
    PoisonResist = 71,
    HealthRecovery = 72,
    SpellRecovery = 73,
    PoisonRecovery = 74,
    HPRate = 75,
    MPRate = 76,
    CriticalRate = 77,
    CriticalDamage = 78,
    MaxACRate = 79,
    MaxMACRate = 80,

    Holy = 100,
    Freezing = 101,
    PoisonAttack = 102,
    HPDrainRate = 103,

    ExpRatePercent = 110,
    ItemDropRatePercent = 111,
    GoldDropRatePercent = 112,
    SkillRatePercent = 113,
    MineRatePercent = 114,
    GemRatePercent = 115,
    FishRatePercent = 116,
    CraftRatePercent = 117,
    //AttackBonus

    Unknown = 255
}

public enum StatType : byte
{
    Stack,
    Set,
    None
}

public class StatDescription : Attribute
{
    public string Title { get; set; }
    public StatType Type { get; set; }
}