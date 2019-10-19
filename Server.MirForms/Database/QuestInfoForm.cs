using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Server.MirDatabase;
using Server.MirEnvir;
using System.Diagnostics;

namespace Server
{
    public partial class QuestInfoForm : Form
    {
        public string QuestListPath = Path.Combine(Settings.ExportPath, "QuestList.txt");

        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        private List<QuestInfo> _selectedQuestInfos;

        public QuestInfoForm()
        {
            InitializeComponent();

            QTypeComboBox.Items.AddRange(Enum.GetValues(typeof(QuestType)).Cast<object>().ToArray());
            RequiredClassComboBox.Items.AddRange(Enum.GetValues(typeof(RequiredClass)).Cast<object>().ToArray());

            UpdateInterface();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Envir.CreateQuestInfo();
            UpdateInterface();
        }
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (_selectedQuestInfos.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected Quests?", "Remove Quests?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++) Envir.Remove(_selectedQuestInfos[i]);

            if (Envir.QuestInfoList.Count == 0) Envir.QuestIndex = 0;

            UpdateInterface();
        }

        private void UpdateInterface()
        {
            if (QuestInfoListBox.Items.Count != Envir.QuestInfoList.Count)
            {
                QuestInfoListBox.Items.Clear();
                RequiredQuestComboBox.Items.Clear();

                RequiredQuestComboBox.Items.Add(new QuestInfo() { Index = 0, Name = "None" });

                for (int i = 0; i < Envir.QuestInfoList.Count; i++)
                {
                    QuestInfoListBox.Items.Add(Envir.QuestInfoList[i]);
                    RequiredQuestComboBox.Items.Add(Envir.QuestInfoList[i]);
                }

            }

            _selectedQuestInfos = QuestInfoListBox.SelectedItems.Cast<QuestInfo>().ToList();

            if (_selectedQuestInfos.Count == 0)
            {
                QuestInfoPanel.Enabled = false;
                QuestIndexTextBox.Text = string.Empty;
                QFileNameTextBox.Text = string.Empty;
                QNameTextBox.Text = string.Empty;
                QGroupTextBox.Text = string.Empty;
                QTypeComboBox.SelectedItem = null;

                QGotoTextBox.Text = string.Empty;
                QKillTextBox.Text = string.Empty;
                QItemTextBox.Text = string.Empty;
                QFlagTextBox.Text = string.Empty;

                RequiredMinLevelTextBox.Text = string.Empty;
                RequiredMaxLevelTextBox.Text = string.Empty;
                RequiredQuestComboBox.SelectedItem = null;
                RequiredClassComboBox.SelectedItem = null;

                return;
            }

            QuestInfo info = _selectedQuestInfos[0];

            QuestInfoPanel.Enabled = true;
            QuestIndexTextBox.Text = info.Index.ToString();
            QFileNameTextBox.Text = info.FileName;
            QNameTextBox.Text = info.Name;
            QGroupTextBox.Text = info.Group;
            QTypeComboBox.SelectedItem = info.Type;

            QGotoTextBox.Text = info.GotoMessage;
            QKillTextBox.Text = info.KillMessage;
            QItemTextBox.Text = info.ItemMessage;
            QFlagTextBox.Text = info.FlagMessage;

            RequiredMinLevelTextBox.Text = info.RequiredMinLevel.ToString();
            RequiredMaxLevelTextBox.Text = info.RequiredMaxLevel.ToString();

            if (Convert.ToInt32(RequiredMaxLevelTextBox.Text) <= 0) RequiredMaxLevelTextBox.Text = byte.MaxValue.ToString();

            QuestInfo tempQuest = Envir.QuestInfoList.FirstOrDefault(c => c.Index == info.RequiredQuest);
                
            if (tempQuest == null)
            {
                tempQuest = (QuestInfo)RequiredQuestComboBox.Items[0];
            }

            RequiredQuestComboBox.SelectedItem = tempQuest;  //test
            RequiredClassComboBox.SelectedItem = info.RequiredClass;

            for (int i = 1; i < _selectedQuestInfos.Count; i++)
            {
                info = _selectedQuestInfos[i];

                if(QFileNameTextBox.Text != info.FileName) QFileNameTextBox.Text = string.Empty;
                if (QNameTextBox.Text != info.Name) QNameTextBox.Text = string.Empty;
                if (QGroupTextBox.Text != info.Group) QGroupTextBox.Text = string.Empty;

                if (QTypeComboBox.SelectedItem != null)
                    if ((QuestType)QTypeComboBox.SelectedItem != info.Type) QTypeComboBox.SelectedItem = null;

                if (QGotoTextBox.Text != info.GotoMessage) QGotoTextBox.Text = string.Empty;
                if (QKillTextBox.Text != info.KillMessage) QKillTextBox.Text = string.Empty;
                if (QItemTextBox.Text != info.ItemMessage) QItemTextBox.Text = string.Empty;
                if (QFlagTextBox.Text != info.ItemMessage) QFlagTextBox.Text = string.Empty;

                if (RequiredMinLevelTextBox.Text != info.RequiredMinLevel.ToString()) RequiredMinLevelTextBox.Text = string.Empty;
                if (RequiredMaxLevelTextBox.Text != info.RequiredMaxLevel.ToString()) RequiredMaxLevelTextBox.Text = byte.MaxValue.ToString();

                if (RequiredQuestComboBox.SelectedValue != null)
                    if ((string)RequiredQuestComboBox.SelectedValue != info.RequiredQuest.ToString()) RequiredQuestComboBox.SelectedItem = null;

                if (RequiredClassComboBox.SelectedItem != null)
                    if ((RequiredClass)RequiredClassComboBox.SelectedItem != info.RequiredClass) RequiredClassComboBox.SelectedItem = null;
            }
        }

