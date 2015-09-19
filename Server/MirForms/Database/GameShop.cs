using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Server.MirEnvir;

namespace Server
{
    public partial class GameShop : Form
    {

        GameShopItem SelectedItem;

        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        public GameShop()
        {
            InitializeComponent();

            LoadGameShopItems();
        }

        private void GameShop_Load(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        private void GameShop_FormClosed(object sender, FormClosedEventArgs e)
        {
            Envir.SaveDB();
        }

        public class ListBoxItem
        {
            public string DisplayMember { get; set; }
            public object ValueMember { get; set; }

            public override string ToString()
            {
                return DisplayMember;
            }
        }

        private void LoadGameShopItems()
        {
            GameShopListBox.Items.Clear();
            for (int i = 0; i < SMain.EditEnvir.GameShopList.Count; i++)
            {
                //GameShopItems_listbox.Items.Add(Envir.GameShopInfoList[i]);
                GameShopListBox.Items.Add(SMain.EditEnvir.GameShopList[i]);
            }
        }

        private void GameShopListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        public void UpdateInterface(bool refreshList = false)
        {
            GoldPrice_textbox.Text = String.Empty;
            GPPrice_textbox.Text = String.Empty;
            Stock_textbox.Text = String.Empty;
            Individual_checkbox.Checked = false;
            Class_combo.Text = "All";
            Category_textbox.Text = "";
            TopItem_checkbox.Checked = false;
            DealofDay_checkbox.Checked = false;
            CredxGold_textbox.Text = Settings.CredxGold.ToString();
            SelectedItem = (GameShopItem)GameShopListBox.SelectedItem;
            ItemDetails_gb.Visible = false;
            TotalSold_label.Text = "0";
            LeftinStock_label.Text = "";

            if (SelectedItem == null) return;

            ItemDetails_gb.Visible = true;
            GoldPrice_textbox.Text = SelectedItem.GoldPrice.ToString();
            GPPrice_textbox.Text = SelectedItem.CreditPrice.ToString();
            Stock_textbox.Text = SelectedItem.Stock.ToString();
            Individual_checkbox.Checked = SelectedItem.iStock;
            Class_combo.Text = SelectedItem.Class;
            Category_textbox.Text = SelectedItem.Category;
            TopItem_checkbox.Checked = SelectedItem.TopItem;
            DealofDay_checkbox.Checked = SelectedItem.Deal;

            GetStats();
        }

        private void GetStats()
        {
            int purchased;

            SMain.Envir.GameshopLog.TryGetValue(SelectedItem.Info.Index, out purchased);
            TotalSold_label.Text = purchased.ToString();

            if (!Individual_checkbox.Checked && SelectedItem.Stock != 0)
            {
                if (SelectedItem.Stock - purchased >= 0)
                    LeftinStock_label.Text = (SelectedItem.Stock - purchased).ToString();
                else
                    LeftinStock_label.Text = "";
            }
            else if (SelectedItem.Stock == 0)
            {
                LeftinStock_label.Text = "Infinite";
            }
            else if (Individual_checkbox.Checked)
            {
                LeftinStock_label.Text = "Can't calc individual levels";
            }
        }

        private void GoldPrice_textbox_TextChanged(object sender, EventArgs e)
        {

            uint temp;

            if (!uint.TryParse(GoldPrice_textbox.Text, out temp))
            {
                GoldPrice_textbox.BackColor = Color.Red;
                return;
            }

            GoldPrice_textbox.BackColor = SystemColors.Window;
            SelectedItem.GoldPrice = temp;
        }

        private void GPPrice_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            SelectedItem.CreditPrice = temp;

            if (ActiveControl.Text != "") GoldPrice_textbox.Text = (temp * Settings.CredxGold).ToString();
        }

        private void Class_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            string temp = ActiveControl.Text;

            SelectedItem.Class = temp;
        }

        private void TopItem_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            SelectedItem.TopItem = TopItem_checkbox.Checked;
        }

        private void Remove_button_Click(object sender, EventArgs e)
        {
            if (SelectedItem == null) return;

            Envir.Remove(SelectedItem);

            LoadGameShopItems();
            UpdateInterface();
        }

        private void DealofDay_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            SelectedItem.Deal= DealofDay_checkbox.Checked;
        }

        private void Category_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            string temp = ActiveControl.Text;

            SelectedItem.Category = temp;
        }

        private void Stock_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            SelectedItem.Stock = temp;

            GetStats();
        }

        private void Individual_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            SelectedItem.iStock = Individual_checkbox.Checked;
        }

        private void CredxGold_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            short temp;

            if (!short.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.CredxGold = temp;
        }
    }
}
