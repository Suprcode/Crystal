using Shared.Functions;

namespace Shared {
    public class GameLanguage {
        //Client
        public static string PetMode_Both = "[Pet: Attack and Move]",
            PetMode_MoveOnly = "[Pet: Do Not Attack]",
            PetMode_AttackOnly = "[Pet: Do Not Move]",
            PetMode_None = "[Pet: Do Not Attack or Move]",
            PetMode_FocusMasterTarget = "[Pet: Focus Master Target]",
            AttackMode_Peace = "[Mode: Peaceful]",
            AttackMode_Group = "[Mode: Group]",
            AttackMode_Guild = "[Mode: Guild]",
            AttackMode_EnemyGuild = "[Mode: Enemy Guild]",
            AttackMode_RedBrown = "[Mode: Red/Brown]",
            AttackMode_All = "[Mode: Attack All]",
            LogOutTip = "Do you want to log out of Legend of Mir?",
            ExitTip = "Do you want to quit Legend of Mir?",
            DiedTip = "You have died, Do you want to revive in town?",
            DropTip = "Are you sure you want to drop {0}?",
            Inventory = "Inventory ({0})",
            Character = "Character ({0})",
            Skills = "Skills ({0})",
            Quests = "Quests ({0})",
            Options = "Options ({0})",
            Menu = "Menu",
            GameShop = "Game Shop ({0})",
            BigMap = "BigMap ({0})",
            DuraPanel = "Dura Panel",
            Mail = "Mail",
            Exit = "Exit ({0})",
            LogOut = "Log Out ({0})",
            Help = "Help ({0})",
            Keybinds = "Keybinds",
            Ranking = "Ranking ({0})",
            Creatures = "Creatures ({0})",
            Mount = "Mount ({0})",
            Fishing = "Fishing ({0})",
            Friends = "Friends ({0})",
            Mentor = "Mentor ({0})",
            Relationship = "Relationship ({0})",
            Groups = "Groups ({0})",
            Guild = "Guild ({0})",
            Expire = "Expire: {0}",
            ExpireNever = "Expire: Never",
            ExpirePaused = "Expire: Paused",
            Never = "Never",
            Trade = "Trade ({0})",
            Size = "Size",
            ChatSettings = "Chat Settings",
            Rotate = "Rotate",
            Close = "Close ({0})",
            GameMaster = "GameMaster",
            PatchErr = "Could not get Patch Information",
            LastOnline = "Last Online",
            Gold = "Gold",
            Credit = "Credit",
            YouGained = "You gained {0}.",
            YouGained2 = "You gained {0:###,###,###} {1}",
            ExperienceGained = "Experience Gained {0}",
            HeroInventory = "Hero Inventory ({0})",
            HeroCharacter = "Hero Character ({0})",
            HeroSkills = "Hero Skills ({0})",
            HeroExperienceGained = "Hero Experience Gained {0}",
            ItemDescription = "Item Description",
            RequiredLevel = "Required Level : {0}",
            RequiredDC = "Required DC : {0}",
            RequiredMC = "Required MC : {0}",
            RequiredSC = "Required SC : {0}",
            ClassRequired = "Class Required : {0}",
            Holy = "Holy: + {0} (+{1})",
            Holy2 = "Holy: + {0}",
            Accuracy = "Accuracy: + {0} (+{1})",
            Accuracy2 = "Accuracy: + {0}",
            Agility = "Agility: + {0} (+{1})",
            Agility2 = "Agility: + {0}",
            DC = "DC + {0}~{1} (+{2})",
            DC2 = "DC + {0}~{1}",
            MC = "MC + {0}~{1} (+{2})",
            MC2 = "MC + {0}~{1}",
            SC = "SC + {0}~{1} (+{2})",
            SC2 = "SC + {0}~{1}",
            Durability = "Durability",
            Weight = "W:",
            AC = "AC + {0}~{1} (+{2})",
            AC2 = "AC + {0}~{1}",
            MAC = "MAC + {0}~{1} (+{2})",
            MAC2 = "MAC + {0}~{1}",
            Luck = "Luck + {0}",
            DeleteCharacter = "Are you sure you want to Delete the character {0}",
            CharacterDeleted = "Your character was deleted successfully.",
            CharacterCreated = "Your character was created successfully.",
            Resolution = "Resolution",
            Autostart = "Auto start",
            Usrname = "Username",
            Password = "Password",
            ShuttingDown = "Disconnected: Server is shutting down.",
            MaxCombine = "Max Combine Count : {0}{1}Shift + Left click to split the stack",
            Count = " Count {0}",
            ExtraSlots8 = "Are you sure you would like to buy 8 extra slots for 1,000,000 gold?" +
                          "Next purchase you can unlock 4 extra slots up to a maximum of 40 slots.",
            ExtraSlots4 = "Are you sure you would like to unlock 4 extra slots? for gold: {0:###,###}",
            Chat_All = "All",
            Chat_Short = "Shout",
            Chat_Whisper = "Whisper",
            Chat_Lover = "Lover",
            Chat_Mentor = "Mentor",
            Chat_Group = "Group",
            Chat_Guild = "Guild",
            ExpandedStorageLocked = "Expanded Storage Locked",
            ExtraStorage = "Would you like to rent extra storage for 10 days at a cost of 1,000,000 gold?",
            ExtendYourRentalPeriod =
                "Would you like to extend your rental period for 10 days at a cost of 1,000,000 gold?",
            CannotLeaveGame = "Cannot leave game for {0} seconds",
            SelectKey = "Select the Key for: {0}",
            WeaponSpiritFire = "Your weapon is glowed by spirit of fire.",
            SpiritsFireDisappeared = "The spirits of fire disappeared.",
            WeddingRing = "WeddingRing",
            ItemTextFormat = "{0}{1}{2} {3}",
            DropAmount = "Drop Amount:",
            LowMana = "Not Enough Mana to cast.",
            NoCreatures = "You do not own any creatures.",
            NoMount = "You do not own a mount.",
            NoFishingRod = "You are not holding a fishing rod.",
            AttemptingConnect = "Attempting to connect to the server.{0}Attempt:{1}",
            CreatingCharactersDisabled = "Creating new characters is currently disabled.",
            InvalidCharacterName = "Your Character Name is not acceptable.",
            NoClass = "The class you selected does not exist. Contact a GM for assistance.",
            ToManyCharacters = "You cannot make anymore then {0} Characters.",
            CharacterNameExists = "A Character with this name already exists.",
            WarriorsDes =
                "Warriors are a class of great strength and vitality. They are not easily killed in battle and have the advantage of being able to use" +
                " a variety of heavy weapons and Armour. Therefore, Warriors favor attacks that are based on melee physical damage. They are weak in ranged" +
                " attacks, however the variety of equipment that are developed specifically for Warriors complement their weakness in ranged combat.",
            WizardDes =
                "Wizards are a class of low strength and stamina, but have the ability to use powerful spells. Their offensive spells are very effective, but" +
                " because it takes time to cast these spells, they're likely to leave themselves open for enemy's attacks. Therefore, the physically weak wizards" +
                " must aim to attack their enemies from a safe distance.",
            TaoistDes =
                "Taoists are well disciplined in the study of Astronomy, Medicine, and others aside from Mu-Gong. Rather then directly engaging the enemies, their" +
                " specialty lies in assisting their allies with support. Taoists can summon powerful creatures and have a high resistance to magic, and is a class" +
                " with well balanced offensive and defensive abilities.",
            AssassinDes =
                "Assassins are members of a secret organization and their history is relatively unknown. They're capable of hiding themselves and performing attacks" +
                " while being unseen by others, which naturally makes them excellent at making fast kills. It is necessary for them to avoid being in battles with" +
                " multiple enemies due to their weak vitality and strength.",
            ArcherDes =
                "Archers are a class of great accuracy and strength, using their powerful skills with bows to deal extraordinary damage from range. Much like" +
                " wizards, they rely on their keen instincts to dodge oncoming attacks as they tend to leave themselves open to frontal attacks. However, their" +
                " physical prowess and deadly aim allows them to instil fear into anyone they hit.",
            DateSent = "Date Sent : {0}",
            Send = "Send",
            Reply = "Reply",
            Read = "Read",
            Delete = "Delete",
            BlockList = "Block List",
            EnterMailToName = "Please enter the name of the person you would like to mail.",
            AddFriend = "Add",
            RemoveFriend = "Remove",
            FriendMemo = "Memo",
            FriendMail = "Mail",
            FriendWhisper = "Whisper",
            FriendEnterAddName = "Please enter the name of the person you would like to Add.",
            FriendEnterBlockName = "Please enter the name of the person you would like to Block.",
            AddMentor = "Add Mentor",
            RemoveMentorMentee = "Remove Mentor/Mentee",
            MentorRequests = "Allow/Disallow Mentor Requests",
            MentorEnterName = "Please enter the name of the person you would like to be your Mentor.",
            RestedBuff = "Rested{0}Increases Exp Rate by {1}%{2}",
            ItemTypeWeapon = "Weapon",
            ItemTypeArmour = "Armour",
            ItemTypeHelmet = "Helmet",
            ItemTypeNecklace = "Necklace",
            ItemTypeBracelet = "Bracelet",
            ItemTypeRing = "Ring",
            ItemTypeAmulet = "Amulet",
            ItemTypeBelt = "Belt",
            ItemTypeBoots = "Boots",
            ItemTypeStone = "Stone",
            ItemTypeTorch = "Torch",
            ItemTypePotion = "Potion",
            ItemTypeOre = "Ore",
            ItemTypeMeat = "Meat",
            ItemTypeCraftingMaterial = "CraftingMaterial",
            ItemTypeScroll = "Scroll",
            ItemTypeGem = "Gem",
            ItemTypeMount = "Mount",
            ItemTypeBook = "Book",
            ItemTypeScript = "Script",
            ItemTypeReins = "Reins",
            ItemTypeBells = "Bells",
            ItemTypeSaddle = "Saddle",
            ItemTypeRibbon = "Ribbon",
            ItemTypeMask = "Mask",
            ItemTypeFood = "Food",
            ItemTypeHook = "Hook",
            ItemTypeFloat = "Float",
            ItemTypeBait = "Bait",
            ItemTypeFinder = "Finder",
            ItemTypeReel = "Reel",
            ItemTypeFish = "Fish",
            ItemTypeQuest = "Quest",
            ItemTypeAwakening = "Awakening",
            ItemTypePets = "Pets",
            ItemTypeTransform = "Transform",
            ItemTypeDeco = "Deco",
            ItemTypeMonsterSpawn = "SpawnEgg",
            ItemTypeSealedHero = "SealedHero",
            ItemGradeCommon = "Common",
            ItemGradeRare = "Rare",
            ItemGradeLegendary = "Legendary",
            ItemGradeMythical = "Mythical",
            ItemGradeHeroic = "Heroic",
            NoAccountID = "The AccountID does not exist.",
            IncorrectPasswordAccountID = "Incorrect Password and AccountID combination.",
            GroupSwitch = "Allow/Disallow Group Requests",
            GroupAdd = "Add",
            GroupRemove = "Remove",
            GroupAddEnterName = "Please enter the name of the person you wish to add.",
            GroupRemoveEnterName = "Please enter the name of the person you wish to remove.",
            TooHeavyToHold = "It is too heavy to Hold.",
            SwitchMarriage = "Allow/Block Marriage",
            RequestMarriage = "Request Marriage",
            RequestDivorce = "Request Divorce",
            MailLover = "Mail Lover",
            WhisperLover = "Whisper Lover";

