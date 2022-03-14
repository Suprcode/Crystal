using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using Font = System.Drawing.Font;
using C = ClientPackets;
using Effect = Client.MirObjects.Effect;

namespace Client.MirScenes.Dialogs
{
    public sealed class HeroDialog : MirImageControl
    {
        public MirLabel TitleLabel;
        public HeroDialog()
        {
            Index = 73;
            Library = Libraries.Prguse;
            Location = Center;

            TitleLabel = new MirLabel
            {
                Text = "Create Hero",
                Parent = this,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                ForeColour = Color.BurlyWood,
                Location = new Point(274, 6),
                AutoSize = true
            };
        }
    }
}
