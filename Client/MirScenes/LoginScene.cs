using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using S = ServerPackets;
using C = ClientPackets;
using System.Collections.Generic;
using System.Linq;

namespace Client.MirScenes
{
    public sealed class LoginScene : MirScene
    {
        private MirAnimatedControl _background;
        public MirLabel Version;

        private LoginDialog _login;
        private NewAccountDialog _account;
        private ChangePasswordDialog _password;

        private MirMessageBox _connectBox;

        private InputKeyDialog _ViewKey;

        public MirImageControl TestLabel, ViolenceLabel, MinorLabel, YouthLabel; 

        public LoginScene()
        {

            SoundManager.PlaySound(SoundList.IntroMusic, true);
            Disposing += (o, e) => SoundManager.StopSound(SoundList.IntroMusic);

            _background = new MirAnimatedControl
                {
                    Animated = false,
                    AnimationCount = 19,
                    AnimationDelay = 100,
                    Index = 0,
                    Library = Libraries.ChrSel,
                    Loop = false,
                    Parent = this,
                };

            _login = new LoginDialog {Parent = _background, Visible = false};
            _login.AccountButton.Click += (o, e) =>
                {
                    _login.Hide();
                    if(_ViewKey != null && !_ViewKey.IsDisposed) _ViewKey.Dispose();
                    _account = new NewAccountDialog { Parent = _background };
                    _account.Disposing += (o1, e1) => _login.Show();
                };
            _login.PassButton.Click += (o, e) =>
                {
                    _login.Hide();
                    if (_ViewKey != null && !_ViewKey.IsDisposed) _ViewKey.Dispose();
                    _password = new ChangePasswordDialog { Parent = _background };
                    _password.Disposing += (o1, e1) => _login.Show();
                };

            _login.ViewKeyButton.Click += (o, e) =>     //ADD
            {
                if (_ViewKey != null && !_ViewKey.IsDisposed) return;

                _ViewKey = new InputKeyDialog(_login) { Parent = _background };
            };

            Version = new MirLabel
                {
                    AutoSize = true,
                    BackColour = Color.FromArgb(200, 50, 50, 50),
                    Border = true,
                    BorderColour = Color.Black,
                    Location = new Point(5, 580),
                    Parent = _background,
                    Text = string.Format("Version: {0}", Application.ProductVersion),
                };

            TestLabel = new MirImageControl
            {
                Index = 79,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(Settings.ScreenWidth - 116, 10),
                Visible = Settings.UseTestConfig
            };

            //ViolenceLabel = new MirImageControl
            //{
            //    Index = 89,
            //    Library = Libraries.Prguse,
            //    Parent = this,
            //    Location = new Point(471, 10)
            //};

            //MinorLabel = new MirImageControl
            //{
            //    Index = 87,
            //    Library = Libraries.Prguse,
            //    Parent = this,
            //    Location = new Point(578, 10)
            //};

            //YouthLabel = new MirImageControl
            //{
            //    Index = 88,
            //    Library = Libraries.Prguse,
            //    Parent = this,
            //    Location = new Point(684, 10)
            //};

            _connectBox = new MirMessageBox("Attempting to connect to the server.", MirMessageBoxButtons.Cancel);
            _connectBox.CancelButton.Click += (o, e) => Program.Form.Close();
            Shown += (sender, args) =>
                {
                    Network.Connect();
                    _connectBox.Show();
                };
        }

        public override void Process()
        {
            if (!Network.Connected && _connectBox.Label != null)
                _connectBox.Label.Text = string.Format("Attempting to connect to the server.\nAttempt:{0}", Network.ConnectAttempt);
        }
        public override void ProcessPacket(Packet p)
        {
            switch (p.Index)
            {
                case (short)ServerPacketIds.Connected:
                    Network.Connected = true;
                    SendVersion();
                    break;
                case (short)ServerPacketIds.ClientVersion:
                    ClientVersion((S.ClientVersion) p);
                    break;
                case (short)ServerPacketIds.NewAccount:
                    NewAccount((S.NewAccount) p);
                    break;
                case (short)ServerPacketIds.ChangePassword:
                    ChangePassword((S.ChangePassword) p);
                    break;
                case (short)ServerPacketIds.ChangePasswordBanned:
                    ChangePassword((S.ChangePasswordBanned) p);
                    break;
                case (short)ServerPacketIds.Login:
                    Login((S.Login) p);
                    break;
                case (short)ServerPacketIds.LoginBanned:
                    Login((S.LoginBanned) p);
                    break;
                case (short)ServerPacketIds.LoginSuccess:
                    Login((S.LoginSuccess) p);
                    break;
                default:
                    base.ProcessPacket(p);
                    break;
            }
        }

