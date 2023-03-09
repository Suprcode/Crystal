using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ManTree : ZumaMonster
    {
        protected internal ManTree(MonsterInfo info)
            : base(info)
        {
        }

        protected override void Attack()
        {
            ShockTime = 0;

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);            

            AttackTime = Envir.Time + AttackSpeed;
            ActionTime = Envir.Time + 300;

            if (Envir.Random.Next(8) > 0)
            {
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);

                if (Envir.Random.Next(4) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 600, Target, damage, DefenceType.ACAgility, false, false);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 600, Target, damage, DefenceType.ACAgility, true, false);
                    ActionList.Add(action);
                }
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 600, Target, damage, DefenceType.ACAgility, false, true);
                ActionList.Add(action);
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool halfMoonAttack = (bool)data[3];
            bool boulderSmashAttack = (bool)data[4];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (halfMoonAttack)
            {
                MirDirection dir = Functions.PreviousDir(Direction);

                for (int i = 0; i < 4; i++)
                {
                    Point halfMoontarget = Functions.PointMove(CurrentLocation, dir, 1);
                    dir = Functions.NextDir(dir);

                    if (!CurrentMap.ValidPoint(halfMoontarget)) continue;

                    Cell cell = CurrentMap.GetCell(halfMoontarget);
                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                        if (!ob.IsAttackTarget(this)) continue;

                        ob.Attacked(this, damage, defence);
                        break;
                    }
                }

                return;
            }

            if (boulderSmashAttack)
            {
                List<MapObject> targets = FindAllTargets(1, target.CurrentLocation);
                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].IsAttackTarget(this))
                    {
                        if (targets[i].Attacked(this, damage, defence) <= 0) continue;
                        PoisonTarget(targets[Envir.Random.Next(targets.Count)], 5, 5, PoisonType.Stun);
                    }
                }

                return;
            }

            target.Attacked(this, damage, defence);
        }
    }
}

