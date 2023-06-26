namespace Client
{

    public enum KeybindOptions : int
    {
        Bar1Skill1 = 0,
        Bar1Skill2,
        Bar1Skill3,
        Bar1Skill4,
        Bar1Skill5,
        Bar1Skill6,
        Bar1Skill7,
        Bar1Skill8,
        Bar2Skill1,
        Bar2Skill2,
        Bar2Skill3,
        Bar2Skill4,
        Bar2Skill5,
        Bar2Skill6,
        Bar2Skill7,
        Bar2Skill8,
        Inventory,
        Inventory2,
        Equipment,
        Equipment2,
        Skills,
        Skills2,
        Creature,
        MountWindow,
        Mount,
        Fishing,
        Skillbar,
        Mentor,
        Relationship,
        Friends,
        Guilds,
        GameShop,
        Quests,
        Closeall,
        Options,
        Options2,
        Group,
        Belt,
        BeltFlip,
        Pickup,
        Belt1,
        Belt1Alt,
        Belt2,
        Belt2Alt,
        Belt3,
        Belt3Alt,
        Belt4,
        Belt4Alt,
        Belt5,
        Belt5Alt,
        Belt6,
        Belt6Alt,
        Belt7,
        Belt7Alt,
        Belt8,
        Belt8Alt,
        Logout,
        Exit,
        CreaturePickup,
        CreatureAutoPickup,
        Minimap,
        Bigmap,
        Trade,
        Rental,
        ChangeAttackmode,
        AttackmodePeace,
        AttackmodeGroup,
        AttackmodeGuild,
        AttackmodeEnemyguild,
        AttackmodeRedbrown,
        AttackmodeAll,
        ChangePetmode,
        PetmodeBoth,
        PetmodeMoveonly,
        PetmodeAttackonly,
        PetmodeNone,
        Help,
        Keybind,
        Autorun,
        Cameramode,
        Screenshot,
        DropView,
        TargetDead,
        Ranking,
        AddGroupMember,
        HeroSkill1,
        HeroSkill2,
        HeroSkill3,
        HeroSkill4,
        HeroSkill5,
        HeroSkill6,
        HeroSkill7,
        HeroSkill8,
        HeroInventory,
        HeroEquipment,
        HeroSkills,
        TargetSpellLockOn,
        PetmodeFocusMasterTarget
    }

    public class KeyBind
    {
        public KeybindOptions function = KeybindOptions.Bar1Skill1;
        public string Group = "", Description = "";
        public Keys Key = 0;

        /// <summary>
        /// Require Options : 0 = Require unpressed key, 1 = Require pressed key, 2 = Don't care
        /// </summary>
        public byte RequireCtrl = 0;
        public byte RequireShift = 0;
        public byte RequireAlt = 0;
        public byte RequireTilde = 0;
    }


    public class KeyBindSettings
    {
        private static InIReader Reader = new InIReader(@".\KeyBinds.ini");
        public List<KeyBind> Keylist = new List<KeyBind>();
        public List<KeyBind> DefaultKeylist = new List<KeyBind>();

        public KeyBindSettings()
        {
            New(Keylist);
            New(DefaultKeylist);

            if (!File.Exists(@".\KeyBinds.ini"))
            {
                Save(DefaultKeylist);
                return;
            }

            Load();
        }

        public void Load()
        {
            foreach (KeyBind Inputkey in Keylist)
            {
                Inputkey.RequireAlt = Reader.ReadByte(Inputkey.function.ToString(), "RequireAlt", Inputkey.RequireAlt);
                Inputkey.RequireShift = Reader.ReadByte(Inputkey.function.ToString(), "RequireShift", Inputkey.RequireShift);
                Inputkey.RequireTilde = Reader.ReadByte(Inputkey.function.ToString(), "RequireTilde", Inputkey.RequireTilde);
                Inputkey.RequireCtrl = Reader.ReadByte(Inputkey.function.ToString(), "RequireCtrl", Inputkey.RequireCtrl);
                string Input = Reader.ReadString(Inputkey.function.ToString(), "RequireKey", Inputkey.Key.ToString());
                Enum.TryParse(Input, out Inputkey.Key);
                
            }
        }

        public void Save(List<KeyBind> keyList)
        {
            Reader.Write("Guide", "01", "RequireAlt,RequireShift,RequireTilde,RequireCtrl");
            Reader.Write("Guide", "02", "have 3 options: 0/1/2");
            Reader.Write("Guide", "03", "0 < you cannot have this key pressed to use the function");
            Reader.Write("Guide", "04", "1 < you have to have this key pressed to use this function");
            Reader.Write("Guide", "05", "2 < it doesnt matter if you press this key to use this function");
            Reader.Write("Guide", "06", "by default just use 2, unless you have 2 functions on the same key");
            Reader.Write("Guide", "07", "example: change attack mode (ctrl+h) and help (h)");
            Reader.Write("Guide", "08", "if you set either of those to requireshift 2, then they wil both work at the same time or not work");
            Reader.Write("Guide", "09", "");
            Reader.Write("Guide", "10", "To get the value for RequireKey look at:");
            Reader.Write("Guide", "11", "https://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx");
        
            foreach (KeyBind Inputkey in keyList)
            {
                Reader.Write(Inputkey.function.ToString(), "RequireAlt", Inputkey.RequireAlt);
                Reader.Write(Inputkey.function.ToString(), "RequireShift", Inputkey.RequireShift);
                Reader.Write(Inputkey.function.ToString(), "RequireTilde", Inputkey.RequireTilde);
                Reader.Write(Inputkey.function.ToString(), "RequireCtrl", Inputkey.RequireCtrl);
                Reader.Write(Inputkey.function.ToString(), "RequireKey", Inputkey.Key.ToString());
            }
        }

        public void New(List<KeyBind> list)
        {
            KeyBind InputKey;
            InputKey = new KeyBind { Group = "Dialogs", Description = "Inventory Open/Close", function = KeybindOptions.Inventory, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F9 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Inventory Open/Close Alt", function = KeybindOptions.Inventory2, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 0, Key = Keys.I };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Equipment Open/Close", function = KeybindOptions.Equipment, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F10 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Equipment Open/Close Alt", function = KeybindOptions.Equipment2, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 0, Key = Keys.C };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Skills Open/Close", function = KeybindOptions.Skills, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F11 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Skills Open/Close Alt", function = KeybindOptions.Skills2, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 0, Key = Keys.S };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Hero Inventory Open/Close", function = KeybindOptions.HeroInventory, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 1, Key = Keys.I };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Hero Equipment Open/Close", function = KeybindOptions.HeroEquipment, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 1, Key = Keys.C };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Hero Skills Open/Close", function = KeybindOptions.HeroSkills, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 1, Key = Keys.S };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Creatures Open/Close", function = KeybindOptions.Creature, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.E };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Mount Open/Close", function = KeybindOptions.MountWindow, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.J };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Fishing Open/Close", function = KeybindOptions.Fishing, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.N };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Skillbar Open/Close", function = KeybindOptions.Skillbar, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.R };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Mentor Open/Close", function = KeybindOptions.Mentor, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Relationship Open/Close", function = KeybindOptions.Relationship, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.L };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Friends Open/Close", function = KeybindOptions.Friends, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Guild Open/Close", function = KeybindOptions.Guilds, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 0, Key = Keys.G };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Gameshop Open/Close", function = KeybindOptions.GameShop, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Y };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Quest Diary Open/Close", function = KeybindOptions.Quests, RequireAlt = 0, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Q };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Options Open/Close", function = KeybindOptions.Options, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F12 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Options Open/Close Alt", function = KeybindOptions.Options2, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.O };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Group Open/Close", function = KeybindOptions.Group, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.P };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Belt Open/Close", function = KeybindOptions.Belt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 0, Key = Keys.Z };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Minimap Open/Close", function = KeybindOptions.Minimap, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.V };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Bigmap Open/Close", function = KeybindOptions.Bigmap, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.B };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Ranking Open/Close", function = KeybindOptions.Ranking, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.K };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Help Open/Close", function = KeybindOptions.Help, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 0, Key = Keys.H };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Keybinds Open/Close", function = KeybindOptions.Keybind, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.U };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Dialogs", Description = "Close All Windows", function = KeybindOptions.Closeall, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Escape };
            list.Add(InputKey);

            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Slot 1", function = KeybindOptions.Bar1Skill1, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F1 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Slot 2", function = KeybindOptions.Bar1Skill2, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F2 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Slot 3", function = KeybindOptions.Bar1Skill3, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F3 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Slot 4", function = KeybindOptions.Bar1Skill4, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F4 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Slot 5", function = KeybindOptions.Bar1Skill5, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F5 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Slot 6", function = KeybindOptions.Bar1Skill6, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F6 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Slot 7", function = KeybindOptions.Bar1Skill7, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F7 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Slot 8", function = KeybindOptions.Bar1Skill8, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F8 };
            list.Add(InputKey);

            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Alt Slot 1", function = KeybindOptions.Bar2Skill1, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F1 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Alt Slot 2", function = KeybindOptions.Bar2Skill2, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F2 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Alt Slot 3", function = KeybindOptions.Bar2Skill3, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F3 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Alt Slot 4", function = KeybindOptions.Bar2Skill4, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F4 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Alt Slot 5", function = KeybindOptions.Bar2Skill5, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F5 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Alt Slot 6", function = KeybindOptions.Bar2Skill6, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F6 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Alt Slot 7", function = KeybindOptions.Bar2Skill7, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F7 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Skillbar Alt Slot 8", function = KeybindOptions.Bar2Skill8, RequireAlt = 2, RequireShift = 0, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F8 };
            list.Add(InputKey);

            InputKey = new KeyBind { Group = "Skillbar", Description = "Hero Skillbar Slot 1", function = KeybindOptions.HeroSkill1, RequireAlt = 2, RequireShift = 1, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F1 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Hero Skillbar Slot 2", function = KeybindOptions.HeroSkill2, RequireAlt = 2, RequireShift = 1, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F2 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Hero Skillbar Slot 3", function = KeybindOptions.HeroSkill3, RequireAlt = 2, RequireShift = 1, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F3 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Hero Skillbar Slot 4", function = KeybindOptions.HeroSkill4, RequireAlt = 2, RequireShift = 1, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F4 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Hero Skillbar Slot 5", function = KeybindOptions.HeroSkill5, RequireAlt = 2, RequireShift = 1, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F5 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Hero Skillbar Slot 6", function = KeybindOptions.HeroSkill6, RequireAlt = 2, RequireShift = 1, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F6 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Hero Skillbar Slot 7", function = KeybindOptions.HeroSkill7, RequireAlt = 2, RequireShift = 1, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F7 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Skillbar", Description = "Hero Skillbar Slot 8", function = KeybindOptions.HeroSkill8, RequireAlt = 2, RequireShift = 1, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F8 };
            list.Add(InputKey);

            InputKey = new KeyBind { Group = "Belt", Description = "Rotate Belt", function = KeybindOptions.BeltFlip, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 1, Key = Keys.Z };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 1", function = KeybindOptions.Belt1, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D1 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 1 Alt", function = KeybindOptions.Belt1Alt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad1 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 2", function = KeybindOptions.Belt2, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D2 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 2 Alt", function = KeybindOptions.Belt2Alt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad2 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 3", function = KeybindOptions.Belt3, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D3 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 3 Alt", function = KeybindOptions.Belt3Alt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad3 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 4", function = KeybindOptions.Belt4, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D4 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 4 Alt", function = KeybindOptions.Belt4Alt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad4 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 5", function = KeybindOptions.Belt5, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D5 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 5 Alt", function = KeybindOptions.Belt5Alt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad5 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 6", function = KeybindOptions.Belt6, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D6 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 6 Alt", function = KeybindOptions.Belt6Alt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad6 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 7", function = KeybindOptions.Belt7, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D7 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 7 Alt", function = KeybindOptions.Belt7Alt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad7 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 8", function = KeybindOptions.Belt8, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D8 };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Belt", Description = "Belt Slot 8 Alt", function = KeybindOptions.Belt8Alt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad8 };
            list.Add(InputKey);

            InputKey = new KeyBind { Group = "General", Description = "Logout", function = KeybindOptions.Logout, RequireAlt = 1, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.X };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "General", Description = "Exit", function = KeybindOptions.Exit, RequireAlt = 1, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Q };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "General", Description = "Mount/Dismount", function = KeybindOptions.Mount, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.M };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "General", Description = "Pickup Floor Item", function = KeybindOptions.Pickup, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Tab };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "General", Description = "Creature Item Pickup", function = KeybindOptions.CreaturePickup, RequireAlt = 0, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.X };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "General", Description = "Creature Auto Pickup", function = KeybindOptions.CreatureAutoPickup, RequireAlt = 1, RequireShift = 2, RequireTilde = 2, RequireCtrl = 0, Key = Keys.A };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "General", Description = "Request Trade", function = KeybindOptions.Trade, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.T };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "General", Description = "Recruit Group Member", function = KeybindOptions.AddGroupMember, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 1, Key = Keys.G };
            list.Add(InputKey);

            InputKey = new KeyBind { Group = "Toggle", Description = "Toggle Attack Mode", function = KeybindOptions.ChangeAttackmode, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 1, Key = Keys.H };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Attack Mode : Peace", function = KeybindOptions.AttackmodePeace, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Attack Mode : Group", function = KeybindOptions.AttackmodeGroup, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Attack Mode : Guild", function = KeybindOptions.AttackmodeGuild, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Attack Mode : Enemy Guild", function = KeybindOptions.AttackmodeEnemyguild, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Attack Mode : Red/Brown", function = KeybindOptions.AttackmodeRedbrown, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Attack Mode : All", function = KeybindOptions.AttackmodeAll, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Toggle Pet Mode", function = KeybindOptions.ChangePetmode, RequireAlt = 0, RequireShift = 2, RequireTilde = 2, RequireCtrl = 1, Key = Keys.A };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Pet Mode : Both", function = KeybindOptions.PetmodeBoth, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Pet Mode : Move Only", function = KeybindOptions.PetmodeMoveonly, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Pet Mode : Attack Only", function = KeybindOptions.PetmodeAttackonly, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Pet Mode : None", function = KeybindOptions.PetmodeNone, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Set Pet Mode : Focus Master Target", function = KeybindOptions.PetmodeFocusMasterTarget, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Toggle Autorun", function = KeybindOptions.Autorun, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Toggle Camera Mode", function = KeybindOptions.Cameramode, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Insert };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Take Screenshot", function = KeybindOptions.Screenshot, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.PrintScreen };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Toggle", Description = "Toggle Dropview", function = KeybindOptions.DropView, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Tab };
            list.Add(InputKey);
            InputKey = new KeyBind { Group = "Combat", Description = "Hold to enable target spell lock-on", function = KeybindOptions.TargetSpellLockOn, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            list.Add(InputKey);
        }

        public string GetKey(KeybindOptions Option, bool defaultKey = false)
        {
            List<KeyBind> lst;

            if (defaultKey) lst = CMain.InputKeys.DefaultKeylist;
            else lst = CMain.InputKeys.Keylist;

            string output = "";
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].function == Option)
                {
                    if (lst[i].Key == Keys.None) return output;
                    if (lst[i].RequireAlt == 1)
                        output = "Alt";
                    if (lst[i].RequireCtrl == 1)
                        output = output != "" ? output + " + Ctrl" : "Ctrl";
                    if (lst[i].RequireShift == 1)
                        output = output != "" ? output + " + Shift" : "Shift";
                    if (lst[i].RequireTilde == 1)
                        output = output != "" ? output + " + ~" : "~";

                    output = output != "" ? output + " + " + lst[i].Key.ToString() : lst[i].Key.ToString();
                    return output;
                }
            }
            return "";
        }
    }

    
}
