using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;
using System.Diagnostics;
using System.Drawing.Text;
using System.Numerics;

namespace Server
{
    public partial class PlayerInfoForm : Form
    {
        CharacterInfo Character = null;

        public PlayerInfoForm()
        {
            InitializeComponent();
        }

        public PlayerInfoForm(uint playerId)
        {
            InitializeComponent();

            PlayerObject player = SMain.Envir.GetPlayer(playerId);

            if (player == null)
            {
                Close();
                return;
            }

            Character = SMain.Envir.GetCharacterInfo(player.Name);

            UpdateTabs();
        }

        #region PlayerInfo
        private void UpdatePlayerInfo()
        {
            IndexTextBox.Text = Character.Index.ToString();
            NameTextBox.Text = Character.Name;
            LevelTextBox.Text = Character.Level.ToString();
            PKPointsTextBox.Text = Character.PKPoints.ToString();
            GoldTextBox.Text = $"{Character.AccountInfo.Gold:n0}";
            GameGoldTextBox.Text = String.Format("{0:n0}", Character.AccountInfo.Credit);


            if (Character?.Player != null)
            {
                CurrentMapLabel.Text = $"{Character.Player.CurrentMap.Info.Title} / {Character.Player.CurrentMap.Info.FileName}";
                CurrentXY.Text = $"X:{Character.CurrentLocation.X}: Y:{Character.CurrentLocation.Y}";

                ExpTextBox.Text = $"{string.Format("{0:#0.##%}", Character.Player.Experience / (double)Character.Player.MaxExperience)}";
                ACBox.Text = $"{Character.Player.Stats[Stat.MinAC]}-{Character.Player.Stats[Stat.MaxAC]}";
                AMCBox.Text = $"{Character.Player.Stats[Stat.MinMAC]}-{Character.Player.Stats[Stat.MaxMAC]}";
                DCBox.Text = $"{Character.Player.Stats[Stat.MinDC]}-{Character.Player.Stats[Stat.MaxDC]}";
                MCBox.Text = $"{Character.Player.Stats[Stat.MinMC]}-{Character.Player.Stats[Stat.MaxMC]}";
                SCBox.Text = $"{Character.Player.Stats[Stat.MinSC]}-{Character.Player.Stats[Stat.MaxSC]}";
                ACCBox.Text = $"{Character.Player.Stats[Stat.Accuracy]}";
                AGILBox.Text = $"{Character.Player.Stats[Stat.Agility]}";
                ATKSPDBox.Text = $"{Character.Player.Stats[Stat.AttackSpeed]}";
            }
            else
            {
                CurrentMapLabel.Text = "OFFLINE";
                CurrentXY.Text = "OFFLINE";
            }

            CurrentIPLabel.Text = Character.AccountInfo.LastIP;
            OnlineTimeLabel.Text = Character.LastLoginDate > Character.LastLogoutDate ? (SMain.Envir.Now - Character.LastLoginDate).TotalMinutes.ToString("##") + " minutes" : "Offline";

            ChatBanExpiryTextBox.Text = Character.ChatBanExpiryDate.ToString();
        }
        #endregion

        #region PlayerPets
        private void UpdatePetInfo()
        {
            ClearPetInfo();

            if (Character?.Player == null) return;

            foreach (MonsterObject Pet in Character.Player.Pets)
            {
                var listItem = new ListViewItem(Pet.Name) { Tag = Pet };
                listItem.SubItems.Add(Pet.PetLevel.ToString());
                listItem.SubItems.Add($"{Pet.Health}/{Pet.MaxHealth}");
                listItem.SubItems.Add($"Map: {Pet.CurrentMap.Info.Title}, X: {Pet.CurrentLocation.X}, Y: {Pet.CurrentLocation.Y}");

                PetView.Items.Add(listItem);
            }
        }

        private void ClearPetInfo()
        {
            PetView.Items.Clear();
        }
        #endregion

        #region PlayerMagics
        private void UpdatePlayerMagics()
        {
            MagicListViewNF.Items.Clear();

            for (int i = 0; i < Character.Magics.Count; i++)
            {
                UserMagic magic = Character.Magics[i];
                if (magic == null) continue;

                ListViewItem ListItem = new ListViewItem(magic.Info.Name.ToString()) { Tag = this };

                ListItem.SubItems.Add(magic.Level.ToString());

                switch (magic.Level)
                {
                    case 0:
                        ListItem.SubItems.Add($"{magic.Experience}/{magic.Info.Need1}");
                        break;
                    case 1:
                        ListItem.SubItems.Add($"{magic.Experience}/{magic.Info.Need2}");
                        break;
                    case 2:
                        ListItem.SubItems.Add($"{magic.Experience}/{magic.Info.Need3}");
                        break;
                    case 3:
                        ListItem.SubItems.Add($"-");
                        break;
                }

                if (magic.Key > 8)
                {
                    var key = magic.Key % 8;

                    ListItem.SubItems.Add(string.Format("CTRL+F{0}", key != 0 ? key : 8));
                }
                else if (magic.Key > 0)
                {
                    ListItem.SubItems.Add(string.Format("F{0}", magic.Key));
                }
                else if (magic.Key == 0)
                {
                    ListItem.SubItems.Add(string.Format("No Key", magic.Key));
                }

                ListItem.SubItems.Add(magic.Key.ToString());
                MagicListViewNF.Items.Add(ListItem);
            }
        }
        #endregion

        #region PlayerQuests
        private void UpdatePlayerQuests()
        {
            QuestInfoListViewNF.Items.Clear();

            foreach (int completedQuestID in Character.CompletedQuests)
            {
                QuestInfo completedQuest = SMain.Envir.GetQuestInfo(completedQuestID);

                ListViewItem item = new ListViewItem(completedQuestID.ToString());
                item.SubItems.Add("Completed");
                item.SubItems.Add(completedQuest.Name.ToString());
                QuestInfoListViewNF.Items.Add(item);
            }

            foreach (QuestProgressInfo currentQuest in Character.CurrentQuests)
            {
                ListViewItem item = new ListViewItem(currentQuest.Index.ToString());
                item.SubItems.Add("In Progress");
                item.SubItems.Add(currentQuest.Info.Name.ToString());
                QuestInfoListViewNF.Items.Add(item);
            }
        }
        #endregion

        #region PlayerItems
        private void UpdatePlayerItems()
        {
            PlayerItemInfoListViewNF.Items.Clear();

            if (Character == null) return;

            for (int i = 0; i < Character.Inventory.Length; i++)
            {
                UserItem inventoryItem = Character.Inventory[i];

                if (inventoryItem == null) continue;

                ListViewItem inventoryItemListItem = new ListViewItem($"{inventoryItem.UniqueID}");

                if (i < 6)
                {
                    inventoryItemListItem.SubItems.Add($"Belt | Slot: [{i + 1}]");
                }
                else if (i >= 6 && i < 46)
                {
                    inventoryItemListItem.SubItems.Add($"Inventory Bag I | Slot: [{i - 5}]");
                }
                else
                {
                    inventoryItemListItem.SubItems.Add($"Inventory Bag II | Slot: [{i - 45}]");
                }

                inventoryItemListItem.SubItems.Add($"{inventoryItem.FriendlyName}");
                inventoryItemListItem.SubItems.Add($"{inventoryItem.Count}/{inventoryItem.Info.StackSize}");
                inventoryItemListItem.SubItems.Add($"{inventoryItem.CurrentDura}/{inventoryItem.MaxDura}");

                PlayerItemInfoListViewNF.Items.Add(inventoryItemListItem);
            }


            for (int i = 0; i < Character.QuestInventory.Length; i++)
            {
                UserItem questItem = Character.QuestInventory[i];

                if (questItem == null) continue;

                ListViewItem questItemListItem = new ListViewItem($"{questItem.UniqueID}");
                questItemListItem.SubItems.Add($"Quest Bag | Slot: [{i + 1}]");

                questItemListItem.SubItems.Add($"{questItem.FriendlyName}");
                questItemListItem.SubItems.Add($"{questItem.Count}/{questItem.Info.StackSize}");
                questItemListItem.SubItems.Add($"{questItem.CurrentDura}/{questItem.MaxDura}");

                PlayerItemInfoListViewNF.Items.Add(questItemListItem);
            }

            for (int i = 0; i < Character.AccountInfo.Storage.Length; i++)
            {
                UserItem storeItem = Character.AccountInfo.Storage[i];

                if (storeItem == null) continue;

                ListViewItem storeItemListItem = new ListViewItem($"{storeItem.UniqueID}");

                if (i < 80)
                {
                    storeItemListItem.SubItems.Add($"Storage I | Slot: [{i + 1}]");
                }
                else
                {
                    storeItemListItem.SubItems.Add($"Storage II | Slot: [{i - 79}]");
                }

                storeItemListItem.SubItems.Add($"{storeItem.FriendlyName}");
                storeItemListItem.SubItems.Add($"{storeItem.Count}/{storeItem.Info.StackSize}");
                storeItemListItem.SubItems.Add($"{storeItem.CurrentDura}/{storeItem.MaxDura}");

                PlayerItemInfoListViewNF.Items.Add(storeItemListItem);
            }

            for (int i = 0; i < Character.Equipment.Length; i++)
            {
                UserItem equipItem = Character.Equipment[i];

                if (equipItem == null) continue;

                ListViewItem equipItemListItem = new ListViewItem($"{equipItem.UniqueID}");

                equipItemListItem.SubItems.Add($"Equipment | Slot: [{i + 1}]");

                equipItemListItem.SubItems.Add($"{equipItem.FriendlyName}");
                equipItemListItem.SubItems.Add($"{equipItem.Count}/{equipItem.Info.StackSize}");
                equipItemListItem.SubItems.Add($"{equipItem.CurrentDura}/{equipItem.MaxDura}");

                PlayerItemInfoListViewNF.Items.Add(equipItemListItem);
            }
        }
        #endregion

        #region Buttons
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Update?", "Update.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            SaveChanges();
        }

