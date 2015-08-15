using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Server.MirObjects;
using System.Text.RegularExpressions;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class QuestProgressInfo
    {
        public int Index;

        public QuestInfo Info;

        public DateTime StartDateTime = DateTime.MinValue;
        public DateTime EndDateTime = DateTime.MaxValue;

        public List<int> KillTaskCount = new List<int>();
        public List<long> ItemTaskCount = new List<long>();
        public List<bool> FlagTaskSet = new List<bool>();

        public List<string> TaskList = new List<string>();

        public bool Taken
        {
            get { return StartDateTime > DateTime.MinValue; }
        }

        public bool Completed
        {
            get { return EndDateTime < DateTime.MaxValue; }
        }

        public bool New
        {
            get { return StartDateTime > DateTime.Now.AddDays(-1); }
        }

        public QuestProgressInfo(int index)
        {
            Index = index;

            Info = SMain.Envir.QuestInfoList.FirstOrDefault(e => e.Index == index);

            foreach (var kill in Info.KillTasks)
                KillTaskCount.Add(0);

            foreach (var item in Info.ItemTasks)
                ItemTaskCount.Add(0);

            foreach (var flag in Info.FlagTasks)
                FlagTaskSet.Add(false);

            CheckCompleted();
        }

        public QuestProgressInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Info = SMain.Envir.QuestInfoList.FirstOrDefault(e => e.Index == Index);

            StartDateTime = DateTime.FromBinary(reader.ReadInt64());
            EndDateTime = DateTime.FromBinary(reader.ReadInt64());

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                KillTaskCount.Add(reader.ReadInt32());

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                ItemTaskCount.Add(reader.ReadInt64());

            if (Envir.LoadVersion >= 37)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    FlagTaskSet.Add(reader.ReadBoolean());
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);

            writer.Write(StartDateTime.ToBinary());
            writer.Write(EndDateTime.ToBinary());

            writer.Write(KillTaskCount.Count);
            for (int i = 0; i < KillTaskCount.Count; i++)
                writer.Write(KillTaskCount[i]);

            writer.Write(ItemTaskCount.Count);
            for (int i = 0; i < ItemTaskCount.Count; i++)
                writer.Write(ItemTaskCount[i]);

            writer.Write(FlagTaskSet.Count);
            for (int i = 0; i < FlagTaskSet.Count; i++)
                writer.Write(FlagTaskSet[i]);
        }

        public void ResyncTasks()
        {
            if (Info.KillTasks.Count != KillTaskCount.Count)
            {
                if (KillTaskCount.Count > Info.KillTasks.Count)
                {
                    KillTaskCount.RemoveRange(Info.KillTasks.Count, KillTaskCount.Count - Info.KillTasks.Count);
                }
                else
                {
                    while (KillTaskCount.Count < Info.KillTasks.Count)
                    {
                        KillTaskCount.Add(0);
                    }
                }

                EndDateTime = DateTime.MaxValue;
            }

            if (Info.ItemTasks.Count != ItemTaskCount.Count)
            {
                if (ItemTaskCount.Count > Info.ItemTasks.Count)
                {
                    ItemTaskCount.RemoveRange(Info.ItemTasks.Count, ItemTaskCount.Count - Info.ItemTasks.Count);
                }
                else
                {
                    while (ItemTaskCount.Count < Info.ItemTasks.Count)
                    {
                        ItemTaskCount.Add(0);
                    }
                }

                EndDateTime = DateTime.MaxValue;
            }

            if (Info.FlagTasks.Count != FlagTaskSet.Count)
            {
                if (FlagTaskSet.Count > Info.FlagTasks.Count)
                {
                    FlagTaskSet.RemoveRange(Info.FlagTasks.Count, FlagTaskSet.Count - Info.FlagTasks.Count);
                }
                else
                {
                    while (FlagTaskSet.Count < Info.FlagTasks.Count)
                    {
                        FlagTaskSet.Add(false);
                    }
                }

                EndDateTime = DateTime.MaxValue;
            }

        }

        public bool CheckCompleted()
        {
            UpdateTasks();

            bool canComplete = true;

            for (int i = 0; i < Info.KillTasks.Count; i++)
            {
                if (KillTaskCount[i] >= Info.KillTasks[i].Count) continue;

                canComplete = false;
            }

            for (int i = 0; i < Info.ItemTasks.Count; i++)
            {
                if (ItemTaskCount[i] >= Info.ItemTasks[i].Count) continue;

                canComplete = false;
            }

            for (int i = 0; i < Info.FlagTasks.Count; i++)
            {
                if (FlagTaskSet[i]) continue;

                canComplete = false;
            }

            if (!canComplete) return false;

            if (!Completed)
                EndDateTime = DateTime.Now;

            return true;
        }

        #region Need Requirement

        public bool NeedItem(ItemInfo iInfo)
        {
            return Info.ItemTasks.Where((task, i) => ItemTaskCount[i] < task.Count && task.Item == iInfo).Any();
        }

        public bool NeedKill(MonsterInfo mInfo)
        {
            //if (info.Name != name && !info.Name.Replace(" ", "").StartsWith(name, StringComparison.OrdinalIgnoreCase)) continue;
            return Info.KillTasks.Where((task, i) => KillTaskCount[i] < task.Count && mInfo.Name.StartsWith(task.Monster.Name, StringComparison.OrdinalIgnoreCase)).Any();
        }

        public bool NeedFlag(int flagNumber)
        {
            return Info.FlagTasks.Where((task, i) => FlagTaskSet[i] == false && task.Number == flagNumber).Any();
        }

        #endregion

        #region Process Quest Task

        public void ProcessKill(MonsterInfo mInfo)
        {
            if (Info.KillTasks.Count < 1) return;

            for (int i = 0; i < Info.KillTasks.Count; i++)
            {
                //if (Info.KillTasks[i].Monster.Index != mobIndex) continue;
                if (!mInfo.Name.StartsWith(Info.KillTasks[i].Monster.Name, StringComparison.OrdinalIgnoreCase)) continue;
                KillTaskCount[i]++;

                return;
            }
        }

        public void ProcessItem(UserItem[] inventory)
        {
            for (int i = 0; i < Info.ItemTasks.Count; i++)
            {
                long count = inventory.Where(item => item != null).
                    Where(item => item.Info == Info.ItemTasks[i].Item).
                    Aggregate<UserItem, long>(0, (current, item) => current + item.Count);

                ItemTaskCount[i] = count;
            }
        }

        public void ProcessFlag(bool[] Flags)
        {
            for (int i = 0; i < Info.FlagTasks.Count; i++)
            {
                for (int j = 0; j < Flags.Length - 1000; j++)
                {
                    if (Info.FlagTasks[i].Number != j || !Flags[j]) continue;

                    FlagTaskSet[i] = Flags[j];
                    break;
                }
            }
        }

        #endregion

        #region Update Task Messages

        public void UpdateTasks()
        {
            TaskList = new List<string>();

            UpdateKillTasks();
            UpdateItemTasks();
            UpdateFlagTasks();
            UpdateGotoTask();
        }

        public void UpdateKillTasks()
        {
            if(Info.KillMessage.Length > 0 && Info.KillTasks.Count > 0) 
            {
                bool allComplete = true;
                for (int i = 0; i < Info.KillTasks.Count; i++)
                {
                    if (KillTaskCount[i] >= Info.KillTasks[i].Count) continue;

                    allComplete = false;
                }

                TaskList.Add(string.Format("{0} {1}", Info.KillMessage, allComplete ? "(Completed)" : ""));
                return;
            }

            for (int i = 0; i < Info.KillTasks.Count; i++)
            {
                if (string.IsNullOrEmpty(Info.KillTasks[i].Message))
                    TaskList.Add(string.Format("Kill {0}: {1}/{2} {3}", Info.KillTasks[i].Monster.GameName, KillTaskCount[i],
                        Info.KillTasks[i].Count, KillTaskCount[i] >= Info.KillTasks[i].Count ? "(Completed)" : ""));
                else
                    TaskList.Add(string.Format("{0} {1}", Info.KillTasks[i].Message, KillTaskCount[i] >= Info.KillTasks[i].Count ? "(Completed)" : ""));
                    
            }
        }

        public void UpdateItemTasks()
        {
            if (Info.ItemMessage.Length > 0 && Info.ItemTasks.Count > 0)
            {
                bool allComplete = true;
                for (int i = 0; i < Info.ItemTasks.Count; i++)
                {
                    if (ItemTaskCount[i] >= Info.ItemTasks[i].Count) continue;

                    allComplete = false;
                }

                TaskList.Add(string.Format("{0} {1}", Info.ItemMessage, allComplete ? "(Completed)" : ""));
                return;
            }

            for (int i = 0; i < Info.ItemTasks.Count; i++)
            {
                if (string.IsNullOrEmpty(Info.ItemTasks[i].Message))
                    TaskList.Add(string.Format("Collect {0}: {1}/{2} {3}", Info.ItemTasks[i].Item.Name, ItemTaskCount[i],
                        Info.ItemTasks[i].Count, ItemTaskCount[i] >= Info.ItemTasks[i].Count ? "(Completed)" : ""));
                else
                    TaskList.Add(string.Format("{0} {1}", Info.ItemTasks[i].Message, ItemTaskCount[i] >= Info.ItemTasks[i].Count ? "(Completed)" : ""));
            }
        }

        public void UpdateFlagTasks()
        {
            if (Info.FlagMessage.Length > 0)
            {
                bool allComplete = true;
                for (int i = 0; i < Info.FlagTasks.Count; i++)
                {
                    if (FlagTaskSet[i]) continue;

                    allComplete = false;
                }

                TaskList.Add(string.Format("{0} {1}", Info.FlagMessage, allComplete ? "(Completed)" : ""));
                return;
            }

            for (int i = 0; i < Info.FlagTasks.Count; i++)
            {
                if (string.IsNullOrEmpty(Info.FlagTasks[i].Message))
                    TaskList.Add(string.Format("Activate Flag {0} {1}", Info.FlagTasks[i].Number, FlagTaskSet[i] ? "(Completed)" : ""));
                else
                    TaskList.Add(string.Format("{0} {1}", Info.FlagTasks[i].Message, FlagTaskSet[i] ? "(Completed)" : ""));

            }
        }

        public void UpdateGotoTask()
        {
            if (Info.GotoMessage.Length <= 0 || !Completed) return;

            TaskList.Add(Info.GotoMessage);
        }

        #endregion

        public ClientQuestProgress CreateClientQuestProgress()
        {
            return new ClientQuestProgress
            {
                Id = Index,
                TaskList = TaskList,
                Taken = Taken,
                Completed = Completed,
                New = New
            };
        }
    }
}
