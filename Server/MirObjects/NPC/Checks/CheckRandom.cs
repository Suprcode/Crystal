namespace Server.MirObjects.Checks
{
	[CheckCommand("RANDOM")]
	public class CheckRandom : NPCCheck
	{
		protected readonly int Amount;
		public CheckRandom(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[1], out Amount))
				return;
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			return Random.Next(0, Amount) == 0;
		}
	}
}