        private void SaveChanges()
        {
            CharacterInfo info = Character;

            string tempGold = GoldTextBox.Text.Replace(",", "");
            string tempCredit = GameGoldTextBox.Text.Replace(",", "");

            info.Name = NameTextBox.Text;
            info.Level = Convert.ToByte(LevelTextBox.Text);
            info.PKPoints = Convert.ToInt32(PKPointsTextBox.Text);
            info.AccountInfo.Gold = Convert.ToUInt32(tempGold);
            info.AccountInfo.Credit = Convert.ToUInt32(tempCredit);

            UpdateTabs();
        }

        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            if (Character?.Player == null) return;

            if (SendMessageTextBox.Text.Length < 1) return;

            Character.Player.ReceiveChat(SendMessageTextBox.Text, ChatType.Announcement);
        }

        private void KickButton_Click(object sender, EventArgs e)
        {
            if (Character?.Player == null) return;

            Character.Player.Connection.SendDisconnect(4);
            //also update account so player can't log back in for x minutes?
        }

        private void KillButton_Click(object sender, EventArgs e)
        {
            if (Character?.Player == null) return;

            Character.Player.Die();
        }

        private void KillPetsButton_Click(object sender, EventArgs e)
        {
            if (Character?.Player == null) return;

            for (int i = Character.Player.Pets.Count - 1; i >= 0; i--)
                Character.Player.Pets[i].Die();

            ClearPetInfo();
        }
        private void SafeZoneButton_Click(object sender, EventArgs e)
        {
            if (Character?.Player == null) return;

            Character.Player.Teleport(SMain.Envir.GetMap(Character.BindMapIndex), Character.BindLocation);
        }

