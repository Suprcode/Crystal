using System;
using System.Collections.Generic;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects.Monsters;
using S = ServerPackets;
using System.IO;
using System.Linq;

namespace Server.MirObjects
{
    public abstract class MapObject
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public readonly uint ObjectID = SMain.Envir.ObjectID;

        public abstract ObjectType Race { get; }

        public abstract string Name { get; set; }

        public long ExplosionInflictedTime;
        public int ExplosionInflictedStage;

        private int SpawnThread;

        //Position
        private Map _currentMap;
        public Map CurrentMap
        {
            set
            {
                _currentMap = value;
                CurrentMapIndex = _currentMap != null ? _currentMap.Info.Index : 0;
            }
            get { return _currentMap; }
        }

        public abstract int CurrentMapIndex { get; set; }
        public abstract Point CurrentLocation { get; set; }
        public abstract MirDirection Direction { get; set; }

        public abstract ushort Level { get; set; }

        public abstract uint Health { get; }
        public abstract uint MaxHealth { get; }
        public byte PercentHealth
        {
            get { return (byte) (Health/(float) MaxHealth*100); }

        }

        public ushort MinAC, MaxAC, MinMAC, MaxMAC;
        public ushort MinDC, MaxDC, MinMC, MaxMC, MinSC, MaxSC;

        public byte Accuracy, Agility, Light;
        public sbyte ASpeed, Luck;
        public int AttackSpeed;

        public ushort CurrentHandWeight,
                   MaxHandWeight,
                   CurrentWearWeight,
                   MaxWearWeight;

        public ushort CurrentBagWeight,
                      MaxBagWeight;

        public byte MagicResist, PoisonResist, HealthRecovery, SpellRecovery, PoisonRecovery, CriticalRate, CriticalDamage, Holy, Freezing, PoisonAttack;

        public long CellTime, BrownTime, PKPointTime, LastHitTime, EXPOwnerTime;
        public Color NameColour = Color.White;
        
        public bool Dead, Undead, Harvested, AutoRev;

        public List<KeyValuePair<string, string>> NPCVar = new List<KeyValuePair<string, string>>();

        public virtual int PKPoints { get; set; }

        public ushort PotHealthAmount, PotManaAmount, HealAmount, VampAmount;
        //public bool HealthChanged;

        public float ItemDropRateOffset = 0, GoldDropRateOffset = 0;

        public bool CoolEye;
        private bool _hidden;
        
        public bool Hidden
        {
            get
            {
                return _hidden;
            }
            set
            {
                if (_hidden == value) return;
                _hidden = value;
                CurrentMap.Broadcast(new S.ObjectHidden {ObjectID = ObjectID, Hidden = value}, CurrentLocation);
            }
        }

        private bool _observer;
        public bool Observer
        {
            get
            {
                return _observer;
            }
            set
            {
                if (_observer == value) return;
                _observer = value;
                if (!_observer)
                    BroadcastInfo();
                else
                    Broadcast(new S.ObjectRemove { ObjectID = ObjectID });
            }
        }

        #region Sneaking
        private bool _sneakingActive;
        public bool SneakingActive
        {
            get { return _sneakingActive; }
            set
            {
                if (_sneakingActive == value) return;
                _sneakingActive = value;

                Observer = _sneakingActive;

                //CurrentMap.Broadcast(new S.ObjectSneaking { ObjectID = ObjectID, SneakingActive = value }, CurrentLocation);
            }
        }

        private bool _sneaking;
        public bool Sneaking
        {
            get { return _sneaking; }
            set { _sneaking = value; SneakingActive = value; }
        }
        #endregion

        public MapObject _target;
        public virtual MapObject Target
        {
            get { return _target; }
            set
            {
                if (_target == value) return;
                _target = value;
            }

        }

        public MapObject Master, LastHitter, EXPOwner, Owner;
        public long ExpireTime, OwnerTime, OperateTime;
        public int OperateDelay = 100;

