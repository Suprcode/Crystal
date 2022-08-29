using System.Drawing;
using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("MOVE")]
	public class ActionMove : NPCAction
	{
		protected readonly int X, Y;
		protected readonly string MapName;
		protected Map Map;
		public ActionMove(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (parts.Length > 3 &&
				(!int.TryParse(parts[2], out X) ||
				 !int.TryParse(parts[3], out Y)))
			{
				X = 0;
				Y = 0;
			}
			MapName = parts[1];
			Map = Envir.GetMapByNameAndInstance(MapName);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Map ??= Envir.GetMapByNameAndInstance(MapName);
			if (Map is null)
			{
				InitializationSuccess = false;
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					var coords = new Point(X, Y);
					if (coords.X == 0 && coords.Y == 0)
						player.TeleportRandom(200, 0, Map);
					else
						player.Teleport(Map, coords);
					break;
			}
		}
	}
}