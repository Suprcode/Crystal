namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVECREDIT")]
	public class ActionGiveCredit : NPCAction
	{
		protected readonly uint Amount;

		public ActionGiveCredit(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
					var credit = Amount;
					if (credit + player.Account.Credit >= uint.MaxValue)
						credit = uint.MaxValue - player.Account.Credit;
					player.GainCredit(credit);
					break;
			}
		}
	}
}