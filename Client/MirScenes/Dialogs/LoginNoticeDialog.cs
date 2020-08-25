using Client.MirControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Client.MirGraphics;
using System.Windows.Forms;
using Client.MirSounds;
using System.Text.RegularExpressions;
using Shared;

namespace Client.MirScenes.Dialogs
{
    public sealed class LoginNoticeDialog : MirImageControl
    {
        public static Regex R = new Regex(@"<(.*?/\@.*?)>");
        public static Regex C = new Regex(@"{(.*?/.*?)}");
        public MirButton CloseButton, UpButton, DownButton, PositionBar, OkButton;
        public MirLabel[] TextLabel;
        public List<MirLabel> TextButtons;
        public List<LogNotice> notice = new List<LogNotice>();
        public MirImageControl TitleLabel;
        public MirLabel NameLabel;

        Font font = new Font(Settings.FontName, 10F);

        public List<string> CurrentLines = new List<string>();
        private int _index = 0;
        public int MaximumLines = 19;

        public LoginNoticeDialog()
        {
            Index = 961;
            Library = Libraries.Prguse;
            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 3);
            TextLabel = new MirLabel[40];
            TextButtons = new List<MirLabel>();

            MouseWheel += LoginNoticeDialog_MouseWheel;

            Sort = true;

            CloseButton = new MirButton
            {
                Index = 633,
                HoverIndex = 634,
                PressedIndex = 635,
                Location = new Point(285, 4),
                Library = Libraries.Title,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            OkButton = new MirButton
            {
                Enabled = true,
                HoverIndex = 194,
                Index = 193,
                Library = Libraries.Title,
                Location = new Point(120, 441),
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
                Location = new Point(292, 33),
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
                Location = new Point(292, 416),
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
                Location = new Point(292, 48),
                Parent = this,
                Movable = true,
                Sound = SoundList.None,
                Visible = true
            };
            PositionBar.OnMoving += PositionBar_OnMoving;
        }

        private void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = 292;
            int y = PositionBar.Location.Y;

            if (y >= 418) y = 418;
            if (y <= 48) y = 48;

            int location = y - 47;
            int interval = 108 / (CurrentLines.Count - MaximumLines);

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

            int interval = 108 / (CurrentLines.Count - MaximumLines);

            int x = 292;
            int y = 48 + (_index * interval);

            if (y >= 418) y = 418;
            if (y <= 48) y = 48;

            PositionBar.Location = new Point(x, y);
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

                List<Match> matchList = R.Matches(currentLine).Cast<Match>().ToList();
                matchList.AddRange(C.Matches(currentLine).Cast<Match>());

                int oldLength = currentLine.Length;

                foreach (Match match in matchList.OrderBy(o => o.Index).ToList())
                {
                    int offSet = oldLength - currentLine.Length;

                    Capture capture = match.Groups[1].Captures[0];
                    string[] values = capture.Value.Split('/');
                    currentLine = currentLine.Remove(capture.Index - 1 - offSet, capture.Length + 2).Insert(capture.Index - 1 - offSet, values[0]);
                    string text = currentLine.Substring(0, capture.Index - 1 - offSet) + " ";
                    Size size = TextRenderer.MeasureText(CMain.Graphics, text, TextLabel[i].Font, TextLabel[i].Size, TextFormatFlags.TextBoxControl);

                    if (R.Match(match.Value).Success)
                    {
                        string link = GetLinkFromString(values[0], values[1]);

                        NewButton(values[0], values[1], TextLabel[i].Location.Add(new Point(size.Width - 11, 0)), link);
                    }

                    if (C.Match(match.Value).Success)
                        NewColour(values[0], values[1], TextLabel[i].Location.Add(new Point(size.Width - 11, 0)));
                }

                TextLabel[i].Text = currentLine;
                TextLabel[i].MouseWheel += LoginNoticeDialog_MouseWheel;

            }
            OkButton.BringToFront();
        }


        public string GetLinkFromString(string source, string link)
        {
            string retString = string.Empty;
            for (int i = 0; i < CurrentLines.Count; i++)
            {
                if (CurrentLines[i].Contains("<" + source + "/" + link + ">"))
                {
                    for (int x = 0; x < notice.Count; x++)
                    {
                        if (notice[x].LogString.Contains("<" + source + "/" + link + ">"))
                            retString = notice[x].Link;
                    }
                }
            }
            return retString;
        }

        private void NewButton(string text, string key, Point p, string link = "")
        {
            key = string.Format("[{0}]", key);

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
            //Fontstyle.Underline;

            temp.MouseEnter += (o, e) => temp.ForeColour = Color.Red;
            temp.MouseLeave += (o, e) => temp.ForeColour = Color.Yellow;
            temp.MouseDown += (o, e) => temp.ForeColour = Color.Yellow;
            temp.MouseUp += (o, e) => temp.ForeColour = Color.Red;

            temp.Click += (o, e) =>
            {
                if (link.Length > 0 && link.Contains("http") && link.Contains("://"))
                {
                    System.Diagnostics.Process.Start(link);
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


        public override void Hide()
        {
            Visible = false;
        }
    }
}
