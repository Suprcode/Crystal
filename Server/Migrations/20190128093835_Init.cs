using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConquestInfos",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullMap = table.Column<bool>(nullable: false),
                    LocationString = table.Column<string>(nullable: true),
                    Size = table.Column<ushort>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MapIndex = table.Column<int>(nullable: false),
                    PalaceIndex = table.Column<int>(nullable: false),
                    GuardIndex = table.Column<int>(nullable: false),
                    GateIndex = table.Column<int>(nullable: false),
                    WallIndex = table.Column<int>(nullable: false),
                    SiegeIndex = table.Column<int>(nullable: false),
                    FlagIndex = table.Column<int>(nullable: false),
                    StartHour = table.Column<byte>(nullable: false),
                    WarLength = table.Column<int>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    Game = table.Column<byte>(nullable: false),
                    Monday = table.Column<bool>(nullable: false),
                    Tuesday = table.Column<bool>(nullable: false),
                    Wednesday = table.Column<bool>(nullable: false),
                    Thursday = table.Column<bool>(nullable: false),
                    Friday = table.Column<bool>(nullable: false),
                    Saturday = table.Column<bool>(nullable: false),
                    Sunday = table.Column<bool>(nullable: false),
                    KingLocationString = table.Column<string>(nullable: true),
                    KingSize = table.Column<ushort>(nullable: false),
                    ControlPointIndex = table.Column<int>(nullable: false),
                    BinData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConquestInfos", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "GameShopItems",
                columns: table => new
                {
                    GIndex = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemIndex = table.Column<int>(nullable: false),
                    GoldPrice = table.Column<uint>(nullable: false),
                    CreditPrice = table.Column<uint>(nullable: false),
                    Count = table.Column<uint>(nullable: false),
                    Class = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Stock = table.Column<int>(nullable: false),
                    iStock = table.Column<bool>(nullable: false),
                    Deal = table.Column<bool>(nullable: false),
                    TopItem = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameShopItems", x => x.GIndex);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<byte>(nullable: false),
                    Grade = table.Column<byte>(nullable: false),
                    RequiredType = table.Column<byte>(nullable: false),
                    RequiredClass = table.Column<byte>(nullable: false),
                    RequiredGender = table.Column<byte>(nullable: false),
                    Set = table.Column<byte>(nullable: false),
                    Shape = table.Column<short>(nullable: false),
                    Weight = table.Column<byte>(nullable: false),
                    Light = table.Column<byte>(nullable: false),
                    RequiredAmount = table.Column<byte>(nullable: false),
                    Image = table.Column<ushort>(nullable: false),
                    Durability = table.Column<ushort>(nullable: false),
                    Price = table.Column<uint>(nullable: false),
                    StackSize = table.Column<uint>(nullable: false),
                    MinAC = table.Column<byte>(nullable: false),
                    MaxAC = table.Column<byte>(nullable: false),
                    MinMAC = table.Column<byte>(nullable: false),
                    MaxMAC = table.Column<byte>(nullable: false),
                    MinDC = table.Column<byte>(nullable: false),
                    MaxDC = table.Column<byte>(nullable: false),
                    MinMC = table.Column<byte>(nullable: false),
                    MaxMC = table.Column<byte>(nullable: false),
                    MinSC = table.Column<byte>(nullable: false),
                    MaxSC = table.Column<byte>(nullable: false),
                    Accuracy = table.Column<byte>(nullable: false),
                    Agility = table.Column<byte>(nullable: false),
                    HP = table.Column<ushort>(nullable: false),
                    MP = table.Column<ushort>(nullable: false),
                    AttackSpeed = table.Column<byte>(nullable: false),
                    Luck = table.Column<byte>(nullable: false),
                    BagWeight = table.Column<byte>(nullable: false),
                    HandWeight = table.Column<byte>(nullable: false),
                    WearWeight = table.Column<byte>(nullable: false),
                    StartItem = table.Column<bool>(nullable: false),
                    Effect = table.Column<byte>(nullable: false),
                    Strong = table.Column<byte>(nullable: false),
                    MagicResist = table.Column<byte>(nullable: false),
                    PoisonResist = table.Column<byte>(nullable: false),
                    HealthRecovery = table.Column<byte>(nullable: false),
                    SpellRecovery = table.Column<byte>(nullable: false),
                    PoisonRecovery = table.Column<byte>(nullable: false),
                    HPrate = table.Column<byte>(nullable: false),
                    MPrate = table.Column<byte>(nullable: false),
                    CriticalRate = table.Column<byte>(nullable: false),
                    CriticalDamage = table.Column<byte>(nullable: false),
                    NeedIdentify = table.Column<bool>(nullable: false),
                    ShowGroupPickup = table.Column<bool>(nullable: false),
                    GlobalDropNotify = table.Column<bool>(nullable: false),
                    ClassBased = table.Column<bool>(nullable: false),
                    LevelBased = table.Column<bool>(nullable: false),
                    CanMine = table.Column<bool>(nullable: false),
                    CanFastRun = table.Column<bool>(nullable: false),
                    CanAwakening = table.Column<bool>(nullable: false),
                    MaxAcRate = table.Column<byte>(nullable: false),
                    MaxMacRate = table.Column<byte>(nullable: false),
                    Holy = table.Column<byte>(nullable: false),
                    Freezing = table.Column<byte>(nullable: false),
                    PoisonAttack = table.Column<byte>(nullable: false),
                    HpDrainRate = table.Column<byte>(nullable: false),
                    Bind = table.Column<short>(nullable: false),
                    Reflect = table.Column<byte>(nullable: false),
                    Unique = table.Column<short>(nullable: false),
                    RandomStatsId = table.Column<byte>(nullable: false),
                    ToolTip = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Magics",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Spell = table.Column<byte>(nullable: false),
                    BaseCost = table.Column<byte>(nullable: false),
                    LevelCost = table.Column<byte>(nullable: false),
                    Icon = table.Column<byte>(nullable: false),
                    Level1 = table.Column<byte>(nullable: false),
                    Level2 = table.Column<byte>(nullable: false),
                    Level3 = table.Column<byte>(nullable: false),
                    Need1 = table.Column<ushort>(nullable: false),
                    Need2 = table.Column<ushort>(nullable: false),
                    Need3 = table.Column<ushort>(nullable: false),
                    DelayBase = table.Column<uint>(nullable: false),
                    DelayReduction = table.Column<uint>(nullable: false),
                    PowerBase = table.Column<ushort>(nullable: false),
                    PowerBonus = table.Column<ushort>(nullable: false),
                    MPowerBase = table.Column<ushort>(nullable: false),
                    MPowerBonus = table.Column<ushort>(nullable: false),
                    MultiplierBase = table.Column<float>(nullable: false),
                    MultiplierBonus = table.Column<float>(nullable: false),
                    Range = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magics", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    MiniMap = table.Column<ushort>(nullable: false),
                    BigMap = table.Column<ushort>(nullable: false),
                    Music = table.Column<ushort>(nullable: false),
                    Light = table.Column<byte>(nullable: false),
                    MapDarkLight = table.Column<byte>(nullable: false),
                    MineIndex = table.Column<byte>(nullable: false),
                    NoTeleport = table.Column<bool>(nullable: false),
                    NoReconnect = table.Column<bool>(nullable: false),
                    NoRandom = table.Column<bool>(nullable: false),
                    NoEscape = table.Column<bool>(nullable: false),
                    NoRecall = table.Column<bool>(nullable: false),
                    NoDrug = table.Column<bool>(nullable: false),
                    NoPosition = table.Column<bool>(nullable: false),
                    NoFight = table.Column<bool>(nullable: false),
                    NoThrowItem = table.Column<bool>(nullable: false),
                    NoDropPlayer = table.Column<bool>(nullable: false),
                    NoDropMonster = table.Column<bool>(nullable: false),
                    NoNames = table.Column<bool>(nullable: false),
                    NoMount = table.Column<bool>(nullable: false),
                    NeedBridle = table.Column<bool>(nullable: false),
                    Fight = table.Column<bool>(nullable: false),
                    NeedHole = table.Column<bool>(nullable: false),
                    Fire = table.Column<bool>(nullable: false),
                    Lightning = table.Column<bool>(nullable: false),
                    NoReconnectMap = table.Column<string>(nullable: true),
                    FireDamage = table.Column<int>(nullable: false),
                    LightningDamage = table.Column<int>(nullable: false),
                    SafeZoneBytes = table.Column<byte[]>(nullable: true),
                    MineZoneBytes = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Monsters",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<ushort>(nullable: false),
                    AI = table.Column<byte>(nullable: false),
                    Effect = table.Column<byte>(nullable: false),
                    ViewRange = table.Column<byte>(nullable: false),
                    CoolEye = table.Column<byte>(nullable: false),
                    Level = table.Column<ushort>(nullable: false),
                    HP = table.Column<uint>(nullable: false),
                    Accuracy = table.Column<byte>(nullable: false),
                    Agility = table.Column<byte>(nullable: false),
                    Light = table.Column<byte>(nullable: false),
                    MinAC = table.Column<ushort>(nullable: false),
                    MaxAC = table.Column<ushort>(nullable: false),
                    MinMAC = table.Column<ushort>(nullable: false),
                    MaxMAC = table.Column<ushort>(nullable: false),
                    MinDC = table.Column<ushort>(nullable: false),
                    MaxDC = table.Column<ushort>(nullable: false),
                    MinMC = table.Column<ushort>(nullable: false),
                    MaxMC = table.Column<ushort>(nullable: false),
                    MinSC = table.Column<ushort>(nullable: false),
                    MaxSC = table.Column<ushort>(nullable: false),
                    AttackSpeed = table.Column<ushort>(nullable: false),
                    MoveSpeed = table.Column<ushort>(nullable: false),
                    Experience = table.Column<uint>(nullable: false),
                    CanTame = table.Column<bool>(nullable: false),
                    CanPush = table.Column<bool>(nullable: false),
                    AutoRev = table.Column<bool>(nullable: false),
                    Undead = table.Column<bool>(nullable: false),
                    HasSpawnScript = table.Column<bool>(nullable: false),
                    HasDieScript = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monsters", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MapIndex = table.Column<int>(nullable: false),
                    SourceString = table.Column<string>(nullable: true),
                    DestinationString = table.Column<string>(nullable: true),
                    NeedHole = table.Column<bool>(nullable: false),
                    NeedMove = table.Column<bool>(nullable: false),
                    ConquestIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "NpcInfos",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    MapIndex = table.Column<int>(nullable: false),
                    LocationString = table.Column<string>(nullable: true),
                    Rate = table.Column<ushort>(nullable: false),
                    Image = table.Column<ushort>(nullable: false),
                    ColorData = table.Column<int>(nullable: false),
                    TimeVisible = table.Column<bool>(nullable: false),
                    HourStart = table.Column<byte>(nullable: false),
                    MinuteStart = table.Column<byte>(nullable: false),
                    HourEnd = table.Column<byte>(nullable: false),
                    MinuteEnd = table.Column<byte>(nullable: false),
                    MinLev = table.Column<short>(nullable: false),
                    MaxLev = table.Column<short>(nullable: false),
                    DayofWeek = table.Column<string>(nullable: true),
                    ClassRequired = table.Column<string>(nullable: true),
                    Sabuk = table.Column<bool>(nullable: false),
                    FlagNeeded = table.Column<int>(nullable: false),
                    Conquest = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    IsRobot = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NpcInfos", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Respawns",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonsterIndex = table.Column<int>(nullable: false),
                    LocationString = table.Column<string>(nullable: true),
                    Count = table.Column<ushort>(nullable: false),
                    Spread = table.Column<ushort>(nullable: false),
                    Delay = table.Column<ushort>(nullable: false),
                    RandomDelay = table.Column<ushort>(nullable: false),
                    Direction = table.Column<byte>(nullable: false),
                    RoutePath = table.Column<string>(nullable: true),
                    RespawnIndex = table.Column<int>(nullable: false),
                    SaveRespawnTime = table.Column<bool>(nullable: false),
                    RespawnTicks = table.Column<ushort>(nullable: false),
                    MapIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respawns", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NpcIndex = table.Column<uint>(nullable: false),
                    NpcInfoIndex = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    GotoMessage = table.Column<string>(nullable: true),
                    KillMessage = table.Column<string>(nullable: true),
                    ItemMessage = table.Column<string>(nullable: true),
                    FlagMessage = table.Column<string>(nullable: true),
                    RequiredMinLevel = table.Column<int>(nullable: false),
                    RequiredMaxLevel = table.Column<int>(nullable: false),
                    RequiredQuest = table.Column<int>(nullable: false),
                    RequiredClass = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    GoldReward = table.Column<uint>(nullable: false),
                    ExpReward = table.Column<uint>(nullable: false),
                    CreditReward = table.Column<uint>(nullable: false),
                    FinishNpcIndex = table.Column<uint>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Index);
                    table.ForeignKey(
                        name: "FK_Quests_NpcInfos_NpcInfoIndex",
                        column: x => x.NpcInfoIndex,
                        principalTable: "NpcInfos",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quests_NpcInfoIndex",
                table: "Quests",
                column: "NpcInfoIndex");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConquestInfos");

            migrationBuilder.DropTable(
                name: "GameShopItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Magics");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Monsters");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "Respawns");

            migrationBuilder.DropTable(
                name: "NpcInfos");
        }
    }
}
