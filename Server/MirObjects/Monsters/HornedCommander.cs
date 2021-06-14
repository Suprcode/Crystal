using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    //Hit (Attack1) (added)
    //Alt Hit? (Attack 2) (added)
    //Charge Hit Spin (Attack3) (added)
    //Hammer up, teleport (AttackRange1) (added)
    //Hammer smash down (dust in front) (AttackRange2) (added)
    //Left hand up, calls spell effect rocks (AttackRange3)
    //Hammer up (repeating effect), causes AOE rock fall (Attack4) (added)
    //Shield (Attack5) (added)

    public class HornedCommander : MonsterObject
    {
        protected int HPPercent { get { return (HP * 100) / MaxHealth; } }
        private bool _CalledShield;
        private bool _CalledRocks;

        private bool _StartAdvanced;
        private bool _Charging;

        protected internal HornedCommander(MonsterInfo info)
            : base(info)
        {
        }

        protected override void ProcessAI()
        {
            if (_Charging) return;

            if (HPPercent < 10 && !_CalledShield)
            {
                _CalledShield = true;
                _CalledRocks = false;
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 3 });
                //ADD BUFF
                //SUMMON SLAVE
                //STOP MOVING
                //STOP ATTACKS
            }
            
            if (HPPercent < 30 && HPPercent >= 10 && !_CalledRocks)
            {
                _CalledRocks = true;
                //Start loop to call rocks
                //Stop when under 10% HP
                //Stop when whole map covered
                //Stop when dead
            }

            if (HPPercent < 100 && !_StartAdvanced)
            {
                _StartAdvanced = true;
            }
            else if (HPPercent == 10 && _StartAdvanced)
            {
                _StartAdvanced = false;
            }

            base.ProcessAI();
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (InAttackRange())
            {
                Attack();

                if (Target != null && Target.Dead)
                {
                    FindTarget();
                }

                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }


        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            //Charge-Up AOE Rock Fall
            if (_StartAdvanced && Envir.Random.Next(20) == 0)
            {
                _Charging = true;

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 3 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.AC);
                ActionList.Add(action);
                return;
            }

            //Charge-Up Spin Hit
            if (_StartAdvanced && Envir.Random.Next(15) == 0)
            {
                _Charging = true;

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.AC);
                ActionList.Add(action);
                return;
            }

            //Hammer Smash
            if (_StartAdvanced && Envir.Random.Next(10) == 0)
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 300, Target, damage, DefenceType.AC);
                ActionList.Add(action);
                return;
            }
            
            //Teleport
            if (_StartAdvanced && Envir.Random.Next(10) == 0)
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
            
                return;
            }


            //Normal Attacks
            if (Envir.Random.Next(2) == 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
                ActionList.Add(action);
            }
        }
    }
}
