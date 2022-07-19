using Server.MirEnvir;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKHUM")]
	public class CheckHum : NPCCheck
	{
		protected readonly string MapName;
		protected Map RequestedMap;
		protected readonly int RequestedInstance;
		protected readonly int RequiredAmount;
		protected readonly string Operator;
		public CheckHum(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[1], out RequiredAmount))
				return;
			MapName = parts[2];
			if (parts.Length > 3)
				InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			RequestedMap ??= Envir.GetMapByNameAndInstance(MapName, RequestedInstance);
			if (RequestedMap is null)
			{
				InitializationSuccess = false;
				return false;
			}
			return Compare(Operator, RequestedMap.Players.Count, RequiredAmount);
		}
	}
}