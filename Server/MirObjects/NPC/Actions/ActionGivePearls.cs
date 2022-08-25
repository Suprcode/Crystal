namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVEPEARLS")]
	public class ActionGivePearls : NPCAction
	{
		protected readonly uint Amount;
		public ActionGivePearls(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
					var pearls = Amount;
					if (pearls + player.Info.PearlCount > int.MaxValue)
						pearls = (uint)(int.MaxValue - player.Info.PearlCount);
					player.IntelligentCreatureGainPearls((int) pearls);
					break;
			}
		}
	}
}