        //Server
        public static string Welcome = "Welcome to the {0} Server.",
            OnlinePlayers = "Online Players: {0}",
            WeaponLuck = "Luck dwells within your weapon.",
            WeaponCurse = "Curse dwells within your weapon.",
            WeaponNoEffect = "No effect.",
            InventoryIncreased = "Inventory size increased.",
            FaceToTrade = "You must face someone to trade.",
            NoTownTeleport = "You cannot use Town Teleports here",
            CanNotRandom = "You cannot use Random Teleports here",
            CanNotDungeon = "You cannot use Dungeon Escapes here",
            CannotResurrection = "You cannot use Resurrection Scrolls whilst alive",
            CanNotDrop = "You cannot drop items on this map",
            NewMail = "New mail has arrived.",
            CouldNotFindPlayer = "Could not find player {0}",
            BeenPoisoned = "You have been poisoned",
            AllowingMentorRequests = "You're now allowing mentor requests.",
            BlockingMentorRequests = "You're now blocking mentor requests.";

        //common
        public static string LowLevel = "You are not a high enough level.",
            LowGold = "Not enough gold.",
            LevelUp = "Congratulations! You have leveled up. Your HP and MP have been restored.",
            LowDC = "You do not have enough DC.",
            LowMC = "You do not have enough MC.",
            LowSC = "You do not have enough SC.",
            GameName = "Legend of Mir 2",
            ExpandedStorageExpiresOn = "Expanded Storage Expires On",
            NotFemale = "You are not Female.",
            NotMale = "You are not Male.",
            NotInGuild = "You are not in a guild.",
            NoMentorship = "You don't currently have a Mentorship to cancel.",
            NoBagSpace = "You do not have enough space.";


