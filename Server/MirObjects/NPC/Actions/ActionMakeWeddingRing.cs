namespace Server.MirObjects.Actions
{
	[ActionCommand("MAKEWEDDINGRING")]
	public class ActionMakeWeddingRing : NPCAction
	{
		public ActionMakeWeddingRing(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					player.MakeWeddingRing();
					break;
			}
		}
	}
}