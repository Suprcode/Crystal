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
