namespace Server.MirObjects.Actions
{
	[ActionCommand("CANGAINEXP")]
	public class ActionCanGainExp : NPCAction
	{
		protected readonly bool CanGain;
		public ActionCanGainExp(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!bool.TryParse(parts[1], out CanGain))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.CanGainExp = CanGain;
					break;
			}
		}
	}
}