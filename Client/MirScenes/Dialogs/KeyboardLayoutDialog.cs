using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;

namespace Client.MirScenes.Dialogs
{
    public sealed class KeyboardLayoutDialog : MirImageControl
    {
        public MirImageControl TitleLabel, EnforceButtonChecked;
        public MirLabel PageLabel, EnforceButtonLabel;
        public MirButton CloseButton;

        public MirButton ScrollUpButton, ScrollDownButton, PositionBar;
        public MirButton ResetButton, EnforceButton;

        public List<MirControl> Rows = new List<MirControl>();

        public bool Enforce = true;

        public int TopLine;
        public int LineCount = 16;

        public int PosX = 491, PosMinY = 101, PosMaxY = 344;

        public KeyBind WaitingForBind = null;

        public KeyboardLayoutDialog()
        {
            Index = 119;
            Library = Libraries.Title;
            Movable = true;
            Sort = true;
            Location = Center;

            TitleLabel = new MirImageControl
            {
                //Index = 7,
                Library = Libraries.Title,
                Location = new Point(18, 4),
                Parent = this
            };

            PageLabel = new MirLabel
            {
                Text = "Keyboard Settings",
                Font = new Font(Settings.FontName, Settings.FontSize + 2, FontStyle.Bold),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Size = new System.Drawing.Size(242, 30),
                Location = new Point(135, 34)
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(489, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };

            CloseButton.Click += (o, e) =>
            {
                CMain.InputKeys.Save(CMain.InputKeys.Keylist);
                Hide();
            };

            ScrollUpButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(491, 88),
                Sound = SoundList.ButtonA,
                Visible = true
            };

            ScrollUpButton.Click += (o, e) =>
            {
                if (TopLine <= 0) return;

                TopLine--;

                UpdateText();
                UpdatePositionBar();
            };

            ScrollDownButton = new MirButton
            {
                Index = 207,
                HoverIndex = 208,
                Library = Libraries.Prguse2,
                PressedIndex = 209,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(491, 363),
                Sound = SoundList.ButtonA,
                Visible = true
            };

            ScrollDownButton.Click += (o, e) =>
            {
                if (TopLine + LineCount >= CMain.InputKeys.Keylist.Count + CMain.InputKeys.Keylist.GroupBy(x => x.Group).Select(y => y.First()).Count()) return;

                TopLine++;

                UpdateText();
                UpdatePositionBar();
            };

            PositionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                PressedIndex = 206,
                Library = Libraries.Prguse2,
                Location = new Point(491, 101),
                Parent = this,
                Movable = true,
                Sound = SoundList.None,
                Visible = true
            };
            PositionBar.OnMoving += PositionBar_OnMoving;


            ResetButton = new MirButton
            {
                Index = 120,
                HoverIndex = 121,
                PressedIndex = 122,
                Library = Libraries.Title,
                Size = new Size(72, 25),
                Location = new Point(30, 400),
                Parent = this,
                Visible = true,
            };
            ResetButton.Click += (o, e) =>
            {
                for (int i = 0; i < CMain.InputKeys.Keylist.Count; i++)
                {
                    KeyBind bind = CMain.InputKeys.Keylist[i];
                    KeyBind defaultBind = CMain.InputKeys.DefaultKeylist[i];

                    if (bind.Key != defaultBind.Key || bind.RequireAlt != defaultBind.RequireAlt || bind.RequireCtrl != defaultBind.RequireCtrl || bind.RequireShift != defaultBind.RequireShift || bind.RequireTilde != defaultBind.RequireTilde)
                    {
                        CMain.InputKeys.Keylist[i].Key = defaultBind.Key;
                        CMain.InputKeys.Keylist[i].RequireAlt = defaultBind.RequireAlt;
                        CMain.InputKeys.Keylist[i].RequireCtrl = defaultBind.RequireCtrl;
                        CMain.InputKeys.Keylist[i].RequireShift = defaultBind.RequireShift;
                        CMain.InputKeys.Keylist[i].RequireTilde = defaultBind.RequireTilde;

                        //CMain.Profile.InputKeys.Save(CMain.Profile.InputKeys.Keylist[i]);
                    }
                }

                UpdateText();

                MirMessageBox messageBox = new MirMessageBox("Keyboard settings have been reset back to default.", MirMessageBoxButtons.OK);
                messageBox.Show();
            };

