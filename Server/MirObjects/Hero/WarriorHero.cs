﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using C = ClientPackets;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirNetwork;
using S = ServerPackets;
using System.Text.RegularExpressions;
using Server.MirObjects.Monsters;

namespace Server.MirObjects
{
    public class WarriorHero : HeroObject
    {
        public WarriorHero(CharacterInfo info, PlayerObject owner) : base(info, owner) { }
        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            if (Info.Thrusting)
            {
                int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
                int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

                if (x > 2 || y > 2) return false;

                return (x <= 1 && y <= 1) || (x == y || x % 2 == y % 2);
            }

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }
        protected override void ProcessFriend()
        {
            if (!CanCast) return;

            if (Target != null)
            {
                UserMagic magic = GetMagic(Spell.Rage);
                if (CanUseMagic(magic) && !HasBuff(BuffType.Rage))
                {
                    BeginMagic(magic.Spell, Direction, ObjectID, CurrentLocation);
                    return;
                }

                magic = GetMagic(Spell.ProtectionField);
                if (CanUseMagic(magic) && !HasBuff(BuffType.ProtectionField))
                {
                    BeginMagic(magic.Spell, Direction, ObjectID, CurrentLocation);
                    return;
                }
            }
        }
        protected override void ProcessAttack()
        {
            if (Target == null || Target.Dead) return;
            TargetDistance = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);

            if (HasMagic(Spell.FlamingSword) && Envir.Time > FlamingSwordTime)
                SpellToggle(Spell.FlamingSword, SpellToggleState.True);
        }
        protected override void Attack()
        {
            if (!Target.IsAttackTarget(Owner))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Spell spell = Spell.None;

            if (TargetDistance > 1 && Info.Thrusting)
                spell = Spell.Thrusting;

            if (spell == Spell.None && Slaying)
                spell = Spell.Slaying;

            if (spell == Spell.None)
            {
                if (Info.HalfMoon)
                    spell = Spell.HalfMoon;

                if (Info.CrossHalfMoon)
                    spell = Spell.CrossHalfMoon;

                if (TwinDrakeBlade)
                    spell = Spell.TwinDrakeBlade;

                if (FlamingSword)
                    spell = Spell.FlamingSword;
            }

            Attack(Direction, spell);
        }
    }
}
