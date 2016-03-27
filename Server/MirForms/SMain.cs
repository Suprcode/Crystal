using System;
using System.Collections.Concurrent;
using System.Windows.Forms;
using Server.MirEnvir;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using Server.MirDatabase;
using Server.MirForms.Systems;

namespace Server
{
    public partial class SMain : Form
    {
        public static readonly Envir Envir = new Envir(), EditEnvir = new Envir();
        private static readonly ConcurrentQueue<string> MessageLog = new ConcurrentQueue<string>();
        private static readonly ConcurrentQueue<string> DebugLog = new ConcurrentQueue<string>();
        private static readonly ConcurrentQueue<string> ChatLog = new ConcurrentQueue<string>();

        public SMain()
        {
                InitializeComponent();

                AutoResize();
        }

        private void AutoResize()
        {
            int columnCount = PlayersOnlineListView.Columns.Count;

            foreach (ColumnHeader column in PlayersOnlineListView.Columns)
            {
                column.Width = PlayersOnlineListView.Width / (columnCount - 1) - 1;
            }

            indexHeader.Width = 2;
        }

        public static void Enqueue(Exception ex)
        {
            if (MessageLog.Count < 100)
            MessageLog.Enqueue(String.Format("[{0}]: {1} - {2}" + Environment.NewLine, DateTime.Now, ex.TargetSite, ex));
            File.AppendAllText(Settings.LogPath + "Log (" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ").txt",
                                               String.Format("[{0}]: {1} - {2}" + Environment.NewLine, DateTime.Now, ex.TargetSite, ex));
        }

        public static void EnqueueDebugging(string msg)
        {
            if (DebugLog.Count < 100)
            DebugLog.Enqueue(String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
            File.AppendAllText(Settings.LogPath + "DebugLog (" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ").txt",
                                           String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
        }
        public static void EnqueueChat(string msg)
        {
            if (ChatLog.Count < 100)
            ChatLog.Enqueue(String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
            File.AppendAllText(Settings.LogPath + "ChatLog (" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ").txt",
                                           String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
        }

        public static void Enqueue(string msg)
        {
            if (MessageLog.Count < 100)
            MessageLog.Enqueue(String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
            File.AppendAllText(Settings.LogPath + "Log (" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ").txt",
                                           String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void InterfaceTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Text = string.Format("Total: {0}, Real: {1}", Envir.LastCount, Envir.LastRealCount);
                PlayersLabel.Text = string.Format("Players: {0}", Envir.Players.Count);
                MonsterLabel.Text = string.Format("Monsters: {0}", Envir.MonsterCount);
                ConnectionsLabel.Text = string.Format("Connections: {0}", Envir.Connections.Count);

                if (Settings.Multithreaded && (Envir.MobThreads != null))
                {
                    CycleDelayLabel.Text = string.Format("CycleDelays: {0:0000}", Envir.LastRunTime);
                    for (int i = 0; i < Envir.MobThreads.Length; i++)
                    {
                        if (Envir.MobThreads[i] == null) break;
                        CycleDelayLabel.Text = CycleDelayLabel.Text + string.Format("|{0:0000}", Envir.MobThreads[i].LastRunTime);

                    }
                }
                else
                    CycleDelayLabel.Text = string.Format("CycleDelay: {0}", Envir.LastRunTime);

                while (!MessageLog.IsEmpty)
                {
                    string message;

                    if (!MessageLog.TryDequeue(out message)) continue;

                    LogTextBox.AppendText(message);
                }

                while (!DebugLog.IsEmpty)
                {
                    string message;

                    if (!DebugLog.TryDequeue(out message)) continue;

                    DebugLogTextBox.AppendText(message);
                }

                while (!ChatLog.IsEmpty)
                {
                    string message;

                    if (!ChatLog.TryDequeue(out message)) continue;

                    ChatLogTextBox.AppendText(message);
                }

                ProcessPlayersOnlineTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ProcessPlayersOnlineTab()
        {
            if (PlayersOnlineListView.Items.Count != Envir.Players.Count)
            {
                PlayersOnlineListView.Items.Clear();

                for (int i = PlayersOnlineListView.Items.Count; i < Envir.Players.Count; i++)
                {
                    Server.MirDatabase.CharacterInfo character = Envir.Players[i].Info;

                    ListViewItem tempItem = character.CreateListView();

                    PlayersOnlineListView.Items.Add(tempItem);
                }
            }
        }

        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Envir.Start();
        }

        private void stopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Envir.Stop();
            Envir.MonsterCount = 0;
        }

        private void SMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Envir.Stop();
        }

        private void closeServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void itemInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemInfoForm form = new ItemInfoForm();

            form.ShowDialog();
        }

        private void monsterInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonsterInfoForm form = new MonsterInfoForm();

            form.ShowDialog();
        }

        private void nPCInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NPCInfoForm form = new NPCInfoForm();

            form.ShowDialog();
        }

        private void balanceConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BalanceConfigForm form = new BalanceConfigForm();

            form.ShowDialog();
        }

