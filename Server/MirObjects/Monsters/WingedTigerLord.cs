using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class WingedTigerLord : MonsterObject
    {
        enum AttackType
        {
            SingleSlash,
            Tornado,
            Stomp
        }

        private bool stomp, tornado;

        private int AttackRange = 5;

        protected internal WingedTigerLord(MonsterInfo info) : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            DelayedAction action;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            int damage = 0;

            if (ranged)
            {
                if (tornado)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0, TargetID = Target.ObjectID });

                    damage = GetAttackPower(MinDC, MaxDC);

                    List<MapObject> targets = FindAllTargets(1, Target.CurrentLocation);

                    for (int i = 0; i < targets.Count; i++)
                    {
                        action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 1000, targets[i], damage, DefenceType.ACAgility);
                        ActionList.Add(action);
                    }

                    ActionTime = Envir.Time + 800;
                    AttackTime = Envir.Time + AttackSpeed;

                    tornado = false;
                    return;
                }
            }

            if (!ranged)
            {
                if (stomp)
                {
                    //Foot stomp
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });

                    MirDirection dir = Functions.PreviousDir(Direction);
                    Point tar;
                    Cell cell;

                    damage = GetAttackPower(MinDC, MaxDC);

                    for (int i = 0; i < 8; i++)
                    {
                        tar = Functions.PointMove(CurrentLocation, dir, 1);
                        dir = Functions.NextDir(dir);

                        if (!CurrentMap.ValidPoint(tar)) continue;

                        cell = CurrentMap.GetCell(tar);

                        if (cell.Objects == null) continue;

                        for (int o = 0; o < cell.Objects.Count; o++)
                        {
                            MapObject ob = cell.Objects[o];
                            if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                            if (!ob.IsAttackTarget(this)) continue;

                            action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility, AttackType.Stomp);
                            ActionList.Add(action);
                            break;
                        }
                    }

                    ActionTime = Envir.Time + 800;
                    AttackTime = Envir.Time + AttackSpeed;

                    stomp = false;
                    return;
                }

                switch (Envir.Random.Next(2))
                {
                    case 0:
                        //Slash
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                        damage = GetAttackPower(MinDC, MaxDC);
                        action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility, AttackType.SingleSlash);
                        ActionList.Add(action);

                        damage = GetAttackPower(MinDC, MaxDC);
                        action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, AttackType.SingleSlash);
                        ActionList.Add(action);
                        break;
                    case 1:
                        //Two hand slash
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                        damage = GetAttackPower(MinDC, MaxDC);
                        action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility, AttackType.SingleSlash);
                        ActionList.Add(action);
                        break;
                }

                if (Envir.Random.Next(5) == 0)
                    stomp = true;

                if (Envir.Random.Next(2) == 0)
                    tornado = true;
            }

            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            int poisonTime = GetAttackPower(MinSC, MaxSC);

            if (target.Attacked(this, damage, defence) <= 0) return;

            if (Envir.Random.Next(Settings.PoisonResistWeight) >= target.PoisonResist)
            {
                if (Envir.Random.Next(2) == 0)
                {
                    target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Stun, Value = poisonTime, TickSpeed = 2000 }, this);
                    Broadcast(new S.ObjectEffect { ObjectID = target.ObjectID, Effect = SpellEffect.Stunned, Time = (uint)poisonTime * 1000 });
                }
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            AttackType type = (AttackType)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            int poisonTime = GetAttackPower(MinSC, MaxSC);

            if (target.Attacked(this, damage, defence) <= 0) return;

            switch (type)
            {
                case AttackType.Stomp:
                    {
                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= target.PoisonResist)
                        {
                            if (Envir.Random.Next(2) == 0)
                            {
                                target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Paralysis, Value = poisonTime, TickSpeed = 2000 }, this);
                            }
                        }
                    }
                    break;
            }
        }
    }
}
