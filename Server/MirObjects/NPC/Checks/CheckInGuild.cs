namespace Server.MirObjects.Checks
{
	[CheckCommand("INGUILD")]
	public class CheckInGuild : NPCCheck
	{
		protected readonly string GuildName;
		
		public CheckInGuild(string line, string[] parts) : base(line, parts)
		{
			GuildName = parts[1];
			InitializationSuccess = !string.IsNullOrEmpty(GuildName);
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject player:
					return !(player.MyGuild is null) && player.MyGuild.Name == GuildName;
			}
			return false;
		}
	}
}