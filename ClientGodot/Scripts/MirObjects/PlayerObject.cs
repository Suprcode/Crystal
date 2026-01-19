using System;
using System.Drawing; // For Point
using System.Collections.Generic;
using Godot;
using ClientGodot.Scripts.MirGraphics;
using ClientGodot.Scripts.MirControls;
using ClientGodot.Scripts.Algorithms; // Add
using ClientGodot.Scripts.MirScenes; // Add
using ClientPackets; // Add

namespace ClientGodot.Scripts.MirObjects
{
    public class PlayerObject : MapObject
    {
        public MirClass Class;
        public MirGender Gender;

        public int Hair;
        public int Weapon;
        public int Armour;

        public Queue<MirDirection> MovementQueue = new Queue<MirDirection>();
        public long MoveTime;

        public PlayerObject(uint objectID)
        {
            ObjectID = objectID;
            CurrentAction = MirAction.Standing;
            Direction = MirDirection.Down;
        }

        public void MoveTo(Point target)
        {
            if (GameScene.Scene.MapControl == null) return;
            if (CurrentAction == MirAction.Attack1) return; // Can't move while attacking

            // Re-init pathfinder
            PathFinder.SetMap(GameScene.Scene.MapControl);
            var path = PathFinder.FindPath(CurrentLocation, target);

            if (path != null)
            {
                MovementQueue.Clear();
                foreach(var dir in path)
                    MovementQueue.Enqueue(dir);
            }
        }

        public void Attack(MapObject target, Point location = default)
        {
            if (CurrentAction == MirAction.Attack1 || CurrentAction == MirAction.Walking) return;

            long now = System.Environment.TickCount64;
            // Attack Speed check?

            CurrentAction = MirAction.Attack1;
            AnimationCount = 0;
            NextFrameTime = now + 100; // Attack frame speed
            MoveTime = now + 600; // Global cooldown/Attack duration
            MovementQueue.Clear(); // Stop moving

            // Turn towards target
            if (target != null)
            {
                Direction = ClientGodot.Scripts.MirGraphics.Functions.DirectionFromPoint(CurrentLocation, target.CurrentLocation);
            }
            else if (location != default)
            {
                Direction = ClientGodot.Scripts.MirGraphics.Functions.DirectionFromPoint(CurrentLocation, location);
            }

            // Send Packet
            NetworkManager.Enqueue(new ClientPackets.Attack { Direction = Direction, Spell = Spell.None });
        }

        public override void Process()
        {
            long now = System.Environment.TickCount64;

            // Movement Logic
            if (MovementQueue.Count > 0 && now > MoveTime)
            {
                MirDirection dir = MovementQueue.Dequeue();
                Direction = dir;
                CurrentAction = MirAction.Walking;

                // Optimistic visual update
                // CurrentLocation = Functions.PointMove(CurrentLocation, dir, 1);
                // GameScene.Scene.MapControl.SetUserLocation(CurrentLocation); // Trigger redraw

                // Send Packet
                NetworkManager.Enqueue(new ClientPackets.Walk { Direction = dir });

                MoveTime = now + 500; // Walking speed
            }
            else if (MovementQueue.Count == 0 && now > MoveTime)
            {
                CurrentAction = MirAction.Standing;
            }

            // Simple Animation Loop
            // Standard: Stand(4), Walk(6), Run(6)
            int frameCount = 4;
            int interval = 200;

            if (CurrentAction == MirAction.Walking)
            {
                frameCount = 6;
                interval = 100;
            }
            else if (CurrentAction == MirAction.Attack1)
            {
                frameCount = 6;
                interval = 100;
            }

            if (now >= NextFrameTime)
            {
                NextFrameTime = now + interval;
                AnimationCount++;
                if (AnimationCount >= frameCount)
                {
                    if (CurrentAction == MirAction.Attack1)
                    {
                        CurrentAction = MirAction.Standing; // End attack
                        AnimationCount = 0;
                    }
                    else
                    {
                        AnimationCount = 0;
                    }
                }
            }
        }

        public override void DrawOnCanvas(Node2D canvas, Vector2 screenPos)
        {
            // Apply Walking Offset
            // If Walking, MoveTime is future.
            // t = (MoveTime - now) / Duration.
            // Offset = DirectionVector * CellSize * t.
            // ... Simplified for now: just draw at screenPos (which is cell center).
            // To add smoothing, we need to pass a calculated offset from MapControl or calculate it here.
            // Let's assume screenPos IS the smoothed position passed by MapControl?
            // Currently MapControl passes Center of Screen.
            // We need to shift the sprite if we are "between" cells?
            // Actually, MapControl handles camera. If MapControl camera is locked to User Grid X,Y,
            // then User is always at Center.
            // To see smoothing, the Camera must move smoothly, OR the sprite must move relative to camera.
            // Let's implement visual offset in DrawLayer if needed.

            // Draw Body (Armour)
            DrawLayer(canvas, screenPos, Libraries.CArmours, Armour);

            // Draw Head (Hair)
            DrawLayer(canvas, screenPos, Libraries.CHair, Hair);
            DrawLayer(canvas, screenPos, Libraries.CWeapons, Weapon);
        }

        private void DrawLayer(Node2D canvas, Vector2 screenPos, MLibrary[] libraries, int shape)
        {
            if (libraries == null || shape < 0 || shape >= libraries.Length) return;

            MLibrary lib = libraries[shape];
            if (lib == null) return;

            // Action Offsets (Simplified Standard Mir2)
            // Stand: 0
            // Walk: 64 (8 dirs * 8 frames?) -> Actually Walk is often Action 1.
            // Let's assume standard sequence in Lib:
            // [Stand (8*4)] [Walk (8*6)] [Run (8*6)] ...

            int frameBase = 0;
            int framesPerDir = 8; // Default stride

            if (CurrentAction == MirAction.Walking)
            {
                frameBase = 64;
            }
            else if (CurrentAction == MirAction.Attack1)
            {
                frameBase = 128; // Action 2 * 64? Or usually Attack is action 2 or 3.
            }

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
