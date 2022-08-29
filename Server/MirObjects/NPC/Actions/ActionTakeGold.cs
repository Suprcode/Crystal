using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("TAKEGOLD")]
	public class ActionTakeGold : NPCAction
	{
		protected readonly uint Amount;
		public ActionTakeGold(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!uint.TryParse(parts[1], out Amount))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					var gold = Amount;
					if (gold >= player.Account.Gold)
						gold = player.Account.Gold;
					player.Account.Gold -= gold;
					player.Enqueue(new LoseGold { Gold = gold });
					break;
			}
		}
	}
}