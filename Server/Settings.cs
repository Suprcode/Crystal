using System.Security.Cryptography;
using Server.MirDatabase;
using Server.MirObjects;

namespace Server
{
    public static class Settings
    {
        private static MessageQueue MessageQueue
        {
            get { return MessageQueue.Instance; }
        }

        public const int Day = 24 * Hour, Hour = 60 * Minute, Minute = 60 * Second, Second = 1000;

        public static readonly string EnvirPath = Path.Combine(".", "Envir"),
            ConfigPath = Path.Combine(".", "Configs"),
            MapPath = Path.Combine(".", "Maps"),
            ExportPath = Path.Combine(".", "Exports"),
            GuildPath = Path.Combine(".", "Guilds"),
            ConquestsPath = Path.Combine(".", "Conquests"),
            NPCPath = Path.Combine(EnvirPath, "NPCs"),
            GoodsPath = Path.Combine(EnvirPath, "Goods"),
            RecipePath = Path.Combine(EnvirPath, "Recipe"),
            QuestPath = Path.Combine(EnvirPath, "Quests"),
            DropPath = Path.Combine(EnvirPath, "Drops"),
            RoutePath = Path.Combine(EnvirPath, "Routes"),
            NameListPath = Path.Combine(EnvirPath, "NameLists"),
            ValuePath = Path.Combine(EnvirPath, "Values"),
            NoticePath = Path.Combine(EnvirPath, "Notice.txt");

        private static readonly InIReader Reader = new InIReader(Path.Combine(ConfigPath, "Setup.ini"));

        public static Notice Notice;

        //General
        public static string VersionPath = Path.Combine(".", "Mir2.Exe");
        public static bool CheckVersion = true;
        public static List<byte[]> VersionHashes;
        public static string GMPassword = "C#Mir 4.0";
        public static bool Multithreaded = true;
        public static int ThreadLimit = 2;
        public static bool TestServer = false;
        public static bool EnforceDBChecks = true;

        public static bool MonsterProcessWhenAlone = false;

        public static string DefaultNPCFilename = "00Default";
        public static string MonsterNPCFilename = "00Monster";
        public static string RobotNPCFilename = "00Robot";
        public static string FishingDropFilename = "00Fishing";
	    public static string AwakeningDropFilename = "00Awakening";
        public static string StrongboxDropFilename = "00Strongbox";
        public static string BlackstoneDropFilename = "00Blackstone";

        //Network
        public static string IPAddress = "127.0.0.1";

        public static ushort Port = 7000,
                             TimeOut = 10000,
                             MaxUser = 50,
                             RelogDelay = 50,
                             MaxIP = 5,
                             MaxPacket = 50;

        public static int IPBlockSeconds = 5;

        //HTTP
        public static bool StartHTTPService = false;
        public static string HTTPIPAddress = "http://127.0.0.1:5679/";
        public static string HTTPTrustedIPAddress = "127.0.0.1";

        //Permission
        public static bool AllowNewAccount = true,
                           AllowChangePassword = true,
                           AllowLogin = true,
                           AllowNewCharacter = true,
                           AllowDeleteCharacter = true,
                           AllowStartGame = false,
                           AllowCreateAssassin = true,
                           AllowCreateArcher = true;

        public static int AllowedResolution = 1024;

        //Optional
        public static bool SafeZoneBorder = false,
                           SafeZoneHealing = false,
                           GameMasterEffect = false,
                           GatherOrbsPerLevel = true,
                           ExpMobLevelDifference = true;
        public static int LineMessageTimer = 10;

        //Database
        public static int SaveDelay = 5;
        public static short CredxGold = 30;

        //Game
        public static List<long> ExperienceList = new List<long>();
        public static List<long> OrbsExpList = new List<long>();
        public static List<long> OrbsDefList = new List<long>();
        public static List<long> OrbsDmgList = new List<long>();

        public static float DropRate = 1F, ExpRate = 1F;
        public static int TeleportToNPCCost = 3000;

        public static int ItemTimeOut = 30,
                          PlayerDiedItemTimeOut = 120,
                          DropRange = 4,
                          DropStackSize = 5,
                          PKDelay = 12;

        public static bool PetSave = false;

        public static int RestedPeriod = 60,
                          RestedBuffLength = 10,
                          RestedExpBonus = 5,
                          RestedMaxBonus = 24,
                          NewbieGuildMaxSize = 1000;

        public static string NewbieGuild = "NewbieGuild",
                             SkeletonName = "BoneFamiliar",
                             ShinsuName = "Shinsu",
                             BugBatName = "BugBat",
                             Zuma1 = "ZumaStatue",
                             Zuma2 = "ZumaGuardian",
                             Zuma3 = "ZumaArcher",
                             Zuma4 = "WedgeMoth",
                             Zuma5 = "ZumaArcher3",
                             Zuma6 = "ZumaStatue3",
                             Zuma7 = "ZumaGuardian3",
                             Turtle1 = "RedTurtle",
                             Turtle2 = "GreenTurtle",
                             Turtle3 = "BlueTurtle",
                             Turtle4 = "TowerTurtle",
                             Turtle5 = "FinialTurtle",
                             BoneMonster1 = "BoneSpearman",
                             BoneMonster2 = "BoneBlademan",
                             BoneMonster3 = "BoneArcher",
                             BoneMonster4 = "BoneCaptain",
                             BehemothMonster1 = "Hugger",
                             BehemothMonster2 = "PoisonHugger",
                             BehemothMonster3 = "MutatedHugger",
                             HellKnight1 = "HellKnight1",
                             HellKnight2 = "HellKnight2",
                             HellKnight3 = "HellKnight3",
                             HellKnight4 = "HellKnight4",
                             HellBomb1 = "HellBomb1",
                             HellBomb2 = "HellBomb2",
                             HellBomb3 = "HellBomb3",
                             WhiteSnake = "WhiteSerpent",
                             AngelName = "HolyDeva",
                             BombSpiderName = "BombSpider",
                             CloneName = "Clone",
                             AssassinCloneName = "AssassinClone",
                             VampireName = "VampireSpider",
                             ToadName = "SpittingToad",
                             SnakeTotemName = "SnakeTotem",
                             SnakesName = "CharmedSnake",
                             AncientBatName = "AncientBat",
                             TucsonGeneralEgg = "GeneralTucson",
                             GeneralMeowMeowMob1 = "StainHammerCat",
                             GeneralMeowMeowMob2 = "BlackHammerCat",
                             GeneralMeowMeowMob3 = "StrayCat",
                             GeneralMeowMeowMob4 = "CatShaman",
                             KingHydraxMob = "Hydrax",
                             HornedCommanderMob = "HornedSorceror",
                             HornedCommanderBombMob = "BoulderSpirit",
                             SnowWolfKingMob = "SnowWolf",
                             ScrollMob1 = "WarriorScroll",
                             ScrollMob2 = "TaoistScroll",
                             ScrollMob3 = "WizardScroll",
                             ScrollMob4 = "AssassinScroll",
                             StoneName = "StoneTrap",
                             HeroName = "Hero";

        public static string HealRing = "Healing",
                             FireRing = "FireBall",
                             ParalysisRing = "Paralysis",
                             BlinkSkill = "Blink";

        public static string PKTownMapName = "3";
        public static int PKTownPositionX = 848,
                          PKTownPositionY = 677;

        public static uint MaxDropGold = 2000;
        public static bool DropGold = true;

        public static string CreatureBlackStoneName = "BlackCreatureStone";

        //Fishing Settings
        public static int FishingAttempts = 30;
        public static int FishingSuccessStart = 10;
        public static int FishingSuccessMultiplier = 10;
        public static long FishingDelay = 0;
        public static int FishingMobSpawnChance = 5;
        public static string FishingMonster = "GiantKeratoid";

        //Mail Settings
        public static bool MailAutoSendGold = false;
        public static bool MailAutoSendItems = false;
        public static bool MailFreeWithStamp = true;
        public static uint MailCostPer1KGold = 100;
        public static uint MailItemInsurancePercentage = 5;
        public static uint MailCapacity = 100;

        //Refine Settings
        public static bool OnlyRefineWeapon = true;
        public static byte RefineBaseChance = 20;
        public static int RefineTime = 20;
        public static byte RefineIncrease = 1;
        public static byte RefineCritChance = 10;
        public static byte RefineCritIncrease = 2;
        public static byte RefineWepStatReduce = 6;
        public static byte RefineItemStatReduce = 15;
        public static int RefineCost = 125;

        public static string RefineOreName = "BlackIronOre";

        //Marriage Settings
        public static int LoverEXPBonus = 5;
        public static int MarriageCooldown = 7;
        public static bool WeddingRingRecall = true;
        public static int MarriageLevelRequired = 10;
        public static int ReplaceWedRingCost = 125;

        //Mentor Settings
        public static byte MentorLevelGap = 10;
        public static bool MentorSkillBoost = true;
        public static byte MentorLength = 7;
        public static byte MentorDamageBoost = 10;
        public static byte MentorExpBoost = 10;
        public static byte MenteeExpBank = 1;

        //Gem Settings
        public static bool GemStatIndependent = true;


        //Goods Settings
        public static bool GoodsOn = true;
        public static uint GoodsMaxStored = 15;
        public static uint GoodsBuyBackTime = 60;
        public static uint GoodsBuyBackMaxStored = 20;
        public static bool GoodsHideAddedStats = true;

        public static BaseStats[] ClassBaseStats = new BaseStats[5] { new BaseStats(MirClass.Warrior), new BaseStats(MirClass.Wizard), new BaseStats(MirClass.Taoist), new BaseStats(MirClass.Assassin), new BaseStats(MirClass.Archer) };

        public static List<RandomItemStat> RandomItemStatsList = new List<RandomItemStat>();
        public static List<MineSet> MineSetList = new List<MineSet>();
        public static WorldMapSetup WorldMapSetup = new WorldMapSetup();

        //item related settings
        public static byte MagicResistWeight = 10,
                            PoisonResistWeight = 10,
                            CriticalRateWeight = 5,
                            CriticalDamageWeight = 50,
                            FreezingAttackWeight = 10,
                            PoisonAttackWeight = 10,
                            HealthRegenWeight = 10,
                            ManaRegenWeight = 10,
                            MaxLuck = 10;

        public static ushort ItemSealDelay = 60;

        public static bool PvpCanResistMagic = false,
                              PvpCanResistPoison = false,
                              PvpCanFreeze = false;

