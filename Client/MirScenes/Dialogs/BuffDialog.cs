using System;
using System.Collections.Generic;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System.Drawing;
using System.Windows.Forms;

namespace Client.MirScenes.Dialogs
{
    public class BuffDialog : MirImageControl
    {
        public List<ClientBuff> Buffs = new List<ClientBuff>();

        protected MirButton _expandCollapseButton;
        protected MirLabel _buffCountLabel;
        protected List<MirImageControl> _buffList = new List<MirImageControl>();
        protected bool _fadedOut, _fadedIn;
        protected int _buffCount;
        protected long _nextFadeTime;

        protected const long FadeDelay = 55;
        protected const float FadeRate = 0.2f;

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
                Sort = true,
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
            {
                UpdateWindow();
            }

            for (var i = 0; i < _buffList.Count; i++)
            {
                var image = _buffList[i];
                var buff = Buffs[i];

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

                var location = new Point(Size.Width - 10 - 23 - (i * 23) + ((10 * 23) * (i / 10)), 6 + ((i / 10) * 24));

                image.Location = new Point(location.X, location.Y);
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

                if (buff.Paused || buff.Infinite || !(Math.Round((buff.ExpireTime - CMain.Time) / 1000D) <= 5))
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

            var baseImage = 20;
            var heightOffset = 0;

            //foreach (var dialog in GameScene.Scene.BuffDialogs)
            //{
            //    if (dialog.Category == Category) break;

            //    if (dialog.Buffs.Count > 0)
            //    {
            //        heightOffset += dialog.Size.Height;
            //    }
            //}

            if (_buffCount > 0 && Settings.ExpandedBuffWindow)
            {
                var oldWidth = Size.Width;

                if (_buffCount <= 10)
                    Index = baseImage + _buffCount - 1;
                else if (_buffCount > 10)
                    Index = baseImage + 10;
                else if (_buffCount > 20)
                    Index = baseImage + 11;
                else if (_buffCount > 30)
                    Index = baseImage + 12;
                else if (_buffCount > 40)
                    Index = baseImage + 13;

                var newX = Location.X - Size.Width + oldWidth;
                var newY = heightOffset;
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
                var newY = heightOffset;
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
                case BuffType.Blizzard:
                    text = string.Format("Blizzard.\n\nCooldDown still active\n");
                    break;
                case BuffType.MeteorStrike:
                    text = string.Format("MeteorStrike.\n\nCooldDown still active\n");
                    break;
                case BuffType.PoisonCloud:
                    text = string.Format("PoisonCloud.\n\nCooldDown still active\n");
                    break;
                case BuffType.HeavenAndHell:
                    text = string.Format("HeavenAndHell.\n\nCooldDown still active\n");
                    break;
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
                    text += string.Format("Increases MC by: {0}-{1}.\nIncreases consumption by {2}%.\n", buff.Stats[Stat.MinMC], buff.Stats[Stat.MaxMC], buff.Stats[Stat.ManaPenaltyPercent]);
                    break;
                case BuffType.Transform:
                    text += "Disguises your appearance.\n";
                    break;
                case BuffType.Mentee:
                    text += "Learn skill points twice as quick.\n";
                    break;
                case BuffType.ExpRate:
                    overridestats = true;
                    text += string.Format("Increases ExpRate x{0}\n", buff.Stats[Stat.ExpRate]);
                    break;
                case BuffType.ExpRatePercent:
                    overridestats = true;
                    text += string.Format("Increases ExpRate by {0}%\n", buff.Stats[Stat.ExpRatePercent]);
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

            if (buff.Paused)
            {
                text += GameLanguage.ExpirePaused;
            }
            else if (buff.Infinite)
            {
                text += GameLanguage.ExpireNever;
            }
            else
            {
                text += string.Format(GameLanguage.Expire, Functions.PrintTimeSpanFromSeconds(Math.Round((buff.ExpireTime - CMain.Time) / 1000D)));
            }

            if (!string.IsNullOrEmpty(buff.Caster)) text += string.Format("\nCaster: {0}", buff.Caster);

            return text;
        }

        private string CombinedBuffText()
        {
            string text = "Active Buffs\n";
            var stats = new Stats();

            for (var i = 0; i < _buffList.Count; i++)
            {
                var buff = Buffs[i];

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
                //Reborn System Buff
                case BuffType.Reborn1:
                    return 365;
                case BuffType.Reborn2:
                    return 366;
                case BuffType.Reborn3:
                    return 367;
                case BuffType.Reborn4:
                    return 368;
                case BuffType.Reborn5:
                    return 369;
                case BuffType.Reborn6:
                    return 370;
                case BuffType.Reborn7:
                    return 371;
                case BuffType.Reborn8:
                    return 372;
                case BuffType.Reborn9:
                    return 373;
                case BuffType.Reborn10:
                    return 374;
                case BuffType.Reborn11:
                    return 375;
                case BuffType.Reborn12:
                    return 376;
                case BuffType.Reborn13:
                    return 377;
                case BuffType.Reborn14:
                    return 378;
                case BuffType.Reborn15:
                    return 379;
                case BuffType.Reborn16:
                    return 380;
                case BuffType.Reborn17:
                    return 381;
                case BuffType.Reborn18:
                    return 382;
                case BuffType.Reborn19:
                    return 383;
                case BuffType.Reborn20:
                    return 384;

                //InstanceStage System Buff
                case BuffType.InstanceStage1:
                    return 265;
                case BuffType.InstanceStage2:
                    return 265;
                case BuffType.InstanceStage3:
                    return 265;
                case BuffType.InstanceStage4:
                    return 265;
                case BuffType.InstanceStage5:
                    return 265;
                case BuffType.InstanceStage6:
                    return 265;
                case BuffType.InstanceStage7:
                    return 265;
                case BuffType.InstanceStage8:
                    return 265;
                case BuffType.InstanceStage9:
                    return 265;
                case BuffType.InstanceStage10:
                    return 265;
                case BuffType.InstanceStage11:
                    return 265;
                case BuffType.InstanceStage12:
                    return 265;
                case BuffType.InstanceStage13:
                    return 265;
                case BuffType.InstanceStage14:
                    return 265;
                case BuffType.InstanceStage15:
                    return 265;
                case BuffType.InstanceStage16:
                    return 265;
                case BuffType.InstanceStage17:
                    return 265;
                case BuffType.InstanceStage18:
                    return 265;
                case BuffType.InstanceStage19:
                    return 265;
                case BuffType.InstanceStage20:
                    return 265;
                case BuffType.InstanceStage21:
                    return 265;
                case BuffType.InstanceStage22:
                    return 265;
                case BuffType.InstanceStage23:
                    return 265;
                case BuffType.InstanceStage24:
                    return 265;
                case BuffType.InstanceStage25:
                    return 265;
                case BuffType.InstanceStage26:
                    return 265;
                case BuffType.InstanceStage27:
                    return 265;
                case BuffType.InstanceStage28:
                    return 265;
                case BuffType.InstanceStage29:
                    return 265;
                case BuffType.InstanceStage30:
                    return 265;
                case BuffType.InstanceStage31:
                    return 265;
                case BuffType.InstanceStage32:
                    return 265;
                case BuffType.InstanceStage33:
                    return 265;
                case BuffType.InstanceStage34:
                    return 265;
                case BuffType.InstanceStage35:
                    return 265;
                case BuffType.InstanceStage36:
                    return 265;
                case BuffType.InstanceStage37:
                    return 265;
                case BuffType.InstanceStage38:
                    return 265;
                case BuffType.InstanceStage39:
                    return 265;
                case BuffType.InstanceStage40:
                    return 265;
                case BuffType.InstanceStage41:
                    return 265;
                case BuffType.InstanceStage42:
                    return 265;
                case BuffType.InstanceStage43:
                    return 265;
                case BuffType.InstanceStage44:
                    return 265;
                case BuffType.InstanceStage45:
                    return 265;
                case BuffType.InstanceStage46:
                    return 265;
                case BuffType.InstanceStage47:
                    return 265;
                case BuffType.InstanceStage48:
                    return 265;
                case BuffType.InstanceStage49:
                    return 265;
                case BuffType.InstanceStage50:
                    return 265;

                //ChallengeStage System Buff
                case BuffType.ChallengeStage1:
                    return 266;
                case BuffType.ChallengeStage2:
                    return 266;
                case BuffType.ChallengeStage3:
                    return 266;
                case BuffType.ChallengeStage4:
                    return 266;
                case BuffType.ChallengeStage5:
                    return 266;
                case BuffType.ChallengeStage6:
                    return 266;
                case BuffType.ChallengeStage7:
                    return 266;
                case BuffType.ChallengeStage8:
                    return 266;
                case BuffType.ChallengeStage9:
                    return 266;
                case BuffType.ChallengeStage10:
                    return 266;
                case BuffType.ChallengeStage11:
                    return 266;
                case BuffType.ChallengeStage12:
                    return 266;
                case BuffType.ChallengeStage13:
                    return 266;
                case BuffType.ChallengeStage14:
                    return 266;
                case BuffType.ChallengeStage15:
                    return 266;
                case BuffType.ChallengeStage16:
                    return 266;
                case BuffType.ChallengeStage17:
                    return 266;
                case BuffType.ChallengeStage18:
                    return 266;
                case BuffType.ChallengeStage19:
                    return 266;
                case BuffType.ChallengeStage20:
                    return 266;
                case BuffType.ChallengeStage21:
                    return 266;
                case BuffType.ChallengeStage22:
                    return 266;
                case BuffType.ChallengeStage23:
                    return 266;
                case BuffType.ChallengeStage24:
                    return 266;
                case BuffType.ChallengeStage25:
                    return 266;
                case BuffType.ChallengeStage26:
                    return 266;
                case BuffType.ChallengeStage27:
                    return 266;
                case BuffType.ChallengeStage28:
                    return 266;
                case BuffType.ChallengeStage29:
                    return 266;
                case BuffType.ChallengeStage30:
                    return 266;
                case BuffType.ChallengeStage31:
                    return 266;
                case BuffType.ChallengeStage32:
                    return 266;
                case BuffType.ChallengeStage33:
                    return 266;
                case BuffType.ChallengeStage34:
                    return 266;
                case BuffType.ChallengeStage35:
                    return 266;
                case BuffType.ChallengeStage36:
                    return 266;
                case BuffType.ChallengeStage37:
                    return 266;
                case BuffType.ChallengeStage38:
                    return 266;
                case BuffType.ChallengeStage39:
                    return 266;
                case BuffType.ChallengeStage40:
                    return 266;
                case BuffType.ChallengeStage41:
                    return 266;
                case BuffType.ChallengeStage42:
                    return 266;
                case BuffType.ChallengeStage43:
                    return 266;
                case BuffType.ChallengeStage44:
                    return 266;
                case BuffType.ChallengeStage45:
                    return 266;
                case BuffType.ChallengeStage46:
                    return 266;
                case BuffType.ChallengeStage47:
                    return 266;
                case BuffType.ChallengeStage48:
                    return 266;
                case BuffType.ChallengeStage49:
                    return 266;
                case BuffType.ChallengeStage50:
                    return 266;

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

                case BuffType.InfiniteBuff:
                    return 228;

                //Special
                case BuffType.GameMaster:
                    return 173;
                case BuffType.General:
                    return 182;
                case BuffType.ExpRatePercent:
                    return 260;
                case BuffType.ExpRate:
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

                case BuffType.HPDrugSmall:
                    return 273;
                case BuffType.HPDrugMedium:
                    return 274;
                case BuffType.HPDrugLarge:
                    return 275;
                case BuffType.HPDrugXL:
                    return 276;
                case BuffType.HPSMegaDrug:
                    return 277;
                case BuffType.HPMMegaDrug:
                    return 278;
                case BuffType.HPLMegaDrug:
                    return 279;
                case BuffType.HPXLMegaDrug:
                    return 280;
                case BuffType.HPSUltraDrug:
                    return 281;
                case BuffType.HPMUltraDrug:
                    return 282;
                case BuffType.HPLUltraDrug:
                    return 283;
                case BuffType.HPXLUltraDrug:
                    return 284;
                case BuffType.HPXXLUltraDrug:
                    return 285;
                case BuffType.HPSSupremeDrug:
                    return 286;
                case BuffType.HPMSupremeDrug:
                    return 287;
                case BuffType.HPLSupremeDrug:
                    return 288;
                case BuffType.HPXLSupremeDrug:
                    return 289;
                case BuffType.HPXXLSupremeDrug:
                    return 290;

                case BuffType.MPDrugSmall:
                    return 291;
                case BuffType.MPDrugMedium:
                    return 292;
                case BuffType.MPDrugLarge:
                    return 293;
                case BuffType.MPDrugXL:
                    return 294;
                case BuffType.MPSMegaDrug:
                    return 295;
                case BuffType.MPMMegaDrug:
                    return 296;
                case BuffType.MPLMegaDrug:
                    return 297;
                case BuffType.MPXLMegaDrug:
                    return 298;
                case BuffType.MPSUltraDrug:
                    return 299;
                case BuffType.MPMUltraDrug:
                    return 300;
                case BuffType.MPLUltraDrug:
                    return 301;
                case BuffType.MPXLUltraDrug:
                    return 302;
                case BuffType.MPXXLUltraDrug:
                    return 303;
                case BuffType.MPSSupremeDrug:
                    return 304;
                case BuffType.MPMSupremeDrug:
                    return 305;
                case BuffType.MPLSupremeDrug:
                    return 306;
                case BuffType.MPXLSupremeDrug:
                    return 307;
                case BuffType.MPXXLSupremeDrug:
                    return 308;
                default:
                    return 0;
            }
        }
    }


