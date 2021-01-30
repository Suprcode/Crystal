using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Values[(Stat)reader.ReadInt32()] = reader.ReadInt32();
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
            writer.Write((int)pair.Key);
            writer.Write(pair.Value);
        }
    }

    public void Clear()
    {
        Values.Clear();
    }
}


public enum Stat : byte
{
    MinAC,
    MaxAC,
    MinMAC,
    MaxMAC,
    MinDC,
    MaxDC,
    MinMC,
    MaxMC,
    MinSC,
    MaxSC,

    Accuracy,
    Agility,

    HP,
    MP,

    AttackSpeed,
    Luck,

    BagWeight,
    HandWeight,
    WearWeight,

    Reflect,
    Strong,

    MagicResist,
    PoisonResist,
    HealthRecovery,
    SpellRecovery,
    PoisonRecovery,
    HPrate,
    MPrate,
    CriticalRate,
    CriticalDamage,
    MaxACRate,
    MaxMACRate,

    Holy,
    Freezing,
    PoisonAttack,
    HpDrainRate,

    None
}