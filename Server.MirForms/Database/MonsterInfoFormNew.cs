using Server.MirDatabase;
using Server.MirEnvir;
using System.Data;
using System.Text;

namespace Server.Database
{
    public partial class MonsterInfoFormNew : Form
    {
        public Envir Envir => SMain.EditEnvir;

        private readonly Array StatEnums = Enum.GetValues(typeof(Stat));

        private DataTable Table;

        public MonsterInfoFormNew()
        {
            InitializeComponent();

            SetDoubleBuffered(monsterInfoGridView);

            InitializeItemInfoGridView();

            CreateDynamicColumns();

            PopulateTable();

            rbtnViewBasic.Checked = true;
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }

        private void InitializeItemInfoGridView()
        {
            Modified.ValueType = typeof(bool);
            MonsterIndex.ValueType = typeof(int);
            MonsterName.ValueType = typeof(string);
            MonsterAI.ValueType = typeof(byte);
            MonsterEffect.ValueType = typeof(byte);
            MonsterLevel.ValueType = typeof(ushort);
            MonsterLight.ValueType = typeof(byte);
            MonsterAttackSpeed.ValueType = typeof(ushort);
            MonsterMoveSpeed.ValueType = typeof(ushort);
            MonsterViewRange.ValueType = typeof(byte);
            MonsterCoolEye.ValueType = typeof(byte);
            MonsterExperience.ValueType = typeof(uint);
            MonsterCanPush.ValueType = typeof(bool);
            MonsterAutoRev.ValueType = typeof(bool);
            MonsterUndead.ValueType = typeof(bool);
            MonsterCanTame.ValueType = typeof(bool);
            MonsterDropPath.ValueType = typeof(string);

            //Basic
            this.MonsterImage.ValueType = typeof(Monster);
            this.MonsterImage.DataSource = Enum2DataTable<Monster>(true);
            this.MonsterImage.ValueMember = "Value";
            this.MonsterImage.DisplayMember = "Display";
        }

        public static DataTable Enum2DataTable<T>(bool sort = false)
        {
            DataTable enumTable = new DataTable();
            enumTable.Columns.Add(new DataColumn("Value", Enum.GetUnderlyingType(typeof(T))));
            enumTable.Columns.Add(new DataColumn("Display", typeof(string)));
            DataRow EnumRow;

            var values = Enum.GetValues(typeof(T)).Cast<T>();

            if (sort)
            {
                values = values.OrderBy(x => x.ToString());
            }

            foreach (T e in values)
            {
                EnumRow = enumTable.NewRow();
                EnumRow["Value"] = e;
                EnumRow["Display"] = e.ToString();
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

                monsterInfoGridView.Columns.Add(col);
            }

        }

        private void PopulateTable()
        {
            Table = new DataTable("monsterInfo");

            foreach (DataGridViewColumn col in monsterInfoGridView.Columns)
            {
                Table.Columns.Add(col.DataPropertyName, col.ValueType);
            }

            foreach (MirDatabase.MonsterInfo item in Envir.MonsterInfoList)
            {
                DataRow row = Table.NewRow();

                row["Modified"] = false;

                row["MonsterIndex"] = item.Index;
                row["MonsterName"] = item.Name;

                row["MonsterImage"] = item.Image;
                row["MonsterAI"] = item.AI;
                row["MonsterLevel"] = item.Level;
                row["MonsterEffect"] = item.Effect;
                row["MonsterLight"] = item.Light;
                row["MonsterAttackSpeed"] = item.AttackSpeed;
                row["MonsterMoveSpeed"] = item.MoveSpeed;
                row["MonsterViewRange"] = item.ViewRange;
                row["MonsterCoolEye"] = item.CoolEye;
                row["MonsterExperience"] = item.Experience;
                row["MonsterCanPush"] = item.CanPush;
                row["MonsterCanTame"] = item.CanTame;
                row["MonsterUndead"] = item.Undead;
                row["MonsterAutoRev"] = item.AutoRev;
                row["MonsterDropPath"] = item.DropPath;

                foreach (Stat stat in StatEnums)
                {
                    if (stat == Stat.Unknown) continue;

                    row["Stat" + stat.ToString()] = item.Stats[stat];
                }

                Table.Rows.Add(row);
            }

            monsterInfoGridView.DataSource = Table;
        }

