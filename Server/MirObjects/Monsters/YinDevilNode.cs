using System.Collections.Generic;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class YinDevilNode : MonsterObject
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

                BuffType bufftype = Info.AI == 41 ? BuffType.BlessedArmour : BuffType.UltimateEnhancer;
                Target.AddBuff(new Buff { Type = bufftype, Caster = this, ExpireTime = Envir.Time + 5 * 1000, Values = new int[] { Target.Level / 7 + 4 } });
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
