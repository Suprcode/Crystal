using System;
using System.Drawing; // For Point
using Godot;
using ClientGodot.Scripts.MirGraphics;
using ClientGodot.Scripts.MirControls;

namespace ClientGodot.Scripts.MirObjects
{
    public class SpellObject : MapObject
    {
        public SpellEffect Effect;
        public uint EffectType;

        public SpellObject(uint objectID)
        {
            ObjectID = objectID;
        }

        public override void Process()
        {
            long now = System.Environment.TickCount64;
            if (now >= NextFrameTime)
            {
                NextFrameTime = now + 100; // Fast effect
                AnimationCount++;
                if (AnimationCount >= 10) // Approx frame count
                {
                    // Loop or expire?
                    // Server usually sends RemoveObject or we self-expire.
                    // For now loop.
                    AnimationCount = 0;
                }
            }
        }

        public override void DrawOnCanvas(Node2D canvas, Vector2 screenPos)
        {
            if (Libraries.Effect == null) return;

            // Mapping Effect Enum to Lib Index is complex.
            // Simplified: EffectType is the start index?
            // Or SpellEffect enum maps to ranges.

            // Let's assume EffectType IS the image index for now.
            int index = (int)EffectType + AnimationCount;

            var img = Libraries.Effect.GetImage(index);
            if (img != null)
            {
                var tex = img.CreateTexture();
                if (tex != null)
                {
                    Vector2 drawPos = screenPos + new Vector2(img.X, img.Y);
                    canvas.DrawTexture(tex, drawPos);
                }
            }
        }
    }
}
