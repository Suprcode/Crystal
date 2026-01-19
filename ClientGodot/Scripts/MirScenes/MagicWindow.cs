using Godot;
using ClientPackets;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class MagicWindow : Panel
    {
        private VBoxContainer _list;

        public override void _Ready()
        {
            _list = GetNode<VBoxContainer>("ScrollContainer/List");
        }

        public void Process()
        {
            // Simple refresh logic: Clear and Rebuild if count differs?
            // Or mostly static?
            // Let's rebuild for prototype simplicity
            foreach(Node child in _list.GetChildren()) child.QueueFree();

            if (GameScene.Scene.User != null)
            {
                var cellScene = GD.Load<PackedScene>("res://Scenes/Windows/MagicCell.tscn");

                foreach(var mag in GameScene.Scene.User.Magics)
                {
                    if (cellScene != null)
                    {
                        var cell = cellScene.Instantiate<MagicCell>();
                        _list.AddChild(cell);
                        cell.UpdateMagic(mag);
                    }
                }
            }
        }
    }
}
