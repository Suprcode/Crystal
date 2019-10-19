using Server.MirDatabase;
using Server.MirObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

            GoldLabel.Text = String.Format("{0:n0}", Character.AccountInfo.Gold);

            if (Character.Player != null)
                CurrentMapLabel.Text = string.Format("{0} {1}:{2}", Character.Player.CurrentMap.Info.FileName, Character.CurrentLocation.X, Character.CurrentLocation.Y);
            else
                CurrentMapLabel.Text = "OFFLINE";

            PKPointsLabel.Text = Character.PKPoints.ToString();
            CurrentIPLabel.Text = Character.AccountInfo.LastIP;
            OnlineTimeLabel.Text = (DateTime.Now - Character.LastDate).TotalMinutes.ToString("##") + " minutes";

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

    }
}
