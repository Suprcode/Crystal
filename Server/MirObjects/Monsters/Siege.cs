using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Siege : MonsterObject
    {
        public long FearTime;

        public ConquestObject Conquest;
        public int WallIndex;

        private bool _stationary;
        //private bool _canRepair;
        //private bool _canTeleport;

        //private int _minAttackRange = 0;
        //private int _maxAttackRange = 10;

        protected virtual byte AttackRange
        {
            get
            {
                return 6;
            }
        }

        protected override bool CanMove => base.CanMove && !_stationary;

        public override bool IsAttackTarget(MonsterObject attacker) { return false; }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected internal Siege(MonsterInfo info) : base(info)
        {
            switch (info.Effect)
            {
                case 1: //Catapult
                    //_canTeleport = true;
                    //_minAttackRange = 5;
                    break;
                case 2: //ChariotBallista
                    //_canTeleport = true;
                    //_minAttackRange = 0;
                    break;
                case 3: //Ballista
                    _stationary = true;
                    //_canRepair = true;
                    //_minAttackRange = 0;
                    break;
                case 4: //Trebuchet
                    _stationary = true;
                    //_canRepair = true;
                    //_minAttackRange = 10;
                    break;
                case 5: //CanonTrebuchet
                    _stationary = true;
                    //_canRepair = true;
                    //_minAttackRange = 7;
                    break;
            }
        }

        protected override bool CanRegen
        {
            get
            {
                return false;
            }
        }

        protected override void FindTarget()
        {
            
        }

        protected override void ProcessSearch()
        {
            
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            ProjectileAttack(damage);
        }

        protected override void ProcessTarget()
        {
        }

        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            return base.Attacked(attacker, damage, type, damageWeapon);
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            return base.Attacked(attacker, damage, type);
        }
    }
}
