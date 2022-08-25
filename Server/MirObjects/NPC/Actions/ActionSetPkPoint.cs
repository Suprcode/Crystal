namespace Server.MirObjects.Actions
{
	[ActionCommand("SETPKPOINT")]
	public class ActionSetPkPoint : NPCAction
	{
		protected readonly int PkPoints;
		public ActionSetPkPoint(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out PkPoints))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.PKPoints = PkPoints;
					break;
			}
		}
	}
}