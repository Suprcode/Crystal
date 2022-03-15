using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client.MirScenes;
using S = ServerPackets;

namespace Client.MirObjects
{
    public class UserHeroObject : UserObject
    {
        public UserHeroObject(uint objectID)
        {
            ObjectID = objectID;
            Stats = new Stats();
            Frames = FrameSet.Player;
        }

        public override void Load(S.UserInformation info)
        {
            Name = info.Name;
            NameColour = info.NameColour;
            Class = info.Class;
            Gender = info.Gender;
            Level = info.Level;
            Hair = info.Hair;

            HP = info.HP;
            MP = info.MP;

            Experience = info.Experience;
            MaxExperience = info.MaxExperience;

            Inventory = info.Inventory;
            Equipment = info.Equipment;

            Magics = info.Magics;
            for (int i = 0; i < Magics.Count; i++)
            {
                Magics[i].CastTime += CMain.Time;
            }           

            BindAllItems();

            RefreshStats();
        }
    }
}
