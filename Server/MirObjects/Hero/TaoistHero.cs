using Server.MirDatabase;

namespace Server.MirObjects
{
    public class TaoistHero : HeroObject
    {
        public TaoistHero(CharacterInfo info, PlayerObject owner) : base(info, owner) { }
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
            UserMagic magic;
            List<MapObject> TargetList = new List<MapObject>()
            {
                this,
                Owner
            };

            for (int i = 0; i < TargetList.Count; i++)
            {
                MapObject target = TargetList[i];
                if (target.Dead) continue;

                MirDirection direction = Functions.DirectionFromPoint(CurrentLocation, target.CurrentLocation);

                if (target.Buffs.Any(b => b.Properties.HasFlag(BuffProperty.Debuff)) || target.PoisonList.Count > 0)
                {
                    magic = GetMagic(Spell.Purification);
                    if (CanUseMagic(magic))
                    {
                        BeginMagic(magic.Spell, direction, target.ObjectID, target.CurrentLocation);
                        return;
                    }
                }

                if (target.PercentHealth < 90)
                {
                    magic = GetMagic(Spell.MassHealing);
                    if (CanUseMagic(magic))
                    {
                        BeginMagic(magic.Spell, direction, target.ObjectID, target.CurrentLocation);
                        return;
                    }

                    magic = GetMagic(Spell.Healing);
                    if (CanUseMagic(magic))
                    {
                        BeginMagic(magic.Spell, direction, target.ObjectID, target.CurrentLocation);
                        return;
                    }
                }

                UserItem item = GetAmulet(1);

                if (Target != null)
                {
                    magic = GetMagic(Spell.SoulShield);
                    if (CanUseMagic(magic) && item != null && !target.HasBuff(BuffType.SoulShield))
                    {
                        BeginMagic(magic.Spell, direction, target.ObjectID, target.CurrentLocation);
                        return;
                    }

                    magic = GetMagic(Spell.BlessedArmour);
                    if (CanUseMagic(magic) && item != null && !target.HasBuff(BuffType.BlessedArmour))
                    {
                        BeginMagic(magic.Spell, direction, target.ObjectID, target.CurrentLocation);
                        return;
                    }

                    magic = GetMagic(Spell.UltimateEnhancer);
                    if (CanUseMagic(magic) && item != null && !target.HasBuff(BuffType.UltimateEnhancer))
                    {
                        BeginMagic(magic.Spell, direction, target.ObjectID, target.CurrentLocation);
                        return;
                    }
                }
            }

            magic = GetMagic(Spell.None);
            {
                return;
            }
        }

        protected override void ProcessAttack()
        {           
            if (Target == null || Target.Dead) return;
            TargetDistance = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);
            if (!CanCast) return;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            UserMagic magic;
            UserItem amuletItem = GetAmulet(1);
            UserItem poisonItem = GetPoison(1);

            if (InAttackRange())
            {
                magic = GetMagic(Spell.Poisoning);
                if (CanUseMagic(magic) && poisonItem != null)
                {
                    if ((poisonItem.Info.Shape == 1 && !Target.PoisonList.Any(p => p.PType == PoisonType.Green)) || (poisonItem.Info.Shape == 2 && !Target.PoisonList.Any(p => p.PType == PoisonType.Red)))
                    {
                        BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                        return;
                    }
                }

                magic = GetMagic(Spell.Curse);
                if (CanUseMagic(magic) && !Target.HasBuff(BuffType.Curse) && amuletItem != null)
                {
                    BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                    return;
                }

                if (Target.AutoRev == false && Envir.Time > Target.RevTime) 
                {
                    magic = GetMagic(Spell.Revelation);
                    if (CanUseMagic(magic) && amuletItem != null)
                    {
                        BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                        return;
                    }
                }

                magic = GetMagic(Spell.SoulFireBall);
                if (CanUseMagic(magic) && amuletItem != null)
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

            if (CanCast && NextMagicSpell != Spell.None)
            {
                if (Target != Owner && !CanAttack) return;
                Magic(NextMagicSpell, NextMagicDirection, NextMagicTargetID, NextMagicLocation);
                NextMagicSpell = Spell.None;
            }

            if (Target == null || !CanAttack) return;

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
