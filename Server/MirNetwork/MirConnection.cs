using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Server.Library.MirDatabase;
using Server.Library.MirEnvir;
using Server.Library.MirObjects;
using Server.Library.Utils;
using Shared;
using Shared.Data;
using Shared.Functions;

namespace Server.Library.MirNetwork {
    public enum GameStage {
        None,
        Login,
        Select,
        Game,
        Observer,
        Disconnected
    }

    public class MirConnection {
        protected static Envir Envir => Envir.Main;

        protected static MessageQueue MessageQueue => MessageQueue.Instance;

        public readonly int SessionID;
        public readonly string IPAddress;

        public GameStage Stage;

        private TcpClient _client;
        private ConcurrentQueue<Packet> _receiveList;
        private ConcurrentQueue<Packet> _sendList;
        private Queue<Packet> _retryList;

        private bool _disconnecting;
        public bool Connected;

        public bool Disconnecting {
            get => _disconnecting;
            set {
                if(_disconnecting == value) {
                    return;
                }

                _disconnecting = value;
                TimeOutTime = Envir.Time + 500;
            }
        }

        public readonly long TimeConnected;
        public long TimeDisconnected, TimeOutTime;

        private byte[] _rawData = new byte[0];
        private byte[] _rawBytes = new byte[8 * 1024];

        public AccountInfo Account;
        public PlayerObject Player;

        public List<MirConnection> Observers = new();
        public MirConnection Observing;

        public List<ItemInfo> SentItemInfo = new();
        public List<QuestInfo> SentQuestInfo = new();
        public List<RecipeInfo> SentRecipeInfo = new();
        public List<UserItem> SentChatItem = new(); //TODO - Add Expiry time
        public List<MapInfo> SentMapInfo = new();
        public List<ulong> SentHeroInfo = new();
        public bool WorldMapSetupSent;
        public bool StorageSent;
        public bool HeroStorageSent;
        public Dictionary<long, DateTime> SentRankings = new();

        private DateTime _dataCounterReset;
        private int _dataCounter;
        private FixedSizedQueue<Packet> _lastPackets;

        public MirConnection(int sessionID, TcpClient client) {
            SessionID = sessionID;
            IPAddress = client.Client.RemoteEndPoint.ToString().Split(':')[0];

            Envir.UpdateIPBlock(IPAddress, TimeSpan.FromSeconds(Settings.IPBlockSeconds));

            MessageQueue.Enqueue(IPAddress + ", Connected.");

            _client = client;
            _client.NoDelay = true;

            TimeConnected = Envir.Time;
            TimeOutTime = TimeConnected + Settings.TimeOut;

            _lastPackets = new FixedSizedQueue<Packet>(10);

            _receiveList = new ConcurrentQueue<Packet>();
            _sendList = new ConcurrentQueue<Packet>();
            _sendList.Enqueue(new ServerPacket.Connected());
            _retryList = new Queue<Packet>();

            Connected = true;
            BeginReceive();
        }

        public void AddObserver(MirConnection c) {
            Observers.Add(c);

            if(c.Observing != null) {
                c.Observing.Observers.Remove(c);
            }

            c.Observing = this;

            c.Stage = GameStage.Observer;
        }

        private void BeginReceive() {
            if(!Connected) {
                return;
            }

            try {
                _client.Client.BeginReceive(_rawBytes, 0, _rawBytes.Length, SocketFlags.None, ReceiveData, _rawBytes);
            } catch {
                Disconnecting = true;
            }
        }

        private void ReceiveData(IAsyncResult result) {
            if(!Connected) {
                return;
            }

            int dataRead;

            try {
                dataRead = _client.Client.EndReceive(result);
            } catch {
                Disconnecting = true;
                return;
            }

            if(dataRead == 0) {
                Disconnecting = true;
                return;
            }

            if(_dataCounterReset < Envir.Now) {
                _dataCounterReset = Envir.Now.AddSeconds(5);
                _dataCounter = 0;
            }

            _dataCounter++;

            try {
                byte[] rawBytes = result.AsyncState as byte[];

                byte[] temp = _rawData;
                _rawData = new byte[dataRead + temp.Length];
                Buffer.BlockCopy(temp, 0, _rawData, 0, temp.Length);
                Buffer.BlockCopy(rawBytes, 0, _rawData, temp.Length, dataRead);

                Packet p;

                while ((p = Packet.ReceivePacket(_rawData, out _rawData)) != null) {
                    _receiveList.Enqueue(p);
                }
            } catch {
                Envir.UpdateIPBlock(IPAddress, TimeSpan.FromHours(24));

                MessageQueue.Enqueue($"{IPAddress} Disconnected, Invalid packet.");

                Disconnecting = true;
                return;
            }

            if(_dataCounter > Settings.MaxPacket) {
                Envir.UpdateIPBlock(IPAddress, TimeSpan.FromHours(24));

                List<string> packetList = new();

                while (_lastPackets.Count > 0) {
                    _lastPackets.TryDequeue(out Packet pkt);

                    Enum.TryParse<ClientPacketIds>((pkt?.Index ?? 0).ToString(), out ClientPacketIds cPacket);

                    packetList.Add(cPacket.ToString());
                }

                MessageQueue.Enqueue(
                    $"{IPAddress} Disconnected, Large amount of Packets. LastPackets: {String.Join(",", packetList.Distinct())}.");

                Disconnecting = true;
                return;
            }

            BeginReceive();
        }

        private void BeginSend(List<byte> data) {
            if(!Connected || data.Count == 0) {
                return;
            }

            //Interlocked.Add(ref Network.Sent, data.Count);

            try {
                _client.Client.BeginSend(data.ToArray(), 0, data.Count, SocketFlags.None, SendData, Disconnecting);
            } catch {
                Disconnecting = true;
            }
        }

        private void SendData(IAsyncResult result) {
            try {
                _client.Client.EndSend(result);
            } catch { }
        }

        public void Enqueue(Packet p) {
            if(p == null) {
                return;
            }

            if(_sendList != null && p != null) {
                _sendList.Enqueue(p);
            }

            if(!p.Observable) {
                return;
            }

            foreach(MirConnection c in Observers) {
                c.Enqueue(p);
            }
        }

        public void Process() {
            if(_client == null || !_client.Connected) {
                Disconnect(20);
                return;
            }

            while (!_receiveList.IsEmpty && !Disconnecting) {
                Packet p;
                if(!_receiveList.TryDequeue(out p)) {
                    continue;
                }

                _lastPackets.Enqueue(p);

                TimeOutTime = Envir.Time + Settings.TimeOut;
                ProcessPacket(p);

                if(_receiveList == null) {
                    return;
                }
            }

            while (_retryList.Count > 0) {
                _receiveList.Enqueue(_retryList.Dequeue());
            }

            if(Envir.Time > TimeOutTime) {
                Disconnect(21);
                return;
            }

            if(_sendList == null || _sendList.Count <= 0) {
                return;
            }

            List<byte> data = new();

            while (!_sendList.IsEmpty) {
                Packet p;
                if(!_sendList.TryDequeue(out p) || p == null) {
                    continue;
                }

                data.AddRange(p.GetPacketBytes());
            }

            BeginSend(data);
        }

