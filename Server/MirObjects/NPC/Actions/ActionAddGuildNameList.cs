using System.IO;
using System.Linq;
namespace Server.MirObjects.Actions
{
	[ActionCommand("ADDGUILDNAMELIST")]
	public class ActionAddGuildNameList : NPCAction
	{
		protected readonly string FileName;
		public ActionAddGuildNameList(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var quoteMatch = QuoteRegex.Match(line);
			var listPath = parts[1];
			if (quoteMatch.Success)
				listPath = quoteMatch.Groups[1].Captures[0].Value;
			FileName = Path.Combine(Settings.NameListPath, listPath);
			var sDirectory = Path.GetDirectoryName(FileName);
			if (!Directory.Exists(sDirectory))
				Directory.CreateDirectory(sDirectory);
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject playerObject:
					if (playerObject.MyGuild is null) return;
					if (File.ReadAllLines(FileName).All(t => playerObject.MyGuild.Name != t))
					{
						using var writer = File.AppendText(FileName);
						writer.WriteLine(playerObject.MyGuild.Name);
					}
					break;
			}
		}
	}
}