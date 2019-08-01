using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Server.MirObjects;

namespace Server.MirDatabase
{
    public class RecipeInfo
    {
        protected static Envir Envir
        {
            get { return SMain.Envir; }
        }

        public UserItem Item;
        public List<UserItem> Ingredients;

        public List<int> RequiredFlag = new List<int>();
        public ushort? RequiredLevel = null;
        public List<int> RequiredQuest = new List<int>();
        public List<MirClass> RequiredClass = new List<MirClass>();
        public MirGender? RequiredGender = null;

        public RecipeInfo(string name)
        {
            ItemInfo itemInfo = SMain.Envir.GetItemInfo(name);
            if (itemInfo == null)
            {
                SMain.Enqueue(string.Format("Could not find Item: {0}", name));
                return;
            }

            Item = SMain.Envir.CreateShopItem(itemInfo);

            LoadIngredients(name);
        }

        private void LoadIngredients(string recipe)
        {
            List<string> lines = File.ReadAllLines(Settings.RecipePath + recipe + ".txt").ToList();

            Ingredients = new List<UserItem>();

            var mode = "ingredients";

            for (int i = 0; i < lines.Count; i++)
            {
                if (String.IsNullOrEmpty(lines[i])) continue;

                if (lines[i].StartsWith("["))
                {
                    mode = lines[i].Substring(1, lines[i].Length - 2).ToLower();
                    continue;
                }

                switch (mode)
                {
                    case "ingredients":
                        {
                            var data = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            ItemInfo info = SMain.Envir.GetItemInfo(data[0]);

                            if (info == null)
                            {
                                SMain.Enqueue(string.Format("Could not find Item: {0}, Recipe: {1}", lines[i], recipe));
                                continue;
                            }

                            uint count = 1;
                            if (data.Length == 2)
                                uint.TryParse(data[1], out count);

                            UserItem ingredient = SMain.Envir.CreateShopItem(info);

                            ingredient.Count = count > info.StackSize ? info.StackSize : count;

                            Ingredients.Add(ingredient);
                        }
                        break;
                    case "criteria":
                        {
                            var data = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (data.Length < 2) continue;

                            try
                            {
                                switch (data[0].ToLower())
                                {
                                    case "level":
                                        RequiredLevel = ushort.Parse(data[1]);
                                        break;
                                    case "class":
                                        RequiredClass.Add((MirClass)byte.Parse(data[1]));
                                        break;
                                    case "gender":
                                        RequiredGender = (MirGender)byte.Parse(data[1]);
                                        break;
                                    case "flag":
                                        RequiredFlag.Add(int.Parse(data[1]));
                                        break;
                                    case "quest":
                                        RequiredQuest.Add(int.Parse(data[1]));
                                        break;
                                }
                            }
                            catch
                            {
                                SMain.Enqueue(string.Format("Could not parse option: {0}, Value: {1}", data[0], data[1]));
                                continue;
                            }
                        }
                        break;
                }
            }
        }

        public bool MatchItem(int index)
        {
            return Item != null && Item.ItemIndex == index;
        }

        public bool CanCraft(PlayerObject player)
        {
            if (RequiredLevel != null && RequiredLevel.Value > player.Level)
                return false;

            if (RequiredGender != null && RequiredGender.Value != player.Gender)
                return false;

            if (RequiredClass.Count > 0 && !RequiredClass.Contains(player.Class))
                return false;

            if (RequiredFlag.Count > 0)
            {
                foreach (var flag in RequiredFlag)
                {
                     if(!player.Info.Flags[flag])
                        return false;
                }
            }

            if (RequiredQuest.Count > 0)
            {
                foreach (var quest in RequiredQuest)
                {
                    if (!player.Info.CompletedQuests.Contains(quest))
                        return false;
                }
            }

            return true;
        }

        public ClientRecipeInfo CreateClientRecipeInfo()
        {
            ClientRecipeInfo clientInfo = new ClientRecipeInfo
            {
                Item = Item.Clone(),
                Ingredients = Ingredients.Select(x => x).ToList()
            };

            return clientInfo;
        }
    }
}


