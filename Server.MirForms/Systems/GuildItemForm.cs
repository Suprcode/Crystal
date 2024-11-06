using Server.MirObjects;
using Server.MirObjects.Monsters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Systems
{
    public partial class GuildItemForm : Form
    {
        public GuildObject Guild { get; set; }
        public string GuildName;
        public SMain main;

        public GuildItemForm()
        {
            InitializeComponent();
            this.Load += GuildItemForm_Load;
        }

        #region Load Guild Notice
        public void SetGuildNotice(List<string> notice)
        {
            GuildNoticeBox.Text = string.Join(Environment.NewLine, notice);
        }
        #endregion

        #region Load Member Count
        public void SetMemberCount(int memberCount, int memberCap)
        {
            MemberCountLabel.Text = $"Members: {memberCount}/{memberCap}";
        }
        #endregion

        #region Load Guild Points
        public void SetGuildPoints(byte sparePoints)
        {
            GuildPointsLabel.Text = $"Points: {sparePoints}";
        }
        #endregion

        #region Load Guild EXP
        public void SetGuildExperience(long experience)
        {
            GuildEXPLabel.Text = $"EXP: {experience}";
        }
        #endregion

        #region Load Guild Ranks
        public void SetGuildRanks(List<GuildRank> ranks)
        {
            GuildRanksListView.Items.Clear();

            foreach (var rank in ranks)
            {
                ListViewItem item = new ListViewItem(rank.Name);
                GuildRanksListView.Items.Add(item);
            }
        }
        #endregion

        #region Load Guild Chat
        public void LoadGuildChat()
        {
            if (main == null || Guild == null) return;

            GuildChatBox.Clear();

            string[] chatLogLines = main.ChatLogTextBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var line in chatLogLines)
            {
                if (line.Contains($"SYSTEM to Guild: '{GuildName}':"))
                {
                    GuildChatBox.AppendText(line + Environment.NewLine);
                    continue;
                }

                int guildMessageIndex = line.IndexOf(": !~");
                if (guildMessageIndex > -1)
                {
                    int playerNameStart = line.IndexOf("]: ") + 3;
                    int playerNameEnd = line.IndexOf(":", playerNameStart);

                    if (playerNameStart > 0 && playerNameEnd > playerNameStart)
                    {
                        string playerName = line.Substring(playerNameStart, playerNameEnd - playerNameStart).Trim();

                        if (Guild.Ranks.Any(rank => rank.Members.Any(member => member.Name == playerName)))
                        {
                            GuildChatBox.AppendText(line + Environment.NewLine);
                        }
                    }
                }
            }
        }
        #endregion

        #region Buff List
        public void SetBuffList(List<GuildBuff> activeBuffs, List<GuildBuffInfo> allBuffInfos)
        {
            BuffListView.Items.Clear();

            // Dictionary to quickly check if a buff is active
            var activeBuffsById = activeBuffs.ToDictionary(buff => buff.Id);

            foreach (var buffInfo in allBuffInfos)
            {
                ListViewItem item = new ListViewItem(buffInfo.Id.ToString());

                // Display the name of the buff
                item.SubItems.Add(buffInfo.Name);

                // Check if this buff is active
                if (activeBuffsById.TryGetValue(buffInfo.Id, out GuildBuff activeBuff))
                {
                    // Buff is active
                    item.SubItems.Add("Active");
                    item.SubItems.Add(activeBuff.ActiveTimeRemaining.ToString());
                }
                else
                {
                    // Buff is inactive
                    item.SubItems.Add("Inactive");
                    item.SubItems.Add("0"); // No time remaining for inactive buff
                }

                BuffListView.Items.Add(item);
            }
        }
        #endregion

        #region Delete Button
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (MemberListView == null) return;
            if (MemberListView.SelectedItems == null) return;

            Server.MirObjects.GuildObject Guild = SMain.Envir.GetGuild(GuildName);
            if (Guild == null) return;

            foreach (var m in MemberListView.SelectedItems)
            {
                var lm = (ListViewItem)m;

                Guild.DeleteMember(lm.SubItems[0].Text);
                MemberListView.Items.Remove(lm);
                main.ProcessGuildViewTab();
                break;
            }
        }
        #endregion

        #region Update Guild Notice
        private void RefreshNoticeButton_Click(object sender, EventArgs e)
        {
            var guild = SMain.Envir.GetGuild(GuildName);
            if (guild == null) return;

            List<string> newNotice = GuildNoticeBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            // Log the guild notice update
            string noticeUpdateLog = $"Guild: '{GuildName}' notice changed by SYSTEM.";
            SMain.EnqueueChat(noticeUpdateLog);

            Logger.GetLogger(LogType.Server).Info(noticeUpdateLog);

            guild.NewNotice(newNotice);

            SetGuildNotice(guild.Info.Notice);
        }
        #endregion

        #region Send Guild Message
        private void SendGuildMessageButton_Click(object sender, EventArgs e)
        {
            var guild = SMain.Envir.GetGuild(GuildName);
            if (guild == null) return;

            string message = SendGuildMesageBox.Text.Trim();

            if (string.IsNullOrEmpty(message)) return;

            guild.SendMessage($"SYSTEM: {message}", ChatType.Guild);

            string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            GuildChatBox.AppendText($"[{timestamp}]: SYSTEM: {message}" + Environment.NewLine);

            string logMessage = $"SYSTEM to Guild: '{GuildName}': {message}";
            SMain.EnqueueChat(logMessage);

            SendGuildMesageBox.Clear();
        }
        #endregion

        #region Load Form
        private void GuildItemForm_Load(object sender, EventArgs e)
        {
            LoadGuildChat();
        }
        #endregion
    }
}