using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;


namespace Server.MirObjects.Monsters
{
    public class FurbolgWarrior : MonsterObject
    {

        protected internal FurbolgWarrior(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 2 || y > 2) return false;

            return (x <= 1 && y <= 1) || (x == y || x % 2 == y % 2);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;

            bool isCrit = false;
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            int critBonus = (int)Math.Round((double)damage * 0.5);

            if (ranged)
            {
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                for (int i = 1; i <= 2; i++)
                {
                    if (Envir.Random.Next(10) == 0)
                        isCrit = true;

                    if (isCrit)
                        damage += critBonus;
                    
                    Point target = Functions.PointMove(CurrentLocation, Direction, i);

                    if (!CurrentMap.ValidPoint(target)) continue;

                    Cell cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                        {
                            if (!ob.IsAttackTarget(this)) continue;
                            int delay = Functions.MaxDistance(CurrentLocation, ob.CurrentLocation) * 50 + 500;
                            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, ob, damage, DefenceType.ACAgility);
                            ActionList.Add(action);

                            if (isCrit)
                            {
                                DelayedAction action1 = new DelayedAction(DelayedType.SpellEffect, Envir.Time + delay, ob, SpellEffect.FurbolgWarriorCritical);
                                ActionList.Add(action1);
                            }
                        }
                        else
                            continue;

                        break;
                    }
                    if (isCrit)
                        isCrit = false;
                }
            }
            else
            {
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1});

                MirDirection dir = Functions.PreviousDir(Direction);

                for (int i = 0; i < 6; i++)
                {
                    if (Envir.Random.Next(10) == 0)
                        isCrit = true;

                    if (isCrit)
                        damage += critBonus;

                    Point target = Functions.PointMove(CurrentLocation, dir, 1);
                    dir = Functions.NextDir(dir);

                    if (!CurrentMap.ValidPoint(target)) continue;

                    Cell cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                        if (!ob.IsAttackTarget(this)) continue;

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 600, ob, damage, DefenceType.ACAgility);
                        ActionList.Add(action);

                        if (isCrit)
                        {
                            DelayedAction action1 = new DelayedAction(DelayedType.SpellEffect, Envir.Time + 600, ob, SpellEffect.FurbolgWarriorCritical);
                            ActionList.Add(action1);
                        }
                        break;
                    }
                    if (isCrit)
                        isCrit = false;

                }
            }
        }
    }
}
