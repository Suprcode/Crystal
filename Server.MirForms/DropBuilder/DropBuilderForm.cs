using Server.MirEnvir;

namespace Server.MirForms.DropBuilder
{
    public class MonsterDropInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public partial class DropGenForm : Form
    {
        string Gold = "0", GoldOdds;

        List<DropItem>
            Weapon = new List<DropItem>(),
            Armour = new List<DropItem>(),
            Helmet = new List<DropItem>(),
            Necklace = new List<DropItem>(),
            Bracelet = new List<DropItem>(),
            Ring = new List<DropItem>(),
            Amulet = new List<DropItem>(),
            Belt = new List<DropItem>(),
            Boot = new List<DropItem>(),
            Stone = new List<DropItem>(),
            Torch = new List<DropItem>(),
            Potion = new List<DropItem>(),
            Ore = new List<DropItem>(),
            Meat = new List<DropItem>(),
            CraftingMaterial = new List<DropItem>(),
            Scrolls = new List<DropItem>(),
            Gem = new List<DropItem>(),
            Mount = new List<DropItem>(),
            Book = new List<DropItem>(),
            Nothing = new List<DropItem>(),
            Script = new List<DropItem>(),
            Reins = new List<DropItem>(),
            Bells = new List<DropItem>(),
            Saddle = new List<DropItem>(),
            Ribbon = new List<DropItem>(),
            Mask = new List<DropItem>(),
            Food = new List<DropItem>(),
            Hook = new List<DropItem>(),
            Float = new List<DropItem>(),
            Bait = new List<DropItem>(),
            Finder = new List<DropItem>(),
            Reel = new List<DropItem>(),
            Fish = new List<DropItem>(),
            Quest = new List<DropItem>(),
            Awakening = new List<DropItem>(),
            Pets = new List<DropItem>(),
            Transform = new List<DropItem>();

        List<DropItem>[] ItemLists;
        ListBox[] ItemListBoxes;

        static readonly string[] TabTags =
        {
            "Weapon", "Armour", "Helmet", "Necklace", "Bracelet", "Ring", "Amulet", "Belt", "Boots",
            "Stone", "Torch", "Potion", "Ore", "Meat", "CraftingMaterial", "Scroll", "Gem", "Mount",
            "Book", "Nothing", "Script", "Reins", "Bells", "Saddle", "Ribbon", "Mask", "Food", "Hook",
            "Float", "Bait", "Finder", "Reel", "Fish", "Quest", "Awakening", "Pets", "Transform"
        };

        static readonly string[] CanonicalHeaders =
        {
            ";Weapons", ";Armours", ";Helmets", ";Necklaces", ";Bracelets", ";Rings", ";Amulets", ";Belts", ";Boots",
            ";Stones", ";Torches", ";Potions", ";Ores", ";Meat", ";Crafting Materials", ";Scrolls", ";Gems", ";Mount",
            ";Books", ";Nothing", ";Script", ";Reins", ";Bells", ";Saddle", ";Ribbon", ";Mask", ";Food", ";Hook",
            ";Float", ";Bait", ";Finder", ";Reel", ";Fish", ";Quest", ";Awakening", ";Pets", ";Transform"
        };

        static readonly string[][] HeaderAliases =
        {
            new[] { "weapons", "weapon" },
            new[] { "armours", "armour", "armors", "armor" },
            new[] { "helmets", "helmet" },
            new[] { "necklaces", "necklace" },
            new[] { "bracelets", "bracelet" },
            new[] { "rings", "ring" },
            new[] { "amulets", "amulet" },
            new[] { "belts", "belt" },
            new[] { "boots", "boot", "shoes", "shoe" },
            new[] { "stones", "stone" },
            new[] { "torches", "torch" },
            new[] { "potions", "potion", "pots + other", "pots", "pot" },
            new[] { "ores", "ore" },
            new[] { "meat", "meats" },
            new[] { "crafting materials", "crafting material", "craftingmaterials", "craftingmaterial", "crafting materals", "craftingmaterals" },
            new[] { "scrolls", "scroll" },
            new[] { "gems", "gem" },
            new[] { "mount", "mounts" },
            new[] { "books", "book" },
            new[] { "nothing" },
            new[] { "script", "scripts" },
            new[] { "reins" },
            new[] { "bells" },
            new[] { "saddle" },
            new[] { "ribbon" },
            new[] { "mask" },
            new[] { "food" },
            new[] { "hook" },
            new[] { "float" },
            new[] { "bait" },
            new[] { "finder" },
            new[] { "reel" },
            new[] { "fish" },
            new[] { "quest", "quests" },
            new[] { "awakening" },
            new[] { "pets", "pet" },
            new[] { "transform" }
        };

