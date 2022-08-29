namespace Server.MirObjects.Actions
{
	[ActionCommand("BREAK")]
	public class ActionBreak : NPCAction
	{
		public ActionBreak(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			Page.BreakFromSegments = true;
		}
	}
}