using System.Drawing;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class RootSpider : BugBagMaggot
    {

        protected internal RootSpider(MonsterInfo info)
            : base(info)
        {
            byte randomdirection = (byte)Envir.Random.Next(3);
            Direction = (MirDirection)randomdirection;
        }

        protected override void Attack()
        {
            ShockTime = 0;

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            if (SlaveList.Count >= 20) return;


            MonsterObject spawn = GetMonster(Envir.GetMonsterInfo(Settings.BombSpiderName));

            if (spawn == null) return;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + 3000;

            spawn.Target = Target;
            spawn.ActionTime = Envir.Time + 1000;
            Point spawnlocation = Point.Empty;
            switch (Direction)
            {
                case MirDirection.Up:
                    spawnlocation = Back;
                    break;
                case MirDirection.UpRight:
                    spawnlocation = Functions.PointMove(CurrentLocation, MirDirection.DownRight, 1);
                    break;
                case MirDirection.Right:
                    spawnlocation = Functions.PointMove(CurrentLocation, MirDirection.DownLeft, 1);
                    break;
            }
            CurrentMap.ActionList.Add(new DelayedAction(DelayedType.Spawn, Envir.Time + 500, spawn, spawnlocation, this));
        }
    }
}
