using Server.MirEnvir;
using System;
using System.Windows.Forms;

namespace Server.Custom
{
    public partial class CustomMobAISettings : Form
    {
        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }
        public CustomAI monster;
        private string mobName = "";
        public string MonsterName
        {
            get { return mobName; }
            set { mobName = value; }
        }
        string[,] Effects = new string[,]
        {
            //Changed to the correct names of the effects. Pete107|Petesn00beh. Updated 26/02/2016
            { "None", "Entrapment", "GreatFoxSpiritAura" },
            { "None", "Entrapment", "GreatFoxSpiritAura" },
            { "None", "Entrapment", "GreatFoxSpiritAura" },
            { "None", "Entrapment", "GreatFoxSpiritAura" },
            { "None", "Entrapment", "GreatFoxSpiritAura" },
        };
        public CustomMobAISettings()
        {
            InitializeComponent();
            effectPanel.Hide();
            magicdmgBox.Hide();
            magicdmgLbl.Hide();
            rangedmgBox.Hide();
            rangedmgLbl.Hide();
            specialdmgBox.Hide();
            specialdmgLbl.Hide();
            meleedmgBox.Hide();
            meleedmgLbl.Hide();
            massdmgBox.Hide();
            massdmgLbl.Hide();
            dmgToPetBox.Hide();
            dmgToPetLbl.Hide();
            dmgToClassBox.Hide();
            dmgToClassLbl.Hide();
            dmgToTargetBox.Hide();
            dmgToTargetLbl.Hide();
            targetClass.SelectedIndex = 5;
            editaddPanel.Hide();
            editaddPanel.Dock = DockStyle.Fill;
            spawnList.Hide();
            deleteButton.Hide();
            editButton.Hide();
            addSpawn.Hide();
            mobList.Hide();
            editItemPanel.Hide();
            editItemPanel.Dock = DockStyle.Fill;
            itemInfoBox.Hide();
            guidePanel.Hide();
            guidePanel.Dock = DockStyle.Fill;
            massEffectButton.Hide();
            rangeEffectButton.Hide();
            meleeEffectButton.Hide();
            specialEffectButton.Hide();
            magicEffectButton.Hide();
            massOption.Hide();
            magicOption.Hide();
            targetOption.Hide();
            petOption.Hide();
            meleeOption.Hide();
            rangeOption.Hide();
            classOption.Hide();
            minuteBox.Hide();
            hourBox.Hide();
            dayBox.Hide();
        }

