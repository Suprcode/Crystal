using System.Linq;
using Server.MirDatabase;
using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("CONQUESTSIEGE")]
	public class ActionConquestSiege : NPCAction
	{
		protected readonly int ConquestIndex;
		protected readonly int GuardIndex;
		protected ConquestObject Conquest;
		protected ConquestGuildSiegeInfo Siege;
		public ActionConquestSiege(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!int.TryParse(parts[2], out GuardIndex))
				return;
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			InitializationSuccess = true;
			if (Conquest == null)
				return;
			Siege = Conquest.SiegeList.FirstOrDefault(siege => siege.Info.Index == GuardIndex);
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
			Siege ??= Conquest.SiegeList.FirstOrDefault(siege => siege.Info.Index == GuardIndex);
			if (Siege is null)
			{
				InitializationSuccess = false;
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					if (player.MyGuild is null) return;
					if (player.MyGuild.Gold < Siege.GetRepairCost()) return;
					player.MyGuild.SendServerPacket(new GuildStorageGoldChange
					{
						Type = 2,
						Amount = (uint) Siege.GetRepairCost()
					});
					player.MyGuild.Gold -= (uint) Siege.GetRepairCost();
					break;
			}
		}
	}
}