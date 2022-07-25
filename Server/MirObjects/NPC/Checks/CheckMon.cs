using Server.MirEnvir;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKMON")]
	public class CheckMon : NPCCheck
	{
		protected readonly int RequiredAmount;
		protected readonly string MapName;
		protected Map RequestedMap;
		protected readonly int RequestedInstance;
		protected readonly string Operator;
		public CheckMon(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[2], out RequiredAmount))
				return;
			if (parts.Length < 5)
				RequestedInstance = 1;
			else if (!int.TryParse(parts[4], out RequestedInstance))
				return;
			MapName = parts[3];
			RequestedMap = Envir.GetMapByNameAndInstance(parts[3], RequestedInstance);
			Operator = parts[1];
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			RequestedMap ??= Envir.GetMapByNameAndInstance(MapName, RequestedInstance);
			if (RequestedMap == null)
				return false;
			return Compare(Operator, RequestedMap.MonsterCount, RequiredAmount);
		}
	}
}