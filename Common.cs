using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using C = ClientPackets;
using S = ServerPackets;
using System.Linq;

public enum BlendMode : sbyte
{
    NONE = -1,
    NORMAL = 0,
    LIGHT = 1,
    LIGHTINV = 2,
    INVNORMAL = 3,
    INVLIGHT = 4,
    INVLIGHTINV = 5,
    INVCOLOR = 6,
    INVBACKGROUND = 7
}

public enum DamageType : byte
{
    Hit = 0,
    Miss = 1,
    Critical = 2,
    Hp = 3,
    Mp = 4,
    MagicalHit = 5,
    GreenPoison = 6,
    Bleeding = 7,
    Burning = 8,
    ElementFire = 200,
    ElementEarth = 201,
    ElementWater = 202,
    ElementAir = 203,
    ElementWind = 204,
    ElementHoly = 205,
    ElementDark = 206,
    ElementPhantom = 207
}

[Flags]
public enum GMOptions : byte
{
    None = 0,
    GameMaster = 0x0001,
    Observer = 0x0002,
    Superman = 0x0004
}

public enum AwakeType : byte
{
    None = 0,
    DC,
    MC,
    SC,
    AC,
    MAC,
    HPMP,
}

[Flags]
public enum LevelEffects : byte
{
    None = 0,
    Mist = 0x0001,
    RedDragon = 0x0002,
    BlueDragon = 0x0004,
    GmHead = 0x0008,
}

public enum OutputMessageType : byte
{
    Normal, 
    Quest,
    Guild,
}

public enum ItemGrade : byte
{
    None = 0,
    Common = 1,
    Rare = 2,
    Legendary = 3,
    Mythical = 4,
    Godly = 5,
    Unique = 6,
    Quest = 7,
}
public enum StatType : byte
{
    AC = 0,
    MAC = 1,
    DC = 2,
    MC = 3,
    SC = 4,
    HP = 5,
    MP = 6,
    HP_Percent = 7,
    MP_Percent = 8,
    HP_Regen = 9,
    MP_Regen = 10,
    ASpeed = 11,
    Luck = 12,
    Strong = 13,
    Accuracy = 14,
    Agility = 15,
    MagicResist = 16,
    PoisonResist = 17,
    PoisonAttack = 18,
    PoisonRegen = 19,
    Freezing = 20,
    Holy = 21,
    Durability = 22,
    Unknown = 23,
    Update = 24,
    CriticalRate = 25,
    CriticalDamage = 26,
    HpDrainRate = 27,
    Reflect = 28,
    Socket = 29
}
public enum RefinedValue : byte
{
    None = 0,
    DC = 1,
    MC = 2,
    SC = 3,
}

public enum QuestType : byte
{
    General = 0,
    Daily = 1,
    Repeatable = 2,
    Story = 3
}

public enum QuestIcon : byte
{
    None = 0,
    QuestionWhite = 1,
    ExclamationYellow = 2,
    QuestionYellow = 3,
    ExclamationBlue = 5,
    QuestionBlue = 6,
    ExclamationGreen = 52,
    QuestionGreen = 53,
    ExclamationRed = 54,
    QuestionRed = 55,
    ExclamationDarkBlue = 56,
    QuestionDarkBlue = 57,
    ExclamationOrange = 58,
    QuestionOrange = 59,
}

public enum QuestState : byte
{
    Add,
    Update,
    Remove
}

public enum DefaultNPCType : byte
{
    Login,
    LevelUp,
    UseItem,
    MapCoord,
    MapEnter,
    Die,
    Trigger,
    CustomCommand,
    OnAcceptQuest,
    OnFinishQuest,
    Daily,
    TalkMonster,
    EventReward
}

public enum IntelligentCreatureType : byte
{
    None = 99,
    BabyPig = 0,
    Chick = 1,
    Kitten = 2,
    BabySkeleton = 3,
    Baekdon = 4,
    Wimaen = 5,
    BlackKitten = 6,
    BabyDragon = 7,
    OlympicFlame = 8,
    BabySnowMan = 9,
    Frog = 10,
    BabyMonkey = 11,
    AngryBird = 12,
    Foxey = 13
}

//1 blank mob files
//7 mob frames not added
//2 blank frame sets (92, 173)
//4 mob frames duplicate of other frame sets

//TODO: add 2 missing frames in to blank frames, remove 2 duplicate frames (leaving no blanks and 2 duplicates)
public enum Monster : ushort
{
    Guard = 0,
    TaoistGuard = 1,
    Guard2 = 2,
    Hen = 3,
    Deer = 4,
    Scarecrow = 5,
    HookingCat = 6,
    RakingCat = 7,
    Yob = 8,
    Oma = 9,
    CannibalPlant = 10,
    ForestYeti = 11,
    SpittingSpider = 12,
    ChestnutTree = 13,
    EbonyTree = 14,
    LargeMushroom = 15,
    CherryTree = 16,
    OmaFighter = 17,
    OmaWarrior = 18,
    CaveBat = 19,
    CaveMaggot = 20,
    Scorpion = 21,
    Skeleton = 22,
    BoneFighter = 23,
    AxeSkeleton = 24,
    BoneWarrior = 25,
    BoneElite = 26,
    Dung = 27,
    Dark = 28,
    WoomaSoldier = 29,
    WoomaFighter = 30,
    WoomaWarrior = 31,
    FlamingWooma = 32,
    WoomaGuardian = 33,
    WoomaTaurus = 34,
    WhimperingBee = 35,
    GiantWorm = 36,
    Centipede = 37,
    BlackMaggot = 38,
    Tongs = 39,
    EvilTongs = 40,
    EvilCentipede = 41,
    BugBat = 42,
    BugBatMaggot = 43,
    WedgeMoth = 44,
    RedBoar = 45,
    BlackBoar = 46,
    SnakeScorpion = 47,
    WhiteBoar = 48,
    EvilSnake = 49,
    BombSpider = 50,
    RootSpider = 51,
    SpiderBat = 52,
    VenomSpider = 53,
    GangSpider = 54,
    GreatSpider = 55,
    LureSpider = 56,
    BigApe = 57,
    EvilApe = 58,
    GrayEvilApe = 59,
    RedEvilApe = 60,
    CrystalSpider = 61,
    RedMoonEvil = 62,
    BigRat = 63,
    ZumaArcher = 64,
    ZumaStatue = 65,
    ZumaGuardian = 66,
    RedThunderZuma = 67,
    ZumaTaurus = 68,
    DigOutZombie = 69,
    ClZombie = 70,
    NdZombie = 71,
    CrawlerZombie = 72,
    ShamanZombie = 73,
    Ghoul = 74,
    KingScorpion = 75,
    KingHog = 76,
    DarkDevil = 77,
    BoneFamiliar = 78,
    Shinsu = 79,
    Shinsu1 = 80,
    SpiderFrog = 81,
    HoroBlaster = 82,
    BlueHoroBlaster = 83,
    KekTal = 84,
    VioletKekTal = 85,
    Khazard = 86,
    RoninGhoul = 87,
    ToxicGhoul = 88,
    BoneCaptain = 89,
    BoneSpearman = 90,
    BoneBlademan = 91,
    BoneArcher = 92,
    BoneLord = 93,
    Minotaur = 94,
    IceMinotaur = 95,
    ElectricMinotaur = 96,
    WindMinotaur = 97,
    FireMinotaur = 98,
    RightGuard = 99,
    LeftGuard = 100,
    MinotaurKing = 101,
    FrostTiger = 102,
    Sheep = 103,
    Wolf = 104,
    ShellNipper = 105,
    Keratoid = 106,
    GiantKeratoid = 107,
    SkyStinger = 108,
    SandWorm = 109,
    VisceralWorm = 110,
    RedSnake = 111,
    TigerSnake = 112,
    Yimoogi = 113,
    GiantWhiteSnake = 114,
    BlueSnake = 115,
    YellowSnake = 116,
    HolyDeva = 117,
    AxeOma = 118,
    SwordOma = 119,
    CrossbowOma = 120,
    WingedOma = 121,
    FlailOma = 122,
    OmaGuard = 123,
    YinDevilNode = 124,
    YangDevilNode = 125,
    OmaKing = 126,
    BlackFoxman = 127,
    RedFoxman = 128,
    WhiteFoxman = 129,
    TrapRock = 130,
    GuardianRock = 131,
    ThunderElement = 132,
    CloudElement = 133,
    GreatFoxSpirit = 134,
    HedgeKekTal = 135,
    BigHedgeKekTal = 136,
    RedFrogSpider = 137,
    BrownFrogSpider = 138,
    ArcherGuard = 139,
    KatanaGuard = 140,
    //BLANK_141 = 141,
    Pig = 142,
    Bull = 143,
    Bush = 144,
    ChristmasTree = 145,
    HighAssassin = 146,
    DarkDustPile = 147,
    DarkBrownWolf = 148,
    Football = 149, 
    GingerBreadman = 150,
    HalloweenScythe = 151,
    GhastlyLeecher = 152,
    CyanoGhast = 153,
    MutatedManworm = 154,
    CrazyManworm = 155,
    MudPile = 156,
    TailedLion = 157,
    Behemoth = 158,
    DarkDevourer = 159,
    PoisonHugger = 160,
    Hugger = 161,
    MutatedHugger = 162,
    DreamDevourer = 163,
    Treasurebox = 164,
    SnowPile = 165,
    Snowman = 166,
    SnowTree = 167,
    GiantEgg = 168,
    RedTurtle = 169,
    GreenTurtle = 170,
    BlueTurtle = 171,
    Catapult = 172, //not added frames //special 3 states in 1 
    SabukWallSection = 173, //not added frames
    NammandWallSection = 174, //not added frames
    SiegeRepairman = 175, //not added frames
    BlueSanta = 176,
    BattleStandard = 177,
    ArcherGuard2 = 178,
    RedYimoogi = 179,
    LionRiderMale = 180, //frames not added
    LionRiderFemale = 181, //frames not added
    Tornado = 182,
    FlameTiger = 183,
    WingedTigerLord = 184,
    TowerTurtle = 185,
    FinialTurtle = 186,
    TurtleKing = 187,
    DarkTurtle = 188,
    LightTurtle = 189, 
    DarkSwordOma = 190,
    DarkAxeOma = 191,
    DarkCrossbowOma = 192,
    DarkWingedOma = 193,
    BoneWhoo = 194,
    DarkSpider = 195,
    ViscusWorm = 196,
    ViscusCrawler = 197,
    CrawlerLave = 198,
    DarkYob = 199,

    FlamingMutant = 200,//FINISH
    StoningStatue = 201,
    FlyingStatue = 202,
    ValeBat = 203,
    Weaver = 204,
    VenomWeaver = 205,//done
    CrackingWeaver = 206,//done
    ArmingWeaver = 207,//done
    CrystalWeaver = 208,//done
    FrozenZumaStatue = 209,//done
    FrozenZumaGuardian = 210,//done
    FrozenRedZuma = 211,//done
    GreaterWeaver = 212,//done
    SpiderWarrior = 213,//done
    SpiderBarbarian = 214,//done
    HellSlasher = 215,//done
    HellPirate = 216,//done
    HellCannibal = 217,//done
    HellKeeper = 218, //done BOSS
    HellBolt = 219, //done
    WitchDoctor = 220,//done
    ManectricHammer = 221,//done
    ManectricClub = 222,//done
    ManectricClaw = 223,//done
    ManectricStaff = 224,//done
    NamelessGhost = 225,//done
    DarkGhost = 226,//done
    ChaosGhost = 227,//done
    ManectricBlest = 228,//done
    ManectricKing = 229,//done
    FrozenDoor = 230,//done
    IcePillar = 231,//done
    FrostYeti = 232,//done
    ManectricSlave = 233,//done
    TrollHammer = 234,//done
    TrollBomber = 235,//done
    TrollStoner = 236,//done
    TrollKing = 237,//done BOSS
    FlameSpear = 238,//done
    FlameMage = 239,//done
    FlameScythe = 240,//done
    FlameAssassin = 241,//done
    FlameQueen = 242, //done
    HellKnight1 = 243,//done
    HellKnight2 = 244,//done
    HellKnight3 = 245,//done
    HellKnight4 = 246,//done
    HellLord = 247,//done BOSS
    WaterGuard = 248,//done
    IceGuard = 249,// done
    ElementGuard = 250,//done
    DemonGuard = 251,//done
    KingGuard = 252,//done
    Snake10 = 253,//done
    Snake11 = 254,//done
    Snake12 = 255,//done
    Snake13 = 256,//done
    Snake14 = 257,//done
    Snake15 = 258,//done
    Snake16 = 259,//done
    Snake17 = 260,//done

    DeathCrawler = 261,
    BurningZombie = 262,
    MudZombie = 263,
    FrozenZombie = 264,
    UndeadWolf = 265,
    Demonwolf = 266,
    WhiteMammoth = 267,
    DarkBeast = 268,
    LightBeast = 269,
    BloodBaboon = 270,
    HardenRhino = 271,
    AncientBringer = 272,
    FightingCat = 273,//done
    FireCat = 274,//done
    CatWidow = 275,//done
    StainHammerCat = 276,//done
    BlackHammerCat = 277,//done
    StrayCat = 278,//done
    CatShaman = 279,//done
    Jar1 = 280,
    Jar2 = 281,
    SeedingsGeneral = 282,
    RestlessJar = 283,
    GeneralJinmYo = 284,
    Bunny = 285,
    Tucson = 286,
    TucsonFighter = 287,
    TucsonMage = 288,//done
    TucsonWarrior = 289,//done
    Armadillo = 290,//done
    ArmadilloElder = 291,//done
    TucsonEgg = 292,
    PlaguedTucson = 293,
    SandSnail = 294,//done
    CannibalTentacles = 295,//done
    TucsonGeneral = 296,//done
    GasToad = 297,
    Mantis = 298,
    SwampWarrior = 299,

    AssassinBird = 300,
    RhinoWarrior = 301,
    RhinoPriest = 302,
    SwampSlime = 303,
    RockGuard = 304,
    MudWarrior = 305,
    SmallPot = 306,
    TreeQueen = 307,
    ShellFighter = 308,
    DarkBaboon = 309,
    TwinHeadBeast = 310,
    OmaCannibal = 311,
    OmaBlest = 312,
    OmaSlasher = 313,
    OmaAssassin = 314,
    OmaMage = 315,
    OmaWitchDoctor = 316,
    LightningBead = 317,
    HealingBead = 318,
    PowerUpBead = 319,
    DarkOmaKing = 320,
    CaveMage = 321,
    Mandrill = 322,
    PlagueCrab = 323,
    CreeperPlant = 324,
    FloatingWraith = 325,
    ArmedPlant = 326,
    AvengerPlant = 327,
    Nadz = 328,
    AvengingSpirit = 329,
    AvengingWarrior = 330,
    AxePlant = 331,
    WoodBox = 332,
    ClawBeast = 333,
    KillerPlant = 334,
    SackWarrior = 335,
    WereTiger = 336,
    KingHydrax = 337,
    Hydrax = 338,
    HornedMage = 339,
    Basiloid = 340,
    HornedArcher = 341,
    ColdArcher = 342,
    HornedWarrior = 343,
    FloatingRock = 344,
    ScalyBeast = 345,
    HornedSorceror = 346,
    BoulderSpirit = 347,
    HornedCommander = 348,
    MoonStone = 349,

    SunStone = 350,
    LightningStone = 351,
    Turtlegrass = 352,
    Mantree = 353,
    Bear = 354,
    Leopard = 355,
    ChieftainArcher = 356,
    ChieftainSword = 357,
    StoningSpider = 358, //Archer Spell mob (not yet coded)
    VampireSpider = 359, //Archer Spell mob
    SpittingToad = 360, //Archer Spell mob
    SnakeTotem = 361, //Archer Spell mob
    CharmedSnake = 362, //Archer Spell mob
    FrozenSoldier = 363,
    FrozenFighter = 364,
    FrozenArcher = 365,
    FrozenKnight = 366,
    FrozenGolem = 367,
    IcePhantom = 368,
    SnowWolf = 369,
    SnowWolfKing = 370,
    WaterDragon = 371,
    BlackTortoise = 372,
    Manticore = 373,
    DragonWarrior = 374,
    DragonArcher = 375,
    Kirin = 376,
    Guard3 = 377,
    ArcherGuard3 = 378,
    Bunny2 = 379,
    FrozenMiner = 380,
    FrozenAxeman = 381,
    FrozenMagician = 382,
    SnowYeti = 383,
    IceCrystalSoldier = 384,
    DarkWraith = 385,
    DarkSpirit = 386,
    CrystalBeast = 387,
    RedOrb = 388,
    BlueOrb = 389,
    YellowOrb = 390,
    GreenOrb = 391,
    WhiteOrb = 392,
    FatalLotus = 393,
    AntCommander = 394,
    CargoBoxwithlogo = 395,
    Doe = 396,
    Reindeer = 397, //frames not added
    AngryReindeer = 398,
    CargoBox = 399,
    
    Ram1 = 400,
    Ram2 = 401,
    Kite = 402,
    SnowFlower = 403,
    SnowMouse = 404,
    SnowSnail = 405,
    SnowFlowerQueen = 409,
    SecretPaper1 = 414,
    SecretPaper2 = 415,
    SecretPaper3 = 416,
    SecretPaper4 = 417,

    FDung = 427,
    FDark = 428,
    FWoomaSoldier = 429,
    FWoomaFighter = 430,
    FWoomaWarrior = 431,
    FFlamingWooma = 432,
    FWoomaGuardian = 433,
    FWoomaTaurus = 434,
    MWoomaTaurus = 435,
    Vulture = 436,

    OrcDagger = 437,
    OrcMiner = 438,
    OrcSpearthrower = 439,
    OrcWarrior = 440,
    OrcWithAnimal = 441,
    OrcRider = 442,
    OrcWizard = 443,
    OrcCook = 444,
    OrcMace = 445,
    OrcCommander = 446,
    OrcMutant = 447,
    OrcGeneral = 448,
    SandDragon = 449,

    BoneFamiliar1 = 450,
    BoneFamiliar2 = 451,
    BoneFamiliar3 = 452,
    SuperShinsu = 453,
    SuperShinsu1 = 454,
    SuperShinsu2 = 455,
    SuperShinsu3 = 456,
    SuperShinsu4 = 457,
    SuperShinsu5 = 458,

    AncCaveBat = 459,
    AncCaveMaggot = 460,
    AncScorpion = 461,
    AncSkeleton = 462,
    AncBoneFighter = 463,
    AncAxeSkeleton = 464,
    AncBoneWarrior = 465,
    AncBoneElite = 466,

    BeastKing = 471,

    GonRyunAch = 480,
    GonRyunSin = 481,
    GonRyunMag = 482,
    GonRyunTao = 483,
    GonRyunHam = 484,
    GonRyunAchie = 485,
    GonRyunWar = 486,
    GonRyunBow = 487,
    GonRyunSword = 488,
    GonRyunSiner = 489,
    GonRyunCaptin = 490,

    HolyDeva1 = 499,
    HolyDragon = 500,
    HolyDragon1 = 501,

    NumaMage = 502,
    NumaAxemen = 503,
    NumaFighter = 504,
    NumaRider = 505,
    NumaClubber = 506,
    NumaBladesmen = 507,
    NumaMacemen = 508,
    FalconLord = 509,
    BearMinotaur = 510,
    Pillar = 511,
    PumpkinHead = 512,
    SquidSnail = 513,
    SpawnerHut = 514,
    RockDemon = 515,
    PoolFlag = 516,
    WalkingDemon = 517,
    AntQueen = 518,
    Seal = 519,
    GateKeeper = 520,
    WareWolf = 521,
    RNumaMage = 522,
    SoilderAnt = 523,
    FlamingDemon = 524,
    SlitherSerpent = 525,
    RNumaAxemen = 526,
    WhalingSpirit = 527,
    PhantonDemong = 528,
    OmaChieftain = 529,
    SKoreanFlag = 530,
    MutatedZombie = 531,
    BattonPumeler = 532,
    RedKnight = 533,
    BlueKnight = 534,
    DarkBark = 535,
    IceAmazonSpearer = 536,
    AncientDemon0 = 537,
    AncientDemon1 = 538,
    GreenLizard = 539,
    SandShark = 540,
    Cactus = 541,
    SandSummoner = 542,
    AmazonClubber = 543,
    WalkingBlueDemon = 544,
    AncientDemon2 = 545,
    AncientDemon3 = 546,
    AncientDemon4 = 547,
    ConflictedDemon = 548,
    Emperor = 549,
    FemaleVillager = 550,
    WhiteMonkey = 551,
    RNumaSlicer = 552,
    RNumaClibber = 553,
    SpectialSpirit = 554,
    AncientRedKnight = 555,
    BatLord = 556,
    AncientKing = 557,
    BrownMonkey = 558,
    WhiteMonkey1 = 559,
    AmazonSpearer = 560,
    AmazonFighter = 561,
    AncientElephant = 562,
    AncientGoldGriffen = 563,
    AncientWhiteGriffen = 564,
    JinCaptain = 565,
    JinRightGuard = 566,
    JinLeftGuard = 567,
    JinLord = 568,
    AncientLord = 569,
    AncientLord0 = 570,
    AncientBoar = 571,
    IceQueen = 572,
    SandDemon = 573,
    RNumaRagingBull = 574,
    AncientNumaMage = 575,
    Bones0 = 576,
    SpikeSlinger = 577,
    BoneArm = 578,
    AxeDemon = 579,
    FlamingCatapult = 580,
    TormentedDemon = 581,
    FatAxeDemon = 582,
    Taganda = 583,
    SandPillar = 584,
    ChestOGoodies = 595,
    Bones1 = 586,
    Dino0 = 587,
    Dino1 = 588,
    MutatedDino = 589,
    FinnedDinp = 590,
    ClawedDino = 591,
    SpikedDino = 592,
    Dino2 = 593,
    DinoLord = 594,
    Dino3 = 595,
    AncientArcher = 596,
    MinotaurLord = 597,
    AncientSprirt0 = 598,
    AncientSpirit1 = 599,
    AncientHorsement = 600,
    AncientLord1 = 601,
    TwoHandedAxement = 602,
    ForstWarewolf = 603,
    FrostWolf = 604,
    DemonHook = 605,
    DemonLord0 = 606,
    FlyingSerpentLord = 607,
    AncientGuardian = 608,
    AncientFighter = 609,
    AncientSoilder = 610,
    AncientShaman = 611,
    AncientScout = 612,
    DemonizedZombie = 613,
    DemonizedMage = 614,
    PortalDoor = 615,
    DemonAxement = 616,


    EvilMir = 900,
    EvilMirBody = 901,
    DragonStatue = 902,
    HellBomb1 = 903,
    HellBomb2 = 904,
    HellBomb3 = 905,

    SabukGate = 950,
    PalaceWallLeft = 951,
    PalaceWall1 = 952,
    PalaceWall2 = 953,
    GiGateSouth = 954,
    GiGateEast = 955,
    GiGateWest = 956,
    SSabukWall1 = 957,
    SSabukWall2 = 958,
    SSabukWall3 = 959,

    BabyPig = 10000,//Permanent
    Chick = 10001,//Special
    Kitten = 10002,//Permanent
    BabySkeleton = 10003,//Special
    Baekdon = 10004,//Special
    Wimaen = 10005,//Event
    BlackKitten = 10006,//unknown
    BabyDragon = 10007,//unknown
    OlympicFlame = 10008,//unknown
    BabySnowMan = 10009,//unknown
    Frog = 10010,//unknown
    BabyMonkey = 10011,//unknown
    AngryBird = 10012,
    Foxey = 10013,
}

public enum MirAction : byte
{
    Standing,
    Walking,
    Running,
    Pushed,
    DashL,
    DashR,
    DashFail,
    Stance,
    Stance2,
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Attack5,
    AttackRange1,
    AttackRange2,
    AttackRange3,
    Special,
    Struck,
    Harvest,
    Spell,
    Die,
    Dead,
    Skeleton,
    Show,
    Hide,
    Stoned,
    Appear,
    Revive,
    SitDown,
    Mine,
    Sneek,
    DashAttack,
    Lunge,

    WalkingBow,
    RunningBow,
    Jump,

    MountStanding,
    MountWalking,
    MountRunning,
    MountStruck,
    MountAttack,

    FishingCast,
    FishingWait,
    FishingReel
}

public enum CellAttribute : byte
{
    Walk = 0,
    HighWall = 1,
    LowWall = 2,
}

public enum LightSetting : byte
{
    Normal = 0,
    Dawn = 1,
    Day = 2,
    Evening = 3,
    Night = 4
}
[Obfuscation(Feature = "renaming", Exclude = true)]
public enum MirGender : byte
{
    Male = 0,
    Female = 1
}

[Obfuscation(Feature = "renaming", Exclude = true)]
public enum MirClass : byte
{
    Warrior = 0,
    Wizard = 1,
    Taoist = 2,
    Assassin = 3,
    Archer = 4,
    NONE
}

public enum HeroState : byte
{
    Unspawned = 0,
    Dead = 1,
    Spawned = 2,
}

public enum MirDirection : byte
{
    Up = 0,
    UpRight = 1,
    Right = 2,
    DownRight = 3,
    Down = 4,
    DownLeft = 5,
    Left = 6,
    UpLeft = 7
}

public enum ObjectType : byte
{
    None= 0,
    Player = 1,
    Item = 2,
    Merchant = 3,
    Spell = 4,
    Monster = 5,
    Deco = 6,
    Creature = 7,
    Hero = 8
}

public enum ChatType : byte
{
    Normal = 0,
    Shout = 1,
    System = 2,
    Hint = 3,
    Announcement = 4,
    Group = 5,
    WhisperIn = 6,
    WhisperOut = 7,
    Guild = 8,
    Trainer = 9,
    LevelUp = 10,
    System2 = 11,
    Relationship = 12,
    Mentor = 13,
    Shout2 = 14,
    Shout3 = 15,
    Event = 16,
}

public enum WearType : byte
{
    All = 0,
    Hero = 1,
    Player = 2,
}

public enum ItemType : byte
{
    Nothing = 0,
    Weapon = 1,
    Armour = 2,
    Helmet = 4,
    Necklace = 5,
    Bracelet = 6,
    Ring = 7,
    Amulet = 8,
    Belt = 9,
    Boots = 10,
    Stone = 11,
    Torch = 12,
    Potion = 13,
    Ore = 14,
    Meat = 15,
    CraftingMaterial = 16,
    Scroll = 17,
    Gem = 18,
    Mount = 19,
    Book = 20,
    Script = 21,
    Reins = 22,
    Bells = 23,
    Saddle = 24,
    Ribbon = 25,
    Mask = 26,
    Food = 27,
    Hook = 28,
    Float = 29,
    Bait = 30,
    Finder = 31,
    Reel = 32,
    Fish = 33,
    Quest = 34,
	Awakening = 35,
    Pets = 36,
    Transform = 37,
    Poison = 38,
    Medals = 39,
    Shield = 40,
    Pads = 41,
    Crystal = 42,
    Trinket = 43,
    RuneStone = 60
}

public enum MirGridType : byte
{
    None = 0,
    Inventory = 1,
    Equipment = 2,
    Trade = 3,
    Storage = 4,
    BuyBack = 5,
    DropPanel = 6,
    Inspect = 7,
    TrustMerchant = 8,
    GuildStorage = 9,
    GuestTrade = 10,
    Mount = 11,
    Fishing = 12,
    QuestInventory = 13,
    AwakenItem = 14,
    Mail = 15,
    Refine = 16,
    HeroInventory = 17,
    HeroEquipment = 18,
    HeroQuestInventory = 19,
    TMPanel = 20,
    Socketing = 21,
    Crafting = 22,
    AddSocket = 23
}

public enum EquipmentSlot : byte
{
    Weapon = 0,
    Armour = 1,
    Helmet = 2,
    Torch = 3,
    Necklace = 4,
    BraceletL = 5,
    BraceletR = 6,
    RingL = 7,
    RingR = 8,
    Amulet = 9,
    Belt = 10,
    Boots = 11,
    Stone = 12,
    Mount = 13,
    Poison = 14,
    Medals = 15,
    Shield = 16,
    Pads = 17,
    Crystal = 18,
    Trinket = 19
}

public enum MountSlot : byte
{
    Reins = 0,
    Bells = 1,
    Saddle = 2,
    Ribbon = 3,
    Mask = 4
}

public enum FishingSlot : byte
{
    Hook = 0,
    Float = 1,
    Bait = 2,
    Finder = 3,
    Reel = 4
}

[Obfuscation(Feature = "renaming", Exclude = true)]
public enum AttackMode : byte
{
    Peace = 0,
    Group = 1,
    Guild = 2,
    EnemyGuild = 3,
    RedBrown = 4,
    All = 5
}

[Obfuscation(Feature = "renaming", Exclude = true)]
public enum PetMode : byte
{
    Both = 0,
    MoveOnly = 1,
    AttackOnly = 2,
    None = 3,
}

[Flags]
[Obfuscation(Feature = "renaming", Exclude = true)]

public enum HeroMode : byte
{
    Offensive = 0,
    Defensive = 1,
    Guard = 2,
    DontMove = 3,
    DontAttack = 4,

}
public enum PoisonType : ushort
{
    None = 0,
    Green = 1,
    Red = 2,
    Slow = 4,
    Frozen = 8,
    Stun = 16,
    Paralysis = 32,
    DelayedExplosion = 64,
    Bleeding = 128,
    LRParalysis = 256,
    Trap = 512,
    Burning = 1024,
}

[Flags]
[Obfuscation(Feature = "renaming", Exclude = true)]

public enum BindMode : short
{
    none = 0,
    DontDeathdrop = 1,//0x0001
    DontDrop = 2,//0x0002
    DontSell = 4,//0x0004
    DontStore = 8,//0x0008
    DontTrade = 16,//0x0010
    DontRepair = 32,//0x0020
    DontUpgrade = 64,//0x0040
    DestroyOnDrop = 128,//0x0080
    BreakOnDeath = 256,//0x0100
    BindOnEquip = 512,//0x0200
    NoSRepair = 1024,//0x0400
    NoWeddingRing = 2048,//0x0800
    UnableToDisassemble = 4096, //0x1600
    NoMail = 8192//0x3200
}

[Flags]
[Obfuscation(Feature = "renaming", Exclude = true)]
public enum SpecialItemMode : short
{
    None = 0,
    Paralize = 0x0001,
    Teleport = 0x0002,
    Clearring = 0x0004,
    Protection = 0x0008,
    Revival = 0x0010,
    Muscle = 0x0020,
    Flame = 0x0040,
    Healing = 0x0080,
    Probe = 0x0100,
    Skill = 0x0200,
    NoDuraLoss = 0x0400,
}

[Flags]
[Obfuscation(Feature = "renaming", Exclude = true)]
public enum RequiredClass : byte
{
    Warrior = 1,
    Wizard = 2,
    Taoist = 4,
    Assassin = 8,
    Archer = 16,
    WarWizTao = Warrior | Wizard | Taoist,
    None = WarWizTao | Assassin | Archer
}
[Flags]
[Obfuscation(Feature = "renaming", Exclude = true)]
public enum RequiredGender : byte
{
    Male = 1,
    Female = 2,
    None = Male | Female
}
[Obfuscation(Feature = "renaming", Exclude = true)]
public enum RequiredType : byte
{
    Level = 0,
    AC = 1,
    MAC = 2,
    DC = 3,
    MC = 4,
    SC = 5,
}

