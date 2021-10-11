using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Linq;

namespace WymTool
{
    public class ConfigManager
    {
        const string IP4_JK = "172.16.10.116";
        const string IP4_TOSHIBA = "192.168.8.10";
        public bool IsDebug = true;
        public string PathConfig;
        private Dictionary<string, string> config = new Dictionary<string, string>();// Language配置用到的脚本比较多，调试阶段最好用绝对目录
        private static ConfigManager _instance = null;
        private static object _instanceLock = new object();
        public static ConfigManager Instance()
        {
            if (_instance == null) //双if +lock
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigManager();
                    }
                }
                _instance.init();
            }
            return _instance;
        }

        public const string STR_WEAPON = "武器";//"Weapon";
        public const string STR_WEAPONS = "武器s";//"Weapons";
        public const string STR_ARMOUR = "Armour";//"Armour";
        public const string STR_ARMOURS = "Armours";//"Armours";
        public const string STR_HELMET = "Helmet";//"Helmet";
        public const string STR_HELMETS = "Helmets";//"Helmets";
        public const string STR_NECKLACE = "Necklace";//"Necklace";
        public const string STR_BRACELET = "Bracelet";//"Bracelet";
        public const string STR_RING = "Ring";//"Ring";
        public const string STR_AMULET = "Amulet";//"Amulet";
        public const string STR_BELT = "Belt";//"Belt";
        public const string STR_BELTS = "Belts";//"Belts";
        public const string STR_BOOT = "Boot";//"Boot";
        public const string STR_BOOTS = "Boots";//"Boots";
        public const string STR_STONE = "Stone";//"Stone";
        public const string STR_TORCH = "Torch";//"Torch";
        public const string STR_POTION = "Potion";//"Potion";
        public const string STR_ORE = "Ore";//"Ore";
        public const string STR_MEAT = "Meat";//"Meat";
        public const string STR_CRAFTINGMATERIAL = "CraftingMaterial";//"CraftingMaterial";
        public const string STR_CRAFTING_MATERIAL = "Crafting Material";//"Crafting Material";
        public const string STR_SCROLL = "Scroll";//"Scroll";
        public const string STR_GEM = "Gem";//"Gem";
        public const string STR_MOUNT = "Mount";//"Mount";
        public const string STR_BOOK = "Book";//"Book";
        public const string STR_NOTHING = "无";//"Nothing";
        public const string STR_SCRIPT = "Script";//"Script";
        public const string STR_REINS = "Reins";//"Reins";
        public const string STR_BELLS = "Bells";//"Bells";
        public const string STR_SADDLE = "Saddle";//"Saddle";
        public const string STR_RIBBON = "Ribbon";//"Ribbon";
        public const string STR_MASK = "Mask";//"Mask";
        public const string STR_FOOD = "Food";//"Food";
        public const string STR_HOOK = "Hook";//"Hook";
        public const string STR_FLOAT = "Float";//"Float";
        public const string STR_BAIT = "Bait";//"Bait";
        public const string STR_FINDER = "Finder";//"Finder";
        public const string STR_REEL = "Reel";//"Reel";
        public const string STR_FISH = "Fish";//"Fish";
        public const string STR_QUEST = "Quest";//"Quest";
        public const string STR_AWAKENING = "Awakening";//"Awakening";
        public const string STR_PETS = "Pets";//"Pets";
        public const string STR_TRANSFORM = "Transform";//"Transform";
        public const string STR_JEWELRY = "Jewelry";//"Jewelry";
        public const string STR_OTHERS = "Others";//"Others";

        public const string STR_SHOWALL = "Show All";//"Show All";
        public const string STR_TOPITEMS = "TopItems";//"TopItems";
        public const string STR_NEWITEMS = "NewItems";//"NewItems";
        public const string STR_DEALITEMS = "DealItems";//"DealItems";

        public const string STR_ALL_CLASSES = "All Classes";//"All Classes";
        public const string STR_WARRIOR = "战士";//"Warrior";
        public const string STR_WIZARD = "法师";//"Wizard";
        public const string STR_TAOIST = "道士";//"Taoist";
        public const string STR_ASSASSIN = "刺客";//"Assassin";
        public const string STR_ARCHER = "弓箭手";//"Archer";
        public const string STR_ALL = "All";//"All";

        public const string STR_SHOW_ALL_ITEMS = "Show All Items";//"Show All Items";
        public const string STR_WEAPON_ITEMS = "Weapon Items";//"Weapon Items";
        public const string STR_DRAPERY_ITEMS = "Drapery Items";//"Drapery Items";
        public const string STR_ACCESSORY_ITEMS = "Accessory Items";//"Accessory Items";
        public const string STR_CONSUMABLE_ITEMS = "Consumable Items";//"Consumable Items";
        public const string STR_ENHANCEMENT = "Enhancement";//"Enhancement";
        public const string STR_BOOKS = "Books";//"Books";
        public const string STR_CRAFT_ITEMS = "Craft Items";//"Craft Items";

        public const string STR_NECKLACES = "Necklaces";//"Necklaces";
        public const string STR_BRACELETS = "Bracelets";//"Bracelets";
        public const string STR_RINGS = "Rings";//"Rings";
        public const string STR_RECOVERY_POTS = "Recovery Pots";//"Recovery Pots";
        public const string STR_BUFF_POTS = "Buff Pots";//"Buff Pots";
        public const string STR_SCROLLS_OILS = "Scrolls / Oils";//"Scrolls / Oils";
        public const string STR_MISC_ITEMS = "Misc Items";//"Misc Items";
        public const string STR_GEMS = "Gems";//"Gems";
        public const string STR_ORBS = "Orbs";//"Orbs";

        public const string STR_MATERIALS = "Materials";//"Materials";

        public const string STR_SOLD = "Sold";//"Sold";
        public const string STR_EXPIRED = "Expired";//"Expired";
        public const string STR_FOR_SALE = "For Sale";//"For Sale";
        public const string STR_BID_MET = "Bid Met";//"Bid Met";
        public const string STR_NO_BID = "No Bid";//"No Bid";

        public const string STR_CREDITS = "Credits";//"Credits";
        public const string STR_GOLD = "Gold";//"Gold";

        public const string STR_ALL_ITEMS = "All Items";//"All Items";
        public const string STR_ALL_CATEGORIES = "All Categories";//"All Categories";
        public const string STR_TOP_ITEMS = "Top Items";//"Top Items";
        public const string STR_SALE_ITEMS = "Sale Items";//"Sale Items";
        public const string STR_NEW_ITEMS = "New Items";//"New Items";

        private void init()
        {
            string filePath;
            switch (GetHostIP4())
            {
                case IP4_JK:
                    filePath = "F:/Mir2Suprcode/mir2/WymTool/Config.txt";
                    break;
                case IP4_TOSHIBA:
                    filePath = "D:/Projects/mir2suprcode/mir2/WymTool/Config.txt";
                    break;
                default:
                    filePath = "";
                    LogManager.Instance().Log("找不到此IP对应的设备.");
                    break;
            }

            string[] arr = File.ReadAllLines(filePath);
            foreach (string e in arr)
            {
                string[] v = e.Split('\t');
                config.Add(v[0], v[1]);
            }

            switch (GetHostIP4())
            {
                case IP4_JK:
                    PathConfig = config["wangyimin_jk"];
                    break;
                case IP4_TOSHIBA:
                    PathConfig = config["wangyimin_toshiba"];
                    break;
                default:
                    PathConfig = "";
                    LogManager.Instance().Log("找不到此IP对应的设备.");
                    break;
            }
        }

        public string GetHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// 返回IPv4的地址
        /// </summary>
        /// <returns></returns>
        public string GetHostIP4()
        {
            return Dns.GetHostAddresses(GetHostName()).Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToString();
        }

        private string GetFileJson(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }
    }
}
