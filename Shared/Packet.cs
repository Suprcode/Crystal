using C = ClientPackets;
using S = ServerPackets;

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
            case (short)ClientPacketIds.TakeBackItem:
                return new C.TakeBackItem();
            case (short)ClientPacketIds.MergeItem:
                return new C.MergeItem();
            case (short)ClientPacketIds.EquipItem:
                return new C.EquipItem();
            case (short)ClientPacketIds.RemoveItem:
                return new C.RemoveItem();
            case (short)ClientPacketIds.RemoveSlotItem:
                return new C.RemoveSlotItem();
            case (short)ClientPacketIds.SplitItem:
                return new C.SplitItem();
            case (short)ClientPacketIds.UseItem:
                return new C.UseItem();
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
            case (short)ClientPacketIds.TakeBackHeroItem:
                return new C.TakeBackHeroItem();
            case (short)ClientPacketIds.TransferHeroItem:
                return new C.TransferHeroItem();
            case (short)ClientPacketIds.DropGold:
                return new C.DropGold();
            case (short)ClientPacketIds.PickUp:
                return new C.PickUp();
            case (short)ClientPacketIds.RequestMapInfo:
                return new C.RequestMapInfo();
            case (short)ClientPacketIds.TeleportToNpc:
                return new C.TeleportToNpc();
            case (short)ClientPacketIds.SearchMap:
                return new C.SearchMap();
            case (short)ClientPacketIds.Inspect:
                return new C.Inspect();
            case (short)ClientPacketIds.Observe:
                return new C.Observe();
            case (short)ClientPacketIds.ChangeAMode:
                return new C.ChangeAMode();
            case (short)ClientPacketIds.ChangePMode:
                return new C.ChangePMode();
            case (short)ClientPacketIds.ChangeTrade:
                return new C.ChangeTrade();
            case (short)ClientPacketIds.Attack:
                return new C.Attack();
            case (short)ClientPacketIds.RangeAttack:
                return new C.RangeAttack();
            case (short)ClientPacketIds.Harvest:
                return new C.Harvest();
            case (short)ClientPacketIds.CallNpc:
                return new C.CallNpc();
            case (short)ClientPacketIds.BuyItem:
                return new C.BuyItem();
            case (short)ClientPacketIds.SellItem:
                return new C.SellItem();
            case (short)ClientPacketIds.CraftItem:
                return new C.CraftItem();
            case (short)ClientPacketIds.RepairItem:
                return new C.RepairItem();
            case (short)ClientPacketIds.BuyItemBack:
                return new C.BuyItemBack();
            case (short)ClientPacketIds.SRepairItem:
                return new C.SRepairItem();
            case (short)ClientPacketIds.MagicKey:
                return new C.MagicKey();
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
            case (short)ClientPacketIds.NewHero:
                return new C.NewHero();
            case (short)ClientPacketIds.SetAutoPotValue:
                return new C.SetAutoPotValue();
            case (short)ClientPacketIds.SetAutoPotItem:
                return new C.SetAutoPotItem();
            case (short)ClientPacketIds.SetHeroBehaviour:
                return new C.SetHeroBehaviour();
            case (short)ClientPacketIds.ChangeHero:
                return new C.ChangeHero();
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
            case (short)ClientPacketIds.MarketBuy:
                return new C.MarketBuy();
            case (short)ClientPacketIds.MarketGetBack:
                return new C.MarketGetBack();
            case (short)ClientPacketIds.MarketSellNow:
                return new C.MarketSellNow();
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
            case (short)ClientPacketIds.RequestIntelligentCreatureUpdates:
                return new C.RequestIntelligentCreatureUpdates();
            case (short)ClientPacketIds.UpdateIntelligentCreature:
                return new C.UpdateIntelligentCreature();
            case (short)ClientPacketIds.IntelligentCreaturePickup:
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
            case (short)ClientPacketIds.NpcConfirmInput:
                return new C.NpcConfirmInput();
            case (short)ClientPacketIds.ReportIssue:
                return new C.ReportIssue();
            case (short)ClientPacketIds.GetRanking:
                return new C.GetRanking();
            case (short)ClientPacketIds.Opendoor:
                return new C.Opendoor();
            case (short)ClientPacketIds.GetRentedItems:
                return new C.GetRentedItems();
            case (short)ClientPacketIds.ItemRentalRequest:
                return new C.ItemRentalRequest();
            case (short)ClientPacketIds.ItemRentalFee:
                return new C.ItemRentalFee();
            case (short)ClientPacketIds.ItemRentalPeriod:
                return new C.ItemRentalPeriod();
            case (short)ClientPacketIds.DepositRentalItem:
                return new C.DepositRentalItem();
            case (short)ClientPacketIds.RetrieveRentalItem:
                return new C.RetrieveRentalItem();
            case (short)ClientPacketIds.CancelItemRental:
                return new C.CancelItemRental();
            case (short)ClientPacketIds.ItemRentalLockFee:
                return new C.ItemRentalLockFee();
            case (short)ClientPacketIds.ItemRentalLockItem:
                return new C.ItemRentalLockItem();
            case (short)ClientPacketIds.ConfirmItemRental:
                return new C.ConfirmItemRental();
            default:
                return null;
        }

    }
    public static Packet GetServerPacket(short index)
    {
        switch (index)
        {
            case (short)ServerPacketIds.Connected:
                return new S.ServerPacket.Connected();
            case (short)ServerPacketIds.ClientVersion:
                return new S.ServerPacket.ClientVersion();
            case (short)ServerPacketIds.Disconnect:
                return new S.ServerPacket.Disconnect();
            case (short)ServerPacketIds.KeepAlive:
                return new S.ServerPacket.KeepAlive();
            case (short)ServerPacketIds.NewAccount:
                return new S.ServerPacket.NewAccount();
            case (short)ServerPacketIds.ChangePassword:
                return new S.ServerPacket.ChangePassword();
            case (short)ServerPacketIds.ChangePasswordBanned:
                return new S.ServerPacket.ChangePasswordBanned();
            case (short)ServerPacketIds.Login:
                return new S.ServerPacket.Login();
            case (short)ServerPacketIds.LoginBanned:
                return new S.ServerPacket.LoginBanned();
            case (short)ServerPacketIds.LoginSuccess:
                return new S.ServerPacket.LoginSuccess();
            case (short)ServerPacketIds.NewCharacter:
                return new S.ServerPacket.NewCharacter();
            case (short)ServerPacketIds.NewCharacterSuccess:
                return new S.ServerPacket.NewCharacterSuccess();
            case (short)ServerPacketIds.DeleteCharacter:
                return new S.ServerPacket.DeleteCharacter();
            case (short)ServerPacketIds.DeleteCharacterSuccess:
                return new S.ServerPacket.DeleteCharacterSuccess();
            case (short)ServerPacketIds.StartGame:
                return new S.ServerPacket.StartGame();
            case (short)ServerPacketIds.StartGameBanned:
                return new S.ServerPacket.StartGameBanned();
            case (short)ServerPacketIds.StartGameDelay:
                return new S.ServerPacket.StartGameDelay();
            case (short)ServerPacketIds.MapInformation:
                return new S.ServerPacket.MapInformation();
            case (short)ServerPacketIds.NewMapInfo:
                return new S.ServerPacket.NewMapInfo();
            case (short)ServerPacketIds.WorldMapSetup:
                return new S.ServerPacket.WorldMapSetupInfo();
            case (short)ServerPacketIds.SearchMapResult:
                return new S.ServerPacket.SearchMapResult();
            case (short)ServerPacketIds.UserInformation:
                return new S.ServerPacket.UserInformation();
            case (short)ServerPacketIds.UserSlotsRefresh:
                return new S.ServerPacket.UserSlotsRefresh();
            case (short)ServerPacketIds.UserLocation:
                return new S.ServerPacket.UserLocation();
            case (short)ServerPacketIds.ObjectPlayer:
                return new S.ServerPacket.ObjectPlayer();
            case (short)ServerPacketIds.ObjectHero:
                return new S.ServerPacket.ObjectHero();
            case (short)ServerPacketIds.ObjectRemove:
                return new S.ServerPacket.ObjectRemove();
            case (short)ServerPacketIds.ObjectTurn:
                return new S.ServerPacket.ObjectTurn();
            case (short)ServerPacketIds.ObjectWalk:
                return new S.ServerPacket.ObjectWalk();
            case (short)ServerPacketIds.ObjectRun:
                return new S.ServerPacket.ObjectRun();
            case (short)ServerPacketIds.Chat:
                return new S.ServerPacket.Chat();
            case (short)ServerPacketIds.ObjectChat:
                return new S.ServerPacket.ObjectChat();
            case (short)ServerPacketIds.NewItemInfo:
                return new S.ServerPacket.NewItemInfo();
            case (short)ServerPacketIds.NewHeroInfo:
                return new S.ServerPacket.NewHeroInfo();
            case (short)ServerPacketIds.NewChatItem:
                return new S.ServerPacket.NewChatItem();
            case (short)ServerPacketIds.MoveItem:
                return new S.ServerPacket.MoveItem();
            case (short)ServerPacketIds.EquipItem:
                return new S.ServerPacket.EquipItem();
            case (short)ServerPacketIds.MergeItem:
                return new S.ServerPacket.MergeItem();
            case (short)ServerPacketIds.RemoveItem:
                return new S.ServerPacket.RemoveItem();
            case (short)ServerPacketIds.RemoveSlotItem:
                return new S.ServerPacket.RemoveSlotItem();
            case (short)ServerPacketIds.TakeBackItem:
                return new S.ServerPacket.TakeBackItem();
            case (short)ServerPacketIds.TakeBackHeroItem:
                return new S.ServerPacket.TakeBackHeroItem();
            case (short)ServerPacketIds.TransferHeroItem:
                return new S.ServerPacket.TransferHeroItem();
            case (short)ServerPacketIds.StoreItem:
                return new S.ServerPacket.StoreItem();
            case (short)ServerPacketIds.DepositRefineItem:
                return new S.ServerPacket.DepositRefineItem();
            case (short)ServerPacketIds.RetrieveRefineItem:
                return new S.ServerPacket.RetrieveRefineItem();
            case (short)ServerPacketIds.RefineItem:
                return new S.ServerPacket.RefineItem();
            case (short)ServerPacketIds.DepositTradeItem:
                return new S.ServerPacket.DepositTradeItem();
            case (short)ServerPacketIds.RetrieveTradeItem:
                return new S.ServerPacket.RetrieveTradeItem();
            case (short)ServerPacketIds.SplitItem:
                return new S.ServerPacket.SplitItem();
            case (short)ServerPacketIds.SplitItem1:
                return new S.ServerPacket.SplitItem1();
            case (short)ServerPacketIds.UseItem:
                return new S.ServerPacket.UseItem();
            case (short)ServerPacketIds.DropItem:
                return new S.ServerPacket.DropItem();
            case (short)ServerPacketIds.PlayerUpdate:
                return new S.ServerPacket.PlayerUpdate();
            case (short)ServerPacketIds.PlayerInspect:
                return new S.ServerPacket.PlayerInspect();
            case (short)ServerPacketIds.LogOutSuccess:
                return new S.ServerPacket.LogOutSuccess();
            case (short)ServerPacketIds.LogOutFailed:
                return new S.ServerPacket.LogOutFailed();
            case (short)ServerPacketIds.ReturnToLogin:
                return new S.ServerPacket.ReturnToLogin();
            case (short)ServerPacketIds.TimeOfDay:
                return new S.ServerPacket.TimeOfDay();
            case (short)ServerPacketIds.ChangeAMode:
                return new S.ServerPacket.ChangeAMode();
            case (short)ServerPacketIds.ChangePMode:
                return new S.ServerPacket.ChangePMode();
            case (short)ServerPacketIds.ObjectItem:
                return new S.ServerPacket.ObjectItem();
            case (short)ServerPacketIds.ObjectGold:
                return new S.ServerPacket.ObjectGold();
            case (short)ServerPacketIds.GainedItem:
                return new S.ServerPacket.GainedItem();
            case (short)ServerPacketIds.GainedGold:
                return new S.ServerPacket.GainedGold();
            case (short)ServerPacketIds.LoseGold:
                return new S.ServerPacket.LoseGold();
            case (short)ServerPacketIds.GainedCredit:
                return new S.ServerPacket.GainedCredit();
            case (short)ServerPacketIds.LoseCredit:
                return new S.ServerPacket.LoseCredit();
            case (short)ServerPacketIds.ObjectMonster:
                return new S.ServerPacket.ObjectMonster();
            case (short)ServerPacketIds.ObjectAttack:
                return new S.ServerPacket.ObjectAttack();
            case (short)ServerPacketIds.Struck:
                return new S.ServerPacket.Struck();
            case (short)ServerPacketIds.DamageIndicator:
                return new S.ServerPacket.DamageIndicator();
            case (short)ServerPacketIds.ObjectStruck:
                return new S.ServerPacket.ObjectStruck();
            case (short)ServerPacketIds.DuraChanged:
                return new S.ServerPacket.DuraChanged();
            case (short)ServerPacketIds.HealthChanged:
                return new S.ServerPacket.HealthChanged();
            case (short)ServerPacketIds.HeroHealthChanged:
                return new S.ServerPacket.HeroHealthChanged();
            case (short)ServerPacketIds.DeleteItem:
                return new S.ServerPacket.DeleteItem();
            case (short)ServerPacketIds.Death:
                return new S.ServerPacket.Death();
            case (short)ServerPacketIds.ObjectDied:
                return new S.ServerPacket.ObjectDied();
            case (short)ServerPacketIds.ColourChanged:
                return new S.ServerPacket.ColourChanged();
            case (short)ServerPacketIds.ObjectColourChanged:
                return new S.ServerPacket.ObjectColourChanged();
            case (short)ServerPacketIds.ObjectGuildNameChanged:
                return new S.ServerPacket.ObjectGuildNameChanged();
            case (short)ServerPacketIds.GainExperience:
                return new S.ServerPacket.GainExperience();
            case (short)ServerPacketIds.GainHeroExperience:
                return new S.ServerPacket.GainHeroExperience();
            case (short)ServerPacketIds.LevelChanged:
                return new S.ServerPacket.LevelChanged();
            case (short)ServerPacketIds.HeroLevelChanged:
                return new S.ServerPacket.HeroLevelChanged();
            case (short)ServerPacketIds.ObjectLeveled:
                return new S.ServerPacket.ObjectLeveled();
            case (short)ServerPacketIds.ObjectHarvest:
                return new S.ServerPacket.ObjectHarvest();
            case (short)ServerPacketIds.ObjectHarvested:
                return new S.ServerPacket.ObjectHarvested();
            case (short)ServerPacketIds.ObjectNpc:
                return new S.ServerPacket.ObjectNpc();
            case (short)ServerPacketIds.NpcResponse:
                return new S.ServerPacket.NpcResponse();
            case (short)ServerPacketIds.ObjectHide:
                return new S.ServerPacket.ObjectHide();
            case (short)ServerPacketIds.ObjectShow:
                return new S.ServerPacket.ObjectShow();
            case (short)ServerPacketIds.Poisoned:
                return new S.ServerPacket.Poisoned();
            case (short)ServerPacketIds.ObjectPoisoned:
                return new S.ServerPacket.ObjectPoisoned();
            case (short)ServerPacketIds.MapChanged:
                return new S.ServerPacket.MapChanged();
            case (short)ServerPacketIds.ObjectTeleportOut:
                return new S.ServerPacket.ObjectTeleportOut();
            case (short)ServerPacketIds.ObjectTeleportIn:
                return new S.ServerPacket.ObjectTeleportIn();
            case (short)ServerPacketIds.TeleportIn:
                return new S.ServerPacket.TeleportIn();
            case (short)ServerPacketIds.NpcGoods:
                return new S.ServerPacket.NpcGoods();
            case (short)ServerPacketIds.NpcSell:
                return new S.ServerPacket.NpcSell();
            case (short)ServerPacketIds.NpcRepair:
                return new S.ServerPacket.NpcRepair();
            case (short)ServerPacketIds.NpcSRepair:
                return new S.ServerPacket.NpcSRepair();
            case (short)ServerPacketIds.NpcRefine:
                return new S.ServerPacket.NpcRefine();
            case (short)ServerPacketIds.NpcCheckRefine:
                return new S.ServerPacket.NpcCheckRefine();
            case (short)ServerPacketIds.NpcCollectRefine:
                return new S.ServerPacket.NpcCollectRefine();
            case (short)ServerPacketIds.NpcReplaceWedRing:
                return new S.ServerPacket.NpcReplaceWedRing();
            case (short)ServerPacketIds.NpcStorage:
                return new S.ServerPacket.NpcStorage();
            case (short)ServerPacketIds.SellItem:
                return new S.ServerPacket.SellItem();
            case (short)ServerPacketIds.CraftItem:
                return new S.ServerPacket.CraftItem();
            case (short)ServerPacketIds.RepairItem:
                return new S.ServerPacket.RepairItem();
            case (short)ServerPacketIds.ItemRepaired:
                return new S.ServerPacket.ItemRepaired();
            case (short)ServerPacketIds.ItemSlotSizeChanged:
                return new S.ServerPacket.ItemSlotSizeChanged();
            case (short)ServerPacketIds.ItemSealChanged:
                return new S.ServerPacket.ItemSealChanged();
            case (short)ServerPacketIds.NewMagic:
                return new S.ServerPacket.NewMagic();
            case (short)ServerPacketIds.MagicLeveled:
                return new S.ServerPacket.MagicLeveled();
            case (short)ServerPacketIds.Magic:
                return new S.ServerPacket.Magic();
            case (short)ServerPacketIds.MagicDelay:
                return new S.ServerPacket.MagicDelay();
            case (short)ServerPacketIds.MagicCast:
                return new S.ServerPacket.MagicCast();
            case (short)ServerPacketIds.ObjectMagic:
                return new S.ServerPacket.ObjectMagic();
            case (short)ServerPacketIds.ObjectProjectile:
                return new S.ServerPacket.ObjectProjectile();
            case (short)ServerPacketIds.ObjectEffect:
                return new S.ServerPacket.ObjectEffect();
            case (short)ServerPacketIds.RangeAttack:
                return new S.ServerPacket.RangeAttack();
            case (short)ServerPacketIds.Pushed:
                return new S.ServerPacket.Pushed();
            case (short)ServerPacketIds.ObjectPushed:
                return new S.ServerPacket.ObjectPushed();
            case (short)ServerPacketIds.ObjectName:
                return new S.ServerPacket.ObjectName();
            case (short)ServerPacketIds.UserStorage:
                return new S.ServerPacket.UserStorage();
            case (short)ServerPacketIds.SwitchGroup:
                return new S.ServerPacket.SwitchGroup();
            case (short)ServerPacketIds.DeleteGroup:
                return new S.ServerPacket.DeleteGroup();
            case (short)ServerPacketIds.DeleteMember:
                return new S.ServerPacket.DeleteMember();
            case (short)ServerPacketIds.GroupInvite:
                return new S.ServerPacket.GroupInvite();
            case (short)ServerPacketIds.AddMember:
                return new S.ServerPacket.AddMember();
            case (short)ServerPacketIds.GroupMembersMap:
                return new S.ServerPacket.GroupMembersMap();
            case (short)ServerPacketIds.SendMemberLocation:
                return new S.ServerPacket.SendMemberLocation();
            case (short)ServerPacketIds.Revived:
                return new S.ServerPacket.Revived();
            case (short)ServerPacketIds.ObjectRevived:
                return new S.ServerPacket.ObjectRevived();
            case (short)ServerPacketIds.SpellToggle:
                return new S.ServerPacket.SpellToggle();
            case (short)ServerPacketIds.ObjectHealth:
                return new S.ServerPacket.ObjectHealth();
            case (short)ServerPacketIds.ObjectMana:
                return new S.ServerPacket.ObjectMana();
            case (short)ServerPacketIds.MapEffect:
                return new S.ServerPacket.MapEffect();
            case (short)ServerPacketIds.AllowObserve:
                return new S.ServerPacket.AllowObserve();
            case (short)ServerPacketIds.ObjectRangeAttack:
                return new S.ServerPacket.ObjectRangeAttack();
            case (short)ServerPacketIds.AddBuff:
                return new S.ServerPacket.AddBuff();
            case (short)ServerPacketIds.RemoveBuff:
                return new S.ServerPacket.RemoveBuff();
            case (short)ServerPacketIds.PauseBuff:
                return new S.ServerPacket.PauseBuff();
            case (short)ServerPacketIds.ObjectHidden:
                return new S.ServerPacket.ObjectHidden();
            case (short)ServerPacketIds.RefreshItem:
                return new S.ServerPacket.RefreshItem();
            case (short)ServerPacketIds.ObjectSpell:
                return new S.ServerPacket.ObjectSpell();
            case (short)ServerPacketIds.UserDash:
                return new S.ServerPacket.UserDash();
            case (short)ServerPacketIds.ObjectDash:
                return new S.ServerPacket.ObjectDash();
            case (short)ServerPacketIds.UserDashFail:
                return new S.ServerPacket.UserDashFail();
            case (short)ServerPacketIds.ObjectDashFail:
                return new S.ServerPacket.ObjectDashFail();
            case (short)ServerPacketIds.NpcConsign:
                return new S.ServerPacket.NpcConsign();
            case (short)ServerPacketIds.NpcMarket:
                return new S.ServerPacket.NpcMarket();
            case (short)ServerPacketIds.NpcMarketPage:
                return new S.ServerPacket.NpcMarketPage();
            case (short)ServerPacketIds.ConsignItem:
                return new S.ServerPacket.ConsignItem();
            case (short)ServerPacketIds.MarketFail:
                return new S.ServerPacket.MarketFail();
            case (short)ServerPacketIds.MarketSuccess:
                return new S.ServerPacket.MarketSuccess();
            case (short)ServerPacketIds.ObjectSitDown:
                return new S.ServerPacket.ObjectSitDown();
            case (short)ServerPacketIds.InTrapRock:
                return new S.ServerPacket.InTrapRock();
            case (short)ServerPacketIds.RemoveMagic:
                return new S.ServerPacket.RemoveMagic();
            case (short)ServerPacketIds.BaseStatsInfo:
                return new S.ServerPacket.BaseStatsInfo();
            case (short)ServerPacketIds.HeroBaseStatsInfo:
                return new S.ServerPacket.HeroBaseStatsInfo();
            case (short)ServerPacketIds.UserName:
                return new S.ServerPacket.UserName();
            case (short)ServerPacketIds.ChatItemStats:
                return new S.ServerPacket.ChatItemStats();
            case (short)ServerPacketIds.GuildMemberChange:
                return new S.ServerPacket.GuildMemberChange();
            case (short)ServerPacketIds.GuildNoticeChange:
                return new S.ServerPacket.GuildNoticeChange();
            case (short)ServerPacketIds.GuildStatus:
                return new S.ServerPacket.GuildStatus();
            case (short)ServerPacketIds.GuildInvite:
                return new S.ServerPacket.GuildInvite();
            case (short)ServerPacketIds.GuildExpGain:
                return new S.ServerPacket.GuildExpGain();
            case (short)ServerPacketIds.GuildNameRequest:
                return new S.ServerPacket.GuildNameRequest();
            case (short)ServerPacketIds.GuildStorageGoldChange:
                return new S.ServerPacket.GuildStorageGoldChange();
            case (short)ServerPacketIds.GuildStorageItemChange:
                return new S.ServerPacket.GuildStorageItemChange();
            case (short)ServerPacketIds.GuildStorageList:
                return new S.ServerPacket.GuildStorageList();
            case (short)ServerPacketIds.GuildRequestWar:
                return new S.ServerPacket.GuildRequestWar();
            case (short)ServerPacketIds.HeroCreateRequest:
                return new S.ServerPacket.HeroCreateRequest();
            case (short)ServerPacketIds.NewHero:
                return new S.ServerPacket.NewHero();
            case (short)ServerPacketIds.HeroInformation:
                return new S.ServerPacket.HeroInformation();
            case (short)ServerPacketIds.UpdateHeroSpawnState:
                return new S.ServerPacket.UpdateHeroSpawnState();
            case (short)ServerPacketIds.UnlockHeroAutoPot:
                return new S.ServerPacket.UnlockHeroAutoPot();
            case (short)ServerPacketIds.SetAutoPotValue:
                return new S.ServerPacket.SetAutoPotValue();
            case (short)ServerPacketIds.SetAutoPotItem:
                return new S.ServerPacket.SetAutoPotItem();
            case (short)ServerPacketIds.SetHeroBehaviour:
                return new S.ServerPacket.SetHeroBehaviour();
            case (short)ServerPacketIds.ManageHeroes:
                return new S.ServerPacket.ManageHeroes();
            case (short)ServerPacketIds.ChangeHero:
                return new S.ServerPacket.ChangeHero();
            case (short)ServerPacketIds.DefaultNpc:
                return new S.ServerPacket.DefaultNpc();
            case (short)ServerPacketIds.NpcUpdate:
                return new S.ServerPacket.NpcUpdate();
            case (short)ServerPacketIds.NpcImageUpdate:
                return new S.ServerPacket.NpcImageUpdate();
            case (short)ServerPacketIds.MarriageRequest:
                return new S.ServerPacket.MarriageRequest();
            case (short)ServerPacketIds.DivorceRequest:
                return new S.ServerPacket.DivorceRequest();
            case (short)ServerPacketIds.MentorRequest:
                return new S.ServerPacket.MentorRequest();
            case (short)ServerPacketIds.TradeRequest:
                return new S.ServerPacket.TradeRequest();
            case (short)ServerPacketIds.TradeAccept:
                return new S.ServerPacket.TradeAccept();
            case (short)ServerPacketIds.TradeGold:
                return new S.ServerPacket.TradeGold();
            case (short)ServerPacketIds.TradeItem:
                return new S.ServerPacket.TradeItem();
            case (short)ServerPacketIds.TradeConfirm:
                return new S.ServerPacket.TradeConfirm();
            case (short)ServerPacketIds.TradeCancel:
                return new S.ServerPacket.TradeCancel();
            case (short)ServerPacketIds.MountUpdate:
                return new S.ServerPacket.MountUpdate();
            case (short)ServerPacketIds.TransformUpdate:
                return new S.ServerPacket.TransformUpdate();
            case (short)ServerPacketIds.EquipSlotItem:
                return new S.ServerPacket.EquipSlotItem();
            case (short)ServerPacketIds.FishingUpdate:
                return new S.ServerPacket.FishingUpdate();
            case (short)ServerPacketIds.ChangeQuest:
                return new S.ServerPacket.ChangeQuest();
            case (short)ServerPacketIds.CompleteQuest:
                return new S.ServerPacket.CompleteQuest();
            case (short)ServerPacketIds.ShareQuest:
                return new S.ServerPacket.ShareQuest();
            case (short)ServerPacketIds.NewQuestInfo:
                return new S.ServerPacket.NewQuestInfo();
            case (short)ServerPacketIds.GainedQuestItem:
                return new S.ServerPacket.GainedQuestItem();
            case (short)ServerPacketIds.DeleteQuestItem:
                return new S.ServerPacket.DeleteQuestItem();
            case (short)ServerPacketIds.CancelReincarnation:
                return new S.ServerPacket.CancelReincarnation();
            case (short)ServerPacketIds.RequestReincarnation:
                return new S.ServerPacket.RequestReincarnation();
            case (short)ServerPacketIds.UserBackStep:
                return new S.ServerPacket.UserBackStep();
            case (short)ServerPacketIds.ObjectBackStep:
                return new S.ServerPacket.ObjectBackStep();
            case (short)ServerPacketIds.UserDashAttack:
                return new S.ServerPacket.UserDashAttack();
            case (short)ServerPacketIds.ObjectDashAttack:
                return new S.ServerPacket.ObjectDashAttack();
            case (short)ServerPacketIds.UserAttackMove://Warrior Skill - SlashingBurst
                return new S.ServerPacket.UserAttackMove();
            case (short)ServerPacketIds.CombineItem:
                return new S.ServerPacket.CombineItem();
            case (short)ServerPacketIds.ItemUpgraded:
                return new S.ServerPacket.ItemUpgraded();
            case (short)ServerPacketIds.SetConcentration:
                return new S.ServerPacket.SetConcentration();
            case (short)ServerPacketIds.SetElemental:
                return new S.ServerPacket.SetElemental();
            case (short)ServerPacketIds.RemoveDelayedExplosion:
                return new S.ServerPacket.RemoveDelayedExplosion();
            case (short)ServerPacketIds.ObjectDeco:
                return new S.ServerPacket.ObjectDeco();
            case (short)ServerPacketIds.ObjectSneaking:
                return new S.ServerPacket.ObjectSneaking();
            case (short)ServerPacketIds.ObjectLevelEffects:
                return new S.ServerPacket.ObjectLevelEffects();
            case (short)ServerPacketIds.SetBindingShot:
                return new S.ServerPacket.SetBindingShot();
            case (short)ServerPacketIds.SendOutputMessage:
                return new S.ServerPacket.SendOutputMessage();
            case (short)ServerPacketIds.NpcAwakening:
                return new S.ServerPacket.NpcAwakening();
            case (short)ServerPacketIds.NpcDisassemble:
                return new S.ServerPacket.NpcDisassemble();
            case (short)ServerPacketIds.NpcDowngrade:
                return new S.ServerPacket.NpcDowngrade();
            case (short)ServerPacketIds.NpcReset:
                return new S.ServerPacket.NpcReset();
            case (short)ServerPacketIds.AwakeningNeedMaterials:
                return new S.ServerPacket.AwakeningNeedMaterials();
            case (short)ServerPacketIds.AwakeningLockedItem:
                return new S.ServerPacket.AwakeningLockedItem();
            case (short)ServerPacketIds.Awakening:
                return new S.ServerPacket.Awakening();
            case (short)ServerPacketIds.ReceiveMail:
                return new S.ServerPacket.ReceiveMail();
            case (short)ServerPacketIds.MailLockedItem:
                return new S.ServerPacket.MailLockedItem();
            case (short)ServerPacketIds.MailSent:
                return new S.ServerPacket.MailSent();
            case (short)ServerPacketIds.MailSendRequest:
                return new S.ServerPacket.MailSendRequest();
            case (short)ServerPacketIds.ParcelCollected:
                return new S.ServerPacket.ParcelCollected();
            case (short)ServerPacketIds.MailCost:
                return new S.ServerPacket.MailCost();
            case (short)ServerPacketIds.ResizeInventory:
                return new S.ServerPacket.ResizeInventory();
            case (short)ServerPacketIds.ResizeStorage:
                return new S.ServerPacket.ResizeStorage();
            case (short)ServerPacketIds.NewIntelligentCreature:
                return new S.ServerPacket.NewIntelligentCreature();
            case (short)ServerPacketIds.UpdateIntelligentCreatureList:
                return new S.ServerPacket.UpdateIntelligentCreatureList();
            case (short)ServerPacketIds.IntelligentCreatureEnableRename:
                return new S.ServerPacket.IntelligentCreatureEnableRename();
            case (short)ServerPacketIds.IntelligentCreaturePickup:
                return new S.ServerPacket.IntelligentCreaturePickup();
            case (short)ServerPacketIds.NpcPearlGoods:
                return new S.ServerPacket.NpcPearlGoods();
            case (short)ServerPacketIds.FriendUpdate:
                return new S.ServerPacket.FriendUpdate();
            case (short)ServerPacketIds.LoverUpdate:
                return new S.ServerPacket.LoverUpdate();
            case (short)ServerPacketIds.MentorUpdate:
                return new S.ServerPacket.MentorUpdate();
            case (short)ServerPacketIds.GuildBuffList:
                return new S.ServerPacket.GuildBuffList();
            case (short)ServerPacketIds.GameShopInfo:
                return new S.ServerPacket.GameShopInfo();
            case (short)ServerPacketIds.GameShopStock:
                return new S.ServerPacket.GameShopStock();
            case (short)ServerPacketIds.NpcRequestInput:
                return new S.ServerPacket.NpcRequestInput();
            case (short)ServerPacketIds.Rankings:
                return new S.ServerPacket.Rankings();
            case (short)ServerPacketIds.Opendoor:
                return new S.ServerPacket.Opendoor();
            case (short)ServerPacketIds.GetRentedItems:
                return new S.ServerPacket.GetRentedItems();
            case (short)ServerPacketIds.ItemRentalRequest:
                return new S.ServerPacket.ItemRentalRequest();
            case (short)ServerPacketIds.ItemRentalFee:
                return new S.ServerPacket.ItemRentalFee();
            case (short)ServerPacketIds.ItemRentalPeriod:
                return new S.ServerPacket.ItemRentalPeriod();
            case (short)ServerPacketIds.DepositRentalItem:
                return new S.ServerPacket.DepositRentalItem();
            case (short)ServerPacketIds.RetrieveRentalItem:
                return new S.ServerPacket.RetrieveRentalItem();
            case (short)ServerPacketIds.UpdateRentalItem:
                return new S.ServerPacket.UpdateRentalItem();
            case (short)ServerPacketIds.CancelItemRental:
                return new S.ServerPacket.CancelItemRental();
            case (short)ServerPacketIds.ItemRentalLock:
                return new S.ServerPacket.ItemRentalLock();
            case (short)ServerPacketIds.ItemRentalPartnerLock:
                return new S.ServerPacket.ItemRentalPartnerLock();
            case (short)ServerPacketIds.CanConfirmItemRental:
                return new S.ServerPacket.CanConfirmItemRental();
            case (short)ServerPacketIds.ConfirmItemRental:
                return new S.ServerPacket.ConfirmItemRental();
            case (short)ServerPacketIds.NewRecipeInfo:
                return new S.ServerPacket.NewRecipeInfo();
            case (short)ServerPacketIds.OpenBrowser:
                return new S.ServerPacket.OpenBrowser();
            case (short)ServerPacketIds.PlaySound:
                return new S.ServerPacket.PlaySound();
            case (short)ServerPacketIds.SetTimer:
                return new S.ServerPacket.SetTimer();
            case (short)ServerPacketIds.ExpireTimer:
                return new S.ServerPacket.ExpireTimer();
            case (short)ServerPacketIds.UpdateNotice:
                return new S.ServerPacket.UpdateNotice();
            case (short)ServerPacketIds.Roll:
                return new S.ServerPacket.Roll();
            case (short)ServerPacketIds.SetCompass:
                return new S.ServerPacket.SetCompass();
            default:
                return null;
        }
    }
}