[Obfuscation(Feature = "renaming", Exclude = true)]
public enum ItemSet : byte
{
    None = 0,
    Spirit = 1,
    Recall = 2,
    RedOrchid = 3,
    RedFlower = 4,
    Smash = 5,
    HwanDevil = 6,
    Purity = 7,
    FiveString = 8,
    Mundane = 9,
    NokChi = 10,
    TaoProtect = 11,
    Mir = 12,
    Bone = 13,
    Bug = 14,
    WhiteGold = 15,
    WhiteGoldH = 16,
    RedJade = 17,
    RedJadeH = 18,
    Nephrite = 19,
    NephriteH = 20,
    Whisker1 = 21,
    Whisker2 = 22,
    Whisker3 = 23,
    Whisker4 = 24,
    Whisker5 = 25,
    Hyeolryong = 26,
    Monitor = 27,
    Oppressive = 28,
    Paeok = 29,
    Sulgwan = 30,
    BlueFrost = 31,
}

[Obfuscation(Feature = "renaming", Exclude = true)]
public enum Spell : byte
{
    None = 0,

    //Warrior
    Fencing = 1,
    Slaying = 2,
    Thrusting = 3,
    HalfMoon = 4,
    ShoulderDash = 5,
    TwinDrakeBlade = 6,
    Entrapment = 7,
    FlamingSword = 8,
    LionRoar = 9,
    CrossHalfMoon = 10,
    BladeAvalanche = 11,
    Fury = 12,
    ProtectionField = 13,
    Rage = 14,
    CounterAttack = 15,
    SliceNDice = 16,
    SlashingBurst = 17,
    BladeStorm = 18,
    ImmortalSkin = 19,


    
    //Wizard
    FireBall = 31,
    Repulsion = 32,
    ElectricShock = 33,
    GreatFireBall = 34,
    HellFire = 35,
    ThunderBolt = 36,
    Teleport = 37,
    FireBang = 38,
    FireWall = 39,
    Lightning = 40,
    FrostCrunch = 41,
    ThunderStorm = 42,
    MagicShield = 43,
    TurnUndead = 44,
    Vampirism = 45,
    IceStorm = 46,
    FlameDisruptor = 47,
    Mirroring = 48,
    FlameField = 49,
    Blizzard = 50,
    MagicBooster = 51,
    MeteorStrike = 52,
    IceThrust = 53,
    Blink = 54,


    //Taoist
    Healing = 61,
    SpiritSword = 62,
    Poisoning = 63,
    SoulFireBall = 64,
    SummonSkeleton = 65,
    Hiding = 67,
    MassHiding = 68,
    SoulShield = 69,
    Revelation = 70,
    BlessedArmour = 71,
    EnergyRepulsor = 72,
    TrapHexagon = 73,
    Purification = 74,
    MassHealing = 75,
    Hallucination = 76,
    UltimateEnhancer = 77,
    SummonShinsu = 78,
    Reincarnation = 79,
    SummonHolyDeva = 80,
    Curse = 81,
    PoisonCloud = 82,
    EnergyShield = 83,
    PetEnhancer = 84,
    Plague = 85,

    //Assassin
    FatalSword = 91,
    DoubleSlash = 92,
    Haste = 93,
    FlashDash = 94,
    LightBody = 95,
    HeavenlySword = 96,
    FireBurst = 97,
    Trap = 98,
    PoisonSword = 99,
    MoonLight = 100,
    MPEater = 101,
    SwiftFeet = 102,
    DarkBody = 103,
    Hemorrhage = 104,
    CrescentSlash = 105,
    MoonMist = 106,
    ShadowStep = 107,

    SoulReaper = 108, // Hero
    //Archer
    Focus = 121,
    StraightShot = 122,
    DoubleShot = 123,
    ExplosiveTrap = 124,
    DelayedExplosion = 125,
    Meditation = 126,
    BackStep = 127,
    ElementalShot = 128,
    Concentration = 129,
    Stonetrap = 130,
    ElementalBarrier = 131,
    SummonVampire = 132,
    VampireShot = 133,
    SummonToad = 134,
    PoisonShot = 135,
    CrippleShot = 136,
    SummonSnakes = 137,
    NapalmShot = 138,
    OneWithNature = 139,
    BindingShot = 140,
    MentalState = 141,

    //Custom
    Portal = 152,
    FuryWaves = 153,
    FrozenRains = 154,
    HeadShot = 155,
    LavaKing = 156,
    DragNet = 158,
    YinYangTiger = 159,
    HolyShield = 160,
    BlazingSword = 161,
    SummonHolyDragon = 162,

    //  Recodes from Dual skills
    BrokenSoul = 163,
    DragonTears = 164,
    ThunderRolls = 165,
    StarPower = 166,
    SoulEater = 167,
    GodsHand = 168,
    Judgement = 169,
    SoulStealer = 170,


    StormEscape = 180,
    FastMove = 181,
    DragonFlames = 182,
    ThunderClap = 183,
    ChopChopStar = 184,
    SoulEaterSwamp = 185,
    HandOfGod = 186,
    BrokenSoulCut = 187, //hero
    FireSpray = 188,
    HealingCircle = 189,
    LastJudgement = 190,

    //Map Events
    DigOutZombie = 200,
    Rubble = 201,
    MapLightning = 202,
    MapLava = 203,
    MapQuake1 = 204,
    MapQuake2 = 205,
    CrystalBeastBlizz = 206,
    MobFireWall = 207,
    MobSoulShield = 208,
    MobBlessedArmour = 209,
    MobPoisonCloud = 210,
    MobMeteorStrike = 211,
    MobBlizzard = 212,
    Armadilo = 213,
    SpecialMob = 255,
}

public enum SpellEffect : byte
{
    None,
    FatalSword,
    Teleport,
    Healing,
    RedMoonEvil,
    TwinDrakeBlade,
    MagicShieldUp,
    MagicShieldDown,
    CounterAttackUp,
    CounterAttackDown,
    FireSprayHit,
    GreatFoxSpirit,
    Entrapment,
    Reflect,
    Critical,
    Mine,
    ElementalBarrierUp,
    ElementalBarrierDown,
    DelayedExplosion,
    MPEater,
    Hemorrhage,
    Bleeding,
    AwakeningSuccess,
    AwakeningFail,
    AwakeningMiss,
    AwakeningHit,
    StormEscape,
    TurtleKing,
    Behemoth,
    Stunned,
    IcePillar,
    #region Weapon Effects
    CrystalBeastSpin,
    CrystalBeastExplosion,
    CrystalBeastSplash,
    CrystalBeastBlast,
    GFBHit,
    PsnHit,
    SFBHit,
    Shocking,
    UEnhance,
    TDBEff0,
    TDBEff1,
    Cake,
    CircleEff,
    PurpleBlastEff,
    BubbleEff,
    ExplosionEff,
    RainbowEff,
    GoldenBubble,
    PurpleExplosion,
    GreenExplosion,
    Swirling,
    EyeEff,
    PhoenixEff,
    ElectricExplosion,
    TrapEff,
    CastingBlizzard,
    BlizzardCast,
    CHM,
    #endregion
    MutantWeb,
    MutantWebHold,
    FlyingStatueCast,
    FlyingStatuePurple,
    FlyingStatueGreen,
    StainHammerCatHit,
    BlackHammerCatSmash,
    KingGuardHold,
    KingGuardSmash,
    HumanFireBall,
    HumanThunderBolt,
    HumanRepulse,
    HumanFireWallCast,
    HumanBlizzardCast,
    HumanMeteorCast,
    HumanMagicShield,
    HumanFlamingSword,
    HumanTwinDrakeBlade,
    HumanHalfMoon,
    HumanCrossHalfMoon,
    HumanThrusting,
    HumanRage,
    HumanSoulFireBall,
    HumanPoisoning,
    HumanCurseCast,
    HumanCastBlessArm,
    HumanCastSoulShield,
    HumanCastPoisonCloud,
    DragonFlamesEffect,
    BrokenSoulCutEffect,
    ThunderClapEffect,
    LastJudgementEffect,
    ChopChopStarEffect,
    SoulEaterSwampEffect,
    HandOfGodEffect,
    TucsonGeneralEffect,
    LavaBurning,
    Jar2Effect,
    RestlessJarSpawn,
    RestlessJarSpawnOnPlayer,
    RestlessJarMultipleHit,
    RestlessJarTornado,
    MoonMist,
    FalconShield,
    BearFlameSpin,

}

public class HumansMonsterSettings
{
    public string HumansName = string.Empty;
    public MirClass MobsClass;
    public MirGender MobsGender;
    public short Weapon, Armour;
    public byte Wing, Hair, Light;

    public HumansMonsterSettings() { }
}

public class MonitorToolUsers
{
    public int Index;
    public string UserName;
    public string Password;
    public byte Permissions;
    public MonitorToolUsers() { }
}

public class BoardInfo
{
    public bool Notice;

    public string Text, Name;

    public DateTime Date;

    public BoardInfo()
    {
        Notice = false;
        Text = string.Empty;
        Name = string.Empty;
        Date = DateTime.Now;
    }

    public BoardInfo(BinaryReader reader)
    {
        Notice = reader.ReadBoolean();
        Text = reader.ReadString();
        Name = reader.ReadString();
        Date = DateTime.FromBinary(reader.ReadInt64());
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Notice);
        writer.Write(Text);
        writer.Write(Name);
        writer.Write(Date.ToBinary());
    }
}

public enum BuffType : byte
{
    None = 0,

    //magics
    HealthRecovery,
    ManaRecovery,
    TemporalFlux,
    ShadowStepFlux,
    Hiding,
    Cloak,
    Haste,
    SwiftFeet,
    Fury,
    SoulShield,
    BlessedArmour,
    LightBody,
    UltimateEnhancer,
    UltimateEnhancerQuest,
    ProtectionField,
    Rage,
    Curse,
    MoonLight,
    DarkBody,
    Concentration,
    VampireShot,
    PoisonShot,
    CounterAttack,
    MentalState,
    EnergyShield,
    MagicBooster,
    PetEnhancer,
    ImmortalSkin,
    MagicShield,
    HumUp,
    Global,
    FastMove,

    //special
    GameMaster = 100,
    General,
    Exp,
    ExpQuest,
    Drop,
    DropQuest,
    Gold,
    BagWeight,
    Transform,
    RelationshipEXP,
    Mentee,
    Mentor,
    Guild,
    Prison,
    Rested,

    //stats
    Impact = 200,
    Magic,
    Taoist,
    Storm,
    StormQuest,
    Accuracy,
    AccuracyQuest,
    Agility,
    HealthAid,
    ManaAid,
    Defence,
    MagicDefence,
    WonderDrug,
    Knapsack,
    VIP,
    HeroBuff,
    MoonMist,
    NewbieGuild,
    Group,
    //  Socket Skills
    PinPoint,
    Evasive,
    Enrage,
    IronWall,
    SpeedyMagician,
    //  Hero
    HeroEnergyShield = 230,
    MobDebuff = 231,
    CombinedBuff = 240,

    LoverRecall = 250,
    SwBuff = 251
}

public enum DefenceType : byte
{
    ACAgility,
    AC,
    MACAgility,
    MAC,
    Agility,
    Repulsion,
    None
}

public enum ServerPacketIds : short
{
    Connected,
    ClientVersion,
    Disconnect,
    KeepAlive,
    NewAccount,
    ChangePassword,
    ChangePasswordBanned,
    Login,
    LoginBanned,
    LoginSuccess,
    NewCharacter,
    NewCharacterSuccess,
    DeleteCharacter,
    DeleteCharacterSuccess,
    StartGame,
    StartGameBanned,
    StartGameDelay,
    MapInformation,
    UserInformation,
    HeroInformation,
    HeroStats,
    UserLocation,
    ObjectPlayer,
    ObjectRemove,
    ObjectTurn,
    ObjectWalk,
    ObjectRun,
    Chat,
    ObjectChat,
    NewItemInfo,
    MoveItem,
    EquipItem,
    HeroEquipItem,
    MergeItem,
    RemoveItem,
    HeroRemoveItem,
    RemoveSlotItem,
    TakeBackItem,
    HeroInventoryToInventory,
    StoreItem,
    InventoryToHeroInventory,
    SplitItem,
    SplitItem1,
    DepositRefineItem,
    RetrieveRefineItem,
    RefineCancel,
    RefineItem,
    DepositTradeItem,
    RetrieveTradeItem,
    UseItem,
    HeroUseItem,
    DropItem,
    PlayerUpdate,
    UpdateItemInfo,
    RefreshStats,
    PlayerInspect,
    LogOutSuccess,
    LogOutFailed,
    TimeOfDay,
    ChangeAMode,
    ChangePMode,
    ChangeHMode,
    ObjectItem,
    ObjectGold,
    GainedItem,
    GainedGold,
    LoseGold,
    GainedCredit,
    LoseCredit,
    ObjectMonster,
    ObjectAttack,
    Struck,
    ObjectStruck,
    DamageIndicator,
    DuraChanged,
    HealthChanged,
    HeroHealthChanged,
    HeroStatsChanged,
    DeleteItem,
    Death,
    ObjectDied,
    ColourChanged,
    ObjectColourChanged,
    ObjectGuildNameChanged,
    GainExperience,
    HeroGainExperience,
    LevelChanged,
    HeroLevelChanged,
    ObjectLeveled,
    ObjectHarvest,
    ObjectHarvested,
    ObjectNpc,
    NPCResponse,
    ObjectHide,
    ObjectShow,
    Poisoned,
    ObjectPoisoned,
    MapChanged,
    ObjectTeleportOut,
    ObjectTeleportIn,
    TeleportIn,
    NPCGoods,
    NPCSell,
    NPCRepair,
    NPCSRepair,
    NPCRefine,
    NPCCheckRefine,
    NPCCollectRefine,
    NPCReplaceWedRing,
    NPCStorage,
    SellItem,
    RepairItem,
    ItemRepaired,
    NewMagic,
    RefreshMagic,
    HeroNewMagic,
    RemoveMagic,
    MagicLeveled,
    HeroMagicLeveled,
    Magic,
    MagicDelay,
    MagicCast,
    ObjectMagic,
    ObjectEffect,
    RangeAttack,
    Pushed,
    ObjectPushed,
    ObjectName,
    UserStorage,
    SwitchGroup,
    DeleteGroup,
    DeleteMember,
    GroupInvite,
    AddMember,
    Revived,
    ObjectRevived,
    SpellToggle,
    ObjectHealth,
    MapEffect,
    ObjectRangeAttack,
    AddBuff,
    RemoveBuff,
    ObjectHidden,
    RefreshItem,
    ObjectSpell,
    UserDash,
    ObjectDash,
    UserDashFail,
    ObjectDashFail,
    NPCConsign,
    NPCMarket,
    NPCMarketPage,
    GuildTerritoryPage,
    ConsignItem,
    MarketFail,
    MarketSuccess,
    ObjectSitDown,
    InTrapRock,
    BaseStatsInfo,
    UserName,
    ChatItemStats,
    GuildNoticeChange,
    GuildMemberChange,
    GuildStatus,
    GuildInvite,
    GuildExpGain,
    GuildNameRequest,
    GuildStorageGoldChange,
    GuildStorageItemChange,
    GuildStorageList,
    GuildRequestWar,
    DefaultNPC,
    NPCUpdate,
    NPCImageUpdate,
    MarriageRequest,
    DivorceRequest,
    MentorRequest,
    TradeRequest,
    TradeAccept,
    TradeGold,
    TradeItem,
    TradeConfirm,
    TradeCancel,
    MountUpdate,
    EquipSlotItem,
    FishingUpdate,
    ChangeQuest,
    CompleteQuest,
    ShareQuest,
    NewQuestInfo,
    RefreshQuestInfo,
    GainedQuestItem,
    DeleteQuestItem,
    CancelReincarnation,
    RequestReincarnation,
    UserBackStep,
    ObjectBackStep,
    UserDashAttack,
    ObjectDashAttack,
    UserAttackMove,
    CombineItem,
    ItemUpgraded,
    SetConcentration,
    SetObjectConcentration,
    SetElemental,
    SetObjectElemental,
    RemoveDelayedExplosion,
    ObjectDeco,
    ObjectSneaking,
    ObjectLevelEffects,
    SetBindingShot,
    SendOutputMessage,

    NPCAwakening,
    NPCDisassemble,
    NPCDowngrade,
    NPCReset,
    AwakeningNeedMaterials,
    AwakeningLockedItem,
    Awakening,

    ReceiveMail,
    MailLockedItem,
    MailSendRequest,
    MailSent,
    ParcelCollected,
    MailCost,
	ResizeInventory,
    ResizeHeroInventory,
    ComboHero,
    ComboStance,
    ResizeStorage,
    NewIntelligentCreature,
    UpdateIntelligentCreatureList,
    IntelligentCreatureEnableRename,
    IntelligentCreaturePickup,
    NPCPearlGoods,

    TransformUpdate,
    FriendUpdate,
    SetVIP,
    SetHumUp,
    SetAutoPot,
    SetHero,
    SetHeroSpawned,
    SetHeroLocked,
    LoverUpdate,
    MentorUpdate,
    GuildBuffList,
    NPCRequestInput,
    GameShopInfo,
    HeroStashInfo,
    HeroStashActivate,
    GameShopStock,
    Rankings,
    Opendoor,
    MobHealthChanged,
    Crafting,
    CraftingRecipe,
    UpdatePlayerRecipes,
    UpdateQuestInfo,
    GainShieldExperience,
    CreateHero,
    ItemGainLevel,
    RecipieShopContents,
    PurchaseResult,
    EESocketDisplay,
    EERuneSocketResult,
    EEOpenSocketDialog,
    EERemoveRune,
    EEHeroRank,
    EEOpenGuildBoardDialog,
    OpenRankDialog,
    OpenHeroRankDialog,
    CustomAIEffect,
    LogNotice,
    EEClientCompletedQuests,
    EEClientAvailableQuests,
    ItemInfoList,
    RefreshHeroCap,
    LeavePublicEvent,
    ActivateEvent,
    DeactivateEvent,
    EnterPublicEvent,
    HelmetChange,
    ToggleBuff,
    ExternalToolLogin,
    ToolStats,
    ToolPlayerList,
    BRList,
    LMSDialog,
    BigMapQuestLocations,
}

public class LogNotice
{
    public string Title = string.Empty;
    public string LogString = string.Empty;
    public string Link = string.Empty;
    public LogNotice() { }

    public LogNotice(BinaryReader reader)
    {
        Title = reader.ReadString();
        LogString = reader.ReadString();
        Link = reader.ReadString();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Title);
        writer.Write(LogString);
        writer.Write(Link);
    }
}

public enum ClientPacketIds : short
{
    ClientVersion,
    Disconnect,
    KeepAlive,
    NewAccount,
    ChangePassword,
    Login,
    NewCharacter,
    DeleteCharacter,
    StartGame,
    LogOut,
    Turn,
    Walk,
    Run,
    Chat,
    MoveItem,
    StoreItem,
    InventoryToHeroInventory,
    TakeBackItem,
    HeroInventoryToInventory,
    MergeItem,
    EquipItem,
    HeroEquipItem,
    RemoveItem,
    HeroRemoveItem,
    RemoveSlotItem,
    SplitItem,
    UseItem,
    HeroUseItem,
    DropItem,
    DepositRefineItem,
    RetrieveRefineItem,
    RefineCancel,
    RefineItem,
    CheckRefine,
    ReplaceWedRing,
    DepositTradeItem,
    RetrieveTradeItem,
    DropGold,
    PickUp,
    Inspect,
    HeroRunTo,
    ChangeAMode,
    ChangePMode,
    ChangeHMode,
    ChangeTrade,
    Attack,
    RangeAttack,
    Harvest,
    CallNPC,
    TalkMonsterNPC,
    BuyItem,
    SellItem,
    RepairItem,
    BuyItemBack,
    SRepairItem,
    MagicKey,
    HeroMagicKey,
    AutoPotUpdate,
    Magic,
    SwitchGroup,
    AddMember,
    DellMember,
    GroupInvite,
    TownRevive,
    SpellToggle,
    ConsignItem,
    MarketSearch,
    MarketRefresh,
    MarketPage,
    GuildTerritoryPage,
    MarketBuy,
    MarketGetBack,
    RequestUserName,
    RequestChatItem,
    EditGuildMember,
    EditGuildNotice,
    GuildInvite,
    GuildNameReturn,
    RequestGuildInfo,
    GuildStorageGoldChange,
    GuildStorageItemChange,
    GuildWarReturn,
    MarriageRequest,
    MarriageReply,
    ChangeMarriage,
    DivorceRequest,
    DivorceReply,
    AddMentor,
    MentorReply,
    AllowMentor,
    CancelMentor,
    TradeRequest,
    TradeReply,
    TradeGold,
    TradeConfirm,
    TradeCancel,
    EquipSlotItem,
    FishingCast,
    FishingChangeAutocast,
    AcceptQuest,
    FinishQuest,
    AbandonQuest,
    ShareQuest,

    AcceptReincarnation,
    CancelReincarnation,
    CombineItem,
    StashHero,

    SetConcentration,
    AwakeningNeedMaterials,
    AwakeningLockedItem,
    Awakening,
    DisassembleItem,
    DowngradeAwakening,
    ResetAddedItem,

    SendMail,
    ReadMail,
    CollectParcel,
    DeleteMail,
    LockMail,
    MailLockedItem,
    MailCost,

    UpdateIntelligentCreature,
    IntelligentCreaturePickup,

    AddFriend,
    RemoveFriend,
    RefreshFriends,
    AddMemo,
    GuildBuffUpdate,
    NPCConfirmInput,
    GameshopBuy,

    ReportIssue,
    GetRanking,
    Opendoor,
    RequestCraft,
    GetCrafts,
    CollectCraft,
    CreateHero,
    ComboSpell,
    RecipieShop,
    BuyRecipie,
    EERequestSocketInfo,
    EESocketItem,
    EERemoveRune,
    EEHeroRank,
    HeroUseMagic,
    ChangeHeroMove,
    CheckClientItemFile,
    ExternalToolLogin,
    ToolStats,
    ReportCheat,
    AdjustGuildTax,
    LMS_Signup,
    Get_LMS_BR,
}

public enum ConquestType : byte
{
    Request = 0,
    Auto = 1,
    Forced = 2,
}

public enum ConquestGame : byte
{
    CapturePalace = 0,
    KingOfHill = 1,
    Random = 2,
    Classic = 3,
    ControlPoints = 4
}

public class InIReader
{
    #region Fields
    private readonly List<string> _contents;
    private readonly string _fileName;
    #endregion

    #region Constructor
    public InIReader(string fileName)
    {
        _fileName = fileName;

        _contents = new List<string>();
        try
        {
            if (File.Exists(_fileName))
                _contents.AddRange(File.ReadAllLines(_fileName));
        }
        catch
        {
        }
    }
    #endregion

    #region Functions
    private string FindValue(string section, string key)
    {
        for (int a = 0; a < _contents.Count; a++)
            if (String.CompareOrdinal(_contents[a], "[" + section + "]") == 0)
                for (int b = a + 1; b < _contents.Count; b++)
                    if (String.CompareOrdinal(_contents[b].Split('=')[0], key) == 0)
                        return _contents[b].Split('=')[1];
                    else if (_contents[b].StartsWith("[") && _contents[b].EndsWith("]"))
                        return null;
        return null;
    }

    private int FindIndex(string section, string key)
    {
        for (int a = 0; a < _contents.Count; a++)
            if (String.CompareOrdinal(_contents[a], "[" + section + "]") == 0)
                for (int b = a + 1; b < _contents.Count; b++)
                    if (String.CompareOrdinal(_contents[b].Split('=')[0], key) == 0)
                        return b;
                    else if (_contents[b].StartsWith("[") && _contents[b].EndsWith("]"))
                    {
                        _contents.Insert(b - 1, key + "=");
                        return b - 1;
                    }
                    else if (_contents.Count - 1 == b)
                    {
                        _contents.Add(key + "=");
                        return _contents.Count - 1;
                    }
        if (_contents.Count > 0)
            _contents.Add("");

        _contents.Add("[" + section + "]");
        _contents.Add(key + "=");
        return _contents.Count - 1;
    }

    public void Save()
    {
        try
        {
            File.WriteAllLines(_fileName, _contents);
        }
        catch
        {
        }
    }
    #endregion

