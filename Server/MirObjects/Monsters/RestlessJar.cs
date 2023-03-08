using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class RestlessJar : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 6;
            }
        }

        protected override bool CanMove { get { return false; } }
        protected override bool CanRegen { get { return false; } }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected internal RestlessJar(MonsterInfo info)
            : base(info)
        {
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
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!ranged)
            {
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                var hpPercent = (HP * 100) / Stats[Stat.HP];

                switch (Envir.Random.Next(3))
                {
                    case 0: //Spin
                    case 1:
                        {
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                            if (damage == 0) return;

                            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.MACAgility, (int)0);
                            ActionList.Add(action);
                        }
                        break;
                    case 2:
                        {
                            if (hpPercent >= 50) //Tornado
                            {
                                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });

                                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                                if (damage == 0) return;

                                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.MACAgility, (int)1);
                                ActionList.Add(action);
                            }
                            else //Stomp
                            {
                                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });

                                int damage = Stats[Stat.MaxDC];
                                if (damage == 0) return;

                                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.MACAgility, (int)2);
                                ActionList.Add(action);
                            }
                        }
                        break;
                }
            }
            else
            {
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + (AttackSpeed * 2);

                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });

                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;

                ProjectileAttack(damage);
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            int type = (int)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            switch (type)
            {
                case 0:
                    {
                        var targets = FindAllTargets(1, CurrentLocation);

                        for (int i = 0; i < targets.Count; i++)
                        {
                            if (targets[i].Attacked(this, damage, defence) <= 0) continue;
                        }
                    }
                    break;
                case 1:
                    {
                        if (target.Attacked(this, damage, defence) <= 0) return;
                        PoisonTarget(target, 4, 10, PoisonType.Blindness, 1000);
                    }
                    break;
                case 2:
                    {
                        var targets = FindAllTargets(1, CurrentLocation);

                        for (int i = 0; i < targets.Count; i++)
                        {
                            if (targets[i].Attacked(this, damage, defence) <= 0) return;

                            MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, targets[i].CurrentLocation);
                            targets[i].Pushed(this, dir, 1);
                        }
                    }
                    break;
            }
        }
    }
}
