using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client.MirScenes;
using S = ServerPackets;

namespace Client.MirObjects
{
    public class CameraObject : MapObject
    {
        public CameraObject(uint objectID) : base(objectID)
        {
            GameScene.Camera = this;
        }

        public void Load(Point location)
        {
            CurrentLocation = location;
            GameScene.Scene.MapControl.AddObject(this);
        }

        public override ObjectType Race => ObjectType.Player;

        public override bool Blocking => false;

        public override void Draw()
        {
            
        }

        public override void DrawBehindEffects(bool effectsEnabled)
        {
            
        }

        public override void DrawEffects(bool effectsEnabled)
        {
           
        }

        public override bool MouseOver(Point p)
        {
            return false;
        }

        public override void Process()
        {
            Movement = CurrentLocation;
        }
    }
}