    #region Read
    public bool ReadBoolean(string section, string key, bool Default)
    {

        if (!bool.TryParse(FindValue(section, key), out bool result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }

    public byte ReadByte(string section, string key, byte Default)
    {
        
        if (!byte.TryParse(FindValue(section, key), out byte result))
        {
            result = Default;
            Write(section, key, Default);
        }


        return result;
    }

    public sbyte ReadSByte(string section, string key, sbyte Default)
    {
        
        if (!sbyte.TryParse(FindValue(section, key), out sbyte result))
        {
            result = Default;
            Write(section, key, Default);
        }


        return result;
    }

    public ushort ReadUInt16(string section, string key, ushort Default)
    {
        
        if (!ushort.TryParse(FindValue(section, key), out ushort result))
        {
            result = Default;
            Write(section, key, Default);
        }


        return result;
    }

    public short ReadInt16(string section, string key, short Default)
    {


        if (!short.TryParse(FindValue(section, key), out short result))
        {
            result = Default;
            Write(section, key, Default);
        }


        return result;
    }

    public uint ReadUInt32(string section, string key, uint Default)
    {


        if (!uint.TryParse(FindValue(section, key), out uint result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }

    public int ReadInt32(string section, string key, int Default)
    {
        
        if (!int.TryParse(FindValue(section, key), out int result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }

    public ulong ReadUInt64(string section, string key, ulong Default)
    {
        
        if (!ulong.TryParse(FindValue(section, key), out ulong result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }

    public long ReadInt64(string section, string key, long Default)
    {
        
        if (!long.TryParse(FindValue(section, key), out long result))
        {
            result = Default;
            Write(section, key, Default);
        }


        return result;
    }

    public float ReadSingle(string section, string key, float Default)
    {
        
        if (!float.TryParse(FindValue(section, key), out float result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }

    public double ReadDouble(string section, string key, double Default)
    {
        
        if (!double.TryParse(FindValue(section, key), out double result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }

    public decimal ReadDecimal(string section, string key, decimal Default)
    {
        
        if (!decimal.TryParse(FindValue(section, key), out decimal result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }

    public string ReadString(string section, string key, string Default)
    {
        string result = FindValue(section, key);

        if (string.IsNullOrEmpty(result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }

    public char ReadChar(string section, string key, char Default)
    {
        
        if (!char.TryParse(FindValue(section, key), out char result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }

    public Point ReadPoint(string section, string key, Point Default)
    {
        string temp = FindValue(section, key);
        if (temp == null || !int.TryParse(temp.Split(',')[0], out int tempX))
        {
            Write(section, key, Default);
            return Default;
        }
        if (!int.TryParse(temp.Split(',')[1], out int tempY))
        {
            Write(section, key, Default);
            return Default;
        }

        return new Point(tempX, tempY);
    }

    public Size ReadSize(string section, string key, Size Default)
    {
        string temp = FindValue(section, key);
        if (!int.TryParse(temp.Split(',')[0], out int tempX))
        {
            Write(section, key, Default);
            return Default;
        }
        if (!int.TryParse(temp.Split(',')[1], out int tempY))
        {
            Write(section, key, Default);
            return Default;
        }

        return new Size(tempX, tempY);
    }

    public TimeSpan ReadTimeSpan(string section, string key, TimeSpan Default)
    {

        if (!TimeSpan.TryParse(FindValue(section, key), out TimeSpan result))
        {
            result = Default;
            Write(section, key, Default);
        }


        return result;
    }

    public float ReadFloat(string section, string key, float Default)
    {

        if (!float.TryParse(FindValue(section, key), out float result))
        {
            result = Default;
            Write(section, key, Default);
        }

        return result;
    }
    #endregion

    #region Write
    public void Write(string section, string key, bool value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, byte value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, sbyte value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, ushort value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, short value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, uint value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, int value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, ulong value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, long value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, float value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, double value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, decimal value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, string value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, char value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }

    public void Write(string section, string key, Point value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value.X + "," + value.Y;
        Save();
    }

    public void Write(string section, string key, Size value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value.Width + "," + value.Height;
        Save();
    }

    public void Write(string section, string key, TimeSpan value)
    {
        _contents[FindIndex(section, key)] = key + "=" + value;
        Save();
    }
    #endregion
}

public static class Globals
{
    public const int
        MinAccountIDLength = 3,
        MaxAccountIDLength = 15,

        MinPasswordLength = 5,
        MaxPasswordLength = 15,

        MinCharacterNameLength = 3,
        MaxCharacterNameLength = 15,
        MaxCharacterCount = 3,

        MaxChatLength = 116,// it's not this btw

        MaxGroup = 15,
        
        MaxAttackRange = 9,

        MaxDragonLevel = 12,

        FlagIndexCount = 1999,

        MaxConcurrentQuests = 20,

        LogDelay = 10000,

        DataRange = 14;//Was 24

    public static float Commission = 0.05F;

    public const uint SearchDelay = 50,
                      ConsignmentLength = 7,
                      ConsignmentCost = 1000,
                      MinConsignment = 50,
                      MaxConsignment = 50000000;

}

public static class Functions
{
    public static bool CompareBytes(byte[] a, byte[] b)
    {
        if (a == b) return true;

        if (a == null || b == null || a.Length != b.Length) return false;

        for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) return false;

        return true;
    }


    /// <summary>
    /// Get the outter points of a circle
    /// </summary>
    /// <param name="info">The centeral location</param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static List<Point> GetCircleLocations(Point info, int radius)
    {
        List<Point> list = new List<Point>();
        int x, y, r2;
        r2 = radius * radius;
        list.Add(new Point(info.X, info.Y + radius));
        list.Add(new Point(info.X, info.Y - radius));
        list.Add(new Point(info.X + radius, info.Y));
        list.Add(new Point(info.X - radius, info.Y));
        for (x = -radius; x <= radius; x++)
        {
            y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
            while (x < y)
            {
                list.Add(new Point(info.X + x, info.Y + y));
                list.Add(new Point(info.X + x, info.Y - y));
                list.Add(new Point(info.X - x, info.Y + y));
                list.Add(new Point(info.X - x, info.Y - y));

                list.Add(new Point(info.X + y, info.Y + x));
                list.Add(new Point(info.X + y, info.Y - x));
                list.Add(new Point(info.X + y, info.Y + x));
                list.Add(new Point(info.X - y, info.Y + x));
                list.Add(new Point(info.X - y, info.Y - x));
                x += 1;
                y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
            }
            if (x == y)
            {
                list.Add(new Point(info.X + x, info.Y + y));
                list.Add(new Point(info.X + x, info.Y - y));
                list.Add(new Point(info.X - x, info.Y + y));
                list.Add(new Point(info.X - x, info.Y - y));
            }
        }
        return list;
    }

    public static bool TryParse(string s, out Point temp)
    {
        temp = Point.Empty;
        if (String.IsNullOrWhiteSpace(s)) return false;

        string[] data = s.Split(',');
        if (data.Length <= 1) return false;

        if (!Int32.TryParse(data[0], out int tempX))
            return false;

        if (!Int32.TryParse(data[1], out int tempY))
            return false;

        temp = new Point(tempX, tempY);
        return true;
    }
    public static Point Subtract(this Point p1, Point p2)
    {
        return new Point(p1.X - p2.X, p1.Y - p2.Y);
    }
    public static Point Subtract(this Point p1, int x, int y)
    {
        return new Point(p1.X - x, p1.Y - y);
    }
    public static Point Add(this Point p1, Point p2)
    {
        return new Point(p1.X + p2.X, p1.Y + p2.Y);
    }
    public static Point Add(this Point p1, int x, int y)
    {
        return new Point(p1.X + x, p1.Y + y);
    }
    public static string PointToString(Point p)
    {
        return String.Format("{0}, {1}", p.X, p.Y);
    }

    public static bool InRange(Point a, Point b, int i)
    {
        return Math.Abs(a.X - b.X) <= i && Math.Abs(a.Y - b.Y) <= i;
    }
    /// <summary>
    /// Checks if both Objects are facing each other
    /// </summary>
    /// <param name="dirA">Player A's current facing Direction</param>
    /// <param name="pointA">Player A's current Location</param>
    /// <param name="dirB">Player B's current facing Direction</param>
    /// <param name="pointB">Player B's current Location</param>
    /// <returns></returns>
    public static bool FacingEachOther(MirDirection dirA, Point pointA, MirDirection dirB, Point pointB)
    {
        if (dirA == DirectionFromPoint(pointA, pointB) && dirB == DirectionFromPoint(pointB, pointA))
        {
            return true;
        }

        return false;
    }

    public static string PrintTimeSpanFromSeconds(double secs, bool accurate = true)
    {
        TimeSpan t = TimeSpan.FromSeconds(secs);
        string answer;
        if (t.TotalMinutes < 1.0)
        {
            answer = string.Format("{0}s", t.Seconds);
        }
        else if (t.TotalHours < 1.0)
        {
            answer = accurate ? string.Format("{0}m {1:D2}s", t.Minutes, t.Seconds) : string.Format("{0}m", t.Minutes);
        }
        else if (t.TotalDays < 1.0)
        {
            answer = accurate ? string.Format("{0}h {1:D2}m {2:D2}s", (int)t.Hours, t.Minutes, t.Seconds) : string.Format("{0}h {1:D2}m", (int)t.TotalHours, t.Minutes);
        }
        else // more than 1 day
        {
            answer = accurate ? string.Format("{0}d {1:D2}h {2:D2}m {3:D2}s", (int)t.Days, (int)t.Hours, t.Minutes, t.Seconds) : string.Format("{0}d {1}h {2:D2}m", (int)t.TotalDays, (int)t.Hours, t.Minutes);
        }

        return answer;
    }

    public static string PrintTimeSpanFromMilliSeconds(double milliSeconds)
    {
        TimeSpan t = TimeSpan.FromMilliseconds(milliSeconds);
        string answer;
        if (t.TotalMinutes < 1.0)
        {
            answer = string.Format("{0}.{1}s", t.Seconds, (decimal)(t.Milliseconds / 100));
        }
        else if (t.TotalHours < 1.0)
        {
            answer = string.Format("{0}m {1:D2}s", t.TotalMinutes, t.Seconds);
        }
        else if (t.TotalDays < 1.0)
        {
            answer = string.Format("{0}h {1:D2}m {2:D2}s", (int)t.TotalHours, t.Minutes, t.Seconds);
        }
        else
        {
            answer = string.Format("{0}d {1}h {2:D2}m {3:D2}s", (int)t.Days, (int)t.Hours, t.Minutes, t.Seconds);
        }

        return answer;
    }

    public static MirDirection PreviousDir(MirDirection d)
    {
        switch (d)
        {
            case MirDirection.Up:
                return MirDirection.UpLeft;
            case MirDirection.UpRight:
                return MirDirection.Up;
            case MirDirection.Right:
                return MirDirection.UpRight;
            case MirDirection.DownRight:
                return MirDirection.Right;
            case MirDirection.Down:
                return MirDirection.DownRight;
            case MirDirection.DownLeft:
                return MirDirection.Down;
            case MirDirection.Left:
                return MirDirection.DownLeft;
            case MirDirection.UpLeft:
                return MirDirection.Left;
            default: return d;
        }
    }
    public static MirDirection NextDir(MirDirection d)
    {
        switch (d)
        {
            case MirDirection.Up:
                return MirDirection.UpRight;
            case MirDirection.UpRight:
                return MirDirection.Right;
            case MirDirection.Right:
                return MirDirection.DownRight;
            case MirDirection.DownRight:
                return MirDirection.Down;
            case MirDirection.Down:
                return MirDirection.DownLeft;
            case MirDirection.DownLeft:
                return MirDirection.Left;
            case MirDirection.Left:
                return MirDirection.UpLeft;
            case MirDirection.UpLeft:
                return MirDirection.Up;
            default: return d;
        }
    }
    public static MirDirection DirectionFromPoint(Point source, Point dest)
    {
        if (source.X < dest.X)
        {
            if (source.Y < dest.Y)
                return MirDirection.DownRight;
            if (source.Y > dest.Y)
                return MirDirection.UpRight;
            return MirDirection.Right;
        }

        if (source.X > dest.X)
        {
            if (source.Y < dest.Y)
                return MirDirection.DownLeft;
            if (source.Y > dest.Y)
                return MirDirection.UpLeft;
            return MirDirection.Left;
        }

        return source.Y < dest.Y ? MirDirection.Down : MirDirection.Up;
    }



    public static Size Add(this Size p1, Size p2)
    {
        return new Size(p1.Width + p2.Width, p1.Height + p2.Height);
    }
    public static Size Add(this Size p1, int width, int height)
    {
        return new Size(p1.Width + width, p1.Height + height);
    }

    public static Point PointMove(Point p, MirDirection d, int i)
    {
        switch (d)
        {
            case MirDirection.Up:
                p.Offset(0, -i);
                break;
            case MirDirection.UpRight:
                p.Offset(i, -i);
                break;
            case MirDirection.Right:
                p.Offset(i, 0);
                break;
            case MirDirection.DownRight:
                p.Offset(i, i);
                break;
            case MirDirection.Down:
                p.Offset(0, i);
                break;
            case MirDirection.DownLeft:
                p.Offset(-i, i);
                break;
            case MirDirection.Left:
                p.Offset(-i, 0);
                break;
            case MirDirection.UpLeft:
                p.Offset(-i, -i);
                break;
        }
        return p;
    }
    public static Point Left(Point p, MirDirection d)
    {
        switch (d)
        {
            case MirDirection.Up:
                p.Offset(-1, 0);
                break;
            case MirDirection.UpRight:
                p.Offset(-1, -1);
                break;
            case MirDirection.Right:
                p.Offset(0, -1);
                break;
            case MirDirection.DownRight:
                p.Offset(1, -1);
                break;
            case MirDirection.Down:
                p.Offset(1, 0);
                break;
            case MirDirection.DownLeft:
                p.Offset(1, 1);
                break;
            case MirDirection.Left:
                p.Offset(0, 1);
                break;
            case MirDirection.UpLeft:
                p.Offset(-1, 1);
                break;
        }
        return p;
    }

    public static Point Right(Point p, MirDirection d)
    {
        switch (d)
        {
            case MirDirection.Up:
                p.Offset(1, 0);
                break;
            case MirDirection.UpRight:
                p.Offset(1, 1);
                break;
            case MirDirection.Right:
                p.Offset(0, 1);
                break;
            case MirDirection.DownRight:
                p.Offset(-1, 1);
                break;
            case MirDirection.Down:
                p.Offset(-1, 0);
                break;
            case MirDirection.DownLeft:
                p.Offset(-1,-1);
                break;
            case MirDirection.Left:
                p.Offset(0, -1);
                break;
            case MirDirection.UpLeft:
                p.Offset(1, -1);
                break;
        }
        return p;
    }

    public static int MaxDistance(Point p1, Point p2)
    {
        return Math.Max(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));

    }

    public static MirDirection ReverseDirection(MirDirection dir)
    {
        switch (dir)
        {
            case MirDirection.Up:
                return MirDirection.Down;
            case MirDirection.UpRight:
                return MirDirection.DownLeft;
            case MirDirection.Right:
                return MirDirection.Left;
            case MirDirection.DownRight:
                return MirDirection.UpLeft;
            case MirDirection.Down:
                return MirDirection.Up;
            case MirDirection.DownLeft:
                return MirDirection.UpRight;
            case MirDirection.Left:
                return MirDirection.Right;
            case MirDirection.UpLeft:
                return MirDirection.DownRight;
            default:
                return dir;
        }
    }
    public static ItemInfo GetRealItem(ItemInfo Origin, ushort Level, MirClass job, List<ItemInfo> ItemList)
    {
        if (Origin.ClassBased && Origin.LevelBased)
            return GetClassAndLevelBasedItem(Origin, job, Level, ItemList);
        if (Origin.ClassBased)
            return GetClassBasedItem(Origin, job, ItemList);
        if (Origin.LevelBased)
            return GetLevelBasedItem(Origin, Level, ItemList);
        return Origin;
    }
    public static ItemInfo GetLevelBasedItem(ItemInfo Origin, ushort level, List<ItemInfo> ItemList)
    {
        ItemInfo output = Origin;
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemInfo info = ItemList[i];
            if (info.Name.StartsWith(Origin.Name))
                if ((info.RequiredType == RequiredType.Level) && (info.RequiredAmount <= level) && (output.RequiredAmount < info.RequiredAmount) && (Origin.RequiredGender == info.RequiredGender))
                    output = info;
        }
        return output;
    }
    public static ItemInfo GetClassBasedItem(ItemInfo Origin, MirClass job, List<ItemInfo> ItemList)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemInfo info = ItemList[i];
            if (info.Name.StartsWith(Origin.Name))
                if (((byte)info.RequiredClass == (1 << (byte)job)) && (Origin.RequiredGender == info.RequiredGender))
                    return info;
        }
        return Origin;
    }

    public static ItemInfo GetClassAndLevelBasedItem(ItemInfo Origin, MirClass job, ushort level, List<ItemInfo> ItemList)
    {
        ItemInfo output = Origin;
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemInfo info = ItemList[i];
            if (info.Name.StartsWith(Origin.Name))
                if ((byte)info.RequiredClass == (1 << (byte)job))
                    if ((info.RequiredType == RequiredType.Level) && (info.RequiredAmount <= level) && (output.RequiredAmount <= info.RequiredAmount) && (Origin.RequiredGender == info.RequiredGender))
                        output = info;
        }
        return output;
    }

    public static string StringOverLines(string line, int maxWordsPerLine, int maxLettersPerLine)
    {
        string newString = string.Empty;

        string[] words = line.Split(' ');

        int lineLength = 0;

        for (int i = 0; i < words.Length; i++)
        {
            lineLength += words[i].Length + 1;

            newString += words[i] + " ";
            if (i > 0 && i % maxWordsPerLine == 0 && lineLength > maxLettersPerLine)
            {
                lineLength = 0;
                newString += "\r\n";
            }
        }

        return newString;
    }

    public static byte[] ImageToByteArray(Image imageIn)
    {
        MemoryStream ms = new MemoryStream();
        imageIn.Save(ms, ImageFormat.Gif);
        return ms.ToArray();
    }

    public static Image ByteArrayToImage(byte[] byteArrayIn)
    {
        MemoryStream ms = new MemoryStream(byteArrayIn);
        Image returnImage = Image.FromStream(ms);
        return returnImage;
    }

    public static IEnumerable<byte[]> SplitArray(byte[] value, int bufferLength)
    {
        int countOfArray = value.Length / bufferLength;
        if (value.Length % bufferLength > 0)
            countOfArray++;
        for (int i = 0; i < countOfArray; i++)
        {
            yield return value.Skip(i * bufferLength).Take(bufferLength).ToArray();
        }
    }

    public static byte[] CombineArray(List<byte[]> arrays)
    {
        byte[] rv = new byte[arrays.Sum(x => x.Length)];
        int offset = 0;
        foreach (byte[] array in arrays)
        {
            System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }
        return rv;
    }

    internal static string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }

    public static int GetPercentage(int percent, int origNumber)
    {
        return origNumber * percent / 100;
    }
}

public class SelectInfo
{
    public int Index;
    public string Name = string.Empty;
    public ushort Level;
    public MirClass Class;
    public MirGender Gender;
    public DateTime LastAccess;
    public bool HumUp;

        public SelectInfo()
        { }
        public SelectInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Name = reader.ReadString();
            Level = reader.ReadUInt16();
            Class = (MirClass)reader.ReadByte();
            Gender = (MirGender)reader.ReadByte();
            LastAccess = DateTime.FromBinary(reader.ReadInt64());
            HumUp = reader.ReadBoolean();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Name);
            writer.Write(Level);
            writer.Write((byte)Class);
            writer.Write((byte)Gender);
            writer.Write(LastAccess.ToBinary());
            writer.Write(HumUp);
        }
}

public enum ElementPos : byte
{
    None = 0,
    Water = 1,
    Earth = 2,
    Air = 4,
    Wind = 5,
    Holy = 6,
    Dark = 7,
    Phantom = 8,
    Fire = 9
}
public enum ElementNeg : byte
{
    None = 0,
    Water = 1,
    Earth = 2,
    Air = 4,
    Wind = 5,
    Holy = 6,
    Dark = 7,
    Phantom = 8,
    Fire = 9
}
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
    public byte Weight, Light, RequiredAmount;

    public ushort Image, Durability;

    public uint Price, StackSize = 1;

    public ushort MinAC, MaxAC, MinMAC, MaxMAC, MinDC, MaxDC, MinMC, MaxMC, MinSC, MaxSC, Accuracy, Agility;
    public ushort HP, MP;
    public sbyte AttackSpeed, Luck;
    public byte BagWeight, HandWeight, WearWeight;

    public bool StartItem;
    public byte Effect;
    public byte WeaponEffects;
    public byte ItemGlow;

    public byte Strong;
    public byte MagicResist, PoisonResist, HealthRecovery, ManaRecovery, PoisonRecovery, HPrate, MPrate;
    public byte CriticalRate, CriticalDamage;
    public bool NeedIdentify, ShowGroupPickup, GlobalDropNotify;
    public bool ClassBased;
    public bool HumUpBased;
    public bool HumUpRestricted;
    public WearType WearType = WearType.All;
    public WearType LvlableBy = WearType.All;

    public bool LevelBased;
    public bool CanMine;
    public bool CanFastRun;
    public bool CanAwakening;
    public byte MaxAcRate, MaxMacRate, Holy, Freezing, PoisonAttack, HpDrainRate;
    public int UpdateTo;
    public string UpdateToName = string.Empty;
    
    public BindMode Bind = BindMode.none;
    public byte Reflect;
    public SpecialItemMode Unique = SpecialItemMode.None;
    public byte RandomStatsId;
    public RandomItemStat RandomStats;
    public string ToolTip = string.Empty;

    public bool AllowLvlSys = false;
    public bool AllowRandomStats = false;

    public int[] LevelWeapLooks = new int[10];
    public int[] LevelArmourLooks = new int[10];
    public int[] LevelItemLooks = new int[10];
    public int[] LevelWeapEffect = new int[10];
    public int[] LevelItemGlow = new int[10];
    public int[] LevelArmourEffect = new int[10];

    public int[] LvlSysExp = new int[10];

    public int[] LvlSysMinAC = new int[10];
    public int[] LvlSysMaxAC = new int[10];

    public int[] LvlSysMinMAC = new int[10];
    public int[] LvlSysMaxMAC = new int[10];

    public int[] LvlSysMinSC = new int[10];
    public int[] LvlSysMaxSC = new int[10];

    public int[] LvlSysMinMC = new int[10];
    public int[] LvlSysMaxMC = new int[10];

    public int[] LvlSysMinDC = new int[10];
    public int[] LvlSysMaxDC = new int[10];

    public byte MinSocket = 0, MaxSocket = 0;
    public ElementPos PositiveElement = ElementPos.None;
    public ElementNeg NegativeElement = ElementNeg.None;

    public byte PositiveElementAmount = 0;
    public byte NegativeElementAmount = 0;

    public byte BaseRate, BaseRateDrop, MaxStats, MaxGemStat;
    public byte SocketAdd;

    public static ItemInfo CloneItem(ItemInfo item)
    {
        ItemInfo Clone = new ItemInfo
        {
            Index = item.Index,
            Accuracy = item.Accuracy,
            Agility = item.Agility,
            AttackSpeed = item.AttackSpeed,
            BagWeight = item.BagWeight,
            Bind = item.Bind,
            CanAwakening = item.CanAwakening,
            CanFastRun = item.CanFastRun,
            CanMine = item.CanMine,
            ClassBased = item.ClassBased,
            HumUpBased = item.HumUpBased,
            HumUpRestricted = item.HumUpRestricted,
            WearType = item.WearType,
            LvlableBy = item.LvlableBy,
            UpdateTo = item.UpdateTo,
            CriticalDamage = item.CriticalDamage,
            CriticalRate = item.CriticalRate,
            Durability = item.Durability,
            Effect = item.Effect,
            Freezing = item.Freezing,
            Grade = item.Grade,
            HandWeight = item.HandWeight,
            HealthRecovery = item.HealthRecovery,
            Holy = item.Holy,
            HP = item.HP,
            HpDrainRate = item.HpDrainRate,
            HPrate = item.HPrate,
            Image = item.Image,
            LevelBased = item.LevelBased,
            Light = item.Light,
            Luck = item.Luck,
            MagicResist = item.MagicResist,
            MaxAC = item.MaxAC,
            MaxAcRate = item.MaxAcRate,
            MaxDC = item.MaxDC,
            MaxMAC = item.MaxMAC,
            MaxMacRate = item.MaxMacRate,
            MaxMC = item.MaxMC,
            MaxSC = item.MaxSC,
            MinAC = item.MinAC,
            MinDC = item.MinDC,
            MinMAC = item.MinMAC,
            MinMC = item.MinMC,
            MinSC = item.MinSC,
            MP = item.MP,
            MPrate = item.MPrate,
            Name = item.Name,
            NeedIdentify = item.NeedIdentify,
            PoisonAttack = item.PoisonAttack,
            PoisonRecovery = item.PoisonRecovery,
            PoisonResist = item.PoisonResist,
            Price = item.Price,
            RandomStats = item.RandomStats,
            RandomStatsId = item.RandomStatsId,
            Reflect = item.Reflect,
            RequiredAmount = item.RequiredAmount,
            RequiredClass = item.RequiredClass,
            RequiredGender = item.RequiredGender,
            RequiredType = item.RequiredType,
            Set = item.Set,
            Shape = item.Shape,
            ShowGroupPickup = item.ShowGroupPickup,
            ManaRecovery = item.ManaRecovery,
            StackSize = item.StackSize,
            StartItem = item.StartItem,
            Strong = item.Strong,
            ToolTip = item.ToolTip,
            Type = item.Type,
            Unique = item.Unique,
            WearWeight = item.WearWeight,
            Weight = item.Weight,
            WeaponEffects = item.WeaponEffects,
            ItemGlow = item.ItemGlow,
            AllowLvlSys = item.AllowLvlSys,
            AllowRandomStats = item.AllowRandomStats,
            LvlSysExp = item.LvlSysExp,

            LvlSysMinAC = item.LvlSysMinAC,
            LvlSysMaxAC = item.LvlSysMaxAC,

            LvlSysMinMAC = item.LvlSysMinMAC,
            LvlSysMaxMAC = item.LvlSysMaxMAC,

            LvlSysMinSC = item.LvlSysMinSC,
            LvlSysMaxSC = item.LvlSysMaxSC,

            LvlSysMinDC = item.LvlSysMinDC,
            LvlSysMaxDC = item.LvlSysMaxDC,

            LvlSysMinMC = item.LvlSysMinMC,
            LvlSysMaxMC = item.LvlSysMaxMC,

            MinSocket = item.MinSocket,
            MaxSocket = item.MaxSocket,

            PositiveElement = item.PositiveElement,
            PositiveElementAmount = item.PositiveElementAmount,

            NegativeElement = item.NegativeElement,
            NegativeElementAmount = item.NegativeElementAmount,
            LevelArmourLooks = item.LevelArmourLooks,
            LevelWeapLooks = item.LevelWeapLooks,
            LevelItemLooks = item.LevelItemLooks,
            LevelWeapEffect = item.LevelWeapEffect,
            BaseRate = item.BaseRate,
            BaseRateDrop = item.BaseRateDrop,
            MaxStats = item.MaxStats,
            MaxGemStat = item.MaxGemStat,
            SocketAdd = item.SocketAdd
        };

        return Clone;
    }

    public void UpdateItem(ItemInfo item)
    {
        UpdateToName = item.UpdateToName;
        Accuracy = item.Accuracy;
        Agility = item.Agility;
        AttackSpeed = item.AttackSpeed;
        BagWeight = item.BagWeight;
        Bind = item.Bind;
        CanAwakening = item.CanAwakening;
        CanFastRun = item.CanFastRun;
        CanMine = item.CanMine;
        ClassBased = item.ClassBased;
        HumUpBased = item.HumUpBased;
        HumUpRestricted = item.HumUpRestricted;
        WearType = item.WearType;
        LvlableBy = item.LvlableBy;
        UpdateTo = item.UpdateTo;
        CriticalDamage = item.CriticalDamage;
        CriticalRate = item.CriticalRate;
        Durability = item.Durability;
        Effect = item.Effect;
        Freezing = item.Freezing;
        Grade = item.Grade;
        HandWeight = item.HandWeight;
        HealthRecovery = item.HealthRecovery;
        Holy = item.Holy;
        HP = item.HP;
        HpDrainRate = item.HpDrainRate;
        HPrate = item.HPrate;
        Image = item.Image;
        Index = item.Index;
        LevelBased = item.LevelBased;
        Light = item.Light;
        Luck = item.Luck;
        MagicResist = item.MagicResist;
        MaxAC = item.MaxAC;
        MaxAcRate = item.MaxAcRate;
        MaxDC = item.MaxDC;
        MaxMAC = item.MaxMAC;
        MaxMacRate = item.MaxMacRate;
        MaxMC = item.MaxMC;
        MaxSC = item.MaxSC;
        MinAC = item.MinAC;
        MinDC = item.MinDC;
        MinMAC = item.MinMAC;
        MinMC = item.MinMC;
        MinSC = item.MinSC;
        MP = item.MP;
        MPrate = item.MPrate;
        Name = item.Name;
        NeedIdentify = item.NeedIdentify;
        PoisonAttack = item.PoisonAttack;
        PoisonRecovery = item.PoisonRecovery;
        PoisonResist = item.PoisonResist;
        Price = item.Price;
        RandomStats = item.RandomStats;
        RandomStatsId = item.RandomStatsId;
        Reflect = item.Reflect;
        RequiredAmount = item.RequiredAmount;
        RequiredClass = item.RequiredClass;
        RequiredGender = item.RequiredGender;
        RequiredType = item.RequiredType;
        Set = item.Set;
        Shape = item.Shape;
        ShowGroupPickup = item.ShowGroupPickup;
        ManaRecovery = item.ManaRecovery;
        StackSize = item.StackSize;
        StartItem = item.StartItem;
        Strong = item.Strong;
        ToolTip = item.ToolTip;
        Type = item.Type;
        Unique = item.Unique;
        WearWeight = item.WearWeight;
        Weight = item.Weight;
        WeaponEffects = item.WeaponEffects;
        ItemGlow = item.ItemGlow;
        AllowLvlSys = item.AllowLvlSys;
        AllowRandomStats = item.AllowRandomStats;
        LvlSysExp = item.LvlSysExp;

        LvlSysMinAC = item.LvlSysMinAC;
        LvlSysMaxAC = item.LvlSysMaxAC;

        LvlSysMinMAC = item.LvlSysMinMAC;
        LvlSysMaxMAC = item.LvlSysMaxMAC;

        LvlSysMinSC = item.LvlSysMinSC;
        LvlSysMaxSC = item.LvlSysMaxSC;

        LvlSysMinDC = item.LvlSysMinDC;
        LvlSysMaxDC = item.LvlSysMaxDC;

        LvlSysMinMC = item.LvlSysMinMC;
        LvlSysMaxMC = item.LvlSysMaxMC;

        MinSocket = item.MinSocket;
        MaxSocket = item.MaxSocket;

        PositiveElement = item.PositiveElement;
        PositiveElementAmount = item.PositiveElementAmount;

        NegativeElement = item.NegativeElement;
        NegativeElementAmount = item.NegativeElementAmount;
        LevelArmourLooks = item.LevelArmourLooks;
        LevelWeapLooks = item.LevelWeapLooks;
        LevelItemLooks = item.LevelItemLooks;
        LevelWeapEffect = item.LevelWeapEffect;
        BaseRate = item.BaseRate;
        BaseRateDrop = item.BaseRateDrop;
        MaxStats = item.MaxStats;
        MaxGemStat = item.MaxGemStat;
        SocketAdd = item.SocketAdd;
    }

    public bool IsConsumable
    {
        get { return Type == ItemType.Potion || Type == ItemType.Scroll || Type == ItemType.Food || Type == ItemType.Transform || Type == ItemType.Script; }
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
    }
    public ItemInfo(BinaryReader reader, int version = int.MaxValue, int Customversion = int.MaxValue, bool client = false)
    {
        Index = reader.ReadInt32();
        Name = reader.ReadString();
        Type = (ItemType) reader.ReadByte();
        if (version >= 40) Grade = (ItemGrade)reader.ReadByte();
        RequiredType = (RequiredType) reader.ReadByte();
        RequiredClass = (RequiredClass) reader.ReadByte();
        RequiredGender = (RequiredGender) reader.ReadByte();
        if(version >= 17) Set = (ItemSet)reader.ReadByte();

        Shape = version >= 30 ? reader.ReadInt16() : reader.ReadSByte();
        Weight = reader.ReadByte();
        Light = reader.ReadByte();
        RequiredAmount = reader.ReadByte();

        Image = reader.ReadUInt16();
        Durability = reader.ReadUInt16();

        StackSize = reader.ReadUInt32();
        Price = reader.ReadUInt32();

        if (version < 105)
        {
            MinAC = reader.ReadByte();
            MaxAC = reader.ReadByte();
            MinMAC = reader.ReadByte();
            MaxMAC = reader.ReadByte();
            MinDC = reader.ReadByte();
            MaxDC = reader.ReadByte();
            MinMC = reader.ReadByte();
            MaxMC = reader.ReadByte();
            MinSC = reader.ReadByte();
            MaxSC = reader.ReadByte();
        }
        else
        {
            MinAC = reader.ReadUInt16();
            MaxAC = reader.ReadUInt16();
            MinMAC = reader.ReadUInt16();
            MaxMAC = reader.ReadUInt16();
            MinDC = reader.ReadUInt16();
            MaxDC = reader.ReadUInt16();
            MinMC = reader.ReadUInt16();
            MaxMC = reader.ReadUInt16();
            MinSC = reader.ReadUInt16();
            MaxSC = reader.ReadUInt16();
        }

        if (version < 25)
        {
            HP = reader.ReadByte();
            MP = reader.ReadByte();
        }
        else
        {
            HP = reader.ReadUInt16();
            MP = reader.ReadUInt16();
        }

        if (version < 105)
        {
            Accuracy = reader.ReadByte();
            Agility = reader.ReadByte();
        }
        else
        {
            Accuracy = reader.ReadUInt16();
            Agility = reader.ReadUInt16();
        }

        Luck = reader.ReadSByte();
        AttackSpeed = reader.ReadSByte();

        StartItem = reader.ReadBoolean();

        BagWeight = reader.ReadByte();
        HandWeight = reader.ReadByte();
        WearWeight = reader.ReadByte();

        if (version >= 9) Effect = reader.ReadByte();
        if (version >= 20)
        {
            Strong = reader.ReadByte();
            MagicResist = reader.ReadByte();
            PoisonResist = reader.ReadByte();
            HealthRecovery = reader.ReadByte();
            ManaRecovery = reader.ReadByte();
            PoisonRecovery = reader.ReadByte();
            HPrate = reader.ReadByte();
            MPrate = reader.ReadByte();
            CriticalRate = reader.ReadByte();
            CriticalDamage = reader.ReadByte();
            byte bools = reader.ReadByte();
            NeedIdentify = (bools & 0x01) == 0x01;
            ShowGroupPickup = (bools & 0x02) == 0x02;
            ClassBased = (bools & 0x04) == 0x04;
            LevelBased = (bools & 0x08) == 0x08;
            CanMine = (bools & 0x10) == 0x10;

            if (version >= 77)
                GlobalDropNotify = (bools & 0x20) == 0x20;

            MaxAcRate = reader.ReadByte();
            MaxMacRate = reader.ReadByte();
            Holy = reader.ReadByte();
            Freezing = reader.ReadByte();
            PoisonAttack = reader.ReadByte();
            if (version < 55)
            {
                Bind = (BindMode)reader.ReadByte();
            }
            else
            {
                Bind = (BindMode)reader.ReadInt16();
            }
            
        }
        if (version >= 21)
        {
            Reflect = reader.ReadByte();
            HpDrainRate = reader.ReadByte();
            Unique = (SpecialItemMode)reader.ReadInt16();
        }
        if (version >= 24)
        {
            RandomStatsId = reader.ReadByte();
        }
        else
        {
            RandomStatsId = 255;
            if ((Type == ItemType.Weapon) || (Type == ItemType.Armour) || (Type == ItemType.Helmet) || (Type == ItemType.Necklace) || (Type == ItemType.Bracelet) || (Type == ItemType.Ring) || (Type == ItemType.Mount))
                RandomStatsId = (byte)Type;
            if ((Type == ItemType.Belt) || (Type == ItemType.Boots))
                RandomStatsId = 7;
        }

        if (version >= 40) CanFastRun = reader.ReadBoolean();

        if (version >= 41)
        {
            CanAwakening = reader.ReadBoolean();
            bool isTooltip = reader.ReadBoolean();
            if (isTooltip)
                ToolTip = reader.ReadString();
        }
        if (version < 70) //before db version 70 all specialitems had wedding rings disabled, after that it became a server option
        {
            if ((Type == ItemType.Ring) &&  (Unique != SpecialItemMode.None))
                Bind |= BindMode.NoWeddingRing;
        }

        if (version > 71)
        {
            WeaponEffects = reader.ReadByte();
        }

        if (version > 76)
        {
            ItemGlow = reader.ReadByte();
        }

        if (version > 81)
        {
            HumUpBased = reader.ReadBoolean();
        }

        if (version > 83)
        {
            HumUpRestricted = reader.ReadBoolean();
        }

        if (version > 95)
        {
           UpdateTo  = reader.ReadInt32();
        }

        if (version > 97)
        {
            WearType = (WearType)reader.ReadByte();
        }

         AllowLvlSys = reader.ReadBoolean();
        //  maybe I can fix jps shitty code like this lol
        if (version < 134)
        {
            for (int i = 0; i < LvlSysExp.Length; i++)
                LvlSysExp[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMinAC.Length; i++)
                LvlSysMinAC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxAC.Length; i++)
                LvlSysMaxAC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMinMAC.Length; i++)
                LvlSysMinMAC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxMAC.Length; i++)
                LvlSysMaxMAC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMinDC.Length; i++)
                LvlSysMinDC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxDC.Length; i++)
                LvlSysMaxDC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMinMC.Length; i++)
                LvlSysMinMC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxMC.Length; i++)
                LvlSysMaxMC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMinSC.Length; i++)
                LvlSysMinSC[i] = reader.ReadInt32();

            for (int i = 0; i < LvlSysMaxSC.Length; i++)
                LvlSysMaxSC[i] = reader.ReadInt32();
            LvlableBy = (WearType)reader.ReadByte();
            AllowRandomStats = reader.ReadBoolean();
            MinSocket = reader.ReadByte();
            MaxSocket = reader.ReadByte();
            PositiveElement = (ElementPos)reader.ReadByte();
            PositiveElementAmount = reader.ReadByte();
            NegativeElement = (ElementNeg)reader.ReadByte();
            NegativeElementAmount = reader.ReadByte();
            if (version > 128)
            {
                for (int i = 0; i < LevelArmourLooks.Length; i++)
                    LevelArmourLooks[i] = reader.ReadInt32();
                for (int i = 0; i < LevelWeapLooks.Length; i++)
                    LevelWeapLooks[i] = reader.ReadInt32();
                for (int i = 0; i < LevelItemLooks.Length; i++)
                    LevelItemLooks[i] = reader.ReadInt32();
            }

            if (version > 130)
            {
                BaseRate = reader.ReadByte();
                BaseRateDrop = reader.ReadByte();
                MaxStats = reader.ReadByte();
                MaxGemStat = reader.ReadByte();
            }

            if (version > 131)
            {
                SocketAdd = reader.ReadByte();
            }

            if (version > 132)
            {

                for (int i = 0; i < LevelWeapEffect.Length; i++)
                    LevelWeapEffect[i] = reader.ReadInt32();
            }
        }
        else
        {
            if (AllowLvlSys)
            {
                for (int i = 0; i < LvlSysExp.Length; i++)
                    LvlSysExp[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMinAC.Length; i++)
                    LvlSysMinAC[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMaxAC.Length; i++)
                    LvlSysMaxAC[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMinMAC.Length; i++)
                    LvlSysMinMAC[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMaxMAC.Length; i++)
                    LvlSysMaxMAC[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMinDC.Length; i++)
                    LvlSysMinDC[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMaxDC.Length; i++)
                    LvlSysMaxDC[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMinMC.Length; i++)
                    LvlSysMinMC[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMaxMC.Length; i++)
                    LvlSysMaxMC[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMinSC.Length; i++)
                    LvlSysMinSC[i] = reader.ReadInt32();

                for (int i = 0; i < LvlSysMaxSC.Length; i++)
                    LvlSysMaxSC[i] = reader.ReadInt32();
                for (int i = 0; i < LevelArmourLooks.Length; i++)
                    LevelArmourLooks[i] = reader.ReadInt32();
                for (int i = 0; i < LevelWeapLooks.Length; i++)
                    LevelWeapLooks[i] = reader.ReadInt32();
                for (int i = 0; i < LevelItemLooks.Length; i++)
                    LevelItemLooks[i] = reader.ReadInt32();
                for (int i = 0; i < LevelWeapEffect.Length; i++)
                    LevelWeapEffect[i] = reader.ReadInt32();
                if (version > 134)
                {
                    for (int i = 0; i < LevelArmourEffect.Length; i++)
                        LevelArmourEffect[i] = reader.ReadInt32();
                    for (int i = 0; i < LevelItemGlow.Length; i++)
                        LevelItemGlow[i] = reader.ReadInt32();
                }
            }
            LvlableBy = (WearType)reader.ReadByte();
            AllowRandomStats = reader.ReadBoolean();
            MinSocket = reader.ReadByte();
            MaxSocket = reader.ReadByte();
            PositiveElement = (ElementPos)reader.ReadByte();
            PositiveElementAmount = reader.ReadByte();
            NegativeElement = (ElementNeg)reader.ReadByte();
            NegativeElementAmount = reader.ReadByte();
            if (version > 130)
            {
                BaseRate = reader.ReadByte();
                BaseRateDrop = reader.ReadByte();
                MaxStats = reader.ReadByte();
                MaxGemStat = reader.ReadByte();
            }

            if (version > 131)
            {
                SocketAdd = reader.ReadByte();
            }
        }
        
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        writer.Write(Name);
        writer.Write((byte) Type);
        writer.Write((byte) Grade);
        writer.Write((byte) RequiredType);
        writer.Write((byte) RequiredClass);
        writer.Write((byte) RequiredGender);
        writer.Write((byte) Set);

        writer.Write(Shape);
        writer.Write(Weight);
        writer.Write(Light);
        writer.Write(RequiredAmount);     

        writer.Write(Image);
        writer.Write(Durability);

        writer.Write(StackSize);
        writer.Write(Price);

        writer.Write(MinAC);
        writer.Write(MaxAC);
        writer.Write(MinMAC);
        writer.Write(MaxMAC);
        writer.Write(MinDC);
        writer.Write(MaxDC);
        writer.Write(MinMC);
        writer.Write(MaxMC);
        writer.Write(MinSC);
        writer.Write(MaxSC);
        writer.Write(HP);
        writer.Write(MP);
        writer.Write(Accuracy);
        writer.Write(Agility);

        writer.Write(Luck);
        writer.Write(AttackSpeed);

        writer.Write(StartItem);

        writer.Write(BagWeight);
        writer.Write(HandWeight);
        writer.Write(WearWeight);

        writer.Write(Effect);
        writer.Write(Strong);
        writer.Write(MagicResist);
        writer.Write(PoisonResist);
        writer.Write(HealthRecovery);
        writer.Write(ManaRecovery);
        writer.Write(PoisonRecovery);
        writer.Write(HPrate);
        writer.Write(MPrate);
        writer.Write(CriticalRate);
        writer.Write(CriticalDamage);
        byte bools = 0;
        if (NeedIdentify) bools |= 0x01;
        if (ShowGroupPickup) bools |= 0x02;
        if (ClassBased) bools |= 0x04;
        if (LevelBased) bools |= 0x08;
        if (CanMine) bools |= 0x10;
        if (GlobalDropNotify) bools |= 0x20;
        writer.Write(bools);
        writer.Write(MaxAcRate);
        writer.Write(MaxMacRate);
        writer.Write(Holy);
        writer.Write(Freezing);
        writer.Write(PoisonAttack);
        writer.Write((short)Bind);
        writer.Write(Reflect);
        writer.Write(HpDrainRate);
        writer.Write((short)Unique);
        writer.Write(RandomStatsId);
        writer.Write(CanFastRun);
		writer.Write(CanAwakening);
        writer.Write(ToolTip != null);
        if (ToolTip != null)
            writer.Write(ToolTip);

        writer.Write(WeaponEffects);
        writer.Write(ItemGlow);
        writer.Write(HumUpBased);
        writer.Write(HumUpRestricted);
        writer.Write(UpdateTo);
        writer.Write((byte)WearType);


        
        writer.Write(AllowLvlSys);
        if (AllowLvlSys)
        {
            for (int i = 0; i < LvlSysExp.Length; i++)
            {
                writer.Write(LvlSysExp[i]);
            }

            for (int i = 0; i < LvlSysMinAC.Length; i++)
            {
                writer.Write(LvlSysMinAC[i]);
            }
            for (int i = 0; i < LvlSysMaxAC.Length; i++)
            {
                writer.Write(LvlSysMaxAC[i]);
            }

            for (int i = 0; i < LvlSysMinMAC.Length; i++)
            {
                writer.Write(LvlSysMinMAC[i]);
            }
            for (int i = 0; i < LvlSysMaxMAC.Length; i++)
            {
                writer.Write(LvlSysMaxMAC[i]);
            }

            for (int i = 0; i < LvlSysMinDC.Length; i++)
            {
                writer.Write(LvlSysMinDC[i]);
            }
            for (int i = 0; i < LvlSysMaxDC.Length; i++)
            {
                writer.Write(LvlSysMaxDC[i]);
            }

            for (int i = 0; i < LvlSysMinMC.Length; i++)
            {
                writer.Write(LvlSysMinMC[i]);
            }
            for (int i = 0; i < LvlSysMaxMC.Length; i++)
            {
                writer.Write(LvlSysMaxMC[i]);
            }

            for (int i = 0; i < LvlSysMinSC.Length; i++)
            {
                writer.Write(LvlSysMinSC[i]);
            }
            for (int i = 0; i < LvlSysMaxSC.Length; i++)
            {
                writer.Write(LvlSysMaxSC[i]);
            }
            for (int i = 0; i < LevelArmourLooks.Length; i++)
                writer.Write(LevelArmourLooks[i]);

            for (int i = 0; i < LevelWeapLooks.Length; i++)
                writer.Write(LevelWeapLooks[i]);

            for (int i = 0; i < LevelItemLooks.Length; i++)
                writer.Write(LevelItemLooks[i]);

            for (int i = 0; i < LevelWeapEffect.Length; i++)
                writer.Write(LevelWeapEffect[i]);

            for (int i = 0; i < LevelArmourEffect.Length; i++)
                writer.Write(LevelArmourEffect[i]);
            for (int i = 0; i < LevelItemGlow.Length; i++)
                writer.Write(LevelItemGlow[i]);
        }
        writer.Write((byte)LvlableBy);
        writer.Write(AllowRandomStats);


        writer.Write(MinSocket);
        writer.Write(MaxSocket);
        writer.Write((byte)PositiveElement);
        writer.Write(PositiveElementAmount);
        writer.Write((byte)NegativeElement);
        writer.Write(NegativeElementAmount);


        writer.Write(BaseRate);
        writer.Write(BaseRateDrop);
        writer.Write(MaxStats);
        writer.Write(MaxGemStat);

        writer.Write(SocketAdd);
    }

