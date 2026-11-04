namespace Server
{
    public partial class ChangePasswordDialog : Form
    {
        public ChangePasswordDialog()
        {
            InitializeComponent();
            UiText.Apply(this);

            PasswordTextBox.MaxLength = Globals.MaxPasswordLength;
        }
    }
}
