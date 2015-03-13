using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class ZumaTaurus : ZumaMonster
    {
        private byte _stage = 7;

        protected internal ZumaTaurus(MonsterInfo info) : base(info)
        {
            Direction = MirDirection.DownLeft;
            AvoidFireWall = false;
        }

        protected override void ProcessAI()
        {
            if (Dead) return;
            
            if (MaxHP >= 7)
            {
                byte stage = (byte)(HP / (MaxHP / 7));

                if (stage < _stage) SpawnSlaves();
                _stage = stage;
            }


            base.ProcessAI();
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
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.MACAgility);
        }
        private void SpawnSlaves()
        {
            int count = Math.Min(8, 40 - SlaveList.Count);

            for (int i = 0; i < count; i++)
            {
                MonsterObject mob = null;
                switch (Envir.Random.Next(7))
                {
                    case 0:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Zuma1));
                        break;
                    case 1:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Zuma2));
                        break;
                    case 2:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Zuma3));
                        break;
                    case 3:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Zuma4));
                        break;
                    case 4:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Zuma5));
                        break;
                    case 5:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Zuma6));
                        break;
                    case 6:
                        mob = GetMonster(Envir.GetMonsterInfo(Settings.Zuma7));
                        break;
                }

                if (mob == null) continue;

                if (!mob.Spawn(CurrentMap, Front))
                    mob.Spawn(CurrentMap, CurrentLocation);

                mob.Target = Target;
                mob.ActionTime = Envir.Time + 2000;
                SlaveList.Add(mob);
            }

        }
    }
}
