using Godot;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class InventoryWindow : Panel
    {
        // Simple Inventory Window
        // GridContainer for slots

        public override void _Ready()
        {
            // Placeholder: Create 46 slots (Standard Mir2 Inventory)
            var grid = GetNode<GridContainer>("GridContainer");
            for(int i = 0; i < 46; i++)
            {
                var slot = new Panel();
                slot.CustomMinimumSize = new Vector2(36, 36);
                grid.AddChild(slot);
            }
        }
    }
}
