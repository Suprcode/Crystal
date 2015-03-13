using System;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class CannibalPlant : HarvestMonster
    {
        public bool Visible;
        public long VisibleTime;

        protected override bool CanAttack
        {
            get
            {
                return Visible && base.CanAttack;
            }
        }
        protected override bool CanMove { get { return false; } }
        public override bool Blocking
        {
            get
            {
                return Visible && base.Blocking;
            }
        }

        protected internal CannibalPlant(MonsterInfo info)
            : base(info)
        {
            Visible = false;
        }

        protected override void ProcessAI()
        {
            if (!Dead && Envir.Time > VisibleTime)
            {
                VisibleTime = Envir.Time + 2000;

                bool visible = FindNearby(3);

                if (!Visible && visible)
                {
                    Visible = true;
                    CellTime = Envir.Time + 500;
                    Broadcast(GetInfo());
                    Broadcast(new S.ObjectShow { ObjectID = ObjectID });
                    ActionTime = Envir.Time + 1000;
                }

                if (Visible && !visible)
                {
                    Visible = false;
                    VisibleTime = Envir.Time + 3000;

                    Broadcast(new S.ObjectHide { ObjectID = ObjectID });

                    SetHP(MaxHP);
                }
            }

            base.ProcessAI();
        }


        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return Visible && base.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            return Visible && base.IsAttackTarget(attacker);
        }

        protected override void ProcessRoam() { }

        protected override void ProcessSearch()
        {
            if (Visible)
                base.ProcessSearch();
        }

        public override Packet GetInfo()
        {
            if (!Visible) return null;

            return base.GetInfo();
        }
    }
}