        private  void SendVersion()
        {
            _connectBox.Label.Text = "Sending Client Version.";

            C.ClientVersion p = new C.ClientVersion();
            try
            {
                byte[] sum;
                using (MD5 md5 = MD5.Create())
                using (FileStream stream = File.OpenRead(Application.ExecutablePath))
                    sum = md5.ComputeHash(stream);

                p.VersionHash = sum;
                    Network.Enqueue(p);
            }
            catch (Exception ex)
            {
                if (Settings.LogErrors) CMain.SaveError(ex.ToString());
            }
        }
        private void ClientVersion(S.ClientVersion p)
        {
            switch (p.Result)
            {
                case 0:
                    MirMessageBox.Show("Wrong version, please update your game.\nGame will now Close", true);

                    Network.Disconnect();
                    break;
                case 1:
                    _connectBox.Dispose();
                    _login.Show();
                    break;
            }
        }
        private void NewAccount(S.NewAccount p)
        {
            _account.OKButton.Enabled = true;
            switch (p.Result)
            {
                case 0:
                    MirMessageBox.Show("Account creation is currently disabled.");
                    _account.Dispose();
                    break;
                case 1:
                    MirMessageBox.Show("Your AccountID is not acceptable.");
                    _account.AccountIDTextBox.SetFocus();
                    break;
                case 2:
                    MirMessageBox.Show("Your Password is not acceptable.");
                    _account.Password1TextBox.SetFocus();
                    break;
                case 3:
                    MirMessageBox.Show("Your E-Mail Address is not acceptable.");
                    _account.EMailTextBox.SetFocus();
                    break;
                case 4:
                    MirMessageBox.Show("Your User Name is not acceptable.");
                    _account.UserNameTextBox.SetFocus();
                    break;
                case 5:
                    MirMessageBox.Show("Your Secret Question is not acceptable.");
                    _account.QuestionTextBox.SetFocus();
                    break;
                case 6:
                    MirMessageBox.Show("Your Secret Answer is not acceptable.");
                    _account.AnswerTextBox.SetFocus();
                    break;
                case 7:
                    MirMessageBox.Show("An Account with this ID already exists.");
                    _account.AccountIDTextBox.Text = string.Empty;
                    _account.AccountIDTextBox.SetFocus();
                    break;
                case 8:
                    MirMessageBox.Show("Your account was created successfully.");
                    _account.Dispose();
                    break;
            }
        }
        private void ChangePassword(S.ChangePassword p)
        {
            _password.OKButton.Enabled = true;

            switch (p.Result)
            {
                case 0:
                    MirMessageBox.Show("Password Changing is currently disabled.");
                    _password.Dispose();
                    break;
                case 1:
                    MirMessageBox.Show("Your AccountID is not acceptable.");
                    _password.AccountIDTextBox.SetFocus();
                    break;
                case 2:
                    MirMessageBox.Show("The current Password is not acceptable.");
                    _password.CurrentPasswordTextBox.SetFocus();
                    break;
                case 3:
                    MirMessageBox.Show("Your new Password is not acceptable.");
                    _password.NewPassword1TextBox.SetFocus();
                    break;
                case 4:
                    MirMessageBox.Show("The AccountID does not exist.");
                    _password.AccountIDTextBox.SetFocus();
                    break;
                case 5:
                    MirMessageBox.Show("Incorrect Password and AccountID combination.");
                    _password.CurrentPasswordTextBox.SetFocus();
                    _password.CurrentPasswordTextBox.Text = string.Empty;
                    break;
                case 6:
                    MirMessageBox.Show("Your password was changed successfully.");
                    _password.Dispose();
                    break;
            }
        }
        private void ChangePassword(S.ChangePasswordBanned p)
        {
            _password.Dispose();

            TimeSpan d = p.ExpiryDate - CMain.Now;
            MirMessageBox.Show(string.Format("This account is banned.\n\nReason: {0}\nExpiryDate: {1}\nDuration: {2:#,##0} Hours, {3} Minutes, {4} Seconds", p.Reason,
                                             p.ExpiryDate, Math.Floor(d.TotalHours), d.Minutes, d.Seconds ));
        }
        private void Login(S.Login p)
        {
            _login.OKButton.Enabled = true;
            switch (p.Result)
            {
                case 0:
                    MirMessageBox.Show("Logging in is currently disabled.");
                    _login.Clear();
                    break;
                case 1:
                    MirMessageBox.Show("Your AccountID is not acceptable.");
                    _login.AccountIDTextBox.SetFocus();
                    break;
                case 2:
                    MirMessageBox.Show("Your Password is not acceptable.");
                    _login.PasswordTextBox.SetFocus();
                    break;
                case 3:
                    MirMessageBox.Show("The AccountID does not exist.");
                    _login.PasswordTextBox.SetFocus();
                    break;
                case 4:
                    MirMessageBox.Show("Incorrect Password and AccountID combination.");
                    _login.PasswordTextBox.Text = string.Empty;
                    _login.PasswordTextBox.SetFocus();
                    break;
            }
        }
        private void Login(S.LoginBanned p)
        {
            _login.OKButton.Enabled = true;

            TimeSpan d = p.ExpiryDate - CMain.Now;
            MirMessageBox.Show(string.Format("This account is banned.\n\nReason: {0}\nExpiryDate: {1}\nDuration: {2:#,##0} Hours, {3} Minutes, {4} Seconds", p.Reason,
                                             p.ExpiryDate, Math.Floor(d.TotalHours), d.Minutes, d.Seconds));
        }
        private void Login(S.LoginSuccess p)
        {
            Enabled = false;
            _login.Dispose();
            if(_ViewKey != null && !_ViewKey.IsDisposed) _ViewKey.Dispose();

            SoundManager.PlaySound(SoundList.LoginEffect);
            _background.Animated = true;
            _background.AfterAnimation += (o, e) =>
                {
                    Dispose();
                    ActiveScene = new SelectScene(p.Characters);
                };
        }

