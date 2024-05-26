using CustomFormControl;
using Server.Account;
using Server.Database;
using Server.DropBuilder;
using Server.Library;
using Server.Library.MirDatabase;
using Server.Library.MirEnvir;
using Server.Library.MirObjects;
using Server.Systems;
using Shared;
using Shared.Data;

namespace Server {
    public partial class SMain : Form {
        public static Envir Envir => Envir.Main;

        public static Envir EditEnvir => Envir.Edit;

        protected static MessageQueue MessageQueue => MessageQueue.Instance;

        public SMain() {
            InitializeComponent();

            AutoResize();
        }

        private void AutoResize() {
            int columnCount = PlayersOnlineListView.Columns.Count;

            foreach(ColumnHeader column in PlayersOnlineListView.Columns) {
                column.Width = (PlayersOnlineListView.Width / (columnCount - 1)) - 1;
            }

            indexHeader.Width = 2;
        }

        public static void Enqueue(Exception ex) {
            MessageQueue.Enqueue(ex);
        }

        public static void EnqueueDebugging(string msg) {
            MessageQueue.EnqueueDebugging(msg);
        }

        public static void EnqueueChat(string msg) {
            MessageQueue.EnqueueChat(msg);
        }

        public static void Enqueue(string msg) {
            MessageQueue.Enqueue(msg);
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void InterfaceTimer_Tick(object sender, EventArgs e) {
            try {
                Text = $"Total: {Envir.LastCount}, Real: {Envir.LastRealCount}";
                PlayersLabel.Text = $"Players: {Envir.Players.Count}";
                MonsterLabel.Text = $"Monsters: {Envir.MonsterCount}";
                ConnectionsLabel.Text = $"Connections: {Envir.Connections.Count}";
                BlockedIPsLabel.Text = $"Blocked IPs: {Envir.IPBlocks.Count(x => x.Value > Envir.Now)}";
                UpTimeLabel.Text =
                    $"Uptime: {Envir.Stopwatch.ElapsedMilliseconds / 1000 / 60 / 60 / 24}d:{Envir.Stopwatch.ElapsedMilliseconds / 1000 / 60 / 60 % 24}h:{Envir.Stopwatch.ElapsedMilliseconds / 1000 / 60 % 60}m:{Envir.Stopwatch.ElapsedMilliseconds / 1000 % 60}s";

                if(Settings.Multithreaded && Envir.MobThreads != null) {
                    CycleDelayLabel.Text = $"CycleDelays: {Envir.LastRunTime:0000}";
                    for (int i = 0; i < Envir.MobThreads.Length; i++) {
                        if(Envir.MobThreads[i] == null) {
                            break;
                        }

                        CycleDelayLabel.Text = CycleDelayLabel.Text + $"|{Envir.MobThreads[i].LastRunTime:0000}";
                    }
                } else {
                    CycleDelayLabel.Text = $"CycleDelay: {Envir.LastRunTime}";
                }

                while (!MessageQueue.MessageLog.IsEmpty) {
                    string message;

                    if(!MessageQueue.MessageLog.TryDequeue(out message)) {
                        continue;
                    }

                    LogTextBox.AppendText(message);
                }

                while (!MessageQueue.DebugLog.IsEmpty) {
                    string message;

                    if(!MessageQueue.DebugLog.TryDequeue(out message)) {
                        continue;
                    }

                    DebugLogTextBox.AppendText(message);
                }

                while (!MessageQueue.ChatLog.IsEmpty) {
                    string message;

                    if(!MessageQueue.ChatLog.TryDequeue(out message)) {
                        continue;
                    }

                    ChatLogTextBox.AppendText(message);
                }

                ProcessPlayersOnlineTab(false);
                ProcessGuildViewTab(false);
            } catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private ListViewItem CreateListView(CharacterInfo character) {
            ListViewItem ListItem = new(character.Index.ToString()) { Tag = character };

            ListItem.SubItems.Add(character.Name);
            ListItem.SubItems.Add(character.Level.ToString());
            ListItem.SubItems.Add(character.Class.ToString());
            ListItem.SubItems.Add(character.Gender.ToString());

            return ListItem;
        }

        private void ProcessPlayersOnlineTab(bool forced = false) {
            if(PlayersOnlineListView.Items.Count != Envir.Players.Count || forced == true) {
                PlayersOnlineListView.Items.Clear();

                for (int i = PlayersOnlineListView.Items.Count; i < Envir.Players.Count; i++) {
                    CharacterInfo character = Envir.Players[i].Info;

                    ListViewItem tempItem = CreateListView(character);

                    PlayersOnlineListView.Items.Add(tempItem);
                }
            }
        }

        private void startServerToolStripMenuItem_Click(object sender, EventArgs e) {
            Envir.Start();
        }

        private void stopServerToolStripMenuItem_Click(object sender, EventArgs e) {
            Envir.Stop();
            Envir.MonsterCount = 0;
        }

        private void SMain_FormClosing(object sender, FormClosingEventArgs e) {
            Envir.Stop();
        }

        private void closeServerToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void itemInfoToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemInfoForm form = new();

            form.ShowDialog();
        }

        private void monsterInfoToolStripMenuItem_Click(object sender, EventArgs e) {
            MonsterInfoForm form = new();

            form.ShowDialog();
        }

        private void nPCInfoToolStripMenuItem_Click(object sender, EventArgs e) {
            NpcInfoForm form = new();

            form.ShowDialog();
        }

        private void balanceConfigToolStripMenuItem_Click(object sender, EventArgs e) {
            BalanceConfigForm form = new();

            form.ShowDialog();
        }

        private void questInfoToolStripMenuItem_Click(object sender, EventArgs e) {
            QuestInfoForm form = new();

            form.ShowDialog();
        }

        private void serverToolStripMenuItem_Click(object sender, EventArgs e) {
            ConfigForm form = new();

            form.ShowDialog();
        }

        private void balanceToolStripMenuItem_Click(object sender, EventArgs e) {
            BalanceConfigForm form = new();

            form.ShowDialog();
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e) {
            AccountInfoForm form = new();

            form.ShowDialog();
        }

        private void mapInfoToolStripMenuItem_Click(object sender, EventArgs e) {
            MapInfoForm form = new();

            form.ShowDialog();
        }

        private void itemInfoToolStripMenuItem_Click_1(object sender, EventArgs e) {
            ItemInfoForm form = new();

            form.ShowDialog();
        }

        private void monsterInfoToolStripMenuItem_Click_1(object sender, EventArgs e) {
            MonsterInfoForm form = new();

            form.ShowDialog();
        }

        private void nPCInfoToolStripMenuItem_Click_1(object sender, EventArgs e) {
            NpcInfoForm form = new();

            form.ShowDialog();
        }

        private void questInfoToolStripMenuItem_Click_1(object sender, EventArgs e) {
            QuestInfoForm form = new();

            form.ShowDialog();
        }

        private void dragonSystemToolStripMenuItem_Click(object sender, EventArgs e) {
            DragonInfoForm form = new();

            form.ShowDialog();
        }

        private void miningToolStripMenuItem_Click(object sender, EventArgs e) {
            MiningInfoForm form = new();

            form.ShowDialog();
        }

        private void guildsToolStripMenuItem_Click(object sender, EventArgs e) {
            GuildInfoForm form = new();

            form.ShowDialog();
        }

        private void fishingToolStripMenuItem_Click(object sender, EventArgs e) {
            SystemInfoForm form = new(0);

            form.ShowDialog();
        }

        private void GlobalMessageButton_Click(object sender, EventArgs e) {
            if(GlobalMessageTextBox.Text.Length < 1) {
                return;
            }

            foreach(PlayerObject player in Envir.Players) {
                player.ReceiveChat(GlobalMessageTextBox.Text, ChatType.Announcement);
            }

            EnqueueChat(GlobalMessageTextBox.Text);
            GlobalMessageTextBox.Text = string.Empty;
        }

        private void PlayersOnlineListView_DoubleClick(object sender, EventArgs e) {
            ListViewNF list = (ListViewNF)sender;

            if(list.SelectedItems.Count > 0) {
                ListViewItem item = list.SelectedItems[0];
                string index = item.SubItems[0].Text;

                PlayerInfoForm form = new(Convert.ToUInt32(index));

                form.ShowDialog();
            }
        }

        private void PlayersOnlineListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e) {
            e.Cancel = true;
            e.NewWidth = PlayersOnlineListView.Columns[e.ColumnIndex].Width;
        }

