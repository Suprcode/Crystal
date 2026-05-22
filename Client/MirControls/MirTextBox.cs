#if !WINDOWS
using System;
using System.Drawing;
using System.Linq;
using Client.MirGraphics;
using Client.MirScenes;

namespace Client.MirControls
{
    public sealed class MirTextBox : MirControl
    {
        public bool CanLoseFocus { get; set; } = true;
        public int MaxLength { get; set; } = 100;
        public bool Password { get; set; }

        private string _text = "";
        public string Text
        {
            get => _text;
            set
            {
                _text = value ?? "";
                UpdateLabel();
                TextChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public string[] MultiText
        {
            get => new string[] { _text };
            set
            {
                if (value != null && value.Length > 0)
                    Text = value[0];
            }
        }

        private readonly MirLabel _textLabel;
        private readonly MirLabel _caretLabel;
        private bool _isFocused;
        public bool Focused => _isFocused;

        public class TextBoxStub
        {
            private readonly MirTextBox _parent;
            public TextBoxStub(MirTextBox parent) => _parent = parent;

            public string Text
            {
                get => _parent.Text;
                set => _parent.Text = value;
            }

            public event KeyPressEventHandler KeyPress
            {
                add => _parent.KeyPressEvent += value;
                remove => _parent.KeyPressEvent -= value;
            }

            public event EventHandler TextChanged
            {
                add => _parent.TextChangedEvent += value;
                remove => _parent.TextChangedEvent -= value;
            }

            public event KeyEventHandler KeyDown
            {
                add => _parent.KeyDownEvent += value;
                remove => _parent.KeyDownEvent -= value;
            }

            public event KeyEventHandler KeyUp
            {
                add => _parent.KeyUpEvent += value;
                remove => _parent.KeyUpEvent -= value;
            }

            public int SelectionStart { get; set; }
            public int SelectionLength { get; set; }
            public int GetFirstCharIndexFromLine(int line) => 0;
            public void ScrollToCaret() { }

            public int MaxLength
            {
                get => _parent.MaxLength;
                set => _parent.MaxLength = value;
            }

            public bool Focused => _parent.Focused;

            public event EventHandler GotFocus;
            public void OnGotFocus() => GotFocus?.Invoke(this, EventArgs.Empty);

            public event EventHandler LostFocus;
            public void OnLostFocus() => LostFocus?.Invoke(this, EventArgs.Empty);
        }

        private event KeyPressEventHandler KeyPressEvent;
        private event EventHandler TextChangedEvent;
        private event KeyEventHandler KeyDownEvent;
        private event KeyEventHandler KeyUpEvent;
        public TextBoxStub TextBox { get; }

        public MirTextBox()
        {
            TextBox = new TextBoxStub(this);
            BackColour = Color.Black;
            DrawControlTexture = true;
            TextureValid = false;

            _textLabel = new MirLabel
            {
                AutoSize = true,
                BackColour = Color.Transparent,
                ForeColour = Color.White,
                Parent = this,
                Location = new Point(2, 2),
                NotControl = true
            };

            _caretLabel = new MirLabel
            {
                AutoSize = true,
                BackColour = Color.Transparent,
                ForeColour = Color.White,
                Text = "|",
                Parent = this,
                Location = new Point(2, 2),
                NotControl = true,
                Visible = false,
                OutLine = false
            };

            Font = new Font(Settings.FontName, 10F);

            MouseDown += OnMouseDownEvent;
        }

        private void OnMouseDownEvent(object sender, MouseEventArgs e)
        {
            SetFocus();
        }

        public void SetFocus()
        {
            if (MirScene.ActiveScene != null)
            {
                UnfocusAllTextBoxes(MirScene.ActiveScene);
            }

            Activate();

            _isFocused = true;
            _caretLabel.Visible = true;
            
            // Set active control on program form boundary if possible
            TextureValid = false;
            Redraw();
            TextBox.OnGotFocus();
#if FNA
            Microsoft.Xna.Framework.Input.TextInputEXT.StartTextInput();
#endif
        }

        private void UnfocusAllTextBoxes(MirControl parent)
        {
            if (parent == null) return;
            if (parent is MirTextBox textBox && textBox != this)
            {
                textBox.LoseFocus();
            }
            if (parent.Controls != null)
            {
                for (int i = 0; i < parent.Controls.Count; i++)
                {
                    UnfocusAllTextBoxes(parent.Controls[i]);
                }
            }
        }

        public void LoseFocus()
        {
            Deactivate();
            _isFocused = false;
            _caretLabel.Visible = false;
            TextureValid = false;
            Redraw();
#if FNA
            Microsoft.Xna.Framework.Input.TextInputEXT.StopTextInput();
#endif
            TextBox.OnLostFocus();
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            KeyDownEvent?.Invoke(this, e);
            if (e.Handled) return;

            if (e.KeyCode == Client.Platform.MirKeys.Escape)
            {
                KeyPressEventArgs args = new KeyPressEventArgs((char)Keys.Escape);
                OnKeyPress(args);
                if (args.Handled)
                {
                    e.Handled = true;
                    return;
                }

                if (CanLoseFocus) LoseFocus();
                e.Handled = true;
            }

            if (e.KeyCode == Client.Platform.MirKeys.Tab)
            {
                TryTabFocus();
                e.Handled = true;
            }
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            KeyUpEvent?.Invoke(this, e);
        }

        public override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!_isFocused) return;

            base.OnKeyPress(e);
            KeyPressEvent?.Invoke(this, e);
            if (e.Handled) return;

            if (e.KeyChar == (char)Keys.Escape)
            {
                LoseFocus();
                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char)Keys.Back)
            {
                if (Text.Length > 0)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                }
                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                if (CanLoseFocus) LoseFocus();
                e.Handled = true;
                return;
            }