        public sealed class LoginDialog : MirImageControl
        {
            public MirImageControl TitleLabel, AccountIDLabel, PassLabel;
            public MirButton AccountButton, CloseButton, OKButton, PassButton, ViewKeyButton;
            public MirTextBox AccountIDTextBox, PasswordTextBox;
            private bool _accountIDValid, _passwordValid;

            public LoginDialog()
            {
                Index = 1084;
                Library = Libraries.Prguse;
                Location = new Point((Settings.ScreenWidth - Size.Width)/2, (Settings.ScreenHeight - Size.Height)/2);
                PixelDetect = false;
                Size = new Size(328, 220);

                TitleLabel = new MirImageControl
                    {
                        Index = 30,
                        Library = Libraries.Title,
                        Parent = this,
                    };
                TitleLabel.Location = new Point((Size.Width - TitleLabel.Size.Width)/2, 12);

                AccountIDLabel = new MirImageControl
                    {
                        Index = 31,
                        Library = Libraries.Title,
                        Parent = this,
                        Location = new Point(52, 83),
                    };

                PassLabel = new MirImageControl
                    {
                        Index = 32,
                        Library = Libraries.Title,
                        Parent = this,
                        Location = new Point(43, 105)
                    };

                OKButton = new MirButton
                    {
                        Enabled = false,
                        Size = new Size(42,42),
                        HoverIndex = 321,
                        Index = 320,
                        Library = Libraries.Title,
                        Location = new Point(227, 81),
                        Parent = this,
                        PressedIndex = 322
                    };
                OKButton.Click += (o, e) => Login();

                AccountButton = new MirButton
                    {
                        HoverIndex = 324,
                        Index = 323,
                        Library = Libraries.Title,
                        Location = new Point(60, 163),
                        Parent = this,
                        PressedIndex = 325,
                    };

                PassButton = new MirButton
                    {
                        HoverIndex = 327,
                        Index = 326,
                        Library = Libraries.Title,
                        Location = new Point(166, 163),
                        Parent = this,
                        PressedIndex = 328,
                    };

                ViewKeyButton = new MirButton
                {
                    HoverIndex = 333,
                    Index = 332,
                    Library = Libraries.Title,
                    Location = new Point(60, 189),
                    Parent = this,
                    PressedIndex = 334,
                };

                CloseButton = new MirButton
                    {
                        HoverIndex = 330,
                        Index = 329,
                        Library = Libraries.Title,
                        Location = new Point(166, 189),
                        Parent = this,
                        PressedIndex = 331,
                    };
                CloseButton.Click += (o, e) => Program.Form.Close();

                AccountIDTextBox = new MirTextBox
                    {
                        Location = new Point(85, 85),
                        Parent = this,
                        Size = new Size(136, 12),
                        MaxLength = Globals.MaxAccountIDLength
                };
                AccountIDTextBox.SetFocus();
                AccountIDTextBox.TextBox.TextChanged += AccountIDTextBox_TextChanged;
                AccountIDTextBox.TextBox.KeyPress += TextBox_KeyPress;
                AccountIDTextBox.Text = Settings.AccountID;



                PasswordTextBox = new MirTextBox
                    {
                        Location = new Point(85, 108),
                        Parent = this,
                        Password = true,
                        Size = new Size(136, 15),
                        MaxLength = Globals.MaxPasswordLength
                    };

                PasswordTextBox.TextBox.TextChanged += PasswordTextBox_TextChanged;
                PasswordTextBox.TextBox.KeyPress += TextBox_KeyPress;
                PasswordTextBox.Text = Settings.Password;

            }

            private void AccountIDTextBox_TextChanged(object sender, EventArgs e)
            {
                Regex reg =
                    new Regex(@"^[A-Za-z0-9]{" + Globals.MinAccountIDLength + "," + Globals.MaxAccountIDLength + "}$");

                if (string.IsNullOrEmpty(AccountIDTextBox.Text) || !reg.IsMatch(AccountIDTextBox.TextBox.Text))
                {
                    _accountIDValid = false;
                    AccountIDTextBox.Border = !string.IsNullOrEmpty(AccountIDTextBox.Text);
                    AccountIDTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _accountIDValid = true;
                    AccountIDTextBox.Border = true;
                    AccountIDTextBox.BorderColour = Color.Green;
                }

            }
            private void PasswordTextBox_TextChanged(object sender, EventArgs e)
            {
                Regex reg =
                    new Regex(@"^[A-Za-z0-9]{" + Globals.MinPasswordLength + "," + Globals.MaxPasswordLength + "}$");

                if (string.IsNullOrEmpty(PasswordTextBox.TextBox.Text) || !reg.IsMatch(PasswordTextBox.TextBox.Text))
                {
                    _passwordValid = false;
                    PasswordTextBox.Border = !string.IsNullOrEmpty(PasswordTextBox.TextBox.Text);
                    PasswordTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _passwordValid = true;
                    PasswordTextBox.Border = true;
                    PasswordTextBox.BorderColour = Color.Green;
                }

                RefreshLoginButton();
            }
            public void TextBox_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (sender == null || e.KeyChar != (char) Keys.Enter) return;

