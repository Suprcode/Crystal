using System.Drawing; // For Point, Size if needed (or use Godot.Vector2)
using Godot;

namespace ClientGodot.Scripts.MirObjects
{
    // Simplified MapObject for Godot
    public abstract class MapObject
    {
        public uint ObjectID;
        public string Name = string.Empty;
        public Point CurrentLocation;
        public MirDirection Direction;

        // Visual Node in Godot (Sprite, etc.)
        public Node2D Node;

        public abstract void Process();
        public abstract void Draw();
    }
}
