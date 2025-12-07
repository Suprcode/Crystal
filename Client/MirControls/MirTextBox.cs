using Client.MirGraphics;
using SlimDX;
using SlimDX.Direct3D9;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
                UpdateTextBoxHostLocation();

            TextureValid = false;
            Redraw();
        }

        private void UpdateTextBoxHostLocation(bool force = false)
        {
            if (TextBox == null || TextBox.IsDisposed)
                return;

            bool hide = !Settings.FullScreen;

            if (!force)
            {
                if (!hide && TextBox.Location == DisplayLocation && !_textBoxOffscreen)
                    return;
                if (hide && TextBox.Location == HiddenTextBoxLocation && _textBoxOffscreen)
                    return;
            }

            if (hide)
            {
                TextBox.Location = HiddenTextBoxLocation;
                _textBoxOffscreen = true;
            }
            else
            {
                TextBox.Location = DisplayLocation;
                _textBoxOffscreen = false;
            }

            UpdateMouseMoveHook(!_textBoxOffscreen);
        }

        private void UpdateMouseMoveHook(bool enabled)
        {
            if (TextBox == null || TextBox.IsDisposed)
                return;

            if (enabled)
            {
                if (_mouseMoveHooked)
                    return;

                TextBox.MouseMove += CMain.CMain_MouseMove;
                _mouseMoveHooked = true;
            }
            else
            {
                if (!_mouseMoveHooked)
                    return;

                TextBox.MouseMove -= CMain.CMain_MouseMove;
                _mouseMoveHooked = false;
            }
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
            UpdateTextBoxHostLocation(true);
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
            if (TextBox != null && !TextBox.IsDisposed)
            {
                TextBox.Size = Size;
                UpdateTextBoxHostLocation();
            }

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
        private bool _textBoxOffscreen;
        private bool _mouseMoveHooked;

        private static readonly Point HiddenTextBoxLocation = new Point(-5000, -5000);

        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int WM_MOUSEWHEEL = 0x020A;

        private const int MK_LBUTTON = 0x0001;
        private const int MK_RBUTTON = 0x0002;
        private const int MK_SHIFT = 0x0004;
        private const int MK_CONTROL = 0x0008;
        private const int MK_MBUTTON = 0x0010;

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
            {
                TextBox.Visible = Visible;
                UpdateTextBoxHostLocation();
            }
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

        #region Mouse Forwarding

        public override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_textBoxOffscreen && e.Button == MouseButtons.Left)
            {
                SetFocus();
                ForwardMouseMessage(WM_LBUTTONDOWN, e);
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_textBoxOffscreen)
                ForwardMouseMessage(WM_MOUSEMOVE, e, true);
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_textBoxOffscreen && e.Button == MouseButtons.Left)
                ForwardMouseMessage(WM_LBUTTONUP, e);
        }

        public override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (_textBoxOffscreen && e.Button == MouseButtons.Left)
                ForwardMouseMessage(WM_LBUTTONDBLCLK, e);
        }

        public override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (_textBoxOffscreen)
                ForwardMouseWheel(e);
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

            UpdateTextBoxHostLocation(true);
        }

        private void TextBox_NeedRedraw(object sender, EventArgs e)
        {
            TextureValid = false;
            Redraw();
        }

        protected unsafe override void CreateTexture()
        {
            UpdateTextBoxHostLocation();

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
            UpdateTextBoxHostLocation(true);
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

        private void ForwardMouseMessage(int message, MouseEventArgs e, bool includeCurrentButtons = false)
        {
            if (!_textBoxOffscreen || TextBox == null || TextBox.IsDisposed)
                return;

            if (!TextBox.IsHandleCreated)
                TextBox.CreateControl();

            Point localPoint = GetLocalPoint();
            int wParam = BuildMouseKeyState(e, includeCurrentButtons);
            int lParam = PackPoint(localPoint);

            SendMessage(TextBox.Handle, message, (IntPtr)wParam, (IntPtr)lParam);
        }

        private void ForwardMouseWheel(MouseEventArgs e)
        {
            if (!_textBoxOffscreen || TextBox == null || TextBox.IsDisposed)
                return;

            if (!TextBox.IsHandleCreated)
                TextBox.CreateControl();

            Point localPoint = GetLocalPoint();
            int keyState = BuildMouseKeyState(e, true);
            int wParam = ((short)e.Delta << 16) | (keyState & 0xFFFF);
            int lParam = PackPoint(localPoint);

            SendMessage(TextBox.Handle, WM_MOUSEWHEEL, (IntPtr)wParam, (IntPtr)lParam);
        }

        private int PackPoint(Point point)
        {
            int x = point.X;
            int y = point.Y;

            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x > 0xFFFF) x = 0xFFFF;
            if (y > 0xFFFF) y = 0xFFFF;

            return (y << 16) | (x & 0xFFFF);
        }

        private Point GetLocalPoint()
        {
            int x = CMain.MPoint.X - DisplayLocation.X;
            int y = CMain.MPoint.Y - DisplayLocation.Y;

            if (Size.Width > 0)
            {
                if (x < 0) x = 0;
                if (x >= Size.Width) x = Size.Width - 1;
            }
            else
                x = 0;

            if (Size.Height > 0)
            {
                if (y < 0) y = 0;
                if (y >= Size.Height) y = Size.Height - 1;
            }
            else
                y = 0;

            return new Point(x, y);
        }

        private int BuildMouseKeyState(MouseEventArgs e, bool includeCurrentButtons)
        {
            int state = 0;
            MouseButtons buttons = e.Button;

            if (includeCurrentButtons && buttons == MouseButtons.None)
                buttons = Control.MouseButtons;

            if ((buttons & MouseButtons.Left) == MouseButtons.Left)
                state |= MK_LBUTTON;
            if ((buttons & MouseButtons.Right) == MouseButtons.Right)
                state |= MK_RBUTTON;
            if ((buttons & MouseButtons.Middle) == MouseButtons.Middle)
                state |= MK_MBUTTON;

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                state |= MK_SHIFT;
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                state |= MK_CONTROL;

            return state;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);


        #region Disposable

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            UpdateMouseMoveHook(false);

            if (!TextBox.IsDisposed)
                TextBox.Dispose();
        }


        #endregion
    }
}
