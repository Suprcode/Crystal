using System.Drawing;
using Godot;

namespace ClientGodot.Scripts.MirObjects
{
    public class PlayerObject : MapObject
    {
        public PlayerObject(uint objectID)
        {
            ObjectID = objectID;
        }

        public override void Process()
        {
            // Update animation state, etc.
        }

        public override void Draw()
        {
            // In a custom draw setup, we might draw the sprite here via the MapControl
            // OR we just manage the Godot Node position here.
        }
    }
}
