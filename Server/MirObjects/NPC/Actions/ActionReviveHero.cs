namespace Server.MirObjects.Actions
{
	[ActionCommand("REVIVEHERO")]
	public class ActionReviveHero : NPCAction
	{
		public ActionReviveHero(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					player.ReviveHero();
					break;
			}
		}
	}
}