using System.Collections.Generic;
using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Linq;
using System.Text;

namespace Server.MirObjects.Monsters
{
    //TODO - COMPLETE GeneralJinmYo AI
    /* *** NOTES: ***
    Thunderbolt and Lightning Orb at end of lib file - possibly a summoned ball of thunder, similar to HellLord, it spawns the lightning ball
     * which will attack players until killed? Perhaps it spawns them every so often and the amount of balls will increase unless killed and 
     * will overrun players unless they are killed?
     */
    class GeneralJinmYo : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 12;
            }
        }

        protected internal GeneralJinmYo(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
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
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 2);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (!ranged)
            {
                if (Envir.Random.Next(2) == 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                    Attack1(1); //Halfmoon Attack
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    Attack2(1);//Weapon Slam Attack
                }

            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });
                RangeAttack1(1);//Standard Ranged Attack
            }


            if (Target.Dead)
                FindTarget();

        }

        Cell cell = null;

        private void Attack1(int distance)//Halfmoon Attack
        {
            MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

            dir = Functions.NextDir(dir);

            Point target = Functions.PointMove(CurrentLocation, dir, 1);

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.AC);

            for (int i = 0; i < 6; i++)
            {
                target = Functions.PointMove(CurrentLocation, dir, 1);
                dir = Functions.NextDir(dir);

                if (!CurrentMap.ValidPoint(target)) continue;

                cell = CurrentMap.GetCell(target);

                if (cell.Objects == null) continue;

                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject ob = cell.Objects[o];
                    if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                    if (!ob.IsAttackTarget(this)) continue;

                    ob.Attacked(this, damage, DefenceType.AC);
                    break;
                }
            }
        }

        private void Attack2(int distance)//Weapon Slam Attack
        {
            List<MapObject> targets = FindAllTargets(3, Target.CurrentLocation);
            if (targets.Count == 0) return;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC] * 2);
            if (damage == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                Target = targets[i];
                if (Target.IsAttackTarget(this))
                    Target.Attacked(this, damage, DefenceType.AC);
            }
        }

        private void RangeAttack1(int distance)//Standard Ranged Attack
        {
            int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MAC);
            ActionList.Add(action);
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);

        }
    }

}

