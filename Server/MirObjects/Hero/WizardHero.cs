using Server.MirDatabase;

namespace Server.MirObjects
{
    public class WizardHero : HeroObject
    {
        public WizardHero(CharacterInfo info, PlayerObject owner) : base(info, owner) { }
        public int SurroundedCount;
        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            if (HasRangedSpell)
                return TargetDistance <= ViewRange;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }
        protected override void ProcessFriend()
        {
            if (!CanCast) return;

            if (Target != null)
            {
                UserMagic magic = GetMagic(Spell.MagicShield);
                if (CanUseMagic(magic) && !HasBuff(BuffType.MagicShield))
                {
                    BeginMagic(magic.Spell, Direction, ObjectID, CurrentLocation);
                    return;
                }

                magic = GetMagic(Spell.MagicBooster);
                if (CanUseMagic(magic) && !HasBuff(BuffType.MagicBooster))
                {
                    BeginMagic(magic.Spell, Direction, ObjectID, CurrentLocation);
                    return;
                }
            }
        }

        protected override void ProcessAttack()
        {
            if (!CanCast || Target == null || Target.Dead) return;
            TargetDistance = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);
            SurroundedCount = FindAllTargets(2, CurrentLocation).Count;
            if (!HasRangedSpell) return;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            UserMagic magic;

            if (TargetDistance == 1)
            {
                if (Target.Level < Level)
                {
                    magic = GetMagic(Spell.Repulsion);
                    if (CanUseMagic(magic))
                    {
                        BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                        return;
                    }
                }
            }

            if (TargetDistance < 3 && SurroundedCount > 1)
            {
                magic = GetMagic(Spell.FlameField);
                if (CanUseMagic(magic))
                {
                    BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                    return;
                }

                magic = GetMagic(Spell.ThunderStorm);
                if (CanUseMagic(magic))
                {
                    BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                    return;
                }
            }

            magic = GetMagic(Spell.FlameDisruptor);
            if (CanUseMagic(magic))
            {
                BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                return;
            }

            magic = GetMagic(Spell.Vampirism);
            if (CanUseMagic(magic))
            {
                BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                return;
            }

            magic = GetMagic(Spell.FrostCrunch);
            if (CanUseMagic(magic))
            {
                BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                return;
            }

            magic = GetMagic(Spell.ThunderBolt);
            if (CanUseMagic(magic))
            {
                BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                return;
            }

            magic = GetMagic(Spell.FireBall);
            if (CanUseMagic(magic))
            {
                BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                return;
            }
        }

        protected override void ProcessTarget()
        {
            if (CanCast && NextMagicSpell != Spell.None)
            {
                Magic(NextMagicSpell, NextMagicDirection, NextMagicTargetID, NextMagicLocation);
                NextMagicSpell = Spell.None;
            }

            if (Target == null || !CanAttack) return;            

            if (!HasRangedSpell && InAttackRange())
            {
                Attack();

                if (Target != null && Target.Dead)
                {
                    FindTarget();
                }

                return;
            }

            if (!HasRangedSpell)
                MoveTo(Target.CurrentLocation);
        }

        private bool HasRangedSpell => Info.Magics.Select(x => x.Spell).Intersect(Globals.RangedSpells).Any();
    }
}