        private void UpdateFilter()
        {
            var filterText = txtSearch.Text;

            if (string.IsNullOrEmpty(filterText))
            {
                (monsterInfoGridView.DataSource as DataTable).DefaultView.RowFilter = "";
                return;
            }

            string rowFilter = string.Format("[MonsterName] LIKE '%{0}%'", filterText);

            (monsterInfoGridView.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
        }

        private void SaveForm()
        {
            int lastIndex = 0;
            if (Envir.MonsterInfoList.Count > 0)
            {
                lastIndex = Envir.MonsterInfoList.Max(x => x.Index);
            }

            foreach (DataGridViewRow row in monsterInfoGridView.Rows)
            {
                var name = row.Cells["MonsterName"].Value;

                if (name == null || name.GetType() == typeof(System.DBNull) || string.IsNullOrWhiteSpace((string)name))
                {
                    continue;
                }

                MonsterInfo monster;

                if (string.IsNullOrEmpty((string)row.Cells["MonsterIndex"].FormattedValue))
                {
                    Envir.MonsterInfoList.Add(monster = new MonsterInfo());

                    monster.Index = ++lastIndex;
                }
                else
                {
                    int index = (int)row.Cells["MonsterIndex"].Value;

                    monster = Envir.MonsterInfoList.FirstOrDefault(x => x.Index == index);

                    if (row.Cells["Modified"].Value != null && (bool)row.Cells["Modified"].Value == false) continue;
                }
 
                monster.Name = (string)row.Cells["MonsterName"].Value;
                monster.Image = (Monster)row.Cells["MonsterImage"].Value;
                monster.AI = (byte)row.Cells["MonsterAI"].Value;
                monster.Level = (ushort)row.Cells["MonsterLevel"].Value;
                monster.Effect = (byte)row.Cells["MonsterEffect"].Value;
                monster.Light = (byte)row.Cells["MonsterLight"].Value;
                monster.AttackSpeed = (ushort)row.Cells["MonsterAttackSpeed"].Value;
                monster.MoveSpeed = (ushort)row.Cells["MonsterMoveSpeed"].Value;
                monster.ViewRange = (byte)row.Cells["MonsterViewRange"].Value;
                monster.CoolEye = (byte)row.Cells["MonsterCoolEye"].Value;
                monster.Experience = (uint)row.Cells["MonsterExperience"].Value;
                monster.CanPush = (bool)row.Cells["MonsterCanPush"].Value;
                monster.CanTame = (bool)row.Cells["MonsterCanTame"].Value;
                monster.Undead = (bool)row.Cells["MonsterUndead"].Value;
                monster.AutoRev = (bool)row.Cells["MonsterAutoRev"].Value;
                monster.DropPath = (string)row.Cells["MonsterDropPath"].Value;

                monster.Stats.Clear();

                foreach (DataGridViewColumn col in monsterInfoGridView.Columns)
                {
                    if (col.Name.StartsWith("Stat"))
                    {
                        var stat = col.Name.Substring(4);

                        Stat enumStat = (Stat)Enum.Parse(typeof(Stat), stat);

                        monster.Stats[enumStat] = (int)row.Cells[col.Name].Value;
                    }
                }
            }
        }

        private DataRow FindRowByMonsterName(string value)
        {
            foreach (DataRow row in Table.Rows)
            {
                var val = row["MonsterName"];

                if (val?.ToString().Equals(value) ?? false)
                {
                    return row;
                }
            }

            return null;
        }

        private void monsterInfoGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var col = monsterInfoGridView.Columns[e.ColumnIndex];

            var cell = monsterInfoGridView.Rows[e.RowIndex].Cells[col.Name];

            if (cell.FormattedValue != null && e.FormattedValue != null && cell.FormattedValue.ToString() == e.FormattedValue.ToString())
            {
                return;
            }

            monsterInfoGridView.Rows[e.RowIndex].Cells["Modified"].Value = true;

            var val = e.FormattedValue.ToString();

            monsterInfoGridView.Rows[e.RowIndex].ErrorText = "";

            if (col.ValueType == typeof(int) && int.TryParse(val, out int val1) && val1 < 0)
            {
                e.Cancel = true;
                monsterInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a positive integer";
            }

            if (col.ValueType == typeof(int) && !int.TryParse(val, out _))
            {
                e.Cancel = true;
                monsterInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be an integer";
            }
            else if (col.ValueType == typeof(byte) && !byte.TryParse(val, out _))
            {
                e.Cancel = true;
                monsterInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a byte";
            }
            else if (col.ValueType == typeof(short) && !short.TryParse(val, out _))
            {
                e.Cancel = true;
                monsterInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a short";
            }
            else if (col.ValueType == typeof(ushort) && !ushort.TryParse(val, out _))
            {
                e.Cancel = true;
                monsterInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a ushort";
            }
            else if (col.ValueType == typeof(long) && !long.TryParse(val, out _))
            {
                e.Cancel = true;
                monsterInfoGridView.Rows[e.RowIndex].ErrorText = "the value must be a long";
            }
        }

