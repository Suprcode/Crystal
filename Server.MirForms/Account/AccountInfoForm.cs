using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Server.MirDatabase;
using Server.MirEnvir;

namespace Server
{
    public partial class AccountInfoForm : Form
    {
        private List<AccountInfo> _selectedAccountInfos;

        public AccountInfoForm()
        {
            InitializeComponent();

            Setup();
        }

        public AccountInfoForm(string accountId, bool match = false)
        {
            InitializeComponent();

            FilterTextBox.Text = accountId;
            MatchFilterCheckBox.Checked = match;

            Setup();
        }

        private void Setup()
        {
            RefreshInterface();
            AutoResize();

            AccountIDTextBox.MaxLength = Globals.MaxAccountIDLength;
            PasswordTextBox.MaxLength = Globals.MaxPasswordLength;

            UserNameTextBox.MaxLength = 20;
            BirthDateTextBox.MaxLength = 10;
            QuestionTextBox.MaxLength = 30;
            AnswerTextBox.MaxLength = 30;
            EMailTextBox.MaxLength = 50;
        }

        private void AutoResize()
        {
            indexHeader.Width = -2;
            accountIDHeader.Width = -2;
            passwordHeader.Width = -2;
            userNameHeader.Width = -2;
            bannedHeader.Width = -2;
            banReasonHeader.Width = -2;
            expiryDateHeader.Width = -2;
        }

