using System;
using System.Collections.Generic;

namespace Client.MirControls
{
    public class MirAnimatedControl : MirImageControl
    {
        public static List<MirAnimatedControl> Animations = new List<MirAnimatedControl>();

        #region Animated

        private bool _animated;
        public event EventHandler AnimatedChanged;
        public bool Animated
        {
            get { return _animated; }
            set
            {
                if (_animated == value) return;
                _animated = value;
                _nextOffSet = CMain.Time + _fadeInDelay;
                OnAnimatedChanged(EventArgs.Empty);
            }
        }
        protected virtual void OnAnimatedChanged(EventArgs e)
        {
            Redraw();
            if (AnimatedChanged != null)
                AnimatedChanged.Invoke(this, e);
        }

        #endregion

        #region Animation Count

        private int _animationCount;
        public event EventHandler AnimationCountChanged;
        public virtual int AnimationCount
        {
            get { return _animationCount; }
            set
            {
                if (_animationCount == value) return;
                _animationCount = value;
                OnAnimationCountChanged(EventArgs.Empty);
            }
        }
        protected virtual void OnAnimationCountChanged(EventArgs e)
        {
            if (AnimationCountChanged != null)
                AnimationCountChanged.Invoke(this, e);
        }

        #endregion

        #region Animation Delay

        private long _animationDelay;
        public event EventHandler AnimationDelayChanged;
        public long AnimationDelay
        {
            get { return _animationDelay; }
            set
            {
                if (_animationDelay == value) return;
                _animationDelay = value;
                OnAnimationDelayChanged();
            }
        }
        protected virtual void OnAnimationDelayChanged()
        {
            if (AnimationDelayChanged != null)
                AnimationDelayChanged.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region FadeIn

        private long _nextFadeTime;
        private bool _fadeIn;
        public event EventHandler FadeInChanged;
        public bool FadeIn
        {
            get { return _fadeIn; }
            set
            {
                if (_fadeIn == value) return;
                _nextFadeTime = CMain.Time + _fadeInDelay;
                _fadeIn = value;
                OnFadeInChanged(EventArgs.Empty);
            }
        }
        protected virtual void OnFadeInChanged(EventArgs e)
        {
            if (FadeInChanged != null)
                FadeInChanged.Invoke(this, e);
        }

        #endregion

        #region FadeIn Rate

        private float _fadeInRate;
        public event EventHandler FadeInRateChanged;
        public virtual float FadeInRate
        {
            get { return _fadeInRate; }
            set
            {
                if (_fadeInRate == value) return;
                _fadeInRate = value;
                OnFadeInRateChanged(EventArgs.Empty);
            }
        }
        protected virtual void OnFadeInRateChanged(EventArgs e)
        {
            if (FadeInRateChanged != null)
                FadeInRateChanged.Invoke(this, e);
        }

        #endregion

        #region FadeIn Delay

        private long _fadeInDelay;
        public event EventHandler FadeInDelayChanged;
        public long FadeInDelay
        {
            get { return _fadeInDelay; }
            set
            {
                if (_fadeInDelay == value) return;
                _fadeInDelay = value;
                OnFadeInDelayChanged();
            }
        }
        protected virtual void OnFadeInDelayChanged()
        {
            if (FadeInDelayChanged != null)
                FadeInDelayChanged.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Events

        public event EventHandler AfterAnimation;

        #endregion

        public override int Index
        {
            get { return base.Index + OffSet; }
            set { base.Index = value; }
        }

        #region Loop

        private bool _loop;
        public event EventHandler LoopChanged;
        public bool Loop
        {
            get { return _loop; }
            set
            {
                if (_loop == value) return;
                _loop = value;
                OnLoopChanged(EventArgs.Empty);
            }
        }
        protected virtual void OnLoopChanged(EventArgs e)
        {
            if (LoopChanged != null)
                LoopChanged.Invoke(this, e);
        }

        #endregion

        #region OffSet

        private int _offSet;
        public event EventHandler OffSetChanged;
        public virtual int OffSet
        {
            protected get { return _offSet; }
            set
            {
                if (_offSet == value) return;
                _offSet = value;
                OnOffSetChanged(EventArgs.Empty);
            }
        }
        protected virtual void OnOffSetChanged(EventArgs e)
        {
            OnIndexChanged();
            if (OffSetChanged != null)
                OffSetChanged.Invoke(this, e);
        }
        private long _nextOffSet;

        #endregion

        public MirAnimatedControl()
        {
            _loop = true;
            _nextFadeTime = CMain.Time;
            _nextOffSet = CMain.Time;
            Animations.Add(this);
        }

        public void UpdateOffSet()
        {
            if (_fadeIn && CMain.Time > _nextFadeTime)
            {
                if ((Opacity += _fadeInRate) > 1F)
                {
                    Opacity = 1F;
                    _fadeIn = false;
                }

                _nextFadeTime = CMain.Time + _fadeInDelay;
            }

            if (!Visible || !_animated || _animationDelay == 0 || _animationCount == 0) return;

            if (CMain.Time < _nextOffSet) return;

            Redraw();

            _nextOffSet = CMain.Time + _animationDelay;


            if (++OffSet < _animationCount) return;

            EventHandler temp = AfterAnimation;
            AfterAnimation = null;

            if (!Loop)
                Animated = false;
            else
                OffSet = 0;

            if (temp != null)
                temp.Invoke(this, EventArgs.Empty);
        }

        #region Disposable

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            AnimatedChanged = null;
            _animated = false;

            AnimationCountChanged = null;
            _animationCount = 0;

            AnimationDelayChanged = null;
            _animationDelay = 0;

            AfterAnimation = null;

            LoopChanged = null;
            _loop = false;

            OffSetChanged = null;
            _offSet = 0;

            _nextOffSet = 0;

            Animations.Remove(this);
        }

        #endregion
    }
}