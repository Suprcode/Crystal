namespace Server.MirObjects.Actions
{
	[ActionCommand("CHANGELEVEL")]
	public class ActionChangeLevel : NPCAction
	{
		protected readonly ushort Level;
		public ActionChangeLevel(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!ushort.TryParse(parts[1], out Level))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.Level = Level;
					player.Experience = 0;
					player.LevelUp();
					break;
			}
		}
	}
}