using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Server.Database
{
    public partial class ItemInfoFormNew : Form
    {
        public Envir Envir => SMain.EditEnvir;

        private readonly Array StatEnums = Enum.GetValues(typeof(Stat));
        private readonly Array BindEnums = Enum.GetValues(typeof(BindMode));
        private readonly Array SpecialEnums = Enum.GetValues(typeof(SpecialItemMode));

        private DataTable Table;

        public ItemInfoFormNew()
        {
            InitializeComponent();

            InitializeItemInfoFilters();
            InitializeItemInfoGridView();

            CreateDynamicColumns();

            PopulateTable();
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
            Modified.ValueType = typeof(bool);
            ItemIndex.ValueType = typeof(int);
            ItemName.ValueType = typeof(string);
            ItemRandomStatsId.ValueType = typeof(byte);
            ItemRequiredAmount.ValueType = typeof(byte);
            ItemImage.ValueType = typeof(ushort);
            ItemShape.ValueType = typeof(short);
            ItemEffect.ValueType = typeof(byte);
            ItemStackSize.ValueType = typeof(ushort);
            ItemSlots.ValueType = typeof(byte);
            ItemWeight.ValueType = typeof(byte);

            ItemLightIntensity.ValueType = typeof(byte);
            ItemLightRange.ValueType = typeof(byte);
            ItemDurability.ValueType = typeof(ushort);
            ItemPrice.ValueType = typeof(uint);

            //Basic
            this.ItemType.ValueType = typeof(ItemType);
            this.ItemType.DataSource = Enum2DataTable<ItemType>();
            this.ItemType.ValueMember = "Value";
            this.ItemType.DisplayMember = "Display";

            this.ItemGrade.ValueType = typeof(ItemGrade);
            this.ItemGrade.DataSource = Enum2DataTable<ItemGrade>();
            this.ItemGrade.ValueMember = "Value";
            this.ItemGrade.DisplayMember = "Display";

            this.ItemRequiredType.ValueType = typeof(RequiredType);
            this.ItemRequiredType.DataSource = Enum2DataTable<RequiredType>();
            this.ItemRequiredType.ValueMember = "Value";
            this.ItemRequiredType.DisplayMember = "Display";

            this.ItemRequiredGender.ValueType = typeof(RequiredGender);
            this.ItemRequiredGender.DataSource = Enum2DataTable<RequiredGender>();
            this.ItemRequiredGender.ValueMember = "Value";
            this.ItemRequiredGender.DisplayMember = "Display";

            this.ItemRequiredClass.ValueType = typeof(RequiredClass);
            this.ItemRequiredClass.DataSource = Enum2DataTable<RequiredClass>();
            this.ItemRequiredClass.ValueMember = "Value";
            this.ItemRequiredClass.DisplayMember = "Display";

            this.ItemSet.ValueType = typeof(ItemSet);
            this.ItemSet.DataSource = Enum2DataTable<ItemSet>();
            this.ItemSet.ValueMember = "Value";
            this.ItemSet.DisplayMember = "Display";
        }

        public static DataTable Enum2DataTable<T>()
        {
            DataTable enumTable = new DataTable();
            enumTable.Columns.Add(new DataColumn("Value", Enum.GetUnderlyingType(typeof(T))));
            enumTable.Columns.Add(new DataColumn("Display", typeof(string)));
            DataRow EnumRow;
            foreach (T E in Enum.GetValues(typeof(T)))
            {
                EnumRow = enumTable.NewRow();
                EnumRow["Value"] = E;
                EnumRow["Display"] = E.ToString();
                enumTable.Rows.Add(EnumRow);
            }

            return enumTable;
        }

        private void CreateDynamicColumns()
        {
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
                    DataPropertyName = "Stat" + stat.ToString()
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
                    DataPropertyName = "Bind" + bind.ToString()
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
                    DataPropertyName = "Special" + special.ToString()
                };

                itemInfoGridView.Columns.Add(col);
            }

        }

        private void PopulateTable()
        {
            Table = new DataTable("itemInfo");

            foreach (DataGridViewColumn col in itemInfoGridView.Columns)
            {
                Table.Columns.Add(col.DataPropertyName, col.ValueType);
            }

            foreach (ItemInfo item in Envir.ItemInfoList)
            {
                DataRow row = Table.NewRow();

                row["Modified"] = false;

                row["ItemIndex"] = item.Index;
                row["ItemName"] = item.Name;

                row["ItemType"] = item.Type;
                row["ItemGrade"] = item.Grade;
                row["ItemRequiredType"] = item.RequiredType;
                row["ItemRequiredGender"] = item.RequiredGender;
                row["ItemRequiredClass"] = item.RequiredClass;
                row["ItemSet"] = item.Set;
                row["ItemRandomStatsId"] = item.RandomStatsId;
                row["ItemRequiredAmount"] = item.RequiredAmount;
                row["ItemImage"] = item.Image;
                row["ItemShape"] = item.Shape;
                row["ItemEffect"] = item.Effect;
                row["ItemStackSize"] = item.StackSize;
                row["ItemSlots"] = item.Slots;
                row["ItemWeight"] = item.Weight;
                row["ItemLightRange"] = (byte)(item.Light % 15);
                row["ItemLightIntensity"] = (byte)(item.Light / 15);
                row["ItemDurability"] = item.Durability;
                row["ItemPrice"] = item.Price;

                foreach (Stat stat in StatEnums)
                {
                    if (stat == Stat.Unknown) continue;

                    row["Stat" + stat.ToString()] = item.Stats[stat];
                }

                foreach (BindMode bind in BindEnums)
                {
                    if (bind == BindMode.None) continue;

                    row["Bind" + bind.ToString()] = item.Bind.HasFlag(bind);
                }

                foreach (SpecialItemMode special in SpecialEnums)
                {
                    if (special == SpecialItemMode.None) continue;

                    row["Special" + special.ToString()] = item.Unique.HasFlag(special);
                }

                Table.Rows.Add(row);
            }

            itemInfoGridView.DataSource = Table;
        }

        private void UpdateFilter()
        {
            var filterText = txtSearch.Text;
            var filterType = ((ListItem)drpFilterType.SelectedItem)?.Value ?? "-1";

            if (string.IsNullOrEmpty(filterText) && filterType == "-1")
            {
                (itemInfoGridView.DataSource as DataTable).DefaultView.RowFilter = "";
                return;
            }

            string rowFilter = string.Format("([ItemType] = '{0}' OR '{0}' = -1) AND [ItemName] LIKE '%{1}%'", filterType, filterText);

            (itemInfoGridView.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
        }

        private void SaveForm()
        {
            var lastIndex = Envir.ItemInfoList.Max(x => x.Index);

            foreach (DataGridViewRow row in itemInfoGridView.Rows)
            {
                if (string.IsNullOrEmpty((string)row.Cells["ItemName"].Value))
                {
                    continue;
                }

                ItemInfo item;

                if (string.IsNullOrEmpty((string)row.Cells["ItemIndex"].FormattedValue))
                {
                    Envir.ItemInfoList.Add(item = new ItemInfo());

                    item.Index = ++lastIndex;
                }
                else
                {
                    int index = (int)row.Cells["ItemIndex"].Value;

                    item = Envir.ItemInfoList.FirstOrDefault(x => x.Index == index);

                    if (row.Cells["Modified"].Value != null && (bool)row.Cells["Modified"].Value == false) continue;
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

        private DataRow FindRowByItemName(string value)
        {
            foreach (DataRow row in Table.Rows)
            {
                var val = row["ItemName"];

                if (val?.ToString().Equals(value) ?? false)
                {
                    return row;
                }
            }

            return null;
        }

        private void itemInfoGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var col = itemInfoGridView.Columns[e.ColumnIndex];

            var cell = itemInfoGridView.Rows[e.RowIndex].Cells[col.Name];

            if (cell.FormattedValue != null && e.FormattedValue != null && cell.FormattedValue.ToString() == e.FormattedValue.ToString())
            {
                return;
            }

            itemInfoGridView.Rows[e.RowIndex].Cells["Modified"].Value = true;

            var val = e.FormattedValue.ToString();

            itemInfoGridView.Rows[e.RowIndex].ErrorText = "";

            //Only AttackSpeed stat can be negative
            if (col.ValueType == typeof(int) && col.Name != "StatAttackSpeed" && int.TryParse(val, out int val1) && val1 < 0)
            {
                e.Cancel = true;
                itemInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a positive integer";
            }

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
                    if (col.Name == "Modified") continue;

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
                    if (col.Name == "ItemIndex" || col.Name == "ItemName" || col.Name == "Modified")
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
                    if (col.Name == "ItemIndex" || col.Name == "ItemName" || col.Name == "Modified")
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
                    if (col.Name == "ItemIndex" || col.Name == "ItemName" || col.Name == "Modified")
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
                    if (col.Name == "ItemIndex" || col.Name == "ItemName" || col.Name == "Modified")
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
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV (*.csv)|*.csv";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var fileName = ofd.FileName;
                bool fileError = false;

                var rows = File.ReadAllLines(fileName);

                if (rows.Length > 1)
                {
                    var columns = rows[0].Split(',');

                    if (columns.Length < 2)
                    {
                        fileError = true;
                        MessageBox.Show("No columns to import.");
                    }

                    if (!fileError)
                    {
                        itemInfoGridView.EditMode = DataGridViewEditMode.EditProgrammatically;

                        int rowsEdited = 0;

                        for (int i = 1; i < rows.Length; i++)
                        {
                            var row = rows[i];

                            var cells = row.Split(',');

                            if (string.IsNullOrWhiteSpace(cells[0]))
                            {
                                continue;
                            }

                            if (cells.Length != columns.Length)
                            {
                                fileError = true;
                                MessageBox.Show($"Row {i} column count does not match the headers column count.");
                                break;
                            }

                            var dataRow = FindRowByItemName(cells[0]);

                            itemInfoGridView.BeginEdit(true);

                            if (dataRow == null)
                            {
                                dataRow = Table.NewRow();

                                Table.Rows.Add(dataRow);
                            }

                            try
                            {
                                for (int j = 0; j < columns.Length; j++)
                                {
                                    var column = columns[j];

                                    if (string.IsNullOrWhiteSpace(column))
                                    {
                                        continue;
                                    }

                                    var dataColumn = itemInfoGridView.Columns[column];

                                    if (dataColumn == null)
                                    {
                                        fileError = true;
                                        MessageBox.Show($"Column {column} was not found.");
                                        break;
                                    }

                                    if (dataColumn.ValueType.IsEnum)
                                    {
                                        dataRow[column] = Enum.Parse(dataColumn.ValueType, cells[j]);
                                    }
                                    else
                                    {
                                        dataRow[column] = cells[j];
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                                fileError = true;
                                MessageBox.Show($"Error when importing item {cells[0]}. {ex.Message}");
                                continue;
                            }

                            itemInfoGridView.EndEdit();

                            rowsEdited++;

                            if (fileError)
                            {
                                break;
                            }
                        }

                        if (!fileError)
                        {
                            MessageBox.Show($"{rowsEdited} items have been imported.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No rows to import.");
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (itemInfoGridView.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "ItemInfo Output.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = itemInfoGridView.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[itemInfoGridView.Rows.Count + 1];
                            for (int i = 2; i < columnCount; i++)
                            {
                                columnNames += itemInfoGridView.Columns[i].Name.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < itemInfoGridView.Rows.Count; i++)
                            {
                                for (int j = 2; j < columnCount; j++)
                                {
                                    var cell = itemInfoGridView.Rows[i - 1].Cells[j];

                                    var valueType = itemInfoGridView.Columns[j].ValueType;
                                    if (valueType.IsEnum)
                                    {
                                        outputCsv[i] += ((Enum.ToObject(valueType, cell.Value ?? 0))?.ToString() ?? "") + ",";
                                    }
                                    else
                                    {
                                        outputCsv[i] += (cell.Value?.ToString() ?? "") + ",";
                                    }
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully.", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Items To Export.", "Info");
            }
        }

        private void itemInfoGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            var row = e.Row;

            row.Cells["Modified"].Value = (bool)true;
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
            row.Cells["ItemStackSize"].Value = (ushort)1;
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
            SaveForm();
            Envir.SaveDB();
        }

        private void itemInfoGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
