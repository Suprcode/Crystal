using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using S = ServerPackets;
using System.Drawing;
using Server.MirEnvir;

namespace Server.MirObjects.Monsters
{
    public class BoneFamiliar3 : MonsterObject
    {
        public bool Summoned;

        protected internal BoneFamiliar3(MonsterInfo info) : base(info)
        {
            Direction = MirDirection.DownLeft;
        }
        public override void RefreshAll()
        {
            base.RefreshAll();
            if (Master != null)
            {
                Stats[Stat.MinAC] = (ushort)Math.Min(ushort.MaxValue, Master.Stats[Stat.MinAC] * Settings.MinAC / 100 + Stats[Stat.MinAC]);
                Stats[Stat.MaxAC] = (ushort)Math.Min(ushort.MaxValue, Master.Stats[Stat.MaxAC] * Settings.MaxAC / 100 + Stats[Stat.MaxAC]);
                Stats[Stat.MinMAC] = (ushort)Math.Min(ushort.MaxValue, Master.Stats[Stat.MinMAC] * Settings.MinMAC / 100 + Stats[Stat.MinMAC]);
                Stats[Stat.MaxMAC] = (ushort)Math.Min(ushort.MaxValue, Master.Stats[Stat.MaxMAC] * Settings.MaxMAC / 100 + Stats[Stat.MaxMAC]);
                Stats[Stat.HP] = (ushort)Math.Min(ushort.MaxValue, Master.Stats[Stat.HP] * Settings.HP / 100 + Stats[Stat.HP]);
            }
            AttackSpeed = Info.AttackSpeed;
            MoveSpeed = Info.MoveSpeed;
        }

        public override void Spawned()
        {
            base.Spawned();

            Summoned = true;
        }
        protected override void Attack()
        {
            int damage = 0;

            if (Master != null)
            {
                damage = GetAttackPower(Master.Stats[Stat.MinSC] * Settings.MinSC / 100 + Stats[Stat.MinSC], Master.Stats[Stat.MaxDC] * Settings.MaxSC / 100 + Stats[Stat.MaxSC]);
            }

            if (damage == 0) return;

            List<MapObject> targets = FindAllTargets(1, CurrentLocation, false);
            for (int i = 0; i < targets.Count; i++)
                if (targets[i].IsAttackTarget(this))
                    targets[i].Attacked(this, damage, DefenceType.ACAgility);

            Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.MonsterCrossHalmoon });
            AttackTime = Envir.Time + AttackSpeed;
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;
            RefreshAll();

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }

        public override Packet GetInfo()
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Location = CurrentLocation,
                Image = Info.Image,
                Direction = Direction,
                Effect = Info.Effect,
                AI = (byte)Info.AI,
                Light = Info.Light,
                Dead = Dead,
                Skeleton = Harvested,
                Poison = CurrentPoison,
                Hidden = Hidden,
                Extra = Summoned,
            };
        }
    }
}
