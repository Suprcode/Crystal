using System.Collections.Generic;
using Server.MirDatabase;
using System.Drawing;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class DigOutZombie : MonsterObject
    {
        public bool Visible, DoneDigOut;
        public long VisibleTime, DigOutTime;
        public Point DigOutLocation;
        public MirDirection DigOutDirection;

        protected override bool CanAttack
        {
            get
            {
                return Visible && base.CanAttack;
            }
        }
        protected override bool CanMove
        {
            get
            {
                return Visible && base.CanMove;
            }
        }
        public override bool Blocking
        {
            get
            {
                return Visible && base.Blocking;
            }
        }

        protected internal DigOutZombie(MonsterInfo info)
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
                    ActionTime = Envir.Time + 2000;
                    DigOutTime = Envir.Time;
                    DigOutLocation = CurrentLocation;
                    DigOutDirection = Direction;
                }
            }

            if (Visible && Envir.Time > DigOutTime + 1000 && !DoneDigOut)
            {
                SpellObject ob = new SpellObject
                    {
                        Spell = Spell.DigOutZombie,
                        Value = 1,
                        ExpireTime = Envir.Time + (5 * 60 * 1000),
                        TickSpeed = 2000,
                        Caster = null,
                        CurrentLocation = DigOutLocation,
                        CurrentMap = this.CurrentMap,
                        Direction = DigOutDirection
                    };
                CurrentMap.AddObject(ob);
                ob.Spawned();
                DoneDigOut = true;                    
            }

            base.ProcessAI();
        }

        public override bool Walk(MirDirection dir)
        {
            return Visible && base.Walk(dir);
        }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return Visible && base.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            return Visible && base.IsAttackTarget(attacker);
        }

        protected override void ProcessSearch()
        {
            if (Visible)
                base.ProcessSearch();
        }

        public override Packet GetInfo()
        {
            return !Visible ? null : base.GetInfo();
        }
    }
}