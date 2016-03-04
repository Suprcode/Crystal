using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class MentorDialog : MirImageControl
    {
        public MirImageControl TitleLabel;
        public MirButton CloseButton, AllowButton, AddButton, RemoveButton;
        public MirLabel MentorNameLabel, MentorLevelLabel, MentorOnlineLabel, StudentNameLabel, StudentLevelLabel, StudentOnlineLabel, MentorLabel, StudentLabel, MenteeEXPLabel;

        public string MentorName;
        public ushort MentorLevel;
        public bool MentorOnline;
        public long MenteeEXP;

        public MentorDialog()
        {
            Index = 170;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Location = Center;


            TitleLabel = new MirImageControl
            {
                Index = 51,
                Library = Libraries.Title,
                Location = new Point(18, 8),
                Parent = this
            };



            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(219, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            AllowButton = new MirButton
            {
                HoverIndex = 115,
                Index = 114,
                Location = new Point(30, 178),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 116,
                Sound = SoundList.ButtonA,
                Hint = "允许/拒绝 师徒请求",
            };
            AllowButton.Click += (o, e) =>
            {
                if (AllowButton.Index == 116)
                {
                    AllowButton.Index = 117;
                    AllowButton.HoverIndex = 118;
                    AllowButton.PressedIndex = 119;
                }
                else
                {
                    AllowButton.Index = 114;
                    AllowButton.HoverIndex = 115;
                    AllowButton.PressedIndex = 116;
                }

                Network.Enqueue(new C.AllowMentor());
            };


            AddButton = new MirButton
            {
                HoverIndex = 214,
                Index = 213,
                Location = new Point(60, 178),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 215,
                Sound = SoundList.ButtonA,
                Hint = "拜师",
            };
            AddButton.Click += (o, e) =>
            {
                if (MentorLevel != 0)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("你已经有一个师傅了.", ChatType.System);
                    return;
                }

                string message = "请输入你要拜师的人名.";

                MirInputBox inputBox = new MirInputBox(message);

                inputBox.OKButton.Click += (o1, e1) =>
                {
                    Network.Enqueue(new C.AddMentor { Name = inputBox.InputTextBox.Text });
                    inputBox.Dispose();
                };

                inputBox.Show();

            };

            RemoveButton = new MirButton
            {
                HoverIndex = 217,
                Index = 216,
                Location = new Point(135, 178),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 218,
                Sound = SoundList.ButtonA,
                Hint = "移除 师傅/徒弟",
            };
            RemoveButton.Click += (o, e) =>
            {
                if (MentorName == "")
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("你没有师徒关系.", ChatType.System);
                    return;
                }

                MirMessageBox messageBox = new MirMessageBox(string.Format("移除师徒关系将会有冷却，你确定?"), MirMessageBoxButtons.YesNo);

                messageBox.YesButton.Click += (oo, ee) => Network.Enqueue(new C.CancelMentor { });
                messageBox.NoButton.Click += (oo, ee) => { messageBox.Dispose(); };

                messageBox.Show();

            };

            MentorNameLabel = new MirLabel
            {
                Location = new Point(20, 58),
                Size = new Size(200, 30),
                BackColour = Color.Empty,
                ForeColour = Color.LightGray,
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 10F),
            };

            MentorLevelLabel = new MirLabel
            {
                Location = new Point(170, 58),
                Size = new Size(200, 30),
                BackColour = Color.Empty,
                ForeColour = Color.LightGray,
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 10F),
            };

            MentorOnlineLabel = new MirLabel
            {
                Location = new Point(125, 58),
                Size = new Size(200, 30),
                BackColour = Color.Empty,
                ForeColour = Color.Green,
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 7F),
                Visible = false,
                Text = "在线",
            };

            StudentNameLabel = new MirLabel
            {
                Location = new Point(20, 112),
                Size = new Size(200, 30),
                BackColour = Color.Empty,
                ForeColour = Color.LightGray,
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 10F),
            };

            StudentLevelLabel = new MirLabel
            {
                Location = new Point(170, 111),
                Size = new Size(200, 30),
                BackColour = Color.Empty,
                ForeColour = Color.LightGray,
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 10F),
            };

            StudentOnlineLabel = new MirLabel
            {
                Location = new Point(125, 112),
                Size = new Size(200, 30),
                BackColour = Color.Empty,
                ForeColour = Color.Green,
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 7F),
                Visible = false,
                Text = "在线",
            };

            MentorLabel = new MirLabel
            {
                Location = new Point(15, 41),
                Size = new Size(200, 30),
                BackColour = Color.Empty,
                ForeColour = Color.DimGray,
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 7F),
                Text = "师傅",
            };

            StudentLabel = new MirLabel
            {
                Location = new Point(15, 94),
                Size = new Size(200, 30),
                BackColour = Color.Empty,
                ForeColour = Color.DimGray,
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 7F),
                Text = "徒弟",
            };

            MenteeEXPLabel = new MirLabel
            {
                Location = new Point(15, 147),
                Size = new Size(200, 30),
                BackColour = Color.Empty,
                ForeColour = Color.DimGray,
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 7F),
            };




        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }
        public void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public void UpdateInterface()
        {
            if (MentorLevel == 0)
            {
                MentorNameLabel.Visible = false;
                MentorLevelLabel.Visible = false;
                MentorOnlineLabel.Visible = false;
                StudentNameLabel.Visible = false;
                StudentLevelLabel.Visible = false;
                StudentOnlineLabel.Visible = false;
                MenteeEXPLabel.Visible = false;
                return;
            }

            MentorNameLabel.Visible = true;
            MentorLevelLabel.Visible = true;
            MentorOnlineLabel.Visible = true;
            StudentNameLabel.Visible = true;
            StudentLevelLabel.Visible = true;
            StudentOnlineLabel.Visible = true;

            if (GameScene.User.Level > MentorLevel)
            {
                MentorNameLabel.Text = GameScene.User.Name;
                MentorLevelLabel.Text = "Lv " + GameScene.User.Level.ToString();
                MentorOnlineLabel.Visible = false;

                StudentNameLabel.Text = MentorName;
                StudentLevelLabel.Text = "Lv " + MentorLevel.ToString();
                if (MentorOnline)
                    StudentOnlineLabel.Visible = true;
                else
                    StudentOnlineLabel.Visible = false;

                MenteeEXPLabel.Visible = true;
                MenteeEXPLabel.Text = "徒弟经验: " + MenteeEXP;
            }
            else
            {
                MentorNameLabel.Text = MentorName;
                MentorLevelLabel.Text = "Lv " + MentorLevel.ToString();
                if (MentorOnline)
                    MentorOnlineLabel.Visible = true;
                else
                    MentorOnlineLabel.Visible = false;

                StudentNameLabel.Text = GameScene.User.Name;
                StudentLevelLabel.Text = "Lv " + GameScene.User.Level.ToString();
                StudentOnlineLabel.Visible = false;
            }
        }

    }
}
