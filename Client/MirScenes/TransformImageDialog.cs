using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Client.MirNetwork;
using S = ServerPackets;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public class TransformImageDialog : MirImageControl  //stupple
    {
        public MirImageControl Title, ItemBox1, ItemBox2;
        public MirButton TransformButton, CloseButton;
        public MirLabel GoldLabel;
        public MirItemCell[] ItemCells = new MirItemCell[2];
        public static UserItem[] Items = new UserItem[2];
        public static int[] ItemsIdx = new int[2];

        public TransformImageDialog()
        {
            Index = 537;
            Library = Libraries.Prguse;
            Movable = false;
            Sort = true;
            Location = new Point(247, 224);

            TransformButton = new MirButton
            {
                Index = 496,
                HoverIndex = 497,
                PressedIndex = 498,
                Library = Libraries.Title,
                Location = new Point(70, 134),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            TransformButton.Click += (o, e) => Transform_Click();

            CloseButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Location = new Point(170, 1),
                Library = Libraries.Prguse2,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            Title = new MirImageControl
            {
                Index = 44,
                Library = Libraries.Title,
                Location = new Point(30, 5),
                Parent = this,
                NotControl = true,
            };

            ItemBox1 = new MirImageControl
            {
                Index = 989,
                Library = Libraries.Prguse,
                Location = new Point(32, 61),
                Parent = this,
                NotControl = true,
            };

            ItemBox2 = new MirImageControl
            {
                Index = 989,
                Library = Libraries.Prguse,
                Location = new Point(122, 61),
                Parent = this,
                NotControl = true,
            };

            ItemCells[0] = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.Transform,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(32, 61),
                ItemSlot = 0,
            };

            ItemCells[1] = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.Transform,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(122, 61),
                ItemSlot = 1,
            };
        }

        public void Transform_Click()
        {
            if (ItemCells[0].Item != null && ItemCells[1].Item != null && EqualClass() == true)
            {
                Network.Enqueue(new C.Transform { ToUniqueID = ItemCells[0].Item.UniqueID, FromUniqueID = ItemCells[1].Item.UniqueID });
            }
            else
            {
                MirMessageBox messageBox = new MirMessageBox("This item is missing or not such items classified.", MirMessageBoxButtons.OK);
                messageBox.Show();
            }
            Clear();


        }

        public bool EqualClass()
        {
            if (ItemCells[0].Item != null && ItemCells[1].Item != null)
            {
                int toShape = ItemCells[0].Item.Transform.IsHumup ? ItemCells[0].Item.Shape : ItemCells[0].Item.Info.Shape;
                int fromShape = ItemCells[1].Item.Transform.IsHumup ? ItemCells[1].Item.Shape : ItemCells[1].Item.Info.Shape;

                if (toShape / 100 == fromShape / 100)
                    return true;
            }

            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < 2; i++)
            {
                ItemCells[i].Item = null;
                Items[i] = null;
                if (ItemsIdx[i] != -1 && GameScene.Scene.InventoryDialog.Grid[ItemsIdx[i]].Locked == true)
                    GameScene.Scene.InventoryDialog.Grid[ItemsIdx[i]].Locked = false;

                ItemsIdx[i] = -1;
            }
        }

        public void Show()
        {
            Clear();
            if (GameScene.Scene.TransformBackDialog != null)
                GameScene.Scene.TransformBackDialog.Hide();
            if (Visible) return;
            Visible = true;
        }

        public void Hide()
        {
            Clear();
            if (!Visible) return;
            Visible = false;
        }
    }

    public class TransformBackDialog : MirImageControl
    {
        public MirImageControl Title, ItemBox;
        public MirButton TransformButton, CloseButton;
        public MirLabel GoldLabel;
        public MirItemCell ItemCell;
        public static UserItem[] Items = new UserItem[1];
        public static int[] ItemsIdx = new int[1];

        public TransformBackDialog()
        {
            Index = 537;
            Library = Libraries.Prguse;
            Movable = false;
            Sort = true;
            Location = new Point(247, 224);

            TransformButton = new MirButton
            {
                Index = 496,
                HoverIndex = 497,
                PressedIndex = 498,
                Library = Libraries.Title,
                Location = new Point(70, 134),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            TransformButton.Click += (o, e) => Transform_Click();

            CloseButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Location = new Point(170, 1),
                Library = Libraries.Prguse2,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            Title = new MirImageControl
            {
                Index = 44,
                Library = Libraries.Title,
                Location = new Point(30, 5),
                Parent = this,
                NotControl = true,
            };

            ItemBox = new MirImageControl
            {
                Index = 989,
                Library = Libraries.Prguse,
                Location = new Point(75, 61),
                Parent = this,
                NotControl = true,
            };

            ItemCell = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.TransformBack,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(75, 61),
                ItemSlot = 0,
            };
        }

        public void Transform_Click()
        {
            if (ItemCell.Item != null)
            {
                Network.Enqueue(new C.Transform { ToUniqueID = ItemCell.Item.UniqueID, FromUniqueID = ulong.MaxValue });
            }
            else
            {
                MirMessageBox messageBox = new MirMessageBox("No items.", MirMessageBoxButtons.OK);
                messageBox.Show();
            }
            Clear();


        }

        public void Clear()
        {
            ItemCell.Item = null;
            Items[0] = null;
            if (ItemsIdx[0] != -1 && GameScene.Scene.InventoryDialog.Grid[ItemsIdx[0]].Locked == true)
                GameScene.Scene.InventoryDialog.Grid[ItemsIdx[0]].Locked = false;

            ItemsIdx[0] = -1;
        }

        public void Show()
        {
            Clear();
            if (GameScene.Scene.TransformImageDialog != null)
                GameScene.Scene.TransformImageDialog.Hide();
            if (GameScene.Scene.HumupTransformDialog != null)
                GameScene.Scene.HumupTransformDialog.Hide();
            if (Visible) return;
            Visible = true;
        }

        public void Hide()
        {
            Clear();
            if (!Visible) return;
            Visible = false;
        }
    }

    public class HumupTransformDialog : MirImageControl
    {
        public MirImageControl Title;
        public MirButton TransformButton, CloseButton;
        public MirLabel GoldLabel;
        public MirItemCell[] ItemCells = new MirItemCell[6];
        public static UserItem[] Items = new UserItem[6];
        public static int[] ItemsIdx = new int[6];

        public HumupTransformDialog()
        {
            Index = 661;
            Library = Libraries.Prguse;
            Movable = false;
            Sort = true;
            Location = new Point(125, 224);

            TransformButton = new MirButton
            {
                Index = 496,
                HoverIndex = 497,
                PressedIndex = 498,
                Library = Libraries.Title,
                Location = new Point(132, 177),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            TransformButton.Click += (o, e) => Transform_Click();

            CloseButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Location = new Point(286, 5),
                Library = Libraries.Prguse2,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            Title = new MirImageControl
            {
                Index = 44,
                Library = Libraries.Title,
                Location = new Point(20, 5),
                Parent = this,
                NotControl = true,
            };

            for (int i = 0; i < ItemCells.Length; i++)
            {
                ItemCells[i] = new MirItemCell
                {
                    BorderColour = Color.Lime,
                    GridType = MirGridType.HumupTransform,
                    Library = Libraries.Items,
                    Parent = this,
                    Location = new Point(51 + i * 35, 56),
                    ItemSlot = i,
                    Size = new Size(35, 31)
                };
            }
        }

        public void Transform_Click()
        {
            ulong[] uniqueId = new ulong[6];

            for (int i = 0; i < ItemCells.Length; i++)
            {
                if (ItemCells[i].Item == null)
                    uniqueId[i] = ulong.MaxValue;
                else
                    uniqueId[i] = ItemCells[i].Item.UniqueID;
            }

            Network.Enqueue(new C.HumupTransform { UniqueID = uniqueId });

            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < ItemCells.Length; i++)
            {
                ItemCells[i].Item = null;
                Items[i] = null;
                if (ItemsIdx[i] != -1 && GameScene.Scene.InventoryDialog.Grid[ItemsIdx[i]].Locked == true)
                    GameScene.Scene.InventoryDialog.Grid[ItemsIdx[i]].Locked = false;

                ItemsIdx[i] = -1;
            }
        }

        public void Show()
        {
            Clear();
            if (GameScene.Scene.TransformBackDialog != null)
                GameScene.Scene.TransformBackDialog.Hide();
            if (Visible) return;
            Visible = true;
        }

        public void Hide()
        {
            Clear();
            if (!Visible) return;
            Visible = false;
        }
    }
}
