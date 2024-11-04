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
        public string GuildName;
        public SMain main;

        public GuildItemForm()
        {
            InitializeComponent();
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
    }
}
