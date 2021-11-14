using System;
using Server.MirDatabase;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class WizardClone : MonsterObject
    {
        public long FearTime, DecreaseMPTime;
        public byte AttackRange = 6;
        public bool Summoned;

        protected internal WizardClone(MonsterInfo info)
            : base(info)
        {
            Direction = MirDirection.Down;
            Summoned = true;
        }
        public override void RefreshAll()
        {
            if (Master != null)
            {
                Stats[Stat.MinAC] = (ushort)Math.Min(ushort.MaxValue, (Master.Stats[Stat.MinAC] * Settings.MirroringMinAC / 100) + Stats[Stat.MinAC]);
                Stats[Stat.MaxAC] = (ushort)Math.Min(ushort.MaxValue, (Master.Stats[Stat.MaxAC] * Settings.MirroringMaxAC / 100) + Stats[Stat.MaxAC]);
                Stats[Stat.MinMAC] = (ushort)Math.Min(ushort.MaxValue, (Master.Stats[Stat.MinMAC] * Settings.MirroringMinMAC / 100) + Stats[Stat.MinMAC]);
                Stats[Stat.MaxMAC] = (ushort)Math.Min(ushort.MaxValue, (Master.Stats[Stat.MaxMAC] * Settings.MirroringMaxMAC / 100) + Stats[Stat.MaxMAC]);
            }
            AttackSpeed = Info.AttackSpeed;
            MoveSpeed = Info.MoveSpeed;
            base.RefreshAll();
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
            
            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Spell = Spell.ThunderBolt, TargetID = Target.ObjectID, Target = Target.CurrentLocation, Cast = true, Level = 3 });

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = 0;

            if (Master != null)
                damage = GetAttackPower((Master.Stats[Stat.MinMC] * Settings.MirroringMinMC / 100) + Stats[Stat.MinMC], (Master.Stats[Stat.MaxMC] * Settings.MirroringMaxMC / 100) + Stats[Stat.MaxMC]);
            //int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MAC);
            ActionList.Add(action);
        }

        protected override void ProcessAI()
        {
            base.ProcessAI();

            if (Master != null && Master is PlayerObject && Envir.Time > DecreaseMPTime)
            {
                DecreaseMPTime = Envir.Time + 1000;
                if (!Master.Dead) ((PlayerObject)Master).ChangeMP(-10);

                if (((PlayerObject)Master).MP <= 0) Die();
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;
            RefreshAll();

            if (Master != null)
                MoveTo(Master.CurrentLocation);

            if (InAttackRange() && (Master != null || Envir.Time < FearTime))
            {
                Attack();
                return;
            }

            FearTime = Envir.Time + 5000;

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            int dist = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);

            if (dist < AttackRange)
            {
                MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

                if (Walk(dir)) return;

                switch (Envir.Random.Next(2)) //No favour
                {
                    case 0:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.NextDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                    default:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.PreviousDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                }
            }
        }

        public override void Spawned()
        {
            base.Spawned();
            Summoned = false;
        }

        public override void ChangeHP(int amount)
        {
            if (Master != null && Master is PlayerObject)
            {
                ((PlayerObject)Master).ChangeMP(amount);
                return;
            }
            base.ChangeHP(amount);
        }

        public override void Die()
        {
            if (Dead) return;

            HP = 0;
            Dead = true;

            //DeadTime = Envir.Time + DeadDelay;
            DeadTime = 0;

            Broadcast(new S.ObjectDied { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = (byte)(Master != null ? 1 : 0) });

            if (EXPOwner != null && Master == null && EXPOwner.Race == ObjectType.Player) EXPOwner.WinExp(Experience);

            if (Respawn != null)
                Respawn.Count--;

            if (Master == null)
                Drop();

            PoisonList.Clear();
            Envir.MonsterCount--;
            CurrentMap.MonsterCount--;
        }

        public override Packet GetInfo()
        {
            PlayerObject master = null;
            short weapon = -1;
            short armour = 0;
            byte wing = 0;
            if (Master != null && Master is PlayerObject) master = (PlayerObject)Master;
            if (master != null)
            {
                weapon = master.Looks_Weapon;
                armour = master.Looks_Armour;
                wing = master.Looks_Wings;
            }
            return new S.ObjectPlayer
            {
                ObjectID = ObjectID,
                Name = master != null ? master.Name : Name,
                NameColour = NameColour,
                Class = master != null ? master.Class : MirClass.Wizard,
                Gender =  master != null ? master.Gender : MirGender.Male,
                Location = CurrentLocation,
                Direction = Direction,
                Hair = master != null ? master.Hair : (byte)0,
                Weapon = weapon,
                Armour = armour,
                Light = master != null ? master.Light : Light,
                Poison = CurrentPoison,
                Dead = Dead,
                Hidden = Hidden,
                Effect = SpellEffect.None,
                WingEffect = wing,
                Extra = Summoned,
                TransformType = -1
            };
        }
    }
}
