using System;
using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("GLOBALMESSAGE")]
	public class ActionGlobalMessage : NPCAction
	{
		protected readonly string Message;
		protected readonly ChatType Type;
		public ActionGlobalMessage(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var match = QuoteRegex.Match(line);
			if (match.Success)
			{
				Message = match.Groups[1].Captures[0].Value;
				if (!Enum.TryParse(parts[parts.Length - 1], out Type))
					return;
				InitializationSuccess = true;
			}
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Envir.Broadcast(new Chat
			{
				Message = Message,
				Type = Type
			});
		}
	}
}