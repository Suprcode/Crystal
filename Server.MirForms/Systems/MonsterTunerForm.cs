using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.MirForms.Systems
{
    public partial class MonsterTunerForm : Form
    {
        public Envir Envir
        {
            get { return SMain.Envir; }
        }

        public MonsterTunerForm()
        {
            InitializeComponent();
            
            for (int i = 0; i < Envir.MonsterInfoList.Count; i++)
            {
                SelectMonsterComboBox.Items.Add(Envir.MonsterInfoList[i]);
            }
        }

        private void SelectMonsterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            MonsterInfo monster = (MonsterInfo)comboBox.SelectedItem;

            if (monster == null) return;

            MonsterNameTextBox.Text = monster.Name;
            HPTextBox.Text = monster.HP.ToString();
            EffectTextBox.Text = monster.Effect.ToString();
            LevelTextBox.Text = monster.Level.ToString();
            ViewRangeTextBox.Text = monster.ViewRange.ToString();
            CoolEyeTextBox.Text = monster.CoolEye.ToString();
            MinACTextBox.Text = monster.MinAC.ToString();
            MaxACTextBox.Text = monster.MaxAC.ToString();
            MinMACTextBox.Text = monster.MinMAC.ToString();
            MaxMACTextBox.Text = monster.MaxMAC.ToString();
            MinDCTextBox.Text = monster.MinDC.ToString();
            MaxDCTextBox.Text = monster.MaxDC.ToString();
            MinMCTextBox.Text = monster.MinMC.ToString();
            MaxMCTextBox.Text = monster.MaxMC.ToString();
            MinSCTextBox.Text = monster.MinSC.ToString();
            MaxSCTextBox.Text = monster.MaxSC.ToString();
            AccuracyTextBox.Text = monster.Accuracy.ToString();
            AgilityTextBox.Text = monster.Agility.ToString();
            ASpeedTextBox.Text = monster.AttackSpeed.ToString();
            MSpeedTextBox.Text = monster.MoveSpeed.ToString();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            MonsterInfo monster = (MonsterInfo)SelectMonsterComboBox.SelectedItem;

            if (monster == null) return;

            try
            {
                monster.HP = uint.Parse(HPTextBox.Text);
                monster.Effect = byte.Parse(EffectTextBox.Text);
                monster.Level = ushort.Parse(LevelTextBox.Text);
                monster.ViewRange = byte.Parse(ViewRangeTextBox.Text);
                monster.CoolEye = byte.Parse(CoolEyeTextBox.Text);
                monster.MinAC = ushort.Parse(MinACTextBox.Text);
                monster.MaxAC = ushort.Parse(MaxACTextBox.Text);
                monster.MinMAC = ushort.Parse(MinMACTextBox.Text);
                monster.MaxMAC = ushort.Parse(MaxMACTextBox.Text);
                monster.MinDC = ushort.Parse(MinDCTextBox.Text);
                monster.MaxDC = ushort.Parse(MaxDCTextBox.Text);
                monster.MinMC = ushort.Parse(MinMCTextBox.Text);
                monster.MaxMC = ushort.Parse(MaxMCTextBox.Text);
                monster.MinSC = ushort.Parse(MinSCTextBox.Text);
                monster.MaxSC = ushort.Parse(MaxSCTextBox.Text);
                monster.Accuracy = byte.Parse(AccuracyTextBox.Text);
                monster.Agility = byte.Parse(AgilityTextBox.Text);
                monster.AttackSpeed = ushort.Parse(ASpeedTextBox.Text);
                monster.MoveSpeed = ushort.Parse(MSpeedTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Value validation failed. Please correct before updating", "Notice",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            foreach (var item in Envir.Objects)
            {
                if (item.Race != ObjectType.Monster) continue;

                MonsterObject mob = (MonsterObject)item;

                mob.RefreshAll();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SelectMonsterComboBox.Items.Count; i++)
            {
                MonsterInfo mob = (MonsterInfo)SelectMonsterComboBox.Items[i];

                if (mob == null) continue;

                if (Envir.MonsterInfoList[i].Index != mob.Index) break;

                Envir.MonsterInfoList[i] = mob;
            }

            Envir.SaveDB();
        }
    }
}