            // Append standard printable characters
            if (!char.IsControl(e.KeyChar) && Text.Length < MaxLength)
            {
                Text += e.KeyChar;
                e.Handled = true;
            }
        }

        private void UpdateLabel()
        {
            if (_textLabel == null || _caretLabel == null) return;

            if (Password)
            {
                _textLabel.Text = new string('*', _text.Length);
            }
            else
            {
                _textLabel.Text = _text;
            }

            int labelHeight = _textLabel.Size.Height > 0 ? _textLabel.Size.Height : _caretLabel.Size.Height;
            int yOffset = (Size.Height - labelHeight) / 2;
            if (yOffset < 0) yOffset = 0;

            _textLabel.Location = new Point(2, yOffset);
            int caretX = _textLabel.Location.X;
            if (_text.Length > 0)
            {
                caretX += _textLabel.Size.Width - 5;
            }
            _caretLabel.Location = new Point(caretX, yOffset);
            
            TextureValid = false;
            Redraw();
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            UpdateLabel();
        }

        private Font _font;
        public Font Font
        {
            get => _font ??= new Font(Settings.FontName, 10F);
            set
            {
                _font = value;
                if (value != null)
                {
                    if (_textLabel != null) _textLabel.Font = new Font(value.Name, value.Size, value.Style);
                    if (_caretLabel != null) _caretLabel.Font = new Font(value.Name, value.Size, value.Style);
                }
                UpdateLabel();
            }
        }

        protected override void OnForeColourChanged()
        {
            base.OnForeColourChanged();
            if (_textLabel != null) _textLabel.ForeColour = ForeColour;
            if (_caretLabel != null) _caretLabel.ForeColour = ForeColour;
        }

        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();
            if (!Visible)
            {
                LoseFocus();
            }
        }

        public override void Draw()
        {
            // Caret Blinking
            if (_isFocused)
            {
                _caretLabel.Visible = (CMain.Time / 500) % 2 == 0;
            }
            base.Draw();
        }

        public override void MultiLine()
        {
        }

        public void DialogChanged()
        {
            MirMessageBox box1 = null;
            MirInputBox box2 = null;
            MirAmountBox box3 = null;

            if (MirScene.ActiveScene != null && MirScene.ActiveScene.Controls.Count > 0)
            {
                box1 = (MirMessageBox)MirScene.ActiveScene.Controls.FirstOrDefault(ob => ob is MirMessageBox);
                box2 = (MirInputBox)MirScene.ActiveScene.Controls.FirstOrDefault(O => O is MirInputBox);
                box3 = (MirAmountBox)MirScene.ActiveScene.Controls.FirstOrDefault(ob => ob is MirAmountBox);
            }

            if ((box1 != null && box1 != Parent) || (box2 != null && box2 != Parent) || (box3 != null && box3 != Parent))
                Visible = false;
        }

        private void TryTabFocus()
        {
            if (MirScene.ActiveScene == null) return;
            var textBoxes = new System.Collections.Generic.List<MirTextBox>();
            FindTextBoxes(MirScene.ActiveScene, textBoxes);
            if (textBoxes.Count <= 1) return;

            int index = textBoxes.IndexOf(this);
            if (index == -1) return;

            int nextIndex;
            if (CMain.Shift)
            {
                nextIndex = index - 1;
                if (nextIndex < 0) nextIndex = textBoxes.Count - 1;
            }
            else
            {
                nextIndex = (index + 1) % textBoxes.Count;
            }
            textBoxes[nextIndex].SetFocus();
        }

        private static void FindTextBoxes(MirControl parent, System.Collections.Generic.List<MirTextBox> list)
        {
            if (parent == null || !parent.Visible) return;
            if (parent is MirTextBox textBox && textBox.Enabled)
            {
                list.Add(textBox);
            }
            if (parent.Controls != null)
            {
                for (int i = 0; i < parent.Controls.Count; i++)
                {
                    FindTextBoxes(parent.Controls[i], list);
                }
            }
        }
    }
}
#else
using Client.MirGraphics;
using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

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
                Font = new System.Drawing.Font(Settings.FontName, 10F * 120f / CMain.Graphics.DpiX),
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
#endif
