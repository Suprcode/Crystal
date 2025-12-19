using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using SlimDX;
using SlimDX.Direct3D9;
using Font = System.Drawing.Font;
using S = ServerPackets;
using C = ClientPackets;
using Effect = Client.MirObjects.Effect;
using Client.MirScenes.Dialogs;
using Client.Utils;
using Client.MirGraphics.Particles;

namespace Client.MirScenes
{
    public sealed class GameScene : MirScene
    {
        public static GameScene Scene;
        public static bool Observing;
        public static bool AllowObserve;

        public static UserObject User
        {
            get { return MapObject.User; }
            set { MapObject.User = value; }
        }

        public static UserHeroObject Hero
        {
            get { return MapObject.Hero; }
            set { MapObject.Hero = value; }
        }
        public static HeroObject HeroObject
        {
            get { return MapObject.HeroObject; }
            set { MapObject.HeroObject = value; }
        }

        public static long MoveTime, AttackTime, NextRunTime, LogTime, LastRunTime, ChangePModeTime, ChangeAModeTime, HeroSpellTime, IntelligentCreaturePickupTime;
        public static bool CanMove, CanRun;

        private bool hasHero;
        public bool HasHero
        {
            get { return hasHero; }
            set
            {
                if (hasHero == value) return;

                hasHero = value;
                MainDialog.HeroSummonButton.Visible = value;
            }
        }
        public HeroSpawnState HeroSpawnState;
        public List<ParticleEngine> ParticleEngines = new List<ParticleEngine>();
        public MapControl MapControl;
        public MainDialog MainDialog;
        public ChatDialog ChatDialog;
        public ChatControlBar ChatControl;
        public InventoryDialog InventoryDialog;
        public CharacterDialog CharacterDialog;
        public CharacterDialog HeroDialog;
        public HeroInventoryDialog HeroInventoryDialog;
        public HeroManageDialog HeroManageDialog;
        public CraftDialog CraftDialog;
        public StorageDialog StorageDialog;
        public BeltDialog BeltDialog;
        public MiniMapDialog MiniMapDialog;
        public InspectDialog InspectDialog;
        public OptionDialog OptionDialog;
        public MenuDialog MenuDialog;
        public NPCDialog NPCDialog;
        public NPCGoodsDialog NPCGoodsDialog;
        public NPCGoodsDialog NPCSubGoodsDialog;
        public NPCGoodsDialog NPCCraftGoodsDialog;
        public NPCDropDialog NPCDropDialog;
        public NPCAwakeDialog NPCAwakeDialog;
        public HelpDialog HelpDialog;
        public MountDialog MountDialog;
        public FishingDialog FishingDialog;
        public FishingStatusDialog FishingStatusDialog;
        public RefineDialog RefineDialog;

        public GroupDialog GroupDialog;
        public GuildDialog GuildDialog;
        public GuildTerritoryDialog GuildTerritoryDialog;
        public NewCharacterDialog NewHeroDialog;
        public HeroBeltDialog HeroBeltDialog;

        public BigMapDialog BigMapDialog;
        public TrustMerchantDialog TrustMerchantDialog;
        public CharacterDuraPanel CharacterDuraPanel;
        public DuraStatusDialog DuraStatusPanel;
        public TradeDialog TradeDialog;
        public GuestTradeDialog GuestTradeDialog;

        public HeroMenuPanel HeroMenuPanel;
        public HeroBehaviourPanel HeroBehaviourPanel;
        public SocketDialog SocketDialog;

        public List<SkillBarDialog> SkillBarDialogs = new List<SkillBarDialog>();
        public ChatOptionDialog ChatOptionDialog;
        public ChatNoticeDialog ChatNoticeDialog;

        public QuestListDialog QuestListDialog;
        public QuestDetailDialog QuestDetailDialog;
        public QuestDiaryDialog QuestLogDialog;
        public QuestTrackingDialog QuestTrackingDialog;

        public RankingDialog RankingDialog;

        public MailListDialog MailListDialog;
        public MailComposeLetterDialog MailComposeLetterDialog;
        public MailComposeParcelDialog MailComposeParcelDialog;
        public MailReadLetterDialog MailReadLetterDialog;
        public MailReadParcelDialog MailReadParcelDialog;

        public IntelligentCreatureDialog IntelligentCreatureDialog;
        public IntelligentCreatureOptionsDialog IntelligentCreatureOptionsDialog;
        public IntelligentCreatureOptionsGradeDialog IntelligentCreatureOptionsGradeDialog;

        public FriendDialog FriendDialog;
        public MemoDialog MemoDialog;
        public RelationshipDialog RelationshipDialog;
        public MentorDialog MentorDialog;
        public GameShopDialog GameShopDialog;

        public ReportDialog ReportDialog;

        public ItemRentingDialog ItemRentingDialog;
        public ItemRentDialog ItemRentDialog;
        public GuestItemRentingDialog GuestItemRentingDialog;
        public GuestItemRentDialog GuestItemRentDialog;
        public ItemRentalDialog ItemRentalDialog;

        public BuffDialog BuffsDialog;
        public BuffDialog HeroBuffsDialog;

        public KeyboardLayoutDialog KeyboardLayoutDialog;
        public NoticeDialog NoticeDialog;

        public TimerDialog TimerControl;
        public CompassDialog CompassControl;
        public RollDialog RollControl;


        public static List<ItemInfo> ItemInfoList = new List<ItemInfo>();
        public static List<UserId> UserIdList = new List<UserId>();
        public static List<UserItem> ChatItemList = new List<UserItem>();
        public static List<ClientQuestInfo> QuestInfoList = new List<ClientQuestInfo>();
        public static List<GameShopItem> GameShopInfoList = new List<GameShopItem>();
        public static List<ClientRecipeInfo> RecipeInfoList = new List<ClientRecipeInfo>();
        public static Dictionary<int, BigMapRecord> MapInfoList = new Dictionary<int, BigMapRecord>();
        public static List<ClientHeroInformation> HeroInfoList = new List<ClientHeroInformation>();
        public static ClientHeroInformation[] HeroStorage = new ClientHeroInformation[8];
        public static Dictionary<long, RankCharacterInfo> RankingList = new Dictionary<long, RankCharacterInfo>();
        public static int TeleportToNPCCost;
        public static int MaximumHeroCount;

        public static List<ClientMonsterInfo> MonsterInfoList = new List<ClientMonsterInfo>();
        public static List<ClientNPCInfo> NPCInfoList = new List<ClientNPCInfo>();
        private static readonly HashSet<int> RequestedItemInfo = new HashSet<int>();
        private static readonly HashSet<int> RequestedMonsterInfo = new HashSet<int>();
        private static readonly HashSet<int> RequestedNPCInfo = new HashSet<int>();

        public static void RequestItemInfo(int index)
        {
            if (index <= 0 || HasItemInfo(index) || !RequestedItemInfo.Add(index)) return;
            if (!Network.Connected) return;

            Network.Enqueue(new C.RequestItemInfo { ItemIndex = index });
        }

        public static void RequestMonsterInfo(int index)
        {
            if (index <= 0 || HasMonsterInfo(index) || !RequestedMonsterInfo.Add(index)) return;
            if (!Network.Connected) return;

            Network.Enqueue(new C.RequestMonsterInfo { MonsterIndex = index });
        }

        public static void RequestNPCInfo(int index)
        {
            if (index <= 0 || HasNPCInfo(index) || !RequestedNPCInfo.Add(index)) return;
            if (!Network.Connected) return;

            Network.Enqueue(new C.RequestNPCInfo { NPCIndex = index });
        }

        public static void OnItemInfoReceived(int index)
        {
            RequestedItemInfo.Remove(index);
        }

        public static void OnMonsterInfoReceived(int index)
        {
            RequestedMonsterInfo.Remove(index);
        }

        public static void OnNPCInfoReceived(int index)
        {
            RequestedNPCInfo.Remove(index);
        }

        private static bool HasItemInfo(int index)
        {
            for (int i = 0; i < ItemInfoList.Count; i++)
            {
                if (ItemInfoList[i].Index == index)
                    return true;
            }

            return false;
        }

        private static bool HasMonsterInfo(int index)
        {
            for (int i = 0; i < MonsterInfoList.Count; i++)
            {
                if (MonsterInfoList[i].Index == index)
                    return true;
            }

            return false;
        }

        private static bool HasNPCInfo(int index)
        {
            for (int i = 0; i < NPCInfoList.Count; i++)
            {
                if (NPCInfoList[i].Index == index)
                    return true;
            }

            return false;
        }

        public static UserItem[] Storage = new UserItem[80];
        public static UserItem[] GuildStorage = new UserItem[112];
        public static UserItem[] Refine = new UserItem[16];
        public static UserItem HoverItem, SelectedItem;
        public static MirItemCell SelectedCell;

        public static bool PickedUpGold;
        public MirControl ItemLabel, MailLabel, MemoLabel, GuildBuffLabel;
        public static long UseItemTime, PickUpTime, DropViewTime, TargetDeadTime;
        public static uint Gold, Credit;
        public static long InspectTime;
        public bool ShowReviveMessage;


        public bool NewMail;
        public int NewMailCounter = 0;


        public AttackMode AMode;
        public PetMode PMode;
        public LightSetting Lights;

        public static long NPCTime;
        public static uint NPCID;
        public static float NPCRate;
        public static PanelType NPCPanelType;
        public static uint DefaultNPCID;
        public static bool HideAddedStoreStats;

        public long ToggleTime;
        public static long SpellTime;

        public MirLabel[] OutputLines = new MirLabel[10];
        public List<OutPutMessage> OutputMessages = new List<OutPutMessage>();

        public long OutputDelay;

        public GameScene()
        {
            MapControl.AutoRun = false;
            MapControl.AutoHit = false;

            Scene = this;
            RequestedItemInfo.Clear();
            RequestedMonsterInfo.Clear();
            RequestedNPCInfo.Clear();
            BackColour = Color.Transparent;
            MoveTime = CMain.Time;

            KeyDown += GameScene_KeyDown;

            MainDialog = new MainDialog { Parent = this };
            ChatDialog = new ChatDialog { Parent = this };
            ChatControl = new ChatControlBar { Parent = this };
            InventoryDialog = new InventoryDialog { Parent = this };
            BeltDialog = new BeltDialog { Parent = this };
            StorageDialog = new StorageDialog { Parent = this, Visible = false };
            CraftDialog = new CraftDialog { Parent = this, Visible = false };
            MiniMapDialog = new MiniMapDialog { Parent = this };
            InspectDialog = new InspectDialog { Parent = this, Visible = false };
            OptionDialog = new OptionDialog { Parent = this, Visible = false };
            MenuDialog = new MenuDialog { Parent = this, Visible = false };
            NPCDialog = new NPCDialog { Parent = this, Visible = false };
            NPCGoodsDialog = new NPCGoodsDialog(PanelType.Buy) { Parent = this, Visible = false };
            NPCSubGoodsDialog = new NPCGoodsDialog(PanelType.BuySub) { Parent = this, Visible = false };
            NPCCraftGoodsDialog = new NPCGoodsDialog(PanelType.Craft) { Parent = this, Visible = false };
            NPCDropDialog = new NPCDropDialog { Parent = this, Visible = false };
            NPCAwakeDialog = new NPCAwakeDialog { Parent = this, Visible = false, Location = new Point(0, GameScene.Scene.NPCDialog.Size.Height) };

            HelpDialog = new HelpDialog { Parent = this, Visible = false };
            KeyboardLayoutDialog = new KeyboardLayoutDialog { Parent = this, Visible = false };
            NoticeDialog = new NoticeDialog { Parent = this, Visible = false };

            MountDialog = new MountDialog { Parent = this, Visible = false };
            FishingDialog = new FishingDialog { Parent = this, Visible = false };
            FishingStatusDialog = new FishingStatusDialog { Parent = this, Visible = false };

            GroupDialog = new GroupDialog { Parent = this, Visible = false };
            GuildDialog = new GuildDialog { Parent = this, Visible = false };
            GuildTerritoryDialog = new GuildTerritoryDialog { Parent = this, Visible = false };
            NewHeroDialog = new NewCharacterDialog { Parent = this, Visible = false };
            NewHeroDialog.TitleLabel.Index = 847;
            NewHeroDialog.TitleLabel.Location = new Point(246, 11);
            NewHeroDialog.OnCreateCharacter += (o, e) =>
            {
                Network.Enqueue(new C.NewHero
                {
                    Name = NewHeroDialog.NameTextBox.Text,
                    Class = NewHeroDialog.Class,
                    Gender = NewHeroDialog.Gender
                });
            };

            HeroMenuPanel = new HeroMenuPanel(this) { Visible = false };
            HeroBehaviourPanel = new HeroBehaviourPanel { Parent = this, Visible = false };
            HeroManageDialog = new HeroManageDialog { Parent = this, Visible = false };

            BigMapDialog = new BigMapDialog { Parent = this, Visible = false };
            TrustMerchantDialog = new TrustMerchantDialog { Parent = this, Visible = false };
            CharacterDuraPanel = new CharacterDuraPanel { Parent = this, Visible = false };
            DuraStatusPanel = new DuraStatusDialog { Parent = this, Visible = true };
            TradeDialog = new TradeDialog { Parent = this, Visible = false };
            GuestTradeDialog = new GuestTradeDialog { Parent = this, Visible = false };

            SocketDialog = new SocketDialog { Parent = this, Visible = false };

            SkillBarDialog Bar1 = new SkillBarDialog { Parent = this, Visible = false, BarIndex = 0 };
            SkillBarDialogs.Add(Bar1);
            SkillBarDialog Bar2 = new SkillBarDialog { Parent = this, Visible = false, BarIndex = 1 };
            SkillBarDialogs.Add(Bar2);
            ChatOptionDialog = new ChatOptionDialog { Parent = this, Visible = false };
            ChatNoticeDialog = new ChatNoticeDialog { Parent = this, Visible = false };

            QuestListDialog = new QuestListDialog { Parent = this, Visible = false };
            QuestDetailDialog = new QuestDetailDialog { Parent = this, Visible = false };
            QuestTrackingDialog = new QuestTrackingDialog { Parent = this, Visible = false };
            QuestLogDialog = new QuestDiaryDialog { Parent = this, Visible = false };

            RankingDialog = new RankingDialog { Parent = this, Visible = false };

            MailListDialog = new MailListDialog { Parent = this, Visible = false };
            MailComposeLetterDialog = new MailComposeLetterDialog { Parent = this, Visible = false };
            MailComposeParcelDialog = new MailComposeParcelDialog { Parent = this, Visible = false };
            MailReadLetterDialog = new MailReadLetterDialog { Parent = this, Visible = false };
            MailReadParcelDialog = new MailReadParcelDialog { Parent = this, Visible = false };

            IntelligentCreatureDialog = new IntelligentCreatureDialog { Parent = this, Visible = false };
            IntelligentCreatureOptionsDialog = new IntelligentCreatureOptionsDialog { Parent = this, Visible = false };
            IntelligentCreatureOptionsGradeDialog = new IntelligentCreatureOptionsGradeDialog { Parent = this, Visible = false };

            RefineDialog = new RefineDialog { Parent = this, Visible = false };
            RelationshipDialog = new RelationshipDialog { Parent = this, Visible = false };
            FriendDialog = new FriendDialog { Parent = this, Visible = false };
            MemoDialog = new MemoDialog { Parent = this, Visible = false };
            MentorDialog = new MentorDialog { Parent = this, Visible = false };
            GameShopDialog = new GameShopDialog { Parent = this, Visible = false };
            ReportDialog = new ReportDialog { Parent = this, Visible = false };

            ItemRentingDialog = new ItemRentingDialog { Parent = this, Visible = false };
            ItemRentDialog = new ItemRentDialog { Parent = this, Visible = false };
            GuestItemRentingDialog = new GuestItemRentingDialog { Parent = this, Visible = false };
            GuestItemRentDialog = new GuestItemRentDialog { Parent = this, Visible = false };
            ItemRentalDialog = new ItemRentalDialog { Parent = this, Visible = false };

            BuffsDialog = new BuffDialog
            {
                Parent = this,
                Visible = true,
                GetExpandedParameter = () => { return Settings.ExpandedBuffWindow; },
                SetExpandedParameter = (value) => { Settings.ExpandedBuffWindow = value; }
            };

            KeyboardLayoutDialog = new KeyboardLayoutDialog { Parent = this, Visible = false };

            TimerControl = new TimerDialog { Parent = this, Visible = false };
            CompassControl = new CompassDialog { Parent = this, Visible = false };
            RollControl = new RollDialog { Parent = this, Visible = false };

            for (int i = 0; i < OutputLines.Length; i++)
                OutputLines[i] = new MirLabel
                {
                    AutoSize = true,
                    BackColour = Color.Transparent,
                    Font = new Font(Settings.FontName, 10F),
                    ForeColour = Color.LimeGreen,
                    Location = new Point(20, 25 + i * 13),
                    OutLine = true,
                };

            if (MapInfoList.Count > 0)
                RecreateBigMapButtons();
        }

        private void UpdateMouseCursor()
        {
            if (!Settings.UseMouseCursors) return;

            if (GameScene.HoverItem != null)
            {
                if (GameScene.SelectedCell != null && GameScene.SelectedCell.Item != null && GameScene.SelectedCell.Item.Info.Type == ItemType.Gem && CMain.Ctrl)
                {
                    CMain.SetMouseCursor(MouseCursor.Upgrade);
                }
                else
                {
                    CMain.SetMouseCursor(MouseCursor.Default);
                }
            }
            else if (MapObject.MouseObject != null)
            {
                switch (MapObject.MouseObject.Race)
                {
                    case ObjectType.Monster:
                        CMain.SetMouseCursor(MouseCursor.Attack);
                        break;
                    case ObjectType.Merchant:
                        CMain.SetMouseCursor(MouseCursor.NPCTalk);
                        break;
                    case ObjectType.Player:
                        if (CMain.Shift)
                        {
                            CMain.SetMouseCursor(MouseCursor.AttackRed);
                        }
                        else
                        {
                            CMain.SetMouseCursor(MouseCursor.Default);
                        }
                        break;
                    default:
                        CMain.SetMouseCursor(MouseCursor.Default);
                        break;
                }
            }
            else
            {
                CMain.SetMouseCursor(MouseCursor.Default);
            }

        }

        public void OutputMessage(string message, OutputMessageType type = OutputMessageType.Normal)
        {
            OutputMessages.Add(new OutPutMessage { Message = message, ExpireTime = CMain.Time + 5000, Type = type });
            if (OutputMessages.Count > 10)
                OutputMessages.RemoveAt(0);
        }

        private void ProcessOuput()
        {
            for (int i = 0; i < OutputMessages.Count; i++)
            {
                if (CMain.Time >= OutputMessages[i].ExpireTime)
                    OutputMessages.RemoveAt(i);
            }

            for (int i = 0; i < OutputLines.Length; i++)
            {
                if (OutputMessages.Count > i)
                {
                    Color color;
                    switch (OutputMessages[i].Type)
                    {
                        case OutputMessageType.Quest:
                            color = Color.Gold;
                            break;
                        case OutputMessageType.Guild:
                            color = Color.DeepPink;
                            break;
                        default:
                            color = Color.LimeGreen;
                            break;
                    }

                    OutputLines[i].Text = OutputMessages[i].Message;
                    OutputLines[i].ForeColour = color;
                    OutputLines[i].Visible = true;
                }
                else
                {
                    OutputLines[i].Text = string.Empty;
                    OutputLines[i].Visible = false;
                }
            }
        }
        private void GameScene_KeyDown(object sender, KeyEventArgs e)
        {
            if (GameScene.Scene.KeyboardLayoutDialog.WaitingForBind != null)
            {
                GameScene.Scene.KeyboardLayoutDialog.CheckNewInput(e);
                return;
            }

            foreach (KeyBind KeyCheck in CMain.InputKeys.Keylist)
            {
                if (KeyCheck.Key == Keys.None)
                    continue;
                if (KeyCheck.Key != e.KeyCode)
                    continue;
                if (KeyCheck.Key == Keys.Insert && e.KeyCode == Keys.Insert && e.Modifiers == Keys.Control)
                {
                    //When Ctrl is held down, KeyData=Insert | Control, the system may mistakenly recognize it as Keys.Insert, requiring special judgment to be added.
                    continue;
                }
                if ((KeyCheck.RequireAlt != 2) && (KeyCheck.RequireAlt != (CMain.Alt ? 1 : 0)))
                    continue;
                if ((KeyCheck.RequireShift != 2) && (KeyCheck.RequireShift != (CMain.Shift ? 1 : 0)))
                    continue;
                if ((KeyCheck.RequireCtrl != 2) && (KeyCheck.RequireCtrl != (CMain.Ctrl ? 1 : 0)))
                    continue;
                if ((KeyCheck.RequireTilde != 2) && (KeyCheck.RequireTilde != (CMain.Tilde ? 1 : 0)))
                    continue;
                //now run the real code
                switch (KeyCheck.function)
                {
                    case KeybindOptions.Bar1Skill1: UseSpell(1); break;
                    case KeybindOptions.Bar1Skill2: UseSpell(2); break;
                    case KeybindOptions.Bar1Skill3: UseSpell(3); break;
                    case KeybindOptions.Bar1Skill4: UseSpell(4); break;
                    case KeybindOptions.Bar1Skill5: UseSpell(5); break;
                    case KeybindOptions.Bar1Skill6: UseSpell(6); break;
                    case KeybindOptions.Bar1Skill7: UseSpell(7); break;
                    case KeybindOptions.Bar1Skill8: UseSpell(8); break;
                    case KeybindOptions.Bar2Skill1: UseSpell(9); break;
                    case KeybindOptions.Bar2Skill2: UseSpell(10); break;
                    case KeybindOptions.Bar2Skill3: UseSpell(11); break;
                    case KeybindOptions.Bar2Skill4: UseSpell(12); break;
                    case KeybindOptions.Bar2Skill5: UseSpell(13); break;
                    case KeybindOptions.Bar2Skill6: UseSpell(14); break;
                    case KeybindOptions.Bar2Skill7: UseSpell(15); break;
                    case KeybindOptions.Bar2Skill8: UseSpell(16); break;
                    case KeybindOptions.HeroSkill1: UseSpell(17); break;
                    case KeybindOptions.HeroSkill2: UseSpell(18); break;
                    case KeybindOptions.HeroSkill3: UseSpell(19); break;
                    case KeybindOptions.HeroSkill4: UseSpell(20); break;
                    case KeybindOptions.HeroSkill5: UseSpell(21); break;
                    case KeybindOptions.HeroSkill6: UseSpell(22); break;
                    case KeybindOptions.HeroSkill7: UseSpell(23); break;
                    case KeybindOptions.HeroSkill8: UseSpell(24); break;
                    case KeybindOptions.Inventory:
                    case KeybindOptions.Inventory2:
                        if (!InventoryDialog.Visible) InventoryDialog.Show();
                        else InventoryDialog.Hide();
                        break;
                    case KeybindOptions.Equipment:
                    case KeybindOptions.Equipment2:
                        if (!CharacterDialog.Visible || !CharacterDialog.CharacterPage.Visible)
                        {
                            CharacterDialog.Show();
                            CharacterDialog.ShowCharacterPage();
                        }
                        else CharacterDialog.Hide();
                        break;
                    case KeybindOptions.Skills:
                    case KeybindOptions.Skills2:
                        if (!CharacterDialog.Visible || !CharacterDialog.SkillPage.Visible)
                        {
                            CharacterDialog.Show();
                            CharacterDialog.ShowSkillPage();
                        }
                        else CharacterDialog.Hide();
                        break;
                    case KeybindOptions.HeroInventory:
                        if (Hero == null)
                            break;
                        if (!HeroInventoryDialog.Visible) HeroInventoryDialog.Show();
                        else HeroInventoryDialog.Hide();
                        break;
                    case KeybindOptions.HeroEquipment:
                        if (Hero == null)
                            break;
                        if (!HeroDialog.Visible || !HeroDialog.CharacterPage.Visible)
                        {
                            HeroDialog.Show();
                            HeroDialog.ShowCharacterPage();
                        }
                        else HeroDialog.Hide();
                        break;
                    case KeybindOptions.HeroSkills:
                        if (Hero == null)
                            break;
                        if (!HeroDialog.Visible || !HeroDialog.SkillPage.Visible)
                        {
                            HeroDialog.Show();
                            HeroDialog.ShowSkillPage();
                        }
                        else HeroDialog.Hide();
                        break;
                    case KeybindOptions.Creature:
                        if (!IntelligentCreatureDialog.Visible) IntelligentCreatureDialog.Show();
                        else IntelligentCreatureDialog.Hide();
                        break;
                    case KeybindOptions.MountWindow:
                        if (!MountDialog.Visible) MountDialog.Show();
                        else MountDialog.Hide();
                        break;

                    case KeybindOptions.GameShop:
                        if (!GameShopDialog.Visible) GameShopDialog.Show();
                        else GameShopDialog.Hide();
                        break;
                    case KeybindOptions.Fishing:
                        if (!FishingDialog.Visible) FishingDialog.Show();
                        else FishingDialog.Hide();
                        break;
                    case KeybindOptions.Skillbar:
                        if (!Settings.SkillBar)
                            foreach (SkillBarDialog Bar in SkillBarDialogs)
                                Bar.Show();
                        else
                            foreach (SkillBarDialog Bar in SkillBarDialogs)
                                Bar.Hide();
                        break;
                    case KeybindOptions.Mount:
                        if (GameScene.Scene.MountDialog.CanRide())
                            GameScene.Scene.MountDialog.Ride();
                        break;
                    case KeybindOptions.Mentor:
                        if (!MentorDialog.Visible) MentorDialog.Show();
                        else MentorDialog.Hide();
                        break;
                    case KeybindOptions.Relationship:
                        if (!RelationshipDialog.Visible) RelationshipDialog.Show();
                        else RelationshipDialog.Hide();
                        break;
                    case KeybindOptions.Friends:
                        if (!FriendDialog.Visible) FriendDialog.Show();
                        else FriendDialog.Hide();
                        break;
                    case KeybindOptions.Guilds:
                        if (!GuildDialog.Visible) GuildDialog.Show();
                        else
                        {
                            GuildDialog.Hide();
                        }
                        break;

                    case KeybindOptions.Ranking:
                        if (!RankingDialog.Visible) RankingDialog.Show();
                        else RankingDialog.Hide();
                        break;
                    case KeybindOptions.Quests:
                        if (!QuestLogDialog.Visible) QuestLogDialog.Show();
                        else QuestLogDialog.Hide();
                        break;
                    case KeybindOptions.Exit:
                        QuitGame();
                        return;

                    case KeybindOptions.Closeall:
                        InventoryDialog.Hide();
                        CharacterDialog.Hide();
                        OptionDialog.Hide();
                        MenuDialog.Hide();
                        if (NPCDialog.Visible) NPCDialog.Hide();
                        HelpDialog.Hide();
                        KeyboardLayoutDialog.Hide();
                        RankingDialog.Hide();
                        IntelligentCreatureDialog.Hide();
                        IntelligentCreatureOptionsDialog.Hide();
                        IntelligentCreatureOptionsGradeDialog.Hide();
                        MountDialog.Hide();
                        FishingDialog.Hide();
                        FriendDialog.Hide();
                        RelationshipDialog.Hide();
                        MentorDialog.Hide();
                        GameShopDialog.Hide();
                        GroupDialog.Hide();
                        GuildDialog.Hide();
                        InspectDialog.Hide();
                        StorageDialog.Hide();
                        TrustMerchantDialog.Hide();
                        //CharacterDuraPanel.Hide();
                        QuestListDialog.Hide();
                        QuestDetailDialog.Hide();
                        QuestLogDialog.Hide();
                        NPCAwakeDialog.Hide();
                        RefineDialog.Hide();
                        BigMapDialog.Hide();
                        if (FishingStatusDialog.bEscExit) FishingStatusDialog.Cancel();
                        MailComposeLetterDialog.Hide();
                        MailComposeParcelDialog.Hide();
                        MailListDialog.Hide();
                        MailReadLetterDialog.Hide();
                        MailReadParcelDialog.Hide();
                        ItemRentalDialog.Hide();
                        NoticeDialog.Hide();
                        HeroInventoryDialog?.Hide();
                        HeroManageDialog?.Hide();
                        HeroDialog?.Hide();

                        GameScene.Scene.DisposeItemLabel();
                        break;
                    case KeybindOptions.Options:
                    case KeybindOptions.Options2:
                        if (!OptionDialog.Visible) OptionDialog.Show();
                        else OptionDialog.Hide();
                        break;
                    case KeybindOptions.Group:
                        if (!GroupDialog.Visible) GroupDialog.Show();
                        else GroupDialog.Hide();
                        break;
                    case KeybindOptions.Belt:
                        if (!BeltDialog.Visible) BeltDialog.Show();
                        else BeltDialog.Hide();
                        break;
                    case KeybindOptions.BeltFlip:
                        BeltDialog.Flip();
                        break;
                    case KeybindOptions.Pickup:
                        if (CMain.Time > PickUpTime)
                        {
                            PickUpTime = CMain.Time + 200;
                            Network.Enqueue(new C.PickUp());
                        }
                        break;
                    case KeybindOptions.Belt1:
                    case KeybindOptions.Belt1Alt:
                        BeltDialog.Grid[0].UseItem();
                        break;
                    case KeybindOptions.Belt2:
                    case KeybindOptions.Belt2Alt:
                        BeltDialog.Grid[1].UseItem();
                        break;
                    case KeybindOptions.Belt3:
                    case KeybindOptions.Belt3Alt:
                        BeltDialog.Grid[2].UseItem();
                        break;
                    case KeybindOptions.Belt4:
                    case KeybindOptions.Belt4Alt:
                        BeltDialog.Grid[3].UseItem();
                        break;
                    case KeybindOptions.Belt5:
                    case KeybindOptions.Belt5Alt:
                        BeltDialog.Grid[4].UseItem();
                        break;
                    case KeybindOptions.Belt6:
                    case KeybindOptions.Belt6Alt:
                        BeltDialog.Grid[5].UseItem();
                        break;
                    case KeybindOptions.Belt7:
                    case KeybindOptions.Belt7Alt:
                        HeroBeltDialog?.Grid[0].UseItem();
                        break;
                    case KeybindOptions.Belt8:
                    case KeybindOptions.Belt8Alt:
                        HeroBeltDialog?.Grid[1].UseItem();
                        break;
                    case KeybindOptions.Logout:
                        LogOut();
                        break;
                    case KeybindOptions.Minimap:
                        MiniMapDialog.Toggle();
                        break;
                    case KeybindOptions.Bigmap:
                        BigMapDialog.Toggle();
                        break;
                    case KeybindOptions.Trade:
                        Network.Enqueue(new C.TradeRequest());
                        break;
                    case KeybindOptions.Rental:
                        ItemRentalDialog.Toggle();
                        break;
                    case KeybindOptions.ChangePetmode:
                        ChangePetMode();
                        break;
                    case KeybindOptions.PetmodeBoth:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.Both });
                        return;
                    case KeybindOptions.PetmodeMoveonly:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.MoveOnly });
                        return;
                    case KeybindOptions.PetmodeAttackonly:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.AttackOnly });
                        return;
                    case KeybindOptions.PetmodeNone:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.None });
                        return;
                    case KeybindOptions.PetmodeFocusMasterTarget:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.FocusMasterTarget });
                        return;
                    case KeybindOptions.CreatureAutoPickup://semiauto!
                        if (CMain.Time > IntelligentCreaturePickupTime)
                        {
                            IntelligentCreaturePickupTime = CMain.Time + 200;
                            Network.Enqueue(new C.IntelligentCreaturePickup { MouseMode = false, Location = MapControl.MapLocation });
                        }
                        break;
                    case KeybindOptions.CreaturePickup:
                        if (CMain.Time > IntelligentCreaturePickupTime)
                        {
                            IntelligentCreaturePickupTime = CMain.Time + 200;
                            Network.Enqueue(new C.IntelligentCreaturePickup { MouseMode = true, Location = MapControl.MapLocation });
                        }
                        break;
                    case KeybindOptions.ChangeAttackmode:
                        ChangeAttackMode();
                        break;
                    case KeybindOptions.AttackmodePeace:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.Peace });
                        return;
                    case KeybindOptions.AttackmodeGroup:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.Group });
                        return;
                    case KeybindOptions.AttackmodeGuild:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.Guild });
                        return;
                    case KeybindOptions.AttackmodeEnemyguild:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.EnemyGuild });
                        return;
                    case KeybindOptions.AttackmodeRedbrown:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.RedBrown });
                        return;
                    case KeybindOptions.AttackmodeAll:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.All });
                        return;

                    case KeybindOptions.Help:
                        if (!HelpDialog.Visible) HelpDialog.Show();
                        else HelpDialog.Hide();
                        break;
                    case KeybindOptions.Keybind:
                        if (!KeyboardLayoutDialog.Visible) KeyboardLayoutDialog.Show();
                        else KeyboardLayoutDialog.Hide();
                        break;
                    case KeybindOptions.Autorun:
                        MapControl.AutoRun = !MapControl.AutoRun;
                        break;
                    case KeybindOptions.Cameramode:

                        if (!MainDialog.Visible)
                        {
                            MainDialog.Show();
                            ChatDialog.Show();
                            BeltDialog.Show();
                            ChatControl.Show();
                            MiniMapDialog.Show();
                            CharacterDuraPanel.Show();
                            DuraStatusPanel.Show();
                            BuffsDialog.Show();
                        }
                        else
                        {
                            MainDialog.Hide();
                            ChatDialog.Hide();
                            BeltDialog.Hide();
                            ChatControl.Hide();
                            MiniMapDialog.Hide();
                            CharacterDuraPanel.Hide();
                            DuraStatusPanel.Hide();
                            BuffsDialog.Hide();
                        }
                        break;
                    case KeybindOptions.DropView:
                        if (CMain.Time > DropViewTime)
                            DropViewTime = CMain.Time + 5000;
                        break;
                    case KeybindOptions.TargetDead:
                        if (CMain.Time > TargetDeadTime)
                            TargetDeadTime = CMain.Time + 5000;
                        break;
                    case KeybindOptions.AddGroupMember:
                        if (MapObject.MouseObject == null) break;
                        if (MapObject.MouseObject.Race != ObjectType.Player) break;

                        GameScene.Scene.GroupDialog.AddMember(MapObject.MouseObject.Name);
                        break;
                }
            }
        }

        public void ChangeSkillMode(bool? ctrl)
        {
            if (Settings.SkillMode || ctrl == true)
            {
                Settings.SkillMode = false;
                GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SkillModeCtrl), ChatType.Hint);
                GameScene.Scene.OptionDialog.ToggleSkillButtons(true);
            }
            else if (!Settings.SkillMode || ctrl == false)
            {
                Settings.SkillMode = true;
                GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SkillModeTilde), ChatType.Hint);
                GameScene.Scene.OptionDialog.ToggleSkillButtons(false);
            }
        }

        public void ChangePetMode()
        {
            if (CMain.Time > ChangePModeTime)
            {
                ChangePModeTime = CMain.Time + 500;

                switch (PMode)
                {
                    case PetMode.Both:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.MoveOnly });
                        return;
                    case PetMode.MoveOnly:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.AttackOnly });
                        return;
                    case PetMode.AttackOnly:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.None });
                        return;
                    case PetMode.None:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.FocusMasterTarget });
                        return;
                    case PetMode.FocusMasterTarget:
                        Network.Enqueue(new C.ChangePMode { Mode = PetMode.Both });
                        return;
                }
            }
        }

        public void ChangeAttackMode()
        {
            if (CMain.Time > ChangeAModeTime)
            {
                ChangeAModeTime = CMain.Time + 300;

                switch (AMode)
                {
                    case AttackMode.Peace:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.Group });
                        return;
                    case AttackMode.Group:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.Guild });
                        return;
                    case AttackMode.Guild:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.EnemyGuild });
                        return;
                    case AttackMode.EnemyGuild:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.RedBrown });
                        return;
                    case AttackMode.RedBrown:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.All });
                        return;
                    case AttackMode.All:
                        Network.Enqueue(new C.ChangeAMode { Mode = AttackMode.Peace });
                        return;
                }
            }
        }

        public void UseSpell(int key)
        {
            UserObject actor = User;
            if (key > 16)
            {
                if (HeroObject == null || CMain.Time < HeroSpellTime) return;

                actor = Hero;
                HeroSpellTime = CMain.Time + 200;
            }

            if (actor.Dead || actor.RidingMount || actor.Fishing) return;

            if (!actor.HasClassWeapon && actor.Weapon >= 0)
            {
                ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MustWearSuitableWeapon), ChatType.System);
                return;
            }

            if (CMain.Time < actor.BlizzardStopTime || CMain.Time < actor.ReincarnationStopTime) return;

            ClientMagic magic = null;

            for (int i = 0; i < actor.Magics.Count; i++)
            {
                if (actor.Magics[i].Key != key) continue;
                magic = actor.Magics[i];
                break;
            }

            if (magic == null) return;

            switch (magic.Spell)
            {
                case Spell.CounterAttack:
                    if ((CMain.Time < magic.CastTime + magic.Delay))
                    {
                        if (CMain.Time >= OutputDelay)
                        {
                            OutputDelay = CMain.Time + 1000;
                            Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.CannotCastForSeconds), magic.Name,
                                ((magic.CastTime + magic.Delay) - CMain.Time - 1) / 1000 + 1));
                        }

                        return;
                    }
                    magic.CastTime = CMain.Time;
                    break;
            }

            int cost;
            string prefix = actor == Hero ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Hero) : string.Empty;
            switch (magic.Spell)
            {
                case Spell.Fencing:
                case Spell.FatalSword:
                case Spell.MPEater:
                case Spell.Hemorrhage:
                case Spell.SpiritSword:
                case Spell.Slaying:
                case Spell.Focus:
                case Spell.Meditation:
                    return;
                case Spell.Thrusting:
                    if (CMain.Time < ToggleTime) return;
                    actor.Thrusting = !actor.Thrusting;
                    ChatDialog.ReceiveChat(
                        prefix + (actor.Thrusting ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UseThrusting) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DoNotUseThrusting)),
                        ChatType.Hint);
                    ToggleTime = CMain.Time + 1000;
                    SendSpellToggle(actor, magic.Spell, actor.Thrusting);
                    break;
                case Spell.HalfMoon:
                    if (CMain.Time < ToggleTime) return;
                    actor.HalfMoon = !actor.HalfMoon;
                    ChatDialog.ReceiveChat(
                        prefix + (actor.HalfMoon ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UseHalfMoon) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DoNotUseHalfMoon)),
                        ChatType.Hint);
                    ToggleTime = CMain.Time + 1000;
                    SendSpellToggle(actor, magic.Spell, actor.HalfMoon);
                    break;
                case Spell.CrossHalfMoon:
                    if (CMain.Time < ToggleTime) return;
                    actor.CrossHalfMoon = !actor.CrossHalfMoon;
                    ChatDialog.ReceiveChat(
                        prefix + (actor.CrossHalfMoon ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UseCrossHalfMoon) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DoNotUseCrossHalfMoon)),
                        ChatType.Hint);
                    ToggleTime = CMain.Time + 1000;
                    SendSpellToggle(actor, magic.Spell, actor.CrossHalfMoon);
                    break;
                case Spell.DoubleSlash:
                    if (CMain.Time < ToggleTime) return;
                    actor.DoubleSlash = !actor.DoubleSlash;
                    ChatDialog.ReceiveChat(
                        prefix + (actor.DoubleSlash ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UseDoubleSlash) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DoNotUseDoubleSlash)),
                        ChatType.Hint);
                    ToggleTime = CMain.Time + 1000;
                    SendSpellToggle(actor, magic.Spell, actor.DoubleSlash);
                    break;
                case Spell.TwinDrakeBlade:
                    if (CMain.Time < ToggleTime) return;
                    ToggleTime = CMain.Time + 500;

                    cost = magic.Level * magic.LevelCost + magic.BaseCost;
                    if (cost > actor.MP)
                    {
                        Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowMana));
                        return;
                    }
                    actor.TwinDrakeBlade = true;
                    SendSpellToggle(actor, magic.Spell, true);
                    if (actor == Hero)
                        HeroObject?.Effects.Add(new Effect(Libraries.Magic2, 210, 6, 500, HeroObject));
                    else
                        actor.Effects.Add(new Effect(Libraries.Magic2, 210, 6, 500, actor));
                    break;
                case Spell.FlamingSword:
                    if (CMain.Time < ToggleTime) return;
                    ToggleTime = CMain.Time + 500;

                    cost = magic.Level * magic.LevelCost + magic.BaseCost;
                    if (cost > actor.MP)
                    {
                        Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowMana));
                        return;
                    }
                    SendSpellToggle(actor, magic.Spell, true);
                    break;
                case Spell.CounterAttack:
                    cost = magic.Level * magic.LevelCost + magic.BaseCost;
                    if (cost > actor.MP)
                    {
                        Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowMana));
                        return;
                    }

                    SoundManager.PlaySound(20000 + (ushort)Spell.CounterAttack * 10);
                    SendSpellToggle(actor, magic.Spell, true);
                    break;
                case Spell.MentalState:
                    if (CMain.Time < ToggleTime) return;
                    ToggleTime = CMain.Time + 500;
                    SendSpellToggle(actor, magic.Spell, true);
                    break;
                default:
                    actor.NextMagic = magic;
                    actor.NextMagicLocation = MapControl.MapLocation;
                    actor.NextMagicObject = MapObject.MouseObject;
                    actor.NextMagicDirection = MapControl.MouseDirection();

                    if (actor == Hero)
                        MapControl.UseMagic(Hero.NextMagic, Hero);
                    break;
            }
        }
        private void SendSpellToggle(UserObject Actor, Spell Spell, bool CanUse)
        {
            if (Actor == User)
                Network.Enqueue(new C.SpellToggle { Spell = Spell, CanUse = CanUse });
            else
                Network.Enqueue(new C.SpellToggle { Spell = Spell });
        }
        public void QuitGame()
        {
            if (CMain.Time >= LogTime)
            {
                //If Last Combat < 10 CANCEL
                MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ExitTip), MirMessageBoxButtons.YesNo);
                messageBox.YesButton.Click += (o, e) => Program.Form.Close();
                messageBox.Show();
            }
            else
            {
                ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.CannotLeaveGame), (LogTime - CMain.Time) / 1000), ChatType.System);
            }
        }
        public void LogOut()
        {
            if (CMain.Time >= LogTime)
            {
                //If Last Combat < 10 CANCEL
                MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LogOutTip), MirMessageBoxButtons.YesNo);
                messageBox.YesButton.Click += (o, e) =>
                {
                    Network.Enqueue(new C.LogOut());
                    Enabled = false;
                };
                messageBox.Show();
            }
            else
            {
                ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.CannotLeaveGame), (LogTime - CMain.Time) / 1000), ChatType.System);
            }
        }

        protected internal override void DrawControl()
        {
            if (MapControl != null && !MapControl.IsDisposed)
                MapControl.DrawControl();
            base.DrawControl();


            if (PickedUpGold || (SelectedCell != null && SelectedCell.Item != null))
            {
                int image = PickedUpGold ? 116 : SelectedCell.Item.Image;
                Size imgSize = Libraries.Items.GetTrueSize(image);
                Point p = CMain.MPoint.Add(-imgSize.Width / 2, -imgSize.Height / 2);

                if (p.X + imgSize.Width >= Settings.ScreenWidth)
                    p.X = Settings.ScreenWidth - imgSize.Width;

                if (p.Y + imgSize.Height >= Settings.ScreenHeight)
                    p.Y = Settings.ScreenHeight - imgSize.Height;

                Libraries.Items.Draw(image, p.X, p.Y);
            }

            for (int i = 0; i < OutputLines.Length; i++)
                OutputLines[i].Draw();
        }
        public override void Process()
        {
            if (MapControl == null || User == null)
                return;

            if (CMain.Time >= MoveTime)
            {
                MoveTime = CMain.Time + 100; //Move Speed
                CanMove = true;
                MapControl.AnimationCount++;
                MapControl.TextureValid = false;
            }
            else
                CanMove = false;

            if (CMain.Time >= CMain.NextPing)
            {
                CMain.NextPing = CMain.Time + 60000;
                Network.Enqueue(new C.KeepAlive() { Time = CMain.Time });
            }

            TimerControl.Process();
            CompassControl.Process();
            RankingDialog.Process();

            MirItemCell cell = MouseControl as MirItemCell;

            if (cell != null && HoverItem != cell.Item && HoverItem != cell.ShadowItem)
            {
                DisposeItemLabel();
                HoverItem = null;
                CreateItemLabel(cell.Item);
            }

            if (ItemLabel != null && !ItemLabel.IsDisposed)
            {
                ItemLabel.BringToFront();

                int x = CMain.MPoint.X + 28, y = CMain.MPoint.Y + 28;
                if (x + ItemLabel.Size.Width > Settings.ScreenWidth)
                    x = Settings.ScreenWidth - ItemLabel.Size.Width;

                if (y + ItemLabel.Size.Height > Settings.ScreenHeight)
                    y = Settings.ScreenHeight - ItemLabel.Size.Height;
                ItemLabel.Location = new Point(x, y);
            }

            if (MailLabel != null && !MailLabel.IsDisposed)
            {
                MailLabel.BringToFront();

                int x = CMain.MPoint.X + 15, y = CMain.MPoint.Y;
                if (x + MailLabel.Size.Width > Settings.ScreenWidth)
                    x = Settings.ScreenWidth - MailLabel.Size.Width;

                if (y + MailLabel.Size.Height > Settings.ScreenHeight)
                    y = Settings.ScreenHeight - MailLabel.Size.Height;
                MailLabel.Location = new Point(x, y);
            }

            if (MemoLabel != null && !MemoLabel.IsDisposed)
            {
                MemoLabel.BringToFront();

                int x = CMain.MPoint.X + 15, y = CMain.MPoint.Y;
                if (x + MemoLabel.Size.Width > Settings.ScreenWidth)
                    x = Settings.ScreenWidth - MemoLabel.Size.Width;

                if (y + MemoLabel.Size.Height > Settings.ScreenHeight)
                    y = Settings.ScreenHeight - MemoLabel.Size.Height;
                MemoLabel.Location = new Point(x, y);
            }

            if (GuildBuffLabel != null && !GuildBuffLabel.IsDisposed)
            {
                GuildBuffLabel.BringToFront();

                int x = CMain.MPoint.X + 15, y = CMain.MPoint.Y;
                if (x + GuildBuffLabel.Size.Width > Settings.ScreenWidth)
                    x = Settings.ScreenWidth - GuildBuffLabel.Size.Width;

                if (y + GuildBuffLabel.Size.Height > Settings.ScreenHeight)
                    y = Settings.ScreenHeight - GuildBuffLabel.Size.Height;
                GuildBuffLabel.Location = new Point(x, y);
            }

            if (!User.Dead) ShowReviveMessage = false;

            if (ShowReviveMessage && CMain.Time > User.DeadTime && User.CurrentAction == MirAction.Dead)
            {
                ShowReviveMessage = false;
                MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DiedTip), MirMessageBoxButtons.YesNo, false);

                messageBox.YesButton.Click += (o, e) =>
                {
                    if (User.Dead) Network.Enqueue(new C.TownRevive());
                };

                messageBox.AfterDraw += (o, e) =>
                {
                    if (!User.Dead) messageBox.Dispose();
                };

                messageBox.Show();
            }

            BuffsDialog.Process();
            HeroBuffsDialog?.Process();

            MapControl.Process();
            MainDialog.Process();
            InventoryDialog.Process();
            GameShopDialog.Process();
            MiniMapDialog.Process();

            foreach (SkillBarDialog Bar in Scene.SkillBarDialogs)
                Bar.Process();

            foreach (ParticleEngine pe in ParticleEngines)
                pe.Process();

            DialogProcess();

            ProcessOuput();

            UpdateMouseCursor();

            SoundManager.ProcessDelayedSounds();
        }

        public void DialogProcess()
        {
            if (Settings.SkillBar)
            {
                foreach (SkillBarDialog Bar in Scene.SkillBarDialogs)
                    Bar.Show();
            }
            else
            {
                foreach (SkillBarDialog Bar in Scene.SkillBarDialogs)
                    Bar.Hide();
            }

            for (int i = 0; i < Scene.SkillBarDialogs.Count; i++)
            {
                if (i * 2 > Settings.SkillbarLocation.Length) break;
                if ((Settings.SkillbarLocation[i, 0] > Settings.Resolution - 100) || (Settings.SkillbarLocation[i, 1] > 700))
                    continue; //in theory you'd want the y coord to be validated based on resolution, but since client only allows for wider screens and not higher :(
                Scene.SkillBarDialogs[i].Location = new Point(Settings.SkillbarLocation[i, 0], Settings.SkillbarLocation[i, 1]);
            }

            if (Settings.DuraView)
                CharacterDuraPanel.Show();
            else
                CharacterDuraPanel.Hide();
        }

        public override void ProcessPacket(Packet p)
        {
            switch (p.Index)
            {
                case (short)ServerPacketIds.KeepAlive:
                    KeepAlive((S.KeepAlive)p);
                    break;
                case (short)ServerPacketIds.MapInformation: //MapInfo
                    MapInformation((S.MapInformation)p);
                    break;
                case (short)ServerPacketIds.NewMapInfo:
                    NewMapInfo((S.NewMapInfo)p);
                    break;
                case (short)ServerPacketIds.WorldMapSetup:
                    WorldMapSetup((S.WorldMapSetupInfo)p);
                    break;
                case (short)ServerPacketIds.SearchMapResult:
                    SearchMapResult((S.SearchMapResult)p);
                    break;
                case (short)ServerPacketIds.UserInformation:
                    UserInformation((S.UserInformation)p);
                    break;
                case (short)ServerPacketIds.UserSlotsRefresh:
                    UserSlotsRefresh((S.UserSlotsRefresh)p);
                    break;
                case (short)ServerPacketIds.UserLocation:
                    UserLocation((S.UserLocation)p);
                    break;
                case (short)ServerPacketIds.ObjectPlayer:
                    ObjectPlayer((S.ObjectPlayer)p);
                    break;
                case (short)ServerPacketIds.ObjectHero:
                    ObjectHero((S.ObjectHero)p);
                    break;
                case (short)ServerPacketIds.ObjectRemove:
                    ObjectRemove((S.ObjectRemove)p);
                    break;
                case (short)ServerPacketIds.ObjectTurn:
                    ObjectTurn((S.ObjectTurn)p);
                    break;
                case (short)ServerPacketIds.ObjectWalk:
                    ObjectWalk((S.ObjectWalk)p);
                    break;
                case (short)ServerPacketIds.ObjectRun:
                    ObjectRun((S.ObjectRun)p);
                    break;
                case (short)ServerPacketIds.Chat:
                    ReceiveChat((S.Chat)p);
                    break;
                case (short)ServerPacketIds.ObjectChat:
                    ObjectChat((S.ObjectChat)p);
                    break;
                case (short)ServerPacketIds.MoveItem:
                    MoveItem((S.MoveItem)p);
                    break;
                case (short)ServerPacketIds.EquipItem:
                    EquipItem((S.EquipItem)p);
                    break;
                case (short)ServerPacketIds.MergeItem:
                    MergeItem((S.MergeItem)p);
                    break;
                case (short)ServerPacketIds.RemoveItem:
                    RemoveItem((S.RemoveItem)p);
                    break;
                case (short)ServerPacketIds.RemoveSlotItem:
                    RemoveSlotItem((S.RemoveSlotItem)p);
                    break;
                case (short)ServerPacketIds.TakeBackItem:
                    TakeBackItem((S.TakeBackItem)p);
                    break;
                case (short)ServerPacketIds.StoreItem:
                    StoreItem((S.StoreItem)p);
                    break;
                case (short)ServerPacketIds.DepositRefineItem:
                    DepositRefineItem((S.DepositRefineItem)p);
                    break;
                case (short)ServerPacketIds.RetrieveRefineItem:
                    RetrieveRefineItem((S.RetrieveRefineItem)p);
                    break;
                case (short)ServerPacketIds.RefineCancel:
                    RefineCancel((S.RefineCancel)p);
                    break;
                case (short)ServerPacketIds.RefineItem:
                    RefineItem((S.RefineItem)p);
                    break;
                case (short)ServerPacketIds.DepositTradeItem:
                    DepositTradeItem((S.DepositTradeItem)p);
                    break;
                case (short)ServerPacketIds.RetrieveTradeItem:
                    RetrieveTradeItem((S.RetrieveTradeItem)p);
                    break;
                case (short)ServerPacketIds.SplitItem:
                    SplitItem((S.SplitItem)p);
                    break;
                case (short)ServerPacketIds.SplitItem1:
                    SplitItem1((S.SplitItem1)p);
                    break;
                case (short)ServerPacketIds.UseItem:
                    UseItem((S.UseItem)p);
                    break;
                case (short)ServerPacketIds.DropItem:
                    DropItem((S.DropItem)p);
                    break;
                case (short)ServerPacketIds.TakeBackHeroItem:
                    TakeBackHeroItem((S.TakeBackHeroItem)p);
                    break;
                case (short)ServerPacketIds.TransferHeroItem:
                    TransferHeroItem((S.TransferHeroItem)p);
                    break;
                case (short)ServerPacketIds.PlayerUpdate:
                    PlayerUpdate((S.PlayerUpdate)p);
                    break;
                case (short)ServerPacketIds.PlayerInspect:
                    PlayerInspect((S.PlayerInspect)p);
                    break;
                case (short)ServerPacketIds.LogOutSuccess:
                    LogOutSuccess((S.LogOutSuccess)p);
                    break;
                case (short)ServerPacketIds.LogOutFailed:
                    LogOutFailed((S.LogOutFailed)p);
                    break;
                case (short)ServerPacketIds.ReturnToLogin:
                    ReturnToLogin((S.ReturnToLogin)p);
                    break;
                case (short)ServerPacketIds.TimeOfDay:
                    TimeOfDay((S.TimeOfDay)p);
                    break;
                case (short)ServerPacketIds.ChangeAMode:
                    ChangeAMode((S.ChangeAMode)p);
                    break;
                case (short)ServerPacketIds.ChangePMode:
                    ChangePMode((S.ChangePMode)p);
                    break;
                case (short)ServerPacketIds.ObjectItem:
                    ObjectItem((S.ObjectItem)p);
                    break;
                case (short)ServerPacketIds.ObjectGold:
                    ObjectGold((S.ObjectGold)p);
                    break;
                case (short)ServerPacketIds.GainedItem:
                    GainedItem((S.GainedItem)p);
                    break;
                case (short)ServerPacketIds.GainedGold:
                    GainedGold((S.GainedGold)p);
                    break;
                case (short)ServerPacketIds.LoseGold:
                    LoseGold((S.LoseGold)p);
                    break;
                case (short)ServerPacketIds.GainedCredit:
                    GainedCredit((S.GainedCredit)p);
                    break;
                case (short)ServerPacketIds.LoseCredit:
                    LoseCredit((S.LoseCredit)p);
                    break;
                case (short)ServerPacketIds.ObjectMonster:
                    ObjectMonster((S.ObjectMonster)p);
                    break;
                case (short)ServerPacketIds.ObjectAttack:
                    ObjectAttack((S.ObjectAttack)p);
                    break;
                case (short)ServerPacketIds.Struck:
                    Struck((S.Struck)p);
                    break;
                case (short)ServerPacketIds.DamageIndicator:
                    DamageIndicator((S.DamageIndicator)p);
                    break;
                case (short)ServerPacketIds.ObjectStruck:
                    ObjectStruck((S.ObjectStruck)p);
                    break;
                case (short)ServerPacketIds.DuraChanged:
                    DuraChanged((S.DuraChanged)p);
                    break;
                case (short)ServerPacketIds.HealthChanged:
                    HealthChanged((S.HealthChanged)p);
                    break;
                case (short)ServerPacketIds.HeroHealthChanged:
                    HeroHealthChanged((S.HeroHealthChanged)p);
                    break;
                case (short)ServerPacketIds.DeleteItem:
                    DeleteItem((S.DeleteItem)p);
                    break;
                case (short)ServerPacketIds.Death:
                    Death((S.Death)p);
                    break;
                case (short)ServerPacketIds.ObjectDied:
                    ObjectDied((S.ObjectDied)p);
                    break;
                case (short)ServerPacketIds.ColourChanged:
                    ColourChanged((S.ColourChanged)p);
                    break;
                case (short)ServerPacketIds.ObjectColourChanged:
                    ObjectColourChanged((S.ObjectColourChanged)p);
                    break;
                case (short)ServerPacketIds.ObjectGuildNameChanged:
                    ObjectGuildNameChanged((S.ObjectGuildNameChanged)p);
                    break;
                case (short)ServerPacketIds.GainExperience:
                    GainExperience((S.GainExperience)p);
                    break;
                case (short)ServerPacketIds.GainHeroExperience:
                    GainHeroExperience((S.GainHeroExperience)p);
                    break;
                case (short)ServerPacketIds.LevelChanged:
                    LevelChanged((S.LevelChanged)p);
                    break;
                case (short)ServerPacketIds.HeroLevelChanged:
                    HeroLevelChanged((S.HeroLevelChanged)p);
                    break;
                case (short)ServerPacketIds.ObjectLeveled:
                    ObjectLeveled((S.ObjectLeveled)p);
                    break;
                case (short)ServerPacketIds.ObjectHarvest:
                    ObjectHarvest((S.ObjectHarvest)p);
                    break;
                case (short)ServerPacketIds.ObjectHarvested:
                    ObjectHarvested((S.ObjectHarvested)p);
                    break;
                case (short)ServerPacketIds.ObjectNpc:
                    ObjectNPC((S.ObjectNPC)p);
                    break;
                case (short)ServerPacketIds.NPCResponse:
                    NPCResponse((S.NPCResponse)p);
                    break;
                case (short)ServerPacketIds.ObjectHide:
                    ObjectHide((S.ObjectHide)p);
                    break;
                case (short)ServerPacketIds.ObjectShow:
                    ObjectShow((S.ObjectShow)p);
                    break;
                case (short)ServerPacketIds.Poisoned:
                    Poisoned((S.Poisoned)p);
                    break;
                case (short)ServerPacketIds.ObjectPoisoned:
                    ObjectPoisoned((S.ObjectPoisoned)p);
                    break;
                case (short)ServerPacketIds.MapChanged:
                    MapChanged((S.MapChanged)p);
                    break;
                case (short)ServerPacketIds.ObjectTeleportOut:
                    ObjectTeleportOut((S.ObjectTeleportOut)p);
                    break;
                case (short)ServerPacketIds.ObjectTeleportIn:
                    ObjectTeleportIn((S.ObjectTeleportIn)p);
                    break;
                case (short)ServerPacketIds.TeleportIn:
                    TeleportIn();
                    break;
                case (short)ServerPacketIds.NPCGoods:
                    NPCGoods((S.NPCGoods)p);
                    break;
                case (short)ServerPacketIds.NPCSell:
                    NPCSell();
                    break;
                case (short)ServerPacketIds.NPCRepair:
                    NPCRepair((S.NPCRepair)p);
                    break;
                case (short)ServerPacketIds.NPCSRepair:
                    NPCSRepair((S.NPCSRepair)p);
                    break;
                case (short)ServerPacketIds.NPCRefine:
                    NPCRefine((S.NPCRefine)p);
                    break;
                case (short)ServerPacketIds.NPCCheckRefine:
                    NPCCheckRefine((S.NPCCheckRefine)p);
                    break;
                case (short)ServerPacketIds.NPCCollectRefine:
                    NPCCollectRefine((S.NPCCollectRefine)p);
                    break;
                case (short)ServerPacketIds.NPCReplaceWedRing:
                    NPCReplaceWedRing((S.NPCReplaceWedRing)p);
                    break;
                case (short)ServerPacketIds.NPCStorage:
                    NPCStorage();
                    break;
                case (short)ServerPacketIds.NPCRequestInput:
                    NPCRequestInput((S.NPCRequestInput)p);
                    break;
                case (short)ServerPacketIds.SellItem:
                    SellItem((S.SellItem)p);
                    break;
                case (short)ServerPacketIds.CraftItem:
                    CraftItem((S.CraftItem)p);
                    break;
                case (short)ServerPacketIds.RepairItem:
                    RepairItem((S.RepairItem)p);
                    break;
                case (short)ServerPacketIds.ItemRepaired:
                    ItemRepaired((S.ItemRepaired)p);
                    break;
                case (short)ServerPacketIds.ItemSlotSizeChanged:
                    ItemSlotSizeChanged((S.ItemSlotSizeChanged)p);
                    break;
                case (short)ServerPacketIds.ItemSealChanged:
                    ItemSealChanged((S.ItemSealChanged)p);
                    break;
                case (short)ServerPacketIds.NewMagic:
                    NewMagic((S.NewMagic)p);
                    break;
                case (short)ServerPacketIds.MagicLeveled:
                    MagicLeveled((S.MagicLeveled)p);
                    break;
                case (short)ServerPacketIds.Magic:
                    Magic((S.Magic)p);
                    break;
                case (short)ServerPacketIds.MagicDelay:
                    MagicDelay((S.MagicDelay)p);
                    break;
                case (short)ServerPacketIds.MagicCast:
                    MagicCast((S.MagicCast)p);
                    break;
                case (short)ServerPacketIds.ObjectMagic:
                    ObjectMagic((S.ObjectMagic)p);
                    break;
                case (short)ServerPacketIds.ObjectProjectile:
                    ObjectProjectile((S.ObjectProjectile)p);
                    break;
                case (short)ServerPacketIds.ObjectEffect:
                    ObjectEffect((S.ObjectEffect)p);
                    break;
                case (short)ServerPacketIds.RangeAttack:
                    RangeAttack((S.RangeAttack)p);
                    break;
                case (short)ServerPacketIds.Pushed:
                    Pushed((S.Pushed)p);
                    break;
                case (short)ServerPacketIds.ObjectPushed:
                    ObjectPushed((S.ObjectPushed)p);
                    break;
                case (short)ServerPacketIds.ObjectName:
                    ObjectName((S.ObjectName)p);
                    break;
                case (short)ServerPacketIds.UserStorage:
                    UserStorage((S.UserStorage)p);
                    break;
                case (short)ServerPacketIds.SwitchGroup:
                    SwitchGroup((S.SwitchGroup)p);
                    break;
                case (short)ServerPacketIds.DeleteGroup:
                    DeleteGroup();
                    break;
                case (short)ServerPacketIds.DeleteMember:
                    DeleteMember((S.DeleteMember)p);
                    break;
                case (short)ServerPacketIds.GroupInvite:
                    GroupInvite((S.GroupInvite)p);
                    break;
                case (short)ServerPacketIds.AddMember:
                    AddMember((S.AddMember)p);
                    break;
                case (short)ServerPacketIds.GroupMembersMap:
                    GroupMembersMap((S.GroupMembersMap)p);
                    break;
                case (short)ServerPacketIds.SendMemberLocation:
                    SendMemberLocation((S.SendMemberLocation)p);
                    break;
                case (short)ServerPacketIds.Revived:
                    Revived();
                    break;
                case (short)ServerPacketIds.ObjectRevived:
                    ObjectRevived((S.ObjectRevived)p);
                    break;
                case (short)ServerPacketIds.SpellToggle:
                    SpellToggle((S.SpellToggle)p);
                    break;
                case (short)ServerPacketIds.ObjectHealth:
                    ObjectHealth((S.ObjectHealth)p);
                    break;
                case (short)ServerPacketIds.ObjectMana:
                    ObjectMana((S.ObjectMana)p);
                    break;
                case (short)ServerPacketIds.MapEffect:
                    MapEffect((S.MapEffect)p);
                    break;
                case (short)ServerPacketIds.AllowObserve:
                    AllowObserve = ((S.AllowObserve)p).Allow;
                    break;
                case (short)ServerPacketIds.ObjectRangeAttack:
                    ObjectRangeAttack((S.ObjectRangeAttack)p);
                    break;
                case (short)ServerPacketIds.AddBuff:
                    AddBuff((S.AddBuff)p);
                    break;
                case (short)ServerPacketIds.RemoveBuff:
                    RemoveBuff((S.RemoveBuff)p);
                    break;
                case (short)ServerPacketIds.PauseBuff:
                    PauseBuff((S.PauseBuff)p);
                    break;
                case (short)ServerPacketIds.ObjectHidden:
                    ObjectHidden((S.ObjectHidden)p);
                    break;
                case (short)ServerPacketIds.RefreshItem:
                    RefreshItem((S.RefreshItem)p);
                    break;
                case (short)ServerPacketIds.ObjectSpell:
                    ObjectSpell((S.ObjectSpell)p);
                    break;
                case (short)ServerPacketIds.UserDash:
                    UserDash((S.UserDash)p);
                    break;
                case (short)ServerPacketIds.ObjectDash:
                    ObjectDash((S.ObjectDash)p);
                    break;
                case (short)ServerPacketIds.UserDashFail:
                    UserDashFail((S.UserDashFail)p);
                    break;
                case (short)ServerPacketIds.ObjectDashFail:
                    ObjectDashFail((S.ObjectDashFail)p);
                    break;
                case (short)ServerPacketIds.NPCConsign:
                    NPCConsign();
                    break;
                case (short)ServerPacketIds.NPCMarket:
                    NPCMarket((S.NPCMarket)p);
                    break;
                case (short)ServerPacketIds.NPCMarketPage:
                    NPCMarketPage((S.NPCMarketPage)p);
                    break;
                case (short)ServerPacketIds.ConsignItem:
                    ConsignItem((S.ConsignItem)p);
                    break;
                case (short)ServerPacketIds.MarketFail:
                    MarketFail((S.MarketFail)p);
                    break;
                case (short)ServerPacketIds.MarketSuccess:
                    MarketSuccess((S.MarketSuccess)p);
                    break;
                case (short)ServerPacketIds.ObjectSitDown:
                    ObjectSitDown((S.ObjectSitDown)p);
                    break;
                case (short)ServerPacketIds.InTrapRock:
                    S.InTrapRock packetdata = (S.InTrapRock)p;
                    User.InTrapRock = packetdata.Trapped;
                    break;
                case (short)ServerPacketIds.RemoveMagic:
                    RemoveMagic((S.RemoveMagic)p);
                    break;
                case (short)ServerPacketIds.BaseStatsInfo:
                    BaseStatsInfo((S.BaseStatsInfo)p);
                    break;
                case (short)ServerPacketIds.HeroBaseStatsInfo:
                    HeroBaseStatsInfo((S.HeroBaseStatsInfo)p);
                    break;
                case (short)ServerPacketIds.UserName:
                    UserName((S.UserName)p);
                    break;
                case (short)ServerPacketIds.ChatItemStats:
                    ChatItemStats((S.ChatItemStats)p);
                    break;
                case (short)ServerPacketIds.GuildInvite:
                    GuildInvite((S.GuildInvite)p);
                    break;
                case (short)ServerPacketIds.GuildMemberChange:
                    GuildMemberChange((S.GuildMemberChange)p);
                    break;
                case (short)ServerPacketIds.GuildNoticeChange:
                    GuildNoticeChange((S.GuildNoticeChange)p);
                    break;
                case (short)ServerPacketIds.GuildStatus:
                    GuildStatus((S.GuildStatus)p);
                    break;
                case (short)ServerPacketIds.GuildExpGain:
                    GuildExpGain((S.GuildExpGain)p);
                    break;
                case (short)ServerPacketIds.GuildNameRequest:
                    GuildNameRequest((S.GuildNameRequest)p);
                    break;
                case (short)ServerPacketIds.GuildStorageGoldChange:
                    GuildStorageGoldChange((S.GuildStorageGoldChange)p);
                    break;
                case (short)ServerPacketIds.GuildStorageItemChange:
                    GuildStorageItemChange((S.GuildStorageItemChange)p);
                    break;
                case (short)ServerPacketIds.GuildStorageList:
                    GuildStorageList((S.GuildStorageList)p);
                    break;
                case (short)ServerPacketIds.GuildRequestWar:
                    GuildRequestWar((S.GuildRequestWar)p);
                    break;
                case (short)ServerPacketIds.HeroCreateRequest:
                    HeroCreateRequest((S.HeroCreateRequest)p);
                    break;
                case (short)ServerPacketIds.NewHero:
                    NewHero((S.NewHero)p);
                    break;
                case (short)ServerPacketIds.HeroInformation:
                    HeroInformation((S.HeroInformation)p);
                    break;
                case (short)ServerPacketIds.UpdateHeroSpawnState:
                    UpdateHeroSpawnState((S.UpdateHeroSpawnState)p);
                    break;
                case (short)ServerPacketIds.UnlockHeroAutoPot:
                    UnlockHeroAutoPot(true);
                    break;
                case (short)ServerPacketIds.SetAutoPotValue:
                    SetAutoPotValue((S.SetAutoPotValue)p);
                    break;
                case (short)ServerPacketIds.SetHeroBehaviour:
                    SetHeroBehaviour((S.SetHeroBehaviour)p);
                    break;
                case (short)ServerPacketIds.SetAutoPotItem:
                    SetAutoPotItem((S.SetAutoPotItem)p);
                    break;
                case (short)ServerPacketIds.ManageHeroes:
                    ManageHeroes((S.ManageHeroes)p);
                    break;
                case (short)ServerPacketIds.ChangeHero:
                    ChangeHero((S.ChangeHero)p);
                    break;
                case (short)ServerPacketIds.DefaultNPC:
                    DefaultNPC((S.DefaultNPC)p);
                    break;
                case (short)ServerPacketIds.NPCUpdate:
                    NPCUpdate((S.NPCUpdate)p);
                    break;
                case (short)ServerPacketIds.NPCImageUpdate:
                    NPCImageUpdate((S.NPCImageUpdate)p);
                    break;
                case (short)ServerPacketIds.MarriageRequest:
                    MarriageRequest((S.MarriageRequest)p);
                    break;
                case (short)ServerPacketIds.DivorceRequest:
                    DivorceRequest((S.DivorceRequest)p);
                    break;
                case (short)ServerPacketIds.MentorRequest:
                    MentorRequest((S.MentorRequest)p);
                    break;
                case (short)ServerPacketIds.TradeRequest:
                    TradeRequest((S.TradeRequest)p);
                    break;
                case (short)ServerPacketIds.TradeAccept:
                    TradeAccept((S.TradeAccept)p);
                    break;
                case (short)ServerPacketIds.TradeGold:
                    TradeGold((S.TradeGold)p);
                    break;
                case (short)ServerPacketIds.TradeItem:
                    TradeItem((S.TradeItem)p);
                    break;
                case (short)ServerPacketIds.TradeConfirm:
                    TradeConfirm();
                    break;
                case (short)ServerPacketIds.TradeCancel:
                    TradeCancel((S.TradeCancel)p);
                    break;
                case (short)ServerPacketIds.MountUpdate:
                    MountUpdate((S.MountUpdate)p);
                    break;
                case (short)ServerPacketIds.TransformUpdate:
                    TransformUpdate((S.TransformUpdate)p);
                    break;
                case (short)ServerPacketIds.EquipSlotItem:
                    EquipSlotItem((S.EquipSlotItem)p);
                    break;
                case (short)ServerPacketIds.FishingUpdate:
                    FishingUpdate((S.FishingUpdate)p);
                    break;
                case (short)ServerPacketIds.ChangeQuest:
                    ChangeQuest((S.ChangeQuest)p);
                    break;
                case (short)ServerPacketIds.CompleteQuest:
                    CompleteQuest((S.CompleteQuest)p);
                    break;
                case (short)ServerPacketIds.ShareQuest:
                    ShareQuest((S.ShareQuest)p);
                    break;
                case (short)ServerPacketIds.GainedQuestItem:
                    GainedQuestItem((S.GainedQuestItem)p);
                    break;
                case (short)ServerPacketIds.DeleteQuestItem:
                    DeleteQuestItem((S.DeleteQuestItem)p);
                    break;
                case (short)ServerPacketIds.CancelReincarnation:
                    User.ReincarnationStopTime = 0;
                    break;
                case (short)ServerPacketIds.RequestReincarnation:
                    if (!User.Dead) return;
                    RequestReincarnation();
                    break;
                case (short)ServerPacketIds.UserBackStep:
                    UserBackStep((S.UserBackStep)p);
                    break;
                case (short)ServerPacketIds.ObjectBackStep:
                    ObjectBackStep((S.ObjectBackStep)p);
                    break;
                case (short)ServerPacketIds.UserDashAttack:
                    UserDashAttack((S.UserDashAttack)p);
                    break;
                case (short)ServerPacketIds.ObjectDashAttack:
                    ObjectDashAttack((S.ObjectDashAttack)p);
                    break;
                case (short)ServerPacketIds.UserAttackMove://Warrior Skill - SlashingBurst
                    UserAttackMove((S.UserAttackMove)p);
                    break;
                case (short)ServerPacketIds.CombineItem:
                    CombineItem((S.CombineItem)p);
                    break;
                case (short)ServerPacketIds.ItemUpgraded:
                    ItemUpgraded((S.ItemUpgraded)p);
                    break;
                case (short)ServerPacketIds.SetConcentration:
                    SetConcentration((S.SetConcentration)p);
                    break;
                case (short)ServerPacketIds.SetElemental:
                    SetElemental((S.SetElemental)p);
                    break;
                case (short)ServerPacketIds.RemoveDelayedExplosion:
                    RemoveDelayedExplosion((S.RemoveDelayedExplosion)p);
                    break;
                case (short)ServerPacketIds.ObjectDeco:
                    ObjectDeco((S.ObjectDeco)p);
                    break;
                case (short)ServerPacketIds.ObjectSneaking:
                    ObjectSneaking((S.ObjectSneaking)p);
                    break;
                case (short)ServerPacketIds.ObjectLevelEffects:
                    ObjectLevelEffects((S.ObjectLevelEffects)p);
                    break;
                case (short)ServerPacketIds.SetBindingShot:
                    SetBindingShot((S.SetBindingShot)p);
                    break;
                case (short)ServerPacketIds.SendOutputMessage:
                    SendOutputMessage((S.SendOutputMessage)p);
                    break;
                case (short)ServerPacketIds.NPCAwakening:
                    NPCAwakening();
                    break;
                case (short)ServerPacketIds.NPCDisassemble:
                    NPCDisassemble();
                    break;
                case (short)ServerPacketIds.NPCDowngrade:
                    NPCDowngrade();
                    break;
                case (short)ServerPacketIds.NPCReset:
                    NPCReset();
                    break;
                case (short)ServerPacketIds.AwakeningNeedMaterials:
                    AwakeningNeedMaterials((S.AwakeningNeedMaterials)p);
                    break;
                case (short)ServerPacketIds.AwakeningLockedItem:
                    AwakeningLockedItem((S.AwakeningLockedItem)p);
                    break;
                case (short)ServerPacketIds.Awakening:
                    Awakening((S.Awakening)p);
                    break;
                case (short)ServerPacketIds.ReceiveMail:
                    ReceiveMail((S.ReceiveMail)p);
                    break;
                case (short)ServerPacketIds.MailLockedItem:
                    MailLockedItem((S.MailLockedItem)p);
                    break;
                case (short)ServerPacketIds.MailSent:
                    MailSent((S.MailSent)p);
                    break;
                case (short)ServerPacketIds.MailSendRequest:
                    MailSendRequest((S.MailSendRequest)p);
                    break;
                case (short)ServerPacketIds.ParcelCollected:
                    ParcelCollected((S.ParcelCollected)p);
                    break;
                case (short)ServerPacketIds.MailCost:
                    MailCost((S.MailCost)p);
                    break;
                case (short)ServerPacketIds.ResizeInventory:
                    ResizeInventory((S.ResizeInventory)p);
                    break;
                case (short)ServerPacketIds.ResizeStorage:
                    ResizeStorage((S.ResizeStorage)p);
                    break;
                case (short)ServerPacketIds.NewIntelligentCreature:
                    NewIntelligentCreature((S.NewIntelligentCreature)p);
                    break;
                case (short)ServerPacketIds.UpdateIntelligentCreatureList:
                    UpdateIntelligentCreatureList((S.UpdateIntelligentCreatureList)p);
                    break;
                case (short)ServerPacketIds.IntelligentCreatureEnableRename:
                    IntelligentCreatureEnableRename((S.IntelligentCreatureEnableRename)p);
                    break;
                case (short)ServerPacketIds.IntelligentCreaturePickup:
                    IntelligentCreaturePickup((S.IntelligentCreaturePickup)p);
                    break;
                case (short)ServerPacketIds.NPCPearlGoods:
                    NPCPearlGoods((S.NPCPearlGoods)p);
                    break;
                case (short)ServerPacketIds.FriendUpdate:
                    FriendUpdate((S.FriendUpdate)p);
                    break;
                case (short)ServerPacketIds.LoverUpdate:
                    LoverUpdate((S.LoverUpdate)p);
                    break;
                case (short)ServerPacketIds.MentorUpdate:
                    MentorUpdate((S.MentorUpdate)p);
                    break;
                case (short)ServerPacketIds.GuildBuffList:
                    GuildBuffList((S.GuildBuffList)p);
                    break;
                case (short)ServerPacketIds.GameShopInfo:
                    GameShopUpdate((S.GameShopInfo)p);
                    break;
                case (short)ServerPacketIds.GameShopStock:
                    GameShopStock((S.GameShopStock)p);
                    break;
                case (short)ServerPacketIds.Rankings:
                    Rankings((S.Rankings)p);
                    break;
                case (short)ServerPacketIds.Opendoor:
                    Opendoor((S.Opendoor)p);
                    break;
                case (short)ServerPacketIds.GetRentedItems:
                    RentedItems((S.GetRentedItems)p);
                    break;
                case (short)ServerPacketIds.ItemRentalRequest:
                    ItemRentalRequest((S.ItemRentalRequest)p);
                    break;
                case (short)ServerPacketIds.ItemRentalFee:
                    ItemRentalFee((S.ItemRentalFee)p);
                    break;
                case (short)ServerPacketIds.ItemRentalPeriod:
                    ItemRentalPeriod((S.ItemRentalPeriod)p);
                    break;
                case (short)ServerPacketIds.DepositRentalItem:
                    DepositRentalItem((S.DepositRentalItem)p);
                    break;
                case (short)ServerPacketIds.RetrieveRentalItem:
                    RetrieveRentalItem((S.RetrieveRentalItem)p);
                    break;
                case (short)ServerPacketIds.UpdateRentalItem:
                    UpdateRentalItem((S.UpdateRentalItem)p);
                    break;
                case (short)ServerPacketIds.CancelItemRental:
                    CancelItemRental((S.CancelItemRental)p);
                    break;
                case (short)ServerPacketIds.ItemRentalLock:
                    ItemRentalLock((S.ItemRentalLock)p);
                    break;
                case (short)ServerPacketIds.ItemRentalPartnerLock:
                    ItemRentalPartnerLock((S.ItemRentalPartnerLock)p);
                    break;
                case (short)ServerPacketIds.CanConfirmItemRental:
                    CanConfirmItemRental((S.CanConfirmItemRental)p);
                    break;
                case (short)ServerPacketIds.ConfirmItemRental:
                    ConfirmItemRental((S.ConfirmItemRental)p);
                    break;
                case (short)ServerPacketIds.OpenBrowser:
                    OpenBrowser((S.OpenBrowser)p);
                    break;
                case (short)ServerPacketIds.PlaySound:
                    PlaySound((S.PlaySound)p);
                    break;
                case (short)ServerPacketIds.SetTimer:
                    SetTimer((S.SetTimer)p);
                    break;
                case (short)ServerPacketIds.ExpireTimer:
                    ExpireTimer((S.ExpireTimer)p);
                    break;
                case (short)ServerPacketIds.UpdateNotice:
                    ShowNotice((S.UpdateNotice)p);
                    break;
                case (short)ServerPacketIds.Roll:
                    Roll((S.Roll)p);
                    break;
                case (short)ServerPacketIds.SetCompass:
                    SetCompass((S.SetCompass)p);
                    break;
                case (short)ServerPacketIds.GuildTerritoryPage:
                    GuildTerritoryPage((S.GuildTerritoryPage)p);
                    break;
                case (short)ServerPacketIds.NewMonsterInfo:
                    NewMonsterInfo((S.NewMonsterInfo)p);
                    break;
                case (short)ServerPacketIds.NewNPCInfo:
                    NewNPCInfo((S.NewNPCInfo)p);
                    break;
                default:
                    base.ProcessPacket(p);
                    break;
            }
        }

        private void KeepAlive(S.KeepAlive p)
        {
            if (p.Time == 0) return;
            CMain.PingTime = (CMain.Time - p.Time);
        }
        private void MapInformation(S.MapInformation p)
        {
            if (MapControl != null && !MapControl.IsDisposed)
                MapControl.Dispose();
            MapControl = new MapControl
            {
                Index = p.MapIndex,
                FileName = Path.Combine(Settings.MapPath, p.FileName + ".map"),
                Title = p.Title,
                MiniMap = p.MiniMap,
                BigMap = p.BigMap,
                Lights = p.Lights,
                Lightning = p.Lightning,
                Fire = p.Fire,
                MapDarkLight = p.MapDarkLight,
                Music = p.Music
            };
            MapControl.Weather = p.WeatherParticles;
            MapControl.LoadMap();
            InsertControl(0, MapControl);
        }

        private void WorldMapSetup(S.WorldMapSetupInfo info)
        {
            BigMapDialog.WorldMapSetup(info.Setup);
            TeleportToNPCCost = info.TeleportToNPCCost;
        }

        private void NewMapInfo(S.NewMapInfo info)
        {
            BigMapRecord newRecord = new BigMapRecord() { Index = info.MapIndex, MapInfo = info.Info };
            CreateBigMapButtons(newRecord);
            MapInfoList.Add(info.MapIndex, newRecord);
        }

        private void CreateBigMapButtons(BigMapRecord record)
        {
            record.MovementButtons.Clear();
            record.NPCButtons.Clear();

            foreach (ClientMovementInfo mInfo in record.MapInfo.Movements)
            {
                MirButton button = new MirButton()
                {
                    Library = Libraries.MapLinkIcon,
                    Index = mInfo.Icon,
                    PressedIndex = mInfo.Icon,
                    Sound = SoundList.ButtonA,
                    Parent = BigMapDialog.ViewPort,
                    Location = new Point(20, 38),
                    Hint = mInfo.Title,
                    Visible = false
                };
                button.MouseEnter += (o, e) =>
                {
                    BigMapDialog.MouseLocation = mInfo.Location;
                };

                button.Click += (o, e) =>
                {
                    BigMapDialog.SetTargetMap(mInfo.Destination);
                };
                record.MovementButtons.Add(mInfo, button);
            }

            foreach (ClientNPCInfo npcInfo in record.MapInfo.NPCs)
            {
                BigMapNPCRow row = new BigMapNPCRow(npcInfo) { Parent = BigMapDialog };
                record.NPCButtons.Add(row);
            }
        }

        private void RecreateBigMapButtons()
        {
            foreach (var record in MapInfoList.Values)
                CreateBigMapButtons(record);
        }

        private void SearchMapResult(S.SearchMapResult info)
        {
            if (info.MapIndex == -1 && info.NPCIndex == 0)
            {
                MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NothingFound), MirMessageBoxButtons.OK);
                messageBox.OKButton.Click += (o, a) => { BigMapDialog.SearchTextBox.SetFocus(); };
                messageBox.Show();
                return;
            }

            BigMapDialog.SetTargetMap(info.MapIndex);
            BigMapDialog.SetTargetNPC(info.NPCIndex);
        }
        private void UserInformation(S.UserInformation p)
        {
            User = new UserObject(p.ObjectID);
            User.Load(p);
            MainDialog.PModeLabel.Visible = User.Class == MirClass.Wizard || User.Class == MirClass.Taoist;
            HasHero = p.HasHero;
            HeroBehaviourPanel.UpdateBehaviour(p.HeroBehaviour);
            Gold = p.Gold;
            Credit = p.Credit;

            CharacterDialog = new CharacterDialog(MirGridType.Equipment, User) { Parent = this, Visible = false };
            InventoryDialog.RefreshInventory();
            foreach (SkillBarDialog Bar in SkillBarDialogs)
                Bar.Update();
            AllowObserve = p.AllowObserve;
            Observing = p.Observer;
        }
        private void UserSlotsRefresh(S.UserSlotsRefresh p)
        {
            User.SetSlots(p);
        }

        private void UserLocation(S.UserLocation p)
        {
            MapControl.NextAction = 0;
            if (User.CurrentLocation == p.Location && User.Direction == p.Direction) return;

            MapControl.RemoveObject(User);
            User.CurrentLocation = p.Location;
            User.MapLocation = p.Location;
            MapControl.AddObject(User);

            MapControl.FloorValid = false;
            MapControl.InputDelay = CMain.Time + 400;

            if (User.Dead) return;

            User.ClearMagic();
            User.QueuedAction = null;

            for (int i = User.ActionFeed.Count - 1; i >= 0; i--)
            {
                if (User.ActionFeed[i].Action == MirAction.Pushed) continue;
                User.ActionFeed.RemoveAt(i);
            }

            User.SetAction();
        }
        private void ReceiveChat(S.Chat p)
        {
            ChatDialog.ReceiveChat(p.Message, p.Type);
        }
        private void ObjectPlayer(S.ObjectPlayer p)
        {
            PlayerObject player = new PlayerObject(p.ObjectID);
            player.Load(p);
        }

        private void ObjectHero(S.ObjectHero p)
        {
            HeroObject hero = new HeroObject(p.ObjectID);
            hero.Load(p);

            if (p.ObjectID == Hero?.ObjectID)
                HeroObject = hero;
        }

        private void ObjectRemove(S.ObjectRemove p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob))
                ob.Remove();
        }
        private void ObjectTurn(S.ObjectTurn p)
        {
            if (p.ObjectID == User.ObjectID && !Observing) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob))
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Standing, Direction = p.Direction, Location = p.Location });
        }

        private void ObjectWalk(S.ObjectWalk p)
        {
            if (p.ObjectID == User.ObjectID && !Observing) return;

            if (p.ObjectID == Hero?.ObjectID)
                Hero.CurrentLocation = p.Location;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob))
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Walking, Direction = p.Direction, Location = p.Location });
        }

        private void ObjectRun(S.ObjectRun p)
        {
            if (p.ObjectID == User.ObjectID && !Observing) return;

            if (p.ObjectID == Hero?.ObjectID)
                Hero.CurrentLocation = p.Location;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob))
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Running, Direction = p.Direction, Location = p.Location });
        }

        private void ObjectChat(S.ObjectChat p)
        {
            ChatDialog.ReceiveChat(p.Text, p.Type);

            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob))
                ob.Chat(RegexFunctions.CleanChatString(p.Text));
        }

        private void NewMonsterInfo(S.NewMonsterInfo info)
        {
            GameScene.MonsterInfoList.RemoveAll(x => x.Index == info.Info.Index);
            GameScene.MonsterInfoList.Add(info.Info);
            GameScene.OnMonsterInfoReceived(info.Info.Index);
        }

        private void NewNPCInfo(S.NewNPCInfo info)
        {
            GameScene.NPCInfoList.RemoveAll(x => x.Index == info.Info.Index);
            GameScene.NPCInfoList.Add(info.Info);
            GameScene.OnNPCInfoReceived(info.Info.Index);
        }

        private void MoveItem(S.MoveItem p)
        {
            MirItemCell toCell, fromCell;

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    fromCell = p.From < User.BeltIdx ? BeltDialog.Grid[p.From] : InventoryDialog.Grid[p.From - User.BeltIdx];
                    break;
                case MirGridType.Storage:
                    fromCell = StorageDialog.Grid[p.From];
                    break;
                case MirGridType.Trade:
                    fromCell = TradeDialog.Grid[p.From];
                    break;
                case MirGridType.Refine:
                    fromCell = RefineDialog.Grid[p.From];
                    break;
                case MirGridType.HeroInventory:
                    fromCell = p.From < User.HeroBeltIdx ? HeroBeltDialog.Grid[p.From] : HeroInventoryDialog.Grid[p.From - User.HeroBeltIdx];
                    break;
                default:
                    return;
            }

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];
                    break;
                case MirGridType.Storage:
                    toCell = StorageDialog.Grid[p.To];
                    break;
                case MirGridType.Trade:
                    toCell = TradeDialog.Grid[p.To];
                    break;
                case MirGridType.Refine:
                    toCell = RefineDialog.Grid[p.To];
                    break;
                case MirGridType.HeroInventory:
                    toCell = p.To < User.HeroBeltIdx ? HeroBeltDialog.Grid[p.To] : HeroInventoryDialog.Grid[p.To - User.HeroBeltIdx];
                    break;
                default:
                    return;
            }

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (p.Grid == MirGridType.Trade)
                TradeDialog.ChangeLockState(false);

            if (!p.Success) return;

            UserItem i = fromCell.Item;
            fromCell.Item = toCell.Item;
            toCell.Item = i;

            User.RefreshStats();
            CharacterDuraPanel.GetCharacterDura();
        }
        private void EquipItem(S.EquipItem p)
        {
            MirItemCell fromCell, toCell;

            switch (p.Grid)
            {
                case MirGridType.HeroInventory:
                    toCell = HeroDialog.Grid[p.To];
                    break;
                default:
                    toCell = CharacterDialog.Grid[p.To];
                    break;
            }

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    fromCell = InventoryDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);
                    break;
                case MirGridType.Storage:
                    fromCell = StorageDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);
                    break;
                case MirGridType.HeroInventory:
                    fromCell = HeroInventoryDialog.GetCell(p.UniqueID) ?? HeroBeltDialog.GetCell(p.UniqueID);
                    break;
                default:
                    return;
            }

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;

            UserItem i = fromCell.Item;
            fromCell.Item = toCell.Item;
            toCell.Item = i;
            CharacterDuraPanel.UpdateCharacterDura(i);
            if (p.Grid == MirGridType.HeroInventory)
                Hero.RefreshStats();
            else
                User.RefreshStats();
        }
        private void EquipSlotItem(S.EquipSlotItem p)
        {
            MirItemCell fromCell;
            MirItemCell toCell;

            switch (p.GridTo)
            {
                case MirGridType.Socket:
                    toCell = SocketDialog.Grid[p.To];
                    break;
                case MirGridType.Mount:
                    toCell = MountDialog.Grid[p.To];
                    break;
                case MirGridType.Fishing:
                    toCell = FishingDialog.Grid[p.To];
                    break;
                default:
                    return;
            }

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    fromCell = InventoryDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);
                    break;
                case MirGridType.Storage:
                    fromCell = StorageDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);
                    break;
                default:
                    return;
            }

            //if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;

            UserItem i = fromCell.Item;
            fromCell.Item = null;
            toCell.Item = i;
            User.RefreshStats();
        }

        private void CombineItem(S.CombineItem p)
        {
            MirItemCell fromCell = null;
            MirItemCell toCell = null;
            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    fromCell = InventoryDialog.GetCell(p.IDFrom) ?? BeltDialog.GetCell(p.IDFrom);
                    toCell = InventoryDialog.GetCell(p.IDTo) ?? BeltDialog.GetCell(p.IDTo);
                    break;
                case MirGridType.HeroInventory:
                    fromCell = HeroInventoryDialog.GetCell(p.IDFrom) ?? HeroBeltDialog.GetCell(p.IDFrom);
                    toCell = HeroInventoryDialog.GetCell(p.IDTo) ?? HeroBeltDialog.GetCell(p.IDTo);
                    break;
            }

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (p.Destroy) toCell.Item = null;

            if (!p.Success) return;

            if (fromCell.Item.Count > 1)
                fromCell.Item.Count--;
            else
                fromCell.Item = null;

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    User.RefreshStats();
                    break;
                case MirGridType.HeroInventory:
                    Hero.RefreshStats();
                    break;
            }
        }

        private void MergeItem(S.MergeItem p)
        {
            MirItemCell toCell, fromCell;

            switch (p.GridFrom)
            {
                case MirGridType.Inventory:
                    fromCell = InventoryDialog.GetCell(p.IDFrom) ?? BeltDialog.GetCell(p.IDFrom);
                    break;
                case MirGridType.Storage:
                    fromCell = StorageDialog.GetCell(p.IDFrom);
                    break;
                case MirGridType.Equipment:
                    fromCell = CharacterDialog.GetCell(p.IDFrom);
                    break;
                case MirGridType.Trade:
                    fromCell = TradeDialog.GetCell(p.IDFrom);
                    break;
                case MirGridType.Fishing:
                    fromCell = FishingDialog.GetCell(p.IDFrom);
                    break;
                case MirGridType.HeroEquipment:
                    fromCell = HeroDialog.GetCell(p.IDFrom);
                    break;
                case MirGridType.HeroInventory:
                    fromCell = HeroInventoryDialog.GetCell(p.IDFrom) ?? HeroBeltDialog.GetCell(p.IDFrom);
                    break;
                default:
                    return;
            }

            switch (p.GridTo)
            {
                case MirGridType.Inventory:
                    toCell = InventoryDialog.GetCell(p.IDTo) ?? BeltDialog.GetCell(p.IDTo);
                    break;
                case MirGridType.Storage:
                    toCell = StorageDialog.GetCell(p.IDTo);
                    break;
                case MirGridType.Equipment:
                    toCell = CharacterDialog.GetCell(p.IDTo);
                    break;
                case MirGridType.Trade:
                    toCell = TradeDialog.GetCell(p.IDTo);
                    break;
                case MirGridType.Fishing:
                    toCell = FishingDialog.GetCell(p.IDTo);
                    break;
                case MirGridType.HeroEquipment:
                    toCell = HeroDialog.GetCell(p.IDTo);
                    break;
                case MirGridType.HeroInventory:
                    toCell = HeroInventoryDialog.GetCell(p.IDTo) ?? HeroBeltDialog.GetCell(p.IDTo);
                    break;
                default:
                    return;
            }

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (p.GridFrom == MirGridType.Trade || p.GridTo == MirGridType.Trade)
                TradeDialog.ChangeLockState(false);

            if (!p.Success) return;
            if (fromCell.Item.Count <= toCell.Item.Info.StackSize - toCell.Item.Count)
            {
                toCell.Item.Count += fromCell.Item.Count;
                fromCell.Item = null;
            }
            else
            {
                fromCell.Item.Count -= (ushort)(toCell.Item.Info.StackSize - toCell.Item.Count);
                toCell.Item.Count = toCell.Item.Info.StackSize;
            }

            User.RefreshStats();
        }
        private void RemoveItem(S.RemoveItem p)
        {
            MirItemCell toCell;

            int index = -1;
            MirItemCell fromCell = null;
            for (int i = 0; i < MapObject.User.Equipment.Length; i++)
            {
                if (MapObject.User.Equipment[i] == null || MapObject.User.Equipment[i].UniqueID != p.UniqueID) continue;
                index = i;
                fromCell = CharacterDialog.Grid[index];
                break;
            }

            if (index == -1 && Hero != null)
            {
                for (int i = 0; i < MapObject.Hero.Equipment.Length; i++)
                {
                    if (MapObject.Hero.Equipment[i] == null || MapObject.Hero.Equipment[i].UniqueID != p.UniqueID) continue;
                    index = i;
                    fromCell = HeroDialog.Grid[index];
                    break;
                }
            }

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];
                    break;
                case MirGridType.Storage:
                    toCell = StorageDialog.Grid[p.To];
                    break;
                case MirGridType.HeroInventory:
                    toCell = p.To < User.HeroBeltIdx ? HeroBeltDialog.Grid[p.To] : HeroInventoryDialog.Grid[p.To - User.HeroBeltIdx];
                    break;
                default:
                    return;
            }

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            CharacterDuraPanel.GetCharacterDura();
            if (p.Grid == MirGridType.HeroInventory)
                Hero.RefreshStats();
            else
                User.RefreshStats();

        }
        private void RemoveSlotItem(S.RemoveSlotItem p)
        {
            MirItemCell fromCell;
            MirItemCell toCell;

            switch (p.Grid)
            {
                case MirGridType.Socket:
                    fromCell = SocketDialog.GetCell(p.UniqueID);
                    break;
                case MirGridType.Mount:
                    fromCell = MountDialog.GetCell(p.UniqueID);
                    break;
                case MirGridType.Fishing:
                    fromCell = FishingDialog.GetCell(p.UniqueID);
                    break;
                default:
                    return;
            }

            switch (p.GridTo)
            {
                case MirGridType.Inventory:
                    toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];
                    break;
                case MirGridType.Storage:
                    toCell = StorageDialog.Grid[p.To];
                    break;
                default:
                    return;
            }

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            CharacterDuraPanel.GetCharacterDura();
            User.RefreshStats();
        }
        private void TakeBackItem(S.TakeBackItem p)
        {
            MirItemCell fromCell = StorageDialog.Grid[p.From];

            MirItemCell toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();
            CharacterDuraPanel.GetCharacterDura();
        }
        private void StoreItem(S.StoreItem p)
        {
            MirItemCell fromCell = p.From < User.BeltIdx ? BeltDialog.Grid[p.From] : InventoryDialog.Grid[p.From - User.BeltIdx];

            MirItemCell toCell = StorageDialog.Grid[p.To];

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();
        }
        private void DepositRefineItem(S.DepositRefineItem p)
        {
            MirItemCell fromCell = p.From < User.BeltIdx ? BeltDialog.Grid[p.From] : InventoryDialog.Grid[p.From - User.BeltIdx];

            MirItemCell toCell = RefineDialog.Grid[p.To];

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();
        }

        private void RetrieveRefineItem(S.RetrieveRefineItem p)
        {
            MirItemCell fromCell = RefineDialog.Grid[p.From];
            MirItemCell toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();
        }

        private void RefineCancel(S.RefineCancel p)
        {
            RefineDialog.RefineReset();
        }

        private void RefineItem(S.RefineItem p)
        {
            RefineDialog.RefineReset();
            for (int i = 0; i < User.Inventory.Length; i++)
            {
                if (User.Inventory[i] != null && User.Inventory[i].UniqueID == p.UniqueID)
                {
                    User.Inventory[i] = null;
                    break;
                }
            }
            NPCDialog.Hide();
        }
        private void DepositTradeItem(S.DepositTradeItem p)
        {
            MirItemCell fromCell = p.From < User.BeltIdx ? BeltDialog.Grid[p.From] : InventoryDialog.Grid[p.From - User.BeltIdx];

            MirItemCell toCell = TradeDialog.Grid[p.To];

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;
            TradeDialog.ChangeLockState(false);

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();
        }
        private void RetrieveTradeItem(S.RetrieveTradeItem p)
        {
            MirItemCell fromCell = TradeDialog.Grid[p.From];
            MirItemCell toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;
            TradeDialog.ChangeLockState(false);

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();
        }
        private void SplitItem(S.SplitItem p)
        {
            Bind(p.Item);

            UserItem[] array;
            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    array = MapObject.User.Inventory;
                    break;
                case MirGridType.Storage:
                    array = Storage;
                    break;
                default:
                    return;
            }

            if (p.Grid == MirGridType.Inventory && (p.Item.Info.Type == ItemType.Potion || p.Item.Info.Type == ItemType.Scroll || p.Item.Info.Type == ItemType.Amulet || (p.Item.Info.Type == ItemType.Script && p.Item.Info.Effect == 1)))
            {
                if (p.Item.Info.Type == ItemType.Potion || p.Item.Info.Type == ItemType.Scroll || (p.Item.Info.Type == ItemType.Script && p.Item.Info.Effect == 1))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (array[i] != null) continue;
                        array[i] = p.Item;
                        User.RefreshStats();
                        return;
                    }
                }
                else if (p.Item.Info.Type == ItemType.Amulet)
                {
                    for (int i = 4; i < GameScene.User.BeltIdx; i++)
                    {
                        if (array[i] != null) continue;
                        array[i] = p.Item;
                        User.RefreshStats();
                        return;
                    }
                }
            }

            for (int i = GameScene.User.BeltIdx; i < array.Length; i++)
            {
                if (array[i] != null) continue;
                array[i] = p.Item;
                User.RefreshStats();
                return;
            }

            for (int i = 0; i < GameScene.User.BeltIdx; i++)
            {
                if (array[i] != null) continue;
                array[i] = p.Item;
                User.RefreshStats();
                return;
            }
        }

        private void SplitItem1(S.SplitItem1 p)
        {
            MirItemCell cell;

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    cell = InventoryDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);
                    break;
                case MirGridType.Storage:
                    cell = StorageDialog.GetCell(p.UniqueID);
                    break;
                default:
                    return;
            }

            if (cell == null) return;

            cell.Locked = false;

            if (!p.Success) return;
            cell.Item.Count -= p.Count;
            User.RefreshStats();
        }
        private void UseItem(S.UseItem p)
        {
            MirItemCell cell = null;
            bool hero = false;

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    cell = InventoryDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);
                    break;
                case MirGridType.HeroInventory:
                    cell = HeroInventoryDialog.GetCell(p.UniqueID) ?? HeroBeltDialog.GetCell(p.UniqueID);
                    hero = true;
                    break;
            }

            if (cell == null) return;

            cell.Locked = false;

            if (!p.Success) return;
            if (cell.Item.Count > 1) cell.Item.Count--;
            else cell.Item = null;
            if (hero)
                Hero.RefreshStats();
            else
                User.RefreshStats();
        }
        private void DropItem(S.DropItem p)
        {
            MirItemCell cell;
            if (p.HeroItem)
            {
                cell = HeroInventoryDialog.GetCell(p.UniqueID) ?? HeroBeltDialog.GetCell(p.UniqueID);
            }
            else
            {
                cell = InventoryDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);
            }


            if (cell == null) return;

            cell.Locked = false;

            if (!p.Success) return;

            if (p.Count == cell.Item.Count)
                cell.Item = null;
            else
                cell.Item.Count -= p.Count;

            if (p.HeroItem)
            {
                Hero.RefreshStats();
            }
            else
            {
                User.RefreshStats();
            }

        }

        private void TakeBackHeroItem(S.TakeBackHeroItem p)
        {
            MirItemCell fromCell = p.From < User.HeroBeltIdx ? HeroBeltDialog.Grid[p.From] : HeroInventoryDialog.Grid[p.From - User.HeroBeltIdx];

            MirItemCell toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();
            Hero.RefreshStats();
            CharacterDuraPanel.GetCharacterDura();
        }

        private void TransferHeroItem(S.TransferHeroItem p)
        {
            MirItemCell fromCell = p.From < User.BeltIdx ? BeltDialog.Grid[p.From] : InventoryDialog.Grid[p.From - User.BeltIdx];

            MirItemCell toCell = p.To < User.HeroBeltIdx ? HeroBeltDialog.Grid[p.To] : HeroInventoryDialog.Grid[p.To - User.HeroBeltIdx];

            if (toCell == null || fromCell == null) return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success) return;
            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();
            Hero.RefreshStats();
            CharacterDuraPanel.GetCharacterDura();
        }

        private void MountUpdate(S.MountUpdate p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob))
            {
                if (ob is PlayerObject player)
                    player.MountUpdate(p);
            }

            if (p.ObjectID != User.ObjectID) return;

            CanRun = false;

            User.RefreshStats();

            MountDialog.RefreshDialog();
            Redraw();
        }

        private void TransformUpdate(S.TransformUpdate p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob))
                if (ob is PlayerObject player)
                    player.TransformType = p.TransformType;
        }

        private void FishingUpdate(S.FishingUpdate p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob) && ob is PlayerObject player)
                player.FishingUpdate(p);

            if (p.ObjectID != User.ObjectID) return;

            FishingStatusDialog.ProgressPercent = p.ProgressPercent;
            FishingStatusDialog.ChancePercent = p.ChancePercent;
            FishingStatusDialog.ChanceLabel.Text = string.Format("{0}%", FishingStatusDialog.ChancePercent);

            if (p.Fishing)
                FishingStatusDialog.Show();
            else
                FishingStatusDialog.Hide();

            Redraw();
        }

        private void CompleteQuest(S.CompleteQuest p)
        {
            User.CompletedQuests = p.CompletedQuests;
        }

        private void ShareQuest(S.ShareQuest p)
        {
            ClientQuestInfo quest = GameScene.QuestInfoList.FirstOrDefault(e => e.Index == p.QuestIndex);

            if (quest == null) return;

            MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerShareQuestRequest), p.SharerName), MirMessageBoxButtons.YesNo);

            messageBox.YesButton.Click += (o, e) => Network.Enqueue(new C.AcceptQuest { NPCIndex = 0, QuestIndex = quest.Index });

            messageBox.Show();
        }

        private void ChangeQuest(S.ChangeQuest p)
        {
            switch (p.QuestState)
            {
                case QuestState.Add:
                    User.CurrentQuests.Add(p.Quest);

                    foreach (ClientQuestProgress quest in User.CurrentQuests)
                        BindQuest(quest);
                    if (Settings.TrackedQuests.Contains(p.Quest.Id))
                    {
                        GameScene.Scene.QuestTrackingDialog.AddQuest(p.Quest, true);
                    }

                    if (p.TrackQuest)
                    {
                        GameScene.Scene.QuestTrackingDialog.AddQuest(p.Quest);
                    }

                    break;
                case QuestState.Update:
                    for (int i = 0; i < User.CurrentQuests.Count; i++)
                    {
                        if (User.CurrentQuests[i].Id != p.Quest.Id) continue;

                        User.CurrentQuests[i] = p.Quest;
                    }

                    foreach (ClientQuestProgress quest in User.CurrentQuests)
                        BindQuest(quest);

                    break;
                case QuestState.Remove:

                    for (int i = User.CurrentQuests.Count - 1; i >= 0; i--)
                    {
                        if (User.CurrentQuests[i].Id != p.Quest.Id) continue;

                        User.CurrentQuests.RemoveAt(i);
                    }

                    GameScene.Scene.QuestTrackingDialog.RemoveQuest(p.Quest);

                    break;
            }

            GameScene.Scene.QuestTrackingDialog.DisplayQuests();

            if (Scene.QuestListDialog.Visible)
            {
                Scene.QuestListDialog.DisplayInfo();
            }

            if (Scene.QuestLogDialog.Visible)
            {
                Scene.QuestLogDialog.DisplayQuests();
            }
        }

        private void PlayerUpdate(S.PlayerUpdate p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob) && ob is PlayerObject player)
                player.Update(p);
        }

        private void PlayerInspect(S.PlayerInspect p)
        {
            InspectDialog.Items = p.Equipment;

            InspectDialog.Name = p.Name;
            InspectDialog.GuildName = p.GuildName;
            InspectDialog.GuildRank = p.GuildRank;
            InspectDialog.Class = p.Class;
            InspectDialog.Gender = p.Gender;
            InspectDialog.Hair = p.Hair;
            InspectDialog.Level = p.Level;
            InspectDialog.LoverName = p.LoverName;
            InspectDialog.AllowObserve = p.AllowObserve;

            InspectDialog.RefreshInferface(p.IsHero);
            InspectDialog.Show();
        }
        private void LogOutSuccess(S.LogOutSuccess p)
        {
            for (int i = 0; i <= 3; i++)//Fix for orbs sound
                SoundManager.StopSound(20000 + 126 * 10 + 5 + i);

            User = null;
            if (Settings.Resolution != 1024)
            {
                CMain.SetResolution(1024, 768);
            }

            ActiveScene = new SelectScene(p.Characters);

            Dispose();
        }
        private void LogOutFailed(S.LogOutFailed p)
        {
            Enabled = true;
        }

        private void ReturnToLogin(S.ReturnToLogin p)
        {
            User = null;
            if (Settings.Resolution != 1024)
                CMain.SetResolution(1024, 768);

            ActiveScene = new LoginScene();
            Dispose();
            MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ObservedPlayerLoggedOff));
        }

        private void TimeOfDay(S.TimeOfDay p)
        {
            Lights = p.Lights;
            switch (Lights)
            {
                case LightSetting.Day:
                case LightSetting.Normal:
                    MiniMapDialog.LightSetting.Index = 2093;
                    break;
                case LightSetting.Dawn:
                    MiniMapDialog.LightSetting.Index = 2095;
                    break;
                case LightSetting.Evening:
                    MiniMapDialog.LightSetting.Index = 2094;
                    break;
                case LightSetting.Night:
                    MiniMapDialog.LightSetting.Index = 2092;
                    break;
            }
        }
        private void ChangeAMode(S.ChangeAMode p)
        {
            AMode = p.Mode;

            switch (p.Mode)
            {
                case AttackMode.Peace:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AttackMode_Peace), ChatType.Hint);
                    break;
                case AttackMode.Group:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AttackMode_Group), ChatType.Hint);
                    break;
                case AttackMode.Guild:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AttackMode_Guild), ChatType.Hint);
                    break;
                case AttackMode.EnemyGuild:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AttackMode_EnemyGuild), ChatType.Hint);
                    break;
                case AttackMode.RedBrown:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AttackMode_RedBrown), ChatType.Hint);
                    break;
                case AttackMode.All:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AttackMode_All), ChatType.Hint);
                    break;
            }
        }
        private void ChangePMode(S.ChangePMode p)
        {
            PMode = p.Mode;
            switch (p.Mode)
            {
                case PetMode.Both:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PetMode_Both), ChatType.Hint);
                    break;
                case PetMode.MoveOnly:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PetMode_MoveOnly), ChatType.Hint);
                    break;
                case PetMode.AttackOnly:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PetMode_AttackOnly), ChatType.Hint);
                    break;
                case PetMode.None:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PetMode_None), ChatType.Hint);
                    break;
                case PetMode.FocusMasterTarget:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PetMode_FocusMasterTarget), ChatType.Hint);
                    break;
            }
        }

        private void ObjectItem(S.ObjectItem p)
        {
            ItemObject ob = new ItemObject(p.ObjectID);
            ob.Load(p);
            /*
            string[] Warnings = new string[] {"HeroNecklace","AdamantineNecklace","8TrigramWheel","HangMaWheel","BaekTaGlove","SpiritReformer","BokMaWheel","BoundlessRing","ThunderRing","TaeGukRing","OmaSpiritRing","NobleRing"};
            if (Warnings.Contains(p.Name))
            {
                ChatDialog.ReceiveChat(string.Format("{0} at {1}", p.Name, p.Location), ChatType.Hint);
            }
            */
        }
        private void ObjectGold(S.ObjectGold p)
        {
            ItemObject ob = new ItemObject(p.ObjectID);
            ob.Load(p);
        }
        private void GainedItem(S.GainedItem p)
        {
            Bind(p.Item);
            AddItem(p.Item);
            User.RefreshStats();

            OutputMessage(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.YouGainedItem), p.Item.FriendlyName));
        }
        private void GainedQuestItem(S.GainedQuestItem p)
        {
            Bind(p.Item);
            AddQuestItem(p.Item);
        }

        private void GainedGold(S.GainedGold p)
        {
            if (p.Gold == 0) return;

            Gold += p.Gold;
            SoundManager.PlaySound(SoundList.Gold);
            OutputMessage(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.YouGainedGold), p.Gold, GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Gold)));
        }
        private void LoseGold(S.LoseGold p)
        {
            Gold -= p.Gold;
            SoundManager.PlaySound(SoundList.Gold);
        }
        private void GainedCredit(S.GainedCredit p)
        {
            if (p.Credit == 0) return;

            Credit += p.Credit;
            SoundManager.PlaySound(SoundList.Gold);
            OutputMessage(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.YouGainedGold), p.Credit, GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Credit)));
        }
        private void LoseCredit(S.LoseCredit p)
        {
            Credit -= p.Credit;
            SoundManager.PlaySound(SoundList.Gold);
        }
        private void ObjectMonster(S.ObjectMonster p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob) && ob is MonsterObject mob)
            {
                mob.Load(p, true);
                return;
            }

            mob = new MonsterObject(p.ObjectID);
            mob.Load(p, false);
        }

        private void ObjectAttack(S.ObjectAttack p)
        {
            if (p.ObjectID == User.ObjectID && !Observing) return;

            QueuedAction action = null;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                if (ob.Race == ObjectType.Player)
                {
                    action = new QueuedAction { Action = MirAction.Attack1, Direction = p.Direction, Location = p.Location, Params = new List<object>() };
                }
                else
                {
                    switch (p.Type)
                    {
                        default:
                            {
                                action = new QueuedAction { Action = MirAction.Attack1, Direction = p.Direction, Location = p.Location, Params = new List<object>() };
                                break;
                            }
                        case 1:
                            {
                                action = new QueuedAction { Action = MirAction.Attack2, Direction = p.Direction, Location = p.Location, Params = new List<object>() };
                                break;
                            }
                        case 2:
                            {
                                action = new QueuedAction { Action = MirAction.Attack3, Direction = p.Direction, Location = p.Location, Params = new List<object>() };
                                break;
                            }
                        case 3:
                            {
                                action = new QueuedAction { Action = MirAction.Attack4, Direction = p.Direction, Location = p.Location, Params = new List<object>() };
                                break;
                            }
                        case 4:
                            {
                                action = new QueuedAction { Action = MirAction.Attack5, Direction = p.Direction, Location = p.Location, Params = new List<object>() };
                                break;
                            }
                    }
                }
                action.Params.Add(p.Spell);
                action.Params.Add(p.Level);
                ob.ActionFeed.Add(action);
            }
        }
        private void Struck(S.Struck p)
        {
            LogTime = CMain.Time + Globals.LogDelay;

            NextRunTime = CMain.Time + 2500;
            User.BlizzardStopTime = 0;
            User.ClearMagic();
            if (User.ReincarnationStopTime > CMain.Time)
                Network.Enqueue(new C.CancelReincarnation { });

            MirDirection dir = User.Direction;
            Point location = User.CurrentLocation;

            for (int i = 0; i < User.ActionFeed.Count; i++)
                if (User.ActionFeed[i].Action == MirAction.Struck) return;


            if (User.ActionFeed.Count > 0)
            {
                dir = User.ActionFeed[User.ActionFeed.Count - 1].Direction;
                location = User.ActionFeed[User.ActionFeed.Count - 1].Location;
            }

            if (User.Buffs.Any(a => a == BuffType.EnergyShield))
            {
                for (int j = 0; j < User.Effects.Count; j++)
                {
                    BuffEffect effect = null;
                    effect = User.Effects[j] as BuffEffect;

                    if (effect != null && effect.BuffType == BuffType.EnergyShield)
                    {
                        effect.Clear();
                        effect.Remove();

                        User.Effects.Add(effect = new BuffEffect(Libraries.Magic2, 1890, 6, 600, User, true, BuffType.EnergyShield) { Repeat = false });
                        SoundManager.PlaySound(20000 + (ushort)Spell.EnergyShield * 10 + 1);

                        effect.Complete += (o, e) =>
                        {
                            User.Effects.Add(new BuffEffect(Libraries.Magic2, 1900, 2, 800, User, true, BuffType.EnergyShield) { Repeat = true });
                        };


                        break;
                    }
                }
            }

            QueuedAction action = new QueuedAction
            {
                Action = MirAction.Struck,
                Direction = dir,
                Location = location,
                Params = new List<object>()
            };
            action.Params.Add(p.AttackerID);

            int weapon = -2;
            if (MapControl.Objects.TryGetValue(p.AttackerID, out MapObject attacker) &&
                attacker.Race == ObjectType.Player)
            {
                PlayerObject player = (PlayerObject)attacker;
                weapon = player.Weapon;
                if (player.Class == MirClass.Assassin && weapon > -1)
                    weapon = 1;
            }
            action.Params.Add(weapon);
            User.ActionFeed.Add(action);

        }
        private void ObjectStruck(S.ObjectStruck p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                if (ob.SkipFrames) return;
                if (ob.ActionFeed.Count > 0 && ob.ActionFeed[ob.ActionFeed.Count - 1].Action == MirAction.Struck) return;

                if (ob.Race == ObjectType.Player)
                    ((PlayerObject)ob).BlizzardStopTime = 0;
                QueuedAction action = new QueuedAction
                {
                    Action = MirAction.Struck,
                    Direction = p.Direction,
                    Location = p.Location,
                    Params = new List<object>()
                };
                action.Params.Add(p.AttackerID);

                int weapon = -2;
                if (MapControl.Objects.TryGetValue(p.AttackerID, out MapObject attacker) &&
                    attacker.Race == ObjectType.Player)
                {
                    PlayerObject player = (PlayerObject)attacker;
                    weapon = player.Weapon;
                    if (player.Class == MirClass.Assassin && weapon > -1)
                        weapon = 1;
                }
                action.Params.Add(weapon);
                ob.ActionFeed.Add(action);

                if (ob.Buffs.Any(a => a == BuffType.EnergyShield))
                {
                    for (int j = 0; j < ob.Effects.Count; j++)
                    {
                        BuffEffect effect = null;
                        effect = ob.Effects[j] as BuffEffect;

                        if (effect != null && effect.BuffType == BuffType.EnergyShield)
                        {
                            effect.Clear();
                            effect.Remove();

                            ob.Effects.Add(effect = new BuffEffect(Libraries.Magic2, 1890, 6, 600, ob, true, BuffType.EnergyShield) { Repeat = false });
                            SoundManager.PlaySound(20000 + (ushort)Spell.EnergyShield * 10 + 1);

                            effect.Complete += (o, e) =>
                            {
                                ob.Effects.Add(new BuffEffect(Libraries.Magic2, 1900, 2, 800, ob, true, BuffType.EnergyShield) { Repeat = true });
                            };

                            break;
                        }
                    }
                }
            }
        }

        private void DamageIndicator(S.DamageIndicator p)
        {
            if (Settings.DisplayDamage)
            {
                if (MapControl.Objects.TryGetValue(p.ObjectID, out var obj))
                {
                    if (obj.Damages.Count >= 10) return;

                    switch (p.Type)
                    {
                        case DamageType.Hit: //add damage level colours
                            obj.Damages.Add(new Damage(p.Damage.ToString("#,##0"), 1000, obj.Race == ObjectType.Player ? Color.Red : Color.White, 50));
                            break;
                        case DamageType.Miss:
                            obj.Damages.Add(new Damage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Miss), 1200, obj.Race == ObjectType.Player ? Color.LightCoral : Color.LightGray, 50));
                            break;
                        case DamageType.Critical:
                            obj.Damages.Add(
                                new Damage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Crit), 1000, obj.Race == ObjectType.Player ? Color.DarkRed : Color.DarkRed, 50) { Offset = 15 });
                            break;
                    }
                }
            }
        }

        private void DuraChanged(S.DuraChanged p)
        {
            UserItem item = null;
            for (int i = 0; i < User.Inventory.Length; i++)
                if (User.Inventory[i] != null && User.Inventory[i].UniqueID == p.UniqueID)
                {
                    item = User.Inventory[i];
                    break;
                }


            if (item == null)
                for (int i = 0; i < User.Equipment.Length; i++)
                {
                    if (User.Equipment[i] != null && User.Equipment[i].UniqueID == p.UniqueID)
                    {
                        item = User.Equipment[i];
                        break;
                    }
                    if (User.Equipment[i] != null && User.Equipment[i].Slots != null)
                    {
                        for (int j = 0; j < User.Equipment[i].Slots.Length; j++)
                        {
                            if (User.Equipment[i].Slots[j] != null && User.Equipment[i].Slots[j].UniqueID == p.UniqueID)
                            {
                                item = User.Equipment[i].Slots[j];
                                break;
                            }
                        }

                        if (item != null) break;
                    }
                }

            if (item == null) return;

            item.CurrentDura = p.CurrentDura;

            if (item.CurrentDura == 0)
            {
                User.RefreshStats();
                switch (item.Info.Type)
                {
                    case ItemType.Mount:
                        ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.FriendlyNoLongerLoyal), item.Info.FriendlyName), ChatType.System);
                        break;
                    default:
                        ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.FriendlyDuraDroppedToZero), item.Info.FriendlyName), ChatType.System);
                        break;
                }

            }

            if (HoverItem == item)
            {
                DisposeItemLabel();
                CreateItemLabel(item);
            }

            CharacterDuraPanel.UpdateCharacterDura(item);
        }
        private void HealthChanged(S.HealthChanged p)
        {
            User.HP = p.HP;
            User.MP = p.MP;

            User.PercentHealth = (byte)(User.HP / (float)User.Stats[Stat.HP] * 100);
        }
        private void HeroHealthChanged(S.HeroHealthChanged p)
        {
            Hero.HP = p.HP;
            Hero.MP = p.MP;

            Hero.PercentHealth = (byte)(Hero.HP / (float)Hero.Stats[Stat.HP] * 100);
            Hero.PercentMana = (byte)(Hero.MP / (float)Hero.Stats[Stat.MP] * 100);
        }

        private void DeleteQuestItem(S.DeleteQuestItem p)
        {
            for (int i = 0; i < User.QuestInventory.Length; i++)
            {
                UserItem item = User.QuestInventory[i];

                if (item == null || item.UniqueID != p.UniqueID) continue;

                if (item.Count == p.Count)
                    User.QuestInventory[i] = null;
                else
                    item.Count -= p.Count;
                break;
            }
        }

        private void DeleteItem(S.DeleteItem p)
        {
            UserObject actor = null;
            for (int i = 0; i < User.Inventory.Length; i++)
            {
                if (actor != null) break;
                UserItem item = User.Inventory[i];

                if (item != null && item.Slots.Length > 0)
                {
                    for (int j = 0; j < item.Slots.Length; j++)
                    {
                        UserItem slotItem = item.Slots[j];

                        if (slotItem == null || slotItem.UniqueID != p.UniqueID) continue;

                        if (slotItem.Count == p.Count)
                            item.Slots[j] = null;
                        else
                            slotItem.Count -= p.Count;
                        actor = User;
                        break;
                    }
                }

                if (item == null || item.UniqueID != p.UniqueID) continue;

                if (item.Count == p.Count)
                    User.Inventory[i] = null;
                else
                    item.Count -= p.Count;
                actor = User;
            }

            if (actor == null)
            {
                for (int i = 0; i < User.Equipment.Length; i++)
                {
                    if (actor != null) break;
                    UserItem item = User.Equipment[i];

                    if (item != null && item.Slots.Length > 0)
                    {
                        for (int j = 0; j < item.Slots.Length; j++)
                        {
                            UserItem slotItem = item.Slots[j];

                            if (slotItem == null || slotItem.UniqueID != p.UniqueID) continue;

                            if (slotItem.Count == p.Count)
                                item.Slots[j] = null;
                            else
                                slotItem.Count -= p.Count;
                            actor = User;
                            break;
                        }
                    }

                    if (item == null || item.UniqueID != p.UniqueID) continue;

                    if (item.Count == p.Count)
                        User.Equipment[i] = null;
                    else
                        item.Count -= p.Count;
                    actor = User;
                }
            }

            if (Hero != null && actor == null)
            {
                for (int i = 0; i < Hero.Inventory.Length; i++)
                {
                    if (actor != null) break;
                    UserItem item = Hero.Inventory[i];

                    if (item != null && item.Slots.Length > 0)
                    {
                        for (int j = 0; j < item.Slots.Length; j++)
                        {
                            UserItem slotItem = item.Slots[j];

                            if (slotItem == null || slotItem.UniqueID != p.UniqueID) continue;

                            if (slotItem.Count == p.Count)
                                item.Slots[j] = null;
                            else
                                slotItem.Count -= p.Count;
                            actor = Hero;
                            break;
                        }
                    }

                    if (item == null || item.UniqueID != p.UniqueID) continue;

                    if (item.Count == p.Count)
                        User.Inventory[i] = null;
                    else
                        item.Count -= p.Count;
                    actor = Hero;
                }

                if (actor == null)
                {
                    for (int i = 0; i < Hero.Equipment.Length; i++)
                    {
                        if (actor != null) break;
                        UserItem item = Hero.Equipment[i];

                        if (item != null && item.Slots.Length > 0)
                        {
                            for (int j = 0; j < item.Slots.Length; j++)
                            {
                                UserItem slotItem = item.Slots[j];

                                if (slotItem == null || slotItem.UniqueID != p.UniqueID) continue;

                                if (slotItem.Count == p.Count)
                                    item.Slots[j] = null;
                                else
                                    slotItem.Count -= p.Count;
                                actor = Hero;
                                break;
                            }
                        }

                        if (item == null || item.UniqueID != p.UniqueID) continue;

                        if (item.Count == p.Count)
                            User.Equipment[i] = null;
                        else
                            item.Count -= p.Count;
                        actor = Hero;
                    }
                }
            }

            if (actor == null)
            {
                for (int i = 0; i < Storage.Length; i++)
                {
                    var item = Storage[i];
                    if (item == null || item.UniqueID != p.UniqueID) continue;

                    if (item.Count == p.Count)
                        Storage[i] = null;
                    else
                        item.Count -= p.Count;
                    break;
                }
            }
            actor?.RefreshStats();
        }
        private void Death(S.Death p)
        {
            User.Dead = true;

            User.ActionFeed.Add(new QueuedAction { Action = MirAction.Die, Direction = p.Direction, Location = p.Location });
            ShowReviveMessage = true;

            LogTime = 0;
        }
        private void ObjectDied(S.ObjectDied p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                switch (p.Type)
                {
                    default:
                        ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Die, Direction = p.Direction, Location = p.Location });
                        ob.Dead = true;
                        break;
                    case 1:
                        MapControl.Effects.Add(new Effect(Libraries.Magic2, 690, 10, 1000, ob.CurrentLocation));
                        ob.Remove();
                        break;
                    case 2:
                        SoundManager.PlaySound(20000 + (ushort)Spell.DarkBody * 10 + 1);
                        MapControl.Effects.Add(new Effect(Libraries.Magic2, 2600, 10, 1200, ob.CurrentLocation));
                        ob.Remove();
                        break;
                }
            }
        }
        private void ColourChanged(S.ColourChanged p)
        {
            User.NameColour = p.NameColour;
        }
        private void ObjectColourChanged(S.ObjectColourChanged p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.NameColour = p.NameColour;
        }

        private void ObjectGuildNameChanged(S.ObjectGuildNameChanged p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob) && ob is PlayerObject obPlayer)
                obPlayer.GuildName = p.GuildName;
        }

        private void GainExperience(S.GainExperience p)
        {
            OutputMessage(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.ExperienceGained), p.Amount));
            MapObject.User.Experience += p.Amount;
        }

        private void GainHeroExperience(S.GainHeroExperience p)
        {
            OutputMessage(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.HeroExperienceGained), p.Amount));
            MapObject.Hero.Experience += p.Amount;
        }
        private void LevelChanged(S.LevelChanged p)
        {
            User.Level = p.Level;
            User.Experience = p.Experience;
            User.MaxExperience = p.MaxExperience;
            User.RefreshStats();
            OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LevelUp));
            User.Effects.Add(new Effect(Libraries.Magic2, 1200, 20, 2000, User));
            SoundManager.PlaySound(SoundList.LevelUp);
            ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LevelUp), ChatType.LevelUp);
        }
        private void HeroLevelChanged(S.HeroLevelChanged p)
        {
            Hero.Level = p.Level;
            Hero.Experience = p.Experience;
            Hero.MaxExperience = p.MaxExperience;
            Hero.RefreshStats();
            Hero.Effects.Add(new Effect(Libraries.Magic2, 1200, 20, 2000, User));
            SoundManager.PlaySound(SoundList.LevelUp);
            MainDialog.HeroInfoPanel.Update();
        }
        private void ObjectLeveled(S.ObjectLeveled p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                ob.Effects.Add(new Effect(Libraries.Magic2, 1180, 16, 2500, ob));
                SoundManager.PlaySound(SoundList.LevelUp);
            }
        }
        private void ObjectHarvest(S.ObjectHarvest p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Harvest, Direction = ob.Direction, Location = ob.CurrentLocation });
        }

        private void ObjectHarvested(S.ObjectHarvested p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Skeleton, Direction = ob.Direction, Location = ob.CurrentLocation });
        }

        private void ObjectNPC(S.ObjectNPC p)
        {
            NPCObject ob = new NPCObject(p.ObjectID);
            ob.Load(p);
        }
        private void NPCResponse(S.NPCResponse p)
        {
            NPCTime = 0;
            NPCDialog.BigButtons.Clear();
            NPCDialog.BigButtonDialog.Hide();
            NPCDialog.NewText(p.Page);

            if (p.Page.Count > 0 || NPCDialog.BigButtons.Count > 0)
                NPCDialog.Show();
            else
                NPCDialog.Hide();

            NPCGoodsDialog.Hide();
            NPCSubGoodsDialog.Hide();
            NPCCraftGoodsDialog.Hide();
            NPCDropDialog.Hide();
            StorageDialog.Hide();
            NPCAwakeDialog.Hide();
            RefineDialog.Hide();
            StorageDialog.Hide();
            TrustMerchantDialog.Hide();
            QuestListDialog.Hide();
        }

        private void NPCUpdate(S.NPCUpdate p)
        {
            GameScene.NPCID = p.NPCID; //Updates the client with the correct NPC ID if it's manually called from the client
        }

        private void NPCImageUpdate(S.NPCImageUpdate p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob) && ob is NPCObject npc && ob.Race == ObjectType.Merchant)
            {
                npc.Image = p.Image;
                npc.Colour = p.Colour;
                npc.LoadLibrary();
            }
        }

        private void DefaultNPC(S.DefaultNPC p)
        {
            GameScene.DefaultNPCID = p.ObjectID; //Updates the client with the correct Default NPC ID
        }


        private void ObjectHide(S.ObjectHide p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Hide, Direction = ob.Direction, Location = ob.CurrentLocation });
        }

        private void ObjectShow(S.ObjectShow p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Show, Direction = ob.Direction, Location = ob.CurrentLocation });
        }

        private void Poisoned(S.Poisoned p)
        {
            var previousPoisons = User.Poison;

            User.Poison = p.Poison;
            if (p.Poison.HasFlag(PoisonType.Stun) || p.Poison.HasFlag(PoisonType.Dazed) || p.Poison.HasFlag(PoisonType.Frozen) || p.Poison.HasFlag(PoisonType.Paralysis) || p.Poison.HasFlag(PoisonType.LRParalysis))
            {
                User.ClearMagic();
            }

            if (previousPoisons.HasFlag(PoisonType.Blindness) && !User.Poison.HasFlag(PoisonType.Blindness))
            {
                User.BlindCount = 0;
            }
        }

        private void ObjectPoisoned(S.ObjectPoisoned p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.Poison = p.Poison;
        }

        private void MapChanged(S.MapChanged p)
        {
            var isCurrentMap = (MapControl.Index == p.MapIndex);

            if (isCurrentMap)
                MapControl.ResetMap();
            else
            {
                MapControl.Index = p.MapIndex;
                MapControl.FileName = Path.Combine(Settings.MapPath, p.FileName + ".map");
                MapControl.Title = p.Title;
                MapControl.MiniMap = p.MiniMap;
                MapControl.BigMap = p.BigMap;
                MapControl.Lights = p.Lights;
                MapControl.MapDarkLight = p.MapDarkLight;
                MapControl.Music = p.Music;
                MapControl.Weather = p.Weather;
                MapControl.LoadMap();
            }

            MapControl.NextAction = 0;
            Scene.MapControl.AutoPath = false;
            User.CurrentLocation = p.Location;
            User.MapLocation = p.Location;
            MapControl.AddObject(User);

            User.Direction = p.Direction;

            User.QueuedAction = null;
            User.ActionFeed.Clear();
            User.ClearMagic();
            User.SetAction();

            GameScene.CanRun = false;

            MapControl.FloorValid = false;
            MapControl.InputDelay = CMain.Time + 400;

            MapControl.UpdateWeather();
        }
        private void ObjectTeleportOut(S.ObjectTeleportOut p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                Effect effect = null;

                bool playDefaultSound = true;

                switch (p.Type)
                {
                    case 1: //Yimoogi
                        {
                            effect = new Effect(Libraries.Magic2, 1300, 10, 500, ob.CurrentLocation);
                            break;
                        }
                    case 2: //RedFoxman
                        {
                            effect = new Effect(Libraries.Monsters[(ushort)Monster.RedFoxman], 243, 10, 500, ob.CurrentLocation);
                            break;
                        }
                    case 4: //MutatedManWorm
                        {
                            effect = new Effect(Libraries.Monsters[(ushort)Monster.MutatedManworm], 272, 6, 500, ob.CurrentLocation);

                            SoundManager.PlaySound(((ushort)Monster.MutatedManworm) * 10 + 7);
                            playDefaultSound = false;
                            break;
                        }
                    case 5: //WitchDoctor
                        {
                            effect = new Effect(Libraries.Monsters[(ushort)Monster.WitchDoctor], 328, 20, 1000, ob.CurrentLocation);
                            SoundManager.PlaySound(((ushort)Monster.WitchDoctor) * 10 + 7);
                            playDefaultSound = false;
                            break;
                        }
                    case 6: //TurtleKing
                        {
                            effect = new Effect(Libraries.Monsters[(ushort)Monster.TurtleKing], 946, 10, 500, ob.CurrentLocation);
                            break;
                        }
                    case 7: //Mandrill
                        {
                            effect = new Effect(Libraries.Monsters[(ushort)Monster.Mandrill], 280, 10, 1000, ob.CurrentLocation);
                            SoundManager.PlaySound(((ushort)Monster.Mandrill) * 10 + 6);
                            playDefaultSound = false;
                            break;
                        }
                    case 8: //DarkCaptain
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.DarkCaptain], 1224, 10, 1000, ob.CurrentLocation));
                            SoundManager.PlaySound(((ushort)Monster.DarkCaptain) * 10 + 8);
                            playDefaultSound = false;
                            break;
                        }
                    case 9: //Doe
                        {
                            effect = new Effect(Libraries.Monsters[(ushort)Monster.Doe], 208, 10, 1000, ob.CurrentLocation);
                            SoundManager.PlaySound(((ushort)Monster.Doe) * 10 + 7);
                            playDefaultSound = false;
                            break;
                        }
                    case 10: //HornedCommander
                        {
                            MapControl.Effects.Add(effect = new Effect(Libraries.Monsters[(ushort)Monster.HornedCommander], 928, 10, 1000, ob.CurrentLocation));
                            SoundManager.PlaySound(8455);
                            playDefaultSound = false;
                            break;
                        }
                    case 11: //SnowWolfKing
                        {
                            MapControl.Effects.Add(effect = new Effect(Libraries.Monsters[(ushort)Monster.SnowWolfKing], 561, 10, 1000, ob.CurrentLocation));
                            SoundManager.PlaySound(8455);
                            playDefaultSound = false;
                            break;
                        }
                    default:
                        {
                            effect = new Effect(Libraries.Magic, 250, 10, 500, ob.CurrentLocation);
                            break;
                        }
                }

                MapControl.Effects.Add(effect);

                if (playDefaultSound)
                    SoundManager.PlaySound(SoundList.Teleport);
            }
        }
        private void ObjectTeleportIn(S.ObjectTeleportIn p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                bool playDefaultSound = true;

                switch (p.Type)
                {
                    case 1: //Yimoogi
                        {
                            ob.Effects.Add(new Effect(Libraries.Magic2, 1310, 10, 500, ob));
                            break;
                        }
                    case 2: //RedFoxman
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.RedFoxman], 253, 10, 500, ob));
                            break;
                        }
                    case 4: //MutatedManWorm
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.MutatedManworm], 278, 7, 500, ob));
                            SoundManager.PlaySound(((ushort)Monster.MutatedManworm) * 10 + 7);
                            playDefaultSound = false;
                            break;
                        }
                    case 5: //WitchDoctor
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.WitchDoctor], 348, 20, 1000, ob));
                            SoundManager.PlaySound(((ushort)Monster.WitchDoctor) * 10 + 7);
                            playDefaultSound = false;
                            break;
                        }
                    case 6: //TurtleKing
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.TurtleKing], 956, 10, 500, ob));
                            break;
                        }
                    case 7: //Mandrill
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.Mandrill], 290, 10, 1000, ob));
                            SoundManager.PlaySound(((ushort)Monster.Mandrill) * 10 + 6);
                            playDefaultSound = false;
                            break;
                        }
                    case 8: //DarkCaptain
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.DarkCaptain], 1224, 10, 1000, ob));
                            SoundManager.PlaySound(((ushort)Monster.DarkCaptain) * 10 + 9);
                            playDefaultSound = false;
                            break;
                        }
                    case 9: //Doe
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.Doe], 208, 10, 1000, ob));
                            SoundManager.PlaySound(((ushort)Monster.Doe) * 10 + 7);
                            playDefaultSound = false;
                            break;
                        }
                    case 10: //HornedCommander
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.HornedCommander], 928, 10, 1000, ob));
                            SoundManager.PlaySound(8455);
                            playDefaultSound = false;
                            break;
                        }
                    case 11: //SnowWolfKing
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.SnowWolfKing], 571, 10, 1000, ob));
                            SoundManager.PlaySound(8455);
                            playDefaultSound = false;
                            break;
                        }
                    default:
                        {
                            ob.Effects.Add(new Effect(Libraries.Magic, 260, 10, 500, ob));
                            break;
                        }
                }

                if (p.ObjectID == User.ObjectID)
                    User.TargetID = User.LastTargetObjectId;

                if (playDefaultSound)
                    SoundManager.PlaySound(SoundList.Teleport);
            }
        }

        private void TeleportIn()
        {
            User.Effects.Add(new Effect(Libraries.Magic, 260, 10, 500, User));
            SoundManager.PlaySound(SoundList.Teleport);
        }
        private void NPCGoods(S.NPCGoods p)
        {
            for (int i = 0; i < p.List.Count; i++)
            {
                p.List[i].Info = GetItemInfo(p.List[i].ItemIndex);
            }

            NPCRate = p.Rate;
            NPCPanelType = p.Type;
            HideAddedStoreStats = p.HideAddedStats;

            if (!NPCDialog.Visible) return;

            switch (NPCPanelType)
            {
                case PanelType.Buy:
                    NPCGoodsDialog.UsePearls = false;
                    NPCGoodsDialog.NewGoods(p.List);
                    NPCGoodsDialog.Show();
                    break;
                case PanelType.BuySub:
                    NPCSubGoodsDialog.UsePearls = false;
                    NPCSubGoodsDialog.NewGoods(p.List);
                    NPCSubGoodsDialog.Show();
                    break;
                case PanelType.Craft:
                    NPCCraftGoodsDialog.UsePearls = false;
                    NPCCraftGoodsDialog.NewGoods(p.List);
                    NPCCraftGoodsDialog.Show();
                    CraftDialog.Show();
                    break;
            }
        }
        private void NPCPearlGoods(S.NPCPearlGoods p)
        {
            for (int i = 0; i < p.List.Count; i++)
            {
                p.List[i].Info = GetItemInfo(p.List[i].ItemIndex);
            }

            NPCRate = p.Rate;
            NPCPanelType = p.Type;

            if (!NPCDialog.Visible) return;

            NPCGoodsDialog.UsePearls = true;
            NPCGoodsDialog.NewGoods(p.List);
            NPCGoodsDialog.Show();
        }

        private void NPCSell()
        {
            if (!NPCDialog.Visible) return;
            NPCDropDialog.PType = PanelType.Sell;
            NPCDropDialog.Show();
        }
        private void NPCRepair(S.NPCRepair p)
        {
            NPCRate = p.Rate;
            if (!NPCDialog.Visible) return;
            NPCDropDialog.PType = PanelType.Repair;
            NPCDropDialog.Show();
        }
        private void NPCStorage()
        {
            if (NPCDialog.Visible)
                StorageDialog.Show();
        }
        private void NPCRequestInput(S.NPCRequestInput p)
        {
            MirInputBox inputBox = new MirInputBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PleaseEnterRequiredInformation));

            inputBox.OKButton.Click += (o1, e1) =>
            {
                Network.Enqueue(new C.NPCConfirmInput { Value = inputBox.InputTextBox.Text, NPCID = p.NPCID, PageName = p.PageName });
                inputBox.Dispose();
            };
            inputBox.Show();
        }

        private void NPCSRepair(S.NPCSRepair p)
        {
            NPCRate = p.Rate;
            if (!NPCDialog.Visible) return;
            NPCDropDialog.PType = PanelType.SpecialRepair;
            NPCDropDialog.Show();
        }

        private void NPCRefine(S.NPCRefine p)
        {
            NPCRate = p.Rate;
            if (!NPCDialog.Visible) return;
            NPCDropDialog.PType = PanelType.Refine;
            if (p.Refining)
            {
                NPCDropDialog.Hide();
                NPCDialog.Hide();
            }
            else
                NPCDropDialog.Show();
        }

        private void NPCCheckRefine(S.NPCCheckRefine p)
        {
            if (!NPCDialog.Visible) return;
            NPCDropDialog.PType = PanelType.CheckRefine;
            NPCDropDialog.Show();
        }

        private void NPCCollectRefine(S.NPCCollectRefine p)
        {
            if (!NPCDialog.Visible) return;
            NPCDialog.Hide();
        }

        private void NPCReplaceWedRing(S.NPCReplaceWedRing p)
        {
            if (!NPCDialog.Visible) return;
            NPCRate = p.Rate;
            NPCDropDialog.PType = PanelType.ReplaceWedRing;
            NPCDropDialog.Show();
        }


        private void SellItem(S.SellItem p)
        {
            MirItemCell cell = InventoryDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);

            if (cell == null) return;

            cell.Locked = false;

            if (!p.Success) return;

            if (p.Count == cell.Item.Count)
                cell.Item = null;
            else
                cell.Item.Count -= p.Count;

            User.RefreshStats();
        }
        private void RepairItem(S.RepairItem p)
        {
            MirItemCell cell = InventoryDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);

            if (cell == null) return;

            cell.Locked = false;
        }
        private void CraftItem(S.CraftItem p)
        {
            if (!p.Success) return;

            CraftDialog.UpdateCraftCells();
            User.RefreshStats();
        }
        private void ItemRepaired(S.ItemRepaired p)
        {
            UserItem item = null;
            for (int i = 0; i < User.Inventory.Length; i++)
            {
                if (User.Inventory[i] != null && User.Inventory[i].UniqueID == p.UniqueID)
                {
                    item = User.Inventory[i];
                    break;
                }
            }

            if (item == null)
            {
                for (int i = 0; i < User.Equipment.Length; i++)
                {
                    if (User.Equipment[i] != null && User.Equipment[i].UniqueID == p.UniqueID)
                    {
                        item = User.Equipment[i];
                        break;
                    }
                }
            }

            if (Hero != null)
            {
                if (item == null)
                {
                    for (int i = 0; i < Hero.Inventory.Length; i++)
                    {
                        if (Hero.Inventory[i] != null && Hero.Inventory[i].UniqueID == p.UniqueID)
                        {
                            item = Hero.Inventory[i];
                            break;
                        }
                    }
                }

                if (item == null)
                {
                    for (int i = 0; i < Hero.Equipment.Length; i++)
                    {
                        if (Hero.Equipment[i] != null && Hero.Equipment[i].UniqueID == p.UniqueID)
                        {
                            item = Hero.Equipment[i];
                            break;
                        }
                    }
                }
            }

            if (item == null) return;

            item.MaxDura = p.MaxDura;
            item.CurrentDura = p.CurrentDura;

            if (HoverItem == item)
            {
                DisposeItemLabel();
                CreateItemLabel(item);
            }
        }

        private void ItemSlotSizeChanged(S.ItemSlotSizeChanged p)
        {
            UserItem item = null;
            for (int i = 0; i < User.Inventory.Length; i++)
            {
                if (User.Inventory[i] != null && User.Inventory[i].UniqueID == p.UniqueID)
                {
                    item = User.Inventory[i];
                    break;
                }
            }

            if (item == null)
            {
                for (int i = 0; i < User.Equipment.Length; i++)
                {
                    if (User.Equipment[i] != null && User.Equipment[i].UniqueID == p.UniqueID)
                    {
                        item = User.Equipment[i];
                        break;
                    }
                }
            }

            if (Hero != null)
            {
                if (item == null)
                {
                    for (int i = 0; i < Hero.Inventory.Length; i++)
                    {
                        if (Hero.Inventory[i] != null && Hero.Inventory[i].UniqueID == p.UniqueID)
                        {
                            item = Hero.Inventory[i];
                            break;
                        }
                    }
                }

                if (item == null)
                {
                    for (int i = 0; i < Hero.Equipment.Length; i++)
                    {
                        if (Hero.Equipment[i] != null && Hero.Equipment[i].UniqueID == p.UniqueID)
                        {
                            item = Hero.Equipment[i];
                            break;
                        }
                    }
                }
            }

            if (item == null) return;

            item.SetSlotSize(p.SlotSize);
        }

        private void ItemSealChanged(S.ItemSealChanged p)
        {
            UserItem item = null;
            for (int i = 0; i < User.Inventory.Length; i++)
            {
                if (User.Inventory[i] != null && User.Inventory[i].UniqueID == p.UniqueID)
                {
                    item = User.Inventory[i];
                    break;
                }
            }

            if (item == null)
            {
                for (int i = 0; i < User.Equipment.Length; i++)
                {
                    if (User.Equipment[i] != null && User.Equipment[i].UniqueID == p.UniqueID)
                    {
                        item = User.Equipment[i];
                        break;
                    }
                }
            }

            if (Hero != null)
            {
                if (item == null)
                {
                    for (int i = 0; i < Hero.Inventory.Length; i++)
                    {
                        if (Hero.Inventory[i] != null && Hero.Inventory[i].UniqueID == p.UniqueID)
                        {
                            item = Hero.Inventory[i];
                            break;
                        }
                    }
                }

                if (item == null)
                {
                    for (int i = 0; i < Hero.Equipment.Length; i++)
                    {
                        if (Hero.Equipment[i] != null && Hero.Equipment[i].UniqueID == p.UniqueID)
                        {
                            item = Hero.Equipment[i];
                            break;
                        }
                    }
                }
            }

            if (item == null) return;

            item.SealedInfo = new SealedInfo { ExpiryDate = p.ExpiryDate };

            if (HoverItem == item)
            {
                DisposeItemLabel();
                CreateItemLabel(item);
            }
        }

        private void ItemUpgraded(S.ItemUpgraded p)
        {
            UserItem item = null;
            MirGridType grid = MirGridType.Inventory;
            for (int i = 0; i < User.Inventory.Length; i++)
            {
                if (User.Inventory[i] != null && User.Inventory[i].UniqueID == p.Item.UniqueID)
                {
                    item = User.Inventory[i];
                    break;
                }
            }

            if (item == null && Hero != null)
            {
                for (int i = 0; i < Hero.Inventory.Length; i++)
                {
                    if (Hero.Inventory[i] != null && Hero.Inventory[i].UniqueID == p.Item.UniqueID)
                    {
                        item = Hero.Inventory[i];
                        grid = MirGridType.HeroInventory;
                        break;
                    }
                }
            }

            if (item == null) return;

            item.AddedStats.Clear();
            item.AddedStats.Add(p.Item.AddedStats);

            item.MaxDura = p.Item.MaxDura;
            item.RefineAdded = p.Item.RefineAdded;

            switch (grid)
            {
                case MirGridType.Inventory:
                    InventoryDialog.DisplayItemGridEffect(item.UniqueID, 0);
                    break;
                case MirGridType.HeroInventory:
                    HeroInventoryDialog.DisplayItemGridEffect(item.UniqueID, 0);
                    break;
            }


            if (HoverItem == item)
            {
                DisposeItemLabel();
                CreateItemLabel(item);
            }
        }

        private void NewMagic(S.NewMagic p)
        {
            ClientMagic magic = p.Magic;

            UserObject actor = User;
            if (p.Hero)
                actor = Hero;

            actor.Magics.Add(magic);
            actor.RefreshStats();
            foreach (SkillBarDialog Bar in SkillBarDialogs)
            {
                Bar.Update();
            }
        }

        private void RemoveMagic(S.RemoveMagic p)
        {
            User.Magics.RemoveAt(p.PlaceId);
            User.RefreshStats();
            foreach (SkillBarDialog Bar in SkillBarDialogs)
            {
                Bar.Update();
            }
        }

        private void MagicLeveled(S.MagicLeveled p)
        {
            UserObject actor = p.ObjectID == Hero?.ObjectID ? Hero : User;

            for (int i = 0; i < actor.Magics.Count; i++)
            {
                ClientMagic magic = actor.Magics[i];
                if (magic.Spell != p.Spell) continue;

                if (magic.Level != p.Level)
                {
                    magic.Level = p.Level;
                    actor.RefreshStats();
                }

                magic.Experience = p.Experience;
                break;
            }
        }
        private void Magic(S.Magic p)
        {
            User.Spell = p.Spell;
            User.Cast = p.Cast;
            User.TargetID = p.TargetID;
            User.TargetPoint = p.Target;
            User.SpellLevel = p.Level;
            User.SecondaryTargetIDs = p.SecondaryTargetIDs;

            if (!p.Cast) return;

            ClientMagic magic = User.GetMagic(p.Spell);
            magic.CastTime = CMain.Time;
        }

        private void MagicDelay(S.MagicDelay p)
        {
            ClientMagic magic;
            if (p.ObjectID == Hero?.ObjectID)
                magic = Hero.GetMagic(p.Spell);
            else
                magic = User.GetMagic(p.Spell);
            magic.Delay = p.Delay;
        }

        private void MagicCast(S.MagicCast p)
        {
            ClientMagic magic = User.GetMagic(p.Spell);
            magic.CastTime = CMain.Time;
        }

        private void ObjectMagic(S.ObjectMagic p)
        {
            if (p.SelfBroadcast == false && p.ObjectID == User.ObjectID && !Observing) return;

            if (p.ObjectID == Hero?.ObjectID && p.Cast)
            {
                ClientMagic magic = Hero.GetMagic(p.Spell);
                magic.CastTime = CMain.Time;
            }

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                QueuedAction action = new QueuedAction { Action = MirAction.Spell, Direction = p.Direction, Location = p.Location, Params = new List<object>() };
                action.Params.Add(p.Spell);
                action.Params.Add(p.TargetID);
                action.Params.Add(p.Target);
                action.Params.Add(p.Cast);
                action.Params.Add(p.Level);
                action.Params.Add(p.SecondaryTargetIDs);

                ob.ActionFeed.Add(action);
            }
        }

        private void ObjectProjectile(S.ObjectProjectile p)
        {
            MapObject source = MapControl.GetObject(p.Source);

            if (source == null) return;

            switch (p.Spell)
            {
                case Spell.FireBounce:
                    {
                        SoundManager.PlaySound(20000 + (ushort)Spell.GreatFireBall * 10 + 1);

                        Missile missile = source.CreateProjectile(410, Libraries.Magic, true, 6, 30, 4, targetID: p.Destination);

                        if (missile.Target != null)
                        {
                            missile.Complete += (o, e) =>
                            {
                                var sender = (Missile)o;

                                if (sender.Target.CurrentAction == MirAction.Dead) return;
                                sender.Target.Effects.Add(new Effect(Libraries.Magic, 570, 10, 600, sender.Target));
                                SoundManager.PlaySound(20000 + (ushort)Spell.GreatFireBall * 10 + 2);
                            };
                        }
                    }
                    break;
            }
        }

        private void ObjectEffect(S.ObjectEffect p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                PlayerObject player;

                switch (p.Effect)
                {
                    case SpellEffect.FurbolgWarriorCritical:
                        ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.FurbolgWarrior], 400, 6, 600, ob));
                        SoundManager.PlaySound(20000 + (ushort)Spell.FatalSword * 10);
                        break;
                    case SpellEffect.FatalSword:
                        ob.Effects.Add(new Effect(Libraries.Magic2, 1940, 4, 400, ob));
                        SoundManager.PlaySound(20000 + (ushort)Spell.FatalSword * 10);
                        break;
                    case SpellEffect.StormEscape:
                        ob.Effects.Add(new Effect(Libraries.Magic3, 610, 10, 600, ob));
                        SoundManager.PlaySound(SoundList.Teleport);
                        break;
                    case SpellEffect.Teleport:
                        ob.Effects.Add(new Effect(Libraries.Magic, 1600, 10, 600, ob));
                        SoundManager.PlaySound(SoundList.Teleport);
                        break;
                    case SpellEffect.Healing:
                        SoundManager.PlaySound(20000 + (ushort)Spell.Healing * 10 + 1);
                        ob.Effects.Add(new Effect(Libraries.Magic, 370, 10, 800, ob));
                        break;
                    case SpellEffect.RedMoonEvil:
                        ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.RedMoonEvil], 32, 6, 400, ob) { Blend = false });
                        break;
                    case SpellEffect.TwinDrakeBlade:
                        ob.Effects.Add(new Effect(Libraries.Magic2, 380, 6, 800, ob));
                        break;
                    case SpellEffect.MPEater:
                        if (MapControl.Objects.TryGetValue(p.EffectType, out var ob2))
                            ob2.Effects.Add(new Effect(Libraries.Magic2, 2411, 19, 1900, ob2));

                        ob.Effects.Add(new Effect(Libraries.Magic2, 2400, 9, 900, ob));
                        SoundManager.PlaySound(20000 + (ushort)Spell.FatalSword * 10);
                        break;
                    case SpellEffect.Bleeding:
                        ob.Effects.Add(new Effect(Libraries.Magic3, 60, 3, 400, ob));
                        break;
                    case SpellEffect.Hemorrhage:
                        SoundManager.PlaySound(20000 + (ushort)Spell.Hemorrhage * 10);
                        ob.Effects.Add(new Effect(Libraries.Magic3, 0, 4, 400, ob));
                        ob.Effects.Add(new Effect(Libraries.Magic3, 28, 6, 600, ob));
                        ob.Effects.Add(new Effect(Libraries.Magic3, 46, 8, 800, ob));
                        break;
                    case SpellEffect.MagicShieldUp:
                        if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Hero) return;
                        player = (PlayerObject)ob;
                        if (player.ShieldEffect != null)
                        {
                            player.ShieldEffect.Clear();
                            player.ShieldEffect.Remove();
                        }
                        player.MagicShield = true;
                        player.Effects.Add(player.ShieldEffect = new Effect(Libraries.Magic, 3890, 3, 600, ob) { Repeat = true });
                        break;
                    case SpellEffect.MagicShieldDown:
                        if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Hero) return;
                        player = (PlayerObject)ob;
                        if (player.ShieldEffect != null)
                        {
                            player.ShieldEffect.Clear();
                            player.ShieldEffect.Remove();
                        }
                        player.ShieldEffect = null;
                        player.MagicShield = false;
                        break;
                    case SpellEffect.GreatFoxSpirit:
                        ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.GreatFoxSpirit], 375 + (CMain.Random.Next(3) * 20), 20, 1400, ob));
                        SoundManager.PlaySound(((ushort)Monster.GreatFoxSpirit * 10) + 5);
                        break;
                    case SpellEffect.Entrapment:
                        ob.Effects.Add(new Effect(Libraries.Magic2, 1010, 10, 1500, ob));
                        ob.Effects.Add(new Effect(Libraries.Magic2, 1020, 8, 1200, ob));
                        break;
                    case SpellEffect.Critical:
                        //ob.Effects.Add(new Effect(Libraries.CustomEffects, 0, 12, 60, ob));
                        break;
                    case SpellEffect.Reflect:
                        ob.Effects.Add(new Effect(Libraries.Effect, 580, 10, 70, ob));
                        break;
                    case SpellEffect.ElementalBarrierUp:
                        if (ob.Race != ObjectType.Player) return;
                        player = (PlayerObject)ob;
                        if (player.ElementalBarrierEffect != null)
                        {
                            player.ElementalBarrierEffect.Clear();
                            player.ElementalBarrierEffect.Remove();
                        }

                        player.ElementalBarrier = true;
                        player.Effects.Add(player.ElementalBarrierEffect = new Effect(Libraries.Magic3, 1890, 10, 2000, ob) { Repeat = true });
                        break;
                    case SpellEffect.ElementalBarrierDown:
                        if (ob.Race != ObjectType.Player) return;
                        player = (PlayerObject)ob;
                        if (player.ElementalBarrierEffect != null)
                        {
                            player.ElementalBarrierEffect.Clear();
                            player.ElementalBarrierEffect.Remove();
                        }
                        player.ElementalBarrierEffect = null;
                        player.ElementalBarrier = false;
                        player.Effects.Add(player.ElementalBarrierEffect = new Effect(Libraries.Magic3, 1910, 7, 1400, ob));
                        SoundManager.PlaySound(20000 + 131 * 10 + 5);
                        break;
                    case SpellEffect.DelayedExplosion:
                        int effectid = DelayedExplosionEffect.GetOwnerEffectID(ob.ObjectID);
                        if (effectid < 0)
                        {
                            ob.Effects.Add(new DelayedExplosionEffect(Libraries.Magic3, 1590, 8, 1200, ob, true, 0, 0));
                        }
                        else if (effectid >= 0)
                        {
                            if (DelayedExplosionEffect.effectlist[effectid].stage < p.EffectType)
                            {
                                DelayedExplosionEffect.effectlist[effectid].Remove();
                                ob.Effects.Add(new DelayedExplosionEffect(Libraries.Magic3, 1590 + ((int)p.EffectType * 10), 8, 1200, ob, true, (int)p.EffectType, 0));
                            }
                        }
                        break;
                    case SpellEffect.AwakeningSuccess:
                        {
                            Effect ef = new Effect(Libraries.Magic3, 900, 16, 1600, ob, CMain.Time + p.DelayTime);
                            ef.Played += (o, e) => SoundManager.PlaySound(50002);
                            ef.Complete += (o, e) => MapControl.AwakeningAction = false;
                            ob.Effects.Add(ef);
                            ob.Effects.Add(new Effect(Libraries.Magic3, 840, 16, 1600, ob, CMain.Time + p.DelayTime) { Blend = false });
                        }
                        break;
                    case SpellEffect.AwakeningFail:
                        {
                            Effect ef = new Effect(Libraries.Magic3, 920, 9, 900, ob, CMain.Time + p.DelayTime);
                            ef.Played += (o, e) => SoundManager.PlaySound(50003);
                            ef.Complete += (o, e) => MapControl.AwakeningAction = false;
                            ob.Effects.Add(ef);
                            ob.Effects.Add(new Effect(Libraries.Magic3, 860, 9, 900, ob, CMain.Time + p.DelayTime) { Blend = false });
                        }
                        break;
                    case SpellEffect.AwakeningHit:
                        {
                            Effect ef = new Effect(Libraries.Magic3, 880, 5, 500, ob, CMain.Time + p.DelayTime);
                            ef.Played += (o, e) => SoundManager.PlaySound(50001);
                            ob.Effects.Add(ef);
                            ob.Effects.Add(new Effect(Libraries.Magic3, 820, 5, 500, ob, CMain.Time + p.DelayTime) { Blend = false });
                        }
                        break;
                    case SpellEffect.AwakeningMiss:
                        {
                            Effect ef = new Effect(Libraries.Magic3, 890, 5, 500, ob, CMain.Time + p.DelayTime);
                            ef.Played += (o, e) => SoundManager.PlaySound(50000);
                            ob.Effects.Add(ef);
                            ob.Effects.Add(new Effect(Libraries.Magic3, 830, 5, 500, ob, CMain.Time + p.DelayTime) { Blend = false });
                        }
                        break;
                    case SpellEffect.TurtleKing:
                        {
                            Effect ef = new Effect(Libraries.Monsters[(ushort)Monster.TurtleKing], CMain.Random.Next(2) == 0 ? 922 : 934, 12, 1200, ob);
                            ef.Played += (o, e) => SoundManager.PlaySound(20000 + (ushort)Spell.HellFire * 10 + 1);
                            ob.Effects.Add(ef);
                        }
                        break;
                    case SpellEffect.Behemoth:
                        {
                            MapControl.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.Behemoth], 788, 10, 1500, ob.CurrentLocation));
                            MapControl.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.Behemoth], 778, 10, 1500, ob.CurrentLocation, 0, true) { Blend = false });
                        }
                        break;
                    case SpellEffect.Stunned:
                        ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.StoningStatue], 632, 10, 1000, ob)
                        {
                            Repeat = p.Time > 0,
                            RepeatUntil = p.Time > 0 ? CMain.Time + p.Time : 0
                        });
                        break;
                    case SpellEffect.IcePillar:
                        ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.IcePillar], 18, 8, 800, ob));
                        break;
                    case SpellEffect.KingGuard:
                        if (p.EffectType == 0)
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.KingGuard], 753, 10, 1000, ob) { Blend = false });
                        }
                        else
                        {
                            ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.KingGuard], 763, 10, 1000, ob) { Blend = false });
                        }
                        break;
                    case SpellEffect.FlamingMutantWeb:
                        ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.FlamingMutant], 330, 10, 1000, ob)
                        {
                            Repeat = p.Time > 0,
                            RepeatUntil = p.Time > 0 ? CMain.Time + p.Time : 0
                        });
                        break;
                    case SpellEffect.DeathCrawlerBreath:
                        ob.Effects.Add(new Effect(Libraries.Monsters[(ushort)Monster.DeathCrawler], 272 + ((int)ob.Direction * 4), 4, 400, ob) { Blend = true });
                        break;
                    case SpellEffect.MoonMist:
                        ob.Effects.Add(new Effect(Libraries.Magic3, 705, 10, 800, ob));
                        break;
                }
            }
        }

        private void RangeAttack(S.RangeAttack p)
        {
            User.TargetID = p.TargetID;
            User.TargetPoint = p.Target;
            User.Spell = p.Spell;
        }

        private void Pushed(S.Pushed p)
        {
            User.ActionFeed.Add(new QueuedAction { Action = MirAction.Pushed, Direction = p.Direction, Location = p.Location });
        }

        private void ObjectPushed(S.ObjectPushed p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Pushed, Direction = p.Direction, Location = p.Location });
        }

        private void ObjectName(S.ObjectName p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.Name = p.Name;
        }

        private void UserStorage(S.UserStorage p)
        {
            if (Storage.Length != p.Storage.Length)
            {
                Array.Resize(ref Storage, p.Storage.Length);
            }

            Storage = p.Storage;

            for (int i = 0; i < Storage.Length; i++)
            {
                if (Storage[i] == null) continue;
                Bind(Storage[i]);
            }
        }
        private void SwitchGroup(S.SwitchGroup p)
        {
            GroupDialog.AllowGroup = p.AllowGroup;

            if (!p.AllowGroup && GroupDialog.GroupList.Count > 0)
                DeleteGroup();
        }

        private void DeleteGroup()
        {
            GroupDialog.GroupList.Clear();
            GroupDialog.GroupMembersMap.Clear();
            BigMapViewPort.PlayerLocations.Clear();
            ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.YouHaveLeftGroup), ChatType.Group);
        }

        private void DeleteMember(S.DeleteMember p)
        {
            GroupDialog.GroupList.Remove(p.Name);
            GroupDialog.GroupMembersMap.Remove(p.Name);
            BigMapViewPort.PlayerLocations.Remove(p.Name);
            ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerHasLeftGroup), p.Name), ChatType.Group);
        }

        private void GroupInvite(S.GroupInvite p)
        {
            MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.DoYouWantGroupWithPlayer), p.Name), MirMessageBoxButtons.YesNo);

            messageBox.YesButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.GroupInvite { AcceptInvite = true });
                GroupDialog.Show();
            };
            messageBox.NoButton.Click += (o, e) => Network.Enqueue(new C.GroupInvite { AcceptInvite = false });
            messageBox.Show();
        }
        private void AddMember(S.AddMember p)
        {
            GroupDialog.GroupList.Add(p.Name);
            ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerHasJoinedGroup), p.Name), ChatType.Group);
        }
        private void GroupMembersMap(S.GroupMembersMap p)
        {
            if (!GroupDialog.GroupMembersMap.ContainsKey(p.PlayerName))
                GroupDialog.GroupMembersMap.Add(p.PlayerName, p.PlayerMap);
            else
            {
                GroupDialog.GroupMembersMap.Remove(p.PlayerName);
                GroupDialog.GroupMembersMap.Add(p.PlayerName, p.PlayerMap);
            }
        }
        private void SendMemberLocation(S.SendMemberLocation p)
        {
            if (!BigMapViewPort.PlayerLocations.ContainsKey(p.MemberName))
                BigMapViewPort.PlayerLocations.Add(p.MemberName, p.MemberLocation);
            else
            {
                BigMapViewPort.PlayerLocations.Remove(p.MemberName);
                BigMapViewPort.PlayerLocations.Add(p.MemberName, p.MemberLocation);
            }
        }
        private void Revived()
        {
            User.SetAction();
            User.Dead = false;
            User.Effects.Add(new Effect(Libraries.Magic2, 1220, 20, 2000, User));
            SoundManager.PlaySound(SoundList.Revive);
        }
        private void ObjectRevived(S.ObjectRevived p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                if (p.Effect)
                {
                    ob.Effects.Add(new Effect(Libraries.Magic2, 1220, 20, 2000, ob));
                    SoundManager.PlaySound(SoundList.Revive);
                }
                ob.Dead = false;
                ob.ActionFeed.Clear();
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Revive, Direction = ob.Direction, Location = ob.CurrentLocation });
            }
        }
        private void SpellToggle(S.SpellToggle p)
        {
            UserObject actor = User;
            string prefix = string.Empty;

            if (p.ObjectID == Hero?.ObjectID)
            {
                actor = Hero;
                prefix = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Hero);
            }

            switch (p.Spell)
            {
                //Warrior
                case Spell.Slaying:
                    actor.Slaying = p.CanUse;
                    break;
                case Spell.Thrusting:
                    actor.Thrusting = p.CanUse;
                    ChatDialog.ReceiveChat(
                        prefix + (actor.Thrusting ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UseThrusting) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DoNotUseThrusting)),
                        ChatType.Hint);
                    break;
                case Spell.HalfMoon:
                    actor.HalfMoon = p.CanUse;
                    ChatDialog.ReceiveChat(
                        prefix + (actor.HalfMoon ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UseHalfMoon) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DoNotUseHalfMoon)),
                        ChatType.Hint);
                    break;
                case Spell.CrossHalfMoon:
                    actor.CrossHalfMoon = p.CanUse;
                    ChatDialog.ReceiveChat(
                        prefix + (actor.CrossHalfMoon ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UseCrossHalfMoon) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DoNotUseCrossHalfMoon)),
                        ChatType.Hint);
                    break;
                case Spell.DoubleSlash:
                    actor.DoubleSlash = p.CanUse;
                    ChatDialog.ReceiveChat(
                        prefix + (actor.DoubleSlash ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UseDoubleSlash) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DoNotUseDoubleSlash)),
                        ChatType.Hint);
                    break;
                case Spell.FlamingSword:
                    actor.FlamingSword = p.CanUse;
                    if (actor.FlamingSword)
                        ChatDialog.ReceiveChat(prefix + GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.WeaponSpiritFire), ChatType.Hint);
                    else
                        ChatDialog.ReceiveChat(prefix + GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SpiritsFireDisappeared), ChatType.System);
                    break;
            }
        }

        private void ObjectHealth(S.ObjectHealth p)
        {
            if (p.ObjectID == Hero?.ObjectID)
                Hero.PercentHealth = p.Percent;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                ob.PercentHealth = p.Percent;
                ob.HealthTime = CMain.Time + p.Expire * 1000;
            }
        }

        private void ObjectMana(S.ObjectMana p)
        {
            if (p.ObjectID == Hero?.ObjectID)
                Hero.PercentMana = p.Percent;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.PercentMana = p.Percent;
        }


        private void MapEffect(S.MapEffect p)
        {
            switch (p.Effect)
            {
                case SpellEffect.Mine:
                    SoundManager.PlaySound(10091);
                    Effect HitWall = new Effect(Libraries.Effect, 8 * p.Value, 3, 240, p.Location) { Light = 0 };
                    MapControl.Effects.Add(HitWall);
                    break;
                case SpellEffect.Tester:
                    Effect eff = new Effect(Libraries.Effect, 328, 10, 500, p.Location) { Light = 0 };
                    MapControl.Effects.Add(eff);
                    break;
            }
        }

        private void ObjectRangeAttack(S.ObjectRangeAttack p)
        {
            if (p.ObjectID == User.ObjectID && !Observing) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                QueuedAction action = null;
                if (ob.Race == ObjectType.Player)
                {
                    action = p.Type switch
                    {
                        _ => new QueuedAction { Action = MirAction.AttackRange1, Direction = p.Direction, Location = p.Location, Params = new List<object>() }
                    };
                }
                else
                {
                    action = p.Type switch
                    {
                        1 => new QueuedAction { Action = MirAction.AttackRange2, Direction = p.Direction, Location = p.Location, Params = new List<object>() },
                        2 => new QueuedAction { Action = MirAction.AttackRange3, Direction = p.Direction, Location = p.Location, Params = new List<object>() },
                        _ => new QueuedAction { Action = MirAction.AttackRange1, Direction = p.Direction, Location = p.Location, Params = new List<object>() }
                    };
                }

                action.Params.Add(p.TargetID);
                action.Params.Add(p.Target);
                action.Params.Add(p.Spell);
                action.Params.Add(new List<uint>());
                action.Params.Add(p.Level);

                ob.ActionFeed.Add(action);
            }
        }

        private void AddBuff(S.AddBuff p)
        {
            ClientBuff buff = p.Buff;

            if (!buff.Paused)
            {
                buff.ExpireTime += CMain.Time;
            }

            if (buff.ObjectID == User.ObjectID)
            {
                for (int i = 0; i < BuffsDialog.Buffs.Count; i++)
                {
                    if (BuffsDialog.Buffs[i].Type != buff.Type) continue;

                    BuffsDialog.Buffs[i] = buff;
                    User.RefreshStats();
                    return;
                }

                BuffsDialog.Buffs.Add(buff);
                BuffsDialog.CreateBuff(buff);

                User.RefreshStats();
            }

            if (Hero != null && buff.ObjectID == Hero.ObjectID)
            {
                for (int i = 0; i < HeroBuffsDialog.Buffs.Count; i++)
                {
                    if (HeroBuffsDialog.Buffs[i].Type != buff.Type) continue;

                    HeroBuffsDialog.Buffs[i] = buff;
                    Hero.RefreshStats();
                    return;
                }

                HeroBuffsDialog.Buffs.Add(buff);
                HeroBuffsDialog.CreateBuff(buff);

                Hero.RefreshStats();
            }

            if (!buff.Visible || buff.ObjectID <= 0) return;

            if (MapControl.Objects.TryGetValue(buff.ObjectID, out var ob) && (ob is PlayerObject || ob is MonsterObject))
            {
                if (!ob.Buffs.Contains(buff.Type))
                    ob.Buffs.Add(buff.Type);
                ob.AddBuffEffect(buff.Type);
            }
        }

        private void RemoveBuff(S.RemoveBuff p)
        {
            if (User.ObjectID == p.ObjectID)
            {
                for (int i = 0; i < BuffsDialog.Buffs.Count; i++)
                {
                    if (BuffsDialog.Buffs[i].Type != p.Type) continue;

                    switch (BuffsDialog.Buffs[i].Type)
                    {
                        case BuffType.SwiftFeet:
                            User.Sprint = false;
                            break;
                        case BuffType.Transform:
                            User.TransformType = -1;
                            break;
                    }

                    BuffsDialog.RemoveBuff(i);
                    BuffsDialog.Buffs.RemoveAt(i);
                }
                User.RefreshStats();
            }

            if (Hero != null && Hero.ObjectID == p.ObjectID)
            {
                for (int i = 0; i < HeroBuffsDialog.Buffs.Count; i++)
                {
                    if (HeroBuffsDialog.Buffs[i].Type != p.Type) continue;

                    switch (HeroBuffsDialog.Buffs[i].Type)
                    {
                        case BuffType.SwiftFeet:
                            Hero.Sprint = false;
                            break;
                        case BuffType.Transform:
                            Hero.TransformType = -1;
                            break;
                    }

                    HeroBuffsDialog.RemoveBuff(i);
                    HeroBuffsDialog.Buffs.RemoveAt(i);
                }
                Hero.RefreshStats();
            }

            if (p.ObjectID <= 0) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                ob.Buffs.Remove(p.Type);
                ob.RemoveBuffEffect(p.Type);
            }
        }

        private void PauseBuff(S.PauseBuff p)
        {
            if (User.ObjectID == p.ObjectID)
            {
                for (int i = 0; i < BuffsDialog.Buffs.Count; i++)
                {
                    if (BuffsDialog.Buffs[i].Type != p.Type) continue;

                    User.RefreshStats();

                    if (BuffsDialog.Buffs[i].Paused == p.Paused) return;

                    BuffsDialog.Buffs[i].Paused = p.Paused;

                    if (p.Paused)
                    {
                        BuffsDialog.Buffs[i].ExpireTime -= CMain.Time;
                    }
                    else
                    {
                        BuffsDialog.Buffs[i].ExpireTime += CMain.Time;
                    }
                }
            }

            if (Hero != null && Hero.ObjectID == p.ObjectID)
            {
                for (int i = 0; i < HeroBuffsDialog.Buffs.Count; i++)
                {
                    if (HeroBuffsDialog.Buffs[i].Type != p.Type) continue;

                    Hero.RefreshStats();

                    if (HeroBuffsDialog.Buffs[i].Paused == p.Paused) return;

                    HeroBuffsDialog.Buffs[i].Paused = p.Paused;

                    if (p.Paused)
                    {
                        HeroBuffsDialog.Buffs[i].ExpireTime -= CMain.Time;
                    }
                    else
                    {
                        HeroBuffsDialog.Buffs[i].ExpireTime += CMain.Time;
                    }
                }
            }
        }

        private void ObjectHidden(S.ObjectHidden p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.Hidden = p.Hidden;
        }

        private void ObjectSneaking(S.ObjectSneaking p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                // ob.SneakingActive = p.SneakingActive;
            }
        }

        private void ObjectLevelEffects(S.ObjectLevelEffects p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob) && ob is PlayerObject player)
            {
                player.LevelEffects = p.LevelEffects;
                player.SetEffects();
            }
        }

        private void RefreshItem(S.RefreshItem p)
        {
            Bind(p.Item);

            if (SelectedCell != null && SelectedCell.Item.UniqueID == p.Item.UniqueID)
                SelectedCell = null;

            if (HoverItem != null && HoverItem.UniqueID == p.Item.UniqueID)
            {
                DisposeItemLabel();
                CreateItemLabel(p.Item);
            }

            for (int i = 0; i < User.Inventory.Length; i++)
            {
                if (User.Inventory[i] != null && User.Inventory[i].UniqueID == p.Item.UniqueID)
                {
                    User.Inventory[i] = p.Item;
                    User.RefreshStats();
                    return;
                }
            }

            for (int i = 0; i < User.Equipment.Length; i++)
            {
                if (User.Equipment[i] != null && User.Equipment[i].UniqueID == p.Item.UniqueID)
                {
                    User.Equipment[i] = p.Item;
                    User.RefreshStats();
                    return;
                }
            }

            if (Hero != null)
            {
                for (int i = 0; i < Hero.Inventory.Length; i++)
                {
                    if (Hero.Inventory[i] != null && Hero.Inventory[i].UniqueID == p.Item.UniqueID)
                    {
                        Hero.Inventory[i] = p.Item;
                        Hero.RefreshStats();
                        return;
                    }
                }

                for (int i = 0; i < Hero.Equipment.Length; i++)
                {
                    if (Hero.Equipment[i] != null && Hero.Equipment[i].UniqueID == p.Item.UniqueID)
                    {
                        Hero.Equipment[i] = p.Item;
                        Hero.RefreshStats();
                        return;
                    }
                }
            }
        }

        private void ObjectSpell(S.ObjectSpell p)
        {
            SpellObject ob = new SpellObject(p.ObjectID);
            ob.Load(p);
        }

        private void ObjectDeco(S.ObjectDeco p)
        {
            DecoObject ob = new DecoObject(p.ObjectID);
            ob.Load(p);
        }

        private void UserDash(S.UserDash p)
        {
            if (User.Direction == p.Direction && User.CurrentLocation == p.Location)
            {
                MapControl.NextAction = 0;
                return;
            }
            MirAction action = User.CurrentAction == MirAction.DashL ? MirAction.DashR : MirAction.DashL;
            for (int i = User.ActionFeed.Count - 1; i >= 0; i--)
            {
                if (User.ActionFeed[i].Action == MirAction.DashR)
                {
                    action = MirAction.DashL;
                    break;
                }
                if (User.ActionFeed[i].Action == MirAction.DashL)
                {
                    action = MirAction.DashR;
                    break;
                }
            }

            User.ActionFeed.Add(new QueuedAction { Action = action, Direction = p.Direction, Location = p.Location });
        }

        private void UserDashFail(S.UserDashFail p)
        {
            MapControl.NextAction = 0;
            User.ActionFeed.Add(new QueuedAction { Action = MirAction.DashFail, Direction = p.Direction, Location = p.Location });
        }

        private void ObjectDash(S.ObjectDash p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                var action = ob.ActionFeed.Count > 0 && ob.ActionFeed[^1].Action == MirAction.DashL ? MirAction.DashR : MirAction.DashL;
                ob.ActionFeed.Add(new QueuedAction { Action = action, Direction = p.Direction, Location = p.Location });
            }

        }

        private void ObjectDashFail(S.ObjectDashFail p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.DashFail, Direction = p.Direction, Location = p.Location });
        }

        private void UserBackStep(S.UserBackStep p)
        {
            if (User.Direction == p.Direction && User.CurrentLocation == p.Location)
            {
                MapControl.NextAction = 0;
                return;
            }
            User.ActionFeed.Add(new QueuedAction { Action = MirAction.Jump, Direction = p.Direction, Location = p.Location });
        }

        private void ObjectBackStep(S.ObjectBackStep p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                ob.JumpDistance = p.Distance;
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Jump, Direction = p.Direction, Location = p.Location });
            }
        }

        private void UserDashAttack(S.UserDashAttack p)
        {
            if (User.Direction == p.Direction && User.CurrentLocation == p.Location)
            {
                MapControl.NextAction = 0;
                return;
            }
            //User.JumpDistance = p.Distance;
            User.ActionFeed.Add(new QueuedAction { Action = MirAction.DashAttack, Direction = p.Direction, Location = p.Location });
        }

        private void ObjectDashAttack(S.ObjectDashAttack p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob))
            {
                ob.JumpDistance = p.Distance;
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.DashAttack, Direction = p.Direction, Location = p.Location });
            }
        }

        private void UserAttackMove(S.UserAttackMove p)//Warrior Skill - SlashingBurst
        {
            MapControl.NextAction = 0;
            if (User.CurrentLocation == p.Location && User.Direction == p.Direction) return;


            MapControl.RemoveObject(User);
            User.CurrentLocation = p.Location;
            User.MapLocation = p.Location;
            MapControl.AddObject(User);


            MapControl.FloorValid = false;
            MapControl.InputDelay = CMain.Time + 400;


            if (User.Dead) return;


            User.ClearMagic();
            User.QueuedAction = null;


            for (int i = User.ActionFeed.Count - 1; i >= 0; i--)
            {
                if (User.ActionFeed[i].Action == MirAction.Pushed) continue;
                User.ActionFeed.RemoveAt(i);
            }


            User.SetAction();

            User.ActionFeed.Add(new QueuedAction { Action = MirAction.Standing, Direction = p.Direction, Location = p.Location });
        }

        private void SetConcentration(S.SetConcentration p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob) && (ob.Race == ObjectType.Player || ob.Race == ObjectType.Hero))
            {
                var player = (PlayerObject)ob;
                player.Concentrating = p.Enabled;
                player.ConcentrateInterrupted = p.Interrupted;

                if (p.Enabled && !p.Interrupted && InterruptionEffect.GetOwnerEffectID(player.ObjectID) < 0)
                {
                    player.Effects.Add(new InterruptionEffect(Libraries.Magic3, 1860, 8, 8 * 100, player, true));
                    SoundManager.PlaySound(20000 + 129 * 10);
                }
            }
        }

        private void SetElemental(S.SetElemental p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out var ob) && (ob.Race == ObjectType.Player || ob.Race == ObjectType.Hero))
            {
                var player = (PlayerObject)ob;
                player.HasElements = p.Enabled;
                player.ElementCasted = p.Casted && User.ObjectID != p.ObjectID;
                player.ElementsLevel = (int)p.Value;

                if (p.Enabled && p.ElementType > 0)
                {
                    int elementType = (int)p.ElementType;
                    int maxExp = (int)p.ExpLast;
                    player.Effects.Add(new ElementsEffect(Libraries.Magic3, 1630 + ((elementType - 1) * 10), 10, 10 * 100, player, true, 1 + (elementType - 1), maxExp, User.ObjectID == p.ObjectID && (elementType == 4 || elementType == 3)));
                }
            }
        }

        private void RemoveDelayedExplosion(S.RemoveDelayedExplosion p)
        {
            //if (p.ObjectID == User.ObjectID) return;

            int effectid = DelayedExplosionEffect.GetOwnerEffectID(p.ObjectID);
            if (effectid >= 0)
                DelayedExplosionEffect.effectlist[effectid].Remove();
        }
        private void SetBindingShot(S.SetBindingShot p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob) && ob.Race == ObjectType.Monster)
            {
                var NetCast = new TrackableEffect(new Effect(Libraries.MagicC, 0, 8, 700, ob)) { EffectName = "BindingShotDrop" };
                var NetDropped = new TrackableEffect(new Effect(Libraries.MagicC, 7, 1, 1000, ob) { Repeat = true, RepeatUntil = CMain.Time + (p.Value - 1500) }) { EffectName = "BindingShotDown" };
                var NetFall = new TrackableEffect(new Effect(Libraries.MagicC, 8, 8, 700, ob)) { EffectName = "BindingShotFall" };

                NetDropped.Complete += (o1, e1) =>
                {
                    SoundManager.PlaySound(20000 + 130 * 10 + 6); // sound M130-6
                    ob.Effects.Add(NetFall);
                };

                NetCast.Complete += (o, e) =>
                {
                    SoundManager.PlaySound(20000 + 130 * 10 + 5); // sound M130-5
                    ob.Effects.Add(NetDropped);
                };

                ob.Effects.Add(NetCast);
            }
        }

        private void SendOutputMessage(S.SendOutputMessage p)
        {
            OutputMessage(p.Message, p.Type);
        }

        private void NPCConsign()
        {
            if (!NPCDialog.Visible) return;
            NPCDropDialog.PType = PanelType.Consign;
            NPCDropDialog.Show();
        }
        private void NPCMarket(S.NPCMarket p)
        {
            for (int i = 0; i < p.Listings.Count; i++)
                Bind(p.Listings[i].Item);

            TrustMerchantDialog.Show();
            TrustMerchantDialog.UserMode = p.UserMode;
            TrustMerchantDialog.Listings = p.Listings;
            TrustMerchantDialog.Page = 0;
            TrustMerchantDialog.PageCount = p.Pages;
            TrustMerchantDialog.UpdateInterface();
        }
        private void NPCMarketPage(S.NPCMarketPage p)
        {
            if (!TrustMerchantDialog.Visible) return;

            for (int i = 0; i < p.Listings.Count; i++)
                Bind(p.Listings[i].Item);

            TrustMerchantDialog.Listings.AddRange(p.Listings);
            TrustMerchantDialog.Page = (TrustMerchantDialog.Listings.Count - 1) / 10;
            TrustMerchantDialog.UpdateInterface();
        }
        private void ConsignItem(S.ConsignItem p)
        {
            MirItemCell cell = InventoryDialog.GetCell(p.UniqueID) ?? BeltDialog.GetCell(p.UniqueID);

            if (cell == null) return;

            cell.Locked = false;

            if (!p.Success) return;

            cell.Item = null;

            User.RefreshStats();
        }
        private void MarketFail(S.MarketFail p)
        {
            TrustMerchantDialog.MarketTime = 0;
            switch (p.Reason)
            {
                case 0:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.YouCannotUseTrustMerchantWhenDead));
                    break;
                case 1:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.YouCannotBuyFromTrustMerchantWithoutUsing));
                    break;
                case 2:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemHasBeenSold));
                    break;
                case 3:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemExpiredCannotBeBought));
                    break;
                case 4:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowGold));
                    break;
                case 5:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NotEnoughWeightOrSpaceToBuyItem));
                    break;
                case 6:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotBuyOwnItems));
                    break;
                case 7:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.TooFarFromTrustMerchant));
                    break;
                case 8:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotHoldGoldForSale));
                    break;
                case 9:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemNotMetMinimumBid));
                    break;
                case 10:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AuctionAlreadyEndedForItem));
                    break;
            }

        }
        private void MarketSuccess(S.MarketSuccess p)
        {
            TrustMerchantDialog.MarketTime = 0;
            MirMessageBox.Show(p.Message);
        }
        private void ObjectSitDown(S.ObjectSitDown p)
        {
            if (p.ObjectID == User.ObjectID) return;

            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob) && ob.Race == ObjectType.Monster)
            {
                ob.SitDown = p.Sitting;
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.SitDown, Direction = p.Direction, Location = p.Location });
            }
        }

        private void BaseStatsInfo(S.BaseStatsInfo p)
        {
            User.CoreStats = p.Stats;
            User.RefreshStats();
        }

        private void HeroBaseStatsInfo(S.HeroBaseStatsInfo p)
        {
            if (Hero == null) return;

            Hero.CoreStats = p.Stats;
            Hero.RefreshStats();
        }

        private void UserName(S.UserName p)
        {
            for (int i = 0; i < UserIdList.Count; i++)
                if (UserIdList[i].Id == p.Id)
                {
                    UserIdList[i].UserName = p.Name;
                    break;
                }
            DisposeItemLabel();
            HoverItem = null;
        }

        private void ChatItemStats(S.ChatItemStats p)
        {
            //for (int i = 0; i < ChatItemList.Count; i++)
            //    if (ChatItemList[i].ID == p.ChatItemId)
            //    {
            //        ChatItemList[i].ItemStats = p.Stats;
            //        ChatItemList[i].RecievedTick = CMain.Time;
            //    }
        }

        private void GuildInvite(S.GuildInvite p)
        {
            MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.JoinGuildPrompt), p.Name), MirMessageBoxButtons.YesNo);

            messageBox.YesButton.Click += (o, e) => Network.Enqueue(new C.GuildInvite { AcceptInvite = true });
            messageBox.NoButton.Click += (o, e) => Network.Enqueue(new C.GuildInvite { AcceptInvite = false });

            messageBox.Show();
        }

        private void GuildNameRequest(S.GuildNameRequest p)
        {
            MirInputBox inputBox = new MirInputBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.EnterGuildNameLengthLimit));
            inputBox.InputTextBox.TextBox.KeyPress += (o, e) =>
            {
                string Allowed = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                if (!Allowed.Contains(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                    e.Handled = true;
            };
            inputBox.OKButton.Click += (o, e) =>
            {
                if (inputBox.InputTextBox.Text.Contains('\\'))
                {
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotUseBackslashInGuildName), ChatType.System);
                    inputBox.InputTextBox.Text = "";
                }
                Network.Enqueue(new C.GuildNameReturn { Name = inputBox.InputTextBox.Text });
                inputBox.Dispose();
            };
            inputBox.Show();
        }

        private void GuildRequestWar(S.GuildRequestWar p)
        {
            MirInputBox inputBox = new MirInputBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.EnterGuildToWarWith));

            inputBox.OKButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.GuildWarReturn { Name = inputBox.InputTextBox.Text });
                inputBox.Dispose();
            };
            inputBox.Show();
        }

        private void GuildNoticeChange(S.GuildNoticeChange p)
        {
            if (p.update == -1)
                GuildDialog.NoticeChanged = true;
            else
                GuildDialog.NoticeChange(p.notice);
        }
        private void GuildMemberChange(S.GuildMemberChange p)
        {
            switch (p.Status)
            {
                case 0: // logged of
                    GuildDialog.MemberStatusChange(p.Name, false);
                    break;
                case 1: // logged on
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerLoggedOn), p.Name), ChatType.Guild);
                    GuildDialog.MemberStatusChange(p.Name, true);
                    break;
                case 2: //new member
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerJoinedGuild), p.Name), ChatType.Guild);
                    GuildDialog.MemberCount++;
                    GuildDialog.MembersChanged = true;
                    break;
                case 3: //kicked member
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerRemovedFromGuild), p.Name), ChatType.Guild);
                    GuildDialog.MemberCount = Math.Max(0, GuildDialog.MemberCount - 1);
                    GuildDialog.MembersChanged = true;
                    break;
                case 4: //member left
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerLeftGuild), p.Name), ChatType.Guild);
                    GuildDialog.MemberCount = Math.Max(0, GuildDialog.MemberCount - 1);
                    GuildDialog.MembersChanged = true;
                    break;
                case 5://rank change (name or different rank)
                    GuildDialog.MembersChanged = true;
                    break;
                case 6: //new rank
                    if (p.Ranks.Count > 0)
                        GuildDialog.NewRankRecieved(p.Ranks[0]);
                    break;
                case 7: //rank option changed
                    if (p.Ranks.Count > 0)
                        GuildDialog.RankChangeRecieved(p.Ranks[0]);
                    break;
                case 8: //my rank changed
                    if (p.Ranks.Count > 0)
                        GuildDialog.MyRankChanged(p.Ranks[0]);
                    break;
                case 255:
                    GuildDialog.NewMembersList(p.Ranks);
                    break;
            }
        }

        private void GuildStatus(S.GuildStatus p)
        {
            if ((User.GuildName == "") && (p.GuildName != ""))
            {
                GuildDialog.NoticeChanged = true;
                GuildDialog.MembersChanged = true;
            }
            if (p.GuildName == "")
            {
                GuildDialog.Hide();
            }

            if ((User.GuildName == p.GuildName) && (GuildDialog.Level < p.Level))
            {
                //guild leveled
            }
            bool GuildChange = User.GuildName != p.GuildName;
            User.GuildName = p.GuildName;
            User.GuildRankName = p.GuildRankName;
            GuildDialog.Level = p.Level;
            GuildDialog.Experience = p.Experience;
            GuildDialog.MaxExperience = p.MaxExperience;
            GuildDialog.Gold = p.Gold;
            GuildDialog.SparePoints = p.SparePoints;
            GuildDialog.MemberCount = p.MemberCount;
            GuildDialog.MaxMembers = p.MaxMembers;
            GuildDialog.Voting = p.Voting;
            GuildDialog.ItemCount = p.ItemCount;
            GuildDialog.BuffCount = p.BuffCount;
            GuildDialog.StatusChanged(p.MyOptions);
            GuildDialog.MyRankId = p.MyRankId;
            GuildDialog.UpdateMembers();
            //reset guildbuffs
            if (GuildChange)
            {
                GuildDialog.EnabledBuffs.Clear();
                GuildDialog.UpdateActiveStats();
                RemoveBuff(new S.RemoveBuff { ObjectID = User.ObjectID, Type = BuffType.Guild });
                User.RefreshStats();
            }
        }

        private void GuildExpGain(S.GuildExpGain p)
        {
            //OutputMessage(string.Format("Guild Experience Gained {0}.", p.Amount));
            GuildDialog.Experience += p.Amount;
        }

        private void GuildStorageGoldChange(S.GuildStorageGoldChange p)
        {
            switch (p.Type)
            {
                case 0:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerDonatedGoldToGuildFunds), p.Name, p.Amount), ChatType.Guild);
                    GuildDialog.Gold += p.Amount;
                    break;
                case 1:
                    ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerRetrievedGoldFromGuildFunds), p.Name, p.Amount), ChatType.Guild);
                    if (GuildDialog.Gold > p.Amount)
                        GuildDialog.Gold -= p.Amount;
                    else
                        GuildDialog.Gold = 0;
                    break;
                case 2:
                    if (GuildDialog.Gold > p.Amount)
                        GuildDialog.Gold -= p.Amount;
                    else
                        GuildDialog.Gold = 0;
                    break;
                case 3:
                    GuildDialog.Gold += p.Amount;
                    break;
            }
        }

        private void GuildStorageItemChange(S.GuildStorageItemChange p)
        {
            MirItemCell fromCell = null;
            MirItemCell toCell = null;
            switch (p.Type)
            {
                case 0://store
                    toCell = GuildDialog.StorageGrid[p.To];

                    if (toCell == null) return;

                    toCell.Locked = false;
                    toCell.Item = p.Item.Item;
                    Bind(toCell.Item);
                    if (p.User != User.Id) return;
                    fromCell = p.From < User.BeltIdx ? BeltDialog.Grid[p.From] : InventoryDialog.Grid[p.From - User.BeltIdx];
                    fromCell.Locked = false;
                    if (fromCell != null)
                        fromCell.Item = null;
                    User.RefreshStats();
                    break;
                case 1://retrieve
                    fromCell = GuildDialog.StorageGrid[p.From];

                    if (fromCell == null) return;
                    fromCell.Locked = false;

                    if (p.User != User.Id)
                    {
                        fromCell.Item = null;
                        return;
                    }
                    toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];
                    if (toCell == null) return;
                    toCell.Locked = false;
                    toCell.Item = fromCell.Item;
                    fromCell.Item = null;
                    break;

                case 2:
                    toCell = GuildDialog.StorageGrid[p.To];
                    fromCell = GuildDialog.StorageGrid[p.From];

                    if (toCell == null || fromCell == null) return;

                    toCell.Locked = false;
                    fromCell.Locked = false;
                    fromCell.Item = toCell.Item;
                    toCell.Item = p.Item.Item;

                    Bind(toCell.Item);
                    if (fromCell.Item != null)
                        Bind(fromCell.Item);
                    break;
                case 3://failstore
                    fromCell = p.From < User.BeltIdx ? BeltDialog.Grid[p.From] : InventoryDialog.Grid[p.From - User.BeltIdx];
                    toCell = GuildDialog.StorageGrid[p.To];

                    if (toCell == null || fromCell == null) return;

                    toCell.Locked = false;
                    fromCell.Locked = false;
                    break;
                case 4://failretrieve
                    toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];
                    fromCell = GuildDialog.StorageGrid[p.From];

                    if (toCell == null || fromCell == null) return;

                    toCell.Locked = false;
                    fromCell.Locked = false;
                    break;
                case 5://failmove
                    fromCell = GuildDialog.StorageGrid[p.To];
                    toCell = GuildDialog.StorageGrid[p.From];

                    if (toCell == null || fromCell == null) return;

                    GuildDialog.StorageGrid[p.From].Locked = false;
                    GuildDialog.StorageGrid[p.To].Locked = false;
                    break;
            }
        }
        private void GuildStorageList(S.GuildStorageList p)
        {
            for (int i = 0; i < p.Items.Length; i++)
            {
                if (i >= GuildDialog.StorageGrid.Length) break;
                if (p.Items[i] == null)
                {
                    GuildDialog.StorageGrid[i].Item = null;
                    continue;
                }
                GuildDialog.StorageGrid[i].Item = p.Items[i].Item;
                Bind(GuildDialog.StorageGrid[i].Item);
            }
        }
        private void GuildTerritoryPage(S.GuildTerritoryPage p)
        {
            if (!GuildTerritoryDialog.Visible)
            {
                GuildTerritoryDialog.Show();
            }

            GuildTerritoryDialog.GTMapList = p.Listings;
            GuildTerritoryDialog.Lenght = p.length;
            GuildTerritoryDialog.UpdateInterface();
        }

        private void HeroCreateRequest(S.HeroCreateRequest p)
        {
            NewHeroDialog.WarriorButton.Visible = p.CanCreateClass[(int)MirClass.Warrior];
            NewHeroDialog.WizardButton.Visible = p.CanCreateClass[(int)MirClass.Wizard];
            NewHeroDialog.TaoistButton.Visible = p.CanCreateClass[(int)MirClass.Taoist];
            NewHeroDialog.AssassinButton.Visible = p.CanCreateClass[(int)MirClass.Assassin];
            NewHeroDialog.ArcherButton.Visible = p.CanCreateClass[(int)MirClass.Archer];

            NewHeroDialog.Show();
        }

        private void ManageHeroes(S.ManageHeroes p)
        {
            if (p.Heroes != null)
            {
                for (int i = 0; i < p.Heroes.Length; i++)
                    AddHeroInformation(p.Heroes[i], i);
            }

            MaximumHeroCount = p.MaximumCount;
            HeroManageDialog.SetCurrentHero(p.CurrentHero);
            HeroManageDialog.Show();
        }

        private void ChangeHero(S.ChangeHero p)
        {
            ClientHeroInformation temp = HeroStorage[p.FromIndex];
            HeroStorage[p.FromIndex] = HeroManageDialog.CurrentAvatar.Info;
            HeroManageDialog.SetCurrentHero(temp);
            HeroManageDialog.RefreshInterface();
        }

        public int HeroAvatar(MirClass job, MirGender gender) => 1400 + (byte)job + 10 * (byte)gender;

        private void UnlockHeroAutoPot(bool value)
        {
            if (Hero == null) return;

            Hero.AutoPot = value;
            HeroInventoryDialog.RefreshInterface();
        }

        private void SetAutoPotValue(S.SetAutoPotValue p)
        {
            if (Hero == null) return;

            if (p.Stat == Stat.HP)
                Hero.AutoHPPercent = p.Value;
            else
                Hero.AutoMPPercent = p.Value;

            HeroInventoryDialog.RefreshInterface();
        }

        private void SetAutoPotItem(S.SetAutoPotItem p)
        {
            if (Hero == null) return;

            if (p.Grid == MirGridType.HeroHPItem)
                Hero.HPItem[0] = p.ItemIndex > 0 ? new UserItem(GetItemInfo(p.ItemIndex)) : null;
            else
                Hero.MPItem[0] = p.ItemIndex > 0 ? new UserItem(GetItemInfo(p.ItemIndex)) : null;
        }

        private void SetHeroBehaviour(S.SetHeroBehaviour p)
        {
            if (Hero == null) return;
            HeroBehaviourPanel.UpdateBehaviour(p.Behaviour);
        }

        private void NewHero(S.NewHero p)
        {
            NewHeroDialog.OKButton.Enabled = true;

            switch (p.Result)
            {
                case 0:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CreatingNewHeroesDisabled));
                    NewHeroDialog.Hide();
                    break;
                case 1:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.YourHeroNameNotAcceptable));
                    NewHeroDialog.NameTextBox.SetFocus();
                    break;
                case 2:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SelectedGenderNotExist));
                    break;
                case 3:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SelectedClassNotExist));
                    break;
                case 4:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotMakeMoreHeroes));
                    NewHeroDialog.Hide();
                    break;
                case 5:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CharacterNameAlreadyExists));
                    NewHeroDialog.NameTextBox.SetFocus();
                    break;
                case 6:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NoBagSpace));
                    NewHeroDialog.Hide();
                    break;
                case 10:
                    MirMessageBox.Show(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HeroCreatedSuccessfully));
                    NewHeroDialog.Hide();
                    break;
            }
        }

        private void HeroInformation(S.HeroInformation p)
        {
            Hero = new UserHeroObject(p.ObjectID);
            Hero.Load(p);

            Hero.AutoPot = p.AutoPot;
            Hero.AutoHPPercent = p.AutoHPPercent;
            Hero.AutoMPPercent = p.AutoMPPercent;

            if (p.HPItemIndex > 0)
                Hero.HPItem[0] = new UserItem(GetItemInfo(p.HPItemIndex));
            if (p.MPItemIndex > 0)
                Hero.MPItem[0] = new UserItem(GetItemInfo(p.MPItemIndex));

            HeroDialog = new CharacterDialog(MirGridType.HeroEquipment, Hero) { Parent = this, Visible = false };
            HeroInventoryDialog = new HeroInventoryDialog { Parent = this };
            HeroBeltDialog = new HeroBeltDialog { Parent = this };
            HeroBuffsDialog = new BuffDialog
            {
                Parent = this,
                Visible = true,
                Location = new Point(Settings.ScreenWidth - 170, 80),
                GetExpandedParameter = () => { return Settings.ExpandedHeroBuffWindow; },
                SetExpandedParameter = (value) => { Settings.ExpandedHeroBuffWindow = value; }
            };
            MainDialog.HeroInfoPanel.Update();

            Hero.RefreshStats();
        }

        private void UpdateHeroSpawnState(S.UpdateHeroSpawnState p)
        {
            HeroSpawnState = p.State;

            HasHero = p.State > HeroSpawnState.None;
            MainDialog.HeroInfoPanel.Visible = p.State > HeroSpawnState.Unsummoned;
            MainDialog.HeroMenuButton.Visible = p.State > HeroSpawnState.Unsummoned;
            HeroBehaviourPanel.Visible = p.State > HeroSpawnState.Unsummoned;
            HeroMenuPanel.Visible = HeroMenuPanel.Visible && MainDialog.HeroMenuButton.Visible;

            if (p.State < HeroSpawnState.Summoned)
            {
                HeroInventoryDialog.Dispose();
                HeroDialog.Dispose();
                HeroBeltDialog.Dispose();
                HeroBuffsDialog.Dispose();
            }
        }

        private void MarriageRequest(S.MarriageRequest p)
        {
            MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerAskedForMarriage), p.Name), MirMessageBoxButtons.YesNo);

            messageBox.YesButton.Click += (o, e) => Network.Enqueue(new C.MarriageReply { AcceptInvite = true });
            messageBox.NoButton.Click += (o, e) => { Network.Enqueue(new C.MarriageReply { AcceptInvite = false }); messageBox.Dispose(); };

            messageBox.Show();
        }

        private void DivorceRequest(S.DivorceRequest p)
        {
            MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerRequestedDivorce), p.Name), MirMessageBoxButtons.YesNo);

            messageBox.YesButton.Click += (o, e) => Network.Enqueue(new C.DivorceReply { AcceptInvite = true });
            messageBox.NoButton.Click += (o, e) => { Network.Enqueue(new C.DivorceReply { AcceptInvite = false }); messageBox.Dispose(); };

            messageBox.Show();
        }

        private void MentorRequest(S.MentorRequest p)
        {
            MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerRequestedTeachClass), p.Name, p.Level, GameScene.User.Class.ToString()),
                MirMessageBoxButtons.YesNo);

            messageBox.YesButton.Click += (o, e) => Network.Enqueue(new C.MentorReply { AcceptInvite = true });
            messageBox.NoButton.Click += (o, e) => { Network.Enqueue(new C.MentorReply { AcceptInvite = false }); messageBox.Dispose(); };

            messageBox.Show();
        }

        private bool UpdateGuildBuff(GuildBuff buff, bool Remove = false)
        {
            for (int i = 0; i < GuildDialog.EnabledBuffs.Count; i++)
            {
                if (GuildDialog.EnabledBuffs[i].Id == buff.Id)
                {
                    if (Remove)
                    {
                        GuildDialog.EnabledBuffs.RemoveAt(i);
                    }
                    else
                        GuildDialog.EnabledBuffs[i] = buff;
                    return true;
                }
            }
            return false;
        }

        private void GuildBuffList(S.GuildBuffList p)
        {
            //getting the list of all guildbuffs on server?
            if (p.GuildBuffs.Count > 0)
                GuildDialog.GuildBuffInfos.Clear();
            for (int i = 0; i < p.GuildBuffs.Count; i++)
            {
                GuildDialog.GuildBuffInfos.Add(p.GuildBuffs[i]);
            }
            //getting the list of all active/removedbuffs?
            for (int i = 0; i < p.ActiveBuffs.Count; i++)
            {
                //if (p.ActiveBuffs[i].ActiveTimeRemaining > 0)
                //    p.ActiveBuffs[i].ActiveTimeRemaining = Convert.ToInt32(CMain.Time / 1000) + (p.ActiveBuffs[i].ActiveTimeRemaining * 60);
                if (UpdateGuildBuff(p.ActiveBuffs[i], p.Remove == 1)) continue;
                if (!(p.Remove == 1))
                {
                    GuildDialog.EnabledBuffs.Add(p.ActiveBuffs[i]);
                    //CreateGuildBuff(p.ActiveBuffs[i]);
                }
            }

            for (int i = 0; i < GuildDialog.EnabledBuffs.Count; i++)
            {
                if (GuildDialog.EnabledBuffs[i].Info == null)
                {
                    GuildDialog.EnabledBuffs[i].Info = GuildDialog.FindGuildBuffInfo(GuildDialog.EnabledBuffs[i].Id);
                }
            }

            ClientBuff buff = BuffsDialog.Buffs.FirstOrDefault(e => e.Type == BuffType.Guild);

            if (GuildDialog.EnabledBuffs.Any(e => e.Active))
            {
                if (buff == null)
                {
                    buff = new ClientBuff { Type = BuffType.Guild, ObjectID = User.ObjectID, Caster = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Guild), Infinite = true, Values = new int[0] };

                    BuffsDialog.Buffs.Add(buff);
                    BuffsDialog.CreateBuff(buff);
                }

                GuildDialog.UpdateActiveStats();
            }
            else
            {
                RemoveBuff(new S.RemoveBuff { ObjectID = User.ObjectID, Type = BuffType.Guild });
            }

            User.RefreshStats();
        }

        private void TradeRequest(S.TradeRequest p)
        {
            MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PlayerRequestedTrade), p.Name), MirMessageBoxButtons.YesNo);

            messageBox.YesButton.Click += (o, e) => Network.Enqueue(new C.TradeReply { AcceptInvite = true });
            messageBox.NoButton.Click += (o, e) => { Network.Enqueue(new C.TradeReply { AcceptInvite = false }); messageBox.Dispose(); };

            messageBox.Show();
        }
        private void TradeAccept(S.TradeAccept p)
        {
            GuestTradeDialog.GuestName = p.Name;
            TradeDialog.TradeAccept();
        }
        private void TradeGold(S.TradeGold p)
        {
            GuestTradeDialog.GuestGold = p.Amount;
            TradeDialog.ChangeLockState(false);
            TradeDialog.RefreshInterface();
        }
        private void TradeItem(S.TradeItem p)
        {
            GuestTradeDialog.GuestItems = p.TradeItems;
            TradeDialog.ChangeLockState(false);
            TradeDialog.RefreshInterface();
        }
        private void TradeConfirm()
        {
            TradeDialog.TradeReset();
        }
        private void TradeCancel(S.TradeCancel p)
        {
            if (p.Unlock)
            {
                TradeDialog.ChangeLockState(false);
            }
            else
            {
                TradeDialog.TradeReset();

                MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DealCancelledMustFace), MirMessageBoxButtons.OK);
                messageBox.Show();
            }
        }
        private void NPCAwakening()
        {
            if (NPCAwakeDialog.Visible != true)
                NPCAwakeDialog.Show();
        }
        private void NPCDisassemble()
        {
            if (!NPCDialog.Visible) return;
            NPCDropDialog.PType = PanelType.Disassemble;
            NPCDropDialog.Show();
        }
        private void NPCDowngrade()
        {
            if (!NPCDialog.Visible) return;
            NPCDropDialog.PType = PanelType.Downgrade;
            NPCDropDialog.Show();
        }
        private void NPCReset()
        {
            if (!NPCDialog.Visible) return;
            NPCDropDialog.PType = PanelType.Reset;
            NPCDropDialog.Show();
        }
        private void AwakeningNeedMaterials(S.AwakeningNeedMaterials p)
        {
            NPCAwakeDialog.setNeedItems(p.Materials, p.MaterialsCount);
        }
        private void AwakeningLockedItem(S.AwakeningLockedItem p)
        {
            MirItemCell cell = InventoryDialog.GetCell(p.UniqueID);
            if (cell != null)
                cell.Locked = p.Locked;
        }
        private void Awakening(S.Awakening p)
        {
            if (NPCAwakeDialog.Visible)
                NPCAwakeDialog.Hide();
            if (InventoryDialog.Visible)
                InventoryDialog.Hide();

            InventoryDialog.Location = new Point(0, 0);

            MirItemCell cell = InventoryDialog.GetCell((ulong)p.removeID);
            if (cell != null)
            {
                cell.Locked = false;
                cell.Item = null;
            }

            for (int i = 0; i < InventoryDialog.Grid.Length; i++)
            {
                if (InventoryDialog.Grid[i].Locked == true)
                {
                    InventoryDialog.Grid[i].Locked = false;

                    //if (InventoryDialog.Grid[i].Item.UniqueID == (ulong)p.removeID)
                    //{
                    //    InventoryDialog.Grid[i].Item = null;
                    //}
                }
            }

            for (int i = 0; i < NPCAwakeDialog.ItemsIdx.Length; i++)
            {
                NPCAwakeDialog.ItemsIdx[i] = 0;
            }

            MirMessageBox messageBox = null;

            switch (p.result)
            {
                case -4:
                    messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NotEnoughMaterials), MirMessageBoxButtons.OK);
                    MapControl.AwakeningAction = false;
                    break;
                case -3:
                    messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowGold), MirMessageBoxButtons.OK);
                    MapControl.AwakeningAction = false;
                    break;
                case -2:
                    messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AwakeningMaxLevel), MirMessageBoxButtons.OK);
                    MapControl.AwakeningAction = false;
                    break;
                case -1:
                    messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotAwakenItem), MirMessageBoxButtons.OK);
                    MapControl.AwakeningAction = false;
                    break;
                case 0:
                    //messageBox = new MirMessageBox("Upgrade Failed.", MirMessageBoxButtons.OK);
                    break;
                case 1:
                    //messageBox = new MirMessageBox("Upgrade Success.", MirMessageBoxButtons.OK);
                    break;

            }

            if (messageBox != null) messageBox.Show();
        }

        private void ReceiveMail(S.ReceiveMail p)
        {
            NewMail = false;
            NewMailCounter = 0;
            User.Mail.Clear();

            User.Mail = p.Mail.OrderByDescending(e => !e.Locked).ThenByDescending(e => e.DateSent).ToList();

            foreach (ClientMail mail in User.Mail)
            {
                foreach (UserItem itm in mail.Items)
                {
                    Bind(itm);
                }
            }

            //display new mail received
            if (User.Mail.Any(e => e.Opened == false))
            {
                NewMail = true;
            }

            GameScene.Scene.MailListDialog.UpdateInterface();
        }

        private void MailLockedItem(S.MailLockedItem p)
        {
            MirItemCell cell = InventoryDialog.GetCell(p.UniqueID);
            if (cell != null)
                cell.Locked = p.Locked;
        }

        private void MailSendRequest(S.MailSendRequest p)
        {
            MirInputBox inputBox = new MirInputBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.EnterMailRecipientName));

            inputBox.OKButton.Click += (o1, e1) =>
            {
                GameScene.Scene.MailComposeParcelDialog.ComposeMail(inputBox.InputTextBox.Text);
                GameScene.Scene.InventoryDialog.Show();

                //open letter dialog, pass in name
                inputBox.Dispose();
            };

            inputBox.Show();
        }

        private void MailSent(S.MailSent p)
        {
            for (int i = 0; i < InventoryDialog.Grid.Length; i++)
            {
                if (InventoryDialog.Grid[i].Locked == true)
                {
                    InventoryDialog.Grid[i].Locked = false;
                }
            }

            for (int i = 0; i < BeltDialog.Grid.Length; i++)
            {
                if (BeltDialog.Grid[i].Locked == true)
                {
                    BeltDialog.Grid[i].Locked = false;
                }
            }

            GameScene.Scene.MailComposeParcelDialog.Hide();
        }

        private void ParcelCollected(S.ParcelCollected p)
        {
            switch (p.Result)
            {
                case -1:
                    MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NoParcelsToCollect), MirMessageBoxButtons.OK);
                    messageBox.Show();
                    break;
                case 0:
                    messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AllParcelsCollected), MirMessageBoxButtons.OK);
                    messageBox.Show();
                    break;
                case 1:
                    GameScene.Scene.MailReadParcelDialog.Hide();
                    break;
            }
        }

        private void ResizeInventory(S.ResizeInventory p)
        {
            Array.Resize(ref User.Inventory, p.Size);
            InventoryDialog.RefreshInventory2();
        }

        private void ResizeStorage(S.ResizeStorage p)
        {
            Array.Resize(ref Storage, p.Size);
            User.HasExpandedStorage = p.HasExpandedStorage;
            User.ExpandedStorageExpiryTime = p.ExpiryTime;

            StorageDialog.RefreshStorage2();
        }

        private void MailCost(S.MailCost p)
        {
            if (GameScene.Scene.MailComposeParcelDialog.Visible)
            {
                if (p.Cost > 0)
                    SoundManager.PlaySound(SoundList.Gold);

                GameScene.Scene.MailComposeParcelDialog.ParcelCostLabel.Text = p.Cost.ToString();
            }
        }

        public void AddQuestItem(UserItem item)
        {
            Redraw();

            if (item.Info.StackSize > 1) //Stackable
            {
                for (int i = 0; i < User.QuestInventory.Length; i++)
                {
                    UserItem temp = User.QuestInventory[i];
                    if (temp == null || item.Info != temp.Info || temp.Count >= temp.Info.StackSize) continue;

                    if (item.Count + temp.Count <= temp.Info.StackSize)
                    {
                        temp.Count += item.Count;
                        return;
                    }
                    item.Count -= (ushort)(temp.Info.StackSize - temp.Count);
                    temp.Count = temp.Info.StackSize;
                }
            }

            for (int i = 0; i < User.QuestInventory.Length; i++)
            {
                if (User.QuestInventory[i] != null) continue;
                User.QuestInventory[i] = item;
                return;
            }
        }

        private void RequestReincarnation()
        {
            if (CMain.Time > User.DeadTime && User.CurrentAction == MirAction.Dead)
            {
                MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.WouldYouLikeToBeRevived), MirMessageBoxButtons.YesNo);

                messageBox.YesButton.Click += (o, e) => Network.Enqueue(new C.AcceptReincarnation());

                messageBox.Show();
            }
        }

        private void NewIntelligentCreature(S.NewIntelligentCreature p)
        {
            User.IntelligentCreatures.Add(p.Creature);

            MirInputBox inputBox = new MirInputBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PleaseGiveYourCreatureName));
            inputBox.InputTextBox.Text = GameScene.User.IntelligentCreatures[User.IntelligentCreatures.Count - 1].CustomName;
            inputBox.OKButton.Click += (o1, e1) =>
            {
                if (IntelligentCreatureDialog.Visible) IntelligentCreatureDialog.Update();//refresh changes
                GameScene.User.IntelligentCreatures[User.IntelligentCreatures.Count - 1].CustomName = inputBox.InputTextBox.Text;
                Network.Enqueue(new C.UpdateIntelligentCreature { Creature = GameScene.User.IntelligentCreatures[User.IntelligentCreatures.Count - 1] });
                inputBox.Dispose();
            };
            inputBox.Show();
        }

        private void UpdateIntelligentCreatureList(S.UpdateIntelligentCreatureList p)
        {
            User.CreatureSummoned = p.CreatureSummoned;
            User.SummonedCreatureType = p.SummonedCreatureType;
            User.PearlCount = p.PearlCount;
            if (p.CreatureList.Count != User.IntelligentCreatures.Count)
            {
                User.IntelligentCreatures.Clear();
                for (int i = 0; i < p.CreatureList.Count; i++)
                    User.IntelligentCreatures.Add(p.CreatureList[i]);

                for (int i = 0; i < IntelligentCreatureDialog.CreatureButtons.Length; i++)
                    IntelligentCreatureDialog.CreatureButtons[i].Clear();

                IntelligentCreatureDialog.Hide();
            }
            else
            {
                for (int i = 0; i < p.CreatureList.Count; i++)
                    User.IntelligentCreatures[i] = p.CreatureList[i];
                if (IntelligentCreatureDialog.Visible) IntelligentCreatureDialog.Update();
            }
        }

        private void IntelligentCreatureEnableRename(S.IntelligentCreatureEnableRename p)
        {
            IntelligentCreatureDialog.CreatureRenameButton.Visible = true;
            if (IntelligentCreatureDialog.Visible) IntelligentCreatureDialog.Update();
        }

        private void IntelligentCreaturePickup(S.IntelligentCreaturePickup p)
        {
            if (MapControl.Objects.TryGetValue(p.ObjectID, out MapObject ob) && ob is MonsterObject monOb)
                monOb.PlayPickupSound();
        }


        private void FriendUpdate(S.FriendUpdate p)
        {
            FriendDialog.Friends = p.Friends;

            if (FriendDialog.Visible)
                FriendDialog.Update(false);
        }

        private void LoverUpdate(S.LoverUpdate p)
        {
            RelationshipDialog.LoverName = p.Name;
            RelationshipDialog.Date = p.Date;
            RelationshipDialog.MapName = p.MapName;
            RelationshipDialog.MarriedDays = p.MarriedDays;
            RelationshipDialog.UpdateInterface();
        }

        private void MentorUpdate(S.MentorUpdate p)
        {
            MentorDialog.MentorName = p.Name;
            MentorDialog.MentorLevel = p.Level;
            MentorDialog.MentorOnline = p.Online;
            MentorDialog.MenteeEXP = p.MenteeEXP;
            MentorDialog.UpdateInterface();
        }

        private void GameShopUpdate(S.GameShopInfo p)
        {
            p.Item.Stock = p.StockLevel;
            GameShopInfoList.Add(p.Item);
            if (p.Item.Date > CMain.Now.AddDays(-7)) GameShopDialog.New.Visible = true;
        }

        private void GameShopStock(S.GameShopStock p)
        {
            for (int i = 0; i < GameShopInfoList.Count; i++)
            {
                if (GameShopInfoList[i].GIndex == p.GIndex)
                {
                    if (p.StockLevel == 0) GameShopInfoList.Remove(GameShopInfoList[i]);
                    else GameShopInfoList[i].Stock = p.StockLevel;

                    if (GameShopDialog.Visible) GameShopDialog.UpdateShop();
                }
            }
        }
        public void AddItem(UserItem item)
        {
            Redraw();

            if (item.Info.StackSize > 1) //Stackable
            {
                for (int i = 0; i < User.Inventory.Length; i++)
                {
                    UserItem temp = User.Inventory[i];
                    if (temp == null || item.Info != temp.Info || temp.Count >= temp.Info.StackSize) continue;

                    if (item.Count + temp.Count <= temp.Info.StackSize)
                    {
                        temp.Count += item.Count;
                        return;
                    }
                    item.Count -= (ushort)(temp.Info.StackSize - temp.Count);
                    temp.Count = temp.Info.StackSize;
                }
            }

            if (item.Info.Type == ItemType.Potion || item.Info.Type == ItemType.Scroll || (item.Info.Type == ItemType.Script && item.Info.Effect == 1))
            {
                for (int i = 0; i < User.BeltIdx - 2; i++)
                {
                    if (User.Inventory[i] != null) continue;
                    User.Inventory[i] = item;
                    return;
                }
            }
            else if (item.Info.Type == ItemType.Amulet)
            {
                for (int i = 4; i < User.BeltIdx; i++)
                {
                    if (User.Inventory[i] != null) continue;
                    User.Inventory[i] = item;
                    return;
                }
            }
            else
            {
                for (int i = User.BeltIdx; i < User.Inventory.Length; i++)
                {
                    if (User.Inventory[i] != null) continue;
                    User.Inventory[i] = item;
                    return;
                }
            }

            for (int i = 0; i < User.Inventory.Length; i++)
            {
                if (User.Inventory[i] != null) continue;
                User.Inventory[i] = item;
                return;
            }
        }
        public static void Bind(UserItem item)
        {
            for (int i = 0; i < ItemInfoList.Count; i++)
            {
                if (ItemInfoList[i].Index != item.ItemIndex) continue;

                item.Info = ItemInfoList[i];

                for (int s = 0; s < item.Slots.Length; s++)
                {
                    if (item.Slots[s] == null) continue;

                    Bind(item.Slots[s]);
                }

                return;
            }
        }

        public ItemInfo GetItemInfo(int index)
        {
            return ItemInfoList.Find(x => x.Index == index);
        }

        public static void BindQuest(ClientQuestProgress quest)
        {
            for (int i = 0; i < QuestInfoList.Count; i++)
            {
                if (QuestInfoList[i].Index != quest.Id) continue;

                quest.QuestInfo = QuestInfoList[i];

                return;
            }
        }

        public Color GradeNameColor(ItemGrade grade)
        {
            switch (grade)
            {
                case ItemGrade.Common:
                    return Color.Yellow;
                case ItemGrade.Rare:
                    return Color.DeepSkyBlue;
                case ItemGrade.Legendary:
                    return Color.DarkOrange;
                case ItemGrade.Mythical:
                    return Color.Plum;
                case ItemGrade.Heroic:
                    return Color.Red;
                default:
                    return Color.Yellow;
            }
        }

        public void DisposeItemLabel()
        {
            if (ItemLabel != null && !ItemLabel.IsDisposed)
                ItemLabel.Dispose();
            ItemLabel = null;
        }
        public void DisposeMailLabel()
        {
            if (MailLabel != null && !MailLabel.IsDisposed)
                MailLabel.Dispose();
            MailLabel = null;
        }
        public void DisposeMemoLabel()
        {
            if (MemoLabel != null && !MemoLabel.IsDisposed)
                MemoLabel.Dispose();
            MemoLabel = null;
        }
        public void DisposeGuildBuffLabel()
        {
            if (GuildBuffLabel != null && !GuildBuffLabel.IsDisposed)
                GuildBuffLabel.Dispose();
            GuildBuffLabel = null;
        }

        public MirControl NameInfoLabel(UserItem item, bool inspect = false, bool hideDura = false)
        {
            ushort level = inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);

            string GradeString = "";
            switch (HoverItem.Info.Grade)
            {
                case ItemGrade.None:
                    break;
                case ItemGrade.Common:
                    GradeString = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemGradeCommon);
                    break;
                case ItemGrade.Rare:
                    GradeString = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemGradeRare);
                    break;
                case ItemGrade.Legendary:
                    GradeString = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemGradeLegendary);
                    break;
                case ItemGrade.Mythical:
                    GradeString = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemGradeMythical);
                    break;
                case ItemGrade.Heroic:
                    GradeString = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemGradeHeroic);
                    break;
            }
            MirLabel nameLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = GradeNameColor(HoverItem.Info.Grade),
                Location = new Point(4, 4),
                OutLine = true,
                Parent = ItemLabel,
                Text = HoverItem.Info.Grade != ItemGrade.None ? string.Format("{0}{1}{2}", HoverItem.Info.FriendlyName, "\n", GradeString) : HoverItem.Info.FriendlyName,
            };

            if (HoverItem.RefineAdded > 0)
                nameLabel.Text = "(*)" + nameLabel.Text;

            ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, nameLabel.DisplayRectangle.Right + 4),
                Math.Max(ItemLabel.Size.Height, nameLabel.DisplayRectangle.Bottom));

            string text = "";

            if (HoverItem.Info.Durability > 0 && !hideDura)
            {
                switch (HoverItem.Info.Type)
                {
                    case ItemType.Amulet:
                        if (HoverItem.CurrentDura > 0 || HoverItem.MaxDura > 0)
                            text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UsageCurrentMax, HoverItem.CurrentDura, HoverItem.MaxDura);
                        break;
                    case ItemType.Ore:
                        if (HoverItem.CurrentDura > 0)
                            text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PurityValue, Math.Floor(HoverItem.CurrentDura / 1000M));
                        break;
                    case ItemType.Meat:
                        if (HoverItem.CurrentDura > 0)
                            text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.QualityValue, Math.Floor(HoverItem.CurrentDura / 1000M));
                        break;
                    case ItemType.Mount:
                        if (HoverItem.CurrentDura > 0 || HoverItem.MaxDura > 0)
                            text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LoyaltyValues, HoverItem.CurrentDura, HoverItem.MaxDura);
                        break;
                    case ItemType.Food:
                        if (HoverItem.CurrentDura > 0)
                            text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NutritionValue, HoverItem.CurrentDura);
                        break;
                    case ItemType.Gem:
                    case ItemType.Potion:
                    case ItemType.Transform:
                    case ItemType.SealedHero:
                        break;
                    case ItemType.Pets:
                        if ((HoverItem.Info.Shape == 26 || HoverItem.Info.Shape == 28) && HoverItem.CurrentDura > 0)//WonderDrug, Knapsack
                        {
                            string strTime = Functions.PrintTimeSpanFromSeconds((HoverItem.CurrentDura * 3600), false);
                            text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DurationValue, strTime);
                        }
                        break;
                    default:
                        int current = (int)Math.Floor(HoverItem.CurrentDura / 1000M);
                        int maximum = (int)Math.Floor(HoverItem.MaxDura / 1000M);
                        if (current > 0 || maximum > 0)
                            text = string.Format("{0} {1}/{2}", GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Durability), current, maximum);
                        break;
                }
            }

            string baseText = string.Empty;
            switch (HoverItem.Info.Type)
            {
                case ItemType.Nothing:
                    break;
                case ItemType.Weapon:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeWeapon);
                    break;
                case ItemType.Armour:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeArmour);
                    break;
                case ItemType.Helmet:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeHelmet);
                    break;
                case ItemType.Necklace:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeNecklace);
                    break;
                case ItemType.Bracelet:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeBracelet);
                    break;
                case ItemType.Ring:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeRing);
                    break;
                case ItemType.Amulet:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeAmulet);
                    break;
                case ItemType.Belt:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeBelt);
                    break;
                case ItemType.Boots:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeBoots);
                    break;
                case ItemType.Stone:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeStone);
                    break;
                case ItemType.Torch:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeTorch);
                    break;
                case ItemType.Potion:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypePotion);
                    break;
                case ItemType.Ore:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeOre);
                    break;
                case ItemType.Meat:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeMeat);
                    break;
                case ItemType.CraftingMaterial:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeCraftingMaterial);
                    break;
                case ItemType.Scroll:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeScroll);
                    break;
                case ItemType.Gem:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeGem);
                    break;
                case ItemType.Mount:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeMount);
                    break;
                case ItemType.Book:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeBook);
                    break;
                case ItemType.Script:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeScript);
                    break;
                case ItemType.Reins:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeReins);
                    break;
                case ItemType.Bells:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeBells);
                    break;
                case ItemType.Saddle:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeSaddle);
                    break;
                case ItemType.Ribbon:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeRibbon);
                    break;
                case ItemType.Mask:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeMask);
                    break;
                case ItemType.Food:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeFood);
                    break;
                case ItemType.Hook:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeHook);
                    break;
                case ItemType.Float:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeFloat);
                    break;
                case ItemType.Bait:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeBait);
                    break;
                case ItemType.Finder:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeFinder);
                    break;
                case ItemType.Reel:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeReel);
                    break;
                case ItemType.Fish:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeFish);
                    break;
                case ItemType.Quest:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeQuest);
                    break;
                case ItemType.Awakening:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeAwakening);
                    break;
                case ItemType.Pets:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypePets);
                    break;
                case ItemType.Transform:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeTransform);
                    break;
                case ItemType.Deco:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeDeco);
                    break;
                case ItemType.MonsterSpawn:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeMonsterSpawn);
                    break;
                case ItemType.SealedHero:
                    baseText = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTypeSealedHero);
                    break;
            }

            if (HoverItem.WeddingRing != -1)
            {
                baseText += GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.WeddingRing);
            }

            List<string> tailParts = new List<string>();
            if (HoverItem.Weight > 0)
                tailParts.Add(string.Format("{0} {1}", GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Weight), HoverItem.Weight));
            if (!string.IsNullOrEmpty(text))
                tailParts.Add(text);

            if (tailParts.Count > 0)
            {
                string tailText = string.Join("  ", tailParts.ToArray());
                baseText = string.IsNullOrEmpty(baseText)
                    ? tailText
                    : string.Format("{0}\n{1}", baseText, tailText);
            }

            MirLabel etcLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Location = new Point(4, nameLabel.DisplayRectangle.Bottom),
                OutLine = true,
                Parent = ItemLabel,
                Text = baseText
            };

            ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, etcLabel.DisplayRectangle.Right + 4),
                Math.Max(ItemLabel.Size.Height, etcLabel.DisplayRectangle.Bottom + 4));

            #region OUTLINE
            MirControl outLine = new MirControl
            {
                BackColour = Color.FromArgb(255, 50, 50, 50),
                Border = true,
                BorderColour = Color.Gray,
                NotControl = true,
                Parent = ItemLabel,
                Opacity = 0.4F,
                Location = new Point(0, 0)
            };
            outLine.Size = ItemLabel.Size;
            #endregion

            return outLine;
        }

        private Stats GetTotalAddedStats(UserItem item, ushort level, MirClass job)
        {
            Stats total = new Stats();

            if (item == null) return total;

            total.Add(item.AddedStats);

            if (item.Slots == null || item.Slots.Length == 0) return total;

            for (int i = 0; i < item.Slots.Length; i++)
            {
                UserItem socketItem = item.Slots[i];
                if (socketItem == null) continue;

                ItemInfo socketInfo = Functions.GetRealItem(socketItem.Info, level, job, ItemInfoList);

                if (socketItem.CurrentDura == 0 && socketInfo.Durability > 0) continue;

                total.Add(socketInfo.Stats);
                total.Add(socketItem.AddedStats);
            }

            return total;
        }
        public MirControl AttackInfoLabel(UserItem item, bool Inspect = false, bool hideAdded = false)
        {
            ushort level = Inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = Inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);
            Stats addedStats = GetTotalAddedStats(item, level, job);

            ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

            bool fishingItem = false;

            switch (HoverItem.Info.Type)
            {
                case ItemType.Hook:
                case ItemType.Float:
                case ItemType.Bait:
                case ItemType.Finder:
                case ItemType.Reel:
                    fishingItem = true;
                    break;
                case ItemType.Weapon:
                    if (Globals.FishingRodShapes.Contains(HoverItem.Info.Shape))
                        fishingItem = true;
                    break;
                default:
                    fishingItem = false;
                    break;
            }

            int count = 0;
            int minValue = 0;
            int maxValue = 0;
            int addValue = 0;
            string text = "";

            #region Dura gem
            minValue = realItem.Durability;

            if (minValue > 0 && realItem.Type == ItemType.Gem)
            {
                switch (realItem.Shape)
                {
                    default:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AddsDurability, minValue / 1000);
                        break;
                    case 8:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SealsFor, Functions.PrintTimeSpanFromSeconds(minValue * 60));
                        break;
                }

                count++;
                MirLabel DuraLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DuraLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DuraLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region DC
            minValue = realItem.Stats[Stat.MinDC];
            maxValue = realItem.Stats[Stat.MaxDC];
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.MaxDC] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DC, minValue, maxValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                else
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AddsDC, minValue + maxValue + addValue);
                MirLabel DCLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DCLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DCLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region MC

            minValue = realItem.Stats[Stat.MinMC];
            maxValue = realItem.Stats[Stat.MaxMC];
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.MaxMC] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MC, minValue, maxValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                else
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AddsMC, minValue + maxValue + addValue);
                MirLabel MCLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MCLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MCLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region SC

            minValue = realItem.Stats[Stat.MinSC];
            maxValue = realItem.Stats[Stat.MaxSC];
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.MaxSC] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SC, minValue, maxValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                else
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AddsSC, minValue + maxValue + addValue);
                MirLabel SCLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("SC + {0}~{1}", minValue, maxValue + addValue)
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, SCLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, SCLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region LUCK / SUCCESS

            minValue = realItem.Stats[Stat.Luck];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.Luck] : 0;

            if (minValue != 0 || addValue != 0)
            {
                count++;

                if (realItem.Type == ItemType.Pets && realItem.Shape == 28)
                {
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.BagWeightPercent, minValue + addValue);
                }
                else if (realItem.Type == ItemType.Potion && realItem.Shape == 4)
                {
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ExpPercent, minValue + addValue);
                }
                else if (realItem.Type == ItemType.Potion && realItem.Shape == 5)
                {
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DropPercent, minValue + addValue);
                }
                else
                {
                    text = string.Format(minValue + addValue > 0 ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Luck) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CursePlusValue),
                        Math.Abs(minValue + addValue));
                }

                MirLabel LUCKLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, LUCKLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, LUCKLabel.DisplayRectangle.Bottom));
            }

            #endregion



            #region ACC

            minValue = realItem.Stats[Stat.Accuracy];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.Accuracy] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Accuracy, minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                else
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AddsAccuracyValue, minValue + maxValue + addValue);
                MirLabel ACCLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Accuracy + {0}", minValue + addValue)
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, ACCLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, ACCLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region HOLY

            minValue = realItem.Stats[Stat.Holy];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel HOLYLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Holy + {0}", minValue + addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Holy, minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, HOLYLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, HOLYLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region ASPEED

            minValue = realItem.Stats[Stat.AttackSpeed];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.AttackSpeed] : 0;

            if (minValue != 0 || maxValue != 0 || addValue != 0)
            {
                string plus = (addValue + minValue < 0) ? String.Empty : "+";

                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                {
                    string negative = "+";
                    if (addValue < 0) negative = String.Empty;

                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AttackSpeedValue), plus, minValue + addValue) + (addValue != 0 ? $" ({negative}{addValue})" : string.Empty);
                    //text = string.Format(addValue != 0 ? "A.Speed: " + plus + "{0} ({2}{1})" : "A.Speed: " + plus + "{0}", minValue + addValue, addValue, negative);
                    //text = string.Format(addValue > 0 ? "A.Speed: + {0} (+{1})" : "A.Speed: + {0}", minValue + addValue, addValue);
                }
                else
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AddAttackSpeed), minValue + maxValue + addValue);

                MirLabel ASPEEDLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("A.Speed + {0}", minValue + addValue)
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, ASPEEDLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, ASPEEDLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region FREEZING

            minValue = realItem.Stats[Stat.Freezing];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.Freezing] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                {
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.FreezingPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                }
                else text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AddsFreezingPlus), minValue + maxValue + addValue);

                MirLabel FREEZINGLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Freezing + {0}", minValue + addValue)
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, FREEZINGLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, FREEZINGLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region POISON

            minValue = realItem.Stats[Stat.PoisonAttack];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.PoisonAttack] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                {
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PoisonPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                }
                else
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AddsPoisonPlus), minValue + maxValue + addValue);

                MirLabel POISONLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Poison + {0}", minValue + addValue)
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, POISONLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, POISONLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region CRITICALRATE / FLEXIBILITY

            minValue = realItem.Stats[Stat.CriticalRate];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.CriticalRate] : 0;

            if ((minValue > 0 || maxValue > 0 || addValue > 0) && (realItem.Type != ItemType.Gem))
            {
                count++;
                MirLabel CRITICALRATELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Critical Chance + {0}", minValue + addValue)
                    //Text = string.Format(addValue > 0 ? "Critical Chance: + {0} (+{1})" : "Critical Chance: + {0}", minValue + addValue, addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.CriticalChancePlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty)
                };

                if (fishingItem)
                {
                    CRITICALRATELabel.Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.FlexibilityPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                }

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, CRITICALRATELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, CRITICALRATELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region CRITICALDAMAGE

            minValue = realItem.Stats[Stat.CriticalDamage];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.CriticalDamage] : 0;

            if ((minValue > 0 || maxValue > 0 || addValue > 0) && (realItem.Type != ItemType.Gem))
            {
                count++;
                MirLabel CRITICALDAMAGELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Critical Damage + {0}", minValue + addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.CriticalDamagePlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, CRITICALDAMAGELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, CRITICALDAMAGELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region Reflect

            minValue = realItem.Stats[Stat.Reflect];
            maxValue = 0;
            addValue = 0;

            if ((minValue > 0 || maxValue > 0 || addValue > 0) && (realItem.Type != ItemType.Gem))
            {
                count++;
                MirLabel ReflectLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.ReflectChance), minValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, ReflectLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, ReflectLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region Hpdrain

            minValue = realItem.Stats[Stat.HPDrainRatePercent];
            maxValue = 0;
            addValue = 0;

            if ((minValue > 0 || maxValue > 0 || addValue > 0) && (realItem.Type != ItemType.Gem))
            {
                count++;
                MirLabel HPdrainLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.HPDrainRate), minValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, HPdrainLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, HPdrainLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region Exp Rate

            minValue = realItem.Stats[Stat.ExpRatePercent];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.ExpRatePercent] : 0;

            if (minValue != 0 || maxValue != 0 || addValue != 0)
            {
                string plus = (addValue + minValue < 0) ? string.Empty : "+";

                count++;
                string negative = "+";
                if (addValue < 0) negative = String.Empty;
                text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.ExpRate), plus, minValue + addValue) + (addValue != 0 ? $" ({negative}{addValue}%)" : String.Empty);

                MirLabel expRateLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, expRateLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, expRateLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region Drop Rate

            minValue = realItem.Stats[Stat.ItemDropRatePercent];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.ItemDropRatePercent] : 0;

            if (minValue != 0 || maxValue != 0 || addValue != 0)
            {
                string plus = (addValue + minValue < 0) ? String.Empty : "+";

                count++;
                string negative = "+";
                if (addValue < 0) negative = String.Empty;
                text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.DropRate), plus, minValue + addValue) + (addValue != 0 ? $" ({negative}{addValue}%)" : string.Empty);

                MirLabel dropRateLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, dropRateLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, dropRateLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region Gold Rate

            minValue = realItem.Stats[Stat.GoldDropRatePercent];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.GoldDropRatePercent] : 0;

            if (minValue != 0 || maxValue != 0 || addValue != 0)
            {
                string plus = (addValue + minValue < 0) ? String.Empty : "+";

                count++;
                string negative = "+";
                if (addValue < 0) negative = "";
                text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.GoldRate), plus, minValue + addValue) + (addValue != 0 ? $" ({negative}{addValue}%)" : String.Empty);

                MirLabel goldRateLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, goldRateLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, goldRateLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region Hero

            if (addedStats[Stat.Hero] > 0)
            {
                ClientHeroInformation heroInfo = HeroInfoList.FirstOrDefault(x => x.Index == addedStats[Stat.Hero]);
                if (heroInfo != null)
                {
                    count++;
                    text = heroInfo.ToString();

                    MirLabel heroLabel = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = text
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, heroLabel.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, heroLabel.DisplayRectangle.Bottom));
                }
            }

            #endregion

            if (count > 0)
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                #region OUTLINE
                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;
                #endregion

                return outLine;
            }
            else
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height - 4);
            }
            return null;
        }
        public MirControl DefenceInfoLabel(UserItem item, bool Inspect = false, bool hideAdded = false)
        {
            ushort level = Inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = Inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);
            Stats addedStats = GetTotalAddedStats(item, level, job);

            ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

            bool fishingItem = false;

            switch (HoverItem.Info.Type)
            {
                case ItemType.Hook:
                case ItemType.Float:
                case ItemType.Bait:
                case ItemType.Finder:
                case ItemType.Reel:
                    fishingItem = true;
                    break;
                case ItemType.Weapon:
                    if (HoverItem.Info.Shape == 49 || HoverItem.Info.Shape == 50)
                        fishingItem = true;
                    break;
                default:
                    fishingItem = false;
                    break;
            }

            int count = 0;
            int minValue = 0;
            int maxValue = 0;
            int addValue = 0;

            string text = "";
            #region AC

            minValue = realItem.Stats[Stat.MinAC];
            maxValue = realItem.Stats[Stat.MaxAC];
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.MaxAC] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AC), minValue, maxValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                else
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AddsAC), minValue + maxValue + addValue);
                MirLabel ACLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("AC + {0}~{1}", minValue, maxValue + addValue)
                    Text = text
                };

                if (fishingItem)
                {
                    if (HoverItem.Info.Type == ItemType.Float)
                    {
                        ACLabel.Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.NibbleChance), minValue, maxValue + addValue) +
                                       (addValue > 0 ? $" (+{addValue})" : String.Empty);
                    }
                    else if (HoverItem.Info.Type == ItemType.Finder)
                    {
                        ACLabel.Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.FinderIncrease), minValue, maxValue + addValue) +
                                       (addValue > 0 ? $" (+{addValue})" : String.Empty);
                    }
                    else
                    {
                        ACLabel.Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.SuccessChance), maxValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty);
                    }
                }

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, ACLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, ACLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region MAC

            minValue = realItem.Stats[Stat.MinMAC];
            maxValue = realItem.Stats[Stat.MaxMAC];
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.MaxMAC] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MAC), minValue, maxValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                else
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AddsMAC), minValue + maxValue + addValue);
                MirLabel MACLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("MAC + {0}~{1}", minValue, maxValue + addValue)
                    Text = text
                };

                if (fishingItem)
                {
                    MACLabel.Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AutoReelChance), maxValue + addValue);
                }

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MACLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MACLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region MAXHP

            if (HoverItem.Info.Type != ItemType.MonsterSpawn)
            {
                minValue = realItem.Stats[Stat.HP];
                maxValue = 0;
                addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.HP] : 0;

                if (minValue > 0 || maxValue > 0 || addValue > 0)
                {
                    count++;
                    MirLabel MAXHPLabel = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        //Text = string.Format(realItem.Type == ItemType.Potion ? "HP + {0} Recovery" : "MAXHP + {0}", minValue + addValue)
                        Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxHpPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAXHPLabel.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, MAXHPLabel.DisplayRectangle.Bottom));
                }
            }

            #endregion

            #region MAXMP

            minValue = realItem.Stats[Stat.MP];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.MP] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel MAXMPLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format(realItem.Type == ItemType.Potion ? "MP + {0} Recovery" : "MAXMP + {0}", minValue + addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxMpPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAXMPLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MAXMPLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region MAXHPRATE

            minValue = realItem.Stats[Stat.HPRatePercent];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel MAXHPRATELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxHpPlusPercent), minValue + addValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAXHPRATELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MAXHPRATELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region MAXMPRATE

            minValue = realItem.Stats[Stat.MPRatePercent];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel MAXMPRATELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxMpPlusPercent), minValue + addValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAXMPRATELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MAXMPRATELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region MAXACRATE

            minValue = realItem.Stats[Stat.MaxACRatePercent];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel MAXACRATE = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxAcPlusPercent), minValue + addValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAXACRATE.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MAXACRATE.DisplayRectangle.Bottom));
            }

            #endregion

            #region MAXMACRATE

            minValue = realItem.Stats[Stat.MaxMACRatePercent];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel MAXMACRATELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxMacPlusPercent), minValue + addValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAXMACRATELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MAXMACRATELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region HEALTH_RECOVERY

            minValue = realItem.Stats[Stat.HealthRecovery];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.HealthRecovery] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel HEALTH_RECOVERYLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.HealthRecoveryPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, HEALTH_RECOVERYLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, HEALTH_RECOVERYLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region MANA_RECOVERY

            minValue = realItem.Stats[Stat.SpellRecovery];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.SpellRecovery] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel MANA_RECOVERYLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("ManaRecovery + {0}", minValue + addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.ManaRecoveryPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MANA_RECOVERYLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MANA_RECOVERYLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region POISON_RECOVERY

            minValue = realItem.Stats[Stat.PoisonRecovery];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.PoisonRecovery] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel POISON_RECOVERYabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Poison Recovery + {0}", minValue + addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PoisonRecoveryPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, POISON_RECOVERYabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, POISON_RECOVERYabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region AGILITY

            minValue = realItem.Stats[Stat.Agility];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.Agility] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.Agility), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : string.Empty);
                else
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AddsAgilityPlus), minValue + maxValue + addValue);

                MirLabel AGILITYLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, AGILITYLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, AGILITYLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region STRONG

            minValue = realItem.Stats[Stat.Strong];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.Strong] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel STRONGLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Strong + {0}", minValue + addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.StrongPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, STRONGLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, STRONGLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region POISON_RESIST

            minValue = realItem.Stats[Stat.PoisonResist];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.PoisonResist] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.PoisonResistPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty);
                else
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AddPoisonResistPlus), minValue + maxValue + addValue);
                MirLabel POISON_RESISTLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, POISON_RESISTLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, POISON_RESISTLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region MAGIC_RESIST

            minValue = realItem.Stats[Stat.MagicResist];
            maxValue = 0;
            addValue = (!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) ? addedStats[Stat.MagicResist] : 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                if (HoverItem.Info.Type != ItemType.Gem)
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MagicResistPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty);
                else
                    text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AddMagicResistPlus), minValue + maxValue + addValue);
                MirLabel MAGIC_RESISTLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Magic Resist + {0}", minValue + addValue)
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAGIC_RESISTLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MAGIC_RESISTLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region MAX_DC_RATE

            minValue = realItem.Stats[Stat.MaxDCRatePercent];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel MAXDCRATE = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxDcPlusPercent), minValue + addValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAXDCRATE.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MAXDCRATE.DisplayRectangle.Bottom));
            }
            #endregion

            #region MAX_MC_RATE

            minValue = realItem.Stats[Stat.MaxMCRatePercent];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel MAXMCRATE = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxMcPlusPercent), minValue + addValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAXMCRATE.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MAXMCRATE.DisplayRectangle.Bottom));
            }
            #endregion

            #region MAX_SC_RATE

            minValue = realItem.Stats[Stat.MaxSCRatePercent];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel MAXSCRATE = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxScPlusPercent), minValue + addValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, MAXSCRATE.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, MAXSCRATE.DisplayRectangle.Bottom));
            }
            #endregion

            #region DAMAGE_REDUCTION

            minValue = realItem.Stats[Stat.DamageReductionPercent];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel DAMAGEREDUC = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AllDamageReductionPlusPercent), minValue + addValue)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DAMAGEREDUC.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DAMAGEREDUC.DisplayRectangle.Bottom));
            }
            #endregion
            if (count > 0)
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                #region OUTLINE
                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;
                #endregion

                return outLine;
            }
            else
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height - 4);
            }
            return null;
        }
        public MirControl WeightInfoLabel(UserItem item, bool Inspect = false)
        {
            ushort level = Inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = Inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);

            ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

            int count = 0;
            int minValue = 0;
            int maxValue = 0;
            int addValue = 0;

            #region HANDWEIGHT

            minValue = realItem.Stats[Stat.HandWeight];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel HANDWEIGHTLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Hand Weight + {0}", minValue + addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.HandWeightPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, HANDWEIGHTLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, HANDWEIGHTLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region WEARWEIGHT

            minValue = realItem.Stats[Stat.WearWeight];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel WEARWEIGHTLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Wear Weight + {0}", minValue + addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.WearWeightPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, WEARWEIGHTLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, WEARWEIGHTLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region BAGWEIGHT

            minValue = realItem.Stats[Stat.BagWeight];
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel BAGWEIGHTLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    //Text = string.Format("Bag Weight + {0}", minValue + addValue)
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.BagWeightPlus), minValue + addValue) + (addValue > 0 ? $" (+{addValue})" : String.Empty)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, BAGWEIGHTLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, BAGWEIGHTLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region FASTRUN

            minValue = realItem.CanFastRun == true ? 1 : 0;
            maxValue = 0;
            addValue = 0;

            if (minValue > 0 || maxValue > 0 || addValue > 0)
            {
                count++;
                MirLabel BAGWEIGHTLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.InstantRun)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, BAGWEIGHTLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, BAGWEIGHTLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region TIME & RANGE
            minValue = 0;
            maxValue = 0;
            addValue = 0;

            if (HoverItem.Info.Type == ItemType.Potion && HoverItem.Info.Durability > 0)
            {
                count++;
                MirLabel TNRLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.TimeValue), Functions.PrintTimeSpanFromSeconds(HoverItem.Info.Durability * 60))
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, TNRLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, TNRLabel.DisplayRectangle.Bottom));
            }

            if (HoverItem.Info.Type == ItemType.Transform && HoverItem.Info.Durability > 0)
            {
                count++;
                MirLabel TNRLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = addValue > 0 ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.TimeValue), Functions.PrintTimeSpanFromSeconds(HoverItem.Info.Durability, false))
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, TNRLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, TNRLabel.DisplayRectangle.Bottom));
            }

            #endregion

            if (count > 0)
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                #region OUTLINE
                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;
                #endregion

                return outLine;
            }
            else
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height - 4);
            }
            return null;
        }
        public MirControl AwakeInfoLabel(UserItem item, bool Inspect = false)
        {
            ushort level = Inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = Inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);

            ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

            int count = 0;

            #region AWAKENAME
            if (HoverItem.Awake.GetAwakeLevel() > 0)
            {
                count++;
                MirLabel AWAKENAMELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = GradeNameColor(HoverItem.Info.Grade),
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AwakeningWithValue), HoverItem.Awake.Type.ToLocalizedString(), HoverItem.Awake.GetAwakeLevel())
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, AWAKENAMELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, AWAKENAMELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region AWAKE_TOTAL_VALUE
            if (HoverItem.Awake.GetAwakeValue() > 0)
            {
                count++;
                MirLabel AWAKE_TOTAL_VALUELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = string.Format(realItem.Type != ItemType.Armour ? "{0} + {1}~{2}" : "MAX {0} + {1}", HoverItem.Awake.Type.ToLocalizedString(), HoverItem.Awake.GetAwakeValue(), HoverItem.Awake.GetAwakeValue())
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, AWAKE_TOTAL_VALUELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, AWAKE_TOTAL_VALUELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region AWAKE_LEVEL_VALUE
            if (HoverItem.Awake.GetAwakeLevel() > 0)
            {
                count++;
                for (int i = 0; i < HoverItem.Awake.GetAwakeLevel(); i++)
                {
                    MirLabel AWAKE_LEVEL_VALUELabel = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = string.Format(realItem.Type != ItemType.Armour ? "Level {0} : {1} + {2}~{3}" : "Level {0} : MAX {1} + {2}~{3}", i + 1, HoverItem.Awake.Type.ToLocalizedString(), HoverItem.Awake.GetAwakeLevelValue(i), HoverItem.Awake.GetAwakeLevelValue(i))
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, AWAKE_LEVEL_VALUELabel.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, AWAKE_LEVEL_VALUELabel.DisplayRectangle.Bottom));
                }
            }

            #endregion

            if (count > 0)
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                #region OUTLINE
                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;
                #endregion

                return outLine;
            }
            else
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height - 4);
            }
            return null;
        }
        public MirControl SocketInfoLabel(UserItem item, bool Inspect = false)
        {
            ushort level = Inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = Inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);

            ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);


            int count = 0;

            #region SOCKET

            for (int i = 0; i < item.Slots.Length; i++)
            {
                count++;
                MirLabel SOCKETLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = (count > realItem.Slots && !realItem.IsFishingRod && realItem.Type != ItemType.Mount) ? Color.Cyan : Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.SocketWithValue),
                        item.Slots[i] == null ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Empty) : item.Slots[i].FriendlyName)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, SOCKETLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, SOCKETLabel.DisplayRectangle.Bottom));
            }

            #endregion

            if (count > 0)
            {
                #region SOCKET

                count++;
                MirLabel SOCKETLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.OpenSocketsTips)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, SOCKETLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, SOCKETLabel.DisplayRectangle.Bottom));

                #endregion

                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                #region OUTLINE
                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;
                #endregion

                return outLine;
            }
            else
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height - 4);
            }
            return null;
        }
        public MirControl NeedInfoLabel(UserItem item, bool Inspect = false)
        {
            ushort level = Inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = Inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);

            ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

            int count = 0;

            #region LEVEL
            if (realItem.RequiredAmount > 0)
            {
                count++;
                string text;
                Color colour = Color.White;
                switch (realItem.RequiredType)
                {
                    case RequiredType.Level:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredLevel), realItem.RequiredAmount);
                        if (MapObject.User.Level < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MaxAC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredAC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MaxAC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MaxMAC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredMAC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MaxMAC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MaxDC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredDC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MaxDC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MaxMC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredMC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MaxMC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MaxSC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredSC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MaxSC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MaxLevel:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaximumLevel), realItem.RequiredAmount);
                        if (MapObject.User.Level > realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MinAC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredBaseAC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MinAC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MinMAC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredBaseMAC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MinMAC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MinDC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredBaseDC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MinDC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MinMC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredBaseMC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MinMC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    case RequiredType.MinSC:
                        text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RequiredBaseSC), realItem.RequiredAmount);
                        if (MapObject.User.Stats[Stat.MinSC] < realItem.RequiredAmount)
                            colour = Color.Red;
                        break;
                    default:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UnknownTypeRequired);
                        break;
                }

                MirLabel LEVELLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = colour,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, LEVELLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, LEVELLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region CLASS
            if (realItem.RequiredClass != RequiredClass.None)
            {
                count++;
                Color colour = Color.White;

                switch (MapObject.User.Class)
                {
                    case MirClass.Warrior:
                        if (!realItem.RequiredClass.HasFlag(RequiredClass.Warrior))
                            colour = Color.Red;
                        break;
                    case MirClass.Wizard:
                        if (!realItem.RequiredClass.HasFlag(RequiredClass.Wizard))
                            colour = Color.Red;
                        break;
                    case MirClass.Taoist:
                        if (!realItem.RequiredClass.HasFlag(RequiredClass.Taoist))
                            colour = Color.Red;
                        break;
                    case MirClass.Assassin:
                        if (!realItem.RequiredClass.HasFlag(RequiredClass.Assassin))
                            colour = Color.Red;
                        break;
                    case MirClass.Archer:
                        if (!realItem.RequiredClass.HasFlag(RequiredClass.Archer))
                            colour = Color.Red;
                        break;
                }

                MirLabel CLASSLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = colour,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.ClassRequired), realItem.RequiredClass.ToLocalizedString())
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, CLASSLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, CLASSLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region BUYING - SELLING PRICE
            if (item.Price() > 0)
            {
                count++;
                string text;
                var colour = Color.White;

                text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.SellingPriceGold), ((long)(item.Price() / 2)).ToString("###,###,##0"));

                var costLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = colour,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, costLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, costLabel.DisplayRectangle.Bottom));
            }


            #endregion

            if (count > 0)
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                #region OUTLINE
                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;
                #endregion

                return outLine;
            }
            else
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height - 4);
            }
            return null;
        }
        public MirControl BindInfoLabel(UserItem item, bool Inspect = false, bool hideAdded = false)
        {
            ushort level = Inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = Inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);

            ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

            int count = 0;

            #region DONT_DEATH_DROP

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.DontDeathdrop))
            {
                count++;
                MirLabel DONT_DEATH_DROPLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CantDropOnDeath)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_DEATH_DROPLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_DEATH_DROPLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region DONT_DROP

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.DontDrop))
            {
                count++;
                MirLabel DONT_DROPLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CantDrop)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_DROPLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_DROPLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region DONT_UPGRADE

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.DontUpgrade))
            {
                count++;
                MirLabel DONT_UPGRADELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CantUpgrade)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_UPGRADELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_UPGRADELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region DONT_SELL

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.DontSell))
            {
                count++;
                MirLabel DONT_SELLLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CantSell)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_SELLLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_SELLLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region DONT_TRADE

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.DontTrade))
            {
                count++;
                MirLabel DONT_TRADELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CantTrade)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_TRADELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_TRADELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region DONT_STORE

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.DontStore))
            {
                count++;
                MirLabel DONT_STORELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CantStore)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_STORELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_STORELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region DONT_REPAIR

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.DontRepair))
            {
                count++;
                MirLabel DONT_REPAIRLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CantRepair)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_REPAIRLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_REPAIRLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region DONT_SPECIALREPAIR

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.NoSRepair))
            {
                count++;
                MirLabel DONT_REPAIRLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CantSpecialRepair)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_REPAIRLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_REPAIRLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region BREAK_ON_DEATH

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.BreakOnDeath))
            {
                count++;
                MirLabel DONT_REPAIRLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.BreaksOnDeath)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_REPAIRLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_REPAIRLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region DONT_DESTROY_ON_DROP

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.DestroyOnDrop))
            {
                count++;
                MirLabel DONT_DODLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DestroyedWhenDropped)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, DONT_DODLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, DONT_DODLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region NoWeddingRing

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.NoWeddingRing))
            {
                count++;
                MirLabel No_WedLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotBeWeddingRing)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, No_WedLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, No_WedLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region NoHero

            if (HoverItem.Info.Bind != BindMode.None && HoverItem.Info.Bind.HasFlag(BindMode.NoHero))
            {
                count++;
                MirLabel No_HeroLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotBeUsedByHero)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, No_HeroLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, No_HeroLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region BIND_ON_EQUIP

            if ((HoverItem.Info.Bind.HasFlag(BindMode.BindOnEquip)) & HoverItem.SoulBoundId == -1)
            {
                count++;
                MirLabel BOELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SoulBindsOnEquip)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, BOELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, BOELabel.DisplayRectangle.Bottom));
            }
            else if (HoverItem.SoulBoundId != -1)
            {
                count++;
                MirLabel BOELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SoulboundTo) + GetUserName((uint)HoverItem.SoulBoundId)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, BOELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, BOELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region CURSED

            if ((!hideAdded && (!HoverItem.Info.NeedIdentify || HoverItem.Identified)) && HoverItem.Cursed)
            {
                count++;
                MirLabel CURSEDLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Cursed)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, CURSEDLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, CURSEDLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region Gems

            if (HoverItem.Info.Type == ItemType.Gem)
            {
                #region UseOn text
                count++;
                string Text = "";
                if (HoverItem.Info.Unique == SpecialItemMode.None)
                {
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotBeUsedOnAnyItem);
                }
                else
                {
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CanBeUsedOn);
                }
                MirLabel GemUseOn = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = Text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, GemUseOn.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, GemUseOn.DisplayRectangle.Bottom));
                #endregion
                #region Weapon text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.Paralize))
                {
                    MirLabel GemWeapon = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterWeapon)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, GemWeapon.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, GemWeapon.DisplayRectangle.Bottom));
                }
                #endregion
                #region Armour text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.Teleport))
                {
                    MirLabel GemArmour = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterArmour)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, GemArmour.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, GemArmour.DisplayRectangle.Bottom));
                }
                #endregion
                #region Helmet text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.ClearRing))
                {
                    MirLabel Gemhelmet = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterHelmet)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, Gemhelmet.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, Gemhelmet.DisplayRectangle.Bottom));
                }
                #endregion
                #region Necklace text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.Protection))
                {
                    MirLabel Gemnecklace = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterNecklace)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, Gemnecklace.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, Gemnecklace.DisplayRectangle.Bottom));
                }
                #endregion
                #region Bracelet text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.Revival))
                {
                    MirLabel GemBracelet = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterBracelet)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, GemBracelet.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, GemBracelet.DisplayRectangle.Bottom));
                }
                #endregion
                #region Ring text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.Muscle))
                {
                    MirLabel GemRing = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterRing)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, GemRing.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, GemRing.DisplayRectangle.Bottom));
                }
                #endregion
                #region Amulet text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.Flame))
                {
                    MirLabel Gemamulet = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterAmulet)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, Gemamulet.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, Gemamulet.DisplayRectangle.Bottom));
                }
                #endregion
                #region Belt text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.Healing))
                {
                    MirLabel Gembelt = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterBelt)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, Gembelt.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, Gembelt.DisplayRectangle.Bottom));
                }
                #endregion
                #region Boots text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.Probe))
                {
                    MirLabel Gemboots = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterBoots)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, Gemboots.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, Gemboots.DisplayRectangle.Bottom));
                }
                #endregion
                #region Stone text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.Skill))
                {
                    MirLabel Gemstone = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Stone)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, Gemstone.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, Gemstone.DisplayRectangle.Bottom));
                }
                #endregion
                #region Torch text
                count++;
                if (HoverItem.Info.Unique.HasFlag(SpecialItemMode.NoDuraLoss))
                {
                    MirLabel Gemtorch = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.White,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AfterCandle)
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, Gemtorch.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, Gemtorch.DisplayRectangle.Bottom));
                }
                #endregion
            }

            #endregion

            #region CANTAWAKEN

            //if ((HoverItem.Info.CanAwakening != true) && (HoverItem.Info.Type != ItemType.Gem))
            //{
            //    count++;
            //    MirLabel CANTAWAKENINGLabel = new MirLabel
            //    {
            //        AutoSize = true,
            //        ForeColour = Color.Yellow,
            //        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
            //        OutLine = true,
            //        Parent = ItemLabel,
            //        Text = string.Format("Can't awaken")
            //    };

            //    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, CANTAWAKENINGLabel.DisplayRectangle.Right + 4),
            //        Math.Max(ItemLabel.Size.Height, CANTAWAKENINGLabel.DisplayRectangle.Bottom));
            //}

            #endregion

            #region EXPIRE

            if (HoverItem.ExpireInfo != null)
            {
                double remainingSeconds = (HoverItem.ExpireInfo.ExpiryDate - CMain.Now).TotalSeconds;

                count++;
                MirLabel EXPIRELabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = remainingSeconds > 0 ? GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.ExpiresIn), Functions.PrintTimeSpanFromSeconds(remainingSeconds)) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Expired)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, EXPIRELabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, EXPIRELabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region SEALED

            if (HoverItem.SealedInfo != null)
            {
                double remainingSeconds = (HoverItem.SealedInfo.ExpiryDate - CMain.Now).TotalSeconds;

                if (remainingSeconds > 0)
                {
                    count++;
                    MirLabel SEALEDLabel = new MirLabel
                    {
                        AutoSize = true,
                        ForeColour = Color.Red,
                        Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                        OutLine = true,
                        Parent = ItemLabel,
                        Text = remainingSeconds > 0 ? GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.SealedFor), Functions.PrintTimeSpanFromSeconds(remainingSeconds)) : string.Empty
                    };

                    ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, SEALEDLabel.DisplayRectangle.Right + 4),
                        Math.Max(ItemLabel.Size.Height, SEALEDLabel.DisplayRectangle.Bottom));
                }
            }

            #endregion

            if (HoverItem.RentalInformation?.RentalLocked == false)
            {
                count++;
                MirLabel OWNERLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.DarkKhaki,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemRentedFrom) + HoverItem.RentalInformation.OwnerName
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, OWNERLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, OWNERLabel.DisplayRectangle.Bottom));

                double remainingTime = (HoverItem.RentalInformation.ExpiryDate - CMain.Now).TotalSeconds;

                count++;
                MirLabel RENTALLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Khaki,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = remainingTime > 0 ? GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RentalExpiresIn), Functions.PrintTimeSpanFromSeconds(remainingTime)) : "Rental expired"
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, RENTALLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, RENTALLabel.DisplayRectangle.Bottom));
            }
            else if (HoverItem.RentalInformation?.RentalLocked == true && HoverItem.RentalInformation.ExpiryDate > CMain.Now)
            {
                count++;
                var remainingTime = (HoverItem.RentalInformation.ExpiryDate - CMain.Now).TotalSeconds;
                var RentalLockLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.DarkKhaki,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = remainingTime > 0
                        ? GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.RentalLockExpiresIn), Functions.PrintTimeSpanFromSeconds(remainingTime))
                        : "Rental lock expired"
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, RentalLockLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, RentalLockLabel.DisplayRectangle.Bottom));
            }

            if (count > 0)
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                #region OUTLINE
                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;
                #endregion

                return outLine;
            }
            else
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height - 4);
            }
            return null;
        }
        public MirControl OverlapInfoLabel(UserItem item, bool Inspect = false)
        {
            ushort level = Inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = Inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);

            ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

            int count = 0;


            #region GEM

            if (realItem.Type == ItemType.Gem)
            {
                string text = "";

                switch (realItem.Shape)
                {
                    case 1:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HoldCtrlPartialRepairWeaponsAccessories);
                        break;
                    case 2:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HoldCtrlPartialRepairArmourDrapery);
                        break;
                    case 3:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HoldCtrlCombineDestroyChance);
                        break;
                    case 4:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HoldCtrlCombineNoDestroy);
                        break;
                    case 5:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HoldCtrlFullRepairWeaponsAccessories);
                        break;
                    case 6:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HoldCtrlFullRepairArmourDrapery);
                        break;
                    case 8:
                        text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HoldCtrlSealItem);
                        break;
                }
                count++;
                MirLabel GEMLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = text
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, GEMLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, GEMLabel.DisplayRectangle.Bottom));
            }

            #endregion

            #region SPLITUP

            if (realItem.StackSize > 1 && realItem.Type != ItemType.Gem)
            {
                count++;
                MirLabel SPLITUPLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.White,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.MaxCombine), realItem.StackSize, "\n")
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, SPLITUPLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, SPLITUPLabel.DisplayRectangle.Bottom));
            }

            #endregion

            if (count > 0)
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                #region OUTLINE
                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;
                #endregion

                return outLine;
            }
            else
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height - 4);
            }
            return null;
        }
        public MirControl StoryInfoLabel(UserItem item, bool Inspect = false)
        {
            ushort level = Inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = Inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);

            ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

            int count = 0;

            #region TOOLTIP

            if (realItem.Type == ItemType.Scroll && realItem.Shape == 7)//Credit Scroll
            {
                HoverItem.Info.ToolTip = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AddsCreditsToAccount), HoverItem.Info.Price);
            }

            if (!string.IsNullOrEmpty(HoverItem.Info.ToolTip))
            {
                count++;

                MirLabel IDLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.DarkKhaki,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemDescription)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, IDLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, IDLabel.DisplayRectangle.Bottom));

                MirLabel TOOLTIPLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Khaki,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = HoverItem.Info.ToolTip
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, TOOLTIPLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height, TOOLTIPLabel.DisplayRectangle.Bottom));
            }

            #endregion

            if (count > 0)
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                #region OUTLINE
                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;
                #endregion

                return outLine;
            }
            else
            {
                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height - 4);
            }
            return null;
        }

        public MirControl GMMadeLabel(UserItem item)
        {
            if (item.GMMade)
            {


                ItemLabel.Size = new Size(ItemLabel.Size.Width, ItemLabel.Size.Height + 4);

                MirLabel GMLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Orchid,
                    Location = new Point(4, ItemLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = ItemLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CreatedByGameMaster)
                };

                ItemLabel.Size = new Size(Math.Max(ItemLabel.Size.Width, GMLabel.DisplayRectangle.Right + 4),
                    Math.Max(ItemLabel.Size.Height + 4, GMLabel.DisplayRectangle.Bottom + 4));

                MirControl outLine = new MirControl
                {
                    BackColour = Color.FromArgb(255, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Gray,
                    NotControl = true,
                    Parent = ItemLabel,
                    Opacity = 0.4F,
                    Location = new Point(0, 0)
                };
                outLine.Size = ItemLabel.Size;

                return outLine;
            }
            else
            {
                return null;
            }
        }

        public void CreateItemLabel(UserItem item, bool inspect = false, bool hideDura = false, bool hideAdded = false)
        {
            CMain.DebugText = CMain.Random.Next(1, 100).ToString();

            if (item == null || HoverItem != item)
            {
                DisposeItemLabel();

                if (item == null)
                {
                    HoverItem = null;
                    return;
                }
            }

            if (item == HoverItem && ItemLabel != null && !ItemLabel.IsDisposed) return;
            ushort level = inspect ? InspectDialog.Level : MapObject.User.Level;
            MirClass job = inspect ? InspectDialog.Class : MapObject.User.Class;
            HoverItem = item;
            ItemInfo realItem = Functions.GetRealItem(item.Info, level, job, ItemInfoList);

            ItemLabel = new MirControl
            {
                BackColour = Color.FromArgb(255, 0, 0, 0),
                Border = true,
                BorderColour = ((HoverItem.CurrentDura == 0 && HoverItem.MaxDura != 0) ? Color.Red : Color.FromArgb(255, 148, 146, 148)),
                DrawControlTexture = true,
                NotControl = true,
                Parent = this,
                Opacity = 0.8F
            };

            //Name Info Label
            MirControl[] outlines = new MirControl[11];
            outlines[0] = NameInfoLabel(item, inspect, hideDura);
            //Attribute Info1 Label - Attack Info
            outlines[1] = AttackInfoLabel(item, inspect, hideAdded);
            //Attribute Info2 Label - Defence Info
            outlines[2] = DefenceInfoLabel(item, inspect, hideAdded);
            //Attribute Info3 Label - Weight Info
            outlines[3] = WeightInfoLabel(item, inspect);
            //Awake Info Label
            outlines[4] = AwakeInfoLabel(item, inspect);
            //Socket Info Label
            outlines[5] = SocketInfoLabel(item, inspect);
            //need Info Label
            outlines[6] = NeedInfoLabel(item, inspect);
            //Bind Info Label
            outlines[7] = BindInfoLabel(item, inspect, hideAdded);
            //Overlap Info Label
            outlines[8] = OverlapInfoLabel(item, inspect);
            //Story Label
            outlines[9] = StoryInfoLabel(item, inspect);
            //GM Made
            outlines[10] = GMMadeLabel(item);

            foreach (var outline in outlines)
            {
                if (outline != null)
                {
                    outline.Size = new Size(ItemLabel.Size.Width, outline.Size.Height);
                }
            }

            //ItemLabel.Visible = true;
        }
        public void CreateMailLabel(ClientMail mail)
        {
            if (mail == null)
            {
                DisposeMailLabel();
                return;
            }

            if (MailLabel != null && !MailLabel.IsDisposed) return;

            MailLabel = new MirControl
            {
                BackColour = Color.FromArgb(255, 50, 50, 50),
                Border = true,
                BorderColour = Color.Gray,
                DrawControlTexture = true,
                NotControl = true,
                Parent = this,
                Opacity = 0.7F
            };

            MirLabel nameLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.Yellow,
                Location = new Point(4, 4),
                OutLine = true,
                Parent = MailLabel,
                Text = mail.SenderName
            };

            MailLabel.Size = new Size(Math.Max(MailLabel.Size.Width, nameLabel.DisplayRectangle.Right + 4),
                Math.Max(MailLabel.Size.Height, nameLabel.DisplayRectangle.Bottom));

            MirLabel dateLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Location = new Point(4, MailLabel.DisplayRectangle.Bottom),
                OutLine = true,
                Parent = MailLabel,
                Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.DateSent), mail.DateSent.ToString("dd/MM/yy H:mm:ss"))
            };

            MailLabel.Size = new Size(Math.Max(MailLabel.Size.Width, dateLabel.DisplayRectangle.Right + 4),
                Math.Max(MailLabel.Size.Height, dateLabel.DisplayRectangle.Bottom));

            if (mail.Gold > 0)
            {
                MirLabel goldLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.White,
                    Location = new Point(4, MailLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = MailLabel,
                    Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MailGoldLabel) + mail.Gold
                };

                MailLabel.Size = new Size(Math.Max(MailLabel.Size.Width, goldLabel.DisplayRectangle.Right + 4),
                Math.Max(MailLabel.Size.Height, goldLabel.DisplayRectangle.Bottom));
            }

            MirLabel openedLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.Red,
                Location = new Point(4, MailLabel.DisplayRectangle.Bottom),
                OutLine = true,
                Parent = MailLabel,
                Text = mail.Opened ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MailHeaderOld) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MailHeaderNew)
            };

            MailLabel.Size = new Size(Math.Max(MailLabel.Size.Width, openedLabel.DisplayRectangle.Right + 4),
            Math.Max(MailLabel.Size.Height, openedLabel.DisplayRectangle.Bottom));
        }
        public void CreateMemoLabel(ClientFriend friend)
        {
            if (friend == null)
            {
                DisposeMemoLabel();
                return;
            }

            if (MemoLabel != null && !MemoLabel.IsDisposed) return;

            MemoLabel = new MirControl
            {
                BackColour = Color.FromArgb(255, 50, 50, 50),
                Border = true,
                BorderColour = Color.Gray,
                DrawControlTexture = true,
                NotControl = true,
                Parent = this,
                Opacity = 0.7F
            };

            MirLabel memoLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Location = new Point(4, 4),
                OutLine = true,
                Parent = MemoLabel,
                Text = Functions.StringOverLines(friend.Memo, 5, 20)
            };

            MemoLabel.Size = new Size(Math.Max(MemoLabel.Size.Width, memoLabel.DisplayRectangle.Right + 4),
                Math.Max(MemoLabel.Size.Height, memoLabel.DisplayRectangle.Bottom + 4));
        }

        public string GetUserName(uint id)
        {
            for (int i = 0; i < UserIdList.Count; i++)
            {
                UserId who = UserIdList[i];
                if (id == who.Id)
                    return who.UserName;
            }
            Network.Enqueue(new C.RequestUserName { UserID = id });
            UserIdList.Add(new UserId() { Id = id, UserName = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.UserNameUnknown) });
            return String.Empty;
        }

        public class UserId
        {
            public long Id = 0;
            public string UserName = String.Empty;
        }

        public class OutPutMessage
        {
            public string Message;
            public long ExpireTime;
            public OutputMessageType Type;
        }

        public void Rankings(S.Rankings p)
        {
            foreach (RankCharacterInfo info in p.ListingDetails)
            {
                if (RankingList.ContainsKey(info.PlayerId))
                    RankingList[info.PlayerId] = info;
                else
                    RankingList.Add(info.PlayerId, info);
            }
            List<RankCharacterInfo> listings = new List<RankCharacterInfo>();
            foreach (long id in p.Listings)
                listings.Add(RankingList[id]);

            RankingDialog.RecieveRanks(listings, p.RankType, p.MyRank, p.Count);
        }

        public void Opendoor(S.Opendoor p)
        {
            MapControl.OpenDoor(p.DoorIndex, p.Close);
        }

        private void RentedItems(S.GetRentedItems p)
        {
            ItemRentalDialog.ReceiveRentedItems(p.RentedItems);
        }

        private void ItemRentalRequest(S.ItemRentalRequest p)
        {
            if (!p.Renting)
            {
                GuestItemRentDialog.SetGuestName(p.Name);
                ItemRentingDialog.OpenItemRentalDialog();
            }
            else
            {
                GuestItemRentingDialog.SetGuestName(p.Name);
                ItemRentDialog.OpenItemRentDialog();
            }

            ItemRentalDialog.Visible = false;
        }

        private void ItemRentalFee(S.ItemRentalFee p)
        {
            GuestItemRentDialog.SetGuestFee(p.Amount);
            ItemRentDialog.RefreshInterface();
        }

        private void ItemRentalPeriod(S.ItemRentalPeriod p)
        {
            GuestItemRentingDialog.GuestRentalPeriod = p.Days;
            ItemRentingDialog.RefreshInterface();
        }

        private void DepositRentalItem(S.DepositRentalItem p)
        {
            var fromCell = p.From < User.BeltIdx ? BeltDialog.Grid[p.From] : InventoryDialog.Grid[p.From - User.BeltIdx];
            var toCell = ItemRentingDialog.ItemCell;

            if (toCell == null || fromCell == null)
                return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success)
                return;

            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();

            if (ItemRentingDialog.RentalPeriod == 0)
                ItemRentingDialog.InputRentalPeroid();
        }

        private void RetrieveRentalItem(S.RetrieveRentalItem p)
        {
            var fromCell = ItemRentingDialog.ItemCell;
            var toCell = p.To < User.BeltIdx ? BeltDialog.Grid[p.To] : InventoryDialog.Grid[p.To - User.BeltIdx];

            if (toCell == null || fromCell == null)
                return;

            toCell.Locked = false;
            fromCell.Locked = false;

            if (!p.Success)
                return;

            toCell.Item = fromCell.Item;
            fromCell.Item = null;
            User.RefreshStats();
        }

        private void UpdateRentalItem(S.UpdateRentalItem p)
        {
            GuestItemRentingDialog.GuestLoanItem = p.LoanItem;
            ItemRentDialog.RefreshInterface();
        }

        private void CancelItemRental(S.CancelItemRental p)
        {
            User.RentalGoldLocked = false;
            User.RentalItemLocked = false;

            ItemRentingDialog.Reset();
            ItemRentDialog.Reset();

            var messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemRentalCancelledFaceOtherParty));
            messageBox.Show();
        }

        private void ItemRentalLock(S.ItemRentalLock p)
        {
            if (!p.Success)
                return;

            User.RentalGoldLocked = p.GoldLocked;
            User.RentalItemLocked = p.ItemLocked;

            if (User.RentalGoldLocked)
                ItemRentDialog.Lock();
            else if (User.RentalItemLocked)
                ItemRentingDialog.Lock();
        }

        private void ItemRentalPartnerLock(S.ItemRentalPartnerLock p)
        {
            if (p.GoldLocked)
                GuestItemRentDialog.Lock();
            else if (p.ItemLocked)
                GuestItemRentingDialog.Lock();
        }

        private void CanConfirmItemRental(S.CanConfirmItemRental p)
        {
            ItemRentingDialog.EnableConfirmButton();
        }

        private void ConfirmItemRental(S.ConfirmItemRental p)
        {
            User.RentalGoldLocked = false;
            User.RentalItemLocked = false;

            ItemRentingDialog.Reset();
            ItemRentDialog.Reset();
        }

        private void OpenBrowser(S.OpenBrowser p)
        {
            BrowserHelper.OpenDefaultBrowser(p.Url);
        }

        public void PlaySound(S.PlaySound p)
        {
            SoundManager.PlaySound(p.Sound, false);
        }
        private void SetTimer(S.SetTimer p)
        {
            GameScene.Scene.TimerControl.AddTimer(p);
        }

        private void ExpireTimer(S.ExpireTimer p)
        {
            GameScene.Scene.TimerControl.ExpireTimer(p.Key);
        }

        private void SetCompass(S.SetCompass p)
        {
            GameScene.Scene.CompassControl.SetPoint(p.Location);
        }

        private void Roll(S.Roll p)
        {
            GameScene.Scene.RollControl.Setup(p.Type, p.Page, p.Result, p.AutoRoll);
        }

        public void ShowNotice(S.UpdateNotice p)
        {
            NoticeDialog.Update(p.Notice);
        }

        #region Disposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Scene = null;
                User = null;

                MoveTime = 0;
                AttackTime = 0;
                NextRunTime = 0;
                LastRunTime = 0;
                CanMove = false;
                CanRun = false;

                MapControl = null;
                MainDialog = null;
                ChatDialog = null;
                ChatControl = null;
                InventoryDialog = null;
                CharacterDialog = null;
                StorageDialog = null;
                BeltDialog = null;
                MiniMapDialog = null;
                InspectDialog = null;
                OptionDialog = null;
                MenuDialog = null;
                NPCDialog = null;
                QuestDetailDialog = null;
                QuestListDialog = null;
                QuestLogDialog = null;
                QuestTrackingDialog = null;
                GameShopDialog = null;
                MentorDialog = null;

                NewHeroDialog = null;
                HeroInventoryDialog = null;
                HeroDialog = null;
                HeroBeltDialog = null;

                RelationshipDialog = null;
                CharacterDuraPanel = null;
                DuraStatusPanel = null;

                HoverItem = null;
                SelectedCell = null;
                PickedUpGold = false;

                UseItemTime = 0;
                PickUpTime = 0;
                InspectTime = 0;

                DisposeItemLabel();

                AMode = 0;
                PMode = 0;
                Lights = 0;

                NPCTime = 0;
                NPCID = 0;
                DefaultNPCID = 0;

                for (int i = 0; i < OutputLines.Length; i++)
                    if (OutputLines[i] != null && OutputLines[i].IsDisposed)
                        OutputLines[i].Dispose();

                OutputMessages.Clear();
                OutputMessages = null;
            }

            base.Dispose(disposing);
        }

        #endregion

    }

    public sealed class MapControl : MirControl
    {
        public static UserObject User
        {
            get { return MapObject.User; }
            set { MapObject.User = value; }
        }

        public static UserHeroObject Hero
        {
            get { return MapObject.Hero; }
            set { MapObject.Hero = value; }
        }

        public static Dictionary<uint, MapObject> Objects = new Dictionary<uint, MapObject>();
        public static List<MapObject> ObjectsList = new List<MapObject>();

        public const int CellWidth = 48;
        public const int CellHeight = 32;

        public static int OffSetX;
        public static int OffSetY;

        public static int ViewRangeX;
        public static int ViewRangeY;

        private bool _autoPath;
        public bool AutoPath
        {
            get
            {
                return _autoPath;
            }
            set
            {
                if (_autoPath == value) return;
                _autoPath = value;

                if (!_autoPath)
                    CurrentPath = null;
            }
        }

        public PathFinder PathFinder;
        public List<Node> CurrentPath = null;

        public static Point MapLocation
        {
            get { return GameScene.User == null ? Point.Empty : new Point(MouseLocation.X / CellWidth - OffSetX, MouseLocation.Y / CellHeight - OffSetY).Add(GameScene.User.CurrentLocation); }
        }

        public static Point ToMouseLocation(Point p)
        {
            return new Point((p.X - MapObject.User.Movement.X + OffSetX) * CellWidth, (p.Y - MapObject.User.Movement.Y + OffSetY) * CellHeight).Add(MapObject.User.OffSetMove);
        }

        public static MouseButtons MapButtons;
        public static Point MouseLocation;
        public static long InputDelay;

        private static long nextAction;
        public static long NextAction
        {
            get { return nextAction; }
            set
            {
                if (GameScene.Observing) return;
                nextAction = value;
            }
        }

        public CellInfo[,] M2CellInfo;
        public List<Door> Doors = new List<Door>();
        public int Width, Height;

        public int Index;
        public string FileName = String.Empty;
        public string Title = String.Empty;
        public ushort MiniMap, BigMap, Music, SetMusic;
        public LightSetting Lights;
        public bool Lightning, Fire;
        public byte MapDarkLight;
        public long LightningTime, FireTime;
        public WeatherSetting Weather = WeatherSetting.None;
        public bool FloorValid, LightsValid;

        public long OutputDelay;

        private static bool _awakeningAction;
        public static bool AwakeningAction
        {
            get { return _awakeningAction; }
            set
            {
                if (_awakeningAction == value) return;
                _awakeningAction = value;
            }
        }

        private static bool _autoRun;
        public static bool AutoRun
        {
            get { return _autoRun; }
            set
            {
                if (_autoRun == value) return;
                _autoRun = value;
                if (GameScene.Scene != null)
                    GameScene.Scene.ChatDialog.ReceiveChat(value ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AutoRunOn) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AutoRunOff),
                        ChatType.Hint);
            }

        }
        public static bool AutoHit;

        public int AnimationCount;

        public static List<Effect> Effects = new List<Effect>();

        public MapControl()
        {
            MapButtons = MouseButtons.None;

            OffSetX = Settings.ScreenWidth / 2 / CellWidth;
            OffSetY = Settings.ScreenHeight / 2 / CellHeight - 1;

            ViewRangeX = OffSetX + 6;
            ViewRangeY = OffSetY + 6;

            Size = new Size(Settings.ScreenWidth, Settings.ScreenHeight);
            DrawControlTexture = true;
            BackColour = Color.Black;

            MouseDown += OnMouseDown;
            MouseMove += (o, e) => MouseLocation = e.Location;
            Click += OnMouseClick;
        }

        public void ResetMap()
        {
            GameScene.Scene.NPCDialog.Hide();

            MapObject.MouseObjectID = 0;
            MapObject.TargetObjectID = 0;
            MapObject.MagicObjectID = 0;

            if (M2CellInfo != null)
                for (var i = ObjectsList.Count - 1; i >= 0; i--)
                    ObjectsList[i]?.Remove();

            Objects.Clear();
            ObjectsList.Clear();
            Effects.Clear();
            Doors.Clear();

            if (User != null)
            {
                Objects[User.ObjectID] = User;
                ObjectsList.Add(User);
            }
        }

        public void LoadMap()
        {
            ResetMap();

            MapObject.MouseObjectID = 0;
            MapObject.TargetObjectID = 0;
            MapObject.MagicObjectID = 0;

            MapReader Map = new MapReader(FileName);
            M2CellInfo = Map.MapCells;
            Width = Map.Width;
            Height = Map.Height;

            PathFinder = new PathFinder(this);

            try
            {
                if (SetMusic != Music)
                {
                    SoundManager.Music?.Dispose();
                    SoundManager.PlayMusic(Music, true);
                }
            }
            catch (Exception)
            {
                // Do nothing. index was not valid.
            }

            SetMusic = Music;
            SoundList.Music = Music;

            UpdateWeather();
        }


        public void Process()
        {
            Processdoors();
            User.Process();
            for (int i = ObjectsList.Count - 1; i >= 0; i--)
            {
                if (ObjectsList[i] == User) continue;
                ObjectsList[i].Process();
            }

            for (int i = Effects.Count - 1; i >= 0; i--)
                Effects[i].Process();

            if (MapObject.TargetObject != null && MapObject.TargetObject is MonsterObject && MapObject.TargetObject.AI == 64)
                MapObject.TargetObjectID = 0;
            if (MapObject.MagicObject != null && MapObject.MagicObject is MonsterObject && MapObject.MagicObject.AI == 64)
                MapObject.MagicObjectID = 0;

            CheckInput();

            MapObject bestmouseobject = null;
            for (int y = MapLocation.Y + 2; y >= MapLocation.Y - 2; y--)
            {
                if (y >= Height) continue;
                if (y < 0) break;
                for (int x = MapLocation.X + 2; x >= MapLocation.X - 2; x--)
                {
                    if (x >= Width) continue;
                    if (x < 0) break;
                    CellInfo cell = M2CellInfo[x, y];
                    if (cell.CellObjects == null) continue;

                    for (int i = cell.CellObjects.Count - 1; i >= 0; i--)
                    {
                        MapObject ob = cell.CellObjects[i];
                        if (ob == MapObject.User || !ob.MouseOver(CMain.MPoint)) continue;

                        if (MapObject.MouseObject != ob)
                        {
                            if (ob.Dead)
                            {
                                if (!Settings.TargetDead && GameScene.TargetDeadTime <= CMain.Time) continue;

                                bestmouseobject = ob;
                                //continue;
                            }
                            MapObject.MouseObjectID = ob.ObjectID;
                            Redraw();
                        }
                        if (bestmouseobject != null && MapObject.MouseObject == null)
                        {
                            MapObject.MouseObjectID = bestmouseobject.ObjectID;
                            Redraw();
                        }
                        return;
                    }
                }
            }

            if (MapObject.MouseObject != null)
            {
                MapObject.MouseObjectID = 0;
                Redraw();
            }
        }

        public static MapObject GetObject(uint targetID)
        {
            Objects.TryGetValue(targetID, out var ob);
            return ob;
        }

        public override void Draw()
        {
            //Do nothing.
        }

        protected override void CreateTexture()
        {
            if (User == null) return;

            if (!FloorValid)
                DrawFloor();


            if (Size != TextureSize)
                DisposeTexture();

            if (ControlTexture == null || ControlTexture.Disposed)
            {
                DXManager.ControlList.Add(this);
                ControlTexture = new Texture(DXManager.Device, Size.Width, Size.Height, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
                TextureSize = Size;
            }

            Surface oldSurface = DXManager.CurrentSurface;
            Surface surface = ControlTexture.GetSurfaceLevel(0);
            DXManager.SetSurface(surface);
            DXManager.Device.Clear(ClearFlags.Target, BackColour, 0, 0);

            DrawBackground();

            if (FloorValid)
            {
                DXManager.Draw(DXManager.FloorTexture, new Rectangle(0, 0, Settings.ScreenWidth, Settings.ScreenHeight), Vector3.Zero, Color.White);
            }

            DrawObjects();

            //render weather
            foreach (ParticleEngine engine in GameScene.Scene.ParticleEngines)
            {
                engine.Draw();
            }

            //Render Death, 

            LightSetting setting = Lights == LightSetting.Normal ? GameScene.Scene.Lights : Lights;

            if (setting != LightSetting.Day || GameScene.User.Poison.HasFlag(PoisonType.Blindness))
            {
                DrawLights(setting);
            }

            if (Settings.DropView || GameScene.DropViewTime > CMain.Time)
            {
                foreach (var ob in Objects.Values.OfType<ItemObject>())
                {
                    if (!ob.MouseOver(MouseLocation))
                        ob.DrawName();
                }
            }

            if (MapObject.MouseObject != null && !(MapObject.MouseObject is ItemObject))
                MapObject.MouseObject.DrawName();

            int offSet = 0;

            if (Settings.DisplayBodyName)
            {
                foreach (var ob in Objects.Values.OfType<MonsterObject>())
                {
                    if (ob.MouseOver(MouseLocation))
                        ob.DrawName();
                }
            }

            foreach (var ob in Objects.Values.OfType<ItemObject>())
            {
                if (ob.MouseOver(MouseLocation))
                {
                    ob.DrawName(offSet);
                    offSet -= ob.NameLabel.Size.Height + (ob.NameLabel.Border ? 1 : 0);
                }
            }

            if (MapObject.User.MouseOver(MouseLocation))
                MapObject.User.DrawName();

            DXManager.SetSurface(oldSurface);
            surface.Dispose();
            TextureValid = true;

        }
        protected internal override void DrawControl()
        {
            if (!DrawControlTexture)
                return;

            if (!TextureValid)
                CreateTexture();

            if (ControlTexture == null || ControlTexture.Disposed)
                return;

            float oldOpacity = DXManager.Opacity;

            if (MapObject.User.Dead) DXManager.SetGrayscale(true);

            DXManager.DrawOpaque(ControlTexture, new Rectangle(0, 0, Settings.ScreenWidth, Settings.ScreenHeight), Vector3.Zero, Color.White, Opacity);

            if (MapObject.User.Dead) DXManager.SetGrayscale(false);

            CleanTime = CMain.Time + Settings.CleanDelay;
        }

        private void DrawFloor()
        {
            if (DXManager.FloorTexture == null || DXManager.FloorTexture.Disposed)
            {
                DXManager.FloorTexture = new Texture(DXManager.Device, Settings.ScreenWidth, Settings.ScreenHeight, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
                DXManager.FloorSurface = DXManager.FloorTexture.GetSurfaceLevel(0);
            }

            Surface oldSurface = DXManager.CurrentSurface;
            DXManager.SetSurface(DXManager.FloorSurface);
            DXManager.Device.Clear(ClearFlags.Target, Color.Empty, 0, 0);


            int startX = User.Movement.X - ViewRangeX;
            int endX = User.Movement.X + ViewRangeX;
            int startY = User.Movement.Y - ViewRangeY;
            int endY = User.Movement.Y + ViewRangeY;
            int endYExtended = endY + 5;


            int[] drawXCache = new int[endX - startX + 1];
            for (int xi = startX; xi <= endX; xi++)
                drawXCache[xi - startX] = (xi - User.Movement.X + OffSetX) * CellWidth - OffSetX + User.OffSetMove.X;

            int[] drawYCache = new int[endYExtended - startY + 1];
            for (int yi = startY; yi <= endYExtended; yi++)
                drawYCache[yi - startY] = (yi - User.Movement.Y + OffSetY) * CellHeight + User.OffSetMove.Y;


            for (int y = startY; y <= endYExtended; y++)
            {
                if (y <= 0) continue;
                if (y >= Height) break;

                int drawY = drawYCache[y - startY];

                for (int x = startX; x <= endX; x++)
                {
                    if (x < 0) continue;
                    if (x >= Width) break;

                    int drawX = drawXCache[x - startX];
                    var cell = M2CellInfo[x, y];

                    // Back
                    if (y % 2 == 0 && x % 2 == 0 && y <= endY)
                    {
                        if (cell.BackImage != 0 && cell.BackIndex != -1)
                        {
                            int index = (cell.BackImage & 0x1FFFFFFF) - 1;
                            var lib = Libraries.MapLibs[cell.BackIndex];
                            lib.Draw(index, drawX, drawY);
                        }
                    }

                    // Middle
                    int midIndex = cell.MiddleImage - 1;
                    if (midIndex >= 0 && cell.MiddleIndex != -1)
                    {
                        var lib = Libraries.MapLibs[cell.MiddleIndex];
                        Size s = lib.GetSize(midIndex);
                        if ((s.Width == CellWidth && s.Height == CellHeight) ||
                            (s.Width == CellWidth * 2 && s.Height == CellHeight * 2))
                        {
                            lib.Draw(midIndex, drawX, drawY);
                        }
                    }

                    // Front
                    int frontIndex = (cell.FrontImage & 0x7FFF) - 1;
                    if (frontIndex != -1)
                    {
                        int fileIndex = cell.FrontIndex;
                        if (fileIndex != -1 && fileIndex != 200)
                        {
                            var lib = Libraries.MapLibs[fileIndex];
                            Size s = lib.GetSize(frontIndex);

                            // door
                            if (cell.DoorIndex > 0)
                            {
                                Door doorInfo = GetDoor(cell.DoorIndex);
                                if (doorInfo == null)
                                {
                                    doorInfo = new Door() { index = cell.DoorIndex, DoorState = 0, ImageIndex = 0, LastTick = CMain.Time };
                                    Doors.Add(doorInfo);
                                }
                                else if (doorInfo.DoorState != 0)
                                {
                                    frontIndex += (doorInfo.ImageIndex + 1) * cell.DoorOffset;
                                }
                            }

                            if (frontIndex >= 0 &&
                                ((s.Width == CellWidth && s.Height == CellHeight) ||
                                 (s.Width == CellWidth * 2 && s.Height == CellHeight * 2)))
                            {
                                lib.Draw(frontIndex, drawX, drawY);
                            }
                        }
                    }
                }
            }

            DXManager.SetSurface(oldSurface);
            FloorValid = true;
        }
        private void DrawBackground()
        {
            string cleanFilename = FileName.Replace(Settings.MapPath, "");

            if (cleanFilename.StartsWith("ID1") || cleanFilename.StartsWith("ID2"))
            {
                Libraries.Background.Draw(10, 0, 0); //mountains
            }
            else if (cleanFilename.StartsWith("ID3_013"))
            {
                Libraries.Background.Draw(22, 0, 0); //desert
            }
            else if (cleanFilename.StartsWith("ID3_015"))
            {
                Libraries.Background.Draw(23, 0, 0); //greatwall
            }
            else if (cleanFilename.StartsWith("ID3_023") || cleanFilename.StartsWith("ID3_025"))
            {
                Libraries.Background.Draw(21, 0, 0); //village entrance
            }
        }

        private void DrawObjects()
        {
            if (Settings.Effect)
            {
                for (int i = Effects.Count - 1; i >= 0; i--)
                {
                    if (!Effects[i].DrawBehind) continue;
                    Effects[i].Draw();
                }
            }

            for (int y = User.Movement.Y - ViewRangeY; y <= User.Movement.Y + ViewRangeY + 25; y++)
            {
                if (y <= 0) continue;
                if (y >= Height) break;
                for (int x = User.Movement.X - ViewRangeX; x <= User.Movement.X + ViewRangeX; x++)
                {
                    if (x < 0) continue;
                    if (x >= Width) break;
                    M2CellInfo[x, y].DrawDeadObjects();
                }
            }

            for (int y = User.Movement.Y - ViewRangeY; y <= User.Movement.Y + ViewRangeY + 25; y++)
            {
                if (y <= 0) continue;
                if (y >= Height) break;
                int drawY = (y - User.Movement.Y + OffSetY + 1) * CellHeight + User.OffSetMove.Y;

                for (int x = User.Movement.X - ViewRangeX; x <= User.Movement.X + ViewRangeX; x++)
                {
                    if (x < 0) continue;
                    if (x >= Width) break;
                    int drawX = (x - User.Movement.X + OffSetX) * CellWidth - OffSetX + User.OffSetMove.X;
                    int index;
                    byte animation;
                    bool blend;
                    Size s;
                    #region Draw shanda's tile animation layer
                    index = M2CellInfo[x, y].TileAnimationImage;
                    animation = M2CellInfo[x, y].TileAnimationFrames;
                    if ((index > 0) & (animation > 0))
                    {
                        index--;
                        int animationoffset = M2CellInfo[x, y].TileAnimationOffset ^ 0x2000;
                        index += animationoffset * (AnimationCount % animation);
                        Libraries.MapLibs[190].DrawUp(index, drawX, drawY);
                    }

                    #endregion

                    #region Draw mir3 middle layer
                    if ((M2CellInfo[x, y].MiddleIndex >= 0) && (M2CellInfo[x, y].MiddleIndex != -1))   //M2P '> 199' changed to '>= 0' to include mir2 libraries. Fixes middle layer tile strips draw. Also changed in 'DrawFloor' above.
                    {
                        index = M2CellInfo[x, y].MiddleImage - 1;
                        if (index > 0)
                        {
                            animation = M2CellInfo[x, y].MiddleAnimationFrame;
                            blend = false;
                            if ((animation > 0) && (animation < 255))
                            {
                                if ((animation & 0x0f) > 0)
                                {
                                    blend = true;
                                    animation &= 0x0f;
                                }
                                if (animation > 0)
                                {
                                    byte animationTick = M2CellInfo[x, y].MiddleAnimationTick;
                                    index += (AnimationCount % (animation + (animation * animationTick))) / (1 + animationTick);

                                    if (blend && (animation == 10 || animation == 8)) //diamond mines, abyss blends
                                    {
                                        Libraries.MapLibs[M2CellInfo[x, y].MiddleIndex].DrawUpBlend(index, new Point(drawX, drawY));
                                    }
                                    else
                                    {
                                        Libraries.MapLibs[M2CellInfo[x, y].MiddleIndex].DrawUp(index, drawX, drawY);
                                    }
                                }
                            }
                            s = Libraries.MapLibs[M2CellInfo[x, y].MiddleIndex].GetSize(index);
                            if ((s.Width != CellWidth || s.Height != CellHeight) && (s.Width != (CellWidth * 2) || s.Height != (CellHeight * 2)) && !blend)
                            {
                                Libraries.MapLibs[M2CellInfo[x, y].MiddleIndex].DrawUp(index, drawX, drawY);
                            }
                        }
                    }
                    #endregion

                    #region Draw front layer
                    index = (M2CellInfo[x, y].FrontImage & 0x7FFF) - 1;

                    if (index < 0) continue;

                    int fileIndex = M2CellInfo[x, y].FrontIndex;
                    if (fileIndex == -1) continue;
                    animation = M2CellInfo[x, y].FrontAnimationFrame;

                    if ((animation & 0x80) > 0)
                    {
                        blend = true;
                        animation &= 0x7F;
                    }
                    else
                        blend = false;


                    if (animation > 0)
                    {
                        byte animationTick = M2CellInfo[x, y].FrontAnimationTick;
                        index += (AnimationCount % (animation + (animation * animationTick))) / (1 + animationTick);
                    }


                    if (M2CellInfo[x, y].DoorIndex > 0)
                    {
                        Door DoorInfo = GetDoor(M2CellInfo[x, y].DoorIndex);
                        if (DoorInfo == null)
                        {
                            DoorInfo = new Door() { index = M2CellInfo[x, y].DoorIndex, DoorState = 0, ImageIndex = 0, LastTick = CMain.Time };
                            Doors.Add(DoorInfo);
                        }
                        else
                        {
                            if (DoorInfo.DoorState != 0)
                            {
                                index += (DoorInfo.ImageIndex + 1) * M2CellInfo[x, y].DoorOffset;//'bad' code if you want to use animation but it's gonna depend on the animation > has to be custom designed for the animtion
                            }
                        }
                    }

                    s = Libraries.MapLibs[fileIndex].GetSize(index);
                    if (s.Width == CellWidth && s.Height == CellHeight && animation == 0) continue;
                    if ((s.Width == CellWidth * 2) && (s.Height == CellHeight * 2) && (animation == 0)) continue;

                    if (blend)
                    {
                        if (fileIndex == 14 || fileIndex == 27 || (fileIndex > 99 & fileIndex < 199))
                            Libraries.MapLibs[fileIndex].DrawBlend(index, new Point(drawX, drawY - (3 * CellHeight)), Color.White, true);
                        else
                            Libraries.MapLibs[fileIndex].DrawBlend(index, new Point(drawX, drawY - s.Height), Color.White, (index >= 2723 && index <= 2732));
                    }
                    else
                    {
                        if (fileIndex == 28 && Libraries.MapLibs[fileIndex].GetOffSet(index) != Point.Empty)
                            Libraries.MapLibs[fileIndex].Draw(index, new Point(drawX, drawY - CellHeight), Color.White, true);
                        else
                            Libraries.MapLibs[fileIndex].Draw(index, drawX, drawY - s.Height);
                    }
                    #endregion
                }

                for (int x = User.Movement.X - ViewRangeX; x <= User.Movement.X + ViewRangeX; x++)
                {
                    if (x < 0) continue;
                    if (x >= Width) break;
                    M2CellInfo[x, y].DrawObjects();
                }
            }

            DXManager.Sprite.Flush();
            float oldOpacity = DXManager.Opacity;
            DXManager.SetOpacity(0.4F);

            //MapObject.User.DrawMount();

            MapObject.User.DrawBody();

            if ((MapObject.User.Direction == MirDirection.Up) ||
                (MapObject.User.Direction == MirDirection.UpLeft) ||
                (MapObject.User.Direction == MirDirection.UpRight) ||
                (MapObject.User.Direction == MirDirection.Right) ||
                (MapObject.User.Direction == MirDirection.Left))
            {
                MapObject.User.DrawHead();
                MapObject.User.DrawWings();
            }
            else
            {
                MapObject.User.DrawWings();
                MapObject.User.DrawHead();
            }

            DXManager.SetOpacity(oldOpacity);

            if (Settings.HighlightTarget)
            {
                if (MapObject.MouseObject != null && !MapObject.MouseObject.Dead && MapObject.MouseObject != MapObject.TargetObject && MapObject.MouseObject.Blend)
                    MapObject.MouseObject.DrawBlend();

                if (MapObject.TargetObject != null)
                    MapObject.TargetObject.DrawBlend();
            }

            if (Settings.Effect)
            {
                for (int i = Effects.Count - 1; i >= 0; i--)
                {
                    if (Effects[i].DrawBehind) continue;
                    Effects[i].Draw();
                }
            }

            foreach (var ob in Objects.Values)
            {
                ob.DrawEffects(Settings.Effect);

                if (Settings.NameView && !(ob is ItemObject) && !ob.Dead)
                    ob.DrawName();

                ob.DrawChat();
                //ob.DrawHealth();
                ob.DrawPoison();
                ob.DrawDamages();
            }

            foreach (var ob in Objects.Values)
            {
                ob.DrawHealth();
            }
        }

        private Color GetBlindLight(Color light)
        {
            if (MapObject.User.BlindTime <= CMain.Time && MapObject.User.BlindCount < 25)
            {
                MapObject.User.BlindTime = CMain.Time + 100;
                MapObject.User.BlindCount++;
            }

            int count = MapObject.User.BlindCount;
            light = Color.FromArgb(255, Math.Max(20, light.R - (count * 10)), Math.Max(20, light.G - (count * 10)), Math.Max(20, light.B - (count * 10)));

            return light;
        }

        private void DrawLights(LightSetting setting)
        {
            if (DXManager.Lights == null || DXManager.Lights.Count == 0) return;

            if (DXManager.LightTexture == null || DXManager.LightTexture.Disposed)
            {
                DXManager.LightTexture = new Texture(DXManager.Device, Settings.ScreenWidth, Settings.ScreenHeight, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
                DXManager.LightSurface = DXManager.LightTexture.GetSurfaceLevel(0);
            }

            Surface oldSurface = DXManager.CurrentSurface;
            DXManager.SetSurface(DXManager.LightSurface);

            #region Night Lights
            Color darkness;

            switch (setting)
            {
                case LightSetting.Night:
                    {
                        switch (MapDarkLight)
                        {
                            case 1:
                                darkness = Color.FromArgb(255, 20, 20, 20);
                                break;
                            case 2:
                                darkness = Color.LightSlateGray;
                                break;
                            case 3:
                                darkness = Color.SkyBlue;
                                break;
                            case 4:
                                darkness = Color.Goldenrod;
                                break;
                            default:
                                darkness = Color.Black;
                                break;
                        }
                    }
                    break;
                case LightSetting.Evening:
                case LightSetting.Dawn:
                    darkness = Color.FromArgb(255, 50, 50, 50);
                    break;
                default:
                case LightSetting.Day:
                    darkness = Color.FromArgb(255, 255, 255, 255);
                    break;
            }

            if (MapObject.User.Poison.HasFlag(PoisonType.Blindness))
            {
                darkness = GetBlindLight(darkness);
            }

            DXManager.Device.Clear(ClearFlags.Target, darkness, 0, 0);

            #endregion

            int light;
            Point p;
            DXManager.SetBlend(true);
            DXManager.Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            DXManager.Device.SetRenderState(RenderState.DestinationBlend, Blend.One);

            #region Object Lights (Player/Mob/NPC)
            foreach (var ob in Objects.Values)
            {
                if (ob.Light > 0 && (!ob.Dead || ob == MapObject.User || ob.Race == ObjectType.Spell))
                {
                    light = ob.Light;

                    int lightRange = light % 15;
                    if (lightRange >= DXManager.Lights.Count)
                        lightRange = DXManager.Lights.Count - 1;

                    p = ob.DrawLocation;

                    Color lightColour = ob.LightColour;

                    if (ob.Race == ObjectType.Player)
                    {
                        switch (light / 15)
                        {
                            case 0://no light source
                                lightColour = Color.FromArgb(255, 60, 60, 60);
                                break;
                            case 1:
                                lightColour = Color.FromArgb(255, 120, 120, 120);
                                break;
                            case 2://Candle
                                lightColour = Color.FromArgb(255, 180, 180, 180);
                                break;
                            case 3://Torch
                                lightColour = Color.FromArgb(255, 240, 240, 240);
                                break;
                            default://Peddler Torch
                                lightColour = Color.FromArgb(255, 255, 255, 255);
                                break;
                        }
                    }
                    else if (ob.Race == ObjectType.Merchant)
                    {
                        lightColour = Color.FromArgb(255, 120, 120, 120);
                    }

                    if (MapObject.User.Poison.HasFlag(PoisonType.Blindness))
                    {
                        lightColour = GetBlindLight(lightColour);
                    }

                    if (DXManager.Lights[lightRange] != null && !DXManager.Lights[lightRange].Disposed)
                    {
                        p.Offset(-(DXManager.LightSizes[lightRange].X / 2) - (CellWidth / 2), -(DXManager.LightSizes[lightRange].Y / 2) - (CellHeight / 2) - 5);
                        DXManager.Draw(DXManager.Lights[lightRange], null, new Vector3((float)p.X, (float)p.Y, 0.0F), lightColour);
                    }
                }

                #region Object Effect Lights
                if (!Settings.Effect) continue;
                for (int e = 0; e < ob.Effects.Count; e++)
                {
                    Effect effect = ob.Effects[e];
                    if (!effect.Blend || CMain.Time < effect.Start || (!(effect is Missile) && effect.Light < ob.Light)) continue;

                    light = effect.Light;

                    p = effect.DrawLocation;

                    var lightColour = effect.LightColour;

                    if (MapObject.User.Poison.HasFlag(PoisonType.Blindness))
                    {
                        lightColour = GetBlindLight(lightColour);
                    }

                    if (DXManager.Lights[light] != null && !DXManager.Lights[light].Disposed)
                    {
                        p.Offset(-(DXManager.LightSizes[light].X / 2) - (CellWidth / 2), -(DXManager.LightSizes[light].Y / 2) - (CellHeight / 2) - 5);
                        DXManager.Draw(DXManager.Lights[light], null, new Vector3((float)p.X, (float)p.Y, 0.0F), lightColour);
                    }

                }
                #endregion
            }
            #endregion

            #region Map Effect Lights
            if (Settings.Effect)
            {
                for (int e = 0; e < Effects.Count; e++)
                {
                    Effect effect = Effects[e];
                    if (!effect.Blend || CMain.Time < effect.Start) continue;

                    light = effect.Light;
                    if (light == 0) continue;

                    p = effect.DrawLocation;

                    var lightColour = Color.White;

                    if (MapObject.User.Poison.HasFlag(PoisonType.Blindness))
                    {
                        lightColour = GetBlindLight(lightColour);
                    }

                    if (DXManager.Lights[light] != null && !DXManager.Lights[light].Disposed)
                    {
                        p.Offset(-(DXManager.LightSizes[light].X / 2) - (CellWidth / 2), -(DXManager.LightSizes[light].Y / 2) - (CellHeight / 2) - 5);
                        DXManager.Draw(DXManager.Lights[light], null, new Vector3((float)p.X, (float)p.Y, 0.0F), lightColour);
                    }
                }
            }
            #endregion

            #region Map Lights
            for (int y = MapObject.User.Movement.Y - ViewRangeY - 24; y <= MapObject.User.Movement.Y + ViewRangeY + 24; y++)
            {
                if (y < 0) continue;
                if (y >= Height) break;
                for (int x = MapObject.User.Movement.X - ViewRangeX - 24; x < MapObject.User.Movement.X + ViewRangeX + 24; x++)
                {
                    if (x < 0) continue;
                    if (x >= Width) break;
                    int imageIndex = (M2CellInfo[x, y].FrontImage & 0x7FFF) - 1;
                    if (imageIndex == -1) continue;
                    int fileIndex = M2CellInfo[x, y].FrontIndex;
                    if (fileIndex == -1) continue;
                    if (M2CellInfo[x, y].Light <= 0 || M2CellInfo[x, y].Light >= 10) continue;
                    if (M2CellInfo[x, y].Light == 0) continue;

                    Color lightIntensity;

                    light = (M2CellInfo[x, y].Light % 10) * 3;

                    switch (M2CellInfo[x, y].Light / 10)
                    {
                        case 1:
                            lightIntensity = Color.FromArgb(255, 255, 255, 255);
                            break;
                        case 2:
                            lightIntensity = Color.FromArgb(255, 120, 180, 255);
                            break;
                        case 3:
                            lightIntensity = Color.FromArgb(255, 255, 180, 120);
                            break;
                        case 4:
                            lightIntensity = Color.FromArgb(255, 22, 160, 5);
                            break;
                        default:
                            lightIntensity = Color.FromArgb(255, 255, 255, 255);
                            break;
                    }

                    if (MapObject.User.Poison.HasFlag(PoisonType.Blindness))
                    {
                        lightIntensity = GetBlindLight(lightIntensity);
                    }

                    p = new Point(
                        (x + OffSetX - MapObject.User.Movement.X) * CellWidth + MapObject.User.OffSetMove.X,
                        (y + OffSetY - MapObject.User.Movement.Y) * CellHeight + MapObject.User.OffSetMove.Y + 32);


                    if (M2CellInfo[x, y].FrontAnimationFrame > 0)
                        p.Offset(Libraries.MapLibs[fileIndex].GetOffSet(imageIndex));

                    if (light >= DXManager.Lights.Count)
                        light = DXManager.Lights.Count - 1;

                    if (DXManager.Lights[light] != null && !DXManager.Lights[light].Disposed)
                    {
                        p.Offset(-(DXManager.LightSizes[light].X / 2) - (CellWidth / 2) + 10, -(DXManager.LightSizes[light].Y / 2) - (CellHeight / 2) - 5);
                        DXManager.Draw(DXManager.Lights[light], null, new Vector3((float)p.X, (float)p.Y, 0.0F), lightIntensity);
                    }
                }
            }
            #endregion

            DXManager.SetBlend(false);
            DXManager.SetSurface(oldSurface);

            DXManager.Device.SetRenderState(RenderState.SourceBlend, Blend.Zero);
            DXManager.Device.SetRenderState(RenderState.DestinationBlend, Blend.SourceColor);

            DXManager.Draw(DXManager.LightTexture, new Rectangle(0, 0, Settings.ScreenWidth, Settings.ScreenHeight), Vector3.Zero, Color.White);

            DXManager.Sprite.End();
            DXManager.Sprite.Begin(SpriteFlags.AlphaBlend);
        }

        private static void OnMouseClick(object sender, EventArgs e)
        {
            if (!(e is MouseEventArgs me)) return;

            if (AwakeningAction == true) return;
            switch (me.Button)
            {
                case MouseButtons.Left:
                    {
                        AutoRun = false;
                        GameScene.Scene.MapControl.AutoPath = false;
                        if (MapObject.MouseObject == null) return;
                        NPCObject npc = MapObject.MouseObject as NPCObject;
                        if (npc != null)
                        {
                            if (npc.ObjectID == GameScene.NPCID &&
                                (CMain.Time <= GameScene.NPCTime || GameScene.Scene.NPCDialog.Visible))
                            {
                                return;
                            }

                            //GameScene.Scene.NPCDialog.Hide();

                            GameScene.NPCTime = CMain.Time + 5000;
                            GameScene.NPCID = npc.ObjectID;
                            Network.Enqueue(new C.CallNPC { ObjectID = npc.ObjectID, Key = "[@Main]" });
                        }
                    }
                    break;
                case MouseButtons.Right:
                    {
                        AutoRun = false;
                        if (MapObject.MouseObject == null)
                        {
                            if (Settings.NewMove && MapLocation != MapObject.User.CurrentLocation && GameScene.Scene.MapControl.EmptyCell(MapLocation))
                            {
                                var path = GameScene.Scene.MapControl.PathFinder.FindPath(MapObject.User.CurrentLocation, MapLocation, 20);

                                if (path != null && path.Count > 0)
                                {
                                    GameScene.Scene.MapControl.CurrentPath = path;
                                    GameScene.Scene.MapControl.AutoPath = true;
                                    var offset = MouseLocation.Subtract(ToMouseLocation(MapLocation));
                                    Effects.Add(new Effect(Libraries.Magic3, 500, 10, 600, MapLocation) { DrawOffset = offset.Subtract(8, 15) });
                                }
                            }
                            return;
                        }

                        if (CMain.Ctrl)
                        {
                            HeroObject hero = MapObject.MouseObject as HeroObject;

                            if (hero != null &&
                                hero.ObjectID != (Hero is null ? 0 : Hero.ObjectID) &&
                                CMain.Time >= GameScene.InspectTime)
                            {
                                GameScene.InspectTime = CMain.Time + 500;
                                InspectDialog.InspectID = hero.ObjectID;
                                Network.Enqueue(new C.Inspect { ObjectID = hero.ObjectID, Hero = true });
                                return;
                            }

                            PlayerObject player = MapObject.MouseObject as PlayerObject;

                            if (player != null &&
                                player != User &&
                                CMain.Time >= GameScene.InspectTime)
                            {
                                GameScene.InspectTime = CMain.Time + 500;
                                InspectDialog.InspectID = player.ObjectID;
                                Network.Enqueue(new C.Inspect { ObjectID = player.ObjectID });
                                return;
                            }
                        }
                    }
                    break;
                case MouseButtons.Middle:
                    AutoRun = !AutoRun;
                    break;
            }
        }

        private static void OnMouseDown(object sender, MouseEventArgs e)
        {
            MapButtons |= e.Button;
            if (e.Button != MouseButtons.Right || !Settings.NewMove)
                GameScene.CanRun = false;

            if (AwakeningAction == true) return;

            if (e.Button != MouseButtons.Left) return;

            if (GameScene.SelectedCell != null)
            {
                if (GameScene.SelectedCell.GridType != MirGridType.Inventory && GameScene.SelectedCell.GridType != MirGridType.HeroInventory)
                {
                    GameScene.SelectedCell = null;
                    return;
                }

                MirItemCell cell = GameScene.SelectedCell;
                if (cell.Item.Info.Bind.HasFlag(BindMode.DontDrop))
                {
                    MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.YouCannotDrop), cell.Item.FriendlyName), MirMessageBoxButtons.OK);
                    messageBox.Show();
                    GameScene.SelectedCell = null;
                    return;
                }
                if (cell.Item.Count == 1)
                {
                    MirMessageBox messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.DropTip), cell.Item.FriendlyName), MirMessageBoxButtons.YesNo);

                    messageBox.YesButton.Click += (o, a) =>
                    {
                        Network.Enqueue(new C.DropItem
                        {
                            UniqueID = cell.Item.UniqueID,
                            Count = 1,
                            HeroInventory = cell.GridType == MirGridType.HeroInventory
                        });

                        cell.Locked = true;
                    };
                    messageBox.Show();
                }
                else
                {
                    MirAmountBox amountBox = new MirAmountBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DropAmount), cell.Item.Info.Image, cell.Item.Count);

                    amountBox.OKButton.Click += (o, a) =>
                    {
                        if (amountBox.Amount <= 0) return;
                        Network.Enqueue(new C.DropItem
                        {
                            UniqueID = cell.Item.UniqueID,
                            Count = (ushort)amountBox.Amount,
                            HeroInventory = cell.GridType == MirGridType.HeroInventory
                        });

                        cell.Locked = true;
                    };

                    amountBox.Show();
                }
                GameScene.SelectedCell = null;

                return;
            }

            if (GameScene.PickedUpGold)
            {
                MirAmountBox amountBox = new MirAmountBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.DropAmount), 116, GameScene.Gold);

                amountBox.OKButton.Click += (o, a) =>
                {
                    if (amountBox.Amount > 0)
                    {
                        Network.Enqueue(new C.DropGold { Amount = amountBox.Amount });
                    }
                };

                amountBox.Show();
                GameScene.PickedUpGold = false;
            }

            if (MapObject.MouseObject != null && !MapObject.MouseObject.Dead && !(MapObject.MouseObject is ItemObject) &&
                !(MapObject.MouseObject is NPCObject) && !(MapObject.MouseObject is MonsterObject && MapObject.MouseObject.AI == 64)
                 && !(MapObject.MouseObject is MonsterObject && MapObject.MouseObject.AI == 70))
            {
                MapObject.TargetObjectID = MapObject.MouseObject.ObjectID;
                if (MapObject.MouseObject is MonsterObject && MapObject.MouseObject.AI != 6)
                    MapObject.MagicObjectID = MapObject.TargetObject.ObjectID;
            }
            else
                MapObject.TargetObjectID = 0;
        }

        private void CheckInput()
        {
            if (AwakeningAction == true) return;

            if ((MouseControl == this) && (MapButtons != MouseButtons.None)) AutoHit = false;//mouse actions stop mining even when frozen!
            if (!CanRideAttack()) AutoHit = false;

            if (CMain.Time < InputDelay || User.Poison.HasFlag(PoisonType.Paralysis) || User.Poison.HasFlag(PoisonType.LRParalysis) || User.Poison.HasFlag(PoisonType.Frozen) || User.Fishing) return;

            if (User.NextMagic != null && !User.RidingMount)
            {
                UseMagic(User.NextMagic, User);
                return;
            }

            if (CMain.Time < User.BlizzardStopTime || CMain.Time < User.ReincarnationStopTime) return;

            if (MapObject.TargetObject != null && !MapObject.TargetObject.Dead)
            {
                if (((MapObject.TargetObject.Name.EndsWith(")") || MapObject.TargetObject is PlayerObject) && CMain.Shift) ||
                    (!MapObject.TargetObject.Name.EndsWith(")") && MapObject.TargetObject is MonsterObject))
                {
                    GameScene.LogTime = CMain.Time + Globals.LogDelay;

                    if (User.Class == MirClass.Archer && User.HasClassWeapon && !User.RidingMount && !User.Fishing)//ArcherTest - non aggressive targets (player / pets)
                    {
                        if (Functions.InRange(MapObject.TargetObject.CurrentLocation, User.CurrentLocation, Globals.MaxAttackRange))
                        {
                            if (CMain.Time > GameScene.AttackTime)
                            {
                                User.QueuedAction = new QueuedAction { Action = MirAction.AttackRange1, Direction = Functions.DirectionFromPoint(User.CurrentLocation, MapObject.TargetObject.CurrentLocation), Location = User.CurrentLocation, Params = new List<object>() };
                                User.QueuedAction.Params.Add(MapObject.TargetObject != null ? MapObject.TargetObject.ObjectID : (uint)0);
                                User.QueuedAction.Params.Add(MapObject.TargetObject.CurrentLocation);

                                // MapObject.TargetObject = null; //stop constant attack when close up
                            }
                        }
                        else
                        {
                            if (CMain.Time >= OutputDelay)
                            {
                                OutputDelay = CMain.Time + 1000;
                                GameScene.Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.TargetTooFar));
                            }
                        }
                        //  return;
                    }

                    else if (Functions.InRange(MapObject.TargetObject.CurrentLocation, User.CurrentLocation, 1))
                    {
                        if (CMain.Time > GameScene.AttackTime && CanRideAttack() && !User.Poison.HasFlag(PoisonType.Dazed))
                        {
                            User.QueuedAction = new QueuedAction { Action = MirAction.Attack1, Direction = Functions.DirectionFromPoint(User.CurrentLocation, MapObject.TargetObject.CurrentLocation), Location = User.CurrentLocation };
                            return;
                        }
                    }
                }
            }
            if (AutoHit && !User.RidingMount)
            {
                if (CMain.Time > GameScene.AttackTime)
                {
                    User.QueuedAction = new QueuedAction { Action = MirAction.Mine, Direction = User.Direction, Location = User.CurrentLocation };
                    return;
                }
            }


            MirDirection direction;
            if (MouseControl == this)
            {
                direction = MouseDirection();
                if (AutoRun)
                {
                    if (GameScene.CanRun && CanRun(direction) && CMain.Time > GameScene.NextRunTime && User.HP >= 10 && (!User.Sneaking || (User.Sneaking && User.Sprint))) //slow remove
                    {
                        int distance = User.RidingMount || User.Sprint && !User.Sneaking ? 3 : 2;
                        bool fail = false;
                        for (int i = 1; i <= distance; i++)
                        {
                            if (!CheckDoorOpen(Functions.PointMove(User.CurrentLocation, direction, i)))
                                fail = true;
                        }
                        if (!fail)
                        {
                            User.QueuedAction = new QueuedAction { Action = MirAction.Running, Direction = direction, Location = Functions.PointMove(User.CurrentLocation, direction, distance) };
                            return;
                        }
                    }
                    if ((CanWalk(direction, out direction)) && (CheckDoorOpen(Functions.PointMove(User.CurrentLocation, direction, 1))))
                    {
                        User.QueuedAction = new QueuedAction { Action = MirAction.Walking, Direction = direction, Location = Functions.PointMove(User.CurrentLocation, direction, 1) };
                        return;
                    }
                    if (direction != User.Direction)
                    {
                        User.QueuedAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = User.CurrentLocation };
                        return;
                    }
                    return;
                }

                switch (MapButtons)
                {
                    case MouseButtons.Left:
                        if (MapObject.MouseObject is NPCObject || (MapObject.MouseObject is PlayerObject && MapObject.MouseObject != User)) break;
                        if (MapObject.MouseObject is MonsterObject && MapObject.MouseObject.AI == 70) break;

                        if (CMain.Alt && !User.RidingMount)
                        {
                            User.QueuedAction = new QueuedAction { Action = MirAction.Harvest, Direction = direction, Location = User.CurrentLocation };
                            return;
                        }

                        if (CMain.Shift)
                        {
                            if (CMain.Time > GameScene.AttackTime && CanRideAttack()) //ArcherTest - shift click
                            {
                                MapObject target = null;
                                if (MapObject.MouseObject is MonsterObject || MapObject.MouseObject is PlayerObject) target = MapObject.MouseObject;

                                if (User.Class == MirClass.Archer && User.HasClassWeapon && !User.RidingMount && !User.Poison.HasFlag(PoisonType.Dazed))
                                {
                                    if (target != null)
                                    {
                                        if (!Functions.InRange(MapObject.MouseObject.CurrentLocation, User.CurrentLocation, Globals.MaxAttackRange))
                                        {
                                            if (CMain.Time >= OutputDelay)
                                            {
                                                OutputDelay = CMain.Time + 1000;
                                                GameScene.Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.TargetTooFar));
                                            }
                                            return;
                                        }
                                    }

                                    User.QueuedAction = new QueuedAction { Action = MirAction.AttackRange1, Direction = MouseDirection(), Location = User.CurrentLocation, Params = new List<object>() };
                                    User.QueuedAction.Params.Add(target != null ? target.ObjectID : (uint)0);
                                    User.QueuedAction.Params.Add(Functions.PointMove(User.CurrentLocation, MouseDirection(), 9));
                                    return;
                                }

                                //stops double slash from being used without empty hand or assassin weapon (otherwise bugs on second swing)
                                if (GameScene.User.DoubleSlash && (!User.HasClassWeapon && User.Weapon > -1)) return;
                                if (User.Poison.HasFlag(PoisonType.Dazed)) return;

                                User.QueuedAction = new QueuedAction { Action = MirAction.Attack1, Direction = direction, Location = User.CurrentLocation };
                            }
                            return;
                        }

                        if (MapObject.MouseObject is MonsterObject && User.Class == MirClass.Archer && MapObject.TargetObject != null && !MapObject.TargetObject.Dead && User.HasClassWeapon && !User.RidingMount) //ArcherTest - range attack
                        {
                            if (Functions.InRange(MapObject.MouseObject.CurrentLocation, User.CurrentLocation, Globals.MaxAttackRange))
                            {
                                if (CMain.Time > GameScene.AttackTime)
                                {
                                    User.QueuedAction = new QueuedAction { Action = MirAction.AttackRange1, Direction = direction, Location = User.CurrentLocation, Params = new List<object>() };
                                    User.QueuedAction.Params.Add(MapObject.TargetObject.ObjectID);
                                    User.QueuedAction.Params.Add(MapObject.TargetObject.CurrentLocation);
                                }
                            }
                            else
                            {
                                if (CMain.Time >= OutputDelay)
                                {
                                    OutputDelay = CMain.Time + 1000;
                                    GameScene.Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.TargetTooFar));
                                }
                            }
                            return;
                        }

                        if (MapLocation == User.CurrentLocation)
                        {
                            if (CMain.Time > GameScene.PickUpTime)
                            {
                                GameScene.PickUpTime = CMain.Time + 200;
                                Network.Enqueue(new C.PickUp());
                            }
                            return;
                        }

                        //mine
                        if (!ValidPoint(Functions.PointMove(User.CurrentLocation, direction, 1)))
                        {
                            if ((MapObject.User.Equipment[(int)EquipmentSlot.Weapon] != null) && (MapObject.User.Equipment[(int)EquipmentSlot.Weapon].Info.CanMine))
                            {
                                if (direction != User.Direction)
                                {
                                    User.QueuedAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = User.CurrentLocation };
                                    return;
                                }
                                AutoHit = true;
                                return;
                            }
                        }
                        if ((CanWalk(direction, out direction)) && (CheckDoorOpen(Functions.PointMove(User.CurrentLocation, direction, 1))))
                        {

                            User.QueuedAction = new QueuedAction { Action = MirAction.Walking, Direction = direction, Location = Functions.PointMove(User.CurrentLocation, direction, 1) };
                            return;
                        }
                        if (direction != User.Direction)
                        {
                            User.QueuedAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = User.CurrentLocation };
                            return;
                        }

                        if (CanFish(direction))
                        {
                            User.FishingTime = CMain.Time;
                            Network.Enqueue(new C.FishingCast { CastOut = true });
                            return;
                        }

                        break;
                    case MouseButtons.Right:
                        if (MapObject.MouseObject is PlayerObject && MapObject.MouseObject != User && CMain.Ctrl) break;
                        if (Settings.NewMove) break;

                        if (Functions.InRange(MapLocation, User.CurrentLocation, 2))
                        {
                            if (direction != User.Direction)
                            {
                                User.QueuedAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = User.CurrentLocation };
                            }
                            return;
                        }

                        GameScene.CanRun = User.FastRun ? true : GameScene.CanRun;

                        if (GameScene.CanRun && CanRun(direction) && CMain.Time > GameScene.NextRunTime && User.HP >= 10 && (!User.Sneaking || (User.Sneaking && User.Sprint))) //slow removed
                        {
                            int distance = User.RidingMount || User.Sprint && !User.Sneaking ? 3 : 2;
                            bool fail = false;
                            for (int i = 0; i <= distance; i++)
                            {
                                if (!CheckDoorOpen(Functions.PointMove(User.CurrentLocation, direction, i)))
                                    fail = true;
                            }
                            if (!fail)
                            {
                                User.QueuedAction = new QueuedAction { Action = MirAction.Running, Direction = direction, Location = Functions.PointMove(User.CurrentLocation, direction, User.RidingMount || (User.Sprint && !User.Sneaking) ? 3 : 2) };
                                return;
                            }
                        }
                        if ((CanWalk(direction, out direction)) && (CheckDoorOpen(Functions.PointMove(User.CurrentLocation, direction, 1))))
                        {
                            User.QueuedAction = new QueuedAction { Action = MirAction.Walking, Direction = direction, Location = Functions.PointMove(User.CurrentLocation, direction, 1) };
                            return;
                        }
                        if (direction != User.Direction)
                        {
                            User.QueuedAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = User.CurrentLocation };
                            return;
                        }
                        break;
                }
            }

            if (AutoPath)
            {
                if (CurrentPath == null || CurrentPath.Count == 0)
                {
                    AutoPath = false;
                    return;
                }

                var path = GameScene.Scene.MapControl.PathFinder.FindPath(MapObject.User.CurrentLocation, CurrentPath.Last().Location);

                if (path != null && path.Count > 0)
                    GameScene.Scene.MapControl.CurrentPath = path;
                else
                {
                    AutoPath = false;
                    return;
                }

                Node currentNode = CurrentPath.SingleOrDefault(x => User.CurrentLocation == x.Location);
                if (currentNode != null)
                {
                    while (true)
                    {
                        Node first = CurrentPath.First();
                        CurrentPath.Remove(first);

                        if (first == currentNode)
                            break;
                    }
                }

                if (CurrentPath.Count > 0)
                {
                    MirDirection dir = Functions.DirectionFromPoint(User.CurrentLocation, CurrentPath.First().Location);

                    if (GameScene.CanRun && CanRun(dir) && CMain.Time > GameScene.NextRunTime && User.HP >= 10 && CurrentPath.Count > (User.RidingMount ? 2 : 1))
                    {
                        User.QueuedAction = new QueuedAction { Action = MirAction.Running, Direction = dir, Location = Functions.PointMove(User.CurrentLocation, dir, User.RidingMount ? 3 : 2) };
                        return;
                    }
                    if (CanWalk(dir))
                    {
                        User.QueuedAction = new QueuedAction { Action = MirAction.Walking, Direction = dir, Location = Functions.PointMove(User.CurrentLocation, dir, 1) };

                        return;
                    }
                }
            }

            if (MapObject.TargetObject == null || MapObject.TargetObject.Dead) return;
            if (((!MapObject.TargetObject.Name.EndsWith(")") && !(MapObject.TargetObject is PlayerObject)) || !CMain.Shift) &&
                (MapObject.TargetObject.Name.EndsWith(")") || !(MapObject.TargetObject is MonsterObject))) return;
            if (Functions.InRange(MapObject.TargetObject.CurrentLocation, User.CurrentLocation, 1)) return;
            if (User.Class == MirClass.Archer && User.HasClassWeapon && (MapObject.TargetObject is MonsterObject || MapObject.TargetObject is PlayerObject)) return; //ArcherTest - stop walking
            direction = Functions.DirectionFromPoint(User.CurrentLocation, MapObject.TargetObject.CurrentLocation);

            if (!CanWalk(direction, out direction)) return;

            User.QueuedAction = new QueuedAction { Action = MirAction.Walking, Direction = direction, Location = Functions.PointMove(User.CurrentLocation, direction, 1) };
        }

        public void UseMagic(ClientMagic magic, UserObject actor)
        {
            if (CMain.Time < GameScene.SpellTime || actor.Poison.HasFlag(PoisonType.Stun))
            {
                actor.ClearMagic();
                return;
            }

            if ((CMain.Time <= magic.CastTime + magic.Delay))
            {
                if (CMain.Time >= OutputDelay)
                {
                    OutputDelay = CMain.Time + 1000;
                    GameScene.Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.CannotCastSpellSeconds), magic.Name,
                        ((magic.CastTime + magic.Delay) - CMain.Time - 1) / 1000 + 1));
                }

                actor.ClearMagic();
                return;
            }

            int cost = magic.Level * magic.LevelCost + magic.BaseCost;

            if (magic.Spell == Spell.Teleport || magic.Spell == Spell.Blink || magic.Spell == Spell.StormEscape)
            {
                if (actor.Stats[Stat.TeleportManaPenaltyPercent] > 0)
                {
                    cost += (cost * actor.Stats[Stat.TeleportManaPenaltyPercent]) / 100;
                }
            }

            if (actor.Stats[Stat.ManaPenaltyPercent] > 0)
            {
                cost += (cost * actor.Stats[Stat.ManaPenaltyPercent]) / 100;
            }

            if (cost > actor.MP)
            {
                if (CMain.Time >= OutputDelay)
                {
                    OutputDelay = CMain.Time + 1000;
                    GameScene.Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowMana));
                }
                actor.ClearMagic();
                return;
            }

            //bool isTargetSpell = true;

            MapObject target = null;

            //Targeting
            switch (magic.Spell)
            {
                case Spell.FireBall:
                case Spell.GreatFireBall:
                case Spell.ElectricShock:
                case Spell.Poisoning:
                case Spell.ThunderBolt:
                case Spell.FlameDisruptor:
                case Spell.SoulFireBall:
                case Spell.TurnUndead:
                case Spell.FrostCrunch:
                case Spell.Vampirism:
                case Spell.Revelation:
                case Spell.Entrapment:
                case Spell.Hallucination:
                case Spell.DarkBody:
                case Spell.FireBounce:
                case Spell.MeteorShower:
                    if (actor.NextMagicObject != null)
                    {
                        if (!actor.NextMagicObject.Dead && actor.NextMagicObject.Race != ObjectType.Item && actor.NextMagicObject.Race != ObjectType.Merchant)
                            target = actor.NextMagicObject;
                    }

                    if (target == null) target = MapObject.MagicObject;

                    if (target != null && target.Race == ObjectType.Monster) MapObject.MagicObjectID = target.ObjectID;
                    break;
                case Spell.StraightShot:
                case Spell.DoubleShot:
                case Spell.ElementalShot:
                case Spell.DelayedExplosion:
                case Spell.BindingShot:
                case Spell.VampireShot:
                case Spell.PoisonShot:
                case Spell.CrippleShot:
                case Spell.NapalmShot:
                case Spell.SummonVampire:
                case Spell.SummonToad:
                case Spell.SummonSnakes:
                    if (!actor.HasClassWeapon)
                    {
                        GameScene.Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MustWearBowForSkill));
                        actor.ClearMagic();
                        return;
                    }
                    if (actor.NextMagicObject != null)
                    {
                        if (!actor.NextMagicObject.Dead && actor.NextMagicObject.Race != ObjectType.Item && actor.NextMagicObject.Race != ObjectType.Merchant)
                            target = actor.NextMagicObject;
                    }

                    if (target == null) target = MapObject.MagicObject;

                    if (target != null && target.Race == ObjectType.Monster) MapObject.MagicObjectID = target.ObjectID;
                    break;
                case Spell.Stonetrap:
                    if (!User.HasClassWeapon)
                    {
                        GameScene.Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MustWearBowForSkill));
                        User.ClearMagic();
                        return;
                    }
                    if (User.NextMagicObject != null)
                    {
                        if (!User.NextMagicObject.Dead && User.NextMagicObject.Race != ObjectType.Item && User.NextMagicObject.Race != ObjectType.Merchant)
                            target = User.NextMagicObject;
                    }

                    //if(magic.Spell == Spell.ElementalShot)
                    //{
                    //    isTargetSpell = User.HasElements;
                    //}

                    //switch(magic.Spell)
                    //{
                    //    case Spell.SummonVampire:
                    //    case Spell.SummonToad:
                    //    case Spell.SummonSnakes:
                    //        isTargetSpell = false;
                    //        break;
                    //}

                    break;
                case Spell.Purification:
                case Spell.Healing:
                case Spell.UltimateEnhancer:
                case Spell.EnergyShield:
                case Spell.PetEnhancer:
                    if (actor.NextMagicObject != null)
                    {
                        if (!actor.NextMagicObject.Dead && actor.NextMagicObject.Race != ObjectType.Item && actor.NextMagicObject.Race != ObjectType.Merchant && !(actor.NextMagicObject is MonsterObject monster && monster.MasterObjectId == 0))
                            target = actor.NextMagicObject;
                    }

                    if (target == null) target = User;
                    break;
                case Spell.FireBang:
                case Spell.MassHiding:
                case Spell.FireWall:
                case Spell.TrapHexagon:
                case Spell.HealingCircle:
                case Spell.CatTongue:
                    if (actor.NextMagicObject != null)
                    {
                        if (!actor.NextMagicObject.Dead && actor.NextMagicObject.Race != ObjectType.Item && actor.NextMagicObject.Race != ObjectType.Merchant)
                            target = actor.NextMagicObject;
                    }
                    break;
                case Spell.PoisonCloud:
                    if (actor.NextMagicObject != null)
                    {
                        if (!actor.NextMagicObject.Dead && actor.NextMagicObject.Race != ObjectType.Item && actor.NextMagicObject.Race != ObjectType.Merchant)
                            target = actor.NextMagicObject;
                    }
                    break;
                case Spell.Blizzard:
                case Spell.MeteorStrike:
                    if (actor.NextMagicObject != null)
                    {
                        if (!actor.NextMagicObject.Dead && actor.NextMagicObject.Race != ObjectType.Item && actor.NextMagicObject.Race != ObjectType.Merchant)
                            target = actor.NextMagicObject;
                    }
                    break;
                case Spell.Reincarnation:
                    if (actor == Hero && actor.NextMagicObject == null)
                        actor.NextMagicObject = User;
                    if (actor.NextMagicObject != null)
                    {
                        if (actor.NextMagicObject.Dead && actor.NextMagicObject.Race == ObjectType.Player)
                            target = actor.NextMagicObject;
                    }
                    break;
                case Spell.Trap:
                    if (actor.NextMagicObject != null)
                    {
                        if (!actor.NextMagicObject.Dead && User.NextMagicObject.Race != ObjectType.Item && actor.NextMagicObject.Race != ObjectType.Merchant)
                            target = actor.NextMagicObject;
                    }
                    break;
                case Spell.FlashDash:
                    if (actor.GetMagic(Spell.FlashDash).Level <= 1 && actor.IsDashAttack() == false)
                    {
                        actor.ClearMagic();
                        return;
                    }
                    //isTargetSpell = false;
                    break;
                default:
                    //isTargetSpell = false;
                    break;
            }

            MirDirection dir = (target == null || target == User) ? actor.NextMagicDirection : Functions.DirectionFromPoint(actor.CurrentLocation, target.CurrentLocation);

            Point location = target != null ? target.CurrentLocation : actor.NextMagicLocation;

            uint targetID = target != null ? target.ObjectID : 0;

            if (magic.Spell == Spell.FlashDash)
                dir = actor.Direction;

            if ((magic.Range != 0) && (!Functions.InRange(actor.CurrentLocation, location, magic.Range)))
            {
                if (CMain.Time >= OutputDelay)
                {
                    OutputDelay = CMain.Time + 1000;
                    GameScene.Scene.OutputMessage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.TargetTooFar));
                }
                actor.ClearMagic();
                return;
            }

            GameScene.LogTime = CMain.Time + Globals.LogDelay;

            if (actor == User)
            {
                User.QueuedAction = new QueuedAction { Action = MirAction.Spell, Direction = dir, Location = User.CurrentLocation, Params = new List<object>() };
                User.QueuedAction.Params.Add(magic.Spell);
                User.QueuedAction.Params.Add(targetID);
                User.QueuedAction.Params.Add(location);
                User.QueuedAction.Params.Add(magic.Level);
            }
            else
            {
                Network.Enqueue(new C.Magic { ObjectID = actor.ObjectID, Spell = magic.Spell, Direction = dir, TargetID = targetID, Location = location, SpellTargetLock = CMain.SpellTargetLock });
            }
        }

        public static MirDirection MouseDirection(float ratio = 45F) //22.5 = 16
        {
            Point p = new Point(MouseLocation.X / CellWidth, MouseLocation.Y / CellHeight);
            if (Functions.InRange(new Point(OffSetX, OffSetY), p, 2))
                return Functions.DirectionFromPoint(new Point(OffSetX, OffSetY), p);

            PointF c = new PointF(OffSetX * CellWidth + CellWidth / 2F, OffSetY * CellHeight + CellHeight / 2F);
            PointF a = new PointF(c.X, 0);
            PointF b = MouseLocation;
            float bc = (float)Distance(c, b);
            float ac = bc;
            b.Y -= c.Y;
            c.Y += bc;
            b.Y += bc;
            float ab = (float)Distance(b, a);
            double x = (ac * ac + bc * bc - ab * ab) / (2 * ac * bc);
            double angle = Math.Acos(x);

            angle *= 180 / Math.PI;

            if (MouseLocation.X < c.X) angle = 360 - angle;
            angle += ratio / 2;
            if (angle > 360) angle -= 360;

            return (MirDirection)(angle / ratio);
        }

        public static int Direction16(Point source, Point destination)
        {
            PointF c = new PointF(source.X, source.Y);
            PointF a = new PointF(c.X, 0);
            PointF b = new PointF(destination.X, destination.Y);
            float bc = (float)Distance(c, b);
            float ac = bc;
            b.Y -= c.Y;
            c.Y += bc;
            b.Y += bc;
            float ab = (float)Distance(b, a);
            double x = (ac * ac + bc * bc - ab * ab) / (2 * ac * bc);
            double angle = Math.Acos(x);

            angle *= 180 / Math.PI;

            if (destination.X < c.X) angle = 360 - angle;
            angle += 11.25F;
            if (angle > 360) angle -= 360;

            return (int)(angle / 22.5F);
        }

        public static double Distance(PointF p1, PointF p2)
        {
            double x = p2.X - p1.X;
            double y = p2.Y - p1.Y;
            return Math.Sqrt(x * x + y * y);
        }

        public bool EmptyCell(Point p)
        {
            if ((M2CellInfo[p.X, p.Y].BackImage & 0x20000000) != 0 || (M2CellInfo[p.X, p.Y].FrontImage & 0x8000) != 0)
                return false;

            foreach (var ob in Objects.Values)
                if (ob.CurrentLocation == p && ob.Blocking)
                    return false;

            return true;
        }


        private bool CanWalk(MirDirection dir)
        {
            return EmptyCell(Functions.PointMove(User.CurrentLocation, dir, 1)) && !User.InTrapRock;
        }

        private bool CanWalk(MirDirection dir, out MirDirection outDir)
        {
            outDir = dir;
            if (User.InTrapRock) return false;

            if (EmptyCell(Functions.PointMove(User.CurrentLocation, dir, 1)))
                return true;

            dir = Functions.NextDir(outDir);
            if (EmptyCell(Functions.PointMove(User.CurrentLocation, dir, 1)))
            {
                outDir = dir;
                return true;
            }

            dir = Functions.PreviousDir(outDir);
            if (EmptyCell(Functions.PointMove(User.CurrentLocation, dir, 1)))
            {
                outDir = dir;
                return true;
            }

            return false;
        }

        private bool CheckDoorOpen(Point p)
        {
            if (M2CellInfo[p.X, p.Y].DoorIndex == 0) return true;
            Door DoorInfo = GetDoor(M2CellInfo[p.X, p.Y].DoorIndex);
            if (DoorInfo == null) return false;//if the door doesnt exist then it isnt even being shown on screen (and cant be open lol)
            if ((DoorInfo.DoorState == DoorState.Closed) || (DoorInfo.DoorState == DoorState.Closing))
            {
                if (CMain.Time > _doorTime)
                {
                    _doorTime = CMain.Time + 4000;
                    Network.Enqueue(new C.Opendoor() { DoorIndex = DoorInfo.index });
                }

                return false;
            }
            if ((DoorInfo.DoorState == DoorState.Open) && (DoorInfo.LastTick + 4000 > CMain.Time))
            {
                if (CMain.Time > _doorTime)
                {
                    _doorTime = CMain.Time + 4000;
                    Network.Enqueue(new C.Opendoor() { DoorIndex = DoorInfo.index });
                }
            }
            return true;
        }

        private long _doorTime = 0;


        private bool CanRun(MirDirection dir)
        {
            if (User.InTrapRock) return false;
            if (User.CurrentBagWeight > User.Stats[Stat.BagWeight]) return false;
            if (User.CurrentWearWeight > User.Stats[Stat.BagWeight]) return false;
            if (CanWalk(dir) && EmptyCell(Functions.PointMove(User.CurrentLocation, dir, 2)))
            {
                if (User.RidingMount || User.Sprint && !User.Sneaking)
                {
                    return EmptyCell(Functions.PointMove(User.CurrentLocation, dir, 3));
                }

                return true;
            }

            return false;
        }

        private bool CanRideAttack()
        {
            if (GameScene.User.RidingMount)
            {
                UserItem item = GameScene.User.Equipment[(int)EquipmentSlot.Mount];
                if (item == null || item.Slots.Length < 4 || item.Slots[(int)MountSlot.Bells] == null) return false;
            }

            return true;
        }

        public bool CanFish(MirDirection dir)
        {
            if (!GameScene.User.HasFishingRod || GameScene.User.FishingTime + 1000 > CMain.Time) return false;
            if (GameScene.User.CurrentAction != MirAction.Standing) return false;
            if (GameScene.User.Direction != dir) return false;
            if (GameScene.User.TransformType >= 6 && GameScene.User.TransformType <= 9) return false;

            Point point = Functions.PointMove(User.CurrentLocation, dir, 3);

            if (!M2CellInfo[point.X, point.Y].FishingCell) return false;

            return true;
        }

        public bool CanFly(Point target)
        {
            Point location = User.CurrentLocation;
            while (location != target)
            {
                MirDirection dir = Functions.DirectionFromPoint(location, target);

                location = Functions.PointMove(location, dir, 1);

                if (location.X < 0 || location.Y < 0 || location.X >= GameScene.Scene.MapControl.Width || location.Y >= GameScene.Scene.MapControl.Height) return false;

                if (!GameScene.Scene.MapControl.ValidPoint(location)) return false;
            }

            return true;
        }


        public bool ValidPoint(Point p)
        {
            //GameScene.Scene.ChatDialog.ReceiveChat(string.Format("cell: {0}", (M2CellInfo[p.X, p.Y].BackImage & 0x20000000)), ChatType.Hint);
            return (M2CellInfo[p.X, p.Y].BackImage & 0x20000000) == 0;
        }
        public bool HasTarget(Point p)
        {
            foreach (var ob in Objects.Values)
                if (ob.CurrentLocation == p && ob.Blocking)
                    return true;
            return false;
        }

        public bool CanHalfMoon(Point p, MirDirection d)
        {
            d = Functions.PreviousDir(d);
            for (int i = 0; i < 4; i++)
            {
                if (HasTarget(Functions.PointMove(p, d, 1))) return true;
                d = Functions.NextDir(d);
            }
            return false;
        }
        public bool CanCrossHalfMoon(Point p)
        {
            MirDirection dir = MirDirection.Up;
            for (int i = 0; i < 8; i++)
            {
                if (HasTarget(Functions.PointMove(p, dir, 1))) return true;
                dir = Functions.NextDir(dir);
            }
            return false;
        }

        #region Disposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Objects.Clear();

                MapButtons = 0;
                MouseLocation = Point.Empty;
                InputDelay = 0;
                NextAction = 0;

                M2CellInfo = null;
                Width = 0;
                Height = 0;

                Index = 0;
                FileName = String.Empty;
                Title = String.Empty;
                MiniMap = 0;
                BigMap = 0;
                Lights = 0;
                FloorValid = false;
                LightsValid = false;
                MapDarkLight = 0;
                Music = 0;

                AnimationCount = 0;
                Effects.Clear();
            }

            base.Dispose(disposing);
        }

        #endregion

        public void UpdateWeather()
        {
            for (int i = GameScene.Scene.ParticleEngines.Count - 1; i > 0; i--)
                GameScene.Scene.ParticleEngines[i].Dispose();

            GameScene.Scene.ParticleEngines.Clear();
            List<ParticleImageInfo> textures = new List<ParticleImageInfo>();
            foreach (WeatherSetting itemWeather in Enum.GetValues(typeof(WeatherSetting)).Cast<object>().ToArray())
            {
                //if not enabled skip
                if ((Weather & itemWeather) != itemWeather)
                    continue;

                //foreach (WeatherSetting itemWeather in Weather)
                //{
                switch (itemWeather)
                {
                    case WeatherSetting.Leaves:
                        textures = new List<ParticleImageInfo>();
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 359, 170, 50));
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 531, 55, 50));
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 587, 200, 50));


                        ParticleEngine LeavesEngine2 = new ParticleEngine(textures, new Vector2(2f, 0), ParticleType.Leaves);
                        Vector2 lVelocity = new Vector2(0F, 0F);
                        for (int y = 512 * -1; y < Settings.ScreenHeight + 512; y += 512)
                            for (int x = 512 * -1; x < Settings.ScreenWidth + 512; x += 512)
                            {
                                Particle part = LeavesEngine2.GenerateNewParticle(ParticleType.Leaves);
                                part.Position = new Vector2(x, y);
                                part.Velocity = lVelocity;
                            }
                        LeavesEngine2.GenerateParticles = false;
                        GameScene.Scene.ParticleEngines.Add(LeavesEngine2);
                        break;
                    case WeatherSetting.FireyLeaves:
                        textures = new List<ParticleImageInfo>();
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 359, 170, 50));
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 531, 55, 50));
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 587, 200, 50));


                        ParticleEngine FLeavesEngine2 = new ParticleEngine(textures, new Vector2(2f, 0), ParticleType.FireyLeaves);
                        Vector2 FlVelocity = new Vector2(0F, 0F);
                        for (int y = 512 * -1; y < Settings.ScreenHeight + 512; y += 512)
                            for (int x = 512 * -1; x < Settings.ScreenWidth + 512; x += 512)
                            {
                                Particle part = FLeavesEngine2.GenerateNewParticle(ParticleType.FireyLeaves);
                                part.Position = new Vector2(x, y);
                                part.Velocity = FlVelocity;
                            }
                        FLeavesEngine2.GenerateParticles = false;
                        GameScene.Scene.ParticleEngines.Add(FLeavesEngine2);
                        break;
                    case WeatherSetting.Rain:
                        textures = new List<ParticleImageInfo>();
                        //Rain
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 164, 150, 50));


                        ParticleEngine RainEngine2 = new ParticleEngine(textures, new Vector2(2f, 0), ParticleType.Rain);
                        Vector2 rsevelocity = new Vector2(0F, 0F);
                        var xVar = 512;
                        var yVar = 512;
                        for (int y = yVar * -1; y < Settings.ScreenHeight + yVar; y += yVar)
                            for (int x = xVar * -1; x < Settings.ScreenWidth + xVar; x += xVar)
                            {
                                Particle part = RainEngine2.GenerateNewParticle(ParticleType.Rain);
                                part.Position = new Vector2(x, y);
                                part.Velocity = rsevelocity;
                            }
                        RainEngine2.GenerateParticles = false;
                        GameScene.Scene.ParticleEngines.Add(RainEngine2);
                        break;

                    case WeatherSetting.Snow:
                        textures = new List<ParticleImageInfo>();
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 43, 20, 50));

                        ParticleEngine RainEngine = new ParticleEngine(textures, new Vector2(0, 0), ParticleType.Snow);
                        Vector2 rsvelocity = new Vector2(1F, -1F);

                        for (int y = -400; y < Settings.ScreenHeight + 400; y += 400)
                            for (int x = -400; x < Settings.ScreenWidth + 400; x += 400)
                            {
                                Particle part = RainEngine.GenerateNewParticle(ParticleType.Snow);
                                part.Position = new Vector2(x, y);
                                part.Velocity = rsvelocity;
                            }
                        RainEngine.GenerateParticles = false;
                        GameScene.Scene.ParticleEngines.Add(RainEngine);

                        break;
                    case WeatherSetting.Fog:
                        List<ParticleImageInfo> ftextures = new List<ParticleImageInfo>();
                        ftextures.Add(new ParticleImageInfo(Libraries.Weather, 0));
                        ParticleEngine fengine = new ParticleEngine(ftextures, new Vector2(0, 0), ParticleType.Fog);
                        fengine.UpdateDelay = TimeSpan.FromMilliseconds(20);

                        Vector2 fvelocity = new Vector2(2F, -2F);
                        for (int y = -512; y < Settings.ScreenHeight + 512; y += 512)
                            for (int x = -512; x < Settings.ScreenWidth + 512; x += 512)
                            {
                                Particle part = fengine.GenerateNewParticle(ParticleType.Fog);
                                part.Position = new Vector2(x, y);
                                part.Velocity = fvelocity;
                            }


                        fengine.GenerateParticles = false;
                        GameScene.Scene.ParticleEngines.Add(fengine);
                        break;
                    case WeatherSetting.RedEmber:
                        var rtextures = new List<ParticleImageInfo>();
                        rtextures.Add(new ParticleImageInfo(Libraries.Weather, 1, 9, 150));

                        var rengine = new ParticleEngine(rtextures, new Vector2(0, 0), ParticleType.RedFogEmber);
                        GameScene.Scene.ParticleEngines.Add(rengine);
                        break;
                    case WeatherSetting.WhiteEmber:

                        textures = new List<ParticleImageInfo>();
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 1, 9, 150));
                        var whiteEmberEngine = new ParticleEngine(textures, new Vector2(0, 0), ParticleType.WhiteEmber);
                        GameScene.Scene.ParticleEngines.Add(whiteEmberEngine);
                        break;
                    case WeatherSetting.PurpleLeaves:

                        textures = new List<ParticleImageInfo>();
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 359, 170, 50));
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 531, 55, 50));
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 587, 200, 50));
                        //textures.Add(new ParticleImageInfo(Libraries.Weather, 10, 20, 50));

                        var pEmberEngine = new ParticleEngine(textures, new Vector2(0, 0), ParticleType.PurpleLeaves);

                        for (int y = 512 * -1; y < Settings.ScreenHeight + 512; y += 512)
                            for (int x = 512 * -1; x < Settings.ScreenWidth + 512; x += 512)
                            {
                                Particle part = pEmberEngine.GenerateNewParticle(ParticleType.PurpleLeaves);
                                part.Position = new Vector2(x, y);
                                part.Velocity = new Vector2(0, 0);
                            }
                        pEmberEngine.GenerateParticles = false;
                        GameScene.Scene.ParticleEngines.Add(pEmberEngine);
                        break;

                    case WeatherSetting.YellowEmber:

                        textures = new List<ParticleImageInfo>();
                        textures.Add(new ParticleImageInfo(Libraries.Weather, 1, 9, 100));

                        var yellowEmberEngine = new ParticleEngine(textures, new Vector2(0, 0), ParticleType.YellowEmber);
                        GameScene.Scene.ParticleEngines.Add(yellowEmberEngine);
                        break;
                    case WeatherSetting.FireParticle:

                        textures = new List<ParticleImageInfo>();
                        //textures.Add(new ParticleImageInfo(Libraries.StateEffect, 640)); << TODO - Win
                        //   textures.Add(new ParticleImageInfo(Libraries.Prguse4, 642));
                        var fEmberEngine = new ParticleEngine(textures, new Vector2(0, 0), ParticleType.Bird);
                        GameScene.Scene.ParticleEngines.Add(fEmberEngine);
                        break;


                }

            }

        }

        public void RemoveObject(MapObject ob)
        {
            M2CellInfo[ob.MapLocation.X, ob.MapLocation.Y].RemoveObject(ob);
        }
        public void AddObject(MapObject ob)
        {
            M2CellInfo[ob.MapLocation.X, ob.MapLocation.Y].AddObject(ob);
        }
        public MapObject FindObject(uint ObjectID, int x, int y)
        {
            return M2CellInfo[x, y].FindObject(ObjectID);
        }
        public void SortObject(MapObject ob)
        {
            M2CellInfo[ob.MapLocation.X, ob.MapLocation.Y].Sort();
        }

        public Door GetDoor(byte Index)
        {
            for (int i = 0; i < Doors.Count; i++)
            {
                if (Doors[i].index == Index)
                    return Doors[i];
            }
            return null;
        }
        public void Processdoors()
        {
            for (int i = 0; i < Doors.Count; i++)
            {
                if ((Doors[i].DoorState == DoorState.Opening) || (Doors[i].DoorState == DoorState.Closing))
                {
                    if (Doors[i].LastTick + 50 < CMain.Time)
                    {
                        Doors[i].LastTick = CMain.Time;
                        Doors[i].ImageIndex++;

                        if (Doors[i].ImageIndex == 1)//change the 1 if you want to animate doors opening/closing
                        {
                            Doors[i].ImageIndex = 0;
                            Doors[i].DoorState = (DoorState)Enum.ToObject(typeof(DoorState), ((byte)++Doors[i].DoorState % 4));
                        }

                        FloorValid = false;
                    }
                }
                if (Doors[i].DoorState == DoorState.Open)
                {
                    if (Doors[i].LastTick + 5000 < CMain.Time)
                    {
                        Doors[i].LastTick = CMain.Time;
                        Doors[i].DoorState = DoorState.Closing;
                        FloorValid = false;
                    }
                }
            }
        }
        public void OpenDoor(byte Index, bool closed)
        {
            Door Info = GetDoor(Index);
            if (Info == null) return;
            Info.DoorState = (closed ? DoorState.Closing : Info.DoorState == DoorState.Open ? DoorState.Open : DoorState.Opening);
            Info.ImageIndex = 0;
            Info.LastTick = CMain.Time;
        }
    }
}

