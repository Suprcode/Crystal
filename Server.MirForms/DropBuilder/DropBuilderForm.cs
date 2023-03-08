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

        // Updates the drop file text
        private void UpdateDropFile()
        {
            textBoxDropList.Clear();

            textBoxDropList.Text += $";Gold{Environment.NewLine}";
            if (Gold != "0")
            {
                textBoxDropList.Text += $"1/{GoldOdds} Gold {Gold}{Environment.NewLine}";
                textBoxGoldAmount.Text = Gold;
                textBoxGoldOdds.Text = GoldOdds;
            }
            else
            {
                textBoxGoldAmount.Text = "0";
                textBoxGoldOdds.Text = string.Empty;
            }
                
            textBoxDropList.Text += string.Format("{0};Weapons{0}", Environment.NewLine);
            for (int i = 0; i < Weapon.Count; i++)
                textBoxDropList.Text += $"{Weapon[i].Odds} {Weapon[i].Name} {Weapon[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Armours{0}", Environment.NewLine);
            for (int i = 0; i < Armour.Count; i++)
                textBoxDropList.Text += $"{Armour[i].Odds} {Armour[i].Name} {Armour[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Helmets{0}", Environment.NewLine);
            for (int i = 0; i < Helmet.Count; i++)
                textBoxDropList.Text += $"{Helmet[i].Odds} {Helmet[i].Name} {Helmet[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Necklaces{0}", Environment.NewLine);
            for (int i = 0; i < Necklace.Count; i++)
                textBoxDropList.Text +=
                    $"{Necklace[i].Odds} {Necklace[i].Name} {Necklace[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Bracelets{0}", Environment.NewLine);
            for (int i = 0; i < Bracelet.Count; i++)
                textBoxDropList.Text +=
                    $"{Bracelet[i].Odds} {Bracelet[i].Name} {Bracelet[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Rings{0}", Environment.NewLine);
            for (int i = 0; i < Ring.Count; i++)
                textBoxDropList.Text += $"{Ring[i].Odds} {Ring[i].Name} {Ring[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Amulets{0}", Environment.NewLine);
            for (int i = 0; i < Amulet.Count; i++)
                textBoxDropList.Text += $"{Amulet[i].Odds} {Amulet[i].Name} {Amulet[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Belts{0}", Environment.NewLine);
            for (int i = 0; i < Belt.Count; i++)
                textBoxDropList.Text += $"{Belt[i].Odds} {Belt[i].Name} {Belt[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Boots{0}", Environment.NewLine);
            for (int i = 0; i < Boot.Count; i++)
                textBoxDropList.Text += $"{Boot[i].Odds} {Boot[i].Name} {Boot[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Stones{0}", Environment.NewLine);
            for (int i = 0; i < Stone.Count; i++)
                textBoxDropList.Text += $"{Stone[i].Odds} {Stone[i].Name} {Stone[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Torches{0}", Environment.NewLine);
            for (int i = 0; i < Torch.Count; i++)
                textBoxDropList.Text += $"{Torch[i].Odds} {Torch[i].Name} {Torch[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Potions{0}", Environment.NewLine);
            for (int i = 0; i < Potion.Count; i++)
                textBoxDropList.Text += $"{Potion[i].Odds} {Potion[i].Name} {Potion[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Ores{0}", Environment.NewLine);
            for (int i = 0; i < Ore.Count; i++)
                textBoxDropList.Text += $"{Ore[i].Odds} {Ore[i].Name} {Ore[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Meat{0}", Environment.NewLine);
            for (int i = 0; i < Meat.Count; i++)
                textBoxDropList.Text += $"{Meat[i].Odds} {Meat[i].Name} {Meat[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Crafting Materials{0}", Environment.NewLine);
            for (int i = 0; i < CraftingMaterial.Count; i++)
                textBoxDropList.Text +=
                    $"{CraftingMaterial[i].Odds} {CraftingMaterial[i].Name} {CraftingMaterial[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Scrolls{0}", Environment.NewLine);
            for (int i = 0; i < Scrolls.Count; i++)
                textBoxDropList.Text += $"{Scrolls[i].Odds} {Scrolls[i].Name} {Scrolls[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Gems{0}", Environment.NewLine);
            for (int i = 0; i < Gem.Count; i++)
                textBoxDropList.Text += $"{Gem[i].Odds} {Gem[i].Name} {Gem[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Mount{0}", Environment.NewLine);
            for (int i = 0; i < Mount.Count; i++)
                textBoxDropList.Text += $"{Mount[i].Odds} {Mount[i].Name} {Mount[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Books{0}", Environment.NewLine);
            for (int i = 0; i < Book.Count; i++)
                textBoxDropList.Text += $"{Book[i].Odds} {Book[i].Name} {Book[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Nothing{0}", Environment.NewLine);
            for (int i = 0; i < Nothing.Count; i++)
                textBoxDropList.Text += $"{Nothing[i].Odds} {Nothing[i].Name} {Nothing[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Script{0}", Environment.NewLine);
            for (int i = 0; i < Script.Count; i++)
                textBoxDropList.Text += $"{Script[i].Odds} {Script[i].Name} {Script[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Reins{0}", Environment.NewLine);
            for (int i = 0; i < Reins.Count; i++)
                textBoxDropList.Text += $"{Reins[i].Odds} {Reins[i].Name} {Reins[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Bells{0}", Environment.NewLine);
            for (int i = 0; i < Bells.Count; i++)
                textBoxDropList.Text += $"{Bells[i].Odds} {Bells[i].Name} {Bells[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Saddle{0}", Environment.NewLine);
            for (int i = 0; i < Saddle.Count; i++)
                textBoxDropList.Text += $"{Saddle[i].Odds} {Saddle[i].Name} {Saddle[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Ribbon{0}", Environment.NewLine);
            for (int i = 0; i < Ribbon.Count; i++)
                textBoxDropList.Text += $"{Ribbon[i].Odds} {Ribbon[i].Name} {Ribbon[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Mask{0}", Environment.NewLine);
            for (int i = 0; i < Mask.Count; i++)
                textBoxDropList.Text += $"{Mask[i].Odds} {Mask[i].Name} {Mask[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Food{0}", Environment.NewLine);
            for (int i = 0; i < Food.Count; i++)
                textBoxDropList.Text += $"{Food[i].Odds} {Food[i].Name} {Food[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Hook{0}", Environment.NewLine);
            for (int i = 0; i < Hook.Count; i++)
                textBoxDropList.Text += $"{Hook[i].Odds} {Hook[i].Name} {Hook[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Float{0}", Environment.NewLine);
            for (int i = 0; i < Float.Count; i++)
                textBoxDropList.Text += $"{Float[i].Odds} {Float[i].Name} {Float[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Bait{0}", Environment.NewLine);
            for (int i = 0; i < Bait.Count; i++)
                textBoxDropList.Text += $"{Bait[i].Odds} {Bait[i].Name} {Bait[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Finder{0}", Environment.NewLine);
            for (int i = 0; i < Finder.Count; i++)
                textBoxDropList.Text += $"{Finder[i].Odds} {Finder[i].Name} {Finder[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Reel{0}", Environment.NewLine);
            for (int i = 0; i < Reel.Count; i++)
                textBoxDropList.Text += $"{Reel[i].Odds} {Reel[i].Name} {Reel[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Fish{0}", Environment.NewLine);
            for (int i = 0; i < Fish.Count; i++)
                textBoxDropList.Text += $"{Fish[i].Odds} {Fish[i].Name} {Fish[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Quest{0}", Environment.NewLine);
            for (int i = 0; i < Quest.Count; i++)
                textBoxDropList.Text += $"{Quest[i].Odds} {Quest[i].Name} {Quest[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Awakening{0}", Environment.NewLine);
            for (int i = 0; i < Awakening.Count; i++)
                textBoxDropList.Text +=
                    $"{Awakening[i].Odds} {Awakening[i].Name} {Awakening[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Pets{0}", Environment.NewLine);
            for (int i = 0; i < Pets.Count; i++)
                textBoxDropList.Text += $"{Pets[i].Odds} {Pets[i].Name} {Pets[i].Quest}{Environment.NewLine}";

            textBoxDropList.Text += string.Format("{0};Transform{0}", Environment.NewLine);
            for (int i = 0; i < Transform.Count; i++)
                textBoxDropList.Text +=
                    $"{Transform[i].Odds} {Transform[i].Name} {Transform[i].Quest}{Environment.NewLine}";

            SaveDropFile();
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

        // Add the item to the drop list
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int dropChance;

            int.TryParse(textBoxItemOdds.Text, out dropChance);

            if (dropChance < 1) dropChance = 1;

            string quest = QuestOnlyCheckBox.Checked ? "Q" : "";

            try
            {
                switch (tabControlSeperateItems.SelectedTab.Tag.ToString())
                {
                    case "Weapon":
                        Weapon.Add(new DropItem { Name = listBoxWeapon.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Armour":
                        Armour.Add(new DropItem { Name = listBoxArmour.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Helmet":
                        Helmet.Add(new DropItem { Name = listBoxHelmet.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Necklace":
                        Necklace.Add(new DropItem { Name = listBoxNecklace.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Bracelet":
                        Bracelet.Add(new DropItem { Name = listBoxBracelet.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Ring":
                        Ring.Add(new DropItem { Name = listBoxRing.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Amulet":
                        Amulet.Add(new DropItem { Name = listBoxAmulet.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Belt":
                        Belt.Add(new DropItem { Name = listBoxBelt.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Boots":
                        Boot.Add(new DropItem { Name = listBoxBoot.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Stone":
                        Stone.Add(new DropItem { Name = listBoxStone.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Torch":
                        Torch.Add(new DropItem { Name = listBoxTorch.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Potion":
                        Potion.Add(new DropItem { Name = listBoxPotion.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Ore":
                        Ore.Add(new DropItem { Name = listBoxOre.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Meat":
                        Meat.Add(new DropItem { Name = listBoxMeat.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "CraftingMaterial":
                        CraftingMaterial.Add(new DropItem { Name = listBoxCraftingMaterial.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}"
                        });
                        break;
                    case "Scroll":
                        Scrolls.Add(new DropItem { Name = listBoxScroll.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Gem":
                        Gem.Add(new DropItem { Name = listBoxGem.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Mount":
                        Mount.Add(new DropItem { Name = listBoxMount.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Book":
                        Book.Add(new DropItem { Name = listBoxBook.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Nothing":
                        Nothing.Add(new DropItem { Name = listBoxNothing.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Script":
                        Script.Add(new DropItem { Name = listBoxScript.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Reins":
                        Reins.Add(new DropItem { Name = listBoxReins.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Bells":
                        Bells.Add(new DropItem { Name = listBoxBells.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Saddle":
                        Saddle.Add(new DropItem { Name = listBoxSaddle.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Ribbon":
                        Ribbon.Add(new DropItem { Name = listBoxRibbon.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Mask":
                        Mask.Add(new DropItem { Name = listBoxMask.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Food":
                        Food.Add(new DropItem { Name = listBoxFood.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Hook":
                        Hook.Add(new DropItem { Name = listBoxHook.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Float":
                        Float.Add(new DropItem { Name = listBoxFloat.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Bait":
                        Bait.Add(new DropItem { Name = listBoxBait.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Finder":
                        Finder.Add(new DropItem { Name = listBoxFinder.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Reel":
                        Reel.Add(new DropItem { Name = listBoxReel.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Fish":
                        Fish.Add(new DropItem { Name = listBoxFish.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Quest":
                        Quest.Add(new DropItem { Name = listBoxQuest.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Awakening":
                        Awakening.Add(new DropItem { Name = listBoxAwakening.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Pets":
                        Pets.Add(new DropItem { Name = listBoxPets.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                    case "Transform":
                        Transform.Add(new DropItem { Name = listBoxTransform.SelectedItem.ToString().Replace(" ", string.Empty), Odds =
                            $"1/{dropChance}", Quest = quest });
                        break;
                }

                UpdateDropFile();
            }
            catch
            {
                //No item selected when trying to add an item to the drop
            }
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

            labelMobLevel.Text =
                $"Currently Editing: {((MonsterDropInfo)listBoxMonsters.SelectedItem).Name} - Level: {Envir.MonsterInfoList[listBoxMonsters.SelectedIndices[0]].Level}";
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

        // Load the monster.txt drop file.
        private void LoadDropFile(bool edit)
        {
            var lines = (edit == false) ? File.ReadAllLines(GetPathOfSelectedItem()) : textBoxDropList.Lines;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(";Gold"))
                {
                    if (lines[i + 1].StartsWith("1/"))
                    {
                        var workingLine = lines[i + 1].Split(' ');
                        GoldOdds = workingLine[0].Remove(0,2);
                        Gold = workingLine[2];
                        break;
                    }
                    else
                    {
                        GoldOdds = "0";
                        Gold = "0";
                    }
                }
            }

            string[] Headers = new string[37]
            {
            ";Weapons",
            ";Armours",
            ";Helmets",
            ";Necklaces",
            ";Bracelets",
            ";Rings",
            ";Amulets",
            ";Belts",
            ";Boots",
            ";Stones",
            ";Torches",
            ";Potions",
            ";Ores",
            ";Meat",
            ";Crafting Materials",
            ";Scrolls",
            ";Gems",
            ";Mount",
            ";Books",
            ";Nothing",
            ";Script",
            ";Reins",
            ";Bells",
            ";Saddle",
            ";Ribbon",
            ";Mask",
            ";Food",
            ";Hook",
            ";Float",
            ";Bait",
            ";Finder",
            ";Reel",
            ";Fish",
            ";Quest",
            ";Awakening",
            ";Pets",
            ";Transform"
            };

            for (int i = 0; i < Headers.Length; i++)
            {
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[j].StartsWith(Headers[i]))
                    {
                        for (int k = j + 1; k < lines.Length; k++)
                        {
                            if (lines[k].StartsWith(";")) break;

                            var workingLine = lines[k].Split(' ');
                            if (workingLine.Length < 2) continue;

                            var quest = "";

                            if(workingLine.Length > 2 && workingLine[2] == "Q")
                            {
                                quest = workingLine[2];
                            }

                            DropItem newDropItem = new DropItem { Odds = workingLine[0], Name = workingLine[1], Quest = quest };
                            switch (i)
                            {
                                case 0:
                                    Weapon.Add(newDropItem);
                                    break;
                                case 1:
                                    Armour.Add(newDropItem);
                                    break;
                                case 2:
                                    Helmet.Add(newDropItem);
                                    break;
                                case 3:
                                    Necklace.Add(newDropItem);
                                    break;
                                case 4:
                                    Bracelet.Add(newDropItem);
                                    break;
                                case 5:
                                    Ring.Add(newDropItem);
                                    break;
                                case 6:
                                    Amulet.Add(newDropItem);
                                    break;
                                case 7:
                                    Belt.Add(newDropItem);
                                    break;
                                case 8:
                                    Boot.Add(newDropItem);
                                    break;
                                case 9:
                                    Stone.Add(newDropItem);
                                    break;
                                case 10:
                                    Torch.Add(newDropItem);
                                    break;
                                case 11:
                                    Potion.Add(newDropItem);
                                    break;
                                case 12:
                                    Ore.Add(newDropItem);
                                    break;
                                case 13:
                                    Meat.Add(newDropItem);
                                    break;
                                case 14:
                                    CraftingMaterial.Add(newDropItem);
                                    break;
                                case 15:
                                    Scrolls.Add(newDropItem);
                                    break;
                                case 16:
                                    Gem.Add(newDropItem);
                                    break;
                                case 17:
                                    Mount.Add(newDropItem);
                                    break;
                                case 18:
                                    Book.Add(newDropItem);
                                    break;
                                case 19:
                                    Nothing.Add(newDropItem);
                                    break;
                                case 20:
                                    Script.Add(newDropItem);
                                    break;
                                case 21:
                                    Reins.Add(newDropItem);
                                    break;
                                case 22:
                                    Bells.Add(newDropItem);
                                    break;
                                case 23:
                                    Saddle.Add(newDropItem);
                                    break;
                                case 24:
                                    Ribbon.Add(newDropItem);
                                    break;
                                case 25:
                                    Mask.Add(newDropItem);
                                    break;
                                case 26:
                                    Food.Add(newDropItem);
                                    break;
                                case 27:
                                    Hook.Add(newDropItem);
                                    break;
                                case 28:
                                    Float.Add(newDropItem);
                                    break;
                                case 29:
                                    Bait.Add(newDropItem);
                                    break;
                                case 30:
                                    Finder.Add(newDropItem);
                                    break;
                                case 31:
                                    Reel.Add(newDropItem);
                                    break;
                                case 32:
                                    Fish.Add(newDropItem);
                                    break;
                                case 33:
                                    Quest.Add(newDropItem);
                                    break;
                                case 34:
                                    Awakening.Add(newDropItem);
                                    break;
                                case 35:
                                    Pets.Add(newDropItem);
                                    break;
                                case 36:
                                    Transform.Add(newDropItem);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        // Save the monster.txt drop file
        private void SaveDropFile()
        {
            var dropFile = GetPathOfSelectedItem();

            if (dropFile == null) return;

            using (FileStream fs = new FileStream(dropFile, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (string line in textBoxDropList.Lines)
                        sw.Write(line + sw.NewLine);
                }
            }
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
            Gold = textBoxGoldAmount.Text;
            GoldOdds = textBoxGoldOdds.Text;

            UpdateDropFile();

            buttonUpdateGold.Enabled = false;
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
