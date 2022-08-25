using System.Linq;
namespace Server.MirObjects.Actions
{
	[ActionCommand("SETCONQUESTRATE")]
	public class ActionSetConquestRate : NPCAction
	{
		protected readonly int ConquestIndex;
		protected readonly byte Rate;
		protected ConquestObject Conquest;
		public ActionSetConquestRate(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!byte.TryParse(parts[2], out Rate))
				return;
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			InitializationSuccess = true;
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
			switch (ob)
			{
				case PlayerObject player:
					if (player.MyGuild is null) return;
					if (player.MyGuild.Guildindex != Conquest.GuildInfo.Owner)
						return;
					Conquest.GuildInfo.NPCRate = Rate;
					break;
			}
		}
	}
}