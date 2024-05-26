using Server.Library.MirEnvir;
using Server.Library.MirObjects;
using Shared;
using Shared.Data;
using Shared.Functions;

namespace Server.Library.MirDatabase {
    public class BuffInfo {
        public BuffType Type { get; set; }
        public BuffStackType StackType { get; set; }
        public BuffProperty Properties { get; set; }
        public int Icon { get; set; }
        public bool Visible { get; set; }

        public static List<BuffInfo> Load() {
            List<BuffInfo> info = new() {
                //Magics
                new() {
                    Type = BuffType.TemporalFlux, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.Hiding, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.Haste, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.SwiftFeet, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration,
                    Visible = true
                },
                new() {
                    Type = BuffType.Fury, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration,
                    Visible = true
                },
                new() {
                    Type = BuffType.SoulShield, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.BlessedArmour, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.LightBody, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.UltimateEnhancer, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetStatAndDuration
                },
                new() {
                    Type = BuffType.ProtectionField, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration
                },
                new() { Type = BuffType.Rage, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new() {
                    Type = BuffType.Curse, Properties = BuffProperty.RemoveOnDeath | BuffProperty.Debuff,
                    StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.MoonLight, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration,
                    Visible = true
                },
                new() {
                    Type = BuffType.DarkBody, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration,
                    Visible = true
                },
                new() {
                    Type = BuffType.Concentration, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.VampireShot, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.PoisonShot, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration,
                    Visible = true
                },
                new() {
                    Type = BuffType.CounterAttack, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.MentalState, Properties = BuffProperty.None, StackType = BuffStackType.Infinite
                },
                new() {
                    Type = BuffType.EnergyShield, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.MagicBooster, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.PetEnhancer, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.ImmortalSkin, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.MagicShield, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.ElementalBarrier, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration
                },

                //Monsters
                new() {
                    Type = BuffType.HornedArcherBuff, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.ColdArcherBuff, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.GeneralMeowMeowShield, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.RhinoPriestDebuff, Properties = BuffProperty.Debuff,
                    StackType = BuffStackType.ResetDuration
                },
                new() {
                    Type = BuffType.PowerBeadBuff, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.HornedWarriorShield, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.HornedCommanderShield, Properties = BuffProperty.None,
                    StackType = BuffStackType.ResetDuration, Visible = true
                },
                new() {
                    Type = BuffType.Blindness, Properties = BuffProperty.RemoveOnDeath | BuffProperty.Debuff,
                    StackType = BuffStackType.ResetDuration
                },

                //Special
                new() {
                    Type = BuffType.GameMaster, Properties = BuffProperty.None, StackType = BuffStackType.Infinite,
                    Visible = Settings.GameMasterEffect
                },
                new() { Type = BuffType.Mentee, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new() { Type = BuffType.Mentor, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new() { Type = BuffType.Guild, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new() { Type = BuffType.Skill, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new() { Type = BuffType.ClearRing, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new() { Type = BuffType.Transform, Properties = BuffProperty.None, StackType = BuffStackType.None },
                new() {
                    Type = BuffType.Lover, Properties = BuffProperty.RemoveOnExit, StackType = BuffStackType.Infinite
                },
                new() {
                    Type = BuffType.Rested, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration
                },
                new() { Type = BuffType.Prison, Properties = BuffProperty.None, StackType = BuffStackType.None }, //???
                new() { Type = BuffType.General, Properties = BuffProperty.None, StackType = BuffStackType.None }, //???

                //Stats
                new() {
                    Type = BuffType.Exp, Properties = BuffProperty.PauseInSafeZone,
                    StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.Drop, Properties = BuffProperty.PauseInSafeZone,
                    StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.Gold, Properties = BuffProperty.PauseInSafeZone,
                    StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.BagWeight, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.Impact, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.Magic, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.Taoist, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.Storm, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.HealthAid, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.ManaAid, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.Defence, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.MagicDefence, Properties = BuffProperty.None,
                    StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.WonderDrug, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                },
                new() {
                    Type = BuffType.Knapsack, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration
                }
            };

            return info;
        }
    }

    public class Buff {
        protected static Envir Envir => Envir.Main;

        private Dictionary<string, object> Data { get; set; } = new();

        public BuffInfo Info;
        public MapObject Caster;
        public uint ObjectID;
        public long ExpireTime;

        public long LastTime, NextTime;

        public Stats Stats;

        public int[] Values;

        public bool FlagForRemoval;
        public bool Paused;

        public BuffType Type => Info.Type;

        public BuffStackType StackType => Info.StackType;

        public BuffProperty Properties => Info.Properties;

        public Buff(BuffType type) {
            Info = Envir.GetBuffInfo(type);
            Stats = new Stats();
            Data = new Dictionary<string, object>();
        }

        public Buff(BinaryReader reader, int version, int customVersion) {
            BuffType type = (BuffType)reader.ReadByte();

            Info = Envir.GetBuffInfo(type);

            Caster = null;

            if(version < 88) {
                bool visible = reader.ReadBoolean();
            }

            ObjectID = reader.ReadUInt32();
            ExpireTime = reader.ReadInt64();

            if(version <= 84) {
                Values = new int[reader.ReadInt32()];

                for (int i = 0; i < Values.Length; i++) {
                    Values[i] = reader.ReadInt32();
                }

                if(version < 88) {
                    bool infinite = reader.ReadBoolean();
                }

                Stats = new Stats();
                Data = new Dictionary<string, object>();
            } else {
                if(version < 88) {
                    bool stackable = reader.ReadBoolean();
                }

                Values = new int[0];
                Stats = new Stats(reader, version, customVersion);
                Data = new Dictionary<string, object>();

                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++) {
                    string key = reader.ReadString();
                    int length = reader.ReadInt32();

                    byte[] array = new byte[length];

                    for (int j = 0; j < array.Length; j++) {
                        array[j] = reader.ReadByte();
                    }

                    Data[key] = Functions.DeserializeFromBytes(array);
                }

                if(version > 86) {
                    count = reader.ReadInt32();

                    Values = new int[count];

                    for (int i = 0; i < count; i++) {
                        Values[i] = reader.ReadInt32();
                    }
                }
            }
        }

        public void Save(BinaryWriter writer) {
            writer.Write((byte)Type);
            writer.Write(ObjectID);
            writer.Write(ExpireTime);

            Stats.Save(writer);

            writer.Write(Data.Count);

            foreach(KeyValuePair<string, object> pair in Data) {
                byte[] bytes = Functions.SerializeToBytes(pair.Value);

                writer.Write(pair.Key);
                writer.Write(bytes.Length);

                for (int i = 0; i < bytes.Length; i++) {
                    writer.Write(bytes[i]);
                }
            }

            writer.Write(Values.Length);

            for (int i = 0; i < Values.Length; i++) {
                writer.Write(Values[i]);
            }
        }

        public T Get<T>(string key) {
            if(!Data.TryGetValue(key, out object result)) {
                return default;
            }

            return (T)result;
        }

        public void Set(string key, object val) {
            Data[key] = val;
        }

        public ClientBuff ToClientBuff() {
            return new ClientBuff {
                Type = Type,
                Caster = Caster?.Name ?? "",
                ObjectID = ObjectID,
                Visible = Info.Visible,
                Infinite = StackType == BuffStackType.Infinite,
                Paused = Paused,
                ExpireTime = ExpireTime,
                Stats = new Stats(Stats),
                Values = Values
            };
        }
    }
}
