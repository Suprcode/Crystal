namespace Server.MirObjects.Actions
{
	[ActionCommand("PARAM2")]
	public class ActionParam2 : NPCAction
	{
		protected readonly int X;
		public ActionParam2(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out X))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Segment.Param2 = X;
		}
	}
}