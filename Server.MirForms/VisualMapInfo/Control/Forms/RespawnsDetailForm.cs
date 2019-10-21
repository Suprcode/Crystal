using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server.MirForms.VisualMapInfo.Control.Forms
{
    public partial class RespawnsDetailForm : Form
    {
        public RespawnsDetailForm()
        {
            InitializeComponent();
        }

        private void Chk(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsDigit(e.KeyChar)) && (!char.IsControl(e.KeyChar));
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            if (X.Text == string.Empty) X.Text = "0";
            if (Y.Text == string.Empty) Y.Text = "0";
            if (Spread.Text == string.Empty) Spread.Text = "0";
            if (Count.Text == string.Empty) Count.Text = "0";
            if (Delay.Text == string.Empty) Delay.Text = "0";
            if (RoutePath.Text == string.Empty) RoutePath.Text = "";
            if (Direction.Text == string.Empty) Direction.Text = "0";
            if (RDelay.Text == string.Empty) RDelay.Text = "0";

            this.Close();
        }
    }
}
