namespace Server.MirObjects.Actions
{
	[ActionCommand("SEALHERO")]
	public class ActionSealHero : NPCAction
	{
		public ActionSealHero(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					player.SealHero();
					break;
			}
		}
	}
}