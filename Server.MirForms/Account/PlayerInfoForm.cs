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
        }

        private void UpdatePlayerInfo()
        {
            IndexTextBox.Text = Character.Index.ToString();
            NameTextBox.Text = Character.Name;
            LevelTextBox.Text = Character.Level.ToString();

            GoldLabel.Text = $"{Character.AccountInfo.Gold:n0}";

            if (Character.Player != null)
                CurrentMapLabel.Text =
                    $"{Character.Player.CurrentMap.Info.Title} {Character.Player.CurrentMap.Info.FileName} {Character.CurrentLocation.X}:{Character.CurrentLocation.Y}";
            else
                CurrentMapLabel.Text = "OFFLINE";

            PKPointsLabel.Text = Character.PKPoints.ToString();
            CurrentIPLabel.Text = Character.AccountInfo.LastIP;
            OnlineTimeLabel.Text = Character.LastLoginDate > Character.LastLogoutDate ? (SMain.Envir.Now - Character.LastLoginDate).TotalMinutes.ToString("##") + " minutes" : "Offline";

            ChatBanExpiryTextBox.Text = Character.ChatBanExpiryDate.ToString();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            CharacterInfo info = Character;

            info.Name = NameTextBox.Text;
            info.Level = Convert.ToByte(LevelTextBox.Text);
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

        private void FlagSearchBox_TextChanged(object sender, EventArgs e)
        {
            TextBox flagTextBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(flagTextBox.Text))
            {
                ResultLabel.Text = string.Empty;
                return;
            }

            if (int.TryParse(flagTextBox.Text, out int flagIndex))
            {
                if (flagIndex >= 0 && flagIndex < Character.Flags.Length)
                {
                    bool flagValue = Character.Flags[flagIndex];

                    if (flagValue)
                    {
                        ResultLabel.Text = $"Flag {flagIndex} is active.";
                        ResultLabel.ForeColor = Color.Green;
                    }
                    else
                    {
                        ResultLabel.Text = $"Flag {flagIndex} is inactive.";
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

        private void FlagUp_Click(object sender, EventArgs e)
        {
            if (int.TryParse(FlagSearchBox.Text, out int currentValue))
            {
                int newValue = currentValue + 1;

                FlagSearchBox.Text = newValue.ToString();
            }
            else
            {
                FlagSearchBox.Text = "1";
            }
        }

        private void FlagDown_Click(object sender, EventArgs e)
        {
            if (int.TryParse(FlagSearchBox.Text, out int currentValue))
            {
                int newValue = currentValue - 1;

                newValue = Math.Max(0, newValue);

                FlagSearchBox.Text = newValue.ToString();
            }
        }

        private void QuestSearchBox_TextChanged(object sender, EventArgs e)
        {
            TextBox questTextBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(questTextBox.Text))
            {
                QuestResultLabel.Text = string.Empty;
                return;
            }

            if (int.TryParse(questTextBox.Text, out int questID))
            {
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
                    QuestResultLabel.Text = $"Quest {questID} is {(isQuestComplete ? "Completed" : "Not Picked-up")}";
                    QuestResultLabel.ForeColor = isQuestComplete ? Color.Green : Color.Red;
                }
            }
            else
            {
                QuestResultLabel.Text = "Invalid Quest Index";
                QuestResultLabel.ForeColor = Color.Red;
            }
        }

        private void QuestUp_Click(object sender, EventArgs e)
        {
            if (int.TryParse(QuestSearchBox.Text, out int currentValue))
            {
                int newValue = currentValue + 1;

                QuestSearchBox.Text = newValue.ToString();
            }
            else
            {
                QuestSearchBox.Text = "1";
            }
        }

        private void QuestDown_Click(object sender, EventArgs e)
        {
            if (int.TryParse(QuestSearchBox.Text, out int currentValue))
            {
                int newValue = currentValue - 1;

                newValue = Math.Max(0, newValue);

                QuestSearchBox.Text = newValue.ToString();
            }
        }
    }
}