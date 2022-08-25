using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("ROLLYUT")]
	public class ActionRollYut : NPCAction
	{
		protected readonly bool AutoRoll;
		protected readonly string PageName;
		public ActionRollYut(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			PageName = parts[1];
			if (!bool.TryParse(parts[2], out AutoRoll))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					var result = Random.Next(1, 7);
					var p = new Roll
					{
						Type = 0,
						Page = PageName,
						AutoRoll = AutoRoll,
						Result = result
					};
					player.NPCData["NPCRollResult"] = result;
					player.Enqueue(p);
					break;
			}
		}
	}
}