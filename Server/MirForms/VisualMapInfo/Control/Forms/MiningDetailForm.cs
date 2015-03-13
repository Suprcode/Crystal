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
    public partial class MiningDetailForm : Form
    {
        public MiningDetailForm()
        {
            InitializeComponent();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            if (X.Text == string.Empty) X.Text = "0";
            if (Y.Text == string.Empty) Y.Text = "0";
            if (Range.Text == string.Empty) Range.Text = "0";

            this.Close();
        }

        private void Insert(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsDigit(e.KeyChar)) && (!char.IsControl(e.KeyChar));
        }
    }
}
