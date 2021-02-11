using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            InitializeItemInfoGridView();

            UpdateItemInfoGridView();
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

            foreach (var stat in statEnums)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    HeaderText = stat.ToString(),
                    Name = "Stat" + stat.ToString(),
                    ValueType = typeof(int),
                };

                itemInfoGridView.Columns.Add(col);
            }

            foreach (var item in Envir.ItemInfoList)
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

                foreach (var stat in statEnums)
                {
                    row.Cells["Stat" + stat.ToString()].Value = item.Stats[(Stat)stat];
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

        }
    }
}
