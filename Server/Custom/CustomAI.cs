using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.MirEnvir;

namespace Server.Custom
{
    public class CustomAI
    {
        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        #region Defaults
        //Defaults
        private int respawnDay = 0;
        private int respawnMonth = 0;
        private int respawnYear = 0;
        private int respawnHour = 0;
        private int respawnMinute = 0;
        private int lastKillDay = 0;
        private int lastKillMonth = 0;
        private int lastKillYear = 0;
        private int lastKillHour = 0;
        private int lastKillMinute = 0;

        private int itemCount = 0;
        private int massAttackEffect = 0;
        private int specialAttackEffect = 0;
        private int meleeAttackEffect = 0;
        private int rangeAttackEffect = 0;
        private int magicAttackEffect = 0;
        private int mapX = 0;
        private int mapY = 0;
        private int meleeAttackDamage = 100; // 100 Melee damage by default
        private int rangeAttackDamage = 0;
        private int magicAttackDamage = 0;
        private int massAttackDamage = 0;
        private int specialAttackDamage = 0;
        private int targetAttackDamage = 0;
        private int petAttackDamage = 0;
        private int targetClass = 5; //None by default     



        private string killTimer = ""; // The time it was last killed
        private string name = ""; // Mobs name        
        private string mapName = ""; // File name
        private string spawnMessage = "";
        private string deathMessage = "";
        private string itemMessage = "";

        private bool setXY = false; // Set map + X Y drop
        private bool mapSpecific = false; // Specific Map required for system to work.
        private bool announceSpawn = false; // Announcment for when spawning.
        private bool announceDrop = false; // Announce the items dropped from the DropItemsWithAnnounce
        private bool announceDeath = false; // Announce the death
        private bool useMassAttack = false; // Use Mass Attack?
        private bool useSpecialAttack = false; // Use Special Attack?
        private bool useMeleeAttack = true; // Melee by default
        private bool useRangeAttack = false; // Use Range Attack?
        private bool useMagicAttack = false; // Use Magic Attack?
        private bool targetTarget = false; // Target priority
        private bool useKillTimer = false; // Death timer
        private bool ignorePets = false; // Ignore pets
        private bool damagePetMore = false; // More damage vs pets
        private bool canPara = false; // Can Para
        private bool canGreenPoison = false; // Can green
        private bool canRedPoison = false; // Can red
        private bool spawnSlaves = false; // Can Spawn Slaves
        //Status
        private bool alive = false; // Alive || Dead
        private int currentX = 0;
        private int currentY = 0;
        private string currentMap = "";
        #endregion

        #region Getters & Setters
        public bool Alive { get { return alive; } set { alive = value; } }
        public int CurrentX { get { return currentX; } set { currentX = value; } }
        public int CurrentY { get { return currentY; } set { currentY = value; } }
        public string CurrentMap { get { return currentMap; } set { currentMap = value; } }

        // Getters and Setters.
        public int RespawnDay { get { return respawnDay; } set { respawnDay = value; } }
        public int RespawnMonth { get { return respawnMonth; } set { respawnMonth = value; } }
        public int RespawnYear { get { return respawnYear; } set { respawnYear = value; } }
        public int RespawnHour { get { return respawnHour; } set { respawnHour = value; } }
        public int RespawnMinute { get { return respawnMinute; } set { respawnMinute = value; } }
        public int LastKillDay { get { return lastKillDay; } set { lastKillDay = value; } }
        public int LastKillMonth { get { return lastKillMonth; } set { lastKillMonth = value; } }
        public int LastKillYear { get { return lastKillYear; } set { lastKillYear = value; } }
        public int LastKillMinute { get { return lastKillMinute; } set { lastKillMinute = value; } }
        public int LastKillHour { get { return lastKillHour; } set { lastKillHour = value; } }

