using System.Linq;
namespace Server.MirObjects.Actions
{
	[ActionCommand("BREAKTIMERECALL")]
	public class ActionBreakTimeRecall : NPCAction
	{
		public ActionBreakTimeRecall(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					foreach (var delayedAction in player.ActionList.Where(action => action.Type == DelayedType.NPC))
					{
						delayedAction.FlaggedToRemove = true;
					}
					break;
			}
		}
	}
}