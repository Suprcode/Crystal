using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Database
{
    public partial class ItemInfoFormNew : Form
    {
        public Envir Envir => SMain.EditEnvir;

        public ItemInfoFormNew()
        {
            InitializeComponent();
            InitializeItemInfoFilters();
            InitializeItemInfoGridView();

            UpdateItemInfoGridView();

        }

        private void InitializeItemInfoFilters()
        {
            var types = Enum.GetValues(typeof(ItemType));
            drpFilterType.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            foreach (ItemType type in types)
            {
                drpFilterType.Items.Add(new System.Web.UI.WebControls.ListItem(type.ToString(), ((byte)type).ToString()));
            }
        }

        private void InitializeItemInfoGridView()
        {
            //Basic
            this.ItemType.ValueType = typeof(ItemType);
            this.ItemType.DataSource = Enum.GetValues(typeof(ItemType));

            this.ItemGrade.ValueType = typeof(ItemGrade);
            this.ItemGrade.DataSource = Enum.GetValues(typeof(ItemGrade));

            this.ItemRequiredType.ValueType = typeof(RequiredType);
            this.ItemRequiredType.DataSource = Enum.GetValues(typeof(RequiredType));

            this.ItemRequiredGender.ValueType = typeof(RequiredGender);
            this.ItemRequiredGender.DataSource = Enum.GetValues(typeof(RequiredGender));

            this.ItemRequiredClass.ValueType = typeof(RequiredClass);
            this.ItemRequiredClass.DataSource = Enum.GetValues(typeof(RequiredClass));

            this.ItemSet.ValueType = typeof(ItemSet);
            this.ItemSet.DataSource = Enum.GetValues(typeof(ItemSet));
        }

        private void UpdateItemInfoGridView()
        {
            itemInfoGridView.Rows.Clear();

            var statEnums = Enum.GetValues(typeof(Stat));
            var bindEnums = Enum.GetValues(typeof(BindMode));
            var specialEnums = Enum.GetValues(typeof(SpecialItemMode));

            foreach (Stat stat in statEnums)
            {
                if (stat == Stat.Unknown) continue;

                var key = stat.ToString();
                var strKey = RegexFunctions.SeperateCamelCase(key.Replace("Rate", "").Replace("Multiplier", "").Replace("Percent", ""));

                var sign = "";

                if (key.Contains("Percent"))
                    sign = "%";
                else if (key.Contains("Multiplier"))
                    sign = "x";

                var col = new DataGridViewTextBoxColumn
                {
                    HeaderText = $"{strKey} {sign}",
                    Name = "Stat" + stat.ToString(),
                    ValueType = typeof(int),
                };

                itemInfoGridView.Columns.Add(col);
            }

            foreach (BindMode bind in bindEnums)
            {
                if (bind == BindMode.None) continue;

                var col = new DataGridViewCheckBoxColumn
                {
                    HeaderText = bind.ToString(),
                    Name = "Bind" + bind.ToString(),
                    ValueType = typeof(bool),
                };

                itemInfoGridView.Columns.Add(col);
            }

            foreach (SpecialItemMode special in specialEnums)
            {
                if (special == SpecialItemMode.None) continue;

                var col = new DataGridViewCheckBoxColumn
                {
                    HeaderText = special.ToString(),
                    Name = "Special" + special.ToString(),
                    ValueType = typeof(bool),
                };

                itemInfoGridView.Columns.Add(col);
            }

            foreach (ItemInfo item in Envir.ItemInfoList)
            {
                int rowIndex = itemInfoGridView.Rows.Add();

                var row = itemInfoGridView.Rows[rowIndex];

                row.Cells["ItemIndex"].Value = item.Index;
                row.Cells["ItemName"].Value = item.Name;

                row.Cells["ItemType"].Value = item.Type;
                row.Cells["ItemGrade"].Value = item.Grade;
                row.Cells["ItemRequiredType"].Value = item.RequiredType;
                row.Cells["ItemRequiredGender"].Value = item.RequiredGender;
                row.Cells["ItemRequiredClass"].Value = item.RequiredClass;
                row.Cells["ItemSet"].Value = item.Set;
                row.Cells["ItemRandomStats"].Value = item.RandomStatsId;
                row.Cells["ItemRequiredAmount"].Value = item.RequiredAmount;
                row.Cells["ItemImage"].Value = item.Image;
                row.Cells["ItemShape"].Value = item.Shape;
                row.Cells["ItemEffect"].Value = item.Effect;
                row.Cells["ItemStackSize"].Value = item.StackSize;
                row.Cells["ItemSlots"].Value = item.Slots;
                row.Cells["ItemWeight"].Value = item.Weight;
                row.Cells["ItemLightRange"].Value = item.Light % 15;
                row.Cells["ItemLightIntensity"].Value = item.Light / 15;
                row.Cells["ItemDurability"].Value = item.Durability;
                row.Cells["ItemPrice"].Value = item.Price;

                foreach (Stat stat in statEnums)
                {
                    if (stat == Stat.Unknown) continue;

                    row.Cells["Stat" + stat.ToString()].Value = item.Stats[stat];
                }

                foreach (BindMode bind in bindEnums)
                {
                    if (bind == BindMode.None) continue;

                    row.Cells["Bind" + bind.ToString()].Value = item.Bind.HasFlag(bind);
                }

                foreach (SpecialItemMode special in specialEnums)
                {
                    if (special == SpecialItemMode.None) continue;

                    row.Cells["Special" + special.ToString()].Value = item.Unique.HasFlag(special);
                }
            }
        }

        private void itemInfoGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var col = itemInfoGridView.Columns[e.ColumnIndex];

            var val = e.FormattedValue.ToString();

            if (col.ValueType == typeof(int) && !int.TryParse(val, out _))
            {
                e.Cancel = true;
                itemInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be an integer";
            }
        }

        private void rbtnViewAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnViewAll.Checked)
            {
                foreach (DataGridViewColumn col in itemInfoGridView.Columns)
                {
                    col.Visible = true;
                    continue;
                }
            }
        }

        private void rbtnViewBasic_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnViewBasic.Checked)
            {
                foreach (DataGridViewColumn col in itemInfoGridView.Columns)
                {
                    if (col.Name == "ItemIndex" || col.Name == "ItemName")
                    {
                        continue;
                    }

                    if (col.Name.StartsWith("Item"))
                    {
                        col.Visible = true;
                        continue;
                    }

                    col.Visible = false;
                }
            }
        }

        private void rbtnViewStats_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnViewStats.Checked)
            {
                foreach (DataGridViewColumn col in itemInfoGridView.Columns)
                {
                    if (col.Name == "ItemIndex" || col.Name == "ItemName")
                    {
                        continue;
                    }

                    if (col.Name.StartsWith("Stat"))
                    {
                        col.Visible = true;
                        continue;
                    }

                    col.Visible = false;
                }
            }
        }

        private void rbtnViewBinding_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnViewBinding.Checked)
            {
                foreach (DataGridViewColumn col in itemInfoGridView.Columns)
                {
                    if (col.Name == "ItemIndex" || col.Name == "ItemName")
                    {
                        continue;
                    }

                    if (col.Name.StartsWith("Bind"))
                    {
                        col.Visible = true;
                        continue;
                    }

                    col.Visible = false;
                }
            }
        }

        private void rBtnViewSpecial_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtnViewSpecial.Checked)
            {
                foreach (DataGridViewColumn col in itemInfoGridView.Columns)
                {
                    if (col.Name == "ItemIndex" || col.Name == "ItemName")
                    {
                        continue;
                    }

                    if (col.Name.StartsWith("Special"))
                    {
                        col.Visible = true;
                        continue;
                    }

                    col.Visible = false;
                }
            }
        }

        private void drpFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilter();

            if (drpFilterType.Text == "Gem")
            {
                //TODO - Change columns for gems when gem option is chosen.
            }
            else
            {

            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateFilter();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void UpdateFilter()
        {
            var filterText = txtSearch.Text;
            var filterType = (System.Web.UI.WebControls.ListItem)drpFilterType.SelectedItem;

            foreach (DataGridViewRow row in itemInfoGridView.Rows)
            {
                bool visible = true;

                var itemName = (string)row.Cells["ItemName"].Value;

                if (string.IsNullOrEmpty(itemName)) continue;

                var itemType = ((ItemType)row.Cells["ItemType"].Value).ToString();

                if (!string.IsNullOrWhiteSpace(filterText))
                {
                    if (itemName.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        visible = false;
                    }
                }

                if (visible && filterType != null && filterType.Text != "")
                {
                    if (itemType.IndexOf(filterType.Text, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        visible = false;
                    }
                }

                row.Visible = visible;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            //TODO - export all visible as CSV
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //TODO - import all and match on itemname
        }
    }
}