        private void ProcessPacket(Packet p) {
            if(p == null || Disconnecting) {
                return;
            }

            switch (p.Index) {
                case (short)ClientPacketIds.ClientVersion:
                    ClientVersion((ClientPacket.ClientVersion)p);
                    break;
                case (short)ClientPacketIds.Disconnect:
                    Disconnect(22);
                    break;
                case (short)ClientPacketIds.KeepAlive: // Keep Alive
                    ClientKeepAlive((ClientPacket.KeepAlive)p);
                    break;
                case (short)ClientPacketIds.NewAccount:
                    NewAccount((ClientPacket.NewAccount)p);
                    break;
                case (short)ClientPacketIds.ChangePassword:
                    ChangePassword((ClientPacket.ChangePassword)p);
                    break;
                case (short)ClientPacketIds.Login:
                    Login((ClientPacket.Login)p);
                    break;
                case (short)ClientPacketIds.NewCharacter:
                    NewCharacter((ClientPacket.NewCharacter)p);
                    break;
                case (short)ClientPacketIds.DeleteCharacter:
                    DeleteCharacter((ClientPacket.DeleteCharacter)p);
                    break;
                case (short)ClientPacketIds.StartGame:
                    StartGame((ClientPacket.StartGame)p);
                    break;
                case (short)ClientPacketIds.LogOut:
                    LogOut();
                    break;
                case (short)ClientPacketIds.Turn:
                    Turn((ClientPacket.Turn)p);
                    break;
                case (short)ClientPacketIds.Walk:
                    Walk((ClientPacket.Walk)p);
                    break;
                case (short)ClientPacketIds.Run:
                    Run((ClientPacket.Run)p);
                    break;
                case (short)ClientPacketIds.Chat:
                    Chat((ClientPacket.Chat)p);
                    break;
                case (short)ClientPacketIds.MoveItem:
                    MoveItem((ClientPacket.MoveItem)p);
                    break;
                case (short)ClientPacketIds.StoreItem:
                    StoreItem((ClientPacket.StoreItem)p);
                    break;
                case (short)ClientPacketIds.DepositRefineItem:
                    DepositRefineItem((ClientPacket.DepositRefineItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveRefineItem:
                    RetrieveRefineItem((ClientPacket.RetrieveRefineItem)p);
                    break;
                case (short)ClientPacketIds.RefineCancel:
                    RefineCancel((ClientPacket.RefineCancel)p);
                    break;
                case (short)ClientPacketIds.RefineItem:
                    RefineItem((ClientPacket.RefineItem)p);
                    break;
                case (short)ClientPacketIds.CheckRefine:
                    CheckRefine((ClientPacket.CheckRefine)p);
                    break;
                case (short)ClientPacketIds.ReplaceWedRing:
                    ReplaceWedRing((ClientPacket.ReplaceWedRing)p);
                    break;
                case (short)ClientPacketIds.DepositTradeItem:
                    DepositTradeItem((ClientPacket.DepositTradeItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveTradeItem:
                    RetrieveTradeItem((ClientPacket.RetrieveTradeItem)p);
                    break;
                case (short)ClientPacketIds.TakeBackItem:
                    TakeBackItem((ClientPacket.TakeBackItem)p);
                    break;
                case (short)ClientPacketIds.MergeItem:
                    MergeItem((ClientPacket.MergeItem)p);
                    break;
                case (short)ClientPacketIds.EquipItem:
                    EquipItem((ClientPacket.EquipItem)p);
                    break;
                case (short)ClientPacketIds.RemoveItem:
                    RemoveItem((ClientPacket.RemoveItem)p);
                    break;
                case (short)ClientPacketIds.RemoveSlotItem:
                    RemoveSlotItem((ClientPacket.RemoveSlotItem)p);
                    break;
                case (short)ClientPacketIds.SplitItem:
                    SplitItem((ClientPacket.SplitItem)p);
                    break;
                case (short)ClientPacketIds.UseItem:
                    UseItem((ClientPacket.UseItem)p);
                    break;
                case (short)ClientPacketIds.DropItem:
                    DropItem((ClientPacket.DropItem)p);
                    break;
                case (short)ClientPacketIds.TakeBackHeroItem:
                    TakeBackHeroItem((ClientPacket.TakeBackHeroItem)p);
                    break;
                case (short)ClientPacketIds.TransferHeroItem:
                    TransferHeroItem((ClientPacket.TransferHeroItem)p);
                    break;
                case (short)ClientPacketIds.DropGold:
                    DropGold((ClientPacket.DropGold)p);
                    break;
                case (short)ClientPacketIds.PickUp:
                    PickUp();
                    break;
                case (short)ClientPacketIds.RequestMapInfo:
                    RequestMapInfo((ClientPacket.RequestMapInfo)p);
                    break;
                case (short)ClientPacketIds.TeleportToNpc:
                    TeleportToNpc((ClientPacket.TeleportToNpc)p);
                    break;
                case (short)ClientPacketIds.SearchMap:
                    SearchMap((ClientPacket.SearchMap)p);
                    break;
                case (short)ClientPacketIds.Inspect:
                    Inspect((ClientPacket.Inspect)p);
                    break;
                case (short)ClientPacketIds.Observe:
                    Observe((ClientPacket.Observe)p);
                    break;
                case (short)ClientPacketIds.ChangeAMode:
                    ChangeAMode((ClientPacket.ChangeAMode)p);
                    break;
                case (short)ClientPacketIds.ChangePMode:
                    ChangePMode((ClientPacket.ChangePMode)p);
                    break;
                case (short)ClientPacketIds.ChangeTrade:
                    ChangeTrade((ClientPacket.ChangeTrade)p);
                    break;
                case (short)ClientPacketIds.Attack:
                    Attack((ClientPacket.Attack)p);
                    break;
                case (short)ClientPacketIds.RangeAttack:
                    RangeAttack((ClientPacket.RangeAttack)p);
                    break;
                case (short)ClientPacketIds.Harvest:
                    Harvest((ClientPacket.Harvest)p);
                    break;
                case (short)ClientPacketIds.CallNpc:
                    CallNpc((ClientPacket.CallNpc)p);
                    break;
                case (short)ClientPacketIds.BuyItem:
                    BuyItem((ClientPacket.BuyItem)p);
                    break;
                case (short)ClientPacketIds.CraftItem:
                    CraftItem((ClientPacket.CraftItem)p);
                    break;
                case (short)ClientPacketIds.SellItem:
                    SellItem((ClientPacket.SellItem)p);
                    break;
                case (short)ClientPacketIds.RepairItem:
                    RepairItem((ClientPacket.RepairItem)p);
                    break;
                case (short)ClientPacketIds.BuyItemBack:
                    BuyItemBack((ClientPacket.BuyItemBack)p);
                    break;
                case (short)ClientPacketIds.SRepairItem:
                    SRepairItem((ClientPacket.SRepairItem)p);
                    break;
                case (short)ClientPacketIds.MagicKey:
                    MagicKey((ClientPacket.MagicKey)p);
                    break;
                case (short)ClientPacketIds.Magic:
                    Magic((ClientPacket.Magic)p);
                    break;
                case (short)ClientPacketIds.SwitchGroup:
                    SwitchGroup((ClientPacket.SwitchGroup)p);
                    return;
                case (short)ClientPacketIds.AddMember:
                    AddMember((ClientPacket.AddMember)p);
                    return;
                case (short)ClientPacketIds.DellMember:
                    DelMember((ClientPacket.DelMember)p);
                    return;
                case (short)ClientPacketIds.GroupInvite:
                    GroupInvite((ClientPacket.GroupInvite)p);
                    return;
                case (short)ClientPacketIds.NewHero:
                    NewHero((ClientPacket.NewHero)p);
                    break;
                case (short)ClientPacketIds.SetAutoPotValue:
                    SetAutoPotValue((ClientPacket.SetAutoPotValue)p);
                    break;
                case (short)ClientPacketIds.SetAutoPotItem:
                    SetAutoPotItem((ClientPacket.SetAutoPotItem)p);
                    break;
                case (short)ClientPacketIds.SetHeroBehaviour:
                    SetHeroBehaviour((ClientPacket.SetHeroBehaviour)p);
                    break;
                case (short)ClientPacketIds.ChangeHero:
                    ChangeHero((ClientPacket.ChangeHero)p);
                    break;
                case (short)ClientPacketIds.TownRevive:
                    TownRevive();
                    return;
                case (short)ClientPacketIds.SpellToggle:
                    SpellToggle((ClientPacket.SpellToggle)p);
                    return;
                case (short)ClientPacketIds.ConsignItem:
                    ConsignItem((ClientPacket.ConsignItem)p);
                    return;
                case (short)ClientPacketIds.MarketSearch:
                    MarketSearch((ClientPacket.MarketSearch)p);
                    return;
                case (short)ClientPacketIds.MarketRefresh:
                    MarketRefresh();
                    return;
                case (short)ClientPacketIds.MarketPage:
                    MarketPage((ClientPacket.MarketPage)p);
                    return;
                case (short)ClientPacketIds.MarketBuy:
                    MarketBuy((ClientPacket.MarketBuy)p);
                    return;
                case (short)ClientPacketIds.MarketGetBack:
                    MarketGetBack((ClientPacket.MarketGetBack)p);
                    return;
                case (short)ClientPacketIds.MarketSellNow:
                    MarketSellNow((ClientPacket.MarketSellNow)p);
                    return;
                case (short)ClientPacketIds.RequestUserName:
                    RequestUserName((ClientPacket.RequestUserName)p);
                    return;
                case (short)ClientPacketIds.RequestChatItem:
                    RequestChatItem((ClientPacket.RequestChatItem)p);
                    return;
                case (short)ClientPacketIds.EditGuildMember:
                    EditGuildMember((ClientPacket.EditGuildMember)p);
                    return;
                case (short)ClientPacketIds.EditGuildNotice:
                    EditGuildNotice((ClientPacket.EditGuildNotice)p);
                    return;
                case (short)ClientPacketIds.GuildInvite:
                    GuildInvite((ClientPacket.GuildInvite)p);
                    return;
                case (short)ClientPacketIds.RequestGuildInfo:
                    RequestGuildInfo((ClientPacket.RequestGuildInfo)p);
                    return;
                case (short)ClientPacketIds.GuildNameReturn:
                    GuildNameReturn((ClientPacket.GuildNameReturn)p);
                    return;
                case (short)ClientPacketIds.GuildStorageGoldChange:
                    GuildStorageGoldChange((ClientPacket.GuildStorageGoldChange)p);
                    return;
                case (short)ClientPacketIds.GuildStorageItemChange:
                    GuildStorageItemChange((ClientPacket.GuildStorageItemChange)p);
                    return;
                case (short)ClientPacketIds.GuildWarReturn:
                    GuildWarReturn((ClientPacket.GuildWarReturn)p);
                    return;
                case (short)ClientPacketIds.MarriageRequest:
                    MarriageRequest((ClientPacket.MarriageRequest)p);
                    return;
                case (short)ClientPacketIds.MarriageReply:
                    MarriageReply((ClientPacket.MarriageReply)p);
                    return;
                case (short)ClientPacketIds.ChangeMarriage:
                    ChangeMarriage((ClientPacket.ChangeMarriage)p);
                    return;
                case (short)ClientPacketIds.DivorceRequest:
                    DivorceRequest((ClientPacket.DivorceRequest)p);
                    return;
                case (short)ClientPacketIds.DivorceReply:
                    DivorceReply((ClientPacket.DivorceReply)p);
                    return;
                case (short)ClientPacketIds.AddMentor:
                    AddMentor((ClientPacket.AddMentor)p);
                    return;
                case (short)ClientPacketIds.MentorReply:
                    MentorReply((ClientPacket.MentorReply)p);
                    return;
                case (short)ClientPacketIds.AllowMentor:
                    AllowMentor((ClientPacket.AllowMentor)p);
                    return;
                case (short)ClientPacketIds.CancelMentor:
                    CancelMentor((ClientPacket.CancelMentor)p);
                    return;
                case (short)ClientPacketIds.TradeRequest:
                    TradeRequest((ClientPacket.TradeRequest)p);
                    return;
                case (short)ClientPacketIds.TradeGold:
                    TradeGold((ClientPacket.TradeGold)p);
                    return;
                case (short)ClientPacketIds.TradeReply:
                    TradeReply((ClientPacket.TradeReply)p);
                    return;
                case (short)ClientPacketIds.TradeConfirm:
                    TradeConfirm((ClientPacket.TradeConfirm)p);
                    return;
                case (short)ClientPacketIds.TradeCancel:
                    TradeCancel((ClientPacket.TradeCancel)p);
                    return;
                case (short)ClientPacketIds.EquipSlotItem:
                    EquipSlotItem((ClientPacket.EquipSlotItem)p);
                    break;
                case (short)ClientPacketIds.FishingCast:
                    FishingCast((ClientPacket.FishingCast)p);
                    break;
                case (short)ClientPacketIds.FishingChangeAutocast:
                    FishingChangeAutocast((ClientPacket.FishingChangeAutocast)p);
                    break;
                case (short)ClientPacketIds.AcceptQuest:
                    AcceptQuest((ClientPacket.AcceptQuest)p);
                    break;
                case (short)ClientPacketIds.FinishQuest:
                    FinishQuest((ClientPacket.FinishQuest)p);
                    break;
                case (short)ClientPacketIds.AbandonQuest:
                    AbandonQuest((ClientPacket.AbandonQuest)p);
                    break;
                case (short)ClientPacketIds.ShareQuest:
                    ShareQuest((ClientPacket.ShareQuest)p);
                    break;
                case (short)ClientPacketIds.AcceptReincarnation:
                    AcceptReincarnation();
                    break;
                case (short)ClientPacketIds.CancelReincarnation:
                    CancelReincarnation();
                    break;
                case (short)ClientPacketIds.CombineItem:
                    CombineItem((ClientPacket.CombineItem)p);
                    break;
                case (short)ClientPacketIds.AwakeningNeedMaterials:
                    AwakeningNeedMaterials((ClientPacket.AwakeningNeedMaterials)p);
                    break;
                case (short)ClientPacketIds.AwakeningLockedItem:
                    Enqueue(new ServerPacket.AwakeningLockedItem {
                        UniqueID = ((ClientPacket.AwakeningLockedItem)p).UniqueID,
                        Locked = ((ClientPacket.AwakeningLockedItem)p).Locked
                    });
                    break;
                case (short)ClientPacketIds.Awakening:
                    Awakening((ClientPacket.Awakening)p);
                    break;
                case (short)ClientPacketIds.DisassembleItem:
                    DisassembleItem((ClientPacket.DisassembleItem)p);
                    break;
                case (short)ClientPacketIds.DowngradeAwakening:
                    DowngradeAwakening((ClientPacket.DowngradeAwakening)p);
                    break;
                case (short)ClientPacketIds.ResetAddedItem:
                    ResetAddedItem((ClientPacket.ResetAddedItem)p);
                    break;
                case (short)ClientPacketIds.SendMail:
                    SendMail((ClientPacket.SendMail)p);
                    break;
                case (short)ClientPacketIds.ReadMail:
                    ReadMail((ClientPacket.ReadMail)p);
                    break;
                case (short)ClientPacketIds.CollectParcel:
                    CollectParcel((ClientPacket.CollectParcel)p);
                    break;
                case (short)ClientPacketIds.DeleteMail:
                    DeleteMail((ClientPacket.DeleteMail)p);
                    break;
                case (short)ClientPacketIds.LockMail:
                    LockMail((ClientPacket.LockMail)p);
                    break;
                case (short)ClientPacketIds.MailLockedItem:
                    Enqueue(new ServerPacket.MailLockedItem {
                        UniqueID = ((ClientPacket.MailLockedItem)p).UniqueID,
                        Locked = ((ClientPacket.MailLockedItem)p).Locked
                    });
                    break;
                case (short)ClientPacketIds.MailCost:
                    MailCost((ClientPacket.MailCost)p);
                    break;
                case (short)ClientPacketIds.RequestIntelligentCreatureUpdates:
                    RequestIntelligentCreatureUpdates((ClientPacket.RequestIntelligentCreatureUpdates)p);
                    break;
                case (short)ClientPacketIds.UpdateIntelligentCreature:
                    UpdateIntelligentCreature((ClientPacket.UpdateIntelligentCreature)p);
                    break;
                case (short)ClientPacketIds.IntelligentCreaturePickup:
                    IntelligentCreaturePickup((ClientPacket.IntelligentCreaturePickup)p);
                    break;
                case (short)ClientPacketIds.AddFriend:
                    AddFriend((ClientPacket.AddFriend)p);
                    break;
                case (short)ClientPacketIds.RemoveFriend:
                    RemoveFriend((ClientPacket.RemoveFriend)p);
                    break;
                case (short)ClientPacketIds.RefreshFriends: {
                    if(Stage != GameStage.Game) {
                        return;
                    }

                    Player.GetFriends();
                    break;
                }
                case (short)ClientPacketIds.AddMemo:
                    AddMemo((ClientPacket.AddMemo)p);
                    break;
                case (short)ClientPacketIds.GuildBuffUpdate:
                    GuildBuffUpdate((ClientPacket.GuildBuffUpdate)p);
                    break;
                case (short)ClientPacketIds.GameshopBuy:
                    GameshopBuy((ClientPacket.GameshopBuy)p);
                    return;
                case (short)ClientPacketIds.NpcConfirmInput:
                    NpcConfirmInput((ClientPacket.NpcConfirmInput)p);
                    break;
                case (short)ClientPacketIds.ReportIssue:
                    ReportIssue((ClientPacket.ReportIssue)p);
                    break;
                case (short)ClientPacketIds.GetRanking:
                    GetRanking((ClientPacket.GetRanking)p);
                    break;
                case (short)ClientPacketIds.Opendoor:
                    Opendoor((ClientPacket.Opendoor)p);
                    break;
                case (short)ClientPacketIds.GetRentedItems:
                    GetRentedItems();
                    break;
                case (short)ClientPacketIds.ItemRentalRequest:
                    ItemRentalRequest();
                    break;
                case (short)ClientPacketIds.ItemRentalFee:
                    ItemRentalFee((ClientPacket.ItemRentalFee)p);
                    break;
                case (short)ClientPacketIds.ItemRentalPeriod:
                    ItemRentalPeriod((ClientPacket.ItemRentalPeriod)p);
                    break;
                case (short)ClientPacketIds.DepositRentalItem:
                    DepositRentalItem((ClientPacket.DepositRentalItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveRentalItem:
                    RetrieveRentalItem((ClientPacket.RetrieveRentalItem)p);
                    break;
                case (short)ClientPacketIds.CancelItemRental:
                    CancelItemRental();
                    break;
                case (short)ClientPacketIds.ItemRentalLockFee:
                    ItemRentalLockFee();
                    break;
                case (short)ClientPacketIds.ItemRentalLockItem:
                    ItemRentalLockItem();
                    break;
                case (short)ClientPacketIds.ConfirmItemRental:
                    ConfirmItemRental();
                    break;
                default:
                    MessageQueue.Enqueue(string.Format("Invalid packet received. Index : {0}", p.Index));
                    break;
            }
        }

        public void SoftDisconnect(byte reason) {
            Stage = GameStage.Disconnected;
            TimeDisconnected = Envir.Time;

            lock (Envir.AccountLock) {
                if(Player != null) {
                    Player.StopGame(reason);
                }

                if(Account != null && Account.Connection == this) {
                    Account.Connection = null;
                }
            }

            Account = null;
        }

        public void Disconnect(byte reason) {
            if(!Connected) {
                return;
            }

            Connected = false;
            Stage = GameStage.Disconnected;
            TimeDisconnected = Envir.Time;

            lock (Envir.Connections) {
                Envir.Connections.Remove(this);
            }

            lock (Envir.AccountLock) {
                if(Player != null) {
                    Player.StopGame(reason);
                }

                if(Account != null && Account.Connection == this) {
                    Account.Connection = null;
                }
            }

            if(Observing != null) {
                Observing.Observers.Remove(this);
            }

            Account = null;

            _sendList = null;
            _receiveList = null;
            _retryList = null;
            _rawData = null;

            if(_client != null) {
                _client.Client.Dispose();
            }

            _client = null;
        }

        public void SendDisconnect(byte reason) {
            if(!Connected) {
                Disconnecting = true;
                SoftDisconnect(reason);
                return;
            }

            Disconnecting = true;

            List<byte> data = new();

            data.AddRange(new ServerPacket.Disconnect { Reason = reason }.GetPacketBytes());

            BeginSend(data);
            SoftDisconnect(reason);
        }

        public void CleanObservers() {
            foreach(MirConnection c in Observers) {
                c.Stage = GameStage.Login;
                c.Enqueue(new ServerPacket.ReturnToLogin());
            }
        }

        private void ClientVersion(ClientPacket.ClientVersion p) {
            if(Stage != GameStage.None) {
                return;
            }

            if(Settings.CheckVersion) {
                bool match = false;

                foreach(byte[] hash in Settings.VersionHashes) {
                    if(Functions.CompareBytes(hash, p.VersionHash)) {
                        match = true;
                        break;
                    }
                }

                if(!match) {
                    Disconnecting = true;

                    List<byte> data = new();

                    data.AddRange(new ServerPacket.ClientVersion { Result = 0 }.GetPacketBytes());

                    BeginSend(data);
                    SoftDisconnect(10);
                    MessageQueue.Enqueue(SessionID + ", Disconnnected - Wrong Client Version.");
                    return;
                }
            }

            MessageQueue.Enqueue(SessionID + ", " + IPAddress + ", Client version matched.");
            Enqueue(new ServerPacket.ClientVersion { Result = 1 });

            Stage = GameStage.Login;
        }

        private void ClientKeepAlive(ClientPacket.KeepAlive p) {
            Enqueue(new ServerPacket.KeepAlive {
                Time = p.Time
            });
        }

        private void NewAccount(ClientPacket.NewAccount p) {
            if(Stage != GameStage.Login) {
                return;
            }

            MessageQueue.Enqueue(SessionID + ", " + IPAddress + ", New account being created.");
            Envir.NewAccount(p, this);
        }

        private void ChangePassword(ClientPacket.ChangePassword p) {
            if(Stage != GameStage.Login) {
                return;
            }

            MessageQueue.Enqueue(SessionID + ", " + IPAddress + ", Password being changed.");
            Envir.ChangePassword(p, this);
        }

        private void Login(ClientPacket.Login p) {
            if(Stage != GameStage.Login) {
                return;
            }

            MessageQueue.Enqueue(SessionID + ", " + IPAddress + ", User logging in.");
            Envir.Login(p, this);
        }

        private void NewCharacter(ClientPacket.NewCharacter p) {
            if(Stage != GameStage.Select) {
                return;
            }

            Envir.NewCharacter(p, this, Account.AdminAccount);
        }

        private void DeleteCharacter(ClientPacket.DeleteCharacter p) {
            if(Stage != GameStage.Select) {
                return;
            }

            if(!Settings.AllowDeleteCharacter) {
                Enqueue(new ServerPacket.DeleteCharacter { Result = 0 });
                return;
            }

            CharacterInfo temp = null;


            for (int i = 0; i < Account.Characters.Count; i++) {
                if(Account.Characters[i].Index != p.CharacterIndex) {
                    continue;
                }

                temp = Account.Characters[i];
                break;
            }

            if(temp == null) {
                Enqueue(new ServerPacket.DeleteCharacter { Result = 1 });
                return;
            }

            temp.Deleted = true;
            temp.DeleteDate = Envir.Now;
            Envir.RemoveRank(temp);
            Enqueue(new ServerPacket.DeleteCharacterSuccess { CharacterIndex = temp.Index });
        }

        private void StartGame(ClientPacket.StartGame p) {
            if(Stage != GameStage.Select) {
                return;
            }

            if(!Settings.AllowStartGame && (Account == null || (Account != null && !Account.AdminAccount))) {
                Enqueue(new ServerPacket.StartGame { Result = 0 });
                return;
            }

            if(Account == null) {
                Enqueue(new ServerPacket.StartGame { Result = 1 });
                return;
            }


            CharacterInfo info = null;

            for (int i = 0; i < Account.Characters.Count; i++) {
                if(Account.Characters[i].Index != p.CharacterIndex) {
                    continue;
                }

                info = Account.Characters[i];
                break;
            }

            if(info == null) {
                Enqueue(new ServerPacket.StartGame { Result = 2 });
                return;
            }

            if(info.Banned) {
                if(info.ExpiryDate > Envir.Now) {
                    Enqueue(new ServerPacket.StartGameBanned { Reason = info.BanReason, ExpiryDate = info.ExpiryDate });
                    return;
                }

                info.Banned = false;
            }

            info.BanReason = string.Empty;
            info.ExpiryDate = DateTime.MinValue;

            long delay = (long)(Envir.Now - info.LastLogoutDate).TotalMilliseconds;


            //if (delay < Settings.RelogDelay)
            //{
            //    Enqueue(new S.StartGameDelay { Milliseconds = Settings.RelogDelay - delay });
            //    return;
            //}

            Player = new PlayerObject(info, this);
            Player.StartGame();
        }

        public void LogOut() {
            if(Stage != GameStage.Game) {
                return;
            }

            if(Envir.Time < Player.LogTime) {
                Enqueue(new ServerPacket.LogOutFailed());
                return;
            }

            Player.StopGame(23);

            Stage = GameStage.Select;
            Player = null;

            Enqueue(new ServerPacket.LogOutSuccess { Characters = Account.GetSelectInfo() });
        }

        private void Turn(ClientPacket.Turn p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(Player.ActionTime > Envir.Time) {
                _retryList.Enqueue(p);
            } else {
                Player.Turn(p.Direction);
            }
        }

        private void Walk(ClientPacket.Walk p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(Player.ActionTime > Envir.Time) {
                _retryList.Enqueue(p);
            } else {
                Player.Walk(p.Direction);
            }
        }

        private void Run(ClientPacket.Run p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(Player.ActionTime > Envir.Time) {
                _retryList.Enqueue(p);
            } else {
                Player.Run(p.Direction);
            }
        }

        private void Chat(ClientPacket.Chat p) {
            if(p.Message.Length > Globals.MaxChatLength) {
                SendDisconnect(2);
                return;
            }

            if(Stage != GameStage.Game) {
                return;
            }

            Player.Chat(p.Message, p.LinkedItems);
        }

        private void MoveItem(ClientPacket.MoveItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MoveItem(p.Grid, p.From, p.To);
        }

        private void StoreItem(ClientPacket.StoreItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.StoreItem(p.From, p.To);
        }

        private void DepositRefineItem(ClientPacket.DepositRefineItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DepositRefineItem(p.From, p.To);
        }

        private void RetrieveRefineItem(ClientPacket.RetrieveRefineItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RetrieveRefineItem(p.From, p.To);
        }

        private void RefineCancel(ClientPacket.RefineCancel p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RefineCancel();
        }

        private void RefineItem(ClientPacket.RefineItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RefineItem(p.UniqueID);
        }

        private void CheckRefine(ClientPacket.CheckRefine p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.CheckRefine(p.UniqueID);
        }

        private void ReplaceWedRing(ClientPacket.ReplaceWedRing p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ReplaceWeddingRing(p.UniqueID);
        }

        private void DepositTradeItem(ClientPacket.DepositTradeItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DepositTradeItem(p.From, p.To);
        }

        private void RetrieveTradeItem(ClientPacket.RetrieveTradeItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RetrieveTradeItem(p.From, p.To);
        }

        private void TakeBackItem(ClientPacket.TakeBackItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TakeBackItem(p.From, p.To);
        }

        private void MergeItem(ClientPacket.MergeItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MergeItem(p.GridFrom, p.GridTo, p.IDFrom, p.IDTo);
        }

        private void EquipItem(ClientPacket.EquipItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.EquipItem(p.Grid, p.UniqueID, p.To);
        }

        private void RemoveItem(ClientPacket.RemoveItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RemoveItem(p.Grid, p.UniqueID, p.To);
        }

        private void RemoveSlotItem(ClientPacket.RemoveSlotItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RemoveSlotItem(p.Grid, p.UniqueID, p.To, p.GridTo, p.FromUniqueID);
        }

        private void SplitItem(ClientPacket.SplitItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SplitItem(p.Grid, p.UniqueID, p.Count);
        }

        private void UseItem(ClientPacket.UseItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            switch (p.Grid) {
                case MirGridType.Inventory:
                    Player.UseItem(p.UniqueID);
                    break;
                case MirGridType.HeroInventory:
                    Player.HeroUseItem(p.UniqueID);
                    break;
            }
        }

        private void DropItem(ClientPacket.DropItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DropItem(p.UniqueID, p.Count, p.HeroInventory);
        }

        private void TakeBackHeroItem(ClientPacket.TakeBackHeroItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TakeBackHeroItem(p.From, p.To);
        }

        private void TransferHeroItem(ClientPacket.TransferHeroItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TransferHeroItem(p.From, p.To);
        }

        private void DropGold(ClientPacket.DropGold p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DropGold(p.Amount);
        }

        private void PickUp() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.PickUp();
        }

        private void RequestMapInfo(ClientPacket.RequestMapInfo p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RequestMapInfo(p.MapIndex);
        }

        private void TeleportToNpc(ClientPacket.TeleportToNpc p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TeleportToNpc(p.ObjectID);
        }

        private void SearchMap(ClientPacket.SearchMap p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SearchMap(p.Text);
        }

        private void Inspect(ClientPacket.Inspect p) {
            if(Stage != GameStage.Game && Stage != GameStage.Observer) {
                return;
            }

            if(p.Ranking) {
                Envir.Inspect(this, (int)p.ObjectID);
            } else if(p.Hero) {
                Envir.InspectHero(this, (int)p.ObjectID);
            } else {
                Envir.Inspect(this, p.ObjectID);
            }
        }

        private void Observe(ClientPacket.Observe p) {
            if(Stage != GameStage.Game && Stage != GameStage.Observer) {
                return;
            }

            Envir.Observe(this, p.Name);
        }

        private void ChangeAMode(ClientPacket.ChangeAMode p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AMode = p.Mode;

            Enqueue(new ServerPacket.ChangeAMode { Mode = Player.AMode });
        }

        private void ChangePMode(ClientPacket.ChangePMode p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.PMode = p.Mode;

            Enqueue(new ServerPacket.ChangePMode { Mode = Player.PMode });
        }

        private void ChangeTrade(ClientPacket.ChangeTrade p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AllowTrade = p.AllowTrade;
        }

        private void Attack(ClientPacket.Attack p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(!Player.Dead && (Player.ActionTime > Envir.Time || Player.AttackTime > Envir.Time)) {
                _retryList.Enqueue(p);
            } else {
                Player.Attack(p.Direction, p.Spell);
            }
        }

        private void RangeAttack(ClientPacket.RangeAttack p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(!Player.Dead && (Player.ActionTime > Envir.Time || Player.AttackTime > Envir.Time)) {
                _retryList.Enqueue(p);
            } else {
                Player.RangeAttack(p.Direction, p.TargetLocation, p.TargetID);
            }
        }

        private void Harvest(ClientPacket.Harvest p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(!Player.Dead && Player.ActionTime > Envir.Time) {
                _retryList.Enqueue(p);
            } else {
                Player.Harvest(p.Direction);
            }
        }

        private void CallNpc(ClientPacket.CallNpc p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(p.Key.Length > 30) //No Npc Key should be that long.
            {
                SendDisconnect(2);
                return;
            }

            if(p.ObjectID == Envir.DefaultNpc.LoadedObjectID && Player.NpcObjectID == Envir.DefaultNpc.LoadedObjectID) {
                Player.CallDefaultNpc(p.Key);
                return;
            }

            if(p.ObjectID == uint.MaxValue) {
                Player.CallDefaultNpc(DefaultNpcType.Client, null);
                return;
            }

            Player.CallNpc(p.ObjectID, p.Key);
        }

        private void BuyItem(ClientPacket.BuyItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.BuyItem(p.ItemIndex, p.Count, p.Type);
        }

        private void CraftItem(ClientPacket.CraftItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.CraftItem(p.UniqueID, p.Count, p.Slots);
        }

        private void SellItem(ClientPacket.SellItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SellItem(p.UniqueID, p.Count);
        }

        private void RepairItem(ClientPacket.RepairItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RepairItem(p.UniqueID);
        }

        private void BuyItemBack(ClientPacket.BuyItemBack p) {
            if(Stage != GameStage.Game) {
                return;
            }

            // Player.BuyItemBack(p.UniqueID, p.Count);
        }

        private void SRepairItem(ClientPacket.SRepairItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RepairItem(p.UniqueID, true);
        }

        private void MagicKey(ClientPacket.MagicKey p) {
            if(Stage != GameStage.Game) {
                return;
            }

            HumanObject actor = Player;
            if(p.Key > 16 || p.OldKey > 16) {
                if(!Player.HeroSpawned || Player.Hero.Dead) {
                    return;
                }

                actor = Player.Hero;
            }

            for (int i = 0; i < actor.Info.Magics.Count; i++) {
                UserMagic magic = actor.Info.Magics[i];
                if(magic.Spell != p.Spell) {
                    if(magic.Key == p.Key) {
                        magic.Key = 0;
                    }

                    continue;
                }

                magic.Key = p.Key;
            }
        }

        private void Magic(ClientPacket.Magic p) {
            if(Stage != GameStage.Game) {
                return;
            }

            HumanObject actor = Player;
            if(Player.HeroSpawned && p.ObjectID == Player.Hero.ObjectID) {
                actor = Player.Hero;
            }

            if(actor.Dead) {
                return;
            }

            if(!actor.Dead && (actor.ActionTime > Envir.Time || actor.SpellTime > Envir.Time)) {
                _retryList.Enqueue(p);
            } else {
                actor.BeginMagic(p.Spell, p.Direction, p.TargetID, p.Location, p.SpellTargetLock);
            }
        }

        private void SwitchGroup(ClientPacket.SwitchGroup p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SwitchGroup(p.AllowGroup);
        }

        private void AddMember(ClientPacket.AddMember p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AddMember(p.Name);
        }

        private void DelMember(ClientPacket.DelMember p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DelMember(p.Name);
        }

        private void GroupInvite(ClientPacket.GroupInvite p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.GroupInvite(p.AcceptInvite);
        }

        private void NewHero(ClientPacket.NewHero p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.NewHero(p);
        }

        private void SetAutoPotValue(ClientPacket.SetAutoPotValue p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SetAutoPotValue(p.Stat, p.Value);
        }

        private void SetAutoPotItem(ClientPacket.SetAutoPotItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SetAutoPotItem(p.Grid, p.ItemIndex);
        }

        private void SetHeroBehaviour(ClientPacket.SetHeroBehaviour p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SetHeroBehaviour(p.Behaviour);
        }

        private void ChangeHero(ClientPacket.ChangeHero p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ChangeHero(p.ListIndex);
        }

        private void TownRevive() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TownRevive();
        }

        private void SpellToggle(ClientPacket.SpellToggle p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(p.canUse > SpellToggleState.None) {
                Player.SpellToggle(p.Spell, p.canUse);
                return;
            }

            if(Player.HeroSpawned) {
                Player.Hero.SpellToggle(p.Spell, p.canUse);
            }
        }

        private void ConsignItem(ClientPacket.ConsignItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ConsignItem(p.UniqueID, p.Price, p.Type);
        }

        private void MarketSearch(ClientPacket.MarketSearch p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.UserMatch = p.Usermode;
            Player.MinShapes = p.MinShape;
            Player.MaxShapes = p.MaxShape;
            Player.MarketPanelType = p.MarketType;

            Player.MarketSearch(p.Match, p.Type);
        }

        private void MarketRefresh() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MarketSearch(string.Empty, Player.MatchType);
        }

        private void MarketPage(ClientPacket.MarketPage p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MarketPage(p.Page);
        }

        private void MarketBuy(ClientPacket.MarketBuy p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MarketBuy(p.AuctionID, p.BidPrice);
        }

        private void MarketSellNow(ClientPacket.MarketSellNow p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MarketSellNow(p.AuctionID);
        }

        private void MarketGetBack(ClientPacket.MarketGetBack p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MarketGetBack(p.AuctionID);
        }

        private void RequestUserName(ClientPacket.RequestUserName p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RequestUserName(p.UserID);
        }

        private void RequestChatItem(ClientPacket.RequestChatItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RequestChatItem(p.ChatItemID);
        }

        private void EditGuildMember(ClientPacket.EditGuildMember p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.EditGuildMember(p.Name, p.RankName, p.RankIndex, p.ChangeType);
        }

        private void EditGuildNotice(ClientPacket.EditGuildNotice p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.EditGuildNotice(p.notice);
        }

        private void GuildInvite(ClientPacket.GuildInvite p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.GuildInvite(p.AcceptInvite);
        }

        private void RequestGuildInfo(ClientPacket.RequestGuildInfo p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RequestGuildInfo(p.Type);
        }

        private void GuildNameReturn(ClientPacket.GuildNameReturn p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.GuildNameReturn(p.Name);
        }

        private void GuildStorageGoldChange(ClientPacket.GuildStorageGoldChange p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.GuildStorageGoldChange(p.Type, p.Amount);
        }

        private void GuildStorageItemChange(ClientPacket.GuildStorageItemChange p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.GuildStorageItemChange(p.Type, p.From, p.To);
        }

        private void GuildWarReturn(ClientPacket.GuildWarReturn p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.GuildWarReturn(p.Name);
        }


        private void MarriageRequest(ClientPacket.MarriageRequest p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MarriageRequest();
        }

        private void MarriageReply(ClientPacket.MarriageReply p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MarriageReply(p.AcceptInvite);
        }

        private void ChangeMarriage(ClientPacket.ChangeMarriage p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(Player.Info.Married == 0) {
                Player.AllowMarriage = !Player.AllowMarriage;
                if(Player.AllowMarriage) {
                    Player.ReceiveChat("You're now allowing marriage requests.", ChatType.Hint);
                } else {
                    Player.ReceiveChat("You're now blocking marriage requests.", ChatType.Hint);
                }
            } else {
                Player.AllowLoverRecall = !Player.AllowLoverRecall;
                if(Player.AllowLoverRecall) {
                    Player.ReceiveChat("You're now allowing recall from lover.", ChatType.Hint);
                } else {
                    Player.ReceiveChat("You're now blocking recall from lover.", ChatType.Hint);
                }
            }
        }

        private void DivorceRequest(ClientPacket.DivorceRequest p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DivorceRequest();
        }

        private void DivorceReply(ClientPacket.DivorceReply p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DivorceReply(p.AcceptInvite);
        }

        private void AddMentor(ClientPacket.AddMentor p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AddMentor(p.Name);
        }

        private void MentorReply(ClientPacket.MentorReply p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MentorReply(p.AcceptInvite);
        }

        private void AllowMentor(ClientPacket.AllowMentor p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AllowMentor = !Player.AllowMentor;
            if(Player.AllowMentor) {
                Player.ReceiveChat(GameLanguage.AllowingMentorRequests, ChatType.Hint);
            } else {
                Player.ReceiveChat(GameLanguage.BlockingMentorRequests, ChatType.Hint);
            }
        }

        private void CancelMentor(ClientPacket.CancelMentor p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.MentorBreak(true);
        }

        private void TradeRequest(ClientPacket.TradeRequest p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TradeRequest();
        }

        private void TradeGold(ClientPacket.TradeGold p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TradeGold(p.Amount);
        }

        private void TradeReply(ClientPacket.TradeReply p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TradeReply(p.AcceptInvite);
        }

        private void TradeConfirm(ClientPacket.TradeConfirm p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TradeConfirm(p.Locked);
        }

        private void TradeCancel(ClientPacket.TradeCancel p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.TradeCancel();
        }

        private void EquipSlotItem(ClientPacket.EquipSlotItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.EquipSlotItem(p.Grid, p.UniqueID, p.To, p.GridTo, p.ToUniqueID);
        }

        private void FishingCast(ClientPacket.FishingCast p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.FishingCast(p.CastOut, true);
        }

        private void FishingChangeAutocast(ClientPacket.FishingChangeAutocast p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.FishingChangeAutocast(p.AutoCast);
        }

        private void AcceptQuest(ClientPacket.AcceptQuest p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AcceptQuest(p.QuestIndex); //p.NpcIndex,
        }

        private void FinishQuest(ClientPacket.FinishQuest p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.FinishQuest(p.QuestIndex, p.SelectedItemIndex);
        }

        private void AbandonQuest(ClientPacket.AbandonQuest p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AbandonQuest(p.QuestIndex);
        }

        private void ShareQuest(ClientPacket.ShareQuest p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ShareQuest(p.QuestIndex);
        }

        private void AcceptReincarnation() {
            if(Stage != GameStage.Game) {
                return;
            }

            if(Player.ReincarnationHost != null && Player.ReincarnationHost.ReincarnationReady) {
                Player.Revive(Player.Stats[Stat.HP] / 2, true);
                Player.ReincarnationHost = null;
                return;
            }

            Player.ReceiveChat("Reincarnation failed", ChatType.System);
        }

        private void CancelReincarnation() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ReincarnationExpireTime = Envir.Time;
        }

        private void CombineItem(ClientPacket.CombineItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.CombineItem(p.Grid, p.IDFrom, p.IDTo);
        }

        private void Awakening(ClientPacket.Awakening p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.Awakening(p.UniqueID, p.Type);
        }

        private void AwakeningNeedMaterials(ClientPacket.AwakeningNeedMaterials p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AwakeningNeedMaterials(p.UniqueID, p.Type);
        }

        private void DisassembleItem(ClientPacket.DisassembleItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DisassembleItem(p.UniqueID);
        }

        private void DowngradeAwakening(ClientPacket.DowngradeAwakening p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DowngradeAwakening(p.UniqueID);
        }

        private void ResetAddedItem(ClientPacket.ResetAddedItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ResetAddedItem(p.UniqueID);
        }

        public void SendMail(ClientPacket.SendMail p) {
            if(Stage != GameStage.Game) {
                return;
            }

            if(p.Gold > 0 || p.ItemsIdx.Length > 0) {
                Player.SendMail(p.Name, p.Message, p.Gold, p.ItemsIdx, p.Stamped);
            } else {
                Player.SendMail(p.Name, p.Message);
            }
        }

        public void ReadMail(ClientPacket.ReadMail p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ReadMail(p.MailID);
        }

        public void CollectParcel(ClientPacket.CollectParcel p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.CollectMail(p.MailID);
        }

        public void DeleteMail(ClientPacket.DeleteMail p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DeleteMail(p.MailID);
        }

        public void LockMail(ClientPacket.LockMail p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.LockMail(p.MailID, p.Lock);
        }

        public void MailCost(ClientPacket.MailCost p) {
            if(Stage != GameStage.Game) {
                return;
            }

            uint cost = Player.GetMailCost(p.ItemsIdx, p.Gold, p.Stamped);

            Enqueue(new ServerPacket.MailCost { Cost = cost });
        }

        private void RequestIntelligentCreatureUpdates(ClientPacket.RequestIntelligentCreatureUpdates p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SendIntelligentCreatureUpdates = p.Update;
        }

        private void UpdateIntelligentCreature(ClientPacket.UpdateIntelligentCreature p) {
            if(Stage != GameStage.Game) {
                return;
            }

            ClientIntelligentCreature petUpdate = p.Creature;
            if(petUpdate == null) {
                return;
            }

            if(p.ReleaseMe) {
                Player.ReleaseIntelligentCreature(petUpdate.PetType);
                return;
            } else if(p.SummonMe) {
                Player.SummonIntelligentCreature(petUpdate.PetType);
                return;
            } else if(p.UnSummonMe) {
                Player.UnSummonIntelligentCreature(petUpdate.PetType);
                return;
            } else {
                //Update the creature info
                for (int i = 0; i < Player.Info.IntelligentCreatures.Count; i++) {
                    if(Player.Info.IntelligentCreatures[i].PetType == petUpdate.PetType) {
                        Regex reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinCharacterNameLength + "," +
                                              Globals.MaxCharacterNameLength + "}$");

                        if(reg.IsMatch(petUpdate.CustomName)) {
                            Player.Info.IntelligentCreatures[i].CustomName = petUpdate.CustomName;
                        }

                        Player.Info.IntelligentCreatures[i].SlotIndex = petUpdate.SlotIndex;
                        Player.Info.IntelligentCreatures[i].Filter = petUpdate.Filter;
                        Player.Info.IntelligentCreatures[i].petMode = petUpdate.petMode;
                    } else {
                        continue;
                    }
                }

                if(Player.CreatureSummoned) {
                    if(Player.SummonedCreatureType == petUpdate.PetType) {
                        Player.UpdateSummonedCreature(petUpdate.PetType);
                    }
                }
            }
        }

        private void IntelligentCreaturePickup(ClientPacket.IntelligentCreaturePickup p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.IntelligentCreaturePickup(p.MouseMode, p.Location);
        }

        private void AddFriend(ClientPacket.AddFriend p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AddFriend(p.Name, p.Blocked);
        }

        private void RemoveFriend(ClientPacket.RemoveFriend p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RemoveFriend(p.CharacterIndex);
        }

        private void AddMemo(ClientPacket.AddMemo p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.AddMemo(p.CharacterIndex, p.Memo);
        }

        private void GuildBuffUpdate(ClientPacket.GuildBuffUpdate p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.GuildBuffUpdate(p.Action, p.Id);
        }

        private void GameshopBuy(ClientPacket.GameshopBuy p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.GameshopBuy(p.GIndex, p.Quantity);
        }

        private void NpcConfirmInput(ClientPacket.NpcConfirmInput p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.NpcData["NpcInputStr"] = p.Value;

            if(p.NpcID == Envir.DefaultNpc.LoadedObjectID && Player.NpcObjectID == Envir.DefaultNpc.LoadedObjectID) {
                Player.CallDefaultNpc(p.PageName);
                return;
            }

            Player.CallNpc(Player.NpcObjectID, p.PageName);
        }

        public List<byte[]> Image = new();

        private void ReportIssue(ClientPacket.ReportIssue p) {
            if(Stage != GameStage.Game) {
                return;
            }

            return;

            // Image.Add(p.Image);

            // if (p.ImageChunk >= p.ImageSize)
            // {
            //     System.Drawing.Image image = Functions.ByteArrayToImage(Functions.CombineArray(Image));
            //     image.Save("Reported-" + Player.Name + "-" + DateTime.Now.ToString("yyMMddHHmmss") + ".jpg");
            //     Image.Clear();
            // }
        }

        private void GetRanking(ClientPacket.GetRanking p) {
            if(Stage != GameStage.Game && Stage != GameStage.Observer) {
                return;
            }

            Envir.GetRanking(this, p.RankType, p.RankIndex, p.OnlineOnly);
        }

        private void Opendoor(ClientPacket.Opendoor p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.Opendoor(p.DoorIndex);
        }

        private void GetRentedItems() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.GetRentedItems();
        }

