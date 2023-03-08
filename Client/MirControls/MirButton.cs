using Client.MirSounds;

namespace Client.MirControls
{
    public class MirButton : MirImageControl
    {
        #region Font Colour
        public virtual Color FontColour
        {
            get
            {
                if (_label != null && !_label.IsDisposed)
                    return _label.ForeColour;
                return Color.Empty;
            }
            set
            {
                if (_label != null && !_label.IsDisposed)
                    _label.ForeColour = value;
            }
        }
        #endregion

        #region Hover Index
        private int _hoverIndex;
        public int HoverIndex
        {
            get { return _hoverIndex; }
            set
            {
                if (_hoverIndex == value)
                    return;
                _hoverIndex = value;
                OnHoverIndexChanged();
            }
        }
        public event EventHandler HoverIndexChanged;
        private void OnHoverIndexChanged()
        {
            if (HoverIndexChanged != null)
                HoverIndexChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Index
        public override int Index
        {
            get
            {
                if (!Enabled)
                    return _disabledIndex >= 0 ? _disabledIndex : base.Index;

                if (_pressedIndex >= 0 && ActiveControl == this && MouseControl == this)
                    return _pressedIndex;

                if (_hoverIndex >= 0 && MouseControl == this)
                    return _hoverIndex;

                return base.Index;
            }
            set { base.Index = value; }
        }
        #endregion

        #region Label
        protected MirLabel _label;
        #endregion

        #region CenterText
        protected bool _center;
        public virtual bool CenterText
        {
            get
            {
                return _center;
            }
            set
            {
                _center = value;
                if (_center)
                {
                    _label.Size = Size;
                    _label.DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                }
                else
                    _label.AutoSize = true;
            }
        }
        #endregion

        #region Pressed Index
        private int _pressedIndex;
        public int PressedIndex
        {
            set
            {
                if (_pressedIndex == value)
                    return;
                _pressedIndex = value;
                OnPressedIndexChanged();
            }
            get { return _pressedIndex; }
        }

        public event EventHandler PressedIndexChanged;
        private void OnPressedIndexChanged()
        {
            if (PressedIndexChanged != null)
                PressedIndexChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Disabled Index
        private int _disabledIndex;
        public int DisabledIndex
        {
            set
            {
                if (_disabledIndex == value)
                    return;
                _disabledIndex = value;
                OnDisabledIndexChanged();
            }
            get { return _disabledIndex; }
        }

        public event EventHandler DisabledIndexChanged;
        private void OnDisabledIndexChanged()
        {
            if (DisabledIndexChanged != null)
                DisabledIndexChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Size
        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();

            if (_label != null && !_label.IsDisposed)
                _label.Size = Size;
        }
        #endregion

        #region Text
        public virtual string Text
        {
            set
            {
                if (_label == null || _label.IsDisposed)
                    return;
                _label.Text = value;
                _label.Visible = !string.IsNullOrEmpty(value);
            }
        }
        #endregion

        public bool OnlyDrawWhenActive;

        public MirButton()
        {
            HoverIndex = -1;
            PressedIndex = -1;
            DisabledIndex = -1;
            Sound = SoundList.ButtonB;

            _label = new MirLabel
                {
                    NotControl = true,
                    Parent = this,
                    //Font = new Font("Constantia", 8, FontStyle.Italic),
                    //OutLine = true,
                    //OutLineColour = Color.FromArgb(255, 70, 50, 30),
                };
        }

        protected internal override void DrawControl()
        {
            if (OnlyDrawWhenActive && ActiveControl != this && MouseControl != this)
                return;

            base.DrawControl();
        }

        protected override void Highlight()
        {
            Redraw();
            base.Highlight();
        }
        protected override void Activate()
        {
            Redraw();
            base.Activate();
        }
        protected override void Dehighlight()
        {
            Redraw();
            base.Dehighlight();
        }
        protected override void Deactivate()
        {
            Redraw();
            base.Deactivate();
        }


        #region Disposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            HoverIndexChanged = null;
            _hoverIndex = 0;

            if (_label != null && !_label.IsDisposed)
                _label.Dispose();
            _label = null;

            PressedIndexChanged = null;
            _pressedIndex = 0;

            DisabledIndexChanged = null;
            _disabledIndex = 0;
        }
        #endregion
    }
}
