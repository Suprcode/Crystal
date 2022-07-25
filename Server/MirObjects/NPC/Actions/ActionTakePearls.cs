namespace Server.MirObjects.Actions
{
	[ActionCommand("TAKEPEARLS")]
	public class ActionTakePearls : NPCAction
	{
		protected readonly uint Amount;
		public ActionTakePearls(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
					if (pearls >= player.Info.PearlCount)
						pearls = (uint) player.Info.PearlCount;
					player.IntelligentCreatureLosePearls((int) pearls);
					break;
			}
		}
	}
}