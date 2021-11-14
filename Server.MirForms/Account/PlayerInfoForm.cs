using Server.MirDatabase;
using Server.MirObjects;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Server.MirEnvir;

namespace Server
{
    public partial class PlayerInfoForm : Form
    {
        CharacterInfo Character = null;
        public ListView.SelectedListViewItemCollection QuestIndexChecked { get; private set; }

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
            QuestList.Items.Clear();
            IndexTextBox.Text = Character.Index.ToString();
            NameTextBox.Text = Character.Name;
            LevelTextBox.Text = Character.Level.ToString();

            GoldLabel.Text = $"{Character.AccountInfo.Gold:n0}";

            if (Character.Player != null)
                CurrentMapLabel.Text =
                    $"{Character.Player.CurrentMap.Info.FileName} {Character.CurrentLocation.X}:{Character.CurrentLocation.Y}";
            else
                CurrentMapLabel.Text = "OFFLINE";

            PKPointsLabel.Text = Character.PKPoints.ToString();
            CurrentIPLabel.Text = Character.AccountInfo.LastIP;
            OnlineTimeLabel.Text = Character.LastLoginDate > Character.LastLogoutDate ? (DateTime.Now - Character.LastLoginDate).TotalMinutes.ToString("##") + " minutes" : "Offline";

            ChatBanExpiryTextBox.Text = Character.ChatBanExpiryDate.ToString();

            foreach (var q in Character.CompletedQuests)
            {
                var quest = SMain.Envir.QuestInfoList.FirstOrDefault(x => x.Index == q);
                if (quest == null) continue;

                var itm = new ListViewItem($"{q.ToString()}") { Tag = this };
                itm.SubItems.Add("Completed");

                QuestList.Items.Add(itm);
            }

            foreach (var q in Character.CurrentQuests)
            {
                var itm = new ListViewItem($"{q.Index.ToString()}") { Tag = this };
                itm.SubItems.Add("Active");

                QuestList.Items.Add(itm);
            }
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
        private void button1_Click(object sender, EventArgs e)
        {
            {

                MirForms.PlayerItemForm form = new MirForms.PlayerItemForm();

                foreach (var i in Character.Equipment)
                {
                    if (i == null) continue;
                    ListViewItem tempItem = new ListViewItem(i.ItemIndex.ToString()) { Tag = this };

                    tempItem.SubItems.Add("Equipment");
                    tempItem.SubItems.Add(i.Name);
                    tempItem.SubItems.Add(i.Count.ToString());
                    tempItem.SubItems.Add(i.CurrentDura.ToString() + "/" + i.MaxDura.ToString());
                    tempItem.SubItems.Add(i.UniqueID.ToString());
                    form.PlayersItemListView.Items.Add(tempItem);
                }

                foreach (var i in Character.Inventory)
                {
                    if (i == null) continue;
                    ListViewItem tempItem = new ListViewItem(i.ItemIndex.ToString()) { Tag = this };

                    tempItem.SubItems.Add("Inventory");
                    tempItem.SubItems.Add(i.Name);
                    tempItem.SubItems.Add(i.Count.ToString());
                    tempItem.SubItems.Add(i.CurrentDura.ToString() + "/" + i.MaxDura.ToString());
                    tempItem.SubItems.Add(i.UniqueID.ToString());
                    form.PlayersItemListView.Items.Add(tempItem);
                }

                foreach (var i in Character.AccountInfo.Storage)
                {
                    if (i == null) continue;
                    ListViewItem tempItem = new ListViewItem(i.ItemIndex.ToString()) { Tag = this };

                    tempItem.SubItems.Add("Storage");
                    tempItem.SubItems.Add(i.Name);
                    tempItem.SubItems.Add(i.Count.ToString());
                    tempItem.SubItems.Add(i.CurrentDura.ToString() + "/" + i.MaxDura.ToString());
                    tempItem.SubItems.Add(i.UniqueID.ToString());
                    form.PlayersItemListView.Items.Add(tempItem);
                }

                foreach (var i in Character.QuestInventory)
                {
                    if (i == null) continue;
                    ListViewItem tempItem = new ListViewItem(i.ItemIndex.ToString()) { Tag = this };

                    tempItem.SubItems.Add("QuestInventory");
                    tempItem.SubItems.Add(i.Name);
                    tempItem.SubItems.Add(i.Count.ToString());
                    tempItem.SubItems.Add(i.CurrentDura.ToString() + "/" + i.MaxDura.ToString());
                    tempItem.SubItems.Add(i.UniqueID.ToString());
                    form.PlayersItemListView.Items.Add(tempItem);
                }

                foreach (var mail in Character.Mail)
                {
                    foreach (var i in mail.Items)
                    {
                        if (i == null) continue;
                        ListViewItem tempItem = new ListViewItem(i.ItemIndex.ToString()) { Tag = this };

                        tempItem.SubItems.Add("Mail");
                        tempItem.SubItems.Add(i.Name);
                        tempItem.SubItems.Add(i.Count.ToString());
                        tempItem.SubItems.Add(i.CurrentDura.ToString() + "/" + i.MaxDura.ToString());
                        tempItem.SubItems.Add(i.UniqueID.ToString());
                        form.PlayersItemListView.Items.Add(tempItem);
                    }
                }




                form.ShowDialog();

            }
        }

        private void QuestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuestIndexChecked = QuestList.SelectedItems;
        }

        private void DeleteQuestButton_Click(object sender, EventArgs e)
        {
            if (QuestIndexChecked == null) return;

            foreach (var item in QuestIndexChecked)
            {
                var listViewItem = item as ListViewItem;

                if (int.TryParse(listViewItem.SubItems[0].Text, out int questIdx))
                {
                    Character.CompletedQuests.RemoveAll(x => x == questIdx);
                    Character.CurrentQuests.RemoveAll(x => x.Index == questIdx);
                }

            }

            UpdatePlayerInfo();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                GGConfirmGroup.Enabled = true;
            }
            else
            {
                GGConfirmGroup.Enabled = false;
            }
        }


        private void ConfirmOrder_Click(object sender, EventArgs e)
        {
            CharacterInfo info = Character;

            DialogResult ConfirmMsg = MessageBox.Show(String.Format("Are you sure you want to send {0} GG to {1}?", GGAmountText.Text, info.Name), "Confirmation", MessageBoxButtons.YesNo);
            switch (ConfirmMsg)
            {
                case DialogResult.Yes:
                    if (GGAmountText.TextLength > 1)
                    {
                        Character.Player.GainCredit(Convert.ToUInt32(GGAmountText.Text));
                        MailInfo mail = new MailInfo(info.Index, false)
                        {
                            MailID = ++SMain.Envir.NextMailID,
                            Sender = "GameShop",
                            Message = String.Format("Your order has been verified.\n{0} GameGold has been added to your account.\n\nThank you for supporting the server!", GGAmountText.Text),
                        };
                        mail.Send();
                        SMain.Enqueue("[GameShop Order Completed]");
                        SMain.Enqueue("Amount Sent: " + GGAmountText.Text + "GG");
                        SMain.Enqueue("Player: " + info.Name);
                        GGConfirmGroup.Enabled = false;
                        GGAmountText.Text = "Order Completed";
                        checkBox1.Checked = false;
                    }
                    break;

                case DialogResult.No:
                    GGAmountText.Text = "Cancelled";
                    break;
            }
        }

        private void GGAmountText_TextChanged(object sender, EventArgs e)
        {
            SelectedGGLabel.Text = "Amount: " + GGAmountText.Text;
        }
    }
}
