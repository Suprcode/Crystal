public static class Globals
{
    public const string ProductCodename = "Crystal";

    public const int

        MinAccountIDLength = 3,
        MaxAccountIDLength = 15,

        MinPasswordLength = 5,
        MaxPasswordLength = 15,

        MinCharacterNameLength = 3,
        MaxCharacterNameLength = 15,
        MaxCharacterCount = 4,

        MaxChatLength = 80,

        MaxGroup = 15,

        MaxPets = 5,

        MaxAttackRange = 9,

        MaxDragonLevel = 13,

        ClassWeaponCount = 100,

        FlagIndexCount = 1999,

        MaxConcurrentQuests = 20,

        LogDelay = 10000,

        DataRange = 16;//Was 24

    public static float Commission = 0.05F;

    public const uint SearchDelay = 500,
                      ConsignmentLength = 7,
                      ConsignmentCost = 5000,
                      MinConsignment = 5000,
                      MaxConsignment = 50000000,
                      AuctionCost = 5000,
                      MinStartingBid = 0,
                      MaxStartingBid = 50000;

    public static int[] FishingRodShapes = new int[] { 49, 50 };

    public static Spell[] RangedSpells = new Spell[]
    {
        Spell.FireBall,
        Spell.ThunderBolt,
        Spell.FireBang,
        Spell.FireWall,
        Spell.FrostCrunch,
        Spell.Vampirism,
        Spell.FlameDisruptor,
        Spell.IceStorm,
        Spell.MeteorStrike,
        Spell.Blizzard,
        Spell.SoulFireBall
    };
}