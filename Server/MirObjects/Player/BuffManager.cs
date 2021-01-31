using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using S = ServerPackets;

namespace Server.Library.MirObjects
{
    public class BuffManager
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        protected MapObject Owner { get; set; }

        protected List<Buff> Buffs = new List<Buff>();

        public BuffManager(MapObject owner)
        {
            Owner = owner;
        }

        public virtual void Process()
        {
            foreach (var buff in Buffs)
            {
                
            }
        }

        public IEnumerable<Buff> GetBuffs(BuffType type)
        {
            var buffs = Buffs.Where(x => x.Info.Type == type);

            return buffs;
        }

        public void ExpireBuff(BuffType type)
        {
            var buffs = GetBuffs(type);

            foreach (var buff in buffs)
            {
                buff.ExpireTime = 0;
            }
        }

        public void AddBuff(BuffType types)
        {
            var buffs = GetBuffs(types);

            if (buffs.Count() > 0 && buffs.First().Info.CanStack == false) return;

            Buff buff = CreateBuff(types);
            

            string caster = buff.Caster != null ? buff.Caster.Name : string.Empty;

            //if (buff.Values == null) buff.Values = new int[1];

            S.AddBuff addBuff = new S.AddBuff { Type = buff.Info.Type };

            if (buff.Info.Visible) Owner.Broadcast(addBuff);

            if (Owner.Race == ObjectType.Player)
            {
                ((PlayerObject)Owner).Enqueue(addBuff);
                ((PlayerObject)Owner).RefreshStats();
            }
        }

        private Buff CreateBuff(BuffType buff)
        {
            var b = new Buff
            {
                Info = BuffInfo.Buffs.Single(x => x.Type == buff)
            };

            return b;
        }

        public void RemoveBuff(BuffType buff)
        {
            
        }

        public void RefreshBuffs()
        {
            foreach (var buff in Buffs)
            {
                foreach (var kv in buff.Info.Values)
                {
                    var key = kv.Key;
                    var value = kv.Value;

                    switch (key)
                    {
                        case "ASpeed":
                           // Owner.ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, Owner.ASpeed + Convert.ToSByte(value))));
                            break;
                        case "MinAC":
                           // Owner.MinAC = (ushort)Math.Min(ushort.MaxValue, (Math.Min(ushort.MaxValue, Owner.MinAC + Convert.ToUInt16(value))));
                            break;
                        case "MaxAC":
                           // Owner.MaxAC = (ushort)Math.Min(ushort.MaxValue, (Math.Min(ushort.MaxValue, Owner.MaxAC + Convert.ToUInt16(value))));
                            break;
                    }
                }

                //switch (buff.Info.Type)
                //{
                //    case BuffType.Haste:
                //    case BuffType.Fury:
                //        Owner.ASpeed = (sbyte)Math.Max(sbyte.MinValue, (Math.Min(sbyte.MaxValue, Owner.ASpeed + buff.Values[0])));
                //        break;
                //    case BuffType.ImmortalSkin:
                //        Owner.MaxAC = (ushort)Math.Min(ushort.MaxValue, Owner.MaxAC + buff.Values[0]);
                //        Owner.MaxDC = (ushort)Math.Max(ushort.MinValue, Owner.MaxDC - buff.Values[1]);
                //        break;
                //}
            }        
        }
    }

    public class Buff
    {
        public MapObject Caster;
        public uint ObjectID;
        public long ExpireTime;
        public bool RealTime;
        public DateTime RealTimeExpire;

        public bool Paused;

        public BuffInfo Info;
    }

    //TemporalFlux,
    //Hiding,
    //Haste,
    //SwiftFeet,
    //Fury,
    //SoulShield,
    //BlessedArmour,
    //LightBody,
    //UltimateEnhancer,
    //ProtectionField,
    //Rage,
    //Curse,
    //MoonLight,
    //DarkBody,
    //Concentration,
    //VampireShot,
    //PoisonShot,
    //CounterAttack,
    //MentalState,
    //EnergyShield,
    //MagicBooster,
    //PetEnhancer,
    //ImmortalSkin,
    //MagicShield,

    ////special
    //GameMaster = 100,
    //General,
    //Exp,
    //Drop,
    //Gold,
    //BagWeight,
    //Transform,
    //RelationshipEXP,
    //Mentee,
    //Mentor,
    //Guild,
    //Prison,
    //Rested,

    ////stats
    //Impact = 200,
    //Magic,
    //Taoist,
    //Storm,
    //HealthAid,
    //ManaAid,
    //Defence,
    //MagicDefence,
    //WonderDrug,
    //Knapsack

    //MaxHealth
    //    public ushort MinAC, MaxAC, MinMAC, MaxMAC;
    //public ushort MinDC, MaxDC, MinMC, MaxMC, MinSC, MaxSC;

    //public byte Accuracy, Agility, Light;
    //public sbyte ASpeed, Luck;
    //public int AttackSpeed;

    //public ushort ,
    //           MaxHandWeight,
    //           ,
    //           MaxWearWeight;

    //public ushort ,
    //              MaxBagWeight;

    //public byte MagicResist, PoisonResist, HealthRecovery, SpellRecovery, PoisonRecovery, CriticalRate, CriticalDamage, Holy, Freezing, PoisonAttack;

    //public virtual int PKPoints { get; set; }

    //public float ItemDropRateOffset = 0, GoldDropRateOffset = 0, ExpRateOffset = 0;
}
