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
    public partial class NPCInfoForm : Form
    {
        public string NPCListPath = Path.Combine(Settings.ExportPath, "NPCList.txt");

        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        private List<NPCInfo> _selectedNPCInfos;

        public NPCInfoForm()
        {
            InitializeComponent();

            for (int i = 0; i < Envir.MapInfoList.Count; i++) MapComboBox.Items.Add(Envir.MapInfoList[i]);

            if (ConquestHidden_combo.Items.Count != Envir.ConquestInfos.Count)
            {
                ConquestHidden_combo.Items.Clear();

                for (int i = 0; i < Envir.ConquestInfos.Count; i++)
                {
                    ConquestHidden_combo.Items.Add(Envir.ConquestInfos[i]);
                }
            }


            UpdateInterface();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Envir.CreateNPCInfo();
            UpdateInterface();
        }
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (_selectedNPCInfos.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected NPCs?", "Remove NPCs?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            for (int i = 0; i < _selectedNPCInfos.Count; i++) Envir.Remove(_selectedNPCInfos[i]);

            if (Envir.NPCInfoList.Count == 0) Envir.NPCIndex = 0;

            UpdateInterface();
        }

        private void UpdateInterface()
        {
            if (NPCInfoListBox.Items.Count != Envir.NPCInfoList.Count)
            {
                NPCInfoListBox.Items.Clear();

                for (int i = 0; i < Envir.NPCInfoList.Count; i++)
                    NPCInfoListBox.Items.Add(Envir.NPCInfoList[i]);
            }

            _selectedNPCInfos = NPCInfoListBox.SelectedItems.Cast<NPCInfo>().ToList();

            if (_selectedNPCInfos.Count == 0)
            {
                tabPage1.Enabled = false;
                tabPage2.Enabled = false;
                NPCIndexTextBox.Text = string.Empty;
                NFileNameTextBox.Text = string.Empty;
                NNameTextBox.Text = string.Empty;
                NXTextBox.Text = string.Empty;
                NYTextBox.Text = string.Empty;
                NImageTextBox.Text = string.Empty;
                NRateTextBox.Text = string.Empty;
                MapComboBox.SelectedItem = null;
                MinLev_textbox.Text = string.Empty;
                MaxLev_textbox.Text = string.Empty;
                Class_combo.Text = string.Empty;
                ConquestHidden_combo.SelectedIndex = -1;
                Day_combo.Text = string.Empty;
                TimeVisible_checkbox.Checked = false;
                StartHour_combo.Text = string.Empty;
                EndHour_combo.Text = string.Empty;
                StartMin_num.Value = 0;
                EndMin_num.Value = 1;
                Flag_textbox.Text = string.Empty;
                return;
            }

            NPCInfo info = _selectedNPCInfos[0];

            tabPage1.Enabled = true;
            tabPage2.Enabled = true;

            NPCIndexTextBox.Text = info.Index.ToString();
            NFileNameTextBox.Text = info.FileName;
            NNameTextBox.Text = info.Name;
            NXTextBox.Text = info.Location.X.ToString();
            NYTextBox.Text = info.Location.Y.ToString();
            NImageTextBox.Text = info.Image.ToString();
            NRateTextBox.Text = info.Rate.ToString();
            MapComboBox.SelectedItem = Envir.MapInfoList.FirstOrDefault(x => x.Index == info.MapIndex);
            MinLev_textbox.Text = info.MinLev.ToString();
            MaxLev_textbox.Text = info.MaxLev.ToString();
            Class_combo.Text = info.ClassRequired;
            ConquestHidden_combo.SelectedItem = Envir.ConquestInfos.FirstOrDefault(x => x.Index == info.Conquest);
            Day_combo.Text = info.DayofWeek;
            TimeVisible_checkbox.Checked = info.TimeVisible;
            StartHour_combo.Text = info.HourStart.ToString();
            EndHour_combo.Text = info.HourEnd.ToString();
            StartMin_num.Value = info.MinuteStart;
            EndMin_num.Value = info.MinuteEnd;
            Flag_textbox.Text = info.FlagNeeded.ToString();


            for (int i = 1; i < _selectedNPCInfos.Count; i++)
            {
                info = _selectedNPCInfos[i];

                if (NFileNameTextBox.Text != info.FileName) NFileNameTextBox.Text = string.Empty;
                if (NNameTextBox.Text != info.Name) NNameTextBox.Text = string.Empty;
                if (NXTextBox.Text != info.Location.X.ToString()) NXTextBox.Text = string.Empty;

                if (NYTextBox.Text != info.Location.Y.ToString()) NYTextBox.Text = string.Empty;
                if (NImageTextBox.Text != info.Image.ToString()) NImageTextBox.Text = string.Empty;
                if (NRateTextBox.Text != info.Rate.ToString()) NRateTextBox.Text = string.Empty;
            }
        }

        private void RefreshNPCList()
        {
            NPCInfoListBox.SelectedIndexChanged -= NPCInfoListBox_SelectedIndexChanged;

            List<bool> selected = new List<bool>();

            for (int i = 0; i < NPCInfoListBox.Items.Count; i++) selected.Add(NPCInfoListBox.GetSelected(i));
            NPCInfoListBox.Items.Clear();

            for (int i = 0; i < Envir.NPCInfoList.Count; i++) NPCInfoListBox.Items.Add(Envir.NPCInfoList[i]);
            for (int i = 0; i < selected.Count; i++) NPCInfoListBox.SetSelected(i, selected[i]);

            NPCInfoListBox.SelectedIndexChanged += NPCInfoListBox_SelectedIndexChanged;
        }

        private void NPCInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        private void NFileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].FileName = ActiveControl.Text;

            RefreshNPCList();
        }
        private void NNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].Name = ActiveControl.Text;
        }
        private void NXTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].Location.X = temp;

            RefreshNPCList();
        }
        private void NYTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].Location.Y = temp;

            RefreshNPCList();
        }
        private void NImageTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].Image = temp;

        }
        private void NRateTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].Rate = temp;
        }

        private void MapComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedNPCInfos.Count; i++)
            {
                MapInfo temp = (MapInfo)MapComboBox.SelectedItem;
                _selectedNPCInfos[i].MapIndex = temp.Index;
            }

        }

        private void NPCInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Envir.SaveDB();
        }




        private void PasteMButton_Click(object sender, EventArgs e)
        {
            string data = Clipboard.GetText();

            if (!data.StartsWith("NPC", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Cannot Paste, Copied data is not NPC Information.");
                return;
            }


            string[] npcs = data.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);


            //for (int i = 1; i < npcs.Length; i++)
            //    NPCInfo.FromText(npcs[i]);

            UpdateInterface();
        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            ExportNPCs(Envir.NPCInfoList);
        }

        private void ExportSelected_Click(object sender, EventArgs e)
        {
            var list = NPCInfoListBox.SelectedItems.Cast<NPCInfo>().ToList();

            ExportNPCs(list);
        }

        public void ExportNPCs(List<NPCInfo> NPCs)
        {
            if (NPCs.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath + @"\Exports";
            sfd.Filter = "Text File|*.txt";
            sfd.ShowDialog();

            if (sfd.FileName == string.Empty) return;

            using (StreamWriter sw = File.AppendText(sfd.FileNames[0]))
            {
                for (int j = 0; j < NPCs.Count; j++)
                {
                    sw.WriteLine(NPCs[j].ToText());
                }
            }
            MessageBox.Show("NPC Export complete");
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

            var npcs = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var m in npcs)
            {
                try
                {
                    NPCInfo.FromText(m);
                }
                catch { }
            }

            UpdateInterface();
            MessageBox.Show("NPC Import complete");
        }

        private void OpenNButton_Click(object sender, EventArgs e)
        {
            if (NFileNameTextBox.Text == string.Empty) return;

            var scriptPath = Settings.NPCPath + NFileNameTextBox.Text + ".txt";

            if (File.Exists(scriptPath))
                Process.Start(scriptPath);
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(scriptPath));
                File.Create(scriptPath).Close();
                Process.Start(scriptPath);
            }
        }

        private void MinLev_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            short temp;

            if (!short.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].MinLev = temp;
        }

        private void HourShow_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].HourStart = temp;
        }

        private void MinutesShow_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].MinuteStart = temp;
        }

        private void Class_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].ClassRequired = ActiveControl.Text;
        }

        private void CopyMButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DateTime.Now.DayOfWeek.ToString());
        }

        private void MaxLev_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            short temp;

            if (!short.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].MaxLev = temp;
        }

        private void Class_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            string temp = ActiveControl.Text;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].ClassRequired = temp;
        }

        private void Day_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            string temp = ActiveControl.Text;


            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].DayofWeek = temp;
        }

        private void TimeVisible_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].TimeVisible = TimeVisible_checkbox.Checked;
        }

        private void StartHour_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].HourStart = temp;
        }

        private void EndHour_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].HourEnd = temp;
        }

        private void StartMin_num_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].MinuteStart = (byte)StartMin_num.Value;
        }

        private void EndMin_num_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].MinuteEnd = (byte)EndMin_num.Value;
        }

        private void Flag_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].FlagNeeded = temp;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            MessageBox.Show(DateTime.Now.TimeOfDay.ToString());
        }

        private void NPCInfoForm_Load(object sender, EventArgs e)
        {

        }

        private void ConquestHidden_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ConquestInfo temp = (ConquestInfo)ConquestHidden_combo.SelectedItem;

            for (int i = 0; i < _selectedNPCInfos.Count; i++)
                _selectedNPCInfos[i].Conquest = temp.Index;
        }
    }
}
