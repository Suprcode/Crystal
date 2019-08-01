using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Server.MirEnvir;

namespace Server
{
    public partial class ItemInfoForm : Form
    {
        public string ItemListPath = Path.Combine(Settings.ExportPath, "ItemList.txt");

        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }
        private List<ItemInfo> _selectedItemInfos;

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        public ItemInfoForm()
        {
            InitializeComponent();

            ITypeComboBox.Items.AddRange(Enum.GetValues(typeof (ItemType)).Cast<object>().ToArray());
            IGradeComboBox.Items.AddRange(Enum.GetValues(typeof(ItemGrade)).Cast<object>().ToArray());
            RTypeComboBox.Items.AddRange(Enum.GetValues(typeof (RequiredType)).Cast<object>().ToArray());
            RClassComboBox.Items.AddRange(Enum.GetValues(typeof (RequiredClass)).Cast<object>().ToArray());
            RGenderComboBox.Items.AddRange(Enum.GetValues(typeof (RequiredGender)).Cast<object>().ToArray());
            ISetComboBox.Items.AddRange(Enum.GetValues(typeof(ItemSet)).Cast<object>().ToArray());

            ITypeFilterComboBox.Items.AddRange(Enum.GetValues(typeof(ItemType)).Cast<object>().ToArray());
            ITypeFilterComboBox.Items.Add(new ComboBoxItem { Text = "All" });
            ITypeFilterComboBox.SelectedIndex = ITypeFilterComboBox.Items.Count - 1;

            UpdateInterface();
        }

        public void RefreshUniqueTab()
        {
            if ((ITypeComboBox.SelectedItem != null) && ((ItemType)ITypeComboBox.SelectedItem == ItemType.Gem))
            {
                tabControl1.TabPages[3].Text = "Usable on";
                ParalysischeckBox.Text = "Weapon";
                TeleportcheckBox.Text = "Armour";
                ClearcheckBox.Text = "Helmet";
                ProtectioncheckBox.Text = "Necklace";
                RevivalcheckBox.Text = "Bracelet";
                MusclecheckBox.Text = "Ring";
                FlamecheckBox.Text = "Amulet";
                HealingcheckBox.Text = "Belt";
                ProbecheckBox.Text = "Boots";
                SkillcheckBox.Text = "Stone";
                NoDuraLosscheckBox.Text = "Torch";
                PickaxecheckBox.Text = "Unused";
                label50.Text = "Base rate%";
                label52.Text = "Success drop";
                label51.Text = "Max stats (all)";
                label49.Text = "Max gem stat";
                BlinkcheckBox.Text = "Unsure?";
            }
            else
            {
                tabControl1.TabPages[3].Text = "Special Stats";
                ParalysischeckBox.Text = "Paralysis ring";
                TeleportcheckBox.Text = "Teleport ring";
                ClearcheckBox.Text = "Clear ring";
                ProtectioncheckBox.Text = "Protection ring";
                RevivalcheckBox.Text = "Revival ring";
                MusclecheckBox.Text = "Muscle ring";
                FlamecheckBox.Text = "Flame ring";
                HealingcheckBox.Text = "Healing ring";
                ProbecheckBox.Text = "Probe necklace";
                SkillcheckBox.Text = "Skill necklace";
                NoDuraLosscheckBox.Text = "No dura loss";
                PickaxecheckBox.Text = "Pickaxe";
                label50.Text = "Critical rate:";
                label52.Text = "Reflect:";
                label51.Text = "Critical Dmg:";
                label49.Text = "HP Drain:";
                BlinkcheckBox.Text = "Blink";
            }
        }