        private void questInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuestInfoForm form = new QuestInfoForm();

            form.ShowDialog();
        }

        private void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigForm form = new ConfigForm();

            form.ShowDialog();
        }

        private void balanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BalanceConfigForm form = new BalanceConfigForm();

            form.ShowDialog();
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountInfoForm form = new AccountInfoForm();

            form.ShowDialog();
        }

        private void mapInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MapInfoForm form = new MapInfoForm();

            form.ShowDialog();
        }

        private void itemInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ItemInfoForm form = new ItemInfoForm();

            form.ShowDialog();
        }

        private void monsterInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MonsterInfoForm form = new MonsterInfoForm();

            form.ShowDialog();
        }

        private void nPCInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            NPCInfoForm form = new NPCInfoForm();

            form.ShowDialog();
        }

        private void questInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            QuestInfoForm form = new QuestInfoForm();

            form.ShowDialog();
        }

        private void dragonSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DragonInfoForm form = new DragonInfoForm();

            form.ShowDialog();
        }

        private void miningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MiningInfoForm form = new MiningInfoForm();

            form.ShowDialog();
        }

        private void guildsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuildInfoForm form = new GuildInfoForm();

            form.ShowDialog();
        }

        private void fishingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInfoForm form = new SystemInfoForm(0);

            form.ShowDialog();
        }

        private void GlobalMessageButton_Click(object sender, EventArgs e)
        {
            if (GlobalMessageTextBox.Text.Length < 1) return;

            foreach (var player in Envir.Players)
            {
                player.ReceiveChat(GlobalMessageTextBox.Text, ChatType.Announcement);
            }

            EnqueueChat(GlobalMessageTextBox.Text);
            GlobalMessageTextBox.Text = string.Empty;
        }

        private void PlayersOnlineListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewNF list = (ListViewNF)sender;

            if (list.SelectedItems.Count > 0)
            {
                ListViewItem item = list.SelectedItems[0];
                string index = item.SubItems[0].Text;

                PlayerInfoForm form = new PlayerInfoForm(Convert.ToUInt32(index));

                form.ShowDialog();
            }
        }

        private void PlayersOnlineListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = PlayersOnlineListView.Columns[e.ColumnIndex].Width;
        }

        private void mailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInfoForm form = new SystemInfoForm(1);

            form.ShowDialog();
        }

        private void goodsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInfoForm form = new SystemInfoForm(2);

            form.ShowDialog();
        }

        private void relationshipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInfoForm form = new SystemInfoForm(4);

            form.ShowDialog();
        }

        private void refiningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInfoForm form = new SystemInfoForm(3);

            form.ShowDialog();
        }

        private void mentorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInfoForm form = new SystemInfoForm(5);

            form.ShowDialog();
        }

        private void magicInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MagicInfoForm form = new MagicInfoForm();
            form.ShowDialog();
        }

        private void SMain_Load(object sender, EventArgs e)
        {
            EditEnvir.LoadDB();
            Envir.Start();
            AutoResize();
        }

        private void gemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInfoForm form = new SystemInfoForm(6);

            form.ShowDialog();
        }

        private void conquestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConquestInfoForm form = new ConquestInfoForm();

            form.ShowDialog();
        }

        private void rebootServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Envir.Reboot();
        }

        private void respawnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInfoForm form = new SystemInfoForm(7);
            
            form.ShowDialog();
        }

        private void monsterTunerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SMain.Envir.Running)
            {
                MessageBox.Show("Server must be running to tune monsters", "Notice",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            MonsterTunerForm form = new MonsterTunerForm();

            form.ShowDialog();
        }

        private void gameshopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameShop form = new GameShop();
            form.ShowDialog();
        }

    }
}
