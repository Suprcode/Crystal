﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class HellKeeper : MonsterObject
    {
        protected override bool CanMove { get { return false; } }
        protected override bool CanRegen { get { return false; } }
        

        protected internal HellKeeper(MonsterInfo info) : base(info)
        {
            Direction = MirDirection.Up;
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }

        public override void Turn(MirDirection dir)
        {
        }
        public override bool Walk(MirDirection dir) { return false; }


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

            for (int i = PoisonList.Count - 1; i >= 0; i--)
            {
                if (PoisonList[i].PType != PoisonType.LRParalysis) continue;

                PoisonList.RemoveAt(i);
                OperateTime = 0;
            }

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

            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });

            ChangeHP(armour - damage);
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

            for (int i = PoisonList.Count - 1; i >= 0; i--)
            {
                if (PoisonList[i].PType != PoisonType.LRParalysis) continue;

                PoisonList.RemoveAt(i);
                OperateTime = 0;
            }

            if (Master != null && Master != attacker)
                if (Envir.Time > Master.BrownTime && Master.PKPoints < 200)
                    attacker.BrownTime = Envir.Time + Settings.Minute;

            if (EXPOwner == null || EXPOwner.Dead)
                EXPOwner = GetAttacker(attacker);

            if (EXPOwner == attacker)
                EXPOwnerTime = Envir.Time + EXPOwnerDelay;

            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });
            attacker.GatherElement();

            ChangeHP(armour - damage);
            return 1;
        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true)
        {

        }

        protected override void ProcessTarget()
        {
            if (!CanAttack) return;

            Attack();

            ShockTime = 0;

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
        }

        protected override void Attack()
        {
            byte attacktype1 = (byte)(Envir.Random.Next(3) > 0 ? 0 : 1);

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = attacktype1 });

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, attacktype1);
            ActionList.Add(action);
        }

        protected override void CompleteAttack(IList<object> data)
        {
            byte attackType = (byte)data[0];

            List<MapObject> targets = FindAllTargets(Info.ViewRange, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];

                if (attackType == 0)
                {
                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) continue;

                    Target.Attacked(this, damage);
                }
                else
                {
                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) continue;

                    if (Target.Attacked(this, damage, DefenceType.MACAgility) <= 0) continue;

                    PoisonTarget(Target, 10, damage, PoisonType.Dazed, 1000);
                }
            }
        }
    }
}
