using System;
using System.Collections.Generic;
using System.Text;
using Server.MirEnvir;

namespace Server.MirObjects
{
    public class PlayerFunctions
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        //Getters
        public Func<int> getLevel;
        public Func<Stats> getStats;
        public Func<MirClass> getClass;
        public Func<int> getCurrentBagWeight;
        public Func<UserItem[]> getInventory;
        public Func<bool> getDead;
        public Func<bool> getCanRegen;
        public Func<long> getRegenTime;
        public Func<long> getRegenDelay;
        public Func<int> getHP;
        public Func<int> getMP;
        public Func<long> getPotTime;
        public Func<long> getPotDelay;
        public Func<ushort> getPotHealthAmount;
        public Func<ushort> getPotManaAmount;
        public Func<long> getHealTime;
        public Func<long> getHealDelay;
        public Func<ushort> getHealAmount;
        public Func<long> getVampTime;
        public Func<long> getVampDelay;
        public Func<ushort> getVampAmount;

        //Setters
        public Action<long> setMaxExperience;
        public Action<int> setCurrentBagWeight;
        public Action<long> setRegenTime;
        public Action<long> setPotTime;
        public Action<ushort> setPotHealthAmount;
        public Action<ushort> setPotManaAmount;
        public Action<long> setHealTime;
        public Action<ushort> setHealAmount;
        public Action<long> setVampTime;
        public Action<ushort> setVampAmount;

        //Functions
        public Action<int> ChangeHP;
        public Action<int> ChangeMP;
        public Action<DamageType, int> BroadcastDamageIndicator;
        

        private Stats Stats { get => getStats(); }
        private long MaxExperience { set => setMaxExperience(value); }
        private int Level { get => getLevel(); }
        private MirClass Class { get => getClass(); }
        private int CurrentBagWeight { get => getCurrentBagWeight(); set => setCurrentBagWeight(value); }
        private UserItem[] Inventory { get => getInventory(); }
        private bool Dead { get => getDead(); }
        private bool CanRegen { get => getCanRegen(); }
        private long RegenTime { get => getRegenTime(); set => setRegenTime(value); }
        private long RegenDelay { get => getRegenDelay(); }
        private int HP { get => getHP(); }
        private int MP { get => getMP(); }
        private long PotTime { get => getPotTime(); set => setPotTime(value); }
        private long PotDelay { get => getPotDelay(); }
        private ushort PotHealthAmount { get => getPotHealthAmount(); set => setPotHealthAmount(value); }
        private ushort PotManaAmount { get => getPotManaAmount(); set => setPotManaAmount(value); }
        private long HealTime { get => getHealTime(); set => setHealTime(value); }
        private long HealDelay { get => getHealDelay(); }
        private ushort HealAmount { get => getHealAmount(); set => setHealAmount(value); }
        private long VampTime { get => getVampTime(); set => setVampTime(value); }
        private long VampDelay { get => getVampDelay(); }
        private ushort VampAmount { get => getVampAmount(); set => setVampAmount(value); }

        public PlayerFunctions() { }

        public void RefreshLevelStats()
        {
            MaxExperience = Level < Settings.ExperienceList.Count ? Settings.ExperienceList[Level - 1] : 0;

            foreach (var stat in Settings.ClassBaseStats[(byte)Class].Stats)
            {
                Stats[stat.Type] = stat.Calculate(Class, Level);
            }
        }

        public void RefreshBagWeight()
        {
            CurrentBagWeight = 0;

            for (int i = 0; i < Inventory.Length; i++)
            {
                UserItem item = Inventory[i];
                if (item != null)
                {
                    CurrentBagWeight += item.Weight;
                }
            }
        }

        public void ProcessRegen()
        {
            if (Dead) return;

            int healthRegen = 0, manaRegen = 0;

            if (CanRegen)
            {
                RegenTime = Envir.Time + RegenDelay;

                if (HP < Stats[Stat.HP])
                {
                    healthRegen += (int)(Stats[Stat.HP] * 0.03F) + 1;
                    healthRegen += (int)(healthRegen * ((double)Stats[Stat.HealthRecovery] / Settings.HealthRegenWeight));
                }

                if (MP < Stats[Stat.MP])
                {
                    manaRegen += (int)(Stats[Stat.MP] * 0.03F) + 1;
                    manaRegen += (int)(manaRegen * ((double)Stats[Stat.SpellRecovery] / Settings.ManaRegenWeight));
                }
            }

            if (Envir.Time > PotTime)
            {
                //PotTime = Envir.Time + Math.Max(50,Math.Min(PotDelay, 600 - (Level * 10)));
                PotTime = Envir.Time + PotDelay;
                int PerTickRegen = 5 + (Level / 10);

                if (PotHealthAmount > PerTickRegen)
                {
                    healthRegen += PerTickRegen;
                    PotHealthAmount -= (ushort)PerTickRegen;
                }
                else
                {
                    healthRegen += PotHealthAmount;
                    PotHealthAmount = 0;
                }

                if (PotManaAmount > PerTickRegen)
                {
                    manaRegen += PerTickRegen;
                    PotManaAmount -= (ushort)PerTickRegen;
                }
                else
                {
                    manaRegen += PotManaAmount;
                    PotManaAmount = 0;
                }
            }

            if (Envir.Time > HealTime)
            {
                HealTime = Envir.Time + HealDelay;

                int incHeal = (Level / 10) + (HealAmount / 10);
                if (HealAmount > (5 + incHeal))
                {
                    healthRegen += (5 + incHeal);
                    HealAmount -= (ushort)Math.Min(HealAmount, 5 + incHeal);
                }
                else
                {
                    healthRegen += HealAmount;
                    HealAmount = 0;
                }
            }

            if (Envir.Time > VampTime)
            {
                VampTime = Envir.Time + VampDelay;

                if (VampAmount > 10)
                {
                    healthRegen += 10;
                    VampAmount -= 10;
                }
                else
                {
                    healthRegen += VampAmount;
                    VampAmount = 0;
                }
            }

            if (healthRegen > 0)
            {
                ChangeHP(healthRegen);
                BroadcastDamageIndicator(DamageType.Hit, healthRegen);
            }

            if (HP == Stats[Stat.HP])
            {
                PotHealthAmount = 0;
                HealAmount = 0;
            }

            if (manaRegen > 0) ChangeMP(manaRegen);
            if (MP == Stats[Stat.MP]) PotManaAmount = 0;
        }
    }
}
