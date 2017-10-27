using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

            for (int i = 0; i < lines.Count; i++)
            {
                if (String.IsNullOrEmpty(lines[i])) continue;

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
        }

        public bool MatchItem(int index)
        {
            return Item != null && Item.ItemIndex == index;
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
