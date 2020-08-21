using System.IO;
using System;
using Client.MirSounds;
using System.Windows.Forms;
using System.Collections.Generic;


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
        Autorun,
        Cameramode,
        Screenshot,
        DropView,
        TargetDead,
        Ranking,
        AddGroupMember
    }

    public class KeyBind
    {
        public KeybindOptions function = KeybindOptions.Bar1Skill1;
        public byte RequireCtrl = 0; //so these requirexxx: 0 < only works if you DONT hold the key, 1 < only works if you HOLD the key, 2 < works REGARDLESSS of the key
        public byte RequireShift = 0;
        public byte RequireAlt = 0;
        public byte RequireTilde = 0;
        public Keys Key = 0;
    }


    public class KeyBindSettings
    {
        private static InIReader Reader = new InIReader(@".\KeyBinds.ini");
        public List<KeyBind> Keylist = new List<KeyBind>();
        public KeyBindSettings()
        {
            New();
            if (!File.Exists(@".\KeyBinds.ini"))
            {
                Save();
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

        public void Save()
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
        
            foreach (KeyBind Inputkey in Keylist)
            {
                Reader.Write(Inputkey.function.ToString(), "RequireAlt", Inputkey.RequireAlt);
                Reader.Write(Inputkey.function.ToString(), "RequireShift", Inputkey.RequireShift);
                Reader.Write(Inputkey.function.ToString(), "RequireTilde", Inputkey.RequireTilde);
                Reader.Write(Inputkey.function.ToString(), "RequireCtrl", Inputkey.RequireCtrl);
                Reader.Write(Inputkey.function.ToString(), "RequireKey", Inputkey.Key.ToString());
            }
        }

        public void New()
        {
            KeyBind InputKey;
            InputKey = new KeyBind{ function = KeybindOptions.Bar1Skill1, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F1 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar1Skill2, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F2 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar1Skill3, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F3 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar1Skill4, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F4 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar1Skill5, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F5 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar1Skill6, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F6 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar1Skill7, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F7 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar1Skill8, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 0, Key = Keys.F8 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar2Skill1, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F1 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar2Skill2, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F2 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar2Skill3, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F3 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar2Skill4, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F4 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar2Skill5, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F5 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar2Skill6, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F6 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar2Skill7, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F7 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bar2Skill8, RequireAlt = 2, RequireShift = 2, RequireTilde = 0, RequireCtrl = 1, Key = Keys.F8 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Inventory, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F9 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Inventory2, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.I };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Equipment, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F10 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Equipment2, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.C };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Skills, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F11 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Skills2, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.S };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Creature, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.E };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.MountWindow, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.J };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Mount, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.M };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Fishing, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.N };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Skillbar, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.R };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Mentor, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.W };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Relationship, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.L };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Friends, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Guilds, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 0, Key = Keys.G };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.GameShop, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Y };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Quests, RequireAlt = 0, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Q };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Closeall, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Escape };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Options, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.F12 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Options2, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.O };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Group, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.P };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 0, Key = Keys.Z };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.BeltFlip, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 1, Key = Keys.Z };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Pickup, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Tab };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt1, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D1 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt1Alt, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad1 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt2, RequireAlt = 0, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D2 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt2Alt, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad2 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt3, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D3 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt3Alt, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad3 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt4, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D4 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt4Alt, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad4 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt5, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D5 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt5Alt, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad5 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt6, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D6 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Belt6Alt, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.NumPad6 };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Logout, RequireAlt = 1, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.X };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Exit, RequireAlt = 1, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Q };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.CreaturePickup, RequireAlt = 0, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.X };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.CreatureAutoPickup, RequireAlt = 1, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.A };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Minimap, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.V };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Bigmap, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.B };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Trade, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.T };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Rental, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 0, Key = Keys.A };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.ChangeAttackmode, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 1, Key = Keys.H };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.AttackmodePeace, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None};
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.AttackmodeGroup, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.AttackmodeGuild, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.AttackmodeEnemyguild, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.AttackmodeRedbrown, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.AttackmodeAll, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.ChangePetmode, RequireAlt = 0, RequireShift = 0, RequireTilde = 2, RequireCtrl = 1, Key = Keys.A };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.PetmodeBoth, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.PetmodeMoveonly, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.PetmodeAttackonly, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.PetmodeNone, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.None};
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Help, RequireAlt = 2, RequireShift = 0, RequireTilde = 2, RequireCtrl = 2, Key = Keys.H };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Autorun, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.D };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Cameramode, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Insert };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Screenshot, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.PrintScreen };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.DropView, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.Tab };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.TargetDead, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 1, Key = Keys.ControlKey };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.Ranking, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 2, Key = Keys.K };
            Keylist.Add(InputKey);
            InputKey = new KeyBind { function = KeybindOptions.AddGroupMember, RequireAlt = 2, RequireShift = 2, RequireTilde = 2, RequireCtrl = 1, Key = Keys.G };
            Keylist.Add(InputKey);
        }
        public string GetKey(KeybindOptions Option)
        {
            string output = "";
            for (int i = 0; i < Keylist.Count; i++ )
            {
                if (Keylist[i].function == Option)
                {
                    if (CMain.InputKeys.Keylist[i].Key == Keys.None) return output;
                    if (CMain.InputKeys.Keylist[i].RequireAlt == 1)
                        output = "Alt";
                    if (CMain.InputKeys.Keylist[i].RequireCtrl == 1)
                        output = output != "" ? output + "\nCtrl" : "Ctrl";
                    if (CMain.InputKeys.Keylist[i].RequireShift == 1)
                        output = output != "" ? output + "\nShift" : "Shift";
                    if (CMain.InputKeys.Keylist[i].RequireTilde == 1)
                        output = output != "" ? output + "\n~" : "~";

                    output = output != "" ? output + "\n" + CMain.InputKeys.Keylist[i].Key.ToString() : CMain.InputKeys.Keylist[i].Key.ToString();
                    return output;
                }
            }
            return "";
        }
    }

    
}
