using System;
using System.Drawing; // For Point
using Godot;
using ClientGodot.Scripts.MirGraphics;
using ClientGodot.Scripts.MirControls;

namespace ClientGodot.Scripts.MirObjects
{
    public class ItemObject : MapObject
    {
        public int ImageIndex;
        public uint Amount;
        public string ItemName;

        public ItemObject(uint objectID)
        {
            ObjectID = objectID;
        }

        public override void Process()
        {
            // Ground items usually static, but maybe simple bobbing?
        }

        public override void DrawOnCanvas(Node2D canvas, Vector2 screenPos)
        {
            if (Libraries.FloorItems == null) return;

            var img = Libraries.FloorItems.GetImage(ImageIndex);
            if (img != null)
            {
                var tex = img.CreateTexture();
                if (tex != null)
                {
                    Vector2 drawPos = screenPos + new Vector2(img.X, img.Y);
                    canvas.DrawTexture(tex, drawPos);
                }
            }

            // Draw Name?
            // Typically DrawName() is separate, but we can do it here simply for now
            // canvas.DrawString(ThemeDB.FallbackFont, screenPos + new Vector2(-20, -20), ItemName);
        }
    }
}