        public List<MonsterObject> Pets = new List<MonsterObject>();
        public List<Buff> Buffs = new List<Buff>();

        public List<PlayerObject> GroupMembers;

        public virtual AttackMode AMode { get; set; }

        public virtual PetMode PMode { get; set; }
        public bool InSafeZone;

        public float ArmourRate, DamageRate; //recieved not given

        public List<Poison> PoisonList = new List<Poison>();
        public PoisonType CurrentPoison = PoisonType.None;
        public List<DelayedAction> ActionList = new List<DelayedAction>();

        public LinkedListNode<MapObject> Node;
        public LinkedListNode<MapObject> NodeThreaded;
        public long RevTime;

        public virtual bool Blocking
        {
            get { return true; }
        }

        public Point Front
        {
            get { return Functions.PointMove(CurrentLocation, Direction, 1); }

        }
        public Point Back
        {
            get { return Functions.PointMove(CurrentLocation, Direction, -1); }

        }
        
        
        public virtual void Process()
        {
            if (Master != null && Master.Node == null) Master = null;
            if (LastHitter != null && LastHitter.Node == null) LastHitter = null;
            if (EXPOwner != null && EXPOwner.Node == null) EXPOwner = null;
            if (Target != null && (Target.Node == null || Target.Dead)) Target = null;
            if (Owner != null && Owner.Node == null) Owner = null;

            if (Envir.Time > PKPointTime && PKPoints > 0)
            {
                PKPointTime = Envir.Time + Settings.PKDelay * Settings.Second;
                PKPoints--;
            }
            
            if (LastHitter != null && Envir.Time > LastHitTime)
                LastHitter = null;


            if (EXPOwner != null && Envir.Time > EXPOwnerTime)
            {
                EXPOwner = null;
            }

            for (int i = 0; i < ActionList.Count; i++)
            {
                if (Envir.Time < ActionList[i].Time) continue;
                Process(ActionList[i]);
                ActionList.RemoveAt(i);
            }
        }

        public abstract void SetOperateTime();

        public int GetAttackPower(int min, int max)
        {
            if (min < 0) min = 0;
            if (min > max) max = min;

            if (Luck > 0)
            {
                if (Luck > Envir.Random.Next(Settings.MaxLuck))
                    return max;
            }
            else if (Luck < 0)
            {
                if (Luck < -Envir.Random.Next(Settings.MaxLuck))
                    return min;
            }

            return Envir.Random.Next(min, max + 1);
        }

        public int GetDefencePower(int min, int max)
        {
            if (min < 0) min = 0;
            if (min > max) max = min;

            return Envir.Random.Next(min, max + 1);
        }

        public virtual void Remove(PlayerObject player)
        {
            player.Enqueue(new S.ObjectRemove {ObjectID = ObjectID});
        }
        public virtual void Add(PlayerObject player)
        {
            if (Race == ObjectType.Merchant)
            {
                NPCObject NPC = (NPCObject)this;
                NPC.CheckVisible(player, true);
                return;
            }

            player.Enqueue(GetInfo());

            //if (Race == ObjectType.Player)
            //{
            //    PlayerObject me = (PlayerObject)this;
            //    player.Enqueue(me.GetInfoEx(player));
            //}
            //else
            //{
            //    player.Enqueue(GetInfo());
            //}
        }
        public virtual void Remove(MonsterObject monster)
        {

        }
        public virtual void Add(MonsterObject monster)
        {

        }

        public abstract void Process(DelayedAction action);


        public bool CanFly(Point target)
        {
            Point location = CurrentLocation;
            while (location != target)
            {
                MirDirection dir = Functions.DirectionFromPoint(location, target);

                location = Functions.PointMove(location, dir, 1);

                if (location.X < 0 || location.Y < 0 || location.X >= CurrentMap.Width || location.Y >= CurrentMap.Height) return false;

                if (!CurrentMap.GetCell(location).Valid) return false;

            }

            return true;
        }