    //UNFINISHED
    public class ClientPoisonBuff
    {
        public PoisonType Type;
        public string Caster;
        public int Value;
        public int TickSpeed;
        public long ExpireTime;
    }

    public sealed class PoisonBuffDialog : MirImageControl
    {
        public List<ClientPoisonBuff> Buffs = new List<ClientPoisonBuff>();

        protected MirButton _expandCollapseButton;
        protected MirLabel _buffCountLabel;
        protected List<MirImageControl> _buffList = new List<MirImageControl>();
        protected bool _fadedOut, _fadedIn;
        protected int _buffCount;
        protected long _nextFadeTime;

        protected const long FadeDelay = 55;
        protected const float FadeRate = 0.2f;

        public PoisonBuffDialog()
        {
            Index = 40;
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
                Sort = true,
                Visible = false,
                ForeColour = Color.Yellow,
                OutLineColour = Color.Black,
            };
        }

        public void CreateBuff(ClientPoisonBuff buff)
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

        public string BuffString(ClientPoisonBuff buff)
        {
            string text = RegexFunctions.SeperateCamelCase(buff.Type.ToString()) + "\n";
            bool overridestats = false;

            switch (buff.Type)
            {
                case PoisonType.Green:
                    {
                        var tick = buff.TickSpeed / 1000;
                        var tickName = tick > 1 ? "seconds" : "second";

                        text += $"Recieve {buff.Value} damage every {tick} {tickName}.\n";
                    }
                    break;
                case PoisonType.Red:
                    {
                        var tick = buff.TickSpeed / 1000;
                        var tickName = tick > 1 ? "seconds" : "second";

                        text += $"Reduces armour rate by 10% every {tick} {tickName}.\n";
                    }
                    break;
                case PoisonType.Slow:
                    text += "Reduces movement speed.\n";
                    break;
                case PoisonType.Frozen:
                    text += "Prevents casting, movin\nand attacking.\n";
                    break;
                case PoisonType.Stun:
                    {
                        var tick = buff.TickSpeed / 1000;
                        var tickName = tick > 1 ? "seconds" : "second";

                        text += $"Increases damage received by 20% every {tick} {tickName}.\n";
                    }
                    break;
                case PoisonType.Paralysis:
                    text += "Prevents moving and attacking.\n";
                    break;
                case PoisonType.DelayedExplosion:
                    text += "Ticking time bomb.\n";
                    break;
                case PoisonType.Bleeding:
                    {
                        var tick = buff.TickSpeed / 1000;
                        var tickName = tick > 1 ? "seconds" : "second";

                        text += $"Recieve {buff.Value} damage every {tick} {tickName}.\n";
                    }
                    break;
                case PoisonType.LRParalysis:
                    text += "Prevents moving and attacking.\nCancels when attacked\n";
                    break;
                case PoisonType.Blindness:
                    text += "Causes temporary blindness.\n";
                    break;
                case PoisonType.Dazed:
                    text += "Prevents attacking.\n";
                    break;
            }

            text += string.Format(GameLanguage.Expire, Functions.PrintTimeSpanFromSeconds(Math.Round((buff.ExpireTime - CMain.Time) / 1000D)));

            if (!string.IsNullOrEmpty(buff.Caster)) text += string.Format("\nCaster: {0}", buff.Caster);

            return text;
        }

