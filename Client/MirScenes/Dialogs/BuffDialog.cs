using System;
using System.Collections.Generic;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System.Drawing;
using System.Windows.Forms;

namespace Client.MirScenes.Dialogs
{
    public sealed class BuffDialog : MirImageControl
    {
        private MirButton _expandCollapseButton;
        private MirLabel _buffCountLabel;
        private List<MirImageControl> _buffList = new List<MirImageControl>();
        private bool _fadedOut, _fadedIn;
        private int _buffCount;
        private long _nextFadeTime;

        private const long FadeDelay = 55;
        private const float FadeRate = 0.2f;

        public BuffDialog()
        {
            Index = 20;
            Library = Libraries.Prguse2;
            Movable = false;
            Size = new Size(44, 34);
            Location = new Point(Settings.ScreenWidth - 170, 0);
            Sort = true;

            Opacity = 0f;
            _fadedOut = true;

            _expandCollapseButton = new MirButton
            {
                Index = 7,
                HoverIndex = 8,
                Size = new Size(16, 15),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 9,
                Sound = SoundList.ButtonA,
                Opacity = 0f
            };
            _expandCollapseButton.Click += (o, e) =>
            {
                if (_buffCount == 1)
                {
                    Settings.ExpandedBuffWindow = true;
                }
                else
                {
                    Settings.ExpandedBuffWindow = !Settings.ExpandedBuffWindow;
                }

                UpdateWindow();
            };

            _buffCountLabel = new MirLabel
            {
                Parent = this,
                AutoSize = true,
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                NotControl = true,
                Sort = false,
                Visible = false,
                ForeColour = Color.Yellow,
                OutLineColour = Color.Black,
            };
        }

        public void CreateBuff(ClientBuff buff)
        {
            var buffImage = BuffImage(buff.Type);

            var buffLibrary = Libraries.BuffIcon;

            if (buffImage >= 20000)
            {
                buffImage -= 20000;
                buffLibrary = Libraries.MagIcon;
            }

            if (buffImage >= 10000)
            {
                buffImage -= 10000;
                buffLibrary = Libraries.Prguse2;
            }

            var image = new MirImageControl
            {
                Library = buffLibrary,
                Parent = this,
                Visible = true,
                Sort = false,
                Index = buffImage
            };

            _buffList.Insert(0, image);
            UpdateWindow();
        }

        public void RemoveBuff(int buffId)
        {
            _buffList[buffId].Dispose();
            _buffList.RemoveAt(buffId);

            UpdateWindow();
        }

        public void Process()
        {
            if (!Visible) return;

            if (_buffList.Count != _buffCount)
                UpdateWindow();

            for (var i = 0; i < _buffList.Count; i++)
            {
                var image = _buffList[i];
                var buff = GameScene.Scene.Buffs[i];

                var buffImage = BuffImage(buff.Type);
                var buffLibrary = Libraries.BuffIcon;

                //ArcherSpells - VampireShot,PoisonShot
                if (buffImage >= 20000)
                {
                    buffImage -= 20000;
                    buffLibrary = Libraries.MagIcon;
                }

                if (buffImage >= 10000)
                {
                    buffImage -= 10000;
                    buffLibrary = Libraries.Prguse2;
                }

                image.Location = new Point(Size.Width - 10 - 23 - (i * 23) + ((10 * 23) * (i / 10)), 6 + ((i / 10) * 24));
                image.Hint = Settings.ExpandedBuffWindow ? BuffString(buff) : CombinedBuffText();
                image.Index = buffImage;
                image.Library = buffLibrary;

                if (Settings.ExpandedBuffWindow || !Settings.ExpandedBuffWindow && i == 0)
                {
                    image.Visible = true;
                    image.Opacity = 1f;
                }
                else
                {
                    image.Visible = false;
                    image.Opacity = 0.6f;
                }

                if (buff.Infinite || !(Math.Round((buff.ExpireTime - CMain.Time) / 1000D) <= 5))
                    continue;

                var time = (buff.ExpireTime - CMain.Time) / 100D;

                if (Math.Round(time) % 10 < 5)
                    image.Index = -1;
            }

            if (IsMouseOver(CMain.MPoint))
            { 
                if (_buffCount == 0 || (!_fadedIn && CMain.Time <= _nextFadeTime))
                    return;

                Opacity += FadeRate;
                _expandCollapseButton.Opacity += FadeRate;

                if (Opacity > 1f)
                {
                    Opacity = 1f;
                    _expandCollapseButton.Opacity = 1f;
                    _fadedIn = true;
                    _fadedOut = false;
                }

                _nextFadeTime = CMain.Time + FadeDelay;
            }
            else
            {
                if (!_fadedOut && CMain.Time <= _nextFadeTime)
                    return;

                Opacity -= FadeRate;
                _expandCollapseButton.Opacity -= FadeRate;

                if (Opacity < 0f)
                {
                    Opacity = 0f;
                    _expandCollapseButton.Opacity = 0f;
                    _fadedOut = true;
                    _fadedIn = false;
                }
                    
                _nextFadeTime = CMain.Time + FadeDelay;
            }
        }