        public static void LoadClientLanguage(string languageIniPath) {
            if(!File.Exists(languageIniPath)) {
                SaveClientLanguage(languageIniPath);
                return;
            }

            InIReader reader = new(languageIniPath);
            PetMode_Both = reader.ReadString("Language", "PetMode_Both", PetMode_Both);
            PetMode_MoveOnly = reader.ReadString("Language", "PetMode_MoveOnly", PetMode_MoveOnly);
            PetMode_AttackOnly = reader.ReadString("Language", "PetMode_AttackOnly", PetMode_AttackOnly);
            PetMode_None = reader.ReadString("Language", "PetMode_None", PetMode_None);
            PetMode_FocusMasterTarget =
                reader.ReadString("Language", "PetMode_FocusMasterTarget", PetMode_FocusMasterTarget);

            AttackMode_Peace = reader.ReadString("Language", "AttackMode_Peace", AttackMode_Peace);
            AttackMode_Group = reader.ReadString("Language", "AttackMode_Group", AttackMode_Group);
            AttackMode_Guild = reader.ReadString("Language", "AttackMode_Guild", AttackMode_Guild);
            AttackMode_EnemyGuild = reader.ReadString("Language", "AttackMode_EnemyGuild", AttackMode_EnemyGuild);
            AttackMode_RedBrown = reader.ReadString("Language", "AttackMode_RedBrown", AttackMode_RedBrown);
            AttackMode_All = reader.ReadString("Language", "AttackMode_All", AttackMode_All);

            LogOutTip = reader.ReadString("Language", "LogOutTip", LogOutTip);
            ExitTip = reader.ReadString("Language", "ExitTip", ExitTip);
            DiedTip = reader.ReadString("Language", "DiedTip", DiedTip);
            DropTip = reader.ReadString("Language", "DropTip", DropTip);

            Inventory = reader.ReadString("Language", "Inventory", Inventory);
            Character = reader.ReadString("Language", "Character", Character);
            Skills = reader.ReadString("Language", "Skills", Skills);
            Quests = reader.ReadString("Language", "Quests", Quests);
            Options = reader.ReadString("Language", "Options", Options);
            Menu = reader.ReadString("Language", "Menu", Menu);
            GameShop = reader.ReadString("Language", "GameShop", GameShop);
            BigMap = reader.ReadString("Language", "BigMap", BigMap);
            DuraPanel = reader.ReadString("Language", "DuraPanel", DuraPanel);
            Mail = reader.ReadString("Language", "Mail", Mail);
            Exit = reader.ReadString("Language", "Exit", Exit);
            LogOut = reader.ReadString("Language", "LogOut", LogOut);
            Help = reader.ReadString("Language", "Help", Help);
            Keybinds = reader.ReadString("Language", "Keybinds", Keybinds);
            Ranking = reader.ReadString("Language", "Ranking", Ranking);
            Creatures = reader.ReadString("Language", "Creatures", Creatures);
            Mount = reader.ReadString("Language", "Mount", Mount);
            Fishing = reader.ReadString("Language", "Fishing", Fishing);
            Friends = reader.ReadString("Language", "Friends", Friends);
            Mentor = reader.ReadString("Language", "Mentor", Mentor);
            Relationship = reader.ReadString("Language", "Relationship", Relationship);
            Groups = reader.ReadString("Language", "Groups", Groups);
            Guild = reader.ReadString("Language", "Guild", Guild);
            Trade = reader.ReadString("Language", "Trade", Trade);
            Size = reader.ReadString("Language", "Size", Size);
            ChatSettings = reader.ReadString("Language", "ChatSettings", ChatSettings);
            Rotate = reader.ReadString("Language", "Rotate", Rotate);
            Close = reader.ReadString("Language", "Close", Close);
            GameMaster = reader.ReadString("Language", "GameMaster", GameMaster);
            Expire = reader.ReadString("Language", "Expire", Expire);
            ExpireNever = reader.ReadString("Language", "ExpireNever", ExpireNever);
            ExpirePaused = reader.ReadString("Language", "ExpirePaused", ExpirePaused);
            Never = reader.ReadString("Language", "Never", Never);

            PatchErr = reader.ReadString("Language", "PatchErr", PatchErr);
            LastOnline = reader.ReadString("Language", "LastOnline", LastOnline);

            LowLevel = reader.ReadString("Language", "LowLevel", LowLevel);
            LowGold = reader.ReadString("Language", "LowGold", LowGold);
            LowDC = reader.ReadString("Language", "LowDC", LowDC);
            LowMC = reader.ReadString("Language", "LowMC", LowMC);
            LowSC = reader.ReadString("Language", "LowSC", LowSC);

            Gold = reader.ReadString("Language", "Gold", Gold);
            Credit = reader.ReadString("Language", "Credit", Credit);

            YouGained = reader.ReadString("Language", "YouGained", YouGained);
            YouGained2 = reader.ReadString("Language", "YouGained2", YouGained2);
            ExperienceGained = reader.ReadString("Language", "ExperienceGained", ExperienceGained);
            LevelUp = reader.ReadString("Language", "LevelUp", LevelUp);

            HeroInventory = reader.ReadString("Language", "HeroInventory", HeroInventory);
            HeroCharacter = reader.ReadString("Language", "HeroCharacter", HeroCharacter);
            HeroSkills = reader.ReadString("Language", "HeroSkills", HeroSkills);
            HeroExperienceGained = reader.ReadString("Language", "HeroExperienceGained", HeroExperienceGained);

            ItemDescription = reader.ReadString("Language", "ItemDescription", ItemDescription);
            RequiredLevel = reader.ReadString("Language", "RequiredLevel", RequiredLevel);
            RequiredDC = reader.ReadString("Language", "RequiredDC", RequiredDC);
            RequiredMC = reader.ReadString("Language", "RequiredMC", RequiredMC);
            RequiredSC = reader.ReadString("Language", "RequiredSC", RequiredSC);
            ClassRequired = reader.ReadString("Language", "ClassRequired", ClassRequired);
            Holy = reader.ReadString("Language", "Holy", Holy);
            Holy2 = reader.ReadString("Language", "Holy2", Holy2);
            Accuracy = reader.ReadString("Language", "Accuracy", Accuracy);
            Accuracy2 = reader.ReadString("Language", "Accuracy2", Accuracy2);
            Agility = reader.ReadString("Language", "Agility", Agility);
            Agility2 = reader.ReadString("Language", "Agility2", Agility2);
            DC = reader.ReadString("Language", "DC", DC);
            DC2 = reader.ReadString("Language", "DC2", DC2);
            MC = reader.ReadString("Language", "MC", MC);
            MC2 = reader.ReadString("Language", "MC2", MC2);
            SC = reader.ReadString("Language", "SC", SC);
            SC2 = reader.ReadString("Language", "SC2", SC2);
            Durability = reader.ReadString("Language", "Durability", Durability);
            Weight = reader.ReadString("Language", "Weight", Weight);
            AC = reader.ReadString("Language", "AC", AC);
            AC2 = reader.ReadString("Language", "AC2", AC2);
            MAC = reader.ReadString("Language", "MAC", MAC);
            MAC2 = reader.ReadString("Language", "MAC2", MAC2);
            Luck = reader.ReadString("Language", "Luck", Luck);

            DeleteCharacter = reader.ReadString("Language", "DeleteCharacter", DeleteCharacter);
            CharacterDeleted = reader.ReadString("Language", "CharacterDeleted", CharacterDeleted);
            CharacterCreated = reader.ReadString("Language", "CharacterCreated", CharacterCreated);

            Resolution = reader.ReadString("Language", "Resolution", Resolution);
            Autostart = reader.ReadString("Language", "Autostart", Autostart);
            Usrname = reader.ReadString("Language", "Usrname", Usrname);
            Password = reader.ReadString("Language", "Password", Password);

            ShuttingDown = reader.ReadString("Language", "ShuttingDown", ShuttingDown);

            MaxCombine = reader.ReadString("Language", "MaxCombine", MaxCombine);
            Count = reader.ReadString("Language", "Count", Count);
            ExtraSlots8 = reader.ReadString("Language", "ExtraSlots8", ExtraSlots8);
            ExtraSlots4 = reader.ReadString("Language", "ExtraSlots4", ExtraSlots4);

            Chat_All = reader.ReadString("Language", "Chat_All", Chat_All);
            Chat_Short = reader.ReadString("Language", "Chat_Short", Chat_Short);
            Chat_Whisper = reader.ReadString("Language", "Chat_Whisper", Chat_Whisper);
            Chat_Lover = reader.ReadString("Language", "Chat_Lover", Chat_Lover);
            Chat_Mentor = reader.ReadString("Language", "Chat_Mentor", Chat_Mentor);
            Chat_Group = reader.ReadString("Language", "Chat_Group", Chat_Group);
            Chat_Guild = reader.ReadString("Language", "Chat_Guild", Chat_Guild);
            ExpandedStorageLocked = reader.ReadString("Language", "ExpandedStorageLocked", ExpandedStorageLocked);
            ExtraStorage = reader.ReadString("Language", "ExtraStorage", ExtraStorage);
            ExtendYourRentalPeriod = reader.ReadString("Language", "ExtendYourRentalPeriod", ExtendYourRentalPeriod);
            ExpandedStorageExpiresOn =
                reader.ReadString("Language", "ExpandedStorageExpiresOn", ExpandedStorageExpiresOn);
            GameName = reader.ReadString("Language", "GameName", GameName);
            CannotLeaveGame = reader.ReadString("Language", "CannotLeaveGame", CannotLeaveGame);
            SelectKey = reader.ReadString("Language", "SelectKey", SelectKey);
            WeaponSpiritFire = reader.ReadString("Language", "WeaponSpiritFire", WeaponSpiritFire);
            SpiritsFireDisappeared = reader.ReadString("Language", "SpiritsFireDisappeared", SpiritsFireDisappeared);
            WeddingRing = reader.ReadString("Language", "WeddingRing", WeddingRing);
            ItemTextFormat = reader.ReadString("Language", "ItemTextFormat", ItemTextFormat);
            DropAmount = reader.ReadString("Language", "DropAmount", DropAmount);
            LowMana = reader.ReadString("Language", "LowMana", LowMana);

            NotFemale = reader.ReadString("Language", "NotFemale", NotFemale);
            NotMale = reader.ReadString("Language", "NotMale", NotMale);
            NoCreatures = reader.ReadString("Language", "NoCreatures", NoCreatures);
            NoMount = reader.ReadString("Language", "NoMount", NoMount);
            NoFishingRod = reader.ReadString("Language", "NoFishingRod", NoFishingRod);
            NotInGuild = reader.ReadString("Language", "NotInGuild", NotInGuild);
            NoBagSpace = reader.ReadString("Language", "NoBagSpace", NoBagSpace);
            AttemptingConnect = reader.ReadString("Language", "AttemptingConnect", AttemptingConnect);

            CreatingCharactersDisabled =
                reader.ReadString("Language", "CreatingCharactersDisabled", CreatingCharactersDisabled);
            InvalidCharacterName = reader.ReadString("Language", "InvalidCharacterName", InvalidCharacterName);
            NoClass = reader.ReadString("Language", "NoClass", NoClass);
            ToManyCharacters = reader.ReadString("Language", "ToManyCharacters", ToManyCharacters);
            CharacterNameExists = reader.ReadString("Language", "CharacterNameExists", CharacterNameExists);

            WarriorsDes = reader.ReadString("Language", "WarriorsDes", WarriorsDes);
            WizardDes = reader.ReadString("Language", "WizardDes", WizardDes);
            TaoistDes = reader.ReadString("Language", "TaoistDes", TaoistDes);
            AssassinDes = reader.ReadString("Language", "AssassinDes", AssassinDes);
            ArcherDes = reader.ReadString("Language", "ArcherDes", ArcherDes);

            DateSent = reader.ReadString("Language", "DateSent", DateSent);
            Send = reader.ReadString("Language", "Send", Send);
            Reply = reader.ReadString("Language", "Reply", Reply);
            Read = reader.ReadString("Language", "Read", Read);
            Delete = reader.ReadString("Language", "Delete", Delete);
            BlockList = reader.ReadString("Language", "BlockList", BlockList);
            EnterMailToName = reader.ReadString("Language", "EnterMailToName", EnterMailToName);
            BeenPoisoned = reader.ReadString("Language", "BeenPoisoned", BeenPoisoned);
            AddFriend = reader.ReadString("Language", "AddFriend", AddFriend);
            RemoveFriend = reader.ReadString("Language", "RemoveFriend", RemoveFriend);
            FriendMemo = reader.ReadString("Language", "FriendMemo", FriendMemo);
            FriendMail = reader.ReadString("Language", "FriendMail", FriendMail);
            FriendWhisper = reader.ReadString("Language", "FriendWhisper", FriendWhisper);
            FriendEnterAddName = reader.ReadString("Language", "FriendEnterAddName", FriendEnterAddName);
            FriendEnterBlockName = reader.ReadString("Language", "FriendEnterBlockName", FriendEnterBlockName);
            AddMentor = reader.ReadString("Language", "AddMentor", AddMentor);
            RemoveMentorMentee = reader.ReadString("Language", "RemoveMentorMentee", RemoveMentorMentee);
            MentorRequests = reader.ReadString("Language", "MentorRequests", MentorRequests);
            MentorEnterName = reader.ReadString("Language", "MentorEnterName", MentorEnterName);
            NoMentorship = reader.ReadString("Language", "NoMentorship", NoMentorship);
            RestedBuff = reader.ReadString("Language", "RestedBuff", RestedBuff);

            ItemTypeWeapon = reader.ReadString("Language", "ItemTypeWeapon", ItemTypeWeapon);
            ItemTypeArmour = reader.ReadString("Language", "ItemTypeArmour", ItemTypeArmour);
            ItemTypeHelmet = reader.ReadString("Language", "ItemTypeHelmet", ItemTypeHelmet);
            ItemTypeNecklace = reader.ReadString("Language", "ItemTypeNecklace", ItemTypeNecklace);
            ItemTypeBracelet = reader.ReadString("Language", "ItemTypeBracelet", ItemTypeBracelet);
            ItemTypeRing = reader.ReadString("Language", "ItemTypeRing", ItemTypeRing);
            ItemTypeAmulet = reader.ReadString("Language", "ItemTypeAmulet", ItemTypeAmulet);
            ItemTypeBelt = reader.ReadString("Language", "ItemTypeBelt", ItemTypeBelt);
            ItemTypeBoots = reader.ReadString("Language", "ItemTypeBoots", ItemTypeBoots);
            ItemTypeStone = reader.ReadString("Language", "ItemTypeStone", ItemTypeStone);
            ItemTypeTorch = reader.ReadString("Language", "ItemTypeTorch", ItemTypeTorch);
            ItemTypePotion = reader.ReadString("Language", "ItemTypePotion", ItemTypePotion);
            ItemTypeOre = reader.ReadString("Language", "ItemTypeOre", ItemTypeOre);
            ItemTypeMeat = reader.ReadString("Language", "ItemTypeMeat", ItemTypeMeat);
            ItemTypeCraftingMaterial =
                reader.ReadString("Language", "ItemTypeCraftingMaterial", ItemTypeCraftingMaterial);
            ItemTypeScroll = reader.ReadString("Language", "ItemTypeScroll", ItemTypeScroll);
            ItemTypeGem = reader.ReadString("Language", "ItemTypeGem", ItemTypeGem);
            ItemTypeMount = reader.ReadString("Language", "ItemTypeMount", ItemTypeMount);
            ItemTypeBook = reader.ReadString("Language", "ItemTypeBook", ItemTypeBook);
            ItemTypeScript = reader.ReadString("Language", "ItemTypeScript", ItemTypeScript);
            ItemTypeReins = reader.ReadString("Language", "ItemTypeReins", ItemTypeReins);
            ItemTypeBells = reader.ReadString("Language", "ItemTypeBells", ItemTypeBells);
            ItemTypeSaddle = reader.ReadString("Language", "ItemTypeSaddle", ItemTypeSaddle);
            ItemTypeRibbon = reader.ReadString("Language", "ItemTypeRibbon", ItemTypeRibbon);
            ItemTypeMask = reader.ReadString("Language", "ItemTypeMask", ItemTypeMask);
            ItemTypeFood = reader.ReadString("Language", "ItemTypeFood", ItemTypeFood);
            ItemTypeHook = reader.ReadString("Language", "ItemTypeHook", ItemTypeHook);
            ItemTypeFloat = reader.ReadString("Language", "ItemTypeFloat", ItemTypeFloat);
            ItemTypeBait = reader.ReadString("Language", "ItemTypeBait", ItemTypeBait);
            ItemTypeFinder = reader.ReadString("Language", "ItemTypeFinder", ItemTypeFinder);
            ItemTypeReel = reader.ReadString("Language", "ItemTypeReel", ItemTypeReel);
            ItemTypeFish = reader.ReadString("Language", "ItemTypeFish", ItemTypeFish);
            ItemTypeQuest = reader.ReadString("Language", "ItemTypeQuest", ItemTypeQuest);
            ItemTypeAwakening = reader.ReadString("Language", "ItemTypeAwakening", ItemTypeAwakening);
            ItemTypePets = reader.ReadString("Language", "ItemTypePets", ItemTypePets);
            ItemTypeTransform = reader.ReadString("Language", "ItemTypeTransform", ItemTypeTransform);
            ItemTypeSealedHero = reader.ReadString("Language", "ItemTypeSealedHero", ItemTypeSealedHero);

            ItemGradeCommon = reader.ReadString("Language", "ItemGradeCommon", ItemGradeCommon);
            ItemGradeRare = reader.ReadString("Language", "ItemGradeRare", ItemGradeRare);
            ItemGradeLegendary = reader.ReadString("Language", "ItemGradeLegendary", ItemGradeLegendary);
            ItemGradeMythical = reader.ReadString("Language", "ItemGradeMythical", ItemGradeMythical);
            ItemGradeHeroic = reader.ReadString("Language", "ItemGradeHeroic", ItemGradeHeroic);

            NoAccountID = reader.ReadString("Language", "NoAccountID", NoAccountID);
            IncorrectPasswordAccountID =
                reader.ReadString("Language", "IncorrectPasswordAccountID", IncorrectPasswordAccountID);
            GroupSwitch = reader.ReadString("Language", "GroupSwitch", GroupSwitch);
            GroupAdd = reader.ReadString("Language", "GroupAdd", GroupAdd);
            GroupRemove = reader.ReadString("Language", "GroupRemove", GroupRemove);
            GroupAddEnterName = reader.ReadString("Language", "GroupAddEnterName", GroupAddEnterName);
            GroupRemoveEnterName = reader.ReadString("Language", "GroupRemoveEnterName", GroupRemoveEnterName);
            TooHeavyToHold = reader.ReadString("Language", "TooHeavyToHold", TooHeavyToHold);
            SwitchMarriage = reader.ReadString("Language", "SwitchMarriage", SwitchMarriage);
            RequestMarriage = reader.ReadString("Language", "RequestMarriage", RequestMarriage);
            RequestDivorce = reader.ReadString("Language", "RequestDivorce", RequestDivorce);
            MailLover = reader.ReadString("Language", "MailLover", MailLover);
            WhisperLover = reader.ReadString("Language", "WhisperLover", WhisperLover);
        }


