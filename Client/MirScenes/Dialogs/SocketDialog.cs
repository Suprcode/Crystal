using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;

namespace Client.MirScenes.Dialogs
{
    public sealed class SocketDialog : MirImageControl
    {
        public MirItemCell[] Grid;
        public MirButton CloseButton;

        public SocketDialog()
        {
            Index = 20;
            Library = Libraries.Prguse3;
            Movable = true;
            Sort = true;
            Location = new Point(0, 0);

            Grid = new MirItemCell[6 * 2];

            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    int idx = 6 * y + x;

                    Grid[idx] = new MirItemCell
                    {
                        ItemSlot = idx,
                        GridType = MirGridType.Socket,
                        Library = Libraries.Items,
                        Parent = this,
                        Location = new Point(x * 36 + 23 + x, y * 33 + 15 + y),
                    };
                }
            }

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
            CloseButton.Click += (o, e) => Hide();
        }

        private void BindGrid()
        {
            int count = 0;

            if (GameScene.SelectedItem != null)
            {
                count = GameScene.SelectedItem.Slots.Length;
            }

            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    int idx = 6 * y + x;

                    Grid[idx].Visible = idx < count;
                }
            }
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

        public override void Hide()
        {
            GameScene.SelectedItem = null;
            base.Hide();
        }

        public void Show(MirGridType grid, UserItem item)
        {
            if (item.Slots.Length == 0)
            {
                GameScene.SelectedItem = null;
                Visible = false;
                return;
            }

            GameScene.SelectedItem = item;

            Index = 20 + (GameScene.SelectedItem.Slots.Length - 1);

            BindGrid();

            CloseButton.Location = new Point(Size.Width - 23, 3);

            switch (grid)
            {
                case MirGridType.Inventory:
                    Location = new Point(
                        GameScene.Scene.InventoryDialog.Location.X + ((GameScene.Scene.InventoryDialog.Size.Width - Size.Width) / 2),
                        GameScene.Scene.InventoryDialog.Location.Y + GameScene.Scene.InventoryDialog.Size.Height + 5);
                    break;
                case MirGridType.Equipment:
                    Location = new Point(
                        GameScene.Scene.CharacterDialog.Location.X + ((GameScene.Scene.CharacterDialog.Size.Width - Size.Width) / 2),
                        GameScene.Scene.CharacterDialog.Location.Y + GameScene.Scene.CharacterDialog.Size.Height + 5);
                    break;
            }

            Visible = true;
        }
    }

}