        public int ItemCount { get { return itemCount; } set { itemCount = value; } }
        public int MassAttackEffect { get { return massAttackEffect; } set { massAttackEffect = value; } }
        public int SpecialAttackEffect { get { return specialAttackEffect; } set { specialAttackEffect = value; } }
        public int MeleeAttackEffect { get { return meleeAttackEffect; } set { meleeAttackEffect = value; } }
        public int RangeAttackEffect { get { return rangeAttackEffect; } set { rangeAttackEffect = value; } }
        public int MagicAttackEffect { get { return magicAttackEffect; } set { magicAttackEffect = value; } }
        public int MapX { get { return mapX; } set { mapX = value; } }
        public int MapY { get { return mapY; } set { mapY = value; } }
        public int MeleeAttackDamage { get { return meleeAttackDamage; } set { meleeAttackDamage = value; } }
        public int MagicAttackDamage { get { return magicAttackDamage; } set { magicAttackDamage = value; } }
        public int RangeAttackDamage { get { return rangeAttackDamage; } set { rangeAttackDamage = value; } }
        public int MassAttackDamage { get { return massAttackDamage; } set { massAttackDamage = value; } }
        public int SpecialAttackDamage { get { return specialAttackDamage; } set { specialAttackDamage = value; } }
        public int TargetAttackDamage { get { return targetAttackDamage; } set { targetAttackDamage = value; } }
        public int PetAttackDamage { get { return petAttackDamage; } set { petAttackDamage = value; } }
        public int TargetClass { get { return targetClass; } set { targetClass = value; } }

        public string Name { get { return name; } set { name = value; } }
        public string KillTimer { get { return killTimer; } set { killTimer = value; } }
        public string MapName { get { return mapName; } set { mapName = value; } }
        public string SpawnMessage { get { return spawnMessage; } set { spawnMessage = value; } }
        public string DeadMessage { get { return deathMessage; } set { deathMessage = value; } }
        public string ItemMessage { get { return itemMessage; } set { itemMessage = value; } }


        public bool SetXY { get { return setXY; } set { setXY = value; } }
        public bool MapSpecific { get { return mapSpecific; } set { mapSpecific = value; } }
        public bool AnnounceSpawn { get { return announceSpawn; } set { announceSpawn = value; } }
        public bool AnnounceDrop { get { return announceDrop; } set { announceDrop = value; } }
        public bool AnnounceDeath { get { return announceDeath; } set { announceDeath = value; } }
        public bool UseMassAttack { get { return useMassAttack; } set { useMassAttack = value; } }
        public bool UseSpecialAttack { get { return useSpecialAttack; } set { useSpecialAttack = value; } }
        public bool UseMeleeAttack { get { return useMeleeAttack; } set { useMeleeAttack = value; } }
        public bool UseRangeAttack { get { return useRangeAttack; } set { useRangeAttack = value; } }
        public bool UseMagicAttack { get { return useMagicAttack; } set { useMagicAttack = value; } }
        public bool Target { get { return targetTarget; } set { targetTarget = value; } }
        public bool UseKillTimer { get { return useKillTimer; } set { useKillTimer = value; } }
        public bool IgnorePets { get { return ignorePets; } set { ignorePets = value; } }
        public bool DamagePetsMore { get { return damagePetMore; } set { damagePetMore = value; } }
        public bool CanPara { get { return canPara; } set { canPara = value; } }
        public bool CanGreen { get { return canGreenPoison; } set { canGreenPoison = value; } }
        public bool CanRed { get { return canRedPoison; } set { canRedPoison = value; } }
        public bool Spawn_Slaves { get { return spawnSlaves; } set { spawnSlaves = value; } }
        #endregion

        #region Lists
        public List<SpawnSlaves> Slaves = new List<SpawnSlaves>(); // MobName Count Maximum 5 different
        public List<DropItemsWithAnnounce> Drops = new List<DropItemsWithAnnounce>(); // ItemName Chance
        #endregion

        public CustomAI() { }

