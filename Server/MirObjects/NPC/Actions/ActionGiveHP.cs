namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVEHP")]
	public class ActionGiveHP : NPCAction
	{
		protected readonly int Amount;
		public ActionGiveHP(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
					player.ChangeHP(Amount);
					break;
			}
		}
	}
}