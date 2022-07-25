namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVEGOLD")]
	public class ActionGiveGold : NPCAction
	{
		protected readonly uint Amount;
		public ActionGiveGold(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
					if (gold + player.Account.Gold >= uint.MaxValue)
						gold = uint.MaxValue - player.Account.Gold;
					player.GainGold(gold);
					break;
			}
		}
	}
}