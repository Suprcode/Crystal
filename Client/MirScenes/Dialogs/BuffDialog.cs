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
        private readonly MirButton _expandCollapseButton;
        private readonly List<MirImageControl> _buffList = new List<MirImageControl>();
        private int _buffCount;

        public BuffDialog()
        {
            Index = 20;
            Library = Libraries.Prguse2;
            Movable = false;
            Size = new Size(44, 34);
            Location = new Point(Settings.ScreenWidth - 170, 0);
            Sort = true;

            _expandCollapseButton = new MirButton
            {
                Index = 7,
                HoverIndex = 8,
                Location = new Point(44 - 16, 0),
                Size = new Size(16, 15),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 9,
                Sound = SoundList.ButtonA,
            };
        }

        public void CreateBuff(Buff buff)
        {
            var text = string.Empty;
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

            var mirLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.Right,
                NotControl = true,
                ForeColour = Color.Yellow,
                Location = new Point(-7, 10),
                Size = new Size(30, 20),
                Parent = image
            };

            switch (buff.Type)
            {
                case BuffType.UltimateEnhancer:
                    switch (GameScene.User.Class)
                    {
                        case MirClass.Wizard:
                        case MirClass.Archer:
                            text =
                                $"MC increased by 0-{buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                            break;
                        case MirClass.Taoist:
                            text =
                                $"SC increased by 0-{buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                            break;
                        case MirClass.Warrior:
                            break;
                        case MirClass.Assassin:
                            break;
                        default:
                            text =
                                $"DC increased by 0-{buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                            break;
                    }
                    break;
                case BuffType.Impact:
                    text = $"DC increased by 0-{buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                    break;
                case BuffType.Magic:
                    text = $"MC increased by 0-{buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                    break;
                case BuffType.Taoist:
                    text = $"SC increased by 0-{buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                    break;
                case BuffType.Storm:
                    text = $"A.Speed increased by {buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                    break;
                case BuffType.HealthAid:
                    text = $"HP increased by {buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                    break;
                case BuffType.ManaAid:
                    text = $"MP increased by {buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                    break;
                case BuffType.Defence:
                    text = $"Max AC increased by {buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                    break;
                case BuffType.MagicDefence:
                    text = $"Max MAC increased by {buff.Values[0]} for {(buff.Expire - CMain.Time) / 1000} seconds.";
                    break;
            }

            if (text != string.Empty)
                GameScene.Scene.ChatDialog.ReceiveChat(text, ChatType.Hint);

            _buffList.Insert(0, image);

            UpdateWindow();
        }

        public void RemoveBuff(int buffId)
        {
            _buffList[buffId].Dispose();
            _buffList.RemoveAt(buffId);

            UpdateWindow();
        }

        public void UpdateBuffs()
        {
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
                image.Hint = buff.ToString();
                image.Index = buffImage;
                image.Library = buffLibrary;
                image.Visible = GameScene.Scene.PositiveBuffsDialog.Visible;

                if (buff.Infinite || !(Math.Round((buff.Expire - CMain.Time) / 1000D) <= 5))
                    continue;

                var time = (buff.Expire - CMain.Time) / 100D;

                if (Math.Round(time) % 10 < 5)
                    image.Index = -1;
            }
        }

        private void UpdateWindow()
        {
            _buffCount = _buffList.Count;

            if (_buffCount == 0)
                Visible = false;
            else
            {
                var oldWidth = Size.Width;
                Visible = true;

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

                _expandCollapseButton.Location = new Point(Size.Width - 15, 0);
                Size = new Size((_buffCount > 10 ? 10 : _buffCount) * 23, 24 + (_buffCount / 10) * 24);
            }
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
