namespace Server.MirObjects.Actions
{
	[ActionCommand("TIMERECALLGROUP")]
	public class ActionTimeRecallGroup : NPCAction
	{
		protected readonly string PageName;
		protected readonly long Delay;
		public ActionTimeRecallGroup(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
					for (int i = 0; i < player.GroupMembers.Count; i++)
					{
						var member = player.GroupMembers[i];
						var action = new DelayedAction(DelayedType.NPC,
						                               Envir.Time + (Delay * 1000),
						                               player.NPCObjectID, player.NPCScriptID, PageName, player.CurrentMap, player.CurrentLocation);
						member.ActionList.Add(action);
					}
					break;
			}
		}
	}
}