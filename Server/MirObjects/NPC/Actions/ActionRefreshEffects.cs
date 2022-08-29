using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("REFRESHEFFECTS")]
	public class ActionRefreshEffects : NPCAction
	{
		public ActionRefreshEffects(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					player.SetLevelEffects();
					var p = new ObjectLevelEffects
					{
						ObjectID = player.ObjectID,
						LevelEffects = player.LevelEffects
					};
					player.Enqueue(p);
					player.Broadcast(p);
					break;
			}
		}
	}
}