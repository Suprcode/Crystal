using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Client.MirGraphics;
using System.Collections.Generic;

namespace Client.MirControls
{
    public class MirDropDownBox : MirControl
    {
        private MirLabel _label;
        private MirButton _DropDownButton;
        private MirLabel[] _Option = new MirLabel[5];
        private MirButton _ScrollUp, _ScrollDown, _ScrollPosition;

        public int _SelectedIndex = -1;
        public int _WantedIndex = -1;
        public int MinimumOption = 0;
        public int ScrollIndex = 0;
        public int OrigHeight = 0;
        public long LastMouseLeave = 0;
        private List<String> _Items = new List<string>();
        public event EventHandler ValueChanged;

        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
                if (_SelectedIndex >= Items.Count)
                    _SelectedIndex = -1;
            }
        }

        public List<String> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
                ItemsChanged();
            }
        }

        #region Back Color

        protected override void OnBackColourChanged()
        {
            base.OnBackColourChanged();
            if (_label != null && !_label.IsDisposed)
                _label.BackColour = BackColour;
        }

        #endregion

        #region Size
        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            OrigHeight = OrigHeight == 0 ? Size.Height : OrigHeight;
            for (int i = 0; i < _Option.Length; i++)
            {
                _Option[i].Size = new Size(Size.Width - 16, 16); //Size.Width - 13, 13
            }

            if (_label != null && !_label.IsDisposed)
                _label.Size = new Size(Size.Width - 16, 15); //Size.Width - 16, 12
            if (_DropDownButton != null && !_DropDownButton.IsDisposed)
                _DropDownButton.Location = new Point(Size.Width - 18, 2);
            if (_ScrollUp != null && !_ScrollUp.IsDisposed)
                _ScrollUp.Location = new Point(Size.Width - 12, 14);
            if (_ScrollDown != null && !_ScrollDown.IsDisposed)
                _ScrollDown.Location = new Point(Size.Width - 12, 52);
            if (_ScrollPosition != null && !_ScrollPosition.IsDisposed)
                _ScrollPosition.Location = new Point(Size.Width - 11, 22);
 
        }
        #endregion

        #region Enabled

        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();
            if (_DropDownButton != null && !_DropDownButton.IsDisposed)
            {
                _DropDownButton.Enabled = Enabled;
                if (Enabled)
                    _DropDownButton.Visible = true;
                else
                    _DropDownButton.Visible = false;
            }
        }

        #endregion

        public MirDropDownBox()
        {
            BackColour = Color.FromArgb(255,6,6,6);
            ForeColour = Color.White;
            Enabled = false;
            _label = new MirLabel
            {
                Parent = this,
                Location = new Point(0, 0),
                ForeColour = ForeColour,
                BackColour = BackColour,
                Font = new Font(Settings.FontName, 8F),
                Visible = true,
            };
            _label.Click += (o, e) =>
            {
                if (_DropDownButton.Enabled)
                    DropDownClick();
            };
            _label.BeforeDraw += (o, e) =>
            {
                if ((Items.Count > 0) && (Items.Count >= SelectedIndex))
                    if (SelectedIndex == -1)
                    {
                        _label.Text = " ";
                    }
                    else

                        _label.Text = Items[SelectedIndex];
                else
                    _label.Text = "None";
            };
            for (int i = 0; i < _Option.Length; i++)
            {
                _Option[i] = new MirLabel
                {
                    Parent = this,
                    Visible = false,
                    Location = new Point(0, 15 + (i * 13)),
                    ForeColour = ForeColour,
                    BackColour = Color.FromArgb(255,20,20,20),
                    Font = new Font(Settings.FontName, 8F)
                };
                int index = i;
                _Option[index].MouseEnter += (o, e) => _Option[index].BackColour = Color.FromArgb(255,140,70,0);
                _Option[index].MouseLeave += (o, e) => _Option[index].BackColour = Color.FromArgb(255, 20, 20, 20);
                _Option[index].MouseDown += (o, e) => _Option[index].BackColour = Color.FromArgb(255, 20, 20, 20);
                _Option[index].MouseUp += (o, e) => _Option[index].BackColour = Color.FromArgb(255,20,20,20);
                _Option[index].Click += (o, e) => SelectOption(index);

                _Option[index].BeforeDraw += (o, e) =>
                {
                    if (Items.Count > (ScrollIndex + index + MinimumOption))
                        _Option[index].Text = _Items[ScrollIndex + index + MinimumOption];
                };
            }
            _DropDownButton = new MirButton
            {
                Index = 207,
                Library = Libraries.Prguse2,
                Location = new Point(Size.Width - 18, 1),
                Parent = this,
                HoverIndex = 208,
                PressedIndex = 209,
                Visible = false,
            };
            _DropDownButton.Click += (o, e) => DropDownClick();
            _ScrollUp = new MirButton
            {
                HoverIndex = 2022,
                Index = 2021,
                Library = Libraries.Prguse,
                Location = new Point(Size.Width - 12, 14),
                Parent = this,
                PressedIndex = 2023,
                Visible = false
            };
            _ScrollUp.Click += (o, e) => ScrollUp();
            _ScrollDown = new MirButton
            {
                HoverIndex = 2025,
                Index = 2024,
                Library = Libraries.Prguse,
                Location = new Point(Size.Width - 12, 52),
                Parent = this,
                PressedIndex = 2026,
                Visible = false
            };
            _ScrollDown.Click += (o, e) => ScrollDown();
           
            _ScrollPosition = new MirButton
            {
                HoverIndex = 2016,
                Index = 2015,
                Library = Libraries.Prguse,
                Location = new Point(Size.Width - 11, 22),
                Movable = true,
                Parent = this,
                PressedIndex = 2017,
                Visible = false
            };
            _ScrollPosition.OnMoving += ScrollPosition;
            BeforeDraw += MirDropDownBox_BeforeDraw;
        }

        void MirDropDownBox_BeforeDraw(object sender, EventArgs e)
        {
            if (!_Option[0].Visible) return;
            if (IsMouseOver(CMain.MPoint)) return;
            foreach (MirControl control in Controls)
                if (control.IsMouseOver(CMain.MPoint))
                    return;

            CloseDropDown();
        }

        protected override void OnMouseLeave()
        {
            if (!IsMouseOver(CMain.MPoint))
                CloseDropDown();
            base.OnMouseLeave();
        }

        public void SelectOption(int index)
        {
            if (ScrollIndex + index + MinimumOption < Items.Count)
            {
                _WantedIndex = ScrollIndex + index + MinimumOption;
                if (ValueChanged != null)
                    ValueChanged.Invoke(this, EventArgs.Empty);
            }
            else
                Update();
            CloseDropDown();
        }

        public void ItemsChanged()
        {
            CloseDropDown();
            SelectedIndex = -1;
        }
        public void Update()
        {
            // Member is not implemented.
        }
        public void DropDownClick()
        {
            if (_Option[0].Visible)
                CloseDropDown();
            else
                OpenDropDown();
        }
        public void OpenDropDown()
        {
            Size = new Size(Size.Width, OrigHeight + (Math.Max(5, Items.Count) * 15));
            ScrollIndex = 0;
            if (Items.Count > 5)
                ScrollIndex = SelectedIndex > 3 ? SelectedIndex - 2 : 0;
            for (int i = 0; i < _Option.Length; i++)
                if (Items.Count > i)
                    _Option[i].Visible = true;
                else
                    _Option[i].Visible = false;
            if (Items.Count > 5)
            {
                _ScrollDown.Visible = true;
                _ScrollPosition.Visible = true;
                _ScrollUp.Visible = true;
            }
            else
            {
                _ScrollDown.Visible = false;
                _ScrollPosition.Visible = false;
                _ScrollUp.Visible = false;
            }
            _ScrollDown.Location = new Point(Size.Width - 12, 9 + Math.Min(5, Items.Count) * 13);
        }
        public void CloseDropDown()
        {
            Size = new Size(Size.Width, OrigHeight);
            for (int i = 0; i < _Option.Length; i++)
                _Option[i].Visible = false;
            _ScrollDown.Visible = false;
            _ScrollPosition.Visible = false;
            _ScrollUp.Visible = false;
        }
        public void ScrollUp()
        {
            if (ScrollIndex > 0) ScrollIndex--;
            Update();
        }
        public void ScrollDown()
        {
            if (ScrollIndex < (Items.Count - 5)) ScrollIndex++;
            Update();
        }
        void ScrollPosition(object sender, MouseEventArgs e)
        {
            int x = Size.Width - 11;
            int y = _ScrollPosition.Location.Y;
            if (y >= _ScrollDown.Location.Y - 14) y = _ScrollDown.Location.Y - 14;
            if (y < 20) y = 20;

            int h = _ScrollDown.Location.Y - _ScrollUp.Location.Y + 6;
            h = (int)((y - 15) / (h / (float)(Items.Count - 1)));
            if (h > Items.Count - 5) h = Math.Max(0, Items.Count - 5);
            if (h != ScrollIndex)
            {
                ScrollIndex = h;
                Update();
            }

            _ScrollPosition.Location = new Point(x, y);
        }

        #region Disposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            if (_label != null && !_label.IsDisposed)
                _label.Dispose();
            _label = null;

            if (_DropDownButton != null && !_DropDownButton.IsDisposed)
                _DropDownButton.Dispose();
            _DropDownButton = null;

            if (_ScrollDown != null && !_ScrollDown.IsDisposed)
                _ScrollDown.Dispose();
            _ScrollDown = null;

            if (_ScrollPosition != null && !_ScrollPosition.IsDisposed)
                _ScrollPosition.Dispose();
            _ScrollPosition = null;

            if (_ScrollUp != null && !_ScrollUp.IsDisposed)
                _ScrollUp.Dispose();
            _ScrollUp = null;
            for (int i = 0; i < _Option.Length; i++)
            {
                if (_Option[i] != null && !_Option[i].IsDisposed)
                    _Option[i].Dispose();
                _Option[i] = null;
            }
        }
        #endregion
    }
}