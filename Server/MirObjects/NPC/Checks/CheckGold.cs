namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKGOLD")]
	public class CheckGold : NPCCheck
	{
		protected readonly uint RequiredAmount;
		protected readonly string Operator;
		public CheckGold(string line, string[] parts) : base(line, parts)
		{
			if (!uint.TryParse(parts[1], out RequiredAmount))
				return;
			Operator = parts[0];
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess)
				return false;
			switch (ob)
			{
				case PlayerObject player:
					return Compare(Operator, player.Account.Gold, RequiredAmount);
			}
			return false;
		}
	}
}