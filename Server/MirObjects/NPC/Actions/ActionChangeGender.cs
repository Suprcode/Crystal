namespace Server.MirObjects.Actions
{
	[ActionCommand("CHANGEGENDER")]
	public class ActionChangeGender : NPCAction
	{
		public ActionChangeGender(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					player.Info.Gender = player.Info.Gender == MirGender.Male ? MirGender.Female : MirGender.Male;
					break;
			}
		}
	}
}