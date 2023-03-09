using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    //TODO - Teleport
    public class SnowWolfKing : MonsterObject
    {
        private bool _SpawnedSlaves;

        protected internal SnowWolfKing(MonsterInfo info)
            : base(info)
        {
        }

        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            int attackerDamage = base.Attacked(attacker, damage, type, damageWeapon);

            int ownDamage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);

            if (attackerDamage > ownDamage && Envir.Random.Next(2) == 0)
            {
                FindWeakerTarget();
            }

            return attackerDamage;
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            int attackerDamage = base.Attacked(attacker, damage, type);

            int ownDamage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);

            if (attackerDamage > ownDamage && Envir.Random.Next(10) == 0)
            {
                FindWeakerTarget();
            }

            return attackerDamage;
        }

        private void FindWeakerTarget()
        {
            List<MapObject> targets = FindAllTargets(Info.ViewRange, CurrentLocation);

            if (targets.Count < 2) return;

            var newTarget = Target;

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].Stats[Stat.MinDC] >= Target.Stats[Stat.MinDC]) continue;

                newTarget = targets[i];
            }

            if (newTarget != Target)
            {
                Target = newTarget;
                TeleportToTarget(Target);
            }
        }

        private bool TeleportToTarget(MapObject target)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, target.CurrentLocation);

            var reverse = Functions.ReverseDirection(Direction);

            var point = Functions.PointMove(target.CurrentLocation, reverse, 1);

            if (point != CurrentLocation)
            {
                if (Teleport(CurrentMap, point, true, 11)) return true;
            }

            return false;
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (Envir.Random.Next(3) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false, false);
                ActionList.Add(action);
            }
            else
            {
                if (HealthPercent >= 60)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, true, false);
                    ActionList.Add(action);
                }
                else if (HealthPercent >= 30)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false, true);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 3 });
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.ACAgility, false, false);
                    ActionList.Add(action);
                }
            }

            if (HealthPercent < 70 && !_SpawnedSlaves)
            {
                SpawnSlaves();
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            target.Attacked(this, damage, defence);
        }

        public override void Die()
        {
            ActionList.Add(new DelayedAction(DelayedType.Die, Envir.Time + 500));
            base.Die();
        }

        protected override void CompleteDeath(IList<object> data)
        {
            var targets = FindAllTargets(1, CurrentLocation, false);

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage > 0)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].Attacked(this, damage, DefenceType.MAC);
                }
            }

            for (int i = 0; i < SlaveList.Count; i++)
            {
                var mob = SlaveList[i];

                if (EXPOwner != null)
                {
                    if (EXPOwner.Pets.Count >= 6) break;

                    mob.Master = EXPOwner;
                    mob.BroadcastHealthChange();
                    EXPOwner.Pets.Add(mob);

                    mob.Target = null;
                    mob.RageTime = 0;
                    mob.ShockTime = 0;
                    mob.OperateTime = 0;

                    if (!Settings.PetSave)
                    {
                        mob.TameTime = Envir.Time + (Settings.Minute * 60);
                    }

                    mob.Broadcast(new S.ObjectName { ObjectID = mob.ObjectID, Name = mob.Name });
                }
            }

            SlaveList.Clear();
        }

        private void SpawnSlaves()
        {
            _SpawnedSlaves = true;

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            for (int i = 0; i < 3; i++)
            {
                MonsterObject mob = GetMonster(Envir.GetMonsterInfo(Settings.SnowWolfKingMob));
                if (mob == null) continue;

                if (!mob.Spawn(CurrentMap, Target.Back))
                    mob.Spawn(CurrentMap, CurrentLocation);

                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
            }
        }
    }
}
