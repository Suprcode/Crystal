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
                Sort = true,
                Visible = false,
                ForeColour = Color.Yellow,
                OutLineColour = Color.Black,
            };
        }

        public void CreateBuff(Buff buff)
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
                image.Hint = Settings.ExpandedBuffWindow ? buff.ToString() : CombinedBuffText();
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

                if (buff.Infinite || !(Math.Round((buff.Expire - CMain.Time) / 1000D) <= 5))
                    continue;

                var time = (buff.Expire - CMain.Time) / 100D;

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

        private string CombinedBuffText()
        {
            var buffText = string.Empty;

            int buffDc = 0,
                buffMinMc = 0,
                buffMc = 0,
                buffSc = 0,
                buffAttackSpeed = 0,
                buffMovementSpeed = 0,
                buffMinMac = 0,
                buffMac = 0,
                buffMinAc = 0,
                buffAc = 0,
                buffAgility = 0,
                buffExp = 0,
                buffDrop = 0,
                buffGold = 0,
                buffHealth = 0,
                buffMana = 0,
                buffBagWeight = 0;

            buffText = "Active Buffs";

            for (var i = 0; i < _buffList.Count; i++)
            {
                var buff = GameScene.Scene.Buffs[i];

                switch (buff.Type)
                {
                    case BuffType.Haste:
                        buffAttackSpeed += buff.Values[0];
                        break;

                    case BuffType.SwiftFeet:
                        buffMovementSpeed += buff.Values[0];
                        break;

                    case BuffType.Fury:
                        buffAttackSpeed += buff.Values[0];
                        break;

                    case BuffType.SoulShield:
                        buffMac += buff.Values[0];
                        break;

                    case BuffType.BlessedArmour:
                        buffAc += buff.Values[0];
                        break;

                    case BuffType.LightBody:
                        buffAgility += buff.Values[0];
                        break;

                    case BuffType.UltimateEnhancer:
                        switch (GameScene.User.Class)
                        {
                            case MirClass.Wizard:
                            case MirClass.Archer:
                                buffMc += buff.Values[0];
                                break;
                            case MirClass.Taoist:
                                buffSc += buff.Values[0];
                                break;
                            default:
                                buffDc += buff.Values[0];
                                break;
                        }
                        break;

                    case BuffType.ProtectionField:
                        buffAc += buff.Values[0];
                        break;

                    case BuffType.Rage:
                        buffDc += buff.Values[0];
                        break;

                    case BuffType.CounterAttack:
                        buffAc += buff.Values[0];
                        buffMac += buff.Values[0];
                        break;

                    case BuffType.MagicBooster:
                        buffMinMc += buff.Values[0];
                        buffMc += buff.Values[0];
                        break;

                    case BuffType.ImmortalSkin:
                        buffAc += buff.Values[0];
                        break;
 
                    case BuffType.General:
                        buffExp += buff.Values[0];

                        if (buff.Values.Length > 1)
                            buffDrop += buff.Values[1];
                        if (buff.Values.Length > 2)
                            buffGold += buff.Values[2];
                        break;

                    case BuffType.Exp:
                        buffExp += buff.Values[0];
                        break;

                    case BuffType.Drop:
                        buffDrop += buff.Values[0];
                        break;

                    case BuffType.Gold:
                        buffGold += buff.Values[0];
                        break;

                    case BuffType.BagWeight:
                        buffBagWeight += buff.Values[0];
                        break;

                    case BuffType.RelationshipEXP:
                        buffExp += buff.Values[0];
                        break;

                    case BuffType.Rested:
                        buffExp += buff.Values[0];
                        break;

                    case BuffType.Impact:
                        buffDc += buff.Values[0];
                        break;

                    case BuffType.Magic:
                        buffMc += buff.Values[0];
                        break;

                    case BuffType.Taoist:
                        buffSc += buff.Values[0];
                        break;

                    case BuffType.Storm:
                        buffAttackSpeed += buff.Values[0];
                        break;

                    case BuffType.HealthAid:
                        buffHealth += buff.Values[0];
                        break;

                    case BuffType.ManaAid:
                        buffMana += buff.Values[0];
                        break;

                    case BuffType.Defence:
                        buffMinAc += buff.Values[0];
                        buffAc += buff.Values[0];
                        break;

                    case BuffType.MagicDefence:
                        buffMinMac += buff.Values[0];
                        buffMac += buff.Values[0];
                        break;

                    case BuffType.WonderDrug:
                        switch (buff.Values[0])
                        {
                            case 0:
                                buffExp += buff.Values[1];
                                break;
                            case 1:
                                buffDrop += buff.Values[1];
                                break;
                            case 2:
                                buffHealth += buff.Values[1];
                                break;
                            case 3:
                                buffMana += buff.Values[1];
                                break;
                            case 4:
                                buffMinAc += buff.Values[1];
                                buffAc += buff.Values[1];
                                break;
                            case 5:
                                buffMinMac += buff.Values[1];
                                buffMac += buff.Values[1];
                                break;
                            case 6:
                                buffAttackSpeed += buff.Values[1];
                                break;
                        }
                        break;

                    case BuffType.Knapsack:
                        buffBagWeight += buff.Values[0];
                        break;
                }
            }
            
            if (buffDc > 0)
                buffText += $"\nIncreased DC: 0-{buffDc}";

            if (buffMinMc > 0 || buffMc > 0)
                buffText += $"\nIncreased MC: {buffMinMc}-{buffMc}";

            if (buffSc > 0)
                buffText += $"\nIncreased SC: 0-{buffSc}";

            if (buffMinAc > 0 || buffAc > 0)
                buffText += $"\nIncreased AC: {buffMinAc}-{buffAc}";

            if (buffMinMac > 0 || buffMac > 0)
                buffText += $"\nIncreased MAC: {buffMinMac}-{buffMac}";

            if (buffAttackSpeed > 0 || buffMovementSpeed > 0 || buffAgility > 0)
                buffText += "\n";

            if (buffAttackSpeed > 0)
                buffText += $"\nIncreased Attack Speed: {buffAttackSpeed}";

            if (buffMovementSpeed > 0)
                buffText += $"\nIncreased Movement Speed: {buffMovementSpeed}";

            if (buffAgility > 0)
                buffText += $"\nIncreased Agility: {buffAgility}";

            if (buffExp > 0 || buffDrop > 0 || buffGold > 0)
                buffText += "\n";

            if (buffExp > 0)
                buffText += $"\nExperience Increased By: {buffExp}%";

            if (buffDrop > 0)
                buffText += $"\nDrop Rate Increased By: {buffDrop}%";

            if (buffGold > 0)
                buffText += $"\nGold Rate Increased By: {buffGold}%";

            if (buffHealth > 0 || buffMana > 0 || buffBagWeight > 0)
                buffText += "\n";

            if (buffHealth > 0)
                buffText += $"Increased Health: {buffHealth}";

            if (buffMana > 0)
                buffText += $"Increased Mana: {buffMana}";

            if (buffBagWeight > 0)
                buffText += $"Increased Bag Weight: {buffBagWeight}";

            return buffText;
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

