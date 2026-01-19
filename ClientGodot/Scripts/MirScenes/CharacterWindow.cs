using Godot;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class CharacterWindow : Panel
    {
        private ItemCell[] _equipCells;
        private Label _nameLabel;

        public override void _Ready()
        {
            _nameLabel = GetNode<Label>("NameLabel");

            _equipCells = new ItemCell[14];
            var grid = GetNode<Control>("EquipGrid"); // Or individual nodes

            // For simplicity, let's look for nodes named "Slot_0", "Slot_1"...
            var cellScene = GD.Load<PackedScene>("res://Scenes/Windows/ItemCell.tscn");

            for(int i = 0; i < 14; i++)
            {
                var placeholder = grid.GetNodeOrNull<Control>($"Slot_{i}");
                if (placeholder != null && cellScene != null)
                {
                    _equipCells[i] = cellScene.Instantiate<ItemCell>();
                    placeholder.AddChild(_equipCells[i]);
                }
            }
        }

        public void Process()
        {
            if (GameScene.Scene.User != null)
            {
                _nameLabel.Text = GameScene.Scene.User.Name;

                for(int i = 0; i < 14; i++)
                {
                    if (_equipCells[i] != null)
                    {
                        _equipCells[i].UpdateItem(GameScene.Scene.User.Equipment[i]);
                    }
                }
            }
        }
    }
}
