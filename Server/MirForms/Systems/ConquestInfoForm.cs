using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Server.MirDatabase;
using Server.MirEnvir;

namespace Server
{
    public partial class ConquestInfoForm : Form
    {
        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        private ConquestInfo selectedConquest;
        private ConquestArcherInfo selectedArcher;
        private ConquestGateInfo selectedGate;
        private ConquestWallInfo selectedWall;
        private ConquestSiegeInfo selectedSiege;
        private ConquestFlagInfo selectedFlag;
        private ConquestFlagInfo selectedControlPoint;

        public ConquestInfoForm()
        {
            InitializeComponent();

            for (int i = 0; i < Envir.MapInfoList.Count; i++)
            {
                ConquestMap_combo.Items.Add(Envir.MapInfoList[i]);
                PalaceMap_combo.Items.Add(Envir.MapInfoList[i]);
                ExtraMaps_combo.Items.Add(Envir.MapInfoList[i]);
            }

            WarType_combo.Items.AddRange(Enum.GetValues(typeof(ConquestType)).Cast<object>().ToArray());
            WarMode_combo.Items.AddRange(Enum.GetValues(typeof(ConquestGame)).Cast<object>().ToArray());
            WarType_combo.Items.Remove(ConquestType.Forced);

            for (int i = 0; i < Envir.MonsterInfoList.Count; i++)
            {
                if (Envir.MonsterInfoList[i].AI == 80)
                    ArcherIndex_combo.Items.Add(Envir.MonsterInfoList[i]);

                if (Envir.MonsterInfoList[i].AI == 81)
                    GateIndex_combo.Items.Add(Envir.MonsterInfoList[i]);

                if (Envir.MonsterInfoList[i].AI == 82)
                    WallIndex_combo.Items.Add(Envir.MonsterInfoList[i]);

                if (Envir.MonsterInfoList[i].AI == 74)
                    SiegeIndex_combo.Items.Add(Envir.MonsterInfoList[i]);
            }

                UpdateInterface();
        }



        private void ConquestInfoForm_Load(object sender, EventArgs e)
        {

        }


        private void UpdateArchers()
        {
            ArcherIndex_combo.SelectedItem = null;
            ArcherIndex_combo.SelectedIndex = -1;

            if (selectedArcher != null)
            {
                Archer_gb.Enabled = true;
                ArcherIndex_combo.SelectedItem = Envir.MonsterInfoList.FirstOrDefault(x => x.Index == selectedArcher.MobIndex);
                ArchXLoc_textbox.Text = selectedArcher.Location.X.ToString();
                ArchYLoc_textbox.Text = selectedArcher.Location.Y.ToString();
                ArcherName_textbox.Text = selectedArcher.Name;
                ArcherCost_textbox.Text = selectedArcher.RepairCost.ToString();
            }
            else
            {
                Archer_gb.Enabled = false;
                ArchXLoc_textbox.Text = string.Empty;
                ArchYLoc_textbox.Text = string.Empty;
                ArcherName_textbox.Text = string.Empty;
                ArcherCost_textbox.Text = string.Empty;
            }
        }

        private void UpdateFlags()
        {
            if (selectedFlag != null)
            {
                Flag_gb.Enabled = true;
                FlagXLoc_textbox.Text = selectedFlag.Location.X.ToString();
                FlagYLoc_textbox.Text = selectedFlag.Location.Y.ToString();
                FlagName_textbox.Text = selectedFlag.Name;
                FlagFilename_textbox.Text = selectedFlag.FileName;
            }
            else
            {
                Flag_gb.Enabled = false;
                FlagXLoc_textbox.Text = string.Empty;
                FlagYLoc_textbox.Text = string.Empty;
                FlagName_textbox.Text = string.Empty;
                FlagFilename_textbox.Text = string.Empty;
            }
        }

