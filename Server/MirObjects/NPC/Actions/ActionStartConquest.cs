using System;
using System.Linq;
namespace Server.MirObjects.Actions
{
	[ActionCommand("STARTCONQUEST")]
	public class ActionStartConquest : NPCAction
	{
		protected readonly int ConquestIndex;
		protected readonly ConquestGame Type;
		protected ConquestObject Conquest;
		public ActionStartConquest(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!Enum.TryParse(parts[2], out Type))
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
			if (Conquest.WarIsOn) return;
			Conquest.StartType = ConquestType.Forced;
			Conquest.GameType = Type;
			Conquest.StartWar(Type);
		}
	}
}