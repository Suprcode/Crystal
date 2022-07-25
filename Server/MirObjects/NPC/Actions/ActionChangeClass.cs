using System;
namespace Server.MirObjects.Actions
{
	[ActionCommand("CHANGECLASS")]
	public class ActionChangeClass : NPCAction
	{
		protected readonly MirClass Class;
		public ActionChangeClass(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!Enum.TryParse(parts[1], true, out Class))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.Info.Class = Class;
					break;
			}
		}
	}
}