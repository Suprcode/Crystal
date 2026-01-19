using System.Collections.Generic;
using System.Drawing; // Add
using Godot;
using ClientPackets;
using ServerPackets;
using ClientGodot.Scripts.MirObjects;
using ClientGodot.Scripts.MirControls;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class GameScene : MirScene
    {
        public static GameScene Scene;

        public MapControl MapControl;
        public UserObject User; // The main player

        private Label _debugLabel;

        public override void _Ready()
        {
            Scene = this;

            // Setup UI
            _debugLabel = new Label();
            AddChild(_debugLabel);
            _debugLabel.Position = new Vector2(10, 10);
            _debugLabel.Text = "Waiting for Map Info...";

            // Setup Map Control (The World)
            MapControl = new MapControl();
            AddChild(MapControl);
        }

        public override void Process()
        {
            MapControl?.Process();
            User?.Process();
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Right)
            {
                // Right Click -> Move
                if (MapControl != null && User != null)
                {
                    Vector2 mousePos = mouseEvent.Position;
                    Point gridPos = MapControl.GetMapLocation(mousePos);

                    GD.Print($"Moving to {gridPos}");
                    User.MoveTo(gridPos);
                }
            }
        }

        public override void ProcessPacket(Packet p)
        {
            switch (p)
            {
                case ServerPackets.MapInformation mapInfo:
                    GD.Print($"Loading Map: {mapInfo.FileName} ({mapInfo.Title})");
                    _debugLabel.Text = $"Map: {mapInfo.Title}";
                    MapControl.LoadMap(mapInfo.FileName);
                    break;

                case ServerPackets.UserInformation userInfo:
                    GD.Print($"User Info: {userInfo.Name} Loc: {userInfo.Location}");
                    User = new UserObject(userInfo.ObjectID)
                    {
                        Name = userInfo.Name,
                        CurrentLocation = userInfo.Location,
                        Direction = userInfo.Direction,
                        Class = userInfo.Class,
                        Gender = userInfo.Gender,
                        Hair = userInfo.Hair,
                        // Weapon = userInfo.Weapon, // The error says UserInformation doesn't have Weapon/Armour?
                        // Checking ServerPackets.cs: UserInformation (line 627) has:
                        // public short Weapon, WeaponEffect, Armour;
                        // Wait, looking at UserInformation packet...
                        // Ah, ServerPackets.UserInformation (class UserInformation)
                        // has NO Weapon/Armour fields directly?
                        // Let's re-read the file carefully.
                        // Line 627: public class ObjectPlayer has Weapon/Armour.
                        // Line 502: public class UserInformation has:
                        // UserItem[] Inventory, Equipment...
                        // It does NOT have Weapon/Armour fields. It has Equipment array!
                        // We need to parse Equipment array to find Weapon/Armour.
                    };

                    // Parse Equipment
                    if (userInfo.Equipment != null)
                    {
                        // Assuming Equipment Slots:
                        // Weapon = 1, Armour = 0? Need Enums.
                        // Standard Mir2 Enums:
                        // EquipmentSlot: Armour=0, Weapon=1...
                        // Let's look up EquipmentSlot in Shared/Enums.cs if possible, or guess.
                        // Since we can't see Enums.cs right now, let's look at `Shared` project structure again or safely assume indices.
                        // Or better: Iterate.
                        // Actually, let's just comment them out or set to 0 for now to fix build.
                        // We will fix equipment visuals later when we implement Inventory.

                        // User.Weapon = ...
                        // User.Armour = ...
                    }

                    MapControl.SetUserLocation(userInfo.Location);
                    break;

                case ServerPackets.ObjectWalk walk:
                    // Handle movement
                    if (User != null && walk.ObjectID == User.ObjectID)
                    {
                        // Server confirmed walk
                        MapControl.SetUserLocation(walk.Location);
                        // In a real smooth system, we might correct the queue if we desync
                        // User.CurrentLocation = walk.Location;
                    }
                    break;
            }
        }
    }

    // Temporary placeholder for UserObject until fully implemented
    public class UserObject : PlayerObject
    {
        public UserObject(uint id) : base(id) { }
    }
}