    public static ItemInfo FromText(string text)
    {
        string[] data = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (data.Length < 69) return null;

        ItemInfo info = new ItemInfo { Name = data[0] };

        if (!Enum.TryParse(data[1], out info.Type)) return null;
        if (!Enum.TryParse(data[2], out info.Grade)) return null;
        if (!Enum.TryParse(data[3], out info.RequiredType)) return null;
        if (!Enum.TryParse(data[4], out info.RequiredClass)) return null;
        if (!Enum.TryParse(data[5], out info.RequiredGender)) return null;
        if (!Enum.TryParse(data[6], out info.Set)) return null;
        if (!short.TryParse(data[7], out info.Shape)) return null;

        if (!byte.TryParse(data[8], out info.Weight)) return null;
        if (!byte.TryParse(data[9], out info.Light)) return null;
        if (!byte.TryParse(data[10], out info.RequiredAmount)) return null;

        if (!ushort.TryParse(data[11], out info.MinAC)) return null;
        if (!ushort.TryParse(data[12], out info.MaxAC)) return null;
        if (!ushort.TryParse(data[13], out info.MinMAC)) return null;
        if (!ushort.TryParse(data[14], out info.MaxMAC)) return null;
        if (!ushort.TryParse(data[15], out info.MinDC)) return null;
        if (!ushort.TryParse(data[16], out info.MaxDC)) return null;
        if (!ushort.TryParse(data[17], out info.MinMC)) return null;
        if (!ushort.TryParse(data[18], out info.MaxMC)) return null;
        if (!ushort.TryParse(data[19], out info.MinSC)) return null;
        if (!ushort.TryParse(data[20], out info.MaxSC)) return null;
        if (!ushort.TryParse(data[21], out info.Accuracy)) return null;
        if (!ushort.TryParse(data[22], out info.Agility)) return null;
        if (!ushort.TryParse(data[23], out info.HP)) return null;
        if (!ushort.TryParse(data[24], out info.MP)) return null;

        if (!sbyte.TryParse(data[25], out info.AttackSpeed)) return null;
        if (!sbyte.TryParse(data[26], out info.Luck)) return null;

        if (!byte.TryParse(data[27], out info.BagWeight)) return null;

        if (!byte.TryParse(data[28], out info.HandWeight)) return null;
        if (!byte.TryParse(data[29], out info.WearWeight)) return null;

        if (!bool.TryParse(data[30], out info.StartItem)) return null;

        if (!ushort.TryParse(data[31], out info.Image)) return null;
        if (!ushort.TryParse(data[32], out info.Durability)) return null;
        if (!uint.TryParse(data[33], out info.Price)) return null;
        if (!uint.TryParse(data[34], out info.StackSize)) return null;
        if (!byte.TryParse(data[35], out info.Effect)) return null;

        if (!byte.TryParse(data[36], out info.Strong)) return null;
        if (!byte.TryParse(data[37], out info.MagicResist)) return null;
        if (!byte.TryParse(data[38], out info.PoisonResist)) return null;
        if (!byte.TryParse(data[39], out info.HealthRecovery)) return null;
        if (!byte.TryParse(data[40], out info.ManaRecovery)) return null;
        if (!byte.TryParse(data[41], out info.PoisonRecovery)) return null;
        if (!byte.TryParse(data[42], out info.HPrate)) return null;
        if (!byte.TryParse(data[43], out info.MPrate)) return null;
        if (!byte.TryParse(data[44], out info.CriticalRate)) return null;
        if (!byte.TryParse(data[45], out info.CriticalDamage)) return null;
        if (!bool.TryParse(data[46], out info.NeedIdentify)) return null;
        if (!bool.TryParse(data[47], out info.ShowGroupPickup)) return null;
        if (!byte.TryParse(data[48], out info.MaxAcRate)) return null;
        if (!byte.TryParse(data[49], out info.MaxMacRate)) return null;
        if (!byte.TryParse(data[50], out info.Holy)) return null;
        if (!byte.TryParse(data[51], out info.Freezing)) return null;
        if (!byte.TryParse(data[52], out info.PoisonAttack)) return null;
        if (!bool.TryParse(data[53], out info.ClassBased)) return null;
        if (!bool.TryParse(data[54], out info.LevelBased)) return null;
        if (!Enum.TryParse(data[55], out info.Bind)) return null;
        if (!byte.TryParse(data[56], out info.Reflect)) return null;
        if (!byte.TryParse(data[57], out info.HpDrainRate)) return null;
        if (!Enum.TryParse(data[58], out info.Unique)) return null;
        if (!byte.TryParse(data[59], out info.RandomStatsId)) return null;
        if (!bool.TryParse(data[60], out info.CanMine)) return null;
        if (!bool.TryParse(data[61], out info.CanFastRun)) return null;
		if (!bool.TryParse(data[62], out info.CanAwakening)) return null;
        if (data[63] == "-")
            info.ToolTip = "";
        else
        {
            info.ToolTip = data[63];
            info.ToolTip = info.ToolTip.Replace("&^&", "\r\n");
        }
        if (!byte.TryParse(data[64], out info.WeaponEffects)) return null;
        if (!byte.TryParse(data[65], out info.ItemGlow)) return null;
        if (!bool.TryParse(data[66], out info.HumUpBased)) return null;
        if (!bool.TryParse(data[67], out info.HumUpRestricted)) return null;
        if (!int.TryParse(data[68], out info.UpdateTo)) return null;
        if (!Enum.TryParse(data[69], out info.WearType)) return null;
        if (!Enum.TryParse(data[70], out info.LvlableBy)) return null;
        if (!byte.TryParse(data[71], out info.MinSocket))
            return null;
        if (!byte.TryParse(data[72], out info.MaxSocket))
            return null;
        if (!Enum.TryParse(data[73], out info.PositiveElement))
            return null;
        if (!byte.TryParse(data[74], out info.PositiveElementAmount))
            return null;
        if (!Enum.TryParse(data[75], out info.NegativeElement))
            return null;
        if (!byte.TryParse(data[76], out info.NegativeElementAmount))
            return null;
        if (data[77] != null &&
            data[77].Length > 0)
        {
            string[] tmp = data[77].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysExp = levelarray;
            if (info.LvlSysExp == null)
                return null;
        }
        if (data[78] != null &&
            data[78].Length > 0)
        {
            string[] tmp = data[78].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMinAC = levelarray;
            if (info.LvlSysMinAC == null)
                return null;
        }
        if (data[79] != null &&
            data[79].Length > 0)
        {
            string[] tmp = data[79].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMaxAC = levelarray;
            if (info.LvlSysMaxAC == null)
                return null;
        }
        if (data[80] != null &&
            data[80].Length > 0)
        {
            string[] tmp = data[80].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMinMAC = levelarray;
            if (info.LvlSysMinMAC == null)
                return null;
        }
        if (data[81] != null &&
            data[81].Length > 0)
        {
            string[] tmp = data[81].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMaxMAC = levelarray;
            if (info.LvlSysMaxMAC == null)
                return null;
        }
        if (data[82] != null &&
            data[82].Length > 0)
        {
            string[] tmp = data[82].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMinDC = levelarray;
            if (info.LvlSysMinDC == null)
                return null;
        }
        if (data[83] != null &&
            data[83].Length > 0)
        {
            string[] tmp = data[83].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMaxDC = levelarray;
            if (info.LvlSysMaxDC == null)
                return null;
        }

        if (data[84] != null &&
            data[84].Length > 0)
        {
            string[] tmp = data[84].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMinMC = levelarray;
            if (info.LvlSysMinMC == null)
                return null;
        }
        if (data[85] != null &&
            data[85].Length > 0)
        {
            string[] tmp = data[85].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMaxMC = levelarray;
            if (info.LvlSysMaxMC == null)
                return null;
        }
        if (data[86] != null &&
            data[86].Length > 0)
        {
            string[] tmp = data[86].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMinSC = levelarray;
            if (info.LvlSysMinSC == null)
                return null;
        }
        if (data[87] != null &&
            data[87].Length > 0)
        {
            string[] tmp = data[87].Split(' ');
            int[] levelarray = new int[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!int.TryParse(tmp[i], out levelarray[i]))
                    continue;
            }
            info.LvlSysMaxSC = levelarray;
            if (info.LvlSysMaxSC == null)
                return null;
        }
        if (!bool.TryParse(data[88], out info.AllowLvlSys)) return null;
        if (!bool.TryParse(data[89], out info.AllowRandomStats)) return null;
        if (!byte.TryParse(data[90], out info.BaseRate)) return null;
        if (!byte.TryParse(data[91], out info.BaseRateDrop)) return null;
        if (!byte.TryParse(data[92], out info.MaxStats)) return null;
        if (!byte.TryParse(data[93], out info.MaxGemStat)) return null;
        return info;

    }

    public string ToText()
    {
        string TransToolTip = ToolTip;
        int length = TransToolTip.Length;

        if (TransToolTip == null || TransToolTip.Length == 0)
        {
            TransToolTip = "-";
        }
        else
        {
            TransToolTip = TransToolTip.Replace("\r\n", "&^&");
        }
        string levelarray = "";
        string minACarray = "";
        string maxACarray = "";
        string minMACarray = "";
        string maxMACarray = "";
        string minDCarray = "";
        string maxDCarray = "";
        string minMCarray = "";
        string maxMCarray = "";
        string minSCarray = "";
        string maxSCarray = "";
        for (int i = 0; i < LvlSysExp.Length; i++)              //  77       
            levelarray += string.Format("{0} ", LvlSysExp[i]);
        
        for (int i = 0; i < LvlSysMinAC.Length; i++)            //  78
            minACarray += string.Format("{0} ", LvlSysMinAC[i]);
        for (int i = 0; i < LvlSysMaxAC.Length; i++)            //  79
            maxACarray += string.Format("{0} ", LvlSysMaxAC[i]);

        for (int i = 0; i < LvlSysMinMAC.Length; i++)           //  80
            minMACarray += string.Format("{0} ", LvlSysMinMAC[i]);
        for (int i = 0; i < LvlSysMaxMAC.Length; i++)           //  81
            maxMACarray += string.Format("{0} ", LvlSysMaxMAC[i]);

        for (int i = 0; i < LvlSysMinDC.Length; i++)            //  82
            minDCarray += string.Format("{0} ", LvlSysMinDC[i]);
        for (int i = 0; i < LvlSysMaxDC.Length; i++)            //  83
            maxDCarray += string.Format("{0} ", LvlSysMaxDC[i]);
        for (int i = 0; i < LvlSysMinMC.Length; i++)            //  84
            minMCarray += string.Format("{0} ", LvlSysMinMC[i]);
        for (int i = 0; i < LvlSysMaxMC.Length; i++)            //  85
            maxMCarray += string.Format("{0} ", LvlSysMaxMC[i]);
        for (int i = 0; i < LvlSysMinSC.Length; i++)            //  86
            minSCarray += string.Format("{0} ", LvlSysMinSC[i]);
        for (int i = 0; i < LvlSysMaxSC.Length; i++)            //  87
            maxSCarray += string.Format("{0} ", LvlSysMaxSC[i]);

        return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26}," +
                             "{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{51}," +
                             "{52},{53},{54},{55},{56},{57},{58},{59},{60},{61},{62},{63},{64},{65},{66},{67},{68},{69},{70},{71},{72},{73},{74},{75},{76}," +
                             "{77},{78},{79},{80},{81},{82},{83},{84},{85},{86},{87},{88},{89},{90},{91},{92},{93}",
            Name, (byte)Type, (byte)Grade, (byte)RequiredType, (byte)RequiredClass, (byte)RequiredGender, (byte)Set, Shape, Weight, Light, RequiredAmount, MinAC, MaxAC, MinMAC, MaxMAC, MinDC, MaxDC,
            MinMC, MaxMC, MinSC, MaxSC, Accuracy, Agility, HP, MP, AttackSpeed, Luck, BagWeight, HandWeight, WearWeight, StartItem, Image, Durability, Price,
            StackSize, Effect, Strong, MagicResist, PoisonResist, HealthRecovery, ManaRecovery, PoisonRecovery, HPrate, MPrate, CriticalRate, CriticalDamage, NeedIdentify,
            ShowGroupPickup, MaxAcRate, MaxMacRate, Holy, Freezing, PoisonAttack, ClassBased, LevelBased, (short)Bind, Reflect, HpDrainRate, (short)Unique,
            RandomStatsId, CanMine, CanFastRun, CanAwakening, TransToolTip, WeaponEffects, ItemGlow, HumUpBased, HumUpRestricted, UpdateTo, (byte)WearType, (byte)LvlableBy, MinSocket, MaxSocket, (byte)PositiveElement, PositiveElementAmount, (byte)NegativeElement, NegativeElementAmount,
            levelarray,
            minACarray,
            maxACarray,
            minMACarray,
            maxMACarray,
            minDCarray,
            maxDCarray,
            minMCarray,
            maxMCarray,
            minSCarray,
            maxSCarray,
            AllowLvlSys,
            AllowRandomStats, BaseRate, BaseRateDrop, MaxStats, MaxGemStat);
    }

    public override string ToString()
    {
        return string.Format("{0}: {1}", Index, Name);
    }

}

#region Socket System Pete107 Edens-Elite Network
public enum SocketType : byte
{
    DestructionBonus1 = 1,
    DestructionBonus2 = 2,
    DestructionBonus3 = 3,
    DestructionBonus4 = 4,
    DestructionBonus5 = 5,

    MagicBonus1 = 6,
    MagicBonus2 = 7,
    MagicBonus3 = 8,
    MagicBonus4 = 9,
    MagicBonus5 = 10,

    SpiritBonus1 = 11,
    SpiritBonus2 = 12,
    SpiritBonus3 = 13,
    SpiritBonus4 = 14,
    SpiritBonus5 = 15,

    HealthBonus1 = 16,
    HealthBonus2 = 17,
    HealthBonus3 = 18,
    HealthBonus4 = 19,
    HealthBonus5 = 20,

    ManaBonus1 = 21,
    ManaBonus2 = 22,
    ManaBonus3 = 23,
    ManaBonus4 = 24,
    ManaBonus5 = 25,

    HealthRegenBonus1 = 26,
    HealthRegenBonus2 = 27,
    HealthRegenBonus3 = 28,
    HealthRegenBonus4 = 29,
    HealthRegenBonus5 = 30,

    ManaRegenBonus1 = 31,
    ManaRegenBonus2 = 32,
    ManaRegenBonus3 = 33,
    ManaRegenBonus4 = 34,
    ManaRegenBonus5 = 35,

    ReflectBonus1 = 36,
    ReflectBonus2 = 37,
    ReflectBonus3 = 38,
    ReflectBonus4 = 39,
    ReflectBonus5 = 40,

    CritRateBonus1 = 41,
    CritRateBonus2 = 42,
    CritRateBonus3 = 43,
    CritRateBonus4 = 44,
    CritRateBonus5 = 45,

    CritDmgBonus1 = 46,
    CritDmgBonus2 = 47,
    CritDmgBonus3 = 48,
    CritDmgBonus4 = 49,
    CritDmgBonus5 = 50,

    HPDrainBonus1 = 51,
    HPDrainBonus2 = 52,
    HPDrainBonus3 = 53,
    HPDrainBonus4 = 54,
    HPDrainBonus5 = 55,

    //  Small Accuracy Boost
    PinPoint = 100,

    //  Small Attack Speed & Crit bonus.
    Enrage = 105,

    //  Decent Defense.
    IronWall = 110,

    //  Small Agility Boost
    Evasive = 115,

    //  Speedy magician
    SpeedyMagician = 120,
    NONE = 255,
}

public class Sockets
{
    //  Depends on how many Socket Slots an item can have.
    public byte Slot;

    //  We'll get this from UserItem
    public string SocketItemName;

    //  We'll have a few types of socket items
    public bool CanRemove;

    //  The Item used within the socket.
    public UserItem SocketedItem;

    public SocketType SocketItemType;

    public Sockets()
    {

    }

    public Sockets(BinaryReader reader, int version = int.MaxValue)
    {
        Slot = reader.ReadByte();
        bool validItem = reader.ReadBoolean();
        if (validItem)
        {
            SocketedItem = new UserItem(reader, version);
            SocketItemName = reader.ReadString();
            CanRemove = reader.ReadBoolean();
            SocketItemType = (SocketType)reader.ReadByte();
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Slot);
        bool validItem = SocketedItem == null ? false : true;
        writer.Write(validItem);
        if (validItem)
        {
            SocketedItem.Save(writer);
            writer.Write(SocketItemName);
            writer.Write(CanRemove);
            writer.Write((byte)SocketItemType);
        }
    }
}
#endregion

public class UserItem
{
    public ulong UniqueID;
    public int ItemIndex;

    public ItemInfo Info;
    public ushort CurrentDura, MaxDura;
    public uint Count = 1, GemCount = 0;
    public ushort MinAC, MaxAC;
    public ushort MinMAC, MaxMAC;
    public ushort MinDC, MaxDC;
    public ushort MinMC, MaxMC;
    public ushort MinSC, MaxSC;
    public byte AC, MAC, DC, MC, SC, Accuracy, Agility, HP, MP, Strong, MagicResist, PoisonResist, HealthRecovery, ManaRecovery, PoisonRecovery, CriticalRate, CriticalDamage, Freezing, PoisonAttack, HpDrainRate, Reflect, SocketAdd;
    public sbyte AttackSpeed, Luck;

    public RefinedValue RefinedValue = RefinedValue.None;
    public byte RefineAdded = 0;

    public bool DuraChanged;
    public int SoulBoundId = -1;
    public bool Identified = false;
    public bool Cursed = false;

    public int WeddingRing = -1;

    //  Gem | Orb cap = 5
    public UserItem[] Slots = new UserItem[5];

    public DateTime BuybackExpiryDate;

    public ExpireInfo ExpireInfo;

	public Awake Awake = new Awake();

    public byte ShieldLevel = 0;
    public long ShieldExp;
    public long NeedShieldExp;

    public int LvlSysExpGained;
    public int LvlSystem;

    public string GTInvite { get; set; } = string.Empty;

    public List<Sockets> Sockets = new List<Sockets>();
    public byte SocketCount = 0;

    public ElementPos ElementPositive = ElementPos.None;
    public ElementNeg ElementNegative = ElementNeg.None;

    public byte PositiveElementAdded = 0;
    public byte NegativeElementAdded = 0;

    public bool IsAdded
    {
        get
        {
            return AC != 0 || MAC != 0 || DC != 0 || MC != 0 || SC != 0 || Accuracy != 0 || Agility != 0 || HP != 0 || MP != 0 || AttackSpeed != 0 || Luck != 0 || Strong != 0 || MagicResist != 0 || PoisonResist != 0 ||
                HealthRecovery != 0 || ManaRecovery != 0 || PoisonRecovery != 0 || CriticalRate != 0 || CriticalDamage != 0 || Freezing != 0 || PoisonAttack != 0;
        }
    }

    public uint Weight
    {
        get { return Info.Type == ItemType.Amulet ? Info.Weight : Info.Weight*Count; }
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

        SetSlotSize();
    }
    public UserItem(BinaryReader reader, int version = int.MaxValue, int Customversion = int.MaxValue)
    {
        UniqueID = reader.ReadUInt64();
        ItemIndex = reader.ReadInt32();

        CurrentDura = reader.ReadUInt16();
        MaxDura = reader.ReadUInt16();

        Count = reader.ReadUInt32();

        AC = reader.ReadByte();
        MAC = reader.ReadByte();
        DC = reader.ReadByte();
        MC = reader.ReadByte();
        SC = reader.ReadByte();
        Accuracy = reader.ReadByte();
        Agility = reader.ReadByte();
        HP = reader.ReadByte();
        MP = reader.ReadByte();

        AttackSpeed = reader.ReadSByte();
        Luck = reader.ReadSByte();

        if (version <= 19) return;
        SoulBoundId = reader.ReadInt32();
        byte Bools = reader.ReadByte();        
        Identified = (Bools & 0x01) == 0x01;
        Cursed = (Bools & 0x02) == 0x02;
        Strong = reader.ReadByte();
        MagicResist = reader.ReadByte();
        PoisonResist = reader.ReadByte();
        HealthRecovery = reader.ReadByte();
        ManaRecovery = reader.ReadByte();
        PoisonRecovery = reader.ReadByte();
        CriticalRate = reader.ReadByte();
        CriticalDamage = reader.ReadByte();
        Freezing = reader.ReadByte();
        PoisonAttack = reader.ReadByte();
        

        if (version <= 31) return;

        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            if (reader.ReadBoolean()) continue;
            UserItem item = new UserItem(reader, version, Customversion);
            Slots[i] = item;
        }

        if (version <= 38) return;

        GemCount = reader.ReadUInt32();

        if (version <= 40) return;

        Awake = new Awake(reader);

        if (version <= 56) return;

        RefinedValue = (RefinedValue)reader.ReadByte();
        RefineAdded = reader.ReadByte();
        if (version < 60) return;
        WeddingRing = reader.ReadInt32();

        if (version < 65) return;

        if (reader.ReadBoolean())
        {
            ExpireInfo = new ExpireInfo(reader, version, Customversion);
        }
        
        if (version > 89)
        {
            ShieldLevel = reader.ReadByte();
            ShieldExp = reader.ReadInt64();
        }
        if (version > 90)
            NeedShieldExp = reader.ReadInt64();

        if (version > 99)
        {
            LvlSysExpGained = reader.ReadInt32();
            LvlSystem = reader.ReadInt32();
        }

