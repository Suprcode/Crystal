namespace Server.MirObjects.Actions
{
	[ActionCommand("GOTO")]
	public class ActionGoto : NPCAction
	{
		protected readonly string PageName;
		public ActionGoto(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			PageName = parts[1];
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					var action = new DelayedAction(DelayedType.NPC,
					                               Envir.Time, player.NPCObjectID, player.NPCScriptID, $"[{PageName}]");
					player.ActionList.Add(action);
					break;
			}
		}
	}
}