        public static void SaveClientLanguage(string languageIniPath) {
            File.Delete(languageIniPath);
            InIReader reader = new(languageIniPath);
            reader.Write("Language", "PetMode_Both", PetMode_Both);
            reader.Write("Language", "PetMode_MoveOnly", PetMode_MoveOnly);
            reader.Write("Language", "PetMode_AttackOnly", PetMode_AttackOnly);
            reader.Write("Language", "PetMode_None", PetMode_None);
            reader.Write("Language", "PetMode_FocusMasterTarget", PetMode_FocusMasterTarget);

            reader.Write("Language", "AttackMode_Peace", AttackMode_Peace);
            reader.Write("Language", "AttackMode_Group", AttackMode_Group);
            reader.Write("Language", "AttackMode_Guild", AttackMode_Guild);
            reader.Write("Language", "AttackMode_EnemyGuild", AttackMode_EnemyGuild);
            reader.Write("Language", "AttackMode_RedBrown", AttackMode_RedBrown);
            reader.Write("Language", "AttackMode_All", AttackMode_All);

            reader.Write("Language", "LogOutTip", LogOutTip);
            reader.Write("Language", "ExitTip", ExitTip);
            reader.Write("Language", "DiedTip", DiedTip);
            reader.Write("Language", "DropTip", DropTip);

            reader.Write("Language", "Inventory", Inventory);
            reader.Write("Language", "Character", Character);
            reader.Write("Language", "Skills", Skills);
            reader.Write("Language", "Quests", Quests);
            reader.Write("Language", "Options", Options);
            reader.Write("Language", "Menu", Menu);
            reader.Write("Language", "GameShop", GameShop);
            reader.Write("Language", "BigMap", BigMap);
            reader.Write("Language", "DuraPanel", DuraPanel);
            reader.Write("Language", "Mail", Mail);
            reader.Write("Language", "Exit", Exit);
            reader.Write("Language", "LogOut", LogOut);
            reader.Write("Language", "Help", Help);
            reader.Write("Language", "Keybinds", Keybinds);
            reader.Write("Language", "Ranking", Ranking);
            reader.Write("Language", "Creatures", Creatures);
            reader.Write("Language", "Mount", Mount);
            reader.Write("Language", "Fishing", Fishing);
            reader.Write("Language", "Friends", Friends);
            reader.Write("Language", "Mentor", Mentor);
            reader.Write("Language", "Relationship", Relationship);
            reader.Write("Language", "Groups", Groups);
            reader.Write("Language", "Guild", Guild);
            reader.Write("Language", "Trade", Trade);
            reader.Write("Language", "Size", Size);
            reader.Write("Language", "ChatSettings", ChatSettings);
            reader.Write("Language", "Rotate", Rotate);
            reader.Write("Language", "Close", Close);
            reader.Write("Language", "GameMaster", GameMaster);


            reader.Write("Language", "Expire", Expire);
            reader.Write("Language", "ExpireNever", ExpireNever);
            reader.Write("Language", "ExpirePaused", ExpirePaused);
            reader.Write("Language", "Never", Never);
            reader.Write("Language", "PatchErr", PatchErr);
            reader.Write("Language", "LastOnline", LastOnline);

            reader.Write("Language", "LowLevel", LowLevel);
            reader.Write("Language", "LowGold", LowGold);
            reader.Write("Language", "LowDC", LowDC);
            reader.Write("Language", "LowMC", LowMC);
            reader.Write("Language", "LowSC", LowSC);

            reader.Write("Language", "Gold", Gold);
            reader.Write("Language", "Credit", Credit);

            reader.Write("Language", "YouGained", YouGained);
            reader.Write("Language", "YouGained2", YouGained2);
            reader.Write("Language", "ExperienceGained", ExperienceGained);
            reader.Write("Language", "LevelUp", LevelUp);

            reader.Write("Language", "HeroInventory", Inventory);
            reader.Write("Language", "HeroCharacter", Character);
            reader.Write("Language", "HeroSkills", Skills);
            reader.Write("Language", "HeroExperienceGained", HeroExperienceGained);

            reader.Write("Language", "ItemDescription", ItemDescription);
            reader.Write("Language", "RequiredLevel", RequiredLevel);
            reader.Write("Language", "RequiredDC", RequiredDC);
            reader.Write("Language", "RequiredMC", RequiredMC);
            reader.Write("Language", "RequiredSC", RequiredSC);
            reader.Write("Language", "ClassRequired", ClassRequired);
            reader.Write("Language", "Holy", Holy);
            reader.Write("Language", "Accuracy", Accuracy);
            reader.Write("Language", "Agility", Agility);
            reader.Write("Language", "DC", DC);
            reader.Write("Language", "MC", MC);
            reader.Write("Language", "SC", SC);
            reader.Write("Language", "Durability", Durability);
            reader.Write("Language", "Weight", Weight);
            reader.Write("Language", "AC", AC);
            reader.Write("Language", "MAC", MAC);
            reader.Write("Language", "Luck", Luck);

            reader.Write("Language", "DeleteCharacter", DeleteCharacter);
            reader.Write("Language", "CharacterDeleted", CharacterDeleted);
            reader.Write("Language", "CharacterCreated", CharacterCreated);

            reader.Write("Language", "Resolution", Resolution);
            reader.Write("Language", "Autostart", Autostart);
            reader.Write("Language", "Usrname", Usrname);
            reader.Write("Language", "Password", Password);

            reader.Write("Language", "ShuttingDown", ShuttingDown);

            reader.Write("Language", "MaxCombine", MaxCombine);
            reader.Write("Language", "Count", Count);
            reader.Write("Language", "ExtraSlots8", ExtraSlots8);
            reader.Write("Language", "ExtraSlots4", ExtraSlots4);

            reader.Write("Language", "Chat_All", Chat_All);
            reader.Write("Language", "Chat_Short", Chat_Short);
            reader.Write("Language", "Chat_Whisper", Chat_Whisper);
            reader.Write("Language", "Chat_Lover", Chat_Lover);
            reader.Write("Language", "Chat_Mentor", Chat_Mentor);
            reader.Write("Language", "Chat_Group", Chat_Group);
            reader.Write("Language", "Chat_Guild", Chat_Guild);
            reader.Write("Language", "ExpandedStorageLocked", ExpandedStorageLocked);
            reader.Write("Language", "ExtraStorage", ExtraStorage);
            reader.Write("Language", "ExtendYourRentalPeriod", ExtendYourRentalPeriod);
            reader.Write("Language", "ExpandedStorageExpiresOn", ExpandedStorageExpiresOn);
            reader.Write("Language", "GameName", GameName);
            reader.Write("Language", "CannotLeaveGame", CannotLeaveGame);
            reader.Write("Language", "SelectKey", SelectKey);
            reader.Write("Language", "WeaponSpiritFire", WeaponSpiritFire);
            reader.Write("Language", "SpiritsFireDisappeared", SpiritsFireDisappeared);
            reader.Write("Language", "WeddingRing", WeddingRing);
            reader.Write("Language", "ItemTextFormat", ItemTextFormat);
            reader.Write("Language", "DropAmount", DropAmount);
            reader.Write("Language", "LowMana", LowMana);

            reader.Write("Language", "NotFemale", NotFemale);
            reader.Write("Language", "NotMale", NotMale);
            reader.Write("Language", "NoCreatures", NoCreatures);
            reader.Write("Language", "NoMount", NoMount);
            reader.Write("Language", "NoFishingRod", NoFishingRod);
            reader.Write("Language", "NotInGuild", NotInGuild);
            reader.Write("Language", "AttemptingConnect", AttemptingConnect);
            reader.Write("Language", "NoBagSpace", NoBagSpace);

            reader.Write("Language", "CreatingCharactersDisabled", CreatingCharactersDisabled);
            reader.Write("Language", "InvalidCharacterName", InvalidCharacterName);
            reader.Write("Language", "NoClass", NoClass);
            reader.Write("Language", "ToManyCharacters", ToManyCharacters);
            reader.Write("Language", "CharacterNameExists", CharacterNameExists);

            reader.Write("Language", "WarriorsDes", WarriorsDes);
            reader.Write("Language", "WizardDes", WizardDes);
            reader.Write("Language", "TaoistDes", TaoistDes);
            reader.Write("Language", "AssassinDes", AssassinDes);
            reader.Write("Language", "ArcherDes", ArcherDes);

            reader.Write("Language", "DateSent", DateSent);
            reader.Write("Language", "Send", Send);
            reader.Write("Language", "Reply", Reply);
            reader.Write("Language", "Read", Read);
            reader.Write("Language", "Delete", Delete);
            reader.Write("Language", "BlockList", BlockList);
            reader.Write("Language", "EnterMailToName", EnterMailToName);
            reader.Write("Language", "BeenPoisoned", BeenPoisoned);
            reader.Write("Language", "AddFriend", AddFriend);
            reader.Write("Language", "RemoveFriend", RemoveFriend);
            reader.Write("Language", "FriendMemo", FriendMemo);
            reader.Write("Language", "FriendMail", FriendMail);
            reader.Write("Language", "FriendWhisper", FriendWhisper);
            reader.Write("Language", "FriendEnterAddName", FriendEnterAddName);
            reader.Write("Language", "FriendEnterBlockName", FriendEnterBlockName);
            reader.Write("Language", "AddMentor", AddMentor);
            reader.Write("Language", "RemoveMentorMentee", RemoveMentorMentee);
            reader.Write("Language", "MentorRequests", MentorRequests);
            reader.Write("Language", "MentorEnterName", MentorEnterName);
            reader.Write("Language", "NoMentorship", NoMentorship);
            reader.Write("Language", "RestedBuff", RestedBuff);

            reader.Write("Language", "ItemTypeWeapon", ItemTypeWeapon);
            reader.Write("Language", "ItemTypeArmour", ItemTypeArmour);
            reader.Write("Language", "ItemTypeHelmet", ItemTypeHelmet);
            reader.Write("Language", "ItemTypeNecklace", ItemTypeNecklace);
            reader.Write("Language", "ItemTypeBracelet", ItemTypeBracelet);
            reader.Write("Language", "ItemTypeRing", ItemTypeRing);
            reader.Write("Language", "ItemTypeAmulet", ItemTypeAmulet);
            reader.Write("Language", "ItemTypeBelt", ItemTypeBelt);
            reader.Write("Language", "ItemTypeBoots", ItemTypeBoots);
            reader.Write("Language", "ItemTypeStone", ItemTypeStone);
            reader.Write("Language", "ItemTypeTorch", ItemTypeTorch);
            reader.Write("Language", "ItemTypePotion", ItemTypePotion);
            reader.Write("Language", "ItemTypeOre", ItemTypeOre);
            reader.Write("Language", "ItemTypeMeat", ItemTypeMeat);
            reader.Write("Language", "ItemTypeCraftingMaterial", ItemTypeCraftingMaterial);
            reader.Write("Language", "ItemTypeScroll", ItemTypeScroll);
            reader.Write("Language", "ItemTypeGem", ItemTypeGem);
            reader.Write("Language", "ItemTypeMount", ItemTypeMount);
            reader.Write("Language", "ItemTypeBook", ItemTypeBook);
            reader.Write("Language", "ItemTypeScript", ItemTypeScript);
            reader.Write("Language", "ItemTypeReins", ItemTypeReins);
            reader.Write("Language", "ItemTypeBells", ItemTypeBells);
            reader.Write("Language", "ItemTypeSaddle", ItemTypeSaddle);
            reader.Write("Language", "ItemTypeRibbon", ItemTypeRibbon);
            reader.Write("Language", "ItemTypeMask", ItemTypeMask);
            reader.Write("Language", "ItemTypeFood", ItemTypeFood);
            reader.Write("Language", "ItemTypeHook", ItemTypeHook);
            reader.Write("Language", "ItemTypeFloat", ItemTypeFloat);
            reader.Write("Language", "ItemTypeBait", ItemTypeBait);
            reader.Write("Language", "ItemTypeFinder", ItemTypeFinder);
            reader.Write("Language", "ItemTypeReel", ItemTypeReel);
            reader.Write("Language", "ItemTypeFish", ItemTypeFish);
            reader.Write("Language", "ItemTypeQuest", ItemTypeQuest);
            reader.Write("Language", "ItemTypeAwakening", ItemTypeAwakening);
            reader.Write("Language", "ItemTypePets", ItemTypePets);
            reader.Write("Language", "ItemTypeTransform", ItemTypeTransform);
            reader.Write("Language", "ItemTypeSealedHero", ItemTypeSealedHero);

            reader.Write("Language", "ItemGradeCommon", ItemGradeCommon);
            reader.Write("Language", "ItemGradeRare", ItemGradeRare);
            reader.Write("Language", "ItemGradeLegendary", ItemGradeLegendary);
            reader.Write("Language", "ItemGradeMythical", ItemGradeMythical);
            reader.Write("Language", "ItemGradeHeroic", ItemGradeHeroic);

            reader.Write("Language", "NoAccountID", NoAccountID);
            reader.Write("Language", "IncorrectPasswordAccountID", IncorrectPasswordAccountID);
            reader.Write("Language", "GroupSwitch", GroupSwitch);
            reader.Write("Language", "GroupAdd", GroupAdd);
            reader.Write("Language", "GroupRemove", GroupRemove);
            reader.Write("Language", "GroupAddEnterName", GroupAddEnterName);
            reader.Write("Language", "GroupRemoveEnterName", GroupRemoveEnterName);
            reader.Write("Language", "TooHeavyToHold", TooHeavyToHold);
            reader.Write("Language", "SwitchMarriage", SwitchMarriage);
            reader.Write("Language", "RequestMarriage", RequestMarriage);
            reader.Write("Language", "RequestDivorce", RequestDivorce);
            reader.Write("Language", "MailLover", MailLover);
            reader.Write("Language", "WhisperLover", WhisperLover);
        }


