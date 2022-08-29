using System.Linq;
namespace Server.MirObjects.Actions
{
	[ActionCommand("SCHEDULECONQUEST")]
	public class ActionScheduleConquest : NPCAction
	{
		protected readonly int ConquestIndex;
		protected ConquestObject Conquest;
		public ActionScheduleConquest(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
			if (Conquest is null)
			{
				InitializationSuccess = false;
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					if (player.MyGuild is null || player.MyGuild.Guildindex == Conquest.GuildInfo.Owner || Conquest.WarIsOn)
						return;
					Conquest.GuildInfo.AttackerID = player.MyGuild.Guildindex;
					break;
			}
		}
	}
}