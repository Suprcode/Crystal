﻿using System;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class TucsonEgg : MonsterObject
    {
        protected override bool CanMove { get { return false; } }


        protected internal TucsonEgg(MonsterInfo info)
            : base(info)
        {
        }

        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return base.IsAttackTarget(attacker);
        }
        public override bool IsAttackTarget(PlayerObject attacker)
        {
            return base.IsAttackTarget(attacker);
        }

        protected override void ProcessRoam() { }

        protected override void ProcessSearch()
        {
            base.ProcessSearch();
        }

        public override Packet GetInfo()
        {
            return base.GetInfo();
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            int armour = 0;

            switch (type)
            {
                case DefenceType.ACAgility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetDefencePower(MinAC, MaxAC);
                    break;
                case DefenceType.AC:
                    armour = GetDefencePower(MinAC, MaxAC);
                    break;
                case DefenceType.MACAgility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetDefencePower(MinMAC, MaxMAC);
                    break;
                case DefenceType.MAC:
                    armour = GetDefencePower(MinMAC, MaxMAC);
                    break;
                case DefenceType.Agility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
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
                    EXPOwner = attacker.Master;

                if (EXPOwner == attacker.Master)
                    EXPOwnerTime = Envir.Time + EXPOwnerDelay;

            }

            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });

            ChangeHP(-1);
            return 1;
        }
        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            int armour = 0;

            switch (type)
            {
                case DefenceType.ACAgility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetDefencePower(MinAC, MaxAC);
                    break;
                case DefenceType.AC:
                    armour = GetDefencePower(MinAC, MaxAC);
                    break;
                case DefenceType.MACAgility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetDefencePower(MinMAC, MaxMAC);
                    break;
                case DefenceType.MAC:
                    armour = GetDefencePower(MinMAC, MaxMAC);
                    break;
                case DefenceType.Agility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
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
                EXPOwner = attacker;

            if (EXPOwner == attacker)
                EXPOwnerTime = Envir.Time + EXPOwnerDelay;

            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });
            attacker.GatherElement();
            ChangeHP(-1);

            return 1;
        }

        protected override void ProcessTarget()
        {
            if (Target == null)
                return;

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

        }

        protected override void CompleteAttack(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(1, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;

                if (targets[i].Attacked(this, damage, DefenceType.MAC) <= 0) return;

                if (Envir.Random.Next(3) == 0)
                    targets[i].ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 2000 }, this);
            }

        }

        public override void Die()
        {
            ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + 300));
            base.Die();
        }
    }
}
