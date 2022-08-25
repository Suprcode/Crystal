namespace Server.MirObjects.Actions
{
	[ActionCommand("PARAM3")]
	public class ActionParam3 : NPCAction
	{
		protected readonly int Y;
		public ActionParam3(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out Y))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Segment.Param3 = Y;
		}
	}
}