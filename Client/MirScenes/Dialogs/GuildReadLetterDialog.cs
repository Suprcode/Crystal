using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using ClientPackets;
using System;
using System.Drawing;

namespace Client.MirScenes.Dialogs
{
    public class GuildReadLetterDialog : MirImageControl
    {
        private MirLabel SenderNameLabel, DateSentLabel;

        private MirTextBox MessageTextBox;

        private MirButton DeleteButton, ModifyButton, CloseButton;

        public BoardInfo Info;

        public GuildReadLetterDialog()
        {
            Index = 689;
            Library = Libraries.Prguse;
            Size = new Size(296, 252);
            Movable = true;
            Sort = true;
            Location = new Point(100, 100);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(Size.Width - 27, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA
            };
            CloseButton.Click += (o, e) =>
            {
                Hide();
            };

            SenderNameLabel = new MirLabel
            {
                Parent = this,
                Font = new Font(Settings.FontName, 12f),
                ForeColour = Color.Goldenrod,
                Location = new Point(10, 11),
                Size = new Size(150, 20),
                NotControl = true
            };

            DateSentLabel = new MirLabel
            {
                Parent = this,
                Font = new Font(Settings.FontName, 8f),
                ForeColour = Color.White,
                Location = new Point(10, 35),
                Size = new Size(150, 15),
                NotControl = true
            };

            MessageTextBox = new MirTextBox
            {
                ForeColour = Color.White,
                Parent = this,
                Font = new Font(Settings.FontName, 8f),
                Location = new Point(12, 55),
                Size = new Size(265, 155),
                CanLoseFocus = true,
                ReadOnly = true
            };
            MessageTextBox.MultiLine();

            DeleteButton = new MirButton
            {
                Index = 540,
                HoverIndex = 541,
                PressedIndex = 542,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(144, 223)
            };
            DeleteButton.Click += (o, e) =>
            {
                bool flag = Info == null;
                if (!flag)
                {
                    Network.Enqueue(new SendGuildHouseBoard
                    {
                        Mode = 2,
                        Info = Info
                    });
                    Hide();
                }
            };

            ModifyButton = new MirButton
            {
                Index = 193,
                HoverIndex = 194,
                PressedIndex = 195,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(214, 223)
            };
            ModifyButton.Click += (o, e) =>
            {
                Hide();
            };
        }

        public void ReadBoard(BoardInfo info)
        {
            bool flag = info == null;
            if (!flag)
            {
                Info = info;
                SenderNameLabel.Text = info.Name;
                DateSentLabel.Text = info.Date.ToString("dd/MM/yy H:mm:ss");
                MessageTextBox.Text = info.Text.Replace("\\r\\n", "\r\n");
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