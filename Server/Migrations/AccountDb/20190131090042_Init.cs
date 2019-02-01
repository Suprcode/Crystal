using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations.AccountDb
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountID = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    SecretQuestion = table.Column<string>(nullable: true),
                    SecretAnswer = table.Column<string>(nullable: true),
                    EMailAddress = table.Column<string>(nullable: true),
                    CreationIP = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Banned = table.Column<bool>(nullable: false),
                    BanReason = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    WrongPasswordCount = table.Column<int>(nullable: false),
                    LastIP = table.Column<string>(nullable: true),
                    LastDate = table.Column<DateTime>(nullable: false),
                    HasExpandedStorage = table.Column<bool>(nullable: false),
                    ExpandedStorageExpiryDate = table.Column<DateTime>(nullable: false),
                    Gold = table.Column<uint>(nullable: false),
                    Credit = table.Column<uint>(nullable: false),
                    AdminAccount = table.Column<bool>(nullable: false),
                    StorageString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    AuctionID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemUniqueID = table.Column<ulong>(nullable: false),
                    ConsignmentDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<uint>(nullable: false),
                    CharacterIndex = table.Column<int>(nullable: false),
                    Expired = table.Column<bool>(nullable: false),
                    Sold = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.AuctionID);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Index = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Level = table.Column<ushort>(nullable: false),
                    Class = table.Column<byte>(nullable: false),
                    Gender = table.Column<byte>(nullable: false),
                    Hair = table.Column<byte>(nullable: false),
                    GuildIndex = table.Column<int>(nullable: false),
                    CreationIP = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Banned = table.Column<bool>(nullable: false),
                    BanReason = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    ChatBanned = table.Column<bool>(nullable: false),
                    ChatBanExpiryDate = table.Column<DateTime>(nullable: false),
                    LastIP = table.Column<string>(nullable: true),
                    LastDate = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeleteDate = table.Column<DateTime>(nullable: false),
                    Married = table.Column<int>(nullable: false),
                    MarriedDate = table.Column<DateTime>(nullable: false),
                    Mentor = table.Column<int>(nullable: false),
                    MentorDate = table.Column<DateTime>(nullable: false),
                    isMentor = table.Column<bool>(nullable: false),
                    MentorExp = table.Column<long>(nullable: false),
                    CurrentMapIndex = table.Column<int>(nullable: false),
                    CurrentLocationString = table.Column<string>(nullable: true),
                    Direction = table.Column<byte>(nullable: false),
                    BindMapIndex = table.Column<int>(nullable: false),
                    BindLocationString = table.Column<string>(nullable: true),
                    HP = table.Column<ushort>(nullable: false),
                    MP = table.Column<ushort>(nullable: false),
                    Experience = table.Column<long>(nullable: false),
                    AMode = table.Column<byte>(nullable: false),
                    PMode = table.Column<byte>(nullable: false),
                    AllowGroup = table.Column<bool>(nullable: false),
                    AllowTrade = table.Column<bool>(nullable: false),
                    PKPoints = table.Column<int>(nullable: false),
                    NewDay = table.Column<bool>(nullable: false),
                    Thrusting = table.Column<bool>(nullable: false),
                    HalfMoon = table.Column<bool>(nullable: false),
                    CrossHalfMoon = table.Column<bool>(nullable: false),
                    DoubleSlash = table.Column<bool>(nullable: false),
                    MentalState = table.Column<byte>(nullable: false),
                    MentalStateLvl = table.Column<byte>(nullable: false),
                    InventoryString = table.Column<string>(nullable: true),
                    QuestInventoryString = table.Column<string>(nullable: true),
                    EquipmentString = table.Column<string>(nullable: true),
                    RentedItemsBytes = table.Column<byte[]>(nullable: true),
                    HasRentedItem = table.Column<bool>(nullable: false),
                    CurrentRefineItemIndex = table.Column<ulong>(nullable: false),
                    CollectTime = table.Column<long>(nullable: false),
                    MagicsBytes = table.Column<byte[]>(nullable: true),
                    PetsBytes = table.Column<byte[]>(nullable: true),
                    BuffsBytes = table.Column<byte[]>(nullable: true),
                    FriendsBytes = table.Column<byte[]>(nullable: true),
                    IntelligentCreaturesBytes = table.Column<byte[]>(nullable: true),
                    PearlCount = table.Column<int>(nullable: false),
                    CurrentQuestsBytes = table.Column<byte[]>(nullable: true),
                    CompletedQuestsString = table.Column<string>(nullable: true),
                    FlagsString = table.Column<string>(nullable: true),
                    AccountInfoIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Mails",
                columns: table => new
                {
                    MailID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sender = table.Column<string>(nullable: true),
                    RecipientIndex = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Gold = table.Column<uint>(nullable: false),
                    ItemsString = table.Column<string>(nullable: true),
                    DateSent = table.Column<DateTime>(nullable: false),
                    DateOpened = table.Column<DateTime>(nullable: false),
                    Locked = table.Column<bool>(nullable: false),
                    Collected = table.Column<bool>(nullable: false),
                    CanReply = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mails", x => x.MailID);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    UniqueID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemIndex = table.Column<int>(nullable: false),
                    CurrentDura = table.Column<ushort>(nullable: false),
                    MaxDura = table.Column<ushort>(nullable: false),
                    Count = table.Column<uint>(nullable: false),
                    GemCount = table.Column<uint>(nullable: false),
                    AC = table.Column<byte>(nullable: false),
                    MAC = table.Column<byte>(nullable: false),
                    DC = table.Column<byte>(nullable: false),
                    MC = table.Column<byte>(nullable: false),
                    SC = table.Column<byte>(nullable: false),
                    Accuracy = table.Column<byte>(nullable: false),
                    Agility = table.Column<byte>(nullable: false),
                    HP = table.Column<byte>(nullable: false),
                    MP = table.Column<byte>(nullable: false),
                    Strong = table.Column<byte>(nullable: false),
                    MagicResist = table.Column<byte>(nullable: false),
                    PoisonResist = table.Column<byte>(nullable: false),
                    HealthRecovery = table.Column<byte>(nullable: false),
                    ManaRecovery = table.Column<byte>(nullable: false),
                    PoisonRecovery = table.Column<byte>(nullable: false),
                    CriticalRate = table.Column<byte>(nullable: false),
                    CriticalDamage = table.Column<byte>(nullable: false),
                    Freezing = table.Column<byte>(nullable: false),
                    PoisonAttack = table.Column<byte>(nullable: false),
                    AttackSpeed = table.Column<sbyte>(nullable: false),
                    Luck = table.Column<sbyte>(nullable: false),
                    RefinedValue = table.Column<byte>(nullable: false),
                    RefineAdded = table.Column<byte>(nullable: false),
                    DuraChanged = table.Column<bool>(nullable: false),
                    SoulBoundId = table.Column<int>(nullable: false),
                    Identified = table.Column<bool>(nullable: false),
                    Cursed = table.Column<bool>(nullable: false),
                    WeddingRing = table.Column<int>(nullable: false),
                    BuybackExpiryDate = table.Column<DateTime>(nullable: false),
                    SlotString = table.Column<string>(nullable: true),
                    ExpireInfoBytes = table.Column<byte[]>(nullable: true),
                    RentalInformationBytes = table.Column<byte[]>(nullable: true),
                    AwakeBytes = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.UniqueID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Mails");

            migrationBuilder.DropTable(
                name: "UserItems");
        }
    }
}
