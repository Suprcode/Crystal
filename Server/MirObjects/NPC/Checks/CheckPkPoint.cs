namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKPKPOINT")]
	public class CheckPkPoint : NPCCheck
	{
		protected readonly int RequiredAmount;
		protected readonly string Operator;
		
		public CheckPkPoint(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[2], out RequiredAmount))
				return;
			Operator = parts[1];
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject player:
					return Compare(Operator, player.PKPoints, RequiredAmount);
			}
			return false;
		}
	}
}