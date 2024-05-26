using Server.Library.MirObjects;

namespace Server.Systems {
    public partial class GuildItemForm : Form {
        public string GuildName;
        public SMain main;

        public GuildItemForm() {
            InitializeComponent();
        }

        #region Delete Button

        private void DeleteButton_Click(object sender, EventArgs e) {
            if(MemberListView == null) {
                return;
            }

            if(MemberListView.SelectedItems == null) {
                return;
            }

            GuildObject Guild = SMain.Envir.GetGuild(GuildName);
            if(Guild == null) {
                return;
            }

            foreach(object m in MemberListView.SelectedItems) {
                ListViewItem lm = (ListViewItem)m;

                Guild.DeleteMember(lm.SubItems[0].Text);
                MemberListView.Items.Remove(lm);
                main.ProcessGuildViewTab();
                break;
            }
        }

        #endregion
    }
}
