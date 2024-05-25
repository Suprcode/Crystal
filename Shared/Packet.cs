namespace Shared;

public abstract class Packet
{
    public static bool IsServer;
    public virtual bool Observable => true;
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
                {
                    //prevents server from getting stuck in a 'loop' (only on this connection)
                    //if the incomming data is corrupt/invalid > simply remove all data instead of trying to process it over and over again
                    extra = new byte[0];
                    return null;
                }

                p.ReadPacket(reader);
            }
            catch
            {
                throw new InvalidDataException();
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
                return new ClientPacket.ClientVersion();
            case (short)ClientPacketIds.Disconnect:
                return new ClientPacket.Disconnect();
            case (short)ClientPacketIds.KeepAlive:
                return new ClientPacket.KeepAlive();
            case (short)ClientPacketIds.NewAccount:
                return new ClientPacket.NewAccount();
            case (short)ClientPacketIds.ChangePassword:
                return new ClientPacket.ChangePassword();
            case (short)ClientPacketIds.Login:
                return new ClientPacket.Login();
            case (short)ClientPacketIds.NewCharacter:
                return new ClientPacket.NewCharacter();
            case (short)ClientPacketIds.DeleteCharacter:
                return new ClientPacket.DeleteCharacter();
            case (short)ClientPacketIds.StartGame:
                return new ClientPacket.StartGame();
            case (short)ClientPacketIds.LogOut:
                return new ClientPacket.LogOut();
            case (short)ClientPacketIds.Turn:
                return new ClientPacket.Turn();
            case (short)ClientPacketIds.Walk:
                return new ClientPacket.Walk();
            case (short)ClientPacketIds.Run:
                return new ClientPacket.Run();
            case (short)ClientPacketIds.Chat:
                return new ClientPacket.Chat();
            case (short)ClientPacketIds.MoveItem:
                return new ClientPacket.MoveItem();
            case (short)ClientPacketIds.StoreItem:
                return new ClientPacket.StoreItem();
            case (short)ClientPacketIds.TakeBackItem:
                return new ClientPacket.TakeBackItem();
            case (short)ClientPacketIds.MergeItem:
                return new ClientPacket.MergeItem();
            case (short)ClientPacketIds.EquipItem:
                return new ClientPacket.EquipItem();
            case (short)ClientPacketIds.RemoveItem:
                return new ClientPacket.RemoveItem();
            case (short)ClientPacketIds.RemoveSlotItem:
                return new ClientPacket.RemoveSlotItem();
            case (short)ClientPacketIds.SplitItem:
                return new ClientPacket.SplitItem();
            case (short)ClientPacketIds.UseItem:
                return new ClientPacket.UseItem();
            case (short)ClientPacketIds.DropItem:
                return new ClientPacket.DropItem();
            case (short)ClientPacketIds.DepositRefineItem:
                return new ClientPacket.DepositRefineItem();
            case (short)ClientPacketIds.RetrieveRefineItem:
                return new ClientPacket.RetrieveRefineItem();
            case (short)ClientPacketIds.RefineCancel:
                return new ClientPacket.RefineCancel();
            case (short)ClientPacketIds.RefineItem:
                return new ClientPacket.RefineItem();
            case (short)ClientPacketIds.CheckRefine:
                return new ClientPacket.CheckRefine();
            case (short)ClientPacketIds.ReplaceWedRing:
                return new ClientPacket.ReplaceWedRing();
            case (short)ClientPacketIds.DepositTradeItem:
                return new ClientPacket.DepositTradeItem();
            case (short)ClientPacketIds.RetrieveTradeItem:
                return new ClientPacket.RetrieveTradeItem();
            case (short)ClientPacketIds.TakeBackHeroItem:
                return new ClientPacket.TakeBackHeroItem();
            case (short)ClientPacketIds.TransferHeroItem:
                return new ClientPacket.TransferHeroItem();
            case (short)ClientPacketIds.DropGold:
                return new ClientPacket.DropGold();
            case (short)ClientPacketIds.PickUp:
                return new ClientPacket.PickUp();
            case (short)ClientPacketIds.RequestMapInfo:
                return new ClientPacket.RequestMapInfo();
            case (short)ClientPacketIds.TeleportToNpc:
                return new ClientPacket.TeleportToNpc();
            case (short)ClientPacketIds.SearchMap:
                return new ClientPacket.SearchMap();
            case (short)ClientPacketIds.Inspect:
                return new ClientPacket.Inspect();
            case (short)ClientPacketIds.Observe:
                return new ClientPacket.Observe();
            case (short)ClientPacketIds.ChangeAMode:
                return new ClientPacket.ChangeAMode();
            case (short)ClientPacketIds.ChangePMode:
                return new ClientPacket.ChangePMode();
            case (short)ClientPacketIds.ChangeTrade:
                return new ClientPacket.ChangeTrade();
            case (short)ClientPacketIds.Attack:
                return new ClientPacket.Attack();
            case (short)ClientPacketIds.RangeAttack:
                return new ClientPacket.RangeAttack();
            case (short)ClientPacketIds.Harvest:
                return new ClientPacket.Harvest();
            case (short)ClientPacketIds.CallNpc:
                return new ClientPacket.CallNpc();
            case (short)ClientPacketIds.BuyItem:
                return new ClientPacket.BuyItem();
            case (short)ClientPacketIds.SellItem:
                return new ClientPacket.SellItem();
            case (short)ClientPacketIds.CraftItem:
                return new ClientPacket.CraftItem();
            case (short)ClientPacketIds.RepairItem:
                return new ClientPacket.RepairItem();
            case (short)ClientPacketIds.BuyItemBack:
                return new ClientPacket.BuyItemBack();
            case (short)ClientPacketIds.SRepairItem:
                return new ClientPacket.SRepairItem();
            case (short)ClientPacketIds.MagicKey:
                return new ClientPacket.MagicKey();
            case (short)ClientPacketIds.Magic:
                return new ClientPacket.Magic();
            case (short)ClientPacketIds.SwitchGroup:
                return new ClientPacket.SwitchGroup();
            case (short)ClientPacketIds.AddMember:
                return new ClientPacket.AddMember();
            case (short)ClientPacketIds.DellMember:
                return new ClientPacket.DelMember();
            case (short)ClientPacketIds.GroupInvite:
                return new ClientPacket.GroupInvite();
            case (short)ClientPacketIds.NewHero:
                return new ClientPacket.NewHero();
            case (short)ClientPacketIds.SetAutoPotValue:
                return new ClientPacket.SetAutoPotValue();
            case (short)ClientPacketIds.SetAutoPotItem:
                return new ClientPacket.SetAutoPotItem();
            case (short)ClientPacketIds.SetHeroBehaviour:
                return new ClientPacket.SetHeroBehaviour();
            case (short)ClientPacketIds.ChangeHero:
                return new ClientPacket.ChangeHero();
            case (short)ClientPacketIds.TownRevive:
                return new ClientPacket.TownRevive();
            case (short)ClientPacketIds.SpellToggle:
                return new ClientPacket.SpellToggle();
            case (short)ClientPacketIds.ConsignItem:
                return new ClientPacket.ConsignItem();
            case (short)ClientPacketIds.MarketSearch:
                return new ClientPacket.MarketSearch();
            case (short)ClientPacketIds.MarketRefresh:
                return new ClientPacket.MarketRefresh();
            case (short)ClientPacketIds.MarketPage:
                return new ClientPacket.MarketPage();
            case (short)ClientPacketIds.MarketBuy:
                return new ClientPacket.MarketBuy();
            case (short)ClientPacketIds.MarketGetBack:
                return new ClientPacket.MarketGetBack();
            case (short)ClientPacketIds.MarketSellNow:
                return new ClientPacket.MarketSellNow();
            case (short)ClientPacketIds.RequestUserName:
                return new ClientPacket.RequestUserName();
            case (short)ClientPacketIds.RequestChatItem:
                return new ClientPacket.RequestChatItem();
            case (short)ClientPacketIds.EditGuildMember:
                return new ClientPacket.EditGuildMember();
            case (short)ClientPacketIds.EditGuildNotice:
                return new ClientPacket.EditGuildNotice();
            case (short)ClientPacketIds.GuildInvite:
                return new ClientPacket.GuildInvite();
            case (short)ClientPacketIds.GuildNameReturn:
                return new ClientPacket.GuildNameReturn();
            case (short)ClientPacketIds.RequestGuildInfo:
                return new ClientPacket.RequestGuildInfo();
            case (short)ClientPacketIds.GuildStorageGoldChange:
                return new ClientPacket.GuildStorageGoldChange();
            case (short)ClientPacketIds.GuildStorageItemChange:
                return new ClientPacket.GuildStorageItemChange();
            case (short)ClientPacketIds.GuildWarReturn:
                return new ClientPacket.GuildWarReturn();
            case (short)ClientPacketIds.MarriageRequest:
                return new ClientPacket.MarriageRequest();
            case (short)ClientPacketIds.MarriageReply:
                return new ClientPacket.MarriageReply();
            case (short)ClientPacketIds.ChangeMarriage:
                return new ClientPacket.ChangeMarriage();
            case (short)ClientPacketIds.DivorceRequest:
                return new ClientPacket.DivorceRequest();
            case (short)ClientPacketIds.DivorceReply:
                return new ClientPacket.DivorceReply();
            case (short)ClientPacketIds.AddMentor:
                return new ClientPacket.AddMentor();
            case (short)ClientPacketIds.MentorReply:
                return new ClientPacket.MentorReply();
            case (short)ClientPacketIds.AllowMentor:
                return new ClientPacket.AllowMentor();
            case (short)ClientPacketIds.CancelMentor:
                return new ClientPacket.CancelMentor();
            case (short)ClientPacketIds.TradeRequest:
                return new ClientPacket.TradeRequest();
            case (short)ClientPacketIds.TradeReply:
                return new ClientPacket.TradeReply();
            case (short)ClientPacketIds.TradeGold:
                return new ClientPacket.TradeGold();
            case (short)ClientPacketIds.TradeConfirm:
                return new ClientPacket.TradeConfirm();
            case (short)ClientPacketIds.TradeCancel:
                return new ClientPacket.TradeCancel();
            case (short)ClientPacketIds.EquipSlotItem:
                return new ClientPacket.EquipSlotItem();
            case (short)ClientPacketIds.FishingCast:
                return new ClientPacket.FishingCast();
            case (short)ClientPacketIds.FishingChangeAutocast:
                return new ClientPacket.FishingChangeAutocast();
            case (short)ClientPacketIds.AcceptQuest:
                return new ClientPacket.AcceptQuest();
            case (short)ClientPacketIds.FinishQuest:
                return new ClientPacket.FinishQuest();
            case (short)ClientPacketIds.AbandonQuest:
                return new ClientPacket.AbandonQuest();
            case (short)ClientPacketIds.ShareQuest:
                return new ClientPacket.ShareQuest();
            case (short)ClientPacketIds.AcceptReincarnation:
                return new ClientPacket.AcceptReincarnation();
            case (short)ClientPacketIds.CancelReincarnation:
                return new ClientPacket.CancelReincarnation();
            case (short)ClientPacketIds.CombineItem:
                return new ClientPacket.CombineItem();
            case (short)ClientPacketIds.AwakeningNeedMaterials:
                return new ClientPacket.AwakeningNeedMaterials();
            case (short)ClientPacketIds.AwakeningLockedItem:
                return new ClientPacket.AwakeningLockedItem();
            case (short)ClientPacketIds.Awakening:
                return new ClientPacket.Awakening();
            case (short)ClientPacketIds.DisassembleItem:
                return new ClientPacket.DisassembleItem();
            case (short)ClientPacketIds.DowngradeAwakening:
                return new ClientPacket.DowngradeAwakening();
            case (short)ClientPacketIds.ResetAddedItem:
                return new ClientPacket.ResetAddedItem();
            case (short)ClientPacketIds.SendMail:
                return new ClientPacket.SendMail();
            case (short)ClientPacketIds.ReadMail:
                return new ClientPacket.ReadMail();
            case (short)ClientPacketIds.CollectParcel:
                return new ClientPacket.CollectParcel();
            case (short)ClientPacketIds.DeleteMail:
                return new ClientPacket.DeleteMail();
            case (short)ClientPacketIds.LockMail:
                return new ClientPacket.LockMail();
            case (short)ClientPacketIds.MailLockedItem:
                return new ClientPacket.MailLockedItem();
            case (short)ClientPacketIds.MailCost:
                return new ClientPacket.MailCost();
            case (short)ClientPacketIds.RequestIntelligentCreatureUpdates:
                return new ClientPacket.RequestIntelligentCreatureUpdates();
            case (short)ClientPacketIds.UpdateIntelligentCreature:
                return new ClientPacket.UpdateIntelligentCreature();
            case (short)ClientPacketIds.IntelligentCreaturePickup:
                return new ClientPacket.IntelligentCreaturePickup();
            case (short)ClientPacketIds.AddFriend:
                return new ClientPacket.AddFriend();
            case (short)ClientPacketIds.RemoveFriend:
                return new ClientPacket.RemoveFriend();
            case (short)ClientPacketIds.RefreshFriends:
                return new ClientPacket.RefreshFriends();
            case (short)ClientPacketIds.AddMemo:
                return new ClientPacket.AddMemo();
            case (short)ClientPacketIds.GuildBuffUpdate:
                return new ClientPacket.GuildBuffUpdate();
            case (short)ClientPacketIds.GameshopBuy:
                return new ClientPacket.GameshopBuy();
            case (short)ClientPacketIds.NpcConfirmInput:
                return new ClientPacket.NpcConfirmInput();
            case (short)ClientPacketIds.ReportIssue:
                return new ClientPacket.ReportIssue();
            case (short)ClientPacketIds.GetRanking:
                return new ClientPacket.GetRanking();
            case (short)ClientPacketIds.Opendoor:
                return new ClientPacket.Opendoor();
            case (short)ClientPacketIds.GetRentedItems:
                return new ClientPacket.GetRentedItems();
            case (short)ClientPacketIds.ItemRentalRequest:
                return new ClientPacket.ItemRentalRequest();
            case (short)ClientPacketIds.ItemRentalFee:
                return new ClientPacket.ItemRentalFee();
            case (short)ClientPacketIds.ItemRentalPeriod:
                return new ClientPacket.ItemRentalPeriod();
            case (short)ClientPacketIds.DepositRentalItem:
                return new ClientPacket.DepositRentalItem();
            case (short)ClientPacketIds.RetrieveRentalItem:
                return new ClientPacket.RetrieveRentalItem();
            case (short)ClientPacketIds.CancelItemRental:
                return new ClientPacket.CancelItemRental();
            case (short)ClientPacketIds.ItemRentalLockFee:
                return new ClientPacket.ItemRentalLockFee();
            case (short)ClientPacketIds.ItemRentalLockItem:
                return new ClientPacket.ItemRentalLockItem();
            case (short)ClientPacketIds.ConfirmItemRental:
                return new ClientPacket.ConfirmItemRental();
            default:
                return null;
        }

    }
    public static Packet GetServerPacket(short index)
    {
        switch (index)
        {
            case (short)ServerPacketIds.Connected:
                return new ServerPacket.Connected();
            case (short)ServerPacketIds.ClientVersion:
                return new ServerPacket.ClientVersion();
            case (short)ServerPacketIds.Disconnect:
                return new ServerPacket.Disconnect();
            case (short)ServerPacketIds.KeepAlive:
                return new ServerPacket.KeepAlive();
            case (short)ServerPacketIds.NewAccount:
                return new ServerPacket.NewAccount();
            case (short)ServerPacketIds.ChangePassword:
                return new ServerPacket.ChangePassword();
            case (short)ServerPacketIds.ChangePasswordBanned:
                return new ServerPacket.ChangePasswordBanned();
            case (short)ServerPacketIds.Login:
                return new ServerPacket.Login();
            case (short)ServerPacketIds.LoginBanned:
                return new ServerPacket.LoginBanned();
            case (short)ServerPacketIds.LoginSuccess:
                return new ServerPacket.LoginSuccess();
            case (short)ServerPacketIds.NewCharacter:
                return new ServerPacket.NewCharacter();
            case (short)ServerPacketIds.NewCharacterSuccess:
                return new ServerPacket.NewCharacterSuccess();
            case (short)ServerPacketIds.DeleteCharacter:
                return new ServerPacket.DeleteCharacter();
            case (short)ServerPacketIds.DeleteCharacterSuccess:
                return new ServerPacket.DeleteCharacterSuccess();
            case (short)ServerPacketIds.StartGame:
                return new ServerPacket.StartGame();
            case (short)ServerPacketIds.StartGameBanned:
                return new ServerPacket.StartGameBanned();
            case (short)ServerPacketIds.StartGameDelay:
                return new ServerPacket.StartGameDelay();
            case (short)ServerPacketIds.MapInformation:
                return new ServerPacket.MapInformation();
            case (short)ServerPacketIds.NewMapInfo:
                return new ServerPacket.NewMapInfo();
            case (short)ServerPacketIds.WorldMapSetup:
                return new ServerPacket.WorldMapSetupInfo();
            case (short)ServerPacketIds.SearchMapResult:
                return new ServerPacket.SearchMapResult();
            case (short)ServerPacketIds.UserInformation:
                return new ServerPacket.UserInformation();
            case (short)ServerPacketIds.UserSlotsRefresh:
                return new ServerPacket.UserSlotsRefresh();
            case (short)ServerPacketIds.UserLocation:
                return new ServerPacket.UserLocation();
            case (short)ServerPacketIds.ObjectPlayer:
                return new ServerPacket.ObjectPlayer();
            case (short)ServerPacketIds.ObjectHero:
                return new ServerPacket.ObjectHero();
            case (short)ServerPacketIds.ObjectRemove:
                return new ServerPacket.ObjectRemove();
            case (short)ServerPacketIds.ObjectTurn:
                return new ServerPacket.ObjectTurn();
            case (short)ServerPacketIds.ObjectWalk:
                return new ServerPacket.ObjectWalk();
            case (short)ServerPacketIds.ObjectRun:
                return new ServerPacket.ObjectRun();
            case (short)ServerPacketIds.Chat:
                return new ServerPacket.Chat();
            case (short)ServerPacketIds.ObjectChat:
                return new ServerPacket.ObjectChat();
            case (short)ServerPacketIds.NewItemInfo:
                return new ServerPacket.NewItemInfo();
            case (short)ServerPacketIds.NewHeroInfo:
                return new ServerPacket.NewHeroInfo();
            case (short)ServerPacketIds.NewChatItem:
                return new ServerPacket.NewChatItem();
            case (short)ServerPacketIds.MoveItem:
                return new ServerPacket.MoveItem();
            case (short)ServerPacketIds.EquipItem:
                return new ServerPacket.EquipItem();
            case (short)ServerPacketIds.MergeItem:
                return new ServerPacket.MergeItem();
            case (short)ServerPacketIds.RemoveItem:
                return new ServerPacket.RemoveItem();
            case (short)ServerPacketIds.RemoveSlotItem:
                return new ServerPacket.RemoveSlotItem();
            case (short)ServerPacketIds.TakeBackItem:
                return new ServerPacket.TakeBackItem();
            case (short)ServerPacketIds.TakeBackHeroItem:
                return new ServerPacket.TakeBackHeroItem();
            case (short)ServerPacketIds.TransferHeroItem:
                return new ServerPacket.TransferHeroItem();
            case (short)ServerPacketIds.StoreItem:
                return new ServerPacket.StoreItem();
            case (short)ServerPacketIds.DepositRefineItem:
                return new ServerPacket.DepositRefineItem();
            case (short)ServerPacketIds.RetrieveRefineItem:
                return new ServerPacket.RetrieveRefineItem();
            case (short)ServerPacketIds.RefineItem:
                return new ServerPacket.RefineItem();
            case (short)ServerPacketIds.DepositTradeItem:
                return new ServerPacket.DepositTradeItem();
            case (short)ServerPacketIds.RetrieveTradeItem:
                return new ServerPacket.RetrieveTradeItem();
            case (short)ServerPacketIds.SplitItem:
                return new ServerPacket.SplitItem();
            case (short)ServerPacketIds.SplitItem1:
                return new ServerPacket.SplitItem1();
            case (short)ServerPacketIds.UseItem:
                return new ServerPacket.UseItem();
            case (short)ServerPacketIds.DropItem:
                return new ServerPacket.DropItem();
            case (short)ServerPacketIds.PlayerUpdate:
                return new ServerPacket.PlayerUpdate();
            case (short)ServerPacketIds.PlayerInspect:
                return new ServerPacket.PlayerInspect();
            case (short)ServerPacketIds.LogOutSuccess:
                return new ServerPacket.LogOutSuccess();
            case (short)ServerPacketIds.LogOutFailed:
                return new ServerPacket.LogOutFailed();
            case (short)ServerPacketIds.ReturnToLogin:
                return new ServerPacket.ReturnToLogin();
            case (short)ServerPacketIds.TimeOfDay:
                return new ServerPacket.TimeOfDay();
            case (short)ServerPacketIds.ChangeAMode:
                return new ServerPacket.ChangeAMode();
            case (short)ServerPacketIds.ChangePMode:
                return new ServerPacket.ChangePMode();
            case (short)ServerPacketIds.ObjectItem:
                return new ServerPacket.ObjectItem();
            case (short)ServerPacketIds.ObjectGold:
                return new ServerPacket.ObjectGold();
            case (short)ServerPacketIds.GainedItem:
                return new ServerPacket.GainedItem();
            case (short)ServerPacketIds.GainedGold:
                return new ServerPacket.GainedGold();
            case (short)ServerPacketIds.LoseGold:
                return new ServerPacket.LoseGold();
            case (short)ServerPacketIds.GainedCredit:
                return new ServerPacket.GainedCredit();
            case (short)ServerPacketIds.LoseCredit:
                return new ServerPacket.LoseCredit();
            case (short)ServerPacketIds.ObjectMonster:
                return new ServerPacket.ObjectMonster();
            case (short)ServerPacketIds.ObjectAttack:
                return new ServerPacket.ObjectAttack();
            case (short)ServerPacketIds.Struck:
                return new ServerPacket.Struck();
            case (short)ServerPacketIds.DamageIndicator:
                return new ServerPacket.DamageIndicator();
            case (short)ServerPacketIds.ObjectStruck:
                return new ServerPacket.ObjectStruck();
            case (short)ServerPacketIds.DuraChanged:
                return new ServerPacket.DuraChanged();
            case (short)ServerPacketIds.HealthChanged:
                return new ServerPacket.HealthChanged();
            case (short)ServerPacketIds.HeroHealthChanged:
                return new ServerPacket.HeroHealthChanged();
            case (short)ServerPacketIds.DeleteItem:
                return new ServerPacket.DeleteItem();
            case (short)ServerPacketIds.Death:
                return new ServerPacket.Death();
            case (short)ServerPacketIds.ObjectDied:
                return new ServerPacket.ObjectDied();
            case (short)ServerPacketIds.ColourChanged:
                return new ServerPacket.ColourChanged();
            case (short)ServerPacketIds.ObjectColourChanged:
                return new ServerPacket.ObjectColourChanged();
            case (short)ServerPacketIds.ObjectGuildNameChanged:
                return new ServerPacket.ObjectGuildNameChanged();
            case (short)ServerPacketIds.GainExperience:
                return new ServerPacket.GainExperience();
            case (short)ServerPacketIds.GainHeroExperience:
                return new ServerPacket.GainHeroExperience();
            case (short)ServerPacketIds.LevelChanged:
                return new ServerPacket.LevelChanged();
            case (short)ServerPacketIds.HeroLevelChanged:
                return new ServerPacket.HeroLevelChanged();
            case (short)ServerPacketIds.ObjectLeveled:
                return new ServerPacket.ObjectLeveled();
            case (short)ServerPacketIds.ObjectHarvest:
                return new ServerPacket.ObjectHarvest();
            case (short)ServerPacketIds.ObjectHarvested:
                return new ServerPacket.ObjectHarvested();
            case (short)ServerPacketIds.ObjectNpc:
                return new ServerPacket.ObjectNpc();
            case (short)ServerPacketIds.NpcResponse:
                return new ServerPacket.NpcResponse();
            case (short)ServerPacketIds.ObjectHide:
                return new ServerPacket.ObjectHide();
            case (short)ServerPacketIds.ObjectShow:
                return new ServerPacket.ObjectShow();
            case (short)ServerPacketIds.Poisoned:
                return new ServerPacket.Poisoned();
            case (short)ServerPacketIds.ObjectPoisoned:
                return new ServerPacket.ObjectPoisoned();
            case (short)ServerPacketIds.MapChanged:
                return new ServerPacket.MapChanged();
            case (short)ServerPacketIds.ObjectTeleportOut:
                return new ServerPacket.ObjectTeleportOut();
            case (short)ServerPacketIds.ObjectTeleportIn:
                return new ServerPacket.ObjectTeleportIn();
            case (short)ServerPacketIds.TeleportIn:
                return new ServerPacket.TeleportIn();
            case (short)ServerPacketIds.NpcGoods:
                return new ServerPacket.NpcGoods();
            case (short)ServerPacketIds.NpcSell:
                return new ServerPacket.NpcSell();
            case (short)ServerPacketIds.NpcRepair:
                return new ServerPacket.NpcRepair();
            case (short)ServerPacketIds.NpcSRepair:
                return new ServerPacket.NpcSRepair();
            case (short)ServerPacketIds.NpcRefine:
                return new ServerPacket.NpcRefine();
            case (short)ServerPacketIds.NpcCheckRefine:
                return new ServerPacket.NpcCheckRefine();
            case (short)ServerPacketIds.NpcCollectRefine:
                return new ServerPacket.NpcCollectRefine();
            case (short)ServerPacketIds.NpcReplaceWedRing:
                return new ServerPacket.NpcReplaceWedRing();
            case (short)ServerPacketIds.NpcStorage:
                return new ServerPacket.NpcStorage();
            case (short)ServerPacketIds.SellItem:
                return new ServerPacket.SellItem();
            case (short)ServerPacketIds.CraftItem:
                return new ServerPacket.CraftItem();
            case (short)ServerPacketIds.RepairItem:
                return new ServerPacket.RepairItem();
            case (short)ServerPacketIds.ItemRepaired:
                return new ServerPacket.ItemRepaired();
            case (short)ServerPacketIds.ItemSlotSizeChanged:
                return new ServerPacket.ItemSlotSizeChanged();
            case (short)ServerPacketIds.ItemSealChanged:
                return new ServerPacket.ItemSealChanged();
            case (short)ServerPacketIds.NewMagic:
                return new ServerPacket.NewMagic();
            case (short)ServerPacketIds.MagicLeveled:
                return new ServerPacket.MagicLeveled();
            case (short)ServerPacketIds.Magic:
                return new ServerPacket.Magic();
            case (short)ServerPacketIds.MagicDelay:
                return new ServerPacket.MagicDelay();
            case (short)ServerPacketIds.MagicCast:
                return new ServerPacket.MagicCast();
            case (short)ServerPacketIds.ObjectMagic:
                return new ServerPacket.ObjectMagic();
            case (short)ServerPacketIds.ObjectProjectile:
                return new ServerPacket.ObjectProjectile();
            case (short)ServerPacketIds.ObjectEffect:
                return new ServerPacket.ObjectEffect();
            case (short)ServerPacketIds.RangeAttack:
                return new ServerPacket.RangeAttack();
            case (short)ServerPacketIds.Pushed:
                return new ServerPacket.Pushed();
            case (short)ServerPacketIds.ObjectPushed:
                return new ServerPacket.ObjectPushed();
            case (short)ServerPacketIds.ObjectName:
                return new ServerPacket.ObjectName();
            case (short)ServerPacketIds.UserStorage:
                return new ServerPacket.UserStorage();
            case (short)ServerPacketIds.SwitchGroup:
                return new ServerPacket.SwitchGroup();
            case (short)ServerPacketIds.DeleteGroup:
                return new ServerPacket.DeleteGroup();
            case (short)ServerPacketIds.DeleteMember:
                return new ServerPacket.DeleteMember();
            case (short)ServerPacketIds.GroupInvite:
                return new ServerPacket.GroupInvite();
            case (short)ServerPacketIds.AddMember:
                return new ServerPacket.AddMember();
            case (short)ServerPacketIds.GroupMembersMap:
                return new ServerPacket.GroupMembersMap();
            case (short)ServerPacketIds.SendMemberLocation:
                return new ServerPacket.SendMemberLocation();
            case (short)ServerPacketIds.Revived:
                return new ServerPacket.Revived();
            case (short)ServerPacketIds.ObjectRevived:
                return new ServerPacket.ObjectRevived();
            case (short)ServerPacketIds.SpellToggle:
                return new ServerPacket.SpellToggle();
            case (short)ServerPacketIds.ObjectHealth:
                return new ServerPacket.ObjectHealth();
            case (short)ServerPacketIds.ObjectMana:
                return new ServerPacket.ObjectMana();
            case (short)ServerPacketIds.MapEffect:
                return new ServerPacket.MapEffect();
            case (short)ServerPacketIds.AllowObserve:
                return new ServerPacket.AllowObserve();
            case (short)ServerPacketIds.ObjectRangeAttack:
                return new ServerPacket.ObjectRangeAttack();
            case (short)ServerPacketIds.AddBuff:
                return new ServerPacket.AddBuff();
            case (short)ServerPacketIds.RemoveBuff:
                return new ServerPacket.RemoveBuff();
            case (short)ServerPacketIds.PauseBuff:
                return new ServerPacket.PauseBuff();
            case (short)ServerPacketIds.ObjectHidden:
                return new ServerPacket.ObjectHidden();
            case (short)ServerPacketIds.RefreshItem:
                return new ServerPacket.RefreshItem();
            case (short)ServerPacketIds.ObjectSpell:
                return new ServerPacket.ObjectSpell();
            case (short)ServerPacketIds.UserDash:
                return new ServerPacket.UserDash();
            case (short)ServerPacketIds.ObjectDash:
                return new ServerPacket.ObjectDash();
            case (short)ServerPacketIds.UserDashFail:
                return new ServerPacket.UserDashFail();
            case (short)ServerPacketIds.ObjectDashFail:
                return new ServerPacket.ObjectDashFail();
            case (short)ServerPacketIds.NpcConsign:
                return new ServerPacket.NpcConsign();
            case (short)ServerPacketIds.NpcMarket:
                return new ServerPacket.NpcMarket();
            case (short)ServerPacketIds.NpcMarketPage:
                return new ServerPacket.NpcMarketPage();
            case (short)ServerPacketIds.ConsignItem:
                return new ServerPacket.ConsignItem();
            case (short)ServerPacketIds.MarketFail:
                return new ServerPacket.MarketFail();
            case (short)ServerPacketIds.MarketSuccess:
                return new ServerPacket.MarketSuccess();
            case (short)ServerPacketIds.ObjectSitDown:
                return new ServerPacket.ObjectSitDown();
            case (short)ServerPacketIds.InTrapRock:
                return new ServerPacket.InTrapRock();
            case (short)ServerPacketIds.RemoveMagic:
                return new ServerPacket.RemoveMagic();
            case (short)ServerPacketIds.BaseStatsInfo:
                return new ServerPacket.BaseStatsInfo();
            case (short)ServerPacketIds.HeroBaseStatsInfo:
                return new ServerPacket.HeroBaseStatsInfo();
            case (short)ServerPacketIds.UserName:
                return new ServerPacket.UserName();
            case (short)ServerPacketIds.ChatItemStats:
                return new ServerPacket.ChatItemStats();
            case (short)ServerPacketIds.GuildMemberChange:
                return new ServerPacket.GuildMemberChange();
            case (short)ServerPacketIds.GuildNoticeChange:
                return new ServerPacket.GuildNoticeChange();
            case (short)ServerPacketIds.GuildStatus:
                return new ServerPacket.GuildStatus();
            case (short)ServerPacketIds.GuildInvite:
                return new ServerPacket.GuildInvite();
            case (short)ServerPacketIds.GuildExpGain:
                return new ServerPacket.GuildExpGain();
            case (short)ServerPacketIds.GuildNameRequest:
                return new ServerPacket.GuildNameRequest();
            case (short)ServerPacketIds.GuildStorageGoldChange:
                return new ServerPacket.GuildStorageGoldChange();
            case (short)ServerPacketIds.GuildStorageItemChange:
                return new ServerPacket.GuildStorageItemChange();
            case (short)ServerPacketIds.GuildStorageList:
                return new ServerPacket.GuildStorageList();
            case (short)ServerPacketIds.GuildRequestWar:
                return new ServerPacket.GuildRequestWar();
            case (short)ServerPacketIds.HeroCreateRequest:
                return new ServerPacket.HeroCreateRequest();
            case (short)ServerPacketIds.NewHero:
                return new ServerPacket.NewHero();
            case (short)ServerPacketIds.HeroInformation:
                return new ServerPacket.HeroInformation();
            case (short)ServerPacketIds.UpdateHeroSpawnState:
                return new ServerPacket.UpdateHeroSpawnState();
            case (short)ServerPacketIds.UnlockHeroAutoPot:
                return new ServerPacket.UnlockHeroAutoPot();
            case (short)ServerPacketIds.SetAutoPotValue:
                return new ServerPacket.SetAutoPotValue();
            case (short)ServerPacketIds.SetAutoPotItem:
                return new ServerPacket.SetAutoPotItem();
            case (short)ServerPacketIds.SetHeroBehaviour:
                return new ServerPacket.SetHeroBehaviour();
            case (short)ServerPacketIds.ManageHeroes:
                return new ServerPacket.ManageHeroes();
            case (short)ServerPacketIds.ChangeHero:
                return new ServerPacket.ChangeHero();
            case (short)ServerPacketIds.DefaultNpc:
                return new ServerPacket.DefaultNpc();
            case (short)ServerPacketIds.NpcUpdate:
                return new ServerPacket.NpcUpdate();
            case (short)ServerPacketIds.NpcImageUpdate:
                return new ServerPacket.NpcImageUpdate();
            case (short)ServerPacketIds.MarriageRequest:
                return new ServerPacket.MarriageRequest();
            case (short)ServerPacketIds.DivorceRequest:
                return new ServerPacket.DivorceRequest();
            case (short)ServerPacketIds.MentorRequest:
                return new ServerPacket.MentorRequest();
            case (short)ServerPacketIds.TradeRequest:
                return new ServerPacket.TradeRequest();
            case (short)ServerPacketIds.TradeAccept:
                return new ServerPacket.TradeAccept();
            case (short)ServerPacketIds.TradeGold:
                return new ServerPacket.TradeGold();
            case (short)ServerPacketIds.TradeItem:
                return new ServerPacket.TradeItem();
            case (short)ServerPacketIds.TradeConfirm:
                return new ServerPacket.TradeConfirm();
            case (short)ServerPacketIds.TradeCancel:
                return new ServerPacket.TradeCancel();
            case (short)ServerPacketIds.MountUpdate:
                return new ServerPacket.MountUpdate();
            case (short)ServerPacketIds.TransformUpdate:
                return new ServerPacket.TransformUpdate();
            case (short)ServerPacketIds.EquipSlotItem:
                return new ServerPacket.EquipSlotItem();
            case (short)ServerPacketIds.FishingUpdate:
                return new ServerPacket.FishingUpdate();
            case (short)ServerPacketIds.ChangeQuest:
                return new ServerPacket.ChangeQuest();
            case (short)ServerPacketIds.CompleteQuest:
                return new ServerPacket.CompleteQuest();
            case (short)ServerPacketIds.ShareQuest:
                return new ServerPacket.ShareQuest();
            case (short)ServerPacketIds.NewQuestInfo:
                return new ServerPacket.NewQuestInfo();
            case (short)ServerPacketIds.GainedQuestItem:
                return new ServerPacket.GainedQuestItem();
            case (short)ServerPacketIds.DeleteQuestItem:
                return new ServerPacket.DeleteQuestItem();
            case (short)ServerPacketIds.CancelReincarnation:
                return new ServerPacket.CancelReincarnation();
            case (short)ServerPacketIds.RequestReincarnation:
                return new ServerPacket.RequestReincarnation();
            case (short)ServerPacketIds.UserBackStep:
                return new ServerPacket.UserBackStep();
            case (short)ServerPacketIds.ObjectBackStep:
                return new ServerPacket.ObjectBackStep();
            case (short)ServerPacketIds.UserDashAttack:
                return new ServerPacket.UserDashAttack();
            case (short)ServerPacketIds.ObjectDashAttack:
                return new ServerPacket.ObjectDashAttack();
            case (short)ServerPacketIds.UserAttackMove://Warrior Skill - SlashingBurst
                return new ServerPacket.UserAttackMove();
            case (short)ServerPacketIds.CombineItem:
                return new ServerPacket.CombineItem();
            case (short)ServerPacketIds.ItemUpgraded:
                return new ServerPacket.ItemUpgraded();
            case (short)ServerPacketIds.SetConcentration:
                return new ServerPacket.SetConcentration();
            case (short)ServerPacketIds.SetElemental:
                return new ServerPacket.SetElemental();
            case (short)ServerPacketIds.RemoveDelayedExplosion:
                return new ServerPacket.RemoveDelayedExplosion();
            case (short)ServerPacketIds.ObjectDeco:
                return new ServerPacket.ObjectDeco();
            case (short)ServerPacketIds.ObjectSneaking:
                return new ServerPacket.ObjectSneaking();
            case (short)ServerPacketIds.ObjectLevelEffects:
                return new ServerPacket.ObjectLevelEffects();
            case (short)ServerPacketIds.SetBindingShot:
                return new ServerPacket.SetBindingShot();
            case (short)ServerPacketIds.SendOutputMessage:
                return new ServerPacket.SendOutputMessage();
            case (short)ServerPacketIds.NpcAwakening:
                return new ServerPacket.NpcAwakening();
            case (short)ServerPacketIds.NpcDisassemble:
                return new ServerPacket.NpcDisassemble();
            case (short)ServerPacketIds.NpcDowngrade:
                return new ServerPacket.NpcDowngrade();
            case (short)ServerPacketIds.NpcReset:
                return new ServerPacket.NpcReset();
            case (short)ServerPacketIds.AwakeningNeedMaterials:
                return new ServerPacket.AwakeningNeedMaterials();
            case (short)ServerPacketIds.AwakeningLockedItem:
                return new ServerPacket.AwakeningLockedItem();
            case (short)ServerPacketIds.Awakening:
                return new ServerPacket.Awakening();
            case (short)ServerPacketIds.ReceiveMail:
                return new ServerPacket.ReceiveMail();
            case (short)ServerPacketIds.MailLockedItem:
                return new ServerPacket.MailLockedItem();
            case (short)ServerPacketIds.MailSent:
                return new ServerPacket.MailSent();
            case (short)ServerPacketIds.MailSendRequest:
                return new ServerPacket.MailSendRequest();
            case (short)ServerPacketIds.ParcelCollected:
                return new ServerPacket.ParcelCollected();
            case (short)ServerPacketIds.MailCost:
                return new ServerPacket.MailCost();
            case (short)ServerPacketIds.ResizeInventory:
                return new ServerPacket.ResizeInventory();
            case (short)ServerPacketIds.ResizeStorage:
                return new ServerPacket.ResizeStorage();
            case (short)ServerPacketIds.NewIntelligentCreature:
                return new ServerPacket.NewIntelligentCreature();
            case (short)ServerPacketIds.UpdateIntelligentCreatureList:
                return new ServerPacket.UpdateIntelligentCreatureList();
            case (short)ServerPacketIds.IntelligentCreatureEnableRename:
                return new ServerPacket.IntelligentCreatureEnableRename();
            case (short)ServerPacketIds.IntelligentCreaturePickup:
                return new ServerPacket.IntelligentCreaturePickup();
            case (short)ServerPacketIds.NpcPearlGoods:
                return new ServerPacket.NpcPearlGoods();
            case (short)ServerPacketIds.FriendUpdate:
                return new ServerPacket.FriendUpdate();
            case (short)ServerPacketIds.LoverUpdate:
                return new ServerPacket.LoverUpdate();
            case (short)ServerPacketIds.MentorUpdate:
                return new ServerPacket.MentorUpdate();
            case (short)ServerPacketIds.GuildBuffList:
                return new ServerPacket.GuildBuffList();
            case (short)ServerPacketIds.GameShopInfo:
                return new ServerPacket.GameShopInfo();
            case (short)ServerPacketIds.GameShopStock:
                return new ServerPacket.GameShopStock();
            case (short)ServerPacketIds.NpcRequestInput:
                return new ServerPacket.NpcRequestInput();
            case (short)ServerPacketIds.Rankings:
                return new ServerPacket.Rankings();
            case (short)ServerPacketIds.Opendoor:
                return new ServerPacket.Opendoor();
            case (short)ServerPacketIds.GetRentedItems:
                return new ServerPacket.GetRentedItems();
            case (short)ServerPacketIds.ItemRentalRequest:
                return new ServerPacket.ItemRentalRequest();
            case (short)ServerPacketIds.ItemRentalFee:
                return new ServerPacket.ItemRentalFee();
            case (short)ServerPacketIds.ItemRentalPeriod:
                return new ServerPacket.ItemRentalPeriod();
            case (short)ServerPacketIds.DepositRentalItem:
                return new ServerPacket.DepositRentalItem();
            case (short)ServerPacketIds.RetrieveRentalItem:
                return new ServerPacket.RetrieveRentalItem();
            case (short)ServerPacketIds.UpdateRentalItem:
                return new ServerPacket.UpdateRentalItem();
            case (short)ServerPacketIds.CancelItemRental:
                return new ServerPacket.CancelItemRental();
            case (short)ServerPacketIds.ItemRentalLock:
                return new ServerPacket.ItemRentalLock();
            case (short)ServerPacketIds.ItemRentalPartnerLock:
                return new ServerPacket.ItemRentalPartnerLock();
            case (short)ServerPacketIds.CanConfirmItemRental:
                return new ServerPacket.CanConfirmItemRental();
            case (short)ServerPacketIds.ConfirmItemRental:
                return new ServerPacket.ConfirmItemRental();
            case (short)ServerPacketIds.NewRecipeInfo:
                return new ServerPacket.NewRecipeInfo();
            case (short)ServerPacketIds.OpenBrowser:
                return new ServerPacket.OpenBrowser();
            case (short)ServerPacketIds.PlaySound:
                return new ServerPacket.PlaySound();
            case (short)ServerPacketIds.SetTimer:
                return new ServerPacket.SetTimer();
            case (short)ServerPacketIds.ExpireTimer:
                return new ServerPacket.ExpireTimer();
            case (short)ServerPacketIds.UpdateNotice:
                return new ServerPacket.UpdateNotice();
            case (short)ServerPacketIds.Roll:
                return new ServerPacket.Roll();
            case (short)ServerPacketIds.SetCompass:
                return new ServerPacket.SetCompass();
            default:
                return null;
        }
    }
}