using Server.MirDatabase;
using Server.MirEnvir;
using System;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ConquestArcher : TownArcher
    {
        public ConquestObject Conquest;
        public int ArcherIndex;

        protected override bool CanMove
        {
            get { return Route.Count > 0 && !Dead && Envir.Time > MoveTime && Envir.Time > ActionTime && Envir.Time > ShockTime; }
        }

        protected internal ConquestArcher(MonsterInfo info) : base(info) { }

        public override bool IsAttackTarget(MonsterObject attacker) { return false; }

        public override void Die()
        {
            base.Die();
        }
        
        protected override void FindTarget()
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
                            switch (ob.Race)
                            {
                                case ObjectType.Player:
                                    PlayerObject playerob = (PlayerObject)ob;
                                    if (!ob.IsAttackTarget(this)) continue;
                                    if (playerob.MyGuild != null && playerob.MyGuild.Conquest != null && Conquest.Info.Index == playerob.MyGuild.Conquest.Info.Index || ob.Hidden && (!CoolEye || Level < ob.Level) || !Conquest.WarIsOn) continue;
                                    Target = ob;
                                    return;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
        }

        public override int Attacked(PlayerObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            if (!Conquest.WarIsOn || attacker.MyGuild != null && Conquest.Owner == attacker.MyGuild.Guildindex) damage = 0;

            return base.Attacked(attacker, damage, type, damageWeapon);
        }
        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            if (!Conquest.WarIsOn) damage = 0;

            return base.Attacked(attacker, damage, type);
        }
    }
}
