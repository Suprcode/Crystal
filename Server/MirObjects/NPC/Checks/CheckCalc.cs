namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKCALC")]
	public class CheckCalc : NPCCheck
	{
		protected readonly int Left, Right;
		protected readonly string Operator;
		public CheckCalc(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[2], out Left) ||
				!int.TryParse(parts[3], out Right))
				return;
			Operator = parts[1];
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			return InitializationSuccess && Compare(Operator, Left, Right);
		}
	}
}