using System.Linq;
using Server.MirDatabase;
using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("CONQUESTWALL")]
	public class ActionConquestWall : NPCAction
	{
		protected readonly int ConquestIndex;
		protected readonly int GuardIndex;
		protected ConquestObject Conquest;
		protected ConquestGuildWallInfo Wall;
		public ActionConquestWall(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!int.TryParse(parts[2], out GuardIndex))
				return;
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			InitializationSuccess = true;
			if (Conquest == null)
				return;
			Wall = Conquest.WallList.FirstOrDefault(wall => wall.Info.Index == GuardIndex);
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Conquest ??= Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			if (Conquest is null)
			{
				InitializationSuccess = false;
				return;
			}
			Wall ??= Conquest.WallList.FirstOrDefault(wall => wall.Info.Index == GuardIndex);
			if (Wall is null)
			{
				InitializationSuccess = false;
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					if (player.MyGuild is null) return;
					if (player.MyGuild.Gold < Wall.GetRepairCost()) return;
					player.MyGuild.SendServerPacket(new GuildStorageGoldChange
					{
						Type = 2,
						Amount = (uint) Wall.GetRepairCost()
					});
					player.MyGuild.Gold -= (uint) Wall.GetRepairCost();
					break;
			}
		}
	}
}