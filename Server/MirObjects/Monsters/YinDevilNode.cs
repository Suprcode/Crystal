using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class YinDevilNode : MonsterObject
    {
        protected override bool CanMove { get { return false; } }

        protected internal YinDevilNode(MonsterInfo info) 
            : base(info)
        {
        }

        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        protected override void ProcessRoam() { }

        protected override void CompleteAttack(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(7, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];
                if (Target == null || !Target.IsFriendlyTarget(this) || Target.CurrentMap != CurrentMap || Target.Node == null) continue;

                BuffType type = Info.AI == 41 ? BuffType.BlessedArmour : BuffType.UltimateEnhancer;

                var stats = new Stats
                {
                    [type == BuffType.BlessedArmour ? Stat.MaxAC : Stat.MaxDC] = Target.Level / 7 + 4
                };

                Target.AddBuff(type, this, Settings.Second * 5, stats);
                Target.OperateTime = 0;
            }

        }
        protected override void ProcessTarget()
        {
            if (!CanAttack) return;
            if (!FindFriendsNearby(7)) return;

            ShockTime = 0;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + 500));
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
        }
    }
}
