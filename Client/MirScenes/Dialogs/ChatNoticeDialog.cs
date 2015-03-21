using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Client.MirScenes.Dialogs
{
    public class ChatNoticeDialog : MirImageControl
    {
        public MirImageControl Layout;
        public MirLabel TextLabel1, TextLabel2;
        private long ViewTime = 10000;
        private long CurrentTime = 0;

        public ChatNoticeDialog()
        {
            Index = 1361;
            Library = Libraries.Prguse;
            Movable = false;
            Sort = false;
            Location = new Point(Settings.ScreenWidth / 2 - Size.Width / 2, Settings.ScreenHeight / 6 - Size.Height / 2);
            Opacity = 0.7F;

            TextLabel1 = new MirLabel
            {
                Text = "",
                Font = new Font(Settings.FontName, 10F),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                NotControl = true,
                Location = new Point(0, -6),
                Size = new Size(660, 40),
                ForeColour = Color.Yellow,
                OutLineColour = Color.Black,
            };

            TextLabel2 = new MirLabel
            {
                Text = "",
                Font = new Font(Settings.FontName, 15F),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                NotControl = true,
                Location = new Point(0, 0),
                Size = new Size(660, 40),
                ForeColour = Color.Yellow,
                OutLineColour = Color.Black,
            };

            Layout = new MirImageControl
            {
                Index = 1360,
                Library = Libraries.Prguse,
                Location = new Point(0, 0),
                Parent = this
            };

            AfterDraw += ChatNotice_AfterDraw;
        }

        private void ChatNotice_AfterDraw(object sender, EventArgs e)
        {

            if (CurrentTime < CMain.Time)
            {
                Hide();
            }
        }

        public void ShowNotice(string text, int type = 0)
        {
            Index = type == 0 ? 1361 : 1363;
            Layout.Index = type == 0 ? 1360 : 1362;
            TextLabel1.Text = TextLabel2.Text = text;
            TextLabel1.Visible = type == 0;
            TextLabel2.Visible = type == 1;

            Show();
            CurrentTime = CMain.Time + ViewTime;
        }

        public void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }
    }
}