        private void mailToolStripMenuItem_Click(object sender, EventArgs e) {
            SystemInfoForm form = new(1);

            form.ShowDialog();
        }

        private void goodsToolStripMenuItem_Click(object sender, EventArgs e) {
            SystemInfoForm form = new(2);

            form.ShowDialog();
        }

        private void relationshipToolStripMenuItem_Click(object sender, EventArgs e) {
            SystemInfoForm form = new(4);

            form.ShowDialog();
        }

        private void refiningToolStripMenuItem_Click(object sender, EventArgs e) {
            SystemInfoForm form = new(3);

            form.ShowDialog();
        }

        private void mentorToolStripMenuItem_Click(object sender, EventArgs e) {
            SystemInfoForm form = new(5);

            form.ShowDialog();
        }

        private void magicInfoToolStripMenuItem_Click(object sender, EventArgs e) {
            MagicInfoForm form = new();
            form.ShowDialog();
        }

        private void SMain_Load(object sender, EventArgs e) {
            bool loaded = EditEnvir.LoadDB();

            if(loaded) {
                Envir.Start();
            }

            AutoResize();
        }

        private void gemToolStripMenuItem_Click(object sender, EventArgs e) {
            SystemInfoForm form = new(6);

            form.ShowDialog();
        }

        private void conquestToolStripMenuItem_Click(object sender, EventArgs e) {
            ConquestInfoForm form = new();

            form.ShowDialog();
        }

