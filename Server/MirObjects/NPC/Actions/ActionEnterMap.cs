using System.Drawing;
using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("ENTERMAP")]
	public class ActionEnterMap : NPCAction
	{
		public ActionEnterMap(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					if (!player.NPCData.TryGetValue("NPCMoveMap", out var npcMoveMap) ||
						!player.NPCData.TryGetValue("NPCMoveCoord", out var npcMoveCoord)) return;
					player.Teleport((Map)npcMoveMap, (Point)npcMoveCoord);
					player.NPCData.Remove("NPCMoveMap");
					player.NPCData.Remove("NPCMoveCoord");
					break;
			}	
		}
	}
}