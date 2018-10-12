using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client.MirNetwork;
using Client.MirScenes;
using S = ServerPackets;
using C = ClientPackets;
using System.IO;

namespace Client.MirObjects
{

    public class ObserverObject : MapObject, ICamera
    {
        public string Name { get; set; }
        public bool LockedOn { get; set; }
        public ObserverObject(uint objectID) : base(objectID)
        {
            FreeMovement();
        }
        public uint LockedID;

        public void Load(S.Observe p, UserObject user = null)
        {
            if (user != null && p.ObserveObjectID == 0)
            {
                CurrentLocation = user.CurrentLocation;
                MapLocation = user.CurrentLocation;
                Name = user.Name;
                LockedID = 0;
                FreeMovement();
            }
            else
            {
                LockedID = p.ObserveObjectID;
            }
            GameScene.Scene.MapControl.AddObject(this);
        }

        public void LockOnObject(uint objectID, bool serverlock = false)
        {
            LockedOn = true;

           // if (!serverlock)
                //Network.Enqueue(new C.ObserveLock { ObjectID = objectID });

            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                MapObject ob = MapControl.Objects[i];
                if (ob.ObjectID != objectID) continue;
                GameScene.Camera = ob as ICamera;
                return;
            }
        }

        public void FreeMovement()
        {
            if (LockedOn)
            {
                LockedOn = false;
                LockedID = 0;
                Network.Enqueue(new C.ObserveLock { ObjectID = 0 });
                CurrentLocation = GameScene.Camera.CurrentLocation;
                Name = GameScene.Camera.Name;
            }
            GameScene.Camera = this;
        }

        public override ObjectType Race => ObjectType.Observer;
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
