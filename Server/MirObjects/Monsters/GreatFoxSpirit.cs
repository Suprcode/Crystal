using System.Collections.Generic;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class GreatFoxSpirit : MonsterObject
    {
        private byte _stage;
        private long RecallTime;
        private byte AttackRange = 7;

        protected override bool CanMove { get { return false; } }

        protected internal GreatFoxSpirit(MonsterInfo info)
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

            if (MaxHP >= 4)
            {
                byte stage = (byte)(4 - (HP / (MaxHP / 4)));

                if (stage > _stage)
                {
                    _stage = stage;
                    Broadcast(GetInfo());
                }
            }

            base.ProcessAI();
        }


        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        protected override void ProcessRoam() { }

        protected override void ProcessTarget()
        {
            if (Target == null) return;
            //remark: does this mean nobody gets teleported if the main target is standing closeby + does it mean it always try's to teleport the person with lowest x/y coords?)
            if (Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) > 3 && Envir.Random.Next(10) == 0 && Envir.Time >= RecallTime)
            {
                RecallTime = Envir.Time + 10000;
                List<MapObject> targets = FindAllTargets(30, CurrentLocation);
                if (targets.Count != 0 && Envir.Random.Next(4) > 0)
                {
                    for (int i = 0; i < targets.Count; i++)
                    {
                        if (Functions.MaxDistance(CurrentLocation, targets[i].CurrentLocation) > 3)
                        {
                            if (Envir.Random.Next(Settings.MagicResistWeight) < targets[i].MagicResist) continue;
                            if (!targets[i].Teleport(CurrentMap, Functions.PointMove(CurrentLocation, (MirDirection)((byte)Envir.Random.Next(7)), 1)))
                            targets[i].Teleport(CurrentMap, CurrentLocation);
                            return;
                        }
                    }
                }
            }

            if (InAttackRange() && CanAttack)
            {
                Attack();

                if (Target.Dead)
                    FindTarget();

                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            bool ranged = CurrentLocation == Target.CurrentLocation || Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) > 2;
            
            List<MapObject> targets = FindAllTargets(ranged ? AttackRange : 2, CurrentLocation);
            if (targets.Count == 0) return;

            if (ranged) Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            else Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];
                if (ranged) Broadcast(new S.ObjectEffect { ObjectID = Target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                if (Target.Attacked(this, damage, DefenceType.MAC) <= 0) return;

                if (Envir.Random.Next(5) == 0)
                    Target.ApplyPoison(new Poison { Owner = this, Duration = 15, PType = PoisonType.Slow, TickSpeed = 1000 }, this);
                if (Envir.Random.Next(15) == 0)
                    Target.ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, this);
            }
        }

        public override void Die()
        {
            if (Dead) return;
            ChangeGuardianState(false);
            base.Die();
        }

        public override void Spawned()
        {
            base.Spawned();
            ChangeGuardianState(true);
        }

        private void ChangeGuardianState(bool Active)
        {
            int ExpectedDistance = 20;
            for (int y = CurrentLocation.Y - ExpectedDistance; y <= CurrentLocation.Y + ExpectedDistance; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = CurrentLocation.X - ExpectedDistance; x <= CurrentLocation.X + ExpectedDistance; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;

                    Cell cell = CurrentMap.GetCell(x, y);

                    if (!cell.Valid || cell.Objects == null) continue;

                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        GuardianRock target = cell.Objects[i] as GuardianRock;
                        if (target == null) continue;
                        target.Active = Active;
                    }
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
                ExtraByte = _stage,
            };
        }
    }
}
