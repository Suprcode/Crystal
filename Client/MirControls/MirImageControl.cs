﻿using System;
using System.Drawing;
using Client.MirGraphics;

namespace Client.MirControls
{
    public class MirImageControl : MirControl
    {
        public override Point DisplayLocation { get { return UseOffSet ? base.DisplayLocation.Add(Library.GetOffSet(Index)) : base.DisplayLocation; } }
        public Point DisplayLocationWithoutOffSet { get { return base.DisplayLocation; } }

        #region DrawImage
        private bool _drawImage;
        public bool DrawImage
        {
            get { return _drawImage; }
            set
            {
                if (_drawImage == value)
                    return;
                _drawImage = value;
                OnDrawImageChanged();
            }
        }
        public event EventHandler DrawImageChanged;
        private void OnDrawImageChanged()
        {
            Redraw();
            if (DrawImageChanged != null)
                DrawImageChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Index
        private int _index;
        public virtual int Index
        {
            get { return _index; }
            set
            {
                if (_index == value)
                    return;
                _index = value;
                OnIndexChanged();
            }
        }
        public event EventHandler IndexChanged;
        protected void OnIndexChanged()
        {
            OnSizeChanged();
            if (IndexChanged != null)
                IndexChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Library
        private MLibrary _library;
        public MLibrary Library
        {
            get { return _library; }
            set
            {
                if (_library == value)
                    return;
                _library = value;
                OnLibraryChanged();
            }
        }
        public event EventHandler LibraryChanged;
        private void OnLibraryChanged()
        {
            OnSizeChanged();
            if (LibraryChanged != null)
                LibraryChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region PixelDetect
        private bool _pixelDetect;
        protected bool PixelDetect
        {
            set
            {
                if (_pixelDetect == value)
                    return;
                _pixelDetect = value;
                OnPixelDetectChanged();
            }
        }
        public event EventHandler PixelDetectChanged;
        private void OnPixelDetectChanged()
        {
            Redraw();
            if (PixelDetectChanged != null)
                PixelDetectChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region UseOffset
        private bool _useOffSet;
        public bool UseOffSet
        {
            protected get { return _useOffSet; }
            set
            {
                if (_useOffSet == value)
                    return;
                _useOffSet = value;
                OnUseOffSetChanged();
            }
        }
        public event EventHandler UseOffSetChanged;
        private void OnUseOffSetChanged()
        {
            OnLocationChanged();
            if (UseOffSetChanged != null)
                UseOffSetChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Size
        public override Size Size
        {
            set { base.Size = value; }
            get
            {
                if (Library != null && Index >= 0)
                    return Library.GetTrueSize(Index);
                return base.Size;
            }
        }

        public override Size TrueSize
        {
            get
            {
                if (Library != null && Index >= 0)
                    return Library.GetTrueSize(Index);
                return base.TrueSize;
            }
        }

        #endregion

        public MirImageControl()
        {
            _drawImage = true;
            _index = -1;
            ForeColour = Color.White;
        }

        protected internal override void DrawControl()
        {

            base.DrawControl();

            if (DrawImage && Library != null)
            {

                if (Blending) Library.DrawBlend(Index, DisplayLocation, ForeColour, false, 1F);
                else Library.Draw(Index, DisplayLocation, ForeColour, false, Opacity);
            }

            if (DrawImage && Library != null)
            {
                if (GrayScale) DXManager.SetGrayscale(1F, Color.White);
                else if (Blending) Library.DrawBlend(Index, DisplayLocation, ForeColour, false, BlendingRate);
                else Library.Draw(Index, DisplayLocation, ForeColour, false, Opacity);
                if (GrayScale) DXManager.SetNormal(1F, Color.White);
            }
        }

        public override bool IsMouseOver(Point p)
        {
            return base.IsMouseOver(p) && (!_pixelDetect || Library.VisiblePixel(Index, p.Subtract(DisplayLocation),true) || Moving);
        }

        #region Disposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            if (!disposing) return;

            DrawImageChanged = null;
            _drawImage = false;

            IndexChanged = null;
            _index = 0;

            LibraryChanged = null;
            Library = null;

            PixelDetectChanged = null;
            _pixelDetect = false;

            UseOffSetChanged = null;
            _useOffSet = false;
        }
        #endregion
    }
}