using System.Linq;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CONQUESTAVAILABLE")]
	public class CheckConquestAvailable : NPCCheck
	{
		protected readonly int ConquestIndex;
		protected ConquestObject Conquest;
		public CheckConquestAvailable(string line, string[] parts) : base(line, parts)
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
				case PlayerObject playerObject:
					if (playerObject.MyGuild is null) return false;
					return Conquest.GuildInfo.AttackerID == -1;
			}
			return false;
		}
	}
}