        public static void LoadServerLanguage(string languageIniPath) {
            if(!File.Exists(languageIniPath)) {
                SaveServerLanguage(languageIniPath);
                return;
            }

            InIReader reader = new(languageIniPath);
            Welcome = reader.ReadString("Language", "Welcome", Welcome);
            OnlinePlayers = reader.ReadString("Language", "OnlinePlayers", OnlinePlayers);
            LowLevel = reader.ReadString("Language", "LowLevel", LowLevel);
            LowGold = reader.ReadString("Language", "LowGold", LowGold);
            LowDC = reader.ReadString("Language", "LowDC", LowDC);
            LowMC = reader.ReadString("Language", "LowMC", LowMC);
            LowSC = reader.ReadString("Language", "LowSC", LowSC);

            LevelUp = reader.ReadString("Language", "LevelUp", LevelUp);

            WeaponLuck = reader.ReadString("Language", "WeaponLuck", WeaponLuck);
            WeaponCurse = reader.ReadString("Language", "WeaponCurse", WeaponCurse);
            WeaponNoEffect = reader.ReadString("Language", "WeaponNoEffect", WeaponNoEffect);

            InventoryIncreased = reader.ReadString("Language", "InventoryIncreased", InventoryIncreased);
            ExpandedStorageExpiresOn =
                reader.ReadString("Language", "ExpandedStorageExpiresOn", ExpandedStorageExpiresOn);
            GameName = reader.ReadString("Language", "GameName", GameName);
            FaceToTrade = reader.ReadString("Language", "FaceToTrade", FaceToTrade);
            NoTownTeleport = reader.ReadString("Language", "NoTownTeleport", NoTownTeleport);
            CanNotRandom = reader.ReadString("Language", "CanNotRandom", CanNotRandom);
            CanNotDungeon = reader.ReadString("Language", "CanNotDungeon", CanNotDungeon);
            CannotResurrection = reader.ReadString("Language", "CannotResurrection", CannotResurrection);
            CanNotDrop = reader.ReadString("Language", "CanNotDrop", CanNotDrop);

            NotFemale = reader.ReadString("Language", "NotFemale", NotFemale);
            NotMale = reader.ReadString("Language", "NotMale", NotMale);
            NotInGuild = reader.ReadString("Language", "NotInGuild", NotInGuild);
            NewMail = reader.ReadString("Language", "NewMail", NewMail);
            CouldNotFindPlayer = reader.ReadString("Language", "CouldNotFindPlayer", CouldNotFindPlayer);
            NoMentorship = reader.ReadString("Language", "NoMentorship", NoMentorship);
            NoBagSpace = reader.ReadString("Language", "NoBagSpace", NoBagSpace);
            AllowingMentorRequests = reader.ReadString("Language", "AllowingMentorRequests", AllowingMentorRequests);
            BlockingMentorRequests = reader.ReadString("Language", "BlockingMentorRequests", BlockingMentorRequests);
        }

