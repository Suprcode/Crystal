using Client;
using System.Resources;
using System.Reflection;
using Client.Resolution;

namespace Launcher
{

    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            this.label10.Text = GameLanguage.Resolution;
            this.AutoStart_label.Text = GameLanguage.Autostart;
            this.ID_l.Text = GameLanguage.Usrname;
            this.Password_l.Text = GameLanguage.Password;

            DrawSupportedResolutions();
        }
                                   
        private void Res1_pb_Click(object sender, EventArgs e)
        {
            resolutionChoice(eSupportedResolution.w1024h768);

        }

        public void resolutionChoice(eSupportedResolution res)
        {
            Res2_pb.Image = Client.Resources.Images.Radio_Unactive;
            Res3_pb.Image = Client.Resources.Images.Radio_Unactive;
            Res4_pb.Image = Client.Resources.Images.Radio_Unactive;
            Res5_pb.Image = Client.Resources.Images.Radio_Unactive;

            switch (res)
            {
                case eSupportedResolution.w1024h768:
                    Res2_pb.Image = Client.Resources.Images.Config_Radio_On;
                    break;
                case eSupportedResolution.w1366h768:
                    Res3_pb.Image = Client.Resources.Images.Config_Radio_On;
                    break;
                case eSupportedResolution.w1280h720:
                    Res4_pb.Image = Client.Resources.Images.Config_Radio_On;
                    break;
                case eSupportedResolution.w1920h1080:
                    Res5_pb.Image = Client.Resources.Images.Config_Radio_On;
                    break;

            }

            Settings.Resolution = (int)res;
        }

        private void Res2_pb_Click(object sender, EventArgs e)
        {
            resolutionChoice(eSupportedResolution.w1024h768);
        }

        private void Res3_pb_Click(object sender, EventArgs e)
        {
            resolutionChoice(eSupportedResolution.w1366h768);
        }

        private void Config_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                AccountLogin_txt.Text = Settings.AccountID;
                AccountPass_txt.Text = Settings.Password;
                resolutionChoice((eSupportedResolution)Settings.Resolution);

                Fullscreen_pb.Image = Settings.FullScreen
                    ? Client.Resources.Images.Config_Check_On
                    : Client.Resources.Images.Config_Check_Off1;

                FPScap_pb.Image = Settings.FPSCap
                    ? Client.Resources.Images.Config_Check_On
                    : Client.Resources.Images.Config_Check_Off1;

                OnTop_pb.Image = Settings.TopMost
                    ? Client.Resources.Images.Config_Check_On
                    : Client.Resources.Images.Config_Check_Off1;

                AutoStart_pb.Image = Settings.P_AutoStart
                    ? Client.Resources.Images.Config_Check_On
                    : Client.Resources.Images.Config_Check_Off1;

                this.ActiveControl = label4;
            }
            else
            {             
                Settings.AccountID = AccountLogin_txt.Text;
                Settings.Password = AccountPass_txt.Text;
                Settings.Save();
            }
        }

        private void AccountLogin_txt_TextChanged(object sender, EventArgs e)
        {
            if (AccountLogin_txt.Text == string.Empty) ID_l.Visible = true;
            else ID_l.Visible = false;
        }

        private void AccountPass_txt_TextChanged(object sender, EventArgs e)
        {
            if (AccountPass_txt.Text == string.Empty) Password_l.Visible = true;
            else Password_l.Visible = false;
        }

        private void AccountLogin_txt_Click(object sender, EventArgs e)
        {
            ID_l.Visible = false;
            AccountLogin_txt.Focus();
        }

        private void AccountPass_txt_Click(object sender, EventArgs e)
        {
            Password_l.Visible = false;
            AccountPass_txt.Focus();
        }

        private void Config_Click(object sender, EventArgs e)
        {
            this.ActiveControl = label4;
        }

        private void Fullscreen_pb_Click(object sender, EventArgs e)
        {
            Settings.FullScreen = !Settings.FullScreen;

            Fullscreen_pb.Image = Settings.FullScreen
                    ? Client.Resources.Images.Config_Check_On
                    : Client.Resources.Images.Config_Check_Off1;
        }

        private void FPScap_pb_Click(object sender, EventArgs e)
        {
            Settings.FPSCap = !Settings.FPSCap;

            FPScap_pb.Image = Settings.FPSCap
                    ? Client.Resources.Images.Config_Check_On
                    : Client.Resources.Images.Config_Check_Off1;
        }

        private void OnTop_pb_Click(object sender, EventArgs e)
        {
            Settings.TopMost = !Settings.TopMost;

            OnTop_pb.Image = Settings.TopMost
                    ? Client.Resources.Images.Config_Check_On
                    : Client.Resources.Images.Config_Check_Off1;
        }

        private void AutoStart_pb_Click(object sender, EventArgs e)
        {
            Settings.P_AutoStart = !Settings.P_AutoStart;

            AutoStart_pb.Image = Settings.P_AutoStart
                    ? Client.Resources.Images.Config_Check_On
                    : Client.Resources.Images.Config_Check_Off1;
        }

        private void CleanFiles_pb_MouseDown(object sender, MouseEventArgs e)
        {
            CleanFiles_pb.Image = Client.Resources.Images.CheckF_Pressed;
        }

        private void CleanFiles_pb_MouseUp(object sender, MouseEventArgs e)
        {
            CleanFiles_pb.Image = Client.Resources.Images.CheckF_Base2;
        }

        private void CleanFiles_pb_MouseEnter(object sender, EventArgs e)
        {
            CleanFiles_pb.Image = Client.Resources.Images.CheckF_Hover;
        }

        private void CleanFiles_pb_MouseLeave(object sender, EventArgs e)
        {
            CleanFiles_pb.Image = Client.Resources.Images.CheckF_Base2;
        }

        private void CleanFiles_pb_Click(object sender, EventArgs e)
        {
            if (!Program.PForm.Launch_pb.Enabled) return;

            Program.PForm.Completed = false;
            Program.PForm.InterfaceTimer.Enabled = true;
            Program.PForm.CleanFiles = true;
            Program.PForm._workThread = new Thread(Program.PForm.Start) { IsBackground = true };
            Program.PForm._workThread.Start();
        }

        private void Res4_pb_Click(object sender, EventArgs e)
        {
            resolutionChoice(eSupportedResolution.w1280h720);
        }

        private void Res5_pb_Click(object sender, EventArgs e)
        {
            resolutionChoice(eSupportedResolution.w1920h1080);
        }

        private void DrawSupportedResolutions()
        {
            Res2_pb.Enabled = false;
            label2.ForeColor = Color.Red;
            Res4_pb.Enabled = false;
            label5.ForeColor = Color.Red;
            Res3_pb.Enabled = false;
            label3.ForeColor = Color.Red;
            Res5_pb.Enabled = false;
            label1.ForeColor = Color.Red;

            foreach (eSupportedResolution supportedResolution in DisplayResolutions.DisplaySupportedResolutions)
            {
                switch (supportedResolution)
                {
                    case (eSupportedResolution.w1024h768):
                        Res2_pb.Enabled = true;
                        label2.ForeColor = Color.Gray;
                        break;
                    case (eSupportedResolution.w1280h720):
                        Res4_pb.Enabled = true;
                        label5.ForeColor = Color.Gray;
                        break;
                    case (eSupportedResolution.w1366h768):
                        Res3_pb.Enabled = true;
                        label3.ForeColor = Color.Gray;
                        break;
                    case (eSupportedResolution.w1920h1080):
                        Res5_pb.Enabled = true;
                        label1.ForeColor = Color.Gray;
                        break;
                }
            }
        }
    }
}
