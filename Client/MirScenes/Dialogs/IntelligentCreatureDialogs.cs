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
    public sealed class IntelligentCreatureDialog : MirImageControl
    {
        public MirImageControl FullnessBG, FullnessFG, FullnessMin, FullnessNow;
        public MirImageControl PearlImage, BlackStoneImageBG, BlackStoneImageFG;
        public MirLabel CreatureName, CreatureDeadline, CreaturePearls, CreatureInfo, CreatureInfo1, CreatureInfo2, CreatureMaintainFoodBuff, HoverLabel;
        public MirButton CloseButton, HelpPetButton, CreatureRenameButton, SummonButton, DismissButton, ReleaseButton;
        public MirButton AutomaticModeButton, SemiAutoModeButton, OptionsMenuButton;
        public CreatureButton[] CreatureButtons;
        public int SelectedCreatureSlot = -1;
        public MirControl HoverLabelParent = null;

        private MirAnimatedControl CreatureImage;
        public long SwitchAnimTime;
        public bool AnimSwitched = false;
        public bool AnimNeedSwitch = false;

        private const long blackstoneProduceTime = 10800;//3 hours in seconds

        private bool showing = false;

        public IntelligentCreatureDialog()
        {
            Index = 468;
            Library = Libraries.Title;
            Movable = true;
            Sort = true;
            Location = Center;
            BeforeDraw += IntelligentCreatureDialog_BeforeDraw;

            #region CreatureButtons
            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(Size.Width - 25, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            HelpPetButton = new MirButton
            {
                HoverIndex = 258,
                Index = 257,
                Location = new Point(Size.Width - 48, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 259,
                Sound = SoundList.ButtonA,
            };

            CreatureRenameButton = new MirButton
            {
                HoverIndex = 571,
                Index = 570,
                Location = new Point(344, 50),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 572,
                Sound = SoundList.ButtonA,
                Visible = false,
            };
            CreatureRenameButton.Click += ButtonClick;

            SummonButton = new MirButton
            {
                Index = 576,
                HoverIndex = 577,
                PressedIndex = 578,
                Location = new Point(113, 217),
                Library = Libraries.Title,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            SummonButton.Click += ButtonClick;

            DismissButton = new MirButton//Dismiss the summoned pet
            {
                HoverIndex = 581,
                Index = 580,
                Location = new Point(113, 217),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 582,
                Sound = SoundList.ButtonA,
            };
            DismissButton.Click += ButtonClick;

            ReleaseButton = new MirButton//Removes the selected pet
            {
                HoverIndex = 584,
                Index = 583,
                Location = new Point(255, 217),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 585,
                Sound = SoundList.ButtonA,
            };
            ReleaseButton.Click += ButtonClick;

            OptionsMenuButton = new MirButton//Options
            {
                HoverIndex = 574,
                Index = 573,
                Location = new Point(375, 160),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 575,
                Sound = SoundList.ButtonA,
            };
            OptionsMenuButton.Click += ButtonClick;

            AutomaticModeButton = new MirButton//image is wrongly translated should be "Auto" instaid of "Enable"
            {
                HoverIndex = 611,
                Index = 610,
                Location = new Point(375, 187),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 612,
                Sound = SoundList.ButtonA,
            };
            AutomaticModeButton.Click += ButtonClick;

            SemiAutoModeButton = new MirButton//image is wrongly translated should be "SemiAuto" instaid of "Disable"
            {
                HoverIndex = 614,
                Index = 613,
                Location = new Point(375, 187),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 615,
                Sound = SoundList.ButtonA,
            };
            SemiAutoModeButton.Click += ButtonClick;

            CreatureButtons = new CreatureButton[10];
            for (int i = 0; i < CreatureButtons.Length; i++)
            {
                int offsetX = i * 81;
                int offsetY = 259;
                if (i >= 5)
                {
                    offsetX = (i - 5) * 81;
                    offsetY += 40;
                }
                CreatureButtons[i] = new CreatureButton { idx = i, Parent = this, Visible = false, Location = new Point((44 + offsetX), offsetY) };
            }
            #endregion

            #region CreatureImage
            CreatureImage = new MirAnimatedControl
            {
                Animated = false,
                AnimationCount = 4,
                AnimationDelay = 250,
                Index = 0,
                Library = Libraries.Prguse2,
                Loop = true,
                Parent = this,
                NotControl = true,
                UseOffSet = true,
                Location = new Point(50, 110),
            };

            FullnessBG = new MirImageControl
            {
                Index = 530,
                Library = Libraries.Prguse2,
                Location = new Point(185, 129),
                Parent = this,
                NotControl = true,
            };
            FullnessBG.MouseEnter += Control_MouseEnter;
            FullnessBG.MouseLeave += Control_MouseLeave;

            FullnessFG = new MirImageControl
            {
                Index = 531,
                Library = Libraries.Prguse2,
                Location = new Point(185, 129),
                Parent = this,
                DrawImage = false,
                //NotControl = true,
            };
            FullnessFG.AfterDraw += FullnessForeGround_AfterDraw;
            FullnessFG.MouseEnter += Control_MouseEnter;
            FullnessFG.MouseLeave += Control_MouseLeave;

            FullnessMin = new MirImageControl
            {
                Index = 532,
                Library = Libraries.Prguse2,
                Location = new Point(179, 118),
                Parent = this,
                //Visible = false,
                //NotControl = true,
            };
            FullnessMin.MouseEnter += Control_MouseEnter;
            FullnessMin.MouseLeave += Control_MouseLeave;

            FullnessNow = new MirImageControl
            {
                Index = 533,
                Library = Libraries.Prguse2,
                Location = new Point(179, 143),
                Parent = this,
                //Visible = false,
                NotControl = true,
            };

            PearlImage = new MirImageControl
            {
                Index = 427,
                Library = Libraries.Prguse2,
                Location = new Point(29, 348),
                Parent = this,
                NotControl = true,
            };

            BlackStoneImageBG = new MirImageControl
            {
                Index = 428,
                Library = Libraries.Prguse2,
                Location = new Point(215, 348),
                Parent = this,
                Visible = true,
                NotControl = true,
            };
            BlackStoneImageBG.MouseEnter += Control_MouseEnter;
            BlackStoneImageBG.MouseLeave += Control_MouseLeave;

            BlackStoneImageFG = new MirImageControl
            {
                Index = 420,
                Library = Libraries.Prguse2,
                Location = new Point(242, 353),
                Parent = this,
                Visible = true,
                DrawImage = false,
                //NotControl = true,
            };
            BlackStoneImageFG.AfterDraw += BlackStoneImageFG_AfterDraw;
            BlackStoneImageFG.MouseEnter += Control_MouseEnter;
            BlackStoneImageFG.MouseLeave += Control_MouseLeave;

            #endregion

            #region CreatureLabels
            CreatureName = new MirLabel
            {
                Parent = this,
                Location = new Point(170, 50),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Size = new Size(166, 21),
                NotControl = true,
            };

            CreatureDeadline = new MirLabel
            {
                Parent = this,
                Location = new Point(140, 85),
                DrawFormat = TextFormatFlags.VerticalCenter,
                Size = new Size(350, 21),
                NotControl = true,

            };

            CreaturePearls = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(53, 348),
                DrawFormat = TextFormatFlags.VerticalCenter,
                //Size = new Size(350, 21),
                Text = "0",
                NotControl = true,
            };

            CreatureInfo = new MirLabel
            {
                Parent = this,
                Location = new Point(19, 161),
                DrawFormat = TextFormatFlags.VerticalCenter,
                Size = new Size(350, 15),
                NotControl = true,
            };

            CreatureInfo1 = new MirLabel
            {
                Parent = this,
                Location = new Point(19, 176),
                DrawFormat = TextFormatFlags.VerticalCenter,
                Size = new Size(350, 15),
                NotControl = true,
            };

            CreatureInfo2 = new MirLabel
            {
                Parent = this,
                Location = new Point(19, 191),
                DrawFormat = TextFormatFlags.VerticalCenter,
                Size = new Size(350, 15),
                NotControl = true,
            };

            CreatureMaintainFoodBuff = new MirLabel
            {
                Parent = this,
                Location = new Point(25, 25),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Size = new Size(166, 21),
                NotControl = true,
                Visible = false //FAR made invisible as position was wierd - not sure where it's meant to be displayed
            };

            HoverLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(0, 0),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Size = new Size(100, 15),
                NotControl = true,
            };
            #endregion

        }

        #region EventHandlers
        private void IntelligentCreatureDialog_BeforeDraw(object sender, EventArgs e)
        {
            RefreshDialog();
        }
        private void FullnessForeGround_AfterDraw(object sender, EventArgs e)
        {
            int selectedCreature = BeforeAfterDraw();
            if (selectedCreature < 0) return;

            double percent = GameScene.User.IntelligentCreatures[selectedCreature].Fullness / ((double)10000);
            if (percent > 1) percent = 1;
            if (percent <= 0)
            {
                FullnessNow.Location = new Point(179, 143);
                return;
            }

            if (HoverLabel.Visible && HoverLabelParent != null && HoverLabelParent == FullnessFG)
                HoverLabel.Text = GameScene.User.IntelligentCreatures[selectedCreature].Fullness.ToString() + " / 10000";

            Rectangle section = new Rectangle
            {
                Size = new Size((int)((FullnessFG.Size.Width) * percent), FullnessFG.Size.Height)
            };

            FullnessFG.Library.Draw(FullnessFG.Index, section, FullnessFG.DisplayLocation, Color.White, false);


            FullnessNow.Location = new Point(FullnessFG.Location.X + section.Size.Width - 8, FullnessNow.Location.Y);

            percent = GameScene.User.IntelligentCreatures[selectedCreature].CreatureRules.MinimalFullness / ((double)10000);
            Size size = new Size((int)((FullnessFG.Size.Width) * percent), FullnessFG.Size.Height);

            FullnessMin.Location = new Point(FullnessFG.Location.X + size.Width - 8, FullnessMin.Location.Y);
        }
        private void BlackStoneImageFG_AfterDraw(object sender, EventArgs e)
        {
            int selectedCreature = BeforeAfterDraw();
            if (selectedCreature < 0) return;

            double percent = GameScene.User.IntelligentCreatures[selectedCreature].BlackstoneTime / ((double)blackstoneProduceTime);
            if (percent > 1) percent = 1;
            if (percent <= 0) return;

            if (HoverLabel.Visible && HoverLabelParent != null && HoverLabelParent == BlackStoneImageFG)
            {
                if (GameScene.User.IntelligentCreatures[selectedCreature].CreatureRules.CanProduceBlackStone)
                    HoverLabel.Text = string.Format("{0}", Functions.PrintTimeSpanFromSeconds(blackstoneProduceTime - GameScene.User.IntelligentCreatures[selectedCreature].BlackstoneTime));
                else
                    HoverLabel.Text = "No Production.";
            }

            Rectangle section = new Rectangle
            {
                Size = new Size((int)((BlackStoneImageFG.Size.Width) * percent), BlackStoneImageFG.Size.Height)
            };

            BlackStoneImageFG.Library.Draw(BlackStoneImageFG.Index, section, BlackStoneImageFG.DisplayLocation, Color.White, false);
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            int selectedCreature = BeforeAfterDraw();
            if (selectedCreature < 0) return;

            if (sender == FullnessMin)
            {
                HoverLabel.Visible = true;
                HoverLabel.Text = "Needed " + GameScene.User.IntelligentCreatures[selectedCreature].CreatureRules.MinimalFullness.ToString();
                HoverLabel.Size = new Size(150, 15);
                HoverLabel.Location = new Point((FullnessMin.Location.X + 8) - (HoverLabel.Size.Width / 2), FullnessFG.Location.Y - 18);
            }
            if (sender == FullnessFG || sender == FullnessBG)
            {
                HoverLabel.Visible = true;
                HoverLabel.Text = GameScene.User.IntelligentCreatures[selectedCreature].Fullness.ToString() + " / 10000";
                HoverLabel.Size = FullnessFG.Size;
                HoverLabel.Location = new Point(FullnessFG.Location.X, FullnessFG.Location.Y - 2);
                HoverLabelParent = FullnessFG;
            }
            if (sender == BlackStoneImageBG || sender == BlackStoneImageFG)
            {
                HoverLabel.Visible = true;
                HoverLabel.Text = string.Format("{0}", Functions.PrintTimeSpanFromSeconds(blackstoneProduceTime - GameScene.User.IntelligentCreatures[selectedCreature].BlackstoneTime));
                HoverLabel.Size = BlackStoneImageBG.Size;
                HoverLabel.Location = new Point(BlackStoneImageBG.Location.X + 5, BlackStoneImageBG.Location.Y - 2);
                HoverLabelParent = BlackStoneImageFG;
            }
        }
        private void Control_MouseLeave(object sender, EventArgs e)
        {
            HoverLabel.Text = "";
            HoverLabel.Visible = false;
            HoverLabel.Parent = this;
            HoverLabelParent = null;
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            int selectedCreature = BeforeAfterDraw();
            if (selectedCreature < 0) return;

            bool needSummon = false, needDismiss = false, needRelease = false, needUpdate = false;

            if (sender == CreatureRenameButton)
            {
                MirInputBox inputBox = new MirInputBox("Please enter a new name for the creature.");
                inputBox.InputTextBox.Text = GameScene.User.IntelligentCreatures[selectedCreature].CustomName;
                inputBox.OKButton.Click += (o1, e1) =>
                {
                    Update();//refresh changes
                    GameScene.User.IntelligentCreatures[selectedCreature].CustomName = inputBox.InputTextBox.Text;
                    Network.Enqueue(new C.UpdateIntelligentCreature { Creature = GameScene.User.IntelligentCreatures[selectedCreature] });
                    inputBox.Dispose();
                };
                inputBox.Show();
                CreatureRenameButton.Visible = false;
                return;
            }
            if (sender == SummonButton)
            {
                //if (GameScene.User.IntelligentCreatures[selectedCreature].Fullness == 0)
                //{
                //    GameScene.Scene.ChatDialog.ReceiveChat((string.Format("Creature {0} is starving, revitalize first.", GameScene.User.IntelligentCreatures[selectedCreature].CustomName)), ChatType.System);
                //}

                needSummon = true;
                needUpdate = true;

                SummonButton.Enabled = false;
                DismissButton.Enabled = true;
                DismissButton.Visible = true;
            }
            if (sender == DismissButton)
            {
                needDismiss = true;
                needUpdate = true;

                SummonButton.Enabled = true;
                DismissButton.Enabled = false;
                DismissButton.Visible = false;
            }
            if (sender == ReleaseButton)
            {
                MirInputBox verificationBox = new MirInputBox("Please enter the creature's name for verification.");
                verificationBox.OKButton.Click += (o1, e1) =>
                {
                    if (String.Compare(verificationBox.InputTextBox.Text, GameScene.User.IntelligentCreatures[selectedCreature].CustomName, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Verification Failed!!", ChatType.System);
                    }
                    else
                    {
                        //clear all and get new info after server got update
                        for (int i = 0; i < CreatureButtons.Length; i++) CreatureButtons[i].Clear();
                        Hide();
                        Network.Enqueue(new C.UpdateIntelligentCreature { Creature = GameScene.User.IntelligentCreatures[selectedCreature], ReleaseMe = true });
                    }
                    verificationBox.Dispose();
                };
                verificationBox.Show();
                return;
            }
            if (sender == SemiAutoModeButton)
            {
                //make sure rules allow Automatic Mode
                if (!GameScene.User.IntelligentCreatures[selectedCreature].CreatureRules.AutoPickupEnabled) return;

                //turn on automatic pickupmode
                SemiAutoModeButton.Visible = false;
                AutomaticModeButton.Visible = true;
                GameScene.User.IntelligentCreatures[selectedCreature].petMode = IntelligentCreaturePickupMode.Automatic;
                needUpdate = true;
            }
            if (sender == AutomaticModeButton)
            {
                //make sure rules allow SemiAutomatic Mode
                if (!GameScene.User.IntelligentCreatures[selectedCreature].CreatureRules.SemiAutoPickupEnabled) return;

                //turn on semiauto pickupmode
                AutomaticModeButton.Visible = false;
                SemiAutoModeButton.Visible = true;
                GameScene.User.IntelligentCreatures[selectedCreature].petMode = IntelligentCreaturePickupMode.SemiAutomatic;
                needUpdate = true;
            }
            if (sender == OptionsMenuButton)
            {
                //show ItemFilter
                if (!GameScene.Scene.IntelligentCreatureOptionsDialog.Visible) GameScene.Scene.IntelligentCreatureOptionsDialog.Show(GameScene.User.IntelligentCreatures[selectedCreature].Filter);
                if (!GameScene.Scene.IntelligentCreatureOptionsGradeDialog.Visible) GameScene.Scene.IntelligentCreatureOptionsGradeDialog.Show(GameScene.User.IntelligentCreatures[selectedCreature].Filter.PickupGrade);
            }

            if (needUpdate)
            {
                Update();//refresh changes
                Network.Enqueue(new C.UpdateIntelligentCreature { Creature = GameScene.User.IntelligentCreatures[selectedCreature], SummonMe = needSummon, UnSummonMe = needDismiss, ReleaseMe = needRelease });
            }
        }

        #endregion

        #region Process
        public void Update()
        {
            if (!Visible) return;
            RefreshDialog();
        }
        public void RefreshDialog()
        {
            RefreshInfo();
            RefreshUI();
            RefreshMode();
            BeforeAfterDraw();
            DrawCreatureAnimation();
        }
        private void RefreshInfo()
        {
            CreaturePearls.Text = GameScene.User.PearlCount.ToString();

            int SelectedButton = -1;

            for (int i = 0; i < CreatureButtons.Length; i++)
            {
                if (i >= GameScene.User.IntelligentCreatures.Count)
                {
                    CreatureButtons[i].Clear();
                    continue;
                }

                CreatureButtons[i].Visible = true;
                CreatureButtons[i].Update(GameScene.User.IntelligentCreatures[i], showing);

                //Check what creature is currently summoned if at all
                if (showing && GameScene.User.CreatureSummoned && CreatureButtons[i].PetType == GameScene.User.SummonedCreatureType) SelectedButton = i;
            }
            showing = false;

            if (SelectedButton < 0) return;
            CreatureButtons[SelectedButton].SelectButton();
        }
        private void RefreshUI()
        {
            bool error = false;
            int selectedCreature = -1;
            if (SelectedCreatureSlot < 0)
            {
                error = true;
            }
            else
            {
                selectedCreature = GetCreatureFromSlot(SelectedCreatureSlot);
                if (selectedCreature < 0) error = true;
            }

            if (error)
            {
                CreatureImage.Visible = false;
                CreatureName.Visible = false;
                CreatureDeadline.Visible = false;
                CreatureInfo.Visible = false;
                CreatureInfo1.Visible = false;
                CreatureInfo2.Visible = false;

                CreatureRenameButton.Enabled = false;
                SummonButton.Enabled = false;
                DismissButton.Enabled = false;
                DismissButton.Visible = false;
                ReleaseButton.Enabled = false;
                SemiAutoModeButton.Enabled = false;
                AutomaticModeButton.Enabled = false;
                OptionsMenuButton.Enabled = false;
            }
            else
            {
                CreatureImage.Visible = true;
                CreatureName.Visible = true;
                CreatureDeadline.Visible = true;
                CreatureInfo.Visible = true;
                CreatureInfo1.Visible = true;
                CreatureInfo2.Visible = true;

                CreatureRenameButton.Enabled = true;
                ReleaseButton.Enabled = true;
                OptionsMenuButton.Enabled = true;
                SemiAutoModeButton.Enabled = true;
                AutomaticModeButton.Enabled = true;

                //Check what creature is currently summoned
                if (GameScene.User.CreatureSummoned)
                {
                    if (GameScene.User.IntelligentCreatures[selectedCreature].PetType == GameScene.User.SummonedCreatureType)
                    {

                        DismissButton.Enabled = true;
                        DismissButton.Visible = true;
                        ReleaseButton.Enabled = false;
                    }
                    else
                    {
                        SummonButton.Index = 593;
                        SummonButton.HoverIndex = 594;
                        SummonButton.PressedIndex = 595;
                        SummonButton.Enabled = false;
                        DismissButton.Enabled = false;
                        DismissButton.Visible = false;
                    }
                }
                else
                {
                    DismissButton.Enabled = false;
                    DismissButton.Visible = false;
                    SummonButton.Index = 576;
                    SummonButton.HoverIndex = 577;
                    SummonButton.PressedIndex = 578;
                    SummonButton.Enabled = true;
                }
            }


        }
        private void RefreshMode()
        {
            int selectedCreature = BeforeAfterDraw();
            if (selectedCreature < 0) return;

            if (GameScene.User.IntelligentCreatures[selectedCreature].petMode == IntelligentCreaturePickupMode.Automatic)
            {
                AutomaticModeButton.Visible = true;
                SemiAutoModeButton.Visible = false;
            }
            else
            {
                AutomaticModeButton.Visible = false;
                SemiAutoModeButton.Visible = true;
            }
        }

        public int BeforeAfterDraw()//No idea why.. but without this FullnessForeGround_AfterDraw wont work...
        {
            if (FullnessFG.Library == null) return -1;

            if (SelectedCreatureSlot < 0)
            {
                CreatureImage.Index = 0;
                CreatureImage.Animated = false;
                FullnessFG.Visible = false;
                FullnessMin.Visible = false;
                FullnessNow.Visible = false;
                return -1;
            }
            else
            {
                int selectedCreature = GetCreatureFromSlot(SelectedCreatureSlot);
                if (selectedCreature < 0)
                {
                    CreatureImage.Index = 0;
                    CreatureImage.Animated = false;
                    FullnessFG.Visible = false;
                    FullnessMin.Visible = false;
                    FullnessNow.Visible = false;
                    return -1;
                }
                FullnessFG.Visible = true;
                FullnessMin.Visible = true;
                FullnessNow.Visible = true;
                return selectedCreature;
            }
        }

        #region CreatureAnimation
        private void DrawCreatureAnimation()
        {
            int selectedCreature = BeforeAfterDraw();
            if (selectedCreature < 0) return;

            CreatureName.Text = GameScene.User.IntelligentCreatures[selectedCreature].CustomName;
            CreatureInfo.Text = GameScene.User.IntelligentCreatures[selectedCreature].CreatureRules.Info;
            CreatureInfo1.Text = GameScene.User.IntelligentCreatures[selectedCreature].CreatureRules.Info1;
            CreatureInfo2.Text = GameScene.User.IntelligentCreatures[selectedCreature].CreatureRules.Info2;
            //Expire
            if (GameScene.User.IntelligentCreatures[selectedCreature].ExpireTime == -9999)
                CreatureDeadline.Text = "Expire: Never";
            else
                CreatureDeadline.Text = string.Format("Expire: {0}", Functions.PrintTimeSpanFromSeconds(GameScene.User.IntelligentCreatures[selectedCreature].ExpireTime));
            //
            if (GameScene.User.IntelligentCreatures[selectedCreature].MaintainFoodTime == 0)
                CreatureMaintainFoodBuff.Text = "0";
            else
                CreatureMaintainFoodBuff.Text = string.Format("FoodBuff: {0}", Functions.PrintTimeSpanFromSeconds(GameScene.User.IntelligentCreatures[selectedCreature].MaintainFoodTime));

            int StartIndex = CreatureButtons[SelectedCreatureSlot].AnimDefaultIdx;
            int AnimCount = CreatureButtons[SelectedCreatureSlot].AnimDefaultCount;
            long AnimDelay = CreatureButtons[SelectedCreatureSlot].AnimDefaultDelay;

            if (AnimSwitched)
            {
                StartIndex = CreatureButtons[SelectedCreatureSlot].AnimExIdx;
                AnimCount = CreatureButtons[SelectedCreatureSlot].AnimExCount;
                AnimDelay = CreatureButtons[SelectedCreatureSlot].AnimExDelay;
            }

            if (SwitchAnimTime <= CMain.Time)//need switch
                if (!AnimSwitched) AnimNeedSwitch = true;

            bool AnimExFinished = false;
            if ((CreatureImage.Index - StartIndex) >= AnimCount - 1) AnimExFinished = true;

            CreatureImage.AnimationCount = AnimCount;
            CreatureImage.AnimationDelay = AnimDelay;
            CreatureImage.Index = StartIndex;//sets base.Index
            if (!CreatureImage.Animated) CreatureImage.Animated = true;

            if (AnimExFinished)
            {
                if (AnimNeedSwitch)
                {
                    SwitchAnimTime = CMain.Time + 8000;
                    AnimSwitched = true;
                }
                else if (AnimSwitched)
                {
                    SwitchAnimTime = CMain.Time + 8000;
                    AnimSwitched = false;
                }
                CreatureImage.OffSet = 0;
                AnimNeedSwitch = false;
            }
        }

        public int GetCreatureFromSlot(int slotidx)
        {
            for (int i = 0; i < GameScene.User.IntelligentCreatures.Count; i++)
            {
                if (GameScene.User.IntelligentCreatures[i].SlotIndex == slotidx) return i;
            }
            return -1;
        }
        #endregion

        #endregion

        public void SaveItemFilter(IntelligentCreatureItemFilter filter)
        {
            int selectedCreature = BeforeAfterDraw();
            if (selectedCreature < 0) return;

            GameScene.User.IntelligentCreatures[selectedCreature].Filter = filter;
            Network.Enqueue(new C.UpdateIntelligentCreature { Creature = GameScene.User.IntelligentCreatures[selectedCreature] });
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (GameScene.Scene.IntelligentCreatureOptionsGradeDialog.Visible) GameScene.Scene.IntelligentCreatureOptionsGradeDialog.BringToFront();
            if (GameScene.Scene.IntelligentCreatureOptionsDialog.Visible) GameScene.Scene.IntelligentCreatureOptionsDialog.BringToFront();
        }

        public void Hide()
        {
            if (!Visible) return;
            if (GameScene.Scene.IntelligentCreatureOptionsGradeDialog.Visible) GameScene.Scene.IntelligentCreatureOptionsGradeDialog.Hide();
            if (GameScene.Scene.IntelligentCreatureOptionsDialog.Visible) GameScene.Scene.IntelligentCreatureOptionsDialog.Hide();
            AnimSwitched = false;
            AnimNeedSwitch = false;
            Visible = false;
        }
        public void Show()
        {
            if (Visible) return;

            if (!GameScene.User.IntelligentCreatures.Any())
            {
                MirMessageBox messageBox = new MirMessageBox("You do not own any creatures.", MirMessageBoxButtons.OK);
                messageBox.Show();
                return;
            }

            if (!CreatureButtons.Any(x => x.Selected))
            {
                CreatureButtons[0].SelectButton();
            }


            Visible = true;
            showing = true;
            SwitchAnimTime = CMain.Time + 8000;
            AnimSwitched = false;
            AnimNeedSwitch = false;
            RefreshDialog();
        }
    }
    public sealed class CreatureButton : MirControl
    {
        public MirImageControl SelectionImage;
        public MirLabel NameLabel;
        public MirButton PetButton;
        public IntelligentCreatureType PetType = IntelligentCreatureType.None;
        public int idx;
        public bool Selected;

        public int AnimDefaultIdx = 540;
        public int AnimDefaultCount = 6;
        public long AnimDefaultDelay = 400;
        public int AnimExIdx = 550;
        public int AnimExCount = 5;
        public long AnimExDelay = 400;


        public CreatureButton()
        {
            Size = new Size(231, 33);

            PetButton = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(0, 0),
                Sound = SoundList.ButtonA,
            };
            PetButton.Click += PetButtonClick;
            PetButton.MouseEnter += PetButtonMouseEnter;
            PetButton.MouseLeave += PetButtonMouseLeave;

            SelectionImage = new MirImageControl
            {
                Index = 535,
                Library = Libraries.Prguse2,
                Location = new Point(-2, -2),
                Parent = this,
                NotControl = true,
                Visible = false,
            };

            NameLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(-22, -12),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Size = new Size(80, 15),
                NotControl = true,
                Visible = false,
            };

        }

        private void SetButtonInfo(ClientIntelligentCreature pet)
        {
            if (pet == null) return;

            PetType = pet.PetType;

            NameLabel.Text = pet.CustomName.ToString();

            PetButton.Index = pet.Icon;
            PetButton.PressedIndex = pet.Icon;

            SetCreatureFrames();
        }

        public void Update(ClientIntelligentCreature pet, bool setnew = false)
        {
            if (pet == null) return;
            if (PetType == IntelligentCreatureType.None || PetType != pet.PetType) setnew = true;//force new

            if (setnew) SetButtonInfo(pet);
            else
            {
                NameLabel.Text = pet.CustomName.ToString();
            }
        }

        void PetButtonClick(object sender, EventArgs e)
        {
            SelectButton();
        }
        void PetButtonMouseEnter(object sender, EventArgs e)
        {
            NameLabel.Visible = true;
        }
        void PetButtonMouseLeave(object sender, EventArgs e)
        {
            NameLabel.Visible = false;
        }

        public void SelectButton()
        {
            if (Selected) return;
            for (int i = 0; i < GameScene.Scene.IntelligentCreatureDialog.CreatureButtons.Length; i++)
            {
                if (i == idx) continue;
                GameScene.Scene.IntelligentCreatureDialog.CreatureButtons[i].SelectButton(false);
            }

            SelectButton(true);
            GameScene.Scene.IntelligentCreatureDialog.SelectedCreatureSlot = idx;
            GameScene.Scene.IntelligentCreatureDialog.SwitchAnimTime = CMain.Time + 10000;
            GameScene.Scene.IntelligentCreatureDialog.AnimSwitched = false;
            GameScene.Scene.IntelligentCreatureDialog.AnimNeedSwitch = false;
            GameScene.Scene.IntelligentCreatureDialog.Update();
        }
        private void SelectButton(bool selection)
        {
            Selected = selection;
            SelectionImage.Visible = Selected;
        }

        public void Clear()
        {
            PetType = IntelligentCreatureType.None;
            Visible = false;
            SelectButton(false);
        }

        private void SetCreatureFrames()
        {
            switch (PetType)
            {
                case IntelligentCreatureType.BabyPig:
                    AnimDefaultIdx = 540;
                    AnimDefaultCount = 6;
                    AnimDefaultDelay = 200;

                    AnimExIdx = 550;
                    AnimExCount = 5;
                    AnimExDelay = 300;
                    break;
                case IntelligentCreatureType.Chick:
                    AnimDefaultIdx = 570;
                    AnimDefaultCount = 4;
                    AnimDefaultDelay = 350;

                    AnimExIdx = 580;
                    AnimExCount = 10;
                    AnimExDelay = 200;
                    break;
                case IntelligentCreatureType.Kitten:
                    AnimDefaultIdx = 600;
                    AnimDefaultCount = 6;
                    AnimDefaultDelay = 250;

                    AnimExIdx = 610;
                    AnimExCount = 10;
                    AnimExDelay = 200;
                    break;
                case IntelligentCreatureType.BabySkeleton:
                    AnimDefaultIdx = 630;
                    AnimDefaultCount = 11;
                    AnimDefaultDelay = 200;

                    AnimExIdx = 650;
                    AnimExCount = 7;
                    AnimExDelay = 250;
                    break;
                case IntelligentCreatureType.Baekdon:
                    AnimDefaultIdx = 660;
                    AnimDefaultCount = 6;
                    AnimDefaultDelay = 250;

                    AnimExIdx = 670;
                    AnimExCount = 8;
                    AnimExDelay = 250;
                    break;
                case IntelligentCreatureType.Wimaen:
                    AnimDefaultIdx = 690;
                    AnimDefaultCount = 4;
                    AnimDefaultDelay = 350;

                    AnimExIdx = 700;
                    AnimExCount = 6;
                    AnimExDelay = 300;
                    break;
                case IntelligentCreatureType.BlackKitten:
                    AnimDefaultIdx = 720;
                    AnimDefaultCount = 6;
                    AnimDefaultDelay = 250;

                    AnimExIdx = 730;
                    AnimExCount = 10;
                    AnimExDelay = 200;
                    break;
                case IntelligentCreatureType.BabyDragon:
                    AnimDefaultIdx = 750;
                    AnimDefaultCount = 6;
                    AnimDefaultDelay = 300;

                    AnimExIdx = 760;
                    AnimExCount = 7;
                    AnimExDelay = 250;
                    break;
                case IntelligentCreatureType.OlympicFlame:
                    AnimDefaultIdx = 780;
                    AnimDefaultCount = 6;
                    AnimDefaultDelay = 300;

                    AnimExIdx = 790;
                    AnimExCount = 10;
                    AnimExDelay = 200;
                    break;
                case IntelligentCreatureType.BabySnowMan:
                    AnimDefaultIdx = 810;
                    AnimDefaultCount = 6;
                    AnimDefaultDelay = 300;

                    AnimExIdx = 820;
                    AnimExCount = 6;
                    AnimExDelay = 300;
                    break;
                case IntelligentCreatureType.Frog:
                    AnimDefaultIdx = 840;
                    AnimDefaultCount = 6;
                    AnimDefaultDelay = 300;
                    AnimExIdx = 850;
                    AnimExCount = 6;
                    AnimExDelay = 300;
                    break;
                case IntelligentCreatureType.BabyMonkey:
                    AnimDefaultIdx = 870;
                    AnimDefaultCount = 6;
                    AnimDefaultDelay = 300;
                    AnimExIdx = 880;
                    AnimExCount = 9;
                    AnimExDelay = 300;
                    break;
                case IntelligentCreatureType.AngryBird:
                    AnimDefaultIdx = 1400;
                    AnimDefaultCount = 12;
                    AnimDefaultDelay = 300;
                    AnimExIdx = 1332;
                    AnimExCount = 12;
                    AnimExDelay = 300;
                    break;
                case IntelligentCreatureType.Foxey:
                    AnimDefaultIdx = 1430;
                    AnimDefaultCount = 9;
                    AnimDefaultDelay = 300;
                    AnimExIdx = 1439;
                    AnimExCount = 8;
                    AnimExDelay = 300;
                    break;
                case IntelligentCreatureType.None:
                    AnimDefaultIdx = 539;
                    AnimDefaultCount = 1;
                    AnimDefaultDelay = 000;
                    AnimExIdx = 539;
                    AnimExCount = 1;
                    AnimExDelay = 000;
                    break;
            }
        }
    }
    public sealed class IntelligentCreatureOptionsDialog : MirImageControl
    {
        public readonly string[] OptionNames = { "All Items", "Gold", "Weapons", "Armours", "Helmets", "Boots", "Belts", "Jewelry", "Others" };
        public IntelligentCreatureItemFilter Filter;
        public Point locationOffset = new Point(450, 63);

        public MirButton OptionsSaveButton, OptionsCancelButton;
        public MirCheckBox[] CreatureOptions;

        public IntelligentCreatureOptionsDialog()
        {
            Index = 469;
            Library = Libraries.Title;
            Movable = false;
            Sort = true;
            Location = new Point(GameScene.Scene.IntelligentCreatureDialog.Location.X + locationOffset.X, GameScene.Scene.IntelligentCreatureDialog.Location.Y + locationOffset.Y);
            BeforeDraw += IntelligentCreatureOptionsDialog_BeforeDraw;

            CreatureOptions = new MirCheckBox[9];
            for (int i = 0; i < CreatureOptions.Length; i++)
            {
                int offsetY = i * 30;
                CreatureOptions[i] = new MirCheckBox { Index = 2086, UnTickedIndex = 2086, TickedIndex = 2087, Parent = this, Location = new Point(16, (16 + offsetY)), Library = Libraries.Prguse };
                CreatureOptions[i].LabelText = OptionNames[i];
                CreatureOptions[i].Click += CheckBoxClick;
            }

            OptionsSaveButton = new MirButton
            {
                HoverIndex = 587,
                Index = 586,
                Location = new Point(10, 280),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 588,
                Sound = SoundList.ButtonA,
            };
            OptionsSaveButton.Click += ButtonClick;

            OptionsCancelButton = new MirButton
            {
                HoverIndex = 591,
                Index = 590,
                Location = new Point(60, 280),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 592,
                Sound = SoundList.ButtonA,
            };
            OptionsCancelButton.Click += ButtonClick;
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if (sender == OptionsSaveButton)
            {
                Filter.PickupGrade = GameScene.Scene.IntelligentCreatureOptionsGradeDialog.GradeType;
                GameScene.Scene.IntelligentCreatureOptionsGradeDialog.Hide();
                GameScene.Scene.IntelligentCreatureDialog.SaveItemFilter(Filter);
                Hide();
            }
            if (sender == OptionsCancelButton)
            {
                Filter = new IntelligentCreatureItemFilter();
                GameScene.Scene.IntelligentCreatureOptionsGradeDialog.GradeType = ItemGrade.None;
                GameScene.Scene.IntelligentCreatureOptionsGradeDialog.RefreshGradeFilter();
                GameScene.Scene.IntelligentCreatureOptionsGradeDialog.Hide();
                RefreshFilter();
                Hide();
            }
        }
        private void CheckBoxClick(object sender, EventArgs e)
        {
            for (int i = 0; i < CreatureOptions.Length; i++)
            {
                if (CreatureOptions[i] != sender) continue;
                Filter.SetItemFilter(i);
                break;
            }
            RefreshFilter();
        }

        void IntelligentCreatureOptionsDialog_BeforeDraw(object sender, EventArgs e)
        {
            if (!GameScene.Scene.IntelligentCreatureDialog.Visible)
            {
                Hide();
                return;
            }
            Location = new Point(GameScene.Scene.IntelligentCreatureDialog.Location.X + locationOffset.X, GameScene.Scene.IntelligentCreatureDialog.Location.Y + locationOffset.Y);
        }

        private void RefreshFilter()
        {
            for (int i = 0; i < CreatureOptions.Length; i++)
            {
                switch (i)
                {
                    case 0://all items
                        CreatureOptions[i].Checked = Filter.PetPickupAll;
                        break;
                    case 1://gold
                        CreatureOptions[i].Checked = Filter.PetPickupGold;
                        break;
                    case 2://weapons
                        CreatureOptions[i].Checked = Filter.PetPickupWeapons;
                        break;
                    case 3://armours
                        CreatureOptions[i].Checked = Filter.PetPickupArmours;
                        break;
                    case 4://helmets
                        CreatureOptions[i].Checked = Filter.PetPickupHelmets;
                        break;
                    case 5://boots
                        CreatureOptions[i].Checked = Filter.PetPickupBoots;
                        break;
                    case 6://belts
                        CreatureOptions[i].Checked = Filter.PetPickupBelts;
                        break;
                    case 7://jewelry
                        CreatureOptions[i].Checked = Filter.PetPickupAccessories;
                        break;
                    case 8://others
                        CreatureOptions[i].Checked = Filter.PetPickupOthers;
                        break;
                }
            }
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }
        public void Show(IntelligentCreatureItemFilter filter)
        {
            if (Visible) return;
            Filter = filter;
            Visible = true;
            RefreshFilter();
        }
    }
    public sealed class IntelligentCreatureOptionsGradeDialog : MirImageControl
    {
        private string[] GradeStrings = { "All", "Common", "Rare", "Mythical", "Legendary" };

        public MirButton NextButton, PrevButton;
        public MirLabel GradeLabel;
        public int SelectedGrade = 0;
        public ItemGrade GradeType;

        public Point locationOffset = new Point(449, 39);

        public IntelligentCreatureOptionsGradeDialog()
        {
            Index = 237;
            Library = Libraries.Prguse;
            Movable = false;
            Sort = true;
            Location = new Point(GameScene.Scene.IntelligentCreatureDialog.Location.X + locationOffset.X, GameScene.Scene.IntelligentCreatureDialog.Location.Y + locationOffset.Y);
            BeforeDraw += IntelligentCreatureOptionsGradeDialog_BeforeDraw;

            NextButton = new MirButton()
            {
                HoverIndex = 396,
                Index = 396,
                Location = new Point(96, 5),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 397,
                Sound = SoundList.ButtonA,
            };
            NextButton.Click += Button_Click;

            PrevButton = new MirButton()
            {
                HoverIndex = 398,
                Index = 398,
                Location = new Point(76, 5),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 399,
                Sound = SoundList.ButtonA,
            };
            PrevButton.Click += Button_Click;

            GradeLabel = new MirLabel()
            {
                Parent = this,
                Location = new Point(8, 0),
                DrawFormat = TextFormatFlags.VerticalCenter,
                Size = new Size(70, 21),
                NotControl = true,
            };
        }

        void Button_Click(object sender, EventArgs e)
        {
            if (sender == NextButton)
            {
                SelectedGrade++;
                if (SelectedGrade >= GradeStrings.Length) SelectedGrade = GradeStrings.Length - 1;
            }
            if (sender == PrevButton)
            {
                SelectedGrade--;
                if (SelectedGrade <= 0) SelectedGrade = 0;
            }

            GradeLabel.Text = GradeStrings[SelectedGrade];
            GradeType = (ItemGrade)((byte)SelectedGrade);

            GradeLabel.ForeColour = GradeNameColor(GradeType);
        }

        private Color GradeNameColor(ItemGrade grade)
        {
            switch (grade)
            {
                case ItemGrade.Common:
                    return Color.Yellow;
                case ItemGrade.Rare:
                    return Color.DeepSkyBlue;
                case ItemGrade.Legendary:
                    return Color.DarkOrange;
                case ItemGrade.Mythical:
                    return Color.Plum;
                default:
                    return Color.White;
            }
        }

        // public override void OnMouseDown(MouseEventArgs e)
        //{
        //     GameScene.Scene.IntelligentCreatureOptionsDialog.BringToFront();
        //    base.OnMouseDown(e);
        // }

        void IntelligentCreatureOptionsGradeDialog_BeforeDraw(object sender, EventArgs e)
        {
            if (!GameScene.Scene.IntelligentCreatureDialog.Visible)
            {
                Hide();
                return;
            }
            Location = new Point(GameScene.Scene.IntelligentCreatureDialog.Location.X + locationOffset.X, GameScene.Scene.IntelligentCreatureDialog.Location.Y + locationOffset.Y);
        }

        public void RefreshGradeFilter()
        {
            SelectedGrade = (int)((byte)GradeType);
            GradeLabel.Text = GradeStrings[SelectedGrade];
            GradeLabel.ForeColour = GradeNameColor(GradeType);
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }

        public void Show(ItemGrade grade)
        {
            if (Visible) return;
            Visible = true;
            GradeType = grade;
            RefreshGradeFilter();
        }
    }
}
