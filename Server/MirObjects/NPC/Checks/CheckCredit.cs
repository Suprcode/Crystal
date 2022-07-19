namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKCREDIT")]
	public class CheckCredit : NPCCheck
	{
		protected readonly uint RequiredAmount;
		protected readonly string Operator;
		public CheckCredit(string line, string[] parts) : base(line, parts)
		{
			if (!uint.TryParse(parts[2], out RequiredAmount))
				return;
			Operator = parts[1];
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess)
				return false;
			switch (ob)
			{
				case PlayerObject playerObject:
					return Compare(Operator, playerObject.Account.Credit, RequiredAmount);
			}
			return false;
		}
	}
}