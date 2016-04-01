using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class FrostTiger : MonsterObject
    {
        protected byte AttackRange = 6;
        public long SitDownTime;

        protected internal FrostTiger(MonsterInfo info)
            : base(info)
        {
            SitDownTime = NewSitDownTime();
        }

        private bool _sitting;
        public bool Sitting
        {
            get { return _sitting; }
            set
            {
                if (_sitting == value) return;
                _sitting = value;
                Hidden = value;
                CurrentMap.Broadcast(new S.ObjectSitDown { ObjectID = ObjectID, Location = CurrentLocation, Direction = Direction, Sitting = value }, CurrentLocation);
            }

        }

        protected override bool CanAttack
        {
            get
            {
                return !Sitting && base.CanAttack;
            }
        }
        protected override bool CanMove
        {
            get
            {
                return !Sitting && base.CanMove;
            }
        }
        public override bool Walk(MirDirection dir)
        {
            return !Sitting && base.Walk(dir);
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected long NewSitDownTime()
        {
            long newtime = Envir.Time + Envir.Random.Next(1000 * 60 * 2);
            return newtime < SitDownTime ? SitDownTime : newtime;
        }

        protected override void FindTarget() { }

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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (!ranged)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                if (damage == 0) return;

                Target.Attacked(this, damage, DefenceType.ACAgility);
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                AttackTime = Envir.Time + AttackSpeed + 500;
                if (damage == 0) return;

                int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MAC);
                ActionList.Add(action);

                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    if (Envir.Random.Next(8) == 0)
                    {
                        if (Info.Effect == 0)
                        {
                            Target.ApplyPoison(new Poison
                            {
                                Owner = this,
                                Duration = 5,
                                PType = PoisonType.Bleeding,
                                TickSpeed = 1000,
                            }, this);
                        }
                        else if (Info.Effect == 1)
                        {
                            Target.ApplyPoison(new Poison
                            {
                                Owner = this,
                                Duration = 5,
                                PType = PoisonType.Slow,
                                TickSpeed = 1000,
                            }, this);
                        }
                    }
                }
            }

            if (Target.Dead)
                FindTarget();
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (Sitting) Sitting = false;
            SitDownTime = NewSitDownTime();

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

        protected override void ProcessAI()
        {
            if (!Dead && !Sitting && Envir.Time > SitDownTime)
            {
                Sitting = true;
            }

            base.ProcessAI();
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
                Extra = Sitting,
            };
        }
    }
}