        public static byte RangeAccuracyBonus = 0;

        public static bool AllowNewHero;
        public static byte Hero_RequiredLevel = 22;
        public static bool[] Hero_CanCreateClass = new bool[0];
        public static string HeroSealItemName;
        public static ushort HeroMaximumSealCount;
        public static byte MaximumHeroCount = 1;

        public static bool AllowObserve;

        //Guild related settings
        public static byte Guild_RequiredLevel = 22, Guild_PointPerLevel = 0;
        public static float Guild_ExpRate = 0.01f;
        public static uint Guild_WarCost = 3000;
        public static long Guild_WarTime = 180;

        public static List<GuildItemVolume> Guild_CreationCostList = new List<GuildItemVolume>();
        public static List<long> Guild_ExperienceList = new List<long>();
        public static List<int> Guild_MembercapList = new List<int>();
        public static List<GuildBuffInfo> Guild_BuffList = new List<GuildBuffInfo>();
        public static long GroupInviteDelay { get; internal set; } = 2000;
        public static long TradeDelay { get; internal set; } = 2000;

        //Archive Settings
        public static int ArchiveInactiveCharacterAfterMonths = 12;
        public static int ArchiveDeletedCharacterAfterMonths = 1;

        public static void LoadVersion()
        {
            try
            {
                VersionHashes = new List<byte[]>();

                var paths = VersionPath.Split(',');

                //foreach (var path in paths)
                //{
                //    if (File.Exists(path))
                //        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                //        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                //            VersionHashes.Add(md5.ComputeHash(stream));
                //}

                foreach (var path in paths)
                {
                    if (File.Exists(path))
                        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                        using (MD5 md5 = MD5.Create())
                            VersionHashes.Add(md5.ComputeHash(stream));
                }

            }
            catch (Exception ex)
            {
                MessageQueue.Enqueue(ex);
            }
        }

        public static void Load()
        {            
            //General
            VersionPath = Reader.ReadString("General", "VersionPath", VersionPath);
            CheckVersion = Reader.ReadBoolean("General", "CheckVersion", CheckVersion);
            RelogDelay = Reader.ReadUInt16("General", "RelogDelay", RelogDelay);
            GMPassword = Reader.ReadString("General", "GMPassword", GMPassword);
            Multithreaded = Reader.ReadBoolean("General", "Multithreaded", Multithreaded);
            ThreadLimit = Reader.ReadInt32("General", "ThreadLimit", ThreadLimit);
            TestServer = Reader.ReadBoolean("General", "TestServer", TestServer);
            EnforceDBChecks = Reader.ReadBoolean("General", "EnforceDBChecks", EnforceDBChecks);
            MonsterProcessWhenAlone = Reader.ReadBoolean("General", "MonsterProcessWhenAlone", MonsterProcessWhenAlone);

            //Paths
            IPAddress = Reader.ReadString("Network", "IPAddress", IPAddress);
            Port = Reader.ReadUInt16("Network", "Port", Port);
            TimeOut = Reader.ReadUInt16("Network", "TimeOut", TimeOut);
            MaxUser = Reader.ReadUInt16("Network", "MaxUser", MaxUser);
            MaxIP = Reader.ReadUInt16("Network", "MaxIP", MaxIP);
            MaxPacket = Reader.ReadUInt16("Network", "MaxPacket", MaxPacket);

            //HTTP
            StartHTTPService = Reader.ReadBoolean("Network", "StartHTTPService", StartHTTPService);
            HTTPIPAddress = Reader.ReadString("Network", "HTTPIPAddress", HTTPIPAddress);
            HTTPTrustedIPAddress = Reader.ReadString("Network", "HTTPTrustedIPAddress", HTTPTrustedIPAddress);

            //Permission
            AllowNewAccount = Reader.ReadBoolean("Permission", "AllowNewAccount", AllowNewAccount);
            AllowChangePassword = Reader.ReadBoolean("Permission", "AllowChangePassword", AllowChangePassword);
            AllowLogin = Reader.ReadBoolean("Permission", "AllowLogin", AllowLogin);
            AllowNewCharacter = Reader.ReadBoolean("Permission", "AllowNewCharacter", AllowNewCharacter);
            AllowDeleteCharacter = Reader.ReadBoolean("Permission", "AllowDeleteCharacter", AllowDeleteCharacter);
            AllowStartGame = Reader.ReadBoolean("Permission", "AllowStartGame", AllowStartGame);
            AllowCreateAssassin = Reader.ReadBoolean("Permission", "AllowCreateAssassin", AllowCreateAssassin);
            AllowCreateArcher = Reader.ReadBoolean("Permission", "AllowCreateArcher", AllowCreateArcher);
            AllowedResolution = Reader.ReadInt32("Permission", "MaxResolution", AllowedResolution);

            //Optional
            SafeZoneBorder = Reader.ReadBoolean("Optional", "SafeZoneBorder", SafeZoneBorder);
            SafeZoneHealing = Reader.ReadBoolean("Optional", "SafeZoneHealing", SafeZoneHealing);
            GatherOrbsPerLevel = Reader.ReadBoolean("Optional", "GatherOrbsPerLevel", GatherOrbsPerLevel);
            ExpMobLevelDifference = Reader.ReadBoolean("Optional", "ExpMobLevelDifference", ExpMobLevelDifference);
            GameMasterEffect = Reader.ReadBoolean("Optional", "GameMasterEffect", GameMasterEffect);
            LineMessageTimer = Reader.ReadInt32("Optional", "LineMessageTimer", LineMessageTimer);

            //Database
            SaveDelay = Reader.ReadInt32("Database", "SaveDelay", SaveDelay);
            CredxGold = Reader.ReadInt16("Database", "CredxGold", CredxGold);

            //Game
            DropRate = Reader.ReadSingle("Game", "DropRate", DropRate);
            ExpRate = Reader.ReadSingle("Game", "ExpRate", ExpRate);
            ItemTimeOut = Reader.ReadInt32("Game", "ItemTimeOut", ItemTimeOut);
            PlayerDiedItemTimeOut = Reader.ReadInt32("Game", "PlayerDiedItemTimeOut", PlayerDiedItemTimeOut);
            PetSave = Reader.ReadBoolean("Game", "PetSave", PetSave);
            PKDelay = Reader.ReadInt32("Game", "PKDelay", PKDelay);
            NewbieGuild = Reader.ReadString("Game", "NewbieGuild", NewbieGuild);
            NewbieGuildMaxSize = Reader.ReadInt32("Game", "NewbieGuildMaxSize", NewbieGuildMaxSize);
            SkeletonName = Reader.ReadString("Game", "SkeletonName", SkeletonName);
            BugBatName = Reader.ReadString("Game", "BugBatName", BugBatName);
            ShinsuName = Reader.ReadString("Game", "ShinsuName", ShinsuName);
            Zuma1 = Reader.ReadString("Game", "Zuma1", Zuma1);
            Zuma2 = Reader.ReadString("Game", "Zuma2", Zuma2);
            Zuma3 = Reader.ReadString("Game", "Zuma3", Zuma3);
            Zuma4 = Reader.ReadString("Game", "Zuma4", Zuma4);
            Zuma5 = Reader.ReadString("Game", "Zuma5", Zuma5);
            Zuma6 = Reader.ReadString("Game", "Zuma6", Zuma6);
            Zuma7 = Reader.ReadString("Game", "Zuma7", Zuma7);
            Turtle1 = Reader.ReadString("Game", "Turtle1", Turtle1);
            Turtle2 = Reader.ReadString("Game", "Turtle2", Turtle2);
            Turtle3 = Reader.ReadString("Game", "Turtle3", Turtle3);
            Turtle4 = Reader.ReadString("Game", "Turtle4", Turtle4);
            Turtle5 = Reader.ReadString("Game", "Turtle5", Turtle5);
            BoneMonster1 = Reader.ReadString("Game", "BoneMonster1", BoneMonster1);
            BoneMonster2 = Reader.ReadString("Game", "BoneMonster2", BoneMonster2);
            BoneMonster3 = Reader.ReadString("Game", "BoneMonster3", BoneMonster3);
            BoneMonster4 = Reader.ReadString("Game", "BoneMonster4", BoneMonster4);
            BehemothMonster1 = Reader.ReadString("Game", "BehemothMonster1", BehemothMonster1);
            BehemothMonster2 = Reader.ReadString("Game", "BehemothMonster2", BehemothMonster2);
            BehemothMonster3 = Reader.ReadString("Game", "BehemothMonster3", BehemothMonster3);
            HellKnight1 = Reader.ReadString("Game", "HellKnight1", HellKnight1);
            HellKnight2 = Reader.ReadString("Game", "HellKnight2", HellKnight2);
            HellKnight3 = Reader.ReadString("Game", "HellKnight3", HellKnight3);
            HellKnight4 = Reader.ReadString("Game", "HellKnight4", HellKnight4);
            HellBomb1 = Reader.ReadString("Game", "HellBomb1", HellBomb1);
            HellBomb2 = Reader.ReadString("Game", "HellBomb2", HellBomb2);
            HellBomb3 = Reader.ReadString("Game", "HellBomb3", HellBomb3);
            GeneralMeowMeowMob1 = Reader.ReadString("Game", "GeneralMeowMeowMob1", GeneralMeowMeowMob1);
            GeneralMeowMeowMob2 = Reader.ReadString("Game", "GeneralMeowMeowMob2", GeneralMeowMeowMob2);
            GeneralMeowMeowMob3 = Reader.ReadString("Game", "GeneralMeowMeowMob3", GeneralMeowMeowMob3);
            GeneralMeowMeowMob4 = Reader.ReadString("Game", "GeneralMeowMeowMob4", GeneralMeowMeowMob4);
            KingHydraxMob = Reader.ReadString("Game", "KingHydraxMob", KingHydraxMob);
            HornedCommanderMob = Reader.ReadString("Game", "HornedCommanderMob", HornedCommanderMob);
            HornedCommanderBombMob = Reader.ReadString("Game", "HornedCommanderBombMob", HornedCommanderBombMob);
            SnowWolfKingMob = Reader.ReadString("Game", "SnowWolfKingMob", SnowWolfKingMob);
            ScrollMob1 = Reader.ReadString("Game", "ScrollMob1", ScrollMob1);
            ScrollMob2 = Reader.ReadString("Game", "ScrollMob2", ScrollMob2);
            ScrollMob3 = Reader.ReadString("Game", "ScrollMob3", ScrollMob3);
            ScrollMob4 = Reader.ReadString("Game", "ScrollMob4", ScrollMob4);
            WhiteSnake = Reader.ReadString("Game", "WhiteSnake", WhiteSnake);
            AngelName = Reader.ReadString("Game", "AngelName", AngelName);
            BombSpiderName = Reader.ReadString("Game", "BombSpiderName", BombSpiderName);
            CloneName = Reader.ReadString("Game", "CloneName", CloneName);
            FishingMonster = Reader.ReadString("Game", "FishMonster", FishingMonster);
            AssassinCloneName = Reader.ReadString("Game", "AssassinCloneName", AssassinCloneName);
            VampireName = Reader.ReadString("Game", "VampireName", VampireName);
            ToadName = Reader.ReadString("Game", "ToadName", ToadName);
            SnakeTotemName = Reader.ReadString("Game", "SnakeTotemName", SnakeTotemName);
            SnakesName = Reader.ReadString("Game", "SnakesName", SnakesName);
            StoneName = Reader.ReadString("Game", "StoneName", StoneName);
            AncientBatName = Reader.ReadString("Game", "AncientBatName", AncientBatName);
            TucsonGeneralEgg = Reader.ReadString("Game", "TucsonGeneralEgg", TucsonGeneralEgg);
            GroupInviteDelay = Reader.ReadInt64("Game", "GroupInviteDelay", GroupInviteDelay);
            TradeDelay = Reader.ReadInt64("Game", "TradeDelay", TradeDelay);
            TeleportToNPCCost = Reader.ReadInt32("Game", "TeleportToNPCCost", TeleportToNPCCost);
            HeroName = Reader.ReadString("Game", "HeroName", HeroName);

            //Rested
            RestedPeriod = Reader.ReadInt32("Rested", "Period", RestedPeriod);
            RestedBuffLength = Reader.ReadInt32("Rested", "BuffLength", RestedBuffLength);
            RestedExpBonus = Reader.ReadInt32("Rested", "ExpBonus", RestedExpBonus);
            RestedMaxBonus = Reader.ReadInt32("Rested", "MaxBonus", RestedMaxBonus);

            //Items
            HealRing = Reader.ReadString("Items", "HealRing", HealRing);
            FireRing = Reader.ReadString("Items", "FireRing", FireRing);
            BlinkSkill = Reader.ReadString("Items", "BlinkSkill", BlinkSkill);

            //PKTown
            PKTownMapName = Reader.ReadString("PKTown", "PKTownMapName", PKTownMapName);
            PKTownPositionX = Reader.ReadInt32("PKTown", "PKTownPositionX", PKTownPositionX);
            PKTownPositionY = Reader.ReadInt32("PKTown", "PKTownPositionY", PKTownPositionY);

            DropGold = Reader.ReadBoolean("DropGold", "DropGold", DropGold);
            MaxDropGold = Reader.ReadUInt32("DropGold", "MaxDropGold", MaxDropGold);

            MagicResistWeight = Reader.ReadByte("Items","MagicResistWeight",MagicResistWeight);
            PoisonResistWeight = Reader.ReadByte("Items","PoisonResistWeight",PoisonResistWeight);
            CriticalRateWeight = Reader.ReadByte("Items","CriticalRateWeight",CriticalRateWeight);
            CriticalDamageWeight = Math.Max((byte)1, Reader.ReadByte("Items","CriticalDamageWeight",CriticalDamageWeight));
            FreezingAttackWeight = Reader.ReadByte("Items","FreezingAttackWeight",FreezingAttackWeight);
            PoisonAttackWeight = Reader.ReadByte("Items","PoisonAttackWeight",PoisonAttackWeight);
            HealthRegenWeight = Math.Max((byte)1, Reader.ReadByte("Items", "HealthRegenWeight", HealthRegenWeight));
            ManaRegenWeight = Math.Max((byte)1, Reader.ReadByte("Items", "ManaRegenWeight", ManaRegenWeight));
            MaxLuck = Reader.ReadByte("Items", "MaxLuck", MaxLuck);
            ItemSealDelay = Reader.ReadUInt16("Items", "SealDelay", ItemSealDelay);

            PvpCanResistMagic = Reader.ReadBoolean("Items","PvpCanResistMagic",PvpCanResistMagic);
            PvpCanResistPoison = Reader.ReadBoolean("Items", "PvpCanResistPoison", PvpCanResistPoison);
            PvpCanFreeze = Reader.ReadBoolean("Items", "PvpCanFreeze", PvpCanFreeze);

            RangeAccuracyBonus = Reader.ReadByte("Bonus", "RangeAccuracyBonus", RangeAccuracyBonus);

            CreatureBlackStoneName = Reader.ReadString("IntelligentCreatures", "CreatureBlackStoneName", CreatureBlackStoneName);

            AllowObserve = Reader.ReadBoolean("Observe", "AllowObserve", AllowObserve);

            //Archive
            ArchiveInactiveCharacterAfterMonths = Math.Max(1, Reader.ReadInt32("Archive", "InactiveCharacterMonths", ArchiveInactiveCharacterAfterMonths));
            ArchiveDeletedCharacterAfterMonths = Math.Max(1, Reader.ReadInt32("Archive", "DeletedCharacterMonths", ArchiveDeletedCharacterAfterMonths));

            if (!Directory.Exists(EnvirPath))
                Directory.CreateDirectory(EnvirPath);
            if (!Directory.Exists(ConfigPath))
                Directory.CreateDirectory(ConfigPath);

            if (!Directory.Exists(MapPath))
                Directory.CreateDirectory(MapPath);
            if (!Directory.Exists(NPCPath))
                Directory.CreateDirectory(NPCPath);
            if (!Directory.Exists(GoodsPath))
                Directory.CreateDirectory(GoodsPath);
            if (!Directory.Exists(QuestPath))
                Directory.CreateDirectory(QuestPath);
            if (!Directory.Exists(DropPath))
                Directory.CreateDirectory(DropPath);
            if (!Directory.Exists(ExportPath))
                Directory.CreateDirectory(ExportPath);
            if (!Directory.Exists(RoutePath))
                Directory.CreateDirectory(RoutePath);         
            if (!Directory.Exists(NameListPath))
                Directory.CreateDirectory(NameListPath);
            if (!Directory.Exists(RecipePath))
                Directory.CreateDirectory(RecipePath);

            string fileName = Path.Combine(Settings.NPCPath, DefaultNPCFilename + ".txt");

            if (!File.Exists(fileName))
            {
                FileStream NewFile = File.Create(fileName);
                NewFile.Close();
            }

            fileName = Path.Combine(Settings.NPCPath, MonsterNPCFilename + ".txt");

            if (!File.Exists(fileName))
            {
                FileStream NewFile = File.Create(fileName);
                NewFile.Close();
            }

            fileName = Path.Combine(Settings.NPCPath, RobotNPCFilename + ".txt");

            if (!File.Exists(fileName))
            {
                FileStream NewFile = File.Create(fileName);
                NewFile.Close();
            }

            LoadVersion();
            LoadEXP();
            LoadBaseStats();
            LoadRandomItemStats();
            LoadMines();
            LoadGuildSettings();
			LoadAwakeAttribute();
            LoadFishing();
            LoadMail();
            LoadRefine();
            LoadMarriage();
            LoadMentor();
            LoadGoods();
            LoadGem();
            LoadNotice();
            LoadWorldMap();
            LoadHeroSettings();

            GameLanguage.LoadServerLanguage(Path.Combine(ConfigPath, "Language.ini"));
        }

