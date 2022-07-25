using System.Linq;
using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("TAKECONQUESTGOLD")]
	public class ActionTakeConquestGold : NPCAction
	{
		protected readonly int ConquestIndex;
		protected ConquestObject Conquest;
		public ActionTakeConquestGold(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex))
				return;
			InitializationSuccess = true;
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			if (Conquest is null) return;
			switch (ob)
			{
				case PlayerObject player:
					
					if (player.MyGuild is null || player.MyGuild.Guildindex != Conquest.GuildInfo.Owner)
						return;
					player.MyGuild.SendServerPacket(new GuildStorageGoldChange
					{
						Type = 3,
						Amount = Conquest.GuildInfo.GoldStorage
					});
					player.MyGuild.Gold += Conquest.GuildInfo.GoldStorage;
					Conquest.GuildInfo.GoldStorage = 0;
					break;
			}
		}
	}
}