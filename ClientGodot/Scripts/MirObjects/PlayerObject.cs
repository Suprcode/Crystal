using System;
using System.Drawing; // For Point
using Godot;
using ClientGodot.Scripts.MirGraphics;
using ClientGodot.Scripts.MirControls;

namespace ClientGodot.Scripts.MirObjects
{
    public class PlayerObject : MapObject
    {
        public MirClass Class;
        public MirGender Gender;

        public int Hair;
        public int Weapon;
        public int Armour;

        public MirAction CurrentAction;
        public int FrameIndex;
        public long NextFrameTime;
        public int AnimationCount; // Which frame of animation we are on

        public PlayerObject(uint objectID)
        {
            ObjectID = objectID;
            CurrentAction = MirAction.Standing;
            Direction = MirDirection.Down;
        }

        public override void Process()
        {
            // Simple Animation Loop
            // Stand: 8 directions, usually 4 frames per direction.
            // Interval ~200ms

            long now = System.Environment.TickCount64;
            if (now >= NextFrameTime)
            {
                NextFrameTime = now + 200; // Interval
                AnimationCount++;
                if (AnimationCount >= 4) // Assuming 4 frames for Stand
                {
                    AnimationCount = 0;
                }
            }
        }

        public override void Draw()
        {
            // Calculate Position
            // Assuming MapControl handles world-to-screen conversion in its _Draw or we pass it here.
            // But MapControl._Draw iterates objects.
            // Let's assume MapControl calls this with a position offset or we calculate it.

            // For custom renderer, MapControl has the logic.
            // We need to return the textures to be drawn or draw them immediately if we are inside _Draw context.
            // However, we are in a pure C# class, not a Node (MapControl is the Node).
            // So we need a reference to MapControl to call DrawTexture?
            // Or better: MapControl passes itself or the current canvas item.
        }

        // Helper to draw self onto the canvas
        public void DrawOnCanvas(Node2D canvas, Vector2 screenPos)
        {
            // Draw Body (Armour)
            DrawLayer(canvas, screenPos, Libraries.CArmours, Armour);

            // Draw Head (Hair) - usually drawn after body? Or depends on direction?
            // Standard Mir2: Body first, then Hair, then Weapon (if on top).
            // Direction Up: Weapon might be behind.
            // Simplified: Body -> Hair -> Weapon.

            DrawLayer(canvas, screenPos, Libraries.CHair, Hair);
            DrawLayer(canvas, screenPos, Libraries.CWeapons, Weapon);
        }

        private void DrawLayer(Node2D canvas, Vector2 screenPos, MLibrary[] libraries, int shape)
        {
            if (libraries == null || shape < 0 || shape >= libraries.Length) return;

            MLibrary lib = libraries[shape];
            if (lib == null) return;

            // Calculate Frame Index
            // Standard Mir2 Logic:
            // Base Offset for Action + (Direction * FramesPerDir) + AnimationFrame
            // Standing Action Base: 0.
            // Frames per dir: 8 (usually).

            // Simplified "Standing" logic:
            int frameBase = 0;
            int framesPerDir = 8; // Actually varies by action. Stand is 8? Wait, Stand is 4 frames?
            // In .Lib, usually: [Dir0 Frame0] [Dir0 Frame1] ...

            // Actually: Start + (Dir * FramesPerDir) + Frame

            int index = frameBase + ((int)Direction * framesPerDir) + AnimationCount;

            // Get Image
            var img = lib.GetImage(index);
            if (img != null)
            {
                var tex = img.CreateTexture();
                if (tex != null)
                {
                    // Calculate Draw Position
                    // Texture has X, Y offsets (offsets from center/foot)
                    // Godot coordinates: (0,0) is top-left.
                    // Mir coordinates: X,Y in Lib are offsets from the pivot point.

                    Vector2 drawPos = screenPos + new Vector2(img.X, img.Y);
                    canvas.DrawTexture(tex, drawPos);
                }
            }
        }
    }
}
