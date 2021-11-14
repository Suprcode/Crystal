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
    public class BoneFamiliar2 : MonsterObject
    {
        public bool Summoned;

        protected internal BoneFamiliar2(MonsterInfo info) : base(info)
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

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + AttackSpeed;

            if (Envir.Random.Next(5) == 1)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.MonsterHalfmoon });

                int damage = 0;

                if (Master != null)
                {
                    damage = GetAttackPower((Master.Stats[Stat.MinSC] * Settings.MinSC / 100) + Stats[Stat.MinDC], (Master.Stats[Stat.MaxSC] * Settings.MaxSC / 100) + Stats[Stat.MaxDC]);
                }
                if (damage == 0) return;

                HalfmoonAttack();

            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                int damage = 0;

                if (Master != null)
                {
                    damage = GetAttackPower((Master.Stats[Stat.MinSC] * Settings.MinSC / 100) + Stats[Stat.MinDC], (Master.Stats[Stat.MaxSC] * Settings.MaxSC / 100) + Stats[Stat.MaxDC]);
                }
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.AC);
                ActionList.Add(action);
            }

            if (Target.Dead)
                FindTarget();

        }


        private void HalfmoonAttack()
        {
            int damage = 0;

            if (Master != null)
            {
                damage = GetAttackPower((Master.Stats[Stat.MinSC] * Settings.MinSC / 100) + Stats[Stat.MinDC], (Master.Stats[Stat.MaxSC] * Settings.MaxSC / 100) + Stats[Stat.MaxDC]);
            }

            MirDirection dir = Functions.PreviousDir(Direction);
            for (int i = 0; i < 3; i++)
            {
                Point location = Functions.PointMove(CurrentLocation, dir, 1);
                if (!CurrentMap.ValidPoint(location))
                    continue;
                Cell cell = CurrentMap.GetCell(location);
                if (cell != null &&
                    cell.Objects != null)
                {
                    for (int x = 0; x < cell.Objects.Count; x++)
                    {
                        if (cell.Objects[x].Race == ObjectType.Player ||
                            cell.Objects[x].Race == ObjectType.Monster)
                        {
                            if (cell.Objects[x].IsAttackTarget(this))
                            {
                                cell.Objects[x].Attacked(this, damage, DefenceType.MAC);
                            }
                        }
                    }
                }
                dir = Functions.NextDir(dir);
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
