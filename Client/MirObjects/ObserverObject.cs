using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client.MirScenes;
using S = ServerPackets;

namespace Client.MirObjects
{
    public class ObserverObject : PlayerObject
    {
        public ObserverObject(uint objectID) : base(objectID)
        {
            GameScene.Camera = this;
        }
    }
}
