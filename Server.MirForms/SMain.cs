using System;
using System.Windows.Forms;
using Server.MirEnvir;
using Server.MirDatabase;
using Server.MirForms.Systems;
using Server.Database;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using S = ServerPackets;
using Server.MirForms;
using System.Collections.Generic;
using System.Drawing;

namespace Server
{
    public partial class SMain : Form
    {
        public static Envir Envir => Envir.Main;

        public static Envir EditEnvir => Envir.Edit;

        protected static MessageQueue MessageQueue => MessageQueue.Instance;

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
            MessageQueue.Enqueue(ex);
        }

        public static void EnqueueDebugging(string msg)
        {
            MessageQueue.EnqueueDebugging(msg);
        }

        public static void EnqueueChat(string msg)
        {
            MessageQueue.EnqueueChat(msg);
        }

        public static void Enqueue(string msg)
        {
            MessageQueue.Enqueue(msg);
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void InterfaceTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Text = $"Total: {Envir.LastCount}, Real: {Envir.LastRealCount}";
                PlayersLabel.Text = $"Players: {Envir.Players.Count}";
                MonsterLabel.Text = $"Monsters: {Envir.MonsterCount}";
                ConnectionsLabel.Text = $"Connections: {Envir.Connections.Count}";

                if (Settings.Multithreaded && (Envir.MobThreads != null))
                {
                    CycleDelayLabel.Text = $"CycleDelays: {Envir.LastRunTime:0000}";
                    for (int i = 0; i < Envir.MobThreads.Length; i++)
                    {
                        if (Envir.MobThreads[i] == null) break;
                        CycleDelayLabel.Text = CycleDelayLabel.Text + $"|{Envir.MobThreads[i].LastRunTime:0000}";

                    }
                }
                else
                    CycleDelayLabel.Text = $"CycleDelay: {Envir.LastRunTime}";

                while (!MessageQueue.MessageLog.IsEmpty)
                {
                    string message;

                    if (!MessageQueue.MessageLog.TryDequeue(out message)) continue;

                    LogTextBox.AppendText(message);
                }

                while (!MessageQueue.DebugLog.IsEmpty)
                {
                    string message;

                    if (!MessageQueue.DebugLog.TryDequeue(out message)) continue;

                    DebugLogTextBox.AppendText(message);
                }

                while (!MessageQueue.ChatLog.IsEmpty)
                {
                    string message;

                    if (!MessageQueue.ChatLog.TryDequeue(out message)) continue;

                    ChatLogTextBox.AppendText(message);
                }

                ProcessPlayersOnlineTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ListViewItem CreateListView(CharacterInfo character)
        {
            ListViewItem ListItem = new ListViewItem(character.Index.ToString()) { Tag = character };

            ListItem.SubItems.Add(character.Name);
            ListItem.SubItems.Add(character.Level.ToString());
            ListItem.SubItems.Add(character.Class.ToString());
            ListItem.SubItems.Add(character.Gender.ToString());

            return ListItem;
        }

        private void ProcessPlayersOnlineTab()
        {
            if (PlayersOnlineListView.Items.Count != Envir.Players.Count)
            {
                PlayersOnlineListView.Items.Clear();

                for (int i = PlayersOnlineListView.Items.Count; i < Envir.Players.Count; i++)
                {
                    CharacterInfo character = Envir.Players[i].Info;

                    ListViewItem tempItem = CreateListView(character);

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
            CustomFormControl.ListViewNF list = (CustomFormControl.ListViewNF)sender;

            if (list.SelectedItems.Count > 0)
            {
                ListViewItem item = list.SelectedItems[0];
                string index = item.SubItems[0].Text;

                PlayerInfoForm form = new PlayerInfoForm(Convert.ToUInt32(index));

                form.ShowDialog();
            }
        }

        private void MainTab_MouseClick(object sender, MouseEventArgs e)
        {
            ProcessGuildViewTab();
        }

        public void ProcessGuildViewTab()
        {

            GuildListView.Items.Clear();


            for (int i = 0; i < Envir.GuildList.Count; i++)
            {
                Server.MirObjects.GuildObject Guild = Envir.GuildList[i];

                ListViewItem tempItem = new ListViewItem(Guild.Guildindex.ToString()) { Tag = this };

                tempItem.SubItems.Add(Guild.Name);

                if (Guild.Ranks.Count > 0 && Guild.Ranks[0].Members.Count > 0)
                    tempItem.SubItems.Add(Guild.Ranks[0].Members[0].name);
                else
                    tempItem.SubItems.Add("Not Existing");

                tempItem.SubItems.Add(Guild.Membercount.ToString());
                tempItem.SubItems.Add(Guild.Level.ToString());
                tempItem.SubItems.Add(Guild.Gold.ToString());
                tempItem.SubItems.Add(Guild.HasGT ? Guild.GTRent.ToString() : "None");

                GuildListView.Items.Add(tempItem);
            }
        }


        private void GuildListView_DoubleClick(object sender, EventArgs e)
        {

            CustomFormControl.ListViewNF list = (CustomFormControl.ListViewNF)sender;

            if (list.SelectedItems.Count > 0)
            {
                ListViewItem item = list.SelectedItems[0];
                int index = Int32.Parse(item.Text);

                Server.MirObjects.GuildObject Guild = Envir.GetGuild(index);

                MirForms.GuildItemForm form = new MirForms.GuildItemForm()
                {
                    GuildName = Guild.Name,
                    main = this,
                };

                if (Guild == null) return;

                foreach (var i in Guild.StoredItems)
                {
                    if (i == null) continue;
                    ListViewItem tempItem = new ListViewItem(i.Item.UniqueID.ToString()) { Tag = this };

                    Server.MirDatabase.CharacterInfo character = Envir.GetCharacterInfo((int)i.UserId);
                    if (character != null)
                        tempItem.SubItems.Add(character.Name);
                    else if (i.UserId == -1)
                        tempItem.SubItems.Add("Server");
                    else
                        tempItem.SubItems.Add("Unknown");

                    tempItem.SubItems.Add(i.Item.Name);
                    tempItem.SubItems.Add(i.Item.Count.ToString());
                    tempItem.SubItems.Add(i.Item.CurrentDura.ToString() + "/" + i.Item.MaxDura.ToString());

                    form.GuildItemListView.Items.Add(tempItem);
                }

                foreach (var r in Guild.Ranks)
                    foreach (var m in r.Members)
                    {
                        ListViewItem tempItem = new ListViewItem(m.name) { Tag = this };
                        tempItem.SubItems.Add(r.Name);
                        form.MemberListView.Items.Add(tempItem);
                    }


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
            var loaded = EditEnvir.LoadDB();

            if (loaded)
            {
                Envir.Start();
            }
            AutoResize();
        }
        public const string BackUpPath = @".\Back Up\";
        public const string AccountDBPath = BackUpPath + @"Account\";

        public const string GuildDBPath = BackUpPath + @"Guilds\";
        public const string ConquestDBPath = BackUpPath + @"Conquest\";
        public const string CraftingDBPath = BackUpPath + @"Crafting\";
        public const string DragonDBPath = BackUpPath + @"Dragon\";
        public const string GameShopDBPath = BackUpPath + @"GameShop\";
        public const string ItemDBPath = BackUpPath + @"Item\";
        public const string MagicDBPath = BackUpPath + @"Magic\";
        public const string MapDBPath = BackUpPath + @"Map\";
        public const string MobDBPath = BackUpPath + @"Mob\";
        public const string NPCDBPath = BackUpPath + @"NPC\";
        public const string QuestDBPath = BackUpPath + @"Quest\";
        public const string RespawnTickDBPath = BackUpPath + @"Respawn\";
        public const string VersionDBPath = BackUpPath + @"Version\";

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

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int u = 0;


            foreach (var NewItem in EditEnvir.ItemInfoList)
            {
                ItemInfo OldItem = Envir.ItemInfoList.Find(x => x.Index == NewItem.Index);
                if (OldItem != null)
                {
                    OldItem.UpdateItem(NewItem);
                }
                else
                {
                    ItemInfo CloneItem = ItemInfo.CloneItem(NewItem);
                    Envir.ItemInfoList.Add(CloneItem);
                    u++;
                }
            }

            SMain.Enqueue("[Item DataBase] total items :" + Envir.ItemInfoList.Count.ToString());
            SMain.Enqueue("[Item DataBase] " + (Envir.ItemInfoList.Count - u).ToString() + " has been updated");
            SMain.Enqueue("[Item DataBase] " + u.ToString() + " has been added");

            foreach (var p in Envir.Players) // refresh all existing players stats
            {
                if (p.Info == null) continue;

                p.RefreshStats();
                p.Enqueue(new S.RefreshStats());

            }

        }
        private void REGameShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int u = 0;


            foreach (var NewItem in EditEnvir.GameShopList)
            {
                var OldItem = Envir.GameShopList.Find(x => x.GIndex == NewItem.GIndex);
                if (OldItem != null)
                {
                    OldItem.UpdateItem(NewItem);
                }
                else
                {
                    var CloneItem = GameShopItem.CloneItem(NewItem);
                    Envir.GameShopList.Add(CloneItem);
                    u++;
                }
            }

            SMain.Enqueue("[Gameshop DataBase] total items :" + Envir.GameShopList.Count.ToString());
            SMain.Enqueue("[Gameshop DataBase] " + (Envir.GameShopList.Count - u).ToString() + " has been updated");
            SMain.Enqueue("[Gameshop DataBase] " + u.ToString() + " has been added");

            foreach (var p in Envir.Players)// update all info on players items
            {
                if (p.Info == null) continue;

                p.GetGameShop();

            }
        }
        private void monsterToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int u = 0;


            foreach (var NewMob in EditEnvir.MonsterInfoList)
            {
                MonsterInfo OldMob = Envir.MonsterInfoList.Find(x => x.Index == NewMob.Index);
                if (OldMob != null)
                {
                    OldMob.UpdateMonster(NewMob);
                }
                else
                {
                    MonsterInfo CloneMonster = MonsterInfo.CloneMonster(NewMob);
                    Envir.MonsterInfoList.Add(CloneMonster);
                    u++;
                }
            }

            SMain.Enqueue("[Monster DataBase] total monsters :" + Envir.MonsterInfoList.Count.ToString());
            SMain.Enqueue("[Monster DataBase] " + (Envir.MonsterInfoList.Count - u).ToString() + " has been updated");
            SMain.Enqueue("[Monster DataBase] " + u.ToString() + " has been added");


        }
        private void magicToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (var NewMagic in EditEnvir.MagicInfoList)
            {
                MagicInfo OldMagic = Envir.MagicInfoList.Find(x => x.Spell == NewMagic.Spell);
                if (OldMagic != null)
                {
                    OldMagic.Copy(NewMagic);
                }
            }

            foreach (var p in Envir.Players)
            {
                foreach (var Magic in Envir.MagicInfoList)
                {
                    p.Enqueue(new S.RefreshMagic { Magic = (new UserMagic(Magic.Spell)).CreateClientMagic() });
                }
            }

            SMain.Enqueue("[Magic DataBase] total magics :" + Envir.MagicInfoList.Count.ToString());
            SMain.Enqueue("[Magic DataBase] " + (Envir.MagicInfoList.Count).ToString() + " has been updated");

        }

        private void reloadNPCsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Envir.ReloadNPCs();
        }

        private void reloadDropsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Envir.ReloadDrops();
        }

        private void gameshopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameShop form = new GameShop();
            form.ShowDialog();
        }

        private void itemNEWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemInfoFormNew form = new ItemInfoFormNew();

            form.ShowDialog();
        }

        private void monsterExperimentalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonsterInfoFormNew form = new MonsterInfoFormNew();

            form.ShowDialog();
        }
    }
}
