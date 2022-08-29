using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("OPENBROWSER")]
	public class ActionOpenBrowser : NPCAction
	{
		protected readonly string Url;
		public ActionOpenBrowser(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			Url = parts[1];
			InitializationSuccess = !string.IsNullOrWhiteSpace(Url);
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					player.Enqueue(new OpenBrowser
					{
						Url = Url
					});
					break;
			}
		}
	}
}