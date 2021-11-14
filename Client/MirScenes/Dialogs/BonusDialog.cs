using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using ClientPackets;
using C = ClientPackets;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Client.MirScenes.Dialogs
{
	public sealed class BonusDialog : MirImageControl
	{
		public MirLabel AP, MaxHPLabel, MaxMPLabel, MinACLabel, MaxACLabel, MinMACLabel, MaxMACLabel, MinDCLabel, MaxDCLabel, MinMCLabel, MaxMCLabel, MinSCLabel, MaxSCLabel;
		public MirLabel MatterFireLabel, MatterEarthLabel, MatterThunderLabel, MatterWaterLabel, MatterLightLabel, MatterDarkLabel;
		public MirButton MaxHPUpButton, MaxMPUpButton, MinACUpButton, MaxACUpButton, MinMACUpButton, MaxMACUpButton, MinDCUpButton, MaxDCUpButton, MinMCUpButton, MaxMCUpButton, MinSCUpButton, MaxSCUpButton, CloseButton;
		public MirButton MatterFireUpButton, MatterEarthUpButton, MatterThunderUpButton, MatterWaterUpButton, MatterLightUpButton, MatterDarkUpButton;

		public Dictionary<string, int> Costs = new Dictionary<string, int>();

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

		public void RefreshLabels()
		{
			AP.Text = GameScene.User.CurrentAttributesAvailable.ToString();
			MaxHPLabel.Text = GameScene.User.Attributes.GetValue("HP").ToString();
			MaxMPLabel.Text = GameScene.User.Attributes.GetValue("MP").ToString();
			MinACLabel.Text = GameScene.User.Attributes.GetValue("MinAC").ToString();
			MaxACLabel.Text = GameScene.User.Attributes.GetValue("MaxAC").ToString();
			MinMACLabel.Text = GameScene.User.Attributes.GetValue("MinMAC").ToString();
			MaxMACLabel.Text = GameScene.User.Attributes.GetValue("MaxMAC").ToString();
			MinDCLabel.Text = GameScene.User.Attributes.GetValue("MinDC").ToString();
			MaxDCLabel.Text = GameScene.User.Attributes.GetValue("MaxDC").ToString();
			MinMCLabel.Text = GameScene.User.Attributes.GetValue("MinMC").ToString();
			MaxMCLabel.Text = GameScene.User.Attributes.GetValue("MaxMC").ToString();
			MinSCLabel.Text = GameScene.User.Attributes.GetValue("MinSC").ToString();
			MaxSCLabel.Text = GameScene.User.Attributes.GetValue("MaxSC").ToString();
			//MatterFireLabel.Text = GameScene.User.Attributes.GetValue("Fire").ToString();
			//MatterEarthLabel.Text = GameScene.User.Attributes.GetValue("Earth").ToString();
			//MatterThunderLabel.Text = GameScene.User.Attributes.GetValue("Thunder").ToString();
			//MatterWaterLabel.Text = GameScene.User.Attributes.GetValue("Water").ToString();
			//MatterLightLabel.Text = GameScene.User.Attributes.GetValue("Light").ToString();
			//MatterDarkLabel.Text = GameScene.User.Attributes.GetValue("Dark").ToString();


		}

		public void UpdateHints()
		{
			MaxHPUpButton.Hint = "Increase attribute by 10 (Costs 5 AP)";
			MaxMPUpButton.Hint = "Increase attribute by 10 (Costs 5 AP)";
			MinACUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MinAC") + " AP)";
			MaxACUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MaxAC") + " AP)";
			MinMACUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MinMAC") + " AP)";
			MaxMACUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MaxMAC") + " AP)";
			MinDCUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MinDC") + " AP)";
			MaxDCUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MaxDC") + " AP)";
			MinMCUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MinMC") + " AP)";
			MaxMCUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MaxMC") + " AP)";
			MinSCUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MinSC") + " AP)";
			MaxSCUpButton.Hint = "Increase attribute by 1 (Costs " + GetCost("MaxSC") + " AP)";
		}

		public BonusDialog()
		{
			Index = 0;
			Library = Libraries.CustomApSystem;
			Location = base.Center;
			Parent = GameScene.Scene;
			Visible = true;
			Movable = true;
			Sort = true;

			MaxHPLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(480, 70),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(61, 15)
			};

			MaxHPUpButton = new MirButton
			{
				Index = 6,
				HoverIndex = 7,
				PressedIndex = 8,
				Location = new Point(206, 70),
				Library = Libraries.CustomApSystem,
				Parent = this,
				Sound = SoundList.ButtonA,
			};
			MaxHPUpButton.Click += (o, e) =>
			{
				AddAttribute("HP", 5, e);
			};

			MaxMPLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(480, 89),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(61, 15)
			};

			MaxMPUpButton = new MirButton
			{
				Index = 12,
				HoverIndex = 13,
				PressedIndex = 14,
				Location = new Point(274, 104),
				Library = (Library = Libraries.CustomApSystem),
				Parent = this,
				Sound = SoundList.ButtonA,
			};
			MaxMPUpButton.Click += (o, e) =>
			{
				AddAttribute("MP", 5, e);
			};

			MinACLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 292),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15)
			};

			MinACUpButton = new MirButton
			{
				Index = 18,
				HoverIndex = 19,
				PressedIndex = 20,
				Location = new Point(314, 245),
				Library = (Library = Libraries.CustomApSystem),
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MinACUpButton.Click += (o, e) =>
			{
				AddAttribute("MinAC", 1, e);
			};

			MaxACLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 311),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15)
			};

			MaxACUpButton = new MirButton
			{
				Index = 24,
				HoverIndex = 25,
				PressedIndex = 26,
				Location = new Point(312, 171),
				Library = (Library = Libraries.CustomApSystem),
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MaxACUpButton.Click += (o, e) =>
			{
				AddAttribute("MaxAC", 1, e);
			};

			MinMACLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 330),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15)
			};

			MinMACUpButton = new MirButton
			{
				Index = 30,
				HoverIndex = 31,
				PressedIndex = 32,
				Location = new Point(276, 313),
				Library = (Library = Libraries.CustomApSystem),
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MinMACUpButton.Click += (o, e) =>
			{
				AddAttribute("MinMAC", 1, e);
			};

			MaxMACLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 349),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15)
			};

			MaxMACUpButton = new MirButton
			{
				Index = 36,
				HoverIndex = 37,
				PressedIndex = 38,
				Location = new Point(212, 350),
				Library = (Library = Libraries.CustomApSystem),
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MaxMACUpButton.Click += (o, e) =>
			{
				AddAttribute("MaxMAC", 1, e);
			};

			MinDCLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 143),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15)
			};

			MinDCUpButton = new MirButton
			{
				Index = 72,
				HoverIndex = 73,
				PressedIndex = 74,
				Location = new Point(129, 69),
				Library = Libraries.CustomApSystem,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MinDCUpButton.Click += (o, e) =>
			{
				AddAttribute("MinDC", 1, e);
			};

			MaxDCLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 160),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15),
			};

			MaxDCUpButton = new MirButton
			{
				Index = 66,
				HoverIndex = 67,
				PressedIndex = 68,
				Location = new Point(64, 110),
				Library = Libraries.CustomApSystem,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MaxDCUpButton.Click += (o, e) =>
			{
				AddAttribute("MaxDC", 1, e);
			};

			MinMCLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 180),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15)
			};

			MinMCUpButton = new MirButton
			{
				Index = 60,
				HoverIndex = 61,
				PressedIndex = 62,
				Location = new Point(29, 173),
				Library = Libraries.CustomApSystem,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MinMCUpButton.Click += (o, e) =>
			{
				AddAttribute("MinMC", 1, e);
			};

			MaxMCLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 199),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15)
			};

			MaxMCUpButton = new MirButton
			{
				Index = 54,
				HoverIndex = 55,
				PressedIndex = 56,
				Location = new Point(29, 252),
				Library = Libraries.CustomApSystem,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MaxMCUpButton.Click += (o, e) =>
			{
				AddAttribute("MaxMC", 1, e);
			};

			MinSCLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 218),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15)
			};

			MinSCUpButton = new MirButton
			{
				Index = 48,
				HoverIndex = 49,
				PressedIndex = 50,
				Location = new Point(70, 316),
				Library = Libraries.CustomApSystem,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MinSCUpButton.Click += (o, e) =>
			{
				AddAttribute("MinSC", 1, e);
			};

			MaxSCLabel = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(510, 237),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(31, 15)
			};

			MaxSCUpButton = new MirButton
			{
				Index = 42,
				HoverIndex = 43,
				PressedIndex = 44,
				Location = new Point(134, 354),
				Library = Libraries.CustomApSystem,
				Parent = this,
				Sound = SoundList.ButtonA,
				Hint = "Increase attribute by 1"
			};
			MaxSCUpButton.Click += (o, e) =>
			{
				AddAttribute("MaxSC", 1, e);
			};

			//         MatterFireLabel = new MirLabel
			//{
			//	Parent = this,
			//	NotControl = true,
			//	Text = "0",
			//	Location = new Point(510, 278),
			//	DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
			//	Size = new Size(31, 15)
			//};

			//MatterFireUpButton = new MirButton
			//{
			//	Index = 84,
			//	HoverIndex = 85,
			//	PressedIndex = 86,
			//	Location = new Point(92, 170),
			//	Library = Libraries.CustomApSystem,
			//	Parent = this,
			//	Sound = SoundList.ButtonA,
			//	Hint = "Increase attribute by 1"
			//};
			//         MatterFireUpButton.Click += (o, e) =>
			//         {
			//             AddAttribute("Fire", 1, e);
			//         };

			//         MatterEarthLabel = new MirLabel
			//{
			//	Parent = this,
			//	NotControl = true,
			//	Text = "0",
			//	Location = new Point(510, 297),
			//	DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
			//	Size = new Size(31, 15)
			//};

			//MatterEarthUpButton = new MirButton
			//{
			//	Index = 90,
			//	HoverIndex = 91,
			//	PressedIndex = 92,
			//	Location = new Point(245, 170),
			//	Library = Libraries.CustomApSystem,
			//	Parent = this,
			//	Sound = SoundList.ButtonA,
			//	Hint = "Increase attribute by 1"
			//};
			//         MatterEarthUpButton.Click += (o, e) =>
			//         {
			//             AddAttribute("Earth", 1, e);
			//         };

			//         MatterThunderLabel = new MirLabel
			//{
			//	Parent = this,
			//	NotControl = true,
			//	Text = "0",
			//	Location = new Point(510, 316),
			//	DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
			//	Size = new Size(31, 15)
			//};

			//MatterThunderUpButton = new MirButton
			//{
			//	Index = 102,
			//	HoverIndex = 103,
			//	PressedIndex = 104,
			//	Location = new Point(245, 258),
			//	Library = Libraries.CustomApSystem,
			//	Parent = this,
			//	Sound = SoundList.ButtonA,
			//	Hint = "Increase attribute by 1"
			//};
			//         MatterThunderUpButton.Click += (o, e) =>
			//         {
			//             AddAttribute("Thunder", 1, e);
			//         };

			//         MatterWaterLabel = new MirLabel
			//{
			//	Parent = this,
			//	NotControl = true,
			//	Text = "0",
			//	Location = new Point(510, 335),
			//	DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
			//	Size = new Size(31, 15)
			//};

			//MatterWaterUpButton = new MirButton
			//{
			//	Index = 96,
			//	HoverIndex = 97,
			//	PressedIndex = 98,
			//	Location = new Point(92, 258),
			//	Library = Libraries.CustomApSystem,
			//	Parent = this,
			//	Sound = SoundList.ButtonA,
			//	Hint = "Increase attribute by 1"
			//};
			//         MatterWaterUpButton.Click += (o, e) =>
			//         {
			//             AddAttribute("Water", 1, e);
			//         };

			//         MatterLightLabel = new MirLabel
			//{
			//	Parent = this,
			//	NotControl = true,
			//	Text = "0",
			//	Location = new Point(510, 354),
			//	DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
			//	Size = new Size(31, 15)
			//};

			//MatterLightUpButton = new MirButton
			//{
			//	Index = 108,
			//	HoverIndex = 109,
			//	PressedIndex = 110,
			//	Location = new Point(169, 121),
			//	Library = Libraries.CustomApSystem,
			//	Parent = this,
			//	Sound = SoundList.ButtonA,
			//	Hint = "Increase attribute by 1"
			//};
			//         MatterLightUpButton.Click += (o, e) =>
			//         {
			//             AddAttribute("Light", 1, e);
			//         };

			//         MatterDarkLabel = new MirLabel
			//{
			//	Parent = this,
			//	NotControl = true,
			//	Text = "0",
			//	Location = new Point(510, 373),
			//	DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
			//	Size = new Size(31, 15)
			//};

			//MatterDarkUpButton = new MirButton
			//{
			//	Index = 114,
			//	HoverIndex = 115,
			//	PressedIndex = 116,
			//	Location = new Point(169, 304),
			//	Library = Libraries.CustomApSystem,
			//	Parent = this,
			//	Sound = SoundList.ButtonA,
			//	Hint = "Increase attribute by 1"
			//};
			//         MatterDarkUpButton.Click += (o, e) =>
			//         {
			//             AddAttribute("Dark", 1, e);
			//         };

			AP = new MirLabel
			{
				Parent = this,
				NotControl = true,
				Text = "0",
				Location = new Point(480, 413),
				DrawFormat = (TextFormatFlags.Right | TextFormatFlags.VerticalCenter),
				Size = new Size(61, 15)
			};

			CloseButton = new MirButton
			{
				Index = 361,
				HoverIndex = 362,
				PressedIndex = 363,
				Location = new Point(530, 8),
				Library = Libraries.Prguse,
				Parent = this,
				Sound = SoundList.ButtonA
			};
			CloseButton.Click += delegate (object o, EventArgs e)
			{
				Hide();
			};
		}

		private int GetCost(string stat)
		{
			try
			{
				return Costs[stat];
			}
			catch
			{
				return 1;
			}
		}

		private void AddAttribute(string attribute, int value, EventArgs e)
		{
			int val = value;

			//Remove attribute when right clicking
			MouseEventArgs me = e as MouseEventArgs;
			if (me != null && me.Button == MouseButtons.Right)
			{
				val = val * -1;
			}

			Network.Enqueue(new C.AddAttribute { Attribute = attribute, Value = val });
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

			RefreshLabels();
		}
	}
}
