using System;
using System.Collections.Generic;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Client.MirNetwork;

using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class ItemRentalDialog : MirImageControl
    {
        private readonly ItemRow[] _itemRows = new ItemRow[3];
        private DateTime _lastRequestTime = DateTime.Now;

        public ItemRentalDialog()
        {
            Index = 1;
            Library = Libraries.Prguse3;
            Movable = true;
            Size = new Size(400, 174);
            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);
            Sort = true;

            // Title

            var windowTitle = new MirImageControl
            {
                Index = 0,
                Library = Libraries.Prguse3,
                Location = new Point(22, 8),
                Parent = this
            };

            // Rented Tab

            var rentedTabButton = new MirButton
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

            var borrowedTabButton = new MirButton
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

            var rentItemButton = new MirButton
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
            rentItemButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.ItemRentalRequest());
            };

            // Close Button

            var closeButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(375, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            closeButton.Click += (o, e) => Toggle();

            // Item Rows

            for (var i = 0; i < _itemRows.Length; i++)
            {
                _itemRows[i] = new ItemRow
                {
                    Parent = this,
                    Location = new Point(0, 78 + i * 21),
                    Size = new Size(383, 21)
                };
            }
        }

        public void Toggle()
        {
            Visible = !Visible;

            if (Visible)
                RequestRentedItems();
        }
        
        public void ReceiveRentedItems(List<ItemRentalInformation> rentedItems)
        {
            for (var i = 0; i < _itemRows.Length; i++)
            {
                _itemRows[i].Clear();

                if (i < rentedItems.Count && rentedItems[i] != null)
                    _itemRows[i].Update(rentedItems[i].ItemName,
                        rentedItems[i].RentingPlayerName,
                        rentedItems[i].ItemReturnDate.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void RequestRentedItems()
        {
            if (_lastRequestTime > CMain.Now)
                return;
            
            _lastRequestTime = CMain.Now.AddSeconds(60);
            Network.Enqueue(new ClientPackets.GetRentedItems());
        }

        private sealed class ItemRow : MirControl
        {
            private readonly MirLabel _itemNameLabel, _rentingPlayerLabel, _returnDateLabel;

            public ItemRow()
            {
                _itemNameLabel = new MirLabel
                {
                    Size = new Size(128, 20),
                    Location = new Point(5, 0),
                    DrawFormat = TextFormatFlags.HorizontalCenter,
                    Parent = this,
                    NotControl = true,
                };

                _rentingPlayerLabel = new MirLabel
                {
                    Size = new Size(128, 20),
                    Location = new Point(137, 0),
                    DrawFormat = TextFormatFlags.HorizontalCenter,
                    Parent = this,
                    NotControl = true,
                };

                _returnDateLabel = new MirLabel
                {
                    Size = new Size(128, 20),
                    Location = new Point(264, 0),
                    DrawFormat = TextFormatFlags.HorizontalCenter,
                    Parent = this,
                    NotControl = true,
                };
            }

            public void Clear()
            {
                Visible = false;

                _itemNameLabel.Text = string.Empty;
                _rentingPlayerLabel.Text = string.Empty;
                _returnDateLabel.Text = string.Empty;
            }

            public void Update(string itemName, string rentingPlayerName, string returnDate)
            {
                _itemNameLabel.Text = itemName;
                _rentingPlayerLabel.Text = rentingPlayerName;
                _returnDateLabel.Text = returnDate;

                Visible = true;
            }
        }
    }
}
