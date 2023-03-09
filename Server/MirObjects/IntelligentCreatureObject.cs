using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects
{
    public class IntelligentCreatureObject : MonsterObject
    {
        public bool Summoned;
        public string CustomName { get { return CreatureInfo.CustomName; } set { CreatureInfo.CustomName = value; } }

        public UserIntelligentCreature CreatureInfo;

        public IntelligentCreatureType PetType = IntelligentCreatureType.None;

        public ItemGrade GradeFilter = ItemGrade.None;

        public IntelligentCreatureRules CreatureRules = new IntelligentCreatureRules();
        public IntelligentCreatureItemFilter ItemFilter { get { return CreatureInfo.Filter; } set { CreatureInfo.Filter = value; } }

        public IntelligentCreaturePickupMode CurrentPickupMode { get { return CreatureInfo.petMode; } set { CreatureInfo.petMode = value; } }

        public List<MapObject> TargetList = new List<MapObject>();
        public bool FillingTargetList = false;
        public bool DoTargetList = false;
        public bool TargetListTargetClean = false;

        public int Fullness { get { return CreatureInfo.Fullness; } set { CreatureInfo.Fullness = value; } }
        public long FullnessTicker = 0;
        public const long FullnessDelay = Settings.Second;

        public bool DoDelayedPickup = false;
        public long DelayedpickupTicker = 0;
        public const long DelayedpickupDelay = Settings.Second;//1 second

        public long CreatureTime;

        public long BlackstoneTime { get { return CreatureInfo.BlackstoneTime; } set { CreatureInfo.BlackstoneTime = value; } }
        public const long BlackstoneProduceTime = 10800;//3 hours in seconds

        public long PearlTicker = 0;
        public const long PearlProduceCount = 1000;//1000 items = 1 pearl

        public long AnimvariantTicker = 0;
        public const long AnimvariantDelay = 10 * Settings.Second;//10 seconds

        public long MaintainfoodTime { get { return CreatureInfo.MaintainFoodTime; } set { CreatureInfo.MaintainFoodTime = value; } }

        public long TimedSayTicker = 0;
        public const long TimedSayDelay = 20 * Settings.Second;

        private bool shortcheck = true;

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
            get { return Master == null ? CustomName : (Dead ? CustomName : string.Format("{0}_{1}'s Pet", CustomName, Master.Name)); }
            set { throw new NotSupportedException(); }
        }
        protected override bool CanAttack
        {
            get
            {
                return !Dead && Envir.Time > AttackTime && Envir.Time > ActionTime;
            }
        }

        public override ObjectType Race
        {
            get { return ObjectType.Creature; }
        }


        public IntelligentCreatureObject(MonsterInfo info) : base(info)
        {
            ActionTime = Envir.Time + 1000;
            PetType = (IntelligentCreatureType)info.Effect;
        }

        public override void SetOperateTime()
        {
            base.SetOperateTime();

            var time = OperateTime;

            if (CreatureTime < time && CreatureTime > Envir.Time)
                time = RoamTime;

            if (OperateTime <= Envir.Time || time < OperateTime)
                OperateTime = time;
        }

        public override void Process()
        {
            RefreshNameColour();

            if (Target != null && (Target.CurrentMap != CurrentMap || !Functions.InRange(CurrentLocation, Target.CurrentLocation, Globals.DataRange)))
                Target = null;

            if (Dead && Envir.Time >= DeadTime)
            {
                CurrentMap.RemoveObject(this);
                if (Master != null)
                {
                    Master.Pets.Remove(this);
                    Master = null;
                }

                Despawn();
                return;
            }

            if (Envir.Time > CreatureTime)
            {
                CreatureTime = Envir.Time + Settings.Second;

                ProcessBlackStoneProduction();
                ProcessMaintainFoodBuff();
            }

            if (Fullness == 0)//unable to operate with food level 0
            {
                CreatureTimedSay("I'm starving!!.");
                return;
            }

            DecreaseFullness(1);//Decrease Feeding

            if (DoDelayedPickup && Target != null && DoTargetList)//delayed pickup
            {
                if (Envir.Time > DelayedpickupTicker)
                {
                    PickupAllItems(Target.CurrentLocation);
                    Target = null;
                    DoDelayedPickup = false;

                    if (TargetList.Count > 0)
                    {
                        TargetList.RemoveAt(0);
                    }

                    if (TargetList.Count == 0)
                    {
                        DoTargetList = false;
                        TargetListTargetClean = false;
                        return;
                    }
                }
                else return;
            }

            if (DoTargetList)//Semi-auto | mouse pickup
            {
                ProcessTargetList();
                return;
            }

            if (Target == null)
            {
                shortcheck = true;
                FindTarget();
            }

            if (Target == null) ProcessAI();
            else ProcessTarget();

            ProcessRegen();
        }

        public void ProcessAnimVariant()
        {
            
            if (Envir.Time > AnimvariantTicker)
            {
                AnimvariantTicker = Envir.Time + AnimvariantDelay;
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                switch (PetType)
                {
                    case IntelligentCreatureType.BabyDragon:
                    case IntelligentCreatureType.OlympicFlame:
                        if (Envir.Random.Next(10) > 5)
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                        else
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                        break;
                    case IntelligentCreatureType.BabySnowMan:
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                        break;
                    default:
                        switch(Envir.Random.Next(10))
                        {
                            case 0:
                                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                                break;
                            case 1:
                            case 2:
                            case 3:
                                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                                break;
                            case 4:
                            case 5:
                            case 6:
                                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                                break;
                            case 7:
                            case 8:
                            case 9:
                                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 3 });
                                break;
                        }
                        break;
                }
            }
        }

        public void ProcessBuffs()
        {
            //bool refresh = false;
            //if (refresh) RefreshAll();
        }

        protected override void ProcessAI()
        {
            if (Dead) return;

            if (Master != null)
            {
                if ((Master.PMode == PetMode.Both || Master.PMode == PetMode.MoveOnly))
                {
                    if (!Functions.InRange(CurrentLocation, Master.CurrentLocation, Globals.DataRange) || CurrentMap != Master.CurrentMap)
                    {
                        Target = null;
                        PetRecall();
                    }
                }
            }

            if (Target == null)
            {
                shortcheck = true;
                FindTarget();
            }
            if (Target == null)
            {
                ProcessSearch();
                ProcessRoam();
            }
            ProcessTarget();
        }      
        
        protected override void ProcessSearch()
        {
            if (Target == null)
            {
                shortcheck = true;
                FindTarget();
            }
            if (Target != null) return;
            if (Envir.Time < SearchTime) return;

            SearchTime = Envir.Time + SearchDelay;

            if (Target == null || Envir.Random.Next(3) == 0) FindTarget();
        }
        protected override void ProcessRoam()
        {
            if (Target == null) FindTarget();
            if (Target != null || Envir.Time < RoamTime) return;

            //if (ProcessRoute()) return;
            RoamTime = Envir.Time + 500;

            if (Master != null)
            {
                if (!Functions.InRange(CurrentLocation, Master.CurrentLocation, 2))
                    MoveTo(Functions.PointMove(Master.CurrentLocation,Master.Direction, -2));
                else
                    if (Envir.Random.Next(100) >= 60) ProcessAnimVariant();//random anims
            }
        }
        
        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (Target.CurrentLocation == CurrentLocation || (!CheckAndMoveTo(Target.CurrentLocation) && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1)))
            {
                Attack();

                if (Target == null)
                {
                    shortcheck = true;
                    FindTarget();
                }
                return;
            }
        }
        
        protected override void FindTarget()
        {
            if (Dead) return;

            if (Fullness < CreatureRules.MinimalFullness) return;

            //do automatic pickup/find
            if (CreatureRules.AutoPickupEnabled && CurrentPickupMode == IntelligentCreaturePickupMode.Automatic)
            {
                FindItemTarget();
            }
        }

        private void FindItemTarget()
        {
            if (Master == null) return;

            int range = shortcheck ? 4 : CreatureRules.AutoPickupRange;

            for (int d = 0; d <= range; d++)
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
                            if (ob == null) continue;
                            if (ob.Race != ObjectType.Item) continue;
                            if (ob.Owner != null && ob.Owner != this && ob.Owner != Master && !IsMasterGroupMember(ob.Owner)) continue;

                            ItemObject item = (ItemObject)ob;
                            if (item.Item != null)
                            {
                                if (!((PlayerObject)Master).CanGainItem(item.Item)) continue;
                                if (CheckItemAgainstFilter(item.Item.Info.Type))
                                {
                                    //Master.ReceiveChat("YEAH ITEM I CAN GAIN {" + item.Item.FriendlyName + "} " + item.Item.Info.Type.ToString(), ChatType.System);
                                    if (item.Item.Info.Grade >= ItemFilter.PickupGrade)
                                        Target = ob;
                                    shortcheck = false;
                                    return;
                                }
                            }
                            else
                            {
                                if (item.Gold > 0)
                                {
                                    if (!((PlayerObject)Master).CanGainGold(item.Gold)) continue;
                                    if (ItemFilter.PetPickupAll || ItemFilter.PetPickupGold)
                                    {
                                        //Master.ReceiveChat("YEAH GOLD I CAN GAIN {" + item.Gold + "}", ChatType.System);
                                        Target = ob;
                                        shortcheck = false;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            shortcheck = false;
        }

        private bool FillTargetList(Point location)
        {
            int range = CreatureRules.SemiAutoPickupRange;

            TargetList.Clear();
            for (int d = 0; d <= range; d++)
            {
                for (int y = location.Y - d; y <= location.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = location.X - d; x <= location.X + d; x += Math.Abs(y - location.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (!cell.Valid || cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            if (ob == null) continue;
                            if (ob.Race != ObjectType.Item) continue;
                            if (ob.Owner != null && ob.Owner != this && ob.Owner != Master && !IsMasterGroupMember(ob.Owner)) continue;

                            ItemObject item = (ItemObject)ob;
                            if (item.Item != null)
                            {
                                if (!((PlayerObject)Master).CanGainItem(item.Item)) continue;
                                if (CheckItemAgainstFilter(item.Item.Info.Type))
                                {
                                    if(item.Item.Info.Grade >= ItemFilter.PickupGrade)
                                        TargetList.Add(ob);
                                    break;
                                }
                            }
                            else
                            {
                                if (item.Gold > 0)
                                {
                                    if (!((PlayerObject)Master).CanGainGold(item.Gold)) continue;
                                    if (ItemFilter.PetPickupAll || ItemFilter.PetPickupGold)
                                    {
                                        TargetList.Add(ob);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (TargetList.Count > 0);
        }

        private void ProcessTargetList()
        {
            if (!DoTargetList) return;
            if (!CanAttack) return;
            if (TargetList.Count == 0)
            {
                DoTargetList = false;
                TargetListTargetClean = false;
                return;
            }

            bool remove = false;
            if (TargetList[0] == null) remove = true;
            if (TargetList[0].Race != ObjectType.Item) remove = true;
            if (TargetList[0].Owner != null && TargetList[0].Owner != this && TargetList[0].Owner != Master && !IsMasterGroupMember(TargetList[0].Owner)) remove = true;
            if (remove || TargetListTargetClean || TargetList[0].CurrentMap != CurrentMap)
            {
                TargetList.RemoveAt(0);
                TargetListTargetClean = false;
                ProcessTargetList();//retry
                return;
            }

            Target = TargetList[0];

            if (Target.CurrentLocation == CurrentLocation || (!CheckAndMoveTo(Target.CurrentLocation) && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1)))
            {
                DelayedAttack(500);

                if (Target == null)
                {
                    TargetList.RemoveAt(0);
                    if (TargetList.Count == 0)
                    {
                        DoTargetList = false;
                        TargetListTargetClean = false;
                        return;
                    }
                }
                return;
            }
        }

        private bool CheckAndMoveTo(Point location)
        {
            if (CurrentLocation == location) return true;

            bool inRange = Functions.InRange(location, CurrentLocation, 1);

            if (inRange)
            {
                if (!CurrentMap.ValidPoint(location)) return false;
                Cell cell = CurrentMap.GetCell(location);
                if (cell.Objects != null)
                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if (!ob.Blocking) continue;
                        return false;
                    }
            }

            MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, location);

            if (Walk(dir)) return true;

            switch (Envir.Random.Next(2)) //No favour
            {
                case 0:
                    for (int i = 0; i < 7; i++)
                    {
                        dir = Functions.NextDir(dir);

                        if (Walk(dir)) return true;
                    }
                    break;
                default:
                    for (int i = 0; i < 7; i++)
                    {
                        dir = Functions.PreviousDir(dir);

                        if (Walk(dir)) return true;
                    }
                    break;
            }
            return false;
        }

        protected override void Attack()
        {
            bool singleitem = CurrentPickupMode == IntelligentCreaturePickupMode.SemiAutomatic ? true : false;
            PickupAllItems(Target.CurrentLocation);

            if (Target.CurrentLocation != CurrentLocation)
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            Target = null;

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            DecreaseFullness(1);//use some food for operation
            IncreasePearlProduction();
        }

        private bool DelayedAttack(long delay)
        {
            DelayedPickup(delay);

            if (Target.CurrentLocation != CurrentLocation)
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            DecreaseFullness(1);//use some food for operation
            IncreasePearlProduction();
            return true;
        }

        public void DelayedPickup(long delay)
        {
            DelayedpickupTicker = Envir.Time + delay;
            DoDelayedPickup = true;
        }

        public void PickupAllItems(Point location)
        {
            if (Dead || Master == null) return;

            Cell cell = CurrentMap.GetCell(Target.CurrentLocation);
            if (!cell.Valid || cell.Objects == null) return;


            int count = cell.Objects.Count;

            for (int i = 0; i < count; i++)
            {
                PickUpItem(Target.CurrentLocation);
            }
        }

        public void PickUpItem(Point location)
        {
            if (Dead || Master == null) return;

            Cell cell = CurrentMap.GetCell(location);
            if (!cell.Valid || cell.Objects == null) return;
            for (int i = 0; i < cell.Objects.Count; i++)
            {
                MapObject ob = cell.Objects[i];
                if (ob == null) continue;
                if (ob.Race != ObjectType.Item) continue;
                if (ob.Owner != null && ob.Owner != this && ob.Owner != Master && !IsMasterGroupMember(ob.Owner)) continue;

                ItemObject item = (ItemObject)ob;
                if (item == null) continue;
                if (item.Item != null)
                {
                    if (!((PlayerObject)Master).CanGainItem(item.Item)) continue;

                    if (item.Item.Info.ShowGroupPickup && IsMasterGroupMember(Master))
                        for (int j = 0; j < Master.GroupMembers.Count; j++)
                            Master.GroupMembers[j].ReceiveChat(Name + " Picked up: {" + item.Item.FriendlyName + "}", ChatType.Hint);

                    if (item.Item.Info.Grade == ItemGrade.Mythical || item.Item.Info.Grade == ItemGrade.Legendary || item.Item.Info.Grade == ItemGrade.Heroic)
                    {
                        Master.ReceiveChat("Pet Picked up: {" + item.Item.FriendlyName + "}", ChatType.Hint);
                        ((PlayerObject)Master).Enqueue(new S.IntelligentCreaturePickup { ObjectID = ObjectID });
                    }

                    ((PlayerObject)Master).GainItem(item.Item);
                    CurrentMap.RemoveObject(ob);
                    ob.Despawn();
                    return;
                }
                else
                {
                    if (ob == null) continue;
                    if (!((PlayerObject)Master).CanGainGold(item.Gold)) continue;
                    ((PlayerObject)Master).GainGold(item.Gold);
                    CurrentMap.RemoveObject(ob);
                    ob.Despawn();
                    return;
                }
            }
        }

        private bool CheckItemAgainstFilter(ItemType iType)
        {
            //dont use this method to check for gold.
            //instaid just do a simple check like -->>      if (ItemFilter.PetPickupAll || ItemFilter.PetPickupGold)
            if (ItemFilter.PetPickupAll) return true;

            switch (iType)
            {
                case ItemType.Nothing:// <---- im not sure if any item will ever hold this ItemType but better to prevent then cure
                    return false;
                case ItemType.Weapon:
                    return ItemFilter.PetPickupWeapons;
                case ItemType.Armour:
                    return ItemFilter.PetPickupArmours;
                case ItemType.Helmet:
                    return ItemFilter.PetPickupHelmets;
                case ItemType.Boots:
                    return ItemFilter.PetPickupBoots;
                case ItemType.Belt:
                    return ItemFilter.PetPickupBelts;
                case ItemType.Necklace:
                case ItemType.Bracelet:
                case ItemType.Ring:
                    return ItemFilter.PetPickupAccessories;
                default:
                    return ItemFilter.PetPickupOthers;
            }
        }

        public void ManualPickup(bool mousemode, Point atLocation)
        {
            if (Fullness < CreatureRules.MinimalFullness) return;

            if (CreatureRules.MousePickupEnabled && mousemode)
            {
                FillTargetList(atLocation);
                DoTargetList = true;
                return;
            }

            if (!mousemode && CreatureRules.SemiAutoPickupEnabled && CurrentPickupMode == IntelligentCreaturePickupMode.SemiAutomatic)
            {
                FillTargetList(CurrentLocation);
                DoTargetList = true;
                return;
            }
        }

        public void IncreaseFullness(int amount)
        {
            if (Fullness >= 10000) return;
            FullnessTicker = Envir.Time + FullnessDelay;
            Fullness += amount;
            if (Fullness < CreatureRules.MinimalFullness) CreatureSay("*Hmmm*");
            else CreatureSay("*Burp*");
            if (Fullness > 10000) Fullness = 10000;
        }

        public void DecreaseFullness(int amount)
        {
            if (Fullness <= 0 || MaintainfoodTime > 0) return;

            if (Envir.Time > FullnessTicker)
            {
                FullnessTicker = Envir.Time + FullnessDelay;
                Fullness -= amount;
                if (Fullness < 0) Fullness = 0;
                if (Fullness < CreatureRules.MinimalFullness) CreatureTimedSay("*Me Hungry*");
            }
        }

        public void IncreasePearlProduction()
        {
            PearlTicker++;

            if (PearlTicker >= PearlProduceCount)
            {
                if (Master != null)
                {
                    ((PlayerObject)Master).IntelligentCreatureProducePearl();
                }

                PearlTicker = 0;
            }
        }

        public void ProcessBlackStoneProduction()
        {
            if (!CreatureRules.CanProduceBlackStone) return;

            BlackstoneTime++;

            if (BlackstoneTime >= BlackstoneProduceTime)
            {
                if (Master != null)
                {
                    ((PlayerObject)Master).IntelligentCreatureProduceBlackStone();
                }

                BlackstoneTime = 0;
            }
        }

        public void ProcessMaintainFoodBuff()
        {
            if (MaintainfoodTime > 0)
            {
                MaintainfoodTime--;

                if (MaintainfoodTime < 0)
                {
                    MaintainfoodTime = 0;
                }
            }
        }

        public void CreatureTimedSay(string message)
        {
            if (Envir.Time > TimedSayTicker)
            {
                CreatureSay(message);
                TimedSayTicker = Envir.Time + TimedSayDelay;
            }
        }

        public void CreatureSay(string message)
        {
            if (Master != null)
            {
                message = String.Format("{0}:{1}", CustomName, message);
                ((PlayerObject)Master).IntelligentCreatureSay(PetType, message);
            }
        }

        public override void ReceiveChat(string text, ChatType type)
        {
            if (type == ChatType.WhisperIn) CreatureSay("What?");
        }

        public override bool IsAttackTarget(HumanObject attacker)
        {
            return false;
        }
        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return false;
        }
        public override bool IsFriendlyTarget(HumanObject ally)
        {
            return true;
        }
        public override bool IsFriendlyTarget(MonsterObject ally)
        {
            return true;
        }
        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            return 0;
        }
        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            return 0;
        }

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            return 0;
        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true)
        {
            //FindTarget();
        }

        private bool IsMasterGroupMember(MapObject player)
        {
            if (player.Race != ObjectType.Player || Master == null) return false;
            return ((PlayerObject)Master).GroupMembers != null && ((PlayerObject)Master).GroupMembers.Contains((PlayerObject)player);
        }


        public override void Spawned()
        {
            base.Spawned();
            Summoned = true;
            FullnessTicker = Envir.Time + FullnessDelay;
            AnimvariantTicker = Envir.Time + AnimvariantDelay;
        }

        public override void Die()
        {
            if (Dead) return;

            base.Die();

            if (Dead)
            {
                CurrentMap.RemoveObject(this);
                Despawn();
                return;
            }
        }

        public override Packet GetInfo()
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Location = CurrentLocation,
                Image = Info.Image,
                Direction = Direction,
                Effect = Info.Effect,
                AI = Info.AI,
                Light = Info.Light,
                Dead = Dead,
                Skeleton = Harvested,
                Poison = CurrentPoison,
                Hidden = Hidden,
                Extra = Summoned,
            };
        }
    }
}
