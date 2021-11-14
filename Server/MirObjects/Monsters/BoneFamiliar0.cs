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
    public class BoneFamiliar0 : MonsterObject
    {
        public bool Summoned;

        protected internal BoneFamiliar0(MonsterInfo info) : base(info)
        {
            Direction = MirDirection.DownLeft;
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
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = 0;

            //if (Master != null && Master.Race == ObjectType.Player)
            if (Master != null)
            {
                damage = GetAttackPower((Master.Stats[Stat.MinSC] * Settings.MinSC / 100) + Stats[Stat.MinDC], (Master.Stats[Stat.MaxSC] * Settings.MaxSC / 100) + Stats[Stat.MaxDC]);
            }

            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MAC);
            ActionList.Add(action);

            if (Target.Dead)
                FindTarget();

        }
        public void CrossHalmoonAttack()
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
        public void ThrustingAttack()
        {
            int damage = 0;

            if (Master != null)
            {
                damage = GetAttackPower(Master.Stats[Stat.MinSC] * Settings.MinSC / 100 + Stats[Stat.MinSC], Master.Stats[Stat.MaxDC] * Settings.MaxSC / 100 + Stats[Stat.MaxSC]);
            }

            if (damage == 0)
                return;

            for (int i = 1; i <= 2; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (target == Target.CurrentLocation)
                {
                    if (Target.Attacked(this, damage, DefenceType.MACAgility) > 0 && Envir.Random.Next(8) == 0)
                    {
                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
                        {
                            int poison = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]);

                            Target.ApplyPoison(new Poison
                            {
                                Owner = this,
                                Duration = 5,
                                PType = PoisonType.Green,
                                Value = poison,
                                TickSpeed = 2000
                            }, this);
                        }
                    }
                }
                else
                {
                    if (!CurrentMap.ValidPoint(target))
                        continue;

                    Cell cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null)
                        continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                        {
                            if (!ob.IsAttackTarget(this))
                                continue;

                            if (ob.Attacked(this, damage, DefenceType.MACAgility) > 0 && Envir.Random.Next(8) == 0)
                            {
                                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
                                {
                                    int poison = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]);

                                    ob.ApplyPoison(new Poison
                                    {
                                        Owner = this,
                                        Duration = 5,
                                        PType = PoisonType.Green,
                                        Value = poison,
                                        TickSpeed = 2000
                                    }, this);
                                }
                            }
                        }
                        else
                            continue;

                        break;
                    }
                }
            }
            Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.MonsterThrusting });
        }
        public void HalmoonAttack()
        {
            int damage = 0;

            if (Master != null)
            {
                damage = GetAttackPower(Master.Stats[Stat.MinSC] * Settings.MinSC / 100 + Stats[Stat.MinSC], Master.Stats[Stat.MaxDC] * Settings.MaxSC / 100 + Stats[Stat.MaxSC]);
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
                                cell.Objects[x].Attacked(this, damage, DefenceType.ACAgility);
                            }
                        }
                    }
                }
                dir = Functions.NextDir(dir);
            }
            Broadcast(new S.ObjectEffect { ObjectID = ObjectID, Direction = Direction, Effect = SpellEffect.MonsterHalfmoon });
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                if (Envir.Time > AttackTime)
                {
                    if (Master != null)
                    {
                        if (Master.Race == ObjectType.Player)
                        {
                            PlayerObject ob = (PlayerObject)Master;
                            UserMagic tmp = ob.GetMagic(Spell.SummonSkeleton);
                            if (tmp != null &&
                                tmp.Level >= 3)
                                CrossHalmoonAttack();
                            else
                            Attack();
                        }
                        if (Target.Dead)
                            FindTarget();
                        return;
                    }
                    else
                        Attack();
                    if (Target.Dead)
                        FindTarget();
                    return;
                }
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
