using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.MirEnvir;

namespace Server.MirDatabase.Extensions
{
    public static class GameShopItemExtensions
    {
        public static void Save(this GameShopItem gsItem)
        {
            using (Envir.ServerDb = new ServerDbContext())
            {
                var newItemIndex = SMain.Envir.GetItemInfo(gsItem.Info.Name)?.Index;
                if (gsItem.ItemIndex != newItemIndex && newItemIndex != null) gsItem.ItemIndex = newItemIndex.Value;
                if (gsItem.GIndex == 0) Envir.ServerDb.GameShopItems.Add(gsItem);
                if (Envir.ServerDb.Entry(gsItem).State == EntityState.Detached)
                {
                    Envir.ServerDb.GameShopItems.Attach(gsItem);
                    Envir.ServerDb.Entry(gsItem).State = EntityState.Modified;
                }

                Envir.ServerDb.SaveChanges();
            }
        }
    }
}
