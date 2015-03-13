using System;
using System.Drawing;
using System.Windows.Forms;
using Client.MirGraphics;

namespace Client.MirControls
{
    public enum MirMessageBoxButtons { OK, OKCancel, YesNo, YesNoCancel, Cancel }

    public sealed class MirMessageBox : MirImageControl
    {
        public MirLabel Label;
        public MirButton OKButton, CancelButton, NoButton, YesButton;
        public MirMessageBoxButtons Buttons;


        public MirMessageBox(string message, MirMessageBoxButtons b = MirMessageBoxButtons.OK)
        {
            DrawImage = true;
            ForeColour = Color.White;
            Buttons = b;
            Modal = true;
            Movable = false;

            Index = 360;
            Library = Libraries.Prguse;

            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);


            Label = new MirLabel
            {
                AutoSize = false,
               // DrawFormat = StringFormatFlags.FitBlackBox,
                Location = new Point(35, 35),
                Size = new Size(390, 110),
                Parent = this,
                Text = message
            };

            
            switch (Buttons)
            {
                case MirMessageBoxButtons.OK:
                    OKButton = new MirButton
                    {
                        HoverIndex = 201,
                        Index = 200,
                        Library = Libraries.Title,
                        Location = new Point(360, 157),
                        Parent = this,
                        PressedIndex = 202,
                    };
                    OKButton.Click += (o, e) => Dispose();
                    break;
                case MirMessageBoxButtons.OKCancel:
                    OKButton = new MirButton
                    {
                        HoverIndex = 201,
                        Index = 200,
                        Library = Libraries.Title,
                        Location = new Point(260, 157),
                        Parent = this,
                        PressedIndex = 022,
                    };
                    OKButton.Click += (o, e) => Dispose();
                    CancelButton = new MirButton
                    {
                        HoverIndex = 204,
                        Index = 203,
                        Library = Libraries.Title,
                        Location = new Point(360, 157),
                        Parent = this,
                        PressedIndex = 205,
                    };
                    CancelButton.Click += (o, e) => Dispose();
                    break;
                case MirMessageBoxButtons.YesNo:
                    YesButton = new MirButton
                    {
                        HoverIndex = 207,
                        Index = 206,
                        Library = Libraries.Title,
                        Location = new Point(260, 157),
                        Parent = this,
                        PressedIndex = 208,
                    };
                    YesButton.Click += (o, e) => Dispose();
                    NoButton = new MirButton
                    {
                        HoverIndex = 211,
                        Index = 210,
                        Library = Libraries.Title,
                        Location = new Point(360, 157),
                        Parent = this,
                        PressedIndex = 212,
                    };
                    NoButton.Click += (o, e) => Dispose();
                    break;
                case MirMessageBoxButtons.YesNoCancel:
                    YesButton = new MirButton
                    {
                        HoverIndex = 207,
                        Index = 206,
                        Library = Libraries.Title,
                        Location = new Point(160, 157),
                        Parent = this,
                        PressedIndex = 208,
                    };
                    YesButton.Click += (o, e) => Dispose();
                    NoButton = new MirButton
                    {
                        HoverIndex = 211,
                        Index = 210,
                        Library = Libraries.Title,
                        Location = new Point(260, 157),
                        Parent = this,
                        PressedIndex = 212,
                    };
                    NoButton.Click += (o, e) => Dispose();
                    CancelButton = new MirButton
                    {
                        HoverIndex = 204,
                        Index = 203,
                        Library = Libraries.Title,
                        Location = new Point(360, 157),
                        Parent = this,
                        PressedIndex = 205,
                    };
                    CancelButton.Click += (o, e) => Dispose();
                    break;
                case MirMessageBoxButtons.Cancel:
                    CancelButton = new MirButton
                    {
                        HoverIndex = 204,
                        Index = 203,
                        Library = Libraries.Title,
                        Location = new Point(360, 157),
                        Parent = this,
                        PressedIndex = 205,
                    };
                    CancelButton.Click += (o, e) => Dispose();
                    break;
            }
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
            {
                switch (Buttons)
                {
                    case MirMessageBoxButtons.OK:
                        if (OKButton != null && !OKButton.IsDisposed) OKButton.InvokeMouseClick(null);
                        break;
                    case MirMessageBoxButtons.OKCancel:
                    case MirMessageBoxButtons.YesNoCancel:
                        if (CancelButton != null && !CancelButton.IsDisposed) CancelButton.InvokeMouseClick(null);
                        break;
                    case MirMessageBoxButtons.YesNo:
                        if (NoButton != null && !NoButton.IsDisposed) NoButton.InvokeMouseClick(null);
                        break;
                }
            }

            else if (e.KeyChar == (char)Keys.Enter)
            {
                switch (Buttons)
                {
                    case MirMessageBoxButtons.OK:
                    case MirMessageBoxButtons.OKCancel:
                        if (OKButton != null && !OKButton.IsDisposed) OKButton.InvokeMouseClick(null);
                        break;
                    case MirMessageBoxButtons.YesNoCancel:
                    case MirMessageBoxButtons.YesNo:
                        if (YesButton != null && !YesButton.IsDisposed) YesButton.InvokeMouseClick(null);
                        break;

                }
            }
            e.Handled = true;
        }


        public static void Show(string message, bool close = false)
        {
            MirMessageBox box = new MirMessageBox(message);

            if (close) box.OKButton.Click += (o, e) => Program.Form.Close();

            box.Show();
        }

        #region Disposable

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);

            if (!disposing) return;

            Label = null;
            OKButton = null;
            CancelButton = null;
            NoButton = null;
            YesButton = null;
            Buttons = 0;

            for (int i = 0; i < Program.Form.Controls.Count; i++)
            {
                TextBox T = (TextBox) Program.Form.Controls[i];
                if (T != null && T.Tag != null)
                    ((MirTextBox) T.Tag).DialogChanged();
            }
        }

        #endregion
    }
}
