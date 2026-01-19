using System;
using System.Drawing; // For Point
using Godot;
using ClientGodot.Scripts.MirGraphics;
using ClientGodot.Scripts.MirControls;

namespace ClientGodot.Scripts.MirObjects
{
    public class NPCObject : MapObject
    {
        public int ImageIndex;

        public NPCObject(uint objectID)
        {
            ObjectID = objectID;
            CurrentAction = MirAction.Standing;
            Direction = MirDirection.Down;
        }

        public override void Process()
        {
            long now = System.Environment.TickCount64;
            // NPCs usually have simpler animation (e.g. 4 frames stand)
            if (now >= NextFrameTime)
            {
                NextFrameTime = now + 200;
                AnimationCount++;
                if (AnimationCount >= 4) // Standard 4 frames
                    AnimationCount = 0;
            }
        }

        public override void DrawOnCanvas(Node2D canvas, Vector2 screenPos)
        {
            if (Libraries.NPCs == null) return;

            // NPC Image Logic:
            // Base Index = ImageIndex * 60 (or variable?).
            // Standard Mir2 NPC Lib layout is messy.
            // Usually: 0..59 is one NPC? Or 0..X is one NPC.
            // Let's assume standard: ImageIndex (from Packet) is the start index.
            // Packet says: Image (ushort).
            // Actually, in standard files, Image ID maps to an offset.
            // Standard: Start + (Dir * Frames) + Frame.
            // Most NPCs only have Direction 0 (Down).
            // Layout: [Stand 4 frames] [Action?]
            // Let's assume ImageIndex IS the start of the frames for that NPC.
            // So we draw ImageIndex + AnimationCount.

            // Wait, looking at original code (MLibrary.cs in Client context if avail):
            // usually it's just Image * FrameCount?
            // Let's assume the Packet Image ID is the direct start index in the Lib.

            int index = ImageIndex + AnimationCount;

            // Sometimes NPCs have direction. If Direction > 0, we might need offset.
            // But usually packet sends specific image ID.

            var img = Libraries.NPCs.GetImage(index);
            if (img != null)
            {
                var tex = img.CreateTexture();
                if (tex != null)
                {
                    Vector2 drawPos = screenPos + new Vector2(img.X, img.Y);
                    canvas.DrawTexture(tex, drawPos);
                }
            }

            // Name
            // canvas.DrawString(ThemeDB.FallbackFont, screenPos + new Vector2(-20, -60), Name);
        }
    }
}
