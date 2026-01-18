using System.Collections.Generic;
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
                    // Create User Object
                    // For now, we just track location in MapControl
                    MapControl.SetUserLocation(userInfo.Location);
                    break;

                case ServerPackets.ObjectWalk walk:
                    // Handle movement (simple update for now)
                    if (User != null && walk.ObjectID == User.ObjectID)
                    {
                        MapControl.SetUserLocation(walk.Location);
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
