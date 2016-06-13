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
        public ConquestObject Conquest;
        public int GateIndex;

        public bool Closed;
        private long CloseTime;

        private bool AutoOpen = true;

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
                    SMain.EnqueueDebugging(string.Format("{3} blocking mob not spawned at {0} {1}:{2}", CurrentMap.Info.FileName, block.X, block.Y, Info.Name));
                }
            }
        }
        protected override void ProcessAI()
        {
            base.ProcessAI();

            if(!Closed && CloseTime > 0 && CloseTime < Envir.Time)
            {
                CloseDoor();
                CloseTime = 0;
            }
        }

        protected override void ProcessSearch()
        {
            if (Envir.Time < SearchTime) return;

            SearchTime = Envir.Time + SearchDelay;

            if(Closed && AutoOpen)
            {
                var nearby = FindAllNearby(4, CurrentLocation);

                for (int i = 0; i < nearby.Count; i++)
                {
                    if (nearby[i].Race != ObjectType.Player) continue;
                    PlayerObject player = (PlayerObject)nearby[i];

                    if (player.MyGuild == null || player.MyGuild.Conquest == null || player.MyGuild.Conquest != Conquest || player.WarZone) continue;

                    OpenDoor();
                    CloseTime = Envir.Time + (Settings.Second * 10);
                    break;
                }
            }
        }


        public override void Turn(MirDirection dir) { }

        public override bool Walk(MirDirection dir) { return false; }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            if (attacker.Master != null && attacker.Master.Race == ObjectType.Player)
            {
                PlayerObject owner = (PlayerObject)attacker.Master;

                if (owner.MyGuild != null && owner.MyGuild.Conquest != null && owner.MyGuild.Conquest == Conquest) return false;
            }

            return Closed && base.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            if (attacker.MyGuild != null && attacker.MyGuild.Conquest != null && attacker.MyGuild.Conquest == Conquest) return false;

            return Closed && base.IsAttackTarget(attacker);
        }

        protected override void ProcessRoam() { }
        
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
                if (obj == null) continue;
                if (closed)
                    obj.Show();
                else
                    obj.Hide();
            }
        }

    }
}
