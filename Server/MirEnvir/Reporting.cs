using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Server.MirObjects;
using Server.MirDatabase;

namespace Server.MirEnvir
{
    public class Reporting
    {
        public PlayerObject Player;
        public List<Action> Actions = new List<Action>();

        //private int _traceDepth = 2;
        private int _saveCount = 200;
        private string _baseDir = "";

        #region Public Properties

        private bool _enabled = true; //Get from individual player to enabled/disable logging ??

        public bool DoLog
        {
            get { return _enabled; }
        }

        #endregion

        #region Constructors

        public Reporting() { }

        public Reporting(PlayerObject player)
        {
            Player = player;

            string baseDir = Settings.ReportPath + player.Name;

            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            _baseDir = baseDir;
        }

        #endregion

        #region Log Actions

        #region Move Actions

        public void MapChange(string source, MapInfo oldMap, MapInfo newMap, string info = "")
        {
            string task = string.Format("地图移动 {0} => {1}", oldMap.FileName, newMap.FileName);

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        #endregion
        
        #region Item Actions

        //TOADD
        //ItemSplit
        //ItemMerge

        public void ItemCombined(string source, UserItem fromItem, UserItem toItem, int slotFrom, int slotTo, MirGridType grid)
        {
            string task = string.Empty;
            if (fromItem != null && toItem != null)
            {
                task = string.Format("物品合并 - {0} 跟 {1} 从 {2} 到 {3} 在 {4} ({5})", fromItem.Info.Name, toItem.Info.Name, slotFrom, slotTo, grid, toItem.UniqueID);
            }

            Action action = new Action { Source = source, Task = task };

            RecordAction(action);
        }

        public void ItemMoved(string source, UserItem item, MirGridType from, MirGridType to, int slotFrom, int slotTo, string info = "")
        {
            string task = string.Empty;

            if (item != null)
            {
                task = string.Format("物品移动 - {0} 从 {1}:{2} 到 {3}:{4} ({5})", item.Info.Name, from, slotFrom, to, slotTo, item.UniqueID);
            }

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        public void ItemChanged(string source, UserItem item, uint amount, int state)
        {
            string type = string.Empty;
            string task = string.Empty;
            if (item.Info == null)
            {
                using (var ctx = new DataContext())
                {
                    item.Info = ctx.ItemInfos.FirstOrDefault(i => i.Index == item.ItemIndex);
                }
            }
            switch (state)
            {
                case 1:
                    type = "失去";
                    break;
                case 2:
                    type = "获得";
                    break;
            }

            if (item != null)
            {
                task = string.Format("物品 {0} - {1} x{2} ({3})", type, item.Info.Name, amount, item.UniqueID);
            }

            Action action = new Action { Source = source, Task = task };

            RecordAction(action);
        }

        public void ItemGSBought(string source, GameShopItem item, uint amount, uint CreditCost, uint GoldCost)
        {
            string type = string.Empty;
            string task = string.Empty;

            if (item != null)
            {
                task = string.Format("交易了 {1} x{0} {2} 点数 跟 {3} 金币.", item.Info.FriendlyName, amount, CreditCost, GoldCost );
            }

            Action action = new Action { Source = source, Task = task };

            RecordAction(action);
        }

        public void ItemMailed(string source, UserItem item, uint amount, int reason)
        {
            string task = string.Empty;
            string message = string.Empty;

            switch (reason)
            {
                case 1:
                    message = "Could not return item to bag after trade.";
                    break;
                default:
                    message = "No reason provided.";
                    break;
            }

            if (item != null)
            {
                task = string.Format("Mailed {1} x{0}. Reason : {2}.", item.Info.FriendlyName, amount, message);
            }

            Action action = new Action { Source = source, Task = task };

            RecordAction(action);
        }

        public void GoldChanged(string source, uint amount, bool lost = true, string info = "")
        {
            string task = string.Format("金币{0} - x{1}", lost ? "失去" : "获得", amount);

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        public void CreditChanged(string source, uint amount, bool lost = true, string info = "")
        {
            string task = string.Format("点数{0} - x{1}", lost ? "失去" : "获得", amount);

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        public void ItemError(string source, MirGridType from, MirGridType to, int slotFrom, int slotTo)
        {
            string task = string.Empty;

            task = string.Format("物品移动出错 - 从 {0}:{1} 到 {2}:{3}", from, slotFrom, to, slotTo);

            Action action = new Action { Source = source, Task = task };

            RecordAction(action);
        }

        #endregion

        #region Kill Actions

        public void KilledPlayer(string source, PlayerObject obj, string info = "")
        {
            string task = string.Format("杀死玩家 {0}", obj.Name);

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        public void KilledMonster(string source, MonsterObject obj, string info = "")
        {
            string task = string.Format("杀死怪物 {0}", obj.Name);

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        #endregion

        #region Other Actions

        public void ChatMessage(string msg)
        {
            string task = string.Format("Chat: {0}", msg);

            Action action = new Action { Task = task };

            RecordAction(action);
        }

        public void Levelled(int level)
        {
            string task = string.Format("升级到 {0}", level);

            Action action = new Action { Task = task };

            RecordAction(action);
        }

        public void Died(string map = "")
        {
            string info = "";

            if(!string.IsNullOrEmpty(map))
            {
                string.Format("地图 {0}", map);
            }

            Action action = new Action { Task = "Died", AddedInfo = info };

            RecordAction(action);
        }

        public void Connected(string ipAddress)
        {
            Action action = new Action { Task = "Connected", AddedInfo = ipAddress };

            RecordAction(action);
        }

        public void Disconnected(string reason)
        {
            Action action = new Action { Task = "Disconnected", AddedInfo = reason };

            RecordAction(action);
        }

        #endregion

        public void ForceSave()
        {
            Save();
        }

        #endregion

        #region Private Methods

        private void RecordAction(Action action)
        {
            if (!DoLog || Player.Info == null) return;

            action.Time = SMain.Envir.Now;
            action.Player = Player.Name;

            Actions.Add(action);

            if (Actions.Count > _saveCount)
                Save();
        }

        private void Save()
        {
            if (!DoLog || Actions.Count < 1) return;

            string filename = SMain.Envir.Now.Date.ToString(@"yyyy-MM-dd");
            string fullPath = _baseDir + @"\" + filename + ".txt";

            if (!File.Exists(fullPath))
                File.Create(fullPath).Close();
            
            for (int i = 0; i < Actions.Count; i++)
            {
                Action action = Actions[i];

                string output = string.Format("{0:hh\\:mm\\:ss}, {1}, {2}, {3}, {4}" + Environment.NewLine,
                    action.Time, action.Player, action.Task, action.AddedInfo, action.Source);

                File.AppendAllText(fullPath, output);
            }

            Actions.Clear();
        }

        //private string StackTrace(int offset = 0)
        //{
        //    if (offset < 0) return "";

        //    int depth = _traceDepth + offset;

        //    StackTrace stackTrace = new StackTrace();
        //    string methodName = stackTrace.GetFrame(depth).GetMethod().Name;

        //    if (methodName.Length > 0) return methodName;

        //    return "Unknown";
        //}

        #endregion
    }

    public class Action
    {
        public string Player;
        public DateTime Time;
        public string Task;
        public string AddedInfo;
        public string Source;
    }
}