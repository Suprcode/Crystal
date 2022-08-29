using System;
namespace Server.MirObjects.Actions
{
	[ActionCommand("REDUCEPKPOINT")]
	public class ActionReducePkPoint : NPCAction
	{
		protected readonly int Amount;
		public ActionReducePkPoint(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
					player.PKPoints = Math.Max(player.PKPoints - Amount, 0);
					break;
			}
		}
	}
}