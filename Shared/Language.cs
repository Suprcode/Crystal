public class GameLanguage
{
    //Client
    public static string PetMode_Both = "[宠物:攻击和移动]",
                         PetMode_MoveOnly = "[宠物:不攻击]",
                         PetMode_AttackOnly = "[宠物:不移动]",
                         PetMode_None = "[宠物:不攻击不移动]",
                         PetMode_FocusMasterTarget = "[宠物:专注主人目标]",

                         AttackMode_Peace = "[模式:和平]",
                         AttackMode_Group = "[模式:组队]",
                         AttackMode_Guild = "[模式:行会]",
                         AttackMode_EnemyGuild = "[模式:敌对行会]",
                         AttackMode_RedBrown = "[模式:红名/棕名]",
                         AttackMode_All = "[模式:攻击全部]",

                         LogOutTip = "您想退出传奇吗？",
                         ExitTip = "您想退出传奇吗？",
                         DiedTip = "您已死亡，是否要在城镇中复活？",
                         DropTip = "您确定要丢弃{0}吗？",

                         Inventory = "物品栏 ({0})",
                         Character = "角色 ({0})",
                         Skills = "技能 ({0})",
                         Quests = "任务 ({0})",
                         Options = "选项 ({0})",
                         Menu = "菜单",
                         GameShop = "商城 ({0})",
                         BigMap = "大地图 ({0})",
                         DuraPanel = "持久面板",
                         Mail = "邮件",
                         Exit = "退出 ({0})",
                         LogOut = "注销 ({0})",
                         Help = "帮助 ({0})",
                         Keybinds = "键位绑定",
                         Ranking = "排行 ({0})",
                         Creatures = "宠物 ({0})",
                         Mount = "坐骑 ({0})",
                         Fishing = "钓鱼 ({0})",
                         Friends = "好友 ({0})",
                         Mentor = "师徒 ({0})",
                         Relationship = "关系 ({0})",
                         Groups = "组队 ({0})",
                         Guild = "行会 ({0})",
                         Expire = "到期: {0}",
                         ExpireNever = "到期: 从不",
                         ExpirePaused = "到期: 暂停",
                         Never = "从不",
                         Trade = "交易 ({0})",
                         Size = "大小",
                         ChatSettings = "聊天设置",
                         Rotate = "旋转",
                         Close = "关闭 ({0})",
                         GameMaster = "游戏管理员",

                         PatchErr = "无法获取补丁信息",
                         LastOnline = "上次在线",

                         Gold = "金币",
                         Credit = "信用点",

                         YouGained = "你获得了{0}。",

                         YouGained2 = "你获得了{0:###,###,###} {1}",

                         ExperienceGained = "经验增加 {0}",

                         HeroInventory = "英雄物品栏 ({0})",
                         HeroCharacter = "英雄角色 ({0})",
                         HeroSkills = "英雄技能 ({0})",
                         HeroExperienceGained = "英雄经验增加 {0}",

                         ItemDescription = "物品描述",
                         RequiredLevel = "要求等级 : {0}",
                         RequiredDC = "要求攻击 : {0}",
                         RequiredMC = "要求魔法 : {0}",
                         RequiredSC = "要求道术 : {0}",
                         ClassRequired = "要求职业 : {0}",

                         Holy = "神圣: + {0} (+{1})",
                         Holy2 = "神圣: + {0}",
                         Accuracy = "准确: + {0} (+{1})",
                         Accuracy2 = "准确: + {0}",
                         Agility = "敏捷: + {0} (+{1})",
                         Agility2 = "敏捷: + {0}",
                         DC = "攻击 + {0}~{1} (+{2})",
                         DC2 = "攻击 + {0}~{1}",
                         MC = "魔法 + {0}~{1} (+{2})",
                         MC2 = "魔法 + {0}~{1}",
                         SC = "道术 + {0}~{1} (+{2})",
                         SC2 = "道术 + {0}~{1}",
                         Durability = "持久",
                         Weight = "重量:",
                         AC = "防御 + {0}~{1} (+{2})",
                         AC2 = "防御 + {0}~{1}",
                         MAC = "魔防 + {0}~{1} (+{2})",
                         MAC2 = "魔防 + {0}~{1}",
                         Luck = "幸运 + {0}",

                         DeleteCharacter = "您确定要删除角色 {0} 吗？",
                         CharacterDeleted = "您的角色已成功删除。",
                         CharacterCreated = "您的角色已成功创建。",

                         Resolution = "分辨率",
                         Autostart = "自动启动",
                         Usrname = "用户名",
                         Password = "密码",

                         ShuttingDown = "已断开连接：服务器正在关闭。",
                         MaxCombine = "最大合并数量 : {0}{1}Shift + 左键单击可拆分堆叠",
                         Count = " 数量 {0}",
                         ExtraSlots8 = "您确定要花费1,000,000金币购买8个额外栏位吗？" +
                         "下次购买可以解锁4个额外栏位，最多可达40个栏位。",
                         ExtraSlots4 = "您确定要花费金币：{0:###,###}解锁4个额外栏位吗？",

                         Chat_All = "全部",
                         Chat_Short = "喊话",
                         Chat_Whisper = "私聊",
                         Chat_Lover = "情侣",
                         Chat_Mentor = "师徒",
                         Chat_Group = "组队",
                         Chat_Guild = "行会",
                         ExpandedStorageLocked = "扩展仓库已锁定",
                         ExtraStorage = "您想以1,000,000金币的价格租用额外仓库10天吗？",
                         ExtendYourRentalPeriod = "您想以1,000,000金币的价格将租期延长10天吗？",

                         CannotLeaveGame = " {0} 秒内无法离开游戏",
                         SelectKey = "选择按键: {0}",

                         WeaponSpiritFire = "你的武器被火之精灵附魔。",
                         SpiritsFireDisappeared = "火之精灵消失了。",
                         WeddingRing = "结婚戒指",
                         ItemTextFormat = "{0}{1}{2} {3}",
                         DropAmount = "丢弃数量:",
                         LowMana = "法力不足。",
                         NoCreatures = "您没有任何宠物。",
                         NoMount = "您没有坐骑。",
                         NoFishingRod = "您没有装备鱼竿。",
                         AttemptingConnect = "正在尝试连接到服务器。{0}尝试:{1}",

                         CreatingCharactersDisabled = "当前禁止创建新角色。",
                         InvalidCharacterName = "您的角色名称不可接受。",
                         NoClass = "您选择的职业不存在。请联系游戏管理员寻求帮助。",
                         ToManyCharacters = "您最多只能创建 {0} 个角色。",
                         CharacterNameExists = "具有此名称的角色已存在。",
                         WarriorsDes = "战士拥有强大的力量和体力。他们在战斗中不易被杀死，并且可以使用各种重型武器和盔甲。因此，战士偏爱基于近战物理伤害的攻击。他们在远程攻击方面较弱，但是专门为战士开发的各种装备弥补了他们在远程战斗中的弱点。",
                         WizardDes = "法师的力量和耐力较低，但可以使用强大的法术。他们的攻击性法术非常有效，但是由于施法需要时间，他们很容易受到敌人的攻击。因此，体力较弱的法师必须从安全距离攻击敌人。",
                         TaoistDes = "道士除了武功外，还在天文学，医学等方面训练有素。他们的专长不是直接与敌人交战，而是协助盟友提供支持。道士可以召唤强大的生物并且具有很高的魔法抵抗力，是一个攻防兼备的职业。",
                         AssassinDes = "刺客是一个秘密组织的成员，其历史相对不为人知。他们能够隐藏自己并在不被他人发现的情况下进行攻击，这自然使他们非常擅长快速击杀。由于他们的生命力和力量较弱，因此有必要避免与多个敌人作战。",
                         ArcherDes = "弓箭手拥有出色的准确性和力量，他们使用强大的弓箭技能从远处造成巨大伤害。与法师非常相似，他们依靠敏锐的直觉来躲避即将来临的攻击，因为他们往往会让自己受到正面攻击。但是，他们强大的体能和致命的瞄准能力使他们能够让任何被击中的人感到恐惧。",
                         DateSent = "发送日期 : {0}",
                         Send = "发送",
                         Reply = "回复",
                         Read = "阅读",
                         Delete = "删除",
                         BlockList = "黑名单",
                         EnterMailToName = "请输入您要邮寄的人的姓名。",
                         AddFriend = "添加",
                         RemoveFriend = "删除",
                         FriendMemo = "备注",
                         FriendMail = "邮件",
                         FriendWhisper = "私聊",
                         FriendEnterAddName = "请输入您要添加的人的姓名。",
                         FriendEnterBlockName = "请输入您要拉黑的人的姓名。",
                         AddMentor = "添加导师",
                         RemoveMentorMentee = "删除导师/学徒",
                         MentorRequests = "允许/禁止拜师请求",
                         MentorEnterName = "请输入您想拜为导师的人的姓名。",
                         RestedBuff = "休息{0}经验获取率增加 {1}%{2}",

                         ItemTypeWeapon = "武器",
                         ItemTypeArmour = "盔甲",
                         ItemTypeHelmet = "头盔",
                         ItemTypeNecklace = "项链",
                         ItemTypeBracelet = "手镯",
                         ItemTypeRing = "戒指",
                         ItemTypeAmulet = "护身符",
                         ItemTypeBelt = "腰带",
                         ItemTypeBoots = "靴子",
                         ItemTypeStone = "石头",
                         ItemTypeTorch = "火把",
                         ItemTypePotion = "药水",
                         ItemTypeOre = "矿石",
                         ItemTypeMeat = "肉",
                         ItemTypeCraftingMaterial = "工艺材料",
                         ItemTypeScroll = "卷轴",
                         ItemTypeGem = "宝石",
                         ItemTypeMount = "坐骑",
                         ItemTypeBook = "书籍",
                         ItemTypeScript = "脚本",
                         ItemTypeReins = "缰绳",
                         ItemTypeBells = "铃铛",
                         ItemTypeSaddle = "马鞍",
                         ItemTypeRibbon = "缎带",
                         ItemTypeMask = "面具",
                         ItemTypeFood = "食物",
                         ItemTypeHook = "鱼钩",
                         ItemTypeFloat = "浮漂",
                         ItemTypeBait = "鱼饵",
                         ItemTypeFinder = "探鱼器",
                         ItemTypeReel = "卷线器",
                         ItemTypeFish = "鱼",
                         ItemTypeQuest = "任务物品",
                         ItemTypeAwakening = "觉醒材料",
                         ItemTypePets = "宠物",
                         ItemTypeTransform = "变身",
                         ItemTypeDeco = "装饰",
                         ItemTypeMonsterSpawn = "怪物蛋",
                         ItemTypeSealedHero = "封印英雄",

                         ItemGradeCommon = "普通",
                         ItemGradeRare = "稀有",
                         ItemGradeLegendary = "传说",
                         ItemGradeMythical = "神话",
                         ItemGradeHeroic = "英雄",
                         NoAccountID = "该帐号ID不存在。",
                         IncorrectPasswordAccountID = "密码和帐号ID组合不正确。",
                         GroupSwitch = "允许/禁止组队请求",
                         GroupAdd = "添加",
                         GroupRemove = "删除",
                         GroupAddEnterName = "请输入您要添加的人的姓名。",
                         GroupRemoveEnterName = "请输入您要删除的人的姓名。",
                         TooHeavyToHold = "太重了，拿不住。",
                         SwitchMarriage = "允许/禁止结婚",
                         RequestMarriage = "请求结婚",
                         RequestDivorce = "请求离婚",
                         MailLover = "邮寄爱人",
                         WhisperLover = "私聊爱人";

    //Server
    public static string Welcome = "欢迎来到 {0} 服务器。",
                         OnlinePlayers = "在线玩家: {0}",
                         WeaponLuck = "幸运降临在你的武器上。",
                         WeaponCurse = "诅咒降临在你的武器上。",
                         WeaponNoEffect = "没有效果。",
                         InventoryIncreased = "物品栏大小已增加。",
                         FaceToTrade = "您必须面对某人才能交易。",
                         NoTownTeleport = "您不能在此处使用城镇传送。",
                         CanNotRandom = "您不能在此处使用随机传送。",
                         CanNotDungeon = "您不能在此处使用地牢逃脱。",
                         CannotResurrection = "您不能在活着的时候使用复活卷轴。",
                         CanNotDrop = "您不能在此地图上丢弃物品。",
                         NewMail = "有新邮件。",
                         CouldNotFindPlayer = "找不到玩家 {0}",
                         BeenPoisoned = "你中毒了",
                         AllowingMentorRequests = "您现在允许拜师请求。",
                         BlockingMentorRequests = "您现在阻止拜师请求。";

    //common
    public static string LowLevel = "您的等级不够。",
                         LowGold = "金币不足。",
                         LevelUp = "恭喜！您已升级。您的HP和MP已恢复。",
                         LowDC = "您的攻击不足。",
                         LowMC = "您的魔法不足。",
                         LowSC = "您的道术不足。",
                         GameName = "传奇2",
                         ExpandedStorageExpiresOn = "扩展仓库到期时间",

                         NotFemale = "您不是女性。",
                         NotMale = "您不是男性。",
                         NotInGuild = "您不在行会中。",
                         NoMentorship = "您当前没有师徒关系可以取消。",
                         NoBagSpace = "您的物品栏空间不足。";

    // Launcher specific strings
    public static string Launcher_EndOfStreamError = "发现流的末尾。主机可能使用的是1.1.0.0版本之前的补丁系统",
                         Launcher_ErrorTitle = "错误",
                         Launcher_TooManyProblems = "发生的问题过多，不再显示未来的错误",
                         Launcher_ProblemSavingFile = "保存此文件时出现问题: ",
                         Launcher_FailedToDownloadFile = "下载文件失败: {0}",
                         Launcher_BadHostFormat = "请检查启动器主机设置格式是否正确\n可能是由于缺少或多余的斜杠以及拼写错误引起的。\n如果不需要修补，则可以忽略此错误。",
                         Launcher_BadHostFormatTitle = "主机格式错误",
                         Launcher_BadBrowserFormat = "请检查启动器浏览器设置格式是否正确。\n可能是由于缺少或多余的斜杠以及拼写错误引起的。\n可以忽略此错误。",
                         Launcher_BadBrowserFormatTitle = "浏览器格式错误",
                         Launcher_Build = "版本: ",
                         Launcher_Debug = "调试",
                         Launcher_Release = "发布",
                         Launcher_UpToDate = "已是最新。",
                         Launcher_FilesRemaining = " 文件剩余",
                         Launcher_MBRemaining = "MB 剩余",
                         Launcher_ConcurrentDownloads = "<并发> ",
                         Launcher_DownloadFailedCheckLog = "一个或多个文件下载失败，请检查Error.txt以获取详细信息。",
                         Launcher_FailedToDownloadTitle = "下载失败。",
                         Launcher_FilesCleaned = "您的文件已清理完毕。",
                         Launcher_CleanFilesTitle = "清理文件",
                         Launcher_CreditsCrystalM2 = "由 Crystal M2 提供技术支持",
                         Launcher_CreditsBreezer = "由 Breezer 设计";

    // Units
    public static string Unit_Bytes = " 字节",
                         Unit_Kilobytes = " KB",
                         Unit_Megabytes = " MB",
                         Unit_Gigabytes = " GB",
                         Unit_Terabytes = " TB",
                         Unit_Petabytes = " PB",
                         Unit_Day = "天",
                         Unit_Hour = "小时",
                         Unit_Minute = "分钟",
                         Unit_Second = "秒";

    // Server UI specific strings
    public static string ServerUI_Total = "总计: ",
                         ServerUI_Real = ", 真实: ",
                         ServerUI_Players = "玩家: ",
                         ServerUI_Monsters = "怪物: ",
                         ServerUI_Connections = "连接: ",
                         ServerUI_BlockedIPs = "已屏蔽IP: ",
                         ServerUI_Uptime = "运行时间: ",
                         ServerUI_CycleDelays = "周期延迟: ",
                         ServerUI_CycleDelay = "周期延迟: ",
                         ServerUI_TuneMonstersServerRunning = "调整怪物参数需要服务器正在运行",
                         ServerUI_NoticeTitle = "注意",
                         ServerUI_Deleted = "已删除",
                         ServerUI_None = "无";


    public static void LoadClientLanguage(string languageIniPath)
    {
        if (!File.Exists(languageIniPath))
        {
            SaveClientLanguage(languageIniPath);
            return;
        }

        InIReader reader = new InIReader(languageIniPath);
        GameLanguage.PetMode_Both = reader.ReadString("Language", "PetMode_Both", GameLanguage.PetMode_Both);
        GameLanguage.PetMode_MoveOnly = reader.ReadString("Language", "PetMode_MoveOnly", GameLanguage.PetMode_MoveOnly);
        GameLanguage.PetMode_AttackOnly = reader.ReadString("Language", "PetMode_AttackOnly", GameLanguage.PetMode_AttackOnly);
        GameLanguage.PetMode_None = reader.ReadString("Language", "PetMode_None", GameLanguage.PetMode_None);
        GameLanguage.PetMode_FocusMasterTarget = reader.ReadString("Language", "PetMode_FocusMasterTarget", GameLanguage.PetMode_FocusMasterTarget);

        GameLanguage.AttackMode_Peace = reader.ReadString("Language", "AttackMode_Peace", GameLanguage.AttackMode_Peace);
        GameLanguage.AttackMode_Group = reader.ReadString("Language", "AttackMode_Group", GameLanguage.AttackMode_Group);
        GameLanguage.AttackMode_Guild = reader.ReadString("Language", "AttackMode_Guild", GameLanguage.AttackMode_Guild);
        GameLanguage.AttackMode_EnemyGuild = reader.ReadString("Language", "AttackMode_EnemyGuild", GameLanguage.AttackMode_EnemyGuild);
        GameLanguage.AttackMode_RedBrown = reader.ReadString("Language", "AttackMode_RedBrown", GameLanguage.AttackMode_RedBrown);
        GameLanguage.AttackMode_All = reader.ReadString("Language", "AttackMode_All", GameLanguage.AttackMode_All);

        GameLanguage.LogOutTip = reader.ReadString("Language", "LogOutTip", GameLanguage.LogOutTip);
        GameLanguage.ExitTip = reader.ReadString("Language", "ExitTip", GameLanguage.ExitTip);
        GameLanguage.DiedTip = reader.ReadString("Language", "DiedTip", GameLanguage.DiedTip);
        GameLanguage.DropTip = reader.ReadString("Language", "DropTip", GameLanguage.DropTip);

        GameLanguage.Inventory = reader.ReadString("Language", "Inventory", GameLanguage.Inventory);
        GameLanguage.Character = reader.ReadString("Language", "Character", GameLanguage.Character);
        GameLanguage.Skills = reader.ReadString("Language", "Skills", GameLanguage.Skills);
        GameLanguage.Quests = reader.ReadString("Language", "Quests", GameLanguage.Quests);
        GameLanguage.Options = reader.ReadString("Language", "Options", GameLanguage.Options);
        GameLanguage.Menu = reader.ReadString("Language", "Menu", GameLanguage.Menu);
        GameLanguage.GameShop = reader.ReadString("Language", "GameShop", GameLanguage.GameShop);
        GameLanguage.BigMap = reader.ReadString("Language", "BigMap", GameLanguage.BigMap);
        GameLanguage.DuraPanel = reader.ReadString("Language", "DuraPanel", GameLanguage.DuraPanel);
        GameLanguage.Mail = reader.ReadString("Language", "Mail", GameLanguage.Mail);
        GameLanguage.Exit = reader.ReadString("Language", "Exit", GameLanguage.Exit);
        GameLanguage.LogOut = reader.ReadString("Language", "LogOut", GameLanguage.LogOut);
        GameLanguage.Help = reader.ReadString("Language", "Help", GameLanguage.Help);
        GameLanguage.Keybinds = reader.ReadString("Language", "Keybinds", GameLanguage.Keybinds);
        GameLanguage.Ranking = reader.ReadString("Language", "Ranking", GameLanguage.Ranking);
        GameLanguage.Creatures = reader.ReadString("Language", "Creatures", GameLanguage.Creatures);
        GameLanguage.Mount = reader.ReadString("Language", "Mount", GameLanguage.Mount);
        GameLanguage.Fishing = reader.ReadString("Language", "Fishing", GameLanguage.Fishing);
        GameLanguage.Friends = reader.ReadString("Language", "Friends", GameLanguage.Friends);
        GameLanguage.Mentor = reader.ReadString("Language", "Mentor", GameLanguage.Mentor);
        GameLanguage.Relationship = reader.ReadString("Language", "Relationship", GameLanguage.Relationship);
        GameLanguage.Groups = reader.ReadString("Language", "Groups", GameLanguage.Groups);
        GameLanguage.Guild = reader.ReadString("Language", "Guild", GameLanguage.Guild);
        GameLanguage.Trade = reader.ReadString("Language", "Trade", GameLanguage.Trade);
        GameLanguage.Size = reader.ReadString("Language", "Size", GameLanguage.Size);
        GameLanguage.ChatSettings = reader.ReadString("Language", "ChatSettings", GameLanguage.ChatSettings);
        GameLanguage.Rotate = reader.ReadString("Language", "Rotate", GameLanguage.Rotate);
        GameLanguage.Close = reader.ReadString("Language", "Close", GameLanguage.Close);
        GameLanguage.GameMaster = reader.ReadString("Language", "GameMaster", GameLanguage.GameMaster);
        GameLanguage.Expire = reader.ReadString("Language", "Expire", GameLanguage.Expire);
        GameLanguage.ExpireNever = reader.ReadString("Language", "ExpireNever", GameLanguage.ExpireNever);
        GameLanguage.ExpirePaused = reader.ReadString("Language", "ExpirePaused", GameLanguage.ExpirePaused);
        GameLanguage.Never = reader.ReadString("Language", "Never", GameLanguage.Never);

        GameLanguage.PatchErr = reader.ReadString("Language", "PatchErr", GameLanguage.PatchErr);
        GameLanguage.LastOnline = reader.ReadString("Language", "LastOnline", GameLanguage.LastOnline);

        GameLanguage.LowLevel = reader.ReadString("Language", "LowLevel", GameLanguage.LowLevel);
        GameLanguage.LowGold = reader.ReadString("Language", "LowGold", GameLanguage.LowGold);
        GameLanguage.LowDC = reader.ReadString("Language", "LowDC", GameLanguage.LowDC);
        GameLanguage.LowMC = reader.ReadString("Language", "LowMC", GameLanguage.LowMC);
        GameLanguage.LowSC = reader.ReadString("Language", "LowSC", GameLanguage.LowSC);

        GameLanguage.Gold = reader.ReadString("Language", "Gold", GameLanguage.Gold);
        GameLanguage.Credit = reader.ReadString("Language", "Credit", GameLanguage.Credit);

        GameLanguage.YouGained = reader.ReadString("Language", "YouGained", GameLanguage.YouGained);
        GameLanguage.YouGained2 = reader.ReadString("Language", "YouGained2", GameLanguage.YouGained2);
        GameLanguage.ExperienceGained = reader.ReadString("Language", "ExperienceGained", GameLanguage.ExperienceGained);        
        GameLanguage.LevelUp = reader.ReadString("Language", "LevelUp", GameLanguage.LevelUp);

        GameLanguage.HeroInventory = reader.ReadString("Language", "HeroInventory", GameLanguage.HeroInventory);
        GameLanguage.HeroCharacter = reader.ReadString("Language", "HeroCharacter", GameLanguage.HeroCharacter);
        GameLanguage.HeroSkills = reader.ReadString("Language", "HeroSkills", GameLanguage.HeroSkills);
        GameLanguage.HeroExperienceGained = reader.ReadString("Language", "HeroExperienceGained", GameLanguage.HeroExperienceGained);

        GameLanguage.ItemDescription = reader.ReadString("Language", "ItemDescription", GameLanguage.ItemDescription);
        GameLanguage.RequiredLevel = reader.ReadString("Language", "RequiredLevel", GameLanguage.RequiredLevel);
        GameLanguage.RequiredDC = reader.ReadString("Language", "RequiredDC", GameLanguage.RequiredDC);
        GameLanguage.RequiredMC = reader.ReadString("Language", "RequiredMC", GameLanguage.RequiredMC);
        GameLanguage.RequiredSC = reader.ReadString("Language", "RequiredSC", GameLanguage.RequiredSC);
        GameLanguage.ClassRequired = reader.ReadString("Language", "ClassRequired", GameLanguage.ClassRequired);
        GameLanguage.Holy = reader.ReadString("Language", "Holy", GameLanguage.Holy);
        GameLanguage.Holy2 = reader.ReadString("Language", "Holy2", GameLanguage.Holy2);
        GameLanguage.Accuracy = reader.ReadString("Language", "Accuracy", GameLanguage.Accuracy);
        GameLanguage.Accuracy2 = reader.ReadString("Language", "Accuracy2", GameLanguage.Accuracy2);
        GameLanguage.Agility = reader.ReadString("Language", "Agility", GameLanguage.Agility);
        GameLanguage.Agility2 = reader.ReadString("Language", "Agility2", GameLanguage.Agility2);
        GameLanguage.DC = reader.ReadString("Language", "DC", GameLanguage.DC);
        GameLanguage.DC2 = reader.ReadString("Language", "DC2", GameLanguage.DC2);
        GameLanguage.MC = reader.ReadString("Language", "MC", GameLanguage.MC);
        GameLanguage.MC2 = reader.ReadString("Language", "MC2", GameLanguage.MC2);
        GameLanguage.SC = reader.ReadString("Language", "SC", GameLanguage.SC);
        GameLanguage.SC2 = reader.ReadString("Language", "SC2", GameLanguage.SC2);
        GameLanguage.Durability = reader.ReadString("Language", "Durability", GameLanguage.Durability);
        GameLanguage.Weight = reader.ReadString("Language", "Weight", GameLanguage.Weight);
        GameLanguage.AC = reader.ReadString("Language", "AC", GameLanguage.AC);
        GameLanguage.AC2 = reader.ReadString("Language", "AC2", GameLanguage.AC2);
        GameLanguage.MAC = reader.ReadString("Language", "MAC", GameLanguage.MAC);
        GameLanguage.MAC2 = reader.ReadString("Language", "MAC2", GameLanguage.MAC2);
        GameLanguage.Luck = reader.ReadString("Language", "Luck", GameLanguage.Luck);

        GameLanguage.DeleteCharacter = reader.ReadString("Language", "DeleteCharacter", GameLanguage.DeleteCharacter);
        GameLanguage.CharacterDeleted = reader.ReadString("Language", "CharacterDeleted", GameLanguage.CharacterDeleted);
        GameLanguage.CharacterCreated = reader.ReadString("Language", "CharacterCreated", GameLanguage.CharacterCreated);

        GameLanguage.Resolution = reader.ReadString("Language", "Resolution", GameLanguage.Resolution);
        GameLanguage.Autostart = reader.ReadString("Language", "Autostart", GameLanguage.Autostart);
        GameLanguage.Usrname = reader.ReadString("Language", "Usrname", GameLanguage.Usrname);
        GameLanguage.Password = reader.ReadString("Language", "Password", GameLanguage.Password);

        GameLanguage.ShuttingDown = reader.ReadString("Language", "ShuttingDown", GameLanguage.ShuttingDown);

        GameLanguage.MaxCombine = reader.ReadString("Language", "MaxCombine", GameLanguage.MaxCombine);
        GameLanguage.Count = reader.ReadString("Language", "Count", GameLanguage.Count);
        GameLanguage.ExtraSlots8 = reader.ReadString("Language", "ExtraSlots8", GameLanguage.ExtraSlots8);
        GameLanguage.ExtraSlots4 = reader.ReadString("Language", "ExtraSlots4", GameLanguage.ExtraSlots4);

        GameLanguage.Chat_All = reader.ReadString("Language", "Chat_All", GameLanguage.Chat_All);
        GameLanguage.Chat_Short = reader.ReadString("Language", "Chat_Short", GameLanguage.Chat_Short);
        GameLanguage.Chat_Whisper = reader.ReadString("Language", "Chat_Whisper", GameLanguage.Chat_Whisper);
        GameLanguage.Chat_Lover = reader.ReadString("Language", "Chat_Lover", GameLanguage.Chat_Lover);
        GameLanguage.Chat_Mentor = reader.ReadString("Language", "Chat_Mentor", GameLanguage.Chat_Mentor);
        GameLanguage.Chat_Group = reader.ReadString("Language", "Chat_Group", GameLanguage.Chat_Group);
        GameLanguage.Chat_Guild = reader.ReadString("Language", "Chat_Guild", GameLanguage.Chat_Guild);
        GameLanguage.ExpandedStorageLocked = reader.ReadString("Language", "ExpandedStorageLocked", GameLanguage.ExpandedStorageLocked);
        GameLanguage.ExtraStorage = reader.ReadString("Language", "ExtraStorage", GameLanguage.ExtraStorage);
        GameLanguage.ExtendYourRentalPeriod = reader.ReadString("Language", "ExtendYourRentalPeriod", GameLanguage.ExtendYourRentalPeriod);
        GameLanguage.ExpandedStorageExpiresOn = reader.ReadString("Language", "ExpandedStorageExpiresOn", GameLanguage.ExpandedStorageExpiresOn);
        GameLanguage.GameName = reader.ReadString("Language", "GameName", GameLanguage.GameName);
        GameLanguage.CannotLeaveGame = reader.ReadString("Language", "CannotLeaveGame", GameLanguage.CannotLeaveGame);
        GameLanguage.SelectKey = reader.ReadString("Language", "SelectKey", GameLanguage.SelectKey);
        GameLanguage.WeaponSpiritFire = reader.ReadString("Language", "WeaponSpiritFire", GameLanguage.WeaponSpiritFire);
        GameLanguage.SpiritsFireDisappeared = reader.ReadString("Language", "SpiritsFireDisappeared", GameLanguage.SpiritsFireDisappeared);
        GameLanguage.WeddingRing = reader.ReadString("Language", "WeddingRing", GameLanguage.WeddingRing);
        GameLanguage.ItemTextFormat = reader.ReadString("Language", "ItemTextFormat", GameLanguage.ItemTextFormat);
        GameLanguage.DropAmount = reader.ReadString("Language", "DropAmount", GameLanguage.DropAmount);
        GameLanguage.LowMana = reader.ReadString("Language", "LowMana", GameLanguage.LowMana);

        GameLanguage.NotFemale = reader.ReadString("Language", "NotFemale", GameLanguage.NotFemale);
        GameLanguage.NotMale = reader.ReadString("Language", "NotMale", GameLanguage.NotMale);
        GameLanguage.NoCreatures = reader.ReadString("Language", "NoCreatures", GameLanguage.NoCreatures);
        GameLanguage.NoMount = reader.ReadString("Language", "NoMount", GameLanguage.NoMount);
        GameLanguage.NoFishingRod = reader.ReadString("Language", "NoFishingRod", GameLanguage.NoFishingRod);
        GameLanguage.NotInGuild = reader.ReadString("Language", "NotInGuild", GameLanguage.NotInGuild);
        GameLanguage.NoBagSpace = reader.ReadString("Language", "NoBagSpace", GameLanguage.NoBagSpace);
        GameLanguage.AttemptingConnect = reader.ReadString("Language", "AttemptingConnect", GameLanguage.AttemptingConnect);

        GameLanguage.CreatingCharactersDisabled = reader.ReadString("Language", "CreatingCharactersDisabled", GameLanguage.CreatingCharactersDisabled);
        GameLanguage.InvalidCharacterName = reader.ReadString("Language", "InvalidCharacterName", GameLanguage.InvalidCharacterName);
        GameLanguage.NoClass = reader.ReadString("Language", "NoClass", GameLanguage.NoClass);
        GameLanguage.ToManyCharacters = reader.ReadString("Language", "ToManyCharacters", GameLanguage.ToManyCharacters);
        GameLanguage.CharacterNameExists = reader.ReadString("Language", "CharacterNameExists", GameLanguage.CharacterNameExists);

        GameLanguage.WarriorsDes = reader.ReadString("Language", "WarriorsDes", GameLanguage.WarriorsDes);
        GameLanguage.WizardDes = reader.ReadString("Language", "WizardDes", GameLanguage.WizardDes);
        GameLanguage.TaoistDes = reader.ReadString("Language", "TaoistDes", GameLanguage.TaoistDes);
        GameLanguage.AssassinDes = reader.ReadString("Language", "AssassinDes", GameLanguage.AssassinDes);
        GameLanguage.ArcherDes = reader.ReadString("Language", "ArcherDes", GameLanguage.ArcherDes);

        GameLanguage.DateSent = reader.ReadString("Language", "DateSent", GameLanguage.DateSent);
        GameLanguage.Send = reader.ReadString("Language", "Send", GameLanguage.Send);
        GameLanguage.Reply = reader.ReadString("Language", "Reply", GameLanguage.Reply);
        GameLanguage.Read = reader.ReadString("Language", "Read", GameLanguage.Read);
        GameLanguage.Delete = reader.ReadString("Language", "Delete", GameLanguage.Delete);
        GameLanguage.BlockList = reader.ReadString("Language", "BlockList", GameLanguage.BlockList);
        GameLanguage.EnterMailToName = reader.ReadString("Language", "EnterMailToName", GameLanguage.EnterMailToName);
        GameLanguage.BeenPoisoned = reader.ReadString("Language", "BeenPoisoned", GameLanguage.BeenPoisoned);
        GameLanguage.AddFriend = reader.ReadString("Language", "AddFriend", GameLanguage.AddFriend);
        GameLanguage.RemoveFriend = reader.ReadString("Language", "RemoveFriend", GameLanguage.RemoveFriend);
        GameLanguage.FriendMemo = reader.ReadString("Language", "FriendMemo", GameLanguage.FriendMemo);
        GameLanguage.FriendMail = reader.ReadString("Language", "FriendMail", GameLanguage.FriendMail);
        GameLanguage.FriendWhisper = reader.ReadString("Language", "FriendWhisper", GameLanguage.FriendWhisper);
        GameLanguage.FriendEnterAddName = reader.ReadString("Language", "FriendEnterAddName", GameLanguage.FriendEnterAddName);
        GameLanguage.FriendEnterBlockName = reader.ReadString("Language", "FriendEnterBlockName", GameLanguage.FriendEnterBlockName);
        GameLanguage.AddMentor = reader.ReadString("Language", "AddMentor", GameLanguage.AddMentor);
        GameLanguage.RemoveMentorMentee = reader.ReadString("Language", "RemoveMentorMentee", GameLanguage.RemoveMentorMentee);
        GameLanguage.MentorRequests = reader.ReadString("Language", "MentorRequests", GameLanguage.MentorRequests);
        GameLanguage.MentorEnterName = reader.ReadString("Language", "MentorEnterName", GameLanguage.MentorEnterName);
        GameLanguage.NoMentorship = reader.ReadString("Language", "NoMentorship", GameLanguage.NoMentorship);
        GameLanguage.RestedBuff = reader.ReadString("Language", "RestedBuff", GameLanguage.RestedBuff);

        GameLanguage.ItemTypeWeapon = reader.ReadString("Language", "ItemTypeWeapon", GameLanguage.ItemTypeWeapon);
        GameLanguage.ItemTypeArmour = reader.ReadString("Language", "ItemTypeArmour", GameLanguage.ItemTypeArmour);
        GameLanguage.ItemTypeHelmet = reader.ReadString("Language", "ItemTypeHelmet", GameLanguage.ItemTypeHelmet);
        GameLanguage.ItemTypeNecklace = reader.ReadString("Language", "ItemTypeNecklace", GameLanguage.ItemTypeNecklace);
        GameLanguage.ItemTypeBracelet = reader.ReadString("Language", "ItemTypeBracelet", GameLanguage.ItemTypeBracelet);
        GameLanguage.ItemTypeRing = reader.ReadString("Language", "ItemTypeRing", GameLanguage.ItemTypeRing);
        GameLanguage.ItemTypeAmulet = reader.ReadString("Language", "ItemTypeAmulet", GameLanguage.ItemTypeAmulet);
        GameLanguage.ItemTypeBelt = reader.ReadString("Language", "ItemTypeBelt", GameLanguage.ItemTypeBelt);
        GameLanguage.ItemTypeBoots = reader.ReadString("Language", "ItemTypeBoots", GameLanguage.ItemTypeBoots);
        GameLanguage.ItemTypeStone = reader.ReadString("Language", "ItemTypeStone", GameLanguage.ItemTypeStone);
        GameLanguage.ItemTypeTorch = reader.ReadString("Language", "ItemTypeTorch", GameLanguage.ItemTypeTorch);
        GameLanguage.ItemTypePotion = reader.ReadString("Language", "ItemTypePotion", GameLanguage.ItemTypePotion);
        GameLanguage.ItemTypeOre = reader.ReadString("Language", "ItemTypeOre", GameLanguage.ItemTypeOre);
        GameLanguage.ItemTypeMeat = reader.ReadString("Language", "ItemTypeMeat", GameLanguage.ItemTypeMeat);
        GameLanguage.ItemTypeCraftingMaterial = reader.ReadString("Language", "ItemTypeCraftingMaterial", GameLanguage.ItemTypeCraftingMaterial);
        GameLanguage.ItemTypeScroll = reader.ReadString("Language", "ItemTypeScroll", GameLanguage.ItemTypeScroll);
        GameLanguage.ItemTypeGem = reader.ReadString("Language", "ItemTypeGem", GameLanguage.ItemTypeGem);
        GameLanguage.ItemTypeMount = reader.ReadString("Language", "ItemTypeMount", GameLanguage.ItemTypeMount);
        GameLanguage.ItemTypeBook = reader.ReadString("Language", "ItemTypeBook", GameLanguage.ItemTypeBook);
        GameLanguage.ItemTypeScript = reader.ReadString("Language", "ItemTypeScript", GameLanguage.ItemTypeScript);
        GameLanguage.ItemTypeReins = reader.ReadString("Language", "ItemTypeReins", GameLanguage.ItemTypeReins);
        GameLanguage.ItemTypeBells = reader.ReadString("Language", "ItemTypeBells", GameLanguage.ItemTypeBells);
        GameLanguage.ItemTypeSaddle = reader.ReadString("Language", "ItemTypeSaddle", GameLanguage.ItemTypeSaddle);
        GameLanguage.ItemTypeRibbon = reader.ReadString("Language", "ItemTypeRibbon", GameLanguage.ItemTypeRibbon);
        GameLanguage.ItemTypeMask = reader.ReadString("Language", "ItemTypeMask", GameLanguage.ItemTypeMask);
        GameLanguage.ItemTypeFood = reader.ReadString("Language", "ItemTypeFood", GameLanguage.ItemTypeFood);
        GameLanguage.ItemTypeHook = reader.ReadString("Language", "ItemTypeHook", GameLanguage.ItemTypeHook);
        GameLanguage.ItemTypeFloat = reader.ReadString("Language", "ItemTypeFloat", GameLanguage.ItemTypeFloat);
        GameLanguage.ItemTypeBait = reader.ReadString("Language", "ItemTypeBait", GameLanguage.ItemTypeBait);
        GameLanguage.ItemTypeFinder = reader.ReadString("Language", "ItemTypeFinder", GameLanguage.ItemTypeFinder);
        GameLanguage.ItemTypeReel = reader.ReadString("Language", "ItemTypeReel", GameLanguage.ItemTypeReel);
        GameLanguage.ItemTypeFish = reader.ReadString("Language", "ItemTypeFish", GameLanguage.ItemTypeFish);
        GameLanguage.ItemTypeQuest = reader.ReadString("Language", "ItemTypeQuest", GameLanguage.ItemTypeQuest);
        GameLanguage.ItemTypeAwakening = reader.ReadString("Language", "ItemTypeAwakening", GameLanguage.ItemTypeAwakening);
        GameLanguage.ItemTypePets = reader.ReadString("Language", "ItemTypePets", GameLanguage.ItemTypePets);
        GameLanguage.ItemTypeTransform = reader.ReadString("Language", "ItemTypeTransform", GameLanguage.ItemTypeTransform);
        GameLanguage.ItemTypeSealedHero = reader.ReadString("Language", "ItemTypeSealedHero", GameLanguage.ItemTypeSealedHero);

        GameLanguage.ItemGradeCommon = reader.ReadString("Language", "ItemGradeCommon", GameLanguage.ItemGradeCommon);
        GameLanguage.ItemGradeRare = reader.ReadString("Language", "ItemGradeRare", GameLanguage.ItemGradeRare);
        GameLanguage.ItemGradeLegendary = reader.ReadString("Language", "ItemGradeLegendary", GameLanguage.ItemGradeLegendary);
        GameLanguage.ItemGradeMythical = reader.ReadString("Language", "ItemGradeMythical", GameLanguage.ItemGradeMythical);
        GameLanguage.ItemGradeHeroic = reader.ReadString("Language", "ItemGradeHeroic", GameLanguage.ItemGradeHeroic);

        GameLanguage.NoAccountID = reader.ReadString("Language", "NoAccountID", GameLanguage.NoAccountID);
        GameLanguage.IncorrectPasswordAccountID = reader.ReadString("Language", "IncorrectPasswordAccountID", GameLanguage.IncorrectPasswordAccountID);
        GameLanguage.GroupSwitch = reader.ReadString("Language", "GroupSwitch", GameLanguage.GroupSwitch);
        GameLanguage.GroupAdd = reader.ReadString("Language", "GroupAdd", GameLanguage.GroupAdd);
        GameLanguage.GroupRemove = reader.ReadString("Language", "GroupRemove", GameLanguage.GroupRemove);
        GameLanguage.GroupAddEnterName = reader.ReadString("Language", "GroupAddEnterName", GameLanguage.GroupAddEnterName);
        GameLanguage.GroupRemoveEnterName = reader.ReadString("Language", "GroupRemoveEnterName", GameLanguage.GroupRemoveEnterName);
        GameLanguage.TooHeavyToHold = reader.ReadString("Language", "TooHeavyToHold", GameLanguage.TooHeavyToHold);
        GameLanguage.SwitchMarriage = reader.ReadString("Language", "SwitchMarriage", GameLanguage.SwitchMarriage);
        GameLanguage.RequestMarriage = reader.ReadString("Language", "RequestMarriage", GameLanguage.RequestMarriage);
        GameLanguage.RequestDivorce = reader.ReadString("Language", "RequestDivorce", GameLanguage.RequestDivorce);
        GameLanguage.MailLover = reader.ReadString("Language", "MailLover", GameLanguage.MailLover);
        GameLanguage.WhisperLover = reader.ReadString("Language", "WhisperLover", GameLanguage.WhisperLover);

        // Launcher specific strings
        GameLanguage.Launcher_EndOfStreamError = reader.ReadString("Language", "Launcher_EndOfStreamError", GameLanguage.Launcher_EndOfStreamError);
        GameLanguage.Launcher_ErrorTitle = reader.ReadString("Language", "Launcher_ErrorTitle", GameLanguage.Launcher_ErrorTitle);
        GameLanguage.Launcher_TooManyProblems = reader.ReadString("Language", "Launcher_TooManyProblems", GameLanguage.Launcher_TooManyProblems);
        GameLanguage.Launcher_ProblemSavingFile = reader.ReadString("Language", "Launcher_ProblemSavingFile", GameLanguage.Launcher_ProblemSavingFile);
        GameLanguage.Launcher_FailedToDownloadFile = reader.ReadString("Language", "Launcher_FailedToDownloadFile", GameLanguage.Launcher_FailedToDownloadFile);
        GameLanguage.Launcher_BadHostFormat = reader.ReadString("Language", "Launcher_BadHostFormat", GameLanguage.Launcher_BadHostFormat);
        GameLanguage.Launcher_BadHostFormatTitle = reader.ReadString("Language", "Launcher_BadHostFormatTitle", GameLanguage.Launcher_BadHostFormatTitle);
        GameLanguage.Launcher_BadBrowserFormat = reader.ReadString("Language", "Launcher_BadBrowserFormat", GameLanguage.Launcher_BadBrowserFormat);
        GameLanguage.Launcher_BadBrowserFormatTitle = reader.ReadString("Language", "Launcher_BadBrowserFormatTitle", GameLanguage.Launcher_BadBrowserFormatTitle);
        GameLanguage.Launcher_Build = reader.ReadString("Language", "Launcher_Build", GameLanguage.Launcher_Build);
        GameLanguage.Launcher_Debug = reader.ReadString("Language", "Launcher_Debug", GameLanguage.Launcher_Debug);
        GameLanguage.Launcher_Release = reader.ReadString("Language", "Launcher_Release", GameLanguage.Launcher_Release);
        GameLanguage.Launcher_UpToDate = reader.ReadString("Language", "Launcher_UpToDate", GameLanguage.Launcher_UpToDate);
        GameLanguage.Launcher_FilesRemaining = reader.ReadString("Language", "Launcher_FilesRemaining", GameLanguage.Launcher_FilesRemaining);
        GameLanguage.Launcher_MBRemaining = reader.ReadString("Language", "Launcher_MBRemaining", GameLanguage.Launcher_MBRemaining);
        GameLanguage.Launcher_ConcurrentDownloads = reader.ReadString("Language", "Launcher_ConcurrentDownloads", GameLanguage.Launcher_ConcurrentDownloads);
        GameLanguage.Launcher_DownloadFailedCheckLog = reader.ReadString("Language", "Launcher_DownloadFailedCheckLog", GameLanguage.Launcher_DownloadFailedCheckLog);
        GameLanguage.Launcher_FailedToDownloadTitle = reader.ReadString("Language", "Launcher_FailedToDownloadTitle", GameLanguage.Launcher_FailedToDownloadTitle);
        GameLanguage.Launcher_FilesCleaned = reader.ReadString("Language", "Launcher_FilesCleaned", GameLanguage.Launcher_FilesCleaned);
        GameLanguage.Launcher_CleanFilesTitle = reader.ReadString("Language", "Launcher_CleanFilesTitle", GameLanguage.Launcher_CleanFilesTitle);
        GameLanguage.Launcher_CreditsCrystalM2 = reader.ReadString("Language", "Launcher_CreditsCrystalM2", GameLanguage.Launcher_CreditsCrystalM2);
        GameLanguage.Launcher_CreditsBreezer = reader.ReadString("Language", "Launcher_CreditsBreezer", GameLanguage.Launcher_CreditsBreezer);

        // Units
        GameLanguage.Unit_Bytes = reader.ReadString("Language", "Unit_Bytes", GameLanguage.Unit_Bytes);
        GameLanguage.Unit_Kilobytes = reader.ReadString("Language", "Unit_Kilobytes", GameLanguage.Unit_Kilobytes);
        GameLanguage.Unit_Megabytes = reader.ReadString("Language", "Unit_Megabytes", GameLanguage.Unit_Megabytes);
        GameLanguage.Unit_Gigabytes = reader.ReadString("Language", "Unit_Gigabytes", GameLanguage.Unit_Gigabytes);
        GameLanguage.Unit_Terabytes = reader.ReadString("Language", "Unit_Terabytes", GameLanguage.Unit_Terabytes);
        GameLanguage.Unit_Petabytes = reader.ReadString("Language", "Unit_Petabytes", GameLanguage.Unit_Petabytes);
        GameLanguage.Unit_Day = reader.ReadString("Language", "Unit_Day", GameLanguage.Unit_Day);
        GameLanguage.Unit_Hour = reader.ReadString("Language", "Unit_Hour", GameLanguage.Unit_Hour);
        GameLanguage.Unit_Minute = reader.ReadString("Language", "Unit_Minute", GameLanguage.Unit_Minute);
        GameLanguage.Unit_Second = reader.ReadString("Language", "Unit_Second", GameLanguage.Unit_Second);
    }


    public static void SaveClientLanguage(string languageIniPath)
    {
        File.Delete(languageIniPath);
        InIReader reader = new InIReader(languageIniPath);
        reader.Write("Language", "PetMode_Both", GameLanguage.PetMode_Both);
        reader.Write("Language", "PetMode_MoveOnly", GameLanguage.PetMode_MoveOnly);
        reader.Write("Language", "PetMode_AttackOnly", GameLanguage.PetMode_AttackOnly);
        reader.Write("Language", "PetMode_None", GameLanguage.PetMode_None);
        reader.Write("Language", "PetMode_FocusMasterTarget", GameLanguage.PetMode_FocusMasterTarget);

        reader.Write("Language", "AttackMode_Peace", GameLanguage.AttackMode_Peace);
        reader.Write("Language", "AttackMode_Group", GameLanguage.AttackMode_Group);
        reader.Write("Language", "AttackMode_Guild", GameLanguage.AttackMode_Guild);
        reader.Write("Language", "AttackMode_EnemyGuild", GameLanguage.AttackMode_EnemyGuild);
        reader.Write("Language", "AttackMode_RedBrown", GameLanguage.AttackMode_RedBrown);
        reader.Write("Language", "AttackMode_All", GameLanguage.AttackMode_All);

        reader.Write("Language", "LogOutTip", GameLanguage.LogOutTip);
        reader.Write("Language", "ExitTip", GameLanguage.ExitTip);
        reader.Write("Language", "DiedTip", GameLanguage.DiedTip);
        reader.Write("Language", "DropTip", GameLanguage.DropTip);

        reader.Write("Language", "Inventory", GameLanguage.Inventory);
        reader.Write("Language", "Character", GameLanguage.Character);
        reader.Write("Language", "Skills", GameLanguage.Skills);
        reader.Write("Language", "Quests", GameLanguage.Quests);
        reader.Write("Language", "Options", GameLanguage.Options);
        reader.Write("Language", "Menu", GameLanguage.Menu);
        reader.Write("Language", "GameShop", GameLanguage.GameShop);
        reader.Write("Language", "BigMap", GameLanguage.BigMap);
        reader.Write("Language", "DuraPanel", GameLanguage.DuraPanel);
        reader.Write("Language", "Mail", GameLanguage.Mail);
        reader.Write("Language", "Exit", GameLanguage.Exit);
        reader.Write("Language", "LogOut", GameLanguage.LogOut);
        reader.Write("Language", "Help", GameLanguage.Help);
        reader.Write("Language", "Keybinds", GameLanguage.Keybinds);
        reader.Write("Language", "Ranking", GameLanguage.Ranking);
        reader.Write("Language", "Creatures", GameLanguage.Creatures);
        reader.Write("Language", "Mount", GameLanguage.Mount);
        reader.Write("Language", "Fishing", GameLanguage.Fishing);
        reader.Write("Language", "Friends", GameLanguage.Friends);
        reader.Write("Language", "Mentor", GameLanguage.Mentor);
        reader.Write("Language", "Relationship", GameLanguage.Relationship);
        reader.Write("Language", "Groups", GameLanguage.Groups);
        reader.Write("Language", "Guild", GameLanguage.Guild);
        reader.Write("Language", "Trade", GameLanguage.Trade);
        reader.Write("Language", "Size", GameLanguage.Size);
        reader.Write("Language", "ChatSettings", GameLanguage.ChatSettings);
        reader.Write("Language", "Rotate", GameLanguage.Rotate);
        reader.Write("Language", "Close", GameLanguage.Close);
        reader.Write("Language", "GameMaster", GameLanguage.GameMaster);


        reader.Write("Language", "Expire", GameLanguage.Expire);
        reader.Write("Language", "ExpireNever", GameLanguage.ExpireNever);
        reader.Write("Language", "ExpirePaused", GameLanguage.ExpirePaused);
        reader.Write("Language", "Never", GameLanguage.Never);
        reader.Write("Language", "PatchErr", GameLanguage.PatchErr);
        reader.Write("Language", "LastOnline", GameLanguage.LastOnline);

        reader.Write("Language", "LowLevel", GameLanguage.LowLevel);
        reader.Write("Language", "LowGold", GameLanguage.LowGold);
        reader.Write("Language", "LowDC", GameLanguage.LowDC);
        reader.Write("Language", "LowMC", GameLanguage.LowMC);
        reader.Write("Language", "LowSC", GameLanguage.LowSC);

        reader.Write("Language", "Gold", GameLanguage.Gold);
        reader.Write("Language", "Credit", GameLanguage.Credit);

        reader.Write("Language", "YouGained", GameLanguage.YouGained);
        reader.Write("Language", "YouGained2", GameLanguage.YouGained2);
        reader.Write("Language", "ExperienceGained", GameLanguage.ExperienceGained);        
        reader.Write("Language", "LevelUp", GameLanguage.LevelUp);

        reader.Write("Language", "HeroInventory", GameLanguage.Inventory);
        reader.Write("Language", "HeroCharacter", GameLanguage.Character);
        reader.Write("Language", "HeroSkills", GameLanguage.Skills);
        reader.Write("Language", "HeroExperienceGained", GameLanguage.HeroExperienceGained);

        reader.Write("Language", "ItemDescription", GameLanguage.ItemDescription);
        reader.Write("Language", "RequiredLevel", GameLanguage.RequiredLevel);
        reader.Write("Language", "RequiredDC", GameLanguage.RequiredDC);
        reader.Write("Language", "RequiredMC", GameLanguage.RequiredMC);
        reader.Write("Language", "RequiredSC", GameLanguage.RequiredSC);
        reader.Write("Language", "ClassRequired", GameLanguage.ClassRequired);
        reader.Write("Language", "Holy", GameLanguage.Holy);
        reader.Write("Language", "Accuracy", GameLanguage.Accuracy);
        reader.Write("Language", "Agility", GameLanguage.Agility);
        reader.Write("Language", "DC", GameLanguage.DC);
        reader.Write("Language", "MC", GameLanguage.MC);
        reader.Write("Language", "SC", GameLanguage.SC);
        reader.Write("Language", "Durability", GameLanguage.Durability);
        reader.Write("Language", "Weight", GameLanguage.Weight);
        reader.Write("Language", "AC", GameLanguage.AC);
        reader.Write("Language", "MAC", GameLanguage.MAC);
        reader.Write("Language", "Luck", GameLanguage.Luck);

        reader.Write("Language", "DeleteCharacter", GameLanguage.DeleteCharacter);
        reader.Write("Language", "CharacterDeleted", GameLanguage.CharacterDeleted);
        reader.Write("Language", "CharacterCreated", GameLanguage.CharacterCreated);

        reader.Write("Language", "Resolution", GameLanguage.Resolution);
        reader.Write("Language", "Autostart", GameLanguage.Autostart);
        reader.Write("Language", "Usrname", GameLanguage.Usrname);
        reader.Write("Language", "Password", GameLanguage.Password);

        reader.Write("Language", "ShuttingDown", GameLanguage.ShuttingDown);

        reader.Write("Language", "MaxCombine", GameLanguage.MaxCombine);
        reader.Write("Language", "Count", GameLanguage.Count);
        reader.Write("Language", "ExtraSlots8", GameLanguage.ExtraSlots8);
        reader.Write("Language", "ExtraSlots4", GameLanguage.ExtraSlots4);

        reader.Write("Language", "Chat_All", GameLanguage.Chat_All);
        reader.Write("Language", "Chat_Short", GameLanguage.Chat_Short);
        reader.Write("Language", "Chat_Whisper", GameLanguage.Chat_Whisper);
        reader.Write("Language", "Chat_Lover", GameLanguage.Chat_Lover);
        reader.Write("Language", "Chat_Mentor", GameLanguage.Chat_Mentor);
        reader.Write("Language", "Chat_Group", GameLanguage.Chat_Group);
        reader.Write("Language", "Chat_Guild", GameLanguage.Chat_Guild);
        reader.Write("Language", "ExpandedStorageLocked", GameLanguage.ExpandedStorageLocked);
        reader.Write("Language", "ExtraStorage", GameLanguage.ExtraStorage);
        reader.Write("Language", "ExtendYourRentalPeriod", GameLanguage.ExtendYourRentalPeriod);
        reader.Write("Language", "ExpandedStorageExpiresOn", GameLanguage.ExpandedStorageExpiresOn);
        reader.Write("Language", "GameName", GameLanguage.GameName);
        reader.Write("Language", "CannotLeaveGame", GameLanguage.CannotLeaveGame);
        reader.Write("Language", "SelectKey", GameLanguage.SelectKey);
        reader.Write("Language", "WeaponSpiritFire", GameLanguage.WeaponSpiritFire);
        reader.Write("Language", "SpiritsFireDisappeared", GameLanguage.SpiritsFireDisappeared);
        reader.Write("Language", "WeddingRing", GameLanguage.WeddingRing);
        reader.Write("Language", "ItemTextFormat", GameLanguage.ItemTextFormat);
        reader.Write("Language", "DropAmount", GameLanguage.DropAmount);
        reader.Write("Language", "LowMana", GameLanguage.LowMana);

        reader.Write("Language", "NotFemale", GameLanguage.NotFemale);
        reader.Write("Language", "NotMale", GameLanguage.NotMale);
        reader.Write("Language", "NoCreatures", GameLanguage.NoCreatures);
        reader.Write("Language", "NoMount", GameLanguage.NoMount);
        reader.Write("Language", "NoFishingRod", GameLanguage.NoFishingRod);
        reader.Write("Language", "NotInGuild", GameLanguage.NotInGuild);
        reader.Write("Language", "AttemptingConnect", GameLanguage.AttemptingConnect);
        reader.Write("Language", "NoBagSpace", GameLanguage.NoBagSpace);

        reader.Write("Language", "CreatingCharactersDisabled", GameLanguage.CreatingCharactersDisabled);
        reader.Write("Language", "InvalidCharacterName", GameLanguage.InvalidCharacterName);
        reader.Write("Language", "NoClass", GameLanguage.NoClass);
        reader.Write("Language", "ToManyCharacters", GameLanguage.ToManyCharacters);
        reader.Write("Language", "CharacterNameExists", GameLanguage.CharacterNameExists);

        reader.Write("Language", "WarriorsDes", GameLanguage.WarriorsDes);
        reader.Write("Language", "WizardDes", GameLanguage.WizardDes);
        reader.Write("Language", "TaoistDes", GameLanguage.TaoistDes);
        reader.Write("Language", "AssassinDes", GameLanguage.AssassinDes);
        reader.Write("Language", "ArcherDes", GameLanguage.ArcherDes);

        reader.Write("Language", "DateSent", GameLanguage.DateSent);
        reader.Write("Language", "Send", GameLanguage.Send);
        reader.Write("Language", "Reply", GameLanguage.Reply);
        reader.Write("Language", "Read", GameLanguage.Read);
        reader.Write("Language", "Delete", GameLanguage.Delete);
        reader.Write("Language", "BlockList", GameLanguage.BlockList);
        reader.Write("Language", "EnterMailToName", GameLanguage.EnterMailToName);
        reader.Write("Language", "BeenPoisoned", GameLanguage.BeenPoisoned);
        reader.Write("Language", "AddFriend", GameLanguage.AddFriend);
        reader.Write("Language", "RemoveFriend", GameLanguage.RemoveFriend);
        reader.Write("Language", "FriendMemo", GameLanguage.FriendMemo);
        reader.Write("Language", "FriendMail", GameLanguage.FriendMail);
        reader.Write("Language", "FriendWhisper", GameLanguage.FriendWhisper);
        reader.Write("Language", "FriendEnterAddName", GameLanguage.FriendEnterAddName);
        reader.Write("Language", "FriendEnterBlockName", GameLanguage.FriendEnterBlockName);
        reader.Write("Language", "AddMentor", GameLanguage.AddMentor);
        reader.Write("Language", "RemoveMentorMentee", GameLanguage.RemoveMentorMentee);
        reader.Write("Language", "MentorRequests", GameLanguage.MentorRequests);
        reader.Write("Language", "MentorEnterName", GameLanguage.MentorEnterName);
        reader.Write("Language", "NoMentorship", GameLanguage.NoMentorship);
        reader.Write("Language", "RestedBuff", GameLanguage.RestedBuff);

        reader.Write("Language", "ItemTypeWeapon", GameLanguage.ItemTypeWeapon);
        reader.Write("Language", "ItemTypeArmour", GameLanguage.ItemTypeArmour);
        reader.Write("Language", "ItemTypeHelmet", GameLanguage.ItemTypeHelmet);
        reader.Write("Language", "ItemTypeNecklace", GameLanguage.ItemTypeNecklace);
        reader.Write("Language", "ItemTypeBracelet", GameLanguage.ItemTypeBracelet);
        reader.Write("Language", "ItemTypeRing", GameLanguage.ItemTypeRing);
        reader.Write("Language", "ItemTypeAmulet", GameLanguage.ItemTypeAmulet);
        reader.Write("Language", "ItemTypeBelt", GameLanguage.ItemTypeBelt);
        reader.Write("Language", "ItemTypeBoots", GameLanguage.ItemTypeBoots);
        reader.Write("Language", "ItemTypeStone", GameLanguage.ItemTypeStone);
        reader.Write("Language", "ItemTypeTorch", GameLanguage.ItemTypeTorch);
        reader.Write("Language", "ItemTypePotion", GameLanguage.ItemTypePotion);
        reader.Write("Language", "ItemTypeOre", GameLanguage.ItemTypeOre);
        reader.Write("Language", "ItemTypeMeat", GameLanguage.ItemTypeMeat);
        reader.Write("Language", "ItemTypeCraftingMaterial", GameLanguage.ItemTypeCraftingMaterial);
        reader.Write("Language", "ItemTypeScroll", GameLanguage.ItemTypeScroll);
        reader.Write("Language", "ItemTypeGem", GameLanguage.ItemTypeGem);
        reader.Write("Language", "ItemTypeMount", GameLanguage.ItemTypeMount);
        reader.Write("Language", "ItemTypeBook", GameLanguage.ItemTypeBook);
        reader.Write("Language", "ItemTypeScript", GameLanguage.ItemTypeScript);
        reader.Write("Language", "ItemTypeReins", GameLanguage.ItemTypeReins);
        reader.Write("Language", "ItemTypeBells", GameLanguage.ItemTypeBells);
        reader.Write("Language", "ItemTypeSaddle", GameLanguage.ItemTypeSaddle);
        reader.Write("Language", "ItemTypeRibbon", GameLanguage.ItemTypeRibbon);
        reader.Write("Language", "ItemTypeMask", GameLanguage.ItemTypeMask);
        reader.Write("Language", "ItemTypeFood", GameLanguage.ItemTypeFood);
        reader.Write("Language", "ItemTypeHook", GameLanguage.ItemTypeHook);
        reader.Write("Language", "ItemTypeFloat", GameLanguage.ItemTypeFloat);
        reader.Write("Language", "ItemTypeBait", GameLanguage.ItemTypeBait);
        reader.Write("Language", "ItemTypeFinder", GameLanguage.ItemTypeFinder);
        reader.Write("Language", "ItemTypeReel", GameLanguage.ItemTypeReel);
        reader.Write("Language", "ItemTypeFish", GameLanguage.ItemTypeFish);
        reader.Write("Language", "ItemTypeQuest", GameLanguage.ItemTypeQuest);
        reader.Write("Language", "ItemTypeAwakening", GameLanguage.ItemTypeAwakening);
        reader.Write("Language", "ItemTypePets", GameLanguage.ItemTypePets);
        reader.Write("Language", "ItemTypeTransform", GameLanguage.ItemTypeTransform);
        reader.Write("Language", "ItemTypeSealedHero", GameLanguage.ItemTypeSealedHero);

        reader.Write("Language", "ItemGradeCommon", GameLanguage.ItemGradeCommon);
        reader.Write("Language", "ItemGradeRare", GameLanguage.ItemGradeRare);
        reader.Write("Language", "ItemGradeLegendary", GameLanguage.ItemGradeLegendary);
        reader.Write("Language", "ItemGradeMythical", GameLanguage.ItemGradeMythical);
        reader.Write("Language", "ItemGradeHeroic", GameLanguage.ItemGradeHeroic);

        reader.Write("Language", "NoAccountID", GameLanguage.NoAccountID);
        reader.Write("Language", "IncorrectPasswordAccountID", GameLanguage.IncorrectPasswordAccountID);
        reader.Write("Language", "GroupSwitch", GameLanguage.GroupSwitch);
        reader.Write("Language", "GroupAdd", GameLanguage.GroupAdd);
        reader.Write("Language", "GroupRemove", GameLanguage.GroupRemove);
        reader.Write("Language", "GroupAddEnterName", GameLanguage.GroupAddEnterName);
        reader.Write("Language", "GroupRemoveEnterName", GameLanguage.GroupRemoveEnterName);
        reader.Write("Language", "TooHeavyToHold", GameLanguage.TooHeavyToHold);
        reader.Write("Language", "SwitchMarriage", GameLanguage.SwitchMarriage);
        reader.Write("Language", "RequestMarriage", GameLanguage.RequestMarriage);
        reader.Write("Language", "RequestDivorce", GameLanguage.RequestDivorce);
        reader.Write("Language", "MailLover", GameLanguage.MailLover);
        reader.Write("Language", "WhisperLover", GameLanguage.WhisperLover);

        // Launcher specific strings
        reader.Write("Language", "Launcher_EndOfStreamError", GameLanguage.Launcher_EndOfStreamError);
        reader.Write("Language", "Launcher_ErrorTitle", GameLanguage.Launcher_ErrorTitle);
        reader.Write("Language", "Launcher_TooManyProblems", GameLanguage.Launcher_TooManyProblems);
        reader.Write("Language", "Launcher_ProblemSavingFile", GameLanguage.Launcher_ProblemSavingFile);
        reader.Write("Language", "Launcher_FailedToDownloadFile", GameLanguage.Launcher_FailedToDownloadFile);
        reader.Write("Language", "Launcher_BadHostFormat", GameLanguage.Launcher_BadHostFormat);
        reader.Write("Language", "Launcher_BadHostFormatTitle", GameLanguage.Launcher_BadHostFormatTitle);
        reader.Write("Language", "Launcher_BadBrowserFormat", GameLanguage.Launcher_BadBrowserFormat);
        reader.Write("Language", "Launcher_BadBrowserFormatTitle", GameLanguage.Launcher_BadBrowserFormatTitle);
        reader.Write("Language", "Launcher_Build", GameLanguage.Launcher_Build);
        reader.Write("Language", "Launcher_Debug", GameLanguage.Launcher_Debug);
        reader.Write("Language", "Launcher_Release", GameLanguage.Launcher_Release);
        reader.Write("Language", "Launcher_UpToDate", GameLanguage.Launcher_UpToDate);
        reader.Write("Language", "Launcher_FilesRemaining", GameLanguage.Launcher_FilesRemaining);
        reader.Write("Language", "Launcher_MBRemaining", GameLanguage.Launcher_MBRemaining);
        reader.Write("Language", "Launcher_ConcurrentDownloads", GameLanguage.Launcher_ConcurrentDownloads);
        reader.Write("Language", "Launcher_DownloadFailedCheckLog", GameLanguage.Launcher_DownloadFailedCheckLog);
        reader.Write("Language", "Launcher_FailedToDownloadTitle", GameLanguage.Launcher_FailedToDownloadTitle);
        reader.Write("Language", "Launcher_FilesCleaned", GameLanguage.Launcher_FilesCleaned);
        reader.Write("Language", "Launcher_CleanFilesTitle", GameLanguage.Launcher_CleanFilesTitle);
        reader.Write("Language", "Launcher_CreditsCrystalM2", GameLanguage.Launcher_CreditsCrystalM2);
        reader.Write("Language", "Launcher_CreditsBreezer", GameLanguage.Launcher_CreditsBreezer);

        // Units
        reader.Write("Language", "Unit_Bytes", GameLanguage.Unit_Bytes);
        reader.Write("Language", "Unit_Kilobytes", GameLanguage.Unit_Kilobytes);
        reader.Write("Language", "Unit_Megabytes", GameLanguage.Unit_Megabytes);
        reader.Write("Language", "Unit_Gigabytes", GameLanguage.Unit_Gigabytes);
        reader.Write("Language", "Unit_Terabytes", GameLanguage.Unit_Terabytes);
        reader.Write("Language", "Unit_Petabytes", GameLanguage.Unit_Petabytes);
        reader.Write("Language", "Unit_Day", GameLanguage.Unit_Day);
        reader.Write("Language", "Unit_Hour", GameLanguage.Unit_Hour);
        reader.Write("Language", "Unit_Minute", GameLanguage.Unit_Minute);
        reader.Write("Language", "Unit_Second", GameLanguage.Unit_Second);
    }


    public static void LoadServerLanguage(string languageIniPath)
    {
        if (!File.Exists(languageIniPath))
        {
            SaveServerLanguage(languageIniPath);
            return;
        }
        InIReader reader = new InIReader(languageIniPath);
        GameLanguage.Welcome = reader.ReadString("Language", "Welcome", GameLanguage.Welcome);
        GameLanguage.OnlinePlayers = reader.ReadString("Language", "OnlinePlayers", GameLanguage.OnlinePlayers);
        GameLanguage.LowLevel = reader.ReadString("Language", "LowLevel", GameLanguage.LowLevel);
        GameLanguage.LowGold = reader.ReadString("Language", "LowGold", GameLanguage.LowGold);
        GameLanguage.LowDC = reader.ReadString("Language", "LowDC", GameLanguage.LowDC);
        GameLanguage.LowMC = reader.ReadString("Language", "LowMC", GameLanguage.LowMC);
        GameLanguage.LowSC = reader.ReadString("Language", "LowSC", GameLanguage.LowSC);

        GameLanguage.LevelUp = reader.ReadString("Language", "LevelUp", GameLanguage.LevelUp);

        GameLanguage.WeaponLuck = reader.ReadString("Language", "WeaponLuck", GameLanguage.WeaponLuck);
        GameLanguage.WeaponCurse = reader.ReadString("Language", "WeaponCurse", GameLanguage.WeaponCurse);
        GameLanguage.WeaponNoEffect = reader.ReadString("Language", "WeaponNoEffect", GameLanguage.WeaponNoEffect);

        GameLanguage.InventoryIncreased = reader.ReadString("Language", "InventoryIncreased", GameLanguage.InventoryIncreased);
        GameLanguage.ExpandedStorageExpiresOn = reader.ReadString("Language", "ExpandedStorageExpiresOn", GameLanguage.ExpandedStorageExpiresOn);
        GameLanguage.GameName = reader.ReadString("Language", "GameName", GameLanguage.GameName);
        GameLanguage.FaceToTrade = reader.ReadString("Language", "FaceToTrade", GameLanguage.FaceToTrade);
        GameLanguage.NoTownTeleport = reader.ReadString("Language", "NoTownTeleport", GameLanguage.NoTownTeleport);
        GameLanguage.CanNotRandom = reader.ReadString("Language", "CanNotRandom", GameLanguage.CanNotRandom);
        GameLanguage.CanNotDungeon = reader.ReadString("Language", "CanNotDungeon", GameLanguage.CanNotDungeon);
        GameLanguage.CannotResurrection = reader.ReadString("Language", "CannotResurrection", GameLanguage.CannotResurrection);
        GameLanguage.CanNotDrop = reader.ReadString("Language", "CanNotDrop", GameLanguage.CanNotDrop);

        GameLanguage.NotFemale = reader.ReadString("Language", "NotFemale", GameLanguage.NotFemale);
        GameLanguage.NotMale = reader.ReadString("Language", "NotMale", GameLanguage.NotMale);
        GameLanguage.NotInGuild = reader.ReadString("Language", "NotInGuild", GameLanguage.NotInGuild);
        GameLanguage.NewMail = reader.ReadString("Language", "NewMail", GameLanguage.NewMail);
        GameLanguage.CouldNotFindPlayer = reader.ReadString("Language", "CouldNotFindPlayer", GameLanguage.CouldNotFindPlayer);
        GameLanguage.NoMentorship = reader.ReadString("Language", "NoMentorship", GameLanguage.NoMentorship);
        GameLanguage.NoBagSpace = reader.ReadString("Language", "NoBagSpace", GameLanguage.NoBagSpace);
        GameLanguage.AllowingMentorRequests = reader.ReadString("Language", "AllowingMentorRequests", GameLanguage.AllowingMentorRequests);
        GameLanguage.BlockingMentorRequests = reader.ReadString("Language", "BlockingMentorRequests", GameLanguage.BlockingMentorRequests);

        // Server UI specific strings
        GameLanguage.ServerUI_Total = reader.ReadString("Language", "ServerUI_Total", GameLanguage.ServerUI_Total);
        GameLanguage.ServerUI_Real = reader.ReadString("Language", "ServerUI_Real", GameLanguage.ServerUI_Real);
        GameLanguage.ServerUI_Players = reader.ReadString("Language", "ServerUI_Players", GameLanguage.ServerUI_Players);
        GameLanguage.ServerUI_Monsters = reader.ReadString("Language", "ServerUI_Monsters", GameLanguage.ServerUI_Monsters);
        GameLanguage.ServerUI_Connections = reader.ReadString("Language", "ServerUI_Connections", GameLanguage.ServerUI_Connections);
        GameLanguage.ServerUI_BlockedIPs = reader.ReadString("Language", "ServerUI_BlockedIPs", GameLanguage.ServerUI_BlockedIPs);
        GameLanguage.ServerUI_Uptime = reader.ReadString("Language", "ServerUI_Uptime", GameLanguage.ServerUI_Uptime);
        GameLanguage.ServerUI_CycleDelays = reader.ReadString("Language", "ServerUI_CycleDelays", GameLanguage.ServerUI_CycleDelays);
        GameLanguage.ServerUI_CycleDelay = reader.ReadString("Language", "ServerUI_CycleDelay", GameLanguage.ServerUI_CycleDelay);
        GameLanguage.ServerUI_TuneMonstersServerRunning = reader.ReadString("Language", "ServerUI_TuneMonstersServerRunning", GameLanguage.ServerUI_TuneMonstersServerRunning);
        GameLanguage.ServerUI_NoticeTitle = reader.ReadString("Language", "ServerUI_NoticeTitle", GameLanguage.ServerUI_NoticeTitle);
        GameLanguage.ServerUI_Deleted = reader.ReadString("Language", "ServerUI_Deleted", GameLanguage.ServerUI_Deleted);
        GameLanguage.ServerUI_None = reader.ReadString("Language", "ServerUI_None", GameLanguage.ServerUI_None);
    }

    public static void SaveServerLanguage(string languageIniPath)
    {
        File.Delete(languageIniPath);
        InIReader reader = new InIReader(languageIniPath);
        reader.Write("Language", "Welcome", GameLanguage.Welcome);
        reader.Write("Language", "OnlinePlayers", GameLanguage.OnlinePlayers);
        reader.Write("Language", "LowLevel", GameLanguage.LowLevel);
        reader.Write("Language", "LowGold", GameLanguage.LowGold);
        reader.Write("Language", "LowDC", GameLanguage.LowDC);
        reader.Write("Language", "LowMC", GameLanguage.LowMC);
        reader.Write("Language", "LowSC", GameLanguage.LowSC);

        reader.Write("Language", "LevelUp", GameLanguage.LevelUp);

        reader.Write("Language", "WeaponLuck", GameLanguage.WeaponLuck);
        reader.Write("Language", "WeaponCurse", GameLanguage.WeaponCurse);
        reader.Write("Language", "WeaponNoEffect", GameLanguage.WeaponNoEffect);

        reader.Write("Language", "InventoryIncreased", GameLanguage.InventoryIncreased);
        reader.Write("Language", "ExpandedStorageExpiresOn", GameLanguage.ExpandedStorageExpiresOn);
        reader.Write("Language", "GameName", GameLanguage.GameName);
        reader.Write("Language", "FaceToTrade", GameLanguage.FaceToTrade);
        reader.Write("Language", "NoTownTeleport", GameLanguage.NoTownTeleport);
        reader.Write("Language", "CanNotRandom", GameLanguage.CanNotRandom);
        reader.Write("Language", "CanNotDungeon", GameLanguage.CanNotDungeon);
        reader.Write("Language", "CannotResurrection", GameLanguage.CannotResurrection);
        reader.Write("Language", "CanNotDrop", GameLanguage.CanNotDrop);

        reader.Write("Language", "NotFemale", GameLanguage.NotFemale);
        reader.Write("Language", "NotMale", GameLanguage.NotMale);
        reader.Write("Language", "NotInGuild", GameLanguage.NotInGuild);
        reader.Write("Language", "NewMail", GameLanguage.NewMail);
        reader.Write("Language", "CouldNotFindPlayer", GameLanguage.CouldNotFindPlayer);
        reader.Write("Language", "NoMentorship", GameLanguage.NoMentorship);
        reader.Write("Language", "NoBagSpace", GameLanguage.NoBagSpace);
        reader.Write("Language", "AllowingMentorRequests", GameLanguage.AllowingMentorRequests);
        reader.Write("Language", "BlockingMentorRequests", GameLanguage.BlockingMentorRequests);

        // Server UI specific strings
        reader.Write("Language", "ServerUI_Total", GameLanguage.ServerUI_Total);
        reader.Write("Language", "ServerUI_Real", GameLanguage.ServerUI_Real);
        reader.Write("Language", "ServerUI_Players", GameLanguage.ServerUI_Players);
        reader.Write("Language", "ServerUI_Monsters", GameLanguage.ServerUI_Monsters);
        reader.Write("Language", "ServerUI_Connections", GameLanguage.ServerUI_Connections);
        reader.Write("Language", "ServerUI_BlockedIPs", GameLanguage.ServerUI_BlockedIPs);
        reader.Write("Language", "ServerUI_Uptime", GameLanguage.ServerUI_Uptime);
        reader.Write("Language", "ServerUI_CycleDelays", GameLanguage.ServerUI_CycleDelays);
        reader.Write("Language", "ServerUI_CycleDelay", GameLanguage.ServerUI_CycleDelay);
        reader.Write("Language", "ServerUI_TuneMonstersServerRunning", GameLanguage.ServerUI_TuneMonstersServerRunning);
        reader.Write("Language", "ServerUI_NoticeTitle", GameLanguage.ServerUI_NoticeTitle);
        reader.Write("Language", "ServerUI_Deleted", GameLanguage.ServerUI_Deleted);
        reader.Write("Language", "ServerUI_None", GameLanguage.ServerUI_None);
    }
}
