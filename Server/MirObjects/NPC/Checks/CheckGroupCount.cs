namespace Server.MirObjects.Checks
{
	[CheckCommand("GROUPCOUNT")]
	public class CheckGroupCount : NPCCheck
	{
		protected readonly int RequiredAmount;
		protected readonly string Operator;
		public CheckGroupCount(string line, string[] parts) : base(line, parts)
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
					return !(player.GroupMembers is null) &&
						Compare(Operator, player.GroupMembers.Count, RequiredAmount);
			}
			return false;
		}
	}
}