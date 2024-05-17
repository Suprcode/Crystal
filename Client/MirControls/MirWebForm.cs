using System.Drawing.Imaging;
using Client.MirGraphics;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using SlimDX;
using SlimDX.Direct3D9;

namespace Client.MirControls
{
    public sealed class MirWebForm : MirControl
    {
        public readonly WebView2 WebForm;

        #region Location

        protected override void OnLocationChanged()
        {
            base.OnLocationChanged();
            if (WebForm != null && !WebForm.IsDisposed)
                WebForm.Location = DisplayLocation;

            TextureValid = false;
            Redraw();
        }

        #endregion

        #region Size

        protected override void OnSizeChanged()
        {
            WebForm.Size = Size;

            DisposeTexture();

            _size = Size;

            if (WebForm != null && !WebForm.IsDisposed)
                base.OnSizeChanged();
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

            if (WebForm != null && !WebForm.IsDisposed)
                WebForm.Visible = Visible;
        }
        private void WebForm_VisibleChanged(object sender, EventArgs e)
        {
            DialogChanged();

            if (WebForm.Visible && WebForm.CanFocus)
                if (Program.Form.ActiveControl == null || Program.Form.ActiveControl == Program.Form)
                    Program.Form.ActiveControl = WebForm;


            if (!WebForm.Visible)
                if (Program.Form.ActiveControl == WebForm)
                    Program.Form.Focus();
        }
        private void SetFocus(object sender, EventArgs e)
        {
            if (WebForm.Visible)
                WebForm.VisibleChanged -= SetFocus;
            if (WebForm.Parent != null)
                WebForm.ParentChanged -= SetFocus;

            if (WebForm.CanFocus) WebForm.Focus();
            else if (WebForm.Visible && WebForm.Parent != null)
                Program.Form.ActiveControl = WebForm;
        }

        #endregion

        public MirWebForm()
        {
            TextureValid = false;

            WebForm = new WebView2
            {
                Location = DisplayLocation,
                Size = Size,
                Visible = Visible,
                Tag = this,
                Source = new Uri("about:blank")
            };
            WebForm.VisibleChanged += WebForm_VisibleChanged;
            WebForm.ParentChanged += WebForm_VisibleChanged;
            WebForm.MouseDown += WebForm_NeedRedraw;
            WebForm.MouseUp += WebForm_NeedRedraw;
            WebForm.LostFocus += WebForm_NeedRedraw;
            WebForm.GotFocus += WebForm_NeedRedraw;
            WebForm.MouseWheel += WebForm_NeedRedraw;
            Shown += MirWebForm_Shown;
            WebForm.MouseMove += CMain.CMain_MouseMove;
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

            DataRectangle stream = ControlTexture.LockRectangle(0, LockFlags.Discard);
            using (Bitmap bm = new Bitmap(Size.Width, Size.Height, Size.Width * 4, PixelFormat.Format32bppArgb, stream.Data.DataPointer))
            {
                WebForm.DrawToBitmap(bm, new Rectangle(0, 0, Size.Width, Size.Height));
                using (Graphics graphics = Graphics.FromImage(bm))
                {
                    graphics.DrawImage(bm, Point.Empty);
                }

            }
            ControlTexture.UnlockRectangle(0);
            DXManager.Sprite.Flush();
            TextureValid = true;
        }
        private void WebForm_NeedRedraw(object sender, EventArgs e)
        {
            TextureValid = false;
            Redraw();
        }
        public void DialogChanged()
        {
            MirWebForm form01 = null;

            if (MirScene.ActiveScene != null && MirScene.ActiveScene.Controls.Count > 0)
            {
                form01 = (MirWebForm)MirScene.ActiveScene.Controls.FirstOrDefault(ob => ob is MirWebForm);
            }

            if (form01 != null && form01 != Parent)
                WebForm.Visible = false;
            else
                WebForm.Visible = Visible && WebForm.Parent != null;
        }

        public void SetFocus()
        {
            if (!WebForm.Visible)
                WebForm.VisibleChanged += SetFocus;
            else if (WebForm.Parent == null)
                WebForm.ParentChanged += SetFocus;
            else
                WebForm.Focus();
        }

        public override void Show()
        {
            if (Parent != null) return;
            Parent = MirScene.ActiveScene;

            for (int i = 0; i < Program.Form.Controls.Count; i++)
            {
                WebView2 T = Program.Form.Controls[i] as WebView2;
                if (T != null && T.Tag != null && T.Tag != null)
                    ((MirWebForm)T.Tag).DialogChanged();
            }
        }

        public override void Hide()
        {
            ClearBrowsingData();
            WebForm.Source = new Uri("about:blank");
            WebForm.Dispose();
        }


        void MirWebForm_Shown(object sender, EventArgs e)
        {
            WebForm.Parent = Program.Form;
            CMain.Ctrl = false;
            CMain.Shift = false;
            CMain.Alt = false;
            CMain.Tilde = false;

            TextureValid = false;
            SetFocus();
        }
        private void ClearBrowsingData()
        {
            var profile = WebForm.CoreWebView2.Profile;

            if (profile != null)
            {
                profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.Cookies);
                profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.DiskCache);
                profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.DownloadHistory);
                profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.LocalStorage);
                profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.PasswordAutosave);
                profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.WebSql);
            }
        }

        #region Disposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing) return;
            if (!WebForm.IsDisposed)
                WebForm.Dispose();
        }
        #endregion
    }
}
