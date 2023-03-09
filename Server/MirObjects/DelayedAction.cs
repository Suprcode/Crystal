using Server.MirEnvir;

namespace Server.MirObjects
{
    public enum DelayedType
    {
        Magic,
        /// <summary>
        /// Param0 MapObject (Target) | Param1 Damage | Param2 Defence | Param3 damageWeapon | Param4 UserMagic | Param5 FinalHit
        /// </summary>
        Damage,
        RangeDamage,        
        Spawn,
        Die,
        Recall,
        MapMovement,
        Mine,
        NPC,
        Poison,
        DamageIndicator,
        Quest,

        // Sanjian
        SpellEffect,
    }

    public class DelayedAction
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public DelayedType Type;
        public long Time;
        public long StartTime;
        public object[] Params;

        public bool FlaggedToRemove;

        public DelayedAction(DelayedType type, long time, params object[] p)
        {
            StartTime = Envir.Time;
            Type = type;
            Time = time;
            Params = p;
        }
    }
}
