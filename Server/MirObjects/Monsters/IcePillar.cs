﻿using Server.MirDatabase;
using System.Collections.Generic;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class IcePillar : MonsterObject
    {
        protected override bool CanMove { get { return false; } }
        protected override bool CanRegen { get { return false; } }

        protected internal IcePillar(MonsterInfo info)
            : base(info)
        {
            Direction = MirDirection.Up;
        }
        
        protected override void FindTarget() { }

        public override void Turn(MirDirection dir)
        {
        }
        public override bool Walk(MirDirection dir) 
        { 
            return false; 
        }        

        protected override void ProcessRegen() { }
        protected override void ProcessSearch() { }
        protected override void ProcessRoam() { }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            int armour = 0;

            switch (type)
            {
                case DefenceType.ACAgility:
                    if (Envir.Random.Next(Stats[Stat.Agility] + 1) > attacker.Stats[Stat.Accuracy]) return 0;
                    armour = GetAttackPower(Stats[Stat.MinAC], Stats[Stat.MaxAC]);
                    break;
                case DefenceType.AC:
                    armour = GetAttackPower(Stats[Stat.MinAC], Stats[Stat.MaxAC]);
                    break;
                case DefenceType.MACAgility:
                    if (Envir.Random.Next(Stats[Stat.Agility] + 1) > attacker.Stats[Stat.Accuracy]) return 0;
                    armour = GetAttackPower(Stats[Stat.MinMAC], Stats[Stat.MaxMAC]);
                    break;
                case DefenceType.MAC:
                    armour = GetAttackPower(Stats[Stat.MinMAC], Stats[Stat.MaxMAC]);
                    break;
                case DefenceType.Agility:
                    if (Envir.Random.Next(Stats[Stat.Agility] + 1) > attacker.Stats[Stat.Accuracy]) return 0;
                    break;
            }

            if (armour >= damage) return 0;
            
            ShockTime = 0;

            if (attacker.Info.AI == 6)
                EXPOwner = null;
            else if (attacker.Master != null)
            {
                if (EXPOwner == null || EXPOwner.Dead)
                    EXPOwner = attacker.Master switch
                    {
                        HeroObject hero => hero.Owner,
                        _ => attacker.Master
                    };

                if (EXPOwner == attacker.Master)
                    EXPOwnerTime = Envir.Time + EXPOwnerDelay;
            }

            if(Envir.Random.Next(3) == 0)
            {
                CloseAttack(damage);
            }

            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });

            ChangeHP(-1);
            return 1;
        
        }

        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            int armour = 0;

            switch (type)
            {
                case DefenceType.ACAgility:
                    if (Envir.Random.Next(Stats[Stat.Agility] + 1) > attacker.Stats[Stat.Accuracy]) return 0;
                    armour = GetAttackPower(Stats[Stat.MinAC], Stats[Stat.MaxAC]);
                    break;
                case DefenceType.AC:
                    armour = GetAttackPower(Stats[Stat.MinAC], Stats[Stat.MaxAC]);
                    break;
                case DefenceType.MACAgility:
                    if (Envir.Random.Next(Stats[Stat.Agility] + 1) > attacker.Stats[Stat.Accuracy]) return 0;
                    armour = GetAttackPower(Stats[Stat.MinMAC], Stats[Stat.MaxMAC]);
                    break;
                case DefenceType.MAC:
                    armour = GetAttackPower(Stats[Stat.MinMAC], Stats[Stat.MaxMAC]);
                    break;
                case DefenceType.Agility:
                    if (Envir.Random.Next(Stats[Stat.Agility] + 1) > attacker.Stats[Stat.Accuracy]) return 0;
                    break;
            }

            if (armour >= damage) return 0;

            if (damageWeapon)
                attacker.DamageWeapon();

            ShockTime = 0;

            if (Master != null && Master != attacker)
                if (Envir.Time > Master.BrownTime && Master.PKPoints < 200)
                    attacker.BrownTime = Envir.Time + Settings.Minute;

            if (EXPOwner == null || EXPOwner.Dead)
                EXPOwner = GetAttacker(attacker);

            if (EXPOwner == attacker)
                EXPOwnerTime = Envir.Time + EXPOwnerDelay;

            if (Envir.Random.Next(3) == 0)
            {
                CloseAttack(damage);
            }

            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });
            attacker.GatherElement();
            ChangeHP(-1);

            return 1;
        }

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            return 0;
        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true) { }

        private void CloseAttack(int damage)
        {
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            List<MapObject> targets = FindAllTargets(1, CurrentLocation);

            if (targets.Count == 0) return;
            
            for (int i = 0; i < targets.Count; i++)
            {
                Broadcast(new S.ObjectEffect { ObjectID = targets[i].ObjectID, Effect = SpellEffect.IcePillar });

                if (targets[i].Attacked(this, damage, DefenceType.MACAgility) <= 0) continue;

                PoisonTarget(targets[i], 5, GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]), PoisonType.Frozen, 1000);
            }
        }

        public override void Die()
        {
            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);

            List<MapObject> targets = FindAllTargets(7, CurrentLocation, false);

            for (int i = 0; i < targets.Count; i++)
            {
                int delay = Functions.MaxDistance(CurrentLocation, targets[i].CurrentLocation) * 50 + 500; //50 MS per Step

                DelayedAction action = new DelayedAction(DelayedType.Die, Envir.Time + delay, targets[i], damage, DefenceType.ACAgility);
                ActionList.Add(action);
            }
            
            base.Die();
        }

        protected override void CompleteDeath(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            target.Attacked(this, damage, defence);

            PoisonTarget(target, 5, 5, PoisonType.Frozen, 1000);
        }
    }
}
