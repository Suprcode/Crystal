using System.Text.RegularExpressions;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;

namespace Client.MirScenes.Dialogs
{
    public sealed class NewCharacterDialog : MirImageControl
    {
        private static readonly Regex Reg = new Regex(@"^[A-Za-z0-9]|[\u4e00-\u9fa5]{" + Globals.MinCharacterNameLength + "," + Globals.MaxCharacterNameLength + "}$");

        public MirImageControl TitleLabel;
        public MirAnimatedControl CharacterDisplay;

        public MirButton OKButton,
            CancelButton,
            WarriorButton,
            WizardButton,
            TaoistButton,
            AssassinButton,
            ArcherButton,
            MaleButton,
            FemaleButton;

        public MirTextBox NameTextBox;

        public MirLabel Description;

        public MirClass Class;
        public MirGender Gender;

        #region Descriptions

        public readonly string WarriorDescription = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.WarriorsDescription);

        public readonly string WizardDescription = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.WizardsDescription);

        public readonly string TaoistDescription = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.TaoistsDescription);

        public readonly string AssassinDescription = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AssassinsDescription);

        public readonly string ArcherDescription = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ArchersDescription);

        #endregion

        public NewCharacterDialog()
        {
            Index = 73;
            Library = Libraries.Prguse;
            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);
            Modal = true;

            TitleLabel = new MirImageControl
            {
                Index = 20,
                Library = Libraries.Title,
                Location = new Point(206, 11),
                Parent = this,
            };

            CancelButton = new MirButton
            {
                HoverIndex = 281,
                Index = 280,
                Library = Libraries.Title,
                Location = new Point(425, 425),
                Parent = this,
                PressedIndex = 282
            };
            CancelButton.Click += (o, e) => Hide();

            OKButton = new MirButton
            {
                Enabled = false,
                HoverIndex = 361,
                Index = 360,
                Library = Libraries.Title,
                Location = new Point(160, 425),
                Parent = this,
                PressedIndex = 362,
            };
            OKButton.Click += (o, e) => CreateCharacter();

            NameTextBox = new MirTextBox
            {
                Location = new Point(325, 268),
                Parent = this,
                Size = new Size(240, 20),
                MaxLength = Globals.MaxCharacterNameLength
            };
            NameTextBox.TextBox.KeyPress += TextBox_KeyPress;
            NameTextBox.TextBox.TextChanged += CharacterNameTextBox_TextChanged;
            NameTextBox.SetFocus();

            CharacterDisplay = new MirAnimatedControl
            {
                Animated = true,
                AnimationCount = 16,
                AnimationDelay = 250,
                Index = 20,
                Library = Libraries.ChrSel,
                Location = new Point(120, 250),
                Parent = this,
                UseOffSet = true,
            };
            CharacterDisplay.AfterDraw += (o, e) =>
            {
                if (Class == MirClass.Wizard)
                    Libraries.ChrSel.DrawBlend(CharacterDisplay.Index + 560, CharacterDisplay.DisplayLocationWithoutOffSet, Color.White, true);
            };


            WarriorButton = new MirButton
            {
                HoverIndex = 2427,
                Index = 2427,
                Library = Libraries.Prguse,
                Location = new Point(323, 296),
                Parent = this,
                PressedIndex = 2428,
                Sound = SoundList.ButtonA,
            };
            WarriorButton.Click += (o, e) =>
            {
                Class = MirClass.Warrior;
                UpdateInterface();
            };


            WizardButton = new MirButton
            {
                HoverIndex = 2430,
                Index = 2429,
                Library = Libraries.Prguse,
                Location = new Point(373, 296),
                Parent = this,
                PressedIndex = 2431,
                Sound = SoundList.ButtonA,
            };
            WizardButton.Click += (o, e) =>
            {
                Class = MirClass.Wizard;
                UpdateInterface();
            };


            TaoistButton = new MirButton
            {
                HoverIndex = 2433,
                Index = 2432,
                Library = Libraries.Prguse,
                Location = new Point(423, 296),
                Parent = this,
                PressedIndex = 2434,
                Sound = SoundList.ButtonA,
            };
            TaoistButton.Click += (o, e) =>
            {
                Class = MirClass.Taoist;
                UpdateInterface();
            };

            AssassinButton = new MirButton
            {
                HoverIndex = 2436,
                Index = 2435,
                Library = Libraries.Prguse,
                Location = new Point(473, 296),
                Parent = this,
                PressedIndex = 2437,
                Sound = SoundList.ButtonA,
            };
            AssassinButton.Click += (o, e) =>
            {
                Class = MirClass.Assassin;
                UpdateInterface();
            };

            ArcherButton = new MirButton
            {
                HoverIndex = 2439,
                Index = 2438,
                Library = Libraries.Prguse,
                Location = new Point(523, 296),
                Parent = this,
                PressedIndex = 2440,
                Sound = SoundList.ButtonA,
            };
            ArcherButton.Click += (o, e) =>
            {
                Class = MirClass.Archer;
                UpdateInterface();
            };


            MaleButton = new MirButton
            {
                HoverIndex = 2421,
                Index = 2421,
                Library = Libraries.Prguse,
                Location = new Point(323, 343),
                Parent = this,
                PressedIndex = 2422,
                Sound = SoundList.ButtonA,
            };
            MaleButton.Click += (o, e) =>
            {
                Gender = MirGender.Male;
                UpdateInterface();
            };

            FemaleButton = new MirButton
            {
                HoverIndex = 2424,
                Index = 2423,
                Library = Libraries.Prguse,
                Location = new Point(373, 343),
                Parent = this,
                PressedIndex = 2425,
                Sound = SoundList.ButtonA,
            };
            FemaleButton.Click += (o, e) =>
            {
                Gender = MirGender.Female;
                UpdateInterface();
            };

            Description = new MirLabel
            {
                Border = true,
                Location = new Point(279, 70),
                Parent = this,
                Size = new Size(278, 170),
                Text = WarriorDescription,
            };
        }

        public override void Show()
        {
            base.Show();

            Class = MirClass.Warrior;
            Gender = MirGender.Male;
            NameTextBox.Text = string.Empty;

            UpdateInterface();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == null) return;
            if (e.KeyChar != (char)Keys.Enter) return;
            e.Handled = true;

            if (OKButton.Enabled)
                OKButton.InvokeMouseClick(null);
        }

        private void CharacterNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                OKButton.Enabled = false;
                NameTextBox.Border = false;
            }
            else if (!Reg.IsMatch(NameTextBox.Text))
            {
                OKButton.Enabled = false;
                NameTextBox.Border = true;
                NameTextBox.BorderColour = Color.Red;
            }
            else
            {
                OKButton.Enabled = true;
                NameTextBox.Border = true;
                NameTextBox.BorderColour = Color.Green;
            }
        }

        public event EventHandler OnCreateCharacter;

        private void CreateCharacter()
        {
            OKButton.Enabled = false;

            if (OnCreateCharacter != null)
                OnCreateCharacter.Invoke(this, EventArgs.Empty);
        }

        private void UpdateInterface()
        {
            MaleButton.Index = 2420;
            FemaleButton.Index = 2423;

            WarriorButton.Index = 2426;
            WizardButton.Index = 2429;
            TaoistButton.Index = 2432;
            AssassinButton.Index = 2435;
            ArcherButton.Index = 2438;

            switch (Gender)
            {
                case MirGender.Male:
                    MaleButton.Index = 2421;
                    break;
                case MirGender.Female:
                    FemaleButton.Index = 2424;
                    break;
            }

            switch (Class)
            {
                case MirClass.Warrior:
                    WarriorButton.Index = 2427;
                    Description.Text = WarriorDescription;
                    CharacterDisplay.Index = (byte)Gender == 0 ? 20 : 300; //220 : 500;
                    break;
                case MirClass.Wizard:
                    WizardButton.Index = 2430;
                    Description.Text = WizardDescription;
                    CharacterDisplay.Index = (byte)Gender == 0 ? 40 : 320; //240 : 520;
                    break;
                case MirClass.Taoist:
                    TaoistButton.Index = 2433;
                    Description.Text = TaoistDescription;
                    CharacterDisplay.Index = (byte)Gender == 0 ? 60 : 340; //260 : 540;
                    break;
                case MirClass.Assassin:
                    AssassinButton.Index = 2436;
                    Description.Text = AssassinDescription;
                    CharacterDisplay.Index = (byte)Gender == 0 ? 80 : 360; //280 : 560;
                    break;
                case MirClass.Archer:
                    ArcherButton.Index = 2439;
                    Description.Text = ArcherDescription;
                    CharacterDisplay.Index = (byte)Gender == 0 ? 100 : 140; //160 : 180;
                    break;
            }

            //CharacterDisplay.Index = ((byte)_class + 1) * 20 + (byte)_gender * 280;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            OnCreateCharacter = null;
        }
    }
}