using Shared;

namespace Server.Account
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
