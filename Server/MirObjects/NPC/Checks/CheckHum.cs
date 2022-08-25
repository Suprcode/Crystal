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
			Operator = parts[1];
			if (!int.TryParse(parts[2], out RequiredAmount))
				return;
			
			MapName = parts[3];
			if (parts.Length > 4)
			{
				if (!int.TryParse(parts[4], out RequestedInstance))
					return;
			}
			else
				RequestedInstance = 0;
				
			
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			RequestedMap ??= Envir.GetMapByNameAndInstance(MapName, RequestedInstance);
			if (RequestedMap is null)
				return false;
			return Compare(Operator, RequestedMap.Players.Count, RequiredAmount);
		}
	}
}