        private void UpdateWindow()
        {
            _buffCount = _buffList.Count;

            if (_buffCount > 0 && Settings.ExpandedBuffWindow)
            {
                var oldWidth = Size.Width;

                if (_buffCount <= 10)
                    Index = 20 + _buffCount - 1;
                else if (_buffCount > 10)
                    Index = 20 + 10;
                else if (_buffCount > 20)
                    Index = 20 + 11;
                else if (_buffCount > 30)
                    Index = 20 + 12;
                else if (_buffCount > 40)
                    Index = 20 + 13;

                var newX = Location.X - Size.Width + oldWidth;
                var newY = Location.Y;
                Location = new Point(newX, newY);

                _buffCountLabel.Visible = false;

                _expandCollapseButton.Location = new Point(Size.Width - 15, 0);
                Size = new Size((_buffCount > 10 ? 10 : _buffCount) * 23, 24 + (_buffCount / 10) * 24);
            }
            else if (_buffCount > 0 && !Settings.ExpandedBuffWindow)
            {
                var oldWidth = Size.Width;

                Index = 20;
            
                var newX = Location.X - Size.Width + oldWidth;
                var newY = Location.Y;
                Location = new Point(newX, newY);

                _buffCountLabel.Visible = true;
                _buffCountLabel.Text = $"{_buffCount}";
                _buffCountLabel.Location = new Point(Size.Width / 2 - _buffCountLabel.Size.Width / 2, Size.Height / 2 - 10);
                _buffCountLabel.BringToFront();

                _expandCollapseButton.Location = new Point(Size.Width - 15, 0);
                Size = new Size(44, 34);
            }
        }




        public string BuffString(ClientBuff buff)
        {
            string text = RegexFunctions.SeperateCamelCase(buff.Type.ToString()) + "\n";
            bool overridestats = false;

            switch (buff.Type)
            {
                case BuffType.GameMaster:
                    GMOptions options = (GMOptions)buff.Values[0];

                    if (options.HasFlag(GMOptions.GameMaster)) text += "-Invisible\n";
                    if (options.HasFlag(GMOptions.Superman)) text += "-Superman\n";
                    if (options.HasFlag(GMOptions.Observer)) text += "-Observer\n";
                    break;
                case BuffType.MentalState:
                    switch (buff.Values[0])
                    {
                        case 0:
                            text += "Agressive (Full damage)\nCan't shoot over walls.\n";
                            break;
                        case 1:
                            text += "Trick shot (Minimal damage)\nCan shoot over walls.\n";
                            break;
                        case 2:
                            text += "Group Mode (Medium damage)\nDon't steal agro.\n";
                            break;
                    }
                    break;
                case BuffType.Hiding:
                case BuffType.ClearRing:
                    text += "Invisible to many monsters.\n";
                    break;
                case BuffType.MoonLight:
                    text += "Invisible to players and many\nmonsters when at a distance.\n";
                    break;
                case BuffType.EnergyShield:
                    overridestats = true;
                    text += string.Format("{0}% chance to gain {1} HP when attacked.\n", buff.Stats[Stat.EnergyShieldPercent], buff.Stats[Stat.EnergyShieldHPGain]);
                    break;
                case BuffType.DarkBody:
                    text += "Invisible to many monsters and able to move.\n";
                    break;
                case BuffType.VampireShot:
                    text += "Gives you a vampiric ability\nthat can be released with\ncertain skills.\n";
                    break;
                case BuffType.PoisonShot:
                    text += "Gives you a poison ability\nthat can be released with\ncertain skills.\n";
                    break;
                case BuffType.Concentration:
                    text += "Increases chance on element extraction.\n";
                    break;
                case BuffType.MagicBooster:
                    overridestats = true;
                    text = string.Format("Increases MC by: {0}-{1}.\nIncreases consumption by {2}%.\n", buff.Stats[Stat.MinMC], buff.Stats[Stat.MaxMC], buff.Stats[Stat.ManaPenaltyPercent]);
                    break;
                case BuffType.Transform:
                    text = "Disguises your appearance.\n";
                    break;
                case BuffType.Mentee:
                    text = "Learn skill points twice as quick.\n";
                    break;
                case BuffType.Guild:
                    text += GameScene.Scene.GuildDialog.ActiveStats;
                    break;
            }

            if (!overridestats)
            {
                foreach (var val in buff.Stats.Values)
                {
                    var c = val.Value < 0 ? "Decreases" : "Increases";
                    var key = val.Key.ToString();

                    var strKey = RegexFunctions.SeperateCamelCase(key.Replace("Rate", "").Replace("Multiplier", "").Replace("Percent", ""));

                    var sign = "";

                    if (key.Contains("Percent"))
                        sign = "%";
                    else if (key.Contains("Multiplier"))
                        sign = "x";

                    var txt = $"{c} {strKey} by: {val.Value}{sign}.\n";

                    text += txt;
                }
            }

            text += buff.Infinite ? GameLanguage.ExpireNever : string.Format(GameLanguage.Expire, Functions.PrintTimeSpanFromSeconds(Math.Round((buff.ExpireTime - CMain.Time) / 1000D)));

            if (!string.IsNullOrEmpty(buff.Caster)) text += string.Format("\nCaster: {0}", buff.Caster);

            return text;
        }

