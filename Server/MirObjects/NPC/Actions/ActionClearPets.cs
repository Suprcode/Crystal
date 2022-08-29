namespace Server.MirObjects.Actions
{
	[ActionCommand("CLEARPETS")]
	public class ActionClearPets : NPCAction
	{
		public ActionClearPets(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					for (int i = player.Pets.Count - 1; i >= 0; i--)
					{
						player.Pets[i].DieNextTurn = true;
					}
					break;
					
			}
		}
	}
}