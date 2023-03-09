using Client.MirControls;
using S = ServerPackets;

namespace Client.MirObjects
{
    public class HeroObject : PlayerObject
    {
        public override ObjectType Race
        {
            get { return ObjectType.Hero; }
        }

        public string OwnerName;
        public MirLabel OwnerLabel;

        public override bool ShouldDrawHealth()
        {
            return false; //OwnerName == GameScene.HeroInfo.Name;
        }

        public HeroObject(uint objectID) : base(objectID)
        {
        }

        public void Load(S.ObjectHero info)
        {
            Load((S.ObjectPlayer)info);
            OwnerName = info.OwnerName;

            if (info.ObjectID == Hero?.ObjectID)
                Hero.CurrentLocation = info.Location;
        }

        public override void CreateLabel()
        {
            base.CreateLabel();

            OwnerLabel = null;
            string ownerText = $"{OwnerName}'s Hero";

            for (int i = 0; i < LabelList.Count; i++)
            {
                if (LabelList[i].Text != ownerText || LabelList[i].ForeColour != NameColour) continue;
                OwnerLabel = LabelList[i];
                break;
            }

            if (OwnerLabel != null && !OwnerLabel.IsDisposed) return;

            OwnerLabel = new MirLabel
            {
                AutoSize = true,
                BackColour = Color.Transparent,
                ForeColour = NameColour,
                OutLine = true,
                OutLineColour = Color.Black,
                Text = ownerText,
            };
            OwnerLabel.Disposing += (o, e) => LabelList.Remove(OwnerLabel);
            LabelList.Add(OwnerLabel);
        }

        public override void DrawName()
        {
            CreateLabel();

            if (NameLabel == null || OwnerLabel == null) return;

            NameLabel.Location = new Point(DisplayRectangle.X + (50 - NameLabel.Size.Width) / 2, DisplayRectangle.Y - (42 - NameLabel.Size.Height / 2) + (Dead ? 35 : 8));
            NameLabel.Draw();

            OwnerLabel.Location = new Point(DisplayRectangle.X + (50 - OwnerLabel.Size.Width) / 2, NameLabel.Location.Y + NameLabel.Size.Height - 1);
            OwnerLabel.Draw();
        }
    }
}
