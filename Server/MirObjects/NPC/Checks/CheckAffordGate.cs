using System.Linq;
using Server.MirDatabase;
namespace Server.MirObjects.Checks
{
	[CheckCommand("AFFORDGATE")]
	public class CheckAffordGate : NPCCheck
	{
		protected ConquestObject Conquest;
		protected ConquestGuildGateInfo GateInfo;
		protected readonly int ConquestIndex;
		protected readonly int GuardIndex;
		public CheckAffordGate(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!int.TryParse(parts[2], out GuardIndex))
				return;
			InitializationSuccess = true;
			//Attempt to Cache the Objects
			
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			if (Conquest is null)
				return;
			GateInfo = Conquest.GateList.FirstOrDefault(gate => gate.Info.Index == GuardIndex);
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject playerObject:
					//Check if the Conquest has been set, if not, try get it
					Conquest ??= Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
					//Ensure the Conquest is not null
					if (Conquest is null)
					{
						//Mark this command as failed (Error, Conquest not found)
						InitializationSuccess = false;
						return false;
					}
					//Check if the Gate has been set, if not, try get it
					GateInfo ??= Conquest.GateList.FirstOrDefault(gate => gate.Info.Index == GuardIndex);
					//Ensure the Gate is not null
					if (GateInfo is null)
					{
						//Mark this command as failed (Error, Gate not found)
						InitializationSuccess = false;
						return false;
					}
					if (playerObject.MyGuild is null) return false;
					return playerObject.MyGuild.Gold >= GateInfo.GetRepairCost();
			}
			return false;
		}
	}
}