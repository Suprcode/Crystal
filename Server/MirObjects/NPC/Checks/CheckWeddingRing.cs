namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKWEDDINGRING")]
	public class CheckWeddingRing : NPCCheck
	{
		public CheckWeddingRing(string line, string[] parts) : base(line, parts)
		{
		}
		public override bool Check(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player: return player.CheckMakeWeddingRing();
			}
			return false;
		}
	}
}