        private string CombinedBuffText()
        {
            string text = "Active Buffs\n";
            var stats = new Stats();

            for (var i = 0; i < _buffList.Count; i++)
            {
                var buff = GameScene.Scene.Buffs[i];

                stats.Add(buff.Stats);
            }

            foreach (var val in stats.Values)
            {
                var c = val.Value < 0 ? "Decreased" : "Increased";
                var key = val.Key.ToString();

                var strKey = RegexFunctions.SeperateCamelCase(key.Replace("Rate", "").Replace("Multiplier", "").Replace("Percent", ""));

                var sign = "";

                if (key.Contains("Percent"))
                    sign = "%";
                else if (key.Contains("Multiplier"))
                    sign = "x";

                var txt = $"{c} {strKey} by: {val.Value}{sign}.\n";

                text += txt;
            }

            return text;
        }

        private int BuffImage(BuffType type)
        {
            switch (type)
            {
                //Skills
                case BuffType.Fury:
                    return 76;
                case BuffType.Rage:
                    return 49;
                case BuffType.ImmortalSkin:
                    return 80;
                case BuffType.CounterAttack:
                    return 7;

                case BuffType.MagicBooster:
                    return 73;
                case BuffType.MagicShield:
                    return 30;

                case BuffType.Hiding:
                case BuffType.ClearRing:
                    return 17;
                case BuffType.Haste:
                    return 60;
                case BuffType.SoulShield:
                    return 13;
                case BuffType.BlessedArmour:
                    return 14;
                case BuffType.ProtectionField:
                    return 50;
                case BuffType.UltimateEnhancer:
                    return 35;
                case BuffType.Curse:
                    return 45;
                case BuffType.EnergyShield:
                    return 57;

                case BuffType.SwiftFeet:
                    return 67;
                case BuffType.LightBody:
                    return 68;
                case BuffType.MoonLight:
                    return 65;
                case BuffType.DarkBody:
                    return 70;

                case BuffType.Concentration:
                    return 96;
                case BuffType.VampireShot:
                    return 100;
                case BuffType.PoisonShot:
                    return 102;
                case BuffType.MentalState:
                    return 199;

                //Monster
                case BuffType.RhinoPriestDebuff:
                    return 217;

                //Special
                case BuffType.GameMaster:
                    return 173;
                case BuffType.General:
                    return 182;
                case BuffType.Exp:
                    return 260;
                case BuffType.Drop:
                    return 162;
                case BuffType.Gold:
                    return 168;
                case BuffType.Knapsack:
                case BuffType.BagWeight:
                    return 235;
                case BuffType.Transform:
                    return 241;
                case BuffType.Mentor:
                    return 248;
                case BuffType.Mentee:
                    return 248;
                case BuffType.RelationshipEXP:
                    return 201;
                case BuffType.Guild:
                    return 203;
                case BuffType.Rested:
                    return 240;
                case BuffType.TemporalFlux:
                    return 261;
                case BuffType.Skill:
                    return 200;

                //Stats
                case BuffType.Impact:
                    return 249;
                case BuffType.Magic:
                    return 165;
                case BuffType.Taoist:
                    return 250;
                case BuffType.Storm:
                    return 170;
                case BuffType.HealthAid:
                    return 161;
                case BuffType.ManaAid:
                    return 169;
                case BuffType.Defence:
                    return 166;
                case BuffType.MagicDefence:
                    return 158;
                case BuffType.WonderDrug:
                    return 252;
                default:
                    return 0;
            }
        }
    }
}

