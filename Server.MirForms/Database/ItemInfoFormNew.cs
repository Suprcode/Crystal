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

        private readonly Array StatEnums = Enum.GetValues(typeof(Stat));
        private readonly Array BindEnums = Enum.GetValues(typeof(BindMode));
        private readonly Array SpecialEnums = Enum.GetValues(typeof(SpecialItemMode));

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

            foreach (Stat stat in StatEnums)
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

            foreach (BindMode bind in BindEnums)
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

            foreach (SpecialItemMode special in SpecialEnums)
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
                row.Cells["ItemRandomStatsId"].Value = item.RandomStatsId;
                row.Cells["ItemRequiredAmount"].Value = item.RequiredAmount;
                row.Cells["ItemImage"].Value = item.Image;
                row.Cells["ItemShape"].Value = item.Shape;
                row.Cells["ItemEffect"].Value = item.Effect;
                row.Cells["ItemStackSize"].Value = item.StackSize;
                row.Cells["ItemSlots"].Value = item.Slots;
                row.Cells["ItemWeight"].Value = item.Weight;
                row.Cells["ItemLightRange"].Value = (byte)(item.Light % 15);
                row.Cells["ItemLightIntensity"].Value = (byte)(item.Light / 15);
                row.Cells["ItemDurability"].Value = item.Durability;
                row.Cells["ItemPrice"].Value = item.Price;

                foreach (Stat stat in StatEnums)
                {
                    if (stat == Stat.Unknown) continue;

                    row.Cells["Stat" + stat.ToString()].Value = item.Stats[stat];
                }

                foreach (BindMode bind in BindEnums)
                {
                    if (bind == BindMode.None) continue;

                    row.Cells["Bind" + bind.ToString()].Value = item.Bind.HasFlag(bind);
                }

                foreach (SpecialItemMode special in SpecialEnums)
                {
                    if (special == SpecialItemMode.None) continue;

                    row.Cells["Special" + special.ToString()].Value = item.Unique.HasFlag(special);
                }
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

        private void Save()
        {
            var lastIndex = Envir.ItemInfoList.Max(x => x.Index);

            foreach (DataGridViewRow row in itemInfoGridView.Rows)
            {
                if (string.IsNullOrEmpty((string)row.Cells["ItemName"].Value))
                {
                    continue;
                }

                ItemInfo item;

                if (row.Cells["ItemIndex"].Value == null)
                {
                    Envir.ItemInfoList.Add(item = new ItemInfo());

                    item.Index = ++lastIndex;
                }
                else
                {
                    int index = (int)row.Cells["ItemIndex"].Value;

                    item = Envir.ItemInfoList.FirstOrDefault(x => x.Index == index);
                }

                item.Name = (string)row.Cells["ItemName"].Value;
                item.Type = (ItemType)row.Cells["ItemType"].Value;
                item.Grade = (ItemGrade)row.Cells["ItemGrade"].Value;
                item.RequiredType = (RequiredType)row.Cells["ItemRequiredType"].Value;
                item.RequiredGender = (RequiredGender)row.Cells["ItemRequiredGender"].Value;
                item.RequiredClass = (RequiredClass)row.Cells["ItemRequiredClass"].Value;
                item.Set = (ItemSet)row.Cells["ItemSet"].Value;
                item.RandomStatsId = (byte)row.Cells["ItemRandomStatsId"].Value;
                item.RequiredAmount = (byte)row.Cells["ItemRequiredAmount"].Value;
                item.Image = (ushort)row.Cells["ItemImage"].Value;
                item.Shape = (short)row.Cells["ItemShape"].Value;
                item.Effect = (byte)row.Cells["ItemEffect"].Value;
                item.StackSize = (ushort)row.Cells["ItemStackSize"].Value;
                item.Slots = (byte)row.Cells["ItemSlots"].Value;
                item.Weight = (byte)row.Cells["ItemWeight"].Value;

                var light = ((byte)row.Cells["ItemLightRange"].Value % 15) + ((byte)row.Cells["ItemLightIntensity"].Value * 15);
                item.Light = (byte)Math.Min(byte.MaxValue, light);
                item.Durability = (ushort)row.Cells["ItemDurability"].Value;
                item.Price = (uint)row.Cells["ItemPrice"].Value;

                item.Stats.Clear();
                item.Bind = BindMode.None;
                item.Unique = SpecialItemMode.None;

                foreach (DataGridViewColumn col in itemInfoGridView.Columns)
                {
                    if (col.Name.StartsWith("Stat"))
                    {
                        var stat = col.Name.Substring(4);

                        Stat enumStat = (Stat)Enum.Parse(typeof(Stat), stat);

                        item.Stats[enumStat] = (int)row.Cells[col.Name].Value;
                    }
                    else if (col.Name.StartsWith("Bind"))
                    {
                        var bind = col.Name.Substring(4);

                        BindMode enumBind = (BindMode)Enum.Parse(typeof(BindMode), bind);

                        if ((bool)row.Cells[col.Name].Value)
                        {
                            item.Bind |= enumBind;
                        }
                    }
                    else if (col.Name.StartsWith("Special"))
                    {
                        var special = col.Name.Substring(7);

                        SpecialItemMode enumSpecial = (SpecialItemMode)Enum.Parse(typeof(SpecialItemMode), special);

                        if ((bool)row.Cells[col.Name].Value)
                        {
                            item.Unique |= enumSpecial;
                        }
                    }
                }
            }
        }

        private void itemInfoGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var col = itemInfoGridView.Columns[e.ColumnIndex];

            var val = e.FormattedValue.ToString();

            if (col.ValueType == null && col.Name.Length > 4)
            {
                Type t = typeof(ItemInfo);

                var field = t.GetField(col.Name.Substring(4));

                if (field != null)
                {
                    col.ValueType = field.FieldType;
                }
            }

            itemInfoGridView.Rows[e.RowIndex].ErrorText = "";

            if (col.ValueType == typeof(int) && !int.TryParse(val, out _))
            {
                e.Cancel = true;
                itemInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be an integer";
            }
            else if (col.ValueType == typeof(byte) && !byte.TryParse(val, out _))
            {
                e.Cancel = true;
                itemInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a byte";
            }
            else if (col.ValueType == typeof(short) && !short.TryParse(val, out _))
            {
                e.Cancel = true;
                itemInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a short";
            }
            else if (col.ValueType == typeof(ushort) && !ushort.TryParse(val, out _))
            {
                e.Cancel = true;
                itemInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a ushort";
            }
            else if (col.ValueType == typeof(long) && !long.TryParse(val, out _))
            {
                e.Cancel = true;
                itemInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a long";
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

        private void btnImport_Click(object sender, EventArgs e)
        {
            //TODO - export all visible as CSV
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //TODO - import all and match on itemname
        }

        private void itemInfoGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            var row = e.Row;

            row.Cells["ItemName"].Value = "";

            row.Cells["ItemType"].Value = (ItemType)0;
            row.Cells["ItemGrade"].Value = (ItemGrade)0;
            row.Cells["ItemRequiredType"].Value = (RequiredType)0;
            row.Cells["ItemRequiredGender"].Value = RequiredGender.None;
            row.Cells["ItemRequiredClass"].Value = RequiredClass.None;
            row.Cells["ItemSet"].Value = (ItemSet)0;
            row.Cells["ItemRandomStatsId"].Value = (byte)0;
            row.Cells["ItemRequiredAmount"].Value = (byte)0;
            row.Cells["ItemImage"].Value = (ushort)0;
            row.Cells["ItemShape"].Value = (short)0;
            row.Cells["ItemEffect"].Value = (byte)0;
            row.Cells["ItemStackSize"].Value = (ushort)0;
            row.Cells["ItemSlots"].Value = (byte)0;
            row.Cells["ItemWeight"].Value = (byte)0;
            row.Cells["ItemLightRange"].Value = (byte)0;
            row.Cells["ItemLightIntensity"].Value = (byte)0;
            row.Cells["ItemDurability"].Value = (ushort)0;
            row.Cells["ItemPrice"].Value = (uint)0;

            foreach (Stat stat in StatEnums)
            {
                if (stat == Stat.Unknown) continue;

                row.Cells["Stat" + stat.ToString()].Value = 0;
            }

            foreach (BindMode bind in BindEnums)
            {
                if (bind == BindMode.None) continue;

                row.Cells["Bind" + bind.ToString()].Value = false;
            }

            foreach (SpecialItemMode special in SpecialEnums)
            {
                if (special == SpecialItemMode.None) continue;

                row.Cells["Special" + special.ToString()].Value = false;
            }
        }

        private void itemInfoGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;

            if (row.Cells["ItemIndex"].Value != null)
            {
                int index = (int)row.Cells["ItemIndex"].Value;

                var item = Envir.ItemInfoList.FirstOrDefault(x => x.Index == index);

                Envir.ItemInfoList.Remove(item);
            }
        }

        private void ItemInfoFormNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            Save();
            Envir.SaveDB();
        }
    }
}
