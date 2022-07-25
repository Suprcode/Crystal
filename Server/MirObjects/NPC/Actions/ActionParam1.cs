namespace Server.MirObjects.Actions
{
	[ActionCommand("PARAM1")]
	public class ActionParam1 : NPCAction
	{
		protected readonly string MapName;
		protected readonly int InstanceId;
		
		public ActionParam1(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (parts.Length < 3)
				InstanceId = 1;
			else if (!int.TryParse(parts[2], out InstanceId))
				return;
			MapName = parts[1];
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			Segment.Param1 = MapName;
			Segment.Param1Instance = InstanceId;
		}
	}
}