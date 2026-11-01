using System.Runtime.CompilerServices;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;
using S = ServerPackets;

// Exercise the production handlers without starting a server or loading player databases.
int checks = 0;
Envir.Main.NextUserItemID = 10000;
void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    checks++;
}

TestPlayer Player()
{
    var p = (TestPlayer)RuntimeHelpers.GetUninitializedObject(typeof(TestPlayer));
    p.Packets = new();
    p.Info = new CharacterInfo { Index = 1, Name = "SecurityTest", Level = 50 };
    p.Account = new AccountInfo { Gold = 1000 };
    p.Info.AccountInfo = p.Account;
    p.Stats = new Stats { [Stat.BagWeight] = int.MaxValue };
    p.Report = new Reporting(p);
    return p;
}

UserItem Item(ulong id, ushort count = 60, ItemInfo info = null) =>
    new(info ?? new ItemInfo { Name = "Test", StackSize = 100, Type = ItemType.Potion })
    { UniqueID = id, Count = count };

void AtNPC(TestPlayer p, string key)
{
    var npc = (NPCObject)RuntimeHelpers.GetUninitializedObject(typeof(NPCObject));
    npc.Info = new NPCInfo();
    var map = (Map)RuntimeHelpers.GetUninitializedObject(typeof(Map));
    map.Info = new MapInfo();
    map.NPCs = new List<NPCObject> { npc };
    p.CurrentMap = map;
    p.NPCObjectID = npc.ObjectID;
    p.NPCPage = new NPCPage(key);
}

MirGridType[] grids = { MirGridType.Inventory, MirGridType.Storage, MirGridType.Equipment,
    MirGridType.Fishing, MirGridType.HeroInventory, MirGridType.HeroEquipment };
foreach (var from in grids)
foreach (var to in grids)
foreach (ushort count in new ushort[] { 1, 49, 50, 51, 60, 99, 100 })
{
    var p = Player();
    var item = Item(7, count);
    p.Info.Inventory[0] = item;
    p.MergeItem(from, to, 7, 7);
    Check(p.Packets.Single() is S.MergeItem { Success: false } && item.Count == count &&
        p.Info.Inventory[0] == item, "Self-merge changed inventory");
}

foreach (ushort source in new ushort[] { 1, 40, 60, 90 })
foreach (ushort destination in new ushort[] { 10, 40, 60, 99 })
{
    var p = Player();
    var a = Item(1, source);
    var b = Item(2, destination, a.Info);
    p.Info.Inventory[0] = a;
    p.Info.Inventory[1] = b;
    // Stop after the production success response, before unrelated world/stat refreshes.
    p.StopAfterMerge = true;
    try { p.MergeItem(MirGridType.Inventory, MirGridType.Inventory, 1, 2); }
    catch (MergeCompleted) { }
    Check(p.Packets.Last() is S.MergeItem { Success: true } &&
        (p.Info.Inventory[0]?.Count ?? 0) + b.Count == source + destination &&
        b.Count == Math.Min(100, source + destination), "Valid merge did not conserve items");
}

Action<UserItem>[] restrictions = {
    item => item.SoulBoundId = 9,
    item => item.RentalInformation = new RentalInformation { BindingFlags = BindMode.DontTrade },
    item => item.ExpireInfo = new ExpireInfo { ExpiryDate = DateTime.MaxValue },
    item => item.SealedInfo = new SealedInfo { ExpiryDate = DateTime.MaxValue },
    item => item.AddedStats[Stat.MaxDC] = 100,
    item => item.Cursed = true,
    item => item.Slots = new[] { Item(99, 1) }
};
foreach (var restrict in restrictions)
{
    var p = Player();
    var a = Item(1);
    var b = Item(2, 20, a.Info);
    restrict(a);
    p.Info.Inventory[0] = a;
    p.Info.Inventory[1] = b;
    p.MergeItem(MirGridType.Inventory, MirGridType.Inventory, 1, 2);
    Check(p.Packets.Last() is S.MergeItem { Success: false } && a.Count == 60 && b.Count == 20,
        "Restricted manual merge accepted");
    p.Info.Inventory[0] = null;
    p.AddForTest(a);
    Check(b.Count == 20 && p.Info.Inventory.Contains(a), "Automatic stacking laundered restrictions");
}

