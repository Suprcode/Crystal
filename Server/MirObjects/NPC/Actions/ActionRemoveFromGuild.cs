namespace Server.MirObjects.Actions
{
	[ActionCommand("REMOVEFROMGUILD")]
	public class ActionRemoveFromGuild : NPCAction
	{
		protected readonly string GuildName;
		protected GuildObject Guild;
		public ActionRemoveFromGuild(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			GuildName = parts[1];
			Guild = Envir.GetGuild(GuildName);
			InitializationSuccess = true;
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
					if (player.MyGuild is null) return;
					if (player.MyGuildRank is null) return;
					player.MyGuild.DeleteMember(player, player.Name);
					break;
			}
		}
	}
}