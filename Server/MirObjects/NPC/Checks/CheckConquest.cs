using System.Linq;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKCONQUEST")]
	public class CheckConquest : NPCCheck
	{
		protected readonly int ConquestIndex;
		protected ConquestObject ConquestObject;
		public CheckConquest(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex))
				return;
			InitializationSuccess = true;
			ConquestObject = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			ConquestObject ??= Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			if (!(ConquestObject is null))
				return !ConquestObject.WarIsOn;
			InitializationSuccess = false;
			return false;
		}
	}
}