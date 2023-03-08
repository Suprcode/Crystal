using Client.MirScenes;
using Client.MirScenes.Dialogs;
using S = ServerPackets;

namespace Client.MirObjects
{
    public class UserHeroObject : UserObject
    {
        public bool AutoPot;
        public uint AutoHPPercent;
        public uint AutoMPPercent;

        public UserItem[] HPItem = new UserItem[1];
        public UserItem[] MPItem = new UserItem[1];
        public override BuffDialog GetBuffDialog => GameScene.Scene.HeroBuffsDialog;
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
        }      
    }
}