            EnforceButton = new MirButton
            {
                Visible = true,
                Index = 1346,
                Library = Libraries.Prguse,
                Sound = SoundList.ButtonA,
                Parent = this,
                Location = new Point(105, 406)
            };
            EnforceButton.Click += EnforceButton_Click;

            EnforceButtonChecked = new MirImageControl()
            {
                Visible = Enforce,
                Index = 1347,
                Library = Libraries.Prguse,
                Parent = this,
                NotControl = true,
                Location = new Point(105, 406)
            };

            EnforceButtonLabel = new MirLabel
            {
                Visible = true,
                NotControl = true,
                Parent = this,
                Location = new Point(120, 404),
                AutoSize = true,
                Text = "Assign Rule: Strict"
            };

            UpdateText();
        }

        private void EnforceButton_Click(object sender, EventArgs e)
        {
            Enforce = !Enforce;

            EnforceButtonChecked.Visible = Enforce;

            if (Enforce) EnforceButtonLabel.Text = "Assign Rule: Strict";
            else EnforceButtonLabel.Text = "Assign Rule: Relaxed";
        }

        public void UpdateText()
        {
            foreach (MirControl t in Rows)
                t.Dispose();

            Rows.Clear();

            var orderedList = CMain.InputKeys.Keylist.OrderBy(x => x.Group).ThenBy(x => x.Description).ToList();

            string currentGroup = "";
            int groupCount = 0;

            for (int i = 0; i < orderedList.Count; i++)
            {
                if (i < TopLine) continue;

                int y = (18 * (i - TopLine)) + (groupCount * 30);

                if (y > 260) break;

                if (currentGroup != orderedList[i].Group)
                {
                    Rows.Add(new KeybindHeadingRow(orderedList[i].Group)
                    {
                        Parent = this,
                        Location = new Point(15, 90 + y),
                        Visible = true,
                        Size = new Size(400, 40)
                    });

                    groupCount++;
                    currentGroup = orderedList[i].Group;
                }

                y = (18 * (i - TopLine)) + (groupCount * 30);

                if (y > 260) break;

                Rows.Add(new KeybindRow(orderedList[i].function)
                {
                    Parent = this,
                    Location = new Point(20, 90 + y),
                    Visible = true,
                    Size = new Size(460, 15)
                });
            }
        }

        private void UpdatePositionBar()
        {
            if (CMain.InputKeys.Keylist.Count <= LineCount)
            {
                PositionBar.Visible = false;
                return;
            }

            PositionBar.Visible = true;

            int interval = (PosMaxY - PosMinY) / (CMain.InputKeys.Keylist.Count - LineCount);

            int x = PosX;
            int y = PosMinY + (TopLine * interval);

            if (y >= PosMaxY) y = PosMaxY;
            if (y <= PosMinY) y = PosMinY;


            PositionBar.Location = new Point(x, y);
        }

        private void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = PosX;
            int y = PositionBar.Location.Y;

            if (y >= PosMaxY) y = PosMaxY;
            if (y <= PosMinY) y = PosMinY;

            int location = y - PosMinY;
            int interval = (PosMaxY - PosMinY) / (CMain.InputKeys.Keylist.Count - LineCount);

            double yPoint = (double)location / interval;

            TopLine = Convert.ToInt16(Math.Floor(yPoint));

            PositionBar.Location = new Point(x, y);

