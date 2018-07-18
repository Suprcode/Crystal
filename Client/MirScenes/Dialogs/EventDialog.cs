using Client.MirControls;
using Client.MirGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.MirScenes.Dialogs
{
    public class EventDialog : MirImageControl
    {
        public MirLabel EventName, ObjectiveMessage;
        //MirImageControl _progress;
        int completedPercentage = 0;
        public List<MirLabel> MonsterObjectives = new List<MirLabel>();
        public List<MirLabel> ObjectiveMessages = new List<MirLabel>();

        public Font QuestFont = new Font(Settings.FontName, 8F);

        public EventDialog()
        {
            Movable = true;
            Sort = false;

            EventName = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.LimeGreen,
                Location = new Point(40, 60),
                Parent = this,
                NotControl = true,
            };
            Location = new Point(Settings.ScreenWidth - 200, 200);

        }
        public List<string> GetLines(string objective, int lineMaxLength)
        {
            List<string> lines = new List<string>();

            if (objective.Length > lineMaxLength)
            {
                var words = objective.Split(' ');

                if (words.Length > 1)
                {
                    string constructedString = string.Empty;
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (constructedString.Length + words[i].Length >= lineMaxLength)
                        {
                            lines.Add(constructedString);
                            constructedString = words[i];
                        }
                        else
                        {
                            if (constructedString.Length == 0)
                                constructedString = words[i];
                            else
                                constructedString = string.Format("{0} {1}", constructedString, words[i]);

                            if (constructedString.Length >= lineMaxLength || i == words.Length - 1)
                            {
                                lines.Add(constructedString);
                                constructedString = string.Empty;
                            }
                        }
                    }
                }
                return lines;
            }
            else
                return new List<string>() { objective };
        }

        public void UpdateDialog(string eventName, string objectiveMsg, List<MonsterEventObjective> objectives, int stage)
        {
            foreach (MirLabel label in MonsterObjectives)
                label.Dispose();

            foreach (MirLabel label in ObjectiveMessages)
                label.Dispose();

            ObjectiveMessages.Clear();
            MonsterObjectives.Clear();

            if (!string.IsNullOrEmpty(eventName))
            {
                EventName.Text = eventName;
            }
            int increment = 0;

            if (stage > 0)
            {
                ObjectiveMessages.Add(new MirLabel
                {
                    AutoSize = true,
                    OutLine = true,
                    Parent = this,
                    Text = string.Format("Invasion Stage: {0}", stage),
                    Visible = true,
                    ForeColour = Color.GreenYellow,
                    Location = new Point(40, 80 + increment)
                });
                increment += 15;
            }


            if (!string.IsNullOrEmpty(objectiveMsg))
            {
                int msgWidth = Settings.Resolution > 800 ? 30 : 17;
                List<string> msgs = GetLines(objectiveMsg, msgWidth);

                for (int i = 0; i < msgs.Count; i++)
                {
                    ObjectiveMessages.Add(new MirLabel
                    {
                        AutoSize = true,
                        OutLine = true,
                        Parent = this,
                        Text = msgs[i],
                        Visible = true,
                        ForeColour = Color.LightYellow,
                        Location = new Point(40, 80 + increment)
                    });
                    increment += 15;
                }
            }


            foreach (var monObj in objectives)
            {
                MirLabel lblMon = new MirLabel
                {
                    Text = string.Format("{0} : {1}/{2}", monObj.MonsterName, monObj.MonsterTotalCount - monObj.MonsterAliveCount, monObj.MonsterTotalCount),
                    AutoSize = true,
                    Font = QuestFont,
                    ForeColour = Color.YellowGreen,
                    Location = new Point(40, 80 + increment),
                    OutLine = true,
                    Parent = this,
                    Visible = true,
                };
                increment += 15;

                MonsterObjectives.Add(lblMon);
            }
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