        DropFileDocument _document;
        string _documentPath;
        int _hiddenSections;
        public DropGenForm()
        {
            InitializeComponent();

            // Array of items
            ItemLists = new List<DropItem>[37]
            {
                Weapon,
                Armour,
                Helmet,
                Necklace,
                Bracelet,
                Ring,
                Amulet,
                Belt,
                Boot,
                Stone,
                Torch,
                Potion,
                Ore,
                Meat,
                CraftingMaterial,
                Scrolls,
                Gem,
                Mount,
                Book,
                Nothing,
                Script,
                Reins,
                Bells,
                Saddle,
                Ribbon,
                Mask,
                Food,
                Hook,
                Float,
                Bait,
                Finder,
                Reel,
                Fish,
                Quest,
                Awakening,
                Pets,
                Transform
            };

            // Array of item list boxes
            ItemListBoxes = new ListBox[37]
            {
                listBoxWeapon,
                listBoxArmour,
                listBoxHelmet,
                listBoxNecklace,
                listBoxBracelet,
                listBoxRing,
                listBoxAmulet,
                listBoxBelt,
                listBoxBoot,
                listBoxStone,
                listBoxTorch,
                listBoxPotion,
                listBoxOre,
                listBoxMeat,
                listBoxCraftingMaterial,
                listBoxScroll,
                listBoxGem,
                listBoxMount,
                listBoxBook,
                listBoxNothing,
                listBoxScript,
                listBoxReins,
                listBoxBells,
                listBoxSaddle,
                listBoxRibbon,
                listBoxMask,
                listBoxFood,
                listBoxHook,
                listBoxFloat,
                listBoxBait,
                listBoxFinder,
                listBoxReel,
                listBoxFish,
                listBoxQuest,
                listBoxAwakening,
                listBoxPets,
                listBoxTransform
            };

            // Add monsters to list
            for (int i = 0; i < Envir.MonsterInfoList.Count; i++)
            {
                listBoxMonsters.Items.Add(new MonsterDropInfo { Name = Envir.MonsterInfoList[i].Name, Path = Envir.MonsterInfoList[i].DropPath });
            }

            tabControlSeperateItems_SelectedIndexChanged(tabControlSeperateItems, null);
            listBoxMonsters.SelectedIndex = 0;
            labelMonsterList.Text = $"Monster Count: {Envir.MonsterInfoList.Count}";
        }

        // Gets server data
        public Envir Envir => SMain.EditEnvir;

        // Renders the current document into the text box and refreshes the gold fields.
        private void UpdateDropFile()
        {
            if (_document == null) return;

            textBoxDropList.Text = _document.Render();

            var gold = _document.GetGold();

            if (gold.HasValue)
            {
                Gold = gold.Value.Amount;
                GoldOdds = gold.Value.Odds;
                textBoxGoldAmount.Text = Gold;
                textBoxGoldOdds.Text = GoldOdds;
            }
            else
            {
                Gold = "0";
                GoldOdds = string.Empty;
                textBoxGoldAmount.Text = "0";
                textBoxGoldOdds.Text = string.Empty;
            }

            buttonUpdateGold.Enabled = false;
        }

        private static int IndexForTag(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return -1;

            for (int i = 0; i < TabTags.Length; i++)
                if (string.Equals(TabTags[i], tag, StringComparison.OrdinalIgnoreCase)) return i;

            return -1;
        }

        private static int IndexForHeader(string header)
        {
            if (string.IsNullOrEmpty(header)) return -1;

            for (int i = 0; i < HeaderAliases.Length; i++)
                if (HeaderAliases[i].Contains(header)) return i;

            return -1;
        }

        private static bool TryParseDropLine(string text, out string odds, out string name, out string quest)
        {
            odds = null;
            name = null;
            quest = string.Empty;

            var parts = (text ?? string.Empty).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) return false;
            if (!parts[0].StartsWith("1/", StringComparison.Ordinal)) return false;
            if (!int.TryParse(parts[0].Substring(2), out int chance) || chance < 1) return false;

