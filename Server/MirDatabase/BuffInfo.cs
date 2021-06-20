using System;
using System.Collections.Generic;
using System.Text;

namespace Server.MirDatabase
{
    public class BuffInfo
    {
        public BuffType Type { get; set; }
        public BuffStackType StackType { get; set; }
        public BuffProperty Properties { get; set; }
        public int Icon { get; set; }
        public bool Visible { get; set; }

        public static List<BuffInfo> Load()
        {
            List<BuffInfo> info = new List<BuffInfo>
            {
                new BuffInfo { Type = BuffType.TemporalFlux, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.Hiding, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.Haste, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.SwiftFeet, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.Fury, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.SoulShield, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.BlessedArmour, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.LightBody, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.UltimateEnhancer, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.ProtectionField, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.Rage, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.Curse, Properties = BuffProperty.RemoveOnDeath | BuffProperty.Debuff, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.MoonLight, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.DarkBody, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.Concentration, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.VampireShot, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.PoisonShot, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.CounterAttack, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.MentalState, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.EnergyShield, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.MagicBooster, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.PetEnhancer, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.ImmortalSkin, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.MagicShield, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.ElementalBarrier, Properties = BuffProperty.None, StackType = BuffStackType.Reset },

                new BuffInfo { Type = BuffType.HornedArcherBuff, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.ColdArcherBuff, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.GeneralMeowMeowShield, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.RhinoPriestDebuff, Properties = BuffProperty.Debuff, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.PowerBeadBuff, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.HornedWarriorShield, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },
                new BuffInfo { Type = BuffType.HornedCommanderShield, Properties = BuffProperty.None, StackType = BuffStackType.Reset, Visible = true },

                new BuffInfo { Type = BuffType.GameMaster, Properties = BuffProperty.None, StackType = BuffStackType.Infinite, Visible = Settings.GameMasterEffect },
                new BuffInfo { Type = BuffType.General, Properties = BuffProperty.None, StackType = BuffStackType.None },
                new BuffInfo { Type = BuffType.Exp, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.Drop, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.Gold, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.BagWeight, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.Transform, Properties = BuffProperty.None, StackType = BuffStackType.None },
                new BuffInfo { Type = BuffType.RelationshipEXP, Properties = BuffProperty.None, StackType = BuffStackType.None },
                new BuffInfo { Type = BuffType.Mentee, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.Mentor, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.Guild, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.Prison, Properties = BuffProperty.None, StackType = BuffStackType.None },
                new BuffInfo { Type = BuffType.Rested, Properties = BuffProperty.None, StackType = BuffStackType.Reset },
                new BuffInfo { Type = BuffType.Skill, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.ClearRing, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },

                new BuffInfo { Type = BuffType.Impact, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.Magic, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.Taoist, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.Storm, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.HealthAid, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.ManaAid, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.Defence, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.MagicDefence, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.WonderDrug, Properties = BuffProperty.None, StackType = BuffStackType.Duration },
                new BuffInfo { Type = BuffType.Knapsack, Properties = BuffProperty.None, StackType = BuffStackType.Duration }
            };

            return info;
        }
    }
}
