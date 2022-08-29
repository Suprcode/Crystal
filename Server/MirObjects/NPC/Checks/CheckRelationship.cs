namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKRELATIONSHIP")]
	public class CheckRelationship : NPCCheck
	{
		public CheckRelationship(string line, string[] parts) : base(line, parts)
		{
		}
		public override bool Check(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player: return player.Info.Married != 0;
			}
			return false;
		}
	}
}