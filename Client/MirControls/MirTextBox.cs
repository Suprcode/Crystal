using Client.MirGraphics;
using SlimDX;
using SlimDX.Direct3D9;
using System.Drawing.Imaging;

namespace Client.MirControls
{
    public sealed class MirTextBox : MirControl
    {
        #region Back Color

        protected override void OnBackColourChanged()
        {
            base.OnBackColourChanged();
            if (TextBox != null && !TextBox.IsDisposed)
                TextBox.BackColor = BackColour;
        }

        #endregion

        #region Enabled

        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();
            if (TextBox != null && !TextBox.IsDisposed)
                TextBox.Enabled = Enabled;
        }

        #endregion

        #region Fore Color

        protected override void OnForeColourChanged()
        {
            base.OnForeColourChanged();
            if (TextBox != null && !TextBox.IsDisposed)
                TextBox.ForeColor = ForeColour;
        }

        #endregion

        #region Location

        protected override void OnLocationChanged()
        {
            base.OnLocationChanged();
            if (TextBox != null && !TextBox.IsDisposed)
                TextBox.Location = DisplayLocation;

            TextureValid = false;
            Redraw();
        }

        #endregion

        #region Max Length

        public int MaxLength
        {
            get
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    return TextBox.MaxLength;
                return -1;
            }
            set
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    TextBox.MaxLength = value;
            }
        }

        #endregion

        #region Parent

        protected override void OnParentChanged()
        {
            base.OnParentChanged();
            if (TextBox != null && !TextBox.IsDisposed)
                OnVisibleChanged();
        }

        #endregion

        #region Password

        public bool Password
        {
            get
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    return TextBox.UseSystemPasswordChar;
                return false;
            }
            set
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    TextBox.UseSystemPasswordChar = value;
            }
        }

        #endregion

        #region Font

        public System.Drawing.Font Font
        {
            get
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    return TextBox.Font;
                return null;
            }
            set
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    TextBox.Font = ScaleFont(value);
            }
        }

        #endregion

        #region Size

        protected override void OnSizeChanged()
        {
            TextBox.Size = Size;

            DisposeTexture();

            _size = Size;

            if (TextBox != null && !TextBox.IsDisposed)
                base.OnSizeChanged();
        }

        #endregion
        
        #region TextBox

        public bool CanLoseFocus;
        public readonly TextBox TextBox;
        private Pen CaretPen;

        #endregion

        #region Label

        public string Text
        {
            get
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    return TextBox.Text;
                return null;
            }
            set
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    TextBox.Text = value;
            }
        }
        public string[] MultiText
        {
            get
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    return TextBox.Lines;
                return null;
            }
            set
            {
                if (TextBox != null && !TextBox.IsDisposed)
                    TextBox.Lines = value;
            }
        }

        #endregion

        #region Visible

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                OnVisibleChanged();
            }
        }

        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            if (TextBox != null && !TextBox.IsDisposed)
                TextBox.Visible = Visible;
        }
        private void TextBox_VisibleChanged(object sender, EventArgs e)
        {
            DialogChanged();

            if (TextBox.Visible && TextBox.CanFocus)
                if (Program.Form.ActiveControl == null || Program.Form.ActiveControl == Program.Form)
                    Program.Form.ActiveControl = TextBox;

            if (!TextBox.Visible)
                if (Program.Form.ActiveControl == TextBox)
                    Program.Form.Focus();
        }
        private void SetFocus(object sender, EventArgs e)
        {
            if (TextBox.Visible)
                TextBox.VisibleChanged -= SetFocus;
            if (TextBox.Parent != null)
                TextBox.ParentChanged -= SetFocus;

            if (TextBox.CanFocus) TextBox.Focus();
            else if (TextBox.Visible && TextBox.Parent != null)
                Program.Form.ActiveControl = TextBox;


        }

        #endregion

        #region MultiLine

        public override void MultiLine()
        {
            TextBox.Multiline = true;
            TextBox.Size = Size;

            DisposeTexture();
            Redraw();
        }

        #endregion

        public MirTextBox()
        {
            BackColour = Color.Black;

            DrawControlTexture = true;
            TextureValid = false;

            TextBox = new TextBox
            {
                BackColor = BackColour,
                BorderStyle = BorderStyle.None,
                Font = new System.Drawing.Font(Settings.FontName, 10F * 96f / CMain.Graphics.DpiX),
                ForeColor = ForeColour,
                Location = DisplayLocation,
                Size = Size,
                Visible = Visible,
                Tag = this,
                Cursor = CMain.Cursors[(byte)MouseCursor.TextPrompt]
            };

            CaretPen = new Pen(ForeColour, 1);

            TextBox.VisibleChanged += TextBox_VisibleChanged;
            TextBox.ParentChanged += TextBox_VisibleChanged;
            TextBox.KeyUp += TextBoxOnKeyUp;  
            TextBox.KeyPress += TextBox_KeyPress;

            TextBox.KeyPress += TextBox_NeedRedraw;
            TextBox.KeyUp += TextBox_NeedRedraw;
            TextBox.MouseDown += TextBox_NeedRedraw;
            TextBox.MouseUp += TextBox_NeedRedraw;
            TextBox.LostFocus += TextBox_NeedRedraw;
            TextBox.GotFocus += TextBox_NeedRedraw;
            TextBox.MouseWheel += TextBox_NeedRedraw;

            Shown += MirTextBox_Shown;
            TextBox.MouseMove += CMain.CMain_MouseMove;
        }

        private void TextBox_NeedRedraw(object sender, EventArgs e)
        {
            TextureValid = false;
            Redraw();
        }

        protected unsafe override void CreateTexture()
        {
            if (!Settings.FullScreen) return;

            if (Size.IsEmpty)
                return;

            if (TextureSize != Size)
                DisposeTexture();

            if (ControlTexture == null || ControlTexture.Disposed)
            {
                DXManager.ControlList.Add(this);

                ControlTexture = new Texture(DXManager.Device, Size.Width, Size.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
                TextureSize = Size;
            }

            Point caret = GetCaretPosition();

            DataRectangle stream = ControlTexture.LockRectangle(0, LockFlags.Discard);
            using (Bitmap bm = new Bitmap(Size.Width, Size.Height, Size.Width * 4, PixelFormat.Format32bppArgb, stream.Data.DataPointer))
            {
                TextBox.DrawToBitmap(bm, new Rectangle(0, 0, Size.Width, Size.Height));
                using (Graphics graphics = Graphics.FromImage(bm))
                {
                    graphics.DrawImage(bm, Point.Empty);
                    if (TextBox.Focused)
                        graphics.DrawLine(CaretPen, new Point(caret.X, caret.Y), new Point(caret.X, caret.Y + TextBox.Font.Height));
                }

            }
            ControlTexture.UnlockRectangle(0);
            DXManager.Sprite.Flush();
            TextureValid = true;
        }

        private Point GetCaretPosition()
        {
            Point result = TextBox.GetPositionFromCharIndex(TextBox.SelectionStart);

            if (result.X == 0 && TextBox.Text.Length > 0)
            {
                result = TextBox.GetPositionFromCharIndex(TextBox.Text.Length - 1);
                int s = result.X / TextBox.Text.Length;
                result.X = (int)(result.X + (s * 1.46));
                result.Y = TextBox.GetLineFromCharIndex(TextBox.SelectionStart) * TextBox.Font.Height;
            }

            return result;
        }

        private void TextBoxOnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.PrintScreen:
                    CMain.CMain_KeyUp(sender, e);
                    break;

            }
        }

        void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.KeyChar == (char)Keys.Escape)
            {
                Program.Form.ActiveControl = null;
                e.Handled = true;
            }
        }


        void MirTextBox_Shown(object sender, EventArgs e)
        {
            TextBox.Parent = Program.Form;
            CMain.Ctrl = false;
            CMain.Shift = false;
            CMain.Alt = false;
            CMain.Tilde = false;

            TextureValid = false;
            SetFocus();
        }

        public void SetFocus()
        {
            if (!TextBox.Visible)
                TextBox.VisibleChanged += SetFocus;
            else if (TextBox.Parent == null)
                TextBox.ParentChanged += SetFocus;
            else
                TextBox.Focus();
        }

        public void DialogChanged()
        {
            MirMessageBox box1 = null;
            MirInputBox box2 = null;
            MirAmountBox box3 = null;

            if (MirScene.ActiveScene != null && MirScene.ActiveScene.Controls.Count > 0)
            {
                box1 = (MirMessageBox) MirScene.ActiveScene.Controls.FirstOrDefault(ob => ob is MirMessageBox);
                box2 = (MirInputBox) MirScene.ActiveScene.Controls.FirstOrDefault(O => O is MirInputBox);
                box3 = (MirAmountBox) MirScene.ActiveScene.Controls.FirstOrDefault(ob => ob is MirAmountBox);
            }


            if ((box1 != null && box1 != Parent) || (box2 != null && box2 != Parent)  || (box3 != null && box3 != Parent))
                TextBox.Visible = false;
            else
                TextBox.Visible = Visible && TextBox.Parent != null;
        }


        #region Disposable

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            if (!TextBox.IsDisposed)
                TextBox.Dispose();
        }


        #endregion
    }
}
