using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public class PositionMoveDialog : MirImageControl
    {
        public MirButton RememberButton, PositionMoveButton, PositionDeleteButton, CloseButton, UpButton, DownButton, PositionBar;
        private MoveCell[] Movers;
        public static List<PlayerTeleportInfo> MoveList;
        public static int SelectIndex = 0;
        private int ScrollIndex = 0;
        private int ShowCount = 1;
        private int PageRows = 24;
        private int MapIndex = -1;
        private int ScrollBarBaseX = 717;
        string PositionName = string.Empty;

        public PositionMoveDialog()
        {
            Index = 860;
            Library = Libraries.Title;
            Movable = true;

            BeforeDraw += (o, e) => OnBeforeDraw();

            Sort = true;
            Location = Center;
            RememberButton = new MirButton
            {
                Index = 880,
                HoverIndex = 881,
                PressedIndex = 882,
                Location = new Point(10, 270),
                Library = Libraries.Title,
                Parent = this,
                Hint = "Input the location coordinates",
                Sound = SoundList.ButtonC
            };
            RememberButton.Click += (o, e) =>
            {
                new RememberMoveDialog(PositionName);
            };

            PositionMoveButton = new MirButton
            {
                Index = 883,
                HoverIndex = 884,
                PressedIndex = 885,
                Location = new Point(70, 270),
                Library = Libraries.Title,
                Parent = this,
                Hint = "Teleport to the marked location",
                Sound = SoundList.ButtonC
            };
            PositionMoveButton.Click += (o, e) =>
            {
                MirMessageBox mirMessageBox = new MirMessageBox("Do you want to move to the selected map location?\nThis teleport will cost 3000 gold.", MirMessageBoxButtons.YesNo);
                mirMessageBox.Show();
                mirMessageBox.YesButton.Click += (o1, e1) =>
                {
                    Network.Enqueue(new PositionMove { SelectIndex = PositionMoveDialog.SelectIndex });
                    Hide();
                };
            };

            PositionDeleteButton = new MirButton
            {
                Index = 886,
                HoverIndex = 887,
                PressedIndex = 858,
                Location = new Point(130, 270),
                Library = Libraries.Title,
                Parent = this,
                Hint = "Remove the designated point",
                Sound = SoundList.ButtonC
            };
            PositionDeleteButton.Click += (o, e) =>
            {
                MirMessageBox mirMessageBox = new MirMessageBox("Are you sure you want to delete the selected location?\nThis action cannot be undone.", MirMessageBoxButtons.YesNo);
                mirMessageBox.Show();
                mirMessageBox.YesButton.Click += (o1, e1) =>
                {
                    Network.Enqueue(new Chat
                    {
                        Message = "@DELETELOCATIONMEMORY " + PositionMoveDialog.SelectIndex.ToString()
                    });
                    UpdateMoveCells();

                };
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(174, 5),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA
            };
            CloseButton.Click += (o, e) => Hide();

            UpButton = new MirButton
            {
                HoverIndex = 198,
                Index = 197,
                Visible = true,
                Library = Libraries.Prguse2,
                Location = new Point(ScrollBarBaseX - 540, 34),
                Size = new Size(16, 14),
                Parent = this,
                PressedIndex = 199,
                Sound = SoundList.ButtonA
            };
            UpButton.Click += (o, e) =>
            {
                if (ScrollIndex != 0)
                {
                    ScrollIndex--;
                    UpdateMoveCells();
                    UpdateScrollPosition();
                }
            };

            DownButton = new MirButton
            {
                HoverIndex = 208,
                Index = 207,
                Visible = true,
                Library = Libraries.Prguse2,
                Location = new Point(ScrollBarBaseX - 540, 250),
                Size = new Size(16, 14),
                Parent = this,
                PressedIndex = 209,
                Sound = SoundList.ButtonA
            };
            DownButton.Click += (o, e) =>
            {
                if (ScrollIndex == ShowCount - PageRows)
                    return;
                ScrollIndex++;
                UpdateMoveCells();
                UpdateScrollPosition();

            };

            PositionBar = new MirButton
            {
                Index = 206,
                Library = Libraries.Prguse2,
                Location = new Point(ScrollBarBaseX - 540, 46),
                Parent = this,
                Movable = true,
                Sound = SoundList.None
            };
            PositionBar.OnMoving += PositionBar_OnMoving;
        }

        private void OnBeforeDraw()
        {
            MapControl map = GameScene.Scene.MapControl;

            if (map == null || !Visible) return;

            PositionName = string.Format(map.Title + "   " + GameScene.User.CurrentLocation.X + " : " + GameScene.User.CurrentLocation.Y);

        }
        public void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int scrollBarBaseX = ScrollBarBaseX;
            int newPositionY = PositionBar.Location.Y;
            if (PositionBar.Location.Y >= DownButton.Location.Y - 15)
            {
                newPositionY = DownButton.Location.Y - 15;
            }
            if (newPositionY < UpButton.Location.Y + 15)
            {
                newPositionY = UpButton.Location.Y + 15;
            }
            int adjustedPositionY = newPositionY - 62;
            int scrollRatio = 366 / (ShowCount - PageRows);
            double d = (double)(adjustedPositionY / scrollRatio);
            ScrollIndex = (int)Convert.ToInt16(Math.Floor(d));
            if (ScrollIndex > ShowCount - PageRows)
            {
                ScrollIndex = ShowCount - PageRows;
            }
            if (ScrollIndex <= 0)
            {
                ScrollIndex = 0;
            }
            UpdateMoveCells();
            PositionBar.Location = new Point(scrollBarBaseX - 540, newPositionY);
        }

        private void KeyPanel_MouseWheel(object sender, MouseEventArgs e)
        {

            int scrollSteps = e.Delta / SystemInformation.MouseWheelScrollDelta;

            bool isAtTop = ScrollIndex == 0 && scrollSteps >= 0;
            bool isAtBottom = ScrollIndex == ShowCount - PageRows && scrollSteps <= 0;

            if (!isAtTop && !isAtBottom)
            {
                ScrollIndex -= (scrollSteps > 0) ? 1 : -1;

                UpdateMoveCells();
                UpdateScrollPosition();
            }
        }

        private void UpdateScrollPosition()
        {
            int scrollRatio = 366 / (ShowCount - PageRows);
            int scrollBarBaseX = ScrollBarBaseX;
            int newPositionY = 62 + ScrollIndex * scrollRatio;
            if (newPositionY >= DownButton.Location.Y - 15)
            {
                newPositionY = DownButton.Location.Y - 15;
            }
            if (newPositionY < UpButton.Location.Y + 15)
            {
                newPositionY = UpButton.Location.Y + 15;
            }
            PositionBar.Location = new Point(scrollBarBaseX - 540, newPositionY);
        }

        public void UpdateMoveCells()
        {
            if (ShowCount < PageRows)
            {
                ScrollIndex = 0;
            }
            for (int i = 0; i < Movers.Length; i++)
            {
                Movers[i].Location = new Point(15, 50 + i * 16 - ScrollIndex * 16 - 10);
                if (ScrollIndex <= i && ScrollIndex + PageRows > i)
                {
                    Movers[i].Show();
                }
                else
                {
                    Movers[i].Hide();
                }
            }
        }
        private void ClearMoveList()
        {
            bool flag = Movers == null;
            if (!flag)
            {
                for (int i = 0; i < Movers.Length; i++)
                {
                    Movers[i].Dispose();
                }
            }
        }

        public void ReloadMoveList()
        {
            ClearMoveList();
            if (PositionMoveDialog.MoveList != null)
            {
                int count = PositionMoveDialog.MoveList.Count;
                Movers = new MoveCell[count];
                ShowCount = Movers.Length;
                for (int i = 0; i < count; i++)
                {
                    Movers[i] = new MoveCell
                    {
                        Parent = this,
                        Location = new Point(15, 50 + i * 16 - 10),
                        Size = new Size(115, 16),
                        MoveLocation = PositionMoveDialog.MoveList[i].Location,
                        Name = PositionMoveDialog.MoveList[i].Name,
                        ColorIndex = PositionMoveDialog.MoveList[i].ColorIndex,
                        Index = i,
                    };
                    Movers[i].MouseWheel += KeyPanel_MouseWheel;
                }
                UpdateMoveCells();
            }
        }
        public void Show()
        {
            bool visible = Visible;
            if (!visible)
            {
                Visible = true;
            }
        }

        public void Hide()
        {
            bool flag = !Visible;
            if (!flag)
            {
                Visible = false;
            }
        }
    }

    public sealed class MoveCell : MirControl
    {
        public int Index;
        public Point MoveLocation;
        private MirLabel NameLabel;
        public MirImageControl SelectedImage;
        public int ColorIndex;
        public string Name
        {
            get
            {
                return (NameLabel != null) ? NameLabel.Text : string.Empty;
            }
            set
            {
                bool flag;
                string text = GetText(value, out flag) + (flag ? ".." : "");
                if (text != NameLabel.Text)
                {
                    NameLabel.Text = text;
                }
            }
        }

        public MoveCell()
        {
            SelectedImage = new MirImageControl
            {
                Index = 1346,
                Library = Libraries.Prguse2,
                Location = new Point(0, -4),
                Parent = this,
                Visible = false
            };
            Border = false;
            base.BorderColour = Color.Transparent;
            base.BeforeDraw += Border_BeforeDraw;
            base.Click += delegate (object o, EventArgs e)
            {
                PositionMoveDialog.SelectIndex = Index;
            };
            NameLabel = new MirLabel
            {
                Text = "text",
                Location = new Point(0, 0),
                Parent = this,
                AutoSize = true,
                DrawFormat = TextFormatFlags.Default,
                Font = new Font(Settings.FontName, 8f),
                ForeColour = Color.White,
                NotControl = true
            };
        }

        private string GetText(string text, out bool isOver)
        {
            isOver = false;
            for (int i = 0; i < text.Length; i++)
            {
                string text2 = text.Remove(i);
                if (Encoding.Default.GetBytes(text2).Length >= 30)
                {
                    isOver = true;
                    return text2;
                }
            }
            return text;
        }

        private void Border_BeforeDraw(object sender, EventArgs e)
        {
            if (MirControl.MouseControl != null)
            {
                Border = (IsMouseOver(MirControl.MouseControl.DisplayLocation) || PositionMoveDialog.SelectIndex == Index);
                if (GameScene.User != null)
                {
                    NameLabel.ForeColour = GetColor();
                }
                if (MirControl.MouseControl == null)
                {
                }
            }
        }

        private Color GetColor()
        {
            if (GameScene.User == null)
            {
                return Color.White;
            }
            else
            {
                if (PositionMoveDialog.SelectIndex == Index)
                {
                    return Color.Red;
                }
                else
                {
                    //return Color.White;
                    switch (ColorIndex)
                    {
                        case 0:
                            return Color.White;
                        case 1:
                            return Color.Gray;
                        case 2:
                            return Color.Orange;
                        case 3:
                            return Color.Red;
                        case 4:
                            return Color.Pink;
                        case 5:
                            return Color.Yellow;
                        case 6:
                            return Color.LightPink;
                        case 7:
                            return Color.Green;
                        case 8:
                            return Color.Blue;
                        case 9:
                            return Color.LightSkyBlue;
                        default:
                            return Color.White;

                    }
                }
            }
        }

        public void Show()
        {
            if (!Visible)
            {
                Visible = true;
                Network.Enqueue(new Chat { Message = "@LocationMoveList" });
            }
        }

        public void Hide()
        {
            if (Visible)
            {
                Visible = false;
            }
        }

        public void Toggle()
        {
            Visible = !Visible;
            if (Visible)
            {
                Network.Enqueue(new Chat { Message = "@LocationMoveList" });
                Show();
            }
            else
            {
                Hide();
            }
            Redraw();
        }
    }

    public sealed class RememberMoveDialog : MirImageControl
    {
        public readonly MirButton OKButton, CancelButton;
        public readonly MirTextBox InputTextBox;
        public readonly MirLabel CaptionLabel;
        public MirControl[] ColorIcon;
        public static int SelectIndex = 0;

        public RememberMoveDialog(string name)
        {
            Index = 860;
            Library = Libraries.Title;
            Parent = GameScene.Scene;
            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);
            Modal = true;
            Visible = true;

            CaptionLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.WordBreak,
                Location = new Point(12, 35),
                Size = new Size(235, 40),
                Parent = this,
                Text = "Please specify the name of the location",
            };

            InputTextBox = new MirTextBox
            {
                Parent = this,
                Location = new Point(17, 57),
                Size = new Size(215, 5),
                MaxLength = 50,
                Text = name,
                Font = new Font(Settings.FontName, 8F),
            };
            InputTextBox.SetFocus();
            InputTextBox.TextBox.KeyPress += MirInputBox_KeyPress;

            OKButton = new MirButton
            {
                HoverIndex = 201,
                Index = 200,
                Library = Libraries.Title,
                Location = new Point(60, 123),
                Parent = this,
                PressedIndex = 202,
            };
            OKButton.Click += (o, e) =>
            {
                if (PositionMoveDialog.MoveList.Count >= Globals.MaxPositionMove)
                {
                    MirMessageBox messagea = new MirMessageBox("Cannot save more locations");
                    messagea.Show();
                    return;
                }
                Network.Enqueue(new MemoryLocation { Name = InputTextBox.Text, ColorIndex = RememberMoveDialog.SelectIndex });
                Dispose();
                GameScene.Scene.PositionMoveDialog.UpdateMoveCells();

            };

            CancelButton = new MirButton
            {
                HoverIndex = 204,
                Index = 203,
                Library = Libraries.Title,
                Location = new Point(160, 123),
                Parent = this,
                PressedIndex = 205,
            };
            CancelButton.Click += DisposeDialog;

            ColorIcon = new MirControl[12];

            for (int i = 0; i < ColorIcon.Length; i++)
            {
                ColorIcon[i] = new ColorIcon
                {
                    Parent = this,
                    Location = new Point(24 + (i * 17), 85),
                    Size = new Size(15, 15),
                    Index = i,
                };
            }
        }
        public void MirInputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (OKButton != null && !OKButton.IsDisposed)
                    OKButton.InvokeMouseClick(EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                if (CancelButton != null && !CancelButton.IsDisposed)
                    CancelButton.InvokeMouseClick(EventArgs.Empty);
                e.Handled = true;
            }
        }
        public void DisposeDialog(object sender, EventArgs e)
        {
            Dispose();
        }
    }

    public sealed class ColorIcon : MirControl
    {
        public int Index;
        public ColorIcon()
        {
            Border = true;
            BorderColour = Color.Gold;
            BeforeDraw += Border_BeforeDraw;
            base.Click += delegate (object o, EventArgs e)
            {
                RememberMoveDialog.SelectIndex = Index;
            };
        }

        private void Border_BeforeDraw(object sender, EventArgs e)
        {
            if (MirControl.MouseControl != null)
            {
                Border = (IsMouseOver(MirControl.MouseControl.DisplayLocation) || RememberMoveDialog.SelectIndex == Index);
            }
        }
    }
}