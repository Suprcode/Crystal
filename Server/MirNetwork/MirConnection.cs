using System.Collections.Concurrent;
using System.Net.Sockets;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;
using C = ClientPackets;
using S = ServerPackets;
using System.Text.RegularExpressions;
using Server.Utils;

namespace Server.MirNetwork
{
    public enum GameStage { None, Login, Select, Game, Observer, Disconnected }

    public class MirConnection
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        protected static MessageQueue MessageQueue
        {
            get { return MessageQueue.Instance; }
        }

        public readonly int SessionID;
        public readonly string IPAddress;

        public GameStage Stage;

        private TcpClient _client;
        private ConcurrentQueue<Packet> _receiveList;
        private ConcurrentQueue<Packet> _sendList; 
        private Queue<Packet> _retryList;

        private bool _disconnecting;
        public bool Connected;
        public bool Disconnecting
        {
            get { return _disconnecting; }
            set
            {
                if (_disconnecting == value) return;
                _disconnecting = value;
                TimeOutTime = Envir.Time + 500;
            }
        }
        public readonly long TimeConnected;
        public long TimeDisconnected, TimeOutTime;

        byte[] _rawData = new byte[0];
        byte[] _rawBytes = new byte[8 * 1024];

        public AccountInfo Account;
        public PlayerObject Player;

        public List<MirConnection> Observers = new List<MirConnection>();
        public MirConnection Observing;

        public List<ItemInfo> SentItemInfo = new List<ItemInfo>();
        public List<QuestInfo> SentQuestInfo = new List<QuestInfo>();
        public List<RecipeInfo> SentRecipeInfo = new List<RecipeInfo>();
        public List<UserItem> SentChatItem = new List<UserItem>(); //TODO - Add Expiry time
        public List<MapInfo> SentMapInfo = new List<MapInfo>();
        public List<ulong> SentHeroInfo = new List<ulong>();
        public bool WorldMapSetupSent;
        public bool StorageSent;
        public bool HeroStorageSent;
        public Dictionary<long, DateTime> SentRankings = new Dictionary<long, DateTime>();

        private DateTime _dataCounterReset;
        private int _dataCounter;
        private FixedSizedQueue<Packet> _lastPackets;

        public MirConnection(int sessionID, TcpClient client)
        {
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
            _sendList.Enqueue(new S.Connected());
            _retryList = new Queue<Packet>();

            Connected = true;
            BeginReceive();
        }

        public void AddObserver(MirConnection c)
        {
            Observers.Add(c);

            if (c.Observing != null)
                c.Observing.Observers.Remove(c);
            c.Observing = this;

            c.Stage = GameStage.Observer;
        }

        private void BeginReceive()
        {
            if (!Connected) return;

            try
            {
                _client.Client.BeginReceive(_rawBytes, 0, _rawBytes.Length, SocketFlags.None, ReceiveData, _rawBytes);
            }
            catch
            {
                Disconnecting = true;
            }
        }

        private void ReceiveData(IAsyncResult result)
        {
            if (!Connected) return;

            int dataRead;

            try
            {
                dataRead = _client.Client.EndReceive(result);
            }
            catch
            {
                Disconnecting = true;
                return;
            }

            if (dataRead == 0)
            {
                Disconnecting = true;
                return;
            }

            if (_dataCounterReset < Envir.Now)
            {
                _dataCounterReset = Envir.Now.AddSeconds(5);
                _dataCounter = 0;
            }

            _dataCounter++;

            try
            {
                byte[] rawBytes = result.AsyncState as byte[];

                byte[] temp = _rawData;
                _rawData = new byte[dataRead + temp.Length];
                Buffer.BlockCopy(temp, 0, _rawData, 0, temp.Length);
                Buffer.BlockCopy(rawBytes, 0, _rawData, temp.Length, dataRead);

                Packet p;

                while ((p = Packet.ReceivePacket(_rawData, out _rawData)) != null)
                    _receiveList.Enqueue(p);
            }
            catch
            {
                Envir.UpdateIPBlock(IPAddress, TimeSpan.FromHours(24));

                MessageQueue.Enqueue($"{IPAddress} Disconnected, Invalid packet.");

                Disconnecting = true;
                return;
            }

            if (_dataCounter > Settings.MaxPacket)
            {
                Envir.UpdateIPBlock(IPAddress, TimeSpan.FromHours(24));

                List<string> packetList = new List<string>();

                while (_lastPackets.Count > 0)
                {
                    _lastPackets.TryDequeue(out Packet pkt);

                    Enum.TryParse<ClientPacketIds>((pkt?.Index ?? 0).ToString(), out ClientPacketIds cPacket);

                    packetList.Add(cPacket.ToString());
                }

                MessageQueue.Enqueue($"{IPAddress} Disconnected, Large amount of Packets. LastPackets: {String.Join(",", packetList.Distinct())}.");

                Disconnecting = true;
                return;
            }

            BeginReceive();
        }
        private void BeginSend(List<byte> data)
        {
            if (!Connected || data.Count == 0) return;

            //Interlocked.Add(ref Network.Sent, data.Count);

            try
            {
                _client.Client.BeginSend(data.ToArray(), 0, data.Count, SocketFlags.None, SendData, Disconnecting);
            }
            catch
            {
                Disconnecting = true;
            }
        }
        private void SendData(IAsyncResult result)
        {
            try
            {
                _client.Client.EndSend(result);
            }
            catch
            { }
        }
        
        public void Enqueue(Packet p)
        {
            if (p == null) return;
            if (_sendList != null && p != null)
                _sendList.Enqueue(p);

            if (!p.Observable) return;
            foreach (MirConnection c in Observers)
                c.Enqueue(p);
        }

