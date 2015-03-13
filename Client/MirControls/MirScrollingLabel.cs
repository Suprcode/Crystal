using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Client.MirControls
{
    public class MirScrollingLabel : MirControl
    {
        private static readonly Regex R = new Regex(@"<(.*?/\@.*?)>");
        private static readonly Regex C = new Regex(@"{(.*?/.*?)}");

        private readonly MirLabel[] _textLabel;
        private readonly List<MirLabel> _textButtons;

        public int Index;

        public Font Font = new Font(Settings.FontName, 8F);
        public List<string> CurrentLines = new List<string>();
        public int VisibleLines = 8;

        public MirScrollingLabel()
        {
            _textLabel = new MirLabel[12];
            _textButtons = new List<MirLabel>();
        }

        public void NewText(List<string> lines, bool resetIndex = true)
        {
            if (resetIndex)
            {
                Index = 0;
                CurrentLines = lines;
            }

            foreach (MirLabel t in _textButtons)
                t.Dispose();

            foreach (MirLabel t in _textLabel.Where(t => t != null))
                t.Text = "";

            _textButtons.Clear();

            int lastLine = lines.Count > VisibleLines ? ((VisibleLines + Index) > lines.Count ? lines.Count : (VisibleLines + Index)) : lines.Count;

            for (int i = Index; i < lastLine; i++)
            {
                _textLabel[i - Index] = new MirLabel
                {
                    Font = Font,
                    DrawFormat = TextFormatFlags.WordBreak,
                    Visible = true,
                    Parent = this,
                    Size = new Size(Size.Width, 20),
                    Location = new Point(0, 0 + (i - Index) * 15),
                    NotControl = true
                };

                if (i >= lines.Count)
                {
                    _textLabel[i - Index].Text = string.Empty;
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
                    Size size = TextRenderer.MeasureText(CMain.Graphics, text, _textLabel[i - Index].Font, _textLabel[i - Index].Size, TextFormatFlags.TextBoxControl);

                    //if (R.Match(match.Value).Success)
                    //    NewButton(values[0], values[1], TextLabel[i].Location.Add(new Point(size.Width - 10, 0)));

                    if (C.Match(match.Value).Success)
                        NewColour(values[0], values[1], _textLabel[i - Index].Location.Add(new Point(size.Width - 10, 0)));
                }

                _textLabel[i - Index].Text = currentLine;
                //TextLabel[i].MouseWheel += NPCDialog_MouseWheel;
            }
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
                Font = Font
            };
            //temp.MouseWheel += NPCDialog_MouseWheel;

            _textButtons.Add(temp);
        }
    }
}
