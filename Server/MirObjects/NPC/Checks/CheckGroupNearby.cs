using System.Drawing;
namespace Server.MirObjects.Checks
{
	[CheckCommand("GROUPCHECKNEARBY")]
	public class CheckGroupNearby : NPCCheck
	{
		public CheckGroupNearby(string line, string[] parts) : base(line, parts)
		{
		}
		public override bool Check(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					if (player.GroupMembers is null)
						return false;
					var target = new Point(-1, -1);
					for (var i = 0; i < player.CurrentMap.NPCs.Count; i++)
					{
						var npc = player.CurrentMap.NPCs[i];
						if (npc.ObjectID != player.NPCObjectID) continue;
						target = npc.CurrentLocation;
						break;
					}
					if (target.X == - 1 || target.Y == -1) return false;
					var success = false;
					for (var i = 0; i < player.GroupMembers.Count; i++)
					{
						if (player.GroupMembers[i] is null) continue;
						success = Functions.InRange(player.GroupMembers[i].CurrentLocation, target, 9);
						if (!success)
							break;
					}
					return success;
			}
			return false;
		}
	}
}