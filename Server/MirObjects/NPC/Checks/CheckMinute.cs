namespace Server.MirObjects.Checks
{
	[CheckCommand("MINUTE")]
	public class CheckMinute : NPCCheck
	{
		protected readonly uint RequiredMinute;
		public CheckMinute(string line, string[] parts) : base(line, parts)
		{
			if (!uint.TryParse(parts[1], out RequiredMinute))
				return;
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			return InitializationSuccess &&
				Envir.Now.Minute == RequiredMinute;
		}
	}
}