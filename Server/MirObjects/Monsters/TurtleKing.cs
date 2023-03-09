using Server.MirDatabase;
using S = ServerPackets;
using Server.MirEnvir;

namespace Server.MirObjects.Monsters
{
    public class TurtleKing : MonsterObject
    {
        public byte AttackRange = 7;
        public byte CloseRange = 2;

        private byte _stage = 5;

        protected internal TurtleKing(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }
        
        protected override void ProcessAI()
        {
            if (Dead) return;

            if (Stats[Stat.HP] >= 5)
            {
                byte stage = (byte)(HP / (Stats[Stat.HP] / 5));

                if (stage < _stage)
                {
                    SpawnSlaves();
                    _stage = stage;
                }
            }

            base.ProcessAI();
        }
        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, CloseRange);

            if (!ranged)
            {
                switch (Envir.Random.Next(5))
                {
                    case 0:
                    case 1:
                        {
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                            if (damage == 0) return;

                            LineAttack(damage, 2, 300);
                        }
                        break;
                    case 2:
                    case 3:
                        {
                            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                            if (damage == 0) return;

                            LineAttack(damage, 3, 300);
                        }
                        break;
                    case 4:
                        {
                            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                            if (damage == 0) return;

                            DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                            ActionList.Add(action);
                        }
                        break;
                }
            }
            else
            {
                if (!Functions.InRange(CurrentLocation, Target.CurrentLocation, CloseRange) && Envir.Random.Next(4) == 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                    Point target = Functions.PointMove(CurrentLocation, Direction, 1);
                    Target.Teleport(CurrentMap, target, true, 6);
                }
                else if (!Functions.InRange(CurrentLocation, Target.CurrentLocation, CloseRange) && Envir.Random.Next(4) == 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });

                    Point target = Functions.PointMove(Target.CurrentLocation, Target.Direction, 1);
                    Teleport(CurrentMap, target, true, 6);
                }
                else
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                    ActionList.Add(action);
                }
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (target.Attacked(this, damage, defence) <= 0) return;

            PoisonTarget(target, 8, 3, PoisonType.Dazed, 1000);
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            List<MapObject> targets = FindAllTargets(AttackRange, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                Broadcast(new S.ObjectEffect { ObjectID = Target.ObjectID, Effect = SpellEffect.TurtleKing });
                if (targets[i].Attacked(this, damage, DefenceType.MAC) <= 0) return;

                PoisonTarget(targets[i], 5, 15, PoisonType.Slow, 1000);
                PoisonTarget(targets[i], 5, 5, PoisonType.Paralysis, 1000);
            }
        }

        private void SpawnSlaves()
        {
            int count = Math.Min(8, 30 - SlaveList.Count);

            for (int i = 0; i < count; i++)
            {
                MonsterObject mob = null;
                switch (Envir.Random.Next(7))
                {
                    case 0:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle1));
                        break;
                    case 1:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle2));
                        break;
                    case 2:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle3));
                        break;
                    case 3:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle4));
                        break;
                    case 4:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Turtle5));
                        break;
                }

                if (mob == null) continue;

                if (!mob.Spawn(CurrentMap, Front))
                    mob.Spawn(CurrentMap, CurrentLocation);

                //mob.Master = this;
                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
            }
        }
    }
}
