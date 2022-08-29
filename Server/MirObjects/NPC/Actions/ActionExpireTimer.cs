namespace Server.MirObjects.Actions
{
	[ActionCommand("EXPIRETIMER")]
	public class ActionExpireTimer : NPCAction
	{
		protected readonly string Key;
		public ActionExpireTimer(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			Key = parts[1];
		}
		public override void Execute(MapObject ob)
		{
			if (Envir.Timers.ContainsKey($"_{Key}"))
			{
				Envir.Timers.Remove($"_{Key}");
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					player.ExpireTimer(Key);
					break;
			}
		}
	}
}