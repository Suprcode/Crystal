using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using ClientPackets;
using System;
using System.Drawing;

namespace Client.MirScenes.Dialogs
{
    public class GuildLetterDialog : MirImageControl
    {
        private MirLabel RecipientNameLabel;

        private MirTextBox MessageTextBox;

        private MirButton SendButton, CancelButton, CloseButton;

        public GuildLetterDialog()
        {
            Index = 671;
            Library = Libraries.Title;
            Size = new Size(236, 300);
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

            RecipientNameLabel = new MirLabel
            {
                Text = "",
                Parent = this,
                Font = new Font(Settings.FontName, 8f),
                ForeColour = Color.White,
                Location = new Point(70, 33),
                Size = new Size(150, 15),
                NotControl = true
            };

            MessageTextBox = new MirTextBox
            {
                ForeColour = Color.White,
                Parent = this,
                Font = new Font(Settings.FontName, 8f),
                Location = new Point(15, 92),
                Size = new Size(202, 165)
            };
            MessageTextBox.MultiLine();

            SendButton = new MirButton
            {
                Index = 190,
                HoverIndex = 191,
                PressedIndex = 192,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(30, 265)
            };
            SendButton.Click += (o, e) =>
            {
                BoardInfo info = new BoardInfo();
                info.Name = RecipientNameLabel.Text;
                info.Text = MessageTextBox.Text;
                Network.Enqueue(new SendGuildHouseBoard
                {
                    Mode = 0,
                    Info = info
                });
                Hide();
            };

            CancelButton = new MirButton
            {
                Index = 193,
                HoverIndex = 194,
                PressedIndex = 195,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(135, 265)
            };
            CancelButton.Click += (o, e) =>
            {
                Hide();
            };
        }

        public void Hide()
        {
            bool flag = !Visible;
            if (!flag)
            {
                Visible = false;
            }
        }

        public void ComposeBoard(string recipientName)
        {
            bool flag = string.IsNullOrEmpty(recipientName);
            if (!flag)
            {
                RecipientNameLabel.Text = recipientName;
                MessageTextBox.Text = string.Empty;
                MessageTextBox.SetFocus();
                Visible = true;
            }
        }
    }
}