        public CustomAI LoadCustomAI(string mobName)
        {
            if (mobName.Length <= 0) return null;
            try
            {
                InIReader Reader = new InIReader(@".\Custom\Unique AIs\" + mobName + ".ini");
                CustomAI mobAI = new CustomAI();
                mobAI.Name = mobName;

                #region Loading the Bools
                mobAI.SetXY = Reader.ReadBoolean("Bools", "SetXY", mobAI.SetXY);
                mobAI.MapSpecific = Reader.ReadBoolean("Bools", "MapSpecific", mobAI.MapSpecific);
                mobAI.AnnounceSpawn = Reader.ReadBoolean("Bools", "AnnounceSpawn", mobAI.AnnounceSpawn);
                mobAI.AnnounceDrop = Reader.ReadBoolean("Bools", "AnnounceDrop", mobAI.AnnounceDrop);
                mobAI.AnnounceDeath = Reader.ReadBoolean("Bools", "AnnounceDeath", mobAI.AnnounceDeath);
                mobAI.UseMassAttack = Reader.ReadBoolean("Bools", "UseMassAttack", mobAI.UseMassAttack);
                mobAI.UseSpecialAttack = Reader.ReadBoolean("Bools", "UseSpecialAttack", mobAI.UseSpecialAttack);
                mobAI.UseMeleeAttack = Reader.ReadBoolean("Bools", "UseMeleeAttack", mobAI.UseMeleeAttack);
                mobAI.UseRangeAttack = Reader.ReadBoolean("Bools", "UseRangeAttack", mobAI.UseRangeAttack);
                mobAI.UseMagicAttack = Reader.ReadBoolean("Bools", "UseMagicAttack", mobAI.UseMagicAttack);
                mobAI.Target = Reader.ReadBoolean("Bools", "Target", mobAI.Target);
                mobAI.UseKillTimer = Reader.ReadBoolean("Bools", "UseKillTimer", mobAI.UseKillTimer);
                mobAI.IgnorePets = Reader.ReadBoolean("Bools", "IgnorePets", mobAI.IgnorePets);
                mobAI.DamagePetsMore = Reader.ReadBoolean("Bools", "DamagePetsMore", mobAI.DamagePetsMore);
                mobAI.CanPara = Reader.ReadBoolean("Bools", "CanPara", mobAI.CanPara);
                mobAI.CanGreen = Reader.ReadBoolean("Bools", "CanGreen", mobAI.CanGreen);
                mobAI.CanRed = Reader.ReadBoolean("Bools", "CanRed", mobAI.CanRed);
                mobAI.Spawn_Slaves = Reader.ReadBoolean("Bools", "SpawnSlaves", mobAI.Spawn_Slaves);
                #endregion

                #region Loading the Kill Timer
                if (mobAI.UseKillTimer)
                {
                    mobAI.KillTimer = Reader.ReadString("Strings", "KillTimer", mobAI.KillTimer);
                    if (mobAI.killTimer.Length > 0)
                    {
                        string[] timeString = mobAI.KillTimer.Split(' ');
                        string[] dateString = timeString[0].Split('-');
                        dateString[0] = dateString[0].Replace("(", "");
                        mobAI.LastKillDay = Convert.ToInt32(dateString[0]);
                        mobAI.LastKillMonth = Convert.ToInt32(dateString[1]);
                        mobAI.LastKillYear = Convert.ToInt32(dateString[2]);
                        dateString[1] = dateString[1].Replace("(", "");
                        dateString[1] = dateString[1].Replace(")", "");
                        string[] tempString = timeString[1].Split(':');
                        if (tempString[0].Contains("("))
                            tempString[0] = tempString[0].Replace("(", "");
                        if (tempString[1].Contains(")"))
                            tempString[1] = tempString[1].Replace(")", "");
                        mobAI.LastKillHour = Convert.ToInt32(tempString[0]);
                        mobAI.LastKillMinute = Convert.ToInt32(tempString[1]);
                    }
                    mobAI.RespawnDay = Reader.ReadInt32("Ints", "RespawnDays", mobAI.RespawnDay);
                    mobAI.RespawnMonth = Reader.ReadInt32("Ints", "RespawnMonths", mobAI.RespawnMonth);
                    mobAI.RespawnYear = Reader.ReadInt32("Ints", "RespawnYears", mobAI.RespawnYear);
                    mobAI.RespawnHour = Reader.ReadInt32("Ints", "RespawnHours", mobAI.RespawnHour);
                    mobAI.RespawnMinute = Reader.ReadInt32("Ints", "RespawnMinutes", mobAI.RespawnMinute);
                }
                #endregion

                if (mobAI.MapSpecific)
                    mobAI.MapName = Reader.ReadString("Strings", "MapName", mobAI.MapName);

                #region Loading the announcers
                if (mobAI.AnnounceSpawn)
                    mobAI.SpawnMessage = Reader.ReadString("Strings", "SpawnMessage", mobAI.SpawnMessage);

                if (mobAI.AnnounceDeath)
                    mobAI.DeadMessage = Reader.ReadString("Strings", "DeathMessage", mobAI.DeadMessage);

                if (mobAI.AnnounceDrop)
                    mobAI.ItemMessage = Reader.ReadString("Strings", "ItemMessage", mobAI.ItemMessage);
                #endregion

                if (SetXY)
                {
                    mobAI.MapX = Reader.ReadInt32("Ints", "MapX", mobAI.MapX);
                    mobAI.MapY = Reader.ReadInt32("Ints", "MapY", mobAI.MapY);
                }
                #region Loading the Attacks
                if (mobAI.UseMassAttack)
                {
                    mobAI.MassAttackEffect = Reader.ReadInt32("Ints", "MassAttackEffect", mobAI.MassAttackEffect);
                    mobAI.MassAttackDamage = Reader.ReadInt32("Damage", "MassAttackDamage", mobAI.MassAttackDamage);
                }
                if (mobAI.UseSpecialAttack)
                {
                    mobAI.SpecialAttackEffect = Reader.ReadInt32("Ints", "SpecialAttackEffect", mobAI.SpecialAttackEffect);
                    mobAI.SpecialAttackDamage = Reader.ReadInt32("Damage", "SpecialAttackDamage", mobAI.SpecialAttackDamage);
                }
                if (mobAI.UseMeleeAttack)
                {
                    mobAI.MeleeAttackEffect = Reader.ReadInt32("Ints", "MeleeAttackEffect", mobAI.MeleeAttackEffect);
                    mobAI.MeleeAttackDamage = Reader.ReadInt32("Damage", "MeleeAttackDamage", mobAI.MeleeAttackDamage);
                }
                if (mobAI.UseRangeAttack)
                {
                    mobAI.RangeAttackEffect = Reader.ReadInt32("Ints", "RangeAttackEffect", mobAI.RangeAttackEffect);
                    mobAI.RangeAttackDamage = Reader.ReadInt32("Damage", "RangeAttackDamage", mobAI.RangeAttackDamage);
                }
                if (mobAI.UseMagicAttack)
                {
                    mobAI.MagicAttackEffect = Reader.ReadInt32("Ints", "MagicAttackEffect", mobAI.MagicAttackEffect);
                    mobAI.MagicAttackDamage = Reader.ReadInt32("Damage", "MagicAttackDamage", mobAI.MagicAttackDamage);
                }
                if (mobAI.Target)
                {
                    mobAI.TargetClass = Reader.ReadInt32("Strings", "TargetClass", mobAI.TargetClass); //Default = None (5)
                    mobAI.TargetAttackDamage = Reader.ReadInt32("Damage", "TargetDamage", mobAI.TargetAttackDamage);
                }
                if (mobAI.DamagePetsMore)
                    mobAI.PetAttackDamage = Reader.ReadInt32("Damage", "PetDamage", mobAI.PetAttackDamage);
                #endregion

                #region Loading the Slaves Spawns
                if (mobAI.Spawn_Slaves)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        SpawnSlaves monsters = new SpawnSlaves();
                        monsters.Name = Reader.ReadString("Slaves", "Slave" + i, monsters.Name);
                        monsters.Count = Reader.ReadInt32("Slaves", "SlaveCount" + i, monsters.Count);
                        mobAI.Slaves.Add(monsters);
                    }
                }
                #endregion

                #region Loading the Item Drops
                mobAI.ItemCount = Reader.ReadInt32("Ints", "ItemCount", mobAI.ItemCount);
                if (mobAI.ItemCount > 0)
                {
                    for (int i = 0; i < mobAI.ItemCount; i++)
                    {
                        DropItemsWithAnnounce items = new DropItemsWithAnnounce();
                        items.Name = Reader.ReadString("Items", "Item" + i, items.Name);
                        items.Chance = Reader.ReadInt32("Items", "ItemChance" + i, items.Chance);
                        mobAI.Drops.Add(items);
                    }
                }
                #endregion

                #region Loading the current location
                mobAI.CurrentMap = Reader.ReadString("States", "CurrentMap", CurrentMap);
                mobAI.CurrentX = Reader.ReadInt32("States", "CurrentX", CurrentX);
                mobAI.CurrentY = Reader.ReadInt32("States", "CurrentY", CurrentY);

                #endregion

                return mobAI;
            }
            catch
            {
                SMain.Enqueue("Error with loading config");
                return null;
            }

        }