        if (version > 112)
            GTInvite = reader.ReadString();
        if (version > 125)
        {
            ElementPositive = (ElementPos)reader.ReadByte();
            PositiveElementAdded = reader.ReadByte();
            ElementNegative = (ElementNeg)reader.ReadByte();
            NegativeElementAdded = reader.ReadByte();

            SocketCount = reader.ReadByte();


            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Sockets socket = new Sockets(reader, version);
                Sockets.Add(socket);
            }
        }
        if (version > 148)
        {
            MinAC = reader.ReadUInt16();
            MaxAC = reader.ReadUInt16();
            MinMAC = reader.ReadUInt16();
            MaxMAC = reader.ReadUInt16();
            MinDC = reader.ReadUInt16();
            MaxDC = reader.ReadUInt16();
            MinMC = reader.ReadUInt16();
            MaxMC = reader.ReadUInt16();
            MinSC = reader.ReadUInt16();
            MaxSC = reader.ReadUInt16();
        }

    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(UniqueID);
        writer.Write(ItemIndex);

        writer.Write(CurrentDura);
        writer.Write(MaxDura);

        writer.Write(Count);

        writer.Write(AC);
        writer.Write(MAC);
        writer.Write(DC);
        writer.Write(MC);
        writer.Write(SC);

        writer.Write(Accuracy);
        writer.Write(Agility);
        writer.Write(HP);
        writer.Write(MP);

        writer.Write(AttackSpeed);
        writer.Write(Luck);
        writer.Write(SoulBoundId);
        byte Bools=0;        
        if (Identified) Bools |= 0x01;
        if (Cursed) Bools |= 0x02;
        writer.Write(Bools);
        writer.Write(Strong);
        writer.Write(MagicResist);
        writer.Write(PoisonResist);
        writer.Write(HealthRecovery);
        writer.Write(ManaRecovery);
        writer.Write(PoisonRecovery);
        writer.Write(CriticalRate);
        writer.Write(CriticalDamage);
        writer.Write(Freezing);
        writer.Write(PoisonAttack);

        writer.Write(Slots.Length);
        for (int i = 0; i < Slots.Length; i++)
        {
            writer.Write(Slots[i] == null);
            if (Slots[i] == null) continue;

            Slots[i].Save(writer);
        }

        writer.Write(GemCount);

       
        Awake.Save(writer);

        writer.Write((byte)RefinedValue);
        writer.Write(RefineAdded);

        writer.Write(WeddingRing);

        writer.Write(ExpireInfo != null);

        if (ExpireInfo != null)
        {
            ExpireInfo.Save(writer);
        }
        writer.Write(ShieldLevel);
        writer.Write(ShieldExp);
        writer.Write(NeedShieldExp);

        writer.Write(LvlSysExpGained);
        writer.Write(LvlSystem);

        writer.Write(GTInvite);
        
        writer.Write((byte)ElementPositive);
        writer.Write(PositiveElementAdded);
        writer.Write((byte)ElementNegative);
        writer.Write(NegativeElementAdded);

        writer.Write(SocketCount);
        writer.Write(Sockets.Count);
        for (int i = 0; i < Sockets.Count; i++)
            Sockets[i].Save(writer);

        writer.Write(MinAC);
        writer.Write(MaxAC);
        writer.Write(MinMAC);
        writer.Write(MaxMAC);
        writer.Write(MinDC);
        writer.Write(MaxDC);
        writer.Write(MinMC);
        writer.Write(MaxMC);
        writer.Write(MinSC);
        writer.Write(MaxSC);
    }


    public uint Price()
    {
        if (Info == null) return 0;

        uint p = Info.Price;


        if (Info.Durability > 0)//Dura
        {
            float r = ((Info.Price / 2F) / Info.Durability);

            p = (uint)(MaxDura * r);

            if (MaxDura > 0)
                r = CurrentDura / (float)MaxDura;
            else
                r = 0;

            p = (uint)Math.Floor(p / 2F + ((p / 2F) * r) + Info.Price / 2F);//Standard price
        }

        //  Stats
        p = (uint)(p * ((AC + MAC + DC + MC + SC + Accuracy + Agility + HP + MP + AttackSpeed + Luck + Strong + MagicResist + PoisonResist + HealthRecovery + ManaRecovery + PoisonRecovery + CriticalRate + CriticalDamage + Freezing + PoisonAttack) * 0.1F + 1F));
        
        //  Final Price * number of items in stack
        return p * Count;
    }

    public void ItemLevelUp(ushort level, MirClass job, MirGender gender, List<ItemInfo> list)
    {
        ItemInfo realItem = Functions.GetRealItem(Info, level, job, list);
        if (Info.AllowRandomStats)
        {
            
            Random rand = new Random(DateTime.Now.Millisecond);
            
            MC = (byte)Math.Min(byte.MaxValue, MC + rand.Next(0, realItem.LvlSysMaxMC[LvlSystem - 1]));
            DC = (byte)Math.Min(byte.MaxValue, DC + rand.Next(0, realItem.LvlSysMaxDC[LvlSystem - 1]));
            SC = (byte)Math.Min(byte.MaxValue, SC + rand.Next(0, realItem.LvlSysMaxSC[LvlSystem - 1]));

            AC = (byte)Math.Min(byte.MaxValue, AC + rand.Next(0, realItem.LvlSysMaxAC[LvlSystem - 1]));
            MAC = (byte)Math.Min(byte.MaxValue, MAC + rand.Next(0, realItem.LvlSysMaxMAC[LvlSystem - 1]));
        }
        else
        {
            MC = (byte)Math.Min(byte.MaxValue, MC + realItem.LvlSysMaxMC[LvlSystem - 1]);
            DC = (byte)Math.Min(byte.MaxValue, DC + realItem.LvlSysMaxDC[LvlSystem - 1]);
            SC = (byte)Math.Min(byte.MaxValue, SC + realItem.LvlSysMaxSC[LvlSystem - 1]);

            AC = (byte)Math.Min(byte.MaxValue, AC + realItem.LvlSysMaxAC[LvlSystem - 1]);
            MAC = (byte)Math.Min(byte.MaxValue, MAC + realItem.LvlSysMaxMAC[LvlSystem - 1]);
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
        while(loop)
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
    }


    public uint RepairPrice()
    {
        if (Info == null || Info.Durability == 0) return 0;

        var p = Info.Price;

        if (Info.Durability > 0)
        {
            p = (uint)Math.Floor(MaxDura * ((Info.Price / 2F) / Info.Durability) + Info.Price / 2F);
            p = (uint)(p * ((AC + MAC + DC + MC + SC + Accuracy + Agility + HP + MP + AttackSpeed + Luck + Strong + MagicResist + PoisonResist + HealthRecovery + ManaRecovery + PoisonRecovery + CriticalRate + CriticalDamage + Freezing + PoisonAttack) * 0.1F + 1F));

        }

        return (p * Count) - Price();
    }

    public uint Quality()
    {
        uint q = (uint)(AC + MAC + DC + MC + SC + Accuracy + Agility + HP + MP + AttackSpeed + Luck + Strong + MagicResist + PoisonResist + HealthRecovery + ManaRecovery + PoisonRecovery + CriticalRate + CriticalDamage + Freezing + PoisonAttack + Awake.AwakeLevel+ 1);

        return q;
    }

    public uint AwakeningPrice()
    {
        if (Info == null) return 0;

        uint p = 1500;

        p = (uint)((p * (1 + Awake.AwakeLevel* 2)) * (uint)Info.Grade);
       
        return p;
    }

    public uint DisassemblePrice()
    {
        if (Info == null) return 0;

        uint p = 1500 * (uint)Info.Grade;

        p = (uint)(p * ((AC + MAC + DC + MC + SC + Accuracy + Agility + HP + MP + AttackSpeed + Luck + Strong + MagicResist + PoisonResist + HealthRecovery + ManaRecovery + PoisonRecovery + CriticalRate + CriticalDamage + Freezing + PoisonAttack + Awake.AwakeLevel) * 0.1F + 1F));

        return p;
    }

    public uint DowngradePrice()
    {
        if (Info == null) return 0;

        uint p = 3000;

        p = (uint)((p * (1 + (Awake.AwakeLevel+ 1) * 2)) * (uint)Info.Grade);

        return p;
    }

    public uint ResetPrice()
    {
        if (Info == null) return 0;

        uint p = 3000 * (uint)Info.Grade;

        p = (uint)(p * ((AC + MAC + DC + MC + SC + Accuracy + Agility + HP + MP + AttackSpeed + Luck + Strong + MagicResist + PoisonResist + HealthRecovery + ManaRecovery + PoisonRecovery + CriticalRate + CriticalDamage + Freezing + PoisonAttack) * 0.2F + 1F));

        return p;
    }
    public void SetSlotSize()
    {
        int amount = 0;

        switch (Info.Type)
        {
            case ItemType.Mount:
                if (Info.Shape < 7)
                    amount = 4;
                else if (Info.Shape < 12)
                    amount = 5;
                break;
            case ItemType.Weapon:
                if (Info.Shape == 49 || Info.Shape == 50)
                    amount = 5;
                break;
        }

        if (amount == Slots.Length) return;

        Array.Resize(ref Slots, amount);
    }

    public ushort Image
    {
        get
        {
            switch (Info.Type)
            {
                #region Amulet and Poison Stack Image changes
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
                            case 1: //Grey Poison
                                if (Count >= 150) return 3675;
                                if (Count >= 100) return 2960;
                                if (Count >= 50) return 3674;
                                return 3673;
                            case 2: //Yellow Poison
                                if (Count >= 150) return 3672;
                                if (Count >= 100) return 2961;
                                if (Count >= 50) return 3671;
                                return 3670;
                        }
                    }
                    break;
                }

            #endregion
            
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

            AC = AC,
            MAC = MAC,
            DC = DC,
            MC = MC,
            SC = SC,
            Accuracy = Accuracy,
            Agility = Agility,
            HP = HP,
            MP = MP,
            MinAC = MinAC,
            MaxAC = MaxAC,
            MinDC = MinDC,
            MaxDC = MaxDC,
            MinMAC = MinMAC,
            MinMC = MinMC,
            MinSC = MinSC,
            AttackSpeed = AttackSpeed,
            Luck = Luck,

            DuraChanged = DuraChanged,
            SoulBoundId = SoulBoundId,
            Identified = Identified,
            Cursed = Cursed,
            Strong = Strong,
            MagicResist = MagicResist,
            PoisonResist = PoisonResist,
            HealthRecovery = HealthRecovery,
            ManaRecovery = ManaRecovery,
            PoisonRecovery = PoisonRecovery,
            CriticalRate = CriticalRate,
            CriticalDamage = CriticalDamage,
            Freezing = Freezing,
            PoisonAttack = PoisonAttack,

            Slots = Slots,
            Awake = Awake,

            RefinedValue = RefinedValue,
            RefineAdded = RefineAdded,

            ExpireInfo = ExpireInfo,
            ShieldLevel = ShieldLevel,
            ShieldExp = ShieldExp,
            NeedShieldExp = NeedShieldExp,
            LvlSystem = LvlSystem,
            LvlSysExpGained = LvlSysExpGained,
            NegativeElementAdded = NegativeElementAdded,
            ElementNegative = ElementNegative,
            ElementPositive = ElementPositive,
            PositiveElementAdded = PositiveElementAdded,
            SocketCount = SocketCount,
            Sockets = Sockets
            };

        return item;
    }

}

public class ExpireInfo
{
    public DateTime ExpiryDate;

    public ExpireInfo()
    {

    }

    public ExpireInfo(BinaryReader reader, int version = int.MaxValue, int Customversion = int.MaxValue)
    {
        ExpiryDate = DateTime.FromBinary(reader.ReadInt64());
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(ExpiryDate.ToBinary());
    }
}

public class GameShopItem
{
    public int ItemIndex;
    public int GIndex;
    public ItemInfo Info;
    public uint GoldPrice = 0;
    public uint CreditPrice = 0;
    public uint Count = 1;
    public string Class = "";
    public string Category = "";
    public int Stock = 0;
    public bool iStock = false;
    public bool Deal = false;
    public bool TopItem = false;
    public DateTime Date;
    public bool CanBuyGold = false;
    public bool CanBuyCredit = false;


    public GameShopItem()
    {
    }

    public void UpdateItem(GameShopItem g)
    {
        ItemIndex = g.ItemIndex;
        GIndex = g.GIndex;
        Info = g.Info;
        GoldPrice = g.GoldPrice;
        CreditPrice = g.CreditPrice;
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
            Count = g.Count,
            Class = g.Class,
            Category = g.Category,
            Stock = g.Stock,
            iStock = g.iStock,
            Deal = g.Deal,
            TopItem = g.TopItem,
            Date = g.Date,
            CanBuyGold = g.CanBuyGold,
            CanBuyCredit = g.CanBuyCredit
        };

        return n;
    }

    public GameShopItem(BinaryReader reader, int version = int.MaxValue, int Customversion = int.MaxValue)
    {
        //The index is fucked on the items
        ItemIndex = reader.ReadInt32();
        GIndex = reader.ReadInt32();
        GoldPrice = reader.ReadUInt32();
        CreditPrice = reader.ReadUInt32();
        Count = reader.ReadUInt32();
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
        }
    }

    public GameShopItem(BinaryReader reader, bool packet = false)
    {
        ItemIndex = reader.ReadInt32();
        GIndex = reader.ReadInt32();
        Info = new ItemInfo(reader);
        GoldPrice = reader.ReadUInt32();
        CreditPrice = reader.ReadUInt32();
        Count = reader.ReadUInt32();
        Class = reader.ReadString();
        Category = reader.ReadString();
        Stock = reader.ReadInt32();
        iStock = reader.ReadBoolean();
        Deal = reader.ReadBoolean();
        TopItem = reader.ReadBoolean();
        Date = DateTime.FromBinary(reader.ReadInt64());
        CanBuyGold = reader.ReadBoolean();
        CanBuyCredit = reader.ReadBoolean();
    }

    public void Save(BinaryWriter writer, bool packet = false)
    {
        writer.Write(ItemIndex);
        writer.Write(GIndex);
        if (packet) Info.Save(writer);
        writer.Write(GoldPrice);
        writer.Write(CreditPrice);
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
    }

    public override string ToString()
    {
        return string.Format("{0}: {1}", GIndex, Info != null ? Info.Name : "NONE");
    }

}


#region Crafting Structure

public class PlayerRecipes
{
    public byte Recipe;
    public bool Learnt;
    public long CurrentCraftTime;
    public bool Finishedcraft;
    public bool Notified;
    public bool Collected;
    public bool InProgress;
    public PlayerRecipes()
    {

    }

    public PlayerRecipes(BinaryReader reader, int version = int.MaxValue)
    {
        Recipe = reader.ReadByte();
        Learnt = reader.ReadBoolean();
        if (version > 88)
        {

            CurrentCraftTime = reader.ReadInt64();
            Finishedcraft = reader.ReadBoolean();
            Notified = reader.ReadBoolean();
            Collected = reader.ReadBoolean();
            InProgress = reader.ReadBoolean();
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Recipe);
        writer.Write(Learnt);
        writer.Write(CurrentCraftTime);
        writer.Write(Finishedcraft);
        writer.Write(Notified);
        writer.Write(Collected);
        writer.Write(InProgress);
    }
}

/// <summary>
/// Under construction
/// </summary>
public class CraftItem
{
    public byte Recipie;
    public string Category = string.Empty;
    public string RecipeName = string.Empty;
    public List<CraftItemRequirement> Requirments = new List<CraftItemRequirement>();
    public ItemInfo ItemResult;
    public uint GoldRequired;
    public ushort LevelRequired;
    public MirClass RequiredClass;
    
    public byte RequiredTimeDay;
    public byte RequiredTimeHour;
    public byte RequiredTimeMinute;

    public byte CraftSuccessRate = 100;


    public CraftItem()
    {

    }
    
    public CraftItem(BinaryReader reader, int version = int.MaxValue)
    {
        Recipie = reader.ReadByte();
        RecipeName = reader.ReadString();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
            Requirments.Add(new CraftItemRequirement(reader,version));

        bool validItem = reader.ReadBoolean();
        if (validItem)
            ItemResult = new ItemInfo(reader, version);
        GoldRequired = reader.ReadUInt32();
        LevelRequired = reader.ReadUInt16();
        RequiredClass = (MirClass)reader.ReadByte();
        RequiredTimeDay = reader.ReadByte();
        RequiredTimeHour = reader.ReadByte();
        RequiredTimeMinute = reader.ReadByte();
        CraftSuccessRate = reader.ReadByte();
        if (version > 128)
            Category = reader.ReadString();
        
    }

    public void Save(BinaryWriter writer, bool packet = false)
    {
        writer.Write(Recipie);
        writer.Write(RecipeName);
        writer.Write(Requirments.Count);
        for (int i = 0; i < Requirments.Count; i++)
            Requirments[i].Save(writer);
        bool validItem = ItemResult != null ? true : false;
        writer.Write(validItem);
        if (validItem)
            ItemResult.Save(writer);
       
        writer.Write(GoldRequired);
        writer.Write(LevelRequired);
        writer.Write((byte)RequiredClass);
        
        writer.Write(RequiredTimeDay);
        writer.Write(RequiredTimeHour);
        writer.Write(RequiredTimeMinute);
        writer.Write(CraftSuccessRate);
        writer.Write(Category);
    }

    public override string ToString()
    {
        return string.Format("{0}: {1}", Recipie, RecipeName);
    }
}

public class CraftItemRequirement
{
    public ItemInfo Item;
    public int Amount;
    public int ItemIndex;
    public CraftItemRequirement() { }

    public CraftItemRequirement(BinaryReader reader, int version = int.MaxValue)
    {
        ItemIndex = reader.ReadInt32();

        Amount = reader.ReadInt32();
    }

    public void Save(BinaryWriter writer)
    {
        /*
        bool validItem = Item != null ? true : false;
        writer.Write(validItem);//oopsie
        if (validItem)
            Item.Save(writer);
        */
        writer.Write(ItemIndex);
        writer.Write(Amount);
    }
    public override string ToString()
    {
        return string.Format("{0}: {1}", ItemIndex, Amount);
    }
}
#endregion

#region Awaking
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

    public AwakeType type;
    List<byte> listAwake = new List<byte>();

    public Awake(BinaryReader reader)
    {
        type = (AwakeType)reader.ReadByte();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            listAwake.Add(reader.ReadByte());
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)type);
        writer.Write(listAwake.Count);
        foreach (byte value in listAwake)
        {
            writer.Write(value);
        }
    }

    public Awake()
    {
        type = AwakeType.None;
    }

    public bool IsMaxLevel() { return listAwake.Count == Awake.MaxAwakeLevel; }

    public int AwakeLevel => listAwake.Count;

    public byte AwakeValue
    {
        get
        {
            byte total = 0;

            foreach (byte value in listAwake)
            {
                total += value;
            }

            return total;
        }
    }

    public bool CheckAwakening(UserItem item, AwakeType type)
    {
        if (item.Info.CanAwakening != true) return false;

        if (item.Info.Grade == ItemGrade.None) return false;

        if (IsMaxLevel()) return false;

        if (this.type == AwakeType.None)
        {
            if (item.Info.Type == ItemType.Weapon)
            {
                if (type == AwakeType.DC ||
                    type == AwakeType.MC ||
                    type == AwakeType.SC)
                {
                    this.type = type;
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
                    this.type = type;
                    return true;
                }
                else
                    return false;
            }
            else if (item.Info.Type == ItemType.Armour)
            {
                if (type == AwakeType.HPMP)
                {
                    this.type = type;
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
            if (this.type == type)
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
            isHit = makeHit(1, out int idx);
            return 0;
        }
    }

    public int RemoveAwake()
    {
        if (listAwake.Count > 0)
        {
            listAwake.Remove(listAwake[listAwake.Count - 1]);

            if (listAwake.Count == 0)
                type = AwakeType.None;

            return 1;
        }
        else
        {
            type = AwakeType.None;
            return 0;
        }
    }

    public int GetAwakeLevelValue(int i) { return listAwake[i]; }

    public byte DC => (type == AwakeType.DC ? AwakeValue : (byte)0); public byte getMC() { return (type == AwakeType.MC ? AwakeValue: (byte)0); }
    public byte getSC() { return (type == AwakeType.SC ? AwakeValue: (byte)0); }
    public byte getAC() { return (type == AwakeType.AC ? AwakeValue: (byte)0); }
    public byte getMAC() { return (type == AwakeType.MAC ? AwakeValue: (byte)0); }
    public byte getHPMP() { return (type == AwakeType.HPMP ? AwakeValue: (byte)0); }

    private bool[] makeHit(int maxValue, out int makeValue)
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
        int maxValue = (AwakeChanceMax[(int)item.Info.Grade - 1] < AwakeChanceMin) ? AwakeChanceMin : AwakeChanceMax[(int)item.Info.Grade - 1];


        bool[] returnValue = makeHit(maxValue, out int result);

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
#endregion

public class ClientMagic
{
    public Spell Spell;
    public byte BaseCost, LevelCost, Icon;
    public byte Level1, Level2, Level3 , Level4 , Level5;
    public ushort Need1, Need2, Need3 , Need4 , Need5;

    public byte Level, Key, Range;
    public ushort Experience;

    public bool IsTempSpell;
    public long CastTime, Delay;

    public bool HumUpEnable;
    public bool OverrideHumUp;
    public ushort PowerBase, PowerBonus;
    public ushort MPowerBase, MPowerBonus;
    public float MultiplierBase = 1.0f, MultiplierBonus;
    public ushort PvPPowerBase, PvPPowerBonus;
    public ushort PvPMPowerBase, PvPMPowerBonus;
    public float PvPMultiplierBase = 1.0f, PvPMultiplierBonus;
    public ClientMagic()
    {
    }

    public ClientMagic(BinaryReader reader, int version = int.MaxValue)
    {
        Spell = (Spell)reader.ReadByte();

        BaseCost = reader.ReadByte();
        LevelCost = reader.ReadByte();
        Icon = reader.ReadByte();
        Level1 = reader.ReadByte();
        Level2 = reader.ReadByte();
        Level3 = reader.ReadByte();
        Level4 = reader.ReadByte();
        Level5 = reader.ReadByte();
        Need1 = reader.ReadUInt16();
        Need2 = reader.ReadUInt16();
        Need3 = reader.ReadUInt16();
        Need4 = reader.ReadUInt16();
        Need5 = reader.ReadUInt16();

        HumUpEnable = reader.ReadBoolean();

        Level = reader.ReadByte();
        Key = reader.ReadByte();
        Experience = reader.ReadUInt16();

        Delay = reader.ReadInt64();

        Range = reader.ReadByte();
        CastTime = reader.ReadInt64();
        OverrideHumUp = reader.ReadBoolean();
        PowerBase = reader.ReadUInt16();
        PowerBonus = reader.ReadUInt16();
        MPowerBase = reader.ReadUInt16();
        MPowerBonus = reader.ReadUInt16();
        MultiplierBase = reader.ReadSingle();
        MultiplierBonus = reader.ReadSingle();
        PvPMPowerBase = reader.ReadUInt16();
        PvPMPowerBonus = reader.ReadUInt16();
        PvPMultiplierBase = reader.ReadSingle();
        PvPMultiplierBonus = reader.ReadSingle();
        PvPPowerBase = reader.ReadUInt16();
        PvPPowerBonus = reader.ReadUInt16();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)Spell);

        writer.Write(BaseCost);
        writer.Write(LevelCost);
        writer.Write(Icon);
        writer.Write(Level1);
        writer.Write(Level2);
        writer.Write(Level3);
        writer.Write(Level4);
        writer.Write(Level5);
        writer.Write(Need1);
        writer.Write(Need2);
        writer.Write(Need3);
        writer.Write(Need4);
        writer.Write(Need5);

        writer.Write(HumUpEnable);

        writer.Write(Level);
        writer.Write(Key);
        writer.Write(Experience);

        writer.Write(Delay);

        writer.Write(Range);
        writer.Write(CastTime);
        writer.Write(OverrideHumUp);
        writer.Write(PowerBase);
        writer.Write(PowerBonus);
        writer.Write(MPowerBase);
        writer.Write(MPowerBonus);
        writer.Write(MultiplierBase);
        writer.Write(MultiplierBonus);
        writer.Write(PvPMPowerBase);
        writer.Write(PvPMPowerBonus);
        writer.Write(PvPMultiplierBase);
        writer.Write(PvPMultiplierBonus);
        writer.Write(PvPPowerBase);
        writer.Write(PvPPowerBonus);
    }
   
}

public class ClientAuction
{
    public ulong AuctionID;
    public UserItem Item;
    public string Seller = string.Empty;
    public uint Price , ggPrice;
    public DateTime ConsignmentDate;

    public ClientAuction()
    {
        
    }
    public ClientAuction(BinaryReader reader)
    {
        AuctionID = reader.ReadUInt64();
        Item = new UserItem(reader);
        Seller = reader.ReadString();
        Price = reader.ReadUInt32();
        ggPrice = reader.ReadUInt32();
        ConsignmentDate = DateTime.FromBinary(reader.ReadInt64());
    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(AuctionID);
        Item.Save(writer);
        writer.Write(Seller);
        writer.Write(Price);
        writer.Write(ggPrice);
        writer.Write(ConsignmentDate.ToBinary());
    }
}

public class RobotInfo
{
    public string Command;
    public sbyte _Day, _Hour, _Min;

    public static RobotInfo FromLine(string s)
    {
        string[] parts = s.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

        RobotInfo RobotCommand = new RobotInfo();

        if (parts.Length < 4) return null;

        if (!sbyte.TryParse(parts[0], out RobotCommand._Day)) return null; // Day of week 0 -6

        if (!sbyte.TryParse(parts[1], out RobotCommand._Hour)) return null; // Hour 0 - 23

        if (!sbyte.TryParse(parts[2], out RobotCommand._Min)) return null; // Minute 0-59

        if (RobotCommand._Day < -1 || RobotCommand._Day > 6) return null;
        if (RobotCommand._Hour < -1 || RobotCommand._Hour > 23) return null;
        if (RobotCommand._Min < -1 || RobotCommand._Min > 59) return null;



        RobotCommand.Command = parts[3];


        return RobotCommand;
    }

    public RobotInfo()
    {
    }


}

public class ClientAvailableQuests
{
    public int Index;
    public string Name;
    public string Group;
    public int MinLevel, MaxLevel;
    

    public ClientAvailableQuests() { }
    public ClientAvailableQuests(BinaryReader reader)
    {
        Index = reader.ReadInt32();
        Name = reader.ReadString();
        Group = reader.ReadString();
        MinLevel = reader.ReadInt32();
        MaxLevel = reader.ReadInt32();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        writer.Write(Name);
        writer.Write(Group);
        writer.Write(MinLevel);
        writer.Write(MaxLevel);
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

    public bool Hero;

    public bool percentageExp;
    public bool autoComplete;
    public uint RewardGold;
    public uint RewardExp;
    public uint RewardCredit;
    public uint ItemEXPReward;
    public List<QuestItemReward> RewardsFixedItem = new List<QuestItemReward>();
    public List<QuestItemReward> RewardsSelectItem = new List<QuestItemReward>();
    public List<BuffReward> BuffRewards = new List<BuffReward>();

    public int time;

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
        RewardGold = reader.ReadUInt32();
        RewardExp = reader.ReadUInt32();
        RewardCredit = reader.ReadUInt32();

        count = reader.ReadInt32();

        for (int i = 0; i < count; i++ )
            RewardsFixedItem.Add(new QuestItemReward(reader));

        count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            RewardsSelectItem.Add(new QuestItemReward(reader));

        count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            BuffRewards.Add(new BuffReward(reader));

        FinishNPCIndex = reader.ReadUInt32();
        percentageExp = reader.ReadBoolean();
        autoComplete = reader.ReadBoolean();

        time = reader.ReadInt32();
        Hero = reader.ReadBoolean();
        ItemEXPReward = reader.ReadUInt32();
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
        writer.Write(RewardGold);
        writer.Write(RewardExp);
        writer.Write(RewardCredit);

        writer.Write(RewardsFixedItem.Count);

        for (int i = 0; i < RewardsFixedItem.Count; i++)
            RewardsFixedItem[i].Save(writer);

        writer.Write(RewardsSelectItem.Count);

        for (int i = 0; i < RewardsSelectItem.Count; i++)
            RewardsSelectItem[i].Save(writer);

        writer.Write(BuffRewards.Count);

        for (int i = 0; i < BuffRewards.Count; i++)
            BuffRewards[i].Save(writer);

        writer.Write(FinishNPCIndex);
        writer.Write(percentageExp);
        writer.Write(autoComplete);

        writer.Write(time);
        writer.Write(Hero);
        writer.Write(ItemEXPReward);
    }

    public QuestIcon GetQuestIcon(bool taken = false, bool completed = false)
    {
        QuestIcon icon = QuestIcon.None;

        switch (Type)
        {
            case QuestType.General:
            case QuestType.Repeatable:
                if (completed)
                    icon = Hero ? QuestIcon.QuestionRed : QuestIcon.QuestionYellow;
                else if (taken)
                    icon = QuestIcon.QuestionWhite;
                else
                    icon = Hero ? QuestIcon.ExclamationRed : QuestIcon.ExclamationYellow;
                break;
            case QuestType.Daily:
                if (completed)
                    icon = Hero ? QuestIcon.QuestionDarkBlue: QuestIcon.QuestionBlue;
                else if (taken)
                    icon = QuestIcon.QuestionWhite;
                else
                    icon = Hero ? QuestIcon.ExclamationDarkBlue : QuestIcon.ExclamationBlue;
                break;
            case QuestType.Story:
                if (completed)
                    icon = Hero ? QuestIcon.QuestionOrange : QuestIcon.QuestionGreen;
                else if (taken)
                    icon = QuestIcon.QuestionWhite;
                else
                    icon = Hero ? QuestIcon.ExclamationOrange : QuestIcon.ExclamationGreen;
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

    public int EndTime;

    public QuestIcon Icon
    {
        get 
        {
            return QuestInfo.GetQuestIcon(Taken, Completed); 
        }
    }

    public ClientQuestProgress(){ }

    public ClientQuestProgress(BinaryReader reader)
    {
        Id = reader.ReadInt32();

        int count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            TaskList.Add(reader.ReadString());

        Taken = reader.ReadBoolean();
        Completed = reader.ReadBoolean();
        New = reader.ReadBoolean();
        EndTime = reader.ReadInt32();
        bool gotInfo = reader.ReadBoolean();
        if (gotInfo)
            QuestInfo = new ClientQuestInfo(reader);
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
        writer.Write(EndTime);
        if (QuestInfo != null)
        {
            writer.Write(true);
            QuestInfo.Save(writer);
        }
        else
            writer.Write(false);
    }
}

public class BuffReward
{
    public BuffType buff;
    public int amount;
    public long time;

    public BuffReward() { }

    public BuffReward(BinaryReader reader)
    {
        buff = (BuffType)reader.ReadByte();
        amount = reader.ReadInt32();
        time = reader.ReadInt64();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)buff);
        writer.Write(amount);
        writer.Write(time);
    }
}

public class QuestItemReward
{
    public ItemInfo Item;
    public uint Count = 1;

    public QuestItemReward() { }

    public QuestItemReward(BinaryReader reader)
    {
        Item = new ItemInfo(reader);
        Count = reader.ReadUInt32();
    }

