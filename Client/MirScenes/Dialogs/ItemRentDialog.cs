using System;
using System.Diagnostics;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using System.Drawing;
using System.Windows.Forms;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class ItemRentDialog : MirImageControl
    {
        private readonly MirLabel _nameLabel, _rentalPriceLabel;
        private readonly MirButton _lockButton, _rentalPriceButton;

        public ItemRentDialog()
        {
            Index = 238;
            Library = Libraries.Prguse;
            Movable = true;
            Size = new Size(204, 109);
            Location = new Point(Settings.ScreenWidth - Size.Width - Size.Width / 2, Size.Height + Size.Height / 2);
            Sort = true;

            var closeButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(180, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            closeButton.Click += (sender, args) =>
            {
                CancelItemRental();
            };

            _lockButton = new MirButton
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
            _lockButton.Click += (o, e) =>
            {
                if (GameScene.User.RentalGoldAmount < 1)
                    return;

                Network.Enqueue(new C.ItemRentalLockFee());
            };

            _rentalPriceButton = new MirButton
            {
                Index = 28,
                Location = new Point(18, 46),
                Size = new Size(32, 17),
                Library = Libraries.Prguse,
                Parent = this,
                Sound = SoundList.Gold,
            };
            _rentalPriceButton.Click += (o, e) =>
            {
                if (GameScene.SelectedCell != null || GameScene.Gold <= 0)
                    return;

                var amountBox = new MirAmountBox("Rental fee:", 116, GameScene.Gold);

                amountBox.OKButton.Click += (c, a) =>
                {
                    if (amountBox.Amount <= 0)
                        return;

                    GameScene.User.RentalGoldAmount += amountBox.Amount;
                    Network.Enqueue(new C.ItemRentalFee { Amount = GameScene.User.RentalGoldAmount });

                    RefreshInterface();
                };

                amountBox.Show();
                GameScene.PickedUpGold = false;
            };

            _nameLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(30, 8),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };

            _rentalPriceLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(60, 42),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.Left | TextFormatFlags.VerticalCenter,
            };
            _rentalPriceLabel.Click += (o, e) =>
            {
                var clickEventArgs = e as MouseEventArgs;

                if (clickEventArgs == null)
                    return;

                switch (clickEventArgs.Button)
                {
                    case MouseButtons.Left:
                        if (GameScene.SelectedCell != null || GameScene.Gold <= 0)
                            return;

                        var amountBox = new MirAmountBox("Rental fee:", 116, GameScene.Gold);

                        amountBox.OKButton.Click += (c, a) =>
                        {
                            if (amountBox.Amount <= 0)
                                return;

                            GameScene.User.RentalGoldAmount += amountBox.Amount;
                            Network.Enqueue(new C.ItemRentalFee { Amount = GameScene.User.RentalGoldAmount });

                            RefreshInterface();
                        };

                        amountBox.Show();
                        GameScene.PickedUpGold = false;

                        break;
                }
            };
        }

        public void RefreshInterface()
        {
            _nameLabel.Text = GameScene.User.Name;
            _rentalPriceLabel.Text = $"Rental Fee: {GameScene.User.RentalGoldAmount:###,###,##0}";

            GameScene.Scene.GuestItemRentingDialog.RefreshInterface();
            GameScene.Scene.GuestItemRentDialog.RefreshInterface();

            Redraw();
        }

        public void OpenItemRentDialog()
        {
            GameScene.Scene.InventoryDialog.Show();

            Show();
            RefreshInterface();
            GameScene.Scene.GuestItemRentingDialog.Show();
        }

        public void Reset()
        {
            GameScene.User.RentalGoldAmount = 0;
            GameScene.Scene.GuestItemRentDialog.Reset();
            
            RefreshInterface();
            Unlock();
            Hide();
        }

        public void Lock()
        {
            _lockButton.Index = 253;
            _lockButton.Enabled = false;
            _rentalPriceButton.Enabled = false;

            RefreshInterface();
        }

        private void Unlock()
        {
            _lockButton.Index = 250;
            _lockButton.Enabled = true;
            _rentalPriceButton.Enabled = true;
        }

        private static void CancelItemRental()
        {
            Network.Enqueue(new C.CancelItemRental());
        }

        private void Hide()
        {
            Visible = false;
        }

        private void Show()
        {
            Visible = true;
        }
    }

    public sealed class GuestItemRentDialog : MirImageControl
    {
        public string GuestName => _guestName;

        private readonly MirLabel _nameLabel, _rentalPriceLabel;
        private readonly MirButton _lockButton, _rentalPriceButton;
        private string _guestName;
        private uint _guestGold;
        private bool _guestGoldLocked;

        public GuestItemRentDialog()
        {
            Index = 238;
            Library = Libraries.Prguse;
            Movable = true;
            Size = new Size(204, 109);
            Location = new Point(Settings.ScreenWidth - Size.Width - Size.Width / 2, Size.Height + Size.Height / 2);
            Sort = true;

            _lockButton = new MirButton
            {
                Index = 250,
                Location = new Point(22, 76),
                Size = new Size(28, 25),
                Library = Libraries.Prguse,
                Parent = this,
                Enabled = false
            };

            _rentalPriceButton = new MirButton
            {
                Index = 28,
                Location = new Point(18, 46),
                Size = new Size(32, 17),
                Library = Libraries.Prguse,
                Parent = this,
                Enabled = false
            };

            _nameLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(30, 8),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };

            _rentalPriceLabel = new MirLabel
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
            _nameLabel.Text = _guestName;
            _rentalPriceLabel.Text = $"Rental Fee: {_guestGold:###,###,##0}";

            Redraw();
        }

        public void SetGuestName(string name)
        {
            _guestName = name;
        }

        public void SetGuestFee(uint amount)
        {
            _guestGold = amount;
        }

        public void Reset()
        {
            Unlock();
            _guestName = string.Empty;
            _guestGold = 0;

            Hide();
        }

        public void Lock()
        {
            _lockButton.Index = 253;
            _guestGoldLocked = true;

            RefreshInterface();
        }

        private void Unlock()
        {
            _lockButton.Index = 250;
            _guestGoldLocked = false;
        }

        private void Hide()
        {
            Visible = false;
        }

        public void Show()
        {
            Visible = true;
        }
    }
}
