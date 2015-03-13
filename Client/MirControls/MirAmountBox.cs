using System;
using System.Drawing;
using System.Windows.Forms;
using Client.MirGraphics;
using Client.MirSounds;

namespace Client.MirControls
{
    public sealed class MirAmountBox : MirImageControl
    {
        public MirLabel TitleLabel, TextLabel;
        public MirButton OKButton, CancelButton, CloseButton;
        public MirTextBox InputTextBox;
        public MirControl ItemImage;
        public int ImageIndex;
        public uint Amount, MinAmount, MaxAmount;

        public MirAmountBox(string title, int image, uint max, uint min = 0)
        {
            ImageIndex = image;
            MaxAmount = max;
            MinAmount = min;
            Amount = max;
            Modal = true;
            Movable = false;

            Index = 238;
            Library = Libraries.Prguse;

            Location = new Point((800 - Size.Width) / 2, (600 - Size.Height) / 2);

            TitleLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(19, 8),
                Parent = this,
                NotControl = true,
                Text = title
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(180, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Dispose();

            ItemImage = new MirControl
            {
                Location = new Point(15, 34),
                Size = new Size(38, 34),
                Parent = this,
            };
            ItemImage.AfterDraw += (o, e) => DrawItem();

            OKButton = new MirButton
            {
                HoverIndex = 201,
                Index = 200,
                Library = Libraries.Title,
                Location = new Point(23, 76),
                Parent = this,
                PressedIndex = 202,
            };
            OKButton.Click += (o, e) => Dispose();

            CancelButton = new MirButton
            {
                HoverIndex = 204,
                Index = 203,
                Library = Libraries.Title,
                Location = new Point(110, 76),
                Parent = this,
                PressedIndex = 205,
            };
            CancelButton.Click += (o, e) => Dispose();

            InputTextBox = new MirTextBox
            {
                Parent = this,
                Border = true,
                BorderColour = Color.Lime,
                Location = new Point(58, 43),
                Size = new Size(132, 19),
            };
            InputTextBox.SetFocus();
            InputTextBox.TextBox.KeyPress += MirInputBox_KeyPress;
            InputTextBox.TextBox.TextChanged += TextBox_TextChanged;
            InputTextBox.Text = MaxAmount.ToString();
            InputTextBox.TextBox.SelectionStart = 0;
            InputTextBox.TextBox.SelectionLength = InputTextBox.Text.Length;

        }
        public MirAmountBox(string title, int image, string message)
        {
            ImageIndex = image;

            Modal = true;
            Movable = false;

            Index = 238;
            Library = Libraries.Prguse;

            Location = new Point((800 - Size.Width) / 2, (600 - Size.Height) / 2);



            TitleLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(19, 8),
                Parent = this,
                NotControl = true,
                Text = title
            };

            TextLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(60, 43),
                ForeColour = Color.Yellow,
                Parent = this,
                NotControl = true,
                Text = message
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(180, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Dispose();

            ItemImage = new MirControl
            {
                Location = new Point(15, 34),
                Size = new Size(38, 34),
                Parent = this,
            };
            ItemImage.AfterDraw += (o, e) => DrawItem();

            OKButton = new MirButton
            {
                HoverIndex = 201,
                Index = 200,
                Library = Libraries.Title,
                Location = new Point(23, 76),
                Parent = this,
                PressedIndex = 202,
            };
            OKButton.Click += (o, e) => Dispose();

            CancelButton = new MirButton
            {
                HoverIndex = 204,
                Index = 203,
                Library = Libraries.Title,
                Location = new Point(110, 76),
                Parent = this,
                PressedIndex = 205,
            };
            CancelButton.Click += (o, e) => Dispose();
        }

        void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (uint.TryParse(InputTextBox.Text, out Amount) && Amount >= MinAmount)
            {
                InputTextBox.BorderColour = Color.Lime;

                OKButton.Visible = true;
                if (Amount > MaxAmount)
                {
                    Amount = MaxAmount;
                    InputTextBox.Text = MaxAmount.ToString();
                    InputTextBox.TextBox.SelectionStart = InputTextBox.Text.Length;
                }

                if (Amount == MaxAmount)
                    InputTextBox.BorderColour = Color.Orange;
            }
            else
            {
                InputTextBox.BorderColour = Color.Red;
                OKButton.Visible = false;
            }
        }

        void MirInputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

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

        void DrawItem()
        {
            int x = ItemImage.DisplayLocation.X, y = ItemImage.DisplayLocation.Y;

            Size s = Libraries.Items.GetTrueSize(ImageIndex);

            x += (ItemImage.Size.Width - s.Width) / 2;
            y += (ItemImage.Size.Height - s.Height) / 2;

            Libraries.Items.Draw(ImageIndex, x, y);
        }

        public void Show()
        {
            if (Parent != null) return;

            Parent = MirScene.ActiveScene;

            Highlight();

            for (int i = 0; i < Program.Form.Controls.Count; i++)
            {
                TextBox T = Program.Form.Controls[i] as TextBox;
                if (T != null && T.Tag != null && T.Tag != null)
                    ((MirTextBox)T.Tag).DialogChanged();
            }
            
            /*
            CMain.Shift = false;
            CMain.Ctrl = false;
            CMain.Alt = false;

            Parent = MirScene.ActiveScene;
            Activate();
            Highlight();

            for (int i = 0; i < Main.This.Controls.Count; i++)
            {
                TextBox T = (TextBox)Main.This.Controls[i];
                if (T != null && T.Tag != null && T.Tag != null)
                    ((MirTextBox)T.Tag).DialogChanged();
            }*/
        }
        public override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            e.Handled = true;
        }
        public override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            e.Handled = true;
        }
        public override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.KeyChar == (char)Keys.Escape)
                CancelButton.InvokeMouseClick(EventArgs.Empty);
            else if (e.KeyChar == (char)Keys.Enter)
                OKButton.InvokeMouseClick(EventArgs.Empty);
            e.Handled = true;
        }

        #region Disposable

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            for (int i = 0; i < Program.Form.Controls.Count; i++)
            {
                TextBox T = (TextBox)Program.Form.Controls[i];
                if (T != null && T.Tag != null && T.Tag != null)
                    ((MirTextBox)T.Tag).DialogChanged();
            }
        }

        #endregion
    }
}
