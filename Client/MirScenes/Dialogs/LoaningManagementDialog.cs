using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System.Drawing;

namespace Client.MirScenes.Dialogs
{
    public sealed class LoaningManagementDialog : MirImageControl
    {
        public MirImageControl WindowTitle;
        public MirButton RentItemButton, RentedTabButton, BorrowedTabButton, CloseButton;

        public LoaningManagementDialog()
        {
            Index = 1;
            Library = Libraries.Prguse3;
            Movable = true;
            Size = new Size(400, 174);
            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);
            Sort = true;

            // Title

            WindowTitle = new MirImageControl
            {
                Index = 0,
                Library = Libraries.Prguse3,
                Location = new Point(22, 8),
                Parent = this
            };

            // Rented Tab

            RentedTabButton = new MirButton
            {
                Index = 2,
                HoverIndex = 2,
                Location = new Point(8, 32),
                Size = new Size(72, 23),
                Library = Libraries.Prguse3,
                Parent = this,
                PressedIndex = 2,
                Sound = SoundList.ButtonA,
                Enabled = false
            };

            // Borrowed Tab

            BorrowedTabButton = new MirButton
            {
                Index = 3,
                Location = new Point(81, 32),
                Size = new Size(84, 23),
                Library = Libraries.Prguse3,
                Parent = this,
                Sound = SoundList.ButtonA,
                Enabled = false
            };

            // Rent Item Button

            RentItemButton = new MirButton
            {
                Index = 4,
                HoverIndex = 5,
                Location = new Point(295, 144),
                Size = new Size(85, 29),
                Library = Libraries.Prguse3,
                Parent = this,
                PressedIndex = 6,
                Sound = SoundList.ButtonA,
            };

            // Close Button

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(375, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();
        }

        public void Hide()
        {
            Visible = false;
        }

        public void Show()
        {
            Visible = true;
        }
    }
}