        private void CustomMobAISettings_Load(object sender, EventArgs e)
        {
            //Pete107|Petesn00beh. Updated 02/03/2016
            if (MonsterName.Length > 0)
                if (Envir.CustomAIList.Count > 0) // List has values
                {
                    bool found = false;
                    for (int i = 0; i < Envir.CustomAIList.Count; i++)
                    {
                        if (Envir.CustomAIList[i].Name == MonsterName) // Find the monster
                        {
                            monster = Envir.CustomAIList[i]; // Monster found, assign it to the monster.
                            found = true;
                        }
                    }
                    if (!found) // Monster wasn't found, create a new one.
                    {
                        monster = new CustomAI();
                        monster = monster.LoadCustomAI(MonsterName);
                        Envir.CustomAIList.Add(monster);
                    }
                }
                else // List has no values
                {
                    monster = new CustomAI();
                    monster = monster.LoadCustomAI(MonsterName);
                    Envir.CustomAIList.Add(monster); // Add it to the List within the environment
                }
            minuteLbl.Hide();
            minuteBox.Hide();
            dayBox.Hide();
            dayLbl.Hide();
            hourBox.Hide();
            hourLbl.Hide();
            meleeOption.Hide();
            magicOption.Hide();
            rangeOption.Hide();
            massOption.Hide();
            specialOption.Hide();
            petOption.Hide();
            targetOption.Hide();
            classOption.Hide();
            dmgToClassBox.Hide();
            dmgToClassLbl.Hide();
            mobnameBox.Text = monster.Name;
            useMagicAttack.Checked = monster.UseMagicAttack;
            useRangeAttack.Checked = monster.UseRangeAttack;
            useMassAttack.Checked = monster.UseMassAttack;
            useMeleeAttack.Checked = monster.UseMeleeAttack;
            useSpecialAttack.Checked = monster.UseSpecialAttack;
            usespawnMessage.Checked = monster.AnnounceSpawn;
            spawnMessageBox.Text = monster.SpawnMessage;
            useDeathMessage.Checked = monster.AnnounceDeath;
            deathMessageBox.Text = monster.DeadMessage;
            useItemDropMessage.Checked = monster.AnnounceDrop;
            dropMessageBox.Text = monster.ItemMessage;
            targetClass.SelectedIndex = monster.TargetClass;
            ignorePets.Checked = monster.IgnorePets;
            spawnSlaves.Checked = monster.Spawn_Slaves;
            meleedmgBox.Text = monster.MeleeAttackDamage.ToString();
            magicdmgBox.Text = monster.MagicAttackDamage.ToString();
            massdmgBox.Text = monster.MassAttackDamage.ToString();
            rangedmgBox.Text = monster.RangeAttackDamage.ToString();
            specialdmgBox.Text = monster.SpecialAttackDamage.ToString();
            dmgToPetBox.Text = monster.PetAttackDamage.ToString();
            dmgToTargetBox.Text = monster.TargetAttackDamage.ToString();
            canParaBox.Checked = monster.CanPara;
            canRedBox.Checked = monster.CanRed;
            canGreenBox.Checked = monster.CanGreen;
            killTimerBox.Checked = monster.UseKillTimer;
            minuteBox.Text = monster.RespawnMinute.ToString();
            dayBox.Text = monster.RespawnDay.ToString();
            hourBox.Text = monster.RespawnHour.ToString();
            dmgPetMoreBox.Checked = monster.DamagePetsMore;
            if (monster.Slaves != null && monster.Slaves.Count > 0 && monster.Spawn_Slaves)
                for (int i = 0; i < monster.Slaves.Count; i++)
                    spawnList.Items.Add(monster.Slaves[i].Name);
            if (monster.ItemCount > 0 && monster.Drops != null && monster.Drops.Count > 0)
                for (int i = 0; i < monster.Drops.Count; i++)
                    itemBox.Items.Add(monster.Drops[i].Name);

            effectPanel.PreviewKeyDown += EffectPanel_PreviewKeyDown;
        }