                e.Handled = true;

                if (!_accountIDValid)
                {
                    AccountIDTextBox.SetFocus();
                    return;
                }
                if (!_passwordValid)
                {
                    PasswordTextBox.SetFocus();
                    return;
                }

                if (OKButton.Enabled)
                    OKButton.InvokeMouseClick(null);
            }
            private void RefreshLoginButton()
            {
                OKButton.Enabled = _accountIDValid && _passwordValid;
            }
            
            private void Login()
            {
                OKButton.Enabled = false;
                Network.Enqueue(new C.Login {AccountID = AccountIDTextBox.Text, Password = PasswordTextBox.Text});
            }

            public void Hide()
            {
                if (!Visible) return;
                Visible = false;
            }
            public void Show()
            {
                if (Visible) return;
                Visible = true;
                AccountIDTextBox.SetFocus();

                if (Settings.Password != string.Empty && Settings.AccountID != string.Empty)
                {
                    Login();
                }
            }
            public void Clear()
            {
                AccountIDTextBox.Text = string.Empty;
                PasswordTextBox.Text = string.Empty;
            }

            #region Disposable

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    TitleLabel = null;
                    AccountIDLabel = null;
                    PassLabel = null;
                    AccountButton = null;
                    CloseButton = null;
                    OKButton = null;
                    PassButton = null;
                    AccountIDTextBox = null;
                    PasswordTextBox = null;

                }

