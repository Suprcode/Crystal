namespace Server.MirObjects.Actions
{
	[ActionCommand("TIMERECALL")]
	public class ActionTimeRecall : NPCAction
	{
		protected readonly string PageName;
		protected readonly long Delay;
		public ActionTimeRecall(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!long.TryParse(parts[1], out Delay))
				return;
			PageName = parts.Length > 2 ? $"[{parts[2]}]" : "";
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					var tempMap = player.CurrentMap;
					var tempPoint = player.CurrentLocation;
					var action = new DelayedAction(DelayedType.NPC,
					                               Envir.Time + (Delay * 1000), player.NPCObjectID, player.NPCScriptID, PageName, tempMap, tempPoint);
					player.ActionList.Add(action);
					break;
			}
		}
	}
}