using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;

namespace Client.MirScenes.Dialogs
{
    public sealed class NoticeDialog : MirImageControl
    {
        public static Regex C = new Regex(@"{((.*?)\/(.*?))}");
        public static Regex L = new Regex(@"\(((.*?)\/(.*?))\)");

        public MirButton CloseButton, UpButton, DownButton, PositionBar, OkButton;
        public MirLabel[] TextLabel;
        public List<MirLabel> TextButtons;
        public MirImageControl TitleLabel;
        public MirLabel NameLabel;

        public Notice Notice = new Notice();

        Font font = new Font(Settings.FontName, 10F);

        public List<string> CurrentLines = new List<string>();
        private int _index = 0;
        public int MaximumLines = 19;

        public NoticeDialog()
        {
            Index = 961;
            Library = Libraries.Prguse;
            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 3);
            TextLabel = new MirLabel[40];
            TextButtons = new List<MirLabel>();

            MouseWheel += LoginNoticeDialog_MouseWheel;

            Sort = true;

            NameLabel = new MirLabel
            {
                Text = "",
                Parent = this,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                ForeColour = Color.BurlyWood,
                Location = new Point(30, 6),
                AutoSize = true
            };

            CloseButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(289, 3),
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            OkButton = new MirButton
            {
                Enabled = true,
                HoverIndex = 194,
                Index = 193,
                Library = Libraries.Title,
                Location = new Point(120, 436),
                Parent = this,
                PressedIndex = 195,
            };
            OkButton.Click += (o, e) => Hide();

            UpButton = new MirButton
            {
                Index = 470,
                HoverIndex = 471,
                PressedIndex = 472,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(293, 33),
                Sound = SoundList.ButtonA,
                Visible = false
            };
            UpButton.Click += (o, e) =>
            {
                if (_index <= 0) return;

                _index--;

                NewText(CurrentLines, false);
                UpdatePositionBar();
            };

            DownButton = new MirButton
            {
                Index = 473,
                HoverIndex = 474,
                Library = Libraries.Prguse2,
                PressedIndex = 475,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(293, 418),
                Sound = SoundList.ButtonA,
                Visible = true
            };
            DownButton.Click += (o, e) =>
            {
                if (_index + MaximumLines >= CurrentLines.Count) return;

                _index++;

                NewText(CurrentLines, false);
                UpdatePositionBar();
            };

            PositionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                PressedIndex = 206,
                Library = Libraries.Prguse2,
                Location = new Point(293, 46),
                Parent = this,
                Movable = true,
                Sound = SoundList.None,
                Visible = true
            };
            PositionBar.OnMoving += PositionBar_OnMoving;
        }

        private void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = 293;
            int y = PositionBar.Location.Y;

            if (y >= 399) y = 399;
            if (y <= 46) y = 46;

            int location = y - 46;
            int interval = 400 / (CurrentLines.Count - MaximumLines);

            double yPoint = location / interval;

            _index = Convert.ToInt16(Math.Floor(yPoint));

            NewText(CurrentLines, false);

            PositionBar.Location = new Point(x, y);
        }

        private void LoginNoticeDialog_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (_index == 0 && count >= 0) return;
            if (_index == CurrentLines.Count - 1 && count <= 0) return;
            if (CurrentLines.Count <= MaximumLines) return;

            _index -= count;

            if (_index < 0) _index = 0;
            if (_index + MaximumLines > CurrentLines.Count - 1) _index = CurrentLines.Count - MaximumLines;

            NewText(CurrentLines, false);

            UpdatePositionBar();
        }

        private void UpdatePositionBar()
        {
            if (CurrentLines.Count <= MaximumLines) return;

            int interval = 400 / (CurrentLines.Count - MaximumLines);

            int x = 293;
            int y = 46 + (_index * interval);

            if (y >= 399) y = 399;
            if (y <= 46) y = 46;

            PositionBar.Location = new Point(x, y);
        }

        public void Update(Notice notice)
        {
            this.Notice = notice;

            List<string> temp = new List<string>();

            if (string.IsNullOrWhiteSpace(notice.Message))
            {
                return;
            }

            string[] lines = notice.Message.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                temp.Add(line);
            }

            NewText(temp);
            Visible = true;
        }

        public void NewText(List<string> lines, bool resetIndex = true)
        {
            if (resetIndex)
            {
                _index = 0;
                CurrentLines = lines;
                UpdatePositionBar();
            }

            if (resetIndex)
            {
                if (Index != -1)
                {
                    MaximumLines = 19;
                    if (lines.Count > MaximumLines)
                    {
                        UpButton.Visible = true;
                        DownButton.Visible = true;
                        PositionBar.Visible = true;
                    }
                    else
                    {
                        UpButton.Visible = false;
                        DownButton.Visible = false;
                        PositionBar.Visible = false;
                    }
                }
                else
                {
                    MaximumLines = 19;
                    UpButton.Visible = false;
                    DownButton.Visible = false;
                    PositionBar.Visible = false;
                }
            }

            for (int i = 0; i < TextButtons.Count; i++)
                TextButtons[i].Dispose();

            for (int i = 0; i < TextLabel.Length; i++)
            {
                if (TextLabel[i] != null) TextLabel[i].Text = "";
            }

            TextButtons.Clear();

            if (!string.IsNullOrEmpty(Notice.Title))
            {
                NameLabel.Text = Notice.Title;
            }

            int lastLine = lines.Count > MaximumLines ? ((MaximumLines + _index) > lines.Count ? lines.Count : (MaximumLines + _index)) : lines.Count;

            for (int i = _index; i < lastLine; i++)
            {
                TextLabel[i] = new MirLabel
                {
                    Font = font,
                    DrawFormat = TextFormatFlags.WordBreak,
                    Visible = true,
                    Parent = this,
                    Size = new Size(420, 20),
                    Location = new Point(25, 50 + (i - _index) * 20),
                    NotControl = true
                };

                if (i >= lines.Count)
                {
                    TextLabel[i].Text = string.Empty;
                    continue;
                }

                string currentLine = lines[i];

                List<Match> matchList = C.Matches(currentLine).Cast<Match>().ToList();
                matchList.AddRange(L.Matches(currentLine).Cast<Match>());

                int oldLength = currentLine.Length;

                foreach (Match match in matchList.OrderBy(o => o.Index).ToList())
                {
                    int offSet = oldLength - currentLine.Length;

                    Capture capture = match.Groups[1].Captures[0];
                    string txt = match.Groups[2].Captures[0].Value;
                    string action = match.Groups[3].Captures[0].Value;

                    currentLine = currentLine.Remove(capture.Index - 1 - offSet, capture.Length + 2).Insert(capture.Index - 1 - offSet, txt);
                    string text = currentLine.Substring(0, capture.Index - 1 - offSet) + " ";
                    Size size = TextRenderer.MeasureText(CMain.Graphics, text, TextLabel[i].Font, TextLabel[i].Size, TextFormatFlags.TextBoxControl);

                    if (L.Match(match.Value).Success)
                    {
                        NewLink(txt, action, TextLabel[i].Location.Add(new Point(size.Width - 11, 0)));
                    }

                    if (C.Match(match.Value).Success)
                    {
                        NewColour(txt, action, TextLabel[i].Location.Add(new Point(size.Width - 11, 0)));
                    }
                }

                TextLabel[i].Text = currentLine;
                TextLabel[i].MouseWheel += LoginNoticeDialog_MouseWheel;

            }
            OkButton.BringToFront();
        }

        private void NewLink(string text, string link, Point p)
        {
            MirLabel temp = new MirLabel
            {
                AutoSize = true,
                Visible = true,
                Parent = this,
                Location = p,
                Text = text,
                ForeColour = Color.Yellow,
                Sound = SoundList.ButtonC,
                Font = font
            };


            temp.MouseEnter += (o, e) => temp.ForeColour = Color.Red;
            temp.MouseLeave += (o, e) => temp.ForeColour = Color.Yellow;
            temp.MouseDown += (o, e) => temp.ForeColour = Color.Yellow;
            temp.MouseUp += (o, e) => temp.ForeColour = Color.Red;

            temp.Click += (o, e) =>
            {
                if (link.StartsWith("http://", true, CultureInfo.InvariantCulture))
                {
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = link,
                        UseShellExecute = true
                    });
                }
            };
            temp.MouseWheel += LoginNoticeDialog_MouseWheel;

            TextButtons.Add(temp);
        }

        private void NewColour(string text, string colour, Point p)
        {
            Color textColour = Color.FromName(colour);

            MirLabel temp = new MirLabel
            {
                AutoSize = true,
                Visible = true,
                Parent = this,
                Location = p,
                Text = text,
                ForeColour = textColour,
                Font = font
            };
            temp.MouseWheel += LoginNoticeDialog_MouseWheel;

            TextButtons.Add(temp);
        }
    }
}