        public void Process()
        {
            if (_client == null || !_client.Connected)
            {
                Disconnect(20);
                return;
            }

            while (!_receiveList.IsEmpty && !Disconnecting)
            {
                Packet p;
                if (!_receiveList.TryDequeue(out p)) continue;

                _lastPackets.Enqueue(p);

                TimeOutTime = Envir.Time + Settings.TimeOut;
                ProcessPacket(p);

                if (_receiveList == null)
                    return;
            }

            while (_retryList.Count > 0)
                _receiveList.Enqueue(_retryList.Dequeue());

            if (Envir.Time > TimeOutTime)
            {
                Disconnect(21);
                return;
            }

            if (_sendList == null || _sendList.Count <= 0) return;

            List<byte> data = new List<byte>();

            while (!_sendList.IsEmpty)
            {
                Packet p;
                if (!_sendList.TryDequeue(out p) || p == null) continue;
                data.AddRange(p.GetPacketBytes());
            }

            BeginSend(data);
        }
        private void ProcessPacket(Packet p)
        {
            if (p == null || Disconnecting) return;

            switch (p.Index)
            {
                case (short)ClientPacketIds.ClientVersion:
                    ClientVersion((C.ClientVersion) p);
                    break;
                case (short)ClientPacketIds.Disconnect:
                    Disconnect(22);
                    break;
                case (short)ClientPacketIds.KeepAlive: // Keep Alive
                    ClientKeepAlive((C.KeepAlive)p);
                    break;
                case (short)ClientPacketIds.NewAccount:
                    NewAccount((C.NewAccount) p);
                    break;
                case (short)ClientPacketIds.ChangePassword:
                    ChangePassword((C.ChangePassword) p);
                    break;
                case (short)ClientPacketIds.Login:
                    Login((C.Login) p);
                    break;
                case (short)ClientPacketIds.NewCharacter:
                    NewCharacter((C.NewCharacter) p);
                    break;
                case (short)ClientPacketIds.DeleteCharacter:
                    DeleteCharacter((C.DeleteCharacter) p);
                    break;
                case (short)ClientPacketIds.StartGame:
                    StartGame((C.StartGame) p);
                    break;
                case (short)ClientPacketIds.LogOut:
                    LogOut();
                    break;
                case (short)ClientPacketIds.Turn:
                    Turn((C.Turn) p);
                    break;
                case (short)ClientPacketIds.Walk:
                    Walk((C.Walk) p);
                    break;
                case (short)ClientPacketIds.Run:
                    Run((C.Run) p);
                    break;
                case (short)ClientPacketIds.Chat:
                    Chat((C.Chat) p);
                    break;
                case (short)ClientPacketIds.MoveItem:
                    MoveItem((C.MoveItem) p);
                    break;
                case (short)ClientPacketIds.StoreItem:
                    StoreItem((C.StoreItem) p);
                    break;
                case (short)ClientPacketIds.DepositRefineItem:
                    DepositRefineItem((C.DepositRefineItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveRefineItem:
                    RetrieveRefineItem((C.RetrieveRefineItem)p);
                    break;
                case (short)ClientPacketIds.RefineCancel:
                    RefineCancel((C.RefineCancel)p);
                    break;
                case (short)ClientPacketIds.RefineItem:
                    RefineItem((C.RefineItem)p);
                    break;
                case (short)ClientPacketIds.CheckRefine:
                    CheckRefine((C.CheckRefine)p);
                    break;
                case (short)ClientPacketIds.ReplaceWedRing:
                    ReplaceWedRing((C.ReplaceWedRing)p);
                    break;
                case (short)ClientPacketIds.DepositTradeItem:
                    DepositTradeItem((C.DepositTradeItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveTradeItem:
                    RetrieveTradeItem((C.RetrieveTradeItem)p);
                    break;
                case (short)ClientPacketIds.TakeBackItem:
                    TakeBackItem((C.TakeBackItem) p);
                    break;
                case (short)ClientPacketIds.MergeItem:
                    MergeItem((C.MergeItem) p);
                    break;
                case (short)ClientPacketIds.EquipItem:
                    EquipItem((C.EquipItem) p);
                    break;
                case (short)ClientPacketIds.RemoveItem:
                    RemoveItem((C.RemoveItem) p);
                    break;
                case (short)ClientPacketIds.RemoveSlotItem:
                    RemoveSlotItem((C.RemoveSlotItem)p);
                    break;
                case (short)ClientPacketIds.SplitItem:
                    SplitItem((C.SplitItem) p);
                    break;
                case (short)ClientPacketIds.UseItem:
                    UseItem((C.UseItem) p);
                    break;
                case (short)ClientPacketIds.DropItem:
                    DropItem((C.DropItem) p);
                    break;
                case (short)ClientPacketIds.TakeBackHeroItem:
                    TakeBackHeroItem((C.TakeBackHeroItem)p);
                    break;
                case (short)ClientPacketIds.TransferHeroItem:
                    TransferHeroItem((C.TransferHeroItem)p);
                    break;
                case (short)ClientPacketIds.DropGold:
                    DropGold((C.DropGold) p);
                    break;
                case (short)ClientPacketIds.PickUp:
                    PickUp();
                    break;
                case (short)ClientPacketIds.RequestMapInfo:
                    RequestMapInfo((C.RequestMapInfo)p);
                    break;
                case (short)ClientPacketIds.TeleportToNPC:
                    TeleportToNPC((C.TeleportToNPC)p);
                    break;
                case (short)ClientPacketIds.SearchMap:
                    SearchMap((C.SearchMap)p);
                    break;
                case (short)ClientPacketIds.Inspect:
                    Inspect((C.Inspect)p);
                    break;
                case (short)ClientPacketIds.Observe:
                    Observe((C.Observe)p);
                    break;
                case (short)ClientPacketIds.ChangeAMode:
                    ChangeAMode((C.ChangeAMode)p);
                    break;
                case (short)ClientPacketIds.ChangePMode:
                    ChangePMode((C.ChangePMode)p);
                    break;
                case (short)ClientPacketIds.ChangeTrade:
                    ChangeTrade((C.ChangeTrade)p);
                    break;
                case (short)ClientPacketIds.Attack:
                    Attack((C.Attack)p);
                    break;
                case (short)ClientPacketIds.RangeAttack:
                    RangeAttack((C.RangeAttack)p);
                    break;
                case (short)ClientPacketIds.Harvest:
                    Harvest((C.Harvest)p);
                    break;
                case (short)ClientPacketIds.CallNPC:
                    CallNPC((C.CallNPC)p);
                    break;
                case (short)ClientPacketIds.BuyItem:
                    BuyItem((C.BuyItem)p);
                    break;
                case (short)ClientPacketIds.CraftItem:
                    CraftItem((C.CraftItem)p);
                    break;
                case (short)ClientPacketIds.SellItem:
                    SellItem((C.SellItem)p);
                    break;
                case (short)ClientPacketIds.RepairItem:
                    RepairItem((C.RepairItem)p);
                    break;
                case (short)ClientPacketIds.BuyItemBack:
                    BuyItemBack((C.BuyItemBack)p);
                    break;
                case (short)ClientPacketIds.SRepairItem:
                    SRepairItem((C.SRepairItem)p);
                    break;
                case (short)ClientPacketIds.MagicKey:
                    MagicKey((C.MagicKey)p);
                    break;
                case (short)ClientPacketIds.Magic:
                    Magic((C.Magic)p);
                    break;
                case (short)ClientPacketIds.SwitchGroup:
                    SwitchGroup((C.SwitchGroup)p);
                    return;
                case (short)ClientPacketIds.AddMember:
                    AddMember((C.AddMember)p);
                    return;
                case (short)ClientPacketIds.DellMember:
                    DelMember((C.DelMember)p);
                    return;
                case (short)ClientPacketIds.GroupInvite:
                    GroupInvite((C.GroupInvite)p);
                    return;
                case (short)ClientPacketIds.NewHero:
                    NewHero((C.NewHero)p);
                    break;
                case (short)ClientPacketIds.SetAutoPotValue:
                    SetAutoPotValue((C.SetAutoPotValue)p);
                    break;
                case (short)ClientPacketIds.SetAutoPotItem:
                    SetAutoPotItem((C.SetAutoPotItem)p);
                    break;
                case (short)ClientPacketIds.SetHeroBehaviour:
                    SetHeroBehaviour((C.SetHeroBehaviour)p);
                    break;
                case (short)ClientPacketIds.ChangeHero:
                    ChangeHero((C.ChangeHero)p);
                    break;
                case (short)ClientPacketIds.TownRevive:
                    TownRevive();
                    return;
                case (short)ClientPacketIds.SpellToggle:
                    SpellToggle((C.SpellToggle)p);
                    return;
                case (short)ClientPacketIds.ConsignItem:
                    ConsignItem((C.ConsignItem)p);
                    return;
                case (short)ClientPacketIds.MarketSearch:
                    MarketSearch((C.MarketSearch)p);
                    return;
                case (short)ClientPacketIds.MarketRefresh:
                    MarketRefresh();
                    return;
                case (short)ClientPacketIds.MarketPage:
                    MarketPage((C.MarketPage) p);
                    return;
                case (short)ClientPacketIds.MarketBuy:
                    MarketBuy((C.MarketBuy)p);
                    return;
                case (short)ClientPacketIds.MarketGetBack:
                    MarketGetBack((C.MarketGetBack)p);
                    return;
                case (short)ClientPacketIds.MarketSellNow:
                    MarketSellNow((C.MarketSellNow)p);
                    return;
                case (short)ClientPacketIds.RequestUserName:
                    RequestUserName((C.RequestUserName)p);
                    return;
                case (short)ClientPacketIds.RequestChatItem:
                    RequestChatItem((C.RequestChatItem)p);
                    return;
                case (short)ClientPacketIds.EditGuildMember:
                    EditGuildMember((C.EditGuildMember)p);
                    return;
                case (short)ClientPacketIds.EditGuildNotice:
                    EditGuildNotice((C.EditGuildNotice)p);
                    return;
                case (short)ClientPacketIds.GuildInvite:
                    GuildInvite((C.GuildInvite)p);
                    return;
                case (short)ClientPacketIds.RequestGuildInfo:
                    RequestGuildInfo((C.RequestGuildInfo)p);
                    return;
                case (short)ClientPacketIds.GuildNameReturn:
                    GuildNameReturn((C.GuildNameReturn)p);
                    return;
                case (short)ClientPacketIds.GuildStorageGoldChange:
                    GuildStorageGoldChange((C.GuildStorageGoldChange)p);
                    return;
                case (short)ClientPacketIds.GuildStorageItemChange:
                    GuildStorageItemChange((C.GuildStorageItemChange)p);
                    return;
                case (short)ClientPacketIds.GuildWarReturn:
                    GuildWarReturn((C.GuildWarReturn)p);
                    return;
                case (short)ClientPacketIds.MarriageRequest:
                    MarriageRequest((C.MarriageRequest)p);
                    return;
                case (short)ClientPacketIds.MarriageReply:
                    MarriageReply((C.MarriageReply)p);
                    return;
                case (short)ClientPacketIds.ChangeMarriage:
                    ChangeMarriage((C.ChangeMarriage)p);
                    return;
                case (short)ClientPacketIds.DivorceRequest:
                    DivorceRequest((C.DivorceRequest)p);
                    return;
                case (short)ClientPacketIds.DivorceReply:
                    DivorceReply((C.DivorceReply)p);
                    return;
                case (short)ClientPacketIds.AddMentor:
                    AddMentor((C.AddMentor)p);
                    return;
                case (short)ClientPacketIds.MentorReply:
                    MentorReply((C.MentorReply)p);
                    return;
                case (short)ClientPacketIds.AllowMentor:
                    AllowMentor((C.AllowMentor)p);
                    return;
                case (short)ClientPacketIds.CancelMentor:
                    CancelMentor((C.CancelMentor)p);
                    return;
                case (short)ClientPacketIds.TradeRequest:
                    TradeRequest((C.TradeRequest)p);
                    return;
                case (short)ClientPacketIds.TradeGold:
                    TradeGold((C.TradeGold)p);
                    return;
                case (short)ClientPacketIds.TradeReply:
                    TradeReply((C.TradeReply)p);
                    return;
                case (short)ClientPacketIds.TradeConfirm:
                    TradeConfirm((C.TradeConfirm)p);
                    return;
                case (short)ClientPacketIds.TradeCancel:
                    TradeCancel((C.TradeCancel)p);
                    return;
                case (short)ClientPacketIds.EquipSlotItem:
                    EquipSlotItem((C.EquipSlotItem)p);
                    break;
                case (short)ClientPacketIds.FishingCast:
                    FishingCast((C.FishingCast)p);
                    break;
                case (short)ClientPacketIds.FishingChangeAutocast:
                    FishingChangeAutocast((C.FishingChangeAutocast)p);
                    break;
                case (short)ClientPacketIds.AcceptQuest:
                    AcceptQuest((C.AcceptQuest)p);
                    break;
                case (short)ClientPacketIds.FinishQuest:
                    FinishQuest((C.FinishQuest)p);
                    break;
                case (short)ClientPacketIds.AbandonQuest:
                    AbandonQuest((C.AbandonQuest)p);
                    break;
                case (short)ClientPacketIds.ShareQuest:
                    ShareQuest((C.ShareQuest)p);
                    break;
                case (short)ClientPacketIds.AcceptReincarnation:
                    AcceptReincarnation();
                    break;
                case (short)ClientPacketIds.CancelReincarnation:
                     CancelReincarnation();
                    break;
                case (short)ClientPacketIds.CombineItem:
                    CombineItem((C.CombineItem)p);
                    break;
                case (short)ClientPacketIds.AwakeningNeedMaterials:
                    AwakeningNeedMaterials((C.AwakeningNeedMaterials)p);
                    break;
                case (short)ClientPacketIds.AwakeningLockedItem:
                    Enqueue(new S.AwakeningLockedItem { UniqueID = ((C.AwakeningLockedItem)p).UniqueID, Locked = ((C.AwakeningLockedItem)p).Locked });
                    break;
                case (short)ClientPacketIds.Awakening:
                    Awakening((C.Awakening)p);
                    break;
                case (short)ClientPacketIds.DisassembleItem:
                    DisassembleItem((C.DisassembleItem)p);
                    break;
                case (short)ClientPacketIds.DowngradeAwakening:
                    DowngradeAwakening((C.DowngradeAwakening)p);
                    break;
                case (short)ClientPacketIds.ResetAddedItem:
                    ResetAddedItem((C.ResetAddedItem)p);
                    break;
                case (short)ClientPacketIds.SendMail:
                    SendMail((C.SendMail)p);
                    break;
                case (short)ClientPacketIds.ReadMail:
                    ReadMail((C.ReadMail)p);
                    break;
                case (short)ClientPacketIds.CollectParcel:
                    CollectParcel((C.CollectParcel)p);
                    break;
                case (short)ClientPacketIds.DeleteMail:
                    DeleteMail((C.DeleteMail)p);
                    break;
                case (short)ClientPacketIds.LockMail:
                    LockMail((C.LockMail)p);
                    break;
                case (short)ClientPacketIds.MailLockedItem:
                    Enqueue(new S.MailLockedItem { UniqueID = ((C.MailLockedItem)p).UniqueID, Locked = ((C.MailLockedItem)p).Locked });
                    break;
                case (short)ClientPacketIds.MailCost:
                    MailCost((C.MailCost)p);
                    break;
                case (short)ClientPacketIds.RequestIntelligentCreatureUpdates:
                    RequestIntelligentCreatureUpdates((C.RequestIntelligentCreatureUpdates)p);
                    break;
                case (short)ClientPacketIds.UpdateIntelligentCreature:
                    UpdateIntelligentCreature((C.UpdateIntelligentCreature)p);
                    break;
                case (short)ClientPacketIds.IntelligentCreaturePickup:
                    IntelligentCreaturePickup((C.IntelligentCreaturePickup)p);
                    break;
                case (short)ClientPacketIds.AddFriend:
                    AddFriend((C.AddFriend)p);
                    break;
                case (short)ClientPacketIds.RemoveFriend:
                    RemoveFriend((C.RemoveFriend)p);
                    break;
                case (short)ClientPacketIds.RefreshFriends:
                    {
                        if (Stage != GameStage.Game) return;
                        Player.GetFriends();
                        break;
                    }
                case (short)ClientPacketIds.AddMemo:
                    AddMemo((C.AddMemo)p);
                    break;
                case (short)ClientPacketIds.GuildBuffUpdate:
                    GuildBuffUpdate((C.GuildBuffUpdate)p);
                    break;
                case (short)ClientPacketIds.GameshopBuy:
                    GameshopBuy((C.GameshopBuy)p);
                    return;
                case (short)ClientPacketIds.NPCConfirmInput:
                    NPCConfirmInput((C.NPCConfirmInput)p);
                    break;
                case (short)ClientPacketIds.ReportIssue:
                    ReportIssue((C.ReportIssue)p);
                    break;
                case (short)ClientPacketIds.GetRanking:
                    GetRanking((C.GetRanking)p);
                    break;
                case (short)ClientPacketIds.Opendoor:
                    Opendoor((C.Opendoor)p);
                    break;
                case (short)ClientPacketIds.GetRentedItems:
                    GetRentedItems();
                    break;
                case (short)ClientPacketIds.ItemRentalRequest:
                    ItemRentalRequest();
                    break;
                case (short)ClientPacketIds.ItemRentalFee:
                    ItemRentalFee((C.ItemRentalFee)p);
                    break;
                case (short)ClientPacketIds.ItemRentalPeriod:
                    ItemRentalPeriod((C.ItemRentalPeriod)p);
                    break;
                case (short)ClientPacketIds.DepositRentalItem:
                    DepositRentalItem((C.DepositRentalItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveRentalItem:
                    RetrieveRentalItem((C.RetrieveRentalItem)p);
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

        public void SoftDisconnect(byte reason)
        {
            Stage = GameStage.Disconnected;
            TimeDisconnected = Envir.Time;
            
            lock (Envir.AccountLock)
            {
                if (Player != null)
                    Player.StopGame(reason);

                if (Account != null && Account.Connection == this)
                    Account.Connection = null;
            }

            Account = null;
        }
        public void Disconnect(byte reason)
        {
            if (!Connected) return;

            Connected = false;
            Stage = GameStage.Disconnected;
            TimeDisconnected = Envir.Time;

            lock (Envir.Connections)
                Envir.Connections.Remove(this);

            lock (Envir.AccountLock)
            {
                if (Player != null)
                    Player.StopGame(reason);

                if (Account != null && Account.Connection == this)
                    Account.Connection = null;
            }

            if (Observing != null)
                Observing.Observers.Remove(this);            

            Account = null;

            _sendList = null;
            _receiveList = null;
            _retryList = null;
            _rawData = null;

            if (_client != null) _client.Client.Dispose();
            _client = null;
        }
        public void SendDisconnect(byte reason)
        {
            if (!Connected)
            {
                Disconnecting = true;
                SoftDisconnect(reason);
                return;
            }
            
            Disconnecting = true;

            List<byte> data = new List<byte>();

            data.AddRange(new S.Disconnect { Reason = reason }.GetPacketBytes());

            BeginSend(data);
            SoftDisconnect(reason);
        }
        public void CleanObservers()
        {
            foreach (MirConnection c in Observers)
            {
                c.Stage = GameStage.Login;
                c.Enqueue(new S.ReturnToLogin());
            }
        }

        private void ClientVersion(C.ClientVersion p)
        {
            if (Stage != GameStage.None) return;

            if (Settings.CheckVersion)
            {
                bool match = false;

                foreach (var hash in Settings.VersionHashes)
                {
                    if (Functions.CompareBytes(hash, p.VersionHash))
                    {
                        match = true;
                        break;
                    }
                }

                if (!match)
                {
                    Disconnecting = true;

                    List<byte> data = new List<byte>();

                    data.AddRange(new S.ClientVersion { Result = 0 }.GetPacketBytes());

                    BeginSend(data);
                    SoftDisconnect(10);
                    MessageQueue.Enqueue(SessionID + ", Disconnnected - Wrong Client Version.");
                    return;
                }
            }

            MessageQueue.Enqueue(SessionID + ", " + IPAddress + ", Client version matched.");
            Enqueue(new S.ClientVersion { Result = 1 });

            Stage = GameStage.Login;
        }
        private void ClientKeepAlive(C.KeepAlive p)
        {
            Enqueue(new S.KeepAlive
            {
                Time = p.Time
            });
        }
        private void NewAccount(C.NewAccount p)
        {
            if (Stage != GameStage.Login) return;

            MessageQueue.Enqueue(SessionID + ", " + IPAddress + ", New account being created.");
            Envir.NewAccount(p, this);
        }
        private void ChangePassword(C.ChangePassword p)
        {
            if (Stage != GameStage.Login) return;

            MessageQueue.Enqueue(SessionID + ", " + IPAddress + ", Password being changed.");
            Envir.ChangePassword(p, this);
        }
        private void Login(C.Login p)
        {
            if (Stage != GameStage.Login) return;

            MessageQueue.Enqueue(SessionID + ", " + IPAddress + ", User logging in.");
            Envir.Login(p, this);
        }
        private void NewCharacter(C.NewCharacter p)
        {
            if (Stage != GameStage.Select) return;

            Envir.NewCharacter(p, this, Account.AdminAccount);
        }
        private void DeleteCharacter(C.DeleteCharacter p)
        {
            if (Stage != GameStage.Select) return;
            
            if (!Settings.AllowDeleteCharacter)
            {
                Enqueue(new S.DeleteCharacter { Result = 0 });
                return;
            }

            CharacterInfo temp = null;
            

            for (int i = 0; i < Account.Characters.Count; i++)
			{
			    if (Account.Characters[i].Index != p.CharacterIndex) continue;

			    temp = Account.Characters[i];
			    break;
			}

            if (temp == null)
            {
                Enqueue(new S.DeleteCharacter { Result = 1 });
                return;
            }

            temp.Deleted = true;
            temp.DeleteDate = Envir.Now;
            Envir.RemoveRank(temp);
            Enqueue(new S.DeleteCharacterSuccess { CharacterIndex = temp.Index });
        }
        private void StartGame(C.StartGame p)
        {
            if (Stage != GameStage.Select) return;

            if (!Settings.AllowStartGame && (Account == null || (Account != null && !Account.AdminAccount)))
            {
                Enqueue(new S.StartGame { Result = 0 });
                return;
            }

            if (Account == null)
            {
                Enqueue(new S.StartGame { Result = 1 });
                return;
            }


            CharacterInfo info = null;

            for (int i = 0; i < Account.Characters.Count; i++)
            {
                if (Account.Characters[i].Index != p.CharacterIndex) continue;

                info = Account.Characters[i];
                break;
            }
            if (info == null)
            {
                Enqueue(new S.StartGame { Result = 2 });
                return;
            }

            if (info.Banned)
            {
                if (info.ExpiryDate > Envir.Now)
                {
                    Enqueue(new S.StartGameBanned { Reason = info.BanReason, ExpiryDate = info.ExpiryDate });
                    return;
                }
                info.Banned = false;
            }
            info.BanReason = string.Empty;
            info.ExpiryDate = DateTime.MinValue;

            long delay = (long) (Envir.Now - info.LastLogoutDate).TotalMilliseconds;


            //if (delay < Settings.RelogDelay)
            //{
            //    Enqueue(new S.StartGameDelay { Milliseconds = Settings.RelogDelay - delay });
            //    return;
            //}

            Player = new PlayerObject(info, this);
            Player.StartGame();
        }

        public void LogOut()
        {
            if (Stage != GameStage.Game) return;

            if (Envir.Time < Player.LogTime)
            {
                Enqueue(new S.LogOutFailed());
                return;
            }

            Player.StopGame(23);

            Stage = GameStage.Select;
            Player = null;

            Enqueue(new S.LogOutSuccess { Characters = Account.GetSelectInfo() });
        }

        private void Turn(C.Turn p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.ActionTime > Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Turn(p.Direction);
        }
        private void Walk(C.Walk p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.ActionTime > Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Walk(p.Direction);
        }
        private void Run(C.Run p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.ActionTime > Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Run(p.Direction);
        }
        
        private void Chat(C.Chat p)
        {
            if (p.Message.Length > Globals.MaxChatLength)
            {
                SendDisconnect(2);
                return;
            }

            if (Stage != GameStage.Game) return;

            Player.Chat(p.Message, p.LinkedItems);
        }

        private void MoveItem(C.MoveItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.MoveItem(p.Grid, p.From, p.To);
        }
        private void StoreItem(C.StoreItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.StoreItem(p.From, p.To);
        }

        private void DepositRefineItem(C.DepositRefineItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DepositRefineItem(p.From, p.To);
        }

        private void RetrieveRefineItem(C.RetrieveRefineItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RetrieveRefineItem(p.From, p.To);
        }

        private void RefineCancel(C.RefineCancel p)
        {
            if (Stage != GameStage.Game) return;

            Player.RefineCancel();
        }

        private void RefineItem(C.RefineItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RefineItem(p.UniqueID);
        }

        private void CheckRefine(C.CheckRefine p)
        {
            if (Stage != GameStage.Game) return;

            Player.CheckRefine(p.UniqueID);
        }

        private void ReplaceWedRing(C.ReplaceWedRing p)
        {
            if (Stage != GameStage.Game) return;

            Player.ReplaceWeddingRing(p.UniqueID);
        }

        private void DepositTradeItem(C.DepositTradeItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DepositTradeItem(p.From, p.To);
        }
        
        private void RetrieveTradeItem(C.RetrieveTradeItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RetrieveTradeItem(p.From, p.To);
        }
        private void TakeBackItem(C.TakeBackItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.TakeBackItem(p.From, p.To);
        }
        private void MergeItem(C.MergeItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.MergeItem(p.GridFrom, p.GridTo, p.IDFrom, p.IDTo);
        }
        private void EquipItem(C.EquipItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.EquipItem(p.Grid, p.UniqueID, p.To);
        }
        private void RemoveItem(C.RemoveItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RemoveItem(p.Grid, p.UniqueID, p.To);
        }
        private void RemoveSlotItem(C.RemoveSlotItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RemoveSlotItem(p.Grid, p.UniqueID, p.To, p.GridTo, p.FromUniqueID);
        }
        private void SplitItem(C.SplitItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.SplitItem(p.Grid, p.UniqueID, p.Count);
        }
        private void UseItem(C.UseItem p)
        {
            if (Stage != GameStage.Game) return;

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    Player.UseItem(p.UniqueID);
                    break;
                case MirGridType.HeroInventory:
                    Player.HeroUseItem(p.UniqueID);
                    break;
            }            
        }
        private void DropItem(C.DropItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DropItem(p.UniqueID, p.Count, p.HeroInventory);
        }

        private void TakeBackHeroItem(C.TakeBackHeroItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.TakeBackHeroItem(p.From, p.To);
        }

        private void TransferHeroItem(C.TransferHeroItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.TransferHeroItem(p.From, p.To);
        }
        private void DropGold(C.DropGold p)
        {
            if (Stage != GameStage.Game) return;

            Player.DropGold(p.Amount);
        }
        private void PickUp()
        {
            if (Stage != GameStage.Game) return;

            Player.PickUp();
        }

        private void RequestMapInfo(C.RequestMapInfo p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestMapInfo(p.MapIndex);
        }

        private void TeleportToNPC(C.TeleportToNPC p)
        {
            if (Stage != GameStage.Game) return;

            Player.TeleportToNPC(p.ObjectID);
        }

        private void SearchMap(C.SearchMap p)
        {
            if (Stage != GameStage.Game) return;

            Player.SearchMap(p.Text);
        }
        private void Inspect(C.Inspect p)
        {
            if (Stage != GameStage.Game && Stage != GameStage.Observer) return;

            if (p.Ranking)
            {
                Envir.Inspect(this, (int)p.ObjectID);
            }
            else if (p.Hero)
            {
                Envir.InspectHero(this, (int)p.ObjectID);
            }
            else
            {
                Envir.Inspect(this, p.ObjectID);
            } 
        }
        private void Observe(C.Observe p)
        {
            if (Stage != GameStage.Game && Stage != GameStage.Observer) return;

            Envir.Observe(this, p.Name);
        }
        private void ChangeAMode(C.ChangeAMode p)
        {
            if (Stage != GameStage.Game) return;

            Player.AMode = p.Mode;

            Enqueue(new S.ChangeAMode {Mode = Player.AMode});
        }
        private void ChangePMode(C.ChangePMode p)
        {
            if (Stage != GameStage.Game) return;

            Player.PMode = p.Mode;

            Enqueue(new S.ChangePMode { Mode = Player.PMode });
        }
        private void ChangeTrade(C.ChangeTrade p)
        {
            if (Stage != GameStage.Game) return;

            Player.AllowTrade = p.AllowTrade;
        }
        private void Attack(C.Attack p)
        {
            if (Stage != GameStage.Game) return;

            if (!Player.Dead && (Player.ActionTime > Envir.Time || Player.AttackTime > Envir.Time))
                _retryList.Enqueue(p);
            else
                Player.Attack(p.Direction, p.Spell);
        }
        private void RangeAttack(C.RangeAttack p)
        {
            if (Stage != GameStage.Game) return;

            if (!Player.Dead && (Player.ActionTime > Envir.Time || Player.AttackTime > Envir.Time))
                _retryList.Enqueue(p);
            else
                Player.RangeAttack(p.Direction, p.TargetLocation, p.TargetID);
        }
        private void Harvest(C.Harvest p)
        {
            if (Stage != GameStage.Game) return;

            if (!Player.Dead && Player.ActionTime > Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Harvest(p.Direction);
        }

        private void CallNPC(C.CallNPC p)
        {
            if (Stage != GameStage.Game) return;

            if (p.Key.Length > 30) //No NPC Key should be that long.
            {
                SendDisconnect(2);
                return;
            }

            if (p.ObjectID == Envir.DefaultNPC.LoadedObjectID && Player.NPCObjectID == Envir.DefaultNPC.LoadedObjectID)
            {
                Player.CallDefaultNPC(p.Key);
                return;
            }

            if (p.ObjectID == uint.MaxValue)
            {
                Player.CallDefaultNPC(DefaultNPCType.Client, null);
                return;
            }

            Player.CallNPC(p.ObjectID, p.Key);
        }

        private void BuyItem(C.BuyItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.BuyItem(p.ItemIndex, p.Count, p.Type);
        }
        private void CraftItem(C.CraftItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.CraftItem(p.UniqueID, p.Count, p.Slots);
        }
        private void SellItem(C.SellItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.SellItem(p.UniqueID, p.Count);
        }
        private void RepairItem(C.RepairItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RepairItem(p.UniqueID);
        }
        private void BuyItemBack(C.BuyItemBack p)
        {
            if (Stage != GameStage.Game) return;

           // Player.BuyItemBack(p.UniqueID, p.Count);
        }
        private void SRepairItem(C.SRepairItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RepairItem(p.UniqueID, true);
        }
        private void MagicKey(C.MagicKey p)
        {
            if (Stage != GameStage.Game) return;

            HumanObject actor = Player;
            if (p.Key > 16 || p.OldKey > 16)
            {
                if (!Player.HeroSpawned || Player.Hero.Dead) return;
                actor = Player.Hero;
            }

            for (int i = 0; i < actor.Info.Magics.Count; i++)
            {
                UserMagic magic = actor.Info.Magics[i];
                if (magic.Spell != p.Spell)
                {
                    if (magic.Key == p.Key)
                        magic.Key = 0;
                    continue;
                }

                magic.Key = p.Key;
            }
        }
        private void Magic(C.Magic p)
        {
            if (Stage != GameStage.Game) return;

            HumanObject actor = Player;
            if (Player.HeroSpawned && p.ObjectID == Player.Hero.ObjectID)
                actor = Player.Hero;

            if (actor.Dead) return;

            if (!actor.Dead && (actor.ActionTime > Envir.Time || actor.SpellTime > Envir.Time))
                _retryList.Enqueue(p);
            else
                actor.BeginMagic(p.Spell, p.Direction, p.TargetID, p.Location, p.SpellTargetLock);
        }

        private void SwitchGroup(C.SwitchGroup p)
        {
            if (Stage != GameStage.Game) return;

            Player.SwitchGroup(p.AllowGroup);
        }
        private void AddMember(C.AddMember p)
        {
            if (Stage != GameStage.Game) return;

            Player.AddMember(p.Name);
        }
        private void DelMember(C.DelMember p)
        {
            if (Stage != GameStage.Game) return;

            Player.DelMember(p.Name);
        }
        private void GroupInvite(C.GroupInvite p)
        {
            if (Stage != GameStage.Game) return;

            Player.GroupInvite(p.AcceptInvite);
        }

        private void NewHero(C.NewHero p)
        {
            if (Stage != GameStage.Game) return;

            Player.NewHero(p);
        }

        private void SetAutoPotValue(C.SetAutoPotValue p)
        {
            if (Stage != GameStage.Game) return;

            Player.SetAutoPotValue(p.Stat, p.Value);
        }

        private void SetAutoPotItem(C.SetAutoPotItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.SetAutoPotItem(p.Grid, p.ItemIndex);
        }

        private void SetHeroBehaviour(C.SetHeroBehaviour p)
        {
            if (Stage != GameStage.Game) return;

            Player.SetHeroBehaviour(p.Behaviour);
        }

        private void ChangeHero(C.ChangeHero p)
        {
            if (Stage != GameStage.Game) return;

            Player.ChangeHero(p.ListIndex);
        }

        private void TownRevive()
        {
            if (Stage != GameStage.Game) return;

            Player.TownRevive();
        }

        private void SpellToggle(C.SpellToggle p)
        {
            if (Stage != GameStage.Game) return;

            if (p.canUse > SpellToggleState.None)
            {
                Player.SpellToggle(p.Spell, p.canUse);
                return;
            }
            if (Player.HeroSpawned)
                Player.Hero.SpellToggle(p.Spell, p.canUse);            
        }
        private void ConsignItem(C.ConsignItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.ConsignItem(p.UniqueID, p.Price, p.Type);
        }
        private void MarketSearch(C.MarketSearch p)
        {
            if (Stage != GameStage.Game) return;

            Player.UserMatch = p.Usermode;
            Player.MinShapes = p.MinShape;
            Player.MaxShapes = p.MaxShape;
            Player.MarketPanelType = p.MarketType;

            Player.MarketSearch(p.Match, p.Type);
        }
        private void MarketRefresh()
        {
            if (Stage != GameStage.Game) return;

            Player.MarketSearch(string.Empty, Player.MatchType);
        }

        private void MarketPage(C.MarketPage p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketPage(p.Page);
        }
        private void MarketBuy(C.MarketBuy p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketBuy(p.AuctionID, p.BidPrice);
        }
        private void MarketSellNow(C.MarketSellNow p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketSellNow(p.AuctionID);
        }

        private void MarketGetBack(C.MarketGetBack p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketGetBack(p.AuctionID);
        }
        private void RequestUserName(C.RequestUserName p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestUserName(p.UserID);
        }
        private void RequestChatItem(C.RequestChatItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestChatItem(p.ChatItemID);
        }
        private void EditGuildMember(C.EditGuildMember p)
        {
            if (Stage != GameStage.Game) return;
            Player.EditGuildMember(p.Name,p.RankName,p.RankIndex,p.ChangeType);
        }
        private void EditGuildNotice(C.EditGuildNotice p)
        {
            if (Stage != GameStage.Game) return;
            Player.EditGuildNotice(p.notice);
        }
        private void GuildInvite(C.GuildInvite p)
        {
            if (Stage != GameStage.Game) return;

            Player.GuildInvite(p.AcceptInvite);
        }
        private void RequestGuildInfo(C.RequestGuildInfo p)
        {
            if (Stage != GameStage.Game) return;
            Player.RequestGuildInfo(p.Type);
        }
        private void GuildNameReturn(C.GuildNameReturn p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildNameReturn(p.Name);
        }
        private void GuildStorageGoldChange(C.GuildStorageGoldChange p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildStorageGoldChange(p.Type, p.Amount);
        }
        private void GuildStorageItemChange(C.GuildStorageItemChange p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildStorageItemChange(p.Type, p.From, p.To);
        }
        private void GuildWarReturn(C.GuildWarReturn p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildWarReturn(p.Name);
        }


        private void MarriageRequest(C.MarriageRequest p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarriageRequest();
        }

        private void MarriageReply(C.MarriageReply p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarriageReply(p.AcceptInvite);
        }

        private void ChangeMarriage(C.ChangeMarriage p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.Info.Married == 0)
            {
                Player.AllowMarriage = !Player.AllowMarriage;
                if (Player.AllowMarriage)
                    Player.ReceiveChat("You're now allowing marriage requests.", ChatType.Hint);
                else
                    Player.ReceiveChat("You're now blocking marriage requests.", ChatType.Hint);
            }
            else
            {
                Player.AllowLoverRecall = !Player.AllowLoverRecall;
                if (Player.AllowLoverRecall)
                    Player.ReceiveChat("You're now allowing recall from lover.", ChatType.Hint);
                else
                    Player.ReceiveChat("You're now blocking recall from lover.", ChatType.Hint);
            }
        }

        private void DivorceRequest(C.DivorceRequest p)
        {
            if (Stage != GameStage.Game) return;

            Player.DivorceRequest();
        }

        private void DivorceReply(C.DivorceReply p)
        {
            if (Stage != GameStage.Game) return;

            Player.DivorceReply(p.AcceptInvite);
        }

        private void AddMentor(C.AddMentor p)
        {
            if (Stage != GameStage.Game) return;

            Player.AddMentor(p.Name);
        }

        private void MentorReply(C.MentorReply p)
        {
            if (Stage != GameStage.Game) return;

            Player.MentorReply(p.AcceptInvite);
        }

        private void AllowMentor(C.AllowMentor p)
        {
            if (Stage != GameStage.Game) return;

                Player.AllowMentor = !Player.AllowMentor;
                if (Player.AllowMentor)
                    Player.ReceiveChat(GameLanguage.AllowingMentorRequests, ChatType.Hint);
                else
                    Player.ReceiveChat(GameLanguage.BlockingMentorRequests, ChatType.Hint);
        }

        private void CancelMentor(C.CancelMentor p)
        {
            if (Stage != GameStage.Game) return;

            Player.MentorBreak(true);
        }

        private void TradeRequest(C.TradeRequest p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeRequest();
        }
        private void TradeGold(C.TradeGold p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeGold(p.Amount);
        }
        private void TradeReply(C.TradeReply p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeReply(p.AcceptInvite);
        }
        private void TradeConfirm(C.TradeConfirm p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeConfirm(p.Locked);
        }
        private void TradeCancel(C.TradeCancel p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeCancel();
        }
        private void EquipSlotItem(C.EquipSlotItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.EquipSlotItem(p.Grid, p.UniqueID, p.To, p.GridTo, p.ToUniqueID);
        }

        private void FishingCast(C.FishingCast p)
        {
            if (Stage != GameStage.Game) return;

            Player.FishingCast(p.CastOut, true);
        }

        private void FishingChangeAutocast(C.FishingChangeAutocast p)
        {
            if (Stage != GameStage.Game) return;

            Player.FishingChangeAutocast(p.AutoCast);
        }

        private void AcceptQuest(C.AcceptQuest p)
        {
            if (Stage != GameStage.Game) return;

            Player.AcceptQuest(p.QuestIndex); //p.NPCIndex,
        }

        private void FinishQuest(C.FinishQuest p)
        {
            if (Stage != GameStage.Game) return;

            Player.FinishQuest(p.QuestIndex, p.SelectedItemIndex);
        }

        private void AbandonQuest(C.AbandonQuest p)
        {
            if (Stage != GameStage.Game) return;

            Player.AbandonQuest(p.QuestIndex);
        }

        private void ShareQuest(C.ShareQuest p)
        {
            if (Stage != GameStage.Game) return;

            Player.ShareQuest(p.QuestIndex);
        }

        private void AcceptReincarnation()
        {
            if (Stage != GameStage.Game) return;

            if (Player.ReincarnationHost != null && Player.ReincarnationHost.ReincarnationReady)
            {
                Player.Revive(Player.Stats[Stat.HP] / 2, true);
                Player.ReincarnationHost = null;
                return;
            }

            Player.ReceiveChat("Reincarnation failed", ChatType.System);
        }

        private void CancelReincarnation()
        {
            if (Stage != GameStage.Game) return;
            Player.ReincarnationExpireTime = Envir.Time;

        }

        private void CombineItem(C.CombineItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.CombineItem(p.Grid, p.IDFrom, p.IDTo);
        }

        private void Awakening(C.Awakening p)
        {
            if (Stage != GameStage.Game) return;

            Player.Awakening(p.UniqueID, p.Type);
        }

        private void AwakeningNeedMaterials(C.AwakeningNeedMaterials p)
        {
            if (Stage != GameStage.Game) return;

            Player.AwakeningNeedMaterials(p.UniqueID, p.Type);
        }

        private void DisassembleItem(C.DisassembleItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DisassembleItem(p.UniqueID);
        }

        private void DowngradeAwakening(C.DowngradeAwakening p)
        {
            if (Stage != GameStage.Game) return;

            Player.DowngradeAwakening(p.UniqueID);
        }

        private void ResetAddedItem(C.ResetAddedItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.ResetAddedItem(p.UniqueID);
        }

        public void SendMail(C.SendMail p)
        {
            if (Stage != GameStage.Game) return;

            if (p.Gold > 0 || p.ItemsIdx.Length > 0)
            {
                Player.SendMail(p.Name, p.Message, p.Gold, p.ItemsIdx, p.Stamped);
            }
            else
            {
                Player.SendMail(p.Name, p.Message);
            }
        }

        public void ReadMail(C.ReadMail p)
        {
            if (Stage != GameStage.Game) return;

            Player.ReadMail(p.MailID);
        }

        public void CollectParcel(C.CollectParcel p)
        {
            if (Stage != GameStage.Game) return;

            Player.CollectMail(p.MailID);
        }

        public void DeleteMail(C.DeleteMail p)
        {
            if (Stage != GameStage.Game) return;

            Player.DeleteMail(p.MailID);
        }

        public void LockMail(C.LockMail p)
        {
            if (Stage != GameStage.Game) return;

            Player.LockMail(p.MailID, p.Lock);
        }

        public void MailCost(C.MailCost p)
        {
            if (Stage != GameStage.Game) return;

            uint cost = Player.GetMailCost(p.ItemsIdx, p.Gold, p.Stamped);

            Enqueue(new S.MailCost { Cost = cost });
        }

        private void RequestIntelligentCreatureUpdates(C.RequestIntelligentCreatureUpdates p)
        {
            if (Stage != GameStage.Game) return;

            Player.SendIntelligentCreatureUpdates = p.Update;
        }

        private void UpdateIntelligentCreature(C.UpdateIntelligentCreature p)
        {
            if (Stage != GameStage.Game) return;

            ClientIntelligentCreature petUpdate = p.Creature;
            if (petUpdate == null) return;

            if (p.ReleaseMe)
            {
                Player.ReleaseIntelligentCreature(petUpdate.PetType);
                return;
            }
            else if (p.SummonMe)
            {
                Player.SummonIntelligentCreature(petUpdate.PetType);
                return;
            }
            else if (p.UnSummonMe)
            {
                Player.UnSummonIntelligentCreature(petUpdate.PetType);
                return;
            }
            else
            {
                //Update the creature info
                for (int i = 0; i < Player.Info.IntelligentCreatures.Count; i++)
                {
                    if (Player.Info.IntelligentCreatures[i].PetType == petUpdate.PetType)
                    {
                        var reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinCharacterNameLength + "," + Globals.MaxCharacterNameLength + "}$");

                        if (reg.IsMatch(petUpdate.CustomName))
                        {
                            Player.Info.IntelligentCreatures[i].CustomName = petUpdate.CustomName;
                        }

                        Player.Info.IntelligentCreatures[i].SlotIndex = petUpdate.SlotIndex;
                        Player.Info.IntelligentCreatures[i].Filter = petUpdate.Filter;
                        Player.Info.IntelligentCreatures[i].petMode = petUpdate.petMode;
                    }
                    else continue;
                }

                if (Player.CreatureSummoned)
                {
                    if (Player.SummonedCreatureType == petUpdate.PetType)
                        Player.UpdateSummonedCreature(petUpdate.PetType);
                }
            }
        }

        private void IntelligentCreaturePickup(C.IntelligentCreaturePickup p)
        {
            if (Stage != GameStage.Game) return;

            Player.IntelligentCreaturePickup(p.MouseMode, p.Location);
        }

        private void AddFriend(C.AddFriend p)
        {
            if (Stage != GameStage.Game) return;

            Player.AddFriend(p.Name, p.Blocked);
        }

        private void RemoveFriend(C.RemoveFriend p)
        {
            if (Stage != GameStage.Game) return;

            Player.RemoveFriend(p.CharacterIndex);
        }

        private void AddMemo(C.AddMemo p)
        {
            if (Stage != GameStage.Game) return;

            Player.AddMemo(p.CharacterIndex, p.Memo);
        }
        private void GuildBuffUpdate(C.GuildBuffUpdate p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildBuffUpdate(p.Action,p.Id);
        }
        private void GameshopBuy(C.GameshopBuy p)
        {
            if (Stage != GameStage.Game) return;
            Player.GameshopBuy(p.GIndex, p.Quantity);
        }

        private void NPCConfirmInput(C.NPCConfirmInput p)
        {
            if (Stage != GameStage.Game) return;

            Player.NPCData["NPCInputStr"] = p.Value;

            Player.CallNPC(Player.NPCObjectID, p.PageName);
        }

        public List<byte[]> Image = new List<byte[]>();
        
        private void ReportIssue(C.ReportIssue p)
        {
            if (Stage != GameStage.Game) return;

            return;

            // Image.Add(p.Image);

            // if (p.ImageChunk >= p.ImageSize)
            // {
            //     System.Drawing.Image image = Functions.ByteArrayToImage(Functions.CombineArray(Image));
            //     image.Save("Reported-" + Player.Name + "-" + DateTime.Now.ToString("yyMMddHHmmss") + ".jpg");
            //     Image.Clear();
            // }
        }
        private void GetRanking(C.GetRanking p)
        {
            if (Stage != GameStage.Game && Stage != GameStage.Observer) return;
            Envir.GetRanking(this, p.RankType, p.RankIndex, p.OnlineOnly);
        }

        private void Opendoor(C.Opendoor p)
        {
            if (Stage != GameStage.Game) return;
            Player.Opendoor(p.DoorIndex);
        }

        private void GetRentedItems()
        {
            if (Stage != GameStage.Game)
                return;

            Player.GetRentedItems();
        }

        private void ItemRentalRequest()
        {
            if (Stage != GameStage.Game)
                return;

            Player.ItemRentalRequest();
        }

        private void ItemRentalFee(C.ItemRentalFee p)
        {
            if (Stage != GameStage.Game)
                return;

            Player.SetItemRentalFee(p.Amount);
        }

        private void ItemRentalPeriod(C.ItemRentalPeriod p)
        {
            if (Stage != GameStage.Game)
                return;

            Player.SetItemRentalPeriodLength(p.Days);
        }

        private void DepositRentalItem(C.DepositRentalItem p)
        {
            if (Stage != GameStage.Game)
                return;

            Player.DepositRentalItem(p.From, p.To);
        }

        private void RetrieveRentalItem(C.RetrieveRentalItem p)
        {
            if (Stage != GameStage.Game)
                return;

            Player.RetrieveRentalItem(p.From, p.To);
        }

        private void CancelItemRental()
        {
            if (Stage != GameStage.Game)
                return;

            Player.CancelItemRental();
        }

        private void ItemRentalLockFee()
        {
            if (Stage != GameStage.Game)
                return;

            Player.ItemRentalLockFee();
        }

        private void ItemRentalLockItem()
        {
            if (Stage != GameStage.Game)
                return;

            Player.ItemRentalLockItem();
        }

        private void ConfirmItemRental()
        {
            if (Stage != GameStage.Game)
                return;

            Player.ConfirmItemRental();
        }

        public void CheckItemInfo(ItemInfo info, bool dontLoop = false)
        {
            if ((dontLoop == false) && (info.ClassBased | info.LevelBased)) //send all potential data so client can display it
            {
                for (int i = 0; i < Envir.ItemInfoList.Count; i++)
                {
                    if ((Envir.ItemInfoList[i] != info) && (Envir.ItemInfoList[i].Name.StartsWith(info.Name)))
                        CheckItemInfo(Envir.ItemInfoList[i], true);
                }
            }

            if (SentItemInfo.Contains(info)) return;
            Enqueue(new S.NewItemInfo { Info = info });
            SentItemInfo.Add(info);
        }
        public void CheckItem(UserItem item)
        {
            CheckItemInfo(item.Info);

            for (int i = 0; i < item.Slots.Length; i++)
            {
                if (item.Slots[i] == null) continue;

                CheckItemInfo(item.Slots[i].Info);
            }

            CheckHeroInfo(item);
        }
        private void CheckHeroInfo(UserItem item)
        {
            if (item.AddedStats[Stat.Hero] == 0) return;
            if (SentHeroInfo.Contains(item.UniqueID)) return;

            HeroInfo heroInfo = Envir.GetHeroInfo(item.AddedStats[Stat.Hero]);
            if (heroInfo == null) return;

            Enqueue(new S.NewHeroInfo { Info = heroInfo.ClientInformation });
            SentHeroInfo.Add(item.UniqueID);
        }
    }
}
