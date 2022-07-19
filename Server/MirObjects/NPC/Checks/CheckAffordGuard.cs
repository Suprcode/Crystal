using System.Linq;
using Server.MirDatabase;
namespace Server.MirObjects.Checks
{
	[CheckCommand("AFFORDGUARD")]
	public class CheckAffordGuard : NPCCheck
	{
		protected ConquestObject Conquest;
		protected ConquestGuildArcherInfo ArcherInfo;
		protected readonly int ConquestIndex;
		protected readonly int ArcherIndex;
		public CheckAffordGuard(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!int.TryParse(parts[2], out ArcherIndex))
				return;
			InitializationSuccess = true;
			//Attempt to Cache the Objects
			
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			if (Conquest is null)
				return;
			ArcherInfo = Conquest.ArcherList.FirstOrDefault(archer => archer.Info.Index == ArcherIndex);
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
					//Check if the Archer has been set, if not, try get it
					ArcherInfo ??= Conquest.ArcherList.FirstOrDefault(archer => archer.Info.Index == ArcherIndex);
					//Ensure the Archer is not null
					if (ArcherInfo is null)
					{
						//Mark this command as failed (Error, Gate not found)
						InitializationSuccess = false;
						return false;
					}
					if (playerObject.MyGuild is null) return false;
					return playerObject.MyGuild.Gold >= ArcherInfo.GetRepairCost();
			}
			return false;
		}
	}
}