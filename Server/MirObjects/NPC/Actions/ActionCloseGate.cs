using System.Linq;
using Server.MirDatabase;
namespace Server.MirObjects.Actions
{
	[ActionCommand("CLOSEGATE")]
	public class ActionCloseGate : NPCAction
	{
		protected readonly int ConquestIndex;
		protected readonly int GateIndex;
		protected ConquestObject Conquest;
		protected ConquestGuildGateInfo Gate;
		public ActionCloseGate(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!int.TryParse(parts[2], out GateIndex))
				return;
			InitializationSuccess = true;
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			if (Conquest is null) return;
			Gate = Conquest.GateList.FirstOrDefault(gate => gate.Info.Index == GateIndex);
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
			Gate ??= Conquest.GateList.FirstOrDefault(gate => gate.Info.Index == GateIndex);
			if (Gate is null)
			{
				InitializationSuccess = false;
				return;
			}
			Gate.Gate?.CloseDoor();
		}
	}
}