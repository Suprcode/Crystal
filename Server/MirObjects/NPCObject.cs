using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects
{
    public sealed class NPCObject : MapObject
    {
        public override ObjectType Race
        {
            get { return ObjectType.Merchant; }
        }

        public static NPCObject Get(uint objectID)
        {
            if (objectID == 0) return null;

            var obj = Envir.NPCs.SingleOrDefault(x => x.ObjectID == objectID);

            if (obj != null && obj is NPCObject)
            {
                return obj as NPCObject;
            }

            return null;
        }

        public int ScriptID { get; set; }

        public NPCInfo Info;
        private const long TurnDelay = 10000, SpeechDelay = 5000;
        public long TurnTime, UsedGoodsTime, VisTime, SpeechTime;
        public bool Visible = true;

        public Dictionary<int, bool> VisibleLog = new Dictionary<int, bool>();

        public ConquestObject Conq;
        public List<QuestInfo> Quests = new List<QuestInfo>();
        public List<NPCSpeech> Speech = new List<NPCSpeech>();

        public List<UserItem> UsedGoods = new List<UserItem>();
        public Dictionary<string, List<UserItem>> BuyBack = new Dictionary<string, List<UserItem>>();

        public bool NeedSave;

        public NPCObject(NPCInfo info)
        {
            Info = info;
            NameColour = Color.Lime;

            Direction = (MirDirection)Envir.Random.Next(3);
            TurnTime = Envir.Time + Envir.Random.Next(100);

            Envir.NPCs.Add(this);

            Spawned();
            LoadScript();
        }

        private void LoadScript()
        {
            var script = NPCScript.GetOrAdd(ObjectID, Info.FileName, NPCScriptType.Normal);

            ScriptID = script.ScriptID;
        }

        public void ProcessGoods(bool clear = false)
        {
            if (!Settings.GoodsOn) return;

            var script = NPCScript.Get(ScriptID);

            List<UserItem> deleteList = new List<UserItem>();

            foreach (var playerGoods in BuyBack)
            {
                List<UserItem> items = playerGoods.Value;

                for (int i = 0; i < items.Count; i++)
                {
                    UserItem item = items[i];

                    if (DateTime.Compare(item.BuybackExpiryDate.AddMinutes(Settings.GoodsBuyBackTime), Envir.Now) <= 0 || clear)
                    {
                        deleteList.Add(BuyBack[playerGoods.Key][i]);

                        if (script.UsedTypes.Count != 0 && !script.UsedTypes.Contains(item.Info.Type))
                        {
                            continue;
                        }

                        var multiCount = UsedGoods.Count(x => x.Info.Index == item.Info.Index);

                        if (multiCount >= Settings.GoodsMaxStored)
                        {
                            UserItem nonAddedItem = UsedGoods.FirstOrDefault(e => e.IsAdded == false);

                            if (nonAddedItem != null)
                            {
                                UsedGoods.Remove(nonAddedItem);
                            }
                            else
                            {
                                UsedGoods.RemoveAt(0);
                            }
                        }

                        UsedGoods.Add(item);
                        NeedSave = true;
                    }
                }

                for (int i = 0; i < deleteList.Count; i++)
                {
                    BuyBack[playerGoods.Key].Remove(deleteList[i]);
                }
            }
        }


        #region Overrides
        public override void Process(DelayedAction action)
        {
            throw new NotSupportedException();
        }

        public override bool IsAttackTarget(HumanObject attacker)
        {
            return false;
        }
        public override bool IsFriendlyTarget(HumanObject ally)
        {
            throw new NotSupportedException();
        }
        public override bool IsFriendlyTarget(MonsterObject ally)
        {
            throw new NotSupportedException();
        }
        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return false;
        }

        public override Buff AddBuff(BuffType type, MapObject owner, int duration, Stats stats, bool refreshStats = true, bool updateOnly = false, params int[] values)
        {
            throw new NotSupportedException();
        }

        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            throw new NotSupportedException();
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            throw new NotSupportedException();
        }

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            throw new NotSupportedException();
        }

        public override void SendHealth(HumanObject player)
        {
            throw new NotSupportedException();
        }

        public override void Die()
        {
            throw new NotSupportedException();
        }

        public override int Pushed(MapObject pusher, MirDirection dir, int distance)
        {
            throw new NotSupportedException();
        }

        public override ushort Level
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override void ReceiveChat(string text, ChatType type)
        {
            throw new NotSupportedException();
        }

        public void Turn(MirDirection dir)
        {
            Direction = dir;

            Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
        }

        public override void Process()
        {
            base.Process();

            if (Envir.Time > TurnTime)
            {
                TurnTime = Envir.Time + TurnDelay;
                Turn((MirDirection)Envir.Random.Next(3));
            }

            if (Envir.Time > UsedGoodsTime)
            {
                UsedGoodsTime = Envir.Time + (Settings.Minute * Settings.GoodsBuyBackTime);
                ProcessGoods();
            }

            if (Envir.Time > VisTime)
            {
                VisTime = Envir.Time + (Settings.Minute);

                if (Info.DayofWeek != "" && Info.DayofWeek != Envir.Now.DayOfWeek.ToString())
                {
                    if (Visible) Hide();
                }
                else
                {
                    int StartTime = ((Info.HourStart * 60) + Info.MinuteStart);
                    int FinishTime = ((Info.HourEnd * 60) + Info.MinuteEnd);
                    int CurrentTime = ((Envir.Now.Hour * 60) + Envir.Now.Minute);

                    if (Info.TimeVisible)
                    {
                        if (StartTime > CurrentTime || FinishTime <= CurrentTime)
                        {
                            if (Visible) Hide();
                        }
                        else if (StartTime <= CurrentTime && FinishTime > CurrentTime)
                        {
                            if (!Visible) Show();
                        }
                    }
                }
            }

            if (Speech.Count > 0 && Envir.Time > SpeechTime)
            {
                var nearby = FindNearby(4);

                SpeechTime = Envir.Time + (SpeechDelay * (nearby ? Envir.Random.Next(1, 13) : 1));

                if (nearby)
                {
                    var maxWeight = Speech.Max(x => x.Weight);

                    var speech = Speech.OrderBy(x => x.GetWeight(Envir.Random, maxWeight)).Last();

                    Broadcast(new S.ObjectChat { ObjectID = this.ObjectID, Text = $"{Info.Name.Split('_')[0]}:{speech.Message}", Type = ChatType.Normal });
                }
            }
        }

        public override void SetOperateTime()
        {
            long time = Envir.Time + 2000;

            if (TurnTime < time && TurnTime > Envir.Time)
                time = TurnTime;

            if (OwnerTime < time && OwnerTime > Envir.Time)
                time = OwnerTime;

            if (ExpireTime < time && ExpireTime > Envir.Time)
                time = ExpireTime;

            if (PKPointTime < time && PKPointTime > Envir.Time)
                time = PKPointTime;

            if (LastHitTime < time && LastHitTime > Envir.Time)
                time = LastHitTime;

            if (EXPOwnerTime < time && EXPOwnerTime > Envir.Time)
                time = EXPOwnerTime;

            if (BrownTime < time && BrownTime > Envir.Time)
                time = BrownTime;

            for (int i = 0; i < ActionList.Count; i++)
            {
                if (ActionList[i].Time >= time && ActionList[i].Time > Envir.Time) continue;
                time = ActionList[i].Time;
            }

            for (int i = 0; i < PoisonList.Count; i++)
            {
                if (PoisonList[i].TickTime >= time && PoisonList[i].TickTime > Envir.Time) continue;
                time = PoisonList[i].TickTime;
            }

            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].NextTime >= time && Buffs[i].NextTime > Envir.Time) continue;
                time = Buffs[i].NextTime;
            }

            if (OperateTime <= Envir.Time || time < OperateTime)
                OperateTime = time;
        }

        public void Hide()
        {
            CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation);
            Visible = false;
        }

        public void Show()
        {
            Visible = true;
            for (int i = CurrentMap.Players.Count - 1; i >= 0; i--)
            {
                PlayerObject player = CurrentMap.Players[i];

                if (Functions.InRange(CurrentLocation, player.CurrentLocation, Globals.DataRange))
                {
                    CheckVisible(player, true);
                    if (player.CheckStacked())
                    {
                        player.StackingTime = Envir.Time + 1000;
                        player.Stacking = true;
                    }
                }
            }
        }

        public bool FindNearby(int distance)
        {
            for (int d = 0; d <= distance; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;
                        if (!CurrentMap.ValidPoint(x, y)) continue;
                        Cell cell = CurrentMap.GetCell(x, y);
                        if (cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            switch (ob.Race)
                            {
                                case ObjectType.Player:
                                    if (ob == this || ob.Dead) continue;
                                    return true;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public override Packet GetInfo()
        {
            return new S.ObjectNPC
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Image = Info.Image,
                Colour = Info.Colour,
                Location = CurrentLocation,
                Direction = Direction,
                QuestIDs = (from q in Quests
                            select q.Index).ToList()
            };
        }

        public Packet GetUpdateInfo()
        {
            return new S.NPCImageUpdate
            {
                ObjectID = ObjectID,
                Image = Info.Image,
                Colour = Info.Colour
            };
        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true)
        {
            throw new NotSupportedException();
        }

        public override string Name
        {
            get { return Info.Name; }
            set { throw new NotSupportedException(); }
        }

        public override bool Blocking
        {
            get { return Visible; }
        }


        public void CheckVisible(PlayerObject Player, bool Force = false)
        {
            VisibleLog.TryGetValue(Player.Info.Index, out bool canSee);

            if (Conq != null &&
                Conq.WarIsOn &&
                !Info.ConquestVisible)
            {
                if (canSee) CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation, Player);
                VisibleLog[Player.Info.Index] = false;
                return;
            }

            if (Info.FlagNeeded != 0 && !Player.Info.Flags[Info.FlagNeeded])
            {
                if (canSee) CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation, Player);
                VisibleLog[Player.Info.Index] = false;
                return;
            }

            if (Info.MinLev != 0 && Player.Level < Info.MinLev || Info.MaxLev != 0 && Player.Level > Info.MaxLev)
            {
                if (canSee) CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation, Player);
                VisibleLog[Player.Info.Index] = false;
                return;
            }

            if (Info.ClassRequired != "" && Player.Class.ToString() != Info.ClassRequired)
            {
                if (canSee) CurrentMap.Broadcast(new S.ObjectRemove { ObjectID = ObjectID }, CurrentLocation, Player);
                VisibleLog[Player.Info.Index] = false;
                return;
            }

            if (Visible && !canSee) CurrentMap.Broadcast(GetInfo(), CurrentLocation, Player);
            else if (Force && Visible) CurrentMap.Broadcast(GetInfo(), CurrentLocation, Player);

            VisibleLog[Player.Info.Index] = true;
        }

        public override int CurrentMapIndex { get; set; }

        public override Point CurrentLocation
        {
            get { return Info.Location; }
            set { throw new NotSupportedException(); }
        }

        public override MirDirection Direction { get; set; }

        public override int Health
        {
            get { throw new NotSupportedException(); }
        }

        public override int MaxHealth
        {
            get { throw new NotSupportedException(); }
        }
        #endregion

    }

    public class NPCSpeech
    {
        public int Weight { get; set; }
        public string Message { get; set; }

        public int GetWeight(RandomProvider rnd, int max)
        {
            return rnd.Next(Weight, max + 100);
        }
    }
}
