using System;
using System.Text.RegularExpressions;
namespace Server.MirObjects.Actions
{
	[ActionCommand("CALC")]
	public class ActionCalc : NPCAction
	{
		protected readonly int LeftNumberValue = -1, RightNumberValue = -1;
		protected readonly string LeftStringValue, RightStringValue;
		protected readonly string Operator;
		protected readonly string Key;
		protected readonly string Value;
		public ActionCalc(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var match = Regex.Match(parts[1], @"[A-Z][0-9]", RegexOptions.IgnoreCase);
			var quoteMatch = QuoteRegex.Match(line);
			Value = parts[3];
			if (quoteMatch.Success)
				Value = quoteMatch.Groups[1].Captures[0].Value;
			Key = parts[1].Insert(1, "-");
			if (!int.TryParse($"%{parts[1]}", out LeftNumberValue) ||
				!int.TryParse(Value, out RightNumberValue))
			{
				LeftStringValue = parts[1];
				RightStringValue = Value;
			}
			Operator = parts[2];
			InitializationSuccess = match.Success;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			
			switch (ob)
			{
				case PlayerObject player:
				{
					if (string.IsNullOrEmpty(LeftStringValue + RightStringValue))
					{
						try
						{
							var result = Calculate(Operator, LeftNumberValue, RightNumberValue);
							AddVariable(player, Key.Replace("-", ""), result.ToString());
						}
						catch (ArgumentException)
						{
							MessageQueue.Enqueue($"Incorrect operator: {Operator}, Page: {Page.Key}");
						}
					}
					else
						AddVariable(player, Key.Replace("-", ""), LeftStringValue + RightStringValue);
				}
					break;
			}
		}
	}
}