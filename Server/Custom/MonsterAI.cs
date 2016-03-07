using Server.MirDatabase;
using Server.MirObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using S = ServerPackets;
using Server.MirEnvir;

namespace Server.Custom
{
    public class MonsterAI : MonsterObject
    {
        private const byte AttackRange = 9;
        public List<PlayerObject> PlayerTargets = new List<PlayerObject>();
        public List<MonsterObject> PetTargets = new List<MonsterObject>();
        public List<MapObject> AllTargets = new List<MapObject>();
        public DateTime TimeStarted = new DateTime();


        public int SpecialAttackDamage;
        public int MassAttackDamage;
        public int MeleeAttackDamage;
        public int RangeAttackDamage;
        public int MagicAttackDamage;
        public int TargetedDamage;
        public int DamageToPets;
        public MirClass AttackClass;
        public bool AttackPet;
        private const long MassAttackTime = 10000;
        private const long SpecialAttackTime = 20000;
        private const long MeleeAttackTime = 1500;
        private const long MagicAttackTime = 3000;
        private const long RangeAttackTime = 2500;
        private const long PetAttackTime = 5000;
        private const long ResetTime = 30000;

        private long _resetTime;
        private long _massAttkTime;
        private long _speciallAttkTime;
        private long _normalAttackTime;
        private long _magicAttackTime;
        private long _rangeAttackTime;
        string LastPlayerHitter = "";
        Stopwatch sWatch = new Stopwatch();

        protected internal MonsterAI(MonsterInfo info) : base(info)
        {
            if (!uniqueAI.IgnorePets && uniqueAI.DamagePetsMore)    //Pet Damage
                DamageToPets = uniqueAI.PetAttackDamage;
            if (uniqueAI.UseSpecialAttack) // Special Damage
                SpecialAttackDamage = uniqueAI.SpecialAttackDamage;
            if (uniqueAI.UseMassAttack) // Mass Damage
                MassAttackDamage = uniqueAI.MassAttackDamage;
            if (uniqueAI.UseMeleeAttack) // Melee Damage
                MeleeAttackDamage = uniqueAI.MeleeAttackDamage;
            if (uniqueAI.UseRangeAttack) // Range Damage
                RangeAttackDamage = uniqueAI.RangeAttackDamage;
            if (uniqueAI.UseMagicAttack) // Magic Damage
                MagicAttackDamage = uniqueAI.MagicAttackDamage;
            if (uniqueAI.Target) // Damage to specific Target(s)
            {
                AttackClass = (MirClass)uniqueAI.TargetClass;
                TargetedDamage = uniqueAI.TargetAttackDamage;
            }
            uniqueAI.Alive = true;
            if (!uniqueAI.Save(uniqueAI))
                SMain.Enqueue("ERROR saving status");
        }

        bool spawn1 = false;
        bool spawn2 = false;
        bool spawn3 = false;
        bool spawn4 = false;

        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = false)
        {
            LastPlayerHitter = attacker.Name;
            return base.Attacked(attacker, damage, type);
        }

        public void SpawnSlaves(Point spawnLocation)
        {
            int count = Math.Min(8, 40 - SlaveList.Count);

            for (int i = 0; i < count; i++)
            {
                MonsterObject mob = null;
                switch (Envir.Random.Next(uniqueAI.Slaves.Count))
                {
                    case 0:
                        mob = GetMonster(Envir.GetMonsterInfo(uniqueAI.Slaves[0].Name));
                        break;
                    case 1:
                        mob = GetMonster(Envir.GetMonsterInfo(uniqueAI.Slaves[1].Name));
                        break;
                    case 2:
                        mob = GetMonster(Envir.GetMonsterInfo(uniqueAI.Slaves[2].Name));
                        break;
                    case 3:
                        mob = GetMonster(Envir.GetMonsterInfo(uniqueAI.Slaves[3].Name));
                        break;
                    default:
                        break;
                }

                if (mob == null) continue;

                if (!mob.Spawn(CurrentMap, spawnLocation))
                    mob.Spawn(CurrentMap, CurrentLocation);

                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
            }
        }