        public static void LoadNotice()
        {
            Notice = new Notice();

            if (!File.Exists(NoticePath))
            {
                FileStream NewFile = File.Create(NoticePath);
                NewFile.Close();
            }

            var lines = File.ReadAllLines(NoticePath);

            if (lines.Length == 0)
            {
                return;
            }

            Notice.LastUpdate = File.GetLastWriteTime(NoticePath);

            var links = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (string.Compare(line, "TITLE", false) > 0 && line.Contains("="))
                {
                    Notice.Title = line.Split('=')[1];
                    continue;
                }

                Notice.Message += line + "\r\n";
            }
        }

        public static void Save()
        {
            //General
            Reader.Write("General", "VersionPath", VersionPath);
            Reader.Write("General", "CheckVersion", CheckVersion);
            Reader.Write("General", "RelogDelay", RelogDelay);
            Reader.Write("General", "Multithreaded", Multithreaded);
            Reader.Write("General", "ThreadLimit", ThreadLimit);
            Reader.Write("General", "TestServer", TestServer);
            Reader.Write("General", "EnforceDBChecks", EnforceDBChecks);
            Reader.Write("General", "MonsterProcessWhenAlone", MonsterProcessWhenAlone);

            //Paths
            Reader.Write("Network", "IPAddress", IPAddress);
            Reader.Write("Network", "Port", Port);
            Reader.Write("Network", "TimeOut", TimeOut);
            Reader.Write("Network", "MaxUser", MaxUser);
            Reader.Write("Network", "MaxIP", MaxIP);

            //HTTP
            Reader.Write("Network", "StartHTTPService", StartHTTPService);
            Reader.Write("Network", "HTTPIPAddress", HTTPIPAddress);
            Reader.Write("Network", "HTTPTrustedIPAddress", HTTPTrustedIPAddress);

            //Permission
            Reader.Write("Permission", "AllowNewAccount", AllowNewAccount);
            Reader.Write("Permission", "AllowChangePassword", AllowChangePassword);
            Reader.Write("Permission", "AllowLogin", AllowLogin);
            Reader.Write("Permission", "AllowNewCharacter", AllowNewCharacter);
            Reader.Write("Permission", "AllowDeleteCharacter", AllowDeleteCharacter);
            Reader.Write("Permission", "AllowStartGame", AllowStartGame);
            Reader.Write("Permission", "AllowCreateAssassin", AllowCreateAssassin);
            Reader.Write("Permission", "AllowCreateArcher", AllowCreateArcher);
            Reader.Write("Permission", "MaxResolution", AllowedResolution);

            //Optional
            Reader.Write("Optional", "SafeZoneBorder", SafeZoneBorder);
            Reader.Write("Optional", "SafeZoneHealing", SafeZoneHealing);
            Reader.Write("Optional", "GatherOrbsPerLevel", GatherOrbsPerLevel);
            Reader.Write("Optional", "ExpMobLevelDifference", ExpMobLevelDifference);
            Reader.Write("Optional", "GameMasterEffect", GameMasterEffect);
            Reader.Write("Optional", "LineMessageTimer", LineMessageTimer);

            //Database
            Reader.Write("Database", "SaveDelay", SaveDelay);
            Reader.Write("Database", "CredxGold", CredxGold);

            //Game
            Reader.Write("Game", "DropRate", DropRate);
            Reader.Write("Game", "ExpRate", ExpRate);
            Reader.Write("Game", "ItemTimeOut", ItemTimeOut);
            Reader.Write("Game", "PlayerDiedItemTimeOut", PlayerDiedItemTimeOut);
            Reader.Write("Game", "PetSave", PetSave);
            Reader.Write("Game", "PKDelay", PKDelay);
            Reader.Write("Game", "NewbieGuild", NewbieGuild);
            Reader.Write("Game", "NewbieGuildMaxSize", NewbieGuildMaxSize);
            Reader.Write("Game", "SkeletonName", SkeletonName);
            Reader.Write("Game", "BugBatName", BugBatName);
            Reader.Write("Game", "ShinsuName", ShinsuName);

            Reader.Write("Game", "Zuma1", Zuma1);
            Reader.Write("Game", "Zuma2", Zuma2);
            Reader.Write("Game", "Zuma3", Zuma3);
            Reader.Write("Game", "Zuma4", Zuma4);
            Reader.Write("Game", "Zuma5", Zuma5);
            Reader.Write("Game", "Zuma6", Zuma6);
            Reader.Write("Game", "Zuma7", Zuma7);

            Reader.Write("Game", "Turtle1", Turtle1);
            Reader.Write("Game", "Turtle2", Turtle2);
            Reader.Write("Game", "Turtle3", Turtle3);
            Reader.Write("Game", "Turtle4", Turtle4);
            Reader.Write("Game", "Turtle5", Turtle5);

            Reader.Write("Game", "BoneMonster1", BoneMonster1);
            Reader.Write("Game", "BoneMonster2", BoneMonster2);
            Reader.Write("Game", "BoneMonster3", BoneMonster3);
            Reader.Write("Game", "BoneMonster4", BoneMonster4);

            Reader.Write("Game", "BehemothMonster1", BehemothMonster1);
            Reader.Write("Game", "BehemothMonster2", BehemothMonster2);
            Reader.Write("Game", "BehemothMonster3", BehemothMonster3);

            Reader.Write("Game", "HellKnight1", HellKnight1);
            Reader.Write("Game", "HellKnight2", HellKnight2);
            Reader.Write("Game", "HellKnight3", HellKnight3);
            Reader.Write("Game", "HellKnight4", HellKnight4);
            Reader.Write("Game", "HellBomb1", HellBomb1);
            Reader.Write("Game", "HellBomb2", HellBomb2);
            Reader.Write("Game", "HellBomb3", HellBomb3);

            Reader.Write("Game", "WhiteSnake", WhiteSnake);
            Reader.Write("Game", "AngelName", AngelName);
            Reader.Write("Game", "BombSpiderName", BombSpiderName);
            Reader.Write("Game", "CloneName", CloneName);
            Reader.Write("Game", "AssassinCloneName", AssassinCloneName);

            Reader.Write("Game", "VampireName", VampireName);
            Reader.Write("Game", "ToadName", ToadName);
            Reader.Write("Game", "SnakeTotemName", SnakeTotemName);
            Reader.Write("Game", "SnakesName", SnakesName);
            Reader.Write("Game", "StoneName", StoneName);
            Reader.Write("Game", "AncientBatName", AncientBatName);
            Reader.Write("Game", "TucsonGeneralEgg", TucsonGeneralEgg);
            Reader.Write("Game", "GroupInviteDelay", GroupInviteDelay);
            Reader.Write("Game", "TradeDelay", TradeDelay);

            Reader.Write("Rested", "Period", RestedPeriod);
            Reader.Write("Rested", "BuffLength", RestedBuffLength);
            Reader.Write("Rested", "ExpBonus", RestedExpBonus);
            Reader.Write("Rested", "MaxBonus", RestedMaxBonus);

            Reader.Write("Items", "HealRing", HealRing);
            Reader.Write("Items", "FireRing", FireRing);
            Reader.Write("Items", "BlinkSkill", BlinkSkill);

            Reader.Write("PKTown", "PKTownMapName", PKTownMapName);
            Reader.Write("PKTown", "PKTownPositionX", PKTownPositionX);
            Reader.Write("PKTown", "PKTownPositionY", PKTownPositionY);

            Reader.Write("DropGold", "DropGold", DropGold);
            Reader.Write("DropGold", "MaxDropGold", MaxDropGold);
            
            Reader.Write("Items", "MagicResistWeight", MagicResistWeight);
            Reader.Write("Items", "PoisonResistWeight", PoisonResistWeight);
            Reader.Write("Items", "CriticalRateWeight", CriticalRateWeight);
            Reader.Write("Items", "CriticalDamageWeight", CriticalDamageWeight);
            Reader.Write("Items", "FreezingAttackWeight", FreezingAttackWeight);
            Reader.Write("Items", "PoisonAttackWeight", PoisonAttackWeight);
            Reader.Write("Items", "HealthRegenWeight", HealthRegenWeight);
            Reader.Write("Items", "ManaRegenWeight", ManaRegenWeight);
            Reader.Write("Items", "MaxLuck", MaxLuck);
            Reader.Write("Items", "SealDelay", ItemSealDelay);

            Reader.Write("Items", "PvpCanResistMagic", PvpCanResistMagic);
            Reader.Write("Items", "PvpCanResistPoison", PvpCanResistPoison);
            Reader.Write("Items", "PvpCanFreeze", PvpCanFreeze);

            Reader.Write("Bonus", "RangeAccuracyBonus", RangeAccuracyBonus);

            Reader.Write("Observe", "AllowObserve", AllowObserve);

            Reader.Write("Game", "GeneralMeowMeowMob1", GeneralMeowMeowMob1);
            Reader.Write("Game", "GeneralMeowMeowMob2", GeneralMeowMeowMob2);
            Reader.Write("Game", "GeneralMeowMeowMob3", GeneralMeowMeowMob3);
            Reader.Write("Game", "GeneralMeowMeowMob4", GeneralMeowMeowMob4);

            Reader.Write("Game", "KingHydraxMob", KingHydraxMob);
            Reader.Write("Game", "HornedCommanderMob", HornedCommanderMob);
            Reader.Write("Game", "HornedCommanderBombMob", HornedCommanderBombMob);
            Reader.Write("Game", "SnowWolfKingMob", SnowWolfKingMob);

            Reader.Write("Game", "ScrollMob1", ScrollMob1);
            Reader.Write("Game", "ScrollMob2", ScrollMob2);
            Reader.Write("Game", "ScrollMob3", ScrollMob3);
            Reader.Write("Game", "ScrollMob4", ScrollMob4);

            Reader.Write("IntelligentCreatures", "CreatureBlackStoneName", CreatureBlackStoneName);

            //Archive
            Reader.Write("Archive", "InactiveCharacterMonths", ArchiveInactiveCharacterAfterMonths);
            Reader.Write("Archive", "DeletedCharacterMonths", ArchiveDeletedCharacterAfterMonths);

            Reader.Write("Game", "TeleportToNPCCost", TeleportToNPCCost);

            Reader.Write("Game", "HeroName", HeroName);

            SaveAwakeAttribute();
        }