{
    var p = Player();
    var a = Item(1);
    var expiry = new ExpireInfo { ExpiryDate = DateTime.MaxValue };
    a.SoulBoundId = 12; a.ExpireInfo = expiry;
    p.Info.Inventory[0] = a;
    p.SplitItem(MirGridType.Inventory, 1, 20);
    var split = p.Info.Inventory.Single(item => item != null && item != a);
    Check(a.Count == 40 && split.Count == 20 && split.UniqueID != a.UniqueID &&
        split.SoulBoundId == 12 && split.ExpireInfo.ExpiryDate == expiry.ExpiryDate,
        "Split lost restrictions or item counts");
}

{
    var p = Player();
    var mount = Item(1, 1, new ItemInfo { Type = ItemType.Mount, StackSize = 1 });
    var attachment = Item(2, 1);
    mount.Slots = new[] { attachment };
    p.Info.Equipment[(int)EquipmentSlot.Mount] = mount;
    p.Info.Inventory[0] = Item(3, 1);
    p.RemoveSlotItem(MirGridType.Mount, 2, 0, MirGridType.Inventory, 1);
    Check(mount.Slots[0] == attachment && p.Packets.Last() is S.RemoveSlotItem { Success: false },
        "Occupied destination deleted attachment");
    mount.Slots[0] = null;
    p.Info.Inventory[1] = Item(4, 1, new ItemInfo { Type = ItemType.Armour, StackSize = 1 });
    p.EquipSlotItem(MirGridType.Inventory, 4, 0, MirGridType.Mount, 1);
    Check(mount.Slots[0] == null && p.Info.Inventory[1] != null &&
        p.Packets.Last() is S.EquipSlotItem { Success: false }, "Armour accepted in mount attachment slot");
}

{
    var script = (NPCScript)RuntimeHelpers.GetUninitializedObject(typeof(NPCScript));
    script.CraftGoods = new();
    var p = Player();
    script.Craft(p, 999, 1, Array.Empty<int>());
    Check(p.Packets.Last() is S.CraftItem { Success: false }, "Unknown recipe accepted");
    var recipe = (RecipeInfo)RuntimeHelpers.GetUninitializedObject(typeof(RecipeInfo));
    recipe.Item = Item(5, 1);
    recipe.Tools = new(); recipe.Ingredients = new();
    recipe.RequiredClass = new(); recipe.RequiredFlag = new(); recipe.RequiredQuest = new();
    script.CraftGoods.Add(recipe);
    foreach (var slots in new[] { new[] { 46 }, new[] { -1 }, new[] { 0, 0 }, null })
    {
        script.Craft(p, 5, 1, slots);
        Check(p.Packets.Last() is S.CraftItem { Success: false } && p.Account.Gold == 1000,
            "Malformed crafting slots changed funds");
    }
    recipe.RequiredLevel = 100;
    script.Craft(p, 5, 1, Array.Empty<int>());
    Check(p.Packets.Last() is S.CraftItem { Success: false }, "Crafting bypassed level requirement");
    recipe.RequiredLevel = null;
    recipe.Gold = uint.MaxValue / 2 + 1;
    script.Craft(p, 5, 2, Array.Empty<int>());
    Check(p.Account.Gold == 1000 && p.Packets.Last() is S.CraftItem { Success: false },
        "Crafting price wrapped");
    recipe.Gold = 0;
    var toolA = Item(6, 1); var toolB = Item(7, 1);
    recipe.Tools.Add(toolA); recipe.Tools.Add(toolB);
    p.Info.Inventory[0] = Item(8, 1, toolA.Info);
    p.Info.Inventory[1] = Item(9, 1, toolA.Info);
    p.Info.Inventory[0].CurrentDura = p.Info.Inventory[1].CurrentDura = 10000;
    script.Craft(p, 5, 1, new[] { 0, 1 });
    Check(p.Packets.Last() is S.CraftItem { Success: false } && p.Info.Inventory[0].CurrentDura == 10000,
        "Duplicate tools substituted for missing tool");
}

