using System.IO;
using System.Text.RegularExpressions;
namespace Server.MirObjects.Actions
{
	[ActionCommand("GETRANDOMTEXT")]
	public class ActionGetRandomText : NPCAction
	{
		protected readonly string FileName;
		protected readonly string Key;
		public ActionGetRandomText(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var match = Regex.Match(parts[2], @"[A-Z][0-9]", RegexOptions.IgnoreCase);
			InitializationSuccess = match.Success;
			if (!InitializationSuccess) return;
			FileName = parts[1];
			Key = parts[2];
			
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					var randomTextPath = Path.Combine(Settings.NPCPath, FileName);
					if (!File.Exists(randomTextPath))
					{
						MessageQueue.Instance.Enqueue($"The Random Text File: {randomTextPath} does not exist.");
						return;
					}
					var lines = File.ReadAllLines(randomTextPath);
					var index = Random.Next(0, lines.Length);
					var randomText = lines[index];
					AddVariable(player, Key, randomText);
					break;
			}
		}
	}
}