        private void ItemRentalRequest() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ItemRentalRequest();
        }

        private void ItemRentalFee(ClientPacket.ItemRentalFee p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SetItemRentalFee(p.Amount);
        }

        private void ItemRentalPeriod(ClientPacket.ItemRentalPeriod p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.SetItemRentalPeriodLength(p.Days);
        }

        private void DepositRentalItem(ClientPacket.DepositRentalItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.DepositRentalItem(p.From, p.To);
        }

        private void RetrieveRentalItem(ClientPacket.RetrieveRentalItem p) {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.RetrieveRentalItem(p.From, p.To);
        }

        private void CancelItemRental() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.CancelItemRental();
        }

        private void ItemRentalLockFee() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ItemRentalLockFee();
        }

        private void ItemRentalLockItem() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ItemRentalLockItem();
        }

        private void ConfirmItemRental() {
            if(Stage != GameStage.Game) {
                return;
            }

            Player.ConfirmItemRental();
        }

        public void CheckItemInfo(ItemInfo info, bool dontLoop = false) {
            if(dontLoop == false &&
               info.ClassBased | info.LevelBased) //send all potential data so client can display it
            {
                for (int i = 0; i < Envir.ItemInfoList.Count; i++) {
                    if(Envir.ItemInfoList[i] != info && Envir.ItemInfoList[i].Name.StartsWith(info.Name)) {
                        CheckItemInfo(Envir.ItemInfoList[i], true);
                    }
                }
            }

            if(SentItemInfo.Contains(info)) {
                return;
            }

            Enqueue(new ServerPacket.NewItemInfo { Info = info });
            SentItemInfo.Add(info);
        }

        public void CheckItem(UserItem item) {
            CheckItemInfo(item.Info);

            for (int i = 0; i < item.Slots.Length; i++) {
                if(item.Slots[i] == null) {
                    continue;
                }

                CheckItemInfo(item.Slots[i].Info);
            }

            CheckHeroInfo(item);
        }

        private void CheckHeroInfo(UserItem item) {
            if(item.AddedStats[Stat.Hero] == 0) {
                return;
            }

            if(SentHeroInfo.Contains(item.UniqueID)) {
                return;
            }

            HeroInfo heroInfo = Envir.GetHeroInfo(item.AddedStats[Stat.Hero]);
            if(heroInfo == null) {
                return;
            }

            Enqueue(new ServerPacket.NewHeroInfo { Info = heroInfo.ClientInformation });
            SentHeroInfo.Add(item.UniqueID);
        }
    }

    public class MirConnectionLog {
        public string IPAddress = "";
        public List<long> AccountsMade = new();
        public List<long> CharactersMade = new();
    }
}
