using System.IO;
namespace Server.MirObjects.Actions
{
	[ActionCommand("CALL")]
	public class ActionCall : NPCAction
	{
		protected readonly int ScriptId;
		public ActionCall(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var quoteMatch = QuoteRegex.Match(line);
			var listPath = parts[1];
			if (quoteMatch.Success)
			{
				listPath = quoteMatch.Groups[1].Captures[0].Value;
			}
			var fileName = Path.Combine(Settings.NPCPath, listPath + ".txt");
			if (!File.Exists(fileName)) return;
			var script = NPCScript.GetOrAdd(0, listPath, NPCScriptType.Called);
			if (script is null)
				return;
			ScriptId = script.ScriptID;
			segment.Page.ScriptCalls.Add(script.ScriptID);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					var action = new DelayedAction(DelayedType.NPC,
					                               -1, player.NPCObjectID, ScriptId, $"[@MAIN]");
					player.ActionList.Add(action);
					break;
			}
		}
	}
}