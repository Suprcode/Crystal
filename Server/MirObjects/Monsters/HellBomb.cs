using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class HellBomb : MonsterObject
    {
        public long ExplosionTime;

        protected override bool CanMove { get { return false; } }
        protected override bool CanRegen { get { return false; } }

        protected internal HellBomb(MonsterInfo info)
            : base(info)
        {
            ExplosionTime = Envir.Time + (10 * Settings.Second);
            Direction = MirDirection.Up;
        }

        protected override void FindTarget() { }

        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir)
        {
            return false;
        }

        protected override void ProcessRegen() { }
        protected override void ProcessSearch() { }
        protected override void ProcessRoam() { }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true) { }
        

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            return 0;
        }

        protected override void ProcessTarget()
        {
            if (Envir.Time > ExplosionTime) { Die(); return; }
        }

        public override void Die()
        {
            if (HP > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                ActionList.Add(new DelayedAction(DelayedType.Die, Envir.Time + 500));
            }
            
            base.Die();
        }

        protected override void CompleteDeath(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(4, CurrentLocation, false);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                if (targets[i].Attacked(this, damage, DefenceType.AC) <= 0) continue;

                switch (Info.Image)
                {
                    case Monster.HellBomb1:
                        PoisonTarget(targets[i], 1, 5, PoisonType.Frozen, 2000);
                        break;
                    case Monster.HellBomb2:
                        PoisonTarget(targets[i], 1, 5, PoisonType.Dazed, 2000);
                        break;
                    case Monster.HellBomb3:
                        PoisonTarget(targets[i], 1, 5, PoisonType.Bleeding, 2000);
                        break;
                }
            }
        }

    }
}
