using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class LoaningDialog : MirImageControl
    {
        public MirItemCell ItemCell;
        public MirLabel NameLabel, RentalPeriodLabel;
        public MirButton LockButton, SetRentalPeriodButton, ConfirmButton, CloseButton;
        public static UserItem LoanItem;
        public int RentalDays;

        public LoaningDialog()
        {
            Index = 238;
            Library = Libraries.Prguse;
            Movable = true;
            Size = new Size(204, 109);
            Location = new Point((Settings.ScreenWidth / 2) - Size.Width - 10, Settings.ScreenHeight - 350);
            Sort = true;

            LockButton = new MirButton
            {
                Index = 250,
                HoverIndex = 251,
                Location = new Point(22, 76),
                Size = new Size(28, 25),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 252,
                Sound = SoundList.ButtonA,
            };
            LockButton.Click += (o, e) =>
            {
                if (RentalDays < 1 || RentalDays > 30)
                {
                    MirMessageBox messageBox = new MirMessageBox("Unable to lock rental item, incorrect rental period.", MirMessageBoxButtons.OK);
                    messageBox.Show();

                    return;
                }
                    
                Network.Enqueue(new C.RentalItemLock());
            };

            SetRentalPeriodButton = new MirButton
            {
                Index = 126,
                HoverIndex = 127,
                Location = new Point(52, 76),
                Size = new Size(72, 25),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 128,
                Sound = SoundList.ButtonA,
            };
            SetRentalPeriodButton.Click += (o, e) =>
            {
                LoaningInputRentalPeroid();
            };

            ConfirmButton = new MirButton
            {
                Index = 123,
                HoverIndex = 124,
                Location = new Point(126, 76),
                Size = new Size(72, 25),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 125,
                Sound = SoundList.ButtonA,
                Enabled = false
            };
            ConfirmButton.Click += (o, e) =>
            {

            };

            NameLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(30, 8),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };

            RentalPeriodLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(40, 42),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };

            ItemCell = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.Renting,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(16, 35),
                ItemSlot = 0,
            };
        }

        public void RefreshInterface()
        {
            NameLabel.Text = GameScene.User.Name;
            RentalPeriodLabel.Text = string.Format("Rental Period: {0} Days", RentalDays.ToString());

            GameScene.Scene.GuestRentingDialog.RefreshInterface();
            GameScene.Scene.GuestLoaningDialog.RefreshInterface();

            Redraw();
        }

        public void LoaningAccept()
        {
            GameScene.Scene.InventoryDialog.Location = new Point(Settings.ScreenWidth - GameScene.Scene.InventoryDialog.Size.Width, 0);
            GameScene.Scene.InventoryDialog.Show();

            RefreshInterface();

            Show();
            GameScene.Scene.GuestRentingDialog.Show();
        }

        public void LoaningReset()
        {
            LoanItem = null;
            RentalDays = 0;
            ConfirmButton.Enabled = false;
            GameScene.User.RentalGoldAmount = 0;
            LoaningUnlock();
            GameScene.Scene.GuestLoaningDialog.LoaningReset();

            RefreshInterface();

            Hide();
        }

        public void LoaningCancel()
        {
            Network.Enqueue(new C.RentalCancel());
        }

        public void LoaningLock()
        {
            LockButton.Index = 253;
            LockButton.Enabled = false;
            SetRentalPeriodButton.Enabled = false;

            RefreshInterface();
        }

        public void LoaningUnlock()
        {
            LockButton.Index = 250;
            LockButton.Enabled = true;
            SetRentalPeriodButton.Enabled = true;
        }

        public void LoaningInputRentalPeroid()
        {
            MirInputBox inputBox = new MirInputBox("How long would you like to rent " + LoanItem.Name + " to " + GameScene.Scene.GuestRentingDialog.GuestName + " for? (1 to 30 days).");

            inputBox.OKButton.Click += (o1, e1) =>
            {
                if (int.TryParse(inputBox.InputTextBox.Text, out RentalDays))
                {
                    if (RentalDays >= 1 && RentalDays <= 30)
                    {
                        RefreshInterface();
                        inputBox.Dispose();
                    }
                }
                else
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("Rental peroid needs to be between 1 and 30 days.", ChatType.System);
                    RentalDays = 0;
                    inputBox.Dispose();
                }
            };

            inputBox.Show();
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

    public sealed class GuestLoaningDialog : MirImageControl
    {
        public MirItemCell GuestItemCell;
        public MirLabel NameLabel, RentalPeriodLabel;
        public MirButton LockButton, SetRentalPeriodButton, ConfirmButton, CloseButton;
        public string GuestName;
        public bool GuestItemLocked;
        public static UserItem GuestLoanItem;

        public GuestLoaningDialog()
        {
            Index = 238;
            Library = Libraries.Prguse;
            Movable = true;
            Size = new Size(204, 109);
            Location = new Point((Settings.ScreenWidth / 2) - Size.Width - 10, Settings.ScreenHeight - 350);
            Sort = true;

            LockButton = new MirButton
            {
                Index = 250,
                Location = new Point(22, 76),
                Size = new Size(28, 25),
                Library = Libraries.Prguse,
                Parent = this,
                Enabled = false
            };

            SetRentalPeriodButton = new MirButton
            {
                Index = 126,
                Location = new Point(52, 76),
                Size = new Size(72, 25),
                Library = Libraries.Title,
                Parent = this,
                Enabled = false
            };

            ConfirmButton = new MirButton
            {
                Index = 123,
                Location = new Point(126, 76),
                Size = new Size(72, 25),
                Library = Libraries.Title,
                Parent = this,
                Enabled = false
            };

            NameLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(30, 8),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };

            RentalPeriodLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(40, 42),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
                Text = "Rental Period: 0 Days"
            };

            GuestItemCell = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.GuestRenting,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(16, 35),
                ItemSlot = 0,
            };
        }

        public void RefreshInterface()
        {
            NameLabel.Text = GuestName;

            if (GuestLoanItem != null)
                GameScene.Bind(GuestLoanItem);
            
            Redraw();
        }

        public void LoaningReset()
        {
            LoaningUnlock();
            GuestLoanItem = null;
            GuestName = string.Empty;

            Hide();
        }

        public void LoaningLock()
        {
            LockButton.Index = 253;
            GuestItemLocked = true;

            RefreshInterface();
        }

        public void LoaningUnlock()
        {
            LockButton.Index = 250;
            GuestItemLocked = false;
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