    public void Save(BinaryWriter writer)
    {
        Item.Save(writer);
        writer.Write(Count);
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

        writer.Write(Gold);
        writer.Write(Items.Count);

        for (int i = 0; i < Items.Count; i++)
            Items[i].Save(writer);
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


public enum IntelligentCreaturePickupMode : byte
{
    Automatic = 0,
    SemiAutomatic = 1,
}

public class IntelligentCreatureRules
{
    public int MinimalFullness = 1;

    public bool MousePickupEnabled = false;
    public int MousePickupRange = 0;
    public bool AutoPickupEnabled = false;
    public int AutoPickupRange = 0;
    public bool SemiAutoPickupEnabled = false;
    public int SemiAutoPickupRange = 0;

    public bool CanProduceBlackStone = false;

    public string Info = "";
    public string Info1 = "";
    public string Info2 = "";

    public IntelligentCreatureRules()
    {
    }

    public IntelligentCreatureRules(BinaryReader reader)
    {
        MinimalFullness = reader.ReadInt32();
        MousePickupEnabled = reader.ReadBoolean();
        MousePickupRange = reader.ReadInt32();
        AutoPickupEnabled = reader.ReadBoolean();
        AutoPickupRange = reader.ReadInt32();
        SemiAutoPickupEnabled = reader.ReadBoolean();
        SemiAutoPickupRange = reader.ReadInt32();

        CanProduceBlackStone = reader.ReadBoolean();

        Info = reader.ReadString();
        Info1 = reader.ReadString();
        Info2 = reader.ReadString();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(MinimalFullness);
        writer.Write(MousePickupEnabled);
        writer.Write(MousePickupRange);
        writer.Write(AutoPickupEnabled);
        writer.Write(AutoPickupRange);
        writer.Write(SemiAutoPickupEnabled);
        writer.Write(SemiAutoPickupRange);

        writer.Write(CanProduceBlackStone);

        writer.Write(Info);
        writer.Write(Info1);
        writer.Write(Info2);
    }
}

public class IntelligentCreatureItemFilter
{
    public bool PetPickupAll = true;
    public bool PetPickupGold = false;
    public bool PetPickupWeapons = false;
    public bool PetPickupArmours = false;
    public bool PetPickupHelmets = false;
    public bool PetPickupBoots = false;
    public bool PetPickupBelts = false;
    public bool PetPickupAccessories = false;
    public bool PetPickupOthers = false;

    public ItemGrade PickupGrade = ItemGrade.None;

    public IntelligentCreatureItemFilter()
    {
    }

    public void SetItemFilter(int idx)
    {
        switch (idx)
        {
            case 0://all items
                PetPickupAll = true;
                PetPickupGold = false;
                PetPickupWeapons = false;
                PetPickupArmours = false;
                PetPickupHelmets = false;
                PetPickupBoots = false;
                PetPickupBelts = false;
                PetPickupAccessories = false;
                PetPickupOthers = false;
                break;
            case 1://gold
                PetPickupAll = false;
                PetPickupGold = !PetPickupGold;
                break;
            case 2://weapons
                PetPickupAll = false;
                PetPickupWeapons = !PetPickupWeapons;
                break;
            case 3://armours
                PetPickupAll = false;
                PetPickupArmours = !PetPickupArmours;
                break;
            case 4://helmets
                PetPickupAll = false;
                PetPickupHelmets = !PetPickupHelmets;
                break;
            case 5://boots
                PetPickupAll = false;
                PetPickupBoots = !PetPickupBoots;
                break;
            case 6://belts
                PetPickupAll = false;
                PetPickupBelts = !PetPickupBelts;
                break;
            case 7://jewelry
                PetPickupAll = false;
                PetPickupAccessories = !PetPickupAccessories;
                break;
            case 8://others
                PetPickupAll = false;
                PetPickupOthers = !PetPickupOthers;
                break;
        }
        if (PetPickupGold && PetPickupWeapons && PetPickupArmours && PetPickupHelmets && PetPickupBoots && PetPickupBelts && PetPickupAccessories && PetPickupOthers)
        {
            PetPickupAll = true;
            PetPickupGold = false;
            PetPickupWeapons = false;
            PetPickupArmours = false;
            PetPickupHelmets = false;
            PetPickupBoots = false;
            PetPickupBelts = false;
            PetPickupAccessories = false;
            PetPickupOthers = false;
        }
        else
            if (!PetPickupGold && !PetPickupWeapons && !PetPickupArmours && !PetPickupHelmets && !PetPickupBoots && !PetPickupBelts && !PetPickupAccessories && !PetPickupOthers)
            {
                PetPickupAll = true;
            }
    }

    public IntelligentCreatureItemFilter(BinaryReader reader)
    {
        PetPickupAll = reader.ReadBoolean();
        PetPickupGold = reader.ReadBoolean();
        PetPickupWeapons = reader.ReadBoolean();
        PetPickupArmours = reader.ReadBoolean();
        PetPickupHelmets = reader.ReadBoolean();
        PetPickupBoots = reader.ReadBoolean();
        PetPickupBelts = reader.ReadBoolean();
        PetPickupAccessories = reader.ReadBoolean();
        PetPickupOthers = reader.ReadBoolean();
        //PickupGrade = (ItemGrade)reader.ReadByte();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(PetPickupAll);
        writer.Write(PetPickupGold);
        writer.Write(PetPickupWeapons);
        writer.Write(PetPickupArmours);
        writer.Write(PetPickupHelmets);
        writer.Write(PetPickupBoots);
        writer.Write(PetPickupBelts);
        writer.Write(PetPickupAccessories);
        writer.Write(PetPickupOthers);
        //writer.Write((byte)PickupGrade);
    }
}

public class ClientIntelligentCreature
{
    public IntelligentCreatureType PetType;
    public int Icon;

    public string CustomName;
    public int Fullness;
    public int SlotIndex;
    public long ExpireTime;//in days
    public long BlackstoneTime;
    public long MaintainFoodTime;

    public IntelligentCreaturePickupMode petMode = IntelligentCreaturePickupMode.SemiAutomatic;

    public IntelligentCreatureRules CreatureRules;
    public IntelligentCreatureItemFilter Filter;


    public ClientIntelligentCreature()
    {
    }

    public ClientIntelligentCreature(BinaryReader reader)
    {
        PetType = (IntelligentCreatureType)reader.ReadByte();
        Icon = reader.ReadInt32();

        CustomName = reader.ReadString();
        Fullness = reader.ReadInt32();
        SlotIndex = reader.ReadInt32();
        ExpireTime = reader.ReadInt64();
        BlackstoneTime = reader.ReadInt64();

        petMode = (IntelligentCreaturePickupMode)reader.ReadByte();

        CreatureRules = new IntelligentCreatureRules(reader);
        Filter = new IntelligentCreatureItemFilter(reader)
        {
            PickupGrade = (ItemGrade)reader.ReadByte()
        };
        MaintainFoodTime = reader.ReadInt64();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)PetType);
        writer.Write(Icon);

        writer.Write(CustomName);
        writer.Write(Fullness);
        writer.Write(SlotIndex);
        writer.Write(ExpireTime);
        writer.Write(BlackstoneTime);

        writer.Write((byte)petMode);

        CreatureRules.Save(writer);
        Filter.Save(writer);
        writer.Write((byte)Filter.PickupGrade);
        writer.Write(MaintainFoodTime);
    }
}


public abstract class Packet
{
    public static bool IsServer;

    public abstract short Index { get; }

    public static Packet ReceivePacket(byte[] rawBytes, out byte[] extra)
    {
        extra = rawBytes;

        Packet p;

        if (rawBytes.Length < 4) return null; //| 2Bytes: Packet Size | 2Bytes: Packet ID |

        int length = (rawBytes[1] << 8) + rawBytes[0];

        if (length > rawBytes.Length || length < 2) return null;

        using (MemoryStream stream = new MemoryStream(rawBytes, 2, length - 2))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            try
            {
                short id = reader.ReadInt16();

                p = IsServer ? GetClientPacket(id) : GetServerPacket(id);
                if (p == null)
                    return null;

                p.ReadPacket(reader);
            }
            catch (Exception)
            {
                return null;
                //return new C.Disconnect();
            }
        }

        extra = new byte[rawBytes.Length - length];
        Buffer.BlockCopy(rawBytes, length, extra, 0, rawBytes.Length - length);

        return p;
    }

    public IEnumerable<byte> GetPacketBytes()
    {
        if (Index < 0) return new byte[0];

        byte[] data;

        using (MemoryStream stream = new MemoryStream())
        {
            stream.SetLength(2);
            stream.Seek(2, SeekOrigin.Begin);
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(Index);
                WritePacket(writer);
                stream.Seek(0, SeekOrigin.Begin);
                writer.Write((short)stream.Length);
                stream.Seek(0, SeekOrigin.Begin);

                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }
        }

        return data;
    }

    protected abstract void ReadPacket(BinaryReader reader);
    protected abstract void WritePacket(BinaryWriter writer);

    private static Packet GetClientPacket(short index)
    {
        switch (index)
        {
            case (short)ClientPacketIds.ClientVersion:
                return new C.ClientVersion();
            case (short)ClientPacketIds.Disconnect:
                return new C.Disconnect();
            case (short)ClientPacketIds.KeepAlive:
                return new C.KeepAlive();
            case (short)ClientPacketIds.NewAccount:
                return new C.NewAccount();
            case (short)ClientPacketIds.ChangePassword:
                return new C.ChangePassword();
            case (short)ClientPacketIds.Login:
                return new C.Login();
            case (short)ClientPacketIds.NewCharacter:
                return new C.NewCharacter();
            case (short)ClientPacketIds.DeleteCharacter:
                return new C.DeleteCharacter();
            case (short)ClientPacketIds.StartGame:
                return new C.StartGame();
            case (short)ClientPacketIds.LogOut:
                return new C.LogOut();
            case (short)ClientPacketIds.StashHero:
                return new C.StashHero();
            case (short)ClientPacketIds.Turn:
                return new C.Turn();
            case (short)ClientPacketIds.Walk:
                return new C.Walk();
            case (short)ClientPacketIds.Run:
                return new C.Run();
            case (short)ClientPacketIds.Chat:
                return new C.Chat();
            case (short)ClientPacketIds.MoveItem:
                return new C.MoveItem();
            case (short)ClientPacketIds.StoreItem:
                return new C.StoreItem();
            case (short)ClientPacketIds.InventoryToHeroInventory:
                return new C.InventoryToHeroInventory();
            case (short)ClientPacketIds.TakeBackItem:
                return new C.TakeBackItem();
            case (short)ClientPacketIds.HeroInventoryToInventory:
                return new C.HeroInventoryToInventory();
            case (short)ClientPacketIds.MergeItem:
                return new C.MergeItem();
            case (short)ClientPacketIds.EquipItem:
                return new C.EquipItem();
            case (short)ClientPacketIds.HeroEquipItem:
                return new C.HeroEquipItem();
            case (short)ClientPacketIds.RemoveItem:
                return new C.RemoveItem();
            case (short)ClientPacketIds.HeroRemoveItem:
                return new C.HeroRemoveItem();
            case (short)ClientPacketIds.RemoveSlotItem:
                return new C.RemoveSlotItem();
            case (short)ClientPacketIds.SplitItem:
                return new C.SplitItem();
            case (short)ClientPacketIds.UseItem:
                return new C.UseItem();
            case (short)ClientPacketIds.HeroUseItem:
                return new C.HeroUseItem();
            case (short)ClientPacketIds.DropItem:
                return new C.DropItem();
            case (short)ClientPacketIds.DepositRefineItem:
                return new C.DepositRefineItem();
            case (short)ClientPacketIds.RetrieveRefineItem:
                return new C.RetrieveRefineItem();
            case (short)ClientPacketIds.RefineCancel:
                return new C.RefineCancel();
            case (short)ClientPacketIds.RefineItem:
                return new C.RefineItem();
            case (short)ClientPacketIds.CheckRefine:
                return new C.CheckRefine();
            case (short)ClientPacketIds.ReplaceWedRing:
                return new C.ReplaceWedRing();
            case (short)ClientPacketIds.DepositTradeItem:
                return new C.DepositTradeItem();
            case (short)ClientPacketIds.RetrieveTradeItem:
                return new C.RetrieveTradeItem();
            case (short)ClientPacketIds.DropGold:
                return new C.DropGold();
            case (short)ClientPacketIds.PickUp:
                return new C.PickUp();
            case (short)ClientPacketIds.Inspect:
                return new C.Inspect();
            case (short)ClientPacketIds.HeroRunTo:
                return new C.HeroRunTo();
            case (short)ClientPacketIds.ChangeAMode:
                return new C.ChangeAMode();
            case (short)ClientPacketIds.ChangePMode:
                return new C.ChangePMode();
            case (short)ClientPacketIds.ChangeHMode:
                return new C.ChangeHMode();
            case (short)ClientPacketIds.ChangeTrade:
                return new C.ChangeTrade();
            case (short)ClientPacketIds.Attack:
                return new C.Attack();
            case (short)ClientPacketIds.RangeAttack:
                return new C.RangeAttack();
            case (short)ClientPacketIds.Harvest:
                return new C.Harvest();
            case (short)ClientPacketIds.CallNPC:
                return new C.CallNPC();
            case (short)ClientPacketIds.TalkMonsterNPC:
                return new C.TalkMonsterNPC();
            case (short)ClientPacketIds.BuyItem:
                return new C.BuyItem();
            case (short)ClientPacketIds.SellItem:
                return new C.SellItem();
            case (short)ClientPacketIds.RepairItem:
                return new C.RepairItem();
            case (short)ClientPacketIds.BuyItemBack:
                return new C.BuyItemBack();
            case (short)ClientPacketIds.SRepairItem:
                return new C.SRepairItem();
            case (short)ClientPacketIds.MagicKey:
                return new C.MagicKey();
            case (short)ClientPacketIds.HeroMagicKey:
                return new C.HeroMagicKey();
            case (short)ClientPacketIds.AutoPotUpdate:
                return new C.AutoPotUpdate();
            case (short)ClientPacketIds.Magic:
                return new C.Magic();
            case (short)ClientPacketIds.SwitchGroup:
                return new C.SwitchGroup();
            case (short)ClientPacketIds.AddMember:
                return new C.AddMember();
            case (short)ClientPacketIds.DellMember:
                return new C.DelMember();
            case (short)ClientPacketIds.GroupInvite:
                return new C.GroupInvite();
            case (short)ClientPacketIds.TownRevive:
                return new C.TownRevive();
            case (short)ClientPacketIds.SpellToggle:
                return new C.SpellToggle();
            case (short)ClientPacketIds.ConsignItem:
                return new C.ConsignItem();
            case (short)ClientPacketIds.MarketSearch:
                return new C.MarketSearch();
            case (short)ClientPacketIds.MarketRefresh:
                return new C.MarketRefresh();
            case (short)ClientPacketIds.MarketPage:
                return new C.MarketPage();
            case (short)ClientPacketIds.GuildTerritoryPage:
                return new C.GuildTerritoryPage();
            case (short)ClientPacketIds.MarketBuy:
                return new C.MarketBuy();
            case (short)ClientPacketIds.MarketGetBack:
                return new C.MarketGetBack();
            case (short)ClientPacketIds.RequestUserName:
                return new C.RequestUserName();
            case (short)ClientPacketIds.RequestChatItem:
                return new C.RequestChatItem();
            case (short)ClientPacketIds.EditGuildMember:
                return new C.EditGuildMember();
            case (short)ClientPacketIds.EditGuildNotice:
                return new C.EditGuildNotice();
            case (short)ClientPacketIds.GuildInvite:
                return new C.GuildInvite();
            case (short)ClientPacketIds.GuildNameReturn:
                return new C.GuildNameReturn();
            case (short)ClientPacketIds.RequestGuildInfo:
                return new C.RequestGuildInfo();
            case (short)ClientPacketIds.GuildStorageGoldChange:
                return new C.GuildStorageGoldChange();
            case (short)ClientPacketIds.GuildStorageItemChange:
                return new C.GuildStorageItemChange();
            case (short)ClientPacketIds.GuildWarReturn:
                return new C.GuildWarReturn();
            case (short)ClientPacketIds.MarriageRequest:
                return new C.MarriageRequest();
            case (short)ClientPacketIds.MarriageReply:
                return new C.MarriageReply();
            case (short)ClientPacketIds.ChangeMarriage:
                return new C.ChangeMarriage();
            case (short)ClientPacketIds.DivorceRequest:
                return new C.DivorceRequest();
            case (short)ClientPacketIds.DivorceReply:
                return new C.DivorceReply();
            case (short)ClientPacketIds.AddMentor:
                return new C.AddMentor();
            case (short)ClientPacketIds.MentorReply:
                return new C.MentorReply();
            case (short)ClientPacketIds.AllowMentor:
                return new C.AllowMentor();
            case (short)ClientPacketIds.CancelMentor:
                return new C.CancelMentor();
            case (short)ClientPacketIds.TradeRequest:
                return new C.TradeRequest();
            case (short)ClientPacketIds.TradeReply:
                return new C.TradeReply();
            case (short)ClientPacketIds.TradeGold:
                return new C.TradeGold();
            case (short)ClientPacketIds.TradeConfirm:
                return new C.TradeConfirm();
            case (short)ClientPacketIds.TradeCancel:
                return new C.TradeCancel();
            case (short)ClientPacketIds.EquipSlotItem:
                return new C.EquipSlotItem();
            case (short)ClientPacketIds.FishingCast:
                return new C.FishingCast();
            case (short)ClientPacketIds.FishingChangeAutocast:
                return new C.FishingChangeAutocast();
            case (short)ClientPacketIds.AcceptQuest:
                return new C.AcceptQuest();
            case (short)ClientPacketIds.FinishQuest:
                return new C.FinishQuest();
            case (short)ClientPacketIds.AbandonQuest:
                return new C.AbandonQuest();
            case (short)ClientPacketIds.ShareQuest:
                return new C.ShareQuest();
            case (short)ClientPacketIds.AcceptReincarnation:
                return new C.AcceptReincarnation();
            case (short)ClientPacketIds.CancelReincarnation:
                return new C.CancelReincarnation();
            case (short)ClientPacketIds.CombineItem:
                return new C.CombineItem();
            case (short)ClientPacketIds.SetConcentration:
                return new C.SetConcentration();
			case (short)ClientPacketIds.AwakeningNeedMaterials:
                return new C.AwakeningNeedMaterials();
            case (short)ClientPacketIds.AwakeningLockedItem:
                return new C.AwakeningLockedItem();
            case (short)ClientPacketIds.Awakening:
                return new C.Awakening();
            case (short)ClientPacketIds.DisassembleItem:
                return new C.DisassembleItem();
            case (short)ClientPacketIds.DowngradeAwakening:
                return new C.DowngradeAwakening();
            case (short)ClientPacketIds.ResetAddedItem:
                return new C.ResetAddedItem();
            case (short)ClientPacketIds.SendMail:
                return new C.SendMail();
            case (short)ClientPacketIds.ReadMail:
                return new C.ReadMail();
            case (short)ClientPacketIds.CollectParcel:
                return new C.CollectParcel();
            case (short)ClientPacketIds.DeleteMail:
                return new C.DeleteMail();
            case (short)ClientPacketIds.LockMail:
                return new C.LockMail();
            case (short)ClientPacketIds.MailLockedItem:
                return new C.MailLockedItem();
            case (short)ClientPacketIds.MailCost:
                return new C.MailCost();
            case (short)ClientPacketIds.UpdateIntelligentCreature://IntelligentCreature
                return new C.UpdateIntelligentCreature();
            case (short)ClientPacketIds.IntelligentCreaturePickup://IntelligentCreature
                return new C.IntelligentCreaturePickup();
            case (short)ClientPacketIds.AddFriend:
                return new C.AddFriend();
            case (short)ClientPacketIds.RemoveFriend:
                return new C.RemoveFriend();
            case (short)ClientPacketIds.RefreshFriends:
                return new C.RefreshFriends();
            case (short)ClientPacketIds.AddMemo:
                return new C.AddMemo();
            case (short)ClientPacketIds.GuildBuffUpdate:
                return new C.GuildBuffUpdate();
            case (short)ClientPacketIds.GameshopBuy:
                return new C.GameshopBuy();
            case (short)ClientPacketIds.NPCConfirmInput:
                return new C.NPCConfirmInput();
            case (short)ClientPacketIds.ReportIssue:
                return new C.ReportIssue();
            case (short)ClientPacketIds.ComboSpell:
                return new C.ComboSpell();
            case (short)ClientPacketIds.GetRanking:
                return new C.GetRanking();
            case (short)ClientPacketIds.Opendoor:
                return new C.Opendoor();
            case (short)ClientPacketIds.GetCrafts:
                return new C.GetRecipes();
            case (short)ClientPacketIds.RequestCraft:
                return new C.RequestCraft();
            case (short)ClientPacketIds.CollectCraft:
                return new C.CollectCraft();
            case (short)ClientPacketIds.CreateHero:
                return new C.HeroCreation();
            case (short)ClientPacketIds.BuyRecipie:
                return new C.BuyRecipe();
            case (short)ClientPacketIds.RecipieShop:
                return new C.GetRecipeShop();
            case (short)ClientPacketIds.EERequestSocketInfo:
                return new C.RequestSocketInfo();
            case (short)ClientPacketIds.EESocketItem:
                return new C.SocketRuneStone();
            case (short)ClientPacketIds.EERemoveRune:
                return new C.RemoveRune();
            case (short)ClientPacketIds.EEHeroRank:
                return new C.GetHeroRank();
            case (short)ClientPacketIds.HeroUseMagic:
                return new C.HeroUseMagic();
            case (short)ClientPacketIds.ChangeHeroMove:
                return new C.ChangeHeroMoveBehaviour();
            case (short)ClientPacketIds.CheckClientItemFile:
                return new C.CheckItemFileVersion();
            case (short)ClientPacketIds.ExternalToolLogin:
                return new C.ExternalToolLogin();
            case (short)ClientPacketIds.ToolStats:
                return new C.RequestStats();
            case (short)ClientPacketIds.ReportCheat:
                return new C.ReportCheat();
            case (short)ClientPacketIds.AdjustGuildTax:
                return new C.AdjustGuildTax();
            case (short)ClientPacketIds.LMS_Signup:
                return new C.BR_Signup();
            case (short)ClientPacketIds.Get_LMS_BR:
                return new C.Get_BR_Matches();
            default:
                return null;
        }

    }
    public static Packet GetServerPacket(short index)
    {
        switch (index)
        {
            case (short)ServerPacketIds.Connected:
                return new S.Connected();
            case (short)ServerPacketIds.ClientVersion:
                return new S.ClientVersion();
            case (short)ServerPacketIds.Disconnect:
                return new S.Disconnect();
            case (short)ServerPacketIds.KeepAlive:
                return new S.KeepAlive();
            case (short)ServerPacketIds.NewAccount:
                return new S.NewAccount();
            case (short)ServerPacketIds.ChangePassword:
                return new S.ChangePassword();
            case (short)ServerPacketIds.ChangePasswordBanned:
                return new S.ChangePasswordBanned();
            case (short)ServerPacketIds.Login:
                return new S.Login();
            case (short)ServerPacketIds.LoginBanned:
                return new S.LoginBanned();
            case (short)ServerPacketIds.LoginSuccess:
                return new S.LoginSuccess();
            case (short)ServerPacketIds.NewCharacter:
                return new S.NewCharacter();
            case (short)ServerPacketIds.NewCharacterSuccess:
                return new S.NewCharacterSuccess();
            case (short)ServerPacketIds.DeleteCharacter:
                return new S.DeleteCharacter();
            case (short)ServerPacketIds.DeleteCharacterSuccess:
                return new S.DeleteCharacterSuccess();
            case (short)ServerPacketIds.StartGame:
                return new S.StartGame();
            case (short)ServerPacketIds.StartGameBanned:
                return new S.StartGameBanned();
            case (short)ServerPacketIds.StartGameDelay:
                return new S.StartGameDelay();       
            case (short)ServerPacketIds.MapInformation:
                return new S.MapInformation();
            case (short)ServerPacketIds.UserInformation:
                return new S.UserInformation();
            case (short)ServerPacketIds.HeroInformation:
                return new S.HeroInformation();
            case (short)ServerPacketIds.HeroStats:
                return new S.HeroStats();
            case (short)ServerPacketIds.UserLocation:
                return new S.UserLocation();
            case (short)ServerPacketIds.ObjectPlayer:
                return new S.ObjectPlayer();
            case (short)ServerPacketIds.ObjectRemove:
                return new S.ObjectRemove();
            case (short)ServerPacketIds.ObjectTurn:
                return new S.ObjectTurn();
            case (short)ServerPacketIds.ObjectWalk:
                return new S.ObjectWalk();
            case (short)ServerPacketIds.ObjectRun:
                return new S.ObjectRun();
            case (short)ServerPacketIds.Chat:
                return new S.Chat();
            case (short)ServerPacketIds.ObjectChat:
                return new S.ObjectChat();
            case (short)ServerPacketIds.NewItemInfo:
                return new S.NewItemInfo();
            case (short)ServerPacketIds.MoveItem:
                return new S.MoveItem();
            case (short)ServerPacketIds.EquipItem:
                return new S.EquipItem();
            case (short)ServerPacketIds.HeroEquipItem:
                return new S.HeroEquipItem();
            case (short)ServerPacketIds.MergeItem:
                return new S.MergeItem();
            case (short)ServerPacketIds.RemoveItem:
                return new S.RemoveItem();
            case (short)ServerPacketIds.HeroRemoveItem:
                return new S.HeroRemoveItem();
            case (short)ServerPacketIds.RemoveSlotItem:
                return new S.RemoveSlotItem();
            case (short)ServerPacketIds.TakeBackItem:
                return new S.TakeBackItem();
            case (short)ServerPacketIds.HeroInventoryToInventory:
                return new S.HeroInventoryToInventory();
            case (short)ServerPacketIds.StoreItem:
                return new S.StoreItem();
            case (short)ServerPacketIds.InventoryToHeroInventory:
                return new S.InventoryToHeroInventory();
            case (short)ServerPacketIds.DepositRefineItem:
                return new S.DepositRefineItem();
            case (short)ServerPacketIds.RetrieveRefineItem:
                return new S.RetrieveRefineItem();
            case (short)ServerPacketIds.RefineItem:
                return new S.RefineItem();
            case (short)ServerPacketIds.DepositTradeItem:
                return new S.DepositTradeItem();
            case (short)ServerPacketIds.RetrieveTradeItem:
                return new S.RetrieveTradeItem();
            case (short)ServerPacketIds.SplitItem:
                return new S.SplitItem();
            case (short)ServerPacketIds.SplitItem1:
                return new S.SplitItem1();
            case (short)ServerPacketIds.UseItem:
                return new S.UseItem();
            case (short)ServerPacketIds.HeroUseItem:
                return new S.HeroUseItem();
            case (short)ServerPacketIds.DropItem:
                return new S.DropItem();
            case (short)ServerPacketIds.PlayerUpdate:
                return new S.PlayerUpdate();
            case (short)ServerPacketIds.UpdateItemInfo:
                return new S.UpdateItemInfo();
            case (short)ServerPacketIds.RefreshStats:
                return new S.RefreshStats();
            case (short)ServerPacketIds.PlayerInspect:
                return new S.PlayerInspect();
            case (short)ServerPacketIds.LogOutSuccess:
                return new S.LogOutSuccess();
            case (short)ServerPacketIds.LogOutFailed:
                return new S.LogOutFailed();
            case (short)ServerPacketIds.TimeOfDay:
                return new S.TimeOfDay();
            case (short)ServerPacketIds.ChangeAMode:
                return new S.ChangeAMode();
            case (short)ServerPacketIds.ChangePMode:
                return new S.ChangePMode();
            case (short)ServerPacketIds.ChangeHMode:
                return new S.ChangeHMode();
            case (short)ServerPacketIds.ObjectItem:
                return new S.ObjectItem();
            case (short)ServerPacketIds.ObjectGold:
                return new S.ObjectGold();
            case (short)ServerPacketIds.GainedItem:
                return new S.GainedItem();
            case (short)ServerPacketIds.GainedGold:
                return new S.GainedGold();
            case (short)ServerPacketIds.LoseGold:
                return new S.LoseGold();
            case (short)ServerPacketIds.GainedCredit:
                return new S.GainedCredit();
            case (short)ServerPacketIds.LoseCredit:
                return new S.LoseCredit();
            case (short)ServerPacketIds.ObjectMonster:
                return new S.ObjectMonster();
            case (short)ServerPacketIds.ObjectAttack:
                return new S.ObjectAttack();
            case (short)ServerPacketIds.Struck:
                return new S.Struck();
            case (short)ServerPacketIds.DamageIndicator:
                return new S.DamageIndicator();
            case (short)ServerPacketIds.ObjectStruck:
                return new S.ObjectStruck();
            case (short)ServerPacketIds.DuraChanged:
                return new S.DuraChanged();
            case (short)ServerPacketIds.HealthChanged:
                return new S.HealthChanged();
            case (short)ServerPacketIds.HeroHealthChanged:
                return new S.HeroHealthChanged();
            case (short)ServerPacketIds.HeroStatsChanged:
                return new S.HeroStatsChanged();
            case (short)ServerPacketIds.DeleteItem:
                return new S.DeleteItem();
            case (short)ServerPacketIds.Death:
                return new S.Death();
            case (short)ServerPacketIds.ObjectDied:
                return new S.ObjectDied();
            case (short)ServerPacketIds.ColourChanged:
                return new S.ColourChanged();
            case (short)ServerPacketIds.ObjectColourChanged:
                return new S.ObjectColourChanged();
            case (short)ServerPacketIds.ObjectGuildNameChanged:
                return new S.ObjectGuildNameChanged();
            case (short)ServerPacketIds.GainExperience:
                return new S.GainExperience();
            case (short)ServerPacketIds.LevelChanged:
                return new S.LevelChanged();
            case (short)ServerPacketIds.HeroGainExperience:
                return new S.HeroGainExperience();
            case (short)ServerPacketIds.HeroLevelChanged:
                return new S.HeroLevelChanged();
            case (short)ServerPacketIds.ObjectLeveled:
                return new S.ObjectLeveled();
            case (short)ServerPacketIds.ObjectHarvest:
                return new S.ObjectHarvest();
            case (short)ServerPacketIds.ObjectHarvested:
                return new S.ObjectHarvested();
            case (short)ServerPacketIds.ObjectNpc:
                return new S.ObjectNPC();
            case (short)ServerPacketIds.NPCResponse:
                return new S.NPCResponse();
            case (short)ServerPacketIds.ObjectHide:
                return new S.ObjectHide();
            case (short)ServerPacketIds.ObjectShow:
                return new S.ObjectShow();
            case (short)ServerPacketIds.Poisoned:
                return new S.Poisoned();
            case (short)ServerPacketIds.ObjectPoisoned:
                return new S.ObjectPoisoned();
            case (short)ServerPacketIds.MapChanged:
                return new S.MapChanged();
            case (short)ServerPacketIds.ObjectTeleportOut:
                return new S.ObjectTeleportOut();
            case (short)ServerPacketIds.ObjectTeleportIn:
                return new S.ObjectTeleportIn();
            case (short)ServerPacketIds.TeleportIn:
                return new S.TeleportIn();
            case (short)ServerPacketIds.NPCGoods:
                return new S.NPCGoods();
            case (short)ServerPacketIds.NPCSell:
                return new S.NPCSell();
            case (short)ServerPacketIds.NPCRepair:
                return new S.NPCRepair();
            case (short)ServerPacketIds.NPCSRepair: 
                return new S.NPCSRepair();
            case (short)ServerPacketIds.NPCRefine:
                return new S.NPCRefine();
            case (short)ServerPacketIds.NPCCheckRefine:
                return new S.NPCCheckRefine();
            case (short)ServerPacketIds.NPCCollectRefine:
                return new S.NPCCollectRefine();
            case (short)ServerPacketIds.NPCReplaceWedRing:
                return new S.NPCReplaceWedRing();
            case (short)ServerPacketIds.NPCStorage:
                return new S.NPCStorage();
            case (short)ServerPacketIds.SellItem:
                return new S.SellItem();
            case (short)ServerPacketIds.RepairItem:
                return new S.RepairItem();
            case (short)ServerPacketIds.ItemRepaired:
                return new S.ItemRepaired();
            case (short)ServerPacketIds.NewMagic:
                return new S.NewMagic();
            case (short)ServerPacketIds.RefreshMagic:
                return new S.RefreshMagic();
            case (short)ServerPacketIds.HeroNewMagic:
                return new S.HeroNewMagic();
            case (short)ServerPacketIds.MagicLeveled:
                return new S.MagicLeveled();
            case (short)ServerPacketIds.HeroMagicLeveled:
                return new S.HeroMagicLeveled();
            case (short)ServerPacketIds.Magic:
                return new S.Magic();
            case (short)ServerPacketIds.MagicDelay:
                return new S.MagicDelay();
            case (short)ServerPacketIds.MagicCast:
                return new S.MagicCast();
            case (short)ServerPacketIds.ObjectMagic:
                return new S.ObjectMagic();
            case (short)ServerPacketIds.ObjectEffect:
                return new S.ObjectEffect();
            case (short)ServerPacketIds.RangeAttack:
                return new S.RangeAttack();
            case (short)ServerPacketIds.Pushed:
                return new S.Pushed();
            case (short)ServerPacketIds.ObjectPushed:
                return new S.ObjectPushed();
            case (short)ServerPacketIds.ObjectName:
                return new S.ObjectName();
            case (short)ServerPacketIds.UserStorage:
                return new S.UserStorage();
            case (short)ServerPacketIds.SwitchGroup:
                return new S.SwitchGroup();
            case (short)ServerPacketIds.DeleteGroup:
                return new S.DeleteGroup();
            case (short)ServerPacketIds.DeleteMember:
                return new S.DeleteMember();
            case (short)ServerPacketIds.GroupInvite:
                return new S.GroupInvite();
            case (short)ServerPacketIds.AddMember:
                return new S.AddMember();
            case (short)ServerPacketIds.Revived:
                return new S.Revived();
            case (short)ServerPacketIds.ObjectRevived:
                return new S.ObjectRevived();
            case (short)ServerPacketIds.SpellToggle:
                return new S.SpellToggle();
            case (short)ServerPacketIds.ObjectHealth:
                return new S.ObjectHealth();
            case (short)ServerPacketIds.MapEffect:
                return new S.MapEffect();
            case (short)ServerPacketIds.ObjectRangeAttack:
                return new S.ObjectRangeAttack();
            case (short)ServerPacketIds.AddBuff:
                return new S.AddBuff();
            case (short)ServerPacketIds.RemoveBuff:
                return new S.RemoveBuff();
            case (short)ServerPacketIds.ObjectHidden:
                return new S.ObjectHidden();
            case (short)ServerPacketIds.RefreshItem:
                return new S.RefreshItem();
            case (short)ServerPacketIds.ObjectSpell:
                return new S.ObjectSpell();
            case (short)ServerPacketIds.UserDash:
                return new S.UserDash();
            case (short)ServerPacketIds.ObjectDash:
                return new S.ObjectDash();
            case (short)ServerPacketIds.UserDashFail:
                return new S.UserDashFail();
            case (short)ServerPacketIds.ObjectDashFail:
                return new S.ObjectDashFail();
            case (short)ServerPacketIds.NPCConsign:
                return new S.NPCConsign();
            case (short)ServerPacketIds.NPCMarket:
                return new S.NPCMarket();
            case (short)ServerPacketIds.NPCMarketPage:
                return new S.NPCMarketPage();
            case (short)ServerPacketIds.GuildTerritoryPage:
                return new S.GuildTerritoryPage();
            case (short)ServerPacketIds.ConsignItem:
                return new S.ConsignItem();
            case (short)ServerPacketIds.MarketFail:
                return new S.MarketFail();
            case (short)ServerPacketIds.MarketSuccess:
                return new S.MarketSuccess();
            case (short)ServerPacketIds.ObjectSitDown:
                return new S.ObjectSitDown();
            case (short)ServerPacketIds.InTrapRock:
                return new S.InTrapRock();
            case (short)ServerPacketIds.RemoveMagic:
                return new S.RemoveMagic();
            case (short)ServerPacketIds.BaseStatsInfo:
                return new S.BaseStatsInfo();
            case (short)ServerPacketIds.UserName:
                return new S.UserName();
            case (short)ServerPacketIds.ChatItemStats:
                return new S.ChatItemStats();
            case (short)ServerPacketIds.GuildMemberChange:
                return new S.GuildMemberChange();
            case (short)ServerPacketIds.GuildNoticeChange:
                return new S.GuildNoticeChange();
            case (short)ServerPacketIds.GuildStatus:
                return new S.GuildStatus();
            case (short)ServerPacketIds.GuildInvite:
                return new S.GuildInvite();
            case (short)ServerPacketIds.GuildExpGain:
                return new S.GuildExpGain();
            case (short)ServerPacketIds.GuildNameRequest:
                return new S.GuildNameRequest();
            case (short)ServerPacketIds.GuildStorageGoldChange:
                return new S.GuildStorageGoldChange();
            case (short)ServerPacketIds.GuildStorageItemChange:
                return new S.GuildStorageItemChange();
            case (short)ServerPacketIds.GuildStorageList:
                return new S.GuildStorageList();
            case (short)ServerPacketIds.GuildRequestWar:
                return new S.GuildRequestWar();
            case (short)ServerPacketIds.DefaultNPC:
                return new S.DefaultNPC();
            case (short)ServerPacketIds.NPCUpdate:
                return new S.NPCUpdate();
            case (short)ServerPacketIds.NPCImageUpdate:
                return new S.NPCImageUpdate();
            case (short)ServerPacketIds.MarriageRequest:
                return new S.MarriageRequest();
            case (short)ServerPacketIds.DivorceRequest:
                return new S.DivorceRequest();
            case (short)ServerPacketIds.MentorRequest:
                return new S.MentorRequest();
            case (short)ServerPacketIds.TradeRequest:
                return new S.TradeRequest();
            case (short)ServerPacketIds.TradeAccept:
                return new S.TradeAccept();
            case (short)ServerPacketIds.TradeGold:
                return new S.TradeGold();
            case (short)ServerPacketIds.TradeItem:
                return new S.TradeItem();
            case (short)ServerPacketIds.TradeConfirm:
                return new S.TradeConfirm();
            case (short)ServerPacketIds.TradeCancel:
                return new S.TradeCancel();
            case (short)ServerPacketIds.MountUpdate:
                return new S.MountUpdate();
            case (short)ServerPacketIds.TransformUpdate:
                return new S.TransformUpdate();
            case (short)ServerPacketIds.EquipSlotItem:
                return new S.EquipSlotItem();
            case (short)ServerPacketIds.FishingUpdate:
                return new S.FishingUpdate();
            case (short)ServerPacketIds.ChangeQuest:
                return new S.ChangeQuest();
            case (short)ServerPacketIds.CompleteQuest:
                return new S.CompleteQuest();
            case (short)ServerPacketIds.ShareQuest:
                return new S.ShareQuest();
            case (short)ServerPacketIds.NewQuestInfo:
                return new S.NewQuestInfo();
            case (short)ServerPacketIds.RefreshQuestInfo:
                return new S.RefreshQuestInfo();
            case (short)ServerPacketIds.GainedQuestItem:
                return new S.GainedQuestItem();
            case (short)ServerPacketIds.DeleteQuestItem:
                return new S.DeleteQuestItem();
            case (short)ServerPacketIds.CancelReincarnation:
                return new S.CancelReincarnation();
            case (short)ServerPacketIds.RequestReincarnation:
                return new S.RequestReincarnation();
            case (short)ServerPacketIds.UserBackStep:
                return new S.UserBackStep();
            case (short)ServerPacketIds.ObjectBackStep:
                return new S.ObjectBackStep();
            case (short)ServerPacketIds.UserDashAttack:
                return new S.UserDashAttack();
            case (short)ServerPacketIds.ObjectDashAttack:
                return new S.ObjectDashAttack();
            case (short)ServerPacketIds.UserAttackMove://Warrior Skill - SlashingBurst
                return new S.UserAttackMove();
            case (short)ServerPacketIds.CombineItem:
                return new S.CombineItem();
            case (short)ServerPacketIds.ItemUpgraded:
                return new S.ItemUpgraded();
            case (short)ServerPacketIds.SetConcentration:
                return new S.SetConcentration();
            case (short)ServerPacketIds.SetObjectConcentration:
                return new S.SetObjectConcentration();
            case (short)ServerPacketIds.SetElemental:
                return new S.SetElemental();
            case (short)ServerPacketIds.SetObjectElemental:
                return new S.SetObjectElemental();
            case (short)ServerPacketIds.RemoveDelayedExplosion:
                return new S.RemoveDelayedExplosion();
            case (short)ServerPacketIds.ObjectDeco:
                return new S.ObjectDeco();
            case (short)ServerPacketIds.ObjectSneaking:
                return new S.ObjectSneaking();
            case (short)ServerPacketIds.ObjectLevelEffects:
                return new S.ObjectLevelEffects();
            case (short)ServerPacketIds.SetBindingShot:
                return new S.SetBindingShot();
            case (short)ServerPacketIds.SendOutputMessage:
                return new S.SendOutputMessage();
			case (short)ServerPacketIds.NPCAwakening:
                return new S.NPCAwakening();
            case (short)ServerPacketIds.NPCDisassemble:
                return new S.NPCDisassemble();
            case (short)ServerPacketIds.NPCDowngrade:
                return new S.NPCDowngrade();
            case (short)ServerPacketIds.NPCReset:
                return new S.NPCReset();
            case (short)ServerPacketIds.AwakeningNeedMaterials:
                return new S.AwakeningNeedMaterials();
            case (short)ServerPacketIds.AwakeningLockedItem:
                return new S.AwakeningLockedItem();
            case (short)ServerPacketIds.Awakening:
                return new S.Awakening();
            case (short)ServerPacketIds.ReceiveMail:
                return new S.ReceiveMail();
            case (short)ServerPacketIds.MailLockedItem:
                return new S.MailLockedItem();
            case (short)ServerPacketIds.MailSent:
                return new S.MailSent();
            case (short)ServerPacketIds.MailSendRequest:
                return new S.MailSendRequest();
            case (short)ServerPacketIds.ParcelCollected:
                return new S.ParcelCollected();
            case (short)ServerPacketIds.MailCost:
                return new S.MailCost();
			case (short)ServerPacketIds.ResizeHeroInventory:
                return new S.ResizeHeroInventory();
            case (short)ServerPacketIds.ComboHero:
                return new S.ComboHero();
            case (short)ServerPacketIds.ComboStance:
                return new S.ComboStance();
            case (short)ServerPacketIds.ResizeInventory:
                return new S.ResizeInventory();
            case (short)ServerPacketIds.ResizeStorage:
                return new S.ResizeStorage();
            case (short)ServerPacketIds.NewIntelligentCreature:
                return new S.NewIntelligentCreature();
            case (short)ServerPacketIds.UpdateIntelligentCreatureList:
                return new S.UpdateIntelligentCreatureList();
            case (short)ServerPacketIds.IntelligentCreatureEnableRename:
                return new S.IntelligentCreatureEnableRename();
            case (short)ServerPacketIds.IntelligentCreaturePickup:
                return new S.IntelligentCreaturePickup();
            case (short)ServerPacketIds.NPCPearlGoods:
                return new S.NPCPearlGoods();
            case (short)ServerPacketIds.FriendUpdate:
                return new S.FriendUpdate();
            case (short)ServerPacketIds.SetVIP:
                return new S.SetVIP();
            case (short)ServerPacketIds.SetHumUp:
                return new S.SetHumUp();
            case (short)ServerPacketIds.SetAutoPot:
                return new S.SetAutoPot();
            case (short)ServerPacketIds.SetHero:
                return new S.SetHero();
            case (short)ServerPacketIds.SetHeroSpawned:
                return new S.SetHeroSpawned();
            case (short)ServerPacketIds.SetHeroLocked:
                return new S.SetHeroLocked();
            case (short)ServerPacketIds.LoverUpdate:
                return new S.LoverUpdate();
            case (short)ServerPacketIds.MentorUpdate:
                return new S.MentorUpdate();
            case (short)ServerPacketIds.GuildBuffList:
                return new S.GuildBuffList();
            case (short)ServerPacketIds.GameShopInfo:
                return new S.GameShopInfo();
            case (short)ServerPacketIds.HeroStashInfo:
                return new S.HeroStashInfo();
            case (short)ServerPacketIds.HeroStashActivate:
                return new S.HeroStashActivate();
            case (short)ServerPacketIds.GameShopStock:
                return new S.GameShopStock();
            case (short)ServerPacketIds.NPCRequestInput:
                return new S.NPCRequestInput();
            case (short)ServerPacketIds.Rankings:
                return new S.Rankings();
            case (short)ServerPacketIds.Opendoor:
                return new S.Opendoor();
            case (short)ServerPacketIds.MobHealthChanged:
                return new S.MobHealthChanged();
            case (short)ServerPacketIds.Crafting:
                return new S.Crafting();
            case (short)ServerPacketIds.UpdatePlayerRecipes:
                return new S.UpdatePlayerRecipes();
            case (short)ServerPacketIds.GainShieldExperience:
                return new S.GainShieldEXP();
            case (short)ServerPacketIds.ItemGainLevel:
                return new S.ItemGainLevel();
            case (short)ServerPacketIds.CreateHero:
                return new S.HeroCreation();
            case (short)ServerPacketIds.RecipieShopContents:
                return new S.RecipieShopContents();
            case (short)ServerPacketIds.PurchaseResult:
                return new S.PurchaseResult();
            case (short)ServerPacketIds.EESocketDisplay:
                return new S.GetSocketInfo();
            case (short)ServerPacketIds.EERuneSocketResult:
                return new S.AddRuneResult();
            case (short)ServerPacketIds.EEOpenSocketDialog:
                return new S.OpenSocketDialog();
            case (short)ServerPacketIds.EERemoveRune:
                return new S.RemoveRune();
            case (short)ServerPacketIds.EEHeroRank:
                return new S.HeroRanking();
            case (short)ServerPacketIds.EEOpenGuildBoardDialog:
                return new S.OpenGuildBoardDialog();
            case (short)ServerPacketIds.OpenRankDialog:
                return new S.OpenRankDialog();
            case (short)ServerPacketIds.OpenHeroRankDialog:
                return new S.OpenHeroRankDialog();
            case (short)ServerPacketIds.LogNotice:
                return new S.UpdateLogNotice();
            case (short)ServerPacketIds.EEClientCompletedQuests:
                return new S.ClientCompletedQuests();
            case (short)ServerPacketIds.EEClientAvailableQuests:
                return new S.ClientAvaileQuests();
            case (short)ServerPacketIds.ItemInfoList:
                return new S.ItemInfoList();
            case (short)ServerPacketIds.RefreshHeroCap:
                return new S.RefreshHeroCap();
            case (short)ServerPacketIds.LeavePublicEvent:
                return new S.LeavePublicEvent();
            case (short)ServerPacketIds.ActivateEvent:
                return new S.ActivateEvent();
            case (short)ServerPacketIds.DeactivateEvent:
                return new S.DeactivateEvent();
            case (short)ServerPacketIds.EnterPublicEvent:
                return new S.EnterOrUpdatePublicEvent();
            case (short)ServerPacketIds.HelmetChange:
                return new S.UpdateHelmet();
            case (short)ServerPacketIds.ToggleBuff:
                return new S.ToggleBuff();
            case (short)ServerPacketIds.ExternalToolLogin:
                return new S.ExternalToolLogin();
            case (short)ServerPacketIds.ToolStats:
                return new S.ToolStats();
            case (short)ServerPacketIds.ToolPlayerList:
                return new S.ToolPlayerList();
            case (short)ServerPacketIds.BRList:
                return new S.LMS_BRs();
            case (short)ServerPacketIds.LMSDialog:
                return new S.OpenLMSDialog();
            case (short)ServerPacketIds.BigMapQuestLocations:
                return new S.BigMapQuestLocations();
            default:
                return null;
        }
    }
}