        public override void ChangeHP(int amount)
        {
            if (PercentHealth <= 90 && !spawn1 && uniqueAI.Slaves.Count >= 1)
            {
                SpawnSlaves(Target.CurrentLocation);
                spawn1 = true;
            }
            if (PercentHealth <= 75 && !spawn2 && uniqueAI.Slaves.Count >= 2)
            {
                SpawnSlaves(Target.CurrentLocation);
                spawn2 = true;
            }
            if (PercentHealth <= 50 && !spawn3 && uniqueAI.Slaves.Count >= 3)
            {
                SpawnSlaves(Target.CurrentLocation);
                spawn3 = true;
            }
            if (PercentHealth <= 25 && !spawn4 && uniqueAI.Slaves.Count >= 4)
            {
                SpawnSlaves(Target.CurrentLocation);
                spawn4 = true;
            }
            base.ChangeHP(amount);
        }

        #region SpecialAttack List Player
        public void SpecialAttack(List<PlayerObject> _targets)
        {
            if (_targets == null) return;
            if (MassAttackDamage <= 0) return;
            Direction = Functions.DirectionFromPoint(CurrentLocation, _targets[0].CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            switch (uniqueAI.MassAttackEffect)
            {
                case 1:
                    Broadcast(new S.ObjectEffect { ObjectID = _targets[0].ObjectID, Effect = SpellEffect.Entrapment });
                    break;
            }
            for (int i = 0; i < _targets.Count; i++)
                SpecialAttack(_targets[i]);
        }
        public void SpecialAttack(PlayerObject _target)
        {
            if (_target == null) return;
            switch (uniqueAI.SpecialAttackEffect)
            {
                case 2:
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                    break;
            }
            _target.Attacked(this, SpecialAttackDamage, DefenceType.None);
        }
        #endregion

        #region SpecialAttack List Mob
        public void SpecialAttack(List<MonsterObject> _targets)
        {
            if (_target == null) return;
            if (!uniqueAI.IgnorePets) SpecialAttackDamage += DamageToPets;
            else FindTarget();
            Direction = Functions.DirectionFromPoint(CurrentLocation, _targets[0].CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            switch (uniqueAI.SpecialAttackEffect)
            {
                case 1:
                    Broadcast(new S.ObjectEffect { ObjectID = _targets[0].ObjectID, Effect = SpellEffect.Entrapment });
                    break;
            }
            for (int i = 0; i < _targets.Count; i++)
                SpecialAttack(_targets[i]);
        }
        public void SpecialAttack(MonsterObject _target)
        {
            if (_target == null || SpecialAttackDamage <= 0) return;
            switch (uniqueAI.SpecialAttackEffect)
            {
                case 2:
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                    break;
            }
            _target.Attacked(this, SpecialAttackDamage, DefenceType.None);
        }
        #endregion

        #region MagicAttack Player
        public void MagicAttack(PlayerObject _target)
        {
            if (_target == null) return;
            Direction = Functions.DirectionFromPoint(CurrentLocation, _target.CurrentLocation);
            AttackTime = Envir.Time + AttackSpeed + 500;
            if (MagicAttackDamage == 0) return;
            switch (uniqueAI.MagicAttackEffect)
            {
                case 1: // needs to be changed
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.Entrapment });
                    break;
                case 2: // FireWorks
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                    break;
                default:
                    break;
            }
            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, _target, MagicAttackDamage, DefenceType.None);
            ActionList.Add(action);
        }
        #endregion

        #region MagicAttack Mob
        public void MagicAttack(MonsterObject _target)
        {
            if (_target == null) return;
            if (!uniqueAI.IgnorePets)
                MagicAttackDamage += DamageToPets;
            else
                FindTarget();

            AttackTime = Envir.Time + AttackSpeed + 500;

            if (MagicAttackDamage == 0) return;
            switch (uniqueAI.MagicAttackEffect)
            {
                case 1:
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.Entrapment });
                    break;
                case 2:
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                    break;
                default:
                    break;
            }

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, _target, MagicAttackDamage, DefenceType.MAC);
            ActionList.Add(action);
        }
        #endregion

        #region MassAttack List Player
        public void MassAttack(List<PlayerObject> _targets)
        {
            if (_targets == null) return;
            if (MassAttackDamage <= 0) return;
            Direction = Functions.DirectionFromPoint(CurrentLocation, _targets[0].CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            switch (uniqueAI.MassAttackEffect)
            {
                case 1:
                    Broadcast(new S.ObjectEffect { ObjectID = _targets[0].ObjectID, Effect = SpellEffect.Entrapment });
                    break;
                default:
                    break;
            }
            for (int i = 0; i < _targets.Count; i++)
                MassAttack(_targets[i]);
        }
        public void MassAttack(PlayerObject _target)
        {
            if (_target == null) return;
            switch (uniqueAI.MassAttackEffect)
            {
                case 2:
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                    break;
                default:
                    break;
            }
            _target.Attacked(this, MassAttackDamage, DefenceType.None);
        }
        #endregion

        #region MassAttack List Mob
        public void MassAttack(List<MonsterObject> _targets)
        {
            if (_targets == null) return;
            if (!uniqueAI.IgnorePets)
                MassAttackDamage += DamageToPets;
            else FindTarget();
            if (MassAttackDamage <= 0) return;
            Direction = Functions.DirectionFromPoint(CurrentLocation, _targets[0].CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            switch (uniqueAI.MassAttackEffect)
            {
                case 1:
                    Broadcast(new S.ObjectEffect { ObjectID = _targets[0].ObjectID, Effect = SpellEffect.Entrapment });
                    break;
                default:
                    break;
            }
            for (int i = 0; i < _targets.Count; i++)
                MassAttack(_targets[i]);
            MassAttackDamage = uniqueAI.MassAttackDamage;
        }

        public void MassAttack(MonsterObject _target)
        {
            if (_target == null) return;
            switch (uniqueAI.MassAttackEffect)
            {
                case 2:
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                    break;
                default:
                    break;
            }
            _target.Attacked(this, MassAttackDamage, DefenceType.None);
        }
        #endregion

        #region RangeAttack Player
        public void RangeAttack(PlayerObject _target)
        {
            if (_target == null) return;
            Direction = Functions.DirectionFromPoint(CurrentLocation, _target.CurrentLocation);
            AttackTime = Envir.Time + AttackSpeed + 500;
            if (RangeAttackDamage == 0) return;
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            switch (uniqueAI.RangeAttackEffect)
            {
                case 1: // needs to be changed
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.Entrapment });
                    break;
                case 2: // FireWorks
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                    break;
                default:
                    break;
            }
            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, _target, RangeAttackDamage, DefenceType.AC);
            ActionList.Add(action);
        }
        #endregion

        #region RangeAttack Mob
        public void RangeAttack(MonsterObject _target)
        {
            if (_target == null) return;
            if (!uniqueAI.IgnorePets) RangeAttackDamage += DamageToPets;
            else FindTarget();
            Direction = Functions.DirectionFromPoint(CurrentLocation, _target.CurrentLocation);

            AttackTime = Envir.Time + AttackSpeed + 500;
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            if (RangeAttackDamage == 0) return;
            switch (uniqueAI.RangeAttackEffect)
            {
                case 1: // needs to be changed
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.Entrapment });
                    break;
                case 2: // FireWorks
                    Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                    break;
                default:
                    break;
            }

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, _target, RangeAttackDamage, DefenceType.AC);
            ActionList.Add(action);
            RangeAttackDamage = uniqueAI.RangeAttackDamage;
        }
        #endregion

        #region MeleeAttack Player
        public void MeleeAttack(PlayerObject _target)
        {
            if (_target == null) return;
            Direction = Functions.DirectionFromPoint(CurrentLocation, _target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            if (MeleeAttackDamage == 0) return;
            if (uniqueAI != null && uniqueAI.MeleeAttackEffect > 0)
            {
                switch (uniqueAI.MeleeAttackEffect)
                {
                    case 1: // needs to be changed
                        Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.Entrapment });
                        break;
                    case 2: // FireWorks
                        Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                        break;
                    default:
                        break;
                }
            }
            _target.Attacked(this, MeleeAttackDamage, DefenceType.None);
            if (_target.Dead)
                FindTarget();
        }
        #endregion

        #region MeleeAttack Mob
        public void MeleeAttack(MonsterObject _target)
        {
            if (_target == null) return;
            if (!uniqueAI.IgnorePets)
                MeleeAttackDamage += DamageToPets;
            else
                return;
            Direction = Functions.DirectionFromPoint(CurrentLocation, _target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            if (MeleeAttackDamage == 0) return;

            if (uniqueAI != null && uniqueAI.MeleeAttackEffect > 0)
            {
                switch (uniqueAI.MeleeAttackEffect)
                {
                    case 1: // Effect 1
                        Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.Entrapment });
                        break;
                    case 2: // Effect 2
                        Broadcast(new S.ObjectEffect { ObjectID = _target.ObjectID, Effect = SpellEffect.GreatFoxSpirit });
                        break;
                    default:
                        break;
                }
            }

            _target.Attacked(this, MeleeAttackDamage, DefenceType.ACAgility);
            if (_target.Dead)
                FindTarget();
            MeleeAttackDamage = uniqueAI.MeleeAttackDamage;
        }
        #endregion


        protected override void ProcessTarget()
        {
            //No Targets
            if (Target == null)
            {
                if (Envir.Time > _resetTime)
                {
                    sWatch.Stop();
                    sWatch.Reset();
                    if (HP < MaxHP)
                        SetHP(MaxHP);
                    //Clear the lists.
                    AllTargets.Clear();
                    PlayerTargets.Clear();
                    PetTargets.Clear();
                    return;
                }
                return;
            }

            #region Reset Stop Watch
            if (sWatch.ElapsedMilliseconds == 0)
            {
                sWatch.Start();
                TimeStarted = DateTime.Now;
            }
            _resetTime = Envir.Time + ResetTime;
            #endregion

            #region Refreshing the Location of the custom Monster
            if (uniqueAI != null)
            {
                uniqueAI.CurrentX = CurrentLocation.X;
                uniqueAI.CurrentY = CurrentLocation.Y;
                uniqueAI.CurrentMap = CurrentMap.Info.Title;
                uniqueAI.CurrentMap = CurrentMap.Info.Title;
                uniqueAI.CurrentX = CurrentLocation.X;
                uniqueAI.CurrentY = CurrentLocation.Y;
                if (Dead)
                    uniqueAI.Alive = false;
                else
                    uniqueAI.Alive = true;
                if (!uniqueAI.Save(uniqueAI))
                    SMain.Enqueue("ERROR saving location 1");
            }
            #endregion

            MapObject weakestTarget = null;


            if (uniqueAI != null && uniqueAI.Target)
            {
                AllTargets.Clear();
                PlayerTargets.Clear();
                AllTargets = FindAllTargets(14, CurrentLocation, false);
                for (int i = 0; i < AllTargets.Count; i++)
                    if (AllTargets[i].Race == ObjectType.Player)
                        PlayerTargets.Add((PlayerObject)AllTargets[i]);
                if (PlayerTargets != null && PlayerTargets.Count > 0)
                    weakestTarget = TargetWeak(PlayerTargets);

                if (weakestTarget != null)
                    Target = weakestTarget;
            }
            AllTargets.Clear();
            PlayerTargets.Clear();
            PetTargets.Clear();


            if (uniqueAI != null &&
                uniqueAI.UseMassAttack &&
                InAttackRange() &&
                CanAttack &&
                Envir.Time > _massAttkTime)
            {
                if (uniqueAI.MassAttackEffect == 1)
                    AllTargets = FindAllTargets(2, Target.CurrentLocation, false); // 3x3 area
                else
                    AllTargets = FindAllTargets((int)AttackRange, CurrentLocation, false); // whole view range
                if (AllTargets != null && AllTargets.Count > 1)
                {
                    for (int i = 0; i < AllTargets.Count; i++)
                    {
                        if (AllTargets[i].Race == ObjectType.Player)
                            PlayerTargets.Add((PlayerObject)AllTargets[i]);
                        if (AllTargets[i].Race == ObjectType.Monster)
                            PetTargets.Add((MonsterObject)AllTargets[i]);
                    }
                    if (PlayerTargets != null && PlayerTargets.Count > 0)
                        MassAttack(PlayerTargets);
                    if (!uniqueAI.IgnorePets && PetTargets != null && PetTargets.Count > 0)
                        MassAttack(PetTargets);
                }
                else
                {
                    if (Target.Race == ObjectType.Player)
                        MassAttack((PlayerObject)Target);
                    else if (Target.Race == ObjectType.Monster)
                        MassAttack((MonsterObject)Target);
                }
                _massAttkTime = Envir.Time + MassAttackTime;
            }
            if (uniqueAI != null &&
                uniqueAI.UseRangeAttack &&
                InAttackRange() &&
                Envir.Time > _rangeAttackTime)
            {
                _rangeAttackTime = Envir.Time + RangeAttackTime;
                if (Target.Race == ObjectType.Player)
                    RangeAttack((PlayerObject)Target);
                else if (Target.Race == ObjectType.Monster)
                    RangeAttack((MonsterObject)Target);
            }


            if (uniqueAI != null &&
                uniqueAI.UseMeleeAttack &&
                CanAttack &&
                InMeleeRange() &&
                Envir.Time > _normalAttackTime)
            {
                _normalAttackTime = Envir.Time + MeleeAttackTime;
                if (Target.Race == ObjectType.Player)
                    MeleeAttack((PlayerObject)Target);
                else if (Target.Race == ObjectType.Monster)
                    MeleeAttack((MonsterObject)Target);
            }

            if (uniqueAI != null &&
                uniqueAI.UseMagicAttack &&
                InAttackRange() &&
                CanAttack &&
                Envir.Time > _magicAttackTime)
            {
                _magicAttackTime = Envir.Time + MagicAttackTime;
                if (Target.Race == ObjectType.Player)
                    MagicAttack((PlayerObject)Target);
                else if (Target.Race == ObjectType.Monster)
                    MagicAttack((MonsterObject)Target);
            }

            AllTargets.Clear();
            PlayerTargets.Clear();
            PetTargets.Clear();
            AllTargets = FindAllTargets(14, CurrentLocation, false);
            if (uniqueAI != null &&
                uniqueAI.UseSpecialAttack &&
                CanAttack &&
                InAttackRange() &&
                Envir.Time > _speciallAttkTime)
            {
                if (uniqueAI.UseMassAttack)
                {
                    if (AllTargets != null && AllTargets.Count > 0)
                    {
                        for (int i = 0; i < AllTargets.Count; i++)
                        {
                            if (AllTargets[i].Race == ObjectType.Player)
                                PlayerTargets.Add((PlayerObject)AllTargets[i]);
                            if (AllTargets[i].Race == ObjectType.Monster)
                                PetTargets.Add((MonsterObject)AllTargets[i]);
                        }
                        if (PlayerTargets != null && PlayerTargets.Count > 0)
                            SpecialAttack(PlayerTargets);
                        if (!uniqueAI.IgnorePets && PetTargets != null && PetTargets.Count > 0)
                            SpecialAttack(PetTargets);
                    }
                }
                else if (InMeleeRange())
                {
                    if (Target.Race == ObjectType.Player)
                        SpecialAttack((PlayerObject)Target);
                    else if (Target.Race == ObjectType.Monster)
                        SpecialAttack((MonsterObject)Target);
                }
                _speciallAttkTime = Envir.Time + SpecialAttackTime;
            }
            MoveTo(Target.CurrentLocation);
        }

        #region Targetting the Weakest player in range
        public PlayerObject TargetWeak(List<PlayerObject> _targets)
        {
            ushort tempHP = 65535;
            int index = -1;
            if (_targets != null &&
                _targets.Count > 0 &&
                uniqueAI != null &&
                uniqueAI.Target)
            {
                for (int i = 0; i < _targets.Count; i++)
                {
                    if (uniqueAI.TargetClass >= 0 && uniqueAI.TargetClass <= 4) // Target selected Class
                    {
                        if (_targets[i].HP < tempHP) //Target lowest HP
                        {
                            tempHP = _targets[i].HP;
                            index = i;
                        }
                    }
                    else if (uniqueAI.TargetClass == 5 && uniqueAI.Target) // 5 = No specific class, just the player with the lowest health.
                    {
                        if (_targets[i].HP < tempHP)
                        {
                            tempHP = _targets[i].HP;
                            index = i;
                        }
                    }
                }
            }
            if (index == -1)
                return null;
            if (_targets[index] != null)
                return _targets[index];
            else
                return null;
        }
        #endregion

        protected override void ProcessPoison()
        {
            PoisonType type = PoisonType.None;
            ArmourRate = 1F;
            DamageRate = 1F;
            for (int i = PoisonList.Count - 1; i >= 0; i--)
            {
                if (Dead) return;
                Poison poison = PoisonList[i];

                if (poison.Owner != null && poison.Owner.Node != null ||
                    (poison.Owner != null && poison.Owner.Node != null && Settings.RemovePoisonMap && poison.Owner.CurrentMap != CurrentMap) ||
                    (poison.Owner != null && poison.Owner.Node == null && Settings.RemovePoisonDistance && !Functions.InRange(CurrentLocation, poison.Owner.CurrentLocation, Globals.DataRange)) ||
                    (poison.Owner != null && poison.Owner.Node == null && !uniqueAI.CanPara) ||
                    (poison.Owner != null && poison.Owner.Node == null && !uniqueAI.CanGreen) ||
                    (poison.Owner != null && poison.Owner.Node == null && !uniqueAI.CanRed))
                {
                    if (PoisonList[i].PType == PoisonType.Paralysis && !uniqueAI.CanPara ||
                        PoisonList[i].PType == PoisonType.LRParalysis && !uniqueAI.CanPara ||
                        PoisonList[i].PType == PoisonType.Red && !uniqueAI.CanRed ||
                        PoisonList[i].PType == PoisonType.Green && !uniqueAI.CanGreen ||
                        PoisonList[i].PType == PoisonType.Frozen && !uniqueAI.CanPara ||
                        PoisonList[i].PType == PoisonType.Bleeding && !uniqueAI.CanGreen ||
                        PoisonList[i].PType == PoisonType.Stun && !uniqueAI.CanPara ||
                        PoisonList[i].PType == PoisonType.Slow && !uniqueAI.CanRed ||
                        PoisonList[i].PType == PoisonType.DelayedExplosion && !uniqueAI.CanGreen)
                        PoisonList.RemoveAt(i);
                    continue;
                }
                type |= poison.PType;
            }
            base.ProcessPoison();
        }

        public override void Die()
        {
            AllTargets.Clear();
            sWatch.Stop();
            if (uniqueAI != null && uniqueAI.AnnounceDeath)
            {
                uniqueAI.CurrentX = CurrentLocation.X;
                uniqueAI.CurrentY = CurrentLocation.Y;
                uniqueAI.CurrentMap = CurrentMap.Info.Title;
                if (uniqueAI.DeadMessage.Length > 0)
                {
                    string origMessage = uniqueAI.DeadMessage;
                    if (uniqueAI.DeadMessage.Contains("{NAME}")) // Mobs Name
                        uniqueAI.DeadMessage = uniqueAI.DeadMessage.Replace("{NAME}", Name);
                    if (uniqueAI.DeadMessage.Contains("{MAP}")) // Map Name (Title)
                        uniqueAI.DeadMessage = uniqueAI.DeadMessage.Replace("{MAP}", CurrentMap.Info.Title);
                    if (uniqueAI.DeadMessage.Contains("{XY}")) // Coords X-Y
                        uniqueAI.DeadMessage = uniqueAI.DeadMessage.Replace("{XY}", string.Format("{0}-{1}", CurrentLocation.X, CurrentLocation.Y));
                    if (uniqueAI.DeadMessage.Contains("{PLAYER}")) // Single Player
                    {
                        if (LastPlayerHitter != "")
                            uniqueAI.DeadMessage = uniqueAI.DeadMessage.Replace("{PLAYER}", LastPlayerHitter);
                    }
                    if (uniqueAI.DeadMessage.Contains("{PLAYERS}"))
                    {
                        List<PlayerObject> tempPlayers = new List<PlayerObject>();
                        AllTargets = FindAllTargets(AttackRange, CurrentLocation, false);
                        string tempString = "";
                        if (AllTargets != null && AllTargets.Count > 0)
                        {
                            for (int i = 0; i < AllTargets.Count; i++)
                            {
                                if (AllTargets[i].Race == ObjectType.Player)
                                {
                                    PlayerObject tempPlayer = (PlayerObject)AllTargets[i];
                                    if (tempPlayer != null)
                                        tempPlayers.Add(tempPlayer);
                                }
                            }
                            if (tempPlayers != null && tempPlayers.Count > 0)
                            {
                                for (int i = 0; i < tempPlayers.Count; i++)
                                {
                                    if (i == tempPlayers.Count)
                                        tempString += "and " + tempPlayers[i].Name;
                                    if (i >= 1)
                                        tempString += ", " + tempPlayers[i].Name;
                                    if (i == 0)
                                        tempString += tempPlayers[i].Name;
                                }
                            }
                            uniqueAI.DeadMessage = uniqueAI.DeadMessage.Replace("{PLAYERS}", tempString);
                        }
                    }
                    if (uniqueAI.DeadMessage.Contains("{TOPDAMAGE}"))
                    {

                    }
                    if (uniqueAI.DeadMessage.Contains("{GROUP}"))
                    {
                        string tempString = "";
                        if (Target.GroupMembers != null && Target.GroupMembers.Count > 0)
                            tempString += string.Format("{0}'s group", Target.GroupMembers[0].Name);
                        else
                            tempString += string.Format("{0}", Target.Name);

                        if (tempString.Length > 0)
                            uniqueAI.DeadMessage = uniqueAI.DeadMessage.Replace("{GROUP}", tempString);
                    }

                    Packet p = new S.Chat { Message = uniqueAI.DeadMessage, Type = ChatType.Announcement };
                    Envir.Broadcast(p);
                    uniqueAI.DeadMessage = origMessage;
                }
            }
            if (uniqueAI.ItemCount > 0)
                Custom_Drop();

            if (uniqueAI != null && uniqueAI.UseKillTimer)
            {
                string tempString = "";
                DateTime tempDateTime = DateTime.Now;
                tempDateTime = tempDateTime.AddDays(uniqueAI.RespawnDay);
                tempDateTime = tempDateTime.AddMonths(uniqueAI.RespawnMonth);
                tempDateTime = tempDateTime.AddYears(uniqueAI.RespawnYear);
                tempDateTime = tempDateTime.AddHours(uniqueAI.RespawnHour);
                tempDateTime = tempDateTime.AddMinutes(uniqueAI.RespawnMinute);


                tempString = string.Format("({0}-{1}-{2} ({3}:{4}))", tempDateTime.Day,
                                                                      tempDateTime.Month,
                                                                      tempDateTime.Year,
                                                                      tempDateTime.Hour,
                                                                      tempDateTime.Minute);
                uniqueAI.Alive = false;
                uniqueAI.KillTimer = tempString;
                if (tempString.Length > 0)
                    if (!uniqueAI.Save(uniqueAI))
                        SMain.Enqueue("Error saving Monsters Kill timer.");
            }
            #region Updated the KillTimer 02-03-2016 - Pete107|Petesn00beh
            if (Envir.CustomAIList != null && Envir.CustomAIList.Count > 0)
            {
                DateTime tempDateTime = DateTime.Now;
                tempDateTime = tempDateTime.AddDays(uniqueAI.RespawnDay);
                tempDateTime = tempDateTime.AddMonths(uniqueAI.RespawnMonth);
                tempDateTime = tempDateTime.AddYears(uniqueAI.RespawnYear);
                tempDateTime = tempDateTime.AddHours(uniqueAI.RespawnHour);
                tempDateTime = tempDateTime.AddMinutes(uniqueAI.RespawnMinute);
                uniqueAI.LastKillDay = tempDateTime.Day;
                uniqueAI.LastKillMonth = tempDateTime.Month;
                uniqueAI.LastKillYear = tempDateTime.Year;
                uniqueAI.LastKillHour = tempDateTime.Hour;
                uniqueAI.LastKillMinute = tempDateTime.Minute;
                if (!uniqueAI.Save(uniqueAI))
                    SMain.Enqueue(string.Format("ERROR saving KillTimer"));
            }
            #endregion
            base.Die();
        }

        public void Custom_Drop()
        {
            //For {ITEMS}
            string tempString = "";
            int l = 0;
            if (uniqueAI != null && uniqueAI.Drops != null && uniqueAI.Drops.Count > 0)
            {
                for (int i = 0; i < uniqueAI.Drops.Count; i++)
                {
                    if (uniqueAI.Drops[i].Name.Length > 0)
                    {
                        for (int j = 0; j < Envir.ItemInfoList.Count; j++)
                        {
                            if (Envir.ItemInfoList[j].Name == uniqueAI.Drops[i].Name)
                            {
                                ItemInfo item = Envir.ItemInfoList[j];
                                if (item != null)
                                {
                                    int rate = uniqueAI.Drops[i].Chance;
                                    if (Envir.Random.Next(0, 100) <= rate)
                                    {
                                        EXPOwner = Target;
                                        UserItem _itemToDrop = Envir.CreateDropItem(item);
                                        if (DropItem(_itemToDrop))
                                        {
                                            if (l == 0)
                                                tempString += _itemToDrop.FriendlyName;
                                            if (l > 0)
                                                tempString += ", " + _itemToDrop.FriendlyName;
                                            l++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (uniqueAI != null)
                {
                    if (uniqueAI.AnnounceDrop)
                    {
                        string origString = uniqueAI.ItemMessage;
                        if (uniqueAI.ItemMessage.Contains("{ITEMS}"))
                            if (tempString.Length > 0)
                            {
                                uniqueAI.ItemMessage = uniqueAI.ItemMessage.Replace("{ITEMS}", tempString);
                                Packet p = new S.Chat { Message = uniqueAI.ItemMessage, Type = ChatType.Announcement };
                                Envir.Broadcast(p);
                                uniqueAI.ItemMessage = origString;
                            }
                    }
                }

            }
            base.Drop();
        }

        public override void Spawned()
        {
            if (uniqueAI != null)
            {
                uniqueAI.CurrentMap = CurrentMap.Info.Title; // Update the Envir list of Custom Monster current location in order for the NPC to get the latest location.
                uniqueAI.CurrentX = CurrentLocation.X;
                uniqueAI.CurrentY = CurrentLocation.Y;
                if (uniqueAI.AnnounceSpawn &&
                    uniqueAI.SpawnMessage.Length > 0)
                {
                    string tempString = uniqueAI.SpawnMessage;
                    if (tempString.Contains("{NAME}"))
                        tempString = tempString.Replace("{NAME}", Info.Name);
                    if (tempString.Contains("{MAP}"))
                        tempString = tempString.Replace("{MAP}", CurrentMap.Info.Title);
                    if (tempString.Contains("{XY}"))
                        tempString = tempString.Replace("{XY}", CurrentLocation.X.ToString() + ":" + CurrentLocation.Y.ToString());
                    Packet p = new S.Chat { Message = tempString, Type = ChatType.Announcement };
                    Envir.Broadcast(p);
                }
            }
            base.Spawned();
        }

        public bool InMeleeRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }
    }
}