        private void UpdateGates()
        {
            if (selectedGate != null)
            {
                Gates_gb.Enabled = true;
                GateIndex_combo.SelectedItem = Envir.MonsterInfoList.FirstOrDefault(x => x.Index == selectedGate.MobIndex);
                GateXLoc_textbox.Text = selectedGate.Location.X.ToString();
                GateYLoc_textbox.Text = selectedGate.Location.Y.ToString();
                GateName_textbox.Text = selectedGate.Name;
                GateCost_textbox.Text = selectedGate.RepairCost.ToString();

            }
            else
            {
                Gates_gb.Enabled = false;
                GateIndex_combo.SelectedItem = -1;
                GateXLoc_textbox.Text = string.Empty;
                GateYLoc_textbox.Text = string.Empty;
                GateName_textbox.Text = string.Empty;
                GateCost_textbox.Text = string.Empty;
            }

        }

        private void UpdateWalls()
        {
            if (selectedWall != null)
            {
                Walls_gb.Enabled = true;
                WallIndex_combo.SelectedItem = Envir.MonsterInfoList.FirstOrDefault(x => x.Index == selectedWall.MobIndex);
                WallXLoc_textbox.Text = selectedWall.Location.X.ToString();
                WallYLoc_textbox.Text = selectedWall.Location.Y.ToString();
                WallName_textbox.Text = selectedWall.Name;
                WallCost_textbox.Text = selectedWall.RepairCost.ToString();

            }
            else
            {
                Walls_gb.Enabled = false;
                WallIndex_combo.SelectedItem = -1;
                WallXLoc_textbox.Text = string.Empty;
                WallYLoc_textbox.Text = string.Empty;
                WallName_textbox.Text = string.Empty;
                WallCost_textbox.Text = string.Empty;
            }

        }

        private void UpdateSiege()
        {
            if (selectedSiege != null)
            {
                Siege_gb.Enabled = true;
                SiegeIndex_combo.SelectedItem = Envir.MonsterInfoList.FirstOrDefault(x => x.Index == selectedSiege.MobIndex);
                SiegeXLoc_textbox.Text = selectedSiege.Location.X.ToString();
                SiegeYLoc_textbox.Text = selectedSiege.Location.Y.ToString();
                SiegeName_textbox.Text = selectedSiege.Name;
                SiegeCost_textbox.Text = selectedSiege.RepairCost.ToString();

            }
            else
            {
                Siege_gb.Enabled = false;
                SiegeIndex_combo.SelectedIndex = -1;
                SiegeXLoc_textbox.Text = string.Empty;
                SiegeYLoc_textbox.Text = string.Empty;
                SiegeName_textbox.Text = string.Empty;
                SiegeCost_textbox.Text = string.Empty;
            }
        }

        private void UpdateControlPoints()
        {
            if (selectedControlPoint != null)
            {
                Control_gb.Enabled = true;
                ControlXLoc_textbox.Text = selectedControlPoint.Location.X.ToString();
                ControlYLoc_textbox.Text = selectedControlPoint.Location.Y.ToString();
                ControlName_textbox.Text = selectedControlPoint.Name;
                ControlFilename_textbox.Text = selectedControlPoint.FileName;
            }
            else
            {
                Control_gb.Enabled = false;
                ControlXLoc_textbox.Text = string.Empty;
                ControlYLoc_textbox.Text = string.Empty;
                ControlName_textbox.Text = string.Empty;
                ControlFilename_textbox.Text = string.Empty;
            }
        }

