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
    public class BoneFamiliar1 : MonsterObject
    {
        public bool Summoned;

        protected virtual byte AttackRange
        {
            get
            {
                return 2;
            }
        }
        protected internal BoneFamiliar1(MonsterInfo info) : base(info)
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

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > AttackRange || y > AttackRange) return false;

            return (x == 0) || (y == 0) || (x == y);
        }

        public override void Spawned()
        {
            base.Spawned();

            Summoned = true;
        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            bool range = !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!range)
            {
                base.Attack();
            }
            else
            {
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.MonsterThrusting });
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                LineAttack(2);
            }


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;


        }

        private void LineAttack(int distance)
        {
            int damage = 0;
            if (Master != null)
            {
                damage = GetAttackPower((Master.Stats[Stat.MinSC] * Settings.MinSC / 100) + Stats[Stat.MinDC], (Master.Stats[Stat.MaxSC] * Settings.MaxSC / 100) + Stats[Stat.MaxDC]);
            }
            if (damage == 0) return;

            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (target == Target.CurrentLocation)
                {
                    Target.Attacked(this, damage, DefenceType.ACAgility);
                }
                else
                {
                    if (!CurrentMap.ValidPoint(target)) continue;

                    Cell cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                        {
                            if (!ob.IsAttackTarget(this)) continue;

                            ob.Attacked(this, damage, DefenceType.ACAgility);
                        }
                        else continue;

                        break;
                    }

                }
            }
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