        public virtual void Spawned()
        {
            Node = Envir.Objects.AddLast(this);
            if ((Race == ObjectType.Monster) && Settings.Multithreaded)
            {
                SpawnThread = CurrentMap.Thread;
                NodeThreaded = Envir.MobThreads[SpawnThread].ObjectsList.AddLast(this);
            }
            OperateTime = Envir.Time + Envir.Random.Next(OperateDelay);

            InSafeZone = CurrentMap != null && CurrentMap.GetSafeZone(CurrentLocation) != null;
            BroadcastInfo();
            BroadcastHealthChange();
        }
        public virtual void Despawn()
        {
            Broadcast(new S.ObjectRemove {ObjectID = ObjectID});
            Envir.Objects.Remove(Node);
            if (Settings.Multithreaded && (Race == ObjectType.Monster))
            {
                Envir.MobThreads[SpawnThread].ObjectsList.Remove(NodeThreaded);
            }            

            ActionList.Clear();

            for (int i = Pets.Count - 1; i >= 0; i--)
                Pets[i].Die();

            Node = null;
        }

        public MapObject FindObject(uint targetID, int dist)
        {
            for (int d = 0; d <= dist; d++)
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

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            if (ob.ObjectID != targetID) continue;

                            return ob;
                        }
                    }
                }
            }
            return null;
        }


        public virtual void Broadcast(Packet p)
        {
            if (p == null || CurrentMap == null) return;

            for (int i = CurrentMap.Players.Count - 1; i >= 0; i--)
            {
                PlayerObject player = CurrentMap.Players[i];
                if (player == this) continue;

                if (Functions.InRange(CurrentLocation, player.CurrentLocation, Globals.DataRange))
                    player.Enqueue(p);
            }
        }

        public virtual void BroadcastInfo()
        {
            Broadcast(GetInfo());
            return;
        } 

        public abstract bool IsAttackTarget(PlayerObject attacker);
        public abstract bool IsAttackTarget(MonsterObject attacker);
        public abstract int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true);
        public abstract int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility);

        public abstract int Struck(int damage, DefenceType type = DefenceType.ACAgility);

        public abstract bool IsFriendlyTarget(PlayerObject ally);
        public abstract bool IsFriendlyTarget(MonsterObject ally);

        public abstract void ReceiveChat(string text, ChatType type);

        public abstract Packet GetInfo();

        public virtual void WinExp(uint amount, uint targetLevel = 0)
        {


        }

        public virtual bool CanGainGold(uint gold)
        {
            return false;
        }
        public virtual void WinGold(uint gold)
        {

        }

        public virtual bool Harvest(PlayerObject player) { return false; }

        public abstract void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true);
        public virtual void AddBuff(Buff b)
        {
            switch (b.Type)
            {
                case BuffType.MoonLight:
                case BuffType.Hiding:
                case BuffType.DarkBody:
                    Hidden = true;

                    if (b.Type == BuffType.MoonLight || b.Type == BuffType.DarkBody) Sneaking = true;

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
                                if (ob.Race != ObjectType.Monster) continue;

                                if (ob.Target == this && (!ob.CoolEye || ob.Level < Level)) ob.Target = null;
                            }
                        }
                    }
                    break;
            }


            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].Type != b.Type) continue;

                Buffs[i] = b;
                Buffs[i].Paused = false;
                return;
            }

            Buffs.Add(b);
        }
        public void RemoveBuff(BuffType b)
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].Type != b) continue;

                Buffs[i].Infinite = false;
                Buffs[i].ExpireTime = Envir.Time;
            }
        }

        public bool CheckStacked()
        {
            Cell cell = CurrentMap.GetCell(CurrentLocation);

            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (ob == this || !ob.Blocking) continue;
                    return true;
                }

            return false;
        }

        public virtual bool Teleport(Map temp, Point location, bool effects = true, byte effectnumber = 0)
        {
            if (temp == null || !temp.ValidPoint(location)) return false;

            CurrentMap.RemoveObject(this);
            if (effects) Broadcast(new S.ObjectTeleportOut {ObjectID = ObjectID, Type = effectnumber});
            Broadcast(new S.ObjectRemove {ObjectID = ObjectID});
            
            CurrentMap = temp;
            CurrentLocation = location;

            InTrapRock = false;

            CurrentMap.AddObject(this);
            BroadcastInfo();

            if (effects) Broadcast(new S.ObjectTeleportIn { ObjectID = ObjectID, Type = effectnumber });
            
            BroadcastHealthChange();
            
            return true;
        }

        public virtual bool TeleportRandom(int attempts, int distance, Map map = null)
        {
            if (map == null) map = CurrentMap;
            if (map.Cells == null) return false;
            if (map.WalkableCells != null && map.WalkableCells.Count == 0) return false;

            if (map.WalkableCells == null)
            {
                map.WalkableCells = new List<Point>();

                for (int x = 0; x < map.Width; x++)
                    for (int y = 0; y < map.Height; y++)
                        if (map.Cells[x, y].Attribute == CellAttribute.Walk)
                            map.WalkableCells.Add(new Point(x, y));

                if (map.WalkableCells.Count == 0) return false;
            }

            int cellIndex = Envir.Random.Next(map.WalkableCells.Count);

            return Teleport(map, map.WalkableCells[cellIndex]);
        }

        public Point GetRandomPoint(int attempts, int distance, Map map)
        {
            byte edgeoffset = 0;

            if (map.Width < 150)
            {
                if (map.Height < 30) edgeoffset = 2;
                else edgeoffset = 20;
            }

            for (int i = 0; i < attempts; i++)
            {
                Point location;

                if (distance <= 0)
                    location = new Point(edgeoffset + Envir.Random.Next(map.Width - edgeoffset), edgeoffset + Envir.Random.Next(map.Height - edgeoffset)); //Can adjust Random Range...
                else
                    location = new Point(CurrentLocation.X + Envir.Random.Next(-distance, distance + 1),
                                         CurrentLocation.Y + Envir.Random.Next(-distance, distance + 1));


                if (map.ValidPoint(location)) return location;
            }

            return new Point(0, 0);
        }

        public void BroadcastHealthChange()
        {
            if (Race != ObjectType.Player && Race != ObjectType.Monster) return;

            byte time = Math.Min(byte.MaxValue, (byte)Math.Max(5, (RevTime - Envir.Time) / 1000));
            Packet p = new S.ObjectHealth { ObjectID = ObjectID, Percent = PercentHealth, Expire = time };

            if (Envir.Time < RevTime)
            {
                CurrentMap.Broadcast(p, CurrentLocation);
                return;
            }

            if (Race == ObjectType.Monster && !AutoRev && Master == null) return;

            if (Race == ObjectType.Player)
            {
                if (GroupMembers != null) //Send HP to group
                {
                    for (int i = 0; i < GroupMembers.Count; i++)
                    {
                        PlayerObject member = GroupMembers[i];

                        if (this == member) continue;
                        if (member.CurrentMap != CurrentMap || !Functions.InRange(member.CurrentLocation, CurrentLocation, Globals.DataRange)) continue;
                        member.Enqueue(p);
                    }
                }

                return;
            }

            if (Master != null && Master.Race == ObjectType.Player)
            {
                PlayerObject player = (PlayerObject)Master;

                player.Enqueue(p);

                if (player.GroupMembers != null) //Send pet HP to group
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


            if (EXPOwner != null && EXPOwner.Race == ObjectType.Player)
            {
                PlayerObject player = (PlayerObject)EXPOwner;

                if (player.IsMember(Master)) return;
                
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

        }

        public void BroadcastDamageIndicator(DamageType type, int damage = 0)
        {
            Packet p = new S.DamageIndicator { ObjectID = ObjectID, Damage = damage, Type = type };

            if (Race == ObjectType.Player)
            {
                PlayerObject player = (PlayerObject)this;
                player.Enqueue(p);
            }
            Broadcast(p);
        }

        public abstract void Die();
        public abstract int Pushed(MapObject pusher, MirDirection dir, int distance);

        public bool IsMember(MapObject member)
        {
            if (member == this) return true;
            if (GroupMembers == null || member == null) return false;

            for (int i = 0; i < GroupMembers.Count; i++)
                if (GroupMembers[i] == member) return true;

            return false;
        }

        public abstract void SendHealth(PlayerObject player);

        public bool InTrapRock
        {
            set
            {
                if (this is PlayerObject)
                {
                    PlayerObject player = (PlayerObject)this;
                    player.Enqueue(new S.InTrapRock { Trapped = value });
                }
            }
            get
            {
                Point checklocation;

                for (int i = 0; i <= 6; i += 2)
                {
                    checklocation = Functions.PointMove(CurrentLocation, (MirDirection)i, 1);

                    if (checklocation.X < 0) continue;
                    if (checklocation.X >= CurrentMap.Width) continue;
                    if (checklocation.Y < 0) continue;
                    if (checklocation.Y >= CurrentMap.Height) continue;

                    Cell cell = CurrentMap.GetCell(checklocation.X, checklocation.Y);
                    if (!cell.Valid || cell.Objects == null) continue;

                    for (int j = 0; j < cell.Objects.Count; j++)
                    {
                        MapObject ob = cell.Objects[j];
                        switch (ob.Race)
                        {
                            case ObjectType.Monster:
                                if (ob is TrapRock)
                                {
                                    TrapRock rock = (TrapRock)ob;
                                    if (rock.Dead) continue;
                                    if (rock.Target != this) continue;
                                    if (!rock.Visible) continue;
                                }
                                else continue;

                                return true;
                            default:
                                continue;
                        }
                    }
                }
                return false;
            }
        }

    }

    public class Poison
    {
        public MapObject Owner;
        public PoisonType PType;
        public int Value;
        public long Duration, Time, TickTime, TickSpeed;

        public Poison() { }

        public Poison(BinaryReader reader)
        {
            Owner = null;
            PType = (PoisonType)reader.ReadByte();
            Value = reader.ReadInt32();
            Duration = reader.ReadInt64();
            Time = reader.ReadInt64();
            TickTime = reader.ReadInt64();
            TickSpeed = reader.ReadInt64();
        }
    }

    public class Buff
    {
        public BuffType Type;
        public MapObject Caster;
        public bool Visible;
        public uint ObjectID;
        public long ExpireTime;
        public int[] Values;
        public bool Infinite;

        public bool RealTime;
        public DateTime RealTimeExpire;

        public bool Paused;

        public Buff() { }

        public Buff(BinaryReader reader)
        {
            Type = (BuffType)reader.ReadByte();
            Caster = null;
            Visible = reader.ReadBoolean();
            ObjectID = reader.ReadUInt32();
            ExpireTime = reader.ReadInt64();

            if (Envir.LoadVersion < 56)
            {
                Values = new int[] { reader.ReadInt32() };
            }
            else
            {
                Values = new int[reader.ReadInt32()];

                for (int i = 0; i < Values.Length; i++)
                {
                    Values[i] = reader.ReadInt32();
                }
            }

            Infinite = reader.ReadBoolean();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write(Visible);
            writer.Write(ObjectID);
            writer.Write(ExpireTime);

            writer.Write(Values.Length);
            for (int i = 0; i < Values.Length; i++)
            {
                writer.Write(Values[i]);
            }

            writer.Write(Infinite);
        }
    }
}
