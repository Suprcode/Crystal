using System.Text.RegularExpressions;
namespace Server.MirObjects.Actions
{
	[ActionCommand("MOV")]
	public class ActionMov : NPCAction
	{
		protected readonly string Key;
		protected readonly string Value;
		public ActionMov(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			Key = parts[1];
			var match = Regex.Match(parts[1], @"[A-Z][0-9]", RegexOptions.IgnoreCase);
			var quoteMatch = QuoteRegex.Match(line);
			Value = parts[2];
			if (quoteMatch.Success)
				Value = quoteMatch.Groups[1].Captures[0].Value;
			InitializationSuccess = match.Success;
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					AddVariable(player, Key, Value);
					break;
			}
		}
	}
}