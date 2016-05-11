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
    public sealed class TradeDialog : MirImageControl
    {
        public MirItemCell[] Grid;
        public MirLabel NameLabel, GoldLabel;
        public MirButton ConfirmButton, CloseButton;

        public TradeDialog()
        {
            Index = 389;
            Library = Libraries.Prguse;
            Movable = true;
            Size = new Size(204, 152);
            Location = new Point((Settings.ScreenWidth / 2) - Size.Width - 10, Settings.ScreenHeight - 350);
            Sort = true;

            #region Buttons
            ConfirmButton = new MirButton
            {
                Index = 520,
                HoverIndex = 521,
                Location = new Point(135, 120),
                Size = new Size(48, 25),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 522,
                Sound = SoundList.ButtonA,
            };
            ConfirmButton.Click += (o, e) => 
            {
                ChangeLockState(!GameScene.User.TradeLocked);
                Network.Enqueue(new C.TradeConfirm { Locked = GameScene.User.TradeLocked });
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(Size.Width - 23, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) =>
            {
                Hide();
                GameScene.Scene.GuestTradeDialog.Hide();
                TradeCancel();
            };

            #endregion

            #region Host labels
            NameLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(20, 10),
                Size = new Size(150, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };

            GoldLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(35, 123),
                Parent = this,
                Size = new Size(90, 15),
                Sound = SoundList.Gold,
            };
            GoldLabel.Click += (o, e) =>
            {
                if (GameScene.SelectedCell == null && GameScene.Gold > 0)
                {
                    MirAmountBox amountBox = new MirAmountBox("Trade Amount:", 116, GameScene.Gold);

                    amountBox.OKButton.Click += (c, a) =>
                    {
                        if (amountBox.Amount > 0)
                        {
                            GameScene.User.TradeGoldAmount += amountBox.Amount;
                            Network.Enqueue(new C.TradeGold { Amount = amountBox.Amount });

                            RefreshInterface();
                        }
                    };

                    amountBox.Show();
                    GameScene.PickedUpGold = false;
                }
            };
            #endregion

            #region Grids
            Grid = new MirItemCell[5 * 2];

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Grid[2 * x + y] = new MirItemCell
                    {
                        ItemSlot = 2 * x + y,
                        GridType = MirGridType.Trade,
                        Parent = this,
                        Location = new Point(x * 36 + 10 + x, y * 32 + 39 + y),
                    };
                }
            }
            #endregion
        }

        public void ChangeLockState(bool lockState, bool cancelled = false)
        {
            GameScene.User.TradeLocked = lockState;

            if (GameScene.User.TradeLocked)
            {
                ConfirmButton.Index = 521;
            }
            else
            {
                ConfirmButton.Index = 520;
            }

            //if (!cancelled)
            //{
            //    //Send lock info to server
            //    Network.Enqueue(new C.TradeConfirm { Locked = lockState });
            //}
        }

        public void RefreshInterface()
        {
            NameLabel.Text = GameScene.User.Name;
            GoldLabel.Text = GameScene.User.TradeGoldAmount.ToString("###,###,##0");

            GameScene.Scene.GuestTradeDialog.RefreshInterface();

            Redraw();
        }

        public void TradeAccept()
        {
            GameScene.Scene.InventoryDialog.Location = new Point(Settings.ScreenWidth - GameScene.Scene.InventoryDialog.Size.Width, 0);
            GameScene.Scene.InventoryDialog.Show();

            RefreshInterface();

            Show();
            GameScene.Scene.GuestTradeDialog.Show();
        }

        public void TradeReset()
        {
            GameScene.Scene.GuestTradeDialog.TradeReset();

            for (int i = 0; i < GameScene.User.Trade.Length; i++)
                GameScene.User.Trade[i] = null;

            GameScene.User.TradeGoldAmount = 0;
            ChangeLockState(false, true);

            RefreshInterface();

            Hide();
            GameScene.Scene.GuestTradeDialog.Hide();
        }

        public void TradeCancel()
        {
            Network.Enqueue(new C.TradeCancel());
        }

        public void Hide()
        {
            Visible = false;
        }

        public void Show()
        {
            Visible = true;
        }

        public MirItemCell GetCell(ulong id)
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i].Item == null || Grid[i].Item.UniqueID != id) continue;
                return Grid[i];
            }
            return null;
        }
    }
    public sealed class GuestTradeDialog : MirImageControl
    {
        public MirItemCell[] GuestGrid;
        public static UserItem[] GuestItems = new UserItem[10];
        public string GuestName;
        public uint GuestGold;

        public MirLabel GuestNameLabel, GuestGoldLabel;

        public MirButton ConfirmButton;

        public GuestTradeDialog()
        {
            Index = 390;
            Library = Libraries.Prguse;
            Movable = true;
            Size = new Size(204, 152);
            Location = new Point((Settings.ScreenWidth / 2) + 10, Settings.ScreenHeight - 350);
            Sort = true;

            #region Host labels
            GuestNameLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(0, 10),
                Size = new Size(204, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
            };

            GuestGoldLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(35, 123),
                Parent = this,
                Size = new Size(90, 15),
                Sound = SoundList.Gold,
                NotControl = true,
            };
            #endregion

            #region Grids
            GuestGrid = new MirItemCell[5 * 2];

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    GuestGrid[2 * x + y] = new MirItemCell
                    {
                        ItemSlot = 2 * x + y,
                        GridType = MirGridType.GuestTrade,
                        Parent = this,
                        Location = new Point(x * 36 + 10 + x, y * 32 + 39 + y),
                    };
                }
            }
            #endregion
        }

        public void RefreshInterface()
        {
            GuestNameLabel.Text = GuestName;
            GuestGoldLabel.Text = string.Format("{0:###,###,##0}", GuestGold);

            for (int i = 0; i < GuestItems.Length; i++)
            {
                if (GuestItems[i] == null) continue;
                GameScene.Bind(GuestItems[i]);
            }

            Redraw();
        }

        public void TradeReset()
        {
            for (int i = 0; i < GuestItems.Length; i++)
                GuestItems[i] = null;

            GuestName = string.Empty;
            GuestGold = 0;

            Hide();
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
