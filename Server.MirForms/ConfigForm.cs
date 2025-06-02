using Server.MirEnvir;
using System.Net;
using System.Text.RegularExpressions;

namespace Server
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();

            VPathTextBox.Text = Settings.VersionPath;
            VersionCheckBox.Checked = Settings.CheckVersion;
            RelogDelayTextBox.Text = Settings.RelogDelay.ToString();

            IPAddressTextBox.Text = Settings.IPAddress;
            PortTextBox.Text = Settings.Port.ToString();
            TimeOutTextBox.Text = Settings.TimeOut.ToString();
            MaxUserTextBox.Text = Settings.MaxUser.ToString();

            StartHTTPCheckBox.Checked = Settings.StartHTTPService;
            HTTPIPAddressTextBox.Text = Settings.HTTPIPAddress;
            HTTPTrustedIPAddressTextBox.Text = Settings.HTTPTrustedIPAddress;

            AccountCheckBox.Checked = Settings.AllowNewAccount;
            PasswordCheckBox.Checked = Settings.AllowChangePassword;
            LoginCheckBox.Checked = Settings.AllowLogin;
            NCharacterCheckBox.Checked = Settings.AllowNewCharacter;
            DCharacterCheckBox.Checked = Settings.AllowDeleteCharacter;
            StartGameCheckBox.Checked = Settings.AllowStartGame;
            AllowAssassinCheckBox.Checked = Settings.AllowCreateAssassin;
            AllowArcherCheckBox.Checked = Settings.AllowCreateArcher;
            Resolution_textbox.Text = Settings.AllowedResolution.ToString();
            ObserveCheckBox.Checked = Settings.AllowObserve;

            SafeZoneBorderCheckBox.Checked = Settings.SafeZoneBorder;
            SafeZoneHealingCheckBox.Checked = Settings.SafeZoneHealing;
            gameMasterEffect_CheckBox.Checked = Settings.GameMasterEffect;
            lineMessageTimeTextBox.Text = Settings.LineMessageTimer.ToString();

            SaveDelayTextBox.Text = Settings.SaveDelay.ToString();

            ServerVersionLabel.Text = Application.ProductVersion;
            DBVersionLabel.Text = MirEnvir.Envir.LoadVersion.ToString() + ((MirEnvir.Envir.LoadVersion < MirEnvir.Envir.Version) ? " (Update needed)" : "");
            maxConnectionsPerIP.Text = Settings.MaxIP.ToString();
            expRateInput.Value = Math.Round((decimal)Settings.ExpRate, 2);
            dropRateInput.Value = Math.Round((decimal)Settings.DropRate, 2);
            tbRestedPeriod.Text = Settings.RestedPeriod.ToString();
            tbRestedBuffLength.Text = Settings.RestedBuffLength.ToString();
            tbRestedExpBonus.Text = Settings.RestedExpBonus.ToString();
            tbMaxRestedBonus.Text = Settings.RestedMaxBonus.ToString();
        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Save();
            Settings.LoadVersion();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        public void Save()
        {
            Settings.VersionPath = VPathTextBox.Text;
            Settings.CheckVersion = VersionCheckBox.Checked;

            IPAddress tempIP;
            if (IPAddress.TryParse(IPAddressTextBox.Text, out tempIP))
                Settings.IPAddress = tempIP.ToString();

            Settings.StartHTTPService = StartHTTPCheckBox.Checked;
            if (tryParseHttp())
                Settings.HTTPIPAddress = HTTPIPAddressTextBox.Text.ToString();

            if (tryParseTrustedHttp())
                Settings.HTTPTrustedIPAddress = HTTPTrustedIPAddressTextBox.Text.ToString();

            ushort tempshort;
            int tempint;

            if (ushort.TryParse(PortTextBox.Text, out tempshort))
                Settings.Port = tempshort;

            if (ushort.TryParse(TimeOutTextBox.Text, out tempshort))
                Settings.TimeOut = tempshort;

            if (ushort.TryParse(MaxUserTextBox.Text, out tempshort))
                Settings.MaxUser = tempshort;

            if (ushort.TryParse(RelogDelayTextBox.Text, out tempshort))
                Settings.RelogDelay = tempshort;

            if (ushort.TryParse(SaveDelayTextBox.Text, out tempshort))
                Settings.SaveDelay = tempshort;

            Settings.AllowNewAccount = AccountCheckBox.Checked;
            Settings.AllowChangePassword = PasswordCheckBox.Checked;
            Settings.AllowLogin = LoginCheckBox.Checked;
            Settings.AllowNewCharacter = NCharacterCheckBox.Checked;
            Settings.AllowDeleteCharacter = DCharacterCheckBox.Checked;
            Settings.AllowStartGame = StartGameCheckBox.Checked;
            Settings.AllowCreateAssassin = AllowAssassinCheckBox.Checked;
            Settings.AllowCreateArcher = AllowArcherCheckBox.Checked;
            Settings.AllowObserve = ObserveCheckBox.Checked;

            if (int.TryParse(Resolution_textbox.Text, out tempint))
                Settings.AllowedResolution = tempint;

            Settings.SafeZoneBorder = SafeZoneBorderCheckBox.Checked;
            Settings.SafeZoneHealing = SafeZoneHealingCheckBox.Checked;
            Settings.GameMasterEffect = gameMasterEffect_CheckBox.Checked;
            if (int.TryParse(lineMessageTimeTextBox.Text, out tempint))
                Settings.LineMessageTimer = tempint;
            if (ushort.TryParse(maxConnectionsPerIP.Text, out tempshort))
                Settings.MaxIP = tempshort;
            Settings.ExpRate = (float)expRateInput.Value;
            Settings.DropRate = (float)dropRateInput.Value;
            Settings.RestedPeriod = Convert.ToInt32(tbRestedPeriod.Text);
            Settings.RestedBuffLength = Convert.ToInt32(tbRestedBuffLength.Text);
            Settings.RestedExpBonus = Convert.ToInt32(tbRestedExpBonus.Text);
            Settings.RestedMaxBonus = Convert.ToInt32(tbMaxRestedBonus.Text);
        }

        private void IPAddressCheck(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            IPAddress temp;

            ActiveControl.BackColor = !IPAddress.TryParse(ActiveControl.Text, out temp) ? Color.Red : SystemColors.Window;
        }

        private void CheckUShort(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            ActiveControl.BackColor = !ushort.TryParse(ActiveControl.Text, out temp) ? Color.Red : SystemColors.Window;
        }

        private void VPathBrowseButton_Click(object sender, EventArgs e)
        {
            if (VPathDialog.ShowDialog() == DialogResult.OK)
            {
                VPathTextBox.Text = string.Join(",", VPathDialog.FileNames);
            }
        }

        private void Resolution_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            ActiveControl.BackColor = !int.TryParse(ActiveControl.Text, out temp) ? Color.Red : SystemColors.Window;

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void SafeZoneBorderCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SafeZoneHealingCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void HTTPIPAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ActiveControl.BackColor = !tryParseHttp() ? Color.Red : SystemColors.Window;
        }


        private void HTTPTrustedIPAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ActiveControl.BackColor = !tryParseTrustedHttp() ? Color.Red : SystemColors.Window;
        }

        bool tryParseHttp()
        {
            if ((HTTPIPAddressTextBox.Text.StartsWith("http://") || HTTPIPAddressTextBox.Text.StartsWith("https://")) && HTTPIPAddressTextBox.Text.EndsWith("/"))
            {
                return true;
            }
            return false;
        }

        bool tryParseTrustedHttp()
        {
            string pattern = @"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}";
            return Regex.IsMatch(HTTPTrustedIPAddressTextBox.Text, pattern);
        }

        private void StartHTTPCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.StartHTTPService = StartHTTPCheckBox.Checked;
        }

        private void expRateInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
                e.Handled = true;
        }

        private void dropRateInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
                e.Handled = true;
        }

        private void expRateInput_ValueChanged(object sender, EventArgs e)
        {
            expRateInput.Value = Math.Round(expRateInput.Value, 2);
        }

        private void dropRateInput_ValueChanged(object sender, EventArgs e)
        {
            dropRateInput.Value = Math.Round(dropRateInput.Value, 2);
        }

        private void tbRestedPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void tbRestedBuffLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void tbRestedExpBonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void tbMaxRestedBonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        #region Drop Adjuster
        private Envir Envir => SMain.EditEnvir;
        private void ProcessFiles(RequiredClass targetClass, bool comment)
        {
            string dropPath = Path.Combine(Application.StartupPath, "Envir", "Drops");

            if (!Directory.Exists(dropPath))
            {
                MessageBox.Show("Drops directory not found!");
                return;
            }

            try
            {
                var itemLookup = Envir.ItemInfoList.ToLookup(
                    i => i.Name.Trim(),
                    StringComparer.OrdinalIgnoreCase
                );

                int totalModified = 0;

                foreach (string filePath in Directory.GetFiles(dropPath, "*.txt", SearchOption.AllDirectories))
                {
                    var lines = File.ReadAllLines(filePath);
                    bool modified = false;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string originalLine = lines[i].Trim();
                        if (string.IsNullOrWhiteSpace(originalLine)) continue;

                        bool isCommented = originalLine.StartsWith(";");
                        string workingLine = isCommented ? originalLine.Substring(1).TrimStart() : originalLine;

                        string itemName = workingLine.Split()
                            .Select(part => part.Trim())
                            .FirstOrDefault(part => itemLookup.Contains(part));

                        if (string.IsNullOrEmpty(itemName)) continue;

                        foreach (ItemInfo item in itemLookup[itemName])
                        {
                            if (item.RequiredClass == RequiredClass.None) continue;
                            if (!item.RequiredClass.HasFlag(targetClass)) continue;

                            if (comment && !isCommented)
                            {
                                lines[i] = ";" + lines[i];
                                modified = true;
                                totalModified++;
                            }
                            else if (!comment && isCommented)
                            {
                                lines[i] = workingLine;
                                modified = true;
                                totalModified++;
                            }
                            break;
                        }
                    }

                    if (modified)
                    {
                        File.WriteAllLines(filePath, lines);
                    }
                }

                MessageBox.Show($"Processed files. Modified {totalModified} entries.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void RemoveSinDrops_Click(object sender, EventArgs e)
        {
            ProcessFiles(RequiredClass.Assassin, true);
        }

        private void ReaddSinDrops_Click(object sender, EventArgs e)
        {
            ProcessFiles(RequiredClass.Assassin, false);
        }

        private void RemoveArcDrops_Click(object sender, EventArgs e)
        {
            ProcessFiles(RequiredClass.Archer, true);
        }

        private void ReaddArcDrops_Click(object sender, EventArgs e)
        {
            ProcessFiles(RequiredClass.Archer, false);
        }
        #endregion
    }
}