        private void EffectPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                effectPanel.Hide();
        }

        private void useMeleeAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (useMeleeAttack.Checked)
            {
                meleeOption.Show(); // Using melee
                meleedmgBox.Show();
                meleedmgLbl.Show();
                rangeOption.Hide(); // Can't use range
                rangedmgBox.Hide();
                rangedmgLbl.Hide();
                magicdmgBox.Hide(); // Can't use magic
                magicdmgLbl.Hide();
                magicOption.Hide();
                useMagicAttack.Checked = false;
                useRangeAttack.Checked = false;
                meleeEffectButton.Show();
            }
            else
            {
                meleeOption.Hide();
                meleedmgLbl.Hide();
                meleedmgBox.Hide();
                meleeEffectButton.Hide();
            }
            monster.UseMeleeAttack = useMeleeAttack.Checked;
            monster.UseMagicAttack = useMagicAttack.Checked;
            monster.UseRangeAttack = useRangeAttack.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x01");
        }

        private void useMagicAttack_CheckedChanged(object sender, EventArgs e)
        {

            if (useMagicAttack.Checked)
            {
                rangeOption.Hide(); // Cant use range
                rangedmgBox.Hide();
                rangedmgLbl.Hide();
                meleeOption.Hide(); // Cant use melee
                meleedmgLbl.Hide();
                meleedmgBox.Hide();
                meleeOption.Hide();
                magicdmgBox.Show(); // Can use magic
                magicdmgLbl.Show();
                magicOption.Show();
                useMeleeAttack.Checked = false;
                useRangeAttack.Checked = false;
                magicEffectButton.Show();
            }
            else
            {
                magicdmgBox.Hide();
                magicdmgLbl.Hide();
                magicOption.Hide();
                magicEffectButton.Hide();
            }
            monster.UseMeleeAttack = useMeleeAttack.Checked;
            monster.UseMagicAttack = useMagicAttack.Checked;
            monster.UseRangeAttack = useRangeAttack.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x02");
        }

        private void useRangeAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (useRangeAttack.Checked)
            {
                rangeOption.Show(); // Using range
                rangedmgBox.Show();
                rangedmgLbl.Show();
                rangeEffectButton.Show();
                meleeOption.Hide(); // Can't use melee
                meleedmgLbl.Hide();
                meleedmgBox.Hide();
                magicdmgBox.Hide(); // Can't use magic
                magicdmgLbl.Hide();
                useMeleeAttack.Checked = false;
                useMagicAttack.Checked = false;
            }
            else
            {
                rangeOption.Hide();
                rangedmgBox.Hide();
                rangedmgLbl.Hide();
                rangeEffectButton.Hide();
            }
            monster.UseMeleeAttack = useMeleeAttack.Checked;
            monster.UseMagicAttack = useMagicAttack.Checked;
            monster.UseRangeAttack = useRangeAttack.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x03");
        }

        private void targetWeak_CheckedChanged(object sender, EventArgs e)
        {
            if (targetWeak.Checked && targetClass.SelectedIndex >= 0 && targetClass.SelectedIndex <= 4) // Target class
            {
                classOption.Show();
                dmgToClassBox.Show();
                dmgToClassLbl.Show();
                targetOption.Hide();
                dmgToTargetLbl.Hide();
                dmgToTargetBox.Hide();
            }
            else if (targetWeak.Checked && targetClass.SelectedIndex == 5) // Target any
            {
                dmgToTargetLbl.Show();
                dmgToTargetBox.Show();
                targetOption.Show();
                dmgToClassLbl.Hide();
                dmgToClassBox.Hide();
                classOption.Hide();
            }
            else if (!targetWeak.Checked) // none
            {
                classOption.Hide();
                targetOption.Hide();
                dmgToTargetBox.Hide();
                dmgToTargetLbl.Hide();
                dmgToClassBox.Hide();
                dmgToClassLbl.Hide();
                return;
            }
            monster.Target = targetWeak.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x04");
        }


        private void useSpecialAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (useSpecialAttack.Checked)
            {
                specialOption.Show();
                specialdmgBox.Show();
                specialdmgLbl.Show();
                specialEffectButton.Show();
            }
            else
            {
                specialOption.Hide();
                specialdmgBox.Hide();
                specialdmgLbl.Hide();
                specialEffectButton.Hide();
            }
            monster.UseSpecialAttack = useSpecialAttack.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x05");
        }

        private void useMassAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (useMassAttack.Checked)
            {
                massOption.Show();
                massdmgBox.Show();
                massdmgLbl.Show();
                massEffectButton.Show();
            }
            else
            {
                massOption.Hide();
                massdmgBox.Hide();
                massdmgLbl.Hide();
                massEffectButton.Hide();
            }
            monster.UseMassAttack = useMassAttack.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x06");
        }

        private void usespawnMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (usespawnMessage.Checked)
                spawnMessageBox.Show();
            else
                spawnMessageBox.Hide();
            monster.AnnounceSpawn = usespawnMessage.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x07");
        }

        private void targetClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (targetClass.SelectedIndex == -1) return;
            if (targetClass.SelectedIndex >= 0 && targetClass.SelectedIndex <= 4)
            {
                dmgToClassBox.Show();
                dmgToClassLbl.Show();
                classOption.Show();
                targetOption.Hide();
                dmgToTargetBox.Hide();
                dmgToTargetLbl.Hide();
                monster.Target = targetWeak.Checked;
                monster.TargetClass = targetClass.SelectedIndex;
                if (monster.Name.Length <= 0) return;
                if (!monster.Save(monster))
                    SMain.Enqueue("ERROR : 01x08");
            }
            else if (targetClass.SelectedIndex == 5 && targetWeak.Checked)
            {
                classOption.Hide();
                dmgToClassBox.Hide();
                dmgToClassLbl.Hide();
                dmgToTargetBox.Show();
                dmgToTargetLbl.Show();
                targetOption.Show();
                monster.Target = targetWeak.Checked;
                monster.TargetClass = targetClass.SelectedIndex;
                if (monster.Name.Length <= 0) return;
                if (!monster.Save(monster))
                    SMain.Enqueue("ERROR : 02x08");
            }
            else if (targetClass.SelectedIndex == 5 && !targetWeak.Checked)
            {
                dmgToClassBox.Hide();
                dmgToClassLbl.Hide();
                dmgToTargetBox.Hide();
                dmgToTargetLbl.Hide();
                classOption.Hide();
                targetOption.Hide();
                if (monster != null)
                {
                    monster.Target = targetWeak.Checked;
                    monster.TargetClass = targetClass.SelectedIndex;
                    if (monster.Name.Length <= 0) return;
                    if (!monster.Save(monster))
                        SMain.Enqueue("ERROR : 03x08");
                }
            }
        }

        private void ignorePets_CheckedChanged(object sender, EventArgs e)
        {
            if (ignorePets.Checked)
            {
                dmgToPetBox.Hide();
                dmgToPetLbl.Hide();
                //Can't deal more damage to pets when we're ignoring pets
                dmgPetMoreBox.Checked = false;
                petOption.Hide();
            }
            else
            {
                petOption.Show();
                dmgToPetBox.Show();
                dmgToPetLbl.Show();
            }
            monster.IgnorePets = ignorePets.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x09");
        }

        private void addSpawn_Click(object sender, EventArgs e)
        {
            if (spawnList.Items.Count >= 4)
            {
                SMain.Enqueue("ERROR : 00x10");
                return;
            }

            if (spawnList.SelectedIndex >= 0 &&
                spawnList.SelectedIndex <= 3)
                editaddPanel.Show();
            else
                return;

        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (spawnList.SelectedIndex >= 0 &&
                spawnList.SelectedIndex <= 3)
            {
                if (monster.Slaves[spawnList.SelectedIndex] != null)
                {
                    slaveNameBox.Text = monster.Slaves[spawnList.SelectedIndex].Name;
                    slaveCountBox.Text = monster.Slaves[spawnList.SelectedIndex].Count.ToString();
                    editaddPanel.Show();
                }
            }
            else
                return;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (spawnList.SelectedIndex >= 0 &&
                spawnList.SelectedIndex <= 3)
            {
                if (monster.Slaves[spawnList.SelectedIndex] != null)
                {
                    monster.Slaves.RemoveAt(spawnList.SelectedIndex);
                    spawnList.Items.RemoveAt(spawnList.SelectedIndex);
                }
            }
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x11");
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            editaddPanel.Hide();
        }

        private void saveSlaveButton_Click(object sender, EventArgs e)
        {
            int _slaveCount = 0;
            if (slaveNameBox.Text.Length > 0)
            {
                if (!int.TryParse(slaveCountBox.Text, out _slaveCount))
                {
                    MessageBox.Show("Slave count must be a number!");
                    return;
                }
                else
                {
                    if (_slaveCount <= 0 || _slaveCount > 10)
                    {
                        MessageBox.Show("Slave count must be between 1-10");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Slave name is too short!");
                return;
            }
            if (_slaveCount <= 0) return;
            monster.Slaves[spawnList.SelectedIndex].Name = slaveNameBox.Text;
            monster.Slaves[spawnList.SelectedIndex].Count = _slaveCount;
            monster.Save(monster);
            spawnList.Items[spawnList.SelectedIndex] = slaveNameBox.Text;
            editaddPanel.Hide();
        }

        private void slaveCountBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;

            if (!int.TryParse(slaveCountBox.Text, out temp))
            {
                MessageBox.Show("Must enter numbers only");
                return;
            }
            else if (temp <= 0 && temp > 10)
            {
                MessageBox.Show("Slave Count must be between 1 and 10");
                return;
            }
            else if (temp == -1) return;
        }

        private void spawnSlaves_CheckedChanged(object sender, EventArgs e)
        {
            if (spawnSlaves.Checked)
            {
                spawnList.Show();
                deleteButton.Show();
                editButton.Show();
                addSpawn.Show();
            }
            else
            {
                spawnList.Hide();
                deleteButton.Hide();
                editButton.Hide();
                addSpawn.Hide();

            }
            monster.Spawn_Slaves = spawnSlaves.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x12");
        }

        private void mobDBListButton_Click(object sender, EventArgs e)
        {
            mobList.Show();
            if (mobList.Items.Count > 0)
                mobList.Items.Clear();
            for (int i = 0; i < Envir.MonsterInfoList.Count; i++)
                mobList.Items.Add(Envir.MonsterInfoList[i].Name);
        }

        private void mobList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mobList.SelectedIndex > Envir.MonsterInfoList.Count) return;
            if (Envir.MonsterInfoList[mobList.SelectedIndex] != null &&
                Envir.MonsterInfoList[mobList.SelectedIndex].Name.Length > 0)
            {
                slaveNameBox.Text = Envir.MonsterInfoList[mobList.SelectedIndex].Name;
            }
        }

        private void addItem_Click(object sender, EventArgs e)
        {
            itemInfoBox.Hide();
            editItemPanel.Show();
        }

        private void editItem_Click(object sender, EventArgs e)
        {
            itemInfoBox.Hide();
            if (itemBox.SelectedIndex >= 0)
            {
                itemNameBox.Text = monster.Drops[itemBox.SelectedIndex].Name;
                itemRateBox.Text = monster.Drops[itemBox.SelectedIndex].Chance.ToString();
            }
            editItemPanel.Show();
        }

        private void deleteItem_Click(object sender, EventArgs e)
        {
            if (itemBox.SelectedIndex >= 0)
            {
                if (monster.Drops[itemBox.SelectedIndex] != null)
                {
                    monster.Drops.RemoveAt(itemBox.SelectedIndex);
                    itemBox.Items.RemoveAt(itemBox.SelectedIndex);
                }
            }
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x13");
        }

        private void viewItemButton_Click(object sender, EventArgs e)
        {
            itemInfoBox.Show();
            if (itemInfoBox.Items.Count > 0)
                itemInfoBox.Items.Clear();
            if (Envir.ItemInfoList.Count > 0)
                for (int i = 0; i < Envir.ItemInfoList.Count; i++)
                    itemInfoBox.Items.Add(Envir.ItemInfoList[i].Name);
        }

        private void editItemCancel_Click(object sender, EventArgs e)
        {
            editItemPanel.Hide();
        }

        private void itemSaveButton_Click(object sender, EventArgs e)
        {
            int rate = 0;
            if (itemNameBox.Text.Length > 0 && int.TryParse(itemRateBox.Text, out rate))
            {
                if (rate < 1 || rate >= 100)
                {
                    MessageBox.Show("Rate must be between 1 and 99");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Name or Rate is invalid");
                return;
            }
            if (monster.Drops.Count <= 0)
                monster.Drops.Add(new DropItemsWithAnnounce(itemNameBox.Text, rate));
            else
            {
                bool found = false;
                for (int i = 0; i < monster.Drops.Count; i++)
                {
                    if (monster.Drops[i].Name == itemNameBox.Text)
                    {
                        monster.Drops[i].Chance = rate;
                        found = true;
                    }
                }
                if (!found)
                    monster.Drops.Add(new DropItemsWithAnnounce(itemNameBox.Text, rate));
            }

            if (itemBox.SelectedIndex == -1)
                itemBox.Items.Add(itemNameBox.Text);
            else
                itemBox.Items[itemBox.SelectedIndex] = itemNameBox.Text;

            monster.ItemCount = itemBox.Items.Count;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x14");
            editItemPanel.Hide();
            itemInfoBox.Hide();
        }

        private void itemInfoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemInfoBox.SelectedIndex != -1)
            {
                itemNameBox.Text = Envir.ItemInfoList[itemInfoBox.SelectedIndex].Name;
            }
            else
                return;
        }

        private void CustomMobAISettings_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void guideButton_Click(object sender, EventArgs e)
        {
            guidePanel.Show();
        }

        private void closeGuidePanel_Click(object sender, EventArgs e)
        {
            guidePanel.Hide();
        }

        private void dropMessageBox_TextChanged(object sender, EventArgs e)
        {
            if (dropMessageBox.Text.Length > 0 && monster.ItemMessage != dropMessageBox.Text)
                monster.ItemMessage = dropMessageBox.Text;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x15");
        }

        private void deathMessageBox_TextChanged(object sender, EventArgs e)
        {
            if (deathMessageBox.Text.Length > 0 && monster.DeadMessage != deathMessageBox.Text)
                monster.DeadMessage = deathMessageBox.Text;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x16");
        }

        private void spawnMessageBox_TextChanged(object sender, EventArgs e)
        {
            if (spawnMessageBox.Text.Length > 0 && monster.SpawnMessage != spawnMessageBox.Text)
                monster.SpawnMessage = spawnMessageBox.Text;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x17");
        }

        private void massdmgBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(massdmgBox.Text, out temp))
            {
                MessageBox.Show("Value must be between 0-65535");
                return;
            }
            if (temp != -1)
                monster.MassAttackDamage = temp;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x18");
        }

        private void meleedmgBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(meleedmgBox.Text, out temp))
            {
                MessageBox.Show("Value must be between 0-65535");
                return;
            }
            if (temp != -1)
                monster.MeleeAttackDamage = temp;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x19");
        }

        private void magicdmgBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(magicdmgBox.Text, out temp))
            {
                MessageBox.Show("Value must be between 0 - 65535");
                return;
            }
            if (temp != -1)
                monster.MagicAttackDamage = temp;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x20");
        }

        private void rangedmgBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(rangedmgBox.Text, out temp))
            {
                MessageBox.Show("Value must be between 0 - 65535");
                return;
            }

            if (temp != -1)
                monster.RangeAttackDamage = temp;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x21");
        }

        private void specialdmgBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(specialdmgBox.Text, out temp))
            {
                MessageBox.Show("Value must be between 0 - 65535");
                return;
            }

            if (temp != -1)
                monster.SpecialAttackDamage = temp;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x22");
        }

        private void dmgToPetBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(dmgToPetBox.Text, out temp))
            {
                MessageBox.Show("Value must be between 0 - 65535");
                return;
            }

            if (temp != -1)
                monster.PetAttackDamage = temp;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x23");
        }

        private void dmgToClassBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(dmgToClassBox.Text, out temp))
            {
                MessageBox.Show("Value must be between 0 - 65535");
                return;
            }

            if (temp != -1)
                monster.TargetAttackDamage = temp;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x24");
        }

        private void dmgToTargetBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(dmgToTargetBox.Text, out temp))
            {
                MessageBox.Show("Value must be between 0 - 65535");
                return;
            }

            if (temp != -1)
                monster.TargetAttackDamage = temp;
            else
                return;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x25");
        }

        private void canParaBox_CheckedChanged(object sender, EventArgs e)
        {
            monster.CanPara = canParaBox.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x26");
        }

        private void canGreenBox_CheckedChanged(object sender, EventArgs e)
        {
            monster.CanGreen = canGreenBox.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x27");
        }

        private void canRedBox_CheckedChanged(object sender, EventArgs e)
        {
            monster.CanRed = canRedBox.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x28");
        }

        private void dmgPetMoreBox_CheckedChanged(object sender, EventArgs e)
        {
            if (dmgPetMoreBox.Checked)
            {
                //Can't ignore pet if we're dealing more damage
                ignorePets.Checked = false;
                dmgToPetBox.Show();
                dmgToPetLbl.Show();
            }
            else
            {
                dmgToPetBox.Hide();
                dmgToPetLbl.Hide();
            }
            monster.DamagePetsMore = dmgPetMoreBox.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x29");
        }

        private void minuteBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(minuteBox.Text, out temp))
            {
                MessageBox.Show("Values must be between 0 and 59");
                return;
            }
            if (temp < 0 || temp > 59)
            {
                MessageBox.Show("Value must be between 0 and 59");
                return;
            }

            monster.RespawnMinute = temp;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x30");
        }

        private void killTimerBox_CheckedChanged(object sender, EventArgs e)
        {
            if (killTimerBox.Checked)
            {
                minuteBox.Show();
                minuteLbl.Show();
                hourBox.Show();
                hourLbl.Show();
                dayBox.Show();
                dayLbl.Show();
            }
            else
            {
                minuteBox.Hide();
                minuteLbl.Hide();
                hourBox.Hide();
                hourLbl.Hide();
                dayBox.Hide();
                dayLbl.Hide();
            }
            monster.UseKillTimer = killTimerBox.Checked;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x31");
        }

        private void hourBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(hourBox.Text, out temp))
            {
                MessageBox.Show("Values must be between 0 and 23");
                return;
            }
            if (temp < 0 || temp > 23)
            {
                MessageBox.Show("Value must be between 0 and 23");
                return;
            }

            monster.RespawnHour = temp;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x32");
        }

        private void dayBox_TextChanged(object sender, EventArgs e)
        {
            int temp = -1;
            if (!int.TryParse(dayBox.Text, out temp))
            {
                MessageBox.Show("Values must be between 0 and 7");
                return;
            }
            if (temp < 0 || temp > 7)
            {
                MessageBox.Show("Value must be between 0 and 7");
                return;
            }

            monster.RespawnDay = temp;
            if (monster.Name.Length <= 0) return;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x33");
        }
        public static int UsingWhich = -1;
        private void massEffectButton_Click(object sender, EventArgs e)
        {
            effectList.Items.Clear();
            UsingWhich = 0;
            for (int i = 0; i <= Effects.GetUpperBound(1); i++)
                effectList.Items.Add(Effects[UsingWhich, i]);
            effectList.SelectedIndex = monster.MassAttackEffect;
            effectPanel.Show();
            effectPanel.Focus();
        }


        private void meleeEffectButton_Click(object sender, EventArgs e)
        {
            effectList.Items.Clear();
            UsingWhich = 1;
            for (int i = 0; i <= Effects.GetUpperBound(1); i++)
                effectList.Items.Add(Effects[UsingWhich, i]);
            effectList.SelectedIndex = monster.MeleeAttackEffect;
            effectPanel.Show();
            effectPanel.Focus();
        }

        private void rangeEffectButton_Click(object sender, EventArgs e)
        {
            effectList.Items.Clear();
            UsingWhich = 2;
            for (int i = 0; i <= Effects.GetUpperBound(1); i++)
                effectList.Items.Add(Effects[UsingWhich, i]);
            effectList.SelectedIndex = monster.RangeAttackEffect;
            effectPanel.Show();
            effectPanel.Focus();
        }

        private void magicEffectButton_Click(object sender, EventArgs e)
        {
            effectList.Items.Clear();
            UsingWhich = 3;
            for (int i = 0; i <= Effects.GetUpperBound(1); i++)
                effectList.Items.Add(Effects[UsingWhich, i]);
            effectList.SelectedIndex = monster.MagicAttackEffect;
            effectPanel.Show();
            effectPanel.Focus();
        }

        private void specialEffectButton_Click(object sender, EventArgs e)
        {
            effectList.Items.Clear();
            UsingWhich = 4;
            for (int i = 0; i <= Effects.GetUpperBound(1); i++)
                effectList.Items.Add(Effects[UsingWhich, i]);
            effectList.SelectedIndex = monster.MeleeAttackEffect;
            effectPanel.Show();
            effectPanel.Focus();
        }

        private void effectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (effectList.SelectedIndex != -1)
            {
                switch (UsingWhich)
                {
                    case 0: // Mass
                        monster.MassAttackEffect = effectList.SelectedIndex;
                        if (!monster.Save(monster))
                            SMain.Enqueue("ERROR : 00x34");
                        break;
                    case 1: // Melee
                        monster.MeleeAttackEffect = effectList.SelectedIndex;
                        if (!monster.Save(monster))
                            SMain.Enqueue("ERROR : 01x34");
                        break;
                    case 2: // Range
                        monster.RangeAttackEffect = effectList.SelectedIndex;
                        if (!monster.Save(monster))
                            SMain.Enqueue("ERROR : 02x34");
                        break;
                    case 3: // Magic
                        monster.MagicAttackEffect = effectList.SelectedIndex;
                        if (!monster.Save(monster))
                            SMain.Enqueue("ERROR : 03x34");
                        break;
                    case 4: // Special
                        monster.SpecialAttackEffect = effectList.SelectedIndex;
                        if (!monster.Save(monster))
                            SMain.Enqueue("ERROR : 04x34");
                        break;
                    default:
                        MessageBox.Show("Value is invalid.");
                        return;
                }
            }
            else
                return;
        }

        private void cancelEffect_Click(object sender, EventArgs e)
        {
            effectPanel.Hide();
        }

        private void useDeathMessage_CheckedChanged(object sender, EventArgs e)
        {
            monster.AnnounceDeath = useDeathMessage.Checked;
            if (monster.AnnounceDeath && deathMessageBox.Text.Length > 0)
                monster.DeadMessage = deathMessageBox.Text;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x35");
        }

        private void useItemDropMessage_CheckedChanged(object sender, EventArgs e)
        {
            monster.AnnounceDrop = useItemDropMessage.Checked;
            if (monster.AnnounceDrop && dropMessageBox.Text.Length > 0)
                monster.ItemMessage = dropMessageBox.Text;
            if (!monster.Save(monster))
                SMain.Enqueue("ERROR : 00x36");
        }
    }
}