        public bool Save(CustomAI mobToSave)
        {
            if (mobToSave == null || mobToSave.Name.Length <= 0)
                return false;
            InIReader Writer = new InIReader(@".\Custom\Unique AIs\" + mobToSave.Name + ".ini");
            Writer.Write("Bools", "SetXY", mobToSave.SetXY);
            Writer.Write("Bools", "MapSpecific", mobToSave.MapSpecific);
            Writer.Write("Bools", "AnnounceSpawn", mobToSave.AnnounceSpawn);
            Writer.Write("Bools", "AnnounceDrop", mobToSave.AnnounceDrop);
            Writer.Write("Bools", "AnnounceDeath", mobToSave.AnnounceDeath);
            Writer.Write("Bools", "UseMassAttack", mobToSave.UseMassAttack);
            Writer.Write("Bools", "UseSpecialAttack", mobToSave.UseSpecialAttack);
            Writer.Write("Bools", "UseMeleeAttack", mobToSave.UseMeleeAttack);
            Writer.Write("Bools", "UseRangeAttack", mobToSave.UseRangeAttack);
            Writer.Write("Bools", "Target", mobToSave.Target);
            Writer.Write("Bools", "UseKillTimer", mobToSave.UseKillTimer);
            Writer.Write("Bools", "IgnorePets", mobToSave.IgnorePets);
            Writer.Write("Bools", "DamagePetsMore", mobToSave.DamagePetsMore);
            Writer.Write("Bools", "CanPara", mobToSave.CanPara);
            Writer.Write("Bools", "CanGreen", mobToSave.CanGreen);
            Writer.Write("Bools", "CanRed", mobToSave.CanRed);
            Writer.Write("Bools", "SpawnSlaves", mobToSave.Spawn_Slaves);
            if (mobToSave.MapSpecific)
                Writer.Write("Strings", "MapName", mobToSave.MapName);
            if (mobToSave.AnnounceSpawn)
                Writer.Write("Strings", "SpawnMessage", mobToSave.SpawnMessage);
            if (mobToSave.AnnounceDeath)
                Writer.Write("Strings", "DeathMessage", mobToSave.DeadMessage);
            if (mobToSave.AnnounceDrop)
                Writer.Write("Strings", "ItemMessage", mobToSave.ItemMessage);
            if (mobToSave.SetXY)
            {
                Writer.Write("Ints", "MapX", mobToSave.MapX);
                Writer.Write("Ints", "MapY", mobToSave.MapY);
            }

            if (mobToSave.UseMassAttack)
            {
                Writer.Write("Ints", "MassAttackEffect", mobToSave.MassAttackEffect);
                Writer.Write("Damage", "MassAttackDamage", mobToSave.MassAttackDamage);
            }
            if (mobToSave.UseSpecialAttack)
            {
                Writer.Write("Ints", "SpecialAttackEffect", mobToSave.SpecialAttackEffect);
                Writer.Write("Damage", "SpecialAttackDamage", mobToSave.SpecialAttackDamage);
            }
            if (mobToSave.UseMeleeAttack)
            {
                Writer.Write("Ints", "MeleeAttackEffect", mobToSave.MeleeAttackEffect);
                Writer.Write("Damage", "MeleeAttackDamage", mobToSave.MeleeAttackDamage);
            }
            if (mobToSave.UseRangeAttack)
            {
                Writer.Write("Ints", "RangeAttackEffect", mobToSave.RangeAttackEffect);
                Writer.Write("Damage", "RangeAttackDamage", mobToSave.RangeAttackDamage);
            }
            if (mobToSave.UseMagicAttack)
            {
                Writer.Write("Ints", "MagicAttackEffect", mobToSave.MagicAttackEffect);
                Writer.Write("Damage", "MagicAttackDamage", mobToSave.MagicAttackDamage);
            }
            if (mobToSave.Target)
            {
                Writer.Write("Strings", "TargetClass", mobToSave.TargetClass); //Default = None (5)
                Writer.Write("Damage", "TargetDamage", mobToSave.TargetAttackDamage);
            }
            if (mobToSave.DamagePetsMore || !mobToSave.IgnorePets)
                Writer.Write("Damage", "PetDamage", mobToSave.PetAttackDamage);

            if (mobToSave.UseKillTimer)
            {
                Writer.Write("Strings", "KillTimer", mobToSave.KillTimer);
                Writer.Write("Ints", "RespawnDays", mobToSave.RespawnDay);
                Writer.Write("Ints", "RespawnMonths", mobToSave.RespawnMonth);
                Writer.Write("Ints", "RespawnYears", mobToSave.RespawnYear);
                Writer.Write("Ints", "RespawnHours", mobToSave.RespawnHour);
                Writer.Write("Ints", "RespawnMinutes", mobToSave.RespawnMinute);
            }
            Writer.Write("Ints", "ItemCount", mobToSave.ItemCount);

            if (mobToSave.Spawn_Slaves)
            {
                for (int i = 0; i < mobToSave.Slaves.Count; i++)
                {
                    Writer.Write("Slaves", "Slave" + i, mobToSave.Slaves[i].Name);
                    Writer.Write("Slaves", "SlaveCount" + i, mobToSave.Slaves[i].Count);
                }
            }
            if (mobToSave.ItemCount > 0)
            {
                for (int i = 0; i < mobToSave.ItemCount; i++)
                {
                    Writer.Write("Items", "Item" + i, mobToSave.Drops[i].Name);
                    Writer.Write("Items", "ItemChance" + i, mobToSave.Drops[i].Chance);
                }
            }
            Writer.Write("States", "Status", mobToSave.Alive);
            Writer.Write("States", "CurrentMap", mobToSave.CurrentMap);
            Writer.Write("States", "CurrentX", mobToSave.CurrentX);
            Writer.Write("States", "CurrentY", mobToSave.CurrentY);
            // Pete107|Petesn00beh. Updated 02/03/2016
            #region Update the List held within the Environment
            if (Envir.CustomAIList.Count > 0)
            {
                bool found = false;
                for (int i = 0; i < Envir.CustomAIList.Count; i++)
                {
                    if (Envir.CustomAIList[i].name == mobToSave.name)
                    {
                        found = true;
                        Envir.CustomAIList[i] = mobToSave; // Update the Environment every save
                    }
                }
                if (!found)
                    Envir.CustomAIList.Add(mobToSave);
            }
            else
                Envir.CustomAIList.Add(mobToSave);
            #endregion
            return true;
        }

    }
    public class SpawnSlaves
    {
        private string name = "";
        private int count = 0;
        public string Name { get { return name; } set { name = value; } }
        public int Count { get { return count; } set { count = value; } }
        public SpawnSlaves() { }

        public SpawnSlaves(string _name, int _count)
        {
            Name = _name;
            Count = _count;
        }
    }

    public class DropItemsWithAnnounce
    {
        private string name = "";
        private int chance = 0;
        public string Name { get { return name; } set { name = value; } }
        public int Chance { get { return chance; } set { chance = value; } }

        public DropItemsWithAnnounce() { }

        public DropItemsWithAnnounce(string _name, int _chance)
        {
            Name = _name;
            Chance = _chance;
        }
    }
}
