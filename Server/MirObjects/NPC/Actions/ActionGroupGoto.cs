namespace Server.MirObjects.Actions
{
	[ActionCommand("GROUPGOTO")]
	public class ActionGroupGoto : NPCAction
	{
		protected readonly string PageName;
		public ActionGroupGoto(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			PageName = parts[1];
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					if (player.GroupMembers is null) return;
					var delay = new DelayedAction(DelayedType.NPC, Envir.Time, player.NPCObjectID, player.NPCScriptID, $"[{PageName}]");
					for (int i = 0; i < player.GroupMembers.Count; i++)
					{
						player.GroupMembers[i].ActionList.Add(delay);
					}
					break;
			}
		}
	}
}