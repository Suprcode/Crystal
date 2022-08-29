namespace Server.MirObjects.Actions
{
	[ActionCommand("FORCEDIVORCE")]
	public class ActionForceDivorce : NPCAction
	{
		public ActionForceDivorce(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					player.NPCDivorce();
					break;
			}
		}
	}
}