        private void rebootServerToolStripMenuItem_Click(object sender, EventArgs e) {
            Envir.Reboot();
        }

        private void respawnsToolStripMenuItem_Click(object sender, EventArgs e) {
            SystemInfoForm form = new(7);

            form.ShowDialog();
        }

        private void monsterTunerToolStripMenuItem_Click(object sender, EventArgs e) {
            if(!Envir.Running) {
                MessageBox.Show("Server must be running to tune monsters", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            MonsterTunerForm form = new();

            form.ShowDialog();
        }

        private void gameshopToolStripMenuItem_Click(object sender, EventArgs e) {
            GameShop form = new();
            form.ShowDialog();
        }

        private void itemNEWToolStripMenuItem_Click(object sender, EventArgs e) {
            ItemInfoFormNew form = new();

            form.ShowDialog();
        }

        private void monsterExperimentalToolStripMenuItem_Click(object sender, EventArgs e) {
            MonsterInfoFormNew form = new();

            form.ShowDialog();
        }

        private void dropBuilderToolStripMenuItem_Click(object sender, EventArgs e) {
            DropGenForm GenForm = new();

            GenForm.ShowDialog();
        }

        private void clearBlockedIPsToolStripMenuItem_Click(object sender, EventArgs e) {
            Envir.IPBlocks.Clear();
        }

        private void nPCsToolStripMenuItem_Click(object sender, EventArgs e) {
            Envir.ReloadNpcs();
        }

        private void dropsToolStripMenuItem_Click(object sender, EventArgs e) {
            Envir.ReloadDrops();
        }

        private void lineMessageToolStripMenuItem_Click(object sender, EventArgs e) {
            Envir.ReloadLineMessages();
        }

        #region Guild View Tab

        public void ProcessGuildViewTab(bool forced = false) {
            if(GuildListView.Items.Count != Envir.GuildList.Count || forced == true) {
                GuildListView.Items.Clear();

                foreach(GuildInfo guild in Envir.GuildList) {
                    ListViewItem tempItem = new(guild.GuildIndex.ToString()) { Tag = this };

                    tempItem.SubItems.Add(guild.Name);

                    if(guild.Ranks.Count > 0 && guild.Ranks[0].Members.Count > 0) {
                        tempItem.SubItems.Add(guild.Ranks[0].Members[0].Name);
                    } else {
                        tempItem.SubItems.Add("DELETED");
                        tempItem.ForeColor = Color.Red;
                    }

                    tempItem.SubItems.Add(guild.Membercount.ToString());
                    tempItem.SubItems.Add(guild.Level.ToString());
                    tempItem.SubItems.Add($"{guild.Gold}");

                    GuildListView.Items.Add(tempItem);
                }
            }
        }

        private void GuildListView_DoubleClick(object sender, EventArgs e) {
            ListViewNF list = (ListViewNF)sender;

            if(list.SelectedItems.Count <= 0) {
                return;
            }

            ListViewItem item = list.SelectedItems[0];
            int index = Int32.Parse(item.Text);

            GuildObject Guild = Envir.GetGuild(index);

            GuildItemForm form = new() {
                GuildName = Guild.Name,
                main = this
            };

            if(Guild == null) {
                return;
            }

            foreach(GuildStorageItem i in Guild.StoredItems) {
                if(i == null) {
                    continue;
                }

                ListViewItem tempItem = new(i.Item.UniqueID.ToString()) { Tag = this };

                CharacterInfo character = Envir.GetCharacterInfo((int)i.UserId);
                if(character != null) {
                    tempItem.SubItems.Add(character.Name);
                } else if(i.UserId == -1) {
                    tempItem.SubItems.Add("Server");
                } else {
                    tempItem.SubItems.Add("Unknown");
                }

                tempItem.SubItems.Add(i.Item.FriendlyName);
                tempItem.SubItems.Add(i.Item.Count.ToString());
                tempItem.SubItems.Add(i.Item.CurrentDura + "/" + i.Item.MaxDura);

                form.GuildItemListView.Items.Add(tempItem);
            }

            foreach(GuildRank r in Guild.Ranks)
            foreach(GuildMember m in r.Members) {
                ListViewItem tempItem = new(m.Name) { Tag = this };
                tempItem.SubItems.Add(r.Name);
                form.MemberListView.Items.Add(tempItem);
            }

            form.ShowDialog();
        }

        #endregion

        private void MainTabs_SelectedIndexChanged(object sender, EventArgs e) {
            ProcessPlayersOnlineTab(true);
            ProcessGuildViewTab(true);
        }
    }
}
