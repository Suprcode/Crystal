namespace Server.MirObjects.Checks
{
	[CheckCommand("INGUILD")]
	public class CheckInGuild : NPCCheck
	{
		protected readonly string GuildName;
		
		public CheckInGuild(string line, string[] parts) : base(line, parts)
		{
			GuildName = parts.Length >= 2 ?  parts[1] : "";
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject player:
					if (!string.IsNullOrEmpty(GuildName))
						return !(player.MyGuild is null) && player.MyGuild.Name == GuildName;
					return player.MyGuild != null; 
			}
			return false;
		}
	}
}