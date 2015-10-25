﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class RedMoonEvil : MonsterObject
    {
        protected override bool CanMove { get { return false; } }
        protected override bool CanRegen { get { return false; } }
        

        protected internal RedMoonEvil(MonsterInfo info) : base(info)
        {
            Direction = MirDirection.Up;

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
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
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.AC:
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.MACAgility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetAttackPower(MinMAC, MaxMAC);
                    break;
                case DefenceType.MAC:
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.Agility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    break;
            }

            if (armour >= damage) return 0;

            ShockTime = 0;

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
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.AC:
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.MACAgility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
                    armour = GetAttackPower(MinMAC, MaxMAC);
                    break;
                case DefenceType.MAC:
                    armour = GetAttackPower(MinAC, MaxAC);
                    break;
                case DefenceType.Agility:
                    if (Envir.Random.Next(Agility + 1) > attacker.Accuracy) return 0;
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
                EXPOwner = attacker;

            if (EXPOwner == attacker)
                EXPOwnerTime = Envir.Time + EXPOwnerDelay;

            Broadcast(new S.ObjectStruck { ObjectID = ObjectID, AttackerID = attacker.ObjectID, Direction = Direction, Location = CurrentLocation });
            attacker.GatherElement();//ArcherSpells - Elemental system
            ChangeHP(-1);

            return 1;
        }

        public override void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false) { }

        protected override void ProcessTarget()
        {
            if (!CanAttack) return;

            List<MapObject> targets = FindAllTargets(Info.ViewRange, CurrentLocation);
            if (targets.Count == 0) return;

            ShockTime = 0;

            Broadcast(new S.ObjectAttack {ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation});
            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];
                Attack();
            }


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
        }

        protected override void Attack()
        {

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.MAC);

            Broadcast(new S.ObjectEffect{ ObjectID = Target.ObjectID, Effect = SpellEffect.RedMoonEvil});
        }

    }
}
