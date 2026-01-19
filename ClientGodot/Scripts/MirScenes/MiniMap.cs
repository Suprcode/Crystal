using System;
using System.Drawing; // For Point
using Godot;
using ClientGodot.Scripts.MirGraphics;
using ClientGodot.Scripts.MirControls;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class MiniMap : Control
    {
        private TextureRect _mapTexture;
        private Control _dotContainer;

        public override void _Ready()
        {
            _mapTexture = GetNode<TextureRect>("MapTexture");
            _dotContainer = GetNode<Control>("Dots");
        }

        public void Process()
        {
            if (GameScene.Scene.MapControl == null || GameScene.Scene.User == null) return;

            // Render Dots
            // Clear old dots? Ineffecient.
            // Pool dots? Better.

            foreach(Node child in _dotContainer.GetChildren()) child.QueueFree();

            foreach(var obj in GameScene.Scene.MapControl.MapObjects)
            {
                // Check if in range
                if (Functions.InRange(obj.CurrentLocation, GameScene.Scene.User.CurrentLocation, 20)) // 20 range
                {
                    // Draw dot
                    Godot.Color c = Colors.White;
                    if (obj is ClientGodot.Scripts.MirObjects.MonsterObject) c = Colors.Red;
                    else if (obj is ClientGodot.Scripts.MirObjects.NPCObject) c = Colors.Green;
                    else if (obj is ClientGodot.Scripts.MirObjects.PlayerObject) c = Colors.Yellow;

                    // Create simple rect
                    ColorRect dot = new ColorRect();
                    dot.Color = c;
                    dot.CustomMinimumSize = new Vector2(2, 2);
                    dot.Size = new Vector2(2, 2);

                    // Relative Pos
                    int offX = obj.CurrentLocation.X - GameScene.Scene.User.CurrentLocation.X;
                    int offY = obj.CurrentLocation.Y - GameScene.Scene.User.CurrentLocation.Y;

                    // Center is 64, 64 (assuming 128 size map)
                    dot.Position = new Vector2(64 + offX * 2, 64 + offY * 2);
                    _dotContainer.AddChild(dot);
                }
            }
        }
    }
}