        private int BuffImage(PoisonType type)
        {
            switch (type)
            {
                case PoisonType.Green:
                    return 221;
                case PoisonType.Red:
                    return 222;
                case PoisonType.Slow:
                    return 225;
                case PoisonType.Frozen:
                    return 223;
                case PoisonType.Stun:
                    return 224;
                case PoisonType.Paralysis:
                    return 233;
                case PoisonType.DelayedExplosion:
                    return 229;
                case PoisonType.Bleeding:
                    return 231;
                case PoisonType.LRParalysis:
                    return 233;
                case PoisonType.Blindness:
                    return 226;
                case PoisonType.Dazed:
                    return 230;
                default:
                    return 0;
            }
        }

        public void Process()
        {
            if (!Visible) return;

            if (_buffList.Count != _buffCount)
            {
                UpdateWindow();
            }

            for (var i = 0; i < _buffList.Count; i++)
            {
                var image = _buffList[i];
                var buff = Buffs[i];

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

                var location = new Point(Size.Width - 10 - 23 - (i * 23) + ((10 * 23) * (i / 10)), 6 + ((i / 10) * 24));

                image.Location = new Point(location.X, location.Y);
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

                if (!(Math.Round((buff.ExpireTime - CMain.Time) / 1000D) <= 5))
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

            var baseImage = 20;
            var heightOffset = 36;

            if (_buffCount > 0 && Settings.ExpandedBuffWindow)
            {
                var oldWidth = Size.Width;

                if (_buffCount <= 10)
                    Index = baseImage + _buffCount - 1;
                else if (_buffCount > 10)
                    Index = baseImage + 10;
                else if (_buffCount > 20)
                    Index = baseImage + 11;
                else if (_buffCount > 30)
                    Index = baseImage + 12;
                else if (_buffCount > 40)
                    Index = baseImage + 13;

                var newX = Location.X - Size.Width + oldWidth;
                var newY = heightOffset;
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
                var newY = heightOffset;
                Location = new Point(newX, newY);

                _buffCountLabel.Visible = true;
                _buffCountLabel.Text = $"{_buffCount}";
                _buffCountLabel.Location = new Point(Size.Width / 2 - _buffCountLabel.Size.Width / 2, Size.Height / 2 - 10);
                _buffCountLabel.BringToFront();

                _expandCollapseButton.Location = new Point(Size.Width - 15, 0);
                Size = new Size(44, 34);
            }
        }

        private string CombinedBuffText()
        {
            string text = "Active Poisons\n";

            return text;
        }
    }

}

