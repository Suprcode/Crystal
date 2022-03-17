using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client.MirScenes;
using Client.MirScenes.Dialogs;
using S = ServerPackets;

namespace Client.MirObjects
{
    public class UserHeroObject : UserObject
    {
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

            GameScene scene = GameScene.Scene;
            scene.HeroDialog = new CharacterDialog(MirGridType.HeroEquipment, Hero) { Parent = scene, Visible = false };
            scene.HeroInventoryDialog = new HeroInventoryDialog { Parent = scene };
            scene.HeroBeltDialog = new HeroBeltDialog { Parent = scene };
            scene.HeroBuffsDialog = new BuffDialog 
            { 
                Parent = scene, 
                Visible = true, 
                Location = new Point(Settings.ScreenWidth - 170, 80),
                GetExpandedParameter = () => { return Settings.ExpandedHeroBuffWindow; },
                SetExpandedParameter = (value) => { Settings.ExpandedHeroBuffWindow = value; }
            };        

            RefreshStats();
        }      
    }
}
