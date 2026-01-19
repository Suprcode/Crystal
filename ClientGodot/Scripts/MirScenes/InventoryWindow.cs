using Godot;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class InventoryWindow : Panel
    {
        // Simple Inventory Window
        // GridContainer for slots

        private ItemCell[] _cells;

        public override void _Ready()
        {
            var grid = GetNode<GridContainer>("GridContainer");
            _cells = new ItemCell[46];

            var cellScene = GD.Load<PackedScene>("res://Scenes/Windows/ItemCell.tscn");

            for(int i = 0; i < 46; i++)
            {
                if (cellScene != null)
                {
                    _cells[i] = cellScene.Instantiate<ItemCell>();
                    grid.AddChild(_cells[i]);
                }
            }
        }

        public void Process()
        {
            // Refresh Inventory from User
            if (GameScene.Scene.User != null)
            {
                for (int i = 0; i < 46; i++)
                {
                    if (i < GameScene.Scene.User.Inventory.Length)
                    {
                        // Check if changed? For now brute force update
                        _cells[i].UpdateItem(GameScene.Scene.User.Inventory[i]);
                    }
                }
            }
        }
    }
}
