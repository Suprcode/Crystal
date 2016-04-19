using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.MirObjects
{
    public enum DelayedType
    {
        Magic,
        Damage,
        RangeDamage,
        Spawn,
        Die,
        Recall,
        MapMovement,
        Mine,
        NPC,
        Poison,
        DamageIndicator
    }

    public class DelayedAction
    {
        public DelayedType Type;
        public long Time;
        public long StartTime;
        public object[] Params;

        public bool FlaggedToRemove;

        public DelayedAction(DelayedType type, long time, params object[] p)
        {
            StartTime = SMain.Envir.Time;
            Type = type;
            Time = time;
            Params = p;
        }
    }
}
