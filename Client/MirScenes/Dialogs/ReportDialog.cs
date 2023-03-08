using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;

namespace Client.MirScenes.Dialogs
{
    public sealed class ReportDialog : MirImageControl
    {
        MirDropDownBox ReportType;
        MirButton SendButton, CloseButton;
        MirTextBox MessageArea;

        public ReportDialog()
        {
            Index = 1633;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Location = Center;

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(336, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            ReportType = new MirDropDownBox()
            {
                Parent = this,
                Location = new Point(12, 35),
                Size = new Size(170, 14),
                ForeColour = Color.White,
                Visible = true,
                Enabled = true,
            };
            ReportType.Items.Add("Select Report Type.");
            ReportType.Items.Add("Submit Bug");
            ReportType.Items.Add("Report Player");

            MessageArea = new MirTextBox
            {
                Parent = this,
                Location = new Point(12, 57),
                Size = new Size(330, 150),
                Font = new Font(Settings.FontName, 8F),
            };

            MessageArea.MultiLine();

            SendButton = new MirButton
            {
                Parent = this,
                Library = Libraries.Title,
                Index = 607,
                HoverIndex = 608,
                PressedIndex = 609,
                Sound = SoundList.ButtonA,
                Location = new Point(260, 219)
            };
            SendButton.Click += SendButton_Click;

        }

        void SendButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
