using System.Linq;
using Server.MirDatabase;
namespace Server.MirObjects.Checks
{
	[CheckCommand("AFFORDWALL")]
	public class CheckAffordWall : NPCCheck
	{
		protected ConquestObject Conquest;
		protected ConquestGuildWallInfo WallInfo;
		protected readonly int ConquestIndex;
		protected readonly int WallIndex;
		public CheckAffordWall(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!int.TryParse(parts[2], out WallIndex))
				return;
			InitializationSuccess = true;
			//Attempt to Cache the Objects
			
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			if (Conquest is null)
				return;
			WallInfo = Conquest.WallList.FirstOrDefault(wall => wall.Info.Index == WallIndex);
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
					//Check if the Wall has been set, if not, try get it
					WallInfo ??= Conquest.WallList.FirstOrDefault(wall => wall.Info.Index == WallIndex);
					//Ensure the Wall is not null
					if (WallInfo is null)
					{
						//Mark this command as failed (Error, Gate not found)
						InitializationSuccess = false;
						return false;
					}
					if (playerObject.MyGuild is null) return false;
					return playerObject.MyGuild.Gold >= WallInfo.GetRepairCost();
			}
			return false;
		}
	}
}