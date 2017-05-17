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
    public sealed class RentingDialog : MirImageControl
    {
        public MirLabel NameLabel, RentalPriceLabel;
        public MirButton LockButton, RentalPriceButton;

        public RentingDialog()
        {
            Index = 238;
            Library = Libraries.Prguse;
            Movable = true;
            Size = new Size(204, 109);
            Location = new Point((Settings.ScreenWidth / 2) - Size.Width - 10, Settings.ScreenHeight - 350 - 109);
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
                if (GameScene.User.RentalGoldAmount < 1)
                {
                    MirMessageBox messageBox = new MirMessageBox("Unable to lock rental fee, incorrect fee amount.", MirMessageBoxButtons.OK);
                    messageBox.Show();

                    return;
                }

                Network.Enqueue(new C.ItemRentalLockFee());
            };

            RentalPriceButton = new MirButton
            {
                Index = 28,
                Location = new Point(18, 46),
                Size = new Size(32, 17),
                Library = Libraries.Prguse,
                Parent = this,
                Sound = SoundList.Gold,
            };
            RentalPriceButton.Click += (o, e) =>
            {
                if (GameScene.SelectedCell == null && GameScene.Gold > 0)
                {
                    MirAmountBox amountBox = new MirAmountBox("Rental fee:", 116, GameScene.Gold);

                    amountBox.OKButton.Click += (c, a) =>
                    {
                        if (amountBox.Amount > 0)
                        {
                            GameScene.User.RentalGoldAmount += amountBox.Amount;
                            Network.Enqueue(new C.ItemRentalFee { Amount = GameScene.User.RentalGoldAmount });

                            RefreshInterface();
                        }
                    };

                    amountBox.Show();
                    GameScene.PickedUpGold = false;
                }
            };

            NameLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(30, 8),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };

            RentalPriceLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(60, 42),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.Left | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };
        }

        public void RefreshInterface()
        {
            NameLabel.Text = GameScene.User.Name;
            RentalPriceLabel.Text = string.Format("Rental Fee: {0}", GameScene.User.RentalGoldAmount.ToString("###,###,##0"));

            GameScene.Scene.GuestLoaningDialog.RefreshInterface();
            GameScene.Scene.GuestRentingDialog.RefreshInterface();

            Redraw();
        }

        public void RentingAccept()
        {
            GameScene.Scene.InventoryDialog.Location = new Point(Settings.ScreenWidth - GameScene.Scene.InventoryDialog.Size.Width, 0);
            GameScene.Scene.InventoryDialog.Show();

            RefreshInterface();

            Show();
            GameScene.Scene.GuestLoaningDialog.Show();
        }

        public void RentingReset()
        {
            GameScene.User.RentalGoldAmount = 0;
            RentingUnlock();
            GameScene.Scene.GuestRentingDialog.RentingReset();
            
            RefreshInterface();

            Hide();
        }

        public void RentingLock()
        {
            LockButton.Index = 253;
            LockButton.Enabled = false;
            RentalPriceButton.Enabled = false;

            RefreshInterface();
        }

        public void RentingUnlock()
        {
            LockButton.Index = 250;
            LockButton.Enabled = true;
            RentalPriceButton.Enabled = true;
        }

        public void RentingCancel()
        {
            Network.Enqueue(new C.CancelItemRental());
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

    public sealed class GuestRentingDialog : MirImageControl
    {
        public MirLabel NameLabel, RentalPriceLabel;
        public MirButton LockButton, RentalPriceButton;
        public string GuestName;
        public uint GuestGold;
        public bool GuestGoldLocked;

        public GuestRentingDialog()
        {
            Index = 238;
            Library = Libraries.Prguse;
            Movable = true;
            Size = new Size(204, 109);
            Location = new Point((Settings.ScreenWidth / 2) - Size.Width - 10, Settings.ScreenHeight - 350 - 109);
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

            RentalPriceButton = new MirButton
            {
                Index = 28,
                Location = new Point(18, 46),
                Size = new Size(32, 17),
                Library = Libraries.Prguse,
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

            RentalPriceLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(60, 42),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.Left | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };
        }

        public void RefreshInterface()
        {
            NameLabel.Text = GuestName;
            RentalPriceLabel.Text = string.Format("Rental Fee: {0}", GuestGold.ToString("###,###,##0"));

            Redraw();
        }

        public void RentingReset()
        {
            RentingUnlock();
            GuestName = string.Empty;
            GuestGold = 0;

            Hide();
        }

        public void RentingLock()
        {
            LockButton.Index = 253;
            GuestGoldLocked = true;

            RefreshInterface();
        }

        public void RentingUnlock()
        {
            LockButton.Index = 250;
            GuestGoldLocked = false;
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
