using System;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;
using System.Drawing;

namespace Server.MirObjects.Monsters
{
    public abstract class CastleGate : MonsterObject
    {
        public bool Closed;

        protected List<BlockingObject> BlockingObjects = new List<BlockingObject>();

        protected Point[] BlockArray;

        protected override bool CanAttack
        {
            get
            {
                return false;
            }
        }

        protected override bool CanMove { get { return false; } }
        public override bool Blocking
        {
            get
            {
                return Closed && base.Blocking;
            }
        }

        protected internal CastleGate(MonsterInfo info)
            : base(info)
        {
            Closed = true;
        }


        public override void Spawned()
        {
            base.Spawned();

            if (BlockArray == null) return;

            MonsterInfo bInfo = new MonsterInfo
            {
                HP = this.HP,
                Image = Monster.EvilMirBody,
                CanTame = false,
                CanPush = false,
                AutoRev = false
            };        

            foreach (var block in BlockArray)
            {
                BlockingObject b = new BlockingObject(this, bInfo);
                BlockingObjects.Add(b);

                if (!b.Spawn(this.CurrentMap, new Point(this.CurrentLocation.X + block.X, this.CurrentLocation.Y + block.Y)))
                {
                    SMain.EnqueueDebugging(string.Format("CastleGate blocking mob not spawned at {0} {1}:{2}", CurrentMap.Info.FileName, block.X, block.Y));
                }
            }
        }
        protected override void ProcessAI()
        {
            base.ProcessAI();
        }


        public override void Turn(MirDirection dir) { }

        public override bool Walk(MirDirection dir) { return false; }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return Closed && base.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            return Closed && base.IsAttackTarget(attacker);
        }

        protected override void ProcessRoam() { }

        protected override void ProcessSearch() { }

        public override Packet GetInfo()
        {
            return base.GetInfo();
        }

        public override void Die()
        {
            ActiveDoorWall(false);
            base.Die();
        }

        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            return base.Attacked(attacker, damage, type, damageWeapon);
        }

        protected abstract int GetDamageLevel();

        public abstract void OpenDoor();

        public abstract void CloseDoor();

        public abstract void RepairGate();

        protected virtual void ActiveDoorWall(bool closed)
        {
            foreach (var obj in BlockingObjects)
            {
                if (closed)
                    obj.Show();
                else
                    obj.Hide();
            }
        }

    }
}
