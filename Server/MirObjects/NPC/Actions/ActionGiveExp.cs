namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVEEXP")]
	public class ActionGiveExp : NPCAction
	{
		protected readonly uint Amount;
		public ActionGiveExp(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			InitializationSuccess = uint.TryParse(parts[1], out Amount);
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.GainExp(Amount);
					break;
			}
		}
	}
}