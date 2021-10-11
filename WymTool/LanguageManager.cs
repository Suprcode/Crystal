using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WymTool
{
    public class LanguageManager
    {
        public enum TYPE
        {
            SYSTEM = 1, MAP = 2, ITEM = 3, MONSTER = 4, DROP = 5
        }
        private Dictionary<string, string> dicSys = new Dictionary<string, string>();
        private Dictionary<string, string> dicMap = new Dictionary<string, string>();
        private Dictionary<string, string> dicItem = new Dictionary<string, string>();
        private Dictionary<string, string> dicMonster = new Dictionary<string, string>();
        private Dictionary<string, string> dicDrop = new Dictionary<string, string>();

        private static LanguageManager _instance = null;
        private static object _instanceLock = new object();
        public static LanguageManager Instance()
        {
            if (_instance == null) //双if +lock
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new LanguageManager();
                    }
                }
                _instance.init();
            }
            return _instance;
        }
        private bool isNeedIndex(TYPE type)
        {
            return type == TYPE.MAP || type == TYPE.ITEM;
        }
        private void init()
        {
            // 配置语言包地址
            dicSys = initDic(TYPE.SYSTEM); dicMap = initDic(TYPE.MAP); dicItem = initDic(TYPE.ITEM); dicMonster = initDic(TYPE.MONSTER); dicDrop = initDic(TYPE.DROP);
        }

        // ------- toCN -------
        public string toCNSys(string strEN) { return toCN(TYPE.SYSTEM, strEN); }
        public string toCNMap(string strEN, string index) { return toCN(TYPE.MAP, strEN, index); }
        public string toCNItem(string strEN, string index) { return toCN(TYPE.ITEM, strEN, index); }
        //public string toCNMonster(string strEN) { return toCN(TYPE.MONSTER, strEN); }
        //public string toCNDrop(string strEN) { return toCN(TYPE.DROP, strEN); }
        // ------- toEN -------
        //public string toENSys(string strCN) { return toEN(TYPE.SYSTEM, strCN); }
        //public string toENMap(string strCN) { return toEN(TYPE.MAP, strCN); }
        //public string toENItem(string strCN) { return toEN(TYPE.ITEM, strCN); }
        //public string toENMonster(string strCN) { return toEN(TYPE.MONSTER, strCN); }
        //public string toENDrop(string strCN) { return toEN(TYPE.DROP, strCN); }

        /// <summary>
        /// 英文转中文
        /// </summary>
        /// <param name="type"></param>
        /// <param name="strEN"></param>
        /// <param name="strIndex">Map才需要</param>
        /// <returns></returns>
        private string toCN(TYPE type, string strEN, string strIndex = null)
        {
            Dictionary<string, string> dic;
            switch (type)
            {
                default: case TYPE.SYSTEM: dic = dicSys; break;
                case TYPE.MAP: dic = dicMap; break;
                case TYPE.ITEM: dic = dicItem; break;
                case TYPE.MONSTER: dic = dicMonster; break;
                case TYPE.DROP: dic = dicDrop; break;
            }
            if (isNeedIndex(type)) { strIndex = "_" + strIndex; strEN += strIndex; }
            if (!dic.ContainsKey(strEN)) { LogManager.Instance().Log(strEN, false); }
            string result = dic.ContainsKey(strEN) ? dic[strEN] : strEN;
            if (isNeedIndex(type)) { result = result.Replace(strIndex, string.Empty); }
            return result;
        }
        /// <summary>
        /// 中文转英文
        /// </summary>
        /// <param name="type"></param>
        /// <param name="strCN"></param>
        /// <param name="strIndex">Map才需要</param>
        /// <returns></returns>
        private string toEN(TYPE type, string strCN, string strIndex = null)
        {
            Dictionary<string, string> dic;
            switch (type)
            {
                default: case TYPE.SYSTEM: dic = dicSys; break;
                case TYPE.MAP: dic = dicMap; break;
                case TYPE.ITEM: dic = dicItem; break;
                case TYPE.MONSTER: dic = dicMonster; break;
                case TYPE.DROP: dic = dicDrop; break;
            }
            //if (isNeedIndex(type)) { strIndex = "_" + strIndex; strCN += strIndex; };
            //var keys = dic.Where(q => q.Value == strCN).Select(q => q.Key);  //get all keys
            //List<string> keyList = (from q in dic where q.Value == strCN select q.Key).ToList<string>(); //get all keys
            string strEN = dic.FirstOrDefault(q => q.Value == strCN).Key;  //get first key
            if (strEN == null) { LogManager.Instance().Log(strCN, false); }
            string result = strEN != null ? strEN : strCN;
            //if (isNeedIndex(type)) { result = result.Replace(strIndex, string.Empty); }
            return result;
        }

        private Dictionary<string, string> initDic(TYPE type)
        {
            string arrStr = ConfigManager.Instance().PathConfig + "Language" + (int)type + ".txt";
            string[] arr = File.ReadAllLines(arrStr);
            Dictionary<string, string> d = new Dictionary<string, string>();
            foreach (string e in arr)
            {
                string[] v = e.Split('\t');
                if (d.ContainsKey(v[0]))
                {
                    LogManager.Instance().Log("重复项 = " + type + ", index = " + (d.Count + 1) + ", key = " + v[0], true);
                }
                d.Add(v[0], v[1]);
            }
            return d;
        }
    }
}
