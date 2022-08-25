using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("TAKECREDIT")]
	public class ActionTakeCredit : NPCAction
	{
		protected readonly uint Amount;
		public ActionTakeCredit(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
					if (credit >= player.Account.Credit) credit = player.Account.Credit;
					player.Account.Credit -= credit;
					player.Enqueue(new LoseCredit
					{
						Credit = credit
					});
					break;
			}
		}
	}
}