namespace Server.MirObjects.Checks
{
	[CheckCommand("GROUPLEADER")]
	public class CheckGroupLeader : NPCCheck
	{
		public CheckGroupLeader(string line, string[] parts) : base(line, parts)
		{
		}
		public override bool Check(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					return !(player.GroupMembers is null) && player.GroupMembers[0] == player; 
			}
			return false;
		}
	}
}