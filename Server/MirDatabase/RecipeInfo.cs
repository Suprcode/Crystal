using Server.MirEnvir;
using Server.MirObjects;

namespace Server.MirDatabase
{
    public class RecipeInfo
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        protected static MessageQueue MessageQueue
        {
            get { return MessageQueue.Instance; }
        }

        public UserItem Item;
        public List<UserItem> Ingredients;
        public List<UserItem> Tools;

        public List<int> RequiredFlag = new List<int>();
        public ushort? RequiredLevel = null;
        public List<int> RequiredQuest = new List<int>();
        public List<MirClass> RequiredClass = new List<MirClass>();
        public MirGender? RequiredGender = null;

        public byte Chance = 100;
        public uint Gold = 0;

        public RecipeInfo(string name)
        {
            ItemInfo itemInfo = Envir.GetItemInfo(name);
            if (itemInfo == null)
            {
                MessageQueue.Enqueue(string.Format("Could not find Item: {0}", name));
                return;
            }

            Item = Envir.CreateShopItem(itemInfo, ++Envir.NextRecipeID);

            LoadIngredients(name);
        }

        private void LoadIngredients(string recipe)
        {
            List<string> lines = File.ReadAllLines(Path.Combine(Settings.RecipePath, recipe + ".txt")).ToList();

            Tools = new List<UserItem>();
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
                    case "recipe":
                        {
                            var data = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (data.Length < 2) continue;

                            switch (data[0].ToLower())
                            {
                                case "amount":
                                    Item.Count = ushort.Parse(data[1]);
                                    break;
                                case "chance":
                                    Chance = byte.Parse(data[1]);

                                    if (Chance > 100)
                                    {
                                        Chance = 100;
                                    }
                                    break;
                                case "gold":
                                    Gold = uint.Parse(data[1]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "tools":
                        {
                            var data = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            ItemInfo info = Envir.GetItemInfo(data[0]);

                            if (info == null)
                            {
                                MessageQueue.Enqueue(string.Format("Could not find Tool: {0}, Recipe: {1}", lines[i], recipe));
                                continue;
                            }

                            UserItem tool = Envir.CreateShopItem(info, 0);

                            Tools.Add(tool);
                        }
                        break;
                    case "ingredients":
                        {
                            var data = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            ItemInfo info = Envir.GetItemInfo(data[0]);

                            if (info == null)
                            {
                                MessageQueue.Enqueue(string.Format("Could not find Ingredient: {0}, Recipe: {1}", lines[i], recipe));
                                continue;
                            }

                            UserItem ingredient = Envir.CreateShopItem(info, 0);

                            ushort count = 1;
                            if (data.Length >= 2)
                                ushort.TryParse(data[1], out count);

                            if (data.Length >= 3)
                                ushort.TryParse(data[2], out ingredient.CurrentDura);

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
                                        if (Enum.TryParse<MirClass>(data[1], true, out MirClass cls))
                                        {
                                            RequiredClass.Add(cls);
                                        }
                                        else
                                        {
                                            RequiredClass.Add((MirClass)byte.Parse(data[1]));
                                        }
                                        break;
                                    case "gender":
                                        if (Enum.TryParse<MirGender>(data[1], true, out MirGender gender))
                                        {
                                            RequiredGender = gender;
                                        }
                                        else
                                        {
                                            RequiredGender = (MirGender)byte.Parse(data[1]);
                                        }
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
                                MessageQueue.Enqueue(string.Format("Could not parse option: {0}, Value: {1}", data[0], data[1]));
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
                Gold = Gold,
                Chance = Chance,
                Item = Item.Clone(),
                Tools = Tools.Select(x => x).ToList(),
                Ingredients = Ingredients.Select(x => x).ToList()
            };

            return clientInfo;
        }
    }
}