        private void UpdateInterface()
        {

            if (ConquestInfoListBox.Items.Count != Envir.ConquestInfos.Count)
            {
                ConquestInfoListBox.Items.Clear();

                for (int i = 0; i < Envir.ConquestInfos.Count; i++)
                {
                    ConquestInfoListBox.Items.Add(Envir.ConquestInfos[i]);
                }
            }

            selectedConquest = (ConquestInfo)ConquestInfoListBox.SelectedItem;


            Maps_listbox.Items.Clear();
            Guards_listbox.Items.Clear();
            Gates_listbox.Items.Clear();
            Walls_listbox.Items.Clear();
            Siege_listbox.Items.Clear();
            Flags_listbox.Items.Clear();
            Index_textbox.Text = string.Empty;
            Name_textbox.Text = string.Empty;
            FullMap_checkbox.Checked = false;
            LocX_textbox.Text = string.Empty;
            LocY_textbox.Text = string.Empty;
            Size_textbox.Text = string.Empty;
            ObLocX_textbox.Text = string.Empty;
            ObLocY_textbox.Text = string.Empty;
            ObSize_textbox.Text = string.Empty;
            Controls_listbox.Items.Clear();
            ConquestMap_combo.SelectedIndex = -1;
            PalaceMap_combo.SelectedIndex = -1;
            ExtraMaps_combo.SelectedIndex = -1;
            WarType_combo.SelectedIndex = -1;
            WarMode_combo.SelectedIndex = -1;
            ArcherIndex_combo.SelectedIndex = -1;
            ArchXLoc_textbox.Text = string.Empty;
            ArchYLoc_textbox.Text = string.Empty;
            Archer_gb.Enabled = false;
            SiegeIndex_combo.SelectedIndex = -1;
            SiegeXLoc_textbox.Text = string.Empty;
            SiegeYLoc_textbox.Text = string.Empty;
            Siege_gb.Enabled = false;
            SiegeName_textbox.Text = string.Empty;
            Main_tabs.Enabled = false;
            WarLength_num.Value = 60;
            StartHour_num.Value = 1;
            Mon_checkbox.Checked = false;
            Tue_checkbox.Checked = false;
            Wed_checkbox.Checked = false;
            Thu_checkbox.Checked = false;
            Fri_checkbox.Checked = false;
            Sat_checkbox.Checked = false;
            Sun_checkbox.Checked = false;

            if (selectedConquest != null)
            {
                Main_tabs.Enabled = true;
                
                Index_textbox.Text = selectedConquest.Index.ToString();
                Name_textbox.Text = selectedConquest.Name.ToString();
                FullMap_checkbox.Checked = selectedConquest.FullMap;
                LocX_textbox.Text = selectedConquest.Location.X.ToString();
                LocY_textbox.Text = selectedConquest.Location.Y.ToString();
                Size_textbox.Text = selectedConquest.Size.ToString();
                ObLocX_textbox.Text = selectedConquest.KingLocation.X.ToString();
                ObLocY_textbox.Text = selectedConquest.KingLocation.Y.ToString();
                ObSize_textbox.Text = selectedConquest.KingSize.ToString();
                ConquestMap_combo.SelectedItem = Envir.MapInfoList.FirstOrDefault(x => x.Index == selectedConquest.MapIndex);
                PalaceMap_combo.SelectedItem = Envir.MapInfoList.FirstOrDefault(x => x.Index == selectedConquest.PalaceIndex);
                WarMode_combo.SelectedItem = selectedConquest.Game;
                WarType_combo.SelectedItem = selectedConquest.Type;
                WarLength_num.Value = selectedConquest.WarLength;
                StartHour_num.Value = selectedConquest.StartHour;
                Mon_checkbox.Checked = selectedConquest.Monday;
                Tue_checkbox.Checked = selectedConquest.Tuesday;
                Wed_checkbox.Checked = selectedConquest.Wednesday;
                Thu_checkbox.Checked = selectedConquest.Thursday;
                Fri_checkbox.Checked = selectedConquest.Friday;
                Sat_checkbox.Checked = selectedConquest.Saturday;
                Sun_checkbox.Checked = selectedConquest.Sunday;
                for (int i = 0; i < selectedConquest.ConquestGuards.Count; i++)
                {
                    Guards_listbox.Items.Add(selectedConquest.ConquestGuards[i]);
                }

                for (int i = 0; i < selectedConquest.ExtraMaps.Count; i++)
                {
                    Maps_listbox.Items.Add(Envir.MapInfoList.FirstOrDefault(x => x.Index == selectedConquest.ExtraMaps[i]));
                }

                for (int i = 0; i < selectedConquest.ConquestGates.Count; i++)
                {
                    Gates_listbox.Items.Add(selectedConquest.ConquestGates[i]);
                }
                for (int i = 0; i < selectedConquest.ConquestWalls.Count; i++)
                {
                    Walls_listbox.Items.Add(selectedConquest.ConquestWalls[i]);
                }
                for (int i = 0; i < selectedConquest.ConquestSieges.Count; i++)
                {
                    Siege_listbox.Items.Add(selectedConquest.ConquestSieges[i]);
                }

                for (int i = 0; i < selectedConquest.ConquestFlags.Count; i++)
                {
                    Flags_listbox.Items.Add(selectedConquest.ConquestFlags[i]);
                }

                for (int i = 0; i < selectedConquest.ControlPoints.Count; i++)
                {
                    Controls_listbox.Items.Add(selectedConquest.ControlPoints[i]);
                }

            }

        }