        public static void LoadEXP()
        {
            long exp = 100;
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "ExpList.ini"));

            for (int i = 1; i <= 500; i++)
            {
                exp = reader.ReadInt64("Exp", "Level" + i, exp);
                ExperienceList.Add(exp);
            }

            //ArcherSpells - Elemental system
            reader = new InIReader(Path.Combine(ConfigPath, "OrbsExpList.ini"));
            for (int i = 1; i <= 4; i++)
            {
                exp = i * 50;//default exp value
                exp = reader.ReadInt64("Exp", "Orb" + i, exp);
                OrbsExpList.Add(exp);
                exp = i * 2;//default defense value
                exp = reader.ReadInt64("Def", "Orb" + i, exp);
                OrbsDefList.Add(exp);
                exp = i * 4;//default power value
                exp = reader.ReadInt64("Att", "Orb" + i, exp);
                OrbsDmgList.Add(exp);
            }
        }

        public static void LoadWorldMap()
        {
            InIReader reader = null;
            string path = Path.Combine(ConfigPath, "WorldMap.ini");
            if (!File.Exists(path))
            {
                FileStream newFile = File.Create(path);
                newFile.Close();
                reader = new InIReader(path);
                reader.Write("Setup", "Enabled", false);
                reader.Write("Layout", "Button0ImageIndex", "");
                reader.Write("Layout", "Button0Title", "");
                reader.Write("Layout", "Button0MapIndex", "");
            }

            if (reader == null)
                reader = new InIReader(path);
            
            WorldMapSetup.Enabled = reader.ReadBoolean("Setup", "Enabled", false);

            int c = 0;
            while (true)
            {
                int imageIndex = reader.ReadInt32("Layout", $"Button{c}ImageIndex", -1, false);
                if (imageIndex == -1)
                    break;

                WorldMapIcon icon = new WorldMapIcon()
                {
                    ImageIndex = imageIndex,
                    Title = reader.ReadString("Layout", $"Button{c}Title", ""),
                    MapIndex = reader.ReadInt32("Layout", $"Button{c}MapIndex", 0)
                };
                WorldMapSetup.Icons.Add(icon);
                c++;
            }
        }

        public static void LoadBaseStats()
        {
            for (int i = 0; i < ClassBaseStats.Length; i++)
            {
                if (!File.Exists(Path.Combine(ConfigPath, $"BaseStats{ClassBaseStats[i].Job}.ini")))
                {
                    SaveBaseStats(new BaseStats[1] { new BaseStats(ClassBaseStats[i].Job) });
                    continue;
                }

                InIReader reader = new InIReader(Path.Combine(ConfigPath, $"BaseStats{ClassBaseStats[i].Job}.ini"));

                ClassBaseStats[i].Stats.Clear();
                ClassBaseStats[i].Caps.Clear();

                foreach (var stat in Enum.GetValues(typeof(Stat)))
                {
                    var key = stat.ToString();

                    var formula = reader.ReadString(key, "Formula", null, false);

                    if (!string.IsNullOrEmpty(formula))
                    {
                        var baseStat = new BaseStat((Stat)stat)
                        {
                            FormulaType = (StatFormula)Enum.Parse(typeof(StatFormula), formula, true),
                            Base = reader.ReadInt32(key, "Base", 0),
                            Gain = reader.ReadFloat(key, "Gain", 0),
                            GainRate = reader.ReadFloat(key, "GainRate", 0),
                            Max = reader.ReadInt32(key, "Max", 0)
                        };

                        ClassBaseStats[i].Stats.Add(baseStat);
                    }

                    ClassBaseStats[i].Caps[(Stat)stat] = reader.ReadInt32("Caps", key, 0, false);
                }
            }
        }

        public static void SaveBaseStats(BaseStats[] classStats = null)
        {
            if (classStats == null)
            {
                classStats = ClassBaseStats;
            }

            foreach (var baseStats in classStats)
            {
                File.Delete(Path.Combine(ConfigPath, $"BaseStats{baseStats.Job}.ini"));
                InIReader reader = new InIReader(Path.Combine(ConfigPath, $"BaseStats{baseStats.Job}.ini"));

                foreach (var stat in baseStats.Stats)
                {
                    reader.Write(stat.Type.ToString(), "Formula", stat.FormulaType.ToString());
                    reader.Write(stat.Type.ToString(), "Base", stat.Base.ToString());
                    reader.Write(stat.Type.ToString(), "Gain", stat.Gain.ToString());
                    reader.Write(stat.Type.ToString(), "GainRate", stat.GainRate.ToString());
                    reader.Write(stat.Type.ToString(), "Max", stat.Max.ToString());
                }

                foreach (var item in baseStats.Caps.Values)
                {
                    reader.Write("Caps", item.Key.ToString(), item.Value);
                }
            }
        }

        public static void LoadRandomItemStats()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "RandomItemStats.ini")))
            {
                RandomItemStatsList.Add(new RandomItemStat());
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Weapon));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Armour));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Helmet));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Necklace));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Bracelet));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Ring));
                RandomItemStatsList.Add(new RandomItemStat(ItemType.Belt));
                SaveRandomItemStats();
                return;
            }

            InIReader reader = new InIReader(Path.Combine(ConfigPath, "RandomItemStats.ini"));
            int i = 0;
            RandomItemStat stat;
            while (reader.ReadByte("Item" + i.ToString(),"MaxDuraChance",255) != 255)
            {
                stat = new RandomItemStat();
                stat.MaxDuraChance = reader.ReadByte("Item" + i.ToString(), "MaxDuraChance", 0);
                stat.MaxDuraStatChance = reader.ReadByte("Item" + i.ToString(), "MaxDuraStatChance", 1);
                stat.MaxDuraMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxDuraMaxStat", 1);
                stat.MaxAcChance = reader.ReadByte("Item" + i.ToString(), "MaxAcChance", 0);
                stat.MaxAcStatChance = reader.ReadByte("Item" + i.ToString(), "MaxAcStatChance", 1);
                stat.MaxAcMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxAcMaxStat", 1);
                stat.MaxMacChance = reader.ReadByte("Item" + i.ToString(), "MaxMacChance", 0);
                stat.MaxMacStatChance = reader.ReadByte("Item" + i.ToString(), "MaxMacStatChance", 1);
                stat.MaxMacMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxMACMaxStat", 1);
                stat.MaxDcChance = reader.ReadByte("Item" + i.ToString(), "MaxDcChance", 0);
                stat.MaxDcStatChance = reader.ReadByte("Item" + i.ToString(), "MaxDcStatChance", 1);
                stat.MaxDcMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxDcMaxStat", 1);
                stat.MaxMcChance = reader.ReadByte("Item" + i.ToString(), "MaxMcChance", 0);
                stat.MaxMcStatChance = reader.ReadByte("Item" + i.ToString(), "MaxMcStatChance", 1);
                stat.MaxMcMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxMcMaxStat", 1);
                stat.MaxScChance = reader.ReadByte("Item" + i.ToString(), "MaxScChance", 0);
                stat.MaxScStatChance = reader.ReadByte("Item" + i.ToString(), "MaxScStatChance", 1);
                stat.MaxScMaxStat = reader.ReadByte("Item" + i.ToString(), "MaxScMaxStat", 1);
                stat.AccuracyChance = reader.ReadByte("Item" + i.ToString(), "AccuracyChance", 0);
                stat.AccuracyStatChance = reader.ReadByte("Item" + i.ToString(), "AccuracyStatChance", 1);
                stat.AccuracyMaxStat = reader.ReadByte("Item" + i.ToString(), "AccuracyMaxStat", 1);
                stat.AgilityChance = reader.ReadByte("Item" + i.ToString(), "AgilityChance", 0);
                stat.AgilityStatChance = reader.ReadByte("Item" + i.ToString(), "AgilityStatChance", 1);
                stat.AgilityMaxStat = reader.ReadByte("Item" + i.ToString(), "AgilityMaxStat", 1);
                stat.HpChance = reader.ReadByte("Item" + i.ToString(), "HpChance", 0);
                stat.HpStatChance = reader.ReadByte("Item" + i.ToString(), "HpStatChance", 1);
                stat.HpMaxStat = reader.ReadByte("Item" + i.ToString(), "HpMaxStat", 1);
                stat.MpChance = reader.ReadByte("Item" + i.ToString(), "MpChance", 0);
                stat.MpStatChance = reader.ReadByte("Item" + i.ToString(), "MpStatChance", 1);
                stat.MpMaxStat = reader.ReadByte("Item" + i.ToString(), "MpMaxStat", 1);
                stat.StrongChance = reader.ReadByte("Item" + i.ToString(), "StrongChance", 0);
                stat.StrongStatChance = reader.ReadByte("Item" + i.ToString(), "StrongStatChance", 1);
                stat.StrongMaxStat = reader.ReadByte("Item" + i.ToString(), "StrongMaxStat", 1);
                stat.MagicResistChance = reader.ReadByte("Item" + i.ToString(), "MagicResistChance", 0);
                stat.MagicResistStatChance = reader.ReadByte("Item" + i.ToString(), "MagicResistStatChance", 1);
                stat.MagicResistMaxStat = reader.ReadByte("Item" + i.ToString(), "MagicResistMaxStat", 1);
                stat.PoisonResistChance = reader.ReadByte("Item" + i.ToString(), "PoisonResistChance", 0);
                stat.PoisonResistStatChance = reader.ReadByte("Item" + i.ToString(), "PoisonResistStatChance", 1);
                stat.PoisonResistMaxStat = reader.ReadByte("Item" + i.ToString(), "PoisonResistMaxStat", 1);
                stat.HpRecovChance = reader.ReadByte("Item" + i.ToString(), "HpRecovChance", 0);
                stat.HpRecovStatChance = reader.ReadByte("Item" + i.ToString(), "HpRecovStatChance", 1);
                stat.HpRecovMaxStat = reader.ReadByte("Item" + i.ToString(), "HpRecovMaxStat", 1);
                stat.MpRecovChance = reader.ReadByte("Item" + i.ToString(), "MpRecovChance", 0);
                stat.MpRecovStatChance = reader.ReadByte("Item" + i.ToString(), "MpRecovStatChance", 1);
                stat.MpRecovMaxStat = reader.ReadByte("Item" + i.ToString(), "MpRecovMaxStat", 1);
                stat.PoisonRecovChance = reader.ReadByte("Item" + i.ToString(), "PoisonRecovChance", 0);
                stat.PoisonRecovStatChance = reader.ReadByte("Item" + i.ToString(), "PoisonRecovStatChance", 1);
                stat.PoisonRecovMaxStat = reader.ReadByte("Item" + i.ToString(), "PoisonRecovMaxStat", 1);
                stat.CriticalRateChance = reader.ReadByte("Item" + i.ToString(), "CriticalRateChance", 0);
                stat.CriticalRateStatChance = reader.ReadByte("Item" + i.ToString(), "CriticalRateStatChance", 1);
                stat.CriticalRateMaxStat = reader.ReadByte("Item" + i.ToString(), "CriticalRateMaxStat", 1);
                stat.CriticalDamageChance = reader.ReadByte("Item" + i.ToString(), "CriticalDamageChance", 0);
                stat.CriticalDamageStatChance = reader.ReadByte("Item" + i.ToString(), "CriticalDamageStatChance", 1);
                stat.CriticalDamageMaxStat = reader.ReadByte("Item" + i.ToString(), "CriticalDamageMaxStat", 1);
                stat.FreezeChance = reader.ReadByte("Item" + i.ToString(), "FreezeChance", 0);
                stat.FreezeStatChance = reader.ReadByte("Item" + i.ToString(), "FreezeStatChance", 1);
                stat.FreezeMaxStat = reader.ReadByte("Item" + i.ToString(), "FreezeMaxStat", 1);
                stat.PoisonAttackChance = reader.ReadByte("Item" + i.ToString(), "PoisonAttackChance", 0);
                stat.PoisonAttackStatChance = reader.ReadByte("Item" + i.ToString(), "PoisonAttackStatChance", 1);
                stat.PoisonAttackMaxStat = reader.ReadByte("Item" + i.ToString(), "PoisonAttackMaxStat", 1);
                stat.AttackSpeedChance = reader.ReadByte("Item" + i.ToString(), "AttackSpeedChance", 0);
                stat.AttackSpeedStatChance = reader.ReadByte("Item" + i.ToString(), "AttackSpeedStatChance", 1);
                stat.AttackSpeedMaxStat = reader.ReadByte("Item" + i.ToString(), "AttackSpeedMaxStat", 1);
                stat.LuckChance = reader.ReadByte("Item" + i.ToString(), "LuckChance", 0);
                stat.LuckStatChance = reader.ReadByte("Item" + i.ToString(), "LuckStatChance", 1);
                stat.LuckMaxStat = reader.ReadByte("Item" + i.ToString(), "LuckMaxStat", 1);
                stat.CurseChance = reader.ReadByte("Item" + i.ToString(), "CurseChance", 0);
                stat.SlotChance = reader.ReadByte("Item" + i.ToString(), "SlotChance", 0);
                stat.SlotStatChance = reader.ReadByte("Item" + i.ToString(), "SlotStatChance", 0);
                stat.SlotMaxStat = reader.ReadByte("Item" + i.ToString(), "SlotMaxStat", 0);
                RandomItemStatsList.Add(stat);
                i++;
            }
        }
        public static void SaveRandomItemStats()
        {
            File.Delete(Path.Combine(ConfigPath, "RandomItemStats.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "RandomItemStats.ini"));
            RandomItemStat stat;
            for (int i = 0; i < RandomItemStatsList.Count; i++)
            {
                stat = RandomItemStatsList[i];
                reader.Write("Item" + i.ToString(), "MaxDuraChance", stat.MaxDuraChance);
                reader.Write("Item" + i.ToString(), "MaxDuraStatChance", stat.MaxDuraStatChance);
                reader.Write("Item" + i.ToString(), "MaxDuraMaxStat", stat.MaxDuraMaxStat);
                reader.Write("Item" + i.ToString(), "MaxAcChance", stat.MaxAcChance);
                reader.Write("Item" + i.ToString(), "MaxAcStatChance", stat.MaxAcStatChance);
                reader.Write("Item" + i.ToString(), "MaxAcMaxStat", stat.MaxAcMaxStat);
                reader.Write("Item" + i.ToString(), "MaxMacChance", stat.MaxMacChance);
                reader.Write("Item" + i.ToString(), "MaxMacStatChance", stat.MaxMacStatChance);
                reader.Write("Item" + i.ToString(), "MaxMACMaxStat", stat.MaxMacMaxStat);
                reader.Write("Item" + i.ToString(), "MaxDcChance", stat.MaxDcChance);
                reader.Write("Item" + i.ToString(), "MaxDcStatChance", stat.MaxDcStatChance);
                reader.Write("Item" + i.ToString(), "MaxDcMaxStat", stat.MaxDcMaxStat);
                reader.Write("Item" + i.ToString(), "MaxMcChance", stat.MaxMcChance);
                reader.Write("Item" + i.ToString(), "MaxMcStatChance",  stat.MaxMcStatChance);
                reader.Write("Item" + i.ToString(), "MaxMcMaxStat", stat.MaxMcMaxStat);
                reader.Write("Item" + i.ToString(), "MaxScChance", stat.MaxScChance);
                reader.Write("Item" + i.ToString(), "MaxScStatChance", stat.MaxScStatChance);
                reader.Write("Item" + i.ToString(), "MaxScMaxStat", stat.MaxScMaxStat);
                reader.Write("Item" + i.ToString(), "AccuracyChance", stat.AccuracyChance);
                reader.Write("Item" + i.ToString(), "AccuracyStatChance", stat.AccuracyStatChance);
                reader.Write("Item" + i.ToString(), "AccuracyMaxStat", stat.AccuracyMaxStat);
                reader.Write("Item" + i.ToString(), "AgilityChance", stat.AgilityChance);
                reader.Write("Item" + i.ToString(), "AgilityStatChance", stat.AgilityStatChance);
                reader.Write("Item" + i.ToString(), "AgilityMaxStat", stat.AgilityMaxStat);
                reader.Write("Item" + i.ToString(), "HpChance", stat.HpChance);
                reader.Write("Item" + i.ToString(), "HpStatChance", stat.HpStatChance);
                reader.Write("Item" + i.ToString(), "HpMaxStat", stat.HpMaxStat);
                reader.Write("Item" + i.ToString(), "MpChance", stat.MpChance);
                reader.Write("Item" + i.ToString(), "MpStatChance", stat.MpStatChance);
                reader.Write("Item" + i.ToString(), "MpMaxStat", stat.MpMaxStat);
                reader.Write("Item" + i.ToString(), "StrongChance", stat.StrongChance);
                reader.Write("Item" + i.ToString(), "StrongStatChance", stat.StrongStatChance);
                reader.Write("Item" + i.ToString(), "StrongMaxStat", stat.StrongMaxStat);
                reader.Write("Item" + i.ToString(), "MagicResistChance", stat.MagicResistChance);
                reader.Write("Item" + i.ToString(), "MagicResistStatChance", stat.MagicResistStatChance);
                reader.Write("Item" + i.ToString(), "MagicResistMaxStat", stat.MagicResistMaxStat);
                reader.Write("Item" + i.ToString(), "PoisonResistChance", stat.PoisonResistChance);
                reader.Write("Item" + i.ToString(), "PoisonResistStatChance", stat.PoisonResistStatChance);
                reader.Write("Item" + i.ToString(), "PoisonResistMaxStat", stat.PoisonResistMaxStat);
                reader.Write("Item" + i.ToString(), "HpRecovChance", stat.HpRecovChance);
                reader.Write("Item" + i.ToString(), "HpRecovStatChance", stat.HpRecovStatChance);
                reader.Write("Item" + i.ToString(), "HpRecovMaxStat", stat.HpRecovMaxStat);
                reader.Write("Item" + i.ToString(), "MpRecovChance", stat.MpRecovChance);
                reader.Write("Item" + i.ToString(), "MpRecovStatChance", stat.MpRecovStatChance);
                reader.Write("Item" + i.ToString(), "MpRecovMaxStat", stat.MpRecovMaxStat);
                reader.Write("Item" + i.ToString(), "PoisonRecovChance", stat.PoisonRecovChance);
                reader.Write("Item" + i.ToString(), "PoisonRecovStatChance", stat.PoisonRecovStatChance);
                reader.Write("Item" + i.ToString(), "PoisonRecovMaxStat", stat.PoisonRecovMaxStat);
                reader.Write("Item" + i.ToString(), "CriticalRateChance", stat.CriticalRateChance);
                reader.Write("Item" + i.ToString(), "CriticalRateStatChance", stat.CriticalRateStatChance);
                reader.Write("Item" + i.ToString(), "CriticalRateMaxStat", stat.CriticalRateMaxStat);
                reader.Write("Item" + i.ToString(), "CriticalDamageChance", stat.CriticalDamageChance);
                reader.Write("Item" + i.ToString(), "CriticalDamageStatChance", stat.CriticalDamageStatChance);
                reader.Write("Item" + i.ToString(), "CriticalDamageMaxStat", stat.CriticalDamageMaxStat);
                reader.Write("Item" + i.ToString(), "FreezeChance", stat.FreezeChance);
                reader.Write("Item" + i.ToString(), "FreezeStatChance", stat.FreezeStatChance);
                reader.Write("Item" + i.ToString(), "FreezeMaxStat", stat.FreezeMaxStat);
                reader.Write("Item" + i.ToString(), "PoisonAttackChance", stat.PoisonAttackChance);
                reader.Write("Item" + i.ToString(), "PoisonAttackStatChance", stat.PoisonAttackStatChance);
                reader.Write("Item" + i.ToString(), "PoisonAttackMaxStat", stat.PoisonAttackMaxStat);
                reader.Write("Item" + i.ToString(), "AttackSpeedChance", stat.AttackSpeedChance);
                reader.Write("Item" + i.ToString(), "AttackSpeedStatChance", stat.AttackSpeedStatChance);
                reader.Write("Item" + i.ToString(), "AttackSpeedMaxStat", stat.AttackSpeedMaxStat);
                reader.Write("Item" + i.ToString(), "LuckChance", stat.LuckChance);
                reader.Write("Item" + i.ToString(), "LuckStatChance", stat.LuckStatChance);
                reader.Write("Item" + i.ToString(), "LuckMaxStat", stat.LuckMaxStat);
                reader.Write("Item" + i.ToString(), "CurseChance", stat.CurseChance);
                reader.Write("Item" + i.ToString(), "SlotChance", stat.SlotChance);
                reader.Write("Item" + i.ToString(), "SlotStatChance", stat.SlotStatChance);
                reader.Write("Item" + i.ToString(), "SlotMaxStat", stat.SlotMaxStat);
            }
        }

        public static void LoadMines()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "Mines.ini")))
            {
                MineSetList.Add(new MineSet(1));
                MineSetList.Add(new MineSet(2));
                SaveMines();
                return;
            }
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "Mines.ini"));
            int i = 0;
            MineSet mine;
            while (reader.ReadByte("Mine" + i.ToString(), "SpotRegenRate", 255) != 255)
            {
                mine = new MineSet();
                mine.Name = reader.ReadString("Mine" + i.ToString(), "Name", mine.Name);
                mine.SpotRegenRate = reader.ReadByte("Mine" + i.ToString(), "SpotRegenRate", mine.SpotRegenRate);
                mine.MaxStones = reader.ReadByte("Mine" + i.ToString(), "MaxStones", mine.MaxStones);
                mine.HitRate = reader.ReadByte("Mine" + i.ToString(), "HitRate", mine.HitRate);
                mine.DropRate = reader.ReadByte("Mine" + i.ToString(), "DropRate", mine.DropRate);
                mine.TotalSlots = reader.ReadByte("Mine" + i.ToString(), "TotalSlots", mine.TotalSlots);
                int j = 0;
                while (reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MinSlot", 255) != 255)
                {
                    mine.Drops.Add(new MineDrop()
                        {
                            ItemName = reader.ReadString("Mine" + i.ToString(), "D" + j.ToString() + "-ItemName", ""),
                            MinSlot = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MinSlot", 255),
                            MaxSlot = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MaxSlot", 255),
                            MinDura = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MinDura", 255),
                            MaxDura = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MaxDura", 255),
                            BonusChance = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-BonusChance", 255),
                            MaxBonusDura = reader.ReadByte("Mine" + i.ToString(), "D" + j.ToString() + "-MaxBonusDura", 255)
                        });
                    j++;
                }
                MineSetList.Add(mine);
                i++;
            }

        }
        public static void SaveMines()
        {
            File.Delete(Path.Combine(ConfigPath, "Mines.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "Mines.ini"));
            MineSet mine;
            for (int i = 0; i < MineSetList.Count; i++)
            {
                mine = MineSetList[i];
                reader.Write("Mine" + i.ToString(), "Name", mine.Name);
                reader.Write("Mine" + i.ToString(), "SpotRegenRate", mine.SpotRegenRate);
                reader.Write("Mine" + i.ToString(), "MaxStones", mine.MaxStones);
                reader.Write("Mine" + i.ToString(), "HitRate", mine.HitRate);
                reader.Write("Mine" + i.ToString(), "DropRate", mine.DropRate);
                reader.Write("Mine" + i.ToString(), "TotalSlots", mine.TotalSlots);
                
                for (int j = 0; j < mine.Drops.Count; j++)
                {
                    MineDrop Drop = mine.Drops[j];
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-ItemName", Drop.ItemName);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MinSlot", Drop.MinSlot);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MaxSlot", Drop.MaxSlot);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MinDura", Drop.MinDura);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MaxDura", Drop.MaxDura);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-BonusChance", Drop.BonusChance);
                    reader.Write("Mine" + i.ToString(), "D" + j.ToString() + "-MaxBonusDura", Drop.MaxBonusDura);
                }
            }
        }

        public static void LoadHeroSettings()
        {
            Array.Resize(ref Hero_CanCreateClass, Enum.GetNames(typeof(MirClass)).Length);
            if (!File.Exists(Path.Combine(ConfigPath, "HeroSettings.ini")))
            {                
                for (int i = 0; i < Hero_CanCreateClass.Length; i++)
                    Hero_CanCreateClass[i] = true;
                SaveHeroSettings();
                return;
            }

            InIReader reader = new InIReader(Path.Combine(ConfigPath, "HeroSettings.ini"));
            AllowNewHero = reader.ReadBoolean("Hero", "AllowNewHero", AllowNewHero);
            Hero_RequiredLevel = reader.ReadByte("Hero", "MinimumLevel", Hero_RequiredLevel);
            MaximumHeroCount = reader.ReadByte("Hero", "MaximumCount", MaximumHeroCount);
            for (int i = 0; i < Hero_CanCreateClass.Length; i++)
                Hero_CanCreateClass[i] = reader.ReadBoolean("Hero", $"CanCreate{Enum.GetName(typeof(MirClass), i)}", true);
            HeroSealItemName = reader.ReadString("Hero", "SealItemName", HeroSealItemName);
            HeroMaximumSealCount = reader.ReadUInt16("Hero", "MaximumSealCount", HeroMaximumSealCount);
        }
        public static void SaveHeroSettings()
        {
            File.Delete(Path.Combine(ConfigPath, "HeroSettings.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "HeroSettings.ini"));

            reader.Write("Hero", "AllowNewHero", AllowNewHero);
            reader.Write("Hero", "MinimumLevel", Hero_RequiredLevel);
            reader.Write("Hero", "MaximumCount", MaximumHeroCount);
            for (int i = 0; i < Hero_CanCreateClass.Length; i++)
                reader.Write("Hero", $"CanCreate{Enum.GetName(typeof(MirClass), i)}", Hero_CanCreateClass[i]);
            reader.Write("Hero", "SealItemName", HeroSealItemName);
            reader.Write("Hero", "MaximumSealCount", HeroMaximumSealCount);
        }

        public static void LoadGuildSettings()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "GuildSettings.ini")))
            {
                Guild_CreationCostList.Add(new GuildItemVolume(){Amount = 1000000});
                Guild_CreationCostList.Add(new GuildItemVolume(){ItemName = "WoomaHorn",Amount = 1});
                return;
            }
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "GuildSettings.ini"));
            Guild_RequiredLevel = reader.ReadByte("Guilds", "MinimumLevel", Guild_RequiredLevel);
            Guild_ExpRate = reader.ReadFloat("Guilds", "ExpRate", Guild_ExpRate);
            Guild_PointPerLevel = reader.ReadByte("Guilds", "PointPerLevel", Guild_PointPerLevel);
            Guild_WarTime = reader.ReadInt64("Guilds", "WarTime", Guild_WarTime);
            Guild_WarCost = reader.ReadUInt32("Guilds", "WarCost", Guild_WarCost);

            int i = 0;
            while (reader.ReadUInt32("Required-" + i.ToString(),"Amount",0) != 0)
            {
                Guild_CreationCostList.Add(new GuildItemVolume()
                {
                    ItemName = reader.ReadString("Required-" + i.ToString(), "ItemName", ""),
                    Amount = reader.ReadUInt32("Required-" + i.ToString(), "Amount", 0)
                }
                );
                i++;
            }
            i = 0;
            while (reader.ReadInt64("Exp", "Level-" + i.ToString(), -1) != -1)
            {
                Guild_ExperienceList.Add(reader.ReadInt64("Exp", "Level-" + i.ToString(), 0));
                i++;
            }
            i = 0;
            while (reader.ReadInt32("Cap", "Level-" + i.ToString(), -1) != -1)
            {
                Guild_MembercapList.Add(reader.ReadInt32("Cap", "Level-" + i.ToString(), 0));
                i++;
            }
            byte TotalBuffs = reader.ReadByte("Guilds", "TotalBuffs", 0);
            for (i = 0; i < TotalBuffs; i++)
            {
                Guild_BuffList.Add(new GuildBuffInfo(reader, i));
            }
        }
        public static void SaveGuildSettings()
        {
            File.Delete(Path.Combine(ConfigPath, "GuildSettings.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "GuildSettings.ini"));
            reader.Write("Guilds", "MinimumLevel", Guild_RequiredLevel);
            reader.Write("Guilds", "ExpRate", Guild_ExpRate);
            reader.Write("Guilds", "PointPerLevel", Guild_PointPerLevel);
            reader.Write("Guilds", "TotalBuffs", Guild_BuffList.Count);
            reader.Write("Guilds", "WarTime", Guild_WarTime);
            reader.Write("Guilds", "WarCost", Guild_WarCost);

            int i;
            for (i = 0; i < Guild_ExperienceList.Count; i++)
            {
                reader.Write("Exp", "Level-" + i.ToString(), Guild_ExperienceList[i]);
            }
            for (i = 0; i < Guild_MembercapList.Count; i++)
            {
                reader.Write("Cap", "Level-" + i.ToString(), Guild_MembercapList[i]);
            }
            for (i = 0; i < Guild_CreationCostList.Count; i++)
            {
                reader.Write("Required-" + i.ToString(), "ItemName", Guild_CreationCostList[i].ItemName);
                reader.Write("Required-" + i.ToString(), "Amount", Guild_CreationCostList[i].Amount);
            }
            for (i = 0; i < Guild_BuffList.Count; i++)
            {
                Guild_BuffList[i].Save(reader, i);
            }
        }
        public static void LinkGuildCreationItems(List<ItemInfo> ItemList)
        {
            for (int i = 0; i < Guild_CreationCostList.Count; i++)
            {
                if (Guild_CreationCostList[i].ItemName != "")
                    for (int j = 0; j < ItemList.Count; j++)
                    {
                        if (String.Compare(ItemList[j].Name.Replace(" ", ""), Guild_CreationCostList[i].ItemName, StringComparison.OrdinalIgnoreCase) != 0) continue;
                        Guild_CreationCostList[i].Item = ItemList[j];
                        break;
                    }
                  
            }
        }

		public static void LoadAwakeAttribute()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "AwakeningSystem.ini")))
            {
                return;
            }

            InIReader reader = new InIReader(Path.Combine(ConfigPath, "AwakeningSystem.ini"));
            Awake.AwakeSuccessRate = reader.ReadByte("Attribute", "SuccessRate", Awake.AwakeSuccessRate);
            Awake.AwakeHitRate = reader.ReadByte("Attribute", "HitRate", Awake.AwakeHitRate);
            Awake.MaxAwakeLevel = reader.ReadInt32("Attribute", "MaxUpgradeLevel", Awake.MaxAwakeLevel);
            Awake.Awake_WeaponRate = reader.ReadByte("IncreaseValue", "WeaponValue", Awake.Awake_WeaponRate);
            Awake.Awake_HelmetRate = reader.ReadByte("IncreaseValue", "HelmetValue", Awake.Awake_HelmetRate);
            Awake.Awake_ArmorRate = reader.ReadByte("IncreaseValue", "ArmorValue", Awake.Awake_ArmorRate);

            for (int i = 0; i < 4; i++)
            {
                Awake.AwakeChanceMax[i] = reader.ReadByte("Value", "ChanceMax_" + ((ItemGrade)(i + 1)).ToString(), Awake.AwakeChanceMax[i]);
            }

            for (int i = 0; i < (int)AwakeType.HPMP; i++)
            {
                List<byte>[] value = new List<byte>[2];

                for (int k = 0; k < 2; k++)
                {
                    value[k] = new List<byte>();
                }

                for (int j = 0; j < 4; j++)
                {
                    byte material1 = 1;
                    material1 = reader.ReadByte("Materials_BaseValue", ((AwakeType)(i + 1)).ToString() + "_" + ((ItemGrade)(j + 1)).ToString() + "_Material1", material1);
                    byte material2 = 1;
                    material2 = reader.ReadByte("Materials_BaseValue", ((AwakeType)(i + 1)).ToString() + "_" + ((ItemGrade)(j + 1)).ToString() + "_Material2", material2);
                    value[0].Add(material1);
                    value[1].Add(material2);
                }

                Awake.AwakeMaterials.Add(value);
            }

            for (int c = 0; c < 4; c++)
            {
                Awake.AwakeMaterialRate[c] = reader.ReadFloat("Materials_IncreaseValue", "Materials_" + ((ItemGrade)(c + 1)).ToString(), Awake.AwakeMaterialRate[c]);
            }

        }
        public static void SaveAwakeAttribute()
        {
            File.Delete(Path.Combine(ConfigPath, "AwakeningSystem.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "AwakeningSystem.ini"));
            reader.Write("Attribute", "SuccessRate", Awake.AwakeSuccessRate);
            reader.Write("Attribute", "HitRate", Awake.AwakeHitRate);
            reader.Write("Attribute", "MaxUpgradeLevel", Awake.MaxAwakeLevel);

            reader.Write("IncreaseValue", "WeaponValue", Awake.Awake_WeaponRate);
            reader.Write("IncreaseValue", "HelmetValue", Awake.Awake_HelmetRate);
            reader.Write("IncreaseValue", "ArmorValue", Awake.Awake_ArmorRate);

            for (int i = 0; i < 4; i++)
            {
                reader.Write("Value", "ChanceMax_" + ((ItemGrade)(i + 1)).ToString(), Awake.AwakeChanceMax[i]);
            }

            if (Awake.AwakeMaterials.Count == 0)
            {
                for (int i = 0; i < (int)AwakeType.HPMP; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        reader.Write("Materials_BaseValue", ((AwakeType)(i + 1)).ToString() + "_" + ((ItemGrade)(j + 1)).ToString() + "_Material1", 1);
                        reader.Write("Materials_BaseValue", ((AwakeType)(i + 1)).ToString() + "_" + ((ItemGrade)(j + 1)).ToString() + "_Material2", 1);
                    }
                }
            }
            else
            {
                for (int i = 0; i < (int)AwakeType.HPMP; i++)
                {
                    List<byte>[] value = Awake.AwakeMaterials[i];

                    for (int j = 0; j < value[0].Count; j++)
                    {
                        reader.Write("Materials_BaseValue", ((AwakeType)(i + 1)).ToString() + "_" + ((ItemGrade)(j + 1)).ToString() + "_Material1", value[0][j]);
                        reader.Write("Materials_BaseValue", ((AwakeType)(i + 1)).ToString() + "_" + ((ItemGrade)(j + 1)).ToString() + "_Material2", value[1][j]);
                    }

                    Awake.AwakeMaterials.Add(value);
                }
            }

            for (int c = 0; c < 4; c++)
            {
                reader.Write("Materials_IncreaseValue", "Materials_" + ((ItemGrade)(c + 1)).ToString(), Awake.AwakeMaterialRate[c]);
            }
        }

        public static void LoadFishing()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "FishingSystem.ini")))
            {
                SaveFishing();
                return;
            }

            InIReader reader = new InIReader(Path.Combine(ConfigPath, "FishingSystem.ini"));
            FishingAttempts = reader.ReadInt32("Rates", "Attempts", FishingAttempts);
            FishingSuccessStart = reader.ReadInt32("Rates", "SuccessStart", FishingSuccessStart);
            FishingSuccessMultiplier = reader.ReadInt32("Rates", "SuccessMultiplier", FishingSuccessMultiplier);
            FishingDelay = reader.ReadInt64("Rates", "Delay", FishingDelay);
            FishingMobSpawnChance = reader.ReadInt32("Rates", "MonsterSpawnChance", FishingMobSpawnChance);
            FishingMonster = reader.ReadString("Game", "Monster", FishingMonster);
        }
        public static void SaveFishing()
        {
            File.Delete(Path.Combine(ConfigPath, "FishingSystem.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "FishingSystem.ini"));
            reader.Write("Rates", "Attempts", FishingAttempts);
            reader.Write("Rates", "SuccessStart", FishingSuccessStart);
            reader.Write("Rates", "SuccessMultiplier", FishingSuccessMultiplier);
            reader.Write("Rates", "Delay", FishingDelay);
            reader.Write("Rates", "MonsterSpawnChance", FishingMobSpawnChance);
            reader.Write("Game", "Monster", FishingMonster);
        }

        public static void LoadMail()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "MailSystem.ini")))
            {
                SaveMail();
                return;
            }

            InIReader reader = new InIReader(Path.Combine(ConfigPath, "MailSystem.ini"));
            MailAutoSendGold = reader.ReadBoolean("AutoSend", "Gold", MailAutoSendGold);
            MailAutoSendItems = reader.ReadBoolean("AutoSend", "Items", MailAutoSendItems);
            MailFreeWithStamp = reader.ReadBoolean("Rates", "FreeWithStamp", MailFreeWithStamp);
            MailCostPer1KGold = reader.ReadUInt32("Rates", "CostPer1k", MailCostPer1KGold);
            MailItemInsurancePercentage = reader.ReadUInt32("Rates", "InsurancePerItem", MailItemInsurancePercentage);
            MailCapacity = reader.ReadUInt32("General", "MailCapacity", MailCapacity);
        }
        public static void SaveMail()
        {
            File.Delete(Path.Combine(ConfigPath, "MailSystem.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "MailSystem.ini"));
            reader.Write("AutoSend", "Gold", MailAutoSendGold);
            reader.Write("AutoSend", "Items", MailAutoSendItems);
            reader.Write("Rates", "FreeWithStamp", MailFreeWithStamp);
            reader.Write("Rates", "CostPer1k", MailCostPer1KGold);
            reader.Write("Rates", "InsurancePerItem", MailItemInsurancePercentage);
            reader.Write("General", "MailCapacity", MailCapacity);
        }

        public static void LoadRefine()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "RefineSystem.ini")))
            {
                SaveRefine();
                return;
            }

            InIReader reader = new InIReader(Path.Combine(ConfigPath, "RefineSystem.ini"));
            OnlyRefineWeapon = reader.ReadBoolean("Config", "OnlyRefineWeapon", OnlyRefineWeapon);
            RefineBaseChance = reader.ReadByte("Config", "BaseChance", RefineBaseChance);
            RefineTime = reader.ReadInt32("Config", "Time", RefineTime);
            RefineIncrease = reader.ReadByte("Config", "StatIncrease", RefineIncrease);
            RefineCritChance = reader.ReadByte("Config", "CritChance", RefineCritChance);
            RefineCritIncrease = reader.ReadByte("Config", "CritIncrease", RefineCritIncrease);
            RefineWepStatReduce = reader.ReadByte("Config", "WepStatReducedChance", RefineWepStatReduce);
            RefineItemStatReduce = reader.ReadByte("Config", "ItemStatReducedChance", RefineItemStatReduce);
            RefineCost = reader.ReadInt32("Config", "RefineCost", RefineCost);

            RefineOreName = reader.ReadString("Ore", "OreName", RefineOreName);
        }
        public static void SaveRefine()
        {
            File.Delete(Path.Combine(ConfigPath, "RefineSystem.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "RefineSystem.ini"));
            reader.Write("Config", "OnlyRefineWeapon", OnlyRefineWeapon);
            reader.Write("Config", "BaseChance", RefineBaseChance);
            reader.Write("Config", "Time", RefineTime);
            reader.Write("Config", "StatIncrease", RefineIncrease);
            reader.Write("Config", "CritChance", RefineCritChance);
            reader.Write("Config", "CritIncrease", RefineCritIncrease);
            reader.Write("Config", "WepStatReducedChance", RefineWepStatReduce);
            reader.Write("Config", "ItemStatReducedChance", RefineItemStatReduce);
            reader.Write("Config", "RefineCost", RefineCost);

            reader.Write("Ore", "OreName", RefineOreName);

        }

        public static void LoadMarriage()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "MarriageSystem.ini")))
            {
                SaveMarriage();
                return;
            }
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "MarriageSystem.ini"));
            LoverEXPBonus = reader.ReadInt32("Config", "EXPBonus", LoverEXPBonus);
            MarriageCooldown = reader.ReadInt32("Config", "MarriageCooldown", MarriageCooldown);
            WeddingRingRecall = reader.ReadBoolean("Config", "AllowLoverRecall", WeddingRingRecall);
            MarriageLevelRequired = reader.ReadInt32("Config", "MinimumLevel", MarriageLevelRequired);
            ReplaceWedRingCost = reader.ReadInt32("Config", "ReplaceRingCost", ReplaceWedRingCost);
        }
        public static void SaveMarriage()
        {
            File.Delete(Path.Combine(ConfigPath, "MarriageSystem.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "MarriageSystem.ini"));
            reader.Write("Config", "EXPBonus", LoverEXPBonus);
            reader.Write("Config", "MarriageCooldown", MarriageCooldown);
            reader.Write("Config", "AllowLoverRecall", WeddingRingRecall);
            reader.Write("Config", "MinimumLevel", MarriageLevelRequired);
            reader.Write("Config", "ReplaceRingCost", ReplaceWedRingCost); 
        }

        public static void LoadMentor()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "MentorSystem.ini")))
            {
                SaveMentor();
                return;
            }
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "MentorSystem.ini"));
            MentorLevelGap = reader.ReadByte("Config", "LevelGap", MentorLevelGap);
            MentorSkillBoost = reader.ReadBoolean("Config", "MenteeSkillBoost", MentorSkillBoost);
            MentorLength = reader.ReadByte("Config", "MentorshipLength", MentorLength);
            MentorDamageBoost = reader.ReadByte("Config", "MentorDamageBoost", MentorDamageBoost);
            MentorExpBoost = reader.ReadByte("Config", "MenteeExpBoost", MentorExpBoost);
            MenteeExpBank = reader.ReadByte("Config", "PercentXPtoMentor", MenteeExpBank);
        }
        public static void SaveMentor()
        {
            File.Delete(Path.Combine(ConfigPath, "MentorSystem.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "MentorSystem.ini"));
            reader.Write("Config", "LevelGap", MentorLevelGap);
            reader.Write("Config", "MenteeSkillBoost", MentorSkillBoost);
            reader.Write("Config", "MentorshipLength", MentorLength);
            reader.Write("Config", "MentorDamageBoost", MentorDamageBoost);
            reader.Write("Config", "MenteeExpBoost", MentorExpBoost);
            reader.Write("Config", "PercentXPtoMentor", MenteeExpBank);
        }
        public static void LoadGem()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "GemSystem.ini")))
            {
                SaveGem();
                return;
            }
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "GemSystem.ini"));
            GemStatIndependent = reader.ReadBoolean("Config", "GemStatIndependent", GemStatIndependent);


        }
        public static void SaveGem()
        {
            File.Delete(Path.Combine(ConfigPath, "GemSystem.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "GemSystem.ini"));
            reader.Write("Config", "GemStatIndependent", GemStatIndependent);
        }

        public static void LoadGoods()
        {
            if (!File.Exists(Path.Combine(ConfigPath, "GoodsSystem.ini")))
            {
                SaveGoods();
                return;
            }

            InIReader reader = new InIReader(Path.Combine(ConfigPath, "GoodsSystem.ini"));
            GoodsOn = reader.ReadBoolean("Goods", "On", GoodsOn);
            GoodsMaxStored = reader.ReadUInt32("Goods", "MaxStored", GoodsMaxStored);
            GoodsBuyBackTime = reader.ReadUInt32("Goods", "BuyBackTime", GoodsBuyBackTime);
            GoodsBuyBackMaxStored = reader.ReadUInt32("Goods", "BuyBackMaxStored", GoodsBuyBackMaxStored);
            GoodsHideAddedStats = reader.ReadBoolean("Goods", "HideAddedStats", GoodsHideAddedStats);
        }
        public static void SaveGoods()
        {
            File.Delete(Path.Combine(ConfigPath, "GoodsSystem.ini"));
            InIReader reader = new InIReader(Path.Combine(ConfigPath, "GoodsSystem.ini"));
            reader.Write("Goods", "On", GoodsOn);
            reader.Write("Goods", "MaxStored", GoodsMaxStored);
            reader.Write("Goods", "BuyBackTime", GoodsBuyBackTime);
            reader.Write("Goods", "BuyBackMaxStored", GoodsBuyBackMaxStored);
            reader.Write("Goods", "HideAddedStats", GoodsHideAddedStats);
        }

    }
}
