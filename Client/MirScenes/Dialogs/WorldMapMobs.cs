using Client.MirControls;
using Client.MirGraphics;
using Client.MirObjects;
using Client.MirSounds;
using System;
using System.Drawing;
using System.Windows.Forms;
using C = ClientPackets;
using Client.MirNetwork;

namespace Client.MirScenes.Dialogs
{
	public sealed class WorldMapMobs : MirImageControl
	{
		public static UserObject User
		{
			get
			{
				return MapObject.User;
			}
			set
			{
				MapObject.User = value;
			}
		}


		public MirButton closeButton, teleButton, teleButton1, teleButton2, teleButton3, teleButton4, teleButton5, teleButton6, teleButton7, teleButton8, teleButton9, teleButton10, nextButton, backButton;

		public MirLabel titleLabel, dungeonNameLabel, informationLabel, suggestedLevelLabel, suggestedLevel, questdropLabel, dropLabel, drop01, drop02, drop03, drop04, drop05, drop06, drop07, drop08, drop09, drop10, drop11, informationArea, pageLabel;

		public MirAnimatedControl flameBar, iceBar;

		public MirAnimatedButton monster01, monster02, monster03, monster04, monster05, monster06, monster07, monster08, monster09, monster10, monster11, monster12, monster13;
		public MirAnimatedButton monster14, monster15, monster16, monster17, monster18, monster19, monster01s, monster02s, monster03s, monster05s, monster06s, monster07s, monster09s;

