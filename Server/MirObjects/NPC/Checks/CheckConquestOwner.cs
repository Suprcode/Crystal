using System.Linq;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CONQUESTOWNER")]
	public class CheckConquestOwner : NPCCheck
	{
		protected readonly int ConquestIndex;
		protected ConquestObject Conquest;
		public CheckConquestOwner(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex))
				return;
			InitializationSuccess = true;
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			Conquest ??= Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			if (Conquest is null)
			{
				InitializationSuccess = false;
				return false;
			}
			switch (ob)
			{
				case PlayerObject player:
					if (player.MyGuild is null) return false;
					return player.MyGuild.Guildindex == Conquest.GuildInfo.Owner;
			}
			return false;
		}
	}
}