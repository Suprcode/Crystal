using Server.MirDatabase;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class TalkingMonster : MonsterObject
    {
        public HashSet<MapObject> TalkingObjects = new HashSet<MapObject>();
        private long TalkTime;

        protected override bool CanMove
        {
            get { return TalkingObjects.Count < 1 && !Dead && Envir.Time > MoveTime && Envir.Time > ActionTime && Envir.Time > ShockTime; }
        }

        protected internal TalkingMonster(MonsterInfo info)
            : base(info)
        {
        }

        public override bool IsAttackTarget(PlayerObject attacker) { return false; }
        public override bool IsAttackTarget(MonsterObject attacker) { return false; }

        public override void Process()
        {
            if (Envir.Time >= TalkTime)
            {
                TalkTime = Envir.Time + (Settings.Second * 5);

                List<MapObject> objToRemove = new List<MapObject>();

                foreach (var obj in TalkingObjects)
                {
                    MapObject target = FindObject(obj.ObjectID, (Globals.DataRange / 2));

                    if (target == null) objToRemove.Add(obj);
                }

                foreach (var removeObj in objToRemove)
                {
                   TalkingObjects.Remove(removeObj); 
                }
            }

            base.Process();
        }
    }
}