        private void rbtnViewAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnViewAll.Checked)
            {
                foreach (DataGridViewColumn col in monsterInfoGridView.Columns)
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
                foreach (DataGridViewColumn col in monsterInfoGridView.Columns)
                {
                    if (col.Name == "MonsterIndex" || col.Name == "MonsterName" || col.Name == "Modified")
                    {
                        continue;
                    }

                    if (col.Name.StartsWith("Monster"))
                    {
                        col.Visible = true;
                        continue;
                    }

                    if (col.Name.Equals("StatHP") || 
                        col.Name.Equals("StatMinAC") || col.Name.Equals("StatMaxAC") || 
                        col.Name.Equals("StatMinMAC") || col.Name.Equals("StatMaxMAC") || 
                        col.Name.Equals("StatMinDC") || col.Name.Equals("StatMaxDC") ||
                        col.Name.Equals("StatMinMC") || col.Name.Equals("StatMaxMC") ||
                        col.Name.Equals("StatMinSC") || col.Name.Equals("StatMaxSC") ||
                        col.Name.Equals("StatAccuracy") || col.Name.Equals("StatAgility"))
                    {
                        col.Visible = true;
                        continue;
                    }

                    col.Visible = false;
                }
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
                        monsterInfoGridView.EditMode = DataGridViewEditMode.EditProgrammatically;

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

                            var dataRow = FindRowByMonsterName(cells[0]);

                            try
                            {
                                if (dataRow != null)
                                {
                                    monsterInfoGridView.BeginEdit(true);
                                }

                                if (dataRow == null)
                                {
                                    dataRow = Table.NewRow();

                                    Table.Rows.Add(dataRow);
                                }

                                for (int j = 0; j < columns.Length; j++)
                                {
                                    var column = columns[j];

                                    if (string.IsNullOrWhiteSpace(column))
                                    {
                                        continue;
                                    }

                                    var dataColumn = monsterInfoGridView.Columns[column];

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

                                dataRow["Modified"] = true;

                                monsterInfoGridView.EndEdit();
                            }
                            catch (Exception ex)
                            {
                                fileError = true;
                                monsterInfoGridView.EndEdit();

                                MessageBox.Show($"Error when importing item {cells[0]}. {ex.Message}");
                                continue;
                            }

                            rowsEdited++;

                            if (fileError)
                            {
                                break;
                            }
                        }

                        if (!fileError)
                        {
                            monsterInfoGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                            MessageBox.Show($"{rowsEdited} monsters have been imported.");
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
            if (monsterInfoGridView.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "MonsterInfo Output.csv";
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
                            int columnCount = monsterInfoGridView.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[monsterInfoGridView.Rows.Count + 1];
                            for (int i = 2; i < columnCount; i++)
                            {
                                columnNames += monsterInfoGridView.Columns[i].Name.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < monsterInfoGridView.Rows.Count; i++)
                            {
                                for (int j = 2; j < columnCount; j++)
                                {
                                    var cell = monsterInfoGridView.Rows[i - 1].Cells[j];

                                    var valueType = monsterInfoGridView.Columns[j].ValueType;
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
                MessageBox.Show("No Monsters To Export.", "Info");
            }
        }

        private void monsterInfoGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            var row = e.Row;

            row.Cells["Modified"].Value = (bool)true;

            row.Cells["MonsterName"].Value = "";
            row.Cells["MonsterImage"].Value = (Monster)0;
            row.Cells["MonsterAI"].Value = (byte)0;
            row.Cells["MonsterLevel"].Value = (ushort)0;
            row.Cells["MonsterEffect"].Value = (byte)0;
            row.Cells["MonsterLight"].Value = (byte)0;
            row.Cells["MonsterAttackSpeed"].Value = (ushort)2500;
            row.Cells["MonsterMoveSpeed"].Value = (ushort)1800;
            row.Cells["MonsterViewRange"].Value = (byte)7;
            row.Cells["MonsterCoolEye"].Value = (byte)0;
            row.Cells["MonsterExperience"].Value = (uint)0;
            row.Cells["MonsterCanPush"].Value = (bool)true;
            row.Cells["MonsterCanTame"].Value = (bool)true;
            row.Cells["MonsterUndead"].Value = (bool)false;
            row.Cells["MonsterAutoRev"].Value = (bool)true;
            row.Cells["MonsterDropPath"].Value = "";

            foreach (Stat stat in StatEnums)
            {
                if (stat == Stat.Unknown) continue;

                row.Cells["Stat" + stat.ToString()].Value = 0;
            }
        }

        private void monsterInfoGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;

            if (row.Cells["MonsterIndex"].Value != null)
            {
                int index = (int)row.Cells["MonsterIndex"].Value;

                var item = Envir.MonsterInfoList.FirstOrDefault(x => x.Index == index);

                Envir.MonsterInfoList.Remove(item);
            }
        }

        private void monsterInfoFormNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveForm();
            Envir.SaveDB();
        }

        private void monsterInfoGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}