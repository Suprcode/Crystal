namespace Server.MirObjects.Actions
{
	[ActionCommand("GROUPRECALL")]
	public class ActionGroupRecall : NPCAction
	{
		public ActionGroupRecall(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					if (player.GroupMembers is null) return;
					for (var i = 0; i < player.GroupMembers.Count; i++)
					{
						player.GroupMembers[i].Teleport(player.CurrentMap, player.CurrentLocation);
					}
					break;
			}
		}
	}
}