        public void RefreshInterface()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(RefreshInterface));
                return;
            }

            List<AccountInfo> accounts = SMain.Envir.AccountList;

            if(FilterTextBox.Text.Length > 0)
                accounts = SMain.Envir.MatchAccounts(FilterTextBox.Text, MatchFilterCheckBox.Checked);

            else if(FilterPlayerTextBox.Text.Length > 0)
                accounts = SMain.Envir.MatchAccountsByPlayer(FilterPlayerTextBox.Text, MatchFilterCheckBox.Checked);

            if (AccountInfoListView.Items.Count != accounts.Count)
            {
                AccountInfoListView.SelectedIndexChanged -= AccountInfoListView_SelectedIndexChanged;
                AccountInfoListView.Items.Clear();
                for (int i = AccountInfoListView.Items.Count; i < accounts.Count; i++)
                {
                    AccountInfo account = accounts[i];

                    ListViewItem tempItem = account.CreateListView();

                    AccountInfoListView.Items.Add(tempItem);
                }
                AccountInfoListView.SelectedIndexChanged += AccountInfoListView_SelectedIndexChanged;
            }

            _selectedAccountInfos = new List<AccountInfo>();


            for (int i = 0; i < AccountInfoListView.SelectedItems.Count; i++)
                _selectedAccountInfos.Add(AccountInfoListView.SelectedItems[i].Tag as AccountInfo);



            if (_selectedAccountInfos.Count == 0)
            {
                AccountInfoPanel.Enabled = false;

                AccountIDTextBox.Text = string.Empty;
                PasswordTextBox.Text = string.Empty;

                UserNameTextBox.Text = string.Empty;
                BirthDateTextBox.Text = string.Empty;
                QuestionTextBox.Text = string.Empty;
                AnswerTextBox.Text = string.Empty;
                EMailTextBox.Text = string.Empty;
                return;
            }


            AccountInfo info = _selectedAccountInfos[0];

            AccountInfoPanel.Enabled = true;

            AccountIDTextBox.Enabled = _selectedAccountInfos.Count == 1;
            AccountIDTextBox.Text = info.AccountID;
            PasswordTextBox.Text = info.Password;

            UserNameTextBox.Text = info.UserName;
            BirthDateTextBox.Text = info.BirthDate.ToShortDateString();
            QuestionTextBox.Text = info.SecretQuestion;
            AnswerTextBox.Text = info.SecretAnswer;
            EMailTextBox.Text = info.EMailAddress;

            CreationIPTextBox.Text = info.CreationIP;
            CreationDateTextBox.Text = info.CreationDate.ToString();
            
            LastIPTextBox.Text = info.LastIP;
            LastDateTextBox.Text = info.LastDate.ToString();

            BanReasonTextBox.Text = info.BanReason;
            BannedCheckBox.CheckState = info.Banned ? CheckState.Checked : CheckState.Unchecked;
            ExpiryDateTextBox.Text = info.ExpiryDate.ToString();
            AdminCheckBox.CheckState = info.AdminAccount ? CheckState.Checked : CheckState.Unchecked;

            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                info = _selectedAccountInfos[i];

                if (AccountIDTextBox.Text != info.AccountID) AccountIDTextBox.Text = string.Empty;
                if (PasswordTextBox.Text != info.Password) PasswordTextBox.Text = string.Empty;
                if (UserNameTextBox.Text != info.UserName) UserNameTextBox.Text = string.Empty;
                if (BirthDateTextBox.Text != info.BirthDate.ToShortDateString()) BirthDateTextBox.Text = string.Empty;
                if (QuestionTextBox.Text != info.SecretQuestion) QuestionTextBox.Text = string.Empty;
                if (AnswerTextBox.Text != info.SecretAnswer) AnswerTextBox.Text = string.Empty;
                if (EMailTextBox.Text != info.EMailAddress) EMailTextBox.Text = string.Empty;

                if (CreationIPTextBox.Text != info.CreationIP) CreationIPTextBox.Text = string.Empty;
                if (CreationDateTextBox.Text != info.CreationDate.ToString()) CreationDateTextBox.Text = string.Empty;


                if (LastIPTextBox.Text != info.LastIP) LastIPTextBox.Text = string.Empty;
                if (LastDateTextBox.Text != info.LastDate.ToString()) LastDateTextBox.Text = string.Empty;


                if (BanReasonTextBox.Text != info.BanReason) BanReasonTextBox.Text = string.Empty;
                if (BannedCheckBox.Checked != info.Banned) BannedCheckBox.CheckState = CheckState.Indeterminate;
                if (ExpiryDateTextBox.Text != info.ExpiryDate.ToString()) ExpiryDateTextBox.Text = string.Empty;
                if (AdminCheckBox.Checked != info.AdminAccount) AdminCheckBox.CheckState = CheckState.Indeterminate;
            }
        }

        private void AccountInfoListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshInterface();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            lock (Envir.AccountLock)
            {
                SMain.Envir.CreateAccountInfo();
                RefreshInterface();
            }
        }

        private void AccountIDTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (_selectedAccountInfos.Count != 1) return;

            lock (Envir.AccountLock)
            {
                if (SMain.Envir.AccountExists(ActiveControl.Text))
                {
                    ActiveControl.BackColor = Color.Red;
                    return;
                }
                AccountInfoListView.BeginUpdate();

                ActiveControl.BackColor = SystemColors.Window;
                _selectedAccountInfos[0].AccountID = ActiveControl.Text;
                _selectedAccountInfos[0].Update();

                AutoResize();
                AccountInfoListView.EndUpdate();
            }
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            AccountInfoListView.BeginUpdate();
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                _selectedAccountInfos[i].Password = ActiveControl.Text;
                _selectedAccountInfos[i].Update();
            }

            AutoResize();
            AccountInfoListView.EndUpdate();
        }

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            AccountInfoListView.BeginUpdate();
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                _selectedAccountInfos[i].UserName = ActiveControl.Text;
                _selectedAccountInfos[i].Update();
            }

            AutoResize();
            AccountInfoListView.EndUpdate();
        }

        private void BirthDateTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            DateTime temp;

            if (!DateTime.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedAccountInfos.Count; i++)
                _selectedAccountInfos[i].BirthDate = temp;
        }

        private void QuestionTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
                _selectedAccountInfos[i].SecretQuestion = ActiveControl.Text;
        }

        private void AnswerTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedAccountInfos.Count; i++)
                _selectedAccountInfos[i].SecretAnswer = ActiveControl.Text;
        }

        private void EMailTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;


            for (int i = 0; i < _selectedAccountInfos.Count; i++)
                _selectedAccountInfos[i].EMailAddress = ActiveControl.Text;
        }
        
        private void DayBanButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to ban the selected Accounts?", "Ban Selected.", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) return;

            DateTime expiry = SMain.Envir.Now.AddDays(1);

            AccountInfoListView.BeginUpdate();
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                _selectedAccountInfos[i].Banned = true;
                _selectedAccountInfos[i].ExpiryDate = expiry;
                _selectedAccountInfos[i].Update();
            }

            RefreshInterface();
            AutoResize();
            AccountInfoListView.EndUpdate();
        }

        private void WeekBanButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to ban the selected Accounts?", "Ban Selected.", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) return;

            DateTime expiry = SMain.Envir.Now.AddDays(7);

            AccountInfoListView.BeginUpdate();
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                _selectedAccountInfos[i].Banned = true;
                _selectedAccountInfos[i].ExpiryDate = expiry;
                _selectedAccountInfos[i].Update();
            }

            RefreshInterface();
            AutoResize();
            AccountInfoListView.EndUpdate();
        }

        private void PermBanButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to ban the selected Accounts?", "Ban Selected.", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) return;


            AccountInfoListView.BeginUpdate();
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                _selectedAccountInfos[i].Banned = true;
                _selectedAccountInfos[i].ExpiryDate = DateTime.MaxValue;
                _selectedAccountInfos[i].Update();
            }

            RefreshInterface();
            AutoResize();
            AccountInfoListView.EndUpdate();
        }

        private void BannedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            AccountInfoListView.BeginUpdate();
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                _selectedAccountInfos[i].Banned = false;
                _selectedAccountInfos[i].Update();
            }
            AutoResize();
            AccountInfoListView.EndUpdate();
        }

        private void BanReasonTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            
            AccountInfoListView.BeginUpdate();
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                _selectedAccountInfos[i].BanReason = ActiveControl.Text;
                _selectedAccountInfos[i].Update();
            }

            AutoResize();
            AccountInfoListView.EndUpdate();
        }

        private void ExpiryDateTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            DateTime temp;

            if (!DateTime.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            AccountInfoListView.BeginUpdate();
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                _selectedAccountInfos[i].ExpiryDate = temp;
                _selectedAccountInfos[i].Update();
            }

            AutoResize();
            AccountInfoListView.EndUpdate();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshInterface();
        }

        private void AccountInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SMain.Envir.Running) return;

            SMain.Envir.SaveAccounts();
        }

        private void AdminCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            AccountInfoListView.BeginUpdate();
            for (int i = 0; i < _selectedAccountInfos.Count; i++)
            {
                _selectedAccountInfos[i].AdminAccount = AdminCheckBox.CheckState == CheckState.Checked ? true : false;
                _selectedAccountInfos[i].Update();
            }
            AutoResize();
            AccountInfoListView.EndUpdate();
        }

        private void WipeCharButton_Click(object sender, EventArgs e)
        {
            if (SMain.Envir.Running)
            {
                MessageBox.Show("Cannot wipe characters whilst the server is running", "Notice",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (MessageBox.Show("Are you sure you want to wipe all characters from the database?", "Notice",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                for (int i = 0; i < SMain.Envir.AccountList.Count; i++)
                {
                    AccountInfo account = SMain.Envir.AccountList[i];

                    account.Characters.Clear();
                }

                SMain.Envir.Auctions.Clear();
                SMain.Envir.GuildList.Clear();

                MessageBox.Show("All characters and associated data has been cleared", "Notice",
               MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
