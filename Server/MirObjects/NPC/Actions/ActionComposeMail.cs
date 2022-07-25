using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("COMPOSEMAIL")]
	public class ActionComposeMail : NPCAction
	{
		protected readonly string Message;
		protected readonly string Sender;
		public MailInfo MailInfo { get; protected set; }
		public ActionComposeMail(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var match = QuoteRegex.Match(line);
			if (!match.Success) return;
			Message = match.Groups[1].Captures[0].Value;
			Sender = parts[parts.Length - 1];
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					MailInfo = new MailInfo(player.Info.Index)
					{
						Sender = Sender,
						Message = Message
					};
					break;
			}
			
		}
	}
}