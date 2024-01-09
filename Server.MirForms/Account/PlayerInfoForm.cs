using Server.MirDatabase;
using Server.MirObjects;

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

            UpdatePlayerInfo();
            UpdatePetInfo();
        }

        private void UpdatePlayerInfo()
        {
            IndexTextBox.Text = Character.Index.ToString();
            NameTextBox.Text = Character.Name;
            LevelTextBox.Text = Character.Level.ToString();
            ExpTextBox.Text = Character.Experience.ToString();
            PKPointsTextBox.Text = Character.PKPoints.ToString();
            GoldTextBox.Text = $"{Character.AccountInfo.Gold:n0}";
            GameGoldTextBox.Text = String.Format("{0:n0}", Character.AccountInfo.Credit);

            ACBox.Text = $"{Character.Player.Stats[Stat.MinAC]}-{Character.Player.Stats[Stat.MaxAC]}";
            AMCBox.Text = $"{Character.Player.Stats[Stat.MinMAC]}-{Character.Player.Stats[Stat.MaxMAC]}";
            DCBox.Text = $"{Character.Player.Stats[Stat.MinDC]}-{Character.Player.Stats[Stat.MaxDC]}";
            MCBox.Text = $"{Character.Player.Stats[Stat.MinMC]}-{Character.Player.Stats[Stat.MaxMC]}";
            SCBox.Text = $"{Character.Player.Stats[Stat.MinSC]}-{Character.Player.Stats[Stat.MaxSC]}";
            ACCBox.Text = $"{Character.Player.Stats[Stat.Accuracy]}";
            AGILBox.Text = $"{Character.Player.Stats[Stat.Agility]}";
            ATKSPDBox.Text = $"{Character.Player.Stats[Stat.AttackSpeed]}";

            if (Character.Player != null)
                CurrentMapLabel.Text =
                    $"{Character.Player.CurrentMap.Info.Title} {Character.Player.CurrentMap.Info.FileName} {Character.CurrentLocation.X}:{Character.CurrentLocation.Y}";
            else
                CurrentMapLabel.Text = "OFFLINE";

            CurrentIPLabel.Text = Character.AccountInfo.LastIP;
            OnlineTimeLabel.Text = Character.LastLoginDate > Character.LastLogoutDate ? (SMain.Envir.Now - Character.LastLoginDate).TotalMinutes.ToString("##") + " minutes" : "Offline";

            ChatBanExpiryTextBox.Text = Character.ChatBanExpiryDate.ToString();
        }

        private void UpdatePetInfo()
        {
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

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            CharacterInfo info = Character;

            string tempGold = GoldTextBox.Text.Replace(",", "");
            string tempCredit = GameGoldTextBox.Text.Replace(",", "");
            string tempEXP = ExpTextBox.Text.Replace(",", "");

            info.Name = NameTextBox.Text;
            info.Level = Convert.ToByte(LevelTextBox.Text);
            info.Experience = Convert.ToInt64(tempEXP);
            info.PKPoints = Convert.ToInt32(PKPointsTextBox.Text);
            info.AccountInfo.Gold = Convert.ToUInt32(tempGold);
            info.AccountInfo.Credit = Convert.ToUInt32(tempCredit);
        }

        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            if (Character.Player == null) return;

            if (SendMessageTextBox.Text.Length < 1) return;

            Character.Player.ReceiveChat(SendMessageTextBox.Text, ChatType.Announcement);
        }

        private void KickButton_Click(object sender, EventArgs e)
        {
            if (Character.Player == null) return;

            Character.Player.Connection.SendDisconnect(4);
            //also update account so player can't log back in for x minutes?
        }

        private void KillButton_Click(object sender, EventArgs e)
        {
            if (Character.Player == null) return;

            Character.Player.Die();
        }

        private void KillPetsButton_Click(object sender, EventArgs e)
        {
            if (Character.Player == null) return;

            for (int i = Character.Player.Pets.Count - 1; i >= 0; i--)
                Character.Player.Pets[i].Die();

            ClearPetInfo();
        }
        private void SafeZoneButton_Click(object sender, EventArgs e)
        {
            Character.Player.Teleport(SMain.Envir.GetMap(Character.BindMapIndex), Character.BindLocation);
        }

        private void ChatBanButton_Click(object sender, EventArgs e)
        {
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

        private void QuestSearchBox_ValueChanged(object sender, EventArgs e)
        {
            int questID = 0;

            if (string.IsNullOrWhiteSpace(QuestSearchBox.Value.ToString()))
            {
                QuestResultLabel.Text = string.Empty;
                return;
            }
            else
            {
                questID = Decimal.ToInt32(QuestSearchBox.Value);
            }

            if (questID < 1)
            {
                QuestResultLabel.Text = "Invalid Quest Index";
                QuestResultLabel.ForeColor = Color.Red;
                return;
            }

            bool isQuestActive = Character.CurrentQuests.Any(x => x.Index == questID);
            bool isQuestComplete = Character.CompletedQuests.Contains(questID);

            if (isQuestActive)
            {
                QuestResultLabel.Text = $"Quest {questID} is Active";
                QuestResultLabel.ForeColor = Color.Blue;
            }
            else
            {
                QuestResultLabel.Text = $"Quest {questID} is {(isQuestComplete ? "Completed" : "Inactive")}";
                QuestResultLabel.ForeColor = isQuestComplete ? Color.Green : Color.Red;
            }
        }

        private void FlagSearchBox_ValueChanged(object sender, EventArgs e)
        {
            int flagIndex = 0;
            if (string.IsNullOrWhiteSpace(FlagSearchBox.Value.ToString()))
            {
                ResultLabel.Text = string.Empty;
                return;
            }
            else
            {
                flagIndex = Decimal.ToInt32(FlagSearchBox.Value);
            }

            if (flagIndex >= 0 && flagIndex < Character.Flags.Length)
            {
                bool flagValue = Character.Flags[flagIndex];

                if (flagValue)
                {
                    ResultLabel.Text = $"Flag {flagIndex} is Active";
                    ResultLabel.ForeColor = Color.Green;
                }
                else
                {
                    ResultLabel.Text = $"Flag {flagIndex} is Inactive";
                    ResultLabel.ForeColor = Color.Red;
                }
            }
            else
            {
                ResultLabel.Text = "Invalid Flag Number";
                ResultLabel.ForeColor = Color.Red;
            }
        }
    }
}