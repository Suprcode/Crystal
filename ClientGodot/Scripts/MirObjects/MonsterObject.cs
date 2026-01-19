using System;
using System.Drawing; // For Point
using Godot;
using ClientGodot.Scripts.MirGraphics;
using ClientGodot.Scripts.MirControls;

namespace ClientGodot.Scripts.MirObjects
{
    public class MonsterObject : MapObject
    {
        public Monster Image;

        public MonsterObject(uint objectID)
        {
            ObjectID = objectID;
            CurrentAction = MirAction.Standing;
            Direction = MirDirection.Down;
        }

        public override void Process()
        {
            long now = System.Environment.TickCount64;

            // Simple Animation Loop (Similar to Player)
            // Monsters usually have 10 frames per action/direction?
            // Depends on Monster.Lib format.
            // Standard: Stand(4), Walk(6), Attack(6), Die(10)...

            int frameCount = 4;
            if (CurrentAction == MirAction.Walking) frameCount = 6;

            int interval = 200;

            if (now >= NextFrameTime)
            {
                NextFrameTime = now + interval;
                AnimationCount++;
                if (AnimationCount >= frameCount)
                {
                    AnimationCount = 0;
                }
            }
        }

        public override void DrawOnCanvas(Node2D canvas, Vector2 screenPos)
        {
            // Calculate Frame Index for Monster Lib
            // Monsters are stored in separate files (Monsters/000.lib, 001.lib...) or one big library?
            // In C# client: Libraries.Monsters is an array of MLibrary.
            // Image (enum) maps to specific index.
            // Usually: (int)Image defines the file index?
            // Or (int)Image is the base offset?

            // Standard Crystal/Mir2:
            // Monster.Image is an enum.
            // We need to map Monster Enum to Library File + Offset.
            // Ideally we'd have a MonsterInfo DB.
            // For now, let's assume Image ID maps to Library Index directly for simplicity,
            // OR use a safe fallback (Lib 0).

            int monsterImage = (int)Image;
            // Let's assume (int)Image corresponds to the index in Libraries.Monsters array.

            if (Libraries.Monsters == null || monsterImage < 0 || monsterImage >= Libraries.Monsters.Length) return;

            MLibrary lib = Libraries.Monsters[monsterImage];
            if (lib == null) return;

            // Frame Index Logic
            // Standard Monster Frame Layout:
            // [Stand (8*4)] [Walk (8*6)] [Attack (8*6)] [Struck (8*2)] [Die (8*10)]
            // Offsets:
            // Stand: 0
            // Walk: 64 (usually 8 dirs * 8 frames stride, even if only 6 used)
            // Attack: 128
            // Struck: 192
            // Die: 256

            int frameBase = 0;
            if (CurrentAction == MirAction.Walking) frameBase = 64;
            else if (CurrentAction == MirAction.Attack1) frameBase = 128; // MirAction usually has Attack1, Attack2...
            else if (CurrentAction == MirAction.Struck) frameBase = 192;
            else if (CurrentAction == MirAction.Die) frameBase = 256;

            int index = frameBase + ((int)Direction * 8) + AnimationCount; // Assuming 8 stride

            var img = lib.GetImage(index);
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
