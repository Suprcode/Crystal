using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class PowerBead : MonsterObject
    {
        public bool Summoned;

        protected internal PowerBead(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool CanMove
        {
            get { return false; }
        }

        public override void Turn(MirDirection dir) { }

        public override void Spawned()
        {
            base.Spawned();

            Summoned = true;
        }

        protected override void ProcessSearch()
        {
            if (Envir.Time < SearchTime) return;
            if (Master != null && (Master.PMode == PetMode.MoveOnly || Master.PMode == PetMode.None || Master.PMode == PetMode.FocusMasterTarget)) return;

            SearchTime = Envir.Time + SearchDelay;

            if (Info.Effect == 0)
            {
                var targets = FindAllTargets(Info.ViewRange, CurrentLocation);

                if (targets.Count > 0)
                {
                    Target = targets[Envir.Random.Next(targets.Count)];
                }
            }
            else if (Info.Effect == 1 || Info.Effect == 2)
            {
                if (Owner == null)
                {
                    var friends = FindAllFriends(Info.ViewRange, CurrentLocation, true, false);

                    if (friends.Count > 0)
                    {
                        Target = friends[Envir.Random.Next(friends.Count)];
                    }
                }
                else
                {
                    Target = Owner;
                }
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (Info.Effect == 0 && !Target.IsAttackTarget(this))
            {
                return;
            }
            else if ((Info.Effect == 1 || Info.Effect == 2) && !Target.IsFriendlyTarget(this))
            {
                return;
            }

            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });

            DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 300, Target);

            ActionList.Add(action);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];

            if (target == null || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (Info.Effect == 0)
            {
                if (target.IsAttackTarget(this))
                {
                    var damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    target.Attacked(this, damage, DefenceType.MACAgility);
                }
            }
            else if (Info.Effect == 1)
            {
                if (target.IsFriendlyTarget(this))
                {
                    for (int i = 0; i < target.Buffs.Count; i++)
                    {
                        var buff = target.Buffs[i];

                        if (!buff.Properties.HasFlag(BuffProperty.Debuff)) continue;

                        target.RemoveBuff(buff.Type);
                    }

                    if (target.PoisonList.Count == 0) return;

                    if (target.PoisonList.Any(x => x.PType == PoisonType.DelayedExplosion))
                    {
                        target.ExplosionInflictedTime = 0;
                        target.ExplosionInflictedStage = 0;

                        target.Broadcast(new S.RemoveDelayedExplosion { ObjectID = target.ObjectID });
                    }

                    target.PoisonList.Clear();
                    target.OperateTime = 0;
                }
            }
            else if (Info.Effect == 2)
            {
                if (target.IsFriendlyTarget(this))
                {
                    var protect = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (protect == 0) return;

                    target.AddBuff(BuffType.PowerBeadBuff, this, Info.AttackSpeed, new Stats { [Stat.MaxAC] = protect, [Stat.MaxMAC] = protect });
                    target.OperateTime = 0;
                }
            }
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
                AI = Info.AI,
                Light = Info.Light,
                Dead = Dead,
                Skeleton = Harvested,
                Poison = CurrentPoison,
                Hidden = Hidden,
                Extra = Summoned,
            };
        }

        public static bool SpawnRandom(MonsterObject owner, Point spawn)
        {
            var beads = Envir.MonsterInfoList.Where(x => x.AI == 149).ToList();

            if (beads.Count > 0)
            {
                var randomBead = beads[Envir.Random.Next(beads.Count)];

                var mob = GetMonster(Envir.GetMonsterInfo(randomBead.Name));

                if (mob.Spawn(owner.CurrentMap, spawn))
                {
                    owner.SlaveList.Add(mob);
                    return true;
                }
            }

            return false;
        }
    }
}
