namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKGUILDGOLD")]
	public class CheckGuildGold : NPCCheck
	{
		protected readonly uint RequiredAmount;
		protected readonly string Operator;
		public CheckGuildGold(string line, string[] parts) : base(line, parts)
		{
			if (!uint.TryParse(parts[1], out RequiredAmount))
				return;
			Operator = parts[0];
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject playerObject:
					if (playerObject.MyGuild is null)
						return false;
					return Compare(Operator, playerObject.MyGuild.Gold, RequiredAmount);
			}
			return false;
		}
	}
}