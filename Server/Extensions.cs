using System;
using System.Linq;
using Server.MirDatabase;

public static class UserItemExtension
{
    public static void LoadSolt(this UserItem item, string slots)
    {
        if(string.IsNullOrEmpty(slots)) return;
        var i = 0;
        foreach (var id in slots.Split(','))
        {
            if (id == "null")
            {
                item.Slots[i] = null;
                i++;
                continue;
            }
            using (var ctx = new DataContext())
            {
                var useritemId = Convert.ToInt64(id);
                item.Slots[i] = ctx.UserItems.FirstOrDefault(ui => ui.UniqueID == useritemId);
            }
        }
    }
}