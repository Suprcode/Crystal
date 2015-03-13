using Client.MirControls;
using Client.MirGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Client.MirSounds;

namespace Client.MirScenes.Dialogs
{
    public class RankingDialog : MirImageControl
    {
        public MirImageControl TitleLabel;
        public MirButton Tab1, Tab2, Tab3, Tab4, Tab5, Tab6, Tab7;
        public MirButton CloseButton;

        public RankingDialog()
        {
            Index = 260;
            Library = Libraries.Prguse2;
            Size = new Size(288, 324);
            Movable = true;
            Sort = true;
            Location = new Point((800 - Size.Width) / 2, (600 - Size.Height) / 2);

            TitleLabel = new MirImageControl
            {
                Index = 11,
                Library = Libraries.Title,
                Location = new Point(18, 4),
                Parent = this
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(255, 5),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();


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

        public void Toggle()
        {
            if (!Visible)
                Show();
            else
                Hide();
        }
    }
}