        public void UpdateInterface(bool refreshList = false)
        {
            if (refreshList)
            {
                ItemInfoListBox.Items.Clear();

                for (int i = 0; i < Envir.ItemInfoList.Count; i++)
                {
                    if (ITypeFilterComboBox.SelectedItem == null ||
                        ITypeFilterComboBox.SelectedIndex == ITypeFilterComboBox.Items.Count - 1 ||
                        Envir.ItemInfoList[i].Type == (ItemType)ITypeFilterComboBox.SelectedItem)
                        ItemInfoListBox.Items.Add(Envir.ItemInfoList[i]);
                }
            }

            _selectedItemInfos = ItemInfoListBox.SelectedItems.Cast<ItemInfo>().ToList();


            if (_selectedItemInfos.Count == 0)
            {
                ItemInfoPanel.Enabled = false;

                ItemIndexTextBox.Text = string.Empty;
                ItemNameTextBox.Text = string.Empty;
                WeightTextBox.Text = string.Empty;
                ImageTextBox.Text = string.Empty;
                DuraTextBox.Text = string.Empty;
                ITypeComboBox.SelectedItem = null;
                IGradeComboBox.SelectedItem = null;
                ISetComboBox.SelectedItem = null;
                ShapeTextBox.Text = string.Empty;
                SSizeTextBox.Text = string.Empty;
                PriceTextBox.Text = string.Empty;
                RTypeComboBox.SelectedItem = null;
                RAmountTextBox.Text = string.Empty;
                RClassComboBox.SelectedItem = null;
                RGenderComboBox.SelectedItem = null;            
                LightTextBox.Text = string.Empty;
                LightIntensitytextBox.Text = string.Empty;

                MinACTextBox.Text = string.Empty;
                MaxACTextBox.Text = string.Empty;
                MinMACTextBox.Text = string.Empty;
                MaxMACTextBox.Text = string.Empty;
                MinDCTextBox.Text = string.Empty;
                MaxDCTextBox.Text = string.Empty;
                MinMCTextBox.Text = string.Empty;
                MaxMCTextBox.Text = string.Empty;
                MinSCTextBox.Text = string.Empty;
                MaxSCTextBox.Text = string.Empty;
                HPTextBox.Text = string.Empty;
                MPTextBox.Text = string.Empty;
                AccuracyTextBox.Text = string.Empty;
                AgilityTextBox.Text = string.Empty;
                ASpeedTextBox.Text = string.Empty;
                LuckTextBox.Text = string.Empty;
                StartItemCheckBox.Checked = false;

                WWeightTextBox.Text = string.Empty;
                HWeightTextBox.Text = string.Empty;
                BWeightText.Text = string.Empty;
                EffectTextBox.Text = string.Empty;
                
                PoisonRecoverytextBox.Text = string.Empty;
                SpellRecoverytextBox.Text = string.Empty;
                MagicResisttextBox.Text = string.Empty;
                HealthRecoveryTextbox.Text = string.Empty;
                StrongTextbox.Text = string.Empty;
                MacRateTextbox.Text = string.Empty;
                ACRateTextbox.Text = string.Empty;
                PoisonResisttextBox.Text = string.Empty;
                PoisonAttacktextbox.Text = string.Empty;
                Freezingtextbox.Text = string.Empty;
                Holytextbox.Text = string.Empty;
                HPratetextbox.Text = string.Empty;
                MPratetextbox.Text = string.Empty;
                HpDrainRatetextBox.Text = string.Empty;
                CriticalDamagetextBox.Text = string.Empty;
                CriticalRatetextBox.Text = string.Empty;
                ReflecttextBox.Text = string.Empty;

                LevelBasedcheckbox.Checked = false;
                ClassBasedcheckbox.Checked = false;

                Bind_dontstorecheckbox.Checked = false;
                Bind_dontupgradecheckbox.Checked = false;
                Bind_dontrepaircheckbox.Checked = false;
                Bind_donttradecheckbox.Checked = false;
                Bind_dontsellcheckbox.Checked = false;
                Bind_destroyondropcheckbox.Checked = false;
                Bind_dontdeathdropcheckbox.Checked = false;
                Bind_dontdropcheckbox.Checked = false;
                Bind_DontSpecialRepaircheckBox.Checked = false;

                NeedIdentifycheckbox.Checked = false;
                ShowGroupPickupcheckbox.Checked = false;
                globalDropNotify_CheckBox.Checked = false;
                BindOnEquipcheckbox.Checked = false;
                ParalysischeckBox.Checked = false;
                TeleportcheckBox.Checked = false;
                ClearcheckBox.Checked = false;
                ProtectioncheckBox.Checked = false;
                RevivalcheckBox.Checked = false;
                MusclecheckBox.Checked = false;
                FlamecheckBox.Checked = false;
                HealingcheckBox.Checked = false;
                ProbecheckBox.Checked = false;
                SkillcheckBox.Checked = false;
                NoDuraLosscheckBox.Checked = false;
                RandomStatstextBox.Text = string.Empty;
                PickaxecheckBox.Checked = false;
                FastRunCheckBox.Checked = false;
                CanAwaken.Checked = false;
                TooltipTextBox.Text = string.Empty;
                BlinkcheckBox.Checked = false;
                return;
            }

            ItemInfo info = _selectedItemInfos[0];

            ItemInfoPanel.Enabled = true;

            ItemIndexTextBox.Text = info.Index.ToString();
            ItemNameTextBox.Text = info.Name;
            WeightTextBox.Text = info.Weight.ToString();
            ImageTextBox.Text = info.Image.ToString();
            DuraTextBox.Text = info.Durability.ToString();
            ITypeComboBox.SelectedItem = info.Type;
            IGradeComboBox.SelectedItem = info.Grade;
            ISetComboBox.SelectedItem = info.Set;
            ShapeTextBox.Text = info.Shape.ToString();
            SSizeTextBox.Text = info.StackSize.ToString();
            PriceTextBox.Text = info.Price.ToString();
            RTypeComboBox.SelectedItem = info.RequiredType;
            RAmountTextBox.Text = info.RequiredAmount.ToString();
            RClassComboBox.SelectedItem = info.RequiredClass;
            RGenderComboBox.SelectedItem = info.RequiredGender;
            LightTextBox.Text = (info.Light % 15).ToString();
            LightIntensitytextBox.Text = (info.Light / 15).ToString();

            MinACTextBox.Text = info.MinAC.ToString();
            MaxACTextBox.Text = info.MaxAC.ToString();
            MinMACTextBox.Text = info.MinMAC.ToString();
            MaxMACTextBox.Text = info.MaxMAC.ToString();
            MinDCTextBox.Text = info.MinDC.ToString();
            MaxDCTextBox.Text = info.MaxDC.ToString();
            MinMCTextBox.Text = info.MinMC.ToString();
            MaxMCTextBox.Text = info.MaxMC.ToString();
            MinSCTextBox.Text = info.MinSC.ToString();
            MaxSCTextBox.Text = info.MaxSC.ToString();
            HPTextBox.Text = info.HP.ToString();
            MPTextBox.Text = info.MP.ToString();
            AccuracyTextBox.Text = info.Accuracy.ToString();
            AgilityTextBox.Text = info.Agility.ToString();
            ASpeedTextBox.Text = info.AttackSpeed.ToString();
            LuckTextBox.Text = info.Luck.ToString();

            WWeightTextBox.Text = info.WearWeight.ToString();
            HWeightTextBox.Text = info.HandWeight.ToString();
            BWeightText.Text = info.BagWeight.ToString();

            StartItemCheckBox.Checked = info.StartItem;
            EffectTextBox.Text = info.Effect.ToString();

            PoisonRecoverytextBox.Text = info.PoisonRecovery.ToString();
            SpellRecoverytextBox.Text = info.SpellRecovery.ToString();
            MagicResisttextBox.Text = info.MagicResist.ToString();
            HealthRecoveryTextbox.Text = info.HealthRecovery.ToString();
            StrongTextbox.Text = info.Strong.ToString();
            MacRateTextbox.Text = info.MaxMacRate.ToString();
            ACRateTextbox.Text = info.MaxAcRate.ToString();
            PoisonResisttextBox.Text = info.PoisonResist.ToString();
            PoisonAttacktextbox.Text = info.PoisonAttack.ToString();
            Freezingtextbox.Text = info.Freezing.ToString();
            Holytextbox.Text = info.Holy.ToString();
            HPratetextbox.Text = info.HPrate.ToString();
            MPratetextbox.Text = info.MPrate.ToString();
            HpDrainRatetextBox.Text = info.HpDrainRate.ToString();
            CriticalRatetextBox.Text = info.CriticalRate.ToString();
            CriticalDamagetextBox.Text = info.CriticalDamage.ToString();
            ReflecttextBox.Text = info.Reflect.ToString();

            LevelBasedcheckbox.Checked = info.LevelBased;
            ClassBasedcheckbox.Checked = info.ClassBased;

            
            Bind_dontstorecheckbox.Checked = info.Bind.HasFlag(BindMode.DontStore);
            Bind_dontupgradecheckbox.Checked = info.Bind.HasFlag(BindMode.DontUpgrade);
            Bind_dontrepaircheckbox.Checked = info.Bind.HasFlag(BindMode.DontRepair);
            Bind_donttradecheckbox.Checked = info.Bind.HasFlag(BindMode.DontTrade);
            Bind_dontsellcheckbox.Checked = info.Bind.HasFlag(BindMode.DontSell);
            Bind_destroyondropcheckbox.Checked = info.Bind.HasFlag(BindMode.DestroyOnDrop);
            Bind_dontdeathdropcheckbox.Checked = info.Bind.HasFlag(BindMode.DontDeathdrop);
            Bind_dontdropcheckbox.Checked = info.Bind.HasFlag(BindMode.DontDrop);
            Bind_DontSpecialRepaircheckBox.Checked = info.Bind.HasFlag(BindMode.NoSRepair);
            BindOnEquipcheckbox.Checked = info.Bind.HasFlag(BindMode.BindOnEquip);
            BreakOnDeathcheckbox.Checked = info.Bind.HasFlag(BindMode.BreakOnDeath);
            NoWeddingRingcheckbox.Checked = info.Bind.HasFlag(BindMode.NoWeddingRing);
            unableToRent_CheckBox.Checked = info.Bind.HasFlag(BindMode.UnableToRent);
            unableToDisassemble_CheckBox.Checked = info.Bind.HasFlag(BindMode.UnableToDisassemble);


            NeedIdentifycheckbox.Checked = info.NeedIdentify;
            ShowGroupPickupcheckbox.Checked = info.ShowGroupPickup;
            globalDropNotify_CheckBox.Checked = info.GlobalDropNotify;
            

            ParalysischeckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Paralize);
            TeleportcheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Teleport);
            ClearcheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Clearring);
            ProtectioncheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Protection);
            RevivalcheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Revival);
            MusclecheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Muscle);
            FlamecheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Flame);
            HealingcheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Healing);
            ProbecheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Probe);
            SkillcheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Skill);
            NoDuraLosscheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.NoDuraLoss);
            RandomStatstextBox.Text = info.RandomStatsId.ToString();
            PickaxecheckBox.Checked = info.CanMine;
            FastRunCheckBox.Checked = info.CanFastRun;
            CanAwaken.Checked = info.CanAwakening;
            TooltipTextBox.Text = info.ToolTip;
            BlinkcheckBox.Checked = info.Unique.HasFlag(SpecialItemMode.Blink);

            for (int i = 1; i < _selectedItemInfos.Count; i++)
            {
                info = _selectedItemInfos[i];

                if (ItemIndexTextBox.Text != info.Index.ToString()) ItemIndexTextBox.Text = string.Empty;
                if (ItemNameTextBox.Text != info.Name) ItemNameTextBox.Text = string.Empty;

                if (WeightTextBox.Text != info.Weight.ToString()) WeightTextBox.Text = string.Empty;
                if (ImageTextBox.Text != info.Image.ToString()) ImageTextBox.Text = string.Empty;
                if (DuraTextBox.Text != info.Durability.ToString()) DuraTextBox.Text = string.Empty;
                if (ITypeComboBox.SelectedItem == null || (ItemType)ITypeComboBox.SelectedItem != info.Type) ITypeComboBox.SelectedItem = null;
                if (IGradeComboBox.SelectedItem == null || (ItemGrade)IGradeComboBox.SelectedItem != info.Grade) IGradeComboBox.SelectedItem = null;
                if (ISetComboBox.SelectedItem == null || (ItemSet)ISetComboBox.SelectedItem != info.Set) ISetComboBox.SelectedItem = null;
                if (ShapeTextBox.Text != info.Shape.ToString()) ShapeTextBox.Text = string.Empty;
                if (SSizeTextBox.Text != info.StackSize.ToString()) SSizeTextBox.Text = string.Empty;
                if (PriceTextBox.Text != info.Price.ToString()) PriceTextBox.Text = string.Empty;
                if (RTypeComboBox.SelectedItem == null || (RequiredType)RTypeComboBox.SelectedItem != info.RequiredType) RTypeComboBox.SelectedItem = null;
                if (RAmountTextBox.Text != info.RequiredAmount.ToString()) RAmountTextBox.Text = string.Empty;
                if (RClassComboBox.SelectedItem == null || (RequiredClass)RClassComboBox.SelectedItem != info.RequiredClass) RClassComboBox.SelectedItem = null;
                if (RGenderComboBox.SelectedItem == null || (RequiredGender)RGenderComboBox.SelectedItem != info.RequiredGender) RGenderComboBox.SelectedItem = null;
                if (LightTextBox.Text != (info.Light % 15).ToString()) LightTextBox.Text = string.Empty;
                if (LightIntensitytextBox.Text != (info.Light / 15).ToString()) LightIntensitytextBox.Text = string.Empty;

                if (MinACTextBox.Text != info.MinAC.ToString()) MinACTextBox.Text = string.Empty;
                if (MaxACTextBox.Text != info.MaxAC.ToString()) MaxACTextBox.Text = string.Empty;
                if (MinMACTextBox.Text != info.MinMAC.ToString()) MinMACTextBox.Text = string.Empty;
                if (MaxMACTextBox.Text != info.MaxMAC.ToString()) MaxMACTextBox.Text = string.Empty;
                if (MinDCTextBox.Text != info.MinDC.ToString()) MinDCTextBox.Text = string.Empty;
                if (MaxDCTextBox.Text != info.MaxDC.ToString()) MaxDCTextBox.Text = string.Empty;
                if (MinMCTextBox.Text != info.MinMC.ToString()) MinMCTextBox.Text = string.Empty;
                if (MaxMCTextBox.Text != info.MaxMC.ToString()) MaxMCTextBox.Text = string.Empty;
                if (MinSCTextBox.Text != info.MinSC.ToString()) MinSCTextBox.Text = string.Empty;
                if (MaxSCTextBox.Text != info.MaxSC.ToString()) MaxSCTextBox.Text = string.Empty;
                if (HPTextBox.Text != info.HP.ToString()) HPTextBox.Text = string.Empty;
                if (MPTextBox.Text != info.MP.ToString()) MPTextBox.Text = string.Empty;
                if (AccuracyTextBox.Text != info.Accuracy.ToString()) AccuracyTextBox.Text = string.Empty;
                if (AgilityTextBox.Text != info.Agility.ToString()) AgilityTextBox.Text = string.Empty;
                if (ASpeedTextBox.Text != info.AttackSpeed.ToString()) ASpeedTextBox.Text = string.Empty;
                if (LuckTextBox.Text != info.Luck.ToString()) LuckTextBox.Text = string.Empty;

                if (WWeightTextBox.Text != info.WearWeight.ToString()) WWeightTextBox.Text = string.Empty;
                if (HWeightTextBox.Text != info.HandWeight.ToString()) HWeightTextBox.Text = string.Empty;
                if (BWeightText.Text != info.BagWeight.ToString()) BWeightText.Text = string.Empty;

                if (StartItemCheckBox.Checked != info.StartItem) StartItemCheckBox.CheckState = CheckState.Indeterminate;
                if (EffectTextBox.Text != info.Effect.ToString()) EffectTextBox.Text = string.Empty;

                if (PoisonRecoverytextBox.Text != info.PoisonRecovery.ToString()) PoisonRecoverytextBox.Text = string.Empty;
                if (SpellRecoverytextBox.Text != info.SpellRecovery.ToString()) SpellRecoverytextBox.Text = string.Empty;
                if (MagicResisttextBox.Text != info.MagicResist.ToString()) MagicResisttextBox.Text = string.Empty;
                if (HealthRecoveryTextbox.Text != info.HealthRecovery.ToString()) HealthRecoveryTextbox.Text = string.Empty;
                if (StrongTextbox.Text != info.Strong.ToString()) StrongTextbox.Text = string.Empty;
                if (MacRateTextbox.Text != info.MaxMacRate.ToString()) MacRateTextbox.Text = string.Empty;
                if (ACRateTextbox.Text != info.MaxAcRate.ToString()) ACRateTextbox.Text = string.Empty;
                if (PoisonResisttextBox.Text != info.PoisonResist.ToString()) PoisonResisttextBox.Text = string.Empty;
                if (PoisonAttacktextbox.Text != info.PoisonAttack.ToString()) PoisonAttacktextbox.Text = string.Empty;
                if (Freezingtextbox.Text != info.Freezing.ToString()) Freezingtextbox.Text = string.Empty;
                if (Holytextbox.Text != info.Holy.ToString()) Holytextbox.Text = string.Empty;
                if (HPratetextbox.Text != info.HPrate.ToString()) HPratetextbox.Text = string.Empty;
                if (MPratetextbox.Text != info.MPrate.ToString()) MPratetextbox.Text = string.Empty;
                if (HpDrainRatetextBox.Text != info.HpDrainRate.ToString()) HpDrainRatetextBox.Text = string.Empty;
                if (CriticalRatetextBox.Text != info.CriticalRate.ToString()) CriticalRatetextBox.Text = string.Empty;
                if (CriticalDamagetextBox.Text != info.CriticalDamage.ToString()) CriticalDamagetextBox.Text = string.Empty;
                if (ReflecttextBox.Text != info.Reflect.ToString()) ReflecttextBox.Text = string.Empty;
                if (LevelBasedcheckbox.Checked != info.LevelBased) LevelBasedcheckbox.CheckState = CheckState.Indeterminate;
                if (ClassBasedcheckbox.Checked != info.ClassBased) ClassBasedcheckbox.CheckState = CheckState.Indeterminate;
                if (Bind_dontstorecheckbox.Checked != info.Bind.HasFlag(BindMode.DontStore)) Bind_dontstorecheckbox.CheckState = CheckState.Indeterminate;
                if (Bind_dontupgradecheckbox.Checked != info.Bind.HasFlag(BindMode.DontUpgrade)) Bind_dontupgradecheckbox.CheckState = CheckState.Indeterminate;
                if (Bind_dontrepaircheckbox.Checked != info.Bind.HasFlag(BindMode.DontRepair)) Bind_dontrepaircheckbox.CheckState = CheckState.Indeterminate;
                if (Bind_donttradecheckbox.Checked != info.Bind.HasFlag(BindMode.DontTrade)) Bind_donttradecheckbox.CheckState = CheckState.Indeterminate;
                if (Bind_dontsellcheckbox.Checked != info.Bind.HasFlag(BindMode.DontSell)) Bind_dontsellcheckbox.CheckState = CheckState.Indeterminate;
                if (Bind_destroyondropcheckbox.Checked != info.Bind.HasFlag(BindMode.DestroyOnDrop)) Bind_destroyondropcheckbox.CheckState = CheckState.Indeterminate;
                if (Bind_dontdeathdropcheckbox.Checked != info.Bind.HasFlag(BindMode.DontDeathdrop)) Bind_dontdeathdropcheckbox.CheckState = CheckState.Indeterminate;
                if (Bind_dontdropcheckbox.Checked != info.Bind.HasFlag(BindMode.DontDrop)) Bind_dontdropcheckbox.CheckState = CheckState.Indeterminate;
                if (Bind_DontSpecialRepaircheckBox.Checked != info.Bind.HasFlag(BindMode.NoSRepair)) Bind_DontSpecialRepaircheckBox.CheckState = CheckState.Indeterminate;
                if (BindOnEquipcheckbox.Checked != info.Bind.HasFlag(BindMode.BindOnEquip)) BindOnEquipcheckbox.CheckState = CheckState.Indeterminate;
                if (BreakOnDeathcheckbox.Checked != info.Bind.HasFlag(BindMode.BreakOnDeath)) BreakOnDeathcheckbox.CheckState = CheckState.Indeterminate;
                if (NoWeddingRingcheckbox.Checked != info.Bind.HasFlag(BindMode.NoWeddingRing)) NoWeddingRingcheckbox.CheckState = CheckState.Indeterminate;

                if (unableToRent_CheckBox.Checked != info.Bind.HasFlag(BindMode.UnableToRent))
                    unableToRent_CheckBox.CheckState = CheckState.Indeterminate;

                if (unableToDisassemble_CheckBox.Checked != info.Bind.HasFlag(BindMode.UnableToDisassemble))
                    unableToDisassemble_CheckBox.CheckState = CheckState.Indeterminate;

                if (NeedIdentifycheckbox.Checked != info.NeedIdentify) NeedIdentifycheckbox.CheckState = CheckState.Indeterminate;
                if (ShowGroupPickupcheckbox.Checked != info.ShowGroupPickup) ShowGroupPickupcheckbox.CheckState = CheckState.Indeterminate;
                if (globalDropNotify_CheckBox.Checked != info.GlobalDropNotify)
                    globalDropNotify_CheckBox.CheckState = CheckState.Indeterminate;

                if (ParalysischeckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Paralize)) ParalysischeckBox.CheckState = CheckState.Indeterminate;
                if (TeleportcheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Teleport)) TeleportcheckBox.CheckState = CheckState.Indeterminate;
                if (ClearcheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Clearring)) ClearcheckBox.CheckState = CheckState.Indeterminate;
                if (ProtectioncheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Protection)) ProtectioncheckBox.CheckState = CheckState.Indeterminate;
                if (RevivalcheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Revival)) RevivalcheckBox.CheckState = CheckState.Indeterminate;
                if (MusclecheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Muscle)) MusclecheckBox.CheckState = CheckState.Indeterminate;
                if (FlamecheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Flame)) FlamecheckBox.CheckState = CheckState.Indeterminate;
                if (HealingcheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Healing)) HealingcheckBox.CheckState = CheckState.Indeterminate;
                if (ProbecheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Probe)) ProbecheckBox.CheckState = CheckState.Indeterminate;
                if (SkillcheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Skill)) SkillcheckBox.CheckState = CheckState.Indeterminate;
                if (NoDuraLosscheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.NoDuraLoss)) NoDuraLosscheckBox.CheckState = CheckState.Indeterminate;
                if (RandomStatstextBox.Text != info.RandomStatsId.ToString()) RandomStatstextBox.Text = string.Empty;
                if (PickaxecheckBox.Checked != info.CanMine) PickaxecheckBox.CheckState = CheckState.Indeterminate;
                if (FastRunCheckBox.Checked != info.CanFastRun) FastRunCheckBox.CheckState = CheckState.Indeterminate;
                if (CanAwaken.Checked != info.CanAwakening) CanAwaken.CheckState = CheckState.Indeterminate;
                if (TooltipTextBox.Text != info.ToolTip) TooltipTextBox.Text = string.Empty;
                if (BlinkcheckBox.Checked != info.Unique.HasFlag(SpecialItemMode.Blink)) BlinkcheckBox.CheckState = CheckState.Indeterminate;
        }
            RefreshUniqueTab();
        }

        private void RefreshItemList()
        {
            ItemInfoListBox.SelectedIndexChanged -= ItemInfoListBox_SelectedIndexChanged;

            List<bool> selected = new List<bool>();

            for (int i = 0; i < ItemInfoListBox.Items.Count; i++) selected.Add(ItemInfoListBox.GetSelected(i));
            ItemInfoListBox.Items.Clear();
            for (int i = 0; i < Envir.ItemInfoList.Count; i++)
            {
                if (ITypeFilterComboBox.SelectedItem == null ||
                    ITypeFilterComboBox.SelectedIndex == ITypeFilterComboBox.Items.Count - 1 ||
                    Envir.ItemInfoList[i].Type == (ItemType)ITypeFilterComboBox.SelectedItem)
                    ItemInfoListBox.Items.Add(Envir.ItemInfoList[i]);
            };
            for (int i = 0; i < selected.Count; i++) ItemInfoListBox.SetSelected(i, selected[i]);

            ItemInfoListBox.SelectedIndexChanged += ItemInfoListBox_SelectedIndexChanged;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (ITypeFilterComboBox.SelectedIndex == ITypeFilterComboBox.Items.Count - 1)
            {
                Envir.CreateItemInfo();
                ITypeFilterComboBox.SelectedIndex = ITypeFilterComboBox.Items.Count - 1;
            }
            else
            {
                Envir.CreateItemInfo((ItemType)ITypeFilterComboBox.SelectedItem);
            }

            UpdateInterface(true);
        }

        private void ItemInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        private void ITypeFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface(true);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (_selectedItemInfos.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected Items?", "Remove Items?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++) Envir.Remove(_selectedItemInfos[i]);

            if (Envir.ItemInfoList.Count == 0) Envir.ItemIndex = 0;

            UpdateInterface(true);
        }
        private void ItemNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Name = ActiveControl.Text;

            RefreshItemList();
        }
        private void ITypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Type = (ItemType)ITypeComboBox.SelectedItem;
        }
        private void RTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].RequiredType = (RequiredType) RTypeComboBox.SelectedItem;
        }
        private void RGenderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].RequiredGender = (RequiredGender)RGenderComboBox.SelectedItem;
        }
        private void RClassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].RequiredClass = (RequiredClass)RClassComboBox.SelectedItem;
        }
        private void StartItemCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].StartItem = StartItemCheckBox.Checked;
        }
        private void WeightTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Weight = temp;
        }
        private void ImageTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Image = temp;
        }
        private void DuraTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Durability = temp;
        }
        private void ShapeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            short temp;

            if (!short.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Shape = temp;
        }
        private void SSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].StackSize = temp;
        }
        private void PriceTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Price = temp;
        }
        private void RAmountTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].RequiredAmount = temp;
        }
        private void LightTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            if (temp > 14)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            
            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Light = (byte)(temp + (_selectedItemInfos[i].Light / 15)*15);
        }
        private void MinACTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MinAC = temp;
        }
        private void MaxACTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MaxAC = temp;
        }
        private void MinMACTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MinMAC = temp;
        }
        private void MaxMACTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MaxMAC = temp;
        }
        private void MinDCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MinDC = temp;
        }
        private void MaxDCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MaxDC = temp;
        }
        private void MinMCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MinMC = temp;
        }
        private void MaxMCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MaxMC = temp;
        }
        private void MinSCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MinSC = temp;
        }
        private void MaxSCTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MaxSC = temp;
        }
        private void HPTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].HP = temp;
        }
        private void MPTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MP = temp;
        }
        private void AccuracyTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Accuracy = temp;
        }
        private void AgilityTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Agility = temp;
        }
        private void ASpeedTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            sbyte temp;

            if (!sbyte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].AttackSpeed = temp;
        }
        private void LuckTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            sbyte temp;

            if (!sbyte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Luck = temp;
        }
        private void BWeightText_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].BagWeight = temp;
        }
        private void HWeightTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].HandWeight = temp;
        }
        private void WWeightTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].WearWeight = temp;
        }
        private void EffectTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Effect = temp;
        }

        private void ItemInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Envir.SaveDB();
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            string data = Clipboard.GetText();

            if (!data.StartsWith("Item", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Cannot Paste, Copied data is not Item Information.");
                return;
            }


            string[] items = data.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);


            for (int i = 1; i < items.Length; i++)
            {
                ItemInfo info = ItemInfo.FromText(items[i]);

                if (info == null) continue;
                info.Index = ++Envir.ItemIndex;
                Envir.ItemInfoList.Add(info);

            }

            UpdateInterface();
        }

        private void CopyMButton_Click(object sender, EventArgs e)
        {

        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            ExportItems(Envir.ItemInfoList);
        }

        private void ExportSelectedButton_Click(object sender, EventArgs e)
        {
            var list = ItemInfoListBox.SelectedItems.Cast<ItemInfo>().ToList();

            ExportItems(list);
        }

        private void ExportItems(IEnumerable<ItemInfo> items)
        {
            var itemInfos = items as ItemInfo[] ?? items.ToArray();
            var list = itemInfos.Select(item => item.ToText()).ToList();

            File.WriteAllLines(ItemListPath, list);

            MessageBox.Show(itemInfos.Count() + " Items have been exported");
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            string Path = string.Empty;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File|*.txt";
            ofd.ShowDialog();

            if (ofd.FileName == string.Empty) return;

            Path = ofd.FileName;

            string data;
            using (var sr = new StreamReader(Path))
            {
                data = sr.ReadToEnd();
            }

            var items = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var count = 0;
            foreach (var info in items.Select(ItemInfo.FromText).Where(info => info != null))
            {
                count++;
                info.Index = ++Envir.ItemIndex;
                Envir.ItemInfoList.Add(info);
            }

            MessageBox.Show(count + " Items have been imported");
            UpdateInterface(true);
        }

        private void ISetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Set = (ItemSet)ISetComboBox.SelectedItem;
        }

        private void ACRateTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MaxAcRate = temp;
        }

        private void MacRateTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MaxMacRate = temp;
        }

        private void MagicResisttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MagicResist = temp;
        }

        private void PoisonResisttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].PoisonResist = temp;
        }

        private void HealthRecoveryTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].HealthRecovery = temp;
        }

        private void SpellRecoverytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].SpellRecovery = temp;
        }

        private void PoisonRecoverytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].PoisonRecovery = temp;
        }

        private void HporMpRatetextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].HPrate = temp;
        }

        private void Holytextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Holy = temp;
        }

        private void Freezingtextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Freezing = temp;
        }

        private void PoisonAttacktextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].PoisonAttack = temp;
        }

        private void ClassBasedcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].ClassBased = ClassBasedcheckbox.Checked;
        }

        private void LevelBasedcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].LevelBased = LevelBasedcheckbox.Checked;
        }

        private void Bind_dontdropcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (Bind_dontdropcheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.DontDrop : _selectedItemInfos[i].Bind ^= BindMode.DontDrop);
        }

        private void Bind_dontdeathdropcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (Bind_dontdeathdropcheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.DontDeathdrop : _selectedItemInfos[i].Bind ^= BindMode.DontDeathdrop);
        }

        private void Bind_destroyondropcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (Bind_destroyondropcheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.DestroyOnDrop : _selectedItemInfos[i].Bind ^= BindMode.DestroyOnDrop);
        }

        private void Bind_dontsellcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (Bind_dontsellcheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.DontSell : _selectedItemInfos[i].Bind ^= BindMode.DontSell);
        }

        private void Bind_donttradecheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (Bind_donttradecheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.DontTrade : _selectedItemInfos[i].Bind ^= BindMode.DontTrade);
        }

        private void Bind_dontrepaircheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (Bind_dontrepaircheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.DontRepair : _selectedItemInfos[i].Bind ^= BindMode.DontRepair);
        }

        private void Bind_dontstorecheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (Bind_dontstorecheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.DontStore : _selectedItemInfos[i].Bind ^= BindMode.DontStore);
        }

        private void Bind_dontupgradecheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (Bind_dontupgradecheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.DontUpgrade : _selectedItemInfos[i].Bind ^= BindMode.DontUpgrade);
        }

        private void NeedIdentifycheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].NeedIdentify = NeedIdentifycheckbox.Checked;
        }

        private void ShowGroupPickupcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].ShowGroupPickup = ShowGroupPickupcheckbox.Checked;
        }

        private void BindOnEquipcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (BindOnEquipcheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.BindOnEquip : _selectedItemInfos[i].Bind ^= BindMode.BindOnEquip);
        }

        private void MPratetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].MPrate = temp;
        }

        private void HpDrainRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].HpDrainRate = temp;
        }


        private void ParalysischeckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (ParalysischeckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Paralize : _selectedItemInfos[i].Unique ^= SpecialItemMode.Paralize);
        }

        private void TeleportcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (TeleportcheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Teleport : _selectedItemInfos[i].Unique ^= SpecialItemMode.Teleport);
        }

        private void ClearcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (ClearcheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Clearring : _selectedItemInfos[i].Unique ^= SpecialItemMode.Clearring);
        }

        private void ProtectioncheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (ProtectioncheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Protection : _selectedItemInfos[i].Unique ^= SpecialItemMode.Protection);
        }

        private void RevivalcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (RevivalcheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Revival : _selectedItemInfos[i].Unique ^= SpecialItemMode.Revival);
        }

        private void MusclecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (MusclecheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Muscle : _selectedItemInfos[i].Unique ^= SpecialItemMode.Muscle);
        }

        private void FlamecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (FlamecheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Flame : _selectedItemInfos[i].Unique ^= SpecialItemMode.Flame);
        }

        private void HealingcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (HealingcheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Healing : _selectedItemInfos[i].Unique ^= SpecialItemMode.Healing);
        }

        private void ProbecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (ProbecheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Probe : _selectedItemInfos[i].Unique ^= SpecialItemMode.Probe);
        }

        private void SkillcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (SkillcheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Skill : _selectedItemInfos[i].Unique ^= SpecialItemMode.Skill);
        }

        private void NoDuraLosscheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (NoDuraLosscheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.NoDuraLoss : _selectedItemInfos[i].Unique ^= SpecialItemMode.NoDuraLoss);
        }

        private void StrongTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Strong = temp;
        }

        private void CriticalRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].CriticalRate = temp;
        }

        private void CriticalDamagetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].CriticalDamage = temp;
        }

        private void ReflecttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Reflect = temp;
        }

        private void Bind_DontSpecialRepaircheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (Bind_DontSpecialRepaircheckBox.Checked ? _selectedItemInfos[i].Bind |= BindMode.NoSRepair : _selectedItemInfos[i].Bind ^= BindMode.NoSRepair);
        }

        private void BlinkcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Unique = (BlinkcheckBox.Checked ? _selectedItemInfos[i].Unique |= SpecialItemMode.Blink : _selectedItemInfos[i].Unique ^= SpecialItemMode.Blink);
        }

        private void LightIntensitytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            if (temp > 4)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Light = (byte)((_selectedItemInfos[i].Light % 15) + (15 * temp));
        }

        private void RandomStatstextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            if ((temp >= Settings.RandomItemStatsList.Count) && (temp != 255))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedItemInfos.Count; i++)
            {
                _selectedItemInfos[i].RandomStatsId = temp;
                if (temp != 255)
                    _selectedItemInfos[i].RandomStats = Settings.RandomItemStatsList[temp];
                else
                    _selectedItemInfos[i].RandomStats = null;
            }
        }

        private void PickaxecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].CanMine = PickaxecheckBox.Checked;
        }

        private void IGradeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Grade = (ItemGrade)IGradeComboBox.SelectedItem;
        }

        private void FastRunCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].CanFastRun = FastRunCheckBox.Checked;
        }

        private void TooltipTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].ToolTip = TooltipTextBox.Text;
        }

        private void CanAwakening_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].CanAwakening = CanAwaken.Checked;
        }

        private void BreakOnDeathcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (BreakOnDeathcheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.BreakOnDeath : _selectedItemInfos[i].Bind ^= BindMode.BreakOnDeath);
        }

        private void ItemInfoForm_Load(object sender, EventArgs e)
        {

        }

        private void Gameshop_button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _selectedItemInfos.Count; i++)
                Envir.AddToGameShop(_selectedItemInfos[i]);
            Envir.SaveDB();
        }

        private void NoWeddingRingcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            for (int i = 0; i < _selectedItemInfos.Count; i++)
                _selectedItemInfos[i].Bind = (NoWeddingRingcheckbox.Checked ? _selectedItemInfos[i].Bind |= BindMode.NoWeddingRing : _selectedItemInfos[i].Bind ^= BindMode.NoWeddingRing);
        }

        private void unableToRent_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender)
                return;

            foreach (var selectedItem in _selectedItemInfos)
                selectedItem.Bind = unableToRent_CheckBox.Checked
                    ? selectedItem.Bind |= BindMode.UnableToRent
                    : selectedItem.Bind ^= BindMode.UnableToRent;
        }

        private void unableToDisassemble_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender)
                return;

            foreach (var selectedItem in _selectedItemInfos)
                selectedItem.Bind = unableToDisassemble_CheckBox.Checked
                    ? selectedItem.Bind |= BindMode.UnableToDisassemble
                    : selectedItem.Bind ^= BindMode.UnableToDisassemble;
        }

        private void globalDropNotify_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender)
                return;

            foreach (var itemInfo in _selectedItemInfos)
                itemInfo.GlobalDropNotify = globalDropNotify_CheckBox.Checked;
        }
    }
}