        private void ChatBanButton_Click(object sender, EventArgs e)
        {
            if (Character?.Player == null) return;
            if (Character.AccountInfo.AdminAccount) return;

            Character.ChatBanned = true;

            DateTime date;

            DateTime.TryParse(ChatBanExpiryTextBox.Text, out date);

            Character.ChatBanExpiryDate = date;
        }

        private void ChatBanExpiryTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            DateTime temp;

            if (!DateTime.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
        }

        private void OpenAccountButton_Click(object sender, EventArgs e)
        {
            string accountId = Character.AccountInfo.AccountID;

            AccountInfoForm form = new AccountInfoForm(accountId, true);

            form.ShowDialog();
        }

        private void CurrentIPLabel_Click(object sender, EventArgs e)
        {
            string ipAddress = CurrentIPLabel.Text;

            string url = $"https://whatismyipaddress.com/ip/{ipAddress}";

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });

                CurrentIPLabel.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AccountBanButton_Click(object sender, EventArgs e)
        {
            if (Character.AccountInfo.AdminAccount) return;

            Character.AccountInfo.Banned = true;

            DateTime date;

            DateTime.TryParse(ChatBanExpiryTextBox.Text, out date);

            Character.AccountInfo.ExpiryDate = date;

            if (Character?.Player != null)
            {
                Character.Player.Connection.SendDisconnect(6);
            }
        }
        #endregion

        #region PlayerFlagSearch
        private List<ListViewItem> allFlagItems = new List<ListViewItem>();
        private void PopulatePlayerFlagsListView()
        {
            PlayerFlagsListView.Items.Clear();
            allFlagItems.Clear();

            for (int flagNumber = 1; flagNumber <= 1000; flagNumber++)
            {
                ListViewItem listItem = new ListViewItem(flagNumber.ToString());

                bool isFlagActive = flagNumber >= 0 && flagNumber < Character.Flags.Length && Character.Flags[flagNumber];

                listItem.SubItems.Add(isFlagActive ? "Active" : "Not Active");

                listItem.ForeColor = isFlagActive ? Color.Green : Color.Red;

                allFlagItems.Add(listItem);
            }

            FilterFlags();
        }
        private void FilterFlags()
        {
            PlayerFlagsListView.Items.Clear();

            foreach (var item in allFlagItems)
            {
                bool showItem = true;

                if (ActiveFlagsCheckBox.Checked && item.SubItems[1].Text != "Active")
                {
                    showItem = false;
                }

                if (!string.IsNullOrEmpty(FlagSearchBox.Text))
                {
                    if (int.TryParse(FlagSearchBox.Text, out int searchFlagNumber))
                    {
                        if (int.Parse(item.SubItems[0].Text) != searchFlagNumber)
                        {
                            showItem = false;
                        }
                    }
                    else
                    {
                        showItem = false;
                    }
                }

                if (showItem)
                {
                    PlayerFlagsListView.Items.Add(item);
                }
            }
        }

        private void ActiveFlagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            FilterFlags();
        }
        private void FlagSearchBox_TextChanged(object sender, EventArgs e)
        {
            FilterFlags();
        }
        private void OpenFlagsButton_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine("Envir", "SET [].txt");

            if (!File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 1; i <= 1999; i++)
                    {
                        writer.WriteLine($"[{i:D3}] -");
                    }
                }
            }

            Process.Start("notepad.exe", filePath);
        }
        private void EnableSelectedFlag_Click(object sender, EventArgs e)
        {
            if (PlayerFlagsListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = PlayerFlagsListView.SelectedItems[0];
                int flagIndex = int.Parse(selectedItem.Text);

                var result = MessageBox.Show("Are you sure you want to enable this flag?", "Confirm Action", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    if (flagIndex >= 0 && flagIndex < Character.Flags.Length)
                    {
                        Character.Flags[flagIndex] = true;

                        selectedItem.SubItems[1].Text = "Active";
                        selectedItem.SubItems[1].ForeColor = Color.Green;
                    }
                    else
                    {
                        MessageBox.Show("Invalid flag index.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a flag to enable.");
            }
        }
        private void DisableSelectedFlag_Click(object sender, EventArgs e)
        {
            if (PlayerFlagsListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = PlayerFlagsListView.SelectedItems[0];
                int flagIndex = int.Parse(selectedItem.Text);

                var result = MessageBox.Show("Are you sure you want to disable this flag?", "Confirm Action", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    if (flagIndex >= 0 && flagIndex < Character.Flags.Length)
                    {
                        Character.Flags[flagIndex] = false;

                        selectedItem.SubItems[1].Text = "Inactive";
                        selectedItem.SubItems[1].ForeColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("Invalid flag index.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a flag to disable.");
            }
        }
        #endregion

        #region UpdateTabs
        private void UpdateTabs()
        {
            if (Character == null)
            {
                Close();
                return;
            }

            UpdatePlayerInfo();
            PopulatePlayerFlagsListView();
            UpdatePetInfo();
            UpdatePlayerItems();
            UpdatePlayerMagics();
            UpdatePlayerQuests();
            UpdateHeroInfo();
        }
        #endregion

        #region Tab Resize
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: //Player
                    Size = new Size(703, 510);
                    break;
                case 1: //Quest
                    Size = new Size(423, 510);
                    break;
                case 2: //Item
                    Size = new Size(597, 510);
                    break;
                case 3: //Magic
                    Size = new Size(458, 510);
                    break;
                case 4: //Pet
                    Size = new Size(533, 510);
                    break;
                case 5: //Hero
                    Size = new Size(802, 510);
                    break;
            }

            UpdateTabs();
        }
        #endregion

        #region Hero List
        private void UpdateHeroInfo()
        {
            if (Character?.Player != null && Character.Player.Hero != null)
                {
                HeroNameTextBox.Text = Character.Player.Hero.Name;
                HeroLevelTextBox.Text = Character.Player.Hero.Level.ToString();
                HeroClassTextBox.Text = $"{Character.Player.Hero.Class}";

                HeroCurrentMapLabel.Text = $"{Character.Player.Hero.CurrentMap.Info.Title} / {Character.Player.Hero.CurrentMap.Info.FileName}";
                HeroCurrentXY.Text = $"X:{Character.Player.Hero.CurrentLocation.X}: Y:{Character.Player.Hero.CurrentLocation.Y}";

                HeroExpTextBox.Text = $"{string.Format("{0:#0.##%}", Character.Player.Hero.Experience / (double)Character.Player.Hero.MaxExperience)}";
                HeroACBox.Text = $"{Character.Player.Hero.Stats[Stat.MinAC]}-{Character.Player.Hero.Stats[Stat.MaxAC]}";
                HeroAMCBox.Text = $"{Character.Player.Hero.Stats[Stat.MinMAC]}-{Character.Player.Hero.Stats[Stat.MaxMAC]}";
                HeroDCBox.Text = $"{Character.Player.Hero.Stats[Stat.MinDC]}-{Character.Player.Hero.Stats[Stat.MaxDC]}";
                HeroMCBox.Text = $"{Character.Player.Hero.Stats[Stat.MinMC]}-{Character.Player.Hero.Stats[Stat.MaxMC]}";
                HeroSCBox.Text = $"{Character.Player.Hero.Stats[Stat.MinSC]}-{Character.Player.Hero.Stats[Stat.MaxSC]}";
                HeroACCBox.Text = $"{Character.Player.Hero.Stats[Stat.Accuracy]}";
                HeroAGILBox.Text = $"{Character.Player.Hero.Stats[Stat.Agility]}";
                HeroATKSPDBox.Text = $"{Character.Player.Hero.Stats[Stat.AttackSpeed]}";

                UpdateHeroMagic();
                UpdateHeroItems();
            }
            else
            {
                HeroCurrentMapLabel.Text = "OFFLINE";
                HeroCurrentXY.Text = "OFFLINE";
            }
        }
        private void UpdateHeroMagic()
        {
            HeroMagicList.Items.Clear();

            if (Character == null || Character.Heroes == null) return;

            foreach (HeroInfo hero in Character.Heroes)
            {
                if (hero == null) continue;

                foreach (UserMagic magic in hero.Magics)
                {
                    if (magic == null) continue;

                    ListViewItem listItem = new ListViewItem(magic.Info.Name.ToString()) { Tag = this };

                    listItem.SubItems.Add(magic.Level.ToString());

                    switch (magic.Level)
                    {
                        case 0:
                            listItem.SubItems.Add($"{magic.Experience}/{magic.Info.Need1}");
                            break;
                        case 1:
                            listItem.SubItems.Add($"{magic.Experience}/{magic.Info.Need2}");
                            break;
                        case 2:
                            listItem.SubItems.Add($"{magic.Experience}/{magic.Info.Need3}");
                            break;
                        case 3:
                            listItem.SubItems.Add("-");
                            break;
                    }

                    if (magic.Key > 8)
                    {
                        var key = magic.Key % 8;
                        listItem.SubItems.Add(string.Format("CTRL+F{0}", key != 0 ? key : 8));
                    }
                    else if (magic.Key > 0)
                    {
                        listItem.SubItems.Add(string.Format("F{0}", magic.Key));
                    }
                    else
                    {
                        listItem.SubItems.Add("No Key");
                    }

                    listItem.SubItems.Add(magic.Key.ToString());

                    HeroMagicList.Items.Add(listItem);
                }
            }
        }
        private void UpdateHeroItems()
        {
            HeroItemInfoListViewNF.Items.Clear();

            if (Character == null || Character.Heroes == null) return;

            HeroInfo selectedHero = Character.Heroes.FirstOrDefault();
            if (selectedHero == null) return;

            for (int i = 0; i < selectedHero.Inventory.Length; i++)
            {
                UserItem inventoryItem = selectedHero.Inventory[i];

                if (inventoryItem == null) continue;

                ListViewItem inventoryItemListItem = new ListViewItem($"{inventoryItem.UniqueID}");

                if (i < 6)
                {
                    inventoryItemListItem.SubItems.Add($"Belt | Slot: [{i + 1}]");
                }
                else if (i >= 6 && i < 46)
                {
                    inventoryItemListItem.SubItems.Add($"Inventory Bag I | Slot: [{i - 5}]");
                }
                else
                {
                    inventoryItemListItem.SubItems.Add($"Inventory Bag II | Slot: [{i - 45}]");
                }

                inventoryItemListItem.SubItems.Add($"{inventoryItem.FriendlyName}");
                inventoryItemListItem.SubItems.Add($"{inventoryItem.Count}/{inventoryItem.Info.StackSize}");
                inventoryItemListItem.SubItems.Add($"{inventoryItem.CurrentDura}/{inventoryItem.MaxDura}");

                HeroItemInfoListViewNF.Items.Add(inventoryItemListItem);
            }

            for (int i = 0; i < selectedHero.Equipment.Length; i++)
            {
                UserItem equipItem = selectedHero.Equipment[i];

                if (equipItem == null) continue;

                ListViewItem equipItemListItem = new ListViewItem($"{equipItem.UniqueID}");

                equipItemListItem.SubItems.Add($"Equipment | Slot: [{i + 1}]");

                equipItemListItem.SubItems.Add($"{equipItem.FriendlyName}");
                equipItemListItem.SubItems.Add($"{equipItem.Count}/{equipItem.Info.StackSize}");
                equipItemListItem.SubItems.Add($"{equipItem.CurrentDura}/{equipItem.MaxDura}");

                HeroItemInfoListViewNF.Items.Add(equipItemListItem);
            }
        }
        private void HeroUpdateButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Update?", "Update.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            HeroSaveChanges();
        }
        private void HeroSaveChanges()
        {
            if (Character == null || Character.Heroes == null) return;

            HeroInfo selectedHero = Character.Heroes.FirstOrDefault();
            if (selectedHero == null) return;

            selectedHero.Name = HeroNameTextBox.Text;
            selectedHero.Level = Convert.ToByte(HeroLevelTextBox.Text);

            UpdateTabs();
        }

        #endregion
    }
}