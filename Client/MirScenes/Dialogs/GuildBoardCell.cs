using Client.MirControls;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client.MirScenes.Dialogs
{
    public sealed class GuildBoardCell : MirControl
    {
        private BoardInfo _info;

        private MirLabel NameLabel, TextLabel;

        public BoardInfo Info
        {
            get
            {
                return this._info;
            }
            set
            {
                _info = value;
                bool flag = _info != null;
                if (flag)
                {
                    NameLabel.Text = _info.Name;
                    string[] buffer = _info.Text.Split(new char[]
                    {
                        '\n'
                    });
                    bool flag2 = buffer.Length != 0;
                    if (flag2)
                    {
                        TextLabel.Text = GetText(buffer[0], 45);
                    }
                    else
                    {
                        TextLabel.Text = GetText(_info.Text, 45);
                    }
                }
            }
        }

        public GuildBoardCell()
        {
            Border = false;
            BorderColour = Color.Gray;
            BeforeDraw += new EventHandler(this.Border_BeforeDraw);
            Click += (o, e) =>
            {
                GuildBoardDialog.SelectBoard = this;
            };
            NameLabel = new MirLabel
            {
                Text = "text",
                Location = new Point(11, 0),
                Parent = this,
                AutoSize = true,
                DrawFormat = TextFormatFlags.Default,
                Font = new Font(Settings.FontName, 9f),
                ForeColour = Color.White,
                NotControl = true
            };
            TextLabel = new MirLabel
            {
                Text = "text",
                Location = new Point(150, 0),
                Parent = this,
                AutoSize = true,
                DrawFormat = TextFormatFlags.Default,
                Font = new Font(Settings.FontName, 9f),
                ForeColour = Color.White,
                NotControl = true
            };
        }

        private string GetText(string text, int count)
        {
            string result;
            for (int i = 0; i < text.Length; i++)
            {
                string d = text.Remove(i);
                bool flag = Encoding.Default.GetBytes(d).Length >= count;
                if (flag)
                {
                    result = d + "...";
                    return result;
                }
            }
            result = text;
            return result;
        }

        private void Border_BeforeDraw(object sender, EventArgs e)
        {
            Border = (IsMouseOver(MirControl.MouseControl.DisplayLocation) || (GuildBoardDialog.SelectBoard != null && GuildBoardDialog.SelectBoard == this));
            NameLabel.ForeColour = ((Info != null && Info.Notice) ? Color.Yellow : Color.White);
            TextLabel.ForeColour = ((Info != null && Info.Notice) ? Color.Yellow : Color.White);
        }

        public void Show()
        {
            bool visible = Visible;
            if (!visible)
            {
                Visible = true;
            }
        }

        public void Hide()
        {
            bool flag = !Visible;
            if (!flag)
            {
                Visible = false;
            }
        }
    }
}
