namespace Server.MirObjects.Actions
{
	[ActionCommand("ADDTOGUILD")]
	public class ActionAddToGuild : NPCAction
	{
		protected readonly string GuildName;
		protected GuildObject Guild;
		
		public ActionAddToGuild(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			GuildName = parts[1];
			Guild = Envir.GetGuild(GuildName);
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Guild ??= Envir.GetGuild(GuildName);
			if (Guild is null)
			{
				InitializationSuccess = false;
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					if (!(player.MyGuild is null)) return;
					player.PendingGuildInvite = Guild;
					player.GuildInvite(true);
					break;
			}
		}
	}
}