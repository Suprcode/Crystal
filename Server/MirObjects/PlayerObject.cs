﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ClientPackets;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirNetwork;
using S = ServerPackets;
using System.Text.RegularExpressions;

namespace Server.MirObjects
{
    public sealed class PlayerObject : MapObject
    {
        public string GMPassword = Settings.GMPassword;
        public bool IsGM, GMLogin, GMNeverDie, GMGameMaster, EnableGroupRecall, EnableGuildInvite;

        public bool HasUpdatedBaseStats = true;

        public long LastRecallTime, LastRevivalTime, LastTeleportTime, LastProbeTime;

        public short Looks_Armour = 0, Looks_Weapon = -1;
        public byte Looks_Wings = 0;
        
        public override ObjectType Race
        {
            get { return ObjectType.Player; }
        }

        public CharacterInfo Info;
        public AccountInfo Account;
        public MirConnection Connection;

        public override string Name
        {
            get { return Info.Name; }
            set { /*Check if Name exists.*/ }
        }

        public override int CurrentMapIndex
        {
            get { return Info.CurrentMapIndex; }
            set { Info.CurrentMapIndex = value; }
        }
        public override Point CurrentLocation
        {
            get { return Info.CurrentLocation; }
            set { Info.CurrentLocation = value; }
        }
        public override MirDirection Direction
        {
            get { return Info.Direction; }
            set { Info.Direction = value; }
        }
        public override byte Level
        {
            get { return Info.Level; }
            set { Info.Level = value; }
        }

        public override uint Health
        {
            get { return HP; }
        }
        public override uint MaxHealth
        {
            get { return MaxHP; }
        }

        public ushort HP
        {
            get { return Info.HP; }
            set { Info.HP = value; }
        }
        public ushort MP
        {
            get { return Info.MP; }
            set { Info.MP = value; }
        }

        public ushort MaxHP, MaxMP;

        public override AttackMode AMode
        {
            get { return Info.AMode; }
            set { Info.AMode = value; }
        }
        public override PetMode PMode
        {
            get { return Info.PMode; }
            set { Info.PMode = value; }
        }

        public long Experience
        {
            set { Info.Experience = value; }
            get { return Info.Experience; }
        }
        public long MaxExperience;
        public byte LifeOnHit;
        public byte HpDrainRate;
        public float HpDrain = 0;

        public float ExpRateOffset = 0;

        public bool NewMail = false;

        public override int PKPoints
        {
            get { return Info.PKPoints; }
            set { Info.PKPoints = value; }
        }

        public byte Hair
        {
            get { return Info.Hair; }
            set { Info.Hair = value; }
        }
        public MirClass Class
        {
            get { return Info.Class; }
        }
        public MirGender Gender
        { get { return Info.Gender; } }

        public int BindMapIndex
        {
            get { return Info.BindMapIndex; }
            set { Info.BindMapIndex = value; }
        }
        public Point BindLocation
        {
            get { return Info.BindLocation; }
            set { Info.BindLocation = value; }
        }

        public bool RidingMount;
        public MountInfo Mount
        {
            get { return Info.Mount; }
        }
        public short MountType
        {
            get { return Mount.MountType; }
            set { Mount.MountType = value; }
        }

        public int FishingChance, FishingChanceCounter, FishingProgressMax, FishingProgress, FishingAutoReelChance = 0, FishingNibbleChance = 0;
        public bool Fishing, FishingAutocast, FishFound, FishFirstFound;

        public bool CanMove
        {
            get { return !Dead && Envir.Time >= ActionTime && !Fishing; }
        }
        public bool CanWalk
        {
            get { return !Dead && Envir.Time >= ActionTime && !InTrapRock && !Fishing; }
        }
        public bool CanRun
        {
            get { return !Dead && Envir.Time >= ActionTime && (_stepCounter > 0 || FastRun) && CurrentPoison != PoisonType.Slow && (!Sneaking || ActiveSwiftFeet) && CurrentBagWeight <= MaxBagWeight; }
        }
        public bool CanAttack
        {
            get
            {
                return !Dead && Envir.Time >= ActionTime && Envir.Time >= AttackTime && CurrentPoison != PoisonType.Paralysis && CurrentPoison != PoisonType.Frozen && Mount.CanAttack && !Fishing;
            }
        }

        public bool CanRegen
        {
            get { return Envir.Time >= RegenTime && _runCounter == 0; }
        }
        private bool CanCast
        {
            get
            {
                return !Dead && Envir.Time >= ActionTime && Envir.Time >= SpellTime && CurrentPoison != PoisonType.Stun &&
                    CurrentPoison != PoisonType.Paralysis && CurrentPoison != PoisonType.Frozen && Mount.CanAttack && !Fishing;
            }
        }

        public const long TurnDelay = 350, MoveDelay = 600, HarvestDelay = 350, RegenDelay = 10000, PotDelay = 200, HealDelay = 600, DuraDelay = 10000, VampDelay = 500, LoyaltyDelay = 1000, FishingCastDelay = 750, FishingDelay = 200, CreatureTimeLeftDelay = 1000;
        public long ActionTime, RunTime, RegenTime, PotTime, HealTime, AttackTime, TorchTime, DuraTime, DecreaseLoyaltyTime, IncreaseLoyaltyTime, ShoutTime, SpellTime, VampTime, SearchTime, FishingTime, LogTime, FishingFoundTime, CreatureTimeLeftTicker;

        public bool MagicShield;
        public byte MagicShieldLv;
        public long MagicShieldTime;

        #region Elemental System
        public bool HasElemental;
        public int ElementsLevel;
        private bool _concentrating;
        public bool Concentrating
        {
            get
            {
                return _concentrating;
            }
            set
            {
                if (_concentrating == value) return;
                _concentrating = value;
            }

        }
        public bool ConcentrateInterrupted;
        public long ConcentrateInterruptTime;
        public bool ElementalBarrier;
        public byte ElementalBarrierLv;
        public long ElementalBarrierTime;
        #endregion

        public IntelligentCreatureType SummonedCreatureType = IntelligentCreatureType.None;
        public bool CreatureSummoned;

        public LevelEffects LevelEffects = LevelEffects.None;

        private int _stepCounter, _runCounter, _fishCounter;

        public MapObject[,] ArcherTrapObjectsArray = new MapObject[4,3];

        public NPCObject DefaultNPC
        {
            get
            {
                return SMain.Envir.DefaultNPC;
            }
        }

        public uint NPCID;
        public NPCPage NPCPage;
        public bool NPCSuccess;
        public bool NPCDelayed;

        public Map NPCMoveMap;
        public Point NPCMoveCoord;

        public List<KeyValuePair<string, string>> NPCVar = new List<KeyValuePair<string, string>>();

        public bool UserMatch;
        public string MatchName;
        public ItemType MatchType;
        public int PageSent;
        public List<AuctionInfo> Search = new List<AuctionInfo>();
        public List<ItemSets> ItemSets = new List<ItemSets>();
        public List<EquipmentSlot> MirSet = new List<EquipmentSlot>();

        public bool FatalSword, Slaying, TwinDrakeBlade, FlamingSword, MPEater, Hemorrhage, bCounterAttack;
        public int MPEaterCount, HemorrhageAttackCount;
        public long FlamingSwordTime, PoisonCloudTime, SlashingBurstTime, FuryTime, TrapTime, SwiftFeetTime, CounterAttackTime;
        public bool ActiveBlizzard, ActiveReincarnation, ActiveSwiftFeet, ReincarnationReady;
        public PlayerObject ReincarnationTarget, ReincarnationHost;
        public long ReincarnationExpireTime;
        public byte Reflect;
        public bool UnlockCurse = false;
        public bool FastRun = false;
        public bool CanGainExp = true;

        public bool CanCreateGuild = false;
        public GuildObject MyGuild = null;
        public Rank MyGuildRank = null;
        public GuildObject PendingGuildInvite = null;
        public bool GuildNoticeChanged = true; //set to false first time client requests notice list, set to true each time someone in guild edits notice
        public bool GuildMembersChanged = true;//same as above but for members
        public bool GuildCanRequestItems = true;
        public override bool Blocking
        {
            get
            {
                return !Dead && !Observer;
            }
        }
        public bool AllowGroup
        {
            get { return Info.AllowGroup; }
            set { Info.AllowGroup = value; }
        }

        public bool AllowTrade
        {
            get { return Info.AllowTrade; }
            set { Info.AllowTrade = value; }
        }

        public bool GameStarted { get; set; }

        public bool HasTeleportRing, HasProtectionRing, HasRevivalRing;

        public bool HasMuscleRing, HasClearRing, HasParalysisRing, HasProbeNecklace, HasSkillNecklace, NoDuraLoss;

        public PlayerObject GroupInvitation;
        public PlayerObject TradeInvitation;

        public PlayerObject TradePartner = null;
        public bool TradeLocked = false;
        public uint TradeGoldAmount = 0;

        public List<QuestProgressInfo> CurrentQuests
        {
            get { return Info.CurrentQuests; }
        }

        public List<int> CompletedQuests
        {
            get { return Info.CompletedQuests; }
        }

        public PlayerObject(CharacterInfo info, MirConnection connection)
        {
            if (info.Player != null)
                throw new InvalidOperationException("Player.Info not Null.");

            info.Player = this;
            info.Mount = new MountInfo(this);

            Connection = connection;
            Info = info;
            Account = Connection.Account;

            if (Level == 255 || Account.AdminAccount)
            {
                IsGM = true;
                SMain.Enqueue(string.Format("{0} is now a GM", Name));
            }

            if (Level == 0) NewCharacter();

            RefreshStats();

            if (HP == 0)
            {
                SetHP(MaxHP);
                SetMP(MaxMP);
                CurrentLocation = BindLocation;
                CurrentMapIndex = BindMapIndex;
            }
        }
        public void StopGame(byte reason)
        {
            if (Node == null) return;

            for (int i = 0; i < Pets.Count; i++)
            {
                MonsterObject pet = Pets[i];

                if (pet.Info.AI == 64)//IntelligentCreature
                {
                    //dont save Creatures they will miss alot of AI-Info when they get spawned on login
                    UnSummonIntelligentCreature(((IntelligentCreatureObject)Pets[i]).petType, false);
                    continue;
                }

                pet.Master = null;

                if (!pet.Dead)
                {
                    Info.Pets.Add(new PetInfo(pet));
                    pet.CurrentMap.RemoveObject(pet);
                    pet.Despawn();
                }
            }
            Pets.Clear();

            for (int i = 0; i < Buffs.Count; i++ )
            {
                Buff buff = Buffs[i];
                if (buff.Infinite) continue;

                buff.ExpireTime -= Envir.Time;

                Info.Buffs.Add(new Buff(buff));
            }
            Buffs.Clear();

            if (MyGuild != null) MyGuild.PlayerLogged(this, false);
            Envir.Players.Remove(this);
            CurrentMap.RemoveObject(this);
            Despawn();

            if (GroupMembers != null)
            {
                GroupMembers.Remove(this);

                if (GroupMembers.Count > 1)
                {
                    Packet p = new S.DeleteMember { Name = Name };

                    for (int i = 0; i < GroupMembers.Count; i++)
                        GroupMembers[i].Enqueue(p);
                }
                else
                {
                    GroupMembers[0].Enqueue(new S.DeleteGroup());
                    GroupMembers[0].GroupMembers = null;
                }
                GroupMembers = null;
            }

            TradeCancel();

            DisplayLogOutMsg(reason);

            Fishing = false;

            Info.LastIP = Connection.IPAddress;
            Info.LastDate = Envir.Now;

            CleanUp();
        }

        private void DisplayLogOutMsg(byte reason)
        {
            switch (reason)
            {
                //0-10 are 'senddisconnect to client'
                case 0:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: Server closed", Name));
                    return;
                case 1:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: Double login", Name));
                    return;
                case 2:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: Chat message too long", Name));
                    return;
                case 3:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: Server crashed", Name));
                    return;
                case 4:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: Kicked by admin", Name));
                    return;
                case 10:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: Wrong client version", Name));
                    return;
                case 20:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: User gone missing / disconnected", Name));
                    return;
                case 21:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: Connection timed out", Name));
                    return;
                case 22:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: User closed game", Name));
                    return;
                case 23:
                    SMain.Enqueue(string.Format("{0} Has logged out. Reason: User returned to select char", Name));
                    return;
            }
        }

        private void NewCharacter()
        {
            if (Envir.StartPoints.Count == 0) return;

            SetBind();

            Level = 1;
            Hair = (byte)SMain.Envir.Random.Next(0, 9);

            
            for (int i = 0; i < Envir.StartItems.Count; i++)
            {
                ItemInfo info = Envir.StartItems[i];
                if (!CorrectStartItem(info)) continue;

                AddItem(Envir.CreateFreshItem(info));
            }

        }

        public override void Process()
        {
            if (GroupInvitation != null && GroupInvitation.Node == null)
                GroupInvitation = null;

            if (MagicShield && Envir.Time > MagicShieldTime)
            {
                MagicShield = false;
                MagicShieldLv = 0;
                MagicShieldTime = 0;
                CurrentMap.Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.MagicShieldDown }, CurrentLocation);
            }

            if (ElementalBarrier && Envir.Time > ElementalBarrierTime)//ArcherSpells - Elemental system
            {
                ElementalBarrier = false;
                ElementalBarrierLv = 0;
                ElementalBarrierTime = 0;
                CurrentMap.Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.ElementBarrierDown }, CurrentLocation);
            }

            for (int i = 0; i <= 3; i++)//ArcherSpells - Explosive Trap     Self destruct when out of range (in this case 15 squares)
            {
                if (ArcherTrapObjectsArray[i,0] == null) continue;
                if (FindObject(ArcherTrapObjectsArray[i,0].ObjectID, 15) != null) continue;
                bool detonated = true;
                for (int j = 0; j <= 2; j++)
                    if (!((SpellObject)ArcherTrapObjectsArray[i, j]).DetonatedTrap) detonated = false;
                if (detonated) continue;
                for (int j = 0; j <= 2; j++)
                    ((SpellObject)ArcherTrapObjectsArray[i,j]).DetonateTrapNow();
            }

            if (Sneaking) CheckSneakRadius();

            if (FlamingSword && Envir.Time >= FlamingSwordTime)
            {
                FlamingSword = false;
                Enqueue(new S.SpellToggle { Spell = Spell.FlamingSword, CanUse = false });
            }

            if (bCounterAttack && Envir.Time >= CounterAttackTime)
            {
                bCounterAttack = false;
            }

            if (ReincarnationReady && Envir.Time >= ReincarnationExpireTime)
            {
                ReincarnationReady = false;
                ActiveReincarnation = false;
                ReincarnationTarget = null;
                ReceiveChat("Reincarnation failed.", ChatType.System);
            }
            if ((ReincarnationReady || ActiveReincarnation) && (ReincarnationTarget == null || !ReincarnationTarget.Dead))
            {
                ReincarnationReady = false;
                ActiveReincarnation = false;
                ReincarnationTarget = null;
            }

            if (Envir.Time > RunTime && _runCounter > 0)
            {
                RunTime = Envir.Time + 1500;
                _runCounter--;
            }

            if(NewMail)
            {
                GetMail();
            }

            //if (Envir.Time > ActionTime && _stepCounter > 0)
            //{
            //    _stepCounter = 0;
            //}

            if (Envir.Time > IncreaseLoyaltyTime && Mount.HasMount)
            {
                IncreaseLoyaltyTime = Envir.Time + (LoyaltyDelay * 60);
                IncreaseMountLoyalty(1);
            }

            if (Envir.Time > FishingTime && Fishing)
            {
                _fishCounter++;
                UpdateFish();
            }

            for (int i = Pets.Count() - 1; i >= 0; i--)
            {
                if (Pets[i].Dead)
                    Pets.Remove(Pets[i]);
            }

            ProcessBuffs();
            ProcessInfiniteBuffs();
            ProcessRegen();
            ProcessPoison();

            RefreshCreaturesTimeLeft();//IntelligentCreature

            /*  if (HealthChanged)
              {
                  Enqueue(new S.HealthChanged { HP = HP, MP = MP });

                  BroadcastHealthChange();

                  HealthChanged = false;
              }*/

            UserItem item;
            if (Envir.Time > TorchTime)
            {
                TorchTime = Envir.Time + 10000;
                item = Info.Equipment[(int)EquipmentSlot.Torch];
                if (item != null)
                {
                    DamageItem(item, 5);

                    if (item.CurrentDura == 0)
                    {
                        Info.Equipment[(int)EquipmentSlot.Torch] = null;
                        Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = item.Count });
                        RefreshStats();
                    }
                }
            }

            if (Envir.Time > DuraTime)
            {
                DuraTime = Envir.Time + DuraDelay;

                for (int i = 0; i < Info.Equipment.Length; i++)
                {
                    item = Info.Equipment[i];
                    if (item == null || !item.DuraChanged) continue; // || item.Info.Type == ItemType.Mount
                    item.DuraChanged = false;
                    Enqueue(new S.DuraChanged { UniqueID = item.UniqueID, CurrentDura = item.CurrentDura });
                }
            }

            base.Process();

            RefreshNameColour();

        }
        public override void SetOperateTime()
        {
            OperateTime = Envir.Time;
        }

        private void ProcessBuffs()
        {
            bool refresh = false;
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {

                Buff buff = Buffs[i];

                if (Envir.Time <= buff.ExpireTime) continue;
                if (buff.Infinite) continue;

                Buffs.RemoveAt(i);
                Enqueue(new S.RemoveBuff { Type = buff.Type, ObjectID = ObjectID });

                if (buff.Visible)
                    Broadcast(new S.RemoveBuff { Type = buff.Type, ObjectID = ObjectID });

                switch (buff.Type)
                {
                    case BuffType.MoonLight:
                    case BuffType.Hiding:
                    case BuffType.DarkBody:
                        if(!HasClearRing) Hidden = false;
                        Sneaking = false;
                        for (int j = 0; j < Buffs.Count; j++)
                        {
                            switch (Buffs[j].Type)
                            {
                                case BuffType.Hiding:
                                case BuffType.MoonLight:
                                case BuffType.DarkBody:
                                    if (Buffs[j].Type != buff.Type)
                                        Buffs[j].ExpireTime = 0;
                                    break;
                            }
                        }
                        break;
                    case BuffType.Concentration://ArcherSpells - Elemental system
                        ConcentrateInterrupted = false;
                        ConcentrateInterruptTime = 0;
                        Concentrating = false;
                        UpdateConcentration();//Update & send to client
                        break;
                    case BuffType.SwiftFeet:
                        ActiveSwiftFeet = false;
                        break;
                }

                refresh = true;
            }

            if (Concentrating && !ConcentrateInterrupted && (ConcentrateInterruptTime != 0))//ArcherSpells - Elemental system
            {
                //check for reenable
                if (ConcentrateInterruptTime <= SMain.Envir.Time)
                {
                    ConcentrateInterruptTime = 0;
                    UpdateConcentration();//Update & send to client
                }
            }

            if (refresh) RefreshStats();
        }

        private void ProcessInfiniteBuffs()
        {
            bool hiding = false;
            bool mentalState = false;

            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                Buff buff = Buffs[i];

                if (!buff.Infinite) continue;

                bool removeBuff = false;

                switch (buff.Type)
                {
                    case BuffType.Hiding:
                        hiding = true;
                        if (!HasClearRing) removeBuff = true;
                        break;
                    case BuffType.MentalState:
                        mentalState = true;
                        break;
                }

                if (removeBuff)
                {
                    Buffs.RemoveAt(i);
                    Enqueue(new S.RemoveBuff { Type = buff.Type, ObjectID = ObjectID });

                    switch(buff.Type)
                    {
                        case BuffType.Hiding:
                            Hidden = false;
                            break;
                    }
                }
            }

            if(HasClearRing && !hiding)
            {
                AddBuff(new Buff { Type = BuffType.Hiding, Caster = this, ExpireTime = Envir.Time + 100, Infinite = true });
            }

            if (GetMagic(Spell.MentalState) != null && !mentalState)
            {
                AddBuff(new Buff { Type = BuffType.MentalState, Caster = this, ExpireTime = Envir.Time + 100, Value = Info.MentalState, Infinite = true });
            }
        }

        
        private void ProcessRegen()
        {
            if (Dead) return;

            int healthRegen = 0, manaRegen = 0;

            if (CanRegen)
            {
                RegenTime = Envir.Time + RegenDelay;


                if (HP < MaxHP)
                {
                    healthRegen += (int)(MaxHP * 0.03F) + 1;
                    healthRegen += (int)(healthRegen * ((double)HealthRecovery / Settings.HealthRegenWeight));
                }

                if (MP < MaxMP)
                {
                    manaRegen += (int)(MaxMP * 0.03F) + 1;
                    manaRegen += (int)(manaRegen * ((double)SpellRecovery / Settings.ManaRegenWeight));
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
                if (HealAmount > 5)
                {
                    healthRegen += (5 + incHeal);
                    HealAmount -= (ushort)(5 + incHeal);
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

            if (healthRegen > 0) ChangeHP(healthRegen);
            if (HP == MaxHP)
            {
                PotHealthAmount = 0;
                HealAmount = 0;
            }

            if (manaRegen > 0) ChangeMP(manaRegen);
            if (MP == MaxMP) PotManaAmount = 0;
        }
        private void ProcessPoison()
        {
            PoisonType type = PoisonType.None;
            PoisonRate = 1F;

            for (int i = PoisonList.Count - 1; i >= 0; i--)
            {
                if (Dead) return;

                Poison poison = PoisonList[i];

                if (poison.Owner != null && poison.Owner.Node == null)
                {
                    PoisonList.RemoveAt(i);
                    continue;
                }

                if (Envir.Time > poison.TickTime)
                {
                    poison.Time++;
                    poison.TickTime = Envir.Time + poison.TickSpeed;

                    if (poison.Time >= poison.Duration)
                        PoisonList.RemoveAt(i);

                    if (poison.PType == PoisonType.Green || poison.PType == PoisonType.Bleeding)
                    {
                        LastHitter = poison.Owner;
                        LastHitTime = Envir.Time + 10000;

                        if (poison.PType == PoisonType.Bleeding)
                        {
                            Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.Bleeding, EffectType = 0 });
                        }

                        ChangeHP(-poison.Value);
                        if (Dead) break;
                        RegenTime = Envir.Time + RegenDelay;
                    }

                    if (poison.PType == PoisonType.DelayedExplosion)//ArcherSpells - DelayedExplosion
                    {
                        if (Envir.Time > ExplosionInflictedTime) ExplosionInflictedStage++;

                        if (!ProcessDelayedExplosion(poison))
                        {
                            if (Dead) break;

                            ExplosionInflictedStage = 0;
                            ExplosionInflictedTime = 0;
                            
                            PoisonList.RemoveAt(i);
                            continue;
                        }
                    }
                }

                switch (poison.PType)
                {
                    case PoisonType.Red:
                        PoisonRate += 0.10F;
                        break;
                    case PoisonType.Stun:
                        PoisonRate += 0.2F;
                        break;
                }

                if ((int)type < (int)poison.PType)
                    type = poison.PType;
            }

            if (type == CurrentPoison) return;

            Enqueue(new S.Poisoned { Poison = type });
            Broadcast(new S.ObjectPoisoned { ObjectID = ObjectID, Poison = type });

            CurrentPoison = type;
        }

        private bool ProcessDelayedExplosion(Poison poison)
        {
            if (Dead) return false;

            if (ExplosionInflictedStage == 0)
            {
                Enqueue(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion, EffectType = 0 });
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion, EffectType = 0 });
                return true;
            }
            if (ExplosionInflictedStage == 1)
            {
                if (Envir.Time > ExplosionInflictedTime)
                    ExplosionInflictedTime = poison.TickTime + 3000;
                Enqueue(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion, EffectType = 1 });
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion, EffectType = 1 });
                return true;
            }
            if (ExplosionInflictedStage == 2)
            {
                Enqueue(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion, EffectType = 2 });
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion, EffectType = 2 });
                if (poison.Owner != null)
                {
                    switch (poison.Owner.Race)
                    {
                        case ObjectType.Player:
                            Attacked((PlayerObject)poison.Owner, poison.Value, DefenceType.MAC, false);
                            break;
                        case ObjectType.Monster://this is in place so it could be used by mobs if one day someone chooses to
                            Attacked((MonsterObject)poison.Owner, poison.Value, DefenceType.MAC);
                            break;
                    }
                    LastHitter = poison.Owner;
                }
                return false;
            }
            return false;
        }

        public override void Process(DelayedAction action)
        {
            switch (action.Type)
            {
                case DelayedType.Magic:
                    CompleteMagic(action.Params);
                    break;
                case DelayedType.Damage:
                    CompleteAttack(action.Params);
                    break;
                case DelayedType.MapMovement:
                    CompleteMapMovement(action.Params);
                    break;
                case DelayedType.Mine:
                    CompleteMine(action.Params);
                    break;
                case DelayedType.NPC:
                    CompleteNPC(action.Params);
                    break;
                case DelayedType.Poison:
                    CompletePoison(action.Params);
                    break;
            }
        }


        private void SetHP(ushort amount)
        {
            if (HP == amount) return;

            HP = amount <= MaxHP ? amount : MaxHP;
            HP = GMNeverDie ? MaxHP : HP;

            if (!Dead && HP == 0) Die();

            //HealthChanged = true;
            Enqueue(new S.HealthChanged { HP = HP, MP = MP });
            BroadcastHealthChange();
        }
        private void SetMP(ushort amount)
        {
            if (MP == amount) return;
            //was info.MP
            MP = amount <= MaxMP ? amount : MaxMP;
            MP = GMNeverDie ? MaxMP : MP;

            // HealthChanged = true;
            Enqueue(new S.HealthChanged { HP = HP, MP = MP });
            BroadcastHealthChange();
        }

        public void ChangeHP(int amount)
        {
            if (amount < 0) amount = (int)(amount * PoisonRate);

            if (HasProtectionRing && MP > 0 && amount < 0)
            {
                ChangeMP(amount);
                return;
            }

            ushort value = (ushort)Math.Max(ushort.MinValue, Math.Min(MaxHP, HP + amount));

            if (value == HP) return;

            HP = value;
            HP = GMNeverDie ? MaxHP : HP;

            if (!Dead && HP == 0) Die();

            // HealthChanged = true;
            Enqueue(new S.HealthChanged { HP = HP, MP = MP });
            BroadcastHealthChange();
        }
        public void ChangeMP(int amount)
        {
            ushort value = (ushort)Math.Max(ushort.MinValue, Math.Min(MaxMP, MP + amount));

            if (value == MP) return;

            MP = value;
            MP = GMNeverDie ? MaxMP : MP;

            // HealthChanged = true;
            Enqueue(new S.HealthChanged { HP = HP, MP = MP });
            BroadcastHealthChange();
        }
        public override void Die()
        {
            if (HasRevivalRing && Envir.Time > LastRevivalTime)
            {
                LastRevivalTime = Envir.Time + 300000;

                for (var i = (int)EquipmentSlot.RingL; i <= (int)EquipmentSlot.RingR; i++)
                {
                    var item = Info.Equipment[i];

                    if (item == null) continue;
                    if (!(item.Info.Unique.HasFlag(SpecialItemMode.Revival)) || item.CurrentDura < 1000) continue;
                    SetHP(MaxHP);
                    item.CurrentDura = (ushort)(item.CurrentDura - 1000);
                    Enqueue(new S.DuraChanged { UniqueID = item.UniqueID, CurrentDura = item.CurrentDura });
                    RefreshStats();
                    ReceiveChat("You have been given a second chance at life", ChatType.System);
                    return;
                }
            }

            if (LastHitter != null && LastHitter.Race == ObjectType.Player && !AtWar((PlayerObject)LastHitter))
            {
                if (Envir.Time > BrownTime && PKPoints < 200)
                {
                    PlayerObject hitter = (PlayerObject)LastHitter;

                    if(hitter.Info.Equipment[(byte)EquipmentSlot.Weapon] != null && Envir.Random.Next(4) == 0)
                    {
                        hitter.Info.Equipment[(byte)EquipmentSlot.Weapon].Cursed = true;
                    }

                    LastHitter.PKPoints = Math.Min(int.MaxValue, LastHitter.PKPoints + 100);
                    LastHitter.ReceiveChat(string.Format("You have murdered {0}", Name), ChatType.System);
                    ReceiveChat(string.Format("You have been murdered by {0}", LastHitter.Name), ChatType.System);
                }
            }

            for (int i = Pets.Count - 1; i >= 0; i--)
                Pets[i].Die();

            if (PKPoints > 200)
                RedDeathDrop(LastHitter);
            else if (!InSafeZone)
                DeathDrop(LastHitter);

            HP = 0;
            Dead = true;

            Enqueue(new S.Death { Direction = Direction, Location = CurrentLocation });
            Broadcast(new S.ObjectDied { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            PoisonList.Clear();
            InTrapRock = false;

            CallDefaultNPC(DefaultNPCType.Die);
        }

        private void DeathDrop(MapObject killer)
        {
            bool pkbodydrop = true;
            if (CurrentMap.Info.NoDropPlayer && Race == ObjectType.Player) return;

            if ((killer == null) || ((pkbodydrop) || (killer.Race != ObjectType.Player)))
            {
                UserItem temp = Info.Equipment[(int)EquipmentSlot.Stone];
                if ((temp != null) && ((killer == null) || ((killer != null) && (killer.Race != ObjectType.Player))))
                {
                    Info.Equipment[(int)EquipmentSlot.Stone] = null;
                    Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = temp.Count });
                }

                for (int i = 0; i < Info.Equipment.Length; i++)
                {
                    temp = Info.Equipment[i];
                    
                    if (temp == null) continue;
                    if (temp.Info.Bind.HasFlag(BindMode.DontDeathdrop)) continue;

                    if (ItemSets.Any(set => set.Set == ItemSet.Spirit && !set.SetComplete))
                    {
                        if (temp.Info.Set == ItemSet.Spirit)
                        {
                            Info.Equipment[i] = null;
                            Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = temp.Count });
                        }
                    }

                    if (temp.Count > 1)
                    {
                        int percent = Envir.RandomomRange(10, 8);

                        uint count = (uint)Math.Ceiling(temp.Count / 10F * percent);

                        if (count > temp.Count)
                            throw new ArgumentOutOfRangeException();

                        UserItem temp2 = Envir.CreateFreshItem(temp.Info);
                        temp2.Count = count;

                        if (DropItem(temp2, Settings.DropRange))
                        {
                            if (count == temp.Count)
                                Info.Equipment[i] = null;

                            Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = count });
                            temp.Count -= count;
                        }
                    }
                    else if (Envir.Random.Next(30) == 0)
                    {
                        if (DropItem(temp, Settings.DropRange))
                        {
                            Info.Equipment[i] = null;
                            Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = temp.Count });
                        }
                    }
                }

            }

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                UserItem temp = Info.Inventory[i];

                if (temp == null) continue;
                if (temp.Info.Bind.HasFlag(BindMode.DontDeathdrop)) continue;

                if (temp.Count > 1)
                {
                    int percent = Envir.RandomomRange(10, 8);
                    if (percent == 0) continue;

                    uint count = (uint)Math.Ceiling(temp.Count / 10F * percent);

                    if (count > temp.Count)
                        throw new ArgumentOutOfRangeException();

                    UserItem temp2 = Envir.CreateFreshItem(temp.Info);
                    temp2.Count = count;

                    if (DropItem(temp2, Settings.DropRange))
                    {
                        if (count == temp.Count)
                            Info.Inventory[i] = null;

                        Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = count });
                        temp.Count -= count;
                    }
                }
                else if (Envir.Random.Next(10) == 0)
                {
                    if (DropItem(temp, Settings.DropRange))
                    {
                        Info.Inventory[i] = null;
                        Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = temp.Count });
                    }
                }
            }

            RefreshStats();
        }
        private void RedDeathDrop(MapObject killer)
        {
            if (killer == null || killer.Race != ObjectType.Player)
            {
                UserItem temp = Info.Equipment[(int)EquipmentSlot.Stone];
                if (temp != null)
                {
                    Info.Equipment[(int)EquipmentSlot.Stone] = null;
                    Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = temp.Count });
                }


                for (int i = 0; i < Info.Equipment.Length; i++)
                {
                    temp = Info.Equipment[i];

                    if (temp == null) continue;
                    if (temp.Info.Bind.HasFlag(BindMode.DontDeathdrop)) continue;

                    if (temp.Count > 1)
                    {
                        int percent = Envir.RandomomRange(10, 4);

                        uint count = (uint)Math.Ceiling(temp.Count / 10F * percent);

                        if (count > temp.Count)
                            throw new ArgumentOutOfRangeException();

                        UserItem temp2 = Envir.CreateFreshItem(temp.Info);
                        temp2.Count = count;

                        if (DropItem(temp2, Settings.DropRange))
                        {
                            if (count == temp.Count)
                                Info.Equipment[i] = null;

                            Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = count });
                            temp.Count -= count;
                        }
                    }
                    else if (Envir.Random.Next(10) == 0)
                    {
                        if (DropItem(temp, Settings.DropRange))
                        {
                            Info.Equipment[i] = null;
                            Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = temp.Count });
                        }
                    }
                }

            }

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                UserItem temp = Info.Inventory[i];

                if (temp == null) continue;
                if (temp.Info.Bind.HasFlag(BindMode.DontDeathdrop)) continue;

                if (!DropItem(temp, Settings.DropRange)) continue;

                Info.Inventory[i] = null;
                Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = temp.Count });
            }

            RefreshStats();
        }


        public override void WinExp(uint amount, uint targetLevel = 0)
        {
            int expPoint;

            if (Level < targetLevel + 10 || !Settings.ExpMobLevelDifference)
            {
                expPoint = (int)amount;
            }
            else
            {
                expPoint = (int)amount - (int)Math.Round(Math.Max(amount / 15, 1) * ((double)Level - (targetLevel + 10)));
            }

            if (expPoint <= 0) expPoint = 1;

            expPoint = (int)(expPoint * Settings.ExpRate);

            //party
            float[] partyExpRate = { 1.0F, 1.3F, 1.4F, 1.5F, 1.6F, 1.7F, 1.8F, 1.9F, 2F, 2.1F, 2.2F };

            if (GroupMembers != null)
            {
                int sumLevel = 0;
                int nearCount = 0;
                for (int i = 0; i < GroupMembers.Count; i++)
                {
                    PlayerObject player = GroupMembers[i];

                    if (Functions.InRange(player.CurrentLocation, CurrentLocation, Globals.DataRange))
                    {
                        sumLevel += player.Level;
                        nearCount++;
                    }
                }

                for (int i = 0; i < GroupMembers.Count; i++)
                {
                    PlayerObject player = GroupMembers[i];
                    if (player.CurrentMap == CurrentMap &&
                        Functions.InRange(player.CurrentLocation, CurrentLocation, Globals.DataRange) && !player.Dead)
                    {
                        player.GainExp((uint)((float)expPoint * partyExpRate[nearCount - 1] * (float)player.Level / (float)sumLevel));
                    }
                }
            }
            else
                GainExp((uint)expPoint);
        }

        public void GainExp(uint amount)
        {
            if (!CanGainExp) return;

            if (amount == 0) return;

            if (ExpRateOffset > 0)
                amount += (uint)(amount * (ExpRateOffset / 100));

            Experience += amount;

            Enqueue(new S.GainExperience { Amount = amount });


            for (int i = 0; i < Pets.Count; i++)
            {
                MonsterObject monster = Pets[i];
                if (monster.CurrentMap == CurrentMap && Functions.InRange(monster.CurrentLocation, CurrentLocation, Globals.DataRange) && !monster.Dead)
                    monster.PetExp(amount);
            }

            if (MyGuild != null)
                MyGuild.GainExp(amount);

            if (Experience < MaxExperience) return;
            if (Level >= byte.MaxValue) return;

            //Calculate increased levels
            var experience = Experience;

            while (experience > MaxExperience)
            {
                Level++;
                experience -= MaxExperience;

                RefreshLevelStats();

                if (Level >= byte.MaxValue) break;
            }

            Experience = experience;

            LevelUp();
        }

        public void LevelUp()
        {
            RefreshStats();
            SetHP(MaxHP);
            SetMP(MaxMP);

            CallDefaultNPC(DefaultNPCType.LevelUp);

            Enqueue(new S.LevelChanged { Level = Level, Experience = Experience, MaxExperience = MaxExperience });
            Broadcast(new S.ObjectLeveled { ObjectID = ObjectID });
        }



        private static int FreeSpace(IList<UserItem> array)
        {
            int count = 0;

            for (int i = 0; i < array.Count; i++)
                if (array[i] == null) count++;

            return count;
        }

        private void AddQuestItem(UserItem item)
        {
            if (item.Info.StackSize > 1) //Stackable
            {
                for (int i = 0; i < Info.QuestInventory.Length; i++)
                {
                    UserItem temp = Info.QuestInventory[i];
                    if (temp == null || item.Info != temp.Info || temp.Count >= temp.Info.StackSize) continue;

                    if (item.Count + temp.Count <= temp.Info.StackSize)
                    {
                        temp.Count += item.Count;
                        return;
                    }
                    item.Count -= temp.Info.StackSize - temp.Count;
                    temp.Count = temp.Info.StackSize;
                }
            }

            for (int i = 0; i < Info.QuestInventory.Length; i++)
            {
                if (Info.QuestInventory[i] != null) continue;
                Info.QuestInventory[i] = item;
                return;
            }
        }

        private void AddItem(UserItem item)
        {
            if (item.Info.StackSize > 1) //Stackable
            {
                for (int i = 0; i < Info.Inventory.Length; i++)
                {
                    UserItem temp = Info.Inventory[i];
                    if (temp == null || item.Info != temp.Info || temp.Count >= temp.Info.StackSize) continue;

                    if (item.Count + temp.Count <= temp.Info.StackSize)
                    {
                        temp.Count += item.Count;
                        return;
                    }
                    item.Count -= temp.Info.StackSize - temp.Count;
                    temp.Count = temp.Info.StackSize;
                }
            }

            if (item.Info.Type == ItemType.Potion || item.Info.Type == ItemType.Scroll)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Info.Inventory[i] != null) continue;
                    Info.Inventory[i] = item;
                    return;
                }
            }
            else if (item.Info.Type == ItemType.Amulet)
            {
                for (int i = 4; i < 6; i++)
                {
                    if (Info.Inventory[i] != null) continue;
                    Info.Inventory[i] = item;
                    return;
                }
            }
            else
            {
                for (int i = 6; i < Info.Inventory.Length; i++)
                {
                    if (Info.Inventory[i] != null) continue;
                    Info.Inventory[i] = item;
                    return;
                }
            }

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                if (Info.Inventory[i] != null) continue;
                Info.Inventory[i] = item;
                return;
            }
        }
        private bool CorrectStartItem(ItemInfo info)
        {
            switch (Class)
            {
                case MirClass.Warrior:
                    if (!info.RequiredClass.HasFlag(RequiredClass.Warrior)) return false;
                    break;
                case MirClass.Wizard:
                    if (!info.RequiredClass.HasFlag(RequiredClass.Wizard)) return false;
                    break;
                case MirClass.Taoist:
                    if (!info.RequiredClass.HasFlag(RequiredClass.Taoist)) return false;
                    break;
                case MirClass.Assassin:
                    if (!info.RequiredClass.HasFlag(RequiredClass.Assassin)) return false;
                    break;
                case MirClass.Archer:
                    if (!info.RequiredClass.HasFlag(RequiredClass.Archer)) return false;
                    break;
                default:
                    return false;
            }

            switch (Gender)
            {
                case MirGender.Male:
                    if (!info.RequiredGender.HasFlag(RequiredGender.Male)) return false;
                    break;
                case MirGender.Female:
                    if (!info.RequiredGender.HasFlag(RequiredGender.Female)) return false;
                    break;
                default:
                    return false;
            }

            return true;
        }
        public void CheckItemInfo(ItemInfo info, bool dontLoop = false)
        {
            if ((dontLoop == false) && (info.ClassBased | info.LevelBased)) //send all potential data so client can display it
            {
                for (int i = 0; i < Envir.ItemInfoList.Count; i++)
                {
                    if ((Envir.ItemInfoList[i] != info) && (Envir.ItemInfoList[i].Name.StartsWith(info.Name)))
                        CheckItemInfo(Envir.ItemInfoList[i], true);
                }
            }

            if (Connection.SentItemInfo.Contains(info)) return;
            Enqueue(new S.NewItemInfo { Info = info });
            Connection.SentItemInfo.Add(info);
        }

        public void CheckItem(UserItem item)
        {
            CheckItemInfo(item.Info);

            for (int i = 0; i < item.Slots.Length; i++)
            {
                if (item.Slots[i] == null) continue;

                CheckItemInfo(item.Slots[i].Info);
            }
        }

        public void CheckQuestInfo(QuestInfo info)
        {
            if (Connection.SentQuestInfo.Contains(info)) return;
            Enqueue(new S.NewQuestInfo { Info = info.CreateClientQuestInfo() });
            Connection.SentQuestInfo.Add(info);
        }

        private void SetBind()
        {
            SafeZoneInfo szi = Envir.StartPoints[Envir.Random.Next(Envir.StartPoints.Count)];

            BindMapIndex = szi.Info.Index;
            BindLocation = szi.Location;
        }
        public void StartGame()
        {
            Map temp = Envir.GetMap(CurrentMapIndex);

            if (temp != null && temp.Info.NoReconnect)
            {
                Map temp1 = Envir.GetMapByNameAndInstance(temp.Info.NoReconnectMap);
                if (temp1 != null)
                {
                    temp = temp1;
                    CurrentLocation = GetRandomPoint(40, 0, temp);
                }
            }

            if (temp == null || !temp.ValidPoint(CurrentLocation))
            {
                temp = Envir.GetMap(BindMapIndex);

                if (temp == null || !temp.ValidPoint(BindLocation))
                {
                    SetBind();
                    temp = Envir.GetMap(BindMapIndex);

                    if (temp == null || !temp.ValidPoint(BindLocation))
                    {
                        StartGameFailed();
                        return;
                    }
                }
                CurrentMapIndex = BindMapIndex;
                CurrentLocation = BindLocation;
            }
            temp.AddObject(this);
            CurrentMap = temp;
            Envir.Players.Add(this);

            StartGameSuccess();

            //Call Login NPC
            CallDefaultNPC(DefaultNPCType.Login);

            //Call Daily NPC
            if(Info.NewDay)
            {
                CallDefaultNPC(DefaultNPCType.Daily);
            }
        }
        private void StartGameSuccess()
        {
            Connection.Stage = GameStage.Game;
            Enqueue(new S.StartGame { Result = 4 });
            ReceiveChat("Welcome to the Legend of Mir 2 C# Server.", ChatType.Hint);
            if (Info.GuildIndex != -1)
            {
                MyGuild = Envir.GetGuild(Info.GuildIndex);
                if (MyGuild == null)
                {
                    Info.GuildIndex = -1;
                    ReceiveChat("You have been removed from the guild.", ChatType.System);
                }
                else
                {
                    MyGuildRank = MyGuild.FindRank(Info.Name);
                    if (MyGuildRank == null)
                    {
                        MyGuild = null;
                        Info.GuildIndex = -1;
                        ReceiveChat("You have been removed from the guild.", ChatType.System);
                    }
                }
            }
            Spawned();

            SetLevelEffects();

            GetItemInfo();
            GetMapInfo();
            GetUserInfo();
            GetQuestInfo();

            GetCompletedQuests();

            GetMail();

            for (int i = 0; i < CurrentQuests.Count; i++)
            {
                QuestProgressInfo quest = CurrentQuests[i];

                if (quest.Info == null) //fail safe incase the quest was deleted
                {
                    CurrentQuests.RemoveAt(i);
                    continue;
                }

                quest.ResyncTasks();
                SendUpdateQuest(quest, QuestState.Add);
            }

            Enqueue(new S.BaseStatsInfo { Stats = Settings.ClassBaseStats[(byte)Class] });
            GetObjectsPassive();
            Enqueue(new S.TimeOfDay { Lights = Envir.Lights });
            Enqueue(new S.ChangeAMode { Mode = AMode });
            if (Class == MirClass.Wizard || Class == MirClass.Taoist)
                Enqueue(new S.ChangePMode { Mode = PMode });
            Enqueue(new S.SwitchGroup { AllowGroup = AllowGroup });

            Enqueue(new S.DefaultNPC { ObjectID = DefaultNPC.ObjectID });

            if (Info.Thrusting) Enqueue(new S.SpellToggle { Spell = Spell.Thrusting, CanUse = true });
            if (Info.HalfMoon) Enqueue(new S.SpellToggle { Spell = Spell.HalfMoon, CanUse = true });
            if (Info.CrossHalfMoon) Enqueue(new S.SpellToggle { Spell = Spell.CrossHalfMoon, CanUse = true });
            if (Info.DoubleSlash) Enqueue(new S.SpellToggle { Spell = Spell.DoubleSlash, CanUse = true });

            for (int i = 0; i < Info.Pets.Count; i++)
            {
                PetInfo info = Info.Pets[i];
                MonsterObject monster = MonsterObject.GetMonster(Envir.GetMonsterInfo(info.MonsterIndex));

                monster.PetLevel = info.Level;
                monster.MaxPetLevel = info.MaxPetLevel;
                monster.PetExperience = info.Experience;

                monster.Master = this;
                Pets.Add(monster);

                monster.RefreshAll();
                if (!monster.Spawn(CurrentMap, Back))
                    monster.Spawn(CurrentMap, CurrentLocation);

                monster.SetHP(info.HP);
            }

            Info.Pets.Clear();

            for (int i = 0; i < Info.Buffs.Count; i++)
            {
                Buff buff = Info.Buffs[i];
                buff.Caster = this;
                buff.ExpireTime += Envir.Time;
                
                AddBuff(buff);
            }

            Info.Buffs.Clear();

            if (MyGuild != null)
                MyGuild.PlayerLogged(this, true);

            SMain.Enqueue(string.Format("{0} has connected.", Info.Name));

        }
        private void StartGameFailed()
        {
            Enqueue(new S.StartGame { Result = 3 });
            CleanUp();
        }

        public void SetLevelEffects()
        {
            LevelEffects = LevelEffects.None;

            if (Info.Flags[990]) LevelEffects |= LevelEffects.Mist;
            if (Info.Flags[991]) LevelEffects |= LevelEffects.RedDragon;
            if (Info.Flags[992]) LevelEffects |= LevelEffects.BlueDragon;
        }

        public void Revive(uint hp, bool effect)
        {
            if (!Dead) return;

            Dead = false;
            SetHP((ushort)hp);

            CurrentMap.RemoveObject(this);
            Broadcast(new S.ObjectRemove { ObjectID = ObjectID });

            CurrentMap = this.CurrentMap;
            CurrentLocation = this.CurrentLocation;

            CurrentMap.AddObject(this);

            GetObjects();
            Enqueue(new S.Revived());
            Broadcast(new S.ObjectRevived { ObjectID = ObjectID, Effect = effect });
            Fishing = false;
            Enqueue(GetFishInfo());
        }
        public void TownRevive()
        {
            if (!Dead) return;

            Map temp = Envir.GetMap(BindMapIndex);
            Point bindLocation = BindLocation;

            if (Info.PKPoints >= 200)
            {
                temp = Envir.GetMapByNameAndInstance(Settings.PKTownMapName, 1);
                bindLocation = new Point(Settings.PKTownPositionX, Settings.PKTownPositionY);

                if (temp == null)
                {
                    temp = Envir.GetMap(BindMapIndex);
                    bindLocation = BindLocation;
                }
            }

            if (temp == null || !temp.ValidPoint(bindLocation)) return;

            Dead = false;
            SetHP(MaxHP);
            SetMP(MaxMP);
            RefreshStats();

            CurrentMap.RemoveObject(this);
            Broadcast(new S.ObjectRemove { ObjectID = ObjectID });

            CurrentMap = temp;
            CurrentLocation = BindLocation;

            CurrentMap.AddObject(this);

            Enqueue(new S.MapChanged
            {
                FileName = CurrentMap.Info.FileName,
                Title = CurrentMap.Info.Title,
                MiniMap = CurrentMap.Info.MiniMap,
                BigMap = CurrentMap.Info.BigMap,
                Lights = CurrentMap.Info.Light,
                Location = CurrentLocation,
                Direction = Direction,
                MapDarkLight = CurrentMap.Info.MapDarkLight
            });

            GetObjects();
            Enqueue(new S.Revived());
            Broadcast(new S.ObjectRevived { ObjectID = ObjectID, Effect = true });


            InSafeZone = true;
            Fishing = false;
            Enqueue(GetFishInfo());
        }

        private void GetItemInfo()
        {
            UserItem item;
            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                item = Info.Inventory[i];
                if (item == null) continue;
                //CheckItemInfo(item.Info);
                CheckItem(item);
            }

            for (int i = 0; i < Info.Equipment.Length; i++)
            {
                item = Info.Equipment[i];

                if (item == null) continue;
                //CheckItemInfo(item.Info);
                CheckItem(item);
            }

            for (int i = 0; i < Info.QuestInventory.Length; i++)
            {
                item = Info.QuestInventory[i];

                if (item == null) continue;
                CheckItem(item);
            }
        }
        private void GetUserInfo()
        {
            string guildname = MyGuild != null ? MyGuild.Name : "";
            string guildrank = MyGuild != null ? MyGuildRank.Name : "";
            S.UserInformation packet = new S.UserInformation
            {
                ObjectID = ObjectID,
                RealId = (uint)Info.Index,
                Name = Name,
                GuildName = guildname,
                GuildRank = guildrank,
                NameColour = GetNameColour(this),
                Class = Class,
                Gender = Gender,
                Level = Level,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = Hair,
                HP = HP,
                MP = MP,

                Experience = Experience,
                MaxExperience = MaxExperience,

                LevelEffects = LevelEffects,

                Inventory = new UserItem[Info.Inventory.Length],
                Equipment = new UserItem[Info.Equipment.Length],
                QuestInventory = new UserItem[Info.QuestInventory.Length],
                Gold = Account.Gold,
            };

            //Copy this method to prevent modification before sending packet information.
            for (int i = 0; i < Info.Magics.Count; i++)
                packet.Magics.Add(Info.Magics[i].CreateClientMagic());

            Info.Inventory.CopyTo(packet.Inventory, 0);
            Info.Equipment.CopyTo(packet.Equipment, 0);
            Info.QuestInventory.CopyTo(packet.QuestInventory, 0);

            //IntelligentCreature
            for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
                packet.IntelligentCreatures.Add(Info.IntelligentCreatures[i].CreateClientIntelligentCreature());
            packet.SummonedCreatureType = SummonedCreatureType;
            packet.CreatureSummoned = CreatureSummoned;

            Enqueue(packet);
        }
        private void GetMapInfo()
        {
            Enqueue(new S.MapInformation
            {
                FileName = CurrentMap.Info.FileName,
                Title = CurrentMap.Info.Title,
                MiniMap = CurrentMap.Info.MiniMap,
                Lights = CurrentMap.Info.Light,
                BigMap = CurrentMap.Info.BigMap,
                Lightning = CurrentMap.Info.Lightning,
                Fire = CurrentMap.Info.Fire,
                MapDarkLight = CurrentMap.Info.MapDarkLight
            });
        }

        private void GetQuestInfo()
        {
            for (int i = 0; i < Envir.QuestInfoList.Count; i++)
            {
                CheckQuestInfo(Envir.QuestInfoList[i]);
            }
        }
        private void GetObjects()
        {
            for (int y = CurrentLocation.Y - Globals.DataRange; y <= CurrentLocation.Y + Globals.DataRange; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = CurrentLocation.X - Globals.DataRange; x <= CurrentLocation.X + Globals.DataRange; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;
                    if (x < 0 || x >= CurrentMap.Width) continue;

                    Cell cell = CurrentMap.GetCell(x, y);

                    if (!cell.Valid || cell.Objects == null) continue;

                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];

                        //if (ob.Race == ObjectType.Player && ob.Observer) continue;

                        ob.Add(this);
                    }
                }
            }
        }
        private void GetObjectsPassive()
        {
            for (int y = CurrentLocation.Y - Globals.DataRange; y <= CurrentLocation.Y + Globals.DataRange; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = CurrentLocation.X - Globals.DataRange; x <= CurrentLocation.X + Globals.DataRange; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;
                    if (x < 0 || x >= CurrentMap.Width) continue;

                    Cell cell = CurrentMap.GetCell(x, y);

                    if (!cell.Valid || cell.Objects == null) continue;

                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if (ob == this) continue;

                        if(ob.Race == ObjectType.Deco)
                        {
                            var tt = 0;

                            tt++;
                        }
                        //if (ob.Race == ObjectType.Player && ob.Observer) continue;
                        if (ob.Race == ObjectType.Player)
                        {
                            PlayerObject Player = (PlayerObject)ob;
                            Enqueue(Player.GetInfoEx(this));
                        }
                        else if (ob.Race == ObjectType.Spell)
                        {
                            SpellObject obSpell = (SpellObject)ob;
                            if ((obSpell.Spell != Spell.ExplosiveTrap) || (IsFriendlyTarget(obSpell.Caster)))
                                Enqueue(ob.GetInfo());
                        }
                        else
                            Enqueue(ob.GetInfo());

                        if (ob.Race == ObjectType.Player || ob.Race == ObjectType.Monster)
                            ob.SendHealth(this);
                    }
                }
            }
        }

        #region Refresh Stats

        public void RefreshStats()
        {
            if (HasUpdatedBaseStats == false)
            {
                Enqueue(new S.BaseStatsInfo { Stats = Settings.ClassBaseStats[(byte)Class] });
                HasUpdatedBaseStats = true;
            }
            RefreshLevelStats();
            RefreshBagWeight();
            RefreshEquipmentStats();
            RefreshItemSetStats();
            RefreshMirSetStats();
            RefreshSkills();
            RefreshBuffs();
            RefreshStatCaps();
            RefreshMountStats();

            //Location Stats ?

            if (HP > MaxHP) SetHP(MaxHP);
            if (MP > MaxMP) SetMP(MaxMP);

            AttackSpeed = 1400 - ((ASpeed * 60) + Math.Min(370, (Level * 14)));



            if (AttackSpeed < 550) AttackSpeed = 550;
        }

        private void RefreshLevelStats()
        {
            MaxExperience = Level < Settings.ExperienceList.Count ? Settings.ExperienceList[Level - 1] : 0;
            MaxHP = 0; MaxMP = 0;
            MinAC = 0; MaxAC = 0;
            MinMAC = 0; MaxMAC = 0;
            MinDC = 0; MaxDC = 0;
            MinMC = 0; MaxMC = 0;
            MinSC = 0; MaxSC = 0;

            Accuracy = Settings.ClassBaseStats[(byte)Class].StartAccuracy;
            Agility = Settings.ClassBaseStats[(byte)Class].StartAgility;
            CriticalRate = Settings.ClassBaseStats[(byte)Class].StartCriticalRate;
            CriticalDamage = Settings.ClassBaseStats[(byte)Class].StartCriticalDamage;
            //Other Stats;
            MaxBagWeight = 0;
            MaxWearWeight = 0;
            MaxHandWeight = 0;
            ASpeed = 0;
            Luck = 0;
            LifeOnHit = 0;
            HpDrainRate = 0;
            Reflect = 0;
            MagicResist = 0;
            PoisonResist = 0;
            HealthRecovery = 0;
            SpellRecovery = 0;
            PoisonRecovery = 0;
            Holy = 0;
            Freezing = 0;
            PoisonAttack = 0;

            ExpRateOffset = 0;
            ItemDropRateOffset = 0;

            MaxHP = (ushort)Math.Min(ushort.MaxValue, 14 + (Level / Settings.ClassBaseStats[(byte)Class].HpGain + Settings.ClassBaseStats[(byte)Class].HpGainRate) * Level);

            MinAC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MinAc > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MinAc : 0);
            MaxAC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MaxAc > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MaxAc : 0);
            MinMAC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MinMac > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MinMac : 0);
            MaxMAC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MaxMac > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MaxMac : 0);
            MinDC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MinDc > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MinDc : 0);
            MaxDC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MaxDc > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MaxDc : 0);
            MinMC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MinMc > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MinMc : 0);
            MaxMC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MaxMc > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MaxMc : 0);
            MinSC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MinSc > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MinSc : 0);
            MaxSC = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].MaxSc > 0 ? Level / Settings.ClassBaseStats[(byte)Class].MaxSc : 0);
            CriticalRate = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].CritialRateGain > 0 ? CriticalRate + (Level / Settings.ClassBaseStats[(byte)Class].CritialRateGain) : CriticalRate);
            CriticalDamage = (byte)Math.Min(byte.MaxValue, Settings.ClassBaseStats[(byte)Class].CriticalDamageGain > 0 ? CriticalDamage + (Level / Settings.ClassBaseStats[(byte)Class].CriticalDamageGain) : CriticalDamage);
            MaxBagWeight = (ushort)(50 + Level / Settings.ClassBaseStats[(byte)Class].BagWeightGain * Level);
            MaxWearWeight = (byte)Math.Min(byte.MaxValue, 15 + Level / Settings.ClassBaseStats[(byte)Class].WearWeightGain * Level);
            MaxHandWeight = (byte)Math.Min(byte.MaxValue, 12 + Level / Settings.ClassBaseStats[(byte)Class].HandWeightGain * Level);

            switch (Class)
            {
                case MirClass.Warrior:
                    MaxHP = (ushort)Math.Min(ushort.MaxValue, 14 + (Level / Settings.ClassBaseStats[(byte)Class].HpGain + Settings.ClassBaseStats[(byte)Class].HpGainRate + Level / 20F) * Level);
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, 11 + (Level * 3.5F) + (Level * Settings.ClassBaseStats[(byte)Class].MpGainRate));
                    break;
                case MirClass.Wizard:
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, 13 + ((Level / 5F + 2F) * 2.2F * Level) + (Level * Settings.ClassBaseStats[(byte)Class].MpGainRate));
                    break;
                case MirClass.Taoist:
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, (13 + Level / 8F * 2.2F * Level) + (Level * Settings.ClassBaseStats[(byte)Class].MpGainRate));
                    break;
                case MirClass.Assassin:
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, (11 + Level * 5F) + (Level * Settings.ClassBaseStats[(byte)Class].MpGainRate));
                    break;
                case MirClass.Archer:
                    MaxMP = (ushort)Math.Min(ushort.MaxValue, (11 + Level * 4F) + (Level * Settings.ClassBaseStats[(byte)Class].MpGainRate));
                    break;
            }

        }

        private void RefreshBagWeight()
        {
            CurrentBagWeight = 0;

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                UserItem item = Info.Inventory[i];
                if (item != null)
                    CurrentBagWeight = (ushort)Math.Min(ushort.MaxValue, CurrentBagWeight + item.Weight);
            }
        }

        private void RefreshEquipmentStats()
        {
            short OldLooks_Weapon = Looks_Weapon;
            short OldLooks_Armour = Looks_Armour;
            short Old_MountType = MountType;
            byte OldLooks_Wings = Looks_Wings;
            byte OldLight = Light;

            Looks_Armour = 0;
            Looks_Weapon = -1;
            Looks_Wings = 0;
            Light = 0;
            CurrentWearWeight = 0;
            CurrentHandWeight = 0;
            MountType = -1;

            HasTeleportRing = false;
            HasProtectionRing = false;
            HasRevivalRing = false;
            HasClearRing = false;
            HasMuscleRing = false;
            HasParalysisRing = false;
            HasProbeNecklace = false;
            HasSkillNecklace = false;
            NoDuraLoss = false;
            FastRun = false;

            var skillsToAdd = new List<string>();
            var skillsToRemove = new List<string> { Settings.HealRing, Settings.FireRing };
            short Macrate = 0, Acrate = 0, HPrate = 0, MPrate = 0;
            ItemSets.Clear();
            MirSet.Clear();

            for (int i = 0; i < Info.Equipment.Length; i++)
            {
                UserItem temp = Info.Equipment[i];
                if (temp == null) continue;

                ItemInfo RealItem = Functions.GetRealItem(temp.Info, Info.Level, Info.Class, Envir.ItemInfoList);
                if (RealItem.Type == ItemType.Weapon || RealItem.Type == ItemType.Torch)
                    CurrentHandWeight = (byte)Math.Min(byte.MaxValue, CurrentHandWeight + temp.Weight);
                else
                    CurrentWearWeight = (byte)Math.Min(byte.MaxValue, CurrentWearWeight + temp.Weight);

                if (temp.CurrentDura == 0 && temp.Info.Durability > 0) continue;


                MinAC = (byte)Math.Min(byte.MaxValue, MinAC + RealItem.MinAC + temp.Awake.getAC());
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + RealItem.MaxAC + temp.AC + temp.Awake.getAC());
                MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + RealItem.MinMAC + temp.Awake.getMAC());
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + RealItem.MaxMAC + temp.MAC + temp.Awake.getMAC());

                MinDC = (byte)Math.Min(byte.MaxValue, MinDC + RealItem.MinDC + temp.Awake.getDC());
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + RealItem.MaxDC + temp.DC + temp.Awake.getDC());
                MinMC = (byte)Math.Min(byte.MaxValue, MinMC + RealItem.MinMC + temp.Awake.getMC());
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + RealItem.MaxMC + temp.MC + temp.Awake.getMC());
                MinSC = (byte)Math.Min(byte.MaxValue, MinSC + RealItem.MinSC + temp.Awake.getSC());
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + RealItem.MaxSC + temp.SC + temp.Awake.getSC());

                Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + RealItem.Accuracy + temp.Accuracy);
                Agility = (byte)Math.Min(byte.MaxValue, Agility + RealItem.Agility + temp.Agility);

                MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + RealItem.HP + temp.HP + temp.Awake.getHPMP());
                MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + RealItem.MP + temp.MP + temp.Awake.getHPMP());

                ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, ASpeed + temp.AttackSpeed + RealItem.AttackSpeed)));
                Luck = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, Luck + temp.Luck + RealItem.Luck)));

                MaxBagWeight = (ushort)Math.Max(ushort.MinValue, (Math.Min(ushort.MaxValue, MaxBagWeight + RealItem.BagWeight)));
                MaxWearWeight = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, MaxWearWeight + RealItem.WearWeight)));
                MaxHandWeight = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, MaxHandWeight + RealItem.HandWeight)));
                HPrate = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, HPrate + RealItem.HPrate));
                MPrate = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, MPrate + RealItem.MPrate));
                Acrate = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, Acrate + RealItem.MaxAcRate));
                Macrate = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, Macrate + RealItem.MaxMacRate));
                MagicResist = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, MagicResist + temp.MagicResist + RealItem.MagicResist)));
                PoisonResist = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, PoisonResist + temp.PoisonResist + RealItem.PoisonResist)));
                HealthRecovery = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, HealthRecovery + temp.HealthRecovery + RealItem.HealthRecovery)));
                SpellRecovery = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, SpellRecovery + temp.ManaRecovery + RealItem.SpellRecovery)));
                PoisonRecovery = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, PoisonRecovery + temp.PoisonRecovery + RealItem.PoisonRecovery)));
                CriticalRate = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, CriticalRate + temp.CriticalRate + RealItem.CriticalRate)));
                CriticalDamage = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, CriticalDamage + temp.CriticalDamage + RealItem.CriticalDamage)));
                Holy = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, Holy + RealItem.Holy)));
                Freezing = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, Freezing + temp.Freezing + RealItem.Freezing)));
                PoisonAttack = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, PoisonAttack + temp.PoisonAttack + RealItem.PoisonAttack)));
                Reflect = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, Reflect + RealItem.Reflect)));
                HpDrainRate = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, HpDrainRate + RealItem.HpDrainRate)));

                if (RealItem.Light > Light) Light = RealItem.Light;
                if (RealItem.Unique != SpecialItemMode.None)
                {
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Paralize)) HasParalysisRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Teleport)) HasTeleportRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Clearring)) HasClearRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Protection)) HasProtectionRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Revival)) HasRevivalRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Muscle)) HasMuscleRing = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Flame))
                    {
                        skillsToAdd.Add(Settings.FireRing);
                        skillsToRemove.Remove(Settings.FireRing);
                    }
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Healing))
                    {
                        skillsToAdd.Add(Settings.HealRing);
                        skillsToRemove.Remove(Settings.HealRing);
                    }
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Probe)) HasProbeNecklace = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.Skill)) HasSkillNecklace = true;
                    if (RealItem.Unique.HasFlag(SpecialItemMode.NoDuraLoss)) NoDuraLoss = true;
                }

                if(RealItem.CanFastRun)
                {
                    FastRun = true;
                }

                if (RealItem.Type == ItemType.Armour)
                {
                    Looks_Armour = RealItem.Shape;
                    Looks_Wings = RealItem.Effect;
                }

                if (RealItem.Type == ItemType.Weapon)
                    Looks_Weapon = RealItem.Shape;

                if (RealItem.Type == ItemType.Mount)
                {
                    MountType = RealItem.Shape;
                    //RealItem.Effect;
                }

                if (RealItem.Set == ItemSet.None) continue;

                //Normal Sets
                bool sameSetFound = false;
                foreach (var set in ItemSets.Where(set => set.Set == RealItem.Set && !set.Type.Contains(RealItem.Type)).TakeWhile(set => !set.SetComplete))
                {
                    set.Type.Add(RealItem.Type);
                    set.Count++;
                    sameSetFound = true;
                }

                if (!ItemSets.Any() || !sameSetFound)
                    ItemSets.Add(new ItemSets { Count = 1, Set = RealItem.Set, Type = new List<ItemType> { RealItem.Type } });

                //Mir Set
                if (RealItem.Set == ItemSet.Mir)
                {
                    if (!MirSet.Contains((EquipmentSlot)i))
                        MirSet.Add((EquipmentSlot)i);
                }
            }

            MaxHP = (ushort)Math.Min(ushort.MaxValue, (((double)HPrate / 100) + 1) * MaxHP);
            MaxMP = (ushort)Math.Min(ushort.MaxValue, (((double)MPrate / 100) + 1) * MaxMP);
            MaxAC = (byte)Math.Min(byte.MaxValue, (((double)Acrate / 100) + 1) * MaxAC);
            MaxMAC = (byte)Math.Min(byte.MaxValue, (((double)Macrate / 100) + 1) * MaxMAC);

            AddTempSkills(skillsToAdd);
            RemoveTempSkills(skillsToRemove);

            if (HasMuscleRing)
            {
                MaxBagWeight = (ushort)(MaxBagWeight * 2);
                MaxWearWeight = Math.Min(byte.MaxValue, (byte)(MaxWearWeight * 2));
                MaxHandWeight = Math.Min(byte.MaxValue, (byte)(MaxHandWeight * 2));
            }
            if ((OldLooks_Armour != Looks_Armour) || (OldLooks_Weapon != Looks_Weapon) || (OldLooks_Wings != Looks_Wings) || (OldLight != Light))
            {
                Broadcast(GetUpdateInfo());

                if ((OldLooks_Weapon == 49 || OldLooks_Weapon == 50) && (Looks_Weapon != 49 && Looks_Weapon != 50))
                {
                    Enqueue(GetFishInfo());
                }
            }

            if (Old_MountType != MountType)
            {
                RefreshMount();
            }
        }

        private void RefreshItemSetStats()
        {
            foreach (var s in ItemSets)
            {
                if ((s.Set == ItemSet.Smash) && (s.Type.Contains(ItemType.Ring)) && (s.Type.Contains(ItemType.Bracelet)))
                    ASpeed = (sbyte)Math.Min(sbyte.MaxValue, ASpeed + 2);
                if ((s.Set == ItemSet.Purity) && (s.Type.Contains(ItemType.Ring)) && (s.Type.Contains(ItemType.Bracelet)))
                    Holy = Math.Min(byte.MaxValue, (byte)(Holy + 3));
                if ((s.Set == ItemSet.HwanDevil) && (s.Type.Contains(ItemType.Ring)) && (s.Type.Contains(ItemType.Bracelet)))
                {
                    MaxWearWeight = (byte)Math.Min(byte.MaxValue, MaxWearWeight + 5);
                    MaxBagWeight = (byte)Math.Min(byte.MaxValue, MaxBagWeight + 20);
                }

                if (!s.SetComplete) continue;
                switch (s.Set)
                {
                    case ItemSet.Mundane:
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 50);
                        break;
                    case ItemSet.NokChi:
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 50);
                        break;
                    case ItemSet.TaoProtect:
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 30);
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 30);
                        break;
                    case ItemSet.RedOrchid:
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + 2);
                        HpDrainRate = (byte)Math.Min(byte.MaxValue, HpDrainRate + 10);
                        break;
                    case ItemSet.RedFlower:
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 50);
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP - 50);
                        break;
                    case ItemSet.Smash:
                        MinDC = (byte)Math.Min(byte.MaxValue, MinDC + 1);
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 3);
                        break;
                    case ItemSet.HwanDevil:
                        MinMC = (byte)Math.Min(byte.MaxValue, MinMC + 1);
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 2);
                        break;
                    case ItemSet.Purity:
                        MinSC = (byte)Math.Min(byte.MaxValue, MinSC + 1);
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 2);
                        //holy +2;
                        break;
                    case ItemSet.FiveString:
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + (((double)MaxHP / 100) * 30));
                        MinAC = (byte)Math.Min(byte.MaxValue, MinAC + 2);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 2);
                        break;
                    case ItemSet.Spirit:
                        MinDC = (byte)Math.Min(byte.MaxValue, MinDC + 2);
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 5);
                        ASpeed = (sbyte)Math.Min(sbyte.MaxValue, ASpeed + 2);
                        break;
                    case ItemSet.Bone:
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 2);
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                        break;
                    case ItemSet.Bug:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 1);
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                        PoisonResist = (byte)Math.Min(byte.MaxValue, PoisonResist + 1);
                        break;
                    case ItemSet.WhiteGold:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 2);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 2);
                        break;
                    case ItemSet.WhiteGoldH:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 3);
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 30);
                        ASpeed = (sbyte)Math.Min(int.MaxValue, ASpeed + 2);
                        break;
                    case ItemSet.RedJade:
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 2);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 2);
                        break;
                    case ItemSet.RedJadeH:
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 2);
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 40);
                        Agility = (byte)Math.Min(byte.MaxValue, Agility + 2);
                        break;
                    case ItemSet.Nephrite:
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 2);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                        break;
                    case ItemSet.NephriteH:
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 2);
                        MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 15);
                        MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 20);
                        Holy = (byte)Math.Min(byte.MaxValue, Holy + 1);
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + 1);
                        break;
                }
            }
        }

        private void RefreshMirSetStats()
        {
            if (MirSet.Count() == 10)
            {
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                MaxBagWeight = (byte)Math.Min(byte.MaxValue, MaxBagWeight + 70);
                Luck = (sbyte)Math.Min(sbyte.MaxValue, Luck + 2);
                ASpeed = (sbyte)Math.Min(int.MaxValue, ASpeed + 2);
                MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + 70);
                MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + 80);
                MagicResist = (byte)Math.Min(byte.MaxValue, MagicResist + 6);
                PoisonResist = (byte)Math.Min(byte.MaxValue, PoisonResist + 6);
            }

            if (MirSet.Contains(EquipmentSlot.RingL) && MirSet.Contains(EquipmentSlot.RingR))
            {
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
            }
            if (MirSet.Contains(EquipmentSlot.BraceletL) && MirSet.Contains(EquipmentSlot.BraceletR))
            {
                MinAC = (byte)Math.Min(byte.MaxValue, MinAC + 1);
                MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + 1);
            }
            if ((MirSet.Contains(EquipmentSlot.RingL) | MirSet.Contains(EquipmentSlot.RingR)) && (MirSet.Contains(EquipmentSlot.BraceletL) | MirSet.Contains(EquipmentSlot.BraceletR)) && MirSet.Contains(EquipmentSlot.Necklace))
            {
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
                MaxBagWeight = (byte)Math.Min(byte.MaxValue, MaxBagWeight + 30);
                MaxWearWeight = (byte)Math.Min(byte.MaxValue, MaxWearWeight + 17);
            }
            if (MirSet.Contains(EquipmentSlot.RingL) && MirSet.Contains(EquipmentSlot.RingR) && MirSet.Contains(EquipmentSlot.BraceletL) && MirSet.Contains(EquipmentSlot.BraceletR) && MirSet.Contains(EquipmentSlot.Necklace))
            {
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + 1);
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + 1);
                MaxBagWeight = (byte)Math.Min(byte.MaxValue, MaxBagWeight + 20);
                MaxWearWeight = (byte)Math.Min(byte.MaxValue, MaxWearWeight + 10);
            }
            if (MirSet.Contains(EquipmentSlot.Armour) && MirSet.Contains(EquipmentSlot.Helmet) && MirSet.Contains(EquipmentSlot.Weapon))
            {
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 2);
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                Agility = (byte)Math.Min(byte.MaxValue, Agility + 1);
            }
            if (MirSet.Contains(EquipmentSlot.Armour) && MirSet.Contains(EquipmentSlot.Boots) && MirSet.Contains(EquipmentSlot.Belt))
            {
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 1);
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                MaxHandWeight = (byte)Math.Min(byte.MaxValue, MaxHandWeight + 17);
            }
            if (MirSet.Contains(EquipmentSlot.Armour) && MirSet.Contains(EquipmentSlot.Boots) && MirSet.Contains(EquipmentSlot.Belt) && MirSet.Contains(EquipmentSlot.Helmet) && MirSet.Contains(EquipmentSlot.Weapon))
            {
                MinDC = (byte)Math.Min(byte.MaxValue, MinDC + 1);
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + 1);
                MinMC = (byte)Math.Min(byte.MaxValue, MinMC + 1);
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + 1);
                MinSC = (byte)Math.Min(byte.MaxValue, MinSC + 1);
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + 1);
                MaxHandWeight = (byte)Math.Min(byte.MaxValue, MaxHandWeight + 17);
            }
        }

        public void RefreshStatCaps()
        {
            MagicResist = Math.Min(Settings.MaxMagicResist, MagicResist);
            PoisonResist = Math.Min(Settings.MaxPoisonResist, PoisonResist);
            CriticalRate = Math.Min(Settings.MaxCriticalRate, CriticalRate);
            CriticalDamage = Math.Min(Settings.MaxCriticalDamage, CriticalDamage);
            Freezing = Math.Min(Settings.MaxFreezing, Freezing);
            PoisonAttack = Math.Min(Settings.MaxPoisonAttack, PoisonAttack);
            HealthRecovery = Math.Min(Settings.MaxHealthRegen, HealthRecovery);
            PoisonRecovery = Math.Min(Settings.MaxPoisonRecovery, PoisonRecovery);
            SpellRecovery = Math.Min(Settings.MaxManaRegen, SpellRecovery);
            HpDrainRate = Math.Min((byte)100, HpDrainRate);
        }

        public void RefreshMountStats()
        {
            if (!RidingMount || !Mount.HasMount) return;

            UserItem[] Slots = Mount.Slots;

            for (int i = 0; i < Slots.Length; i++)
            {
                UserItem temp = Slots[i];
                if (temp == null) continue;

                ItemInfo RealItem = Functions.GetRealItem(temp.Info, Info.Level, Info.Class, Envir.ItemInfoList);

                CurrentWearWeight = (byte)Math.Min(byte.MaxValue, CurrentWearWeight + temp.Weight);

                if (temp.CurrentDura == 0 && temp.Info.Durability > 0) continue;

                MinAC = (byte)Math.Min(byte.MaxValue, MinAC + RealItem.MinAC);
                MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + RealItem.MaxAC + temp.AC);
                MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + RealItem.MinMAC);
                MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + RealItem.MaxMAC + temp.MAC);

                MinDC = (byte)Math.Min(byte.MaxValue, MinDC + RealItem.MinDC);
                MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + RealItem.MaxDC + temp.DC);
                MinMC = (byte)Math.Min(byte.MaxValue, MinMC + RealItem.MinMC);
                MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + RealItem.MaxMC + temp.MC);
                MinSC = (byte)Math.Min(byte.MaxValue, MinSC + RealItem.MinSC);
                MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + RealItem.MaxSC + temp.SC);

                Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + RealItem.Accuracy + temp.Accuracy);
                Agility = (byte)Math.Min(byte.MaxValue, Agility + RealItem.Agility + temp.Agility);

                MaxHP = (ushort)Math.Min(ushort.MaxValue, MaxHP + RealItem.HP + temp.HP);
                MaxMP = (ushort)Math.Min(ushort.MaxValue, MaxMP + RealItem.MP + temp.MP);

                ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, ASpeed + temp.AttackSpeed + RealItem.AttackSpeed)));
                Luck = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, Luck + temp.Luck + RealItem.Luck)));
            }
        }

        #endregion

        private void AddTempSkills(IEnumerable<string> skillsToAdd)
        {
            foreach (var skill in skillsToAdd)
            {
                Spell spelltype;
                bool hasSkill = false;

                if (!Enum.TryParse(skill, out spelltype)) return;

                for (var i = Info.Magics.Count - 1; i >= 0; i--)
                    if (Info.Magics[i].Spell == spelltype) hasSkill = true;

                if (hasSkill) continue;

                var magic = new UserMagic(spelltype) { IsTempSpell = true };
                Info.Magics.Add(magic);
                Enqueue(magic.GetInfo());
            }
        }
        private void RemoveTempSkills(IEnumerable<string> skillsToRemove)
        {
            foreach (var skill in skillsToRemove)
            {
                Spell spelltype;
                if (!Enum.TryParse(skill, out spelltype)) return;

                for (var i = Info.Magics.Count - 1; i >= 0; i--)
                {
                    if (!Info.Magics[i].IsTempSpell || Info.Magics[i].Spell != spelltype) continue;

                    Info.Magics.RemoveAt(i);
                    Enqueue(new S.RemoveMagic { PlaceId = i });
                }
            }
        }

        private void RefreshSkills()
        {
            for (int i = 0; i < Info.Magics.Count; i++)
            {
                UserMagic magic = Info.Magics[i];
                switch (magic.Spell)
                {
                    case Spell.Fencing:
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + magic.Level * 3);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + (magic.Level + 1) * 3);
                        break;
                    case Spell.FatalSword:
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + magic.Level);
                        break;
                    case Spell.SpiritSword:
                        Accuracy = (byte)Math.Min(byte.MaxValue, Accuracy + magic.Level);
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + MaxSC * (magic.Level + 1) * 0.1F);
                        break;
                    case Spell.MentalState:
                        //Info.MentalStateLvl = magic.Level;
                        //for (int j = 0; j < Buffs.Count; j ++)
                        //{
                        //    if (Buffs[j].Type == BuffType.MentalState)
                        //        return;
                        //}
                        //AddBuff(new Buff { Type = BuffType.MentalState, Caster = this, ExpireTime = Envir.Time + 100, Infinite = true, Value = Info.MentalState});
                        break;
                }
            }
        }
        private void RefreshBuffs()
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                Buff buff = Buffs[i];

                switch (buff.Type)
                {
                    case BuffType.Haste:
                    case BuffType.Fury:
                        ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, ASpeed + buff.Value)));
                        break;
                    case BuffType.SwiftFeet:
                        ActiveSwiftFeet = true;
                        break;
                    case BuffType.LightBody:
                        Agility = (byte)Math.Min(byte.MaxValue, Agility + buff.Value);
                        break;
                    case BuffType.SoulShield:
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + buff.Value);
                        break;
                    case BuffType.BlessedArmour:
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + buff.Value);
                        break;
                    case BuffType.UltimateEnhancer:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + buff.Value);
                        break;
                    case BuffType.ProtectionField:
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + buff.Value);
                        break;
                    case BuffType.Rage:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + buff.Value);
                        break;
                    case BuffType.CounterAttack:
                        MinAC = (byte)Math.Min(byte.MaxValue, MinAC + buff.Value);
                        MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + buff.Value);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + buff.Value);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + buff.Value);
                        break;
                    case BuffType.Curse:

                        byte rMaxDC = (byte)(((int)MaxDC / 100) * buff.Value);
                        byte rMaxMC = (byte)(((int)MaxMC / 100) * buff.Value);
                        byte rMaxSC = (byte)(((int)MaxSC / 100) * buff.Value);
                        byte rASpeed = (byte)(((int)ASpeed / 100) * buff.Value);

                        MaxDC = (byte)Math.Max(byte.MinValue, MaxDC - rMaxDC);
                        MaxMC = (byte)Math.Max(byte.MinValue, MaxMC - rMaxMC);
                        MaxSC = (byte)Math.Max(byte.MinValue, MaxSC - rMaxSC);
                        ASpeed = (sbyte)Math.Min(sbyte.MaxValue, (Math.Max(sbyte.MinValue, ASpeed - rASpeed)));
                        break;
                    case BuffType.General:
                        ExpRateOffset = (float)Math.Min(float.MaxValue, ExpRateOffset + buff.Value);
                        ItemDropRateOffset = (float)Math.Min(float.MaxValue, ItemDropRateOffset + buff.Value);
                        break;
                    case BuffType.Exp:
                        ExpRateOffset = (float)Math.Min(float.MaxValue, ExpRateOffset + buff.Value);
                        break;
                    case BuffType.Drop:
                        ItemDropRateOffset = (float)Math.Min(float.MaxValue, ItemDropRateOffset + buff.Value);
                        break;
                    case BuffType.Gold:
                        GoldDropRateOffset = (float)Math.Min(float.MaxValue, GoldDropRateOffset + buff.Value);
                        break;
                    case BuffType.Impact:
                        MaxDC = (byte)Math.Min(byte.MaxValue, MaxDC + buff.Value);
                        break;
                    case BuffType.Magic:
                        MaxMC = (byte)Math.Min(byte.MaxValue, MaxMC + buff.Value);
                        break;
                    case BuffType.Taoist:
                        MaxSC = (byte)Math.Min(byte.MaxValue, MaxSC + buff.Value);
                        break;
                    case BuffType.Storm:
                        ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, ASpeed + buff.Value)));
                        break;
                    case BuffType.HealthAid:
                        HP = (ushort)Math.Min(ushort.MaxValue, HP + buff.Value);
                        break;
                    case BuffType.ManaAid:
                        MP = (ushort)Math.Min(ushort.MaxValue, MP + buff.Value);
                        break;
                    case BuffType.WonderShield:
                        MinAC = (byte)Math.Min(byte.MaxValue, MinAC + buff.Value);
                        MaxAC = (byte)Math.Min(byte.MaxValue, MaxAC + buff.Value);
                        break;
                    case BuffType.MagicWonderShield:
                        MinMAC = (byte)Math.Min(byte.MaxValue, MinMAC + buff.Value);
                        MaxMAC = (byte)Math.Min(byte.MaxValue, MaxMAC + buff.Value);
                        break;
                    case BuffType.BagWeight:
                        MaxBagWeight = (byte)Math.Min(byte.MaxValue, MaxBagWeight + buff.Value);
                        break;
                }

            }
        }

        public void RefreshNameColour()
        {
            Color colour = Color.White;
            
            if (PKPoints >= 200)
                colour = Color.Red;
            else if (Envir.Time < BrownTime)
                colour = Color.SaddleBrown;
            else if (PKPoints >= 100)
                colour = Color.Yellow;
            
            if (colour == NameColour) return;

            
            NameColour = colour;
            if ((MyGuild == null) || (!MyGuild.IsAtWar()))
                Enqueue(new S.ColourChanged { NameColour = NameColour });

            BroadcastColourChange();
        }

        public Color GetNameColour(PlayerObject player)
        {
            if (player == null) return NameColour;
            if (MyGuild != null)
                if (MyGuild.IsAtWar())
                    if (player.MyGuild == MyGuild)
                        return Color.Blue;
                    else
                        if (MyGuild.IsEnemy(player.MyGuild))
                            return Color.Orange;
            return NameColour;
        }

        public void BroadcastColourChange()
        {
            if (CurrentMap == null) return;

            for (int i = CurrentMap.Players.Count - 1; i >= 0; i--)
            {
                PlayerObject player = CurrentMap.Players[i];
                if (player == this) continue;

                if (Functions.InRange(CurrentLocation, player.CurrentLocation, Globals.DataRange))
                    player.Enqueue(new S.ObjectColourChanged { ObjectID = ObjectID, NameColour = GetNameColour(player) });
            }
        }

        public override void BroadcastInfo()
        {
            Packet p;
            if (CurrentMap == null) return;

            for (int i = CurrentMap.Players.Count - 1; i >= 0; i--)
            {
                PlayerObject player = CurrentMap.Players[i];
                if (player == this) continue;

                if (Functions.InRange(CurrentLocation, player.CurrentLocation, Globals.DataRange))
                {
                    p = GetInfoEx(player);
                    if (p != null)
                        player.Enqueue(p);
                }
            }
        }

        public void Chat(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            SMain.EnqueueChat(string.Format("{0}: {1}", Name, message));


            if (GMLogin)
            {
                if (message == GMPassword)
                {
                    IsGM = true;
                    SMain.Enqueue(string.Format("{0} is now a GM", Name));
                    ReceiveChat("You have been made a GM", ChatType.System);
                }
                else
                {
                    SMain.Enqueue(string.Format("{0} attempted a GM login", Name));
                    ReceiveChat("Incorrect login password", ChatType.System);
                }
                GMLogin = false;
                return;
            }

            if (Info.ChatBanned)
            {
                if (Info.ChatBanExpiryDate > DateTime.Now)
                {
                    ReceiveChat("You are currently banned from chatting.", ChatType.System);
                    return;
                }

                Info.ChatBanned = false;
            }

            string[] parts;

            message = message.Replace("$pos", Functions.PointToString(CurrentLocation));


            Packet p;
            if (message.StartsWith("/"))
            {
                //Private Message
                message = message.Remove(0, 1);
                parts = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0) return;

                PlayerObject player = Envir.GetPlayer(parts[0]);

                if (player == null)
                {
                    IntelligentCreatureObject creature = GetCreatureByName(parts[0]);
                    if (creature != null)
                    {
                        creature.ReceiveChat(message.Remove(0, parts[0].Length), ChatType.WhisperIn);
                        return;
                    }
                    ReceiveChat(string.Format("Could not find {0}.", parts[0]), ChatType.System);
                    return;
                }

                ReceiveChat(string.Format("/{0}", message), ChatType.WhisperOut);
                player.ReceiveChat(string.Format("{0}=>{1}", Name, message.Remove(0, parts[0].Length)), ChatType.WhisperIn);
            }
            else if (message.StartsWith("!!"))
            {
                if (GroupMembers == null) return;
                //Group
                message = String.Format("{0}:{1}", Name, message.Remove(0, 2));

                p = new S.ObjectChat { ObjectID = ObjectID, Text = message, Type = ChatType.Group };

                for (int i = 0; i < GroupMembers.Count; i++)
                    GroupMembers[i].Enqueue(p);
            }
            else if (message.StartsWith("!~"))
            {
                if (MyGuild == null) return;

                //Guild
                message = message.Remove(0, 2);
                MyGuild.SendMessage(String.Format("{0}: {1}", Name, message));

            }
            else if (message.StartsWith("!"))
            {
                //Shout
                if (Envir.Time < ShoutTime)
                {
                    ReceiveChat(string.Format("You cannot shout for another {0} seconds.", Math.Ceiling((ShoutTime - Envir.Time) / 1000D)), ChatType.System);
                    return;
                }
                if (Level < 8)
                {
                    ReceiveChat("You need to be level 8 before you can shout.", ChatType.System);
                    return;
                }

                ShoutTime = Envir.Time + 10000;
                message = String.Format("(!){0}:{1}", Name, message.Remove(0, 1));

                p = new S.Chat { Message = message, Type = ChatType.Shout };

                //Envir.Broadcast(p);
                for (int i = 0; i < CurrentMap.Players.Count; i++)
                {
                    if (!Functions.InRange(CurrentLocation, CurrentMap.Players[i].CurrentLocation, Globals.DataRange * 2)) continue;
                    CurrentMap.Players[i].Enqueue(p);
                }

            }
            else if (message.StartsWith("@!"))
            {
                if (!IsGM) return;

                message = String.Format("(*){0}:{1}", Name, message.Remove(0, 2));

                p = new S.Chat { Message = message, Type = ChatType.Announcement };

                Envir.Broadcast(p);
            }
            else if (message.StartsWith("@"))
            {   //Command
                message = message.Remove(0, 1);
                parts = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0) return;

                PlayerObject player;
                CharacterInfo data;
                String hintstring;
                UserItem item;

                switch (parts[0].ToUpper())
                {
                    case "LOGIN":
                        GMLogin = true;
                        ReceiveChat("Please type the GM Password", ChatType.Hint);
                        return;

                    case "KILL":
                        if (!IsGM) return;

                        if (parts.Length >= 2)
                        {
                            player = Envir.GetPlayer(parts[1]);

                            if (player == null)
                            {
                                ReceiveChat(string.Format("Could not find {0}", parts[0]), ChatType.System);
                                return;
                            }
                            if (!player.GMNeverDie) player.Die();
                        }
                        else
                        {
                            if (!CurrentMap.ValidPoint(Front)) return;

                            Cell cell = CurrentMap.GetCell(Front);

                            if (cell == null || cell.Objects == null) return;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];

                                switch (ob.Race)
                                {
                                    case ObjectType.Player:
                                    case ObjectType.Monster:
                                        if (ob.Dead) continue;
                                        ob.EXPOwner = this;
                                        ob.ExpireTime = Envir.Time + MonsterObject.EXPOwnerDelay;
                                        ob.Die();
                                        break;
                                    default:
                                        continue;
                                }
                            }
                        }
                        return;

                    case "RESTORE":
                        if (!IsGM || parts.Length < 2) return;

                        data = Envir.GetCharacterInfo(parts[1]);

                        if (data == null)
                        {
                            ReceiveChat(string.Format("Player {0} was not found", parts[1]), ChatType.System);
                            return;
                        }

                        if (!data.Deleted) return;
                        data.Deleted = false;

                        ReceiveChat(string.Format("Player {0} has been restored by", data.Name), ChatType.System);
                        SMain.Enqueue(string.Format("Player {0} has been restored by {1}", data.Name, Name));

                        break;

                    case "CHANGEGENDER":
                        if (!IsGM) return;

                        data = parts.Length < 2 ? Info : Envir.GetCharacterInfo(parts[1]);

                        if (data == null) return;

                        switch (data.Gender)
                        {
                            case MirGender.Male:
                                data.Gender = MirGender.Female;
                                break;
                            case MirGender.Female:
                                data.Gender = MirGender.Male;
                                break;
                        }

                        ReceiveChat(string.Format("Player {0} has been changed to {1}", data.Name, data.Gender), ChatType.System);
                        SMain.Enqueue(string.Format("Player {0} has been changed to {1} by {2}", data.Name, data.Gender, Name));

                        if (data.Player != null)
                            data.Player.Connection.LogOut();

                        break;

                    case "LEVEL":
                        if (!IsGM || parts.Length < 2) return;

                        byte level;
                        byte old;
                        if (parts.Length >= 3)
                        {
                            if (byte.TryParse(parts[2], out level))
                            {
                                if (level == 0) return;
                                player = Envir.GetPlayer(parts[1]);
                                if (player == null) return;
                                old = player.Level;
                                player.Level = level;
                                player.LevelUp();

                                ReceiveChat(string.Format("Player {0} has been Leveled {1} -> {2}.", player.Name, old, player.Level), ChatType.System);
                                SMain.Enqueue(string.Format("Player {0} has been Leveled {1} -> {2} by {3}", player.Name, old, player.Level, Name));
                                return;
                            }
                        }
                        else
                        {
                            if (byte.TryParse(parts[1], out level))
                            {
                                if (level == 0) return;
                                old = Level;
                                Level = level;
                                LevelUp();

                                ReceiveChat(string.Format("Leveled {0} -> {1}.", old, Level), ChatType.System);
                                SMain.Enqueue(string.Format("Player {0} has been Leveled {1} -> {2} by {3}", Name, old, Level, Name));
                                return;
                            }
                        }

                        ReceiveChat("Could not level player", ChatType.System);
                        return;

                    case "MAKE":
                        if (!IsGM || parts.Length < 2) return;

                        ItemInfo iInfo = Envir.GetItemInfo(parts[1]);
                        if (iInfo == null) return;

                        uint count = 1;
                        if (parts.Length >= 3 && !uint.TryParse(parts[2], out count))
                            count = 1;

                        var tempCount = count;

                        while (count > 0)
                        {
                            if (iInfo.StackSize >= count)
                            {
                                item = Envir.CreateDropItem(iInfo);
                                item.Count = count;

                                if (CanGainItem(item, false)) GainItem(item);

                                return;
                            }
                            item = Envir.CreateDropItem(iInfo);
                            item.Count = iInfo.StackSize;
                            count -= iInfo.StackSize;

                            if (!CanGainItem(item, false)) return;
                            GainItem(item);
                        }

                        ReceiveChat(string.Format("{0} x{1} has been created.", iInfo.Name, tempCount), ChatType.System);
                        SMain.Enqueue(string.Format("Player {0} has attempted to Create {1} x{2}", Name, iInfo.Name, tempCount));
                        break;

                    case "CLEARBAG":
                        if (!IsGM) return;
                        player = this;

                        if (parts.Length >= 2)
                            player = Envir.GetPlayer(parts[1]);

                        if (player == null) return;
                        for (int i = 0; i < player.Info.Inventory.Length; i++)
                        {
                            item = player.Info.Inventory[i];
                            if (item == null) continue;

                            player.Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = item.Count });
                            player.Info.Inventory[i] = null;
                        }
                        break;

                    case "SUPERMAN":
                        if (!IsGM) return;

                        GMNeverDie = !GMNeverDie;

                        hintstring = GMNeverDie ? "Invincible Mode." : "Normal Mode.";
                        ReceiveChat(hintstring, ChatType.Hint);
                        break;

                    case "GAMEMASTER":
                        if (!IsGM) return;

                        GMGameMaster = !GMGameMaster;

                        hintstring = GMGameMaster ? "GameMaster Mode." : "Normal Mode.";
                        ReceiveChat(hintstring, ChatType.Hint);
                        break;

                    case "OBSERVER":
                        if (!IsGM) return;
                        Observer = !Observer;

                        hintstring = Observer ? "Observer Mode." : "Normal Mode.";
                        ReceiveChat(hintstring, ChatType.Hint);
                        break;
                    case "ALLOWGUILD":
                        EnableGuildInvite = !EnableGuildInvite;
                        hintstring = EnableGuildInvite ? "Guild invites enabled." : "Guild invites disabled.";
                        ReceiveChat(hintstring, ChatType.Hint);
                        break;
                    case "RECALL":
                        if (!IsGM) return;

                        if (parts.Length < 2) return;
                        player = Envir.GetPlayer(parts[1]);

                        if (player == null) return;

                        player.Teleport(CurrentMap, Front);
                        break;
                    case "ENABLEGROUPRECALL":
                        EnableGroupRecall = !EnableGroupRecall;
                        hintstring = EnableGroupRecall ? "Group Recall Enabled." : "Group Recall Disabled.";
                        ReceiveChat(hintstring, ChatType.Hint);
                        break;

                    case "GROUPRECALL":
                        if (GroupMembers == null || GroupMembers[0] != this || Dead)
                            return;

                        if (CurrentMap.Info.NoRecall)
                        {
                            ReceiveChat("You cannot recall people on this map", ChatType.System);
                            return;
                        }

                        if (Envir.Time < LastRecallTime)
                        {
                            ReceiveChat(string.Format("You cannot recall for another {0} seconds", (LastRecallTime - Envir.Time) / 1000), ChatType.System);
                            return;
                        }

                        if (ItemSets.Any(set => set.Set == ItemSet.Recall && set.SetComplete))
                        {
                            LastRecallTime = Envir.Time + 180000;
                            for (var i = 1; i < GroupMembers.Count(); i++)
                            {
                                if (GroupMembers[i].EnableGroupRecall)
                                    GroupMembers[i].Teleport(CurrentMap, CurrentLocation);
                                else
                                    GroupMembers[i].ReceiveChat("A recall was attempted without your permission",
                                        ChatType.System);
                            }
                        }
                        break;
                    case "RECALLMEMBER":
                        if (GroupMembers == null || GroupMembers[0] != this)
                        {
                            ReceiveChat("You are not a group leader.", ChatType.System);
                            return;
                        }

                        if (Dead)
                        {
                            ReceiveChat("You cannot recall when you are dead.", ChatType.System);
                            return;
                        }

                        if (Envir.Time < LastRecallTime)
                        {
                            ReceiveChat(string.Format("You cannot recall for another {0} seconds", (LastRecallTime - Envir.Time) / 1000), ChatType.System);
                            return;
                        }
                        if (ItemSets.Any(set => set.Set == ItemSet.Recall && set.SetComplete))
                        {
                            if (parts.Length < 2) return;
                            player = Envir.GetPlayer(parts[1]);

                            if (player == null || !IsMember(player) || this == player)
                            {
                                ReceiveChat((string.Format("Player {0} could not be found", parts[1])), ChatType.System);
                                return;
                            }
                            if (!player.EnableGroupRecall)
                            {
                                player.ReceiveChat("A recall was attempted without your permission",
                                        ChatType.System);
                                ReceiveChat((string.Format("{0} is blocking grouprecall", player.Name)), ChatType.System);
                                return;
                            }
                            LastRecallTime = Envir.Time + 60000;

                            if (!player.Teleport(CurrentMap, Front))
                                player.Teleport(CurrentMap, CurrentLocation);
                        }
                        else
                        {
                            ReceiveChat("You cannot recall without a recallset.", ChatType.System);
                            return;
                        }
                        break;

                    case "TIME":
                        ReceiveChat(string.Format("The time is : {0}", DateTime.Now.ToString("hh:mm tt")), ChatType.System);
                        break;

                    case "ROLL":
                        int diceNum = Envir.Random.Next(5) + 1;

                        if (GroupMembers == null) { return; }

                        for (int i = 0; i < GroupMembers.Count; i++)
                        {
                            PlayerObject playerSend = GroupMembers[i];
                            playerSend.ReceiveChat(string.Format("{0} has rolled a {1}", Name, diceNum), ChatType.Group);
                        }
                        break;

                    case "MAP":
                        var mapName = CurrentMap.Info.FileName;
                        var mapTitle = CurrentMap.Info.Title;
                        ReceiveChat((string.Format("You are currently in {0}. Map ID: {1}", mapTitle, mapName)), ChatType.System);
                        break;

                    case "MOVE":
                        if (!IsGM && !HasTeleportRing) return;
                        if (!IsGM && CurrentMap.Info.NoPosition)
                        {
                            ReceiveChat(("You cannot position move on this map"), ChatType.System);
                            return;
                        }
                        if (Envir.Time < LastTeleportTime)
                        {
                            ReceiveChat(string.Format("You cannot teleport for another {0} seconds", (LastTeleportTime - Envir.Time) / 1000), ChatType.System);
                            return;
                        }

                        int x, y;

                        if (parts.Length <= 2 || !int.TryParse(parts[1], out x) || !int.TryParse(parts[2], out y))
                        {
                            if (!IsGM)
                                LastTeleportTime = Envir.Time + 180000;
                            TeleportRandom(200, 0);
                            return;
                        }
                        if (!IsGM)
                            LastTeleportTime = Envir.Time + 180000;
                        Teleport(CurrentMap, new Point(x, y));
                        break;

                    case "MAPMOVE":
                        if (!IsGM) return;
                        if (parts.Length < 2) return;
                        var instanceID = 1; x = 0; y = 0;

                        if (parts.Length == 3 || parts.Length == 5)
                            int.TryParse(parts[2], out instanceID);

                        if (instanceID < 1) instanceID = 1;

                        var map = SMain.Envir.GetMapByNameAndInstance(parts[1], instanceID);
                        if (map == null)
                        {
                            ReceiveChat((string.Format("Map {0}:[{1}] could not be found", parts[1], instanceID)), ChatType.System);
                            return;
                        }

                        if (parts.Length == 4 || parts.Length == 5)
                        {
                            int.TryParse(parts[parts.Length - 2], out x);
                            int.TryParse(parts[parts.Length - 1], out y);
                        }

                        switch (parts.Length)
                        {
                            case 2:
                                ReceiveChat(TeleportRandom(200, 0, map) ? (string.Format("Moved to Map {0}", map.Info.FileName)) :
                                    (string.Format("Failed movement to Map {0}", map.Info.FileName)), ChatType.System);
                                break;
                            case 3:
                                ReceiveChat(TeleportRandom(200, 0, map) ? (string.Format("Moved to Map {0}:[{1}]", map.Info.FileName, instanceID)) :
                                    (string.Format("Failed movement to Map {0}:[{1}]", map.Info.FileName, instanceID)), ChatType.System);
                                break;
                            case 4:
                                ReceiveChat(Teleport(map, new Point(x, y)) ? (string.Format("Moved to Map {0} at {1}:{2}", map.Info.FileName, x, y)) :
                                    (string.Format("Failed movement to Map {0} at {1}:{2}", map.Info.FileName, x, y)), ChatType.System);
                                break;
                            case 5:
                                ReceiveChat(Teleport(map, new Point(x, y)) ? (string.Format("Moved to Map {0}:[{1}] at {2}:{3}", map.Info.FileName, instanceID, x, y)) :
                                    (string.Format("Failed movement to Map {0}:[{1}] at {2}:{3}", map.Info.FileName, instanceID, x, y)), ChatType.System);
                                break;
                        }
                        break;

                    case "GOTO":
                        if (!IsGM) return;

                        if (parts.Length < 2) return;
                        player = Envir.GetPlayer(parts[1]);

                        if (player == null) return;

                        Teleport(player.CurrentMap, player.CurrentLocation);
                        break;

                    case "MOB":
                        if (!IsGM) return;
                        if (parts.Length < 2)
                        {
                            ReceiveChat("Not enough parameters to spawn monster", ChatType.System);
                            return;
                        }

                        MonsterInfo mInfo = Envir.GetMonsterInfo(parts[1]);
                        if (mInfo == null)
                        {
                            ReceiveChat((string.Format("Monster {0} does not exist", parts[1])), ChatType.System);
                            return;
                        }

                        count = 1;
                        if (parts.Length >= 3)
                            if (!uint.TryParse(parts[2], out count)) count = 1;

                        for (int i = 0; i < count; i++)
                        {
                            MonsterObject monster = MonsterObject.GetMonster(mInfo);
                            if (monster == null) return;
                            monster.Spawn(CurrentMap, Front);
                        }

                        ReceiveChat((string.Format("Monster {0} x{1} has been spawned.", mInfo.Name, count)), ChatType.System);
                        break;

                    case "RECALLMOB":
                        if (!IsGM) return;
                        if (parts.Length < 2) return;

                        MonsterInfo mInfo2 = Envir.GetMonsterInfo(parts[1]);
                        if (mInfo2 == null) return;

                        count = 1;
                        byte petlevel = 0;

                        if (parts.Length > 2)
                            if (!uint.TryParse(parts[2], out count) || count > 50) count = 1;

                        if (parts.Length > 3)
                            if (!byte.TryParse(parts[3], out petlevel) || petlevel > 7) petlevel = 0;

                        for (int i = 0; i < count; i++)
                        {
                            MonsterObject monster = MonsterObject.GetMonster(mInfo2);
                            if (monster == null) return;
                            monster.PetLevel = petlevel;
                            monster.Master = this;
                            monster.MaxPetLevel = 7;
                            monster.Direction = Direction;
                            monster.ActionTime = Envir.Time + 1000;
                            monster.Spawn(CurrentMap, Front);
                            Pets.Add(monster);
                        }

                        ReceiveChat((string.Format("Pet {0} x{1} has been recalled.", mInfo2.Name, count)), ChatType.System);
                        break;

                    case "RELOADDROPS":
                        if (!IsGM) return;
                        foreach (var t in Envir.MonsterInfoList)
                            t.LoadDrops();
                        ReceiveChat("Drops Reloaded.", ChatType.Hint);
                        break;

                    case "RELOADNPCS":
                        if (!IsGM) return;

                        for (int i = 0; i < CurrentMap.NPCs.Count; i++)
                        {
                            CurrentMap.NPCs[i].LoadInfo(true);
                        }

                        DefaultNPC.LoadInfo(true);

                        ReceiveChat("NPCs Reloaded.", ChatType.Hint);
                        break;

                    case "GIVEGOLD":
                        if (!IsGM) return;
                        if (parts.Length < 2) return;

                        player = this;

                        if (parts.Length > 2)
                        {
                            if (!uint.TryParse(parts[2], out count)) return;
                            player = Envir.GetPlayer(parts[1]);

                            if (player == null)
                            {
                                ReceiveChat(string.Format("Player {0} was not found.", parts[1]), ChatType.System);
                                return;
                            }
                        }

                        else if (!uint.TryParse(parts[1], out count)) return;

                        if (count + player.Account.Gold >= uint.MaxValue)
                            count = uint.MaxValue - player.Account.Gold;

                        player.GainGold(count);
                        SMain.Enqueue(string.Format("Player {0} has been given {1} gold", player.Name, count));
                        break;

                    case "GIVEPEARLS":
                        if (!IsGM) return;
                        if (parts.Length < 2) return;

                        player = this;

                        if (parts.Length > 2)
                        {
                            if (!uint.TryParse(parts[2], out count)) return;
                            player = Envir.GetPlayer(parts[1]);

                            if (player == null)
                            {
                                ReceiveChat(string.Format("Player {0} was not found.", parts[1]), ChatType.System);
                                return;
                            }
                        }

                        else if (!uint.TryParse(parts[1], out count)) return;

                        if (count + player.Info.PearlCount >= int.MaxValue)
                            count = (uint)(int.MaxValue - player.Info.PearlCount);

                        player.IntelligentCreatureGainPearls((int)count);
                        if(count > 1)
                            SMain.Enqueue(string.Format("Player {0} has been given {1} pearls", player.Name, count));
                        else
                            SMain.Enqueue(string.Format("Player {0} has been given {1} pearl", player.Name, count));
                        break;

                    case "GIVESKILL":
                        if (!IsGM) return;
                        if (parts.Length < 3) return;

                        byte spellLevel = 0;

                        player = this;
                        Spell skill;

                        if (!Enum.TryParse(parts.Length > 3 ? parts[2] : parts[1], true, out skill)) return;

                        if (skill == Spell.None) return;

                        spellLevel = byte.TryParse(parts.Length > 3 ? parts[3] : parts[2], out spellLevel) ? Math.Min((byte)3, spellLevel) : (byte)0;

                        if (parts.Length > 3)
                        {
                            player = Envir.GetPlayer(parts[1]);

                            if (player == null)
                            {
                                ReceiveChat(string.Format("Player {0} was not found.", parts[1]), ChatType.System);
                                return;
                            }
                        }

                        if (player.Info.Magics.Any(e => e.Spell == skill))
                        {
                            player.Info.Magics.FirstOrDefault(e => e.Spell == skill).Level = spellLevel;
                            player.ReceiveChat(string.Format("Spell {0} changed to level {1}", skill.ToString(), spellLevel), ChatType.Hint);
                            return;
                        }

                        var magic = new UserMagic(skill) { Level = spellLevel };
                        player.ReceiveChat(string.Format("You have learned {0} at level {1}", skill.ToString(), spellLevel), ChatType.Hint);

                        if (player != this)
                        {
                            ReceiveChat(string.Format("{0} has learned {1} at level {2}", player.Name, skill.ToString(), spellLevel), ChatType.Hint);
                        }

                        player.Info.Magics.Add(magic);
                        player.Enqueue(magic.GetInfo());
                        player.RefreshStats();
                        break;

                    case "FIND":
                        if (!IsGM && !HasProbeNecklace) return;

                        if (Envir.Time < LastProbeTime)
                        {
                            ReceiveChat(string.Format("You cannot search for another {0} seconds", (LastProbeTime - Envir.Time) / 1000), ChatType.System);
                            return;
                        }

                        if (parts.Length < 2) return;
                        player = Envir.GetPlayer(parts[1]);

                        if (player == null)
                        {
                            ReceiveChat(parts[1] + " is not online", ChatType.System);
                            return;
                        }
                        if (player.CurrentMap == null) return;
                        if (!IsGM)
                            LastProbeTime = Envir.Time + 180000;
                        ReceiveChat((string.Format("{0} is located at {1} ({2},{3})", player.Name, player.CurrentMap.Info.Title, player.CurrentLocation.X, player.CurrentLocation.Y)), ChatType.System);
                        break;

                    case "LEAVEGUILD":
                        if (MyGuild == null) return;
                        if (MyGuildRank == null) return;
                        MyGuild.DeleteMember(this, Name);
                        break;

                    case "CREATEGUILD":
                        if (!IsGM) return;
                        if (parts.Length < 3) return;
                        player = Envir.GetPlayer(parts[1]);
                        if (player == null)
                        {
                            ReceiveChat(string.Format("Player {0} was not found.", parts[1]), ChatType.System);
                            return;
                        }
                        if (player.MyGuild != null)
                        {
                            ReceiveChat(string.Format("Player {0} is already in a guild.", parts[1]), ChatType.System);
                            return;
                        }
                        if ((parts[2].Length < 3) || (parts[2].Length > 20))
                        {
                            ReceiveChat("Guildname is restricted to 3-20 characters.", ChatType.System);
                            return;
                        }
                        GuildObject guild = Envir.GetGuild(parts[2]);
                        if (guild != null)
                        {
                            ReceiveChat(string.Format("Guild {0} already exists.", parts[2]), ChatType.System);
                            return;
                        }
                        player.CanCreateGuild = true;
                        if (player.CreateGuild(parts[2]))
                            ReceiveChat(string.Format("Successfully created guild {0}", parts[2]), ChatType.System);
                        else
                            ReceiveChat("Failed to create guild", ChatType.System);
                        player.CanCreateGuild = false;
                        break;

                    case "ALLOWTRADE":
                        AllowTrade = !AllowTrade;

                        if(AllowTrade)
                            ReceiveChat("You are now allowing trade", ChatType.System);
                        else
                            ReceiveChat("You are no longer allowing trade", ChatType.System);
                        break;

                    case "TRIGGER":
                        if (!IsGM) return;
                        if (parts.Length < 2) return;

                        if (parts.Length >= 3)
                        {
                            player = Envir.GetPlayer(parts[2]);

                            if (player == null)
                            {
                                ReceiveChat(string.Format("Player {0} was not found.", parts[2]), ChatType.System);
                                return;
                            }

                            player.CallDefaultNPC(DefaultNPCType.Trigger, parts[1]);
                            return;
                        }

                        foreach (var pl in Envir.Players)
                        {
                            pl.CallDefaultNPC(DefaultNPCType.Trigger, parts[1]);
                        }

                        break;

                    case "RIDE":
                        if (MountType > -1)
                        {
                            RidingMount = !RidingMount;

                            RefreshMount();
                        }
                        else
                            ReceiveChat("You haven't a mount...", ChatType.System);

                        break;

                    case "CLEARFLAGS":
                        if (!IsGM) return;

                        player = parts.Length > 1 ? Envir.GetPlayer(parts[1]) : this;

                        if (player == null)
                        {
                            ReceiveChat(parts[1] + " is not online", ChatType.System);
                            return;
                        }

                        for (int i = 0; i < player.Info.Flags.Length; i++)
                        {
                            player.Info.Flags[i] = false;
                        }
                        break;

                    case "CHANGECLASS": //@changeclass [Player] [Class]
                        if (!IsGM) return;

                        data = parts.Length <= 2 ? Info : Envir.GetCharacterInfo(parts[1]);

                        if (data == null) return;

                        MirClass mirClass;

                        if (!Enum.TryParse(parts[parts.Length - 1], true, out mirClass) || data.Class == mirClass) return;

                        data.Class = mirClass;

                        ReceiveChat(string.Format("Player {0} has been changed to {1}", data.Name, data.Class), ChatType.System);
                        SMain.Enqueue(string.Format("Player {0} has been changed to {1} by {2}", data.Name, data.Class, Name));

                        if (data.Player != null)
                            data.Player.Connection.LogOut();
                        break;

                    case "DIE":
                        LastHitter = null;
                        Die();
                        break;

                    case "HAIR":
                        if (!IsGM) return;
                        if(parts.Length < 2)
                            Info.Hair = (byte)SMain.Envir.Random.Next(0, 9);
                        else
                        {
                            byte tempByte = 0;

                            byte.TryParse(parts[1], out tempByte);

                            Info.Hair = tempByte;
                        }
                        break;

                    case "DECO":
                        if (!IsGM) return;
                        if (parts.Length < 2) return;

                        ushort tempShort = 0;

                        ushort.TryParse(parts[1], out tempShort);

                        DecoObject decoOb = new DecoObject
                        {
                            Image = tempShort,
                            CurrentMap = CurrentMap,
                            CurrentLocation = CurrentLocation,
                        };

                        CurrentMap.AddObject(decoOb);
                        decoOb.Spawned();

                        Enqueue(decoOb.GetInfo());
                        break;

                    case "ADJUSTPKPOINT":
                        if (!IsGM) return;
                        if (parts.Length < 2) return;
                        
                        int tempInt;

                        if(parts.Length > 2)
                        {
                            player = Envir.GetPlayer(parts[1]);

                            if (player == null) return;

                            
                            int.TryParse(parts[2], out tempInt);
                        }
                        else
                        {
                            player = this;
                            int.TryParse(parts[1], out tempInt);
                        }

                        player.PKPoints = tempInt;

                        break;

					case "AWAKENING":
                        {
                            if (!IsGM) return;
                            if (parts.Length < 3) return;

                            ItemType type;

                            if (!Enum.TryParse(parts[1], true, out type)) return;

                            AwakeType awakeType;

                            if (!Enum.TryParse(parts[2], true, out awakeType)) return;

                            foreach (UserItem temp in Info.Equipment)
                            {
                                if (temp == null) continue;

                                ItemInfo realItem = Functions.GetRealItem(temp.Info, Info.Level, Info.Class, Envir.ItemInfoList);

                                if (realItem.Type == type)
                                {
                                    Awake awake = temp.Awake;
                                    bool[] isHit;
                                    int result = awake.UpgradeAwake(temp, awakeType, out isHit);
                                    switch (result)
                                    {
                                        case -1:
                                            ReceiveChat(string.Format("{0} : Condition Error.", temp.Name), ChatType.System);
                                            break;
                                        case 0:
                                            ReceiveChat(string.Format("{0} : Upgrade Failed.", temp.Name), ChatType.System);
                                            break;
                                        case 1:
                                            ReceiveChat(string.Format("{0} : AWAKE Level {1}, value {2}~{3}.", temp.Name, awake.getAwakeLevel(), awake.getAwakeValue(), awake.getAwakeValue()), ChatType.System);
                                            p = new S.RefreshItem { Item = temp };
                                            Enqueue(p);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case "REMOVEAWAKENING":
                        {
                            if (!IsGM) return;
                            if (parts.Length < 2) return;

                            ItemType type;

                            if (!Enum.TryParse(parts[1], true, out type)) return;

                            foreach (UserItem temp in Info.Equipment)
                            {
                                if (temp == null) continue;

                                ItemInfo realItem = Functions.GetRealItem(temp.Info, Info.Level, Info.Class, Envir.ItemInfoList);

                                if (realItem.Type == type)
                                {
                                    Awake awake = temp.Awake;
                                    int result = awake.RemoveAwake();
                                    switch (result)
                                    {
                                        case 0:
                                            ReceiveChat(string.Format("{0} : Remove failed Level 0", temp.Name), ChatType.System);
                                            break;
                                        case 1:
                                            ReceiveChat(string.Format("{0} : Remove success. Level {1}", temp.Name, temp.Awake.getAwakeLevel()), ChatType.System);
                                            p = new S.RefreshItem { Item = temp };
                                            Enqueue(p);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        break;

                    case "STARTWAR":
                        if (!IsGM) return;
                        if (parts.Length < 2) return;

                        GuildObject enemyGuild = Envir.GetGuild(parts[1]);

                        if(MyGuild == null)
                        {
                            ReceiveChat("You are not in a guild.", ChatType.System);
                        }

                        if (MyGuild.Ranks[0] != MyGuildRank)
                        {
                            ReceiveChat("You must be a leader to start a war.", ChatType.System);
                            return;
                        }

                        if(enemyGuild == null)
                        {
                            ReceiveChat(string.Format("Could not find guild {0}.", parts[1]), ChatType.System);
                            return;
                        }

                        if(MyGuild == enemyGuild)
                        {
                            ReceiveChat("Cannot go to war with your own guild.", ChatType.System);
                            return;
                        }

                        if(MyGuild.WarringGuilds.Contains(enemyGuild))
                        {
                            ReceiveChat("Already at war with this guild.", ChatType.System);
                            return;
                        }

                        if(MyGuild.GoToWar(enemyGuild))
                        {
                            ReceiveChat(string.Format("You started a war with {0}.", parts[1]), ChatType.System);
                            enemyGuild.SendMessage(string.Format("{0} has started a war", MyGuild.Name), ChatType.System);
                        }

                        break;

                    case "ADDINVENTORY":
                        int openLevel = (int)((Info.Inventory.Length - 46) / 4);
                        uint openGold = (uint)(1000000 + openLevel * 1000000);

                        if (Account.Gold >= openGold)
                        {
                            Account.Gold -= openGold;
                            Enqueue(new S.LoseGold { Gold = openGold });
                            Enqueue(new S.ResizeInventory { Size = Info.ResizeInventory() });
                            ReceiveChat("Inventory size increased.", ChatType.System);
                        }
                        else
                        {
                            ReceiveChat("Not enough gold.", ChatType.System);
                        }
                        break;

                    case "INFO":
                        {
                            if (!IsGM) return;

                            MapObject ob = null;

                            if(parts.Length < 2)
                            {
                                Point target = Functions.PointMove(CurrentLocation, Direction, 1);
                                Cell cell = CurrentMap.GetCell(target);          

                                if (cell.Objects == null || cell.Objects.Count < 1) return;

                                ob = cell.Objects[0];
                            }
                            else
                            {
                                ob = Envir.GetPlayer(parts[1]);
                            }

                            if (ob == null) return;

                            switch(ob.Race)
                            {
                                case ObjectType.Player:
                                    PlayerObject plOb = (PlayerObject)ob;
                                    ReceiveChat("--Player Info--", ChatType.System2);
                                    ReceiveChat(string.Format("Name : {0}, Level : {1}, X : {2}, Y : {3}", plOb.Name, plOb.Level, plOb.CurrentLocation.X, plOb.CurrentLocation.Y), ChatType.System2);
                                    break;
                                case ObjectType.Monster:
                                    MonsterObject monOb = (MonsterObject)ob;
                                    ReceiveChat("--Monster Info--", ChatType.System2);
                                    ReceiveChat(string.Format("ID : {0}, Name : {1}", monOb.Info.Index, monOb.Name), ChatType.System2);
                                    ReceiveChat(string.Format("Level : {0}, X : {1}, Y : {2}", monOb.Level, monOb.CurrentLocation.X, monOb.CurrentLocation.Y), ChatType.System2);
                                    break;
                                case ObjectType.Merchant:
                                    NPCObject npcOb = (NPCObject)ob;
                                    ReceiveChat("--NPC Info--", ChatType.System2);
                                    ReceiveChat(string.Format("ID : {0}, Name : {1}", npcOb.Info.Index, npcOb.Name), ChatType.System2);
                                    ReceiveChat(string.Format("X : {0}, Y : {1}", ob.CurrentLocation.X, ob.CurrentLocation.Y), ChatType.System2);
                                    ReceiveChat(string.Format("File : {0}", npcOb.Info.FileName), ChatType.System2);
                                    break;
                            }
                        }
                        break;

                    case "CLEARQUESTS":
                        if (!IsGM) return;

                        player = parts.Length > 1 ? Envir.GetPlayer(parts[1]) : this;

                        if (player == null)
                        {
                            ReceiveChat(parts[1] + " is not online", ChatType.System);
                            return;
                        }

                        foreach (var quest in player.CurrentQuests)
                        {
                            SendUpdateQuest(quest, QuestState.Remove);
                        }

                        player.CurrentQuests.Clear();

                        player.CompletedQuests.Clear();
                        player.GetCompletedQuests();

                        break;

                    case "SETQUEST":
                        if (!IsGM) return;

                        if (parts.Length < 3) return;

                        player = parts.Length > 3 ? Envir.GetPlayer(parts[3]) : this;

                        if (player == null)
                        {
                            ReceiveChat(parts[3] + " is not online", ChatType.System);
                            return;
                        }

                        int questid = 0;
                        int questState = 0;

                        int.TryParse(parts[1], out questid);
                        int.TryParse(parts[2], out questState);

                        if (questid < 1) return;
                        
                        var activeQuest = player.CurrentQuests.FirstOrDefault(e => e.Index == questid);

                        //remove from active list
                        if (activeQuest != null)
                        {
                            player.SendUpdateQuest(activeQuest, QuestState.Remove);
                            player.CurrentQuests.Remove(activeQuest);
                        }

                        switch(questState)
                        {
                            case 0: //cancel
                                if (player.CompletedQuests.Contains(questid))
                                    player.CompletedQuests.Remove(questid);
                                break;
                            case 1: //complete
                                if (!player.CompletedQuests.Contains(questid))
                                    player.CompletedQuests.Add(questid);
                                break;
                        }

                        player.GetCompletedQuests();
                        break;

                    case "QUESTMIGRATE":
                        if (!IsGM) return;

                        foreach (var character in Envir.CharacterList)
                        {
                            for (int i = 1000; i < character.Flags.Length; i++)
                            {
                                if (character.Flags[i] == false) continue;
                                if (character.CompletedQuests.Contains(i - 1000)) continue;

                                character.CompletedQuests.Add(i - 1000);
                                character.Flags[i] = false;
                            }
                        }
                        break;

                    default:
                        foreach (string command in Envir.CustomCommands)
                        {
                            if (parts[0] != command) continue;
                            CallDefaultNPC(DefaultNPCType.CustomCommand, parts[0]);
                        }
                        break;
                }
            }
            else
            {
                message = String.Format("{0}:{1}", CurrentMap.Info.NoNames ? "?????" : Name, message);

                p = new S.ObjectChat { ObjectID = ObjectID, Text = message, Type = ChatType.Normal };

                Enqueue(p);
                Broadcast(p);
            }
        }


        public void Turn(MirDirection dir)
        {
            _stepCounter = 0;

            if (CanMove)
            {
                ActionTime = Envir.Time + TurnDelay;

                Direction = dir;
                if (CheckMovement(CurrentLocation)) return;

                SafeZoneInfo szi = CurrentMap.GetSafeZone(CurrentLocation);

                if (szi != null)
                {
                    BindLocation = szi.Location;
                    BindMapIndex = CurrentMapIndex;
                    InSafeZone = true;
                }
                else
                    InSafeZone = false;

                Cell cell = CurrentMap.GetCell(CurrentLocation);

                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    if (cell.Objects[i].Race != ObjectType.Spell) continue;
                    SpellObject ob = (SpellObject)cell.Objects[i];

                    ob.ProcessSpell(this);
                    break;
                }

                if (TradePartner != null) TradeCancel();

                Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            }

            Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
        }
        public void Harvest(MirDirection dir)
        {
            if (!CanMove)
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            ActionTime = Envir.Time + HarvestDelay;

            Direction = dir;

            Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
            Broadcast(new S.ObjectHarvest { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            Point front = Front;
            bool send = false;
            for (int d = 0; d <= 1; d++)
            {
                for (int y = front.Y - d; y <= front.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = front.X - d; x <= front.X + d; x += Math.Abs(y - front.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;
                        if (!CurrentMap.ValidPoint(x, y)) continue;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            if (ob.Race != ObjectType.Monster || !ob.Dead || ob.Harvested) continue;

                            if (ob.EXPOwner != null && ob.EXPOwner != this && !IsMember(ob))
                            {
                                send = true;
                                continue;
                            }

                            if (ob.Harvest(this)) return;
                        }
                    }
                }
            }

            if (send)
                ReceiveChat("You do not own any nearby carcasses.", ChatType.System);
        }
        public void Walk(MirDirection dir)
        {

            if (!CanMove || !CanWalk)
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            Point location = Functions.PointMove(CurrentLocation, dir, 1);

            if (!CurrentMap.ValidPoint(location))
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            Cell cell = CurrentMap.GetCell(location);
            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (!ob.Blocking || ob.CellTime >= Envir.Time) continue;

                    Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                    return;
                }

            if (Concentrating)//ArcherSpells - Elemental system
            {
                if (ConcentrateInterrupted)
                    ConcentrateInterruptTime = SMain.Envir.Time + 3000;// needs adjusting
                else
                {
                    ConcentrateInterruptTime = SMain.Envir.Time + 3000;// needs adjusting
                    ConcentrateInterrupted = true;
                    UpdateConcentration();//Update & send to client
                }
            }

            if (Hidden && !HasClearRing)
            {
                for (int i = 0; i < Buffs.Count; i++)
                {
                    switch (Buffs[i].Type)
                    {
                        case BuffType.Hiding:
                            Buffs[i].ExpireTime = 0;
                            break;
                    }
                }
            }

            Direction = dir;
            if (CheckMovement(location)) return;

            CurrentMap.GetCell(CurrentLocation).Remove(this);
            RemoveObjects(dir, 1);

            CurrentLocation = location;
            CurrentMap.GetCell(CurrentLocation).Add(this);
            AddObjects(dir, 1);

            _stepCounter++;

            SafeZoneInfo szi = CurrentMap.GetSafeZone(CurrentLocation);

            if (szi != null)
            {
                BindLocation = szi.Location;
                BindMapIndex = CurrentMapIndex;
                InSafeZone = true;
            }
            else
                InSafeZone = false;


            CellTime = Envir.Time + 500;
            ActionTime = Envir.Time + MoveDelay;

            if (TradePartner != null) TradeCancel();

            if (RidingMount) DecreaseMountLoyalty(1);

            Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
            Broadcast(new S.ObjectWalk { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            cell = CurrentMap.GetCell(CurrentLocation);

            for (int i = 0; i < cell.Objects.Count; i++)
            {
                if (cell.Objects[i].Race != ObjectType.Spell) continue;
                SpellObject ob = (SpellObject)cell.Objects[i];

                ob.ProcessSpell(this);
                break;
            }

        }
        public void Run(MirDirection dir)
        {
            var steps = 2;

            if (!CanMove || !CanWalk || !CanRun)
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            Point location = Functions.PointMove(CurrentLocation, dir, 1);

            if (!CurrentMap.ValidPoint(location))
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            Cell cell = CurrentMap.GetCell(location);

            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (!ob.Blocking || ob.CellTime >= Envir.Time) continue;

                    Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                    return;
                }
            location = Functions.PointMove(CurrentLocation, dir, steps);

            if (!CurrentMap.ValidPoint(location))
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            cell = CurrentMap.GetCell(location);

            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (!ob.Blocking || ob.CellTime >= Envir.Time) continue;

                    Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                    return;
                }

            if (RidingMount || ActiveSwiftFeet && !Sneaking)
            {
                steps = 3;
                location = Functions.PointMove(CurrentLocation, dir, steps);

                if (!CurrentMap.ValidPoint(location))
                {
                    Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                    return;
                }

                cell = CurrentMap.GetCell(location);

                if (cell.Objects != null)
                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if (!ob.Blocking || ob.CellTime >= Envir.Time) continue;

                        Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                        return;
                    }

                DecreaseMountLoyalty(2);
            }

            if (Concentrating)//ArcherSpells - Elemental system
            {
                if (ConcentrateInterrupted)
                    ConcentrateInterruptTime = SMain.Envir.Time + 3000;// needs adjusting
                else
                {
                    ConcentrateInterruptTime = SMain.Envir.Time + 3000;// needs adjusting
                    ConcentrateInterrupted = true;
                    UpdateConcentration();//Update & send to client
                }
            }

            if (Hidden && !HasClearRing && !Sneaking)
            {
                for (int i = 0; i < Buffs.Count; i++)
                {
                    switch (Buffs[i].Type)
                    {
                        case BuffType.Hiding:
                        case BuffType.MoonLight:
                        case BuffType.DarkBody:
                            Buffs[i].ExpireTime = 0;
                            break;
                    }
                }
            }

            Direction = dir;
            if (CheckMovement(location)) return;

            CurrentMap.GetCell(CurrentLocation).Remove(this);
            RemoveObjects(dir, steps);

            CurrentLocation = location;
            CurrentMap.GetCell(CurrentLocation).Add(this);
            AddObjects(dir, steps);


            SafeZoneInfo szi = CurrentMap.GetSafeZone(CurrentLocation);

            if (szi != null)
            {
                BindLocation = szi.Location;
                BindMapIndex = CurrentMapIndex;
                InSafeZone = true;
            }
            else
                InSafeZone = false;


            CellTime = Envir.Time + 500;
            ActionTime = Envir.Time + MoveDelay;

            if (!RidingMount)
                _runCounter++;

            if (_runCounter > 10)
            {
                _runCounter -= 8;
                ChangeHP(-1);
            }

            if (TradePartner != null) TradeCancel();

            Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
            Broadcast(new S.ObjectRun { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            cell = CurrentMap.GetCell(CurrentLocation);

            for (int i = 0; i < cell.Objects.Count; i++)
            {
                if (cell.Objects[i].Race != ObjectType.Spell) continue;
                SpellObject ob = (SpellObject)cell.Objects[i];

                ob.ProcessSpell(this);
                break;
            }


        }
        public override int Pushed(MapObject pusher, MirDirection dir, int distance)
        {
            int result = 0;
            MirDirection reverse = Functions.ReverseDirection(dir);
            Cell cell;
            for (int i = 0; i < distance; i++)
            {
                Point location = Functions.PointMove(CurrentLocation, dir, 1);

                if (!CurrentMap.ValidPoint(location)) return result;

                cell = CurrentMap.GetCell(location);

                bool stop = false;
                if (cell.Objects != null)
                    for (int c = 0; c < cell.Objects.Count; c++)
                    {
                        MapObject ob = cell.Objects[c];
                        if (!ob.Blocking) continue;
                        stop = true;
                    }
                if (stop) break;

                CurrentMap.GetCell(CurrentLocation).Remove(this);

                Direction = reverse;
                RemoveObjects(dir, 1);
                CurrentLocation = location;
                CurrentMap.GetCell(CurrentLocation).Add(this);
                AddObjects(dir, 1);

                if (TradePartner != null) TradeCancel();

                Enqueue(new S.Pushed { Direction = Direction, Location = CurrentLocation });
                Broadcast(new S.ObjectPushed { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                result++;
            }

            if (result > 0)
            {
                if (Concentrating)//ArcherSpells - Elemental system
                {
                    if (ConcentrateInterrupted)
                        ConcentrateInterruptTime = SMain.Envir.Time + 3000;// needs adjusting
                    else
                    {
                        ConcentrateInterruptTime = SMain.Envir.Time + 3000;// needs adjusting
                        ConcentrateInterrupted = true;
                        UpdateConcentration();//Update & send to client
                    }
                }

                cell = CurrentMap.GetCell(CurrentLocation);

                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    if (cell.Objects[i].Race != ObjectType.Spell) continue;
                    SpellObject ob = (SpellObject)cell.Objects[i];

                    ob.ProcessSpell(this);
                    break;
                }
            }

            ActionTime = Envir.Time + 500;
            return result;
        }

        public void RangeAttack(MirDirection dir, Point location, uint targetID) //ArcherTest
        {
            MapObject target = null;

            if (targetID == ObjectID)
                target = this;
            else if (targetID > 0)
                target = FindObject(targetID, 10);

            if (target != null && target.Dead) return;

            Direction = dir;

            Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });

            UserMagic magic;
            Spell spell = Spell.None;
            bool Focus = false;

            if (target != null && !CanFly(target.CurrentLocation) && (Info.MentalState != 1))
            {
                target = null;
                targetID = 0;
            }

            if (target != null)
            {
                magic = GetMagic(Spell.Focus);

                if (magic != null && Envir.Random.Next(5) <= magic.Level)
                {
                    Focus = true;
                    LevelMagic(magic);
                    spell = Spell.Focus;
                }

                int distance = Functions.MaxDistance(CurrentLocation, target.CurrentLocation);
                int damage = GetAttackPower(MinMC, MaxMC);
                damage = (int)(damage * Math.Max(1,(distance * 0.35)));//range boost
                damage = ApplyArcherState(damage);
                int chanceToHit = 60 + (Focus ? 30 : 0) - (int)(distance * 1.5);
                int hitChance = SMain.Envir.Random.Next(100); // Randomise a number between minimum chance and 100       

                if (hitChance < chanceToHit)
                {
                    if (target.CurrentLocation != location)
                        location = target.CurrentLocation;

                    int delay = Functions.MaxDistance(CurrentLocation, target.CurrentLocation) * 50 + 500 + 50; //50 MS per Step

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, target, damage, DefenceType.ACAgility, true);
                    ActionList.Add(action);
                }
            }
            else
                targetID = 0;

            Enqueue(new S.RangeAttack { TargetID = targetID, Target = location, Spell = spell });
            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = targetID, Target = location, Spell = spell });

            return;
        }
        public void Attack(MirDirection dir, Spell spell)
        {
            LogTime = Envir.Time + Globals.LogDelay;

            bool Mined = false;
            bool MoonLightAttack = false;
            bool DarkBodyAttack = false;

            if (!CanAttack)
            {
                switch (spell)
                {
                    case Spell.Slaying:
                        Slaying = false;
                        break;
                }

                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            if (Hidden)
            {
                for (int i = 0; i < Buffs.Count; i++)
                {
                    switch(Buffs[i].Type)
                    {
                        case BuffType.Hiding:
                        case BuffType.MoonLight: 
                        case BuffType.DarkBody:
                            MoonLightAttack = true;
                            DarkBodyAttack = true;
                            Buffs[i].ExpireTime = 0;
                            break;
                    }
                }
            }

            byte level = 0;
            UserMagic magic;
            switch (spell)
            {
                case Spell.Slaying:
                    if (!Slaying)
                        spell = Spell.None;
                    else
                    {
                        magic = GetMagic(Spell.Slaying);
                        level = magic.Level;
                    }

                    Slaying = false;
                    break;
                case Spell.DoubleSlash:
                    magic = GetMagic(spell);
                    if (magic == null || magic.Info.BaseCost + (magic.Level * magic.Info.LevelCost) > MP)
                    {
                        spell = Spell.None;
                        break;
                    }
                    level = magic.Level;
                    ChangeMP(-(magic.Info.BaseCost + magic.Level * magic.Info.LevelCost));
                    break;
                case Spell.Thrusting:
                case Spell.FlamingSword:
                    magic = GetMagic(spell);
                    if (magic == null)
                    {
                        spell = Spell.None;
                        break;
                    }
                    level = magic.Level;
                    break;
                case Spell.HalfMoon:
                case Spell.CrossHalfMoon:
                    magic = GetMagic(spell);
                    if (magic == null || magic.Info.BaseCost + (magic.Level * magic.Info.LevelCost) > MP)
                    {
                        spell = Spell.None;
                        break;
                    }
                    level = magic.Level;
                    ChangeMP(-(magic.Info.BaseCost + magic.Level * magic.Info.LevelCost));
                    break;
                case Spell.TwinDrakeBlade:
                    magic = GetMagic(spell);
                    if (!TwinDrakeBlade || magic == null || magic.Info.BaseCost + magic.Level * magic.Info.LevelCost > MP)
                    {
                        spell = Spell.None;
                        break;
                    }
                    level = magic.Level;
                    ChangeMP(-(magic.Info.BaseCost + magic.Level * magic.Info.LevelCost));
                    break;
                default:
                    spell = Spell.None;
                    break;
            }


            if (!Slaying)
            {
                magic = GetMagic(Spell.Slaying);

                if (magic != null && Envir.Random.Next(12) <= magic.Level)
                {
                    Slaying = true;
                    Enqueue(new S.SpellToggle { Spell = Spell.Slaying, CanUse = Slaying });
                }
            }

            Direction = dir;

            if (RidingMount) DecreaseMountLoyalty(3);

            Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = spell, Level = level });

            AttackTime = Envir.Time + AttackSpeed;
            ActionTime = Envir.Time + 550;
            RegenTime = Envir.Time + RegenDelay;

            Point target = Functions.PointMove(CurrentLocation, dir, 1);

            int damage = GetAttackPower(MinDC, MaxDC);

            if (MoonLightAttack || DarkBodyAttack)
            {
                magic = MoonLightAttack ? GetMagic(Spell.MoonLight) : GetMagic(Spell.DarkBody);

                if(magic != null)
                {
                    damage += (magic.Level + 1) * 5;
                }
            }

            if (!CurrentMap.ValidPoint(target))
            {
                switch (spell)
                {
                    case Spell.Thrusting:
                        goto Thrusting;
                    case Spell.HalfMoon:
                        goto HalfMoon;
                    case Spell.CrossHalfMoon:
                        goto CrossHalfMoon;
                    case Spell.None:
                        Mined = true;
                        goto Mining;
                }
                return;
            }

            Cell cell = CurrentMap.GetCell(target);

            if (cell.Objects == null)
            {
                switch (spell)
                {
                    case Spell.Thrusting:
                        goto Thrusting;
                    case Spell.HalfMoon:
                        goto HalfMoon;
                    case Spell.CrossHalfMoon:
                        goto CrossHalfMoon;
                }
                return;
            }


            for (int i = 0; i < cell.Objects.Count; i++)
            {
                MapObject ob = cell.Objects[i];
                if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                if (!ob.IsAttackTarget(this)) continue;

                //Only undead targets
                if (ob.Undead)
                    damage = Math.Min(int.MaxValue, damage + Holy);

                #region FatalSword
                magic = GetMagic(Spell.FatalSword);

                DefenceType defence = DefenceType.ACAgility;

                if (magic != null)
                {
                    if (FatalSword)
                    {
                        damage += (magic.Level + 1) * 5;
                        LevelMagic(magic);
                        S.ObjectEffect p = new S.ObjectEffect { ObjectID = ob.ObjectID, Effect = SpellEffect.FatalSword };

                        defence = DefenceType.Agility;
                        CurrentMap.Broadcast(p, ob.CurrentLocation);

                        FatalSword = false;
                    }

                    if (!FatalSword && Envir.Random.Next(10) == 0)
                        FatalSword = true;
                }
                #endregion

                #region MPEater
                magic = GetMagic(Spell.MPEater);

                if (magic != null)
                {
                    int baseCount = 1 + Accuracy / 2;
                    int maxCount = baseCount + magic.Level * 5;
                    MPEaterCount += Envir.Random.Next(baseCount, maxCount);
                    if (MPEater)
                    {
                        LevelMagic(magic);

                        defence = DefenceType.ACAgility;

                        S.ObjectEffect p = new S.ObjectEffect { ObjectID = ob.ObjectID, Effect = SpellEffect.MPEater, EffectType = ObjectID };
                        CurrentMap.Broadcast(p, ob.CurrentLocation);

                        int addMp = 5 * (magic.Level + Accuracy / 4);

                        if (ob.Race == ObjectType.Player)
                        {
                            ((PlayerObject)ob).ChangeMP(-addMp);
                        }

                        ChangeMP(addMp);
                        MPEaterCount = 0;
                        MPEater = false;
                    }
                    else if (!MPEater && 100 <= MPEaterCount) MPEater = true;
                }
                #endregion

                #region Hemorrhage
                magic = GetMagic(Spell.Hemorrhage);

                if (magic != null)
                {
                    HemorrhageAttackCount += Envir.Random.Next(1, 1 + magic.Level * 2);
                    if (Hemorrhage)
                    {
                        damage += damage * 10 / 2; //20%
                        LevelMagic(magic);
                        S.ObjectEffect ef = new S.ObjectEffect { ObjectID = ob.ObjectID, Effect = SpellEffect.Hemorrhage };

                        CurrentMap.Broadcast(ef, ob.CurrentLocation);

                        if (ob == null || ob.Node == null) continue;

                        long calcDuration = magic.Level * 2 + Luck / 6;

                        ob.ApplyPoison(new Poison
                        {
                            Duration = (calcDuration <= 0) ? 1 : calcDuration,
                            Owner = this,
                            PType = PoisonType.Bleeding,
                            TickSpeed = 1000,
                            Value = MaxDC + 1
                        }, this);

                        ob.OperateTime = 0;
                        HemorrhageAttackCount = 0;
                        Hemorrhage = false;
                    }
                    else if (!Hemorrhage && 55 <= HemorrhageAttackCount) Hemorrhage = true;
                }
                #endregion

                DelayedAction action;
                switch (spell)
                {
                    case Spell.Slaying:
                        magic = GetMagic(Spell.Slaying);
                        damage += 5 + magic.Level;
                        LevelMagic(magic);
                        break;
                    case Spell.DoubleSlash:
                        magic = GetMagic(Spell.DoubleSlash);
                        damage = damage * (magic.Level + 8) / 10; // 110% Damage level 3

                        if (defence == DefenceType.ACAgility) defence = DefenceType.MACAgility;

                        action = new DelayedAction(DelayedType.Damage, Envir.Time + 400, ob, damage, DefenceType.Agility, false);
                        ActionList.Add(action);
                        LevelMagic(magic);
                        break;
                    case Spell.Thrusting:
                        magic = GetMagic(Spell.Thrusting);
                        LevelMagic(magic);
                        break;
                    case Spell.HalfMoon:
                        magic = GetMagic(Spell.HalfMoon);
                        LevelMagic(magic);
                        break;
                    case Spell.CrossHalfMoon:
                        magic = GetMagic(Spell.CrossHalfMoon);
                        LevelMagic(magic);
                        break;
                    case Spell.TwinDrakeBlade:
                        magic = GetMagic(Spell.TwinDrakeBlade);
                        damage = damage * (magic.Level + 8) / 10; // 110% Damage level 3
                        TwinDrakeBlade = false;
                        action = new DelayedAction(DelayedType.Damage, Envir.Time + 400, ob, damage, DefenceType.Agility, false);
                        ActionList.Add(action);
                        LevelMagic(magic);

                        if ((((ob.Race != ObjectType.Player) || Settings.PvpCanResistPoison) && (Envir.Random.Next(Settings.PoisonAttackWeight) >= ob.PoisonResist)) && (ob.Level < Level + 10 && Envir.Random.Next(ob.Race == ObjectType.Player ? 40 : 20) <= magic.Level + 1))
                        {
                            ob.ApplyPoison(new Poison { PType = PoisonType.Stun, Duration = ob.Race == ObjectType.Player ? 2 : 2 + magic.Level, TickSpeed = 1000 }, this);
                            ob.Broadcast(new S.ObjectEffect { ObjectID = ob.ObjectID, Effect = SpellEffect.TwinDrakeBlade });
                        }

                        break;
                    case Spell.FlamingSword:
                        magic = GetMagic(Spell.FlamingSword);
                        damage = (damage) + (int)(((double)damage / 100) * ((4 + magic.Level * 4) * 10));
                        FlamingSword = false;
                        defence = DefenceType.AC;
                        //action = new DelayedAction(DelayedType.Damage, Envir.Time + 400, ob, damage, DefenceType.Agility, true);
                        //ActionList.Add(action);
                        LevelMagic(magic);
                        break;
                }

                //if (ob.Attacked(this, damage, defence) <= 0) break;
                action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, ob, damage, defence, true);
                ActionList.Add(action);
                break;
            }

        Thrusting:
            if (spell == Spell.Thrusting)
            {
                target = Functions.PointMove(target, dir, 1);

                if (!CurrentMap.ValidPoint(target)) return;

                cell = CurrentMap.GetCell(target);

                if (cell.Objects == null) return;

                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                    if (!ob.IsAttackTarget(this)) continue;

                    magic = GetMagic(spell);
                    damage = damage * (magic.Level + 1) / 4;

                    ob.Attacked(this, damage, DefenceType.Agility,false);
                    break;
                }


            }
        HalfMoon:
            if (spell == Spell.HalfMoon)
            {
                dir = Functions.PreviousDir(dir);

                magic = GetMagic(spell);
                damage = damage * (magic.Level + 3) / 10;
                for (int i = 0; i < 4; i++)
                {
                    target = Functions.PointMove(CurrentLocation, dir, 1);
                    dir = Functions.NextDir(dir);
                    if (target == Front) continue;

                    if (!CurrentMap.ValidPoint(target)) continue;

                    cell = CurrentMap.GetCell(target);

                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                        if (!ob.IsAttackTarget(this)) continue;

                        ob.Attacked(this, damage, DefenceType.Agility, false);
                        break;
                    }
                }
            }

        CrossHalfMoon:
            if (spell == Spell.CrossHalfMoon)
            {
                magic = GetMagic(spell);
                damage = damage * (magic.Level + 4) / 10;
                for (int i = 0; i < 8; i++)
                {
                    target = Functions.PointMove(CurrentLocation, dir, 1);
                    dir = Functions.NextDir(dir);
                    if (target == Front) continue;

                    if (!CurrentMap.ValidPoint(target)) continue;

                    cell = CurrentMap.GetCell(target);

                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                        if (!ob.IsAttackTarget(this)) continue;

                        ob.Attacked(this, damage, DefenceType.Agility, false);
                        break;
                    }
                }
            }

        Mining:
            if (Mined)
            {
                if (Info.Equipment[(int)EquipmentSlot.Weapon] == null) return;
                if (!Info.Equipment[(int)EquipmentSlot.Weapon].Info.CanMine) return;
                if (CurrentMap.Mine == null) return;
                MineSpot Mine = CurrentMap.Mine[target.X, target.Y];
                if ((Mine == null) || (Mine.Mine == null)) return;
                if (Mine.StonesLeft > 0)
                {
                    Mine.StonesLeft--;
                    if (Envir.Random.Next(100) <= (Mine.Mine.HitRate + (Info.Equipment[(int)EquipmentSlot.Weapon].Info.Accuracy + Info.Equipment[(int)EquipmentSlot.Weapon].Accuracy) * 10))
                    {
                        //create some rubble on the floor (or increase whats there)
                        SpellObject Rubble = null;
                        Cell minecell = CurrentMap.GetCell(CurrentLocation);
                        for (int i = 0; i < minecell.Objects.Count; i++)
                        {
                            if (minecell.Objects[i].Race != ObjectType.Spell) continue;
                            SpellObject ob = (SpellObject)minecell.Objects[i];

                            if (ob.Spell != Spell.Rubble) continue;
                            Rubble = ob;
                            Rubble.ExpireTime = Envir.Time + (5 * 60 * 1000);
                            break;
                        }
                        if (Rubble == null)
                        {
                            Rubble = new SpellObject
                            {
                                Spell = Spell.Rubble,
                                Value = 1,
                                ExpireTime = Envir.Time + (5 * 60 * 1000),
                                TickSpeed = 2000,
                                Caster = null,
                                CurrentLocation = CurrentLocation,
                                CurrentMap = this.CurrentMap,
                                Direction = MirDirection.Up
                            };
                            CurrentMap.AddObject(Rubble);
                            Rubble.Spawned();
                        }
                        if (Rubble != null)
                            ActionList.Add(new DelayedAction(DelayedType.Mine, Envir.Time + 400, Rubble));
                        //check if we get a payout
                        if (Envir.Random.Next(100) <= Mine.Mine.DropRate)
                        {
                            GetMinePayout(Mine.Mine);
                        }
                        DamageItem(Info.Equipment[(int)EquipmentSlot.Weapon], 5 + Envir.Random.Next(15));
                    }
                }
                else
                {
                    if (Envir.Time > Mine.LastRegenTick)
                    {
                        Mine.LastRegenTick = Envir.Time + Mine.Mine.SpotRegenRate * 60 * 1000;
                        Mine.StonesLeft = (byte)Envir.Random.Next(Mine.Mine.MaxStones);
                    }
                }
            }
        }

        public void GetMinePayout(MineSet Mine)
        {
            if ((Mine.Drops == null) || (Mine.Drops.Count == 0)) return;
            if (FreeSpace(Info.Inventory) == 0) return;
            byte Slot = (byte)Envir.Random.Next(Mine.TotalSlots);
            for (int i = 0; i < Mine.Drops.Count; i++)
            {
                MineDrop Drop = Mine.Drops[i];
                if ((Drop.MinSlot <= Slot) && (Drop.MaxSlot >= Slot) && (Drop.Item != null))
                {
                    UserItem item = Envir.CreateDropItem(Drop.Item);
                    if (item.Info.Type == ItemType.Ore)
                    {
                        item.CurrentDura = (ushort)Math.Min(ushort.MaxValue, (Drop.MinDura + Envir.Random.Next(Math.Max(0, Drop.MaxDura - Drop.MinDura))) * 1000);
                        if ((Drop.BonusChance > 0) && (Envir.Random.Next(100) <= Drop.BonusChance))
                            item.CurrentDura = (ushort)Math.Min(ushort.MaxValue, item.CurrentDura + (Envir.Random.Next(Drop.MaxBonusDura) * 1000));
                    }
                    if (CanGainItem(item, false)) GainItem(item);
                    return;
                }
            }

        }

        public void Magic(Spell spell, MirDirection dir, uint targetID, Point location)
        {
            if (!CanCast)
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            AttackTime = Envir.Time + MoveDelay;
            SpellTime = Envir.Time + 1800; //Spell Delay
            ActionTime = Envir.Time + MoveDelay;
            LogTime = Envir.Time + Globals.LogDelay;

            UserMagic magic = GetMagic(spell);
            if (magic == null)
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            int cost = magic.Info.BaseCost + magic.Info.LevelCost * magic.Level;

            if (spell == Spell.Teleport || spell == Spell.Blink)
                for (int i = 0; i < Buffs.Count; i++)
                {
                    if (Buffs[i].Type != BuffType.Teleport) continue;
                    cost += (int)(MaxMP * 0.3F);
                    break;
                }

            if (cost > MP)
            {
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });
                return;
            }

            RegenTime = Envir.Time + RegenDelay;
            ChangeMP(-cost);

            Direction = dir;
            if (spell != Spell.ShoulderDash && spell != Spell.BackStep && spell != Spell.FlashDash)//ArcherSpells - Backstep
                Enqueue(new S.UserLocation { Direction = Direction, Location = CurrentLocation });

            MapObject target = null;

            if (targetID == ObjectID)
                target = this;
            else if (targetID > 0)
                target = FindObject(targetID, 10);

            bool cast = true;
            byte level = magic.Level;
            switch (spell)
            {
                case Spell.FireBall:
                case Spell.GreatFireBall:
                case Spell.FrostCrunch:
                    if (!Fireball(target, magic)) targetID = 0;
                    break;
                case Spell.Healing:
                    if (target == null)
                    {
                        target = this;
                        targetID = ObjectID;
                    }
                    Healing(target, magic);
                    break;
                case Spell.Repulsion:
                case Spell.EnergyRepulsor:
                case Spell.FireBurst:
                    Repulsion(magic);
                    break;
                case Spell.ElectricShock:
                    ActionList.Add(new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, target as MonsterObject));
                    break;
                case Spell.Poisoning:
                    if (!Poisoning(target, magic)) cast = false;
                    break;
                case Spell.HellFire:
                    HellFire(magic);
                    break;
                case Spell.ThunderBolt:
                    ThunderBolt(target, magic);
                    break;
                case Spell.SoulFireBall:
                    if (!SoulFireball(target, magic, out cast)) targetID = 0;
                    break;
                case Spell.SummonSkeleton:
                    SummonSkeleton(magic);
                    break;
                case Spell.Teleport:
                case Spell.Blink:
                    ActionList.Add(new DelayedAction(DelayedType.Magic, Envir.Time + 200, magic, location));
                    break;
                case Spell.Hiding:
                    Hiding(magic);
                    break;
                case Spell.Haste:
                case Spell.LightBody:
                    ActionList.Add(new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic));
                    break;
                case Spell.Fury:
                    FurySpell(magic, out cast);
                    break;
                case Spell.FireBang:
                case Spell.IceStorm:
                    FireBang(magic, target == null ? location : target.CurrentLocation);
                    break;
                case Spell.MassHiding:
                    MassHiding(magic, target == null ? location : target.CurrentLocation, out cast);
                    break;
                case Spell.SoulShield:
                case Spell.BlessedArmour:
                    SoulShield(magic, target == null ? location : target.CurrentLocation, out cast);
                    break;
                case Spell.FireWall:
                    FireWall(magic, target == null ? location : target.CurrentLocation);
                    break;
                case Spell.Lightning:
                    Lightning(magic);
                    break;
                case Spell.HeavenlySword:
                    HeavenlySword(magic);
                    break;
                case Spell.MassHealing:
                    MassHealing(magic, target == null ? location : target.CurrentLocation);
                    break;
                case Spell.ShoulderDash:
                    ShoulderDash(magic);
                    return;
                case Spell.ThunderStorm:
                case Spell.FlameField:
                    ThunderStorm(magic);
                    if (spell == Spell.FlameField)
                        SpellTime = Envir.Time + 2500; //Spell Delay
                    break;
                case Spell.MagicShield:
                    ActionList.Add(new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, magic.GetPower(GetAttackPower(MinMC, MaxMC) + 15)));
                    break;
                case Spell.FlameDisruptor:
                    FlameDisruptor(target, magic);
                    break;
                case Spell.TurnUndead:
                    TurnUndead(target, magic);
                    break;
                case Spell.Vampirism:
                    Vampirism(target, magic);
                    break;
                case Spell.SummonShinsu:
                    SummonShinsu(magic);
                    break;
                case Spell.Purification:
                    if (target == null)
                    {
                        target = this;
                        targetID = ObjectID;
                    }
                    Purification(target, magic);
                    break;
                case Spell.LionRoar:
                    CurrentMap.ActionList.Add(new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, CurrentLocation));
                    break;
                case Spell.Revelation:
                    Revelation(target, magic);
                    break;
                case Spell.PoisonCloud:
                    PoisonCloud(magic, location, out cast);
                    break;
                case Spell.Entrapment:
                    Entrapment(target, magic);
                    break;
                case Spell.BladeAvalanche:
                    BladeAvalanche(magic);
                    break;
                case Spell.SlashingBurst:
                    SlashingBurst(magic, out cast);
                    break;
                case Spell.ProtectionField:
                    ProtectionField(magic);
                    break;
                case Spell.Rage:
                    Rage(magic);
                    break;

                case Spell.Mirroring:
                    Mirroring(magic);
                    break;
                case Spell.Blizzard:
                    Blizzard(magic, target == null ? location : target.CurrentLocation, out cast);
                    break;
                case Spell.MeteorStrike:
                    MeteorStrike(magic, target == null ? location : target.CurrentLocation, out cast);
                    break;
                case Spell.TrapHexagon:
                    TrapHexagon(magic, target, out cast);
                    break;
                case Spell.Reincarnation:
                    Reincarnation(magic, target == null ? null : target as PlayerObject, out cast);
                    break;
                case Spell.Curse:
                    Curse(magic, target == null ? location : target.CurrentLocation, out cast);
                    break;
                case Spell.SummonHolyDeva:
                    SummonHolyDeva(magic);
                    break;
                case Spell.Hallucination:
                    Hallucination(target, magic);
                    break;
                case Spell.UltimateEnhancer:
                    UltimateEnhancer(target, magic, out cast);
                    break;
                case Spell.Plague:
                    Plague(magic, target == null ? location : target.CurrentLocation, out cast);
                    break;
                case Spell.StraightShot:
                    if (!StraightShot(target, magic)) targetID = 0;
                    break;
                case Spell.DoubleShot:
                    if (!DoubleShot(target, magic)) targetID = 0;
                    break;

                case Spell.SwiftFeet:
                    SwiftFeet(magic, out cast);
                    break;
                case Spell.MoonLight:
                    MoonLight(magic);
                    break;
                case Spell.Trap:
                    Trap(magic, target, out cast);
                    break;
                case Spell.PoisonSword:
                    PoisonSword(magic);
                    break;
                case Spell.DarkBody:
                    DarkBody(target, magic);
                    break;
                case Spell.CrescentSlash:
                    CrescentSlash(magic);
                    break;
                case Spell.FlashDash:
                    FlashDash(magic);
                    return;
                case Spell.BackStep://ArcherSpells - Backstep
                    BackStep(magic);
                    return;
                case Spell.ExplosiveTrap://ArcherSpells - Explosive Trap
                    ExplosiveTrap(magic, Front);
                    break;
                case Spell.DelayedExplosion://Archerspells - DelayedExplosion
                    if (!DelayedExplosion(target, magic)) targetID = 0;
                    break;
                case Spell.Concentration://ArcherSpells - Elemental system
                    Concentration(magic);
                    break;
                case Spell.ElementalShot://ArcherSpells - Elemental system
                    if (!ElementalShot(target, magic)) targetID = 0;
                    break;
                case Spell.ElementalBarrier://ArcherSpells - Elemental system
                    ActionList.Add(new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, magic.GetPower(GetAttackPower(MinMC, MaxMC) + magic.GetPower())));
                    break;
                case Spell.BindingShot://ArcherSpells - BindingShot
                    BindingShot(magic, target, out cast);
                    break;
                case Spell.SummonVampire:
                case Spell.SummonToad:
                case Spell.SummonSnakes:
                    ArcherSummon(magic, target, location);
                    break;
                case Spell.VampireShot://ArcherSpells - VampireShot
                case Spell.PoisonShot://ArcherSpells - PoisonShot
                case Spell.CrippleShot://ArcherSpells - CrippleShot
                    SpecialArrowShot(target, magic);
                    break;
                case Spell.NapalmShot://ArcherSpells - NapalmShot
                    NapalmShot(target, magic);
                    break;
                case Spell.OneWithNature://ArcherSpells - OneWithNature
                    OneWithNature(target, magic);
                    break;
                default:
                    cast = false;
                    break;
            }

            Enqueue(new S.Magic { Spell = spell, TargetID = targetID, Target = location, Cast = cast, Level = level });
            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = spell, TargetID = targetID, Target = location, Cast = cast, Level = level });
        }


        #region Elemental System
        private void Concentration(UserMagic magic)
        {
            int duration = 45 + (15 * magic.Level);
            int count = Buffs.Where(x => x.Type == BuffType.Concentration).ToList().Count();
            if (count > 0) return;

            AddBuff(new Buff { Type = BuffType.Concentration, Caster = this, ExpireTime = Envir.Time + duration * 1000, Value = magic.Level });

            LevelMagic(magic);

            ConcentrateInterruptTime = 0;
            ConcentrateInterrupted = false;
            Concentrating = true;
            UpdateConcentration();//Update & send to client

            OperateTime = 0;
        }
        public void UpdateConcentration()
        {
            Enqueue(new S.SetConcentration { ObjectID = ObjectID, Enabled = Concentrating, Interrupted = ConcentrateInterrupted });
            Broadcast(new S.SetObjectConcentration { ObjectID = ObjectID, Enabled = Concentrating, Interrupted = ConcentrateInterrupted });
        }
        private bool ElementalShot(MapObject target, UserMagic magic)
        {
            if (HasElemental)
            {
                if (target == null || !target.IsAttackTarget(this)) return false;
                if ((Info.MentalState != 1) && !CanFly(target.CurrentLocation)) return false;

                int orbPower = magic.GetPower() + GetElementalOrbPower(false);//base power + orbpower

                int damage = GetAttackPower(MinMC, MaxMC) + orbPower;
                int delay = Functions.MaxDistance(CurrentLocation, target.CurrentLocation) * 50 + 500; //50 MS per Step

                DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, magic, damage, target);
                ActionList.Add(action);
            }
            else
            {
                ObtainElement(true);//gather orb through casting
                LevelMagic(magic);
                return false;
            }
            return true;
        }

        public void GatherElement()
        {
            UserMagic magic = GetMagic(Spell.Meditation);

            if (magic == null) return;

            int MeditationLvl = magic.Level;

            magic = GetMagic(Spell.Concentration);

            int ConcentrateLvl = magic != null ? magic.Level : -1;

            int MeditateChance = 0;
            int ConcentrateChance = 0;

            if (Concentrating && !ConcentrateInterrupted && ConcentrateLvl >= 0)
                ConcentrateChance = 1 + ConcentrateLvl;

            if (MeditationLvl >= 0)
            {
                MeditateChance = (8 - MeditationLvl);
                int rnd = Envir.Random.Next(10);
                if (rnd >= (MeditateChance - ConcentrateChance))
                {
                    ObtainElement(false);
                    LevelMagic(GetMagic(Spell.Meditation));
                }
            }
        }
        public void ObtainElement(bool cast)
        {
            int orbType = 0;
            int meditateLevel = 0;

            UserMagic spell = GetMagic(Spell.Meditation);

            if(spell == null)
            {
                ReceiveChat("Skill requires meditation.", ChatType.System);
                return;
            }

            meditateLevel = spell.Level;

            int maxOrbs = (int)Settings.OrbsExpList[Settings.OrbsExpList.Count - 1];

            if (cast)
            {
                ElementsLevel = (int)Settings.OrbsExpList[0];
                orbType = 1;
                if (Settings.GatherOrbsPerLevel)//Meditation Orbs per level
                    if (meditateLevel == 3)
                    {
                        Enqueue(new S.SetElemental { ObjectID = ObjectID, Enabled = true, Value = (uint)Settings.OrbsExpList[0], ElementType = 1, ExpLast = (uint)maxOrbs });
                        Broadcast(new S.SetObjectElemental { ObjectID = ObjectID, Enabled = true, Casted = true, Value = (uint)Settings.OrbsExpList[0], ElementType = 1, ExpLast = (uint)maxOrbs });
                        ElementsLevel = (int)Settings.OrbsExpList[1];
                        orbType = 2;
                    }

                HasElemental = true;
            }
            else
            {
                HasElemental = false;
                ElementsLevel++;

                if (Settings.GatherOrbsPerLevel)//Meditation Orbs per level
                    if (ElementsLevel > Settings.OrbsExpList[GetMagic(Spell.Meditation).Level])
                    {
                        HasElemental = true;
                        ElementsLevel = (int)Settings.OrbsExpList[GetMagic(Spell.Meditation).Level];
                        return;
                    }

                if (ElementsLevel >= Settings.OrbsExpList[0]) HasElemental = true;
                for (int i = 0; i <= Settings.OrbsExpList.Count - 1; i++)
                {
                    if (Settings.OrbsExpList[i] != ElementsLevel) continue;
                    orbType = i + 1;
                    break;
                }
            }

            Enqueue(new S.SetElemental { ObjectID = ObjectID, Enabled = HasElemental, Value = (uint)ElementsLevel, ElementType = (uint)orbType, ExpLast = (uint)maxOrbs });
            Broadcast(new S.SetObjectElemental { ObjectID = ObjectID, Enabled = HasElemental, Casted = cast, Value = (uint)ElementsLevel, ElementType = (uint)orbType, ExpLast = (uint)maxOrbs });
        }

        public int GetElementalOrbCount()
        {
            int OrbCount = 0;
            for (int i = Settings.OrbsExpList.Count - 1; i >= 0; i--)
            {
                if (ElementsLevel >= Settings.OrbsExpList[i])
                {
                    OrbCount = i + 1;
                    break;
                }
            }
            return OrbCount;
        }
        public int GetElementalOrbPower(bool defensive)
        {
            if (!HasElemental) return 0;

            if (defensive)
                return (int)Settings.OrbsDefList[GetElementalOrbCount() - 1];

            if (!defensive)
                return (int)Settings.OrbsDmgList[GetElementalOrbCount() - 1];

            return 0;
        }
        #endregion

        #region Wizard/Archer* Skills
        private bool Fireball(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this) || !CanFly(target.CurrentLocation)) return false;

            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            int delay = Functions.MaxDistance(CurrentLocation, target.CurrentLocation) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, magic, damage, target);

            ActionList.Add(action);

            return true;
        }
        private void Repulsion(UserMagic magic)
        {
            bool result = false;
            for (int d = 0; d <= 1; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (!cell.Valid || cell.Objects == null) continue;

                        for (int i = 0; cell.Objects != null && i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            if (ob.Race != ObjectType.Monster && ob.Race != ObjectType.Player) continue;

                            if (!ob.IsAttackTarget(this) || ob.Level >= Level) continue;

                            if (Envir.Random.Next(20) >= 6 + magic.Level * 3 + Level - ob.Level) continue;

                            int distance = 1 + Math.Max(0, magic.Level - 1) + Envir.Random.Next(2);
                            MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, ob.CurrentLocation);

                            if (ob.Pushed(this, dir, distance) == 0) continue;

                            if (ob.Race == ObjectType.Player)
                                ob.Attacked(this, magic.Level + 1, DefenceType.None, false);
                            result = true;
                        }
                    }
                }
            }

            if (result) LevelMagic(magic);
        }
        private void ElectricShock(MonsterObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this)) return;

            if (Envir.Random.Next(4 - magic.Level) > 0)
            {
                if (Envir.Random.Next(2) == 0) LevelMagic(magic);
                return;
            }

            LevelMagic(magic);

            if (target.Master == this)
            {
                target.ShockTime = Envir.Time + (magic.Level * 5 + 10) * 1000;
                target.Target = null;
                return;
            }

            if (Envir.Random.Next(2) > 0)
            {
                target.ShockTime = Envir.Time + (magic.Level * 5 + 10) * 1000;
                target.Target = null;
                return;
            }

            if (target.Level > Level + 2 || !target.Info.CanTame) return;

            if (Envir.Random.Next(Level + 20 + magic.Level * 5) <= target.Level + 10)
            {
                if (Envir.Random.Next(5) > 0 && target.Master == null)
                {
                    target.RageTime = Envir.Time + (Envir.Random.Next(20) + 10) * 1000;
                    target.Target = null;
                }
                return;
            }

            if (Pets.Count(t => !t.Dead) >= magic.Level + 2) return;
            int rate = (int)(target.MaxHP / 100);
            if (rate <= 2) rate = 2;
            else rate *= 2;

            if (Envir.Random.Next(rate) != 0) return;
            //else if (Envir.Random.Next(20) == 0) target.Die();

            if (target.Master != null)
            {
                target.SetHP(target.MaxHP / 10);
                target.Master.Pets.Remove(target);
            }
            else if (target.Respawn != null)
            {
                target.Respawn.Count--;
                Envir.MonsterCount--;
                CurrentMap.MonsterCount--;
                target.Respawn = null;
            }

            target.Master = this;
            //target.HealthChanged = true;
            target.BroadcastHealthChange();
            Pets.Add(target);
            target.Target = null;
            target.RageTime = 0;
            target.ShockTime = 0;
            target.OperateTime = 0;
            target.MaxPetLevel = (byte)(1 + magic.Level * 2);

            target.Broadcast(new S.ObjectName { ObjectID = target.ObjectID, Name = target.Name });
        }
        private void HellFire(UserMagic magic)
        {
            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, CurrentLocation, Direction, 4);
            CurrentMap.ActionList.Add(action);

            if (magic.Level != 3) return;

            MirDirection dir = (MirDirection)(((int)Direction + 1) % 8);
            action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, CurrentLocation, dir, 4);
            CurrentMap.ActionList.Add(action);

            dir = (MirDirection)(((int)Direction - 1 + 8) % 8);
            action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, CurrentLocation, dir, 4);
            CurrentMap.ActionList.Add(action);
        }
        private void ThunderBolt(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this)) return;

            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            if (target.Undead) damage = (int)(damage * 1.5F);

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, damage, target);

            ActionList.Add(action);
        }
        private void Vampirism(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this)) return;

            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, damage, target);

            ActionList.Add(action);
        }
        private void FireBang(UserMagic magic, Point location)
        {
            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, location);
            CurrentMap.ActionList.Add(action);
        }
        private void FireWall(UserMagic magic, Point location)
        {
            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, location);
            CurrentMap.ActionList.Add(action);
        }
        private void Lightning(UserMagic magic)
        {
            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, CurrentLocation, Direction);
            CurrentMap.ActionList.Add(action);
        }
        private void TurnUndead(MapObject target, UserMagic magic)
        {
            if (target == null || !target.Undead || !target.IsAttackTarget(this)) return;
            if (Envir.Random.Next(2) + Level - 1 <= target.Level)
            {
                if (target.Race == ObjectType.Monster) target.Target = this;
                return;
            }

            int dif = Level - target.Level + 15;

            if (Envir.Random.Next(100) >= (magic.Level + 1 << 3) + dif)
            {
                if (target.Race == ObjectType.Monster) target.Target = this;
                return;
            }

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, target);
            ActionList.Add(action);

        }
        private void FlameDisruptor(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this)) return;

            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            if (!target.Undead) damage = (int)(damage * 1.5F);

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, damage, target);

            ActionList.Add(action);
        }
        private void ThunderStorm(UserMagic magic)
        {
            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, CurrentLocation);
            CurrentMap.ActionList.Add(action);
        }
        private void Mirroring(UserMagic magic)
        {
            MonsterObject monster;
            DelayedAction action;
            for (int i = 0; i < Pets.Count; i++)
            {
                monster = Pets[i];
                if ((monster.Info.Name != Settings.CloneName) || monster.Dead) continue;
                if (monster.Node == null) continue;
                action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, monster, Front, true);
                CurrentMap.ActionList.Add(action);
                return;
            }

            MonsterInfo info = Envir.GetMonsterInfo(Settings.CloneName);
            if (info == null) return;


            LevelMagic(magic);

            monster = MonsterObject.GetMonster(info);
            monster.Master = this;
            monster.ActionTime = Envir.Time + 1000;
            monster.RefreshNameColour(false);

            Pets.Add(monster);

            action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, monster, Front, false);
            CurrentMap.ActionList.Add(action);
        }
        private void Blizzard(UserMagic magic, Point location, out bool cast)
        {
            cast = false;

            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, location);

            ActiveBlizzard = true;
            CurrentMap.ActionList.Add(action);
            cast = true;
        }
        private void MeteorStrike(UserMagic magic, Point location, out bool cast)
        {
            cast = false;

            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, location);

            ActiveBlizzard = true;
            CurrentMap.ActionList.Add(action);
            cast = true;
        }
        #endregion

        #region Taoist Skills
        private void Healing(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsFriendlyTarget(this)) return;

            int health = GetAttackPower(MinSC, MaxSC) * 2 + magic.GetPower() + Level;

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, health, target);

            ActionList.Add(action);
        }
        private bool Poisoning(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this)) return false;

            UserItem item = GetPoison(1);
            if (item == null) return false;

            int power = GetAttackPower(MinSC, MaxSC);

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, power, target, item);
            ActionList.Add(action);
            ConsumeItem(item, 1);
            return true;
        }
        private bool SoulFireball(MapObject target, UserMagic magic, out bool cast)
        {
            cast = false;
            UserItem item = GetAmulet(1);
            if (item == null) return false;
            cast = true;

            if (target == null || !target.IsAttackTarget(this) || !CanFly(target.CurrentLocation)) return false;

            int damage = GetAttackPower(MinSC, MaxSC) + magic.GetPower();

            int delay = Functions.MaxDistance(CurrentLocation, target.CurrentLocation) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, magic, damage, target);

            ActionList.Add(action);
            ConsumeItem(item, 1);

            return true;
        }
        private void SummonSkeleton(UserMagic magic)
        {
            MonsterObject monster;
            for (int i = 0; i < Pets.Count; i++)
            {
                monster = Pets[i];
                if ((monster.Info.Name != Settings.SkeletonName) || monster.Dead) continue;
                if (monster.Node == null) continue;
                monster.ActionList.Add(new DelayedAction(DelayedType.Recall, Envir.Time + 500));
                return;
            }

            if (Pets.Count > 1) return;

            UserItem item = GetAmulet(1);
            if (item == null) return;

            MonsterInfo info = Envir.GetMonsterInfo(Settings.SkeletonName);
            if (info == null) return;


            LevelMagic(magic);
            ConsumeItem(item, 1);

            monster = MonsterObject.GetMonster(info);
            monster.PetLevel = magic.Level;
            monster.Master = this;
            monster.MaxPetLevel = (byte)(4 + magic.Level);
            monster.ActionTime = Envir.Time + 1000;
            monster.RefreshNameColour(false);

            Pets.Add(monster);

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, monster, Front);
            CurrentMap.ActionList.Add(action);
        }
        private void Purification(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsFriendlyTarget(this)) return;

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, target);

            ActionList.Add(action);
        }
        private void SummonShinsu(UserMagic magic)
        {
            MonsterObject monster;
            for (int i = 0; i < Pets.Count; i++)
            {
                monster = Pets[i];
                if ((monster.Info.Name != Settings.ShinsuName) || monster.Dead) continue;
                if (monster.Node == null) continue;
                monster.ActionList.Add(new DelayedAction(DelayedType.Recall, Envir.Time + 500));
                return;
            }

            if (Pets.Count > 1) return;

            UserItem item = GetAmulet(5);
            if (item == null) return;

            MonsterInfo info = Envir.GetMonsterInfo(Settings.ShinsuName);
            if (info == null) return;


            LevelMagic(magic);
            ConsumeItem(item, 5);


            monster = MonsterObject.GetMonster(info);
            monster.PetLevel = magic.Level;
            monster.Master = this;
            monster.MaxPetLevel = (byte)(1 + magic.Level * 2);
            monster.Direction = Direction;
            monster.ActionTime = Envir.Time + 1000;

            Pets.Add(monster);

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, monster, Front);
            CurrentMap.ActionList.Add(action);
        }
        private void Hiding(UserMagic magic)
        {
            UserItem item = GetAmulet(1);
            if (item == null) return;

            ConsumeItem(item, 1);

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, GetAttackPower(MinSC, MaxSC) + (magic.Level + 1) * 5);
            ActionList.Add(action);

        }
        private void MassHiding(UserMagic magic, Point location, out bool cast)
        {
            cast = false;
            UserItem item = GetAmulet(1);
            if (item == null) return;
            cast = true;

            int delay = Functions.MaxDistance(CurrentLocation, location) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, this, magic, GetAttackPower(MinSC, MaxSC) / 2 + (magic.Level + 1) * 2, location);
            CurrentMap.ActionList.Add(action);
        }
        private void SoulShield(UserMagic magic, Point location, out bool cast)
        {
            cast = false;
            UserItem item = GetAmulet(1);
            if (item == null) return;
            cast = true;

            int delay = Functions.MaxDistance(CurrentLocation, location) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, this, magic, GetAttackPower(MinSC, MaxSC) * 2 + (magic.Level + 1) * 10, location);
            CurrentMap.ActionList.Add(action);

            ConsumeItem(item, 1);
        }
        private void MassHealing(UserMagic magic, Point location)
        {
            int value = GetAttackPower(MinSC, MaxSC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, value, location);
            CurrentMap.ActionList.Add(action);
        }
        private void Revelation(MapObject target, UserMagic magic)
        {
            if (target == null) return;

            int value = GetAttackPower(MinSC, MaxSC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, value, target);

            ActionList.Add(action);
        }
        private void PoisonCloud(UserMagic magic, Point location, out bool cast)
        {
            cast = false;

            if (Envir.Time < PoisonCloudTime) return;

            UserItem amulet = GetAmulet(5);
            if (amulet == null) return;

            UserItem poison = GetPoison(5, 1);
            if (poison == null) return;

            int delay = Functions.MaxDistance(CurrentLocation, location) * 50 + 500; //50 MS per Step
            int damage = GetAttackPower(MinSC, MaxSC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, this, magic, damage, location, (byte)Envir.Random.Next(PoisonAttack));

            ConsumeItem(amulet, 5);
            ConsumeItem(poison, 5);

            PoisonCloudTime = Envir.Time + (18 - magic.Level * 2) * 1000;

            CurrentMap.ActionList.Add(action);
            cast = true;
        }
        private void TrapHexagon(UserMagic magic, MapObject target, out bool cast)
        {
            cast = false;

            if (target == null || !target.IsAttackTarget(this) || !(target is MonsterObject)) return;
            if (target.Level > Level + 2) return;

            UserItem item = GetAmulet(1);
            Point location = target.CurrentLocation;

            if (item == null) return;

            LevelMagic(magic);
            uint duration = (uint)((magic.Level * 5 + 10) * 1000);
            int value = (int)duration;

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, value, location);
            CurrentMap.ActionList.Add(action);

            ConsumeItem(item, 1);
            cast = true;
        }
        private void Reincarnation(UserMagic magic, PlayerObject target, out bool cast)
        {
            cast = true;

            if (target == null || !target.Dead) return;

            // checks for amulet of revival
            UserItem item = GetAmulet(1,3);
            if (item == null) return;

            if (!ActiveReincarnation && !ReincarnationReady)
            {
                cast = false;
                int CastTime = Math.Abs(((magic.Level + 1) * 1000) - 9000);
                ExpireTime = Envir.Time + CastTime;
                ReincarnationReady = true;
                ActiveReincarnation = true;
                ReincarnationTarget = target;
                ReincarnationExpireTime = ExpireTime + 5000;

                target.ReincarnationHost = this;

                SpellObject ob = new SpellObject
                {
                    Spell = Spell.Reincarnation,
                    ExpireTime = ExpireTime,
                    TickSpeed = 1000,
                    Caster = this,
                    CurrentLocation = CurrentLocation,
                    CastLocation = CurrentLocation,
                    Show = true,
                    CurrentMap = CurrentMap,
                };
                Packet p = new S.Chat { Message = string.Format("{0} is attempting to revive {1}", Name, target.Name), Type = ChatType.Shout };
                Envir.Broadcast(p);
                CurrentMap.AddObject(ob);
                ob.Spawned();
                ConsumeItem(item, 1);
                // chance of failing Reincarnation when casting
                if (Envir.Random.Next(30) > (1 + magic.Level) * 10)
                {
                    return;
                }

                DelayedAction action = new DelayedAction(DelayedType.Magic, ExpireTime, magic);

                ActionList.Add(action);
                return;
            }
            return;
        }
        private void SummonHolyDeva(UserMagic magic)
        {
            MonsterObject monster;
            for (int i = 0; i < Pets.Count; i++)
            {
                monster = Pets[i];
                if ((monster.Info.Name != Settings.AngelName) || monster.Dead) continue;
                if (monster.Node == null) continue;
                monster.ActionList.Add(new DelayedAction(DelayedType.Recall, Envir.Time + 500));
                return;
            }

            if (Pets.Count > 1) return;

            UserItem item = GetAmulet(2);
            if (item == null) return;


            MonsterInfo info = Envir.GetMonsterInfo(Settings.AngelName);
            if (info == null) return;

            LevelMagic(magic);
            ConsumeItem(item, 2);

            monster = MonsterObject.GetMonster(info);
            monster.PetLevel = magic.Level;
            monster.Master = this;
            monster.MaxPetLevel = (byte)(1 + magic.Level * 2);
            monster.Direction = Direction;
            monster.ActionTime = Envir.Time + 1000;

            Pets.Add(monster);

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 1500, this, magic, monster, Front);
            CurrentMap.ActionList.Add(action);
        }
        private void Hallucination(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this)) return;

            int damage = 0;
            int delay = Functions.MaxDistance(CurrentLocation, target.CurrentLocation) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, delay, magic, damage, target);

            ActionList.Add(action);
        }
        private void UltimateEnhancer(MapObject target, UserMagic magic, out bool cast)
        {
            cast = false;
            //int count = Buffs.Where(x => x.Type == BuffType.UltimateEnhancer).ToList().Count();
            //if (count > 0) return;
            if (target == null || !target.IsFriendlyTarget(this)) return;
            UserItem item = GetAmulet(1);
            if (item == null) return;

            long expiretime = GetAttackPower(MinSC, MaxSC) * 2 + (magic.Level + 1) * 10;
            int value = MaxSC >= 5 ? Math.Min(8, MaxSC / 5) : 1;

            switch (target.Race)
            {
                case ObjectType.Monster:
                case ObjectType.Player:
                    //Only targets
                    if (target.IsFriendlyTarget(this))
                    {
                        target.AddBuff(new Buff { Type = BuffType.UltimateEnhancer, Caster = this, ExpireTime = Envir.Time + expiretime * 1000, Value = value });
                        target.OperateTime = 0;
                        LevelMagic(magic);
                        ConsumeItem(item, 1);
                        if (target != this) cast = true;
                    }
                    break;
            }
        }
        private void Plague(UserMagic magic, Point location, out bool cast)
        {
            cast = false;
            UserItem item = GetAmulet(1);
            if (item == null) return;
            cast = true;

            int delay = Functions.MaxDistance(CurrentLocation, location) * 50 + 500; //50 MS per Step


            PoisonType pType = PoisonType.None;

            UserItem itemp = GetPoison(1, 1);

            if (itemp != null)
                pType = PoisonType.Green;
            else
            {
                itemp = GetPoison(1, 2);

                if (itemp != null)
                    pType = PoisonType.Red;
            }

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, this, magic, GetAttackPower(MinSC, MaxSC), location, pType);
            CurrentMap.ActionList.Add(action);

            ConsumeItem(item, 1);
            if (itemp != null) ConsumeItem(itemp, 1);
        }
        private void Curse(UserMagic magic, Point location, out bool cast)
        {
            cast = false;
            UserItem item = GetAmulet(1);
            if (item == null) return;
            cast = true;

            ConsumeItem(item, 1);

            if (Envir.Random.Next(10 - ((magic.Level + 1) * 2)) > 2) return;

            int delay = Functions.MaxDistance(CurrentLocation, location) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, this, magic, GetAttackPower(MinSC, MaxSC) + (magic.Level + 1) * 5, location, 8 + ((magic.Level + 1) * 2));
            CurrentMap.ActionList.Add(action);

        }
        #endregion

        #region Warrior Skills
        private void Entrapment(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this)) return;

            int damage = 0;

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic, damage, target);

            ActionList.Add(action);
        }
        private void BladeAvalanche(UserMagic magic)
        {
            int criticalDamage = Envir.Random.Next(0, 100) <= (1 + Luck) ? MaxDC * 2 : MinDC * 2;
            int nearDamage = (12 + 3 * (magic.Level + Level / 20)) * criticalDamage / 30 + MinDC;
            int farDamage = (8 + 2 * (magic.Level + Level / 20)) * criticalDamage / 30 + MinDC;

            int col = 3;
            int row = 3;

            Point[] loc = new Point[col]; //0 = left 1 = center 2 = right
            loc[0] = Functions.PointMove(CurrentLocation, Functions.PreviousDir(Direction), 1);
            loc[1] = Functions.PointMove(CurrentLocation, Direction, 1);
            loc[2] = Functions.PointMove(CurrentLocation, Functions.NextDir(Direction), 1);

            for (int i = 0; i < col; i++)
            {
                Point startPoint = loc[i];
                for (int j = 0; j < row; j++)
                {
                    Point hitPoint = Functions.PointMove(startPoint, Direction, j);

                    if (!CurrentMap.ValidPoint(hitPoint)) continue;

                    Cell cell = CurrentMap.GetCell(hitPoint);

                    if (cell.Objects == null) continue;

                    for (int k = 0; k < cell.Objects.Count; k++)
                    {
                        MapObject target = cell.Objects[k];
                        switch (target.Race)
                        {
                            case ObjectType.Monster:
                            case ObjectType.Player:
                                //Only targets
                                if (target.IsAttackTarget(this))
                                {
                                    if (target.Attacked(this, j <= 1 ? nearDamage : farDamage, DefenceType.MAC, false) > 0)
                                        LevelMagic(magic);
                                }
                                break;
                        }
                    }
                }
            }
        }
        private void ProtectionField(UserMagic magic)
        {
            int count = Buffs.Where(x => x.Type == BuffType.ProtectionField).ToList().Count();
            if (count > 0) return;

            int duration = 45 + (15 * magic.Level);
            int value = (int)Math.Round(MaxAC * (0.2 + (0.03 * magic.Level)));

            AddBuff(new Buff { Type = BuffType.ProtectionField, Caster = this, ExpireTime = Envir.Time + duration * 1000, Value = value });
            OperateTime = 0;
            LevelMagic(magic);
        }
        private void Rage(UserMagic magic)
        {
            int count = Buffs.Where(x => x.Type == BuffType.Rage).ToList().Count();
            if (count > 0) return;

            int duration = 48 + (6 * magic.Level);
            int value = (int)Math.Round(MaxDC * (0.12 + (0.03 * magic.Level)));

            AddBuff(new Buff { Type = BuffType.Rage, Caster = this, ExpireTime = Envir.Time + duration * 1000, Value = value });
            OperateTime = 0;
            LevelMagic(magic);
        }
        private void ShoulderDash(UserMagic magic)
        {
            ActionTime = Envir.Time; //allow an immediate next action

            if (InTrapRock) return;
            if (!CanWalk) return;

            int dist = Envir.Random.Next(2) + magic.Level + 2;
            int travel = 0;
            bool wall = true;
            Point location = CurrentLocation;
            MapObject target = null;
            for (int i = 0; i < dist; i++)
            {
                location = Functions.PointMove(location, Direction, 1);

                if (!CurrentMap.ValidPoint(location)) break;


                Cell cell = CurrentMap.GetCell(location);

                bool blocking = false;
                if (cell.Objects != null)
                {
                    for (int c = cell.Objects.Count - 1; c >= 0; c--)
                    {
                        MapObject ob = cell.Objects[c];
                        if (!ob.Blocking) continue;
                        wall = false;
                        if (ob.Race != ObjectType.Monster && ob.Race != ObjectType.Player)
                        {
                            blocking = true;
                            break;
                        }

                        if (target == null && ob.Race == ObjectType.Player)
                            target = ob;

                        if (Envir.Random.Next(20) >= 6 + magic.Level * 3 + Level - ob.Level || !ob.IsAttackTarget(this) || ob.Level >= Level || ob.Pushed(this, Direction, 1) == 0)
                        {
                            if (target == ob)
                                target = null;
                            blocking = true;
                            break;
                        }

                        if (cell.Objects == null) break;

                    }
                }

                if (blocking)
                {
                    if (magic.Level != 3) break;

                    Point location2 = Functions.PointMove(location, Direction, 1);

                    if (!CurrentMap.ValidPoint(location2)) break;

                    cell = CurrentMap.GetCell(location2);

                    blocking = false;
                    if (cell.Objects != null)
                    {
                        for (int c = cell.Objects.Count - 1; c >= 0; c--)
                        {
                            MapObject ob = cell.Objects[c];
                            if (!ob.Blocking) continue;
                            if (ob.Race != ObjectType.Monster && ob.Race != ObjectType.Player)
                            {
                                blocking = true;
                                break;
                            }

                            if (!ob.IsAttackTarget(this) || ob.Level >= Level || ob.Pushed(this, Direction, 1) == 0)
                            {
                                blocking = true;
                                break;
                            }

                            if (cell.Objects == null) break;
                        }
                    }

                    if (blocking) break;

                    cell = CurrentMap.GetCell(location);

                    if (cell.Objects != null)
                    {
                        for (int c = cell.Objects.Count - 1; c >= 0; c--)
                        {
                            MapObject ob = cell.Objects[c];
                            if (!ob.Blocking) continue;
                            if (ob.Race != ObjectType.Monster && ob.Race != ObjectType.Player)
                            {
                                blocking = true;
                                break;
                            }

                            if (Envir.Random.Next(20) >= 6 + magic.Level * 3 + Level - ob.Level || !ob.IsAttackTarget(this) || ob.Level >= Level || ob.Pushed(this, Direction, 1) == 0)
                            {
                                blocking = true;
                                break;
                            }

                            if (cell.Objects == null) break;
                        }
                    }

                    if (blocking) break;
                }

                travel++;
                CurrentMap.GetCell(CurrentLocation).Remove(this);
                RemoveObjects(Direction, 1);

                CurrentLocation = location;

                Enqueue(new S.UserDash { Direction = Direction, Location = location });
                Broadcast(new S.ObjectDash { ObjectID = ObjectID, Direction = Direction, Location = location });

                CurrentMap.GetCell(CurrentLocation).Add(this);
                AddObjects(Direction, 1);
            }

            if (travel > 0 && !wall)
            {
                if (target != null) target.Attacked(this, magic.Level + 1, DefenceType.None, false);
                LevelMagic(magic);
            }

            if (travel > 0)
            {

                Cell cell = CurrentMap.GetCell(CurrentLocation);
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    if (cell.Objects[i].Race != ObjectType.Spell) continue;
                    SpellObject ob = (SpellObject)cell.Objects[i];

                    if (ob.Spell != Spell.FireWall || !IsAttackTarget(ob.Caster)) continue;
                    Attacked(ob.Caster, ob.Value, DefenceType.MAC, false);
                    break;
                }
            }

            if (travel == 0 || wall && dist != travel)
            {
                if (travel > 0)
                {
                    Enqueue(new S.UserDash { Direction = Direction, Location = Front });
                    Broadcast(new S.ObjectDash { ObjectID = ObjectID, Direction = Direction, Location = Front });
                }
                else
                    Broadcast(new S.ObjectDash { ObjectID = ObjectID, Direction = Direction, Location = Front });

                Enqueue(new S.UserDashFail { Direction = Direction, Location = CurrentLocation });
                Broadcast(new S.ObjectDashFail { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                ReceiveChat("Not enough pushing Power.", ChatType.System);
            }

            CellTime = Envir.Time + 500;
        }
        private void SlashingBurst(UserMagic magic, out bool cast)
        {
            cast = false;

            // delayTime
            if (Envir.Time < SlashingBurstTime) return;

            SlashingBurstTime = Envir.Time + (14 - magic.Level * 4) * 1000;
            cast = true;

            // damage
            int damage = GetAttackPower(MaxDC, MaxDC) * magic.GetPower();

            // objects = this, magic, damage, currentlocation, direction, attackRange
            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, CurrentLocation, Direction, 1);
            CurrentMap.ActionList.Add(action);

            // telpo location
            Point location = Functions.PointMove(CurrentLocation, Direction, 2);

            if (!CurrentMap.ValidPoint(location)) return;

            Cell cInfo = CurrentMap.GetCell(location);

            bool blocked = false;
            if (cInfo.Objects != null)
            {
                for (int c = 0; c < cInfo.Objects.Count; c++)
                {
                    MapObject ob = cInfo.Objects[c];
                    if (!ob.Blocking) continue;
                    blocked = true;
                    if ((cInfo.Objects == null) || blocked) break;
                }
            }

            // blocked telpo cancel
            if (blocked) return;

            // move character
            CurrentMap.GetCell(CurrentLocation).Remove(this);
            RemoveObjects(Direction, 1);

            CurrentLocation = location;

            CurrentMap.GetCell(CurrentLocation).Add(this);
            AddObjects(Direction, 1);

            Enqueue(new S.UserAttackMove { Direction = Direction, Location = location });
        }
        private void FurySpell(UserMagic magic, out bool cast)
        {
            cast = false;

            // delayTime
            if (Envir.Time < FuryTime) return;
            cast = true;
            FuryTime = 600000 - magic.Level * 120000;
            ActionList.Add(new DelayedAction(DelayedType.Magic, Envir.Time + 500, magic));
        }
        private void CounterAttack(UserMagic magic, MapObject target)
        {
            if (target == null || magic == null) return;

            if (bCounterAttack == false) return;

            int criticalDamage = Envir.Random.Next(0, 100) <= Accuracy ? MaxDC * 2 : MinDC * 2;
            int damage = (MinDC / 5 + 4 * (magic.Level + Level / 20)) * criticalDamage / 20 + MaxDC;

            MirDirection dir = Functions.ReverseDirection(target.Direction);
            Direction = dir;

            if (Functions.InRange(CurrentLocation, target.CurrentLocation, 1) == false) return;
            if (Envir.Random.Next(10) > magic.Level + 6) return;
            Enqueue(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.CounterAttack, TargetID = target.ObjectID, Target = target.CurrentLocation, Cast = true, Level = GetMagic(Spell.CounterAttack).Level, SelfBroadcast = true });
            DelayedAction action = new DelayedAction(DelayedType.Damage, AttackTime, target, damage, DefenceType.AC, true);
            ActionList.Add(action);
            LevelMagic(magic);
            bCounterAttack = false;
            CounterAttackTime = 0;
        }
        #endregion

        #region Assassin Skills
        
        private void HeavenlySword(UserMagic magic)
        {
            int damage = GetAttackPower(MinDC, MaxDC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, CurrentLocation, Direction);
            CurrentMap.ActionList.Add(action);
        }
        private void SwiftFeet(UserMagic magic, out bool cast)
        {
            cast = false;

            // delayTime
            if (Envir.Time < SwiftFeetTime) return;
            cast = true;
            SwiftFeetTime = 210000 - magic.Level * 40000;

            AddBuff(new Buff { Type = BuffType.SwiftFeet, Caster = this, ExpireTime = Envir.Time + 25000 + magic.Level * 5000, Value = 1, Visible = true });
            LevelMagic(magic);
        }
        private void MoonLight(UserMagic magic)
        {
            for (int i = 0; i < Buffs.Count; i++)
                if (Buffs[i].Type == BuffType.MoonLight) return;

            AddBuff(new Buff { Type = BuffType.MoonLight, Caster = this, ExpireTime = Envir.Time + (GetAttackPower(MinAC, MaxAC) + (magic.Level + 1) * 5) * 500, Visible = true });
            LevelMagic(magic);
        }
        private void Trap(UserMagic magic, MapObject target, out bool cast)
        {
            cast = false;
            // delayTime
            if (Envir.Time < TrapTime) return;
            if (target == null || !target.IsAttackTarget(this) || !(target is MonsterObject)) return;
            if (target.Level >= Level + 2) return;

            TrapTime = 60000 - magic.Level * 15000;

            Point location = target.CurrentLocation;

            LevelMagic(magic);
            uint duration = 60000;
            int value = (int)duration;

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, value, location);
            CurrentMap.ActionList.Add(action);
            cast = true;
        }
        private bool PoisonSword(UserMagic magic)
        {
            UserItem item = GetPoison(1);
            if (item == null) return false;

            int power = GetAttackPower(MinDC, MaxDC) + magic.GetPower();

            Point hitPoint = Functions.PointMove(CurrentLocation, Direction, 1);

            if (!CurrentMap.ValidPoint(hitPoint)) return false;

            Cell cell = CurrentMap.GetCell(hitPoint);

            if (cell.Objects == null) return false;

            for (int o = 0; o < cell.Objects.Count; o++)
            {
                MapObject target = cell.Objects[o];
                if (target.Race != ObjectType.Player && target.Race != ObjectType.Monster) continue;

                if (target == null || !target.IsAttackTarget(this) || target.Node == null) continue;

                target.ApplyPoison(new Poison
                {
                    Duration = 3 + power / 10 + magic.Level * 3,
                    Owner = this,
                    PType = PoisonType.Green,
                    TickSpeed = 1000,
                    Value = power / 10 + magic.Level + 1 + Envir.Random.Next(PoisonAttack)
                }, this);

                target.OperateTime = 0;
                break;
            }

            LevelMagic(magic);
            ConsumeItem(item, 1);
            return true;
        }
        private void DarkBody(MapObject target, UserMagic magic)
        {
            MonsterObject monster;
            for (int i = 0; i < Pets.Count; i++)
            {
                monster = Pets[i];
                if ((monster.Info.Name != Settings.AssassinCloneName) || monster.Dead) continue;
                if (monster.Node == null) continue;
                monster.Die();
                return;
            }

            MonsterInfo info = Envir.GetMonsterInfo(Settings.AssassinCloneName);
            if (info == null) return;

            if (target == null) return;

            LevelMagic(magic);

            monster = MonsterObject.GetMonster(info);
            monster.Master = this;
            monster.Direction = Direction;
            monster.ActionTime = Envir.Time + 500;
            monster.RefreshNameColour(false);
            monster.Target = target;
            Pets.Add(monster);

            monster.Spawn(CurrentMap, CurrentLocation);

            for (int i = 0; i < Buffs.Count; i++)
                if (Buffs[i].Type == BuffType.DarkBody) return;

            AddBuff(new Buff { Type = BuffType.DarkBody, Caster = this, ExpireTime = Envir.Time + (GetAttackPower(MinAC, MaxAC) + (magic.Level + 1) * 5) * 500, Visible = true });
            LevelMagic(magic);
        }
        private void CrescentSlash(UserMagic magic)
        {
            int criticalDamage = Envir.Random.Next(0, 100) <= Accuracy ? MaxDC * 2 : MinDC * 2;
            int damage = (MinDC / 5 + 4 * (magic.Level + Level / 20)) * criticalDamage / 20 + MaxDC;

            MirDirection backDir = Functions.ReverseDirection(Direction);
            MirDirection preBackDir = Functions.PreviousDir(backDir);
            MirDirection nextBackDir = Functions.NextDir(backDir);

            for (int i = 0; i < 8; i++)
            {
                MirDirection dir = (MirDirection)i;
                Point hitPoint = Functions.PointMove(CurrentLocation, dir, 1);

                if (dir != backDir && dir != preBackDir && dir != nextBackDir)
                {

                    if (!CurrentMap.ValidPoint(hitPoint)) continue;

                    Cell cell = CurrentMap.GetCell(hitPoint);

                    if (cell.Objects == null) continue;


                    for (int j = 0; j < cell.Objects.Count; j++)
                    {
                        MapObject target = cell.Objects[j];
                        switch (target.Race)
                        {
                            case ObjectType.Monster:
                            case ObjectType.Player:
                                //Only targets
                                if (target.IsAttackTarget(this))
                                {
                                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + AttackSpeed, target, damage, DefenceType.AC, true);
                                    ActionList.Add(action);
                                }
                                break;
                        }
                    }
                    LevelMagic(magic);
                }
            }
        }

        private void FlashDash(UserMagic magic)
        {
            bool success = false;
            ActionTime = Envir.Time;

            int travel = 0;
            bool blocked = false;
            int jumpDistance = (magic.Level <= 1) ? 0 : 1;//3 max
            Point location = CurrentLocation;
            for (int i = 0; i < jumpDistance; i++)
            {
                location = Functions.PointMove(location, Direction, 1);
                if (!CurrentMap.ValidPoint(location)) break;

                Cell cInfo = CurrentMap.GetCell(location);
                if (cInfo.Objects != null)
                {
                    for (int c = 0; c < cInfo.Objects.Count; c++)
                    {
                        MapObject ob = cInfo.Objects[c];
                        if (!ob.Blocking) continue;
                        blocked = true;
                        if ((cInfo.Objects == null) || blocked) break;
                    }
                }
                if (blocked) break;
                travel++;
            }

            jumpDistance = travel;

            if (jumpDistance > 0)
            {
                location = Functions.PointMove(CurrentLocation, Direction, jumpDistance);
                CurrentMap.GetCell(CurrentLocation).Remove(this);
                RemoveObjects(Direction, 1);
                CurrentLocation = location;
                CurrentMap.GetCell(CurrentLocation).Add(this);
                AddObjects(Direction, 1);
                Enqueue(new S.UserDashAttack { Direction = Direction, Location = location });
                Broadcast(new S.ObjectDashAttack { ObjectID = ObjectID, Direction = Direction, Location = location, Distance = jumpDistance });
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            }

            if (travel == 0) location = CurrentLocation;

            int attackDelay = (AttackSpeed - 120) <= 300 ? 300 : (AttackSpeed - 120);
            AttackTime = Envir.Time + attackDelay;
            SpellTime = Envir.Time + 300;

            location = Functions.PointMove(location, Direction, 1);
            if (CurrentMap.ValidPoint(location))
            {
                Cell cInfo = CurrentMap.GetCell(location);
                if (cInfo.Objects != null)
                {
                    for (int c = 0; c < cInfo.Objects.Count; c++)
                    {
                        MapObject ob = cInfo.Objects[c];
                        switch (ob.Race)
                        {
                            case ObjectType.Monster:
                            case ObjectType.Player:
                                //Only targets
                                if (ob.IsAttackTarget(this))
                                {
                                    DelayedAction action = new DelayedAction(DelayedType.Damage, AttackTime, ob, GetAttackPower(MinDC, MaxDC), DefenceType.AC, true);
                                    ActionList.Add(action);
                                    success = true;
                                    if ((((ob.Race != ObjectType.Player) || Settings.PvpCanResistPoison) && (Envir.Random.Next(Settings.PoisonAttackWeight) >= ob.PoisonResist)) && (Envir.Random.Next(15) <= magic.Level + 1))
                                    {
                                        DelayedAction pa = new DelayedAction(DelayedType.Poison, AttackTime, ob, PoisonType.Stun, SpellEffect.TwinDrakeBlade, magic.Level + 1, 1000);
                                        ActionList.Add(pa);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            if (success) //technicaly this makes flashdash lvl when it casts rather then when it hits (it wont lvl if it's not hitting!)
                LevelMagic(magic);
        }
        #endregion

        #region Archer Skills

        private int ApplyArcherState(int damage)
        {
            UserMagic magic = GetMagic(Spell.MentalState);
            if (magic != null)
                LevelMagic(magic);
            int dmgpenalty = 100;
            switch(Info.MentalState)
            {
                case 1: //trickshot
                    dmgpenalty = 55 + (Info.MentalStateLvl * 5);
                    break;
                case 2: //group attack
                    dmgpenalty = 80;
                    break;
            }
            return (damage * dmgpenalty) / 100;
        }

        private bool StraightShot(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this)) return false;
            if ((Info.MentalState != 1) && !CanFly(target.CurrentLocation)) return false;
            int distance = Functions.MaxDistance(CurrentLocation, target.CurrentLocation);
            int damage = (GetAttackPower(MinMC, MaxMC) + magic.GetPower());
            damage = (int)(damage * Math.Max(1, (distance * 0.45)));//range boost
            damage = ApplyArcherState(damage);
            int delay = distance * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, magic, damage, target);

            ActionList.Add(action);

            return true;
        }

        private bool DoubleShot(MapObject target, UserMagic magic)
        {
            if (target == null || !target.IsAttackTarget(this)) return false;
            if ((Info.MentalState != 1) && !CanFly(target.CurrentLocation)) return false;
            int distance = Functions.MaxDistance(CurrentLocation, target.CurrentLocation);
            int damage = (GetAttackPower(MinMC, MaxMC) + magic.GetPower());
            damage = (int)(damage * Math.Max(1, (distance * 0.25)));//range boost
            damage = ApplyArcherState(damage);
            int delay = distance * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, magic, damage, target);

            ActionList.Add(action);

            action = new DelayedAction(DelayedType.Magic, Envir.Time + delay + 50, magic, damage, target);

            ActionList.Add(action);

            return true;
        }
        private void BackStep(UserMagic magic)//ArcherSpells - Backstep
        {
            ActionTime = Envir.Time;
            if (!CanWalk) return;

            int travel = 0;
            bool blocked = false;
            int jumpDistance = (magic.Level == 0) ? 1 : magic.Level;//3 max
            MirDirection jumpDir = Functions.ReverseDirection(Direction);
            Point location = CurrentLocation;
            for (int i = 0; i < jumpDistance; i++)
            {
                location = Functions.PointMove(location, jumpDir, 1);
                if (!CurrentMap.ValidPoint(location)) break;

                Cell cInfo = CurrentMap.GetCell(location);
                if (cInfo.Objects != null)
                    for (int c = 0; c < cInfo.Objects.Count; c++)
                    {
                        MapObject ob = cInfo.Objects[c];
                        if (!ob.Blocking) continue;
                        blocked = true;
                        if ((cInfo.Objects == null) || blocked) break;
                    }
                if (blocked) break;
                travel++;
            }

            jumpDistance = travel;
            if (jumpDistance > 0)
            {
                for (int i = 0; i < jumpDistance; i++)
                {
                    location = Functions.PointMove(CurrentLocation, jumpDir, 1);
                    CurrentMap.GetCell(CurrentLocation).Remove(this);
                    RemoveObjects(jumpDir, 1);
                    CurrentLocation = location;
                    CurrentMap.GetCell(CurrentLocation).Add(this);
                    AddObjects(jumpDir, 1);
                }
                Enqueue(new S.UserBackStep { Direction = Direction, Location = location });
                Broadcast(new S.ObjectBackStep { ObjectID = ObjectID, Direction = Direction, Location = location, Distance = jumpDistance });
                LevelMagic(magic);
            }
            else
            {
                Broadcast(new S.ObjectBackStep { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Distance = jumpDistance });
                ReceiveChat("Not enough jumping power.", ChatType.System);
            }

            CellTime = Envir.Time + 500;
        }
        private bool DelayedExplosion(MapObject target, UserMagic magic)//ArcherSpells - DelayedExplosion
        {
            if (target == null || !target.IsAttackTarget(this) || !CanFly(target.CurrentLocation)) return false;

            int power = GetAttackPower(MinMC, MaxMC) + magic.GetPower();
            int delay = Functions.MaxDistance(CurrentLocation, target.CurrentLocation) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, magic, power, target);
            ActionList.Add(action);
            return true;
        }
        private void ExplosiveTrap(UserMagic magic, Point location)//ArcherSpells - Explosive Trap
        {
            int trapCount = 0;
            for (int i = 0; i <= 3; i++)
                if (ArcherTrapObjectsArray[i,0] != null) trapCount++;
            if (trapCount >= magic.Level + 1) return;//max 4 traps

            int freeTrapSpot = -1;
            for (int i = 0; i <= 3; i++)
                if (ArcherTrapObjectsArray[i,0] == null)
                {
                    freeTrapSpot = i;
                    break;
                }
            if (freeTrapSpot == -1) return;

            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();
            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, location, freeTrapSpot);
            CurrentMap.ActionList.Add(action);
        }
        public void ExplosiveTrapDetonated(int obIDX, int Trapnr)//ArcherSpells - Explosive Trap
        {
            SpellObject ArcherTrap;
            if (ArcherTrapObjectsArray[obIDX,Trapnr] == null) return;
            for (int j = 0; j <= 2; j++)
            {
                if (j != Trapnr)
                {
                    ArcherTrap = (SpellObject)ArcherTrapObjectsArray[obIDX, j];
                    //this should technicaly remove them without explosion but it crashes server so leaving it for now
                    //ArcherTrap.CurrentMap.RemoveObject(ArcherTrap);
                    //ArcherTrap.Despawn();
                    ArcherTrap.DetonateTrapNow();
                }
                ArcherTrapObjectsArray[obIDX, j] = null;
            }
        }
        public void DoKnockback(MapObject target, UserMagic magic)//ElementalShot - knockback
        {
            Cell cell = CurrentMap.GetCell(target.CurrentLocation);
            if (!cell.Valid || cell.Objects == null) return;

            if (target.CurrentLocation.Y < 0 || target.CurrentLocation.Y >= CurrentMap.Height || target.CurrentLocation.X < 0 || target.CurrentLocation.X >= CurrentMap.Height) return;

            if (target.Race != ObjectType.Monster && target.Race != ObjectType.Player) return;
            if (!target.IsAttackTarget(this) || target.Level >= Level) return;

            if (Envir.Random.Next(20) >= 6 + magic.Level * 3 + ElementsLevel + Level - target.Level) return;
            int distance = 1 + Math.Max(0, magic.Level - 1) + Envir.Random.Next(2);
            MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, target.CurrentLocation);

            target.Pushed(this, dir, distance);
        }
        public void BindingShot(UserMagic magic, MapObject target, out bool cast)//ArcherSpells - BindingShot
        {
            cast = false;

            if (target == null || !target.IsAttackTarget(this) || !(target is MonsterObject)) return;
            if ((Info.MentalState != 1) && !CanFly(target.CurrentLocation)) return;
            if (target.Level > Level + 2) return;
            if (((MonsterObject)target).ShockTime >= Envir.Time) return;//Already shocked


            uint duration = (uint)((magic.Level * 5 + 10) * 1000);
            int value = (int)duration;
            int delay = Functions.MaxDistance(CurrentLocation, target.CurrentLocation) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, magic, value, target);
            ActionList.Add(action);

            cast = true;
        }
        public void SpecialArrowShot(MapObject target, UserMagic magic)//ArcherSpells - VampireShot,PoisonShot,CrippleShot
        {
            if (target == null || !target.IsAttackTarget(this)) return;
            if ((Info.MentalState != 1) && !CanFly(target.CurrentLocation)) return;
            int distance = Functions.MaxDistance(CurrentLocation, target.CurrentLocation);
            int damage = (GetAttackPower(MinMC, MaxMC) + magic.GetPower());
            if (magic.Spell != Spell.CrippleShot)
                damage = (int)(damage * Math.Max(1, (distance * 0.4)));//range boost
            damage = ApplyArcherState(damage);

            int delay = distance * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, magic, damage, target);
            ActionList.Add(action);
        }
        public void NapalmShot(MapObject target, UserMagic magic)//ArcherSpells - NapalmShot
        {
            if (target == null || !target.IsAttackTarget(this)) return;
            if ((Info.MentalState != 1) && !CanFly(target.CurrentLocation)) return;

            int distance = Functions.MaxDistance(CurrentLocation, target.CurrentLocation);
            int damage = (GetAttackPower(MinMC, MaxMC) + magic.GetPower());
            damage = ApplyArcherState(damage);

            int delay = distance * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, this, magic, damage, target.CurrentLocation);
            CurrentMap.ActionList.Add(action);
        }
        public void ArcherSummon(UserMagic magic, MapObject target, Point location)//ArcherSpells - Summons
        {
            if (target != null && target.IsAttackTarget(this))
                location = target.CurrentLocation;
            if (!CanFly(location)) return;

            uint duration = (uint)((magic.Level * 5 + 10) * 1000);
            int value = (int)duration;
            int delay = Functions.MaxDistance(CurrentLocation, location) * 50 + 500; //50 MS per Step

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + delay, magic, value, location, target);
            ActionList.Add(action);
        }

        public void OneWithNature(MapObject target, UserMagic magic)//ArcherSpells - OneWithNature
        {
            int damage = GetAttackPower(MinMC, MaxMC) + magic.GetPower();

            DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, damage, CurrentLocation);
            CurrentMap.ActionList.Add(action);
        }
        #endregion

        private void CheckSneakRadius()
        {
            if (!Sneaking) return;

            for (int y = CurrentLocation.Y - 3; y <= CurrentLocation.Y + 3; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = CurrentLocation.X - 3; x <= CurrentLocation.X + 3; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;

                    Cell cell = CurrentMap.GetCell(x, y);
                    if (!cell.Valid || cell.Objects == null) continue;

                    for (int i = 0; cell.Objects != null && i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if ((ob.Race != ObjectType.Player) || ob == this) continue;

                        SneakingActive = false;
                        return;
                    }
                }
            }

            SneakingActive = true;
        }

        private void CompleteMagic(IList<object> data)
        {
            UserMagic magic = (UserMagic)data[0];
            int value;
            MapObject target;
            Point location;
            MonsterObject monster;
            switch (magic.Spell)
            {
                #region FireBall, GreatFireBall, ThunderBolt, SoulFireBall, FlameDisruptor

                case Spell.FireBall:
                case Spell.GreatFireBall:
                case Spell.ThunderBolt:
                case Spell.SoulFireBall:
                case Spell.FlameDisruptor:
                case Spell.StraightShot:
                case Spell.DoubleShot:
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;
                    if (target.Attacked(this, value, DefenceType.MAC, false) > 0) LevelMagic(magic);
                    break;

                #endregion

                #region FrostCrunch
                case Spell.FrostCrunch:
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;
                    if (target.Attacked(this, value, DefenceType.MAC, false) > 0)
                    {
                        if (Level + (target.Race == ObjectType.Player ? 2 : 10) >= target.Level && Envir.Random.Next(target.Race == ObjectType.Player ? 100 : 20) <= magic.Level)
                        {
                            target.ApplyPoison(new Poison
                            {
                                Owner = this,
                                Duration = target.Race == ObjectType.Player ? 4 : 5 + Envir.Random.Next(5),
                                PType = PoisonType.Slow,
                                TickSpeed = 1000,
                            }, this);
                            target.OperateTime = 0;
                        }

                        if (Level + (target.Race == ObjectType.Player ? 2 : 10) >= target.Level && Envir.Random.Next(target.Race == ObjectType.Player ? 100 : 40) <= magic.Level)
                        {
                            target.ApplyPoison(new Poison
                            {
                                Owner = this,
                                Duration = target.Race == ObjectType.Player ? 2 : 5 + Envir.Random.Next(Freezing),
                                PType = PoisonType.Frozen,
                                TickSpeed = 1000,
                            }, this);
                            target.OperateTime = 0;
                        }

                        LevelMagic(magic);
                    }
                    break;

                #endregion

                #region Vampirism

                case Spell.Vampirism:
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;
                    value = target.Attacked(this, value, DefenceType.MAC, false);
                    if (value == 0) return;
                    LevelMagic(magic);
                    if (VampAmount == 0) VampTime = Envir.Time + 1000;
                    VampAmount += (ushort)(value * (magic.Level + 1) * 0.25F);
                    break;

                #endregion

                #region Healing

                case Spell.Healing:
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsFriendlyTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;
                    if (target.Health >= target.MaxHealth) return;
                    target.HealAmount = (ushort)Math.Min(ushort.MaxValue, target.HealAmount + value);
                    target.OperateTime = 0;
                    LevelMagic(magic);
                    break;

                #endregion

                #region ElectricShock

                case Spell.ElectricShock:
                    monster = (MonsterObject)data[1];
                    if (monster == null || !monster.IsAttackTarget(this) || monster.CurrentMap != CurrentMap || monster.Node == null) return;
                    ElectricShock(monster, magic);
                    break;

                #endregion

                #region Poisoning

                case Spell.Poisoning:
                    value = (int)data[1];
                    target = (MapObject)data[2];
                    UserItem item = (UserItem)data[3];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

                    switch (item.Info.Shape)
                    {
                        case 1:
                            target.ApplyPoison(new Poison
                            {
                                Duration = (value * 2) + (magic.Level + 1) * 7,
                                Owner = this,
                                PType = PoisonType.Green,
                                TickSpeed = 2000,
                                Value = value / 15 + magic.Level + 1 + Envir.Random.Next(PoisonAttack)
                            }, this);
                            break;
                        case 2:
                            target.ApplyPoison(new Poison
                            {
                                Duration = (value * 2) + (magic.Level + 1) * 7,
                                Owner = this,
                                PType = PoisonType.Red,
                                TickSpeed = 2000,
                            }, this);
                            break;
                    }
                    target.OperateTime = 0;

                    LevelMagic(magic);
                    break;

                #endregion

                #region Teleport
                case Spell.Teleport:
                    Map temp = Envir.GetMap(BindMapIndex);
                    int mapSizeX = temp.Width / (magic.Level + 1);
                    int mapSizeY = temp.Height / (magic.Level + 1);

                    if (CurrentMap.Info.NoTeleport)
                    {
                        ReceiveChat(("You cannot teleport on this map"), ChatType.System);
                        return;
                    }

                    for (int i = 0; i < 200; i++)
                    {
                        location = new Point(BindLocation.X + Envir.Random.Next(-mapSizeX, mapSizeX),
                                             BindLocation.Y + Envir.Random.Next(-mapSizeY, mapSizeY));

                        if (Teleport(temp, location)) break;
                    }

                    AddBuff(new Buff { Type = BuffType.Teleport, Caster = this, ExpireTime = Envir.Time + 30000 });
                    LevelMagic(magic);

                    break;
                #endregion

                #region Blink

                case Spell.Blink:
                    {
                        location = (Point)data[1];
                        if (CurrentMap.Info.NoTeleport)
                        {
                            ReceiveChat(("You cannot teleport on this map"), ChatType.System);
                            return;
                        }
                        if (!CurrentMap.ValidPoint(location) || Envir.Random.Next(4) >= magic.Level + 1 || !Teleport(CurrentMap, location, false)) return;
                        CurrentMap.Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.Teleport }, CurrentLocation);
                        LevelMagic(magic);
                        AddBuff(new Buff { Type = BuffType.Teleport, Caster = this, ExpireTime = Envir.Time + 30000 });
                    }
                    break;

                #endregion

                #region Hiding

                case Spell.Hiding:
                    for (int i = 0; i < Buffs.Count; i++)
                        if (Buffs[i].Type == BuffType.Hiding) return;

                    value = (int)data[1];
                    AddBuff(new Buff { Type = BuffType.Hiding, Caster = this, ExpireTime = Envir.Time + value * 1000 });
                    LevelMagic(magic);
                    break;

                #endregion

                #region Haste

                case Spell.Haste:
                    AddBuff(new Buff { Type = BuffType.Haste, Caster = this, ExpireTime = Envir.Time + (magic.Level + 1) * 30000, Value = (magic.Level + 1) * 2 });
                    LevelMagic(magic);
                    break;

                #endregion

                #region Fury

                case Spell.Fury:
                    AddBuff(new Buff { Type = BuffType.Fury, Caster = this, ExpireTime = Envir.Time + 60000 + magic.Level * 10000, Value = 4, Visible = true });
                    LevelMagic(magic);
                    break;

                #endregion

                #region LightBody

                case Spell.LightBody:
                    AddBuff(new Buff { Type = BuffType.LightBody, Caster = this, ExpireTime = Envir.Time + (magic.Level + 1) * 30000, Value = (magic.Level + 1) * 2 });
                    LevelMagic(magic);
                    break;

                #endregion

                #region MagicShield

                case Spell.MagicShield:
                    if (MagicShield) return;
                    MagicShield = true;
                    MagicShieldLv = magic.Level;
                    MagicShieldTime = Envir.Time + (int)data[1] * 1000;
                    CurrentMap.Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.MagicShieldUp }, CurrentLocation);
                    LevelMagic(magic);
                    break;

                #endregion

                #region TurnUndead

                case Spell.TurnUndead:
                    monster = (MonsterObject)data[1];
                    if (monster == null || !monster.IsAttackTarget(this) || monster.CurrentMap != CurrentMap || monster.Node == null) return;
                    monster.LastHitter = this;
                    monster.LastHitTime = Envir.Time + 5000;
                    monster.EXPOwner = this;
                    monster.EXPOwnerTime = Envir.Time + 5000;
                    monster.Die();
                    LevelMagic(magic);
                    break;

                #endregion

                #region Purification

                case Spell.Purification:
                    target = (MapObject)data[1];

                    if (target == null || !target.IsFriendlyTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;
                    if (Envir.Random.Next(4) > magic.Level || target.PoisonList.Count == 0) return;

                    target.ExplosionInflictedTime = 0;//ArcherSpells - DelayedExplosion
                    target.ExplosionInflictedStage = 0;//ArcherSpells - DelayedExplosion

                    target.PoisonList.Clear();
                    target.OperateTime = 0;

                    if(target.ObjectID == ObjectID)
                        Enqueue(new S.RemoveDelayedExplosion { ObjectID = target.ObjectID });//ArcherSpells - DelayedExplosion
                    target.Broadcast(new S.RemoveDelayedExplosion { ObjectID = target.ObjectID });//ArcherSpells - DelayedExplosion

                    LevelMagic(magic);
                    break;

                #endregion

                #region Revelation

                case Spell.Revelation:
                    value = (int)data[1];
                    target = (MapObject)data[2];
                    if (target == null || target.CurrentMap != CurrentMap || target.Node == null) return;
                    if (target.Race != ObjectType.Player && target.Race != ObjectType.Monster) return;
                    if (Envir.Random.Next(4) > magic.Level || Envir.Time < target.RevTime) return;

                    target.RevTime = Envir.Time + value * 1000;
                    target.OperateTime = 0;
                    target.BroadcastHealthChange();

                    LevelMagic(magic);
                    break;

                #endregion

                #region Reincarnation

                case Spell.Reincarnation:

                    if (ReincarnationReady)
                    {
                        ReincarnationTarget.Enqueue(new S.RequestReincarnation {});
                        LevelMagic(magic);
                        ReincarnationReady = false;
                    }
                    break;

                #endregion

                #region Entrapment

                case Spell.Entrapment:
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null || target.Race != ObjectType.Monster ||
                        Functions.MaxDistance(CurrentLocation, target.CurrentLocation) > 7 || target.Level >= Level + 5 + Envir.Random.Next(8)) return;

                    MirDirection pulldirection = (MirDirection)((byte)(Direction - 4) % 8);
                    int pulldistance = 0;
                    if ((byte)pulldirection % 2 > 0)
                        pulldistance = Math.Max(0, Math.Min(Math.Abs(CurrentLocation.X - target.CurrentLocation.X), Math.Abs(CurrentLocation.Y - target.CurrentLocation.Y)));
                    else
                        pulldistance = pulldirection == MirDirection.Up || pulldirection == MirDirection.Down ? Math.Abs(CurrentLocation.Y - target.CurrentLocation.Y) - 2 : Math.Abs(CurrentLocation.X - target.CurrentLocation.X) - 2;

                    int levelgap = target.Race == ObjectType.Player ? Level - target.Level + 4 : Level - target.Level + 9;
                    if (Envir.Random.Next(30) >= ((magic.Level + 1) * 3) + levelgap) return;

                    int duration = target.Race == ObjectType.Player ? (int)Math.Round((magic.Level + 1) * 1.6) : (int)Math.Round((magic.Level + 1) * 0.8);
                    if (duration > 0) target.ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = duration, TickSpeed = 1000 }, this);
                    CurrentMap.Broadcast(new S.ObjectEffect { ObjectID = target.ObjectID, Effect = SpellEffect.Entrapment }, target.CurrentLocation);
                    if (target.Pushed(this, pulldirection, pulldistance) > 0) LevelMagic(magic);
                    break;

                #endregion

                #region Hallucination

                case Spell.Hallucination:
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null ||
                        Functions.MaxDistance(CurrentLocation, target.CurrentLocation) > 7 || Envir.Random.Next(Level + 20 + magic.Level * 5) <= target.Level + 10) return;
                    item = GetAmulet(1);
                    if (item == null) return;

                    ((MonsterObject)target).HallucinationTime = Envir.Time + (Envir.Random.Next(20) + 10) * 1000;
                    target.Target = null;

                    ConsumeItem(item, 1);

                    LevelMagic(magic);
                    break;

                #endregion

                #region ElementalBarrier, ElementalShot

                case Spell.ElementalBarrier:
                    if (ElementalBarrier) return;
                    if (!HasElemental)
                    {
                        ObtainElement(true);//gather orb through casting
                        LevelMagic(magic);
                        return;
                    }

                    int barrierPower = GetElementalOrbPower(true);//defensive orbpower
                    //destroy orbs
                    ElementsLevel = 0;
                    ObtainElement(false);
                    LevelMagic(magic);
                    //
                    ElementalBarrier = true;
                    ElementalBarrierLv = (byte)((int)magic.Level);//compensate for lower mc then wizard
                    ElementalBarrierTime = Envir.Time + ((int)data[1]  + barrierPower) * 1000;
                    CurrentMap.Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.ElementBarrierUp }, CurrentLocation);
                    break;

                case Spell.ElementalShot://ArcherSpells - Elemental system
                    
                   
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null)
                    {
                        //destroy orbs
                        ElementsLevel = 0;
                        ObtainElement(false);//update and send to client
                        return;
                    }
                    if (target.Attacked(this, value, DefenceType.MAC, false) > 0)
                        LevelMagic(magic);
                    DoKnockback(target, magic);//ElementalShot - Knockback
                    
                    //destroy orbs
                    ElementsLevel = 0;
                    ObtainElement(false);//update and send to client
                    break;

                #endregion

                #region DelayedExplosion

                case Spell.DelayedExplosion:
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;
                    if (target.Attacked(this, value, DefenceType.MAC, false) > 0) LevelMagic(magic);

                    target.ApplyPoison(new Poison
                    {
                        Duration = (value * 2) + (magic.Level + 1) * 7,
                        Owner = this,
                        PType = PoisonType.DelayedExplosion,
                        TickSpeed = 2000,
                        Value = value
                    }, this);

                    target.OperateTime = 0;
                    LevelMagic(magic);
                    break;

                #endregion

                #region BindingShot

                case Spell.BindingShot:
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;
                    if (((MonsterObject)target).ShockTime >= Envir.Time) return;//Already shocked

                    Point place = target.CurrentLocation;
                    MonsterObject centerTarget = null;

                    for (int y = place.Y - 1; y <= place.Y + 1; y++)
                    {
                        if (y < 0) continue;
                        if (y >= CurrentMap.Height) break;

                        for (int x = place.X - 1; x <= place.X + 1; x++)
                        {
                            if (x < 0) continue;
                            if (x >= CurrentMap.Width) break;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject targetob = cell.Objects[i];

                                if (y == place.Y && x == place.X && targetob.Race == ObjectType.Monster)
                                {
                                    centerTarget = (MonsterObject)targetob;
                                }

                                switch (targetob.Race)
                                {
                                    case ObjectType.Monster:
                                        if (targetob == null || !targetob.IsAttackTarget(this) || targetob.Node == null || targetob.Level > this.Level + 2) continue;

                                        MonsterObject mobTarget = (MonsterObject)targetob;

                                        if (centerTarget == null) centerTarget = mobTarget;

                                        mobTarget.ShockTime = Envir.Time + value;
                                        mobTarget.Target = null;
                                        break;
                                }
                            }
                        }
                    }

                    if (centerTarget == null) return;

                    //only the centertarget holds the effect
                    centerTarget.BindingShotCenter = true;
                    centerTarget.Broadcast(new S.SetBindingShot { ObjectID = centerTarget.ObjectID, Enabled = true, Value = value });

                    LevelMagic(magic);
                    break;

                #endregion

                #region VampireShot, PoisonShot, CrippleShot
                case Spell.VampireShot:
                case Spell.PoisonShot:
                case Spell.CrippleShot:
                    value = (int)data[1];
                    target = (MapObject)data[2];

                    if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;
                    if (target.Attacked(this, value, DefenceType.MAC, false) == 0) return;

                    int buffTime = 5 + (5 * magic.Level);

                    bool hasVampBuff = (Buffs.Where(x => x.Type == BuffType.VampireShot).ToList().Count() > 0);
                    bool hasPoisonBuff = (Buffs.Where(x => x.Type == BuffType.PoisonShot).ToList().Count() > 0);

                    bool doVamp = false, doPoison = false;
                    if (magic.Spell == Spell.VampireShot)
                    {
                        doVamp = true;
                        if (!hasVampBuff && !hasPoisonBuff && (Envir.Random.Next(20) >= 8))//40% chance
                        {
                            AddBuff(new Buff { Type = BuffType.VampireShot, Caster = this, ExpireTime = Envir.Time + (buffTime * 1000), Value = value, Visible = true, ObjectID = this.ObjectID });
                            BroadcastInfo();
                        }
                    }
                    if (magic.Spell == Spell.PoisonShot)
                    {
                        doPoison = true;
                        if (!hasPoisonBuff && !hasVampBuff && (Envir.Random.Next(20) >= 8))//40% chance
                        {
                            AddBuff(new Buff { Type = BuffType.PoisonShot, Caster = this, ExpireTime = Envir.Time + (buffTime * 1000), Value = value, Visible = true, ObjectID = this.ObjectID });
                            BroadcastInfo();
                        }
                    }
                    if (magic.Spell == Spell.CrippleShot)
                    {
                        if (hasVampBuff || hasPoisonBuff)
                        {
                            place = target.CurrentLocation;
                            for (int y = place.Y - 1; y <= place.Y + 1; y++)
                            {
                                if (y < 0) continue;
                                if (y >= CurrentMap.Height) break;
                                for (int x = place.X - 1; x <= place.X + 1; x++)
                                {
                                    if (x < 0) continue;
                                    if (x >= CurrentMap.Width) break;
                                    Cell cell = CurrentMap.GetCell(x, y);
                                    if (!cell.Valid || cell.Objects == null) continue;
                                    for (int i = 0; i < cell.Objects.Count; i++)
                                    {
                                        MapObject targetob = cell.Objects[i];
                                        if (targetob.Race != ObjectType.Monster && targetob.Race != ObjectType.Player) continue;
                                        if (targetob == null || !targetob.IsAttackTarget(this) || targetob.Node == null) continue;
                                        if (targetob.Dead) continue;

                                        if (hasVampBuff)//Vampire Effect
                                        {
                                            //cancel out buff
                                            AddBuff(new Buff { Type = BuffType.VampireShot, Caster = this, ExpireTime = Envir.Time + 1000, Value = value, Visible = true, ObjectID = this.ObjectID });

                                            target.Attacked(this, value, DefenceType.MAC, false);
                                            if (VampAmount == 0) VampTime = Envir.Time + 1000;
                                            VampAmount += (ushort)(value * (magic.Level + 1) * 0.25F);
                                        }
                                        if (hasPoisonBuff)//Poison Effect
                                        {
                                            //cancel out buff
                                            AddBuff(new Buff { Type = BuffType.PoisonShot, Caster = this, ExpireTime = Envir.Time + 1000, Value = value, Visible = true, ObjectID = this.ObjectID });

                                            targetob.ApplyPoison(new Poison
                                            {
                                                Duration = (value * 2) + (magic.Level + 1) * 7,
                                                Owner = this,
                                                PType = PoisonType.Green,
                                                TickSpeed = 2000,
                                                Value = value / 25 + magic.Level + 1 + Envir.Random.Next(PoisonAttack)
                                            }, this);
                                            targetob.OperateTime = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (doVamp)//Vampire Effect
                        {
                            if (VampAmount == 0) VampTime = Envir.Time + 1000;
                            VampAmount += (ushort)(value * (magic.Level + 1) * 0.25F);
                        }
                        if (doPoison)//Poison Effect
                        {
                            target.ApplyPoison(new Poison
                            {
                                Duration = (value * 2) + (magic.Level + 1) * 7,
                                Owner = this,
                                PType = PoisonType.Green,
                                TickSpeed = 2000,
                                Value = value / 25 + magic.Level + 1 + Envir.Random.Next(PoisonAttack)
                            }, this);
                            target.OperateTime = 0;
                        }
                    }

                    LevelMagic(magic);
                    break;
                #endregion

                #region ArcherSummons
                case Spell.SummonVampire:
                case Spell.SummonToad:
                case Spell.SummonSnakes:
                    value = (int)data[1];
                    location = (Point)data[2];
                    target = (MapObject)data[3];

                    int SummonType = 0;
                    switch (magic.Spell)
                    {
                        case Spell.SummonVampire:
                            SummonType = 1;
                            break;
                        case Spell.SummonToad:
                            SummonType = 2;
                            break;
                        case Spell.SummonSnakes:
                            SummonType = 3;
                            break;
                    }
                    if (SummonType == 0) return;

                    for (int i = 0; i < Pets.Count; i++)
                    {
                        monster = Pets[i];
                        if ((monster.Info.Name != (SummonType == 1 ? Settings.VampireName : (SummonType == 2 ? Settings.ToadName : Settings.SnakeTotemName))) || monster.Dead) continue;
                        if (monster.Node == null) continue;
                        monster.ActionList.Add(new DelayedAction(DelayedType.Recall, Envir.Time + 500, target));
                        monster.Target = target;
                        return;
                    }

                    if (Pets.Count > 1) return;

                    //left it in for future summon amulets
                    //UserItem item = GetAmulet(5);
                    //if (item == null) return;

                    MonsterInfo info = Envir.GetMonsterInfo((SummonType == 1 ? Settings.VampireName : (SummonType == 2 ? Settings.ToadName : Settings.SnakeTotemName)));
                    if (info == null) return;

                    LevelMagic(magic);
                    //ConsumeItem(item, 5);

                    monster = MonsterObject.GetMonster(info);
                    monster.PetLevel = magic.Level;
                    monster.Master = this;
                    monster.MaxPetLevel = (byte)(1 + magic.Level * 2);
                    monster.Direction = Direction;
                    monster.ActionTime = Envir.Time + 1000;
                    monster.Target = target;
                    if (SummonType == 1)
                        ((Monsters.VampireSpider)monster).AliveTime = Envir.Time + ((magic.Level * 1500) + 15000);
                    if (SummonType == 2)
                        ((Monsters.SpittingToad)monster).AliveTime = Envir.Time + ((magic.Level * 2000) + 25000);
                    if (SummonType == 3)
                        ((Monsters.SnakeTotem)monster).AliveTime = Envir.Time + ((magic.Level * 1500) + 20000);

                    Pets.Add(monster);

                    DelayedAction action = new DelayedAction(DelayedType.Magic, Envir.Time + 500, this, magic, monster, location);
                    CurrentMap.ActionList.Add(action);
                    break;
                #endregion

            }


        }
        private void CompleteMine(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            if (target == null) return;
            target.Broadcast(new S.MapEffect { Effect = SpellEffect.Mine, Location = target.CurrentLocation, Value = (byte)Direction });
            //target.Broadcast(new S.ObjectEffect { ObjectID = target.ObjectID, Effect = SpellEffect.Mine });
            if ((byte)target.Direction < 6)
                target.Direction++;
            target.Broadcast(target.GetInfo());
        }
        private void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool damageWeapon = (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (target.Attacked(this, damage, defence, damageWeapon) <= 0) return;

            //Level Fencing / SpiritSword
            foreach (UserMagic magic in Info.Magics)
            {
                switch (magic.Spell)
                {
                    case Spell.Fencing:
                    case Spell.SpiritSword:
                        LevelMagic(magic);
                        break;
                }
            }
        }
        private void CompleteNPC(IList<object> data)
        {
            uint npcid = (uint)data[0];
            string page = (string)data[1];

            if (data.Count > 3)
            {
                Map map = (Map)data[2];
                Point coords = (Point)data[3];

                Teleport(map, coords);
            }

            NPCDelayed = true;

            if (page.Length > 0)
                CallNPC(npcid, page);
        }

        private void CompletePoison(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            PoisonType pt = (PoisonType)data[1];
            SpellEffect sp = (SpellEffect)data[2];
            int duration = (int)data[3];
            int tickSpeed = (int)data[4];

            if (target == null) return;

            target.ApplyPoison(new Poison { PType = pt, Duration = duration, TickSpeed = tickSpeed }, this);
            target.Broadcast(new S.ObjectEffect { ObjectID = target.ObjectID, Effect = sp });
        }


        private UserItem GetAmulet(int count, int shape = 0)
        {
            for (int i = 0; i < Info.Equipment.Length; i++)
            {
                UserItem item = Info.Equipment[i];
                if (item != null && item.Info.Type == ItemType.Amulet && item.Info.Shape == shape && item.Count >= count)
                    return item;
            }

            return null;
        }
        private UserItem GetPoison(int count, byte shape = 0)
        {
            for (int i = 0; i < Info.Equipment.Length; i++)
            {
                UserItem item = Info.Equipment[i];
                if (item != null && item.Info.Type == ItemType.Amulet && item.Count >= count)
                {
                    if (shape == 0)
                    {
                        if (item.Info.Shape == 1 || item.Info.Shape == 2)
                            return item;
                    }
                    else
                    {
                        if (item.Info.Shape == shape)
                            return item;
                    }
                }
            }

            return null;
        }
        private UserItem GetBait(int count)
        {
            UserItem item = Info.Equipment[(int)EquipmentSlot.Weapon];
            if (item == null || item.Info.Type != ItemType.Weapon || (item.Info.Shape != 49 && item.Info.Shape != 50)) return null;

            UserItem bait = item.Slots[(int)FishingSlot.Bait];

            if (bait == null || bait.Count < count) return null;

            return bait;
        }

        private UserItem GetFishingItem(FishingSlot type)
        {
            UserItem item = Info.Equipment[(int)EquipmentSlot.Weapon];
            if (item == null || item.Info.Type != ItemType.Weapon || (item.Info.Shape != 49 && item.Info.Shape != 50)) return null;

            UserItem fishingItem = item.Slots[(int)type];

            if (fishingItem == null) return null;

            return fishingItem;
        }

        private void DeleteFishingItem(FishingSlot type)
        {
            UserItem item = Info.Equipment[(int)EquipmentSlot.Weapon];
            if (item == null || item.Info.Type != ItemType.Weapon || (item.Info.Shape != 49 && item.Info.Shape != 50)) return;
            Enqueue(new S.DeleteItem { UniqueID = Info.Equipment[(int)EquipmentSlot.Weapon].Slots[(int)type].UniqueID, Count = 1 });
            Info.Equipment[(int)EquipmentSlot.Weapon].Slots[(int)type] = null;
        }

        private void DamagedFishingItem(FishingSlot type, int lossDura)
        {
            UserItem item = GetFishingItem(type);

            if (item != null)
            {
                if (item.CurrentDura <= 0)
                {

                    DeleteFishingItem(type);
                }
                else
                {
                    DamageItem(item, lossDura, true);
                }
            }
        }

        private UserMagic GetMagic(Spell spell)
        {
            for (int i = 0; i < Info.Magics.Count; i++)
            {
                UserMagic magic = Info.Magics[i];
                if (magic.Spell != spell) continue;
                return magic;
            }


            return null;
        }

        public void LevelMagic(UserMagic magic)
        {
            byte exp = (byte)(Envir.Random.Next(3) + 1);
            if (HasSkillNecklace) exp *= 3;
            if (Level == 255) exp = byte.MaxValue;

            switch (magic.Level)
            {
                case 0:
                    if (Level < magic.Info.Level1)
                        return;

                    magic.Experience += exp;
                    if (magic.Experience >= magic.Info.Need1)
                    {
                        magic.Level++;
                        magic.Experience = (ushort)(magic.Experience - magic.Info.Need1);
                        RefreshStats();
                    }
                    break;
                case 1:
                    if (Level < magic.Info.Level2)
                        return;

                    magic.Experience += exp;
                    if (magic.Experience >= magic.Info.Need2)
                    {
                        magic.Level++;
                        magic.Experience = (ushort)(magic.Experience - magic.Info.Need2);
                        RefreshStats();
                    }
                    break;
                case 2:
                    if (Level < magic.Info.Level3)
                        return;

                    magic.Experience += exp;
                    if (magic.Experience >= magic.Info.Need3)
                    {
                        magic.Level++;
                        magic.Experience = 0;
                        RefreshStats();
                    }
                    break;
                default:
                    return;
            }

            Enqueue(new S.MagicLeveled { Spell = magic.Spell, Level = magic.Level, Experience = magic.Experience });

        }

        public bool CheckMovement(Point location)
        {
            //Script triggered coords
            for (int s = 0; s < CurrentMap.Info.ActiveCoords.Count; s++)
            {
                Point activeCoord = CurrentMap.Info.ActiveCoords[s];

                if (activeCoord != location) continue;

                CallDefaultNPC(DefaultNPCType.MapCoord, CurrentMap.Info.FileName, activeCoord.X, activeCoord.Y);
            }

            //Map movements
            for (int i = 0; i < CurrentMap.Info.Movements.Count; i++)
            {
                MovementInfo info = CurrentMap.Info.Movements[i];

                if (info.Source != location) continue;

                if (info.NeedHole)
                {
                    Cell cell = CurrentMap.GetCell(location);

                    if (cell.Objects == null ||
                        cell.Objects.Where(ob => ob.Race == ObjectType.Spell).All(ob => ((SpellObject)ob).Spell != Spell.DigOutZombie))
                        continue;
                }

                if (info.NeedMove) //use with ENTERMAP npc command
                {
                    NPCMoveMap = Envir.GetMap(info.MapIndex);
                    NPCMoveCoord = info.Destination;
                    continue;
                }

                Map temp = Envir.GetMap(info.MapIndex);

                if (temp == null || !temp.ValidPoint(info.Destination)) continue;
                
                CurrentMap.RemoveObject(this);
                Broadcast(new S.ObjectRemove { ObjectID = ObjectID });

                CompleteMapMovement(temp, info.Destination, CurrentMap, CurrentLocation);
                return true;
            }

            return false;
        }
        private void CompleteMapMovement(params object[] data)
        {
            if (this == null) return;
            Map temp = (Map)data[0];
            Point destination = (Point)data[1];
            Map checkmap = (Map)data[2];
            Point checklocation = (Point)data[3];

            if (CurrentMap != checkmap || CurrentLocation != checklocation) return;

            bool mapChanged = temp != CurrentMap;

            CurrentMap = temp;
            CurrentLocation = destination;

            CurrentMap.AddObject(this);

            Enqueue(new S.MapChanged
            {
                FileName = CurrentMap.Info.FileName,
                Title = CurrentMap.Info.Title,
                MiniMap = CurrentMap.Info.MiniMap,
                BigMap = CurrentMap.Info.BigMap,
                Lights = CurrentMap.Info.Light,
                Location = CurrentLocation,
                Direction = Direction,
                MapDarkLight = CurrentMap.Info.MapDarkLight
            });

            if (RidingMount) RefreshMount();

            GetObjects();

            SafeZoneInfo szi = CurrentMap.GetSafeZone(CurrentLocation);

            if (szi != null)
            {
                BindLocation = szi.Location;
                BindMapIndex = CurrentMapIndex;
                InSafeZone = true;
            }
            else
                InSafeZone = false;

            if (mapChanged)
            {
                CallDefaultNPC(DefaultNPCType.MapEnter, CurrentMap.Info.FileName);
            }
        }

        public override bool Teleport(Map temp, Point location, bool effects = true, byte effectnumber = 0)
        {
            bool mapChanged = temp != CurrentMap;

            if (!base.Teleport(temp, location, effects)) return false;

            Enqueue(new S.MapChanged
            {
                FileName = CurrentMap.Info.FileName,
                Title = CurrentMap.Info.Title,
                MiniMap = CurrentMap.Info.MiniMap,
                BigMap = CurrentMap.Info.BigMap,
                Lights = CurrentMap.Info.Light,
                Location = CurrentLocation,
                Direction = Direction,
                MapDarkLight = CurrentMap.Info.MapDarkLight
            });

            if (effects) Enqueue(new S.TeleportIn());

            if (TradePartner != null) TradeCancel();

            if (RidingMount) RefreshMount();

            GetObjectsPassive();

            SafeZoneInfo szi = CurrentMap.GetSafeZone(CurrentLocation);

            if (szi != null)
            {
                BindLocation = szi.Location;
                BindMapIndex = CurrentMapIndex;
                InSafeZone = true;
            }
            else
                InSafeZone = false;

            Fishing = false;
            Enqueue(GetFishInfo());

            if (mapChanged)
            {
                CallDefaultNPC(DefaultNPCType.MapEnter, CurrentMap.Info.FileName);
            }

            return true;            
        }


        public bool TeleportEscape(int attempts)
        {
            Map temp = Envir.GetMap(BindMapIndex);

            for (int i = 0; i < attempts; i++)
            {
                Point location = new Point(BindLocation.X + Envir.Random.Next(-100, 100),
                                           BindLocation.Y + Envir.Random.Next(-100, 100));

                if (Teleport(temp, location)) return true;
            }

            return false;
        }

        private Packet GetMountInfo()
        {
            return new S.MountUpdate
            {
                ObjectID = ObjectID,
                RidingMount = RidingMount,
                MountType = MountType
            };
        }


        private Packet GetUpdateInfo()
        {
            UpdateConcentration();

            return new S.PlayerUpdate
            {
                ObjectID = ObjectID,
                Weapon = Looks_Weapon,
                Armour = Looks_Armour,
                Light = Light,
                WingEffect = Looks_Wings
            };
        }

        public override Packet GetInfo() 
        {
            //should never use this but i leave it in for safety
            if (Observer) return null;

            return new S.ObjectPlayer
            {
                ObjectID = ObjectID,
                Name = CurrentMap.Info.NoNames ? "?????" : Name,
                NameColour = NameColour,
                GuildName = CurrentMap.Info.NoNames ? "?????" : MyGuild != null ? MyGuild.Name : "",
                GuildRankName = CurrentMap.Info.NoNames ? "?????" : MyGuildRank != null ? MyGuildRank.Name : "",
                Class = Class,
                Gender = Gender,
                Level = Level,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = Hair,
                Weapon = Looks_Weapon,
                Armour = Looks_Armour,
                Light = Light,
                Poison = CurrentPoison,
                Dead = Dead,
                Hidden = Hidden,
                Effect = MagicShield ? SpellEffect.MagicShieldUp : (ElementalBarrier ? SpellEffect.ElementBarrierUp : SpellEffect.None),//ArcherSpells - Elemental system
                WingEffect = Looks_Wings,
                MountType = MountType,
                RidingMount = RidingMount,
                Fishing = Fishing,

                //ArcherSpells - Elemental system
                ElementOrbEffect = (uint)GetElementalOrbCount(),
                ElementOrbLvl = (uint)ElementsLevel,
                ElementOrbMax = (uint)Settings.OrbsExpList[Settings.OrbsExpList.Count - 1],

                Buffs = Buffs.Where(d => d.Visible).Select(e => e.Type).ToList(),

                LevelEffects = LevelEffects
            };
        }

        public Packet GetInfoEx(PlayerObject player)
        {
            if (Observer) return null;

            return new S.ObjectPlayer
            {
                ObjectID = ObjectID,
                Name = CurrentMap.Info.NoNames ? "?????" : Name,
                NameColour = GetNameColour(player),
                GuildName = CurrentMap.Info.NoNames ? "?????" : MyGuild != null ? MyGuild.Name : "",
                GuildRankName = CurrentMap.Info.NoNames ? "?????" : MyGuildRank != null ? MyGuildRank.Name : "",
                Class = Class,
                Gender = Gender,
                Level = Level,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = Hair,
                Weapon = Looks_Weapon,
                Armour = Looks_Armour,
                Light = Light,
                Poison = CurrentPoison,
                Dead = Dead,
                Hidden = Hidden,
                Effect = MagicShield ? SpellEffect.MagicShieldUp : (ElementalBarrier ? SpellEffect.ElementBarrierUp : SpellEffect.None),//ArcherSpells - Elemental system
                WingEffect = Looks_Wings,
                MountType = MountType,
                RidingMount = RidingMount,
                Fishing = Fishing,
                
                //ArcherSpells - Elemental system
                ElementOrbEffect = (uint)GetElementalOrbCount(),
                ElementOrbLvl = (uint)ElementsLevel,
                ElementOrbMax = (uint)Settings.OrbsExpList[Settings.OrbsExpList.Count - 1],

                Buffs = Buffs.Where(d => d.Visible).Select(e => e.Type).ToList(),

                LevelEffects = LevelEffects
            };
        }


        public override bool IsAttackTarget(PlayerObject attacker)
        {
            if (attacker == null || attacker.Node == null) return false;
            if (Dead || InSafeZone || attacker.InSafeZone || attacker == this || GMGameMaster) return false;
            if (CurrentMap.Info.NoFight) return false;

            switch (attacker.AMode)
            {
                case AttackMode.All:
                    return true;
                case AttackMode.Group:
                    return GroupMembers == null || !GroupMembers.Contains(attacker);
                case AttackMode.Guild:
                    return MyGuild == null || MyGuild != attacker.MyGuild;
                case AttackMode.EnemyGuild:
                    return MyGuild != null && MyGuild.IsEnemy(attacker.MyGuild);
                case AttackMode.Peace:
                    return false;
                case AttackMode.RedBrown:
                    return PKPoints >= 200 || Envir.Time < BrownTime;
            }

            return true;
        }
        public override bool IsAttackTarget(MonsterObject attacker)
        {
            if (attacker == null || attacker.Node == null) return false;
            if (Dead || attacker.Master == this || GMGameMaster) return false;
            if (attacker.Info.AI == 6 || attacker.Info.AI == 58) return PKPoints >= 200;
            if (attacker.Master == null) return true;
            if (InSafeZone || attacker.Master.InSafeZone) return false;
            
            if (LastHitter != attacker.Master && attacker.Master.LastHitter != this)
            {
                bool target = false;

                for (int i = 0; i < attacker.Pets.Count; i++)
                {
                    if (attacker.Pets[i].EXPOwner != this) continue;

                    target = true;
                    break;
                }

                if (!target)
                    return false;
            }

            switch (attacker.Master.AMode)
            {
                case AttackMode.All:
                    return true;
                case AttackMode.Group:
                    return GroupMembers == null || !GroupMembers.Contains(attacker.Master);
                case AttackMode.Guild:
                    return true;
                case AttackMode.EnemyGuild:
                    return false;
                case AttackMode.Peace:
                    return false;
                case AttackMode.RedBrown:
                    return PKPoints >= 200 || Envir.Time < BrownTime;
            }

            return true;

        }
        public override bool IsFriendlyTarget(PlayerObject ally)
        {
            if (ally == this) return true;

            switch (ally.AMode)
            {
                case AttackMode.Group:
                    return GroupMembers != null && GroupMembers.Contains(ally);
                case AttackMode.RedBrown:
                    return PKPoints < 200 & Envir.Time > BrownTime;
                case AttackMode.Guild:
                    return false;
                case AttackMode.EnemyGuild:
                    return true;
            }
            return true;
        }
        public override bool IsFriendlyTarget(MonsterObject ally)
        {
            if (ally.Race != ObjectType.Monster) return false;
            if (ally.Master == null) return false;

            switch (ally.Master.Race)
            {
                case ObjectType.Player:
                    if (!ally.Master.IsFriendlyTarget(this)) return false;
                    break;
                case ObjectType.Monster:
                    return false;
            }

            return true;
        }
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            int armour = 0;

            if (Hidden)
            {
                for (int i = 0; i < Buffs.Count; i++)
                {
                    switch (Buffs[i].Type)
                    {
                        //case BuffType.Hiding:
                        case BuffType.MoonLight:
                        case BuffType.DarkBody:
                            Buffs[i].ExpireTime = 0;
                            break;
                    }
                }
            }

            switch (type)
            {
                case DefenceType.ACAgility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.AC:
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.MACAgility:
                    if ((Settings.PvpCanResistMagic) && (Envir.Random.Next(Settings.MagicResistWeight) < MagicResist)) return 0;
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetAttackPower(MinMAC, MaxMAC);
                    break;
                case DefenceType.MAC:
                    if ((Settings.PvpCanResistMagic) && (Envir.Random.Next(Settings.MagicResistWeight) < MagicResist)) return 0;
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.Agility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    break;
            }

            if (damageWeapon)
                attacker.DamageWeapon();

            if ((attacker.CriticalRate * Settings.CriticalRateWeight) > Envir.Random.Next(100))
            {
                CurrentMap.Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.Critical }, CurrentLocation);
                damage = Math.Min(int.MaxValue, damage + (int)Math.Floor(damage * (((double)attacker.CriticalDamage / (double)Settings.CriticalDamageWeight) * 10)));
            }

            if (Envir.Random.Next(100) < Reflect)
            {
                attacker.Attacked(this, damage, type, false);
                CurrentMap.Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.Reflect }, CurrentLocation);
                return 0;
            }



            if (MagicShield)
                damage -= damage * (MagicShieldLv + 2) / 10;

            if (ElementalBarrier)//ArcherSpells - Elemental system
                damage -= damage * (ElementalBarrierLv + 1) / 10;

            if (armour >= damage) return 0;

            MagicShieldTime -= (damage - armour) * 60;

            ElementalBarrierTime -= (damage - armour) * 60;//ArcherSpells - Elemental system

            if (attacker.LifeOnHit > 0)
                attacker.ChangeHP(attacker.LifeOnHit);

            if (attacker.HpDrainRate > 0)
            {
                attacker.HpDrain += Math.Max(0, ((float)(damage - armour) / 100) * attacker.HpDrainRate);
                if (attacker.HpDrain > 2)
                {
                    int HpGain = (int)Math.Floor(attacker.HpDrain);
                    attacker.ChangeHP(HpGain);
                    attacker.HpDrain -= HpGain;

                }
            }

            LastHitter = attacker;
            LastHitTime = Envir.Time + 10000;
            RegenTime = Envir.Time + RegenDelay;
            LogTime = Envir.Time + Globals.LogDelay;

            if (Envir.Time > BrownTime && PKPoints < 200 && !AtWar(attacker))
                attacker.BrownTime = Envir.Time + Settings.Minute;

            if (attacker.HasParalysisRing && 1 == Envir.Random.Next(1, 15))
                ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, attacker);
            byte LevelOffset = (byte)(Level > attacker.Level ? 0 : Math.Min(10, attacker.Level - Level));

            if ((attacker.Freezing > 0) && (Settings.PvpCanFreeze) && type != DefenceType.MAC && type != DefenceType.MACAgility)
            {
                if ((Envir.Random.Next(Settings.FreezingAttackWeight) < attacker.Freezing) && (Envir.Random.Next(LevelOffset) == 0))
                    ApplyPoison(new Poison { PType = PoisonType.Slow, Duration = Math.Min(10, (3 + Envir.Random.Next(attacker.Freezing))), TickSpeed = 1000 }, attacker);
            }

            if (attacker.PoisonAttack > 0)
            {
                if ((Envir.Random.Next(Settings.PoisonAttackWeight) < attacker.PoisonAttack) && (Envir.Random.Next(LevelOffset) == 0))
                    ApplyPoison(new Poison { PType = PoisonType.Green, Duration = 5, TickSpeed = 1000, Value = Math.Min(10, 3 + Envir.Random.Next(attacker.PoisonAttack)) }, attacker);
            }

            attacker.GatherElement();//ArcherSpells - Elemental system

            DamageDura();
            ActiveBlizzard = false;
            ActiveReincarnation = false;

            CounterAttack(GetMagic(Spell.CounterAttack), LastHitter);

            Enqueue(new S.Struck { AttackerID = attacker.ObjectID });
            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });

            ChangeHP(armour - damage);
            return damage - armour;
        }
        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            int armour = 0;

            if (Hidden)
            {
                for (int i = 0; i < Buffs.Count; i++)
                {
                    switch (Buffs[i].Type)
                    {
                        //case BuffType.Hiding:
                        case BuffType.MoonLight:
                        case BuffType.DarkBody:
                            Buffs[i].ExpireTime = 0;
                            break;
                    }
                }
            }

            switch (type)
            {
                case DefenceType.ACAgility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.AC:
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.MACAgility:
                    if (Envir.Random.Next(Settings.MagicResistWeight) < MagicResist) return 0;
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetAttackPower(MinMAC, MaxMAC);
                    break;
                case DefenceType.MAC:
                    if (Envir.Random.Next(Settings.MagicResistWeight) < MagicResist) return 0;
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.Agility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    break;
            }

            if (Envir.Random.Next(100) < Reflect)
            {
                attacker.Attacked(this, damage, type, false);
                CurrentMap.Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.Reflect }, CurrentLocation);
                return 0;
            }

            if (MagicShield)
                damage -= damage * (MagicShieldLv + 2) / 10;

            if (ElementalBarrier)//ArcherSpells - Elemental system
                damage -= damage * (ElementalBarrierLv +1 ) / 10;

            if (armour >= damage) return 0;

            MagicShieldTime -= (damage - armour) * 60;

            ElementalBarrierTime -= (damage - armour) * 60;//ArcherSpells - Elemental system

            LastHitter = attacker.Master ?? attacker;
            LastHitTime = Envir.Time + 10000;
            RegenTime = Envir.Time + RegenDelay;
            LogTime = Envir.Time + Globals.LogDelay;

            DamageDura();
            ActiveBlizzard = false;
            ActiveReincarnation = false;

            CounterAttack(GetMagic(Spell.CounterAttack), LastHitter);

            Enqueue(new S.Struck { AttackerID = attacker.ObjectID });
            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });
            //
            ChangeHP(armour - damage);
            return damage - armour;
        }


        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            int armour = 0;
            if (Hidden)
            {
                for (int i = 0; i < Buffs.Count; i++)
                {
                    switch (Buffs[i].Type)
                    {
                        //case BuffType.Hiding:
                        case BuffType.MoonLight:
                        case BuffType.DarkBody:
                            Buffs[i].ExpireTime = 0;
                            break;
                    }
                }
            }

            switch (type)
            {
                case DefenceType.ACAgility:
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.AC:
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.MACAgility:
                    armour = GetAttackPower(MinMAC, MaxMAC);
                    break;
                case DefenceType.MAC:
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.Agility:
                    break;
            }

            armour = (int)(armour * PoisonRate);
            if (MagicShield)
                damage -= damage * (MagicShieldLv + 2) / 10;

            if (ElementalBarrier)//ArcherSpells - Elemental system
                damage -= damage * (ElementalBarrierLv + 1) / 10;

            if (armour >= damage) return 0;

            MagicShieldTime -= (damage - armour) * 60;

            ElementalBarrierTime -= (damage - armour) * 60;//ArcherSpells - Elemental system
            RegenTime = Envir.Time + RegenDelay;
            LogTime = Envir.Time + Globals.LogDelay;

            DamageDura();
            ActiveBlizzard = false;
            ActiveReincarnation = false;
            Enqueue(new S.Struck { AttackerID = 0});
            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = 0, Direction = Direction, Location = CurrentLocation });

            ChangeHP(armour - damage);
            return damage - armour;
        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false)
        {
            if ((Caster != null) || (!NoResist))
                if (((Caster.Race != ObjectType.Player) || Settings.PvpCanResistPoison) && (Envir.Random.Next(Settings.PoisonResistWeight) < PoisonResist))
                    return;
            if (p.Owner != null && p.Owner.Race == ObjectType.Player && Envir.Time > BrownTime && PKPoints < 200)
                p.Owner.BrownTime = Envir.Time + Settings.Minute;
            if ((p.PType == PoisonType.Green) || (p.PType == PoisonType.Red)) p.Duration = Math.Max(0, p.Duration - PoisonRecovery);
            if (p.Duration == 0) return;

            for (int i = 0; i < PoisonList.Count; i++)
            {
                if (PoisonList[i].PType != p.PType) continue;
                if ((PoisonList[i].PType == PoisonType.Green) && (PoisonList[i].Value > p.Value)) return;//cant cast weak poison to cancel out strong poison
                if ((PoisonList[i].PType != PoisonType.Green) && ((PoisonList[i].Duration - PoisonList[i].Time) > p.Duration)) return;//cant cast 1 second poison to make a 1minute poison go away!
                if ((PoisonList[i].PType == PoisonType.Frozen) || (PoisonList[i].PType == PoisonType.Slow) || (PoisonList[i].PType == PoisonType.Paralysis)) return;//prevents mobs from being perma frozen/slowed
                if (p.PType == PoisonType.DelayedExplosion) return;
                ReceiveChat("You have been poisoned.", ChatType.System2);
                PoisonList[i] = p;
                return;
            }

            if (p.PType == PoisonType.DelayedExplosion)//ArcherSpells - DelayedExplosion
            {
                ExplosionInflictedTime = Envir.Time + 4000;
                Enqueue(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion });
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = SpellEffect.DelayedExplosion });
                ReceiveChat("You are a walking explosive.", ChatType.System);
            }
            else
                ReceiveChat("You have been poisoned.", ChatType.System2);

            PoisonList.Add(p);
        }
        public override void AddBuff(Buff b)
        {
            if (Buffs.Any(d => d.Infinite && d.Type == b.Type)) return; //cant overwrite infinite buff with regular buff

            base.AddBuff(b);

            S.AddBuff addBuff = new S.AddBuff { Type = b.Type, Caster = b.Caster.Name, Expire = b.ExpireTime - Envir.Time, Value = b.Value, Infinite = b.Infinite, ObjectID = ObjectID, Visible = b.Visible };
            Enqueue(addBuff);

            if (b.Visible) Broadcast(addBuff);

            RefreshStats();
        }

        public void DepositTradeItem(int from, int to)
        {
            S.DepositTradeItem p = new S.DepositTradeItem { From = from, To = to, Success = false };

            if (from < 0 || from >= Info.Inventory.Length)
            {
                Enqueue(p);
                return;
            }

            if (to < 0 || to >= Info.Trade.Length)
            {
                Enqueue(p);
                return;
            }

            UserItem temp = Info.Inventory[from];

            if (temp == null)
            {
                Enqueue(p);
                return;
            }

            if (temp.Info.Bind.HasFlag(BindMode.DontTrade))
            {
                Enqueue(p);
                return;
            }

            if (Info.Trade[to] == null)
            {
                Info.Trade[to] = temp;
                Info.Inventory[from] = null;
                RefreshBagWeight();
                TradeItem();

                p.Success = true;
                Enqueue(p);
                return;
            }
            Enqueue(p);

        }
        public void RetrieveTradeItem(int from, int to)
        {
            S.RetrieveTradeItem p = new S.RetrieveTradeItem { From = from, To = to, Success = false };

            if (from < 0 || from >= Info.Trade.Length)
            {
                Enqueue(p);
                return;
            }

            if (to < 0 || to >= Info.Inventory.Length)
            {
                Enqueue(p);
                return;
            }

            UserItem temp = Info.Trade[from];

            if (temp == null)
            {
                Enqueue(p);
                return;
            }

            if (temp.Weight + CurrentBagWeight > MaxBagWeight)
            {
                ReceiveChat("Too heavy to get back.", ChatType.System);
                Enqueue(p);
                return;
            }

            if (Info.Inventory[to] == null)
            {
                Info.Inventory[to] = temp;
                Info.Trade[from] = null;

                p.Success = true;
                RefreshBagWeight();
                TradeItem();

                Enqueue(p);
                return;
            }
            Enqueue(p);
        }

        public void EquipSlotItem(MirGridType grid, ulong id, int to, MirGridType gridTo)
        {
            S.EquipSlotItem p = new S.EquipSlotItem { Grid = grid, UniqueID = id, To = to, GridTo = gridTo, Success = false };

            UserItem Item = null;

            switch (gridTo)
            {
                case MirGridType.Mount:
                    Item = Info.Equipment[(int)EquipmentSlot.Mount];
                    break;
                case MirGridType.Fishing:
                    Item = Info.Equipment[(int)EquipmentSlot.Weapon];
                    break;
                default:
                    Enqueue(p);
                    return;
            }

            if (Item == null || Item.Slots == null)
            {
                Enqueue(p);
                return;
            }

            if (gridTo == MirGridType.Fishing && (Item.Info.Shape != 49 && Item.Info.Shape != 50))
            {
                Enqueue(p);
                return;
            }

            if (to < 0 || to >= Item.Slots.Length)
            {
                Enqueue(p);
                return;
            }

            if (Item.Slots[to] != null)
            {
                Enqueue(p);
                return;
            }

            UserItem[] array;
            switch (grid)
            {
                case MirGridType.Inventory:
                    array = Info.Inventory;
                    break;
                case MirGridType.Storage:
                    if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.StorageKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Enqueue(p);
                        return;
                    }
                    NPCObject ob = null;
                    for (int i = 0; i < CurrentMap.NPCs.Count; i++)
                    {
                        if (CurrentMap.NPCs[i].ObjectID != NPCID) continue;
                        ob = CurrentMap.NPCs[i];
                        break;
                    }

                    if (ob == null || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Globals.DataRange))
                    {
                        Enqueue(p);
                        return;
                    }
                    array = Account.Storage;
                    break;
                default:
                    Enqueue(p);
                    return;
            }


            int index = -1;
            UserItem temp = null;

            for (int i = 0; i < array.Length; i++)
            {
                temp = array[i];
                if (temp == null || temp.UniqueID != id) continue;
                index = i;
                break;
            }

            if (temp == null || index == -1)
            {
                Enqueue(p);
                return;
            }

            if ((temp.SoulBoundId != -1) && (temp.SoulBoundId != Info.Index))
            {
                Enqueue(p);
                return;
            }

            if (CanUseItem(temp))
            {
                if (temp.Info.NeedIdentify && !temp.Identified)
                {
                    temp.Identified = true;
                    Enqueue(new S.RefreshItem { Item = temp });
                }
                //if ((temp.Info.BindOnEquip) && (temp.SoulBoundId == -1))
                //{
                //    temp.SoulBoundId = Info.Index;
                //    Enqueue(new S.RefreshItem { Item = temp });
                //}
                //if (UnlockCurse && Info.Equipment[to].Cursed)
                //    UnlockCurse = false;

                Item.Slots[to] = temp;
                array[index] = null;

                p.Success = true;
                Enqueue(p);
                RefreshStats();

                return;
            }

            Enqueue(p);
        }

        public void RemoveItem(MirGridType grid, ulong id, int to)
        {
            S.RemoveItem p = new S.RemoveItem { Grid = grid, UniqueID = id, To = to, Success = false };
            UserItem[] array;
            switch (grid)
            {
                case MirGridType.Inventory:
                    array = Info.Inventory;
                    break;
                case MirGridType.Storage:
                    if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.StorageKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Enqueue(p);
                        return;
                    }
                    NPCObject ob = null;
                    for (int i = 0; i < CurrentMap.NPCs.Count; i++)
                    {
                        if (CurrentMap.NPCs[i].ObjectID != NPCID) continue;
                        ob = CurrentMap.NPCs[i];
                        break;
                    }

                    if (ob == null || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Globals.DataRange))
                    {
                        Enqueue(p);
                        return;
                    }
                    array = Account.Storage;
                    break;
                default:
                    Enqueue(p);
                    return;
            }

            if (to < 0 || to >= array.Length) return;

            UserItem temp = null;
            int index = -1;

            for (int i = 0; i < Info.Equipment.Length; i++)
            {
                temp = Info.Equipment[i];
                if (temp == null || temp.UniqueID != id) continue;
                index = i;
                break;
            }

            if (temp == null || index == -1)
            {
                Enqueue(p);
                return;
            }

            if (temp.Cursed && !UnlockCurse)
            {
                Enqueue(p);
                return;
            }

            if (!CanRemoveItem(grid, temp)) return;
            Info.Equipment[index] = null;
            if (temp.Cursed)
                UnlockCurse = false;
            if (array[to] == null)
            {
                array[to] = temp;
                p.Success = true;
                Enqueue(p);
                RefreshStats();
                Broadcast(GetUpdateInfo());
                return;
            }

            Enqueue(p);
        }
        public void MoveItem(MirGridType grid, int from, int to)
        {
            S.MoveItem p = new S.MoveItem { Grid = grid, From = from, To = to, Success = false };
            UserItem[] array;
            switch (grid)
            {
                case MirGridType.Inventory:
                    array = Info.Inventory;
                    break;
                case MirGridType.Storage:
                    if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.StorageKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Enqueue(p);
                        return;
                    }
                    NPCObject ob = null;
                    for (int i = 0; i < CurrentMap.NPCs.Count; i++)
                    {
                        if (CurrentMap.NPCs[i].ObjectID != NPCID) continue;
                        ob = CurrentMap.NPCs[i];
                        break;
                    }

                    if (ob == null || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Globals.DataRange))
                    {
                        Enqueue(p);
                        return;
                    }
                    array = Account.Storage;
                    break;
                case MirGridType.Trade:
                    array = Info.Trade;
                    TradeItem();
                    break;
                default:
                    Enqueue(p);
                    return;
            }

            if (from >= 0 && to >= 0 && from < array.Length && to < array.Length)
            {
                UserItem i = array[to];
                array[to] = array[from];
                array[from] = i;

                p.Success = true;
                Enqueue(p);
                return;
            }

            Enqueue(p);
        }
        public void StoreItem(int from, int to)
        {
            S.StoreItem p = new S.StoreItem { From = from, To = to, Success = false };

            if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.StorageKey, StringComparison.CurrentCultureIgnoreCase))
            {
                Enqueue(p);
                return;
            }
            NPCObject ob = null;
            for (int i = 0; i < CurrentMap.NPCs.Count; i++)
            {
                if (CurrentMap.NPCs[i].ObjectID != NPCID) continue;
                ob = CurrentMap.NPCs[i];
                break;
            }

            if (ob == null || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Globals.DataRange))
            {
                Enqueue(p);
                return;
            }


            if (from < 0 || from >= Info.Inventory.Length)
            {
                Enqueue(p);
                return;
            }

            if (to < 0 || to >= Account.Storage.Length)
            {
                Enqueue(p);
                return;
            }

            UserItem temp = Info.Inventory[from];

            if (temp == null)
            {
                Enqueue(p);
                return;
            }

            if (temp.Info.Bind.HasFlag(BindMode.DontStore))
            {
                Enqueue(p);
                return;
            }

            if (Account.Storage[to] == null)
            {
                Account.Storage[to] = temp;
                Info.Inventory[from] = null;
                RefreshBagWeight();

                p.Success = true;
                Enqueue(p);
                return;
            }
            Enqueue(p);
        }
        public void TakeBackItem(int from, int to)
        {
            S.TakeBackItem p = new S.TakeBackItem { From = from, To = to, Success = false };

            if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.StorageKey, StringComparison.CurrentCultureIgnoreCase))
            {
                Enqueue(p);
                return;
            }
            NPCObject ob = null;
            for (int i = 0; i < CurrentMap.NPCs.Count; i++)
            {
                if (CurrentMap.NPCs[i].ObjectID != NPCID) continue;
                ob = CurrentMap.NPCs[i];
                break;
            }

            if (ob == null || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Globals.DataRange))
            {
                Enqueue(p);
                return;
            }


            if (from < 0 || from >= Account.Storage.Length)
            {
                Enqueue(p);
                return;
            }

            if (to < 0 || to >= Info.Inventory.Length)
            {
                Enqueue(p);
                return;
            }

            UserItem temp = Account.Storage[from];

            if (temp == null)
            {
                Enqueue(p);
                return;
            }

            if (temp.Weight + CurrentBagWeight > MaxBagWeight)
            {
                ReceiveChat("Too heavy to get back.", ChatType.System);
                Enqueue(p);
                return;
            }

            if (Info.Inventory[to] == null)
            {
                Info.Inventory[to] = temp;
                Account.Storage[from] = null;

                p.Success = true;
                RefreshBagWeight();
                Enqueue(p);
                return;
            }
            Enqueue(p);
        }
        public void EquipItem(MirGridType grid, ulong id, int to)
        {
            S.EquipItem p = new S.EquipItem { Grid = grid, UniqueID = id, To = to, Success = false };

            if (Fishing)
            {
                Enqueue(p);
                return;
            }

            if (to < 0 || to >= Info.Equipment.Length)
            {
                Enqueue(p);
                return;
            }

            UserItem[] array;
            switch (grid)
            {
                case MirGridType.Inventory:
                    array = Info.Inventory;
                    break;
                case MirGridType.Storage:
                    if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.StorageKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Enqueue(p);
                        return;
                    }
                    NPCObject ob = null;
                    for (int i = 0; i < CurrentMap.NPCs.Count; i++)
                    {
                        if (CurrentMap.NPCs[i].ObjectID != NPCID) continue;
                        ob = CurrentMap.NPCs[i];
                        break;
                    }

                    if (ob == null || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Globals.DataRange))
                    {
                        Enqueue(p);
                        return;
                    }
                    array = Account.Storage;
                    break;
                default:
                    Enqueue(p);
                    return;
            }


            int index = -1;
            UserItem temp = null;

            for (int i = 0; i < array.Length; i++)
            {
                temp = array[i];
                if (temp == null || temp.UniqueID != id) continue;
                index = i;
                break;
            }

            if (temp == null || index == -1)
            {
                Enqueue(p);
                return;
            }
            if ((Info.Equipment[to] != null) && (Info.Equipment[to].Cursed) && (!UnlockCurse))
            {
                Enqueue(p);
                return;
            }

            if ((temp.SoulBoundId != -1) && (temp.SoulBoundId != Info.Index))
            {
                Enqueue(p);
                return;
            }

            if (CanEquipItem(temp, to))
            {
                if (temp.Info.NeedIdentify && !temp.Identified)
                {
                    temp.Identified = true;
                    Enqueue(new S.RefreshItem { Item = temp });
                }
                if ((temp.Info.BindOnEquip) && (temp.SoulBoundId == -1))
                {
                    temp.SoulBoundId = Info.Index;
                    Enqueue(new S.RefreshItem { Item = temp });
                }

                if ((Info.Equipment[to] != null) && (Info.Equipment[to].Cursed) && (UnlockCurse))
                    UnlockCurse = false;

                array[index] = Info.Equipment[to];
                Info.Equipment[to] = temp;
                p.Success = true;
                Enqueue(p);
                RefreshStats();
                //Broadcast(GetUpdateInfo());
                return;
            }
            Enqueue(p);
        }
        public void UseItem(ulong id)
        {
            S.UseItem p = new S.UseItem { UniqueID = id, Success = false };
            if (Dead)
            {
                Enqueue(p);
                return;
            }

            UserItem item = null;
            int index = -1;

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                item = Info.Inventory[i];
                if (item == null || item.UniqueID != id) continue;
                index = i;
                break;
            }

            if (item == null || index == -1 || !CanUseItem(item))
            {
                Enqueue(p);
                return;
            }

            switch (item.Info.Type)
            {
                case ItemType.Potion:
                    switch (item.Info.Shape)
                    {
                        case 0: //NormalPotion
                            PotHealthAmount = (ushort)Math.Min(ushort.MaxValue, PotHealthAmount + item.Info.HP);
                            PotManaAmount = (ushort)Math.Min(ushort.MaxValue, PotManaAmount + item.Info.MP);
                            break;
                        case 1: //SunPotion
                            ChangeHP(item.Info.HP);
                            ChangeMP(item.Info.MP);
                            break;
                        case 2: //MysteryWater
                            if (UnlockCurse)
                            {
                                ReceiveChat("You can already unequip a cursed item.", ChatType.Hint);
                                Enqueue(p);
                                return;
                            }
                            ReceiveChat("You can now unequip a cursed item.", ChatType.Hint);
                            UnlockCurse = true;
                            break;
                        case 3:
                            int time = item.Info.Durability;

                            if ((item.Info.MaxDC + item.DC) > 0)
                                AddBuff(new Buff { Type = BuffType.Impact, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.MaxDC + item.DC });

                            if ((item.Info.MaxMC + item.MC) > 0)
                                AddBuff(new Buff { Type = BuffType.Magic, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.MaxMC + item.MC });

                            if ((item.Info.MaxSC + item.SC) > 0)
                                AddBuff(new Buff { Type = BuffType.Taoist, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.MaxSC + item.SC });

                            if ((item.Info.AttackSpeed + item.AttackSpeed) > 0)
                                AddBuff(new Buff { Type = BuffType.Storm, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.AttackSpeed + item.AttackSpeed });

                            if ((item.Info.HP + item.HP) > 0)
                                AddBuff(new Buff { Type = BuffType.HealthAid, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.HP + item.HP });

                            if ((item.Info.MP + item.MP) > 0)
                                AddBuff(new Buff { Type = BuffType.ManaAid, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.MP + item.MP });

                            break;
                    }
                    break;
                case ItemType.Scroll:
                    UserItem temp;
                    switch (item.Info.Shape)
                    {
                        case 0: //DE
                            if (!TeleportEscape(20))
                            {
                                Enqueue(p);
                                return;
                            }
                            break;
                        case 1: //TT
                            if (!Teleport(Envir.GetMap(BindMapIndex), BindLocation))
                            {
                                Enqueue(p);
                                return;
                            }
                            break;
                        case 2: //RT
                            if (!TeleportRandom(200, item.Info.Durability))
                            {
                                Enqueue(p);
                                return;
                            }
                            break;
                        case 3: //BenedictionOil
                            if (!TryLuckWeapon())
                            {
                                Enqueue(p);
                                return;
                            }
                            break;
                        case 4: //RepairOil
                            temp = Info.Equipment[(int)EquipmentSlot.Weapon];
                            if (temp == null || temp.MaxDura == temp.CurrentDura)
                            {
                                Enqueue(p);
                                return;
                            }
                            if (temp.Info.Bind.HasFlag(BindMode.DontRepair))
                            {
                                Enqueue(p);
                                return;
                            }
                            temp.MaxDura = (ushort)Math.Max(0, temp.MaxDura - Math.Min(5000, temp.MaxDura - temp.CurrentDura) / 30);

                            temp.CurrentDura = (ushort)Math.Min(temp.MaxDura, temp.CurrentDura + 5000);
                            temp.DuraChanged = false;

                            ReceiveChat("Your weapon has been partially repaired", ChatType.Hint);
                            Enqueue(new S.ItemRepaired { UniqueID = temp.UniqueID, MaxDura = temp.MaxDura, CurrentDura = temp.CurrentDura });
                            break;
                        case 5: //WarGodOil
                            temp = Info.Equipment[(int)EquipmentSlot.Weapon];
                            if (temp == null || temp.MaxDura == temp.CurrentDura)
                            {
                                Enqueue(p);
                                return;
                            }
                            if (temp.Info.Bind.HasFlag(BindMode.DontRepair) || temp.Info.BindNoSRepair)
                            {
                                Enqueue(p);
                                return;
                            }
                            temp.CurrentDura = temp.MaxDura;
                            temp.DuraChanged = false;

                            ReceiveChat("Your weapon has been completely repaired", ChatType.Hint);
                            Enqueue(new S.ItemRepaired { UniqueID = temp.UniqueID, MaxDura = temp.MaxDura, CurrentDura = temp.CurrentDura });
                            break;
                    }
                    break;
                case ItemType.Book:
                    UserMagic magic = new UserMagic((Spell)item.Info.Shape);

                    if (magic.Info == null)
                    {
                        Enqueue(p);
                        return;
                    }

                    Info.Magics.Add(magic);
                    Enqueue(magic.GetInfo());
                    RefreshStats();
                    break;
                case ItemType.Script:
                    CallDefaultNPC(DefaultNPCType.UseItem, item.Info.Shape);
                    break;
                case ItemType.Food:
                    temp = Info.Equipment[(int)EquipmentSlot.Mount];
                    if (temp == null || temp.MaxDura == temp.CurrentDura)
                    {
                        Enqueue(p);
                        return;
                    }
                    
                    switch (item.Info.Shape)
                    {
                        case 0:
                            temp.MaxDura = (ushort)Math.Max(0, temp.MaxDura - Math.Min(1000, temp.MaxDura - (temp.CurrentDura / 30)));
                            break;
                    }

                    temp.CurrentDura = (ushort)Math.Min(temp.MaxDura, temp.CurrentDura + item.CurrentDura);
                    temp.DuraChanged = false;

                    ReceiveChat("Your mount has been fed.", ChatType.Hint);
                    Enqueue(new S.ItemRepaired { UniqueID = temp.UniqueID, MaxDura = temp.MaxDura, CurrentDura = temp.CurrentDura });

                    RefreshStats();
                    break;
                case ItemType.Pets://IntelligentCreature
                    if (item.Info.Shape >= 20)
                    {
                        switch (item.Info.Shape)
                        {
                            case 20://Mirror
                                Enqueue(new S.IntelligentCreatureEnableRename());
                                break;
                            case 21://BlackStone
                                if (item.Count > 1) item.Count--;
                                else Info.Inventory[index] = null;
                                RefreshBagWeight();
                                p.Success = true;
                                Enqueue(p);
                                BlackstoneRewardItem();
                                return;
                            case 22://Nuts
                                if (CreatureSummoned)
                                    for (int i = 0; i < Pets.Count; i++)
                                    {
                                        if (Pets[i].Info.AI != 64) continue;
                                        if (((IntelligentCreatureObject)Pets[i]).petType != SummonedCreatureType) continue;
                                        ((IntelligentCreatureObject)Pets[i]).maintainfoodTime = item.Info.Effect * Settings.Hour / 1000;
                                        UpdateCreatureMaintainFoodTime(SummonedCreatureType, 0);
                                        break;
                                    }
                                break;
                            case 23://FairyMoss, FreshwaterClam, Mackerel, Cherry
                                if (CreatureSummoned)
                                    for (int i = 0; i < Pets.Count; i++)
                                    {
                                        if (Pets[i].Info.AI != 64) continue;
                                        if (((IntelligentCreatureObject)Pets[i]).petType != SummonedCreatureType) continue;
                                        if (((IntelligentCreatureObject)Pets[i]).Fullness < 10000)
                                            ((IntelligentCreatureObject)Pets[i]).IncreaseFullness(item.Info.Effect * 100);
                                        break;
                                    }
                                break;
                            case 24://WonderPill
                                if (CreatureSummoned)
                                    for (int i = 0; i < Pets.Count; i++)
                                    {
                                        if (Pets[i].Info.AI != 64) continue;
                                        if (((IntelligentCreatureObject)Pets[i]).petType != SummonedCreatureType) continue;
                                        if (((IntelligentCreatureObject)Pets[i]).Fullness == 0)
                                            ((IntelligentCreatureObject)Pets[i]).IncreaseFullness(100);
                                        break;
                                    }
                                break;
                            case 25://Strongbox
                                byte boxtype = item.Info.Effect;
                                if (item.Count > 1) item.Count--;
                                else Info.Inventory[index] = null;
                                RefreshBagWeight();
                                p.Success = true;
                                Enqueue(p);
                                StrongboxRewardItem(boxtype);
                                return;
                            case 26://Wonderdrugs + Knapsack
                                int time = item.Info.Durability * 3600;
                                switch (item.Info.Effect)
                                {
                                    case 0://exp low/med/high
                                        AddBuff(new Buff { Type = BuffType.Exp, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.Luck + item.Luck });
                                        break;
                                    case 1://drop low/med/high
                                        AddBuff(new Buff { Type = BuffType.Drop, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.Luck + item.Luck });
                                        break;
                                    case 2://hp low/med/high
                                        AddBuff(new Buff { Type = BuffType.HealthAid, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.HP + item.HP });
                                        break;
                                    case 3://mp low/med/high
                                        AddBuff(new Buff { Type = BuffType.ManaAid, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.MP + item.MP });
                                        break;
                                    case 4://ac-ac low/med/high
                                        AddBuff(new Buff { Type = BuffType.WonderShield, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.MaxAC + item.AC });
                                        break;
                                    case 5://mac-mac low/med/high
                                        AddBuff(new Buff { Type = BuffType.MagicWonderShield, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.MaxMAC + item.MAC });
                                        break;
                                    case 6://speed low/med/high
                                        AddBuff(new Buff { Type = BuffType.Storm, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.AttackSpeed + item.AttackSpeed });
                                        break;
                                    case 7://knapsack low/med/high
                                        AddBuff(new Buff { Type = BuffType.BagWeight, Caster = this, ExpireTime = Envir.Time + time * 1000, Value = item.Info.Luck + item.Luck });
                                        break;
                                }
                                break;
                            case 27://FortuneCookies
                                break;
                        }
                    }
                    else
                    {
                        int slotIndex = Info.IntelligentCreatures.Count;
                        UserIntelligentCreature petInfo = new UserIntelligentCreature((IntelligentCreatureType)item.Info.Shape, slotIndex, item.Info.Effect);
                        if (Info.CheckHasIntelligentCreature((IntelligentCreatureType)item.Info.Shape))
                        {
                            ReceiveChat("You already have this creature.", ChatType.Hint);
                            petInfo = null;
                        }

                        if (petInfo == null || slotIndex >= 10)
                        {
                            Enqueue(p);
                            return;
                        }

                        ReceiveChat("Obtained a new creature {" + petInfo.CustomName + "}.", ChatType.Hint);

                        Info.IntelligentCreatures.Add(petInfo);
                        Enqueue(petInfo.GetInfo());
                    }
                    break;
                default:
                    return;
            }

            if (item.Count > 1) item.Count--;
            else Info.Inventory[index] = null;
            RefreshBagWeight();

            p.Success = true;
            Enqueue(p);
        }
        public void SplitItem(MirGridType grid, ulong id, uint count)
        {
            S.SplitItem1 p = new S.SplitItem1 { Grid = grid, UniqueID = id, Count = count, Success = false };
            UserItem[] array;
            switch (grid)
            {
                case MirGridType.Inventory:
                    array = Info.Inventory;
                    break;
                case MirGridType.Storage:
                    if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.StorageKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Enqueue(p);
                        return;
                    }
                    NPCObject ob = null;
                    for (int i = 0; i < CurrentMap.NPCs.Count; i++)
                    {
                        if (CurrentMap.NPCs[i].ObjectID != NPCID) continue;
                        ob = CurrentMap.NPCs[i];
                        break;
                    }

                    if (ob == null || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Globals.DataRange))
                    {
                        Enqueue(p);
                        return;
                    }
                    array = Account.Storage;
                    break;
                default:
                    Enqueue(p);
                    return;
            }

            UserItem temp = null;


            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null || array[i].UniqueID != id) continue;
                temp = array[i];
                break;
            }

            if (temp == null || count >= temp.Count || FreeSpace(array) == 0)
            {
                Enqueue(p);
                return;
            }

            temp.Count -= count;

            temp = Envir.CreateFreshItem(temp.Info);
            temp.Count = count;


            p.Success = true;
            Enqueue(p);
            Enqueue(new S.SplitItem { Item = temp, Grid = grid });

            if (grid == MirGridType.Inventory && (temp.Info.Type == ItemType.Potion || temp.Info.Type == ItemType.Scroll))
            {
                for (int i = 40; i < array.Length; i++)
                {
                    if (array[i] != null) continue;
                    array[i] = temp;
                    RefreshBagWeight();
                    return;
                }
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null) continue;
                array[i] = temp;
                RefreshBagWeight();
                return;
            }
        }
        public void MergeItem(MirGridType gridFrom, MirGridType gridTo, ulong fromID, ulong toID)
        {
            S.MergeItem p = new S.MergeItem { GridFrom = gridFrom, GridTo = gridTo, IDFrom = fromID, IDTo = toID, Success = false };

            UserItem[] arrayFrom;
            switch (gridFrom)
            {
                case MirGridType.Inventory:
                    arrayFrom = Info.Inventory;
                    break;
                case MirGridType.Storage:
                    if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.StorageKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Enqueue(p);
                        return;
                    }
                    NPCObject ob = null;
                    for (int i = 0; i < CurrentMap.NPCs.Count; i++)
                    {
                        if (CurrentMap.NPCs[i].ObjectID != NPCID) continue;
                        ob = CurrentMap.NPCs[i];
                        break;
                    }

                    if (ob == null || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Globals.DataRange))
                    {
                        Enqueue(p);
                        return;
                    }
                    arrayFrom = Account.Storage;
                    break;
                case MirGridType.Equipment:
                    arrayFrom = Info.Equipment;
                    break;
                default:
                    Enqueue(p);
                    return;
            }


            UserItem[] arrayTo;
            switch (gridTo)
            {
                case MirGridType.Inventory:
                    arrayTo = Info.Inventory;
                    break;
                case MirGridType.Storage:
                    if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.StorageKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Enqueue(p);
                        return;
                    }
                    NPCObject ob = null;
                    for (int i = 0; i < CurrentMap.NPCs.Count; i++)
                    {
                        if (CurrentMap.NPCs[i].ObjectID != NPCID) continue;
                        ob = CurrentMap.NPCs[i];
                        break;
                    }

                    if (ob == null || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Globals.DataRange))
                    {
                        Enqueue(p);
                        return;
                    }
                    arrayTo = Account.Storage;
                    break;
                case MirGridType.Equipment:
                    arrayTo = Info.Equipment;
                    break;
                case MirGridType.Fishing:
                    if (Info.Equipment[(int)EquipmentSlot.Weapon] == null || (Info.Equipment[(int)EquipmentSlot.Weapon].Info.Shape != 49 && Info.Equipment[(int)EquipmentSlot.Weapon].Info.Shape != 50))
                    {
                        Enqueue(p);
                        return;
                    }
                    arrayTo = Info.Equipment[(int)EquipmentSlot.Weapon].Slots;
                    break;
                default:
                    Enqueue(p);
                    return;
            }

            UserItem tempFrom = null;
            int index = -1;

            for (int i = 0; i < arrayFrom.Length; i++)
            {
                if (arrayFrom[i] == null || arrayFrom[i].UniqueID != fromID) continue;
                index = i;
                tempFrom = arrayFrom[i];
                break;
            }

            if (tempFrom == null || tempFrom.Info.StackSize == 1 || index == -1)
            {
                Enqueue(p);
                return;
            }


            UserItem tempTo = null;

            for (int i = 0; i < arrayTo.Length; i++)
            {
                if (arrayTo[i] == null || arrayTo[i].UniqueID != toID) continue;
                tempTo = arrayTo[i];
                break;
            }

            if (tempTo == null || tempTo.Info != tempFrom.Info || tempTo.Count == tempTo.Info.StackSize ||
                (tempTo.Info.Type != ItemType.Amulet && (gridFrom == MirGridType.Equipment || gridTo == MirGridType.Equipment)))
            {
                Enqueue(p);
                return;
            }

            if (tempFrom.Count <= tempTo.Info.StackSize - tempTo.Count)
            {
                tempTo.Count += tempFrom.Count;
                arrayFrom[index] = null;
            }
            else
            {
                tempFrom.Count -= tempTo.Info.StackSize - tempTo.Count;
                tempTo.Count = tempTo.Info.StackSize;
            }

            p.Success = true;
            Enqueue(p);
            RefreshStats();
        }
        private bool ValidGemForItem(UserItem Gem, byte itemtype)
        {
            switch (itemtype)
            {
                case 1: //weapon
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Paralize))
                        return true;
                    break;
                case 2: //Armour
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Teleport))
                        return true;
                    break;
                case 4: //Helmet
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Clearring))
                        return true;
                    break;
                case 5: //necklace
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Protection))
                        return true;
                    break;
                case 6: //bracelet
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Revival))
                        return true;
                    break;
                case 7: //ring
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Muscle))
                        return true;
                    break;
                case 8: //amulet
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Flame))
                        return true;
                    break;
                case 9://belt
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Healing))
                        return true;
                    break;
                case 10: //boots
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Probe))
                        return true;
                    break;
                case 11: //stone
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.Skill))
                        return true;
                    break;
                case 12:///torch
                    if (Gem.Info.Unique.HasFlag(SpecialItemMode.NoDuraLoss))
                        return true;
                    break;
            }
            return false;
        }

        private int GetCurrentStatCount(UserItem gem, UserItem item)
        {
            if ((gem.Info.MaxDC + gem.DC) > 0)
                return item.DC;
            else if ((gem.Info.MaxMC + gem.MC) > 0)
                return item.MC;
            else if ((gem.Info.MaxSC + gem.SC) > 0)
                return item.SC;
            else if ((gem.Info.MaxAC + gem.AC) > 0)
                return item.AC;
            else if ((gem.Info.MaxMAC + gem.MAC) > 0)
                return item.MAC;

            else if ((gem.Info.Durability) > 0)
                return item.Info.Durability > item.MaxDura ? 0 : ((item.MaxDura - item.Info.Durability) / 1000);

            else if ((gem.Info.AttackSpeed + gem.AttackSpeed) > 0)
                return item.AttackSpeed;

            else if ((gem.Info.Agility + gem.Agility) > 0)
                return item.Agility;

            else if ((gem.Info.Accuracy + gem.Accuracy) > 0)
                return item.Accuracy;

            else if ((gem.Info.PoisonAttack + gem.PoisonAttack) > 0)
                return item.PoisonAttack;

            else if ((gem.Info.Freezing + gem.Freezing) > 0)
                return item.Freezing;
            else if ((gem.Info.MagicResist + gem.MagicResist) > 0)
                return item.MagicResist;
            else if ((gem.Info.PoisonResist + gem.PoisonResist) > 0)
                return item.PoisonResist;
            return 0;
        }

        public void CombineItem(ulong fromID, ulong toID)
        {
            S.CombineItem p = new S.CombineItem { IDFrom = fromID, IDTo = toID, Success = false };

            UserItem[] array = Info.Inventory;
            UserItem tempFrom = null;
            UserItem tempTo = null;
            int indexFrom = -1;
            int indexTo = -1;

            if (Dead)
            {
                Enqueue(p);
                return;
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null || array[i].UniqueID != fromID) continue;
                indexFrom = i;
                tempFrom = array[i];
                break;
            }

            if (tempFrom == null || indexFrom == -1)
            {
                Enqueue(p);
                return;
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null || array[i].UniqueID != toID) continue;
                indexTo = i;
                tempTo = array[i];
                break;
            }

            if (tempTo == null || indexTo == -1)
            {
                Enqueue(p);
                return;
            }

            if((byte)tempTo.Info.Type < 1 || (byte)tempTo.Info.Type > 11)
            {
                Enqueue(p);
                return;
            }

            bool canRepair = false, canUpgrade = false;

            if (tempFrom.Info.Type != ItemType.Gem)
            {
                Enqueue(p);
                return;
            }

            switch (tempFrom.Info.Shape)
            {
                case 1: //BoneHammer
                case 2: //SewingSupplies
                case 5: //SpecialHammer
                case 6: //SpecialSewingSupplies

                    if (tempTo.Info.Bind.HasFlag(BindMode.DontRepair))
                    {
                        Enqueue(p);
                        return;
                    }

                    switch (tempTo.Info.Type)
                    {
						case ItemType.Weapon:
                        case ItemType.Necklace:
                        case ItemType.Ring:
                        case ItemType.Bracelet:
                            if (tempFrom.Info.Shape == 1 || tempFrom.Info.Shape == 5)
                                canRepair = true;
                            break;
                        case ItemType.Armour:
                        case ItemType.Helmet:
                        case ItemType.Boots:
                        case ItemType.Belt:
                            if (tempFrom.Info.Shape == 2 || tempFrom.Info.Shape == 6)
                                canRepair = true;
                            break;
                        default:
                            canRepair = false;
                            break;
                    }

                    if (canRepair != true)
                    {
                        Enqueue(p);
                        return;
                    }

                    if (tempTo.CurrentDura == tempTo.MaxDura)
                    {
                        ReceiveChat("Item does not need to be repaired.", ChatType.Hint);
                        Enqueue(p);
                        return;
                    }
                    break;
                case 3: //gems
                case 4: //orbs

                    if (tempTo.Info.Bind.HasFlag(BindMode.DontUpgrade) || tempTo.Info.Unique != SpecialItemMode.None)
                    {
                        Enqueue(p);
                        return;
                    }

                    if ((tempTo.GemCount >= tempFrom.Info.CriticalDamage) || (GetCurrentStatCount(tempFrom,tempTo) >= tempFrom.Info.HpDrainRate))
                    {
                        ReceiveChat("Item has already reached maximum added stats", ChatType.Hint);
                        Enqueue(p);
                        return;
                    }

                    int successchance = tempFrom.Info.Reflect * (int)tempTo.GemCount;
                    successchance = successchance >= tempFrom.Info.CriticalRate ? 0 : tempFrom.Info.CriticalRate - successchance;

                    //check if combine will succeed

                    bool succeeded = Envir.Random.Next(100) < successchance;
                    canUpgrade = true;

                    byte itemType = (byte)tempTo.Info.Type;

                    if (!ValidGemForItem(tempFrom, itemType))
                    {
                        ReceiveChat("Invalid combination", ChatType.Hint);
                        Enqueue(p);
                        return;
                    }
                    
                    if ((tempFrom.Info.MaxDC + tempFrom.DC) > 0)
                    {
                        if (succeeded) tempTo.DC = (byte)Math.Min(byte.MaxValue, tempTo.DC + tempFrom.Info.MaxDC + tempFrom.DC);
                    }

                    else if ((tempFrom.Info.MaxMC + tempFrom.MC) > 0)
                    {
                        if (succeeded) tempTo.MC = (byte)Math.Min(byte.MaxValue, tempTo.MC + tempFrom.Info.MaxMC + tempFrom.MC);
                    }

                    else if ((tempFrom.Info.MaxSC + tempFrom.SC) > 0)
                    {
                        if (succeeded) tempTo.SC = (byte)Math.Min(byte.MaxValue, tempTo.SC + tempFrom.Info.MaxSC + tempFrom.SC);
                    }

                    else if ((tempFrom.Info.MaxAC + tempFrom.AC) > 0)
                    {
                        if (succeeded) tempTo.AC = (byte)Math.Min(byte.MaxValue, tempTo.AC + tempFrom.Info.MaxAC + tempFrom.AC);
                    }

                    else if ((tempFrom.Info.MaxMAC + tempFrom.MAC) > 0)
                    {
                        if (succeeded) tempTo.MAC = (byte)Math.Min(byte.MaxValue, tempTo.MAC + tempFrom.Info.MaxMAC + tempFrom.MAC);
                    }

                    else if ((tempFrom.Info.Durability) > 0)
                    {
                        if (succeeded) tempTo.MaxDura = (ushort)Math.Min(ushort.MaxValue, tempTo.MaxDura + tempFrom.MaxDura);
                    }

                    else if ((tempFrom.Info.AttackSpeed + tempFrom.AttackSpeed) > 0)
                    {
                        if (succeeded) tempTo.AttackSpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, tempTo.AttackSpeed + tempFrom.Info.AttackSpeed + tempFrom.AttackSpeed)));
                    }

                    else if ((tempFrom.Info.Agility + tempFrom.Agility) > 0)
                    {
                        if (succeeded) tempTo.Agility = (byte)Math.Min(byte.MaxValue, tempFrom.Info.Agility + tempTo.Agility + tempFrom.Agility);
                    }

                    else if ((tempFrom.Info.Accuracy + tempFrom.Accuracy) > 0)
                    {
                        if (succeeded) tempTo.Accuracy = (byte)Math.Min(byte.MaxValue, tempFrom.Info.Accuracy + tempTo.Accuracy + tempFrom.Accuracy);
                    }

                    else if ((tempFrom.Info.PoisonAttack + tempFrom.PoisonAttack) > 0)
                    {
                        if (succeeded) tempTo.PoisonAttack = (byte)Math.Min(byte.MaxValue, tempFrom.Info.PoisonAttack + tempTo.PoisonAttack + tempFrom.PoisonAttack);
                    }

                    else if ((tempFrom.Info.Freezing + tempFrom.Freezing) > 0)
                    {
                        if (succeeded) tempTo.Freezing = (byte)Math.Min(byte.MaxValue, tempFrom.Info.Freezing + tempTo.Freezing + tempFrom.Freezing);
                    }

                    else if ((tempFrom.Info.MagicResist + tempFrom.MagicResist) > 0)
                    {
                        if (succeeded) tempTo.MagicResist = (byte)Math.Min(byte.MaxValue, tempFrom.Info.MagicResist + tempTo.MagicResist + tempFrom.MagicResist);
                    }

                    else if ((tempFrom.Info.PoisonResist + tempFrom.PoisonResist) > 0)
                    {
                        if (succeeded) tempTo.PoisonResist = (byte)Math.Min(byte.MaxValue, tempFrom.Info.PoisonResist + tempTo.PoisonResist + tempFrom.PoisonResist);
                    }
                    else
                    {
                        ReceiveChat("Cannot combine these items.", ChatType.Hint);
                        Enqueue(p);
                        return;
                    }

                    if (!succeeded)
                    {
                        
                        if ((tempFrom.Info.Shape == 3) && (Envir.Random.Next(15) < 3))
                        {
                            //item destroyed
                            ReceiveChat("Item has been destroyed.", ChatType.Hint);

                            Info.Inventory[indexTo] = null;
                            p.Destroy = true;
                        }
						else
						{
							//upgrade has no effect
                        	ReceiveChat("Upgrade has no effect.", ChatType.Hint);
						}

                        canUpgrade = false;
                    }
                    break;
                default:
                    Enqueue(p);
                    return;
            }

            

            RefreshBagWeight();

            if (canRepair && Info.Inventory[indexTo] != null)
            {
                switch (tempTo.Info.Shape)
                {
                    case 1:
                    case 2:
                        {
                            tempTo.MaxDura = (ushort)Math.Max(0, Math.Min(tempTo.MaxDura, tempTo.MaxDura - 100 * Envir.Random.Next(10)));
                        }
                        break;
                    default:
                        break;
                }
                tempTo.CurrentDura = tempTo.MaxDura;
                tempTo.DuraChanged = false;

                ReceiveChat("Item has been repaired.", ChatType.Hint);
                Enqueue(new S.ItemRepaired { UniqueID = tempTo.UniqueID, MaxDura = tempTo.MaxDura, CurrentDura = tempTo.CurrentDura });
            }

            if (canUpgrade && Info.Inventory[indexTo] != null)
            {
                tempTo.GemCount++;
                ReceiveChat("Item has been upgraded.", ChatType.Hint);
                Enqueue(new S.ItemUpgraded { Item = tempTo });
            }

            if (tempFrom.Count > 1) tempFrom.Count--;
            else Info.Inventory[indexFrom] = null;

            //item merged ok
            p.Success = true;
            Enqueue(p);
        }

        public void DropItem(ulong id, uint count)
        {
            S.DropItem p = new S.DropItem { UniqueID = id, Count = count, Success = false };
            if (Dead)
            {
                Enqueue(p);
                return;
            }

            if (CurrentMap.Info.NoThrowItem)
            {
                ReceiveChat("You cannot drop items on this map", ChatType.System);
                Enqueue(p);
                return;
            }

            UserItem temp = null;
            int index = -1;

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                temp = Info.Inventory[i];
                if (temp == null || temp.UniqueID != id) continue;
                index = i;
                break;
            }

            if (temp == null || index == -1 || count > temp.Count)
            {
                Enqueue(p);
                return;
            }

            if (temp.Info.Bind.HasFlag(BindMode.DontDrop))
            {
                Enqueue(p);
                return;
            }

            if (temp.Count == count)
            {
                if (!temp.Info.Bind.HasFlag(BindMode.DestroyOnDrop))
                    if (!DropItem(temp))
                    {
                        Enqueue(p);
                        return;
                    }
                Info.Inventory[index] = null;
            }
            else
            {
                UserItem temp2 = Envir.CreateFreshItem(temp.Info);
                temp2.Count = count;
                if (!temp.Info.Bind.HasFlag(BindMode.DestroyOnDrop))
                    if (!DropItem(temp2))
                    {
                        Enqueue(p);
                        return;
                    }
                temp.Count -= count;
            }
            p.Success = true;
            Enqueue(p);
            RefreshBagWeight();
        }
        public void DropGold(uint gold)
        {
            if (Account.Gold < gold) return;

            ItemObject ob = new ItemObject(this, gold);

            if (!ob.Drop(5)) return;
            Account.Gold -= gold;
            Enqueue(new S.LoseGold { Gold = gold });
        }
        public void PickUp()
        {
            if (Dead)
            {
                //Send Fail
                return;
            }

            Cell cell = CurrentMap.GetCell(CurrentLocation);

            bool sendFail = false;

            for (int i = 0; i < cell.Objects.Count; i++)
            {
                MapObject ob = cell.Objects[i];
                
                if (ob.Race != ObjectType.Item) continue;

                if (ob.Owner != null && ob.Owner != this && !IsGroupMember(ob.Owner)) //Or Group member.
                {
                    sendFail = true;
                    continue;
                }
                ItemObject item = (ItemObject)ob;

                if (item.Item != null)
                {
                    if (!CanGainItem(item.Item)) continue;

                    if (item.Item.Info.ShowGroupPickup && IsGroupMember(this))
                        for (int j = 0; j < GroupMembers.Count; j++)
                            GroupMembers[j].ReceiveChat(Name + " Picked up: {" + item.Item.Name + "}",
                                ChatType.System);

                    GainItem(item.Item);

                    CurrentMap.RemoveObject(ob);
                    ob.Despawn();                  

                    return;
                }

                if (!CanGainGold(item.Gold)) continue;

                GainGold(item.Gold);
                CurrentMap.RemoveObject(ob);
                ob.Despawn();
                return;
            }

            if (sendFail)
                ReceiveChat("Can not pick up, You do not own this item.", ChatType.System);

        }

        private bool IsGroupMember(MapObject player)
        {
            if (player.Race != ObjectType.Player) return false;
            return GroupMembers != null && GroupMembers.Contains(player);
        }

        public override bool CanGainGold(uint gold)
        {
            return (UInt64)gold + Account.Gold <= uint.MaxValue;
        }
        public override void WinGold(uint gold)
        {
            if (GroupMembers == null)
            {
                GainGold(gold);
                return;
            }

            uint count = 0;

            for (int i = 0; i < GroupMembers.Count; i++)
            {
                PlayerObject player = GroupMembers[i];
                if (player.CurrentMap == CurrentMap && Functions.InRange(player.CurrentLocation, CurrentLocation, Globals.DataRange) && !player.Dead)
                    count++;
            }

            if (count == 0 || count > gold)
            {
                GainGold(gold);
                return;
            }
            gold = gold / count;

            for (int i = 0; i < GroupMembers.Count; i++)
            {
                PlayerObject player = GroupMembers[i];
                if (player.CurrentMap == CurrentMap && Functions.InRange(player.CurrentLocation, CurrentLocation, Globals.DataRange) && !player.Dead)
                    player.GainGold(gold);
            }
        }
        public void GainGold(uint gold)
        {
            if (gold == 0) return;

            if (((UInt64)Account.Gold + gold) > uint.MaxValue)
                gold = uint.MaxValue - Account.Gold;

            Account.Gold += gold;

            Enqueue(new S.GainedGold { Gold = gold });
        }

        public bool CanGainItem(UserItem item, bool useWeight = true)
        {
            if (item.Info.Type == ItemType.Amulet)
            {
                if (FreeSpace(Info.Inventory) > 0 && (CurrentBagWeight + item.Weight <= MaxBagWeight || !useWeight)) return true;

                uint count = item.Count;

                for (int i = 0; i < Info.Inventory.Length; i++)
                {
                    UserItem bagItem = Info.Inventory[i];

                    if (bagItem == null || bagItem.Info != item.Info) continue;

                    if (bagItem.Count + count <= bagItem.Info.StackSize) return true;

                    count -= bagItem.Info.StackSize - bagItem.Count;
                }

                return false;
            }

            if (useWeight && CurrentBagWeight + (item.Weight) > MaxBagWeight) return false;

            if (FreeSpace(Info.Inventory) > 0) return true;

            if (item.Info.StackSize > 1)
            {
                uint count = item.Count;

                for (int i = 0; i < Info.Inventory.Length; i++)
                {
                    UserItem bagItem = Info.Inventory[i];

                    if (bagItem.Info != item.Info) continue;

                    if (bagItem.Count + count <= bagItem.Info.StackSize) return true;

                    count -= bagItem.Info.StackSize - bagItem.Count;
                }
            }

            return false;
        }
        public bool CanGainItems(UserItem[] items)
        {
            int itemCount = items.Count(e => e != null);
            uint itemWeight = 0;
            uint stackOffset = 0;

            if (itemCount < 1) return true;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null) continue;

                itemWeight += items[i].Weight;

                if (items[i].Info.StackSize > 1)
                {
                    uint count = items[i].Count;

                    for (int u = 0; u < Info.Inventory.Length; u++)
                    {
                        UserItem bagItem = Info.Inventory[u];

                        if (bagItem == null || bagItem.Info != items[i].Info) continue;

                        if (bagItem.Count + count > bagItem.Info.StackSize) stackOffset++;
                    }
                }
            }

            if (CurrentBagWeight + (itemWeight) > MaxBagWeight) return false;
            if (FreeSpace(Info.Inventory) < itemCount + stackOffset) return false;

            return true;
        }

        public void GainItem(UserItem item)
        {
            //CheckItemInfo(item.Info);
            CheckItem(item);

            UserItem clonedItem = item.Clone();

            Enqueue(new S.GainedItem { Item = clonedItem }); //Cloned because we are probably going to change the amount.

            AddItem(item);
            RefreshBagWeight();

        }
        private bool DropItem(UserItem item, int range = 1)
        {
            ItemObject ob = new ItemObject(this, item);

            if (!ob.Drop(range)) return false;

            if (item.Info.Type == ItemType.Meat)
                item.CurrentDura = (ushort)Math.Max(0, item.CurrentDura - 2000);

            return true;
        }
        private bool CanUseItem(UserItem item)
        {
            if (item == null) return false;

            switch (Gender)
            {
                case MirGender.Male:
                    if (!item.Info.RequiredGender.HasFlag(RequiredGender.Male))
                    {
                        ReceiveChat("You are not Female.", ChatType.System);
                        return false;
                    }
                    break;
                case MirGender.Female:
                    if (!item.Info.RequiredGender.HasFlag(RequiredGender.Female))
                    {
                        ReceiveChat("You are not Male.", ChatType.System);
                        return false;
                    }
                    break;
            }

            switch (Class)
            {
                case MirClass.Warrior:
                    if (!item.Info.RequiredClass.HasFlag(RequiredClass.Warrior))
                    {
                        ReceiveChat("Warriors cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Wizard:
                    if (!item.Info.RequiredClass.HasFlag(RequiredClass.Wizard))
                    {
                        ReceiveChat("Wizards cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Taoist:
                    if (!item.Info.RequiredClass.HasFlag(RequiredClass.Taoist))
                    {
                        ReceiveChat("Taoists cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Assassin:
                    if (!item.Info.RequiredClass.HasFlag(RequiredClass.Assassin))
                    {
                        ReceiveChat("Assassins cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
            }

            switch (item.Info.RequiredType)
            {
                case RequiredType.Level:
                    if (Level < item.Info.RequiredAmount)
                    {
                        ReceiveChat("You are not a high enough level.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.AC:
                    if (MaxAC < item.Info.RequiredAmount)
                    {
                        ReceiveChat("You do not have enough AC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MAC:
                    if (MaxMAC < item.Info.RequiredAmount)
                    {
                        ReceiveChat("You do not have enough MAC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.DC:
                    if (MaxDC < item.Info.RequiredAmount)
                    {
                        ReceiveChat("You do not have enough DC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MC:
                    if (MaxMC < item.Info.RequiredAmount)
                    {
                        ReceiveChat("You do not have enough MC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.SC:
                    if (MaxSC < item.Info.RequiredAmount)
                    {
                        ReceiveChat("You do not have enough SC.", ChatType.System);
                        return false;
                    }
                    break;
            }

            switch (item.Info.Shape)
            {
                case 0:
                    if (CurrentMap.Info.NoEscape)
                    {
                        ReceiveChat("You cannot use Dungeon Escapes here", ChatType.System);
                        return false;
                    }
                    break;
                case 2:
                    if (CurrentMap.Info.NoRandom)
                    {
                        ReceiveChat("You cannot use Random Teleports here", ChatType.System);
                        return false;
                    }
                    break;
            }

            switch (item.Info.Type)
            {
                case ItemType.Potion:
                    if (CurrentMap.Info.NoDrug)
                    {
                        ReceiveChat("You cannot use Potions here", ChatType.System);
                        return false;
                    }
                    break;

                case ItemType.Book:
                    if (Info.Magics.Any(t => t.Spell == (Spell)item.Info.Shape))
                    {
                        return false;
                    }
                    break;
                case ItemType.Saddle:
                case ItemType.Ribbon:
                case ItemType.Bells:
                case ItemType.Mask:
                case ItemType.Reins:
                    if (Info.Equipment[(int)EquipmentSlot.Mount] == null)
                    {
                        ReceiveChat("Can only be used with a mount", ChatType.System);
                        return false;
                    }
                    break;
                case ItemType.Hook:
                case ItemType.Float:
                case ItemType.Bait:
                case ItemType.Finder:
                case ItemType.Reel:
                    if (Info.Equipment[(int)EquipmentSlot.Weapon] == null || 
                        (Info.Equipment[(int)EquipmentSlot.Weapon].Info.Shape != 49 && Info.Equipment[(int)EquipmentSlot.Weapon].Info.Shape != 50))
                    {
                        ReceiveChat("Can only be used with a fishing rod", ChatType.System);
                        return false;
                    }
                    break;
                case ItemType.Pets://IntelligentCreature
                    switch (item.Info.Shape)
                    {
                        case 20://mirror rename creature
                            if (Info.IntelligentCreatures.Count == 0) return false;
                            break;
                        case 21://creature stone
                            break;
                        case 22://nuts maintain food levels
                            if (!CreatureSummoned)
                            {
                                ReceiveChat("Can only be used with a creature summoned", ChatType.System);
                                return false;
                            }
                            break;
                        case 23://basic creature food
                            if (!CreatureSummoned)
                            {
                                ReceiveChat("Can only be used with a creature summoned", ChatType.System);
                                return false;
                            }
                            else
                            {
                                for (int i = 0; i < Pets.Count; i++)
                                {
                                    if (Pets[i].Info.AI != 64) continue;
                                    if (((IntelligentCreatureObject)Pets[i]).petType != SummonedCreatureType) continue;


                                    if (((IntelligentCreatureObject)Pets[i]).Fullness > 9900)
                                    {
                                        ReceiveChat(((IntelligentCreatureObject)Pets[i]).Name + " is not hungry", ChatType.System);
                                        return false;
                                    }
                                    return true;
                                }
                                return false;
                            }
                        case 24://wonderpill vitalize creature
                            if (!CreatureSummoned)
                            {
                                ReceiveChat("Can only be used with a creature summoned", ChatType.System);
                                return false;
                            }
                            else
                            {
                                for (int i = 0; i < Pets.Count; i++)
                                {
                                    if (Pets[i].Info.AI != 64) continue;
                                    if (((IntelligentCreatureObject)Pets[i]).petType != SummonedCreatureType) continue;


                                    if (((IntelligentCreatureObject)Pets[i]).Fullness > 0)
                                    {
                                        ReceiveChat(((IntelligentCreatureObject)Pets[i]).Name + " does not need to be vitalized", ChatType.System);
                                        return false;
                                    }
                                    return true;
                                }
                                return false;
                            }
                        case 25://Strongbox
                            break;
                        case 26://Wonderdrugs
                            break;
                        case 27://Fortunecookies
                            break;
                    }
                    break;
            }

            if (RidingMount && item.Info.Type != ItemType.Scroll && item.Info.Type != ItemType.Potion)
            {
                return false;
            }

            //if (item.Info.Type == ItemType.Book)
            //    for (int i = 0; i < Info.Magics.Count; i++)
            //        if (Info.Magics[i].Spell == (Spell)item.Info.Shape) return false;

            return true;
        }
        private bool CanEquipItem(UserItem item, int slot)
        {
            switch ((EquipmentSlot)slot)
            {
                case EquipmentSlot.Weapon:
                    if (item.Info.Type != ItemType.Weapon)
                        return false;
                    break;
                case EquipmentSlot.Armour:
                    if (item.Info.Type != ItemType.Armour)
                        return false;
                    break;
                case EquipmentSlot.Helmet:
                    if (item.Info.Type != ItemType.Helmet)
                        return false;
                    break;
                case EquipmentSlot.Torch:
                    if (item.Info.Type != ItemType.Torch)
                        return false;
                    break;
                case EquipmentSlot.Necklace:
                    if (item.Info.Type != ItemType.Necklace)
                        return false;
                    break;
                case EquipmentSlot.BraceletL:
                    if (item.Info.Type != ItemType.Bracelet)
                        return false;
                    break;
                case EquipmentSlot.BraceletR:
                    if (item.Info.Type != ItemType.Bracelet && item.Info.Type != ItemType.Amulet)
                        return false;
                    break;
                case EquipmentSlot.RingL:
                case EquipmentSlot.RingR:
                    if (item.Info.Type != ItemType.Ring)
                        return false;
                    break;
                case EquipmentSlot.Amulet:
                    if (item.Info.Type != ItemType.Amulet)// || item.Info.Shape == 0
                        return false;
                    break;
                case EquipmentSlot.Boots:
                    if (item.Info.Type != ItemType.Boots)
                        return false;
                    break;
                case EquipmentSlot.Belt:
                    if (item.Info.Type != ItemType.Belt)
                        return false;
                    break;
                case EquipmentSlot.Stone:
                    if (item.Info.Type != ItemType.Stone)
                        return false;
                    break;
                case EquipmentSlot.Mount:
                    if (item.Info.Type != ItemType.Mount)
                        return false;
                    break;
                default:
                    return false;
            }


            switch (Gender)
            {
                case MirGender.Male:
                    if (!item.Info.RequiredGender.HasFlag(RequiredGender.Male))
                        return false;
                    break;
                case MirGender.Female:
                    if (!item.Info.RequiredGender.HasFlag(RequiredGender.Female))
                        return false;
                    break;
            }


            switch (Class)
            {
                case MirClass.Warrior:
                    if (!item.Info.RequiredClass.HasFlag(RequiredClass.Warrior))
                        return false;
                    break;
                case MirClass.Wizard:
                    if (!item.Info.RequiredClass.HasFlag(RequiredClass.Wizard))
                        return false;
                    break;
                case MirClass.Taoist:
                    if (!item.Info.RequiredClass.HasFlag(RequiredClass.Taoist))
                        return false;
                    break;
                case MirClass.Assassin:
                    if (!item.Info.RequiredClass.HasFlag(RequiredClass.Assassin))
                        return false;
                    break;
            }

            switch (item.Info.RequiredType)
            {
                case RequiredType.Level:
                    if (Level < item.Info.RequiredAmount)
                        return false;
                    break;
                case RequiredType.AC:
                    if (MaxAC < item.Info.RequiredAmount)
                        return false;
                    break;
                case RequiredType.MAC:
                    if (MaxMAC < item.Info.RequiredAmount)
                        return false;
                    break;
                case RequiredType.DC:
                    if (MaxDC < item.Info.RequiredAmount)
                        return false;
                    break;
                case RequiredType.MC:
                    if (MaxMC < item.Info.RequiredAmount)
                        return false;
                    break;
                case RequiredType.SC:
                    if (MaxSC < item.Info.RequiredAmount)
                        return false;
                    break;
            }

            if (item.Info.Type == ItemType.Weapon || item.Info.Type == ItemType.Torch)
            {
                if (item.Weight - (Info.Equipment[slot] != null ? Info.Equipment[slot].Weight : 0) + CurrentHandWeight > MaxHandWeight)
                    return false;
            }
            else
                if (item.Weight - (Info.Equipment[slot] != null ? Info.Equipment[slot].Weight : 0) + CurrentWearWeight > MaxWearWeight)
                    return false;

            if (RidingMount && item.Info.Type != ItemType.Torch)
            {
                return false;
            }

            return true;
        }
        private bool CanRemoveItem(MirGridType grid, UserItem item)
        {
            //Item  Stuck

            UserItem[] array;
            switch (grid)
            {
                case MirGridType.Inventory:
                    array = Info.Inventory;
                    break;
                case MirGridType.Storage:
                    array = Account.Storage;
                    break;
                default:
                    return false;
            }

            if (RidingMount && item.Info.Type != ItemType.Torch)
            {
                return false;
            }

            return FreeSpace(array) > 0;
        }

        public bool CheckQuestItem(UserItem uItem, uint count)
        {
            foreach (var item in Info.QuestInventory.Where(item => item != null && item.Info == uItem.Info))
            {
                if (count > item.Count)
                {
                    count -= item.Count;
                    continue;
                }

                if (count > item.Count) continue;
                count = 0;
                break;
            }

            return count <= 0;
        }
        public bool CanGainQuestItem(UserItem item)
        {
            if (FreeSpace(Info.QuestInventory) > 0) return true;

            if (item.Info.StackSize > 1)
            {
                uint count = item.Count;

                for (int i = 0; i < Info.QuestInventory.Length; i++)
                {
                    UserItem bagItem = Info.QuestInventory[i];

                    if (bagItem.Info != item.Info) continue;

                    if (bagItem.Count + count <= bagItem.Info.StackSize) return true;

                    count -= bagItem.Info.StackSize - bagItem.Count;
                }
            }

            ReceiveChat("You cannot carry anymore quest items.", ChatType.System);

            return false;
        }
        public void GainQuestItem(UserItem item)
        {
            CheckItem(item);

            UserItem clonedItem = item.Clone();

            Enqueue(new S.GainedQuestItem { Item = clonedItem });

            AddQuestItem(item);
        }
        public void TakeQuestItem(ItemInfo uItem, uint count)
        {
            for (int o = 0; o < Info.QuestInventory.Length; o++)
            {
                UserItem item = Info.QuestInventory[o];
                if (item == null) continue;
                if (item.Info != uItem) continue;

                if (count > item.Count)
                {
                    Enqueue(new S.DeleteQuestItem { UniqueID = item.UniqueID, Count = item.Count });
                    Info.QuestInventory[o] = null;

                    count -= item.Count;
                    continue;
                }

                Enqueue(new S.DeleteQuestItem { UniqueID = item.UniqueID, Count = count });

                if (count == item.Count)
                    Info.QuestInventory[o] = null;
                else
                    item.Count -= count;
                break;
            }
        }

        private void DamageDura()
        {
            if (!NoDuraLoss)
                for (int i = 0; i < Info.Equipment.Length; i++) DamageItem(Info.Equipment[i], Envir.Random.Next(1) + 1);
        }
        public void DamageWeapon()
        {
            if (!NoDuraLoss)
                DamageItem(Info.Equipment[(int)EquipmentSlot.Weapon], Envir.Random.Next(4) + 1);
        }
        private void DamageItem(UserItem item, int amount, bool isChanged = false)
        {
            if (item == null || item.CurrentDura == 0 || item.Info.Type == ItemType.Amulet) return;
            if (item.Info.Strong > 0) amount = Math.Max(1, amount - item.Info.Strong);
            item.CurrentDura = (ushort)Math.Max(ushort.MinValue, item.CurrentDura - amount);
            item.DuraChanged = true;

            if (item.CurrentDura > 0 && isChanged != true) return;
            Enqueue(new S.DuraChanged { UniqueID = item.UniqueID, CurrentDura = item.CurrentDura });
            item.DuraChanged = false;
            RefreshStats();
        }
        private void ConsumeItem(UserItem item, uint cost)
        {
            item.Count -= cost;
            Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = cost });


            if (item.Count != 0) return;

            for (int i = 0; i < Info.Equipment.Length; i++)
            {
                if (Info.Equipment[i] != null && Info.Equipment[i].Slots.Length > 0)
                {
                    for (int j = 0; j < Info.Equipment[i].Slots.Length; j++)
                    {
                        if (Info.Equipment[i].Slots[j] != item) continue;
                        Info.Equipment[i].Slots[j] = null;
                        return;
                    }
                }

                if (Info.Equipment[i] != item) continue;
                Info.Equipment[i] = null;

                return;
            }

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                if (Info.Inventory[i] != item) continue;
                Info.Inventory[i] = null;
                return;
            }
            //Item not found
        }

        public bool TryLuckWeapon()
        {
            UserItem item = Info.Equipment[(int)EquipmentSlot.Weapon];

            if (item == null || item.Luck >= 7) return false;

            if (item.Luck > -10 && Envir.Random.Next(20) == 0)
            {
                Luck--;
                item.Luck--;
                Enqueue(new S.RefreshItem { Item = item });
                ReceiveChat("Curse dwells within your weapon.", ChatType.System);
            }
            else if (item.Luck <= 0 || Envir.Random.Next(10 * item.Luck) == 0)
            {
                Luck++;
                item.Luck++;
                Enqueue(new S.RefreshItem { Item = item });
                ReceiveChat("Luck dwells within your weapon.", ChatType.Hint);
            }
            else
            {
                ReceiveChat("No effect.", ChatType.Hint);
            }

            return true;
        }

        public void CallDefaultNPC(DefaultNPCType type, params object[] value)
        {
            string key = string.Empty;

            switch (type)
            {
                case DefaultNPCType.Login:
                    key = "Login";
                    break;
                case DefaultNPCType.UseItem:
                    if (value.Length < 1) return;
                    key = string.Format("UseItem({0})", value[0]);
                    break;
                case DefaultNPCType.Trigger:
                    if (value.Length < 1) return;
                    key = string.Format("Trigger({0})", value[0]);
                    break;
                case DefaultNPCType.MapCoord:
                    if (value.Length < 3) return;
                    key = string.Format("MapCoord({0},{1},{2})", value[0], value[1], value[2]);
                    break;
                case DefaultNPCType.MapEnter:
                    if (value.Length < 1) return;
                    key = string.Format("MapEnter({0})", value[0]);
                    break;
                case DefaultNPCType.Die:
                    key = "Die";
                    break;
                case DefaultNPCType.LevelUp:
                    key = "LevelUp";
                    break;
                case DefaultNPCType.CustomCommand:
                    if (value.Length < 1) return;
                    key = string.Format("CustomCommand({0})", value[0]);
                    break;
                case DefaultNPCType.OnAcceptQuest:
                    if (value.Length < 1) return;
                    key = string.Format("OnAcceptQuest({0})", value[0]);
                    break;
                case DefaultNPCType.OnFinishQuest:
                    if (value.Length < 1) return;
                    key = string.Format("OnFinishQuest({0})", value[0]);
                    break;
                case DefaultNPCType.Daily:
                    key = "Daily";
                    Info.NewDay = false;
                    break;
            }

            key = string.Format("[@_{0}]", key);

            DelayedAction action = new DelayedAction(DelayedType.NPC, SMain.Envir.Time + 0, DefaultNPC.ObjectID, key);
            ActionList.Add(action);

            Enqueue(new S.NPCUpdate { NPCID = DefaultNPC.ObjectID });
        }
        public void CallNPC(uint objectID, string key)
        {
            if (objectID == DefaultNPC.ObjectID)
            {
                DefaultNPC.Call(this, key.ToUpper());
                return;
            }

            if (Dead) return;

            for (int i = 0; i < CurrentMap.NPCs.Count; i++)
            {
                NPCObject ob = CurrentMap.NPCs[i];
                if (ob.ObjectID != objectID) continue;
                
                ob.Call(this, key.ToUpper());
                break;
            }

            //process any new npc calls immediately
            for (int i = 0; i < ActionList.Count; i++)
            {
                if (ActionList[i].Type != DelayedType.NPC || ActionList[i].Time != -1) continue;
                var action = ActionList[i];

                ActionList.RemoveAt(i);
                CompleteNPC(action.Params);
            }
        }

        public void BuyItem(ulong index, uint count)
        {
            if (Dead) return;

            if (NPCPage == null || !(String.Equals(NPCPage.Key, NPCObject.BuySellKey, StringComparison.CurrentCultureIgnoreCase) || String.Equals(NPCPage.Key, NPCObject.BuyKey, StringComparison.CurrentCultureIgnoreCase) || String.Equals(NPCPage.Key, NPCObject.BuyBackKey, StringComparison.CurrentCultureIgnoreCase) || String.Equals(NPCPage.Key, NPCObject.PearlBuyKey, StringComparison.CurrentCultureIgnoreCase))) return;

            for (int i = 0; i < CurrentMap.NPCs.Count; i++)
            {
                NPCObject ob = CurrentMap.NPCs[i];
                if (ob.ObjectID != NPCID) continue;
                ob.Buy(this, index, count);
            }
        }
        public void SellItem(ulong uniqueID, uint count)
        {
            S.SellItem p = new S.SellItem { UniqueID = uniqueID, Count = count };
            if (Dead || count == 0)
            {
                Enqueue(p);
                return;
            }

            if (NPCPage == null || !(String.Equals(NPCPage.Key, NPCObject.BuySellKey, StringComparison.CurrentCultureIgnoreCase) || String.Equals(NPCPage.Key, NPCObject.SellKey, StringComparison.CurrentCultureIgnoreCase)))
            {
                Enqueue(p);
                return;
            }

            for (int n = 0; n < CurrentMap.NPCs.Count; n++)
            {
                NPCObject ob = CurrentMap.NPCs[n];
                if (ob.ObjectID != NPCID) continue;

                UserItem temp = null;
                int index = -1;

                for (int i = 0; i < Info.Inventory.Length; i++)
                {
                    temp = Info.Inventory[i];
                    if (temp == null || temp.UniqueID != uniqueID) continue;
                    index = i;
                    break;
                }

                if (temp == null || index == -1 || count > temp.Count)
                {
                    Enqueue(p);
                    return;
                }

                if (temp.Info.Bind.HasFlag(BindMode.DontSell))
                {
                    Enqueue(p);
                    return;
                }

                if (ob.Types.Count != 0 && !ob.Types.Contains(temp.Info.Type))
                {
                    ReceiveChat("You cannot sell this item here.", ChatType.System);
                    Enqueue(p);
                    return;
                }

                if (temp.Info.StackSize > 1 && count != temp.Count)
                {
                    UserItem item = Envir.CreateFreshItem(temp.Info);
                    item.Count = count;

                    if (item.Price() / 2 + Account.Gold > uint.MaxValue)
                    {
                        Enqueue(p);
                        return;
                    }

                    temp.Count -= count;
                    temp = item;
                }
                else Info.Inventory[index] = null;

                ob.Sell(this, temp);
                p.Success = true;
                Enqueue(p);
                GainGold(temp.Price() / 2);
                RefreshBagWeight();
                return;
            }



            Enqueue(p);
        }
        public void RepairItem(ulong uniqueID, bool special = false)
        {
            Enqueue(new S.RepairItem { UniqueID = uniqueID });

            if (Dead) return;

            if (NPCPage == null || (!String.Equals(NPCPage.Key, NPCObject.RepairKey, StringComparison.CurrentCultureIgnoreCase) && !special) || (!String.Equals(NPCPage.Key, NPCObject.SRepairKey, StringComparison.CurrentCultureIgnoreCase) && special)) return;

            for (int n = 0; n < CurrentMap.NPCs.Count; n++)
            {
                NPCObject ob = CurrentMap.NPCs[n];
                if (ob.ObjectID != NPCID) continue;

                UserItem temp = null;
                int index = -1;

                for (int i = 0; i < Info.Inventory.Length; i++)
                {
                    temp = Info.Inventory[i];
                    if (temp == null || temp.UniqueID != uniqueID) continue;
                    index = i;
                    break;
                }

                if (temp == null || index == -1) return;

                if ((temp.Info.Bind.HasFlag(BindMode.DontRepair)) || (temp.Info.BindNoSRepair && special))
                {
                    ReceiveChat("You cannot Repair this item.", ChatType.System);
                    return;
                }

                if (ob.Types.Count != 0 && !ob.Types.Contains(temp.Info.Type))
                {
                    ReceiveChat("You cannot Repair this item here.", ChatType.System);
                    return;
                }

                uint cost = (uint)(temp.RepairPrice() * ob.Info.PriceRate);

                if (cost > Account.Gold || cost == 0) return;

                Account.Gold -= cost;
                Enqueue(new S.LoseGold { Gold = cost });

                if (!special) temp.MaxDura = (ushort)Math.Max(0, temp.MaxDura - (temp.MaxDura - temp.CurrentDura) / 30);

                temp.CurrentDura = temp.MaxDura;
                temp.DuraChanged = false;

                Enqueue(new S.ItemRepaired { UniqueID = uniqueID, MaxDura = temp.MaxDura, CurrentDura = temp.CurrentDura });
                return;
            }
        }
        public void SendStorage()
        {
            if (Connection.StorageSent) return;
            Connection.StorageSent = true;

            for (int i = 0; i < Account.Storage.Length; i++)
            {
                UserItem item = Account.Storage[i];
                if (item == null) continue;
                //CheckItemInfo(item.Info);
                CheckItem(item);
            }

            Enqueue(new S.UserStorage { Storage = Account.Storage }); // Should be no alter before being sent.
        }


        public void ConsignItem(ulong uniqueID, uint price)
        {
            S.ConsignItem p = new S.ConsignItem { UniqueID = uniqueID };
            if (price < Globals.MinConsignment || price > Globals.MaxConsignment || Dead)
            {
                Enqueue(p);
                return;
            }

            if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.ConsignKey, StringComparison.CurrentCultureIgnoreCase))
            {
                Enqueue(p);
                return;
            }

            if (Account.Gold < Globals.ConsignmentCost)
            {
                Enqueue(p);
                return;
            }

            for (int n = 0; n < CurrentMap.NPCs.Count; n++)
            {
                NPCObject ob = CurrentMap.NPCs[n];
                if (ob.ObjectID != NPCID) continue;

                UserItem temp = null;
                int index = -1;

                for (int i = 0; i < Info.Inventory.Length; i++)
                {
                    temp = Info.Inventory[i];
                    if (temp == null || temp.UniqueID != uniqueID) continue;
                    index = i;
                    break;
                }

                if (temp == null || index == -1)
                {
                    Enqueue(p);
                    return;
                }

                if (temp.Info.Bind.HasFlag(BindMode.DontSell))
                {
                    Enqueue(p);
                    return;
                }

                //Check Max Consignment.

                AuctionInfo auction = new AuctionInfo
                {
                    AuctionID = ++Envir.NextAuctionID,
                    CharacterIndex = Info.Index,
                    CharacterInfo = Info,
                    ConsignmentDate = Envir.Now,
                    Item = temp,
                    Price = price
                };

                Account.Auctions.AddLast(auction);
                Envir.Auctions.AddFirst(auction);

                p.Success = true;
                Enqueue(p);

                Info.Inventory[index] = null;
                Account.Gold -= Globals.ConsignmentCost;
                Enqueue(new S.LoseGold { Gold = Globals.ConsignmentCost });
                RefreshBagWeight();

            }

            Enqueue(p);
        }
        public bool Match(AuctionInfo info)
        {
            if (Envir.Now >= info.ConsignmentDate.AddDays(Globals.ConsignmentLength) && !info.Sold)
                info.Expired = true;

            return (UserMatch || !info.Expired && !info.Sold) && ((MatchType == ItemType.Nothing || info.Item.Info.Type == MatchType) &&
                (string.IsNullOrWhiteSpace(MatchName) || info.Item.Info.Name.Replace(" ", "").IndexOf(MatchName, StringComparison.OrdinalIgnoreCase) >= 0));
        }
        public void MarketPage(int page)
        {
            if (Dead || Envir.Time < SearchTime) return;

            if (NPCPage == null || (!String.Equals(NPCPage.Key, NPCObject.MarketKey, StringComparison.CurrentCultureIgnoreCase) && !String.Equals(NPCPage.Key, NPCObject.ConsignmentsKey, StringComparison.CurrentCultureIgnoreCase)) || page <= PageSent) return;

            SearchTime = Envir.Time + Globals.SearchDelay;

            for (int n = 0; n < CurrentMap.NPCs.Count; n++)
            {
                NPCObject ob = CurrentMap.NPCs[n];
                if (ob.ObjectID != NPCID) continue;

                List<ClientAuction> listings = new List<ClientAuction>();

                for (int i = 0; i < 10; i++)
                {
                    if (i + page * 10 >= Search.Count) break;
                    listings.Add(Search[i + page * 10].CreateClientAuction(UserMatch));
                }

                for (int i = 0; i < listings.Count; i++)
                {
                    //CheckItemInfo(listings[i].Item.Info);
                    CheckItem(listings[i].Item);
                }

                PageSent = page;
                Enqueue(new S.NPCMarketPage { Listings = listings });
            }
        }
        public void GetMarket(string name, ItemType type)
        {
            Search.Clear();
            MatchName = name.Replace(" ", "");
            MatchType = type;
            PageSent = 0;

            long start = Envir.Stopwatch.ElapsedMilliseconds;

            LinkedListNode<AuctionInfo> current = UserMatch ? Account.Auctions.First : Envir.Auctions.First;

            while (current != null)
            {
                if (Match(current.Value)) Search.Add(current.Value);
                current = current.Next;
            }

            List<ClientAuction> listings = new List<ClientAuction>();

            for (int i = 0; i < 10; i++)
            {
                if (i >= Search.Count) break;
                listings.Add(Search[i].CreateClientAuction(UserMatch));
            }

            for (int i = 0; i < listings.Count; i++)
            {
                //CheckItemInfo(listings[i].Item.Info);
                CheckItem(listings[i].Item);
            }

            Enqueue(new S.NPCMarket { Listings = listings, Pages = (Search.Count - 1) / 10 + 1, UserMode = UserMatch });

            SMain.EnqueueDebugging(string.Format("{0}ms to match {1} items", Envir.Stopwatch.ElapsedMilliseconds - start, UserMatch ? Account.Auctions.Count : Envir.Auctions.Count));
        }
        public void MarketSearch(string match)
        {
            if (Dead || Envir.Time < SearchTime) return;

            if (NPCPage == null || (!String.Equals(NPCPage.Key, NPCObject.MarketKey, StringComparison.CurrentCultureIgnoreCase) && !String.Equals(NPCPage.Key, NPCObject.ConsignmentsKey, StringComparison.CurrentCultureIgnoreCase))) return;

            SearchTime = Envir.Time + Globals.SearchDelay;

            for (int n = 0; n < CurrentMap.NPCs.Count; n++)
            {
                NPCObject ob = CurrentMap.NPCs[n];
                if (ob.ObjectID != NPCID) continue;

                GetMarket(match, ItemType.Nothing);
            }
        }
        public void MarketRefresh()
        {
            if (Dead || Envir.Time < SearchTime) return;

            if (NPCPage == null || (!String.Equals(NPCPage.Key, NPCObject.MarketKey, StringComparison.CurrentCultureIgnoreCase) && !String.Equals(NPCPage.Key, NPCObject.ConsignmentsKey, StringComparison.CurrentCultureIgnoreCase))) return;

            SearchTime = Envir.Time + Globals.SearchDelay;

            for (int n = 0; n < CurrentMap.NPCs.Count; n++)
            {
                NPCObject ob = CurrentMap.NPCs[n];
                if (ob.ObjectID != NPCID) continue;

                GetMarket(string.Empty, MatchType);
            }
        }
        public void MarketBuy(ulong auctionID)
        {
            if (Dead)
            {
                Enqueue(new S.MarketFail { Reason = 0 });
                return;

            }

            if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.MarketKey, StringComparison.CurrentCultureIgnoreCase))
            {
                Enqueue(new S.MarketFail { Reason = 1 });
                return;
            }

            for (int n = 0; n < CurrentMap.NPCs.Count; n++)
            {
                NPCObject ob = CurrentMap.NPCs[n];
                if (ob.ObjectID != NPCID) continue;

                foreach (AuctionInfo auction in Search)
                {
                    if (auction.AuctionID != auctionID) continue;


                    if (auction.Sold)
                    {
                        Enqueue(new S.MarketFail { Reason = 2 });
                        return;
                    }

                    if (auction.Expired)
                    {
                        Enqueue(new S.MarketFail { Reason = 3 });
                        return;
                    }

                    if (auction.Price > Account.Gold)
                    {
                        Enqueue(new S.MarketFail { Reason = 4 });
                        return;
                    }

                    if (!CanGainItem(auction.Item))
                    {
                        Enqueue(new S.MarketFail { Reason = 5 });
                        return;
                    }

                    if (Account.Auctions.Contains(auction))
                    {
                        Enqueue(new S.MarketFail { Reason = 6 });
                        return;
                    }

                    auction.Sold = true;
                    Account.Gold -= auction.Price;
                    Enqueue(new S.LoseGold { Gold = auction.Price });
                    GainItem(auction.Item);
                    Envir.MessageAccount(auction.CharacterInfo.AccountInfo, string.Format("You Sold {0} for {1:#,##0} Gold", auction.Item.Name, auction.Price), ChatType.Hint);
                    Enqueue(new S.MarketSuccess { Message = string.Format("You brought {0} for {1:#,##0} Gold", auction.Item.Name, auction.Price) });
                    MarketSearch(MatchName);
                    return;
                }
            }

            Enqueue(new S.MarketFail { Reason = 7 });
        }
        public void MarketGetBack(ulong auctionID)
        {
            if (Dead)
            {
                Enqueue(new S.MarketFail { Reason = 0 });
                return;

            }

            if (NPCPage == null || !String.Equals(NPCPage.Key, NPCObject.ConsignmentsKey, StringComparison.CurrentCultureIgnoreCase))
            {
                Enqueue(new S.MarketFail { Reason = 1 });
                return;
            }

            for (int n = 0; n < CurrentMap.NPCs.Count; n++)
            {
                NPCObject ob = CurrentMap.NPCs[n];
                if (ob.ObjectID != NPCID) continue;

                foreach (AuctionInfo auction in Account.Auctions)
                {
                    if (auction.AuctionID != auctionID) continue;

                    if (auction.Sold && auction.Expired)
                    {
                        SMain.Enqueue(string.Format("Auction both sold and Expired {0}", Account.AccountID));
                        return;
                    }


                    if (!auction.Sold || auction.Expired)
                    {
                        if (!CanGainItem(auction.Item))
                        {
                            Enqueue(new S.MarketFail { Reason = 5 });
                            return;
                        }

                        Account.Auctions.Remove(auction);
                        Envir.Auctions.Remove(auction);
                        GainItem(auction.Item);
                        MarketSearch(MatchName);
                        return;
                    }

                    uint gold = (uint)Math.Max(0, auction.Price - auction.Price * Globals.Commission);
                    if (!CanGainGold(gold))
                    {
                        Enqueue(new S.MarketFail { Reason = 8 });
                        return;
                    }

                    Account.Auctions.Remove(auction);
                    Envir.Auctions.Remove(auction);
                    GainGold(gold);
                    Enqueue(new S.MarketSuccess { Message = string.Format("You Sold {0} for {1:#,##0} Gold. \nEarnings: {2:#,##0} Gold.\nCommision: {3:#,##0} Gold.‎", auction.Item.Name, auction.Price, gold, auction.Price - gold) });
                    MarketSearch(MatchName);
                    return;
                }

            }

            Enqueue(new S.MarketFail { Reason = 7 });
        }
        public void RequestUserName(uint id)
        {
            CharacterInfo Character = Envir.GetCharacterInfo((int)id);
            if (Character != null)
                Enqueue(new S.UserName { Id = (uint)Character.Index, Name = Character.Name });

            /*
            for (int i = 0; i < Envir.Players.Count; i++)
            {
                if (Envir.Players[i].Info.Index == id)
                {
                    Enqueue(new S.UserName { Id = (uint)Envir.Players[i].Info.Index, Name = Envir.Players[i].Name });
                    return;
                }
            }
            */
        }

        public void RequestChatItem(ulong id)
        {
            //Enqueue(new S.ChatItemStats { ChatItemId = id, Stats = whatever });
        }
        public void Inspect(uint id)
        {
            if (ObjectID == id) return;


            PlayerObject player = CurrentMap.Players.SingleOrDefault(x => x.ObjectID == id || x.Pets.Count(y => y.ObjectID == id && y is Monsters.HumanWizard) > 0);

            if (player == null) return;

            for (int i = 0; i < player.Info.Equipment.Length; i++)
            {
                UserItem u = player.Info.Equipment[i];
                if (u == null) continue;

                //CheckItemInfo(u.Info);
                CheckItem(u);
            }
            string guildname = "";
            string guildrank = "";
            if (player.MyGuild != null)
            {
                guildname = player.MyGuild.Name;
                guildrank = player.MyGuildRank.Name;
            }
            Enqueue(new S.PlayerInspect
            {
                Name = player.Name,
                Equipment = player.Info.Equipment,
                GuildName = guildname,
                GuildRank = guildrank,
                Hair = player.Hair,
                Gender = player.Gender,
                Class = player.Class,
                Level = player.Level
            });
        }
        public void RemoveObjects(MirDirection dir, int count)
        {
            switch (dir)
            {
                case MirDirection.Up:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.UpRight:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }

                    //Left Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange - count; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Right:
                    //Left Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.DownRight:
                    //Top Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }

                    //Left Block
                    for (int a = -Globals.DataRange + count; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Down:
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.DownLeft:
                    //Top Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }

                    //Right Block
                    for (int a = -Globals.DataRange + count; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Left:
                    for (int a = -Globals.DataRange; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
                case MirDirection.UpLeft:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }

                    //Right Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange - count; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Remove(this);
                            }
                        }
                    }
                    break;
            }
        }
        public void AddObjects(MirDirection dir, int count)
        {
            switch (dir)
            {
                case MirDirection.Up:
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.UpRight:
                    //Top Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }

                    //Right Block
                    for (int a = -Globals.DataRange + count; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Right:
                    for (int a = -Globals.DataRange; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.DownRight:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }

                    //Right Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange - count; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X + Globals.DataRange - b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Down:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.DownLeft:
                    //Bottom Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y + Globals.DataRange - a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }

                    //Left Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange - count; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.Left:
                    //Left Block
                    for (int a = -Globals.DataRange; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }
                    break;
                case MirDirection.UpLeft:
                    //Top Block
                    for (int a = 0; a < count; a++)
                    {
                        int y = CurrentLocation.Y - Globals.DataRange + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = -Globals.DataRange; b <= Globals.DataRange; b++)
                        {
                            int x = CurrentLocation.X + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }

                    //Left Block
                    for (int a = -Globals.DataRange + count; a <= Globals.DataRange; a++)
                    {
                        int y = CurrentLocation.Y + a;
                        if (y < 0 || y >= CurrentMap.Height) continue;

                        for (int b = 0; b < count; b++)
                        {
                            int x = CurrentLocation.X - Globals.DataRange + b;
                            if (x < 0 || x >= CurrentMap.Width) continue;

                            Cell cell = CurrentMap.GetCell(x, y);

                            if (!cell.Valid || cell.Objects == null) continue;

                            for (int i = 0; i < cell.Objects.Count; i++)
                            {
                                MapObject ob = cell.Objects[i];
                                ob.Add(this);
                            }
                        }
                    }
                    break;
            }
        }
        public override void Remove(PlayerObject player)
        {
            if (player == this) return;

            base.Remove(player);
            Enqueue(new S.ObjectRemove { ObjectID = player.ObjectID });
        }
        public override void Add(PlayerObject player)
        {
            if (player == this) return;

            base.Add(player);
            Enqueue(player.GetInfoEx(this));

            player.SendHealth(this);
            SendHealth(player);
        }
        public override void Remove(MonsterObject monster)
        {
            Enqueue(new S.ObjectRemove { ObjectID = monster.ObjectID });
        }
        public override void Add(MonsterObject monster)
        {
            Enqueue(monster.GetInfo());

            monster.SendHealth(this);
        }
        public override void SendHealth(PlayerObject player)
        {
            if (!player.IsMember(this) && Envir.Time > RevTime) return;
            byte time = Math.Min(byte.MaxValue, (byte)Math.Max(5, (RevTime - Envir.Time) / 1000));
            player.Enqueue(new S.ObjectHealth { ObjectID = ObjectID, Percent = PercentHealth, Expire = time });
        }

        public override void ReceiveChat(string text, ChatType type)
        {
            Enqueue(new S.Chat { Message = text, Type = type });
        }

        private void CleanUp()
        {
            Connection.Player = null;
            Info.Player = null;
            Connection = null;
            Account = null;
            Info = null;
        }

        public void Enqueue(Packet p)
        {
            if (Connection == null) return;
            Connection.Enqueue(p);
        }

        public void SpellToggle(Spell spell, bool use)
        {
            UserMagic magic;
            int cost;
            switch (spell)
            {
                case Spell.Thrusting:
                    Info.Thrusting = use;
                    break;
                case Spell.HalfMoon:
                    Info.HalfMoon = use;
                    break;
                case Spell.CrossHalfMoon:
                    Info.CrossHalfMoon = use;
                    break;
                case Spell.DoubleSlash:
                    Info.DoubleSlash = use;
                    break;
                case Spell.TwinDrakeBlade:
                    if (TwinDrakeBlade) return;
                    magic = GetMagic(spell);
                    if (magic == null) return;
                    cost = magic.Info.BaseCost + magic.Level * magic.Info.LevelCost;
                    if (cost >= MP) return;

                    TwinDrakeBlade = true;
                    ChangeMP(-cost);
                    break;
                case Spell.FlamingSword:
                    if (FlamingSword || Envir.Time < FlamingSwordTime) return;
                    magic = GetMagic(spell);
                    if (magic == null) return;
                    cost = magic.Info.BaseCost + magic.Level * magic.Info.LevelCost;
                    if (cost >= MP) return;

                    FlamingSword = true;
                    FlamingSwordTime = Envir.Time + 10000;
                    Enqueue(new S.SpellToggle { Spell = Spell.FlamingSword, CanUse = true });
                    ChangeMP(-cost);
                    break;
                case Spell.CounterAttack:
                    if (bCounterAttack || Envir.Time < CounterAttackTime) return;
                    magic = GetMagic(spell);
                    if (magic == null) return;
                    cost = magic.Info.BaseCost + magic.Level * magic.Info.LevelCost;
                    if (cost >= MP) return;

                    bCounterAttack = true;
                    CounterAttackTime = Envir.Time + 7000;
                    AddBuff(new Buff { Type = BuffType.CounterAttack, Caster = this, ExpireTime = CounterAttackTime, Value = 11 + magic.Level * 3, Visible = true });
                    ChangeMP(-cost);
                    break;
                case Spell.MentalState:
                    Info.MentalState = (byte)((Info.MentalState + 1) % 3);
                    for (int i = 0; i < Buffs.Count; i++)
                    {
                        if (Buffs[i].Type == BuffType.MentalState)
                        {
                            Buffs[i].Value = Info.MentalState;
                            S.AddBuff addBuff = new S.AddBuff { Type = Buffs[i].Type, Caster = Buffs[i].Caster.Name, Expire = Buffs[i].ExpireTime - Envir.Time, Value = Buffs[i].Value, Infinite = Buffs[i].Infinite, ObjectID = ObjectID, Visible = Buffs[i].Visible };
                            Enqueue(addBuff);
                            break;
                        }
                    }
                    break;
            }
        }

		#region Awakening
        public void Awakening(ulong UniqueID, AwakeType type)
        {
            if (type == AwakeType.None) return;

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                UserItem item = Info.Inventory[i];
                if (item == null || item.UniqueID != UniqueID) continue;

                Awake awake = item.Awake;

                if (awake.IsMaxLevel())
                {
                    Enqueue(new S.Awakening { result = -2, removeID = -1 });
                    return;
                }

                if (Info.AccountInfo.Gold < item.AwakeningPrice())
                {
                    Enqueue(new S.Awakening { result = -3, removeID = -1 });
                    return;
                }

                if (HasAwakeningNeedMaterials(item, type))
                {
                    Info.AccountInfo.Gold -= item.AwakeningPrice();
                    Enqueue(new S.LoseGold { Gold = item.AwakeningPrice() });

                    bool[] isHit;

                    switch (awake.UpgradeAwake(item, type, out isHit))
                    {
                        case -1:
                            Enqueue(new S.Awakening { result = -1, removeID = -1 });
                            break;
                        case 0:
                            AwakeningEffect(false, isHit);
                            Info.Inventory[i] = null;
                            Enqueue(new S.Awakening { result = 0, removeID = (long)item.UniqueID });
                            break;
                        case 1:
                            Enqueue(new S.RefreshItem { Item = item });
                            AwakeningEffect(true, isHit);
                            Enqueue(new S.Awakening { result = 1, removeID = -1 });
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void DowngradeAwakening(ulong UniqueID)
        {
            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                UserItem item = Info.Inventory[i];
                if (item != null)
                {
                    if (item.UniqueID == UniqueID)
                    {
                        if (Info.AccountInfo.Gold >= item.DowngradePrice())
                        {
                            Info.AccountInfo.Gold -= item.DowngradePrice();
                            Enqueue(new S.LoseGold { Gold = item.DowngradePrice() });

                            Awake awake = item.Awake;
                            int result = awake.RemoveAwake();
                            switch (result)
                            {
                                case 0:
                                    ReceiveChat(string.Format("{0} : Remove failed Level 0", item.Name), ChatType.System);
                                    break;
                                case 1:
                                    ushort maxDura = (Envir.Random.Next(20) == 0) ? (ushort)(item.MaxDura - 1000) : item.MaxDura;
                                    if (maxDura < 1000) maxDura = 1000;

                                    Info.Inventory[i].CurrentDura = (Info.Inventory[i].CurrentDura >= maxDura) ? maxDura : Info.Inventory[i].CurrentDura;
                                    Info.Inventory[i].MaxDura = maxDura;
                                    ReceiveChat(string.Format("{0} : Remove success. Level {1}", item.Name, item.Awake.getAwakeLevel()), ChatType.System);
                                    Enqueue(new S.RefreshItem { Item = item });
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void DisassembleItem(ulong UniqueID)
        {
            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                UserItem item = Info.Inventory[i];
                if (item == null || item.UniqueID != UniqueID) continue;

                if (Info.AccountInfo.Gold >= item.DisassemblePrice())
                {
                    List<ItemInfo> dropList = new List<ItemInfo>();
                    foreach (DropInfo drop in Envir.AwakeningDrops)
                    {
                        if (drop.Item.Grade == item.Info.Grade - 1 ||
                            drop.Item.Grade == item.Info.Grade + 1)
                        {
                            if (Envir.Random.Next((drop.Chance <= 0) ? 1 : drop.Chance) == 0)
                            {
                                dropList.Add(drop.Item);
                            }
                        }

                        if (drop.Item.Grade == item.Info.Grade)
                        {
                            dropList.Add(drop.Item);
                        }
                    }

                    if (dropList.Count == 0) continue;

                    UserItem gainItem = Envir.CreateDropItem(dropList[Envir.Random.Next(dropList.Count)]);
                    if (gainItem == null) continue;
                    gainItem.Count = (uint)Envir.Random.Next((int)((((int)item.Info.Grade * item.Info.RequiredAmount) / 10) + item.Quality()));
                    if (gainItem.Count < 1) gainItem.Count = 1;

                    GainItem(gainItem);

                    Enqueue(new S.LoseGold { Gold = item.DisassemblePrice() });
                    Info.AccountInfo.Gold -= item.DisassemblePrice();

                    Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = item.Count });
                    Info.Inventory[i] = null;
                }
            }
        }

        public void ResetAddedItem(ulong UniqueID)
        {
            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                UserItem item = Info.Inventory[i];
                if (item != null)
                {
                    if (item.UniqueID == UniqueID)
                    {
                        if (Info.AccountInfo.Gold >= item.ResetPrice())
                        {
                            Info.AccountInfo.Gold -= item.ResetPrice();
                            Enqueue(new S.LoseGold { Gold = item.ResetPrice() });

                            UserItem newItem = new UserItem(item.Info);

                            ushort maxDura = (Envir.Random.Next(20) == 0) ? (ushort)(item.MaxDura - 1000) : item.MaxDura;
                            if (maxDura < 1000) maxDura = 1000;

                            newItem.UniqueID = item.UniqueID;
                            newItem.ItemIndex = item.ItemIndex;
                            newItem.CurrentDura = (item.CurrentDura >= maxDura) ? maxDura : item.CurrentDura;
                            newItem.MaxDura = maxDura;
                            newItem.Count = item.Count;
                            newItem.Slots = item.Slots;
                            newItem.Awake = item.Awake;

                            Info.Inventory[i] = newItem;

                            Enqueue(new S.RefreshItem { Item = Info.Inventory[i] });
                        }
                    }
                }
            }
        }

        public void AwakeningNeedMaterials(ulong UniqueID, AwakeType type)
        {
            if (type == AwakeType.None) return;

            foreach (UserItem item in Info.Inventory)
            {
                if (item != null)
                {
                    if (item.UniqueID == UniqueID)
                    {
                        Awake awake = item.Awake;

                        byte[] materialCount = new byte[2];
                        int idx = 0;
                        foreach (List<byte> material in Awake.AwakeMaterials[(int)type - 1])
                        {
                            byte materialRate = (byte)(Awake.AwakeMaterialRate[(int)item.Info.Grade - 1] * (float)awake.getAwakeLevel());
                            materialCount[idx] = material[(int)item.Info.Grade - 1];
                            materialCount[idx] += materialRate;
                            idx++;
                        }

                        ItemInfo[] materials = new ItemInfo[2];

                        foreach (ItemInfo info in Envir.ItemInfoList)
                        {
                            if (item.Info.Grade == info.Grade &&
                                info.Type == ItemType.Awakening)
                            {
                                if (info.Shape == (short)type - 1)
                                {
                                    materials[0] = info;
                                }
                                else if (info.Shape == 100)
                                {
                                    materials[1] = info;
                                }
                            }
                        }
                       
                        Enqueue(new S.AwakeningNeedMaterials { Materials = materials, MaterialsCount = materialCount });
                        break;
                    }
                }
            }
        }

        public void AwakeningEffect(bool isSuccess, bool[] isHit)
        {
            for (int i = 0; i < 5; i++)
            {
                Enqueue(new S.ObjectEffect { ObjectID = ObjectID, Effect = isHit[i] ? SpellEffect.AwakeningHit :SpellEffect.AwakeningMiss, EffectType = 0, DelayTime = (uint)(i * 500) });
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = isHit[i] ? SpellEffect.AwakeningHit :SpellEffect.AwakeningMiss, EffectType = 0, DelayTime = (uint)(i * 500) });
            }

            Enqueue(new S.ObjectEffect { ObjectID = ObjectID, Effect = isSuccess ? SpellEffect.AwakeningSuccess : SpellEffect.AwakeningFail, EffectType = 0, DelayTime = 2500 });
            Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Effect = isSuccess ? SpellEffect.AwakeningSuccess : SpellEffect.AwakeningFail, EffectType = 0, DelayTime = 2500 });
        }

        public bool HasAwakeningNeedMaterials(UserItem item, AwakeType type)
        {
            Awake awake = item.Awake;

            byte[] materialCount = new byte[2];

            int idx = 0;
            foreach (List<byte> material in Awake.AwakeMaterials[(int)type - 1])
            {
                byte materialRate = (byte)(Awake.AwakeMaterialRate[(int)item.Info.Grade - 1] * (float)awake.getAwakeLevel());
                materialCount[idx] = material[(int)item.Info.Grade - 1];
                materialCount[idx] += materialRate;
                idx++;
            }

            byte[] currentCount = new byte[2]{ 0, 0 };

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                UserItem materialItem = Info.Inventory[i];
                if (materialItem != null)
                {
                    if (materialItem.Info.Grade == item.Info.Grade &&
                        materialItem.Info.Type == ItemType.Awakening)
                    {
                        if (materialItem.Info.Shape == ((int)type - 1) &&
                            materialCount[0] - currentCount[0] != 0)
                        {
                            if (materialItem.Count <= materialCount[0] - currentCount[0])
                            {
                                currentCount[0] += (byte)materialItem.Count;
                            }
                            else if (materialItem.Count > materialCount[0]- currentCount[0])
                            {
                                currentCount[0] = (byte)(materialCount[0] - currentCount[0]);
                            }
                        }
                        else if (materialItem.Info.Shape == 100 &&
                            materialCount[1] - currentCount[1] != 0)
                        {
                            if (materialItem.Count <= materialCount[1] - currentCount[1])
                            {
                                currentCount[1] += (byte)materialItem.Count;
                            }
                            else if (materialItem.Count > materialCount[1] - currentCount[1])
                            {
                                currentCount[1] = (byte)(materialCount[1] - currentCount[1]);
                            }
                        } 
                    }
                }
            }

            for (int i = 0; i < materialCount.Length; i++)
            {
                if (materialCount[i] != currentCount[i])
                {
                    Enqueue(new S.Awakening { result = -4, removeID = -1 });
                    return false;
                }
            }

            for (int i = 0; i < Info.Inventory.Length; i++)
            {
                if (Info.Inventory[i] != null)
                {
                    if (Info.Inventory[i].Info.Grade == item.Info.Grade &&
                        Info.Inventory[i].Info.Type == ItemType.Awakening)
                    {
                        if (Info.Inventory[i].Info.Shape == ((int)type - 1) &&
                            currentCount[0] > 0)
                        {
                            if (Info.Inventory[i].Count <= currentCount[0])
                            {
                                Enqueue(new S.DeleteItem { UniqueID = Info.Inventory[i].UniqueID, Count = Info.Inventory[i].Count });
                                currentCount[0] -= (byte)Info.Inventory[i].Count;
                                Info.Inventory[i] = null;
                            }
                            else if (Info.Inventory[i].Count > currentCount[0])
                            {
                                Enqueue(new S.DeleteItem { UniqueID = Info.Inventory[i].UniqueID, Count = (uint)currentCount[0] });
                                Info.Inventory[i].Count -= currentCount[0];
                                currentCount[0] = 0;
                            }
                        }
                        else if (Info.Inventory[i].Info.Shape == 100 &&
                            currentCount[1] > 0)
                        {
                            if (Info.Inventory[i].Count <= currentCount[1])
                            {
                                Enqueue(new S.DeleteItem { UniqueID = Info.Inventory[i].UniqueID, Count = Info.Inventory[i].Count });
                                currentCount[1] -= (byte)Info.Inventory[i].Count;
                                Info.Inventory[i] = null;
                            }
                            else if (Info.Inventory[i].Count > currentCount[1])
                            {
                                Enqueue(new S.DeleteItem { UniqueID = Info.Inventory[i].UniqueID, Count = (uint)currentCount[1] });
                                Info.Inventory[i].Count -= currentCount[1];
                                currentCount[1] = 0;
                            }
                        } 
                    }
                }
            }
            return true;
        }
        #endregion

        #region Groups

        public void SwitchGroup(bool allow)
        {
            Enqueue(new S.SwitchGroup { AllowGroup = allow });

            if (AllowGroup == allow) return;
            AllowGroup = allow;

            if (AllowGroup || GroupMembers == null) return;
            GroupMembers.Remove(this);
            Enqueue(new S.DeleteGroup());

            if (GroupMembers.Count > 1)
            {
                Packet p = new S.DeleteMember { Name = Name };

                for (int i = 0; i < GroupMembers.Count; i++)
                    GroupMembers[i].Enqueue(p);
            }
            else
            {
                GroupMembers[0].Enqueue(new S.DeleteGroup());
                GroupMembers[0].GroupMembers = null;
            }
            GroupMembers = null;
        }
        public void AddMember(string name)
        {
            if (GroupMembers != null && GroupMembers[0] != this)
            {
                ReceiveChat("You are not the group leader.", ChatType.System);
                return;
            }

            if (GroupMembers != null && GroupMembers.Count >= Globals.MaxGroup)
            {
                ReceiveChat("Your group already has the maximum number of members.", ChatType.System);
                return;
            }

            PlayerObject player = Envir.GetPlayer(name);

            if (player == null)
            {
                ReceiveChat(name + " could not be found.", ChatType.System);
                return;
            }
            if (player == this)
            {
                ReceiveChat("You cannot group yourself.", ChatType.System);
                return;
            }

            if (!player.AllowGroup)
            {
                ReceiveChat(name + " is not allowing group.", ChatType.System);
                return;
            }

            if (player.GroupMembers != null)
            {
                ReceiveChat(name + " is already in another group.", ChatType.System);
                return;
            }

            if (player.GroupInvitation != null)
            {
                ReceiveChat(name + " is already receiving an invite from another player.", ChatType.System);
                return;
            }

            SwitchGroup(true);
            player.Enqueue(new S.GroupInvite { Name = Name });
            player.GroupInvitation = this;

        }
        public void DelMember(string name)
        {
            if (GroupMembers == null)
            {
                ReceiveChat("You are not in a group.", ChatType.System);
                return;
            }
            if (GroupMembers[0] != this)
            {
                ReceiveChat("You are not the group leader.", ChatType.System);
                return;
            }

            PlayerObject player = null;

            for (int i = 0; i < GroupMembers.Count; i++)
            {
                if (String.Compare(GroupMembers[i].Name, name, StringComparison.OrdinalIgnoreCase) != 0) continue;
                player = GroupMembers[i];
                break;
            }


            if (player == null)
            {
                ReceiveChat(name + " is not in your group.", ChatType.System);
                return;
            }

            GroupMembers.Remove(player);
            player.Enqueue(new S.DeleteGroup());

            if (GroupMembers.Count > 1)
            {
                Packet p = new S.DeleteMember { Name = player.Name };

                for (int i = 0; i < GroupMembers.Count; i++)
                    GroupMembers[i].Enqueue(p);
            }
            else
            {
                GroupMembers[0].Enqueue(new S.DeleteGroup());
                GroupMembers[0].GroupMembers = null;
            }
            player.GroupMembers = null;
        }
        public void GroupInvite(bool accept)
        {
            if (GroupInvitation == null)
            {
                ReceiveChat("You have not been invited to a group.", ChatType.System);
                return;
            }

            if (!accept)
            {
                GroupInvitation.ReceiveChat(Name + " has declined your group invite.", ChatType.System);
                GroupInvitation = null;
                return;
            }


            if (GroupInvitation.GroupMembers != null && GroupInvitation.GroupMembers[0] != GroupInvitation)
            {
                ReceiveChat(GroupInvitation.Name + " is no longer the group leader.", ChatType.System);
                GroupInvitation = null;
                return;
            }

            if (GroupInvitation.GroupMembers != null && GroupInvitation.GroupMembers.Count >= Globals.MaxGroup)
            {
                ReceiveChat(GroupInvitation.Name + "'s group already has the maximum number of members.", ChatType.System);
                GroupInvitation = null;
                return;
            }
            if (!GroupInvitation.AllowGroup)
            {
                ReceiveChat(GroupInvitation.Name + " is not on allow group.", ChatType.System);
                GroupInvitation = null;
                return;
            }
            if (GroupInvitation.Node == null)
            {
                ReceiveChat(GroupInvitation.Name + " no longer online.", ChatType.System);
                GroupInvitation = null;
                return;
            }

            if (GroupInvitation.GroupMembers == null)
            {
                GroupInvitation.GroupMembers = new List<PlayerObject> { GroupInvitation };
                GroupInvitation.Enqueue(new S.AddMember { Name = GroupInvitation.Name });
            }

            Packet p = new S.AddMember { Name = Name };
            GroupMembers = GroupInvitation.GroupMembers;
            GroupInvitation = null;


            for (int i = 0; i < GroupMembers.Count; i++)
            {
                PlayerObject member = GroupMembers[i];
                member.Enqueue(p);
                Enqueue(new S.AddMember { Name = member.Name });

                if (CurrentMap != member.CurrentMap || !Functions.InRange(CurrentLocation, member.CurrentLocation, Globals.DataRange)) continue;

                byte time = Math.Min(byte.MaxValue, (byte)Math.Max(5, (RevTime - Envir.Time) / 1000));
                member.Enqueue(new S.ObjectHealth { ObjectID = ObjectID, Percent = member.PercentHealth, Expire = time });
                Enqueue(new S.ObjectHealth { ObjectID = member.ObjectID, Percent = member.PercentHealth, Expire = time });
            }

            GroupMembers.Add(this);

            Enqueue(p);
        }

        #endregion

        #region Guilds

        public bool CreateGuild(string GuildName)
        {
            if ((MyGuild != null) || (Info.GuildIndex != -1)) return false;
            if (Envir.GetGuild(GuildName) != null) return false;
            if (Info.Level < Settings.Guild_RequiredLevel)
            {
                ReceiveChat(String.Format("Your level is not high enough to create a guild, required: {0}", Settings.Guild_RequiredLevel), ChatType.System);
                return false;
            }
            //check if we have the required items
            for (int i = 0; i < Settings.Guild_CreationCostList.Count; i++)
            {
                ItemVolume Required = Settings.Guild_CreationCostList[i];
                if (Required.Item == null)
                {
                    if (Info.AccountInfo.Gold < Required.Amount)
                    {
                        ReceiveChat(String.Format("Insufficient gold. Creating a guild requires {0} gold.", Required.Amount), ChatType.System);
                        return false;
                    }
                }
                else
                {
                    uint count = Required.Amount;
                    foreach (var item in Info.Inventory.Where(item => item != null && item.Info == Required.Item))
                    {
                        if ((Required.Item.Type == ItemType.Ore) && (item.CurrentDura / 1000 > Required.Amount))
                        {
                            count = 0;
                            break;
                        }
                        if (item.Count > count)
                            count = 0;
                        else
                            count = count - item.Count;
                        if (count == 0) break;
                    }
                    if (count != 0)
                    {
                        if (Required.Amount == 1)
                            ReceiveChat(String.Format("{0} is required to create a guild.", Required.Item.Name), ChatType.System);
                        else
                        {
                            if (Required.Item.Type == ItemType.Ore)
                                ReceiveChat(string.Format("{0} with purity {1} is recuired to create a guild.", Required.Item.Name, Required.Amount / 1000), ChatType.System);
                            else
                                ReceiveChat(string.Format("Insufficient {0}, you need {1} to create a guild.", Required.Item.Name, Required.Amount), ChatType.System);
                        }
                        return false;
                    }
                }
            }
            //take the required items
            for (int i = 0; i < Settings.Guild_CreationCostList.Count; i++)
            {
                ItemVolume Required = Settings.Guild_CreationCostList[i];
                if (Required.Item == null)
                {
                    if (Info.AccountInfo.Gold >= Required.Amount)
                    {
                        Info.AccountInfo.Gold -= Required.Amount;
                        Enqueue(new S.LoseGold { Gold = Required.Amount });
                    }
                }
                else
                {
                    uint count = Required.Amount;
                    for (int o = 0; o < Info.Inventory.Length; o++)
                    {
                        UserItem item = Info.Inventory[o];
                        if (item == null) continue;
                        if (item.Info != Required.Item) continue;

                        if ((Required.Item.Type == ItemType.Ore) && (item.CurrentDura / 1000 > Required.Amount))
                        {
                            Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = item.Count });
                            Info.Inventory[o] = null;
                            break;
                        }
                        if (count > item.Count)
                        {
                            Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = item.Count });
                            Info.Inventory[o] = null;
                            count -= item.Count;
                            continue;
                        }

                        Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = count });
                        if (count == item.Count)
                            Info.Inventory[o] = null;
                        else
                            item.Count -= count;
                        break;
                    }
                }
            }
            RefreshStats();
            //make the guild
            GuildObject guild = new GuildObject(this, GuildName) { Guildindex = ++Envir.NextGuildID };
            Envir.GuildList.Add(guild);
            Info.GuildIndex = guild.Guildindex;
            MyGuild = guild;
            MyGuildRank = guild.FindRank(Name);
            GuildMembersChanged = true;
            GuildNoticeChanged = true;
            GuildCanRequestItems = true;
            //tell us we now have a guild
            BroadcastInfo();
            MyGuild.SendGuildStatus(this);
            return true;
        }
        public void EditGuildMember(string Name, string RankName, byte RankIndex, byte ChangeType)
        {
            if ((MyGuild == null) || (MyGuildRank == null))
            {
                ReceiveChat("You are not in a guild!", ChatType.System);
                return;
            }
            switch (ChangeType)
            {
                case 0: //add member
                    if (!MyGuildRank.Options.HasFlag(RankOptions.CanRecruit))
                    {
                        ReceiveChat("You are not allowed to recruit new members!", ChatType.System);
                        return;
                    }
                    if (Name == "") return;
                    PlayerObject player = Envir.GetPlayer(Name);
                    if (player == null)
                    {
                        ReceiveChat(String.Format("{0} is not online!", Name), ChatType.System);
                        return;
                    }
                    if ((player.MyGuild != null) || (player.MyGuildRank != null) || (player.Info.GuildIndex != -1))
                    {
                        ReceiveChat(String.Format("{0} is already in a guild!", Name), ChatType.System);
                        return;
                    }
                    if (!player.EnableGuildInvite)
                    {
                        ReceiveChat(String.Format("{0} is disabling guild invites!", Name), ChatType.System);
                        return;
                    }
                    if (player.PendingGuildInvite != null)
                    {
                        ReceiveChat(string.Format("{0} already has a guild invite pending.", Name), ChatType.System);
                        return;
                    }
                    player.Enqueue(new S.GuildInvite { Name = MyGuild.Name });
                    player.PendingGuildInvite = MyGuild;
                    break;
                case 1: //delete member
                    if (!MyGuildRank.Options.HasFlag(RankOptions.CanKick))
                    {
                        ReceiveChat("You are not allowed to remove members!", ChatType.System);
                        return;
                    }
                    if (Name == "") return;

                    if (!MyGuild.DeleteMember(this, Name))
                    {
                        return;
                    }
                    break;
                case 2: //promote member (and it'll auto create a new rank at bottom if the index > total ranks!)
                    if (!MyGuildRank.Options.HasFlag(RankOptions.CanChangeRank))
                    {
                        ReceiveChat("You are not allowed to change other members rank!", ChatType.System);
                        return;
                    }
                    if (Name == "") return;
                    MyGuild.ChangeRank(this, Name, RankIndex, RankName);
                    break;
                case 3: //change rank name
                    if (!MyGuildRank.Options.HasFlag(RankOptions.CanChangeRank))
                    {
                        ReceiveChat("You are not allowed to change ranks!", ChatType.System);
                        return;
                    }
                    if ((RankName == "") || (RankName.Length < 3))
                    {
                        ReceiveChat("Rank name to short!", ChatType.System);
                        return;
                    }
                    if (RankName.Contains("\\") || RankName.Length > 20)
                    {
                        return;
                    }
                    if (!MyGuild.ChangeRankName(this, RankName, RankIndex))
                        return;
                    break;
                case 4: //new rank
                    if (!MyGuildRank.Options.HasFlag(RankOptions.CanChangeRank))
                    {
                        ReceiveChat("You are not allowed to change ranks!", ChatType.System);
                        return;
                    }
                    if (MyGuild.Ranks.Count > 254)
                    {
                        ReceiveChat("No more rank slots available.", ChatType.System);
                        return;
                    }
                    MyGuild.NewRank(this);
                    break;
                case 5: //change rank setting
                    if (!MyGuildRank.Options.HasFlag(RankOptions.CanChangeRank))
                    {
                        ReceiveChat("You are not allowed to change ranks!", ChatType.System);
                        return;
                    }
                    int temp;

                    if (!int.TryParse(RankName, out temp))
                    {
                        return;
                    }
                    MyGuild.ChangeRankOption(this, RankIndex, temp, Name);
                    break;
            }
        }
        public void EditGuildNotice(List<string> notice)
        {
            if ((MyGuild == null) || (MyGuildRank == null))
            {
                ReceiveChat("You are not in a guild!", ChatType.System);
                return;
            }
            if (!MyGuildRank.Options.HasFlag(RankOptions.CanChangeNotice))
            {

                ReceiveChat("You are not allowed to change the guild notice!", ChatType.System);
                return;
            }
            if (notice.Count > 200)
            {
                ReceiveChat("Guild notice can not be longer then 200 lines!", ChatType.System);
                return;
            }
            MyGuild.NewNotice(notice);
        }
        public void GuildInvite(bool accept)
        {
            if (PendingGuildInvite == null)
            {
                ReceiveChat("You have not been invited to a guild.", ChatType.System);
                return;
            }
            if (!accept) return;
            if (!PendingGuildInvite.HasRoom())
            {
                ReceiveChat(String.Format("{0} is full.", PendingGuildInvite.Name), ChatType.System);
                return;
            }
            PendingGuildInvite.NewMember(this);
            Info.GuildIndex = PendingGuildInvite.Guildindex;
            MyGuild = PendingGuildInvite;
            MyGuildRank = PendingGuildInvite.FindRank(Name);
            GuildMembersChanged = true;
            GuildNoticeChanged = true;
            //tell us we now have a guild
            BroadcastInfo();
            MyGuild.SendGuildStatus(this);
            PendingGuildInvite = null;
            EnableGuildInvite = false;
            GuildCanRequestItems = true;
        }
        public void RequestGuildInfo(byte Type)
        {
            if (MyGuild == null) return;
            if (MyGuildRank == null) return;
            switch (Type)
            {
                case 0://notice
                    if (GuildNoticeChanged)
                        Enqueue(new S.GuildNoticeChange() { notice = MyGuild.Notice });
                    GuildNoticeChanged = false;
                    break;
                case 1://memberlist
                    if (GuildMembersChanged)
                        Enqueue(new S.GuildMemberChange() { Status = 255, Ranks = MyGuild.Ranks });
                    break;
            }
        }
        public void GuildNameReturn(string Name)
        {
            if (Name == "") CanCreateGuild = false;
            if (!CanCreateGuild) return;
            if ((Name.Length < 3) || (Name.Length > 20))
            {
                ReceiveChat("Guild name too long.", ChatType.System);
                CanCreateGuild = false;
                return;
            }
            if (Name.Contains('\\'))
            {
                CanCreateGuild = false;
                return;
            }
            if (MyGuild != null)
            {
                ReceiveChat("You are already part of a guild.", ChatType.System);
                CanCreateGuild = false;
                return;
            }
            GuildObject guild = Envir.GetGuild(Name);
            if (guild != null)
            {
                ReceiveChat(string.Format("Guild {0} already exists.", Name), ChatType.System);
                CanCreateGuild = false;
                return;
            }


            CreateGuild(Name);
            CanCreateGuild = false;
        }
        public void GuildStorageGoldChange(Byte Type, uint Amount)
        {
            if ((MyGuild == null) || (MyGuildRank == null))
            {
                ReceiveChat("You are not part of a guild.", ChatType.System);
                return;
            }

            if (!InSafeZone)
            {
                ReceiveChat("You cannot use guild storage outside safezones.", ChatType.System);
                return;
            }

            if (Type == 0)//donate
            {
                if (Account.Gold < Amount)
                {
                    ReceiveChat("Insufficient gold.", ChatType.System);
                    return;
                }
                if ((MyGuild.Gold + (UInt64)Amount) > uint.MaxValue)
                {
                    ReceiveChat("Guild gold limit reached.", ChatType.System);
                    return;
                }
                Account.Gold -= Amount;
                MyGuild.Gold += Amount;
                Enqueue(new S.LoseGold { Gold = Amount });
                MyGuild.SendServerPacket(new S.GuildStorageGoldChange() { Type = 0, Name = Info.Name, Amount = Amount });
                MyGuild.NeedSave = true;
            }
            else
            {
                if (MyGuild.Gold < Amount)
                {
                    ReceiveChat("Insufficient gold.", ChatType.System);
                    return;
                }
                if (!CanGainGold(Amount))
                {
                    ReceiveChat("Gold limit reached.", ChatType.System);
                    return;
                }
                if (MyGuildRank.Index != 0)
                {
                    ReceiveChat("Insufficient rank.", ChatType.System);
                    return;
                }

                MyGuild.Gold -= Amount;
                GainGold(Amount);
                MyGuild.SendServerPacket(new S.GuildStorageGoldChange() { Type = 1, Name = Info.Name, Amount = Amount });
                MyGuild.NeedSave = true;
            }
        }
        public void GuildStorageItemChange(Byte Type, int from, int to)
        {
            S.GuildStorageItemChange p = new S.GuildStorageItemChange { Type = (byte)(3 + Type), From = from, To = to };
            if ((MyGuild == null) || (MyGuildRank == null))
            {
                Enqueue(p);
                ReceiveChat("You are not part of a guild.", ChatType.System);
                return;
            }

            if(!InSafeZone)
            {
                Enqueue(p);
                ReceiveChat("You cannot use guild storage outside safezones.", ChatType.System);
    	        return;
            }

            switch (Type)
            {
                case 0://store
                    if (!MyGuildRank.Options.HasFlag(RankOptions.CanStoreItem))
                    {
                        Enqueue(p);
                        ReceiveChat("You do not have permission to store items in guild storage.", ChatType.System);
                        return;
                    }
                    if (from < 0 || from >= Info.Inventory.Length)
                    {
                        Enqueue(p);
                        return;
                    }
                    if (to < 0 || to >= MyGuild.StoredItems.Length)
                    {
                        Enqueue(p);
                        return;
                    }
                    if (Info.Inventory[from] == null)
                    {
                        Enqueue(p);
                        return;
                    }
                    if (Info.Inventory[from].Info.Bind.HasFlag(BindMode.DontStore))
                    {
                        Enqueue(p);
                        return;
                    }
                    if (MyGuild.StoredItems[to] != null)
                    {
                        ReceiveChat("Target slot not empty.", ChatType.System);
                        Enqueue(p);
                        return;
                    }
                    MyGuild.StoredItems[to] = new GuildStorageItem() { Item = Info.Inventory[from], UserId = Info.Index };
                    Info.Inventory[from] = null;
                    RefreshBagWeight();
                    //MyGuild.SendItemInfo(MyGuild.StoredItems[to].Item);
                    MyGuild.SendItemInfo(MyGuild.StoredItems[to].Item);
                    MyGuild.SendServerPacket(new S.GuildStorageItemChange() { Type = 0, User = Info.Index, Item = MyGuild.StoredItems[to], To = to, From = from });
                    MyGuild.NeedSave = true;
                    break;
                case 1://retrieve
                    if (!MyGuildRank.Options.HasFlag(RankOptions.CanRetrieveItem))
                    {

                        ReceiveChat("You do not have permission to retrieve items from guild storage.", ChatType.System);
                        return;
                    }
                    if (from < 0 || from >= MyGuild.StoredItems.Length)
                    {
                        Enqueue(p);
                        return;
                    }
                    if (to < 0 || to >= Info.Inventory.Length)
                    {
                        Enqueue(p);
                        return;
                    }
                    if (Info.Inventory[to] != null)
                    {
                        ReceiveChat("Target slot not empty.", ChatType.System);
                        Enqueue(p);
                        return;
                    }
                    if (MyGuild.StoredItems[from] == null)
                    {
                        Enqueue(p);
                        return;
                    }
                    if (MyGuild.StoredItems[from].Item.Info.Bind.HasFlag(BindMode.DontStore))
                    {
                        Enqueue(p);
                        return;
                    }
                    Info.Inventory[to] = MyGuild.StoredItems[from].Item;
                    MyGuild.StoredItems[from] = null;
                    MyGuild.SendServerPacket(new S.GuildStorageItemChange() { Type = 1, User = Info.Index, To = to, From = from });
                    RefreshBagWeight();
                    MyGuild.NeedSave = true;
                    break;
                case 2://request list
                    if (!GuildCanRequestItems) return;
                    GuildCanRequestItems = false;
                    for (int i = 0; i < MyGuild.StoredItems.Length; i++)
                    {
                        if (MyGuild.StoredItems[i] == null) continue;
                        UserItem item = MyGuild.StoredItems[i].Item;
                        if (item == null) continue;
                        //CheckItemInfo(item.Info);
                        CheckItem(item);
                    }
                    Enqueue(new S.GuildStorageList() { Items = MyGuild.StoredItems });
                    break;
            }

        }
        public void GuildWarReturn(string Name)
        {
            if (MyGuild == null || MyGuildRank != MyGuild.Ranks[0]) return;

            GuildObject enemyGuild = Envir.GetGuild(Name);

            if (enemyGuild == null)
            {
                ReceiveChat(string.Format("Could not find guild {0}.", Name), ChatType.System);
                return;
            }

            if (MyGuild == enemyGuild)
            {
                ReceiveChat("Cannot go to war with your own guild.", ChatType.System);
                return;
            }

            if (MyGuild.WarringGuilds.Contains(enemyGuild))
            {
                ReceiveChat("Already at war with this guild.", ChatType.System);
                return;
            }

            if(MyGuild.Gold < Settings.Guild_WarCost)
            {
                ReceiveChat("Not enough funds in guild bank.", ChatType.System);
                return;
            }

            if (MyGuild.GoToWar(enemyGuild))
            {
                ReceiveChat(string.Format("You started a war with {0}.", Name), ChatType.System);
                enemyGuild.SendMessage(string.Format("{0} has started a war", MyGuild.Name), ChatType.System);

                MyGuild.Gold -= Settings.Guild_WarCost;
            }
        }

        public bool AtWar(PlayerObject attacker)
        {
            if (CurrentMap.Info.Fight) return true;

            if (MyGuild == null) return false;

            if (attacker == null || attacker.MyGuild == null) return false;

            if (!MyGuild.WarringGuilds.Contains(attacker.MyGuild)) return false;

            return true;
        }

        #endregion

        #region Trading

        public void TradeRequest()
        {
            if (TradePartner != null)
            {
                ReceiveChat("You are already trading.", ChatType.System);
                return;
            }

            Point target = Functions.PointMove(CurrentLocation, Direction, 1);
            Cell cell = CurrentMap.GetCell(target);
            PlayerObject player = null;

            if (cell.Objects == null || cell.Objects.Count < 1) return;

            for (int i = 0; i < cell.Objects.Count; i++)
            {
                MapObject ob = cell.Objects[i];
                if (ob.Race != ObjectType.Player) continue;

                player = Envir.GetPlayer(ob.Name);
            }

            if (player == null)
            {
                ReceiveChat(string.Format("You must face someone to trade."), ChatType.System);
                return;
            }

            if (player != null)
            {
                if (!Functions.FacingEachOther(Direction, CurrentLocation, player.Direction, player.CurrentLocation))
                {
                    ReceiveChat(string.Format("You must face someone to trade."), ChatType.System);
                    return;
                }

                if (player == this)
                {
                    ReceiveChat("You cannot trade with your self.", ChatType.System);
                    return;
                }

                if (player.Dead || Dead)
                {
                    ReceiveChat("Cannot trade when dead", ChatType.System);
                    return;
                }

                if (player.TradeInvitation != null)
                {
                    ReceiveChat(string.Format("Player {0} already has a trade invitation.", player.Info.Name), ChatType.System);
                    return;
                }

                if (!player.AllowTrade)
                {
                    ReceiveChat(string.Format("Player {0} is not allowing trade at the moment.", player.Info.Name), ChatType.System);
                    return;
                }

                if (!Functions.InRange(player.CurrentLocation, CurrentLocation, Globals.DataRange) || player.CurrentMap != CurrentMap)
                {
                    ReceiveChat(string.Format("Player {0} is not within trading range.", player.Info.Name), ChatType.System);
                    return;
                }

                if (player.TradePartner != null)
                {
                    ReceiveChat(string.Format("Player {0} is already trading.", player.Info.Name), ChatType.System);
                    return;
                }

                player.TradeInvitation = this;
                player.Enqueue(new S.TradeRequest { Name = Info.Name });
            }
        }
        public void TradeReply(bool accept)
        {
            if (TradeInvitation == null || TradeInvitation.Info == null)
            {
                TradeInvitation = null;
                return;
            }

            if (!accept)
            {
                TradeInvitation.ReceiveChat(string.Format("Player {0} has refused to trade.", Info.Name), ChatType.System);
                TradeInvitation = null;
                return;
            }

            if (TradePartner != null)
            {
                ReceiveChat("You are already trading.", ChatType.System);
                TradeInvitation = null;
                return;
            }

            if (TradeInvitation.TradePartner != null)
            {
                ReceiveChat(string.Format("Player {0} is already trading.", TradeInvitation.Info.Name), ChatType.System);
                TradeInvitation = null;
                return;
            }

            TradePartner = TradeInvitation;
            TradeInvitation.TradePartner = this;
            TradeInvitation = null;

            Enqueue(new S.TradeAccept { Name = TradePartner.Info.Name });
            TradePartner.Enqueue(new S.TradeAccept { Name = Info.Name });
        }
        public void TradeGold(uint amount)
        {
            if (TradePartner == null) return;

            if (Account.Gold < amount)
            {
                return;
            }

            TradeGoldAmount += amount;
            Account.Gold -= amount;

            Enqueue(new S.LoseGold { Gold = amount });
            TradePartner.Enqueue(new S.TradeGold { Amount = TradeGoldAmount });
        }
        public void TradeItem()
        {
            if (TradePartner == null) return;

            for (int i = 0; i < Info.Trade.Length; i++)
            {
                UserItem u = Info.Trade[i];
                if (u == null) continue;

                //TradePartner.CheckItemInfo(u.Info);
                TradePartner.CheckItem(u);
            }

            TradePartner.Enqueue(new S.TradeItem { TradeItems = Info.Trade });
        }
        public void TradeConfirm(bool locked)
        {
            if (TradePartner == null)
            {
                TradeCancel();
                return;
            }

            if (!Functions.InRange(TradePartner.CurrentLocation, CurrentLocation, Globals.DataRange) || TradePartner.CurrentMap != CurrentMap ||
                !Functions.FacingEachOther(Direction, CurrentLocation, TradePartner.Direction, TradePartner.CurrentLocation))
            {
                TradeCancel();
                return;
            }

            TradeLocked = locked;

            if (TradeLocked && !TradePartner.TradeLocked)
            {
                TradePartner.ReceiveChat(string.Format("Player {0} is waiting for you to confirm trade.", Info.Name), ChatType.System);
            }

            if (!TradeLocked || !TradePartner.TradeLocked) return;

            PlayerObject[] TradePair = new PlayerObject[2] { TradePartner, this };

            bool CanTrade = true;
            UserItem u;

            //check if both people can accept the others items
            for (int p = 0; p < 2; p++)
            {
                int o = p == 0 ? 1 : 0;

                if (!TradePair[o].CanGainItems(TradePair[p].Info.Trade))
                {
                    CanTrade = false;
                    TradePair[p].ReceiveChat("Trading partner cannot accept all items.", ChatType.System);
                    TradePair[p].Enqueue(new S.TradeCancel { Unlock = true });

                    TradePair[o].ReceiveChat("Unable to accept all items.", ChatType.System);
                    TradePair[o].Enqueue(new S.TradeCancel { Unlock = true });

                    return;
                }

                if (!TradePair[o].CanGainGold(TradePair[p].TradeGoldAmount))
                {
                    CanTrade = false;
                    TradePair[p].ReceiveChat("Trading partner cannot accept any more gold.", ChatType.System);
                    TradePair[p].Enqueue(new S.TradeCancel { Unlock = true });

                    TradePair[o].ReceiveChat("Unable to accept any more gold.", ChatType.System);
                    TradePair[o].Enqueue(new S.TradeCancel { Unlock = true });

                    return;
                }
            }

            //swap items
            if (CanTrade)
            {
                for (int p = 0; p < 2; p++)
                {
                    int o = p == 0 ? 1 : 0;

                    for (int i = 0; i < TradePair[p].Info.Trade.Length; i++)
                    {
                        u = TradePair[p].Info.Trade[i];

                        if (u == null) continue;

                        TradePair[o].GainItem(u);
                        TradePair[p].Info.Trade[i] = null;
                    }

                    if (TradePair[p].TradeGoldAmount > 0)
                    {
                        TradePair[o].GainGold(TradePair[p].TradeGoldAmount);
                        TradePair[p].TradeGoldAmount = 0;
                    }

                    TradePair[p].ReceiveChat("Trade successful.", ChatType.System);
                    TradePair[p].Enqueue(new S.TradeConfirm());

                    TradePair[p].TradeLocked = false;
                    TradePair[p].TradePartner = null;
                }
            }
        }
        public void TradeCancel()
        {
            if (TradePartner == null)
            {
                return;
            }

            PlayerObject[] TradePair = new PlayerObject[2] { TradePartner, this };

            for (int p = 0; p < 2; p++)
            {
                if (TradePair[p] != null)
                {
                    for (int t = 0; t < TradePair[p].Info.Trade.Length; t++)
                    {
                        UserItem temp = TradePair[p].Info.Trade[t];

                        if (temp == null) continue;

                        for (int i = 0; i < TradePair[p].Info.Inventory.Length; i++)
                        {
                            if (TradePair[p].Info.Inventory[i] != null) continue;

                            //Put item back in inventory
                            if (TradePair[p].CanGainItem(temp))
                            {
                                TradePair[p].RetrieveTradeItem(t, i);
                            }
                            else //Drop item on floor if it can no longer be stored
                            {
                                if (TradePair[p].DropItem(temp, Settings.DropRange))
                                {
                                    TradePair[p].Enqueue(new S.DeleteItem { UniqueID = temp.UniqueID, Count = temp.Count });
                                }
                            }

                            TradePair[p].Info.Trade[t] = null;

                            break;
                        }
                    }

                    //Put back deposited gold
                    if (TradePair[p].TradeGoldAmount > 0)
                    {
                        TradePair[p].GainGold(TradePair[p].TradeGoldAmount);
                        TradePair[p].TradeGoldAmount = 0;
                    }

                    TradePair[p].TradeLocked = false;
                    TradePair[p].TradePartner = null;

                    TradePair[p].Enqueue(new S.TradeCancel { Unlock = false });
                }
            }
        }

        #endregion

        #region Mounts

        public void RefreshMount()
        {
            if (RidingMount)
            {
                if (MountType < 0)
                {
                    RidingMount = false;
                }
                else if (!Mount.CanRide)
                {
                    RidingMount = false;
                    ReceiveChat("You must have a saddle to ride your mount", ChatType.System);
                }
                else if (!Mount.CanMapRide)
                {
                    RidingMount = false;
                    ReceiveChat("You cannot ride on this map", ChatType.System);
                }
                else if (!Mount.CanDungeonRide)
                {
                    RidingMount = false;
                    ReceiveChat("You cannot ride here without a bridle", ChatType.System);
                }
            }
            else
            {
                RidingMount = false;
            }

            RefreshStats();

            Broadcast(GetMountInfo());
            Enqueue(GetMountInfo());
        }
        public void IncreaseMountLoyalty(int amount)
        {
            UserItem item = Info.Equipment[(int)EquipmentSlot.Mount];
            if (item != null && item.CurrentDura < item.MaxDura)
            {
                item.CurrentDura = (ushort)Math.Min(item.MaxDura, item.CurrentDura + amount);
                item.DuraChanged = false;
                Enqueue(new S.ItemRepaired { UniqueID = item.UniqueID, MaxDura = item.MaxDura, CurrentDura = item.CurrentDura });
            }
        }
        public void DecreaseMountLoyalty(int amount)
        {
            if (Envir.Time > DecreaseLoyaltyTime)
            {
                DecreaseLoyaltyTime = Envir.Time + (Mount.SlowLoyalty ? (LoyaltyDelay * 2) : LoyaltyDelay);
                UserItem item = Info.Equipment[(int)EquipmentSlot.Mount];
                if (item != null && item.CurrentDura > 0)
                {
                    DamageItem(item, amount);

                    if (item.CurrentDura == 0)
                    {
                        RefreshMount();
                    }
                }
            }
        }

        #endregion

        #region Fishing

        public void FishingCast(bool cast, bool cancel = false)
        {
            UserItem rod = Info.Equipment[(int)EquipmentSlot.Weapon];

            byte flexibilityStat = 0;
            sbyte successStat = 0;
            int nibbleMin = 0, nibbleMax = 0;
            int failedAddSuccessMin = 0, failedAddSuccessMax = 0;
            FishingProgressMax = Settings.FishingAttempts;//30;

            if (rod == null || (rod.Info.Shape != 49 && rod.Info.Shape != 50) || rod.CurrentDura <= 0)
            {
                Fishing = false;
                return;
            }
            else
            {
                if (rod.Info.Shape == 49 || rod.Info.Shape == 50)
                {
                    flexibilityStat = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, flexibilityStat + rod.Info.CriticalRate)));
                    successStat = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, successStat + rod.Info.MaxAC)));
                    if (cast)
                    {
                        DamageItem(rod, 1, true);
                    }

                }
            }

            UserItem hook = rod.Slots[(int)FishingSlot.Hook];

            if (hook == null)
            {
                ReceiveChat("You need a hook.", ChatType.System);
                return;
            }
            else
            {
                DamagedFishingItem(FishingSlot.Hook, 1);
            }

            foreach (UserItem temp in rod.Slots)
            {
                if (temp == null) continue;

                ItemInfo realItem = Functions.GetRealItem(temp.Info, Info.Level, Info.Class, Envir.ItemInfoList);

                switch (realItem.Type)
                {
                    case ItemType.Hook:
                        {
                            flexibilityStat = (byte)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, flexibilityStat + temp.CriticalRate + realItem.CriticalRate)));
                        }
                        break;
                    case ItemType.Float:
                        {
                            nibbleMin = (int)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, nibbleMin + realItem.MinAC)));
                            nibbleMax = (int)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, nibbleMax + realItem.MaxAC)));
                        }
                        break;
                    case ItemType.Bait:
                        {
                            successStat = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, successStat + realItem.MaxAC)));
                        }
                        break;
                    case ItemType.Finder:
                        {
                            failedAddSuccessMin = (int)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, failedAddSuccessMin + realItem.MinAC)));
                            failedAddSuccessMax = (int)Math.Max(byte.MinValue, (Math.Min(byte.MaxValue, failedAddSuccessMax + realItem.MaxAC)));
                        }
                        break;
                    case ItemType.Reel:
                        {
                            FishingAutoReelChance = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, FishingAutoReelChance + realItem.MaxMAC)));
                            successStat = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, successStat + realItem.MaxAC)));
                        }
                        break;
                    default:
                        break;
                }
            }
            FishingNibbleChance = 5 + Envir.Random.Next(nibbleMin, nibbleMax);

            if (cast) FishingChance = Settings.FishingSuccessStart + (int)successStat + (FishingChanceCounter != 0 ? Envir.Random.Next(failedAddSuccessMin, failedAddSuccessMax) : 0) + (FishingChanceCounter * Settings.FishingSuccessMultiplier); //10 //10
            if (FishingChanceCounter != 0) DamagedFishingItem(FishingSlot.Finder, 1);

            FishingChance = Math.Min(100, Math.Max(0, FishingChance));
            FishingNibbleChance = Math.Min(100, Math.Max(0, FishingNibbleChance));
            FishingAutoReelChance = Math.Min(100, Math.Max(0, FishingAutoReelChance));

            FishingTime = Envir.Time + FishingCastDelay + Settings.FishingDelay;

            if (cast)
            {
                _fishCounter = 0;
                FishFound = false;

                UserItem item = GetBait(1);

                if (item == null)
                {
                    ReceiveChat("You need bait.", ChatType.System);
                    return;
                }

                ConsumeItem(item, 1);
                Fishing = true;
            }
            else
            {
                if (!Fishing)
                {
                    Enqueue(GetFishInfo());
                    return;
                }
                Fishing = false;

                if (FishingProgress > 99)
                {
                    FishingChanceCounter++;
                }

                int getChance = FishingChance + (FishFound ? Envir.Random.Next(10, 24) : 0) + (FishingProgress < 99 ? flexibilityStat / 2 : 0);
                getChance = Math.Min(100, Math.Max(0, getChance));

                if (Envir.Random.Next(0, 100) <= getChance)
                {
                    FishingChanceCounter = 0;

                    int highRate = int.MaxValue;
                    UserItem dropItem = null;
                    foreach (DropInfo drop in Envir.FishingDrops)
                    {
                        int rate = (int)(Envir.Random.Next(0, drop.Chance) / Settings.DropRate); if (rate < 1) rate = 1;

                        if (highRate > rate)
                        {
                            highRate = rate;
                            dropItem = Envir.CreateDropItem(drop.Item);
                        }
                    }

                    if (FreeSpace(Info.Inventory) < 1)
                    {
                        ReceiveChat("No more space.", ChatType.System);
                        return;
                    }

                    if (dropItem != null)
                        GainItem(dropItem);

                    if (Envir.Random.Next(100 - Settings.FishingMobSpawnChance) == 0)
                    {
                        MonsterObject mob = MonsterObject.GetMonster(Envir.GetMonsterInfo(Settings.FishingMonster));

                        if (mob == null) return;

                        mob.Spawn(CurrentMap, Back);
                    }

                    //ReceiveChat("Fishing Success." + getChance, ChatType.System);

                    DamagedFishingItem(FishingSlot.Reel, 1);
                }
                else
                    ReceiveChat("Fish Escaped.", ChatType.System);

                FishFound = false;
                FishFirstFound = false;
            }

            Enqueue(GetFishInfo());
            Broadcast(GetFishInfo());

            if (FishingAutocast && !cast && !cancel)
            {
                FishingCast(true);
                FishingTime = Envir.Time + (FishingCastDelay * 2);
                FishingFoundTime = Envir.Time;
                FishingAutoReelChance = 0;
                FishingNibbleChance = 0;
                FishFirstFound = false;
            }
        }
        public void FishingChangeAutocast(bool autoCast)
        {
            UserItem rod = Info.Equipment[(int)EquipmentSlot.Weapon];

            if (rod.Info.Shape != 49 && rod.Info.Shape != 50) return;

            UserItem reel = rod.Slots[(int)FishingSlot.Reel];

            if (reel == null)
            {
                FishingAutocast = false;
                return;
            }

            FishingAutocast = autoCast;
        }
        public void UpdateFish()
        {
            if (FishFound != true && FishFirstFound != true)
            {
                FishFound = Envir.Random.Next(0, 100) <= FishingNibbleChance;
                FishingFoundTime = FishFound ? Envir.Time + 3000 : Envir.Time;
                if (FishFound)
                {
                    FishFirstFound = true;
                    DamagedFishingItem(FishingSlot.Float, 1);
                }
            }
            else
            {
                if (FishingAutoReelChance != 0 && Envir.Random.Next(0, 100) <= FishingAutoReelChance)
                {
                    FishingCast(false);
                }
            }

            if (FishingFoundTime < Envir.Time)
                FishFound = false;

            FishingTime = Envir.Time + FishingDelay;

            Enqueue(GetFishInfo());

            if (FishingProgress > 100)
            {
                FishingCast(false);
            }
        }
        Packet GetFishInfo()
        {
            FishingProgress = _fishCounter > 0 ? (int)(((decimal)_fishCounter / FishingProgressMax) * 100) : 0;

            return new S.FishingUpdate
            {
                ObjectID = ObjectID,
                Fishing = Fishing,
                ProgressPercent = FishingProgress,
                FishingPoint = Functions.PointMove(CurrentLocation, Direction, 3),
                ChancePercent = FishingChance,
                FoundFish = FishFound
            };
        }

        #endregion

        #region Quests

        public void AcceptQuest(int index)
        {
            bool canAccept = true;

            if (CurrentQuests.Exists(e => e.Info.Index == index)) return; //e.Info.NpcIndex == npcIndex && 

            QuestInfo info = Envir.QuestInfoList.FirstOrDefault(d => d.Index == index);

            if (info == null || !info.CanAccept(this))
            {
                canAccept = false;
            }

            if (CurrentQuests.Count >= Globals.MaxConcurrentQuests)
            {
                ReceiveChat("Maximum amount of quests already taken.", ChatType.System);
                return;
            }

            if (CompletedQuests.Contains(index))
            {
                ReceiveChat("Quest has already been completed.", ChatType.System);
                return;
            }

            //check previous chained quests have been completed
            QuestInfo tempInfo = info;
            while (tempInfo != null && tempInfo.RequiredQuest != 0)
            {
                if (!CompletedQuests.Contains(tempInfo.RequiredQuest))
                {
                    canAccept = false;
                    break;
                }

                tempInfo = Envir.QuestInfoList.FirstOrDefault(d => d.Index == tempInfo.RequiredQuest);
            }

            if (!canAccept)
            {
                ReceiveChat("Could not accept quest.", ChatType.System);
                return;
            }

            if (info.CarryItems.Count > 0)
            {
                foreach (QuestItemTask carryItem in info.CarryItems)
                {
                    uint count = carryItem.Count;

                    while (count > 1)
                    {
                        UserItem item = SMain.Envir.CreateFreshItem(carryItem.Item);

                        if (item.Info.StackSize > count)
                        {
                            item.Count = count;
                            count = 0;
                        }
                        else
                        {
                            count -= item.Info.StackSize;
                            item.Count = item.Info.StackSize;
                        }

                        if (!CanGainQuestItem(item))
                        {
                            RecalculateQuestBag();
                            return;
                        }

                        GainQuestItem(item);
                    }
                }
            }

            QuestProgressInfo quest = new QuestProgressInfo(index) { StartDateTime = DateTime.Now };

            CurrentQuests.Add(quest);
            SendUpdateQuest(quest, QuestState.Add, true);

            CallDefaultNPC(DefaultNPCType.OnAcceptQuest, index);
        }

        public void FinishQuest(int questIndex, int selectedItemIndex = -1)
        {
            QuestProgressInfo quest = CurrentQuests.FirstOrDefault(e => e.Info.Index == questIndex);

            if (quest == null || !quest.Completed) return;

            List<UserItem> rewardItems = new List<UserItem>();

            foreach (var reward in quest.Info.FixedRewards)
            {
                uint count = reward.Count;

                UserItem rewardItem;

                while (count > 0)
                {
                    rewardItem = Envir.CreateFreshItem(reward.Item);
                    if (reward.Item.StackSize >= count)
                    {                   
                        rewardItem.Count = count;
                        count = 0;
                    }
                    else
                    {
                        rewardItem.Count = reward.Item.StackSize;
                        count -= reward.Item.StackSize;
                    }

                    rewardItems.Add(rewardItem);
                }
            }

            if (selectedItemIndex >= 0)
            {
                for (int i = 0; i < quest.Info.SelectRewards.Count; i++)
                {
                    if (selectedItemIndex != i) continue;

                    uint count = quest.Info.SelectRewards[i].Count;
                    UserItem rewardItem;

                    while (count > 0)
                    {
                        rewardItem = Envir.CreateFreshItem(quest.Info.SelectRewards[i].Item);
                        if (quest.Info.SelectRewards[i].Item.StackSize >= count)
                        {
                            rewardItem.Count = count;
                            count = 0;
                        }
                        else
                        {
                            rewardItem.Count = quest.Info.SelectRewards[i].Item.StackSize;
                            count -= quest.Info.SelectRewards[i].Item.StackSize;
                        }

                        rewardItems.Add(rewardItem);
                    }
                }
            }

            if (!CanGainItems(rewardItems.ToArray()))
            {
                ReceiveChat("Cannot hand in quest whilst bag is full.", ChatType.System);
                return;
            }

            if (quest.Info.Type != QuestType.Repeatable)
            {
                Info.CompletedQuests.Add(quest.Index);
                GetCompletedQuests();
            }

            CurrentQuests.Remove(quest);
            SendUpdateQuest(quest, QuestState.Remove);

            if (quest.Info.CarryItems.Count > 0)
            {
                foreach (QuestItemTask carryItem in quest.Info.CarryItems)
                {
                    TakeQuestItem(carryItem.Item, carryItem.Count);
                }
            }

            foreach (QuestItemTask iTask in quest.Info.ItemTasks)
            {
                TakeQuestItem(iTask.Item, Convert.ToUInt32(iTask.Count));
            }

            foreach (UserItem item in rewardItems)
            {
                GainItem(item);
            }

            RecalculateQuestBag();

            GainGold(quest.Info.GoldReward);
            GainExp(quest.Info.ExpReward);

            CallDefaultNPC(DefaultNPCType.OnFinishQuest, questIndex);
        }
        public void AbandonQuest(int questIndex)
        {
            QuestProgressInfo quest = CurrentQuests.FirstOrDefault(e => e.Info.Index == questIndex);

            if (quest == null) return;

            CurrentQuests.Remove(quest);
            SendUpdateQuest(quest, QuestState.Remove);

            RecalculateQuestBag();
        }
        public void ShareQuest(int questIndex)
        {
            bool shared = false;

            if (GroupMembers != null)
            {
                foreach (PlayerObject player in GroupMembers.
                    Where(player => player.CurrentMap == CurrentMap &&
                        Functions.InRange(player.CurrentLocation, CurrentLocation, Globals.DataRange) &&
                        !player.Dead && player != this))
                {
                    player.Enqueue(new S.ShareQuest { QuestIndex = questIndex, SharerName = Name });
                    shared = true;
                }
            }

            if(!shared)
            {
                ReceiveChat("Quest could not be shared with anyone.", ChatType.System);
            }
        }

        public void CheckGroupQuestKill(MonsterInfo mInfo)
        {
            if (GroupMembers != null)
            {
                foreach (PlayerObject player in GroupMembers.
                    Where(player => player.CurrentMap == CurrentMap &&
                        Functions.InRange(player.CurrentLocation, CurrentLocation, Globals.DataRange) &&
                        !player.Dead))
                {
                    player.CheckNeedQuestKill(mInfo);
                }
            }
            else
                CheckNeedQuestKill(mInfo);
        }
        public bool CheckGroupQuestItem(UserItem item)
        {
            bool itemCollected = false;

            if (GroupMembers != null)
            {
                foreach (PlayerObject player in GroupMembers.
                    Where(player => player.CurrentMap == CurrentMap &&
                        Functions.InRange(player.CurrentLocation, CurrentLocation, Globals.DataRange) &&
                        !player.Dead))
                {
                    if(player.CheckNeedQuestItem(item))
                    {
                        itemCollected = true;
                    }
                }
            }
            else
            {
                if(CheckNeedQuestItem(item))
                {
                    itemCollected = true;
                }
            }

            return itemCollected;
        }

        public bool CheckNeedQuestItem(UserItem item, bool gainItem = true)
        {
            foreach (QuestProgressInfo quest in CurrentQuests.
                Where(e => e.ItemTaskCount.Count > 0).
                Where(e => e.NeedItem(item.Info)).
                Where(e => CanGainQuestItem(item)))
            {
                if (gainItem)
                {
                    GainQuestItem(item);
                    quest.ProcessItem(Info.QuestInventory);

                    Enqueue(new S.SendOutputMessage { Message = string.Format("You found {0}.", item.FriendlyName), Type = OutputMessageType.Quest });

                    SendUpdateQuest(quest, QuestState.Update);
                }
                return true;
            }

            return false;
        }
        public bool CheckNeedQuestFlag(int flagNumber)
        {
            foreach (QuestProgressInfo quest in CurrentQuests.
                Where(e => e.FlagTaskSet.Count > 0).
                Where(e => e.NeedFlag(flagNumber)))
            {
                quest.ProcessFlag(Info.Flags);

                //Enqueue(new S.SendOutputMessage { Message = string.Format("Location visited."), Type = OutputMessageType.Quest });

                SendUpdateQuest(quest, QuestState.Update);
                return true;
            }

            return false;
        }
        public void CheckNeedQuestKill(MonsterInfo mInfo)
        {
            foreach (QuestProgressInfo quest in CurrentQuests.
                    Where(e => e.KillTaskCount.Count > 0).
                    Where(quest => quest.NeedKill(mInfo)))
            {
                quest.ProcessKill(mInfo.Index);

                Enqueue(new S.SendOutputMessage { Message = string.Format("You killed {0}.", mInfo.Name), Type = OutputMessageType.Quest });

                SendUpdateQuest(quest, QuestState.Update);
            }
        }

        public void RecalculateQuestBag()
        {
            for (int i = Info.QuestInventory.Length - 1; i >= 0; i--)
            {
                UserItem itm = Info.QuestInventory[i];

                if (itm == null) continue;

                bool itemRequired = false;

                foreach (QuestProgressInfo quest in CurrentQuests)
                {
                    foreach (QuestItemTask task in quest.Info.ItemTasks)
                    {
                        if (task.Item == itm.Info)
                        {
                            itemRequired = true;
                            break;
                        }
                    }
                }

                if (!itemRequired)
                {
                    Info.QuestInventory[i] = null;
                    Enqueue(new S.DeleteQuestItem { UniqueID = itm.UniqueID, Count = itm.Count });
                }
            }
        }

        public void SendUpdateQuest(QuestProgressInfo quest, QuestState state, bool trackQuest = false)
        {
            quest.CheckCompleted();
 
            Enqueue(new S.ChangeQuest 
            { 
                Quest = quest.CreateClientQuestProgress(), 
                QuestState = state,
                TrackQuest = trackQuest
            });
        }

        public void GetCompletedQuests()
        {
            //CompletedQuests.Clear();
            //for (int i = 1000; i < Globals.FlagIndexCount; i++)
            //    if (Info.Flags[i]) CompletedQuests.Add(i - 1000);

            Enqueue(new S.CompleteQuest
            {
                CompletedQuests = CompletedQuests
            });
        }

        #endregion

        #region Mail

        public void SendMail(string name, string message)
        {
            CharacterInfo player = Envir.GetCharacterInfo(name);

            if(player == null)
            {
                ReceiveChat(string.Format("Could not find player {0}", name), ChatType.System);
                return;
            }

            //sent from player
            MailInfo mail = new MailInfo(player.Index, true)
            {
                MailID = ++Envir.NextMailID,
                Sender = Info.Name,
                Message = message,
                Gold = 0
            };

            mail.Send();
        }

        public void SendMail(string name, string message, uint gold, ulong[] items, bool stamped)
        {
            CharacterInfo player = Envir.GetCharacterInfo(name);

            if (player == null)
            {
                ReceiveChat(string.Format("Could not find player {0}", name), ChatType.System);
                return;
            }

            bool hasStamp = false;
            uint totalGold = 0;
            uint parcelCost = GetMailCost(items, gold, stamped);

            totalGold = gold + parcelCost;

            if (Account.Gold < totalGold)
            {
                Enqueue(new S.MailSent { Result = -1 });
                return;
            }

            //Validate user has stamp
            if (stamped)
            {
                for (int i = 0; i < Info.Inventory.Length; i++)
                {
                    UserItem item = Info.Inventory[i];

                    if (item == null || item.Info.Type != ItemType.Nothing || item.Info.Shape != 1 || item.Count < 1) continue;

                    hasStamp = true;

                    if (item.Count > 1) item.Count--;
                    else Info.Inventory[i] = null;

                    Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = 1 });
                    break;
                }
            }

            List<UserItem> giftItems = new List<UserItem>();

            for (int j = 0; j < (hasStamp ? 5 : 1); j++)
            {
                if (items[j] < 1) continue;

                for (int i = 0; i < Info.Inventory.Length; i++)
                {
                    UserItem item = Info.Inventory[i];

                    if (item == null || items[j] != item.UniqueID) continue;

                    giftItems.Add(item);

                    Info.Inventory[i] = null;
                    Enqueue(new S.DeleteItem { UniqueID = item.UniqueID, Count = item.Count });
                }
            }

            if (totalGold > 0)
            {
                Account.Gold -= totalGold;
                Enqueue(new S.LoseGold { Gold = totalGold });
            }

            //Create parcel
            MailInfo mail = new MailInfo(player.Index, true)
            {
                MailID = ++Envir.NextMailID,
                Sender = Info.Name,
                Message = message,
                Gold = gold,
                Items = giftItems
            };

            mail.Send();

            Enqueue(new S.MailSent { Result = 1 });
        }

        public void ReadMail(ulong mailID)
        {
            MailInfo mail = Info.Mail.FirstOrDefault(e => e.MailID == mailID);

            if (mail == null) return;

            mail.DateOpened = DateTime.Now;

            GetMail();
        }

        public void CollectMail(ulong mailID)
        {
            MailInfo mail = Info.Mail.FirstOrDefault(e => e.MailID == mailID);

            if (mail == null) return;

            if(!mail.Collected)
            {
                ReceiveChat("Mail must be collected from the post office.", ChatType.System);
                return;
            }

            if (mail.Items.Count > 0)
            {
                if (!CanGainItems(mail.Items.ToArray()))
                {
                    ReceiveChat("Cannot collect items when bag is full.", ChatType.System);
                    return;
                }

                for (int i = 0; i < mail.Items.Count; i++)
                {
                    GainItem(mail.Items[i]);
                }
            }

            if(mail.Gold > 0)
            {
                uint count = mail.Gold;

                if (count + Account.Gold >= uint.MaxValue)
                    count = uint.MaxValue - Account.Gold;

                GainGold(count);
            }

            mail.Items = new List<UserItem>();
            mail.Gold = 0;

            mail.Collected = true;

            Enqueue(new S.ParcelCollected { Result = 1 });

            GetMail();
        }

        public void DeleteMail(ulong mailID)
        {
            MailInfo mail = Info.Mail.FirstOrDefault(e => e.MailID == mailID);

            if (mail == null) return;

            Info.Mail.Remove(mail);

            GetMail();
        }

        public void LockMail(ulong mailID, bool lockMail)
        {
            MailInfo mail = Info.Mail.FirstOrDefault(e => e.MailID == mailID);

            if (mail == null) return;

            mail.Locked = lockMail;

            GetMail();
        }

        public uint GetMailCost(ulong[] items, uint gold, bool stamped)
        {
            uint cost = 0;

            if (!Settings.MailFreeWithStamp || !stamped)
            {
                if (gold > 0 && Settings.MailCostPer1KGold > 0)
                {
                    cost += (uint)Math.Floor((decimal)gold / 1000) * Settings.MailCostPer1KGold;
                }

                if (items != null && items.Length > 0 && Settings.MailItemInsurancePercentage > 0)
                {
                    for (int j = 0; j < (stamped ? 5 : 1); j++)
                    {
                        if (items[j] < 1) continue;

                        for (int i = 0; i < Info.Inventory.Length; i++)
                        {
                            UserItem item = Info.Inventory[i];

                            if (item == null || items[j] != item.UniqueID) continue;

                            cost += (uint)Math.Floor((double)item.Price() / 100 * Settings.MailItemInsurancePercentage);
                        }
                    }
                }
            }


            return cost;
        }

        public void GetMail()
        {
            List<ClientMail> mail = new List<ClientMail>();

            foreach(MailInfo m in Info.Mail)
            {
                foreach(UserItem itm in m.Items)
                {
                    CheckItem(itm);
                }

                mail.Add(m.CreateClientMail());
            }

            NewMail = false;

            Enqueue(new S.ReceiveMail { Mail = mail });
        }

        public int GetMailAwaitingCollectionAmount()
        {
            int count = 0;
            for (int i = 0; i < Info.Mail.Count; i++)
            {
                if (!Info.Mail[i].Collected) count++;
            }

            return count;
        }

        #endregion

        #region IntelligentCreatures

        public void SummonIntelligentCreature(IntelligentCreatureType pType)
        {
            if (pType == IntelligentCreatureType.None) return;

            for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
            {
                if (Info.IntelligentCreatures[i].PetType != pType) continue;

                MonsterInfo mInfo = Envir.GetMonsterInfo(Settings.IntelligentCreatureNameList[(byte)pType]);
                if (mInfo == null) return;

                byte petlevel = 0;//for future use

                MonsterObject monster = MonsterObject.GetMonster(mInfo);
                if (monster == null) return;
                monster.PetLevel = petlevel;
                monster.Master = this;
                monster.MaxPetLevel = 7;
                monster.Direction = Direction;
                monster.ActionTime = Envir.Time + 1000;
                ((IntelligentCreatureObject)monster).CustomName = Info.IntelligentCreatures[i].CustomName;
                ((IntelligentCreatureObject)monster).CreatureRules = new IntelligentCreatureRules
                {
                    MinimalFullness = Info.IntelligentCreatures[i].Info.MinimalFullness,
                    MousePickupEnabled = Info.IntelligentCreatures[i].Info.MousePickupEnabled,
                    MousePickupRange = Info.IntelligentCreatures[i].Info.MousePickupRange,
                    AutoPickupEnabled = Info.IntelligentCreatures[i].Info.AutoPickupEnabled,
                    AutoPickupRange = Info.IntelligentCreatures[i].Info.AutoPickupRange,
                    SemiAutoPickupEnabled = Info.IntelligentCreatures[i].Info.SemiAutoPickupEnabled,
                    SemiAutoPickupRange = Info.IntelligentCreatures[i].Info.SemiAutoPickupRange,
                    CanProduceBlackStone = Info.IntelligentCreatures[i].Info.CanProduceBlackStone
                };
                ((IntelligentCreatureObject)monster).ItemFilter = Info.IntelligentCreatures[i].Filter;
                ((IntelligentCreatureObject)monster).CurrentPickupMode = Info.IntelligentCreatures[i].petMode;
                ((IntelligentCreatureObject)monster).Fullness = Info.IntelligentCreatures[i].Fullness;
                ((IntelligentCreatureObject)monster).blackstoneTime = Info.IntelligentCreatures[i].BlackstoneTime;
                ((IntelligentCreatureObject)monster).maintainfoodTime = Info.IntelligentCreatures[i].MaintainFoodTime;


                monster.Spawn(CurrentMap, Front);
                Pets.Add(monster);//make a new creaturelist ? 

                CreatureSummoned = true;
                SummonedCreatureType = pType;

                ReceiveChat((string.Format("Creature {0} has been summoned.", Info.IntelligentCreatures[i].CustomName)), ChatType.System);
                break;
            }
            //update client
            GetCreaturesInfo();
        }
        public void UnSummonIntelligentCreature(IntelligentCreatureType pType, bool doUpdate = true)
        {
            if (pType == IntelligentCreatureType.None) return;

            for (int i = 0; i < Pets.Count; i++)
            {
                if (Pets[i].Info.AI != 64) continue;
                if (((IntelligentCreatureObject)Pets[i]).petType != pType) continue;

                if (doUpdate) ReceiveChat((string.Format("Creature {0} has been dismissed.", ((IntelligentCreatureObject)Pets[i]).CustomName)), ChatType.System);

                Pets[i].Die();

                CreatureSummoned = false;
                SummonedCreatureType = IntelligentCreatureType.None;
                break;
            }
            //update client
            if (doUpdate) GetCreaturesInfo();
        }
        public void ReleaseIntelligentCreature(IntelligentCreatureType pType, bool doUpdate = true)
        {
            if (pType == IntelligentCreatureType.None) return;

            //remove creature
            for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
            {
                if (Info.IntelligentCreatures[i].PetType != pType) continue;

                if (doUpdate) ReceiveChat((string.Format("Creature {0} has been released.", Info.IntelligentCreatures[i].CustomName)), ChatType.System);

                Info.IntelligentCreatures.Remove(Info.IntelligentCreatures[i]);
                break;
            }

            //re-arange slots
            for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
                Info.IntelligentCreatures[i].SlotIndex = i;

            //update client
            if (doUpdate) GetCreaturesInfo();
        }

        public void UpdateSummonedCreature(IntelligentCreatureType pType)
        {
            if (pType == IntelligentCreatureType.None) return;

            UserIntelligentCreature creatureInfo = null;
            for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
            {
                if (Info.IntelligentCreatures[i].PetType != pType) continue;

                creatureInfo = Info.IntelligentCreatures[i];
                break;
            }
            if (creatureInfo == null) return;

            for (int i = 0; i < Pets.Count; i++)
            {
                if (Pets[i].Info.AI != 64) continue;
                if (((IntelligentCreatureObject)Pets[i]).petType != pType) continue;

                ((IntelligentCreatureObject)Pets[i]).CustomName = creatureInfo.CustomName;
                ((IntelligentCreatureObject)Pets[i]).ItemFilter = creatureInfo.Filter;
                ((IntelligentCreatureObject)Pets[i]).CurrentPickupMode = creatureInfo.petMode;
                break;
            }
        }
        public void UpdateCreatureFullness(IntelligentCreatureType pType, int fullness)
        {
            if (pType == IntelligentCreatureType.None) return;

            for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
            {
                if (Info.IntelligentCreatures[i].PetType != pType) continue;
                Info.IntelligentCreatures[i].Fullness = fullness;
                break;
            }

            //update client
            //GetCreaturesInfo();
        }
        public void UpdateCreatureBlackstoneTime(IntelligentCreatureType pType, long blackstonetime)
        {
            if (pType == IntelligentCreatureType.None) return;

            for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
            {
                if (Info.IntelligentCreatures[i].PetType != pType) continue;
                Info.IntelligentCreatures[i].BlackstoneTime = blackstonetime;
                break;
            }

            //update client
            //GetCreaturesInfo();
        }
        public void UpdateCreatureMaintainFoodTime(IntelligentCreatureType pType, long maintainfoodtime)
        {
            if (pType == IntelligentCreatureType.None) return;

            for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
            {
                if (Info.IntelligentCreatures[i].PetType != pType) continue;
                Info.IntelligentCreatures[i].MaintainFoodTime = maintainfoodtime;
                break;
            }

            //update client
            //GetCreaturesInfo();
        }

        public void RefreshCreaturesTimeLeft()
        {
            if (Envir.Time > CreatureTimeLeftTicker)
            {
                //Make sure summoned vars are in correct state
                RefreshCreatureSummoned();

                //ExpireTime
                List<int> releasedPets = new List<int>();
                CreatureTimeLeftTicker = Envir.Time + CreatureTimeLeftDelay;
                for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
                {
                    if (Info.IntelligentCreatures[i].ExpireTime == -9999) continue;//permanent
                    Info.IntelligentCreatures[i].ExpireTime = Info.IntelligentCreatures[i].ExpireTime - 1;
                    if (Info.IntelligentCreatures[i].ExpireTime <= 0)
                    {
                        Info.IntelligentCreatures[i].ExpireTime = 0;
                        if (CreatureSummoned && SummonedCreatureType == Info.IntelligentCreatures[i].PetType)
                            UnSummonIntelligentCreature(SummonedCreatureType, false);//unsummon creature
                        releasedPets.Add(i);
                    }
                }
                for (int i = (releasedPets.Count-1); i >= 0; i--)//start with largest value
                {
                    ReceiveChat((string.Format("Creature {0} has expired.", Info.IntelligentCreatures[releasedPets[i]].CustomName)), ChatType.System);
                    ReleaseIntelligentCreature(Info.IntelligentCreatures[releasedPets[i]].PetType, false);//release creature
                }

                if (CreatureSummoned && SummonedCreatureType != IntelligentCreatureType.None)
                {
                    for (int i = 0; i < Pets.Count; i++)
                    {
                        if (Pets[i].Info.AI != 64) continue;
                        if (((IntelligentCreatureObject)Pets[i]).petType != SummonedCreatureType) continue;

                        ((IntelligentCreatureObject)Pets[i]).ProcessBlackStoneProduction();
                        ((IntelligentCreatureObject)Pets[i]).ProcessMaintainFoodBuff();
                        break;
                    }
                }

                //update client
                GetCreaturesInfo();
            }
        }
        public void RefreshCreatureSummoned()
        {
            if (SummonedCreatureType == IntelligentCreatureType.None || !CreatureSummoned)
            {
                //make sure both are in the unsummoned state
                CreatureSummoned = false;
                SummonedCreatureType = IntelligentCreatureType.None;
                return;
            }
            bool petFound = false;
            for (int i = 0; i < Pets.Count; i++)
            {
                if (Pets[i].Info.AI != 64) continue;
                if (((IntelligentCreatureObject)Pets[i]).petType != SummonedCreatureType) continue;
                petFound = true;
                break;
            }
            if (!petFound)
            {
                SMain.EnqueueDebugging(string.Format("{0}: SummonedCreature no longer exists?!?. {1}", Name, SummonedCreatureType.ToString()));
                CreatureSummoned = false;
                SummonedCreatureType = IntelligentCreatureType.None;
            }
        }

        public void IntelligentCreaturePickup(bool mousemode, Point atlocation)
        {
            if (!CreatureSummoned) return;

            for (int i = 0; i < Pets.Count; i++)
            {
                if (Pets[i].Info.AI != 64) continue;
                if (((IntelligentCreatureObject)Pets[i]).petType != SummonedCreatureType) continue;

                //((IntelligentCreatureObject)Pets[i]).MouseLocation = atlocation;
                ((IntelligentCreatureObject)Pets[i]).ManualPickup(mousemode, atlocation);
                break;
            }
        }

        public void IntelligentCreatureGainPearls(int amount)
        {
            Info.PearlCount += amount;
            if (Info.PearlCount > int.MaxValue) Info.PearlCount = int.MaxValue;
        }

        public void IntelligentCreatureLosePearls(int amount)
        {
            Info.PearlCount -= amount;
            if (Info.PearlCount < 0) Info.PearlCount = 0;
        }

        public void IntelligentCreatureProducePearl()
        {
            Info.PearlCount++;
        }
        public bool IntelligentCreatureProduceBlackStone()
        {
            ItemInfo iInfo = Envir.GetItemInfo(Settings.CreatureBlackStoneName);
            if (iInfo == null) return false;

            UserItem item = Envir.CreateDropItem(iInfo);
            item.Count = 1;

            if (!CanGainItem(item, false)) return false;

            GainItem(item);
            return true;
        }

        public void IntelligentCreatureSay(IntelligentCreatureType pType,string message)
        {
            if(!CreatureSummoned || message == "" ) return;
            if(pType != SummonedCreatureType) return;

            for (int i = 0; i < Pets.Count; i++)
            {
                if (Pets[i].Info.AI != 64) continue;
                if (((IntelligentCreatureObject)Pets[i]).petType != pType) continue;

                Enqueue(new S.ObjectChat { ObjectID = Pets[i].ObjectID, Text = message, Type = ChatType.Normal });
                return;
            }
        }

        public void StrongboxRewardItem(int boxtype)
        {
            int highRate = int.MaxValue;
            UserItem dropItem = null;
            foreach (DropInfo drop in Envir.StrongboxDrops)
            {
                int rate = (int)(Envir.Random.Next(0, drop.Chance) / Settings.DropRate);
                if (rate < 1) rate = 1;

                if (highRate > rate)
                {
                    highRate = rate;
                    dropItem = Envir.CreateFreshItem(drop.Item);
                }
            }

            if (dropItem.Info.Type == ItemType.Pets && dropItem.Info.Shape == 26)
            {
                dropItem = CreateDynamicWonderDrug(boxtype, dropItem);
            }
            else
                dropItem = Envir.CreateDropItem(dropItem.Info);

            if (FreeSpace(Info.Inventory) < 1)
            {
                ReceiveChat("No more space.", ChatType.System);
                return;
            }

            if (dropItem != null) GainItem(dropItem);
        }

        public void BlackstoneRewardItem()
        {
            int highRate = int.MaxValue;
            UserItem dropItem = null;
            foreach (DropInfo drop in Envir.BlackstoneDrops)
            {
                int rate = (int)(Envir.Random.Next(0, drop.Chance) / Settings.DropRate); if (rate < 1) rate = 1;

                if (highRate > rate)
                {
                    highRate = rate;
                    dropItem = Envir.CreateDropItem(drop.Item);
                }
            }
            if (FreeSpace(Info.Inventory) < 1)
            {
                ReceiveChat("No more space.", ChatType.System);
                return;
            }
            if (dropItem != null) GainItem(dropItem);
        }

        private UserItem CreateDynamicWonderDrug(int boxtype, UserItem dropitem)
        {
            dropitem.CurrentDura = (ushort)1;//* 3600
            switch ((int)dropitem.Info.Effect)
            {
                case 0://exp low/med/high
                    dropitem.Luck = (sbyte)5;
                    if (boxtype > 0) dropitem.Luck = (sbyte)10;
                    if (boxtype > 1) dropitem.Luck = (sbyte)20;
                    break;
                case 1://drop low/med/high
                    dropitem.Luck = (sbyte)10;
                    if (boxtype > 0) dropitem.Luck = (sbyte)20;
                    if (boxtype > 1) dropitem.Luck = (sbyte)50;
                    break;
                case 2://hp low/med/high
                    dropitem.HP = (byte)50;
                    if (boxtype > 0) dropitem.HP = (byte)100;
                    if (boxtype > 1) dropitem.HP = (byte)200;
                    break;
                case 3://mp low/med/high
                    dropitem.MP = (byte)50;
                    if (boxtype > 0) dropitem.MP = (byte)100;
                    if (boxtype > 1) dropitem.MP = (byte)200;
                    break;
                case 4://ac low/med/high
                    dropitem.AC = (byte)1;
                    if (boxtype > 0) dropitem.AC = (byte)3;
                    if (boxtype > 1) dropitem.AC = (byte)5;
                    break;
                case 5://amc low/med/high
                    dropitem.MAC = (byte)1;
                    if (boxtype > 0) dropitem.MAC = (byte)3;
                    if (boxtype > 1) dropitem.MAC = (byte)5;
                    break;
                case 6://speed low/med/high
                    dropitem.AttackSpeed = (sbyte)2;
                    if (boxtype > 0) dropitem.AttackSpeed = (sbyte)3;
                    if (boxtype > 1) dropitem.AttackSpeed = (sbyte)4;
                    break;
                case 7://knapsack low/med/high
                    dropitem.CurrentDura = (ushort)5;//* 3600
                    dropitem.Luck = (sbyte)3;
                    if (boxtype > 0) dropitem.Luck = (sbyte)5;
                    if (boxtype > 1) dropitem.Luck = (sbyte)9;
                    break;
            }
            //string dbg = String.Format(" Img: {0} Effect: {1} Dura: {2} Exp: {3} Drop: {3} HP: {4} MP: {5} AC: {6} MAC: {7} ASpeed: {8} BagWeight: {9}", dropitem.Image, dropitem.Info.Effect, dropitem.CurrentDura, dropitem.Luck, dropitem.HP, dropitem.MP, dropitem.AC, dropitem.MAC, dropitem.AttackSpeed, dropitem.Luck);
            //ReceiveChat(dropitem.Name + dbg, ChatType.System);
            return dropitem;
        }

        private IntelligentCreatureObject GetCreatureByName(string creaturename)
        {
            if (!CreatureSummoned || creaturename == "") return null;
            if (SummonedCreatureType == IntelligentCreatureType.None) return null;

            for (int i = 0; i < Pets.Count; i++)
            {
                if (Pets[i].Info.AI != 64) continue;
                if (((IntelligentCreatureObject)Pets[i]).petType != SummonedCreatureType) continue;

                return ((IntelligentCreatureObject)Pets[i]);
            }
            return null;
        }

        private string CreateTimeString(double secs)
        {
            TimeSpan t = TimeSpan.FromSeconds(secs);
            string answer;
            if (t.TotalMinutes < 1.0)
            {
                answer = string.Format("{0}s", t.Seconds);
            }
            else if (t.TotalHours < 1.0)
            {
                answer = string.Format("{0}m", t.Minutes);
            }
            else if (t.TotalDays < 1.0)
            {
                answer = string.Format("{0}h {1:D2}m", (int)t.TotalHours, t.Minutes);
            }
            else // t.TotalDays >= 1.0
            {
                answer = string.Format("{0}d {1}h {2:D2}m", (int)t.TotalDays, (int)t.Hours, t.Minutes);
            }
            return answer;
        }

        private void GetCreaturesInfo()
        {
            S.UpdateIntelligentCreatureList packet = new S.UpdateIntelligentCreatureList
            {
                CreatureSummoned = CreatureSummoned,
                SummonedCreatureType = SummonedCreatureType,
                PearlCount = Info.PearlCount,
            };

            for (int i = 0; i < Info.IntelligentCreatures.Count; i++)
                packet.CreatureList.Add(Info.IntelligentCreatures[i].CreateClientIntelligentCreature());

            Enqueue(packet);
        }


        #endregion

        #region Friends

        public void AddFriend(int index)
        {
            CharacterInfo info = Envir.GetCharacterInfo(index);

            if (info == null) return;

            if (Info.Friends.Any(e => e.CharacterIndex == index)) return;

            FriendInfo friend = new FriendInfo(index);

            Info.Friends.Add(friend);
        }

        public void RemoveFriend(int index)
        {
            FriendInfo friend = Info.Friends.FirstOrDefault(e => e.CharacterIndex == index);

            if (friend == null) return;

            Info.Friends.Remove(friend);
        }

        public void AddMemo(int index, string memo)
        {
            FriendInfo friend = Info.Friends.FirstOrDefault(e => e.CharacterIndex == index);

            if (friend == null) return;

            friend.Memo = memo;
        }

        public void GetFriends()
        {

        }

        #endregion
    }

    public class ItemSets
    {
        public ItemSet Set;
        public List<ItemType> Type;
        private byte Amount
        {
            get
            {
                switch (Set)
                {
                    case ItemSet.Mundane:
                    case ItemSet.NokChi:
                    case ItemSet.TaoProtect:
                        return 2;
                    case ItemSet.RedOrchid:
                    case ItemSet.RedFlower:
                    case ItemSet.Smash:
                    case ItemSet.HwanDevil:
                    case ItemSet.Purity:
                    case ItemSet.FiveString:
                    case ItemSet.Bone:
                    case ItemSet.Bug:
                        return 3;
                    case ItemSet.Recall:
                        return 4;
                    case ItemSet.Spirit:
                    case ItemSet.WhiteGold:
                    case ItemSet.WhiteGoldH:
                    case ItemSet.RedJade:
                    case ItemSet.RedJadeH:
                    case ItemSet.Nephrite:
                    case ItemSet.NephriteH:
                        return 5;
                    default:
                        return 0;
                }
            }
        }
        public byte Count;
        public bool SetComplete
        {
            get
            {
                return Count == Amount;
            }
        }
    }

}

