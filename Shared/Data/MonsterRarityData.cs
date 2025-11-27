using System;
using System.Collections.Generic;
using System.Drawing;

public struct MonsterRarityProfile
{
    public double HpMultiplier;
    public double DefenseMultiplier;
    public double DamageMultiplier;
    public double ExpMultiplier;
    public double GoldMultiplier;
    public int ItemDropBonusPercent;
    public int GoldDropBonusPercent;
    public Color NameColour;
}

public static class MonsterRarityData
{
    private static bool Enabled = true;
    private static readonly Dictionary<MonsterType, MonsterRarityProfile> Profiles = new()
    {
        [MonsterType.Normal] = new MonsterRarityProfile
        {
            HpMultiplier = 1.00,
            DefenseMultiplier = 1.00,
            DamageMultiplier = 1.00,
            ExpMultiplier = 1.00,
            GoldMultiplier = 1.00,
            ItemDropBonusPercent = 0,
            GoldDropBonusPercent = 0,
            NameColour = Color.White
        },
        [MonsterType.Uncommon] = new MonsterRarityProfile
        {
            HpMultiplier = 1.25,
            DefenseMultiplier = 1.15,
            DamageMultiplier = 1.15,
            ExpMultiplier = 1.20,
            GoldMultiplier = 1.25,
            ItemDropBonusPercent = 15,
            GoldDropBonusPercent = 15,
            NameColour = Color.LightGreen
        },
        [MonsterType.Rare] = new MonsterRarityProfile
        {
            HpMultiplier = 1.60,
            DefenseMultiplier = 1.30,
            DamageMultiplier = 1.35,
            ExpMultiplier = 1.60,
            GoldMultiplier = 1.75,
            ItemDropBonusPercent = 35,
            GoldDropBonusPercent = 35,
            NameColour = Color.DeepSkyBlue
        },
        [MonsterType.Elite] = new MonsterRarityProfile
        {
            HpMultiplier = 2.25,
            DefenseMultiplier = 1.55,
            DamageMultiplier = 1.65,
            ExpMultiplier = 2.20,
            GoldMultiplier = 2.50,
            ItemDropBonusPercent = 75,
            GoldDropBonusPercent = 75,
            NameColour = Color.Gold
        }
    };

    private static double EliteChancePercent = 0.1;
    private static double RareChancePercent = 0.75;
    private static double UncommonChancePercent = 3.0;

    public static void Configure(double uncommonChancePercent, double rareChancePercent, double eliteChancePercent)
    {
        UncommonChancePercent = ClampPercent(uncommonChancePercent);
        RareChancePercent = ClampPercent(rareChancePercent);
        EliteChancePercent = ClampPercent(eliteChancePercent);
    }

    public static void ConfigureProfile(MonsterType type, MonsterRarityProfile profile)
    {
        Profiles[type] = profile;
    }

    public static void ConfigureProfiles(IDictionary<MonsterType, MonsterRarityProfile> overrides)
    {
        if (overrides == null) return;

        foreach (var pair in overrides)
            Profiles[pair.Key] = pair.Value;
    }

    public static void SetEnabled(bool enabled)
    {
        Enabled = enabled;
    }

    public static MonsterRarityProfile GetProfile(MonsterType type)
    {
        return Profiles.TryGetValue(type, out var profile) ? profile : Profiles[MonsterType.Normal];
    }

    public static MonsterType Roll(Random random)
    {
        if (!Enabled)
            return MonsterType.Normal;

        if (random == null) random = new Random();

        int roll = random.Next(10000);
        int eliteChanceBp = PercentToBasisPoints(EliteChancePercent);
        int rareChanceBp = PercentToBasisPoints(RareChancePercent);
        int uncommonChanceBp = PercentToBasisPoints(UncommonChancePercent);

        if (roll < eliteChanceBp)
            return MonsterType.Elite;

        if (roll < eliteChanceBp + rareChanceBp)
            return MonsterType.Rare;

        if (roll < eliteChanceBp + rareChanceBp + uncommonChanceBp)
            return MonsterType.Uncommon;

        return MonsterType.Normal;
    }

    private static int PercentToBasisPoints(double percent)
    {
        return (int)Math.Round(ClampPercent(percent) * 100d, MidpointRounding.AwayFromZero);
    }

    private static double ClampPercent(double percent)
    {
        if (percent < 0) return 0;
        if (percent > 100) return 100;
        return percent;
    }
}


