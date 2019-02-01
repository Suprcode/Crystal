using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.MirEnvir;

namespace Server.MirDatabase.Extensions
{
    public static class UserItemExtension
    {
        public static void Save(this UserItem item, bool convert = false)
        {
            if (convert) item.UniqueID = 0;
            using (var accountDb = new AccountDbContext())
            {
                if (item.UniqueID == 0) accountDb.UserItems.Add(item);

                if (accountDb.Entry(item).State == EntityState.Detached)
                {
                    accountDb.UserItems.Attach(item);
                    accountDb.Entry(item).State = EntityState.Modified;
                }

                item.SlotString = string.Join(",", item.Slots.Select(s => s?.UniqueID ?? 0));

                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    item.Awake.Save(writer);
                    item.AwakeBytes = ms.ToArray();
                }

                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    item.ExpireInfo?.Save(writer);
                    item.ExpireInfoBytes = ms.ToArray();
                }

                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    item.RentalInformation?.Save(writer);
                    item.RentalInformationBytes = ms.ToArray();
                }

                accountDb.SaveChanges();
            }
        }
    }
}