{
    var p = Player();
    p.ItemRentalLockFee(); p.ItemRentalLockItem();
    Check(!p.ItemRentalFeeLocked && !p.ItemRentalItemLocked, "Rental locks accepted without partner");
    var partner = Player();
    p.ItemRentalPartner = partner; partner.ItemRentalPartner = p;
    p.ItemRentalDepositedItem = Item(1, 1);
    p.ItemRentalItemLocked = true;
    p.RetrieveRentalItem(0, 0);
    Check(p.ItemRentalDepositedItem != null && p.Info.Inventory[0] == null,
        "Locked rental item could be withdrawn");
    p.ItemRentalItemLocked = false;
    partner.ItemRentalFeeLocked = true; partner.ItemRentalFeeAmount = 100;
    p.ItemRentalPeriodLength = 1;
    var map = (Map)RuntimeHelpers.GetUninitializedObject(typeof(Map));
    map.Info = new MapInfo();
    p.CurrentMap = map; partner.CurrentMap = map;
    p.Info.CurrentLocation = new(0, 0); partner.Info.CurrentLocation = new(0, 1);
    p.Info.Direction = MirDirection.Down; partner.Info.Direction = MirDirection.Up;
    p.ConfirmItemRental();
    Check(p.ItemRentalDepositedItem != null && partner.ItemRentalFeeAmount == 100,
        "Rental completed without both locks");
}

{
    var p = Player();
    var recipient = new CharacterInfo { Index = 987654, Name = "SecurityRecipient" };
    Envir.Main.CharacterList.Add(recipient);
    var good = Item(1, 1);
    var forbidden = Item(2, 1); forbidden.Info.Bind = BindMode.NoMail;
    var stamp = Item(3, 1, new ItemInfo { Type = ItemType.Nothing, Shape = 1, StackSize = 100 });
    p.Info.Inventory[0] = good; p.Info.Inventory[1] = forbidden; p.Info.Inventory[2] = stamp;
    p.SendMail(recipient.Name, "test", 0, new ulong[] { 1, 2, 0, 0, 0 }, true);
    Check(p.Info.Inventory[0] == good && p.Info.Inventory[2] == stamp && recipient.Mail.Count == 0,
        "Rejected parcel consumed attachments or stamp");
    p.NextMailTime = 0; p.Info.Inventory[2] = null;
    p.SendMail(recipient.Name, "test", 0, new ulong[] { 1, 0, 0, 0, 0 }, true);
    Check(p.Info.Inventory[0] == good && recipient.Mail.Count == 0, "Stamped parcel accepted without stamp");
    Envir.Main.CharacterList.Remove(recipient);
}

{
    var p = Player();
    AtNPC(p, NPCScript.MarketKey);
    var item = Item(1, 1);
    item.RentalInformation = new RentalInformation { BindingFlags = BindMode.DontSell | BindMode.DontTrade };
    p.Info.Inventory[0] = item;
    p.Account.Gold = uint.MaxValue;
    p.ConsignItem(1, Globals.MinConsignment, MarketPanelType.Consign);
    Check(p.Info.Inventory[0] == item && p.Account.Auctions.Count == 0, "Borrowed item was consigned");
}

{
    var p = Player();
    var item = Item(1, 3, new ItemInfo { Name = "Expensive", StackSize = 100, Price = uint.MaxValue / 2 + 1 });
    Check(item.Price() == uint.MaxValue, "Stack price overflowed");
    p.Account.Gold = uint.MaxValue;
    p.ItemRentalPartner = Player();
    p.TradePartner = Player(); p.TradeGoldAmount = uint.MaxValue - 1;
    p.TradeGold(2);
    Check(p.TradeGoldAmount == uint.MaxValue - 1 && p.Account.Gold == uint.MaxValue, "Trade escrow overflowed");
}

{
    var p = Player();
    AtNPC(p, NPCScript.StorageKey);
    p.Account.Storage = new UserItem[Globals.StorageGridSize * 2];
    var source = Item(1, 60);
    p.Account.Storage[0] = source;
    // Leave an accessible early slot empty while all later accessible slots are occupied.
    for (int i = 2; i < Globals.StorageGridSize; i++) p.Account.Storage[i] = Item((ulong)i + 100);
    p.SplitItem(MirGridType.Storage, 1, 20);
    Check(p.Account.Storage[1]?.Count == 20 && source.Count == 40 &&
        p.Account.Storage.Skip(Globals.StorageGridSize).All(item => item == null),
        "Storage split placed items in locked expansion");

    var mount = Item(500, 1, new ItemInfo { Type = ItemType.Mount, StackSize = 1 });
    mount.Slots = new UserItem[5];
    p.Info.Equipment[(int)EquipmentSlot.Mount] = mount;
    p.Account.Storage[Globals.StorageGridSize] = Item(600, 1,
        new ItemInfo { Type = ItemType.Reins, StackSize = 1 });
    p.EquipSlotItem(MirGridType.Storage, 600, 0, MirGridType.Mount, 500);
    Check(mount.Slots[0] == null && p.Account.Storage[Globals.StorageGridSize] != null,
        "Attachment equipment bypassed locked storage");
}

