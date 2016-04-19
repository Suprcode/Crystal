using System.Collections.Generic;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class EvilMir : MonsterObject
    {
        public bool Sleeping;
        private bool MassAttack;
        public long WakeUpTime;
        public override MapObject Target
        {
            get { return _target; }
            set
            {
                if (_target == value) return;
                _target = value;

                if (value == null && DragonLink) Envir.DragonSystem.DeLevelTime = Envir.Time + Envir.DragonSystem.DeLevelDelay;
            }
        }

        private bool _dragonlink;
        public bool DragonLink
        {
            get { return _dragonlink && Envir.DragonSystem != null; }
            set
            {
                if (_dragonlink == value) return;

                _dragonlink = value;
            }
        }

        protected override bool CanAttack
        {
            get
            {
                return !Sleeping && base.CanAttack;
            }
        }
        protected override bool CanMove { get { return false; } }

        protected internal EvilMir(MonsterInfo info)
            : base(info)
        {
            Direction = MirDirection.Up;
        }

        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return !Sleeping && base.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            return !Sleeping && base.IsAttackTarget(attacker);
        }
        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }

        protected override void ProcessRoam() { }

        protected override void ProcessSearch()
        {
            if (!Sleeping)
                base.ProcessSearch();
        }

        protected override void ProcessAI()
        {
            if (!Dead && Sleeping && Envir.Time > WakeUpTime)
            {
                Sleeping = false;
                HP = MaxHP;
                return;
            }

            base.ProcessAI();
        }

        protected override void CompleteAttack(IList<object> data)
        {
            if (Target == null) return;

            if (!Target.IsAttackTarget(this) || Target.CurrentMap != CurrentMap || Target.Node == null) return;

            List<MapObject> targets = MassAttack ? FindAllTargets(17/*huge range so it even hits ppl with bigger resolutions*/, CurrentLocation, false) : FindAllTargets(2, Target.CurrentLocation, false);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];
                Attack();
            }
        }
        protected override void ProcessTarget()
        {
            if (Target == null) return;
            if (!CanAttack || !InAttackRange()) return;

            ShockTime = 0;
            if (DragonLink)
            {
                if (Envir.DragonSystem.Info.Level < Globals.MaxDragonLevel)
                    Envir.DragonSystem.DeLevelTime = Envir.Time + Envir.DragonSystem.DeLevelDelay;
                else
                    Envir.DragonSystem.DeLevelTime = Envir.Time + (6 * Envir.DragonSystem.DeLevelDelay);
            }

            byte random = DragonLink ? (byte)(Envir.DragonSystem.MaxLevel + 3 - Envir.DragonSystem.Info.Level) : (byte)8;

            if (Envir.Random.Next(random) > 0 /*&& Target.CurrentLocation.Y >= CurrentLocation.Y - 1*/)//in theory it shouldnt fire 'behind' it, but it should shoot at stuff in it's top left corner (and this code made it only hit below him not 'infront' of him)
            {
                MassAttack = false;
                Direction = SetDirection(Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation));
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 620; //50 MS per Step

                ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + delay));
            }
            else
            {
                MassAttack = true;
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + 500));
            }
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
        }

        protected override void Attack()
        {
            int damage = GetAttackPower(MinDC, DragonLink ? MaxDC + (Envir.DragonSystem.Info.Level - 1 * 10) : MaxDC);
            if (!MassAttack)
                damage = (int)(damage * 0.75);//make mass attacking do slightly more dmg then targeted
            if (damage == 0) return;

            if (Target.Attacked(this, damage, DefenceType.MAC) <= 0) return;

            if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
            {
                if (Envir.Random.Next(5) == 0)
                    Target.ApplyPoison(new Poison { Owner = this, Duration = 15, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
                if (Envir.Random.Next(15) == 0)
                    Target.ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, this);
            }
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            return Sleeping ? 0 : base.Attacked(attacker, damage, type);
        }

        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            return Sleeping ? 0 : base.Attacked(attacker, damage, type, damageWeapon);
        }

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            return 0;
        }

        public override void ChangeHP(int amount)
        {
            if (DragonLink && amount < 0) Envir.DragonSystem.GainExp(Envir.Random.Next(1, 40));
            base.ChangeHP(amount);
        }

        public override void Die()
        {
            if (Dead || Sleeping) return;

            if (!DragonLink) base.Die();
            else
            {
                if (Info.HasDieScript && (SMain.Envir.MonsterNPC != null))
                {
                    SMain.Envir.MonsterNPC.Call(this,string.Format("[@_DIE({0})]", Info.Index));
                }
                Envir.DragonSystem.GainExp(250);//why would hitting em give you so little 'points', while hitting them gives so much
                Sleeping = true;
                WakeUpTime = Envir.Time + 5 * (60 * 1000);
            }

        }

        public MirDirection SetDirection(MirDirection dir)
        {
            switch (dir)
            {
                case MirDirection.DownRight:
                case MirDirection.Right:
                    return MirDirection.Up;
                case MirDirection.Left:
                case MirDirection.UpLeft:
                    return MirDirection.Right;
                default:
                    return MirDirection.UpRight;
            }
        }
    }
}