                base.Dispose(disposing);
            }

            #endregion
        }

        public sealed class InputKeyDialog : MirImageControl
        {
            public readonly MirButton KeyEscButton, KeyDelButton, KeyRandButton, KeyEnterButton;

            private LoginDialog _loginDialog;

            private List<MirButton> _buttons = new List<MirButton>();

            private char[] _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            private char[] _numbers = "0123456789".ToCharArray();

            public InputKeyDialog(LoginDialog loginDialog)
            {
                _loginDialog = loginDialog;

                Index = 1080;
                Library = Libraries.Prguse;
                Location = new Point((Client.Settings.ScreenWidth - Size.Width) / 2 + 285, (Client.Settings.ScreenHeight - Size.Height) / 2 + 150);
                Visible = true;

                KeyEscButton = new MirButton
                {
                    Text = "Esc",
                    HoverIndex = 301,
                    Index = 300,
                    Library = Libraries.Title,
                    Location = new Point(12, 12),
                    Parent = this,
                    PressedIndex = 302,
                    CenterText = true
                };
                KeyEscButton.Click += (o, e) => Dispose();

                KeyDelButton = new MirButton
                {
                    Text = "Delete",
                    HoverIndex = 304,
                    Index = 303,
                    Library = Libraries.Title,
                    Location = new Point(140, 76),
                    Parent = this,
                    PressedIndex = 305,
                    CenterText = true
                };
                KeyDelButton.Click += (o, e) => SecureKeyDelete();

                KeyEnterButton = new MirButton
                {
                    Text = "Enter",
                    HoverIndex = 307,
                    Index = 306,
                    Library = Libraries.Title,
                    Location = new Point(140, 236),
                    Parent = this,
                    PressedIndex = 308,
                    CenterText = true

                };
                KeyEnterButton.Click += (o, e) =>
                {
                    KeyPressEventArgs arg = new KeyPressEventArgs((char)Keys.Enter);

                    _loginDialog.TextBox_KeyPress(o, arg);
                };

                KeyRandButton = new MirButton
                {
                    Text = "Random",
                    HoverIndex = 310,
                    Index = 309,
                    Library = Libraries.Title,
                    Location = new Point(76, 236),
                    Parent = this,
                    PressedIndex = 311,
                    CenterText = true
                };
                KeyRandButton.Click += (o, e) =>
                {
                    _letters = new string(_letters.OrderBy(s => Guid.NewGuid()).ToArray()).ToCharArray();
                    _numbers = new string(_numbers.OrderBy(s => Guid.NewGuid()).ToArray()).ToCharArray();

                    UpdateKeys();
                };

                UpdateKeys();
            }

            private void DisposeKeys()
            {
                foreach(MirButton button in _buttons)
                {
                    if (button != null && !button.IsDisposed) button.Dispose();
                }
            }

            private void UpdateKeys()
            {
                DisposeKeys();

                for (int i = 0; i < _numbers.Length; i++)
                {
                    char key = _numbers[i];

                    MirButton numButton = new MirButton
                    {
                        HoverIndex = 1082,
                        Index = 1081,
                        Size = new Size(32, 30),
                        Library = Libraries.Prguse,
                        Location = new Point(12 + (i % 6 * 32), 44 + (i / 6 * 32)),
                        Parent = this,
                        PressedIndex = 1083,
                        Text = _numbers[i].ToString(),
                        CenterText = true
                    };
                    numButton.Click += (o, e) => SecureKeyPress(key);

                    _buttons.Add(numButton);
                }

                for (int i = 0; i < _letters.Length; i++)
                {
                    char key = _letters[i];

                    MirButton alphButton = new MirButton
                    {
                        HoverIndex = 1082,
                        Index = 1081,
                        Size = new Size(32, 30),
                        Library = Libraries.Prguse,
                        Location = new Point(12 + (i % 6 * 32), 108 + (i / 6 * 32)),
                        Parent = this,
                        PressedIndex = 1083,
                        Text = _letters[i].ToString(),
                        CenterText = true
                    };

                    alphButton.Click += (o, e) => SecureKeyPress(key);

                    _buttons.Add(alphButton);
                }
            }

            private void SecureKeyPress(char chr)
            {
                MirTextBox currentTextBox = GetFocussedTextBox();

                string keyToAdd = chr.ToString();

                if (CMain.IsKeyLocked(Keys.CapsLock)) 
                    keyToAdd = keyToAdd.ToUpper(); 
                else 
                    keyToAdd = keyToAdd.ToLower();

                currentTextBox.Text += keyToAdd;
                currentTextBox.TextBox.SelectionLength = 0;
                currentTextBox.TextBox.SelectionStart = currentTextBox.Text.Length;
            }

            private void SecureKeyDelete()
            {
                MirTextBox currentTextBox = GetFocussedTextBox();

                if (currentTextBox.TextBox.SelectionLength > 0)
                {
                    currentTextBox.Text = currentTextBox.Text.Remove(currentTextBox.TextBox.SelectionStart, currentTextBox.TextBox.SelectionLength);
                }
                else if (currentTextBox.Text.Length > 0)
                {
                    currentTextBox.Text = currentTextBox.Text.Remove(currentTextBox.Text.Length - 1);
                }

                currentTextBox.TextBox.SelectionStart = currentTextBox.Text.Length;
            }

            private MirTextBox GetFocussedTextBox()
            {
                if (_loginDialog.AccountIDTextBox.TextBox.Focused)
                    return _loginDialog.AccountIDTextBox;
                else
                    return _loginDialog.PasswordTextBox;
            }

            #region Disposable
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                DisposeKeys();
            }
            #endregion
        }

        public sealed class NewAccountDialog : MirImageControl
        {
            public MirButton OKButton, CancelButton;

            public MirTextBox AccountIDTextBox,
                              Password1TextBox,
                              Password2TextBox,
                              EMailTextBox,
                              UserNameTextBox,
                              BirthDateTextBox,
                              QuestionTextBox,
                              AnswerTextBox;

            public MirLabel Description;

            private bool _accountIDValid,
                         _password1Valid,
                         _password2Valid,
                         _eMailValid = true,
                         _userNameValid = true,
                         _birthDateValid = true,
                         _questionValid = true,
                         _answerValid = true;


            public NewAccountDialog()
            {
                Index = 63;
                Library = Libraries.Prguse;
                Size = new Size();
                Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);

                CancelButton = new MirButton
                {
                    HoverIndex = 204,
                    Index = 203,
                    Library = Libraries.Title,
                    Location = new Point(409, 425),
                    Parent = this,
                    PressedIndex = 205
                };
                CancelButton.Click += (o, e) => Dispose();

                OKButton = new MirButton
                {
                    Enabled = false,
                    HoverIndex = 201,
                    Index = 200,
                    Library = Libraries.Title,
                    Location = new Point(135, 425),
                    Parent = this,
                    PressedIndex = 202,
                };
                OKButton.Click += (o, e) => CreateAccount();


                AccountIDTextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(226, 103),
                    MaxLength = Globals.MaxAccountIDLength,
                    Parent = this,
                    Size = new Size(136, 18),
                };
                AccountIDTextBox.SetFocus();
                AccountIDTextBox.TextBox.MaxLength = Globals.MaxAccountIDLength;
                AccountIDTextBox.TextBox.TextChanged += AccountIDTextBox_TextChanged;
                AccountIDTextBox.TextBox.GotFocus += AccountIDTextBox_GotFocus;

                Password1TextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(226, 129),
                    MaxLength = Globals.MaxPasswordLength,
                    Parent = this,
                    Password = true,
                    Size = new Size(136, 18),
                    TextBox = { MaxLength = Globals.MaxPasswordLength },
                };
                Password1TextBox.TextBox.TextChanged += Password1TextBox_TextChanged;
                Password1TextBox.TextBox.GotFocus += PasswordTextBox_GotFocus;

                Password2TextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(226, 155),
                    MaxLength = Globals.MaxPasswordLength,
                    Parent = this,
                    Password = true,
                    Size = new Size(136, 18),
                    TextBox = { MaxLength = Globals.MaxPasswordLength },
                };
                Password2TextBox.TextBox.TextChanged += Password2TextBox_TextChanged;
                Password2TextBox.TextBox.GotFocus += PasswordTextBox_GotFocus;

                UserNameTextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(226, 189),
                    MaxLength = 20,
                    Parent = this,
                    Size = new Size(136, 18),
                    TextBox = { MaxLength = 20 },
                };
                UserNameTextBox.TextBox.TextChanged += UserNameTextBox_TextChanged;
                UserNameTextBox.TextBox.GotFocus += UserNameTextBox_GotFocus;


                BirthDateTextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(226, 215),
                    MaxLength = 10,
                    Parent = this,
                    Size = new Size(136, 18),
                    TextBox = { MaxLength = 10 },
                };
                BirthDateTextBox.TextBox.TextChanged += BirthDateTextBox_TextChanged;
                BirthDateTextBox.TextBox.GotFocus += BirthDateTextBox_GotFocus;

                QuestionTextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(226, 250),
                    MaxLength = 30,
                    Parent = this,
                    Size = new Size(190, 18),
                    TextBox = { MaxLength = 30 },
                };
                QuestionTextBox.TextBox.TextChanged += QuestionTextBox_TextChanged;
                QuestionTextBox.TextBox.GotFocus += QuestionTextBox_GotFocus;

                AnswerTextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(226, 276),
                    MaxLength = 30,
                    Parent = this,
                    Size = new Size(190, 18),
                    TextBox = { MaxLength = 30 },
                };
                AnswerTextBox.TextBox.TextChanged += AnswerTextBox_TextChanged;
                AnswerTextBox.TextBox.GotFocus += AnswerTextBox_GotFocus;

                EMailTextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(226, 311),
                    MaxLength = 50,
                    Parent = this,
                    Size = new Size(136, 18),
                    TextBox = { MaxLength = 50 },
                };
                EMailTextBox.TextBox.TextChanged += EMailTextBox_TextChanged;
                EMailTextBox.TextBox.GotFocus += EMailTextBox_GotFocus;


                Description = new MirLabel
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(15, 340),
                    Parent = this,
                    Size = new Size(300, 70),
                    Visible = false
                };
                
            }


            private void AccountIDTextBox_TextChanged(object sender, EventArgs e)
            {
                Regex reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinAccountIDLength + "," + Globals.MaxAccountIDLength + "}$");

                if (string.IsNullOrEmpty(AccountIDTextBox.Text) || !reg.IsMatch(AccountIDTextBox.Text))
                {
                    _accountIDValid = false;
                    AccountIDTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _accountIDValid = true;
                    AccountIDTextBox.BorderColour = Color.Green;
                }
                RefreshConfirmButton();
            }
            private void Password1TextBox_TextChanged(object sender, EventArgs e)
            {
                Regex reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinPasswordLength + "," + Globals.MaxPasswordLength + "}$");

                if (string.IsNullOrEmpty(Password1TextBox.Text) || !reg.IsMatch(Password1TextBox.Text))
                {
                    _password1Valid = false;
                    Password1TextBox.BorderColour = Color.Red;
                }
                else
                {
                    _password1Valid = true;
                    Password1TextBox.BorderColour = Color.Green;
                }
                Password2TextBox_TextChanged(sender, e);
            }
            private void Password2TextBox_TextChanged(object sender, EventArgs e)
            {
                Regex reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinPasswordLength + "," + Globals.MaxPasswordLength + "}$");

                if (string.IsNullOrEmpty(Password2TextBox.Text) || !reg.IsMatch(Password2TextBox.Text) ||
                    Password1TextBox.Text != Password2TextBox.Text)
                {
                    _password2Valid = false;
                    Password2TextBox.BorderColour = Color.Red;
                }
                else
                {
                    _password2Valid = true;
                    Password2TextBox.BorderColour = Color.Green;
                }
                RefreshConfirmButton();
            }
            private void EMailTextBox_TextChanged(object sender, EventArgs e)
            {
                Regex reg = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                if (string.IsNullOrEmpty(EMailTextBox.Text))
                {
                    _eMailValid = true;
                    EMailTextBox.BorderColour = Color.Gray;
                }
                else if (!reg.IsMatch(EMailTextBox.Text) || EMailTextBox.Text.Length > 50)
                {
                    _eMailValid = false;
                    EMailTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _eMailValid = true;
                    EMailTextBox.BorderColour = Color.Green;
                }
                RefreshConfirmButton();
            }
            private void UserNameTextBox_TextChanged(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(UserNameTextBox.Text))
                {
                    _userNameValid = true;
                    UserNameTextBox.BorderColour = Color.Gray;
                }
                else if (UserNameTextBox.Text.Length > 20)
                {
                    _userNameValid = false;
                    UserNameTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _userNameValid = true;
                    UserNameTextBox.BorderColour = Color.Green;
                }
                RefreshConfirmButton();
            }
            private void BirthDateTextBox_TextChanged(object sender, EventArgs e)
            {
                DateTime dateTime;
                if (string.IsNullOrEmpty(BirthDateTextBox.Text))
                {
                    _birthDateValid = true;
                    BirthDateTextBox.BorderColour = Color.Gray;
                }
                else if (!DateTime.TryParse(BirthDateTextBox.Text, out dateTime) || BirthDateTextBox.Text.Length > 10)
                {
                    _birthDateValid = false;
                    BirthDateTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _birthDateValid = true;
                    BirthDateTextBox.BorderColour = Color.Green;
                }
                RefreshConfirmButton();
            }
            private void QuestionTextBox_TextChanged(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(QuestionTextBox.Text))
                {
                    _questionValid = true;
                    QuestionTextBox.BorderColour = Color.Gray;
                }
                else if (QuestionTextBox.Text.Length > 30)
                {
                    _questionValid = false;
                    QuestionTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _questionValid = true;
                    QuestionTextBox.BorderColour = Color.Green;
                }
                RefreshConfirmButton();
            }
            private void AnswerTextBox_TextChanged(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(AnswerTextBox.Text))
                {
                    _answerValid = true;
                    AnswerTextBox.BorderColour = Color.Gray;
                }
                else if (AnswerTextBox.Text.Length > 30)
                {
                    _answerValid = false;
                    AnswerTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _answerValid = true;
                    AnswerTextBox.BorderColour = Color.Green;
                }
                RefreshConfirmButton();
            }

            private void AccountIDTextBox_GotFocus(object sender, EventArgs e)
            {
                Description.Visible = true;
                Description.Text = " Description: Account ID.\n Accepted characters: a-z A-Z 0-9.\n Length: between " +
                                   Globals.MinAccountIDLength + " and " + Globals.MaxAccountIDLength + " characters.";
            }
            private void PasswordTextBox_GotFocus(object sender, EventArgs e)
            {
                Description.Visible = true;
                Description.Text = " Description: Password.\n Accepted characters: a-z A-Z 0-9.\n Length: between " +
                                   Globals.MinPasswordLength + " and " + Globals.MaxPasswordLength + " characters.";
            }
            private void EMailTextBox_GotFocus(object sender, EventArgs e)
            {
                Description.Visible = true;
                Description.Text =
                    " Description: E-Mail Address.\n Format: Example@Example.Com.\n Max Length: 50 characters.\n Optional Field.";
            }
            private void UserNameTextBox_GotFocus(object sender, EventArgs e)
            {
                Description.Visible = true;
                Description.Text =
                    " Description: User Name.\n Accepted characters:All.\n Length: between 0 and 20 characters.\n Optional Field.";
            }
            private void BirthDateTextBox_GotFocus(object sender, EventArgs e)
            {
                Description.Visible = true;
                Description.Text =
                    string.Format(" Description: Birth Date.\n Format: {0}.\n Length: 10 characters.\n Optional Field.",
                                  Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper());
            }
            private void QuestionTextBox_GotFocus(object sender, EventArgs e)
            {
                Description.Visible = true;
                Description.Text =
                    " Description: Secret Question.\n Accepted characters: All.\n Length: between 0 and 30 characters.\n Optional Field.";
            }
            private void AnswerTextBox_GotFocus(object sender, EventArgs e)
            {
                Description.Visible = true;
                Description.Text =
                    " Description: Secret Answer.\n Accepted characters: All.\n Length: between 0 and 30 characters.\n Optional Field.";
            }

            private void RefreshConfirmButton()
            {
                OKButton.Enabled = _accountIDValid && _password1Valid && _password2Valid && _eMailValid &&
                                        _userNameValid && _birthDateValid && _questionValid && _answerValid;
            }
            private void CreateAccount()
            {
                OKButton.Enabled = false;

                Network.Enqueue(new C.NewAccount
                    {
                        AccountID = AccountIDTextBox.Text,
                        Password = Password1TextBox.Text,
                        EMailAddress = EMailTextBox.Text,
                        BirthDate = !string.IsNullOrEmpty(BirthDateTextBox.Text)
                                        ? DateTime.Parse(BirthDateTextBox.Text)
                                        : DateTime.MinValue,
                        UserName = UserNameTextBox.Text,
                        SecretQuestion = QuestionTextBox.Text,
                        SecretAnswer = AnswerTextBox.Text,
                    });
            }
            
            public void Show()
            {
                if (Visible) return;
                Visible = true;
                AccountIDTextBox.SetFocus();
            }

            #region Disposable
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    OKButton = null;
                    CancelButton = null;

                    AccountIDTextBox = null;
                    Password1TextBox = null;
                    Password2TextBox = null;
                    EMailTextBox = null;
                    UserNameTextBox = null;
                    BirthDateTextBox = null;
                    QuestionTextBox = null;
                    AnswerTextBox = null;

                    Description = null;

                }

                base.Dispose(disposing);
            }
            #endregion
        }

        public sealed class ChangePasswordDialog : MirImageControl
        {
            public readonly MirButton OKButton,
                                      CancelButton;

            public readonly MirTextBox AccountIDTextBox,
                                       CurrentPasswordTextBox,
                                       NewPassword1TextBox,
                                       NewPassword2TextBox;

            private bool _accountIDValid,
                         _currentPasswordValid,
                         _newPassword1Valid,
                         _newPassword2Valid;
            
            public ChangePasswordDialog()
            {
                Index = 50;
                Library = Libraries.Prguse;
                Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);

                CancelButton = new MirButton
                {
                    HoverIndex = 111,
                    Index = 110,
                    Library = Libraries.Title,
                    Location = new Point(222, 236),
                    Parent = this,
                    PressedIndex = 112
                };
                CancelButton.Click += (o, e) => Dispose();

                OKButton = new MirButton
                {
                    Enabled = false,
                    HoverIndex = 108,
                    Index = 107,
                    Library = Libraries.Title,
                    Location = new Point(80, 236),
                    Parent = this,
                    PressedIndex = 109,
                };
                OKButton.Click += (o, e) => ChangePassword();


                AccountIDTextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(178, 75),
                    MaxLength = Globals.MaxAccountIDLength,
                    Parent = this,
                    Size = new Size(136, 18),
                };
                AccountIDTextBox.SetFocus();
                AccountIDTextBox.TextBox.MaxLength = Globals.MaxAccountIDLength;
                AccountIDTextBox.TextBox.TextChanged += AccountIDTextBox_TextChanged;

                CurrentPasswordTextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(178, 113),
                    MaxLength = Globals.MaxPasswordLength,
                    Parent = this,
                    Password = true,
                    Size = new Size(136, 18),
                    TextBox = { MaxLength = Globals.MaxPasswordLength },
                };
                CurrentPasswordTextBox.TextBox.TextChanged += CurrentPasswordTextBox_TextChanged;

                NewPassword1TextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(178, 151),
                    MaxLength = Globals.MaxPasswordLength,
                    Parent = this,
                    Password = true,
                    Size = new Size(136, 18),
                    TextBox = { MaxLength = Globals.MaxPasswordLength },
                };
                NewPassword1TextBox.TextBox.TextChanged += NewPassword1TextBox_TextChanged;

                NewPassword2TextBox = new MirTextBox
                {
                    Border = true,
                    BorderColour = Color.Gray,
                    Location = new Point(178, 188),
                    MaxLength = Globals.MaxPasswordLength,
                    Parent = this,
                    Password = true,
                    Size = new Size(136, 18),
                    TextBox = { MaxLength = Globals.MaxPasswordLength },
                };
                NewPassword2TextBox.TextBox.TextChanged += NewPassword2TextBox_TextChanged;

            }

            void RefreshConfirmButton()
            {
                OKButton.Enabled = _accountIDValid && _currentPasswordValid && _newPassword1Valid && _newPassword2Valid;
            }

            private void AccountIDTextBox_TextChanged(object sender, EventArgs e)
            {
                Regex reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinAccountIDLength + "," + Globals.MaxAccountIDLength + "}$");

                if (string.IsNullOrEmpty(AccountIDTextBox.Text) || !reg.IsMatch(AccountIDTextBox.Text))
                {
                    _accountIDValid = false;
                    AccountIDTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _accountIDValid = true;
                    AccountIDTextBox.BorderColour = Color.Green;
                }
                RefreshConfirmButton();
            }
            private void CurrentPasswordTextBox_TextChanged(object sender, EventArgs e)
            {
              Regex reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinPasswordLength + "," + Globals.MaxPasswordLength + "}$");

                if (string.IsNullOrEmpty(CurrentPasswordTextBox.Text) || !reg.IsMatch(CurrentPasswordTextBox.Text))
                {
                    _currentPasswordValid = false;
                    CurrentPasswordTextBox.BorderColour = Color.Red;
                }
                else
                {
                    _currentPasswordValid = true;
                    CurrentPasswordTextBox.BorderColour = Color.Green;
                }
                RefreshConfirmButton();
            }
            private void NewPassword1TextBox_TextChanged(object sender, EventArgs e)
            {
                Regex reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinPasswordLength + "," + Globals.MaxPasswordLength + "}$");

                if (string.IsNullOrEmpty(NewPassword1TextBox.Text) || !reg.IsMatch(NewPassword1TextBox.Text))
                {
                    _newPassword1Valid = false;
                    NewPassword1TextBox.BorderColour = Color.Red;
                }
                else
                {
                    _newPassword1Valid = true;
                    NewPassword1TextBox.BorderColour = Color.Green;
                }
                NewPassword2TextBox_TextChanged(sender, e);
            }
            private void NewPassword2TextBox_TextChanged(object sender, EventArgs e)
            {
                if (NewPassword1TextBox.Text == NewPassword2TextBox.Text)
                {
                    _newPassword2Valid = _newPassword1Valid;
                    NewPassword2TextBox.BorderColour = NewPassword1TextBox.BorderColour;
                }
                else
                {
                    _newPassword2Valid = false;
                    NewPassword2TextBox.BorderColour = Color.Red;
                }
                RefreshConfirmButton();
            }

            private void ChangePassword()
            {
                OKButton.Enabled = false;

                Network.Enqueue(new C.ChangePassword
                    {
                        AccountID = AccountIDTextBox.Text,
                        CurrentPassword = CurrentPasswordTextBox.Text,
                        NewPassword = NewPassword1TextBox.Text
                    });
            }

            public void Show()
            {
                if (Visible) return;
                Visible = true;
                AccountIDTextBox.SetFocus();
            }
        }

        #region Disposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _background = null;
                Version = null;

                _login = null;
                _account = null;
                _password = null;

                _connectBox = null;
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}