using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class Nadz : MonsterObject
    {
        protected internal Nadz(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 3 || y > 3) return false;


            return (x <= 3 && y <= 3) || (x == y || x % 3 == y % 3);
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (Envir.Random.Next(3) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                Attack1(3);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                PushAttack(1);
            }

        }

        //360 degree swing Attack - hits all players within 3 spaces of mob in 360 degrees.
        private void Attack1(int distance)
        {
            List<MapObject> targets = FindAllTargets(3, CurrentLocation);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;
                if (targets[i].Attacked(this, damage, DefenceType.AC) <= 0) return;
            }

        }

        //Repulsion - Attack2 Scream Attack (utilises DelayedAction so player is hit at end of push)
        //need to put Damage Stats (DC/MC/SC) on mob for it to push
        private void PushAttack(int distance)
        {
            int levelGap = 5;
            int mobLevel = this.Level;
            int targetLevel = Target.Level;

            if ((targetLevel <= mobLevel + levelGap))
            {
                if (Target.Pushed(this, Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation), 2) > 0)
                {
                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    AttackTime = Envir.Time + AttackSpeed + 500;
                    if (damage == 0) return;

                    int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MAC);
                    ActionList.Add(action);

                    if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.Stats[Stat.PoisonResist])
                    {
                        if (Envir.Random.Next(3) == 0)
                            Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Paralysis, Value = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]), TickSpeed = 1000 }, this);
                    }
                }
            }
        }
    }
}
