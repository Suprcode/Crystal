using Server.MirDatabase;

namespace Server.MirObjects
{
    public class WizardHero : HeroObject
    {
        public WizardHero(CharacterInfo info, PlayerObject owner) : base(info, owner) { }
        public int SurroundedCount;
        public int TargetSurroundedCount;
        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            if (HasRangedSpell && CanCast)
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
            TargetSurroundedCount = FindAllTargets(1, Target.CurrentLocation).Count;
            if (!HasRangedSpell) return;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            UserMagic magic;

            if (InAttackRange())
            {
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

                if (TargetSurroundedCount > 1)
                {
                    magic = GetMagic(Spell.IceStorm);
                    if (CanUseMagic(magic))
                    {
                        BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                        return;
                    }

                    magic = GetMagic(Spell.FireBang);
                    if (CanUseMagic(magic))
                    {
                        BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                        return;
                    }
                }

                if (Target.Undead == true && Target.Level < Level)
                {
                    magic = GetMagic(Spell.TurnUndead);
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

                magic = GetMagic(Spell.GreatFireBall);
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

                magic = GetMagic(Spell.None);
                {
                    return;
                }
            }
            
            magic = GetMagic(Spell.None);
            {
                return;
            }
        }

        protected override void ProcessTarget()
        {
            if (Target != null && Owner.PMode == PetMode.FocusMasterTarget && !Target.IsAttackTarget(Owner))
            {
                Target = null;
                return;
            }

            if (CanMove && ((!CanCast || NextMagicSpell == Spell.None) && Owner.Info.HeroBehaviour == HeroBehaviour.CounterAttack))
            {
                MoveTo(Owner.Back);
                return;
            }

            if (Target == null || !CanAttack) return;

            if (CanCast && NextMagicSpell != Spell.None)
            {
                Magic(NextMagicSpell, NextMagicDirection, NextMagicTargetID, NextMagicLocation);
                NextMagicSpell = Spell.None;
            }            

            if (CanAttack && (!HasRangedSpell && InAttackRange() || NextMagicSpell == Spell.None && Owner.Info.HeroBehaviour == HeroBehaviour.Attack && TargetDistance == 1))
            {
                Attack();

                if (Target != null && Target.Dead)
                {
                    FindTarget();
                }

                return;
            }

            if (CanMove && (!HasRangedSpell || NextMagicSpell == Spell.None && Owner.Info.HeroBehaviour == HeroBehaviour.Attack && TargetDistance > 1))
                MoveTo(Target.CurrentLocation);
        }

        private bool HasRangedSpell => Info.Magics.Select(x => x.Spell).Intersect(Globals.RangedSpells).Any();
    }
}