        private void RefreshQuestList()
        {
            QuestInfoListBox.SelectedIndexChanged -= QuestInfoListBox_SelectedIndexChanged;

            List<bool> selected = new List<bool>();

            for (int i = 0; i < QuestInfoListBox.Items.Count; i++) selected.Add(QuestInfoListBox.GetSelected(i));
            QuestInfoListBox.Items.Clear();
            for (int i = 0; i < Envir.QuestInfoList.Count; i++) QuestInfoListBox.Items.Add(Envir.QuestInfoList[i]);
            for (int i = 0; i < selected.Count; i++) QuestInfoListBox.SetSelected(i, selected[i]);

            QuestInfoListBox.SelectedIndexChanged += QuestInfoListBox_SelectedIndexChanged;

        }

        private void QuestInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        private void QuestInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Envir.SaveDB();
        }

        private void PasteMButton_Click(object sender, EventArgs e)
        {
            string data = Clipboard.GetText();

            if (!data.StartsWith("Quest", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Cannot Paste, Copied data is not Quest Information.");
                return;
            }


            string[] npcs = data.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);


            //for (int i = 1; i < npcs.Length; i++)
            //    NPCInfo.FromText(npcs[i]);

            UpdateInterface();
        }


        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            ExportQuests(Envir.QuestInfoList);
        }

        private void ExportSelected_Click(object sender, EventArgs e)
        {
            var list = QuestInfoListBox.SelectedItems.Cast<QuestInfo>().ToList();

            ExportQuests(list);
        }

        public void ExportQuests(List<QuestInfo> Quests)
        {
            if (Quests.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath + @"\Exports";
            sfd.Filter = "Text File|*.txt";
            sfd.ShowDialog();

            if (sfd.FileName == string.Empty) return;

            using (StreamWriter sw = File.AppendText(sfd.FileNames[0]))
            {
                for (int j = 0; j < Quests.Count; j++)
                {
                    sw.WriteLine(Quests[j].ToText());
                }
            }
            MessageBox.Show("Quest Export complete");
        }


        private void ImportButton_Click(object sender, EventArgs e)
        {
            string Path = string.Empty;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File|*.txt";
            ofd.ShowDialog();

            if (ofd.FileName == string.Empty) return;

            Path = ofd.FileName;

            string data;
            using (var sr = new StreamReader(Path))
            {
                data = sr.ReadToEnd();
            }

            var quests = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var m in quests)
                QuestInfo.FromText(m);

            UpdateInterface();
            MessageBox.Show("Quest Import complete");
        }

        private void QNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].Name = ActiveControl.Text;

            RefreshQuestList();
        }

        private void QGroupTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].Group = ActiveControl.Text;
        }

        private void QTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].Type = (QuestType)QTypeComboBox.SelectedItem;
        }

        private void QFileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].FileName = ActiveControl.Text;

        }

        private void QGotoTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].GotoMessage = ActiveControl.Text;

        }

        private void QKillTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].KillMessage = ActiveControl.Text;

        }

        private void QItemTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].ItemMessage = ActiveControl.Text;
        }

        private void QFlagTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].FlagMessage = ActiveControl.Text;
        }


        private void RequiredMinLevelTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > Convert.ToInt32(RequiredMaxLevelTextBox.Text) || temp < byte.MinValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].RequiredMinLevel = temp;
        }

        private void RequiredMaxLevelTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp < Convert.ToInt32(RequiredMinLevelTextBox.Text) || temp > byte.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].RequiredMaxLevel = temp;
        }

        private void RequiredQuestComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
            {
                QuestInfo temp = (QuestInfo)RequiredQuestComboBox.SelectedItem;
                
                _selectedQuestInfos[i].RequiredQuest = temp.Index;
            }
        }

        private void RequiredClassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedQuestInfos.Count; i++)
                _selectedQuestInfos[i].RequiredClass = (RequiredClass)RequiredClassComboBox.SelectedItem;
        }

        private void OpenQButton_Click(object sender, EventArgs e)
        {
            if (QFileNameTextBox.Text == string.Empty) return;

            var scriptPath = Settings.QuestPath + QFileNameTextBox.Text + ".txt";

            if (File.Exists(scriptPath))
                Process.Start(scriptPath);
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(scriptPath));

                File.Create(scriptPath).Close();

                File.WriteAllText(scriptPath,
                    string.Format("{0}\r\n\r\n{1}\r\n\r\n{2}\r\n\r\n{3}\r\n\r\n{4}\r\n\r\n{5}\r\n\r\n{6}\r\n\r\n{7}\r\n\r\n{8}\r\n\r\n{9}\r\n\r\n{10}",
                    "[@Description]", "[@TaskDescription]", "[@Completion]", "[@KillTasks]", "[@ItemTasks]", "[@FlagTasks]", "[@CarryItems]", "[@FixedRewards]", "[@SelectRewards]", "[@ExpReward]", "[@GoldReward]"));

                Process.Start(scriptPath);
            }
        }



    }
}
