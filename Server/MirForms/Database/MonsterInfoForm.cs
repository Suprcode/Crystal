using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;

namespace Server
{
    public partial class MonsterInfoForm : Form
    {
        public string MonsterListPath = Path.Combine(Settings.ExportPath, "MonsterList.txt");

        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        private List<MonsterInfo> _selectedMonsterInfos;

        public MonsterInfoForm()
        {
            InitializeComponent();

            ImageComboBox.Items.AddRange(Enum.GetValues(typeof(Monster)).Cast<object>().ToArray());
            UpdateInterface();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Envir.CreateMonsterInfo();
            UpdateInterface();
        }
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (_selectedMonsterInfos.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected Monsters?", "Remove Monsters?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++) Envir.Remove(_selectedMonsterInfos[i]);

            if (Envir.MonsterInfoList.Count == 0) Envir.MonsterIndex = 0;

            UpdateInterface();
        }

        private void UpdateInterface()
        {
            if (MonsterInfoListBox.Items.Count != Envir.MonsterInfoList.Count)
            {
                MonsterInfoListBox.Items.Clear();

                for (int i = 0; i < Envir.MonsterInfoList.Count; i++)
                    MonsterInfoListBox.Items.Add(Envir.MonsterInfoList[i]);
            }

            _selectedMonsterInfos = MonsterInfoListBox.SelectedItems.Cast<MonsterInfo>().ToList();

            if (_selectedMonsterInfos.Count == 0)
            {
                MonsterInfoPanel.Enabled = false;
                MonsterIndexTextBox.Text = string.Empty;
                MonsterNameTextBox.Text = string.Empty;

                ImageComboBox.SelectedItem = null;
                AITextBox.Text = string.Empty;
                EffectTextBox.Text = string.Empty;
                LevelTextBox.Text = string.Empty;
                ViewRangeTextBox.Text = string.Empty;
                CoolEyeTextBox.Text = string.Empty;

                HPTextBox.Text = string.Empty;
                ExperienceTextBox.Text = string.Empty;

                MinACTextBox.Text = string.Empty;
                MaxACTextBox.Text = string.Empty;
                MinMACTextBox.Text = string.Empty;
                MaxMACTextBox.Text = string.Empty;
                MinDCTextBox.Text = string.Empty;
                MaxDCTextBox.Text = string.Empty;
                MinMCTextBox.Text = string.Empty;
                MaxMCTextBox.Text = string.Empty;
                MinSCTextBox.Text = string.Empty;
                MaxSCTextBox.Text = string.Empty;
                AccuracyTextBox.Text = string.Empty;
                AgilityTextBox.Text = string.Empty;
                LightTextBox.Text = string.Empty;

                ASpeedTextBox.Text = string.Empty;
                MSpeedTextBox.Text = string.Empty;

                CanPushCheckBox.Checked = false;
                CanTameCheckBox.Checked = false;
                AutoRevCheckBox.Checked = false;
                UndeadCheckBox.Checked = false;

                return;
            }

            MonsterInfo info = _selectedMonsterInfos[0];

            MonsterInfoPanel.Enabled = true;

            MonsterIndexTextBox.Text = info.Index.ToString();
            MonsterNameTextBox.Text = info.Name;

            ImageComboBox.SelectedItem = null;
            ImageComboBox.SelectedItem = info.Image;
            AITextBox.Text = info.AI.ToString();
            EffectTextBox.Text = info.Effect.ToString();
            LevelTextBox.Text = info.Level.ToString();
            ViewRangeTextBox.Text = info.ViewRange.ToString();
            CoolEyeTextBox.Text = info.CoolEye.ToString();

            HPTextBox.Text = info.HP.ToString();
            ExperienceTextBox.Text = info.Experience.ToString();

            MinACTextBox.Text = info.MinAC.ToString();
            MaxACTextBox.Text = info.MaxAC.ToString();
            MinMACTextBox.Text = info.MinMAC.ToString();
            MaxMACTextBox.Text = info.MaxMAC.ToString();
            MinDCTextBox.Text = info.MinDC.ToString();
            MaxDCTextBox.Text = info.MaxDC.ToString();
            MinMCTextBox.Text = info.MinMC.ToString();
            MaxMCTextBox.Text = info.MaxMC.ToString();
            MinSCTextBox.Text = info.MinSC.ToString();
            MaxSCTextBox.Text = info.MaxSC.ToString();
            AccuracyTextBox.Text = info.Accuracy.ToString();
            AgilityTextBox.Text = info.Agility.ToString();
            LightTextBox.Text = info.Light.ToString();

            ASpeedTextBox.Text = info.AttackSpeed.ToString();
            MSpeedTextBox.Text = info.MoveSpeed.ToString();


            CanPushCheckBox.Checked = info.CanPush;
            CanTameCheckBox.Checked = info.CanTame;
            AutoRevCheckBox.Checked = info.AutoRev;
            UndeadCheckBox.Checked = info.Undead;

            for (int i = 1; i < _selectedMonsterInfos.Count; i++)
            {
                info = _selectedMonsterInfos[i];

                if (MonsterIndexTextBox.Text != info.Index.ToString()) MonsterIndexTextBox.Text = string.Empty;
                if (MonsterNameTextBox.Text != info.Name) MonsterNameTextBox.Text = string.Empty;

                if (ImageComboBox.SelectedItem == null || (Monster)ImageComboBox.SelectedItem != info.Image) ImageComboBox.SelectedItem = null;
                if (AITextBox.Text != info.AI.ToString()) AITextBox.Text = string.Empty;
                if (EffectTextBox.Text != info.Effect.ToString()) EffectTextBox.Text = string.Empty;
                if (LevelTextBox.Text != info.Level.ToString()) LevelTextBox.Text = string.Empty;
                if (ViewRangeTextBox.Text != info.ViewRange.ToString()) ViewRangeTextBox.Text = string.Empty;
                if (CoolEyeTextBox.Text != info.CoolEye.ToString()) CoolEyeTextBox.Text = string.Empty;
                if (HPTextBox.Text != info.HP.ToString()) HPTextBox.Text = string.Empty;
                if (ExperienceTextBox.Text != info.Experience.ToString()) ExperienceTextBox.Text = string.Empty;

                if (MinACTextBox.Text != info.MinAC.ToString()) MinACTextBox.Text = string.Empty;
                if (MaxACTextBox.Text != info.MaxAC.ToString()) MaxACTextBox.Text = string.Empty;
                if (MinMACTextBox.Text != info.MinMAC.ToString()) MinMACTextBox.Text = string.Empty;
                if (MaxMACTextBox.Text != info.MaxMAC.ToString()) MaxMACTextBox.Text = string.Empty;
                if (MinDCTextBox.Text != info.MinDC.ToString()) MinDCTextBox.Text = string.Empty;
                if (MaxDCTextBox.Text != info.MaxDC.ToString()) MaxDCTextBox.Text = string.Empty;
                if (MinMCTextBox.Text != info.MinMC.ToString()) MinMCTextBox.Text = string.Empty;
                if (MaxMCTextBox.Text != info.MaxMC.ToString()) MaxMCTextBox.Text = string.Empty;
                if (MinSCTextBox.Text != info.MinSC.ToString()) MinSCTextBox.Text = string.Empty;
                if (MaxSCTextBox.Text != info.MaxSC.ToString()) MaxSCTextBox.Text = string.Empty;
                if (AccuracyTextBox.Text != info.Accuracy.ToString()) AccuracyTextBox.Text = string.Empty;
                if (AgilityTextBox.Text != info.Agility.ToString()) AgilityTextBox.Text = string.Empty;
                if (LightTextBox.Text != info.Light.ToString()) LightTextBox.Text = string.Empty;
                if (ASpeedTextBox.Text != info.AttackSpeed.ToString()) ASpeedTextBox.Text = string.Empty;
                if (MSpeedTextBox.Text != info.MoveSpeed.ToString()) MSpeedTextBox.Text = string.Empty;

                if (CanPushCheckBox.Checked != info.CanPush) CanPushCheckBox.CheckState = CheckState.Indeterminate;
                if (CanTameCheckBox.Checked != info.CanTame) CanTameCheckBox.CheckState = CheckState.Indeterminate;

                if (AutoRevCheckBox.Checked != info.AutoRev) AutoRevCheckBox.CheckState = CheckState.Indeterminate;
                if (UndeadCheckBox.Checked != info.Undead) UndeadCheckBox.CheckState = CheckState.Indeterminate;
            }

        }
        private void RefreshMonsterList()
        {
            MonsterInfoListBox.SelectedIndexChanged -= MonsterInfoListBox_SelectedIndexChanged;

            List<bool> selected = new List<bool>();

            for (int i = 0; i < MonsterInfoListBox.Items.Count; i++) selected.Add(MonsterInfoListBox.GetSelected(i));
            MonsterInfoListBox.Items.Clear();
            for (int i = 0; i < Envir.MonsterInfoList.Count; i++) MonsterInfoListBox.Items.Add(Envir.MonsterInfoList[i]);
            for (int i = 0; i < selected.Count; i++) MonsterInfoListBox.SetSelected(i, selected[i]);

            MonsterInfoListBox.SelectedIndexChanged += MonsterInfoListBox_SelectedIndexChanged;
        }
        private void MonsterInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface();
        }
        private void MonsterNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].Name = ActiveControl.Text;

            RefreshMonsterList();
        }
        private void AITextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].AI = temp;
        }
        private void EffectTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].Effect = temp;
        }
        private void LevelTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].Level = temp;
        }
        private void LightTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].Light = temp;
        }
        private void ViewRangeTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].ViewRange = temp;
        }
        private void HPTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].HP = temp;
        }
        private void ExperienceTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].Experience = temp;
        }
        private void MinACTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MinAC = temp;
        }
        private void MaxACTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MaxAC = temp;
        }
        private void MinMACTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MinMAC = temp;
        }
        private void MaxMACTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MaxMAC = temp;
        }
        private void MinDCTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MinDC = temp;
        }
        private void MaxDCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MaxDC = temp;
        }
        private void MinMCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MinMC = temp;
        }
        private void MaxMCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MaxMC = temp;
        }
        private void MinSCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MinSC = temp;
        }
        private void MaxSCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp) || temp < 0 || temp > ushort.MaxValue)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MaxSC = temp;
        }
        private void AccuracyTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].Accuracy = temp;
        }
        private void AgilityTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].Agility = temp;
        }
        private void ASpeedTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].AttackSpeed = temp;
        }
        private void MSpeedTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].MoveSpeed = temp;
        }
        private void CanPushCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].CanPush = CanPushCheckBox.Checked;
        }
        private void CanTameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].CanTame = CanTameCheckBox.Checked;
        }
        private void AutoRevCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].AutoRev = AutoRevCheckBox.Checked;
        }

        private void UndeadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].Undead = UndeadCheckBox.Checked;
        }
        private void MonsterInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Envir.SaveDB();
        }

        private void PasteMButton_Click(object sender, EventArgs e)
        {
            string data = Clipboard.GetText();

            if (!data.StartsWith("Monster", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Cannot Paste, Copied data is not Monster Information.");
                return;
            }


            string[] monsters = data.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);


            for (int i = 1; i < monsters.Length; i++)
                MonsterInfo.FromText(monsters[i]);

            UpdateInterface();
        }

        private void CoolEyeTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].CoolEye = temp;
        }

        private void ImageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMonsterInfos.Count; i++)
                _selectedMonsterInfos[i].Image = (Monster)ImageComboBox.SelectedItem;

        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            ExportMonsters(Envir.MonsterInfoList);
        }

        private void ExportSelected_Click(object sender, EventArgs e)
        {
            var list = MonsterInfoListBox.SelectedItems.Cast<MonsterInfo>().ToList();

            ExportMonsters(list);
        }

        private void ExportMonsters(IEnumerable<MonsterInfo> monsters)
        {
            var monsterInfos = monsters as MonsterInfo[] ?? monsters.ToArray();
            var list = monsterInfos.Select(monster => monster.ToText()).ToList();

            File.WriteAllLines(MonsterListPath, list);

            MessageBox.Show(monsterInfos.Count() + " Items have been exported");
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

            var monsters = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var m in monsters)
                MonsterInfo.FromText(m);

            UpdateInterface();
        }

        private void DropBuilderButton_Click(object sender, EventArgs e)
        {
            MirForms.DropBuilder.DropGenForm GenForm = new MirForms.DropBuilder.DropGenForm();

            GenForm.ShowDialog();
        }
    }
}