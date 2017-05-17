﻿using System.Collections.Generic;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System.Drawing;
using System.Windows.Forms;
using Client.MirNetwork;

using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class LoaningManagementDialog : MirImageControl
    {
        public MirImageControl WindowTitle;
        public MirButton RentItemButton, RentedTabButton, BorrowedTabButton, CloseButton;
        public ItemRow[] Rows = new ItemRow[3];

        private long _lastRequestTime;

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
            RentItemButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.ItemRentalRequest());
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

            // Item Rows

            for (var i = 0; i < Rows.Length; i++)
            {
                Rows[i] = new ItemRow
                {
                    Parent = this,
                    Location = new Point(0, 78 + i * 21),
                    Size = new Size(383, 21)
                };
            }
        }

        public void Hide()
        {
            Visible = false;
        }

        public void Show()
        {
            Visible = true;
            RequestRentedItems();
        }

        public void ReceiveRentedItems(List<ItemRentalInformation> rentedItems)
        {
            for (var i = 0; i < Rows.Length; i++)
            {
                Rows[i].Clear();

                if (rentedItems[i] != null)
                    Rows[i].Update(rentedItems[i].ItemName,
                        rentedItems[i].RentingPlayerName,
                        rentedItems[i].ItemReturnDate.ToString());
            }
        }

        private void RequestRentedItems()
        {
            _lastRequestTime = CMain.Time;
            Network.Enqueue(new ClientPackets.GetRentedItems());
        }

        public sealed class ItemRow : MirControl
        {
            public MirLabel ItemNameLabel, RentingPlayerLabel, ReturnDateLabel;

            private long _index;

            public ItemRow()
            {
                ItemNameLabel = new MirLabel
                {
                    Size = new Size(128, 20),
                    Location = new Point(5, 0),
                    DrawFormat = TextFormatFlags.HorizontalCenter,
                    Parent = this,
                    NotControl = true,
                };

                RentingPlayerLabel = new MirLabel
                {
                    Size = new Size(128, 20),
                    Location = new Point(137, 0),
                    DrawFormat = TextFormatFlags.HorizontalCenter,
                    Parent = this,
                    NotControl = true,
                };

                ReturnDateLabel = new MirLabel
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

                ItemNameLabel.Text = string.Empty;
                RentingPlayerLabel.Text = string.Empty;
                ReturnDateLabel.Text = string.Empty;
            }

            public void Update(string itemName, string rentingPlayerName, string returnDate)
            {
                ItemNameLabel.Text = itemName;
                RentingPlayerLabel.Text = rentingPlayerName;
                ReturnDateLabel.Text = returnDate;

                Visible = true;
            }
        }
    }
}