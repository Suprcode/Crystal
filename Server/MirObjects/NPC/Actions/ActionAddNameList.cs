using System.IO;
using System.Linq;
namespace Server.MirObjects.Actions
{
	[ActionCommand("ADDNAMELIST")]
	public class ActionAddNameList : NPCAction
	{
		protected readonly string FileName;
		public ActionAddNameList(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var quoteMatch = QuoteRegex.Match(line);
			var listPath = parts[1];
			if (quoteMatch.Success)
				listPath = quoteMatch.Groups[1].Captures[0].Value;
			FileName = Path.Combine(Settings.NameListPath, listPath);
			var sDirectory = Path.GetDirectoryName(FileName);
			if (!Directory.Exists(sDirectory))
				Directory.CreateDirectory(sDirectory);
			if (!File.Exists(FileName))
				File.Create(FileName).Close();
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					if (File.ReadAllLines(FileName).All(t => player.Name != t))
					{
						using var writer = File.AppendText(FileName);
						writer.WriteLine(player.Name);
					}
					break;
			}
		}
	}
}