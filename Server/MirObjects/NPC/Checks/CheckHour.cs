namespace Server.MirObjects.Checks
{
	[CheckCommand("HOUR")]
	public class CheckHour : NPCCheck
	{
		protected readonly uint RequiredHour;
		public CheckHour(string line, string[] parts) : base(line, parts)
		{
			if (!uint.TryParse(parts[1], out RequiredHour))
				return;
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			return Envir.Now.Hour == RequiredHour;
		}
	}
}