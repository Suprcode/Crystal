using System.Drawing;
using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("INSTANCEMOVE")]
	public class ActionInstanceMove : NPCAction
	{
		protected readonly string MapName;
		protected Map Map;
		protected readonly int InstanceId, X, Y;
		public ActionInstanceMove(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[2], out InstanceId) ||
				!int.TryParse(parts[3], out X) ||
				!int.TryParse(parts[4], out Y))
				return;
			MapName = parts[1];
			Map = Envir.GetMapByNameAndInstance(MapName, InstanceId);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess)
				return;
			Map ??= Envir.GetMapByNameAndInstance(MapName, InstanceId);
			if (Map is null)
				return;
			switch (ob)
			{
				case PlayerObject player:
					player.Teleport(Map, new Point(X, Y));
					break;
			}
		}
	}
}