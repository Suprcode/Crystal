namespace Server.MirObjects.Actions
{
	[ActionCommand("DELAYGOTO")]
	public class ActionDelayGoto : NPCAction
	{
		protected readonly string PageName;
		protected readonly long Delay;
		public ActionDelayGoto(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!long.TryParse(parts[1], out Delay))
				return;
			InitializationSuccess = true;
			PageName = parts[2];
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					var action = new DelayedAction(DelayedType.NPC, Envir.Time + (Delay * 1000), player.NPCObjectID, player.NPCScriptID, $"[{PageName}]");
					player.ActionList.Add(action);
					break;
			}
		}
	}
}