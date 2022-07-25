using System;
namespace Server.MirObjects.Actions
{
	[ActionCommand("LOCALMESSAGE")]
	public class ActionLocalMessage : NPCAction
	{
		protected readonly string Message;
		protected readonly ChatType Type;
		public ActionLocalMessage(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var match = QuoteRegex.Match(line);
			if (match.Success)
			{
				Message = match.Groups[1].Captures[0].Value;
				if (!Enum.TryParse(parts[parts.Length - 1], true, out Type))
					return;
				InitializationSuccess = true;
			}
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.ReceiveChat(Message, Type);
					break;
			}
		}
	}
}