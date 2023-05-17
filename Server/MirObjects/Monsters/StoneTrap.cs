using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class StoneTrap : MonsterObject
    {
        public bool Summoned;
        public long DieTime;

        protected internal StoneTrap(MonsterInfo info) : base(info)
        {
            Direction = MirDirection.Up;
        }

        public override string Name
        {
            get { return Master == null ? Info.GameName : (Dead ? Info.GameName : string.Format("{0}({1})", Info.GameName, Master.Name)); }
            set { throw new NotSupportedException(); }
        }

        protected override void ProcessRegen() { }

        protected override void Attack() { }

        protected override void ProcessAI()
        {
            for (int d = 0; d <= Info.ViewRange; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (!cell.Valid || cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];

                            if (ob == this)
                            {
                                continue;
                            }

                            switch (ob.Race)
                            {
                                case ObjectType.Monster:
                                case ObjectType.Hero:

                                    MonsterInfo mInfo = Envir.GetMonsterInfo(ob.Name);
                                    if (mInfo == null)
                                    {
                                        continue;
                                    }

                                    MonsterObject monster = MonsterObject.GetMonster(mInfo);
                                    if (!monster.Dead)
                                    {
                                        if (monster.Master == null ||
                                            (monster.Master != null &&
                                            monster.IsAttackTarget(this.Master)))
                                        {
                                            if (Target != null &&
                                                Target is StoneTrap &&
                                                Target != this)
                                            {
                                                continue;
                                            }

                                            monster.Target = this;
                                        }     
                                    }

                                    break;
                            }
                        }
                    }
                }
            }
        }

        public override void Turn(MirDirection dir)
        {
        }
        public override bool Walk(MirDirection dir)
        {
            return false;
        }
        public override void Spawned()
        {
            base.Spawned();
            Summoned = true;
        }
        public override void Process()
        {
            if (!Dead &&
                Master != null)
            {
                if (Master.CurrentMap != CurrentMap ||
                    Envir.Time > DieTime ||
                    !Functions.InRange(Master.CurrentLocation, CurrentLocation, 15))
                {
                    Die();
                } 
                else
                {
                    FindTarget();
                }
            }

            base.Process();
        }

        public override int Struck(int damage, DefenceType type = DefenceType.ACAgility)
        {
            return 0;
        }

        public override Packet GetInfo()
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Location = CurrentLocation,
                Image = Monster.StoningSpider,
                Direction = Direction,
                Effect = Info.Effect,
                AI = Info.AI,
                Light = Info.Light,
                Dead = Dead,
                Skeleton = Harvested,
                Poison = CurrentPoison,
                Hidden = Hidden,
                Extra = Summoned,
            };
        }
    }
}
