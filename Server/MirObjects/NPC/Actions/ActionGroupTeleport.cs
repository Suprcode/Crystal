using System.Drawing;
using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("GROUPTELEPORT")]
	public class ActionGroupTeleport : NPCAction
	{
		protected readonly int InstanceId;
		protected readonly int X;
		protected readonly int Y;
		protected readonly string MapName;
		protected Map Map;
		
		public ActionGroupTeleport(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			MapName = parts[1];
			if (parts.Length == 4)
			{
				if (!int.TryParse(parts[2], out X) ||
					!int.TryParse(parts[3], out Y))
					return;
				InstanceId = 1;
			}
			else
			{
				if (parts.Length < 3)
					InstanceId = 1;
				else if (!int.TryParse(parts[2], out InstanceId) ||
					!int.TryParse(parts[3], out X) ||
					!int.TryParse(parts[4], out Y))
					return;
			}
			Map = Envir.GetMapByNameAndInstance(MapName, InstanceId);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Map ??= Envir.GetMapByNameAndInstance(MapName, InstanceId);
			if (Map is null) return;
			switch (ob)
			{
				case PlayerObject player:
					if (player.GroupMembers == null)
						return;
					for (var i = 0; i < player.GroupMembers.Count; i++)
					{
						if (X == 0 || Y == 0)
							player.GroupMembers[i].TeleportRandom(200, 0, Map);
						else
							player.GroupMembers[i].Teleport(Map, new Point(X, Y));
					}
					break;
			}
		}
	}
}