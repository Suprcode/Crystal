using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class ChangePasswordDialog : Form
    {
        public ChangePasswordDialog()
        {
            InitializeComponent();

            PasswordTextBox.MaxLength = Globals.MaxPasswordLength;
        }
    }
}
