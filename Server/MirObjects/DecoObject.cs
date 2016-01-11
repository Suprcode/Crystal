using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects
{
    class DecoObject : MapObject
    {
        public override ObjectType Race
        {
            get { return ObjectType.Deco; }
        }

        public override string Name { get; set; }
        public override int CurrentMapIndex { get; set; }
        public override Point CurrentLocation { get; set; }
        public override MirDirection Direction { get; set; }
        public override ushort Level { get; set; }
        public override bool Blocking
        {
            get
            {
                return false;
            }
        }

        public ushort Image;

        public override uint Health
        {
            get { throw new NotSupportedException(); }
        }
        public override uint MaxHealth
        {
            get { throw new NotSupportedException(); }
        }


        public override void Process()
        {           
            //Cell cell = CurrentMap.GetCell(CurrentLocation);
            //for (int i = 0; i < cell.Objects.Count; i++)
            //    ProcessDeco(cell.Objects[i]);
        }

        public override void SetOperateTime()
        {
            long time = Envir.Time + 2000;

            //if (TickTime < time && TickTime > Envir.Time)
            //    time = TickTime;

            if (OwnerTime < time && OwnerTime > Envir.Time)
                time = OwnerTime;

            if (ExpireTime < time && ExpireTime > Envir.Time)
                time = ExpireTime;

            if (PKPointTime < time && PKPointTime > Envir.Time)
                time = PKPointTime;

            if (LastHitTime < time && LastHitTime > Envir.Time)
                time = LastHitTime;

            if (EXPOwnerTime < time && EXPOwnerTime > Envir.Time)
                time = EXPOwnerTime;

            if (BrownTime < time && BrownTime > Envir.Time)
                time = BrownTime;

            for (int i = 0; i < ActionList.Count; i++)
            {
                if (ActionList[i].Time >= time && ActionList[i].Time > Envir.Time) continue;
                time = ActionList[i].Time;
            }

            for (int i = 0; i < PoisonList.Count; i++)
            {
                if (PoisonList[i].TickTime >= time && PoisonList[i].TickTime > Envir.Time) continue;
                time = PoisonList[i].TickTime;
            }

            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].ExpireTime >= time && Buffs[i].ExpireTime > Envir.Time) continue;
                time = Buffs[i].ExpireTime;
            }

            if (OperateTime <= Envir.Time || time < OperateTime)
                OperateTime = time;
        }

        public override void Process(DelayedAction action)
        {
            throw new NotSupportedException();
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            throw new NotSupportedException();
        }
        public override bool IsAttackTarget(MonsterObject attacker)
        {
            throw new NotSupportedException();
        }
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            throw new NotSupportedException();
        }
        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            throw new NotSupportedException();
        }

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            throw new NotSupportedException();
        }
        public override bool IsFriendlyTarget(PlayerObject ally)
        {
            throw new NotSupportedException();
        }
        public override bool IsFriendlyTarget(MonsterObject ally)
        {
            throw new NotSupportedException();
        }
        public override void ReceiveChat(string text, ChatType type)
        {
            throw new NotSupportedException();
        }

        public override Packet GetInfo()
        {
            return new S.ObjectDeco
            {
                ObjectID = ObjectID,
                Location = CurrentLocation,
                Image = Image
            };
        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true)
        {
            throw new NotSupportedException();
        }
        public override void Die()
        {
            throw new NotSupportedException();
        }
        public override int Pushed(MapObject pusher, MirDirection dir, int distance)
        {
            throw new NotSupportedException();
        }
        public override void SendHealth(PlayerObject player)
        {
            throw new NotSupportedException();
        }
        public override void Despawn()
        {
            base.Despawn();
        }
    }
}
