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

        private bool _enabled = false; //Get from individual player to enabled/disable logging ??

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

        public void GoldChange(string source, uint amount, bool lost = true, string info = "")
        {
            string task = string.Format("{0} x{1} Gold", lost ? "Lost" : "Gained", amount);

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        public void MapChange(string source, MapInfo oldMap, MapInfo newMap, string info = "")
        {
            string task = string.Format("Moved Map {0} => {1}", oldMap.FileName, newMap.FileName);

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        public void ItemChange(string source, UserItem item, uint amount = 1, bool lost = true, string info = "")
        {
            string task = string.Format("{0} x{1} Item {2}", lost ? "Lost" : "Gained", amount, item.Name);

            Action action = new Action { Source = source, Task = task, UniqueID = item.UniqueID, AddedInfo = info };

            RecordAction(action);
        }

        public void KilledPlayer(string source, PlayerObject obj, string info = "")
        {
            string task = string.Format("Killed Player {0}", obj.Name);

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        public void KilledMonster(string source, MonsterObject obj, string info = "")
        {
            string task = string.Format("Killed Monster {0}", obj.Name);

            Action action = new Action { Source = source, Task = task, AddedInfo = info };

            RecordAction(action);
        }

        public void Died()
        {
            Action action = new Action { Task = "Died" };

            RecordAction(action);
        }

        public void Connected()
        {
            Action action = new Action { Task = "Connected" };

            RecordAction(action);
        }

        public void Disconnected()
        {
            Action action = new Action { Task = "Disconnected" };

            RecordAction(action);
        }

        public void ForceSave()
        {
            Save();
        }

        #endregion

        #region Private Methods

        private void RecordAction(Action action)
        {
            if (!DoLog) return;

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

            foreach (Action action in Actions)
            {
                string output = string.Format("{0:hh\\:mm\\:ss}, {1}, {2}, {3}, {4}, {5}" + Environment.NewLine,
                    action.Time, action.Player, action.Source, action.Task, action.UniqueID, action.AddedInfo);

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
        public ulong UniqueID;
        public string Task;
        public string Source;
        public string AddedInfo;
    }
}