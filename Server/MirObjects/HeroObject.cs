using System;
using System.Collections.Generic;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects
{
    public class HeroObject : MonsterObject
    {
        public HeroInfo HInfo;
        private PlayerFunctions SharedFunctions;
        public override bool Blocking
        {
            get
            {
                return false;
            }
        }

        protected override bool CanMove
        {
            get
            {
                return Envir.Time > MoveTime && Envir.Time > ActionTime;
            }
        }

        public override string Name
        {
            get { return Master == null ? Info.Name : HInfo.Name; }
            set { throw new NotSupportedException(); }
        }

        public MirClass Class
        {
            get { return HInfo.Class; }
            set { throw new NotSupportedException(); }
        }

        public long MaxExperience
        {
            get { return HInfo.MaxExperience; }
            set { HInfo.MaxExperience = value; }
        }

        public override ushort Level
        {
            get { return HInfo.Level; }
            set { HInfo.Level = value; }
        }

        public int MP
        {
            get { return HInfo.MP; }
            set { HInfo.MP = value; }
        }

        public int CurrentHandWeight,
                   CurrentWearWeight,
                   CurrentBagWeight;

        public long PotTime, VampTime;
        public const long PotDelay = 200, VampDelay = 500;

        protected override bool CanAttack
        {
            get
            {
                return !Dead && Envir.Time > AttackTime && Envir.Time > ActionTime;
            }
        }

        public override ObjectType Race
        {
            get { return ObjectType.Hero; }
        }


        public HeroObject(MonsterInfo info) : base(info)
        {
            ActionTime = Envir.Time + 1000;
            CreateSharedFunctions();
        }

        private void CreateSharedFunctions()
        {
            SharedFunctions = new PlayerFunctions()
            {
                //Getters
                getStats = () => { return Stats; },
                getLevel = () => { return Level; },
                getClass = () => { return Class; },
                getCurrentBagWeight = () => { return CurrentBagWeight; },
                getInventory = () => { return HInfo.Inventory; },
                getDead = () => { return Dead; },
                getCanRegen = () => { return CanRegen; },
                getRegenTime = () => { return RegenTime; },
                getRegenDelay = () => { return RegenDelay; },
                getHP = () => { return HP; },
                getMP = () => { return MP; },
                getPotTime = () => { return PotTime; },
                getPotDelay = () => { return PotDelay; },
                getPotHealthAmount = () => { return PotHealthAmount; },
                getPotManaAmount = () => { return PotManaAmount; },
                getHealTime = () => { return HealTime; },
                getHealDelay = () => { return HealDelay; },
                getHealAmount = () => { return HealAmount; },
                getVampTime = () => { return VampTime; },
                getVampDelay = () => { return VampDelay; },
                getVampAmount = () => { return VampAmount; },

                //Setters
                setMaxExperience = (value) => { MaxExperience = value; },
                setCurrentBagWeight = (value) => { CurrentBagWeight = value; },
                setRegenTime = (value) => { RegenTime = value; },
                setPotTime = (value) => { PotTime = value; },
                setPotHealthAmount = (value) => { PotHealthAmount = value; },
                setPotManaAmount = (value) => { PotManaAmount = value; },
                setHealTime = (value) => { HealTime = value; },
                setHealAmount = (value) => { HealAmount = value; },
                setVampTime = (value) => { VampTime = value; },
                setVampAmount = (value) => { VampAmount = value; },

                //Functions
                ChangeHP = ChangeHP,
                ChangeMP = ChangeMP,
                BroadcastDamageIndicator = BroadcastDamageIndicator
            };
        }

        public override void Spawned()
        {
            base.Spawned();

            BroadcastManaChange();
        }

        public override void RefreshAll()
        {
            Stats.Clear();

            RefreshLevelStats();
        }

        private void RefreshLevelStats()
        {
            SharedFunctions.RefreshLevelStats();
        }

        public override void RefreshNameColour(bool send = true)
        {
            if (ShockTime < Envir.Time) BindingShotCenter = false;

            Color colour = Color.MediumOrchid;

            if (colour == NameColour || !send) return;

            NameColour = colour;

            Broadcast(new S.ObjectColourChanged { ObjectID = ObjectID, NameColour = NameColour });
        }

        protected override void ProcessRegen()
        {
            SharedFunctions.ProcessRegen();
        }

        public override void BroadcastHealthChange()
        {
            byte time = Math.Min(byte.MaxValue, (byte)Math.Max(5, (RevTime - Envir.Time) / 1000));
            Packet p = new S.ObjectHealth { ObjectID = ObjectID, Percent = PercentHealth, Expire = time };

            if (Envir.Time < RevTime)
            {
                CurrentMap.Broadcast(p, CurrentLocation);
                return;
            }

            PlayerObject player = (PlayerObject)Master;
            player.Enqueue(p);

            if (player.GroupMembers != null)
            {
                for (int i = 0; i < player.GroupMembers.Count; i++)
                {
                    PlayerObject member = player.GroupMembers[i];

                    if (player == member) continue;

                    if (member.CurrentMap != CurrentMap || !Functions.InRange(member.CurrentLocation, CurrentLocation, Globals.DataRange)) continue;
                    member.Enqueue(p);
                }
            }
        }

        public void ChangeMP(int amount)
        {
            if (MP + amount > Stats[Stat.MP])
                amount = Stats[Stat.MP] - MP;

            if (amount == 0) return;

            MP += amount;
            if (MP < 0) MP = 0;

            BroadcastManaChange();
        }

        public byte PercentMana
        {
            get { return (byte)(MP / (float)Stats[Stat.MP] * 100); }
        }

        public void BroadcastManaChange()
        {
            Packet p = new S.ObjectMana { ObjectID = ObjectID, Percent = PercentMana };

            PlayerObject player = (PlayerObject)Master;
            player.Enqueue(p);
        }

        public override Packet GetInfo()
        {
            return new S.ObjectHero
            {
                ObjectID = ObjectID,
                Name = Name,
                OwnerName = Master.Name,
                NameColour = NameColour,
                Class = HInfo.Class,
                Gender = HInfo.Gender,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = HInfo.Hair,
                Weapon = -1,
                Armour = 0,
                Light = Light,
                Poison = CurrentPoison,
                Dead = Dead,
                Hidden = Hidden,
                Effect = SpellEffect.None,
                WingEffect = 0,
                TransformType = -1
            };
        }
    }
}