        public static void SaveServerLanguage(string languageIniPath) {
            File.Delete(languageIniPath);
            InIReader reader = new(languageIniPath);
            reader.Write("Language", "Welcome", Welcome);
            reader.Write("Language", "OnlinePlayers", OnlinePlayers);
            reader.Write("Language", "LowLevel", LowLevel);
            reader.Write("Language", "LowGold", LowGold);
            reader.Write("Language", "LowDC", LowDC);
            reader.Write("Language", "LowMC", LowMC);
            reader.Write("Language", "LowSC", LowSC);

            reader.Write("Language", "LevelUp", LevelUp);

            reader.Write("Language", "WeaponLuck", WeaponLuck);
            reader.Write("Language", "WeaponCurse", WeaponCurse);
            reader.Write("Language", "WeaponNoEffect", WeaponNoEffect);

            reader.Write("Language", "InventoryIncreased", InventoryIncreased);
            reader.Write("Language", "ExpandedStorageExpiresOn", ExpandedStorageExpiresOn);
            reader.Write("Language", "GameName", GameName);
            reader.Write("Language", "FaceToTrade", FaceToTrade);
            reader.Write("Language", "NoTownTeleport", NoTownTeleport);
            reader.Write("Language", "CanNotRandom", CanNotRandom);
            reader.Write("Language", "CanNotDungeon", CanNotDungeon);
            reader.Write("Language", "CannotResurrection", CannotResurrection);
            reader.Write("Language", "CanNotDrop", CanNotDrop);

            reader.Write("Language", "NotFemale", NotFemale);
            reader.Write("Language", "NotMale", NotMale);
            reader.Write("Language", "NotInGuild", NotInGuild);
            reader.Write("Language", "NewMail", NewMail);
            reader.Write("Language", "CouldNotFindPlayer", CouldNotFindPlayer);
            reader.Write("Language", "NoMentorship", NoMentorship);
            reader.Write("Language", "NoBagSpace", NoBagSpace);
            reader.Write("Language", "AllowingMentorRequests", AllowingMentorRequests);
            reader.Write("Language", "BlockingMentorRequests", BlockingMentorRequests);
        }
    }
}
