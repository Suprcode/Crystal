namespace Server.MirObjects.Checks
{
	[CheckCommand("LEVEL")]
	public class CheckLevel : NPCCheck
	{
		protected readonly ushort RequiredLevel;
		protected readonly string Operator;
		public CheckLevel(string line, string[] parts) : base(line, parts)
		{
			if (!ushort.TryParse(parts[2], out RequiredLevel))
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
					return Compare(Operator, player.Level, RequiredLevel);
				case MonsterObject mob:
					return Compare(Operator, mob.Level, RequiredLevel);
			}
			return false;
		}
	}
}