            odds = parts[0];
            name = parts[1];

            if (parts.Length > 2 && string.Equals(parts[2], "Q", StringComparison.OrdinalIgnoreCase)) quest = "Q";

            return true;
        }

        // Item tab change, draw appropriate items
        private void tabControlSeperateItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl Tab = (TabControl)sender;

            foreach (var list in ItemListBoxes)
                list.Items.Clear();

            ListBox TempListBox = new ListBox();
            for (int i = 0; i < Envir.ItemInfoList.Count; i++)
            {
                if (Envir.ItemInfoList[i].Type.ToString() == Tab.SelectedTab.Tag.ToString())
                {
                    try
                    {
                        if (textBoxMinLevel.Text == string.Empty || textBoxMaxLevel.Text == string.Empty)                            
                            TempListBox.Items.Add(Envir.ItemInfoList[i].Name);
                        else if (Envir.ItemInfoList[i].RequiredAmount >= int.Parse(textBoxMinLevel.Text) &
                            Envir.ItemInfoList[i].RequiredAmount <= int.Parse(textBoxMaxLevel.Text))
                            TempListBox.Items.Add(Envir.ItemInfoList[i].Name);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unreadable level filters.");
                        break;
                    }
                }
            }

            switch (Tab.SelectedTab.Tag.ToString())
            {
                case "Weapon":
                    listBoxWeapon.Items.AddRange(TempListBox.Items);
                    break;
                case "Armour":
                    listBoxArmour.Items.AddRange(TempListBox.Items);
                    break;
                case "Helmet":
                    listBoxHelmet.Items.AddRange(TempListBox.Items);
                    break;
                case "Necklace":
                    listBoxNecklace.Items.AddRange(TempListBox.Items);
                    break;
                case "Bracelet":
                    listBoxBracelet.Items.AddRange(TempListBox.Items);
                    break;
                case "Ring":
                    listBoxRing.Items.AddRange(TempListBox.Items);
                    break;
                case "Amulet":
                    listBoxAmulet.Items.AddRange(TempListBox.Items);
                    break;
                case "Belt":
                    listBoxBelt.Items.AddRange(TempListBox.Items);
                    break;
                case "Boots":
                    listBoxBoot.Items.AddRange(TempListBox.Items);
                    break;
                case "Stone":
                    listBoxStone.Items.AddRange(TempListBox.Items);
                    break;
                case "Torch":
                    listBoxTorch.Items.AddRange(TempListBox.Items);
                    break;
                case "Potion":
                    listBoxPotion.Items.AddRange(TempListBox.Items);
                    break;
                case "Ore":
                    listBoxOre.Items.AddRange(TempListBox.Items);
                    break;
                case "Meat":
                    listBoxMeat.Items.AddRange(TempListBox.Items);
                    break;
                case "CraftingMaterial":
                    listBoxCraftingMaterial.Items.AddRange(TempListBox.Items);
                    break;
                case "Scroll":
                    listBoxScroll.Items.AddRange(TempListBox.Items);
                    break;
                case "Gem":
                    listBoxGem.Items.AddRange(TempListBox.Items);
                    break;
                case "Mount":
                    listBoxMount.Items.AddRange(TempListBox.Items);
                    break;
                case "Book":
                    listBoxBook.Items.AddRange(TempListBox.Items);
                    break;
                case "Nothing":
                    listBoxNothing.Items.AddRange(TempListBox.Items);
                    break;
                case "Script":
                    listBoxScript.Items.AddRange(TempListBox.Items);
                    break;
                case "Reins":
                    listBoxReins.Items.AddRange(TempListBox.Items);
                    break;
                case "Bells":
                    listBoxBells.Items.AddRange(TempListBox.Items);
                    break;
                case "Saddle":
                    listBoxSaddle.Items.AddRange(TempListBox.Items);
                    break;
                case "Ribbon":
                    listBoxRibbon.Items.AddRange(TempListBox.Items);
                    break;
                case "Mask":
                    listBoxMask.Items.AddRange(TempListBox.Items);
                    break;
                case "Food":
                    listBoxFood.Items.AddRange(TempListBox.Items);
                    break;
                case "Hook":
                    listBoxHook.Items.AddRange(TempListBox.Items);
                    break;
                case "Float":
                    listBoxFloat.Items.AddRange(TempListBox.Items);
                    break;
                case "Bait":
                    listBoxBait.Items.AddRange(TempListBox.Items);
                    break;
                case "Finder":
                    listBoxFinder.Items.AddRange(TempListBox.Items);
                    break;
                case "Reel":
                    listBoxReel.Items.AddRange(TempListBox.Items);
                    break;
                case "Fish":
                    listBoxFish.Items.AddRange(TempListBox.Items);
                    break;
                case "Quest":
                    listBoxQuest.Items.AddRange(TempListBox.Items);
                    break;
                case "Awakening":
                    listBoxAwakening.Items.AddRange(TempListBox.Items);
                    break;
                case "Pets":
                    listBoxPets.Items.AddRange(TempListBox.Items);
                    break;
                case "Transform":
                    listBoxTransform.Items.AddRange(TempListBox.Items);
                    break;
            }
        }
        
        // Update the results to show them filtered
        private void FilterValueChange(object sender, EventArgs e)
        {
            tabControlSeperateItems_SelectedIndexChanged(tabControlSeperateItems, null);
        }

        // Add the selected item to the drop section that matches the current tab.
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var tab = tabControlSeperateItems.SelectedTab;
            if (tab == null) return;

            int index = IndexForTag(tab.Tag?.ToString());
            if (index < 0) return;

            var listBox = ItemListBoxes[index];
            if (listBox.SelectedItem == null) return;

            if (!int.TryParse((textBoxItemOdds.Text ?? string.Empty).Trim(), out int chance) || chance < 1)
            {
                MessageBox.Show("Drop rate must be a positive whole number.");
                return;
            }

            if (_document == null) LoadDropFile(false);

            string name = listBox.SelectedItem.ToString();
            string quest = QuestOnlyCheckBox.Checked ? "Q" : string.Empty;
            string odds = $"1/{chance}";
            string line = string.IsNullOrEmpty(quest) ? $"{odds} {name}" : $"{odds} {name} {quest}";

            ItemLists[index].Add(new DropItem { Name = name, Odds = odds, Quest = quest });
            _document.AppendItem(CanonicalHeaders[index], HeaderAliases[index], line);

            UpdateDropFile();
            SaveDropFile();
        }

        // Choose another monster.
        private void listBoxMonsters_SelectedItemChanged(object sender, EventArgs e)
        {
            // Empty List<DropItem>'s
            foreach (var item in ItemLists)
                item.Clear();

            LoadDropFile(false);
            UpdateDropFile();

            textBoxMinLevel.Text = string.Empty;
            textBoxMaxLevel.Text = string.Empty;
            checkBoxCap.Checked = false;

            string hidden = _hiddenSections > 0
                ? $"  ({_hiddenSections} section(s) not shown here, kept as-is)"
                : string.Empty;

            labelMobLevel.Text =
                $"Currently Editing: {((MonsterDropInfo)listBoxMonsters.SelectedItem).Name} - Level: {Envir.MonsterInfoList[listBoxMonsters.SelectedIndices[0]].Level}" + hidden;
        }

        public string GetPathOfSelectedItem()
        {
            var selectedItem = (MonsterDropInfo)listBoxMonsters.SelectedItem;

            if (selectedItem == null) return null;

            var path = "";

            if (string.IsNullOrEmpty(selectedItem.Path))
            {
                path = Path.Combine(Settings.DropPath, $"{selectedItem.Name}.txt");
            }
            else
            {
                path = Path.Combine(Settings.DropPath, selectedItem.Path + ".txt");
            }

            return path;

        }

        // Load the selected monster's drop file into the document and the item lists.
        private void LoadDropFile(bool edit)
        {
            foreach (var item in ItemLists)
                item.Clear();

            _documentPath = GetPathOfSelectedItem();

            _document = edit
                ? DropFileDocument.FromText(textBoxDropList.Text, _documentPath, _document?.TextEncoding, _document != null && _document.HasBom)
                : DropFileDocument.Load(_documentPath);

            _hiddenSections = 0;

            foreach (var section in _document.Sections)
            {
                if (section.IsGold) continue;

                int index = IndexForHeader(section.Header);

                if (index < 0)
                {
                    foreach (var line in section.Lines)
                    {
                        if (TryParseDropLine(line.Text, out _, out _, out _))
                        {
                            _hiddenSections++;
                            break;
                        }
                    }

                    continue;
                }

                foreach (var line in section.Lines)
                {
                    if (!TryParseDropLine(line.Text, out string odds, out string name, out string quest)) continue;
                    if (string.Equals(name, "Gold", StringComparison.OrdinalIgnoreCase)) continue;
                    if (name.StartsWith("GROUP", StringComparison.OrdinalIgnoreCase)) continue;

                    ItemLists[index].Add(new DropItem { Odds = odds, Name = name, Quest = quest });
                }
            }
        }

        // Save the document. An untouched document is written back byte for byte.
        private void SaveDropFile()
        {
            if (_document == null) return;

            var path = string.IsNullOrEmpty(_documentPath) ? GetPathOfSelectedItem() : _documentPath;
            if (string.IsNullOrEmpty(path)) return;

            _document.Save(path);
        }

        //Edit gold amount/odds
        private void GoldDropChange(object sender, EventArgs e)
        {
            if (textBoxGoldOdds.Text != GoldOdds || textBoxGoldAmount.Text != Gold)
                buttonUpdateGold.Enabled = true;
            else
                buttonUpdateGold.Enabled = false;
        }

        //Switch to Edit mode
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (buttonEdit.Text == "Accept")
            {
                textBoxDropList.ReadOnly = true;
                textBoxDropList.BackColor = System.Drawing.Color.Cornsilk;
                buttonEdit.Text = "Edit Drop File";
                //buttonEdit.Image = Properties.Resources.edit;

                // Empty List<DropItem>'s
                foreach (var item in ItemLists)
                    item.Clear();

                LoadDropFile(true);
                SaveDropFile();
                UpdateDropFile();

                buttonAdd.Enabled = true;
                listBoxMonsters.Enabled = true;
                tabControlSeperateItems.Enabled = true;
                groupBoxGold.Enabled = true;
                groupBoxItem.Enabled = true;
            }
            else
            {
                textBoxDropList.ReadOnly = false;
                textBoxDropList.BackColor = System.Drawing.Color.Honeydew;
                buttonEdit.Text = "Accept";
                //buttonEdit.Image = Properties.Resources.accept;

                buttonAdd.Enabled = false;
                listBoxMonsters.Enabled = false;
                tabControlSeperateItems.Enabled = false;
                groupBoxGold.Enabled = false;
                groupBoxItem.Enabled = false;
            }
        }

        //Cap item range to monsters level
        private void checkBoxCap_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCap.Checked == true)
            {
                textBoxMinLevel.Text = "0";
                textBoxMaxLevel.Text = Envir.MonsterInfoList[listBoxMonsters.SelectedIndices[0]].Level.ToString();
                tabControlSeperateItems_SelectedIndexChanged(tabControlSeperateItems, null);
            }
            else
            {
                textBoxMinLevel.Text = string.Empty;
                textBoxMaxLevel.Text = string.Empty;
                tabControlSeperateItems_SelectedIndexChanged(tabControlSeperateItems, null);
            }
        }

        private void buttonUpdateGold_Click(object sender, EventArgs e)
        {
            if (_document == null) return;

            string odds = (textBoxGoldOdds.Text ?? string.Empty).Trim();
            string amount = (textBoxGoldAmount.Text ?? string.Empty).Trim();

            if (amount == "0")
            {
                _document.RemoveGold();
                UpdateDropFile();
                SaveDropFile();
                tabControlSeperateItems.Focus();
                return;
            }

            if (!int.TryParse(odds, out int chance) || chance < 1 ||
                !uint.TryParse(amount, out uint gold) || gold == 0)
            {
                MessageBox.Show("Gold rate and amount must be positive numbers.");
                return;
            }

            _document.SetGold(odds, amount);

            UpdateDropFile();
            SaveDropFile();
            tabControlSeperateItems.Focus();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            int index = listBoxMonsters.FindString(textBoxSearch.Text, -1);

            if (index != -1)
            {
                textBoxSearch.BackColor = System.Drawing.SystemColors.Info;
                listBoxMonsters.SetSelected(index, true);
            }
            else
            {
                textBoxSearch.BackColor = System.Drawing.Color.FromArgb(0xCC, 0x33, 0x33);
            }
        }
    }

    // Item setup
    public class DropItem
    {
        public string Name, Odds, Quest;
    }
}