            UpdateText();
        }

        public void CheckNewInput(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Oem8 || e.KeyCode == Keys.None) return;

            KeyBind bind = CMain.InputKeys.Keylist.Single(x => x.function == WaitingForBind.function);

            //if (CMain.Profile.InputKeys.Keylist.Any(x => x.Key == e.KeyCode && x.RequireAlt == (byte)(CMain.Alt ? 1 : 0) && x.RequireShift == (byte)(CMain.Shift ? 1 : 0) && x.RequireCtrl == (byte)(CMain.Ctrl ? 1 : 0) && x.RequireTilde == (byte)(CMain.Tilde ? 1 : 0)))
            //{
            //    GameScene.Scene.ChatDialog.ReceiveChat("Another shortcut with this key bind already exists.", ChatType.System);
            //    WaitingForBind = null;
            //    UpdateText();
            //    return;
            //}

            if (e.KeyCode == Keys.Delete)
            {
                bind.Key = Keys.None;
                bind.RequireAlt = bind.RequireShift = bind.RequireCtrl = bind.RequireTilde = 2;
            }
            else
            {
                bind.Key = e.KeyCode;
                bind.RequireAlt = (byte)(CMain.Alt ? 1 : Enforce ? 0 : 2);
                bind.RequireShift = (byte)(CMain.Shift ? 1 : Enforce ? 0 : 2);
                bind.RequireCtrl = (byte)(CMain.Ctrl ? 1 : Enforce ? 0 : 2);
                bind.RequireTilde = (byte)(CMain.Tilde ? 1 : Enforce ? 0 : 2);
            }

            //CMain.Profile.InputKeys.Save(WaitingForBind);
            WaitingForBind = null;
            UpdateText();
        }
    }

    public sealed class KeybindRow : MirControl
    {
        public MirLabel BindName, DefaultBind;
        public MirButton CurrentBindButton;

        public KeyBind KeyBind;

        public KeybindRow(KeybindOptions option)
        {
            KeyBind defaultBind = CMain.InputKeys.DefaultKeylist.Single(x => x.function == option);
            KeyBind currentBind = CMain.InputKeys.Keylist.Single(x => x.function == option);

            KeyBind = currentBind;

            BindName = new MirLabel
            {
                Parent = this,
                Size = new Size(200, 15),
                Location = new Point(0, 0),
                Text = defaultBind.Description,
                Visible = true
            };

            DefaultBind = new MirLabel
            {
                Parent = this,
                Size = new Size(100, 15),
                Location = new Point(200, 0),
                Text = CMain.InputKeys.GetKey(option, true),
                Visible = true
            };

            CurrentBindButton = new MirButton
            {
                Parent = this,
                Text = string.Format("  {0}", CMain.InputKeys.GetKey(option, false)),
                Location = new Point(340, 0),
                Size = new Size(120, 16),
                Visible = true,
                Index = 190,
                Library = Libraries.Prguse2,
                HoverIndex = 191,
                PressedIndex = 192
            };
            CurrentBindButton.Click += (o, e) =>
            {
                if (GameScene.Scene.KeyboardLayoutDialog.WaitingForBind != null)
                {
                    GameScene.Scene.KeyboardLayoutDialog.WaitingForBind = null;
                    GameScene.Scene.KeyboardLayoutDialog.UpdateText();
                    return;
                }

                GameScene.Scene.KeyboardLayoutDialog.WaitingForBind = KeyBind;

                ((MirButton)o).Text = string.Format("  {0}", "????");
                ((MirButton)o).Index = 192;
                ((MirButton)o).HoverIndex = 192;
                ((MirButton)o).PressedIndex = 192;
            };

        }
    }

    public sealed class KeybindHeadingRow : MirControl
    {
        public MirImageControl BulletImage, SpacerImage;
        public MirLabel BulletLabel;

        public KeybindHeadingRow(string groupName)
        {
            BulletImage = new MirImageControl
            {
                Parent = this,
                Index = 201,
                Library = Libraries.Prguse2,
                Size = new Size(8, 6),
                Location = new Point(10, 10),
                Visible = true
            };

            BulletLabel = new MirLabel
            {
                Parent = this,
                Text = groupName,
                Size = new Size(100, 20),
                Location = new Point(20, 5),
                Font = new Font(Settings.FontName, Settings.FontSize + 1, FontStyle.Bold),
                Visible = true
            };

            SpacerImage = new MirImageControl
            {
                Parent = this,
                Index = 202,
                Library = Libraries.Prguse2,
                Size = new Size(3, 464),
                Location = new Point(0, 25),
                Visible = true
            };
        }
    }

}
