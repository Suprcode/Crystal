namespace Server.MirObjects.Checks
{
	[CheckCommand("ISADMIN")]
	public class CheckIsAdmin : NPCCheck
	{
		public CheckIsAdmin(string line, string[] parts) : base(line, parts)
		{
		}
		public override bool Check(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject playerObject:
					return playerObject.IsGM;
			}
			return false;
		}
	}
}