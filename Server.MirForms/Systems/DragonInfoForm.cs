using System;
using System.Drawing;
using System.Windows.Forms;
using Server.MirDatabase;
using Server.MirEnvir;

namespace Server
{
    public partial class DragonInfoForm : Form
    {
        public Envir Envir
        {
            get { return SMain.Envir; }
        }

        public DragonInfoForm()
        {
            InitializeComponent();
        }

        private void UpdateInterface()
        {
            DragonInfo info = Envir.DragonInfo;
            if (info == null) return;

            EnableDragonCheckBox.CheckState = info.Enabled ? CheckState.Checked : CheckState.Unchecked;

            MapFileNameTextBox.Text = info.MapFileName;
            MonsterNameTextBox.Text = info.MonsterName;
            BodyNameTextBox.Text = info.BodyName;
            XTextBox.Text = info.Location.X.ToString();
            YTextBox.Text = info.Location.Y.ToString();
            DropAreaTopXTextBox.Text = info.DropAreaTop.X.ToString();
            DropAreaTopYTextBox.Text = info.DropAreaTop.Y.ToString();
            DropAreaBottomXTextBox.Text = info.DropAreaBottom.X.ToString();
            DropAreaBottomYTextBox.Text = info.DropAreaBottom.Y.ToString();

            label8.Text = info.Level.ToString();
            label9.Text = info.Experience.ToString();

            Level1ExpTextBox.Text = info.Exps[0].ToString();
            Level2ExpTextBox.Text = info.Exps[1].ToString();
            Level3ExpTextBox.Text = info.Exps[2].ToString();
            Level4ExpTextBox.Text = info.Exps[3].ToString();
            Level5ExpTextBox.Text = info.Exps[4].ToString();
            Level6ExpTextBox.Text = info.Exps[5].ToString();
            Level7ExpTextBox.Text = info.Exps[6].ToString();
            Level8ExpTextBox.Text = info.Exps[7].ToString();
            Level9ExpTextBox.Text = info.Exps[8].ToString();
            Level10ExpTextBox.Text = info.Exps[9].ToString();
            Level11ExpTextBox.Text = info.Exps[10].ToString();
            Level12ExpTextBox.Text = info.Exps[11].ToString();

            tabPage1.Enabled = info.Enabled;
            tabPage2.Enabled = info.Enabled;
        }

        private void DragonInfoForm_Load(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        private void MapFileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Envir.DragonInfo.MapFileName = ActiveControl.Text;
        }

        private void XTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            Envir.DragonInfo.Location.X = temp;
        }

        private void YTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            Envir.DragonInfo.Location.Y = temp;
        }

        private void MonsterNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Envir.DragonInfo.MonsterName = ActiveControl.Text;
        }

        private void BodyNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Envir.DragonInfo.BodyName = ActiveControl.Text;
        }

        private void DragonInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Envir.SaveDB();
        }

        private void EnableDragonCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Envir.DragonInfo.Enabled = EnableDragonCheckBox.Checked;
            UpdateInterface();
        }

        private void DropAreaTopXTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            Envir.DragonInfo.DropAreaTop.X = temp;
        }

        private void DropAreaTopYTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            Envir.DragonInfo.DropAreaTop.Y = temp;
        }

        private void DropAreaBottomXTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            Envir.DragonInfo.DropAreaBottom.X = temp;
        }

        private void DropAreaBottomYTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            Envir.DragonInfo.DropAreaBottom.Y = temp;
        }

        private void Level1ExpTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Int64 temp;

            if (!Int64.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            Envir.DragonInfo.Exps[Convert.ToInt32(ActiveControl.Tag)] = temp;
        }
    }
}
