using Godot;
using ClientPackets;
using ServerPackets;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class LoginScene : MirScene
    {
        // UI References
        private LineEdit _accountEdit;
        private LineEdit _passwordEdit;
        private Button _loginButton;
        private Label _statusLabel;

        public override void _Ready()
        {
            // Find UI nodes (Assuming exact naming in Scene)
            _accountEdit = GetNode<LineEdit>("VBoxContainer/AccountEdit");
            _passwordEdit = GetNode<LineEdit>("VBoxContainer/PasswordEdit");
            _loginButton = GetNode<Button>("VBoxContainer/LoginButton");
            _statusLabel = GetNode<Label>("VBoxContainer/StatusLabel");

            _loginButton.Pressed += OnLoginButtonPressed;

            // Set defaults from Settings
            _accountEdit.Text = Settings.AccountID;
            _passwordEdit.Text = Settings.Password;
        }

        public override void Process()
        {
            // Frame-based updates if needed
        }

        public override void ProcessPacket(Packet p)
        {
             switch (p)
             {
                 case ServerPackets.LoginSuccess success:
                     _statusLabel.Text = "Login Success! Characters: " + success.Characters.Count;
                     GD.Print("Login Success.");
                     // TODO: Switch to SelectScene
                     break;
                 case ServerPackets.LoginBanned banned:
                     _statusLabel.Text = "Banned: " + banned.Reason;
                     break;
                 case ServerPackets.Login fail:
                     _statusLabel.Text = "Login Failed: " + fail.Result;
                     break;
                 case ServerPackets.NewAccount newAcc:
                      _statusLabel.Text = "Account Creation Result: " + newAcc.Result;
                      break;
             }
        }

        private void OnLoginButtonPressed()
        {
            if (!NetworkManager.Connected)
            {
                _statusLabel.Text = "Not Connected to Server.";
                NetworkManager.Connect(); // Try reconnect
                return;
            }

            string id = _accountEdit.Text;
            string pass = _passwordEdit.Text;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
            {
                _statusLabel.Text = "Please enter ID and Password.";
                return;
            }

            Settings.AccountID = id;
            Settings.Password = pass;

            _statusLabel.Text = "Logging in...";

            // Send Login Packet
            NetworkManager.Enqueue(new ClientPackets.Login { AccountID = id, Password = pass });
        }
    }
}
