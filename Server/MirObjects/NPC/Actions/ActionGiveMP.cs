namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVEMP")]
	public class ActionGiveMP : NPCAction
	{
		protected readonly int Amount;
		public ActionGiveMP(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out Amount))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.ChangeMP(Amount);
					break;
			}
		}
	}
}