{
    var p = Player();
    Envir.Main.CharacterList.Add(p.Info);
    var info = new ItemInfo { Index = 654321, Name = "SecurityShop", Type = ItemType.Potion, StackSize = 65535 };
    Envir.Main.ItemInfoList.Add(info);
    var product = new GameShopItem { GIndex = 765432, Info = info, Count = 1, Stock = 1,
        iStock = true, CanBuyGold = true, GoldPrice = 1 };
    Envir.Main.GameShopList.Add(product);
    p.Info.GSpurchases[product.GIndex] = 1;
    p.GameshopBuy(product.GIndex, 1, 1);
    Check(p.Account.Gold == 1000 && p.Info.Mail.Count == 0, "Shop bypassed individual stock limit");
    product.Stock = 0; product.GoldPrice = uint.MaxValue / 2 + 1;
    p.Account.Gold = uint.MaxValue;
    p.GameshopBuy(product.GIndex, 2, 1);
    Check(p.Account.Gold == uint.MaxValue && p.Info.Mail.Count == 0, "Shop price wrapped");
    product.GoldPrice = 1; product.Count = 40000;
    p.GameshopBuy(product.GIndex, 2, 1);
    Check(p.Account.Gold == uint.MaxValue - 2 && p.Info.Mail.Single().Items.Sum(item => item.Count) == 80000,
        "Large shop bundle lost quantities or charged incorrect price");
    Envir.Main.GameShopList.Remove(product);
    Envir.Main.ItemInfoList.Remove(info);
    Envir.Main.CharacterList.Remove(p.Info);
}

{
    var script = (NPCScript)RuntimeHelpers.GetUninitializedObject(typeof(NPCScript));
    var goods = Item(1, 90, new ItemInfo { Name = "ShopStock", StackSize = 100, Price = 10 });
    script.Goods = new() { goods };
    var p = Player(); p.Account.Gold = 0;
    p.NPCPage = new NPCPage(NPCScript.BuyKey);
    script.Buy(p, 1, 50);
    Check(goods.Count == 90 && p.Info.Inventory.All(item => item == null),
        "Failed NPC purchase changed shared stock");
    goods.Info.Price = uint.MaxValue;
    Check(goods.Price() == uint.MaxValue, "Maximum unit price became free");
}

{
    bool previous = Packet.IsServer;
    Packet.IsServer = true;
    foreach (int count in new[] { -1, 1, int.MaxValue })
    {
        byte[] bytes = new ClientPackets.CraftItem { UniqueID = 1, Count = 1, Slots = Array.Empty<int>() }
            .GetPacketBytes().ToArray();
        BitConverter.GetBytes(count).CopyTo(bytes, 14);
        bool rejected = false;
        try { Packet.ReceivePacket(bytes, out _); }
        catch (InvalidDataException) { rejected = true; }
        Check(rejected, "Malformed crafting slot count reached allocation");
    }
    var valid = new ClientPackets.CraftItem { UniqueID = 1, Count = 1, Slots = new[] { 0, 1 } };
    Check(Packet.ReceivePacket(valid.GetPacketBytes().ToArray(), out _) is ClientPackets.CraftItem { Slots.Length: 2 },
        "Valid crafting packet failed to decode");
    Packet.IsServer = previous;
}

foreach (int invalid in new[] { int.MinValue, -1, 1000, int.MaxValue })
{
    var p = Player();
    AtNPC(p, NPCScript.StorageKey);
    var item = Item(1);
    p.Info.Inventory[0] = item;
    Action[] requests = {
        () => p.MoveItem(MirGridType.Inventory, 0, invalid),
        () => p.StoreItem(0, invalid),
        () => p.TakeBackItem(invalid, 0),
        () => p.EquipItem(MirGridType.Inventory, 1, invalid),
        () => p.RemoveItem(MirGridType.Inventory, 1, invalid),
        () => p.DepositTradeItem(0, invalid),
        () => p.RetrieveTradeItem(invalid, 0),
        () => p.RetrieveRefineItem(invalid, 0),
        () => p.TakeBackHeroItem(invalid, 0),
        () => p.TransferHeroItem(0, invalid)
    };
    foreach (var request in requests)
    {
        request();
        Check(p.Info.Inventory[0] == item && item.Count == 60 && p.Account.Gold == 1000,
            "Invalid item index changed inventory or funds");
    }
}

