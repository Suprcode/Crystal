using Client.MirControls;
using Client.MirGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MirScenes.Dialogs
{
    public class EventDialog : MirImageControl
    {
        public MirLabel EventName, ObjectiveTitle, Objective, Percentage, RemainingCount, CompletionPercentage;
        MirImageControl _progress;
        int completedPercentage = 0;

        public EventDialog()
        {
            Index = 14;
            Library = Libraries.Prguse3;
            Sort = true;
            Location = new Point(Settings.ScreenWidth - 260, 300);
            DrawImage = false;
            Visible = false;
            Movable = true;

            this.BeforeDraw += (o, e) =>
            {
                Libraries.Prguse3.Draw(13, this.Location.X, this.Location.Y);
                Libraries.Prguse3.Draw(14, this.Location.X, this.Location.Y);

                Libraries.Prguse3.Draw(15, new Rectangle(0, 0, (int)(completedPercentage * 2), 20), new Point(this.Location.X + 10, this.Location.Y + 184), Color.White, false);


            };

            EventName = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.Yellow,
                Location = new Point(20, 40),
                Parent = this,
                NotControl = true,
            };
            ObjectiveTitle = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.Yellow,
                Location = new Point(20, 80),
                Parent = this,
                NotControl = true,
                Text = "Objective",
            };
            Objective = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Location = new Point(20, 100),
                Parent = this,
                NotControl = true,
            };
            RemainingCount = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Location = new Point(20, 140),
                Parent = this,
                NotControl = true,
            };
            CompletionPercentage = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Location = new Point(20, 160),
                Parent = this,
                NotControl = true,
            };
            //_progress = new MirImageControl()
            //{
            //    Index = 687,
            //    Library = Libraries.Prguse3,
            //    Parent = this,
            //    Size = new Size(200,20),
            //    DrawImage = true,

            //    Location = new Point(10, 182),
            //};
        }

        public void UpdateDialog(string eventName, string objective, int percentage, string remainingCount, int stage)
        {
            if (!string.IsNullOrEmpty(eventName))
                EventName.Text = eventName;


            var splittedRemaining = remainingCount.Split('/');
            var aliveMonsters = splittedRemaining[0];
            var totalMonsters = splittedRemaining[1];

            RemainingCount.Text = string.Format("Monsters:{0}", aliveMonsters);

            if (stage > 0)
            {
                if (!string.IsNullOrEmpty(objective))
                    Objective.Text = string.Format("Stage {0}:{1}", stage, objective);
            }
            else
            {
                if (!string.IsNullOrEmpty(objective))
                    Objective.Text = objective;
            }

            CompletionPercentage.Text = string.Format("Completed Percentage: {0}%", percentage);

            //_progress.Size = new Size(percentage * 2, 182);
            completedPercentage = percentage;

        }
        public void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }
    }
}
