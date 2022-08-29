using System;
namespace Server.MirObjects.Actions
{
	[ActionCommand("INCREASEPKPOINT")]
	public class ActionIncreasePkPoint : NPCAction
	{
		protected readonly int Amount;
		public ActionIncreasePkPoint(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out Amount))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.PKPoints = Math.Min(player.PKPoints + Amount, int.MaxValue);
					break;
			}
		}
	}
}