public class ShieldUpgradeConfig
{
    public byte MinDC, MedDC, MaxDC, MinDCRate, MedDCRate, MaxDCRate;
    public byte MinMC, MedMC, MaxMC, MinMCRate, MedMCRate, MaxMCRate;
    public byte MinSC, MedSC, MaxSC, MinSCRate, MedSCRate, MaxSCRate;
    public byte MinAC, MedAC, MaxAC, MinACRate, MedACRate, MaxACRate;
    public byte MinAMC, MedAMC, MaxAMC, MinAMCRate, MedAMCRate, MaxAMCRate;
    public byte MinHP, MedHP, MaxHP, MinHPRate, MedHPRate, MaxHPRate;
    public byte MinMP, MedMP, MaxMP, MinMPRate, MedMPRate, MaxMPRate;
    public byte MinAcc, MedAcc, MaxAcc, MinAccRate, MedAccRate, MaxAccRate;
    public byte MinAgil, MedAgil, MaxAgil, MinAgilRate, MedAgilRate, MaxAgilRate;
    public byte MinCrit, MedCrit, MaxCrit, MinCritRate, MedCritRate, MaxCritRate;
    public byte MinCritDmg, MedCritDmg, MaxCritDmg, MinCritDmgRate, MedCritDmgRate, MaxCritDmgRate;

    public ShieldUpgradeConfig(ItemGrade Grade)
    {
        switch (Grade)
        {
            case ItemGrade.Common:
                MinDC = 0;
                MedDC = 0;
                MaxDC = 0;
                MinDCRate = 0;
                MedDCRate = 0;
                MaxDCRate = 0;
                MinMC = 0;
                MedMC = 0;
                MaxMC = 0;
                MinMCRate = 0;
                MedMCRate = 0;
                MaxMCRate = 0;
                MinSC = 0;
                MedSC = 0;
                MaxSC = 0;
                MinSCRate = 0;
                MedSCRate = 0;
                MaxSCRate = 0;
                MinAC = 0;
                MedAC = 0;
                MaxAC = 0;
                MinACRate = 0;
                MedACRate = 0;
                MaxACRate = 0;
                MinAMC = 0;
                MedAMC = 0;
                MaxAMC = 0;
                MinAMCRate = 0;
                MedAMCRate = 0;
                MaxAMCRate = 0;
                MinHP = 0;
                MedHP = 0;
                MaxHP = 0;
                MinHPRate = 0;
                MedHPRate = 0;
                MaxHPRate = 0;
                MinMP = 0;
                MedMP = 0;
                MaxMP = 0;
                MinMPRate = 0;
                MedMPRate = 0;
                MaxMPRate = 0;
                MinAcc = 0;
                MedAcc = 0;
                MaxAcc = 0;
                MinAccRate = 0;
                MedAccRate = 0;
                MaxAccRate = 0;
                MinAgil = 0;
                MedAgil = 0;
                MaxAgil = 0;
                MinAgilRate = 0;
                MedAgilRate = 0;
                MaxAgilRate = 0;
                MinCrit = 0;
                MedCrit = 0;
                MaxCrit = 0;
                MinCritRate = 0;
                MedCritRate = 0;
                MaxCritRate = 0;
                MinCritDmg = 0;
                MedCritDmg = 0;
                MaxCritDmg = 0;
                MinCritDmgRate = 0;
                MedCritDmgRate = 0;
                MaxCritDmgRate = 0;
                break;
            case ItemGrade.Rare:
                MinDC = 0;
                MedDC = 0;
                MaxDC = 0;
                MinDCRate = 0;
                MedDCRate = 0;
                MaxDCRate = 0;
                MinMC = 0;
                MedMC = 0;
                MaxMC = 0;
                MinMCRate = 0;
                MedMCRate = 0;
                MaxMCRate = 0;
                MinSC = 0;
                MedSC = 0;
                MaxSC = 0;
                MinSCRate = 0;
                MedSCRate = 0;
                MaxSCRate = 0;
                MinAC = 0;
                MedAC = 0;
                MaxAC = 0;
                MinACRate = 0;
                MedACRate = 0;
                MaxACRate = 0;
                MinAMC = 0;
                MedAMC = 0;
                MaxAMC = 0;
                MinAMCRate = 0;
                MedAMCRate = 0;
                MaxAMCRate = 0;
                MinHP = 0;
                MedHP = 0;
                MaxHP = 0;
                MinHPRate = 0;
                MedHPRate = 0;
                MaxHPRate = 0;
                MinMP = 0;
                MedMP = 0;
                MaxMP = 0;
                MinMPRate = 0;
                MedMPRate = 0;
                MaxMPRate = 0;
                MinAcc = 0;
                MedAcc = 0;
                MaxAcc = 0;
                MinAccRate = 0;
                MedAccRate = 0;
                MaxAccRate = 0;
                MinAgil = 0;
                MedAgil = 0;
                MaxAgil = 0;
                MinAgilRate = 0;
                MedAgilRate = 0;
                MaxAgilRate = 0;
                MinCrit = 0;
                MedCrit = 0;
                MaxCrit = 0;
                MinCritRate = 0;
                MedCritRate = 0;
                MaxCritRate = 0;
                MinCritDmg = 0;
                MedCritDmg = 0;
                MaxCritDmg = 0;
                MinCritDmgRate = 0;
                MedCritDmgRate = 0;
                MaxCritDmgRate = 0;
                break;
            case ItemGrade.Legendary:
                MinDC = 0;
                MedDC = 0;
                MaxDC = 0;
                MinDCRate = 0;
                MedDCRate = 0;
                MaxDCRate = 0;
                MinMC = 0;
                MedMC = 0;
                MaxMC = 0;
                MinMCRate = 0;
                MedMCRate = 0;
                MaxMCRate = 0;
                MinSC = 0;
                MedSC = 0;
                MaxSC = 0;
                MinSCRate = 0;
                MedSCRate = 0;
                MaxSCRate = 0;
                MinAC = 0;
                MedAC = 0;
                MaxAC = 0;
                MinACRate = 0;
                MedACRate = 0;
                MaxACRate = 0;
                MinAMC = 0;
                MedAMC = 0;
                MaxAMC = 0;
                MinAMCRate = 0;
                MedAMCRate = 0;
                MaxAMCRate = 0;
                MinHP = 0;
                MedHP = 0;
                MaxHP = 0;
                MinHPRate = 0;
                MedHPRate = 0;
                MaxHPRate = 0;
                MinMP = 0;
                MedMP = 0;
                MaxMP = 0;
                MinMPRate = 0;
                MedMPRate = 0;
                MaxMPRate = 0;
                MinAcc = 0;
                MedAcc = 0;
                MaxAcc = 0;
                MinAccRate = 0;
                MedAccRate = 0;
                MaxAccRate = 0;
                MinAgil = 0;
                MedAgil = 0;
                MaxAgil = 0;
                MinAgilRate = 0;
                MedAgilRate = 0;
                MaxAgilRate = 0;
                MinCrit = 0;
                MedCrit = 0;
                MaxCrit = 0;
                MinCritRate = 0;
                MedCritRate = 0;
                MaxCritRate = 0;
                MinCritDmg = 0;
                MedCritDmg = 0;
                MaxCritDmg = 0;
                MinCritDmgRate = 0;
                MedCritDmgRate = 0;
                MaxCritDmgRate = 0;
                break;
            case ItemGrade.Mythical:
                MinDC = 0;
                MedDC = 0;
                MaxDC = 0;
                MinDCRate = 0;
                MedDCRate = 0;
                MaxDCRate = 0;
                MinMC = 0;
                MedMC = 0;
                MaxMC = 0;
                MinMCRate = 0;
                MedMCRate = 0;
                MaxMCRate = 0;
                MinSC = 0;
                MedSC = 0;
                MaxSC = 0;
                MinSCRate = 0;
                MedSCRate = 0;
                MaxSCRate = 0;
                MinAC = 0;
                MedAC = 0;
                MaxAC = 0;
                MinACRate = 0;
                MedACRate = 0;
                MaxACRate = 0;
                MinAMC = 0;
                MedAMC = 0;
                MaxAMC = 0;
                MinAMCRate = 0;
                MedAMCRate = 0;
                MaxAMCRate = 0;
                MinHP = 0;
                MedHP = 0;
                MaxHP = 0;
                MinHPRate = 0;
                MedHPRate = 0;
                MaxHPRate = 0;
                MinMP = 0;
                MedMP = 0;
                MaxMP = 0;
                MinMPRate = 0;
                MedMPRate = 0;
                MaxMPRate = 0;
                MinAcc = 0;
                MedAcc = 0;
                MaxAcc = 0;
                MinAccRate = 0;
                MedAccRate = 0;
                MaxAccRate = 0;
                MinAgil = 0;
                MedAgil = 0;
                MaxAgil = 0;
                MinAgilRate = 0;
                MedAgilRate = 0;
                MaxAgilRate = 0;
                MinCrit = 0;
                MedCrit = 0;
                MaxCrit = 0;
                MinCritRate = 0;
                MedCritRate = 0;
                MaxCritRate = 0;
                MinCritDmg = 0;
                MedCritDmg = 0;
                MaxCritDmg = 0;
                MinCritDmgRate = 0;
                MedCritDmgRate = 0;
                MaxCritDmgRate = 0;
                break;
        }  
    }
}

public class BaseStats
{
    public float HpGain, HpGainRate, MpGainRate, BagWeightGain, WearWeightGain, HandWeightGain;
    public byte MinAc, MaxAc, MinMac, MaxMac, MinDc, MaxDc, MinMc, MaxMc, MinSc, MaxSc, StartAgility, StartAccuracy, StartCriticalRate, StartCriticalDamage, CritialRateGain, CriticalDamageGain;

    public BaseStats(MirClass Job)
    {
        switch (Job)
        {
            case MirClass.Warrior:
                HpGain = 4F;
                HpGainRate = 4.5F;
                MpGainRate = 0;
                BagWeightGain = 3F;
                WearWeightGain = 20F;
                HandWeightGain = 13F;
                MinAc = 0;
                MaxAc = 7;
                MinMac = 0;
                MaxMac = 0;
                MinDc = 5;
                MaxDc = 5;
                MinMc = 0;
                MaxMc = 0;
                MinSc = 0;
                MaxSc = 0;
                StartAgility = 15;
                StartAccuracy = 5;
                StartCriticalRate = 0;
                StartCriticalDamage = 0;
                CritialRateGain = 0;
                CriticalDamageGain = 0;
                break;
            case MirClass.Wizard:
                HpGain = 15F;
                HpGainRate = 1.8F;
                MpGainRate = 0;
                BagWeightGain = 5F;
                WearWeightGain = 100F;
                HandWeightGain = 90F;
                MinAc = 0;
                MaxAc = 0;
                MinMac = 0;
                MaxMac = 0;
                MinDc = 7;
                MaxDc = 7;
                MinMc = 7;
                MaxMc = 7;
                MinSc = 0;
                MaxSc = 0;
                StartAgility = 15;
                StartAccuracy = 5;
                StartCriticalRate = 0;
                StartCriticalDamage = 0;
                CritialRateGain = 0;
                CriticalDamageGain = 0;
                break;
            case MirClass.Taoist:
                HpGain = 6F;
                HpGainRate = 2.5F;
                MpGainRate = 0;
                BagWeightGain = 4F;
                WearWeightGain = 50F;
                HandWeightGain = 42F;
                MinAc = 0;
                MaxAc = 0;
                MinMac = 12;
                MaxMac = 6;
                MinDc = 7;
                MaxDc = 7;
                MinMc = 0;
                MaxMc = 0;
                MinSc = 7;
                MaxSc = 7;
                StartAgility = 18;
                StartAccuracy = 5;
                StartCriticalRate = 0;
                StartCriticalDamage = 0;
                CritialRateGain = 0;
                CriticalDamageGain = 0;
                break;
            case MirClass.Assassin:
                HpGain = 4F;
                HpGainRate = 3.25F;
                MpGainRate = 0;
                BagWeightGain = 3.5F;
                WearWeightGain = 33F;
                HandWeightGain = 30F;
                MinAc = 0;
                MaxAc = 0;
                MinMac = 0;
                MaxMac = 0;
                MinDc = 8;
                MaxDc = 8;
                MinMc = 0;
                MaxMc = 0;
                MinSc = 0;
                MaxSc = 0;
                StartAgility = 20;
                StartAccuracy = 5;
                StartCriticalRate = 0;
                StartCriticalDamage = 0;
                CritialRateGain = 0;
                CriticalDamageGain = 0;
                break;
            case MirClass.Archer:
                HpGain = 4F;
                HpGainRate = 3.25F;
                MpGainRate = 0;
                BagWeightGain = 4F;
                WearWeightGain = 33F;
                HandWeightGain = 30F;
                MinAc = 0;
                MaxAc = 0;
                MinMac = 0;
                MaxMac = 0;
                MinDc = 8;
                MaxDc = 8;
                MinMc = 8;
                MaxMc = 8;
                MinSc = 0;
                MaxSc = 0;
                StartAgility = 15;
                StartAccuracy = 8;
                StartCriticalRate = 0;
                StartCriticalDamage = 0;
                CritialRateGain = 0;
                CriticalDamageGain = 0;
                break;
        }
    }
    public BaseStats(BinaryReader reader)
    {
        HpGain = reader.ReadSingle();
        HpGainRate = reader.ReadSingle();
        MpGainRate = reader.ReadSingle();
        MinAc = reader.ReadByte();
        MaxAc = reader.ReadByte();
        MinMac = reader.ReadByte();
        MaxMac = reader.ReadByte();
        MinDc = reader.ReadByte();
        MaxDc = reader.ReadByte();
        MinMc = reader.ReadByte();
        MaxMc = reader.ReadByte();
        MinSc = reader.ReadByte();
        MaxSc = reader.ReadByte();
        StartAccuracy = reader.ReadByte();
        StartAgility = reader.ReadByte();
        StartCriticalRate = reader.ReadByte();
        StartCriticalDamage = reader.ReadByte();
        CritialRateGain = reader.ReadByte();
        CriticalDamageGain = reader.ReadByte();
        BagWeightGain = reader.ReadSingle();
        WearWeightGain = reader.ReadSingle();
        HandWeightGain = reader.ReadSingle();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(HpGain);
        writer.Write(HpGainRate);
        writer.Write(MpGainRate);
        writer.Write(MinAc);
        writer.Write(MaxAc);
        writer.Write(MinMac);
        writer.Write(MaxMac);
        writer.Write(MinDc);
        writer.Write(MaxDc);
        writer.Write(MinMc);
        writer.Write(MaxMc);
        writer.Write(MinSc);
        writer.Write(MaxSc);
        writer.Write(StartAccuracy);
        writer.Write(StartAgility);
        writer.Write(StartCriticalRate);
        writer.Write(StartCriticalDamage);
        writer.Write(CritialRateGain);
        writer.Write(CriticalDamageGain);
        writer.Write(BagWeightGain);
        writer.Write(WearWeightGain);
        writer.Write(HandWeightGain);
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
    public UserItem ItemStats;
}

public class UserId
{
    public long Id = 0;
    public string UserName = "";
}

#region ItemSets

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
            return Count == Amount;
        }
    }
}

