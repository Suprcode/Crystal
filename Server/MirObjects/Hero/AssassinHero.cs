using Server.MirDatabase;
using Server.MirNetwork;

namespace Server.MirObjects
{
    public class AssassinHero : HeroObject
    {
        private bool HasHeavenlySword;
        public AssassinHero(CharacterInfo info, PlayerObject owner) : base(info, owner) { }
        protected override void Load(CharacterInfo info, MirConnection connection)
        {
            base.Load(info, connection);

            if (HasMagic(Spell.DoubleSlash))
                Info.DoubleSlash = true;
        }
        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            if (HasHeavenlySword)
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
                UserMagic magic = GetMagic(Spell.Haste);
                if (CanUseMagic(magic) && !HasBuff(BuffType.Haste))
                {
                    BeginMagic(magic.Spell, Direction, ObjectID, CurrentLocation);
                    return;
                }

                magic = GetMagic(Spell.LightBody);
                if (CanUseMagic(magic) && !HasBuff(BuffType.LightBody))
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
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            UserMagic magic = GetMagic(Spell.HeavenlySword);
            HasHeavenlySword = CanCast && CanUseMagic(magic);
            if (HasHeavenlySword && TargetDistance == 2)
            {
                BeginMagic(magic.Spell, Direction, Target.ObjectID, Target.CurrentLocation);
                return;
            }
        }
        protected override void Attack()
        {
            if (!Target.IsAttackTarget(Owner))
            {
                Target = null;
                return;
            }
            
            Spell spell = Spell.None;

            if (Info.DoubleSlash)
                spell = Spell.DoubleSlash;            

            Attack(Direction, spell);
        }
    }
}
