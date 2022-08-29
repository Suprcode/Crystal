using System;
namespace Server.MirObjects.Actions
{
	[ActionCommand("REMOVEBUFF")]
	public class ActionRemoveBuff : NPCAction
	{
		protected readonly BuffType BuffType;
		public ActionRemoveBuff(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!Enum.TryParse(parts[1], out BuffType))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.RemoveBuff(BuffType);
					break;
			}
		}
	}
}