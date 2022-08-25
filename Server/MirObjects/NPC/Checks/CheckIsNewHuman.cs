namespace Server.MirObjects.Checks
{
	[CheckCommand("ISNEWHUMAN")]
	public class CheckIsNewHuman : NPCCheck
	{
		public CheckIsNewHuman(string line, string[] parts) : base(line, parts)
		{
			
		}
		public override bool Check(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player: return player.Info.AccountInfo.Characters.Count == 1;
			}
			return false;
		}
	}
}