		public WorldMapMobs()
		{
			Index = 29;
			Library = Libraries.WorldMap;
			Movable = true;
			Location = new Point(Settings.ScreenWidth / 2 - Size.Width / 2, Settings.ScreenHeight / 4 - Size.Height / 4);

			flameBar = new MirAnimatedControl
			{
				Size = new Size(450, 100),
				Parent = this,
				Animated = true,
				AnimationCount = 8,
				AnimationDelay = 180L,
				Index = 34,
				Library = Libraries.WorldMap,
				Location = new Point(96, 5),
				Visible = true,
				Loop = true,
				Blending = true,
				BlendingRate = 1f,
				NotControl = true,
				UseOffSet = true
			};

			iceBar = new MirAnimatedControl
			{
				Size = new Size(450, 100),
				Parent = this,
				Animated = true,
				AnimationCount = 5,
				AnimationDelay = 180L,
				Index = 42,
				Library = Libraries.WorldMap,
				Location = new Point(210, 396),
				Visible = true,
				Loop = true,
				Blending = true,
				BlendingRate = 1f,
				NotControl = true,
				UseOffSet = true
			};

			backButton = new MirButton
			{
				Index = 240,
				HoverIndex = 241,
				PressedIndex = 242,
				Library = Libraries.Prguse2,
				Location = new Point(294, 360),
				Sound = SoundList.ButtonA,
				Parent = this,
				Visible = true,
				Hint = "Back Page"
			};

			nextButton = new MirButton
			{
				Index = 243,
				HoverIndex = 244,
				PressedIndex = 245,
				Library = Libraries.Prguse2,
				Location = new Point(360, 360),
				Sound = SoundList.ButtonA,
				Parent = this,
				Visible = true,
				Hint = "Next Page"
			};

			pageLabel = new MirLabel
			{
				Size = new Size(70, 18),
				Location = new Point(300, 360),
				DrawFormat = TextFormatFlags.HorizontalCenter,
				Text = "0/0",
				NotControl = true,
				Parent = this,
				Visible = true
			};

			closeButton = new MirButton
			{
				Index = 361,
				HoverIndex = 362,
				PressedIndex = 363,
				Location = new Point(625, 9),
				Library = Libraries.Prguse,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Close"
			};
			closeButton.Click += delegate (object o, EventArgs e)
			{
				Hide();
			};

			#region Labels
			titleLabel = new MirLabel
			{
				Font = new Font(Settings.FontName, 10f, FontStyle.Bold),
				ForeColour = Color.NavajoWhite,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "World Mobs",
				Size = new Size(80, 22),
				Location = new Point(286, 8),
				Parent = this,
				NotControl = true
			};

			dungeonNameLabel = new MirLabel
			{
				Font = new Font(Settings.FontName, 16f, FontStyle.Bold | FontStyle.Italic),
				ForeColour = Color.Orange,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "",
				Size = new Size(560, 24),
				Location = new Point(45, 42),
				Parent = this,
				NotControl = true
			};

			informationLabel = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f, FontStyle.Bold),
				ForeColour = Color.NavajoWhite,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Location Information:",
				Size = new Size(117, 23),
				Location = new Point(216, 230),
				Parent = this,
				NotControl = true
			};
			suggestedLevelLabel = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f, FontStyle.Bold),
				ForeColour = Color.NavajoWhite,
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Text = "Suggested Level:",
				Size = new Size(90, 23),
				Location = new Point(340, 229),
				Parent = this,
				NotControl = true
			};
			suggestedLevel = new MirLabel
			{
				Font = new Font(Settings.FontName, 12f, FontStyle.Bold),
				ForeColour = Color.Red,
				DrawFormat = TextFormatFlags.VerticalCenter,
				Text = "",
				Size = new Size(50, 23),
				Location = new Point(426, 227),
				Parent = this,
				NotControl = true
			};
			questdropLabel = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f, FontStyle.Bold),
				ForeColour = Color.NavajoWhite,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Quest Drops",
				Size = new Size(90, 23),
				Location = new Point(490, 226),
				Parent = this,
				NotControl = true,
				Visible = false
			};

			dropLabel = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f, FontStyle.Bold),
				ForeColour = Color.NavajoWhite,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Cave Drops",
				Size = new Size(90, 23),
				Location = new Point(490, 232),
				Parent = this,
				NotControl = true,
				Visible = false
			};

			drop01 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 253),
				Parent = this,
				NotControl = true
			};

			drop02 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 267),
				Parent = this,
				NotControl = true
			};

			drop03 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 280),
				Parent = this,
				NotControl = true
			};

			drop04 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 293),
				Parent = this,
				NotControl = true
			};

			drop05 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 306),
				Parent = this,
				NotControl = true
			};

			drop06 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 253),
				Parent = this,
				NotControl = true
			};

			drop07 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 267),
				Parent = this,
				NotControl = true
			};
			drop08 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 280),
				Parent = this,
				NotControl = true
			};
			drop09 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 293),
				Parent = this,
				NotControl = true
			};
			drop10 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 306),
				Parent = this,
				NotControl = true
			};
			drop11 = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f),
				ForeColour = Color.Silver,
				DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter),
				Text = "Drop",
				Size = new Size(111, 16),
				Location = new Point(482, 319),
				Parent = this,
				NotControl = true
			};
			informationArea = new MirLabel
			{
				Font = new Font(Settings.FontName, 7f, FontStyle.Bold),
				ForeColour = Color.Silver,
				DrawFormat = TextFormatFlags.WordBreak,
				Text = "",
				Size = new Size(229, 100),
				Location = new Point(219, 255),
				Parent = this,
				NotControl = true
			};
			#endregion

			#region Mobs
			monster01 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 50,
				Library = Libraries.Monsters[0],
				Location = new Point(50, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster02 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 50,
				Library = Libraries.Monsters[0],
				Location = new Point(130, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster03 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 50,
				Library = Libraries.Monsters[0],
				Location = new Point(210, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster04 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 56,
				Library = Libraries.Monsters[0],
				Location = new Point(290, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster05 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 62,
				Library = Libraries.Monsters[0],
				Location = new Point(380, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster06 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 62,
				Library = Libraries.Monsters[0],
				Location = new Point(460, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster07 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 62,
				Library = Libraries.Monsters[0],
				Location = new Point(540, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster08 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 200L,
				Index = 50,
				Library = Libraries.Monsters[0],
				Location = new Point(50, 320),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster09 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 200L,
				Index = 62,
				Library = Libraries.Monsters[0],
				Location = new Point(130, 320),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster10 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 200L,
				Index = 62,
				Library = Libraries.Monsters[0],
				Location = new Point(130, 320),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster11 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 14,
				AnimationDelay = 250L,
				Index = 22,
				Library = Libraries.Monsters[0],
				Location = new Point(90, 320),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster12 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 83,
				Library = Libraries.Monsters[0],
				Location = new Point(130, 320),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster13 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 0,
				Library = Libraries.Monsters[0],
				Location = new Point(290, 196),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			#endregion

			#region GonRyun Mobs
			monster14 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 200L,
				Index = 50,
				Library = Libraries.Monsters[0],
				Location = new Point(120, 330),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster15 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 50,
				Library = Libraries.Monsters[0],
				Location = new Point(380, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster16 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 50,
				Library = Libraries.Monsters[0],
				Location = new Point(460, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster17 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 50,
				Library = Libraries.Monsters[0],
				Location = new Point(540, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster18 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 40,
				Library = Libraries.Monsters[0],
				Location = new Point(290, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			#endregion

			#region Pb Mobs
			monster19 = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 4,
				AnimationDelay = 250L,
				Index = 12,
				Library = Libraries.Monsters[0],
				Location = new Point(210, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};
			#endregion

			#region Snow Cove Mobs
			monster01s = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 62,
				Library = Libraries.Monsters[0],
				Location = new Point(50, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster02s = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 62,
				Library = Libraries.Monsters[0],
				Location = new Point(130, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster03s = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 62,
				Library = Libraries.Monsters[0],
				Location = new Point(210, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster05s = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 35,
				Library = Libraries.Monsters[0],
				Location = new Point(356, 170),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster06s = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 30,
				Library = Libraries.Monsters[0],
				Location = new Point(460, 194),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster07s = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 250L,
				Index = 62,
				Library = Libraries.Monsters[0],
				Location = new Point(540, 192),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			monster09s = new MirAnimatedButton
			{
				Size = new Size(100, 200),
				Parent = this,
				Animated = true,
				AnimationCount = 6,
				AnimationDelay = 200L,
				Index = 50,
				Library = Libraries.Monsters[0],
				Location = new Point(130, 320),
				Visible = true,
				Loop = true,
				NotControl = false,
				UseOffSet = true
			};

			#endregion

		}
		// Mob links
		public void OmaCave()
		{
			dungeonNameLabel.Text = "Oma Cave's";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "7+";
			informationArea.Text = "Oma Cave is located in the north western mountain range of Bichon Province.\n\nIt is home to low level monsters suited for begginers.\n\nThe Boss of Oma Cave is the BoneElite.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 7-10 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 7-10 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[20];
			monster01.Hint = "CaveMaggot";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CaveMaggot";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "7";
				bool flag = WorldMapMobs.User.Level >= 7;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "A maggot that causes little problems when fighting 1 at a time.\n\nIn larger numbers you could find yourself stunned more often than your not!";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "MaggotGuts";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "MaggotShells";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "MaggotParts";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster02.Library = Libraries.Monsters[21];
			monster02.Hint = "Scorpion";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Scorpion";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "15";
				bool flag = WorldMapMobs.User.Level >= 15;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "A low level mob that shouldnt cause too much problem.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "ScorpionVenom";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[19];
			monster03.Hint = "CaveBat";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CaveBat";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "20";
				bool flag = WorldMapMobs.User.Level >= 20;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "CaveBats are notorious for thier high agility and the ability to drop TownTeleports.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "Batwing";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "BatBlood";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster04.Library = Libraries.Monsters[22];
			monster04.Hint = "Skeleton";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Skeleton";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "15";
				bool flag = WorldMapMobs.User.Level >= 15;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "This mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "Bone";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster05.Library = Libraries.Monsters[24];
			monster05.Hint = "AxeSkeleton";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "AxeSkeleton";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "15";
				bool flag = WorldMapMobs.User.Level >= 15;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "This mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "Bone";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[23];
			monster06.Hint = "BoneFighter";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneFighter";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "16";
				bool flag = WorldMapMobs.User.Level >= 16;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "This mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "Bone";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "LargeBone";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[25];
			monster07.Hint = "BoneWarrior";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "17";
				bool flag = WorldMapMobs.User.Level >= 17;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "This mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "Bone";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "LargeBone";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[26];
			monster08.Hint = "BoneElite";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneElite";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "42";
				bool flag = WorldMapMobs.User.Level >= 42;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "The Boss of Oma Cave. Dont underestimate this simple boss if your a low level. It hurts!\n\nThis mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "BoneHeart";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(0);

		}

		public void TheDeadMine()
		{

			dungeonNameLabel.Text = "The Zombie Cave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "90";
			informationArea.Text = "The Zombie Cave is located on the eastern mountain range in Bichon Province and is home to the zombies. This dungeon was once famous for its consistant skill book drops. Those times have changed.\n\nThe boss of this dungeon is ChainedGhoul.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 90 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 50+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton3 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton3.Click += delegate (object o, EventArgs e)
			{
				Teleport(3);
			};

			monster01.Library = Libraries.Monsters[69];
			monster01.Hint = "GroundZombie";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GroundZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "90";
				bool flag = WorldMapMobs.User.Level >= 90;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster02.Library = Libraries.Monsters[70];
			monster02.Hint = "WalkerZombie";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WalkerZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "90";
				bool flag = WorldMapMobs.User.Level >= 90;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[71];
			monster03.Hint = "WandererZombie";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WandererZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "90";
				bool flag = WorldMapMobs.User.Level >= 90;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};
			monster04.Library = Libraries.Monsters[72];
			monster04.Hint = "CrawlerZombie";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrawlerZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "90";
				bool flag = WorldMapMobs.User.Level >= 90;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};
			monster05.Library = Libraries.Monsters[73];
			monster05.Hint = "ShamanZombie";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ShamanZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "90";
				bool flag = WorldMapMobs.User.Level >= 90;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[262];
			monster06.Hint = "ImmortalZombie";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ImmortalZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "120";
				bool flag = WorldMapMobs.User.Level >= 120;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[74];
			monster07.Hint = "ChainedGhoul";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ChainedGhoul";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "The Boss of The Zombi Cave. Dont underestimate this simple boss if your a low level. It hurts!\n\nThis mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(1);
		}

		public void AncientOmaCave()
		{
			dungeonNameLabel.Text = "Ancient_Oma Cave's";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "11+";
			informationArea.Text = "Ancient_Oma Cave is located in the north western mountain range of Bichon Province.\n\nIt is home to low level monsters suited for begginers.\n\nThe Boss of Ancient_OmeCave is the BoneWhoo.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 11-17 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 11-17 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[459];
			monster01.Hint = "Ancient_CaveBat";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Ancient_CaveBat";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "7";
				bool flag = WorldMapMobs.User.Level >= 20;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "CaveBats are notorious for thier high agility and the ability to drop TownTeleports.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "Batwing";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "BatBlood";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[460];
			monster02.Hint = "Ancient_CaveMaggot";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CaveMaggot";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "11";
				bool flag = WorldMapMobs.User.Level >= 11;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "A maggot that causes little problems when fighting 1 at a time.\n\nIn larger numbers you could find yourself stunned more often than your not!";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[461];
			monster03.Hint = "Ancient_Scorpion";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Ancient_Scorpion";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "15";
				bool flag = WorldMapMobs.User.Level >= 15;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "A low level mob that shouldnt cause too much problem.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[462];
			monster04.Hint = "Ancient_Skeleton";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Ancient_Skeleton";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "15";
				bool flag = WorldMapMobs.User.Level >= 15;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "This mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[463];
			monster05.Hint = "Ancient_AxeSkeleton";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Ancient_AxeSkeleton";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "15";
				bool flag = WorldMapMobs.User.Level >= 15;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "This mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[464];
			monster06.Hint = "Ancient_BoneFighter";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Ancient_BoneFighter";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "16";
				bool flag = WorldMapMobs.User.Level >= 16;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "This mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[465];
			monster07.Hint = "Ancient_BoneWarrior";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Ancient_BoneWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "17";
				bool flag = WorldMapMobs.User.Level >= 17;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "This mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[466];
			monster08.Hint = "Ancient_BoneElite";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Ancient_BoneElite";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "42";
				bool flag = WorldMapMobs.User.Level >= 42;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "The Boss of Oma Cave. Dont underestimate this simple boss if your a low level. It hurts!\n\nThis mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[194];
			monster09.Hint = "BoneWhoo";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneWhoo";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "42";
				bool flag = WorldMapMobs.User.Level >= 42;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "The Boss of Oma Cave. Dont underestimate this simple boss if your a low level. It hurts!\n\nThis mob is an Undead Monster so wil recieve higher damage if you have Holy as an added stat.";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(2);
		}

		public void InsectCave()
		{
			dungeonNameLabel.Text = "Insect Cave's";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "17+";
			informationArea.Text = "The Insect Cave is located in the south of WoomyonWsoodsSouth.\n\nIt is home to medium level monsters suited for  middle range players,\n\nAlso home to Khazard the boss of Insect Cave.";

			dropLabel.Text = "Cave Items";
			drop06.Text = "level 11-17 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 11-17 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[81];
			monster01.Hint = "SpiderFrog";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SpiderFrog";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[82];
			monster02.Hint = "HoroBlaster";
			monster02.Click += delegate (object o, EventArgs e)
			{
				informationLabel.Text = "HoroBlaster";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[83];
			monster03.Hint = "BlueHoroBlaster";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlueHoroBlaster";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[84];
			monster04.Hint = "KekTal";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "KekTal";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[85];
			monster05.Hint = "VioletKekTal";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "VioletKekTal";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster06.Visible = false;

			monster07.Library = Libraries.Monsters[86];
			monster07.Hint = "Khazard";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Khazard";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(3);
		}

		public void WoomaCave()
		{
			dungeonNameLabel.Text = "Wooma Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "50+";
			informationArea.Text = "The Wooma Temple is located in the west of WoomyonWsoods.\n\nIt is home to medium level monsters suited for  middle range players,\n\nAlso home to WoomaTaurus the boss of Wooma Temple.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 50+ Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton1 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton1.Click += delegate (object o, EventArgs e)
			{
				Teleport(1);
			};


			monster01.Library = Libraries.Monsters[27];
			monster01.Hint = "Dung";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Dung";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[28];
			monster02.Hint = "Dark";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Dark";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[29];
			monster03.Hint = "WoomaSoldier";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WoomaSoldier";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "55";
				bool flag = WorldMapMobs.User.Level >= 55;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[30];
			monster04.Hint = "WoomaFighter";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WoomaFighter";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "55";
				bool flag = WorldMapMobs.User.Level >= 55;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[31];
			monster05.Hint = "WoomaWarrior";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WoomaWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "55";
				bool flag = WorldMapMobs.User.Level >= 55;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[32];
			monster06.Hint = "FlamingWooma";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FlamingWooma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "55";
				bool flag = WorldMapMobs.User.Level >= 55;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[33];
			monster07.Hint = "WoomaGuardian";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WoomaGuardian";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "75";
				bool flag = WorldMapMobs.User.Level >= 75;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "GoldBar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[34];
			monster08.Hint = "WoomaTaurus";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WoomaTaurus";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "85";
				bool flag = WorldMapMobs.User.Level >= 85;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Orbs";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Books";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "Normal Kitt";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "Specail Kitt";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(22);
		}

		public void FrozenWoomaCave()
		{
			dungeonNameLabel.Text = "Ice Cave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "160+";
			informationArea.Text = "The Ice Cave is located in the SprentVally.\n\nIt is home to High level monsters suited for  middle range players,\n\nAlso home to IceKing the boss of Ice Cave.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 160 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 100+ Skills";
			drop07.ForeColour = Color.YellowGreen;


			teleButton7 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton7.Click += delegate (object o, EventArgs e)
			{
				Teleport(7);
			};

			monster01.Library = Libraries.Monsters[210];
			monster01.Hint = "FrozenZumaGuardian";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenZumaGuardian";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "160";
				bool flag = WorldMapMobs.User.Level >= 160;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[209];
			monster02.Hint = "FrozenZumaSTatue";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenZumaSTatue";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "160";
				bool flag = WorldMapMobs.User.Level >= 160;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[363];
			monster03.Hint = "FrozenSoldier";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenSoldier";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "165";
				bool flag = WorldMapMobs.User.Level >= 165;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[366];
			monster04.Hint = "FrozenKnight";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenKnight";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "165";
				bool flag = WorldMapMobs.User.Level >= 165;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[382];
			monster05.Hint = "FrozenMagician";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenMagician";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "165";
				bool flag = WorldMapMobs.User.Level >= 165;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[364];
			monster06.Hint = "FrozenFighter";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenFighter";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "170";
				bool flag = WorldMapMobs.User.Level >= 170;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[367];
			monster07.Hint = "FrozenGolem";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenGolem";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "170";
				bool flag = WorldMapMobs.User.Level >= 170;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[228];
			monster08.Hint = "IceGuardian";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "IceGuardian";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster09.Library = Libraries.Monsters[229];
			monster09.Hint = "IceKing";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "IceKing";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "250";
				bool flag = WorldMapMobs.User.Level >= 250;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldChest";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "Orbs";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "Specail Kitt";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(4);
		}

		public void SerpentMines()
		{
			dungeonNameLabel.Text = "Serpent Mines";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "20+";
			informationArea.Text = "The Serpent Mine is located in the North-East of SerpentVally.\n\nIt is home to medium level monsters suited for  middle range players,\n\nAlso home to GreatGhoul the boss of Serpent Mine.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 20-26 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 20-26 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[20];
			monster01.Hint = "CaveMaggot";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CaveMaggot";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[69];
			monster02.Hint = "CursedPriest";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CursedPriest";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[70];
			monster03.Hint = "CursedZombie";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CursedZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[71];
			monster04.Hint = "HungryZombie";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HungryZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[72];
			monster05.Hint = "ToxicZombie";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ToxicZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[73];
			monster06.Hint = "CursedShaman";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CursedShaman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[74];
			monster07.Hint = "GreatGhoul";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GreatGhoul";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(5);



		}

		public void Yimoogis()
		{
			dungeonNameLabel.Text = "Snake Cave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "120+";
			informationArea.Text = "The Snake cave\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to QueenSnake the boss of Snake Cave.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 120 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 120 Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(5);
			};

			monster02.Library = Libraries.Monsters[111];
			monster02.Hint = "RedSnake";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedSnake";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "120";
				bool flag = WorldMapMobs.User.Level >= 120;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[115];
			monster03.Hint = "BlueSnake";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlueSnake";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "120";
				bool flag = WorldMapMobs.User.Level >= 120;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[116];
			monster04.Hint = "YellowSnake";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "YellowSnake";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "120";
				bool flag = WorldMapMobs.User.Level >= 120;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[114];
			monster05.Hint = "WhiteEvilSnake";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WhiteEvilSnake";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};


			monster09.Library = Libraries.Monsters[113];
			monster09.Hint = "SnakeQueen";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SnakeQueen";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "180";
				bool flag = WorldMapMobs.User.Level >= 180;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "GoldBarBundle";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldChest";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "Specail Kitt";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(6);
		}

		public void DeathValley()
		{
			dungeonNameLabel.Text = "Bug Coves";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "28+";
			informationArea.Text = "The Bug Cave is located in the North-West of MongchonProvince.\n\nIt is home to Low level monsters suited for  middle range players,\n\nAlso home to EvilCentipede the boss of Bug Cave.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 30 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 20+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton.Click += delegate (object o, EventArgs e)
			{
				Teleport(0);
			};

			monster01.Library = Libraries.Monsters[35];
			monster01.Hint = "WhimperingBee";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WhimperingBee";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "28";
				bool flag = WorldMapMobs.User.Level >= 28;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[36];
			monster02.Hint = "GiantWorm";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GiantWorm";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "28";
				bool flag = WorldMapMobs.User.Level >= 28;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[37];
			monster04.Hint = "Centipede";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Centipede";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "28";
				bool flag = WorldMapMobs.User.Level >= 28;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[38];
			monster05.Hint = "BlackMaggot";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlackMaggot";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "29";
				bool flag = WorldMapMobs.User.Level >= 29;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[39];
			monster06.Hint = "Tongs";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Tongs";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "30";
				bool flag = WorldMapMobs.User.Level >= 30;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[40];
			monster07.Hint = "EvilTongs";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilTongs";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "35";
				bool flag = WorldMapMobs.User.Level >= 35;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Normal Kitt";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster11.Library = Libraries.Monsters[41];
			monster11.Hint = "EvilCentipede";
			monster11.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilCentipede";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "40";
				bool flag = WorldMapMobs.User.Level >= 40;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "GoldBar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Orbs";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "Books";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(7);
		}

		public void AngledStoneTomb()
		{
			dungeonNameLabel.Text = "Boar Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "28+";
			informationArea.Text = "The Angled Stone Temple is located in the South-West of MongchonProvince.\n\nIt is home to medium level monsters suited for middle range players,\n\nAlso home to EvilSnake the boss of Stone Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 22-33 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 22-33 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "0/0";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.AncientStoneTomb();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			monster01.Library = Libraries.Monsters[42];
			monster01.Hint = "BugBat";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BugBat";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[38];
			monster02.Hint = "BlackMaggot";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlackMaggot";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[44];
			monster03.Hint = "WedgeMoth";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WedgeMoth";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[45];
			monster04.Hint = "RedBoar";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedBoar";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[46];
			monster05.Hint = "BlackBoar";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlackBoar";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[47];
			monster06.Hint = "SnakeScorpion";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SnakeScorpion";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[48];
			monster07.Hint = "WhiteBoar";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WhiteBoar";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[49];
			monster09.Hint = "EvilSnake";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilSnake";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			RefreshMobs(8);
		}

		public void AncientStoneTomb()
		{
			dungeonNameLabel.Text = "Ancient Boar Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "28+";
			informationArea.Text = "The Angled Stone Temple is located in the South-West of MongchonProvince.\n\nIt is home to medium level monsters suited for middle range players,\n\nAlso home to EvilSnake the boss of Stone Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 22-33 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 22-33 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/1";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.AngledStoneTomb();
				nextButton.Visible = true;
				backButton.Visible = false;
			};

			monster01.Library = Libraries.Monsters[42];
			monster01.Hint = "BugBat";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BugBat";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[38];
			monster02.Hint = "BlackMaggot";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlackMaggot";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[44];
			monster03.Hint = "WedgeMoth";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WedgeMoth";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[45];
			monster04.Hint = "RedBoar";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedBoar";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[46];
			monster05.Hint = "BlackBoar";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlackBoar";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[47];
			monster06.Hint = "SnakeScorpion";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SnakeScorpion";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[48];
			monster07.Hint = "WhiteBoar";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WhiteBoar";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[49];
			monster09.Hint = "EvilSnake";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilSnake";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			RefreshMobs(8);
		}

		public void ZumaTemple()
		{
			dungeonNameLabel.Text = "Zuma Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "70+";
			informationArea.Text = "The Zuma Temple is located in the North-East of MongchonProvince.\n\nIt is home to medium level monsters suited for  middle range players,\n\nAlso home to ZumaTaurus the boss of Zuma Temple.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 70+ Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 50+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton2 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton2.Click += delegate (object o, EventArgs e)
			{
				Teleport(2);
			};

			monster01.Library = Libraries.Monsters[63];
			monster01.Hint = "BigRat";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BigRat";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "70";
				bool flag = WorldMapMobs.User.Level >= 70;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[64];
			monster03.Hint = "ZumaArcher";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZumaArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "70";
				bool flag = WorldMapMobs.User.Level >= 70;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[65];
			monster04.Hint = "ZumaStatue";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZumaStatue";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "75";
				bool flag = WorldMapMobs.User.Level >= 75;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[66];
			monster05.Hint = "ZumaGuardian";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZumaGuardian";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "75";
				bool flag = WorldMapMobs.User.Level >= 75;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[67];
			monster07.Hint = "RedThunderZuma";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedThunderZuma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "90";
				bool flag = WorldMapMobs.User.Level >= 90;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "GoldBar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster12.Library = Libraries.Monsters[68];
			monster12.Hint = "ZumaTaurus";
			monster12.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZumaTaurus";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "100";
				bool flag = WorldMapMobs.User.Level >= 100;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Orbs";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Books";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "Normal Kitt";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "Specail Kitt";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;
			RefreshMobs(9);
		}

		public void AncientZumaTemple()
		{
			dungeonNameLabel.Text = "Ancient_Zuma Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "35+";
			informationArea.Text = "The Zuma Temple is located in the North-East of MongchonProvince.\n\nIt is home to medium level monsters suited for  middle range players,\n\nAlso home to ZumaTaurus the boss of Zuma Temple.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 26-40 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 26-40 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/1";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ZumaTemple();
				nextButton.Visible = true;
				backButton.Visible = false;
			};

			monster01.Library = Libraries.Monsters[63];
			monster01.Hint = "BigRat";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BigRat";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[64];
			monster03.Hint = "ZumaArcher";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZumaArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[65];
			monster04.Hint = "ZumaStatue";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZumaStatue";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[66];
			monster05.Hint = "ZumaGuardian";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZumaGuardian";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[67];
			monster07.Hint = "RedThunderZuma";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedThunderZuma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster12.Library = Libraries.Monsters[68];
			monster12.Hint = "ZumaTaurus";
			monster12.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZumaTaurus";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			RefreshMobs(9);
		}

		public void FoxLayer()
		{
			dungeonNameLabel.Text = "Fox Layer's";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "35+";
			informationArea.Text = "The Fox Province is located in the North-East of MongchonProvince.\n\nIt is home to higer level monsters suited for  higer range players,\n\nAlso home to GreatFoxSpirit the boss of Fox Province.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[127];
			monster01.Hint = "BlackFoxman";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlackFoxman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[128];
			monster02.Hint = "RedFoxman";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedFoxman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[129];
			monster03.Hint = "WhiteFoxman";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WhiteFoxman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[136];
			monster05.Hint = "BigHedgeKekTal";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BigHedgeKekTal";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[137];
			monster06.Hint = "RedFrogSpider";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedFrogSpider";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[138];
			monster07.Hint = "BrownFrogSpider";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BrownFrogSpider";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[134];
			monster09.Hint = "GreatFoxSpirit";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GreatFoxSpirit";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(10);
		}

		public void RedmoonValley()
		{
			dungeonNameLabel.Text = "RedMoon Evil";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "38+";
			informationArea.Text = "The RedMoon Vally is located in the North of Tao Village.\n\nIt is home to higer level monsters suited for higer range players,\n\nAlso home to RedMoonEvil the boss of RedMoon Vally.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[57];
			monster01.Hint = "BigApe";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BigApe";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[58];
			monster02.Hint = "EvilApe";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilApe";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};


			monster13.Library = Libraries.Monsters[62];
			monster13.Hint = "RedMoonEvil";
			monster13.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedMoonEvil";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[59];
			monster06.Hint = "GrayEvilApe";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GrayEvilApe";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[60];
			monster07.Hint = "RedEvilApe";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedEvilApe";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[61];
			monster08.Hint = "CrystalSpider";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrystalSpider";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(11);
		}

		public void GonRyunWoods()
		{
			dungeonNameLabel.Text = "GonRyun Woods";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "35+";
			informationArea.Text = "The GonRyun Woods is located in the North West of Tao Village.\n\nIt is home to higer level monsters suited for higer range players,\n\nAlso home to GonRyunCaptin the boss of GonRyun Woods";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[485];
			monster01.Hint = "GonRyunAchie";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GonRyunAchie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[486];
			monster02.Hint = "GonRyunWar";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GonRyunWar";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[487];
			monster03.Hint = "GonRyunBow";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GonRyunBow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster18.Library = Libraries.Monsters[480];
			monster18.Hint = "GonRyunTao";
			monster18.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GonRyunTao";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster15.Library = Libraries.Monsters[483];
			monster15.Hint = "GonRyunTao";
			monster15.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GonRyunTao";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster16.Library = Libraries.Monsters[488];
			monster16.Hint = "GonRyunSword";
			monster16.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GonRyunSword";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster17.Library = Libraries.Monsters[489];
			monster17.Hint = "GonRyunSiner";
			monster17.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GonRyunSiner";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster14.Library = Libraries.Monsters[490];
			monster14.Hint = "GonRyunCaptin";
			monster14.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GonRyunCaptin";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(12);
		}

		public void PrajnaStoneCave()
		{
			dungeonNameLabel.Text = "Prajna Stone Cave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "35+";
			informationArea.Text = "The Prajna Stone cave is located in the North of Prajna Island.\n\nIt is home to medium level monsters suited for middle range players,\n\nAlso home to BoneLord the boss of Prajna Stone cave.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "0/0";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.AncientPrajnaStoneCave();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			monster01.Library = Libraries.Monsters[87];
			monster01.Hint = "RoninGhoul";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RoninGhoul";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[92];
			monster02.Hint = "BoneArcher";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[91];
			monster03.Hint = "BoneBlademan";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneBlademan";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[89];
			monster05.Hint = "BoneCaptain";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneCaptain";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[90];
			monster06.Hint = "BoneSpearman";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneSpearman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[88];
			monster07.Hint = "ToxicGhoul";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ToxicGhoul";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[93];
			monster09.Hint = "BoneLord";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneLord";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			RefreshMobs(13);
		}

		public void AncientPrajnaStoneCave()
		{
			dungeonNameLabel.Text = "Prajna Stone Cave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "35+";
			informationArea.Text = "The Prajna Stone cave is located in the North of Prajna Island.\n\nIt is home to medium level monsters suited for middle range players,\n\nAlso home to BoneLord the boss of Prajna Stone cave.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/1";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.PrajnaStoneCave();
				nextButton.Visible = true;
				backButton.Visible = false;
			};

			monster01.Library = Libraries.Monsters[87];
			monster01.Hint = "RoninGhoul";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RoninGhoul";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[92];
			monster02.Hint = "BoneArcher";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[91];
			monster03.Hint = "BoneBlademan";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneBlademan";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[89];
			monster05.Hint = "BoneCaptain";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneCaptain";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[90];
			monster06.Hint = "BoneSpearman";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneSpearman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[88];
			monster07.Hint = "ToxicGhoul";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ToxicGhoul";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[93];
			monster09.Hint = "BoneLord";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneLord";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			RefreshMobs(13);
		}

		public void PrajnaStone()
		{
			dungeonNameLabel.Text = "Prajna Cave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "35+";
			informationArea.Text = "The Prajna Stone cave is located in the North of Prajna Island.\n\nIt is home to medium level monsters suited for middle range players,Also home to BoneLord the boss of Prajna Stone cave.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[87];
			monster01.Hint = "RoninGhoul";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RoninGhoul";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[92];
			monster02.Hint = "BoneArcher";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[91];
			monster03.Hint = "BoneBlademan";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneBlademan";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[89];
			monster05.Hint = "BoneCaptain";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneCaptain";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[90];
			monster06.Hint = "BoneSpearman";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneSpearman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[88];
			monster07.Hint = "ToxicGhoul";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ToxicGhoul";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[93];
			monster09.Hint = "BoneLord";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneLord";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(13);
		}

		public void PrajnaTemple()
		{
			dungeonNameLabel.Text = "Prajna Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "110+";
			informationArea.Text = "The Prajna Temple is located in the east of Prajna Island.\n\nIt is home to medium level monsters suited for middle range players,\n\nAlso home to PrajnaKing the boss of Prajna Temple.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 110 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 70+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton4 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton4.Click += delegate (object o, EventArgs e)
			{
				Teleport(4);
			};

			monster01.Library = Libraries.Monsters[279];
			monster01.Hint = "catshaman";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "catshaman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "110";
				bool flag = WorldMapMobs.User.Level >= 110;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[95];
			monster02.Hint = "catwidow";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "catwidow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "110";
				bool flag = WorldMapMobs.User.Level >= 110;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[227];
			monster03.Hint = "chaosghost";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "chaosghost";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "110";
				bool flag = WorldMapMobs.User.Level >= 110;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[224];
			monster04.Hint = "ManectricStaff";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ManectricStaff";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "115";
				bool flag = WorldMapMobs.User.Level >= 115;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[223];
			monster05.Hint = "Manectricclaw";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Manectricclaw";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "115";
				bool flag = WorldMapMobs.User.Level >= 115;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[228];
			monster06.Hint = "Manectricblest";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Manectricblest";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "115";
				bool flag = WorldMapMobs.User.Level >= 115;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[221];
			monster07.Hint = "ManectricHammer";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ManectricHammer";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "118";
				bool flag = WorldMapMobs.User.Level >= 118;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[99];
			monster08.Hint = "PrajnaLord";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "PrajnaLord";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "GoldBar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster09.Library = Libraries.Monsters[101];
			monster09.Hint = "PrajnaKing";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "PrajnaKing";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "180";
				bool flag = WorldMapMobs.User.Level >= 180;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "GoldBarBundle";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldChest";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "Specail Kitt";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(8);
		}

		public void BlackDragonDungeon()
		{
			dungeonNameLabel.Text = "Black Dragon Dungeon";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "40+";
			informationArea.Text = "The Black Dragon Dungeon is located in the North West of CastleGi-Ryoong.\n\nIt is home to medium level monsters suited for middle range players.\nAlso home to ,EvilSnake, KingScorpion, EvilTongs, WoomaTaurus, KingHog, DarkDevil.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[40]; // EvilTongs
			monster01.Hint = "EvilTong";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilTong";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[49]; // EvilSnake
			monster02.Hint = "EvilSnake";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilSnake";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[75]; // KingScorpion
			monster03.Hint = "KingScorpion";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "KingScorpion";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[48]; // WhiteBoar
			monster04.Hint = "WhiteBoar";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WhiteBoar";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[76]; // KingHog
			monster05.Hint = "KingHog";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "KingHog";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[65]; // ZumaStatue
			monster06.Hint = "ZumaStatue";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZumaStatue";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[68]; // IncarnatedZT
			monster07.Hint = "IncarnatedZT";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "IncarnatedZT";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[34]; // IncarnatedWT
			monster08.Hint = "IncarnatedWT";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "IncarnatedWT";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[77]; // DarkDevil
			monster09.Hint = "DarkDevil";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "DarkDevil";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(2);
		}

		public void ChogakujiDungeon()
		{
			dungeonNameLabel.Text = "Chogakuji Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "The Chogakuij Dungeon is located in the West of CastleGi-Ryoong.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to Great Chogakuij the boss of Chogakuij Dungeon.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/6";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon1();
				nextButton.Visible = true;
				backButton.Visible = true;
			};

			monster01.Library = Libraries.Monsters[188]; // Donatello
			monster01.Hint = "Donatello";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Donatello";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[189]; // Leonardo
			monster02.Hint = "Leonardo";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Leonardo";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[185]; // Michelangelo
			monster03.Hint = "Michelangelo";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Michelangelo";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[186]; // Raphael
			monster04.Hint = "Raphael";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Raphael";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[169]; // RedTurtle
			monster05.Hint = "RedTurtle";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[170]; // GreenTurtle
			monster06.Hint = "GreenTurtle";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GreenTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[171]; // BlueTurtle
			monster07.Hint = "BlueTurtle";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlueTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[352]; // ChogTurtle
			monster08.Hint = "ChogTurtle";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ChogTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[187]; // MasterSplinter
			monster09.Hint = "MasterSplinter";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "MasterSplinter";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			RefreshMobs(2);
		}

		public void ChogakujiDungeon1()
		{
			dungeonNameLabel.Text = "Chogakuji Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "The Chogakuij Dungeon is located in the West of CastleGi-Ryoong.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to Great Chogakuij the boss of Chogakuij Dungeon.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "2/6";
			pageLabel.ForeColour = Color.YellowGreen;


			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon();
				nextButton.Visible = true;
				backButton.Visible = false;
			};

			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon2();
				nextButton.Visible = true;
				backButton.Visible = true;
			};

			monster01.Library = Libraries.Monsters[205]; // VenomWeaver
			monster01.Hint = "VenomWeaver";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "VenomWeaver";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[206]; // CrackingWeaver
			monster02.Hint = "CrackingWeaver";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrackingWeaver";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[207]; // ArmingWeaver
			monster03.Hint = "ArmingWeaver";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ArmingWeaver";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[211]; // LightWarrior
			monster04.Hint = "LightWarrior";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "LightWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[208]; // CrystalWeaver
			monster05.Hint = "CrystalWeaver";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrystalWeaver";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[213]; // MutantWarrior
			monster06.Hint = "MutantWarrior";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "MutantWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[214]; // MutantWizard
			monster07.Hint = "MutantWizard";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "MutantWizard";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[211]; // LightWarrior
			monster08.Hint = "LightWarrior";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "LightWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};


			monster09.Library = Libraries.Monsters[202]; // FlyingStatue
			monster09.Hint = "FlyingStatue";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FlyingStatue";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			RefreshMobs(8);
		}

		public void ChogakujiDungeon2()
		{
			dungeonNameLabel.Text = "Chogakuji Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "The Chogakuij Dungeon is located in the West of CastleGi-Ryoong.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to Great Chogakuij the boss of Chogakuij Dungeon.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "3/6";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon1();
				nextButton.Visible = true;
				backButton.Visible = true;
			};

			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon3();
				nextButton.Visible = true;
				backButton.Visible = true;
			};


			monster01.Library = Libraries.Monsters[191]; //DarkAxeOma
			monster01.Hint = "DarkAxeOma";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "DarkAxeOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster19.Library = Libraries.Monsters[192]; //DarkCrossbowOma
			monster19.Hint = "DarkCrossbowOma";
			monster19.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "DarkCrossbowOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[190]; //DarkSwordOma
			monster05.Hint = "DarkSwordOma";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "DarkSwordOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[193]; //DarkWingedOma
			monster07.Hint = "DarkWingedOma";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "DarkWingedOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[320]; ///DarkOma
			monster09.Hint = "DarkOma";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "DarkOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			RefreshMobs(14);
		}

		public void ChogakujiDungeon3()
		{
			dungeonNameLabel.Text = "Chogakuji Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "The Chogakuij Dungeon is located in the West of CastleGi-Ryoong.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to Great Chogakuij the boss of Chogakuij Dungeon.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "4/6";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon2();
				nextButton.Visible = true;
				backButton.Visible = true;
			};

			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon4();
				nextButton.Visible = true;
				backButton.Visible = true;
			};

			monster01.Library = Libraries.Monsters[152]; //GhastlyLeecher
			monster01.Hint = "GhastlyLeecher";

			monster02.Library = Libraries.Monsters[153]; //CyanoGhast
			monster02.Hint = "CyanoGhast";

			monster03.Library = Libraries.Monsters[154]; //MutatedManworm
			monster03.Hint = "MutatedManworm";

			monster05.Library = Libraries.Monsters[155]; //CrazyManworm
			monster05.Hint = "CrazyManworm";

			monster06.Library = Libraries.Monsters[159]; //DarkDevourer
			monster06.Hint = "DarkDevourer";

			monster07.Library = Libraries.Monsters[163]; //DreamDevourer
			monster07.Hint = "DreamDevourer";

			monster09.Library = Libraries.Monsters[158]; //Behemoth
			monster09.Hint = "Behemoth";


			RefreshMobs(13);
		}

		public void ChogakujiDungeon4()
		{
			dungeonNameLabel.Text = "Chogakuji Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "The Chogakuij Dungeon is located in the West of CastleGi-Ryoong.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to Great Chogakuij the boss of Chogakuij Dungeon.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "5/6";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon3();
				nextButton.Visible = true;
				backButton.Visible = true;
			};

			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon5();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			monster01.Library = Libraries.Monsters[238]; //FlameSpear
			monster01.Hint = "FlameSpear";

			monster03.Library = Libraries.Monsters[239]; //FlameMage
			monster03.Hint = "FlameMage";

			monster05.Library = Libraries.Monsters[240]; //FlameScythe
			monster05.Hint = "FlameScythe";

			monster07.Library = Libraries.Monsters[241]; //FlameAssassin
			monster07.Hint = "FlameAssassin";

			monster09.Library = Libraries.Monsters[242]; //FlameQueen
			monster09.Hint = "FlameQueen";


			RefreshMobs(15);

		}

		public void ChogakujiDungeon5()
		{
			dungeonNameLabel.Text = "Chogakuji Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "The Chogakuij Dungeon is located in the West of CastleGi-Ryoong.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to Great Chogakuij the boss of Chogakuij Dungeon.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "6/6";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.ChogakujiDungeon4();
				nextButton.Visible = true;
				backButton.Visible = true;
			};

			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				//GameScene.Scene.WorldMapMobs.ChogakujiDungeon6();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			monster01.Library = Libraries.Monsters[233]; //TrollSlave
			monster01.Hint = "TrollSlave";

			monster03.Library = Libraries.Monsters[234]; //TrollHammer
			monster03.Hint = "TrollHammer";

			monster05.Library = Libraries.Monsters[235]; //TrollBomber
			monster05.Hint = "TrollBomber";

			monster07.Library = Libraries.Monsters[236]; //TrollStoner
			monster07.Hint = "TrollStoner";

			monster09.Library = Libraries.Monsters[237]; //TrollKing
			monster09.Hint = "TrollKing";

			RefreshMobs(15);
		}

		public void PbNorthCave()
		{
			dungeonNameLabel.Text = "Past Bichon North";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "33+";
			informationArea.Text = "The Past oma caves is located in the North East to the mountians of Past Bichon.\n\nIt is home to medium level monsters suited for middle range players.\n\nAlso home to OmaKing the boss of Past oma caves.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 33-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 33-45 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[118];
			monster01.Hint = "AxeOma";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "AxeOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[119];
			monster02.Hint = "SwordOma";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SwordOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster19.Library = Libraries.Monsters[120];
			monster19.Hint = "CrossbowOma";
			monster19.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrossbowOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[121];
			monster05.Hint = "WingedOma";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WingedOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[122];
			monster06.Hint = "FlailOma";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FlailOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[123];
			monster07.Hint = "OmaGuard";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "OmaGuard";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[126];
			monster09.Hint = "OmaKing";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "OmaKing";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(10);
		}

		public void PbSouthCave()
		{
			dungeonNameLabel.Text = "Past Bichon South";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "33+";
			informationArea.Text = "The Past oma caves is located in the North East to the mountians of Past Bichon.\n\nIt is home to medium level monsters suited for middle range players.\n\nAlso home to OmaKing the boss of Past oma caves.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 33-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 33-45 Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[118];
			monster01.Hint = "AxeOma";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "AxeOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[119];
			monster02.Hint = "SwordOma";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SwordOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster19.Library = Libraries.Monsters[120];
			monster19.Hint = "CrossbowOma";
			monster19.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrossbowOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[121];
			monster05.Hint = "WingedOma";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WingedOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[122];
			monster06.Hint = "FlailOma";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FlailOma";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[123];
			monster07.Hint = "OmaGuard";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "OmaGuard";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[126];
			monster09.Hint = "OmaKing";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "OmaKing";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(10);
		}

		public void LowWorldBoss()
		{
			dungeonNameLabel.Text = "LowWorldBoss";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "50+";
			informationArea.Text = "The Low World Boss.\n\nIt is home to Low level monsters suited for low range players.\n\nAlso Alot Off Monsters there.\n\nCost You 5.000.000";
			dropLabel.Text = "Cave Items";
			drop06.Text = "Torchs";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "ShoulderPads";
			drop07.ForeColour = Color.YellowGreen;
			drop08.Text = "Crystal";
			drop08.ForeColour = Color.YellowGreen;
			drop09.Text = "Medal";
			drop09.ForeColour = Color.YellowGreen;
			drop10.Text = "Shield";
			drop10.ForeColour = Color.YellowGreen;
			drop11.Text = "TalisMan";
			drop11.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/6";
			pageLabel.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.LowWorldBoss2();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			teleButton6 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton6.Click += delegate (object o, EventArgs e)
			{
				Teleport(9);
			};

			monster01.Library = Libraries.Monsters[184]; // WingedLordLow
			monster01.Hint = "WingedLordLow";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WingedLordLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[337]; // BloodHunterLow
			monster04.Hint = "BloodHunterLow";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BloodHunterLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[101]; // SpearKingLow
			monster03.Hint = "SpearKingLow";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SpearKingLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = " Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster05.Library = Libraries.Monsters[238]; // FlameSpearLow
			monster05.Hint = "FlameSpearLow";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FlameSpearLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[208]; // Weaver
			monster07.Hint = "CrystalWeaverLow";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrystalWeaverLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[299]; // SwampWarriorLow
			monster08.Hint = "SwampWarriorLow";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SwampWarriorLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster09.Library = Libraries.Monsters[239]; // ZoneMageLow
			monster09.Hint = "ZoneMageLow";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneMageLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = true;
			pageLabel.Visible = true;

			RefreshMobs(28);
		}

		public void LowWorldBoss2()
		{
			dungeonNameLabel.Text = "LowWorldBoss";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "50+";
			informationArea.Text = "The Low World Boss.\n\nIt is home to Low level monsters suited for high range players.\n\nAlso Alot Off Monsters there.\n\nCost You 5.000.000";
			dropLabel.Text = "Cave Items";
			drop06.Text = "Torchs";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "ShoulderPads";
			drop07.ForeColour = Color.YellowGreen;
			drop08.Text = "Crystal";
			drop08.ForeColour = Color.YellowGreen;
			drop09.Text = "Medal";
			drop09.ForeColour = Color.YellowGreen;
			drop10.Text = "Shield";
			drop10.ForeColour = Color.YellowGreen;
			drop11.Text = "TalisMan";
			drop11.ForeColour = Color.YellowGreen;
			pageLabel.Text = "2/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.LowWorldBoss();
				nextButton.Visible = true;
				backButton.Visible = false;
			};
			monster01.Library = Libraries.Monsters[229]; // FrozenLordLow
			monster01.Hint = "FrozenLordLow";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenLordLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "5";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[241]; // ZoneAssassinLow
			monster07.Hint = "ZoneAssassinLow";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneAssassinLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[242]; // ZoneQueenLow
			monster03.Hint = "ZoneQueenLow";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneQueenLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster04.Library = Libraries.Monsters[298]; // MantisLow
			monster04.Hint = "MantisLow";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "MantisLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[93]; // BoneKingLow
			monster05.Hint = "BoneKingLow";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneKingLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[240]; // ZoneScytheLow
			monster06.Hint = "ZoneScytheLow";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneScytheLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[252]; // BloodKingLow
			monster08.Hint = "BloodKingLow";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BloodKingLow";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "50";
				bool flag = WorldMapMobs.User.Level >= 50;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			backButton.Visible = true;
			nextButton.Visible = false;
			pageLabel.Visible = true;

			RefreshMobs(23);
		}

		public void MedWorldBoss()
		{
			dungeonNameLabel.Text = "MedWorldBoss";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "150+";
			informationArea.Text = "The Med World Boss.\n\nIt is home to Med level monsters suited for Med range players.\n\nAlso Alot Off Monsters there.\n\nCost You 10.000.000";
			dropLabel.Text = "Cave Items";
			drop06.Text = "Torchs";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "ShoulderPads";
			drop07.ForeColour = Color.YellowGreen;
			drop08.Text = "Crystal";
			drop08.ForeColour = Color.YellowGreen;
			drop09.Text = "Medal";
			drop09.ForeColour = Color.YellowGreen;
			drop10.Text = "Shield";
			drop10.ForeColour = Color.YellowGreen;
			drop11.Text = "TalisMan";
			drop11.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/6";
			pageLabel.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.MedWorldBoss2();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			teleButton6 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton6.Click += delegate (object o, EventArgs e)
			{
				Teleport(10);
			};

			monster01.Library = Libraries.Monsters[184]; // WingedLordMed
			monster01.Hint = "WingedLordMed";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WingedLordMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[337]; // BloodHunterMed
			monster04.Hint = "BloodHunterMed";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BloodHunterMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[101]; // SpearKingMed
			monster03.Hint = "SpearKingMed";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SpearKingMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = " Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster05.Library = Libraries.Monsters[238]; // FlameSpearMed
			monster05.Hint = "FlameSpearMed";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FlameSpearMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[208]; // CrystalWeaverMed
			monster07.Hint = "CrystalWeaverMed";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrystalWeaverMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[299]; // SwampWarriorMed
			monster08.Hint = "SwampWarriorMed";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SwampWarriorMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster09.Library = Libraries.Monsters[239]; // ZoneMageMed
			monster09.Hint = "ZoneMageMed";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneMageMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = true;
			pageLabel.Visible = true;

			RefreshMobs(28);
		}

		public void MedWorldBoss2()
		{
			dungeonNameLabel.Text = "MedWorldBoss";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "150+";
			informationArea.Text = "The Med World Boss.\n\nIt is home to Med level monsters suited for high range players.\n\nAlso Alot Off Monsters there.\n\nCost You 10.000.000";
			dropLabel.Text = "Cave Items";
			drop06.Text = "Torchs";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "ShoulderPads";
			drop07.ForeColour = Color.YellowGreen;
			drop08.Text = "Crystal";
			drop08.ForeColour = Color.YellowGreen;
			drop09.Text = "Medal";
			drop09.ForeColour = Color.YellowGreen;
			drop10.Text = "Shield";
			drop10.ForeColour = Color.YellowGreen;
			drop11.Text = "TalisMan";
			drop11.ForeColour = Color.YellowGreen;
			pageLabel.Text = "2/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.MedWorldBoss();
				nextButton.Visible = true;
				backButton.Visible = false;
			};
			monster01.Library = Libraries.Monsters[229]; // FrozenLordMed
			monster01.Hint = "FrozenLordMed";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenLordMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[241]; // ZoneAssassinMed
			monster07.Hint = "ZoneAssassinMed";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneAssassinMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[242]; // ZoneQueenMed
			monster03.Hint = "ZoneQueenMed";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneQueenMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster04.Library = Libraries.Monsters[298]; // MantisMed
			monster04.Hint = "MantisMed";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "MantisMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[93]; // BoneKingMed
			monster05.Hint = "BoneKingMed";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneKingMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[240]; // ZoneScytheMed
			monster06.Hint = "ZoneScytheMed";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneScytheMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[252]; // BloodKingMed
			monster08.Hint = "BloodKingMed";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BloodKingMed";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			backButton.Visible = true;
			nextButton.Visible = false;
			pageLabel.Visible = true;

			RefreshMobs(23);
		}

		public void HighWorldBoss()
		{
			dungeonNameLabel.Text = "HighWorldBoss";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "200+";
			informationArea.Text = "The High World Boss.\n\nIt is home to High level monsters suited for High range players.\n\nAlso Alot Off Monsters there.\n\nCost You 50 CP";
			dropLabel.Text = "Cave Items";
			drop06.Text = "Torchs";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "ShoulderPads";
			drop07.ForeColour = Color.YellowGreen;
			drop08.Text = "Crystal";
			drop08.ForeColour = Color.YellowGreen;
			drop09.Text = "Medal";
			drop09.ForeColour = Color.YellowGreen;
			drop10.Text = "Shield";
			drop10.ForeColour = Color.YellowGreen;
			drop11.Text = "TalisMan";
			drop11.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/6";
			pageLabel.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.HighWorldBoss2();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			teleButton6 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton6.Click += delegate (object o, EventArgs e)
			{
				Teleport(15);
			};

			monster01.Library = Libraries.Monsters[184]; // WingedLordHigh
			monster01.Hint = "WingedLordHigh";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "WingedLordHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[337]; // BloodHunterHigh
			monster04.Hint = "BloodHunterHigh";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BloodHunterHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[101]; // SpearKingHigh
			monster03.Hint = "SpearKingHigh";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SpearKingHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster05.Library = Libraries.Monsters[238]; // FlameSpearHigh
			monster05.Hint = "FlameSpearHigh";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FlameSpearHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[208]; // CrystalWeaverHigh
			monster07.Hint = "CrystalWeaverHigh";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrystalWeaverHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[299]; // SwampWarriorHigh
			monster08.Hint = "SwampWarriorHigh";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SwampWarriorHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster09.Library = Libraries.Monsters[239]; // ZoneMageHigh
			monster09.Hint = "ZoneMageHigh";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneMageHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = true;
			pageLabel.Visible = true;

			RefreshMobs(28);
		}

		public void HighWorldBoss2()
		{
			dungeonNameLabel.Text = "HighWorldBoss";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "200+";
			informationArea.Text = "The High World Boss.\n\nIt is home to High level monsters suited for high range players.\n\nAlso Alot Off Monsters there.\n\nCost You 50 CP";
			dropLabel.Text = "Cave Items";
			drop06.Text = "Torchs";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "ShoulderPads";
			drop07.ForeColour = Color.YellowGreen;
			drop08.Text = "Crystal";
			drop08.ForeColour = Color.YellowGreen;
			drop09.Text = "Medal";
			drop09.ForeColour = Color.YellowGreen;
			drop10.Text = "Shield";
			drop10.ForeColour = Color.YellowGreen;
			drop11.Text = "TalisMan";
			drop11.ForeColour = Color.YellowGreen;
			pageLabel.Text = "2/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.HighWorldBoss();
				nextButton.Visible = true;
				backButton.Visible = false;
			};
			monster01.Library = Libraries.Monsters[229]; // FrozenLordHigh
			monster01.Hint = "FrozenLordHigh";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenLordHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[241]; // ZoneAssassinHigh
			monster07.Hint = "ZoneAssassinHigh";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneAssassinHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[242]; // ZoneQueenHigh
			monster03.Hint = "ZoneQueenHigh";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneQueenHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster04.Library = Libraries.Monsters[298]; // MantisHigh
			monster04.Hint = "MantisHigh";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "MantisHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[93]; // BoneKingHigh
			monster05.Hint = "BoneKingHigh";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BoneKingHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[240]; // ZoneScytheHigh
			monster06.Hint = "ZoneScytheHigh";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ZoneScytheHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[252]; // BloodKingHigh
			monster08.Hint = "BloodKingHigh";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BloodKingHigh";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Drops";
				drop01.Text = "Torch";
				drop01.ForeColour = Color.YellowGreen;
				drop02.Text = "ShoulderPads";
				drop02.ForeColour = Color.YellowGreen;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			backButton.Visible = true;
			nextButton.Visible = false;
			pageLabel.Visible = true;

			RefreshMobs(23);
		}

		public void CataCombs()
		{
			dungeonNameLabel.Text = "Cata Combs's";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.CataCombs2();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			monster01.Library = Libraries.Monsters[273]; //FightingCat
			monster01.Hint = "FightingCat";

			monster02.Library = Libraries.Monsters[274]; //FireCat
			monster02.Hint = "FireCat";

			monster03.Library = Libraries.Monsters[275]; //CatWidow
			monster03.Hint = "CatWidow";

			monster05.Library = Libraries.Monsters[276]; //StainHammerCat
			monster05.Hint = "StainHammerCat";

			monster06.Library = Libraries.Monsters[277]; //BlackHammerCat
			monster06.Hint = "BlackHammerCat";

			monster07.Library = Libraries.Monsters[278]; //StrayCat
			monster07.Hint = "StrayCat";

			monster09.Library = Libraries.Monsters[279]; //CatShaman
			monster09.Hint = "CatShaman";


			RefreshMobs(13);
		}

		public void CataCombs2()
		{
			dungeonNameLabel.Text = "Cata Combs's";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "2/2";
			pageLabel.ForeColour = Color.YellowGreen;


			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.CataCombs();
				nextButton.Visible = true;
				backButton.Visible = false;
			};

			monster01.Library = Libraries.Monsters[280]; //Jar1
			monster01.Hint = "Jar1";

			monster03.Library = Libraries.Monsters[281]; //Jar2
			monster03.Hint = "Jar2";

			monster05.Library = Libraries.Monsters[283]; //RestlessJar
			monster05.Hint = "RestlessJar";

			monster07.Library = Libraries.Monsters[282]; //SeedingsGeneral
			monster07.Hint = "SeedingsGeneral";

			RefreshMobs(16);
		}

		public void SnowCove()
		{
			dungeonNameLabel.Text = "Snow Cove's";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.SnowCove2();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			monster01s.Library = Libraries.Monsters[380]; //FrozenMiner
			monster01s.Hint = "FrozenMiner";

			monster02s.Library = Libraries.Monsters[381]; //FrozenAxeman
			monster02s.Hint = "FrozenAxeman";

			monster03s.Library = Libraries.Monsters[382]; //FrozenMagician
			monster03s.Hint = "FrozenMagician";

			monster05s.Library = Libraries.Monsters[383]; //SnowYeti
			monster05s.Hint = "SnowYeti";

			monster06s.Library = Libraries.Monsters[384]; //IceCrystalSoldier
			monster06s.Hint = "IceCrystalSoldier";

			monster07s.Library = Libraries.Monsters[385]; //DarkWraith
			monster07s.Hint = "DarkWraith";

			monster09.Library = Libraries.Monsters[386]; //DarkSpirit
			monster09.Hint = "DarkSpirit";

			RefreshMobs(17);
		}

		public void SnowCove2()
		{
			dungeonNameLabel.Text = "Snow Cove's";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "2/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.SnowCove();
				nextButton.Visible = true;
				backButton.Visible = false;
			};

			monster01.Library = Libraries.Monsters[403]; //SnowFlower
			monster01.Hint = "SnowFlower";

			monster03.Library = Libraries.Monsters[409]; //SnowFlowerQueen
			monster03.Hint = "SnowFlowerQueen";

			monster05.Library = Libraries.Monsters[363]; //FrozenSoldier
			monster05.Hint = "FrozenSoldier";

			monster07.Library = Libraries.Monsters[364]; //FrozenFighter
			monster07.Hint = "FrozenFighter";

			monster09s.Library = Libraries.Monsters[387]; //CrystalBeast
			monster09s.Hint = "CrystalBeast";

			RefreshMobs(18);
		}

		public void OrcCombs()
		{
			dungeonNameLabel.Text = "Orc Combs's";
			informationArea.Text = "The Orc Combs is located in the East of Seokcho Valley.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to OrcMohzat the boss of Orc Combs.";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.OrcCombs2();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			monster01.Library = Libraries.Monsters[437]; //OrcDagger
			monster01.Hint = "OrcDagger";

			monster02.Library = Libraries.Monsters[438]; //OrcMiner
			monster02.Hint = "OrcMiner";

			monster03.Library = Libraries.Monsters[439]; //OrcSpearthrower
			monster03.Hint = "OrcSpearthrower";

			monster05.Library = Libraries.Monsters[440]; //OrcWarrior
			monster05.Hint = "OrcWarrior";

			monster06.Library = Libraries.Monsters[441]; //OrcWithAnimal
			monster06.Hint = "OrcWithAnimal";

			monster07.Library = Libraries.Monsters[442]; //OrcRider
			monster07.Hint = "OrcRider";

			monster09.Library = Libraries.Monsters[443]; //OrcWizard
			monster09.Hint = "OrcWizard";

			RefreshMobs(13);
		}

		public void OrcCombs2()
		{
			dungeonNameLabel.Text = "Orc Combs's";
			informationArea.Text = "The Orc Combs is located in the East of Seokcho Valley.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to OrcMohzat the boss of Orc Combs.";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "43+";
			informationArea.Text = "";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "2/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.OrcCombs();
				nextButton.Visible = true;
				backButton.Visible = false;
			};

			monster01.Library = Libraries.Monsters[444]; //OrcCook
			monster01.Hint = "OrcCook";

			monster03.Library = Libraries.Monsters[445]; //OrcMace
			monster03.Hint = "OrcMace";

			monster05.Library = Libraries.Monsters[446]; //OrcCommander
			monster05.Hint = "OrcCommander";

			monster07.Library = Libraries.Monsters[448]; //OrcGeneral
			monster07.Hint = "OrcGeneral";

			RefreshMobs(16);
		}

		public void TurleLayer()
		{
			dungeonNameLabel.Text = "Turle Layer";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "40+";
			informationArea.Text = "The Turle Layers is located in the East of Seokcho Valley.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to MasterSplinter the boss of Turle Layer.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 40-45 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 40-45 Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/6";
			pageLabel.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[188]; // Donatello
			monster01.Hint = "Donatello";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Donatello";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[189]; // Leonardo
			monster02.Hint = "Leonardo";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Leonardo";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[185]; // Michelangelo
			monster03.Hint = "Michelangelo";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Michelangelo";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[186]; // Raphael
			monster04.Hint = "Raphael";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Raphael";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[169]; // RedTurtle
			monster05.Hint = "RedTurtle";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[170]; // GreenTurtle
			monster06.Hint = "GreenTurtle";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "GreenTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[171]; // BlueTurtle
			monster07.Hint = "BlueTurtle";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlueTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[352]; // ChogTurtle
			monster08.Hint = "ChogTurtle";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ChogTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster09.Library = Libraries.Monsters[187]; // MasterSplinter
			monster09.Hint = "MasterSplinter";
			monster09.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "MasterSplinter";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "14";
				bool flag = WorldMapMobs.User.Level >= 14;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			RefreshMobs(2);
		}

		public void SpiderCave()
		{
			dungeonNameLabel.Text = "SpiderCave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "140+";
			informationArea.Text = "The Spider Cave is located in the East of Seokcho Valley.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to SpiderKing the boss of  Spider Cave.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 140+ Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 100+ Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/6";
			pageLabel.ForeColour = Color.YellowGreen;
			pageLabel.Text = "1/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Visible = false;
			nextButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.SpiderCave2();
				nextButton.Visible = false;
				backButton.Visible = true;
			};

			teleButton6 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton6.Click += delegate (object o, EventArgs e)
			{
				Teleport(6);
			};

			monster01.Library = Libraries.Monsters[204]; // Weaver
			monster01.Hint = "Weaver";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Weaver";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "140";
				bool flag = WorldMapMobs.User.Level >= 140;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[205]; // VenomWeaver
			monster03.Hint = "VenomWeaver";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "VenomWeaver";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "140";
				bool flag = WorldMapMobs.User.Level >= 140;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};


			monster05.Library = Libraries.Monsters[206]; // CrackingWeaver
			monster05.Hint = "CrackingWeaver";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrackingWeaver";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "140";
				bool flag = WorldMapMobs.User.Level >= 140;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[207]; // ArmingWeaver
			monster07.Hint = "ArmingWeaver";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ArmingWeaver";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "140";
				bool flag = WorldMapMobs.User.Level >= 140;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = true;
			pageLabel.Visible = true;

			RefreshMobs(19);
		}

		public void SpiderCave2()
		{
			dungeonNameLabel.Text = "SpiderCave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "140+";
			informationArea.Text = "The Spider Cave is located in the East of Seokcho Valley.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to SpiderKing the boss of Spider Cave.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 140+ Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 100+ Skills";
			drop07.ForeColour = Color.YellowGreen;
			pageLabel.Text = "2/2";
			pageLabel.ForeColour = Color.YellowGreen;

			backButton.Click += (o, e) =>
			{
				GameScene.Scene.WorldMapMobs.Show();
				GameScene.Scene.WorldMapMobs.SpiderCave();
				nextButton.Visible = true;
				backButton.Visible = false;
			};

			monster04.Library = Libraries.Monsters[208]; // CrystalWeaver
			monster04.Hint = "CrystalWeaver";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "CrystalWeaver";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "150";
				bool flag = WorldMapMobs.User.Level >= 150;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[202]; // FlyingStatue
			monster05.Hint = "FlyingStatue";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FlyingStatue";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "180";
				bool flag = WorldMapMobs.User.Level >= 180;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[474]; // OrcMiner
			monster07.Hint = "OrcMiner";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "OrcMiner";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "190";
				bool flag = WorldMapMobs.User.Level >= 140;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[201]; // MasterSplinter
			monster08.Hint = "SpiderKing";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SpiderKing";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "220";
				bool flag = WorldMapMobs.User.Level >= 220;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "GoldBarBundle";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldChest";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "Rare Kitt";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			backButton.Visible = true;
			nextButton.Visible = false;
			pageLabel.Visible = true;

			RefreshMobs(20);
		}

		public void TrollCave()
		{
			dungeonNameLabel.Text = "Troll Cave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "180+";
			informationArea.Text = "The Troll Cave is located in the East of Seokcho Valley.\n\nIt is home to high level monsters suited for high range players.\n\nAlso home to KingOfTrolls the boss of Troll Cave.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 180 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 140+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton8 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton8.Click += delegate (object o, EventArgs e)
			{
				Teleport(8);
			};

			monster01.Library = Libraries.Monsters[374];
			monster01.Hint = "TrollWarrior";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TrollWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "180";
				bool flag = WorldMapMobs.User.Level >= 180;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[375];
			monster02.Hint = "TrollArcher";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TrollArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "180";
				bool flag = WorldMapMobs.User.Level >= 180;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[235];
			monster03.Hint = "TrollBomber";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TrollBomber";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "185";
				bool flag = WorldMapMobs.User.Level >= 185;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[234];
			monster04.Hint = "TrollHammer";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TrollHammer";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "185";
				bool flag = WorldMapMobs.User.Level >= 185;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[236];
			monster05.Hint = "TrollStoner";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TrollStoner";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "185";
				bool flag = WorldMapMobs.User.Level >= 185;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster06.Library = Libraries.Monsters[126];
			monster06.Hint = "QueenOfTrolls";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "QueenOfTrolls";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "180";
				bool flag = WorldMapMobs.User.Level >= 180;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[408];
			monster08.Hint = "KingOfTrolls";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "KingOfTrolls";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "230";
				bool flag = WorldMapMobs.User.Level >= 230;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "GoldBarBundle";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldChest";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "Specail Kitt";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;
			RefreshMobs(21);
		}

		public void HighbossCave()
		{
			dungeonNameLabel.Text = "High World Boss";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "150+";
			informationArea.Text = "";
			dropLabel.Text = "Cave Items";
			drop06.Text = "Rare Kitt";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "Rare Skills";
			drop07.ForeColour = Color.YellowGreen;

			monster01.Library = Libraries.Monsters[152];
			monster01.Hint = "GhastlyLeecher";

			monster02.Library = Libraries.Monsters[153];
			monster02.Hint = "CyanoGhast";

			monster03.Library = Libraries.Monsters[154];
			monster03.Hint = "MutatedManworm";

			monster05.Library = Libraries.Monsters[155]; //CrazyManworm
			monster05.Hint = "CrazyManworm";

			monster06.Library = Libraries.Monsters[159]; //DarkDevourer
			monster06.Hint = "DarkDevourer";

			monster07.Library = Libraries.Monsters[163]; //DreamDevourer
			monster07.Hint = "DreamDevourer";

			monster09.Library = Libraries.Monsters[158]; //Behemoth
			monster09.Hint = "Behemoth";

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(13);
		}

		public void Turtle1Cave()
		{
			dungeonNameLabel.Text = "Turtle Cave";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "200+";
			informationArea.Text = "The Turtle Cave\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to MutatedTurtle the boss of Turtle Temple.";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 200 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 150+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(11);
			};

			monster01.Library = Libraries.Monsters[169];
			monster01.Hint = "RedTurtle";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "RedTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[171];
			monster02.Hint = "BlueTurtle";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "BlueTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "200";
				bool flag = WorldMapMobs.User.Level >= 200;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[170];
			monster03.Hint = "GreenTurtle";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "YellowSnake";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "210";
				bool flag = WorldMapMobs.User.Level >= 210;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[185];
			monster04.Hint = "TowerTurtle";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TowerTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "210";
				bool flag = WorldMapMobs.User.Level >= 210;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};


			monster05.Library = Libraries.Monsters[186];
			monster05.Hint = "FinialTurtle";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FinialTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "210";
				bool flag = WorldMapMobs.User.Level >= 210;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[352];
			monster07.Hint = "TurtleProtector";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TurtleProtector";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "260";
				bool flag = WorldMapMobs.User.Level >= 260;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[187];
			monster08.Hint = "MutatedTurtle";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "MutatedTurtle";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "290";
				bool flag = WorldMapMobs.User.Level >= 290;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(25);
		}

		public void ChaosTemple()
		{
			dungeonNameLabel.Text = "Chaos Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "220+";
			informationArea.Text = "The Chaos Temple\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to KingOfChaos the boss of Chaos Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 220 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 200+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(12);
			};

			monster01.Library = Libraries.Monsters[227];
			monster01.Hint = "ChaosGhost";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ChaosGhost";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "220";
				bool flag = WorldMapMobs.User.Level >= 220;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster02.Library = Libraries.Monsters[342];
			monster02.Hint = "ChaosArcher";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ChaosArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "220";
				bool flag = WorldMapMobs.User.Level >= 220;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[153];
			monster03.Hint = "ChaosZombie";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ChaosZombie";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "220";
				bool flag = WorldMapMobs.User.Level >= 220;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster04.Library = Libraries.Monsters[193];
			monster04.Hint = "ChaosWingedBeast";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ChaosWingedBeast";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "220";
				bool flag = WorldMapMobs.User.Level >= 220;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};


			monster05.Library = Libraries.Monsters[199];
			monster05.Hint = "ChaosYob";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ChaosYob";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "220";
				bool flag = WorldMapMobs.User.Level >= 220;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[194];
			monster07.Hint = "ChaosMaker";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "ChaosMaker";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "290";
				bool flag = WorldMapMobs.User.Level >= 290;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[426];
			monster08.Hint = "KingOfChaos";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "KingOfChaos";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "300";
				bool flag = WorldMapMobs.User.Level >= 300;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(26);
		}

		public void HavocTemple()
		{
			dungeonNameLabel.Text = "Havoc Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "240+";
			informationArea.Text = "The Havoc Temple\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to HavocLord the boss of Havoc Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 240 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 200+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(13);
			};

			monster01.Library = Libraries.Monsters[200];
			monster01.Hint = "HavocMutant";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HavocMutant";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "240";
				bool flag = WorldMapMobs.User.Level >= 240;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[88];
			monster03.Hint = "HavocGhoul";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HavocGhoul";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "240";
				bool flag = WorldMapMobs.User.Level >= 240;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[330];
			monster05.Hint = "HavocWarrior";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HavocWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "240";
				bool flag = WorldMapMobs.User.Level >= 240;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster07.Library = Libraries.Monsters[406];
			monster07.Hint = "HavocKnight";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HavocKnight";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[405];
			monster08.Hint = "HavocLord";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HavocLord";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "450";
				bool flag = WorldMapMobs.User.Level >= 450;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(27);
		}

		public void HellTemple()
		{
			dungeonNameLabel.Text = "Hell Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "260+";
			informationArea.Text = "The Hell Temple\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to HellLOrd the boss of Hell Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 260 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 200+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(14);
			};

			monster01.Library = Libraries.Monsters[216];
			monster01.Hint = "HellPirate";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HellPirate";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "260";
				bool flag = WorldMapMobs.User.Level >= 260;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster03.Library = Libraries.Monsters[215];
			monster03.Hint = "HellSlasher";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HellSlasher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "260";
				bool flag = WorldMapMobs.User.Level >= 260;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[217];
			monster05.Hint = "HellCannibal";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HellCannibal";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "260";
				bool flag = WorldMapMobs.User.Level >= 260;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[411];
			monster07.Hint = "HellProtector";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HellProtector";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[452];
			monster08.Hint = "HellLOrd";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "HellLOrd";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "450";
				bool flag = WorldMapMobs.User.Level >= 450;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(27);
		}

		public void OrcalLand()
		{
			dungeonNameLabel.Text = "Orcal Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "280+";
			informationArea.Text = "The Orcal Temple\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to OrcalLord the boss of Orcal Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 280 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 200+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(16);
			};
			monster03.Library = Libraries.Monsters[303];
			monster03.Hint = "OrcalSlime";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "OrcalSlime";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "280";
				bool flag = WorldMapMobs.User.Level >= 280;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};

			monster05.Library = Libraries.Monsters[299];
			monster05.Hint = "OrcalWarrior";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "OrcalWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "280";
				bool flag = WorldMapMobs.User.Level >= 280;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[301];
			monster07.Hint = "OrcalProtector";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "OrcalProtector";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[302];
			monster08.Hint = "OrcalLord";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "OrcalLord";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "450";
				bool flag = WorldMapMobs.User.Level >= 450;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(29);
		}

		public void AzureTemple()
		{
			dungeonNameLabel.Text = "Azure Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "300+";
			informationArea.Text = "The Azure Temple\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to Azure King the boss of Azure Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 300 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 200+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(17);
			};
			monster01.Library = Libraries.Monsters[341];
			monster01.Hint = "AzureArcher";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "AzureArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "300";
				bool flag = WorldMapMobs.User.Level >= 300;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[346];
			monster03.Hint = "AzureSorceror";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "AzureSorceror";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "300";
				bool flag = WorldMapMobs.User.Level >= 300;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster05.Library = Libraries.Monsters[339];
			monster05.Hint = "AzureMage";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "AzureMage";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "300";
				bool flag = WorldMapMobs.User.Level >= 300;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[343];
			monster06.Hint = "AzureWarrior";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "AzureWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "300";
				bool flag = WorldMapMobs.User.Level >= 300;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[301];
			monster07.Hint = "AzureKnight";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "AzureKnight";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[348];
			monster08.Hint = "AzureKing";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "AzureKing";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "450";
				bool flag = WorldMapMobs.User.Level >= 450;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(30);
		}

		public void LandOfIllusion()
		{

			dungeonNameLabel.Text = "The Land Of Illusion";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "320";
			informationArea.Text = "The Land Of Illusion \n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to KingKingOfillusion";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 320 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 200+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton3 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton3.Click += delegate (object o, EventArgs e)
			{
				Teleport(18);
			};

			monster01.Library = Libraries.Monsters[286];
			monster01.Hint = "Tucson22";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "Tucson22";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "320";
				bool flag = WorldMapMobs.User.Level >= 320;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster02.Library = Libraries.Monsters[287];
			monster02.Hint = "TucsonFighter22";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TucsonFighter22";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "320";
				bool flag = WorldMapMobs.User.Level >= 320;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[288];
			monster03.Hint = "TucsonMage22";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TucsonMage22";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "320";
				bool flag = WorldMapMobs.User.Level >= 320;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};
			monster04.Library = Libraries.Monsters[289];
			monster04.Hint = "TucsonWarrior22";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "TucsonWarrior22";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "320";
				bool flag = WorldMapMobs.User.Level >= 320;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[415];
			monster06.Hint = "QueenOfillusion";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "QueenOfillusion";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[296];
			monster08.Hint = "KingOfillusion";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "KingKingOfillusion";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "450";
				bool flag = WorldMapMobs.User.Level >= 450;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(31);
		}

		public void GhostLand()
		{

			dungeonNameLabel.Text = "The Ghost Land";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "340";
			informationArea.Text = "The Ghost Land \n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to SoulTaker";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 340 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 200+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton3 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton3.Click += delegate (object o, EventArgs e)
			{
				Teleport(19);
			};

			monster02.Library = Libraries.Monsters[422];
			monster02.Hint = "IceGhost";
			monster02.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "IceGhost";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "340";
				bool flag = WorldMapMobs.User.Level >= 340;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster04.Library = Libraries.Monsters[423];
			monster04.Hint = "FireGhost";
			monster04.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FireGhost";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "340";
				bool flag = WorldMapMobs.User.Level >= 340;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[424];
			monster06.Hint = "EvilSpirit";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilSpirit";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
			};

			monster08.Library = Libraries.Monsters[425];
			monster08.Hint = "SoulTaker";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SoulTaker";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "450";
				bool flag = WorldMapMobs.User.Level >= 450;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(32);
		}

		public void FrozenTemple()
		{
			dungeonNameLabel.Text = "Frozen Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "360+";
			informationArea.Text = "The Frozen Temple\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to Frozen Lord the boss of Frozen Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 360 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 200+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(20);
			};
			monster01.Library = Libraries.Monsters[365];
			monster01.Hint = "FrozenArcher";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "360";
				bool flag = WorldMapMobs.User.Level >= 360;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[380];
			monster03.Hint = "FrozenMiner";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenMiner";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "360";
				bool flag = WorldMapMobs.User.Level >= 360;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster05.Library = Libraries.Monsters[381];
			monster05.Hint = "FrozenAxeman";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenAxeman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "360";
				bool flag = WorldMapMobs.User.Level >= 360;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[383];
			monster06.Hint = "SnowYeti";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SnowYeti";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "360";
				bool flag = WorldMapMobs.User.Level >= 360;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[369];
			monster07.Hint = "SnowWolf";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "SnowWolf";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "430";
				bool flag = WorldMapMobs.User.Level >= 430;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[384];
			monster08.Hint = "FrozenLord";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "FrozenLord";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "480";
				bool flag = WorldMapMobs.User.Level >= 480;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(30);
		}

		public void EvilTemple()
		{
			dungeonNameLabel.Text = "Evil Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "380+";
			informationArea.Text = "The Evil Temple\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to Evil Beast the boss of Evil Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 380 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 200+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(21);
			};
			monster01.Library = Libraries.Monsters[426];
			monster01.Hint = "EvilWarrior";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "380";
				bool flag = WorldMapMobs.User.Level >= 380;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[427];
			monster03.Hint = "EvilWingedFleshEater";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilWingedFleshEater";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "380";
				bool flag = WorldMapMobs.User.Level >= 380;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster05.Library = Libraries.Monsters[428];
			monster05.Hint = "EvilSpearman";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilSpearman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "380";
				bool flag = WorldMapMobs.User.Level >= 380;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[430];
			monster06.Hint = "EvilAssassin";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilAssassin";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "380";
				bool flag = WorldMapMobs.User.Level >= 380;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[429];
			monster07.Hint = "EvilHound";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilHound";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "450";
				bool flag = WorldMapMobs.User.Level >= 450;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[431];
			monster08.Hint = "EvilBeast";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "EvilBeast";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "500";
				bool flag = WorldMapMobs.User.Level >= 500;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(30);
		}

		public void LostTemple()
		{
			dungeonNameLabel.Text = "Lost Temple";
			informationLabel.Text = "Location Information:";
			suggestedLevelLabel.Text = "Suggested Level:";
			suggestedLevel.Text = "400+";
			informationArea.Text = "The Lost Temple\n\nIt is home to higher level monsters suited for  higer range players,\n\nAlso home to Lost King the boss of Lost Temple";
			dropLabel.Text = "Cave Items";
			drop06.Text = "level 400 Items";
			drop06.ForeColour = Color.YellowGreen;
			drop07.Text = "level 300+ Skills";
			drop07.ForeColour = Color.YellowGreen;

			teleButton5 = new MirButton
			{
				Index = 850,
				HoverIndex = 851,
				PressedIndex = 852,
				Location = new Point(300, 393),
				Library = Libraries.Title,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Teleport"
			};
			teleButton5.Click += delegate (object o, EventArgs e)
			{
				Teleport(22);
			};
			monster01.Library = Libraries.Monsters[451];
			monster01.Hint = "LostWarrior";
			monster01.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "LostWarrior";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster03.Library = Libraries.Monsters[452];
			monster03.Hint = "LostArcher";
			monster03.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "LostArcher";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster05.Library = Libraries.Monsters[453];
			monster05.Hint = "LostSpearman";
			monster05.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "LostSpearman";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster06.Library = Libraries.Monsters[454];
			monster06.Hint = "LostGiantBat";
			monster06.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "LostGiantBat";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "400";
				bool flag = WorldMapMobs.User.Level >= 400;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Quest Drops";
				drop01.Text = "";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster07.Library = Libraries.Monsters[386];
			monster07.Hint = "LostQueen";
			monster07.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "LostQueen";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "480";
				bool flag = WorldMapMobs.User.Level >= 480;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Red;
				}
				informationArea.Text = "";
				questdropLabel.Text = "SubBoss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "Orbs";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "Books";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Normal Kitt";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "";
				drop05.ForeColour = Color.Cyan;
				drop06.Text = "";
				drop06.ForeColour = Color.Cyan;
				drop07.Text = "";
				drop07.ForeColour = Color.Cyan;
			};
			monster08.Library = Libraries.Monsters[320];
			monster08.Hint = "LostKing";
			monster08.Click += delegate (object o, EventArgs e)
			{
				questdropLabel.Visible = true;
				dropLabel.Visible = false;
				informationLabel.Text = "LostKing";
				suggestedLevelLabel.Text = "Level:";
				suggestedLevel.Text = "550";
				bool flag = WorldMapMobs.User.Level >= 550;
				if (flag)
				{
					suggestedLevel.ForeColour = Color.Green;
				}
				else
				{
					suggestedLevel.ForeColour = Color.Yellow;
				}
				informationArea.Text = "";
				questdropLabel.Text = "Boss Drops";
				drop01.Text = "Gold Bar";
				drop01.ForeColour = Color.Cyan;
				drop02.Text = "GoldBarBundle";
				drop02.ForeColour = Color.Cyan;
				drop03.Text = "GoldChest";
				drop03.ForeColour = Color.Cyan;
				drop04.Text = "Orbs";
				drop04.ForeColour = Color.Cyan;
				drop05.Text = "SpecailKitt";
				drop05.ForeColour = Color.Cyan;
			};

			backButton.Visible = false;
			nextButton.Visible = false;
			pageLabel.Visible = false;

			RefreshMobs(30);
		}

		private void RefreshMobs(byte STabid)
		{
			switch (STabid)
			{
				case 0:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 1:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 2:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 3:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 4:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 5:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 6: //			RefreshMobs(6) Snakes
					monster01.Visible = false;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = false;
					monster08.Visible = false;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 7:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = false;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = true;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 8:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 9:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = true;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 10:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = false;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = true;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 11:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = false;
					monster04.Visible = false;
					monster05.Visible = false;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = true;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 12:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = false;
					monster05.Visible = false;
					monster06.Visible = false;
					monster07.Visible = false;
					monster08.Visible = false;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = true;
					monster15.Visible = true;
					monster16.Visible = true;
					monster17.Visible = true;
					monster18.Visible = true;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 13:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 14:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = false;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = true;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 15:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 16:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 17:
					monster01.Visible = false;
					monster02.Visible = false;
					monster03.Visible = false;
					monster04.Visible = false;
					monster05.Visible = false;
					monster06.Visible = false;
					monster07.Visible = false;
					monster08.Visible = false;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = true;
					monster02s.Visible = true;
					monster03s.Visible = true;
					monster05s.Visible = true;
					monster06s.Visible = true;
					monster07s.Visible = true;
					monster09s.Visible = false;
					break;
				case 18:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = true;
					break;
				case 19:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = false;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 20:
					monster01.Visible = false;
					monster02.Visible = false;
					monster03.Visible = false;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 21:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = false;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 22:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 23:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 24:
					monster01.Visible = false;
					monster02.Visible = true;
					monster03.Visible = false;
					monster04.Visible = false;
					monster05.Visible = false;
					monster06.Visible = false;
					monster07.Visible = false;
					monster08.Visible = false;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 25:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 26:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 27:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 28:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = true;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 29:
					monster01.Visible = false;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = false;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 30:
					monster01.Visible = true;
					monster02.Visible = false;
					monster03.Visible = true;
					monster04.Visible = false;
					monster05.Visible = true;
					monster06.Visible = true;
					monster07.Visible = true;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 31:
					monster01.Visible = true;
					monster02.Visible = true;
					monster03.Visible = true;
					monster04.Visible = true;
					monster05.Visible = false;
					monster06.Visible = true;
					monster07.Visible = false;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
				case 32:
					monster01.Visible = false;
					monster02.Visible = true;
					monster03.Visible = false;
					monster04.Visible = true;
					monster05.Visible = false;
					monster06.Visible = true;
					monster07.Visible = false;
					monster08.Visible = true;
					monster09.Visible = false;
					monster10.Visible = false;
					monster11.Visible = false;
					monster12.Visible = false;
					monster13.Visible = false;
					monster14.Visible = false;
					monster15.Visible = false;
					monster16.Visible = false;
					monster17.Visible = false;
					monster18.Visible = false;
					monster19.Visible = false;
					dropLabel.Visible = true;
					questdropLabel.Visible = false;
					drop01.Text = "";
					drop02.Text = "";
					drop03.Text = "";
					drop04.Text = "";
					drop05.Text = "";
					drop08.Text = "";
					drop09.Text = "";
					drop10.Text = "";
					drop11.Text = "";
					monster01s.Visible = false;
					monster02s.Visible = false;
					monster03s.Visible = false;
					monster05s.Visible = false;
					monster06s.Visible = false;
					monster07s.Visible = false;
					monster09s.Visible = false;
					break;
			}
		}

		public void Teleport(byte TPabid)
		{
			switch (TPabid)
			{
				case 0:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + "BugCave20Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 1:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " WoomaTemple50Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 2:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " ZumaTemple70Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 3:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " ZombieCave90Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 4:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " PrajnaTemple110Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 5:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " SnakeCave120Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 6:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " SpiderCave140Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 7:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " IceCave160Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 8:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " TrollCave180Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 9:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " LowWorldBossCaveLOWSTONE" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 10:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " MedWorldBossCaveMEDSTONE" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 11:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " TurtleTemple200Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 12:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " ChaosTemple220Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 13:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " HavocTemple240Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 14:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " HellTemple260Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 15:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " HighWorldBossCaveHighSTONE" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 16:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " OrcalTemple260Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 17:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " AzureTemple300Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 18:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " LandOfIllusion320Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 19:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " GhostLand340Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 20:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " FrozenLand360Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 21:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " EvilTemple380Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
				case 22:
					Network.Enqueue(new C.Chat { Message = "@TRIGGER " + User.Name + " LostTemple400Stone" });
					GameScene.Scene.WorldMapMobs.Hide();
					GameScene.Scene.WorldMapDialog.Hide();
					break;
			}
		}

		public void Show()
		{
			Visible = true;
		}

		public void Hide()
		{
			Visible = false;
		}
	}
}