        private void ConquestInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            UpdateInterface();
        }

        private void AddConq_button_Click(object sender, EventArgs e)
        {
            Envir.ConquestInfos.Add(new ConquestInfo { Index = ++Envir.ConquestIndex, Location = new Point(0, 0), Size = 10, Name = "Conquest Wall", MapIndex = 1, PalaceIndex = 2});
            UpdateInterface();
        }

        private void AddGuard_button_Click(object sender, EventArgs e)
        {
            
            if (selectedConquest != null)
            {
                 selectedConquest.ConquestGuards.Add(new ConquestArcherInfo { Location = new Point(0, 0), Name = "Guard", Index = ++selectedConquest.GuardIndex, MobIndex = 1, RepairCost = 1000 });
                 UpdateInterface();
            }
            
                
        }

        private void Guards_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Guards_listbox.SelectedIndex != -1)
            {
                selectedArcher = (ConquestArcherInfo)Guards_listbox.SelectedItem;
                UpdateArchers();
            }
            else
                selectedArcher = null;
            
        }

        private void AddExtraMap_button_Click(object sender, EventArgs e)
        {
            if (selectedConquest != null && ExtraMaps_combo.SelectedIndex != -1)
            {
                MapInfo temp = (MapInfo)ExtraMaps_combo.SelectedItem;
                selectedConquest.ExtraMaps.Add(temp.Index);
                UpdateInterface();
            }

        }

        private void ConquestInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Envir.SaveDB();
        }

        private void Name_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedConquest.Name = ActiveControl.Text;
        }

        private void LocX_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            selectedConquest.Location.X = temp;
        }

        private void LocY_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            selectedConquest.Location.Y = temp;
        }

        private void Size_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            selectedConquest.Size = temp;
        }

        private void ArchXLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedArcher.Location.X = temp;
        }

        private void ArchYLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedArcher.Location.Y = temp;
        }

        private void ConquestMap_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MapInfo temp = (MapInfo)ConquestMap_combo.SelectedItem;
            selectedConquest.MapIndex = temp.Index;

        }

        private void PalaceMap_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MapInfo temp = (MapInfo)PalaceMap_combo.SelectedItem;
            selectedConquest.PalaceIndex = temp.Index;

        }

        private void ArcherIndex_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MonsterInfo temp = (MonsterInfo)ArcherIndex_combo.SelectedItem;
            selectedArcher.MobIndex = temp.Index;
        }

        private void ArcherName_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedArcher.Name = ActiveControl.Text;
        }

        private void AddGate_button_Click(object sender, EventArgs e)
        {
            if (selectedConquest != null)
            {
                selectedConquest.ConquestGates.Add(new ConquestGateInfo { Location = new Point(0, 0), Name = "Gate", Index = ++selectedConquest.GateIndex, MobIndex = 1, RepairCost = 1000 });
                UpdateInterface();
            }
        }

        private void Gates_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ActiveControl != sender) return;
            if (Gates_listbox.SelectedIndex != -1)
            {
                selectedGate = (ConquestGateInfo)Gates_listbox.SelectedItem;
                UpdateGates();
            }
            else
                selectedGate = null;
        }

        private void GateXLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedGate.Location.X = temp;
        }

        private void GateIndex_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MonsterInfo temp = (MonsterInfo)GateIndex_combo.SelectedItem;
            selectedGate.MobIndex = temp.Index;
        }

        private void GateYLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedGate.Location.Y = temp;
        }

        private void AddWall_button_Click(object sender, EventArgs e)
        {
            if (selectedConquest != null)
            {
                selectedConquest.ConquestWalls.Add(new ConquestWallInfo { Location = new Point(0, 0), Name = "Wall", Index = ++selectedConquest.WallIndex, MobIndex = 1, RepairCost = 1000 });
                UpdateInterface();
            }
        }

        private void Walls_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Walls_listbox.SelectedIndex != -1)
            {
                selectedWall = (ConquestWallInfo)Walls_listbox.SelectedItem;
                UpdateWalls();
            }
            else
                selectedWall = null;
        }

        private void WallIndex_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MonsterInfo temp = (MonsterInfo)WallIndex_combo.SelectedItem;
            selectedWall.MobIndex = temp.Index;
        }

        private void WallXLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedWall.Location.X = temp;
        }

        private void WallYLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedWall.Location.Y = temp;
        }

        private void ArcherCost_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedArcher.RepairCost = temp;
        }

        private void GateCost_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedGate.RepairCost = temp;
        }

        private void WallCost_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedWall.RepairCost = temp;
        }

        private void GateName_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedGate.Name = ActiveControl.Text;
        }

        private void Walls_gb_Enter(object sender, EventArgs e)
        {

        }

        private void WallName_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedWall.Name = ActiveControl.Text;
        }


        private void WarType_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
                selectedConquest.Type = (ConquestType)WarType_combo.SelectedItem;
        }

        private void WarMode_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedConquest.Game = (ConquestGame)WarMode_combo.SelectedItem;
        }

        private void WarTimes_gb_Enter(object sender, EventArgs e)
        {

        }

        private void StartHour_num_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
                selectedConquest.StartHour = (byte)StartHour_num.Value;
        }

        private void WarLength_num_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
                selectedConquest.WarLength = (int)WarLength_num.Value;
        }

        private void Mon_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            selectedConquest.Monday = Mon_checkbox.Checked;
        }

        private void Tue_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            selectedConquest.Tuesday = Tue_checkbox.Checked;
        }

        private void Wed_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            selectedConquest.Wednesday = Wed_checkbox.Checked;
        }

        private void Thu_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            selectedConquest.Thursday = Thu_checkbox.Checked;
        }

        private void Fri_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            selectedConquest.Friday = Fri_checkbox.Checked;
        }

        private void Sat_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            selectedConquest.Saturday = Sat_checkbox.Checked;
        }

        private void Sun_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            selectedConquest.Sunday = Sun_checkbox.Checked;
        }

        private void RemoveConq_button_Click(object sender, EventArgs e)
        {
            if (selectedConquest == null) return;

            if (MessageBox.Show("Are you sure you want to remove the selected Conquest?", "Remove Items?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            Envir.ConquestInfos.Remove(selectedConquest);

            if (Envir.ConquestInfos.Count == 0) Envir.ConquestIndex = 0;

            UpdateInterface();

        }

        private void RemoveMap_button_Click(object sender, EventArgs e)
        {
            if (Maps_listbox.SelectedItem != null)
                selectedConquest.ExtraMaps.Remove(((MapInfo)Maps_listbox.SelectedItem).Index);

            UpdateInterface(); 
        }

        private void Maps_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RemoveGuard_button_Click(object sender, EventArgs e)
        {
            if (Guards_listbox.SelectedItem != null)
                selectedConquest.ConquestGuards.Remove((ConquestArcherInfo)Guards_listbox.SelectedItem);

            UpdateInterface();
        }

        private void RemoveGate_button_Click(object sender, EventArgs e)
        {
            if (Gates_listbox.SelectedItem != null)
                selectedConquest.ConquestGates.Remove((ConquestGateInfo)Gates_listbox.SelectedItem);

            UpdateInterface();
        }

        private void RemoveWall_button_Click(object sender, EventArgs e)
        {
            if (Walls_listbox.SelectedItem != null)
                selectedConquest.ConquestWalls.Remove((ConquestWallInfo)Walls_listbox.SelectedItem);

            UpdateInterface();
        }

        private void ObLocX_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            selectedConquest.KingLocation.X = temp;
        }

        private void ObLocY_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            selectedConquest.KingLocation.Y = temp;
        }

        private void ObSize_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            selectedConquest.KingSize = temp;
        }

        private void Siege_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Siege_listbox.SelectedIndex != -1)
            {
                selectedSiege = (ConquestSiegeInfo)Siege_listbox.SelectedItem;
                UpdateSiege();
            }
            else
                selectedSiege = null;
        }

        private void AddSiege_button_Click(object sender, EventArgs e)
        {
            if (selectedConquest != null)
            {
                selectedConquest.ConquestSieges.Add(new ConquestSiegeInfo { Location = new Point(0, 0), Name = "Siege", Index = ++selectedConquest.SiegeIndex, MobIndex = 1, RepairCost = 1000 });
                UpdateInterface();
            }
        }

        private void RemoveSiege_button_Click(object sender, EventArgs e)
        {
            if (Siege_listbox.SelectedItem != null)
                selectedConquest.ConquestSieges.Remove((ConquestSiegeInfo)Siege_listbox.SelectedItem);

            UpdateInterface();
        }

        private void SiegeXLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedSiege.Location.X = temp;
        }

        private void SiegeYLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedSiege.Location.Y = temp;
        }

        private void SiegeName_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedSiege.Name = ActiveControl.Text;
        }

        private void SiegeCost_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedSiege.RepairCost = temp;
        }

        private void SiegeIndex_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MonsterInfo temp = (MonsterInfo)SiegeIndex_combo.SelectedItem;
            selectedSiege.MobIndex = temp.Index;
        }

        private void RemoveFlag_button_Click(object sender, EventArgs e)
        {
            if (Flags_listbox.SelectedItem != null)
                selectedConquest.ConquestFlags.Remove((ConquestFlagInfo)Flags_listbox.SelectedItem);

            UpdateInterface();
        }

        private void AddFlag_button_Click(object sender, EventArgs e)
        {
            if (selectedConquest != null)
            {
                selectedConquest.ConquestFlags.Add(new ConquestFlagInfo { Location = new Point(0, 0), Name = "Flag", Index = ++selectedConquest.FlagIndex });
                UpdateInterface();
            }
        }

        private void FlagXLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedFlag.Location.X = temp;
        }

        private void FlagYLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedFlag.Location.Y = temp;
        }

        private void FlagName_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedFlag.Name = ActiveControl.Text;
        }

        private void Flags_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Flags_listbox.SelectedIndex != -1)
            {
                selectedFlag = (ConquestFlagInfo)Flags_listbox.SelectedItem;
                UpdateFlags();
            }
            else
                selectedFlag = null;
        }

        private void FlagFilename_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedFlag.FileName = ActiveControl.Text;
        }

        private void FullMap_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedConquest.FullMap = FullMap_checkbox.Checked;
        }



        private void Control_Listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Controls_listbox.SelectedIndex != -1)
            {
                selectedControlPoint = (ConquestFlagInfo)Controls_listbox.SelectedItem;
                UpdateControlPoints();
            }
            else
                selectedControlPoint = null;
        }

        private void AddControl_button_Click(object sender, EventArgs e)
        {
            if (selectedConquest != null)
            {
                selectedConquest.ControlPoints.Add(new ConquestFlagInfo { Location = new Point(0, 0), Name = "Control Point", Index = ++selectedConquest.ControlPointIndex });
                UpdateInterface();
            }
        }

        private void RemoveControl_button_Click(object sender, EventArgs e)
        {
            if (Controls_listbox.SelectedItem != null)
                selectedConquest.ControlPoints.Remove((ConquestFlagInfo)Controls_listbox.SelectedItem);

            UpdateInterface();
        }

        private void ControlXLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedControlPoint.Location.X = temp;
        }

        private void ControlYLoc_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            selectedControlPoint.Location.Y = temp;
        }

        private void ControlName_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedControlPoint.Name = ActiveControl.Text;
        }

        private void ControlFilename_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            selectedControlPoint.FileName = ActiveControl.Text;
        }
    }
}
