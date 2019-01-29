using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.MirEnvir;

namespace Server.MirDatabase.Extensions
{
    public static class ItemInfoExtensions
    {
        public static void Save(this ItemInfo info, int orgIndex)
        {
            using (Envir.ServerDb = new ServerDbContext())
            {
               if (info.Index == 0) Envir.ServerDb.Items.Add(info);

                if (Envir.ServerDb.Entry(info).State == EntityState.Detached)
                {
                    Envir.ServerDb.Items.Attach(info);
                    Envir.ServerDb.Entry(info).State = EntityState.Modified;
                }
                Envir.ServerDb.SaveChanges();
            }

            foreach (var gameShopItem in SMain.Envir.GameShopList)
            {
                if (gameShopItem.ItemIndex == orgIndex && orgIndex != info.Index)
                {
                    gameShopItem.ItemIndex = info.Index;
                }
            }
        }
    }
}
