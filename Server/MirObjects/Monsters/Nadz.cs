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
                SinglePushAttack(Stats[Stat.MinDC], Stats[Stat.MaxDC], attackType: 1);
                PoisonTarget(Target, 3, 5, PoisonType.Paralysis);                
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
    }
}