#endregion

#region "Mine Related"
public class MineSet
{
    public string Name = string.Empty;
    public byte SpotRegenRate = 5;
    public byte MaxStones = 80;
    public byte HitRate = 25;
    public byte DropRate = 10;
    public byte TotalSlots = 100;
    public List<MineDrop> Drops = new List<MineDrop>();
    private bool DropsSet = false;

    public MineSet(byte MineType = 0)
    {
        switch (MineType)
        {
            case 1:
                TotalSlots = 120;
                Drops.Add(new MineDrop(){ItemName = "GoldOre", MinSlot = 1, MaxSlot = 2, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10});
                Drops.Add(new MineDrop() { ItemName = "SilverOre", MinSlot = 3, MaxSlot = 20, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                Drops.Add(new MineDrop() { ItemName = "CopperOre", MinSlot = 21, MaxSlot = 45, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                Drops.Add(new MineDrop() { ItemName = "BlackIronOre", MinSlot = 46, MaxSlot = 56, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                break;
            case 2:
                TotalSlots = 100;
                Drops.Add(new MineDrop(){ItemName = "PlatinumOre", MinSlot = 1, MaxSlot = 2, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10});
                Drops.Add(new MineDrop() { ItemName = "RubyOre", MinSlot = 3, MaxSlot = 20, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                Drops.Add(new MineDrop() { ItemName = "NephriteOre", MinSlot = 21, MaxSlot = 45, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                Drops.Add(new MineDrop() { ItemName = "AmethystOre", MinSlot = 46, MaxSlot = 56, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                break;
        }
    }

    public void SetDrops(List<ItemInfo> items)
    {
        if (DropsSet) return;
        for (int i = 0; i < Drops.Count; i++)
        {
            for (int j = 0; j < items.Count; j++)
            {
                ItemInfo info = items[j];
                if (String.Compare(info.Name.Replace(" ", ""), Drops[i].ItemName, StringComparison.OrdinalIgnoreCase) != 0) continue;
                Drops[i].Item = info;
                break;
            }
        }
        DropsSet = true;
    }
}

public class MineSpot
{
    public byte StonesLeft = 0;
    public long LastRegenTick = 0;
    public MineSet Mine;
}

public class MineDrop
{
    public string ItemName;
    public ItemInfo Item;
    public byte MinSlot = 0;
    public byte MaxSlot = 0;
    public byte MinDura = 1;
    public byte MaxDura = 1;
    public byte BonusChance = 0;
    public byte MaxBonusDura = 1;
}

public class MineZone
{
    public byte Mine;
    public Point Location;
    public ushort Size;

    public MineZone()
    {
    }

    public MineZone(BinaryReader reader)
    {
        Location = new Point(reader.ReadInt32(), reader.ReadInt32());
        Size = reader.ReadUInt16();
        Mine = reader.ReadByte();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Location.X);
        writer.Write(Location.Y);
        writer.Write(Size);
        writer.Write(Mine);
    }
    public override string ToString()
    {
        return string.Format("Mine: {0}- {1}", Functions.PointToString(Location), Mine);
    }
}
#endregion

#region "Guild Related"
public class ItemVolume
{
    public ItemInfo Item;
    public string ItemName;
    public uint Amount;
}

public class Rank
{
    public List<GuildMember> Members = new List<GuildMember>();
    public string Name = "";
    public int Index = 0;
    public RankOptions Options = 0;
    public Rank() 
    {
    }
    public Rank(BinaryReader reader, bool Offline = false, int version = int.MaxValue)
    {
        Name = reader.ReadString();
        Options = (RankOptions)reader.ReadByte();
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
            Members[j].save(writer);
    }
}

public class GuildStorageItem
{
    public UserItem Item;
    public long UserId = 0;
    public GuildStorageItem()
    {
    }
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

    public GuildMember()
    {}
    public GuildMember(BinaryReader reader, bool Offline = false)
    {
        name = reader.ReadString();
        Id = reader.ReadInt32();
        LastLogin = DateTime.FromBinary(reader.ReadInt64());
        hasvoted = reader.ReadBoolean();
        Online = reader.ReadBoolean();
        Online = Offline ? false: Online;
    }
    public void save(BinaryWriter writer)
    {
        writer.Write(name);
        writer.Write(Id);
        writer.Write(LastLogin.ToBinary());
        writer.Write(hasvoted);
        writer.Write(Online);
    }
}

[Flags]
[Obfuscation(Feature = "renaming", Exclude = true)]
public enum RankOptions : byte
{
    CanChangeRank = 1,
    CanRecruit = 2,
    CanKick = 4,
    CanStoreItem = 8,
    CanRetrieveItem = 16,
    CanAlterAlliance = 32,
    CanChangeNotice = 64,
    CanActivateBuff = 128
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
    public int  BuffMaxHp;
    public int  BuffMaxMp;
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

    public GuildBuffInfo()
    {

    }

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
        Id  = reader.ReadInt32("Buff-" + i.ToString(), "Id",0);
        Icon = reader.ReadInt32("Buff-" + i.ToString(), "Icon", 0);
        name = reader.ReadString("Buff-" + i.ToString(), "Name","");
        LevelRequirement = reader.ReadByte("Buff-" + i.ToString(), "LevelReq",0);
        PointsRequirement = reader.ReadByte("Buff-" + i.ToString(), "PointsReq",1);
        TimeLimit = reader.ReadInt32("Buff-" + i.ToString(), "TimeLimit",0);;
        ActivationCost = reader.ReadInt32("Buff-" + i.ToString(), "ActivationCost",0);
        BuffAc = reader.ReadByte("Buff-" + i.ToString(), "BuffAc",0);
        BuffMac = reader.ReadByte("Buff-" + i.ToString(), "BuffMAC",0);
        BuffDc = reader.ReadByte("Buff-" + i.ToString(), "BuffDc",0);
        BuffMc = reader.ReadByte("Buff-" + i.ToString(), "BuffMc",0);
        BuffSc = reader.ReadByte("Buff-" + i.ToString(), "BuffSc",0);
        BuffMaxHp = reader.ReadInt32("Buff-" + i.ToString(), "BuffMaxHp",0);
        BuffMaxMp = reader.ReadInt32("Buff-" + i.ToString(), "BuffMaxMp",0);
        BuffMineRate = reader.ReadByte("Buff-" + i.ToString(), "BuffMineRate",0);
        BuffGemRate = reader.ReadByte("Buff-" + i.ToString(), "BuffGemRate",0);
        BuffFishRate = reader.ReadByte("Buff-" + i.ToString(), "BuffFishRate",0);
        BuffExpRate = reader.ReadByte("Buff-" + i.ToString(), "BuffExpRate",0);
        BuffCraftRate = reader.ReadByte("Buff-" + i.ToString(), "BuffCraftRate",0);
        BuffSkillRate = reader.ReadByte("Buff-" + i.ToString(), "BuffSkillRate",0);
        BuffHpRegen = reader.ReadByte("Buff-" + i.ToString(), "BuffHpRegen",0);
        BuffMPRegen = reader.ReadByte("Buff-" + i.ToString(), "BuffMpRegen",0);
        BuffAttack = reader.ReadByte("Buff-" + i.ToString(), "BuffAttack",0);
        BuffDropRate = reader.ReadByte("Buff-" + i.ToString(), "BuffDropRate",0);
        BuffGoldRate = reader.ReadByte("Buff-" + i.ToString(), "BuffGoldRate",0);
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

    public GuildBuff()
    {
    }

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
    public GuildBuffOld()
    { }
    public GuildBuffOld(BinaryReader reader)
    {
        reader.ReadByte();
        reader.ReadInt64();
    }
}

#endregion

#region Ranking Pete107|Petesn00beh 15/1/2016
public class Rank_Character_Info
{
    public long PlayerId;
    public string Name;
    public MirClass Class;
    public int level;
    //public int rank;
    public long Experience;//clients shouldnt care about this only server
    public object info;//again only keep this on server!

    public Rank_Character_Info()
    {

    }
    public Rank_Character_Info(BinaryReader reader)
    {
        //rank = reader.ReadInt32();
        PlayerId = reader.ReadInt64();
        Name = reader.ReadString();
        level = reader.ReadInt32();
        Class = (MirClass)reader.ReadByte();
    }
    public void Save(BinaryWriter writer)
    {
        //writer.Write(rank);
        writer.Write(PlayerId);
        writer.Write(Name);
        writer.Write(level);
        writer.Write((byte)Class);
    }
}
#endregion

public class Door
{
    public byte index;
    public byte DoorState;//0: closed, 1: opening, 2: open, 3: closing
    public byte ImageIndex;
    public long LastTick;
    public Point Location;
}

public static class Ext
{
    public static bool In<T>(this T val, params T[] values) where T : struct
    {
        return values.Contains(val);
    }
}

#region RecipeShop Pete107 - IceMan
public class RecipeShopForClient
{
    public int Index;
    public UserItem ItemBeingSold;

    //  Important, ensure the price is >= to the set price on the actual item or people will exploit it no matter how little profit
    public uint ItemPrice;

    public RecipeShopForClient()
    {

    }

    public RecipeShopForClient(BinaryReader reader)
    {
        Index = reader.ReadInt32();
        //  Check if an Item was Saved
        bool hasItem = reader.ReadBoolean();
        if (hasItem)
        {
            //  Item was Saved so load it
            ItemBeingSold = new UserItem(reader);
            //  Set the price
            ItemPrice = reader.ReadUInt32();

        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        //  Save true if the item is Valid other wise it'll be false
        writer.Write(ItemBeingSold != null ? true : false);
        if (ItemBeingSold != null)
        {
            //  Save the item
            ItemBeingSold.Save(writer);
            //  Save the Price
            writer.Write(ItemPrice);
        }
    }
}

public class RecipeShop
{
    public int Index;
    public ItemInfo ItemBeingSold;

    //  Important, ensure the price is >= to the set price on the actual item or people will exploit it no matter how little profit
    public uint ItemPrice;

    public RecipeShop()
    {

    }

    public RecipeShop(BinaryReader reader, int version = int.MaxValue)
    {
        Index = reader.ReadInt32();
        bool hadItem = reader.ReadBoolean();
        //  Check if an Item was Saved
        if (hadItem)
        {
            //  Item was Saved so load it
            ItemBeingSold = new ItemInfo(reader, version);
            //  Set the price
            ItemPrice = reader.ReadUInt32();
            
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        //  Save true if the item is Valid other wise it'll be false
        writer.Write(ItemBeingSold != null ? true : false);
        if (ItemBeingSold != null)
        {
            //  Save the item
            ItemBeingSold.Save(writer);
            //  Save the Price
            writer.Write(ItemPrice);            
        }
    }

    public override string ToString()
    {
        if (ItemBeingSold == null)
            return string.Format("[{0}]", Index);
        else
            return string.Format("[{0}] {1}", Index, ItemBeingSold.Name.Length > 0 ? ItemBeingSold.Name : "ERROR");
    }
}
#endregion


public class SocketPassiveBuff
{
    public SocketType _Type;
    public long Duration;
    public long CoolDown;
    public byte Chance;
    public int Val1;
    public int Val2;
    public int Val3;
    public int Val4;
}

public class HeroRank
{
    //  Index   -   Get owners Name from this
    public int Owner;
    //  Players NAme
    public string OwnerName;
    //  Level
    public int Level;
    //  Exp
    public long Experience;
    //  Name    -   Heroes Name
    public string Name;
    //  Class
    public MirClass HeroesClass;
    //  Gender
    public MirGender HeroesGender;
    //  Last Update Time
    public long LastUpdateTime;
    //  Last Rank
    public byte LastRank;
    //  Rank Now
    public byte Rank;

    public HeroRank()
    {
        
    }

    public HeroRank(BinaryReader reader)
    {
        Owner = reader.ReadInt32();
        OwnerName = reader.ReadString();
        Level = reader.ReadInt32();
        Experience = reader.ReadInt64();
        Name = reader.ReadString();
        HeroesClass = (MirClass)reader.ReadByte();
        HeroesGender = (MirGender)reader.ReadByte();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Owner);
        writer.Write(OwnerName);
        writer.Write(Level);
        writer.Write(Experience);
        writer.Write(Name);
        writer.Write((byte)HeroesClass);
        writer.Write((byte)HeroesGender);
    }

    public void UpdateTime(long tmp)
    {
        if (LastUpdateTime > tmp)
            return;
        else
            LastUpdateTime = tmp;
    }
}

public enum HeroBehaviour
{
    None = 0,
    //  Close to the master as much as possible (I.E doesn't run around/away)
    CloseMaster = 1,
    //  Only for Wizard & Tao Heroes (It's the fear movement) 
    Kite = 2,
}

public enum HeroEXPShare
{
    None = 0,
    Equal = 1,
    Damage = 2,
    Default = 3
}

#region Achievement System
[Obfuscation(Feature = "renaming", Exclude = true)]
public class PlayerLogin
{

}
[Obfuscation(Feature = "renaming", Exclude = true)]
public class LoginReward
{

}
[Obfuscation(Feature = "renaming", Exclude = true)]
public class PlayerJournal
{

}
[Obfuscation(Feature = "renaming", Exclude = true)]
public class PlayerAchievment
{
    public Achievement Info;
    public long Progress;   //  #/#
    public bool Complete;
    public bool Claimed;
    public DateTime CompleteDate;
    public DateTime StartDate;
}
/// <summary>
/// The Achievement data that will be held on the Server and Displayed on the Client depending on if they meet the requirements for the Achievement.
/// </summary>
[Obfuscation(Feature = "renaming", Exclude = true)]
public class Achievement
{
    /// <summary>
    /// Index is used to identify a unique Achievement.
    /// </summary>
    public int Index;
    /// <summary>
    /// The title of the Achievement.
    /// </summary>
    public string Name;
    /// <summary>
    /// The description of the Achievement which will be Display on the client.
    /// </summary>
    public string Description;
    /// <summary>
    /// Default set to true to use the LevelRequired and/or AchievementRequired values.
    /// </summary>
    public bool RequirementsNeeded = true;
    /// <summary>
    /// If the RequirementsNeeded is true we will check if the player is the right level to start the Achievement.
    /// </summary>
    public uint LevelRequired;
    /// <summary>
    /// If the RequirementsNeeded is true we will check if the player has completed a specific Achievement, use this for Chain Achievements.
    /// </summary>
    public uint AchievementRequired;
    /// <summary>
    /// Multiple Options allowed.
    /// </summary>
    public AchievementType Type = AchievementType.None;
    /// <summary>
    /// The Achievement Point(s) that are rewarded upon completion.
    /// </summary>
    public uint AchievementPointReward;
    /// <summary>
    /// The Experience that is rewarded upon completion.
    /// </summary>
    public long ExperienceReward;
    /// <summary>
    /// The Gold that is rewarded upon completion.
    /// </summary>
    public long GoldReward;
    /// <summary>
    /// The Credit(s) that is rewarded upon completion.
    /// </summary>
    public long CreditReward;
    /// <summary>
    /// An array of Item index(es) which can hold a maximum of 10.
    /// </summary>
    public int[] ItemRewards = new int[10];
    /// <summary>
    /// A value of 0 = Give or 1 = Pick will determine if the user has a choice of reward(s).
    /// </summary>
    public byte[] RewardFlags = new byte[]
    {
        0,  //  0
        0,  //  1
        0,  //  2
        0,  //  3
        0,  //  4
        0,  //  5
        0,  //  6
        0,  //  7
        0,  //  8
        0,  //  9
    };
    /// <summary>
    /// The Monster(s) and it's/their amount required to complete the Achievement.
    /// </summary>
    public List<MonsterKill> MobKills = new List<MonsterKill>();
    /// <summary>
    /// The Quest(s) required to complete the Achievement.
    /// </summary>
    public List<QuestCompletions> QuestInfos = new List<QuestCompletions>();
    /// <summary>
    /// The Item(s) and it's/their amount(s) required to complete the Achievement.
    /// </summary>
    public List<ItemsRequired> ItemsRequired = new List<ItemsRequired>();
    /// <summary>
    /// The Achievement(s) required to complete the Achievement.
    /// </summary>
    public List<AchievementsComplete> AchievementsCompleted = new List<AchievementsComplete>();
    /// <summary>
    /// The Level required to compelete the Achievement.
    /// </summary>
    public uint LevelGained;
    /// <summary>
    /// The Amount of Gold gained in a lifetime required to complete the Achievement.
    /// </summary>
    public long TotalGoldGained;
    /// <summary>
    /// The Number of Player Kill(s) required to complete the Achievement.
    /// </summary>
    public ushort PlayerKills;
    /// <summary>
    /// The Index of the skill required to complete the Achievement.
    /// </summary>
    public int SkillIndex;
}
[Obfuscation(Feature = "renaming", Exclude = true)]
public class AchievementsComplete
{
    public int AchievementIndex;
}
[Obfuscation(Feature = "renaming", Exclude = true)]
public class ItemsRequired
{
    public int ItemIndex;
    public ushort Amount;
}
[Obfuscation(Feature = "renaming", Exclude = true)]
public class QuestCompletions
{
    public int QuestIndex;
}
[Obfuscation(Feature = "renaming", Exclude = true)]
public class MonsterKill
{
    public int MonsterIndex;
    public ushort KillAmount;
}
[Flags]
[Obfuscation(Feature = "renaming", Exclude = true)] 
public enum AchievementType : uint
{
    None = 0,
    //  Kill 1 Mob
    MobKill = 1,
    //  Kill x Mobs
    MobKills = 2,
    //  Enter X Map
    MapEnter = 4,
    //  Complete X Quest
    CompleteQuest = 8,
    //  Gain X Item
    GainItem = 16,
    //  Reach x Level
    LevelGained = 32,
    //  Earn X Gold in a lifetime
    GoldGained = 64,
    //  Complete X Many Achievements
    AchievementsCompleted = 128,
    //  Kill x Players
    PlayerKills = 256,
    //  Skill Gained
    SkillGained = 512,
    //  Gain x Many items
    GainItems = 1024,
    //  Gained a hero
    GainHero = 2048,
    //  Hero Gained x Level
    HeroLevel = 4096,
    //  Hero Gained X Skill
    HeroGainSkill = 8192,
    //  Hero Quest Completed
    HeroCompleteQuest = 16384
}
#endregion

#region Mob Ai system
/// <summary>
/// The attack Type (will determine the kind of effects used (I.E Client side))
/// </summary>
public enum AIAttackType
{
    Default = 0,
    Melee = 1,
    Range = 2,
    AOE = 3,
    Heal = 4,
    Buff = 5,
    Debuff = 6
}
/// <summary>
/// The style of the Attack to be used.
/// </summary>
public enum AIAttackStyle
{
    Default = 0,
    MeleeSingle = 1,
    MeleeMulti = 2,
    RangeSingle = 3,
    RangeMulti = 4,
    AOESelf = 5,
    AOETarget = 6,
    HealSelf = 7,
    HealTarget = 8,
    HealArea = 9,
    BuffSelf = 10,
    BuffTarget = 11,
    BuffArea = 12,
    DebuffSelf = 13,
    DebuffTarget = 14,
    DebuffArea = 15
}
/// <summary>
/// Which kind of Object to Attack
/// </summary>
public enum AIAttackTargetType
{
    None = 0,
    Player = 1,
    Monster = 2,
    Hero = 3,
}
/// <summary>
/// Which class target to favour
/// </summary>
public enum AIAttackTargetClass
{
    None = 0,
    Warrior = 0,
    Wizard = 1,
    Taoist = 3,
    Assasin = 4,
    Archer = 5,
}
/// <summary>
/// It's walking Behaviour
/// </summary>
public enum AIWalkType
{
    Default = 0,
    RunAway = 1,
    Kite = 2, // Like ZumaArcher
    StandStill = 3,
}
/// <summary>
/// The poisons it can ignore
/// </summary>
public enum AIAntiPoison : short
{
    None = 0,
    Green = 1,
    Red = 2,
    Slow = 4,
    Frozen = 8,
    Stun = 16,
    Paralysis = 32,
    DelayedExplosion = 64,
    Bleeding = 128,
    LRParalysis = 256,
    Trap = 512,
    Burning = 1024,
}
/// <summary>
/// Normal Health or Staged health (I.E 15x 65535 HP)
/// </summary>
public enum AIHealthType
{
    Default = 0,
    Staged = 1,
}
/// <summary>
/// The Debuffs it can apply to it's target(s)
/// </summary>
public enum AIDebuffType
{
    None = 0,
    StatDC = 1,
    StatMC = 2,
    StatSC = 3,
    StatAC = 4,
    StatMAC = 5,
    StatHP = 6,
    StatMP = 7,
    StatAcc = 8,
    StatAgil = 9,
    StatSpeed = 10,
    StatAll = 11
}
/// <summary>
/// The Buffs the Mob can give
/// </summary>
public enum AIBuffType
{
    StatDC = 0,
    StatMC = 1,
    StatSC = 2,
    StatAC = 3,
    StatMAC = 4,
    StatHP = 6,
    StatMP = 7,
    StatAcc = 8,
    StatAgil = 9,
    StatSpeed = 10,
    StatAll = 11
}

/// <summary>
/// The Strength of the Mob vs the Class
/// </summary>
public enum AIStrongVs
{
    None = 0,
    Warrior = 0,
    Wizard = 1,
    Taoist = 3,
    Assasin = 4,
    Archer = 5,
    Mob = 6
}
/// <summary>
/// The mob will be weak vs this kind of attacker/target
/// </summary>
public enum AIWeakVs
{
    None = 0,
    Warrior = 0,
    Wizard = 1,
    Taoist = 3,
    Assasin = 4,
    Archer = 5,
    Mob = 6,
}

public enum AIAttackEffect : ushort
{
    None,
    FatalSword,
    Teleport,
    Healing,
    RedMoonEvil,
    TwinDrakeBlade,
    MagicShieldUp,
    MagicShieldDown,
    FireSprayHit,
    GreatFoxSpirit,
    Entrapment,
    Reflect,
    Critical,
    Mine,
    ElementalBarrierUp,
    ElementalBarrierDown,
    DelayedExplosion,
    MPEater,
    Hemorrhage,
    Bleeding,
    AwakeningSuccess,
    AwakeningFail,
    AwakeningMiss,
    AwakeningHit,
    StormEscape,
    TurtleKing,
    Behemoth,
    Stunned,
    IcePillar,
    #region Weapon Effects
    CrystalBeastSpin,
    CrystalBeastExplosion,
    CrystalBeastSplash,
    CrystalBeastBlast,
    GFBHit,
    PsnHit,
    SFBHit,
    Shocking,
    UEnhance,
    TDBEff0,
    TDBEff1,
    Cake,
    CircleEff,
    PurpleBlastEff,
    BubbleEff,
    ExplosionEff,
    RainbowEff,
    GoldenBubble,
    PurpleExplosion,
    GreenExplosion,
    Swirling,
    EyeEff,
    PhoenixEff,
    ElectricExplosion,
    TrapEff,
    CastingBlizzard,
    BlizzardCast,
    CHM,
    #endregion
    MutantWeb,
    MutantWebHold,
    FlyingStatueCast,
    FlyingStatuePurple,
    FlyingStatueGreen,
    StainHammerCatHit,
    BlackHammerCatSmash,
    KingGuardHold,
    KingGuardSmash,
    HumanFireBall,
    HumanThunderBolt,
    HumanRepulse,
    HumanFireWallCast,
    HumanBlizzardCast,
    HumanMeteorCast,
    HumanMagicShield,
    HumanFlamingSword,
    HumanTwinDrakeBlade,
    HumanHalfMoon,
    HumanCrossHalfMoon,
    HumanThrusting,
    HumanRage,
    HumanSoulFireBall,
    HumanPoisoning,
    HumanCurseCast,
    HumanCastBlessArm,
    HumanCastSoulShield,
    HumanCastPoisonCloud,
    DragonFlamesEffect,
    BrokenSoulCutEffect,
    ThunderClapEffect,
    LastJudgementEffect,
    ChopChopStarEffect,
    SoulEaterSwampEffect,
    HandOfGodEffect,
    TucsonGeneralEffect,
    LavaBurning,
    Jar2Effect,
    RestlessJarSpawn,
    RestlessJarSpawnOnPlayer,
    RestlessJarMultipleHit,
    RestlessJarTornado,
    MoonMist
}


public class AIAttack
{
    public AIAttackType AttackType = AIAttackType.Melee;
    public AIAttackStyle AttackStyle = AIAttackStyle.Default;
    public AIAttackEffect AttackEffect = AIAttackEffect.None;
    public AIDebuffType Debuff = AIDebuffType.None;
    public byte DebuffDuration;
    public bool Steal = false;
    public byte StealPercent = 0;
    public byte StealDuration = 0;
    public long Delay;

    public AIAttack()
    {

    }

    public AIAttack(BinaryReader reader, int version = int.MaxValue)
    {
        AttackType = (AIAttackType)reader.ReadByte();
        AttackStyle = (AIAttackStyle)reader.ReadByte();
        AttackEffect = (AIAttackEffect)reader.ReadUInt16();
        Debuff = (AIDebuffType)reader.ReadByte();
        DebuffDuration = reader.ReadByte();
        Steal = reader.ReadBoolean();
        StealPercent = reader.ReadByte();
        StealDuration = reader.ReadByte();
        Delay = reader.ReadInt64();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)AttackType);
        writer.Write((byte)AttackStyle);
        writer.Write((byte)AttackEffect);
        writer.Write((byte)Debuff);
        writer.Write(DebuffDuration);
        writer.Write(Steal);
        writer.Write(StealPercent);
        writer.Write(StealDuration);
        writer.Write(Delay);
    }
}

#endregion

public class ClientQuestCompleted
{
    public int Index;
    public string QuestName;
    public string Category;
    public DateTime CompleteDateTime;

    public ClientQuestCompleted() { }
    public ClientQuestCompleted(BinaryReader reader)
    {
        Index = reader.ReadInt32();
        QuestName = reader.ReadString();
        Category = reader.ReadString();
        CompleteDateTime = DateTime.FromBinary(reader.ReadInt64());
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        writer.Write(QuestName);
        writer.Write(Category);
        writer.Write(CompleteDateTime.ToBinary());
    }
}


public enum EventType : sbyte
{
    None = 0,
    MonsterSlay = 1,
    Invasion = 2,
    DailyBoss = 3,
    WeeklyBoss = 4
}

public class MonsterEventObjective
{
    public string MonsterName;
    public int MonsterTotalCount;
    public int MonsterAliveCount;
    public MonsterEventObjective()
    {

    }
    public MonsterEventObjective(BinaryReader reader)
    {
        MonsterName = reader.ReadString();
        MonsterTotalCount = reader.ReadInt32();
        MonsterAliveCount = reader.ReadInt32();
    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(MonsterName);
        writer.Write(MonsterTotalCount);
        writer.Write(MonsterAliveCount);
    }


}

public class MapQuestLocationClient
{
    public Point Location;
    public QuestIcon Icon = QuestIcon.None;


    public MapQuestLocationClient() { }
    public MapQuestLocationClient(BinaryReader reader)
    {
        Location = new Point(reader.ReadInt32(), reader.ReadInt32());
        Icon = (QuestIcon)reader.ReadByte();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Location.X);
        writer.Write(Location.Y);
        writer.Write((byte)Icon);
    }
}

public class MapEventClientSide
{
    public int Index;
    public Point Location;
    public int Size;
    public bool IsActive;
    public string EventName;
    public EventType EventType = EventType.MonsterSlay;
    public MapEventClientSide()
    {

    }
    public MapEventClientSide(BinaryReader reader)
    {
        Index = reader.ReadInt32();
        Size = reader.ReadInt32();
        IsActive = reader.ReadBoolean();
        EventName = reader.ReadString();

        int x = reader.ReadInt32();
        int y = reader.ReadInt32();
        Location = new Point(x, y);

        EventType = (EventType) reader.ReadSByte();
    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        writer.Write(Size);
        writer.Write(IsActive);
        writer.Write(EventName);

        writer.Write(Location.X);
        writer.Write(Location.Y);
        writer.Write((sbyte) EventType);
    }
}

public class ToolOnlinePlayerListItem
{
    public string PlayerName;
    public ushort PlayerLevel;
    public string PlayerGuild;
    public string PlayerCurrentMap;
    public string PlayerCurrentLocation;
    public ulong AccountGold, AccountCredit;

    public ToolOnlinePlayerListItem(BinaryReader reader)
    {
        PlayerName = reader.ReadString();
        PlayerLevel = reader.ReadUInt16();
        PlayerGuild = reader.ReadString();
        PlayerCurrentMap = reader.ReadString();
        PlayerCurrentLocation = reader.ReadString();
        AccountGold = reader.ReadUInt64();
        AccountCredit = reader.ReadUInt64();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(PlayerName);
        writer.Write(PlayerLevel);
        writer.Write(PlayerGuild);
        writer.Write(PlayerCurrentMap);
        writer.Write(PlayerCurrentLocation);
        writer.Write(AccountGold);
        writer.Write(AccountCredit);
    }
    public ToolOnlinePlayerListItem() { }
}

public class LMS_BR_ClientInfo
{
    public bool SignedUp;
    public byte PlayersSignedUp;
    public LMS_BR_ClientInfo()
    {

    }

    public LMS_BR_ClientInfo(BinaryReader reader)
    {
        SignedUp = reader.ReadBoolean();
        PlayersSignedUp = reader.ReadByte();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(SignedUp);
        writer.Write(PlayersSignedUp);
    }
}

public class SWBuffInfo
{
    /// <summary>
    /// 0 = Flat, 1 = Percent
    /// </summary>
    public byte Type = 0;
    /// <summary>
    /// 0 = Warrior, 1 = Wizard, 2 = Taoist, 3 = Assasin, 4 = Archer
    /// </summary>
    public List<ClassBuffInfo> BuffInfo = new List<ClassBuffInfo>(4);
    /// <summary>
    /// EXP rate boost
    /// </summary>
    public byte EXPBoost = 0;
    /// <summary>
    /// Drop rate boost
    /// </summary>
    public byte DropBoost = 0;
}

public class ClassBuffInfo
{
    public MirClass ClassInfo;
    public byte MinAC, MaxAC;
    public byte MinMAC, MaxMAC;
    public byte MinDC, MaxDC;
    public byte MinMC, MaxMC;
    public byte MinSC, MaxSC;
    public byte Accuracy, ASpeed, Agility;
    public ushort HP, MP;

    public ClassBuffInfo()
    {

    }
}