{
    var p = Player();
    var a = Item(1, 40);
    var b = Item(2, 30, a.Info);
    p.Info.Inventory[0] = a;
    p.AddForTest(b);
    Check(a.Count == 70 && p.Info.Inventory.Count(item => item != null) == 1,
        "Valid automatic stacking failed");
    AtNPC(p, NPCScript.RefineKey);
    var refining = Item(3, 1);
    p.Info.CurrentRefine = refining;
    p.RefineItem(1);
    Check(p.Info.CurrentRefine == refining && p.Info.Inventory[0] == a,
        "Second refine request overwrote an item in progress");
    AtNPC(p, NPCScript.AwakeningKey);
    p.Awakening(1, (AwakeType)255);
    p.AwakeningNeedMaterials(1, (AwakeType)255);
    Check(p.Info.Inventory[0] == a && p.Account.Gold == 1000, "Invalid awakening type changed inventory");

    p.Account.Gold = uint.MaxValue;
    var mail = new MailInfo(p.Info.Index) { MailID = 10, Gold = 1, Collected = true };
    p.Info.Mail.Add(mail);
    p.CollectMail(10);
    Check(mail.Gold == 1 && p.Account.Gold == uint.MaxValue,
        "Collecting mail at the gold cap destroyed uncollected gold");
}

{
    var p = Player(); var partner = Player();
    p.TradePartner = partner; partner.TradePartner = p;
    var map = (Map)RuntimeHelpers.GetUninitializedObject(typeof(Map)); map.Info = new MapInfo();
    p.CurrentMap = map; partner.CurrentMap = map;
    p.Info.CurrentLocation = new(0, 0); partner.Info.CurrentLocation = new(0, 1);
    p.Info.Direction = MirDirection.Down; partner.Info.Direction = MirDirection.Up;
    p.Info.Trade[0] = Item(1, 1);
    for (int i = 0; i < partner.Info.Inventory.Length; i++) partner.Info.Inventory[i] = Item((ulong)i + 100, 100);
    p.TradeLocked = partner.TradeLocked = true;
    p.TradeConfirm(true);
    Check(!p.TradeLocked && !partner.TradeLocked && p.Info.Trade[0] != null,
        "Failed trade retained consent locks after telling clients they were unlocked");
}

{
    var p = Player(); var partner = Player(); partner.Info.Index = 2;
    p.ItemRentalPartner = partner; partner.ItemRentalPartner = p;
    Envir.Main.CharacterList.Add(p.Info);
    var item = Item(1, 1);
    p.ItemRentalDepositedItem = item;
    for (int i = 0; i < p.Info.Inventory.Length; i++) p.Info.Inventory[i] = Item((ulong)i + 100, 100);
    partner.Account.Gold = 0; partner.ItemRentalFeeAmount = 10;
    p.CancelItemRental();
    Check(p.ItemRentalPartner == null && partner.ItemRentalPartner == null &&
        p.ItemRentalDepositedItem == null && partner.ItemRentalFeeAmount == 0 &&
        partner.Account.Gold == 10 && p.Info.Mail.Single().Items.Single() == item,
        "Full-bag rental cancellation left stale session or lost escrow");
    Envir.Main.CharacterList.Remove(p.Info);
}

Console.WriteLine($"PASS: {checks} item security regression checks.");

sealed class MergeCompleted : Exception { }
sealed class TestPlayer : PlayerObject
{
    public List<Packet> Packets;
    public bool StopAfterMerge;
    public override void Enqueue(Packet packet)
    {
        Packets.Add(packet);
        if (StopAfterMerge && packet is S.MergeItem { Success: true }) throw new MergeCompleted();
    }
    public override void ReceiveChat(string text, ChatType type) { }
    public void AddForTest(UserItem item) => AddItem(item);
}
